/*
 * Author : Mohamed Idhris Sheik Dawood
 * Date created : 06-06-2017
 * Last modified : 17-06-2017 
 * * */

using System;
using System.Data;
using InsproDataAccess;
using System.Web.UI.WebControls;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;
using System.Collections;
using System.Drawing;
using System.Text;
using System.Globalization;

public partial class AttendanceMOD_NewStaffAttendance : System.Web.UI.Page
{
    string grouporusercode = string.Empty;
    string collegeCode = string.Empty;
    string userCode = string.Empty;
    string staffCode = string.Empty;
    GetStudentData studinfo = new GetStudentData();

    InsproDirectAccess dirAcc = new InsproDirectAccess();
    InsproStoreAccess storeAcc = new InsproStoreAccess();
    DAccess2 da = new DAccess2();

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            #region College, User and group user code check

            if (Session["collegecode"] == null)
            {
                Response.Redirect("~/Default.aspx");
            }
            collegeCode = Session["collegecode"].ToString();
            userCode = Session["usercode"].ToString();
            if ((Session["group_code"].ToString().Trim() != "") && (Session["group_code"].ToString().Trim() != "0") && (Session["group_code"].ToString().Trim() != "-1"))
            {
                grouporusercode = " group_code=" + Session["group_code"].ToString().Trim() + "";
            }
            else
            {
                grouporusercode = " usercode=" + Session["usercode"].ToString().Trim() + "";
            }
            if (Convert.ToString(Session["StafforAdmin"]).Trim().ToLower() == "admin")
            {
                Response.Redirect("~/Default.aspx");
            }
            else
            {
                staffCode = Convert.ToString(Session["Staff_Code"]).Trim();
            }

            #endregion

            if (!IsPostBack)
            {
                txtFromDate.Attributes.Add("readonly", "readonly");
                txtFromDate.Text = DateTime.Now.ToString("dd/MM/yyyy");

                txtToDate.Attributes.Add("readonly", "readonly");
                txtToDate.Text = DateTime.Now.ToString("dd/MM/yyyy");
                loadreason();
                btnGo_OnClick(new object(), new EventArgs());
            }
        }
        catch { }
    }

    //Validate from and to date
    protected void CheckDate(object sender, EventArgs e)
    {
        try
        {
            clearAttMarkDet();
            divTimeTable.Visible = false;
            string fromDateTime = txtFromDate.Text.Split('/')[1] + "/" + txtFromDate.Text.Split('/')[0] + "/" + txtFromDate.Text.Split('/')[2];
            string toDateTime = txtToDate.Text.Split('/')[1] + "/" + txtToDate.Text.Split('/')[0] + "/" + txtToDate.Text.Split('/')[2];
            DateTime dt = Convert.ToDateTime(fromDateTime);
            DateTime dt2 = Convert.ToDateTime(toDateTime);
            if (dt > dt2)
            {
                txtFromDate.Text = DateTime.Now.ToString("dd/MM/yyyy");
                txtToDate.Text = DateTime.Now.ToString("dd/MM/yyyy");

            }
        }
        catch { }
    }

    //Load staff time table
    protected void btnGo_OnClick(object sender, EventArgs e)
    {
        try
        {
            clearAttMarkDet();
            if (IsWeekDayOrder())
            {
                //Week day order
                loadGridTimeTableWeekOrder();
            }
            else
            {
                //Day order
                loadGridTimeTableDayOrder();
            }

        }
        catch { }
    }

    //Load staff time table for selected days week day order
    private void loadGridTimeTableWeekOrder()
    {
        try
        {
            divTimeTable.Visible = false;

            string fromDateTime = txtFromDate.Text.Split('/')[1] + "/" + txtFromDate.Text.Split('/')[0] + "/" + txtFromDate.Text.Split('/')[2];
            string toDateTime = txtToDate.Text.Split('/')[1] + "/" + txtToDate.Text.Split('/')[0] + "/" + txtToDate.Text.Split('/')[2];
            DateTime dtFrom = Convert.ToDateTime(fromDateTime);
            DateTime dtTo = Convert.ToDateTime(toDateTime);

            byte maxPeriod = getMaxPeriods();
            Dictionary<string, byte> dicDayOrder = getDayOrder();
            DataTable dtOvalStaffTT = getStaffTT();

            DataTable dtTTDates = dtOvalStaffTT.AsDataView().ToTable(true, "TT_date");

            if (dtTTDates.Rows.Count == 0)
            {
                return;
            }

            List<int> lstBatch = getStfAllotBatch();
            List<Int64> lstDeg = getStfAllotDegree();
            List<byte> lstSem = getStfAllotSem();
            string batchyear = string.Join(",", lstBatch);
            string degreecode = string.Join(",", lstDeg);
            string semester1 = string.Join(",", lstSem);

            string semStart = string.Empty;
            string semEnd = string.Empty;
            int startDayOrder = 1;
            bool seminfo = false;
            getSemStartEndDate(ref semStart, ref semEnd, ref startDayOrder, batchyear, degreecode, semester1, 1);

            DateTime semStartDt = Convert.ToDateTime(semStart);
            DateTime semEndDt = Convert.ToDateTime(semEnd);

            bool dateCheck = false;
            if (dtFrom >= semStartDt && dtFrom <= semEndDt && dtTo >= semStartDt && dtTo <= semEndDt)
                dateCheck = true;

            if (!dateCheck)
                return;

            DataTable HolidayDates = getHolidayDates(semStart, semEnd, degreecode);
            //List<string> holiDates = HolidayDates.AsEnumerable().Select(r => r.Field<string>("Hday")).ToList<string>();

            DataTable dtTTDisp = new DataTable();
            dtTTDisp.Columns.Add("DateDisp");
            dtTTDisp.Columns.Add("DateVal");
            dtTTDisp.Columns.Add("DayOrder");
            dtTTDisp.Columns.Add("DayVal");
            dtTTDisp.Columns.Add("Elective");
            dtTTDisp.Columns.Add("Lab");
            dtTTDisp.Columns.Add("P1ValDisp");
            dtTTDisp.Columns.Add("P1Val");
            dtTTDisp.Columns.Add("TT_1");
            dtTTDisp.Columns.Add("P2ValDisp");
            dtTTDisp.Columns.Add("P2Val");
            dtTTDisp.Columns.Add("TT_2");
            dtTTDisp.Columns.Add("P3ValDisp");
            dtTTDisp.Columns.Add("P3Val");
            dtTTDisp.Columns.Add("TT_3");
            dtTTDisp.Columns.Add("P4ValDisp");
            dtTTDisp.Columns.Add("P4Val");
            dtTTDisp.Columns.Add("TT_4");
            dtTTDisp.Columns.Add("P5ValDisp");
            dtTTDisp.Columns.Add("P5Val");
            dtTTDisp.Columns.Add("TT_5");
            dtTTDisp.Columns.Add("P6ValDisp");
            dtTTDisp.Columns.Add("P6Val");
            dtTTDisp.Columns.Add("TT_6");
            dtTTDisp.Columns.Add("P7ValDisp");
            dtTTDisp.Columns.Add("P7Val");
            dtTTDisp.Columns.Add("TT_7");
            dtTTDisp.Columns.Add("P8ValDisp");
            dtTTDisp.Columns.Add("P8Val");
            dtTTDisp.Columns.Add("TT_8");
            dtTTDisp.Columns.Add("P9ValDisp");
            dtTTDisp.Columns.Add("P9Val");
            dtTTDisp.Columns.Add("TT_9");
            dtTTDisp.Columns.Add("P10ValDisp");
            dtTTDisp.Columns.Add("P10Val");
            dtTTDisp.Columns.Add("TT_10");
            while (dtTo >= dtFrom)
            {
                string dayOfWeek = dtFrom.DayOfWeek.ToString();
                HolidayDates.DefaultView.RowFilter = "Hday='" + dtFrom.ToString("MM/dd/yyyy") + "'";
                DataTable dtCurHoli = HolidayDates.DefaultView.ToTable();

                if (dtCurHoli.Rows.Count > 0)
                {
                    DataRow drTTholi = dtTTDisp.NewRow();
                    drTTholi["DateDisp"] = dtFrom.ToString("dd/MM/yyyy") + "<br> (" + dtFrom.DayOfWeek + ")";
                    drTTholi["DateVal"] = dtFrom.ToString("MM/dd/yyyy");
                    drTTholi["DayOrder"] = dayOfWeek;
                    for (byte curPeriod = 1; curPeriod <= maxPeriod; curPeriod++)
                    {
                        drTTholi["P" + curPeriod + "ValDisp"] = Convert.ToString(dtCurHoli.Rows[0]["holiday_desc"]) + "##MISD";
                    }
                    dtTTDisp.Rows.Add(drTTholi);
                    dtFrom = dtFrom.AddDays(1);
                    continue;
                }
                string college_code = string.Empty;
                string degree_code = string.Empty;
                string semester = string.Empty;
                string frdate = string.Empty;
                bool ishoilday = false;
                bool isholimorn = false;
                bool isholieven = false;
                int fhrs = 0;
                //isholidayCheck(college_code, degree_code, semester, frdate, out  ishoilday, out  isholimorn, out  isholieven, out  fhrs);
                DataRow drTT = dtTTDisp.NewRow();
                drTT["DateDisp"] = dtFrom.ToString("dd/MM/yyyy") + "<br> (" + dtFrom.DayOfWeek + ")";
                drTT["DateVal"] = dtFrom.ToString("MM/dd/yyyy");
                drTT["DayOrder"] = dayOfWeek;

                dtTTDates.DefaultView.RowFilter = "TT_date<='" + dtFrom + "'";
                dtTTDates.DefaultView.Sort = "TT_date desc";
                DataTable dtCurDate = dtTTDates.DefaultView.ToTable();
                DateTime ttCurDate = new DateTime();
                if (dtCurDate.Rows.Count > 0)
                {
                    ttCurDate = Convert.ToDateTime(dtCurDate.Rows[0]["TT_date"]);
                }
                dtOvalStaffTT.DefaultView.RowFilter = "TT_date<='" + dtFrom + "'";
                dtOvalStaffTT.DefaultView.Sort = "TT_date desc";
                DataTable dtDistinctClassTT = dtOvalStaffTT.DefaultView.ToTable(true, "TT_colCode", "TT_batchyear", "TT_degCode", "TT_sem", "TT_sec");
                DataTable dtDistinctClassTTDet = dtOvalStaffTT.DefaultView.ToTable(true, "TT_colCode", "TT_batchyear", "TT_degCode", "TT_sem", "TT_sec", "TT_date", "TT_ClassPK");
                DataTable dtStaffTT = new DataTable();
                DataTable dtStaffTTCopy = new DataTable();
                int ttCount = 0;
                foreach (DataRow drClassTT in dtDistinctClassTT.Rows)
                {
                    dtDistinctClassTTDet.DefaultView.RowFilter = "TT_colCode='" + Convert.ToString(drClassTT["TT_colCode"]).Trim() + "' and TT_batchyear='" + Convert.ToString(drClassTT["TT_batchyear"]).Trim() + "' and TT_degCode='" + Convert.ToString(drClassTT["TT_degCode"]).Trim() + "' and TT_sem='" + Convert.ToString(drClassTT["TT_sem"]).Trim() + "' and TT_sec='" + Convert.ToString(drClassTT["TT_sec"]).Trim() + "'";
                    List<long> lstTTPks = new List<long>();
                    string ttPks = string.Empty;
                    DataTable dtTTPks = new DataTable();
                    dtDistinctClassTTDet.DefaultView.Sort = "TT_date desc";
                    dtTTPks = dtDistinctClassTTDet.DefaultView.ToTable(true, "TT_ClassPK");
                    //lstTTPks = dtDistinctClassTTDet.AsEnumerable().Select(r => r.Field<long>("TT_ClassPK")).ToList<long>();
                    //ttPks = string.Join(",", lstTTPks.ToArray());.
                    string LastDate = da.GetFunction(" select Convert(varchar(20),TT_date,103) TT_date from TT_ClassTimetable where TT_colCode='" + Convert.ToString(drClassTT["TT_colCode"]).Trim() + "' and TT_batchyear='" + Convert.ToString(drClassTT["TT_batchyear"]).Trim() + "' and TT_degCode='" + Convert.ToString(drClassTT["TT_degCode"]).Trim() + "' and TT_sem='" + Convert.ToString(drClassTT["TT_sem"]).Trim() + "' and TT_sec='" + Convert.ToString(drClassTT["TT_sec"]).Trim() + "' and TT_date <= '" + dtFrom + "'  order by TT_date desc");
                    DateTime DtCheck = new DateTime();
                    DateTime DtNewCheck = new DateTime();
                    //DateTime.TryParse(LastDate, out DtCheck);
                    DateTime.TryParseExact(LastDate, "dd/MM/yyyy", null, DateTimeStyles.None, out DtCheck);

                    if (dtTTPks.Rows.Count > 0)//23/8/17
                    {
                        ttPks = Convert.ToString(dtTTPks.Rows[0]["TT_ClassPK"]).Trim();
                        dtOvalStaffTT.DefaultView.RowFilter = "TT_ClassPK ='" + ttPks + "'";
                        DataTable dtCopyTT = new DataTable();
                        dtOvalStaffTT.DefaultView.Sort = "TT_date desc";
                        dtCopyTT = dtOvalStaffTT.DefaultView.ToTable();
                        if (dtCopyTT.Rows.Count > 0)
                        {
                            string NewDate = Convert.ToString(dtCopyTT.Rows[0]["TTDate"]).Trim();
                            //DateTime.TryParse(NewDate, out DtNewCheck);
                            DateTime.TryParseExact(NewDate, "dd/MM/yyyy", null, DateTimeStyles.None, out DtNewCheck);
                            if (DtNewCheck >= DtCheck) // Added by jairam 05-09-2017
                            {
                                dtStaffTTCopy.Merge(dtCopyTT);
                                ttCount++;
                            }
                        }
                    }
                }

                #region Alter time table Data

                string ttPk = string.Empty;
                if (dtStaffTTCopy.Rows.Count > 0)
                {
                    ttPk = Convert.ToString(dtStaffTTCopy.Rows[0]["TT_ClassPk"]);
                    dtStaffTTCopy.DefaultView.RowFilter = string.Empty;
                    dtStaffTTCopy.DefaultView.Sort = "TT_colCode,TT_batchyear,TT_degCode,TT_sem,TT_sec,TT_date desc";
                    dtStaffTT = dtStaffTTCopy.DefaultView.ToTable(true);
                }
                bool isStfAlter = false;
                bool isOthStfAlter = false;
                DataTable dtStaffAlterTT = getStaffAlterTT(dtFrom.ToString("MM/dd/yyyy"), lstBatchYear: lstBatch, lstDegreeCode: lstDeg, lstSemester: lstSem);
                DataTable dtOtrStfAlterTT = getOtherStaffAlterTT(dtFrom.ToString("MM/dd/yyyy"), lstBatchYear: lstBatch, lstDegreeCode: lstDeg, lstSemester: lstSem);

                if (dtStaffAlterTT.Rows.Count > 0)
                {
                    isStfAlter = true;
                }
                if (dtOtrStfAlterTT.Rows.Count > 0)
                {
                    isOthStfAlter = true;
                }

                #endregion

                DataTable dtCurDayTT = new DataTable();
                if (dtStaffTT.Rows.Count > 0)
                {
                    dtStaffTT.DefaultView.RowFilter = "TT_Day='" + dicDayOrder[dayOfWeek] + "'";
                    dtCurDayTT = dtStaffTT.DefaultView.ToTable();
                }

                for (byte curPeriod = 1; curPeriod <= maxPeriod; curPeriod++)
                {
                    DataTable dtCurHourTT = new DataTable();
                    List<decimal> newlist = new List<decimal>();
                    //Check For Alter Schedule
                    bool alterSchd = false;
                    bool hasAlter = false;
                    bool current_hraltered = false;
                    DataTable dtHourTTDet = new DataTable();
                    if (dtStaffTT.Rows.Count > 0)
                    {
                        dtStaffTT.DefaultView.RowFilter = "TT_Day='" + dicDayOrder[dayOfWeek] + "' and TT_Hour='" + curPeriod + "' ";
                        dtHourTTDet = dtStaffTT.DefaultView.ToTable(true, "TT_batchyear", "TT_colCode", "TT_degCode", "TT_sem", "TT_sec");
                    }
                    DataTable dtHourAlter = new DataTable();
                    DataTable dtCurHourTTCopy = new DataTable();
                    string startDt1 = string.Empty;
                    string endDt1 = string.Empty;
                    int hourCount = 0;
                    if (dtHourTTDet.Rows.Count > 0)
                    {
                        List<int> batchYearList = dtHourTTDet.AsEnumerable().Select(r => r.Field<int>("TT_batchyear")).ToList<int>(); ;
                        List<Int64> degreeCodeList = dtHourTTDet.AsEnumerable().Select(r => r.Field<Int64>("TT_degCode")).ToList<Int64>();
                        List<byte> semesterList = dtHourTTDet.AsEnumerable().Select(r => r.Field<byte>("TT_sem")).ToList<byte>();
                        List<string> sectionsList = dtHourTTDet.AsEnumerable().Select(r => r.Field<string>("TT_sec")).ToList<string>();
                        foreach (DataRow drAlter in dtHourTTDet.Rows)
                        {
                            string batchHr = Convert.ToString(drAlter["TT_batchyear"]).Trim();
                            string collegeHr = Convert.ToString(drAlter["TT_colCode"]).Trim();
                            string degreeHr = Convert.ToString(drAlter["TT_degCode"]).Trim();
                            string semHr = Convert.ToString(drAlter["TT_sem"]).Trim();
                            string secHr = Convert.ToString(drAlter["TT_sec"]).Trim();
                            //Rajkumar 23/12/2017
                            DataTable dtStartEnd = dirAcc.selectDataTable("select convert(varchar(10),min(start_date),101) as semstart,convert(varchar(10),max(end_date),101) as semend,isnull(starting_dayorder,1) as dorder from seminfo where degree_code='" + degreeHr + "' and batch_year='" + batchHr + "' and semester='" + semHr + "'  group by starting_dayorder order by semstart desc,semend desc");
                            //DataTable dtEnd = dirAcc.selectDataTable("select convert(varchar(10),max(end_date),101) as semend,isnull(starting_dayorder,1) as dorder from seminfo where degree_code in (" + degree + ") and batch_year in (" + batch + ")  group by starting_dayorder order by semstart desc,semend desc");

                            if (dtStartEnd.Rows.Count > 0)
                            {
                                startDt1 = Convert.ToString(dtStartEnd.Rows[0]["semstart"]);
                                endDt1 = Convert.ToString(dtStartEnd.Rows[0]["semend"]);

                                //startDayOrder = Convert.ToInt32(dtStartEnd.Rows[0]["dorder"]);
                            }
                            //if (isStfAlter)
                            //{
                            //    dtStaffAlterTT.DefaultView.RowFilter = "TT_Hour='" + curPeriod + "'";
                            //    DataTable dtalterdata = dtStaffAlterTT.DefaultView.ToTable();
                            //    if (dtalterdata.Rows.Count > 0)
                            //    {
                            //        DataTable dtSeminfo = new DataTable();
                            //        if (dtalterdata.Rows.Count > 0)
                            //        {
                            //            string strbatch = Convert.ToString(dtalterdata.Rows[0]["TT_batchyear"]);
                            //            string strdeg = Convert.ToString(dtalterdata.Rows[0]["TT_degCode"]);
                            //            string strsem = Convert.ToString(dtalterdata.Rows[0]["TT_Sem"]);
                            //            dtSeminfo = dirAcc.selectDataTable("select convert(varchar(10),min(start_date),101) as semstart,convert(varchar(10),max(end_date),101) as semend,isnull(starting_dayorder,1) as dorder from seminfo where degree_code='" + strdeg + "' and batch_year='" + strbatch + "' and semester='" + strsem + "'  group by starting_dayorder order by semstart desc,semend desc");
                            //        }
                            //        if (dtSeminfo.Rows.Count > 0)
                            //        {
                            //            startDt1 = Convert.ToString(dtSeminfo.Rows[0]["semstart"]);
                            //            endDt1 = Convert.ToString(dtSeminfo.Rows[0]["semend"]);
                            //        }
                            //    }
                            //}
                            DateTime semStartDt1 = Convert.ToDateTime(startDt1);
                            DateTime semEndDt1 = Convert.ToDateTime(endDt1);
                          
                            if (dtFrom >= semStartDt1 && dtFrom <= semEndDt1 && dtTo >= semStartDt1 && dtTo <= semEndDt1)
                            {
                                seminfo = true;
                                hasAlter = false;
                                alterSchd = false;
                                if (isStfAlter) //somebody give the hour to logged staff
                                {
                                    if (isOthStfAlter)  //12sep2017
                                    {
                                        dtOtrStfAlterTT.DefaultView.RowFilter = "TT_Day='" + dicDayOrder[dayOfWeek] + "' and TT_Hour='" + curPeriod + "' and TT_AlterDate='" + dtFrom.ToString("MM/dd/yyyy") + "' and TT_batchyear='" + batchHr + "' and TT_colCode='" + collegeHr + "' and TT_degCode='" + degreeHr + "' and TT_sem='" + semHr + "' and TT_sec='" + secHr + "'";

                                        dtHourAlter = dtOtrStfAlterTT.DefaultView.ToTable();
                                        if (dtHourAlter.Rows.Count > 0)
                                        {
                                            current_hraltered = true;
                                        }
                                    }

                                    //Aruna 28/jul/2017===========================================================================                                
                                    dtStaffAlterTT.DefaultView.RowFilter = "TT_Day='" + dicDayOrder[dayOfWeek] + "' and TT_Hour='" + curPeriod + "' and TT_AlterDate='" + dtFrom.ToString("MM/dd/yyyy") + "'";
                                    //=============================================================================================
                                    //dtCurHourTT = dtStaffAlterTT.DefaultView.ToTable();
                                    dtHourAlter = dtStaffAlterTT.DefaultView.ToTable();
                                    if (dtHourAlter.Rows.Count > 0)
                                    {
                                        newlist = dtHourAlter.AsEnumerable().Select(r => r.Field<decimal>("TT_subno")).ToList<decimal>();//Added by jairam 04-09-2017
                                        alterSchd = true;
                                        hasAlter = true;
                                    }
                                }
                                if (isOthStfAlter && !alterSchd) //Logged Staff gave the hr to other staff
                                {
                                    //Aruna 28/jul/2017===========================================================================

                                    dtOtrStfAlterTT.DefaultView.RowFilter = "TT_Day='" + dicDayOrder[dayOfWeek] + "' and TT_Hour='" + curPeriod + "' and TT_AlterDate='" + dtFrom.ToString("MM/dd/yyyy") + "' and TT_batchyear='" + batchHr + "' and TT_colCode='" + collegeHr + "' and TT_degCode='" + degreeHr + "' and TT_sem='" + semHr + "' and TT_sec='" + secHr + "'";
                                    //=============================================================================================
                                    dtHourAlter = dtOtrStfAlterTT.DefaultView.ToTable();
                                    if (dtHourAlter.Rows.Count > 0)
                                    {
                                        continue;
                                    }
                                }

                                if (dtHourAlter.Rows.Count == 0) //Default Time table if no alter available
                                {
                                    dtCurDayTT.DefaultView.RowFilter = "TT_Day='" + dicDayOrder[dayOfWeek] + "' and TT_Hour='" + curPeriod + "' and (TT_batchyear='" + batchHr + "' and TT_colCode='" + collegeHr + "' and TT_degCode='" + degreeHr + "' and TT_sem='" + semHr + "' and TT_sec='" + secHr + "')";
                                    dtCurHourTTCopy.Merge(dtCurDayTT.DefaultView.ToTable());
                                }
                                else if (current_hraltered == true) //Aruna 12sep2017
                                {
                                    dtStaffAlterTT.DefaultView.RowFilter = "TT_Day='" + dicDayOrder[dayOfWeek] + "' and TT_Hour='" + curPeriod + "' and TT_AlterDate='" + dtFrom.ToString("MM/dd/yyyy") + "'";
                                    dtHourAlter = dtStaffAlterTT.DefaultView.ToTable();  //somebody give the hour to logged staff
                                    dtCurHourTTCopy.Merge(dtHourAlter);
                                    continue;
                                }
                                else
                                {
                                    if (dtCurDayTT.Rows.Count > 0 && !hasAlter)
                                    {
                                        dtCurDayTT.DefaultView.RowFilter = "TT_Day='" + dicDayOrder[dayOfWeek] + "' and TT_Hour='" + curPeriod + "' and ((TT_batchyear not in('" + string.Join("','", batchYearList.ToArray()) + "') and TT_degCode not in('" + string.Join("','", degreeCodeList.ToArray()) + "') and TT_sem not in('" + string.Join("','", semesterList.ToArray()) + "') and TT_sec not in('" + string.Join("','", sectionsList.ToArray()) + "')))";//and TT_batchyear='" + batchHr + "' and TT_colCode='" + collegeHr + "' and TT_degCode='" + degreeHr + "' and TT_sem='" + semHr + "' and TT_sec='" + secHr + "'

                                        DataTable dtCurStfHourTT = dtCurDayTT.DefaultView.ToTable();
                                        dtCurHourTTCopy.Merge(dtCurStfHourTT);
                                    }

                                    else if (dtCurDayTT.Rows.Count > 0 && hasAlter == true) //Add by aruna 29jul2017 Merge current hr[regular hr] + alter hr[somebody gives the hr]
                                    {
                                        string joinq = "";
                                        bool isOtherStaffDeptAlter = false;
                                        if (dtHourAlter.Rows.Count > 0)
                                        {
                                            string batchHr1 = Convert.ToString(dtHourAlter.Rows[0]["TT_batchyear"]);
                                            string collegeHr1 = Convert.ToString(dtHourAlter.Rows[0]["TT_colCode"]).Trim();
                                            string degreeHr1 = Convert.ToString(dtHourAlter.Rows[0]["TT_degCode"]).Trim();
                                            string semHr1 = Convert.ToString(dtHourAlter.Rows[0]["TT_sem"]).Trim();
                                            string secHr1 = Convert.ToString(dtHourAlter.Rows[0]["TT_sec"]).Trim();

                                            dtHourAlter.DefaultView.RowFilter = "TT_batchyear='" + batchHr + "' and TT_colCode='" + collegeHr + "' and TT_degCode='" + degreeHr + "' and TT_sem='" + semHr + "' and TT_sec='" + secHr + "'";
                                            DataTable dtNewStaffAlter = dtHourAlter.DefaultView.ToTable();
                                            if (dtNewStaffAlter.Rows.Count > 0)
                                            {
                                                isOtherStaffDeptAlter = true;
                                            }
                                            //aruna 12 sep2017 joinq = " and (TT_batchyear not in(" + batchHr1 + ")  and TT_sem not in(" + semHr1 + "))-this will remove current hr refer 31-8-2017 issue"; 

                                        }
                                        #region Commented By Malang Raja On Oct 12 2017 Refer Oct 4 2017 Kongu Issues

                                        //dtCurDayTT.DefaultView.RowFilter = "TT_Day='" + dicDayOrder[dayOfWeek] + "' and TT_Hour='" + curPeriod + "' and (TT_batchyear='" + batchHr + "' and TT_colCode='" + collegeHr + "' and TT_degCode='" + degreeHr + "' and TT_sem='" + semHr + "' and TT_sec='" + secHr + "') " + joinq + "";
                                        //dtCurHourTTCopy.Merge(dtCurDayTT.DefaultView.ToTable()); 

                                        #endregion

                                        #region Modified By Malang Raja On Oct 12 2017 Refer Oct 4 2017 Kongu Issues

                                        if (!isOtherStaffDeptAlter)
                                        {
                                            dtCurDayTT.DefaultView.RowFilter = "TT_Day='" + dicDayOrder[dayOfWeek] + "' and TT_Hour='" + curPeriod + "' and (TT_batchyear='" + batchHr + "' and TT_colCode='" + collegeHr + "' and TT_degCode='" + degreeHr + "' and TT_sem='" + semHr + "' and TT_sec='" + secHr + "') " + joinq + "";

                                            dtCurHourTTCopy.Merge(dtCurDayTT.DefaultView.ToTable());
                                        }

                                        #endregion

                                        dtCurHourTTCopy.Merge(dtHourAlter);
                                    }

                                    else
                                    {
                                        dtCurHourTTCopy.Merge(dtHourAlter);
                                    }
                                }
                            }
                            else
                            {
                                seminfo = false;
                            }
                            hourCount++;
                        }
                        if (dtCurHourTTCopy.Rows.Count > 0)
                        {
                            dtCurHourTTCopy.DefaultView.Sort = "TT_colCode,TT_batchyear,TT_degCode,TT_sem,TT_sec,TT_date desc";
                            dtCurHourTT = dtCurHourTTCopy.DefaultView.ToTable(true);
                        }
                    }
                    else //if(seminfo==true)
                    {
                        if (isStfAlter)
                        {
                            dtStaffAlterTT.DefaultView.RowFilter = "TT_Day=" + dicDayOrder[dayOfWeek] + " and TT_Hour=" + curPeriod + " and TT_AlterDate='" + dtFrom.ToString("MM/dd/yyyy") + "'";
                            dtCurHourTT = dtStaffAlterTT.DefaultView.ToTable();
                            if (dtCurHourTT.Rows.Count > 0)
                            {
                                newlist = dtCurHourTT.AsEnumerable().Select(r => r.Field<decimal>("TT_subno")).ToList<decimal>(); //Added by jairam 04-09-2017
                                alterSchd = true;
                                hasAlter = true;
                            }
                        }
                        if (!alterSchd)
                        {
                            //Check for other staff alter
                            if (isOthStfAlter)
                            {
                                dtOtrStfAlterTT.DefaultView.RowFilter = "TT_Day=" + dicDayOrder[dayOfWeek] + " and TT_Hour=" + curPeriod + " and TT_AlterDate='" + dtFrom.ToString("MM/dd/yyyy") + "'";
                                dtCurHourTT = dtOtrStfAlterTT.DefaultView.ToTable();
                                if (dtCurHourTT.Rows.Count > 0)
                                {
                                    continue;
                                }
                            }
                        }
                        //Default Time table if no alter available
                        if (dtCurHourTT.Rows.Count == 0)
                        {
                            if (dtCurDayTT.Rows.Count > 0)
                            {
                                dtCurDayTT.DefaultView.RowFilter = "TT_Day='" + dicDayOrder[dayOfWeek] + "' and TT_Hour=" + curPeriod + "";
                                dtCurHourTT = dtCurDayTT.DefaultView.ToTable();
                            }
                        }
                        else
                        {
                            if (dtCurDayTT.Rows.Count > 0 && !hasAlter)
                            {
                                dtCurDayTT.DefaultView.RowFilter = " TT_Day='" + dicDayOrder[dayOfWeek] + "' and TT_Hour=" + curPeriod + "";
                                DataTable dtCurStfHourTT = dtCurDayTT.DefaultView.ToTable();
                                dtCurHourTT.Merge(dtCurStfHourTT);
                            }
                        }
                    }
                    if (!seminfo)//Rajkumar 
                    {
                        if (isStfAlter)
                        {
                            dtStaffAlterTT.DefaultView.RowFilter = "TT_Day=" + dicDayOrder[dayOfWeek] + " and TT_Hour=" + curPeriod + " and TT_AlterDate='" + dtFrom.ToString("MM/dd/yyyy") + "'";
                            dtCurHourTT = dtStaffAlterTT.DefaultView.ToTable();
                            if (dtCurHourTT.Rows.Count > 0)
                            {
                                newlist = dtCurHourTT.AsEnumerable().Select(r => r.Field<decimal>("TT_subno")).ToList<decimal>(); //Added by jairam 04-09-2017
                                alterSchd = true;
                                hasAlter = true;
                            }
                        }
                        
                    }

                    if (dtCurHourTT.Rows.Count > 0)
                    {
                        drTT["DayVal"] = dicDayOrder[dayOfWeek];

                        StringBuilder sbFnlDisp = new StringBuilder();
                        StringBuilder sbFnlVal = new StringBuilder();
                        StringBuilder sbFnlTTPk = new StringBuilder();

                        for (int subDetI = 0; subDetI < dtCurHourTT.Rows.Count; subDetI++)
                        {
                            string batch = Convert.ToString(dtCurHourTT.Rows[subDetI]["TT_batchyear"]);
                            string degcode = Convert.ToString(dtCurHourTT.Rows[subDetI]["TT_degCode"]);
                            string sem = Convert.ToString(dtCurHourTT.Rows[subDetI]["TT_sem"]);
                            string sec = Convert.ToString(dtCurHourTT.Rows[subDetI]["TT_sec"]);
                            string subno = Convert.ToString(dtCurHourTT.Rows[subDetI]["TT_subno"]);
                            string staffcode = Convert.ToString(dtCurHourTT.Rows[subDetI]["TT_Staffcode"]);
                            string course = Convert.ToString(dtCurHourTT.Rows[subDetI]["Course_Name"]);
                            string deptname = Convert.ToString(dtCurHourTT.Rows[subDetI]["Dept_Name"]);
                            string subname = Convert.ToString(dtCurHourTT.Rows[subDetI]["subject_name"]);
                            string subcode = Convert.ToString(dtCurHourTT.Rows[subDetI]["subject_code"]);
                            string deptacr = Convert.ToString(dtCurHourTT.Rows[subDetI]["dept_acronym"]);
                            string classpk = Convert.ToString(dtCurHourTT.Rows[subDetI]["TT_name"]);
                            string elective = Convert.ToString(dtCurHourTT.Rows[subDetI]["elective"]).ToLower();
                            string lab = Convert.ToString(dtCurHourTT.Rows[subDetI]["lab"]).ToLower();
                            string RoomName = Convert.ToString(dtCurHourTT.Rows[subDetI]["RoomName"]).ToLower();
                            drTT["Elective"] = elective;
                            drTT["Lab"] = lab;
                            //List<string> lstSec = dtCurHourTT.AsEnumerable().Select(r => r.Field<string>("TT_sec")).ToList<string>();
                            //sec = string.Join(",", lstSec);
                            if (!newlist.Contains(Convert.ToDecimal(subno))) //Added by jairam 04-09-2017
                                alterSchd = false;
                            else
                                alterSchd = true;

                            sbFnlDisp.Append(subname + " $" + batch + " $" + course + " $" + deptacr + " $Sem " + sem + " " + sec + " $ RoomNo  " + RoomName.ToString().ToUpper() + ";");
                            sbFnlVal.Append(subcode + "#" + subno + "#" + batch + "#" + degcode + "#" + sem + "#" + sec + "#" + staffcode + "#" + elective + "#" + lab + ";");
                            sbFnlTTPk.Append(classpk + "$" + alterSchd + ";");

                        }

                        drTT["P" + curPeriod + "ValDisp"] = sbFnlDisp.ToString().TrimEnd(';');
                        drTT["P" + curPeriod + "Val"] = sbFnlVal.ToString().TrimEnd(';');
                        drTT["TT_" + curPeriod] = sbFnlTTPk.ToString().TrimEnd(';');
                    }
                    //    }

                    //}
                }
                dtTTDisp.Rows.Add(drTT);
                dtFrom = dtFrom.AddDays(1);
            }

            if (dtTTDisp.Rows.Count > 0)
            {
                gridTimeTable.DataSource = dtTTDisp;
                gridTimeTable.DataBind();
                divTimeTable.Visible = true;
            }
        }
        catch (Exception ex)
        {
            //da.sendErrorMail(ex, ((Session["collegecode"] != null) ? Convert.ToString(Session["collegecode"]).Trim() : "13"), System.IO.Path.GetFileName(Request.Url.AbsolutePath).ToString());
            //divTimeTable.Visible = false;
        }
    }

    //Load staff time table for selected days day order
    private void loadGridTimeTableDayOrder()
    {
        try
        {
            divTimeTable.Visible = false;

            string fromDateTime = txtFromDate.Text.Split('/')[1] + "/" + txtFromDate.Text.Split('/')[0] + "/" + txtFromDate.Text.Split('/')[2];
            string toDateTime = txtToDate.Text.Split('/')[1] + "/" + txtToDate.Text.Split('/')[0] + "/" + txtToDate.Text.Split('/')[2];
            DateTime dtFrom = Convert.ToDateTime(fromDateTime);
            DateTime dtTo = Convert.ToDateTime(toDateTime);

            byte maxPeriod = getMaxPeriods();
            Dictionary<string, byte> dicDayOrder = getDayOrder();
            DataTable dtOvalStaffTT = getStaffTT();

            DataTable dtTTDates = dtOvalStaffTT.AsDataView().ToTable(true, "TT_date");
            if (dtTTDates.Rows.Count == 0)
            {
                return;
            }

            List<int> lstBatch = getStfAllotBatch();
            List<Int64> lstDeg = getStfAllotDegree();
            List<byte> lstSem = getStfAllotSem();
            string batchyear = string.Join(",", lstBatch);
            string degreecode = string.Join(",", lstDeg);
            string semester = string.Join(",", lstSem);

            string semStart = string.Empty;
            string semEnd = string.Empty;
            int startDayOrder = 1;
            getSemStartEndDate(ref semStart, ref semEnd, ref startDayOrder, batchyear, degreecode, semester, 0);

            DateTime semStartDt = Convert.ToDateTime(semStart);
            DateTime semEndDt = Convert.ToDateTime(semEnd);

            bool dateCheck = false;
            if (dtFrom >= semStartDt && dtFrom <= semEndDt && dtTo >= semStartDt && dtTo <= semEndDt)
                dateCheck = true;

            if (!dateCheck)
                return;

            int noofdays = NoOfDaysPerweek();

            DataTable HolidayDates = getHolidayDates(semStart, semEnd, degreecode);
            //List<string> holiDates = HolidayDates.AsEnumerable().Select(r => r.Field<string>("Hday")).ToList<string>();

            DataTable dtTTDisp = new DataTable();
            dtTTDisp.Columns.Add("DateDisp");
            dtTTDisp.Columns.Add("DateVal");
            dtTTDisp.Columns.Add("DayOrder");
            dtTTDisp.Columns.Add("DayVal");
            dtTTDisp.Columns.Add("Elective");
            dtTTDisp.Columns.Add("Lab");
            dtTTDisp.Columns.Add("P1ValDisp");
            dtTTDisp.Columns.Add("P1Val");
            dtTTDisp.Columns.Add("TT_1");
            dtTTDisp.Columns.Add("P2ValDisp");
            dtTTDisp.Columns.Add("P2Val");
            dtTTDisp.Columns.Add("TT_2");
            dtTTDisp.Columns.Add("P3ValDisp");
            dtTTDisp.Columns.Add("P3Val");
            dtTTDisp.Columns.Add("TT_3");
            dtTTDisp.Columns.Add("P4ValDisp");
            dtTTDisp.Columns.Add("P4Val");
            dtTTDisp.Columns.Add("TT_4");
            dtTTDisp.Columns.Add("P5ValDisp");
            dtTTDisp.Columns.Add("P5Val");
            dtTTDisp.Columns.Add("TT_5");
            dtTTDisp.Columns.Add("P6ValDisp");
            dtTTDisp.Columns.Add("P6Val");
            dtTTDisp.Columns.Add("TT_6");
            dtTTDisp.Columns.Add("P7ValDisp");
            dtTTDisp.Columns.Add("P7Val");
            dtTTDisp.Columns.Add("TT_7");
            dtTTDisp.Columns.Add("P8ValDisp");
            dtTTDisp.Columns.Add("P8Val");
            dtTTDisp.Columns.Add("TT_8");
            dtTTDisp.Columns.Add("P9ValDisp");
            dtTTDisp.Columns.Add("P9Val");
            dtTTDisp.Columns.Add("TT_9");
            dtTTDisp.Columns.Add("P10ValDisp");
            dtTTDisp.Columns.Add("P10Val");
            dtTTDisp.Columns.Add("TT_10");

            Hashtable htDayOrder = new Hashtable();
            htDayOrder.Add("Monday", "Day 1");
            htDayOrder.Add("Tuesday", "Day 2");
            htDayOrder.Add("Wednesday", "Day 3");
            htDayOrder.Add("Thursday", "Day 4");
            htDayOrder.Add("Friday", "Day 5");
            htDayOrder.Add("Saturday", "Day 6");
            htDayOrder.Add("Sunday", "Day 7");
            while (dtTo >= dtFrom)
            {
                //string dayOfWeek = findday(dtFrom.ToString("MM/dd/yyyy"), degreecode, semester, batchyear, semStart, noofdays.ToString(), startDayOrder.ToString());

                string dayOfWeek = findday(dtFrom.ToString("MM/dd/yyyy"), lstDeg[0].ToString(), lstSem[0].ToString(), lstBatch[0].ToString(), semStart, noofdays.ToString(), startDayOrder.ToString());

                HolidayDates.DefaultView.RowFilter = "Hday='" + dtFrom.ToString("MM/dd/yyyy") + "'";
                DataTable dtCurHoli = HolidayDates.DefaultView.ToTable();

                if (dtCurHoli.Rows.Count > 0)
                {
                    DataRow drTTholi = dtTTDisp.NewRow();
                    drTTholi["DateDisp"] = dtFrom.ToString("dd/MM/yyyy") + "<br> (" + htDayOrder[dayOfWeek].ToString() + ")";//dtFrom.DayOfWeek
                    drTTholi["DateVal"] = dtFrom.ToString("MM/dd/yyyy");
                    drTTholi["DayOrder"] = dayOfWeek;
                    for (byte curPeriod = 1; curPeriod <= maxPeriod; curPeriod++)
                    {
                        drTTholi["P" + curPeriod + "ValDisp"] = Convert.ToString(dtCurHoli.Rows[0]["holiday_desc"]) + "##MISD";
                    }
                    dtTTDisp.Rows.Add(drTTholi);

                    dtFrom = dtFrom.AddDays(1);
                    continue;
                }

                DataRow drTT = dtTTDisp.NewRow();
                drTT["DateDisp"] = dtFrom.ToString("dd/MM/yyyy") + "<br> (" + htDayOrder[dayOfWeek].ToString() + ")";//dtFrom.DayOfWeek
                drTT["DateVal"] = dtFrom.ToString("MM/dd/yyyy");
                drTT["DayOrder"] = dayOfWeek;

                dtTTDates.DefaultView.RowFilter = "TT_date<='" + dtFrom + "'";
                DataTable dtCurDate = dtTTDates.DefaultView.ToTable();
                DateTime ttCurDate = new DateTime();
                if (dtCurDate.Rows.Count > 0)
                {
                    ttCurDate = Convert.ToDateTime(dtCurDate.Rows[0]["TT_date"]);
                }

                //dtOvalStaffTT.DefaultView.RowFilter = "TT_date = '" + ttCurDate + "'";
                //DataTable dtStaffTT = dtOvalStaffTT.DefaultView.ToTable();

                dtOvalStaffTT.DefaultView.RowFilter = "TT_date<='" + dtFrom + "'";
                dtOvalStaffTT.DefaultView.Sort = "TT_date desc";
                DataTable dtDistinctClassTT = dtOvalStaffTT.DefaultView.ToTable(true, "TT_colCode", "TT_batchyear", "TT_degCode", "TT_sem", "TT_sec");
                DataTable dtDistinctClassTTDet = dtOvalStaffTT.DefaultView.ToTable(true, "TT_colCode", "TT_batchyear", "TT_degCode", "TT_sem", "TT_sec", "TT_date", "TT_ClassPK");
                DataTable dtStaffTT = new DataTable();
                DataTable dtStaffTTCopy = new DataTable();
                int ttCount = 0;
                foreach (DataRow drClassTT in dtDistinctClassTT.Rows)
                {
                    dtDistinctClassTTDet.DefaultView.RowFilter = "TT_colCode='" + Convert.ToString(drClassTT["TT_colCode"]).Trim() + "' and TT_batchyear='" + Convert.ToString(drClassTT["TT_batchyear"]).Trim() + "' and TT_degCode='" + Convert.ToString(drClassTT["TT_degCode"]).Trim() + "' and TT_sem='" + Convert.ToString(drClassTT["TT_sem"]).Trim() + "' and TT_sec='" + Convert.ToString(drClassTT["TT_sec"]).Trim() + "'";
                    List<long> lstTTPks = new List<long>();
                    string ttPks = string.Empty;
                    DataTable dtTTPks = new DataTable();
                    dtDistinctClassTTDet.DefaultView.Sort = "TT_date desc";
                    dtTTPks = dtDistinctClassTTDet.DefaultView.ToTable(true, "TT_ClassPK");
                    if (dtTTPks.Rows.Count > 0)
                    {
                        ttPks = Convert.ToString(dtTTPks.Rows[0]["TT_ClassPK"]).Trim();
                        dtOvalStaffTT.DefaultView.RowFilter = "TT_ClassPK ='" + ttPks + "'";
                        DataTable dtCopyTT = new DataTable();
                        dtOvalStaffTT.DefaultView.Sort = "TT_date desc";
                        dtCopyTT = dtOvalStaffTT.DefaultView.ToTable();
                        if (dtCopyTT.Rows.Count > 0)
                        {
                            dtStaffTTCopy.Merge(dtCopyTT);
                            //if (ttCount == 0)
                            //{
                            //    dtStaffTTCopy = dtOvalStaffTT.DefaultView.ToTable();
                            //}
                            //else
                            //{

                            //}
                            ttCount++;
                        }
                    }
                }

                #region Alter time table Data
                string ttPk = string.Empty;
                if (dtStaffTTCopy.Rows.Count > 0)
                {
                    ttPk = Convert.ToString(dtStaffTTCopy.Rows[0]["TT_ClassPk"]);
                    dtStaffTTCopy.DefaultView.RowFilter = string.Empty;
                    dtStaffTTCopy.DefaultView.Sort = "TT_colCode,TT_batchyear,TT_degCode,TT_sem,TT_sec,TT_date desc";
                    dtStaffTT = dtStaffTTCopy.DefaultView.ToTable();
                }
                bool isStfAlter = false;
                bool isOthStfAlter = false;
                DataTable dtStaffAlterTT = getStaffAlterTT(dtFrom.ToString("MM/dd/yyyy"), lstBatchYear: lstBatch, lstDegreeCode: lstDeg, lstSemester: lstSem);
                DataTable dtOtrStfAlterTT = getOtherStaffAlterTT(dtFrom.ToString("MM/dd/yyyy"), lstBatchYear: lstBatch, lstDegreeCode: lstDeg, lstSemester: lstSem);
                if (dtStaffAlterTT.Rows.Count > 0)
                {
                    isStfAlter = true;
                }
                if (dtOtrStfAlterTT.Rows.Count > 0)
                {
                    isOthStfAlter = true;
                }
                #endregion

                DataTable dtCurDayTT = new DataTable();
                if (dtStaffTT.Rows.Count > 0)
                {
                    dtStaffTT.DefaultView.RowFilter = "TT_Day=" + dicDayOrder[dayOfWeek] + "";
                    dtCurDayTT = dtStaffTT.DefaultView.ToTable();
                }
                for (byte curPeriod = 1; curPeriod <= maxPeriod; curPeriod++)
                {
                    //dtCurDayTT.DefaultView.RowFilter = "TT_Hour=" + curPeriod + "";
                    //DataTable dtCurHourTT = dtCurDayTT.DefaultView.ToTable();
                    DataTable dtCurHourTT = new DataTable();

                    //Check For Alter Schedule
                    bool alterSchd = false;
                    bool hasAlter = false;
                    DataTable dtHourTTDet = new DataTable();
                    if (dtStaffTT.Rows.Count > 0)
                    {
                        dtStaffTT.DefaultView.RowFilter = "TT_Day='" + dicDayOrder[dayOfWeek] + "' and TT_Hour='" + curPeriod + "' ";
                        dtHourTTDet = dtStaffTT.DefaultView.ToTable(true, "TT_batchyear", "TT_colCode", "TT_degCode", "TT_sem", "TT_sec");
                    }
                    //List<int> lstBatchHour = new List<int>();
                    //List<int> lstDegreeHour = new List<int>();
                    //List<int> lstSemHour = new List<int>();
                    //List<int> lstCollegeHour = new List<int>();
                    //List<string> lstSecHour = new List<string>();
                    //lstCollegeHour = dtHourTTDet.AsEnumerable().Select(r => r.Field<int>("TT_colCode")).ToList<int>();
                    //lstBatchHour = dtHourTTDet.AsEnumerable().Select(r => r.Field<int>("TT_batchyear")).ToList<int>();
                    //lstDegreeHour = dtHourTTDet.AsEnumerable().Select(r => r.Field<int>("TT_degCode")).ToList<int>();
                    //lstSemHour = dtHourTTDet.AsEnumerable().Select(r => r.Field<int>("TT_sem")).ToList<int>();
                    //lstSecHour = dtHourTTDet.AsEnumerable().Select(r => r.Field<string>("TT_sec")).ToList<string>();
                    DataTable dtHourAlter = new DataTable();
                    int hourCount = 0;
                    if (dtHourTTDet.Rows.Count > 0)
                    {
                        foreach (DataRow drAlter in dtHourTTDet.Rows)
                        {
                            string batchHr = Convert.ToString(drAlter["TT_batchyear"]).Trim();
                            string collegeHr = Convert.ToString(drAlter["TT_colCode"]).Trim();
                            string degreeHr = Convert.ToString(drAlter["TT_degCode"]).Trim();
                            string semHr = Convert.ToString(drAlter["TT_sem"]).Trim();
                            string secHr = Convert.ToString(drAlter["TT_sec"]).Trim();
                            hasAlter = false;
                            alterSchd = false;
                            if (isStfAlter)
                            {
                                dtStaffAlterTT.DefaultView.RowFilter = "TT_Day='" + dicDayOrder[dayOfWeek] + "' and TT_Hour='" + curPeriod + "' and TT_AlterDate='" + dtFrom.ToString("MM/dd/yyyy") + "' and TT_batchyear='" + batchHr + "' and TT_colCode='" + collegeHr + "' and TT_degCode='" + degreeHr + "' and TT_sem='" + semHr + "' and TT_sec='" + secHr + "'";
                                dtHourAlter = dtStaffAlterTT.DefaultView.ToTable();
                                if (dtHourAlter.Rows.Count > 0)
                                {
                                    alterSchd = true;
                                    hasAlter = true;
                                }
                            }
                            if (!alterSchd)
                            {
                                //Check for other staff alter
                                if (isOthStfAlter)
                                {
                                    dtOtrStfAlterTT.DefaultView.RowFilter = "TT_Day='" + dicDayOrder[dayOfWeek] + "' and TT_Hour='" + curPeriod + "' and TT_AlterDate='" + dtFrom.ToString("MM/dd/yyyy") + "' and TT_batchyear='" + batchHr + "' and TT_colCode='" + collegeHr + "' and TT_degCode='" + degreeHr + "' and TT_sem='" + semHr + "' and TT_sec='" + secHr + "'";
                                    dtHourAlter = dtOtrStfAlterTT.DefaultView.ToTable();
                                    if (dtHourAlter.Rows.Count > 0)
                                    {
                                        continue;
                                    }
                                }
                            }
                            //Default Time table if no alter available
                            if (dtHourAlter.Rows.Count == 0)
                            {
                                dtCurDayTT.DefaultView.RowFilter = "TT_Hour='" + curPeriod + "' and TT_batchyear='" + batchHr + "' and TT_colCode='" + collegeHr + "' and TT_degCode='" + degreeHr + "' and TT_sem='" + semHr + "' and TT_sec='" + secHr + "'";
                                dtCurHourTT.Merge(dtCurDayTT.DefaultView.ToTable());
                            }
                            else
                            {
                                if (dtCurDayTT.Rows.Count > 0 && !hasAlter)
                                {
                                    dtCurDayTT.DefaultView.RowFilter = "TT_Hour='" + curPeriod + "' and TT_batchyear='" + batchHr + "' and TT_colCode='" + collegeHr + "' and TT_degCode='" + degreeHr + "' and TT_sem='" + semHr + "' and TT_sec='" + secHr + "'";
                                    DataTable dtCurStfHourTT = dtCurDayTT.DefaultView.ToTable();
                                    dtCurHourTT.Merge(dtCurStfHourTT);
                                }
                                else
                                {
                                    dtCurHourTT.Merge(dtHourAlter);
                                }
                            }
                            hourCount++;
                        }
                    }
                    else
                    {
                        if (isStfAlter)
                        {
                            dtStaffAlterTT.DefaultView.RowFilter = "TT_Day=" + dicDayOrder[dayOfWeek] + " and TT_Hour=" + curPeriod + " and TT_AlterDate='" + dtFrom.ToString("MM/dd/yyyy") + "'";
                            dtCurHourTT = dtStaffAlterTT.DefaultView.ToTable();
                            if (dtCurHourTT.Rows.Count > 0)
                            {
                                alterSchd = true;
                                hasAlter = true;
                            }
                        }
                        if (!alterSchd)
                        {
                            //Check for other staff alter
                            if (isOthStfAlter)
                            {
                                dtOtrStfAlterTT.DefaultView.RowFilter = "TT_Day=" + dicDayOrder[dayOfWeek] + " and TT_Hour=" + curPeriod + " and TT_AlterDate='" + dtFrom.ToString("MM/dd/yyyy") + "'";
                                dtCurHourTT = dtOtrStfAlterTT.DefaultView.ToTable();
                                if (dtCurHourTT.Rows.Count > 0)
                                {
                                    continue;
                                }
                            }
                        }
                        //Default Time table if no alter available
                        if (dtCurHourTT.Rows.Count == 0)
                        {
                            dtCurDayTT.DefaultView.RowFilter = "TT_Hour=" + curPeriod + "";
                            dtCurHourTT = dtCurDayTT.DefaultView.ToTable();
                        }
                        else
                        {
                            if (dtCurDayTT.Rows.Count > 0 && !hasAlter)
                            {
                                dtCurDayTT.DefaultView.RowFilter = "TT_Hour=" + curPeriod + "";
                                DataTable dtCurStfHourTT = dtCurDayTT.DefaultView.ToTable();
                                dtCurHourTT.Merge(dtCurStfHourTT);
                            }
                            //else
                            //{
                            //    dtCurHourTT.Merge(dtCurHourTT);
                            //}
                        }
                    }
                    //if (isStfAlter)
                    //{
                    //    dtStaffAlterTT.DefaultView.RowFilter = "TT_Day=" + dicDayOrder[dayOfWeek] + " and TT_Hour=" + curPeriod + " and TT_AlterDate='" + dtFrom.ToString("MM/dd/yyyy") + "'";
                    //    dtCurHourTT = dtStaffAlterTT.DefaultView.ToTable();
                    //    if (dtCurHourTT.Rows.Count > 0)
                    //    {
                    //        alterSchd = true;
                    //    }
                    //}
                    //if (!alterSchd)
                    //{
                    //    //Check for other staff alter
                    //    if (isOthStfAlter)
                    //    {
                    //        dtOtrStfAlterTT.DefaultView.RowFilter = "TT_Day=" + dicDayOrder[dayOfWeek] + " and TT_Hour=" + curPeriod + " and TT_AlterDate='" + dtFrom.ToString("MM/dd/yyyy") + "'";
                    //        dtCurHourTT = dtOtrStfAlterTT.DefaultView.ToTable();
                    //        if (dtCurHourTT.Rows.Count > 0)
                    //        {
                    //            continue;
                    //        }
                    //    }
                    //}
                    ////Default Time table if no alter available
                    //if (dtCurHourTT.Rows.Count == 0)
                    //{
                    //    dtCurDayTT.DefaultView.RowFilter = "TT_Hour=" + curPeriod + "";
                    //    dtCurHourTT = dtCurDayTT.DefaultView.ToTable();
                    //}
                    //else
                    //{
                    //    if (dtCurDayTT.Rows.Count > 0)
                    //    {
                    //        dtCurDayTT.DefaultView.RowFilter = "TT_Hour=" + curPeriod + "";
                    //        DataTable dtCurStfHourTT = dtCurDayTT.DefaultView.ToTable();
                    //        dtCurHourTT.Merge(dtCurStfHourTT);
                    //    }
                    //}


                    if (dtCurHourTT.Rows.Count > 0)
                    {
                        drTT["DayVal"] = dicDayOrder[dayOfWeek];
                        string elective = Convert.ToString(dtCurHourTT.Rows[0]["elective"]).ToLower();
                        string lab = Convert.ToString(dtCurHourTT.Rows[0]["lab"]).ToLower();
                        drTT["Elective"] = elective;
                        drTT["Lab"] = lab;

                        StringBuilder sbFnlDisp = new StringBuilder();
                        StringBuilder sbFnlVal = new StringBuilder();
                        StringBuilder sbFnlTTPk = new StringBuilder();

                        for (int subDetI = 0; subDetI < dtCurHourTT.Rows.Count; subDetI++)
                        {
                            string batch = Convert.ToString(dtCurHourTT.Rows[subDetI]["TT_batchyear"]);
                            string degcode = Convert.ToString(dtCurHourTT.Rows[subDetI]["TT_degCode"]);
                            string sem = Convert.ToString(dtCurHourTT.Rows[subDetI]["TT_sem"]);
                            string sec = Convert.ToString(dtCurHourTT.Rows[subDetI]["TT_sec"]);
                            string subno = Convert.ToString(dtCurHourTT.Rows[subDetI]["TT_subno"]);
                            string staffcode = Convert.ToString(dtCurHourTT.Rows[subDetI]["TT_Staffcode"]);
                            string course = Convert.ToString(dtCurHourTT.Rows[subDetI]["Course_Name"]);
                            string deptname = Convert.ToString(dtCurHourTT.Rows[subDetI]["Dept_Name"]);
                            string subname = Convert.ToString(dtCurHourTT.Rows[subDetI]["subject_name"]);
                            string subcode = Convert.ToString(dtCurHourTT.Rows[subDetI]["subject_code"]);
                            string deptacr = Convert.ToString(dtCurHourTT.Rows[subDetI]["dept_acronym"]);
                            string classpk = Convert.ToString(dtCurHourTT.Rows[subDetI]["TT_name"]);

                            //List<string> lstSec = dtCurHourTT.AsEnumerable().Select(r => r.Field<string>("TT_sec")).ToList<string>();
                            //sec = string.Join(",", lstSec);

                            sbFnlDisp.Append(subname + " $" + batch + " $" + course + " $" + deptacr + " $Sem " + sem + " " + sec + ";");
                            sbFnlVal.Append(subcode + "#" + subno + "#" + batch + "#" + degcode + "#" + sem + "#" + sec + "#" + staffcode + "#" + elective + "#" + lab + ";");
                            sbFnlTTPk.Append(classpk + "$" + alterSchd + ";");

                        }

                        drTT["P" + curPeriod + "ValDisp"] = sbFnlDisp.ToString().TrimEnd(';');
                        drTT["P" + curPeriod + "Val"] = sbFnlVal.ToString().TrimEnd(';');
                        drTT["TT_" + curPeriod] = sbFnlTTPk.ToString().TrimEnd(';');
                    }
                }

                dtTTDisp.Rows.Add(drTT);
                dtFrom = dtFrom.AddDays(1);
            }

            if (dtTTDisp.Rows.Count > 0)
            {
                gridTimeTable.DataSource = dtTTDisp;
                gridTimeTable.DataBind();
                divTimeTable.Visible = true;
            }

        }
        catch (Exception ex)
        {
            da.sendErrorMail(ex, ((Session["collegecode"] != null) ? Convert.ToString(Session["collegecode"]).Trim() : "13"), System.IO.Path.GetFileName(Request.Url.AbsolutePath).ToString());
            divTimeTable.Visible = false;
        }
    }

    //Data bound for time table grid
    protected void gridTimeTable_OnDataBound(object sender, EventArgs e)
    {
        try
        {
            byte periodCnt = getMaxPeriods();
            hideExtraPeriods(periodCnt);
            spanHoliday(periodCnt);
            //addColor(); aruna 29jan2018
        }
        catch { }
    }

    //Add color to alternate schedule
    private void addColor()
    {
        foreach (GridViewRow gRow in gridTimeTable.Rows)
        {
            Label lblCurDate = (Label)gRow.FindControl("lblDate");

            for (int coli = 1; coli < gridTimeTable.Columns.Count; coli++)
            {
                LinkButton lnkPeriod = (LinkButton)gRow.FindControl("lnkPeriod_" + coli);

                if (lnkPeriod != null && lnkPeriod.Text.Trim() != string.Empty && !lnkPeriod.Text.Contains("##MISD"))
                {
                    Label lblPeriod = (Label)gRow.FindControl("lblPeriod_" + coli);
                    Label lblTT = (Label)gRow.FindControl("lblTT_" + coli);
                    Label lblDayVal = (Label)gRow.FindControl("lblDayVal");

                    string labDay = string.Empty;
                    switch (lblDayVal.Text)
                    {
                        case "1":
                            labDay = "mon";
                            break;
                        case "2":
                            labDay = "tue";
                            break;
                        case "3":
                            labDay = "wed";
                            break;
                        case "4":
                            labDay = "thu";
                            break;
                        case "5":
                            labDay = "fri";
                            break;
                        case "6":
                            labDay = "sat";
                            break;
                        case "7":
                            labDay = "sun";
                            break;
                    }

                    bool isAlter = false;
                    string[] subDets = lblPeriod.Text.Split(';');
                    string[] subDispDets = lnkPeriod.Text.Split(';');
                    string[] classttPks = lblTT.Text.Split(';');
                    DataTable dtStudData = new DataTable();
                    if (subDets.Length > 0 && !string.IsNullOrEmpty(subDets[0].Trim()))
                    {
                        for (int subI = 0; subI < subDets.Length; subI++)
                        {
                            string[] ttData = subDets[subI].Split('#');
                            string[] ttDispData = subDispDets[subI].Split('$');
                            string[] ttClasspk = classttPks[subI].Split('$');

                            string batch = ttData[2];
                            string degcode = ttData[3];
                            string sem = ttData[4];
                            string sec = ttData[5];
                            string subno = ttData[1];
                            string staffcode = ttData[6];
                            string course = ttDispData[2];
                            string deptname = ttDispData[3];
                            string subname = ttDispData[0];
                            string subcode = ttData[0];
                            string deptacr = ttDispData[3];
                            string elective = ttData[7].Trim();
                            string lab = ttData[8].Trim();

                            isAlter = isAlterHour(lblCurDate.Text, lblDayVal.Text, coli.ToString(), staffcode, subno);
                            dtStudData.Merge(studinfo.getStudentData(collegeCode, batch, degcode, sem, sec, subno, staffcode, elective, lab, labDay, coli.ToString(), ttClasspk[0], ttClasspk[1], lblCurDate.Text));
                            if (isAlter)
                                break;


                        }


                        if (isAlter)
                        {
                            lnkPeriod.ForeColor = Color.DarkTurquoise;
                        }
                        else
                        {
                            dtStudData = dtStudData.DefaultView.ToTable(true);
                            List<Decimal> appNos = dtStudData.AsEnumerable().Select(r => r.Field<Decimal>("app_no")).ToList<Decimal>();
                            string appNoString = string.Join(",", appNos);

                            Dictionary<string, string> dicAttSaved = attLoadAtt(appNoString, lblCurDate.Text, coli.ToString());
                            if (dtStudData.Rows.Count > 0)
                            {
                                if (dtStudData.Rows.Count == dicAttSaved.Count)
                                {
                                    lnkPeriod.ForeColor = Color.ForestGreen;
                                }
                                else if (dtStudData.Rows.Count > dicAttSaved.Count && dicAttSaved.Count > 0)
                                {
                                    lnkPeriod.ForeColor = Color.DarkOrchid;
                                }
                                else
                                {
                                    DateTime dtNew = new DateTime();
                                    if (DateTime.TryParseExact(lblCurDate.Text.Trim(), "MM/dd/yyyy", null, DateTimeStyles.None, out dtNew))
                                    {
                                        if (dtNew == DateTime.Today)
                                        {
                                            lnkPeriod.ForeColor = Color.Blue;
                                        }
                                        else if (dtNew < DateTime.Today)
                                        {
                                            lnkPeriod.ForeColor = Color.Red;
                                        }
                                    }
                                }
                            }
                            //else
                            //{
                            //    DateTime dtNew = new DateTime();
                            //    if (DateTime.TryParseExact(lblCurDate.Text.Trim(), "MM/dd/yyyy", null, DateTimeStyles.None, out dtNew))
                            //    {
                            //        if (dtNew == DateTime.Today)
                            //        {
                            //            lnkPeriod.ForeColor = Color.Blue;
                            //        }
                            //        else if (dtNew < DateTime.Today)
                            //        {
                            //            lnkPeriod.ForeColor = Color.Red;
                            //        }
                            //    }
                            //}
                        }
                    }
                }
            }
        }
    }

    //Check if the hour is altered hour
    private bool isAlterHour(string alterDate, string day, string hour, string staffcode, string subno)
    {
        bool isAlter = false;
        try
        {
            string alterpk = dirAcc.selectScalarString("select TT_AlterDetPK from TT_ClassTimetable ct, TT_AlterTimetableDet at where ct.TT_ClassPK = at.TT_ClassFk and TT_AlterDate='" + alterDate + "' and TT_Day='" + day + "' and TT_Hour='" + hour + "' and TT_staffcode='" + staffcode + "' and TT_subno='" + subno + "'");
            if (!string.IsNullOrEmpty(alterpk))
            {
                isAlter = true;
            }
        }
        catch { }
        return isAlter;
    }

    //Span Holidays
    private void spanHoliday(byte periodCnt)
    {
        foreach (GridViewRow gRow in gridTimeTable.Rows)
        {
            bool isCheck = false;
            if (isCheck)
            {
                int countHoliHour = 0;
                int startColumn = 1;
                int endColumn = 1;
                bool isStart = false;
                for (int i = 1; i < gRow.Cells.Count; i++)
                {
                    LinkButton lnkPeriod = (LinkButton)gRow.Cells[i].FindControl("lnkPeriod_" + i);
                    if (lnkPeriod.Text.Contains("##MISD"))
                    {
                        if (!isStart)
                            startColumn = i;
                        isStart = true;
                        lnkPeriod.Enabled = false;
                        lnkPeriod.Attributes.Add("style", "text-decoration:none;color:Red;Font-size:25px;Font-weight:Bold;");
                        //lnkPeriod.Text = lnkPeriod.Text.Replace("##MISD", string.Empty);
                        countHoliHour++;
                        endColumn = i;
                    }
                }
                if (isStart)
                {
                    LinkButton lnkPeriod_2 = (LinkButton)gRow.FindControl("lnkPeriod_" + startColumn);
                    if (lnkPeriod_2.Text.Contains("##MISD"))
                    {
                        lnkPeriod_2.Enabled = false;
                        lnkPeriod_2.Attributes.Add("style", "text-decoration:none;color:Red;Font-size:25px;Font-weight:Bold;");
                        lnkPeriod_2.Text = lnkPeriod_2.Text.Replace("##MISD", string.Empty);
                        gRow.Cells[startColumn].HorizontalAlign = HorizontalAlign.Center;
                        gRow.Cells[startColumn].ColumnSpan = periodCnt;
                        for (int pCntI = 2; pCntI <= periodCnt; pCntI++)
                        {
                            if (pCntI < gRow.Cells.Count)
                                gRow.Cells[pCntI].Visible = false;
                        }
                    }
                    if (countHoliHour == periodCnt)
                    {
                        gRow.Visible = false;
                        for (int i = 0; i < gRow.Cells.Count; i++)
                        {
                            gRow.Cells[i].Visible = false;
                        }
                    }
                }
            }
            LinkButton lnkPeriod_1 = (LinkButton)gRow.FindControl("lnkPeriod_1");
            if (lnkPeriod_1.Text.Contains("##MISD"))
            {
                lnkPeriod_1.Enabled = false;
                lnkPeriod_1.Attributes.Add("style", "text-decoration:none;color:Red;Font-size:25px;Font-weight:Bold;");
                lnkPeriod_1.Text = lnkPeriod_1.Text.Replace("##MISD", string.Empty);
                gRow.Cells[1].HorizontalAlign = HorizontalAlign.Center;
                gRow.Cells[1].ColumnSpan = periodCnt;

                for (int pCntI = 2; pCntI <= periodCnt; pCntI++)
                {
                    gRow.Cells[pCntI].Visible = false;
                }
            }
        }
    }

    //Invisible extra columns
    private void hideExtraPeriods(byte periodCnt)
    {
        periodCnt += 1;
        for (int pCntI = periodCnt; pCntI < gridTimeTable.Columns.Count; pCntI++)
        {
            gridTimeTable.Columns[pCntI].Visible = false;
        }
    }

    //Get time table details for the login staff
    private DataTable getStaffTT()
    {
        DataTable dtStaffTt = new DataTable();
        try
        {
            //dtStaffTt = dirAcc.selectDataTable(" select TT_ClassPK,TT_colCode,TT_batchyear,TT_degCode,TT_sem,TT_sec,TT_date,TT_subno,TT_Staffcode,TT_Day,TT_Hour,Daydiscription,c.Course_Name,dt.Dept_Name,s.subject_name,subject_code,dt.dept_acronym,isnull(ss.ElectivePap,'0') as Elective,isnull(ss.Lab,'0') as Lab from TT_ClassTimetable ct,TT_ClassTimetableDet ctd,TT_Day_Dayorder do,subject s,sub_sem ss,Degree d,Department dt,course c where s.subType_no=ss.subType_no and s.syll_code=ss.syll_code and d.Degree_Code=ct.TT_degCode and c.Course_Id=d.Course_Id and dt.Dept_Code=d.Dept_Code and s.subject_no=ctd.TT_subno and  ct.TT_ClassPK=ctd.TT_ClassFk and ctd.TT_Day = do.TT_Day_DayorderPK and TT_lastRec='1' and TT_colCode='" + collegeCode + "' and TT_staffcode='" + staffCode + "'");

            //dtStaffTt = dirAcc.selectDataTable(" select distinct TT_ClassPK,TT_colCode,TT_batchyear,TT_degCode,TT_sem,TT_sec,TT_date,TT_subno,TT_Staffcode,TT_Day,TT_Hour,Daydiscription,c.Course_Name,dt.Dept_Name,s.subject_name,subject_code,dt.dept_acronym,isnull(ss.ElectivePap,'0') as Elective,isnull(ss.Lab,'0') as Lab from TT_ClassTimetable ct,TT_ClassTimetableDet ctd,TT_Day_Dayorder do,subject s,sub_sem ss,Degree d,Department dt,course c,Registration r,syllabus_master sm where sm.syll_code=s.syll_code and sm.syll_code=ss.syll_code and sm.Batch_Year = r.Batch_Year and sm.Degree_Code=r.degree_code and sm.semester = TT_sem and r.degree_code=d.Degree_Code and r.Batch_Year=TT_batchyear and r.Current_Semester=TT_sem and r.degree_code=TT_degCode and r.CC=0 and r.DelFlag='0' and r.Exam_Flag<>'debar' and s.subType_no=ss.subType_no and s.syll_code=ss.syll_code and d.Degree_Code=ct.TT_degCode and c.Course_Id=d.Course_Id and dt.Dept_Code=d.Dept_Code and s.subject_no=ctd.TT_subno and  ct.TT_ClassPK=ctd.TT_ClassFk and ctd.TT_Day = do.TT_Day_DayorderPK and TT_lastRec='1' and TT_colCode='" + collegeCode + "' and TT_staffcode='" + staffCode + "'  and TT_date = (select  max(nct.TT_date) from TT_ClassTimetable nct,TT_ClassTimetableDet nctd where nct.TT_ClassPK=nctd.TT_ClassFk and nct.TT_colCode='" + collegeCode + "' and nctd.TT_staffcode='" + staffCode + "')");

            dtStaffTt = dirAcc.selectDataTable(" select distinct TT_ClassPK,TT_name,TT_colCode,TT_batchyear,TT_degCode,TT_sem,TT_sec,TT_date,TT_subno,TT_Staffcode,TT_Day,TT_Hour,Daydiscription,c.Course_Name,dt.Dept_Name,s.subject_name,subject_code,dt.dept_acronym,isnull(ss.ElectivePap,'0') as Elective,isnull(ss.Lab,'0') as Lab,(select Room_Name from room_detail where roompk=TT_Room) as RoomName,Convert(varchar(20),TT_date,103) as TTDate from TT_ClassTimetable ct,TT_ClassTimetableDet ctd,TT_Day_Dayorder do,subject s,sub_sem ss,Degree d,Department dt,course c,Registration r,syllabus_master sm where sm.syll_code=s.syll_code and sm.syll_code=ss.syll_code and sm.Batch_Year = r.Batch_Year and sm.Degree_Code=r.degree_code and sm.semester = TT_sem and r.degree_code=d.Degree_Code and r.Batch_Year=TT_batchyear and r.Current_Semester=TT_sem and r.degree_code=TT_degCode and r.CC=0 and r.DelFlag='0' and r.Exam_Flag<>'debar' and s.subType_no=ss.subType_no and s.syll_code=ss.syll_code and d.Degree_Code=ct.TT_degCode and c.Course_Id=d.Course_Id and dt.Dept_Code=d.Dept_Code and s.subject_no=ctd.TT_subno and  ct.TT_ClassPK=ctd.TT_ClassFk and ctd.TT_Day = do.TT_Day_DayorderPK and TT_lastRec='1' and TT_colCode='" + collegeCode + "' and TT_staffcode='" + staffCode + "' and TT_date <= '" + (txtToDate.Text.Split('/')[1] + "/" + txtToDate.Text.Split('/')[0] + "/" + txtToDate.Text.Split('/')[2]) + "' order by TT_date desc ");
        }
        catch { dtStaffTt.Clear(); }
        return dtStaffTt;
    }

    //Get Staff alter timetable
    //Get alter time table details for the login staff

    private DataTable getStaffAlterTT(string ttAlterDate, List<int> lstBatchYear = null, List<Int64> lstDegreeCode = null, List<byte> lstSemester = null, List<string> lstCollegeCode = null)
    {
        DataTable dtStaffTt = new DataTable();
        try
        {
            string batchYear = string.Empty;
            string degreeCode = string.Empty;
            string semester = string.Empty;
            string collegeCode1 = collegeCode;
            if (lstCollegeCode != null)
            {
                collegeCode1 = string.Join("','", lstCollegeCode.ToArray());
                if (!string.IsNullOrEmpty(collegeCode1))
                    collegeCode1 = " and TT_colCode in('" + collegeCode1 + "')";
            }
            else
            {
                if (!string.IsNullOrEmpty(collegeCode1))
                    collegeCode1 = " and TT_colCode in('" + collegeCode1 + "')";
            }
            if (lstBatchYear != null)
            {
                batchYear = string.Join("','", lstBatchYear.ToArray());
                if (!string.IsNullOrEmpty(batchYear))
                    batchYear = " and TT_batchyear in('" + batchYear + "')";
            }
            if (lstDegreeCode != null)
            {
                degreeCode = string.Join("','", lstDegreeCode.ToArray());
                if (!string.IsNullOrEmpty(degreeCode))
                    degreeCode = " and TT_degCode in('" + degreeCode + "')";
            }
            if (lstSemester != null)
            {
                semester = string.Join("','", lstSemester.ToArray());
                if (!string.IsNullOrEmpty(semester))
                    semester = " and TT_sem in('" + semester + "')";
            }

            //Aruna 02/08/2017 dtStaffTt = dirAcc.selectDataTable(" select distinct TT_ClassPK,TT_name,TT_colCode,TT_batchyear,TT_degCode,TT_sem,TT_sec,Convert(varchar(10),TT_AlterDate,101) as TT_AlterDate, TT_date,TT_subno,TT_Staffcode,TT_Day,TT_Hour,Daydiscription,c.Course_Name,dt.Dept_Name,s.subject_name,subject_code,dt.dept_acronym,isnull(ss.ElectivePap,'0') as Elective,isnull(ss.Lab,'0') as Lab,(select Room_Name from room_detail where roompk=TT_Room) as RoomName from TT_ClassTimetable ct,TT_AlterTimetableDet ctd,TT_Day_Dayorder do,subject s,sub_sem ss,Degree d,Department dt,course c,Registration r,syllabus_master sm where sm.syll_code=s.syll_code and sm.syll_code=ss.syll_code and sm.Batch_Year = r.Batch_Year and sm.Degree_Code=r.degree_code and sm.semester = TT_sem and r.degree_code=d.Degree_Code and r.Batch_Year=TT_batchyear and r.Current_Semester=TT_sem and r.degree_code=TT_degCode and r.CC=0 and r.DelFlag='0' and r.Exam_Flag<>'debar' and s.subType_no=ss.subType_no and s.syll_code=ss.syll_code and d.Degree_Code=ct.TT_degCode and c.Course_Id=d.Course_Id and dt.Dept_Code=d.Dept_Code and s.subject_no=ctd.TT_subno and  ct.TT_ClassPK=ctd.TT_ClassFk and ctd.TT_Day = do.TT_Day_DayorderPK and TT_lastRec='1'  and TT_staffcode='" + staffCode + "' and ctd.TT_AlterDate='" + ttAlterDate + "' " + collegeCode1 + batchYear + degreeCode + semester);//and TT_colCode='" + collegeCode + "'
            dtStaffTt = dirAcc.selectDataTable(" select distinct TT_ClassPK,TT_name,TT_colCode,TT_batchyear,TT_degCode,TT_sem,TT_sec,Convert(varchar(10),TT_AlterDate,101) as TT_AlterDate, TT_date,TT_subno,TT_Staffcode,TT_Day,TT_Hour,Daydiscription,c.Course_Name,dt.Dept_Name,s.subject_name,subject_code,dt.dept_acronym,isnull(ss.ElectivePap,'0') as Elective,isnull(ss.Lab,'0') as Lab,(select Room_Name from room_detail where roompk=TT_Room) as RoomName from TT_ClassTimetable ct,TT_AlterTimetableDet ctd,TT_Day_Dayorder do,subject s,sub_sem ss,Degree d,Department dt,course c,Registration r,syllabus_master sm where sm.syll_code=s.syll_code and sm.syll_code=ss.syll_code and sm.Batch_Year = r.Batch_Year and sm.Degree_Code=r.degree_code and sm.semester = TT_sem and r.degree_code=d.Degree_Code and r.Batch_Year=TT_batchyear and r.Current_Semester=TT_sem and r.degree_code=TT_degCode and r.CC=0 and r.DelFlag='0' and r.Exam_Flag<>'debar' and s.subType_no=ss.subType_no and s.syll_code=ss.syll_code and d.Degree_Code=ct.TT_degCode and c.Course_Id=d.Course_Id and dt.Dept_Code=d.Dept_Code and s.subject_no=ctd.TT_subno and  ct.TT_ClassPK=ctd.TT_ClassFk and ctd.TT_Day = do.TT_Day_DayorderPK and TT_lastRec='1'  and TT_staffcode='" + staffCode + "' and ctd.TT_AlterDate='" + ttAlterDate + "'");
        }
        catch { dtStaffTt.Clear(); }
        return dtStaffTt;
    }

    //Get time table details for other staffs
    private DataTable getOtherStaffAlterTT(string ttAlterDate, List<int> lstBatchYear = null, List<Int64> lstDegreeCode = null, List<byte> lstSemester = null, List<string> lstCollegeCode = null)
    {
        DataTable dtStaffTt = new DataTable();
        try
        {
            string batchYear = string.Empty;
            string degreeCode = string.Empty;
            string semester = string.Empty;
            string collegeCode1 = collegeCode;
            if (lstCollegeCode != null)
            {
                collegeCode1 = string.Join("','", lstCollegeCode.ToArray());
                if (!string.IsNullOrEmpty(collegeCode1))
                    collegeCode1 = " and TT_colCode in('" + collegeCode1 + "')";
            }
            else
            {
                if (!string.IsNullOrEmpty(collegeCode1))
                    collegeCode1 = " and TT_colCode in('" + collegeCode1 + "')";
            }
            if (lstBatchYear != null)
            {
                batchYear = string.Join("','", lstBatchYear.ToArray());
                if (!string.IsNullOrEmpty(batchYear))
                    batchYear = " and TT_batchyear in('" + batchYear + "')";
            }
            if (lstDegreeCode != null)
            {
                degreeCode = string.Join("','", lstDegreeCode.ToArray());
                if (!string.IsNullOrEmpty(degreeCode))
                    degreeCode = " and TT_degCode in('" + degreeCode + "')";
            }
            if (lstSemester != null)
            {
                semester = string.Join("','", lstSemester.ToArray());
                if (!string.IsNullOrEmpty(semester))
                    semester = " and TT_sem in('" + semester + "')";
            }

            dtStaffTt = dirAcc.selectDataTable(" select distinct TT_ClassPK,TT_name,TT_colCode,TT_batchyear,TT_degCode,TT_sem,TT_sec,Convert(varchar(10),TT_AlterDate,101) as TT_AlterDate,TT_date,TT_subno,TT_Staffcode,TT_Day,TT_Hour,Daydiscription,c.Course_Name,dt.Dept_Name,s.subject_name,subject_code,dt.dept_acronym,isnull(ss.ElectivePap,'0') as Elective,isnull(ss.Lab,'0') as Lab,(select Room_Name from room_detail where roompk=TT_Room) as RoomName from TT_ClassTimetable ct,TT_AlterTimetableDet ctd,TT_Day_Dayorder do,subject s,sub_sem ss,Degree d,Department dt,course c,Registration r,syllabus_master sm where sm.syll_code=s.syll_code and sm.syll_code=ss.syll_code and sm.Batch_Year = r.Batch_Year and sm.Degree_Code=r.degree_code and sm.semester = TT_sem and r.degree_code=d.Degree_Code and r.Batch_Year=TT_batchyear and r.Current_Semester=TT_sem and r.degree_code=TT_degCode and r.CC=0 and r.DelFlag='0' and r.Exam_Flag<>'debar' and s.subType_no=ss.subType_no and s.syll_code=ss.syll_code and d.Degree_Code=ct.TT_degCode and c.Course_Id=d.Course_Id and dt.Dept_Code=d.Dept_Code and s.subject_no=ctd.TT_subno and  ct.TT_ClassPK=ctd.TT_ClassFk and ctd.TT_Day = do.TT_Day_DayorderPK and TT_lastRec='1' and ctd.TT_AlterDate='" + ttAlterDate + "' and TT_staffcode<>'" + staffCode + "' " + collegeCode1 + semester + batchYear + degreeCode + " "); // and TT_staffcode='" + staffCode + "'  and TT_colCode='" + collegeCode + "' 
        }
        catch { dtStaffTt.Clear(); }
        return dtStaffTt;
    }

    //Get alloted degree,batch,sem for staffs
    private DataTable getStfAlotBatDegSem()
    {
        DataTable dtStfBatDegSem = new DataTable();
        try
        {
            dtStfBatDegSem = dirAcc.selectDataTable("select distinct TT_batchyear,TT_degCode,TT_sem,TT_date from TT_ClassTimetable ct,TT_ClassTimetableDet ctd where ct.TT_ClassPK=ctd.TT_ClassFk and TT_lastRec='1' and TT_colCode='" + collegeCode + "' and TT_staffcode='" + staffCode + "'");
            //and ctd.TT_Day='1' and ctd.TT_Hour='1'
        }
        catch { dtStfBatDegSem.Clear(); }
        return dtStfBatDegSem;
    }

    //Get staff alloted batch
    private List<int> getStfAllotBatch()
    {
        List<int> lstBatch = new List<int>();
        try
        {
            DataTable dtBatch = getStfAlotBatDegSem().DefaultView.ToTable(true, "TT_batchyear");
            lstBatch = dtBatch.AsEnumerable().Select(r => r.Field<int>("TT_batchyear")).ToList<int>();
        }
        catch { lstBatch.Clear(); }
        return lstBatch;
    }

    //Get staff alloted degree
    private List<Int64> getStfAllotDegree()
    {
        List<Int64> lstDeg = new List<Int64>();
        try
        {
            DataTable dtDeg = getStfAlotBatDegSem().DefaultView.ToTable(true, "TT_degCode");
            lstDeg = dtDeg.AsEnumerable().Select(r => r.Field<Int64>("TT_degCode")).ToList<Int64>();
        }
        catch { lstDeg.Clear(); }
        return lstDeg;
    }

    //Get staff alloted semester
    private List<byte> getStfAllotSem()
    {
        List<byte> lstSem = new List<byte>();
        try
        {
            DataTable dtSem = getStfAlotBatDegSem().DefaultView.ToTable(true, "TT_sem");
            lstSem = dtSem.AsEnumerable().Select(r => r.Field<byte>("TT_sem")).ToList<byte>();
        }
        catch { lstSem.Clear(); }
        return lstSem;
    }

    //Get day values for access from time table
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

    //Get maximum number of periods
    private byte getMaxPeriods()
    {
        byte period = 0;
        try
        {
            period = Convert.ToByte(dirAcc.selectScalarInt("select max(No_of_hrs_per_day) as noofhours from PeriodAttndSchedule "));
        }
        catch { }
        return period;
    }

    //Mark Attendance Screen
    protected void lnkAttMark(object sender, EventArgs e)
    {
        try
        {
            chkAbsEntry.Checked = false;
            chkAbsEntry_OnCheckChanged(sender, e);

            #region Basic Data and Display

            clearAttMarkDet();

            LinkButton lnkSelected = (LinkButton)sender;
            string rowIndxS = lnkSelected.UniqueID.ToString().Split('$')[3].Replace("ctl", string.Empty);
            int rowIndx = Convert.ToInt32(rowIndxS) - 2;
            string colIndxS = lnkSelected.UniqueID.ToString().Split('$')[4].Replace("lnkPeriod_", string.Empty);
            int colIndx = Convert.ToInt32(colIndxS);

            Label lblData = (Label)gridTimeTable.Rows[rowIndx].FindControl("lblPeriod_" + colIndx);
            Label lblCurDate = (Label)gridTimeTable.Rows[rowIndx].FindControl("lblDate");
            Label lblDateDisp = (Label)gridTimeTable.Rows[rowIndx].FindControl("lblDateDisp");
            Label lblDayVal = (Label)gridTimeTable.Rows[rowIndx].FindControl("lblDayVal");
            Label lblTTclasspk = (Label)gridTimeTable.Rows[rowIndx].FindControl("lblTT_" + colIndx);

            DateTime dtAttendanceDate = new DateTime();
            DateTime.TryParseExact(lblCurDate.Text.Trim(), "MM/dd/yyyy", null, DateTimeStyles.None, out dtAttendanceDate);
            if (dtAttendanceDate > DateTime.Now)
            {
                if (Session["dt"] != null) Session.Remove("dt");
                ScriptManager.RegisterStartupScript(this, typeof(Page), UniqueID, "alert('You cannot Mark Attendance to this Day/Hour attendance due to Future Date.')", true);
                return;
            }
            string labDay = string.Empty;
            switch (lblDayVal.Text)
            {
                case "1":
                    labDay = "mon";
                    break;
                case "2":
                    labDay = "tue";
                    break;
                case "3":
                    labDay = "wed";
                    break;
                case "4":
                    labDay = "thu";
                    break;
                case "5":
                    labDay = "fri";
                    break;
                case "6":
                    labDay = "sat";
                    break;
                case "7":
                    labDay = "sun";
                    break;
            }
            byte hrLock = 0;
            byte dayLock = 0;
            DataTable dtStudData = new DataTable();
            string[] subDets = lblData.Text.Split(';');
            string[] subDispDets = lnkSelected.Text.Split(';');
            string[] classttPks = lblTTclasspk.Text.Split(';'); // itsWrong
            Hashtable hat=new Hashtable();
            DataTable dtSimilarStudnt = new DataTable();
            bool genSlectQ = false;
            for (int subI = 0; subI < subDets.Length; subI++)
            {
                string[] ttData = subDets[subI].Split('#');
                string[] ttDispData = subDispDets[subI].Split('$');
                string[] ttClasspk = classttPks[subI].Split('$');

                ListItem ls = new ListItem(subDispDets[subI], subDets[subI] + "@" + classttPks[subI]);
                ddlMultiSub.Items.Add(ls);
                if (subDets.Length > 1)
                {
                    divMultiSubj.Visible = true;
                }

                string batch = ttData[2];
                string degcode = ttData[3];
                string sem = ttData[4];
                string sec = ttData[5];
                string subno = ttData[1];
                string staffcode = ttData[6];
                string course = ttDispData[2];
                string deptname = ttDispData[3];
                string subname = ttDispData[0];
                string subcode = ttData[0];
                string deptacr = ttDispData[3];
                string elective = ttData[7].Trim();
                string lab = ttData[8].Trim();

                lblBatch.Text += batch + ",";
                lblCourseDisp.Text += course + " " + deptacr + ",";
                lblDegCode.Text = degcode + ",";
                lblSubname.Text += subname + ",";
                lblSubno.Text += subno + ",";
                lblSubCode.Text += subcode + ",";
                lblSem.Text += sem + ",";
                lblSec.Text += sec + ",";
                lblHourFk.Text = colIndx.ToString();
                lblDayFK.Text = lblDayVal.Text;
                lblDate.Text = lblDateDisp.Text.Replace("<br>", string.Empty) + " Hour : " + colIndx;

                #region Day Lock for attendance
                bool daylock = daycheck(Convert.ToDateTime(lblCurDate.Text), degcode, sem);
                if (!daylock)
                {
                    dayLock++;
                    break;
                }
                #endregion

                #region Hour Lock For attendance
                bool hrlock = Hour_lock(degcode, batch, sem, "Period " + colIndx.ToString(), sec);
                if (hrlock)
                {
                    hrLock++;
                    break;
                }
                

                #endregion

                //Rajkumar Modified on KONGU Performance

                //if (genSlectQ == false && lab.ToLower() == "false" && elective.ToLower() == "false" && Convert.ToString(ttClasspk[1]).ToLower() == "false")
                //{
                //   dtSimilarStudnt=(studinfo.StudentData(staffcode, lblCurDate.Text));

                //}
                //if (dtSimilarStudnt.Rows.Count > 0)
                //{
                //    bool staffSelector = false;
                //    string qryStudeStaffSelector1 = string.Empty;
                //    string minimumabsentsms = da.GetFunction("select LinkValue from New_InsSettings where LinkName='Studnet Staff Selector' and college_code='" + collegeCode + "'");
                //    string[] splitminimumabsentsms = minimumabsentsms.Split('-');
                //    if (splitminimumabsentsms.Length == 2)
                //    {
                //        int batchyearsetting = 0;
                //        int.TryParse(Convert.ToString(splitminimumabsentsms[1]).Trim(), out batchyearsetting);
                //        if (splitminimumabsentsms[0].ToString() == "1")
                //        {
                //            if (Convert.ToInt32(batch.ToString()) >= batchyearsetting)
                //            {
                //                staffSelector = true;
                //            }
                //        }
                //    }
                //    else if (splitminimumabsentsms.Length > 0)
                //    {
                //        if (splitminimumabsentsms[0].ToString() == "1")
                //        {
                //            staffSelector = true;
                //        }
                //    }
                //    if (staffSelector )
                //    {
                //        //qryStudeStaffSelector = " and sc.staffcode like '%" + staffcode + "%'";
                //        qryStudeStaffSelector1 = " and staffcode=''" + staffcode + "''";
                //    }
                //    DataTable student = new DataTable();
                //    if (!string.IsNullOrEmpty(sec))
                //    {
                //        dtSimilarStudnt.DefaultView.RowFilter = "college_code='" + collegeCode + "' and Batch_Year='" + batch + "' and degree_code='" + degcode + "' and Current_Semester='" + sem + "' and regSec='" + sec + "' and staffSec='" + sec + "' and subject_no='" + subno + "' " + qryStudeStaffSelector1 + "";
                //        student = dtSimilarStudnt.DefaultView.ToTable();
                //    }
                //    else
                //    {
                //        dtSimilarStudnt.DefaultView.RowFilter = "college_code='" + collegeCode + "' and Batch_Year='" + batch + "' and degree_code='" + degcode + "' and Current_Semester='" + sem + "' and subject_no='" + subno + "' " + qryStudeStaffSelector1 + "";
                //        student = dtSimilarStudnt.DefaultView.ToTable();
                //    }
                //    if (student.Rows.Count > 0)
                //    {
                //        dtStudData.Merge(student);
                //    }
                //    else
                //    {
                //        dtStudData.Merge(studinfo.getStudentData(collegeCode, batch, degcode, sem, sec, subno, staffcode, elective, lab, labDay, colIndx.ToString(), ttClasspk[0], ttClasspk[1], lblCurDate.Text));
                //    }
                //}
                //else
                //{
                    dtStudData.Merge(studinfo.getStudentData(collegeCode, batch, degcode, sem, sec, subno, staffcode, elective, lab, labDay, colIndx.ToString(), ttClasspk[0], ttClasspk[1], lblCurDate.Text));
                //}
               
            }

            //Day Lock  & Hour Lock Check
            if (dayLock > 0 || hrLock > 0)
            {
                if (Session["dt"] != null) Session.Remove("dt");
                ScriptManager.RegisterStartupScript(this, typeof(Page), UniqueID, "alert('You cannot edit this Day/Hour attendance due to security reasons. Get permission from PRINCIPAL to update the attendance')", true);
                return;
            }

            lblSubname.Text = lblSubname.Text.Trim(',');
            dtStudData = dtStudData.DefaultView.ToTable(true);
            List<Decimal> appNos = dtStudData.AsEnumerable().Select(r => r.Field<Decimal>("app_no")).ToList<Decimal>();
            string appNoString = string.Join(",", appNos);


            bool isShowRollNo = ColumnHeaderVisiblity(0);
            bool isShowRegNo = ColumnHeaderVisiblity(1);
            bool isShowAdmissionNo = ColumnHeaderVisiblity(2);
            bool isShowStudentType = ColumnHeaderVisiblity(3);
            bool isShowApplicationNo = ColumnHeaderVisiblity(4);
            bool isShowAttendCount = IsShowStudentAttendanceCount();
            bool isShowReason = IsShowAttendanceReason();
            bool isShowOnlyPresent = IsShowPresentAbsentOnly();

            Dictionary<string, string> dicAttSaved = attLoadAtt(appNoString, lblCurDate.Text, colIndx.ToString());
            bool saveorupdate = false;
            if (dtStudData.Rows.Count > 0)
            {
                WebService web = new WebService();
                ArrayList notarray = new ArrayList();
                notarray = getNotArrayLeaveCodes();
                Hashtable absent_calcflag = getAbsentAttendanceTypes();

                DataTable dtDispStud = new DataTable();
                dtDispStud.Columns.Add("app_no");
                dtDispStud.Columns.Add("Application No");
                dtDispStud.Columns.Add("Roll No");
                dtDispStud.Columns.Add("Register No");
                dtDispStud.Columns.Add("Admission No");
                dtDispStud.Columns.Add("Student Name");
                dtDispStud.Columns.Add("Resident");
                dtDispStud.Columns.Add("Con (Attnd)");
                dtDispStud.Columns.Add("AttendanceLock");

                DataTable dtAttndType = getAttendanceTypes();
                if (isShowOnlyPresent)
                {
                    dtDispStud.Columns.Add("Attendance");
                }
                else
                {
                    foreach (DataRow drAttType in dtAttndType.Rows)
                    {
                        if (!dtDispStud.Columns.Contains(Convert.ToString(drAttType["DispText"]).Trim()))
                            dtDispStud.Columns.Add(Convert.ToString(drAttType["DispText"]).Trim());
                    }
                }
                dtDispStud.Columns.Add("Reason");
                string[] date = lblCurDate.Text.Split('/');
                byte day = Convert.ToByte(date[1]);
                byte month = Convert.ToByte(date[0]);
                int year = Convert.ToInt32(date[2]);
                string monthyear = ((year * 12) + month).ToString();
                string attColumn = "[d" + day + "d" + colIndx.ToString() + "]";
                foreach (DataRow drStud in dtStudData.Rows)
                {
                    DataRow drDisp = dtDispStud.NewRow();
                    string appNo = drStud["app_no"].ToString().Trim();
                    drDisp["app_no"] = appNo;
                    drDisp["Application No"] = drStud["app_formno"];
                    drDisp["Roll No"] = drStud["Roll_No"];
                    drDisp["Register No"] = drStud["Reg_No"];
                    drDisp["Admission No"] = drStud["Roll_Admit"];
                    drDisp["Student Name"] = drStud["stud_name"];
                    drDisp["Resident"] = drStud["Resident"];

                    #region Reason

                    string attReas = dirAcc.selectScalarString("select isnull(" + attColumn + ",'') from Attendance_withreason where roll_no='" + drStud["Roll_No"].ToString() + "' and month_year='" + monthyear + "' and AtWr_App_no ='" + appNo + "' and AttWr_CollegeCode='" + collegeCode + "'");
                    drDisp["Reason"] = attReas;

                    #endregion

                    #region Conduct Hours

                    int absent_hour = 0;
                    int total_conduct_hour = 0;

                    string degCode = string.Empty;
                    string semester = string.Empty;
                    string batch = string.Empty;
                    string regno = string.Empty;
                    string admno = string.Empty;
                    getStudentDetails(appNo, ref degCode, ref semester, ref batch, ref regno, ref admno);
                    string[] semStartDate = getSemStartDate(degCode, semester, batch).Split('/');
                    byte sday = Convert.ToByte(semStartDate[0]);
                    byte smonth = Convert.ToByte(semStartDate[1]);
                    int syear = Convert.ToInt32(semStartDate[2]);
                    int smonthyear = ((year * 12) + month);

                    string value_return = web.coundected_hour(Convert.ToInt32(monthyear), smonthyear, drStud["Roll_No"].ToString(), absent_calcflag, notarray);
                    if (value_return == "Empty")
                    {
                        total_conduct_hour = 1;
                        absent_hour = 1;
                    }
                    else
                    {
                        string[] splitvalue = value_return.Split('-');
                        if (splitvalue.Length > 0)
                        {
                            if (splitvalue[0].ToString() != "")
                            {
                                total_conduct_hour = Convert.ToInt32(splitvalue[0]);
                                total_conduct_hour++;
                            }
                            else
                            {
                                total_conduct_hour++;
                            }
                            if (splitvalue[1].ToString() != "")
                            {
                                absent_hour = Convert.ToInt32(splitvalue[1]);
                                absent_hour++;
                            }
                            else
                            {
                                absent_hour++;
                            }
                        }
                    }
                    drDisp["Con (Attnd)"] = total_conduct_hour + " (" + absent_hour + ")";

                    #endregion

                    if (dicAttSaved.ContainsKey(appNo) && !string.IsNullOrEmpty(dicAttSaved[appNo]))
                    {
                        string dispText = dicAttSaved[appNo];
                        if (isShowOnlyPresent)
                        {
                            string qry = "select CalcFlag from AttMasterSetting where DispText='" + dicAttSaved[appNo] + "'";
                            string calCode = dirAcc.selectScalarString(qry);
                            switch (calCode)
                            {
                                case "0":
                                    if (dispText.Trim().ToLower() == "p")
                                        drDisp["Attendance"] = 1;
                                    break;
                                case "1":
                                    drDisp["Attendance"] = 0;
                                    break;
                            }
                        }
                        else
                        {
                            if (dtDispStud.Columns.Contains(dicAttSaved[appNo]))
                                drDisp[dicAttSaved[appNo]] = 1;
                        }
                        switch (dispText.Trim().ToLower())
                        {
                            case "od":
                                drDisp["AttendanceLock"] = 1;
                                break;
                            default:
                                drDisp["AttendanceLock"] = 0;
                                break;
                        }
                        saveorupdate = true;
                    }
                    else
                    {
                        drDisp["AttendanceLock"] = 0;
                    }
                    dtDispStud.Rows.Add(drDisp);
                }

                if (saveorupdate)
                {
                    btnMarkAttSave.Visible = false;
                    btnMarkAttUpdate.Visible = true;
                }
                else
                {
                    btnMarkAttSave.Visible = true;
                    btnMarkAttUpdate.Visible = false;
                }
                markDiv.Visible = true;
                lblPresentCol.Text = Convert.ToString(dtDispStud.Columns.Count);
                lblReasonCol.Text = Convert.ToString(dtDispStud.Columns.Count);
                gridMarkAttnd.DataSource = dtDispStud;
                gridMarkAttnd.DataBind();
                Session["dt"] = dtDispStud;
            }
            else
            {
                WebService web = new WebService();
                ArrayList notarray = new ArrayList();
                notarray = getNotArrayLeaveCodes();
                Hashtable absent_calcflag = getAbsentAttendanceTypes();

                DataTable dtDispStud = new DataTable();
                dtDispStud.Columns.Add("app_no");
                dtDispStud.Columns.Add("Application No");
                dtDispStud.Columns.Add("Roll No");
                dtDispStud.Columns.Add("Register No");
                dtDispStud.Columns.Add("Admission No");
                dtDispStud.Columns.Add("Student Name");
                dtDispStud.Columns.Add("Resident");
                dtDispStud.Columns.Add("Con (Attnd)");
                dtDispStud.Columns.Add("AttendanceLock");

                DataTable dtAttndType = getAttendanceTypes();
                //foreach (DataRow drAttType in dtAttndType.Rows)
                //{
                //    dtDispStud.Columns.Add(Convert.ToString(drAttType["DispText"]));
                //}
                if (isShowOnlyPresent)
                {
                    dtDispStud.Columns.Add("Attendance");
                }
                else
                {
                    foreach (DataRow drAttType in dtAttndType.Rows)
                    {
                        if (!dtDispStud.Columns.Contains(Convert.ToString(drAttType["DispText"]).Trim()))
                            dtDispStud.Columns.Add(Convert.ToString(drAttType["DispText"]).Trim());
                    }
                }
                dtDispStud.Columns.Add("Reason");
                markDiv.Visible = true;
                lblPresentCol.Text = Convert.ToString(dtDispStud.Columns.Count);
                lblReasonCol.Text = Convert.ToString(dtDispStud.Columns.Count);
                gridMarkAttnd.DataSource = dtDispStud;
                gridMarkAttnd.DataBind();
                Session["dt"] = dtDispStud;
                //Session.Remove("dt");
            }

            #endregion

            #region Column InVisible

            int countCol = 0;
            int reasonCol = 0;
            int rowVisible = 0;
            for (int row = 0; row < gridMarkAttnd.Rows.Count; row++)
            {
                countCol = 0;
                rowVisible = 0;
                reasonCol = gridMarkAttnd.Rows[row].Cells.Count - 1;
                string locked = gridMarkAttnd.Rows[row].Cells[9].Text;
                int isAttendanceLocked = 0;
                int.TryParse(locked, out isAttendanceLocked);
                if (isAttendanceLocked == 1)
                {
                    gridMarkAttnd.Rows[row].Enabled = false;
                    gridMarkAttnd.Rows[row].BackColor = Color.DarkViolet;
                }
                for (int col = 0; col < gridMarkAttnd.Rows[row].Cells.Count; col++)
                {
                    bool isVisible = true;
                    switch (col)
                    {
                        case 1:
                            isVisible = false;
                            break;
                        case 2:
                            isVisible = isShowApplicationNo;
                            break;
                        case 3:
                            isVisible = isShowRollNo;
                            break;
                        case 4:
                            isVisible = isShowRegNo;
                            break;
                        case 5:
                            isVisible = isShowAdmissionNo;
                            break;
                        case 7:
                            isVisible = isShowStudentType;
                            break;
                        case 8:
                            isVisible = isShowAttendCount;
                            break;
                        case 9:
                            isVisible = false;
                            break;
                        default:
                            if (reasonCol == col)
                            {
                                isVisible = isShowReason;
                            }
                            break;
                    }

                    if (isVisible)
                    {
                        rowVisible++;
                        if (col < 10)
                            countCol++;
                    }
                    TextBox txt = new TextBox();
                    CheckBox chkLeaveCode = new CheckBox();
                    if (col >= 10 && col < reasonCol)
                    {
                        //if (row == 1)
                        //{
                        //    txt = new TextBox();
                        //    txt = (TextBox)gridMarkAttnd.HeaderRow.Cells[col - 1].FindControl("col" + (col - 1));
                        //    txt.Attributes.Add("onclick", "checkvalueHeader(" + (col - countCol) + "," + countCol + ")");
                        //}
                        //gridMarkAttnd.HeaderRow.Cells[col].Attributes.Add("onclick", "checkvalueHeader(" + (col - countCol) + "," + countCol + ")");
                        txt = new TextBox();
                        txt = (TextBox)gridMarkAttnd.HeaderRow.Cells[col].FindControl("col" + col);
                        txt.Attributes.Add("onclick", "checkvalueHeader(" + (col - countCol) + "," + countCol + ")");
                        chkLeaveCode = new CheckBox();
                        chkLeaveCode = (CheckBox)gridMarkAttnd.Rows[row].Cells[col].FindControl("chk_" + col);
                        chkLeaveCode.Enabled = (isAttendanceLocked == 1) ? false : true;
                        chkLeaveCode.Attributes.Add("onclick", "Check_Click(this,'" + (row + 1) + "'," + (rowVisible - 1) + "," + countCol + ");");
                    }
                    gridMarkAttnd.Rows[row].Cells[col].Visible = isVisible;
                    gridMarkAttnd.HeaderRow.Cells[col].Visible = isVisible;
                }
            }
            lblResonColEx.Text = "0";
            if (isShowReason)
            {
                lblResonColEx.Text = "1";
            }
            lblPresentOnly.Text = "0";
            if (isShowOnlyPresent)
                lblPresentOnly.Text = "1";
            lblPresentCol.Text = Convert.ToString(countCol).Trim();
            lblReasonCol.Text = Convert.ToString(reasonCol + 1).Trim();

            #endregion
        }
        catch (Exception ex)
        {
            da.sendErrorMail(ex, ((Session["collegecode"] != null) ? Convert.ToString(Session["collegecode"]).Trim() : "13"), System.IO.Path.GetFileName(Request.Url.AbsolutePath).ToString());
            markDiv.Visible = false;
            ScriptManager.RegisterStartupScript(this, typeof(Page), UniqueID, "alert('Please try later')", true);
        }
    }

    //Load Students for Particular subject
    protected void ddlMultiSub_OnChanged(object sender, EventArgs e)
    {
        try
        {
            string[] date = lblDate.Text.Split('(')[0].Trim().Split('/');
            byte day = Convert.ToByte(date[0]);
            byte month = Convert.ToByte(date[1]);
            int year = Convert.ToInt32(date[2]);
            string curDate = month + "/" + day + "/" + year;

            string hour = lblDate.Text.Split(':')[1].Trim();

            string labDay = string.Empty;
            switch (lblDayFK.Text)
            {
                case "1":
                    labDay = "mon";
                    break;
                case "2":
                    labDay = "tue";
                    break;
                case "3":
                    labDay = "wed";
                    break;
                case "4":
                    labDay = "thu";
                    break;
                case "5":
                    labDay = "fri";
                    break;
                case "6":
                    labDay = "sat";
                    break;
                case "7":
                    labDay = "sun";
                    break;
            }
            byte hrLock = 0;
            byte dayLock = 0;
            DataTable dtStudData = new DataTable();
            StringBuilder sbsubDet = new StringBuilder();
            StringBuilder sbsubDetDisp = new StringBuilder();
            StringBuilder sbclassPks = new StringBuilder();
            if (ddlMultiSub.SelectedIndex == 0)
            {
                bool firsttime = true;
                foreach (ListItem lst in ddlMultiSub.Items)
                {
                    if (firsttime)
                    {
                        firsttime = false;
                        continue;
                    }
                    sbsubDetDisp.Append(lst.Text + ";");
                    sbsubDet.Append(lst.Value.Split('@')[0] + ";");
                    sbclassPks.Append(lst.Value.Split('@')[1] + ";");
                }
            }
            else
            {
                sbsubDetDisp.Append(ddlMultiSub.SelectedItem.Text + ";");
                sbsubDet.Append(ddlMultiSub.SelectedItem.Value.Split('@')[0] + ";");
                sbclassPks.Append(ddlMultiSub.SelectedItem.Value.Split('@')[1] + ";");
            }

            string[] subDets = sbsubDet.ToString().TrimEnd(';').Split(';');
            string[] subDispDets = sbsubDetDisp.ToString().TrimEnd(';').Split(';');
            string[] classttPks = sbclassPks.ToString().TrimEnd(';').Split(';');

            for (int subI = 0; subI < subDets.Length; subI++)
            {
                string[] ttData = subDets[subI].Split('#');
                string[] ttDispData = subDispDets[subI].Split('$');
                string[] ttClasspk = classttPks[subI].Split('$');

                string batch = ttData[2];
                string degcode = ttData[3];
                string sem = ttData[4];
                string sec = ttData[5];
                string subno = ttData[1];
                string staffcode = ttData[6];
                string course = ttDispData[2];
                string deptname = ttDispData[3];
                string subname = ttDispData[0];
                string subcode = ttData[0];
                string deptacr = ttDispData[3];
                string elective = ttData[7].Trim();
                string lab = ttData[8].Trim();

                #region Day Lock for attendance
                bool daylock = daycheck(Convert.ToDateTime(curDate), degcode, sem);
                if (!daylock)
                {
                    dayLock++;
                    break;
                }
                #endregion

                #region Hour Lock For attendance
                bool hrlock = Hour_lock(degcode, batch, sem, "Period " + hour, sec);
                if (hrlock)
                {
                    hrLock++;
                    break;
                }
                #endregion

                dtStudData.Merge(studinfo.getStudentData(collegeCode, batch, degcode, sem, sec, subno, staffcode, elective, lab, labDay, hour.ToString(), ttClasspk[0], ttClasspk[1], curDate));
            }

            bool isShowRollNo = ColumnHeaderVisiblity(0);
            bool isShowRegNo = ColumnHeaderVisiblity(1);
            bool isShowAdmissionNo = ColumnHeaderVisiblity(2);
            bool isShowStudentType = ColumnHeaderVisiblity(3);
            bool isShowApplicationNo = ColumnHeaderVisiblity(4);
            bool isShowAttendCount = IsShowStudentAttendanceCount();
            bool isShowReason = IsShowAttendanceReason();
            bool isShowOnlyPresent = IsShowPresentAbsentOnly();
            //Day Lock  & Hour Lock Check
            if (dayLock > 0 || hrLock > 0)
            {
                if (Session["dt"] != null) Session.Remove("dt");
                ScriptManager.RegisterStartupScript(this, typeof(Page), UniqueID, "alert('You cannot edit this Day/Hour attendance due to security reasons. Get permission from PRINCIPAL to update the attendance')", true);
                return;
            }

            lblSubname.Text = lblSubname.Text.Trim(',');
            dtStudData = dtStudData.DefaultView.ToTable(true);
            List<Decimal> appNos = dtStudData.AsEnumerable().Select(r => r.Field<Decimal>("app_no")).ToList<Decimal>();
            string appNoString = string.Join(",", appNos);

            Dictionary<string, string> dicAttSaved = attLoadAtt(appNoString, curDate, hour.ToString());
            bool saveorupdate = false;
            if (dtStudData.Rows.Count > 0)
            {
                WebService web = new WebService();
                ArrayList notarray = new ArrayList();
                notarray = getNotArrayLeaveCodes();
                Hashtable absent_calcflag = getAbsentAttendanceTypes();

                DataTable dtDispStud = new DataTable();
                dtDispStud.Columns.Add("app_no");
                dtDispStud.Columns.Add("Application No");
                dtDispStud.Columns.Add("Roll No");
                dtDispStud.Columns.Add("Register No");
                dtDispStud.Columns.Add("Admission No");
                dtDispStud.Columns.Add("Student Name");
                dtDispStud.Columns.Add("Resident");
                dtDispStud.Columns.Add("Con (Attnd)");
                dtDispStud.Columns.Add("AttendanceLock");

                DataTable dtAttndType = getAttendanceTypes();
                if (isShowOnlyPresent)
                {
                    dtDispStud.Columns.Add("Attendance");
                }
                else
                {
                    foreach (DataRow drAttType in dtAttndType.Rows)
                    {
                        if (!dtDispStud.Columns.Contains(Convert.ToString(drAttType["DispText"]).Trim()))
                            dtDispStud.Columns.Add(Convert.ToString(drAttType["DispText"]).Trim());
                    }
                }
                dtDispStud.Columns.Add("Reason");

                string monthyear = ((year * 12) + month).ToString();
                string attColumn = "[d" + day + "d" + hour.ToString() + "]";

                foreach (DataRow drStud in dtStudData.Rows)
                {
                    DataRow drDisp = dtDispStud.NewRow();
                    string appNo = drStud["app_no"].ToString().Trim();
                    drDisp["app_no"] = appNo;
                    drDisp["Application No"] = drStud["app_formno"];
                    drDisp["Roll No"] = drStud["Roll_No"];
                    drDisp["Register No"] = drStud["Reg_No"];
                    drDisp["Admission No"] = drStud["Roll_Admit"];
                    drDisp["Student Name"] = drStud["stud_name"];
                    drDisp["Resident"] = drStud["Resident"];

                    #region Reason
                    string attReas = dirAcc.selectScalarString("select isnull(" + attColumn + ",'') from Attendance_withreason where roll_no='" + drStud["Roll_No"].ToString() + "' and month_year='" + monthyear + "' and AtWr_App_no ='" + appNo + "' and AttWr_CollegeCode='" + collegeCode + "'");
                    drDisp["Reason"] = attReas;
                    #endregion

                    #region Conduct Hours

                    int absent_hour = 0;
                    int total_conduct_hour = 0;

                    string degCode = string.Empty;
                    string semester = string.Empty;
                    string batch = string.Empty;
                    string regno = string.Empty;
                    string admno = string.Empty;
                    getStudentDetails(appNo, ref degCode, ref semester, ref batch, ref regno, ref admno);
                    string[] semStartDate = getSemStartDate(degCode, semester, batch).Split('/');
                    byte sday = Convert.ToByte(semStartDate[0]);
                    byte smonth = Convert.ToByte(semStartDate[1]);
                    int syear = Convert.ToInt32(semStartDate[2]);
                    int smonthyear = ((year * 12) + month);

                    string value_return = web.coundected_hour(Convert.ToInt32(monthyear), smonthyear, drStud["Roll_No"].ToString(), absent_calcflag, notarray);
                    if (value_return == "Empty")
                    {
                        total_conduct_hour = 1;
                        absent_hour = 1;
                    }
                    else
                    {
                        string[] splitvalue = value_return.Split('-');
                        if (splitvalue.Length > 0)
                        {
                            if (splitvalue[0].ToString() != "")
                            {
                                total_conduct_hour = Convert.ToInt32(splitvalue[0]);
                                total_conduct_hour++;
                            }
                            else
                            {
                                total_conduct_hour++;
                            }
                            if (splitvalue[1].ToString() != "")
                            {
                                absent_hour = Convert.ToInt32(splitvalue[1]);
                                absent_hour++;
                            }
                            else
                            {
                                absent_hour++;
                            }
                        }
                    }
                    drDisp["Con (Attnd)"] = total_conduct_hour + " (" + absent_hour + ")";
                    #endregion

                    if (dicAttSaved.ContainsKey(appNo) && !string.IsNullOrEmpty(dicAttSaved[appNo]))
                    {
                        string dispText = dicAttSaved[appNo];
                        if (isShowOnlyPresent)
                        {
                            string qry = "select CalcFlag from AttMasterSetting where DispText='" + dicAttSaved[appNo] + "'";
                            string calCode = dirAcc.selectScalarString(qry);
                            switch (calCode)
                            {
                                case "0":
                                    if (dispText.Trim().ToLower() == "p")
                                        drDisp["Attendance"] = 1;
                                    break;
                                case "1":
                                    drDisp["Attendance"] = 0;
                                    break;
                            }
                        }
                        else
                        {
                            if (dtDispStud.Columns.Contains(dicAttSaved[appNo]))
                                drDisp[dicAttSaved[appNo]] = 1;
                        }
                        switch (dispText.Trim().ToLower())
                        {
                            case "od":
                                drDisp["AttendanceLock"] = 1;
                                break;
                            default:
                                drDisp["AttendanceLock"] = 0;
                                break;
                        }
                        saveorupdate = true;
                    }
                    else
                    {
                        drDisp["AttendanceLock"] = 0;
                    }
                    dtDispStud.Rows.Add(drDisp);
                }

                if (saveorupdate)
                {
                    btnMarkAttSave.Visible = false;
                    btnMarkAttUpdate.Visible = true;
                }
                else
                {
                    btnMarkAttSave.Visible = true;
                    btnMarkAttUpdate.Visible = false;
                }
                markDiv.Visible = true;
                gridMarkAttnd.DataSource = dtDispStud;
                gridMarkAttnd.DataBind();
                Session["dt"] = dtDispStud;
            }
            else
            {
                WebService web = new WebService();
                ArrayList notarray = new ArrayList();
                notarray = getNotArrayLeaveCodes();
                Hashtable absent_calcflag = getAbsentAttendanceTypes();

                DataTable dtDispStud = new DataTable();
                dtDispStud.Columns.Add("app_no");
                dtDispStud.Columns.Add("Application No");
                dtDispStud.Columns.Add("Roll No");
                dtDispStud.Columns.Add("Register No");
                dtDispStud.Columns.Add("Admission No");
                dtDispStud.Columns.Add("Student Name");
                dtDispStud.Columns.Add("Resident");
                dtDispStud.Columns.Add("Con (Attnd)");
                dtDispStud.Columns.Add("AttendanceLock");

                DataTable dtAttndType = getAttendanceTypes();
                if (isShowOnlyPresent)
                {
                    dtDispStud.Columns.Add("Attendance");
                }
                else
                {
                    foreach (DataRow drAttType in dtAttndType.Rows)
                    {
                        if (!dtDispStud.Columns.Contains(Convert.ToString(drAttType["DispText"]).Trim()))
                            dtDispStud.Columns.Add(Convert.ToString(drAttType["DispText"]).Trim());
                    }
                }
                dtDispStud.Columns.Add("Reason");
                markDiv.Visible = true;
                lblPresentCol.Text = Convert.ToString(dtDispStud.Columns.Count);
                lblReasonCol.Text = Convert.ToString(dtDispStud.Columns.Count);
                gridMarkAttnd.DataSource = dtDispStud;
                gridMarkAttnd.DataBind();
                Session["dt"] = dtDispStud;
                //Session.Remove("dt");
            }

            #region Column InVisible

            int countCol = 0;
            int reasonCol = 0;
            int rowVisible = 0;
            for (int row = 0; row < gridMarkAttnd.Rows.Count; row++)
            {
                countCol = 0;
                rowVisible = 0;
                reasonCol = gridMarkAttnd.Rows[row].Cells.Count - 1;
                string locked = gridMarkAttnd.Rows[row].Cells[9].Text;
                int isAttendanceLocked = 0;
                int.TryParse(locked, out isAttendanceLocked);
                if (isAttendanceLocked == 1)
                {
                    gridMarkAttnd.Rows[row].Enabled = false;
                    gridMarkAttnd.Rows[row].BackColor = Color.DarkViolet;
                }
                for (int col = 0; col < gridMarkAttnd.Rows[row].Cells.Count; col++)
                {
                    bool isVisible = true;
                    switch (col)
                    {
                        case 1:
                            isVisible = false;
                            break;
                        case 2:
                            isVisible = isShowApplicationNo;
                            break;
                        case 3:
                            isVisible = isShowRollNo;
                            break;
                        case 4:
                            isVisible = isShowRegNo;
                            break;
                        case 5:
                            isVisible = isShowAdmissionNo;
                            break;
                        case 7:
                            isVisible = isShowStudentType;
                            break;
                        case 8:
                            isVisible = isShowAttendCount;
                            break;
                        case 9:
                            isVisible = false;
                            break;
                        default:
                            if (reasonCol == col)
                            {
                                isVisible = isShowReason;
                            }
                            break;
                    }

                    if (isVisible)
                    {
                        rowVisible++;
                        if (col < 10)
                            countCol++;
                    }
                    TextBox txt = new TextBox();
                    CheckBox chkLeaveCode = new CheckBox();
                    if (col >= 10 && col < reasonCol)
                    {
                        //if (row == 1)
                        //{
                        //    txt = new TextBox();
                        //    txt = (TextBox)gridMarkAttnd.HeaderRow.Cells[col - 1].FindControl("col" + (col - 1));
                        //    txt.Attributes.Add("onclick", "checkvalueHeader(" + (col - countCol) + "," + countCol + ")");
                        //}
                        //gridMarkAttnd.HeaderRow.Cells[col].Attributes.Add("onclick", "checkvalueHeader(" + (col - countCol) + "," + countCol + ")");
                        txt = new TextBox();
                        txt = (TextBox)gridMarkAttnd.HeaderRow.Cells[col].FindControl("col" + col);
                        txt.Attributes.Add("onclick", "checkvalueHeader(" + (col - countCol) + "," + countCol + ")");

                        chkLeaveCode = new CheckBox();
                        chkLeaveCode = (CheckBox)gridMarkAttnd.Rows[row].Cells[col].FindControl("chk_" + col);
                        chkLeaveCode.Attributes.Add("onclick", "Check_Click(this,'" + (row + 1) + "'," + (rowVisible - 1) + "," + countCol + ");");
                    }
                    gridMarkAttnd.Rows[row].Cells[col].Visible = isVisible;
                    gridMarkAttnd.HeaderRow.Cells[col].Visible = isVisible;
                }
            }
            lblResonColEx.Text = "0";
            if (isShowReason)
            {
                lblResonColEx.Text = "1";
            }
            lblPresentOnly.Text = "0";
            if (isShowOnlyPresent)
                lblPresentOnly.Text = "1";
            lblPresentCol.Text = Convert.ToString(countCol).Trim();
            lblReasonCol.Text = Convert.ToString(reasonCol + 1).Trim();

            #endregion

        }
        catch { ScriptManager.RegisterStartupScript(this, typeof(Page), UniqueID, "alert('Please try later')", true); }
    }

    //Get Attendance Types
    private DataTable getAttendanceTypes()
    {
        DataTable dtAttCrit = new DataTable();
        try
        {
            dtAttCrit = dirAcc.selectDataTable("select DispText,LeaveCode from AttMasterSetting where CollegeCode=" + collegeCode + " and (CalcFlag='1' or CalcFlag='0')  ");
            //and DispText in ('A','P') 
            //---------------------------------load rights
            string[] strcomo = new string[20];
            string[] attnd_rights1 = new string[100];
            int i = 0;
            string grouporusercode1 = string.Empty;
            if (Session["group_code"] != null && (Convert.ToString(Session["group_code"]).Trim() != "") && (Convert.ToString(Session["group_code"]).Trim() != "0") && (Convert.ToString(Session["group_code"]).Trim() != "-1"))
            {
                grouporusercode1 = " group_code=" + Convert.ToString(Session["group_code"]).Trim().Split(';')[0] + "";
            }
            else if (Session["usercode"] != null)
            {
                grouporusercode1 = " usercode=" + Convert.ToString(Session["usercode"]).Trim() + "";
            }
            string odrights = da.GetFunction("select rights from  OD_Master_Setting where " + grouporusercode1 + "");
            if (odrights.Trim() != null && odrights.Trim() != "" && odrights.Trim() != "0")
            {
                string od_rights = string.Empty;
                od_rights = odrights;
                string[] split_od_rights = od_rights.Split(',');
                strcomo = new string[split_od_rights.GetUpperBound(0) + 2];
                strcomo[i++] = string.Empty;
                for (int od_temp = 0; od_temp <= split_od_rights.GetUpperBound(0); od_temp++)
                {
                    strcomo[i++] = split_od_rights[od_temp].ToString();
                }
            }
            else
            {
                strcomo[0] = string.Empty;
                strcomo[1] = "P";
                strcomo[2] = "A";
                strcomo[3] = "OD";
                strcomo[4] = "SOD";
                strcomo[5] = "ML";
                strcomo[6] = "NSS";
                strcomo[7] = "L";
                strcomo[8] = "NCC";
                strcomo[9] = "HS";
                strcomo[10] = "PP";
                strcomo[11] = "SYOD";
                strcomo[12] = "COD";
                strcomo[13] = "OOD";
                strcomo[14] = "LA";
            }
            //---------------------------
            DataTable dtNewCrit = new DataTable();
            dtNewCrit.Columns.Add("DispText");
            dtNewCrit.Columns.Add("LeaveCode");

            foreach (string val in strcomo)
            {
                if (!string.IsNullOrEmpty(val))
                {
                    dtAttCrit.DefaultView.RowFilter = "DispText='" + val + "'";
                    DataView dvAttCrit = dtAttCrit.DefaultView;
                    if (dvAttCrit.Count > 0)
                    {
                        DataRow dr = dtNewCrit.NewRow();
                        dr["DispText"] = Convert.ToString(dvAttCrit[0]["DispText"]);
                        dr["LeaveCode"] = Convert.ToString(dvAttCrit[0]["LeaveCode"]);
                        dtNewCrit.Rows.Add(dr);
                    }
                }
            }
            dtAttCrit.Clear();
            dtAttCrit = dtNewCrit;
        }
        catch { dtAttCrit.Clear(); }
        return dtAttCrit;
    }

    //Get Student Data command by rajkumar 9/2/2018
    //private DataTable getStudentData(string colCode, string batch, string degcode, string cursem, string sec, string subno, string staffcode, string elect, string lab, string labDay, string labhr, string ttname, string isAlter, string curDate)
    //{
    //    DataTable dtStud = new DataTable();
    //    try
    //    {
    //        string qrySection = string.Empty;
    //        string qryStaffSection = string.Empty;
    //        string[] sections = sec.Split(',');
    //        sec = string.Empty;
    //        foreach (string curSec in sections)
    //        {
    //            if (!string.IsNullOrEmpty(curSec.Trim()) && curSec.Trim().ToLower() != "all")
    //            {
    //                if (sec == string.Empty)
    //                {
    //                    sec = "'" + curSec + "'";
    //                }
    //                else
    //                {
    //                    sec += ",'" + curSec + "'";
    //                }
    //            }
    //        }
    //        if (!string.IsNullOrEmpty(sec))
    //        {
    //            qrySection = " and LTRIM(RTRIM(isnull(r.Sections,''))) in (" + sec + ")";
    //            qryStaffSection = " and LTRIM(RTRIM(isnull(ss.Sections,''))) in (" + sec + ")";
    //        }

    //        //string selQ = " select a.app_no,r.stud_name,a.app_formno,r.Roll_No,r.Roll_Admit,r.Reg_No,case when isnull(CampusReq,'0')=0 then 'Day Scholar' else 'Hostel' end  as Hostel from Registration r, applyn a where r.App_No = a.app_no and r.college_code='" + colCode + "' and r.Batch_Year='" + batch + "' and r.degree_code='" + degcode + "' and r.Current_Semester='" + cursem + "' and LTRIM(RTRIM(isnull(r.Sections,''))) in ('" + sec + "')";

    //        //string selQ = " select a.app_no,r.stud_name,a.app_formno,r.Roll_No,r.Roll_Admit,r.Reg_No,case when isnull(CampusReq,'0')=0 then 'Day Scholar' else 'Hostel' end  as Hostel from Registration r, applyn a, subjectChooser sc where r.App_No = a.app_no and r.Current_Semester=sc.semester and r.Roll_No = sc.roll_no and sc.subject_no='" + subno + "' and sc.staffcode  like '%" + staffcode + "%'    and r.college_code='" + colCode + "' and r.Batch_Year in ('" + batch + "') and r.degree_code in ('" + degcode + "') and r.Current_Semester in ('" + cursem + "') and LTRIM(RTRIM(isnull(r.Sections,''))) in ('" + sec + "') ";

    //        //string selQ = " select sc.staffcode,a.app_no,r.stud_name,a.app_formno,r.Roll_No,r.Roll_Admit,r.Reg_No,case when isnull(CampusReq,'0')=0 then 'Day Scholar' else 'Hostel' end  as Hostel from Registration r, applyn a, subjectChooser sc ,staff_selector ss where sc.subject_no=ss.subject_no and r.App_No = a.app_no and r.Current_Semester=sc.semester and r.Roll_No = sc.roll_no  and sc.subject_no='" + subno + "' and ss.staff_code  like '%" + staffcode + "%' and sc.staffcode like '%" + staffcode + "%' and r.college_code='" + colCode + "' and r.Batch_Year in ('" + batch + "') and r.degree_code in ('" + degcode + "') and r.Current_Semester in ('" + cursem + "')  " + qrySection + qryStaffSection;
    //        string orderBy = orderByStudents();
    //        bool staffSelector = false;
    //        string qryStudeStaffSelector = string.Empty;  //colCode
    //        string minimumabsentsms = da.GetFunction("select LinkValue from New_InsSettings where LinkName='Studnet Staff Selector' and college_code='" + colCode + "'");
    //        string[] splitminimumabsentsms = minimumabsentsms.Split('-');
    //        if (splitminimumabsentsms.Length == 2)
    //        {
    //            int batchyearsetting = 0;
    //            int.TryParse(Convert.ToString(splitminimumabsentsms[1]).Trim(), out batchyearsetting);
    //            if (splitminimumabsentsms[0].ToString() == "1")
    //            {
    //                if (Convert.ToInt32(batch.ToString()) >= batchyearsetting)
    //                {
    //                    staffSelector = true;
    //                }
    //            }
    //        }
    //        else if (splitminimumabsentsms.Length > 0)
    //        {
    //            if (splitminimumabsentsms[0].ToString() == "1")
    //            {
    //                staffSelector = true;
    //            }
    //        }
    //        if (staffSelector)
    //        {
    //            qryStudeStaffSelector = " and sc.staffcode like '%" + staffcode + "%'";
    //        }

    //        string selQ = " select sc.staffcode,a.app_no,r.stud_name,a.app_formno,r.Roll_No,r.Roll_Admit,r.Reg_No,case when isnull(r.Stud_Type,'Day Scholar')='Day Scholar' then 'D' else case when ISNULL(rd.Room_Name,'')='' then 'H' else 'H - '+ISNULL(rd.Room_Name,'') end  end  as Resident from applyn a, subjectChooser sc ,staff_selector ss,Registration r left join HT_HostelRegistration hr on hr.APP_No=r.App_No left  join Room_Detail rd on RoomPK=hr.RoomFK where sc.subject_no=ss.subject_no and r.App_No = a.app_no and r.Current_Semester=sc.semester and r.Roll_No = sc.roll_no and sc.subject_no='" + subno + "' and ss.staff_code  like '%" + staffcode + "%'  and r.college_code='" + colCode + "' and r.Batch_Year in ('" + batch + "') and r.degree_code in ('" + degcode + "') and r.Current_Semester in ('" + cursem + "') and r.cc=0 and r.delflag=0 and r.Exam_Flag<>'debar' and adm_date<='" + curDate + "' " + qrySection + qryStaffSection + qryStudeStaffSelector + orderBy;

    //        if (elect == "true")
    //        {
    //            //selQ = " select sc.staffcode,a.app_no,r.stud_name,a.app_formno,r.Roll_No,r.Roll_Admit,r.Reg_No,case when isnull(CampusReq,'0')=0 then 'Day Scholar' else 'Hostel' end  as Resident from Registration r, applyn a, subjectChooser sc ,staff_selector ss where sc.subject_no=ss.subject_no and r.App_No = a.app_no and r.Current_Semester=sc.semester and r.Roll_No = sc.roll_no and r.Batch_Year=ss.batch_year and sc.subject_no='" + subno + "' and ss.staff_code  like '%" + staffcode + "%' and sc.staffcode like '%" + staffcode + "%' and r.college_code='" + colCode + "' and r.Batch_Year in ('" + batch + "') and r.degree_code in ('" + degcode + "') and r.Current_Semester in ('" + cursem + "')   " + qrySection + qryStaffSection;

    //            selQ = " select sc.staffcode,a.app_no,r.stud_name,a.app_formno,r.Roll_No,r.Roll_Admit,r.Reg_No,case when isnull(r.Stud_Type,'Day Scholar')='Day Scholar' then 'D' else case when ISNULL(rd.Room_Name,'')='' then 'H' else 'H - '+ISNULL(rd.Room_Name,'') end  end  as Resident from  applyn a, subjectChooser sc ,staff_selector ss,Registration r left join HT_HostelRegistration hr on hr.APP_No=r.App_No left  join Room_Detail rd on RoomPK=hr.RoomFK where sc.subject_no=ss.subject_no and r.App_No = a.app_no and r.Current_Semester=sc.semester and r.Roll_No = sc.roll_no and r.Batch_Year=ss.batch_year and sc.subject_no='" + subno + "' and ss.staff_code  like '%" + staffcode + "%' and r.college_code='" + colCode + "' and r.Batch_Year in ('" + batch + "') and r.degree_code in ('" + degcode + "') and r.Current_Semester in ('" + cursem + "') and r.cc=0 and r.delflag=0 and r.Exam_Flag<>'debar' and adm_date<='" + curDate + "'  " + qrySection + qryStaffSection + qryStudeStaffSelector + orderBy;

    //        }
    //        else if (lab == "true")
    //        {
    //            //selQ = " select a.app_no,r.stud_name,a.app_formno,r.Roll_No,r.Roll_Admit,r.Reg_No,case when isnull(CampusReq,'0')=0 then 'Day Scholar' else 'Hostel' end  as Hostel from Registration r, applyn a, subjectChooser sc where r.App_No = a.app_no and r.Current_Semester=sc.semester and r.Roll_No = sc.roll_no and sc.subject_no='" + subno + "' and sc.staffcode='" + staffcode + "'   and r.college_code='" + colCode + "' and r.Batch_Year='" + batch + "' and r.degree_code='" + degcode + "' and r.Current_Semester='" + cursem + "' and LTRIM(RTRIM(isnull(r.Sections,''))) in ('" + sec + "')";

    //            //selQ = " select a.app_no,r.stud_name,a.app_formno,r.Roll_No,r.Roll_Admit,r.Reg_No,case when isnull(CampusReq,'0')=0 then 'Day Scholar' else 'Hostel' end  as Hostel from Registration r, applyn a, subjectChooser sc,LabAlloc l where r.degree_code=l.Degree_Code and r.Current_Semester = l.Semester and r.Batch_Year=l.Batch_Year and LTRIM(RTRIM(ISNULL(r.Sections,'')))=LTRIM(RTRIM(ISNULL(l.Sections,''))) and l.Stu_Batch=sc.Batch and sc.subject_no=l.Subject_No and sc.staffcode=l.Staff_Code and   r.App_No = a.app_no and r.Current_Semester=sc.semester and r.Roll_No = sc.roll_no  and sc.subject_no='" + subno + "' and sc.staffcode='" + staffcode + "'   and r.college_code='" + colCode + "' and r.Batch_Year='" + batch + "' and r.degree_code='" + degcode + "' and r.Current_Semester='" + cursem + "' and LTRIM(RTRIM(isnull(r.Sections,''))) in ('" + sec + "')  and Day_Value='" + labDay + "' and Hour_Value='" + labhr + "'";
    //            if (isAlter.ToLower().Trim() == "false")
    //            {

    //                //selQ = " select a.app_no,r.stud_name,a.app_formno,r.Roll_No,r.Roll_Admit,r.Reg_No,case when isnull(CampusReq,'0')=0 then 'Day Scholar' else 'Hostel' end  as Hostel from Registration r, applyn a, subjectChooser sc,LabAlloc l,staff_selector ss  where  sc.subject_no=ss.subject_no and r.degree_code=l.Degree_Code and r.Current_Semester = l.Semester and r.Batch_Year=l.Batch_Year and LTRIM(RTRIM(ISNULL(r.Sections,'')))=LTRIM(RTRIM(ISNULL(l.Sections,''))) and l.Stu_Batch=sc.Batch and sc.subject_no=l.Subject_No and   r.App_No = a.app_no and r.Current_Semester=sc.semester and r.Roll_No = sc.roll_no  and sc.subject_no='" + subno + "' and ss.staff_code  like '%" + staffcode + "%'  and sc.staffcode like '%" + staffcode + "%' and r.college_code='" + colCode + "' and r.Batch_Year in ('" + batch + "') and r.degree_code in ('" + degcode + "') and r.Current_Semester in ('" + cursem + "')  and Day_Value='" + labDay + "' and Hour_Value='" + labhr + "'  and l.Timetablename='" + ttname + "'" + qrySection + qryStaffSection;

    //                selQ = " select a.app_no,r.stud_name,a.app_formno,r.Roll_No,r.Roll_Admit,r.Reg_No,case when isnull(r.Stud_Type,'Day Scholar')='Day Scholar' then 'D' else case when ISNULL(rd.Room_Name,'')='' then 'H' else 'H - '+ISNULL(rd.Room_Name,'') end  end  as Resident from applyn a, subjectChooser sc,LabAlloc l,staff_selector ss,Registration r left join HT_HostelRegistration hr on hr.APP_No=r.App_No left  join Room_Detail rd on RoomPK=hr.RoomFK  where  sc.subject_no=ss.subject_no and r.degree_code=l.Degree_Code and r.Current_Semester = l.Semester and r.Batch_Year=l.Batch_Year and LTRIM(RTRIM(ISNULL(r.Sections,'')))=LTRIM(RTRIM(ISNULL(l.Sections,''))) and l.Stu_Batch=sc.Batch and sc.subject_no=l.Subject_No and   r.App_No = a.app_no and r.Current_Semester=sc.semester and r.Roll_No = sc.roll_no and sc.subject_no='" + subno + "' and ss.staff_code  like '%" + staffcode + "%' and r.college_code='" + colCode + "' and r.Batch_Year in ('" + batch + "') and r.degree_code in ('" + degcode + "') and r.Current_Semester in ('" + cursem + "')  and r.cc=0 and r.delflag=0 and r.Exam_Flag<>'debar' and Day_Value='" + labDay + "' and Hour_Value='" + labhr + "'  and l.Timetablename='" + ttname + "' and adm_date<='" + curDate + "'" + qrySection + qryStaffSection + qryStudeStaffSelector + orderBy;

    //            }
    //            else
    //            {
    //                //selQ = " select a.app_no,r.stud_name,a.app_formno,r.Roll_No,r.Roll_Admit,r.Reg_No,case when isnull(CampusReq,'0')=0 then 'Day Scholar' else 'Hostel' end  as Hostel from Registration r, applyn a, subjectChooser_new sc,LabAlloc_new,staff_selector ss  l where  sc.subject_no=ss.subject_no  and r.degree_code=l.Degree_Code and r.Current_Semester = l.Semester and r.Batch_Year=l.Batch_Year and LTRIM(RTRIM(ISNULL(r.Sections,'')))=LTRIM(RTRIM(ISNULL(l.Sections,''))) and l.Stu_Batch=sc.Batch and sc.subject_no=l.Subject_No and   r.App_No = a.app_no and r.Current_Semester=sc.semester and r.Roll_No = sc.roll_no  and sc.subject_no='" + subno + "' and ss.staff_code  like '%" + staffcode + "%' and sc.staffcode like '%" + staffcode + "%' and r.college_code='" + colCode + "' and r.Batch_Year in ('" + batch + "') and r.degree_code in ('" + degcode + "') and r.Current_Semester in ('" + cursem + "') and Day_Value='" + labDay + "' and Hour_Value='" + labhr + "'  and l.fdate>='" + curDate + "' and l.tdate<='" + curDate + "' " + qrySection + qryStaffSection;

    //                selQ = " select a.app_no,r.stud_name,a.app_formno,r.Roll_No,r.Roll_Admit,r.Reg_No,case when isnull(r.Stud_Type,'Day Scholar')='Day Scholar' then 'D' else case when ISNULL(rd.Room_Name,'')='' then 'H' else 'H - '+ISNULL(rd.Room_Name,'') end  end  as Resident from applyn a, subjectChooser_new sc,LabAlloc_new l,staff_selector ss  ,Registration r left join HT_HostelRegistration hr on hr.APP_No=r.App_No left  join Room_Detail rd on RoomPK=hr.RoomFK where  sc.subject_no=ss.subject_no  and r.degree_code=l.Degree_Code and r.Current_Semester = l.Semester and r.Batch_Year=l.Batch_Year and LTRIM(RTRIM(ISNULL(r.Sections,'')))=LTRIM(RTRIM(ISNULL(l.Sections,''))) and l.Stu_Batch=sc.Batch and sc.subject_no=l.Subject_No and   r.App_No = a.app_no and r.Current_Semester=sc.semester and r.Roll_No = sc.roll_no and sc.subject_no='" + subno + "' and ss.staff_code  like '%" + staffcode + "%' and r.college_code='" + colCode + "' and r.Batch_Year in ('" + batch + "') and r.degree_code in ('" + degcode + "') and r.Current_Semester in ('" + cursem + "') and r.cc=0 and r.delflag=0 and r.Exam_Flag<>'debar' and Day_Value='" + labDay + "' and Hour_Value='" + labhr + "'  and l.fdate>='" + curDate + "' and l.tdate<='" + curDate + "' and adm_date<='" + curDate + "'" + qrySection + qryStaffSection + orderBy;

    //            }

    //        }
    //        dtStud = dirAcc.selectDataTable(selQ);
    //    }
    //    catch { dtStud.Clear(); }
    //    return dtStud;
    //}

    private void clearAttMarkDet()
    {
        lblBatch.Text = string.Empty;
        lblCourseDisp.Text = string.Empty;
        lblDegCode.Text = string.Empty;
        lblSubname.Text = string.Empty;
        lblSubno.Text = string.Empty;
        lblSubCode.Text = string.Empty;
        lblSem.Text = string.Empty;
        lblSec.Text = string.Empty;
        lblHourFk.Text = string.Empty;
        lblDayFK.Text = string.Empty;
        lblDate.Text = string.Empty;

        markDiv.Visible = false;

        divMultiSubj.Visible = false;
        ddlMultiSub.Items.Clear();
        ddlMultiSub.Items.Add("All");
    }

    protected void closeAttMark(object sender, EventArgs e)
    {
        markDiv.Visible = false;
    }

    //Load Mark Attendance screen
    protected void Page_Init(object sender, EventArgs e)
    {
        try
        {
            if (Session["dt"] != null)
            {
                DataTable dtNewTable = (DataTable)Session["dt"];
                gridMarkAttnd.DataSource = dtNewTable;
                gridMarkAttnd.DataBind();
                string uid = Request.Form["__EVENTTARGET"];// this.Page.Request.Params.Get("__EVENTTARGET");
                if (uid != null && !uid.Contains("btnMarkAtt"))
                {
                    if (!uid.Contains("chkAbsEntry"))
                    {
                        Session.Remove("dt");
                    }
                }
            }
        }
        catch { }
    }

    protected void gridMarkAttnd_OnRowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {
            e.Row.Cells[1].Visible = false;
            if (e.Row.RowType == DataControlRowType.Header)
            {
                for (int colI = 10; colI < e.Row.Cells.Count - 1; colI++)
                {
                    string[] cellVal = e.Row.Cells[colI].Text.Split('#');
                    e.Row.Cells[colI].Text = cellVal[0];
                }

                for (int i = 10; i < e.Row.Cells.Count - 1; i++)
                {
                    TextBox txt = new TextBox();
                    txt.Text = e.Row.Cells[i].Text.Trim();
                    txt.ForeColor = Color.Black;
                    txt.BackColor = Color.Transparent;
                    txt.ID = "col" + i;
                    txt.Enabled = true;
                    txt.BorderColor = Color.Transparent;
                    txt.Visible = true;
                    txt.Attributes.Add("onclick", "checkvalueHeader(" + (i - 10) + "," + 10 + ")");
                    txt.Attributes.Add("style", "width:25px; readonly:readonly; border:0px; text-align:center; font-weight:bold;");
                    e.Row.Cells[i].Text = txt.Text.Trim();
                    e.Row.Cells[i].Controls.Add(txt);
                    e.Row.Cells[i].Visible = true;
                }
            }
            else if (e.Row.RowType == DataControlRowType.DataRow)
            {
                for (int colI = 10; colI < e.Row.Cells.Count; colI++)
                {
                    if (colI != (e.Row.Cells.Count - 1))
                    {
                        CheckBox chkLeaveCode = new CheckBox();
                        //chkLeaveCode.ID = "chkLvCode_" + colI;
                        chkLeaveCode.ID = "chk_" + colI;
                        chkLeaveCode.Height = 20;
                        chkLeaveCode.Width = 20;
                        chkLeaveCode.Attributes.Add("onclick", "Check_Click(this,'" + (e.Row.RowIndex + 1) + "'," + colI + "," + 10 + ");");
                        if (e.Row.Cells[colI].Text.Trim() == "1")
                        {
                            chkLeaveCode.Checked = true;
                            e.Row.Cells[colI].BackColor = Color.Green;
                        }
                        else
                        {
                            e.Row.Cells[colI].BackColor = Color.Red;
                        }
                        e.Row.Cells[colI].Controls.Add(chkLeaveCode);
                    }
                    else
                    {
                        DropDownList ddlReason = new DropDownList();
                        ddlReason.ID = "ddlAbReas";
                        ddlReason.Width = 120;
                        ddlReason.Height = 30;
                        ddlReason.DataSource = getAbReasons();
                        ddlReason.DataTextField = "Textval";
                        ddlReason.DataValueField = "TextCode";
                        ddlReason.DataBind();
                        ddlReason.Items.Insert(0, string.Empty);
                        string val = e.Row.Cells[colI].Text.Trim().Replace("&nbsp;", string.Empty);
                        if (!string.IsNullOrEmpty(val))
                        {
                            ddlReason.SelectedIndex = ddlReason.Items.IndexOf(ddlReason.Items.FindByValue(val));
                        }
                        e.Row.Cells[colI].Controls.Add(ddlReason);
                    }
                }
            }
        }
        catch { }
    }

    protected void gridMarkAttnd_OnDataBound(object sender, EventArgs e)
    {
        try
        {
            GridViewRow gr = gridMarkAttnd.HeaderRow;
            for (int i = 10; i < gr.Cells.Count - 1; i++)
            {
                gr.Cells[i].Attributes.Add("onclick", "checkvalueHeader(" + (i - 10) + "," + 10 + ")");
            }
        }
        catch { }
    }

    //Mark Attendance 
    protected void btnMarkAttSave_Click(object sender, EventArgs e)
    {
        try
        {
            if (IsAttChecked())
            {
                if (attSaveUpdate() > 0)
                {
                    //ScriptManager.RegisterStartupScript(this, typeof(Page), UniqueID, "alert('Saved successfully')", true);
                    lblAlertMsg.Text = "Saved successfully";
                    divPopAlert.Visible = true;
                }
                else
                {
                    //ScriptManager.RegisterStartupScript(this, typeof(Page), UniqueID, "alert('Not saved')", true);
                    lblAlertMsg.Text = "Not saved";
                    divPopAlert.Visible = true;
                }
            }
            else
            {
                //ScriptManager.RegisterStartupScript(this, typeof(Page), UniqueID, "alert('Please mark attendance')", true);
                lblAlertMsg.Text = "Please mark attendance";
                divPopAlert.Visible = true;
            }
        }
        catch (Exception ex)
        {
            //ScriptManager.RegisterStartupScript(this, typeof(Page), UniqueID, "alert('Please try later')", true);
            da.sendErrorMail(ex, ((Session["collegecode"] != null) ? Convert.ToString(Session["collegecode"]).Trim() : "13"), System.IO.Path.GetFileName(Request.Url.AbsolutePath).ToString());
            lblAlertMsg.Text = "Please try later";
            divPopAlert.Visible = true;
        }
    }

    //Update Attendance
    protected void btnMarkAttUpdate_Click(object sender, EventArgs e)
    {
        try
        {
            if (IsAttChecked())
            {
                if (attSaveUpdate() > 0)
                {
                    //ScriptManager.RegisterStartupScript(this, typeof(Page), UniqueID, "alert('Updated successfully')", true);
                    lblAlertMsg.Text = "Updated successfully";
                    divPopAlert.Visible = true;
                }
                else
                {
                    //ScriptManager.RegisterStartupScript(this, typeof(Page), UniqueID, "alert('Not updated')", true);
                    lblAlertMsg.Text = "Not updated";
                    divPopAlert.Visible = true;
                }
            }
            else
            {
                //ScriptManager.RegisterStartupScript(this, typeof(Page), UniqueID, "alert('Please mark attendance')", true);
                lblAlertMsg.Text = "Please mark attendance";
                divPopAlert.Visible = true;
            }
        }
        catch
        {
            //ScriptManager.RegisterStartupScript(this, typeof(Page), UniqueID, "alert('Please try later')", true);
            lblAlertMsg.Text = "Please try later";
            divPopAlert.Visible = true;
        }
    }

    //Save and Update Attendance Mark
    private int attSaveUpdate()
    {
        int save = 0;
        try
        {
            WebService web = new WebService();
            ArrayList notarray = new ArrayList();
            notarray = getNotArrayLeaveCodes();
            Hashtable absent_calcflag = getAbsentAttendanceTypes();
            int total_conduct_hour = 0;
            int absent_hour = 0;

            string[] date = lblDate.Text.Split('(')[0].Trim().Split('/');
            byte day = Convert.ToByte(date[0]);
            byte month = Convert.ToByte(date[1]);
            int year = Convert.ToInt32(date[2]);
            string monthyear = ((year * 12) + month).ToString();

            string hour = lblDate.Text.Split(':')[1].Trim();
            DataTable dtAttType = getAttendanceTypes();
            bool isShowOnlyPresent = IsShowPresentAbsentOnly();

            Dictionary<byte, string> dicLvCodeVal = new Dictionary<byte, string>();
            GridViewRow gr = gridMarkAttnd.HeaderRow;
            if (isShowOnlyPresent)
            {
                dtAttType.DefaultView.RowFilter = " DispText ='P'";
                DataView dvAttType = dtAttType.DefaultView;
                if (dvAttType.Count > 0)
                {
                    if (!dicLvCodeVal.ContainsKey(0))
                        dicLvCodeVal.Add(0, Convert.ToString(dvAttType[0]["LeaveCode"]));
                }
                dtAttType.DefaultView.RowFilter = " DispText ='A'";
                dvAttType = dtAttType.DefaultView;
                if (dvAttType.Count > 0)
                {
                    if (!dicLvCodeVal.ContainsKey(1))
                        dicLvCodeVal.Add(1, Convert.ToString(dvAttType[0]["LeaveCode"]));
                }
            }
            else
            {
                for (byte i = 10; i < gr.Cells.Count - 1; i++)
                {
                    dtAttType.DefaultView.RowFilter = " DispText ='" + gr.Cells[i].Text.Trim() + "'";
                    DataView dvAttType = dtAttType.DefaultView;
                    if (dvAttType.Count > 0)
                    {
                        dicLvCodeVal.Add(i, Convert.ToString(dvAttType[0]["LeaveCode"]));
                    }
                }
            }

            foreach (GridViewRow gRow in gridMarkAttnd.Rows)
            {
                string leaveCode = string.Empty;
                bool isRowLocked = gRow.Enabled;
                for (byte colI = 10; colI < gRow.Cells.Count - 1; colI++)
                {
                    CheckBox chkLeaveCode = (CheckBox)gRow.FindControl("chk_" + colI);
                    bool isLocked = chkLeaveCode.Enabled;
                    if (isShowOnlyPresent)
                    {
                        if (chkLeaveCode.Checked)
                        {
                            if (dicLvCodeVal.ContainsKey(0))
                            {
                                leaveCode = dicLvCodeVal[0];
                            }
                            else
                            {
                                leaveCode = "1";
                            }
                        }
                        else
                        {
                            if (dicLvCodeVal.ContainsKey(1))
                            {
                                leaveCode = dicLvCodeVal[1];
                            }
                            else
                            {
                                leaveCode = "2";
                            }
                        }
                    }
                    else
                    {
                        if (chkLeaveCode.Checked)
                        {
                            leaveCode = dicLvCodeVal[colI];
                            break;
                        }
                    }
                }
                Label lblAppNo = (Label)gRow.FindControl("lblAppNo");
                string rollNo = dirAcc.selectScalarString("select roll_no from registration where app_no='" + lblAppNo.Text + "'");
                string Att_dcolumna = "[" + "d" + day + "d" + hour + "] ='" + leaveCode + "'";
                string Att_dcolumnainsert = "[" + "d" + day + "d" + hour + "] ";
                string Att_dcolumnainsertr = "'" + leaveCode + "'";
                Dictionary<string, string> dicAttVal = new Dictionary<string, string>();
                DropDownList ddlReason = (DropDownList)gRow.FindControl("ddlAbReas");
                if (!string.IsNullOrEmpty(leaveCode) && isRowLocked)
                {
                    dicAttVal.Add("Att_App_no", lblAppNo.Text);
                    dicAttVal.Add("Att_CollegeCode", collegeCode);
                    dicAttVal.Add("rollno", rollNo);
                    dicAttVal.Add("monthyear", monthyear);
                    dicAttVal.Add("columnname", Att_dcolumnainsert);
                    dicAttVal.Add("colvalues", Att_dcolumnainsertr);
                    dicAttVal.Add("coulmnvalue", Att_dcolumna);
                    save += storeAcc.insertData("sp_ins_upd_student_attendance_Dead", dicAttVal);

                    #region Reason Save

                    if (ddlReason.SelectedIndex > 0)
                    {
                        Dictionary<string, string> dicAttResVal = new Dictionary<string, string>();
                        dicAttResVal.Add("AtWr_App_no", lblAppNo.Text);
                        dicAttResVal.Add("AttWr_CollegeCode", collegeCode);
                        dicAttResVal.Add("columnname", "d" + day + "d" + hour);
                        dicAttResVal.Add("roll_no", rollNo);
                        dicAttResVal.Add("month_year", monthyear);
                        dicAttResVal.Add("values", ddlReason.SelectedValue);
                        int a = storeAcc.insertData("sp_ins_upd_student_attendance_reason", dicAttResVal);
                    }

                    #endregion
                }

                #region Send Sms on absent

                if (absent_calcflag.Contains(leaveCode))
                {
                    string degCode = string.Empty;
                    string semester = string.Empty;
                    string batch = string.Empty;
                    string regno = string.Empty;
                    string admno = string.Empty;
                    getStudentDetails(lblAppNo.Text, ref degCode, ref semester, ref batch, ref regno, ref admno);

                    string[] semStartDate = getSemStartDate(degCode, semester, batch).Split('/');
                    byte sday = Convert.ToByte(semStartDate[0]);
                    byte smonth = Convert.ToByte(semStartDate[1]);
                    int syear = Convert.ToInt32(semStartDate[2]);
                    int smonthyear = ((year * 12) + month);

                    string value_return = web.coundected_hour(Convert.ToInt32(monthyear), smonthyear, rollNo, absent_calcflag, notarray);
                    if (value_return == "Empty")
                    {
                        total_conduct_hour = 1;
                        absent_hour = 1;
                    }
                    else
                    {
                        string[] splitvalue = value_return.Split('-');
                        if (splitvalue.Length > 0)
                        {
                            if (splitvalue[0].ToString() != "")
                            {
                                total_conduct_hour = Convert.ToInt32(splitvalue[0]);
                                total_conduct_hour++;
                            }
                            else
                            {
                                total_conduct_hour++;
                            }
                            if (splitvalue[1].ToString() != "")
                            {
                                absent_hour = Convert.ToInt32(splitvalue[1]);
                                absent_hour++;
                            }
                            else
                            {
                                absent_hour++;
                            }
                        }
                    }
                    string AttDate = date[1] + "/" + date[0] + "/" + date[2];
                    SendingSms(rollNo, lblAppNo.Text, regno, admno, AttDate, hour, degCode, total_conduct_hour, absent_hour);
                    sendvoicecall(rollNo, AttDate, hour, batch, degCode);
                }

                #endregion
            }
        }
        catch { }
        return save;
    }

    //Load attendance entry details
    private Dictionary<string, string> attLoadAtt(string appNostring, string attdate, string atthour)
    {
        Dictionary<string, string> dicAttSaved = new Dictionary<string, string>();
        try
        {
            string[] date = attdate.Split('/');
            byte day = Convert.ToByte(date[1]);
            byte month = Convert.ToByte(date[0]);
            int year = Convert.ToInt32(date[2]);
            string monthyear = ((year * 12) + month).ToString();

            string hour = atthour.Trim();

            string column = "d" + day + "d" + hour;
            DataTable dtAttSaved = dirAcc.selectDataTable("select roll_no,Att_App_no,Att_CollegeCode,month_year,(select am.DispText from AttMasterSetting am where am.LeaveCode=a." + column + ") as LeaveType,a." + column + " from attendance a where Att_App_no in (" + appNostring + ") and Att_CollegeCode = " + collegeCode + " and month_year='" + monthyear + "' and ISNULL(a." + column + ",'')<>''");

            foreach (DataRow drAtt in dtAttSaved.Rows)
            {
                dicAttSaved.Add(Convert.ToString(drAtt["Att_App_no"]), Convert.ToString(drAtt["LeaveType"]).Trim());
            }
        }
        catch { dicAttSaved.Clear(); }
        finally { GC.Collect(); }
        return dicAttSaved;
    }

    //Check for every students attendance are marked
    private bool IsAttChecked()
    {
        bool attOk = true;
        bool isShowOnlyPresent = IsShowPresentAbsentOnly();
        bool isValidation = false;
        try
        {
            foreach (GridViewRow gRow in gridMarkAttnd.Rows)
            {
                bool checkOk = false;
                if (!isShowOnlyPresent && isValidation)
                {
                    for (int i = 10; i < gRow.Cells.Count - 1; i++)
                    {
                        CheckBox chk = (CheckBox)gRow.FindControl("chk_" + i);
                        if (chk.Checked)
                        {
                            checkOk = true;
                            break;
                        }
                    }
                    if (!checkOk)
                        attOk = false;
                }
                else
                {
                    break;
                }
            }
        }
        catch { attOk = false; }
        return attOk;
    }

    //Get Day order or Week day order
    private bool IsWeekDayOrder()
    {
        bool weekorder = true;
        try
        {
            int schtype = dirAcc.selectScalarInt("select top 1 schOrder from PeriodAttndSchedule ");
            weekorder = schtype == 0 ? false : true;
        }
        catch { weekorder = true; }
        return weekorder;
    }

    //Get Number of days per week
    private int NoOfDaysPerweek()
    {
        int nodays = dirAcc.selectScalarInt("select top 1 isnull(nodays,0) from PeriodAttndSchedule ");
        return nodays;
    }

    //Get semester start and end date for the degree and batch
    private void getSemStartEndDate(ref string startDt, ref string endDt, ref int startDayOrder, string batch, string degree, string semester, byte type = 0)
    {
        try
        {
            DataTable dtStartEnd = dirAcc.selectDataTable("select convert(varchar(10),min(start_date),101) as semstart,convert(varchar(10),max(end_date),101) as semend,isnull(starting_dayorder,1) as dorder from seminfo where degree_code in (" + degree + ") and batch_year in (" + batch + ") and semester in (" + semester + ")  group by starting_dayorder order by semstart desc,semend desc");
            //DataTable dtEnd = dirAcc.selectDataTable("select convert(varchar(10),max(end_date),101) as semend,isnull(starting_dayorder,1) as dorder from seminfo where degree_code in (" + degree + ") and batch_year in (" + batch + ")  group by starting_dayorder order by semstart desc,semend desc");

            if (dtStartEnd.Rows.Count > 0)
            {
                startDt = Convert.ToString(dtStartEnd.Rows[0]["semstart"]);
                endDt = Convert.ToString(dtStartEnd.Rows[0]["semend"]);
                startDayOrder = Convert.ToInt32(dtStartEnd.Rows[0]["dorder"]);
            }
            else
            {
                startDt = "01/01/1900"; endDt = "01/01/1900";
            }

            //DataTable dtStartEnd = dirAcc.selectDataTable("select convert(varchar(10),min(start_date),101) as semstart,isnull(starting_dayorder,1) as dorder from seminfo where degree_code in (" + degree + ") and batch_year in (" + batch + ")  and semester in (" + semester + ")  group by starting_dayorder order by dorder " + ((type == 0) ? " asc" : " desc") + ",semstart desc");

            //DataTable dtEnd = dirAcc.selectDataTable("select convert(varchar(10),max(end_date),101) as semend,isnull(starting_dayorder,1) as dorder from seminfo where degree_code in (" + degree + ") and batch_year in (" + batch + ") and semester in (" + semester + ") group by starting_dayorder order by dorder " + ((type == 0) ? " asc" : " desc") + ",semend desc");

            //if (dtStartEnd.Rows.Count > 0)
            //{
            //    startDt = Convert.ToString(dtStartEnd.Rows[0]["semstart"]);
            //    //endDt = Convert.ToString(dtStartEnd.Rows[0]["semend"]);
            //    startDayOrder = Convert.ToInt32(dtStartEnd.Rows[0]["dorder"]);
            //}
            //else
            //{
            //    startDt = "01/01/1900"; //endDt = "01/01/1900";
            //}
            //if (dtEnd.Rows.Count > 0)
            //{
            //    //startDt = Convert.ToString(dtStartEnd.Rows[0]["semstart"]);
            //    endDt = Convert.ToString(dtEnd.Rows[0]["semend"]);
            //    startDayOrder = Convert.ToInt32(dtEnd.Rows[0]["dorder"]);
            //}
            //else
            //{
            //    endDt = "01/01/1900";
            //    startDayOrder = type;
            //}
        }
        catch { startDt = "01/01/1900"; endDt = "01/01/1900"; startDayOrder = type; }
    }

    //Get Holiday Dates for the degree batch and sem start-end dates
    private DataTable getHolidayDates(string semStart, string semEnd, string degcode)
    {
        DataTable dtHolidayDates = new DataTable();
        try
        {
            dtHolidayDates = dirAcc.selectDataTable("Select distinct convert(varchar(10),holiday_date,101) as Hday,holiday_desc,degree_code,isnull(halforfull,'0') as halforfull,isnull(morning,'0') as morning,isnull(evening,'0') as evening from holidaystudents where holiday_date between '" + semStart + "' and '" + semEnd + "' and degree_code in (" + degcode + ")");
        }
        catch { dtHolidayDates.Clear(); }
        return dtHolidayDates;
    }

    //Get Day For Day order wise attendance -- copied from newstaf attendance page to find day order
    public string findday(string curday, string deg_code, string semester, string batch_year, string sdate, string no_days, string stastdayorder)
    {
        try
        {
            string Day_Order = string.Empty;
            int holiday = 0;
            if (string.IsNullOrEmpty(no_days))
                return string.Empty;
            string start_date = string.Empty;
            if (sdate != string.Empty)
            {
                start_date = sdate;
                DateTime dt1 = Convert.ToDateTime(start_date);
                DateTime dt2 = Convert.ToDateTime(curday);
                string currentdate = dt1.ToString("MM/dd/yyyy");
                string startdate = dt2.ToString("MM/dd/yyyy");
                dt1 = Convert.ToDateTime(currentdate);
                dt2 = Convert.ToDateTime(startdate);
                TimeSpan ts = dt2 - dt1;
                string query1 = "select count(*) as count from holidaystudents  where degree_code=" + deg_code.ToString() + " and semester=" + semester.ToString() + " and holiday_date between'" + dt1.ToString("yyyy-MM-dd") + "' and '" + dt2.ToString("yyyy-MM-dd") + "' and isnull(Not_include_dayorder,0)<>'1'";
                string holday = dirAcc.selectScalarString(query1);
                if (holday != string.Empty)
                    holiday = Convert.ToInt32(holday);
                int dif_days = ts.Days;
                string leave = dirAcc.selectScalarString(" select Holiday_desc from holidaystudents  where degree_code=" + deg_code.ToString() + "  and semester=" + semester.ToString() + " and  holiday_date='" + dt2.ToString("yyyy-MM-dd") + "' ");
                if (leave != string.Empty)
                {
                    dif_days = dif_days + 1;
                }
                int dayorderchangedate = 0;
                try
                {
                    string strdayorder = "select * from tbl_consider_day_order where Degree_code='" + deg_code.ToString() + "' and Batch_year='" + batch_year + "' and Semester='" + semester + "' and ((From_Date between '" + dt1.ToString("yyyy-MM-dd") + "' and '" + dt2.ToString("yyyy-MM-dd") + "') or (To_Date between '" + dt1.ToString("yyyy-MM-dd") + "' and '" + dt2.ToString("yyyy-MM-dd") + "')) ;";
                    strdayorder = strdayorder + " select CONVERT(nvarchar(15),holiday_date,101) as hdate from holidaystudents  where degree_code=" + deg_code.ToString() + "  and semester=" + semester.ToString() + " and  holiday_date<='" + dt2.ToString("yyyy-MM-dd") + "' ";
                    DataSet dsdayorderchange = dirAcc.selectDataSet(strdayorder);
                    if (dsdayorderchange.Tables[0].Rows.Count > 0)
                    {
                        Hashtable hatholidc = new Hashtable();
                        for (int hda = 0; hda < dsdayorderchange.Tables[1].Rows.Count; hda++)
                        {
                            string hdater = dsdayorderchange.Tables[1].Rows[hda]["hdate"].ToString();
                            if (!hatholidc.Contains(hdater))
                            {
                                hatholidc.Add(hdater, hdater);
                            }
                        }
                        for (int doc = 0; doc < dsdayorderchange.Tables[0].Rows.Count; doc++)
                        {
                            DateTime dtdcf = Convert.ToDateTime(dsdayorderchange.Tables[0].Rows[doc]["From_Date"].ToString());
                            DateTime dtdct = Convert.ToDateTime(dsdayorderchange.Tables[0].Rows[doc]["To_Date"].ToString());
                            for (DateTime dtdcst = dtdcf; dtdcst <= dtdct; dtdcst = dtdcst.AddDays(1))
                            {
                                if (!hatholidc.Contains(dtdcst.ToString("MM/dd/yyyy")))
                                {
                                    if (dtdcst <= dt2)
                                    {
                                        dayorderchangedate = dayorderchangedate + 1;
                                    }
                                }
                            }
                        }
                    }
                    holiday = holiday + dayorderchangedate;
                }
                catch
                {
                }
                int nodays = Convert.ToInt32(no_days);
                int order = (dif_days - holiday) % nodays;
                //order = order + 1;
                if (stastdayorder.ToString().Trim() != "")
                {
                    if ((stastdayorder.ToString().Trim() != "1") && (stastdayorder.ToString().Trim() != "0"))
                    {
                        order = order + (Convert.ToInt16(stastdayorder) - 1);
                        if (order == (nodays + 1))
                            order = 1;
                        else if (order > nodays)
                            order = order % nodays;
                    }
                }
                if (order.ToString() == "0")
                {
                    order = Convert.ToInt32(no_days);
                }
                string finddayorder = string.Empty;
                if (order == 1) finddayorder = "Monday";
                else if (order == 2) finddayorder = "Tuesday";
                else if (order == 3) finddayorder = "Wednesday";
                else if (order == 4) finddayorder = "Thursday";
                else if (order == 5) finddayorder = "Friday";
                else if (order == 6) finddayorder = "Saturday";
                else if (order == 7) finddayorder = "Sunday";
                if (order >= 1)
                {
                    Day_Order = Convert.ToString(order) + "-" + Convert.ToString(finddayorder);
                }
                else
                {
                    Day_Order = string.Empty;
                }
                return finddayorder;
            }
            else
                return string.Empty;
        }
        catch
        {
            return string.Empty;
        }
    }

    //Get Leave codes
    private ArrayList getNotArrayLeaveCodes()
    {
        ArrayList arr = new ArrayList();
        try
        {
            DataTable dtLv = dirAcc.selectDataTable("select leavecode from AttMasterSetting where calcflag='2' and collegecode=" + collegeCode + "");
            foreach (DataRow dRow in dtLv.Rows)
            {
                if (!arr.Contains(dRow["leavecode"]))
                    arr.Add(Convert.ToString(dRow["leavecode"]));
            }
        }
        catch { arr.Clear(); }
        return arr;
    }

    //Get Leave codes
    private Hashtable getAbsentAttendanceTypes()
    {
        Hashtable htAttCrit = new Hashtable();
        try
        {
            DataTable dtAttCrit = dirAcc.selectDataTable("select LeaveCode from AttMasterSetting where CollegeCode=" + collegeCode + " and CalcFlag='1'");
            foreach (DataRow dROw in dtAttCrit.Rows)
            {
                htAttCrit.Add(Convert.ToString(dROw["LeaveCode"]), Convert.ToString(dROw["LeaveCode"]));
            }
        }
        catch { htAttCrit.Clear(); }
        return htAttCrit;
    }

    //Get Student Base details
    private void getStudentDetails(string appNo, ref string degCode, ref string semester, ref string batch, ref string regno, ref string admno)
    {
        try
        {
            DataTable dtStudDet = dirAcc.selectDataTable("select reg_no, roll_admit,batch_year,degree_code,current_semester from registration where app_no='" + appNo + "'");
            if (dtStudDet.Rows.Count > 0)
            {
                degCode = Convert.ToString(dtStudDet.Rows[0]["degree_code"]);
                semester = Convert.ToString(dtStudDet.Rows[0]["current_semester"]);
                batch = Convert.ToString(dtStudDet.Rows[0]["batch_year"]);
                regno = Convert.ToString(dtStudDet.Rows[0]["reg_no"]);
                admno = Convert.ToString(dtStudDet.Rows[0]["roll_admit"]);
            }
        }
        catch { }
    }

    //Get Semester start date
    private string getSemStartDate(string degCode, string semester, string batch)
    {
        string semStart = string.Empty;
        try
        {
            semStart = dirAcc.selectScalarString("select convert(varchar(10),start_date,103) as start_date from seminfo where  degree_code='" + degCode + "' and semester='" + semester + "' and batch_year='" + batch + "'");
        }
        catch { semStart = string.Empty; }
        return semStart;
    }

    //Get Absent Reasons
    private DataTable getAbReasons()
    {
        DataTable dtReas = new DataTable();
        try
        {
            string query = "select TextCode,Textval from textvaltable where TextCriteria='Attrs' and college_code=" + Session["collegecode"].ToString() + "";
            dtReas = dirAcc.selectDataTable(query);
        }
        catch { dtReas.Clear(); }
        return dtReas;
    }

    //Show Absent entry mark screen old page copy
    protected void chkAbsEntry_OnCheckChanged(object sender, EventArgs e)
    {
        try
        {
            appentiesentry();
        }
        catch { }
    }

    protected void btnaddrow_Click(object sender, EventArgs e)
    {
        fpattendanceentry.Sheets[0].RowCount++;
    }

    protected void btnaddattendance_Click(object sender, EventArgs e)
    {
        //try
        //{
        //    rbgraphics.Checked = false;
        //    rbappenses.Checked = true;
        //    lblerrmsg.Visible = false;
        //    fpattendanceentry.SaveChanges();
        //    string studroll = string.Empty;
        //    string rollprefix = string.Empty;
        //    string setattandance = ddlattend.SelectedItem.ToString().Trim();
        //    string setrestattendance = ddlreststudent.SelectedItem.ToString().Trim();
        //    Hashtable hatrestroll = new Hashtable();
        //    Hashtable hatinvalidroll = new Hashtable();
        //    Boolean entryfalag = false;
        //    hatinvalidroll.Clear();
        //    hatrestroll.Clear();
        //    fpattendanceentry.SaveChanges();
        //    //string strinvalidroll =string.Empty;
        //    if (setattandance.Trim() != "" && setattandance.Trim() != null && setattandance.Trim() != "-1")
        //    {
        //        for (int j = 0; j < FpSpread2.Sheets[0].RowCount; j++)
        //        {
        //            studroll = FpSpread2.Sheets[0].Cells[j, 1].Text.ToString().ToLower().Trim();
        //            if (!hatroll.Contains(studroll.Trim().ToLower()))
        //            {
        //                hatroll.Add(studroll.Trim().ToLower(), j);
        //            }
        //        }
        //        if (fpattendanceentry.Sheets[0].RowCount > 0)
        //        {
        //            for (int i = 0; i < fpattendanceentry.Sheets[0].RowCount; i++)
        //            {
        //                rollprefix = fpattendanceentry.Sheets[0].Cells[i, 0].Text.ToString().Trim().ToLower();
        //                string prefixrollno = fpattendanceentry.Sheets[0].Cells[i, 1].Text.ToString().Trim().ToLower();
        //                if (rollprefix.Trim() != null && rollprefix.Trim() != "" && prefixrollno != null && prefixrollno.Trim() != "")
        //                {
        //                    string[] prerollno = prefixrollno.Split(',');
        //                    for (int j = 0; j <= prerollno.GetUpperBound(0); j++)
        //                    {
        //                        for (int col = 7; col < FpSpread2.Sheets[0].ColumnCount; col = col + 2)
        //                        {
        //                            studroll = rollprefix + prerollno[j].ToString().Trim().ToLower();
        //                            if (hatroll.Contains(studroll.Trim().ToLower()))
        //                            {
        //                                entryfalag = true;
        //                                string rowvalue = GetCorrespondingKey(studroll.Trim().ToLower(), hatroll).ToString();
        //                                if (rowvalue != "Entered" && FpSpread2.Sheets[0].Cells[Convert.ToInt32(rowvalue), col].Locked == false && FpSpread2.Sheets[0].Cells[Convert.ToInt32(rowvalue), col].Text != "S" && FpSpread2.Sheets[0].Cells[Convert.ToInt32(rowvalue), col].Text.ToLower().Trim() != "od")
        //                                {
        //                                    int row = Convert.ToInt32(rowvalue);
        //                                    FpSpread2.Sheets[0].Cells[row, col].Text = setattandance;
        //                                    if (col == FpSpread2.Sheets[0].ColumnCount - 2)
        //                                    {
        //                                        hatroll[studroll] = "Entered";
        //                                    }
        //                                }
        //                            }
        //                            else
        //                            {
        //                                if (!hatinvalidroll.Contains(studroll.ToLower()))
        //                                {
        //                                    hatinvalidroll.Add(studroll, studroll);
        //                                }
        //                            }
        //                        }
        //                    }
        //                }
        //            }
        //            if (setrestattendance.Trim() != "" && setrestattendance.Trim() != null && setrestattendance.Trim() != "-1")
        //            {
        //                for (int j = 0; j < FpSpread2.Sheets[0].RowCount; j++)
        //                {
        //                    studroll = FpSpread2.Sheets[0].Cells[j, 1].Text.ToString().ToLower().Trim();
        //                    if (hatroll.Contains(studroll.Trim().ToLower()))
        //                    {
        //                        for (int col = 7; col < FpSpread2.Sheets[0].ColumnCount; col = col + 2)
        //                        {
        //                            string restroll = GetCorrespondingKey(studroll.Trim().ToLower(), hatroll).ToString();
        //                            if (restroll != "Entered" && FpSpread2.Sheets[0].Cells[Convert.ToInt32(restroll), col].Text != "S" && FpSpread2.Sheets[0].Cells[Convert.ToInt32(restroll), col].Locked == false && FpSpread2.Sheets[0].Cells[Convert.ToInt32(restroll), col].Text.ToLower().Trim() != "od")
        //                            {
        //                                FpSpread2.Sheets[0].Cells[Convert.ToInt32(restroll), col].Text = setrestattendance;
        //                            }
        //                        }
        //                    }
        //                }
        //            }
        //            if (hatinvalidroll.Count > 0)
        //            {
        //                foreach (DictionaryEntry parameter1 in hatinvalidroll)
        //                {
        //                    if (strinvalidroll == "")
        //                    {
        //                        strinvalidroll = (parameter1.Key).ToString();
        //                    }
        //                    else
        //                    {
        //                        strinvalidroll = strinvalidroll + " , " + (parameter1.Key).ToString();
        //                    }
        //                }
        //            }
        //            if (entryfalag == true)
        //            {
        //                //if (strinvalidroll != "")
        //                //{
        //                //    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "clientScript", "alert('Following Roll Nos Are Invalid:" + strinvalidroll + "')", true);
        //                //}
        //                Buttonsave_Click(sender, e);
        //                appentiesentry();
        //            }
        //            else
        //            {
        //                lblerrmsg.Visible = true;
        //                lblerrmsg.Text = "No Student Match";
        //            }
        //        }
        //        else
        //        {
        //            lblerrmsg.Visible = true;
        //            lblerrmsg.Text = "Please Add Row";
        //        }
        //    }
        //    else
        //    {
        //        lblerrmsg.Visible = true;
        //        lblerrmsg.Text = "Please Enter Selected Students Attendance";
        //    }
        //    //FpSpread2.SaveChanges();
        //}
        //catch (Exception ex)
        //{
        //}
    }

    public void appentiesentry()
    {
        try
        {
            if (!chkAbsEntry.Checked)
            {
                //ddlMultiSub_OnChanged(new object(), new EventArgs());
                divAttMark.Visible = true;
                fieldat.Visible = false;
                return;
            }

            divAttMark.Visible = false;
            string[] date = lblDate.Text.Split('(')[0].Trim().Split('/');
            byte day = Convert.ToByte(date[0]);
            byte month = Convert.ToByte(date[1]);
            int year = Convert.ToInt32(date[2]);
            string monthyear = ((year * 12) + month).ToString();
            string hour = lblDate.Text.Split(':')[1].Trim();

            //FpSpread2.Visible = false;
            //Buttonselectall.Visible = false;
            //Buttondeselect.Visible = false;
            //Buttonsave.Visible = false;
            //Buttonupdate.Visible = false;
            //lblmanysubject.Visible = false;
            //ddlselectmanysub.Visible = false;
            lblatdate.Visible = true;
            lblcurdate.Visible = true;
            lblhour.Visible = true;
            lblhrvalue.Visible = true;
            lblattend.Visible = true;
            ddlattend.Visible = true;
            btnaddrow.Visible = true;
            fpattendanceentry.Visible = true;
            lblreststudent.Visible = true;
            ddlreststudent.Visible = true;
            lblerrmsg.Visible = true;
            btnaddattendance.Visible = true;
            fieldat.Visible = true;
            lblerrmsg.Visible = false;
            fpattendanceentry.Sheets[0].RowCount = 0;

            lblcurdate.Text = lblDate.Text.Split(' ')[0].Trim() + " " + lblDate.Text.Split(' ')[1].Trim();

            lblhrvalue.Text = hour.ToString();
            ddlattend.Items.Clear();
            ddlreststudent.Items.Clear();
            string odrights = da.GetFunction("select rights from OD_Master_Setting where " + grouporusercode + "");
            if (odrights.Trim() != null && odrights.Trim() != "" && odrights.Trim() != "0")
            {
                string od_rights = string.Empty;
                od_rights = odrights;
                string[] split_od_rights = od_rights.Split(',');
                ddlattend.Items.Add(" ");
                ddlreststudent.Items.Add(" ");
                for (int od_temp = 0; od_temp <= split_od_rights.GetUpperBound(0); od_temp++)
                {
                    string value = split_od_rights[od_temp];
                    ddlattend.Items.Add("" + value + " ");
                    ddlreststudent.Items.Add("" + value + " ");
                }
            }
            else
            {
                ddlreststudent.Items.Add(" ");
                ddlreststudent.Items.Add("P");
                ddlreststudent.Items.Add("A");
                ddlreststudent.Items.Add("OD ");
                ddlreststudent.Items.Add("SOD");
                ddlreststudent.Items.Add("ML");
                ddlreststudent.Items.Add("NSS");
                ddlreststudent.Items.Add("L");
                ddlreststudent.Items.Add("NCC");
                ddlreststudent.Items.Add("HS");
                ddlreststudent.Items.Add("PP");
                ddlreststudent.Items.Add("SYOD");
                ddlreststudent.Items.Add("COD");
                ddlreststudent.Items.Add("OOD");
                ddlreststudent.Items.Add("LA");

                ddlattend.Items.Add(" ");
                ddlattend.Items.Add("P");
                ddlattend.Items.Add("A");
                ddlattend.Items.Add("OD ");
                ddlattend.Items.Add("SOD");
                ddlattend.Items.Add("ML");
                ddlattend.Items.Add("NSS");
                ddlattend.Items.Add("L");
                ddlattend.Items.Add("NCC");
                ddlattend.Items.Add("HS");
                ddlattend.Items.Add("PP");
                ddlattend.Items.Add("SYOD");
                ddlattend.Items.Add("COD");
                ddlattend.Items.Add("OOD");
                ddlattend.Items.Add("LA");
            }
            fpattendanceentry.Sheets[0].ColumnHeader.RowCount = 1;
            fpattendanceentry.Sheets[0].ColumnCount = 2;
            FarPoint.Web.Spread.TextCellType txtcell = new FarPoint.Web.Spread.TextCellType();
            fpattendanceentry.Sheets[0].Columns[0].CellType = txtcell;
            fpattendanceentry.Sheets[0].Columns[1].CellType = txtcell;
            fpattendanceentry.Sheets[0].ColumnHeader.Cells[0, 0].Text = "Roll Prefix";
            fpattendanceentry.Sheets[0].ColumnHeader.Cells[0, 1].Text = "Roll No of the Student";
            fpattendanceentry.Sheets[0].ColumnHeader.Rows[0].Font.Bold = true;
            fpattendanceentry.Sheets[0].Columns[0].Width = 150;
            fpattendanceentry.Sheets[0].Columns[1].Width = 451;
            fpattendanceentry.Sheets[0].ColumnHeader.Rows[0].Height = 45;
            fpattendanceentry.Height = 250;
            fpattendanceentry.Enabled = true;

        }
        catch
        {
        }
    }

    #region Old Attendance screen data

    //Reason Add and Delete
    public void btnremovereason_Click(object sender, EventArgs e)
    {
        try
        {
            if (ddlreason.Items.Count > 0)
            {
                string collegecode = Session["collegecode"].ToString();
                string reason = ddlreason.SelectedItem.ToString();
                if (reason.Trim().ToLower() != "all" && reason.Trim() != "")
                {
                    string strquery = "delete textvaltable where TextVal='" + reason + "' and TextCriteria='Attrs' and college_code='" + collegecode + "'";
                    int a = da.update_method_wo_parameter(strquery, "Text");
                    loadreason();
                }
            }
        }
        catch (Exception ex)
        {
        }
    }

    public void btnaddreason_Click(object sender, EventArgs e)
    {
        panel1.Visible = true;
    }

    public void loadreason()
    {
        ddlreason.Items.Clear();
        string collegecode = collegeCode;
        string query = "select TextCode,Textval from textvaltable where TextCriteria='Attrs' and college_code=" + collegeCode + "";
        DataSet ds = da.select_method_wo_parameter(query, "Text");
        if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
        {
            ddlreason.DataSource = ds;
            ddlreason.DataTextField = "Textval";
            ddlreason.DataValueField = "TextCode";
            ddlreason.DataBind();
        }
    }

    public void btnreasonnew_Click(object sender, EventArgs e)
    {
        panel1.Visible = true;
        string collegecode = Session["collegecode"].ToString();
        string reason = txtreason.Text.ToString();
        if (reason.Trim() != "")
        {
            string strquery = "insert into textvaltable (TextVal,TextCriteria,college_code) values('" + reason + "','Attrs','" + collegecode + "')";
            int a = da.update_method_wo_parameter(strquery, "Text");
            txtreason.Text = string.Empty;
            loadreason();
        }
    }

    public void btnreasonexit_Click(object sender, EventArgs e)
    {
        panel1.Visible = false;
    }

    //Sending SMS
    public void SendingSms(string rollno, string appno, string regno, string admno, string date, string hour, string degree, int total, int absent)
    {
        try
        {
            string MsgText = string.Empty;
            string RecepientNo = string.Empty;
            string SenderID = string.Empty;
            string Password = string.Empty;
            string user_id = string.Empty;
            string sections = string.Empty;
            string Gender = string.Empty;
            string Hour = hour;
            string hour_check = string.Empty;
            //UserEmailID =string.Empty;
            MsgText = string.Empty;
            RecepientNo = string.Empty;
            int check = 0;
            string coursename = string.Empty;
            string collegename = string.Empty;
            string[] split = date.Split(new Char[] { '/' });
            string datefrom = split[1].ToString() + "-" + split[0].ToString() + "-" + split[2].ToString();
            date = datefrom;
            if (Convert.ToInt16(hour) == 1)
            {
                Hour = hour + "st ";
                hour_check = hour;
            }
            else if (Convert.ToInt16(hour) == 2)
            {
                Hour = hour + "nd ";
                hour_check = hour;
            }
            else if (Convert.ToInt16(hour) == 3)
            {
                Hour = hour + "rd ";
                hour_check = hour;
            }
            else if (Convert.ToInt16(hour) > 3)
            {
                Hour = hour + "th ";
                hour_check = hour;
            }
            string collquery = "Select collname,Coll_acronymn from collinfo where college_code=" + collegeCode + "";
            DataSet datacol = new DataSet();
            datacol.Clear();
            datacol = da.select_method_wo_parameter(collquery, "Text");
            if (datacol.Tables.Count > 0 && datacol.Tables[0].Rows.Count > 0)
            {
                collegename = datacol.Tables[0].Rows[0]["Coll_acronymn"].ToString();
            }
            //string degreequery = "select distinct Course_Name,Dept_Name from Department dep, Degree deg, course c where dep.Dept_Code=deg.Dept_Code and c.Course_Id=deg.Course_Id and deg.college_code =" +collegeCode + " and Degree_Code=" + degree + "";
            string degreequery = "select distinct Course_Name,Dept_Name,r.degree_code from Department dep, Degree deg, course c,Registration r where dep.Dept_Code=deg.Dept_Code and c.Course_Id=deg.Course_Id and r.degree_code=deg.Degree_Code and r.Roll_No='" + rollno + "'";
            DataSet dscode = new DataSet(); string degreecode = string.Empty;
            dscode = da.select_method_wo_parameter(degreequery, "Text");
            if (dscode.Tables.Count > 0 && dscode.Tables[0].Rows.Count > 0)
            {
                string course = dscode.Tables[0].Rows[0]["Course_Name"].ToString();
                string deptname = dscode.Tables[0].Rows[0]["Dept_Name"].ToString();
                degreecode = dscode.Tables[0].Rows[0]["degree_code"].ToString();
                coursename = course + "-" + deptname;
            }
            string str1 = string.Empty;
            string group_code = grouporusercode;
            if (group_code.Contains(";"))
            {
                string[] group_semi = group_code.Split(';');
                group_code = group_semi[0].ToString();
            }
            if ((grouporusercode.Trim() != "") && (grouporusercode.Trim() != "0") && (grouporusercode.Trim() != "-1"))
            {
                str1 = "select distinct textname,taxtval from Attendance_Settings where  college_code='" + collegeCode + "' and USER_ID='" + group_code + "'";
                str1 = str1 + "  select template from master_Settings where settings='SmsAttendanceTepmlate' and group_code='" + group_code + "'and value='1'";
            }
            else
            {
                str1 = "select distinct textname,taxtval from Attendance_Settings where college_code='" + collegeCode + "' and USER_ID='" + userCode + "'";
                str1 = str1 + "  select template from master_Settings where settings='SmsAttendanceTepmlate' and usercode='" + userCode + "'and value='1'";
            }
            Boolean flage = false;
            DataSet ds1;
            ds1 = da.select_method_wo_parameter(str1, "txt");
            string hodphone = da.GetFunction("  select d.PhoneNo from Department d,Degree de,staffmaster s,staff_appl_master sa where d.Dept_Code=de.Dept_Code and s.appl_no=sa.appl_no and d.Head_Of_Dept=s.staff_code and resign='0' and settled='0' and de.Dept_Code ='" + degreecode + "'");
            if (ds1.Tables.Count > 0 && ds1.Tables[0].Rows.Count > 0)
            {
                for (int jj = 0; jj < ds1.Tables[0].Rows.Count; jj++)
                {
                    if (ds1.Tables[0].Rows[jj]["TextName"].ToString() == "Attendance Sms for Absent" && ds1.Tables[0].Rows[jj]["Taxtval"].ToString() == "1")
                    {
                        flage = true;
                    }
                }
                if (flage == true)
                {
                    for (int k = 0; k < ds1.Tables[0].Rows.Count; k++)
                    {
                        if (ds1.Tables[0].Rows[k]["TextName"].ToString() == "Period" && ds1.Tables[0].Rows[k]["Taxtval"].ToString() != "")
                        {
                            string splihours = ds1.Tables[0].Rows[k]["Taxtval"].ToString();
                            string[] fin_split = splihours.Split(',');
                            int count = fin_split.Length;
                            for (int i = 0; i < count; i++)
                            {
                                string final_Hours = fin_split[i];
                                if (hour_check == final_Hours)
                                {
                                    check = check + 1;
                                }
                            }
                        }
                    }
                }
            }
            if (check > 0)
            {
                check = 0;
                string ssr = "select * from Track_Value where college_code='" + collegeCode + "'";
                DataSet dstrack;
                dstrack = da.select_method_wo_parameter(ssr, "txt");
                if (dstrack.Tables.Count > 0 && dstrack.Tables[0].Rows.Count > 0)
                {
                    user_id = Convert.ToString(dstrack.Tables[0].Rows[0]["SMS_User_ID"]);
                    string Phone = "select distinct isnull(a.parentF_Mobile,'0') as FatherMobile,isnull(a.parentM_Mobile,'0')as MotherMobile,isnull(a.Student_Mobile,'0') as StudentMobile,a.sex as Gender,isnull(a.stud_name,r.stud_name) as StudName from applyn a,registration r where a.app_no=r.app_no and r.roll_no='" + rollno + "' and r.college_code='" + collegeCode + "'";
                    DataSet dsMobile;
                    dsMobile = da.select_method_wo_parameter(Phone, "txt");
                    if (ds1.Tables.Count > 1 && ds1.Tables[1].Rows.Count > 0) //************************ added by jairam****************************** 10-10-2014
                    {
                        DateTime dt = Convert.ToDateTime(date);
                        string templatevlaue = Convert.ToString(ds1.Tables[1].Rows[0]["template"]);
                        if (templatevlaue.Trim() != "")
                        {
                            string[] splittemplate = templatevlaue.Split('$');
                            if (splittemplate.Length > 0)
                            {
                                for (int j = 0; j <= splittemplate.GetUpperBound(0); j++)
                                {
                                    if (splittemplate[j].ToString() != "")
                                    {
                                        if (splittemplate[j].ToString() == "College Name")
                                        {
                                            MsgText = MsgText + " " + collegename;
                                        }
                                        else if (splittemplate[j].ToString() == "Student Name")
                                        {
                                            MsgText = MsgText + " " + dsMobile.Tables[0].Rows[0]["StudName"].ToString();
                                        }
                                        else if (splittemplate[j].ToString() == "Degree")
                                        {
                                            MsgText = MsgText + " " + coursename;
                                        }
                                        else if (splittemplate[j].ToString() == "Section")
                                        {
                                            if (sections != "")
                                            {
                                                MsgText = MsgText + " " + "" + sections + " Section";
                                            }
                                        }
                                        else if (splittemplate[j].ToString() == "Thank You")
                                        {
                                            MsgText = MsgText + " " + splittemplate[j].ToString();
                                        }
                                        else if (splittemplate[j].ToString() == "Absent")
                                        {
                                            MsgText = MsgText + " " + Hour + " hour Absent";
                                        }
                                        //22/09/16
                                        else if (splittemplate[j].ToString() == "Date")
                                        {
                                            MsgText = MsgText + " Date: " + dt.ToString("dd/MM/yyyy") + "";
                                        }
                                        else if (splittemplate[j].ToString() == "HOD")
                                        {
                                            if (hodphone.Trim() != "")
                                            {
                                                MsgText = MsgText + " - " + hodphone;
                                            }
                                            else
                                            {
                                                MsgText = MsgText + " ";
                                            }
                                        }
                                        else if (splittemplate[j].ToString() == "Roll No")
                                        {
                                            MsgText = MsgText + " " + rollno;
                                        }
                                        else if (splittemplate[j].ToString() == "Register No")
                                        {
                                            MsgText = MsgText + " " + regno;
                                        }
                                        else if (splittemplate[j].ToString() == "Application No")
                                        {
                                            MsgText = MsgText + " " + appno;
                                        }
                                        else if (splittemplate[j].ToString() == "Admission No")
                                        {
                                            MsgText = MsgText + " " + admno;
                                        }
                                        else
                                        {
                                            if (MsgText == "")
                                            {
                                                MsgText = splittemplate[j].ToString();
                                            }
                                            else
                                            {
                                                MsgText = MsgText + " " + splittemplate[j].ToString();
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                    else
                    {
                        MsgText = "Dear Parent, Good Morning. This Message from" + " " + collegename + ". Your ward " + dsMobile.Tables[0].Rows[0]["StudName"].ToString() + " of " + coursename + " is found absent  " + Hour + " hour. Conducted Hours:" + total + " Absent Hours:" + absent + ". Thank you";
                    }
                    if (ds1.Tables.Count > 0 && ds1.Tables[0].Rows.Count > 0 && dsMobile.Tables.Count > 0 && dsMobile.Tables[0].Rows.Count > 0)
                    {
                        for (int jj1 = 0; jj1 < ds1.Tables[0].Rows.Count; jj1++)
                        {
                            if (ds1.Tables[0].Rows[jj1]["TextName"].ToString() == "Father" && ds1.Tables[0].Rows[jj1]["Taxtval"].ToString() == "1")
                            {
                                if (dsMobile.Tables[0].Rows[0]["FatherMobile"].ToString() != "0")
                                {
                                    RecepientNo = dsMobile.Tables[0].Rows[0]["FatherMobile"].ToString();
                                    string getval = da.GetUserapi(user_id);
                                    string[] spret = getval.Split('-');
                                    if (spret.GetUpperBound(0) == 1)
                                    {
                                        SenderID = spret[0].ToString();
                                        Password = spret[1].ToString();
                                        Session["api"] = user_id;
                                        Session["senderid"] = SenderID;
                                    }
                                    //Modified By Srinath
                                    string strpath = string.Empty;
                                    //if (SenderID != "eSNCET" && Password != "yahoo10")
                                    //{
                                    //strpath = "http://dnd.airsmsmarketing.info/api/sendmsg.php?user=" + SenderID + "&pass=" + Password + "&sender=" + SenderID + "&phone=" + RecepientNo + "&text=" + MsgText + "&priority=ndnd&stype=normal";
                                    //}
                                    //else
                                    //{
                                    //    strpath = "http://49.50.69.90/api/smsapi.aspx?username=" + user_id + "&password=" + Password + "&to=" + RecepientNo + "&from=" + SenderID + "&message=" + MsgText;
                                    //}
                                    //string isst = "0";
                                    //smsreport(strpath, isst, dt);
                                    int nofosmssend = da.send_sms(user_id, collegeCode, userCode, RecepientNo, MsgText, "0");
                                }
                            }
                            if (ds1.Tables[0].Rows[jj1]["TextName"].ToString() == "Mother" && ds1.Tables[0].Rows[jj1]["Taxtval"].ToString() == "1")
                            {
                                if (dsMobile.Tables[0].Rows[0]["MotherMobile"].ToString() != "0")
                                {
                                    RecepientNo = dsMobile.Tables[0].Rows[0]["MotherMobile"].ToString();
                                    string getval = da.GetUserapi(user_id);
                                    string[] spret = getval.Split('-');
                                    if (spret.GetUpperBound(0) == 1)
                                    {
                                        SenderID = spret[0].ToString();
                                        Password = spret[1].ToString();
                                        Session["api"] = user_id;
                                        Session["senderid"] = SenderID;
                                    }
                                    //Modified By SRinath /2/2014
                                    string strpath = string.Empty;
                                    //if (SenderID != "eSNCET" && Password != "yahoo10")
                                    //{
                                    //strpath = "http://dnd.airsmsmarketing.info/api/sendmsg.php?user=" + SenderID + "&pass=" + Password + "&sender=" + SenderID + "&phone=" + RecepientNo + "&text=" + MsgText + "&priority=ndnd&stype=normal";
                                    //}
                                    //else
                                    //{
                                    //    strpath = "http://49.50.69.90/api/smsapi.aspx?username=" + user_id + "&password=" + Password + "&to=" + RecepientNo + "&from=" + SenderID + "&message=" + MsgText;
                                    //}
                                    //string isst = "0";
                                    //smsreport(strpath, isst, dt);
                                    int nofosmssend = da.send_sms(user_id, collegeCode, userCode, RecepientNo, MsgText, "0");
                                }
                            }
                            if (ds1.Tables[0].Rows[jj1]["TextName"].ToString() == "Student" && ds1.Tables[0].Rows[jj1]["Taxtval"].ToString() == "1")
                            {
                                if (dsMobile.Tables[0].Rows[0]["StudentMobile"].ToString() != "0")
                                {
                                    RecepientNo = dsMobile.Tables[0].Rows[0]["StudentMobile"].ToString();
                                    string getval = da.GetUserapi(user_id);
                                    string[] spret = getval.Split('-');
                                    if (spret.GetUpperBound(0) == 1)
                                    {
                                        SenderID = spret[0].ToString();
                                        Password = spret[1].ToString();
                                        Session["api"] = user_id;
                                        Session["senderid"] = SenderID;
                                    }
                                    //Modified By Srinatrh 8/2/2014
                                    string strpath = string.Empty;
                                    //if (SenderID != "eSNCET" && Password != "yahoo10")
                                    //{
                                    //strpath = "http://dnd.airsmsmarketing.info/api/sendmsg.php?user=" + SenderID + "&pass=" + Password + "&sender=" + SenderID + "&phone=" + RecepientNo + "&text=" + MsgText + "&priority=ndnd&stype=normal";
                                    //}
                                    //else
                                    //{
                                    //    strpath = "http://49.50.69.90/api/smsapi.aspx?username=" + user_id + "&password=" + Password + "&to=" + RecepientNo + "&from=" + SenderID + "&message=" + MsgText;
                                    //}
                                    //string isst = "0";
                                    //smsreport(strpath, isst, dt);
                                    int nofosmssend = da.send_sms(user_id, collegeCode, userCode, RecepientNo, MsgText, "0");
                                }
                            }
                        }
                    }
                }
            }
        }
        catch (Exception ex)
        {
        }
    }

    //Sending Voice Call
    public void sendvoicecall(string rollno, string date, string hour, string batch, string degree)
    {
        try
        {
            string Gender = string.Empty;
            string Hour = hour;
            string hour_check = string.Empty;
            //UserEmailID =string.Empty;
            string roll = rollno;
            string batchyear = batch;
            string coursename = string.Empty;
            string collegename = string.Empty;
            string collaccronymn = string.Empty;
            string voicelanguage = string.Empty;
            string MsgText = string.Empty;
            string RecepientNo = string.Empty;
            int check = 0;
            string[] split = date.Split(new Char[] { '-' });
            string datefrom = split[1].ToString() + "-" + split[0].ToString() + "-" + split[2].ToString();
            date = datefrom;
            if (Convert.ToInt16(hour) == 1)
            {
                Hour = hour + "st ";
                hour_check = hour;
            }
            else if (Convert.ToInt16(hour) == 2)
            {
                Hour = hour + "nd ";
                hour_check = hour;
            }
            else if (Convert.ToInt16(hour) == 3)
            {
                Hour = hour + "rd ";
                hour_check = hour;
            }
            else if (Convert.ToInt16(hour) > 3)
            {
                Hour = hour + "th ";
                hour_check = hour;
            }
            string collquery = "Select collname from collinfo where college_code=" + collegeCode + "";
            DataSet datacol = new DataSet();
            datacol.Clear();
            datacol = da.select_method_wo_parameter(collquery, "Text");
            if (datacol.Tables.Count > 0 && datacol.Tables[0].Rows.Count > 0)
            {
                collegename = datacol.Tables[0].Rows[0]["collname"].ToString();
            }
            string degreequery = "select distinct Course_Name,Dept_Name from Department dep, Degree deg, course c where dep.Dept_Code=deg.Dept_Code and c.Course_Id=deg.Course_Id and deg.college_code =" + collegeCode + " and Degree_Code=" + degree + "";
            DataSet dscode = new DataSet();
            dscode = da.select_method_wo_parameter(degreequery, "Text");
            if (dscode.Tables.Count > 0 && dscode.Tables[0].Rows.Count > 0)
            {
                string course = dscode.Tables[0].Rows[0]["Course_Name"].ToString();
                string deptname = dscode.Tables[0].Rows[0]["Dept_Name"].ToString();
                coursename = course + "-" + deptname;
            }
            string str1 = string.Empty;
            if ((grouporusercode.Trim() != "") && (grouporusercode.Trim() != "0") && (grouporusercode.Trim() != "-1"))
            {
                str1 = "select distinct textname,taxtval from Attendance_Settings where  college_code='" + collegeCode + "'";
            }
            else
            {
                str1 = "select distinct textname,taxtval from Attendance_Settings where college_code='" + collegeCode + "'";
            }
            Boolean flage = false;
            DataSet ds1;
            ds1 = da.select_method_wo_parameter(str1, "txt");
            if (ds1.Tables.Count > 0 && ds1.Tables[0].Rows.Count > 0)
            {
                for (int jj = 0; jj < ds1.Tables[0].Rows.Count; jj++)
                {
                    if (ds1.Tables[0].Rows[jj]["TextName"].ToString() == "Voice Call for Absent" && ds1.Tables[0].Rows[jj]["Taxtval"].ToString() == "1")
                    {
                        flage = true;
                    }
                }
                if (flage == true)
                {
                    for (int k = 0; k < ds1.Tables[0].Rows.Count; k++)
                    {
                        if (ds1.Tables[0].Rows[k]["TextName"].ToString() == "Period" && ds1.Tables[0].Rows[k]["Taxtval"].ToString() != "")
                        {
                            string splihours = ds1.Tables[0].Rows[k]["Taxtval"].ToString();
                            string[] fin_split = splihours.Split(',');
                            int count = fin_split.Length;
                            for (int i = 0; i < count; i++)
                            {
                                string final_Hours = fin_split[i];
                                if (hour_check == final_Hours)
                                {
                                    check = check + 1;
                                }
                            }
                        }
                    }
                }
            }
            if (check > 0)
            {
                check = 0;
                string Phone = "select distinct isnull(a.parentF_Mobile,'0') as FatherMobile,isnull(a.parentM_Mobile,'0')as MotherMobile,isnull(a.Student_Mobile,'0') as StudentMobile,a.sex as Gender,isnull(a.stud_name,r.stud_name) as StudName,VoiceLang from applyn a,registration r where a.app_no=r.app_no and r.roll_no='" + rollno + "' and r.college_code='" + collegeCode + "'";
                DataSet dsMobile;
                dsMobile = da.select_method_wo_parameter(Phone, "txt");
                string str = string.Empty;
                if ((grouporusercode.Trim() != "") && (grouporusercode.Trim() != "0") && (grouporusercode.Trim() != "-1"))
                {
                    str = "select distinct textname,taxtval from Attendance_Settings where  college_code='" + collegeCode + "'";
                }
                else
                {
                    str = "select distinct textname,taxtval from Attendance_Settings where  college_code='" + collegeCode + "'";
                }
                string voicelang = Convert.ToString(dsMobile.Tables[0].Rows[0]["VoiceLang"]);
                if (voicelang != "")
                {
                    string langquery = string.Empty;
                    langquery = "select TextVal from textvaltable where TextCode  ='" + voicelang + "' and TextCriteria='PLang' and college_code=" + collegeCode + "";
                    DataSet datalang = new DataSet();
                    datalang = da.select_method_wo_parameter(langquery, "Text");
                    if (datalang.Tables[0].Rows.Count > 0)
                    {
                        voicelanguage = datalang.Tables[0].Rows[0]["TextVal"].ToString();
                    }
                }
                // voicelanguage = "English";
                DataSet ds;
                ds = da.select_method_wo_parameter(str, "txt");
                if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0 && dsMobile.Tables.Count > 0 && dsMobile.Tables[0].Rows.Count > 0)
                {
                    //    if (Convert.ToInt16(dsMobile.Tables[0].Rows[0]["Gender"].ToString()) == 0)
                    //    {
                    //        Gender = "Your Son ";
                    //    }
                    //    else
                    //    {
                    //        Gender = "Your daughter";
                    //    }
                    //    string studentname = dsMobile.Tables[0].Rows[0]["stud_name"].ToString();
                    //    string[] splitname = studentname.Split('.');
                    //    string finalstudentname = splitname[0].ToString();
                    //    if (System.IO.File.Exists("C:\\Documents and Settings\\Admin\\Desktop\\cutter\\dear parents.wav") == true)
                    //    {
                    //        if (System.IO.File.Exists("C:\\Documents and Settings\\Admin\\Desktop\\cutter\\good morning.wav") == true)
                    //        {
                    //            if (System.IO.File.Exists("C:\\Documents and Settings\\Admin\\Desktop\\cutter\\this call from.wav") == true)
                    //            {
                    //                if (System.IO.File.Exists("C:\\Documents and Settings\\Admin\\Desktop\\cutter\\javagar school.wav") == true)
                    //                {
                    //                    if (System.IO.File.Exists("C:\\Documents and Settings\\Admin\\Desktop\\cutter\\" + Gender + ".wav") == true)
                    //                    {
                    //                        if (System.IO.File.Exists("C:\\Documents and Settings\\Admin\\Desktop\\cutter\\" + finalstudentname + ".wav") == true)
                    //                        {
                    //                            if (System.IO.File.Exists("C:\\Documents and Settings\\Admin\\Desktop\\cutter\\" + coursename.ToString() + ".wav") == true)
                    //                            {
                    //                                if (System.IO.File.Exists("C:\\Documents and Settings\\Admin\\Desktop\\cutter\\is absent today.wav") == true)
                    //                                {
                    //                                    if (System.IO.File.Exists("C:\\Documents and Settings\\Admin\\Desktop\\cutter\\at 7th.wav") == true)
                    //                                    {
                    //                                        if (System.IO.File.Exists("C:\\Documents and Settings\\Admin\\Desktop\\cutter\\thank you.wav") == true)
                    //                                        {
                    //                                            string[] files = new string[10] { "C:\\Documents and Settings\\Admin\\Desktop\\cutter\\dear parents.wav", "C:\\Documents and Settings\\Admin\\Desktop\\cutter\\good morning.wav", "C:\\Documents and Settings\\Admin\\Desktop\\cutter\\this call from.wav", "C:\\Documents and Settings\\Admin\\Desktop\\cutter\\javagar school.wav",
                    //                                          "C:\\Documents and Settings\\Admin\\Desktop\\cutter\\" + Gender + ".wav" ,"C:\\Documents and Settings\\Admin\\Desktop\\cutter\\" + studentname + ".wav","C:\\Documents and Settings\\Admin\\Desktop\\cutter\\" + coursename.ToString() + ".wav","C:\\Documents and Settings\\Admin\\Desktop\\cutter\\is absent today.wav","C:\\Documents and Settings\\Admin\\Desktop\\cutter\\at 7th.wav","C:\\Documents and Settings\\Admin\\Desktop\\cutter\\thank you.wav"};
                    //                                            // WaveIO wa = new WaveIO();
                    //                                            Concatenate(Server.MapPath("~/UploadFiles/chinnamaili.wav"), files);
                    //                                            filepath = Server.MapPath("~/UploadFiles/chinnamaili.wav");
                    //                                            insertmethod(filepath);
                    //                                        }
                    //                                    }
                    //                                }
                    //                            }
                    //                        }
                    //                    }
                    //                }
                    //            }
                    //        }
                    //    }
                    //    FileInfo fileinfo = new FileInfo(filepath);
                    //    string filename = fileinfo.Name;
                    string gender = string.Empty;
                    if (Convert.ToInt16(dsMobile.Tables[0].Rows[0]["Gender"].ToString()) == 0)
                    {
                        gender = "MALE";
                    }
                    else
                    {
                        gender = "FEMALE";
                    }
                    string orginalname = string.Empty;
                    string student_name = Convert.ToString(dsMobile.Tables[0].Rows[0]["StudName"]);
                    if (student_name.Contains(".") == true)
                    {
                        string[] splitname = student_name.Split('.');
                        for (int i = 0; i <= splitname.GetUpperBound(0); i++)
                        {
                            string lengthname = splitname[i].ToString();
                            if (lengthname.Trim().Length > 2)
                            {
                                orginalname = splitname[i].ToString();
                            }
                        }
                    }
                    else
                    {
                        string[] split2ndname = student_name.Split(' ');
                        if (split2ndname.Length > 0)
                        {
                            for (int k = 0; k <= split2ndname.GetUpperBound(0); k++)
                            {
                                string firstname = split2ndname[k].ToString();
                                if (firstname.Trim().Length > 2)
                                {
                                    if (orginalname == "")
                                    {
                                        orginalname = firstname.ToString();
                                    }
                                    else
                                    {
                                        orginalname = orginalname + " " + firstname.ToString();
                                    }
                                }
                            }
                        }
                    }
                    DateTime dt = Convert.ToDateTime(date);
                    for (int jj1 = 0; jj1 < ds.Tables[0].Rows.Count; jj1++)
                    {
                        if (ds.Tables[0].Rows[jj1]["TextName"].ToString() == "Father" && ds.Tables[0].Rows[jj1]["Taxtval"].ToString() == "1")
                        {
                            if (dsMobile.Tables[0].Rows[0]["FatherMobile"].ToString() != "0")
                            {
                                //  DateTime dt = Convert.ToDateTime(date);
                                MsgText = "ABSETN AT ";
                                RecepientNo = dsMobile.Tables[0].Rows[0]["FatherMobile"].ToString();
                                //Modified By Srinath
                                //string strpath = "http://dnd.airsmsmarketing.info/api/sendmsg.php?user=" + user_id + "&pass=" + Password + "&sender=" + SenderID + "&phone=" + RecepientNo + "&text=" + MsgText + "&priority=ndnd&stype=normal";
                                // string strpath = "http://inter.onlinespeedsms.in/sendhttp.php?user=" + user_id.ToLower() + "&password=" + Password + "&mobiles=" + RecepientNo + "&message=" + MsgText + "&sender=" + SenderID;
                                biz.lbinfotech.www.Data h = new biz.lbinfotech.www.Data();
                                string NEW = h.GetData("" + RecepientNo + "", "ATTENDANCE", "DAILYHOUR", "" + collegename + "", "" + orginalname + "", "" + gender + "", "" + batchyear + "", "" + coursename + "", "" + roll + "", "" + dt.ToString("yyyy-MM-dd") + "", "" + hour + "", "" + MsgText + "", "" + voicelanguage.ToString() + "");
                                string isst = "0";
                                //smsreport(strpath, isst, dt);
                            }
                        }
                        if (ds.Tables[0].Rows[jj1]["TextName"].ToString() == "Mother" && ds.Tables[0].Rows[jj1]["Taxtval"].ToString() == "1")
                        {
                            if (dsMobile.Tables[0].Rows[0]["MotherMobile"].ToString() != "0")
                            {
                                // DateTime dt = Convert.ToDateTime(date);
                                MsgText = " ABSETN AT ";
                                RecepientNo = dsMobile.Tables[0].Rows[0]["MotherMobile"].ToString();
                                //Modified By SRinath /2/2014
                                //string strpath = "http://dnd.airsmsmarketing.info/api/sendmsg.php?user=" + user_id + "&pass=" + Password + "&sender=" + SenderID + "&phone=" + RecepientNo + "&text=" + MsgText + "&priority=ndnd&stype=normal";
                                //  string strpath = "http://inter.onlinespeedsms.in/sendhttp.php?user=" + user_id.ToLower() + "&password=" + Password + "&mobiles=" + RecepientNo + "&message=" + MsgText + "&sender=" + SenderID;
                                biz.lbinfotech.www.Data h = new biz.lbinfotech.www.Data();
                                string NEW = h.GetData("" + RecepientNo + "", "ATTENDANCE", "DAILYHOUR", "" + collegename + "", "" + orginalname + "", "" + gender + "", "" + batchyear + "", "" + coursename + "", "" + roll + "", "" + dt.ToString("yyyy-MM-dd") + "", "" + hour + "", "" + MsgText + "", "" + voicelanguage.ToString() + "");
                                string isst = "0";
                                //smsreport(strpath, isst, dt);
                            }
                        }
                        if (ds.Tables[0].Rows[jj1]["TextName"].ToString() == "Student" && ds.Tables[0].Rows[jj1]["Taxtval"].ToString() == "1")
                        {
                            if (dsMobile.Tables[0].Rows[0]["StudentMobile"].ToString() != "0")
                            {
                                MsgText = " ABSENT AT";
                                RecepientNo = dsMobile.Tables[0].Rows[0]["StudentMobile"].ToString();
                                //Modified By Srinatrh 8/2/2014
                                //string strpath = "http://dnd.airsmsmarketing.info/api/sendmsg.php?user=" + user_id + "&pass=" + Password + "&sender=" + SenderID + "&phone=" + RecepientNo + "&text=" + MsgText + "&priority=ndnd&stype=normal";
                                //string strpath = "http://inter.onlinespeedsms.in/sendhttp.php?user=" + user_id.ToLower() + "&password=" + Password + "&mobiles=" + RecepientNo + "&message=" + MsgText + "&sender=" + SenderID;
                                biz.lbinfotech.www.Data h = new biz.lbinfotech.www.Data();
                                string NEW = h.GetData("" + RecepientNo + "", "ATTENDANCE", "DAILYHOUR", "" + collegename + "", "" + orginalname + "", "" + gender + "", "" + batchyear + "", "" + coursename + "", "" + roll + "", "" + dt.ToString("yyyy-MM-dd") + "", "" + hour + "", "" + MsgText + "", "" + voicelanguage.ToString() + "");
                                string isst = "0";
                                //smsreport(strpath, isst, dt);
                            }
                        }
                    }
                    //}
                    //}
                }
            }
        }
        catch
        {
        }
    }

    //Hour Lock
    public Boolean Hour_lock(string degree_code, string batch_year, string semester, string prd, string secval)
    {
        Hashtable ht_period = new Hashtable();
        Hashtable ht_bell = new Hashtable();
        Hashtable hat = new Hashtable();
        hat.Add("college_code", collegeCode);
        string vari = string.Empty;
        bool hr_lock = false;
        string degree_var = string.Empty;
        string sql_stringvar = "sp_select_details_staff";
        DataSet ds_attndmaster = new DataSet();

        ds_attndmaster = da.select_method(sql_stringvar, hat, "sp");

        ht_bell.Clear();

        if (ds_attndmaster.Tables.Count > 2 && ds_attndmaster.Tables[2].Rows.Count > 0)
        {
            for (int pcont = 0; pcont < ds_attndmaster.Tables[2].Rows.Count; pcont++)
            {
                degree_var = Convert.ToString(ds_attndmaster.Tables[2].Rows[pcont]["batch_year"]) + "-" + Convert.ToString(ds_attndmaster.Tables[2].Rows[pcont]["degree_code"]) + "-" + Convert.ToString(ds_attndmaster.Tables[2].Rows[pcont]["semester"]) + "-" + Convert.ToString(ds_attndmaster.Tables[2].Rows[pcont]["period1"]);
                if (!ht_bell.Contains(Convert.ToString(degree_var)))
                {
                    vari = ds_attndmaster.Tables[2].Rows[pcont]["start_time"] + "," + ds_attndmaster.Tables[2].Rows[pcont]["end_time"];
                    ht_bell.Add(degree_var, Convert.ToString(vari));
                }
            }
        }
        ht_period.Clear();

        if (ds_attndmaster.Tables.Count > 3 && ds_attndmaster.Tables[3].Rows.Count > 0)
        {
            for (int pcont = 0; pcont < ds_attndmaster.Tables[3].Rows.Count; pcont++)
            {
                degree_var = Convert.ToString(ds_attndmaster.Tables[3].Rows[pcont]["lock_hr"]);
                if (!ht_period.Contains(Convert.ToString(degree_var)))
                {
                    vari = ds_attndmaster.Tables[3].Rows[pcont]["markatt_from"] + "," + ds_attndmaster.Tables[3].Rows[pcont]["markatt_to"];
                    ht_period.Add(degree_var, Convert.ToString(vari));
                }
            }
        }
        hr_lock = false;
        if (ds_attndmaster.Tables.Count > 4 && ds_attndmaster.Tables[4].Rows.Count > 0)
        {
            string locktrue = ds_attndmaster.Tables[4].Rows[0]["hrlock"].ToString();
            if (locktrue == "1")
            {
                hr_lock = true;
            }
        }

        string starttime = string.Empty;
        string endtime = string.Empty;
        string startperiod = string.Empty;
        string endperiod = string.Empty;
        string actualtime = string.Empty;
        string period = string.Empty;
        string[] sp = prd.Split(' ');
        DateTime current_time;
        DateTime start_time;
        DateTime end_time;
        Boolean lock_flag = false;
        if (sp.GetUpperBound(0) >= 1)
        {
            period = Convert.ToString(sp[1]);
        }

        string getlock = string.Empty;
        if (secval.Trim() != "")
        {
            getlock = da.GetFunction("select lockstatus from attendance_hrlock where degree_code='" + degree_code + "' and batch_year='" + batch_year + "' and semester='" + semester + "' and section='" + secval + "' and locktype=2");
        }
        else
        {
            getlock = da.GetFunction("select lockstatus from attendance_hrlock where degree_code='" + degree_code + "' and batch_year='" + batch_year + "' and semester='" + semester + "' and locktype=2 ");
        }
        if (getlock.Trim().ToLower() == "true" || getlock.Trim() == "1")
        {
            hr_lock = true;
        }

        if (hr_lock == true)
        {
            if (ht_period.Count > 0)
            {
                if (ht_period.Contains(Convert.ToString(period)))
                {
                    string contvar = Convert.ToString(GetCorrespondingKey(Convert.ToString(period), ht_period));
                    string[] sp_rd_semi = contvar.Split(',');
                    if (sp_rd_semi.GetUpperBound(0) >= 1) //Get Mark attendance Hrs for lock
                    {
                        startperiod = Convert.ToString(sp_rd_semi[0]);
                        endperiod = Convert.ToString(sp_rd_semi[1]);
                        if (ht_bell.Count > 0)
                        {
                            degree_var = Convert.ToString(batch_year) + "-" + Convert.ToString(degree_code) + "-" + Convert.ToString(semester) + "-" + Convert.ToString(startperiod);
                            if (ht_bell.Contains(Convert.ToString(degree_var))) //Get period start time for lock
                            {
                                string contvar1 = Convert.ToString(GetCorrespondingKey(Convert.ToString(degree_var), ht_bell));
                                string[] sp_rd_semi1 = contvar1.Split(',');
                                if (sp_rd_semi1.GetUpperBound(0) >= 1)
                                {
                                    starttime = Convert.ToString(sp_rd_semi1[0]);
                                }
                            }
                            degree_var = Convert.ToString(batch_year) + "-" + Convert.ToString(degree_code) + "-" + Convert.ToString(semester) + "-" + Convert.ToString(endperiod);
                            if (ht_bell.Contains(Convert.ToString(degree_var))) //Get period end time for lock
                            {
                                string contvar1 = Convert.ToString(GetCorrespondingKey(Convert.ToString(degree_var), ht_bell));
                                string[] sp_rd_semi1 = contvar1.Split(',');
                                if (sp_rd_semi1.GetUpperBound(0) >= 1)
                                {
                                    endtime = Convert.ToString(sp_rd_semi1[1]);
                                }
                            }
                            sql_stringvar = "SELECT LTRIM(RIGHT(CONVERT(VARCHAR(20), GETDATE(), 100), 7))as time";
                            hat.Clear();
                            ds_attndmaster.Clear();
                            ds_attndmaster = da.select_method(sql_stringvar, hat, "Text");
                            if (ds_attndmaster.Tables.Count > 0 && ds_attndmaster.Tables[0].Rows.Count > 0)
                            {
                                actualtime = Convert.ToString(ds_attndmaster.Tables[0].Rows[0]["time"]);
                            }
                            if (starttime.ToString().Trim() != "" && endtime.ToString().Trim() != "" && actualtime.ToString().Trim() != "")
                            {
                                current_time = Convert.ToDateTime(actualtime);
                                start_time = Convert.ToDateTime(starttime);
                                end_time = Convert.ToDateTime(endtime);
                                if (current_time >= start_time && current_time <= end_time)
                                {
                                    lock_flag = false;
                                }
                                else
                                {
                                    lock_flag = true;
                                }
                            }
                        }
                    }
                }
            }
        }
        return lock_flag;
    }

    //Day Lock
    public bool daycheck(DateTime seldate, string degCode, string sem)
    {
        string collegecode = collegeCode;
        bool daycheck = false;
        DateTime curdate;//, prevdate;
        long total, k, s;
        string[] ddate;
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
            Hashtable hat = new Hashtable();
            //Modified by srinath 12/8/2013
            string lockdayvalue = "select lockdays,lflag from collinfo where college_code=" + collegecode + "";
            DataSet ds = new DataSet();
            ds = da.select_method(lockdayvalue, hat, "Text");
            // da.Fill(ds);
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    if (ds.Tables[0].Rows[i][1].ToString() == "True")
                    {
                        if (ds.Tables[0].Rows[i][0].ToString() != null && int.Parse(ds.Tables[0].Rows[i][0].ToString()) >= 0)
                        {
                            total = int.Parse(ds.Tables[0].Rows[i][0].ToString());
                            total = total + 1;
                            //Modified by srinath 12/8/2013
                            String strholidasquery = "select holiday_date from holidaystudents where degree_code=" + degCode + "  and semester=" + sem + "";
                            DataSet ds1 = new DataSet();
                            ds1 = da.select_method(strholidasquery, hat, "Text");
                            //if (ds1.Tables[0].Rows.Count <= 0)
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
                                ddate = new string[ds1.Tables[0].Rows.Count];
                                for (int i1 = 1; i1 < ds1.Tables[0].Rows.Count; i1++)
                                {
                                    ddate[k] = ds1.Tables[0].Rows[i1][0].ToString();
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

    //Get Key for corresponding hashtable
    public object GetCorrespondingKey(object key, Hashtable hashTable)
    {
        IDictionaryEnumerator e = hashTable.GetEnumerator();
        while (e.MoveNext())
        {
            if (e.Key.ToString() == key.ToString())
            {
                return e.Value.ToString();
            }
        }
        return null;
    }

    #endregion

    /// <summary>
    /// It is Used to Get Day Name For DayOrder
    /// Developed By Malang Raja T
    /// </summary>
    /// <param name="dayOrder"></param>
    /// <returns></returns>
    private string findDayName(byte dayOrder)
    {
        string dayName = string.Empty;
        switch (dayOrder)
        {
            case 0:
                dayName = string.Empty;
                break;
            case 1:
                dayName = "mon";
                break;
            case 2:
                dayName = "tue";
                break;
            case 3:
                dayName = "wed";
                break;
            case 4:
                dayName = "thu";
                break;
            case 5:
                dayName = "fri";
                break;
            case 6:
                dayName = "sat";
                break;
            case 7:
                dayName = "sun";
                break;
            default:
                break;
        }
        return dayName;
    }

    /// <summary>
    /// This Is Used to Get All Fee of Students 
    /// Developed By Malang Raja T
    /// </summary>
    /// <param name="dicFeeOfRoll">referenced type Dictionary To Hold Fee of Roll Student</param>
    /// <param name="dtFromDate">From Date Format dd-MM-yyyy</param>
    /// <param name="dtToDate">To Date Format dd-MM-yyyy</param>
    private void GetFeeOfRollStudent(ref Dictionary<string, DateTime[]> dicFeeOffRollStudents, ref Dictionary<string, byte> dicFeeOnRoll, string fromDate = null, string toDate = null)
    {
        try
        {
            DataSet dsFeeOfRollDate = new DataSet();
            DateTime dtFromDate = new DateTime();
            DateTime dtToDate = new DateTime();
            bool isFromSuccess = false;
            bool isToSuccess = false;
            if (!string.IsNullOrEmpty(fromDate))
            {
                isFromSuccess = DateTime.TryParseExact(fromDate, "dd-MM-yyyy", null, DateTimeStyles.None, out dtFromDate);
            }
            if (!string.IsNullOrEmpty(toDate))
            {
                isToSuccess = DateTime.TryParseExact(toDate, "dd-MM-yyyy", null, DateTimeStyles.None, out dtToDate);
            }
            string qryFeeOfRollDate = string.Empty;
            if (isFromSuccess && isToSuccess)
            {
                qryFeeOfRollDate = " and curr_date between '" + dtFromDate.ToString("mm/dd/yyyy") + "' and '" + dtToDate.ToString("mm/dd/yyyy") + "'";
            }
            else if (isFromSuccess)
            {
                qryFeeOfRollDate = " and curr_date='" + dtFromDate.ToString("mm/dd/yyyy") + "'";
            }
            else if (isToSuccess)
            {
                qryFeeOfRollDate = " and curr_date='" + dtToDate.ToString("mm/dd/yyyy") + "'";
            }
            else
            {
                qryFeeOfRollDate = string.Empty;
            }
            //qry = "select roll_no, Convert(varchar(50),curr_date,103) as curr_date,infr_type,Convert(varchar(50),CAST(ack_date as DateTime),103) as ack_date,Convert(varchar(50),feeOnRollDate,103) as feeOnRollDate,ack_diss,ack_fine,ack_remarks,ack_susp,ack_warn,tot_days,fine_amo,semester,ack_fee_of_roll,Remark,Convert(varchar(50),suspendFromDate,103) as suspendFromDate,Convert(varchar(50),suspendToDate,103) as suspendToDate from stucon where (ack_fee_of_roll=1 or feeOnRollDate is not null) " + qryFeeOfRollDate;
            string qry = "select roll_no,Convert(varchar(50),curr_date,103) as curr_date,Convert(varchar(50),CAST(ack_date as DateTime),103) as ack_date,Convert(varchar(50),feeOnRollDate,103) as feeOnRollDate,semester,ack_fee_of_roll from stucon where (ack_fee_of_roll=1 or feeOnRollDate is not null) " + qryFeeOfRollDate;
            dsFeeOfRollDate = da.select_method_wo_parameter(qry, "text");
            if (dsFeeOfRollDate.Tables.Count > 0 && dsFeeOfRollDate.Tables[0].Rows.Count > 0)
            {
                dicFeeOffRollStudents.Clear();
                foreach (DataRow drFeeOfRoll in dsFeeOfRollDate.Tables[0].Rows)
                {
                    string rollNo = Convert.ToString(drFeeOfRoll["roll_no"]).Trim().ToLower();
                    string feeOffRollDate = Convert.ToString(drFeeOfRoll["curr_date"]).Trim();
                    string feeOffRollDate1 = Convert.ToString(drFeeOfRoll["ack_date"]).Trim();
                    string feeOnRollDate = Convert.ToString(drFeeOfRoll["feeOnRollDate"]).Trim();
                    string isFeeOfRoll = Convert.ToString(drFeeOfRoll["ack_fee_of_roll"]).Trim();
                    byte FeeOnRoll = 0;
                    byte.TryParse(isFeeOfRoll.Trim(), out FeeOnRoll);
                    DateTime dtFeeOffRollDate = new DateTime();
                    DateTime dtFeeOnRollDate = new DateTime();
                    bool isFeeOff = DateTime.TryParseExact(feeOffRollDate, "dd/MM/yyyy", null, DateTimeStyles.None, out dtFeeOffRollDate);
                    bool isFeeOn = DateTime.TryParseExact(feeOnRollDate, "dd/MM/yyyy", null, DateTimeStyles.None, out dtFeeOnRollDate);
                    DateTime[] dtFeeRoll = new DateTime[2];
                    dtFeeRoll[0] = dtFeeOffRollDate;
                    dtFeeRoll[1] = dtFeeOnRollDate;
                    if (!isFeeOn)
                    {
                        //dtFeeOnRollDate = ;
                    }
                    if (!dicFeeOffRollStudents.ContainsKey(rollNo.Trim().ToLower()))
                    {
                        dicFeeOffRollStudents.Add(rollNo.Trim().ToLower(), dtFeeRoll);
                    }
                    if (!dicFeeOnRoll.ContainsKey(rollNo.Trim().ToLower().ToLower()))
                    {
                        dicFeeOnRoll.Add(rollNo.Trim().ToLower(), FeeOnRoll);
                    }
                }
            }
        }
        catch
        {
        }
    }

    private bool CheckSchoolOrCollege(string collegeCode)
    {
        bool isSchoolOrCollege = false;
        try
        {
            if (!string.IsNullOrEmpty(collegeCode))
            {
                //qry = "select ISNULL(InstType,'0') as InstType,case when ISNULL(InstType,'0')='0' then 'College' when ISNULL(InstType,'0')='1' then 'School' end as CollegeOrSchool from collinfo where college_code='" + collegeCode + "'";
                string qry = "select ISNULL(InstType,'0') as InstType from collinfo where college_code='" + collegeCode + "'";
                string insType = da.GetFunction(qry);
                if (!string.IsNullOrEmpty(insType) && insType.Trim() != "0")
                {
                    isSchoolOrCollege = true;
                }
                else
                {
                    isSchoolOrCollege = false;
                }

            }
            return isSchoolOrCollege;
        }
        catch
        {
            return false;
        }
    }

    private bool IsShowStudentAttendanceCount()
    {
        bool isVisible = false;
        try
        {
            string grouporusercode1 = string.Empty;
            if (Session["group_code"] != null && (Convert.ToString(Session["group_code"]).Trim() != "") && (Convert.ToString(Session["group_code"]).Trim() != "0") && (Convert.ToString(Session["group_code"]).Trim() != "-1"))
            {
                grouporusercode1 = " and group_code=" + Convert.ToString(Session["group_code"]).Trim().Split(',')[0] + "";
            }
            else if (Session["usercode"] != null)
            {
                grouporusercode1 = " and usercode=" + Convert.ToString(Session["usercode"]).Trim() + "";
            }
            string qry = "select value from Master_Settings where settings='Require Students Attendance Count' " + grouporusercode1;
            string value = dirAcc.selectScalarString(qry);
            if (!string.IsNullOrEmpty(value) && value.Trim() == "1")
            {
                isVisible = true;
            }
        }
        catch { return false; }
        return isVisible;
    }

    private bool IsShowAttendanceReason()
    {
        bool isVisible = false;
        try
        {
            string grouporusercode1 = string.Empty;
            if (Session["group_code"] != null && (Convert.ToString(Session["group_code"]).Trim() != "") && (Convert.ToString(Session["group_code"]).Trim() != "0") && (Convert.ToString(Session["group_code"]).Trim() != "-1"))
            {
                grouporusercode1 = " and group_code=" + Convert.ToString(Session["group_code"]).Trim().Split(',')[0] + "";
            }
            else if (Session["usercode"] != null)
            {
                grouporusercode1 = " and usercode=" + Convert.ToString(Session["usercode"]).Trim() + "";
            }
            string qry = "select value from Master_Settings where settings='Require Reason in the Staff Attendance' " + grouporusercode1;
            string value = dirAcc.selectScalarString(qry);
            if (!string.IsNullOrEmpty(value) && value.Trim() == "1")
            {
                isVisible = true;
            }
        }
        catch { return false; }
        return isVisible;
    }

    private bool IsShowPresentAbsentOnly()
    {
        bool isVisible = false;
        try
        {
            string grouporusercode1 = string.Empty;
            if (Session["group_code"] != null && (Convert.ToString(Session["group_code"]).Trim() != "") && (Convert.ToString(Session["group_code"]).Trim() != "0") && (Convert.ToString(Session["group_code"]).Trim() != "-1"))
            {
                grouporusercode1 = " and group_code=" + Convert.ToString(Session["group_code"]).Trim().Split(',')[0] + "";
            }
            else if (Session["usercode"] != null)
            {
                grouporusercode1 = " and usercode=" + Convert.ToString(Session["usercode"]).Trim() + "";
            }
            string qry = "select value from Master_Settings where settings='Require Present/Absent Only' " + grouporusercode1;
            string value = dirAcc.selectScalarString(qry);
            if (!string.IsNullOrEmpty(value) && value.Trim() == "1")
            {
                isVisible = true;
            }
        }
        catch { return false; }
        return isVisible;
    }

    public void isholidayCheck(string college_code, string degree_code, string semester, string frdate, out bool ishoilday, out bool isholimorn, out bool isholieven, out int fhrs)
    {
        Hashtable holiday_table = new Hashtable();
        DAccess2 d2 = new DAccess2();
        Hashtable hat = new Hashtable();
        DataSet ds2 = new DataSet();
        DataSet ds_holi = new DataSet();
        DateTime dumm_from_date = new DateTime();
        string[] dsplit = frdate.Split(new Char[] { '/' });
        frdate = Convert.ToString(dsplit[2]) + "/" + Convert.ToString(dsplit[1]) + "/" + Convert.ToString(dsplit[0]);
        dumm_from_date = Convert.ToDateTime(frdate);
        ishoilday = false;
        isholimorn = false;
        isholieven = false;
        fhrs = 0;
        try
        {
            hat.Clear();
            hat.Add("degree_code", degree_code);
            hat.Add("sem", semester);
            hat.Add("from_date", frdate);
            hat.Add("to_date", frdate);
            hat.Add("coll_code", college_code);
            int iscount = 0;
            string strquery = "select  isnull(count(holiday_date),0)as cnt FROM holidayStudents where holiday_date between '" + frdate + "' and '" + frdate + "' and degree_code=" + degree_code + " and semester=" + semester + "";
            ds2.Reset();
            ds2.Dispose();
            ds2 = d2.select_method(strquery, hat, "Text");
            if (ds2.Tables.Count > 0 && ds2.Tables[0].Rows.Count > 0)
            {
                iscount = 0;
                int.TryParse(Convert.ToString(ds2.Tables[0].Rows[0]["cnt"]), out iscount);
            }
            hat.Add("iscount", iscount);
            ds_holi = d2.select_method("ALL_HOLIDATE_DETAILS", hat, "sp");
            holiday_table.Clear();
            if (ds_holi.Tables.Count > 0 && ds_holi.Tables[0].Rows.Count != 0)
            {
                for (int k = 0; k < ds_holi.Tables[0].Rows.Count; k++)
                {
                    if (!holiday_table.ContainsKey(Convert.ToDateTime(Convert.ToString(ds_holi.Tables[0].Rows[k]["HOLI_DATE"]))))
                    {
                        holiday_table.Add(Convert.ToDateTime(Convert.ToString(ds_holi.Tables[0].Rows[k]["HOLI_DATE"])), k);
                    }
                }
            }
            if (ds_holi.Tables.Count > 1 && ds_holi.Tables[1].Rows.Count != 0)
            {
                for (int k = 0; k < ds_holi.Tables[1].Rows.Count; k++)
                {
                    if (!holiday_table.ContainsKey(Convert.ToDateTime(Convert.ToString(ds_holi.Tables[1].Rows[k]["HOLI_DATE"]))))
                    {
                        holiday_table.Add(Convert.ToDateTime(Convert.ToString(ds_holi.Tables[1].Rows[k]["HOLI_DATE"])), k);
                    }
                }
            }
            if (ds_holi.Tables.Count > 2 && ds_holi.Tables[2].Rows.Count != 0)
            {
                for (int k = 0; k < ds_holi.Tables[2].Rows.Count; k++)
                {
                    if (!holiday_table.ContainsKey(Convert.ToDateTime(Convert.ToString(ds_holi.Tables[2].Rows[k]["HOLI_DATE"]))))
                    {
                        holiday_table.Add(Convert.ToDateTime(Convert.ToString(ds_holi.Tables[2].Rows[k]["HOLI_DATE"])), k);
                    }
                }
            }
            fhrs = 0;
            string hrs = d2.GetFunction("select no_of_hrs_I_half_day from periodattndschedule where degree_code=" + degree_code + " and semester='" + semester + "'");
            if (hrs.Trim() != "" && hrs != null && hrs.Trim() != "0")
            {
                int.TryParse(hrs, out fhrs);
            }
            if (!holiday_table.ContainsKey(dumm_from_date))
            {
                ishoilday = false;
                isholimorn = false;
                isholieven = false;
            }
            else
            {
                ishoilday = true;
                isholimorn = false;
                isholieven = false;
                int starthout = 0;
                string strholyquery = "select halforfull,morning,evening from holidaystudents where halforfull=1 and holiday_date='" + dumm_from_date.ToString("MM/dd/yyyy") + "'";
                DataSet dsholidayval = d2.select_method_wo_parameter(strholyquery, "Text");
                if (dsholidayval.Tables.Count > 0 && dsholidayval.Tables[0].Rows.Count > 0)
                {
                    if (Convert.ToString(dsholidayval.Tables[0].Rows[0]["morning"]) == "1" || Convert.ToString(dsholidayval.Tables[0].Rows[0]["morning"]).Trim().ToLower() == "true")
                    {
                        ishoilday = false;
                        isholimorn = true;
                        isholieven = false;
                    }
                    if (Convert.ToString(dsholidayval.Tables[0].Rows[0]["evening"]) == "1" || Convert.ToString(dsholidayval.Tables[0].Rows[0]["evening"]).Trim().ToLower() == "true")
                    {
                        isholimorn = false;
                        ishoilday = false;
                        isholieven = true;
                    }
                }
                else
                {
                    ishoilday = true;
                    isholimorn = true;
                    isholieven = true;
                }
            }
        }
        catch (Exception ex)
        {

        }
    }

    /// <summary>
    /// Developed By Malang Raja on Dec 7 2016
    /// </summary>
    /// <param name="type">0 For Roll No,1 For Register No,2 For Admission No, 3 For Student Type , 4 For Application No</param>
    /// <param name="dsSettingsOptional">it is Optional Parameter</param>
    /// <returns>true or false</returns>
    private bool ColumnHeaderVisiblity(int type, DataSet dsSettingsOptional = null)
    {
        bool hasValues = false;
        try
        {
            DataSet dsSettings = new DataSet();
            if (dsSettingsOptional == null)
            {
                string grouporusercode = string.Empty;
                if (Session["group_code"] != null && (Convert.ToString(Session["group_code"]).Trim() != "") && (Convert.ToString(Session["group_code"]).Trim() != "0") && (Convert.ToString(Session["group_code"]).Trim() != "-1"))
                {
                    string groupCode = Convert.ToString(Session["group_code"]).Trim();
                    string[] groupUser = Convert.ToString(groupCode).Trim().Split(';');
                    if (groupUser.Length > 0)
                    {
                        groupCode = groupUser[0].Trim();
                    }
                    if (!string.IsNullOrEmpty(groupCode.Trim()))
                    {
                        grouporusercode = " and  group_code=" + Convert.ToString(groupCode).Trim() + "";
                    }
                }
                else if (Session["usercode"] != null)
                {
                    grouporusercode = " and usercode=" + Convert.ToString(Session["usercode"]).Trim() + "";
                }
                if (!string.IsNullOrEmpty(grouporusercode))
                {
                    string Master1 = "select * from Master_Settings where settings in('Roll No','Register No','Admission No','Student_Type','Application No') and value='1' " + grouporusercode + "";
                    dsSettings = da.select_method_wo_parameter(Master1, "Text");
                }
            }
            else
            {
                dsSettings = dsSettingsOptional;
            }
            if (dsSettings.Tables.Count > 0 && dsSettings.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow drSettings in dsSettings.Tables[0].Rows)
                {
                    switch (type)
                    {
                        case 0:
                            if (Convert.ToString(drSettings["settings"]).Trim().ToLower() == "roll no")
                            {
                                hasValues = true;
                            }
                            break;
                        case 1:
                            if (Convert.ToString(drSettings["settings"]).Trim().ToLower() == "register no")
                            {
                                hasValues = true;
                            }
                            break;
                        case 2:
                            if (Convert.ToString(drSettings["settings"]).Trim().ToLower() == "admission no")
                            {
                                hasValues = true;
                            }
                            break;
                        case 3:
                            if (Convert.ToString(drSettings["settings"]).Trim().ToLower() == "student_type")
                            {
                                hasValues = true;
                            }
                            break;
                        case 4:
                            if (Convert.ToString(drSettings["settings"]).Trim().ToLower() == "application no")
                            {
                                hasValues = true;
                            }
                            break;
                    }
                    if (hasValues)
                        break;
                }
            }
            return hasValues;
        }
        catch (Exception ex)
        {
            //lblErrSearch.Text = Convert.ToString(ex);
            //lblErrSearch.Visible = true;
            //da.sendErrorMail(ex, ((Session["collegecode"] != null) ? Convert.ToString(Session["collegecode"]).Trim() : ((ddlCollege.Items.Count > 0) ? Convert.ToString(ddlCollege.SelectedValue).Trim() : "13")), Convert.ToString(System.IO.Path.GetFileName(Request.Url.AbsolutePath)).Trim());
            return false;
        }
    }

    private string orderByStudents()
    {
        string orderBySetting = da.GetFunction("select value from master_Settings where settings='order_by'");
        orderBySetting = orderBySetting.Trim();
        string orderBy = "ORDER BY r.roll_no";
        switch (orderBySetting)
        {
            case "0":
                orderBy = "ORDER BY r.roll_no";
                break;
            case "1":
                orderBy = "ORDER BY r.Reg_No";
                break;
            case "2":
                orderBy = "ORDER BY r.Stud_Name";
                break;
            case "0,1,2":
                orderBy = "ORDER BY r.roll_no,r.Reg_No,r.stud_name";
                break;
            case "0,1":
                orderBy = "ORDER BY r.roll_no,r.Reg_No";
                break;
            case "1,2":
                orderBy = "ORDER BY r.Reg_No,r.Stud_Name";
                break;
            case "0,2":
                orderBy = "ORDER BY r.roll_no,r.Stud_Name";
                break;
            default:
                orderBy = "ORDER BY r.roll_no";
                break;
        }
        return orderBy;
    }

    protected void btnPopAlertClose_Click(object sender, EventArgs e)
    {
        try
        {
            lblAlertMsg.Text = string.Empty;
            divPopAlert.Visible = false;
        }
        catch (Exception ex)
        {

        }
    }

}
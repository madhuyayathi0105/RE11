using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using InsproDataAccess;
using System.Data;
using System.Drawing;
using Gios.Pdf;
using System.IO;
using System.Globalization;

public partial class AttendanceMOD_CondonationNewReport : System.Web.UI.Page
{
    #region Var Declaration
    InsproDirectAccess da = new InsproDirectAccess();
    DAccess2 d2 = new DAccess2();
    string usercode = string.Empty;
    string collegecode = string.Empty;
    string singleuser = string.Empty;
    string group_user = string.Empty;
    string grouporusercode = string.Empty;
    string RollNo = string.Empty;
    string BatchYr = string.Empty;
    string Semester = string.Empty;
    string conddate = string.Empty;
    string condchallan = string.Empty;
    string condamount = string.Empty; 
    #endregion

    [System.Web.Services.WebMethod]
    [System.Web.Script.Services.ScriptMethod()]
    public static List<string> GetRollNo(string prefixText)
    {
        WebService ws = new WebService();
        List<string> name = new List<string>();
        string query = "select Roll_No from Registration where DelFlag=0 and Exam_Flag <>'Debar' and Roll_No Like '" + prefixText + "%'  order by Roll_No";
        name = ws.Getname(query);
        return name;
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["collegecode"] == null)
        {
            Response.Redirect("~/Default.aspx");
        }
        usercode = Session["usercode"].ToString();
        collegecode = Session["collegecode"].ToString();
        singleuser = Session["single_user"].ToString();
        group_user = Session["group_code"].ToString();
        if (group_user.Contains(';'))
        {
            string[] group_semi = group_user.Split(';');
            group_user = group_semi[0].ToString();
        }
        if ((Session["group_code"].ToString().Trim() != "") && (Session["group_code"].ToString().Trim() != "0") && (Session["group_code"].ToString().Trim() != "-1"))
        {
            grouporusercode = " group_code=" + Session["group_code"].ToString().Trim() + "";
        }
        else
        {
            grouporusercode = " usercode=" + Session["usercode"].ToString().Trim() + "";
        }
        if (!IsPostBack)
        {
            divMainContent.Visible = false;
            lblmsg.Text = string.Empty;
            lblmsg.Visible = false;
        }
    }

    protected void btnGo_OnClick(object sender, EventArgs e)
    {
        divMainContent.Visible = false;
        lblmsg.Text = string.Empty;
        lblmsg.Visible = false;
        FarPoint.Web.Spread.StyleInfo style2 = new FarPoint.Web.Spread.StyleInfo();

        FpSpreadCondonationList.Sheets[0].ColumnCount = 0;
        FpSpreadCondonationList.Sheets[0].RowCount = 0;
        FpSpreadCondonationList.Sheets[0].SheetCorner.ColumnCount = 0;
        FpSpreadCondonationList.CommandBar.Visible = false;
        FpSpreadCondonationList.Sheets[0].ColumnCount = 14;
        FpSpreadCondonationList.Sheets[0].ColumnHeader.Cells[0, 0].Text = "S.No";
        FpSpreadCondonationList.Sheets[0].ColumnHeader.Cells[0, 1].Text = "Roll.No";
        FpSpreadCondonationList.Sheets[0].ColumnHeader.Cells[0, 2].Text = "Reg.No";
        FpSpreadCondonationList.Sheets[0].ColumnHeader.Cells[0, 3].Text = "Student Type";
        FpSpreadCondonationList.Sheets[0].ColumnHeader.Cells[0, 4].Text = "Student Name";
        FpSpreadCondonationList.Sheets[0].ColumnHeader.Cells[0, 5].Text = "Present Percentage";
        FpSpreadCondonationList.Sheets[0].ColumnHeader.Cells[0, 6].Text = "Absent Percentage";
        FpSpreadCondonationList.Sheets[0].ColumnHeader.Cells[0, 7].Text = "Fine Amount";
        FpSpreadCondonationList.Sheets[0].ColumnHeader.Cells[0, 8].Text = "Conducted Days";
        FpSpreadCondonationList.Sheets[0].ColumnHeader.Cells[0, 9].Text = "Present Days";
        FpSpreadCondonationList.Sheets[0].ColumnHeader.Cells[0, 10].Text = "Absent Days";
        FpSpreadCondonationList.Sheets[0].ColumnHeader.Cells[0, 11].Text = "Remarks";
        FpSpreadCondonationList.Sheets[0].ColumnHeader.Cells[0, 12].Text = "Select";
        FpSpreadCondonationList.Sheets[0].ColumnHeader.Cells[0, 13].Text = "Select";

        FpSpreadCondonationList.Sheets[0].Columns[0].Width = 50;
        FpSpreadCondonationList.Sheets[0].Columns[1].Width = 180;
        FpSpreadCondonationList.Sheets[0].Columns[2].Width = 180;
        FpSpreadCondonationList.Sheets[0].Columns[3].Width = 200;
        FpSpreadCondonationList.Sheets[0].Columns[4].Width = 300;
        FpSpreadCondonationList.Sheets[0].Columns[5].Width = 160;
        FpSpreadCondonationList.Sheets[0].Columns[6].Width = 150;
        FpSpreadCondonationList.Sheets[0].Columns[7].Width = 100;
        FpSpreadCondonationList.Sheets[0].Columns[8].Width = 150;
        FpSpreadCondonationList.Sheets[0].Columns[9].Width = 150;
        FpSpreadCondonationList.Sheets[0].Columns[10].Width = 150;
        FpSpreadCondonationList.Sheets[0].Columns[11].Width = 100;
        FpSpreadCondonationList.Sheets[0].Columns[12].Width = 30;
        FpSpreadCondonationList.Sheets[0].Columns[13].Width = 30;

        FpSpreadCondonationList.Sheets[0].AutoPostBack = false;

        FpSpreadCondonationList.Sheets[0].ColumnHeader.Cells[0, 0].Locked = true;
        FpSpreadCondonationList.Sheets[0].ColumnHeader.Cells[0, 1].Locked = true;
        FpSpreadCondonationList.Sheets[0].ColumnHeader.Cells[0, 2].Locked = true;
        FpSpreadCondonationList.Sheets[0].ColumnHeader.Cells[0, 3].Locked = true;
        FpSpreadCondonationList.Sheets[0].ColumnHeader.Cells[0, 4].Locked = true;
        FpSpreadCondonationList.Sheets[0].ColumnHeader.Cells[0, 5].Locked = true;
        FpSpreadCondonationList.Sheets[0].ColumnHeader.Cells[0, 6].Locked = true;
        FpSpreadCondonationList.Sheets[0].ColumnHeader.Cells[0, 7].Locked = true;
        FpSpreadCondonationList.Sheets[0].ColumnHeader.Cells[0, 11].Locked = true;

        FpSpreadCondonationList.Sheets[0].Columns[12].Locked = false;
        FpSpreadCondonationList.Sheets[0].Columns[13].Locked = false;
        FpSpreadCondonationList.Sheets[0].Columns[12].HorizontalAlign = HorizontalAlign.Center;
        FpSpreadCondonationList.Sheets[0].Columns[12].VerticalAlign = VerticalAlign.Middle;

        FarPoint.Web.Spread.CheckBoxCellType cbAll = new FarPoint.Web.Spread.CheckBoxCellType();
        cbAll.AutoPostBack = true;

        FarPoint.Web.Spread.CheckBoxCellType cbEach = new FarPoint.Web.Spread.CheckBoxCellType();
        cbEach.AutoPostBack = false;

        FarPoint.Web.Spread.ButtonCellType btn = new FarPoint.Web.Spread.ButtonCellType();
        btn.CommandName = "btnedit";

        style2.Font.Size = 13;
        style2.Font.Name = "Book Antiqua";
        style2.Font.Bold = true;
        style2.HorizontalAlign = HorizontalAlign.Center;
        style2.ForeColor = System.Drawing.Color.White;
        style2.BackColor = System.Drawing.Color.Teal;

        FpSpreadCondonationList.Sheets[0].ColumnHeader.DefaultStyle = new FarPoint.Web.Spread.StyleInfo(style2);
        FpSpreadCondonationList.Sheets[0].SheetName = " ";
        FpSpreadCondonationList.Sheets[0].Columns.Default.VerticalAlign = VerticalAlign.Middle;
        FpSpreadCondonationList.Sheets[0].Rows.Default.VerticalAlign = VerticalAlign.Middle;
        FpSpreadCondonationList.Sheets[0].DefaultStyle.Font.Name = "Book Antiqua";
        FpSpreadCondonationList.Sheets[0].DefaultStyle.Font.Size = FontUnit.Medium;
        FpSpreadCondonationList.Sheets[0].DefaultStyle.Font.Bold = false;
        FpSpreadCondonationList.Sheets[0].AutoPostBack = true;
        FpSpreadCondonationList.Sheets[0].RowCount = 1;
        FpSpreadCondonationList.Sheets[0].Cells[0, 12].CellType = cbAll;
        string sprdbind = string.Empty;

        if (ddlrollType.SelectedValue == "1")
        {
            string[] regnoarr = txtRollNo.Text.Split(',');
            string regqry = string.Empty;

            if (regnoarr.Length > 0)
            {
                for (int i = 0; i < regnoarr.Length; i++)
                {
                    regqry += "'" + regnoarr[i] + "',";
                }
            }
            else
            {
                lblmsg.Text = "Enter the Valid Student Reg No";
                lblmsg.Visible = true;
                return;
            }
            regqry = regqry + "''";
            sprdbind = "select  e.*,r.Stud_Type,r.Current_Semester,r.Reg_No,r.Batch_Year,r.degree_code,r.Sections,r.Branch_code from Eligibility_list e,Registration r where e.Roll_no=r.Roll_No and e.batch_year=r.Batch_Year and e.degree_code=r.degree_code and e.Semester=r.Current_Semester and r.Reg_No IN (" + regqry + ") and r.college_code='" + collegecode + "'";
        }
        else if (ddlrollType.SelectedValue == "0")

        {
            string[] rollnoarr = txtRollNo.Text.Split(',');
            string rollqry = string.Empty;

            if (rollnoarr.Length > 0)
            {
                for (int i = 0; i < rollnoarr.Length; i++)
                {
                    rollqry += "'" + rollnoarr[i] + "',";
                }
            }
            else
            {
                lblmsg.Text = "Enter the Valid Student Roll No";
                lblmsg.Visible = true;
                return;
            }
            rollqry = rollqry + "''";
             sprdbind = "select  e.*,r.Stud_Type,r.Current_Semester,r.Reg_No,r.Batch_Year,r.degree_code,r.Sections,r.Branch_code from Eligibility_list e,Registration r where e.Roll_no=r.Roll_No and e.batch_year=r.Batch_Year and e.degree_code=r.degree_code and e.Semester=r.Current_Semester and r.Roll_No IN (" + rollqry + ") and r.college_code='" + collegecode + "'";
        }
        try
        {
            DataTable dtsprd = da.selectDataTable(sprdbind);
            if (dtsprd.Rows.Count > 0)
            {
                int rowval = 1;
                for (int i = 0; i < dtsprd.Rows.Count; i++)
                {
                    string rollNo = Convert.ToString(dtsprd.Rows[i]["Roll_No"]);
                    string degreecode = Convert.ToString(dtsprd.Rows[i]["degree_code"]);
                    string regNo = Convert.ToString(dtsprd.Rows[i]["Reg_No"]);
                    string Semester = Convert.ToString(dtsprd.Rows[i]["Semester"]);
                    string BatchYr = Convert.ToString(dtsprd.Rows[i]["batch_year"]);
                    string studname = Convert.ToString(dtsprd.Rows[i]["stud_name"]);
                    string studtype = Convert.ToString(dtsprd.Rows[i]["Stud_Type"]);
                    string FineAmount = Convert.ToString(dtsprd.Rows[i]["fine_amt"]);
                    string appno = Convert.ToString(dtsprd.Rows[i]["app_no"]);
                    string remarks = Convert.ToString(dtsprd.Rows[i]["Remarks"]);
                    string presentDays = Convert.ToString(dtsprd.Rows[i]["presentDays"]).Trim();
                    string absentDays = Convert.ToString(dtsprd.Rows[i]["absentDays"]).Trim();
                    string workingDays = Convert.ToString(dtsprd.Rows[i]["workingDays"]).Trim();
                    string presentHours = Convert.ToString(dtsprd.Rows[i]["presentHours"]).Trim();
                    string absentHours = Convert.ToString(dtsprd.Rows[i]["absentHours"]).Trim();
                    string workingHours = Convert.ToString(dtsprd.Rows[i]["workingHours"]).Trim();
                    string dayWisePresentPercentage = Convert.ToString(dtsprd.Rows[i]["dayWisePresentPercentage"]).Trim();
                    string dayWiseAbsentPercentage = Convert.ToString(dtsprd.Rows[i]["dayWiseAbsentPercentage"]).Trim();
                    string HourWisePresentPercentage = Convert.ToString(dtsprd.Rows[i]["HourWisePresentPercentage"]).Trim();
                    string HourWiseAbsentPercentage = Convert.ToString(dtsprd.Rows[i]["HourWiseAbsentPercentage"]).Trim();
                    string conddate = Convert.ToString(dtsprd.Rows[i]["ChallanDate"]).Trim();
                    string condchallan = Convert.ToString(dtsprd.Rows[i]["ChallanNo"]).Trim();
                    string degreeDetails = da.selectScalarString("select  c.Course_Name+' '+dt.Dept_Name+case when(ltrim(rtrim(isnull(r.Sections,'')))<>'') then ' - '+ltrim(rtrim(isnull(r.Sections,''))) else '' end  as DegreeDetails,dt.dept_acronym + CASE WHEN (LTRIM(RTRIM(ISNULL(r.Sections, ''))) <> '') THEN ' - ' + LTRIM(RTRIM(ISNULL(r.Sections, ''))) ELSE '' END AS ClassDetails from Registration r,Course c,Degree dg,Department dt,collinfo clg where r.college_code=c.college_code and c.college_code=dg.college_code and dg.college_code=dt.college_code and dt.college_code=c.college_code and dt.college_code=clg.college_code and clg.college_code=r.college_code and r.college_code=dg.college_code and r.college_code=dt.college_code and c.Course_Id=dg.Course_Id and dg.Dept_Code=dt.Dept_Code and dg.Degree_Code=r.degree_code and r.Roll_No='" + rollNo + "'");

                    FpSpreadCondonationList.Sheets[0].RowCount++;
                    FpSpreadCondonationList.Sheets[0].Cells[rowval, 0].Text = Convert.ToString(rowval);
                    FpSpreadCondonationList.Sheets[0].Cells[rowval, 1].Note = Convert.ToString(degreecode);
                    FpSpreadCondonationList.Sheets[0].Cells[rowval, 1].Tag = Convert.ToString(appno);
                    FpSpreadCondonationList.Sheets[0].Cells[rowval, 1].Text = Convert.ToString(rollNo);
                    FpSpreadCondonationList.Sheets[0].Cells[rowval, 2].Text = Convert.ToString(regNo);
                    FpSpreadCondonationList.Sheets[0].Cells[rowval, 2].Note = Convert.ToString(Semester);
                    FpSpreadCondonationList.Sheets[0].Cells[rowval, 2].Tag = Convert.ToString(BatchYr);
                    FpSpreadCondonationList.Sheets[0].Cells[rowval, 3].Text = Convert.ToString(studtype);
                    FpSpreadCondonationList.Sheets[0].Cells[rowval, 3].Tag = Convert.ToString(degreeDetails);
                    FpSpreadCondonationList.Sheets[0].Cells[rowval, 4].Text = Convert.ToString(studname);
                    FpSpreadCondonationList.Sheets[0].Cells[rowval, 5].Text = Convert.ToString(dayWisePresentPercentage);
                    FpSpreadCondonationList.Sheets[0].Cells[rowval, 6].Text = Convert.ToString(dayWiseAbsentPercentage);
                    FpSpreadCondonationList.Sheets[0].Cells[rowval, 7].Text = Convert.ToString(FineAmount);
                    FpSpreadCondonationList.Sheets[0].Cells[rowval, 8].Text = Convert.ToString(workingDays);
                    FpSpreadCondonationList.Sheets[0].Cells[rowval, 9].Text = Convert.ToString(presentDays);
                    FpSpreadCondonationList.Sheets[0].Cells[rowval, 10].Text = Convert.ToString(absentDays);
                    FpSpreadCondonationList.Sheets[0].Cells[rowval, 10].Note = Convert.ToString(conddate);
                    FpSpreadCondonationList.Sheets[0].Cells[rowval, 10].Tag = Convert.ToString(condchallan);

                    FpSpreadCondonationList.Sheets[0].Cells[rowval, 11].Text = (remarks.Trim() == "" || string.IsNullOrEmpty(remarks)) ? Convert.ToString("--") : Convert.ToString(remarks);
                    FpSpreadCondonationList.Sheets[0].Cells[rowval, 12].CellType = cbEach;
                    cbEach.AutoPostBack = false;
                    FpSpreadCondonationList.Sheets[0].Cells[rowval, 12].HorizontalAlign = HorizontalAlign.Center;
                    FpSpreadCondonationList.Sheets[0].Cells[rowval, 12].VerticalAlign = VerticalAlign.Middle;

                    btn.Text = "Edit";
                    FpSpreadCondonationList.Sheets[0].Cells[rowval, 13].CellType = btn;
                    FpSpreadCondonationList.Sheets[0].Cells[rowval, 13].HorizontalAlign = HorizontalAlign.Center;
                    FpSpreadCondonationList.Sheets[0].Cells[rowval, 13].VerticalAlign = VerticalAlign.Middle;


                    rowval++;
                }
                divMainContent.Visible = true;
                FpSpreadCondonationList.Visible = true;
                FpSpreadCondonationList.Sheets[0].PageSize = FpSpreadCondonationList.Sheets[0].RowCount;
                FpSpreadCondonationList.Width = 800;
                FpSpreadCondonationList.SaveChanges();
                btnPrint.Visible = FpSpreadCondonationList.Visible;
            }
            else
            {
                lblmsg.Text = "Selected Student is Not Eligible For Condonation";
                lblmsg.Visible = true;
            }
        }
        catch
        {

        }

    }

    protected void FpSpreadCondonationList_BuutonCommand(object sender, FarPoint.Web.Spread.SpreadCommandEventArgs e)
    {
        try
        {
            if (e.CommandName == "btnedit")
            {
                    string Position = e.CommandArgument.ToString().Replace("}", "").Replace("{", "");
                    string[] pos = Position.Split(',');

                    int xpos = 0;
                    int ypos = 0;

                    if (pos.Length > 0)
                    {
                        string[] xVal = (pos.Length > 0) ? pos[0].Split('=') : new string[0];
                        string[] yVal = (pos.Length > 1) ? pos[1].Split('=') : new string[0];
                        if (xVal.Length > 1)
                        {
                            int.TryParse(xVal[1], out xpos);
                            lblxpos.Text = xVal[1];
                        }
                        if (yVal.Length > 1)
                        {
                            int.TryParse(yVal[1], out ypos);
                            lblypos.Text = yVal[1];
                        }
                        int actrow = xpos;

                        FpSpreadCondonationList.Sheets[0].AutoPostBack = false;

                    //actrow = e.SheetView.ActiveRow;
                    if (actrow > -1)
                    {
                        //conddate = txtCondonationDate.Text;
                        //condchallan = txtChallanAmount.Text;

                        //Int32.TryParse(lblxpos.Text, out actrow);
                        string Cond_app_no = FpSpreadCondonationList.Sheets[0].Cells[actrow, 1].Tag.ToString();
                        string Cond_semester = FpSpreadCondonationList.Sheets[0].Cells[actrow, 2].Note;
                        string Cond_batchyr = FpSpreadCondonationList.Sheets[0].Cells[actrow, 2].Tag.ToString();
                        string Cond_degreecode = FpSpreadCondonationList.Sheets[0].Cells[actrow, 1].Note.ToString();
                        string condqry = "select convert(varchar(20), ChallanDate , 103) as ChallanDate,ChallanNo from Eligibility_list where app_no='" + Cond_app_no + "' and Semester='" + Cond_semester + "' and batch_year='" + Cond_batchyr + "' and degree_code='" + Cond_degreecode + "' and is_eligible='2'";
                        DataTable dtCondonationApplied = da.selectDataTable(condqry);
                        if (dtCondonationApplied.Rows.Count > 0)
                        {
                            FpSpreadCondonationList.Sheets[0].Cells[actrow, 13].Note = Convert.ToString(dtCondonationApplied.Rows[0]["ChallanDate"]);
                            FpSpreadCondonationList.Sheets[0].Cells[actrow, 13].Tag = Convert.ToString(dtCondonationApplied.Rows[0]["ChallanNo"]);
                            txtCondonationDate.Text = Convert.ToString(dtCondonationApplied.Rows[0]["ChallanDate"]);
                            txtChallanAmount.Text = Convert.ToString(dtCondonationApplied.Rows[0]["ChallanNo"]);
                            divPopCond.Visible = true;
                        }
                    }
                    FpSpreadCondonationList.Sheets[0].AutoPostBack = true;
                    FpSpreadCondonationList.SaveChanges();
                }

            }
            else
            {
                string Position = e.CommandArgument.ToString().Replace("}", "").Replace("{", "");
                string[] pos = Position.Split(',');

                int xpos = 0;
                int ypos = 0;

                if (pos.Length > 0)
                {
                    string[] xVal = (pos.Length > 0) ? pos[0].Split('=') : new string[0];
                    string[] yVal = (pos.Length > 1) ? pos[1].Split('=') : new string[0];
                    if (xVal.Length > 1)
                    {
                        lblxpos.Text = xVal[1];
                        int.TryParse(xVal[1], out xpos);
                        //actR = xpos;
                    }
                    if (yVal.Length > 1)
                    {
                        lblypos.Text = yVal[1];
                        int.TryParse(yVal[1], out ypos);
                    }
                    int actrow = xpos;



                    //actrow = e.SheetView.ActiveRow;
                    if (actrow > -1 && ypos == 12)
                    {
                        //conddate = txtCondonationDate.Text;
                        //condchallan = txtChallanAmount.Text;
                        //FarPoint.Web.Spread.CheckBoxCellType checkBox = (FarPoint.Web.Spread.CheckBoxCellType) FpSpreadCondonationList.Sheets[0].Cells[actrow, ypos].Value ; 
                        FarPoint.Web.Spread.CheckBoxCellType checkBox = (FarPoint.Web.Spread.CheckBoxCellType)FpSpreadCondonationList.Sheets[0].Cells[actrow, ypos].CellType;

                        if (actrow != 0)
                        {
                            if (Convert.ToString(FpSpreadCondonationList.Sheets[0].Cells[actrow, ypos].Value).Trim() == "1")
                            {
                                FpSpreadCondonationList.Sheets[0].Cells[actrow, ypos].Value = 0;
                            }
                            else
                            {
                                FpSpreadCondonationList.Sheets[0].Cells[actrow, ypos].Value = 1;
                            }

                        }
                        else
                        {
                            int value = 0;
                            if (Convert.ToString(FpSpreadCondonationList.Sheets[0].Cells[actrow, ypos].Value).Trim() == "1")
                            {
                                FpSpreadCondonationList.Sheets[0].Cells[actrow, ypos].Value = 0;
                                value = 0;
                            }
                            else
                            {
                                FpSpreadCondonationList.Sheets[0].Cells[actrow, ypos].Value = 1;
                                value = 1;
                            }
                            for (int r = 1; r < FpSpreadCondonationList.Sheets[0].RowCount; r++)
                            {
                                FarPoint.Web.Spread.CheckBoxCellType checkBox1 = (FarPoint.Web.Spread.CheckBoxCellType)FpSpreadCondonationList.Sheets[0].Cells[r, ypos].CellType;
                                if (checkBox1 != null)
                                    FpSpreadCondonationList.Sheets[0].Cells[r, ypos].Value = value;
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


    protected void btnPrint_OnClick(object sender, EventArgs e)
    {
        lblmsg.Text = string.Empty;
        lblmsg.Visible = false;
        FpSpreadCondonationList.SaveChanges();
        bool status = false;
        Font fondColName = new Font("Times New Roman", 14, FontStyle.Bold);
        Font Font8bold = new Font("Times New Roman", 8, FontStyle.Bold);
        Font Font10bold = new Font("Times New Roman", 10, FontStyle.Bold);
        Font Fontco10 = new Font("Times New Roman", 10, FontStyle.Regular);
        PdfDocument mydoc = new PdfDocument(PdfDocumentFormat.InCentimeters(21.3, 30.3));
        PdfPage mypdfpage;
        PdfTextArea pdfCollege;
        PdfTextArea pdftxt;
        PdfImage pdfLogo;
        PdfTable pdftbl;
        PdfTablePage pdftblPage;
        PdfLine pdfline;
        int PosY = 0;
        int PosX = 0;
        if (ddlCondonationFormat.SelectedValue == "0")
        {
            try
            {
                if (FpSpreadCondonationList.Sheets[0].RowCount > 1)
                {
                    int selected = 0;
                    int val = 0;
                    for (int re = 0; re < FpSpreadCondonationList.Sheets[0].RowCount; re++)
                    {
                        val = 0;
                        int.TryParse(Convert.ToString(FpSpreadCondonationList.Sheets[0].Cells[re, 12].Value).Trim(), out val);
                        FarPoint.Web.Spread.CheckBoxCellType checkBox1 = (FarPoint.Web.Spread.CheckBoxCellType)FpSpreadCondonationList.Sheets[0].Cells[re, 12].CellType;
                        if (val == 1 && checkBox1 != null)
                        {
                            selected++;
                        }
                    }
                    if (selected == 0)
                    {
                        lblmsg.Visible = true;
                        lblmsg.Text = "Please Select Atleast One Student And Then Proceed";
                        return;
                    }
                    else
                    {
                        string strquery = "select collname+' ('+category+')' as collegeName,university,affliatedby,acr,address3,pincode,district,district+' - '+pincode  as districtpin from collinfo where college_code='" + Session["collegecode"].ToString() + "'";
                        DataSet dsCollegeDetail = d2.select_method_wo_parameter(strquery, "Text");
                        string Collegename = string.Empty;
                        string aff = string.Empty;
                        string collacr = string.Empty;
                        string dispin = string.Empty;
                        string clgaddress = string.Empty;
                        string univ = string.Empty;
                        string pincode = string.Empty;
                        if (dsCollegeDetail.Tables.Count > 0 && dsCollegeDetail.Tables[0].Rows.Count > 0)
                        {
                            Collegename = Convert.ToString(dsCollegeDetail.Tables[0].Rows[0]["collegeName"]).Trim();
                            aff = Convert.ToString(dsCollegeDetail.Tables[0].Rows[0]["affliatedby"]).Trim();
                            univ = Convert.ToString(dsCollegeDetail.Tables[0].Rows[0]["university"]).Trim();
                            string[] strpa = aff.Split(',');
                            aff = "( " + univ + " " + strpa[0] + " )";
                            collacr = Convert.ToString(dsCollegeDetail.Tables[0].Rows[0]["acr"]).Trim();
                            pincode = Convert.ToString(dsCollegeDetail.Tables[0].Rows[0]["pincode"]).Trim();
                            pincode = pincode.Substring(pincode.Length - 3);
                            int pin = 0;
                            int.TryParse(pincode, out pin);
                            clgaddress = Convert.ToString(dsCollegeDetail.Tables[0].Rows[0]["address3"]).Trim() + " , " + Convert.ToString(dsCollegeDetail.Tables[0].Rows[0]["district"]).Trim() + ((pin != 0) ? (" - " + Convert.ToString(pin).Trim()) : " - " + pincode);
                            //clgaddress = Convert.ToString(dsCollegeDetail.Tables[0].Rows[0]["address3"]);
                            dispin = Convert.ToString(dsCollegeDetail.Tables[0].Rows[0]["districtpin"]).Trim();
                        }
                        for (int re = 1; re < FpSpreadCondonationList.Sheets[0].RowCount; re++)
                        {
                            val = 0;
                            int.TryParse(Convert.ToString(FpSpreadCondonationList.Sheets[0].Cells[re, 12].Value).Trim(), out val);
                            FarPoint.Web.Spread.CheckBoxCellType checkBox1 = (FarPoint.Web.Spread.CheckBoxCellType)FpSpreadCondonationList.Sheets[0].Cells[re, 12].CellType;
                            if (val == 1 && checkBox1 != null)
                            {
                                string rollNo = Convert.ToString(FpSpreadCondonationList.Sheets[0].Cells[re, 1].Text).Trim();
                                string regNo = Convert.ToString(FpSpreadCondonationList.Sheets[0].Cells[re, 2].Text).Trim();
                                string studentName = Convert.ToString(FpSpreadCondonationList.Sheets[0].Cells[re, 4].Text).Trim();
                                string degreeDeatils = Convert.ToString(FpSpreadCondonationList.Sheets[0].Cells[re, 3].Tag).Trim();
                                string semester = Convert.ToString(FpSpreadCondonationList.Sheets[0].Cells[re, 2].Note).Trim();
                                status = true;
                                mypdfpage = mydoc.NewPage();
                                PosY = 25;
                                pdfCollege = new PdfTextArea(fondColName, Color.Black, new PdfArea(mydoc, 10, PosY, mydoc.PageWidth - 20, 50), ContentAlignment.MiddleCenter, Collegename);
                                mypdfpage.Add(pdfCollege);
                                PosY += 28;
                                pdfCollege = new PdfTextArea(fondColName, Color.Black, new PdfArea(mydoc, 10, PosY, mydoc.PageWidth - 20, 50), ContentAlignment.MiddleCenter, "APPLICATION FOR CONDONATIOIN OF SHORTAGE OF ATTENDANCE");
                                mypdfpage.Add(pdfCollege);
                                PosY += 60;
                                if (File.Exists(HttpContext.Current.Server.MapPath("~/college/Left_Logo.jpeg")))
                                {
                                    PdfImage LogoImage = mydoc.NewImage(HttpContext.Current.Server.MapPath("~/college/Left_Logo.jpeg"));
                                    mypdfpage.Add(LogoImage, 10, 13, 550);
                                }

                                pdftbl = mydoc.NewTable(Fontco10, 1, 4, 0);
                                pdftbl.VisibleHeaders = false;
                                pdftbl.SetBorders(Color.Black, 1, BorderType.None);
                                pdftbl.SetColumnsWidth(new int[] { 50, 150, 40, 20 });
                                pdftbl.Cell(0, 0).SetContentAlignment(ContentAlignment.MiddleLeft);
                                pdftbl.Cell(0, 0).SetContent("MONTH & YEAR : ");
                                pdftbl.Cell(0, 1).SetContentAlignment(ContentAlignment.MiddleLeft);
                                pdftbl.Cell(0, 1).SetContent("");
                                pdftbl.Cell(0, 2).SetContentAlignment(ContentAlignment.MiddleRight);
                                pdftbl.Cell(0, 2).SetContent("SEMESTER : ");
                                pdftbl.Cell(0, 3).SetContentAlignment(ContentAlignment.MiddleLeft);
                                pdftbl.Cell(0, 3).SetContent(ToRoman(semester));
                                pdftblPage = pdftbl.CreateTablePage(new Gios.Pdf.PdfArea(mydoc, 20, PosY, mydoc.PageWidth - 40, 100));
                                mypdfpage.Add(pdftblPage);
                                double tblHeight = pdftblPage.Area.Height;
                                PosY += int.Parse(Convert.ToString(tblHeight)) + 15;

                                pdftbl = mydoc.NewTable(Fontco10, 1, 4, 0);
                                pdftbl.VisibleHeaders = false;
                                pdftbl.SetBorders(Color.Black, 1, BorderType.None);
                                pdftbl.SetColumnsWidth(new int[] { 50, 260, 99, 140 });
                                pdftbl.Cell(0, 0).SetContentAlignment(ContentAlignment.MiddleLeft);
                                pdftbl.Cell(0, 0).SetContent("NAME : ");
                                pdftbl.Cell(0, 1).SetContentAlignment(ContentAlignment.MiddleLeft);
                                pdftbl.Cell(0, 1).SetContent(studentName);
                                pdftbl.Cell(0, 2).SetContentAlignment(ContentAlignment.MiddleLeft);
                                pdftbl.Cell(0, 2).SetContent("CLASS & GROUP : ");
                                pdftbl.Cell(0, 3).SetContentAlignment(ContentAlignment.MiddleLeft);
                                pdftbl.Cell(0, 3).SetContent(degreeDeatils);
                                pdftblPage = pdftbl.CreateTablePage(new Gios.Pdf.PdfArea(mydoc, 20, PosY, mydoc.PageWidth - 40, 100));
                                mypdfpage.Add(pdftblPage);

                                tblHeight = pdftblPage.Area.Height;
                                PosY += int.Parse(Convert.ToString(tblHeight)) + 15;
                                pdftbl = mydoc.NewTable(Fontco10, 6, 2, 0);
                                pdftbl.VisibleHeaders = false;
                                pdftbl.SetBorders(Color.Black, 1, BorderType.None);
                                pdftbl.SetColumnsWidth(new int[] { 250, 250 });
                                pdftbl.Cell(0, 0).SetContentAlignment(ContentAlignment.MiddleLeft);
                                pdftbl.Cell(0, 0).SetContent("TOTAL NUMBER OF WORKING DAYS\t\t\t\t\t\t\t:\t\t");
                                pdftbl.Cell(0, 1).SetContentAlignment(ContentAlignment.MiddleLeft);
                                pdftbl.Cell(0, 1).SetContent("90 DAYS");
                                pdftbl.Cell(1, 0).SetContentAlignment(ContentAlignment.MiddleLeft);
                                pdftbl.Cell(1, 0).SetContent("MAX.NO OF DAYS OF ABSENCE PERMITTED\t\t\t\t:\t\t");
                                pdftbl.Cell(1, 1).SetContentAlignment(ContentAlignment.MiddleLeft);
                                pdftbl.Cell(1, 1).SetContent("22.5 DAYS");
                                pdftbl.Cell(2, 0).SetContentAlignment(ContentAlignment.MiddleLeft);
                                pdftbl.Cell(2, 0).SetContent("DETAILS OF ATTENDANCE:");
                                foreach (PdfCell pc in pdftbl.CellRange(2, 0, 2, 0).Cells)
                                {
                                    pc.ColSpan = 2;
                                }
                                pdftbl.Cell(3, 0).SetContentAlignment(ContentAlignment.MiddleLeft);
                                pdftbl.Cell(3, 0).SetContent("ABSENT WITH LEAVE\t\t\t\t\t\t\t:\t\t");
                                pdftbl.Cell(3, 1).SetContentAlignment(ContentAlignment.MiddleLeft);
                                pdftbl.Cell(3, 1).SetContent("-------------------- DAYS");
                                pdftbl.Cell(4, 0).SetContentAlignment(ContentAlignment.MiddleLeft);
                                pdftbl.Cell(4, 0).SetContent("ABSENT WITHOUT LEAVE\t\t\t\t\t\t\t:\t\t");
                                pdftbl.Cell(4, 1).SetContentAlignment(ContentAlignment.MiddleLeft);
                                pdftbl.Cell(4, 1).SetContent("-------------------- DAYS");
                                pdftbl.Cell(5, 0).SetContentAlignment(ContentAlignment.MiddleLeft);
                                pdftbl.Cell(5, 0).SetContent("TOTAL ABSENT\t\t\t\t\t\t\t:\t\t");
                                pdftbl.Cell(5, 1).SetContentAlignment(ContentAlignment.MiddleLeft);
                                pdftbl.Cell(5, 1).SetContent("-------------------- DAYS");
                                pdftblPage = pdftbl.CreateTablePage(new Gios.Pdf.PdfArea(mydoc, 20, PosY, mydoc.PageWidth - 40, 200));
                                mypdfpage.Add(pdftblPage);
                                tblHeight = pdftblPage.Area.Height;
                                PosY += int.Parse(Convert.ToString(tblHeight)) + 15;
                                pdftxt = new PdfTextArea(Fontco10, Color.Black, new PdfArea(mydoc, 20, PosY, mydoc.PageWidth - 40, 20), ContentAlignment.TopLeft, "DATE OF APPLICATON :SINGNATURE OF STUDENT ");
                                mypdfpage.Add(pdftxt);
                                PosY += 30;
                                pdfline = new PdfLine(mydoc, new Point(20, PosY), new Point(Convert.ToInt32(mydoc.PageWidth - 40), PosY), Color.Black, 1);
                                mypdfpage.Add(pdfline);
                                PosY += 15;
                                pdftxt = new PdfTextArea(Fontco10, Color.Black, new PdfArea(mydoc, 20, PosY, mydoc.PageWidth - 40, 20), ContentAlignment.TopLeft, "ELIGIBLE FOR CONDONATION ");
                                mypdfpage.Add(pdftxt);
                                PosY += 30;
                                pdftxt = new PdfTextArea(Fontco10, Color.Black, new PdfArea(mydoc, 20, PosY, mydoc.PageWidth - 40, 20), ContentAlignment.TopLeft, "DEAN OF STUDENT AFFAIRS");
                                mypdfpage.Add(pdftxt);
                                PosY += 30;
                                pdfline = new PdfLine(mydoc, new Point(20, PosY), new Point(Convert.ToInt32(mydoc.PageWidth - 40), PosY), Color.Black, 1);
                                mypdfpage.Add(pdfline);
                                PosY += 15;
                                pdftxt = new PdfTextArea(Fontco10, Color.Black, new PdfArea(mydoc, 20, PosY, mydoc.PageWidth - 40, 20), ContentAlignment.TopLeft, "RECOMMENDED FOR CONDONATION ");
                                mypdfpage.Add(pdftxt);
                                PosY += 30;
                                pdftxt = new PdfTextArea(Fontco10, Color.Black, new PdfArea(mydoc, 20, PosY, mydoc.PageWidth - 40, 20), ContentAlignment.TopLeft, "HEAD OF THE DEPARTMENT ");
                                mypdfpage.Add(pdftxt);
                                PosY += 30;
                                pdfline = new PdfLine(mydoc, new Point(20, PosY), new Point(Convert.ToInt32(mydoc.PageWidth - 40), PosY), Color.Black, 1);
                                mypdfpage.Add(pdfline);
                                PosY += 15;
                                pdftxt = new PdfTextArea(Font10bold, Color.Black, new PdfArea(mydoc, 20, PosY, mydoc.PageWidth - 40, 20), ContentAlignment.TopLeft, "CONDONATION GRANTED /NOT GRANTED");
                                mypdfpage.Add(pdftxt);
                                PosY += 30;
                                pdftxt = new PdfTextArea(Font10bold, Color.Black, new PdfArea(mydoc, 20, PosY, mydoc.PageWidth - 40, 20), ContentAlignment.TopLeft, "PRINCIPAL");
                                mypdfpage.Add(pdftxt);
                                PosY += 30;
                                pdfline = new PdfLine(mydoc, new Point(20, PosY), new Point(Convert.ToInt32(mydoc.PageWidth - 40), PosY), Color.Black, 1);
                                mypdfpage.Add(pdfline);
                                PosY += 25;
                                pdftxt = new PdfTextArea(Font10bold, Color.Black, new PdfArea(mydoc, 20, PosY, mydoc.PageWidth - 40, 20), ContentAlignment.MiddleCenter, "(FOR RECORDS OFFICE USE ONLY)");
                                mypdfpage.Add(pdftxt);
                                PosY += 30;

                                pdftbl = mydoc.NewTable(Font10bold, 4, 2, 0);
                                pdftbl.VisibleHeaders = false;
                                pdftbl.SetBorders(Color.Black, 1, BorderType.None);
                                pdftbl.SetColumnsWidth(new int[] { 250, 250 });
                                pdftbl.Cell(0, 0).SetContentAlignment(ContentAlignment.MiddleLeft);
                                pdftbl.Cell(0, 0).SetContent("DETAILS OF FEES PAID\t\t\t\t\t\t\t:\t\t");
                                pdftbl.Cell(0, 1).SetContentAlignment(ContentAlignment.MiddleLeft);
                                pdftbl.Cell(0, 1).SetContent("");
                                pdftbl.Cell(1, 0).SetContentAlignment(ContentAlignment.MiddleLeft);
                                pdftbl.Cell(1, 0).SetContent("RECEIPT NO.\t\t\t\t:\t\t");
                                pdftbl.Cell(1, 1).SetContentAlignment(ContentAlignment.MiddleLeft);
                                pdftbl.Cell(1, 1).SetContent("");
                                pdftbl.Cell(2, 0).SetContentAlignment(ContentAlignment.MiddleLeft);
                                pdftbl.Cell(2, 0).SetContent("AMOUNT PAID\t\t\t\t\t\t\t:\t\t");
                                pdftbl.Cell(2, 1).SetContentAlignment(ContentAlignment.MiddleLeft);
                                pdftbl.Cell(2, 1).SetContent("");
                                pdftbl.Cell(3, 0).SetContentAlignment(ContentAlignment.MiddleLeft);
                                pdftbl.Cell(3, 0).SetContent("DATE\t\t\t\t\t\t\t:\t\t");
                                pdftbl.Cell(3, 1).SetContentAlignment(ContentAlignment.MiddleLeft);
                                pdftbl.Cell(3, 1).SetContent("");
                                pdftblPage = pdftbl.CreateTablePage(new Gios.Pdf.PdfArea(mydoc, 20, PosY, mydoc.PageWidth - 40, 200));
                                mypdfpage.Add(pdftblPage);
                                tblHeight = pdftblPage.Area.Height;
                                PosY += int.Parse(Convert.ToString(tblHeight)) + 15;
                                mypdfpage.SaveToDocument();
                            }
                        }
                    }
                }
                else
                {
                    lblmsg.Visible = true;
                    lblmsg.Text = "No Record(s) Found";
                    return;
                }
                if (status)
                {
                    string appPath = HttpContext.Current.Server.MapPath("~");
                    if (appPath != "")
                    {
                        string szPath = appPath + "/Report/";
                        string szFile = "Condonation_Report" + DateTime.Now.ToString("ddMMyyyy") + DateTime.Now.ToString("HHmmss") + ".pdf";
                        mydoc.SaveToFile(szPath + szFile);
                        Response.ClearHeaders();
                        Response.AddHeader("Content-Disposition", "attachment; filename=" + szFile);
                        Response.ContentType = "application/pdf";
                        Response.WriteFile(szPath + szFile);
                    }
                }
            }
            catch (Exception ex)
            {
            }
        }
        else
        {
            mydoc = new PdfDocument(PdfDocumentFormat.A4_Horizontal);//.InCentimeters(27.94, 21.59)

            PdfRectangle pdfrect;
            PdfArea tete;


            Font fontCollegeHeaderbig = new Font("Times New Roman", 12, FontStyle.Bold);
            Font fontCollegeHeadersmall = new Font("Times New Roman", 12, FontStyle.Regular);
            if (FpSpreadCondonationList.Sheets[0].RowCount > 1)
            {
                int selected = 0;
                int val = 0;
                for (int re = 0; re < FpSpreadCondonationList.Sheets[0].RowCount; re++)
                {
                    val = 0;
                    int.TryParse(Convert.ToString(FpSpreadCondonationList.Sheets[0].Cells[re, 12].Value).Trim(), out val);
                    FarPoint.Web.Spread.CheckBoxCellType checkBox1 = (FarPoint.Web.Spread.CheckBoxCellType)FpSpreadCondonationList.Sheets[0].Cells[re, 12].CellType;
                    if (val == 1 && checkBox1 != null)
                    {
                        selected++;
                    }
                }
                if (selected == 0)
                {
                    lblmsg.Visible = true;
                    lblmsg.Text = "Please Select Atleast One Student And Then Proceed";
                    return;
                }
                else
                {
                    string strquery = "select collname,college_code,category,university,affliatedby,acr,address3,pincode,district,district+' - '+pincode  as districtpin,logo1 from collinfo where college_code='" + Session["collegecode"].ToString() + "'";
                    DataSet dsCollegeDetail = d2.select_method_wo_parameter(strquery, "Text");
                    string Collegename = string.Empty;
                    string aff = string.Empty;
                    string collacr = string.Empty;
                    string dispin = string.Empty;
                    string clgaddress = string.Empty;
                    string univ = string.Empty;
                    string pincode = string.Empty;
                    string catogery = string.Empty;
                    string Accredited = string.Empty;
                    string affiliated = string.Empty;

                    if (dsCollegeDetail.Tables.Count > 0 && dsCollegeDetail.Tables[0].Rows.Count > 0)
                    {
                        Collegename = Convert.ToString(dsCollegeDetail.Tables[0].Rows[0]["collname"]).Trim();
                        catogery = "(" + Convert.ToString(dsCollegeDetail.Tables[0].Rows[0]["category"]).Trim() + ")";
                        aff = Convert.ToString(dsCollegeDetail.Tables[0].Rows[0]["affliatedby"]).Trim();
                        univ = Convert.ToString(dsCollegeDetail.Tables[0].Rows[0]["university"]).Trim();
                        string[] strpa = aff.Split(',');
                        aff = strpa[0];
                        Accredited = strpa[1];
                        affiliated = strpa[2];
                        collacr = Convert.ToString(dsCollegeDetail.Tables[0].Rows[0]["acr"]).Trim();
                        pincode = Convert.ToString(dsCollegeDetail.Tables[0].Rows[0]["pincode"]).Trim();
                        pincode = pincode.Substring(pincode.Length - 3);
                        int pin = 0;
                        int.TryParse(pincode, out pin);
                        clgaddress = Convert.ToString(dsCollegeDetail.Tables[0].Rows[0]["address3"]).Trim() + " , " + Convert.ToString(dsCollegeDetail.Tables[0].Rows[0]["district"]).Trim() + ((pin != 0) ? (" - " + Convert.ToString(pin).Trim()) : " - " + pincode);
                        //clgaddress = Convert.ToString(dsCollegeDetail.Tables[0].Rows[0]["address3"]);
                        dispin = Convert.ToString(dsCollegeDetail.Tables[0].Rows[0]["districtpin"]).Trim();


                    }
                    for (int re = 1; re < FpSpreadCondonationList.Sheets[0].RowCount; re++)
                    {
                        val = 0;
                        int.TryParse(Convert.ToString(FpSpreadCondonationList.Sheets[0].Cells[re, 12].Value).Trim(), out val);
                        FarPoint.Web.Spread.CheckBoxCellType checkBox1 = (FarPoint.Web.Spread.CheckBoxCellType)FpSpreadCondonationList.Sheets[0].Cells[re, 12].CellType;
                        if (val == 1 && checkBox1 != null)
                        {
                            string rollNo = Convert.ToString(FpSpreadCondonationList.Sheets[0].Cells[re, 1].Text).Trim();
                            string degreeCode = Convert.ToString(FpSpreadCondonationList.Sheets[0].Cells[re, 1].Note);
                            string regNo = Convert.ToString(FpSpreadCondonationList.Sheets[0].Cells[re, 2].Text).Trim();
                            string semester = Convert.ToString(FpSpreadCondonationList.Sheets[0].Cells[re, 2].Note).Trim();
                            string batchYear = Convert.ToString(FpSpreadCondonationList.Sheets[0].Cells[re, 2].Tag);
                            string degreeDeatils = Convert.ToString(FpSpreadCondonationList.Sheets[0].Cells[re, 3].Tag).Trim();
                            string studentName = Convert.ToString(FpSpreadCondonationList.Sheets[0].Cells[re, 4].Text).Trim();
                            string presentpercent = Convert.ToString(FpSpreadCondonationList.Sheets[0].Cells[re, 5].Text).Trim() + " %";
                            string absentpercent = Convert.ToString(FpSpreadCondonationList.Sheets[0].Cells[re, 6].Text).Trim();
                            string condamount = Convert.ToString(FpSpreadCondonationList.Sheets[0].Cells[re, 7].Text).Trim();
                            string conducteddays = Convert.ToString(FpSpreadCondonationList.Sheets[0].Cells[re, 8].Text).Trim() + " Days";
                            string daysattended = Convert.ToString(FpSpreadCondonationList.Sheets[0].Cells[re, 9].Text).Trim() + " Days";
                            string absentdays = Convert.ToString(FpSpreadCondonationList.Sheets[0].Cells[re, 10].Text).Trim() + " Days";
                            string conddate = Convert.ToString(FpSpreadCondonationList.Sheets[0].Cells[re, 13].Note).Trim();
                            string condchallan = Convert.ToString(FpSpreadCondonationList.Sheets[0].Cells[re, 13].Tag).Trim();
                            string duration = string.Empty;
                            int max_sem1 = 0;
                            if (!string.IsNullOrEmpty(batchYear) && !string.IsNullOrEmpty(degreeCode))
                            {
                                string max_sem = d2.GetFunctionv("select NDurations from ndegree where batch_year='" + batchYear + "'  and Degree_code='" + degreeCode + "'");
                                if (max_sem == "" || max_sem == null)
                                {
                                    max_sem = d2.GetFunctionv("SELECT Duration FROM Degree where  Degree_Code='" + degreeCode + "'");
                                }
                                int.TryParse(max_sem, out max_sem1);
                            }

                            switch (max_sem1)
                            {
                                case 2:
                                    duration = "1 Year";
                                    break;
                                case 4:
                                    duration = "2 Years";
                                    break;
                                case 6:
                                    duration = "3 Years";
                                    break;
                            }

                            string currentsem = da.selectScalarString("select Current_Semester from Registration where Reg_No='" + regNo + "'");
                            string currentyr = string.Empty;
                            int studentSemester = 0;

                            //int.TryParse(currentsem, out studentSemester);
                            //currentyr = Convert.ToString((studentSemester % 2) + ((studentSemester % 2) == 0) ? 0 : 1);
                            if (!(currentsem == "" || currentsem == null))
                            {
                                if (currentsem.Trim() == "1" || currentsem.Trim() == "2")
                                    currentyr = "1st Year";
                                if (currentsem.Trim() == "3" || currentsem.Trim() == "4")
                                    currentyr = "2nd Year";
                                if (currentsem.Trim() == "5" || currentsem.Trim() == "6")
                                    currentyr = "3rd Year";
                                if (currentsem.Trim() == "7" || currentsem.Trim() == "8")
                                    currentyr = "4th Year";
                                if (currentsem.Trim() == "9" || currentsem.Trim() == "10")
                                    currentyr = "5th Year";
                            }

                            switch (currentsem.Trim())
                            {
                                case "1":
                                    currentsem = currentsem + "st";
                                    break;
                                case "2":
                                    currentsem = currentsem + "nd";
                                    break;
                                case "3":
                                    currentsem = currentsem + "rd";
                                    break;
                                default:
                                    currentsem = currentsem + "th";
                                    break;
                            }

                            mypdfpage = mydoc.NewPage();
                            PosY = 20;

                            pdfCollege = new PdfTextArea(fontCollegeHeaderbig, Color.Black, new PdfArea(mydoc, 15, PosY, mydoc.PageWidth / 2 - 15, 20), ContentAlignment.MiddleCenter, Collegename);
                            mypdfpage.Add(pdfCollege);

                            pdfCollege = new PdfTextArea(fontCollegeHeaderbig, Color.Black, new PdfArea(mydoc, 395, PosY, mydoc.PageWidth / 2 - 15, 20), ContentAlignment.MiddleCenter, Collegename);
                            mypdfpage.Add(pdfCollege);

                            PosY += 15;

                            pdfCollege = new PdfTextArea(fontCollegeHeaderbig, Color.Black, new PdfArea(mydoc, 15, PosY, mydoc.PageWidth / 2 - 15, 20), ContentAlignment.MiddleCenter, catogery);
                            mypdfpage.Add(pdfCollege);
                            pdfCollege = new PdfTextArea(fontCollegeHeaderbig, Color.Black, new PdfArea(mydoc, 395, PosY, mydoc.PageWidth / 2 - 15, 20), ContentAlignment.MiddleCenter, catogery);
                            mypdfpage.Add(pdfCollege);
                            PosY += 15;

                            pdfCollege = new PdfTextArea(fontCollegeHeaderbig, Color.Black, new PdfArea(mydoc, 15, PosY, mydoc.PageWidth / 2 - 15, 20), ContentAlignment.MiddleCenter, aff);
                            mypdfpage.Add(pdfCollege);
                            pdfCollege = new PdfTextArea(fontCollegeHeaderbig, Color.Black, new PdfArea(mydoc, 395, PosY, mydoc.PageWidth / 2 - 15, 20), ContentAlignment.MiddleCenter, aff);
                            mypdfpage.Add(pdfCollege);
                            PosY += 15;

                            pdfCollege = new PdfTextArea(fontCollegeHeadersmall, Color.Black, new PdfArea(mydoc, 25, PosY, mydoc.PageWidth / 2 - 15, 20), ContentAlignment.MiddleCenter, Convert.ToString(Accredited.Split('\\').Last()));
                            mypdfpage.Add(pdfCollege);
                            pdfCollege = new PdfTextArea(fontCollegeHeadersmall, Color.Black, new PdfArea(mydoc, 410, PosY, mydoc.PageWidth / 2 - 15, 20), ContentAlignment.MiddleCenter, Convert.ToString(Accredited.Split('\\').Last()));
                            mypdfpage.Add(pdfCollege);
                            PosY += 15;

                            pdfCollege = new PdfTextArea(fontCollegeHeadersmall, Color.Black, new PdfArea(mydoc, 15, PosY, mydoc.PageWidth / 2 - 15, 20), ContentAlignment.MiddleCenter, Convert.ToString(affiliated.Split('\\').Last()));
                            mypdfpage.Add(pdfCollege);
                            pdfCollege = new PdfTextArea(fontCollegeHeadersmall, Color.Black, new PdfArea(mydoc, 395, PosY, mydoc.PageWidth / 2 - 15, 20), ContentAlignment.MiddleCenter, Convert.ToString(affiliated.Split('\\').Last()));
                            mypdfpage.Add(pdfCollege);
                            PosY += 15;

                            pdfCollege = new PdfTextArea(fontCollegeHeaderbig, Color.Black, new PdfArea(mydoc, 15, PosY, mydoc.PageWidth / 2 - 15, 20), ContentAlignment.MiddleCenter, dispin);
                            mypdfpage.Add(pdfCollege);
                            pdfCollege = new PdfTextArea(fontCollegeHeaderbig, Color.Black, new PdfArea(mydoc, 395, PosY, mydoc.PageWidth / 2 - 15, 20), ContentAlignment.MiddleCenter, dispin);
                            mypdfpage.Add(pdfCollege);
                            PosY += 30;
                            MemoryStream memoryStream = new MemoryStream();
                            string studentCollegeCode = Convert.ToString(dsCollegeDetail.Tables[0].Rows[0]["college_code"]).Trim();
                            if (!File.Exists(HttpContext.Current.Server.MapPath("~/college/Left_Logo" + studentCollegeCode + ".jpeg")))
                            {
                                byte[] file = (byte[])dsCollegeDetail.Tables[0].Rows[0]["logo1"];
                                memoryStream.Write(file, 0, file.Length);
                                if (file.Length > 0)
                                {
                                    System.Drawing.Image imgx = System.Drawing.Image.FromStream(memoryStream, true, true);
                                    System.Drawing.Image thumb = imgx.GetThumbnailImage(350, 350, null, IntPtr.Zero);
                                    if (!File.Exists(HttpContext.Current.Server.MapPath("~/college/Left_Logo" + studentCollegeCode.ToString() + ".jpeg")))
                                    {
                                        thumb.Save(HttpContext.Current.Server.MapPath("~/college/Left_Logo" + studentCollegeCode.ToString() + ".jpeg"), System.Drawing.Imaging.ImageFormat.Jpeg);
                                    }
                                }
                            }
                            if (File.Exists(HttpContext.Current.Server.MapPath("~/college/Left_Logo" + studentCollegeCode.ToString() + ".jpeg")))
                            {
                                PdfImage LogoImage = mydoc.NewImage(HttpContext.Current.Server.MapPath("~/college/Left_Logo" + studentCollegeCode.ToString() + ".jpeg"));
                                mypdfpage.Add(LogoImage, 20, 13, 300);
                                mypdfpage.Add(LogoImage, 400, 13, 300);
                            }


                            pdfline = new PdfLine(mydoc, new Point(20, PosY), new Point(Convert.ToInt32(mydoc.PageWidth), PosY), Color.Black, 1);
                            mypdfpage.Add(pdfline);
                            PosY += 10;

                            pdftxt = new PdfTextArea(new Font("Times New Roman", 10, FontStyle.Bold), Color.Black, new PdfArea(mydoc, 20, PosY, (mydoc.PageWidth) / 2, 15), ContentAlignment.MiddleCenter, "ATTENDANCE CERTIFICATE");
                            mypdfpage.Add(pdftxt);

                            pdftxt = new PdfTextArea(new Font("Times New Roman", 10, FontStyle.Bold), Color.Black, new PdfArea(mydoc, 400, PosY, (mydoc.PageWidth) / 2, 15), ContentAlignment.MiddleCenter, "APPLICATION FOR GRANT OF CONDONATION");
                            mypdfpage.Add(pdftxt);
                            PosY += 02;
                            pdftxt = new PdfTextArea(new Font("Times New Roman", 8, FontStyle.Bold), Color.Black, new PdfArea(mydoc, 20, PosY, (mydoc.PageWidth) / 2, 10), ContentAlignment.MiddleCenter, "___________________________________");
                            mypdfpage.Add(pdftxt);

                            pdftxt = new PdfTextArea(new Font("Times New Roman", 10, FontStyle.Bold), Color.Black, new PdfArea(mydoc, 400, PosY, (mydoc.PageWidth) / 2, 10), ContentAlignment.MiddleCenter, "______________________________________________");
                            mypdfpage.Add(pdftxt);

                            PosY += 13;

                            pdftxt = new PdfTextArea(Fontco10, Color.Black, new PdfArea(mydoc, 20, PosY, (mydoc.PageWidth) / 2, 20), ContentAlignment.TopLeft, "(Should be sent to the Controller of Examinatons, Jamal Mohamed College atleast \n by 10  days prior to the date of commencement of the Examinations )");
                            mypdfpage.Add(pdftxt);

                            PosY += 25;

                            pdftbl = mydoc.NewTable(Fontco10, 10, 3, 1);
                            pdftbl.VisibleHeaders = false;
                            pdftbl.SetBorders(Color.Black, 1, BorderType.None);
                            pdftbl.SetColumnsWidth(new int[] { 30, 30 });
                            pdftbl.SetCellPadding(3);

                            pdftbl.Cell(0, 0).SetContentAlignment(ContentAlignment.MiddleLeft);
                            pdftbl.Cell(0, 0).SetFont(Font10bold);
                            pdftbl.Cell(0, 0).SetContent("Name of the Candidate: ");
                            //pdftbl.Cell(0, 1).SetContent(":");
                            pdftbl.Cell(0, 1).SetContentAlignment(ContentAlignment.MiddleLeft);
                            pdftbl.Cell(0, 1).SetContent(studentName);

                            pdftbl.Cell(1, 0).SetContentAlignment(ContentAlignment.MiddleLeft);
                            pdftbl.Cell(1, 0).SetFont(Font10bold);
                            pdftbl.Cell(1, 0).SetContent("Register No: ");
                            // pdftbl.Cell(1, 1).SetContent(":");
                            pdftbl.Cell(1, 1).SetContentAlignment(ContentAlignment.MiddleLeft);
                            pdftbl.Cell(1, 1).SetContent(regNo);

                            pdftbl.Cell(2, 0).SetContentAlignment(ContentAlignment.MiddleLeft);
                            pdftbl.Cell(2, 0).SetFont(Font10bold);
                            pdftbl.Cell(2, 0).SetContent("Class & Main: ");  //UG - 2017 - B.Sc. - PHYSICS - A
                            //  pdftbl.Cell(2, 1).SetContent(":");
                            pdftbl.Cell(2, 1).SetContentAlignment(ContentAlignment.MiddleLeft);
                            pdftbl.Cell(2, 1).SetContent(Convert.ToString(degreeDeatils));

                            pdftbl.Cell(3, 0).SetContentAlignment(ContentAlignment.MiddleLeft);
                            pdftbl.Cell(3, 0).SetFont(Font10bold);
                            pdftbl.Cell(3, 0).SetContent("Year/Semester: ");
                            // pdftbl.Cell(3, 1).SetContent(":");
                            pdftbl.Cell(3, 1).SetContentAlignment(ContentAlignment.MiddleLeft);
                            pdftbl.Cell(3, 1).SetContent(currentyr + " / " + currentsem + " Sem");

                            pdftbl.Cell(4, 0).SetContentAlignment(ContentAlignment.MiddleLeft);
                            pdftbl.Cell(4, 0).SetFont(Font10bold);
                            pdftbl.Cell(4, 0).SetContent("Period Of The Course: ");
                            // pdftbl.Cell(4, 1).SetContent(":");
                            pdftbl.Cell(4, 1).SetContentAlignment(ContentAlignment.MiddleLeft);
                            pdftbl.Cell(4, 1).SetContent(duration);

                            pdftbl.Cell(5, 0).SetContentAlignment(ContentAlignment.MiddleLeft);
                            pdftbl.Cell(5, 0).SetFont(Font10bold);
                            pdftbl.Cell(5, 0).SetContent("1. Total No. of working days: ");
                            //pdftbl.Cell(5, 1).SetContent(":");
                            pdftbl.Cell(5, 1).SetContentAlignment(ContentAlignment.MiddleLeft);
                            pdftbl.Cell(5, 1).SetContent(conducteddays);

                            pdftbl.Cell(6, 0).SetContentAlignment(ContentAlignment.MiddleLeft);
                            pdftbl.Cell(6, 0).SetFont(Font10bold);
                            pdftbl.Cell(6, 0).SetContent("2. No of days attend: ");
                            //  pdftbl.Cell(6, 1).SetContent(":");
                            pdftbl.Cell(6, 1).SetContentAlignment(ContentAlignment.MiddleLeft);
                            pdftbl.Cell(6, 1).SetContent(daysattended);

                            pdftbl.Cell(7, 0).SetContentAlignment(ContentAlignment.MiddleLeft);
                            pdftbl.Cell(7, 0).SetFont(Font10bold);
                            pdftbl.Cell(7, 0).SetContent("3.Present Percentage: ");
                            // pdftbl.Cell(7, 1).SetContent(":");
                            pdftbl.Cell(7, 1).SetContentAlignment(ContentAlignment.MiddleLeft);
                            pdftbl.Cell(7, 1).SetContent(presentpercent);

                            pdftbl.Cell(8, 0).SetContentAlignment(ContentAlignment.MiddleLeft);
                            pdftbl.Cell(8, 0).SetFont(Font10bold);
                            pdftbl.Cell(8, 0).SetContent("Signature of Candidate:");
                            //pdftbl.Cell(8, 1).SetContent(":");
                            pdftbl.Cell(8, 1).SetContentAlignment(ContentAlignment.MiddleLeft);
                            pdftbl.Cell(8, 1).SetContent("");

                            pdftblPage = pdftbl.CreateTablePage(new Gios.Pdf.PdfArea(mydoc, 20, PosY, (mydoc.PageWidth) / 2, 300));
                            mypdfpage.Add(pdftblPage);
                            // tblHeight = pdftblPage.Area.Height;
                            //PosY += int.Parse(Convert.ToString(tblHeight)) + 15;

                            PosY += 170;

                            tete = new PdfArea(mydoc, 30, PosY, 7, 7);
                            pdfrect = new PdfRectangle(mydoc, tete, Color.Black);
                            mypdfpage.Add(pdfrect);

                            pdftxt = new PdfTextArea(Font10bold, Color.Black, new PdfArea(mydoc, 45, PosY, (mydoc.PageWidth) / 2, 20), ContentAlignment.TopLeft, " Certified that the candidate has earned the required percentage of attendance.");
                            mypdfpage.Add(pdftxt);

                            PosY += 15;

                            tete = new PdfArea(mydoc, 30, PosY, 7, 7);
                            pdfrect = new PdfRectangle(mydoc, tete, Color.Black);
                            mypdfpage.Add(pdfrect);
                            pdftxt = new PdfTextArea(Font10bold, Color.Black, new PdfArea(mydoc, 45, PosY, (mydoc.PageWidth) / 2, 20), ContentAlignment.TopLeft, " The candidate requires Condonation of attendance.");
                            mypdfpage.Add(pdftxt);

                            PosY += 15;

                            tete = new PdfArea(mydoc, 30, PosY, 7, 7);
                            pdfrect = new PdfRectangle(mydoc, tete, Color.Black);
                            mypdfpage.Add(pdfrect);
                            pdftxt = new PdfTextArea(Font10bold, Color.Black, new PdfArea(mydoc, 45, PosY, (mydoc.PageWidth) / 2, 20), ContentAlignment.TopLeft, " The Candidate has not earned the required attenedance and \n hence He/She is not permitted to sit for the examination.");
                            mypdfpage.Add(pdftxt);

                            PosY += 30;

                            pdftbl = mydoc.NewTable(Font10bold, 2, 3, 1);
                            pdftbl.VisibleHeaders = false;
                            pdftbl.SetBorders(Color.Black, 1, BorderType.None);
                            pdftbl.SetColumnsWidth(new int[] { 50, 50 });
                            pdftbl.SetCellPadding(1);

                            pdftbl.Cell(0, 0).SetContentAlignment(ContentAlignment.MiddleLeft);
                            pdftbl.Cell(0, 0).SetContent("Whether the attendance is regular: ");
                            // pdftbl.Cell(0, 1).SetContent(":");
                            pdftbl.Cell(0, 1).SetContentAlignment(ContentAlignment.MiddleLeft);
                            pdftbl.Cell(0, 1).SetFont(Fontco10);
                            pdftbl.Cell(0, 1).SetContent("Satisfaction");

                            pdftbl.Cell(1, 0).SetContentAlignment(ContentAlignment.MiddleLeft);
                            pdftbl.Cell(1, 0).SetContent("Conduct & Character: ");
                            // pdftbl.Cell(1, 1).SetContent(":");
                            pdftbl.Cell(1, 1).SetContentAlignment(ContentAlignment.MiddleLeft);
                            pdftbl.Cell(1, 1).SetFont(Fontco10);
                            pdftbl.Cell(1, 1).SetContent("Good");

                            pdftblPage = pdftbl.CreateTablePage(new Gios.Pdf.PdfArea(mydoc, 20, PosY, (mydoc.PageWidth) / 2, 200));
                            mypdfpage.Add(pdftblPage);
                            PosY += 40;


                            pdftxt = new PdfTextArea(Font10bold, Color.Black, new PdfArea(mydoc, 20, PosY, (mydoc.PageWidth) / 2, 20), ContentAlignment.TopLeft, "NOTE:Strike off whichever is not applicable.");
                            mypdfpage.Add(pdftxt);

                            PosY += 30;


                            pdftxt = new PdfTextArea(Font10bold, Color.Black, new PdfArea(mydoc, 20, PosY, (mydoc.PageWidth) / 2, 20), ContentAlignment.BottomLeft, "Condonation:");
                            mypdfpage.Add(pdftxt);

                            pdftxt = new PdfTextArea(Fontco10, Color.Black, new PdfArea(mydoc, 100, PosY, 200, 20), ContentAlignment.BottomLeft, "Sanctioned /NonSanctioned ");
                            mypdfpage.Add(pdftxt);
                            pdftxt = new PdfTextArea(new Font("Times New Roman", 10, FontStyle.Bold), Color.Black, new PdfArea(mydoc, 100, PosY, 200, 20), ContentAlignment.BottomLeft, "                  ---------------------");
                            mypdfpage.Add(pdftxt);
                            PosY += 35;

                            pdftxt = new PdfTextArea(Font10bold, Color.Black, new PdfArea(mydoc, 20, PosY, (mydoc.PageWidth) / 2, 20), ContentAlignment.BottomLeft, "Date:" + conddate);
                            mypdfpage.Add(pdftxt);


                            pdftxt = new PdfTextArea(new Font("Times New Roman", 10, FontStyle.Bold), Color.Black, new PdfArea(mydoc, 100, PosY, (mydoc.PageWidth) / 2, 20), ContentAlignment.BottomLeft, "Challan :" + condchallan);
                            mypdfpage.Add(pdftxt);
                            PosY += 50;

                            pdftbl = mydoc.NewTable(Font10bold, 2, 2, 1);
                            pdftbl.VisibleHeaders = false;
                            pdftbl.SetBorders(Color.Black, 1, BorderType.None);
                            pdftbl.SetColumnsWidth(new int[] { 50, 50 });
                            pdftbl.SetCellPadding(1);

                            pdftbl.Cell(0, 0).SetContentAlignment(ContentAlignment.MiddleLeft);
                            pdftbl.Cell(0, 0).SetContent("Signature of the Registrar");
                            pdftbl.Cell(0, 1).SetContentAlignment(ContentAlignment.MiddleRight);
                            pdftbl.Cell(0, 1).SetContent("Signature of the Principal");

                            pdftblPage = pdftbl.CreateTablePage(new Gios.Pdf.PdfArea(mydoc, 20, PosY, ((mydoc.PageWidth) / 2) - 60, 20));
                            mypdfpage.Add(pdftblPage);

                            PosY = 150;

                            pdftxt = new PdfTextArea(Fontco10, Color.Black, new PdfArea(mydoc, 400, PosY, (mydoc.PageWidth) / 2, 20), ContentAlignment.TopLeft, "(Only those candidates who fall short of attendance from 26% to 40% of the working days\n need to use this form)");
                            mypdfpage.Add(pdftxt);
                            PosY += 30;
                            pdftbl = mydoc.NewTable(Font10bold, 10, 4, 2);
                            pdftbl.VisibleHeaders = false;
                            pdftbl.SetBorders(Color.Black, 1, BorderType.None);
                            pdftbl.SetColumnsWidth(new int[] { 120, 120, 80, 90 });
                            //pdftbl.SetRowHeight(50);

                            pdftbl.Cell(0, 0).SetContentAlignment(ContentAlignment.MiddleLeft);
                            pdftbl.Cell(0, 0).SetContent("Name of the Candidate: ");
                            //pdftbl.Cell(0, 1).SetContent(":");
                            pdftbl.Cell(0, 1).SetContentAlignment(ContentAlignment.MiddleLeft);
                            pdftbl.Cell(0, 1).SetFont(Fontco10);
                            pdftbl.Cell(0, 1).SetContent(studentName);

                            pdftbl.Cell(0, 2).SetContentAlignment(ContentAlignment.MiddleLeft);
                            pdftbl.Cell(0, 2).SetContent("Register No: ");
                            // pdftbl.Cell(0, 4).SetContent(":");
                            pdftbl.Cell(0, 3).SetContentAlignment(ContentAlignment.MiddleLeft);
                            pdftbl.Cell(0, 3).SetFont(Fontco10);
                            pdftbl.Cell(0, 3).SetContent(regNo);
                            pdftbl.Cell(1, 0).SetContentAlignment(ContentAlignment.MiddleLeft);
                            pdftbl.Cell(1, 0).SetContent("Course and Subject: ");
                            // pdftbl.Cell(1, 1).SetContent(":");
                            pdftbl.Cell(1, 1).SetFont(Fontco10);
                            pdftbl.Cell(1, 1).SetContentAlignment(ContentAlignment.MiddleCenter);
                            pdftbl.Cell(1, 1).SetContent(Convert.ToString(degreeDeatils));

                            pdftbl.Cell(1, 2).SetContentAlignment(ContentAlignment.MiddleLeft);
                            pdftbl.Cell(1, 2).SetContent("Year/Semester: ");
                            pdftbl.Cell(1, 3).SetFont(Fontco10);
                            // pdftbl.Cell(1, 4).SetContent(":");
                            pdftbl.Cell(1, 3).SetContentAlignment(ContentAlignment.MiddleCenter);
                            pdftbl.Cell(1, 3).SetContent(currentyr + " / " + currentsem + " Sem");


                            pdftblPage = pdftbl.CreateTablePage(new Gios.Pdf.PdfArea(mydoc, 400, PosY, (mydoc.PageWidth / 2), 200));
                            mypdfpage.Add(pdftblPage);

                            PosY += 40;

                            pdftbl = mydoc.NewTable(Font10bold, 5, 2, 1);
                            pdftbl.VisibleHeaders = false;
                            pdftbl.SetBorders(Color.Black, 1, BorderType.None);
                            pdftbl.SetColumnsWidth(new int[] { 150, 150 });
                            pdftbl.SetCellPadding(2);

                            pdftbl.Cell(0, 0).SetContentAlignment(ContentAlignment.MiddleLeft);
                            pdftbl.Cell(0, 0).SetContent("Total No. of days/hours the college Worked: ");
                            //pdftbl.Cell(0, 1).SetContentAlignment(ContentAlignment.MiddleLeft);
                            // pdftbl.Cell(0, 1).SetContent(":");
                            pdftbl.Cell(0, 1).SetContentAlignment(ContentAlignment.MiddleLeft);
                            pdftbl.Cell(0, 1).SetFont(Fontco10);
                            pdftbl.Cell(0, 1).SetContent(conducteddays);

                            pdftbl.Cell(1, 0).SetContentAlignment(ContentAlignment.MiddleLeft);
                            pdftbl.Cell(1, 0).SetContent("No. of days/hours the Candidate attended: ");
                            //pdftbl.Cell(1, 1).SetContentAlignment(ContentAlignment.MiddleLeft);
                            // pdftbl.Cell(1, 1).SetContent(":");
                            pdftbl.Cell(1, 1).SetContentAlignment(ContentAlignment.MiddleLeft);
                            pdftbl.Cell(1, 1).SetFont(Fontco10);
                            pdftbl.Cell(1, 1).SetContent(daysattended);

                            pdftbl.Cell(2, 0).SetContentAlignment(ContentAlignment.MiddleLeft);
                            pdftbl.Cell(2, 0).SetContent("Actual shortage of attendance: ");
                            // pdftbl.Cell(2, 1).SetContentAlignment(ContentAlignment.MiddleLeft);
                            // pdftbl.Cell(2, 1).SetContent(":");
                            pdftbl.Cell(2, 1).SetContentAlignment(ContentAlignment.MiddleLeft);
                            pdftbl.Cell(2, 1).SetFont(Fontco10);
                            pdftbl.Cell(2, 1).SetContent(absentpercent + " %");
                            if (currentyr.Trim() == "3rd Year")
                            {
                                pdftbl.Cell(3, 0).SetContentAlignment(ContentAlignment.MiddleLeft);
                                pdftbl.Cell(3, 0).SetContent("Category: \n\n(i) 26% to 30%     \n(ii) 31% to 40%");
                            }
                            else
                            {
                                pdftbl.Cell(3, 0).SetContentAlignment(ContentAlignment.MiddleLeft);
                                pdftbl.Cell(3, 0).SetContent("Category: \n\n(i) 26% to 35%     \n(ii) 36% to 50%");
                            }
                            pdftbl.Cell(3, 1).SetContentAlignment(ContentAlignment.MiddleLeft);
                            pdftbl.Cell(3, 1).SetContent("\n\nCondonation Fee : 600 \nCondonation Fee : (600+50)");

                            pdftbl.Cell(4, 0).SetContentAlignment(ContentAlignment.MiddleLeft);
                            pdftbl.Cell(4, 0).SetContent("Reason for shortage attendance: \n(Relevant authentic evidence should be enclosed)");
                            pdftbl.Cell(4, 1).SetContentAlignment(ContentAlignment.MiddleLeft);
                            pdftbl.Cell(4, 1).SetContent("");

                            pdftblPage = pdftbl.CreateTablePage(new Gios.Pdf.PdfArea(mydoc, 400, PosY, (mydoc.PageWidth) / 2, 200));
                            mypdfpage.Add(pdftblPage);
                            PosY += 130;

                            pdftxt = new PdfTextArea(Font10bold, Color.Black, new PdfArea(mydoc, 400, PosY, (mydoc.PageWidth) / 2, 10), ContentAlignment.TopLeft, "Condonation Fee :" + condamount);
                            mypdfpage.Add(pdftxt);

                            PosY += 20;

                            pdftxt = new PdfTextArea(Font10bold, Color.Black, new PdfArea(mydoc, 700, PosY, (mydoc.PageWidth) / 2, 10), ContentAlignment.TopLeft, "Signature of the Candidate ");
                            mypdfpage.Add(pdftxt);

                            PosY += 10;

                            pdftxt = new PdfTextArea(Font10bold, Color.Black, new PdfArea(mydoc, 400, PosY, (mydoc.PageWidth) / 2, 10), ContentAlignment.TopLeft, "Challan :" + condchallan);
                            mypdfpage.Add(pdftxt);

                            PosY += 15;

                            pdftbl = mydoc.NewTable(Font10bold, 2, 2, 1);
                            pdftbl.VisibleHeaders = false;
                            pdftbl.SetBorders(Color.Black, 1, BorderType.None);
                            pdftbl.SetColumnsWidth(new int[] { 200, 80 });
                            pdftbl.SetCellPadding(1);

                            pdftbl.Cell(0, 0).SetContentAlignment(ContentAlignment.TopLeft);
                            pdftbl.Cell(0, 0).SetContent("Sanction by the principal in the case of category \n (i) SPECIFIC RECOMMENDATION of the Registration of \nattendance in the case of category \n (ii) (The Principal should certify to the genuinity  of the \n reason for absence)");
                            pdftbl.Cell(0, 1).SetContentAlignment(ContentAlignment.MiddleCenter);
                            pdftbl.Cell(0, 1).SetContent("Recommended with Medical Certificate");


                            pdftblPage = pdftbl.CreateTablePage(new Gios.Pdf.PdfArea(mydoc, 400, PosY, (mydoc.PageWidth) / 2, 200));
                            mypdfpage.Add(pdftblPage);

                            PosY += 80;

                            pdftxt = new PdfTextArea(Font10bold, Color.Black, new PdfArea(mydoc, 400, PosY, (mydoc.PageWidth) / 2, 10), ContentAlignment.TopLeft, "Date:" + conddate);
                            mypdfpage.Add(pdftxt);

                            pdftxt = new PdfTextArea(Font10bold, Color.Black, new PdfArea(mydoc, 700, PosY, (mydoc.PageWidth) / 2, 10), ContentAlignment.TopLeft, "Signature of the Principal");
                            mypdfpage.Add(pdftxt);

                            PosY += 10;

                            pdfline = new PdfLine(mydoc, new Point(400, PosY), new Point(800, PosY), Color.Black, 1);
                            mypdfpage.Add(pdfline);
                            PosY += 5;

                            pdftxt = new PdfTextArea(Font10bold, Color.Black, new PdfArea(mydoc, 400, PosY, (mydoc.PageWidth) / 2, 15), ContentAlignment.MiddleCenter, "FOR THE CONTROLLER OF EXAMINATIONS JAMAL MOHAMED COLLEGE\nOFFICE USE ONLY");
                            mypdfpage.Add(pdftxt);

                            PosY += 30;

                            pdftxt = new PdfTextArea(Font10bold, Color.Black, new PdfArea(mydoc, 400, PosY, (mydoc.PageWidth) / 2, 10), ContentAlignment.MiddleLeft, "Remarks of the section");
                            mypdfpage.Add(pdftxt);

                            PosY += 40;

                            pdftbl = mydoc.NewTable(Font10bold, 2, 2, 1);
                            pdftbl.VisibleHeaders = false;
                            pdftbl.SetBorders(Color.Black, 1, BorderType.None);
                            pdftbl.SetColumnsWidth(new int[] { 50, 50 });
                            pdftbl.SetCellPadding(1);

                            pdftbl.Cell(0, 0).SetContentAlignment(ContentAlignment.MiddleLeft);
                            pdftbl.Cell(0, 0).SetContent("Section Head");
                            pdftbl.Cell(0, 1).SetContentAlignment(ContentAlignment.MiddleRight);
                            pdftbl.Cell(0, 1).SetContent("Order of the Controller \n of Examinations");


                            pdftblPage = pdftbl.CreateTablePage(new Gios.Pdf.PdfArea(mydoc, 400, PosY, (mydoc.PageWidth) / 2, 50));
                            mypdfpage.Add(pdftblPage);


                            mypdfpage.SaveToDocument();
                            status = true;
                        }
                        if (status)
                        {
                            string appPath = HttpContext.Current.Server.MapPath("~");
                            if (appPath != "")
                            {
                                string szPath = appPath + "/Report/";
                                string szFile = "Cndntn_Report" + DateTime.Now.ToString("ddMMyyyy") + DateTime.Now.ToString("HHmmss") + ".pdf";
                                mydoc.SaveToFile(szPath + szFile);
                                Response.ClearHeaders();
                                Response.AddHeader("Content-Disposition", "attachment; filename=" + szFile);
                                Response.ContentType = "application/pdf";
                                Response.WriteFile(szPath + szFile);
                            }
                        }
                    }
                }
            }

        }


    }

    public string ToRoman(string part)
    {
        string roman = string.Empty;
        try
        {
            switch (part)
            {
                case "1":
                    roman = "I";
                    break;
                case "2":
                    roman = "II";
                    break;
                case "3":
                    roman = "III";
                    break;
                case "4":
                    roman = "IV";
                    break;
                case "5":
                    roman = "V";
                    break;
                case "6":
                    roman = "VI";
                    break;
                case "7":
                    roman = "VII";
                    break;
                case "8":
                    roman = "VIII";
                    break;
                case "9":
                    roman = "IX";
                    break;
                case "10":
                    roman = "X";
                    break;
                case "11":
                    roman = "XI";
                    break;
                case "12":
                    roman = "XII";
                    break;
            }
        }
        catch (Exception ex)
        {
        }
        return roman;
    }

    protected void btnPopAlertClose_Click(object sender, EventArgs e)
    {
        try
        {
            DateTime date = new DateTime();
            FpSpreadCondonationList.SaveChanges();
            conddate = txtCondonationDate.Text;
            condchallan = txtChallanAmount.Text;
            DateTime.TryParseExact(conddate, "d/MM/yyyy", null, DateTimeStyles.None, out date);
            conddate = date.ToString("MM/dd/yyyy");
            int actrow = 0;
            Int32.TryParse(lblxpos.Text, out actrow);

            string Cond_app_no = FpSpreadCondonationList.Sheets[0].Cells[actrow, 1].Tag.ToString();
            string Cond_semester = FpSpreadCondonationList.Sheets[0].Cells[actrow, 2].Note;
            string Cond_batchyr = FpSpreadCondonationList.Sheets[0].Cells[actrow, 2].Tag.ToString();
            string Cond_degreecode = FpSpreadCondonationList.Sheets[0].Cells[actrow, 1].Note.ToString();
            string Cond_roll_no = FpSpreadCondonationList.Sheets[0].Cells[actrow, 1].Text;
            string Cond_name = FpSpreadCondonationList.Sheets[0].Cells[actrow, 4].Text;
            string Cond_fineamnt = Convert.ToString(FpSpreadCondonationList.Sheets[0].Cells[actrow, 7].Text).Trim();

            string qry = "if exists(select ChallanDate,ChallanNo  from Eligibility_list where app_no= '" + Cond_app_no + "' and Semester= '" + Cond_semester + "' and degree_code= '" + Cond_degreecode + "' and batch_year='" + Cond_batchyr + "' ) update  Eligibility_list set Roll_no= '" + Cond_roll_no + "',stud_name= '" + Cond_name + "',is_eligible='2',batch_year= '" + Cond_batchyr + "' ,Semester= '" + Cond_semester + "',degree_code= '" + Cond_degreecode + "',fine_amt= '" + Cond_fineamnt + "',app_no= '" + Cond_app_no + "',isCondonationFee='1',isCompleteRedo='',Remarks='',ChallanDate='" + date.ToString("MM/dd/yyyy") + "',ChallanNo='" + condchallan + "'  where app_no= '" + Cond_app_no + "' and Semester= '" + Cond_semester + "' and degree_code= '" + Cond_degreecode + "' and batch_year='" + Cond_batchyr + "'";

            int res = da.insertData(qry);
            btnGo_OnClick(sender, e);
            if (res > 0)
            {
                txtCondonationDate.Text = date.ToString("MM/dd/yyyy");
                txtChallanAmount.Text = condchallan;
                FpSpreadCondonationList.Sheets[0].Cells[actrow, 13].Note = date.ToString("MM/dd/yyyy"); 
                FpSpreadCondonationList.Sheets[0].Cells[actrow, 13].Tag = condchallan;
                FpSpreadCondonationList.Sheets[0].AutoPostBack = true;
                divPopUpAlert.Visible = false;
                lblAlertMsg.Text = "Saved Successfully";
            }
            divPopCond.Visible = false;

        }
        catch (Exception ex)
        {

        }
    }


    protected void btnCondExit_Click(object sender, EventArgs e)
    {
        divPopCond.Visible = false;
        txtCondonationDate.Text = string.Empty;
        txtChallanAmount.Text = string.Empty;
        //btngo_Click(sender, e);
    }

}


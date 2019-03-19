/*
 * Author : Mohamed Idhris Sheik Dawood 
 * Date created  : 02-01-2017
 */

using System;
using InsproDataAccess;
using System.Data;
using System.Web.UI.WebControls;
using System.Text;
using System.Web.UI;
using System.Collections;
using System.Collections.Generic;
using System.Data.OleDb;
using System.IO;
using System.Drawing;
using System.Linq;

public partial class AttendanceMOD_AutoTimeTableGeneration : System.Web.UI.Page
{
    string usercode = string.Empty;
    string collegecode = string.Empty;
    string singleuser = string.Empty;
    string group_user = string.Empty;

    InsproDirectAccess dirAccess = new InsproDirectAccess();
    ReuasableMethods reUse = new ReuasableMethods();

    Dictionary<string, byte> dicTotalHoursDetails = new Dictionary<string, byte>();
    Dictionary<string, string> dicAllSubjects = new Dictionary<string, string>();
    Dictionary<string, string> dicBatchYearDetails = new Dictionary<string, string>();
    Dictionary<string, string> dicDegreeDetails = new Dictionary<string, string>();
    Dictionary<string, string> dicAllStaffDetails = new Dictionary<string, string>();
    Dictionary<string, string> dicStaffSelectors = new Dictionary<string, string>();

    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["collegecode"] == null)
        {
            Response.Redirect("~/Default.aspx");
        }
        usercode = Session["usercode"].ToString();
        singleuser = Session["single_user"].ToString();
        group_user = Session["group_code"].ToString();

        if (!IsPostBack)
        {
            loadCollege();
            collegecode = ddlCollege.Items.Count > 0 ? ddlCollege.SelectedValue.ToString() : "13";
            loadEdulevel(collegecode);
            loadBatch(collegecode);
            loadCriteria(collegecode);
        }
        else
        {
            collegecode = ddlCollege.Items.Count > 0 ? ddlCollege.SelectedValue.ToString() : "13";
        }
    }
    //Load college and operation
    private void loadCollege()
    {
        try
        {
            DataTable dtCollege = new DataTable();
            string selectQ = "select collname,college_code from collinfo";
            dtCollege = dirAccess.selectDataTable(selectQ);

            ddlCollege.Items.Clear();
            if (dtCollege.Rows.Count > 0)
            {
                ddlCollege.DataSource = dtCollege;
                ddlCollege.DataTextField = "collname";
                ddlCollege.DataValueField = "college_code";
                ddlCollege.DataBind();
            }
        }
        catch { }
    }
    private void loadEdulevel(string collegeCode)
    {
        try
        {
            DataTable dtEdulev = new DataTable();
            string selectQ = "select distinct Edu_Level from course where college_code='" + collegeCode + "' ";
            dtEdulev = dirAccess.selectDataTable(selectQ);

            ddlEduLev.Items.Clear();
            if (dtEdulev.Rows.Count > 0)
            {
                ddlEduLev.DataSource = dtEdulev;
                ddlEduLev.DataTextField = "Edu_Level";
                ddlEduLev.DataValueField = "Edu_Level";
                ddlEduLev.DataBind();
            }
        }
        catch { }
    }
    private void loadBatch(string collegeCode)
    {
        DataTable dtBatch = new DataTable();
        string selectQ = "select distinct batch_year from applyn where batch_year<>'-1' and batch_year<>''order by batch_year desc";
        dtBatch = dirAccess.selectDataTable(selectQ);
        cbBatch.Checked = true;
        cblBatch.Items.Clear();
        if (dtBatch.Rows.Count > 0)
        {
            cblBatch.DataSource = dtBatch;
            cblBatch.DataTextField = "batch_year";
            cblBatch.DataValueField = "batch_year";
            cblBatch.DataBind();
        }
        reUse.CallCheckBoxChangedEvent(cblBatch, cbBatch, txtBatch, lblBatch.Text);
    }
    private void loadCriteria(string collegeCode)
    {
        try
        {
            ddlCriteria.Items.Clear();
            ArrayList arrLstDegBatch = getDegreeArrayList();
            foreach (string batchDeg in arrLstDegBatch)
            {
                string[] arrBatchDeg = batchDeg.Split('$');
                if (arrBatchDeg.Length == 3)
                {
                    int batchYear = Convert.ToInt32(arrBatchDeg[0]);
                    int degreeCode = Convert.ToInt32(arrBatchDeg[1]);
                    string dispText = Convert.ToString(arrBatchDeg[2]);

                    int currentSem = dirAccess.selectScalarInt("select distinct r.Current_Semester from Registration r where  r.degree_code ='" + degreeCode + "' and r.Batch_Year='" + batchYear + "' and r.college_code='" + collegecode + "'  and CC='0' and DelFlag='0' and Exam_Flag<>'DEBAR'");

                    DataTable dtCriteria = new DataTable();
                    string selectQ = "select distinct criterianame+'-'+subject_code+'-'+c.staff_code as criterianame,criterianame+'-'+convert(varchar(10),c.subject_no) as subject_no from TT_StudentCriteria c,subject s where c.subject_no=s.subject_no and c.semester='" + currentSem + "' and c.degree_code='" + degreeCode + "' and c.batch_year='" + batchYear + "'";// "select distinct criterianame from TT_StudentCriteria ";
                    dtCriteria = dirAccess.selectDataTable(selectQ);

                    if (dtCriteria.Rows.Count > 0)
                    {
                        ddlCriteria.DataSource = dtCriteria;
                        ddlCriteria.DataTextField = "criterianame";
                        ddlCriteria.DataValueField = "subject_no";
                        ddlCriteria.DataBind();
                    }
                }
            }
        }
        catch { }
        ddlCriteria_NewOnIndexChanged(new object(), new EventArgs());
    }
    protected void ddlCollege_IndexChanged(object sender, EventArgs e)
    {
        loadEdulevel(ddlCollege.SelectedValue);
        loadBatch(ddlCollege.SelectedValue);
        loadCriteria(ddlCollege.SelectedValue);
        loadGrid();
    }
    protected void chkBatch_OnCheckedChanged(object sender, EventArgs e)
    {
        reUse.CallCheckBoxChangedEvent(cblBatch, cbBatch, txtBatch, lblBatch.Text);
    }
    protected void cblBatch_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        reUse.CallCheckBoxListChangedEvent(cblBatch, cbBatch, txtBatch, lblBatch.Text);
    }
    protected void ddlCriteria_NewOnIndexChanged(object sender, EventArgs e)
    {
        ddlCriteriaReduced.Items.Clear();
        for (int i = 0; i < ddlCriteria.Items.Count; i++)
        {
            if (ddlCriteria.SelectedIndex != i)
            {
                ListItem item = new ListItem(ddlCriteria.Items[i].Text, ddlCriteria.Items[i].Value);
                ddlCriteriaReduced.Items.Add(item);
            }
        }
    }
    protected void cb_select_CheckedChanged(object sender, EventArgs e)
    {
        int ro = 0;
        int indx = rowIndxClicked();
        foreach (GridViewRow gRow in gridDetails.Rows)
        {
            CheckBox cb_select = (CheckBox)gRow.FindControl("cb_select");
            cb_select.Checked = false;
            if (ro == indx)
            {
                cb_select.Checked = true;
            }
            ro++;
        }
        loadCriteria(collegecode);
    }
    public static Control GetPostBackControl(Page page)
    {
        Control control = null;
        string ctrlname = page.Request.Params.Get("__EVENTTARGET");
        if (ctrlname != null && ctrlname != string.Empty)
        {
            control = page.FindControl(ctrlname);
        }
        else
        {
            foreach (string ctl in page.Request.Form)
            {
                Control c = page.FindControl(ctl);
                if (c is System.Web.UI.WebControls.Button)
                {
                    control = c;
                    break;
                }
            }
        }
        return control;
    }
    public int rowIndxClicked()
    {
        int rownumber = -1;
        try
        {
            Control ctrlid = GetPostBackControl(this.Page);
            string rno = Convert.ToString(ctrlid.UniqueID).Split('$')[3].Replace("ctl", "");
            int.TryParse(rno, out rownumber);
            rownumber -= 2;
        }
        catch { rownumber = -1; }

        return rownumber;
    }
    //Load Grid
    protected void btnGo_Click(object sender, EventArgs e)
    {
        loadGrid();
    }
    private void loadGrid()
    {
        try
        {
            lblRecNotFound.Visible = false;
            gridDetails.Visible = false;
            gridDetails.DataSource = null;
            gridDetails.DataBind();
            btnGenerate.Visible = false;
            btnGenOption.Visible = false;

            string collegeCode = collegecode;
            string batch = reUse.GetSelectedItemsText(cblBatch);
            string eduLev = ddlEduLev.Items.Count > 0 ? ddlEduLev.SelectedItem.Text : string.Empty;

            if (collegeCode != string.Empty && batch != string.Empty && eduLev != string.Empty)
            {
                DataTable dtDegBranch = new DataTable();
                string selectQ = "select c.Course_Id,c.Course_Name,d.Degree_Code,dt.Dept_Name from Course c, Degree d, Department dt where c.Course_Id=d.Course_Id and d.Dept_Code=dt.Dept_Code and c.college_code='" + collegeCode + "' and c.Edu_Level='" + eduLev + "' ";
                dtDegBranch = dirAccess.selectDataTable(selectQ);
                if (dtDegBranch.Rows.Count > 0)
                {
                    DataTable dtBind = new DataTable();
                    dtBind.Columns.Add("Batch");
                    dtBind.Columns.Add("Degree");
                    dtBind.Columns.Add("DegreeCode");
                    dtBind.Columns.Add("Branch");
                    dtBind.Columns.Add("BranchCode");

                    for (int batI = 0; batI < cblBatch.Items.Count; batI++)
                    {
                        if (cblBatch.Items[batI].Selected)
                        {
                            string curBatch = cblBatch.Items[batI].Value.Trim();
                            for (int degBraI = 0; degBraI < dtDegBranch.Rows.Count; degBraI++)
                            {
                                DataRow drBind = dtBind.NewRow();
                                drBind["Batch"] = curBatch;
                                drBind["Degree"] = Convert.ToString(dtDegBranch.Rows[degBraI]["Course_Name"]).Trim();
                                drBind["Branch"] = Convert.ToString(dtDegBranch.Rows[degBraI]["Dept_Name"]).Trim();
                                drBind["DegreeCode"] = Convert.ToString(dtDegBranch.Rows[degBraI]["Course_Id"]).Trim();
                                drBind["BranchCode"] = Convert.ToString(dtDegBranch.Rows[degBraI]["Degree_Code"]).Trim();
                                dtBind.Rows.Add(drBind);
                            }
                        }
                    }

                    gridDetails.Visible = true;
                    gridDetails.DataSource = dtBind;
                    gridDetails.DataBind();

                    btnGenerate.Visible = true;
                    btnGenOption.Visible = true;
                }
                else
                {
                    lblRecNotFound.Visible = true;
                }

            }
            else
            {
                ScriptManager.RegisterStartupScript(this, typeof(Page), UniqueID, "alert('Please provide all inputs')", true);
            }
        }
        catch { }
    }
    protected void gridDetails_OnRowDataBound(object sender, GridViewRowEventArgs e)
    {
    }
    protected void gridDetails_DataBound(object sender, EventArgs e)
    {
        try
        {
            for (int i = gridDetails.Rows.Count - 1; i > 0; i--)
            {
                GridViewRow row = gridDetails.Rows[i];
                GridViewRow previousRow = gridDetails.Rows[i - 1];
                for (int j = 1; j <= 3; j++)
                {
                    bool validation = false;
                    switch (j)
                    {
                        case 1:
                            {
                                Label lnlname = (Label)row.FindControl("lbl_Batch");
                                Label lnlname1 = (Label)previousRow.FindControl("lbl_Batch");
                                if (lnlname.Text == lnlname1.Text)
                                {
                                    validation = true;
                                }
                            }
                            break;
                        case 2:
                            {
                                Label lnlname = (Label)row.FindControl("lbl_Degree");
                                Label lnlname1 = (Label)previousRow.FindControl("lbl_Degree");
                                if (lnlname.Text == lnlname1.Text)
                                {
                                    validation = true;
                                }
                            }
                            break;
                        case 3:
                            {
                                Label lnlname = (Label)row.FindControl("lbl_Branch");
                                Label lnlname1 = (Label)previousRow.FindControl("lbl_Branch");
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
    //Manual Staff Allotment
    protected void btnStaffAllot_Click(object sender, EventArgs e)
    {
        divAllotStaff.Visible = true;
        loadStaff();
        loadStaffBatch();
        loadDegree();
        loadStaffBranch();
        loadStaffSem();
    }
    protected void closedivAllotStaff(object sender, EventArgs e)
    {
        divAllotStaff.Visible = false;
    }
    private void loadStaff()
    {
        ddlStaff.Items.Clear();
        try
        {
            DataTable dtStaff = dirAccess.selectDataTable("select sa.appl_id,sm.staff_code+'-'+sm.staff_name as staffname  from staffmaster sm,stafftrans st,hrdept_master h,staff_appl_master sa where sm.staff_code=st.staff_code and sm.appl_no=sa.appl_no and st.dept_code=h.dept_code and sm.college_code=h.college_code and st.latestrec='1' and sm.resign=0 and sm.settled=0 and ISNULL(Discontinue,'0')='0' and sm.college_code ='" + collegecode + "' ");
            ddlStaff.DataSource = dtStaff;
            ddlStaff.DataTextField = "staffname";
            ddlStaff.DataValueField = "appl_id";
            ddlStaff.DataBind();
        }
        catch { }
        ddlStaff.Items.Insert(0, "Select");
    }
    private void loadStaffBatch()
    {
        ddlBatchStaffAllot.Items.Clear();
        try
        {
            DataTable dtBatch = dirAccess.selectDataTable("select distinct batch_year from applyn where batch_year<>'-1' and batch_year<>''order by batch_year desc ");
            ddlBatchStaffAllot.DataSource = dtBatch;
            ddlBatchStaffAllot.DataTextField = "batch_year";
            ddlBatchStaffAllot.DataValueField = "batch_year";
            ddlBatchStaffAllot.DataBind();
        }
        catch { }
    }
    private void loadDegree()
    {
        ddlDegreeStaffAllot.Items.Clear();
        try
        {
            DataTable dtDegree = dirAccess.selectDataTable("select c.Course_Id,c.Course_Name from Course c where  c.college_code='" + collegecode + "' ");
            ddlDegreeStaffAllot.DataSource = dtDegree;
            ddlDegreeStaffAllot.DataTextField = "Course_Name";
            ddlDegreeStaffAllot.DataValueField = "Course_Id";
            ddlDegreeStaffAllot.DataBind();
        }
        catch { }
    }
    private void loadStaffBranch()
    {
        ddlBranchStaffAllot.Items.Clear();
        try
        {
            DataTable dtBranch = dirAccess.selectDataTable("select d.Degree_Code,dt.Dept_Name from Course c, Degree d, Department dt where c.Course_Id=d.Course_Id and d.Dept_Code=dt.Dept_Code and c.college_code='" + collegecode + "' and c.Course_Id='" + (ddlDegreeStaffAllot.Items.Count > 0 ? ddlDegreeStaffAllot.SelectedValue : "0") + "' ");
            ddlBranchStaffAllot.DataSource = dtBranch;
            ddlBranchStaffAllot.DataTextField = "Dept_Name";
            ddlBranchStaffAllot.DataValueField = "Degree_Code";
            ddlBranchStaffAllot.DataBind();
        }
        catch { }
    }
    private void loadStaffSem()
    {
        ddlSemStaffAllot.Items.Clear();
        try
        {
            for (int semI = 1; semI < 11; semI++)
            {
                ddlSemStaffAllot.Items.Add(semI.ToString());
            }
            //DataTable dtSem = dirAccess.selectDataTable("select sa.appl_id,sm.staff_code+'-'+sm.staff_name as staffname  from staffmaster sm,stafftrans st,hrdept_master h,staff_appl_master sa where sm.staff_code=st.staff_code and sm.appl_no=sa.appl_no and st.dept_code=h.dept_code and sm.college_code=h.college_code and st.latestrec='1' and sm.resign=0 and sm.settled=0 and ISNULL(Discontinue,'0')='0' and sm.college_code ='" + collegecode + "' ");
            //ddlSemStaffAllot.DataSource = dtSem;
            //ddlSemStaffAllot.DataTextField = "staffname";
            //ddlSemStaffAllot.DataValueField = "appl_id";
            //ddlSemStaffAllot.DataBind();
        }
        catch { }
    }
    //Import Click
    protected void btnImport_Click(object sender, EventArgs e)
    {
        divImport.Visible = true;
    }
    protected void closeTTImport(object sender, EventArgs e)
    {
        divImport.Visible = false;
    }
    protected void btnImportExcel_Click(object sender, EventArgs e)
    {
        try
        {
            using (Stream Stream = this.fuImport.FileContent as Stream)
            {
                if (fuImport.HasFile)
                {
                    string extension = Path.GetFileName(fuImport.PostedFile.FileName);
                    if (extension.Trim() != "")
                    {
                        if (System.IO.Path.GetExtension(fuImport.FileName) == ".xls" || System.IO.Path.GetExtension(fuImport.FileName) == ".xlsx")
                        {
                            OleDbDataAdapter adapter = new OleDbDataAdapter();
                            string path = Server.MapPath("~/Upload/StudentBatchYearUpdation" + System.IO.Path.GetExtension(fuImport.FileName));
                            fuImport.SaveAs(path);
                            DataSet ds = new DataSet();
                            ds = Excelconvertdataset(path);
                            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                            {
                                if (rblImportType.SelectedIndex == 0)
                                {
                                    importStaffAllotment(ds);
                                }
                                else
                                {
                                    //importTimeTableCriteria(ds);
                                    ImportTimeTableCriteria();
                                }
                            }
                            else
                            {
                                ScriptManager.RegisterStartupScript(this, typeof(Page), UniqueID, "alert('Excel does not having any data!')", true);
                                return;
                            }
                        }
                        else
                        {
                            ScriptManager.RegisterStartupScript(this, typeof(Page), UniqueID, "alert('Please Select .xls and .xlsx files only!!!')", true);
                            return;
                        }
                    }
                    else
                    {
                        ScriptManager.RegisterStartupScript(this, typeof(Page), UniqueID, "alert('Please Browse Upload File')", true);
                        return;
                    }
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this, typeof(Page), UniqueID, "alert('Please Browse Upload File')", true);
                    return;
                }
            }
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterStartupScript(this, typeof(Page), UniqueID, "alert('Please Try Later')", true);
        }
    }
    private void importStaffAllotment(DataSet ds)
    {
        DataTable dtStaffDet = dirAccess.selectDataTable("select sa.appl_id,sm.staff_code,sm.staff_name  from staffmaster sm,stafftrans st,hrdept_master h,staff_appl_master sa where sm.staff_code=st.staff_code and sm.appl_no=sa.appl_no and st.dept_code=h.dept_code and sm.college_code=h.college_code and st.latestrec='1' and sm.resign=0 and sm.settled=0 and ISNULL(Discontinue,'0')='0' and sm.college_code ='" + collegecode + "'");

        for (int rowI = 0; rowI < ds.Tables[0].Rows.Count; rowI++)
        {
            string staffcode = Convert.ToString(ds.Tables[0].Rows[rowI][0]).Trim();
            dtStaffDet.DefaultView.RowFilter = " staff_code='" + staffcode + "'";
            DataView dvStaff = dtStaffDet.DefaultView;
            string applid = string.Empty;


            if (dvStaff.Count > 0)
            {
                applid = dvStaff[0]["appl_id"].ToString().Trim();
                if (applid != string.Empty)
                {
                    for (int colI = 1; colI < ds.Tables[0].Columns.Count; colI++)
                    {
                        string[] hrs = ds.Tables[0].Rows[rowI][colI].ToString().Trim().Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                        if (hrs.Length > 0 && hrs[0].Trim().ToUpper() != "NIL" && hrs[0].Trim().ToUpper() != "")
                        {
                            foreach (string hour in hrs)
                            {
                                string Q = "if exists (select staffallotavailpk from TT_Staff_AllotAvail where Semester='0' and degreeCode='0' and DaysFk='" + colI + "' and HoursFk='" + hour + "' and batch_year='0000' and staff_appno='" + applid + "') update  TT_Staff_AllotAvail set IsEngaged='1' where Semester='0' and degreeCode='0' and DaysFk='" + colI + "' and HoursFk='" + hour + "' and batch_year='0000' and staff_appno='" + applid + "' else insert into TT_Staff_AllotAvail (staff_appno ,DaysFK ,HoursFK , degreeCode ,batch_year , semester ,IsEngaged ,subject_no) values (" + applid + " ," + colI + " ," + hour + " , '0' ,'0000' , '0' ,'1' ,'0')";

                                dirAccess.updateData(Q);
                            }
                        }
                    }
                }
            }
        }
        ScriptManager.RegisterStartupScript(this, typeof(Page), UniqueID, "alert('Import Success!')", true);
    }
    private void importTimeTableCriteria(DataSet ds)
    {
        for (int rowI = 0; rowI < ds.Tables[0].Rows.Count; rowI++)
        {
            string timetableCriteria = Convert.ToString(ds.Tables[0].Rows[rowI][0]).Trim();
            if (timetableCriteria != string.Empty)
            {
                for (int colI = 1; colI < ds.Tables[0].Columns.Count; colI++)
                {
                    string[] hrs = ds.Tables[0].Rows[rowI][colI].ToString().Trim().Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                    if (hrs.Length > 0 && hrs[0].Trim().ToUpper() != "NIL" && hrs[0].Trim().ToUpper() != "")
                    {
                        foreach (string hour in hrs)
                        {
                            string Q = "if exists (select criteriaPk from TT_StudentCriteria where DayPk='" + colI + "' and HourPk='" + hour + "' and CriteriaName='" + timetableCriteria + "') update TT_StudentCriteria set IsEngaged='1' where DayPk='" + colI + "' and HourPk='" + hour + "' and CriteriaName='" + timetableCriteria + "' else insert into TT_StudentCriteria(CriteriaName,DayPk,HourPk,IsEngaged) values ('" + timetableCriteria + "','" + colI + "','" + hour + "','1')";

                            dirAccess.updateData(Q);
                        }
                    }
                }
            }

        }
        ScriptManager.RegisterStartupScript(this, typeof(Page), UniqueID, "alert('Import Success!')", true);
    }
    public static DataSet Excelconvertdataset(string path)
    {
        DataSet ds3 = new DataSet();
        string StrSheetName = string.Empty;

        string excelConnectionString = @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + path + ";Extended Properties='Excel 12.0;HDR=YES;IMEX=1;';";
        OleDbConnection excelConnection = new OleDbConnection(excelConnectionString);
        try
        {
            OleDbDataAdapter adapter = new OleDbDataAdapter();
            if (excelConnection.State == ConnectionState.Closed)
                excelConnection.Open();
            DataTable dtSheets = excelConnection.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);
            if (dtSheets != null && dtSheets.Rows.Count > 0)
            {
                StrSheetName = dtSheets.Rows[0].ItemArray[2].ToString();

            }
            if (!string.IsNullOrEmpty(StrSheetName))
            {
                OleDbCommand cmd = new OleDbCommand("Select * from [" + StrSheetName + "]", excelConnection);
                adapter = new OleDbDataAdapter(cmd);
                adapter.Fill(ds3, "excelData");
            }
        }
        catch
        {

        }
        finally
        {
            if (excelConnection.State != ConnectionState.Closed)
                excelConnection.Close();
        }
        return ds3;
    }
    //Show Faculty, Course and Hour, manual settings option
    protected void generateOptions(object sender, EventArgs e)
    {
        loadOptionsGrid();
    }
    private void loadOptionsGrid()
    {
        try
        {
            gridUserOptions.Visible = false;
            gridUserOptions.DataSource = null;
            gridUserOptions.DataBind();

            ArrayList arrLstDegBatch = getDegreeArrayList();
            foreach (string batchDeg in arrLstDegBatch)
            {
                string[] arrBatchDeg = batchDeg.Split('$');
                if (arrBatchDeg.Length == 3)
                {
                    int batchYear = Convert.ToInt32(arrBatchDeg[0]);
                    int degreeCode = Convert.ToInt32(arrBatchDeg[1]);
                    string dispText = Convert.ToString(arrBatchDeg[2]);

                    int currentSem = dirAccess.selectScalarInt("select distinct r.Current_Semester from Registration r where  r.degree_code ='" + degreeCode + "' and r.Batch_Year='" + batchYear + "' and r.college_code='" + collegecode + "'  and CC='0' and DelFlag='0' and Exam_Flag<>'DEBAR'");

                    DataTable dtSubjectDet = dirAccess.selectDataTable("select sm.syll_code,ss.subType_no,ss.subject_type,ss.ElectivePap,ss.Lab,s.subject_no,s.subject_code,s.subject_name,isnull(s.sub_lab,0) as sub_lab,isnull(s.noofhrsperweek,0) as noofhrsperweek,s.maximumHrsPerDay,isnull(s.subjectpriority,0) as subjectpriority from syllabus_master sm,sub_sem ss, subject s where sm.syll_code = ss.syll_code and sm.syll_code = s.syll_code and ss.syll_code= s.syll_code and ss.subType_no = s.subType_no and sm.Batch_Year='" + batchYear + "' and sm.degree_code ='" + degreeCode + "' and sm.semester = '" + currentSem + "'");

                    if (dtSubjectDet.Rows.Count > 0)
                    {
                        gridUserOptions.DataSource = dtSubjectDet;
                        gridUserOptions.DataBind();
                        gridUserOptions.Visible = true;
                    }

                }
            }
        }
        catch
        {
            gridUserOptions.Visible = false;
            gridUserOptions.DataSource = null;
            gridUserOptions.DataBind();
        }
    }
    protected void gridUserOptions_OnRowDataBound(object sender, GridViewRowEventArgs e)
    {

    }
    protected void gridUserOptions_DataBound(object sender, EventArgs e)
    {
        try
        {
            DataTable dtStaffSubjectDet = dirAccess.selectDataTable("select sa.appl_id,sm.syll_code,ss.subType_no,ss.subject_type,isnull(ss.ElectivePap,0) as ElectivePap,ss.Lab,isnull(s.sub_lab,0) as sub_lab,isnull(s.noofhrsperweek,0) as noofhrsperweek, s.maximumHrsPerDay, s.subject_no, sts.staff_code, staff_name, s.subject_code, s.subject_name, isnull(s.subjectpriority,0) as subjectpriority, sts.staffPriority, sts.facultyChoice   from  staff_appl_master sa, staffmaster sf ,syllabus_master sm,sub_sem ss, subject s,staff_selector sts where sm.syll_code = ss.syll_code and sm.syll_code = s.syll_code and ss.syll_code= s.syll_code and ss.subType_no = s.subType_no and s.subject_no =sts.subject_no and sa.appl_no = sf.appl_no and sf.staff_code=sts.staff_code order by sts.facultyChoice asc ");

            //LabDet
            DataTable dtLabSubjDetWt = dirAccess.selectDataTable("select sa.appl_id, sm.syll_code, ss.subType_no, ss.subject_type, isnull(ss.ElectivePap,0) as ElectivePap, ss.Lab,s.subject_no, s.subject_code, s.subject_name,sf.staff_name, isnull(s.sub_lab,0) as sub_lab, isnull(s.noofhrsperweek,0) as noofhrsperweek, s.maximumHrsPerDay, sts.staff_code, isnull(s.subjectpriority,0) as subjectpriority, sts.staffPriority, sts.facultyChoice, lc.FacLabChoiceValue from  staff_appl_master sa, staffmaster sf ,syllabus_master sm,sub_sem ss, subject s,staff_selector sts,TT_facultyLabChoice lc where sts.staffPriority=lc.staffPriorityFk and sts.facultyChoice is null and ss.Lab='1' and sm.syll_code = ss.syll_code and sm.syll_code = s.syll_code and ss.syll_code= s.syll_code and ss.subType_no = s.subType_no and s.subject_no =sts.subject_no and sa.appl_no = sf.appl_no and sf.staff_code=sts.staff_code  and sf.college_code ='" + collegecode + "'  order by lc.FacLabChoiceValue asc ");//and sm.Batch_Year='" + batchYear + "' and sm.degree_code ='" + degreeCode + "' and sm.semester = '" + currentSem + "'

            byte maxFacultyChoice = (byte)dirAccess.selectScalarInt("select max(isnull(sts.facultyChoice,1)) as facultyChoice from  staff_appl_master sa, staffmaster sf ,syllabus_master sm,sub_sem ss, subject s,staff_selector sts where sm.syll_code = ss.syll_code and sm.syll_code = s.syll_code and ss.syll_code= s.syll_code and ss.subType_no = s.subType_no and s.subject_no =sts.subject_no and sa.appl_no = sf.appl_no and sf.staff_code=sts.staff_code ");// and sf.college_code ='" + collegecode + "' and sm.Batch_Year='" + batchYear + "' and sm.degree_code ='" + degreeCode + "' and sm.semester = '" + currentSem + "'

            foreach (GridViewRow gRow in gridUserOptions.Rows)
            {
                Label lblSubNo = (Label)gRow.FindControl("lblSubNo");
                Label lblIsLab = (Label)gRow.FindControl("lblIsLab");
                bool isLab = lblIsLab.Text.Trim().ToUpper() == "TRUE" ? true : false;
                DropDownList ddlFaculty = (DropDownList)gRow.FindControl("ddlFaculty");


                if (!isLab)
                {
                    dtStaffSubjectDet.DefaultView.RowFilter = "subject_no='" + lblSubNo.Text + "'";
                    DataView dvStaffs = dtStaffSubjectDet.DefaultView;
                    DataTable dtStaff = dvStaffs.ToTable();
                    if (dtStaff.Rows.Count > 0)
                    {
                        ddlFaculty.DataSource = dtStaff;
                        ddlFaculty.DataTextField = "staff_name";
                        ddlFaculty.DataValueField = "appl_id";
                        ddlFaculty.DataBind();
                    }
                }
                else
                {
                    dtLabSubjDetWt.DefaultView.RowFilter = "subject_no='" + lblSubNo.Text + "'";
                    DataView dvStaffs = dtLabSubjDetWt.DefaultView;
                    DataTable dtStaff = dvStaffs.ToTable();
                    if (dtStaff.Rows.Count > 0)
                    {
                        for (int facChoiceI = 1; facChoiceI <= maxFacultyChoice; facChoiceI++)
                        {
                            dtStaff.DefaultView.RowFilter = "FacLabChoiceValue='" + facChoiceI + "'";
                            DataView dvFacLab = dtStaff.DefaultView;
                            if (dvFacLab.Count > 1)
                            {
                                string staff = Convert.ToString(dvFacLab[0]["staff_name"]) + "-" + Convert.ToString(dvFacLab[1]["staff_name"]);
                                string staffid = Convert.ToString(dvFacLab[0]["appl_id"]) + "-" + Convert.ToString(dvFacLab[1]["appl_id"]);
                                ListItem lstStaff = new ListItem(staff, staffid);
                                ddlFaculty.Items.Add(lstStaff);
                            }
                        }
                    }
                }
                ddlFaculty.Items.Insert(0, " ");
            }

        }
        catch { }

    }
    protected void ddlFaculty_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        int noOfHrsPerday = dirAccess.selectScalarInt("select No_of_hrs_per_day from PeriodAttndSchedule");// where degree_code ='" + degreeCode + "' and semester ='" + currentSem + "'

        //Staff availability
        DataTable dtStaffDet = dirAccess.selectDataTable("select staff_appno ,DaysFK ,HoursFK , degreeCode ,batch_year , semester ,IsEngaged ,subject_no ,section  from TT_Staff_AllotAvail  ");//where Batch_Year='" + batchYear + "' and degreecode ='" + degreeCode + "' and semester = '" + currentSem + "'


        foreach (GridViewRow gRow in gridUserOptions.Rows)
        {
            Label lblSubNo = (Label)gRow.FindControl("lblSubNo");
            DropDownList ddlFaculty = (DropDownList)gRow.FindControl("ddlFaculty");
            DropDownList ddlMondayHour = (DropDownList)gRow.FindControl("ddlMondayHour");
            DropDownList ddlTuesdayHour = (DropDownList)gRow.FindControl("ddlTuesdayHour");
            DropDownList ddlWednesdayHour = (DropDownList)gRow.FindControl("ddlWednesdayHour");
            DropDownList ddlThursdayHour = (DropDownList)gRow.FindControl("ddlThursdayHour");
            DropDownList ddlFridayHour = (DropDownList)gRow.FindControl("ddlFridayHour");
            Label lblSubType = (Label)gRow.FindControl("lblSubType");

            if (ddlFaculty.SelectedIndex != 0)
            {
                if (ddlMondayHour.Items.Count == 0 || lblSubType.Text.Trim().ToUpper() == "LAB")
                {
                    if (lblSubType.Text.Trim().ToUpper() == "LAB")
                    {
                        ddlMondayHour.Items.Clear();
                        ddlTuesdayHour.Items.Clear();
                        ddlWednesdayHour.Items.Clear();
                        ddlThursdayHour.Items.Clear();
                        ddlFridayHour.Items.Clear();
                    }
                    //Already Engaged
                    dtStaffDet.DefaultView.RowFilter = " staff_appno='" + ddlFaculty.SelectedValue + "'";
                    DataView dvStaffAvail = dtStaffDet.DefaultView;


                    ddlMondayHour.Items.Add(string.Empty);
                    ddlTuesdayHour.Items.Add(string.Empty);
                    ddlWednesdayHour.Items.Add(string.Empty);
                    ddlThursdayHour.Items.Add(string.Empty);
                    ddlFridayHour.Items.Add(string.Empty);
                    if (lblSubType.Text.Trim().ToUpper() != "LAB")
                    {
                        for (int hrsI = 1; hrsI <= noOfHrsPerday; hrsI++)
                        {
                            bool AddMonday = true;
                            if (dvStaffAvail.Count > 0)
                            {
                                DataTable dtStaffAvail = dvStaffAvail.ToTable();
                                dtStaffAvail.DefaultView.RowFilter = "DaysFk='" + 1 + "' and HoursFk='" + hrsI + "' and IsEngaged='True'";
                                DataView fnlDvStaffAvail = dtStaffAvail.DefaultView;
                                if (fnlDvStaffAvail.Count > 0)
                                    AddMonday = false;
                            }
                            if (AddMonday)
                                ddlMondayHour.Items.Add(hrsI.ToString());


                            bool AddTuesday = true;
                            if (dvStaffAvail.Count > 0)
                            {
                                DataTable dtStaffAvail = dvStaffAvail.ToTable();
                                dtStaffAvail.DefaultView.RowFilter = "DaysFk='" + 2 + "' and HoursFk='" + hrsI + "' and IsEngaged='True'";
                                DataView fnlDvStaffAvail = dtStaffAvail.DefaultView;
                                if (fnlDvStaffAvail.Count > 0)
                                    AddTuesday = false;
                            }
                            if (AddTuesday)
                                ddlTuesdayHour.Items.Add(hrsI.ToString());

                            bool AddWednesday = true;
                            if (dvStaffAvail.Count > 0)
                            {
                                DataTable dtStaffAvail = dvStaffAvail.ToTable();
                                dtStaffAvail.DefaultView.RowFilter = "DaysFk='" + 3 + "' and HoursFk='" + hrsI + "' and IsEngaged='True'";
                                DataView fnlDvStaffAvail = dtStaffAvail.DefaultView;
                                if (fnlDvStaffAvail.Count > 0)
                                    AddWednesday = false;
                            }
                            if (AddWednesday)
                                ddlWednesdayHour.Items.Add(hrsI.ToString());

                            bool AddThursday = true;
                            if (dvStaffAvail.Count > 0)
                            {
                                DataTable dtStaffAvail = dvStaffAvail.ToTable();
                                dtStaffAvail.DefaultView.RowFilter = "DaysFk='" + 4 + "' and HoursFk='" + hrsI + "' and IsEngaged='True'";
                                DataView fnlDvStaffAvail = dtStaffAvail.DefaultView;
                                if (fnlDvStaffAvail.Count > 0)
                                    AddThursday = false;
                            }
                            if (AddThursday)
                                ddlThursdayHour.Items.Add(hrsI.ToString());

                            bool AddFriday = true;
                            if (dvStaffAvail.Count > 0)
                            {
                                DataTable dtStaffAvail = dvStaffAvail.ToTable();
                                dtStaffAvail.DefaultView.RowFilter = "DaysFk='" + 5 + "' and HoursFk='" + hrsI + "' and IsEngaged='True'";
                                DataView fnlDvStaffAvail = dtStaffAvail.DefaultView;
                                if (fnlDvStaffAvail.Count > 0)
                                    AddFriday = false;
                            }
                            if (AddFriday)
                                ddlFridayHour.Items.Add(hrsI.ToString());
                        }
                    }
                    else
                    {
                        string[] staffs = ddlFaculty.SelectedValue.ToString().Split('-');

                        dtStaffDet.DefaultView.RowFilter = " staff_appno='" + staffs[0] + "' or staff_appno='" + staffs[1] + "'";
                        dvStaffAvail = dtStaffDet.DefaultView;

                        for (int hrsI = 1; hrsI <= noOfHrsPerday; hrsI += 2)
                        {
                            bool AddMonday = true;
                            if (dvStaffAvail.Count > 0)
                            {
                                DataTable dtStaffAvail = dvStaffAvail.ToTable();
                                dtStaffAvail.DefaultView.RowFilter = "DaysFk='" + 1 + "' and (HoursFk='" + hrsI + "' or HoursFk='" + (hrsI + 1) + "') and IsEngaged='True'";
                                DataView fnlDvStaffAvail = dtStaffAvail.DefaultView;
                                if (fnlDvStaffAvail.Count > 0)
                                    AddMonday = false;
                            }
                            if (AddMonday)
                                ddlMondayHour.Items.Add(hrsI.ToString() + "," + (hrsI + 1).ToString());


                            bool AddTuesday = true;
                            if (dvStaffAvail.Count > 0)
                            {
                                DataTable dtStaffAvail = dvStaffAvail.ToTable();
                                dtStaffAvail.DefaultView.RowFilter = "DaysFk='" + 2 + "' and (HoursFk='" + hrsI + "' or HoursFk='" + (hrsI + 1) + "') and IsEngaged='True'";
                                DataView fnlDvStaffAvail = dtStaffAvail.DefaultView;
                                if (fnlDvStaffAvail.Count > 0)
                                    AddTuesday = false;
                            }
                            if (AddTuesday)
                                ddlTuesdayHour.Items.Add(hrsI.ToString() + "," + (hrsI + 1).ToString());

                            bool AddWednesday = true;
                            if (dvStaffAvail.Count > 0)
                            {
                                DataTable dtStaffAvail = dvStaffAvail.ToTable();
                                dtStaffAvail.DefaultView.RowFilter = "DaysFk='" + 3 + "' and (HoursFk='" + hrsI + "' or HoursFk='" + (hrsI + 1) + "') and IsEngaged='True'";
                                DataView fnlDvStaffAvail = dtStaffAvail.DefaultView;
                                if (fnlDvStaffAvail.Count > 0)
                                    AddWednesday = false;
                            }
                            if (AddWednesday)
                                ddlWednesdayHour.Items.Add(hrsI.ToString() + "," + (hrsI + 1).ToString());

                            bool AddThursday = true;
                            if (dvStaffAvail.Count > 0)
                            {
                                DataTable dtStaffAvail = dvStaffAvail.ToTable();
                                dtStaffAvail.DefaultView.RowFilter = "DaysFk='" + 4 + "' and (HoursFk='" + hrsI + "' or HoursFk='" + (hrsI + 1) + "') and IsEngaged='True'";
                                DataView fnlDvStaffAvail = dtStaffAvail.DefaultView;
                                if (fnlDvStaffAvail.Count > 0)
                                    AddThursday = false;
                            }
                            if (AddThursday)
                                ddlThursdayHour.Items.Add(hrsI.ToString() + "," + (hrsI + 1).ToString());

                            bool AddFriday = true;
                            if (dvStaffAvail.Count > 0)
                            {
                                DataTable dtStaffAvail = dvStaffAvail.ToTable();
                                dtStaffAvail.DefaultView.RowFilter = "DaysFk='" + 5 + "' and (HoursFk='" + hrsI + "' or HoursFk='" + (hrsI + 1) + "') and IsEngaged='True'";
                                DataView fnlDvStaffAvail = dtStaffAvail.DefaultView;
                                if (fnlDvStaffAvail.Count > 0)
                                    AddFriday = false;
                            }
                            if (AddFriday)
                                ddlFridayHour.Items.Add(hrsI.ToString() + "," + (hrsI + 1).ToString());
                        }
                    }
                }

            }
            else
            {
                ddlMondayHour.Items.Clear();
                ddlTuesdayHour.Items.Clear();
                ddlWednesdayHour.Items.Clear();
                ddlThursdayHour.Items.Clear();
                ddlFridayHour.Items.Clear();
            }
        }
    }
    //Generate TimeTable
    protected void closeTTOutput(object sender, EventArgs e)
    {
        ddlSelectedTimeTable.Items.Clear();
        ddlCriteriaReduced.Items.Clear();
        ddlCriteria_NewOnIndexChanged(sender, e);
       
        divTimeTableOutput.Visible = false;
    }
    protected void btnClear_Click(object sender, EventArgs e)
    {
        try
        {
            #region Clear Sessions
            if (Session["selectedDataSet"] != null)
            {
                Session.Remove("selectedDataSet");
            }
            if (Session["prevDataSet"] != null)
            {
                Session.Remove("prevDataSet");
            }
            if (Session["FromSaved"] != null)
            {
                Session.Remove("FromSaved");
            }
            #endregion
        }
        catch { }
    }
    protected void btnGenerate_Click(object sender, EventArgs e)
    {
        try
        {
            bindStaff();
            #region Clear Sessions
            if (Session["selectedDataSet"] != null && Session["prevDataSet"] != null)
            {
                saveAndGenerate(0);
                return;
            }

            //if (Session["selectedDataSet"] != null)
            //{
            //    Session.Remove("selectedDataSet");
            //}
            //if (Session["prevDataSet"] != null)
            //{
            //    Session.Remove("prevDataSet");
            //}
            #endregion
            DataSet dsTimeTable = new DataSet();
            #region Generate Time Table for every selected branches
            ArrayList arrLstDegBatch = getDegreeArrayList();
            foreach (string batchDeg in arrLstDegBatch)
            {
                string[] arrBatchDeg = batchDeg.Split('$');
                if (arrBatchDeg.Length == 3)
                {
                    int batchYear = Convert.ToInt32(arrBatchDeg[0]);
                    int degreeCode = Convert.ToInt32(arrBatchDeg[1]);
                    string dispText = Convert.ToString(arrBatchDeg[2]);

                    int currentSem = dirAccess.selectScalarInt("select distinct r.Current_Semester from Registration r where  r.degree_code ='" + degreeCode + "' and r.Batch_Year='" + batchYear + "' and r.college_code='" + collegecode + "'  and CC='0' and DelFlag='0' and Exam_Flag<>'DEBAR'");

                    DataTable dtTimeTable = new DataTable();
                    //Room Avalability Detail
                    ArrayList arrlstRoomDet = new ArrayList();
                    ArrayList arrlstLabDet = new ArrayList();
                    Dictionary<string, int> dicRoomAvailability = getRoomAvailability(batchYear, degreeCode, currentSem, ref arrlstRoomDet, ref arrlstLabDet, 0);

                    int noOfHrsPerDay = 0;
                    DataTable dtBellSchedule = new DataTable();
                    DataTable dtSubjectDet = new DataTable();
                    DataTable dtSubjectDetWt = new DataTable();
                    DataTable dtStaffDet = new DataTable();

                    //DataTable dtCriteria = dirAccess.selectDataTable("select criterianame ,DayPk,HourPk,IsEngaged from TT_StudentCriteria where criterianame='" + ddlCriteria.SelectedItem.Text + "'");

                    DataTable dtCriteria = dirAccess.selectDataTable("select distinct subject_code+'-'+c.staff_code as criterianame,DayPk,HourPk,IsEngaged from TT_StudentCriteria c,subject s where c.subject_no=s.subject_no and c.semester='" + currentSem + "' and c.degree_code='" + degreeCode + "' and c.batch_year='" + batchYear + "' and c.criterianame='" + ddlCriteria.SelectedItem.Text.Split('-')[0] + "'  and c.staff_code='" + ddlCriteria.SelectedItem.Text.Split('-')[2] + "'  and c.subject_no = '" + ddlCriteria.SelectedItem.Value.Split('-')[1] + "' ");

                    #region PreAllocation Section
                    DataTable dtPreAllocation = new DataTable();
                    dtPreAllocation.Columns.Add("subject_type");
                    dtPreAllocation.Columns.Add("subType_no");
                    dtPreAllocation.Columns.Add("subject_name");
                    dtPreAllocation.Columns.Add("subject_code");
                    dtPreAllocation.Columns.Add("subject_no");
                    dtPreAllocation.Columns.Add("Lab");
                    dtPreAllocation.Columns.Add("ElectivePap");
                    dtPreAllocation.Columns.Add("Faculty");
                    dtPreAllocation.Columns.Add("Days");
                    dtPreAllocation.Columns.Add("Hours");

                    foreach (GridViewRow gRow in gridUserOptions.Rows)
                    {
                        Label lblSubType = (Label)gRow.FindControl("lblSubType");
                        Label lblSubTypeNo = (Label)gRow.FindControl("lblSubTypeNo");
                        Label lblSubName = (Label)gRow.FindControl("lblSubName");
                        Label lblSubCode = (Label)gRow.FindControl("lblSubCode");
                        Label lblSubNo = (Label)gRow.FindControl("lblSubNo");
                        Label lblIsLab = (Label)gRow.FindControl("lblIsLab");
                        Label lblIsElective = (Label)gRow.FindControl("lblIsElective");

                        DropDownList ddlFaculty = (DropDownList)gRow.FindControl("ddlFaculty");

                        DropDownList ddlMondayHour = (DropDownList)gRow.FindControl("ddlMondayHour");
                        DropDownList ddlTuesdayHour = (DropDownList)gRow.FindControl("ddlTuesdayHour");
                        DropDownList ddlWednesdayHour = (DropDownList)gRow.FindControl("ddlWednesdayHour");
                        DropDownList ddlThursdayHour = (DropDownList)gRow.FindControl("ddlThursdayHour");
                        DropDownList ddlFridayHour = (DropDownList)gRow.FindControl("ddlFridayHour");

                        if (ddlMondayHour.SelectedIndex > 0)
                        {
                            DataRow drNewRow = dtPreAllocation.NewRow();

                            drNewRow["subject_type"] = lblSubType.Text;
                            drNewRow["subType_no"] = lblSubTypeNo.Text;
                            drNewRow["subject_name"] = lblSubName.Text;
                            drNewRow["subject_code"] = lblSubCode.Text;
                            drNewRow["subject_no"] = lblSubNo.Text;
                            drNewRow["Lab"] = lblIsLab.Text.ToUpper();
                            drNewRow["ElectivePap"] = lblIsElective.Text.ToUpper();
                            drNewRow["Faculty"] = ddlFaculty.SelectedValue;

                            drNewRow["Days"] = 0;
                            drNewRow["Hours"] = ddlMondayHour.SelectedItem.Text;
                            dtPreAllocation.Rows.Add(drNewRow);
                        }

                        if (ddlTuesdayHour.SelectedIndex > 0)
                        {

                            DataRow drNewRow = dtPreAllocation.NewRow();

                            drNewRow["subject_type"] = lblSubType.Text;
                            drNewRow["subType_no"] = lblSubTypeNo.Text;
                            drNewRow["subject_name"] = lblSubName.Text;
                            drNewRow["subject_code"] = lblSubCode.Text;
                            drNewRow["subject_no"] = lblSubNo.Text;
                            drNewRow["Lab"] = lblIsLab.Text;
                            drNewRow["ElectivePap"] = lblIsElective.Text;
                            drNewRow["Faculty"] = ddlFaculty.SelectedValue;

                            drNewRow["Days"] = 1;
                            drNewRow["Hours"] = ddlTuesdayHour.SelectedItem.Text;
                            dtPreAllocation.Rows.Add(drNewRow);

                        }

                        if (ddlWednesdayHour.SelectedIndex > 0)
                        {
                            DataRow drNewRow = dtPreAllocation.NewRow();

                            drNewRow["subject_type"] = lblSubType.Text;
                            drNewRow["subType_no"] = lblSubTypeNo.Text;
                            drNewRow["subject_name"] = lblSubName.Text;
                            drNewRow["subject_code"] = lblSubCode.Text;
                            drNewRow["subject_no"] = lblSubNo.Text;
                            drNewRow["Lab"] = lblIsLab.Text;
                            drNewRow["ElectivePap"] = lblIsElective.Text;
                            drNewRow["Faculty"] = ddlFaculty.SelectedValue;

                            drNewRow["Days"] = 2;
                            drNewRow["Hours"] = ddlWednesdayHour.SelectedItem.Text;
                            dtPreAllocation.Rows.Add(drNewRow);
                        }

                        if (ddlThursdayHour.SelectedIndex > 0)
                        {

                            DataRow drNewRow = dtPreAllocation.NewRow();

                            drNewRow["subject_type"] = lblSubType.Text;
                            drNewRow["subType_no"] = lblSubTypeNo.Text;
                            drNewRow["subject_name"] = lblSubName.Text;
                            drNewRow["subject_code"] = lblSubCode.Text;
                            drNewRow["subject_no"] = lblSubNo.Text;
                            drNewRow["Lab"] = lblIsLab.Text;
                            drNewRow["ElectivePap"] = lblIsElective.Text;
                            drNewRow["Faculty"] = ddlFaculty.SelectedValue;

                            drNewRow["Days"] = 3;
                            drNewRow["Hours"] = ddlThursdayHour.SelectedItem.Text;
                            dtPreAllocation.Rows.Add(drNewRow);

                        }

                        if (ddlFridayHour.SelectedIndex > 0)
                        {

                            DataRow drNewRow = dtPreAllocation.NewRow();

                            drNewRow["subject_type"] = lblSubType.Text;
                            drNewRow["subType_no"] = lblSubTypeNo.Text;
                            drNewRow["subject_name"] = lblSubName.Text;
                            drNewRow["subject_code"] = lblSubCode.Text;
                            drNewRow["subject_no"] = lblSubNo.Text;
                            drNewRow["Lab"] = lblIsLab.Text;
                            drNewRow["ElectivePap"] = lblIsElective.Text;
                            drNewRow["Faculty"] = ddlFaculty.SelectedValue;

                            drNewRow["Days"] = 4;
                            drNewRow["Hours"] = ddlFridayHour.SelectedItem.Text;
                            dtPreAllocation.Rows.Add(drNewRow);

                        }
                    }
                    #endregion

                    int maxNoCanAllot = 0;
                    DataTable dtFacultyChoices = getFacultyChoices(batchYear, degreeCode, currentSem, ref  dtSubjectDet, ref  dtSubjectDetWt, ref maxNoCanAllot, ref noOfHrsPerDay, ref  dtBellSchedule, ref dtStaffDet);
                    if (noOfHrsPerDay > 0 && dtBellSchedule.Rows.Count > 0 && dicRoomAvailability.Count > 0)
                    {
                        Hashtable hashElectivesubject = new Hashtable();//For Avoiding same day Elective
                        Hashtable hashElectivesubjectHr = new Hashtable();//For Avoiding same hour Elective
                        Hashtable hashLabsubject = new Hashtable();//For Avoiding same day Lab 
                        Hashtable hashLabsubjectHr = new Hashtable();//For Avoiding same hour Lab
                        Hashtable hashsubject = new Hashtable();//For Avoiding same day Theory
                        Hashtable hashsubjectHr = new Hashtable();//For Avoiding same hour Theory

                        if (dtPreAllocation.Rows.Count > 0)
                        {
                            dtTimeTable = getTimeTableFormatPreAlloted(batchYear, degreeCode, currentSem, (dispText + "-" + (0 + 1)), dtSubjectDet, dtSubjectDetWt, dtFacultyChoices, 0, maxNoCanAllot, noOfHrsPerDay, dtBellSchedule, dtCriteria, ref dtStaffDet, hashElectivesubject, hashElectivesubjectHr, hashLabsubject, hashLabsubjectHr, hashsubject, hashsubjectHr, dicRoomAvailability, arrlstRoomDet, arrlstLabDet, dtPreAllocation);
                            if (dtTimeTable.Rows.Count > 0)
                            {
                                dsTimeTable.Tables.Add(dtTimeTable);
                            }
                        }
                        //Call Sequence I
                        for (int facChoiceI = 0; facChoiceI < dtFacultyChoices.Rows.Count; facChoiceI++)
                        {
                            dtTimeTable = getTimeTableFormat(batchYear, degreeCode, currentSem, (dispText + "-" + (facChoiceI + 1)), dtSubjectDet, dtSubjectDetWt, dtFacultyChoices, facChoiceI, maxNoCanAllot, noOfHrsPerDay, dtBellSchedule, dtCriteria, ref dtStaffDet, hashElectivesubject, hashElectivesubjectHr, hashLabsubject, hashLabsubjectHr, hashsubject, hashsubjectHr, dicRoomAvailability, arrlstRoomDet, arrlstLabDet);
                            if (dtTimeTable.Rows.Count > 0)
                            {
                                dsTimeTable.Tables.Add(dtTimeTable);
                            }
                        }

                        ddlSelectedTimeTable.Items.Clear();
                        for (int tblI = 0; tblI < dsTimeTable.Tables.Count; tblI++)
                        {
                            string tblName = dsTimeTable.Tables[tblI].TableName.Replace("Table", dispText + "-" + ddlCriteria.SelectedItem.Text.Split('-')[0] + "-");
                            dsTimeTable.Tables[tblI].TableName = tblName;
                            ddlSelectedTimeTable.Items.Add(tblName);
                        }
                    }
                }
            }
            if (dsTimeTable.Tables.Count > 0)
            {
                Session["prevDataSet"] = dsTimeTable;
                tblHeaderNextTT.Visible = true;
            }
            else
            {
                tblHeaderNextTT.Visible = true;
            }
            #endregion
            #region Display generated Tables
            //Adding Colors
            ArrayList arrSubName = new ArrayList();

            List<string> lstCellValues = new List<string>();
            lstCellValues.Add("monday");
            lstCellValues.Add("tuesday");
            lstCellValues.Add("wednesday");
            lstCellValues.Add("thursday");
            lstCellValues.Add("friday");
            lstCellValues.Add("");

            //Building an HTML string.
            StringBuilder html = new StringBuilder();

            for (int ttI = 0; ttI < dsTimeTable.Tables.Count; ttI++)
            {
                html.Append("<center><span style='color: Green; font-size:medium;'>" + dsTimeTable.Tables[ttI].TableName + "</span></center><br/>");
                //Table start.
                html.Append("<table cellpadding='0' cellspacing='0' style=' border:1px solid black; border-radius:5px; text-align:center; width:920px; font-size:10px;'>");
                int cnt = 1;
                //Building the Last row.
                html.Append("<tr  style='background-color:#3B6D93;color:#FFFFFF; font-size:12px;'>");
                foreach (DataColumn column in dsTimeTable.Tables[ttI].Columns)
                {
                    html.Append("<td>");
                    html.Append(dsTimeTable.Tables[ttI].Rows[dsTimeTable.Tables[ttI].Rows.Count - 1][column.ColumnName]);
                    html.Append("</td>");
                }
                html.Append("</tr>");
                //Building the Header row.
                html.Append("<tr style='background-color:#3B6D93;color:#FFFFFF; font-size:12px;'>");
                foreach (DataColumn column in dsTimeTable.Tables[ttI].Columns)
                {
                    html.Append("<td>");
                    html.Append(column.ColumnName);
                    html.Append("</td>");
                }
                html.Append("</tr>");

                //Building the Data rows.
                foreach (DataRow row in dsTimeTable.Tables[ttI].Rows)
                {
                    if (cnt == dsTimeTable.Tables[ttI].Rows.Count)
                    {
                        continue;
                    }
                    cnt++;
                    html.Append("<tr>");
                    foreach (DataColumn column in dsTimeTable.Tables[ttI].Columns)
                    {
                        string slotValue = row[column.ColumnName].ToString().Trim();

                        if (!lstCellValues.Contains(slotValue.ToLower()))
                        {
                            if (!arrSubName.Contains(slotValue.Split('-')[0]))
                                arrSubName.Add(slotValue.Split('-')[0]);
                            int index = arrSubName.IndexOf(slotValue.Split('-')[0]);
                            string bgcolor = getColor(index);
                            html.Append("<td style='background-color:" + bgcolor + "'>");
                        }
                        else
                        {
                            if (string.IsNullOrEmpty(slotValue))
                            {
                                html.Append("<td style='background-color:#FFFFFF;'>");
                            }
                            else
                            {
                                html.Append("<td style='background-color:#3B6D93;color:#FFFFFF; font-size:12px;'>");
                            }
                        }
                        html.Append(slotValue);
                        html.Append("</td>");

                    }
                    html.Append("</tr>");
                }
                //Table end.
                html.Append("</table><br>");
            }
            //Append the HTML string to Placeholder.
            divTimeTableOutput.Visible = true;
            phTimeTable.Controls.Add(new Literal { Text = html.ToString() });

            #endregion
        }
        catch { ScriptManager.RegisterStartupScript(this, typeof(Page), UniqueID, "alert('Please Try Later!')", true); }
    }
    protected void btnGenerateNext_Click(object sender, EventArgs e)
    {
        try
        {
            if (ddlStaffTT.Items.Count > 0)
                ddlStaffTT.SelectedIndex = 0;

            if (Session["FromSaved"] != null)
            {
                saveAndGenerate(1);
                return;
            }

            DataSet dsNewTimeTables = new DataSet();

            if (Session["selectedDataSet"] != null)
            {
                dsNewTimeTables = (DataSet)Session["selectedDataSet"];
            }
            string newName = (dsNewTimeTables.Tables.Count + 1).ToString();
            if (Session["prevDataSet"] != null)
            {
                DataSet dsTimeTables = (DataSet)Session["prevDataSet"];
                string prevTableName = ddlSelectedTimeTable.Items.Count > 0 ? ddlSelectedTimeTable.SelectedItem.Text.Trim() : "---";
                if (dsTimeTables.Tables.Contains(prevTableName) || prevTableName == "---")
                {
                    if (prevTableName != "---")
                    {
                        DataTable dtTimeTableSelected = dsTimeTables.Tables[prevTableName].Copy();
                        dtTimeTableSelected.TableName = dtTimeTableSelected.TableName + "-" + newName;
                        dsNewTimeTables.Tables.Add(dtTimeTableSelected);
                        Session["selectedDataSet"] = dsNewTimeTables;
                    }

                    //Modified Generate Button Click Event for Next Options
                    #region Modified Generate Button Click Event
                    DataSet dsTimeTable = new DataSet();
                    #region Generate Time Table for every selected branches
                    ArrayList arrLstDegBatch = getDegreeArrayList();
                    foreach (string batchDeg in arrLstDegBatch)
                    {
                        string[] arrBatchDeg = batchDeg.Split('$');
                        if (arrBatchDeg.Length == 3)
                        {
                            int batchYear = Convert.ToInt32(arrBatchDeg[0]);
                            int degreeCode = Convert.ToInt32(arrBatchDeg[1]);
                            string dispText = Convert.ToString(arrBatchDeg[2]);

                            int currentSem = dirAccess.selectScalarInt("select distinct r.Current_Semester from Registration r where  r.degree_code ='" + degreeCode + "' and r.Batch_Year='" + batchYear + "' and r.college_code='" + collegecode + "'  and CC='0' and DelFlag='0' and Exam_Flag<>'DEBAR'");

                            DataTable dtTimeTable = new DataTable();
                            //Room Avalability Detail
                            ArrayList arrlstRoomDet = new ArrayList();
                            ArrayList arrlstLabDet = new ArrayList();
                            Dictionary<string, int> dicRoomAvailability = getRoomAvailability(batchYear, degreeCode, currentSem, ref arrlstRoomDet, ref arrlstLabDet, dsNewTimeTables.Tables.Count);//, dsNewTimeTables.Tables.Count

                            int noOfHrsPerDay = 0;
                            DataTable dtBellSchedule = new DataTable();
                            DataTable dtSubjectDet = new DataTable();
                            DataTable dtSubjectDetWt = new DataTable();
                            DataTable dtStaffDet = new DataTable();

                            DataTable dtCriteria = new DataTable();
                            if (ddlCriteriaReduced.Items.Count > 0)
                            {
                                dtCriteria = dirAccess.selectDataTable("select distinct subject_code+'-'+c.staff_code as criterianame,DayPk,HourPk,IsEngaged from TT_StudentCriteria c,subject s where c.subject_no=s.subject_no and c.semester='" + currentSem + "' and c.degree_code='" + degreeCode + "' and c.batch_year='" + batchYear + "'  and c.criterianame='" + ddlCriteriaReduced.SelectedItem.Text.Split('-')[0] + "'  and c.staff_code='" + ddlCriteriaReduced.SelectedItem.Text.Split('-')[2] + "' and c.subject_no='" + ddlCriteriaReduced.SelectedItem.Value.Split('-')[1] + "'");
                                ddlCriteriaReduced.Items.Remove(ddlCriteriaReduced.SelectedItem);

                                if (ddlCriteriaReduced.Items.Count == 0)
                                {
                                    for (int i = 0; i < ddlCriteria.Items.Count; i++)
                                    {
                                        //if (ddlCriteria.SelectedIndex != i)
                                        //{
                                        ListItem item = new ListItem(ddlCriteria.Items[i].Text, ddlCriteria.Items[i].Value);
                                        ddlCriteriaReduced.Items.Add(item);
                                        //}
                                    }
                                }
                            }
                            else
                            {
                                dtCriteria = dirAccess.selectDataTable("select distinct criterianame+'-'+subject_code+'-'+c.staff_code as criterianame from TT_StudentCriteria c,subject s where c.subject_no=s.subject_no and c.semester='" + currentSem + "' and c.degree_code='" + degreeCode + "' and c.batch_year='" + batchYear + "' ");

                                //dtCriteria = dirAccess.selectDataTable("select criterianame ,DayPk,HourPk,IsEngaged from TT_StudentCriteria  ");
                            }
                            //else
                            //{
                            //    byte criteriaNum = getRandomCriteria(ddlCriteria.Items.Count);
                            //    dtCriteria = dirAccess.selectDataTable("select criterianame ,DayPk,HourPk,IsEngaged from TT_StudentCriteria where criterianame='" + ddlCriteria.Items[criteriaNum].Text + "' ");

                            //}

                            int maxNoCanAllot = 0;
                            DataTable dtFacultyChoices = getFacultyChoices(batchYear, degreeCode, currentSem, ref  dtSubjectDet, ref  dtSubjectDetWt, ref maxNoCanAllot, ref noOfHrsPerDay, ref  dtBellSchedule, ref dtStaffDet);

                            DataTable dtFacultyChoicesTemp = dtFacultyChoices.Copy();
                            if (noOfHrsPerDay > 0 && dtBellSchedule.Rows.Count > 0 && dicRoomAvailability.Count > 0)
                            {
                                #region Engage StaffAvailability, Lab & Elective Allotment Conditions and Room
                                ArrayList arrAlreadyAddedRowCol = new ArrayList();
                                //ArrayList arrAlreadyAddedCol = new ArrayList();//To Check whether already added in the row & column

                                Hashtable htElectPreAllot = new Hashtable();
                                Hashtable htElectRooms = new Hashtable();

                                DataTable dtCurStaffDet = dtStaffDet.Copy();
                                dtCurStaffDet.Clear();

                                for (int ttI = 0; ttI < dsNewTimeTables.Tables.Count; ttI++)
                                {
                                    DataTable dtcurrentTable = dsNewTimeTables.Tables[ttI];

                                    for (int dayI = 0; dayI < (dtcurrentTable.Rows.Count - 1); dayI++)
                                    {
                                        for (int hrsI = 1; hrsI < dtcurrentTable.Columns.Count; hrsI++)
                                        {
                                            string colName = dtcurrentTable.Columns[hrsI].ColumnName.ToString().Trim();
                                            byte realHrs = 0;
                                            if (byte.TryParse(colName, out realHrs))
                                            {
                                                string cellValue = Convert.ToString(dtcurrentTable.Rows[dayI][realHrs.ToString()]);
                                                string[] cellValues = cellValue.Split('$');//with room

                                                if (cellValues[0] != string.Empty && !cellValues[0].Contains(","))
                                                {
                                                    string[] resultValues = cellValues[0].Split('-');//subject code-staffcode
                                                    if (resultValues.Length > 1)
                                                    {
                                                        string[] subcodes = resultValues[0].Split('#');
                                                        //Theory and Lab
                                                        if (subcodes.Length == 1)
                                                        {
                                                            string[] staffs = resultValues[1].Split('/');
                                                            foreach (string faculty in staffs)
                                                            {
                                                                dtSubjectDetWt.DefaultView.RowFilter = "subject_code='" + resultValues[0] + "' and staff_code='" + faculty + "'";
                                                                DataView dvCurrStaff = dtSubjectDetWt.DefaultView;
                                                                if (dvCurrStaff.Count > 0)
                                                                {
                                                                    string subject_no = Convert.ToString(dvCurrStaff[0]["subject_no"]);
                                                                    DataRow drNewRow = dtCurStaffDet.NewRow();

                                                                    drNewRow["staff_appno"] = dvCurrStaff[0]["appl_id"];
                                                                    drNewRow["DaysFK"] = (dayI + 1);
                                                                    drNewRow["HoursFK"] = realHrs;
                                                                    drNewRow["degreeCode"] = degreeCode;
                                                                    drNewRow["batch_year"] = batchYear;
                                                                    drNewRow["IsEngaged"] = "True";
                                                                    drNewRow["subject_no"] = subject_no;
                                                                    drNewRow["section"] = "";
                                                                    drNewRow["MaxHour"] = "1";
                                                                    dtCurStaffDet.Rows.Add(drNewRow);

                                                                    string hrI = " #" + cellValues[1] + "$" + (dayI + 1) + "_" + realHrs;
                                                                    dicRoomAvailability[hrI] = 1;

                                                                    //Lab Allocated from previous Time table for non repetition
                                                                    if (staffs.Length == 2)
                                                                    {
                                                                        if (!arrAlreadyAddedRowCol.Contains(subject_no + "$" + cellValues[1] + "_" + dayI + "_" + realHrs))
                                                                        {
                                                                            arrAlreadyAddedRowCol.Add(subject_no + "$" + cellValues[1] + "_" + dayI + "_" + realHrs);
                                                                        }
                                                                        string staffcheck = String.Format(staffs[0] + "'" + "," + "'" + staffs[1]); //Remove already added Staff 
                                                                        dtFacultyChoices.DefaultView.RowFilter = "[" + subject_no + "] <>'" + staffcheck.Replace("'", "''") + "'";
                                                                        DataView dvNew = dtFacultyChoices.DefaultView;
                                                                        //dtFacultyChoices.Clear();
                                                                        dtFacultyChoices = dvNew.ToTable();

                                                                        dtCurStaffDet.Rows[dtCurStaffDet.Rows.Count - 1]["MaxHour"] = "2";
                                                                    }
                                                                    else//Remove already added Staff 
                                                                    {
                                                                        dtFacultyChoices.DefaultView.RowFilter = "[" + subject_no + "] <>'" + faculty + "'";
                                                                        DataView dvNew = dtFacultyChoices.DefaultView;
                                                                        //dtFacultyChoices.Clear();
                                                                        dtFacultyChoices = dvNew.ToTable();
                                                                    }
                                                                    //Lab Allocation Ends
                                                                }
                                                                else
                                                                {
                                                                    if (cellValues.Length > 1)
                                                                    {
                                                                        string hrI = " #" + cellValues[1] + "$" + (dayI + 1) + "_" + realHrs;
                                                                        dicRoomAvailability[hrI] = 1;
                                                                    }
                                                                }
                                                            }
                                                        }
                                                        else
                                                        {   // Combined Lab
                                                            string[] staffs = resultValues[1].Split('/');

                                                            if (staffs.Length > 1)
                                                            {

                                                                dtSubjectDetWt.DefaultView.RowFilter = "subject_code in ('" + subcodes[0] + "') and staff_code in ('" + staffs[0] + "')";
                                                                DataView dvCurrStaff = dtSubjectDetWt.DefaultView;

                                                                string subject_no = Convert.ToString(dvCurrStaff[0]
["subject_no"]);

                                                                dtSubjectDetWt.DefaultView.RowFilter = "subject_code in ('" + subcodes[1] + "') and staff_code in ('" + staffs[1] + "')";
                                                                DataView dvCurrStaff1 = dtSubjectDetWt.DefaultView;

                                                                string subject_no2 = Convert.ToString(dvCurrStaff1[0]
["subject_no"]);
                                                                DataRow drNewRow = dtCurStaffDet.NewRow();

                                                                drNewRow["staff_appno"] = dvCurrStaff[0]["appl_id"];
                                                                drNewRow["DaysFK"] = (dayI + 1);
                                                                drNewRow["HoursFK"] = realHrs;
                                                                drNewRow["degreeCode"] = degreeCode;
                                                                drNewRow["batch_year"] = batchYear;
                                                                drNewRow["IsEngaged"] = "True";
                                                                drNewRow["subject_no"] = subject_no;
                                                                drNewRow["section"] = "";
                                                                drNewRow["MaxHour"] = "2";
                                                                dtCurStaffDet.Rows.Add(drNewRow);


                                                                DataRow drNewRow1 = dtCurStaffDet.NewRow();

                                                                drNewRow1["staff_appno"] = dvCurrStaff1[0]["appl_id"];
                                                                drNewRow1["DaysFK"] = (dayI + 1);
                                                                drNewRow1["HoursFK"] = realHrs;
                                                                drNewRow1["degreeCode"] = degreeCode;
                                                                drNewRow1["batch_year"] = batchYear;
                                                                drNewRow1["IsEngaged"] = "True";
                                                                drNewRow1["subject_no"] = subject_no2;
                                                                drNewRow1["section"] = "";
                                                                drNewRow["MaxHour"] = "2";
                                                                dtCurStaffDet.Rows.Add(drNewRow1);


                                                                string hrI = " #" + cellValues[1] + "$" + (dayI + 1) + "_" + realHrs;
                                                                dicRoomAvailability[hrI] = 1;

                                                                //Lab Allocated from previous Time table for non repetition
                                                                if (staffs.Length == 2)
                                                                {
                                                                    if (!arrAlreadyAddedRowCol.Contains(subject_no + "$" + cellValues[1] + "_" + dayI + "_" + realHrs))
                                                                    {
                                                                        arrAlreadyAddedRowCol.Add(subject_no + "$" + cellValues[1] + "_" + dayI + "_" + realHrs);
                                                                    }
                                                                    if (!arrAlreadyAddedRowCol.Contains(subject_no2 + "$" + cellValues[1] + "_" + dayI + "_" + realHrs))
                                                                    {
                                                                        arrAlreadyAddedRowCol.Add(subject_no2 + "$" + cellValues[1] + "_" + dayI + "_" + realHrs);
                                                                    }
                                                                    string staffcheck = String.Format(staffs[0] + "'" + "," + "'" + staffs[1]); //Remove already added Staff 
                                                                    dtFacultyChoices.DefaultView.RowFilter = "[" + subject_no + "-" + subject_no2 + "] <>'" + staffcheck.Replace("'", "''") + "'";
                                                                    DataView dvNew = dtFacultyChoices.DefaultView;
                                                                    //dtFacultyChoices.Clear();
                                                                    dtFacultyChoices = dvNew.ToTable();
                                                                }
                                                                else//Remove already added Staff 
                                                                {
                                                                    dtFacultyChoices.DefaultView.RowFilter = "[" + subject_no + "-" + subject_no2 + "] <>'" + staffs[0] + "' or [" + subject_no + "-" + subject_no2 + "] <>'" + staffs[1] + "'";
                                                                    DataView dvNew = dtFacultyChoices.DefaultView;
                                                                    //dtFacultyChoices.Clear();
                                                                    dtFacultyChoices = dvNew.ToTable();
                                                                }
                                                                //Lab Allocation Ends
                                                            }
                                                            else
                                                            {
                                                                if (cellValues.Length > 1)
                                                                {
                                                                    string hrI = " #" + cellValues[1] + "$" + (dayI + 1) + "_" + realHrs;
                                                                    dicRoomAvailability[hrI] = 1;
                                                                }
                                                            }
                                                        }
                                                    }
                                                }
                                                else if (cellValues[0] != string.Empty && cellValues[0].Contains(","))
                                                {
                                                    //For Elective
                                                    string[] electPapers = cellValues[0].Split(',');
                                                    foreach (string electPap in electPapers)
                                                    {
                                                        string[] resultValues = electPap.Split('-');//subject code-staffcode
                                                        if (resultValues.Length > 1)
                                                        {
                                                            string subType_noFnl = string.Empty;
                                                            string[] staffs = resultValues[1].Split('/');
                                                            foreach (string faculty in staffs)
                                                            {
                                                                dtSubjectDetWt.DefaultView.RowFilter = "subject_code='" + resultValues[0] + "' and staff_code='" + faculty + "'";
                                                                DataView dvCurrStaff = dtSubjectDetWt.DefaultView;
                                                                if (dvCurrStaff.Count > 0)
                                                                {
                                                                    string subject_no = Convert.ToString(dvCurrStaff[0]["subject_no"]);
                                                                    string subType_no = Convert.ToString(dvCurrStaff[0]["subType_no"]);
                                                                    DataRow drNewRow = dtCurStaffDet.NewRow();

                                                                    drNewRow["staff_appno"] = dvCurrStaff[0]["appl_id"];
                                                                    drNewRow["DaysFK"] = (dayI + 1);
                                                                    drNewRow["HoursFK"] = realHrs;
                                                                    drNewRow["degreeCode"] = degreeCode;
                                                                    drNewRow["batch_year"] = batchYear;
                                                                    drNewRow["IsEngaged"] = "True";
                                                                    drNewRow["subject_no"] = subject_no;
                                                                    drNewRow["section"] = "";
                                                                    drNewRow["MaxHour"] = "1";
                                                                    dtCurStaffDet.Rows.Add(drNewRow);

                                                                    subType_noFnl = subType_no;
                                                                }
                                                            }

                                                            string[] rooms = cellValues[1].Split(',');
                                                            foreach (string room in rooms)
                                                            {
                                                                string hrI = " #" + room + "$" + (dayI + 1) + "_" + realHrs;
                                                                dicRoomAvailability[hrI] = 1;
                                                            }
                                                            if (!htElectRooms.Contains(subType_noFnl))
                                                                htElectRooms.Add(subType_noFnl, cellValues[1]);

                                                            string subjecthrs = subType_noFnl + "_" + dayI + "_" + realHrs;

                                                            if (!htElectPreAllot.Contains(subjecthrs))
                                                            {
                                                                htElectPreAllot.Add(subjecthrs, "1");
                                                            }
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                                dtStaffDet.Merge(dtCurStaffDet);

                                if (dtFacultyChoices.Rows.Count == 0)
                                    dtFacultyChoices = dtFacultyChoicesTemp;
                                #endregion

                                //Call Sequence I
                                for (int facChoiceI = 0; facChoiceI < dtFacultyChoices.Rows.Count; facChoiceI++)
                                {
                                    if (dsNewTimeTables.Tables.Count < 5)
                                    {
                                        dtTimeTable = getTimeTableFormatRegenerate(batchYear, degreeCode, currentSem, (dispText + "-" + (facChoiceI + 1)), dtSubjectDet, dtSubjectDetWt, dtFacultyChoices, facChoiceI, maxNoCanAllot, noOfHrsPerDay, dtBellSchedule, dtCriteria, ref dtStaffDet, dicRoomAvailability, arrlstRoomDet, arrlstLabDet, arrAlreadyAddedRowCol, htElectPreAllot, htElectRooms);
                                        if (dtTimeTable.Rows.Count > 0)
                                        {
                                            dsTimeTable.Tables.Add(dtTimeTable);
                                        }
                                    }
                                    else
                                    {
                                        dtTimeTable = getTimeTableFormatRegenerateUnfill(batchYear, degreeCode, currentSem, (dispText + "-" + (facChoiceI + 1)), dtSubjectDet, dtSubjectDetWt, dtFacultyChoices, facChoiceI, maxNoCanAllot, noOfHrsPerDay, dtBellSchedule, dtCriteria, ref dtStaffDet, dicRoomAvailability, arrlstRoomDet, arrlstLabDet, arrAlreadyAddedRowCol, htElectPreAllot, htElectRooms);
                                        if (dtTimeTable.Rows.Count > 0)
                                        {
                                            dsTimeTable.Tables.Add(dtTimeTable);
                                        }
                                    }
                                }

                                ddlSelectedTimeTable.Items.Clear();
                                for (int tblI = 0; tblI < dsTimeTable.Tables.Count; tblI++)
                                {
                                    string tblName = dsTimeTable.Tables[tblI].TableName.Replace("Table", dispText + "-" + ddlCriteriaReduced.SelectedItem.Text.Split('-')[0] + "-");
                                    dsTimeTable.Tables[tblI].TableName = tblName;
                                    ddlSelectedTimeTable.Items.Add(tblName);
                                }
                            }
                        }
                    }
                    if (dsTimeTable.Tables.Count > 0)
                    {
                        Session["prevDataSet"] = dsTimeTable;
                        tblHeaderNextTT.Visible = true;
                    }
                    else
                    {
                        tblHeaderNextTT.Visible = true;
                    }
                    #endregion
                    #region Display generated Tables
                    //Adding Colors
                    ArrayList arrSubName = new ArrayList();

                    List<string> lstCellValues = new List<string>();
                    lstCellValues.Add("monday");
                    lstCellValues.Add("tuesday");
                    lstCellValues.Add("wednesday");
                    lstCellValues.Add("thursday");
                    lstCellValues.Add("friday");
                    lstCellValues.Add("");

                    //Building an HTML string.
                    StringBuilder html = new StringBuilder();
                    for (int ttI = 0; ttI < dsTimeTable.Tables.Count; ttI++)
                    {
                        html.Append("<center><span style='color: Green; font-size:medium;'>" + dsTimeTable.Tables[ttI].TableName + "</span></center><br/>");
                        //Table start.
                        html.Append("<table cellpadding='0' cellspacing='0' style=' border:1px solid black; border-radius:5px; text-align:center; width:920px; font-size:10px;'>");
                        int cnt = 1;
                        //Building the Last row.
                        html.Append("<tr  style='background-color:#3B6D93;color:#FFFFFF; font-size:12px;'>");
                        foreach (DataColumn column in dsTimeTable.Tables[ttI].Columns)
                        {
                            html.Append("<td>");
                            html.Append(dsTimeTable.Tables[ttI].Rows[dsTimeTable.Tables[ttI].Rows.Count - 1][column.ColumnName]);
                            html.Append("</td>");
                        }
                        html.Append("</tr>");
                        //Building the Header row.
                        html.Append("<tr style='background-color:#3B6D93;color:#FFFFFF; font-size:12px;'>");
                        foreach (DataColumn column in dsTimeTable.Tables[ttI].Columns)
                        {
                            html.Append("<td>");
                            html.Append(column.ColumnName);
                            html.Append("</td>");
                        }
                        html.Append("</tr>");

                        //Building the Data rows.
                        foreach (DataRow row in dsTimeTable.Tables[ttI].Rows)
                        {
                            if (cnt == dsTimeTable.Tables[ttI].Rows.Count)
                            {
                                continue;
                            }
                            cnt++;
                            html.Append("<tr>");
                            foreach (DataColumn column in dsTimeTable.Tables[ttI].Columns)
                            {
                                string slotValue = row[column.ColumnName].ToString().Trim();
                                if (!lstCellValues.Contains(slotValue.ToLower()))
                                {
                                    if (!arrSubName.Contains(slotValue.Split('-')[0]))
                                        arrSubName.Add(slotValue.Split('-')[0]);
                                    int index = arrSubName.IndexOf(slotValue.Split('-')[0]);
                                    string bgcolor = getColor(index);
                                    html.Append("<td style='background-color:" + bgcolor + "'>");
                                }
                                else
                                {
                                    if (string.IsNullOrEmpty(slotValue))
                                    {
                                        html.Append("<td style='background-color:#FFFFFF;'>");
                                    }
                                    else
                                    {
                                        html.Append("<td style='background-color:#3B6D93;color:#FFFFFF; font-size:12px;'>");
                                    }
                                }
                                html.Append(slotValue);
                                html.Append("</td>");
                            }
                            html.Append("</tr>");
                        }
                        //Table end.
                        html.Append("</table><br>");
                    }
                    //Append the HTML string to Placeholder.
                    divTimeTableOutput.Visible = true;
                    phTimeTable.Controls.Add(new Literal { Text = html.ToString() });

                    #endregion
                    #endregion
                }
            }
        }
        catch { }
    }
    private ArrayList getDegreeArrayList()
    {
        ArrayList arrLstDegBatch = new ArrayList();
        foreach (GridViewRow gRow in gridDetails.Rows)
        {
            CheckBox cb_select = (CheckBox)gRow.FindControl("cb_select");
            if (cb_select.Checked)
            {
                Label lbl_Batch = (Label)gRow.FindControl("lbl_Batch");
                Label lbl_BranchCode = (Label)gRow.FindControl("lbl_BranchCode");
                Label lbl_Degree = (Label)gRow.FindControl("lbl_Degree");
                Label lbl_Branch = (Label)gRow.FindControl("lbl_Branch");
                arrLstDegBatch.Add(lbl_Batch.Text.Trim() + "$" + lbl_BranchCode.Text.Trim() + "$" + lbl_Batch.Text.Trim() + "-" + lbl_Degree.Text.Trim() + "-" + lbl_Branch.Text.Trim());
            }
        }
        return arrLstDegBatch;
    }
    //Pre Alloted Fill
    private DataTable getTimeTableFormatPreAlloted(int batchYear, int degreeCode, int currentSem, string dispText, DataTable dtSubjectDet, DataTable dtSubjectDetWt, DataTable dtFacultyChoices, int facChoiceIndex, int maxNoCanAllot, int noOfHrsPerDay, DataTable dtBellSchedule, DataTable dtCriteria, ref DataTable dtStaffDet, Hashtable hashElectivesubject, Hashtable hashElectivesubjectHr, Hashtable hashLabsubject, Hashtable hashLabsubjectHr, Hashtable hashsubject, Hashtable hashsubjectHr, Dictionary<string, int> dicRoomAvailability, ArrayList arrlstRoomDet, ArrayList arrlstLabDet, DataTable dtPreAllocation)
    {
        DataTable dtTimeTable = new DataTable();
        try
        {
            //Total No of Hours to Allot for a week
            int NoOfHrsToAllot = maxNoCanAllot;
            NoOfHrsToAllot -= 5;

            DataTable dtCurStaffDet = dtStaffDet.Copy();
            dtCurStaffDet.Clear();
            Dictionary<string, int> dicCurRoomAvail = new Dictionary<string, int>();
            foreach (KeyValuePair<string, int> roomVal in dicRoomAvailability)
            {
                dicCurRoomAvail.Add(roomVal.Key, roomVal.Value);
            }

            DataTable dtFnlTable = new DataTable();
            dtFnlTable.Columns.Add("Day/Period");

            dtFnlTable.Rows.Add("Monday");
            dtFnlTable.Rows.Add("Tuesday");
            dtFnlTable.Rows.Add("Wednesday");
            dtFnlTable.Rows.Add("Thursday");
            dtFnlTable.Rows.Add("Friday");
            dtFnlTable.Rows.Add("");
            for (int periodI = 0; periodI < dtBellSchedule.Rows.Count; periodI++)
            {
                dtFnlTable.Columns.Add(Convert.ToString(dtBellSchedule.Rows[periodI]["Period1"]).Trim());
                dtFnlTable.Rows[5][(periodI + 1)] = Convert.ToString(dtBellSchedule.Rows[periodI]["start_time"]).Trim() + "- " + Convert.ToString(dtBellSchedule.Rows[periodI]["end_time"]).Trim();
            }

            //Students Already Engaged in Options
            //NoOfHrsToAllot += 4;
            for (int optI = 0; optI < dtCriteria.Rows.Count; optI++)
            {
                #region Old 25-02-2017
                //byte DaysFk = Convert.ToByte(dtCriteria.Rows[optI]["DayPk"]);
                //byte HrsFk = Convert.ToByte(dtCriteria.Rows[optI]["HourPk"]);
                //string IsEngaged = Convert.ToString(dtCriteria.Rows[optI]["IsEngaged"]).Trim().ToUpper();
                //dtFnlTable.Rows[(DaysFk - 1)][HrsFk.ToString()] = "ENGAGED";
                ////NoOfHrsToAllot--; 
                #endregion
                byte DaysFk = Convert.ToByte(dtCriteria.Rows[optI]["DayPk"]);
                byte HrsFk = Convert.ToByte(dtCriteria.Rows[optI]["HourPk"]);
                string IsEngaged = Convert.ToString(dtCriteria.Rows[optI]["IsEngaged"]).Trim().ToUpper();


                foreach (string roomPKACR in arrlstRoomDet)
                {
                    string[] roomVal = roomPKACR.Split('#');
                    string hrI = roomPKACR + "$" + (DaysFk) + "_" + HrsFk;


                    bool isRoomFree = false;
                    if (dicCurRoomAvail.ContainsKey(hrI) && dicCurRoomAvail[hrI] == 0)
                    {
                        isRoomFree = true;

                    }
                    if (!isRoomFree)
                        continue;

                    string subject_code = ddlCriteria.SelectedItem.Text;
                    string staff_code = "MT" + subject_code.Split(' ')[1];

                    dtFnlTable.Rows[(DaysFk - 1)][HrsFk.ToString()] = subject_code + "-" + staff_code + "$" + roomVal[1];
                    //NoOfHrsToAllot--;
                    dicCurRoomAvail[hrI] = 1;
                    break;
                }
            }
            Dictionary<string, byte> dicAllocSub = new Dictionary<string, byte>();
            Dictionary<string, byte> dicAllocElecSub = new Dictionary<string, byte>();

            ArrayList arrAlreadyAddedRow = new ArrayList();
            ArrayList arrAlreadyAddedCol = new ArrayList();//To Check whether already added in the row & column


            #region Fill Pre allocated subjects and staffs
            #region Elective Fill

            dtPreAllocation.DefaultView.RowFilter = "ElectivePap='True'";
            DataTable dtElective = dtPreAllocation.DefaultView.ToTable(true, "subType_no", "Days", "Hours");

            for (int preAllocI = 0; preAllocI < dtElective.Rows.Count; preAllocI++)
            {


                string subType_no = Convert.ToString(dtElective.Rows[preAllocI]["subType_no"]);
                byte Days = Convert.ToByte(dtElective.Rows[preAllocI]["Days"]);
                byte Hours = Convert.ToByte(dtElective.Rows[preAllocI]["Hours"]);

                byte allotedHrs = 0;


                StringBuilder sbSubDisp = new StringBuilder();
                StringBuilder sbSubjStaff = new StringBuilder();
                sbSubjStaff.Append("(");

                dtSubjectDet.DefaultView.RowFilter = " subType_no = '" + subType_no + "'";
                DataView dvSubType = dtSubjectDet.DefaultView;
                if (dvSubType.Count > 0)
                {

                    byte noofhrsperweek = Convert.ToByte(dvSubType[0]["noofhrsperweek"]);

                    if (dicAllocElecSub.ContainsKey(subType_no))
                    {
                        allotedHrs = dicAllocElecSub[subType_no];
                        if (allotedHrs >= noofhrsperweek)
                            continue;
                    }
                    else
                    {
                        dicAllocElecSub.Add(subType_no, allotedHrs);
                    }

                    for (int electSubI = 0; electSubI < dvSubType.Count; electSubI++)
                    {
                        string subject_noE = Convert.ToString(dvSubType[electSubI]["subject_no"]).Trim();
                        string subject_codeE = Convert.ToString(dvSubType[electSubI]["subject_code"]);

                        dtPreAllocation.DefaultView.RowFilter = "subject_no='" + subject_noE + "'";
                        DataView dvFac = dtPreAllocation.DefaultView;
                        if (dvFac.Count > 0)
                        {
                            //Loop through staff and availability
                            dtSubjectDetWt.DefaultView.RowFilter = "subject_no='" + subject_noE + "'  and appl_id='" + Convert.ToString(dvFac[0]["Faculty"]) + "'";
                            DataView dvSubjectStaff = dtSubjectDetWt.DefaultView;
                            dvSubjectStaff.Sort = " staffPriority asc";
                            if (dvSubjectStaff.Count > 0)
                            {
                                string staff_code = Convert.ToString(dvSubjectStaff[0]["staff_code"]);
                                string staff_appno = Convert.ToString(dvSubjectStaff[0]["appl_id"]);
                                sbSubDisp.Append(subject_codeE + "-" + staff_code + ",");
                                sbSubjStaff.Append(" staff_appno = '" + staff_appno + "' or ");
                            }
                        }
                    }

                }
                if (sbSubDisp.Length > 1)
                    sbSubDisp.Remove(sbSubDisp.Length - 1, 1);

                if (sbSubjStaff.Length > 3)
                    sbSubjStaff.Remove(sbSubjStaff.Length - 3, 3);
                sbSubjStaff.Append(") and ");

                StringBuilder sbElectRoom = new StringBuilder();
                ArrayList arrElectRoom = new ArrayList();
                foreach (string roomPKACR in arrlstRoomDet)
                {
                    string[] roomVal = roomPKACR.Split('#');
                    string hrI = roomPKACR + "$" + (Days + 1) + "_" + Hours;

                    bool isRoomFree = false;
                    if (dicCurRoomAvail.ContainsKey(hrI) && dicCurRoomAvail[hrI] == 0)
                    {
                        isRoomFree = true;
                    }
                    if (!isRoomFree)
                        continue;

                    arrElectRoom.Add(hrI);
                    sbElectRoom.Append(roomVal[1] + ",");

                    if (arrElectRoom.Count == dvSubType.Count)
                        break;
                }

                if (arrElectRoom.Count == dvSubType.Count)
                {
                    if (sbElectRoom.Length > 1)
                        sbElectRoom.Remove(sbElectRoom.Length - 1, 1);

                    dtFnlTable.Rows[Days][Hours.ToString()] = sbSubDisp.ToString() + "$" + sbElectRoom.ToString();
                    allotedHrs++;
                    NoOfHrsToAllot--;
                    dicAllocElecSub[subType_no]++;
                    arrAlreadyAddedRow.Add(subType_no + "E_" + Days);
                    arrAlreadyAddedCol.Add(subType_no + "E_" + Hours);

                    //Room Engage
                    for (int i = 0; i < arrElectRoom.Count; i++)
                    {
                        string hrI = " #" + arrElectRoom[i] + "$" + (Days + 1) + "_" + Hours;
                        dicCurRoomAvail[hrI] = 1;
                    }

                }
            }

            #endregion

            #region Lab Fill
            dtPreAllocation.DefaultView.RowFilter = "Lab='True'";
            DataTable dtLab = dtPreAllocation.DefaultView.ToTable(true, "subType_no", "Days", "Hours", "Faculty");
            for (int preAllocI = 0; preAllocI < dtLab.Rows.Count; preAllocI++)
            {
                string subType_no = Convert.ToString(dtLab.Rows[preAllocI]["subType_no"]);
                byte Days = Convert.ToByte(dtLab.Rows[preAllocI]["Days"]);
                string[] Hours = Convert.ToString(dtLab.Rows[preAllocI]["Hours"]).Split(',');
                string[] Faculties = Convert.ToString(dtLab.Rows[preAllocI]["Faculty"]).Split('-');
                byte allotedHrs = 0;

                if (Faculties.Length == 2)
                {
                    dtSubjectDet.DefaultView.RowFilter = " subType_no = '" + subType_no + "'";
                    DataView dvSubType = dtSubjectDet.DefaultView;
                    if (dvSubType.Count > 0)
                    {

                        string subject_noL = Convert.ToString(dvSubType[0]["subject_no"]).Trim();
                        string subject_codeL = Convert.ToString(dvSubType[0]["subject_code"]);

                        byte noofhrsperweek = Convert.ToByte(dvSubType[0]["noofhrsperweek"]);



                        if (dicAllocSub.ContainsKey(subject_noL))
                        {
                            allotedHrs = dicAllocSub[subject_noL];
                            if (allotedHrs >= noofhrsperweek)
                                continue;
                        }
                        else
                        {
                            dicAllocSub.Add(subject_noL, allotedHrs);
                        }
                        //Loop through staff and availability
                        dtSubjectDetWt.DefaultView.RowFilter = "subject_no='" + subject_noL + "'  and (appl_id='" + Faculties[0] + "' or appl_id='" + Faculties[1] + "')";
                        DataView dvSubjectStaff = dtSubjectDetWt.DefaultView;
                        dvSubjectStaff.Sort = " staffPriority asc";
                        if (dvSubjectStaff.Count > 1)
                        {
                            string staff_code = Convert.ToString(dvSubjectStaff[0]["staff_code"]);
                            string staff_appno = Convert.ToString(dvSubjectStaff[0]["appl_id"]);

                            string staff_code2 = Convert.ToString(dvSubjectStaff[1]["staff_code"]);
                            string staff_appno2 = Convert.ToString(dvSubjectStaff[1]["appl_id"]);

                            foreach (string roomPKACR in arrlstLabDet)
                            {
                                string[] roomVal = roomPKACR.Split('#');
                                string hrI = roomPKACR + "$" + (Days + 1) + "_" + Hours[0];
                                string hrII = roomPKACR + "$" + (Days + 1) + "_" + Hours[1];

                                bool isRoomFree = false;
                                if (dicCurRoomAvail.ContainsKey(hrI) && dicCurRoomAvail.ContainsKey(hrII))
                                {
                                    if (dicCurRoomAvail[hrI] == 0 && dicCurRoomAvail[hrII] == 0)
                                    {
                                        isRoomFree = true;
                                    }
                                }
                                if (!isRoomFree)
                                    continue;

                                dtFnlTable.Rows[Days][Hours[0].ToString()] = subject_codeL + "-" + staff_code + "/" + staff_code2 + "$" + roomVal[1];
                                dtFnlTable.Rows[Days][Hours[1].ToString()] = subject_codeL + "-" + staff_code + "/" + staff_code2 + "$" + roomVal[1];

                                allotedHrs += 2;
                                NoOfHrsToAllot -= 2;
                                dicAllocSub[subject_noL] += 2;
                                arrAlreadyAddedRow.Add(subject_noL + "_" + Days);
                                arrAlreadyAddedCol.Add(subject_noL + "_" + Hours[0]);
                                arrAlreadyAddedCol.Add(subject_noL + "_" + Hours[1]);

                                //Room Engage
                                dicCurRoomAvail[hrI] = 1;
                                dicCurRoomAvail[hrII] = 1;
                                break;
                            }
                        }
                    }
                }

            }
            #endregion

            #region Theory Fill
            dtPreAllocation.DefaultView.RowFilter = "ElectivePap<>'True' and Lab<>'True'";
            DataTable dtTheory = dtPreAllocation.DefaultView.ToTable();

            for (int preAllocI = 0; preAllocI < dtTheory.Rows.Count; preAllocI++)
            {
                string subject_type = Convert.ToString(dtTheory.Rows[preAllocI]["subject_type"]).Trim();
                string subType_no = Convert.ToString(dtTheory.Rows[preAllocI]["subType_no"]).Trim();
                string subject_name = Convert.ToString(dtTheory.Rows[preAllocI]["subject_name"]).Trim();
                string subject_code = Convert.ToString(dtTheory.Rows[preAllocI]["subject_code"]).Trim();
                string subject_no = Convert.ToString(dtTheory.Rows[preAllocI]["subject_no"]).Trim();
                bool Lab = Convert.ToString(dtTheory.Rows[preAllocI]["Lab"]).Trim() == "TRUE" ? true : false; ;
                bool ElectivePap = Convert.ToString(dtTheory.Rows[preAllocI]["ElectivePap"]).Trim() == "TRUE" ? true : false;
                string Faculty = Convert.ToString(dtTheory.Rows[preAllocI]["Faculty"]).Trim();
                byte Days = Convert.ToByte(dtTheory.Rows[preAllocI]["Days"]);
                byte Hours = Convert.ToByte(dtTheory.Rows[preAllocI]["Hours"]);

                byte allotedHrs = 0;

                dtSubjectDet.DefaultView.RowFilter = " subject_no = '" + subject_no + "'";
                DataView dvSubType = dtSubjectDet.DefaultView;
                if (dvSubType.Count > 0)
                {



                    byte noofhrsperweek = Convert.ToByte(dvSubType[0]["noofhrsperweek"]);

                    if (dicAllocSub.ContainsKey(subject_no))
                    {
                        allotedHrs = dicAllocSub[subject_no];
                        if (allotedHrs >= noofhrsperweek)
                            continue;
                    }
                    else
                    {
                        dicAllocSub.Add(subject_no, allotedHrs);
                    }

                    //Loop through staff and availability
                    dtSubjectDetWt.DefaultView.RowFilter = "subject_no='" + subject_no + "' and appl_id='" + Faculty + "'";
                    DataView dvSubjectStaff = dtSubjectDetWt.DefaultView;
                    dvSubjectStaff.Sort = " staffPriority asc";
                    if (dvSubjectStaff.Count > 0)
                    {
                        foreach (string roomPKACR in arrlstRoomDet)
                        {
                            string[] roomVal = roomPKACR.Split('#');
                            string hrI = roomPKACR + "$" + (Days + 1) + "_" + Hours;


                            bool isRoomFree = false;
                            if (dicCurRoomAvail.ContainsKey(hrI) && dicCurRoomAvail[hrI] == 0)
                            {
                                isRoomFree = true;

                            }
                            if (!isRoomFree)
                                continue;
                            string staff_code = Convert.ToString(dvSubjectStaff[0]["staff_code"]);
                            string staff_appno = Convert.ToString(dvSubjectStaff[0]["appl_id"]);

                            dtFnlTable.Rows[Days][Hours.ToString()] = subject_code + "-" + staff_code + "$" + roomVal[1];
                            allotedHrs++;
                            NoOfHrsToAllot--;
                            dicAllocSub[subject_no]++;
                            arrAlreadyAddedRow.Add(subject_no + "_" + Days);
                            arrAlreadyAddedCol.Add(subject_no + "_" + Hours.ToString());

                            //Room Engagement
                            dicCurRoomAvail[hrI] = 1;
                            break;
                        }
                    }
                }
            }
            #endregion
            #endregion

            //restartLoop:
            //Loop through each day and hour
            for (int dayI = 0; dayI < (dtFnlTable.Rows.Count - 1); dayI++)
            {
                //Loop through Hours
                for (int hrsI = 1; hrsI < (dtFnlTable.Columns.Count); hrsI++)
                {
                    //Loop through subjects
                    for (int subI = 0; subI < dtSubjectDet.Rows.Count; subI++)
                    {
                        string colName = Convert.ToString(dtFnlTable.Columns[hrsI].ColumnName).Trim();
                        byte testCol = 0;
                        if (byte.TryParse(dtFnlTable.Columns[hrsI].ColumnName, out testCol) && Convert.ToString(dtFnlTable.Rows[dayI][colName]).Trim() == string.Empty)
                        {
                            #region Check for Lunch Availability
                            string comparingCell4 = Convert.ToString(dtFnlTable.Rows[dayI]["4"]).Trim();
                            string comparingCell5 = Convert.ToString(dtFnlTable.Rows[dayI]["5"]).Trim();

                            if (testCol == 4 && (!string.IsNullOrEmpty(comparingCell5)))
                            {
                                continue;
                            }
                            if (testCol == 5 && (!string.IsNullOrEmpty(comparingCell4)))
                            {
                                continue;
                            }
                            #endregion

                            string subject_no = Convert.ToString(dtSubjectDet.Rows[subI]["subject_no"]).Trim();
                            byte noofhrsperweek = Convert.ToByte(dtSubjectDet.Rows[subI]["noofhrsperweek"]);
                            string subject_code = Convert.ToString(dtSubjectDet.Rows[subI]["subject_code"]);
                            string ElectivePap = Convert.ToString(dtSubjectDet.Rows[subI]["ElectivePap"]).Trim().ToUpper();
                            string Lab = Convert.ToString(dtSubjectDet.Rows[subI]["Lab"]).Trim().ToUpper();
                            byte allotedHrs = 0;

                            //Check For existance
                            if (ElectivePap == "TRUE")
                            {
                            }
                            else if (dicAllocSub.ContainsKey(subject_no))
                            {
                                allotedHrs = dicAllocSub[subject_no];
                                if (allotedHrs >= noofhrsperweek)
                                    continue;
                            }
                            else
                            {
                                dicAllocSub.Add(subject_no, allotedHrs);
                            }

                            if (Lab != "TRUE" && ElectivePap != "TRUE")
                            {
                                #region Theory Region
                                if (arrAlreadyAddedRow.Contains(subject_no + "_" + dayI) || arrAlreadyAddedCol.Contains(subject_no + "_" + hrsI))
                                {
                                    continue;
                                }
                                //Loop through staff and availability
                                dtSubjectDetWt.DefaultView.RowFilter = "subject_no='" + subject_no + "' and staff_code='" + Convert.ToString(dtFacultyChoices.Rows[facChoiceIndex][subject_no]) + "'";
                                DataView dvSubjectStaff = dtSubjectDetWt.DefaultView;
                                dvSubjectStaff.Sort = " staffPriority asc";
                                if (dvSubjectStaff.Count > 0)
                                {
                                    string staff_code = Convert.ToString(dvSubjectStaff[0]["staff_code"]);
                                    string staff_appno = Convert.ToString(dvSubjectStaff[0]["appl_id"]);

                                    dtStaffDet.DefaultView.RowFilter = "staff_appno in (" + staff_appno + ") and DaysFK='" + (dayI + 1) + "' and HoursFK='" + testCol + "' ";
                                    DataView dvStaffAvail = dtStaffDet.DefaultView;
                                    bool IsEngaged = false;
                                    if (dvStaffAvail.Count > 0)
                                    {
                                        if (dvStaffAvail[0]["IsEngaged"].ToString().ToUpper() == "TRUE")
                                            IsEngaged = true;
                                    }

                                    if (!IsEngaged)
                                    {
                                        foreach (string roomPKACR in arrlstRoomDet)
                                        {
                                            string[] roomVal = roomPKACR.Split('#');
                                            string hrI = roomPKACR + "$" + (dayI + 1) + "_" + testCol;


                                            bool isRoomFree = false;
                                            if (dicCurRoomAvail.ContainsKey(hrI) && dicCurRoomAvail[hrI] == 0)
                                            {
                                                isRoomFree = true;

                                            }
                                            if (!isRoomFree)
                                                continue;

                                            dtFnlTable.Rows[dayI][colName] = subject_code + "-" + staff_code + "$" + roomVal[1];
                                            allotedHrs++;
                                            NoOfHrsToAllot--;
                                            dicAllocSub[subject_no]++;
                                            arrAlreadyAddedRow.Add(subject_no + "_" + dayI);
                                            arrAlreadyAddedCol.Add(subject_no + "_" + hrsI);

                                            //Room Engagement
                                            dicCurRoomAvail[hrI] = 1;
                                            break;
                                        }
                                    }
                                }
                                #endregion
                            }
                            else if (ElectivePap == "TRUE")
                            {
                                #region Elective Region
                                //For Subject Type - Elective Group
                                DataTable dtSubjectType = dtSubjectDet.DefaultView.ToTable(true, "subType_no", "subject_type", "subject_no");
                                dtSubjectType.DefaultView.RowFilter = "subject_no='" + subject_no + "'";
                                DataView dvSubjectType = dtSubjectType.DefaultView;
                                if (dvSubjectType.Count > 0)
                                {
                                    string subType_no = Convert.ToString(dvSubjectType[0]["subType_no"]);
                                    if (dicAllocElecSub.ContainsKey(subType_no))
                                    {
                                        allotedHrs = dicAllocElecSub[subType_no];
                                        if (allotedHrs >= noofhrsperweek)
                                            continue;
                                    }
                                    else
                                    {
                                        dicAllocElecSub.Add(subType_no, allotedHrs);
                                    }

                                    if (arrAlreadyAddedRow.Contains(subType_no + "E_" + dayI) || arrAlreadyAddedCol.Contains(subType_no + "E_" + hrsI))
                                    {
                                        continue;
                                    }
                                    StringBuilder sbSubDisp = new StringBuilder();
                                    StringBuilder sbSubjStaff = new StringBuilder();
                                    sbSubjStaff.Append("(");

                                    dtSubjectDet.DefaultView.RowFilter = " subType_no = '" + subType_no + "'";
                                    DataView dvSubType = dtSubjectDet.DefaultView;
                                    if (dvSubType.Count > 0)
                                    {
                                        for (int electSubI = 0; electSubI < dvSubType.Count; electSubI++)
                                        {
                                            string subject_noE = Convert.ToString(dvSubType[electSubI]["subject_no"]).Trim();
                                            string subject_codeE = Convert.ToString(dvSubType[electSubI]["subject_code"]);

                                            //Loop through staff and availability
                                            dtSubjectDetWt.DefaultView.RowFilter = "subject_no='" + subject_noE + "'  and staff_code='" + Convert.ToString(dtFacultyChoices.Rows[facChoiceIndex][subject_noE]) + "'";
                                            DataView dvSubjectStaff = dtSubjectDetWt.DefaultView;
                                            dvSubjectStaff.Sort = " staffPriority asc";
                                            if (dvSubjectStaff.Count > 0)
                                            {
                                                string staff_code = Convert.ToString(dvSubjectStaff[0]["staff_code"]);
                                                string staff_appno = Convert.ToString(dvSubjectStaff[0]["appl_id"]);
                                                sbSubDisp.Append(subject_codeE + "-" + staff_code + ",");
                                                sbSubjStaff.Append(" staff_appno = '" + staff_appno + "' or ");
                                            }
                                        }

                                    }
                                    if (sbSubDisp.Length > 1)
                                        sbSubDisp.Remove(sbSubDisp.Length - 1, 1);

                                    if (sbSubjStaff.Length > 3)
                                        sbSubjStaff.Remove(sbSubjStaff.Length - 3, 3);
                                    sbSubjStaff.Append(") and ");

                                    dtStaffDet.DefaultView.RowFilter = sbSubjStaff.ToString() + " DaysFK='" + (dayI + 1) + "' and HoursFK='" + testCol + "' ";
                                    DataView dvStaffAvail = dtStaffDet.DefaultView;
                                    bool IsEngaged = false;
                                    if (dvStaffAvail.Count > 0)
                                    {
                                        for (int dvI = 0; dvI < dvStaffAvail.Count; dvI++)
                                        {
                                            if (dvStaffAvail[dvI]["IsEngaged"].ToString().ToUpper() == "TRUE")
                                                IsEngaged = true;
                                        }
                                    }

                                    if (!IsEngaged)
                                    {

                                        StringBuilder sbElectRoom = new StringBuilder();
                                        ArrayList arrElectRoom = new ArrayList();
                                        foreach (string roomPKACR in arrlstRoomDet)
                                        {
                                            string[] roomVal = roomPKACR.Split('#');
                                            string hrI = roomPKACR + "$" + (dayI + 1) + "_" + testCol;

                                            bool isRoomFree = false;
                                            if (dicCurRoomAvail.ContainsKey(hrI) && dicCurRoomAvail[hrI] == 0)
                                            {
                                                isRoomFree = true;
                                            }
                                            if (!isRoomFree)
                                                continue;

                                            arrElectRoom.Add(hrI);
                                            sbElectRoom.Append(roomVal[1] + ",");

                                            if (arrElectRoom.Count == dvSubType.Count)
                                                break;
                                        }

                                        if (arrElectRoom.Count == dvSubType.Count)
                                        {
                                            if (sbElectRoom.Length > 1)
                                                sbElectRoom.Remove(sbElectRoom.Length - 1, 1);

                                            dtFnlTable.Rows[dayI][colName.ToString()] = sbSubDisp.ToString() + "$" + sbElectRoom.ToString();
                                            allotedHrs++;
                                            NoOfHrsToAllot--;
                                            dicAllocElecSub[subType_no]++;
                                            arrAlreadyAddedRow.Add(subType_no + "E_" + dayI);
                                            arrAlreadyAddedCol.Add(subType_no + "E_" + hrsI);

                                            //Room Engage
                                            for (int i = 0; i < arrElectRoom.Count; i++)
                                            {
                                                string hrI = " #" + arrElectRoom[i] + "$" + (dayI + 1) + "_" + testCol;
                                                dicCurRoomAvail[hrI] = 1;
                                            }

                                        }

                                    }
                                }
                                #endregion
                            }
                            else if (Lab == "TRUE")
                            {
                                #region Before Room Allocation Inner Side
                                //Lunch Check
                                if (testCol == 3 && (!string.IsNullOrEmpty(comparingCell5) || !string.IsNullOrEmpty(comparingCell4)))
                                {
                                    continue;
                                }
                                if (testCol == 4)
                                {
                                    continue;
                                }
                                #region Lab Region
                                if (arrAlreadyAddedRow.Contains(subject_no + "_" + dayI) || arrAlreadyAddedCol.Contains(subject_no + "_" + hrsI))
                                {
                                    continue;
                                }

                                //Loop through staff and availability
                                dtSubjectDetWt.DefaultView.RowFilter = "subject_no='" + subject_no + "'  and staff_code in ('" + Convert.ToString(dtFacultyChoices.Rows[facChoiceIndex][subject_no]) + "')";
                                DataView dvSubjectStaff = dtSubjectDetWt.DefaultView;
                                dvSubjectStaff.Sort = " staffPriority asc";
                                if (dvSubjectStaff.Count > 1)
                                {
                                    string staff_code = Convert.ToString(dvSubjectStaff[0]["staff_code"]);
                                    string staff_code2 = Convert.ToString(dvSubjectStaff[1]["staff_code"]);

                                    string staff_appno = Convert.ToString(dvSubjectStaff[0]["appl_id"]);
                                    string staff_appno2 = Convert.ToString(dvSubjectStaff[1]["appl_id"]);

                                    byte nextCol = 0;

                                    if ((hrsI + 1) < dtFnlTable.Columns.Count)
                                    {
                                        string colName2 = Convert.ToString(dtFnlTable.Columns[hrsI + 1].ColumnName).Trim();
                                        if (byte.TryParse(dtFnlTable.Columns[hrsI + 1].ColumnName, out nextCol) && Convert.ToString(dtFnlTable.Rows[dayI][colName2]).Trim() == string.Empty)
                                        {
                                            if (arrAlreadyAddedCol.Contains(subject_no + "_" + (testCol + 1)))
                                            {
                                                continue;
                                            }

                                            dtStaffDet.DefaultView.RowFilter = "(staff_appno = '" + staff_appno + "' or staff_appno = '" + staff_appno2 + "') and DaysFK='" + (dayI + 1) + "' and HoursFK='" + testCol + "' ";
                                            DataView dvStaffAvail = dtStaffDet.DefaultView;
                                            bool IsEngaged = false;
                                            if (dvStaffAvail.Count > 0)
                                            {
                                                if (dvStaffAvail[0]["IsEngaged"].ToString().ToUpper() == "TRUE")
                                                    IsEngaged = true;
                                                if (dvStaffAvail.Count > 1 && dvStaffAvail[1]["IsEngaged"].ToString().ToUpper() == "TRUE")
                                                    IsEngaged = true;
                                            }

                                            if (!IsEngaged)
                                            {
                                                #region Check For Next Hour EngageMent
                                                IsEngaged = false;
                                                dtStaffDet.DefaultView.RowFilter = "(staff_appno = '" + staff_appno + "' or staff_appno ='" + staff_appno2 + "') and DaysFK='" + (dayI + 1) + "' and HoursFK='" + (testCol + 1) + "' ";
                                                DataView dvStaffAvailNext = dtStaffDet.DefaultView;

                                                if (dvStaffAvailNext.Count > 0)
                                                {
                                                    if (dvStaffAvailNext[0]["IsEngaged"].ToString().ToUpper() == "TRUE")
                                                        IsEngaged = true;
                                                    if (dvStaffAvailNext.Count > 1 && dvStaffAvailNext[1]["IsEngaged"].ToString().ToUpper() == "TRUE")
                                                        IsEngaged = true;
                                                }
                                                #endregion
                                                if (!IsEngaged)
                                                {
                                                    foreach (string roomPKACR in arrlstLabDet)
                                                    {
                                                        string[] roomVal = roomPKACR.Split('#');
                                                        string hrI = roomPKACR + "$" + (dayI + 1) + "_" + testCol;
                                                        string hrII = roomPKACR + "$" + (dayI + 1) + "_" + (testCol + 1);

                                                        bool isRoomFree = false;
                                                        if (dicCurRoomAvail.ContainsKey(hrI) && dicCurRoomAvail.ContainsKey(hrII))
                                                        {
                                                            if (dicCurRoomAvail[hrI] == 0 && dicCurRoomAvail[hrII] == 0)
                                                            {
                                                                isRoomFree = true;
                                                            }
                                                        }
                                                        if (!isRoomFree)
                                                            continue;
                                                        dtFnlTable.Rows[dayI][dtFnlTable.Columns[(hrsI)].ColumnName] = subject_code + "-" + staff_code + "/" + staff_code2 + "$" + roomVal[1];
                                                        dtFnlTable.Rows[dayI][dtFnlTable.Columns[(hrsI + 1)].ColumnName] = subject_code + "-" + staff_code + "/" + staff_code2 + "$" + roomVal[1];

                                                        allotedHrs += 2;
                                                        NoOfHrsToAllot -= 2;
                                                        dicAllocSub[subject_no] += 2;
                                                        arrAlreadyAddedRow.Add(subject_no + "_" + dayI);
                                                        arrAlreadyAddedCol.Add(subject_no + "_" + testCol);
                                                        arrAlreadyAddedCol.Add(subject_no + "_" + (testCol + 1));

                                                        //Room Engage
                                                        dicCurRoomAvail[hrI] = 1;
                                                        dicCurRoomAvail[hrII] = 1;
                                                        break;
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                                #endregion
                                #endregion
                            }
                        }
                        #region Switch Subject Location
                        //dtSubjectDet.DefaultView.RowFilter = "ElectivePap='True'";
                        //DataTable dtSubjectElectDet = dtSubjectDet.DefaultView.ToTable();
                        //dtSubjectDet.DefaultView.RowFilter = "ElectivePap<>'True'";
                        //DataTable dtSubjectTheoryLabDet = dtSubjectDet.DefaultView.ToTable();

                        //DataRow dr = dtSubjectTheoryLabDet.NewRow();
                        //foreach (DataColumn dc in dtSubjectTheoryLabDet.Columns)
                        //{
                        //    dr[dc] = dtSubjectTheoryLabDet.Rows[dtSubjectTheoryLabDet.Rows.Count - 1][dc];
                        //}
                        //dtSubjectTheoryLabDet.Rows.RemoveAt(dtSubjectTheoryLabDet.Rows.Count - 1);
                        //dtSubjectTheoryLabDet.Rows.InsertAt(dr, 0);

                        //dtSubjectDet.Clear();
                        //if (hrsI % 2 == 1)
                        //{
                        //    dtSubjectDet.Merge(dtSubjectTheoryLabDet);
                        //    dtSubjectDet.Merge(dtSubjectElectDet);
                        //}
                        //else
                        //{
                        //    dtSubjectDet.Merge(dtSubjectElectDet);
                        //    dtSubjectDet.Merge(dtSubjectTheoryLabDet);
                        //}
                        #endregion
                    }
                }
            }
            if (NoOfHrsToAllot != 0)
            {
                //goto restartLoop;
                dtFnlTable.Clear();
            }
            //else
            //{
            //    dtStaffDet.Merge(dtCurStaffDet);
            //    foreach (KeyValuePair<string, int> roomVal in dicCurRoomAvail)
            //    {
            //        if (roomVal.Value == 1)
            //        {
            //            if (dicRoomAvailability.ContainsKey(roomVal.Key))
            //            {
            //                dicRoomAvailability[roomVal.Key] = 1;
            //            }
            //        }
            //    }
            //}
            dtTimeTable = dtFnlTable;
        }
        catch { dtTimeTable.Clear(); }
        return dtTimeTable;
    }
    //Normal Allocation
    private DataTable getTimeTableFormat(int batchYear, int degreeCode, int currentSem, string dispText, DataTable dtSubjectDet, DataTable dtSubjectDetWt, DataTable dtFacultyChoices, int facChoiceIndex, int maxNoCanAllot, int noOfHrsPerDay, DataTable dtBellSchedule, DataTable dtCriteria, ref DataTable dtStaffDet, Hashtable hashElectivesubject, Hashtable hashElectivesubjectHr, Hashtable hashLabsubject, Hashtable hashLabsubjectHr, Hashtable hashsubject, Hashtable hashsubjectHr, Dictionary<string, int> dicRoomAvailability, ArrayList arrlstRoomDet, ArrayList arrlstLabDet)
    {
        DataTable dtSpclStaff = dirAccess.selectDataTable("SELECT STAFF_CODE,HOURPK FROM TT_StudentCriteria WHERE DAYPK='0'");
        DataTable dtEngage = getEngagedHrs();
        DataTable dtTimeTable = new DataTable();
        try
        {
            //Total No of Hours to Allot for a week
            int NoOfHrsToAllot = maxNoCanAllot;
            NoOfHrsToAllot -= 5;

            DataTable dtCurStaffDet = dtStaffDet.Copy();
            dtCurStaffDet.Clear();
            Dictionary<string, int> dicCurRoomAvail = new Dictionary<string, int>();
            foreach (KeyValuePair<string, int> roomVal in dicRoomAvailability)
            {
                dicCurRoomAvail.Add(roomVal.Key, roomVal.Value);
            }

            DataTable dtFnlTable = new DataTable();
            dtFnlTable.Columns.Add("Day/Period");

            dtFnlTable.Rows.Add("Monday");
            dtFnlTable.Rows.Add("Tuesday");
            dtFnlTable.Rows.Add("Wednesday");
            dtFnlTable.Rows.Add("Thursday");
            dtFnlTable.Rows.Add("Friday");
            dtFnlTable.Rows.Add("");
            for (int periodI = 0; periodI < dtBellSchedule.Rows.Count; periodI++)
            {
                dtFnlTable.Columns.Add(Convert.ToString(dtBellSchedule.Rows[periodI]["Period1"]).Trim());
                dtFnlTable.Rows[5][(periodI + 1)] = Convert.ToString(dtBellSchedule.Rows[periodI]["start_time"]).Trim() + "- " + Convert.ToString(dtBellSchedule.Rows[periodI]["end_time"]).Trim();
            }
            int lastPeriod = Convert.ToInt32(dtFnlTable.Columns[dtFnlTable.Columns.Count - 1].ColumnName);

            //Students Already Engaged in Options
            //NoOfHrsToAllot += 4;
            for (int optI = 0; optI < dtCriteria.Rows.Count; optI++)
            {
                #region Old 25-02-2017
                //byte DaysFk = Convert.ToByte(dtCriteria.Rows[optI]["DayPk"]);
                //byte HrsFk = Convert.ToByte(dtCriteria.Rows[optI]["HourPk"]);
                //string IsEngaged = Convert.ToString(dtCriteria.Rows[optI]["IsEngaged"]).Trim().ToUpper();
                //dtFnlTable.Rows[(DaysFk - 1)][HrsFk.ToString()] = "ENGAGED";
                ////NoOfHrsToAllot--; 
                #endregion

                byte DaysFk = Convert.ToByte(dtCriteria.Rows[optI]["DayPk"]);
                byte HrsFk = Convert.ToByte(dtCriteria.Rows[optI]["HourPk"]);
                string IsEngaged = Convert.ToString(dtCriteria.Rows[optI]["IsEngaged"]).Trim().ToUpper();
                string subject_code = Convert.ToString(dtCriteria.Rows[optI]["criterianame"]).Trim();


                foreach (string roomPKACR in arrlstRoomDet)
                {
                    string[] roomVal = roomPKACR.Split('#');
                    string hrI = roomPKACR + "$" + (DaysFk) + "_" + HrsFk;


                    bool isRoomFree = false;
                    if (dicCurRoomAvail.ContainsKey(hrI) && dicCurRoomAvail[hrI] == 0)
                    {
                        isRoomFree = true;

                    }
                    if (!isRoomFree)
                        continue;

                    dtFnlTable.Rows[(DaysFk - 1)][HrsFk.ToString()] = subject_code + "$" + roomVal[1];
                    //NoOfHrsToAllot--;
                    dicCurRoomAvail[hrI] = 1;
                    break;
                }
            }
            Dictionary<string, byte> dicAllocSub = new Dictionary<string, byte>();
            Dictionary<string, byte> dicAllocElecSub = new Dictionary<string, byte>();
            ArrayList arrLabAllocToday = new ArrayList();

            ArrayList arrAlreadyAddedRow = new ArrayList();
            ArrayList arrAlreadyAddedCol = new ArrayList();//To Check whether already added in the row & column

            bool fromRestart = false;
        restartLoop:
            //Loop through each day and hour


            //List<byte> lstDays = new List<byte>();
            //byte daysCount = (byte)(dtFnlTable.Rows.Count - 1);
            //for (int dayI = 0; dayI < daysCount; dayI++)
            //{
            //    lstDays.Add(getRandomDay(daysCount, lstDays));
            //}

            for (int dayI = 0; dayI < (dtFnlTable.Rows.Count - 1); dayI++)
            //for (int dI = 0; dI < lstDays.Count; dI++)
            {
                // int dayI = lstDays[dI];

                //List<byte> lstHours = new List<byte>();
                //byte periodsCount = (byte)dtFnlTable.Columns.Count;//noOfHrsPerDay;
                //for (int perI = 1; perI < periodsCount; perI++)
                //{
                //    lstHours.Add(getRandomPeriod(periodsCount, lstHours));
                //}
                //Loop through Hours
                for (int hrsI = 1; hrsI < (dtFnlTable.Columns.Count); hrsI++)
                //for (int lstI = 0; lstI < lstHours.Count; lstI++)
                {
                    //byte hrsI = lstHours[lstI];

                    Dictionary<string, bool> dicSubType = new Dictionary<string, bool>();
                    //Loop through subjects
                    for (int subI = 0; subI < dtSubjectDet.Rows.Count; subI++)
                    {
                        string colName = Convert.ToString(dtFnlTable.Columns[hrsI].ColumnName).Trim();
                        byte testCol = 0;
                        if (byte.TryParse(dtFnlTable.Columns[hrsI].ColumnName, out testCol) && Convert.ToString(dtFnlTable.Rows[dayI][colName]).Trim() == string.Empty)
                        {
                            #region Check for Lunch Availability
                            string comparingCell4 = Convert.ToString(dtFnlTable.Rows[dayI]["4"]).Trim();
                            string comparingCell5 = Convert.ToString(dtFnlTable.Rows[dayI]["5"]).Trim();

                            if (testCol == 4 && (!string.IsNullOrEmpty(comparingCell5)))
                            {
                                continue;
                            }
                            if (testCol == 5 && (!string.IsNullOrEmpty(comparingCell4)))
                            {
                                continue;
                            }
                            #endregion

                            string subject_no = Convert.ToString(dtSubjectDet.Rows[subI]["subject_no"]).Trim();
                            byte noofhrsperweek = Convert.ToByte(dtSubjectDet.Rows[subI]["noofhrsperweek"]);
                            string subject_code = Convert.ToString(dtSubjectDet.Rows[subI]["subject_code"]);
                            string ElectivePap = Convert.ToString(dtSubjectDet.Rows[subI]["ElectivePap"]).Trim().ToUpper();
                            string Lab = Convert.ToString(dtSubjectDet.Rows[subI]["Lab"]).Trim().ToUpper();
                            byte allotedHrs = 0;

                            //Check For existance
                            if (ElectivePap == "TRUE")
                            {
                            }
                            else if (dicAllocSub.ContainsKey(subject_no))
                            {
                                allotedHrs = dicAllocSub[subject_no];
                                if (allotedHrs >= noofhrsperweek)
                                    continue;
                            }
                            else
                            {
                                dicAllocSub.Add(subject_no, allotedHrs);
                            }

                            if (Lab != "TRUE" && ElectivePap != "TRUE")
                            {
                                #region Theory Region

                                if (!fromRestart)
                                    if (arrAlreadyAddedRow.Contains(subject_no + "_" + dayI))//|| arrAlreadyAddedCol.Contains(subject_no + "_" + hrsI)
                                    {
                                        continue;
                                    }
                                //Loop through staff and availability
                                dtSubjectDetWt.DefaultView.RowFilter = "subject_no='" + subject_no + "' and staff_code='" + Convert.ToString(dtFacultyChoices.Rows[facChoiceIndex][subject_no]) + "'";
                                DataView dvSubjectStaff = dtSubjectDetWt.DefaultView;
                                dvSubjectStaff.Sort = " staffPriority asc";
                                if (dvSubjectStaff.Count > 0)
                                {
                                    string staff_code = Convert.ToString(dvSubjectStaff[0]["staff_code"]);
                                    string staff_appno = Convert.ToString(dvSubjectStaff[0]["appl_id"]);


                                    #region Special Staff
                                    dtSpclStaff.DefaultView.RowFilter = "staff_code='" + staff_code + "'";
                                    DataView dvIsSpecial = dtSpclStaff.DefaultView;
                                    if (dvIsSpecial.Count > 0)
                                    {
                                        DataTable dtSubHr = dvIsSpecial.ToTable();
                                        dtSubHr.DefaultView.RowFilter = "hourpk='" + hrsI + "'";
                                        DataView dvNHr = dtSubHr.DefaultView;
                                        if (dvNHr.Count == 0)
                                        {
                                            continue;
                                        }
                                    }
                                    else
                                    {

                                        //if (arrAlreadyAddedRow.Contains(subject_no + "_" + (dayI - 1)) && arrAlreadyAddedCol.Contains(subject_no + "_" + hrsI))
                                        //{
                                        //    continue;
                                        //}

                                        if (!fromRestart)
                                            if (arrAlreadyAddedCol.Contains(subject_no + "_" + hrsI))//|| 
                                            {
                                                continue;
                                            }
                                    }
                                    #endregion

                                    dtStaffDet.DefaultView.RowFilter = "staff_appno in (" + staff_appno + ") and DaysFK='" + (dayI + 1) + "' and HoursFK='" + testCol + "' ";
                                    DataView dvStaffAvail = dtStaffDet.DefaultView;
                                    bool IsEngaged = false;
                                    if (dvStaffAvail.Count > 0)
                                    {
                                        if (dvStaffAvail[0]["IsEngaged"].ToString().ToUpper() == "TRUE")
                                            IsEngaged = true;
                                    }

                                    #region Staff Theory, Lab, Day maximum hours check

                                    //if (testCol == lastPeriod)
                                    //{
                                    //    dtStaffDet.DefaultView.RowFilter = "staff_appno in (" + staff_appno + ") and DaysFK='" + (dayI + 1) + "' and HoursFK='" + (testCol - 1) + "' ";
                                    //    DataView dvStaffAvailMax = dtStaffDet.DefaultView;
                                    //    if (dvStaffAvailMax.Count > 0)
                                    //    {
                                    //        if (dvStaffAvailMax[0]["IsEngaged"].ToString().ToUpper() == "TRUE")
                                    //            IsEngaged = true;
                                    //    }
                                    //}
                                    //else if (testCol == 1)
                                    //{
                                    //    dtStaffDet.DefaultView.RowFilter = "staff_appno in (" + staff_appno + ") and DaysFK='" + (dayI + 1) + "' and HoursFK='" + (testCol + 1) + "' ";
                                    //    DataView dvStaffAvailMax = dtStaffDet.DefaultView;
                                    //    if (dvStaffAvailMax.Count > 0)
                                    //    {
                                    //        if (dvStaffAvailMax[0]["IsEngaged"].ToString().ToUpper() == "TRUE")
                                    //            IsEngaged = true;
                                    //    }
                                    //}
                                    //else
                                    //{
                                    //    dtStaffDet.DefaultView.RowFilter = "staff_appno in (" + staff_appno + ") and DaysFK='" + (dayI + 1) + "' and (HoursFK='" + (testCol + 1) + "' or  HoursFK='" + (testCol - 1) + "' )";
                                    //    DataView dvStaffAvailMax = dtStaffDet.DefaultView;
                                    //    if (dvStaffAvailMax.Count > 0)
                                    //    {
                                    //        if (dvStaffAvailMax[0]["IsEngaged"].ToString().ToUpper() == "TRUE")
                                    //            IsEngaged = true;
                                    //    }
                                    //}

                                    //dtStaffDet.DefaultView.RowFilter = "staff_appno in (" + staff_appno + ") and DaysFK='" + (dayI + 1) + "' and MaxHour='1'  ";
                                    //DataView dvStaffAvailMaxDay = dtStaffDet.DefaultView;
                                    //if (dvStaffAvailMaxDay.Count > 2)
                                    //{
                                    //    continue;
                                    //}
                                    #endregion

                                    if (!IsEngaged)
                                    {
                                        foreach (string roomPKACR in arrlstRoomDet)
                                        {
                                            string[] roomVal = roomPKACR.Split('#');
                                            string hrI = roomPKACR + "$" + (dayI + 1) + "_" + testCol;


                                            bool isRoomFree = false;
                                            if (dicCurRoomAvail.ContainsKey(hrI) && dicCurRoomAvail[hrI] == 0)
                                            {
                                                isRoomFree = true;

                                            }
                                            if (!isRoomFree)
                                                continue;
                                            dtFnlTable.Rows[dayI][colName] = subject_code + "-" + staff_code + "$" + roomVal[1];
                                            allotedHrs++;
                                            NoOfHrsToAllot--;
                                            dicAllocSub[subject_no]++;
                                            arrAlreadyAddedRow.Add(subject_no + "_" + dayI);
                                            arrAlreadyAddedCol.Add(subject_no + "_" + hrsI);
                                            dicCurRoomAvail[hrI] = 1;
                                            break;
                                        }
                                    }
                                }
                                #endregion
                            }
                            else if (ElectivePap == "TRUE")
                            {
                                dtEngage.DefaultView.RowFilter = "DayPk='" + (dayI + 1) + "' and hourpk='" + testCol + "'";
                                DataView dvEng = dtEngage.DefaultView;
                                if (dvEng.Count > 0)
                                {
                                    continue;
                                }
                                #region Elective Region
                                //For Subject Type - Elective Group
                                DataTable dt_elective = dtSubjectDet.Copy();

                                DataTable dtSubjectType = dt_elective.DefaultView.ToTable(true, "subType_no", "subject_type", "subject_no");
                                dtSubjectType.DefaultView.RowFilter = "subject_no='" + subject_no + "'";
                                DataView dvSubjectType = dtSubjectType.DefaultView;
                                if (dvSubjectType.Count > 0)
                                {
                                    string subType_no = Convert.ToString(dvSubjectType[0]["subType_no"]);
                                    if (dicAllocElecSub.ContainsKey(subType_no))
                                    {
                                        allotedHrs = dicAllocElecSub[subType_no];
                                        if (allotedHrs >= noofhrsperweek)
                                            continue;
                                    }
                                    else
                                    {
                                        dicAllocElecSub.Add(subType_no, allotedHrs);
                                    }

                                    if (arrAlreadyAddedRow.Contains(subType_no + "E_" + dayI) || arrAlreadyAddedCol.Contains(subType_no + "E_" + hrsI))
                                    {
                                        continue;
                                    }
                                    StringBuilder sbSubDisp = new StringBuilder();
                                    StringBuilder sbSubjStaff = new StringBuilder();
                                    sbSubjStaff.Append("(");

                                    dtSubjectDet.DefaultView.RowFilter = " subType_no = '" + subType_no + "'";
                                    DataView dvSubType = dtSubjectDet.DefaultView;
                                    int electStaffCount = 0;
                                    bool isMaxHourOK = true;
                                    if (dvSubType.Count > 0)
                                    {
                                        for (int electSubI = 0; electSubI < dvSubType.Count; electSubI++)
                                        {
                                            string subject_noE = Convert.ToString(dvSubType[electSubI]["subject_no"]).Trim();
                                            string subject_codeE = Convert.ToString(dvSubType[electSubI]["subject_code"]);

                                            //Loop through staff and availability
                                            dtSubjectDetWt.DefaultView.RowFilter = "subject_no='" + subject_noE + "' ";//  and staff_code='" + Convert.ToString(dtFacultyChoices.Rows[facChoiceIndex][subject_noE]) + "'
                                            DataView dvSubjectStaff = dtSubjectDetWt.DefaultView;
                                            dvSubjectStaff.Sort = " staffPriority asc";
                                            if (dvSubjectStaff.Count > 0)
                                            {
                                                StringBuilder sbStaffCodes = new StringBuilder();
                                                for (int stfI = 0; stfI < dvSubjectStaff.Count; stfI++)
                                                {
                                                    string staff_code = Convert.ToString(dvSubjectStaff[stfI]["staff_code"]);
                                                    string staff_appno = Convert.ToString(dvSubjectStaff[stfI]["appl_id"]);
                                                    sbStaffCodes.Append(staff_code + "/");
                                                    sbSubjStaff.Append(" staff_appno = '" + staff_appno + "' or ");
                                                    electStaffCount++;

                                                    dtStaffDet.DefaultView.RowFilter = "staff_appno in (" + staff_appno + ") and DaysFK='" + (dayI + 1) + "' and MaxHour='1'  ";
                                                    DataView dvStaffAvailMaxDay = dtStaffDet.DefaultView;
                                                    if (dvStaffAvailMaxDay.Count > 2)
                                                    {
                                                        isMaxHourOK = false;
                                                    }
                                                }
                                                if (sbStaffCodes.Length > 1)
                                                {
                                                    sbStaffCodes.Remove(sbStaffCodes.Length - 1, 1);
                                                }

                                                sbSubDisp.Append(subject_codeE + "-" + sbStaffCodes.ToString() + ",");
                                            }
                                        }
                                    }
                                    if (sbSubDisp.Length > 1)
                                        sbSubDisp.Remove(sbSubDisp.Length - 1, 1);

                                    if (sbSubjStaff.Length > 3)
                                        sbSubjStaff.Remove(sbSubjStaff.Length - 3, 3);
                                    sbSubjStaff.Append(") and ");

                                    //if (!isMaxHourOK)
                                    //    continue;

                                    dtStaffDet.DefaultView.RowFilter = sbSubjStaff.ToString() + " DaysFK='" + (dayI + 1) + "' and HoursFK='" + testCol + "' ";
                                    DataView dvStaffAvail = dtStaffDet.DefaultView;
                                    bool IsEngaged = false;

                                    int availElect = electStaffCount;
                                    if (dvStaffAvail.Count > 0)
                                    {
                                        for (int dvI = 0; dvI < dvStaffAvail.Count; dvI++)
                                        {
                                            if (dvStaffAvail[dvI]["IsEngaged"].ToString().ToUpper() == "TRUE")
                                            {
                                                IsEngaged = true;
                                                availElect--;
                                            }
                                        }


                                    }

                                    if (chkMinElect.Checked)
                                    {
                                        if (availElect >= dvSubType.Count)
                                        {
                                            IsEngaged = false;
                                        }
                                    }

                                    #region Staff Theory, Lab, Day maximum hours check

                                    //if (testCol == lastPeriod)
                                    //{
                                    //    dtStaffDet.DefaultView.RowFilter = sbSubjStaff.ToString() + " DaysFK='" + (dayI + 1) + "' and HoursFK='" + (testCol - 1) + "' ";
                                    //    DataView dvStaffAvailMax = dtStaffDet.DefaultView;
                                    //    if (dvStaffAvailMax.Count > 0)
                                    //    {
                                    //        for (int dvI = 0; dvI < dvStaffAvailMax.Count; dvI++)
                                    //        {
                                    //            if (dvStaffAvailMax[dvI]["IsEngaged"].ToString().ToUpper() == "TRUE")
                                    //                IsEngaged = true;
                                    //        }
                                    //    }
                                    //}
                                    //else if (testCol == 1)
                                    //{
                                    //    dtStaffDet.DefaultView.RowFilter = sbSubjStaff.ToString() + " DaysFK='" + (dayI + 1) + "' and HoursFK='" + (testCol + 1) + "' ";
                                    //    DataView dvStaffAvailMax = dtStaffDet.DefaultView;
                                    //    if (dvStaffAvailMax.Count > 0)
                                    //    {
                                    //        for (int dvI = 0; dvI < dvStaffAvailMax.Count; dvI++)
                                    //        {
                                    //            if (dvStaffAvailMax[dvI]["IsEngaged"].ToString().ToUpper() == "TRUE")
                                    //                IsEngaged = true;
                                    //        }
                                    //    }
                                    //}
                                    //else
                                    //{
                                    //    dtStaffDet.DefaultView.RowFilter = sbSubjStaff.ToString() + " DaysFK='" + (dayI + 1) + "' and  (HoursFK='" + (testCol + 1) + "' or  HoursFK='" + (testCol - 1) + "' ) ";
                                    //    DataView dvStaffAvailMax = dtStaffDet.DefaultView;
                                    //    if (dvStaffAvailMax.Count > 0)
                                    //    {
                                    //        for (int dvI = 0; dvI < dvStaffAvailMax.Count; dvI++)
                                    //        {
                                    //            if (dvStaffAvailMax[dvI]["IsEngaged"].ToString().ToUpper() == "TRUE")
                                    //                IsEngaged = true;
                                    //        }
                                    //    }
                                    //}
                                    #endregion

                                    if (!IsEngaged)
                                    {

                                        StringBuilder sbElectRoom = new StringBuilder();
                                        ArrayList arrElectRoom = new ArrayList();
                                        foreach (string roomPKACR in arrlstRoomDet)
                                        {
                                            string[] roomVal = roomPKACR.Split('#');
                                            string hrI = roomPKACR + "$" + (dayI + 1) + "_" + testCol;

                                            bool isRoomFree = false;
                                            if (dicCurRoomAvail.ContainsKey(hrI) && dicCurRoomAvail[hrI] == 0)
                                            {
                                                isRoomFree = true;
                                            }
                                            if (!isRoomFree)
                                                continue;

                                            arrElectRoom.Add(hrI);
                                            sbElectRoom.Append(roomVal[1] + ",");

                                            if (arrElectRoom.Count == electStaffCount)
                                                break;
                                        }

                                        if (arrElectRoom.Count == electStaffCount)
                                        {
                                            if (sbElectRoom.Length > 1)
                                                sbElectRoom.Remove(sbElectRoom.Length - 1, 1);

                                            dtFnlTable.Rows[dayI][colName.ToString()] = sbSubDisp.ToString() + "$" + sbElectRoom.ToString();
                                            allotedHrs++;
                                            NoOfHrsToAllot--;
                                            dicAllocElecSub[subType_no]++;
                                            arrAlreadyAddedRow.Add(subType_no + "E_" + dayI);
                                            arrAlreadyAddedCol.Add(subType_no + "E_" + hrsI);

                                            //Room Engage
                                            for (int i = 0; i < arrElectRoom.Count; i++)
                                            {
                                                string hrI = " #" + arrElectRoom[i] + "$" + (dayI + 1) + "_" + testCol;
                                                dicCurRoomAvail[hrI] = 1;
                                            }

                                        }
                                    }
                                }
                                #endregion
                            }
                            else if (Lab == "TRUE")
                            {
                                if (arrLabAllocToday.Contains(dayI))
                                {
                                    continue;
                                }
                                //Lunch Check
                                if (testCol == 3 && (!string.IsNullOrEmpty(comparingCell5) || !string.IsNullOrEmpty(comparingCell4)))
                                {
                                    continue;
                                }
                                if (testCol == 4)
                                {
                                    continue;
                                }
                                #region Lab Region
                                if (arrAlreadyAddedRow.Contains(subject_no + "_" + dayI) || arrAlreadyAddedCol.Contains(subject_no + "_" + hrsI))
                                {
                                    continue;
                                }


                                string subComName = subject_no;
                                #region Practical Pair Check
                                dtSubjectDet.DefaultView.RowFilter = "practicalPair>0";
                                DataTable dtPairValues = dtSubjectDet.DefaultView.ToTable(true, "practicalPair", "subject_no", "subject_code");

                                int pairValue = Convert.ToInt32(dtSubjectDet.Rows[subI]["practicalPair"]);

                                if (pairValue > 0)
                                {
                                    dtPairValues.DefaultView.RowFilter = "practicalPair='" + pairValue + "'";
                                    DataView dvPair = dtPairValues.DefaultView;
                                    if (dvPair.Count > 0)
                                    {
                                        StringBuilder sbSubCode = new StringBuilder();
                                        StringBuilder sbSubNos = new StringBuilder();
                                        StringBuilder sbSubNo = new StringBuilder();
                                        for (int dvI = 0; dvI < dvPair.Count; dvI++)
                                        {
                                            sbSubNo.Append(dvPair[dvI]["subject_no"].ToString() + "-");
                                            sbSubNos.Append(dvPair[dvI]["subject_no"].ToString() + ",");
                                            sbSubCode.Append(dvPair[dvI]["subject_code"].ToString() + "#");
                                        }
                                        if (sbSubNo.Length > 0)
                                        {
                                            sbSubNo.Remove(sbSubNo.Length - 1, 1);
                                        }
                                        if (sbSubNos.Length > 0)
                                        {
                                            sbSubNos.Remove(sbSubNos.Length - 1, 1);
                                        }
                                        if (sbSubCode.Length > 0)
                                        {
                                            sbSubCode.Remove(sbSubCode.Length - 1, 1);
                                        }
                                        if (!dicSubType.ContainsKey(sbSubNo.ToString()))
                                        {
                                            dicSubType.Add(sbSubNo.ToString(), (Lab == "TRUE" ? true : false));

                                            subject_no = sbSubNos.ToString();
                                            subComName = sbSubNo.ToString();
                                            //subject_code = sbSubCode.ToString();
                                        }
                                        else
                                        {
                                            continue;
                                        }
                                    }
                                }
                                #endregion

                                string staffs = Convert.ToString(dtFacultyChoices.Rows[facChoiceIndex][subComName]);
                                string[] staffSub = staffs.Split(',');

                                //Loop through staff and availability


                                if (staffSub.ToString().Trim().Length > 0)
                                {
                                    string staff_code = staffSub[0].Split('-')[0];

                                    dtSubjectDetWt.DefaultView.RowFilter = " staff_code in ('" + staff_code + "') and practicalPair='" + pairValue + "'  and subject_no in ('" + staffSub[0].Split('-')[1] + "')";
                                    DataView dvSubjectStaff = dtSubjectDetWt.DefaultView;

                                    string staff_appno = Convert.ToString(dvSubjectStaff[0]["appl_id"]);

                                    string staff_code2 = staff_code;
                                    string staff_appno2 = Convert.ToString(dvSubjectStaff[0]["appl_id"]);
                                    subject_code = Convert.ToString(dvSubjectStaff[0]["subject_code"]);


                                    dtStaffDet.DefaultView.RowFilter = "staff_appno in (" + staff_appno + ") and DaysFK='" + (dayI + 1) + "' and MaxHour='2'  ";
                                    DataView dvStaffAvailMaxDay = dtStaffDet.DefaultView;
                                    if (dvStaffAvailMaxDay.Count > 1)
                                    {
                                        continue;
                                    }
                                    if (staffSub.Length > 1)
                                    {
                                        staff_code2 = staffSub[1].Split('-')[0];

                                        dtSubjectDetWt.DefaultView.RowFilter = " staff_code in ('" + staff_code2 + "')  and practicalPair='" + pairValue + "'  and subject_no in ('" + staffSub[1].Split('-')[1] + "')";
                                        DataView dvSubjectStaff1 = dtSubjectDetWt.DefaultView;

                                        staff_appno2 = Convert.ToString(dvSubjectStaff1[0]["appl_id"]);
                                        subject_code += "#" + Convert.ToString(dvSubjectStaff1[0]["subject_code"]);

                                        dtStaffDet.DefaultView.RowFilter = "staff_appno in (" + staff_appno2 + ") and DaysFK='" + (dayI + 1) + "' and MaxHour='1'  ";
                                        DataView dvStaffAvailMaxDay1 = dtStaffDet.DefaultView;
                                        if (dvStaffAvailMaxDay1.Count > 1)
                                        {
                                            continue;
                                        }
                                    }
                                    byte nextCol = 0;

                                    if ((hrsI + 1) < dtFnlTable.Columns.Count)
                                    {
                                        string colName2 = Convert.ToString(dtFnlTable.Columns[hrsI + 1].ColumnName).Trim();
                                        if (byte.TryParse(dtFnlTable.Columns[hrsI + 1].ColumnName, out nextCol) && Convert.ToString(dtFnlTable.Rows[dayI][colName2]).Trim() == string.Empty)
                                        {
                                            if (arrAlreadyAddedCol.Contains(subject_no + "_" + (testCol + 1)))
                                            {
                                                continue;
                                            }

                                            dtStaffDet.DefaultView.RowFilter = "(staff_appno = '" + staff_appno + "' or staff_appno = '" + staff_appno2 + "') and DaysFK='" + (dayI + 1) + "' and HoursFK='" + testCol + "' ";
                                            DataView dvStaffAvail = dtStaffDet.DefaultView;
                                            bool IsEngaged = false;
                                            if (dvStaffAvail.Count > 0)
                                            {
                                                if (dvStaffAvail[0]["IsEngaged"].ToString().ToUpper() == "TRUE")
                                                    IsEngaged = true;
                                                if (dvStaffAvail.Count > 1 && dvStaffAvail[1]["IsEngaged"].ToString().ToUpper() == "TRUE")
                                                    IsEngaged = true;
                                            }

                                            if (!IsEngaged)
                                            {
                                                #region Check For Next Hour EngageMent
                                                IsEngaged = false;
                                                dtStaffDet.DefaultView.RowFilter = "(staff_appno = '" + staff_appno + "' or staff_appno ='" + staff_appno2 + "') and DaysFK='" + (dayI + 1) + "' and HoursFK='" + (testCol + 1) + "' ";
                                                DataView dvStaffAvailNext = dtStaffDet.DefaultView;

                                                if (dvStaffAvailNext.Count > 0)
                                                {
                                                    if (dvStaffAvailNext[0]["IsEngaged"].ToString().ToUpper() == "TRUE")
                                                        IsEngaged = true;
                                                    if (dvStaffAvailNext.Count > 1 && dvStaffAvailNext[1]["IsEngaged"].ToString().ToUpper() == "TRUE")
                                                        IsEngaged = true;
                                                }
                                                #endregion
                                                if (!IsEngaged)
                                                {
                                                    foreach (string roomPKACR in arrlstLabDet)
                                                    {
                                                        string[] roomVal = roomPKACR.Split('#');
                                                        string hrI = roomPKACR + "$" + (dayI + 1) + "_" + testCol;
                                                        string hrII = roomPKACR + "$" + (dayI + 1) + "_" + (testCol + 1);

                                                        bool isRoomFree = false;
                                                        if (dicCurRoomAvail.ContainsKey(hrI) && dicCurRoomAvail.ContainsKey(hrII))
                                                        {
                                                            if (dicCurRoomAvail[hrI] == 0 && dicCurRoomAvail[hrII] == 0)
                                                            {
                                                                isRoomFree = true;
                                                            }
                                                        }
                                                        if (!isRoomFree)
                                                            continue;

                                                        dtFnlTable.Rows[dayI][dtFnlTable.Columns[(hrsI)].ColumnName] = subject_code + "-" + staff_code + "/" + staff_code2 + "$" + roomVal[1];
                                                        dtFnlTable.Rows[dayI][dtFnlTable.Columns[(hrsI + 1)].ColumnName] = subject_code + "-" + staff_code + "/" + staff_code2 + "$" + roomVal[1];
                                                        allotedHrs += 2;
                                                        NoOfHrsToAllot -= 2;
                                                        string[] sub_nosl = subject_no.Split(',');
                                                        foreach (string subnol in sub_nosl)
                                                        {
                                                            if (!dicAllocSub.ContainsKey(subnol))
                                                            {
                                                                dicAllocSub.Add(subnol, 0);
                                                            }
                                                            dicAllocSub[subnol] += 2;
                                                            arrAlreadyAddedRow.Add(subnol + "_" + dayI);
                                                            arrAlreadyAddedCol.Add(subnol + "_" + testCol);
                                                            arrAlreadyAddedCol.Add(subnol + "_" + (testCol + 1));
                                                        }

                                                        if (!arrLabAllocToday.Contains(dayI))
                                                        {
                                                            arrLabAllocToday.Add(dayI);
                                                        }

                                                        dicCurRoomAvail[hrI] = 1;
                                                        dicCurRoomAvail[hrII] = 1;
                                                        break;
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                                #endregion
                            }
                        }
                        #region Switch Subject Location
                        //dtSubjectDet.DefaultView.RowFilter = "ElectivePap='True'";
                        //DataTable dtSubjectElectDet = dtSubjectDet.DefaultView.ToTable();
                        //dtSubjectDet.DefaultView.RowFilter = "ElectivePap<>'True'";
                        //DataTable dtSubjectTheoryLabDet = dtSubjectDet.DefaultView.ToTable();

                        //DataRow dr = dtSubjectTheoryLabDet.NewRow();
                        //foreach (DataColumn dc in dtSubjectTheoryLabDet.Columns)
                        //{
                        //    dr[dc] = dtSubjectTheoryLabDet.Rows[dtSubjectTheoryLabDet.Rows.Count - 1][dc];
                        //}
                        //dtSubjectTheoryLabDet.Rows.RemoveAt(dtSubjectTheoryLabDet.Rows.Count - 1);
                        //dtSubjectTheoryLabDet.Rows.InsertAt(dr, 0);

                        //dtSubjectDet.Clear();
                        //if (hrsI % 2 == 1)
                        //{
                        //    dtSubjectDet.Merge(dtSubjectTheoryLabDet);
                        //    dtSubjectDet.Merge(dtSubjectElectDet);
                        //}
                        //else
                        //{
                        //    dtSubjectDet.Merge(dtSubjectElectDet);
                        //    dtSubjectDet.Merge(dtSubjectTheoryLabDet);
                        //}
                        #endregion
                    }
                }
            }

            //if (NoOfHrsToAllot != 0)
            //{
            //    if (fromRestart)
            //    {
            //        if (!chkShowPart.Checked || !chkShowPart2.Checked)
            //        {
            //            if (NoOfHrsToAllot > 1)
            //            {
            //                //goto restartLoop;
            //                dtFnlTable.Clear();
            //            }
            //        }
            //        else
            //        {
            //            dtFnlTable.Clear();
            //        }
            //    }
            //    else
            //    {
            //        fromRestart = true;
            //        goto restartLoop;
            //    }
            //}
            dtTimeTable = dtFnlTable;

        }
        catch { dtTimeTable.Clear(); }
        return dtTimeTable;
    }
    //Regeneration Allocation    
    private DataTable getTimeTableFormatRegenerate(int batchYear, int degreeCode, int currentSem, string dispText, DataTable dtSubjectDet, DataTable dtSubjectDetWt, DataTable dtFacultyChoices, int facChoiceIndex, int maxNoCanAllot, int noOfHrsPerDay, DataTable dtBellSchedule, DataTable dtCriteria, ref DataTable dtStaffDet, Dictionary<string, int> dicRoomAvailability, ArrayList arrlstRoomDet, ArrayList arrlstLabDet, ArrayList arrAlreadyAddedRowCol, Hashtable htElectPreAllot, Hashtable htElectRooms)
    {
        DataTable dtSpclStaff = dirAccess.selectDataTable("SELECT STAFF_CODE,HOURPK FROM TT_StudentCriteria WHERE DAYPK='0'");
        DataTable dtTimeTable = new DataTable();
        try
        {
            //Total No of Hours to Allot for a week
            int NoOfHrsToAllot = maxNoCanAllot;
            NoOfHrsToAllot -= 5;

            DataTable dtCurStaffDet = dtStaffDet.Copy();
            dtCurStaffDet.Clear();
            Dictionary<string, int> dicCurRoomAvail = new Dictionary<string, int>();
            foreach (KeyValuePair<string, int> roomVal in dicRoomAvailability)
            {
                dicCurRoomAvail.Add(roomVal.Key, roomVal.Value);
            }

            DataTable dtFnlTable = new DataTable();
            dtFnlTable.Columns.Add("Day/Period");

            dtFnlTable.Rows.Add("Monday");
            dtFnlTable.Rows.Add("Tuesday");
            dtFnlTable.Rows.Add("Wednesday");
            dtFnlTable.Rows.Add("Thursday");
            dtFnlTable.Rows.Add("Friday");
            dtFnlTable.Rows.Add("");
            for (int periodI = 0; periodI < dtBellSchedule.Rows.Count; periodI++)
            {
                dtFnlTable.Columns.Add(Convert.ToString(dtBellSchedule.Rows[periodI]["Period1"]).Trim());
                dtFnlTable.Rows[5][(periodI + 1)] = Convert.ToString(dtBellSchedule.Rows[periodI]["start_time"]).Trim() + "- " + Convert.ToString(dtBellSchedule.Rows[periodI]["end_time"]).Trim();
            }
            int lastPeriod = Convert.ToInt32(dtFnlTable.Columns[dtFnlTable.Columns.Count - 1].ColumnName);

            //Students Already Engaged in Options
            //NoOfHrsToAllot += 4;
            for (int optI = 0; optI < dtCriteria.Rows.Count; optI++)
            {
                #region Old 25-02-2017
                //byte DaysFk = Convert.ToByte(dtCriteria.Rows[optI]["DayPk"]);
                //byte HrsFk = Convert.ToByte(dtCriteria.Rows[optI]["HourPk"]);
                //string IsEngaged = Convert.ToString(dtCriteria.Rows[optI]["IsEngaged"]).Trim().ToUpper();
                //dtFnlTable.Rows[(DaysFk - 1)][HrsFk.ToString()] = "ENGAGED";
                ////NoOfHrsToAllot--; 
                #endregion

                byte DaysFk = Convert.ToByte(dtCriteria.Rows[optI]["DayPk"]);
                byte HrsFk = Convert.ToByte(dtCriteria.Rows[optI]["HourPk"]);
                string IsEngaged = Convert.ToString(dtCriteria.Rows[optI]["IsEngaged"]).Trim().ToUpper();
                string subject_code = Convert.ToString(dtCriteria.Rows[optI]["criterianame"]).Trim();

                foreach (string roomPKACR in arrlstRoomDet)
                {
                    string[] roomVal = roomPKACR.Split('#');
                    string hrI = roomPKACR + "$" + (DaysFk) + "_" + HrsFk;


                    bool isRoomFree = false;
                    if (dicCurRoomAvail.ContainsKey(hrI) && dicCurRoomAvail[hrI] == 0)
                    {
                        isRoomFree = true;

                    }
                    if (!isRoomFree)
                        continue;

                    dtFnlTable.Rows[(DaysFk - 1)][HrsFk.ToString()] = subject_code + "$" + roomVal[1];
                    //NoOfHrsToAllot--;
                    dicCurRoomAvail[hrI] = 1;
                    break;
                }
            }

            //if (ddlCriteriaReduced.Items.Count == 0)//If No options available
            //    NoOfHrsToAllot -= 4;

            Dictionary<string, byte> dicAllocSub = new Dictionary<string, byte>();
            Dictionary<string, byte> dicAllocElecSub = new Dictionary<string, byte>();
            ArrayList arrLabAllocToday = new ArrayList();

            ArrayList arrAlreadyAddedRow = new ArrayList();
            ArrayList arrAlreadyAddedCol = new ArrayList();//To Check whether already added in the row & column

            #region Fill Elective

            foreach (DictionaryEntry dicElective in htElectPreAllot)
            {
                string[] resultValues = dicElective.Key.ToString().Split('_');//0-subjecttypeno_1-days_2-hours
                if (resultValues.Length == 3)
                {

                    if (Convert.ToString(dtFnlTable.Rows[Convert.ToInt16(resultValues[1])][resultValues[2]]) == string.Empty)
                    {
                        string subType_no = resultValues[0];

                        StringBuilder sbSubDisp = new StringBuilder();
                        StringBuilder sbSubjStaff = new StringBuilder();
                        sbSubjStaff.Append("(");

                        dtSubjectDet.DefaultView.RowFilter = " subType_no = '" + subType_no + "'";
                        DataView dvSubType = dtSubjectDet.DefaultView;
                        int electStaffCount = 0;
                        if (dvSubType.Count > 0)
                        {
                            for (int electSubI = 0; electSubI < dvSubType.Count; electSubI++)
                            {
                                string subject_noE = Convert.ToString(dvSubType[electSubI]["subject_no"]).Trim();
                                string subject_codeE = Convert.ToString(dvSubType[electSubI]["subject_code"]);

                                //Loop through staff and availability
                                dtSubjectDetWt.DefaultView.RowFilter = "subject_no='" + subject_noE + "' ";// and staff_code='" + Convert.ToString(dtFacultyChoices.Rows[facChoiceIndex][subject_noE]) + "'
                                DataView dvSubjectStaff = dtSubjectDetWt.DefaultView;
                                dvSubjectStaff.Sort = " staffPriority asc";
                                if (dvSubjectStaff.Count > 0)
                                {
                                    StringBuilder sbStaffCodes = new StringBuilder();
                                    for (int stfI = 0; stfI < dvSubjectStaff.Count; stfI++)
                                    {
                                        string staff_code = Convert.ToString(dvSubjectStaff[stfI]["staff_code"]);
                                        string staff_appno = Convert.ToString(dvSubjectStaff[stfI]["appl_id"]);
                                        sbStaffCodes.Append(staff_code + "/");
                                        sbSubjStaff.Append(" staff_appno = '" + staff_appno + "' or ");
                                        electStaffCount++;
                                    }
                                    if (sbStaffCodes.Length > 1)
                                    {
                                        sbStaffCodes.Remove(sbStaffCodes.Length - 1, 1);
                                    }

                                    sbSubDisp.Append(subject_codeE + "-" + sbStaffCodes.ToString() + ",");
                                }
                            }

                            if (sbSubDisp.Length > 1)
                                sbSubDisp.Remove(sbSubDisp.Length - 1, 1);

                            if (htElectRooms.Contains(subType_no))
                                sbSubDisp.Append("$" + htElectRooms[subType_no].ToString());

                            if (sbSubjStaff.Length > 3)
                                sbSubjStaff.Remove(sbSubjStaff.Length - 3, 3);
                            sbSubjStaff.Append(") and ");

                            NoOfHrsToAllot--;
                            dtFnlTable.Rows[Convert.ToInt16(resultValues[1])][resultValues[2]] = sbSubDisp.ToString();
                        }
                    }
                }
            }

            #endregion

            bool fromRestart = false;
        restartLoop:
            //Loop through each day and hour


            //List<byte> lstDays = new List<byte>();
            //byte daysCount = (byte)(dtFnlTable.Rows.Count - 1);
            //for (int dayI = 0; dayI < daysCount; dayI++)
            //{
            //    lstDays.Add(getRandomDay(daysCount, lstDays));
            //}

            for (int dayI = 0; dayI < (dtFnlTable.Rows.Count - 1); dayI++)
            //for (int dI = 0; dI < lstDays.Count; dI++)
            {
                // int dayI = lstDays[dI];

                //List<byte> lstHours = new List<byte>();
                //byte periodsCount = (byte)dtFnlTable.Columns.Count;//noOfHrsPerDay;
                //for (int perI = 1; perI < periodsCount; perI++)
                //{
                //    lstHours.Add(getRandomPeriod(periodsCount, lstHours));
                //}
                //Loop through Hours
                for (int hrsI = 1; hrsI < (dtFnlTable.Columns.Count); hrsI++)
                //for (int lstI = 0; lstI < lstHours.Count; lstI++)
                {
                    //byte hrsI = lstHours[lstI];
                    Dictionary<string, bool> dicSubType = new Dictionary<string, bool>();
                    //Loop through subjects
                    for (int subI = 0; subI < dtSubjectDet.Rows.Count; subI++)
                    {
                        string colName = Convert.ToString(dtFnlTable.Columns[hrsI].ColumnName).Trim();
                        byte testCol = 0;
                        if (byte.TryParse(dtFnlTable.Columns[hrsI].ColumnName, out testCol) && Convert.ToString(dtFnlTable.Rows[dayI][colName]).Trim() == string.Empty)
                        {
                            #region Check for Lunch Availability
                            string comparingCell4 = Convert.ToString(dtFnlTable.Rows[dayI]["4"]).Trim();
                            string comparingCell5 = Convert.ToString(dtFnlTable.Rows[dayI]["5"]).Trim();

                            if (testCol == 4 && (!string.IsNullOrEmpty(comparingCell5)))
                            {
                                continue;
                            }
                            if (testCol == 5 && (!string.IsNullOrEmpty(comparingCell4)))
                            {
                                continue;
                            }
                            #endregion

                            string subject_no = Convert.ToString(dtSubjectDet.Rows[subI]["subject_no"]).Trim();
                            byte noofhrsperweek = Convert.ToByte(dtSubjectDet.Rows[subI]["noofhrsperweek"]);
                            string subject_code = Convert.ToString(dtSubjectDet.Rows[subI]["subject_code"]);
                            string ElectivePap = Convert.ToString(dtSubjectDet.Rows[subI]["ElectivePap"]).Trim().ToUpper();
                            string Lab = Convert.ToString(dtSubjectDet.Rows[subI]["Lab"]).Trim().ToUpper();
                            byte allotedHrs = 0;

                            //Check For existance
                            if (ElectivePap == "TRUE")
                            {
                            }
                            else if (dicAllocSub.ContainsKey(subject_no))
                            {
                                allotedHrs = dicAllocSub[subject_no];
                                if (allotedHrs >= noofhrsperweek)
                                    continue;
                            }
                            else
                            {
                                dicAllocSub.Add(subject_no, allotedHrs);
                            }

                            if (Lab != "TRUE" && ElectivePap != "TRUE")
                            {
                                #region Theory Region
                                if (!fromRestart)
                                    if (arrAlreadyAddedRow.Contains(subject_no + "_" + dayI))// || arrAlreadyAddedCol.Contains(subject_no + "_" + hrsI)
                                    {
                                        continue;
                                    }


                                //Loop through staff and availability
                                dtSubjectDetWt.DefaultView.RowFilter = "subject_no='" + subject_no + "' and staff_code='" + Convert.ToString(dtFacultyChoices.Rows[facChoiceIndex][subject_no]) + "'";
                                DataView dvSubjectStaff = dtSubjectDetWt.DefaultView;
                                dvSubjectStaff.Sort = " staffPriority asc";
                                if (dvSubjectStaff.Count > 0)
                                {
                                    string staff_code = Convert.ToString(dvSubjectStaff[0]["staff_code"]);
                                    string staff_appno = Convert.ToString(dvSubjectStaff[0]["appl_id"]);

                                    #region Special Staff
                                    dtSpclStaff.DefaultView.RowFilter = "staff_code='" + staff_code + "'";
                                    DataView dvIsSpecial = dtSpclStaff.DefaultView;
                                    if (dvIsSpecial.Count > 0)
                                    {
                                        DataTable dtSubHr = dvIsSpecial.ToTable();
                                        dtSubHr.DefaultView.RowFilter = "hourpk='" + hrsI + "'";
                                        DataView dvNHr = dtSubHr.DefaultView;
                                        if (dvNHr.Count == 0)
                                        {
                                            continue;
                                        }
                                    }
                                    else
                                    {

                                        //if (arrAlreadyAddedRow.Contains(subject_no + "_" + (dayI - 1)) && arrAlreadyAddedCol.Contains(subject_no + "_" + hrsI))
                                        //{
                                        //    continue;
                                        //}

                                        if (!fromRestart)
                                            if (arrAlreadyAddedCol.Contains(subject_no + "_" + hrsI))//|| 
                                            {
                                                continue;
                                            }
                                    }
                                    #endregion

                                    dtStaffDet.DefaultView.RowFilter = "staff_appno in (" + staff_appno + ") and DaysFK='" + (dayI + 1) + "' and HoursFK='" + testCol + "' ";
                                    DataView dvStaffAvail = dtStaffDet.DefaultView;
                                    bool IsEngaged = false;
                                    if (dvStaffAvail.Count > 0)
                                    {
                                        if (dvStaffAvail[0]["IsEngaged"].ToString().ToUpper() == "TRUE")
                                            IsEngaged = true;
                                    }

                                    #region Staff Theory, Lab, Day maximum hours check

                                    //if (testCol == lastPeriod)
                                    //{
                                    //    dtStaffDet.DefaultView.RowFilter = "staff_appno in (" + staff_appno + ") and DaysFK='" + (dayI + 1) + "' and HoursFK='" + (testCol - 1) + "' ";
                                    //    DataView dvStaffAvailMax = dtStaffDet.DefaultView;
                                    //    if (dvStaffAvailMax.Count > 0)
                                    //    {
                                    //        if (dvStaffAvailMax[0]["IsEngaged"].ToString().ToUpper() == "TRUE")
                                    //            IsEngaged = true;
                                    //    }
                                    //}
                                    //else if (testCol == 1)
                                    //{
                                    //    dtStaffDet.DefaultView.RowFilter = "staff_appno in (" + staff_appno + ") and DaysFK='" + (dayI + 1) + "' and HoursFK='" + (testCol + 1) + "' ";
                                    //    DataView dvStaffAvailMax = dtStaffDet.DefaultView;
                                    //    if (dvStaffAvailMax.Count > 0)
                                    //    {
                                    //        if (dvStaffAvailMax[0]["IsEngaged"].ToString().ToUpper() == "TRUE")
                                    //            IsEngaged = true;
                                    //    }
                                    //}
                                    //else
                                    //{
                                    //    dtStaffDet.DefaultView.RowFilter = "staff_appno in (" + staff_appno + ") and DaysFK='" + (dayI + 1) + "' and (HoursFK='" + (testCol + 1) + "' or  HoursFK='" + (testCol - 1) + "' )";
                                    //    DataView dvStaffAvailMax = dtStaffDet.DefaultView;
                                    //    if (dvStaffAvailMax.Count > 0)
                                    //    {
                                    //        if (dvStaffAvailMax[0]["IsEngaged"].ToString().ToUpper() == "TRUE")
                                    //            IsEngaged = true;
                                    //    }
                                    //}

                                    //dtStaffDet.DefaultView.RowFilter = "staff_appno in (" + staff_appno + ") and DaysFK='" + (dayI + 1) + "' and MaxHour='1'  ";
                                    //DataView dvStaffAvailMaxDay = dtStaffDet.DefaultView;
                                    //if (dvStaffAvailMaxDay.Count > 2)
                                    //{
                                    //    continue;
                                    //}
                                    #endregion

                                    if (!IsEngaged)
                                    {
                                        foreach (string roomPKACR in arrlstRoomDet)
                                        {
                                            string[] roomVal = roomPKACR.Split('#');
                                            string hrI = roomPKACR + "$" + (dayI + 1) + "_" + testCol;


                                            bool isRoomFree = false;
                                            if (dicCurRoomAvail.ContainsKey(hrI) && dicCurRoomAvail[hrI] == 0)
                                            {
                                                isRoomFree = true;

                                            }
                                            if (!isRoomFree)
                                                continue;

                                            dtFnlTable.Rows[dayI][colName] = subject_code + "-" + staff_code + "$" + roomVal[1];
                                            allotedHrs++;
                                            NoOfHrsToAllot--;
                                            dicAllocSub[subject_no]++;
                                            arrAlreadyAddedRow.Add(subject_no + "_" + dayI);
                                            arrAlreadyAddedCol.Add(subject_no + "_" + hrsI);

                                            dicCurRoomAvail[hrI] = 1;
                                            break;
                                        }
                                    }
                                }
                                #endregion
                            }
                            else if (ElectivePap == "TRUE")
                            {
                            }
                            else if (Lab == "TRUE")
                            {
                                if (arrLabAllocToday.Contains(dayI))
                                {
                                    continue;
                                }
                                //Lunch Check
                                if (testCol == 3 && (!string.IsNullOrEmpty(comparingCell5) || !string.IsNullOrEmpty(comparingCell4)))
                                {
                                    continue;
                                }
                                if (testCol == 4)
                                {
                                    continue;
                                }
                                #region Lab Region


                                string subComName = subject_no;
                                #region Practical Pair Check
                                dtSubjectDet.DefaultView.RowFilter = "practicalPair>0";
                                DataTable dtPairValues = dtSubjectDet.DefaultView.ToTable(true, "practicalPair", "subject_no", "subject_code");

                                int pairValue = Convert.ToInt32(dtSubjectDet.Rows[subI]["practicalPair"]);

                                if (pairValue > 0)
                                {
                                    dtPairValues.DefaultView.RowFilter = "practicalPair='" + pairValue + "'";
                                    DataView dvPair = dtPairValues.DefaultView;
                                    if (dvPair.Count > 0)
                                    {
                                        StringBuilder sbSubCode = new StringBuilder();
                                        StringBuilder sbSubNos = new StringBuilder();
                                        StringBuilder sbSubNo = new StringBuilder();
                                        for (int dvI = 0; dvI < dvPair.Count; dvI++)
                                        {
                                            sbSubNo.Append(dvPair[dvI]["subject_no"].ToString() + "-");
                                            sbSubNos.Append(dvPair[dvI]["subject_no"].ToString() + ",");
                                            sbSubCode.Append(dvPair[dvI]["subject_code"].ToString() + "#");
                                        }
                                        if (sbSubNo.Length > 0)
                                        {
                                            sbSubNo.Remove(sbSubNo.Length - 1, 1);
                                        }
                                        if (sbSubNos.Length > 0)
                                        {
                                            sbSubNos.Remove(sbSubNos.Length - 1, 1);
                                        }
                                        if (sbSubCode.Length > 0)
                                        {
                                            sbSubCode.Remove(sbSubCode.Length - 1, 1);
                                        }
                                        if (!dicSubType.ContainsKey(sbSubNo.ToString()))
                                        {
                                            dicSubType.Add(sbSubNo.ToString(), (Lab == "TRUE" ? true : false));

                                            subject_no = sbSubNos.ToString();
                                            subComName = sbSubNo.ToString();
                                            //subject_code = sbSubCode.ToString();
                                        }
                                        else
                                        {
                                            continue;
                                        }
                                    }
                                }
                                #endregion

                                string staffs = Convert.ToString(dtFacultyChoices.Rows[facChoiceIndex][subComName]);
                                string[] staffSub = staffs.Split(',');

                                //Loop through staff and availability

                                if (staffSub.Length > 0)
                                {
                                    string staff_code = staffSub[0].Split('-')[0];

                                    dtSubjectDetWt.DefaultView.RowFilter = " staff_code in ('" + staff_code + "') and practicalPair='" + pairValue + "'  and subject_no in ('" + staffSub[0].Split('-')[1] + "')";
                                    DataView dvSubjectStaff = dtSubjectDetWt.DefaultView;

                                    string staff_appno = Convert.ToString(dvSubjectStaff[0]["appl_id"]);

                                    string staff_code2 = staff_code;
                                    string staff_appno2 = Convert.ToString(dvSubjectStaff[0]["appl_id"]);
                                    subject_code = Convert.ToString(dvSubjectStaff[0]["subject_code"]);

                                    dtStaffDet.DefaultView.RowFilter = "staff_appno in (" + staff_appno + ") and DaysFK='" + (dayI + 1) + "' and MaxHour='2'  ";
                                    DataView dvStaffAvailMaxDay = dtStaffDet.DefaultView;
                                    if (dvStaffAvailMaxDay.Count > 1)
                                    {
                                        continue;
                                    }

                                    if (staffSub.Length > 1)
                                    {
                                        staff_code2 = staffSub[1].Split('-')[0];

                                        dtSubjectDetWt.DefaultView.RowFilter = " staff_code in ('" + staff_code2 + "')  and practicalPair='" + pairValue + "'  and subject_no in ('" + staffSub[1].Split('-')[1] + "')";
                                        DataView dvSubjectStaff1 = dtSubjectDetWt.DefaultView;

                                        staff_appno2 = Convert.ToString(dvSubjectStaff1[0]["appl_id"]);
                                        subject_code += "#" + Convert.ToString(dvSubjectStaff1[0]["subject_code"]);

                                        dtStaffDet.DefaultView.RowFilter = "staff_appno in (" + staff_appno2 + ") and DaysFK='" + (dayI + 1) + "' and MaxHour='2'  ";
                                        DataView dvStaffAvailMaxDay1 = dtStaffDet.DefaultView;
                                        if (dvStaffAvailMaxDay1.Count > 1)
                                        {
                                            continue;
                                        }
                                    }

                                    byte nextCol = 0;

                                    if ((hrsI + 1) < dtFnlTable.Columns.Count)
                                    {
                                        string colName2 = Convert.ToString(dtFnlTable.Columns[hrsI + 1].ColumnName).Trim();
                                        if (byte.TryParse(dtFnlTable.Columns[hrsI + 1].ColumnName, out nextCol) && Convert.ToString(dtFnlTable.Rows[dayI][colName2]).Trim() == string.Empty)
                                        {

                                            dtStaffDet.DefaultView.RowFilter = "(staff_appno = '" + staff_appno + "' or staff_appno = '" + staff_appno2 + "') and DaysFK='" + (dayI + 1) + "' and HoursFK='" + testCol + "' ";
                                            DataView dvStaffAvail = dtStaffDet.DefaultView;
                                            bool IsEngaged = false;
                                            if (dvStaffAvail.Count > 0)
                                            {
                                                if (dvStaffAvail[0]["IsEngaged"].ToString().ToUpper() == "TRUE")
                                                    IsEngaged = true;
                                                if (dvStaffAvail.Count > 1 && dvStaffAvail[1]["IsEngaged"].ToString().ToUpper() == "TRUE")
                                                    IsEngaged = true;
                                            }

                                            if (!IsEngaged)
                                            {
                                                #region Check For Next Hour EngageMent
                                                IsEngaged = false;
                                                dtStaffDet.DefaultView.RowFilter = "(staff_appno = '" + staff_appno + "' or staff_appno ='" + staff_appno2 + "') and DaysFK='" + (dayI + 1) + "' and HoursFK='" + (testCol + 1) + "' ";
                                                DataView dvStaffAvailNext = dtStaffDet.DefaultView;

                                                if (dvStaffAvailNext.Count > 0)
                                                {
                                                    if (dvStaffAvailNext[0]["IsEngaged"].ToString().ToUpper() == "TRUE")
                                                        IsEngaged = true;
                                                    if (dvStaffAvailNext.Count > 1 && dvStaffAvailNext[1]["IsEngaged"].ToString().ToUpper() == "TRUE")
                                                        IsEngaged = true;
                                                }
                                                #endregion
                                                if (!IsEngaged)
                                                {
                                                    foreach (string roomPKACR in arrlstLabDet)
                                                    {
                                                        string[] roomVal = roomPKACR.Split('#');
                                                        string hrI = roomPKACR + "$" + (dayI + 1) + "_" + testCol;
                                                        string hrII = roomPKACR + "$" + (dayI + 1) + "_" + (testCol + 1);

                                                        bool isRoomFree = false;
                                                        if (dicCurRoomAvail.ContainsKey(hrI) && dicCurRoomAvail.ContainsKey(hrII))
                                                        {
                                                            if (dicCurRoomAvail[hrI] == 0 && dicCurRoomAvail[hrII] == 0)
                                                            {
                                                                isRoomFree = true;
                                                            }
                                                        }
                                                        if (!isRoomFree)
                                                            continue;


                                                        if (arrAlreadyAddedRowCol.Contains(subject_no + "$" + roomVal[1] + "_" + dayI + "_" + hrsI) || arrAlreadyAddedRowCol.Contains(subject_no + "$" + roomVal[1] + "_" + dayI + "_" + (testCol + 1)))
                                                        {
                                                            continue;
                                                        }
                                                        //if (arrAlreadyAddedRowCol.Contains(subject_no + "$" + roomVal[1] + "_" + dayI + "_" + (testCol + 1)))
                                                        //{
                                                        //    continue;
                                                        //}

                                                        dtFnlTable.Rows[dayI][dtFnlTable.Columns[(hrsI)].ColumnName] = subject_code + "-" + staff_code + "/" + staff_code2 + "$" + roomVal[1];
                                                        dtFnlTable.Rows[dayI][dtFnlTable.Columns[(hrsI + 1)].ColumnName] = subject_code + "-" + staff_code + "/" + staff_code2 + "$" + roomVal[1];
                                                        allotedHrs += 2;
                                                        NoOfHrsToAllot -= 2;

                                                        string[] sub_nosl = subject_no.Split(',');
                                                        foreach (string subnol in sub_nosl)
                                                        {
                                                            if (!dicAllocSub.ContainsKey(subnol))
                                                            {
                                                                dicAllocSub.Add(subnol, 0);
                                                            }
                                                            dicAllocSub[subnol] += 2;
                                                            arrAlreadyAddedRowCol.Add(subnol + "$" + roomVal[1] + "_" + dayI + "_" + hrsI);
                                                            arrAlreadyAddedRowCol.Add(subnol + "$" + roomVal[1] + "_" + dayI + "_" + (testCol + 1));
                                                        }


                                                        if (!arrLabAllocToday.Contains(dayI))
                                                        {
                                                            arrLabAllocToday.Add(dayI);
                                                        }

                                                        dicCurRoomAvail[hrI] = 1;
                                                        dicCurRoomAvail[hrII] = 1;
                                                        break;
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                                #endregion
                            }
                        }
                    }
                }
            }
            if (NoOfHrsToAllot != 0)
            {
                if (fromRestart)
                {
                    if (!chkShowPart.Checked || !chkShowPart2.Checked)
                    {
                        if (NoOfHrsToAllot > 1)
                        {
                            //goto restartLoop;
                            dtFnlTable.Clear();
                        }
                    }
                    else
                    {
                        dtFnlTable.Clear();
                    }
                }
                else
                {
                    fromRestart = true;
                    goto restartLoop;
                }
            }
            dtTimeTable = dtFnlTable;
        }
        catch { dtTimeTable.Clear(); }
        return dtTimeTable;
    }
    //Regeneration Allocation Unfill    
    private DataTable getTimeTableFormatRegenerateUnfill(int batchYear, int degreeCode, int currentSem, string dispText, DataTable dtSubjectDet, DataTable dtSubjectDetWt, DataTable dtFacultyChoices, int facChoiceIndex, int maxNoCanAllot, int noOfHrsPerDay, DataTable dtBellSchedule, DataTable dtCriteria, ref DataTable dtStaffDet, Dictionary<string, int> dicRoomAvailability, ArrayList arrlstRoomDet, ArrayList arrlstLabDet, ArrayList arrAlreadyAddedRowCol, Hashtable htElectPreAllot, Hashtable htElectRooms)
    {
        DataTable dtSpclStaff = dirAccess.selectDataTable("SELECT STAFF_CODE,HOURPK FROM TT_StudentCriteria WHERE DAYPK='0'");
        DataTable dtTimeTable = new DataTable();
        try
        {
            //Total No of Hours to Allot for a week
            int NoOfHrsToAllot = maxNoCanAllot;
            NoOfHrsToAllot -= 5;

            DataTable dtCurStaffDet = dtStaffDet.Copy();
            dtCurStaffDet.Clear();
            Dictionary<string, int> dicCurRoomAvail = new Dictionary<string, int>();
            foreach (KeyValuePair<string, int> roomVal in dicRoomAvailability)
            {
                dicCurRoomAvail.Add(roomVal.Key, roomVal.Value);
            }

            DataTable dtFnlTable = new DataTable();
            dtFnlTable.Columns.Add("Day/Period");

            dtFnlTable.Rows.Add("Monday");
            dtFnlTable.Rows.Add("Tuesday");
            dtFnlTable.Rows.Add("Wednesday");
            dtFnlTable.Rows.Add("Thursday");
            dtFnlTable.Rows.Add("Friday");
            dtFnlTable.Rows.Add("");
            for (int periodI = 0; periodI < dtBellSchedule.Rows.Count; periodI++)
            {
                dtFnlTable.Columns.Add(Convert.ToString(dtBellSchedule.Rows[periodI]["Period1"]).Trim());
                dtFnlTable.Rows[5][(periodI + 1)] = Convert.ToString(dtBellSchedule.Rows[periodI]["start_time"]).Trim() + "- " + Convert.ToString(dtBellSchedule.Rows[periodI]["end_time"]).Trim();
            }
            int lastPeriod = Convert.ToInt32(dtFnlTable.Columns[dtFnlTable.Columns.Count - 1].ColumnName);

            //Students Already Engaged in Options
            //NoOfHrsToAllot += 4;
            for (int optI = 0; optI < dtCriteria.Rows.Count; optI++)
            {
                #region Old 25-02-2017
                //byte DaysFk = Convert.ToByte(dtCriteria.Rows[optI]["DayPk"]);
                //byte HrsFk = Convert.ToByte(dtCriteria.Rows[optI]["HourPk"]);
                //string IsEngaged = Convert.ToString(dtCriteria.Rows[optI]["IsEngaged"]).Trim().ToUpper();
                //dtFnlTable.Rows[(DaysFk - 1)][HrsFk.ToString()] = "ENGAGED";
                ////NoOfHrsToAllot--; 
                #endregion

                byte DaysFk = Convert.ToByte(dtCriteria.Rows[optI]["DayPk"]);
                byte HrsFk = Convert.ToByte(dtCriteria.Rows[optI]["HourPk"]);
                string IsEngaged = Convert.ToString(dtCriteria.Rows[optI]["IsEngaged"]).Trim().ToUpper();
                string subject_code = Convert.ToString(dtCriteria.Rows[optI]["criterianame"]).Trim();

                //foreach (string roomPKACR in arrlstRoomDet)
                //{
                //    string[] roomVal = roomPKACR.Split('#');
                //    string hrI = roomPKACR + "$" + (DaysFk) + "_" + HrsFk;


                //    bool isRoomFree = false;
                //    if (dicCurRoomAvail.ContainsKey(hrI) && dicCurRoomAvail[hrI] == 0)
                //    {
                //        isRoomFree = true;

                //    }
                //    if (!isRoomFree)
                //        continue;

                dtFnlTable.Rows[(DaysFk - 1)][HrsFk.ToString()] = subject_code;// +"$" + roomVal[1];
                //NoOfHrsToAllot--;
                //    dicCurRoomAvail[hrI] = 1;
                //    break;
                //}
            }

            //if (ddlCriteriaReduced.Items.Count == 0)//If No options available
            //    NoOfHrsToAllot -= 4;

            Dictionary<string, byte> dicAllocSub = new Dictionary<string, byte>();
            Dictionary<string, byte> dicAllocElecSub = new Dictionary<string, byte>();
            ArrayList arrLabAllocToday = new ArrayList();

            ArrayList arrAlreadyAddedRow = new ArrayList();
            ArrayList arrAlreadyAddedCol = new ArrayList();//To Check whether already added in the row & column

            #region Fill Elective

            foreach (DictionaryEntry dicElective in htElectPreAllot)
            {
                string[] resultValues = dicElective.Key.ToString().Split('_');//0-subjecttypeno_1-days_2-hours
                if (resultValues.Length == 3)
                {

                    if (Convert.ToString(dtFnlTable.Rows[Convert.ToInt16(resultValues[1])][resultValues[2]]) == string.Empty)
                    {
                        string subType_no = resultValues[0];

                        StringBuilder sbSubDisp = new StringBuilder();
                        StringBuilder sbSubjStaff = new StringBuilder();
                        sbSubjStaff.Append("(");

                        dtSubjectDet.DefaultView.RowFilter = " subType_no = '" + subType_no + "'";
                        DataView dvSubType = dtSubjectDet.DefaultView;
                        int electStaffCount = 0;
                        if (dvSubType.Count > 0)
                        {
                            for (int electSubI = 0; electSubI < dvSubType.Count; electSubI++)
                            {
                                string subject_noE = Convert.ToString(dvSubType[electSubI]["subject_no"]).Trim();
                                string subject_codeE = Convert.ToString(dvSubType[electSubI]["subject_code"]);

                                //Loop through staff and availability
                                dtSubjectDetWt.DefaultView.RowFilter = "subject_no='" + subject_noE + "' ";// and staff_code='" + Convert.ToString(dtFacultyChoices.Rows[facChoiceIndex][subject_noE]) + "'
                                DataView dvSubjectStaff = dtSubjectDetWt.DefaultView;
                                dvSubjectStaff.Sort = " staffPriority asc";
                                if (dvSubjectStaff.Count > 0)
                                {
                                    StringBuilder sbStaffCodes = new StringBuilder();
                                    for (int stfI = 0; stfI < dvSubjectStaff.Count; stfI++)
                                    {
                                        string staff_code = Convert.ToString(dvSubjectStaff[stfI]["staff_code"]);
                                        string staff_appno = Convert.ToString(dvSubjectStaff[stfI]["appl_id"]);
                                        sbStaffCodes.Append(staff_code + "/");
                                        sbSubjStaff.Append(" staff_appno = '" + staff_appno + "' or ");
                                        electStaffCount++;
                                    }
                                    if (sbStaffCodes.Length > 1)
                                    {
                                        sbStaffCodes.Remove(sbStaffCodes.Length - 1, 1);
                                    }

                                    sbSubDisp.Append(subject_codeE + "-" + sbStaffCodes.ToString() + ",");
                                }
                            }

                            if (sbSubDisp.Length > 1)
                                sbSubDisp.Remove(sbSubDisp.Length - 1, 1);

                            if (htElectRooms.Contains(subType_no))
                                sbSubDisp.Append("$" + htElectRooms[subType_no].ToString());

                            if (sbSubjStaff.Length > 3)
                                sbSubjStaff.Remove(sbSubjStaff.Length - 3, 3);
                            sbSubjStaff.Append(") and ");

                            NoOfHrsToAllot--;
                            dtFnlTable.Rows[Convert.ToInt16(resultValues[1])][resultValues[2]] = sbSubDisp.ToString();
                        }
                    }
                }
            }

            #endregion

            bool fromRestart = false;
        restartLoop:
            //Loop through each day and hour


            //List<byte> lstDays = new List<byte>();
            //byte daysCount = (byte)(dtFnlTable.Rows.Count - 1);
            //for (int dayI = 0; dayI < daysCount; dayI++)
            //{
            //    lstDays.Add(getRandomDay(daysCount, lstDays));
            //}

            for (int dayI = 0; dayI < (dtFnlTable.Rows.Count - 1); dayI++)
            //for (int dI = 0; dI < lstDays.Count; dI++)
            {
                // int dayI = lstDays[dI];

                //List<byte> lstHours = new List<byte>();
                //byte periodsCount = (byte)dtFnlTable.Columns.Count;//noOfHrsPerDay;
                //for (int perI = 1; perI < periodsCount; perI++)
                //{
                //    lstHours.Add(getRandomPeriod(periodsCount, lstHours));
                //}
                //Loop through Hours
                for (int hrsI = 1; hrsI < (dtFnlTable.Columns.Count); hrsI++)
                //for (int lstI = 0; lstI < lstHours.Count; lstI++)
                {
                    //byte hrsI = lstHours[lstI];
                    Dictionary<string, bool> dicSubType = new Dictionary<string, bool>();
                    //Loop through subjects
                    for (int subI = 0; subI < dtSubjectDet.Rows.Count; subI++)
                    {
                        string colName = Convert.ToString(dtFnlTable.Columns[hrsI].ColumnName).Trim();
                        byte testCol = 0;
                        if (byte.TryParse(dtFnlTable.Columns[hrsI].ColumnName, out testCol) && Convert.ToString(dtFnlTable.Rows[dayI][colName]).Trim() == string.Empty)
                        {
                            #region Check for Lunch Availability
                            string comparingCell4 = Convert.ToString(dtFnlTable.Rows[dayI]["4"]).Trim();
                            string comparingCell5 = Convert.ToString(dtFnlTable.Rows[dayI]["5"]).Trim();

                            if (testCol == 4 && (!string.IsNullOrEmpty(comparingCell5)))
                            {
                                continue;
                            }
                            if (testCol == 5 && (!string.IsNullOrEmpty(comparingCell4)))
                            {
                                continue;
                            }
                            #endregion

                            string subject_no = Convert.ToString(dtSubjectDet.Rows[subI]["subject_no"]).Trim();
                            byte noofhrsperweek = Convert.ToByte(dtSubjectDet.Rows[subI]["noofhrsperweek"]);
                            string subject_code = Convert.ToString(dtSubjectDet.Rows[subI]["subject_code"]);
                            string ElectivePap = Convert.ToString(dtSubjectDet.Rows[subI]["ElectivePap"]).Trim().ToUpper();
                            string Lab = Convert.ToString(dtSubjectDet.Rows[subI]["Lab"]).Trim().ToUpper();
                            byte allotedHrs = 0;

                            //Check For existance
                            if (ElectivePap == "TRUE")
                            {
                            }
                            else if (dicAllocSub.ContainsKey(subject_no))
                            {
                                allotedHrs = dicAllocSub[subject_no];
                                if (allotedHrs >= noofhrsperweek)
                                    continue;
                            }
                            else
                            {
                                dicAllocSub.Add(subject_no, allotedHrs);
                            }

                            if (Lab != "TRUE" && ElectivePap != "TRUE")
                            {
                                #region Theory Region
                                if (!fromRestart)
                                    if (arrAlreadyAddedRow.Contains(subject_no + "_" + dayI))// || arrAlreadyAddedCol.Contains(subject_no + "_" + hrsI)
                                    {
                                        continue;
                                    }


                                //Loop through staff and availability
                                dtSubjectDetWt.DefaultView.RowFilter = "subject_no='" + subject_no + "' and staff_code='" + Convert.ToString(dtFacultyChoices.Rows[facChoiceIndex][subject_no]) + "'";
                                DataView dvSubjectStaff = dtSubjectDetWt.DefaultView;
                                dvSubjectStaff.Sort = " staffPriority asc";
                                if (dvSubjectStaff.Count > 0)
                                {
                                    string staff_code = Convert.ToString(dvSubjectStaff[0]["staff_code"]);
                                    string staff_appno = Convert.ToString(dvSubjectStaff[0]["appl_id"]);

                                    #region Special Staff
                                    dtSpclStaff.DefaultView.RowFilter = "staff_code='" + staff_code + "'";
                                    DataView dvIsSpecial = dtSpclStaff.DefaultView;
                                    if (dvIsSpecial.Count > 0)
                                    {
                                        DataTable dtSubHr = dvIsSpecial.ToTable();
                                        dtSubHr.DefaultView.RowFilter = "hourpk='" + hrsI + "'";
                                        DataView dvNHr = dtSubHr.DefaultView;
                                        if (dvNHr.Count == 0)
                                        {
                                            continue;
                                        }
                                    }
                                    else
                                    {

                                        //if (arrAlreadyAddedRow.Contains(subject_no + "_" + (dayI - 1)) && arrAlreadyAddedCol.Contains(subject_no + "_" + hrsI))
                                        //{
                                        //    continue;
                                        //}

                                        if (!fromRestart)
                                            if (arrAlreadyAddedCol.Contains(subject_no + "_" + hrsI))//|| 
                                            {
                                                continue;
                                            }
                                    }
                                    #endregion

                                    dtStaffDet.DefaultView.RowFilter = "staff_appno in (" + staff_appno + ") and DaysFK='" + (dayI + 1) + "' and HoursFK='" + testCol + "' ";
                                    DataView dvStaffAvail = dtStaffDet.DefaultView;
                                    bool IsEngaged = false;
                                    if (dvStaffAvail.Count > 0)
                                    {
                                        if (dvStaffAvail[0]["IsEngaged"].ToString().ToUpper() == "TRUE")
                                            IsEngaged = true;
                                    }

                                    #region Staff Theory, Lab, Day maximum hours check

                                    //if (testCol == lastPeriod)
                                    //{
                                    //    dtStaffDet.DefaultView.RowFilter = "staff_appno in (" + staff_appno + ") and DaysFK='" + (dayI + 1) + "' and HoursFK='" + (testCol - 1) + "' ";
                                    //    DataView dvStaffAvailMax = dtStaffDet.DefaultView;
                                    //    if (dvStaffAvailMax.Count > 0)
                                    //    {
                                    //        if (dvStaffAvailMax[0]["IsEngaged"].ToString().ToUpper() == "TRUE")
                                    //            IsEngaged = true;
                                    //    }
                                    //}
                                    //else if (testCol == 1)
                                    //{
                                    //    dtStaffDet.DefaultView.RowFilter = "staff_appno in (" + staff_appno + ") and DaysFK='" + (dayI + 1) + "' and HoursFK='" + (testCol + 1) + "' ";
                                    //    DataView dvStaffAvailMax = dtStaffDet.DefaultView;
                                    //    if (dvStaffAvailMax.Count > 0)
                                    //    {
                                    //        if (dvStaffAvailMax[0]["IsEngaged"].ToString().ToUpper() == "TRUE")
                                    //            IsEngaged = true;
                                    //    }
                                    //}
                                    //else
                                    //{
                                    //    dtStaffDet.DefaultView.RowFilter = "staff_appno in (" + staff_appno + ") and DaysFK='" + (dayI + 1) + "' and (HoursFK='" + (testCol + 1) + "' or  HoursFK='" + (testCol - 1) + "' )";
                                    //    DataView dvStaffAvailMax = dtStaffDet.DefaultView;
                                    //    if (dvStaffAvailMax.Count > 0)
                                    //    {
                                    //        if (dvStaffAvailMax[0]["IsEngaged"].ToString().ToUpper() == "TRUE")
                                    //            IsEngaged = true;
                                    //    }
                                    //}

                                    //dtStaffDet.DefaultView.RowFilter = "staff_appno in (" + staff_appno + ") and DaysFK='" + (dayI + 1) + "' and MaxHour='1'  ";
                                    //DataView dvStaffAvailMaxDay = dtStaffDet.DefaultView;
                                    //if (dvStaffAvailMaxDay.Count > 2)
                                    //{
                                    //    continue;
                                    //}
                                    #endregion

                                    if (!IsEngaged)
                                    {
                                        foreach (string roomPKACR in arrlstRoomDet)
                                        {
                                            string[] roomVal = roomPKACR.Split('#');
                                            string hrI = roomPKACR + "$" + (dayI + 1) + "_" + testCol;


                                            bool isRoomFree = false;
                                            if (dicCurRoomAvail.ContainsKey(hrI) && dicCurRoomAvail[hrI] == 0)
                                            {
                                                isRoomFree = true;

                                            }
                                            if (!isRoomFree)
                                                continue;

                                            dtFnlTable.Rows[dayI][colName] = subject_code + "-" + staff_code + "$" + roomVal[1];
                                            allotedHrs++;
                                            NoOfHrsToAllot--;
                                            dicAllocSub[subject_no]++;
                                            arrAlreadyAddedRow.Add(subject_no + "_" + dayI);
                                            arrAlreadyAddedCol.Add(subject_no + "_" + hrsI);

                                            dicCurRoomAvail[hrI] = 1;
                                            break;
                                        }
                                    }
                                }
                                #endregion
                            }
                            else if (ElectivePap == "TRUE")
                            {
                            }
                            else if (Lab == "TRUE")
                            {
                                if (arrLabAllocToday.Contains(dayI))
                                {
                                    continue;
                                }
                                //Lunch Check
                                if (testCol == 3 && (!string.IsNullOrEmpty(comparingCell5) || !string.IsNullOrEmpty(comparingCell4)))
                                {
                                    continue;
                                }
                                if (testCol == 4)
                                {
                                    continue;
                                }
                                #region Lab Region


                                string subComName = subject_no;
                                #region Practical Pair Check
                                dtSubjectDet.DefaultView.RowFilter = "practicalPair>0";
                                DataTable dtPairValues = dtSubjectDet.DefaultView.ToTable(true, "practicalPair", "subject_no", "subject_code");

                                int pairValue = Convert.ToInt32(dtSubjectDet.Rows[subI]["practicalPair"]);

                                if (pairValue > 0)
                                {
                                    dtPairValues.DefaultView.RowFilter = "practicalPair='" + pairValue + "'";
                                    DataView dvPair = dtPairValues.DefaultView;
                                    if (dvPair.Count > 0)
                                    {
                                        StringBuilder sbSubCode = new StringBuilder();
                                        StringBuilder sbSubNos = new StringBuilder();
                                        StringBuilder sbSubNo = new StringBuilder();
                                        for (int dvI = 0; dvI < dvPair.Count; dvI++)
                                        {
                                            sbSubNo.Append(dvPair[dvI]["subject_no"].ToString() + "-");
                                            sbSubNos.Append(dvPair[dvI]["subject_no"].ToString() + ",");
                                            sbSubCode.Append(dvPair[dvI]["subject_code"].ToString() + "#");
                                        }
                                        if (sbSubNo.Length > 0)
                                        {
                                            sbSubNo.Remove(sbSubNo.Length - 1, 1);
                                        }
                                        if (sbSubNos.Length > 0)
                                        {
                                            sbSubNos.Remove(sbSubNos.Length - 1, 1);
                                        }
                                        if (sbSubCode.Length > 0)
                                        {
                                            sbSubCode.Remove(sbSubCode.Length - 1, 1);
                                        }
                                        if (!dicSubType.ContainsKey(sbSubNo.ToString()))
                                        {
                                            dicSubType.Add(sbSubNo.ToString(), (Lab == "TRUE" ? true : false));

                                            subject_no = sbSubNos.ToString();
                                            subComName = sbSubNo.ToString();
                                            //subject_code = sbSubCode.ToString();
                                        }
                                        else
                                        {
                                            continue;
                                        }
                                    }
                                }
                                #endregion

                                string staffs = Convert.ToString(dtFacultyChoices.Rows[facChoiceIndex][subComName]);
                                string[] staffSub = staffs.Split(',');

                                //Loop through staff and availability

                                if (staffSub.Length > 0)
                                {
                                    string staff_code = staffSub[0].Split('-')[0];

                                    dtSubjectDetWt.DefaultView.RowFilter = " staff_code in ('" + staff_code + "') and practicalPair='" + pairValue + "'  and subject_no in ('" + staffSub[0].Split('-')[1] + "')";
                                    DataView dvSubjectStaff = dtSubjectDetWt.DefaultView;

                                    string staff_appno = Convert.ToString(dvSubjectStaff[0]["appl_id"]);

                                    string staff_code2 = staff_code;
                                    string staff_appno2 = Convert.ToString(dvSubjectStaff[0]["appl_id"]);
                                    subject_code = Convert.ToString(dvSubjectStaff[0]["subject_code"]);

                                    dtStaffDet.DefaultView.RowFilter = "staff_appno in (" + staff_appno + ") and DaysFK='" + (dayI + 1) + "' and MaxHour='2'  ";
                                    DataView dvStaffAvailMaxDay = dtStaffDet.DefaultView;
                                    if (dvStaffAvailMaxDay.Count > 1)
                                    {
                                        continue;
                                    }

                                    if (staffSub.Length > 1)
                                    {
                                        staff_code2 = staffSub[1].Split('-')[0];

                                        dtSubjectDetWt.DefaultView.RowFilter = " staff_code in ('" + staff_code2 + "')  and practicalPair='" + pairValue + "'  and subject_no in ('" + staffSub[1].Split('-')[1] + "')";
                                        DataView dvSubjectStaff1 = dtSubjectDetWt.DefaultView;

                                        staff_appno2 = Convert.ToString(dvSubjectStaff1[0]["appl_id"]);
                                        subject_code += "#" + Convert.ToString(dvSubjectStaff1[0]["subject_code"]);

                                        dtStaffDet.DefaultView.RowFilter = "staff_appno in (" + staff_appno2 + ") and DaysFK='" + (dayI + 1) + "' and MaxHour='2'  ";
                                        DataView dvStaffAvailMaxDay1 = dtStaffDet.DefaultView;
                                        if (dvStaffAvailMaxDay1.Count > 1)
                                        {
                                            continue;
                                        }
                                    }

                                    byte nextCol = 0;

                                    if ((hrsI + 1) < dtFnlTable.Columns.Count)
                                    {
                                        string colName2 = Convert.ToString(dtFnlTable.Columns[hrsI + 1].ColumnName).Trim();
                                        if (byte.TryParse(dtFnlTable.Columns[hrsI + 1].ColumnName, out nextCol) && Convert.ToString(dtFnlTable.Rows[dayI][colName2]).Trim() == string.Empty)
                                        {

                                            dtStaffDet.DefaultView.RowFilter = "(staff_appno = '" + staff_appno + "' or staff_appno = '" + staff_appno2 + "') and DaysFK='" + (dayI + 1) + "' and HoursFK='" + testCol + "' ";
                                            DataView dvStaffAvail = dtStaffDet.DefaultView;
                                            bool IsEngaged = false;
                                            if (dvStaffAvail.Count > 0)
                                            {
                                                if (dvStaffAvail[0]["IsEngaged"].ToString().ToUpper() == "TRUE")
                                                    IsEngaged = true;
                                                if (dvStaffAvail.Count > 1 && dvStaffAvail[1]["IsEngaged"].ToString().ToUpper() == "TRUE")
                                                    IsEngaged = true;
                                            }

                                            if (!IsEngaged)
                                            {
                                                #region Check For Next Hour EngageMent
                                                IsEngaged = false;
                                                dtStaffDet.DefaultView.RowFilter = "(staff_appno = '" + staff_appno + "' or staff_appno ='" + staff_appno2 + "') and DaysFK='" + (dayI + 1) + "' and HoursFK='" + (testCol + 1) + "' ";
                                                DataView dvStaffAvailNext = dtStaffDet.DefaultView;

                                                if (dvStaffAvailNext.Count > 0)
                                                {
                                                    if (dvStaffAvailNext[0]["IsEngaged"].ToString().ToUpper() == "TRUE")
                                                        IsEngaged = true;
                                                    if (dvStaffAvailNext.Count > 1 && dvStaffAvailNext[1]["IsEngaged"].ToString().ToUpper() == "TRUE")
                                                        IsEngaged = true;
                                                }
                                                #endregion
                                                if (!IsEngaged)
                                                {
                                                    foreach (string roomPKACR in arrlstLabDet)
                                                    {
                                                        string[] roomVal = roomPKACR.Split('#');
                                                        string hrI = roomPKACR + "$" + (dayI + 1) + "_" + testCol;
                                                        string hrII = roomPKACR + "$" + (dayI + 1) + "_" + (testCol + 1);

                                                        bool isRoomFree = false;
                                                        if (dicCurRoomAvail.ContainsKey(hrI) && dicCurRoomAvail.ContainsKey(hrII))
                                                        {
                                                            if (dicCurRoomAvail[hrI] == 0 && dicCurRoomAvail[hrII] == 0)
                                                            {
                                                                isRoomFree = true;
                                                            }
                                                        }
                                                        if (!isRoomFree)
                                                            continue;


                                                        if (arrAlreadyAddedRowCol.Contains(subject_no + "$" + roomVal[1] + "_" + dayI + "_" + hrsI) || arrAlreadyAddedRowCol.Contains(subject_no + "$" + roomVal[1] + "_" + dayI + "_" + (testCol + 1)))
                                                        {
                                                            continue;
                                                        }
                                                        //if (arrAlreadyAddedRowCol.Contains(subject_no + "$" + roomVal[1] + "_" + dayI + "_" + (testCol + 1)))
                                                        //{
                                                        //    continue;
                                                        //}

                                                        dtFnlTable.Rows[dayI][dtFnlTable.Columns[(hrsI)].ColumnName] = subject_code + "-" + staff_code + "/" + staff_code2 + "$" + roomVal[1];
                                                        dtFnlTable.Rows[dayI][dtFnlTable.Columns[(hrsI + 1)].ColumnName] = subject_code + "-" + staff_code + "/" + staff_code2 + "$" + roomVal[1];
                                                        allotedHrs += 2;
                                                        NoOfHrsToAllot -= 2;

                                                        string[] sub_nosl = subject_no.Split(',');
                                                        foreach (string subnol in sub_nosl)
                                                        {
                                                            if (!dicAllocSub.ContainsKey(subnol))
                                                            {
                                                                dicAllocSub.Add(subnol, 0);
                                                            }
                                                            dicAllocSub[subnol] += 2;
                                                            arrAlreadyAddedRowCol.Add(subnol + "$" + roomVal[1] + "_" + dayI + "_" + hrsI);
                                                            arrAlreadyAddedRowCol.Add(subnol + "$" + roomVal[1] + "_" + dayI + "_" + (testCol + 1));
                                                        }


                                                        if (!arrLabAllocToday.Contains(dayI))
                                                        {
                                                            arrLabAllocToday.Add(dayI);
                                                        }

                                                        dicCurRoomAvail[hrI] = 1;
                                                        dicCurRoomAvail[hrII] = 1;
                                                        break;
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                                #endregion
                            }
                        }
                    }
                }
            }
            if (NoOfHrsToAllot != 0)
            {
                if (fromRestart)
                {
                    if (!chkShowPart.Checked || !chkShowPart2.Checked)
                    {
                        if (NoOfHrsToAllot > 1)
                        {
                            //goto restartLoop;
                            dtFnlTable.Clear();
                        }
                    }
                    else
                    {
                        dtFnlTable.Clear();
                    }
                }
                else
                {
                    fromRestart = true;
                    goto restartLoop;
                }
            }
            dtTimeTable = dtFnlTable;
        }
        catch { dtTimeTable.Clear(); }
        return dtTimeTable;
    }
    //Get Faculty Choices for Subjects
    private DataTable getFacultyChoices(int batchYear, int degreeCode, int currentSem, ref DataTable dtSubjectDet, ref DataTable dtSubjectDetWt, ref int maxNoCanAllot, ref int noOfHrsPerDay, ref DataTable dtBellSchedule, ref DataTable dtStaffDet)
    {
        DataTable dtFacultyChoices = new DataTable();
        try
        {
            noOfHrsPerDay = dirAccess.selectScalarInt("select No_of_hrs_per_day from PeriodAttndSchedule where degree_code ='" + degreeCode + "' and semester ='" + currentSem + "'");

            dtBellSchedule = dirAccess.selectDataTable("select Period1,Desc1,SUBSTRING(Convert(Varchar,start_time,108),1,5) as start_time,SUBSTRING(Convert(Varchar,end_time,108),1,5) as end_time,no_of_breaks from BellSchedule where  Degree_Code='" + degreeCode + "' and semester ='" + currentSem + "' and batch_year='" + batchYear + "' order by start_time asc  -- ISNUMERIC(Period1) = 1 and ");


            if (noOfHrsPerDay > 0 && dtBellSchedule.Rows.Count > 0)
            {
                dtSubjectDet = dirAccess.selectDataTable("select sm.syll_code,ss.subType_no,ss.subject_type,ss.ElectivePap,ss.Lab,s.subject_no,s.subject_code,s.subject_name,isnull(s.sub_lab,0) as sub_lab,isnull(s.noofhrsperweek,0) as noofhrsperweek,s.maximumHrsPerDay,isnull(s.subjectpriority,0) as subjectpriority,s.practicalPair from syllabus_master sm,sub_sem ss, subject s where sm.syll_code = ss.syll_code and sm.syll_code = s.syll_code and ss.syll_code= s.syll_code and ss.subType_no = s.subType_no and sm.Batch_Year='" + batchYear + "' and sm.degree_code ='" + degreeCode + "' and sm.semester = '" + currentSem + "'  and (s.subject_no not in (select distinct subject_no from TT_StudentCriteria where semester='" + currentSem + "' and degree_code='" + degreeCode + "' and batch_year='" + batchYear + "')) order by subjectpriority desc,ElectivePap desc,Lab desc");// order by Lab desc,ElectivePap asc  and ss.subtype_no<>'193' 

                //object sumObject;
                //sumObject = dtSubjectDet.Compute("sum(noofhrsperweek)", "ElectivePap='false'");
                //int exceptElectiveCount = Convert.ToInt32(sumObject);
                //List<System.Decimal> listSubNo = dtSubjectDet.AsEnumerable()
                //           .Select(r => r.Field<System.Decimal>("subject_no"))
                //           .ToList();

                //string subNos = string.Join(",", listSubNo.ToArray());

                //DataTable dtPairValues = dirAccess.selectDataTable("select distinct practicalPair from subject where subject_no in ("+subNos+")");

                dtSubjectDet.DefaultView.RowFilter = "practicalPair>0";
                DataTable dtPairValues = dtSubjectDet.DefaultView.ToTable(true, "practicalPair", "subject_no");

                int exceptElectiveCount = Convert.ToInt32(dtSubjectDet.Compute("sum(noofhrsperweek)", "ElectivePap='false'"));
                dtSubjectDet.DefaultView.RowFilter = "ElectivePap='true'";
                int electiveCount = Convert.ToInt32(dtSubjectDet.DefaultView.ToTable(true, "subType_no", "noofhrsperweek").Compute("sum(noofhrsperweek)", string.Empty));

                maxNoCanAllot = exceptElectiveCount + electiveCount + 5;

                dtSubjectDetWt = dirAccess.selectDataTable("select sa.appl_id,sm.syll_code,ss.subType_no,ss.subject_type,isnull(ss.ElectivePap,0) as ElectivePap,ss.Lab,s.subject_no,s.subject_code,s.subject_name,isnull(s.sub_lab,0) as sub_lab,isnull(s.noofhrsperweek,0) as noofhrsperweek,s.maximumHrsPerDay,sts.staff_code,isnull(s.subjectpriority,0) as subjectpriority,sts.staffPriority, sts.facultyChoice,s.practicalPair   from  staff_appl_master sa, staffmaster sf ,syllabus_master sm,sub_sem ss, subject s,staff_selector sts where sm.syll_code = ss.syll_code and sm.syll_code = s.syll_code and ss.syll_code= s.syll_code and ss.subType_no = s.subType_no and s.subject_no =sts.subject_no and sa.appl_no = sf.appl_no and sf.staff_code=sts.staff_code  and sf.college_code ='" + collegecode + "' and sm.Batch_Year='" + batchYear + "' and sm.degree_code ='" + degreeCode + "' and sm.semester = '" + currentSem + "' and (s.subject_no not in (select distinct subject_no from TT_StudentCriteria where semester='" + currentSem + "' and degree_code='" + degreeCode + "' and batch_year='" + batchYear + "')) order by sts.facultyChoice asc ");

                DataTable dtLabSubjDetWt = dirAccess.selectDataTable("select sa.appl_id, sm.syll_code, ss.subType_no, ss.subject_type, isnull(ss.ElectivePap,0) as ElectivePap, ss.Lab,s.subject_no, s.subject_code, s.subject_name, isnull(s.sub_lab,0) as sub_lab, isnull(s.noofhrsperweek,0) as noofhrsperweek, s.maximumHrsPerDay, sts.staff_code, isnull(s.subjectpriority,0) as subjectpriority, sts.staffPriority, sts.facultyChoice, lc.FacLabChoiceValue from  staff_appl_master sa, staffmaster sf ,syllabus_master sm,sub_sem ss, subject s,staff_selector sts,TT_facultyLabChoice lc where sts.staffPriority=lc.staffPriorityFk and sts.facultyChoice is null and ss.Lab='1' and sm.syll_code = ss.syll_code and sm.syll_code = s.syll_code and ss.syll_code= s.syll_code and ss.subType_no = s.subType_no and s.subject_no =sts.subject_no and sa.appl_no = sf.appl_no and sf.staff_code=sts.staff_code  and sf.college_code ='" + collegecode + "' and sm.Batch_Year='" + batchYear + "' and sm.degree_code ='" + degreeCode + "' and sm.semester = '" + currentSem + "' order by lc.FacLabChoiceValue asc ");

                if (dtSubjectDet.Rows.Count > 0 && dtSubjectDetWt.Rows.Count > 0)
                {
                    byte maxFacultyChoice = (byte)dirAccess.selectScalarInt("select max(isnull(sts.facultyChoice,1)) as facultyChoice from  staff_appl_master sa, staffmaster sf ,syllabus_master sm,sub_sem ss, subject s,staff_selector sts where sm.syll_code = ss.syll_code and sm.syll_code = s.syll_code and ss.syll_code= s.syll_code and ss.subType_no = s.subType_no and s.subject_no =sts.subject_no and sa.appl_no = sf.appl_no and sf.staff_code=sts.staff_code  and sf.college_code ='" + collegecode + "' and sm.Batch_Year='" + batchYear + "' and sm.degree_code ='" + degreeCode + "' and sm.semester = '" + currentSem + "'");

                    if (maxFacultyChoice > 0)
                    {
                        //Staff availability
                        dtStaffDet = dirAccess.selectDataTable("select staff_appno ,DaysFK ,HoursFK , degreeCode ,batch_year , semester ,IsEngaged ,subject_no ,section,'0' as MaxHour  from TT_Staff_AllotAvail --  where Batch_Year='" + batchYear + "' and degreecode ='" + degreeCode + "' and semester = '" + currentSem + "' ");

                        Dictionary<string, bool> dicSubType = new Dictionary<string, bool>();
                        for (int subjI = 0; subjI < dtSubjectDet.Rows.Count; subjI++)
                        {
                            string subject_no = Convert.ToString(dtSubjectDet.Rows[subjI]["subject_no"]);
                            //string ElectivePap = Convert.ToString(dtSubjectDet.Rows[subjI]["ElectivePap"]);
                            string Lab = Convert.ToString(dtSubjectDet.Rows[subjI]["Lab"]).Trim().ToUpper();
                            int noofhrsperweek = Convert.ToInt32(dtSubjectDet.Rows[subjI]["noofhrsperweek"]);

                            int pairValue = Convert.ToInt32(dtSubjectDet.Rows[subjI]["practicalPair"]);

                            if (pairValue > 0)
                            {
                                dtPairValues.DefaultView.RowFilter = "practicalPair='" + pairValue + "'";
                                DataView dvPair = dtPairValues.DefaultView;
                                if (dvPair.Count > 0)
                                {
                                    StringBuilder sbSubNo = new StringBuilder();
                                    for (int dvI = 0; dvI < dvPair.Count; dvI++)
                                    {
                                        sbSubNo.Append(dvPair[dvI]["subject_no"].ToString() + "-");
                                    }
                                    if (sbSubNo.Length > 0)
                                    {
                                        sbSubNo.Remove(sbSubNo.Length - 1, 1);
                                    }
                                    if (!dicSubType.ContainsKey(sbSubNo.ToString()))
                                    {
                                        dicSubType.Add(sbSubNo.ToString(), (Lab == "TRUE" ? true : false));
                                        dtFacultyChoices.Columns.Add(sbSubNo.ToString());

                                        maxNoCanAllot -= (dvPair.Count - 1) * noofhrsperweek;
                                    }
                                }
                            }
                            else
                            {
                                dicSubType.Add(subject_no, (Lab == "TRUE" ? true : false));
                                dtFacultyChoices.Columns.Add(subject_no);
                            }
                        }

                        for (byte choiceI = 1; choiceI <= maxFacultyChoice; choiceI++)
                        {
                            DataRow drFacultyChoice = dtFacultyChoices.NewRow();
                            for (int subjI = 0; subjI < dtFacultyChoices.Columns.Count; subjI++)
                            {
                                string subject_no = Convert.ToString(dtFacultyChoices.Columns[subjI].ColumnName);
                                byte currChoice = choiceI;

                            checkForFacultyInLessChoice:

                                bool isLab = dicSubType[subject_no];
                                DataView dvFaculty = new DataView();
                                if (isLab)
                                {
                                    //dtLabSubjDetWt.DefaultView.RowFilter = " FacLabChoiceValue='" + currChoice + "' and subject_no='" + subject_no + "' ";
                                    //dvFaculty = dtLabSubjDetWt.DefaultView;
                                    string[] subnos = subject_no.Split('-');
                                    StringBuilder sbSnos = new StringBuilder();
                                    for (int sI = 0; sI < subnos.Length; sI++)
                                    {
                                        sbSnos.Append(subnos[sI] + ",");
                                    }
                                    if (sbSnos.Length > 0)
                                    {
                                        sbSnos.Remove(sbSnos.Length - 1, 1);
                                    }

                                    dtSubjectDetWt.DefaultView.RowFilter = " facultyChoice='" + currChoice + "' and subject_no in (" + sbSnos.ToString() + ") ";
                                    dvFaculty = dtSubjectDetWt.DefaultView;
                                    dvFaculty.Sort = " subject_no asc";
                                    currChoice--;
                                    if (dvFaculty.Count > 0)
                                    {
                                        if (dvFaculty.Count > 1)
                                        {
                                            drFacultyChoice[subject_no] = Convert.ToString(dvFaculty[0]["staff_code"]) + "-" + subnos[0] + "," + Convert.ToString(dvFaculty[1]["staff_code"]) + "-" + subnos[1];
                                        }
                                        else
                                        {
                                            drFacultyChoice[subject_no] = Convert.ToString(dvFaculty[0]["staff_code"]) + "-" + subnos[0];
                                        }
                                    }
                                    else
                                    {
                                        if (currChoice > 0)
                                            goto checkForFacultyInLessChoice;
                                    }

                                }
                                else
                                {
                                    dtSubjectDetWt.DefaultView.RowFilter = " facultyChoice='" + currChoice + "' and subject_no='" + subject_no + "' ";
                                    dvFaculty = dtSubjectDetWt.DefaultView;

                                    currChoice--;
                                    if (dvFaculty.Count > 0)
                                    {
                                        //if (dvFaculty.Count > 1)
                                        //{
                                        //    drFacultyChoice[subject_no] = Convert.ToString(dvFaculty[0]["staff_code"]) + "','" + Convert.ToString(dvFaculty[1]["staff_code"]);
                                        //}
                                        //else
                                        //{
                                        drFacultyChoice[subject_no] = Convert.ToString(dvFaculty[0]["staff_code"]);
                                        //}
                                    }
                                    else
                                    {
                                        if (currChoice > 0)
                                            goto checkForFacultyInLessChoice;
                                    }
                                }

                            }
                            dtFacultyChoices.Rows.Add(drFacultyChoice);
                        }
                    }
                }
            }
        }
        catch { dtFacultyChoices.Clear(); }
        return getFacultyCombination(dtFacultyChoices);
    }
    //static int staffCombo = 1;
    private DataTable getFacultyCombination(DataTable dtFacultyChoices)
    {
        try
        {
            DataTable dtNewCombination = dtFacultyChoices.Copy();
            dtNewCombination.Clear();
            int rowCnt = dtFacultyChoices.Rows.Count;
            int colCnt = dtFacultyChoices.Columns.Count;

            #region Row wise increment

            //int curRow = 0;
            //int curCol = 0;

            //for (int rowIndx = 0; rowIndx < rowCnt; rowIndx++)
            //{
            //    DataRow drComb = dtNewCombination.NewRow();
            //    for (int colIndx = 0; colIndx < colCnt; colIndx++)
            //    {
            //        drComb[colIndx] = Convert.ToString(dtFacultyChoices.Rows[curRow][curCol]);

            //        curCol++;
            //        if (curCol >= colCnt)
            //            curCol = 0;

            //        curRow++;
            //        if (curRow >= rowCnt)
            //            curRow = 0;
            //    }
            //    dtNewCombination.Rows.Add(drComb);

            //}
            //dtFacultyChoices.Merge(dtNewCombination);

            #endregion
            #region First Column Constant MISD Algorithm
            //for (int rowSpin = 0; rowSpin < rowCnt; rowSpin++)
            //{
            //    int curRow = 0;
            //    int curCol = 0;
            //    for (int rowIndx = 0; rowIndx < rowCnt; rowIndx++)
            //    {
            //        DataRow drComb = dtNewCombination.NewRow();
            //        curRow = rowIndx;
            //        for (int colIndx = 0; colIndx < colCnt; colIndx++)
            //        {
            //            if (colIndx == 1)
            //            {
            //                curRow += rowSpin;
            //                if (curRow >= rowCnt)
            //                    curRow = (curRow - rowCnt);
            //            }

            //            drComb[colIndx] = Convert.ToString(dtFacultyChoices.Rows[curRow][curCol]);

            //            curCol++;
            //            if (curCol >= colCnt)
            //                curCol = 0;

            //            curRow++;
            //            if (curRow >= rowCnt)
            //                curRow = (curRow - rowCnt);
            //        }
            //        dtNewCombination.Rows.Add(drComb);
            //    }
            //}
            //dtFacultyChoices.Merge(dtNewCombination);
            #endregion
            #region MISD Faculty Combo Algorithm
            //for (int colSpin = 1; colSpin < colCnt; colSpin++)
            //{
            //    for (int rowSpin = 0; rowSpin < rowCnt; rowSpin++)
            //    {
            //        int curRow = 0;
            //        int curCol = 0;
            //        for (int rowIndx = 0; rowIndx < rowCnt; rowIndx++)
            //        {
            //            DataRow drComb = dtNewCombination.NewRow();
            //            curRow = rowIndx;
            //            for (int colIndx = 0; colIndx < colCnt; colIndx++)
            //            {
            //                if (colIndx == colSpin)
            //                {
            //                    curRow += rowSpin;
            //                    if (curRow >= rowCnt)
            //                        curRow = (curRow - rowCnt);
            //                }

            //                drComb[colIndx] = Convert.ToString(dtFacultyChoices.Rows[curRow][curCol]);

            //                curCol++;
            //                if (curCol >= colCnt)
            //                    curCol = 0;

            //                curRow++;
            //                if (curRow >= rowCnt)
            //                    curRow = (curRow - rowCnt);
            //            }
            //            dtNewCombination.Rows.Add(drComb);
            //        }
            //    }
            //}
            //dtFacultyChoices.Merge(dtNewCombination);
            #endregion

            #region MISD Faculty Combo Algorithm
            #region Combo I
            for (int colSpin = 1; colSpin < colCnt; colSpin++)
            {
                for (int rowSpin = 0; rowSpin < rowCnt; rowSpin++)
                {
                    int curRow = 0;
                    int curCol = 0;
                    for (int rowIndx = 0; rowIndx < rowCnt; rowIndx++)
                    {
                        DataRow drComb = dtNewCombination.NewRow();
                        curRow = rowIndx;
                        for (int colIndx = 0; colIndx < colCnt; colIndx++)
                        {
                            if (colIndx == colSpin)
                            {
                                curRow += rowSpin;
                                if (curRow >= rowCnt)
                                    curRow = (curRow - rowCnt);
                            }

                            drComb[colIndx] = Convert.ToString(dtFacultyChoices.Rows[curRow][curCol]);

                            curCol++;
                            if (curCol >= colCnt)
                                curCol = 0;

                            curRow++;
                            if (curRow >= rowCnt)
                                curRow = (curRow - rowCnt);
                        }
                        dtNewCombination.Rows.Add(drComb);
                    }
                }
            }
            dtFacultyChoices.Merge(dtNewCombination);
            dtFacultyChoices = dtFacultyChoices.DefaultView.ToTable(true);
            #endregion
            #region Combo II
            //DataTable dtNewCombinationNew = dtFacultyChoices.Copy();
            //dtNewCombinationNew.Clear();

            //rowCnt = dtFacultyChoices.Rows.Count;
            //colCnt = dtFacultyChoices.Columns.Count;

            //for (int colSpin = 1; colSpin < colCnt; colSpin++)
            //{
            //    for (int rowSpin = 0; rowSpin < rowCnt; rowSpin++)
            //    {
            //        int curRow = 0;
            //        int curCol = 0;
            //        for (int rowIndx = 0; rowIndx < rowCnt; rowIndx++)
            //        {
            //            DataRow drComb = dtNewCombinationNew.NewRow();
            //            curRow = rowIndx;
            //            for (int colIndx = 0; colIndx < colCnt; colIndx++)
            //            {
            //                if (colIndx == colSpin)
            //                {
            //                    curRow += rowSpin;
            //                    if (curRow >= rowCnt)
            //                        curRow = (curRow - rowCnt);
            //                }

            //                drComb[colIndx] = Convert.ToString(dtFacultyChoices.Rows[curRow][curCol]);

            //                curCol++;
            //                if (curCol >= colCnt)
            //                    curCol = 0;

            //                curRow++;
            //                if (curRow >= rowCnt)
            //                    curRow = (curRow - rowCnt);
            //            }
            //            dtNewCombinationNew.Rows.Add(drComb);
            //        }
            //    }
            //}
            //dtFacultyChoices.Merge(dtNewCombinationNew);
            #endregion
            #endregion
        }
        catch { }
        dtFacultyChoices = dtFacultyChoices.DefaultView.ToTable(true);
        //dtFacultyChoices.Clear();
        //dtFacultyChoices.DefaultView.RowFilter = "[502]='STF005' and [503]='STF035' and [504]='STF024' and [505]='STF027'";
        //DataView dv = dtFacultyChoices.DefaultView;
        #region Manual Options
        ////First Option
        //DataRow drNewA = dtFacultyChoices.NewRow();
        //drNewA[0] = "STF005";
        //drNewA[1] = "STF035";
        //drNewA[2] = "STF024";
        //drNewA[3] = "STF027";
        //drNewA[4] = "STF033";
        //drNewA[5] = "STF014";
        //drNewA[6] = "STF012";
        //drNewA[7] = "STF009','STF030";
        //dtFacultyChoices.Rows.InsertAt(drNewA, 0);

        //DataRow drNewA2 = dtFacultyChoices.NewRow();
        //drNewA2[0] = "STF005";
        //drNewA2[1] = "STF035";
        //drNewA2[2] = "STF024";
        //drNewA2[3] = "STF027";
        //drNewA2[4] = "STF034";
        //drNewA2[5] = "STF014";
        //drNewA2[6] = "STF012";
        //drNewA2[7] = "STF009','STF030";
        //dtFacultyChoices.Rows.InsertAt(drNewA2, 0);

        //DataRow drNewA3 = dtFacultyChoices.NewRow();
        //drNewA3[0] = "STF005";
        //drNewA3[1] = "STF035";
        //drNewA3[2] = "STF024";
        //drNewA3[3] = "STF027";
        //drNewA3[4] = "STF034";
        //drNewA3[5] = "STF014";
        //drNewA3[6] = "STF019";
        //drNewA3[7] = "STF009','STF030";
        //dtFacultyChoices.Rows.InsertAt(drNewA3, 0);

        //DataRow drNewA4 = dtFacultyChoices.NewRow();
        //drNewA4[0] = "STF005";
        //drNewA4[1] = "STF035";
        //drNewA4[2] = "STF024";
        //drNewA4[3] = "STF027";
        //drNewA4[4] = "STF033";
        //drNewA4[5] = "STF014";
        //drNewA4[6] = "STF019";
        //drNewA4[7] = "STF009','STF030";
        //dtFacultyChoices.Rows.InsertAt(drNewA4, 0);



        //////Second Option

        //DataRow drNewB1 = dtFacultyChoices.NewRow();
        //drNewB1[0] = "STF031";
        //drNewB1[1] = "STF002";
        //drNewB1[2] = "STF003";
        //drNewB1[3] = "STF006";
        //drNewB1[4] = "STF033";
        //drNewB1[5] = "STF014";
        //drNewB1[6] = "STF012";
        //drNewB1[7] = "STF014','STF034";
        //dtFacultyChoices.Rows.InsertAt(drNewB1, 0);

        //DataRow drNewB2 = dtFacultyChoices.NewRow();
        //drNewB2[0] = "STF031";
        //drNewB2[1] = "STF002";
        //drNewB2[2] = "STF003";
        //drNewB2[3] = "STF006";
        //drNewB2[4] = "STF034";
        //drNewB2[5] = "STF014";
        //drNewB2[6] = "STF012";
        //drNewB2[7] = "STF014','STF034";
        //dtFacultyChoices.Rows.InsertAt(drNewB2, 0);

        //DataRow drNewB3 = dtFacultyChoices.NewRow();
        //drNewB3[0] = "STF031";
        //drNewB3[1] = "STF002";
        //drNewB3[2] = "STF003";
        //drNewB3[3] = "STF006";
        //drNewB3[4] = "STF034";
        //drNewB3[5] = "STF014";
        //drNewB3[6] = "STF019";
        //drNewB3[7] = "STF014','STF034";
        //dtFacultyChoices.Rows.InsertAt(drNewB3, 0);

        //DataRow drNewB4 = dtFacultyChoices.NewRow();
        //drNewB4[0] = "STF031";
        //drNewB4[1] = "STF002";
        //drNewB4[2] = "STF003";
        //drNewB4[3] = "STF006";
        //drNewB4[4] = "STF033";
        //drNewB4[5] = "STF014";
        //drNewB4[6] = "STF019";
        //drNewB4[7] = "STF014','STF034";
        //dtFacultyChoices.Rows.InsertAt(drNewB4, 0);

        ////Third Option
        //DataRow drNewC1 = dtFacultyChoices.NewRow();

        //drNewC1[0] = "STF010";
        //drNewC1[1] = "STF025";
        //drNewC1[2] = "STF001";
        //drNewC1[3] = "STF026";
        //drNewC1[4] = "STF033";
        //drNewC1[5] = "STF014";
        //drNewC1[6] = "STF012";
        //drNewC1[7] = "STF008','STF036";
        //dtFacultyChoices.Rows.InsertAt(drNewC1, 0);

        //DataRow drNewC2 = dtFacultyChoices.NewRow();
        //drNewC2[0] = "STF010";
        //drNewC2[1] = "STF025";
        //drNewC2[2] = "STF001";
        //drNewC2[3] = "STF026";
        //drNewC2[4] = "STF034";
        //drNewC2[5] = "STF014";
        //drNewC2[6] = "STF012";
        //drNewC2[7] = "STF008','STF036";
        //dtFacultyChoices.Rows.InsertAt(drNewC2, 0);

        //DataRow drNewC3 = dtFacultyChoices.NewRow();
        //drNewC3[0] = "STF010";
        //drNewC3[1] = "STF025";
        //drNewC3[2] = "STF001";
        //drNewC3[3] = "STF026";
        //drNewC3[4] = "STF034";
        //drNewC3[5] = "STF014";
        //drNewC3[6] = "STF019";
        //drNewC3[7] = "STF008','STF036";
        //dtFacultyChoices.Rows.InsertAt(drNewC3, 0);

        //DataRow drNewC4 = dtFacultyChoices.NewRow();
        //drNewC4[0] = "STF010";
        //drNewC4[1] = "STF025";
        //drNewC4[2] = "STF001";
        //drNewC4[3] = "STF026";
        //drNewC4[4] = "STF033";
        //drNewC4[5] = "STF014";
        //drNewC4[6] = "STF019";
        //drNewC4[7] = "STF008','STF036";
        //dtFacultyChoices.Rows.InsertAt(drNewC4, 0);

        ////Fourth Option
        //DataRow drNewD = dtFacultyChoices.NewRow();

        //drNewD[0] = "STF007";
        //drNewD[1] = "STF028";
        //drNewD[2] = "STF008";
        //drNewD[3] = "STF011";
        //drNewD[4] = "STF033";
        //drNewD[5] = "STF014";
        //drNewD[6] = "STF012";
        //drNewD[7] = "STF023','STF008";
        //dtFacultyChoices.Rows.InsertAt(drNewD, 0);

        ////Fifth Option
        //DataRow drNewE = dtFacultyChoices.NewRow();

        //drNewE[0] = "STF029";
        //drNewE[1] = "STF016";
        //drNewE[2] = "STF009";
        //drNewE[3] = "STF017";
        //drNewE[4] = "STF033";
        //drNewE[5] = "STF014";
        //drNewE[6] = "STF012";
        //drNewE[7] = "STF006','STF022";
        //dtFacultyChoices.Rows.InsertAt(drNewE, 0);
        #endregion

        return dtFacultyChoices;
    }
    //Get Availability of Rooms and create slots
    private Dictionary<string, int> getRoomAvailability(int batchYear, int degreeCode, int currentSem, ref ArrayList arrlstRoomDet, ref ArrayList arrlstLabDet)
    {
        //Dictionary Nomenclature -->  -2 : Lab, -1 : Hall, 0 - Not Allocated, 1 - Allocated 

        Dictionary<string, int> dicRoomAvailability = new Dictionary<string, int>();
        try
        {
            DataTable dtRoomAvailability = new DataTable();
            string selQ = "select Roompk,Building_Name, Floor_Name, Room_Acronym, StartingSerial, Room_Name, Room_Description, College_Code,Room_type, selectionflag, no_of_rows, no_of_columns, room_size, students_allowed, Avl_Student, Dept_Code,	 MaxStudClassStrength from room_detail where isnull(selectionflag,'0') ='0' and dept_code='" + degreeCode + "' and College_Code='" + collegecode + "' ";
            dtRoomAvailability = dirAccess.selectDataTable(selQ);
            foreach (DataRow drRoom in dtRoomAvailability.Rows)
            {
                bool roomType = Convert.ToString(drRoom["Room_type"]).Trim().ToUpper() == "LAB" ? true : false;
                //string roomPKRoomACR = Convert.ToString(drRoom["Roompk"]) + "#" + Convert.ToString(drRoom["Room_Acronym"]);
                string roomPKRoomACR = " " + "#" + Convert.ToString(drRoom["Room_Acronym"]);
                if (roomType)
                {
                    arrlstLabDet.Add(roomPKRoomACR);
                }
                else
                {
                    arrlstRoomDet.Add(roomPKRoomACR);
                }

                for (int dayI = 1; dayI < 6; dayI++)
                {
                    for (int hrsI = 1; hrsI < 9; hrsI++)
                    {
                        dicRoomAvailability.Add(roomPKRoomACR + "$" + dayI + "_" + hrsI, 0);
                    }
                }
            }
        }
        catch { }
        return dicRoomAvailability;
    }
    //New Get Availability of Rooms and create slots
    private Dictionary<string, int> getRoomAvailability(int batchYear, int degreeCode, int currentSem, ref ArrayList arrlstRoomDet, ref ArrayList arrlstLabDet, int startRow)
    {
        //Dictionary Nomenclature -->  -2 : Lab, -1 : Hall, 0 - Not Allocated, 1 - Allocated 

        Dictionary<string, int> dicRoomAvailability = new Dictionary<string, int>();
        try
        {
            DataTable dtRoomAvailability = new DataTable();
            string selQ = "select Roompk,Building_Name, Floor_Name, Room_Acronym, StartingSerial, Room_Name, Room_Description, College_Code,Room_type, selectionflag, no_of_rows, no_of_columns, room_size, students_allowed, Avl_Student, Dept_Code,	 MaxStudClassStrength from room_detail where isnull(selectionflag,'0') ='0' and dept_code='" + degreeCode + "' and College_Code='" + collegecode + "'  order by Room_type asc ";
            dtRoomAvailability = dirAccess.selectDataTable(selQ);

            DataTable dtNewRoomOrder = new DataTable();
            dtNewRoomOrder = dtRoomAvailability.Copy();
            dtNewRoomOrder.Clear();

            dtRoomAvailability.DefaultView.RowFilter = "Room_type<>'LAB'";
            DataTable dtRoomOrder = dtRoomAvailability.DefaultView.ToTable();

            if (startRow >= dtRoomAvailability.Rows.Count)
                startRow = 0;
            int temRow = 0;
            for (int rowI = startRow; temRow < dtRoomOrder.Rows.Count; rowI++, temRow++)
            {
                if (rowI >= dtRoomOrder.Rows.Count)
                {
                    rowI = 0;
                }
                DataRow dr = dtNewRoomOrder.NewRow();
                for (int dc = 0; dc < dtNewRoomOrder.Columns.Count; dc++)
                {
                    dr[dc] = dtRoomOrder.Rows[rowI][dc];
                }
                dtNewRoomOrder.Rows.Add(dr);
            }

            dtRoomAvailability.DefaultView.RowFilter = "Room_type='LAB'";
            DataTable dtLab = dtRoomAvailability.DefaultView.ToTable();
            dtRoomAvailability = dtNewRoomOrder.Copy();
            dtRoomAvailability.Merge(dtLab);

            foreach (DataRow drRoom in dtRoomAvailability.Rows)
            {
                bool roomType = Convert.ToString(drRoom["Room_type"]).Trim().ToUpper() == "LAB" ? true : false;
                //string roomPKRoomACR = Convert.ToString(drRoom["Roompk"]) + "#" + Convert.ToString(drRoom["Room_Acronym"]);
                string roomPKRoomACR = " " + "#" + Convert.ToString(drRoom["Room_Acronym"]);
                if (roomType)
                {
                    arrlstLabDet.Add(roomPKRoomACR);
                }
                else
                {
                    arrlstRoomDet.Add(roomPKRoomACR);
                }

                for (int dayI = 1; dayI < 6; dayI++)
                {
                    for (int hrsI = 1; hrsI < 9; hrsI++)
                    {
                        dicRoomAvailability.Add(roomPKRoomACR + "$" + dayI + "_" + hrsI, 0);
                    }
                }
            }
        }
        catch { }
        return dicRoomAvailability;
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
        clrList.Add("#673AB7");
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

        return clrList;
    }
    private byte getRandomPeriod(byte rangeLimit, List<byte> lstPeriods)
    {
        Random rand = new Random();
        byte rNumber = 0;

        do
        {
            rNumber = (byte)rand.Next(1, rangeLimit);
        } while (lstPeriods.Contains(rNumber));

        return rNumber;
    }
    private byte getRandomDay(byte rangeLimit, List<byte> lstDays)
    {
        Random rand = new Random();
        byte rNumber = 0;

        do
        {
            rNumber = (byte)rand.Next(0, rangeLimit);
        } while (lstDays.Contains(rNumber));

        return rNumber;
    }
    private byte getRandomCriteria(int rangeLimit)
    {
        Random rand = new Random();
        byte rNumber = 0;

        rNumber = (byte)rand.Next(1, (rangeLimit));

        return --rNumber;
    }
    private string getAlphaFromNumber(byte alphaCode)
    {
        string alphaVal = "A" + alphaCode;
        if (alphaCode < 27 && alphaCode > 0)
        {
            alphaCode += 64;
            alphaVal = ((char)alphaCode).ToString();
        }
        return alphaVal;
    }
    //Show Time Tables
    protected void btnShowSavedTables_Click(object sender, EventArgs e)
    {
        try
        {
            // ddlSelectedTimeTable.Items.Clear();
            DataSet dsTimeTable = new DataSet();

            if (Session["selectedDataSet"] != null)
            {
                dsTimeTable = (DataSet)Session["selectedDataSet"];
            }
            DataTable dtStaffSubDet = dirAccess.selectDataTable("select sa.appl_id,sm.syll_code,ss.subType_no,ss.subject_type,isnull(ss.ElectivePap,0) as ElectivePap,ss.Lab,s.subject_no,staff_name,s.subject_code,s.subject_name,isnull(s.sub_lab,0) as sub_lab,isnull(s.noofhrsperweek,0) as noofhrsperweek,s.maximumHrsPerDay,sts.staff_code,isnull(s.subjectpriority,0) as subjectpriority,sts.staffPriority, sts.facultyChoice   from  staff_appl_master sa, staffmaster sf ,syllabus_master sm,sub_sem ss, subject s,staff_selector sts where sm.syll_code = ss.syll_code and sm.syll_code = s.syll_code and ss.syll_code= s.syll_code and ss.subType_no = s.subType_no and s.subject_no =sts.subject_no and sa.appl_no = sf.appl_no and sf.staff_code=sts.staff_code and sf.college_code ='" + collegecode + "'");
            if (dsTimeTable.Tables.Count > 0 && dtStaffSubDet.Rows.Count > 0)
            {
                #region Display generated Tables
                //Adding Colors
                ArrayList arrSubName = new ArrayList();

                List<string> lstCellValues = new List<string>();
                lstCellValues.Add("monday");
                lstCellValues.Add("tuesday");
                lstCellValues.Add("wednesday");
                lstCellValues.Add("thursday");
                lstCellValues.Add("friday");
                lstCellValues.Add("");

                //Building an HTML string.
                StringBuilder html = new StringBuilder();
                for (int ttI = 0; ttI < dsTimeTable.Tables.Count; ttI++)
                {
                    Dictionary<string, string> dicFooter = new Dictionary<string, string>();

                    StringBuilder sbFooter = new StringBuilder();
                    sbFooter.Append("<table cellpadding='0' cellspacing='0' style=' width:920px; font-size:10px;'><tr style='background-color:#3B6D93;color:#FFFFFF;'><td>Staff Code</td><td>Staff Name</td><td>Subject Code</td><td>Subject Name</td><td>Mon</td><td>Tue</td><td>Wed</td><td>Thu</td><td>Fri</td></tr>");
                    Hashtable htFooter = new Hashtable();
                    //dsTimeTable.Tables[ttI].TableName
                    string[] dispNames = dsTimeTable.Tables[ttI].TableName.Split('-');
                    StringBuilder sbDispName = new StringBuilder();
                    for (int dI = 0; dI < dispNames.Length; dI++)
                    {
                        if (dI == (dispNames.Length - 1))
                        {
                            sbDispName.Append(getAlphaFromNumber((byte)(ttI + 1)));
                        }
                        else
                        {
                            sbDispName.Append(dispNames[dI] + "-");
                        }
                    }
                    html.Append("<center><span style='color: Green; font-size:medium;'>" + sbDispName.ToString() + "</span></center><br/>");//dsTimeTable.Tables[ttI].TableName+"-"+getAlphaFromNumber((byte)(ttI + 1)) 
                    //Table start.
                    html.Append("<table cellpadding='0' cellspacing='0' style=' border:1px solid black; border-radius:5px; text-align:center; width:920px; font-size:10px;'>");
                    int cnt = 1;
                    //Building the Last row.
                    html.Append("<tr  style='background-color:#3B6D93;color:#FFFFFF; font-size:12px;'>");
                    foreach (DataColumn column in dsTimeTable.Tables[ttI].Columns)
                    {
                        html.Append("<td>");
                        html.Append(dsTimeTable.Tables[ttI].Rows[dsTimeTable.Tables[ttI].Rows.Count - 1][column.ColumnName]);
                        html.Append("</td>");
                    }
                    html.Append("</tr>");
                    //Building the Header row.
                    html.Append("<tr style='background-color:#3B6D93;color:#FFFFFF; font-size:12px;'>");
                    foreach (DataColumn column in dsTimeTable.Tables[ttI].Columns)
                    {
                        html.Append("<td>");
                        html.Append(column.ColumnName);
                        html.Append("</td>");
                    }
                    html.Append("</tr>");

                    //Building the Data rows.
                    foreach (DataRow row in dsTimeTable.Tables[ttI].Rows)
                    {
                        if (cnt == dsTimeTable.Tables[ttI].Rows.Count)
                        {
                            continue;
                        }
                        cnt++;
                        html.Append("<tr>");

                        foreach (DataColumn column in dsTimeTable.Tables[ttI].Columns)
                        {
                            string slotValue = row[column.ColumnName].ToString().Trim();
                            if (!lstCellValues.Contains(slotValue.ToLower()))
                            {
                                if (!arrSubName.Contains(slotValue.Split('-')[0]))
                                    arrSubName.Add(slotValue.Split('-')[0]);
                                int index = arrSubName.IndexOf(slotValue.Split('-')[0]);
                                string bgcolor = getColor(index);
                                html.Append("<td style='background-color:" + bgcolor + "'>");
                            }
                            else
                            {
                                if (string.IsNullOrEmpty(slotValue))
                                {
                                    html.Append("<td style='background-color:#FFFFFF;'>");
                                }
                                else
                                {
                                    html.Append("<td style='background-color:#3B6D93;color:#FFFFFF; font-size:12px;'>");
                                }
                            }
                            html.Append(slotValue);
                            html.Append("</td>");

                            #region DisplayFooter

                            if (slotValue.Contains("$"))
                            {
                                string[] cellvalues = slotValue.Split('$');//Split from room
                                string[] electRooms = cellvalues[1].Split(',');
                                int staffRoomIndx = 0;
                                string[] cellfnlValues = cellvalues[0].Split(',');// split subjects and staffs if elective
                                foreach (string cellfnlValue in cellfnlValues)
                                {
                                    string[] resValues = cellfnlValue.Split('-');//split subject from staff
                                    string subject_code = resValues[0];
                                    string[] staff_codes = resValues[1].Split('/');
                                    int subI = 0;
                                    string[] subject_codes = subject_code.Split('#');
                                    foreach (string staffcode in staff_codes)
                                    {
                                        string curRoom = electRooms.Length > 1 ? electRooms[staffRoomIndx] : electRooms[0];
                                        staffRoomIndx++;

                                        dtStaffSubDet.DefaultView.RowFilter = "subject_code='" + subject_codes[subI] + "' and staff_code='" + staffcode + "'";
                                        DataView dvNew = dtStaffSubDet.DefaultView;
                                        if (dvNew.Count > 0)
                                        {

                                            if (!htFooter.Contains(staffcode + "_" + subject_codes[subI]))
                                            {
                                                string disp = "<tr><td>" + staffcode + "</td><td>" + dvNew[0]["staff_name"].ToString() + "</td><td>" + subject_codes[subI] + "</td><td>" + dvNew[0]["subject_name"].ToString() + "</td><td>M1Mon</td><td>T2Tue</td><td>W3Wed</td><td>T4Thu</td><td>F5Fri</td></tr>";

                                                htFooter.Add(staffcode + "_" + subject_codes[subI], htFooter.Count);
                                                //sbFooter.Append(disp);

                                                string labHour = string.Empty;
                                                int curCol = Convert.ToInt32(column.ColumnName);
                                                int nexCol = 0;
                                                if (dsTimeTable.Tables[ttI].Columns.Contains((curCol + 1).ToString()) && int.TryParse((curCol + 1).ToString(), out nexCol))
                                                {
                                                    string nextVal = row[(curCol + 1).ToString()].ToString().Trim();
                                                    if (slotValue == nextVal)
                                                        labHour = "," + (Convert.ToInt32(column.ColumnName) + 1).ToString();
                                                }

                                                switch (row[0].ToString())
                                                {
                                                    case "Monday":
                                                        disp = disp.Replace("M1Mon", column.ColumnName + labHour + "-" + curRoom + " ");
                                                        break;
                                                    case "Tuesday":
                                                        disp = disp.Replace("T2Tue", column.ColumnName + labHour + "-" + curRoom + " ");
                                                        break;
                                                    case "Wednesday":
                                                        disp = disp.Replace("W3Wed", column.ColumnName + labHour + "-" + curRoom + " ");
                                                        break;
                                                    case "Thursday":
                                                        disp = disp.Replace("T4Thu", column.ColumnName + labHour + "-" + curRoom + " ");
                                                        break;
                                                    case "Friday":
                                                        disp = disp.Replace("F5Fri", column.ColumnName + labHour + "-" + curRoom + " ");
                                                        break;
                                                }
                                                dicFooter.Add(staffcode + "_" + subject_codes[subI], disp);
                                            }
                                            else
                                            {

                                                string labHour = string.Empty;
                                                int curCol = Convert.ToInt32(column.ColumnName);
                                                int nexCol = 0;
                                                if (dsTimeTable.Tables[ttI].Columns.Contains((curCol + 1).ToString()) && int.TryParse((curCol + 1).ToString(), out nexCol))
                                                {
                                                    string nextVal = row[(curCol + 1).ToString()].ToString().Trim();
                                                    if (slotValue == nextVal)
                                                        labHour = "," + (Convert.ToInt32(column.ColumnName) + 1).ToString();
                                                }

                                                string disp = dicFooter[staffcode + "_" + subject_codes[subI]];
                                                switch (row[0].ToString())
                                                {
                                                    case "Monday":
                                                        disp = disp.Replace("M1Mon", column.ColumnName + labHour + "-" + curRoom + " ");
                                                        break;
                                                    case "Tuesday":
                                                        disp = disp.Replace("T2Tue", column.ColumnName + labHour + "-" + curRoom + " ");
                                                        break;
                                                    case "Wednesday":
                                                        disp = disp.Replace("W3Wed", column.ColumnName + labHour + "-" + curRoom + " ");
                                                        break;
                                                    case "Thursday":
                                                        disp = disp.Replace("T4Thu", column.ColumnName + labHour + "-" + curRoom + " ");
                                                        break;
                                                    case "Friday":
                                                        disp = disp.Replace("F5Fri", column.ColumnName + labHour + "-" + curRoom + " ");
                                                        break;
                                                }
                                                dicFooter[staffcode + "_" + subject_codes[subI]] = disp;
                                            }
                                        }
                                        if (subject_codes.Length == staff_codes.Length)
                                        {
                                            subI++;//increase subject code only when two staffs & subjects available (i.e, combined lab)
                                        }
                                    }
                                }
                            }

                            #endregion
                        }
                        html.Append("</tr>");
                    }
                    foreach (KeyValuePair<string, string> foot in dicFooter)
                    {
                        sbFooter.Append(foot.Value);
                    }
                    sbFooter.Append("</table><br>");
                    sbFooter.Replace("M1Mon", "-");
                    sbFooter.Replace("T2Tue", "-");
                    sbFooter.Replace("W3Wed", "-");
                    sbFooter.Replace("T4Thu", "-");
                    sbFooter.Replace("F5Fri", "-");
                    //Table end.
                    html.Append("</table><br>");
                    html.Append(sbFooter.ToString());
                }
                //Append the HTML string to Placeholder.
                divTimeTableOutput.Visible = true;
                phTimeTable.Controls.Add(new Literal { Text = html.ToString() });

                #endregion
            }
        }
        catch { }
    }
    //Room Time Tables
    protected void btnRoomTables_Click(object sender, EventArgs e)
    {
        try
        {
            // ddlSelectedTimeTable.Items.Clear();
            DataSet dsTimeTable = new DataSet();

            if (Session["selectedDataSet"] != null)
            {
                dsTimeTable = (DataSet)Session["selectedDataSet"];
            }
            DataTable dtStaffSubDet = dirAccess.selectDataTable("select sa.appl_id,sm.syll_code,ss.subType_no,ss.subject_type,isnull(ss.ElectivePap,0) as ElectivePap,ss.Lab,s.subject_no,staff_name,s.subject_code,s.subject_name,isnull(s.sub_lab,0) as sub_lab,isnull(s.noofhrsperweek,0) as noofhrsperweek,s.maximumHrsPerDay,sts.staff_code,isnull(s.subjectpriority,0) as subjectpriority,sts.staffPriority, sts.facultyChoice   from  staff_appl_master sa, staffmaster sf ,syllabus_master sm,sub_sem ss, subject s,staff_selector sts where sm.syll_code = ss.syll_code and sm.syll_code = s.syll_code and ss.syll_code= s.syll_code and ss.subType_no = s.subType_no and s.subject_no =sts.subject_no and sa.appl_no = sf.appl_no and sf.staff_code=sts.staff_code and sf.college_code ='" + collegecode + "'");
            if (dsTimeTable.Tables.Count > 0 && dtStaffSubDet.Rows.Count > 0)
            {
                List<string> lstCellValues = new List<string>();
                lstCellValues.Add("monday");
                lstCellValues.Add("tuesday");
                lstCellValues.Add("wednesday");
                lstCellValues.Add("thursday");
                lstCellValues.Add("friday");


                string degreeCode = string.Empty;
                DataTable dtRoomAvailability = new DataTable();
                string selQ = "select Roompk,Building_Name, Floor_Name, Room_Acronym, StartingSerial, Room_Name, Room_Description, College_Code,Room_type, selectionflag, no_of_rows, no_of_columns, room_size, students_allowed, Avl_Student, Dept_Code,	 MaxStudClassStrength from room_detail where isnull(selectionflag,'0') ='0'  order by Room_type asc  -- and dept_code='" + degreeCode + "' and College_Code='" + collegecode + "' ";
                dtRoomAvailability = dirAccess.selectDataTable(selQ);

                DataSet dsRoomTables = new DataSet();

                foreach (DataRow drRoom in dtRoomAvailability.Rows)
                {
                    bool roomType = Convert.ToString(drRoom["Room_type"]).Trim().ToUpper() == "LAB" ? true : false;
                    //string roomPKRoomACR = Convert.ToString(drRoom["Roompk"]) + "#" + Convert.ToString(drRoom["Room_Acronym"]);
                    string roomPKRoomACR = Convert.ToString(drRoom["Room_Acronym"]);

                    DataTable dtRoomTable = dsTimeTable.Tables[0].Copy();//Copy the Structure
                    dtRoomTable.TableName = Convert.ToString(drRoom["Room_type"]).Trim().ToUpper() + " - " + roomPKRoomACR;

                    for (int row = 0; row < dtRoomTable.Rows.Count - 1; row++)
                    {
                        for (int column = 1; column < dtRoomTable.Columns.Count; column++)
                        {
                            dtRoomTable.Rows[row][column] = string.Empty;
                        }
                    }

                    for (int ttI = 0; ttI < dsTimeTable.Tables.Count; ttI++)
                    {
                        for (int row = 0; row < dsTimeTable.Tables[ttI].Rows.Count - 1; row++)
                        {
                            for (int column = 0; column < dsTimeTable.Tables[ttI].Columns.Count; column++)
                            {
                                string slotValue = dsTimeTable.Tables[ttI].Rows[row][column].ToString().Trim();
                                if (slotValue.Contains(roomPKRoomACR))
                                {
                                    dtRoomTable.Rows[row][column] = slotValue + "-" + getAlphaFromNumber((byte)(ttI + 1));
                                }
                                else if (lstCellValues.Contains(dsTimeTable.Tables[ttI].Rows[row][column].ToString().Trim().ToLower()))
                                {
                                    dtRoomTable.Rows[row][column] = slotValue;
                                }
                            }
                        }
                    }
                    dsRoomTables.Tables.Add(dtRoomTable);
                }

                #region Display generated Tables
                lstCellValues.Add("");
                //Adding Colors
                ArrayList arrSubName = new ArrayList();

                //Building an HTML string.
                StringBuilder html = new StringBuilder();
                for (int ttI = 0; ttI < dsRoomTables.Tables.Count; ttI++)
                {
                    Dictionary<string, string> dicFooter = new Dictionary<string, string>();

                    StringBuilder sbFooter = new StringBuilder();
                    sbFooter.Append("<table cellpadding='0' cellspacing='0' style=' width:920px; font-size:10px;'><tr style='background-color:#3B6D93;color:#FFFFFF;'><td>Staff Code</td><td>Staff Name</td><td>Subject Code</td><td>Subject Name</td><td>Mon</td><td>Tue</td><td>Wed</td><td>Thu</td><td>Fri</td></tr>");
                    Hashtable htFooter = new Hashtable();

                    html.Append("<center><span style='color: Green; font-size:medium;'>" + dsRoomTables.Tables[ttI].TableName + "</span></center><br/>");
                    //Table start.
                    html.Append("<table cellpadding='0' cellspacing='0' style=' border:1px solid black; border-radius:5px; text-align:center; width:920px; font-size:10px;'>");
                    int cnt = 1;
                    //Building the Last row.
                    html.Append("<tr  style='background-color:#3B6D93;color:#FFFFFF; font-size:12px;'>");
                    foreach (DataColumn column in dsRoomTables.Tables[ttI].Columns)
                    {
                        html.Append("<td>");
                        html.Append(dsRoomTables.Tables[ttI].Rows[dsRoomTables.Tables[ttI].Rows.Count - 1][column.ColumnName]);
                        html.Append("</td>");
                    }
                    html.Append("</tr>");
                    //Building the Header row.
                    html.Append("<tr style='background-color:#3B6D93;color:#FFFFFF; font-size:12px;'>");
                    foreach (DataColumn column in dsRoomTables.Tables[ttI].Columns)
                    {
                        html.Append("<td>");
                        html.Append(column.ColumnName);
                        html.Append("</td>");
                    }
                    html.Append("</tr>");

                    //Building the Data rows.
                    foreach (DataRow row in dsRoomTables.Tables[ttI].Rows)
                    {
                        if (cnt == dsRoomTables.Tables[ttI].Rows.Count)
                        {
                            continue;
                        }
                        cnt++;
                        html.Append("<tr>");

                        foreach (DataColumn column in dsRoomTables.Tables[ttI].Columns)
                        {
                            string slotValue = row[column.ColumnName].ToString().Trim();
                            if (!lstCellValues.Contains(slotValue.ToLower()))
                            {
                                if (!arrSubName.Contains(slotValue.Split('-')[0]))
                                    arrSubName.Add(slotValue.Split('-')[0]);
                                int index = arrSubName.IndexOf(slotValue.Split('-')[0]);
                                string bgcolor = getColor(index);
                                html.Append("<td style='background-color:" + bgcolor + "'>");
                            }
                            else
                            {
                                if (string.IsNullOrEmpty(slotValue))
                                {
                                    html.Append("<td style='background-color:#FFFFFF;'>");
                                }
                                else
                                {
                                    html.Append("<td style='background-color:#3B6D93;color:#FFFFFF; font-size:12px;'>");
                                }
                            }
                            html.Append(slotValue);
                            html.Append("</td>");

                            #region DisplayFooter

                            if (slotValue.Contains("$"))
                            {
                                string[] cellvalues = slotValue.Split('$');//Split from room
                                string[] cellfnlValues = cellvalues[0].Split(',');// split subjects and staffs if elective
                                foreach (string cellfnlValue in cellfnlValues)
                                {
                                    string[] resValues = cellfnlValue.Split('-');//split subject from staff
                                    string subject_code = resValues[0];
                                    string[] staff_codes = resValues[1].Split('/');
                                    int subI = 0;
                                    string[] subject_codes = subject_code.Split('#');
                                    foreach (string staffcode in staff_codes)
                                    {
                                        dtStaffSubDet.DefaultView.RowFilter = "subject_code='" + subject_codes[subI] + "' and staff_code='" + staffcode + "'";
                                        DataView dvNew = dtStaffSubDet.DefaultView;
                                        if (dvNew.Count > 0)
                                        {

                                            if (!htFooter.Contains(staffcode + "_" + subject_codes[subI]))
                                            {
                                                string disp = "<tr><td>" + staffcode + "</td><td>" + dvNew[0]["staff_name"].ToString() + "</td><td>" + subject_codes[subI] + "</td><td>" + dvNew[0]["subject_name"].ToString() + "</td><td>M1Mon</td><td>T2Tue</td><td>W3Wed</td><td>T4Thu</td><td>F5Fri</td></tr>";

                                                htFooter.Add(staffcode + "_" + subject_codes[subI], htFooter.Count);
                                                //sbFooter.Append(disp);

                                                string labHour = string.Empty;
                                                int curCol = Convert.ToInt32(column.ColumnName);
                                                int nexCol = 0;
                                                if (dsRoomTables.Tables[ttI].Columns.Contains((curCol + 1).ToString()) && int.TryParse((curCol + 1).ToString(), out nexCol))
                                                {
                                                    //string nextVal = row[(curCol + 1).ToString()].ToString().Trim();
                                                    //if (slotValue == nextVal)
                                                    //    labHour = "," + (Convert.ToInt32(column.ColumnName) + 1).ToString();
                                                }

                                                switch (row[0].ToString())
                                                {
                                                    case "Monday":
                                                        disp = disp.Replace("M1Mon", column.ColumnName + labHour + ",M1Mon");
                                                        break;
                                                    case "Tuesday":
                                                        disp = disp.Replace("T2Tue", column.ColumnName + labHour + ",T2Tue");
                                                        break;
                                                    case "Wednesday":
                                                        disp = disp.Replace("W3Wed", column.ColumnName + labHour + ",W3Wed");
                                                        break;
                                                    case "Thursday":
                                                        disp = disp.Replace("T4Thu", column.ColumnName + labHour + ",T4Thu");
                                                        break;
                                                    case "Friday":
                                                        disp = disp.Replace("F5Fri", column.ColumnName + labHour + ",F5Fri");
                                                        break;
                                                }
                                                dicFooter.Add(staffcode + "_" + subject_codes[subI], disp);
                                            }
                                            else
                                            {

                                                string labHour = string.Empty;
                                                int curCol = Convert.ToInt32(column.ColumnName);
                                                int nexCol = 0;
                                                if (dsRoomTables.Tables[ttI].Columns.Contains((curCol + 1).ToString()) && int.TryParse((curCol + 1).ToString(), out nexCol))
                                                {
                                                    //string nextVal = row[(curCol + 1).ToString()].ToString().Trim();
                                                    //if (slotValue == nextVal)
                                                    //    labHour = "," + (Convert.ToInt32(column.ColumnName) + 1).ToString();
                                                }

                                                string disp = dicFooter[staffcode + "_" + subject_codes[subI]];
                                                switch (row[0].ToString())
                                                {
                                                    case "Monday":
                                                        disp = disp.Replace("M1Mon", column.ColumnName + labHour + ",M1Mon");
                                                        break;
                                                    case "Tuesday":
                                                        disp = disp.Replace("T2Tue", column.ColumnName + labHour + ",T2Tue");
                                                        break;
                                                    case "Wednesday":
                                                        disp = disp.Replace("W3Wed", column.ColumnName + labHour + ",W3Wed");
                                                        break;
                                                    case "Thursday":
                                                        disp = disp.Replace("T4Thu", column.ColumnName + labHour + ",T4Thu");
                                                        break;
                                                    case "Friday":
                                                        disp = disp.Replace("F5Fri", column.ColumnName + labHour + ",F5Fri");
                                                        break;
                                                }
                                                dicFooter[staffcode + "_" + subject_codes[subI]] = disp;
                                            }
                                        }
                                        if (subject_codes.Length == staff_codes.Length)
                                        {
                                            subI++;//increase subject code only when two staffs & subjects available (i.e, combined lab)
                                        }
                                    }
                                }
                            }

                            #endregion
                        }
                        html.Append("</tr>");
                    }
                    foreach (KeyValuePair<string, string> foot in dicFooter)
                    {
                        sbFooter.Append(foot.Value);
                    }
                    sbFooter.Append("</table><br>");
                    sbFooter.Replace(",M1Mon", "");
                    sbFooter.Replace(",T2Tue", "");
                    sbFooter.Replace(",W3Wed", "");
                    sbFooter.Replace(",T4Thu", "");
                    sbFooter.Replace(",F5Fri", "");

                    sbFooter.Replace("M1Mon", "-");
                    sbFooter.Replace("T2Tue", "-");
                    sbFooter.Replace("W3Wed", "-");
                    sbFooter.Replace("T4Thu", "-");
                    sbFooter.Replace("F5Fri", "-");
                    //Table end.
                    html.Append("</table><br>");
                    html.Append(sbFooter.ToString());



                }
                //Append the HTML string to Placeholder.
                divTimeTableOutput.Visible = true;
                phTimeTable.Controls.Add(new Literal { Text = html.ToString() });

                #endregion
            }
        }
        catch { }
    }
    //Staff Time Tabled
    private void bindStaff()
    {
        try
        {
            ddlStaffTT.Items.Clear();
            DataTable dtStaffs = dirAccess.selectDataTable("select appl_id,staff_name+'-'+staff_code as staff from  staff_appl_master sa, staffmaster sf where sa.appl_no=sf.appl_no order by staff_code");
            if (dtStaffs.Rows.Count > 0)
            {
                ddlStaffTT.DataSource = dtStaffs;
                ddlStaffTT.DataTextField = "staff";
                ddlStaffTT.DataValueField = "appl_id";
                ddlStaffTT.DataBind();

                ddlStaffTT.Items.Insert(0, "All");
            }
        }
        catch { }
    }
    protected void btnStaffTimeTable_Click(object sender, EventArgs e)
    {
        try
        {
            // ddlSelectedTimeTable.Items.Clear();
            DataSet dsTimeTable = new DataSet();

            if (Session["selectedDataSet"] != null)
            {
                dsTimeTable = (DataSet)Session["selectedDataSet"];
            }

            DataTable dtStaffDet = dirAccess.selectDataTable("select staff_appno ,DaysFK ,HoursFK , degreeCode ,batch_year , semester ,IsEngaged ,subject_no ,section  from TT_Staff_AllotAvail ");//  where Batch_Year='" + batchYear + "' and degreecode ='" + degreeCode + "' and semester = '" + currentSem + "'

            DataTable dtStaffSubDet = dirAccess.selectDataTable("select sa.appl_id,sm.syll_code,ss.subType_no,ss.subject_type,isnull(ss.ElectivePap,0) as ElectivePap,ss.Lab,s.subject_no,staff_name,s.subject_code,s.subject_name,isnull(s.sub_lab,0) as sub_lab,isnull(s.noofhrsperweek,0) as noofhrsperweek,s.maximumHrsPerDay,sts.staff_code,isnull(s.subjectpriority,0) as subjectpriority,sts.staffPriority, sts.facultyChoice   from  staff_appl_master sa, staffmaster sf ,syllabus_master sm,sub_sem ss, subject s,staff_selector sts where sm.syll_code = ss.syll_code and sm.syll_code = s.syll_code and ss.syll_code= s.syll_code and ss.subType_no = s.subType_no and s.subject_no =sts.subject_no and sa.appl_no = sf.appl_no and sf.staff_code=sts.staff_code and sf.college_code ='" + collegecode + "'");
            if (dsTimeTable.Tables.Count > 0 && dtStaffSubDet.Rows.Count > 0 && dtStaffDet.Rows.Count > 0)
            {
                List<string> lstCellValues = new List<string>();
                lstCellValues.Add("monday");
                lstCellValues.Add("tuesday");
                lstCellValues.Add("wednesday");
                lstCellValues.Add("thursday");
                lstCellValues.Add("friday");

                string degreeCode = string.Empty;
                DataTable dtStaffName = new DataTable();
                string selQ = " select appl_id,staff_code,staff_name from  staff_appl_master sa, staffmaster sf where sa.appl_no=sf.appl_no order by staff_code ";
                if (ddlStaffTT.Items.Count > 0 && ddlStaffTT.SelectedIndex > 0)
                {
                    selQ = " select appl_id,staff_code,staff_name from  staff_appl_master sa, staffmaster sf where sa.appl_no=sf.appl_no and appl_id='" + ddlStaffTT.SelectedValue + "' ";
                }
                dtStaffName = dirAccess.selectDataTable(selQ);

                DataSet dsStaffTables = new DataSet();

                foreach (DataRow drStaff in dtStaffName.Rows)
                {
                    string applId = Convert.ToString(drStaff["appl_id"]).Trim();
                    string staffcode = Convert.ToString(drStaff["staff_code"]);
                    string staffName = Convert.ToString(drStaff["staff_name"]);

                    DataTable dtStaffTable = dsTimeTable.Tables[0].Copy();//Copy the Structure
                    dtStaffTable.TableName = "Staff - " + staffName + " - " + staffcode;

                    for (int row = 0; row < dtStaffTable.Rows.Count - 1; row++)
                    {
                        for (int column = 1; column < dtStaffTable.Columns.Count; column++)
                        {
                            dtStaffTable.Rows[row][column] = string.Empty;
                        }
                    }

                    for (int ttI = 0; ttI < dsTimeTable.Tables.Count; ttI++)
                    {
                        for (int row = 0; row < dsTimeTable.Tables[ttI].Rows.Count - 1; row++)
                        {
                            for (int column = 0; column < dsTimeTable.Tables[ttI].Columns.Count; column++)
                            {
                                string slotValue = dsTimeTable.Tables[ttI].Rows[row][column].ToString().Trim();
                                if (slotValue.Contains(staffcode))
                                {
                                    dtStaffTable.Rows[row][column] = slotValue + "-" + getAlphaFromNumber((byte)(ttI + 1));
                                }
                                else if (lstCellValues.Contains(dsTimeTable.Tables[ttI].Rows[row][column].ToString().Trim().ToLower()))
                                {
                                    dtStaffTable.Rows[row][column] = slotValue;
                                }
                                else
                                {
                                    int hoursFk = 0;
                                    if (int.TryParse(dsTimeTable.Tables[ttI].Columns[column].ColumnName, out hoursFk))
                                    {
                                        dtStaffDet.DefaultView.RowFilter = "staff_appno='" + applId + "' and DaysFK='" + (row + 1) + "' and HoursFK='" + hoursFk + "' and IsEngaged = 'True' ";
                                        DataView dvStafEng = dtStaffDet.DefaultView;
                                        if (dvStafEng.Count > 0)
                                        {
                                            dtStaffTable.Rows[row][column] = "ENG" + "-" + staffcode + "$" + "ROOM";
                                        }
                                    }
                                }
                            }
                        }
                    }
                    dsStaffTables.Tables.Add(dtStaffTable);
                }

                #region Display generated Tables
                lstCellValues.Add("");
                //Adding Colors
                ArrayList arrSubName = new ArrayList();

                //Building an HTML string.
                StringBuilder html = new StringBuilder();
                for (int ttI = 0; ttI < dsStaffTables.Tables.Count; ttI++)
                {
                    Dictionary<string, string> dicFooter = new Dictionary<string, string>();

                    StringBuilder sbFooter = new StringBuilder();
                    sbFooter.Append("<table cellpadding='0' cellspacing='0' style=' width:920px; font-size:10px;'><tr style='background-color:#3B6D93;color:#FFFFFF;'><td>Staff Code</td><td>Staff Name</td><td>Subject Code</td><td>Subject Name</td><td>Mon</td><td>Tue</td><td>Wed</td><td>Thu</td><td>Fri</td></tr>");
                    Hashtable htFooter = new Hashtable();

                    html.Append("<center><span style='color: Green; font-size:medium;'>" + dsStaffTables.Tables[ttI].TableName + "</span></center><br/>");
                    //Table start.
                    html.Append("<table cellpadding='0' cellspacing='0' style=' border:1px solid black; border-radius:5px; text-align:center; width:920px; font-size:10px;'>");
                    int cnt = 1;
                    //Building the Last row.
                    html.Append("<tr  style='background-color:#3B6D93;color:#FFFFFF; font-size:12px;'>");
                    foreach (DataColumn column in dsStaffTables.Tables[ttI].Columns)
                    {
                        html.Append("<td>");
                        html.Append(dsStaffTables.Tables[ttI].Rows[dsStaffTables.Tables[ttI].Rows.Count - 1][column.ColumnName]);
                        html.Append("</td>");
                    }
                    html.Append("</tr>");
                    //Building the Header row.
                    html.Append("<tr style='background-color:#3B6D93;color:#FFFFFF; font-size:12px;'>");
                    foreach (DataColumn column in dsStaffTables.Tables[ttI].Columns)
                    {
                        html.Append("<td>");
                        html.Append(column.ColumnName);
                        html.Append("</td>");
                    }
                    html.Append("</tr>");

                    //Building the Data rows.
                    foreach (DataRow row in dsStaffTables.Tables[ttI].Rows)
                    {
                        if (cnt == dsStaffTables.Tables[ttI].Rows.Count)
                        {
                            continue;
                        }
                        cnt++;
                        html.Append("<tr>");

                        foreach (DataColumn column in dsStaffTables.Tables[ttI].Columns)
                        {
                            string slotValue = row[column.ColumnName].ToString().Trim();
                            if (!lstCellValues.Contains(slotValue.ToLower()))
                            {
                                if (!arrSubName.Contains(slotValue.Split('-')[0]))
                                    arrSubName.Add(slotValue.Split('-')[0]);
                                int index = arrSubName.IndexOf(slotValue.Split('-')[0]);
                                string bgcolor = getColor(index);
                                html.Append("<td style='background-color:" + bgcolor + "'>");
                            }
                            else
                            {
                                if (string.IsNullOrEmpty(slotValue))
                                {
                                    html.Append("<td style='background-color:#FFFFFF;'>");
                                }
                                else
                                {
                                    html.Append("<td style='background-color:#3B6D93;color:#FFFFFF; font-size:12px;'>");
                                }
                            }
                            html.Append(slotValue);
                            html.Append("</td>");

                            #region DisplayFooter

                            if (slotValue.Contains("$"))
                            {
                                string[] cellvalues = slotValue.Split('$');//Split from room
                                string[] electRooms = cellvalues[1].Split(',');
                                int staffRoomIndx = 0;
                                string[] cellfnlValues = cellvalues[0].Split(',');// split subjects and staffs if elective
                                foreach (string cellfnlValue in cellfnlValues)
                                {
                                    string[] resValues = cellfnlValue.Split('-');//split subject from staff
                                    string subject_code = resValues[0];
                                    string[] staff_codes = resValues[1].Split('/');
                                    int subI = 0;
                                    string[] subject_codes = subject_code.Split('#');
                                    foreach (string staffcode in staff_codes)
                                    {
                                        string curRoom = electRooms.Length > 1 ? electRooms[staffRoomIndx] : electRooms[0];
                                        staffRoomIndx++;

                                        dtStaffSubDet.DefaultView.RowFilter = "subject_code='" + subject_codes[subI] + "' and staff_code='" + staffcode + "'";
                                        DataView dvNew = dtStaffSubDet.DefaultView;
                                        if (dvNew.Count > 0)
                                        {

                                            if (!htFooter.Contains(staffcode + "_" + subject_codes[subI]))
                                            {
                                                string disp = "<tr><td>" + staffcode + "</td><td>" + dvNew[0]["staff_name"].ToString() + "</td><td>" + subject_codes[subI] + "</td><td>" + dvNew[0]["subject_name"].ToString() + "</td><td>M1Mon</td><td>T2Tue</td><td>W3Wed</td><td>T4Thu</td><td>F5Fri</td></tr>";

                                                htFooter.Add(staffcode + "_" + subject_codes[subI], htFooter.Count);
                                                //sbFooter.Append(disp);

                                                string labHour = string.Empty;
                                                int curCol = Convert.ToInt32(column.ColumnName);
                                                int nexCol = 0;
                                                if (dsStaffTables.Tables[ttI].Columns.Contains((curCol + 1).ToString()) && int.TryParse((curCol + 1).ToString(), out nexCol))
                                                {
                                                    //string nextVal = row[(curCol + 1).ToString()].ToString().Trim().Split('-')[0];
                                                    //if (subject_code == nextVal)
                                                    //    labHour = "," + (Convert.ToInt32(column.ColumnName) + 1).ToString();
                                                }

                                                switch (row[0].ToString())
                                                {
                                                    case "Monday":
                                                        disp = disp.Replace("M1Mon", column.ColumnName + labHour + "-" + curRoom + " " + ",M1Mon");
                                                        break;
                                                    case "Tuesday":
                                                        disp = disp.Replace("T2Tue", column.ColumnName + labHour + "-" + curRoom + " " + ",T2Tue");
                                                        break;
                                                    case "Wednesday":
                                                        disp = disp.Replace("W3Wed", column.ColumnName + labHour + "-" + curRoom + " " + ",W3Wed");
                                                        break;
                                                    case "Thursday":
                                                        disp = disp.Replace("T4Thu", column.ColumnName + labHour + "-" + curRoom + " " + ",T4Thu");
                                                        break;
                                                    case "Friday":
                                                        disp = disp.Replace("F5Fri", column.ColumnName + labHour + "-" + curRoom + " " + ",F5Fri");
                                                        break;
                                                }
                                                dicFooter.Add(staffcode + "_" + subject_codes[subI], disp);
                                            }
                                            else
                                            {

                                                string labHour = string.Empty;
                                                int curCol = Convert.ToInt32(column.ColumnName);
                                                int nexCol = 0;
                                                if (dsStaffTables.Tables[ttI].Columns.Contains((curCol + 1).ToString()) && int.TryParse((curCol + 1).ToString(), out nexCol))
                                                {
                                                    //string nextVal = row[(curCol + 1).ToString()].ToString().Trim().Split('-')[0];
                                                    //if (subject_code == nextVal)
                                                    //    labHour = "," + (Convert.ToInt32(column.ColumnName) + 1).ToString();
                                                }

                                                string disp = dicFooter[staffcode + "_" + subject_codes[subI]];
                                                switch (row[0].ToString())
                                                {
                                                    case "Monday":
                                                        disp = disp.Replace("M1Mon", column.ColumnName + labHour + "-" + curRoom + " " + ",M1Mon");
                                                        break;
                                                    case "Tuesday":
                                                        disp = disp.Replace("T2Tue", column.ColumnName + labHour + "-" + curRoom + " " + ",T2Tue");
                                                        break;
                                                    case "Wednesday":
                                                        disp = disp.Replace("W3Wed", column.ColumnName + labHour + "-" + curRoom + " " + ",W3Wed");
                                                        break;
                                                    case "Thursday":
                                                        disp = disp.Replace("T4Thu", column.ColumnName + labHour + "-" + curRoom + " " + ",T4Thu");
                                                        break;
                                                    case "Friday":
                                                        disp = disp.Replace("F5Fri", column.ColumnName + labHour + "-" + curRoom + " " + ",F5Fri");
                                                        break;
                                                }
                                                dicFooter[staffcode + "_" + subject_codes[subI]] = disp;
                                            }
                                        }
                                        if (subject_codes.Length == staff_codes.Length)
                                        {
                                            subI++;//increase subject code only when two staffs & subjects available (i.e, combined lab)
                                        }
                                    }
                                }
                            }

                            #endregion
                        }
                        html.Append("</tr>");
                    }
                    foreach (KeyValuePair<string, string> foot in dicFooter)
                    {
                        sbFooter.Append(foot.Value);
                    }
                    sbFooter.Append("</table><br>");
                    sbFooter.Replace(",M1Mon", "");
                    sbFooter.Replace(",T2Tue", "");
                    sbFooter.Replace(",W3Wed", "");
                    sbFooter.Replace(",T4Thu", "");
                    sbFooter.Replace(",F5Fri", "");

                    sbFooter.Replace("M1Mon", "-");
                    sbFooter.Replace("T2Tue", "-");
                    sbFooter.Replace("W3Wed", "-");
                    sbFooter.Replace("T4Thu", "-");
                    sbFooter.Replace("F5Fri", "-");

                    //Table end.
                    html.Append("</table><br>");
                    html.Append(sbFooter.ToString());



                }
                //Append the HTML string to Placeholder.
                divTimeTableOutput.Visible = true;
                phTimeTable.Controls.Add(new Literal { Text = html.ToString() });

                #endregion
            }
        }
        catch { }
    }
    //Last Modified by Idhris -- 12-03-2017

    //Raja 10-03-2017
    #region TimeTable Criteria Import

    #region  PopUpErr Close

    protected void btnPopErrclose_Click(object sender, EventArgs e)
    {
        popuperr.Visible = false;
        lbl_popuperr.Text = string.Empty;
    }

    #endregion  PopUpErr Close

    public void imgbtnNotInserted_Click(object sender, EventArgs e)
    {
        divNotInserted.Visible = false;
    }

    private void ImportTimeTableCriteria()
    {
        try
        {
            Hashtable htDBColumnMapping = new Hashtable();
            htDBColumnMapping.Clear();
            htDBColumnMapping.Add("timetable option", "CriteriaName");
            htDBColumnMapping.Add("monday", "DayPk");
            htDBColumnMapping.Add("tuesday", "DayPk");
            htDBColumnMapping.Add("wednesday", "DayPk");
            htDBColumnMapping.Add("thursday", "DayPk");
            htDBColumnMapping.Add("friday", "DayPk");
            htDBColumnMapping.Add("saturday", "DayPk");
            htDBColumnMapping.Add("sunday", "DayPk");
            htDBColumnMapping.Add("batch", "batch_year");
            htDBColumnMapping.Add("degree", "degree_code");
            htDBColumnMapping.Add("department", "degree_code");
            htDBColumnMapping.Add("semester", "semester");
            htDBColumnMapping.Add("subject code", "subject_no");
            htDBColumnMapping.Add("subject name", "subject_no");
            htDBColumnMapping.Add("staff code", "staff_code");
            htDBColumnMapping.Add("staff name", "staff_code");

            //CriteriaPk,CriteriaName,DayPk,HourPk,IsEngaged,degree_code,batch_year,semester,subject_no,staff_code

            dicTotalHoursDetails = new Dictionary<string, byte>();
            dicAllSubjects = new Dictionary<string, string>();
            dicDegreeDetails = new Dictionary<string, string>();
            dicAllStaffDetails = new Dictionary<string, string>();
            dicStaffSelectors = new Dictionary<string, string>();
            dicBatchYearDetails = new Dictionary<string, string>();
            Dictionary<string, string> dicBatchDegreeDetails = new Dictionary<string, string>();
            Dictionary<string, string> dicSemesterDetails = new Dictionary<string, string>();
            //dicTotalHoursDetails.Clear();
            //dicAllSubjects.Clear();
            //dicDegreeDetails.Clear();
            //dicAllStaffDetails.Clear();
            //dicStaffSelectors.Clear();
            //dicBatchYearDetails.Clear();

            GetBatchYearDetails(out dicBatchYearDetails);
            GetDegreeDetails(out dicDegreeDetails);
            GetHoursDetails(out dicTotalHoursDetails);
            GetAllSubjects(out dicAllSubjects);
            GetAllStaffDetails(out dicAllStaffDetails);
            GetStaffSelector(out dicStaffSelectors);
            GetSemesterDetails(out dicSemesterDetails);
            bool isSavedSucc = false;
            string notInserted = string.Empty;
            bool notInsert = false;
            FarPoint.Web.Spread.FpSpread fpCriteriaTT = new FarPoint.Web.Spread.FpSpread();
            using (Stream Stream = this.fuImport.FileContent as Stream)
            {
                string extension = Path.GetFileName(fuImport.PostedFile.FileName);
                if (extension.Trim() != "")
                {
                    if (fuImport.FileName.EndsWith(".xls") || fuImport.FileName.EndsWith(".xlsx"))
                    {
                        Stream.Position = 0;
                        fpCriteriaTT.OpenExcel(Stream);
                        fpCriteriaTT.SaveChanges();
                        if (fpCriteriaTT.Sheets[0].Rows.Count > 0)
                        {
                            int countcol = 0;
                            string invalidColumn = string.Empty;
                            bool hasInvalidColumn = false;
                            bool isStaffCodeCol = false;
                            bool isSubcodecol = false;

                            bool isTimetableCriteriaCol = false;
                            bool isMonCol = false;
                            bool isTusCol = false;
                            bool isWedCol = false;
                            bool isThurCol = false;
                            bool isFriCol = false;
                            bool isSatCol = false;
                            bool isSunCol = false;
                            bool isBatchCol = false;
                            bool isDegreeCol = false;
                            bool isDeptCol = false;
                            bool isSemCol = false;
                            bool isSubNameCol = false;
                            bool isStaffNameCol = false;

                            string mandadoryMiss = string.Empty;
                            for (int i = 0; i < fpCriteriaTT.Sheets[0].Columns.Count; i++)
                            {
                                bool isMand = false;
                                string colname = Convert.ToString(fpCriteriaTT.Sheets[0].Cells[0, i].Text).Trim().ToLower();
                                if (colname != "")
                                {
                                    if (colname.Trim().ToLower() == "timetable option")
                                    {
                                        countcol++;
                                        isTimetableCriteriaCol = true;
                                        isMand = true;
                                    }
                                    else if (colname.Trim().ToLower() == "monday")
                                    {
                                        countcol++;
                                        isMonCol = true;
                                        isMand = true;
                                    }
                                    else if (colname.Trim().ToLower() == "tuesday")
                                    {
                                        countcol++;
                                        isTusCol = true;
                                        isMand = true;
                                    }
                                    else if (colname.Trim().ToLower() == "wednesday")
                                    {
                                        countcol++;
                                        isWedCol = true;
                                        isMand = true;
                                    }
                                    else if (colname.Trim().ToLower() == "thursday")
                                    {
                                        countcol++;
                                        isThurCol = true;
                                        isMand = true;
                                    }
                                    else if (colname.Trim().ToLower() == "friday")
                                    {
                                        countcol++;
                                        isFriCol = true;
                                        isMand = true;
                                    }
                                    else if (colname.Trim().ToLower() == "saturday")
                                    {
                                        countcol++;
                                        isSatCol = true;
                                        isMand = true;
                                    }
                                    else if (colname.Trim().ToLower() == "sunday")
                                    {
                                        countcol++;
                                        isSunCol = true;
                                        isMand = true;
                                    }
                                    else if (colname.Trim().ToLower() == "batch")
                                    {
                                        countcol++;
                                        isBatchCol = true;
                                        isMand = true;
                                    }
                                    else if (colname.Trim().ToLower() == "degree")
                                    {
                                        countcol++;
                                        isDegreeCol = true;
                                        isMand = true;
                                    }
                                    else if (colname.Trim().ToLower() == "department")
                                    {
                                        countcol++;
                                        isDeptCol = true;
                                        isMand = true;
                                    }
                                    else if (colname.Trim().ToLower() == "semester")
                                    {
                                        countcol++;
                                        isSemCol = true;
                                        isMand = true;
                                    }
                                    else if (colname.Trim().ToLower() == "subject code")
                                    {
                                        countcol++;
                                        isSubcodecol = true;
                                        isMand = true;
                                    }
                                    else if (colname.Trim().ToLower() == "subject name")
                                    {
                                        countcol++;
                                        isSubNameCol = true;
                                        isMand = true;
                                    }
                                    else if (colname.Trim().ToLower() == "staff code")
                                    {
                                        countcol++;
                                        isStaffCodeCol = true;
                                        isMand = true;
                                    }
                                    else if (colname.Trim().ToLower() == "staff name")
                                    {
                                        countcol++;
                                        isStaffNameCol = true;
                                        isMand = true;
                                    }
                                    if (!htDBColumnMapping.Contains(colname))
                                    {
                                        hasInvalidColumn = true;
                                        if (invalidColumn == "")
                                        {
                                            invalidColumn = fpCriteriaTT.Sheets[0].Cells[0, i].Text;
                                        }
                                        else
                                        {
                                            invalidColumn += "," + fpCriteriaTT.Sheets[0].Cells[0, i].Text;
                                        }
                                    }
                                }
                            }
                            if (hasInvalidColumn)
                            {
                                txtNotInserted.Text = "Invalid Columns : " + invalidColumn;
                                divNotInserted.Visible = true;
                                return;
                            }
                            if (countcol != 16)
                            {
                                string mand = ((!isTimetableCriteriaCol) ? "TimeTable Option," : "") + ((!isMonCol) ? "Monday," : "") + ((!isTusCol) ? "Tuesday," : "") + ((!isWedCol) ? "Wednesday," : "") + ((!isThurCol) ? "Thursday," : "") + ((!isFriCol) ? "Friday," : "") + ((!isSatCol) ? "Saturday," : "") + ((!isSunCol) ? "Sunday," : "") + ((!isBatchCol) ? "Batch," : "") + ((!isDegreeCol) ? "Degree," : "") + ((!isDeptCol) ? "Department," : "") + ((!isSemCol) ? "Semester," : "") + ((!isSubcodecol) ? "Subject Code," : "") + ((!isSubNameCol) ? "Subject Name," : "") + ((!isStaffCodeCol) ? "Staff Code," : "") + ((!isStaffNameCol) ? "Staff Name," : "");
                                mand = mand.Trim(',');
                                txtNotInserted.Text = "Madatory Columns " + mand.Trim(',') + (((mand.Split(',')).Length > 1) ? " Are " : " is ") + "missing";
                                divNotInserted.Visible = true;
                                return;
                            }
                            for (int i = 1; i < fpCriteriaTT.Sheets[0].Rows.Count; i++)
                            {
                                string updtquery = string.Empty;
                                string creatqury = string.Empty;
                                string creatvalu = string.Empty;

                                string crteriaName = string.Empty;
                                string monDay = string.Empty;
                                string tuesDay = string.Empty;
                                string wedDay = string.Empty;
                                string thursDay = string.Empty;
                                string friDay = string.Empty;
                                string sadDay = string.Empty;
                                string sunDay = string.Empty;
                                string batchYear = string.Empty;
                                string degreeName = string.Empty;
                                string departmentName = string.Empty;
                                string degreeCode = string.Empty;
                                string semester = string.Empty;
                                string subjectCode = string.Empty;
                                string subjectName = string.Empty;
                                string staffCode = string.Empty;
                                string staffName = string.Empty;

                                bool isTimeTableCriteriaAvail = false;
                                bool isBatchYearAvail = false;
                                bool isBatchYearValid = false;

                                bool isStaffCodeAvail = false;
                                bool isStaffCodeValid = false;

                                bool isSubjectCodeAvail = false;
                                bool isSubjectCodeValid = false;

                                bool isDegreeAvail = false;
                                bool isDepartmentAvail = false;
                                bool isDegreeCodeAvail = false;
                                bool isDegreeCodeValid = false;

                                bool isSemesterAvail = false;
                                bool isSemesterValid = false;

                                bool isHourValid = false;

                                bool isStaffSelectors = false;

                                int totalHours = 0;
                                Dictionary<byte, byte[]> dicDays = new Dictionary<byte, byte[]>();
                                dicDays.Clear();
                                byte[] hours = new byte[0];
                                for (byte day = 1; day <= 7; day++)
                                {
                                    if (!dicDays.ContainsKey(day))
                                    {
                                        dicDays.Add(day, hours);
                                    }
                                }
                                for (int col = 0; col < fpCriteriaTT.Sheets[0].Columns.Count; col++)
                                {
                                    string colname = Convert.ToString(fpCriteriaTT.Sheets[0].Cells[0, col].Text).Trim().ToLower();
                                    string dbtablecolname = Convert.ToString(htDBColumnMapping[colname]);
                                    string Values = Convert.ToString(fpCriteriaTT.Sheets[0].Cells[i, col].Text).Trim();
                                    bool isAvail = false;

                                    if (string.IsNullOrEmpty(Values.Trim()))
                                    {
                                        isAvail = false;
                                    }
                                    else
                                    {
                                        isAvail = true;
                                    }
                                    if (colname.Trim().ToLower() == "timetable option")
                                    {
                                        isTimeTableCriteriaAvail = isAvail;
                                        crteriaName = Values.Trim();
                                        if (isAvail)
                                        {
                                            updtquery += "," + dbtablecolname + "='" + Values + "'";
                                            creatqury += "," + dbtablecolname + "";
                                            creatvalu += ",'" + Values + "'";
                                        }
                                    }
                                    else if (colname.Trim().ToLower() == "monday")
                                    {
                                        //updtquery += "," + dbtablecolname + "='" + Values + "'";
                                        //creatqury += "," + dbtablecolname + "";
                                        //creatvalu += ",'" + Values + "'";
                                        string[] hourList = Values.Split(',');
                                        byte[] hourID = new byte[0];
                                        if (isAvail)
                                        {
                                            foreach (string h in hourList)
                                            {
                                                byte newHr = 0;
                                                if (string.IsNullOrEmpty(h) || h.Trim() == "0" || h == "-1" || h.Trim().ToLower() == "nil")
                                                {
                                                    Array.Resize(ref hourID, hourID.Length + 1);
                                                    hourID[hourID.Length - 1] = 0;
                                                }
                                                else
                                                {
                                                    byte.TryParse(h.Trim(), out newHr);
                                                    Array.Resize(ref hourID, hourID.Length + 1);
                                                    hourID[hourID.Length - 1] = newHr;
                                                }
                                            }
                                        }
                                        else
                                        {
                                            Array.Resize(ref hourID, hourID.Length + 1);
                                            hourID[hourID.Length - 1] = 0;
                                        }
                                        if (dicDays.ContainsKey(1))
                                        {
                                            dicDays[1] = hourID;
                                        }
                                    }
                                    else if (colname.Trim().ToLower() == "tuesday")
                                    {
                                        //updtquery += "," + dbtablecolname + "='" + Values + "'";
                                        //creatqury += "," + dbtablecolname + "";
                                        //creatvalu += ",'" + Values + "'";
                                        string[] hourList = Values.Split(',');
                                        byte[] hourID = new byte[0];
                                        if (isAvail)
                                        {
                                            foreach (string h in hourList)
                                            {
                                                byte newHr = 0;
                                                if (string.IsNullOrEmpty(h) || h.Trim() == "0" || h == "-1" || h.Trim().ToLower() == "nil")
                                                {
                                                    Array.Resize(ref hourID, hourID.Length + 1);
                                                    hourID[hourID.Length - 1] = 0;
                                                }
                                                else
                                                {
                                                    byte.TryParse(h.Trim(), out newHr);
                                                    Array.Resize(ref hourID, hourID.Length + 1);
                                                    hourID[hourID.Length - 1] = newHr;
                                                }
                                            }
                                        }
                                        else
                                        {
                                            Array.Resize(ref hourID, hourID.Length + 1);
                                            hourID[hourID.Length - 1] = 0;
                                        }
                                        if (dicDays.ContainsKey(2))
                                        {
                                            dicDays[2] = hourID;
                                        }
                                    }
                                    else if (colname.Trim().ToLower() == "wednesday")
                                    {
                                        //updtquery += "," + dbtablecolname + "='" + Values + "'";
                                        //creatqury += "," + dbtablecolname + "";
                                        //creatvalu += ",'" + Values + "'";
                                        string[] hourList = Values.Split(',');
                                        byte[] hourID = new byte[0];
                                        if (isAvail)
                                        {
                                            foreach (string h in hourList)
                                            {
                                                byte newHr = 0;
                                                if (string.IsNullOrEmpty(h) || h.Trim() == "0" || h == "-1" || h.Trim().ToLower() == "nil")
                                                {
                                                    Array.Resize(ref hourID, hourID.Length + 1);
                                                    hourID[hourID.Length - 1] = 0;
                                                }
                                                else
                                                {
                                                    byte.TryParse(h.Trim(), out newHr);
                                                    Array.Resize(ref hourID, hourID.Length + 1);
                                                    hourID[hourID.Length - 1] = newHr;
                                                }
                                            }
                                        }
                                        else
                                        {
                                            Array.Resize(ref hourID, hourID.Length + 1);
                                            hourID[hourID.Length - 1] = 0;
                                        }
                                        if (dicDays.ContainsKey(3))
                                        {
                                            dicDays[3] = hourID;
                                        }
                                    }
                                    else if (colname.Trim().ToLower() == "thursday")
                                    {
                                        //updtquery += "," + dbtablecolname + "='" + Values + "'";
                                        //creatqury += "," + dbtablecolname + "";
                                        //creatvalu += ",'" + Values + "'";
                                        string[] hourList = Values.Split(',');
                                        byte[] hourID = new byte[0];
                                        if (isAvail)
                                        {
                                            foreach (string h in hourList)
                                            {
                                                byte newHr = 0;
                                                if (string.IsNullOrEmpty(h) || h.Trim() == "0" || h == "-1" || h.Trim().ToLower() == "nil")
                                                {
                                                    Array.Resize(ref hourID, hourID.Length + 1);
                                                    hourID[hourID.Length - 1] = 0;
                                                }
                                                else
                                                {
                                                    byte.TryParse(h.Trim(), out newHr);
                                                    Array.Resize(ref hourID, hourID.Length + 1);
                                                    hourID[hourID.Length - 1] = newHr;
                                                }
                                            }
                                        }
                                        else
                                        {
                                            Array.Resize(ref hourID, hourID.Length + 1);
                                            hourID[hourID.Length - 1] = 0;
                                        }
                                        if (dicDays.ContainsKey(4))
                                        {
                                            dicDays[4] = hourID;
                                        }
                                    }
                                    else if (colname.Trim().ToLower() == "friday")
                                    {
                                        //updtquery += "," + dbtablecolname + "='" + Values + "'";
                                        //creatqury += "," + dbtablecolname + "";
                                        //creatvalu += ",'" + Values + "'";
                                        string[] hourList = Values.Split(',');
                                        byte[] hourID = new byte[0];
                                        if (isAvail)
                                        {
                                            foreach (string h in hourList)
                                            {
                                                byte newHr = 0;
                                                if (string.IsNullOrEmpty(h) || h.Trim() == "0" || h == "-1" || h.Trim().ToLower() == "nil")
                                                {
                                                    Array.Resize(ref hourID, hourID.Length + 1);
                                                    hourID[hourID.Length - 1] = 0;
                                                }
                                                else
                                                {
                                                    byte.TryParse(h.Trim(), out newHr);
                                                    Array.Resize(ref hourID, hourID.Length + 1);
                                                    hourID[hourID.Length - 1] = newHr;
                                                }
                                            }
                                        }
                                        else
                                        {
                                            Array.Resize(ref hourID, hourID.Length + 1);
                                            hourID[hourID.Length - 1] = 0;
                                        }
                                        if (dicDays.ContainsKey(5))
                                        {
                                            dicDays[5] = hourID;
                                        }
                                    }
                                    else if (colname.Trim().ToLower() == "saturday")
                                    {
                                        //updtquery += "," + dbtablecolname + "='" + Values + "'";
                                        //creatqury += "," + dbtablecolname + "";
                                        //creatvalu += ",'" + Values + "'";
                                        string[] hourList = Values.Split(',');
                                        byte[] hourID = new byte[0];
                                        if (isAvail)
                                        {
                                            foreach (string h in hourList)
                                            {
                                                byte newHr = 0;
                                                if (string.IsNullOrEmpty(h) || h.Trim() == "0" || h == "-1" || h.Trim().ToLower() == "nil")
                                                {
                                                    Array.Resize(ref hourID, hourID.Length + 1);
                                                    hourID[hourID.Length - 1] = 0;
                                                }
                                                else
                                                {
                                                    byte.TryParse(h.Trim(), out newHr);
                                                    Array.Resize(ref hourID, hourID.Length + 1);
                                                    hourID[hourID.Length - 1] = newHr;
                                                }
                                            }
                                        }
                                        else
                                        {
                                            Array.Resize(ref hourID, hourID.Length + 1);
                                            hourID[hourID.Length - 1] = 0;
                                        }
                                        if (dicDays.ContainsKey(6))
                                        {
                                            dicDays[6] = hourID;
                                        }
                                    }
                                    else if (colname.Trim().ToLower() == "sunday")
                                    {
                                        //updtquery += "," + dbtablecolname + "='" + Values + "'";
                                        //creatqury += "," + dbtablecolname + "";
                                        //creatvalu += ",'" + Values + "'";
                                        string[] hourList = Values.Split(',');
                                        byte[] hourID = new byte[0];
                                        if (isAvail)
                                        {
                                            foreach (string h in hourList)
                                            {
                                                byte newHr = 0;
                                                if (string.IsNullOrEmpty(h) || h.Trim() == "0" || h == "-1" || h.Trim().ToLower() == "nil")
                                                {
                                                    Array.Resize(ref hourID, hourID.Length + 1);
                                                    hourID[hourID.Length - 1] = 0;
                                                }
                                                else
                                                {
                                                    byte.TryParse(h.Trim(), out newHr);
                                                    Array.Resize(ref hourID, hourID.Length + 1);
                                                    hourID[hourID.Length - 1] = newHr;
                                                }
                                            }
                                        }
                                        else
                                        {
                                            Array.Resize(ref hourID, hourID.Length + 1);
                                            hourID[hourID.Length - 1] = 0;
                                        }
                                        if (dicDays.ContainsKey(7))
                                        {
                                            dicDays[7] = hourID;
                                        }
                                    }
                                    else if (colname.Trim().ToLower() == "batch")
                                    {
                                        //updtquery += "," + dbtablecolname + "='" + Values + "'";
                                        //creatqury += "," + dbtablecolname + "";
                                        //creatvalu += ",'" + Values + "'";
                                        isBatchYearAvail = isAvail;
                                        int batch = 0;
                                        isBatchYearValid = int.TryParse(Values.Trim(), out batch);
                                        if (batch > 0)
                                            batchYear = batch.ToString();
                                        else
                                        {
                                            isBatchYearValid = false;
                                        }
                                        isBatchYearValid = isValidBatchYearDetails(ref batchYear, dicBatchYearDetails);
                                        if (isAvail && isBatchYearValid)
                                        {
                                            GetDegreeDetails(out dicBatchDegreeDetails, batchYear);
                                        }
                                    }
                                    else if (colname.Trim().ToLower() == "degree")
                                    {
                                        //updtquery += "," + dbtablecolname + "='" + Values + "'";
                                        //creatqury += "," + dbtablecolname + "";
                                        //creatvalu += ",'" + Values + "'";
                                        degreeName = Values.Trim();
                                        isDegreeAvail = isAvail;
                                        degreeCode = string.Empty;

                                    }
                                    else if (colname.Trim().ToLower() == "department")
                                    {
                                        //updtquery += "," + dbtablecolname + "='" + Values + "'";
                                        //creatqury += "," + dbtablecolname + "";
                                        //creatvalu += ",'" + Values + "'";
                                        isDepartmentAvail = isAvail;
                                        departmentName = Values.Trim();

                                    }
                                    else if (colname.Trim().ToLower() == "semester")
                                    {
                                        //updtquery += "," + dbtablecolname + "='" + Values + "'";
                                        //creatqury += "," + dbtablecolname + "";
                                        //creatvalu += ",'" + Values + "'";
                                        isSemesterAvail = isAvail;
                                        int sem = 0;
                                        semester = Values.Trim();
                                        isSemesterValid = int.TryParse(semester.Trim(), out sem);
                                        if (sem > 0)
                                        {
                                            isSemesterValid = true;
                                        }
                                        else
                                        {
                                            isSemesterValid = false;
                                        }

                                    }
                                    else if (colname.Trim().ToLower() == "subject code")
                                    {
                                        //updtquery += "," + dbtablecolname + "='" + Values + "'";
                                        //creatqury += "," + dbtablecolname + "";
                                        //creatvalu += ",'" + Values + "'";
                                        isSubjectCodeAvail = isAvail;
                                        subjectCode = Values.Trim();
                                        //isSubjectCodeValid=isValidSubjectCode(
                                    }
                                    else if (colname.Trim().ToLower() == "subject name")
                                    {
                                        //updtquery += "," + dbtablecolname + "='" + Values + "'";
                                        //creatqury += "," + dbtablecolname + "";
                                        //creatvalu += ",'" + Values + "'";
                                        subjectName = Values.Trim();
                                    }
                                    else if (colname.Trim().ToLower() == "staff code")
                                    {
                                        //updtquery += "," + dbtablecolname + "='" + Values + "'";
                                        //creatqury += "," + dbtablecolname + "";
                                        //creatvalu += ",'" + Values + "'";
                                        isStaffCodeAvail = isAvail;
                                        staffCode = Values.Trim();
                                        string newStaffName = string.Empty;
                                        isStaffCodeValid = isValidStaffCode(staffCode.Trim(), dicAllStaffDetails, ref newStaffName);
                                    }
                                    else if (colname.Trim().ToLower() == "staff name")
                                    {
                                        //updtquery += "," + dbtablecolname + "='" + Values + "'";
                                        //creatqury += "," + dbtablecolname + "";
                                        //creatvalu += ",'" + Values + "'";
                                        staffName = Values.Trim();
                                    }
                                    if ((colname.Trim().ToLower() == "monday") || (colname.Trim().ToLower() == "tuesday") || (colname.Trim().ToLower() == "wednesday") || (colname.Trim().ToLower() == "thursday") || (colname.Trim().ToLower() == "friday") || (colname.Trim().ToLower() == "saturday") || (colname.Trim().ToLower() == "sunday"))
                                    {
                                        //updtquery += "," + dbtablecolname + "='" + Values + "'";
                                        //creatqury += "," + dbtablecolname + "";
                                        //creatvalu += ",'" + Values + "'";
                                    }
                                }

                                if (isTimeTableCriteriaAvail && isBatchYearAvail && isBatchYearValid && isDegreeAvail && isDepartmentAvail && isSemesterAvail && isStaffCodeAvail && isStaffCodeValid && isSubjectCodeAvail)
                                {
                                    isDegreeCodeValid = isValidDegreeDetails(degreeName, departmentName, dicDegreeDetails, ref degreeCode);
                                    if (!string.IsNullOrEmpty(degreeCode))
                                    {
                                        isDegreeCodeAvail = true;
                                    }
                                    else
                                    {
                                        isDegreeCodeAvail = false;
                                    }
                                    bool hasBatchDegree = false;
                                    hasBatchDegree = isValidDegreeDetails(degreeName, departmentName, dicBatchDegreeDetails, ref degreeCode);
                                    string duaration = string.Empty;
                                    isSemesterValid = isValidSemesterDetails(degreeCode, dicSemesterDetails, ref semester, ref duaration);
                                    string fHr = "1";
                                    byte TotHrs = 0;
                                    isHourValid = isValidHours(degreeCode, semester, dicTotalHoursDetails, ref fHr, ref TotHrs);
                                    string subjectNo = string.Empty;
                                    string facultyChoice = string.Empty;
                                    isSubjectCodeValid = isValidSubjectCode(subjectCode, batchYear, degreeCode, semester, dicAllSubjects, ref subjectNo);
                                    isStaffSelectors = isValidStaffSelector(subjectCode, batchYear, degreeCode, semester, staffCode, dicStaffSelectors, ref facultyChoice);
                                    if (isDegreeCodeValid && isDegreeCodeAvail && hasBatchDegree && isSubjectCodeValid && isSemesterValid && isStaffSelectors && isHourValid)
                                    {
                                        string Q = string.Empty;
                                        //if exists (select criteriaPk from TT_StudentCriteria where DayPk='" + Days.Monday + "' and HourPk='" + hours + "' and CriteriaName='" + crteriaName + "' and degree_code='" + degreeCode + "' and batch_year='" + batchYear + "' and semester='" + semester + "' and subject_no='" + subjectNo + "' and staff_code='" + staffCode + "') update TT_StudentCriteria set CriteriaName='" + crteriaName + "',DayPk='" + Days.Monday + "',HourPk='" + hours + "',degree_code='" + degreeCode + "',batch_year='" + batchYear + "',semester='" + semester + "',subject_no='" + subjectNo + "',staff_code='" + staffCode + "' IsEngaged='1' where DayPk='" + Days.Monday + "' and HourPk='" + hours + "' and CriteriaName='" + crteriaName + "' and degree_code='" + degreeCode + "' and batch_year='" + batchYear + "' and semester='" + semester + "' and subject_no='" + subjectNo + "' and staff_code='" + staffCode + "' else insert into TT_StudentCriteria(CriteriaName,DayPk,HourPk,degree_code,batch_year,semester,subject_no,staff_code,IsEngaged) values ('" + crteriaName + "','" + Days.Monday + "','" + hours + "','" + degreeCode + "','" + batchYear + "','" + semester + "','" + subjectNo + "','" + staffCode + "','1')
                                        int inserted = 0;
                                        for (byte day = 1; day <= 7; day++)
                                        {
                                            byte[] engagedHrs = dicDays[day];
                                            for (byte hour = 1; hour <= TotHrs; hour++)
                                            {
                                                if (engagedHrs.Length > 0)
                                                {
                                                    for (byte hr = 0; hr < engagedHrs.Length; hr++)
                                                    {
                                                        string engageHr = Convert.ToString(engagedHrs[hr]).Trim();
                                                        byte totHrsNew = 0;
                                                        bool isValidEngageHr = isValidHours(degreeCode, semester, dicTotalHoursDetails, ref engageHr, ref totHrsNew);
                                                        if (isValidEngageHr)
                                                        {
                                                            if (hour == engagedHrs[hr])
                                                            {
                                                                Q = "if exists (select criteriaPk from TT_StudentCriteria where DayPk='" + day + "' and HourPk='" + hour + "' and CriteriaName='" + crteriaName + "' and degree_code='" + degreeCode + "' and batch_year='" + batchYear + "' and semester='" + semester + "' and subject_no='" + subjectNo + "' and staff_code='" + staffCode + "') update TT_StudentCriteria set IsEngaged='1' where DayPk='" + day + "' and HourPk='" + hour + "' and CriteriaName='" + crteriaName + "' and degree_code='" + degreeCode + "' and batch_year='" + batchYear + "' and semester='" + semester + "' and subject_no='" + subjectNo + "' and staff_code='" + staffCode + "' else insert into TT_StudentCriteria(CriteriaName,DayPk,HourPk,degree_code,batch_year,semester,subject_no,staff_code,IsEngaged) values ('" + crteriaName + "','" + day + "','" + hour + "','" + degreeCode + "','" + batchYear + "','" + semester + "','" + subjectNo + "','" + staffCode + "','1')";//CriteriaName='" + crteriaName + "',DayPk='" + day + "',HourPk='" + hour + "',degree_code='" + degreeCode + "',batch_year='" + batchYear + "',semester='" + semester + "',subject_no='" + subjectNo + "',staff_code='" + staffCode + "'
                                                                inserted = dirAccess.updateData(Q);
                                                                if (inserted > 0)
                                                                {
                                                                    isSavedSucc = true;
                                                                }
                                                            }
                                                            else
                                                            {
                                                                Q = "if exists (select criteriaPk from TT_StudentCriteria where DayPk='" + day + "' and HourPk='" + hour + "' and CriteriaName='" + crteriaName + "' and degree_code='" + degreeCode + "' and batch_year='" + batchYear + "' and semester='" + semester + "' and subject_no='" + subjectNo + "' and staff_code='" + staffCode + "') update TT_StudentCriteria set IsEngaged='0' where DayPk='" + day + "' and HourPk='" + hour + "' and CriteriaName='" + crteriaName + "' and degree_code='" + degreeCode + "' and batch_year='" + batchYear + "' and semester='" + semester + "' and subject_no='" + subjectNo + "' and staff_code='" + staffCode + "' ";//else insert into TT_StudentCriteria(CriteriaName,DayPk,HourPk,degree_code,batch_year,semester,subject_no,staff_code,IsEngaged) values ('" + crteriaName + "','" + day + "','" + hour + "','" + degreeCode + "','" + batchYear + "','" + semester + "','" + subjectNo + "','" + staffCode + "','0') CriteriaName='" + crteriaName + "',DayPk='" + day + "',HourPk='" + hour + "',degree_code='" + degreeCode + "',batch_year='" + batchYear + "',semester='" + semester + "',subject_no='" + subjectNo + "',staff_code='" + staffCode + "'
                                                                inserted = dirAccess.updateData(Q);
                                                                if (inserted > 0)
                                                                {
                                                                    isSavedSucc = true;
                                                                }
                                                            }
                                                        }
                                                        else
                                                        {
                                                            notInsert = true;
                                                            if (notInserted == "")
                                                            {
                                                                notInserted = "Excel Row" + (i + 1) + " : " + ((!isValidEngageHr) ? "Engaged Hour " + engageHr + " is Invalid!! The Maximum Available Hours is " + totHrsNew : "");
                                                            }
                                                            else
                                                            {
                                                                notInserted += ",\nExcel Row" + (i + 1) + " : " + ((!isValidEngageHr) ? "Engaged Hour " + engageHr + " is Invalid!! The Maximum Available Hours is " + totHrsNew : "");
                                                            }
                                                        }
                                                    }
                                                }
                                                else
                                                {
                                                    Q = "if exists (select criteriaPk from TT_StudentCriteria where DayPk='" + day + "' and HourPk='" + hour + "' and CriteriaName='" + crteriaName + "' and degree_code='" + degreeCode + "' and batch_year='" + batchYear + "' and semester='" + semester + "' and subject_no='" + subjectNo + "' and staff_code='" + staffCode + "') update TT_StudentCriteria set IsEngaged='0' where DayPk='" + day + "' and HourPk='" + hour + "' and CriteriaName='" + crteriaName + "' and degree_code='" + degreeCode + "' and batch_year='" + batchYear + "' and semester='" + semester + "' and subject_no='" + subjectNo + "' and staff_code='" + staffCode + "' ";//else insert into TT_StudentCriteria(CriteriaName,DayPk,HourPk,degree_code,batch_year,semester,subject_no,staff_code,IsEngaged) values ('" + crteriaName + "','" + day + "','" + hour + "','" + degreeCode + "','" + batchYear + "','" + semester + "','" + subjectNo + "','" + staffCode + "','0')  CriteriaName='" + crteriaName + "',DayPk='" + day + "',HourPk='" + hour + "',degree_code='" + degreeCode + "',batch_year='" + batchYear + "',semester='" + semester + "',subject_no='" + subjectNo + "',staff_code='" + staffCode + "'
                                                    inserted = dirAccess.updateData(Q);
                                                    if (inserted > 0)
                                                    {
                                                        isSavedSucc = true;
                                                    }
                                                }
                                            }
                                        }
                                    }
                                    else
                                    {
                                        notInsert = true;
                                        string mand = ((!isTimeTableCriteriaAvail) ? "TimeTable Option," : "") + ((!isBatchYearAvail) ? "Batch," : "") + ((!isDegreeAvail) ? "Degree," : "") + ((!isDepartmentAvail) ? "Department," : "") + ((!isSemesterAvail) ? "Semester," : "") + ((!isStaffCodeAvail) ? "Staff Code," : "") + ((!isSubjectCodeAvail) ? "Subject Code," : "");
                                        mand = mand.Trim(',');

                                        string invalid = ((!isStaffCodeValid) ? "Staff Code " + staffCode + " is Not Available" : "") + ((isBatchYearAvail && !isBatchYearValid) ? "Batch Year is Invalid or Batch Year Does Not Have Students" : "") + ((isDepartmentAvail && isDegreeAvail && !isDegreeCodeValid) ? " Degree And Deparment " + degreeName + " - " + departmentName + " is Invalid" : "") + ((isDepartmentAvail && isDegreeAvail && !hasBatchDegree) ? "Batch Year " + batchYear + " Does Not Have Student in The Deparment " + degreeName + " - " + departmentName + "" : "") + ((!isSemesterValid) ? "Semester is Invalid!! Semester Must Be Lesser Than Maximum Duration " + duaration : "") + ((!isStaffSelectors) ? "Staff Code " + staffCode + " - " + staffName + " Does Not Have Select The  Subject " + subjectCode : "");

                                        string error1 = ((mand.Trim(',') != "") ? "Madatory Columns " + mand.Trim(',') + (((mand.Split(',')).Length > 1) ? " Are " : " is ") + "Empty" : "") + invalid;
                                        if (error1.Trim() != "")
                                        {
                                            if (notInserted == "")
                                            {
                                                notInserted = "Excel Row" + (i + 1) + " : " + error1;
                                            }
                                            else
                                            {
                                                notInserted += ",\nExcel Row" + (i + 1) + " : " + error1;
                                            }
                                        }
                                    }
                                }
                                else
                                {
                                    notInsert = true;
                                    string mand = ((!isTimeTableCriteriaAvail) ? "TimeTable Option," : "") + ((!isBatchYearAvail) ? "Batch," : "") + ((!isDegreeAvail) ? "Degree," : "") + ((!isDepartmentAvail) ? "Department," : "") + ((!isSemesterAvail) ? "Semester," : "") + ((!isStaffCodeAvail) ? "Staff Code," : "") + ((!isSubjectCodeAvail) ? "Subject Code," : "");
                                    mand = mand.Trim(',');

                                    string invalid = ((!isStaffCodeValid) ? "Staff Code " + staffCode + " is Not Available" : "") + ((isBatchYearAvail && !isBatchYearValid) ? "Batch Year is Invalid or Batch Year Does Not Have Students" : "");//(isFacultyChoiseAvail && isFacultyChoiseValid && !isFacultyChoiceAlreadyAvail) ? "Faculty Choice is Already Available To Subject" :((!isSubjectCodeValid) ? "\nSubject Code " + subjectCode + " is Not Available or Alloted " + ((isStaffCodeValid) ? " Subject " : "") : "")

                                    string error1 = ((mand.Trim(',') != "") ? "Madatory Columns " + mand.Trim(',') + (((mand.Split(',')).Length > 1) ? " Are " : " is ") + "Empty" : "") + invalid;
                                    if (error1.Trim() != "")
                                    {
                                        if (notInserted == "")
                                        {
                                            notInserted = "Excel Row" + (i + 1) + " : " + error1;
                                        }
                                        else
                                        {
                                            notInserted += ",\nExcel Row" + (i + 1) + " : " + error1;
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
            if (isSavedSucc == true)
            {
                lbl_popuperr.Visible = true;
                string ins = string.Empty;
                if (notInsert)
                {
                    ins = "Not Inserted Details are :\n" + notInserted + "\n";
                    txtNotInserted.Text = ins;
                    divNotInserted.Visible = true;
                }
                else
                {
                    txtNotInserted.Text = string.Empty;
                    divNotInserted.Visible = false;
                }
                lbl_popuperr.Text = "Updated Successfully!!!";
                popuperr.Visible = true;
            }
            else
            {
                lbl_popuperr.Visible = true;
                string ins = string.Empty;
                if (notInsert)
                {
                    ins = "Not Inserted Details are:\n" + notInserted + "\n";
                    txtNotInserted.Text = ins;
                    divNotInserted.Visible = true;
                }
                else
                {
                    txtNotInserted.Text = string.Empty;
                    divNotInserted.Visible = false;
                }
                lbl_popuperr.Text = "Not Updated!";
                popuperr.Visible = true;
            }


        }
        catch
        {
        }
    }

    private void GetBatchYearDetails(out Dictionary<string, string> dicBatchYearDetails)
    {
        dicBatchYearDetails = new Dictionary<string, string>();
        dicBatchYearDetails.Clear();
        try
        {
            string qry = "select distinct Batch_Year from Registration where ISNULL(Batch_Year,'0')<>'' and ISNULL(Batch_Year,'0')<>'0' and ISNULL(Batch_Year,'0')<>'-1' order by Batch_Year desc";
            DataTable dtDegreeDetails = dirAccess.selectDataTable(qry);
            if (dtDegreeDetails.Rows.Count > 0)
            {
                foreach (DataRow drDegree in dtDegreeDetails.Rows)
                {
                    string batchYear = Convert.ToString(drDegree["Batch_Year"]).Trim();
                    string key = batchYear;
                    if (!dicBatchYearDetails.ContainsKey(key.ToLower()))
                    {
                        dicBatchYearDetails.Add(key.ToLower(), batchYear);
                    }
                }
            }
        }
        catch
        {
        }
    }

    private void GetDegreeDetails(out Dictionary<string, string> dicDegreeDetails, string batchYear = null)
    {
        dicDegreeDetails = new Dictionary<string, string>();
        dicDegreeDetails.Clear();
        try
        {
            string qry = "select distinct dg.Degree_Code,c.Course_Name,dt.Dept_Name,dt.Dept_Code,dg.college_code,dg.Duration from Course c,Degree dg,Department dt where dg.Course_Id=c.Course_Id and dg.Dept_Code=dt.Dept_Code and dt.college_code=dg.college_code and dg.college_code=c.college_code and c.college_code=dt.college_code order by dg.Degree_Code";
            if (batchYear != null)
            {
                qry = "select distinct dg.Degree_Code,c.Course_Name,dt.Dept_Name,dt.Dept_Code,dg.college_code,dg.Duration from Course c,Degree dg,Department dt,Registration r where r.degree_code=dg.Degree_Code and dg.Course_Id=c.Course_Id and dg.Dept_Code=dt.Dept_Code and dt.college_code=dg.college_code and r.college_code=dg.college_code and dt.college_code=r.college_code and r.college_code=c.college_code and dg.college_code=c.college_code and c.college_code=dt.college_code and r.batch_year='" + batchYear + "' order by dg.Degree_Code";
            }
            DataTable dtDegreeDetails = dirAccess.selectDataTable(qry);
            if (dtDegreeDetails.Rows.Count > 0)
            {
                foreach (DataRow drDegree in dtDegreeDetails.Rows)
                {
                    string degreeCode = Convert.ToString(drDegree["Degree_Code"]).Trim();
                    string courseName = Convert.ToString(drDegree["Course_Name"]).Trim().ToLower();
                    string departmentName = Convert.ToString(drDegree["Dept_Name"]).Trim().ToLower();
                    string duration = Convert.ToString(drDegree["Duration"]).Trim().ToLower();
                    string key = courseName + "@" + departmentName;
                    if (!dicDegreeDetails.ContainsKey(key.ToLower()))
                    {
                        dicDegreeDetails.Add(key.ToLower(), degreeCode);
                    }
                }
            }
        }
        catch
        {
        }
    }

    private void GetSemesterDetails(out Dictionary<string, string> dicSemesterDetails, string batchYear = null)
    {
        dicSemesterDetails = new Dictionary<string, string>();
        dicSemesterDetails.Clear();
        try
        {
            string qry = "select distinct dg.Degree_Code,c.Course_Name,dt.Dept_Name,dt.Dept_Code,dg.college_code,dg.Duration from Course c,Degree dg,Department dt where dg.Course_Id=c.Course_Id and dg.Dept_Code=dt.Dept_Code and dt.college_code=dg.college_code and dg.college_code=c.college_code and c.college_code=dt.college_code order by dg.Degree_Code";
            if (batchYear != null)
            {
                qry = "select distinct dg.Degree_Code,c.Course_Name,dt.Dept_Name,dt.Dept_Code,dg.college_code,dg.Duration from Course c,Degree dg,Department dt,Registration r where r.degree_code=dg.Degree_Code and dg.Course_Id=c.Course_Id and dg.Dept_Code=dt.Dept_Code and dt.college_code=dg.college_code and r.college_code=dg.college_code and dt.college_code=r.college_code and r.college_code=c.college_code and dg.college_code=c.college_code and c.college_code=dt.college_code and r.batch_year='" + batchYear + "' order by dg.Degree_Code";
            }
            DataTable dtDegreeDetails = dirAccess.selectDataTable(qry);
            if (dtDegreeDetails.Rows.Count > 0)
            {
                foreach (DataRow drDegree in dtDegreeDetails.Rows)
                {
                    string degreeCode = Convert.ToString(drDegree["Degree_Code"]).Trim();
                    string courseName = Convert.ToString(drDegree["Course_Name"]).Trim().ToLower();
                    string departmentName = Convert.ToString(drDegree["Dept_Name"]).Trim().ToLower();
                    string duration = Convert.ToString(drDegree["Duration"]).Trim().ToLower();
                    string key = degreeCode;
                    if (!dicSemesterDetails.ContainsKey(key.ToLower()))
                    {
                        dicSemesterDetails.Add(key.ToLower(), duration);
                    }
                }
            }
        }
        catch
        {
        }
    }

    private void GetHoursDetails(out Dictionary<string, byte> dicTotalHoursDetails)
    {
        dicTotalHoursDetails = new Dictionary<string, byte>();
        dicTotalHoursDetails.Clear();
        try
        {
            string qry = "select distinct degree_code,semester,No_of_hrs_per_day from PeriodAttndSchedule order by degree_code,semester";

            #region Don't Delete This Query???

            //qry = "select distinct degree_code,semester,Max(No_of_hrs_per_day) as No_of_hrs_per_day from PeriodAttndSchedule group by degree_code,semester order by degree_code,semester";  

            #endregion Don't Delete This Query???

            DataTable dtHoursDetails = dirAccess.selectDataTable(qry);
            if (dtHoursDetails.Rows.Count > 0)
            {
                foreach (DataRow drTotHour in dtHoursDetails.Rows)
                {
                    string degreeCode = Convert.ToString(drTotHour["degree_code"]).Trim();
                    string semester = Convert.ToString(drTotHour["semester"]).Trim();
                    string noOfHours = Convert.ToString(drTotHour["No_of_hrs_per_day"]).Trim();
                    byte totHours = 0;
                    byte.TryParse(noOfHours.Trim(), out totHours);
                    string key = degreeCode + "@" + semester;
                    if (!dicTotalHoursDetails.ContainsKey(key))
                    {
                        dicTotalHoursDetails.Add(key, totHours);
                    }
                }
            }
        }
        catch
        {
        }
    }

    private void GetAllSubjects(out Dictionary<string, string> dicAllSubjects)
    {
        dicAllSubjects = new Dictionary<string, string>();
        dicAllSubjects.Clear();
        try
        {
            string strquery = "select distinct sm.Batch_Year,sm.degree_code,sm.semester,s.subject_code,s.subject_no from subject s,syllabus_master sm where sm.syll_code=s.syll_code order by sm.Batch_Year,sm.degree_code,sm.semester,s.subject_code";
            DataTable dtSubjects = dirAccess.selectDataTable(strquery);
            if (dtSubjects.Rows.Count > 0)
            {
                foreach (DataRow drSubjects in dtSubjects.Rows)
                {
                    string Batch_Year = Convert.ToString(drSubjects["Batch_Year"]).Trim().ToLower();
                    string degreeCodeNew = Convert.ToString(drSubjects["degree_code"]).Trim().ToLower();
                    string sem = Convert.ToString(drSubjects["semester"]).Trim().ToLower();
                    string subjectCode = Convert.ToString(drSubjects["subject_code"]).Trim().ToLower();
                    string subjectNo = Convert.ToString(drSubjects["subject_no"]).Trim().ToLower();
                    string key = Batch_Year.Trim().ToLower() + "@" + degreeCodeNew + "@" + sem + "@" + subjectCode;
                    if (!dicAllSubjects.ContainsKey(key))
                    {
                        dicAllSubjects.Add(key, subjectNo);
                    }
                }
            }
        }
        catch (Exception ex)
        {
        }
    }

    public void GetAllStaffDetails(out Dictionary<string, string> dicAllStaffDetails)
    {
        dicAllStaffDetails = new Dictionary<string, string>();
        dicAllStaffDetails.Clear();
        try
        {
            string strquery = "select sf.staff_code,sf.staff_name,sam.appl_id from staffmaster sf,staff_appl_master sam where sam.appl_no=sf.appl_no and ISNULL(sf.resign,'0')='0' and ISNULL(sf.settled,'0')='0' order by sam.appl_id";
            DataTable dtStaffDetails = dirAccess.selectDataTable(strquery);
            if (dtStaffDetails.Rows.Count > 0)
            {
                foreach (DataRow dtStaff in dtStaffDetails.Rows)
                {
                    string staffCode = Convert.ToString(dtStaff["staff_code"]).Trim().ToLower();
                    string staffName = Convert.ToString(dtStaff["staff_name"]).Trim().ToLower();
                    string staffApplID = Convert.ToString(dtStaff["appl_id"]).Trim().ToLower();
                    string key = staffCode;
                    if (!dicAllStaffDetails.ContainsKey(staffCode))
                    {
                        dicAllStaffDetails.Add(staffCode, staffName + "@" + staffApplID);
                    }
                }
            }
        }
        catch (Exception ex)
        {
        }
    }

    private void GetStaffSelector(out Dictionary<string, string> dicStaffSelectors)
    {
        dicStaffSelectors = new Dictionary<string, string>();
        dicStaffSelectors.Clear();
        try
        {
            string strquery = "select sm.Batch_Year,sm.degree_code,sm.semester,s.subject_code,s.subject_no,s.subject_name,sf.staff_code,ISNULL(ss.facultyChoice,'1') facultyChoice from staff_selector ss,staffmaster sf,subject s,syllabus_master sm where sf.staff_code=ss.staff_code and s.subject_no=ss.subject_no and sm.syll_code=s.syll_code and ISNULL(sf.resign,'0')='0' and ISNULL(sf.settled,'0')='0' order by sm.Batch_Year,sm.degree_code,sm.semester,s.subject_code,facultyChoice,sf.staff_code";
            DataTable dtStaffSelector = dirAccess.selectDataTable(strquery);
            if (dtStaffSelector.Rows.Count > 0)
            {
                foreach (DataRow drStaffSelector in dtStaffSelector.Rows)
                {
                    string Batch_Year = Convert.ToString(drStaffSelector["Batch_Year"]).Trim().ToLower();
                    string degreeCodeNew = Convert.ToString(drStaffSelector["degree_code"]).Trim().ToLower();
                    string sem = Convert.ToString(drStaffSelector["semester"]).Trim().ToLower();
                    string subjectCode = Convert.ToString(drStaffSelector["subject_code"]).Trim().ToLower();
                    string subjectNo = Convert.ToString(drStaffSelector["subject_no"]).Trim().ToLower();
                    string subjectName = Convert.ToString(drStaffSelector["subject_name"]).Trim().ToLower();
                    string staffCode = Convert.ToString(drStaffSelector["staff_code"]).Trim().ToLower();
                    string facutyChoice = Convert.ToString(drStaffSelector["facultyChoice"]).Trim().ToLower();
                    string key = Batch_Year.Trim().ToLower() + "@" + degreeCodeNew + "@" + sem + "@" + subjectCode + "@" + staffCode;
                    if (!dicStaffSelectors.ContainsKey(key))
                    {
                        dicStaffSelectors.Add(key, facutyChoice);
                    }
                }
            }
        }
        catch (Exception ex)
        {
        }
    }

    private bool isValidBatchYearDetails(ref string batchYear, Dictionary<string, string> dicDegreeDetails)
    {
        bool isValid = false;
        batchYear = batchYear.Trim().ToLower();
        string key = batchYear;
        //degreeCode = string.Empty;
        if (key != "")
        {
            string keyValue = key.ToLower();
            if (dicBatchYearDetails.Count > 0)
            {
                if (dicBatchYearDetails.ContainsKey(keyValue))
                {
                    isValid = true;
                    batchYear = dicBatchYearDetails[keyValue];
                }
            }
        }
        return isValid;
    }

    private bool isValidDegreeDetails(string courseName, string departmentName, Dictionary<string, string> dicDegreeDetails, ref string degreeCode)
    {
        bool isValid = false;
        courseName = courseName.Trim().ToLower();
        string key = courseName + "@" + departmentName;
        degreeCode = string.Empty;
        if (key != "")
        {
            string keyValue = key.ToLower();
            if (dicDegreeDetails.Count > 0)
            {
                if (dicDegreeDetails.ContainsKey(keyValue))
                {
                    isValid = true;
                    degreeCode = dicDegreeDetails[keyValue];
                }
            }
        }
        return isValid;
    }

    private bool isValidSemesterDetails(string degreeCode, Dictionary<string, string> dicSemesterDetails, ref string semester, ref string maxDuara)
    {
        bool isValid = false;
        degreeCode = degreeCode.Trim().ToLower();
        semester = semester.Trim();
        string key = degreeCode;
        int sem = 0;
        int.TryParse(semester.Trim(), out sem);
        string duartion = string.Empty;
        maxDuara = string.Empty;
        int maxDuration = 0;
        if (key != "")
        {
            string keyValue = key.ToLower();
            if (dicSemesterDetails.Count > 0)
            {
                if (dicSemesterDetails.ContainsKey(keyValue))
                {
                    maxDuara = duartion = dicSemesterDetails[keyValue];
                }
            }
            int.TryParse(duartion.Trim(), out maxDuration);
            if (maxDuration > 0 && sem > 0 && sem <= maxDuration)
            {
                isValid = true;
            }
            else
            {
                isValid = false;
            }
        }
        return isValid;
    }

    private bool isValidHours(string degreeCode, string semester, Dictionary<string, byte> dicTotalHoursDetails, ref string hour, ref byte totHours)
    {
        bool isValid = false;
        degreeCode = degreeCode.Trim().ToLower();
        string key = degreeCode + "@" + semester;
        byte hourNew = 0;
        hour = hour.Trim();
        byte.TryParse(hour.Trim(), out hourNew);
        totHours = 0;
        if (key != "")
        {
            string keyValue = key.ToLower();
            if (dicTotalHoursDetails.Count > 0)
            {
                if (dicTotalHoursDetails.ContainsKey(keyValue))
                {
                    totHours = dicTotalHoursDetails[keyValue];
                }
            }
        }
        if (totHours > 0)
        {
            if (hourNew <= totHours)
            {
                isValid = true;
            }
        }
        return isValid;
    }

    private bool isValidSubjectCode(string subjectCode, string batchYear, string degreeCode, string semester, Dictionary<string, string> dicAllSubjects, ref string subjectNo)
    {
        bool isValid = false;
        subjectCode = subjectCode.Trim().ToLower();
        batchYear = batchYear.Trim();
        degreeCode = degreeCode.Trim();
        semester = semester.Trim();
        subjectNo = string.Empty;
        string key = batchYear.Trim().ToLower() + "@" + degreeCode + "@" + semester + "@" + subjectCode;
        if (key != "")
        {
            string keyValue = key.ToLower();
            if (dicAllSubjects.Count > 0)
            {
                if (dicAllSubjects.ContainsKey(keyValue))
                {
                    isValid = true;
                    subjectNo = dicAllSubjects[keyValue];
                }
            }
        }
        return isValid;
    }

    public bool isValidStaffCode(string staffCode, Dictionary<string, string> dicAllStaffDetails, ref string staffName)
    {
        bool isValid = false;
        staffCode = staffCode.Trim().ToLower();
        string key = staffCode;
        staffName = string.Empty;
        if (staffCode != "")
        {
            string keyValue = staffCode;
            if (dicAllStaffDetails.Count > 0)
            {
                if (dicAllStaffDetails.ContainsKey(keyValue))
                {
                    isValid = true;
                    staffName = dicAllStaffDetails[keyValue];
                }
            }
        }
        return isValid;
    }

    private bool isValidStaffSelector(string subjectCode, string batchYear, string degreeCode, string semester, string staffCode, Dictionary<string, string> dicStaffSelectors, ref string facultyChoice)
    {
        bool isValid = false;
        subjectCode = subjectCode.Trim().ToLower();
        batchYear = batchYear.Trim();
        degreeCode = degreeCode.Trim();
        staffCode = staffCode.Trim().ToLower();
        semester = semester.Trim();
        facultyChoice = string.Empty;
        string key = batchYear.Trim().ToLower() + "@" + degreeCode + "@" + semester + "@" + subjectCode + "@" + staffCode;
        if (key != "")
        {
            string keyValue = key.ToLower();
            if (dicStaffSelectors.Count > 0)
            {
                if (dicStaffSelectors.ContainsKey(keyValue))
                {
                    isValid = true;
                    facultyChoice = dicStaffSelectors[keyValue];
                }
            }
        }
        return isValid;
    }

    #endregion TimeTable Criteria Import
    //Raja 10-03-2017
    private DataTable getEngagedHrs()
    {
        DataTable dt = dirAccess.selectDataTable("select DayPk,HourPk from TT_StudentCriteria where criterianame like '" + ddlCriteria.SelectedItem.Text.Split('-')[0].Split(' ')[0] + "%' ");
        return dt;
    }
    private void saveAndGenerate(int criteria)
    {
        try
        {
            if (ddlStaffTT.Items.Count > 0)
                ddlStaffTT.SelectedIndex = 0;

            DataSet dsNewTimeTables = new DataSet();

            if (Session["selectedDataSet"] != null)
            {
                dsNewTimeTables = (DataSet)Session["selectedDataSet"];
            }
            string newName = (dsNewTimeTables.Tables.Count + 1).ToString();
            if (Session["prevDataSet"] != null)
            {
                DataSet dsTimeTables = (DataSet)Session["prevDataSet"];
                string prevTableName = ddlSelectedTimeTable.Items.Count > 0 ? ddlSelectedTimeTable.SelectedItem.Text.Trim() : "---";
                if (dsTimeTables.Tables.Contains(prevTableName) || prevTableName == "---")
                {
                    if (prevTableName != "---")
                    {
                        DataTable dtTimeTableSelected = dsTimeTables.Tables[prevTableName].Copy();
                        dtTimeTableSelected.TableName = dtTimeTableSelected.TableName + "-" + newName;
                        dsNewTimeTables.Tables.Add(dtTimeTableSelected);
                        Session["selectedDataSet"] = dsNewTimeTables;
                    }

                    //Modified Generate Button Click Event for Next Options
                    #region Modified Generate Button Click Event
                    DataSet dsTimeTable = new DataSet();
                    #region Generate Time Table for every selected branches
                    ArrayList arrLstDegBatch = getDegreeArrayList();
                    foreach (string batchDeg in arrLstDegBatch)
                    {
                        string[] arrBatchDeg = batchDeg.Split('$');
                        if (arrBatchDeg.Length == 3)
                        {
                            int batchYear = Convert.ToInt32(arrBatchDeg[0]);
                            int degreeCode = Convert.ToInt32(arrBatchDeg[1]);
                            string dispText = Convert.ToString(arrBatchDeg[2]);

                            int currentSem = dirAccess.selectScalarInt("select distinct r.Current_Semester from Registration r where  r.degree_code ='" + degreeCode + "' and r.Batch_Year='" + batchYear + "' and r.college_code='" + collegecode + "'  and CC='0' and DelFlag='0' and Exam_Flag<>'DEBAR'");

                            DataTable dtTimeTable = new DataTable();
                            //Room Avalability Detail
                            ArrayList arrlstRoomDet = new ArrayList();
                            ArrayList arrlstLabDet = new ArrayList();
                            Dictionary<string, int> dicRoomAvailability = getRoomAvailability(batchYear, degreeCode, currentSem, ref arrlstRoomDet, ref arrlstLabDet, dsNewTimeTables.Tables.Count);//, dsNewTimeTables.Tables.Count

                            int noOfHrsPerDay = 0;
                            DataTable dtBellSchedule = new DataTable();
                            DataTable dtSubjectDet = new DataTable();
                            DataTable dtSubjectDetWt = new DataTable();
                            DataTable dtStaffDet = new DataTable();

                            DataTable dtCriteria = new DataTable();

                            if (criteria == 0)
                            {
                                dtCriteria = dirAccess.selectDataTable("select distinct subject_code+'-'+c.staff_code as criterianame,DayPk,HourPk,IsEngaged from TT_StudentCriteria c,subject s where c.subject_no=s.subject_no and c.semester='" + currentSem + "' and c.degree_code='" + degreeCode + "' and c.batch_year='" + batchYear + "' and c.criterianame='" + ddlCriteria.SelectedItem.Text.Split('-')[0] + "'  and c.staff_code='" + ddlCriteria.SelectedItem.Text.Split('-')[2] + "'  and c.subject_no = '" + ddlCriteria.SelectedItem.Value.Split('-')[1] + "' ");
                            }
                            else
                            {
                                if (ddlCriteriaReduced.Items.Count > 0)
                                {
                                    dtCriteria = dirAccess.selectDataTable("select distinct subject_code+'-'+c.staff_code as criterianame,DayPk,HourPk,IsEngaged from TT_StudentCriteria c,subject s where c.subject_no=s.subject_no and c.semester='" + currentSem + "' and c.degree_code='" + degreeCode + "' and c.batch_year='" + batchYear + "'  and c.criterianame='" + ddlCriteriaReduced.SelectedItem.Text.Split('-')[0] + "'  and c.staff_code='" + ddlCriteriaReduced.SelectedItem.Text.Split('-')[2] + "' and c.subject_no='" + ddlCriteriaReduced.SelectedItem.Value.Split('-')[1] + "'");
                                    ddlCriteriaReduced.Items.RemoveAt(ddlCriteriaReduced.SelectedIndex);

                                    if (ddlCriteriaReduced.Items.Count == 0)
                                    {
                                        for (int i = 0; i < ddlCriteria.Items.Count; i++)
                                        {
                                            //if (ddlCriteria.SelectedIndex != i)
                                            //{
                                            ListItem item = new ListItem(ddlCriteria.Items[i].Text, ddlCriteria.Items[i].Value);
                                            ddlCriteriaReduced.Items.Add(item);
                                            //}
                                        }
                                    }
                                }
                                else
                                {
                                    dtCriteria = dirAccess.selectDataTable("select distinct criterianame+'-'+subject_code+'-'+c.staff_code as criterianame from TT_StudentCriteria c,subject s where c.subject_no=s.subject_no and c.semester='" + currentSem + "' and c.degree_code='" + degreeCode + "' and c.batch_year='" + batchYear + "' ");

                                    //dtCriteria = dirAccess.selectDataTable("select criterianame ,DayPk,HourPk,IsEngaged from TT_StudentCriteria  ");
                                }
                            }

                            int maxNoCanAllot = 0;
                            DataTable dtFacultyChoices = getFacultyChoicesN(batchYear, degreeCode, currentSem, ref  dtSubjectDet, ref  dtSubjectDetWt, ref maxNoCanAllot, ref noOfHrsPerDay, ref  dtBellSchedule, ref dtStaffDet);

                            DataTable dtFacultyChoicesTemp = dtFacultyChoices.Copy();
                            if (noOfHrsPerDay > 0 && dtBellSchedule.Rows.Count > 0 && dicRoomAvailability.Count > 0)
                            {
                                #region Engage StaffAvailability, Lab & Elective Allotment Conditions and Room
                                ArrayList arrAlreadyAddedRowCol = new ArrayList();
                                //ArrayList arrAlreadyAddedCol = new ArrayList();//To Check whether already added in the row & column

                                Hashtable htElectPreAllot = new Hashtable();
                                Hashtable htElectRooms = new Hashtable();

                                DataTable dtCurStaffDet = dtStaffDet.Copy();
                                dtCurStaffDet.Clear();

                                for (int ttI = 0; ttI < dsNewTimeTables.Tables.Count; ttI++)
                                {
                                    DataTable dtcurrentTable = dsNewTimeTables.Tables[ttI];

                                    for (int dayI = 0; dayI < (dtcurrentTable.Rows.Count - 1); dayI++)
                                    {
                                        for (int hrsI = 1; hrsI < dtcurrentTable.Columns.Count; hrsI++)
                                        {
                                            string colName = dtcurrentTable.Columns[hrsI].ColumnName.ToString().Trim();
                                            byte realHrs = 0;
                                            if (byte.TryParse(colName, out realHrs))
                                            {
                                                string cellValue = Convert.ToString(dtcurrentTable.Rows[dayI][realHrs.ToString()]);
                                                string[] cellValues = cellValue.Split('$');//with room

                                                if (cellValues[0] != string.Empty && !cellValues[0].Contains(","))
                                                {
                                                    string[] resultValues = cellValues[0].Split('-');//subject code-staffcode
                                                    if (resultValues.Length > 1)
                                                    {
                                                        string[] subcodes = resultValues[0].Split('#');
                                                        //Theory and Lab
                                                        if (subcodes.Length == 1)
                                                        {
                                                            string[] staffs = resultValues[1].Split('/');
                                                            foreach (string faculty in staffs)
                                                            {
                                                                dtSubjectDetWt.DefaultView.RowFilter = "subject_code='" + resultValues[0] + "' and staff_code='" + faculty + "'";
                                                                DataView dvCurrStaff = dtSubjectDetWt.DefaultView;
                                                                if (dvCurrStaff.Count > 0)
                                                                {
                                                                    string subject_no = Convert.ToString(dvCurrStaff[0]["subject_no"]);
                                                                    DataRow drNewRow = dtCurStaffDet.NewRow();

                                                                    drNewRow["staff_appno"] = dvCurrStaff[0]["appl_id"];
                                                                    drNewRow["DaysFK"] = (dayI + 1);
                                                                    drNewRow["HoursFK"] = realHrs;
                                                                    drNewRow["degreeCode"] = degreeCode;
                                                                    drNewRow["batch_year"] = batchYear;
                                                                    drNewRow["IsEngaged"] = "True";
                                                                    drNewRow["subject_no"] = subject_no;
                                                                    drNewRow["section"] = "";
                                                                    drNewRow["MaxHour"] = "1";
                                                                    dtCurStaffDet.Rows.Add(drNewRow);
                                                                    if (cellValues.Length > 1)
                                                                    {
                                                                        string hrI = " #" + cellValues[1] + "$" + (dayI + 1) + "_" + realHrs;
                                                                        dicRoomAvailability[hrI] = 1;
                                                                    }

                                                                    //Lab Allocated from previous Time table for non repetition
                                                                    if (staffs.Length == 2)
                                                                    {
                                                                        if (!arrAlreadyAddedRowCol.Contains(subject_no + "$" + cellValues[1] + "_" + dayI + "_" + realHrs))
                                                                        {
                                                                            arrAlreadyAddedRowCol.Add(subject_no + "$" + cellValues[1] + "_" + dayI + "_" + realHrs);
                                                                        }
                                                                        string staffcheck = String.Format(staffs[0] + "'" + "," + "'" + staffs[1]); //Remove already added Staff 
                                                                        //dtFacultyChoices.DefaultView.RowFilter = "[" + subject_no + "] <>'" + staffcheck.Replace("'", "''") + "'";
                                                                        //DataView dvNew = dtFacultyChoices.DefaultView;
                                                                        ////dtFacultyChoices.Clear();
                                                                        //dtFacultyChoices = dvNew.ToTable();

                                                                        //dtCurStaffDet.Rows[dtCurStaffDet.Rows.Count - 1]["MaxHour"] = "2";
                                                                    }
                                                                    else//Remove already added Staff 
                                                                    {
                                                                        //dtFacultyChoices.DefaultView.RowFilter = "[" + subject_no + "] <>'" + faculty + "'";
                                                                        //DataView dvNew = dtFacultyChoices.DefaultView;
                                                                        ////dtFacultyChoices.Clear();
                                                                        //dtFacultyChoices = dvNew.ToTable();
                                                                    }
                                                                    //Lab Allocation Ends
                                                                }
                                                                else
                                                                {
                                                                    if (cellValues.Length > 1)
                                                                    {
                                                                        string hrI = " #" + cellValues[1] + "$" + (dayI + 1) + "_" + realHrs;
                                                                        dicRoomAvailability[hrI] = 1;
                                                                    }
                                                                }
                                                            }
                                                        }
                                                        else
                                                        {   // Combined Lab
                                                            string[] staffs = resultValues[1].Split('/');

                                                            if (staffs.Length > 1)
                                                            {

                                                                dtSubjectDetWt.DefaultView.RowFilter = "subject_code in ('" + subcodes[0] + "') and staff_code in ('" + staffs[0] + "')";
                                                                DataView dvCurrStaff = dtSubjectDetWt.DefaultView;

                                                                string subject_no = Convert.ToString(dvCurrStaff[0]
["subject_no"]);

                                                                dtSubjectDetWt.DefaultView.RowFilter = "subject_code in ('" + subcodes[1] + "') and staff_code in ('" + staffs[1] + "')";
                                                                DataView dvCurrStaff1 = dtSubjectDetWt.DefaultView;

                                                                string subject_no2 = Convert.ToString(dvCurrStaff1[0]
["subject_no"]);
                                                                DataRow drNewRow = dtCurStaffDet.NewRow();

                                                                drNewRow["staff_appno"] = dvCurrStaff[0]["appl_id"];
                                                                drNewRow["DaysFK"] = (dayI + 1);
                                                                drNewRow["HoursFK"] = realHrs;
                                                                drNewRow["degreeCode"] = degreeCode;
                                                                drNewRow["batch_year"] = batchYear;
                                                                drNewRow["IsEngaged"] = "True";
                                                                drNewRow["subject_no"] = subject_no;
                                                                drNewRow["section"] = "";
                                                                drNewRow["MaxHour"] = "2";
                                                                dtCurStaffDet.Rows.Add(drNewRow);


                                                                DataRow drNewRow1 = dtCurStaffDet.NewRow();

                                                                drNewRow1["staff_appno"] = dvCurrStaff1[0]["appl_id"];
                                                                drNewRow1["DaysFK"] = (dayI + 1);
                                                                drNewRow1["HoursFK"] = realHrs;
                                                                drNewRow1["degreeCode"] = degreeCode;
                                                                drNewRow1["batch_year"] = batchYear;
                                                                drNewRow1["IsEngaged"] = "True";
                                                                drNewRow1["subject_no"] = subject_no2;
                                                                drNewRow1["section"] = "";
                                                                drNewRow["MaxHour"] = "2";
                                                                dtCurStaffDet.Rows.Add(drNewRow1);

                                                                if (cellValues.Length > 1)
                                                                {
                                                                    string hrI = " #" + cellValues[1] + "$" + (dayI + 1) + "_" + realHrs;
                                                                    dicRoomAvailability[hrI] = 1;
                                                                }

                                                                //Lab Allocated from previous Time table for non repetition
                                                                if (staffs.Length == 2)
                                                                {
                                                                    if (!arrAlreadyAddedRowCol.Contains(subject_no + "$" + cellValues[1] + "_" + dayI + "_" + realHrs))
                                                                    {
                                                                        arrAlreadyAddedRowCol.Add(subject_no + "$" + cellValues[1] + "_" + dayI + "_" + realHrs);
                                                                    }
                                                                    if (!arrAlreadyAddedRowCol.Contains(subject_no2 + "$" + cellValues[1] + "_" + dayI + "_" + realHrs))
                                                                    {
                                                                        arrAlreadyAddedRowCol.Add(subject_no2 + "$" + cellValues[1] + "_" + dayI + "_" + realHrs);
                                                                    }
                                                                    string staffcheck = String.Format(staffs[0] + "'" + "," + "'" + staffs[1]); //Remove already added Staff 
                                                                    //dtFacultyChoices.DefaultView.RowFilter = "[" + subject_no + "-" + subject_no2 + "] <>'" + staffcheck.Replace("'", "''") + "'";
                                                                    //DataView dvNew = dtFacultyChoices.DefaultView;
                                                                    ////dtFacultyChoices.Clear();
                                                                    //dtFacultyChoices = dvNew.ToTable();
                                                                }
                                                                else//Remove already added Staff 
                                                                {
                                                                    //dtFacultyChoices.DefaultView.RowFilter = "[" + subject_no + "-" + subject_no2 + "] <>'" + staffs[0] + "' or [" + subject_no + "-" + subject_no2 + "] <>'" + staffs[1] + "'";
                                                                    //DataView dvNew = dtFacultyChoices.DefaultView;
                                                                    ////dtFacultyChoices.Clear();
                                                                    //dtFacultyChoices = dvNew.ToTable();
                                                                }
                                                                //Lab Allocation Ends
                                                            }
                                                            else
                                                            {
                                                                if (cellValues.Length > 1)
                                                                {
                                                                    string hrI = " #" + cellValues[1] + "$" + (dayI + 1) + "_" + realHrs;
                                                                    dicRoomAvailability[hrI] = 1;
                                                                }
                                                            }
                                                        }
                                                    }
                                                }
                                                else if (cellValues[0] != string.Empty && cellValues[0].Contains(","))
                                                {
                                                    //For Elective
                                                    string[] electPapers = cellValues[0].Split(',');
                                                    foreach (string electPap in electPapers)
                                                    {
                                                        string[] resultValues = electPap.Split('-');//subject code-staffcode
                                                        if (resultValues.Length > 1)
                                                        {
                                                            string subType_noFnl = string.Empty;
                                                            string[] staffs = resultValues[1].Split('/');
                                                            foreach (string faculty in staffs)
                                                            {
                                                                dtSubjectDetWt.DefaultView.RowFilter = "subject_code='" + resultValues[0] + "' and staff_code='" + faculty + "'";
                                                                DataView dvCurrStaff = dtSubjectDetWt.DefaultView;
                                                                if (dvCurrStaff.Count > 0)
                                                                {
                                                                    string subject_no = Convert.ToString(dvCurrStaff[0]["subject_no"]);
                                                                    string subType_no = Convert.ToString(dvCurrStaff[0]["subType_no"]);
                                                                    DataRow drNewRow = dtCurStaffDet.NewRow();

                                                                    drNewRow["staff_appno"] = dvCurrStaff[0]["appl_id"];
                                                                    drNewRow["DaysFK"] = (dayI + 1);
                                                                    drNewRow["HoursFK"] = realHrs;
                                                                    drNewRow["degreeCode"] = degreeCode;
                                                                    drNewRow["batch_year"] = batchYear;
                                                                    drNewRow["IsEngaged"] = "True";
                                                                    drNewRow["subject_no"] = subject_no;
                                                                    drNewRow["section"] = "";
                                                                    drNewRow["MaxHour"] = "1";
                                                                    dtCurStaffDet.Rows.Add(drNewRow);

                                                                    subType_noFnl = subType_no;
                                                                }
                                                            }

                                                            string[] rooms = cellValues[1].Split(',');
                                                            foreach (string room in rooms)
                                                            {
                                                                string hrI = " #" + room + "$" + (dayI + 1) + "_" + realHrs;
                                                                dicRoomAvailability[hrI] = 1;
                                                            }
                                                            if (!htElectRooms.Contains(subType_noFnl))
                                                                htElectRooms.Add(subType_noFnl, cellValues[1]);

                                                            string subjecthrs = subType_noFnl + "_" + dayI + "_" + realHrs;

                                                            if (!htElectPreAllot.Contains(subjecthrs))
                                                            {
                                                                htElectPreAllot.Add(subjecthrs, "1");
                                                            }
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                                dtStaffDet.Merge(dtCurStaffDet);

                                if (dtFacultyChoices.Rows.Count == 0)
                                    dtFacultyChoices = dtFacultyChoicesTemp;
                                #endregion

                                Hashtable hashElectivesubject = new Hashtable();//For Avoiding same day Elective
                                Hashtable hashElectivesubjectHr = new Hashtable();//For Avoiding same hour Elective
                                Hashtable hashLabsubject = new Hashtable();//For Avoiding same day Lab 
                                Hashtable hashLabsubjectHr = new Hashtable();//For Avoiding same hour Lab
                                Hashtable hashsubject = new Hashtable();//For Avoiding same day Theory
                                Hashtable hashsubjectHr = new Hashtable();//For Avoiding same hour Theory

                                //Call Sequence I
                                for (int facChoiceI = 0; facChoiceI < dtFacultyChoices.Rows.Count; facChoiceI++)
                                {
                                    if (criteria == 0)
                                    {
                                        dtTimeTable = getTimeTableFormat(batchYear, degreeCode, currentSem, (dispText + "-" + (facChoiceI + 1)), dtSubjectDet, dtSubjectDetWt, dtFacultyChoices, facChoiceI, maxNoCanAllot, noOfHrsPerDay, dtBellSchedule, dtCriteria, ref dtStaffDet, hashElectivesubject, hashElectivesubjectHr, hashLabsubject, hashLabsubjectHr, hashsubject, hashsubjectHr, dicRoomAvailability, arrlstRoomDet, arrlstLabDet);
                                        if (dtTimeTable.Rows.Count > 0)
                                        {
                                            dsTimeTable.Tables.Add(dtTimeTable);
                                        }
                                    }
                                    else
                                    {
                                        //if (dsNewTimeTables.Tables.Count < 5)
                                        //{
                                        dtTimeTable = getTimeTableFormatRegenerate(batchYear, degreeCode, currentSem, (dispText + "-" + (facChoiceI + 1)), dtSubjectDet, dtSubjectDetWt, dtFacultyChoices, facChoiceI, maxNoCanAllot, noOfHrsPerDay, dtBellSchedule, dtCriteria, ref dtStaffDet, dicRoomAvailability, arrlstRoomDet, arrlstLabDet, arrAlreadyAddedRowCol, htElectPreAllot, htElectRooms);
                                        if (dtTimeTable.Rows.Count > 0)
                                        {
                                            dsTimeTable.Tables.Add(dtTimeTable);
                                        }
                                        //}
                                        //else
                                        //{
                                        //    dtTimeTable = getTimeTableFormatRegenerateUnfill(batchYear, degreeCode, currentSem, (dispText + "-" + (facChoiceI + 1)), dtSubjectDet, dtSubjectDetWt, dtFacultyChoices, facChoiceI, maxNoCanAllot, noOfHrsPerDay, dtBellSchedule, dtCriteria, ref dtStaffDet, dicRoomAvailability, arrlstRoomDet, arrlstLabDet, arrAlreadyAddedRowCol, htElectPreAllot, htElectRooms);
                                        //    if (dtTimeTable.Rows.Count > 0)
                                        //    {
                                        //        dsTimeTable.Tables.Add(dtTimeTable);
                                        //    }
                                        //}
                                    }
                                }

                                ddlSelectedTimeTable.Items.Clear();
                                for (int tblI = 0; tblI < dsTimeTable.Tables.Count; tblI++)
                                {
                                    string tblName = dsTimeTable.Tables[tblI].TableName.Replace("Table", dispText + "-" + ddlCriteriaReduced.SelectedItem.Text.Split('-')[0] + "-");
                                    dsTimeTable.Tables[tblI].TableName = tblName;
                                    ddlSelectedTimeTable.Items.Add(tblName);
                                }
                            }
                        }
                    }
                    if (dsTimeTable.Tables.Count > 0)
                    {
                        Session["prevDataSet"] = dsTimeTable;
                        tblHeaderNextTT.Visible = true;
                    }
                    else
                    {
                        tblHeaderNextTT.Visible = true;
                    }
                    #endregion
                    #region Display generated Tables
                    //Adding Colors
                    ArrayList arrSubName = new ArrayList();

                    List<string> lstCellValues = new List<string>();
                    lstCellValues.Add("monday");
                    lstCellValues.Add("tuesday");
                    lstCellValues.Add("wednesday");
                    lstCellValues.Add("thursday");
                    lstCellValues.Add("friday");
                    lstCellValues.Add("");

                    //Building an HTML string.
                    StringBuilder html = new StringBuilder();
                    for (int ttI = 0; ttI < dsTimeTable.Tables.Count; ttI++)
                    {
                        html.Append("<center><span style='color: Green; font-size:medium;'>" + dsTimeTable.Tables[ttI].TableName + "</span></center><br/>");
                        //Table start.
                        html.Append("<table cellpadding='0' cellspacing='0' style=' border:1px solid black; border-radius:5px; text-align:center; width:920px; font-size:10px;'>");
                        int cnt = 1;
                        //Building the Last row.
                        html.Append("<tr  style='background-color:#3B6D93;color:#FFFFFF; font-size:12px;'>");
                        foreach (DataColumn column in dsTimeTable.Tables[ttI].Columns)
                        {
                            html.Append("<td>");
                            html.Append(dsTimeTable.Tables[ttI].Rows[dsTimeTable.Tables[ttI].Rows.Count - 1][column.ColumnName]);
                            html.Append("</td>");
                        }
                        html.Append("</tr>");
                        //Building the Header row.
                        html.Append("<tr style='background-color:#3B6D93;color:#FFFFFF; font-size:12px;'>");
                        foreach (DataColumn column in dsTimeTable.Tables[ttI].Columns)
                        {
                            html.Append("<td>");
                            html.Append(column.ColumnName);
                            html.Append("</td>");
                        }
                        html.Append("</tr>");

                        //Building the Data rows.
                        foreach (DataRow row in dsTimeTable.Tables[ttI].Rows)
                        {
                            if (cnt == dsTimeTable.Tables[ttI].Rows.Count)
                            {
                                continue;
                            }
                            cnt++;
                            html.Append("<tr>");
                            foreach (DataColumn column in dsTimeTable.Tables[ttI].Columns)
                            {
                                string slotValue = row[column.ColumnName].ToString().Trim();
                                if (!lstCellValues.Contains(slotValue.ToLower()))
                                {
                                    if (!arrSubName.Contains(slotValue.Split('-')[0]))
                                        arrSubName.Add(slotValue.Split('-')[0]);
                                    int index = arrSubName.IndexOf(slotValue.Split('-')[0]);
                                    string bgcolor = getColor(index);
                                    html.Append("<td style='background-color:" + bgcolor + "'>");
                                }
                                else
                                {
                                    if (string.IsNullOrEmpty(slotValue))
                                    {
                                        html.Append("<td style='background-color:#FFFFFF;'>");
                                    }
                                    else
                                    {
                                        html.Append("<td style='background-color:#3B6D93;color:#FFFFFF; font-size:12px;'>");
                                    }
                                }
                                html.Append(slotValue);
                                html.Append("</td>");
                            }
                            html.Append("</tr>");
                        }
                        //Table end.
                        html.Append("</table><br>");
                    }
                    //Append the HTML string to Placeholder.
                    divTimeTableOutput.Visible = true;
                    phTimeTable.Controls.Add(new Literal { Text = html.ToString() });

                    #endregion
                    #endregion
                }
            }
        }
        catch { }
        Session["FromSaved"] = 1;
    }
    //Get Faculty Choices for Subjects
    private DataTable getFacultyChoicesN(int batchYear, int degreeCode, int currentSem, ref DataTable dtSubjectDet, ref DataTable dtSubjectDetWt, ref int maxNoCanAllot, ref int noOfHrsPerDay, ref DataTable dtBellSchedule, ref DataTable dtStaffDet)
    {
        DataTable dtFacultyChoices = new DataTable();
        try
        {
            noOfHrsPerDay = dirAccess.selectScalarInt("select No_of_hrs_per_day from PeriodAttndSchedule where degree_code ='" + degreeCode + "' and semester ='" + currentSem + "'");

            dtBellSchedule = dirAccess.selectDataTable("select Period1,Desc1,SUBSTRING(Convert(Varchar,start_time,108),1,5) as start_time,SUBSTRING(Convert(Varchar,end_time,108),1,5) as end_time,no_of_breaks from BellSchedule where  Degree_Code='" + degreeCode + "' and semester ='" + currentSem + "' and batch_year='" + batchYear + "' order by start_time asc  -- ISNUMERIC(Period1) = 1 and ");


            if (noOfHrsPerDay > 0 && dtBellSchedule.Rows.Count > 0)
            {
                dtSubjectDet = dirAccess.selectDataTable("select sm.syll_code,ss.subType_no,ss.subject_type,ss.ElectivePap,ss.Lab,s.subject_no,s.subject_code,s.subject_name,isnull(s.sub_lab,0) as sub_lab,isnull(s.noofhrsperweek,0) as noofhrsperweek,s.maximumHrsPerDay,isnull(s.subjectpriority,0) as subjectpriority,s.practicalPair from syllabus_master sm,sub_sem ss, subject s where sm.syll_code = ss.syll_code and sm.syll_code = s.syll_code and ss.syll_code= s.syll_code and ss.subType_no = s.subType_no  and sm.Batch_Year='" + batchYear + "' and sm.degree_code ='" + degreeCode + "' and sm.semester = '" + currentSem + "'  and (s.subject_no not in (select distinct subject_no from TT_StudentCriteria where semester='" + currentSem + "' and degree_code='" + degreeCode + "' and batch_year='" + batchYear + "')) order by subjectpriority desc,ElectivePap desc,Lab desc");// order by Lab desc,ElectivePap asc  and ss.subtype_no<>'193' 

                //object sumObject;
                //sumObject = dtSubjectDet.Compute("sum(noofhrsperweek)", "ElectivePap='false'");
                //int exceptElectiveCount = Convert.ToInt32(sumObject);
                //List<System.Decimal> listSubNo = dtSubjectDet.AsEnumerable()
                //           .Select(r => r.Field<System.Decimal>("subject_no"))
                //           .ToList();

                //string subNos = string.Join(",", listSubNo.ToArray());

                //DataTable dtPairValues = dirAccess.selectDataTable("select distinct practicalPair from subject where subject_no in ("+subNos+")");

                dtSubjectDet.DefaultView.RowFilter = "practicalPair>0";
                DataTable dtPairValues = dtSubjectDet.DefaultView.ToTable(true, "practicalPair", "subject_no");

                int exceptElectiveCount = Convert.ToInt32(dtSubjectDet.Compute("sum(noofhrsperweek)", "ElectivePap='false'"));
                dtSubjectDet.DefaultView.RowFilter = "ElectivePap='true'";
                int electiveCount = Convert.ToInt32(dtSubjectDet.DefaultView.ToTable(true, "subType_no", "noofhrsperweek").Compute("sum(noofhrsperweek)", string.Empty));

                maxNoCanAllot = exceptElectiveCount + electiveCount + 5;

                dtSubjectDetWt = dirAccess.selectDataTable("select sa.appl_id,sm.syll_code,ss.subType_no,ss.subject_type,isnull(ss.ElectivePap,0) as ElectivePap,ss.Lab,s.subject_no,s.subject_code,s.subject_name,isnull(s.sub_lab,0) as sub_lab,isnull(s.noofhrsperweek,0) as noofhrsperweek,s.maximumHrsPerDay,sts.staff_code,isnull(s.subjectpriority,0) as subjectpriority,sts.staffPriority, sts.facultyChoice,s.practicalPair   from  staff_appl_master sa, staffmaster sf ,syllabus_master sm,sub_sem ss, subject s,staff_selector sts where sm.syll_code = ss.syll_code and sm.syll_code = s.syll_code and ss.syll_code= s.syll_code and ss.subType_no = s.subType_no and s.subject_no =sts.subject_no and sa.appl_no = sf.appl_no and sf.staff_code=sts.staff_code  and sf.college_code ='" + collegecode + "' and (s.subject_no not in (select distinct subject_no from TT_StudentCriteria where semester='" + currentSem + "' and degree_code='" + degreeCode + "' and batch_year='" + batchYear + "')) order by sts.facultyChoice asc ");// and sm.Batch_Year='" + batchYear + "' and sm.degree_code ='" + degreeCode + "' and sm.semester = '" + currentSem + "'

                DataTable dtLabSubjDetWt = dirAccess.selectDataTable("select sa.appl_id, sm.syll_code, ss.subType_no, ss.subject_type, isnull(ss.ElectivePap,0) as ElectivePap, ss.Lab,s.subject_no, s.subject_code, s.subject_name, isnull(s.sub_lab,0) as sub_lab, isnull(s.noofhrsperweek,0) as noofhrsperweek, s.maximumHrsPerDay, sts.staff_code, isnull(s.subjectpriority,0) as subjectpriority, sts.staffPriority, sts.facultyChoice, lc.FacLabChoiceValue from  staff_appl_master sa, staffmaster sf ,syllabus_master sm,sub_sem ss, subject s,staff_selector sts,TT_facultyLabChoice lc where sts.staffPriority=lc.staffPriorityFk and sts.facultyChoice is null and ss.Lab='1' and sm.syll_code = ss.syll_code and sm.syll_code = s.syll_code and ss.syll_code= s.syll_code and ss.subType_no = s.subType_no and s.subject_no =sts.subject_no and sa.appl_no = sf.appl_no and sf.staff_code=sts.staff_code  and sf.college_code ='" + collegecode + "' and sm.Batch_Year='" + batchYear + "' and sm.degree_code ='" + degreeCode + "' and sm.semester = '" + currentSem + "' order by lc.FacLabChoiceValue asc ");

                if (dtSubjectDet.Rows.Count > 0 && dtSubjectDetWt.Rows.Count > 0)
                {
                    byte maxFacultyChoice = (byte)dirAccess.selectScalarInt("select max(isnull(sts.facultyChoice,1)) as facultyChoice from  staff_appl_master sa, staffmaster sf ,syllabus_master sm,sub_sem ss, subject s,staff_selector sts where sm.syll_code = ss.syll_code and sm.syll_code = s.syll_code and ss.syll_code= s.syll_code and ss.subType_no = s.subType_no and s.subject_no =sts.subject_no and sa.appl_no = sf.appl_no and sf.staff_code=sts.staff_code  and sf.college_code ='" + collegecode + "' and sm.Batch_Year='" + batchYear + "' and sm.degree_code ='" + degreeCode + "' and sm.semester = '" + currentSem + "'");

                    if (maxFacultyChoice > 0)
                    {
                        //Staff availability
                        dtStaffDet = dirAccess.selectDataTable("select staff_appno ,DaysFK ,HoursFK , degreeCode ,batch_year , semester ,IsEngaged ,subject_no ,section,'0' as MaxHour  from TT_Staff_AllotAvail --  where Batch_Year='" + batchYear + "' and degreecode ='" + degreeCode + "' and semester = '" + currentSem + "' ");

                        Dictionary<string, bool> dicSubType = new Dictionary<string, bool>();
                        for (int subjI = 0; subjI < dtSubjectDet.Rows.Count; subjI++)
                        {
                            string subject_no = Convert.ToString(dtSubjectDet.Rows[subjI]["subject_no"]);
                            //string ElectivePap = Convert.ToString(dtSubjectDet.Rows[subjI]["ElectivePap"]);
                            string Lab = Convert.ToString(dtSubjectDet.Rows[subjI]["Lab"]).Trim().ToUpper();
                            int noofhrsperweek = Convert.ToInt32(dtSubjectDet.Rows[subjI]["noofhrsperweek"]);

                            int pairValue = Convert.ToInt32(dtSubjectDet.Rows[subjI]["practicalPair"]);

                            if (pairValue > 0)
                            {
                                dtPairValues.DefaultView.RowFilter = "practicalPair='" + pairValue + "'";
                                DataView dvPair = dtPairValues.DefaultView;
                                if (dvPair.Count > 0)
                                {
                                    StringBuilder sbSubNo = new StringBuilder();
                                    for (int dvI = 0; dvI < dvPair.Count; dvI++)
                                    {
                                        sbSubNo.Append(dvPair[dvI]["subject_no"].ToString() + "-");
                                    }
                                    if (sbSubNo.Length > 0)
                                    {
                                        sbSubNo.Remove(sbSubNo.Length - 1, 1);
                                    }
                                    if (!dicSubType.ContainsKey(sbSubNo.ToString()))
                                    {
                                        dicSubType.Add(sbSubNo.ToString(), (Lab == "TRUE" ? true : false));
                                        dtFacultyChoices.Columns.Add(sbSubNo.ToString());

                                        maxNoCanAllot -= (dvPair.Count - 1) * noofhrsperweek;
                                    }
                                }
                            }
                            else
                            {
                                dicSubType.Add(subject_no, (Lab == "TRUE" ? true : false));
                                dtFacultyChoices.Columns.Add(subject_no);
                            }
                        }

                        for (byte choiceI = 1; choiceI <= maxFacultyChoice; choiceI++)
                        {
                            DataRow drFacultyChoice = dtFacultyChoices.NewRow();
                            for (int subjI = 0; subjI < dtFacultyChoices.Columns.Count; subjI++)
                            {
                                string subject_no = Convert.ToString(dtFacultyChoices.Columns[subjI].ColumnName);
                                byte currChoice = choiceI;

                            checkForFacultyInLessChoice:

                                bool isLab = dicSubType[subject_no];
                                DataView dvFaculty = new DataView();
                                if (isLab)
                                {
                                    //dtLabSubjDetWt.DefaultView.RowFilter = " FacLabChoiceValue='" + currChoice + "' and subject_no='" + subject_no + "' ";
                                    //dvFaculty = dtLabSubjDetWt.DefaultView;
                                    string[] subnos = subject_no.Split('-');
                                    StringBuilder sbSnos = new StringBuilder();
                                    for (int sI = 0; sI < subnos.Length; sI++)
                                    {
                                        sbSnos.Append(subnos[sI] + ",");
                                    }
                                    if (sbSnos.Length > 0)
                                    {
                                        sbSnos.Remove(sbSnos.Length - 1, 1);
                                    }

                                    dtSubjectDetWt.DefaultView.RowFilter = " facultyChoice='" + currChoice + "' and subject_no in (" + sbSnos.ToString() + ") ";
                                    dvFaculty = dtSubjectDetWt.DefaultView;
                                    dvFaculty.Sort = " subject_no asc";
                                    currChoice--;
                                    if (dvFaculty.Count > 0)
                                    {
                                        if (dvFaculty.Count > 1)
                                        {
                                            drFacultyChoice[subject_no] = Convert.ToString(dvFaculty[0]["staff_code"]) + "-" + subnos[0] + "," + Convert.ToString(dvFaculty[1]["staff_code"]) + "-" + subnos[1];
                                        }
                                        else
                                        {
                                            drFacultyChoice[subject_no] = Convert.ToString(dvFaculty[0]["staff_code"]) + "-" + subnos[0];
                                        }
                                    }
                                    else
                                    {
                                        if (currChoice > 0)
                                            goto checkForFacultyInLessChoice;
                                    }

                                }
                                else
                                {
                                    dtSubjectDetWt.DefaultView.RowFilter = " facultyChoice='" + currChoice + "' and subject_no='" + subject_no + "' ";
                                    dvFaculty = dtSubjectDetWt.DefaultView;

                                    currChoice--;
                                    if (dvFaculty.Count > 0)
                                    {
                                        //if (dvFaculty.Count > 1)
                                        //{
                                        //    drFacultyChoice[subject_no] = Convert.ToString(dvFaculty[0]["staff_code"]) + "','" + Convert.ToString(dvFaculty[1]["staff_code"]);
                                        //}
                                        //else
                                        //{
                                        drFacultyChoice[subject_no] = Convert.ToString(dvFaculty[0]["staff_code"]);
                                        //}
                                    }
                                    else
                                    {
                                        if (currChoice > 0)
                                            goto checkForFacultyInLessChoice;
                                    }
                                }

                            }
                            dtFacultyChoices.Rows.Add(drFacultyChoice);
                        }
                    }
                }
            }
        }
        catch { dtFacultyChoices.Clear(); }
        return getFacultyCombination(dtFacultyChoices);
    }
}
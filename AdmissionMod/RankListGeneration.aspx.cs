using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Text;
using System.Collections;
using InsproDataAccess;
using System.IO;


public partial class AdmissionMod_RankListGeneration : System.Web.UI.Page
{
    DAccess2 d2 = new DAccess2();
    //DataSet ds = new DataSet();
    InsproDirectAccess Dir = new InsproDirectAccess();
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

            getTextCodeOrInsert("ADMst", "Stream I", ddlCollege.SelectedValue);
            getTextCodeOrInsert("ADMst", "Stream II", ddlCollege.SelectedValue);

            bindBatch();
            bindEdulevel();
            bindCourse();
            bindStream();
            bindCriteria();
            BindMultiSeelction();
            BindMultiSeelctionSSLC();

        }
    }

    public void bindCollege()
    {
        try
        {
            DataSet ds = new DataSet();
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
            DataSet ds = new DataSet();
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
            DataSet ds = new DataSet();
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
            DataSet ds = new DataSet();
            ds = d2.select_method_wo_parameter("select distinct course_id,Course_Name from Course where college_code=" + ddlCollege.SelectedValue + " and Edu_level='" + ddlEduLev.SelectedItem.Text + "' order by course_id", "Text");
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
    public void bindCriteria()
    {
        try
        {
            DataSet ds = new DataSet();
            ds = d2.select_method_wo_parameter("select MasterValue,MasterCode from CO_MasterValues where  MasterCriteria ='StudRankCriteria' and CollegeCode ='" + ddlCollege.SelectedValue + "' order by MasterCode", "Text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                ddl_coltypeadd.DataSource = ds;
                ddl_coltypeadd.DataTextField = "MasterValue";
                ddl_coltypeadd.DataValueField = "MasterCode";
                ddl_coltypeadd.DataBind();
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
            DataSet ds = d2.select_method_wo_parameter("SELECT TextVal,TextCode FROM TextValTable WHERE TextCriteria='ADMst' AND college_code='" + ddlCollege.SelectedValue + "' order by TextVal,TextCode ", "Text");
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

    protected void closeTTImport(object sender, EventArgs e)
    {
        divImport.Visible = false;
    }
    public void btn_addtype_OnClick(object sender, EventArgs e)
    {
        imgdiv33.Visible = true;
        panel_description11.Visible = true;
    }
    public void btn_deltype_OnClick(object sender, EventArgs e)
    {
        if (ddl_coltypeadd.SelectedIndex == -1)
        {
            imgdiv2.Visible = true;
            lbl_alert.Text = "No records found";
        }
        else if (ddl_coltypeadd.SelectedIndex == 0)
        {
            imgdiv2.Visible = true;
            lbl_alert.Text = "Select any record";
        }
        else if (ddl_coltypeadd.SelectedIndex != 0)
        {
            string sql = "delete from CO_MasterValues where MasterCode='" + ddl_coltypeadd.SelectedItem.Value.ToString() + "' and MasterCriteria='StudRankCriteria' and CollegeCode='" + ddlCollege.SelectedItem.Value + "' ";
            int delete = d2.update_method_wo_parameter(sql, "TEXT");
            if (delete != 0)
            {
                imgdiv2.Visible = true;
                lbl_alert.Text = "Deleted Sucessfully";
            }
            else
            {
                imgdiv2.Visible = true;
                lbl_alert.Text = "No records found";
            }
        }
        else
        {
            imgdiv2.Visible = true;
            lbl_alert.Text = "No records found";
        }
    }

    public void btndescpopadd_Click(object sender, EventArgs e)
    {
        if (txt_description11.Text != "")
        {
            string sql = "if exists ( select * from CO_MasterValues where MasterValue ='" + txt_description11.Text + "' and MasterCriteria ='StudRankCriteria' and CollegeCode ='" + ddlCollege.SelectedItem.Value + "') update CO_MasterValues set MasterValue ='" + txt_description11.Text + "' where MasterValue ='" + txt_description11.Text + "' and MasterCriteria ='StudRankCriteria' and CollegeCode ='" + ddlCollege.SelectedItem.Value + "' else insert into CO_MasterValues (MasterValue,MasterCriteria,CollegeCode) values ('" + txt_description11.Text + "','StudRankCriteria','" + ddlCollege.SelectedItem.Value + "')";
            int insert = d2.update_method_wo_parameter(sql, "TEXT");
            if (insert != 0)
            {
                imgdiv2.Visible = true;
                lbl_alert.Text = "Added sucessfully";
                txt_description11.Text = string.Empty;
                //imgdiv33.Visible = false;        
                bindCriteria();
            }
        }
        else
        {
            imgdiv2.Visible = true;
            pnl2.Visible = true;
            lbl_alert.Text = "Enter the description";
        }

    }
    public void btndescpopexit_Click(object sender, EventArgs e)
    {
        panel_description11.Visible = false;
        imgdiv33.Visible = false;
    }
    public void btn_errorclose_Click(object sender, EventArgs e)
    {
        imgdiv2.Visible = false;
    }

    protected void btnGenerateSettings_Click(object sender, EventArgs e)
    {
        divImport.Visible = true;
        BindMultiSeelction();
        BindMultiSeelctionSSLC();
        bindCriteria();
        CriteriaGrid.Visible = false;
        string SelectNewQuery = "select CriteriaCode,CriteriaValue,CriteriaCodevalue,MasterValue,SSLCCriteriaValue,SSLCCriteriaCodeValue from ST_RankCriteria St,CO_MasterValues CO where ST.CriteriaCode=CO.MasterCode and MasterCriteria='StudRankCriteria' and st.CollegeCode='" + ddlCollege.SelectedValue + "' and batchYear='" + ddlbatch.SelectedItem.Text + "' and CourseID='" + ddlcourse.SelectedValue + "' and EduLevel='" + ddlEduLev.SelectedItem.Text + "'";
        DataSet ds = new DataSet();
        ds = d2.select_method_wo_parameter(SelectNewQuery, "Text");
        if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
        {
            DataTable dt = new DataTable();
            DataRow dr;
            dt.Columns.Add("S.No", typeof(string));
            dt.Columns.Add("Criteria", typeof(string));
            dt.Columns.Add("HSC Value", typeof(string));
            dt.Columns.Add("SSLC Value", typeof(string));
            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            {
                dr = dt.NewRow();
                dr[0] = (i + 1);
                dr[1] = Convert.ToString(ds.Tables[0].Rows[i]["MasterValue"]);
                dr[2] = Convert.ToString(ds.Tables[0].Rows[i]["CriteriaValue"]);
                dr[3] = Convert.ToString(ds.Tables[0].Rows[i]["SSLCCriteriaValue"]);
                dt.Rows.Add(dr);
            }
            if (dt.Rows.Count > 0)
            {
                CriteriaGrid.DataSource = dt;
                CriteriaGrid.DataBind();
                CriteriaGrid.Visible = true;
            }
        }
        else
        {
            ScriptManager.RegisterStartupScript(this, typeof(Page), UniqueID, "alert('Please select any one district')", true);
        }
    }
    public void bindCriteriaTable()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            StringBuilder sbnew = new StringBuilder();

            StringBuilder SSLCsb = new StringBuilder();
            StringBuilder SSLCsbnew = new StringBuilder();

            for (int i = 0; i < MultipleSelection.Items.Count; i++)
            {
                if (MultipleSelection.Items[i].Selected == true)
                {
                    sb.Append(MultipleSelection.Items[i].Text + ",");
                    sbnew.Append(MultipleSelection.Items[i].Value + ",");
                }
            }
            if (sb.Length > 0)
            {
                sb = sb.Remove(sb.Length - 1, 1);
                sbnew = sbnew.Remove(sbnew.Length - 1, 1);
            }

            for (int i = 0; i < MultipleSelectionSSLC.Items.Count; i++)
            {
                if (MultipleSelectionSSLC.Items[i].Selected == true)
                {
                    SSLCsb.Append(MultipleSelectionSSLC.Items[i].Text + ",");
                    SSLCsbnew.Append(MultipleSelectionSSLC.Items[i].Value + ",");
                }
            }
            if (SSLCsb.Length > 0)
            {
                SSLCsb = SSLCsb.Remove(SSLCsb.Length - 1, 1);
                SSLCsbnew = SSLCsbnew.Remove(SSLCsbnew.Length - 1, 1);
            }

            string SelectQuery = "if exists (select * from ST_RankCriteria where CriteriaCode='" + ddl_coltypeadd.SelectedItem.Value
               + "' and CollegeCode='" + ddlCollege.SelectedValue + "' and batchYear='" + ddlbatch.SelectedItem.Text + "' and CourseID='" + ddlcourse.SelectedValue + "' and EduLevel='" + ddlEduLev.SelectedItem.Text + "') update ST_RankCriteria set CriteriaValue='" + sb + "' ,CriteriaCodevalue='" + sbnew + "',SSLCCriteriaValue='" + SSLCsb + "',SSLCCriteriaCodeValue='" + SSLCsbnew + "'  where CriteriaCode='" + ddl_coltypeadd.SelectedItem.Value + "' and CollegeCode='" + ddlCollege.SelectedValue + "' and batchYear='" + ddlbatch.SelectedItem.Text + "' and CourseID='" + ddlcourse.SelectedValue + "' and EduLevel='" + ddlEduLev.SelectedItem.Text + "' else insert into ST_RankCriteria (CriteriaCode,CriteriaValue,CriteriaCodevalue,CollegeCode,BatchYear,CourseID,EduLevel,SSLCCriteriaValue,SSLCCriteriaCodeValue) values ('" + ddl_coltypeadd.SelectedItem.Value + "','" + sb + "','" + sbnew + "','" + ddlCollege.SelectedValue + "','" + ddlbatch.SelectedItem.Text + "','" + ddlcourse.SelectedValue + "','" + ddlEduLev.SelectedItem.Text + "','" + SSLCsb + "','" + SSLCsbnew + "')";

            int ins = d2.update_method_wo_parameter(SelectQuery, "Text");

            string SelectNewQuery = "select CriteriaCode,CriteriaValue,CriteriaCodevalue,MasterValue,SSLCCriteriaValue,SSLCCriteriaCodeValue from ST_RankCriteria St,CO_MasterValues CO where ST.CriteriaCode=CO.MasterCode and MasterCriteria='StudRankCriteria' and st.CollegeCode='" + ddlCollege.SelectedValue + "' and batchYear='" + ddlbatch.SelectedItem.Text + "' and CourseID='" + ddlcourse.SelectedValue + "' and EduLevel='" + ddlEduLev.SelectedItem.Text + "'";
            DataSet ds = new DataSet();
            ds = d2.select_method_wo_parameter(SelectNewQuery, "Text");
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                DataTable dt = new DataTable();
                DataRow dr;
                dt.Columns.Add("S.No", typeof(string));
                dt.Columns.Add("Criteria", typeof(string));
                dt.Columns.Add("HSC Value", typeof(string));
                dt.Columns.Add("SSLC Value", typeof(string));
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    dr = dt.NewRow();
                    dr[0] = (i + 1);
                    dr[1] = Convert.ToString(ds.Tables[0].Rows[i]["MasterValue"]);
                    dr[2] = Convert.ToString(ds.Tables[0].Rows[i]["CriteriaValue"]);
                    dr[3] = Convert.ToString(ds.Tables[0].Rows[i]["SSLCCriteriaValue"]);
                    dt.Rows.Add(dr);
                }
                if (dt.Rows.Count > 0)
                {
                    CriteriaGrid.DataSource = dt;
                    CriteriaGrid.DataBind();
                    CriteriaGrid.Visible = true;
                }
            }
            else
            {
                ScriptManager.RegisterStartupScript(this, typeof(Page), UniqueID, "alert('Please select any one district')", true);
            }

        }
        catch
        {

        }
    }

    protected void btnImportExcel_Click(object sender, EventArgs e)
    {
        try
        {
            bindCriteriaTable();
        }
        catch
        {

        }
    }

    public void BindMultiSeelction()
    {
        try
        {
            DataSet ds = new DataSet();
            ds = d2.select_method_wo_parameter(" select MasterValue,MasterCode from CO_MasterValues where  MasterCriteria ='HSCDistrict'  and CollegeCode ='" + ddlCollege.SelectedValue + "' order by MasterCode", "Text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                MultipleSelection.DataSource = ds;
                MultipleSelection.DataTextField = "MasterValue";
                MultipleSelection.DataValueField = "MasterCode";
                MultipleSelection.DataBind();
            }
        }
        catch
        {

        }
    }

    public void BindMultiSeelctionSSLC()
    {
        try
        {
            DataSet ds = new DataSet();
            ds = d2.select_method_wo_parameter(" select MasterValue,MasterCode from CO_MasterValues where  MasterCriteria ='SSLCDistrict'  and CollegeCode ='" + ddlCollege.SelectedValue + "' order by MasterCode", "Text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                MultipleSelectionSSLC.DataSource = ds;
                MultipleSelectionSSLC.DataTextField = "MasterValue";
                MultipleSelectionSSLC.DataValueField = "MasterCode";
                MultipleSelectionSSLC.DataBind();
            }
        }
        catch
        {

        }
    }

    protected void btnBordWiseMaximumMark_Click(object sender, EventArgs e)
    {
        try
        {
            DivBoradWiseSetMark.Visible = true;
            string SelectQueryBoard = "select distinct board,TextVal from applyn a,ST_Student_Mark_Detail st,Degree d,Course c,TextValTable T where a.app_no=st.ST_AppNo and d.course_id=c.course_id and a.courseID=d.course_id and c.course_id =a.courseID and T.TextCode=st.board and T.TextCriteria like 'unive%' and a.batch_year='" + ddlbatch.SelectedItem.Text + "' and a.college_code='" + ddlCollege.SelectedValue + "' and c.edu_level='" + ddlEduLev.SelectedItem.Text + "' and c.course_id ='" + ddlcourse.SelectedValue + "' order by board asc";
            DataSet ds = new DataSet();
            ds = d2.select_method_wo_parameter(SelectQueryBoard, "Text");
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                GridBoardWiseMaxMark.DataSource = ds;
                GridBoardWiseMaxMark.DataBind();
            }
            if (GridBoardWiseMaxMark.Rows.Count > 0)
            {
                DataView dv = new DataView();
                string SelectBoard = "select BoardFK,TopperMark,CollegeCode,batchYear,MaximumMark,MathsMark,PhysicsMark,ChemistyMark from ST_Stud_BoardWiseTopper where collegeCode='" + ddlCollege.SelectedValue + "' and batchYear='" + ddlbatch.SelectedItem.Text + "'";
                ds.Clear();
                ds = d2.select_method_wo_parameter(SelectBoard, "Text");
                if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    for (int BoardGrid = 0; BoardGrid < GridBoardWiseMaxMark.Rows.Count; BoardGrid++)
                    {
                        string BoardCode = Convert.ToString((GridBoardWiseMaxMark.Rows[BoardGrid].FindControl("lblboardCode") as Label).Text);
                        string TopperMark = string.Empty;
                        string MAxMark = string.Empty;
                        string MathsMArk = string.Empty;
                        string PhysicsMark = string.Empty;
                        string ChemistryMArk = string.Empty;
                        ds.Tables[0].DefaultView.RowFilter = "BoardFK='" + BoardCode + "' and CollegeCode='" + ddlCollege.SelectedValue + "'";
                        dv = ds.Tables[0].DefaultView;
                        if (dv.Count > 0)
                        {
                            TopperMark = Convert.ToString(dv[0]["TopperMark"]);
                            MAxMark = Convert.ToString(dv[0]["MaximumMark"]);
                            MathsMArk = Convert.ToString(dv[0]["MathsMark"]);
                            PhysicsMark = Convert.ToString(dv[0]["PhysicsMark"]);
                            ChemistryMArk = Convert.ToString(dv[0]["ChemistyMark"]);
                        }
                        (GridBoardWiseMaxMark.Rows[BoardGrid].FindControl("txt_MaxTopper") as TextBox).Text = Convert.ToString(TopperMark);
                        (GridBoardWiseMaxMark.Rows[BoardGrid].FindControl("txt_MaxMark") as TextBox).Text = Convert.ToString
(MAxMark);
                        (GridBoardWiseMaxMark.Rows[BoardGrid].FindControl("txt_MathsMax") as TextBox).Text = Convert.ToString(MathsMArk);
                        (GridBoardWiseMaxMark.Rows[BoardGrid].FindControl("txt_Physics") as TextBox).Text = Convert.ToString(PhysicsMark);
                        (GridBoardWiseMaxMark.Rows[BoardGrid].FindControl("txt_Chemistry") as TextBox).Text = Convert.ToString(ChemistryMArk);
                    }
                }
            }
        }
        catch
        {

        }

    }

    protected void imageButtonNew_Clcik(object sender, EventArgs e)
    {
        try
        {
            DivBoradWiseSetMark.Visible = false;
        }
        catch
        {

        }
    }

    protected void btnMaxMarkSave_Click(object sender, EventArgs e)
    {
        try
        {
            bool CheckSaveFlage = false;
            if (GridBoardWiseMaxMark.Rows.Count > 0)
            {
                for (int BoardI = 0; BoardI < GridBoardWiseMaxMark.Rows.Count; BoardI++)
                {
                    string BoardCode = Convert.ToString((GridBoardWiseMaxMark.Rows[BoardI].FindControl("lblboardCode") as Label).Text);
                    string TopperMark = Convert.ToString((GridBoardWiseMaxMark.Rows[BoardI].FindControl("txt_MaxTopper") as TextBox).Text);
                    if (TopperMark.Trim() == "")
                        TopperMark = "0";
                    string MaxMArk = Convert.ToString((GridBoardWiseMaxMark.Rows[BoardI].FindControl("txt_MaxMark") as TextBox).Text);
                    if (MaxMArk.Trim() == "")
                        MaxMArk = "0";
                    string MathsMark = Convert.ToString((GridBoardWiseMaxMark.Rows[BoardI].FindControl("txt_MathsMax") as TextBox).Text);
                    if (MathsMark.Trim() == "")
                        MathsMark = "0";
                    string ChemistyMark = Convert.ToString((GridBoardWiseMaxMark.Rows[BoardI].FindControl("txt_Chemistry") as TextBox).Text);
                    if (ChemistyMark.Trim() == "") //txt_Chemistry
                        ChemistyMark = "0";
                    string PhysicsMark = Convert.ToString((GridBoardWiseMaxMark.Rows[BoardI].FindControl("txt_Physics") as TextBox).Text);
                    if (PhysicsMark.Trim() == "")
                        PhysicsMark = "0";
                    if (TopperMark.Trim() != "" && TopperMark.Trim() != "0")
                    {
                        string InsertQuery = "if exists (select * from ST_Stud_BoardWiseTopper where BoardFk='" + BoardCode + "' and collegeCode='" + ddlCollege.SelectedValue + "' and batchYear='" + ddlbatch.SelectedItem.Text + "') update ST_Stud_BoardWiseTopper set TopperMark='" + TopperMark + "',MaximumMark='" + MaxMArk + "',MathsMark='" + MathsMark + "',PhysicsMark='" + PhysicsMark + "',ChemistyMark='" + ChemistyMark + "' where BoardFk='" + BoardCode + "' and collegeCode='" + ddlCollege.SelectedValue + "' and batchYear='" + ddlbatch.SelectedItem.Text + "' else insert into ST_Stud_BoardWiseTopper (BoardFK,TopperMark,CollegeCode,batchYear,MaximumMark,MathsMark,PhysicsMark,ChemistyMark) values ('" + BoardCode + "','" + TopperMark + "','" + ddlCollege.SelectedValue + "','" + ddlbatch.SelectedItem.Text + "','" + MaxMArk + "','" + MathsMark + "','" + PhysicsMark + "','" + ChemistyMark + "') ";
                        int ins = d2.update_method_wo_parameter(InsertQuery, "Text");
                        CheckSaveFlage = true;
                    }
                }

                if (CheckSaveFlage == true)
                    ScriptManager.RegisterStartupScript(this, typeof(Page), UniqueID, "alert('Saved Successfully')", true);
                else
                    ScriptManager.RegisterStartupScript(this, typeof(Page), UniqueID, "alert('Not Saved')", true);
            }
        }
        catch
        {

        }
    }

    protected void btnGenOption_Click(object sender, EventArgs e)
    {
        try
        {
            Dictionary<string, double> BoardMark = new Dictionary<string, double>();

            Hashtable RankHashTable = new Hashtable();
            ArrayList AddArray = new ArrayList();
            bool CheckSave = false;

            string SelectTableQuery = "select  a.app_no,jeeMarkSec,jeeMaxMark,HSCMarkSec,HSCMaxMark,board,HSCDistrict,SSLCDistrict from applyn a,ST_Student_Mark_Detail st where a.app_no=st.st_appno and a.batch_year='" + ddlbatch.SelectedItem.Text + "' and a.college_code='" + ddlCollege.SelectedValue + "' and a.courseID='" + ddlcourse.SelectedValue + "'";
            SelectTableQuery += " select BoardFK,TopperMark,CollegeCode,batchYear from ST_Stud_BoardWiseTopper where collegeCode='" + ddlCollege.SelectedValue + "' and batchYear='" + ddlbatch.SelectedItem.Text + "'";

            SelectTableQuery += " select MasterValue,MasterCode,CriteriaCodeValue,CriteriaValue,SSLCCriteriaCodeValue,SSLCCriteriaValue from ST_RankCriteria R,Co_MasterValues C where R.CriteriaCode=C.MasterCode and R.collegeCode='" + ddlCollege.SelectedValue + "' and R.batchYear='" + ddlbatch.SelectedItem.Text + "' and r.CourseID='" + ddlcourse.SelectedValue + "' and R.EduLevel='" + ddlEduLev.SelectedItem.Text + "' order by MasterCode ; select MasterCode,MasterValue from Co_MasterValues where MasterCriteria='StudRankCriteria' and collegecode='" + ddlCollege.SelectedValue + "' ; select MasterCode,MasterValue from Co_MasterValues where MasterCriteria='HSCDistrict' and collegecode='" + ddlCollege.SelectedValue + "' ; select MasterCode,MasterValue from Co_MasterValues where MasterCriteria='SSLCDistrict' and collegecode='" + ddlCollege.SelectedValue + "'";

            DataSet dsGen = d2.select_method_wo_parameter(SelectTableQuery, "Text");
            if (dsGen.Tables.Count > 4 && dsGen.Tables[0].Rows.Count > 0)
            {
                double ToppMark = 0;
                if (dsGen.Tables[1].Rows.Count > 0)
                {
                    for (int TopMark = 0; TopMark < dsGen.Tables[1].Rows.Count; TopMark++)
                    {
                        string Board = Convert.ToString(dsGen.Tables[1].Rows[TopMark]["BoardFK"]);
                        double.TryParse(Convert.ToString(dsGen.Tables[1].Rows[TopMark]["TopperMark"]), out ToppMark);
                        if (!BoardMark.ContainsKey(Board))
                        {
                            BoardMark.Add(Board, ToppMark);
                        }
                    }
                }
                if (BoardMark.Count > 0)
                {
                    DataTable dt = new DataTable();
                    dt.Columns.Add("AppNo", typeof(Int64));
                    dt.Columns.Add("CombinedScore", typeof(decimal));
                    dt.Columns.Add("HSCDistrict", typeof(decimal));
                    dt.Columns.Add("SSLCDistrict", typeof(decimal));

                    double JeeMark = 0;
                    double HSCMarkSec = 0;
                    double CombinedScore = 0;
                    double MaxTopMark = 0;
                    double HSCPercentage = 0;
                    double JeePercentage = 0;
                    double HscDistrict = 0;
                    double SslcDistrict = 0;
                    for (int StRank = 0; StRank < dsGen.Tables[0].Rows.Count; StRank++)
                    {
                        string AppNo = Convert.ToString(dsGen.Tables[0].Rows[StRank]["app_no"]);
                        double.TryParse(Convert.ToString(dsGen.Tables[0].Rows[StRank]["jeeMarkSec"]), out JeeMark);
                        double.TryParse(Convert.ToString(dsGen.Tables[0].Rows[StRank]["HSCMarkSec"]), out HSCMarkSec);
                        double.TryParse(Convert.ToString(dsGen.Tables[0].Rows[StRank]["HSCDistrict"]), out HscDistrict);
                        double.TryParse(Convert.ToString(dsGen.Tables[0].Rows[StRank]["SSLCDistrict"]), out SslcDistrict);
                        string Board = Convert.ToString(dsGen.Tables[0].Rows[StRank]["board"]);
                        MaxTopMark = BoardMark[Board];

                        if (MaxTopMark != 0)
                        {
                            HSCPercentage = (HSCMarkSec / MaxTopMark) * 100;
                            if (ddlStream.SelectedIndex == 0)
                            {
                                HSCPercentage = (HSCPercentage / 100) * 75;

                                JeePercentage = (JeeMark / 100) * 25;
                            }

                            CombinedScore = (HSCPercentage + JeePercentage);
                            DataRow dr = dt.NewRow();
                            dr[0] = AppNo;
                            dr[1] = Convert.ToString(Math.Round(CombinedScore, 4));
                            dr[2] = Convert.ToString(HscDistrict);
                            dr[3] = Convert.ToString(SslcDistrict);
                            dt.Rows.Add(dr);
                        }
                    }
                    if (dt.Rows.Count > 0)
                    {
                        DataTable DTRankCalculation = dt;
                        DataView dvCalc = new DataView();
                        if (dsGen.Tables[3].Rows.Count > 0)
                        {
                            for (byte intCr = 0; intCr < dsGen.Tables[3].Rows.Count; intCr++)
                            {
                                Int64 CriCode = Convert.ToInt64(dsGen.Tables[3].Rows[intCr]["MasterCode"]);
                                dsGen.Tables[2].DefaultView.RowFilter = "MasterCode=" + CriCode + "";
                                dvCalc = dsGen.Tables[2].DefaultView;
                                if (dvCalc.Count > 0)
                                {
                                    for (byte intDv = 0; intDv < dvCalc.Count; intDv++)
                                    {
                                        string CriCodeValue = Convert.ToString(dvCalc[intDv]["CriteriaCodeValue"]);
                                        string SSLCCodeValue = Convert.ToString(dvCalc[intDv]["SSLCCriteriaCodeValue"]);
                                        string View = string.Empty;
                                        if (SSLCCodeValue.Trim() != "" && SSLCCodeValue.Trim() != "0")
                                            View = " HSCDistrict in (" + CriCodeValue + ") and SSLCDistrict in (" + SSLCCodeValue + ")";
                                        else
                                            View = " HSCDistrict in (" + CriCodeValue + ")";
                                        DataTable DvNEwCalCulation = DTRankCalculation;
                                        DvNEwCalCulation.DefaultView.RowFilter = View;
                                        DataView DCalc = DvNEwCalCulation.DefaultView;
                                        DCalc.Sort = "CombinedScore desc";
                                        if (DCalc.Count > 0)
                                        {
                                            if (!RankHashTable.ContainsKey(CriCode))
                                            {
                                                DataTable DSet = DCalc.ToTable();
                                                RankHashTable.Add(CriCode, DSet);
                                                AddArray.Add(DCalc.Count);
                                            }
                                        }
                                    }
                                }
                            }
                            if (RankHashTable.Count > 0)
                            {
                                foreach (DictionaryEntry Di in RankHashTable)
                                {
                                    string KeyVAlue = Convert.ToString(Di.Key);
                                    DataTable DtRanktable = (DataTable)Di.Value;
                                    if (DtRanktable.Rows.Count > 0)
                                    {
                                        for (int intDt = 0; intDt < DtRanktable.Rows.Count; intDt++)
                                        {
                                            Int64 ST_AppNo = Convert.ToInt64(DtRanktable.Rows[intDt]["AppNo"]);
                                            string CBScore = Convert.ToString(DtRanktable.Rows[intDt]["CombinedScore"]);
                                            string InsQuery = "if exists (select ST_AppNo from ST_RankTable where ST_AppNo='" + ST_AppNo + "' and ST_RankCriteria='" + KeyVAlue + "' and ST_stream='" + ddlStream.SelectedValue + "' ) update  ST_RankTable set ST_Rank='" + (intDt + 1) + "' where ST_AppNo='" + ST_AppNo + "' and ST_RankCriteria='" + KeyVAlue + "'  and ST_stream='" + ddlStream.SelectedValue + "'  else insert into ST_RankTable (ST_AppNo,ST_RankCriteria,ST_Rank,ST_stream) values ('" + ST_AppNo + "','" + KeyVAlue + "','" + (intDt + 1) + "','" + ddlStream.SelectedValue + "') ";
                                            if (ddlStream.SelectedIndex == 0)
                                            {
                                                InsQuery += " update ST_Student_Mark_Detail set CombinedScore='" + CBScore + "' where ST_AppNo='" + ST_AppNo + "'";
                                            }
                                            else
                                            {
                                                InsQuery += " update ST_Student_Mark_Detail set CombinedScoreSII='" + CBScore + "' where ST_AppNo='" + ST_AppNo + "'";
                                            }
                                            d2.update_method_wo_parameter(InsQuery, "Text");
                                            CheckSave = true;
                                        }
                                    }
                                }

                                string selectSameQuery = " select a.stud_name,ST_AppNo,HSCMarkSec,ChyMark,PhyMark,MathsMark,board,convert(varchar(10),dob,101)as dob,App_formno,CombinedScore from ST_Student_Mark_Detail ST,Applyn a where a.app_no=ST.ST_AppNo and a.batch_year=" + ddlbatch.SelectedItem.Text + " and a.college_code ='" + ddlCollege.SelectedValue + "' and a.courseID=" + ddlcourse.SelectedValue + " ; select count(ST.ST_AppNo),CombinedScore from ST_Student_Mark_Detail ST ,Applyn a where a.app_no=ST.ST_AppNo and a.batch_year=" + ddlbatch.SelectedItem.Text + " and a.college_code ='" + ddlCollege.SelectedValue + "' and a.courseID=" + ddlcourse.SelectedValue + "  group by CombinedScore having count(ST.ST_AppNo)>1 order by CombinedScore desc ; select BoardFK,MathsMark,PhysicsMark,ChemistyMark from ST_Stud_BoardWiseTopper where collegeCode='" + ddlCollege.SelectedValue + "' and batchYear='" + ddlbatch.SelectedItem.Text + "' ; select ST_Rank,ST_RankCriteria,ST.ST_AppNo,ST.CombinedScore from ST_RankTable SR,ST_Student_Mark_Detail ST,applyn a where SR.ST_AppNo=ST.ST_AppNo and a.app_no=ST.ST_AppNo and a.app_no=SR.ST_AppNo and a.college_Code='" + ddlCollege.SelectedValue + "' and a.batch_year='" + ddlbatch.SelectedItem.Text + "' and a.courseID ='" + ddlcourse.SelectedValue + "' and ST_stream =" + ddlStream.SelectedValue + "";

                                if (ddlStream.SelectedIndex == 1)
                                {
                                    selectSameQuery = " select a.stud_name,ST_AppNo,HSCMarkSec,ChyMark,PhyMark,MathsMark,board,convert(varchar(10),dob,101)as dob,App_formno,CombinedScoreSII as CombinedScore from ST_Student_Mark_Detail ST,Applyn a where a.app_no=ST.ST_AppNo and a.batch_year=" + ddlbatch.SelectedItem.Text + " and a.college_code ='" + ddlCollege.SelectedValue + "' and a.courseID=" + ddlcourse.SelectedValue + " ; select count(ST.ST_AppNo),CombinedScoreSII as CombinedScore from ST_Student_Mark_Detail ST ,Applyn a where a.app_no=ST.ST_AppNo and a.batch_year=" + ddlbatch.SelectedItem.Text + " and a.college_code ='" + ddlCollege.SelectedValue + "' and a.courseID=" + ddlcourse.SelectedValue + "  group by CombinedScoreSII having count(ST.ST_AppNo)>1 order by CombinedScoreSII desc ; select BoardFK,MathsMark,PhysicsMark,ChemistyMark from ST_Stud_BoardWiseTopper where collegeCode='" + ddlCollege.SelectedValue + "' and batchYear='" + ddlbatch.SelectedItem.Text + "' ; select ST_Rank,ST_RankCriteria,ST.ST_AppNo,ST.CombinedScoreSII as CombinedScore from ST_RankTable SR,ST_Student_Mark_Detail ST,applyn a where SR.ST_AppNo=ST.ST_AppNo and a.app_no=ST.ST_AppNo and a.app_no=SR.ST_AppNo and a.college_Code='" + ddlCollege.SelectedValue + "' and a.batch_year='" + ddlbatch.SelectedItem.Text + "' and a.courseID ='" + ddlcourse.SelectedValue + "' and ST_stream =" + ddlStream.SelectedValue + "";
                                }

                                DataSet dsGen2 = d2.select_method_wo_parameter(selectSameQuery, "Text");
                                if (dsGen2.Tables.Count > 2)
                                {
                                    double SameMark = 0;
                                    DataTable MarkData = new DataTable();

                                    MarkData.Columns.Add("AppNo", typeof(string));
                                    MarkData.Columns.Add("Maths", typeof(double));
                                    MarkData.Columns.Add("Physics", typeof(double));
                                    MarkData.Columns.Add("Chemistery", typeof(double));
                                    MarkData.Columns.Add("dob", typeof(DateTime));
                                    MarkData.Columns.Add("ApplicationNo", typeof(string));

                                    for (int indCmb = 0; indCmb < dsGen2.Tables[1].Rows.Count; indCmb++) //ds.Tables[1].Rows.Count
                                    {
                                        double.TryParse(Convert.ToString(dsGen2.Tables[1].Rows[indCmb]["CombinedScore"]), out SameMark);
                                        DataView DvSame = new DataView();
                                        DataView DvRank = new DataView();

                                        dsGen2.Tables[0].DefaultView.RowFilter = "CombinedScore='" + SameMark + "'";
                                        DvSame = dsGen2.Tables[0].DefaultView;

                                        dsGen2.Tables[3].DefaultView.RowFilter = "CombinedScore='" + SameMark + "'";
                                        DvRank = dsGen2.Tables[3].DefaultView;
                                        DataTable DtRankTable = DvRank.ToTable(true, "ST_RankCriteria");
                                        if (DvSame.Count > 0)
                                        {
                                            string borad = string.Empty;
                                            double MathsMark = 0;
                                            double PhysicsMark = 0;
                                            double ChemistryMark = 0;
                                            string StAppNo = string.Empty;

                                            double MathsMaxMark = 0;
                                            double PhysicsMaxMark = 0;
                                            double ChemistryMaxMark = 0;

                                            double MathsAvgMark = 0;
                                            double PhysicsAvgMark = 0;
                                            double ChemistryAvgMark = 0;

                                            DateTime DtDob;
                                            string ApplicationNo = string.Empty;
                                            for (int intDvSame = 0; intDvSame < DvSame.Count; intDvSame++)
                                            {
                                                StAppNo = Convert.ToString(DvSame[intDvSame]["ST_AppNo"]);
                                                string Board = Convert.ToString(DvSame[intDvSame]["board"]);
                                                MathsMark = Convert.ToDouble(DvSame[intDvSame]["MathsMark"]);
                                                PhysicsMark = Convert.ToDouble(DvSame[intDvSame]["PhyMark"]);
                                                ChemistryMark = Convert.ToDouble(DvSame[intDvSame]["ChyMark"]);
                                                DtDob = Convert.ToDateTime(DvSame[intDvSame]["dob"]);
                                                ApplicationNo = Convert.ToString(DvSame[intDvSame]["stud_name"]);
                                                dsGen2.Tables[2].DefaultView.RowFilter = "BoardFK='" + Board + "'";

                                                DataView DvMaxMark = new DataView();
                                                DvMaxMark = dsGen2.Tables[2].DefaultView;
                                                if (DvMaxMark.Count > 0)
                                                {
                                                    double.TryParse(Convert.ToString(DvMaxMark[0]["MathsMark"]), out MathsMaxMark);
                                                    double.TryParse(Convert.ToString(DvMaxMark[0]["PhysicsMark"]), out PhysicsMaxMark);
                                                    double.TryParse(Convert.ToString(DvMaxMark[0]["ChemistyMark"]), out ChemistryMaxMark);
                                                }
                                                if (MathsMaxMark != 0)
                                                    MathsAvgMark = (MathsMark / MathsMaxMark) * 100;
                                                else
                                                    MathsAvgMark = MathsMark;
                                                if (PhysicsMaxMark != 0)
                                                    PhysicsAvgMark = (PhysicsMark / PhysicsMaxMark) * 100;
                                                else
                                                    PhysicsAvgMark = PhysicsMark;
                                                if (ChemistryMaxMark != 0)
                                                    ChemistryAvgMark = (ChemistryMark / ChemistryMaxMark) * 100;
                                                else
                                                    ChemistryAvgMark = ChemistryMark;

                                                DataRow dNew;
                                                dNew = MarkData.NewRow();
                                                dNew[0] = StAppNo;
                                                dNew[1] = Math.Round(MathsAvgMark, 4);
                                                dNew[2] = Math.Round(PhysicsAvgMark, 4);
                                                dNew[3] = Math.Round(ChemistryAvgMark, 4);
                                                dNew[4] = DtDob;
                                                dNew[5] = ApplicationNo;
                                                MarkData.Rows.Add(dNew);
                                            }
                                            DataView DvSort = MarkData.DefaultView;
                                            DvSort.Sort = "Maths desc,Physics desc,Chemistery desc,dob asc,ApplicationNo asc";
                                            if (DtRankTable.Rows.Count > 0)
                                            {
                                                for (int intDRank = 0; intDRank < DtRankTable.Rows.Count; intDRank++)
                                                {
                                                    string GetCriterai = Convert.ToString(DtRankTable.Rows[intDRank]["ST_RankCriteria"]);
                                                    dsGen2.Tables[3].DefaultView.RowFilter = "CombinedScore='" + SameMark + "' and ST_RankCriteria='" + GetCriterai + "'";
                                                    DvRank = dsGen2.Tables[3].DefaultView;
                                                    DvRank.Sort = "ST_Rank asc";
                                                    DataTable DtSortRank = DvRank.ToTable();

                                                    if (DvRank.Count > 0 && DvSort.Count > 0)
                                                    {
                                                        int RankIndex = 0;
                                                        for (int intSort = 0; intSort < DvSort.Count; intSort++)
                                                        {
                                                            string SortApp_no = Convert.ToString(DvSort[intSort]["AppNo"]);
                                                            DtSortRank.DefaultView.RowFilter = "ST_AppNo='" + SortApp_no + "'";
                                                            DataView dsort = DtSortRank.DefaultView;
                                                            if (dsort.Count > 0)
                                                            {
                                                                int GetRank = Convert.ToInt32(DvRank[RankIndex]["ST_Rank"]);
                                                                RankIndex += 1;
                                                                string UpdateRankQuery = " if exists (select ST_AppNo from ST_RankTable where ST_RankCriteria='" + GetCriterai + "' and ST_AppNo='" + SortApp_no + "' and ST_stream='" + ddlStream.SelectedValue + "')  update  ST_RankTable set ST_Rank ='" + GetRank + "' where ST_RankCriteria='" + GetCriterai + "' and ST_AppNo='" + SortApp_no + "'  and ST_stream='" + ddlStream.SelectedValue + "' ";
                                                                d2.update_method_wo_parameter(UpdateRankQuery, "Text");
                                                            }
                                                            string InsQuery = " if exists (select ST_appNo from ST_SecurityMark where ST_AppNo='" + SortApp_no + "' and streamCode='" + ddlStream.SelectedValue + "') update ST_SecurityMark set CombinedScore='" + SameMark + "',MathsMark='" + Convert.ToString(DvSort[intSort]["Maths"]) + "',PhysicsMark='" + Convert.ToString(DvSort[intSort]["Physics"]) + "',ChemistryMark='" + Convert.ToString(DvSort[intSort]["Chemistery"]) + "',Dob='" + Convert.ToString(DvSort[intSort]["dob"]) + "',ApplicationNo='" + Convert.ToString(DvSort[intSort]["ApplicationNo"]) + "' where ST_AppNo='" + Convert.ToString(DvSort[intSort]["AppNo"]) + "'  and streamCode='" + ddlStream.SelectedValue + "' else  insert into  ST_SecurityMark (ST_AppNo,CombinedScore,MathsMark,PhysicsMark,ChemistryMark,Dob,ApplicationNo,streamCode) values ('" + Convert.ToString(DvSort[intSort]["AppNo"]) + "','" + SameMark + "','" + Convert.ToString(DvSort[intSort]["Maths"]) + "','" + Convert.ToString(DvSort[intSort]["Physics"]) + "','" + Convert.ToString(DvSort[intSort]["Chemistery"]) + "','" + Convert.ToString(DvSort[intSort]["dob"]) + "','" + Convert.ToString(DvSort[intSort]["ApplicationNo"]) + "','" + ddlStream.SelectedValue + "')";
                                                            d2.update_method_wo_parameter(InsQuery, "Text");
                                                        }
                                                    }
                                                }
                                            }
                                            MarkData.Rows.Clear();
                                        }
                                    }
                                }
                            }
                        }
                    }
                    if (CheckSave == true)
                        ScriptManager.RegisterStartupScript(this, typeof(Page), UniqueID, "alert('Generated Successfully')", true);
                    else
                        ScriptManager.RegisterStartupScript(this, typeof(Page), UniqueID, "alert('Not Saved')", true);
                }

            }
        }
        catch
        {

        }
    }

    protected void btnShowDetails_Click(object sender, EventArgs e)
    {
        if (ddlStream.SelectedIndex == 0)
        {
            showStreamI();
        }
        else
        {
            showStreamII();
        }
    }

    public void ExportToWord(DataTable dt)
    {

        GridView GridView1 = new GridView();
        GridView1.AllowPaging = false;
        GridView1.DataSource = dt;
        GridView1.DataBind();

        Response.Clear();

        Response.Buffer = true;

        Response.AddHeader("content-disposition",

         "attachment;filename=DataTable.xls");

        Response.Charset = "";

        Response.ContentType = "application/vnd.ms-excel";

        StringWriter sw = new StringWriter();

        HtmlTextWriter hw = new HtmlTextWriter(sw);
        for (int i = 0; i < GridView1.Rows.Count; i++)
        {

            GridView1.Rows[i].Attributes.Add("class", "textmode");

        }

        GridView1.RenderControl(hw);

        string style = @"<style> .textmode { mso-number-format:\@; } </style>";

        Response.Write(style);

        Response.Output.Write(sw.ToString());

        Response.Flush();

        Response.End();

    }
    //TextVal code creation
    public string getTextCodeOrInsert(string textCriteria, string textName, string collegeCode)
    {
        string textCode = string.Empty;
        textName = textName.Trim();
        textCriteria = textCriteria.Trim();
        try
        {
            string select_subno = "select TextCode from textvaltable where TextCriteria='" + textCriteria + "' and college_code ='" + Convert.ToString(collegeCode).Trim() + "' and TextVal='" + textName + "'";
            DataSet ds = new DataSet();
            ds = d2.select_method_wo_parameter(select_subno, "Text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                textCode = Convert.ToString(ds.Tables[0].Rows[0]["TextCode"]).Trim();
            }
            else
            {
                string insertquery = "insert into textvaltable(TextCriteria,TextVal,college_code) values('" + textCriteria + "','" + textName + "','" + Convert.ToString(collegeCode).Trim() + "')";
                int result = d2.update_method_wo_parameter(insertquery, "Text");
                if (result != 0)
                {
                    string select_subno1 = "select TextCode from textvaltable where TextCriteria='" + textCriteria + "' and college_code =" + Convert.ToString(collegeCode).Trim() + " and TextVal='" + textName + "'";
                    ds.Clear();
                    ds = d2.select_method_wo_parameter(select_subno1, "Text");
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        textCode = Convert.ToString(ds.Tables[0].Rows[0]["TextCode"]).Trim();
                    }
                }
            }
        }
        catch
        {
        }
        return textCode;
    }
    //Stream I
    private void showStreamI()
    {
        try
        {
            DataTable dtdata = new DataTable();
            dtdata.Columns.Add("S.No", typeof(string));
            dtdata.Columns.Add("Application No", typeof(string));
            dtdata.Columns.Add("Student Name", typeof(string));
            dtdata.Columns.Add("HSC Mark", typeof(float));
            dtdata.Columns.Add("JEE Mark", typeof(float));
            dtdata.Columns.Add("Combined Score", typeof(float));
            DataRow dr;
            DateTime t1 = DateTime.Now;
            DataTable DirectTable = new DataTable();
            StringBuilder SpOrder = new StringBuilder();
            ShwoDiv.InnerHtml = string.Empty;
            DataSet dsReport = Dir.selectDataSet("select MasterValue,MasterCode from CO_MasterValues where MasterCriteria='StudRankCriteria' and collegeCode =" + ddlCollege.SelectedValue + " order by MasterCode ; select app_formno,stud_Name,HSCMarkSec,jeeMarkSec,CombinedScore,a.app_no from applyn a,ST_Student_Mark_Detail st where a.app_no=st.st_appno and a.batch_year='" + ddlbatch.SelectedItem.Text + "' and a.college_code='" + ddlCollege.SelectedValue + "' and a.courseID='" + ddlcourse.SelectedValue + "' order by CombinedScore desc ; select a.app_no,ST_RankCriteria,ST_Rank from applyn a,ST_RankTable SR where a.app_no=SR.ST_AppNo and a.college_code ='" + ddlCollege.SelectedValue + "' and a.batch_year='" + ddlbatch.SelectedItem.Text + "' and a.courseID='" + ddlcourse.SelectedValue + "' order by ST_Rank,ST_RankCriteria ; select distinct Convert(nvarchar(500),'['+ Convert(nvarchar(500),ST_RankCriteria)+']') as Criteria,ST_RankCriteria from ST_RankTable SR,Applyn A where SR.ST_AppNo=a.app_no and a.college_code='" + ddlCollege.SelectedValue + "' and a.courseId='" + ddlcourse.SelectedValue + "' and a.batch_year='" + ddlbatch.SelectedItem.Text + "' and SR.ST_stream='" + ddlStream.SelectedValue + "' order by ST_RankCriteria ");
            if (dsReport.Tables.Count > 3)
            {
                if (dsReport.Tables[3].Rows.Count > 0)
                {
                    List<string> STCRi = dsReport.Tables[3].AsEnumerable().Select(r => r.Field<string>("Criteria")).ToList<string>();
                    string SbCriteria = string.Join(",", STCRi);
                    string PivotQuery = " SELECT * FROM (SELECT ST_AppNo,ST_RankCriteria,ST_Rank FROM ST_RankTable  where ST_stream='" + ddlStream.SelectedValue + "') up  PIVOT (sum(ST_Rank) FOR ST_RankCriteria IN (" + SbCriteria + ")) AS pvt ORDER BY ST_AppNo";
                    DirectTable = Dir.selectDataTable(PivotQuery);
                }

                StringBuilder SbShowTable = new StringBuilder();
                SbShowTable.Append("<table cellspacing=0 cellpadding=5 border=1 rules='all' style='border:1px solid black; border-radius:5px; text-align:center;'><tr style='background-color:#0CA6CA;font-weight:bold;font-size:16px;'><td>S.No</td><td>Application No</td><td>Student Name</td><td>HSc Mark</td><td>JEE Mark</td><td>Combined Score</td>");

                for (byte intCr = 0; intCr < dsReport.Tables[0].Rows.Count; intCr++)
                {
                    SbShowTable.Append("<td>" + Convert.ToString(dsReport.Tables[0].Rows[intCr]["MasterValue"]) + "</td>");
                    dtdata.Columns.Add(Convert.ToString(dsReport.Tables[0].Rows[intCr]["MasterValue"]), typeof(decimal));
                    if (intCr == 0)
                    {
                        SpOrder.Append(" " + Convert.ToString(dsReport.Tables[0].Rows[intCr]["MasterValue"]) + " asc,");
                    }
                }
                if (SpOrder.Length > 0)
                {
                    SpOrder.Remove(SpOrder.Length - 1, 1);
                }
                SbShowTable.Append("</tr>");
                if (dsReport.Tables[1].Rows.Count > 0)
                {
                    DataTable dtRankTable = dsReport.Tables[1].DefaultView.ToTable();
                    int sNo = 1;
                    foreach (DataRow drRank in dtRankTable.Rows)
                    {
                        SbShowTable.Append("<tr><td>" + sNo + "</td><td style='text-align:left;'>" + drRank["app_formno"] + "</td><td style='text-align:left;'>" + drRank["stud_Name"] + "</td><td>" + drRank["HSCMarkSec"] + "</td><td>" + drRank["jeeMarkSec"] + "</td><td>" + drRank["CombinedScore"] + "</td>");

                        dr = dtdata.NewRow();
                        dr[0] = sNo;
                        dr[1] = drRank["app_formno"];
                        dr[2] = drRank["stud_Name"];
                        dr[3] = drRank["HSCMarkSec"];
                        dr[4] = drRank["jeeMarkSec"];
                        dr[5] = drRank["CombinedScore"];
                        int c = 5;
                        Int64 St_Appno = Convert.ToInt64(drRank["app_no"]);
                        DirectTable.DefaultView.RowFilter = "ST_AppNo=" + St_Appno + "";
                        DataTable dtTest = DirectTable.DefaultView.ToTable();
                        if (dtTest.Rows.Count > 0)
                        {
                            for (byte intDs = 1; intDs < dtTest.Columns.Count; intDs++)
                            {
                                c++;
                                SbShowTable.Append("<td>" + Convert.ToString(dtTest.Rows[0][intDs]) + "</td>");
                                // dr[c] = Convert.ToString(dtTest.Rows[0][intDs]);
                                decimal RankValue = 0;
                                decimal.TryParse(Convert.ToString(dtTest.Rows[0][intDs]), out RankValue);
                                dr[c] = RankValue;
                            }
                        }
                        sNo++;
                        dtdata.Rows.Add(dr);
                    }

                    SbShowTable.Append("</tr></table>");
                    ShwoDiv.InnerHtml = SbShowTable.ToString();
                    btnBasePrint.Visible = true;

                    DateTime t2 = DateTime.Now;
                    TimeSpan t = new TimeSpan();
                    t = t2.Subtract(t1);
                    lbltest.Text = t.Minutes + " Min : " + t.Seconds + " Sec";
                    DataView dv = dtdata.DefaultView;
                    dv.Sort = SpOrder.ToString();
                    if (dtdata.Rows.Count > 0)
                    {
                        ExportToWord(dtdata);
                    }
                }
            }
        }
        catch { }
    }
    //Stream II
    private void showStreamII()
    {
        try
        {
            DataTable dtdata = new DataTable();
            dtdata.Columns.Add("S.No", typeof(string));
            dtdata.Columns.Add("Application No", typeof(string));
            dtdata.Columns.Add("Student Name", typeof(string));
            dtdata.Columns.Add("HSC Mark", typeof(float));
            //dtdata.Columns.Add("JEE Mark", typeof(float));
            dtdata.Columns.Add("Normalized percentile", typeof(float));
            DataRow dr;
            DateTime t1 = DateTime.Now;
            DataTable DirectTable = new DataTable();
            StringBuilder SpOrder = new StringBuilder();
            ShwoDiv.InnerHtml = string.Empty;
            DataSet dsReport = Dir.selectDataSet("select MasterValue,MasterCode,MasterPriority from CO_MasterValues where MasterCriteria='StudRankCriteria' and collegeCode =" + ddlCollege.SelectedValue + " order by MasterCode,MasterPriority asc ; select app_formno,stud_Name,HSCMarkSec,jeeMarkSec,CombinedScoreSII,a.app_no from applyn a,ST_Student_Mark_Detail st where a.app_no=st.st_appno and a.batch_year='" + ddlbatch.SelectedItem.Text + "' and a.college_code='" + ddlCollege.SelectedValue + "' and a.courseID='" + ddlcourse.SelectedValue + "' order by CombinedScoreSII desc ; select a.app_no,ST_RankCriteria,ST_Rank from applyn a,ST_RankTable SR where a.app_no=SR.ST_AppNo and a.college_code ='" + ddlCollege.SelectedValue + "' and a.batch_year='" + ddlbatch.SelectedItem.Text + "' and a.courseID='" + ddlcourse.SelectedValue + "' order by ST_Rank,ST_RankCriteria ; select distinct Convert(nvarchar(500),'['+ Convert(nvarchar(500),ST_RankCriteria)+']') as Criteria,ST_RankCriteria from ST_RankTable SR,Applyn A where SR.ST_AppNo=a.app_no and a.college_code='" + ddlCollege.SelectedValue + "' and a.courseId='" + ddlcourse.SelectedValue + "' and a.batch_year='" + ddlbatch.SelectedItem.Text + "' and SR.ST_stream='" + ddlStream.SelectedValue + "' order by ST_RankCriteria ");
            if (dsReport.Tables.Count > 3)
            {
                if (dsReport.Tables[3].Rows.Count > 0)
                {
                    List<string> STCRi = dsReport.Tables[3].AsEnumerable().Select(r => r.Field<string>("Criteria")).ToList<string>();
                    string SbCriteria = string.Join(",", STCRi);
                    string PivotQuery = " SELECT * FROM (SELECT ST_AppNo,ST_RankCriteria,ST_Rank FROM ST_RankTable  where ST_stream='" + ddlStream.SelectedValue + "') up  PIVOT (sum(ST_Rank) FOR ST_RankCriteria IN (" + SbCriteria + ")) AS pvt ORDER BY ST_AppNo";
                    DirectTable = Dir.selectDataTable(PivotQuery);
                }

                StringBuilder SbShowTable = new StringBuilder();
                SbShowTable.Append("<table cellspacing=0 cellpadding=5 border=1 rules='all' style='border:1px solid black; border-radius:5px; text-align:center;'><tr style='background-color:#0CA6CA;font-weight:bold;font-size:16px;'><td>S.No</td><td>Application No</td><td>Student Name</td><td>HSc Mark</td><td>JEE Mark</td><td>Combined Score</td>");

                for (byte intCr = 0; intCr < dsReport.Tables[0].Rows.Count; intCr++)
                {
                    SbShowTable.Append("<td>" + Convert.ToString(dsReport.Tables[0].Rows[intCr]["MasterValue"]) + "</td>");
                    dtdata.Columns.Add(Convert.ToString(dsReport.Tables[0].Rows[intCr]["MasterValue"]), typeof(decimal));
                    if (intCr == 0)
                    {
                        SpOrder.Append(" " + Convert.ToString(dsReport.Tables[0].Rows[intCr]["MasterValue"]) + " asc,");
                    }
                }
                if (SpOrder.Length > 0)
                {
                    SpOrder.Remove(SpOrder.Length - 1, 1);
                }

                SbShowTable.Append("</tr>");
                if (dsReport.Tables[1].Rows.Count > 0)
                {
                    DataTable dtRankTable = dsReport.Tables[1].DefaultView.ToTable();
                    int sNo = 1;
                    foreach (DataRow drRank in dtRankTable.Rows)
                    {
                        SbShowTable.Append("<tr><td>" + sNo + "</td><td style='text-align:left;'>" + drRank["app_formno"] + "</td><td style='text-align:left;'>" + drRank["stud_Name"] + "</td><td>" + drRank["HSCMarkSec"] + "</td><td>" + drRank["jeeMarkSec"] + "</td><td>" + drRank["CombinedScoreSII"] + "</td>");

                        dr = dtdata.NewRow();
                        dr[0] = sNo;
                        dr[1] = drRank["app_formno"];
                        dr[2] = drRank["stud_Name"];
                        dr[3] = drRank["HSCMarkSec"];
                        // dr[4] = drRank["jeeMarkSec"];
                        dr[4] = drRank["CombinedScoreSII"];
                        int c = 4;
                        Int64 St_Appno = Convert.ToInt64(drRank["app_no"]);
                        DirectTable.DefaultView.RowFilter = "ST_AppNo=" + St_Appno + "";
                        DataTable dtTest = DirectTable.DefaultView.ToTable();
                        if (dtTest.Rows.Count > 0)
                        {
                            for (byte intDs = 1; intDs < dtTest.Columns.Count; intDs++)
                            {
                                c++;
                                SbShowTable.Append("<td>" + Convert.ToString(dtTest.Rows[0][intDs]) + "</td>");
                                decimal RankValue = 0;
                                decimal.TryParse(Convert.ToString(dtTest.Rows[0][intDs]), out RankValue);
                                dr[c] = RankValue;
                                //dr[c] = Convert.ToString(dtTest.Rows[0][intDs]);
                            }
                        }
                        sNo++;
                        dtdata.Rows.Add(dr);
                    }

                    SbShowTable.Append("</tr></table>");
                    ShwoDiv.InnerHtml = SbShowTable.ToString();
                    btnBasePrint.Visible = true;

                    DateTime t2 = DateTime.Now;
                    TimeSpan t = new TimeSpan();
                    t = t2.Subtract(t1);
                    lbltest.Text = t.Minutes + " Min : " + t.Seconds + " Sec";
                    DataView dv = dtdata.DefaultView;
                    dv.Sort = SpOrder.ToString();
                    if (dtdata.Rows.Count > 0)
                    {
                        ExportToWord(dtdata);
                    }
                }
            }
        }
        catch { }
    }
}
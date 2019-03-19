/*
 * 
 * Author : Mohamed Idhris Sheik Dawood
 * Date created : 23-05-2017
 * 
 * */

using System;
using System.Web.UI.WebControls;
using System.Data;
using System.Text;
using System.Collections.Generic;
using InsproDataAccess;
using System.Drawing;

public partial class DegreewiseSeatAllotment : System.Web.UI.Page
{
    DAccess2 d2 = new DAccess2();
    InsproDirectAccess dirAcc = new InsproDirectAccess();
    DataSet ds = new DataSet();
    string UserCode = string.Empty;
    string singleuser = string.Empty;
    string group_user = string.Empty;

    protected void Page_Init(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            //string uid = this.Page.Request.Params.Get("__EVENTTARGET");
            //if (uid != null && !uid.Contains("btnDaySlotSave"))
            //{
            if (Session["dtGrid"] != null)
            {
                Session.Remove("dtGrid");
            }
            //}
        }
        callGridBind();
    }
    public void callGridBind()
    {
        if (Session["dtGrid"] != null)
        {
            DataTable dtGrid = (DataTable)Session["dtGrid"];
            gridBranSeat.DataSource = dtGrid;
            gridBranSeat.DataBind();
        }
        else
        {
            gridBranSeat.DataSource = null;
            gridBranSeat.DataBind();
        }

    }
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
            if (ddlEduLev.Items.Count > 0)
            {
                ddlEduLev.Items.Add("All");
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
            if (ddlEduLev.SelectedItem.Text != "All")
            {
                ds = d2.select_method_wo_parameter("select distinct course_id,Course_Name from Course where college_code=" + ddlCollege.SelectedValue + " and edu_level='" + ddlEduLev.SelectedValue + "' order by course_id", "Text");
            }
            else
            {
                ds = d2.select_method_wo_parameter("select distinct course_id,Course_Name from Course where college_code=" + ddlCollege.SelectedValue + " order by course_id", "Text");
            }
            if (ds.Tables[0].Rows.Count > 0)
            {
                ddlcourse.DataSource = ds;
                ddlcourse.DataTextField = "Course_Name";
                ddlcourse.DataValueField = "course_id";
                ddlcourse.DataBind();

                cbl_Session.DataSource = ds;
                cbl_Session.DataTextField = "Course_Name";
                cbl_Session.DataValueField = "course_id";
                cbl_Session.DataBind();
            }
            CallCheckBoxChangedEvent(cbl_Session, cb_Session, txtSession, "Course");
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
            //ds.Clear();
            //ds = d2.select_method_wo_parameter("select d.Degree_Code,dt.dept_name from Degree d, Department dt,course c where dt.Dept_Code=d.Dept_Code and c.Course_Id=d.Course_Id and d.college_code='" + ddlCollege.SelectedValue + "' and c.Edu_Level='" + ddlEduLev.SelectedValue + "' and d.Course_Id='" + ddlcourse.SelectedValue + "' order by Dept_Name asc ", "Text");
            //if (ds.Tables[0].Rows.Count > 0)
            //{
            //    ddlStream.DataSource = ds;
            //    ddlStream.DataTextField = "dept_name";
            //    ddlStream.DataValueField = "Degree_Code";
            //    ddlStream.DataBind();
            //}

            //ddlStream.Items.Add("Stream I");
            //ddlStream.Items.Add("Stream II");

            string qry = string.Empty;
            qry = "select TextCode,TextVal from TextValTable tv where TextCriteria='ADMst' and college_code ='" + ddlCollege.SelectedValue + "' order by TextVal";
            ds = d2.select_method_wo_parameter(qry, "text");
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                ddlStream.DataSource = ds;
                ddlStream.DataTextField = "TextVal";
                ddlStream.DataValueField = "TextCode";
                ddlStream.DataBind();
                ddlStream.Enabled = true;
                ddlStream.SelectedIndex = 0;
            }
        }
        catch
        {

        }
    }

    private void BindStream()
    {
        try
        {
            string qry = string.Empty;
            qry = "select TextCode,TextVal from TextValTable tv where TextCriteria='ADMst' and college_code ='" + ddlCollege.SelectedValue + "' order by TextVal";
            ds = d2.select_method_wo_parameter(qry, "text");
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                ddlStream.DataSource = ds;
                ddlStream.DataTextField = "TextVal";
                ddlStream.DataValueField = "TextCode";
                ddlStream.DataBind();
                ddlStream.Enabled = true;
                ddlStream.SelectedIndex = 0;
            }
        }
        catch (Exception ex)
        {

        }
    }
    //Base screen controls events
    protected void ddlCollege_SelectedIndexChanged(object sender, EventArgs e)
    {
        bindBatch();
        bindEdulevel();
        bindCourse();
        bindStream();
        btnBaseGo_OnClick(sender, e);
        imgdiv2.Visible = false;
    }
    protected void ddlbatch_SelectedIndexChanged(object sender, EventArgs e)
    {
        bindEdulevel();
        bindCourse();
        bindStream();
        btnBaseGo_OnClick(sender, e);
        imgdiv2.Visible = false;
    }
    protected void ddlEdulevel_SelectedIndexChanged(object sender, EventArgs e)
    {
        bindCourse();
        bindStream();
        btnBaseGo_OnClick(sender, e);
        imgdiv2.Visible = false;
    }
    protected void ddlcourse_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        bindStream();
        btnBaseGo_OnClick(sender, e);
        imgdiv2.Visible = false;
    }
    protected void ddlbranch_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        bindStream();
        btnBaseGo_OnClick(sender, e);
        imgdiv2.Visible = false;
    }
    protected void ddlStream_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        btnBaseGo_OnClick(sender, e);
        imgdiv2.Visible = false;
    }
    //Base screen search
    protected void btnBaseGo_OnClick(object sender, EventArgs e)
    {
        loadSearch();
    }
    private void loadSearch()
    {
        try
        {
            gridBranSeat.Visible = false;
            gridBranSeat.DataSource = null;
            gridBranSeat.DataBind();

            btnDaySlotSave.Visible = false;
            btnBasePrint.Visible = false;

            string collegeCode = Convert.ToString(ddlCollege.SelectedValue);
            string batchYear = Convert.ToString(ddlbatch.SelectedValue);
            string eduLevel = Convert.ToString(ddlEduLev.SelectedValue);
            string courseCode = Convert.ToString(ddlcourse.SelectedValue);
            string categName = Convert.ToString(ddlStream.SelectedItem.Text).Trim();
            string categCode = getTextCodeOrInsert("ADMst", categName, collegeCode);
            StringBuilder sbSlot = new StringBuilder();
            for (int slI = 0; slI < cbl_Session.Items.Count; slI++)
            {
                if (cbl_Session.Items[slI].Selected)
                {
                    sbSlot.Append(cbl_Session.Items[slI].Value + ",");
                }
            }
            if (sbSlot.Length > 1)
            {
                sbSlot.Remove(sbSlot.Length - 1, 1);
            }

            //and c.Edu_Level='" + ddlEduLev.SelectedValue + "'
            DataSet dsBran = d2.select_method_wo_parameter("select d.Degree_Code,dt.dept_name,isnull(d.No_Of_seats,0) as NoOfSeats  from Degree d, Department dt,course c where dt.Dept_Code=d.Dept_Code and c.Course_Id=d.Course_Id and d.college_code='" + ddlCollege.SelectedValue + "'  and d.Course_Id in (" + sbSlot.ToString() + ") order by d.Course_Id, Dept_Name asc ", "Text");

            DataSet dsStudRankCrit = d2.select_method_wo_parameter("select MasterCode,MasterValue from CO_MasterValues where MasterCriteria='StudRankCriteria' and collegeCode ='" + collegeCode + "'", "Text");

            DataTable dtBranSeat = new DataTable();
            dtBranSeat.Columns.Add("S.No");
            dtBranSeat.Columns.Add("Branch");
            // dtBranSeat.Columns.Add("Max Seat");
            dtBranSeat.Columns.Add("Str I & II");
            dtBranSeat.Columns.Add("" + ddlStream.SelectedItem.Text + "");

            for (int i = 0; i < dsStudRankCrit.Tables[0].Rows.Count; i++)
            {
                string criteriaVal = Convert.ToString(dsStudRankCrit.Tables[0].Rows[i]["MasterValue"]) + "#" + Convert.ToString(dsStudRankCrit.Tables[0].Rows[i]["MasterCode"]);
                dtBranSeat.Columns.Add(criteriaVal);
            }

            dtBranSeat.Columns.Add("Total");

            DataTable dtPrevSaved = dirAcc.selectDataTable("SELECT Tot_Seat,Quota,Degree_Code,NoOfSeats,allotedSeats FROM seattype_cat WHERE  Batch_Year='" + batchYear + "' AND collegeCode='" + collegeCode + "' AND Category_Code='" + categCode + "'");
            DataTable dtPrevSavedAll = dirAcc.selectDataTable("SELECT Tot_Seat,Quota,Degree_Code,NoOfSeats,allotedSeats FROM seattype_cat WHERE  Batch_Year='" + batchYear + "' AND collegeCode='" + collegeCode + "' ");

            if (dsBran.Tables.Count > 0 && dsBran.Tables[0].Rows.Count > 0)
            {
                for (int i = 0; i < dsBran.Tables[0].Rows.Count; i++)
                {
                    string degCode = Convert.ToString(dsBran.Tables[0].Rows[i]["Degree_Code"]);

                    DataRow dr = dtBranSeat.NewRow();
                    dr["S.No"] = i + 1;
                    dr["Branch"] = Convert.ToString(dsBran.Tables[0].Rows[i]["dept_name"]) + "#" + degCode;
                    //dr["Max Seat"] = Convert.ToString(dsBran.Tables[0].Rows[i]["NoOfSeats"]);
                    string allotedVal = "0";
                    dtPrevSavedAll.DefaultView.RowFilter = " Degree_Code='" + degCode + "' ";
                    DataTable dtCurSum = dtPrevSavedAll.DefaultView.ToTable();

                    if (dtCurSum.Rows.Count > 0)
                    {
                        var obj = dtCurSum.Compute("SUM(Tot_Seat)", string.Empty);
                        allotedVal = obj.ToString();
                    }

                    dr["Str I & II"] = allotedVal;

                    if (dtPrevSaved.Rows.Count > 0)
                    {
                        int allotTotal = 0;
                        int totalSeats = 0;
                        for (int colI = 4; colI < (dtBranSeat.Columns.Count - 1); colI++)
                        {
                            string[] curColName = Convert.ToString(dtBranSeat.Columns[colI].ColumnName).Split('#');

                            dtPrevSaved.DefaultView.RowFilter = "Quota ='" + curColName[1] + "' and Degree_Code='" + degCode + "'";
                            DataTable dtCurCrit = dtPrevSaved.DefaultView.ToTable();
                            if (dtCurCrit.Rows.Count > 0)
                            {
                                string totalVal = Convert.ToString(dtCurCrit.Rows[0]["NoOfSeats"]).Trim();
                                int.TryParse(totalVal, out allotTotal);

                                string critVal = Convert.ToString(dtCurCrit.Rows[0]["Tot_Seat"]).Trim();
                                int critIntVal = 0; int.TryParse(critVal, out critIntVal);
                                totalSeats += critIntVal;
                                dr[Convert.ToString(dtBranSeat.Columns[colI].ColumnName)] = critVal;
                            }

                            //Total Alloted
                        }

                        dr["" + ddlStream.SelectedItem.Text + ""] = allotTotal;
                        dr["Total"] = totalSeats;
                    }
                    dtBranSeat.Rows.Add(dr);
                }

                Session["dtGrid"] = dtBranSeat;

                gridBranSeat.Visible = true;
                gridBranSeat.DataSource = dtBranSeat;
                gridBranSeat.DataBind();

                btnDaySlotSave.Visible = true;
                btnBasePrint.Visible = true;
            }
            else
            {
                lbl_alert.Text = "No rank criteria available";
                imgdiv2.Visible = true;
            }
        }
        catch
        {
            lbl_alert.Text = "Please check inputs";
            imgdiv2.Visible = true;
        }
    }
    protected void gridBranSeat_DataBound(object sender, EventArgs e)
    {
        try
        {
            foreach (GridViewRow gRow in gridBranSeat.Rows)
            {
                for (int colI = 3; colI < (gRow.Cells.Count - 1); colI++)
                {
                    switch (colI)
                    {
                        case 3:
                            TextBox txttot = (TextBox)gRow.FindControl("txt_val_" + gRow.RowIndex + "_" + colI);
                            StringBuilder sbScriptTot = new StringBuilder();
                            sbScriptTot.Append("var allocatedVal = document.getElementById('MainContent_gridBranSeat_hdn_degtot_" + gRow.RowIndex + "_2_" + gRow.RowIndex + "').innerHTML; allocatedVal = parseInt(allocatedVal);var allocatedSubVal = document.getElementById('MainContent_gridBranSeat_hdn_alottot_" + gRow.RowIndex + "_3_" + gRow.RowIndex + "').innerHTML; allocatedSubVal = parseInt(allocatedSubVal); var total = document.getElementById('MainContent_gridBranSeat_txt_val_" + gRow.RowIndex + "_4_" + gRow.RowIndex + "').value; if(total!=''){ if((allocatedVal-allocatedSubVal)<(parseInt(total))){document.getElementById('MainContent_gridBranSeat_txt_val_" + gRow.RowIndex + "_4_" + gRow.RowIndex + "').value='';}}");
                            for (int inCI = 4; inCI < (gRow.Cells.Count); inCI++)
                            {
                                sbScriptTot.Append("document.getElementById('MainContent_gridBranSeat_txt_val_" + gRow.RowIndex + "_" + inCI + "_" + gRow.RowIndex + "').value='';");
                            }
                            txttot.Attributes.Add("onchange", sbScriptTot.ToString());
                            break;
                        default:
                            if (colI != (gRow.Cells.Count - 1))
                            {
                                TextBox txt = (TextBox)gRow.FindControl("txt_val_" + gRow.RowIndex + "_" + (gRow.Cells.Count - 1));
                                TextBox txtcur = (TextBox)gRow.FindControl("txt_val_" + gRow.RowIndex + "_" + colI);

                                StringBuilder sbScript = new StringBuilder();
                                sbScript.Append("var allot = document.getElementById('MainContent_gridBranSeat_txt_val_" + gRow.RowIndex + "_3_" + gRow.RowIndex + "').value; var allotVal =0; if(allot=='' || allot=='0') {document.getElementById('MainContent_gridBranSeat_txt_val_" + gRow.RowIndex + "_3_" + gRow.RowIndex + "').value='0';}else{allotVal = parseInt(allot);} var fnlCnt = 0;");
                                for (int inCI = 4; inCI < (gRow.Cells.Count - 1); inCI++)
                                {
                                    sbScript.Append("var val" + gRow.RowIndex + "_" + inCI + " = document.getElementById('MainContent_gridBranSeat_txt_val_" + gRow.RowIndex + "_" + inCI + "_" + gRow.RowIndex + "').value; if(val" + gRow.RowIndex + "_" + inCI + "=='' || allot=='' || allot=='0'){ document.getElementById('MainContent_gridBranSeat_txt_val_" + gRow.RowIndex + "_" + inCI + "_" + gRow.RowIndex + "').value='0';val" + gRow.RowIndex + "_" + inCI + "=0;} if(allotVal<parseInt(val" + gRow.RowIndex + "_" + inCI + ") || allotVal<(fnlCnt+parseInt(val" + gRow.RowIndex + "_" + inCI + "))){document.getElementById('MainContent_gridBranSeat_txt_val_" + gRow.RowIndex + "_" + inCI + "_" + gRow.RowIndex + "').value='0';val" + gRow.RowIndex + "_" + inCI + "=0;} fnlCnt+=parseInt(val" + gRow.RowIndex + "_" + inCI + ");");
                                }
                                sbScript.Append("document.getElementById('MainContent_gridBranSeat_txt_val_" + gRow.RowIndex + "_" + (gRow.Cells.Count - 1) + "_" + gRow.RowIndex + "').value = fnlCnt.toString();");
                                txtcur.Attributes.Add("onchange", sbScript.ToString());
                            }
                            break;
                    }
                }
            }
        }
        catch { }
    }
    protected void gridBranSeat_OnRowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {
            if (e.Row.RowType == DataControlRowType.Header)
            {
                for (int colI = 4; colI < e.Row.Cells.Count; colI++)
                {
                    string[] curVal = e.Row.Cells[colI].Text.Split('#');

                    TextBox hdn = new TextBox();
                    hdn.Visible = false;
                    hdn.ID = "hdn_hdr_" + colI;
                    hdn.Text = curVal[1];
                    e.Row.Cells[colI].Controls.Add(hdn);

                    Label lblCrit = new Label();
                    lblCrit.ID = "lbl_crit_" + e.Row.RowIndex + "_" + colI;
                    lblCrit.Text = curVal[0];
                    e.Row.Cells[colI].Controls.Add(lblCrit);
                }
            }
            else if (e.Row.RowType == DataControlRowType.DataRow)
            {
                for (int colI = 0; colI < e.Row.Cells.Count; colI++)
                {
                    string[] curVal = e.Row.Cells[colI].Text.Replace("&nbsp;", string.Empty).Split('#');

                    switch (colI)
                    {
                        case 0:
                            e.Row.Cells[colI].HorizontalAlign = HorizontalAlign.Center;
                            break;
                        case 1:
                            TextBox hdn = new TextBox();
                            hdn.Visible = false;
                            hdn.ID = "hdn_degcode_" + e.Row.RowIndex + "_" + colI;
                            hdn.Text = curVal[1];
                            e.Row.Cells[1].Controls.Add(hdn);

                            Label lblBran = new Label();
                            lblBran.ID = "lbl_degcode_" + e.Row.RowIndex + "_" + colI;
                            lblBran.Text = curVal[0];
                            e.Row.Cells[colI].Controls.Add(lblBran);
                            break;
                        case 2:
                            Label hdnTot = new Label();
                            hdnTot.ID = "hdn_degtot_" + e.Row.RowIndex + "_" + colI;
                            hdnTot.Text = e.Row.Cells[colI].Text;
                            e.Row.Cells[colI].Controls.Add(hdnTot);

                            e.Row.Cells[colI].Font.Bold = true;
                            e.Row.Cells[colI].Font.Size = 12;
                            e.Row.Cells[colI].HorizontalAlign = HorizontalAlign.Center;
                            e.Row.Cells[colI].BackColor = ColorTranslator.FromHtml("#FE6598");

                            break;
                        case 3:
                            TextBox txt = new TextBox();
                            txt.ID = "txt_val_" + e.Row.RowIndex + "_" + colI;
                            txt.Text = curVal[0].Trim();
                            txt.Width = 60;
                            txt.MaxLength = 4;
                            txt.Attributes.Add("style", "text-align:right;font-size:14px; font-weight:bold;");
                            if (colI == (e.Row.Cells.Count - 1))
                            {
                                txt.Attributes.Add("style", "text-align:right;font-size:14px; font-weight:bold;background-color:#3F9FFF;");
                                txt.Attributes.Add("readonly", "readonly");
                            }
                            else if (colI == 3)
                            {
                                txt.Attributes.Add("style", "text-align:right;font-size:14px; font-weight:bold;background-color:pink;");
                            }

                            AjaxControlToolkit.FilteredTextBoxExtender aftet = new AjaxControlToolkit.FilteredTextBoxExtender();
                            aftet.FilterType = AjaxControlToolkit.FilterTypes.Custom;
                            aftet.ValidChars = "0123456789";
                            aftet.TargetControlID = txt.ID;
                            e.Row.Cells[colI].Controls.Add(aftet);
                            e.Row.Cells[colI].Controls.Add(txt);
                            break;
                        default:
                            txt = new TextBox();
                            txt.ID = "txt_val_" + e.Row.RowIndex + "_" + colI;
                            txt.Text = curVal[0].Trim();
                            txt.Width = 50;
                            txt.MaxLength = 4;
                            txt.Attributes.Add("style", "text-align:right;font-size:14px; font-weight:bold;");
                            if (colI == (e.Row.Cells.Count - 1))
                            {
                                txt.Attributes.Add("style", "text-align:right;font-size:14px; font-weight:bold;background-color:#3F9FFF;");
                                txt.Attributes.Add("readonly", "readonly");
                            }
                            else if (colI == 3)
                            {
                                txt.Attributes.Add("style", "text-align:right;font-size:14px; font-weight:bold;background-color:pink;");
                            }

                            AjaxControlToolkit.FilteredTextBoxExtender afte = new AjaxControlToolkit.FilteredTextBoxExtender();
                            afte.FilterType = AjaxControlToolkit.FilterTypes.Custom;
                            afte.ValidChars = "0123456789";
                            afte.TargetControlID = txt.ID;
                            e.Row.Cells[colI].Controls.Add(afte);
                            e.Row.Cells[colI].Controls.Add(txt);
                            break;
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
            string courseCode = Convert.ToString(ddlcourse.SelectedValue);
            string categName = Convert.ToString(ddlStream.SelectedItem.Text).Trim();
            string categCode = getTextCodeOrInsert("ADMst", categName, collegeCode);

            Dictionary<byte, string> dicCrit = new Dictionary<byte, string>();
            for (byte colI = 4; colI < (gridBranSeat.HeaderRow.Cells.Count - 1); colI++)
            {
                TextBox hdnCrit = (TextBox)gridBranSeat.HeaderRow.FindControl("hdn_hdr_" + colI);
                dicCrit.Add(colI, hdnCrit.Text);
            }

            foreach (GridViewRow gRow in gridBranSeat.Rows)
            {
                TextBox hdnBranch = (TextBox)gRow.FindControl("hdn_degcode_" + gRow.RowIndex + "_1");
                TextBox txtAllot = (TextBox)gRow.FindControl("txt_val_" + gRow.RowIndex + "_3");
                TextBox txtTot = (TextBox)gRow.FindControl("txt_val_" + gRow.RowIndex + "_" + (gRow.Cells.Count - 1));

                string degCode = hdnBranch.Text;
                string allotVal = txtAllot.Text.Trim();
                string totalVal = txtTot.Text.Trim();

                int allotIntVal = 0; int.TryParse(allotVal, out allotIntVal);
                int totalIntVal = 0; int.TryParse(totalVal, out totalIntVal);

                for (byte colI = 4; colI < (gRow.Cells.Count - 1); colI++)
                {
                    TextBox txtCritVal = (TextBox)gRow.FindControl("txt_val_" + gRow.RowIndex + "_" + colI);
                    string critVal = txtCritVal.Text.Trim();
                    int critIntVal = 0;
                    int.TryParse(critVal, out critIntVal);
                    string critCode = dicCrit[colI];

                    //Update Individual seat for criteria allotedSeats='0',
                    string insUpdQ = "IF EXISTS (SELECT Tot_Seat FROM seattype_cat WHERE Category_Code='" + categCode + "' AND Degree_Code='" + degCode + "' AND Batch_Year='" + batchYear + "' AND Quota='" + critCode + "' AND Category_Name='" + categName + "' AND collegeCode='" + collegeCode + "') UPDATE seattype_cat SET Tot_Seat='" + critIntVal + "',NoOfSeats='" + allotIntVal + "'  WHERE Category_Code='" + categCode + "' AND Degree_Code='" + degCode + "' AND Batch_Year='" + batchYear + "' AND Quota='" + critCode + "' AND Category_Name='" + categName + "' AND collegeCode='" + collegeCode + "' ELSE INSERT INTO seattype_cat (Tot_Seat, Category_Code, Degree_Code, Batch_Year, Quota, Category_Name, collegeCode,NoOfSeats) VALUES ('" + critIntVal + "', '" + categCode + "', '" + degCode + "', '" + batchYear + "', '" + critCode + "', '" + categName + "', '" + collegeCode + "','" + allotIntVal + "')";
                    d2.update_method_wo_parameter(insUpdQ, "Text");
                }
            }
            loadSearch();
            lbl_alert.Text = "Saved successfully";
            imgdiv2.Visible = true;
        }
        catch
        {
            lbl_alert.Text = "Please try later";
            imgdiv2.Visible = true;
        }

    }
    //Alert Close
    protected void btn_errorclose_Click(object sender, EventArgs e)
    {
        imgdiv2.Visible = false;
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
            ds.Clear();
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

    protected void cb_Session_OnCheckedChanged(object sender, EventArgs e)
    {
        CallCheckBoxChangedEvent(cbl_Session, cb_Session, txtSession, "Course");
        btnBaseGo_OnClick(sender, e);
        imgdiv2.Visible = false;
    }
    protected void cbl_Session_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        CallCheckBoxListChangedEvent(cbl_Session, cb_Session, txtSession, "Course");
        btnBaseGo_OnClick(sender, e);
        imgdiv2.Visible = false;
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
}
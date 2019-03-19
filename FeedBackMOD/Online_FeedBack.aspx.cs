using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Collections;
using System.Configuration;
using System.Drawing;
public partial class Online_FeedBack : System.Web.UI.Page
{
    string collegecode1 = string.Empty;
    DAccess2 d2 = new DAccess2();
    DataSet ds = new DataSet();
    Hashtable hat = new Hashtable();
    string usercode = string.Empty;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["collegecode"] == null)
        {
            Response.Redirect("~/Default.aspx");
        }
        usercode = Session["usercode"].ToString();
        collegecode1 = Session["collegecode"].ToString();
        if (!IsPostBack)
        {
            Session["Optional subject"] = null;
            questiondiv.Visible = false;
            FpSpread1.SaveChanges();
        }
    }
    protected void btn_Submit_Click(object sender, EventArgs e)
    {
        lbl_loginalrt.Visible = false;
        ds.Clear();
        if (!string.IsNullOrEmpty(txt_unic_fb.Text))
        {
            ds = d2.select_method_wo_parameter("select  FeedbackUnicode,IsCheckFlag,FeedBackMasterfK,F.FeedbackName from  CO_FeedbackUniCode FU,CO_FeedbackMaster F where f.FeedBackMasterPK=FU.FeedBackMasterFK and FeedbackUnicode= '" + txt_unic_fb.Text + "'", "Text");
            if (ds.Tables != null)
            {
                if (ds.Tables[0].Rows.Count > 0)
                {
                    string uniq_code = Convert.ToString(ds.Tables[0].Rows[0]["IsCheckFlag"]);
                    string feedbackfk = Convert.ToString(ds.Tables[0].Rows[0]["FeedBackMasterfK"]);
                    lbl_Name.Text = Convert.ToString(ds.Tables[0].Rows[0]["FeedbackName"]);
                    Label2.Text = feedbackfk.ToString();
                    if (uniq_code.ToLower() == "false" || uniq_code == "0")
                    {
                        div1.Visible = false;
                        questiondiv.Visible = true;
                    }
                    else
                    {
                        lbl_loginalrt.Visible = true;
                        lbl_loginalrt.Text = "This User Id Already Writtend Exam";
                    }
                }
                else
                {
                    lbl_loginalrt.Visible = true;
                    lbl_loginalrt.Text = "Please Enter Valid ID";
                }
            }
            else
            {
                lbl_loginalrt.Visible = true;
                lbl_loginalrt.Text = "Please Enter Valid ID";
            }
        }
        else
        {
            lbl_loginalrt.Visible = true;
            lbl_loginalrt.Text = "Please Enter Login ID";
            return;
        }
    }
    protected void btn_errorclose_Click(object sender, EventArgs e)
    {
        imgdiv2.Visible = false;
    }
    public void online(string fbfk)
    {
        try
        {
            lbl_alrt.Visible = false;
            FpSpread1.Sheets[0].RowCount = 0;
            FpSpread1.Sheets[0].ColumnCount = 0;
            FpSpread1.CommandBar.Visible = false;
            FpSpread1.Sheets[0].AutoPostBack = false;
            FpSpread1.Sheets[0].ColumnHeader.RowCount = 2;
            FpSpread1.Sheets[0].RowHeader.Visible = false;
            FpSpread1.Sheets[0].ColumnCount = 3;
            FarPoint.Web.Spread.StyleInfo darkstyle = new FarPoint.Web.Spread.StyleInfo();
            darkstyle.BackColor = ColorTranslator.FromHtml("#0CA6CA");
            darkstyle.ForeColor = Color.White;
            FpSpread1.ActiveSheetView.ColumnHeader.DefaultStyle = darkstyle;
            FpSpread1.Visible = true;
            FarPoint.Web.Spread.ComboBoxCellType cmb = new FarPoint.Web.Spread.ComboBoxCellType();
            cmb.AutoPostBack = false;
            FarPoint.Web.Spread.CheckBoxCellType cball = new FarPoint.Web.Spread.CheckBoxCellType();
            cball.AutoPostBack = true;
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 0].Text = "S.No";
            //FpSpread1.Sheets[0].ColumnHeader.Cells[0, 0].Font.Bold = true;
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 0].HorizontalAlign = HorizontalAlign.Center;
            //FpSpread1.Sheets[0].ColumnHeader.Cells[0, 0].Font.Name = "Book Antiqua";
            //FpSpread1.Sheets[0].ColumnHeader.Cells[0, 0].Font.Size = FontUnit.Medium;
            FpSpread1.Sheets[0].Columns[0].HorizontalAlign = HorizontalAlign.Center;
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 1].Text = "Header";
            //FpSpread1.Sheets[0].ColumnHeader.Cells[0, 1].Font.Bold = true;
            //FpSpread1.Sheets[0].ColumnHeader.Cells[0, 1].Font.Name = "Book Antiqua";
            //FpSpread1.Sheets[0].ColumnHeader.Cells[0, 1].Font.Size = FontUnit.Medium;
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 1].HorizontalAlign = HorizontalAlign.Center;
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 2].Text = "Questions";
            //FpSpread1.Sheets[0].ColumnHeader.Cells[0, 2].Font.Bold = true;
            //FpSpread1.Sheets[0].ColumnHeader.Cells[0, 2].Font.Name = "Book Antiqua";
            //FpSpread1.Sheets[0].ColumnHeader.Cells[0, 2].Font.Size = FontUnit.Medium;
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 2].HorizontalAlign = HorizontalAlign.Center;
            FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, 0, 2, 1);
            FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, 1, 2, 1);
            FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, 2, 2, 1);
            FpSpread1.Sheets[0].ColumnHeader.Columns[0].Width = 52;
            FpSpread1.Sheets[0].ColumnHeader.Columns[1].Width = 200;
            FpSpread1.Sheets[0].ColumnHeader.Columns[2].Width = 350;
            DataView dv = new DataView(); DataView filteroption_dv = new DataView();
            ds.Clear();
            string selqury = "select Batch_Year,DegreeCode,Section,semester,CollegeCode,Subject_Type,OptionalSubject_type,isnull(Acadamic_Isgeneral,0)Acadamic_Isgeneral,isnull(IsType_Individual,0)IsType_Individual,InclueCommon,isnull(IsSubjectType,0)IsSubjectType  from CO_FeedBackMaster where FeedBackMasterPK='" + fbfk + "'";
            ds = d2.select_method_wo_parameter(selqury, "Text");
            if (ds.Tables.Count > 0)
            {
                if (ds.Tables[0].Rows.Count > 0)
                {
                    ViewState["Acadamic_Isgeneral"] = null;
                    string Acadamic_Isgeneral = Convert.ToString(ds.Tables[0].Rows[0]["Acadamic_Isgeneral"]);
                    string Optiontypefilter = Convert.ToString(ds.Tables[0].Rows[0]["IsType_Individual"]);
                    string IsSubjectType = Convert.ToString(ds.Tables[0].Rows[0]["IsSubjectType"]);
                    if (IsSubjectType.Trim() == "1" || IsSubjectType.Trim() == "True")
                        lockNote.Visible = true;
                    else
                        lockNote.Visible = false;
                    if (Acadamic_Isgeneral.Trim() == "0" || Acadamic_Isgeneral.Trim() == "False")
                    {
                        #region acadamic
                        string Deg_code = ds.Tables[0].Rows[0]["DegreeCode"].ToString();
                        string Batch = ds.Tables[0].Rows[0]["Batch_Year"].ToString();
                        string Semester = ds.Tables[0].Rows[0]["semester"].ToString();
                        string collegecode = Convert.ToString(ds.Tables[0].Rows[0]["CollegeCode"]);
                        string section = ds.Tables[0].Rows[0]["Section"].ToString();
                        string subtype = ds.Tables[0].Rows[0]["Subject_Type"].ToString();
                        string subtype1 = ds.Tables[0].Rows[0]["OptionalSubject_type"].ToString();
                        string st_type = string.Empty;
                        string st_type1 = string.Empty;
                        string sub_type = string.Empty;
                        string sub_type1 = string.Empty;
                        string filteroption = string.Empty;
                        if (!string.IsNullOrEmpty(subtype))
                        {
                            st_type = subtype.ToString();
                            string[] split = st_type.Split(',');
                            for (int i = 0; i < split.Length; i++)
                            {
                                if (string.IsNullOrEmpty(sub_type))
                                {
                                    sub_type = split[i];
                                }
                                else
                                {
                                    sub_type += "," + split[i];
                                }
                            }
                        }
                        if (!string.IsNullOrEmpty(subtype1))
                        {
                            st_type1 = subtype1.ToString();
                            string[] split = st_type1.Split(',');
                            for (int i = 0; i < split.Length; i++)
                            {
                                if (string.IsNullOrEmpty(sub_type1))
                                {
                                    sub_type1 = split[i];
                                }
                                else
                                {
                                    sub_type1 += "," + split[i];
                                }
                            }
                        }
                        if (!string.IsNullOrEmpty(sub_type1.Trim()))
                        {
                            if (string.IsNullOrEmpty(sub_type.Trim()))
                            {
                                sub_type = sub_type1;
                            }
                            else
                            {
                                sub_type = sub_type + "," + sub_type1;
                            }
                        }
                        if (Optiontypefilter.Trim() == "1" || Optiontypefilter.Trim() == "True")
                            filteroption = "1";
                        else
                            filteroption = "0";
                        hat.Add("UnicodeValue", txt_unic_fb.Text.ToString());
                        hat.Add("CollegeCode", collegecode);
                        hat.Add("degree_code", Deg_code);
                        hat.Add("batch_year", Batch);
                        hat.Add("Semester", Semester);
                        hat.Add("sections", section);
                        hat.Add("subjectType", sub_type);
                        hat.Add("OptionalSubjectType", sub_type1);
                        hat.Add("filteroptionmark", filteroption);
                        ds.Clear();
                        ds = d2.select_method("FeedbackQuery", hat, "sp");
                        FarPoint.Web.Spread.ComboBoxCellType cb = new FarPoint.Web.Spread.ComboBoxCellType();
                        FarPoint.Web.Spread.TextCellType txt = new FarPoint.Web.Spread.TextCellType();
                       

                        string optional = string.Empty;
                        if (ds.Tables[4].Rows.Count > 0)
                        {
                            for (int p = 0; p < ds.Tables[4].Rows.Count; p++)
                            {
                                if (string.IsNullOrEmpty(optional))
                                {
                                    optional = ds.Tables[4].Rows[p]["subject_no"].ToString();
                                }
                                else
                                {
                                    optional = optional + "," + ds.Tables[4].Rows[p]["subject_no"].ToString();
                                }
                            }
                            Session["Optional subject"] = optional;
                        }
                        else
                        {
                            Session["Optional subject"] = "";
                        }
                        if (ds.Tables[0].Rows.Count > 0 && ds.Tables[1].Rows.Count > 0 && ds.Tables[2].Rows.Count > 0 && ds.Tables[3].Rows.Count > 0)
                        {
                            lbl_Name.Visible = true;
                            lbl_Name.Text = ds.Tables[0].Rows[0]["FeedBackName"].ToString();
                            if (ds.Tables[1].Rows.Count > 0)
                            {
                                for (int row = 0; row < ds.Tables[2].Rows.Count; row++)
                                {
                                    FpSpread1.Sheets[0].ColumnCount++;
                                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Sheets[0].ColumnCount - 1].Text = Convert.ToString(ds.Tables[2].Rows[row]["subject_name"]);
                                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Sheets[0].ColumnCount - 1].Tag = Convert.ToString(ds.Tables[2].Rows[row]["subject_no"]);

                                    //FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Sheets[0].ColumnCount - 1].Font.Bold = true;
                                    //FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Sheets[0].ColumnCount - 1].Font.Name = "Book Antiqua";
                                    //FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Sheets[0].ColumnCount - 1].Font.Size = FontUnit.Medium;
                                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Sheets[0].ColumnCount - 1].HorizontalAlign = HorizontalAlign.Center;
                                    if (ds.Tables[3].Rows.Count > 0)
                                    {
                                        ds.Tables[3].DefaultView.RowFilter = "subject_no='" + Convert.ToString(ds.Tables[2].Rows[row]["subject_no"]) + "'";
                                        dv = ds.Tables[3].DefaultView;
                                        if (dv.Count > 0)
                                        {
                                            for (int d = 0; d < dv.Count; d++)
                                            {
                                                if (d == 0)
                                                {
                                                    FpSpread1.Sheets[0].ColumnHeader.Cells[1, FpSpread1.Sheets[0].ColumnCount - 1].Text = Convert.ToString(dv[d]["staff_name"]);
                                                    FpSpread1.Sheets[0].ColumnHeader.Cells[1, FpSpread1.Sheets[0].ColumnCount - 1].Tag = Convert.ToString(dv[d]["staff_code"]);
                                                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Sheets[0].ColumnCount - 1].Note = Convert.ToString(ds.Tables[2].Rows[row]["subject_type"]);
                                                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Sheets[0].ColumnCount - 1].Tag = Convert.ToString(ds.Tables[2].Rows[row]["subject_no"]);
                                                    //FpSpread1.Sheets[0].ColumnHeader.Cells[2, FpSpread1.Sheets[0].ColumnCount - 1].CellType = cball;
                                                }
                                                else
                                                {
                                                    FpSpread1.Sheets[0].ColumnCount++;
                                                    FpSpread1.Sheets[0].ColumnHeader.Cells[1, FpSpread1.Sheets[0].ColumnCount - 1].Text = Convert.ToString(dv[d]["staff_name"]);
                                                    FpSpread1.Sheets[0].ColumnHeader.Cells[1, FpSpread1.Sheets[0].ColumnCount - 1].Tag = Convert.ToString(dv[d]["staff_code"]);
                                                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Sheets[0].ColumnCount - 1].Note = Convert.ToString(ds.Tables[2].Rows[row]["subject_type"]);
                                                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Sheets[0].ColumnCount - 1].Tag = Convert.ToString(ds.Tables[2].Rows[row]["subject_no"]);
                                                    //FpSpread1.Sheets[0].ColumnHeader.Cells[2, FpSpread1.Sheets[0].ColumnCount - 1].CellType = cball;
                                                }
                                                //FpSpread1.Sheets[0].ColumnHeader.Cells[1, FpSpread1.Sheets[0].ColumnCount - 1].Font.Bold = true;
                                                //FpSpread1.Sheets[0].ColumnHeader.Cells[1, FpSpread1.Sheets[0].ColumnCount - 1].Font.Name = "Book Antiqua";
                                                //FpSpread1.Sheets[0].ColumnHeader.Cells[1, FpSpread1.Sheets[0].ColumnCount - 1].Font.Size = FontUnit.Medium;
                                                FpSpread1.Sheets[0].ColumnHeader.Cells[1, FpSpread1.Sheets[0].ColumnCount - 1].HorizontalAlign = HorizontalAlign.Center;
                                                //FpSpread1.Sheets[0].ColumnHeader.Cells[2, FpSpread1.Sheets[0].ColumnCount - 1].HorizontalAlign = HorizontalAlign.Center;
                                                //FpSpread1.Sheets[0].Columns[FpSpread1.Sheets[0].ColumnCount - 1].CellType = cb;
                                            }
                                            FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, FpSpread1.Sheets[0].ColumnCount - dv.Count, 1, dv.Count);
                                        }
                                    }
                                }
                            }
                            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                            {
                                FpSpread1.Sheets[0].RowCount++;
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Text = Convert.ToString(i + 1);
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Tag = ds.Tables[0].Rows[i]["FeedBackMasterPK"].ToString();
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].Tag = ds.Tables[0].Rows[i]["HeaderCode"].ToString();
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].Text = ds.Tables[0].Rows[i]["HeaderName"].ToString();
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].Text = ds.Tables[0].Rows[i]["Question"].ToString();
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].Tag = ds.Tables[0].Rows[i]["QuestionMasterPK"].ToString();
                                if (Optiontypefilter.Trim() == "1" || Optiontypefilter.Trim() == "True")
                                {
                                    ds.Tables[1].DefaultView.RowFilter = " QuestionmasterFK='" + ds.Tables[0].Rows[i]["QuestionMasterPK"].ToString() + "' and FeedbackFk='" + ds.Tables[0].Rows[i]["FeedBackMasterPK"].ToString() + "'";
                                    filteroption_dv = ds.Tables[1].DefaultView;
                                    cb = new FarPoint.Web.Spread.ComboBoxCellType();
                                    cb.DataSource = filteroption_dv.ToTable();
                                    cb.DataTextField = "MarkType";
                                    cb.DataValueField = "MarkType";
                                    if (filteroption_dv.Count == 0)
                                    {
                                        ds.Tables[1].DefaultView.RowFilter = " QuestionmasterFK='" + ds.Tables[0].Rows[i]["QuestionMasterPK"].ToString() + "'";
                                        //filteroption_dv = ds.Tables[1].DefaultView.(true, "MarkType");
                                        //filteroption_dv = ds.Tables[1].DefaultView(true, "MarkType");
                                      DataTable dt=ds.Tables[1].DefaultView.ToTable(true, "MarkType");
                                      cb = new FarPoint.Web.Spread.ComboBoxCellType();
                                      cb.DataSource = dt;
                                      cb.DataTextField = "MarkType";
                                      cb.DataValueField = "MarkType";
                                        
                                    }
                                    
                                }
                                else
                                {
                                    cb = new FarPoint.Web.Spread.ComboBoxCellType();
                                    cb.DataSource = ds.Tables[1];
                                    cb.DataTextField = "MarkType";
                                    cb.DataValueField = "MarkType";
                                }
                                for (int r = 3; r < FpSpread1.Sheets[0].ColumnCount; r++)
                                {
                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, r].CellType = cb;
                                    FpSpread1.Sheets[0].Columns[r].HorizontalAlign = HorizontalAlign.Center;
                                    if (IsSubjectType.Trim() == "1" || IsSubjectType.Trim() == "True")//Subject Wise
                                    {
                                        string subjectHeader = Convert.ToString(FpSpread1.Sheets[0].ColumnHeader.Cells[0, r].Note);
                                        bool flag = false;//delsi2303
                                        string subjecttype = Convert.ToString(ds.Tables[0].Rows[i]["SubjectType"]);
                                        string[] subjectsplit = subjecttype.Split(',');
                                        if (subjectsplit.Length > 0)
                                        {
                                            for (int val = 0; val < subjectsplit.Length; val++)
                                            {
                                                string subjectstring = Convert.ToString(subjectsplit[val]);
                                                if (subjectstring.ToLower().Trim() == subjectHeader.ToLower().Trim())
                                                {
                                                    flag =true;
                                                 
                                                }

                                            }
                                        }

                                            //if (!Convert.ToString(ds.Tables[0].Rows[i]["SubjectType"]).Contains(subjectHeader))
                                            //{
                                            //    FpSpread1.Sheets[0].Cells[i, r].Locked = true;
                                            //    FpSpread1.Sheets[0].Cells[i, r].BackColor = Color.Lavender;
                                            //}

                                            if (flag==false)//delsi2303
                                            {
                                                FpSpread1.Sheets[0].Cells[i, r].Locked = true;
                                                FpSpread1.Sheets[0].Cells[i, r].BackColor = Color.Lavender;
                                            }
                                    }
                                }
                            }
                            FpSpread1.Sheets[0].RowCount++;//txt
                           
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Text = "Enter Comments";
                            for (int r = 3; r < FpSpread1.Sheets[0].ColumnCount; r++)
                            {
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, r].CellType = txt;
                            }
                            FpSpread1.Sheets[0].Columns[0].Locked = true;
                            FpSpread1.Sheets[0].Columns[1].Locked = true;
                            FpSpread1.Sheets[0].Columns[2].Locked = true;
                            FpSpread1.Width = 1000;
                            FpSpread1.Height = 550;
                            FpSpread1.Sheets[0].FrozenColumnCount = 1;
                            FpSpread1.Sheets[0].FrozenColumnCount = 2;
                            FpSpread1.Sheets[0].FrozenColumnCount = 3;
                            FpSpread1.SaveChanges();
                            FpSpread1.Sheets[0].PageSize = FpSpread1.Sheets[0].RowCount;
                            MainDivPart.Visible = true;
                        }
                        else
                        {
                            lbl_alrt.Visible = true;
                            lbl_alrt.Text = "No Records Found";
                            txt_unic_fb.Text = "";
                            FpSpread1.Visible = false;
                            btn_save.Visible = false;
                            MainDivPart.Visible = false;
                        }
                        #endregion
                    }
                    if (Acadamic_Isgeneral.Trim() == "1" || Acadamic_Isgeneral.Trim() == "True")
                    {
                        #region Acadamic_Isgeneral
                        DataView dv_hd = new DataView(); ViewState["Acadamic_Isgeneral"] = "1";
                        string Withoutquestiontype = "0";
                        if (Optiontypefilter.Trim() == "1" || Optiontypefilter.Trim() == "True")
                            Withoutquestiontype = "1";
                        hat.Clear();
                        hat.Add("FeedbackFK", fbfk);
                        hat.Add("CollegeCode", collegecode1);
                        hat.Add("Withoutquestiontypematch", Withoutquestiontype);
                        ds.Clear();
                        ds = d2.select_method("Feedback_acadamic_general", hat, "sp");//barath change procedure
                        if (ds.Tables[2].Rows.Count > 0)
                        {
                            lbl_Name.Text = ds.Tables[2].Rows[0]["FeedBackName"].ToString();
                        }
                        lbl_Name.Visible = true;
                        FpSpread1.Columns[2].Visible = false;
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 1].Text = "Question";
                        if (ds.Tables[0].Rows.Count > 0 && ds.Tables[1].Rows.Count > 0)
                        {
                            for (int row = 0; row < ds.Tables[1].Rows.Count; row++)
                            {
                                FpSpread1.Sheets[0].ColumnCount++;
                                FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, FpSpread1.Sheets[0].ColumnCount - 1, 2, 1);
                                FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Sheets[0].ColumnCount - 1].Text = Convert.ToString(ds.Tables[1].Rows[row]["MarkType"]);
                                FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Sheets[0].ColumnCount - 1].Tag = Convert.ToString(ds.Tables[1].Rows[row]["MarkMasterPK"]);
                                FpSpread1.Sheets[0].Columns[FpSpread1.Sheets[0].ColumnCount - 1].HorizontalAlign = HorizontalAlign.Center;
                                FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Sheets[0].ColumnCount - 1].HorizontalAlign = HorizontalAlign.Center;
                            }
                            FarPoint.Web.Spread.CheckBoxCellType chk = new FarPoint.Web.Spread.CheckBoxCellType();
                            chk.AutoPostBack = true;
                            int sno = 0; string headername = "";
                            for (int i = 0; i < ds.Tables[3].Rows.Count; i++)
                            {
                                if (headername.Trim() != ds.Tables[3].Rows[i]["HeaderCode"].ToString())
                                {
                                    FpSpread1.Sheets[0].RowCount++;
                                    FpSpread1.Sheets[0].SpanModel.Add(FpSpread1.Sheets[0].RowCount - 1, 0, 1, FpSpread1.Sheets[0].ColumnCount);
                                    //FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].Tag = ds.Tables[3].Rows[i]["HeaderCode"].ToString();
                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Text = ds.Tables[3].Rows[i]["HeaderName"].ToString();
                                    headername = ds.Tables[3].Rows[i]["HeaderCode"].ToString();
                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].ForeColor = Color.Blue;
                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Font.Name = "Book Antiqua";
                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Locked = true;
                                }
                                if (ds.Tables[0].Rows.Count > 0)
                                {
                                    ds.Tables[0].DefaultView.RowFilter = " HeaderCode='" + ds.Tables[3].Rows[i]["HeaderCode"].ToString() + "' and QuestionMasterPK='" + ds.Tables[3].Rows[i]["QuestionMasterPK"].ToString() + "'";
                                    dv_hd = ds.Tables[0].DefaultView; string prewquestionmasterfk = ""; bool checklock = false;
                                    if (dv_hd.Count > 0)
                                    {
                                        for (int rs = 0; rs < dv_hd.Count; rs++)
                                        {
                                            if (prewquestionmasterfk.Trim() != dv_hd[rs]["QuestionMasterPK"].ToString())
                                            {
                                                FpSpread1.Sheets[0].RowCount++;
                                                sno++;
                                                checklock = true;
                                            }
                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Text = Convert.ToString(sno);
                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Tag = dv_hd[rs]["FeedBackMasterPK"].ToString();
                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Note = dv_hd[rs]["FeedBackMasterPK"].ToString();
                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].Text = dv_hd[rs]["Question"].ToString();
                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].Font.Name = "Book Antiqua";
                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].Tag = dv_hd[rs]["QuestionMasterPK"].ToString();
                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Locked = true;
                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].Locked = true;
                                            prewquestionmasterfk = dv_hd[rs]["QuestionMasterPK"].ToString();
                                            //optionfilter
                                            if (Optiontypefilter.Trim() == "1" || Optiontypefilter.Trim() == "True")
                                            {
                                                for (int r = 3; r < FpSpread1.Sheets[0].ColumnHeader.Columns.Count; r++)
                                                {
                                                    string markfk = Convert.ToString(FpSpread1.Sheets[0].ColumnHeader.Cells[0, r].Tag);
                                                    if (Convert.ToString(dv_hd[rs]["MarkMasterFK"]) == markfk)
                                                    {
                                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, r].Locked = false;
                                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, r].CellType = chk;
                                                    }
                                                    else
                                                    {
                                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, r].CellType = chk;
                                                        if (checklock == true)
                                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, r].Locked = true;
                                                    }
                                                }
                                            }
                                            else
                                            {
                                                for (int r = 3; r < FpSpread1.Sheets[0].ColumnHeader.Columns.Count; r++)
                                                {
                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, r].Locked = false;
                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, r].CellType = chk;
                                                }
                                            }
                                            checklock = false;
                                        }
                                    }
                                }
                            }
                            FpSpread1.Sheets[0].PageSize = FpSpread1.Sheets[0].RowCount;
                            FpSpread1.Width = 800;
                            FpSpread1.Height = 500;
                            MainDivPart.Visible = true;
                            btn_save.Visible = true;
                            FpSpread1.Visible = true;
                            FpSpread1.SaveChanges();
                        }
                        else
                        {
                            imgdiv2.Visible = true;
                            lbl_alert1.Text = "No Records Found";
                            FpSpread1.Visible = false;
                            btn_save.Visible = false;
                        }
                        #endregion
                    }
                }
                else
                {
                    imgdiv2.Visible = true;
                    lbl_alert1.Text = "No Records Found";
                    FpSpread1.Visible = false;
                    btn_save.Visible = false;
                    MainDivPart.Visible = false;
                }
            }
            else
            {
                imgdiv2.Visible = true;
                lbl_alert1.Text = "No Records Found";
                FpSpread1.Visible = false;
                btn_save.Visible = false;
                MainDivPart.Visible = false;
            }
        }
        catch (Exception ex)
        {
            imgdiv2.Visible = true;
            lbl_alert1.Text = ex.ToString();
        }
    }
    protected void btn_save_Click(object sender, EventArgs e)
    {
        FpSpread1.SaveChanges();
        try
        {
            int up = 0; string insertquery = "";
            string unic_cods = txt_unic_fb.Text.ToString();
            string H_code = "";
            string QuestionMaster_PK = "";
            string MarkMaster_Pk = "";
            string appl_id = "";
            string name = lbl_Name.Text.ToString();
            if (ViewState["Acadamic_Isgeneral"] == null)
            {
                #region old
                // string nextSubject = string.Empty;
                //for (int row1 = 0; row1 < FpSpread1.Sheets[0].Rows.Count+1; row1++)
                //{
                //      string previousSubjectNo = string.Empty;
                //    Dictionary<string, int> dicSubjectSelectedCount = new Dictionary<string, int>();
                //    for (int col1 = 3; col1 < FpSpread1.Sheets[0].ColumnCount; col1++)
                //    {
                //        string[] optional = Convert.ToString(Session["Optional subject"]).Split(',');
                //        string subjectname = Convert.ToString(FpSpread1.Sheets[0].ColumnHeader.Cells[0, col1].Tag);
                //        string getvalue1 = Convert.ToString(FpSpread1.Sheets[0].Cells[row1, col1].Text);
                //        bool celllock = false;
                //        bool.TryParse(Convert.ToString(FpSpread1.Sheets[0].Cells[row1, col1].Locked).Trim(), out celllock);
                //        nextSubject = Convert.ToString(FpSpread1.Sheets[0].ColumnHeader.Cells[0, col1 + 1].Tag);
                //        if (subjectname.Trim() != "")
                //        {
                //            if (!optional.Contains(subjectname))
                //            {
                //                //string getvalue1 = Convert.ToString(FpSpread1.Sheets[0].Cells[row1, col1].Text);
                //                if (string.IsNullOrEmpty(getvalue1))
                //                {
                //                    //bool celllock = Convert.ToBoolean(FpSpread1.Sheets[0].Cells[row1, col1].Locked);
                //                    if (!celllock)
                //                    {
                //                        //imgdiv2.Visible = true;
                //                        //lbl_alert1.Text = "Please Select All Field";
                //                        //questiondiv.Visible = true;
                //                        //return;
                //                    }
                //                }
                //            }
                //        }
                //    }
                #endregion
                #region Acadamic
                for (int row1 = 0; row1 < FpSpread1.Sheets[0].Rows.Count -1; row1++)
                {
                    string previousSubjectNo = string.Empty;
                    Dictionary<string, int> dicSubjectSelectedCount = new Dictionary<string, int>();
                    Dictionary<string, int> dicSubjectUnselectedCount = new Dictionary<string, int>();
                    for (int col1 = 3; col1 < FpSpread1.Sheets[0].ColumnCount; col1++)
                    {
                        string[] optional = Convert.ToString(Session["Optional subject"]).Split(',');
                        string subjectname = Convert.ToString(FpSpread1.Sheets[0].ColumnHeader.Cells[0, col1].Tag);
                        string getvalue1 = Convert.ToString(FpSpread1.Sheets[0].Cells[row1, col1].Text);
                        bool celllock = false;
                        bool.TryParse(Convert.ToString(FpSpread1.Sheets[0].Cells[row1, col1].Locked).Trim(), out celllock);
                        bool includeCount = true;
                        if (!string.IsNullOrEmpty(previousSubjectNo) && subjectname.Trim() != previousSubjectNo)
                        {
                            if (dicSubjectUnselectedCount.Count != 0)
                            {
                                imgdiv2.Visible = true;
                                lbl_alert1.Text = "Please Select All Field";
                                questiondiv.Visible = true;
                                return;
                            }
                        }
                        if (!string.IsNullOrEmpty(subjectname.Trim()))
                        {
                            if (!optional.Contains(subjectname))
                            {
                                if (string.IsNullOrEmpty(getvalue1))
                                {
                                    if (!celllock)
                                    {
                                        includeCount = false;
                                        if (!dicSubjectSelectedCount.ContainsKey(subjectname.Trim()))
                                        {
                                            if (!dicSubjectUnselectedCount.ContainsKey(subjectname.Trim()))
                                                dicSubjectUnselectedCount.Add(subjectname.Trim(), 1);
                                            else
                                                dicSubjectUnselectedCount[subjectname.Trim()] += 1;
                                        }
                                    }
                                }
                                if (includeCount && !celllock)
                                {
                                    if (!dicSubjectSelectedCount.ContainsKey(subjectname.Trim()))
                                        dicSubjectSelectedCount.Add(subjectname.Trim(), 1);
                                    else
                                        dicSubjectSelectedCount[subjectname.Trim()] += 1;

                                    if (dicSubjectUnselectedCount.ContainsKey(subjectname.Trim()))
                                        dicSubjectUnselectedCount.Remove(subjectname.Trim());
                                }
                            }
                        }
                        previousSubjectNo = subjectname;
                    }
                }
                ds.Clear();
                ds = d2.select_method_wo_parameter("feedback_markstaffappid", "sp");
                DataView dv = new DataView();
                DataView dv1 = new DataView();
                int lastrowcount = FpSpread1.Sheets[0].Rows.Count;
                for (int row = 0; row < FpSpread1.Sheets[0].Rows.Count -1; row++)
                {
                    H_code = Convert.ToString(FpSpread1.Sheets[0].Cells[row, 1].Tag);
                    QuestionMaster_PK = Convert.ToString(FpSpread1.Sheets[0].Cells[row, 2].Tag);
                    for (int col = 3; col < FpSpread1.Sheets[0].ColumnCount; col++)
                    {
                        hat.Clear();
                        string subject_no = Convert.ToString(FpSpread1.Sheets[0].ColumnHeader.Cells[0, col].Tag);
                        if (!string.IsNullOrEmpty(subject_no.Trim()))
                        {
                            string staff_code = Convert.ToString(FpSpread1.Sheets[0].ColumnHeader.Cells[1, col].Tag);
                            string getvalue = Convert.ToString(FpSpread1.Sheets[0].Cells[row, col].Text);
                            string getpkcode = Convert.ToString(FpSpread1.Sheets[0].Cells[row, 0].Tag);
                            if (!string.IsNullOrEmpty(getvalue.Trim()))
                            {
                                if (ds.Tables[0].Rows.Count > 0)
                                {
                                    ds.Tables[0].DefaultView.RowFilter = " MarkType ='" + getvalue + "' ";
                                    dv = ds.Tables[0].DefaultView;
                                    if (dv.Count > 0)
                                    {
                                        MarkMaster_Pk = dv[0]["MarkMasterPK"].ToString();
                                    }
                                }
                                if (ds.Tables[1].Rows.Count > 0)
                                {
                                    ds.Tables[1].DefaultView.RowFilter = " staff_code ='" + staff_code + "' ";
                                    dv1 = ds.Tables[1].DefaultView;
                                    if (dv1.Count > 0)
                                    {
                                        appl_id = dv1[0]["appl_id"].ToString();
                                    }
                                }
                                hat.Add("feedbackfk", getpkcode);
                                hat.Add("questionfk", QuestionMaster_PK);
                                hat.Add("uniquecode", unic_cods);
                                hat.Add("subjectno", subject_no);
                                hat.Add("staffapplid", appl_id);
                                hat.Add("markmasterfk", MarkMaster_Pk);
                                int insert = d2.update_method_with_parameter("Onlinefeedbacksave", hat, "sp");
                            }
                        }
                    }
                }
                for (int col = 3; col < FpSpread1.Sheets[0].ColumnCount; col++)
                {
                    string subject_no = Convert.ToString(FpSpread1.Sheets[0].ColumnHeader.Cells[0, col].Tag);
                    if (!string.IsNullOrEmpty(subject_no.Trim()))
                    {
                        if (!string.IsNullOrEmpty(subject_no.Trim()))
                        {
                            string staff_code = Convert.ToString(FpSpread1.Sheets[0].ColumnHeader.Cells[1, col].Tag);
                            string getvalue = Convert.ToString(FpSpread1.Sheets[0].Cells[lastrowcount -1, col].Text);
                            if(getvalue!="")
                            {
                            if (ds.Tables[1].Rows.Count > 0)
                            {
                                ds.Tables[1].DefaultView.RowFilter = " staff_code ='" + staff_code + "' ";
                                dv1 = ds.Tables[1].DefaultView;
                                if (dv1.Count > 0)
                                {
                                    appl_id = dv1[0]["appl_id"].ToString();
                                }
                            }
                            string updateqry = "update CO_StudFeedBack set comments='" + getvalue + "' where  FeedbackUnicode='" + unic_cods + "' and StaffApplNo='" + appl_id + "' and SubjectNo='" + subject_no + "'";
                            int updat = d2.update_method_wo_parameter(updateqry, "text");
                            }
                        }
                       
                    }
                }
                #endregion
            }
            else
            {
                #region Acadamic General
                for (int row = 0; row < FpSpread1.Sheets[0].Rows.Count; row++)
                {
                    QuestionMaster_PK = Convert.ToString(FpSpread1.Sheets[0].Cells[row, 1].Tag);
                    if (QuestionMaster_PK.Trim() != "")
                    {
                        for (int col = 3; col < FpSpread1.Sheets[0].ColumnCount; col++)
                        {
                            hat.Clear();
                            MarkMaster_Pk = Convert.ToString(FpSpread1.Sheets[0].ColumnHeader.Cells[0, col].Tag);
                            string feedbackFK = Convert.ToString(FpSpread1.Sheets[0].Cells[row, 0].Tag);
                            int getvalue = Convert.ToInt32(FpSpread1.Sheets[0].Cells[row, col].Value);
                            if (getvalue == 1)
                            {
                                if (MarkMaster_Pk.Trim() != "" && feedbackFK.Trim() != null)
                                {
                                    hat.Add("feedbackfk", feedbackFK);
                                    hat.Add("questionfk", QuestionMaster_PK);
                                    hat.Add("uniquecode", unic_cods);
                                    hat.Add("markmasterfk", MarkMaster_Pk);
                                    int insert = d2.update_method_with_parameter("OnlinefeedbackIsgeneralsave", hat, "sp");
                                }
                            }
                        }
                    }
                }
                #endregion
            }
            insertquery = " update CO_FeedbackUniCode set IsCheckFlag='1' where FeedbackUnicode='" + unic_cods + "' ";
            up = d2.update_method_wo_parameter(insertquery, "text");
            if (up != 0)
            {
                imgdiv2.Visible = true;
                lbl_alert1.Text = "Saved Successfully";
                questiondiv.Visible = false;
                txt_unic_fb.Text = "";
                div1.Visible = true;
                MainDivPart.Visible = false;
            }
        }
        catch (Exception ex)
        {
            imgdiv2.Visible = true;
            lbl_alert1.Text = ex.ToString();
        }
    }
    protected void lb3_Click(object sender, EventArgs e)
    {
        questiondiv.Visible = false;
        div1.Visible = true;
        txt_unic_fb.Text = "";
        MainDivPart.Visible = false;
    }
    protected void Clickbtn_Click(object sender, EventArgs e)
    {
        string Fk = Convert.ToString(Label2.Text);
        if (!string.IsNullOrEmpty(Fk.Trim()))
        {
            online(Fk);
        }
        else
        {
            questiondiv.Visible = false;
            div1.Visible = true;
            txt_unic_fb.Text = "";
            MainDivPart.Visible = false;
        }
    }
    protected void FpSpread1_OnButtonCommand(object sender, EventArgs e)
    {
        if (ViewState["Acadamic_Isgeneral"] != null)
        {
            FpSpread1.SaveChanges();
            int activerow = FpSpread1.ActiveSheetView.ActiveRow;
            int activecol = FpSpread1.ActiveSheetView.ActiveColumn;
            int checkval = Convert.ToInt32(FpSpread1.Sheets[0].Cells[activerow, activecol].Value);
            if (checkval == 1)
            {
                for (int i = 3; i < FpSpread1.Sheets[0].ColumnCount; i++)
                {
                    if (i == Convert.ToInt32(activecol))
                    {
                        FpSpread1.Sheets[0].Cells[activerow, i].Value = 1;
                    }
                    else
                    {
                        FpSpread1.Sheets[0].Cells[activerow, i].Value = 0;
                    }
                }
            }
        }
    }
}

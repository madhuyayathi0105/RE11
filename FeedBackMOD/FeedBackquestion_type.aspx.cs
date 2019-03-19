using System;
using System.Collections.Generic;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Web.Services;
public partial class FeedBackMOD_FeedBackquestion_type : System.Web.UI.Page
{
    ReuasableMethods rs = new ReuasableMethods();
    DAccess2 d2 = new DAccess2();
    DataSet ds = new DataSet();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["collegecode"] == null)
        {
            Response.Redirect("~/Default.aspx");
        }
        if (!IsPostBack)
        {
            Bindcollege();
            BindFeedback();
            Bindheader();
            Bindheader1();
            BindQuestion();
            BindQuestion1();
            BindType();
            BindType1();
        }
    }
    //protected void cb_header_CheckedChanged(object sender, EventArgs e)
    //{
    //    rs.CallCheckBoxChangedEvent(cbl_header, cb_header, txt_header, "Header");
    //    BindQuestion();
    //}
    //protected void cbl_header_SelectedIndexChanged(object sender, EventArgs e)
    //{
    //    rs.CallCheckBoxListChangedEvent(cbl_header, cb_header, txt_header, "Header");
    //    BindQuestion();
    //}
    protected void ddl_header_selectedindexChanged(object sender, EventArgs e)
    {
        BindQuestion();
    }
    protected void cb_question_CheckedChanged(object sender, EventArgs e)
    {
        rs.CallCheckBoxChangedEvent(cbl_question, cb_question, txt_question, "Question");
    }
    protected void cbl_question_SelectedIndexChanged(object sender, EventArgs e)
    {
        rs.CallCheckBoxListChangedEvent(cbl_question, cb_question, txt_question, "Question");
    }
    protected void cb_option_CheckedChanged(object sender, EventArgs e)
    {
        //rs.CallCheckBoxChangedEvent(cbl_option, cb_option, txt_option, "Option");
        int count = 0;
        if (cb_option.Checked)
        {
            for (int i = 0; i < cbl_option.Items.Count; i++)
            {
                cbl_option.Items[i].Selected = true;
                if (cbl_option.Items[i].Selected == true)
                {
                    count++;
                }
            }
        }
        else
        {
            for (int i = 0; i < cbl_option.Items.Count; i++)
            {
                cbl_option.Items[i].Selected = false;
            }
        }
        if (count == cbl_option.Items.Count)
        {
            cb_option.Checked = true;
        }
    }
    protected void cb_header1_CheckedChanged(object sender, EventArgs e)
    {
        rs.CallCheckBoxChangedEvent(cbl_header1, cb_header1, txt_header1, "Header");
        BindQuestion1();
        BindType1();
    }
    protected void cbl_header1_SelectedIndexChanged(object sender, EventArgs e)
    {
        rs.CallCheckBoxListChangedEvent(cbl_header1, cb_header1, txt_header1, "Header");
        BindQuestion1();
        BindType1();
    }
    protected void cb_question1_CheckedChanged(object sender, EventArgs e)
    {
        rs.CallCheckBoxChangedEvent(cbl_question1, cb_question1, txt_question1, "Question");
        BindType1();
    }
    protected void cbl_question1_SelectedIndexChanged(object sender, EventArgs e)
    {
        rs.CallCheckBoxListChangedEvent(cbl_question1, cb_question1, txt_question1, "Question");
        BindType1();
    }
    protected void cb_option1_CheckedChanged(object sender, EventArgs e)
    {
        rs.CallCheckBoxChangedEvent(cbl_option1, cb_option1, txt_option1, "Option");
    }
    protected void cbl_option1_SelectedIndexChanged(object sender, EventArgs e)
    {
        rs.CallCheckBoxListChangedEvent(cbl_option1, cb_option1, txt_option1, "Option");
    }
    protected void ddl_feedback_onselectedindexchanged(object sender, EventArgs e)
    {
        Bindheader();
        BindQuestion();
    }
    protected void ddl_feedback1_onselectedindexchanged(object sender, EventArgs e)
    {
        Bindheader1();
        BindQuestion1();
        BindType1();
    }
    public void BindType()
    {
        try
        {
            //txt_option.Text = "--Select--";
            cbl_option.Items.Clear();
            cb_option.Checked = false;
            if (ddl_college.Items.Count > 0)
            {
                string selqry = " SELECT  distinct (MarkType),MarkMasterPK FROM CO_MarkMaster WHERE  CollegeCode in (" + Convert.ToString(ddl_college.SelectedItem.Value) + ")";
                ds = d2.select_method_wo_parameter(selqry, "Text");
                if (ds.Tables[0].Rows.Count > 0)
                {
                    cbl_option.DataSource = ds;
                    cbl_option.DataTextField = "MarkType";
                    cbl_option.DataValueField = "MarkMasterPK";
                    cbl_option.DataBind();
                }
                if (cbl_option.Items.Count > 0)
                {
                    for (int row = 0; row < cbl_option.Items.Count; row++)
                    {
                        cbl_option.Items[row].Selected = true;
                        cb_option.Checked = true;
                    }
                    //txt_option.Text = "Option(" + cbl_option.Items.Count + ")";
                }
                else
                {
                    //txt_option.Text = "--Select--";
                }
            }
        }
        catch
        {
        }
    }
    protected void Bindcollege()
    {
        try
        {
            ds.Clear();
            ddl_college.Items.Clear();
            string clgname = "select cp.college_code,cf.collname from collegeprivilages cp,collinfo cf where user_code=" + Session["usercode"] + " and cp.college_code=cf.college_code";
            ds = d2.select_method_wo_parameter(clgname, "Text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                ddl_college.DataSource = ds;
                ddl_college.DataTextField = "collname";
                ddl_college.DataValueField = "college_code";
                ddl_college.DataBind();
                ddl_college1.DataSource = ds;
                ddl_college1.DataTextField = "collname";
                ddl_college1.DataValueField = "college_code";
                ddl_college1.DataBind();
            }
        }
        catch (Exception ex)
        {
        }
    }
    protected void BindFeedback()
    {
        try
        {
            string q1 = "select distinct  FeedBackName  from CO_FeedBackMaster where FeedBackType ='1' and CollegeCode in ('" + ddl_college.SelectedItem.Value + "')";
            q1 += " select distinct  FeedBackName  from CO_FeedBackMaster where FeedBackType ='1' and CollegeCode in ('" + ddl_college1.SelectedItem.Value + "')";
            ds.Clear();
            ds = d2.select_method_wo_parameter(q1, "Text");
            ddl_feedback.Items.Clear();
            ddl_feedback1.Items.Clear();
            if (ds.Tables[0].Rows.Count > 0)
            {
                ddl_feedback.DataSource = ds;
                ddl_feedback.DataTextField = "FeedBackName";
                ddl_feedback.DataValueField = "FeedBackName";
                ddl_feedback.DataBind();
                //ddl_feedback.Items.Insert(0, "Select");
            }
            else
            {
                ddl_feedback.Items.Clear();
                //ddl_feedback.Items.Insert(0, "Select");
            }
            if (ds.Tables[1].Rows.Count > 0)
            {
                ddl_feedback1.DataSource = ds.Tables[1];
                ddl_feedback1.DataTextField = "FeedBackName";
                ddl_feedback1.DataValueField = "FeedBackName";
                ddl_feedback1.DataBind();
                //ddl_feedback.Items.Insert(0, "Select");
            }
            else
            {
                ddl_feedback1.Items.Clear();
                //ddl_feedback.Items.Insert(0, "Select");
            }
        }
        catch { }
    }
    protected void Bindheader()
    {
        try
        {
            if (ddl_college.Items.Count > 0 && ddl_feedback.Items.Count > 0)
            {
                string q1 = " select FeedBackMasterPK from CO_FeedBackMaster where FeedBackName='" + ddl_feedback.SelectedItem.Value + "'";
                ds.Clear();
                ds = d2.select_method_wo_parameter(q1, "text");
                string FeedBackMasterPK = GetdatasetRowstring(ds, "FeedBackMasterPK");
                string selqry = "  SELECT distinct (select TextVal from TextValTable where TextCode= HeaderCode) as HeaderName,HeaderCode FROM CO_QuestionMaster q,CO_FeedBackQuestions fq where q.QuestionMasterPK=fq.QuestionMasterFK and fq.FeedBackMasterFK in('" + FeedBackMasterPK + "') and CollegeCode in ('" + ddl_college.SelectedItem.Value + "')";//fq.FeedBackMasterFK,
                ds = d2.select_method_wo_parameter(selqry, "Text");
                ddl_header.Items.Clear();
                if (ds.Tables[0].Rows.Count > 0)
                {
                    ddl_header.DataSource = ds.Tables[0];
                    ddl_header.DataTextField = "HeaderName";
                    ddl_header.DataValueField = "HeaderCode";
                    ddl_header.DataBind();
                }
            }
        }
        catch { }
    }
    protected void Bindheader1()
    {
        try
        {
            if (ddl_college1.Items.Count > 0 && ddl_feedback1.Items.Count > 0)
            {

                string q1 = " select FeedBackMasterPK from CO_FeedBackMaster where FeedBackName='" + ddl_feedback1.SelectedItem.Value + "'";
                ds.Clear();
                ds = d2.select_method_wo_parameter(q1, "text");
                string FeedBackMasterPK1 = GetdatasetRowstring(ds, "FeedBackMasterPK");

                string selqry = "  SELECT distinct (select TextVal from TextValTable where TextCode= HeaderCode) as HeaderName,HeaderCode FROM CO_QuestionMaster q,CO_FeedBackQuestions fq where q.QuestionMasterPK=fq.QuestionMasterFK and fq.FeedBackMasterFK in('" + FeedBackMasterPK1 + "') and CollegeCode in ('" + ddl_college1.SelectedItem.Value + "')";//fq.FeedBackMasterFK,
                ds = d2.select_method_wo_parameter(selqry, "Text");
                cbl_header1.Items.Clear();
                txt_header1.Text = "--Select--";
                cb_header1.Checked = false;
                if (ds.Tables[0].Rows.Count > 0)
                {
                    cbl_header1.DataSource = ds.Tables[0];
                    cbl_header1.DataTextField = "HeaderName";
                    cbl_header1.DataValueField = "HeaderCode";
                    cbl_header1.DataBind();
                    if (cbl_header1.Items.Count > 0)
                    {
                        for (int row = 0; row < cbl_header1.Items.Count; row++)
                        {
                            cbl_header1.Items[row].Selected = true;
                            cb_header1.Checked = true;
                        }
                        txt_header1.Text = "Header(" + cbl_header1.Items.Count + ")";
                    }
                }
                else
                {
                    cb_header1.Checked = false;
                    txt_header1.Text = "--Select--";
                }
            }
        }
        catch { }
    }


    protected void BindQuestion()
    {
        try
        {
            if (ddl_college.Items.Count > 0 && ddl_header.Items.Count > 0 && ddl_feedback.Items.Count > 0)
            {
                string headercode = ""; cbl_question.Items.Clear(); txt_question.Text = "--Select--";
                cbl_question1.Items.Clear(); txt_question1.Text = "--Select--";
                string headercode1 = rs.GetSelectedItemsValueAsString(cbl_header1);
                headercode = Convert.ToString(ddl_header.SelectedItem.Value);
                if (headercode.Trim() != "")
                {
                    string q1 = " select FeedBackMasterPK from CO_FeedBackMaster where FeedBackName='" + ddl_feedback.SelectedItem.Value + "'";
                    ds.Clear();
                    ds = d2.select_method_wo_parameter(q1, "text");
                    string FeedBackMasterPK = GetdatasetRowstring(ds, "FeedBackMasterPK");
                    string selqry = " select distinct Question,QuestionMasterPK from CO_QuestionMaster q, CO_FeedBackQuestions fq where q.QuestionMasterPK=fq.QuestionMasterFK and  CollegeCode='" + ddl_college.SelectedItem.Value + "' and HeaderCode in('" + headercode + "') and fq.FeedBackMasterFK in('" + FeedBackMasterPK + "') order by question";
                    selqry += " select distinct Question,QuestionMasterPK from CO_QuestionMaster where CollegeCode='" + ddl_college1.SelectedItem.Value + "' and HeaderCode in('" + headercode1 + "') order by question";
                    ds = d2.select_method_wo_parameter(selqry, "Text");
                    cbl_question.Items.Clear();
                    txt_question.Text = "--Select--";
                    cb_question.Checked = false;
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        cbl_question.DataSource = ds.Tables[0];
                        cbl_question.DataTextField = "Question";
                        cbl_question.DataValueField = "QuestionMasterPK";
                        cbl_question.DataBind();
                        if (cbl_question.Items.Count > 0)
                        {
                            for (int row = 0; row < cbl_question.Items.Count; row++)
                            {
                                cbl_question.Items[row].Selected = true;
                                cb_question.Checked = true;
                            }
                            txt_question.Text = "Question(" + cbl_question.Items.Count + ")";
                        }
                    }
                    else
                    {
                        cb_question.Checked = false;
                        txt_question.Text = "--Select--";
                    }
                    if (ds.Tables[1].Rows.Count > 0)
                    {
                        cbl_question1.DataSource = ds.Tables[1];
                        cbl_question1.DataTextField = "Question";
                        cbl_question1.DataValueField = "QuestionMasterPK";
                        cbl_question1.DataBind();
                        if (cbl_question1.Items.Count > 0)
                        {
                            for (int row = 0; row < cbl_question1.Items.Count; row++)
                            {
                                cbl_question1.Items[row].Selected = true;
                                cb_question1.Checked = true;
                            }
                            txt_question1.Text = "Question(" + cbl_question1.Items.Count + ")";
                        }
                    }
                    else
                    {
                        cb_question1.Checked = false;
                        txt_question1.Text = "--Select--";
                    }
                }
            }
        }
        catch { }
    }
    protected void BindQuestion1()
    {
        try
        {
            if (ddl_college1.Items.Count > 0 && cbl_header1.Items.Count > 0 && ddl_feedback1.Items.Count > 0)
            {
                cbl_question1.Items.Clear();
                txt_question1.Text = "--Select--";
                string headercode1 = rs.GetSelectedItemsValueAsString(cbl_header1);
                if (headercode1.Trim() != "")
                {
                    string q1 = " select FeedBackMasterPK from CO_FeedBackMaster where FeedBackName='" + ddl_feedback1.SelectedItem.Value + "'";
                    ds.Clear();
                    ds = d2.select_method_wo_parameter(q1, "text");
                    string FeedBackMasterPK = GetdatasetRowstring(ds, "FeedBackMasterPK");
                    string selqry = "  select distinct Question,QuestionMasterPK from CO_QuestionMaster q,CO_FeedBackQuestions fq where q.QuestionMasterPK=fq.QuestionMasterFK and fq.FeedBackMasterFK in('" + FeedBackMasterPK + "') and  q.CollegeCode='" + ddl_college1.SelectedItem.Value + "' and q.HeaderCode in('" + headercode1 + "') order by question";
                    ds = d2.select_method_wo_parameter(selqry, "Text");
                    cbl_question.Items.Clear();
                    txt_question.Text = "--Select--";
                    cb_question.Checked = false;
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        cbl_question1.DataSource = ds.Tables[0];
                        cbl_question1.DataTextField = "Question";
                        cbl_question1.DataValueField = "QuestionMasterPK";
                        cbl_question1.DataBind();
                        if (cbl_question1.Items.Count > 0)
                        {
                            for (int row = 0; row < cbl_question1.Items.Count; row++)
                            {
                                cbl_question1.Items[row].Selected = true;
                                cb_question1.Checked = true;
                            }
                            txt_question1.Text = "Question(" + cbl_question1.Items.Count + ")";
                        }
                    }
                    else
                    {
                        cb_question1.Checked = false;
                        txt_question1.Text = "--Select--";
                    }
                }
            }
        }
        catch { }
    }
    public void BindType1()
    {
        try
        {
            txt_option1.Text = "--Select--";
            cbl_option1.Items.Clear();
            cb_option1.Checked = false;
            string headercode = rs.GetSelectedItemsValueAsString(cbl_header1);
            string questionfk = rs.GetSelectedItemsValueAsString(cbl_question1);
            if (ddl_college.Items.Count > 0)
            {
                if (headercode.Trim() != "" && questionfk.Trim() != "")
                {
                    string selqry = " select distinct m.MarkMasterPK,m.MarkType from Co_Question_Type qt,CO_MarkMaster m,CO_QuestionMaster q where qt.MarkMasterFK=m.MarkMasterPK and qt.QuestionmasterFK=q.QuestionMasterPK and qt.HeaderCode=q.HeaderCode and m.CollegeCode=q.CollegeCode and q.HeaderCode in('" + headercode + "') and qt.QuestionmasterFK in('" + questionfk + "')and m.CollegeCode in (" + Convert.ToString(ddl_college1.SelectedItem.Value) + ")";
                    ds = d2.select_method_wo_parameter(selqry, "Text");
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        cbl_option1.DataSource = ds;
                        cbl_option1.DataTextField = "MarkType";
                        cbl_option1.DataValueField = "MarkMasterPK";
                        cbl_option1.DataBind();
                    }
                    if (cbl_option1.Items.Count > 0)
                    {
                        for (int row = 0; row < cbl_option1.Items.Count; row++)
                        {
                            cbl_option1.Items[row].Selected = true;
                            cb_option1.Checked = true;
                        }
                        txt_option1.Text = "Option(" + cbl_option1.Items.Count + ")";
                    }
                    else
                    {
                        txt_option1.Text = "--Select--";
                    }
                }
            }
        }
        catch
        {
        }
    }
    protected void imagebtnpopclose1_Click(object sender, EventArgs e)
    {
        addnew.Visible = false;
    }
    protected void btn_go_Click(object sender, EventArgs e)
    {
        try
        {
            lbl_error.Visible = false;
            FpSpread1.SaveChanges();
            FpSpread1.Sheets[0].RowCount = 0;
            FpSpread1.Sheets[0].ColumnCount = 0;
            FpSpread1.CommandBar.Visible = false;
            FpSpread1.Sheets[0].AutoPostBack = true;
            FpSpread1.Sheets[0].ColumnHeader.RowCount = 1;
            FpSpread1.Sheets[0].RowHeader.Visible = false;
            FpSpread1.Sheets[0].ColumnCount = 4;
            FarPoint.Web.Spread.StyleInfo darkstyle = new FarPoint.Web.Spread.StyleInfo();
            darkstyle.BackColor = ColorTranslator.FromHtml("#0CA6CA");
            darkstyle.ForeColor = Color.White;
            FpSpread1.ActiveSheetView.ColumnHeader.DefaultStyle = darkstyle;
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 0].Text = "S.No";
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 0].Font.Bold = true;
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 0].HorizontalAlign = HorizontalAlign.Center;
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 0].Font.Name = "Book Antiqua";
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 0].Font.Size = FontUnit.Medium;
            FpSpread1.Sheets[0].Columns[0].HorizontalAlign = HorizontalAlign.Center;
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 1].Text = "Header";
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 1].Font.Bold = true;
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 1].Font.Name = "Book Antiqua";
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 1].Font.Size = FontUnit.Medium;
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 1].HorizontalAlign = HorizontalAlign.Center;
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 2].Text = "Questions";
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 2].Font.Bold = true;
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 2].Font.Name = "Book Antiqua";
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 2].Font.Size = FontUnit.Medium;
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 2].HorizontalAlign = HorizontalAlign.Center;
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 3].Text = "Question Option";
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 3].Font.Bold = true;
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 3].Font.Name = "Book Antiqua";
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 3].Font.Size = FontUnit.Medium;
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 3].HorizontalAlign = HorizontalAlign.Center;
            FpSpread1.Sheets[0].ColumnHeader.Columns[0].Width = 59;
            FpSpread1.Sheets[0].ColumnHeader.Columns[1].Width = 150;
            FpSpread1.Sheets[0].ColumnHeader.Columns[2].Width = 328;
            string headercode = rs.GetSelectedItemsValueAsString(cbl_header1);
            string questionfk = rs.GetSelectedItemsValueAsString(cbl_question1);
            string markmasterfk = rs.GetSelectedItemsValueAsString(cbl_option1);

            string q1 = " select FeedBackMasterPK from CO_FeedBackMaster where FeedBackName='" + ddl_feedback1.SelectedItem.Value + "'";
            ds.Clear();
            ds = d2.select_method_wo_parameter(q1, "text");
            string FeedBackMasterPK = GetdatasetRowstring(ds, "FeedBackMasterPK");
            q1 = " select distinct t.TextVal,q.Question,m.MarkType,qt.QuestionmasterFK,qt.HeaderCode,qt.MarkMasterFK,q.CollegeCode  from Co_Question_Type qt,CO_MarkMaster m,CO_QuestionMaster q,TextValTable t,CO_FeedBackQuestions fq where q.QuestionMasterPK=fq.QuestionMasterFK and t.TextCode= qt.HeaderCode and qt.MarkMasterFK=m.MarkMasterPK and qt.QuestionmasterFK=q.QuestionMasterPK and qt.HeaderCode=q.HeaderCode and m.CollegeCode=q.CollegeCode and q.HeaderCode in('" + headercode + "') and qt.MarkMasterFK in('" + markmasterfk + "') and  qt.FeedbackFk=fq.FeedBackMasterFK and qt.FeedbackFk in('" + FeedBackMasterPK + "') and qt.QuestionmasterFK in('" + questionfk + "') order by TextVal";
            ds.Clear();
            ds = d2.select_method_wo_parameter(q1, "text"); int row = 0;
            if (ds.Tables.Count > 0)
            {
                if (ds.Tables[0].Rows.Count > 0)
                {
                    for (int i = 0; i < cbl_header1.Items.Count; i++)
                    {
                        if (cbl_header1.Items[i].Selected == true)
                        {
                            for (int j = 0; j < cbl_question1.Items.Count; j++)
                            {
                                if (cbl_question1.Items[j].Selected == true)
                                {
                                    ds.Tables[0].DefaultView.RowFilter = " HeaderCode='" + cbl_header1.Items[i].Value + "' and  QuestionmasterFK='" + cbl_question1.Items[j].Value + "'"; //and MarkType='"++"' and
                                    DataView dv = ds.Tables[0].DefaultView;
                                    if (dv.Count > 0)
                                    {
                                        foreach (DataRowView dr in dv)
                                        {
                                            row++;
                                            FpSpread1.Sheets[0].RowCount++;
                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Text = row.ToString();
                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Font.Size = FontUnit.Medium;
                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Font.Name = "Book Antiqua";
                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].Text = Convert.ToString(dr["TextVal"]);
                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].HorizontalAlign = HorizontalAlign.Center;
                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].VerticalAlign = VerticalAlign.Middle;
                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].Font.Size = FontUnit.Medium;
                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].Font.Name = "Book Antiqua";
                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].Text = Convert.ToString(dr["Question"]);
                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].HorizontalAlign = HorizontalAlign.Center;
                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].VerticalAlign = VerticalAlign.Middle;
                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].Font.Size = FontUnit.Medium;
                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].Font.Name = "Book Antiqua";
                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 3].Text = Convert.ToString(dr["MarkType"]);
                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 3].HorizontalAlign = HorizontalAlign.Center;
                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 3].Font.Size = FontUnit.Medium;
                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 3].Font.Name = "Book Antiqua";
                                            FpSpread1.Visible = true;
                                            rptprint1.Visible = true;
                                        }
                                    }
                                }
                            }
                        }
                    }
                    FpSpread1.Sheets[0].PageSize = FpSpread1.Sheets[0].Rows.Count;
                    FpSpread1.Sheets[0].SetColumnMerge(1, FarPoint.Web.Spread.Model.MergePolicy.Always);
                    FpSpread1.Sheets[0].SetColumnMerge(2, FarPoint.Web.Spread.Model.MergePolicy.Always);
                }
                else
                {
                    lbl_error.Visible = true;
                    lbl_error.Text = "No Records Found";
                    FpSpread1.Visible = false;
                    rptprint1.Visible = false;
                }
            }
            else
            {
                lbl_error.Visible = true;
                lbl_error.Text = "No Records Found";
                FpSpread1.Visible = false;
                rptprint1.Visible = false;
            }
            FpSpread1.Sheets[0].PageSize = FpSpread1.Sheets[0].RowCount;
        }
        catch
        {
        }
    }
    protected void btnExcel1_Click(object sender, EventArgs e)
    {
        try
        {
            string reportname = txtexcelname1.Text;
            if (reportname.ToString().Trim() != "")
            {
                if (FpSpread1.Visible == true)
                {
                    d2.printexcelreport(FpSpread1, reportname);
                }
                lbl_norec1.Visible = false;
            }
            else
            {
                lbl_norec1.Text = "Please Enter Your Report Name";
                lbl_norec1.Visible = true;
                txtexcelname1.Focus();
            }
        }
        catch
        {
        }
    }
    protected void btnprintmaster1_Click(object sender, EventArgs e)
    {
        try
        {
            string dptname = "Question Type Matching";
            string pagename = "FeedBackquestion_type.aspx";
            if (FpSpread1.Visible == true)
            {
                Printcontrol1.loadspreaddetails(FpSpread1, pagename, dptname);
            }
            else
            {
                Printcontrol1.loadspreaddetails(FpSpread1, pagename, dptname);
            }
            Printcontrol1.Visible = true;
            lbl_norec1.Visible = false;
        }
        catch
        {
        }
    }
    protected void btn_Save_Click(object sender, EventArgs e)
    {
        try
        {
            bool updatecheck = false;
            string optionselected = rs.GetSelectedItemsValueAsString(cbl_option);
            if (ddl_header.Items.Count > 0 && txt_question.Text != "--Select--" && optionselected.Trim() != "")//txt_header.Text != "--Select--"
            {
                if (ddl_header.Items.Count > 0)
                {
                    string q1 = " select FeedBackMasterPK from CO_FeedBackMaster where FeedBackName='" + ddl_feedback1.SelectedItem.Value + "'";
                    ds.Clear();
                    ds = d2.select_method_wo_parameter(q1, "text");
                    string FeedBackMasterPK = GetdatasetRowstring(ds, "FeedBackMasterPK").Replace("','", ",");
                    string[] FeedbackFK = FeedBackMasterPK.Split(',');
                    foreach (string FeedFK in FeedbackFK)
                    {
                        for (int j = 0; j < cbl_question.Items.Count; j++)
                        {
                            if (cbl_question.Items[j].Selected == true)
                            {
                                q1 = " delete  from Co_Question_Type where QuestionmasterFK='" + cbl_question.Items[j].Value + "'  and HeaderCode='" + ddl_header.SelectedItem.Value + "' and FeedbackFk='" + FeedFK + "'";
                                int up = d2.update_method_wo_parameter(q1, "text");
                                for (int k = 0; k < cbl_option.Items.Count; k++)
                                {
                                    if (cbl_option.Items[k].Selected == true)
                                    {
                                        q1 = " if not exists( select QuestionmasterFK from Co_Question_Type where QuestionmasterFK='" + cbl_question.Items[j].Value + "' and MarkMasterFK='" + cbl_option.Items[k].Value + "' and HeaderCode='" + ddl_header.SelectedItem.Value + "' and FeedbackFk='" + FeedFK + "')insert into Co_Question_Type (QuestionmasterFK, MarkMasterFK,HeaderCode,FeedbackFk )values ('" + cbl_question.Items[j].Value + "','" + cbl_option.Items[k].Value + "','" + ddl_header.SelectedItem.Value + "','" + FeedFK + "')";
                                        up = d2.update_method_wo_parameter(q1, "text");
                                        if (up != 0)
                                        {
                                            updatecheck = true;
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
                if (updatecheck == true)
                {
                    lbl_error1.Visible = true;
                    lbl_error1.Text = "Saved Successfully";
                    lbl_error1.ForeColor = Color.Green;

                    cbl_option.ClearSelection();
                }
            }
            else
            {
                lbl_error1.ForeColor = Color.Red;
                lbl_error1.Visible = true;
                lbl_error1.Text = "No Records Founds";
            }
        }
        catch (Exception ex)
        {
            lbl_error1.ForeColor = Color.Red;
            lbl_error1.Visible = true;
            lbl_error1.Text = ex.ToString();
        }
    }
    protected void btn_addnew_Click(object sender, EventArgs e)
    {
        if (ddl_feedback.Items.Count > 0)
            ddl_feedback.SelectedIndex = 0;
        if (ddl_header.Items.Count > 0)
            ddl_header.SelectedIndex = 0;
        cbl_option.ClearSelection();
        lbl_error1.Text = "";
        addnew.Visible = true;
    }
    public string GetdatasetRowstring(DataSet dummy, string collname)
    {
        System.Text.StringBuilder sbSelected = new System.Text.StringBuilder();
        try
        {
            foreach (DataRow dr in dummy.Tables[0].Rows)
            {
                if (sbSelected.Length == 0)
                {
                    sbSelected.Append(Convert.ToString(dr[collname]));
                }
                else
                {
                    sbSelected.Append("','" + Convert.ToString(dr[collname]));
                }
            }
        }
        catch { sbSelected.Clear(); }
        return sbSelected.ToString();
    }
}
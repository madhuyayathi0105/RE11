using System;
using System.Collections.Generic;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Web.Services;


public partial class Type_Master : System.Web.UI.Page
{
    bool cellclick = false;
    string usercode = string.Empty;
    static string collegecode1 = string.Empty;
    static string collegecode = string.Empty;
    string singleuser = string.Empty;
    string group_user = string.Empty;
    static string clgcode = string.Empty;
    static string typ = string.Empty;
    static string clgcode1 = string.Empty;
    string selectQuery = "";
    DAccess2 d2 = new DAccess2();
    DataSet ds = new DataSet();
    DataSet ds1 = new DataSet();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["collegecode"] == null)
        {
            Response.Redirect("~/Default.aspx");
        }
        usercode = Session["usercode"].ToString();
        collegecode1 = Session["collegecode"].ToString();
        singleuser = Session["single_user"].ToString();
        group_user = Session["group_code"].ToString();
        collegecode1 = Session["collegecode"].ToString();
        lbl_norec1.Visible = false;
        if (!IsPostBack)
        {


            bindclg();
            bindclg1();
            BindType();
            btnSearch_Click(sender, e);
        }

    }


    public void Cb_college_CheckedChanged(object sender, EventArgs e)
    {
        try
        {
            int cout = 0;
            Txt_college.Text = "--Select--";
            if (Cb_college.Checked == true)
            {
                cout++;
                for (int i = 0; i < Cbl_college.Items.Count; i++)
                {
                    Cbl_college.Items[i].Selected = true;
                }
                Txt_college.Text = "College(" + (Cbl_college.Items.Count) + ")";
            }
            else
            {
                for (int i = 0; i < Cbl_college.Items.Count; i++)
                {
                    Cbl_college.Items[i].Selected = false;
                }
                Txt_college.Text = "--Select--";
            }

            string college = "";
            for (int row = 0; row < Cbl_college.Items.Count; row++)
            {
                if (Cbl_college.Items[row].Selected == true)
                {
                    if (college == "")
                    {
                        college = Cbl_college.Items[row].Value;
                    }
                    else
                    {
                        college = college + "," + Cbl_college.Items[row].Value;
                    }
                }
            }
            clgcode1 = college;
            //  BindType();

        }

        catch (Exception ex)
        {

        }

    }
    public void Cbl_college_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {

            int commcount = 0;
            Txt_college.Text = "--Select--";
            Cb_college.Checked = false;

            for (int i = 0; i < Cbl_college.Items.Count; i++)
            {
                if (Cbl_college.Items[i].Selected == true)
                {
                    commcount = commcount + 1;
                    Cb_college.Checked = false;
                }
            }
            if (commcount > 0)
            {
                if (commcount == Cbl_college.Items.Count)
                {

                    Cb_college.Checked = true;
                }
                Txt_college.Text = "College(" + commcount.ToString() + ")";

            }
            string college = "";
            for (int row = 0; row < Cbl_college.Items.Count; row++)
            {
                if (Cbl_college.Items[row].Selected == true)
                {
                    if (college == "")
                    {
                        college = Cbl_college.Items[row].Value;
                    }
                    else
                    {
                        college = college + "," + Cbl_college.Items[row].Value;
                    }
                }
            }
            clgcode1 = college;

            //bindhostelname();
            //  BindType();

        }

        catch (Exception ex)
        {

        }

    }
    public void bindclg()
    {
        try
        {
            ds.Clear();
            Cbl_college.Items.Clear();
            string clgname = "select cp.college_code,cf.collname from collegeprivilages cp,collinfo cf where user_code=" + Session["usercode"] + " and cp.college_code=cf.college_code";
            ds = d2.select_method_wo_parameter(clgname, "Text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                Cbl_college.DataSource = ds;
                Cbl_college.DataTextField = "collname";
                Cbl_college.DataValueField = "college_code";
                Cbl_college.DataBind();
            }
            if (Cbl_college.Items.Count > 0)
            {
                for (int row = 0; row < Cbl_college.Items.Count; row++)
                {
                    Cbl_college.Items[row].Selected = true;
                    Cb_college.Checked = true;
                }
                Txt_college.Text = "College(" + Cbl_college.Items.Count + ")";

                string college = "";
                for (int row = 0; row < Cbl_college.Items.Count; row++)
                {
                    if (Cbl_college.Items[row].Selected == true)
                    {
                        if (college == "")
                        {
                            college = Cbl_college.Items[row].Value;
                        }
                        else
                        {
                            college = college + "'" + "," + "'" + Cbl_college.Items[row].Value;
                        }
                    }
                }
                clgcode1 = college;
            }

            else
            {

                Txt_college.Text = "--Select--";
            }
        }
        catch (Exception ex)
        {
        }
    }



    public void Cb_college1_CheckedChanged(object sender, EventArgs e)
    {
        try
        {
            int cout = 0;
            Txt_college1.Text = "--Select--";
            if (Cb_college1.Checked == true)
            {
                cout++;
                for (int i = 0; i < Cbl_college1.Items.Count; i++)
                {
                    Cbl_college1.Items[i].Selected = true;
                }
                Txt_college1.Text = "College(" + (Cbl_college1.Items.Count) + ")";
            }
            else
            {
                for (int i = 0; i < Cbl_college1.Items.Count; i++)
                {
                    Cbl_college1.Items[i].Selected = false;
                }
                Txt_college1.Text = "--Select--";
            }
            string college = "";
            for (int row = 0; row < Cbl_college1.Items.Count; row++)
            {
                if (Cbl_college1.Items[row].Selected == true)
                {
                    if (college == "")
                    {
                        college = Cbl_college1.Items[row].Value;
                    }
                    else
                    {

                        college = college + "'" + "," + "'" + Cbl_college1.Items[row].Value;
                    }
                }
            }

            clgcode = college;
            BindType();
        }


        catch (Exception ex)
        {

        }
    }
    public void Cbl_college1_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {

            int commcount = 0;
            Txt_college1.Text = "--Select--";
            Cb_college1.Checked = false;

            for (int i = 0; i < Cbl_college1.Items.Count; i++)
            {
                if (Cbl_college1.Items[i].Selected == true)
                {
                    commcount = commcount + 1;
                    Cb_college1.Checked = false;
                }
            }
            if (commcount > 0)
            {
                if (commcount == Cbl_college1.Items.Count)
                {

                    Cb_college1.Checked = true;
                }
                Txt_college1.Text = "College(" + commcount.ToString() + ")";
                string college = "";
                for (int row = 0; row < Cbl_college1.Items.Count; row++)
                {
                    if (Cbl_college1.Items[row].Selected == true)
                    {
                        if (college == "")
                        {
                            college = Cbl_college1.Items[row].Value;
                        }
                        else
                        {
                            college = college + "'" + "," + "'" + Cbl_college1.Items[row].Value;
                        }
                    }
                }
                clgcode = college;
            }
            //bindhostelname();
            BindType();

        }

        catch (Exception ex)
        {

        }
    }
    public void bindclg1()
    {
        try
        {
            ds.Clear();
            Cbl_college1.Items.Clear();
            string clgname = "select cp.college_code,cf.collname from collegeprivilages cp,collinfo cf where user_code=" + Session["usercode"] + " and cp.college_code=cf.college_code";
            ds = d2.select_method_wo_parameter(clgname, "Text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                Cbl_college1.DataSource = ds;
                Cbl_college1.DataTextField = "collname";
                Cbl_college1.DataValueField = "college_code";
                Cbl_college1.DataBind();
            }
            if (Cbl_college1.Items.Count > 0)
            {
                for (int row = 0; row < Cbl_college1.Items.Count; row++)
                {
                    Cbl_college1.Items[row].Selected = true;
                    Cb_college1.Checked = true;
                }
                Txt_college1.Text = "College(" + Cbl_college1.Items.Count + ")";

                string college = "";
                for (int row = 0; row < Cbl_college1.Items.Count; row++)
                {
                    if (Cbl_college1.Items[row].Selected == true)
                    {
                        if (college == "")
                        {
                            college = Cbl_college1.Items[row].Value;
                        }
                        else
                        {

                            college = college + "'" + "," + "'" + Cbl_college1.Items[row].Value;
                        }
                    }
                }

                clgcode = college;
            }

            else
            {

                Txt_college1.Text = "--Select--";
            }
        }
        catch (Exception ex)
        {
        }
    }
    public void mark()
    {

        string type = "";
        int typecode = 0;
        for (int i = 0; i < cbl_Type.Items.Count; i++)
        {
            if (cbl_Type.Items[i].Selected == true)
            {
                if (type == "")
                {
                    type = "" + cbl_Type.Items[i].Value.ToString() + "";
                    typecode++;
                }
                else
                {
                    type = type + "','" + cbl_Type.Items[i].Value.ToString() + "";
                }
            }
        }


        string college = "";
        if (Cbl_college1.Items.Count > 0)
        {
            for (int i = 0; i < Cbl_college1.Items.Count; i++)
            {
                if (Cbl_college1.Items[i].Selected == true)
                {
                    if (college == "")
                    {
                        college = Convert.ToString(Cbl_college1.Items[i].Value);
                    }
                    else
                    {
                        college = college + "','" + Convert.ToString(Cbl_college1.Items[i].Value);
                    }
                }
            }



        }

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
        FpSpread1.Visible = true;

        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 0].Text = "S.No";
        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 0].Font.Bold = true;
        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 0].HorizontalAlign = HorizontalAlign.Center;
        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 0].Font.Name = "Book Antiqua";
        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 0].Font.Size = FontUnit.Medium;
        FpSpread1.Sheets[0].Columns[0].HorizontalAlign = HorizontalAlign.Center;


        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 1].Text = "Type";
        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 1].Font.Bold = true;
        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 1].Font.Name = "Book Antiqua";
        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 1].Font.Size = FontUnit.Medium;
        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 1].HorizontalAlign = HorizontalAlign.Center;


        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 2].Text = "Abbreviation";
        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 2].Font.Bold = true;
        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 2].Font.Name = "Book Antiqua";
        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 2].Font.Size = FontUnit.Medium;
        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 2].HorizontalAlign = HorizontalAlign.Center;

        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 3].Text = "Points";
        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 3].Font.Bold = true;
        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 3].Font.Name = "Book Antiqua";
        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 3].Font.Size = FontUnit.Medium;
        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 3].HorizontalAlign = HorizontalAlign.Center;

        FpSpread1.Sheets[0].ColumnHeader.Columns[0].Width = 59;
        FpSpread1.Sheets[0].ColumnHeader.Columns[1].Width = 144;
        FpSpread1.Sheets[0].ColumnHeader.Columns[2].Width = 128;

        ////ds.Clear();

        string selqry = "";
        string txttype = "";
        txttype = txt_search.Text.ToString();
        if (txt_search.Text != "")
        {
            selqry = " SELECT MarkMasterPK,MarkTypeAcr, MarkType , Point, CollegeCode FROM CO_MarkMaster WHERE  MarkType= ('" + txttype + "') and CollegeCode in ('" + college + "')  order by Point desc";
        }
        else if (typecode > 0)
        {
            selqry = " SELECT MarkMasterPK,MarkTypeAcr, MarkType ,CollegeCode, Point FROM CO_MarkMaster WHERE   MarkType in ('" + type + "') and CollegeCode in ( '" + college + "')  order by Point desc";
        }
        if (selqry != "")
        {
            ds = d2.select_method_wo_parameter(selqry, "Text");
        }
        if (ds.Tables.Count > 0)
        {
            if (ds.Tables[0].Rows.Count > 0)
            {

                //FpSpread1.Sheets[0].Rows.Count = ds.Tables[0].Rows.Count; 
                DataView dv = new DataView();
                FpSpread1.Sheets[0].RowCount = 0;
                for (int i = 0; i < Cbl_college1.Items.Count; i++)
                {
                    if (Cbl_college1.Items[i].Selected == true)
                    {
                        ds.Tables[0].DefaultView.RowFilter = "CollegeCode='" + Cbl_college1.Items[i].Value + "'";
                        dv = ds.Tables[0].DefaultView;
                        if (dv.Count > 0)
                        {
                            FpSpread1.Sheets[0].RowCount++;
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Text = Convert.ToString(Cbl_college.Items[i].Text);
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
                            FpSpread1.Sheets[0].SpanModel.Add(FpSpread1.Sheets[0].RowCount - 1, 0, 1, FpSpread1.Sheets[0].ColumnCount);
                            int sno = 1;
                            for (int ik = 0; ik < dv.Count; ik++)
                            {
                                FpSpread1.Sheets[0].RowCount++;
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Text = Convert.ToString(sno++);

                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].Text = dv[ik]["MarkType"].ToString();
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].Tag = dv[ik]["MarkMasterPK"].ToString();

                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].Text = dv[ik]["MarkTypeAcr"].ToString();

                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 3].Text = dv[ik]["Point"].ToString();
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 3].Tag = dv[ik]["CollegeCode"].ToString();

                            }
                        }
                    }
                }

                FpSpread1.Sheets[0].Columns[3].HorizontalAlign = HorizontalAlign.Center;
                FpSpread1.SaveChanges();
                FpSpread1.Sheets[0].PageSize = FpSpread1.Sheets[0].RowCount;
                txt_search.Text = "";
            }
            else
            {
                imgdiv3.Visible = true;
                lbl_alert.Text = "No Records Found";
                FpSpread1.Visible = false;
                div1.Visible = false;
                rptprint1.Visible = false;
            }

        }
        else
        {
            lbl_alert.Text = "No Records Found";
            imgdiv3.Visible = true;
            div1.Visible = false;
            rptprint1.Visible = false;
        }
    }

    protected void btnSearch_Click(object sender, EventArgs e)
    {
        rptprint1.Visible = true;
        div1.Visible = true;
        mark();
    }

    protected void btn_AddNew_Click(object sender, EventArgs e)
    {
        btn_Save.Text = "Save";
        btndel.Visible = false;
        Addmark.Visible = true;
        Txt_college.Enabled = true;
        txt_type2.Text = "";
        txt_abtn.Text = "";
        txt_point.Text = "";
        bindclg();

    }

    public void cb_Type_CheckedChanged(object sender, EventArgs e)
    {
        try
        {
            int count = 0;
            Txt_Type.Text = "--Select--";
            if (cb_Type.Checked == true)
            {
                count++;
                for (int i = 0; i < cbl_Type.Items.Count; i++)
                {
                    cbl_Type.Items[i].Selected = true;
                }
                Txt_Type.Text = "Type(" + (cbl_Type.Items.Count) + ")";


            }
            else
            {
                for (int i = 0; i < cbl_Type.Items.Count; i++)
                {
                    cbl_Type.Items[i].Selected = false;
                }
                Txt_Type.Text = "--Select--";
            }


        }
        catch (Exception ex)
        {

        }

    }
    public void cbl_Type_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            int commcount = 0;
            string buildvalue = "";
            string build = "";
            cb_Type.Checked = false;
            Txt_Type.Text = "--Select--";


            for (int i = 0; i < cbl_Type.Items.Count; i++)
            {
                if (cbl_Type.Items[i].Selected == true)
                {
                    commcount = commcount + 1;
                    //cb_Type.Checked = false;
                    build = cbl_Type.Items[i].Value.ToString();
                    if (buildvalue == "")
                    {
                        buildvalue = build;
                    }
                    else
                    {
                        buildvalue = buildvalue + "'" + "," + "'" + build;
                    }

                }

            }


            if (commcount > 0)
            {
                Txt_Type.Text = "Type(" + commcount.ToString() + ")";
                if (commcount == cbl_Type.Items.Count)
                {
                    cb_Type.Checked = true;
                }
                Txt_Type.Text = "Type(" + commcount.ToString() + ")";
            }

        }
        catch (Exception ex)
        {

        }
    }
    public void BindType()
    {
        try
        {
            string college = "";
            for (int row = 0; row < Cbl_college1.Items.Count; row++)
            {
                if (Cbl_college1.Items[row].Selected == true)
                {
                    if (college == "")
                    {
                        college = Cbl_college1.Items[row].Value;
                    }
                    else
                    {
                        college = college + "," + Cbl_college1.Items[row].Value;
                    }
                }
            }

            Txt_Type.Text = "--Select--";
            cbl_Type.Items.Clear();
            cb_Type.Checked = false;
            if (college.Trim() != "")
            {

                string selqry = " SELECT  distinct (MarkType) FROM CO_MarkMaster WHERE  CollegeCode in (" + college + ")";
                ds = d2.select_method_wo_parameter(selqry, "Text");
                if (ds.Tables[0].Rows.Count > 0)
                {
                    cbl_Type.DataSource = ds;
                    cbl_Type.DataTextField = "MarkType";
                    cbl_Type.DataValueField = "MarkType";
                    cbl_Type.DataBind();
                }
                if (cbl_Type.Items.Count > 0)
                {
                    for (int row = 0; row < cbl_Type.Items.Count; row++)
                    {
                        cbl_Type.Items[row].Selected = true;
                        cb_Type.Checked = true;
                    }
                    Txt_Type.Text = "Type(" + cbl_Type.Items.Count + ")";

                }

                else
                {

                    Txt_Type.Text = "--Select--";
                }
            }

        }
        catch
        {
        }

    }



    protected void btn_errorclose1_Click(object sender, EventArgs e)
    {
        imgdiv3.Visible = false;
    }

    protected void btn_save_Click(object sender, EventArgs e)
    {
        string college_cd = "";
        if (Cbl_college.Items.Count > 0)
        {
            for (int i = 0; i < Cbl_college.Items.Count; i++)
            {
                if (Cbl_college.Items[i].Selected == true)
                {
                    if (college_cd == "")
                    {
                        college_cd = Convert.ToString(Cbl_college.Items[i].Value);
                    }
                    else
                    {
                        college_cd = college_cd + "','" + Convert.ToString(Cbl_college.Items[i].Value);
                    }
                }
            }
        }
        if (college_cd != "")
        {
            string abbreviation = txt_abtn.Text.ToUpper();

            string type = txt_type2.Text;
            string point = txt_point.Text;

            type = System.Threading.Thread.CurrentThread.CurrentCulture.TextInfo.ToTitleCase(type);
            abbreviation = System.Threading.Thread.CurrentThread.CurrentCulture.TextInfo.ToTitleCase(abbreviation);

            if (btn_Save.Text.Trim().ToLower() == "save")
            {
                for (int a = 0; a < Cbl_college.Items.Count; a++)
                {
                    if (Cbl_college.Items[a].Selected == true)
                    {
                        string insert2 = "insert into CO_MarkMaster (MarkTypeAcr, MarkType , Point, CollegeCode ) values ('" + abbreviation + "','" + type + "','" + point + "','" + Cbl_college.Items[a].Value + "')";
                        int insertvalue2 = d2.update_method_wo_parameter(insert2, "Text");
                    }
                }

                imgdiv3.Visible = true;
                lbl_alert.Visible = true;
                lbl_alert.Text = "Saved Successfully";

            }
            else if (btn_Save.Text.Trim().ToLower() == "update")
            { // update CO_MarkMaster set MarkType='type',MarkTypeAcr='abbreviation',Point='point' where MarkMasterpk=''"+value+"'' and collegecode='" + collegecode1 + "'

                string activerow = FpSpread1.ActiveSheetView.ActiveRow.ToString();
                string activecol = FpSpread1.ActiveSheetView.ActiveColumn.ToString();
                int value = Convert.ToInt32(FpSpread1.Sheets[0].Cells[Convert.ToInt32(activerow), 1].Tag.ToString());
                int college = Convert.ToInt32(FpSpread1.Sheets[0].Cells[Convert.ToInt32(activerow), 3].Tag.ToString());

                string sql = " update CO_MarkMaster set MarkType='" + type + "',MarkTypeAcr='" + abbreviation + "',Point='" + point + "' where MarkMasterpk='" + value + "' and collegecode='" + college_cd + "'";
                int insertvalue2 = d2.update_method_wo_parameter(sql, "Text");

                imgdiv3.Visible = true;
                lbl_alert.Visible = true;
                lbl_alert.Text = "Updated Successfully";
                Addmark.Visible = false;
            }


            else
            {
                imgdiv3.Visible = true;
                lbl_alert.Text = "Please Enter value";
                lbl_alert.ForeColor = Color.Red;

            }
            BindType();
            mark();
            txt_type2.Text = "";
            txt_abtn.Text = "";
            txt_point.Text = "";
            btnSearch_Click(sender, e);
            bindclg();

            // update CO_MarkMaster set MarkType='',MarkTypeAcr='',Point='' where MarkMasterpk='' and collegecode=''
        }
        else
        {
            imgdiv3.Visible = true;
            lbl_alert.Visible = true;
            lbl_alert.Text = "Please Select College";
        }
    }
    protected void imagebtnpopclose1_Click(object sender, EventArgs e)
    {
        Addmark.Visible = false;
    }

    protected void lb3_Click(object sender, EventArgs e)
    {
        Session.Abandon();
        Session.Clear();
        Session.RemoveAll();
        System.Web.Security.FormsAuthentication.SignOut();
        Response.Redirect("~/Default.aspx", false);
    }
    protected void FpSpread1_OnCellClick(object sender, EventArgs e)
    {
        string activerow = FpSpread1.ActiveSheetView.ActiveRow.ToString();
        string activecol = FpSpread1.ActiveSheetView.ActiveColumn.ToString();
        cellclick = true;
        FpSpread1.SaveChanges();

    }
    protected void FpSpread1_Selectedindexchange(object sender, EventArgs e)
    {
        try
        {
            if (cellclick == true)
            {

                string activerow = FpSpread1.ActiveSheetView.ActiveRow.ToString();
                string activecol = FpSpread1.ActiveSheetView.ActiveColumn.ToString();

                string type = FpSpread1.Sheets[0].Cells[Convert.ToInt32(activerow), 1].Text.ToString();
                if (type.Trim() != "")
                {
                    string Abbreviation = FpSpread1.Sheets[0].Cells[Convert.ToInt32(activerow), 2].Text.ToString();
                    string Points = FpSpread1.Sheets[0].Cells[Convert.ToInt32(activerow), 3].Text.ToString();
                    string value = FpSpread1.Sheets[0].Cells[Convert.ToInt32(activerow), 1].Tag.ToString();
                    string college = FpSpread1.Sheets[0].Cells[Convert.ToInt32(activerow), 3].Tag.ToString();


                    txt_type2.Text = type;
                    txt_abtn.Text = Abbreviation;
                    txt_point.Text = Points;
                    Txt_college.Enabled = false;
                    int count2 = 0;
                    for (int i = 0; i < Cbl_college.Items.Count; i++)
                    {
                        Cbl_college.Items[i].Selected = false;
                    }
                    for (int i = 0; i < Cbl_college.Items.Count; i++)
                    {
                        if (Cbl_college.Items[i].Value.ToString() == college)
                        {
                            Cbl_college.Items[i].Selected = true;
                            Cb_college.Checked = false;
                            count2 = count2 + 1;
                        }
                        Txt_college.Text = "College(" + count2.ToString() + ")";
                    }
                    btn_Save.Text = "Update";
                    btndel.Visible = true;
                    Addmark.Visible = true;
                }

            }


        }
        catch
        {

        }

    }


    public void btndel_Click(object sender, EventArgs e)
    {
        img4.Visible = true;
        lbl_warning_alert.Visible = true;
        lbl_warning_alert.Text = "Are You Sure You Want Delete?";

        //int savecc = 0;
        //FpSpread1.SaveChanges();
        //string activerow = FpSpread1.ActiveSheetView.ActiveRow.ToString();
        //string sql = "delete  from CO_MarkMaster where  MarkMasterpk = '" + FpSpread1.Sheets[0].Cells[Convert.ToInt32(activerow), 1].Tag.ToString() + "' and collegecode='" + collegecode1 + "'";
        //int qry = d2.update_method_wo_parameter(sql, "Text");
        //savecc++;
        //if (savecc > 0)
        //{
        //    bindclg1();
        //    BindType();
        //    lbl_alert.Text = "Deleted Successfully";
        //    lbl_alert.Visible = true;
        //    imgdiv3.Visible = true;
        //    mark();
        //    txt_type2.Text = "";
        //    txt_abtn.Text = "";
        //    txt_point.Text = "";
        //}
        //Addmark.Visible = false;
    }

    public void btn_warningmsg_Click(object sender, EventArgs e)
    {
        img4.Visible = false;
        lbl_warning_alert.Text = "Are you sure you want delete";

        int savecc = 0;
        FpSpread1.SaveChanges();
        string activerow = FpSpread1.ActiveSheetView.ActiveRow.ToString();
        string sql = "delete  from CO_MarkMaster where  MarkMasterpk = '" + FpSpread1.Sheets[0].Cells[Convert.ToInt32(activerow), 1].Tag.ToString() + "' ";
        int qry = d2.update_method_wo_parameter(sql, "Text");
        savecc++;
        if (savecc > 0)
        {
            bindclg1();
            BindType();
            lbl_alert.Text = "Deleted Successfully";
            lbl_alert.Visible = true;
            imgdiv3.Visible = true;
            mark();
            txt_type2.Text = "";
            txt_abtn.Text = "";
            txt_point.Text = "";
        }
        Addmark.Visible = false;

    }
    protected void btn_warning_exit_Click(object sender, EventArgs e)
    {
        img4.Visible = false;

    }
    protected void btn_errorclose_Click(object sender, EventArgs e)
    {
        imgdiv2.Visible = false;
    }


    [WebMethod]
    public static string CheckUserName(string StoreName)
    {
        string returnValue = "1";
        try
        {
            DAccess2 dd = new DAccess2();
            string user_name = StoreName;
            if (user_name.Trim() != "" && user_name != null)
            {
                string query = dd.GetFunction("select distinct MarkType,MarkMasterpk from CO_MarkMaster  where CollegeCode in('" + clgcode1 + "') and MarkType ='" + user_name + "'");
                if (query.Trim() == "" || query == null || query == "0" || query == "-1")
                {
                    returnValue = "0";
                }

            }
            else
            {
                returnValue = "2";
            }
        }
        catch (SqlException ex)
        {
            returnValue = "error" + ex.ToString();
        }
        return returnValue;
    }




    [System.Web.Services.WebMethod]
    [System.Web.Script.Services.ScriptMethod()]
    public static List<string> Type(string prefixText)
    {
        WebService ws = new WebService();
        List<string> name = new List<string>();
        string query = "SELECT  distinct (MarkType), MarkMasterPK  FROM CO_MarkMaster where CollegeCode in ('" + clgcode + "') and MarkType like '" + prefixText + "%'";

        name = ws.Getname(query);

        return name;
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
            string dptname = "TypeMaster";
            string pagename = "Type_Master.aspx";

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




    protected void btn_exit_Click(object sender, EventArgs e)
    {

    }


}
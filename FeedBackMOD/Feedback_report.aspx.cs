using System;
using System.Web;
using System.Web.UI.WebControls;
using System.Data;
using System.Collections;
using System.Drawing;
using System.IO;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System.Web.UI.DataVisualization.Charting;
using Gios.Pdf;
using System.Text;

public partial class Feedback_report : System.Web.UI.Page
{
    bool cellclick = false;
    bool cellclk = false;
    string usercode = string.Empty;
    string collegecode1 = string.Empty;
    string collegecode = string.Empty;
    string singleuser = string.Empty;
    string group_user = string.Empty;
    string college = "";
    DAccess2 d2 = new DAccess2();
    DataSet ds = new DataSet();
    DataSet ds1 = new DataSet();
    DataSet dsfb = new DataSet();
    ReuasableMethods rs = new ReuasableMethods();

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
        if (!IsPostBack)
        {
            anonymousfilter1.Visible = true;
            anonymousfilter2.Visible = true;
            anonymousfilter3.Visible = false;
            bindclg();
            BindBatch();
            BindDegree();
            bindbranch();
            bindformate6dept();
            bindformate6staff();
            bindformate6feedback();
            bindsem();
            bindsec();
            bindfeedback();
            //Subject();
            bindstafftype();
            load_staffname();
            load_questions();
            rb_farmate1_CheckedChanged(sender, e);
            lbl_headig.Visible = false;
            // string top_point = d2.GetFunction("select top 1 Point from CO_MarkMaster  where CollegeCode='13' order by Point desc");
            ddlstaff();
            stfSubject();
            lbl_stfsubject.Visible = false;
            UpdatePanel17.Visible = false;
            Txt_stfSubject.Visible = false;
            cb_avgcolumn.Visible = true;
            btn_printperticulaterstaff.Visible = true;
            bindformate6sem();
            bindsectionDeptWise();
            if (rb_login.Checked == true)
            {
                ddl_Loginbasec.Visible = true;
                ddl_Anonyomous.Visible = false;
                btn_goanonymous.Visible = false;

            }


        }
    }

    public void bindfeedback()
    {
        //string feedback = "";
        // ds.Clear();
        try
        {
            {
                ds.Clear();
                string Batch_Year = "";
                for (int i = 0; i < cbl_batch.Items.Count; i++)
                {
                    if (cbl_batch.Items[i].Selected == true)
                    {
                        if (Batch_Year == "")
                        {
                            Batch_Year = "" + cbl_batch.Items[i].Value.ToString() + "";
                        }
                        else
                        {
                            Batch_Year = Batch_Year + "','" + cbl_batch.Items[i].Value.ToString() + "";
                        }
                    }
                }
                string college_cd = "";
                if (Cbl_college.Items.Count > 0)
                {
                    for (int i = 0; i < Cbl_college.Items.Count; i++)
                    {
                        if (Cbl_college.Items[i].Selected == true)
                        {
                            if (college_cd == "")
                            {
                                college_cd = "" + Cbl_college.Items[i].Value.ToString() + "";
                            }
                            else
                            {
                                college_cd = college_cd + "','" + Convert.ToString(Cbl_college.Items[i].Value);
                            }
                        }
                    }
                }
                string degree_code = "";
                for (int i = 0; i < cbl_branch.Items.Count; i++)
                {
                    if (cbl_branch.Items[i].Selected == true)
                    {
                        if (degree_code == "")
                        {
                            degree_code = "" + cbl_branch.Items[i].Value.ToString() + "";
                        }
                        else
                        {
                            degree_code = degree_code + "','" + cbl_branch.Items[i].Value.ToString() + "";
                        }
                    }
                }
                string semester = "";
                for (int i = 0; i < cbl_sem.Items.Count; i++)
                {
                    if (cbl_sem.Items[i].Selected == true)
                    {
                        if (semester == "")
                        {
                            semester = "" + cbl_sem.Items[i].Value.ToString() + "";
                        }
                        else
                        {
                            semester = semester + "','" + cbl_sem.Items[i].Value.ToString() + "";
                        }
                    }
                }
                string section = "";
                for (int i = 0; i < cbl_sec.Items.Count; i++)
                {
                    if (cbl_sec.Items[i].Selected == true)
                    {
                        if (section == "")
                        {
                            section = "" + cbl_sec.Items[i].Value.ToString() + "";
                        }
                        else
                        {
                            section = section + "','" + cbl_sec.Items[i].Value.ToString() + "";
                        }
                        if (cbl_sec.Items[i].Value == "Empty")
                        {
                            section = "";
                        }
                    }
                }
                if (section.Trim() != "")
                {
                    section = section + "','";
                }
                string type = "";
                ddl_Feedbackname.Items.Clear();
                string FBname = "";
                if (rb_Acad.Checked == true)
                {
                    type = "1";
                    FBname = "select distinct  FeedBackName  from CO_FeedBackMaster where FeedBackType ='" + type + "' and CollegeCode in ('" + college_cd + "')  and DegreeCode in ('" + degree_code + "') and Batch_Year in ('" + Batch_Year + "') and semester in ('" + semester + "') and Section in ('" + section + "') ";
                }
                else if (rb_Gend.Checked == true)
                {
                    type = "2";
                    FBname = "select  distinct  FeedBackName  from CO_FeedBackMaster where FeedBackType ='" + type + "' and CollegeCode in ('" + college_cd + "') ";
                }
                ds.Clear();
                ds = d2.select_method_wo_parameter(FBname, "Text");
                ddl_Feedbackname.Items.Clear();
                if (ds.Tables[0].Rows.Count > 0)
                {
                    ddl_Feedbackname.DataSource = ds;
                    ddl_Feedbackname.DataTextField = "FeedBackName";
                    ddl_Feedbackname.DataValueField = "FeedBackName";
                    ddl_Feedbackname.DataBind();
                    ddl_Feedbackname.Items.Insert(0, "Select");
                }
                else
                {
                    ddl_Feedbackname.Items.Clear();
                    ddl_Feedbackname.Items.Insert(0, "Select");
                }
            }
        }
        catch (Exception ex)
        {
            d2.sendErrorMail(ex, collegecode1, "FeedbackReport");
        }
    }

    protected void rb_Acad1_CheckedChanged(object sender, EventArgs e)
    {
        anonymousfilter1.Visible = true;
        anonymousfilter2.Visible = true;
        anonymousfilter3.Visible = false;
        chartprint.Visible = false;
        lbl_headig.Text = "";
        Acd.Visible = true;
        gend.Visible = false;
        lbl_subject.Enabled = true;
        Txt_Subject.Enabled = true;
        staff.Visible = false;
        Acad.Visible = true;
        rb_farmate1.Checked = true;
        rb_farmate2.Checked = false;
        rb_farmate3.Checked = false;
        rb_farmate4.Checked = false;
        rb_farmate5.Checked = false;
        rb_farmate6.Checked = false;
        rb_linchart.Visible = false;
        rb_barchart.Visible = false;
        acd_comman_login.Visible = true;
        bindfeedback();
        visibletr();
        rb_login.Checked = true;
        rb_anonymous.Checked = false;
        ind_login.Visible = false;//delsi
        btn_go.Visible = true;
        staff_chart.Visible = false;
    }

    protected void rb_Gend1_CheckedChanged(object sender, EventArgs e)
    {
        anonymousfilter1.Visible = true;
        anonymousfilter2.Visible = true;
        anonymousfilter3.Visible = false;
        chartprint.Visible = false;
        chartfalse();
        Acad.Visible = true;
        Acd.Visible = false;
        gend.Visible = true;
        lbl_subject.Enabled = false;
        Txt_Subject.Enabled = false;
        rb_gen_farmate1.Checked = true;
        rb_gen_farmate2.Checked = false;
        acd_comman_login.Visible = false;
        bindfeedback();
        // visiblefalse();
        acd_anonyms.Visible = false;
        staff_chart.Visible = false;
        Total_points.Visible = false;
        question_chart.Visible = false;
        lbl_headig.Visible = true;
        lbl_headig.Text = " Questionwise Total Points";
    }

    protected void rb_gndfarmate2_CheckedChanged(object sender, EventArgs e)
    {
    }

    protected void rb_gndfarmate1_CheckedChanged(object sender, EventArgs e)
    {
    }

    protected void rb_farmate1_CheckedChanged(object sender, EventArgs e)
    {
        rdb_form4staffwise.Visible = false;
        rdb_form4questwise.Visible = false;
        anonymousfilter1.Visible = true;
        anonymousfilter2.Visible = true;
        anonymousfilter3.Visible = false;
        txtfrom_range.Text = "";
        txtto_range.Text = "";
        ddl_criter.SelectedIndex = 0;
        Total_points.Visible = false;
        rptprint1.Visible = false;
        chartprint.Visible = false;
        question_chart.Visible = false;
        staff_chart.Visible = false;
        FpSpread1.Visible = false;
        FpSpread2.Visible = false;
        FpSpread3.Visible = false;
        FpSpread4.Visible = false;
        //format1();
        lbl_headig.Visible = true;
        lbl_headig.Text = "Staff Wise Report";
        rb_linchart.Visible = false;
        rb_barchart.Visible = false;
        chartfalse();
        format2false();
        lbl_subject.Visible = false;
        UpdatePanel5.Visible = false;
        lbl_stfsubject.Visible = false;
        UpdatePanel17.Visible = false;
        Txt_stfSubject.Visible = false;
        cb_avgcolumn.Visible = false;
        btn_printperticulaterstaff.Visible = true;
        ddl_headershow.Visible = false;
        lbl_header.Visible = false;
    }

    protected void rb_farmate2_CheckedChanged(object sender, EventArgs e)
    {
        rdb_form4staffwise.Visible = false;
        rdb_form4questwise.Visible = false;
        anonymousfilter1.Visible = true;
        anonymousfilter2.Visible = true;
        anonymousfilter3.Visible = false;
        txtfrom_range.Text = "";
        txtto_range.Text = "";
        ddl_criter.SelectedIndex = 0;
        Total_points.Visible = false;
        rptprint1.Visible = false;
        chartprint.Visible = false;
        question_chart.Visible = false;
        staff_chart.Visible = false;
        FpSpread1.Visible = false;
        FpSpread2.Visible = false;
        FpSpread3.Visible = false;
        FpSpread4.Visible = false;
        lbl_headig.Visible = true;
        rb_linchart.Visible = false;
        rb_barchart.Visible = false;
        chartfalse();
        rb_subject.Visible = true;
        rb_type.Visible = true;
        cb_total.Visible = true;
        cb_avg.Visible = true;
        cb_total.Checked = true;
        cb_avg.Checked = true;
        lbl_subject.Visible = true;
        UpdatePanel5.Visible = true;
        form_2.Visible = true;
        Panel_Subject.Visible = true;
        Txt_Subject.Visible = true;
        lbl_stfsubject.Visible = false;
        UpdatePanel17.Visible = false;
        Txt_stfSubject.Visible = false;
        cb_avgcolumn.Visible = false;
        btn_printperticulaterstaff.Visible = false;
        ddl_headershow.Visible = true;
        lbl_header.Visible = true;
    }

    protected void rb_subject_CheckedChanged(object sender, EventArgs e)
    {
        cb_total.Visible = true;
        cb_avg.Visible = true;
        // format2();
    }

    protected void rb_type_CheckedChanged(object sender, EventArgs e)
    {
        cb_total.Visible = true;
        cb_avg.Visible = true;
        // format2a();
    }

    protected void rb_farmate3_CheckedChanged(object sender, EventArgs e)
    {
        rdb_form4staffwise.Visible = false;
        rdb_form4questwise.Visible = false;
        anonymousfilter1.Visible = true;
        anonymousfilter2.Visible = true;
        anonymousfilter3.Visible = false;
        txtfrom_range.Text = "";
        txtto_range.Text = "";
        ddl_criter.SelectedIndex = 0;
        Total_points.Visible = false;
        rptprint1.Visible = false;
        chartprint.Visible = false;
        question_chart.Visible = false;
        staff_chart.Visible = false;
        FpSpread1.Visible = false;
        FpSpread2.Visible = false;
        FpSpread3.Visible = false;
        FpSpread4.Visible = false;
        lbl_headig.Visible = true;
        format2false();
        rb_linchart.Visible = false;
        rb_barchart.Visible = false;
        chartfalse();
        lbl_subject.Visible = false;
        UpdatePanel5.Visible = false;
        lbl_stfsubject.Visible = false;
        UpdatePanel17.Visible = false;
        Txt_stfSubject.Visible = false;
        cb_avgcolumn.Visible = false;
        btn_printperticulaterstaff.Visible = false;
        ddl_headershow.Visible = false;
        lbl_header.Visible = false;
    }

    protected void rb_login_CheckedChanged(object sender, EventArgs e)
    {
        btn_goanonymous.Visible = false;
        rdb_form4staffwise.Visible = false;
        rdb_form4questwise.Visible = false;
        anonymousfilter1.Visible = true;
        anonymousfilter2.Visible = true;
        anonymousfilter3.Visible = false;
        txtfrom_range.Text = "";
        txtto_range.Text = "";
        ddl_criter.SelectedIndex = 0;
        lbl_headig.Text = "";
        chartprint.Visible = false;
        ind_login.Visible = false;
        acd_anonyms.Visible = false;
        btn_go.Visible = true;
        rb_farmate1.Checked = true;
        rb_farmate2.Checked = false;
        rb_farmate3.Checked = false;
        rb_farmate4.Checked = false;
        rb_farmate5.Checked = false;
        rb_farmate6.Checked = false;
        Txt_Subject.Visible = true;
        rb_linchart.Visible = false;
        rb_barchart.Visible = false;
        Total_points.Visible = false;
        rptprint1.Visible = false;
        chartprint.Visible = false;
        question_chart.Visible = false;
        staff_chart.Visible = false;
        FpSpread1.Visible = false;
        FpSpread2.Visible = false;
        FpSpread3.Visible = false;
        FpSpread4.Visible = false;
        chart_staff_chart.Visible = false;
        staff_chart.Visible = false;
        if (rb_login.Checked == true)
        {
            ddl_Loginbasec.Visible = true;
            ddl_Anonyomous.Visible = false;

        }
    }

    protected void rb_anonymous_CheckedChanged(object sender, EventArgs e)
    {

        btn_goanonymous.Visible = true;
        rdb_form4staffwise.Visible = false;
        rdb_form4questwise.Visible = false;
        anonymousfilter1.Visible = true;
        anonymousfilter2.Visible = true;
        anonymousfilter3.Visible = false;
        txtfrom_range.Text = "";
        txtto_range.Text = "";
        ddl_criter.SelectedIndex = 0;
        lbl_headig.Text = "";
        chartprint.Visible = false;
        ind_login.Visible = false;
        // acd_anonyms.Visible = true;s
        btn_go.Visible = false;
        chart_selct.Visible = false;
        //  rb_anonyms_farmate1.Checked = true;
        form_2.Visible = false;
        anonym_form1.Visible = false;
        rb_anonyms_farmate1.Checked = true;
        rb_anonyms_farmate2.Checked = false;
        rb_anonyms_farmate3.Checked = false;
        rb_anonyms_farmate4.Checked = false;
        rb_anonyms_farmate5.Checked = false;
        rb_anonyms_farmate6.Checked = false;
        staff_chart.Visible = false;
        Total_points.Visible = false;
        question_chart.Visible = false;
        Txt_Subject.Visible = true;
        Panel_Subject.Visible = true;
        Total_points.Visible = false;
        rptprint1.Visible = false;
        chartprint.Visible = false;
        question_chart.Visible = false;
        staff_chart.Visible = false;
        FpSpread1.Visible = false;
        FpSpread2.Visible = false;
        FpSpread3.Visible = false;
        FpSpread4.Visible = false;
        chart_staff_chart.Visible = false;
        staff_chart.Visible = false;
        ddl_headershow.Visible = false;
        lbl_header.Visible = false;
        btn_printperticulaterstaff.Visible = false;
        if (rb_anonymous.Checked == true)
        {
            ddl_Loginbasec.Visible = false;
            ddl_Anonyomous.Visible = true;

        }
    }

    protected void rb_cumulative_CheckedChanged(object sender, EventArgs e)
    {
    }

    protected void rb_indiv_CheckedChanged(object sender, EventArgs e)
    {
        txt_indivdgual.Visible = true;
        btn_ind_search.Visible = true;
    }

    protected void lb3_Click(object sender, EventArgs e)
    {
        Session.Abandon();
        Session.Clear();
        Session.RemoveAll();
        System.Web.Security.FormsAuthentication.SignOut();
        Response.Redirect("~/Default.aspx", false);
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
        }
        catch (Exception ex)
        {
        }
        BindBatch();
        BindDegree();
        bindbranch();
        bindsem();
        bindsec();
        bindfeedback();
        //Subject();
        load_staffname();
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
            //bindhostelname();
        }
        catch (Exception ex)
        {
        }
        BindBatch();
        BindDegree();
        bindbranch();
        bindsem();
        bindsec();
        bindfeedback();
        //Subject();
        load_staffname();
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
                cbl_clgnameformat6.DataSource = ds;
                cbl_clgnameformat6.DataTextField = "collname";
                cbl_clgnameformat6.DataValueField = "college_code";
                cbl_clgnameformat6.DataBind();
            }
            if (Cbl_college.Items.Count > 0)
            {
                for (int row = 0; row < Cbl_college.Items.Count; row++)
                {
                    Cbl_college.Items[row].Selected = true;
                    Cb_college.Checked = true;
                }
                Txt_college.Text = "College(" + Cbl_college.Items.Count + ")";
            }
            if (cbl_clgnameformat6.Items.Count > 0)
            {
                for (int row = 0; row < 1; row++)
                {
                    cbl_clgnameformat6.Items[row].Selected = true;
                    cb_clgnameformat6.Checked = true;
                }
                txtclgnameformat6.Text = "College(1)";
            }
            else
            {
                Txt_college.Text = "--Select--";
                txtclgnameformat6.Text = "--Select--";
            }
        }
        catch (Exception ex)
        {
        }
    }

    public void cb_batch_CheckedChanged(object sender, EventArgs e)
    {
        try
        {
            int count = 0;
            txt_batch.Text = "--Select--";
            if (cb_batch.Checked == true)
            {
                count++;
                for (int i = 0; i < cbl_batch.Items.Count; i++)
                {
                    cbl_batch.Items[i].Selected = true;
                }
                txt_batch.Text = "Batch(" + (cbl_batch.Items.Count) + ")";
            }
            else
            {
                for (int i = 0; i < cbl_batch.Items.Count; i++)
                {
                    cbl_batch.Items[i].Selected = false;
                }
                txt_batch.Text = "--Select--";
                // ddl_Feedbackname.Items.Clear();     
            }
            BindDegree();
            bindbranch();
            bindsem();
            bindsec();
            bindfeedback();
            //Subject();
            load_staffname();
        }
        catch (Exception ex)
        {
        }
    }

    public void cbl_batch_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            int commcount = 0;
            string Yearvalue = "";
            string Year = "";
            cb_batch.Checked = false;
            txt_batch.Text = "--Select--";
            for (int i = 0; i < cbl_batch.Items.Count; i++)
            {
                if (cbl_batch.Items[i].Selected == true)
                {
                    commcount = commcount + 1;
                    //cb_batch.Checked = false;
                    Year = cbl_batch.Items[i].Value.ToString();
                    if (Yearvalue == "")
                    {
                        Yearvalue = Year;
                    }
                    else
                    {
                        Yearvalue = Yearvalue + "'" + "," + "'" + Year;
                    }
                }
            }
            if (commcount > 0)
            {
                txt_batch.Text = "Batch(" + commcount.ToString() + ")";
                if (commcount == cbl_batch.Items.Count)
                {
                    cb_batch.Checked = true;
                }
                txt_batch.Text = "Batch(" + commcount.ToString() + ")";
            }
            BindDegree();
            bindbranch();
            bindsem();
            bindsec();
            bindfeedback();
            // Subject();
            load_staffname();
        }
        catch (Exception ex)
        {
        }
    }

    public void BindBatch()
    {
        try
        {
            cbl_batch.Items.Clear();
            cb_batch.Checked = false;
            txt_batch.Text = "--Select--";
            txt_formate6batch.Text = "--Select--";
            string college_cd = "";
            if (Cbl_college.Items.Count > 0)
            {
                for (int i = 0; i < Cbl_college.Items.Count; i++)
                {
                    if (Cbl_college.Items[i].Selected == true)
                    {
                        if (college_cd == "")
                        {
                            college_cd = "" + Cbl_college.Items[i].Value.ToString() + "";
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
                ds = d2.BindBatch();
                if (ds.Tables[0].Rows.Count > 0)
                {
                    cbl_batch.DataSource = ds;
                    cbl_batch.DataTextField = "batch_year";
                    cbl_batch.DataValueField = "batch_year";
                    cbl_batch.DataBind();
                    cbl_formate6batch.DataSource = ds;
                    cbl_formate6batch.DataTextField = "batch_year";
                    cbl_formate6batch.DataValueField = "batch_year";
                    cbl_formate6batch.DataBind();
                }
                if (cbl_batch.Items.Count > 0)
                {
                    for (int row = 0; row < cbl_batch.Items.Count; row++)
                    {
                        cbl_batch.Items[row].Selected = true;
                        cb_batch.Checked = true;
                    }
                    txt_batch.Text = "Batch(" + cbl_batch.Items.Count + ")";
                }
                else
                {
                    txt_batch.Text = "--Select--";
                }
                if (cbl_formate6batch.Items.Count > 0)
                {
                    for (int row = 0; row < cbl_formate6batch.Items.Count; row++)
                    {
                        cbl_formate6batch.Items[row].Selected = true;
                        cb_formate6batch.Checked = true;
                    }
                    txt_formate6batch.Text = "Batch(" + cbl_formate6batch.Items.Count + ")";
                }
                else
                {
                    txt_formate6batch.Text = "--Select--";
                }
            }
            BindDegree();
        }
        catch (Exception ex)
        {
            d2.sendErrorMail(ex, collegecode1, "FeedbackReport");
        }
    }

    public void cb_degree_CheckedChanged(object sender, EventArgs e)
    {
        try
        {
            int count = 0;
            txt_degree.Text = "--Select--";
            if (cb_degree.Checked == true)
            {
                count++;
                for (int i = 0; i < cbl_degree.Items.Count; i++)
                {
                    cbl_degree.Items[i].Selected = true;
                }
                txt_degree.Text = "Degree(" + (cbl_degree.Items.Count) + ")";
            }
            else
            {
                for (int i = 0; i < cbl_degree.Items.Count; i++)
                {
                    cbl_degree.Items[i].Selected = false;
                    //txt_degree.Text = "--Select--";
                    //txtbranch.Text = "--Select--";
                    //chklstbranch.ClearSelection();
                    //chkbranch.Checked = false;
                }
                txt_degree.Text = "--Select--";
            }
            bindbranch();
            bindsem();
            bindsec();
            //Subject();
            bindfeedback();
            load_staffname();
        }
        catch (Exception ex)
        {
        }
    }

    public void cbl_degree_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            int i = 0;
            int commcount = 0;
            cb_degree.Checked = false;
            txt_degree.Text = "--Select--";
            for (i = 0; i < cbl_degree.Items.Count; i++)
            {
                if (cbl_degree.Items[i].Selected == true)
                {
                    commcount = commcount + 1;
                }
            }
            if (commcount > 0)
            {
                if (commcount == cbl_degree.Items.Count)
                {
                    cb_degree.Checked = true;
                }
                txt_degree.Text = "Degree (" + commcount.ToString() + ")";
            }
            bindbranch();
            bindsem();
            bindsec();
            //Subject();
            bindfeedback();
            load_staffname();
        }
        catch (Exception ex)
        {
        }
    }

    public void BindDegree()
    {
        try
        {
            cbl_degree.Items.Clear();
            string college_cd = "";
            if (Cbl_college.Items.Count > 0)
            {
                for (int i = 0; i < Cbl_college.Items.Count; i++)
                {
                    if (Cbl_college.Items[i].Selected == true)
                    {
                        if (college_cd == "")
                        {
                            college_cd = "" + Cbl_college.Items[i].Value.ToString() + "";
                        }
                        else
                        {
                            college_cd = college_cd + "','" + Convert.ToString(Cbl_college.Items[i].Value);
                        }
                    }
                }
            }
            string build = "";
            if (cbl_batch.Items.Count > 0)
            {
                for (int i = 0; i < cbl_batch.Items.Count; i++)
                {
                    if (cbl_batch.Items[i].Selected == true)
                    {
                        if (build == "")
                        {
                            build = Convert.ToString(cbl_batch.Items[i].Value);
                        }
                        else
                        {
                            build = build + "'" + "," + "'" + Convert.ToString(cbl_batch.Items[i].Value);
                        }
                    }
                }
            }
            string query = "";
            if (build != "")
            {
                ds.Clear();
                query = "select distinct degree.course_id,course.course_name from degree,course where course.course_id=degree.course_id and course.college_code = degree.college_code and degree.college_code in ('" + college_cd + "')";
                ds = d2.select_method_wo_parameter(query, "Text");
                // ds = d2.BindDegree(singleuser, group_user, collegecode1, usercode);
                int count1 = ds.Tables[0].Rows.Count;
                if (count1 > 0)
                {
                    cbl_degree.DataSource = ds;
                    cbl_degree.DataTextField = "course_name";
                    cbl_degree.DataValueField = "course_id";
                    cbl_degree.DataBind();
                    if (cbl_degree.Items.Count > 0)
                    {
                        for (int row = 0; row < cbl_degree.Items.Count; row++)
                        {
                            cbl_degree.Items[row].Selected = true;
                        }
                        cb_degree.Checked = true;
                        txt_degree.Text = "Degree(" + cbl_degree.Items.Count + ")";
                    }
                }
            }
            else
            {
                cb_degree.Checked = false;
                txt_degree.Text = "--Select--";
            }
        }
        catch (Exception ex)
        {
            d2.sendErrorMail(ex, collegecode1, "FeedbackReport");
        }
    }

    public void cb_branch_CheckedChanged(object sender, EventArgs e)
    {
        try
        {
            int cout = 0;
            txt_branch.Text = "--Select--";
            if (cb_branch.Checked == true)
            {
                cout++;
                for (int i = 0; i < cbl_branch.Items.Count; i++)
                {
                    cbl_branch.Items[i].Selected = true;
                }
                txt_branch.Text = "Department(" + (cbl_branch.Items.Count) + ")";
            }
            else
            {
                for (int i = 0; i < cbl_branch.Items.Count; i++)
                {
                    cbl_branch.Items[i].Selected = false;
                }
                txt_branch.Text = "--Select--";
            }
            bindsem();
            bindsec();
            //Subject();
            bindfeedback();
            load_staffname();
        }
        catch (Exception ex)
        {
        }
    }

    public void cbl_branch_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            cbl_sem.Items.Clear();
            int commcount = 0;
            cb_branch.Checked = false;
            txt_branch.Text = "--Select--";
            int commcount1 = 0;
            for (int i = 0; i < cbl_branch.Items.Count; i++)
            {
                if (cbl_branch.Items[i].Selected == true)
                {
                    commcount = commcount + 1;
                }
            }
            if (commcount > 0)
            {
                if (commcount == cbl_branch.Items.Count)
                {
                    cb_branch.Checked = true;
                }
                txt_branch.Text = "Department(" + commcount.ToString() + ")";
            }
            bindsem();
            bindsec();
            //Subject();
            bindfeedback();
            load_staffname();
        }
        catch (Exception ex)
        {
        }
    }

    public void bindbranch()
    {
        try
        {
            cbl_branch.Items.Clear();
            string college_cd = "";
            if (Cbl_college.Items.Count > 0)
            {
                for (int i = 0; i < Cbl_college.Items.Count; i++)
                {
                    if (Cbl_college.Items[i].Selected == true)
                    {
                        if (college_cd == "")
                        {
                            college_cd = "" + Cbl_college.Items[i].Value.ToString() + "";
                        }
                        else
                        {
                            college_cd = college_cd + "','" + Convert.ToString(Cbl_college.Items[i].Value);
                        }
                    }
                }
            }
            string course_id = "";
            if (cbl_degree.Items.Count > 0)
            {
                for (int row = 0; row < cbl_degree.Items.Count; row++)
                {
                    if (cbl_degree.Items[row].Selected == true)
                    {
                        if (course_id == "")
                        {
                            course_id = Convert.ToString(cbl_degree.Items[row].Value);
                        }
                        else
                        {
                            course_id = course_id + "," + Convert.ToString(cbl_degree.Items[row].Value);
                        }
                    }
                }
            }
            string query = "";
            if (course_id != "")
            {
                ds.Clear();
                query = " select distinct degree.degree_code,department.dept_name,department.dept_code from degree,department,course where course.course_id=degree.course_id and department.dept_code=degree.dept_code and course.college_code = degree.college_code and department.college_code = degree.college_code and degree.course_id in(" + course_id + ") and degree.college_code in ('" + college_cd + "')";
                ds = d2.select_method_wo_parameter(query, "Text");
                //   ds = d2.BindBranchMultiple(singleuser, group_user, course_id, collegecode1, usercode);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    cbl_branch.DataSource = ds;
                    cbl_branch.DataTextField = "dept_name";
                    cbl_branch.DataValueField = "degree_code";
                    cbl_branch.DataBind();
                    if (cbl_branch.Items.Count > 0)
                    {
                        for (int row = 0; row < cbl_branch.Items.Count; row++)
                        {
                            cbl_branch.Items[row].Selected = true;
                        }
                        cb_branch.Checked = true;
                        txt_branch.Text = "Branch(" + cbl_branch.Items.Count + ")";
                    }
                }
            }
            else
            {
                cb_branch.Checked = false;
                txt_branch.Text = "--Select--";
            }
        }
        catch (Exception ex)
        {
            d2.sendErrorMail(ex, collegecode1, "FeedbackReport");
        }
    }

    public void cb_sem_CheckedChanged(object sender, EventArgs e)
    {
        try
        {
            int cout = 0;
            txt_sem.Text = "--Select--";
            if (cb_sem.Checked == true)
            {
                cout++;
                for (int i = 0; i < cbl_sem.Items.Count; i++)
                {
                    cbl_sem.Items[i].Selected = true;
                }
                txt_sem.Text = "Semester(" + (cbl_sem.Items.Count) + ")";
            }
            else
            {
                for (int i = 0; i < cbl_sem.Items.Count; i++)
                {
                    cbl_sem.Items[i].Selected = false;
                }
            }
            bindsec();
            // Subject();
            bindfeedback();
            load_staffname();
        }
        catch
        {
        }
    }

    public void cbl_sem_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            cb_sem.Checked = false;
            int commcount = 0;
            txt_sem.Text = "--Select--";
            for (int i = 0; i < cbl_sem.Items.Count; i++)
            {
                if (cbl_sem.Items[i].Selected == true)
                {
                    commcount = commcount + 1;
                    cb_sem.Checked = false;
                }
            }
            if (commcount > 0)
            {
                if (commcount == cbl_sem.Items.Count)
                {
                    cb_sem.Checked = true;
                }
                txt_sem.Text = "Semester(" + commcount.ToString() + ")";
            }
            bindsec();
            //Subject();
            bindfeedback();
            load_staffname();
        }
        catch
        {
        }
    }

    public void bindsem()
    {
        cbl_sem.Items.Clear();
        txt_sem.Text = "--Select--";
        Boolean first_year;
        first_year = false;
        int duration = 0;
        int i = 0;
        ds.Clear();
        string branch = "";
        string build = "";
        string batch = "";
        if (cbl_branch.Items.Count > 0)
        {
            for (i = 0; i < cbl_branch.Items.Count; i++)
            {
                if (cbl_branch.Items[i].Selected == true)
                {
                    build = cbl_branch.Items[i].Value.ToString();
                    if (branch == "")
                    {
                        branch = build;
                    }
                    else
                    {
                        branch = branch + "," + build;
                    }
                }
            }
        }
        string college_cd = "";
        if (Cbl_college.Items.Count > 0)
        {
            for (int j = 0; j < Cbl_college.Items.Count; j++)
            {
                if (Cbl_college.Items[j].Selected == true)
                {
                    if (college_cd == "")
                    {
                        college_cd = "" + Cbl_college.Items[j].Value.ToString() + "";
                    }
                    else
                    {
                        college_cd = college_cd + "','" + Convert.ToString(Cbl_college.Items[j].Value);
                    }
                }
            }
        }
        build = "";
        if (cbl_batch.Items.Count > 0)
        {
            for (i = 0; i < cbl_batch.Items.Count; i++)
            {
                if (cbl_batch.Items[i].Selected == true)
                {
                    build = cbl_batch.Items[i].Value.ToString();
                    if (batch == "")
                    {
                        batch = build;
                    }
                    else
                    {
                        batch = batch + "," + build;
                    }
                }
            }
        }
        if (branch.Trim() != "" && batch.Trim() != "")
        {
            //string query = "select distinct Current_Semester from Registration where degree_code in (" + branch + ") and Batch_Year in (" + batch + ") and college_code in ('" + college_cd + "')  and CC=0 and DelFlag =0 and Exam_Flag <>'debar'  order by Current_Semester";
            //ds = d2.select_method_wo_parameter(query, "Text");
            //if (ds.Tables[0].Rows.Count > 0)
            //{
            //    cbl_sem.DataSource = ds;
            //    cbl_sem.DataTextField = "Current_Semester";
            //    cbl_sem.DataBind();
            //    if (cbl_sem.Items.Count > 0)
            //    {
            //        for (int row = 0; row < cbl_sem.Items.Count; row++)
            //        {
            //            cbl_sem.Items[row].Selected = true;
            //            cb_sem.Checked = true;
            //        }
            //        txt_sem.Text = "Semester(" + cbl_sem.Items.Count + ")";
            //    }
            //}
            string query = " select distinct  MAX( ndurations)as ndurations from ndegree where Degree_code in(" + branch + ") union select distinct  MAX(duration) as ndurations  from degree where Degree_Code in(" + branch + ") ";
            ds = d2.select_method_wo_parameter(query, "Text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                //for (i = 0; i < ds.Tables[0].Rows.Count; i++)
                //{
                cbl_sem.Items.Clear();
                string sem = Convert.ToString(ds.Tables[0].Rows[0]["ndurations"]);
                for (int j = 1; j <= Convert.ToInt32(sem); j++)
                {
                    cbl_sem.Items.Add(new System.Web.UI.WebControls.ListItem(j.ToString(), j.ToString()));
                    cbl_sem.Items[j - 1].Selected = true;
                    cb_sem.Checked = true;
                }
                txt_sem.Text = "Semester(" + sem + ")";
                //}
            }
        }
    }

    public void cb_sec_CheckedChanged(object sender, EventArgs e)
    {
        try
        {
            int cout = 0;
            txt_sec.Text = "--Select--";
            if (cb_sec.Checked == true)
            {
                cout++;
                for (int i = 0; i < cbl_sec.Items.Count; i++)
                {
                    cbl_sec.Items[i].Selected = true;
                }
                txt_sec.Text = "Semester(" + (cbl_sec.Items.Count) + ")";
            }
            else
            {
                for (int i = 0; i < cbl_sec.Items.Count; i++)
                {
                    cbl_sec.Items[i].Selected = false;
                }
                txt_sec.Text = "--Select--";
            }
        }
        catch (Exception ex)
        {
            d2.sendErrorMail(ex, collegecode1, "FeedbackReport");
        }
        // Subject();
        bindfeedback();
        load_staffname();
    }

    public void cbl_sec_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            int commcount = 0;
            txt_sec.Text = "--Select--";
            cb_sec.Checked = false;
            for (int i = 0; i < cbl_sec.Items.Count; i++)
            {
                if (cbl_sec.Items[i].Selected == true)
                {
                    commcount = commcount + 1;
                    cb_sec.Checked = false;
                }
            }
            if (commcount > 0)
            {
                if (commcount == cbl_sec.Items.Count)
                {
                    cb_sec.Checked = true;
                }
                txt_sec.Text = "Section(" + commcount.ToString() + ")";
            }
            // Subject();
            bindfeedback();
            load_staffname();
        }
        catch (Exception ex)
        {
        }
    }

    public void bindsec()
    {
        try
        {
            cbl_sec.Items.Clear();
            txt_sec.Text = "---Select---";
            cb_sec.Checked = false;
            string batch = "";
            if (cbl_batch.Items.Count > 0)
            {
                for (int i = 0; i < cbl_batch.Items.Count; i++)
                {
                    if (cbl_batch.Items[i].Selected == true)
                    {
                        if (batch == "")
                        {
                            batch = Convert.ToString(cbl_batch.Items[i].Value);
                        }
                        else
                        {
                            batch = batch + "," + Convert.ToString(cbl_batch.Items[i].Value);
                        }
                    }
                }
            }
            string branchcode1 = "";
            if (cbl_branch.Items.Count > 0)
            {
                for (int i = 0; i < cbl_branch.Items.Count; i++)
                {
                    if (cbl_branch.Items[i].Selected == true)
                    {
                        if (branchcode1 == "")
                        {
                            branchcode1 = Convert.ToString(cbl_branch.Items[i].Value);
                        }
                        else
                        {
                            branchcode1 = branchcode1 + "," + Convert.ToString(cbl_branch.Items[i].Value);
                        }
                    }
                }
            }
            if (batch != "" && branchcode1 != "")
            {
                ds = d2.BindSectionDetail(batch, branchcode1);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    cbl_sec.DataSource = ds;
                    cbl_sec.DataTextField = "sections";
                    cbl_sec.DataValueField = "sections";
                    cbl_sec.DataBind();
                    if (cbl_sec.Items.Count > 0)
                    {
                        for (int row = 0; row < cbl_sec.Items.Count; row++)
                        {
                            cbl_sec.Items[row].Selected = true;
                            cb_sec.Checked = true;
                        }
                        txt_sec.Text = "Section(" + cbl_sec.Items.Count + ")";
                    }
                }
                else
                {
                    cbl_sec.Items.Add("Empty");
                    for (int row = 0; row < cbl_sec.Items.Count; row++)
                    {
                        cbl_sec.Items[row].Selected = true;
                        cb_sec.Checked = true;
                    }
                    txt_sec.Text = "Section(" + cbl_sec.Items.Count + ")";
                }
            }
            else
            {
                cbl_sec.Items.Add("Empty");
                for (int row = 0; row < cbl_sec.Items.Count; row++)
                {
                    cbl_sec.Items[row].Selected = true;
                    cb_sec.Checked = true;
                }
                txt_sec.Text = "Section(" + cbl_sec.Items.Count + ")";
            }
        }
        catch (Exception ex)
        {
            d2.sendErrorMail(ex, collegecode1, "Feedback Report");
        }
    }

    public void Subject()
    {
        try
        {
            ds.Clear();
            string Year = "";
            if (cbl_batch.Items.Count > 0)
            {
                for (int i = 0; i < cbl_batch.Items.Count; i++)
                {
                    if (cbl_batch.Items[i].Selected == true)
                    {
                        if (Year == "")
                        {
                            Year = Convert.ToString(cbl_batch.Items[i].Value);
                        }
                        else
                        {
                            Year = Year + "','" + Convert.ToString(cbl_batch.Items[i].Value);
                        }
                    }
                }
            }
            string branchcode = "";
            if (cbl_branch.Items.Count > 0)
            {
                for (int i = 0; i < cbl_branch.Items.Count; i++)
                {
                    if (cbl_branch.Items[i].Selected == true)
                    {
                        if (branchcode == "")
                        {
                            branchcode = Convert.ToString(cbl_branch.Items[i].Value);
                        }
                        else
                        {
                            branchcode = branchcode + "','" + Convert.ToString(cbl_branch.Items[i].Value);
                        }
                    }
                }
            }
            string sem = "";
            if (cbl_sem.Items.Count > 0)
            {
                for (int i = 0; i < cbl_sem.Items.Count; i++)
                {
                    if (cbl_sem.Items[i].Selected == true)
                    {
                        if (sem == "")
                        {
                            sem = Convert.ToString(cbl_sem.Items[i].Value);
                        }
                        else
                        {
                            sem = sem + "','" + Convert.ToString(cbl_sem.Items[i].Value);
                        }
                    }
                }
            }
            string college_cd = "";
            if (Cbl_college.Items.Count > 0)
            {
                for (int i = 0; i < Cbl_college.Items.Count; i++)
                {
                    if (Cbl_college.Items[i].Selected == true)
                    {
                        if (college_cd == "")
                        {
                            college_cd = "" + Cbl_college.Items[i].Value.ToString() + "";
                        }
                        else
                        {
                            college_cd = college_cd + "','" + Convert.ToString(Cbl_college.Items[i].Value);
                        }
                    }
                }
            }
            string section = "";
            if (cbl_sec.Items.Count > 0)
            {
                for (int i = 0; i < cbl_sec.Items.Count; i++)
                {
                    if (cbl_sec.Items[i].Selected == true)
                    {
                        if (section == "")
                        {
                            section = Convert.ToString(cbl_sec.Items[i].Value);
                        }
                        else
                        {
                            section = section + "','" + Convert.ToString(cbl_sec.Items[i].Value);
                        }
                        if (cbl_sec.Items[i].Value == "Empty")
                        {
                            section = "";
                        }
                    }
                }
            }
            if (section.Trim() != "")
            {
                section = section + "','";
            }
            string type = "";
            if (rb_Acad.Checked == true)
            {
                type = "1";
            }
            else if (rb_Gend.Checked == true)
            {
                type = "2";
            }
            Cbl_Subject.Items.Clear();
            string sub_name = "";
            if (rb_Acad.Checked == true)
            {
                string fbpk = " select FeedBackMasterPK from CO_FeedBackMaster where FeedBackName ='" + ddl_Feedbackname.SelectedItem.Value + "' and DegreeCode in ('" + branchcode + "') and semester in ('" + sem + "') and Batch_Year in('" + Year + "') and isnull(section,'') in ('" + section + "')  ";//and DegreeCode in ('" + degree_code + "')
                DataSet subjectDs = d2.select_method_wo_parameter(fbpk, "Text");//06.02.18 barath
                string feedbakpk = string.Empty;
                if (subjectDs.Tables.Count > 0)
                {
                    if (subjectDs.Tables[0].Rows.Count > 0)
                    {
                        for (int pk = 0; pk < subjectDs.Tables[0].Rows.Count; pk++)
                        {
                            if (feedbakpk == "")
                                feedbakpk = subjectDs.Tables[0].Rows[pk]["FeedBackMasterPK"].ToString();
                            else
                                feedbakpk = feedbakpk + "','" + subjectDs.Tables[0].Rows[pk]["FeedBackMasterPK"].ToString() + "";
                        }
                    }
                }
                if (ddl_Feedbackname.SelectedItem.Text == "Select")
                {
                    // string fbpk = d2.GetFunction(" select FeedBackMasterPK from CO_FeedBackMaster where FeedBackName ='" + ddl_Feedbackname.SelectedItem.Value + "' and DegreeCode in ('" + branchcode + "') and semester in ('" + sem + "') and Batch_Year in('" + Year + "') and section in ('" + section + "')");
                    sub_name = " select  distinct subject_name, c.subject_no,subject_code from Registration r,subjectChooser c ,subject s, CO_StudFeedBack sf where sf.SubjectNo =c.subject_no and r.Roll_No = c.roll_no and r.Current_Semester = c.semester and c.subject_no = s.subject_no and r.Batch_Year in ('" + Year + "') and r.degree_code in ('" + branchcode + "') and r.Current_Semester in ('" + sem + "') and r.Sections in ('" + section + "')  and r.College_code in ('" + college_cd + "') and sf.FeedBackMasterFK in('" + feedbakpk + "')";
                }
                else
                {
                    //string st_type = d2.GetFunction(" select top 1 Subject_Type from CO_FeedBackMaster where FeedBackName ='" + ddl_Feedbackname.SelectedItem.Value + "'");
                    //string sub_type = "";
                    //string[] split = st_type.Split(',');
                    //for (int i = 0; i < split.Length; i++)
                    //{
                    //    if (sub_type == "")
                    //    {
                    //        sub_type = split[i];
                    //    }
                    //    else
                    //    {
                    //        sub_type += "','" + split[i];
                    //    }
                    //}
                    //string fbpk = d2.GetFunction(" select FeedBackMasterPK from CO_FeedBackMaster where FeedBackName ='" + ddl_Feedbackname.SelectedItem.Value + "' and DegreeCode in ('" + branchcode + "') and semester in ('" + sem + "') and Batch_Year in('" + Year + "') and isnull(section,'') in ('" + section + "')");
                    //sub_name = " select  distinct subject_name, c.subject_no,subject_code from Registration r,subjectChooser c ,subject s, sub_sem su,CO_StudFeedBack sf where sf.SubjectNo =s.subject_no and r.Roll_No = c.roll_no and r.Current_Semester = c.semester and c.subject_no = s.subject_no and s.subType_no =su.subType_no  and  r.Batch_Year in ('" + Year + "') and r.degree_code in ('" + branchcode + "') and r.Current_Semester in ('" + sem + "') and r.Sections in ('" + section + "')  and r.College_code in ('" + college_cd + "') and su.subject_type in ('" + sub_type + "')and sf.FeedBackMasterFK in('" + fbpk + "')";
                    //19.08.16
                    string con = "";
                    if (rb_login.Checked == true)
                        con = "";
                    if (rb_anonymous.Checked == true)
                        con = " and sf.App_No is null and FeedbackUnicode is not null";
                    sub_name = " select distinct subject_name, s.subject_no,subject_code from CO_StudFeedBack sf,CO_FeedBackMaster f,subject s  where sf.FeedBackMasterFK =f.FeedBackMasterPK and sf.SubjectNo =s.subject_no " + con + " and f.Batch_Year in('" + Year + "') and f.DegreeCode in('" + branchcode + "') and f.semester in('" + sem + "') and f.CollegeCode in('" + college_cd + "')  and FeedBackMasterPK in('" + feedbakpk + "' ) and isnull(f.Section,'') in('" + section + "')";
                }
            }
            else if (rb_Gend.Checked == true)
            {
            }
            ds = d2.select_method_wo_parameter(sub_name, "Text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                Cbl_Subject.DataSource = ds;
                Cbl_Subject.DataTextField = "Subject_Name";
                Cbl_Subject.DataValueField = "subject_code";
                Cbl_Subject.DataBind();
            }
            if (Cbl_Subject.Items.Count > 0)
            {
                for (int row = 0; row < Cbl_Subject.Items.Count; row++)
                {
                    Cbl_Subject.Items[row].Selected = true;
                    Cb_Subject.Checked = true;
                }
                Txt_Subject.Text = "Subject(" + Cbl_Subject.Items.Count + ")";
            }
            else
            {
                Txt_Subject.Text = "--Select--";
            }
        }
        catch (Exception ex)
        {
            d2.sendErrorMail(ex, collegecode1, "Feedback Report");
        }
    }

    protected void Cb_Subject_CheckedChanged(object sender, EventArgs e)
    {
        if (Cb_Subject.Checked == true)
        {
            for (int i = 0; i < Cbl_Subject.Items.Count; i++)
            {
                Cbl_Subject.Items[i].Selected = true;
            }
            Txt_Subject.Text = "Subject(" + (Cbl_Subject.Items.Count) + ")";
        }
        else
        {
            for (int i = 0; i < Cbl_Subject.Items.Count; i++)
            {
                Cbl_Subject.Items[i].Selected = false;
            }
            Txt_Subject.Text = "--Select--";
        }
        load_staffname();
        load_questions();
    }

    protected void Cbl_Subject_SelectedIndexChanged(object sender, EventArgs e)
    {
        Txt_Subject.Text = "--Select--";
        Cb_Subject.Checked = false;
        int commcount = 0;
        for (int i = 0; i < Cbl_Subject.Items.Count; i++)
        {
            if (Cbl_Subject.Items[i].Selected == true)
            {
                commcount = commcount + 1;
            }
        }
        if (commcount > 0)
        {
            Txt_Subject.Text = "Subject(" + commcount.ToString() + ")";
            if (commcount == Cbl_Subject.Items.Count)
            {
                Cb_Subject.Checked = true;
            }
        }
        load_staffname();
        load_questions();
    }

    public void cb_staffname_CheckedChanged(object sender, EventArgs e)
    {
        try
        {
            int cout = 0;
            txt_staffname.Text = "--Select--";
            if (cb_staffname.Checked == true)
            {
                cout++;
                for (int i = 0; i < cbl_staffname.Items.Count; i++)
                {
                    cbl_staffname.Items[i].Selected = true;
                }
                txt_staffname.Text = "Staff Name(" + (cbl_staffname.Items.Count) + ")";
            }
            else
            {
                for (int i = 0; i < cbl_staffname.Items.Count; i++)
                {
                    cbl_staffname.Items[i].Selected = false;
                }
            }
            load_questions();
        }
        catch (Exception ex)
        {
        }
    }

    public void cbl_staffname_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            cb_staffname.Checked = false;
            int commcount = 0;
            txt_staffname.Text = "--Select--";
            for (int i = 0; i < cbl_staffname.Items.Count; i++)
            {
                if (cbl_staffname.Items[i].Selected == true)
                {
                    commcount = commcount + 1;
                    cb_staffname.Checked = false;
                }
            }
            if (commcount > 0)
            {
                if (commcount == cbl_staffname.Items.Count)
                {
                    cb_staffname.Checked = true;
                }
                txt_staffname.Text = "Staff Name(" + commcount.ToString() + ")";
            }
            load_questions();
        }
        catch (Exception ex)
        {
        }
    }

    public void load_staffname()
    {
        try
        {
            cbl_staffname.Items.Clear();
            ds.Clear();
            string section = "";
            if (cbl_sec.Items.Count > 0)
            {
                for (int i = 0; i < cbl_sec.Items.Count; i++)
                {
                    if (cbl_sec.Items[i].Selected == true)
                    {
                        if (section == "")
                        {
                            section = Convert.ToString(cbl_sec.Items[i].Value);
                        }
                        else
                        {
                            section = section + "','" + Convert.ToString(cbl_sec.Items[i].Value);
                        }
                        if (cbl_sec.Items[i].Value == "Empty")
                        {
                            section = "";
                        }
                    }
                }
            }
            string degreecode = returnwithsinglecodevalue(cbl_branch);
            string sem = returnwithsinglecodevalue(cbl_sem);
            string batchyear = returnwithsinglecodevalue(cbl_batch);
            string suject = returnwithsinglecodevalue(Cbl_Subject);
            string sqlstaffname = "";
            string fbpk = returndswithsinglecodevalue("select FeedBackMasterPK from CO_FeedBackMaster where FeedBackName ='" + ddl_Feedbackname.SelectedItem.Value + "' and DegreeCode in ('" + degreecode + "') and semester in ('" + sem + "') and Batch_Year in('" + batchyear + "') and section in ('" + section + "')");
            if (suject != "" && fbpk.Trim() != "")
            {
                //09.01.17
                //sqlstaffname = " select distinct s.staff_name, ss.staff_code   from staff_selector ss,staffmaster s,subject su where ss.staff_code =s.staff_code and ss.subject_no =su.subject_no and su.subject_code in (" + suject + ") and ss.batch_year in (" + batch + ") and ss.Sections in ('" + section + "')";
                sqlstaffname = "  select distinct s.staff_name, ss.staff_code   from staff_selector ss,staffmaster s,staff_appl_master sa,subject su ,CO_StudFeedBack CS where ss.staff_code =s.staff_code and ss.subject_no =su.subject_no and cs.SubjectNo =su.subject_no and sa.appl_no =s.appl_no and sa.appl_id =cs.StaffApplNo and cs.FeedBackMasterfK in('" + fbpk + "')  and su.subject_code in ('" + suject + "') and ss.batch_year in ('" + batchyear + "') and ss.Sections in ('" + section + "')";
                ds = d2.select_method_wo_parameter(sqlstaffname, "Text");
            }
            cbl_staffname.Items.Clear();
            txt_staffname.Text = "---Select---";
            cb_staffname.Checked = false;
            if (ds.Tables.Count > 0)
            {
                if (ds.Tables[0].Rows.Count > 0)
                {
                    cbl_staffname.DataSource = ds;
                    cbl_staffname.DataTextField = "staff_name";
                    cbl_staffname.DataValueField = "staff_code";
                    cbl_staffname.DataBind();
                    if (cbl_staffname.Items.Count > 0)
                    {
                        for (int row = 0; row < cbl_staffname.Items.Count; row++)
                        {
                            cbl_staffname.Items[row].Selected = true;
                            cb_staffname.Checked = true;
                        }
                        txt_staffname.Text = "Staff Name(" + cbl_staffname.Items.Count + ")";
                    }
                }
            }
        }
        catch (Exception ex)
        {
            d2.sendErrorMail(ex, collegecode1, "Feedback Report");
        }
    }

    public void ddlstaff()
    {
        try
        {
            string branchcode = returnwithsinglecodevalue(cbl_branch);
            string sem = returnwithsinglecodevalue(cbl_sem);
            string batch = returnwithsinglecodevalue(cbl_batch);
            string section = returnwithsinglecodevalue(cbl_sec);
            section = section + "','";
            string sqlstaffname = ""; ds.Clear();
            string fbpk = returndswithsinglecodevalue("select FeedBackMasterPK from CO_FeedBackMaster where FeedBackName ='" + ddl_Feedbackname.SelectedItem.Value + "' and DegreeCode in ('" + branchcode + "') and semester in ('" + sem + "') and Batch_Year in('" + batch + "') and section in ('" + section + "')");
            // sqlstaffname = " select distinct s.staff_name, ss.staff_code   from staff_selector ss,staffmaster s,subject su where ss.staff_code =s.staff_code and ss.subject_no =su.subject_no and su.subject_code in (" + suject + ") and ss.batch_year in (" + batch + ") and ss.Sections in ('" + section + "')";
            if (fbpk.Trim() != "")
            {
                sqlstaffname = " select distinct sa.appl_id,s.staff_name,s.staff_code  from CO_StudFeedBack sf,staff_appl_master sa,staffmaster s where sf.StaffApplNo =sa.appl_id and s.appl_no =sa.appl_no and sf.FeedBackMasterFK in('" + fbpk + "')";
                //09.01.17
                ds.Clear();
                ds = d2.select_method_wo_parameter(sqlstaffname, "Text");
                ddl_staffname.Items.Clear();
                if (ds.Tables[0].Rows.Count > 0)
                {
                    ddl_staffname.DataSource = ds;
                    ddl_staffname.DataTextField = "staff_name";
                    ddl_staffname.DataValueField = "staff_code";
                    ddl_staffname.DataBind();
                    ddl_staffname.Items.Insert(0, "Select");
                }
                else
                {
                    ddl_staffname.Items.Clear();
                    ddl_staffname.Items.Insert(0, "Select");
                }
            }
            else
            {
                ddl_staffname.Items.Clear();
                ddl_staffname.Items.Insert(0, "Select");
            }
        }
        catch (Exception ex)
        {
            d2.sendErrorMail(ex, collegecode1, "Feedback Report");
        }
    }

    public void cb_question_CheckedChanged(object sender, EventArgs e)
    {
        try
        {
            int cout = 0;
            txt_question.Text = "--Select--";
            if (cb_question.Checked == true)
            {
                cout++;
                for (int i = 0; i < cbl_question.Items.Count; i++)
                {
                    cbl_question.Items[i].Selected = true;
                }
                txt_question.Text = "Questions(" + (cbl_question.Items.Count) + ")";
            }
            else
            {
                for (int i = 0; i < cbl_question.Items.Count; i++)
                {
                    cbl_question.Items[i].Selected = false;
                }
            }
        }
        catch (Exception ex)
        {
        }
    }

    public void cbl_question_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            cb_question.Checked = false;
            int commcount = 0;
            txt_question.Text = "--Select--";
            for (int i = 0; i < cbl_question.Items.Count; i++)
            {
                if (cbl_question.Items[i].Selected == true)
                {
                    commcount = commcount + 1;
                    cb_question.Checked = false;
                }
            }
            if (commcount > 0)
            {
                if (commcount == cbl_question.Items.Count)
                {
                    cb_question.Checked = true;
                }
                txt_question.Text = "Questions(" + commcount.ToString() + ")";
            }
        }
        catch (Exception ex)
        {
        }
    }

    public void load_questions()
    {
        cbl_question.Items.Clear();
        ds.Clear();
        string type = "";
        if (rb_Acad.Checked == true)
        {
            type = "1";
        }
        else if (rb_Gend.Checked == true)
        {
            type = "2";
        }
        //string feedbackname = ddl_Feedbackname.SelectedItem.Text.ToString();
        string feedbackname = "";
        if (ddl_Feedbackname.Items.Count > 0)
        {
            feedbackname = ddl_Feedbackname.SelectedItem.Text.ToString();
        }
        string college = "";
        for (int year = 0; year < Cbl_college.Items.Count; year++)
        {
            if (Cbl_college.Items[year].Selected == true)
            {
                if (college == "")
                {
                    college = "'" + Cbl_college.Items[year].Value.ToString() + "'";
                }
                else
                {
                    college = college + "," + "'" + Cbl_college.Items[year].Value.ToString() + "'";
                }
            }
        }
        string Batch_Year = "";
        for (int i = 0; i < cbl_batch.Items.Count; i++)
        {
            if (cbl_batch.Items[i].Selected == true)
            {
                if (Batch_Year == "")
                {
                    Batch_Year = "" + cbl_batch.Items[i].Value.ToString() + "";
                }
                else
                {
                    Batch_Year = Batch_Year + "','" + cbl_batch.Items[i].Value.ToString() + "";
                }
            }
        }
        string degree_code = "";
        for (int i = 0; i < cbl_branch.Items.Count; i++)
        {
            if (cbl_branch.Items[i].Selected == true)
            {
                if (degree_code == "")
                {
                    degree_code = "" + cbl_branch.Items[i].Value.ToString() + "";
                }
                else
                {
                    degree_code = degree_code + "','" + cbl_branch.Items[i].Value.ToString() + "";
                }
            }
        }
        string semester = "";
        for (int i = 0; i < cbl_sem.Items.Count; i++)
        {
            if (cbl_sem.Items[i].Selected == true)
            {
                if (semester == "")
                {
                    semester = "" + cbl_sem.Items[i].Value.ToString() + "";
                }
                else
                {
                    semester = semester + "','" + cbl_sem.Items[i].Value.ToString() + "";
                }
            }
        }
        string staffcod = "";
        for (int i = 0; i < cbl_staffname.Items.Count; i++)
        {
            if (cbl_staffname.Items[i].Selected == true)
            {
                if (staffcod == "")
                {
                    staffcod = "" + cbl_staffname.Items[i].Value.ToString() + "";
                }
                else
                {
                    staffcod = staffcod + "','" + cbl_staffname.Items[i].Value.ToString() + "";
                }
            }
        }
        string sub = "";
        for (int i = 0; i < Cbl_Subject.Items.Count; i++)
        {
            if (Cbl_Subject.Items[i].Selected == true)
            {
                if (sub == "")
                {
                    sub = "" + Cbl_Subject.Items[i].Value.ToString() + "";
                }
                else
                {
                    sub = sub + "','" + Cbl_Subject.Items[i].Value.ToString() + "";
                }
            }
        }
        string section = "";
        for (int i = 0; i < cbl_sec.Items.Count; i++)
        {
            if (cbl_sec.Items[i].Selected == true)
            {
                if (section == "")
                {
                    section = "" + cbl_sec.Items[i].Value.ToString() + "";
                }
                else
                {
                    section = section + "','" + cbl_sec.Items[i].Value.ToString() + "";
                }
                if (cbl_sec.Items[i].Value == "Empty")
                {
                    section = "";
                }
            }
        }
        if (section.Trim() != "")
        {
            section = section + "','";
        }
        ds.Clear();
        string selqry = "";
        if (feedbackname != "")
        {
            selqry = "select distinct Question,QuestionMasterPK , QuestType from CO_QuestionMaster q, CO_FeedBackMaster fm, CO_FeedBackQuestions fb where q.QuestionMasterPK=fb.QuestionMasterFK and fb.FeedBackMasterFK =fm.FeedBackMasterPK and  QuestType ='" + type + "'  and fm.FeedBackName in ('" + feedbackname + "') and  fm.CollegeCode in (" + college + ")   ";
            //selqry = "SELECT Q.Question,Q.QuestionMasterPK FROM CO_StudFeedBack F,CO_MarkMaster M,Subject S,staff_appl_master A,staffmaster T,syllabus_master y,Degree d, Department dt,Course c,CO_FeedBackMaster FM,Registration r, CO_QuestionMaster Q WHERE Q.QuestionMasterPK =f.QuestionMasterFK and  F.MarkMasterPK = M.MarkMasterPK AND F.StaffApplNo = A.appl_id AND F.SubjectNo = S.subject_no AND A.appl_no = T.appl_no  and s.syll_code = y.syll_code and d.Degree_Code =y.degree_code and dt.Dept_Code =d.Dept_Code and c.Course_Id =d.Course_Id  and f.App_No = r.App_No and FM.CollegeCode in ('" + college_cd + "') and y.Batch_Year in ('" + Batch_Year + "') and y.degree_code in ('" + degree_code + "') and y.semester in ('" + semester + "')  and fm.FeedBackType =('" + type + "') and fm.FeedBackName='" + ddl_Feedbackname.SelectedItem.Value + "' and staff_code in('" + staffcod + "')and Subject_Code in ('" + sub + "')  ";
            //if (section != "")
            //{
            //    selqry = selqry + " and r.Sections in ('" + section + "') and f.FeedBackMasterFK = fm.FeedBackMasterPK group by Question,Q.QuestionMasterPK";
            //}
            //else
            //{
            //    selqry = selqry + " and f.FeedBackMasterFK = fm.FeedBackMasterPK group by Question,Q.QuestionMasterPK";
            //}
            ds = d2.select_method_wo_parameter(selqry, "Text");
        }
        cbl_question.Items.Clear();
        txt_question.Text = "---Select---";
        cb_question.Checked = false;
        if (ds.Tables.Count > 0)
        {
            if (ds.Tables[0].Rows.Count > 0)
            {
                cbl_question.DataSource = ds;
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
                    txt_question.Text = "Questions(" + cbl_question.Items.Count + ")";
                }
            }
        }
    }

    protected void rb_farmate4_CheckedChanged(object sender, EventArgs e)
    {
        rdb_form4staffwise.Visible = true;
        rdb_form4questwise.Visible = true;
        rdb_form4questwise.Checked = true;
        anonymousfilter1.Visible = true;
        anonymousfilter2.Visible = true;
        anonymousfilter3.Visible = false;
        txtfrom_range.Text = "";
        txtto_range.Text = "";
        ddl_criter.SelectedIndex = 0;
        Total_points.Visible = false;
        rptprint1.Visible = false;
        chartprint.Visible = false;
        question_chart.Visible = false;
        staff_chart.Visible = false;
        FpSpread1.Visible = false;
        FpSpread2.Visible = false;
        FpSpread3.Visible = false;
        FpSpread4.Visible = false;
        lbl_headig.Visible = true;
        lbl_headig.Text = "Staff Percentage Chart";
        rb_linchart.Visible = true;
        rb_barchart.Visible = true;
        chart();
        format2false();
        load_staffname();
        load_questions();
        lbl_subject.Visible = true;
        UpdatePanel5.Visible = true;
        Txt_Subject.Visible = true;
        UpdatePanel8.Visible = true;
        lbl_stfsubject.Visible = false;
        UpdatePanel17.Visible = false;
        Txt_stfSubject.Visible = false;
        cb_avgcolumn.Visible = false;
        btn_printperticulaterstaff.Visible = false;
        ddl_headershow.Visible = false;
        lbl_header.Visible = false;
    }

    protected void rb_farmate5_CheckedChanged(object sender, EventArgs e)
    {
        rdb_form4staffwise.Visible = false;
        rdb_form4questwise.Visible = false;
        anonymousfilter1.Visible = true;
        anonymousfilter2.Visible = true;
        anonymousfilter3.Visible = false;
        txtfrom_range.Text = "";
        txtto_range.Text = "";
        ddl_criter.SelectedIndex = 0;
        Total_points.Visible = false;
        rptprint1.Visible = false;
        chartprint.Visible = false;
        question_chart.Visible = false;
        staff_chart.Visible = false;
        FpSpread1.Visible = false;
        FpSpread2.Visible = false;
        FpSpread3.Visible = false;
        FpSpread4.Visible = false;
        lbl_headig.Visible = true;
        lbl_headig.Text = "Questionwise Performance Chart";
        format2false();
        chart();
        rb_linchart.Visible = true;
        rb_barchart.Visible = true;
        UpdatePanel8.Visible = true;
        cb_avgcolumn.Visible = false;
        btn_printperticulaterstaff.Visible = false;
        ddl_headershow.Visible = false;
        lbl_header.Visible = false;
    }

    protected void rb_farmate6_CheckedChanged(object sender, EventArgs e)
    {
        rdb_form4staffwise.Visible = false;
        rdb_form4questwise.Visible = false;
        anonymousfilter1.Visible = true;
        anonymousfilter2.Visible = true;
        anonymousfilter3.Visible = false;
        txtfrom_range.Text = "";
        txtto_range.Text = "";
        ddl_criter.SelectedIndex = 0;
        Total_points.Visible = false;
        rptprint1.Visible = false;
        chartprint.Visible = false;
        question_chart.Visible = false;
        staff_chart.Visible = false;
        FpSpread1.Visible = false;
        FpSpread2.Visible = false;
        FpSpread3.Visible = false;
        FpSpread4.Visible = false;
        lbl_headig.Visible = true;
        lbl_headig.Text = " Questionwise Average Chart.";
        format2false();
        chart();
        lbl_subject.Visible = false;
        UpdatePanel5.Visible = false;
        Txt_Subject.Visible = false;
        lbl_staffname.Visible = false;
        txt_staffname.Visible = false;
        Panel_staffname.Visible = false;
        chart_selct.Visible = true;
        Panel_Subject.Visible = false;
        rb_linchart.Visible = true;
        rb_barchart.Visible = true;
        cb_avgcolumn.Visible = false;
        btn_printperticulaterstaff.Visible = false;
        ddl_headershow.Visible = false;
        lbl_header.Visible = false;
    }

    protected void btn_exit_Click(object sender, EventArgs e)
    {
    }

    protected void btnExcel1_Click(object sender, EventArgs e)
    {
        lbl_norec1.Visible = false;
        try
        {
            string reportname = txtexcelname1.Text;
            if (reportname.ToString().Trim() != "")
            {
                if (FpSpread1.Visible == true)
                {
                    d2.printexcelreport(FpSpread1, reportname);
                }
                else if (FpSpread2.Visible == true)
                {
                    d2.printexcelreport(FpSpread2, reportname);
                }
                else if (FpSpread3.Visible == true)
                {
                    d2.printexcelreport(FpSpread3, reportname);
                }
                else if (FpSpread4.Visible == true)
                {
                    d2.printexcelreport(FpSpread4, reportname);
                }
                else if (Fpspread6.Visible == true)
                {
                    d2.printexcelreport(Fpspread6, reportname);
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
            int count = 0;
            int count1 = 0;
            int batchcount = 0;
            int semcount = 0;
            string degree = "";
            string sub = "";
            string batch = "";
            string semester = "";
            string dptname = "Feedback report";
            string pagename = "Feedback_report.aspx";
            for (int i = 0; i < cbl_degree.Items.Count; i++)
            {
                if (cbl_degree.Items[i].Selected == true)
                {
                    count++;
                    degree = cbl_degree.Items[i].Text;
                }
            }
            for (int i = 0; i < Cbl_StfSubject.Items.Count; i++)
            {
                if (Cbl_StfSubject.Items[i].Selected == true)
                {
                    count1++;
                    sub = Cbl_StfSubject.Items[i].Text;
                }
            }
            for (int i = 0; i < cbl_batch.Items.Count; i++)
            {
                if (cbl_batch.Items[i].Selected == true)
                {
                    batchcount++;
                    batch = cbl_batch.Items[i].Text;
                }
            }
            for (int i = 0; i < cbl_sem.Items.Count; i++)
            {
                if (cbl_sem.Items[i].Selected == true)
                {
                    semcount++;
                    semester = cbl_sem.Items[i].Text;
                }
            }
            if (rb_anonyms_farmate3.Checked == true)
            {
                dptname = "Student Count Report ";
            }
            if (rb_anonyms_farmate6.Checked == true)
            {
                //dptname = "Staff Evaluation Report ";
                string departmentname = ddlformate6_deptname.SelectedItem.Text;
                // attendance = "Letter Document Inward Report" + '\n' + "List of Documents " + status + " as on" + " " + dateformat;//delsi1903
                dptname = "Department Wise Feedback" + '@' + "Department Name" + "" + " : " + departmentname;
            }
            //Commented By saranya on 28August2018
            //string singlestaff = "";
            //if (ddl_staffname.SelectedItem.Text != "Select")
            //{
            //    singlestaff = "Staff Name: " + ddl_staffname.SelectedItem.Text + "";
            //}
            //if (count == 1)
            //{
            //    dptname = dptname + "@ Course     : " + degree + "      " + singlestaff;
            //}
            //if (count1 == 1 && batchcount == 1 && semcount == 1)
            //{
            //    dptname = dptname + '@' + " Batch       : " + batch + "             Subject  : " + sub + "           Semester : " + semester + "";
            //}
            //else if (count1 == 1 && batchcount == 1)
            //{
            //    dptname = dptname + '@' + " Batch  : " + batch + "             Subject  : " + sub;
            //}
            //else if (count1 == 1 && semcount == 1)
            //{
            //    dptname = dptname + '@' + " Subject  : " + sub + "             Semester : " + semester;
            //}
            //else if (batchcount == 1 && semcount == 1)
            //{
            //    dptname = dptname + '@' + " Batch  : " + batch + "             Semester : " + semester;
            //}
            //else if (batchcount == 1)
            //{
            //    dptname = dptname + '@' + " Batch       : " + batch;
            //}
            //else if (count1 == 1)
            //{
            //    dptname = dptname + '@' + " Subject  : " + sub;
            //}
            //else if (semcount == 1)
            //{
            //    dptname = dptname + '@' + " Semester : " + semester;
            //}
            //else
            //{
            //    dptname = "Staff Name " + ddl_staffname.SelectedItem.Text + "";
            //}
            if (FpSpread1.Visible == true)
            {
                Printcontrol1.loadspreaddetails(FpSpread1, pagename, dptname);
            }
            else if (FpSpread2.Visible == true)
            {
                Printcontrol1.loadspreaddetails(FpSpread2, pagename, dptname);
            }
            else if (FpSpread3.Visible == true)
            {
                Printcontrol1.loadspreaddetails(FpSpread3, pagename, dptname);
            }
            else if (FpSpread4.Visible == true)
            {
                Printcontrol1.loadspreaddetails(FpSpread4, pagename, dptname);
            }
            else if (Fpspread6.Visible == true)
            {
                Printcontrol1.loadspreaddetails(Fpspread6, pagename, dptname);
            }
            Printcontrol1.Visible = true;
            lbl_norec1.Visible = false;
        }
        catch
        {
        }
    }

    protected void btn_errorclose_Click(object sender, EventArgs e)
    {
        imgdiv2.Visible = false;
    }

    protected void FpSpread1_OnCellClick(object sender, EventArgs e)
    {
        string activerow = FpSpread1.ActiveSheetView.ActiveRow.ToString();
        string activecol = FpSpread1.ActiveSheetView.ActiveColumn.ToString();
        // cellclick = true;
    }

    protected void FpSpread1_Selectedindexchange(object sender, EventArgs e)
    {
    }

    protected void FpSpread2_OnCellClick(object sender, EventArgs e)
    {
        string activerow = FpSpread2.ActiveSheetView.ActiveRow.ToString();
        string activecol = FpSpread2.ActiveSheetView.ActiveColumn.ToString();
        //cellclick = true;
    }

    protected void FpSpread2_Selectedindexchange(object sender, EventArgs e)
    {
    }

    protected void btn_go_gend_Click(object sender, EventArgs e)
    {
    }

    public void format1()
    {
        try
        {
            chartprint.Visible = false;
            chartfalse();
            if (rb_Acad.Checked == true)
            {
                lbl_headig.Visible = true;
                fair();
                FpSpread1.Visible = true;
                rptprint1.Visible = true;
                string college_cd = "";
                if (Cbl_college.Items.Count > 0)
                {
                    for (int i = 0; i < Cbl_college.Items.Count; i++)
                    {
                        if (Cbl_college.Items[i].Selected == true)
                        {
                            if (college_cd == "")
                            {
                                college_cd = "" + Cbl_college.Items[i].Value.ToString() + "";
                            }
                            else
                            {
                                college_cd = college_cd + "','" + Convert.ToString(Cbl_college.Items[i].Value);
                            }
                        }
                    }
                }
                string Batch_Year = "";
                for (int i = 0; i < cbl_batch.Items.Count; i++)
                {
                    if (cbl_batch.Items[i].Selected == true)
                    {
                        if (Batch_Year == "")
                        {
                            Batch_Year = "" + cbl_batch.Items[i].Value.ToString() + "";
                        }
                        else
                        {
                            Batch_Year = Batch_Year + "','" + cbl_batch.Items[i].Value.ToString() + "";
                        }
                    }
                }
                string degree_code = "";
                for (int i = 0; i < cbl_branch.Items.Count; i++)
                {
                    if (cbl_branch.Items[i].Selected == true)
                    {
                        if (degree_code == "")
                        {
                            degree_code = "" + cbl_branch.Items[i].Value.ToString() + "";
                        }
                        else
                        {
                            degree_code = degree_code + "','" + cbl_branch.Items[i].Value.ToString() + "";
                        }
                    }
                }
                string semester = "";
                for (int i = 0; i < cbl_sem.Items.Count; i++)
                {
                    if (cbl_sem.Items[i].Selected == true)
                    {
                        if (semester == "")
                        {
                            semester = "" + cbl_sem.Items[i].Value.ToString() + "";
                        }
                        else
                        {
                            semester = semester + "','" + cbl_sem.Items[i].Value.ToString() + "";
                        }
                    }
                }
                string section = "";
                for (int i = 0; i < cbl_sec.Items.Count; i++)
                {
                    if (cbl_sec.Items[i].Selected == true)
                    {
                        if (section == "")
                        {
                            section = "" + cbl_sec.Items[i].Value.ToString() + "";
                        }
                        else
                        {
                            section = section + "','" + cbl_sec.Items[i].Value.ToString() + "";
                        }
                        if (cbl_sec.Items[i].Value == "Empty")
                        {
                            section = "";
                        }
                    }
                }
                if (section.Trim() != "")
                {
                    section = section + "','";
                }
                dsfb.Clear();
                string fbpk = " select FeedBackMasterPK from CO_FeedBackMaster where FeedBackName ='" + ddl_Feedbackname.SelectedItem.Value + "' and DegreeCode in ('" + degree_code + "') and semester in ('" + semester + "') and Batch_Year in('" + Batch_Year + "') and section in ('" + section + "')";
                dsfb = d2.select_method_wo_parameter(fbpk, "Text");
                string feedbakpk = "";
                if (dsfb.Tables.Count > 0)
                {
                    if (dsfb.Tables[0].Rows.Count > 0)
                    {
                        for (int pk = 0; pk < dsfb.Tables[0].Rows.Count; pk++)
                        {
                            if (feedbakpk == "")
                            {
                                feedbakpk = dsfb.Tables[0].Rows[pk]["FeedBackMasterPK"].ToString();
                            }
                            else
                            {
                                //feedbakpk = dsfb.Tables[0].Rows[pk]["FeedBackMasterPK"].ToString();
                                feedbakpk = feedbakpk + "','" + dsfb.Tables[0].Rows[pk]["FeedBackMasterPK"].ToString() + "";
                            }
                        }
                    }
                }
                string type = "";
                if (rb_Acad.Checked == true)
                {
                    type = "1";
                }
                else if (rb_Gend.Checked == true)
                {
                    type = "2";
                }
                FpSpread1.Sheets[0].RowCount = 0;
                FpSpread1.Sheets[0].ColumnCount = 0;
                FpSpread1.CommandBar.Visible = false;
                FpSpread1.Sheets[0].AutoPostBack = false;
                FpSpread1.Sheets[0].ColumnHeader.RowCount = 1;
                FpSpread1.Sheets[0].RowHeader.Visible = false;
                FpSpread1.Sheets[0].ColumnCount = 9;
                FarPoint.Web.Spread.StyleInfo darkstyle = new FarPoint.Web.Spread.StyleInfo();
                darkstyle.BackColor = ColorTranslator.FromHtml("#0CA6CA");
                darkstyle.ForeColor = System.Drawing.Color.White;
                FpSpread1.ActiveSheetView.ColumnHeader.DefaultStyle = darkstyle;
                FpSpread1.Visible = true;
                if (cb_avgcolumn.Checked == true)
                    FpSpread1.Width = 980;
                else
                    FpSpread1.Width = 935;
                FpSpread1.Height = 500;
                FpSpread1.SaveChanges();
                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 0].Text = "S.No";
                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 0].Font.Bold = true;
                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 0].HorizontalAlign = HorizontalAlign.Center;
                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 0].Font.Name = "Book Antiqua";
                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 0].Font.Size = FontUnit.Medium;
                FpSpread1.Sheets[0].Columns[0].HorizontalAlign = HorizontalAlign.Center;
                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 1].Text = "Staff Name";
                FpSpread1.Sheets[0].SetColumnMerge(1, FarPoint.Web.Spread.Model.MergePolicy.Always);
                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 1].Font.Bold = true;
                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 1].Font.Name = "Book Antiqua";
                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 1].Font.Size = FontUnit.Medium;
                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 1].HorizontalAlign = HorizontalAlign.Center;
                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 2].Text = "S.No";
                //FpSpread1.Sheets[0].SetColumnMerge(2, FarPoint.Web.Spread.Model.MergePolicy.Always);
                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 2].Font.Bold = true;
                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 2].Font.Name = "Book Antiqua";
                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 2].Font.Size = FontUnit.Medium;
                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 2].HorizontalAlign = HorizontalAlign.Center;
                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 3].Text = "Department";
                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 3].Font.Bold = true;
                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 3].Font.Name = "Book Antiqua";
                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 3].Font.Size = FontUnit.Medium;
                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 3].HorizontalAlign = HorizontalAlign.Center;
                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 4].Text = "Subject Code ";
                FpSpread1.Sheets[0].SetColumnMerge(4, FarPoint.Web.Spread.Model.MergePolicy.Always);
                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 4].Font.Bold = true;
                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 4].Font.Name = "Book Antiqua";
                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 4].Font.Size = FontUnit.Medium;
                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 4].HorizontalAlign = HorizontalAlign.Center;
                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 5].Text = "Subject Name";
                FpSpread1.Sheets[0].SetColumnMerge(4, FarPoint.Web.Spread.Model.MergePolicy.Always);
                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 5].Font.Bold = true;
                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 5].Font.Name = "Book Antiqua";
                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 5].Font.Size = FontUnit.Medium;
                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 5].HorizontalAlign = HorizontalAlign.Center;
                FpSpread1.Sheets[0].Columns[5].HorizontalAlign = HorizontalAlign.Center;
                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 6].Text = "Total Points";
                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 6].Font.Bold = true;
                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 6].Font.Name = "Book Antiqua";
                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 6].Font.Size = FontUnit.Medium;
                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 6].HorizontalAlign = HorizontalAlign.Center;
                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 7].Text = "Average";
                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 7].Font.Bold = true;
                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 7].Font.Name = "Book Antiqua";
                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 7].Font.Size = FontUnit.Medium;
                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 7].HorizontalAlign = HorizontalAlign.Center;
                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 8].Text = "Select";
                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 8].Font.Bold = true;
                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 8].Font.Name = "Book Antiqua";
                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 8].Font.Size = FontUnit.Medium;
                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 8].HorizontalAlign = HorizontalAlign.Center;
                FpSpread1.Sheets[0].ColumnHeader.Columns[0].Width = 39;
                FpSpread1.Sheets[0].ColumnHeader.Columns[1].Width = 180;
                FpSpread1.Sheets[0].ColumnHeader.Columns[2].Width = 50;
                FpSpread1.Sheets[0].ColumnHeader.Columns[3].Width = 200;
                FpSpread1.Sheets[0].ColumnHeader.Columns[4].Width = 100;
                FpSpread1.Sheets[0].ColumnHeader.Columns[5].Width = 200;
                FpSpread1.Sheets[0].ColumnHeader.Columns[6].Width = 100;
                FpSpread1.Sheets[0].ColumnHeader.Columns[7].Width = 100;
                FpSpread1.Sheets[0].ColumnHeader.Columns[8].Width = 50;
                ds.Clear();
                string selqry = "";
                selqry = "SELECT Staff_Name,Subject_Code,Subject_Name,SUM(Point)as Points,COUNT(distinct f.app_no)as Strength,(convert(varchar(10), y.Batch_Year)+'-'+c.Course_Name+'-'+ dt.dept_acronym+'-'+convert(varchar(10), y.Semester)+'-'+r.Sections ) as department,dt.dept_code,y.Batch_Year,SubjectNo,y.semester,r.Sections,staff_code,c.Course_Name,dt.Dept_Name FROM CO_StudFeedBack F,CO_MarkMaster M,Subject S,staff_appl_master A,staffmaster T,syllabus_master y,Degree d, Department dt,Course c, CO_FeedBackMaster FM,Registration r WHERE F.MarkMasterPK = M.MarkMasterPK AND F.StaffApplNo = A.appl_id AND F.SubjectNo = S.subject_no AND A.appl_no = T.appl_no  and s.syll_code = y.syll_code and d.Degree_Code =y.degree_code and dt.Dept_Code =d.Dept_Code and c.Course_Id =d.Course_Id  and f.App_No = r.App_No and FM.CollegeCode in ('" + college_cd + "') and y.Batch_Year in ('" + Batch_Year + "') and y.degree_code in ('" + degree_code + "') and y.semester in ('" + semester + "')  and fm.FeedBackType =('" + type + "') and fm.FeedBackMasterPK in ('" + feedbakpk + "') ";
                if (section != "")
                {
                    selqry = selqry + " and r.Sections in ('" + section + "') and f.FeedBackMasterFK = fm.FeedBackMasterPK group by a.appl_id,Staff_Name, y.Semester, Subject_Code,y.Batch_Year, Subject_Name ,r.Sections, dt.Dept_Name ,c.Course_Name,dept_acronym,dt.dept_code,y.Batch_Year,SubjectNo,y.semester,staff_code,c.Course_Name order by Staff_Name";
                }
                else
                {
                    selqry = selqry + " and f.FeedBackMasterFK = fm.FeedBackMasterPK group by a.appl_id,r.Sections, Staff_Name, y.Semester, Subject_Code,y.Batch_Year, Subject_Name ,dt.Dept_Name ,c.Course_Name,dept_acronym,dt.dept_code,y.Batch_Year, SubjectNo,y.semester,staff_code,c.Course_Name order by Staff_Name";
                }
                //selqry = "SELECT Staff_Name,Semester,Subject_Code,Subject_Name,Batch_Year,SUM(Point)as Points,dt.Dept_Name,c.Course_Name  FROM CO_StudFeedBack F,CO_MarkMaster M,Subject S,staff_appl_master A,staffmaster T,syllabus_master y,Degree d,Department dt,Course c WHERE F.MarkMasterPK = M.MarkMasterPK AND F.StaffApplNo = A.appl_id AND F.SubjectNo = S.subject_no  AND A.appl_no = T.appl_no  and s.syll_code = y.syll_code and d.Degree_Code =y.degree_code and dt.Dept_Code =d.Dept_Code and c.Course_Id =d.Course_Id  and y.Batch_Year in ('" + Batch_Year + "') and y.degree_code in ('" + degree_code + "') and y.semester in ('" + semester + "') group by a.appl_id,Staff_Name, Semester, Subject_Code,Batch_Year, Subject_Name ,dt.Dept_Name ,c.Course_Name";
                ds = d2.select_method_wo_parameter(selqry, "Text");
                string question_count = d2.GetFunction("select COUNT( distinct QuestionMasterFK)question_count from CO_FeedBackQuestions where FeedBackMasterFK in ('" + feedbakpk + "')");
                string needs = "5";
                string sum_total = d2.GetFunction("select top 1 Point from CO_MarkMaster  where CollegeCode in('" + collegecode1 + "') order by Point desc");
                if (ds.Tables.Count > 0)
                {
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        Double sum_tot = 0;
                        Double sum_avgs = 0;
                        int k = 0; string staffname = ""; int s = 1;
                        FarPoint.Web.Spread.CheckBoxCellType cb = new FarPoint.Web.Spread.CheckBoxCellType();
                        cb.AutoPostBack = true;
                        FarPoint.Web.Spread.CheckBoxCellType cb1 = new FarPoint.Web.Spread.CheckBoxCellType();
                        cb1.AutoPostBack = false;
                        FpSpread1.Sheets[0].RowCount++;
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 8].HorizontalAlign = HorizontalAlign.Center;
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 8].CellType = cb;
                        for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                        {
                            FpSpread1.Sheets[0].RowCount++;
                            //if (((i + 1) % 2) == 0)
                            //{
                            //    FpSpread1.Sheets[0].Rows[FpSpread1.Sheets[0].RowCount - 1].BackColor = System.Drawing.Color.LightSeaGreen;
                            //}
                            if (staffname.Trim() == "")
                            { k++; }
                            else if (staffname == ds.Tables[0].Rows[i]["staff_name"].ToString())
                            { k++; }
                            else { k = 1; s++; }
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Text = Convert.ToString(s);
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].Text = ds.Tables[0].Rows[i]["Staff_Name"].ToString(); FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].Tag = ds.Tables[0].Rows[i]["staff_code"].ToString();
                            staffname = ds.Tables[0].Rows[i]["staff_name"].ToString();
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].VerticalAlign = FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].VerticalAlign = VerticalAlign.Middle;
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].Text = k.ToString();
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].VerticalAlign = FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].VerticalAlign = VerticalAlign.Middle;
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 3].Text = ds.Tables[0].Rows[i]["department"].ToString();
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 3].Tag = ds.Tables[0].Rows[i]["Batch_Year"].ToString();
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 3].Note = ds.Tables[0].Rows[i]["Dept_Name"].ToString();
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 4].Text = ds.Tables[0].Rows[i]["Subject_Code"].ToString();
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 4].Tag = ds.Tables[0].Rows[i]["Course_Name"].ToString();
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 4].Note = ds.Tables[0].Rows[i]["Dept_Code"].ToString();
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 4].VerticalAlign = FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 3].VerticalAlign = VerticalAlign.Middle;
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 5].Text = ds.Tables[0].Rows[i]["Subject_Name"].ToString();
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 5].Tag = ds.Tables[0].Rows[i]["semester"].ToString();
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 5].Note = ds.Tables[0].Rows[i]["Sections"].ToString();
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 6].Tag = ds.Tables[0].Rows[i]["SubjectNo"].ToString();
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 5].VerticalAlign = FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 4].VerticalAlign = VerticalAlign.Middle;
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 6].Text = ds.Tables[0].Rows[i]["Points"].ToString();
                            Double point = Convert.ToDouble(ds.Tables[0].Rows[i]["Points"]);
                            Double strength = Convert.ToDouble(ds.Tables[0].Rows[i]["Strength"]);
                            Double avg = point / strength;
                            avg = avg / Convert.ToDouble(question_count);
                            string points = Convert.ToString(avg);
                            ConvertedMark(needs, sum_total, ref points);
                            sum_tot = sum_tot + Convert.ToDouble(ds.Tables[0].Rows[i]["Points"]);
                            sum_avgs = sum_avgs + Convert.ToDouble(points);
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 7].Text = Convert.ToString(points);
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].Rows.Count - 1, 8].CellType = cb1;
                        }
                        FpSpread1.Sheets[0].RowCount++;
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 6].Text = Convert.ToString(sum_tot);
                        sum_avgs = sum_avgs / Convert.ToDouble(FpSpread1.Sheets[0].RowCount - 1);
                        string sumavgpoint = Convert.ToString(Math.Round(sum_avgs, 2));
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 7].Text = sumavgpoint;
                        FpSpread1.Sheets[0].SpanModel.Add(FpSpread1.Sheets[0].RowCount - 1, 0, 1, 6);
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].Rows.Count - 1, 0].Text = "Grand Total";
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].Rows.Count - 1, 0].HorizontalAlign = HorizontalAlign.Right;
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].Rows.Count - 1, 0].ForeColor = System.Drawing.Color.Blue;
                        FpSpread1.Sheets[0].Columns[5].HorizontalAlign = HorizontalAlign.Left;
                        FpSpread1.Sheets[0].Columns[3].HorizontalAlign = HorizontalAlign.Center;
                        FpSpread1.Sheets[0].Columns[4].HorizontalAlign = HorizontalAlign.Center;
                        FpSpread1.Sheets[0].Columns[6].HorizontalAlign = HorizontalAlign.Center;
                        FpSpread1.Sheets[0].Columns[7].HorizontalAlign = HorizontalAlign.Center;
                        FpSpread1.Sheets[0].Columns[8].HorizontalAlign = HorizontalAlign.Center;
                        FpSpread1.Sheets[0].Columns[8].VerticalAlign = VerticalAlign.Middle;
                        FpSpread1.Sheets[0].Columns[2].HorizontalAlign = HorizontalAlign.Center;
                        FpSpread1.Sheets[0].Columns[2].VerticalAlign = VerticalAlign.Middle;
                        FpSpread1.Sheets[0].Columns[0].HorizontalAlign = HorizontalAlign.Center;
                        FpSpread1.Sheets[0].Columns[0].VerticalAlign = VerticalAlign.Middle;
                        FpSpread1.Sheets[0].SetColumnMerge(0, FarPoint.Web.Spread.Model.MergePolicy.Always);
                        if (cb_avgcolumn.Checked == true)
                        {
                            FpSpread1.Columns[7].Visible = true;
                        }
                        else
                        {
                            FpSpread1.Columns[7].Visible = false;
                        }
                        FpSpread1.Sheets[0].PageSize = FpSpread1.Sheets[0].RowCount;
                    }
                    else
                    {
                        //div1.Visible = false;
                        imgdiv2.Visible = true;
                        lbl_alert1.Text = "No Records Found";
                        FpSpread1.Visible = false;
                        rptprint1.Visible = false;
                    }
                }
                else
                {
                    //div1.Visible = false;
                    imgdiv2.Visible = true;
                    lbl_alert1.Text = "No Records Found";
                    FpSpread1.Visible = false;
                    rptprint1.Visible = false;
                }
                //FpSpread1.Width = 800;
                //FpSpread1.Height = 500;
            }
        }
        catch (Exception ex)
        {
            d2.sendErrorMail(ex, collegecode1, "FeedbackReport");
        }
    }

    public void gendformat2()
    {
        try
        {
            fair();
            FpSpread2.Visible = true;
            rptprint1.Visible = true;
            string type = "";
            if (rb_Acad.Checked == true)
            {
                type = "1";
            }
            else if (rb_Gend.Checked == true)
            {
                type = "2";
            }
            string college_cd = "";
            if (Cbl_college.Items.Count > 0)
            {
                for (int i = 0; i < Cbl_college.Items.Count; i++)
                {
                    if (Cbl_college.Items[i].Selected == true)
                    {
                        if (college_cd == "")
                        {
                            college_cd = "" + Cbl_college.Items[i].Value.ToString() + "";
                        }
                        else
                        {
                            college_cd = college_cd + "','" + Convert.ToString(Cbl_college.Items[i].Value);
                        }
                    }
                }
            }
            FpSpread2.Sheets[0].RowCount = 0;
            FpSpread2.Sheets[0].ColumnCount = 0;
            FpSpread2.CommandBar.Visible = false;
            FpSpread2.Sheets[0].AutoPostBack = true;
            FpSpread2.Sheets[0].ColumnHeader.RowCount = 1;
            FpSpread2.Sheets[0].RowHeader.Visible = false;
            FpSpread2.Sheets[0].ColumnCount = 4;
            FarPoint.Web.Spread.StyleInfo darkstyle = new FarPoint.Web.Spread.StyleInfo();
            darkstyle.BackColor = ColorTranslator.FromHtml("#0CA6CA");
            darkstyle.ForeColor = System.Drawing.Color.White;
            FpSpread2.ActiveSheetView.ColumnHeader.DefaultStyle = darkstyle;
            FpSpread2.Visible = true;
            //FpSpread2.Width = 971;
            //FpSpread2.Height = 500;
            FpSpread2.SaveChanges();
            FpSpread2.Sheets[0].ColumnHeader.Cells[0, 0].Text = "S.No";
            FpSpread2.Sheets[0].ColumnHeader.Cells[0, 0].Font.Bold = true;
            FpSpread2.Sheets[0].ColumnHeader.Cells[0, 0].HorizontalAlign = HorizontalAlign.Center;
            FpSpread2.Sheets[0].ColumnHeader.Cells[0, 0].Font.Name = "Book Antiqua";
            FpSpread2.Sheets[0].ColumnHeader.Cells[0, 0].Font.Size = FontUnit.Medium;
            FpSpread2.Sheets[0].Columns[0].HorizontalAlign = HorizontalAlign.Center;
            FpSpread2.Sheets[0].ColumnHeader.Cells[0, 1].Text = "EvaluationName";
            FpSpread2.Sheets[0].ColumnHeader.Cells[0, 1].Font.Bold = true;
            FpSpread2.Sheets[0].ColumnHeader.Cells[0, 1].Font.Name = "Book Antiqua";
            FpSpread2.Sheets[0].ColumnHeader.Cells[0, 1].Font.Size = FontUnit.Medium;
            FpSpread2.Sheets[0].ColumnHeader.Cells[0, 1].HorizontalAlign = HorizontalAlign.Center;
            FpSpread2.Sheets[0].ColumnHeader.Cells[0, 2].Text = "Description ";
            FpSpread2.Sheets[0].ColumnHeader.Cells[0, 2].Font.Bold = true;
            FpSpread2.Sheets[0].ColumnHeader.Cells[0, 2].Font.Name = "Book Antiqua";
            FpSpread2.Sheets[0].ColumnHeader.Cells[0, 2].Font.Size = FontUnit.Medium;
            FpSpread2.Sheets[0].ColumnHeader.Cells[0, 2].HorizontalAlign = HorizontalAlign.Center;
            FpSpread2.Sheets[0].ColumnHeader.Cells[0, 3].Text = "Points ";
            FpSpread2.Sheets[0].ColumnHeader.Cells[0, 3].Font.Bold = true;
            FpSpread2.Sheets[0].ColumnHeader.Cells[0, 3].Font.Name = "Book Antiqua";
            FpSpread2.Sheets[0].ColumnHeader.Cells[0, 3].Font.Size = FontUnit.Medium;
            FpSpread2.Sheets[0].ColumnHeader.Cells[0, 3].HorizontalAlign = HorizontalAlign.Center;
            FpSpread2.Sheets[0].ColumnHeader.Columns[0].Width = 59;
            FpSpread2.Sheets[0].ColumnHeader.Columns[1].Width = 80;
            FpSpread2.Sheets[0].Columns[2].HorizontalAlign = HorizontalAlign.Left;
            ds.Clear();
            string selqry = "";
            selqry = "SELECT FeedBackName,TextVal,Question,SUM(M.Point) as points FROM CO_StudFeedBack F,CO_FeedBackMaster B,CO_QuestionMaster Q,CO_MarkMaster M,TextValTable T WHERE F.FeedBackMasterFK = B.FeedBackMasterPK AND F.QuestionMasterFK =  Q.QuestionMasterPK AND F.MarkMasterPK = M.MarkMasterPK AND Q.HeaderCode = T.TextCode AND FeedBackType = '" + type + "' and B.FeedBackName='" + ddl_Feedbackname.SelectedItem.Value + "'  and B.CollegeCode in ('" + college_cd + "') GROUP BY FeedBackName,TextVal,Question";
            ds = d2.select_method_wo_parameter(selqry, "Text");
            if (ds.Tables.Count > 0)
            {
                if (ds.Tables[0].Rows.Count > 0)
                {
                    int total = 0;
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        FpSpread2.Sheets[0].Rows.Count++;
                        FpSpread2.Sheets[0].Cells[i, 0].Text = Convert.ToString(i + 1);
                        FpSpread2.Sheets[0].Cells[i, 1].Text = ds.Tables[0].Rows[i]["TextVal"].ToString();
                        FpSpread2.Sheets[0].Cells[i, 2].Text = ds.Tables[0].Rows[i]["Question"].ToString();
                        FpSpread2.Sheets[0].Cells[i, 3].Text = ds.Tables[0].Rows[i]["points"].ToString();
                        FpSpread2.Sheets[0].Columns[3].HorizontalAlign = HorizontalAlign.Center;
                        total = total + Convert.ToInt32(ds.Tables[0].Rows[i]["points"]);
                    }
                    FpSpread2.Sheets[0].Rows.Count++;
                    FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].Rows.Count - 1, 3].Text = Convert.ToString(total);
                    FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].Rows.Count - 1, FpSpread2.Sheets[0].ColumnCount - 1].HorizontalAlign = HorizontalAlign.Center;
                    FpSpread2.Sheets[0].SpanModel.Add(FpSpread2.Sheets[0].RowCount - 1, 0, 1, 3);
                    FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].Rows.Count - 1, 0].Text = "Grand Total";
                    FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].Rows.Count - 1, 0].HorizontalAlign = HorizontalAlign.Right;
                    FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].Rows.Count - 1, 0].ForeColor = System.Drawing.Color.Blue;
                }
                else
                {
                    //div1.Visible = false;
                    imgdiv2.Visible = true;
                    lbl_alert1.Text = "No Records Found";
                    FpSpread2.Visible = false;
                    rptprint1.Visible = false;
                }
            }
            else
            {
                // div1.Visible = false;
                imgdiv2.Visible = true;
                lbl_alert1.Text = "No Records Found";
                FpSpread2.Visible = false;
                rptprint1.Visible = false;
            }
            FpSpread2.Sheets[0].Columns[2].Width = 550;
            FpSpread2.Sheets[0].Columns[2].HorizontalAlign = HorizontalAlign.Left;
            //FpSpread2.Width = 800;
            //FpSpread2.Height = 500;
        }
        catch (Exception ex)
        {
            d2.sendErrorMail(ex, collegecode1, "FeedbackReport");
        }
    }

    public void gendformat3()
    {
        try
        {
            fair();
            DataView dv = new DataView();
            FpSpread3.Visible = true;
            rptprint1.Visible = true;
            string college_cd = "";
            if (Cbl_college.Items.Count > 0)
            {
                for (int i = 0; i < Cbl_college.Items.Count; i++)
                {
                    if (Cbl_college.Items[i].Selected == true)
                    {
                        if (college_cd == "")
                        {
                            college_cd = "" + Cbl_college.Items[i].Value.ToString() + "";
                        }
                        else
                        {
                            college_cd = college_cd + "','" + Convert.ToString(Cbl_college.Items[i].Value);
                        }
                    }
                }
            }
            string Batch_Year = "";
            for (int i = 0; i < cbl_batch.Items.Count; i++)
            {
                if (cbl_batch.Items[i].Selected == true)
                {
                    if (Batch_Year == "")
                    {
                        Batch_Year = "" + cbl_batch.Items[i].Value.ToString() + "";
                    }
                    else
                    {
                        Batch_Year = Batch_Year + "','" + cbl_batch.Items[i].Value.ToString() + "";
                    }
                }
            }
            string degree_code = "";
            for (int i = 0; i < cbl_branch.Items.Count; i++)
            {
                if (cbl_branch.Items[i].Selected == true)
                {
                    if (degree_code == "")
                    {
                        degree_code = "" + cbl_branch.Items[i].Value.ToString() + "";
                    }
                    else
                    {
                        degree_code = degree_code + "','" + cbl_branch.Items[i].Value.ToString() + "";
                    }
                }
            }
            string section = "";
            for (int i = 0; i < cbl_sec.Items.Count; i++)
            {
                if (cbl_sec.Items[i].Selected == true)
                {
                    if (section == "")
                    {
                        section = "" + cbl_sec.Items[i].Value.ToString() + "";
                    }
                    else
                    {
                        section = section + "','" + cbl_sec.Items[i].Value.ToString() + "";
                    }
                    if (cbl_sec.Items[i].Value == "Empty")
                    {
                        section = "";
                    }
                }
            }
            if (section.Trim() != "")
            {
                section = section + "','";
            }
            string semester = "";
            for (int i = 0; i < cbl_sem.Items.Count; i++)
            {
                if (cbl_sem.Items[i].Selected == true)
                {
                    if (semester == "")
                    {
                        semester = "" + cbl_sem.Items[i].Value.ToString() + "";
                    }
                    else
                    {
                        semester = semester + "','" + cbl_sem.Items[i].Value.ToString() + "";
                    }
                }
            }
            string type = "";
            if (rb_Acad.Checked == true)
            {
                type = "1";
            }
            else if (rb_Gend.Checked == true)
            {
                type = "2";
            }
            FpSpread3.Sheets[0].RowCount = 0;
            FpSpread3.Sheets[0].ColumnCount = 0;
            FpSpread3.CommandBar.Visible = false;
            FpSpread3.Sheets[0].AutoPostBack = true;
            FpSpread3.Sheets[0].ColumnHeader.RowCount = 1;
            FpSpread3.Sheets[0].RowHeader.Visible = false;
            FpSpread3.Sheets[0].ColumnCount = 9;
            FarPoint.Web.Spread.StyleInfo darkstyle = new FarPoint.Web.Spread.StyleInfo();
            darkstyle.BackColor = ColorTranslator.FromHtml("#0CA6CA");
            darkstyle.ForeColor = System.Drawing.Color.White;
            FpSpread3.ActiveSheetView.ColumnHeader.DefaultStyle = darkstyle;
            FpSpread3.Visible = true;
            //FpSpread3.Width = 971;
            //FpSpread3.Height = 500;
            FpSpread3.SaveChanges();
            FpSpread3.Sheets[0].ColumnHeader.Cells[0, 0].Text = "S.No";
            FpSpread3.Sheets[0].ColumnHeader.Cells[0, 0].Font.Bold = true;
            FpSpread3.Sheets[0].ColumnHeader.Cells[0, 0].HorizontalAlign = HorizontalAlign.Center;
            FpSpread3.Sheets[0].ColumnHeader.Cells[0, 0].Font.Name = "Book Antiqua";
            FpSpread3.Sheets[0].ColumnHeader.Cells[0, 0].Font.Size = FontUnit.Medium;
            FpSpread3.Sheets[0].Columns[0].HorizontalAlign = HorizontalAlign.Center;
            FpSpread3.Sheets[0].ColumnHeader.Cells[0, 1].Text = "BranchName";
            FpSpread3.Sheets[0].ColumnHeader.Cells[0, 1].Font.Bold = true;
            FpSpread3.Sheets[0].ColumnHeader.Cells[0, 1].Font.Name = "Book Antiqua";
            FpSpread3.Sheets[0].ColumnHeader.Cells[0, 1].Font.Size = FontUnit.Medium;
            FpSpread3.Sheets[0].ColumnHeader.Cells[0, 1].HorizontalAlign = HorizontalAlign.Center;
            FpSpread3.Sheets[0].ColumnHeader.Cells[0, 2].Text = "Semester ";
            FpSpread3.Sheets[0].ColumnHeader.Cells[0, 2].Font.Bold = true;
            FpSpread3.Sheets[0].ColumnHeader.Cells[0, 2].Font.Name = "Book Antiqua";
            FpSpread3.Sheets[0].ColumnHeader.Cells[0, 2].Font.Size = FontUnit.Medium;
            FpSpread3.Sheets[0].ColumnHeader.Cells[0, 2].HorizontalAlign = HorizontalAlign.Center;
            FpSpread3.Sheets[0].ColumnHeader.Cells[0, 3].Text = "SectionName ";
            FpSpread3.Sheets[0].ColumnHeader.Cells[0, 3].Font.Bold = true;
            FpSpread3.Sheets[0].ColumnHeader.Cells[0, 3].Font.Name = "Book Antiqua";
            FpSpread3.Sheets[0].ColumnHeader.Cells[0, 3].Font.Size = FontUnit.Medium;
            FpSpread3.Sheets[0].ColumnHeader.Cells[0, 3].HorizontalAlign = HorizontalAlign.Center;
            FpSpread3.Sheets[0].ColumnHeader.Cells[0, 4].Text = "FromYear";
            FpSpread3.Sheets[0].ColumnHeader.Cells[0, 4].Font.Bold = true;
            FpSpread3.Sheets[0].ColumnHeader.Cells[0, 4].Font.Name = "Book Antiqua";
            FpSpread3.Sheets[0].ColumnHeader.Cells[0, 4].Font.Size = FontUnit.Medium;
            FpSpread3.Sheets[0].ColumnHeader.Cells[0, 4].HorizontalAlign = HorizontalAlign.Center;
            FpSpread3.Sheets[0].Columns[4].HorizontalAlign = HorizontalAlign.Center;
            FpSpread3.Sheets[0].ColumnHeader.Cells[0, 5].Text = "ToYear";
            FpSpread3.Sheets[0].ColumnHeader.Cells[0, 5].Font.Bold = true;
            FpSpread3.Sheets[0].ColumnHeader.Cells[0, 5].Font.Name = "Book Antiqua";
            FpSpread3.Sheets[0].ColumnHeader.Cells[0, 5].Font.Size = FontUnit.Medium;
            FpSpread3.Sheets[0].ColumnHeader.Cells[0, 5].HorizontalAlign = HorizontalAlign.Center;
            FpSpread3.Sheets[0].ColumnHeader.Cells[0, 6].Text = "Total Strength";
            FpSpread3.Sheets[0].ColumnHeader.Cells[0, 6].Font.Bold = true;
            FpSpread3.Sheets[0].ColumnHeader.Cells[0, 6].Font.Name = "Book Antiqua";
            FpSpread3.Sheets[0].ColumnHeader.Cells[0, 6].Font.Size = FontUnit.Medium;
            FpSpread3.Sheets[0].ColumnHeader.Cells[0, 6].HorizontalAlign = HorizontalAlign.Center;
            FpSpread3.Sheets[0].ColumnHeader.Cells[0, 7].Text = "Total Attended";
            FpSpread3.Sheets[0].ColumnHeader.Cells[0, 7].Font.Bold = true;
            FpSpread3.Sheets[0].ColumnHeader.Cells[0, 7].Font.Name = "Book Antiqua";
            FpSpread3.Sheets[0].ColumnHeader.Cells[0, 7].Font.Size = FontUnit.Medium;
            FpSpread3.Sheets[0].ColumnHeader.Cells[0, 7].HorizontalAlign = HorizontalAlign.Center;
            FpSpread3.Sheets[0].ColumnHeader.Cells[0, 8].Text = "Total Remaining";
            FpSpread3.Sheets[0].ColumnHeader.Cells[0, 8].Font.Bold = true;
            FpSpread3.Sheets[0].ColumnHeader.Cells[0, 8].Font.Name = "Book Antiqua";
            FpSpread3.Sheets[0].ColumnHeader.Cells[0, 8].Font.Size = FontUnit.Medium;
            FpSpread3.Sheets[0].ColumnHeader.Cells[0, 8].HorizontalAlign = HorizontalAlign.Center;
            FpSpread3.Sheets[0].ColumnHeader.Columns[0].Width = 59;
            ds.Clear();
            int rmaining = 0;
            string selqry = "";
            selqry = " SELECT Course_Name+'-'+Dept_Name Degree,Current_semester,Sections,R.Batch_Year, R.degree_code, COUNT(distinct f.app_no)as Strength FROM CO_StudFeedBack F,CO_FeedBackMaster M,Registration R,Degree G,Course C,Department D WHERE F.App_No = R.App_No AND R.degree_code = G.Degree_Code AND G.Course_Id = C.Course_Id  AND G.college_code = C.college_code AND g.Dept_Code = d.Dept_Code and g.college_code = d.college_code and f.FeedBackMasterFK = m.FeedBackMasterPK and m.FeedBackName='" + ddl_Feedbackname.SelectedItem.Value + "' and m.FeedBackType = '" + type + "' AND CC = 0 AND DelFlag = 0 AND Exam_Flag = 'OK' and R.degree_code in ('" + degree_code + "') and R.college_code in ('" + college_cd + "') and R.Batch_Year in ('" + Batch_Year + "') and R.Current_Semester in ('" + semester + "')  GROUP BY Course_Name,Dept_Name,Current_semester,Sections,R.Batch_Year, R.degree_code ORDER BY R.Batch_Year,Course_Name,Dept_Name,Current_Semester,Sections";
            selqry = selqry + " SELECT Course_Name+'-'+Dept_Name Degree,Current_semester,Sections,Batch_Year, R.degree_code,COUNT(*)as TotStrengh FROM Registration R,Degree G,Course C,Department D WHERE R.degree_code = G.Degree_Code AND G.Course_Id = C.Course_Id AND G.college_code = C.college_code AND g.Dept_Code = d.Dept_Code and g.college_code = d.college_code  AND CC = 0 AND DelFlag = 0 AND Exam_Flag = 'OK' and R.college_code in ('" + college_cd + "') and R.degree_code in ('" + degree_code + "') and R.Batch_Year in ('" + Batch_Year + "') and R.Current_Semester in ('" + semester + "')";
            if (section != "")
            {
                selqry = selqry + " and R.Sections in ('" + section + "') GROUP BY Course_Name,Dept_Name,Current_semester,Sections,Batch_Year,R.degree_code, R.college_code  ORDER BY  R.degree_code ";
                //Batch_Year,Course_Name,Dept_Name,Current_Semester,Sections";
            }
            else
            {
                selqry = selqry + " GROUP BY Course_Name,Dept_Name,Current_semester,Sections,Batch_Year,R.degree_code,R.college_code ORDER BY Batch_Year,Course_Name,Dept_Name,Current_Semester,Sections";
            }
            ds = d2.select_method_wo_parameter(selqry, "Text");
            DataView dvnew = new DataView();
            if (ds.Tables.Count > 0)
            {
                if (ds.Tables[0].Rows.Count > 0 && ds.Tables[1].Rows.Count > 0)
                {
                    // FpSpread3.Sheets[0].Rows.Count = ds.Tables[0].Rows.Count;
                    for (int i = 0; i < ds.Tables[1].Rows.Count; i++)
                    {
                        FpSpread3.Sheets[0].Rows.Count++;
                        FpSpread3.Sheets[0].Cells[FpSpread3.Sheets[0].RowCount - 1, 0].Text = Convert.ToString(i + 1);
                        FpSpread3.Sheets[0].Cells[FpSpread3.Sheets[0].RowCount - 1, 1].Text = ds.Tables[1].Rows[i]["Degree"].ToString();
                        FpSpread3.Sheets[0].Cells[FpSpread3.Sheets[0].RowCount - 1, 1].Tag = ds.Tables[1].Rows[i]["degree_code"].ToString();
                        FpSpread3.Sheets[0].Cells[FpSpread3.Sheets[0].RowCount - 1, 2].Text = ds.Tables[1].Rows[i]["Current_semester"].ToString();
                        if (ds.Tables[1].Rows[i]["Sections"].ToString() != "")
                        {
                            FpSpread3.Sheets[0].Cells[FpSpread3.Sheets[0].RowCount - 1, 3].Text = ds.Tables[1].Rows[i]["Sections"].ToString();
                        }
                        else
                        {
                            FpSpread3.Sheets[0].Cells[FpSpread3.Sheets[0].RowCount - 1, 3].Text = "-";
                        }
                        FpSpread3.Sheets[0].Cells[FpSpread3.Sheets[0].RowCount - 1, 4].Text = ds.Tables[1].Rows[i]["Batch_Year"].ToString();
                        FpSpread3.Sheets[0].Cells[FpSpread3.Sheets[0].RowCount - 1, 5].Text = ds.Tables[1].Rows[i]["Batch_Year"].ToString();
                        if (ds.Tables[1].Rows[i]["TotStrengh"].ToString() != "")
                        {
                            FpSpread3.Sheets[0].Cells[FpSpread3.Sheets[0].RowCount - 1, 6].Text = ds.Tables[1].Rows[i]["TotStrengh"].ToString();
                        }
                        else
                        {
                            FpSpread3.Sheets[0].Cells[FpSpread3.Sheets[0].RowCount - 1, 6].Text = "-";
                        }
                        string sectons = Convert.ToString(ds.Tables[1].Rows[i]["Sections"]);
                        string degrecode = Convert.ToString(ds.Tables[1].Rows[i]["degree_code"]);
                        string sem = Convert.ToString(ds.Tables[1].Rows[i]["Current_semester"]);
                        string filterquery = "";
                        int remaining = 0;
                        int attand = 0;
                        filterquery = "degree_code='" + degrecode + "' and  Current_semester='" + sem + "'";
                        if (sectons.Trim() != "")
                        {
                            filterquery = filterquery + " and Sections='" + sectons + "'";
                        }
                        ds.Tables[0].DefaultView.RowFilter = "" + filterquery + "";
                        dvnew = ds.Tables[0].DefaultView;
                        if (dvnew.Count > 0)
                        {
                            if (ds.Tables[1].Rows[i]["TotStrengh"].ToString() != "")
                            {
                                FpSpread3.Sheets[0].Cells[FpSpread3.Sheets[0].RowCount - 1, 7].Text = Convert.ToString(dvnew[0]["Strength"]);
                                attand = Convert.ToInt32(dvnew[0]["Strength"]);
                            }
                        }
                        else
                        {
                            FpSpread3.Sheets[0].Cells[FpSpread3.Sheets[0].RowCount - 1, 7].Text = "-";
                        }
                        int total = 0;
                        total = Convert.ToInt32(ds.Tables[1].Rows[i]["TotStrengh"]);
                        remaining = total - attand;
                        FpSpread3.Sheets[0].Cells[FpSpread3.Sheets[0].RowCount - 1, 8].Text = Convert.ToString(remaining);
                    }
                    //for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    //{
                    //    FpSpread3.Sheets[0].Cells[FpSpread3.Sheets[0].RowCount - 1, 7].Text = ds.Tables[0].Rows[i]["Strength"].ToString();
                    //}
                    FpSpread3.Sheets[0].Columns[2].HorizontalAlign = HorizontalAlign.Center;
                    FpSpread3.Sheets[0].Columns[5].HorizontalAlign = HorizontalAlign.Center;
                    FpSpread3.Sheets[0].Columns[6].HorizontalAlign = HorizontalAlign.Center;
                    FpSpread3.Sheets[0].Columns[7].HorizontalAlign = HorizontalAlign.Center;
                    FpSpread3.Sheets[0].Columns[8].HorizontalAlign = HorizontalAlign.Center;
                    FpSpread3.Sheets[0].Columns[3].HorizontalAlign = HorizontalAlign.Center;
                    FpSpread3.Sheets[0].PageSize = ds.Tables[1].Rows.Count;
                    //FpSpread3.Width = 800;
                    //FpSpread3.Height = 500;
                }
                else
                {
                    //div1.Visible = false;
                    imgdiv2.Visible = true;
                    lbl_alert1.Text = "No Records Found";
                    FpSpread3.Visible = false;
                    rptprint1.Visible = false;
                }
            }
            else
            {
                //div1.Visible = false;
                imgdiv2.Visible = true;
                lbl_alert1.Text = "No Records Found";
                FpSpread3.Visible = false;
                rptprint1.Visible = false;
            }
            FpSpread3.Sheets[0].PageSize = FpSpread3.Sheets[0].RowCount;
        }
        catch (Exception ex)
        {
            d2.sendErrorMail(ex, collegecode1, "FeedbackReport");
        }
    }

    public void format2a()
    {
        // select SUM(point)as total,cs.QuestionMasterFK,COUNT(distinct cs.app_no)as Strength,Cm.Point   from CO_StudFeedBack cs,CO_MarkMaster cm, CO_FeedBackMaster fm where cs.MarkMasterPK =cm.MarkMasterPK and fm.FeedBackMasterPK =cs.FeedBackMasterFK and fm.DegreeCode in ('45','46','47','50','51','52','53','54') and fm.CollegeCode in ('13') and fm.Batch_Year in ('2015') and fm.FeedBackName='Academic Feedback  Test' and fm.semester in ('1')   and fm.Section in ('A','B','C','') and fm.FeedBackType='1' group by  cs.QuestionMasterFK,Cm.Point 
        try
        {
            chartprint.Visible = false;
            chartfalse();
            FpSpread2.Sheets[0].Rows.Count = 0;
            FpSpread2.Sheets[0].Columns.Count = 0;
            FpSpread2.SaveChanges();
            fair();
            cb_total.Visible = true;
            cb_avg.Visible = true;
            FpSpread2.Visible = true;
            rptprint1.Visible = true;
            string Batch_Year = "";
            for (int i = 0; i < cbl_batch.Items.Count; i++)
            {
                if (cbl_batch.Items[i].Selected == true)
                {
                    if (Batch_Year == "")
                    {
                        Batch_Year = "" + cbl_batch.Items[i].Value.ToString() + "";
                    }
                    else
                    {
                        Batch_Year = Batch_Year + "','" + cbl_batch.Items[i].Value.ToString() + "";
                    }
                }
            }
            string college_cd = "";
            if (Cbl_college.Items.Count > 0)
            {
                for (int i = 0; i < Cbl_college.Items.Count; i++)
                {
                    if (Cbl_college.Items[i].Selected == true)
                    {
                        if (college_cd == "")
                        {
                            college_cd = "" + Cbl_college.Items[i].Value.ToString() + "";
                        }
                        else
                        {
                            college_cd = college_cd + "','" + Convert.ToString(Cbl_college.Items[i].Value);
                        }
                    }
                }
            }
            string degree_code = "";
            for (int i = 0; i < cbl_branch.Items.Count; i++)
            {
                if (cbl_branch.Items[i].Selected == true)
                {
                    if (degree_code == "")
                    {
                        degree_code = "" + cbl_branch.Items[i].Value.ToString() + "";
                    }
                    else
                    {
                        degree_code = degree_code + "','" + cbl_branch.Items[i].Value.ToString() + "";
                    }
                }
            }
            string semester = "";
            for (int i = 0; i < cbl_sem.Items.Count; i++)
            {
                if (cbl_sem.Items[i].Selected == true)
                {
                    if (semester == "")
                    {
                        semester = "" + cbl_sem.Items[i].Value.ToString() + "";
                    }
                    else
                    {
                        semester = semester + "','" + cbl_sem.Items[i].Value.ToString() + "";
                    }
                }
            }
            string section = "";
            for (int i = 0; i < cbl_sec.Items.Count; i++)
            {
                if (cbl_sec.Items[i].Selected == true)
                {
                    if (section == "")
                    {
                        section = "" + cbl_sec.Items[i].Value.ToString() + "";
                    }
                    else
                    {
                        section = section + "','" + cbl_sec.Items[i].Value.ToString() + "";
                    }
                    if (cbl_sec.Items[i].Value == "Empty")
                    {
                        section = "";
                    }
                }
            }
            if (section.Trim() != "")
            {
                section = section + "','";
            }
            string fbpk = " select FeedBackMasterPK from CO_FeedBackMaster where FeedBackName ='" + ddl_Feedbackname.SelectedItem.Value + "' and DegreeCode in ('" + degree_code + "') and semester in ('" + semester + "') and Batch_Year in('" + Batch_Year + "') and section in ('" + section + "')";
            dsfb = d2.select_method_wo_parameter(fbpk, "Text");
            string feedbakpk = "";
            if (dsfb.Tables.Count > 0)
            {
                if (dsfb.Tables[0].Rows.Count > 0)
                {
                    for (int pk = 0; pk < dsfb.Tables[0].Rows.Count; pk++)
                    {
                        if (feedbakpk == "")
                        {
                            feedbakpk = dsfb.Tables[0].Rows[pk]["FeedBackMasterPK"].ToString();
                        }
                        else
                        {
                            feedbakpk = feedbakpk + "','" + dsfb.Tables[0].Rows[pk]["FeedBackMasterPK"].ToString() + "";
                        }
                    }
                }
            }
            string type = "";
            if (rb_Acad.Checked == true)
            {
                type = "1";
            }
            else if (rb_Gend.Checked == true)
            {
                type = "2";
            }
            FpSpread2.Sheets[0].RowCount = 0;
            //FpSpread2.Sheets[0].ColumnCount = 0;
            FpSpread2.CommandBar.Visible = false;
            FpSpread2.Sheets[0].AutoPostBack = true;
            FpSpread2.Sheets[0].ColumnHeader.RowCount = 3;
            FpSpread2.Sheets[0].RowHeader.Visible = false;
            FpSpread2.Sheets[0].ColumnCount = 2;
            FarPoint.Web.Spread.StyleInfo darkstyle = new FarPoint.Web.Spread.StyleInfo();
            darkstyle.BackColor = ColorTranslator.FromHtml("#0CA6CA");
            darkstyle.ForeColor = System.Drawing.Color.White;
            FpSpread2.ActiveSheetView.ColumnHeader.DefaultStyle = darkstyle;
            FpSpread2.Visible = true;
            //FpSpread2.Width = 971;
            //FpSpread2.Height = 500;
            FpSpread2.SaveChanges();
            FpSpread2.Sheets[0].ColumnHeader.Cells[0, 0].Text = "S.No";
            FpSpread2.Sheets[0].ColumnHeader.Cells[0, 0].Font.Bold = true;
            FpSpread2.Sheets[0].ColumnHeader.Cells[0, 0].HorizontalAlign = HorizontalAlign.Center;
            FpSpread2.Sheets[0].ColumnHeader.Cells[0, 0].Font.Name = "Book Antiqua";
            FpSpread2.Sheets[0].ColumnHeader.Cells[0, 0].Font.Size = FontUnit.Medium;
            FpSpread2.Sheets[0].Columns[0].HorizontalAlign = HorizontalAlign.Center;
            FpSpread2.Sheets[0].ColumnHeaderSpanModel.Add(0, 0, 3, 1);
            FpSpread2.Sheets[0].ColumnHeader.Cells[0, 1].Text = "TYPE";
            FpSpread2.Sheets[0].ColumnHeader.Cells[0, 1].Font.Bold = true;
            FpSpread2.Sheets[0].ColumnHeader.Cells[0, 1].Font.Name = "Book Antiqua";
            FpSpread2.Sheets[0].ColumnHeader.Cells[0, 1].Font.Size = FontUnit.Medium;
            FpSpread2.Sheets[0].ColumnHeader.Cells[0, 1].HorizontalAlign = HorizontalAlign.Center;
            FpSpread2.Sheets[0].ColumnHeader.Cells[1, 1].Text = "MARKS";
            FpSpread2.Sheets[0].ColumnHeaderSpanModel.Add(1, 1, 2, 1);
            FpSpread2.Sheets[0].ColumnHeader.Cells[1, 1].Font.Name = "Book Antiqua";
            FpSpread2.Sheets[0].ColumnHeader.Cells[1, 1].Font.Size = FontUnit.Medium;
            FpSpread2.Sheets[0].ColumnHeader.Cells[1, 1].HorizontalAlign = HorizontalAlign.Center;
            //FpSpread2.Sheets[0].ColumnHeader.Columns[0].Width = 59;
            //FpSpread2.Sheets[0].ColumnHeader.Columns[1].Width = 100;
            ds.Clear();
            string subject_cd = "";
            string selectquestion = "";
            string selqry = "";
            // selqry = " select DISTINCT(subject_code),c.subject_no,subject_name from Registration r,subjectChooser c ,subject s where r.Roll_No = c.roll_no and r.Current_Semester = c.semester and c.subject_no = s.subject_no and Current_Semester in ('" + semester + "') and r.college_code in ('" + college_cd + "') and r.degree_code in ('" + degree_code + "')  and r.Batch_Year in ('" + Batch_Year + "')";
            selqry = "SELECT MarkMasterPK, MarkType , Point, CollegeCode FROM CO_MarkMaster where CollegeCode in ('" + college_cd + "') ";
            ds = d2.select_method_wo_parameter(selqry, "Text");
            if (ds.Tables.Count > 0)
            {
                if (ds.Tables[0].Rows.Count > 0)
                {
                    for (int row = 0; row < ds.Tables[0].Rows.Count; row++)
                    {
                        if (cb_total.Checked == true && cb_avg.Checked == true)
                        {
                            FpSpread2.Sheets[0].ColumnCount++;
                            //FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, 7, 1, 3);
                            FpSpread2.Sheets[0].ColumnHeader.Cells[0, FpSpread2.Sheets[0].ColumnCount - 1].Text = Convert.ToString(ds.Tables[0].Rows[row]["MarkType"]);
                            FpSpread2.Sheets[0].ColumnHeader.Cells[1, FpSpread2.Sheets[0].ColumnCount - 1].Text = Convert.ToString(ds.Tables[0].Rows[row]["Point"]);
                            FpSpread2.Sheets[0].ColumnHeader.Cells[0, FpSpread2.Sheets[0].ColumnCount - 1].Font.Bold = true;
                            FpSpread2.Sheets[0].ColumnHeader.Cells[0, FpSpread2.Sheets[0].ColumnCount - 1].Font.Name = "Book Antiqua";
                            FpSpread2.Sheets[0].ColumnHeader.Cells[0, FpSpread2.Sheets[0].ColumnCount - 1].Font.Size = FontUnit.Medium;
                            FpSpread2.Sheets[0].ColumnHeader.Cells[0, FpSpread2.Sheets[0].ColumnCount - 1].HorizontalAlign = HorizontalAlign.Center;
                            FpSpread2.Sheets[0].ColumnHeader.Cells[1, FpSpread2.Sheets[0].ColumnCount - 1].Font.Bold = true;
                            FpSpread2.Sheets[0].ColumnHeader.Cells[1, FpSpread2.Sheets[0].ColumnCount - 1].Font.Name = "Book Antiqua";
                            FpSpread2.Sheets[0].ColumnHeader.Cells[1, FpSpread2.Sheets[0].ColumnCount - 1].Font.Size = FontUnit.Medium;
                            FpSpread2.Sheets[0].ColumnHeader.Cells[1, FpSpread2.Sheets[0].ColumnCount - 1].HorizontalAlign = HorizontalAlign.Center;
                            FpSpread2.Sheets[0].ColumnHeader.Cells[2, FpSpread2.Sheets[0].ColumnCount - 1].Font.Bold = true;
                            FpSpread2.Sheets[0].ColumnHeader.Cells[2, FpSpread2.Sheets[0].ColumnCount - 1].Font.Name = "Book Antiqua";
                            FpSpread2.Sheets[0].ColumnHeader.Cells[2, FpSpread2.Sheets[0].ColumnCount - 1].Font.Size = FontUnit.Medium;
                            FpSpread2.Sheets[0].ColumnHeader.Cells[2, FpSpread2.Sheets[0].ColumnCount - 1].HorizontalAlign = HorizontalAlign.Center;
                            FpSpread2.Sheets[0].ColumnHeader.Cells[2, FpSpread2.Sheets[0].ColumnCount - 1].Text = "Total";
                            FpSpread2.Sheets[0].ColumnCount++;
                            FpSpread2.Sheets[0].ColumnHeader.Cells[2, FpSpread2.Sheets[0].ColumnCount - 1].Text = "Avg";
                            FpSpread2.Sheets[0].ColumnHeader.Cells[2, FpSpread2.Sheets[0].ColumnCount - 1].Font.Bold = true;
                            FpSpread2.Sheets[0].ColumnHeader.Cells[2, FpSpread2.Sheets[0].ColumnCount - 1].Font.Name = "Book Antiqua";
                            FpSpread2.Sheets[0].ColumnHeader.Cells[2, FpSpread2.Sheets[0].ColumnCount - 1].Font.Size = FontUnit.Medium;
                            FpSpread2.Sheets[0].ColumnHeader.Cells[2, FpSpread2.Sheets[0].ColumnCount - 1].HorizontalAlign = HorizontalAlign.Center;
                            FpSpread2.Sheets[0].ColumnHeaderSpanModel.Add(0, FpSpread2.Sheets[0].ColumnCount - 2, 1, 2);
                            FpSpread2.Sheets[0].ColumnHeaderSpanModel.Add(1, FpSpread2.Sheets[0].ColumnCount - 2, 1, 2);
                        }
                        else if (cb_total.Checked == true || cb_avg.Checked == true)
                        {
                            FpSpread2.Sheets[0].ColumnCount++;
                            FpSpread2.Sheets[0].ColumnHeader.Cells[0, FpSpread2.Sheets[0].ColumnCount - 1].Text = Convert.ToString(ds.Tables[0].Rows[row]["MarkType"]);
                            FpSpread2.Sheets[0].ColumnHeader.Cells[1, FpSpread2.Sheets[0].ColumnCount - 1].Text = Convert.ToString(ds.Tables[0].Rows[row]["Point"]);
                            FpSpread2.Sheets[0].ColumnHeader.Cells[0, FpSpread2.Sheets[0].ColumnCount - 1].Font.Bold = true;
                            FpSpread2.Sheets[0].ColumnHeader.Cells[0, FpSpread2.Sheets[0].ColumnCount - 1].Font.Name = "Book Antiqua";
                            FpSpread2.Sheets[0].ColumnHeader.Cells[0, FpSpread2.Sheets[0].ColumnCount - 1].Font.Size = FontUnit.Medium;
                            FpSpread2.Sheets[0].ColumnHeader.Cells[0, FpSpread2.Sheets[0].ColumnCount - 1].HorizontalAlign = HorizontalAlign.Center;
                            FpSpread2.Sheets[0].ColumnHeader.Cells[1, FpSpread2.Sheets[0].ColumnCount - 1].Font.Bold = true;
                            FpSpread2.Sheets[0].ColumnHeader.Cells[1, FpSpread2.Sheets[0].ColumnCount - 1].Font.Name = "Book Antiqua";
                            FpSpread2.Sheets[0].ColumnHeader.Cells[1, FpSpread2.Sheets[0].ColumnCount - 1].Font.Size = FontUnit.Medium;
                            FpSpread2.Sheets[0].ColumnHeader.Cells[1, FpSpread2.Sheets[0].ColumnCount - 1].HorizontalAlign = HorizontalAlign.Center;
                            FpSpread2.Sheets[0].ColumnHeader.Cells[2, FpSpread2.Sheets[0].ColumnCount - 1].Font.Bold = true;
                            FpSpread2.Sheets[0].ColumnHeader.Cells[2, FpSpread2.Sheets[0].ColumnCount - 1].Font.Name = "Book Antiqua";
                            FpSpread2.Sheets[0].ColumnHeader.Cells[2, FpSpread2.Sheets[0].ColumnCount - 1].Font.Size = FontUnit.Medium;
                            FpSpread2.Sheets[0].ColumnHeader.Cells[2, FpSpread2.Sheets[0].ColumnCount - 1].HorizontalAlign = HorizontalAlign.Center;
                            if (cb_total.Checked == true)
                            {
                                FpSpread2.Sheets[0].ColumnHeader.Cells[2, FpSpread2.Sheets[0].ColumnCount - 1].Text = "Total";
                            }
                            else
                            {
                                FpSpread2.Sheets[0].ColumnHeader.Cells[2, FpSpread2.Sheets[0].ColumnCount - 1].Text = "Avg";
                            }
                        }
                    }
                    FpSpread2.Sheets[0].ColumnCount++;
                    FpSpread2.Sheets[0].ColumnHeader.Cells[0, FpSpread2.Sheets[0].ColumnCount - 1].Text = "GrandTotal";
                    FpSpread2.Sheets[0].ColumnHeaderSpanModel.Add(0, FpSpread2.Sheets[0].ColumnCount - 1, 3, 1);
                    FpSpread2.Sheets[0].ColumnHeader.Cells[0, FpSpread2.Sheets[0].ColumnCount - 1].Font.Bold = true;
                    FpSpread2.Sheets[0].ColumnHeader.Cells[0, FpSpread2.Sheets[0].ColumnCount - 1].Font.Name = "Book Antiqua";
                    FpSpread2.Sheets[0].ColumnHeader.Cells[0, FpSpread2.Sheets[0].ColumnCount - 1].Font.Size = FontUnit.Medium;
                    FpSpread2.Sheets[0].ColumnHeader.Cells[0, FpSpread2.Sheets[0].ColumnCount - 1].HorizontalAlign = HorizontalAlign.Center;
                    FpSpread2.Sheets[0].ColumnCount++;
                    FpSpread2.Sheets[0].ColumnHeader.Cells[0, FpSpread2.Sheets[0].ColumnCount - 1].Text = "GrandAvg";
                    FpSpread2.Sheets[0].ColumnHeaderSpanModel.Add(0, FpSpread2.Sheets[0].ColumnCount - 1, 3, 1);
                    FpSpread2.Sheets[0].ColumnHeader.Cells[0, FpSpread2.Sheets[0].ColumnCount - 1].Font.Bold = true;
                    FpSpread2.Sheets[0].ColumnHeader.Cells[0, FpSpread2.Sheets[0].ColumnCount - 1].Font.Name = "Book Antiqua";
                    FpSpread2.Sheets[0].ColumnHeader.Cells[0, FpSpread2.Sheets[0].ColumnCount - 1].Font.Size = FontUnit.Medium;
                    FpSpread2.Sheets[0].ColumnHeader.Cells[0, FpSpread2.Sheets[0].ColumnCount - 1].HorizontalAlign = HorizontalAlign.Center;
                    selectquestion = "select distinct (select TextVal from TextValTable where TextCode = HeaderCode) as HeaderName,HeaderCode,Question,QuestionMasterPK from CO_QuestionMaster cq,CO_FeedBackMaster cf,CO_FeedBackQuestions cb,CO_StudFeedBack s where cq.QuestionMasterPK =cb.QuestionMasterFK and cf.FeedBackMasterPK =cb.FeedBackMasterFK and cf.CollegeCode in ('" + college_cd + "') and  s.FeedBackMasterFK =cf.FeedBackMasterPK and s.QuestionMasterFK =cq.QuestionMasterPK and  cf.FeedBackType =('" + type + "') and cf.DegreeCode in ('" + degree_code + "') and cf.Batch_Year in ('" + Batch_Year + "') and cf.FeedBackMasterPK in('" + feedbakpk + "') and cf.semester in ('" + semester + "') ";
                    if (section != "")
                    {
                        selectquestion = selectquestion + " and cf.Section in ('" + section + "')";
                    }
                    selectquestion = selectquestion + " select SUM(point)as total,cs.QuestionMasterFK,COUNT( cs.app_no)as Strength,Cm.Point   from CO_StudFeedBack cs,CO_MarkMaster cm, CO_FeedBackMaster fm where cs.MarkMasterPK =cm.MarkMasterPK and fm.FeedBackMasterPK =cs.FeedBackMasterFK and fm.DegreeCode in ('" + degree_code + "') and fm.CollegeCode in ('" + college_cd + "') and fm.Batch_Year in ('" + Batch_Year + "') and fm.FeedBackName='" + ddl_Feedbackname.SelectedItem.Value + "' and fm.semester in ('" + semester + "') and ISNULL(app_no,0) <>0  ";
                    if (section != "")
                    {
                        selectquestion = selectquestion + " and fm.Section in ('" + section + "') and fm.FeedBackType='1' group by  cs.QuestionMasterFK,Cm.Point";
                    }
                    else
                    {
                        selectquestion = selectquestion + " and fm.FeedBackType='1' group by   cs.QuestionMasterFK,Cm.Point";
                    }
                    DataView dv = new DataView();
                    ds = d2.select_method_wo_parameter(selectquestion, "Text");
                    if (ds.Tables.Count > 0)
                    {
                        if (ds.Tables[0].Rows.Count > 0 && ds.Tables[1].Rows.Count > 0)
                        {
                            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                            {
                                Double total = 0;
                                Double streng = 0;
                                string qtionpk = "";
                                string needs = "5";
                                string sum_total = d2.GetFunction("select top 1 Point from CO_MarkMaster  where CollegeCode in('" + collegecode1 + "') order by Point desc");
                                //string sum_total = "9";
                                string points = "";
                                FpSpread2.Sheets[0].Rows.Count++;
                                FpSpread2.Sheets[0].Cells[i, 0].Text = Convert.ToString(i + 1);
                                if (((i + 1) % 2) == 0)
                                {
                                    FpSpread2.Sheets[0].Rows[FpSpread2.Sheets[0].RowCount - 1].BackColor = System.Drawing.Color.LightSeaGreen;
                                }
                                FpSpread2.Sheets[0].Cells[i, 1].Tag = ds.Tables[0].Rows[i]["QuestionMasterPK"].ToString();
                                qtionpk = Convert.ToString(FpSpread2.Sheets[0].Cells[i, 1].Tag);
                                FpSpread2.Sheets[0].Cells[i, 1].Text = ds.Tables[0].Rows[i]["Question"].ToString();
                                FpSpread2.Sheets[0].Columns[1].HorizontalAlign = HorizontalAlign.Left;
                                if (cb_total.Checked == true && cb_avg.Checked == true)
                                {
                                    for (int col = 2; col < FpSpread2.Sheets[0].ColumnCount - 1; col += 2)
                                    {
                                        string mark = Convert.ToString(FpSpread2.Sheets[0].ColumnHeader.Cells[1, col].Text);
                                        if (mark.Trim() != "")
                                        {
                                            ds.Tables[1].DefaultView.RowFilter = "Point='" + Convert.ToString(mark) + "' and QuestionMasterFK='" + qtionpk + "'";
                                            dv = ds.Tables[1].DefaultView;
                                            if (dv.Count > 0)
                                            {
                                                FpSpread2.Sheets[0].Cells[i, col].Text = Convert.ToString(dv[0]["total"]);
                                                total = total + Convert.ToDouble(dv[0]["total"]);
                                                streng = streng + Convert.ToDouble(dv[0]["Strength"]);
                                                FpSpread2.Sheets[0].Cells[i, col].HorizontalAlign = HorizontalAlign.Center;
                                                Double tot = Convert.ToDouble(dv[0]["total"]);
                                                Double strengt = Convert.ToDouble(dv[0]["Strength"]);
                                                Double bas_avg = tot / strengt;
                                                points = Convert.ToString(bas_avg);
                                                //sum_total = d2.GetFunction("select top 1 Point from CO_MarkMaster  where CollegeCode in('" + collegecode1 + "') order by Point desc");
                                                ConvertedMark(needs, sum_total, ref points);
                                                string aaa = Convert.ToString(points);
                                                FpSpread2.Sheets[0].Cells[i, col + 1].Text = aaa;
                                                FpSpread2.Sheets[0].Cells[i, col + 1].HorizontalAlign = HorizontalAlign.Center;
                                            }
                                            else
                                            {
                                                FpSpread2.Sheets[0].Cells[i, col].Text = "-";
                                                FpSpread2.Sheets[0].Cells[i, col + 1].Text = "-";
                                                FpSpread2.Sheets[0].Cells[i, col].HorizontalAlign = HorizontalAlign.Center;
                                                FpSpread2.Sheets[0].Cells[i, col + 1].HorizontalAlign = HorizontalAlign.Center;
                                            }
                                        }
                                    }
                                }
                                else if (cb_total.Checked == true || cb_avg.Checked == true)
                                {
                                    for (int col = 2; col < FpSpread2.Sheets[0].ColumnCount - 1; col++)
                                    {
                                        string mark = Convert.ToString(FpSpread2.Sheets[0].ColumnHeader.Cells[1, col].Text);
                                        if (mark.Trim() != "")
                                        {
                                            ds.Tables[1].DefaultView.RowFilter = "Point='" + Convert.ToString(mark) + "' and QuestionMasterFK='" + qtionpk + "'";
                                            dv = ds.Tables[1].DefaultView;
                                            if (dv.Count > 0)
                                            {
                                                total = total + Convert.ToDouble(dv[0]["total"]);
                                                streng = streng + Convert.ToDouble(dv[0]["Strength"]);
                                                FpSpread2.Sheets[0].Cells[i, col].HorizontalAlign = HorizontalAlign.Center;
                                                double tot = Convert.ToDouble(dv[0]["total"]);
                                                double strengt = Convert.ToDouble(dv[0]["Strength"]);
                                                Double bas_avg = tot / strengt;
                                                points = Convert.ToString(bas_avg);
                                                ConvertedMark(needs, sum_total, ref points);
                                                string aaa = Convert.ToString(points);
                                                if (cb_total.Checked == true)
                                                {
                                                    if (tot != 0)
                                                    {
                                                        FpSpread2.Sheets[0].Cells[i, col].Text = Convert.ToString(dv[0]["total"]);
                                                    }
                                                }
                                                if (cb_avg.Checked == true)
                                                {
                                                    FpSpread2.Sheets[0].Cells[i, col].Text = aaa;
                                                    FpSpread2.Sheets[0].Cells[i, col + 1].HorizontalAlign = HorizontalAlign.Center;
                                                }
                                            }
                                            else
                                            {
                                                FpSpread2.Sheets[0].Cells[i, col].Text = "-";
                                                FpSpread2.Sheets[0].Cells[i, col + 1].Text = "-";
                                                FpSpread2.Sheets[0].Cells[i, col + 1].HorizontalAlign = HorizontalAlign.Center;
                                                FpSpread2.Sheets[0].Cells[i, col].HorizontalAlign = HorizontalAlign.Center;
                                            }
                                        }
                                    }
                                }
                                FpSpread2.Sheets[0].Cells[i, FpSpread2.Sheets[0].ColumnCount - 2].Text = Convert.ToString(total);
                                FpSpread2.Sheets[0].Cells[i, FpSpread2.Sheets[0].ColumnCount - 2].HorizontalAlign = HorizontalAlign.Center;
                                needs = "5";
                                sum_total = d2.GetFunction("select top 1 Point from CO_MarkMaster  where CollegeCode in('" + collegecode1 + "') order by Point desc");
                                // sum_total = "9";
                                Double point = total / streng;
                                // FpSpread2.Sheets[0].Cells[i, FpSpread2.Sheets[0].ColumnCount - 1].Text = Convert.ToString(point);
                                //point = Math.Round(point, 0, MidpointRounding.AwayFromZero);
                                points = Convert.ToString(point);
                                ConvertedMark(needs, sum_total, ref points);
                                string grandgrd = Convert.ToString(points);
                                Double totav = Convert.ToDouble(grandgrd);
                                totav = Convert.ToDouble(Math.Round(totav, 2));
                                FpSpread2.Sheets[0].Cells[i, FpSpread2.Sheets[0].ColumnCount - 1].Text = Convert.ToString(totav);
                                FpSpread2.Sheets[0].Cells[i, FpSpread2.Sheets[0].ColumnCount - 1].HorizontalAlign = HorizontalAlign.Center;
                            }
                            FpSpread2.Sheets[0].Rows.Count++;
                            if (cb_total.Checked == true && cb_avg.Checked == true)
                            {
                                for (int i = 2; i < FpSpread2.Sheets[0].ColumnCount; i += 2)
                                {
                                    string sum = "";
                                    string sum1 = "";
                                    Double text = 0;
                                    Double sumavg = 0;
                                    int min_count = 0;
                                    for (int row = 0; row < FpSpread2.Sheets[0].Rows.Count - 1; row++)
                                    {
                                        sum = Convert.ToString(FpSpread2.Sheets[0].Cells[row, i].Text);
                                        sum1 = Convert.ToString(FpSpread2.Sheets[0].Cells[row, i + 1].Text);
                                        if (sum != "-")
                                        {
                                            text = text + Convert.ToDouble(sum);
                                            sumavg = sumavg + Convert.ToDouble(sum1);
                                        }
                                        else if (sum == "-")
                                        {
                                            min_count++;
                                        }
                                    }
                                    FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].Rows.Count - 1, i].Text = Convert.ToString(text);
                                    FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].Rows.Count - 1, i].HorizontalAlign = HorizontalAlign.Center;
                                    int counts = FpSpread2.Sheets[0].Rows.Count - 1;
                                    counts = counts - min_count;
                                    Double avgs = Convert.ToDouble(sumavg);
                                    avgs = avgs / Convert.ToDouble(counts);
                                    avgs = Convert.ToDouble(Math.Round(avgs, 2));
                                    FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].Rows.Count - 1, i + 1].Text = Convert.ToString(avgs);
                                    FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].Rows.Count - 1, i + 1].HorizontalAlign = HorizontalAlign.Center;
                                }
                            }
                            else if (cb_total.Checked == true || cb_avg.Checked == true)
                            {
                                for (int i = 2; i < FpSpread2.Sheets[0].ColumnCount; i++)
                                {
                                    string sum = "";
                                    Double text = 0;
                                    int min_count = 0;
                                    for (int row = 0; row < FpSpread2.Sheets[0].Rows.Count - 1; row++)
                                    {
                                        sum = Convert.ToString(FpSpread2.Sheets[0].Cells[row, i].Text);
                                        if (sum != "-")
                                        {
                                            text = text + Convert.ToDouble(sum);
                                        }
                                        else if (sum == "-")
                                        {
                                            min_count++;
                                        }
                                    }
                                    if (cb_total.Checked == true)
                                    {
                                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].Rows.Count - 1, i].Text = Convert.ToString(text);
                                        if (i == FpSpread2.Sheets[0].ColumnCount - 1)
                                        {
                                            Double counts = FpSpread2.Sheets[0].Rows.Count - 1;
                                            Double avgs = Convert.ToDouble(text);
                                            avgs = avgs / Convert.ToDouble(counts);
                                            avgs = Convert.ToDouble(Math.Round(avgs, 2));
                                            FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].Rows.Count - 1, i].Text = Convert.ToString(avgs);
                                        }
                                    }
                                    if (cb_avg.Checked == true)
                                    {
                                        int counts = FpSpread2.Sheets[0].Rows.Count - 1;
                                        counts = counts - min_count;
                                        Double avgs = Convert.ToDouble(text);
                                        avgs = avgs / Convert.ToDouble(counts);
                                        avgs = Convert.ToDouble(Math.Round(avgs, 2));
                                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].Rows.Count - 1, i].Text = Convert.ToString(avgs);
                                        if (i == FpSpread2.Sheets[0].ColumnCount - 2)
                                        {
                                            FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].Rows.Count - 1, i].Text = Convert.ToString(text);
                                        }
                                    }
                                    FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].Rows.Count - 1, i].HorizontalAlign = HorizontalAlign.Center;
                                }
                            }
                            FpSpread2.Sheets[0].SpanModel.Add(FpSpread2.Sheets[0].RowCount - 1, 0, 1, 2);
                            FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].Rows.Count - 1, 0].Text = "Grand Total";
                            FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].Rows.Count - 1, 0].HorizontalAlign = HorizontalAlign.Right;
                            FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].Rows.Count - 1, 0].ForeColor = System.Drawing.Color.Blue;
                            FpSpread2.Visible = true;
                        }
                        else
                        {
                            // div1.Visible = false;
                            imgdiv2.Visible = true;
                            lbl_alert1.Text = "No Records Found";
                            FpSpread2.Visible = false;
                            rptprint1.Visible = false;
                        }
                    }
                    else
                    {
                        //div1.Visible = false;
                        imgdiv2.Visible = true;
                        lbl_alert1.Text = "No Records Found";
                        FpSpread2.Visible = false;
                        rptprint1.Visible = false;
                    }
                }
                else
                {
                    //div1.Visible = false;
                    imgdiv2.Visible = true;
                    lbl_alert1.Text = "No Records Found";
                    FpSpread2.Visible = false;
                    rptprint1.Visible = false;
                }
            }
            else
            {
                // div1.Visible = false;
                imgdiv2.Visible = true;
                lbl_alert1.Text = "No Records Found";
                FpSpread2.Visible = false;
                rptprint1.Visible = false;
            }
            FpSpread2.Sheets[0].PageSize = FpSpread2.Sheets[0].RowCount;
            FpSpread2.Sheets[0].Columns[1].Width = 550;
            //FpSpread2.Width = 800;
            //FpSpread2.Height = 500;
        }
        catch (Exception ex)
        {
            d2.sendErrorMail(ex, collegecode1, "FeedbackReport");
        }
    }

    public void gendformat1ananames()
    {
        try
        {
            FpSpread2.Visible = false;
            fair();
            FpSpread2.Visible = true;
            rptprint1.Visible = true;
            string type = "";
            if (rb_Acad.Checked == true)
            {
                type = "1";
            }
            else if (rb_Gend.Checked == true)
            {
                type = "2";
            }
            string college_cd = "";
            if (Cbl_college.Items.Count > 0)
            {
                for (int i = 0; i < Cbl_college.Items.Count; i++)
                {
                    if (Cbl_college.Items[i].Selected == true)
                    {
                        if (college_cd == "")
                        {
                            college_cd = "" + Cbl_college.Items[i].Value.ToString() + "";
                        }
                        else
                        {
                            college_cd = college_cd + "','" + Convert.ToString(Cbl_college.Items[i].Value);
                        }
                    }
                }
            }
            FpSpread2.Sheets[0].RowCount = 0;
            FpSpread2.Sheets[0].ColumnCount = 0;
            FpSpread2.CommandBar.Visible = false;
            FpSpread2.Sheets[0].AutoPostBack = true;
            FpSpread2.Sheets[0].ColumnHeader.RowCount = 1;
            FpSpread2.Sheets[0].RowHeader.Visible = false;
            FpSpread2.Sheets[0].ColumnCount = 4;
            FarPoint.Web.Spread.StyleInfo darkstyle = new FarPoint.Web.Spread.StyleInfo();
            darkstyle.BackColor = ColorTranslator.FromHtml("#0CA6CA");
            darkstyle.ForeColor = System.Drawing.Color.White;
            FpSpread2.ActiveSheetView.ColumnHeader.DefaultStyle = darkstyle;
            FpSpread2.Visible = true;
            //FpSpread2.Width = 971;
            //FpSpread2.Height = 500;
            FpSpread2.SaveChanges();
            FpSpread2.Sheets[0].ColumnHeader.Cells[0, 0].Text = "S.No";
            FpSpread2.Sheets[0].ColumnHeader.Cells[0, 0].Font.Bold = true;
            FpSpread2.Sheets[0].ColumnHeader.Cells[0, 0].HorizontalAlign = HorizontalAlign.Center;
            FpSpread2.Sheets[0].ColumnHeader.Cells[0, 0].Font.Name = "Book Antiqua";
            FpSpread2.Sheets[0].ColumnHeader.Cells[0, 0].Font.Size = FontUnit.Medium;
            FpSpread2.Sheets[0].Columns[0].HorizontalAlign = HorizontalAlign.Center;
            FpSpread2.Sheets[0].ColumnHeader.Cells[0, 1].Text = "EvaluationName";
            FpSpread2.Sheets[0].SetColumnMerge(1, FarPoint.Web.Spread.Model.MergePolicy.Always);
            FpSpread2.Sheets[0].ColumnHeader.Cells[0, 1].Font.Bold = true;
            FpSpread2.Sheets[0].ColumnHeader.Cells[0, 1].Font.Name = "Book Antiqua";
            FpSpread2.Sheets[0].ColumnHeader.Cells[0, 1].Font.Size = FontUnit.Medium;
            FpSpread2.Sheets[0].ColumnHeader.Cells[0, 1].HorizontalAlign = HorizontalAlign.Center;
            FpSpread2.Sheets[0].ColumnHeader.Cells[0, 2].Text = "Description ";
            FpSpread2.Sheets[0].ColumnHeader.Cells[0, 2].Font.Bold = true;
            FpSpread2.Sheets[0].ColumnHeader.Cells[0, 2].Font.Name = "Book Antiqua";
            FpSpread2.Sheets[0].ColumnHeader.Cells[0, 2].Font.Size = FontUnit.Medium;
            FpSpread2.Sheets[0].ColumnHeader.Cells[0, 2].HorizontalAlign = HorizontalAlign.Center;
            FpSpread2.Sheets[0].ColumnHeader.Cells[0, 3].Text = "Points ";
            FpSpread2.Sheets[0].ColumnHeader.Cells[0, 3].Font.Bold = true;
            FpSpread2.Sheets[0].ColumnHeader.Cells[0, 3].Font.Name = "Book Antiqua";
            FpSpread2.Sheets[0].ColumnHeader.Cells[0, 3].Font.Size = FontUnit.Medium;
            FpSpread2.Sheets[0].ColumnHeader.Cells[0, 3].HorizontalAlign = HorizontalAlign.Center;
            FpSpread2.Sheets[0].ColumnHeader.Columns[0].Width = 59;
            FpSpread2.Sheets[0].ColumnHeader.Columns[1].Width = 80;
            FpSpread2.Sheets[0].Columns[2].HorizontalAlign = HorizontalAlign.Left;
            ds.Clear();
            string selqry = "";
            selqry = "SELECT FeedBackName,TextVal,Question,SUM(M.Point) as points FROM CO_StudFeedBack F,CO_FeedBackMaster B,CO_QuestionMaster Q,CO_MarkMaster M,TextValTable T WHERE F.FeedBackMasterFK = B.FeedBackMasterPK AND F.QuestionMasterFK =  Q.QuestionMasterPK AND F.MarkMasterPK = M.MarkMasterPK AND Q.HeaderCode = T.TextCode AND  b.InclueCommon='1' and FeedBackType = '" + type + "' and B.FeedBackName='" + ddl_Feedbackname.SelectedItem.Value + "'  and B.CollegeCode in ('" + college_cd + "') GROUP BY FeedBackName,TextVal,Question";
            ds = d2.select_method_wo_parameter(selqry, "Text");
            if (ds.Tables.Count > 0)
            {
                if (ds.Tables[0].Rows.Count > 0)
                {
                    int total = 0;
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        FpSpread2.Sheets[0].Rows.Count++;
                        //if (((i + 1) % 2) == 0)
                        //{
                        //    FpSpread2.Sheets[0].Rows[FpSpread2.Sheets[0].RowCount - 1].BackColor = System.Drawing.Color.LightSeaGreen;
                        //}
                        FpSpread2.Sheets[0].Cells[i, 0].Text = Convert.ToString(i + 1);
                        FpSpread2.Sheets[0].Cells[i, 1].Text = ds.Tables[0].Rows[i]["TextVal"].ToString();
                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 1].VerticalAlign = FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 1].VerticalAlign = VerticalAlign.Middle;
                        FpSpread2.Sheets[0].Cells[i, 2].Text = ds.Tables[0].Rows[i]["Question"].ToString();
                        FpSpread2.Sheets[0].Cells[i, 3].Text = ds.Tables[0].Rows[i]["points"].ToString();
                        FpSpread2.Sheets[0].Columns[3].HorizontalAlign = HorizontalAlign.Center;
                        total = total + Convert.ToInt32(ds.Tables[0].Rows[i]["points"]);
                    }
                    FpSpread2.Sheets[0].Rows.Count++;
                    FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].Rows.Count - 1, 3].Text = Convert.ToString(total);
                    FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].Rows.Count - 1, FpSpread2.Sheets[0].ColumnCount - 1].HorizontalAlign = HorizontalAlign.Center;
                    FpSpread2.Sheets[0].SpanModel.Add(FpSpread2.Sheets[0].RowCount - 1, 0, 1, 3);
                    FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].Rows.Count - 1, 0].Text = "Grand Total";
                    FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].Rows.Count - 1, 0].HorizontalAlign = HorizontalAlign.Right;
                    FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].Rows.Count - 1, 0].ForeColor = System.Drawing.Color.Blue;
                }
                else
                {
                    //div1.Visible = false;
                    imgdiv2.Visible = true;
                    lbl_alert1.Text = "No Records Found";
                    FpSpread2.Visible = false;
                    rptprint1.Visible = false;
                }
            }
            else
            {
                // div1.Visible = false;
                imgdiv2.Visible = true;
                lbl_alert1.Text = "No Records Found";
                FpSpread2.Visible = false;
                rptprint1.Visible = false;
            }
            FpSpread2.Sheets[0].PageSize = FpSpread2.Sheets[0].RowCount;
            FpSpread2.Sheets[0].Columns[2].Width = 550;
            FpSpread2.Sheets[0].Columns[2].HorizontalAlign = HorizontalAlign.Left;
            //FpSpread2.Width = 800;
            //FpSpread2.Height = 500;
        }
        catch (Exception ex)
        {
            d2.sendErrorMail(ex, collegecode1, "FeedbackReport");
        }
    }

    public void gendformat2ananames()
    {
        try
        {
            FpSpread2.Visible = false;
            chartfalse();
            if (rb_Acad.Checked == true)
            {
                fair();
                FpSpread1.Visible = true;
                rptprint1.Visible = true;
                string college_cd = "";
                if (Cbl_college.Items.Count > 0)
                {
                    for (int i = 0; i < Cbl_college.Items.Count; i++)
                    {
                        if (Cbl_college.Items[i].Selected == true)
                        {
                            if (college_cd == "")
                            {
                                college_cd = "" + Cbl_college.Items[i].Value.ToString() + "";
                            }
                            else
                            {
                                college_cd = college_cd + "','" + Convert.ToString(Cbl_college.Items[i].Value);
                            }
                        }
                    }
                }
                string Batch_Year = "";
                for (int i = 0; i < cbl_batch.Items.Count; i++)
                {
                    if (cbl_batch.Items[i].Selected == true)
                    {
                        if (Batch_Year == "")
                        {
                            Batch_Year = "" + cbl_batch.Items[i].Value.ToString() + "";
                        }
                        else
                        {
                            Batch_Year = Batch_Year + "','" + cbl_batch.Items[i].Value.ToString() + "";
                        }
                    }
                }
                string degree_code = "";
                for (int i = 0; i < cbl_branch.Items.Count; i++)
                {
                    if (cbl_branch.Items[i].Selected == true)
                    {
                        if (degree_code == "")
                        {
                            degree_code = "" + cbl_branch.Items[i].Value.ToString() + "";
                        }
                        else
                        {
                            degree_code = degree_code + "','" + cbl_branch.Items[i].Value.ToString() + "";
                        }
                    }
                }
                string semester = "";
                for (int i = 0; i < cbl_sem.Items.Count; i++)
                {
                    if (cbl_sem.Items[i].Selected == true)
                    {
                        if (semester == "")
                        {
                            semester = "" + cbl_sem.Items[i].Value.ToString() + "";
                        }
                        else
                        {
                            semester = semester + "','" + cbl_sem.Items[i].Value.ToString() + "";
                        }
                    }
                }
                string section = "";
                for (int i = 0; i < cbl_sec.Items.Count; i++)
                {
                    if (cbl_sec.Items[i].Selected == true)
                    {
                        if (section == "")
                        {
                            section = "" + cbl_sec.Items[i].Value.ToString() + "";
                        }
                        else
                        {
                            section = section + "','" + cbl_sec.Items[i].Value.ToString() + "";
                        }
                        if (cbl_sec.Items[i].Value == "Empty")
                        {
                            section = "";
                        }
                    }
                }
                string sub = "";
                for (int i = 0; i < Cbl_Subject.Items.Count; i++)
                {
                    if (Cbl_Subject.Items[i].Selected == true)
                    {
                        if (sub == "")
                        {
                            sub = "" + Cbl_Subject.Items[i].Value.ToString() + "";
                        }
                        else
                        {
                            sub = sub + "','" + Cbl_Subject.Items[i].Value.ToString() + "";
                        }
                    }
                }
                if (section.Trim() != "")
                {
                    section = section + "','";
                }
                dsfb.Clear();
                if (Batch_Year.Trim() != "" && degree_code.Trim() != "" && semester.Trim() != "" && college_cd.Trim() != "")
                {
                    string fbpk = " select FeedBackMasterPK from CO_FeedBackMaster where FeedBackName ='" + ddl_Feedbackname.SelectedItem.Value + "' and DegreeCode in ('" + degree_code + "') and semester in ('" + semester + "') and Batch_Year in('" + Batch_Year + "') and section in ('" + section + "')";
                    dsfb = d2.select_method_wo_parameter(fbpk, "Text");
                    string feedbakpk = "";
                    if (dsfb.Tables.Count > 0)
                    {
                        if (dsfb.Tables[0].Rows.Count > 0)
                        {
                            for (int pk = 0; pk < dsfb.Tables[0].Rows.Count; pk++)
                            {
                                if (feedbakpk == "")
                                {
                                    feedbakpk = dsfb.Tables[0].Rows[pk]["FeedBackMasterPK"].ToString();
                                }
                                else
                                {
                                    //feedbakpk = dsfb.Tables[0].Rows[pk]["FeedBackMasterPK"].ToString();
                                    feedbakpk = feedbakpk + "','" + dsfb.Tables[0].Rows[pk]["FeedBackMasterPK"].ToString() + "";
                                }
                            }
                        }
                    }
                    string type = "";
                    if (rb_Acad.Checked == true)
                    {
                        type = "1";
                    }
                    else if (rb_Gend.Checked == true)
                    {
                        type = "2";
                    }
                    FpSpread1.Sheets[0].RowCount = 0;
                    FpSpread1.Sheets[0].ColumnCount = 0;
                    FpSpread1.CommandBar.Visible = false;
                    FpSpread1.Sheets[0].AutoPostBack = true;
                    FpSpread1.Sheets[0].ColumnHeader.RowCount = 1;
                    FpSpread1.Sheets[0].RowHeader.Visible = false;
                    FpSpread1.Sheets[0].ColumnCount = 10;
                    FarPoint.Web.Spread.StyleInfo darkstyle = new FarPoint.Web.Spread.StyleInfo();
                    darkstyle.BackColor = ColorTranslator.FromHtml("#0CA6CA");
                    darkstyle.ForeColor = System.Drawing.Color.White;
                    FpSpread1.ActiveSheetView.ColumnHeader.DefaultStyle = darkstyle;
                    FpSpread1.Visible = true;
                    FpSpread1.Width = 980;
                    FpSpread1.Height = 500;
                    FpSpread1.SaveChanges();
                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 0].Text = "S.No";
                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 0].Font.Bold = true;
                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 0].HorizontalAlign = HorizontalAlign.Center;
                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 0].Font.Name = "Book Antiqua";
                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 0].Font.Size = FontUnit.Medium;
                    FpSpread1.Sheets[0].Columns[0].HorizontalAlign = HorizontalAlign.Center;
                    FpSpread1.Sheets[0].SetColumnMerge(0, FarPoint.Web.Spread.Model.MergePolicy.Always);
                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 1].Text = "Staff Name";
                    FpSpread1.Sheets[0].SetColumnMerge(1, FarPoint.Web.Spread.Model.MergePolicy.Always);
                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 1].Font.Bold = true;
                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 1].Font.Name = "Book Antiqua";
                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 1].Font.Size = FontUnit.Medium;
                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 1].HorizontalAlign = HorizontalAlign.Center;
                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 2].Text = "S.NO";
                    //FpSpread1.Sheets[0].SetColumnMerge(2, FarPoint.Web.Spread.Model.MergePolicy.Always);
                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 2].Font.Bold = true;
                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 2].Font.Name = "Book Antiqua";
                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 2].Font.Size = FontUnit.Medium;
                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 2].HorizontalAlign = HorizontalAlign.Center;
                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 3].Text = "Batch ";
                    //FpSpread1.Sheets[0].SetColumnMerge(3, FarPoint.Web.Spread.Model.MergePolicy.Always);
                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 3].Font.Bold = true;
                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 3].Font.Name = "Book Antiqua";
                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 3].Font.Size = FontUnit.Medium;
                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 3].HorizontalAlign = HorizontalAlign.Center;
                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 4].Text = "Department";
                    FpSpread1.Sheets[0].SetColumnMerge(4, FarPoint.Web.Spread.Model.MergePolicy.Always);
                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 4].Font.Bold = true;
                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 4].Font.Name = "Book Antiqua";
                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 4].Font.Size = FontUnit.Medium;
                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 4].HorizontalAlign = HorizontalAlign.Center;
                    FpSpread1.Sheets[0].Columns[4].HorizontalAlign = HorizontalAlign.Center;
                    //FpSpread1.Columns[3].Visible = false;
                    FpSpread1.Columns[4].Visible = false;
                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 5].Text = "Subject Code";
                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 5].Font.Bold = true;
                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 5].Font.Name = "Book Antiqua";
                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 5].Font.Size = FontUnit.Medium;
                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 5].HorizontalAlign = HorizontalAlign.Center;
                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 6].Text = "Subject Name";
                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 6].Font.Bold = true;
                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 6].Font.Name = "Book Antiqua";
                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 6].Font.Size = FontUnit.Medium;
                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 6].HorizontalAlign = HorizontalAlign.Center;
                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 7].Text = "Total Points";
                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 7].Text = ddl_Feedbackname.SelectedItem.Text;
                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 7].Font.Bold = true;
                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 7].Font.Name = "Book Antiqua";
                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 7].Font.Size = FontUnit.Medium;
                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 7].HorizontalAlign = HorizontalAlign.Center;
                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 8].Text = "Average";
                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 8].Font.Bold = true;
                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 8].Font.Name = "Book Antiqua";
                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 8].Font.Size = FontUnit.Medium;
                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 8].HorizontalAlign = HorizontalAlign.Center;
                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 9].Text = "Select";
                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 9].Font.Bold = true;
                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 9].Font.Name = "Book Antiqua";
                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 9].Font.Size = FontUnit.Medium;
                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 9].HorizontalAlign = HorizontalAlign.Center;
                    FpSpread1.Sheets[0].ColumnHeader.Columns[0].Width = 39;
                    FpSpread1.Sheets[0].ColumnHeader.Columns[1].Width = 180;
                    FpSpread1.Sheets[0].ColumnHeader.Columns[2].Width = 50;
                    FpSpread1.Sheets[0].ColumnHeader.Columns[3].Width = 200;
                    FpSpread1.Sheets[0].ColumnHeader.Columns[4].Width = 100;
                    FpSpread1.Sheets[0].ColumnHeader.Columns[5].Width = 70;
                    FpSpread1.Sheets[0].ColumnHeader.Columns[6].Width = 200;
                    FpSpread1.Sheets[0].ColumnHeader.Columns[7].Width = 170;
                    FpSpread1.Sheets[0].ColumnHeader.Columns[8].Width = 59;
                    FpSpread1.Sheets[0].ColumnHeader.Columns[9].Width = 50;
                    ds.Clear();
                    string selqry = "";
                    //          selqry = "SELECT Staff_Name,y.Semester,Subject_Code,Subject_Name,y.Batch_Year,SUM(Point)as Points,COUNT(distinct f.app_no)as Strength, dt.Dept_Name,c.Course_Name FROM CO_StudFeedBack F,CO_MarkMaster M,Subject S,staff_appl_master A,staffmaster T,syllabus_master y,Degree d, Department dt,Course c, CO_FeedBackMaster FM,Registration r WHERE F.MarkMasterPK = M.MarkMasterPK AND F.StaffApplNo = A.appl_id AND F.SubjectNo = S.subject_no AND A.appl_no = T.appl_no  and s.syll_code = y.syll_code and d.Degree_Code =y.degree_code and dt.Dept_Code =d.Dept_Code and c.Course_Id =d.Course_Id  and f.App_No = r.App_No and FM.CollegeCode in ('" + college_cd + "') and y.Batch_Year in ('" + Batch_Year + "') and y.degree_code in ('" + degree_code + "') and y.semester in ('" + semester + "')  and fm.FeedBackType =('" + type + "') and fm.FeedBackMasterPK in ('" + feedbakpk + "') ";
                    //if (section != "")
                    //          {
                    //              selqry = selqry + " and r.Sections in ('" + section + "') and f.FeedBackMasterFK = fm.FeedBackMasterPK group by a.appl_id,Staff_Name, y.Semester, Subject_Code,y.Batch_Year, Subject_Name , dt.Dept_Name ,c.Course_Name";
                    //          }
                    //          else
                    //          {
                    //              selqry = selqry + " and f.FeedBackMasterFK = fm.FeedBackMasterPK group by a.appl_id, Staff_Name, y.Semester, Subject_Code,y.Batch_Year, Subject_Name ,dt.Dept_Name ,c.Course_Name";
                    //          }
                    selqry = " select SUM(M.Point)Points,sm.staff_name ,sbj.subject_name ,count( distinct S.FeedbackUnicode)Strength,sbj.Subject_Code,dt.Dept_Name,f.Batch_Year,f.semester,f.semester,C.Course_Name,((CONVERT(varchar(max), f.Batch_Year)+' - '+C.Course_Name+' - '+dt.dept_acronym+' - '+convert(varchar(20), f.semester)))as Batch,SubjectNo,staff_code,d.Degree_Code from CO_FeedbackUniCode FU,CO_FeedBackMaster F,CO_StudFeedBack S,CO_MarkMaster M,Degree D,Department dt,Course C,staffmaster sm,staff_appl_master sa,Subject sbj  where sbj.Subject_No =s.SubjectNo and sm.appl_no =sa.appl_no and sa.appl_id =s.StaffApplNo and  d.Degree_Code =f.DegreeCode and dt.Dept_Code=d.Dept_Code and c.Course_Id =d.Course_Id and  FU.FeedbackMasterFK=F.FeedBackMasterPK and s.FeedBackMasterFK =f.FeedBackMasterPK and s.FeedBackMasterFK =fu.FeedbackMasterFK and fu.FeedbackUnicode =s.FeedbackUnicode and M.MarkMasterPK =S.MarkMasterPK and Fu.FeedbackMasterFK in ('" + feedbakpk + "') and sbj.Subject_Code in('" + sub + "') and f.InclueCommon ='1' group by sm.staff_name ,sbj.subject_name ,dt.Dept_Name , f.Batch_Year,f.semester,f.semester,C.Course_Name,sbj.Subject_Code,dt.dept_acronym,SubjectNo,staff_code,d.Degree_Code order by sm.staff_name ";
                    selqry = selqry + " select COUNT( distinct QuestionMasterFK)question_count from CO_FeedBackQuestions where FeedBackMasterFK in ('" + feedbakpk + "')";
                    selqry = selqry + " select top 1 Point from CO_MarkMaster  where CollegeCode in('" + collegecode1 + "') order by Point desc";
                    selqry = selqry + " select COUNT(FeedbackUnicode),m.Batch_Year,m.DegreeCode,m.semester,m.Section,FeedBackMasterPK,cr.Course_Id  from CO_FeedbackUniCode c,CO_FeedBackMaster M,Degree d,Department dt,Course cr where cr.Course_Id =d.Course_Id and d.Dept_Code =dt.Dept_Code and d.Degree_Code =m.DegreeCode and c.FeedbackMasterFK =m.FeedBackMasterPK and FeedBackMasterPK in('" + feedbakpk + "')  group by m.DegreeCode,m.Batch_Year,m.semester,m.Section,FeedBackMasterPK,(cr.Course_Name +' - '+dt.Dept_Name) ,cr.Course_Id order by cr.Course_Id asc ";//select COUNT(FeedbackUnicode)as totaluniquecode  from CO_FeedbackUniCode where FeedbackMasterFK in('" + feedbakpk + "')";
                    ds = d2.select_method_wo_parameter(selqry, "Text");
                    //string question_count = d2.GetFunction("select COUNT( distinct QuestionMasterFK)question_count from CO_FeedBackQuestions where FeedBackMasterFK in ('" + feedbakpk + "')");
                    string needs = "5";
                    // string sum_total = d2.GetFunction("select top 1 Point from CO_MarkMaster  where CollegeCode in('" + collegecode1 + "') order by Point desc");
                    string question_count = "";
                    if (ds.Tables[1].Rows.Count > 0)
                    {
                        question_count = Convert.ToString(ds.Tables[1].Rows[0][0]);
                    }
                    if (question_count.Trim() == "")
                        question_count = "0";
                    string sum_total = "";
                    if (ds.Tables[2].Rows.Count > 0)
                    {
                        sum_total = Convert.ToString(ds.Tables[2].Rows[0][0]);
                    }
                    if (sum_total.Trim() == "")
                        sum_total = "0";
                    if (ds.Tables.Count > 0)
                    {
                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            Double sum_tot = 0; double sumfbtot = 0;
                            Double sum_avgs = 0; int k = 0; string staffname = ""; int s = 1;
                            FarPoint.Web.Spread.CheckBoxCellType cb = new FarPoint.Web.Spread.CheckBoxCellType();
                            cb.AutoPostBack = true;
                            FarPoint.Web.Spread.CheckBoxCellType cb1 = new FarPoint.Web.Spread.CheckBoxCellType();
                            cb1.AutoPostBack = false;
                            FpSpread1.Sheets[0].RowCount++;
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 9].HorizontalAlign = HorizontalAlign.Center;
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 9].CellType = cb;
                            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                            {
                                FpSpread1.Sheets[0].RowCount++;
                                //if (((i + 1) % 2) == 0)
                                //{
                                //    FpSpread1.Sheets[0].Rows[FpSpread1.Sheets[0].RowCount - 1].BackColor = System.Drawing.Color.LightSeaGreen;
                                //}
                                if (staffname.Trim() == "")
                                { k++; }
                                else if (staffname == ds.Tables[0].Rows[i]["staff_name"].ToString())
                                { k++; }
                                else { k = 1; s++; }
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Text = Convert.ToString(s);
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].VerticalAlign = FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].VerticalAlign = VerticalAlign.Middle;
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].Text = ds.Tables[0].Rows[i]["staff_name"].ToString();
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].Tag = ds.Tables[0].Rows[i]["staff_code"].ToString();
                                staffname = ds.Tables[0].Rows[i]["staff_name"].ToString();
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].VerticalAlign = FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].VerticalAlign = VerticalAlign.Middle;
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].Text = k.ToString();
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].VerticalAlign = FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].VerticalAlign = VerticalAlign.Middle;
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 3].Text = ds.Tables[0].Rows[i]["Batch"].ToString();
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 3].Tag = ds.Tables[0].Rows[i]["Batch_Year"].ToString();
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 3].VerticalAlign = FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 3].VerticalAlign = VerticalAlign.Middle;
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 4].Text = ds.Tables[0].Rows[i]["Dept_Name"].ToString();
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 4].Tag = ds.Tables[0].Rows[i]["Course_Name"].ToString();
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 4].Note = ds.Tables[0].Rows[i]["Degree_Code"].ToString();
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 4].VerticalAlign = FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 4].VerticalAlign = VerticalAlign.Middle;
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 5].Text = ds.Tables[0].Rows[i]["Subject_Code"].ToString();
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 5].Tag = ds.Tables[0].Rows[i]["semester"].ToString();
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 6].Text = ds.Tables[0].Rows[i]["subject_name"].ToString();
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 6].Tag = ds.Tables[0].Rows[i]["SubjectNo"].ToString();
                                Double point = Convert.ToDouble(ds.Tables[0].Rows[i]["Points"]);
                                //17.08.16
                                string totaluniquecode = "";
                                if (ds.Tables[3].Rows.Count > 0)
                                {
                                    DataView dv2 = new DataView();
                                    ds.Tables[3].DefaultView.RowFilter = " Batch_Year='" + ds.Tables[0].Rows[i]["Batch_Year"].ToString() + "' and semester='" + ds.Tables[0].Rows[i]["semester"].ToString() + "' and DegreeCode='" + ds.Tables[0].Rows[i]["Degree_Code"].ToString() + "'";
                                    dv2 = ds.Tables[3].DefaultView;
                                    if (dv2.Count > 0)
                                        totaluniquecode = Convert.ToString(ds.Tables[3].Rows[0][0]);
                                }
                                if (totaluniquecode.Trim() == "")
                                    totaluniquecode = "0";
                                double calfbcal = Convert.ToDouble(totaluniquecode) * Convert.ToDouble(question_count) * Convert.ToDouble(sum_total);
                                double fbavg = (point / calfbcal) * 100;
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 7].Text = Convert.ToString(Math.Round(fbavg, 0));
                                // FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 7].Text = ds.Tables[0].Rows[i]["Points"].ToString();
                                // Double point = Convert.ToDouble(ds.Tables[0].Rows[i]["Points"]);
                                Double strength = Convert.ToDouble(ds.Tables[0].Rows[i]["Strength"]);
                                strength = strength * Convert.ToDouble(sum_total) * Convert.ToDouble(question_count);
                                Double avg = 0;
                                avg = point / strength;
                                avg = avg * 100;
                                avg = (Math.Round(avg, 1));
                                //  avg = avg / Convert.ToDouble(question_count);
                                //string points = Convert.ToString(avg);
                                //ConvertedMark(needs, sum_total, ref points);
                                sum_tot = sum_tot + Convert.ToDouble(ds.Tables[0].Rows[i]["Points"]);
                                sum_avgs = sum_avgs + Convert.ToDouble(avg);
                                sumfbtot = sumfbtot + Convert.ToDouble(fbavg);
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 8].Text = Convert.ToString(avg);
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].Rows.Count - 1, 9].CellType = cb1;
                            }
                            FpSpread1.Sheets[0].RowCount++;
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 7].Text = Convert.ToString(Math.Round(sumfbtot, 2));
                            //17.08.16
                            //FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 7].Text = Convert.ToString(sum_tot);
                            sum_avgs = sum_avgs / Convert.ToDouble(FpSpread1.Sheets[0].RowCount - 2);
                            string sum_avg = Convert.ToString(Math.Round(sum_avgs, 2));
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 8].Text = Convert.ToString(sum_avg);
                            FpSpread1.Sheets[0].SpanModel.Add(FpSpread1.Sheets[0].RowCount - 1, 0, 1, 7);
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].Rows.Count - 1, 0].Text = "Grand Total";
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].Rows.Count - 1, 0].HorizontalAlign = HorizontalAlign.Right;
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].Rows.Count - 1, 0].ForeColor = System.Drawing.Color.Blue;
                            FpSpread1.Rows[FpSpread1.Sheets[0].Rows.Count - 1].Visible = false;
                            FpSpread1.Sheets[0].Columns[2].HorizontalAlign = HorizontalAlign.Center;
                            FpSpread1.Sheets[0].Columns[5].HorizontalAlign = HorizontalAlign.Center;
                            FpSpread1.Sheets[0].Columns[7].HorizontalAlign = HorizontalAlign.Center;
                            FpSpread1.Sheets[0].Columns[3].HorizontalAlign = HorizontalAlign.Center;
                            FpSpread1.Sheets[0].Columns[8].HorizontalAlign = HorizontalAlign.Center;
                            FpSpread1.Sheets[0].Columns[9].HorizontalAlign = HorizontalAlign.Center;
                            FpSpread1.Sheets[0].PageSize = FpSpread1.Sheets[0].RowCount;
                            if (cb_avgcolumn.Checked == true)
                            {
                                FpSpread1.Columns[8].Visible = true;
                            }
                            else
                            {
                                FpSpread1.Columns[8].Visible = false;
                            }
                            FpSpread1.Columns[9].Visible = false;
                        }
                        else
                        {
                            imgdiv2.Visible = true;
                            lbl_alert1.Text = "No Records Found";
                            FpSpread1.Visible = false;
                            rptprint1.Visible = false;
                        }
                    }
                    else
                    {
                        imgdiv2.Visible = true;
                        lbl_alert1.Text = "No Records Found";
                        FpSpread1.Visible = false;
                        rptprint1.Visible = false;
                    }
                }
                else
                {
                    imgdiv2.Visible = true;
                    lbl_alert1.Text = "Please Select All Fields";
                    FpSpread1.Visible = false;
                    rptprint1.Visible = false;
                }
            }
        }
        catch (Exception ex)
        {
            d2.sendErrorMail(ex, collegecode1, "FeedbackReport");
        }
    }

    public void gendformat3ananames()
    {
        try
        {
            FpSpread2.Visible = false;
            chartfalse();
            cb_total1.Visible = true;
            cb_avg1.Visible = true;
            fair();
            FpSpread2.Visible = true;
            rptprint1.Visible = true;
            string Batch_Year = "";
            for (int i = 0; i < cbl_batch.Items.Count; i++)
            {
                if (cbl_batch.Items[i].Selected == true)
                {
                    if (Batch_Year == "")
                    {
                        Batch_Year = "" + cbl_batch.Items[i].Value.ToString() + "";
                    }
                    else
                    {
                        Batch_Year = Batch_Year + "','" + cbl_batch.Items[i].Value.ToString() + "";
                    }
                }
            }
            string college_cd = "";
            if (Cbl_college.Items.Count > 0)
            {
                for (int i = 0; i < Cbl_college.Items.Count; i++)
                {
                    if (Cbl_college.Items[i].Selected == true)
                    {
                        if (college_cd == "")
                        {
                            college_cd = "" + Cbl_college.Items[i].Value.ToString() + "";
                        }
                        else
                        {
                            college_cd = college_cd + "','" + Convert.ToString(Cbl_college.Items[i].Value);
                        }
                    }
                }
            }
            string degree_code = "";
            for (int i = 0; i < cbl_branch.Items.Count; i++)
            {
                if (cbl_branch.Items[i].Selected == true)
                {
                    if (degree_code == "")
                    {
                        degree_code = "" + cbl_branch.Items[i].Value.ToString() + "";
                    }
                    else
                    {
                        degree_code = degree_code + "','" + cbl_branch.Items[i].Value.ToString() + "";
                    }
                }
            }
            string semester = "";
            for (int i = 0; i < cbl_sem.Items.Count; i++)
            {
                if (cbl_sem.Items[i].Selected == true)
                {
                    if (semester == "")
                    {
                        semester = "" + cbl_sem.Items[i].Value.ToString() + "";
                    }
                    else
                    {
                        semester = semester + "','" + cbl_sem.Items[i].Value.ToString() + "";
                    }
                }
            }
            string section = "";
            for (int i = 0; i < cbl_sec.Items.Count; i++)
            {
                if (cbl_sec.Items[i].Selected == true)
                {
                    if (section == "")
                    {
                        section = "" + cbl_sec.Items[i].Value.ToString() + "";
                    }
                    else
                    {
                        section = section + "','" + cbl_sec.Items[i].Value.ToString() + "";
                    }
                    if (cbl_sec.Items[i].Value == "Empty")
                    {
                        section = "";
                    }
                }
            }
            if (section.Trim() != "")
            {
                section = section + "','";
            }
            dsfb.Clear();
            string fbpk = " select FeedBackMasterPK from CO_FeedBackMaster where FeedBackName ='" + ddl_Feedbackname.SelectedItem.Value + "' and DegreeCode in ('" + degree_code + "') and semester in ('" + semester + "') and Batch_Year in('" + Batch_Year + "') and section in ('" + section + "')";
            dsfb = d2.select_method_wo_parameter(fbpk, "Text");
            string feedbakpk = "";
            if (dsfb.Tables.Count > 0)
            {
                if (dsfb.Tables[0].Rows.Count > 0)
                {
                    for (int pk = 0; pk < dsfb.Tables[0].Rows.Count; pk++)
                    {
                        if (feedbakpk == "")
                        {
                            feedbakpk = dsfb.Tables[0].Rows[pk]["FeedBackMasterPK"].ToString();
                        }
                        else
                        {
                            feedbakpk = feedbakpk + "','" + dsfb.Tables[0].Rows[pk]["FeedBackMasterPK"].ToString() + "";
                        }
                    }
                }
            }
            string type = "";
            if (rb_Acad.Checked == true)
            {
                type = "1";
            }
            else if (rb_Gend.Checked == true)
            {
                type = "2";
            }
            FpSpread2.Sheets[0].RowCount = 0;
            FpSpread2.Sheets[0].ColumnCount = 0;
            FpSpread2.CommandBar.Visible = false;
            FpSpread2.Sheets[0].AutoPostBack = true;
            FpSpread2.Sheets[0].ColumnHeader.RowCount = 2;
            FpSpread2.Sheets[0].RowHeader.Visible = false;
            FpSpread2.Sheets[0].ColumnCount = 3;
            FarPoint.Web.Spread.StyleInfo darkstyle = new FarPoint.Web.Spread.StyleInfo();
            darkstyle.BackColor = ColorTranslator.FromHtml("#0CA6CA");
            darkstyle.ForeColor = System.Drawing.Color.White;
            FpSpread2.ActiveSheetView.ColumnHeader.DefaultStyle = darkstyle;
            FpSpread2.Visible = true;
            //FpSpread2.Width = 971;
            //FpSpread2.Height = 500;
            FpSpread2.SaveChanges();
            FpSpread2.Sheets[0].ColumnHeader.Cells[0, 0].Text = "S.No";
            FpSpread2.Sheets[0].ColumnHeader.Cells[0, 0].HorizontalAlign = HorizontalAlign.Center;
            FpSpread2.Sheets[0].ColumnHeader.Cells[0, 0].Font.Bold = true;
            FpSpread2.Sheets[0].ColumnHeader.Cells[0, 0].Font.Name = "Book Antiqua";
            FpSpread2.Sheets[0].ColumnHeader.Cells[0, 0].Font.Size = FontUnit.Medium;
            FpSpread2.Sheets[0].Columns[0].HorizontalAlign = HorizontalAlign.Center;
            FpSpread2.Sheets[0].ColumnHeader.Cells[0, 1].Text = "EvaluationName";
            FpSpread2.Sheets[0].ColumnHeader.Cells[0, 1].Font.Bold = true;
            FpSpread2.Sheets[0].ColumnHeader.Cells[0, 1].Font.Name = "Book Antiqua";
            FpSpread2.Sheets[0].ColumnHeader.Cells[0, 1].Font.Size = FontUnit.Medium;
            FpSpread2.Sheets[0].ColumnHeader.Cells[0, 1].HorizontalAlign = HorizontalAlign.Center;
            FpSpread2.Sheets[0].ColumnHeader.Cells[0, 2].Text = "Description ";
            FpSpread2.Sheets[0].ColumnHeader.Cells[0, 2].Font.Bold = true;
            FpSpread2.Sheets[0].ColumnHeader.Cells[0, 2].Font.Name = "Book Antiqua";
            FpSpread2.Sheets[0].ColumnHeader.Cells[0, 2].Font.Size = FontUnit.Medium;
            FpSpread2.Sheets[0].ColumnHeader.Cells[0, 2].HorizontalAlign = HorizontalAlign.Center;
            FpSpread2.Sheets[0].ColumnHeader.Columns[0].Width = 59;
            FpSpread2.Sheets[0].ColumnHeader.Columns[1].Width = 80;
            FpSpread2.Sheets[0].Columns[2].HorizontalAlign = HorizontalAlign.Left;
            FpSpread2.Sheets[0].ColumnHeaderSpanModel.Add(0, 0, 2, 1);
            FpSpread2.Sheets[0].ColumnHeaderSpanModel.Add(0, 1, 2, 1);
            FpSpread2.Sheets[0].ColumnHeaderSpanModel.Add(0, 2, 2, 1);
            ds.Clear();
            string subject_cd = "";
            string selectquestion = "";
            string selqry = "";
            selqry = "select DISTINCT(subject_code),cs.subject_no,subject_name,subject_code+'-'+subject_name as codeandname  from subjectChooser cs,CO_FeedbackUniCode FU,CO_FeedBackMaster F,CO_StudFeedBack S,CO_MarkMaster M,Degree D,Department dt,Course C,staffmaster sm,staff_appl_master sa,Subject sbj  where S.SubjectNo =cs.subject_no and cs.subject_no = sbj.subject_no  and  sbj.Subject_No =s.SubjectNo and sm.appl_no =sa.appl_no and sa.appl_id =s.StaffApplNo and  d.Degree_Code =f.DegreeCode and dt.Dept_Code=d.Dept_Code and c.Course_Id =d.Course_Id and  FU.FeedbackMasterFK=F.FeedBackMasterPK and s.FeedBackMasterFK =f.FeedBackMasterPK and s.FeedBackMasterFK =fu.FeedbackMasterFK and fu.FeedbackUnicode =s.FeedbackUnicode and M.MarkMasterPK =S.MarkMasterPK and Fu.FeedbackMasterFK in ('" + feedbakpk + "') and f.InclueCommon ='1' group by cs.subject_no,subject_name,subject_code";
            ds = d2.select_method_wo_parameter(selqry, "Text");
            if (ds.Tables.Count > 0)
            {
                if (ds.Tables[0].Rows.Count > 0)
                {
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        subject_cd = ds.Tables[0].Rows[i]["subject_no"].ToString();
                        subject_cd = subject_cd + "','" + ds.Tables[0].Rows[i]["subject_no"].ToString();
                    }
                    if (cb_total1.Checked == true && cb_avg1.Checked)
                    {
                        for (int row = 0; row < ds.Tables[0].Rows.Count; row++)
                        {
                            FpSpread2.Sheets[0].ColumnCount++;
                            FpSpread2.Sheets[0].ColumnHeader.Cells[0, FpSpread2.Sheets[0].ColumnCount - 1].Text = Convert.ToString(ds.Tables[0].Rows[row]["codeandname"]);
                            FpSpread2.Sheets[0].ColumnHeader.Cells[0, FpSpread2.Sheets[0].ColumnCount - 1].Font.Bold = true;
                            FpSpread2.Sheets[0].ColumnHeader.Cells[0, FpSpread2.Sheets[0].ColumnCount - 1].Font.Name = "Book Antiqua";
                            FpSpread2.Sheets[0].ColumnHeader.Cells[0, FpSpread2.Sheets[0].ColumnCount - 1].Font.Size = FontUnit.Medium;
                            FpSpread2.Sheets[0].ColumnHeader.Cells[1, FpSpread2.Sheets[0].ColumnCount - 1].Text = "Total";
                            FpSpread2.Sheets[0].ColumnHeader.Cells[1, FpSpread2.Sheets[0].ColumnCount - 1].Font.Bold = true;
                            FpSpread2.Sheets[0].ColumnHeader.Cells[1, FpSpread2.Sheets[0].ColumnCount - 1].Font.Name = "Book Antiqua";
                            FpSpread2.Sheets[0].ColumnHeader.Cells[1, FpSpread2.Sheets[0].ColumnCount - 1].HorizontalAlign = HorizontalAlign.Center;
                            FpSpread2.Sheets[0].ColumnHeader.Cells[0, FpSpread2.Sheets[0].ColumnCount - 1].Tag = Convert.ToString(ds.Tables[0].Rows[row]["subject_no"]);
                            FpSpread2.Sheets[0].ColumnCount++;
                            FpSpread2.Sheets[0].ColumnHeader.Cells[1, FpSpread2.Sheets[0].ColumnCount - 1].Text = "Avg";
                            FpSpread2.Sheets[0].ColumnHeader.Cells[1, FpSpread2.Sheets[0].ColumnCount - 1].HorizontalAlign = HorizontalAlign.Center;
                            FpSpread2.Sheets[0].ColumnHeaderSpanModel.Add(0, FpSpread2.Sheets[0].ColumnCount - 2, 1, 2);
                            FpSpread2.Sheets[0].ColumnHeader.Cells[1, FpSpread2.Sheets[0].ColumnCount - 1].Font.Bold = true;
                            FpSpread2.Sheets[0].ColumnHeader.Cells[1, FpSpread2.Sheets[0].ColumnCount - 1].Font.Name = "Book Antiqua";
                            FpSpread2.Sheets[0].ColumnHeader.Cells[1, FpSpread2.Sheets[0].ColumnCount - 1].Font.Size = FontUnit.Medium;
                            FpSpread2.Sheets[0].ColumnHeader.Cells[1, FpSpread2.Sheets[0].ColumnCount - 1].HorizontalAlign = HorizontalAlign.Center;
                        }
                    }
                    else if (cb_total1.Checked == true || cb_avg1.Checked == true)
                    {
                        for (int row = 0; row < ds.Tables[0].Rows.Count; row++)
                        {
                            if (cb_total1.Checked == true)
                            {
                                FpSpread2.Sheets[0].ColumnCount++;
                                FpSpread2.Sheets[0].ColumnHeader.Cells[0, FpSpread2.Sheets[0].ColumnCount - 1].Text = Convert.ToString(ds.Tables[0].Rows[row]["codeandname"]);
                                FpSpread2.Sheets[0].ColumnHeader.Cells[0, FpSpread2.Sheets[0].ColumnCount - 1].HorizontalAlign = HorizontalAlign.Center;
                                FpSpread2.Sheets[0].ColumnHeader.Cells[1, FpSpread2.Sheets[0].ColumnCount - 1].Text = "Total";
                                FpSpread2.Sheets[0].ColumnHeader.Cells[1, FpSpread2.Sheets[0].ColumnCount - 1].HorizontalAlign = HorizontalAlign.Center;
                                FpSpread2.Sheets[0].ColumnHeader.Cells[0, FpSpread2.Sheets[0].ColumnCount - 1].Tag = Convert.ToString(ds.Tables[0].Rows[row]["subject_no"]);
                            }
                            if (cb_avg1.Checked == true)
                            {
                                FpSpread2.Sheets[0].ColumnCount++;
                                FpSpread2.Sheets[0].ColumnHeader.Cells[0, FpSpread2.Sheets[0].ColumnCount - 1].Text = Convert.ToString(ds.Tables[0].Rows[row]["codeandname"]);
                                FpSpread2.Sheets[0].ColumnHeader.Cells[0, FpSpread2.Sheets[0].ColumnCount - 1].Tag = Convert.ToString(ds.Tables[0].Rows[row]["subject_no"]);
                                FpSpread2.Sheets[0].ColumnHeader.Cells[1, FpSpread2.Sheets[0].ColumnCount - 1].Text = "Avg";
                                FpSpread2.Sheets[0].ColumnHeader.Cells[1, FpSpread2.Sheets[0].ColumnCount - 1].HorizontalAlign = HorizontalAlign.Center;
                                FpSpread2.Sheets[0].ColumnHeader.Cells[0, FpSpread2.Sheets[0].ColumnCount - 1].Font.Bold = true;
                                FpSpread2.Sheets[0].ColumnHeader.Cells[0, FpSpread2.Sheets[0].ColumnCount - 1].Font.Name = "Book Antiqua";
                                FpSpread2.Sheets[0].ColumnHeader.Cells[0, FpSpread2.Sheets[0].ColumnCount - 1].Font.Size = FontUnit.Medium;
                                FpSpread2.Sheets[0].ColumnHeader.Cells[0, FpSpread2.Sheets[0].ColumnCount - 1].HorizontalAlign = HorizontalAlign.Center;
                            }
                        }
                    }
                    FpSpread2.Sheets[0].ColumnCount++;
                    FpSpread2.Sheets[0].ColumnHeader.Cells[0, FpSpread2.Sheets[0].ColumnCount - 1].Text = "GrandTotal";
                    FpSpread2.Sheets[0].ColumnHeaderSpanModel.Add(0, FpSpread2.Sheets[0].ColumnCount - 1, 2, 1);
                    FpSpread2.Sheets[0].ColumnHeader.Cells[0, FpSpread2.Sheets[0].ColumnCount - 1].Font.Bold = true;
                    FpSpread2.Sheets[0].ColumnHeader.Cells[0, FpSpread2.Sheets[0].ColumnCount - 1].Font.Name = "Book Antiqua";
                    FpSpread2.Sheets[0].ColumnHeader.Cells[0, FpSpread2.Sheets[0].ColumnCount - 1].Font.Size = FontUnit.Medium;
                    FpSpread2.Sheets[0].ColumnHeader.Cells[0, FpSpread2.Sheets[0].ColumnCount - 1].HorizontalAlign = HorizontalAlign.Center;
                    FpSpread2.Sheets[0].ColumnCount++;
                    FpSpread2.Sheets[0].ColumnHeader.Cells[0, FpSpread2.Sheets[0].ColumnCount - 1].Text = "GrandAvg";
                    FpSpread2.Sheets[0].ColumnHeaderSpanModel.Add(0, FpSpread2.Sheets[0].ColumnCount - 1, 2, 1);
                    FpSpread2.Sheets[0].ColumnHeader.Cells[0, FpSpread2.Sheets[0].ColumnCount - 1].Font.Bold = true;
                    FpSpread2.Sheets[0].ColumnHeader.Cells[0, FpSpread2.Sheets[0].ColumnCount - 1].Font.Name = "Book Antiqua";
                    FpSpread2.Sheets[0].ColumnHeader.Cells[0, FpSpread2.Sheets[0].ColumnCount - 1].Font.Size = FontUnit.Medium;
                    FpSpread2.Sheets[0].ColumnHeader.Cells[0, FpSpread2.Sheets[0].ColumnCount - 1].HorizontalAlign = HorizontalAlign.Center;
                    // selectquestion = "select distinct (select TextVal from TextValTable where TextCode = HeaderCode) as HeaderName,HeaderCode,Question,QuestionMasterPK from CO_QuestionMaster cq,CO_FeedBackMaster cf,CO_FeedBackQuestions cb,CO_StudFeedBack s where cq.QuestionMasterPK =cb.QuestionMasterFK and cf.FeedBackMasterPK =cb.FeedBackMasterFK and cf.CollegeCode in ('" + college_cd + "') and  s.FeedBackMasterFK =cf.FeedBackMasterPK and s.QuestionMasterFK =cq.QuestionMasterPK and  cf.FeedBackType =('" + type + "') and cf.DegreeCode in ('" + degree_code + "') and cf.Batch_Year in ('" + Batch_Year + "') and cf.FeedBackMasterPK in('" + feedbakpk + "') and cf.semester in ('" + semester + "') ";
                    selectquestion = " select distinct (select TextVal from TextValTable where TextCode = HeaderCode) as HeaderName,HeaderCode,Question,QuestionMasterPK from  CO_QuestionMaster cq,CO_FeedBackMaster cf,CO_FeedBackQuestions cb,CO_StudFeedBack s, CO_FeedbackUniCode f where f.FeedbackMasterFK=s.FeedBackMasterFK and cq.QuestionMasterPK =cb.QuestionMasterFK and cf.FeedBackMasterPK =cb.FeedBackMasterFK and s.FeedBackMasterFK =cf.FeedBackMasterPK and s.QuestionMasterFK =cq.QuestionMasterPK and f.FeedbackMasterFK=cf.FeedBackMasterPK and cb.FeedBackMasterFK=f.FeedbackMasterFK and cf.CollegeCode in ('" + college_cd + "') and ISNULL(s.app_no,0)=0   and cf.FeedBackType =('" + type + "') and cf.DegreeCode in ('" + degree_code + "') and cf.Batch_Year in ('" + Batch_Year + "') and cf.FeedBackMasterPK in('" + feedbakpk + "') and cf.semester in ('" + semester + "')   and f.IsCheckFlag='1' ";
                    if (section != "")
                    {
                        selectquestion = selectquestion + " and cf.Section in ('" + section + "')";
                    }
                    // selectquestion = selectquestion + " select SUM(point)as total,cs.QuestionMasterFK,SubjectNo,COUNT(distinct cs.app_no)as Strength from CO_StudFeedBack cs,CO_MarkMaster cm, CO_FeedBackMaster fm where cs.MarkMasterPK =cm.MarkMasterPK and fm.FeedBackMasterPK =cs.FeedBackMasterFK and fm.DegreeCode in ('" + degree_code + "') and fm.CollegeCode in ('" + college_cd + "') and fm.Batch_Year in ('" + Batch_Year + "') and fm.FeedBackName='" + ddl_Feedbackname.SelectedItem.Value + "' and fm.semester in ('" + semester + "') and  ISNULL(app_no,0) <>0 ";
                    // selectquestion = selectquestion + " select SUM(point)as total,cs.QuestionMasterFK,SubjectNo,COUNT(distinct cs.FeedbackUnicode)as Strength from CO_FeedbackUniCode u, CO_StudFeedBack cs,CO_MarkMaster cm, CO_FeedBackMaster fm where cs.MarkMasterPK =cm.MarkMasterPK and fm.FeedBackMasterPK =cs.FeedBackMasterFK and cs.FeedBackMasterFK=u.FeedbackMasterFK and u.FeedbackMasterFK=fm.FeedBackMasterPK  and fm.DegreeCode in ('" + degree_code + "') and fm.CollegeCode in ('" + college_cd + "') and fm.Batch_Year in ('" + Batch_Year + "') and fm.FeedBackName='" + ddl_Feedbackname.SelectedItem.Value + "' and fm.semester in ('" + semester + "') and  ISNULL(cs.app_no,0)=0  and u.IsCheckFlag='1' ";
                    selectquestion = selectquestion + " select SUM(point)as total,QuestionMasterFK,SubjectNo,COUNT(distinct FeedbackUnicode)as Strength  from CO_StudFeedBack st,CO_MarkMaster cm where ISNULL(app_no,0)=0 and ISNULL(FeedbackUnicode,'') <>''  and cm.MarkMasterPK=st.MarkMasterPK group by  QuestionMasterFK, SubjectNo";
                    //if (section != "")
                    //{
                    //    selectquestion = selectquestion + " and fm.Section in ('" + section + "') and fm.FeedBackType='" + type + "' group by  cs.QuestionMasterFK, SubjectNo";
                    //}
                    //else
                    //{
                    //    selectquestion = selectquestion + " and fm.FeedBackType='1' group by  cs.QuestionMasterFK,SubjectNo";
                    //}
                    DataView dv = new DataView();
                    ds = d2.select_method_wo_parameter(selectquestion, "Text");
                    if (ds.Tables.Count > 0)
                    {
                        if (ds.Tables[0].Rows.Count > 0 && ds.Tables[1].Rows.Count > 0)
                        {
                            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                            {
                                FpSpread2.Sheets[0].Rows.Count++;
                                if (((i + 1) % 2) == 0)
                                {
                                    FpSpread2.Sheets[0].Rows[FpSpread2.Sheets[0].RowCount - 1].BackColor = System.Drawing.Color.LightSeaGreen;
                                }
                                FpSpread2.Sheets[0].Cells[i, 0].Text = Convert.ToString(i + 1);
                                FpSpread2.Sheets[0].Cells[i, 1].Text = ds.Tables[0].Rows[i]["HeaderName"].ToString();
                                // FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 4].VerticalAlign = FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 4].VerticalAlign = VerticalAlign.Middle;
                                FpSpread2.Sheets[0].Cells[i, 1].Tag = ds.Tables[0].Rows[i]["HeaderCode"].ToString();
                                FpSpread2.Sheets[0].Cells[i, 2].Text = ds.Tables[0].Rows[i]["Question"].ToString();
                                FpSpread2.Sheets[0].Columns[2].HorizontalAlign = HorizontalAlign.Left;
                                FpSpread2.Sheets[0].Cells[i, 2].Tag = ds.Tables[0].Rows[i]["QuestionMasterPK"].ToString();
                                int total = 0;
                                int streng = 0;
                                string needs = "5";
                                string sum_total = d2.GetFunction("select top 1 Point from CO_MarkMaster  where CollegeCode in('" + collegecode1 + "') order by Point desc");
                                string points = "";
                                if (cb_total1.Checked == true && cb_avg1.Checked == true)
                                {
                                    for (int col = 3; col < FpSpread2.Sheets[0].ColumnCount - 1; col += 2)
                                    {
                                        string subjectno = Convert.ToString(FpSpread2.Sheets[0].ColumnHeader.Cells[0, col].Tag);
                                        if (subjectno.Trim() != "")
                                        {
                                            ds.Tables[1].DefaultView.RowFilter = "QuestionMasterFK='" + Convert.ToString(ds.Tables[0].Rows[i]["QuestionMasterPK"]) + "' and SubjectNo='" + subjectno + "'";
                                            dv = ds.Tables[1].DefaultView;
                                            if (dv.Count > 0)
                                            {
                                                FpSpread2.Sheets[0].Cells[i, col].Text = Convert.ToString(dv[0]["total"]);
                                                FpSpread2.Sheets[0].Cells[i, col].HorizontalAlign = HorizontalAlign.Center;
                                                Int32 tot = Convert.ToInt32(dv[0]["total"]);
                                                int strengt = Convert.ToInt32(dv[0]["Strength"]);
                                                Double bas_avg = tot / strengt;
                                                points = Convert.ToString(bas_avg);
                                                ConvertedMark(needs, sum_total, ref points);
                                                string aaa = Convert.ToString(points);
                                                FpSpread2.Sheets[0].Cells[i, col + 1].Text = aaa;
                                                FpSpread2.Sheets[0].Cells[i, col + 1].HorizontalAlign = HorizontalAlign.Center;
                                                if (Convert.ToString(dv[0]["total"]).Trim() != "")
                                                {
                                                    total = total + Convert.ToInt32(dv[0]["total"]);
                                                    streng = streng + Convert.ToInt32(dv[0]["Strength"]);
                                                    FpSpread2.Sheets[0].Columns[col].HorizontalAlign = HorizontalAlign.Center;
                                                }
                                            }
                                        }
                                    }
                                    FpSpread2.Sheets[0].Cells[i, FpSpread2.Sheets[0].ColumnCount - 2].Text = Convert.ToString(total);
                                    FpSpread2.Sheets[0].Cells[i, FpSpread2.Sheets[0].ColumnCount - 2].HorizontalAlign = HorizontalAlign.Center;
                                    needs = "5";
                                    //sum_total = "9";
                                    sum_total = d2.GetFunction("select top 1 Point from CO_MarkMaster  where CollegeCode in('" + collegecode1 + "') order by Point desc");
                                    Double point = total / streng;
                                    points = Convert.ToString(point);
                                    ConvertedMark(needs, sum_total, ref points);
                                    string grandgrd = Convert.ToString(points);
                                    FpSpread2.Sheets[0].Cells[i, FpSpread2.Sheets[0].ColumnCount - 1].Text = Convert.ToString(grandgrd);
                                    FpSpread2.Sheets[0].Cells[i, FpSpread2.Sheets[0].ColumnCount - 1].HorizontalAlign = HorizontalAlign.Center;
                                }
                                else if (cb_total1.Checked == true || cb_avg1.Checked == true)
                                {
                                    for (int col = 3; col < FpSpread2.Sheets[0].ColumnCount - 1; col++)
                                    {
                                        string subjectno = Convert.ToString(FpSpread2.Sheets[0].ColumnHeader.Cells[0, col].Tag);
                                        if (subjectno.Trim() != "")
                                        {
                                            ds.Tables[1].DefaultView.RowFilter = "QuestionMasterFK='" + Convert.ToString(ds.Tables[0].Rows[i]["QuestionMasterPK"]) + "' and SubjectNo='" + subjectno + "'";
                                            dv = ds.Tables[1].DefaultView;
                                            if (dv.Count > 0)
                                            {
                                                if (cb_total1.Checked == true)
                                                {
                                                    FpSpread2.Sheets[0].Cells[i, col].Text = Convert.ToString(dv[0]["total"]);
                                                    FpSpread2.Sheets[0].Cells[i, col].HorizontalAlign = HorizontalAlign.Center;
                                                }
                                                Int32 tot = Convert.ToInt32(dv[0]["total"]);
                                                int strengt = Convert.ToInt32(dv[0]["Strength"]);
                                                Double bas_avg = tot / strengt;
                                                // bas_avg = Math.Round(bas_avg, 0, MidpointRounding.AwayFromZero);
                                                points = Convert.ToString(bas_avg);
                                                ConvertedMark(needs, sum_total, ref points);
                                                string aaa = Convert.ToString(points);
                                                if (cb_avg1.Checked == true)
                                                {
                                                    FpSpread2.Sheets[0].Cells[i, col].Text = aaa;
                                                    FpSpread2.Sheets[0].Cells[i, col].HorizontalAlign = HorizontalAlign.Center;
                                                }
                                                if (Convert.ToString(dv[0]["total"]).Trim() != "")
                                                {
                                                    total = total + Convert.ToInt32(dv[0]["total"]);
                                                    streng = streng + Convert.ToInt32(dv[0]["Strength"]);
                                                    FpSpread2.Sheets[0].Columns[col].HorizontalAlign = HorizontalAlign.Center;
                                                }
                                            }
                                        }
                                    }
                                    FpSpread2.Sheets[0].Cells[i, FpSpread2.Sheets[0].ColumnCount - 2].Text = Convert.ToString(total);
                                    FpSpread2.Sheets[0].Cells[i, FpSpread2.Sheets[0].ColumnCount - 2].HorizontalAlign = HorizontalAlign.Center;
                                    needs = "5";
                                    sum_total = d2.GetFunction("select top 1 Point from CO_MarkMaster  where CollegeCode in('" + collegecode1 + "') order by Point desc");
                                    Double point = total / streng;
                                    points = Convert.ToString(point);
                                    ConvertedMark(needs, sum_total, ref points);
                                    string grandgrd = Convert.ToString(points);
                                    FpSpread2.Sheets[0].Cells[i, FpSpread2.Sheets[0].ColumnCount - 1].Text = Convert.ToString(grandgrd);
                                    FpSpread2.Sheets[0].Cells[i, FpSpread2.Sheets[0].ColumnCount - 1].HorizontalAlign = HorizontalAlign.Center;
                                }
                            }
                            FpSpread2.Sheets[0].Rows.Count++;
                            if (cb_total1.Checked == true && cb_avg1.Checked == true)
                            {
                                for (int j = 3; j < FpSpread2.Sheets[0].ColumnCount; j += 2)
                                {
                                    int grandtot = 0;
                                    Double grandavg = 0;
                                    for (int i = 0; i < FpSpread2.Sheets[0].Rows.Count; i++)
                                    {
                                        int total = Convert.ToInt32(FpSpread2.Sheets[0].Cells[i, j].Value);
                                        grandtot = grandtot + total;
                                        Double totalav = Convert.ToDouble(FpSpread2.Sheets[0].Cells[i, j + 1].Value);
                                        grandavg = grandavg + totalav;
                                    }
                                    Int32 coun = Convert.ToInt32(FpSpread2.Sheets[0].Rows.Count - 1);
                                    grandavg = grandavg / coun;
                                    grandavg = (Math.Round(grandavg, 2));
                                    if (grandtot == 0)
                                    {
                                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].Rows.Count - 1, j].Text = "";
                                    }
                                    else
                                    {
                                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].Rows.Count - 1, j].Text = Convert.ToString(grandtot);
                                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].Rows.Count - 1, j].HorizontalAlign = HorizontalAlign.Center;
                                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].Rows.Count - 1, j + 1].Text = Convert.ToString(grandavg);
                                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].Rows.Count - 1, j + 1].HorizontalAlign = HorizontalAlign.Center;
                                    }
                                }
                            }
                            else if (cb_total1.Checked == true || cb_avg1.Checked == true)
                            {
                                for (int j = 3; j < FpSpread2.Sheets[0].ColumnCount; j++)
                                {
                                    int grandtot = 0;
                                    Double grandavg = 0;
                                    Double grandt = 0;
                                    for (int i = 0; i < FpSpread2.Sheets[0].Rows.Count; i++)
                                    {
                                        if (cb_total1.Checked == true)
                                        {
                                            int total = Convert.ToInt32(FpSpread2.Sheets[0].Cells[i, j].Value);
                                            grandtot = grandtot + total;
                                        }
                                        if (cb_avg1.Checked == true)
                                        {
                                            Double totalav = Convert.ToDouble(FpSpread2.Sheets[0].Cells[i, j].Value);
                                            grandavg = grandavg + totalav;
                                        }
                                    }
                                    Int32 coun = Convert.ToInt32(FpSpread2.Sheets[0].Rows.Count - 1);
                                    grandt = grandavg;
                                    grandavg = grandavg / coun;
                                    grandavg = (Math.Round(grandavg, 2));
                                    if (cb_total1.Checked == true)
                                    {
                                        if (grandtot == 0)
                                        {
                                            FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].Rows.Count - 1, j].Text = "-";
                                        }
                                        else
                                        {
                                            FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].Rows.Count - 1, j].Text = Convert.ToString(grandtot);
                                            FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].Rows.Count - 1, j].HorizontalAlign = HorizontalAlign.Center;
                                        }
                                    }
                                    if (cb_avg1.Checked == true)
                                    {
                                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].Rows.Count - 1, j].Text = Convert.ToString(grandavg);
                                        if (j == FpSpread2.Sheets[0].ColumnCount - 2)
                                        {
                                            FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].Rows.Count - 1, j].Text = Convert.ToString(grandt);
                                        }
                                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].Rows.Count - 1, j].HorizontalAlign = HorizontalAlign.Center;
                                    }
                                }
                            }
                            FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].Rows.Count - 1, FpSpread2.Sheets[0].ColumnCount - 1].HorizontalAlign = HorizontalAlign.Center;
                            FpSpread2.Sheets[0].SpanModel.Add(FpSpread2.Sheets[0].RowCount - 1, 0, 1, 3);
                            FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].Rows.Count - 1, 0].Text = "Grand Total";
                            FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].Rows.Count - 1, 0].HorizontalAlign = HorizontalAlign.Right;
                            FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].Rows.Count - 1, 0].ForeColor = System.Drawing.Color.Blue;
                        }
                        else
                        {
                            imgdiv2.Visible = true;
                            lbl_alert1.Text = "No Records Found";
                            FpSpread2.Visible = false;
                            rptprint1.Visible = false;
                        }
                    }
                    else
                    {
                        imgdiv2.Visible = true;
                        lbl_alert1.Text = "No Records Found";
                        FpSpread2.Visible = false;
                        rptprint1.Visible = false;
                    }
                }
                else
                {
                    imgdiv2.Visible = true;
                    lbl_alert1.Text = "No Records Found";
                    FpSpread2.Visible = false;
                    rptprint1.Visible = false;
                }
            }
            else
            {
                imgdiv2.Visible = true;
                lbl_alert1.Text = "No Records Found";
                FpSpread2.Visible = false;
                rptprint1.Visible = false;
            }
            FpSpread2.Sheets[0].PageSize = FpSpread2.Sheets[0].RowCount;
            FpSpread2.Sheets[0].Columns[2].Width = 550;
            FpSpread2.Sheets[0].Columns[2].HorizontalAlign = HorizontalAlign.Left;
        }
        catch (Exception ex)
        {
            d2.sendErrorMail(ex, collegecode1, "FeedbackReport");
        }
    }

    public void ananames3()
    {
        try
        {
            chartfalse();
            fair();
            DataView dv = new DataView();
            FpSpread3.Visible = true;
            rptprint1.Visible = true;
            string college_cd = "";
            if (Cbl_college.Items.Count > 0)
            {
                for (int i = 0; i < Cbl_college.Items.Count; i++)
                {
                    if (Cbl_college.Items[i].Selected == true)
                    {
                        if (college_cd == "")
                        {
                            college_cd = "" + Cbl_college.Items[i].Value.ToString() + "";
                        }
                        else
                        {
                            college_cd = college_cd + "','" + Convert.ToString(Cbl_college.Items[i].Value);
                        }
                    }
                }
            }
            string Batch_Year = "";
            for (int i = 0; i < cbl_batch.Items.Count; i++)
            {
                if (cbl_batch.Items[i].Selected == true)
                {
                    if (Batch_Year == "")
                    {
                        Batch_Year = "" + cbl_batch.Items[i].Value.ToString() + "";
                    }
                    else
                    {
                        Batch_Year = Batch_Year + "','" + cbl_batch.Items[i].Value.ToString() + "";
                    }
                }
            }
            string degree_code = "";
            for (int i = 0; i < cbl_branch.Items.Count; i++)
            {
                if (cbl_branch.Items[i].Selected == true)
                {
                    if (degree_code == "")
                    {
                        degree_code = "" + cbl_branch.Items[i].Value.ToString() + "";
                    }
                    else
                    {
                        degree_code = degree_code + "','" + cbl_branch.Items[i].Value.ToString() + "";
                    }
                }
            }
            string section = "";
            for (int i = 0; i < cbl_sec.Items.Count; i++)
            {
                if (cbl_sec.Items[i].Selected == true)
                {
                    if (section == "")
                    {
                        section = "" + cbl_sec.Items[i].Value.ToString() + "";
                    }
                    else
                    {
                        section = section + "','" + cbl_sec.Items[i].Value.ToString() + "";
                    }
                    if (cbl_sec.Items[i].Value == "Empty")
                    {
                        section = "";
                    }
                }
            }
            if (section.Trim() != "")
            {
                section = section + "','";
            }
            string semester = "";
            for (int i = 0; i < cbl_sem.Items.Count; i++)
            {
                if (cbl_sem.Items[i].Selected == true)
                {
                    if (semester == "")
                    {
                        semester = "" + cbl_sem.Items[i].Value.ToString() + "";
                    }
                    else
                    {
                        semester = semester + "','" + cbl_sem.Items[i].Value.ToString() + "";
                    }
                }
            }
            FpSpread3.Sheets[0].RowCount = 0;
            FpSpread3.Sheets[0].ColumnCount = 0;
            FpSpread3.CommandBar.Visible = false;
            FpSpread3.Sheets[0].AutoPostBack = true;
            FpSpread3.Sheets[0].ColumnHeader.RowCount = 1;
            FpSpread3.Sheets[0].RowHeader.Visible = false;
            FpSpread3.Sheets[0].ColumnCount = 9;
            FarPoint.Web.Spread.StyleInfo darkstyle = new FarPoint.Web.Spread.StyleInfo();
            darkstyle.BackColor = ColorTranslator.FromHtml("#0CA6CA");
            darkstyle.ForeColor = System.Drawing.Color.White;
            FpSpread3.ActiveSheetView.ColumnHeader.DefaultStyle = darkstyle;
            FpSpread3.Visible = true;
            //FpSpread3.Width = 971;
            //FpSpread3.Height = 500;
            FpSpread3.SaveChanges();
            FpSpread3.Sheets[0].ColumnHeader.Cells[0, 0].Text = "S.No";
            FpSpread3.Sheets[0].ColumnHeader.Cells[0, 0].Font.Bold = true;
            FpSpread3.Sheets[0].ColumnHeader.Cells[0, 0].HorizontalAlign = HorizontalAlign.Center;
            FpSpread3.Sheets[0].ColumnHeader.Cells[0, 0].Font.Name = "Book Antiqua";
            FpSpread3.Sheets[0].ColumnHeader.Cells[0, 0].Font.Size = FontUnit.Medium;
            FpSpread3.Sheets[0].Columns[0].HorizontalAlign = HorizontalAlign.Center;
            FpSpread3.Sheets[0].ColumnHeader.Cells[0, 1].Text = "BranchName";
            FpSpread3.Sheets[0].ColumnHeader.Cells[0, 1].Font.Bold = true;
            FpSpread3.Sheets[0].ColumnHeader.Cells[0, 1].Font.Name = "Book Antiqua";
            FpSpread3.Sheets[0].ColumnHeader.Cells[0, 1].Font.Size = FontUnit.Medium;
            FpSpread3.Sheets[0].ColumnHeader.Cells[0, 1].HorizontalAlign = HorizontalAlign.Center;
            FpSpread3.Sheets[0].ColumnHeader.Cells[0, 2].Text = "Semester ";
            FpSpread3.Sheets[0].ColumnHeader.Cells[0, 2].Font.Bold = true;
            FpSpread3.Sheets[0].ColumnHeader.Cells[0, 2].Font.Name = "Book Antiqua";
            FpSpread3.Sheets[0].ColumnHeader.Cells[0, 2].Font.Size = FontUnit.Medium;
            FpSpread3.Sheets[0].ColumnHeader.Cells[0, 2].HorizontalAlign = HorizontalAlign.Center;
            FpSpread3.Sheets[0].ColumnHeader.Cells[0, 3].Text = "SectionName ";
            FpSpread3.Sheets[0].ColumnHeader.Cells[0, 3].Font.Bold = true;
            FpSpread3.Sheets[0].ColumnHeader.Cells[0, 3].Font.Name = "Book Antiqua";
            FpSpread3.Sheets[0].ColumnHeader.Cells[0, 3].Font.Size = FontUnit.Medium;
            FpSpread3.Sheets[0].ColumnHeader.Cells[0, 3].HorizontalAlign = HorizontalAlign.Center;
            FpSpread3.Sheets[0].ColumnHeader.Cells[0, 4].Text = "FromYear";
            FpSpread3.Sheets[0].ColumnHeader.Cells[0, 4].Font.Bold = true;
            FpSpread3.Sheets[0].ColumnHeader.Cells[0, 4].Font.Name = "Book Antiqua";
            FpSpread3.Sheets[0].ColumnHeader.Cells[0, 4].Font.Size = FontUnit.Medium;
            FpSpread3.Sheets[0].ColumnHeader.Cells[0, 4].HorizontalAlign = HorizontalAlign.Center;
            FpSpread3.Sheets[0].Columns[4].HorizontalAlign = HorizontalAlign.Center;
            FpSpread3.Sheets[0].ColumnHeader.Cells[0, 5].Text = "ToYear";
            FpSpread3.Sheets[0].ColumnHeader.Cells[0, 5].Font.Bold = true;
            FpSpread3.Sheets[0].ColumnHeader.Cells[0, 5].Font.Name = "Book Antiqua";
            FpSpread3.Sheets[0].ColumnHeader.Cells[0, 5].Font.Size = FontUnit.Medium;
            FpSpread3.Sheets[0].ColumnHeader.Cells[0, 5].HorizontalAlign = HorizontalAlign.Center;
            FpSpread3.Sheets[0].ColumnHeader.Cells[0, 6].Text = "Total Strength";
            FpSpread3.Sheets[0].ColumnHeader.Cells[0, 6].Font.Bold = true;
            FpSpread3.Sheets[0].ColumnHeader.Cells[0, 6].Font.Name = "Book Antiqua";
            FpSpread3.Sheets[0].ColumnHeader.Cells[0, 6].Font.Size = FontUnit.Medium;
            FpSpread3.Sheets[0].ColumnHeader.Cells[0, 6].HorizontalAlign = HorizontalAlign.Center;
            FpSpread3.Sheets[0].ColumnHeader.Cells[0, 7].Text = "Total Attended";
            FpSpread3.Sheets[0].ColumnHeader.Cells[0, 7].Font.Bold = true;
            FpSpread3.Sheets[0].ColumnHeader.Cells[0, 7].Font.Name = "Book Antiqua";
            FpSpread3.Sheets[0].ColumnHeader.Cells[0, 7].Font.Size = FontUnit.Medium;
            FpSpread3.Sheets[0].ColumnHeader.Cells[0, 7].HorizontalAlign = HorizontalAlign.Center;
            FpSpread3.Sheets[0].ColumnHeader.Cells[0, 8].Text = "Total Remaining";
            FpSpread3.Sheets[0].ColumnHeader.Cells[0, 8].Font.Bold = true;
            FpSpread3.Sheets[0].ColumnHeader.Cells[0, 8].Font.Name = "Book Antiqua";
            FpSpread3.Sheets[0].ColumnHeader.Cells[0, 8].Font.Size = FontUnit.Medium;
            FpSpread3.Sheets[0].ColumnHeader.Cells[0, 8].HorizontalAlign = HorizontalAlign.Center;
            FpSpread3.Sheets[0].ColumnHeader.Columns[0].Width = 59;
            ds.Clear();
            int rmaining = 0;
            string selqry = "";
            //12.10.16
            //selqry = " select COUNT( distinct  s.FeedbackUnicode) as Strength,f.FeedBackMasterPK,Batch_Year ,f.semester,f.DegreeCode,f.Section from CO_FeedBackMaster F,CO_StudFeedBack S,CO_FeedbackUniCode fu where  fu.FeedbackUnicode=s.FeedbackUnicode and fu.FeedbackMasterFK=s.FeedBackMasterFK and  s.FeedBackMasterFK =f.FeedBackMasterPK  and f.degreecode in ('" + degree_code + "') and f.Batch_Year in('" + Batch_Year + "') and f.semester in ('" + semester + "') and f.FeedBackName='" + Convert.ToString(ddl_Feedbackname.SelectedItem.Text) + "'";
            selqry = " select COUNT( distinct  s.FeedbackUnicode) as Strength,f.FeedBackMasterPK,Batch_Year ,f.semester,f.DegreeCode,f.Section from CO_FeedBackMaster F,CO_StudFeedBack S where s.FeedBackMasterFK =f.FeedBackMasterPK  and f.degreecode in ('" + degree_code + "') and f.Batch_Year in('" + Batch_Year + "') and f.semester in ('" + semester + "') and f.FeedBackName='" + Convert.ToString(ddl_Feedbackname.SelectedItem.Text) + "'  and f.InclueCommon='1' and s.FeedbackUnicode<>''";
            if (section != "")
            {
                selqry = selqry + " and f.Section in ('" + section + "')  ";
            }
            selqry = selqry + " group by f.FeedBackMasterPK  ,Batch_Year ,f.semester,f.DegreeCode ,f.Section";
            selqry = selqry + " SELECT Course_Name+'-'+Dept_Name Degree,Current_semester,R.degree_code,Sections,Batch_Year,COUNT(*)as TotStrengh FROM Registration R,Degree G,Course C,Department D WHERE R.degree_code = G.Degree_Code AND G.Course_Id = C.Course_Id AND G.college_code = C.college_code AND g.Dept_Code = d.Dept_Code and g.college_code = d.college_code AND CC = 0 AND DelFlag = 0 AND Exam_Flag = 'OK' and r.degree_code in ('" + degree_code + "') and r.Batch_Year in('" + Batch_Year + "') ";//and r.Current_Semester in ('" + semester + "')
            if (section != "")
            {
                selqry = selqry + " and r.Sections in ('" + section + "') GROUP BY Course_Name,R.degree_code, Dept_Name,Current_semester,Sections, Batch_Year ORDER BY R.degree_code,Current_Semester,Sections , Batch_Year,Course_Name,Dept_Name ";
            }
            else
            {
                selqry = selqry + " GROUP BY Course_Name, R.degree_code, Dept_Name,Current_semester,Sections,Batch_Year ORDER BY R.degree_code,Current_Semester,Sections , Batch_Year,Course_Name,Dept_Name";
            }
            selqry = selqry + "   SELECT Course_Name+'-'+Dept_Name Degree,semester,f.DegreeCode,Section,Batch_Year FROM Degree G,Course C,Department D,CO_FeedBackMaster F,CO_StudFeedBack S WHERE f.FeedBackMasterPK=s.FeedBackMasterFK and f.DegreeCode = G.Degree_Code AND G.Course_Id = C.Course_Id AND G.college_code = C.college_code AND g.Dept_Code = d.Dept_Code and g.college_code = d.college_code AND  f.degreecode in ('" + degree_code + "') and f.Batch_Year in('" + Batch_Year + "') and f.Semester in ('" + semester + "') and f.FeedBackName='" + Convert.ToString(ddl_Feedbackname.SelectedItem.Text) + "'  and f.InclueCommon='1' and s.FeedbackUnicode<>''";
            if (section != "")
            {
                selqry = selqry + " and f.Section in ('" + section + "')   GROUP BY Course_Name,f.degreecode, Dept_Name,semester,Section, Batch_Year ORDER BY f.degreecode,Semester,Section , Batch_Year,Course_Name,Dept_Name ";
            }
            else
            {
                selqry = selqry + "   GROUP BY Course_Name,f.degreecode, Dept_Name,semester,Section, Batch_Year ORDER BY f.degreecode,Semester,Section , Batch_Year,Course_Name,Dept_Name ";
            }
            ds = d2.select_method_wo_parameter(selqry, "Text");
            DataView dvnew = new DataView();
            DataView totalview = new DataView();
            if (ds.Tables.Count > 0)
            {
                if (ds.Tables[0].Rows.Count > 0 && ds.Tables[1].Rows.Count > 0 && ds.Tables[2].Rows.Count > 0)
                {
                    lbl_headig.Text = " Student Count Report";
                    for (int i = 0; i < ds.Tables[2].Rows.Count; i++)
                    {
                        FpSpread3.Sheets[0].Rows.Count++;
                        FpSpread3.Sheets[0].Cells[FpSpread3.Sheets[0].RowCount - 1, 0].Text = Convert.ToString(i + 1);
                        if (((i + 1) % 2) == 0)
                        {
                            FpSpread3.Sheets[0].Rows[FpSpread3.Sheets[0].RowCount - 1].BackColor = System.Drawing.Color.LightSeaGreen;
                        }
                        FpSpread3.Sheets[0].Cells[FpSpread3.Sheets[0].RowCount - 1, 1].Text = ds.Tables[2].Rows[i]["Degree"].ToString();
                        FpSpread3.Sheets[0].Cells[FpSpread3.Sheets[0].RowCount - 1, 1].Tag = ds.Tables[2].Rows[i]["DegreeCode"].ToString();
                        FpSpread3.Sheets[0].Cells[FpSpread3.Sheets[0].RowCount - 1, 2].Text = ds.Tables[2].Rows[i]["semester"].ToString();
                        FpSpread3.Sheets[0].Cells[FpSpread3.Sheets[0].RowCount - 1, 3].Text = ds.Tables[2].Rows[i]["Section"].ToString();
                        FpSpread3.Sheets[0].Cells[FpSpread3.Sheets[0].RowCount - 1, 4].Text = ds.Tables[2].Rows[i]["Batch_Year"].ToString();
                        FpSpread3.Sheets[0].Cells[FpSpread3.Sheets[0].RowCount - 1, 5].Text = ds.Tables[2].Rows[i]["Batch_Year"].ToString();
                        string sectons = Convert.ToString(ds.Tables[2].Rows[i]["Section"]);
                        string degrecode = Convert.ToString(ds.Tables[2].Rows[i]["degreecode"]);
                        string sem = Convert.ToString(ds.Tables[2].Rows[i]["semester"]);
                        string totalfind = " degree_code='" + degrecode + "'  and  Batch_Year='" + ds.Tables[2].Rows[i]["Batch_Year"].ToString() + "'";
                        if (sectons.Trim() != "")
                        {
                            totalfind = totalfind + " and Sections='" + sectons + "'";
                        }
                        ds.Tables[1].DefaultView.RowFilter = "" + totalfind + "";
                        totalview = ds.Tables[1].DefaultView;
                        int total = 0;
                        if (totalview.Count > 0)
                        {
                            FpSpread3.Sheets[0].Cells[FpSpread3.Sheets[0].RowCount - 1, 6].Text = totalview[0]["TotStrengh"].ToString();
                            string totas = totalview[0]["TotStrengh"].ToString();
                            if (totas.Trim() == "")
                            {
                                totas = "0";
                            }
                            total = Convert.ToInt32(totas);
                        }
                        string filterquery = "";
                        int remaining = 0;
                        int attand = 0;
                        filterquery = "degreecode='" + degrecode + "'  and  semester='" + sem + "' ";
                        if (sectons.Trim() != "")
                        {
                            filterquery = filterquery + " and Section='" + sectons + "'";
                        }
                        ds.Tables[0].DefaultView.RowFilter = "" + filterquery + "";
                        dvnew = ds.Tables[0].DefaultView;
                        if (dvnew.Count > 0)
                        {
                            FpSpread3.Sheets[0].Cells[FpSpread3.Sheets[0].RowCount - 1, 7].Text = Convert.ToString(dvnew[0]["Strength"]);
                            attand = Convert.ToInt32(dvnew[0]["Strength"]);
                        }
                        // total = Convert.ToInt32(ds.Tables[1].Rows[i]["TotStrengh"]);
                        remaining = total - attand;
                        FpSpread3.Sheets[0].Cells[FpSpread3.Sheets[0].RowCount - 1, 8].Text = Convert.ToString(remaining);
                    }
                    FpSpread3.Sheets[0].Columns[2].HorizontalAlign = HorizontalAlign.Center;
                    FpSpread3.Sheets[0].Columns[5].HorizontalAlign = HorizontalAlign.Center;
                    FpSpread3.Sheets[0].Columns[6].HorizontalAlign = HorizontalAlign.Center;
                    FpSpread3.Sheets[0].Columns[7].HorizontalAlign = HorizontalAlign.Center;
                    FpSpread3.Sheets[0].Columns[8].HorizontalAlign = HorizontalAlign.Center;
                    FpSpread3.Sheets[0].Columns[3].HorizontalAlign = HorizontalAlign.Center;
                    FpSpread3.Sheets[0].PageSize = ds.Tables[1].Rows.Count;
                }
                else
                {
                    imgdiv2.Visible = true;
                    lbl_alert1.Text = "No Records Found";
                    FpSpread3.Visible = false;
                    rptprint1.Visible = false;
                }
            }
            else
            {
                imgdiv2.Visible = true;
                lbl_alert1.Text = "No Records Found";
                FpSpread3.Visible = false;
                rptprint1.Visible = false;
            }
            FpSpread3.Sheets[0].PageSize = FpSpread3.Sheets[0].RowCount;
        }
        catch (Exception ex)
        {
            d2.sendErrorMail(ex, collegecode1, "ananmous format3");
        }
    }

    public void format2()
    {
        try
        {
            chartprint.Visible = false;
            lbl_headig.Visible = true;
            chartfalse();
            cb_total.Visible = true;
            cb_avg.Visible = true;
            fair();
            FpSpread2.Visible = true;
            rptprint1.Visible = true;
            string Batch_Year = "";
            for (int i = 0; i < cbl_batch.Items.Count; i++)
            {
                if (cbl_batch.Items[i].Selected == true)
                {
                    if (Batch_Year == "")
                    {
                        Batch_Year = "" + cbl_batch.Items[i].Value.ToString() + "";
                    }
                    else
                    {
                        Batch_Year = Batch_Year + "','" + cbl_batch.Items[i].Value.ToString() + "";
                    }
                }
            }
            string college_cd = "";
            if (Cbl_college.Items.Count > 0)
            {
                for (int i = 0; i < Cbl_college.Items.Count; i++)
                {
                    if (Cbl_college.Items[i].Selected == true)
                    {
                        if (college_cd == "")
                        {
                            college_cd = "" + Cbl_college.Items[i].Value.ToString() + "";
                        }
                        else
                        {
                            college_cd = college_cd + "','" + Convert.ToString(Cbl_college.Items[i].Value);
                        }
                    }
                }
            }
            string degree_code = "";
            for (int i = 0; i < cbl_branch.Items.Count; i++)
            {
                if (cbl_branch.Items[i].Selected == true)
                {
                    if (degree_code == "")
                    {
                        degree_code = "" + cbl_branch.Items[i].Value.ToString() + "";
                    }
                    else
                    {
                        degree_code = degree_code + "','" + cbl_branch.Items[i].Value.ToString() + "";
                    }
                }
            }
            string semester = "";
            for (int i = 0; i < cbl_sem.Items.Count; i++)
            {
                if (cbl_sem.Items[i].Selected == true)
                {
                    if (semester == "")
                    {
                        semester = "" + cbl_sem.Items[i].Value.ToString() + "";
                    }
                    else
                    {
                        semester = semester + "','" + cbl_sem.Items[i].Value.ToString() + "";
                    }
                }
            }
            string section = "";
            for (int i = 0; i < cbl_sec.Items.Count; i++)
            {
                if (cbl_sec.Items[i].Selected == true)
                {
                    if (section == "")
                    {
                        section = "" + cbl_sec.Items[i].Value.ToString() + "";
                    }
                    else
                    {
                        section = section + "','" + cbl_sec.Items[i].Value.ToString() + "";
                    }
                    if (cbl_sec.Items[i].Value == "Empty")
                    {
                        section = "";
                    }
                }
            }
            if (section.Trim() != "")
            {
                section = section + "','";
            }
            dsfb.Clear();
            string fbpk = " select FeedBackMasterPK from CO_FeedBackMaster where FeedBackName ='" + ddl_Feedbackname.SelectedItem.Value + "' and DegreeCode in ('" + degree_code + "') and semester in ('" + semester + "') and Batch_Year in('" + Batch_Year + "') and section in ('" + section + "')";
            dsfb = d2.select_method_wo_parameter(fbpk, "Text");
            string feedbakpk = "";
            if (dsfb.Tables.Count > 0)
            {
                if (dsfb.Tables[0].Rows.Count > 0)
                {
                    for (int pk = 0; pk < dsfb.Tables[0].Rows.Count; pk++)
                    {
                        if (feedbakpk == "")
                        {
                            feedbakpk = dsfb.Tables[0].Rows[pk]["FeedBackMasterPK"].ToString();
                        }
                        else
                        {
                            feedbakpk = feedbakpk + "','" + dsfb.Tables[0].Rows[pk]["FeedBackMasterPK"].ToString() + "";
                        }
                    }
                }
            }
            //and cf.FeedBackMasterPK in('" + feedbakpk + "')
            string type = "";
            if (rb_Acad.Checked == true)
            {
                type = "1";
            }
            else if (rb_Gend.Checked == true)
            {
                type = "2";
            }
            FpSpread2.Sheets[0].RowCount = 0;
            FpSpread2.Sheets[0].ColumnCount = 0;
            FpSpread2.CommandBar.Visible = false;
            FpSpread2.Sheets[0].AutoPostBack = true;
            FpSpread2.Sheets[0].ColumnHeader.RowCount = 2;
            FpSpread2.Sheets[0].RowHeader.Visible = false;
            FpSpread2.Sheets[0].ColumnCount = 3;
            FarPoint.Web.Spread.StyleInfo darkstyle = new FarPoint.Web.Spread.StyleInfo();
            darkstyle.BackColor = ColorTranslator.FromHtml("#0CA6CA");
            darkstyle.ForeColor = System.Drawing.Color.White;
            FpSpread2.ActiveSheetView.ColumnHeader.DefaultStyle = darkstyle;
            FpSpread2.Visible = true;
            //FpSpread2.Width = 971;
            //FpSpread2.Height = 500;
            FpSpread2.SaveChanges();
            FpSpread2.Sheets[0].ColumnHeader.Cells[0, 0].Text = "S.No";
            FpSpread2.Sheets[0].ColumnHeader.Cells[0, 0].HorizontalAlign = HorizontalAlign.Center;
            FpSpread2.Sheets[0].ColumnHeader.Cells[0, 0].Font.Bold = true;
            FpSpread2.Sheets[0].ColumnHeader.Cells[0, 0].Font.Name = "Book Antiqua";
            FpSpread2.Sheets[0].ColumnHeader.Cells[0, 0].Font.Size = FontUnit.Medium;
            FpSpread2.Sheets[0].Columns[0].HorizontalAlign = HorizontalAlign.Center;
            FpSpread2.Sheets[0].ColumnHeader.Cells[0, 1].Text = "EvaluationName";
            FpSpread2.Sheets[0].ColumnHeader.Cells[0, 1].Font.Bold = true;
            FpSpread2.Sheets[0].ColumnHeader.Cells[0, 1].Font.Name = "Book Antiqua";
            FpSpread2.Sheets[0].ColumnHeader.Cells[0, 1].Font.Size = FontUnit.Medium;
            FpSpread2.Sheets[0].ColumnHeader.Cells[0, 1].HorizontalAlign = HorizontalAlign.Center;
            FpSpread2.Sheets[0].ColumnHeader.Cells[0, 2].Text = "Description ";
            FpSpread2.Sheets[0].ColumnHeader.Cells[0, 2].Font.Bold = true;
            FpSpread2.Sheets[0].ColumnHeader.Cells[0, 2].Font.Name = "Book Antiqua";
            FpSpread2.Sheets[0].ColumnHeader.Cells[0, 2].Font.Size = FontUnit.Medium;
            FpSpread2.Sheets[0].ColumnHeader.Cells[0, 2].HorizontalAlign = HorizontalAlign.Center;
            FpSpread2.Sheets[0].ColumnHeader.Columns[0].Width = 59;
            FpSpread2.Sheets[0].ColumnHeader.Columns[1].Width = 80;
            FpSpread2.Sheets[0].Columns[2].HorizontalAlign = HorizontalAlign.Left;
            FpSpread2.Sheets[0].ColumnHeaderSpanModel.Add(0, 0, 2, 1);
            FpSpread2.Sheets[0].ColumnHeaderSpanModel.Add(0, 1, 2, 1);
            FpSpread2.Sheets[0].ColumnHeaderSpanModel.Add(0, 2, 2, 1);
            ds.Clear();
            string subject_cd = "";
            string selectquestion = "";
            string selqry = "";
            string headershow = "";
            if (ddl_headershow.SelectedItem.Value == "0")
            {
                headershow = " ,subject_name as codeandname";
            }
            else if (ddl_headershow.SelectedItem.Value == "1")
            {
                headershow = " ,subject_code as codeandname";
            }
            else if (ddl_headershow.SelectedItem.Value == "2")
            {
                headershow = " ,subject_code+'-'+subject_name as codeandname";
            }
            selqry = "   select DISTINCT(subject_code),c.subject_no,subject_name " + headershow + " from Registration r,subjectChooser c ,subject s,CO_StudFeedBack F,CO_FeedBackMaster FM where r.Roll_No = c.roll_no and r.Current_Semester = c.semester and F.SubjectNo =s.subject_no and f.SubjectNo =c.subject_no and c.subject_no = s.subject_no and fm.FeedBackMasterPK =f.FeedBackMasterFK  and Current_Semester in ('" + semester + "') and r.college_code in ('" + college_cd + "') and r.degree_code in ('" + degree_code + "')  and r.Batch_Year in ('" + Batch_Year + "') and fm.FeedBackName='" + ddl_Feedbackname.SelectedItem.Value + "'";
            // select distinct (select TextVal from TextValTable where TextCode = HeaderCode) as HeaderName,HeaderCode,Question,QuestionMasterPK from CO_QuestionMaster cq,CO_FeedBackMaster cf,CO_FeedBackQuestions cb,CO_StudFeedBack s where cq.QuestionMasterPK =cb.QuestionMasterFK and cf.FeedBackMasterPK =cb.FeedBackMasterFK and cf.CollegeCode in ('13') and 
            //s.FeedBackMasterFK =cf.FeedBackMasterPK and s.QuestionMasterFK =cq.QuestionMasterPK and cf.FeedBackType =('1') and cf.DegreeCode in ('45','46','47','50','51','52','53','54','48','60','62','64','58','59','61','49','55') and cf.Batch_Year in ('2015','2014','2013','2012','2011','2010') and cf.FeedBackName='Academic Feedback  Test' and cf.semester in ('1','2','3','4','5','6','7','8','9')  and cf.Section in ('A','B','C','')
            if (section != "")
            {
                selqry = selqry + " and r.Sections in ('" + section + "')";
            }
            selqry = selqry + "  select DISTINCT (subject_code),c.subject_no,subject_name ,f.StaffApplNo ,appl_name,subject_code+'-'+subject_name as codeandname   from Registration r,subjectChooser c ,subject s,CO_StudFeedBack F,CO_FeedBackMaster FM ,staff_appl_master a where a.appl_id=f.StaffApplNo and r.Roll_No = c.roll_no and r.Current_Semester = c.semester and F.SubjectNo =s.subject_no and f.SubjectNo =c.subject_no and c.subject_no = s.subject_no and fm.FeedBackMasterPK =f.FeedBackMasterFK  and Current_Semester in ('" + semester + "') and r.college_code in ('" + college_cd + "') and r.degree_code in ('" + degree_code + "')  and r.Batch_Year in ('" + Batch_Year + "') and fm.FeedBackName='" + ddl_Feedbackname.SelectedItem.Value + "'";
            ds = d2.select_method_wo_parameter(selqry, "Text");
            if (ds.Tables.Count > 0)
            {
                if (ds.Tables[0].Rows.Count > 0)
                {
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        subject_cd = ds.Tables[0].Rows[i]["subject_no"].ToString();
                        subject_cd = subject_cd + "','" + ds.Tables[0].Rows[i]["subject_no"].ToString();
                    }
                    if (cb_total.Checked == true && cb_avg.Checked)
                    {
                        for (int row = 0; row < ds.Tables[0].Rows.Count; row++)
                        {
                            FpSpread2.Sheets[0].ColumnCount++;
                            //FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, 7, 1, 3);
                            FpSpread2.Sheets[0].ColumnHeader.Cells[0, FpSpread2.Sheets[0].ColumnCount - 1].Text = Convert.ToString(ds.Tables[0].Rows[row]["codeandname"]);
                            FpSpread2.Sheets[0].ColumnHeader.Cells[0, FpSpread2.Sheets[0].ColumnCount - 1].Font.Bold = true;
                            FpSpread2.Sheets[0].ColumnHeader.Cells[0, FpSpread2.Sheets[0].ColumnCount - 1].Font.Name = "Book Antiqua";
                            FpSpread2.Sheets[0].ColumnHeader.Cells[0, FpSpread2.Sheets[0].ColumnCount - 1].Font.Size = FontUnit.Medium;
                            FpSpread2.Sheets[0].ColumnHeader.Cells[1, FpSpread2.Sheets[0].ColumnCount - 1].Text = "Total";
                            FpSpread2.Sheets[0].ColumnHeader.Cells[1, FpSpread2.Sheets[0].ColumnCount - 1].Font.Bold = true;
                            FpSpread2.Sheets[0].ColumnHeader.Cells[1, FpSpread2.Sheets[0].ColumnCount - 1].Font.Name = "Book Antiqua";
                            FpSpread2.Sheets[0].ColumnHeader.Cells[1, FpSpread2.Sheets[0].ColumnCount - 1].HorizontalAlign = HorizontalAlign.Center;
                            FpSpread2.Sheets[0].ColumnHeader.Cells[0, FpSpread2.Sheets[0].ColumnCount - 1].Tag = Convert.ToString(ds.Tables[0].Rows[row]["subject_no"]);
                            FpSpread2.Sheets[0].ColumnCount++;
                            FpSpread2.Columns[FpSpread2.Sheets[0].ColumnCount - 1].Visible = true;
                            FpSpread2.Sheets[0].ColumnHeader.Cells[1, FpSpread2.Sheets[0].ColumnCount - 1].Text = "Avg";
                            FpSpread2.Sheets[0].ColumnHeader.Cells[1, FpSpread2.Sheets[0].ColumnCount - 1].HorizontalAlign = HorizontalAlign.Center;
                            FpSpread2.Sheets[0].ColumnHeaderSpanModel.Add(0, FpSpread2.Sheets[0].ColumnCount - 2, 1, 2);
                            FpSpread2.Sheets[0].ColumnHeader.Cells[1, FpSpread2.Sheets[0].ColumnCount - 1].Font.Bold = true;
                            FpSpread2.Sheets[0].ColumnHeader.Cells[1, FpSpread2.Sheets[0].ColumnCount - 1].Font.Name = "Book Antiqua";
                            FpSpread2.Sheets[0].ColumnHeader.Cells[1, FpSpread2.Sheets[0].ColumnCount - 1].Font.Size = FontUnit.Medium;
                            FpSpread2.Sheets[0].ColumnHeader.Cells[1, FpSpread2.Sheets[0].ColumnCount - 1].HorizontalAlign = HorizontalAlign.Center;
                        }
                    }
                    else if (cb_total.Checked == true || cb_avg.Checked == true)
                    {
                        for (int row = 0; row < ds.Tables[0].Rows.Count; row++)
                        {
                            if (cb_total.Checked == true)
                            {
                                FpSpread2.Sheets[0].ColumnCount++;
                                //FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, 7, 1, 3);
                                FpSpread2.Sheets[0].ColumnHeader.Cells[0, FpSpread2.Sheets[0].ColumnCount - 1].Text = Convert.ToString(ds.Tables[0].Rows[row]["codeandname"]);
                                FpSpread2.Sheets[0].ColumnHeader.Cells[0, FpSpread2.Sheets[0].ColumnCount - 1].HorizontalAlign = HorizontalAlign.Center;
                                FpSpread2.Sheets[0].ColumnHeader.Cells[1, FpSpread2.Sheets[0].ColumnCount - 1].Text = "Total";
                                FpSpread2.Sheets[0].ColumnHeader.Cells[1, FpSpread2.Sheets[0].ColumnCount - 1].HorizontalAlign = HorizontalAlign.Center;
                                FpSpread2.Sheets[0].ColumnHeader.Cells[0, FpSpread2.Sheets[0].ColumnCount - 1].Tag = Convert.ToString(ds.Tables[0].Rows[row]["subject_no"]);
                            }
                            if (cb_avg.Checked == true)
                            {
                                FpSpread2.Sheets[0].ColumnCount++;
                                FpSpread2.Sheets[0].ColumnHeader.Cells[0, FpSpread2.Sheets[0].ColumnCount - 1].Text = Convert.ToString(ds.Tables[0].Rows[row]["codeandname"]);
                                FpSpread2.Sheets[0].ColumnHeader.Cells[0, FpSpread2.Sheets[0].ColumnCount - 1].Tag = Convert.ToString(ds.Tables[0].Rows[row]["subject_no"]);
                                FpSpread2.Sheets[0].ColumnHeader.Cells[1, FpSpread2.Sheets[0].ColumnCount - 1].Text = "Avg";
                                FpSpread2.Sheets[0].ColumnHeader.Cells[1, FpSpread2.Sheets[0].ColumnCount - 1].HorizontalAlign = HorizontalAlign.Center;
                                // FpSpread2.Sheets[0].ColumnHeaderSpanModel.Add(0, FpSpread2.Sheets[0].ColumnCount - 2, 1, 2);
                                FpSpread2.Sheets[0].ColumnHeader.Cells[0, FpSpread2.Sheets[0].ColumnCount - 1].Font.Bold = true;
                                FpSpread2.Sheets[0].ColumnHeader.Cells[0, FpSpread2.Sheets[0].ColumnCount - 1].Font.Name = "Book Antiqua";
                                FpSpread2.Sheets[0].ColumnHeader.Cells[0, FpSpread2.Sheets[0].ColumnCount - 1].Font.Size = FontUnit.Medium;
                                FpSpread2.Sheets[0].ColumnHeader.Cells[0, FpSpread2.Sheets[0].ColumnCount - 1].HorizontalAlign = HorizontalAlign.Center;
                            }
                        }
                    }
                    FpSpread2.Sheets[0].ColumnCount++;
                    FpSpread2.Sheets[0].ColumnHeader.Cells[0, FpSpread2.Sheets[0].ColumnCount - 1].Text = "GrandTotal";
                    FpSpread2.Sheets[0].ColumnHeaderSpanModel.Add(0, FpSpread2.Sheets[0].ColumnCount - 1, 2, 1);
                    FpSpread2.Sheets[0].ColumnHeader.Cells[0, FpSpread2.Sheets[0].ColumnCount - 1].Font.Bold = true;
                    FpSpread2.Sheets[0].ColumnHeader.Cells[0, FpSpread2.Sheets[0].ColumnCount - 1].Font.Name = "Book Antiqua";
                    FpSpread2.Sheets[0].ColumnHeader.Cells[0, FpSpread2.Sheets[0].ColumnCount - 1].Font.Size = FontUnit.Medium;
                    FpSpread2.Sheets[0].ColumnHeader.Cells[0, FpSpread2.Sheets[0].ColumnCount - 1].HorizontalAlign = HorizontalAlign.Center;
                    FpSpread2.Sheets[0].ColumnCount++;
                    FpSpread2.Sheets[0].ColumnHeader.Cells[0, FpSpread2.Sheets[0].ColumnCount - 1].Text = "GrandAvg";
                    FpSpread2.Columns[FpSpread2.Sheets[0].ColumnCount - 1].Visible = true;
                    FpSpread2.Sheets[0].ColumnHeaderSpanModel.Add(0, FpSpread2.Sheets[0].ColumnCount - 1, 2, 1);
                    FpSpread2.Sheets[0].ColumnHeader.Cells[0, FpSpread2.Sheets[0].ColumnCount - 1].Font.Bold = true;
                    FpSpread2.Sheets[0].ColumnHeader.Cells[0, FpSpread2.Sheets[0].ColumnCount - 1].Font.Name = "Book Antiqua";
                    FpSpread2.Sheets[0].ColumnHeader.Cells[0, FpSpread2.Sheets[0].ColumnCount - 1].Font.Size = FontUnit.Medium;
                    FpSpread2.Sheets[0].ColumnHeader.Cells[0, FpSpread2.Sheets[0].ColumnCount - 1].HorizontalAlign = HorizontalAlign.Center;
                    //selectquestion = " select (select TextVal from TextValTable where TextCode = HeaderCode) as HeaderName,HeaderCode,Question,QuestionMasterPK from CO_QuestionMaster cq,CO_FeedBackMaster cf,CO_FeedBackQuestions cb where cq.QuestionMasterPK =cb.QuestionMasterFK and cf.FeedBackMasterPK =cb.FeedBackMasterFK and cf.CollegeCode in ('" + college_cd + "') and  cf.FeedBackType =('" + type + "') and cf.DegreeCode in ('" + degree_code + "') and cf.Batch_Year in ('" + Batch_Year + "') and cf.FeedBackName='" + ddl_Feedbackname.SelectedItem.Value + "' and cf.semester in ('" + semester + "') ";
                    //*****23-02-2016(distinct prob)
                    selectquestion = "select distinct (select TextVal from TextValTable where TextCode = HeaderCode) as HeaderName,HeaderCode,Question,QuestionMasterPK from CO_QuestionMaster cq,CO_FeedBackMaster cf,CO_FeedBackQuestions cb,CO_StudFeedBack s where cq.QuestionMasterPK =cb.QuestionMasterFK and cf.FeedBackMasterPK =cb.FeedBackMasterFK and cf.CollegeCode in ('" + college_cd + "') and  s.FeedBackMasterFK =cf.FeedBackMasterPK and s.QuestionMasterFK =cq.QuestionMasterPK and  cf.FeedBackType =('" + type + "') and cf.DegreeCode in ('" + degree_code + "') and cf.Batch_Year in ('" + Batch_Year + "') and cf.FeedBackMasterPK in('" + feedbakpk + "') and cf.semester in ('" + semester + "') ";
                    if (section != "")
                    {
                        selectquestion = selectquestion + " and cf.Section in ('" + section + "')";
                    }
                    selectquestion = selectquestion + "select SUM(point)as total,cs.QuestionMasterFK,SubjectNo,COUNT( cs.app_no)as Strength from CO_StudFeedBack cs,CO_MarkMaster cm, CO_FeedBackMaster fm where cs.MarkMasterPK =cm.MarkMasterPK and fm.FeedBackMasterPK =cs.FeedBackMasterFK and fm.DegreeCode in ('" + degree_code + "') and fm.CollegeCode in ('" + college_cd + "') and fm.Batch_Year in ('" + Batch_Year + "') and fm.FeedBackName='" + ddl_Feedbackname.SelectedItem.Value + "' and fm.semester in ('" + semester + "') and  ISNULL(app_no,0) <>0 ";
                    if (section != "")
                    {
                        selectquestion = selectquestion + " and fm.Section in ('" + section + "') and fm.FeedBackType='1' group by  cs.QuestionMasterFK, SubjectNo";
                    }
                    else
                    {
                        selectquestion = selectquestion + " and fm.FeedBackType='1' group by  cs.QuestionMasterFK,SubjectNo";
                    }
                    DataView dv = new DataView();
                    ds1 = d2.select_method_wo_parameter(selectquestion, "Text");
                    if (ds1.Tables.Count > 0)
                    {
                        if (ds1.Tables[0].Rows.Count > 0)
                        {
                            for (int i = 0; i < ds1.Tables[0].Rows.Count; i++)
                            {
                                FpSpread2.Sheets[0].Rows.Count++;
                                if (((i + 1) % 2) == 0)
                                {
                                    FpSpread2.Sheets[0].Rows[FpSpread2.Sheets[0].RowCount - 1].BackColor = System.Drawing.Color.LightSeaGreen;
                                }
                                FpSpread2.Sheets[0].Cells[i, 0].Text = Convert.ToString(i + 1);
                                FpSpread2.Sheets[0].Cells[i, 1].Text = ds1.Tables[0].Rows[i]["HeaderName"].ToString();
                                FpSpread2.Sheets[0].Cells[i, 1].Tag = ds1.Tables[0].Rows[i]["HeaderCode"].ToString();
                                FpSpread2.Sheets[0].Cells[i, 2].Text = ds1.Tables[0].Rows[i]["Question"].ToString();
                                FpSpread2.Sheets[0].Columns[2].HorizontalAlign = HorizontalAlign.Left;
                                FpSpread2.Sheets[0].Cells[i, 2].Tag = ds1.Tables[0].Rows[i]["QuestionMasterPK"].ToString();
                                int total = 0;
                                int streng = 0;
                                string needs = "5";
                                string sum_total = d2.GetFunction("select top 1 Point from CO_MarkMaster  where CollegeCode in('" + collegecode1 + "') order by Point desc");
                                if (cb_total.Checked == true && cb_avg.Checked == true)
                                {
                                    for (int col = 3; col < FpSpread2.Sheets[0].ColumnCount - 1; col += 2)
                                    {
                                        string subjectno = Convert.ToString(FpSpread2.Sheets[0].ColumnHeader.Cells[0, col].Tag);
                                        if (subjectno.Trim() != "")
                                        {
                                            ds1.Tables[1].DefaultView.RowFilter = "QuestionMasterFK='" + Convert.ToString(ds1.Tables[0].Rows[i]["QuestionMasterPK"]) + "' and SubjectNo='" + subjectno + "'";
                                            dv = ds1.Tables[1].DefaultView;
                                            if (dv.Count > 0)
                                            {
                                                FpSpread2.Sheets[0].Cells[i, col].Text = Convert.ToString(dv[0]["total"]);
                                                FpSpread2.Sheets[0].Cells[i, col].HorizontalAlign = HorizontalAlign.Center;
                                                string points = "";
                                                double tot = Convert.ToDouble(dv[0]["total"]);
                                                double strengt = Convert.ToDouble(dv[0]["Strength"]);
                                                Double bas_avg = 0;
                                                bas_avg = tot / strengt;
                                                //bas_avg = Math.Round(bas_avg, 0, MidpointRounding.AwayFromZero);
                                                points = Convert.ToString(bas_avg);
                                                ConvertedMark(needs, sum_total, ref points);
                                                string aaa = Convert.ToString(points);
                                                FpSpread2.Sheets[0].Cells[i, col + 1].Text = aaa;
                                                FpSpread2.Sheets[0].Cells[i, col + 1].HorizontalAlign = HorizontalAlign.Center;
                                                if (Convert.ToString(dv[0]["total"]).Trim() != "")
                                                {
                                                    total = total + Convert.ToInt32(dv[0]["total"]);
                                                    streng = streng + Convert.ToInt32(dv[0]["Strength"]);
                                                    FpSpread2.Sheets[0].Columns[col].HorizontalAlign = HorizontalAlign.Center;
                                                }
                                            }
                                        }
                                    }
                                    FpSpread2.Sheets[0].Cells[i, FpSpread2.Sheets[0].ColumnCount - 2].Text = Convert.ToString(total);
                                    FpSpread2.Sheets[0].Cells[i, FpSpread2.Sheets[0].ColumnCount - 2].HorizontalAlign = HorizontalAlign.Center;
                                    needs = "5";
                                    //sum_total = "9";
                                    sum_total = d2.GetFunction("select top 1 Point from CO_MarkMaster  where CollegeCode in('" + collegecode1 + "') order by Point desc");
                                    Double point = total / streng;
                                    // point = Math.Round(point, 0, MidpointRounding.AwayFromZero);
                                    string points1 = "";
                                    points1 = Convert.ToString(point);
                                    ConvertedMark(needs, sum_total, ref points1);
                                    string grandgrd = Convert.ToString(points1);
                                    FpSpread2.Sheets[0].Cells[i, FpSpread2.Sheets[0].ColumnCount - 1].Text = Convert.ToString(grandgrd);
                                    FpSpread2.Sheets[0].Cells[i, FpSpread2.Sheets[0].ColumnCount - 1].HorizontalAlign = HorizontalAlign.Center;
                                }
                                else if (cb_total.Checked == true || cb_avg.Checked == true)
                                {
                                    for (int col = 3; col < FpSpread2.Sheets[0].ColumnCount - 1; col++)
                                    {
                                        string subjectno = Convert.ToString(FpSpread2.Sheets[0].ColumnHeader.Cells[0, col].Tag);
                                        if (subjectno.Trim() != "")
                                        {
                                            ds1.Tables[1].DefaultView.RowFilter = "QuestionMasterFK='" + Convert.ToString(ds1.Tables[0].Rows[i]["QuestionMasterPK"]) + "' and SubjectNo='" + subjectno + "'";
                                            dv = ds1.Tables[1].DefaultView;
                                            if (dv.Count > 0)
                                            {
                                                if (cb_total.Checked == true)
                                                {
                                                    FpSpread2.Sheets[0].Cells[i, col].Text = Convert.ToString(dv[0]["total"]);
                                                    FpSpread2.Sheets[0].Cells[i, col].HorizontalAlign = HorizontalAlign.Center;
                                                }
                                                Int32 tot = Convert.ToInt32(dv[0]["total"]);
                                                int strengt = Convert.ToInt32(dv[0]["Strength"]);
                                                Double bas_avg = tot / strengt;
                                                // bas_avg = Math.Round(bas_avg, 0, MidpointRounding.AwayFromZero);
                                                string points = Convert.ToString(bas_avg);
                                                ConvertedMark(needs, sum_total, ref points);
                                                string aaa = Convert.ToString(points);
                                                if (cb_avg.Checked == true)
                                                {
                                                    FpSpread2.Sheets[0].Cells[i, col].Text = aaa;
                                                    FpSpread2.Sheets[0].Cells[i, col].HorizontalAlign = HorizontalAlign.Center;
                                                }
                                                if (Convert.ToString(dv[0]["total"]).Trim() != "")
                                                {
                                                    total = total + Convert.ToInt32(dv[0]["total"]);
                                                    streng = streng + Convert.ToInt32(dv[0]["Strength"]);
                                                    FpSpread2.Sheets[0].Columns[col].HorizontalAlign = HorizontalAlign.Center;
                                                }
                                            }
                                        }
                                    }
                                    FpSpread2.Sheets[0].Cells[i, FpSpread2.Sheets[0].ColumnCount - 2].Text = Convert.ToString(total);
                                    FpSpread2.Sheets[0].Cells[i, FpSpread2.Sheets[0].ColumnCount - 2].HorizontalAlign = HorizontalAlign.Center;
                                    needs = "5";
                                    sum_total = d2.GetFunction("select top 1 Point from CO_MarkMaster  where CollegeCode in('" + collegecode1 + "') order by Point desc");
                                    // sum_total = "9";
                                    Double point = total / streng;
                                    //point = Math.Round(point, 0, MidpointRounding.AwayFromZero);
                                    string points1 = Convert.ToString(point);
                                    ConvertedMark(needs, sum_total, ref points1);
                                    string grandgrd = Convert.ToString(points1);
                                    FpSpread2.Sheets[0].Cells[i, FpSpread2.Sheets[0].ColumnCount - 1].Text = Convert.ToString(grandgrd);
                                    FpSpread2.Sheets[0].Cells[i, FpSpread2.Sheets[0].ColumnCount - 1].HorizontalAlign = HorizontalAlign.Center;
                                    //FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, FpSpread2.Sheets[0].ColumnCount].Text = ds.Tables[0].Rows[i]["Points"].ToString();
                                }
                            }
                            FpSpread2.Sheets[0].Rows.Count++;
                            if (cb_total.Checked == true && cb_avg.Checked == true)
                            {
                                for (int j = 3; j < FpSpread2.Sheets[0].ColumnCount; j += 2)
                                {
                                    int grandtot = 0;
                                    Double grandavg = 0;
                                    for (int i = 0; i < FpSpread2.Sheets[0].Rows.Count; i++)
                                    {
                                        int total = Convert.ToInt32(FpSpread2.Sheets[0].Cells[i, j].Value);
                                        grandtot = grandtot + total;
                                        Double totalav = Convert.ToDouble(FpSpread2.Sheets[0].Cells[i, j + 1].Value);
                                        grandavg = grandavg + totalav;
                                    }
                                    Int32 coun = Convert.ToInt32(FpSpread2.Sheets[0].Rows.Count - 1);
                                    grandavg = grandavg / coun;
                                    grandavg = (Math.Round(grandavg, 2));
                                    if (grandtot == 0)
                                    {
                                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].Rows.Count - 1, j].Text = "";
                                    }
                                    else
                                    {
                                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].Rows.Count - 1, j].Text = Convert.ToString(grandtot);
                                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].Rows.Count - 1, j].HorizontalAlign = HorizontalAlign.Center;
                                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].Rows.Count - 1, j + 1].Text = Convert.ToString(grandavg);
                                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].Rows.Count - 1, j + 1].HorizontalAlign = HorizontalAlign.Center;
                                    }
                                }
                            }
                            else if (cb_total.Checked == true || cb_avg.Checked == true)
                            {
                                for (int j = 3; j < FpSpread2.Sheets[0].ColumnCount; j++)
                                {
                                    int grandtot = 0;
                                    Double grandavg = 0;
                                    Double grandt = 0;
                                    for (int i = 0; i < FpSpread2.Sheets[0].Rows.Count; i++)
                                    {
                                        if (cb_total.Checked == true)
                                        {
                                            int total = Convert.ToInt32(FpSpread2.Sheets[0].Cells[i, j].Value);
                                            grandtot = grandtot + total;
                                        }
                                        if (cb_avg.Checked == true)
                                        {
                                            Double totalav = Convert.ToDouble(FpSpread2.Sheets[0].Cells[i, j].Value);
                                            grandavg = grandavg + totalav;
                                        }
                                    }
                                    Int32 coun = Convert.ToInt32(FpSpread2.Sheets[0].Rows.Count - 1);
                                    grandt = grandavg;
                                    grandavg = grandavg / coun;
                                    grandavg = (Math.Round(grandavg, 2));
                                    if (cb_total.Checked == true)
                                    {
                                        if (grandtot == 0)
                                        {
                                            FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].Rows.Count - 1, j].Text = "-";
                                        }
                                        else
                                        {
                                            FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].Rows.Count - 1, j].Text = Convert.ToString(grandtot);
                                            FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].Rows.Count - 1, j].HorizontalAlign = HorizontalAlign.Center;
                                        }
                                    }
                                    if (cb_avg.Checked == true)
                                    {
                                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].Rows.Count - 1, j].Text = Convert.ToString(grandavg);
                                        if (j == FpSpread2.Sheets[0].ColumnCount - 2)
                                        {
                                            FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].Rows.Count - 1, j].Text = Convert.ToString(grandt);
                                        }
                                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].Rows.Count - 1, j].HorizontalAlign = HorizontalAlign.Center;
                                    }
                                }
                            }
                            FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].Rows.Count - 1, FpSpread2.Sheets[0].ColumnCount - 1].HorizontalAlign = HorizontalAlign.Center;
                            FpSpread2.Sheets[0].SpanModel.Add(FpSpread2.Sheets[0].RowCount - 1, 0, 1, 3);
                            FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].Rows.Count - 1, 0].Text = "Grand Total";
                            FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].Rows.Count - 1, 0].HorizontalAlign = HorizontalAlign.Right;
                            FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].Rows.Count - 1, 0].ForeColor = System.Drawing.Color.Blue;
                            FpSpread2.Sheets[0].RowCount++;
                            FpSpread2.Sheets[0].RowCount++;
                            if (ds.Tables[0].Rows.Count > 0)
                            {
                                FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].Rows.Count - 1, 0].Text = "S.No";
                                FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].Rows.Count - 1, 0].BackColor = System.Drawing.Color.BlanchedAlmond;
                                FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].Rows.Count - 1, 0].HorizontalAlign = HorizontalAlign.Center;
                                FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].Rows.Count - 1, 1].Text = Convert.ToString(ddl_headershow.SelectedItem.Text);// "Subject Code / Subject Name";
                                FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].Rows.Count - 1, 1].BackColor = System.Drawing.Color.BlanchedAlmond;
                                FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].Rows.Count - 1, 1].HorizontalAlign = HorizontalAlign.Center;
                                FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].Rows.Count - 1, 2].Text = "Faculty";
                                FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].Rows.Count - 1, 2].BackColor = System.Drawing.Color.BlanchedAlmond;
                                FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].Rows.Count - 1, 2].HorizontalAlign = HorizontalAlign.Center;
                                FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].Rows.Count - 1, 0].Font.Bold = true;
                                FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].Rows.Count - 1, 0].Font.Name = "Book Antiqua";
                                FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].Rows.Count - 1, 0].Font.Size = FontUnit.Medium;
                                FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].Rows.Count - 1, 1].Font.Bold = true;
                                FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].Rows.Count - 1, 1].Font.Name = "Book Antiqua";
                                FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].Rows.Count - 1, 1].Font.Size = FontUnit.Medium;
                                FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].Rows.Count - 1, 2].Font.Bold = true;
                                FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].Rows.Count - 1, 2].Font.Name = "Book Antiqua";
                                FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].Rows.Count - 1, 2].Font.Size = FontUnit.Medium;
                                DataView dv1 = new DataView();
                                for (int m = 0; m < ds.Tables[0].Rows.Count; m++)
                                {
                                    string sname = "";
                                    FpSpread2.Sheets[0].RowCount++;
                                    FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].Rows.Count - 1, 0].Text = (m + 1).ToString();
                                    FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].Rows.Count - 1, 1].Text = Convert.ToString(ds.Tables[0].Rows[m]["codeandname"]);
                                    string subjectno = ds.Tables[0].Rows[m]["subject_no"].ToString();
                                    ds.Tables[1].DefaultView.RowFilter = " subject_no='" + subjectno + "' ";
                                    dv1 = ds.Tables[1].DefaultView;
                                    bool colorcell = false;
                                    if (dv1.Count > 0)
                                    {
                                        for (int k = 0; k < dv1.Count; k++)
                                        {
                                            if (sname.Trim() == "")
                                            {
                                                sname = Convert.ToString(dv1[k]["appl_name"]);
                                            }
                                            else
                                            {
                                                sname = sname + ", " + Convert.ToString(dv1[k]["appl_name"]); colorcell = true;
                                            }
                                        }
                                    }
                                    if (colorcell == true)
                                    {
                                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].Rows.Count - 1, 2].Text = sname;
                                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].Rows.Count - 1, 2].BackColor = System.Drawing.Color.BlanchedAlmond;
                                    }
                                    else
                                    {
                                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].Rows.Count - 1, 2].Text = sname;
                                    }
                                    FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].Rows.Count - 1, 0].Font.Name = "Book Antiqua";
                                    FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].Rows.Count - 1, 0].Font.Size = FontUnit.Medium;
                                    FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].Rows.Count - 1, 1].Font.Name = "Book Antiqua";
                                    FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].Rows.Count - 1, 1].Font.Size = FontUnit.Medium;
                                    FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].Rows.Count - 1, 2].Font.Name = "Book Antiqua";
                                    FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].Rows.Count - 1, 2].Font.Size = FontUnit.Medium;
                                }
                            }
                            // FpSpread2.Sheets[0].PageSize = ds.Tables[0].Rows.Count;
                        }
                        else
                        {
                            // div1.Visible = false;
                            imgdiv2.Visible = true;
                            lbl_alert1.Text = "No Records Found";
                            FpSpread2.Visible = false;
                            rptprint1.Visible = false;
                        }
                    }
                    else
                    {
                        // div1.Visible = false;
                        imgdiv2.Visible = true;
                        lbl_alert1.Text = "No Records Found";
                        FpSpread2.Visible = false;
                        rptprint1.Visible = false;
                    }
                }
                else
                {
                    // div1.Visible = false;
                    imgdiv2.Visible = true;
                    lbl_alert1.Text = "No Records Found";
                    FpSpread2.Visible = false;
                    rptprint1.Visible = false;
                }
            }
            else
            {
                //div1.Visible = false;
                imgdiv2.Visible = true;
                lbl_alert1.Text = "No Records Found";
                FpSpread2.Visible = false;
                rptprint1.Visible = false;
            }
            FpSpread2.Sheets[0].PageSize = FpSpread2.Sheets[0].RowCount;
            FpSpread2.Sheets[0].Columns[2].Width = 550;
            FpSpread2.Sheets[0].Columns[2].HorizontalAlign = HorizontalAlign.Left;
            //FpSpread2.Width = 800;
            //FpSpread2.Height = 500;
        }
        catch (Exception ex)
        {
            d2.sendErrorMail(ex, collegecode1, "FeedbackReport");
        }
    }

    public void format3()
    {
        try
        {
            chartprint.Visible = false;
            chartfalse();
            fair();
            DataView dv = new DataView();
            FpSpread3.Visible = true;
            rptprint1.Visible = true;
            lbl_headig.Visible = true;
            string college_cd = "";
            if (Cbl_college.Items.Count > 0)
            {
                for (int i = 0; i < Cbl_college.Items.Count; i++)
                {
                    if (Cbl_college.Items[i].Selected == true)
                    {
                        if (college_cd == "")
                        {
                            college_cd = "" + Cbl_college.Items[i].Value.ToString() + "";
                        }
                        else
                        {
                            college_cd = college_cd + "','" + Convert.ToString(Cbl_college.Items[i].Value);
                        }
                    }
                }
            }
            string Batch_Year = "";
            for (int i = 0; i < cbl_batch.Items.Count; i++)
            {
                if (cbl_batch.Items[i].Selected == true)
                {
                    if (Batch_Year == "")
                    {
                        Batch_Year = "" + cbl_batch.Items[i].Value.ToString() + "";
                    }
                    else
                    {
                        Batch_Year = Batch_Year + "','" + cbl_batch.Items[i].Value.ToString() + "";
                    }
                }
            }
            string degree_code = "";
            for (int i = 0; i < cbl_branch.Items.Count; i++)
            {
                if (cbl_branch.Items[i].Selected == true)
                {
                    if (degree_code == "")
                    {
                        degree_code = "" + cbl_branch.Items[i].Value.ToString() + "";
                    }
                    else
                    {
                        degree_code = degree_code + "','" + cbl_branch.Items[i].Value.ToString() + "";
                    }
                }
            }
            string section = "";
            for (int i = 0; i < cbl_sec.Items.Count; i++)
            {
                if (cbl_sec.Items[i].Selected == true)
                {
                    if (section == "")
                    {
                        section = "" + cbl_sec.Items[i].Value.ToString() + "";
                    }
                    else
                    {
                        section = section + "','" + cbl_sec.Items[i].Value.ToString() + "";
                    }
                    if (cbl_sec.Items[i].Value == "Empty")
                    {
                        section = "";
                    }
                }
            }
            if (section.Trim() != "")
            {
                section = section + "','";
            }
            string semester = "";
            for (int i = 0; i < cbl_sem.Items.Count; i++)
            {
                if (cbl_sem.Items[i].Selected == true)
                {
                    if (semester == "")
                    {
                        semester = "" + cbl_sem.Items[i].Value.ToString() + "";
                    }
                    else
                    {
                        semester = semester + "','" + cbl_sem.Items[i].Value.ToString() + "";
                    }
                }
            }
            //string type = "";
            //if (rb_Acad1.Checked == true)
            //{
            //    type = "1";
            //}
            //else if (rb_Gend1.Checked == true)
            //{
            //    type = "2";
            //}
            FpSpread3.Sheets[0].RowCount = 0;
            FpSpread3.Sheets[0].ColumnCount = 0;
            FpSpread3.CommandBar.Visible = false;
            FpSpread3.Sheets[0].AutoPostBack = true;
            FpSpread3.Sheets[0].ColumnHeader.RowCount = 1;
            FpSpread3.Sheets[0].RowHeader.Visible = false;
            FpSpread3.Sheets[0].ColumnCount = 9;
            FarPoint.Web.Spread.StyleInfo darkstyle = new FarPoint.Web.Spread.StyleInfo();
            darkstyle.BackColor = ColorTranslator.FromHtml("#0CA6CA");
            darkstyle.ForeColor = System.Drawing.Color.White;
            FpSpread3.ActiveSheetView.ColumnHeader.DefaultStyle = darkstyle;
            FpSpread3.Visible = true;
            //FpSpread3.Width = 971;
            //FpSpread3.Height = 500;
            FpSpread3.SaveChanges();
            FpSpread3.Sheets[0].ColumnHeader.Cells[0, 0].Text = "S.No";
            FpSpread3.Sheets[0].ColumnHeader.Cells[0, 0].Font.Bold = true;
            FpSpread3.Sheets[0].ColumnHeader.Cells[0, 0].HorizontalAlign = HorizontalAlign.Center;
            FpSpread3.Sheets[0].ColumnHeader.Cells[0, 0].Font.Name = "Book Antiqua";
            FpSpread3.Sheets[0].ColumnHeader.Cells[0, 0].Font.Size = FontUnit.Medium;
            FpSpread3.Sheets[0].Columns[0].HorizontalAlign = HorizontalAlign.Center;
            FpSpread3.Sheets[0].ColumnHeader.Cells[0, 1].Text = "BranchName";
            FpSpread3.Sheets[0].ColumnHeader.Cells[0, 1].Font.Bold = true;
            FpSpread3.Sheets[0].ColumnHeader.Cells[0, 1].Font.Name = "Book Antiqua";
            FpSpread3.Sheets[0].ColumnHeader.Cells[0, 1].Font.Size = FontUnit.Medium;
            FpSpread3.Sheets[0].ColumnHeader.Cells[0, 1].HorizontalAlign = HorizontalAlign.Center;
            FpSpread3.Sheets[0].ColumnHeader.Cells[0, 2].Text = "Semester ";
            FpSpread3.Sheets[0].ColumnHeader.Cells[0, 2].Font.Bold = true;
            FpSpread3.Sheets[0].ColumnHeader.Cells[0, 2].Font.Name = "Book Antiqua";
            FpSpread3.Sheets[0].ColumnHeader.Cells[0, 2].Font.Size = FontUnit.Medium;
            FpSpread3.Sheets[0].ColumnHeader.Cells[0, 2].HorizontalAlign = HorizontalAlign.Center;
            FpSpread3.Sheets[0].ColumnHeader.Cells[0, 3].Text = "SectionName ";
            FpSpread3.Sheets[0].ColumnHeader.Cells[0, 3].Font.Bold = true;
            FpSpread3.Sheets[0].ColumnHeader.Cells[0, 3].Font.Name = "Book Antiqua";
            FpSpread3.Sheets[0].ColumnHeader.Cells[0, 3].Font.Size = FontUnit.Medium;
            FpSpread3.Sheets[0].ColumnHeader.Cells[0, 3].HorizontalAlign = HorizontalAlign.Center;
            FpSpread3.Sheets[0].ColumnHeader.Cells[0, 4].Text = "FromYear";
            FpSpread3.Sheets[0].ColumnHeader.Cells[0, 4].Font.Bold = true;
            FpSpread3.Sheets[0].ColumnHeader.Cells[0, 4].Font.Name = "Book Antiqua";
            FpSpread3.Sheets[0].ColumnHeader.Cells[0, 4].Font.Size = FontUnit.Medium;
            FpSpread3.Sheets[0].ColumnHeader.Cells[0, 4].HorizontalAlign = HorizontalAlign.Center;
            FpSpread3.Sheets[0].Columns[4].HorizontalAlign = HorizontalAlign.Center;
            FpSpread3.Sheets[0].ColumnHeader.Cells[0, 5].Text = "ToYear";
            FpSpread3.Sheets[0].ColumnHeader.Cells[0, 5].Font.Bold = true;
            FpSpread3.Sheets[0].ColumnHeader.Cells[0, 5].Font.Name = "Book Antiqua";
            FpSpread3.Sheets[0].ColumnHeader.Cells[0, 5].Font.Size = FontUnit.Medium;
            FpSpread3.Sheets[0].ColumnHeader.Cells[0, 5].HorizontalAlign = HorizontalAlign.Center;
            FpSpread3.Sheets[0].ColumnHeader.Cells[0, 6].Text = "Total Strength";
            FpSpread3.Sheets[0].ColumnHeader.Cells[0, 6].Font.Bold = true;
            FpSpread3.Sheets[0].ColumnHeader.Cells[0, 6].Font.Name = "Book Antiqua";
            FpSpread3.Sheets[0].ColumnHeader.Cells[0, 6].Font.Size = FontUnit.Medium;
            FpSpread3.Sheets[0].ColumnHeader.Cells[0, 6].HorizontalAlign = HorizontalAlign.Center;
            FpSpread3.Sheets[0].ColumnHeader.Cells[0, 7].Text = "Total Attended";
            FpSpread3.Sheets[0].ColumnHeader.Cells[0, 7].Font.Bold = true;
            FpSpread3.Sheets[0].ColumnHeader.Cells[0, 7].Font.Name = "Book Antiqua";
            FpSpread3.Sheets[0].ColumnHeader.Cells[0, 7].Font.Size = FontUnit.Medium;
            FpSpread3.Sheets[0].ColumnHeader.Cells[0, 7].HorizontalAlign = HorizontalAlign.Center;
            FpSpread3.Sheets[0].ColumnHeader.Cells[0, 8].Text = "Total Remaining";
            FpSpread3.Sheets[0].ColumnHeader.Cells[0, 8].Font.Bold = true;
            FpSpread3.Sheets[0].ColumnHeader.Cells[0, 8].Font.Name = "Book Antiqua";
            FpSpread3.Sheets[0].ColumnHeader.Cells[0, 8].Font.Size = FontUnit.Medium;
            FpSpread3.Sheets[0].ColumnHeader.Cells[0, 8].HorizontalAlign = HorizontalAlign.Center;
            FpSpread3.Sheets[0].ColumnHeader.Columns[0].Width = 59;
            ds.Clear();
            int rmaining = 0;
            string selqry = "";
            selqry = "SELECT Course_Name+'-'+Dept_Name Degree, Current_semester, Sections, R.degree_code,Batch_Year,COUNT(distinct f.app_no)as Strength FROM CO_StudFeedBack F,Registration R,Degree G,Course C,Department D WHERE F.App_No = R.App_No  AND R.degree_code = G.Degree_Code AND G.Course_Id = C.Course_Id  AND G.college_code = C.college_code   AND g.Dept_Code = d.Dept_Code and g.college_code = d.college_code AND CC = 0 AND DelFlag = 0 AND Exam_Flag = 'OK'   and r.degree_code in ('" + degree_code + "')   and r.Batch_Year in ('" + Batch_Year + "') and r.Current_Semester in ('" + semester + "')";
            if (section != "")
            {
                selqry = selqry + " and r.Sections in ('" + section + "') GROUP BY Course_Name,R.degree_code, Dept_Name, Current_semester, Sections,Batch_Year ORDER  BY R.degree_code,Current_Semester,Sections , Batch_Year,Course_Name,Dept_Name  ";
            }
            else
            {
                selqry = selqry + "  GROUP BY R.degree_code,Course_Name, Dept_Name, Current_semester, Sections,Batch_Year ORDER BY R.degree_code,Current_Semester,Sections , Batch_Year,Course_Name,Dept_Name";
            }
            selqry = selqry + " SELECT Course_Name+'-'+Dept_Name Degree,Current_semester,R.degree_code,Sections,Batch_Year,COUNT(*)as TotStrengh FROM Registration R,Degree G,Course C,Department D WHERE R.degree_code = G.Degree_Code AND G.Course_Id = C.Course_Id AND G.college_code = C.college_code AND g.Dept_Code = d.Dept_Code and g.college_code = d.college_code AND CC = 0 AND DelFlag = 0 AND Exam_Flag = 'OK' and r.degree_code in ('" + degree_code + "') and r.Batch_Year in('" + Batch_Year + "') and r.Current_Semester in ('" + semester + "')";
            if (section != "")
            {
                selqry = selqry + " and r.Sections in ('" + section + "') GROUP BY Course_Name,R.degree_code, Dept_Name,Current_semester,Sections, Batch_Year ORDER BY R.degree_code,Current_Semester,Sections , Batch_Year,Course_Name,Dept_Name ";
            }
            else
            {
                selqry = selqry + " GROUP BY Course_Name, R.degree_code, Dept_Name,Current_semester,Sections,Batch_Year ORDER BY R.degree_code,Current_Semester,Sections , Batch_Year,Course_Name,Dept_Name";
            }
            ds = d2.select_method_wo_parameter(selqry, "Text");
            DataView dvnew = new DataView();
            if (ds.Tables.Count > 0)
            {
                if (ds.Tables[0].Rows.Count > 0 && ds.Tables[1].Rows.Count > 0)
                {
                    // FpSpread3.Sheets[0].Rows.Count = ds.Tables[0].Rows.Count;
                    for (int i = 0; i < ds.Tables[1].Rows.Count; i++)
                    {
                        FpSpread3.Sheets[0].Rows.Count++;
                        FpSpread3.Sheets[0].Cells[FpSpread3.Sheets[0].RowCount - 1, 0].Text = Convert.ToString(i + 1);
                        if (((i + 1) % 2) == 0)
                        {
                            FpSpread3.Sheets[0].Rows[FpSpread3.Sheets[0].RowCount - 1].BackColor = System.Drawing.Color.LightSeaGreen;
                        }
                        FpSpread3.Sheets[0].Cells[FpSpread3.Sheets[0].RowCount - 1, 1].Text = ds.Tables[1].Rows[i]["Degree"].ToString();
                        FpSpread3.Sheets[0].Cells[FpSpread3.Sheets[0].RowCount - 1, 1].Tag = ds.Tables[1].Rows[i]["degree_code"].ToString();
                        FpSpread3.Sheets[0].Cells[FpSpread3.Sheets[0].RowCount - 1, 2].Text = ds.Tables[1].Rows[i]["Current_semester"].ToString();
                        FpSpread3.Sheets[0].Cells[FpSpread3.Sheets[0].RowCount - 1, 3].Text = ds.Tables[1].Rows[i]["Sections"].ToString();
                        FpSpread3.Sheets[0].Cells[FpSpread3.Sheets[0].RowCount - 1, 4].Text = ds.Tables[1].Rows[i]["Batch_Year"].ToString();
                        FpSpread3.Sheets[0].Cells[FpSpread3.Sheets[0].RowCount - 1, 5].Text = ds.Tables[1].Rows[i]["Batch_Year"].ToString();
                        FpSpread3.Sheets[0].Cells[FpSpread3.Sheets[0].RowCount - 1, 6].Text = ds.Tables[1].Rows[i]["TotStrengh"].ToString();
                        string sectons = Convert.ToString(ds.Tables[1].Rows[i]["Sections"]);
                        string degrecode = Convert.ToString(ds.Tables[1].Rows[i]["degree_code"]);
                        string sem = Convert.ToString(ds.Tables[1].Rows[i]["Current_semester"]);
                        string filterquery = "";
                        int remaining = 0;
                        int attand = 0;
                        filterquery = "degree_code='" + degrecode + "'  and  Current_semester='" + sem + "' ";
                        if (sectons.Trim() != "")
                        {
                            filterquery = filterquery + " and Sections='" + sectons + "'";
                        }
                        ds.Tables[0].DefaultView.RowFilter = "" + filterquery + "";
                        dvnew = ds.Tables[0].DefaultView;
                        if (dvnew.Count > 0)
                        {
                            FpSpread3.Sheets[0].Cells[FpSpread3.Sheets[0].RowCount - 1, 7].Text = Convert.ToString(dvnew[0]["Strength"]);
                            attand = Convert.ToInt32(dvnew[0]["Strength"]);
                        }
                        int total = 0;
                        total = Convert.ToInt32(ds.Tables[1].Rows[i]["TotStrengh"]);
                        remaining = total - attand;
                        FpSpread3.Sheets[0].Cells[FpSpread3.Sheets[0].RowCount - 1, 8].Text = Convert.ToString(remaining);
                    }
                    //for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    //{
                    //    FpSpread3.Sheets[0].Cells[FpSpread3.Sheets[0].RowCount - 1, 7].Text = ds.Tables[0].Rows[i]["Strength"].ToString();
                    //}
                    FpSpread3.Sheets[0].Columns[2].HorizontalAlign = HorizontalAlign.Center;
                    FpSpread3.Sheets[0].Columns[5].HorizontalAlign = HorizontalAlign.Center;
                    FpSpread3.Sheets[0].Columns[6].HorizontalAlign = HorizontalAlign.Center;
                    FpSpread3.Sheets[0].Columns[7].HorizontalAlign = HorizontalAlign.Center;
                    FpSpread3.Sheets[0].Columns[8].HorizontalAlign = HorizontalAlign.Center;
                    FpSpread3.Sheets[0].Columns[3].HorizontalAlign = HorizontalAlign.Center;
                    FpSpread3.Sheets[0].PageSize = ds.Tables[1].Rows.Count;
                    //FpSpread3.Width = 800;
                    //FpSpread3.Height = 500;
                }
                else
                {
                    //div1.Visible = false;
                    imgdiv2.Visible = true;
                    lbl_alert1.Text = "No Records Found";
                    FpSpread3.Visible = false;
                    rptprint1.Visible = false;
                }
            }
            else
            {
                // div1.Visible = false;
                imgdiv2.Visible = true;
                lbl_alert1.Text = "No Records Found";
                FpSpread3.Visible = false;
                rptprint1.Visible = false;
            }
            FpSpread3.Sheets[0].PageSize = FpSpread3.Sheets[0].RowCount;
        }
        catch (Exception ex)
        {
            d2.sendErrorMail(ex, collegecode1, "FeedbackReport");
        }
    }

    public void format4()
    {
        try
        {
            if (rb_Acad.Checked == true)
            {
                rb_linchart.Visible = true;
                rb_barchart.Visible = true;
                staff_chart.Visible = true;
                fair();
                rptprint1.Visible = false;
                string college_cd = "";
                if (Cbl_college.Items.Count > 0)
                {
                    for (int i = 0; i < Cbl_college.Items.Count; i++)
                    {
                        if (Cbl_college.Items[i].Selected == true)
                        {
                            if (college_cd == "")
                            {
                                college_cd = "" + Cbl_college.Items[i].Value.ToString() + "";
                            }
                            else
                            {
                                college_cd = college_cd + "','" + Convert.ToString(Cbl_college.Items[i].Value);
                            }
                        }
                    }
                }
                string Batch_Year = "";
                for (int i = 0; i < cbl_batch.Items.Count; i++)
                {
                    if (cbl_batch.Items[i].Selected == true)
                    {
                        if (Batch_Year == "")
                        {
                            Batch_Year = "" + cbl_batch.Items[i].Value.ToString() + "";
                        }
                        else
                        {
                            Batch_Year = Batch_Year + "','" + cbl_batch.Items[i].Value.ToString() + "";
                        }
                    }
                }
                string degree_code = "";
                for (int i = 0; i < cbl_branch.Items.Count; i++)
                {
                    if (cbl_branch.Items[i].Selected == true)
                    {
                        if (degree_code == "")
                        {
                            degree_code = "" + cbl_branch.Items[i].Value.ToString() + "";
                        }
                        else
                        {
                            degree_code = degree_code + "','" + cbl_branch.Items[i].Value.ToString() + "";
                        }
                    }
                }
                string semester = "";
                for (int i = 0; i < cbl_sem.Items.Count; i++)
                {
                    if (cbl_sem.Items[i].Selected == true)
                    {
                        if (semester == "")
                        {
                            semester = "" + cbl_sem.Items[i].Value.ToString() + "";
                        }
                        else
                        {
                            semester = semester + "','" + cbl_sem.Items[i].Value.ToString() + "";
                        }
                    }
                }
                string staffcod = "";
                for (int i = 0; i < cbl_staffname.Items.Count; i++)
                {
                    if (cbl_staffname.Items[i].Selected == true)
                    {
                        if (staffcod == "")
                        {
                            staffcod = "" + cbl_staffname.Items[i].Value.ToString() + "";
                        }
                        else
                        {
                            staffcod = staffcod + "','" + cbl_staffname.Items[i].Value.ToString() + "";
                        }
                    }
                }
                string sub = "";
                for (int i = 0; i < Cbl_Subject.Items.Count; i++)
                {
                    if (Cbl_Subject.Items[i].Selected == true)
                    {
                        if (sub == "")
                        {
                            sub = "" + Cbl_Subject.Items[i].Value.ToString() + "";
                        }
                        else
                        {
                            sub = sub + "','" + Cbl_Subject.Items[i].Value.ToString() + "";
                        }
                    }
                }
                string section = "";
                for (int i = 0; i < cbl_sec.Items.Count; i++)
                {
                    if (cbl_sec.Items[i].Selected == true)
                    {
                        if (section == "")
                        {
                            section = "" + cbl_sec.Items[i].Value.ToString() + "";
                        }
                        else
                        {
                            section = section + "','" + cbl_sec.Items[i].Value.ToString() + "";
                        }
                        if (cbl_sec.Items[i].Value == "Empty")
                        {
                            section = "";
                        }
                    }
                }
                if (section.Trim() != "")
                {
                    section = section + "','";
                }
                dsfb.Clear();
                string fbpk = " select FeedBackMasterPK from CO_FeedBackMaster where FeedBackName ='" + ddl_Feedbackname.SelectedItem.Value + "' and DegreeCode in ('" + degree_code + "') and semester in ('" + semester + "') and Batch_Year in('" + Batch_Year + "') and section in ('" + section + "')";
                dsfb = d2.select_method_wo_parameter(fbpk, "Text");
                string feedbakpk = "";
                if (dsfb.Tables.Count > 0)
                {
                    if (dsfb.Tables[0].Rows.Count > 0)
                    {
                        for (int pk = 0; pk < dsfb.Tables[0].Rows.Count; pk++)
                        {
                            if (feedbakpk == "")
                            {
                                feedbakpk = dsfb.Tables[0].Rows[pk]["FeedBackMasterPK"].ToString();
                            }
                            else
                            {
                                feedbakpk = feedbakpk + "','" + dsfb.Tables[0].Rows[pk]["FeedBackMasterPK"].ToString() + "";
                            }
                        }
                    }
                }
                //and cf.FeedBackMasterPK in('" + feedbakpk + "')
                string type = "";
                if (rb_Acad.Checked == true)
                {
                    type = "1";
                }
                else if (rb_Gend.Checked == true)
                {
                    type = "2";
                }
                ds.Clear();
                string selqry = "";
                DataTable dtChart1 = new DataTable();
                DataColumn dc;
                if (rdb_form4questwise.Checked == true)
                {
                    selqry = "SELECT Staff_Name,s.subject_no,(Point)as Points,Question,t.staff_code FROM CO_StudFeedBack F,CO_MarkMaster M,Subject S,staff_appl_master A,staffmaster T,syllabus_master y,Degree d, Department dt,Course c,CO_FeedBackMaster FM,Registration r, CO_QuestionMaster Q WHERE Q.QuestionMasterPK =F.QuestionMasterFK and  F.MarkMasterPK = M.MarkMasterPK AND F.StaffApplNo = A.appl_id AND F.SubjectNo = S.subject_no AND A.appl_no = T.appl_no  and s.syll_code = y.syll_code and d.Degree_Code =y.degree_code and dt.Dept_Code =d.Dept_Code and c.Course_Id =d.Course_Id  and f.App_No = r.App_No and FM.CollegeCode in ('" + college_cd + "') and y.Batch_Year in ('" + Batch_Year + "') and y.degree_code in ('" + degree_code + "') and y.semester in ('" + semester + "')  and fm.FeedBackType =('" + type + "') and fm.FeedBackMasterPK in ('" + feedbakpk + "') and staff_code in('" + staffcod + "')and Subject_Code in ('" + sub + "')  ";
                    if (section != "")
                    {
                        selqry = selqry + " and r.Sections in ('" + section + "') and f.FeedBackMasterFK = fm.FeedBackMasterPK";
                    }
                    else
                    {
                        selqry = selqry + " and f.FeedBackMasterFK = fm.FeedBackMasterPK ";
                    }
                    ds = d2.select_method_wo_parameter(selqry, "Text");
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        ArrayList arrcol = new ArrayList();
                        for (int q = 0; q < cbl_question.Items.Count; q++)
                        {
                            if (cbl_question.Items[q].Selected == true)
                            {
                                string name = cbl_question.Items[q].Text.ToString();
                                if (!arrcol.Contains(name))
                                {
                                    dc = new DataColumn();
                                    dc.ColumnName = name;
                                    dtChart1.Columns.Add(dc);
                                    arrcol.Add(name);
                                }
                            }
                        }
                        staff_chart.Titles[0].Text = ("STAFF PERCENTAGE (" + Convert.ToString(ddl_Feedbackname.SelectedItem.Value) + ")");
                        DataRow drp;
                        staff_chart.Series.Clear();
                        string staffnam = "";
                        string subcode = "";
                        string staffandcode = "";
                        ArrayList stafArr = new ArrayList();
                        ArrayList ques = new ArrayList();
                        int s = 0;
                        for (int r = 0; r < ds.Tables[0].Rows.Count; r++)
                        {
                            staffnam = ds.Tables[0].Rows[r]["Staff_Name"].ToString();
                            subcode = ds.Tables[0].Rows[r]["subject_no"].ToString();
                            staffandcode = staffnam + "-" + subcode;
                            if (!stafArr.Contains(staffandcode))
                            {
                                ds.Tables[0].DefaultView.RowFilter = "Staff_Name='" + staffnam + "' and subject_no='" + subcode + "'";
                                DataView dv = new DataView();
                                DataTable dtques = new DataTable();
                                dv = ds.Tables[0].DefaultView;
                                dtques = dv.ToTable();
                                staff_chart.Series.Add(staffandcode + r.ToString());
                                stafArr.Add(staffandcode);
                                Double poin = 0;
                                Double points1 = 0;
                                Double ps = 0;
                                drp = dtChart1.NewRow();
                                for (int q = 0; q < cbl_question.Items.Count; q++)
                                {
                                    ps = 0;
                                    if (cbl_question.Items[q].Selected == true)
                                    {
                                        dtques.DefaultView.RowFilter = "Question='" + cbl_question.Items[q].Text.ToString() + "'";
                                        dv = dtques.DefaultView;
                                        int w = 0;
                                        for (int pt = 0; pt < dv.Count; pt++)
                                        {
                                            w++;
                                            points1 = Convert.ToDouble(dv[pt]["Points"]);
                                            string points = Convert.ToString(points1);
                                            string needs = "5";
                                            //string sum_total = "9";
                                            string sum_total = d2.GetFunction("select top 1 Point from CO_MarkMaster  where CollegeCode in('" + collegecode1 + "') order by Point desc");
                                            ConvertedMark(needs, sum_total, ref points);
                                            string aaa = Convert.ToString(points);
                                            poin = Convert.ToDouble(aaa);
                                            ps = ps + poin;
                                        }
                                        ps = ps / w;
                                        string ps1 = Convert.ToString(Math.Round(ps, 1));
                                        if (ddl_criter.SelectedItem.Value != "0")
                                        {
                                            if (ddl_criter.SelectedItem.Value == "1")
                                            {
                                                if (txtfrom_range.Text.Trim() != "" && txtto_range.Text.Trim() != "")
                                                {
                                                    if (Convert.ToDouble(txtfrom_range.Text.Trim()) <= Convert.ToDouble(ps1) && Convert.ToDouble(ps1) <= Convert.ToDouble(txtto_range.Text.Trim()))
                                                    {
                                                        drp[cbl_question.Items[q].Text.ToString()] = ps1;
                                                    }
                                                    else
                                                    {
                                                        drp[cbl_question.Items[q].Text.ToString()] = "NaN";
                                                    }
                                                }
                                                else
                                                {
                                                    staff_chart.Visible = false;
                                                    chart_staff_chart.Visible = false;
                                                    imgdiv2.Visible = true;
                                                    lbl_alert1.Text = "Please Enter Range";
                                                }
                                            }
                                            else
                                            {
                                                drp[cbl_question.Items[q].Text.ToString()] = ps1;
                                            }
                                        }
                                        if (ddl_criter.SelectedItem.Value == "0")
                                        {
                                            drp[cbl_question.Items[q].Text.ToString()] = ps1;
                                        }
                                    }
                                }
                                dtChart1.Rows.Add(drp);
                            }
                        }
                        staff_chart.RenderType = RenderType.ImageTag;
                        staff_chart.ImageType = ChartImageType.Png;
                        staff_chart.ImageStorageMode = ImageStorageMode.UseImageLocation;
                        staff_chart.ImageLocation = Path.Combine("~/college/", "feedbackchart");
                        if (dtChart1.Rows.Count > 0)
                        {
                            string srr = ""; Hashtable removedthash = new Hashtable(); string val = ""; double finalvalue = 0;
                            for (int cc = 0; cc < dtChart1.Columns.Count; cc++)
                            {
                                finalvalue = 0;
                                for (int rr = 0; rr < dtChart1.Rows.Count; rr++)
                                {
                                    srr = dtChart1.Columns[cc].ToString();
                                    val = dtChart1.Rows[rr][cc].ToString();
                                    if (val.Trim() == "" || val.Trim().ToUpper() == "NAN")
                                        val = "0";
                                    finalvalue = finalvalue + Convert.ToDouble(val);
                                }
                                if (finalvalue == 0)
                                {
                                    removedthash.Add(cc, srr);
                                }
                            }
                            if (removedthash.Count > 0)
                            {
                                foreach (DictionaryEntry col in removedthash)
                                {
                                    dtChart1.Columns.Remove(Convert.ToString(col.Value));
                                }
                            }
                            int chartwidth = 0;
                            if (dtChart1.Columns.Count > 0)
                            {
                                for (int r = 0; r < dtChart1.Rows.Count; r++)
                                {
                                    for (int c = 0; c < dtChart1.Columns.Count; c++)
                                    {
                                        staff_chart.Series[r].Points.AddXY(dtChart1.Columns[c].ToString(), dtChart1.Rows[r][c].ToString());
                                        // staff_chart.Series[r].Points.AddXY(dtChart1.Rows[r][c].ToString(), dtChart1.Columns[c].ToString());
                                        staff_chart.ChartAreas[0].AxisX.LineColor = System.Drawing.Color.Black;
                                        staff_chart.ChartAreas[0].AxisY.LineColor = System.Drawing.Color.Black;
                                        staff_chart.Series[r].IsValueShownAsLabel = true;
                                        staff_chart.Series[r].IsXValueIndexed = true;
                                        if (rb_linchart.Checked == true)
                                        {
                                            staff_chart.Series[r].ChartType = SeriesChartType.Line;
                                        }
                                        staff_chart.ChartAreas[0].AxisX.LabelStyle.Angle = -90;
                                        staff_chart.ChartAreas[0].AxisX.LabelStyle.Interval = 1;
                                        chartwidth += 15;
                                    }
                                }
                                if (chartwidth <= 1125)
                                    staff_chart.Width = chartwidth;
                                else
                                    staff_chart.Width = 1125;
                                chart_staff_chart.Visible = true;
                                chartprint.Visible = true;
                            }
                            else
                            {
                                staff_chart.Visible = false;
                                chart_staff_chart.Visible = false;
                                chartprint.Visible = false;
                                imgdiv2.Visible = true;
                                lbl_alert1.Text = "Please Enter Valid Range";
                            }
                        }
                    }
                    else
                    {
                        staff_chart.Visible = false;
                        chart_staff_chart.Visible = false;
                        imgdiv2.Visible = true;
                        lbl_alert1.Text = "No Records Found";
                    }
                }
                if (rdb_form4staffwise.Checked == true)
                {
                    selqry = "";
                    selqry = "  select sub.subject_code,SUM(m.Point)as point,SubjectNo,FeedBackMasterFK,StaffApplNo, ss.staff_code, ss.staff_name  from CO_StudFeedBack S,CO_MarkMaster M,CO_FeedBackMaster f ,subject sub,staff_appl_master sa,staffmaster ss where sa.appl_id=s.StaffApplNo and ss.appl_no=sa.appl_no  and sub.subject_no=s.SubjectNo and s.MarkMasterPK =M.MarkMasterPK  and f.FeedBackMasterPK =s.FeedBackMasterFK and s.FeedBackMasterFK in('" + feedbakpk + "') and subject_code in('" + sub + "') and f.CollegeCode in ('" + college_cd + "') and f.Batch_Year in ('" + Batch_Year + "') and f.degreecode in ('" + degree_code + "') and f.semester in ('" + semester + "')  and f.FeedBackType in ('" + type + "') group by SubjectNo,FeedBackMasterFK,StaffApplNo,sub.subject_code,ss.staff_code,ss.staff_name ";
                    selqry += " select top 1 COUNT(Distinct StaffApplNo) as Count,subjectNo from CO_StudFeedBack S where s.FeedBackMasterFK in('" + feedbakpk + "')  group by  subjectNo order by COUNT(Distinct StaffApplNo) desc";
                    selqry += " select distinct  SubjectNo ,su.subject_name  from CO_StudFeedBack S ,Subject su where s.SubjectNo =su.subject_no and s.FeedBackMasterFK in('" + feedbakpk + "')";
                    selqry += "  select COUNT(distinct app_no),StaffApplNo,SubjectNo  from CO_StudFeedBack S where s.FeedBackMasterFK  in('" + feedbakpk + "')  group by  StaffApplNo,SubjectNo ";
                    selqry += " select COUNT(*) from CO_FeedBackMaster f,CO_FeedBackQuestions fq where f.FeedBackMasterPK =fq.FeedBackMasterFK and f.FeedBackMasterPK  in('" + feedbakpk + "') ";
                    selqry += " select top 1 Point from CO_MarkMaster  where CollegeCode in('" + collegecode1 + "') order by Point desc";
                    string sum_total = ""; string attendedstudentcount = ""; string questioncount = "";
                    ds.Clear();
                    ds = d2.select_method_wo_parameter(selqry, "text");
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        if (ds.Tables[5].Rows.Count > 0)
                        {
                            sum_total = Convert.ToString(ds.Tables[5].Rows[0][0]);
                        }
                        if (ds.Tables[4].Rows.Count > 0)
                        {
                            questioncount = Convert.ToString(ds.Tables[4].Rows[0][0]);
                        }
                        ArrayList arrcol = new ArrayList();
                        for (int q = 0; q < Cbl_Subject.Items.Count; q++)
                        {
                            if (Cbl_Subject.Items[q].Selected == true)
                            {
                                string name = Cbl_Subject.Items[q].Text.ToString();
                                if (!arrcol.Contains(name))
                                {
                                    dc = new DataColumn();
                                    dc.ColumnName = name;
                                    dtChart1.Columns.Add(dc);
                                    arrcol.Add(name);
                                }
                            }
                        }
                        staff_chart.Titles[0].Text = ("STAFF PERCENTAGE (" + Convert.ToString(ddl_Feedbackname.SelectedItem.Value) + ")");
                        DataRow drp;
                        staff_chart.Series.Clear();
                        string subjectnam = "";
                        string subcode = "";
                        string subjectandcode = "";
                        string staffc = "";
                        ArrayList stafArr = new ArrayList();
                        ArrayList ques = new ArrayList();
                        for (int r = 0; r < ds.Tables[0].Rows.Count; r++)
                        {
                            subjectnam = ds.Tables[0].Rows[r]["staff_name"].ToString();
                            subcode = ds.Tables[0].Rows[r]["subjectno"].ToString();
                            staffc = ds.Tables[0].Rows[r]["staff_code"].ToString();
                            subjectandcode = subjectnam + "-" + staffc;
                            if (!stafArr.Contains(subjectandcode))
                            {
                                //ds.Tables[0].DefaultView.RowFilter = " subjectno='" + subcode + "'";
                                DataView dv = new DataView();
                                DataTable dtques = new DataTable();
                                dv = ds.Tables[0].DefaultView;
                                dtques = dv.ToTable();
                                staff_chart.Series.Add(subjectandcode + r.ToString());
                                stafArr.Add(subjectandcode);
                                Double poin = 0;
                                Double points1 = 0;
                                Double ps = 0;
                                Double totalpoint = 0;
                                drp = dtChart1.NewRow();
                                for (int q = 0; q < Cbl_Subject.Items.Count; q++)
                                {
                                    ps = 0; totalpoint = 0; poin = 0;
                                    if (Cbl_Subject.Items[q].Selected == true)
                                    {
                                        dtques.DefaultView.RowFilter = " staff_code='" + staffc + "' and subject_code='" + Cbl_Subject.Items[q].Value.ToString() + "'";
                                        dv = dtques.DefaultView;
                                        int w = 0;
                                        if (dv.Count > 0)
                                        {
                                            for (int pt = 0; pt < dv.Count; pt++)
                                            {
                                                w++;
                                                points1 = Convert.ToDouble(dv[pt]["Point"]);
                                                totalpoint += points1;
                                            }
                                            DataView attstud = new DataView();
                                            ds.Tables[3].DefaultView.RowFilter = " StaffApplNo ='" + Convert.ToString(dv[0]["StaffApplNo"]) + "' and SubjectNo='" + Convert.ToString(dv[0]["subjectNo"]) + "' ";
                                            attstud = ds.Tables[3].DefaultView;
                                            if (attstud.Count > 0)
                                            {
                                                attendedstudentcount = Convert.ToString(attstud[0][0]);
                                            }
                                            if (attendedstudentcount.Trim() == "")
                                                attendedstudentcount = "0";
                                            if (questioncount.Trim() == "")
                                                questioncount = "0";
                                            double point = totalpoint / (Convert.ToDouble(attendedstudentcount) * Convert.ToDouble(questioncount));
                                            string points = Convert.ToString(point);
                                            string needs = "5";
                                            //string sum_total = "9";
                                            ConvertedMark(needs, sum_total, ref points);
                                            string aaa = Convert.ToString(points);
                                            poin = Convert.ToDouble(aaa);
                                        }
                                        if (poin == 0)
                                            poin = poin / poin;
                                        string ps1 = Convert.ToString(Math.Round(poin, 1));
                                        if (ddl_criter.SelectedItem.Value != "0")
                                        {
                                            if (ddl_criter.SelectedItem.Value == "1")
                                            {
                                                if (txtfrom_range.Text.Trim() != "" && txtto_range.Text.Trim() != "")
                                                {
                                                    if (Convert.ToDouble(txtfrom_range.Text.Trim()) <= Convert.ToDouble(ps1) && Convert.ToDouble(ps1) <= Convert.ToDouble(txtto_range.Text.Trim()))
                                                    {
                                                        drp[Cbl_Subject.Items[q].Text.ToString()] = ps1;
                                                    }
                                                    else
                                                    {
                                                        drp[Cbl_Subject.Items[q].Text.ToString()] = "NaN";
                                                    }
                                                }
                                                else
                                                {
                                                    staff_chart.Visible = false;
                                                    chart_staff_chart.Visible = false;
                                                    imgdiv2.Visible = true;
                                                    lbl_alert1.Text = "Please Enter Range";
                                                }
                                            }
                                            else
                                            {
                                                drp[Cbl_Subject.Items[q].Text.ToString()] = ps1;
                                            }
                                        }
                                        if (ddl_criter.SelectedItem.Value == "0")
                                        {
                                            drp[Cbl_Subject.Items[q].Text.ToString()] = ps1;
                                        }
                                    }
                                }
                                dtChart1.Rows.Add(drp);
                            }
                        }
                        staff_chart.RenderType = RenderType.ImageTag;
                        staff_chart.ImageType = ChartImageType.Png;
                        staff_chart.ImageStorageMode = ImageStorageMode.UseImageLocation;
                        staff_chart.ImageLocation = Path.Combine("~/college/", "feedbackchart");
                        if (dtChart1.Rows.Count > 0)
                        {
                            string srr = ""; Hashtable removedthash = new Hashtable(); string val = ""; double finalvalue = 0;
                            for (int cc = 0; cc < dtChart1.Columns.Count; cc++)
                            {
                                finalvalue = 0;
                                for (int rr = 0; rr < dtChart1.Rows.Count; rr++)
                                {
                                    srr = dtChart1.Columns[cc].ToString();
                                    val = dtChart1.Rows[rr][cc].ToString();
                                    if (val.Trim() == "" || val.Trim().ToUpper() == "NAN")
                                        val = "0";
                                    finalvalue = finalvalue + Convert.ToDouble(val);
                                }
                                if (finalvalue == 0)
                                {
                                    removedthash.Add(cc, srr);
                                }
                            }
                            if (removedthash.Count > 0)
                            {
                                foreach (DictionaryEntry col in removedthash)
                                {
                                    dtChart1.Columns.Remove(Convert.ToString(col.Value));
                                }
                            }
                            if (dtChart1.Columns.Count > 0)
                            {
                                int chartwidth = 0;
                                for (int r = 0; r < dtChart1.Rows.Count; r++)
                                {
                                    for (int c = 0; c < dtChart1.Columns.Count; c++)
                                    {
                                        staff_chart.Series[r].Points.AddXY(dtChart1.Columns[c].ToString(), dtChart1.Rows[r][c].ToString());
                                        staff_chart.ChartAreas[0].AxisX.LineColor = System.Drawing.Color.Black;
                                        staff_chart.ChartAreas[0].AxisY.LineColor = System.Drawing.Color.Black;
                                        staff_chart.Series[r].IsValueShownAsLabel = true;
                                        staff_chart.Series[r].IsXValueIndexed = true;
                                        if (rb_linchart.Checked == true)
                                        {
                                            staff_chart.Series[r].ChartType = SeriesChartType.Line;
                                        }
                                        staff_chart.ChartAreas[0].AxisX.LabelStyle.Angle = -90;
                                        staff_chart.ChartAreas[0].AxisX.LabelStyle.Interval = 1;
                                        chartwidth += 15;
                                    }
                                }
                                if (chartwidth <= 1125)
                                    staff_chart.Width = chartwidth;
                                else
                                    staff_chart.Width = 1125;
                                chart_staff_chart.Visible = true;
                                chartprint.Visible = true;
                            }
                            else
                            {
                                staff_chart.Visible = false;
                                chart_staff_chart.Visible = false;
                                chartprint.Visible = false;
                                imgdiv2.Visible = true;
                                lbl_alert1.Text = "Please Enter Valid Range";
                            }
                        }
                    }
                    else
                    {
                        staff_chart.Visible = false;
                        chart_staff_chart.Visible = false;
                        imgdiv2.Visible = true;
                        lbl_alert1.Text = "No Records Found";
                    }
                }
            }
        }
        catch (Exception ex)
        {
            d2.sendErrorMail(ex, collegecode1, "FeedbackReport");
        }
    }

    public void format5()
    {
        try
        {
            if (rb_Acad.Checked == true)
            {
                fair();
                rb_linchart.Visible = true;
                rb_barchart.Visible = true;
                // div1.Visible = false;
                rptprint1.Visible = false;
                string college_cd = "";
                if (Cbl_college.Items.Count > 0)
                {
                    for (int i = 0; i < Cbl_college.Items.Count; i++)
                    {
                        if (Cbl_college.Items[i].Selected == true)
                        {
                            if (college_cd == "")
                            {
                                college_cd = "" + Cbl_college.Items[i].Value.ToString() + "";
                            }
                            else
                            {
                                college_cd = college_cd + "','" + Convert.ToString(Cbl_college.Items[i].Value);
                            }
                        }
                    }
                }
                string Batch_Year = "";
                for (int i = 0; i < cbl_batch.Items.Count; i++)
                {
                    if (cbl_batch.Items[i].Selected == true)
                    {
                        if (Batch_Year == "")
                        {
                            Batch_Year = "" + cbl_batch.Items[i].Value.ToString() + "";
                        }
                        else
                        {
                            Batch_Year = Batch_Year + "','" + cbl_batch.Items[i].Value.ToString() + "";
                        }
                    }
                }
                string degree_code = "";
                for (int i = 0; i < cbl_branch.Items.Count; i++)
                {
                    if (cbl_branch.Items[i].Selected == true)
                    {
                        if (degree_code == "")
                        {
                            degree_code = "" + cbl_branch.Items[i].Value.ToString() + "";
                        }
                        else
                        {
                            degree_code = degree_code + "','" + cbl_branch.Items[i].Value.ToString() + "";
                        }
                    }
                }
                string semester = "";
                for (int i = 0; i < cbl_sem.Items.Count; i++)
                {
                    if (cbl_sem.Items[i].Selected == true)
                    {
                        if (semester == "")
                        {
                            semester = "" + cbl_sem.Items[i].Value.ToString() + "";
                        }
                        else
                        {
                            semester = semester + "','" + cbl_sem.Items[i].Value.ToString() + "";
                        }
                    }
                }
                string staffcod = "";
                for (int i = 0; i < cbl_staffname.Items.Count; i++)
                {
                    if (cbl_staffname.Items[i].Selected == true)
                    {
                        if (staffcod == "")
                        {
                            staffcod = "" + cbl_staffname.Items[i].Value.ToString() + "";
                        }
                        else
                        {
                            staffcod = staffcod + "','" + cbl_staffname.Items[i].Value.ToString() + "";
                        }
                    }
                }
                string sub = "";
                for (int i = 0; i < Cbl_Subject.Items.Count; i++)
                {
                    if (Cbl_Subject.Items[i].Selected == true)
                    {
                        if (sub == "")
                        {
                            sub = "" + Cbl_Subject.Items[i].Value.ToString() + "";
                        }
                        else
                        {
                            sub = sub + "','" + Cbl_Subject.Items[i].Value.ToString() + "";
                        }
                    }
                }
                string section = "";
                for (int i = 0; i < cbl_sec.Items.Count; i++)
                {
                    if (cbl_sec.Items[i].Selected == true)
                    {
                        if (section == "")
                        {
                            section = "" + cbl_sec.Items[i].Value.ToString() + "";
                        }
                        else
                        {
                            section = section + "','" + cbl_sec.Items[i].Value.ToString() + "";
                        }
                        if (cbl_sec.Items[i].Value == "Empty")
                        {
                            section = "";
                        }
                    }
                }
                if (section.Trim() != "")
                {
                    section = section + "','";
                }
                dsfb.Clear();
                string fbpk = " select FeedBackMasterPK from CO_FeedBackMaster where FeedBackName ='" + ddl_Feedbackname.SelectedItem.Value + "' and DegreeCode in ('" + degree_code + "') and semester in ('" + semester + "') and Batch_Year in('" + Batch_Year + "') and section in ('" + section + "')";
                dsfb = d2.select_method_wo_parameter(fbpk, "Text");
                string feedbakpk = "";
                if (dsfb.Tables.Count > 0)
                {
                    if (dsfb.Tables[0].Rows.Count > 0)
                    {
                        for (int pk = 0; pk < dsfb.Tables[0].Rows.Count; pk++)
                        {
                            if (feedbakpk == "")
                            {
                                feedbakpk = dsfb.Tables[0].Rows[pk]["FeedBackMasterPK"].ToString();
                            }
                            else
                            {
                                feedbakpk = feedbakpk + "','" + dsfb.Tables[0].Rows[pk]["FeedBackMasterPK"].ToString() + "";
                            }
                        }
                    }
                }
                //and cf.FeedBackMasterPK in('" + feedbakpk + "')
                string questionpk = "";
                for (int i = 0; i < cbl_question.Items.Count; i++)
                {
                    if (cbl_question.Items[i].Selected == true)
                    {
                        if (questionpk == "")
                        {
                            questionpk = "" + cbl_question.Items[i].Value.ToString() + "";
                        }
                        else
                        {
                            questionpk = questionpk + "','" + cbl_question.Items[i].Value.ToString() + "";
                        }
                    }
                }
                string type = "";
                if (rb_Acad.Checked == true)
                {
                    type = "1";
                }
                else if (rb_Gend.Checked == true)
                {
                    type = "2";
                }
                ds.Clear();
                string selqry = "";
                selqry = "SELECT Staff_Name,s.subject_no,(Point)as Points,Question,t.staff_code FROM CO_StudFeedBack F,CO_MarkMaster M,Subject S,staff_appl_master A,staffmaster T,syllabus_master y,Degree d, Department dt,Course c,CO_FeedBackMaster FM,Registration r, CO_QuestionMaster Q WHERE Q.QuestionMasterPK =f.QuestionMasterFK and  F.MarkMasterPK = M.MarkMasterPK AND F.StaffApplNo = A.appl_id AND F.SubjectNo = S.subject_no AND A.appl_no = T.appl_no  and s.syll_code = y.syll_code and d.Degree_Code =y.degree_code and dt.Dept_Code =d.Dept_Code and c.Course_Id =d.Course_Id  and f.App_No = r.App_No and FM.CollegeCode in ('" + college_cd + "') and y.Batch_Year in ('" + Batch_Year + "') and y.degree_code in ('" + degree_code + "') and y.semester in ('" + semester + "')  and fm.FeedBackType =('" + type + "') and fm.FeedBackMasterPK in ('" + feedbakpk + "') and staff_code in('" + staffcod + "')and Subject_Code in ('" + sub + "')  ";
                if (section != "")
                {
                    selqry = selqry + " and r.Sections in ('" + section + "') and f.FeedBackMasterFK = fm.FeedBackMasterPK";
                }
                else
                {
                    selqry = selqry + " and f.FeedBackMasterFK = fm.FeedBackMasterPK ";
                }
                ds = d2.select_method_wo_parameter(selqry, "Text");
                DataTable dtChart2 = new DataTable();
                DataColumn dc;
                int width = 0;
                if (ds.Tables[0].Rows.Count > 0)
                {
                    DataRow drp;
                    //drp = dtChart1.NewRow();
                    staff_chart.Series.Clear();
                    string questionstr = "";
                    string subcode = "";
                    string staffandcode = "";
                    string staffnam = "";
                    ArrayList stafArr = new ArrayList();
                    string question = "";
                    question_chart.Series.Clear();
                    for (int i = 0; i < cbl_question.Items.Count; i++)
                    {
                        if (cbl_question.Items[i].Selected == true)
                        {
                            question = cbl_question.Items[i].Text.ToString();
                            question_chart.Series.Add(question.Trim());
                        }
                    }
                    for (int r = 0; r < ds.Tables[0].Rows.Count; r++)
                    {
                        staffnam = ds.Tables[0].Rows[r]["Staff_Name"].ToString();
                        subcode = ds.Tables[0].Rows[r]["subject_no"].ToString();
                        questionstr = ds.Tables[0].Rows[r]["Question"].ToString();
                        staffandcode = staffnam + "-" + subcode;
                        if (!stafArr.Contains(staffandcode))
                        {
                            DataView dv = new DataView();
                            dv = ds.Tables[0].DefaultView;
                            dc = new DataColumn();
                            stafArr.Add(staffandcode);
                            dc.ColumnName = staffandcode;
                            dtChart2.Columns.Add(dc);
                        }
                    }
                    DataRow drpq;
                    //drp = dtChart1.NewRow();
                    question_chart.Titles[0].Text = ("Questions Wise Chart (" + Convert.ToString(ddl_Feedbackname.SelectedItem.Value) + ")");
                    string quest = "";
                    int s = 0;
                    for (int i = 0; i < cbl_question.Items.Count; i++)
                    {
                        if (cbl_question.Items[i].Selected == true)
                        {
                            DataTable dtstp = new DataTable();
                            question = cbl_question.Items[i].Text.ToString();
                            ds.Tables[0].DefaultView.RowFilter = "Question='" + question + "'";
                            dtstp = ds.Tables[0].DefaultView.ToTable();
                            drpq = dtChart2.NewRow();
                            ArrayList ques = new ArrayList();
                            subcode = "";
                            staffandcode = "";
                            staffnam = "";
                            for (int st = 0; st < dtstp.Rows.Count; st++)
                            {
                                staffnam = dtstp.Rows[st]["Staff_Name"].ToString();
                                subcode = dtstp.Rows[st]["subject_no"].ToString();
                                staffandcode = staffnam + "-" + subcode;
                                if (!ques.Contains(staffandcode))
                                {
                                    ques.Add(staffandcode);
                                    dtstp.DefaultView.RowFilter = "Staff_Name='" + staffnam + "' and subject_no='" + subcode + "' ";
                                    DataView dv = new DataView();
                                    dv = dtstp.DefaultView;
                                    string points = "";
                                    Double poin = 0;
                                    Double ps = 0;
                                    int w = 0;
                                    for (int c = 0; c < dv.Count; c++)
                                    {
                                        w++;
                                        points = Convert.ToString(dv[c]["Points"]);
                                        string needs = "5";
                                        // string sum_total = "9";
                                        string sum_total = d2.GetFunction("select top 1 Point from CO_MarkMaster  where CollegeCode in('" + collegecode1 + "') order by Point desc");
                                        ConvertedMark(needs, sum_total, ref points);
                                        string aaa = Convert.ToString(points);
                                        poin = Convert.ToDouble(aaa);
                                        ps = ps + poin;
                                    }
                                    ps = ps / w;
                                    string ps1 = Convert.ToString(Math.Round(ps, 1));
                                    //drpq[staffandcode] = ps1;
                                    if (ddl_criter.SelectedItem.Value != "0")
                                    {
                                        if (ddl_criter.SelectedItem.Value == "1")
                                        {
                                            if (txtfrom_range.Text.Trim() != "" && txtto_range.Text.Trim() != "")
                                            {
                                                if (Convert.ToDouble(txtfrom_range.Text.Trim()) <= Convert.ToDouble(ps1) && Convert.ToDouble(ps1) <= Convert.ToDouble(txtto_range.Text.Trim()))
                                                {
                                                    drpq[staffandcode] = ps1;
                                                }
                                            }
                                            else
                                            {
                                                staff_chart.Visible = false;
                                                chart_staff_chart.Visible = false;
                                                imgdiv2.Visible = true;
                                                lbl_alert1.Text = "Please Enter Range";
                                            }
                                        }
                                        else
                                        {
                                            drpq[staffandcode] = ps1;
                                        }
                                    }
                                    if (ddl_criter.SelectedItem.Value == "0")
                                    {
                                        drpq[staffandcode] = ps1;
                                    }
                                }
                            }
                            dtChart2.Rows.Add(drpq);
                        }
                    }
                    //grd.DataSource = dtChart1;
                    //grd.DataBind();
                    question_chart.RenderType = RenderType.ImageTag;
                    question_chart.ImageType = ChartImageType.Png;
                    question_chart.ImageStorageMode = ImageStorageMode.UseImageLocation;
                    question_chart.ImageLocation = Path.Combine("~/college/", "feedbackchart");
                    if (dtChart2.Rows.Count > 0)
                    {
                        string srr = ""; Hashtable removedthash = new Hashtable(); string val = ""; double finalvalue = 0;
                        for (int cc = 0; cc < dtChart2.Columns.Count; cc++)
                        {
                            finalvalue = 0;
                            for (int rr = 0; rr < dtChart2.Rows.Count; rr++)
                            {
                                srr = dtChart2.Columns[cc].ToString();
                                val = dtChart2.Rows[rr][cc].ToString();
                                if (val.Trim() == "" || val.Trim().ToUpper() == "NAN")
                                    val = "0";
                                finalvalue = finalvalue + Convert.ToDouble(val);
                            }
                            if (finalvalue == 0)
                            {
                                removedthash.Add(cc, srr);
                            }
                        }
                        if (removedthash.Count > 0)
                        {
                            foreach (DictionaryEntry col in removedthash)
                            {
                                dtChart2.Columns.Remove(Convert.ToString(col.Value));
                            }
                        }
                        if (dtChart2.Columns.Count > 0)
                        {
                            int chartwidth = 0;
                            for (int r = 0; r < dtChart2.Rows.Count; r++)
                            {
                                for (int c = 0; c < dtChart2.Columns.Count; c++)
                                {
                                    //width += 40;
                                    string sr = dtChart2.Columns[c].ToString();
                                    question_chart.Series[r].Points.AddXY(sr.ToString().Trim(), dtChart2.Rows[r][c].ToString());
                                    question_chart.ChartAreas[0].AxisX.LineColor = System.Drawing.Color.Black;
                                    question_chart.ChartAreas[0].AxisY.LineColor = System.Drawing.Color.Black;
                                    question_chart.Series[r].IsValueShownAsLabel = true;
                                    question_chart.Series[r].IsXValueIndexed = true;
                                    if (rb_linchart.Checked == true)
                                    {
                                        question_chart.Series[r].ChartType = SeriesChartType.Line;
                                    }
                                    question_chart.ChartAreas[0].AxisX.LabelStyle.Angle = -90;
                                    question_chart.ChartAreas[0].AxisX.LabelStyle.Interval = 1;
                                    chartwidth += 15;
                                }
                            }
                            question_chart.Visible = true;
                            //question_chart.Width = Convert.ToInt32(700);
                            chartprint.Visible = true;
                            if (chartwidth <= 1125)
                                question_chart.Width = chartwidth;
                            else
                                question_chart.Width = 1125;
                            // GridView1.Visible = true;
                        }
                        else
                        {
                            chartprint.Visible = false;
                            imgdiv2.Visible = true;
                            lbl_alert1.Text = "Please Enter Valid Range";
                        }
                    }
                }
                else
                {
                    imgdiv2.Visible = true;
                    lbl_alert1.Text = "No Records Found";
                }
            }
        }
        catch (Exception ex)
        {
            d2.sendErrorMail(ex, collegecode1, "FeedbackReport");
        }
    }

    public void format6()
    {
        try
        {
            chartprint.Visible = false;
            fair();
            rb_linchart.Visible = true;
            rb_barchart.Visible = true;
            Total_points.Visible = false;
            // div1.Visible = false;
            rptprint1.Visible = false;
            string college_cd = "";
            if (Cbl_college.Items.Count > 0)
            {
                for (int i = 0; i < Cbl_college.Items.Count; i++)
                {
                    if (Cbl_college.Items[i].Selected == true)
                    {
                        if (college_cd == "")
                        {
                            college_cd = "" + Cbl_college.Items[i].Value.ToString() + "";
                        }
                        else
                        {
                            college_cd = college_cd + "','" + Convert.ToString(Cbl_college.Items[i].Value);
                        }
                    }
                }
            }
            string Batch_Year = "";
            for (int i = 0; i < cbl_batch.Items.Count; i++)
            {
                if (cbl_batch.Items[i].Selected == true)
                {
                    if (Batch_Year == "")
                    {
                        Batch_Year = "" + cbl_batch.Items[i].Value.ToString() + "";
                    }
                    else
                    {
                        Batch_Year = Batch_Year + "','" + cbl_batch.Items[i].Value.ToString() + "";
                    }
                }
            }
            string degree_code = "";
            for (int i = 0; i < cbl_branch.Items.Count; i++)
            {
                if (cbl_branch.Items[i].Selected == true)
                {
                    if (degree_code == "")
                    {
                        degree_code = "" + cbl_branch.Items[i].Value.ToString() + "";
                    }
                    else
                    {
                        degree_code = degree_code + "','" + cbl_branch.Items[i].Value.ToString() + "";
                    }
                }
            }
            string semester = "";
            for (int i = 0; i < cbl_sem.Items.Count; i++)
            {
                if (cbl_sem.Items[i].Selected == true)
                {
                    if (semester == "")
                    {
                        semester = "" + cbl_sem.Items[i].Value.ToString() + "";
                    }
                    else
                    {
                        semester = semester + "','" + cbl_sem.Items[i].Value.ToString() + "";
                    }
                }
            }
            string staffcod = "";
            for (int i = 0; i < cbl_staffname.Items.Count; i++)
            {
                if (cbl_staffname.Items[i].Selected == true)
                {
                    if (staffcod == "")
                    {
                        staffcod = "" + cbl_staffname.Items[i].Value.ToString() + "";
                    }
                    else
                    {
                        staffcod = staffcod + "','" + cbl_staffname.Items[i].Value.ToString() + "";
                    }
                }
            }
            string sub = "";
            for (int i = 0; i < Cbl_Subject.Items.Count; i++)
            {
                if (Cbl_Subject.Items[i].Selected == true)
                {
                    if (sub == "")
                    {
                        sub = "" + Cbl_Subject.Items[i].Value.ToString() + "";
                    }
                    else
                    {
                        sub = sub + "','" + Cbl_Subject.Items[i].Value.ToString() + "";
                    }
                }
            }
            string section = "";
            for (int i = 0; i < cbl_sec.Items.Count; i++)
            {
                if (cbl_sec.Items[i].Selected == true)
                {
                    if (section == "")
                    {
                        section = "" + cbl_sec.Items[i].Value.ToString() + "";
                    }
                    else
                    {
                        section = section + "','" + cbl_sec.Items[i].Value.ToString() + "";
                    }
                    if (cbl_sec.Items[i].Value == "Empty")
                    {
                        section = "";
                    }
                }
            }
            if (section.Trim() != "")
            {
                section = section + "','";
            }
            string fbpk = " select FeedBackMasterPK from CO_FeedBackMaster where FeedBackName ='" + ddl_Feedbackname.SelectedItem.Value + "' and DegreeCode in ('" + degree_code + "') and semester in ('" + semester + "') and Batch_Year in('" + Batch_Year + "') and section in ('" + section + "')";
            dsfb = d2.select_method_wo_parameter(fbpk, "Text");
            string feedbakpk = "";
            if (dsfb.Tables.Count > 0)
            {
                if (dsfb.Tables[0].Rows.Count > 0)
                {
                    for (int pk = 0; pk < dsfb.Tables[0].Rows.Count; pk++)
                    {
                        if (feedbakpk == "")
                        {
                            feedbakpk = dsfb.Tables[0].Rows[pk]["FeedBackMasterPK"].ToString();
                        }
                        else
                        {
                            feedbakpk = feedbakpk + "','" + dsfb.Tables[0].Rows[pk]["FeedBackMasterPK"].ToString() + "";
                        }
                    }
                }
            }
            //and cf.FeedBackMasterPK in('" + feedbakpk + "')
            string questionpk = "";
            for (int i = 0; i < cbl_question.Items.Count; i++)
            {
                if (cbl_question.Items[i].Selected == true)
                {
                    if (questionpk == "")
                    {
                        questionpk = "" + cbl_question.Items[i].Value.ToString() + "";
                    }
                    else
                    {
                        questionpk = questionpk + "','" + cbl_question.Items[i].Value.ToString() + "";
                    }
                }
            }
            string type = "";
            if (rb_Acad.Checked == true)
            {
                type = "1";
            }
            else if (rb_Gend.Checked == true)
            {
                type = "2";
            }
            ds.Clear();
            string selectquestion = "";
            selectquestion = " select distinct COUNT (cs.App_No)as Studentcount, SUM(point)as total, cq.Question, cs.QuestionMasterFK,SubjectNo from CO_StudFeedBack cs,CO_MarkMaster cm, CO_FeedBackMaster fm,CO_QuestionMaster cq where cs.MarkMasterPK =cm.MarkMasterPK and fm.FeedBackMasterPK =cs.FeedBackMasterFK and cq.QuestionMasterPK =cs.QuestionMasterFK and cq.QuestionMasterPK in('" + questionpk + "') and  fm.DegreeCode in ('" + degree_code + "') and fm.CollegeCode in ('" + college_cd + "') and fm.Batch_Year in ('" + Batch_Year + "') and fm.FeedBackMasterPK in ('" + feedbakpk + "') and fm.semester in ('" + semester + "')  ";
            if (section != "")
            {
                selectquestion = selectquestion + " and fm.Section in ('" + section + "') and fm.FeedBackType='" + type + "' group by  cs.QuestionMasterFK, SubjectNo,Question";
            }
            else
            {
                selectquestion = selectquestion + " and fm.FeedBackType='" + type + "' group by  cs.QuestionMasterFK,SubjectNo,Question";
            }
            ds.Clear();
            ds = d2.select_method_wo_parameter(selectquestion, "Text");
            // select distinct COUNT (cs.App_No) as Studentcount, cs.QuestionMasterFK,SubjectNo from CO_StudFeedBack cs,CO_MarkMaster cm, CO_FeedBackMaster fm,CO_QuestionMaster cq where cs.MarkMasterPK =cm.MarkMasterPK and fm.FeedBackMasterPK =cs.FeedBackMasterFK and cq.QuestionMasterPK =cs.QuestionMasterFK and cq.QuestionMasterPK in('49','50','51') and  fm.DegreeCode in ('45') and fm.CollegeCode in ('13') and fm.Batch_Year in ('2015') and fm.FeedBackMasterPK in ('1') and fm.semester in ('1')   and fm.Section in ('A','') and fm.FeedBackType='1' group by  cs.QuestionMasterFK, SubjectNo,Question
            DataTable dtChart3 = new DataTable();
            DataColumn dct;
            if (ds.Tables.Count > 0)
            {
                if (ds.Tables[0].Rows.Count > 0)
                {
                    string question = "";
                    Total_points.Series.Clear();
                    for (int i = 0; i < cbl_question.Items.Count; i++)
                    {
                        if (cbl_question.Items[i].Selected == true)
                        {
                            question = cbl_question.Items[i].Text.ToString();
                            Total_points.Series.Add(question.Trim());
                            dct = new DataColumn();
                            dct.ColumnName = question;
                            dtChart3.Columns.Add(dct);
                        }
                    }
                    DataRow drtq;
                    string quest = "";
                    int s = 0;
                    drtq = dtChart3.NewRow();
                    for (int i = 0; i < cbl_question.Items.Count; i++)
                    {
                        if (cbl_question.Items[i].Selected == true)
                        {
                            question = cbl_question.Items[i].Text.ToString();
                            ds.Tables[0].DefaultView.RowFilter = "Question='" + question + "'";
                            DataView dv = new DataView();
                            dv = ds.Tables[0].DefaultView;
                            string points = "";
                            string sum_total = "";
                            string needs = "";
                            string tot_stud = "";
                            double tot_points = 0;
                            double sum_tot_points = 0;
                            sum_total = d2.GetFunction("select top 1 Point from CO_MarkMaster  where CollegeCode in('" + collegecode1 + "') order by Point desc");
                            for (int c = 0; c < dv.Count; c++)
                            {
                                sum_tot_points = 0;
                                string totals = dv[c]["total"].ToString();
                                tot_stud = dv[c]["Studentcount"].ToString();
                                sum_tot_points = Convert.ToDouble(totals) / Convert.ToDouble(tot_stud);
                                // sum_total = Convert.ToString(sum_tot_points);
                                needs = "5";
                                points = Convert.ToString(sum_tot_points);
                                if (points != "0" && tot_stud != "0")
                                {
                                    ConvertedMark(needs, sum_total, ref points);
                                    string aaa = Convert.ToString(points);
                                    tot_points = Convert.ToDouble(aaa);
                                    //Double aaa = Convert.ToDouble(points);
                                    //Double ss = Convert.ToString(Math.Round(aaa, 2));
                                    //tot_points = Convert.ToDouble(points) / Convert.ToDouble(tot_stud);
                                }
                            }
                            if (ddl_criter.SelectedItem.Value != "0")
                            {
                                if (ddl_criter.SelectedItem.Value == "1")
                                {
                                    if (txtfrom_range.Text.Trim() != "" && txtto_range.Text.Trim() != "")
                                    {
                                        if (Convert.ToDouble(txtfrom_range.Text.Trim()) <= Convert.ToDouble(tot_points) && Convert.ToDouble(tot_points) <= Convert.ToDouble(txtto_range.Text.Trim()))
                                        {
                                            drtq[s] = tot_points;
                                        }
                                    }
                                    else
                                    {
                                        staff_chart.Visible = false;
                                        chart_staff_chart.Visible = false;
                                        imgdiv2.Visible = true;
                                        lbl_alert1.Text = "Please Enter Range";
                                    }
                                }
                                else
                                {
                                    drtq[s] = tot_points;
                                }
                            }
                            if (ddl_criter.SelectedItem.Value == "0")
                            {
                                drtq[s] = tot_points;
                            }
                            //drtq[s] = tot_points;
                            s++;
                        }
                    }
                    dtChart3.Rows.Add(drtq);
                    if (dtChart3.Rows.Count > 0)
                    {
                        string srr = ""; Hashtable removedthash = new Hashtable(); string val = ""; double finalvalue = 0;
                        for (int cc = 0; cc < dtChart3.Columns.Count; cc++)
                        {
                            finalvalue = 0;
                            for (int rr = 0; rr < dtChart3.Rows.Count; rr++)
                            {
                                srr = dtChart3.Columns[cc].ToString();
                                val = dtChart3.Rows[rr][cc].ToString();
                                if (val.Trim() == "" || val.Trim().ToUpper() == "NAN")
                                    val = "0";
                                finalvalue = finalvalue + Convert.ToDouble(val);
                            }
                            if (finalvalue == 0)
                            {
                                removedthash.Add(cc, srr);
                            }
                        }
                        if (removedthash.Count > 0)
                        {
                            foreach (DictionaryEntry col in removedthash)
                            {
                                dtChart3.Columns.Remove(Convert.ToString(col.Value));
                            }
                        }
                        if (dtChart3.Columns.Count > 0)
                        {
                            for (int r = 0; r < dtChart3.Rows.Count; r++)
                            {
                                for (int c = 0; c < dtChart3.Columns.Count; c++)
                                {
                                    string col = dtChart3.Columns[c].ToString();
                                    Total_points.Series[r].Points.AddXY(col.ToString().Trim(), dtChart3.Rows[r][c].ToString());
                                    Total_points.ChartAreas[0].AxisX.LineColor = System.Drawing.Color.Black;
                                    Total_points.ChartAreas[0].AxisY.LineColor = System.Drawing.Color.Black;
                                    Total_points.Series[r].IsValueShownAsLabel = true;
                                    Total_points.Series[r].IsXValueIndexed = true;
                                    if (rb_linchart.Checked == true)
                                    {
                                        Total_points.Series[r].ChartType = SeriesChartType.Line;
                                    }
                                    Total_points.ChartAreas[0].AxisX.LabelStyle.Angle = -90;
                                    Total_points.ChartAreas[0].AxisX.LabelStyle.Interval = 1;
                                }
                            }
                            Total_points.Visible = true;
                        }
                        else
                        {
                            chartprint.Visible = false;
                            imgdiv2.Visible = true;
                            lbl_alert1.Text = "Please Enter Valid Range";
                        }
                    }
                }
                else
                {
                    imgdiv2.Visible = true;
                    lbl_alert1.Text = "No Records Found";
                }
            }
            else
            {
                imgdiv2.Visible = true;
                lbl_alert1.Text = "No Records Found";
            }
        }
        catch (Exception ex)
        {
            d2.sendErrorMail(ex, collegecode1, "FeedbackReport");
        }
    }

    public void format7()
    {
        try
        {
            chartprint.Visible = false;
            double wid = 300;
            if (rb_Acad.Checked == true)
            {
                if (ddl_staffname.SelectedItem.Text == "Select")
                {
                    // div1.Visible = false;
                    imgdiv2.Visible = true;
                    rptprint1.Visible = false;
                    lbl_alert1.Text = "Please Select Staff Name";
                    FpSpread4.Visible = false;
                    return;
                }
                fair();
                lbl_stfsubject.Visible = true;
                UpdatePanel17.Visible = true;
                Txt_stfSubject.Visible = true;
                stud_div.Visible = true;
                // div1.Visible = false;
                rptprint1.Visible = false;
                FpSpread4.Sheets[0].RowCount = 0;
                FpSpread4.Sheets[0].ColumnCount = 0;
                FpSpread4.CommandBar.Visible = false;
                FpSpread4.Sheets[0].AutoPostBack = true;
                FpSpread4.Sheets[0].ColumnHeader.RowCount = 1;
                FpSpread4.Sheets[0].RowHeader.Visible = false;
                FpSpread4.Sheets[0].ColumnCount = 5;
                FarPoint.Web.Spread.StyleInfo darkstyle = new FarPoint.Web.Spread.StyleInfo();
                darkstyle.BackColor = ColorTranslator.FromHtml("#0CA6CA");
                darkstyle.ForeColor = System.Drawing.Color.White;
                FpSpread4.ActiveSheetView.ColumnHeader.DefaultStyle = darkstyle;
                FpSpread4.Visible = true;
                FpSpread4.SaveChanges();
                FpSpread4.Sheets[0].ColumnHeader.Cells[0, 0].Text = "S.No";
                FpSpread4.Sheets[0].ColumnHeader.Cells[0, 0].Font.Bold = true;
                FpSpread4.Sheets[0].ColumnHeader.Cells[0, 0].HorizontalAlign = HorizontalAlign.Center;
                FpSpread4.Sheets[0].ColumnHeader.Cells[0, 0].Font.Name = "Book Antiqua";
                FpSpread4.Sheets[0].ColumnHeader.Cells[0, 0].Font.Size = FontUnit.Medium;
                FpSpread4.Sheets[0].Columns[0].HorizontalAlign = HorizontalAlign.Center;
                FpSpread4.Sheets[0].ColumnHeader.Cells[0, 1].Text = "App No";
                //FpSpread4.Sheets[0].SetColumnMerge(1, FarPoint.Web.Spread.Model.MergePolicy.Always);
                FpSpread4.Sheets[0].ColumnHeader.Cells[0, 1].Font.Bold = true;
                FpSpread4.Sheets[0].ColumnHeader.Cells[0, 1].Font.Name = "Book Antiqua";
                FpSpread4.Sheets[0].ColumnHeader.Cells[0, 1].Font.Size = FontUnit.Medium;
                FpSpread4.Sheets[0].ColumnHeader.Cells[0, 1].HorizontalAlign = HorizontalAlign.Center;
                FpSpread4.Sheets[0].ColumnHeader.Cells[0, 2].Text = "Roll NO ";
                FpSpread4.Sheets[0].ColumnHeader.Cells[0, 2].Font.Bold = true;
                FpSpread4.Sheets[0].ColumnHeader.Cells[0, 2].Font.Name = "Book Antiqua";
                FpSpread4.Sheets[0].ColumnHeader.Cells[0, 2].Font.Size = FontUnit.Medium;
                FpSpread4.Sheets[0].ColumnHeader.Cells[0, 2].HorizontalAlign = HorizontalAlign.Center;
                FpSpread4.Sheets[0].ColumnHeader.Cells[0, 3].Text = "Reg No  ";
                FpSpread4.Sheets[0].ColumnHeader.Cells[0, 3].Font.Bold = true;
                FpSpread4.Sheets[0].ColumnHeader.Cells[0, 3].Font.Name = "Book Antiqua";
                FpSpread4.Sheets[0].ColumnHeader.Cells[0, 3].Font.Size = FontUnit.Medium;
                FpSpread4.Sheets[0].ColumnHeader.Cells[0, 3].HorizontalAlign = HorizontalAlign.Center;
                FpSpread4.Sheets[0].ColumnHeader.Cells[0, 4].Text = "Student Name";
                FpSpread4.Sheets[0].ColumnHeader.Cells[0, 4].Font.Bold = true;
                FpSpread4.Sheets[0].ColumnHeader.Cells[0, 4].Font.Name = "Book Antiqua";
                FpSpread4.Sheets[0].ColumnHeader.Cells[0, 4].Font.Size = FontUnit.Medium;
                FpSpread4.Sheets[0].ColumnHeader.Cells[0, 4].HorizontalAlign = HorizontalAlign.Center;
                //FpSpread4.Sheets[0].ColumnHeader.Columns[0].Width = 52;
                //FpSpread4.Sheets[0].ColumnHeader.Columns[1].Width = 100;
                //FpSpread4.Sheets[0].ColumnHeader.Columns[2].Width = 100;
                //FpSpread4.Sheets[0].ColumnHeader.Columns[3].Width = 150;
                for (int i = 0; i < cbl_question.Items.Count; i++)
                {
                    if (cbl_question.Items[i].Selected == true)
                    {
                        FpSpread4.Columns.Count++;
                        string qustntg = cbl_question.Items[i].Value.ToString();
                        string qustn = cbl_question.Items[i].Text.ToString();
                        FpSpread4.Sheets[0].ColumnHeader.Cells[0, FpSpread4.Columns.Count - 1].Text = qustn;
                        FpSpread4.Sheets[0].ColumnHeader.Cells[0, FpSpread4.Columns.Count - 1].Tag = qustntg;
                        FpSpread4.Sheets[0].ColumnHeader.Cells[0, FpSpread4.Columns.Count - 1].Font.Bold = true;
                        FpSpread4.Sheets[0].ColumnHeader.Cells[0, FpSpread4.Columns.Count - 1].Font.Name = "Book Antiqua";
                        FpSpread4.Sheets[0].ColumnHeader.Cells[0, FpSpread4.Columns.Count - 1].Font.Size = FontUnit.Medium;
                        FpSpread4.Sheets[0].ColumnHeader.Cells[0, FpSpread4.Columns.Count - 1].HorizontalAlign = HorizontalAlign.Center;
                        FpSpread4.Sheets[0].Columns[FpSpread4.Columns.Count - 1].Width = (qustn.Length * 10);
                        wid += (qustn.Length * 10);
                    }
                }
                //FpSpread4.Sheets[0].Columns.Count++;
                //FpSpread4.Sheets[0].ColumnHeader.Cells[0, FpSpread4.Columns.Count - 1].Text = "Total";
                //FpSpread4.Sheets[0].ColumnHeader.Cells[0, FpSpread4.Columns.Count - 1].Font.Bold = true;
                //FpSpread4.Sheets[0].ColumnHeader.Cells[0, FpSpread4.Columns.Count - 1].Font.Name = "Book Antiqua";
                //FpSpread4.Sheets[0].ColumnHeader.Cells[0, FpSpread4.Columns.Count - 1].Font.Size = FontUnit.Medium;
                //FpSpread4.Sheets[0].ColumnHeader.Cells[0, FpSpread4.Columns.Count - 1].HorizontalAlign = HorizontalAlign.Center;
                //FpSpread4.Sheets[0].Columns.Count++;
                //FpSpread4.Sheets[0].ColumnHeader.Cells[0, FpSpread4.Columns.Count - 1].Text = "AVG";
                //FpSpread4.Sheets[0].ColumnHeader.Cells[0, FpSpread4.Columns.Count - 1].Font.Bold = true;
                //FpSpread4.Sheets[0].ColumnHeader.Cells[0, FpSpread4.Columns.Count - 1].Font.Name = "Book Antiqua";
                //FpSpread4.Sheets[0].ColumnHeader.Cells[0, FpSpread4.Columns.Count - 1].Font.Size = FontUnit.Medium;
                //FpSpread4.Sheets[0].ColumnHeader.Cells[0, FpSpread4.Columns.Count - 1].HorizontalAlign = HorizontalAlign.Center;
                string college_cd = "";
                if (Cbl_college.Items.Count > 0)
                {
                    for (int i = 0; i < Cbl_college.Items.Count; i++)
                    {
                        if (Cbl_college.Items[i].Selected == true)
                        {
                            if (college_cd == "")
                            {
                                college_cd = "" + Cbl_college.Items[i].Value.ToString() + "";
                            }
                            else
                            {
                                college_cd = college_cd + "','" + Convert.ToString(Cbl_college.Items[i].Value);
                            }
                        }
                    }
                }
                string Batch_Year = "";
                for (int i = 0; i < cbl_batch.Items.Count; i++)
                {
                    if (cbl_batch.Items[i].Selected == true)
                    {
                        if (Batch_Year == "")
                        {
                            Batch_Year = "" + cbl_batch.Items[i].Value.ToString() + "";
                        }
                        else
                        {
                            Batch_Year = Batch_Year + "','" + cbl_batch.Items[i].Value.ToString() + "";
                        }
                    }
                }
                string degree_code = "";
                for (int i = 0; i < cbl_branch.Items.Count; i++)
                {
                    if (cbl_branch.Items[i].Selected == true)
                    {
                        if (degree_code == "")
                        {
                            degree_code = "" + cbl_branch.Items[i].Value.ToString() + "";
                        }
                        else
                        {
                            degree_code = degree_code + "','" + cbl_branch.Items[i].Value.ToString() + "";
                        }
                    }
                }
                string semester = "";
                for (int i = 0; i < cbl_sem.Items.Count; i++)
                {
                    if (cbl_sem.Items[i].Selected == true)
                    {
                        if (semester == "")
                        {
                            semester = "" + cbl_sem.Items[i].Value.ToString() + "";
                        }
                        else
                        {
                            semester = semester + "','" + cbl_sem.Items[i].Value.ToString() + "";
                        }
                    }
                }
                string section = "";
                for (int i = 0; i < cbl_sec.Items.Count; i++)
                {
                    if (cbl_sec.Items[i].Selected == true)
                    {
                        if (section == "")
                        {
                            section = "" + cbl_sec.Items[i].Value.ToString() + "";
                        }
                        else
                        {
                            section = section + "','" + cbl_sec.Items[i].Value.ToString() + "";
                        }
                        if (cbl_sec.Items[i].Value == "Empty")
                        {
                            section = "";
                        }
                    }
                }
                if (section.Trim() != "")
                {
                    section = section + "','";
                }
                dsfb.Clear();
                string fbpk = " select FeedBackMasterPK,subject_type from CO_FeedBackMaster where FeedBackName ='" + ddl_Feedbackname.SelectedItem.Value + "' and DegreeCode in ('" + degree_code + "') and semester in ('" + semester + "') and Batch_Year in('" + Batch_Year + "') and section in ('" + section + "')";
                dsfb = d2.select_method_wo_parameter(fbpk, "Text");
                string st_type = "";
                string sub_type = "";
                string feedbakpk = "";
                if (dsfb.Tables.Count > 0)
                {
                    if (dsfb.Tables[0].Rows.Count > 0)
                    {
                        for (int pk = 0; pk < dsfb.Tables[0].Rows.Count; pk++)
                        {
                            if (feedbakpk == "")
                            {
                                feedbakpk = dsfb.Tables[0].Rows[pk]["FeedBackMasterPK"].ToString();
                            }
                            else
                            {
                                feedbakpk = feedbakpk + "','" + dsfb.Tables[0].Rows[pk]["FeedBackMasterPK"].ToString() + "";
                            }
                        }
                        string subtype = dsfb.Tables[0].Rows[0]["subject_type"].ToString();
                        if (subtype != "")
                        {
                            st_type = subtype.ToString();
                            string[] split = st_type.Split(',');
                            for (int i = 0; i < split.Length; i++)
                            {
                                if (sub_type == "")
                                {
                                    sub_type = split[i];
                                }
                                else
                                {
                                    sub_type += "','" + split[i];
                                }
                            }
                        }
                    }
                }
                //and cf.FeedBackMasterPK in('" + feedbakpk + "')
                string questionpk = "";
                for (int i = 0; i < cbl_question.Items.Count; i++)
                {
                    if (cbl_question.Items[i].Selected == true)
                    {
                        if (questionpk == "")
                        {
                            questionpk = "" + cbl_question.Items[i].Value.ToString() + "";
                        }
                        else
                        {
                            questionpk = questionpk + "','" + cbl_question.Items[i].Value.ToString() + "";
                        }
                    }
                }
                string subno = "";
                for (int i = 0; i < Cbl_StfSubject.Items.Count; i++)
                {
                    if (Cbl_StfSubject.Items[i].Selected == true)
                    {
                        if (subno == "")
                        {
                            subno = "" + Cbl_StfSubject.Items[i].Value.ToString() + "";
                        }
                        else
                        {
                            subno = subno + "','" + Cbl_StfSubject.Items[i].Value.ToString() + "";
                        }
                    }
                }
                string type = "";
                if (rb_Acad.Checked == true)
                {
                    type = "1";
                }
                else if (rb_Gend.Checked == true)
                {
                    type = "2";
                }
                string stfapno = d2.GetFunction("select sa.appl_id  from staffmaster s,staff_appl_master sa where s.appl_no =sa.appl_no and s.staff_code ='" + ddl_staffname.SelectedItem.Value.ToString() + "'");
                ds.Clear();
                string selqry = "";
                selqry = "select distinct s.App_No,r.Roll_No,r.Reg_No,r.Stud_Name  from CO_StudFeedBack s,Registration r,subject sc where r.App_No =s.App_No and s.SubjectNo =sc.subject_no and FeedBackMasterFK in ('" + feedbakpk + "') and  s.QuestionMasterFK in ('" + questionpk + "') and StaffApplNo ='" + stfapno + "' and sc.subject_code in ('" + subno + "') ";
                //'" + ddl_staffname.SelectedItem.Value.ToString()+ "' ";
                selqry = selqry + "select s.App_No,s.QuestionMasterFK,m.Point ,FeedBackMasterFK   from CO_StudFeedBack s,Registration r,CO_MarkMaster M ,subject sc where m.MarkMasterPK =s.MarkMasterPK and r.App_No =s.App_No and s.SubjectNo =sc.subject_no and FeedBackMasterFK in ('" + feedbakpk + "') and StaffApplNo ='" + stfapno + "' and sc.subject_code in ('" + subno + "') and s.QuestionMasterFK in ('" + questionpk + "')";
                ds = d2.select_method_wo_parameter(selqry, "Text");
                DataView dv = new DataView();
                if (ds.Tables.Count > 0)
                {
                    if (ds.Tables[0].Rows.Count > 0 && ds.Tables[1].Rows.Count > 0)
                    {
                        rptprint1.Visible = true;
                        for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                        {
                            FpSpread4.Sheets[0].Rows.Count++;
                            FpSpread4.Sheets[0].Cells[FpSpread4.Sheets[0].RowCount - 1, 0].Text = Convert.ToString(i + 1);
                            FpSpread4.Sheets[0].Cells[FpSpread4.Sheets[0].RowCount - 1, 1].Text = ds.Tables[0].Rows[i]["App_No"].ToString();
                            FpSpread4.Sheets[0].Cells[FpSpread4.Sheets[0].RowCount - 1, 2].Text = ds.Tables[0].Rows[i]["Roll_No"].ToString();
                            FpSpread4.Sheets[0].Cells[FpSpread4.Sheets[0].RowCount - 1, 3].Text = ds.Tables[0].Rows[i]["Reg_No"].ToString();
                            FpSpread4.Sheets[0].Cells[FpSpread4.Sheets[0].RowCount - 1, 4].Text = ds.Tables[0].Rows[i]["Stud_Name"].ToString();
                            FpSpread4.Sheets[0].Columns[4].Visible = false;
                        }
                        for (int r = 0; r < FpSpread4.Rows.Count; r++)
                        {
                            Double mark = 0;
                            int coun = 0;
                            for (int cl = 5; cl < FpSpread4.Columns.Count; cl++)
                            {
                                string appno = FpSpread4.Sheets[0].Cells[r, 1].Text;
                                string questionfk = Convert.ToString(FpSpread4.Sheets[0].ColumnHeader.Cells[0, cl].Tag);
                                string filterquery = " App_No='" + appno + "' and QuestionMasterFK='" + questionfk + "'";
                                ds.Tables[1].DefaultView.RowFilter = "" + filterquery + "";
                                dv = ds.Tables[1].DefaultView;
                                if (dv.Count > 0)
                                {
                                    coun++;
                                    FpSpread4.Sheets[0].Cells[r, cl].Text = Convert.ToString(dv[0]["Point"]);
                                    mark = mark + Convert.ToDouble(FpSpread4.Sheets[0].Cells[r, cl].Text);
                                    FpSpread4.Sheets[0].Cells[r, cl].Font.Name = "Book Antiqua";
                                    FpSpread4.Sheets[0].Cells[r, cl].Font.Size = FontUnit.Medium;
                                    FpSpread4.Sheets[0].Cells[r, cl].HorizontalAlign = HorizontalAlign.Center;
                                }
                            }
                            //FpSpread4.Sheets[0].Cells[r, FpSpread4.Columns.Count - 2].Text = Convert.ToString(mark);
                            //FpSpread4.Sheets[0].Cells[r, FpSpread4.Columns.Count - 2].Font.Name = "Book Antiqua";
                            //FpSpread4.Sheets[0].Cells[r, FpSpread4.Columns.Count - 2].Font.Size = FontUnit.Medium;
                            //FpSpread4.Sheets[0].Cells[r, FpSpread4.Columns.Count - 2].HorizontalAlign = HorizontalAlign.Center;
                            //Double avg = mark / coun;
                            //string needs = "5";
                            //string sum_total = d2.GetFunction("select top 1 Point from CO_MarkMaster  where CollegeCode in('" + collegecode1 + "') order by Point desc");
                            //string points = Convert.ToString(avg);
                            //ConvertedMark(needs, sum_total, ref points);
                            //FpSpread4.Sheets[0].Cells[r, FpSpread4.Columns.Count - 1].Text = Convert.ToString(points);
                            //FpSpread4.Sheets[0].Cells[r, FpSpread4.Columns.Count - 1].Font.Name = "Book Antiqua";
                            //FpSpread4.Sheets[0].Cells[r, FpSpread4.Columns.Count - 1].Font.Size = FontUnit.Medium;
                            //FpSpread4.Sheets[0].Cells[r, FpSpread4.Columns.Count - 1].HorizontalAlign = HorizontalAlign.Center;
                        }
                        double GrandTota = 0;
                        FpSpread4.Rows.Count += 2;
                        for (int clmn = 5; clmn < FpSpread4.Columns.Count; clmn++)
                        {
                            double totalval = 0;
                            int counts = 0;
                            double newtota = 0;
                            for (int rw = 0; rw < FpSpread4.Rows.Count - 2; rw++)
                            {
                                counts++;
                                string tol = Convert.ToString(FpSpread4.Sheets[0].Cells[rw, clmn].Text);
                                if (tol.Trim() == "")
                                    tol = "0";
                                if (totalval == 0)
                                {
                                    totalval = Convert.ToDouble(tol);
                                }
                                else
                                {
                                    totalval = totalval + Convert.ToDouble(tol);
                                }
                            }
                            newtota = totalval;
                            GrandTota = GrandTota + newtota;
                            totalval = totalval / Convert.ToDouble(counts);
                            string sum_total = d2.GetFunction("select top 1 Point from CO_MarkMaster  where CollegeCode in('" + collegecode1 + "') order by Point desc");
                            string needs = "5";
                            string points = Convert.ToString(totalval);
                            ConvertedMark(needs, sum_total, ref points);
                            FpSpread4.Sheets[0].Cells[FpSpread4.Sheets[0].RowCount - 2, clmn].Text = Convert.ToString(newtota);
                            FpSpread4.Sheets[0].Cells[FpSpread4.Sheets[0].RowCount - 2, clmn].HorizontalAlign = HorizontalAlign.Center;
                            FpSpread4.Sheets[0].Cells[FpSpread4.Sheets[0].RowCount - 1, clmn].Text = Convert.ToString(points);
                            FpSpread4.Sheets[0].Cells[FpSpread4.Sheets[0].RowCount - 1, clmn].HorizontalAlign = HorizontalAlign.Center;
                        }
                        FpSpread4.Sheets[0].RowCount++;
                        int cound = 0;
                        double totalavg = 0;
                        for (int clms = 5; clms < FpSpread4.Columns.Count; clms++)
                        {
                            cound++;
                            string av = Convert.ToString(FpSpread4.Sheets[0].Cells[FpSpread4.Rows.Count - 2, clms].Text);
                            if (av.Trim() == "")
                                av = "0";
                            if (totalavg == 0)
                            {
                                totalavg = Convert.ToDouble(av);
                            }
                            else
                            {
                                totalavg = totalavg + Convert.ToDouble(av);
                            }
                        }
                        totalavg = totalavg / Convert.ToDouble(cound);
                        totalavg = (Math.Round(totalavg, 2));
                        //double cou = 0;
                        //Double av = 0;
                        //int cnt = 0;
                        //for (int rs = 0; rs < FpSpread4.Sheets[0].RowCount - 2; rs++)
                        //{
                        //    cnt++;
                        //    string tot = FpSpread4.Sheets[0].Cells[rs, FpSpread4.Columns.Count - 2].Text.ToString();
                        //    string average = FpSpread4.Sheets[0].Cells[rs, FpSpread4.Columns.Count - 1].Text.ToString();
                        //    cou = cou + Convert.ToDouble(tot);
                        //    av = av + Convert.ToDouble(average);
                        //}
                        //FpSpread4.Sheets[0].Cells[FpSpread4.Sheets[0].RowCount - 1, FpSpread4.Columns.Count - 2].Text = Convert.ToString(cou);
                        //FpSpread4.Sheets[0].Cells[FpSpread4.Sheets[0].RowCount - 1, FpSpread4.Columns.Count - 2].HorizontalAlign = HorizontalAlign.Center;
                        //Double spavg = av / Convert.ToDouble(cnt);
                        // FpSpread4.Sheets[0].Cells[FpSpread4.Sheets[0].RowCount - 1, FpSpread4.Columns.Count - 1].Text = Convert.ToString(totalavg);
                        //FpSpread4.Sheets[0].Cells[FpSpread4.Sheets[0].RowCount - 1, FpSpread4.Columns.Count - 1].HorizontalAlign = HorizontalAlign.Center;
                        FpSpread4.Sheets[0].SpanModel.Add(FpSpread4.Sheets[0].RowCount - 3, 0, 1, 5);
                        FpSpread4.Sheets[0].Cells[FpSpread4.Sheets[0].Rows.Count - 3, 0].Text = "Total";
                        FpSpread4.Sheets[0].Cells[FpSpread4.Sheets[0].Rows.Count - 3, 0].ForeColor = System.Drawing.Color.Blue;
                        FpSpread4.Sheets[0].SpanModel.Add(FpSpread4.Sheets[0].RowCount - 2, 0, 1, 5);
                        FpSpread4.Sheets[0].Cells[FpSpread4.Sheets[0].Rows.Count - 2, 0].Text = "Total Average";
                        FpSpread4.Sheets[0].Cells[FpSpread4.Sheets[0].Rows.Count - 2, 0].ForeColor = System.Drawing.Color.Blue;
                        //FpSpread4.Sheets[0].Cells[FpSpread4.Sheets[0].Rows.Count - 2, 0].HorizontalAlign = HorizontalAlign.Right;
                        //FpSpread4.Sheets[0].Cells[FpSpread4.Sheets[0].Rows.Count - 2, 0].ForeColor = Color.Blue;
                        //FpSpread4.Sheets[0].SpanModel.Add(FpSpread4.Sheets[0].RowCount - 1, 0, 1, FpSpread4.Columns.Count - 1);
                        //FpSpread4.Sheets[0].SpanModel.Add(FpSpread4.Sheets[0].RowCount - 1, 0, 1, FpSpread4.Columns.Count - 1);
                        //FpSpread4.Sheets[0].SpanModel.Add(FpSpread4.Sheets[0].RowCount - 1, 0, 1, 5);
                        //FpSpread4.Sheets[0].Cells[FpSpread4.Sheets[0].Rows.Count - 1, 0].Text = "Grand Total Average";
                        //FpSpread4.Sheets[0].Cells[FpSpread4.Sheets[0].Rows.Count - 1, 0].HorizontalAlign = HorizontalAlign.Right;
                        //FpSpread4.Sheets[0].Cells[FpSpread4.Sheets[0].Rows.Count - 1, 0].ForeColor = Color.Blue;
                        FpSpread4.Sheets[0].Cells[FpSpread4.Sheets[0].Rows.Count - 1, 0].Text = "Grand Total";
                        FpSpread4.Sheets[0].Cells[FpSpread4.Sheets[0].RowCount - 1, 5].HorizontalAlign = HorizontalAlign.Center;
                        FpSpread4.Sheets[0].Cells[FpSpread4.Sheets[0].Rows.Count - 1, 5].Text = Convert.ToString(GrandTota);
                        FpSpread4.Sheets[0].Cells[FpSpread4.Sheets[0].Rows.Count - 1, 0].ForeColor = System.Drawing.Color.Blue;
                        FpSpread4.Sheets[0].SpanModel.Add(FpSpread4.Sheets[0].RowCount - 1, 0, 1, 5);
                        FpSpread4.Sheets[0].RowCount++;
                        FpSpread4.Sheets[0].Cells[FpSpread4.Sheets[0].Rows.Count - 1, 0].Text = "Grand Total Average";
                        FpSpread4.Sheets[0].Cells[FpSpread4.Sheets[0].Rows.Count - 1, 5].Text = Convert.ToString(totalavg);
                        FpSpread4.Sheets[0].Cells[FpSpread4.Sheets[0].RowCount - 1, 5].HorizontalAlign = HorizontalAlign.Center;
                        FpSpread4.Sheets[0].Cells[FpSpread4.Sheets[0].Rows.Count - 1, 0].ForeColor = System.Drawing.Color.Blue;
                        FpSpread4.Sheets[0].SpanModel.Add(FpSpread4.Sheets[0].RowCount - 1, 0, 1, 5);
                        //FpSpread4.Sheets[0].SpanModel.Add(FpSpread4.Sheets[0].RowCount - 1, 5, 1, FpSpread4.Columns.Count - 1);
                    }
                    else
                    {
                        imgdiv2.Visible = true;
                        lbl_alert1.Text = "No Records Found";
                        FpSpread4.Visible = false;
                        rptprint1.Visible = false;
                        stud_div.Visible = false;
                    }
                }
                else
                {
                    //div1.Visible = false;
                    imgdiv2.Visible = true;
                    lbl_alert1.Text = "No Records Found";
                    FpSpread4.Visible = false;
                    rptprint1.Visible = false;
                    stud_div.Visible = false;
                }
            }
            FpSpread4.Width = (int)wid;
            FpSpread4.SaveChanges();
            FpSpread4.Sheets[0].PageSize = FpSpread4.Sheets[0].RowCount;
        }
        catch (Exception ex)
        {
            d2.sendErrorMail(ex, collegecode1, "FeedbackReport");
        }
    }

    protected void ddl_staffname_SelectedIndexChanged(object sender, EventArgs e)
    {
        stfSubject();
    }

    public void chartfalse()
    {
        lbl_staffname.Visible = false;
        txt_staffname.Visible = false;
        lbl_question.Visible = false;
        txt_question.Visible = false;
        Panel_question.Visible = false;
        Panel_staffname.Visible = false;
        chart_selct.Visible = false;
        UpdatePanel3.Visible = false;
        lbl_stfsubject.Visible = false;
        UpdatePanel17.Visible = false;
        Txt_stfSubject.Visible = false;
    }

    public void format2false()
    {
        cb_total.Visible = false;
        cb_avg.Visible = false;
        rb_subject.Visible = false;
        form_2.Visible = false;
        UpdatePanel5.Visible = true;
        UpdatePanel3.Visible = false;
    }

    public void chart()
    {
        chart_selct.Visible = true;
        Panel_question.Visible = true;
        Panel_staffname.Visible = true;
        lbl_staffname.Visible = true;
        txt_staffname.Visible = true;
        lbl_question.Visible = true;
        txt_question.Visible = true;
    }

    protected void btn_go_Click(object sender, EventArgs e)
    {
        try
        {
            lbl_headig.Text = "";
            //div1.Visible = true;
            question_chart.Visible = false;
            staff_chart.Visible = false;
            Total_points.Visible = false;
            rb_linchart.Visible = false;
            rb_barchart.Visible = false;
            FpSpread4.Visible = false;
            if (ddl_Feedbackname.SelectedItem.Text == "Select")
            {
                // div1.Visible = false;
                imgdiv2.Visible = true;
                lbl_alert1.Text = "Please Select Feedback Name";
                FpSpread1.Visible = false;
                rptprint1.Visible = false;
                return;
            }
            if (rb_Acad.Checked == true)
            {
                if (rb_login.Checked == true)
                {
                    if (rb_farmate1.Checked == true)
                    {
                        lbl_headig.Text = "Staff Wise Report";
                        format1();
                    }
                    else if (rb_farmate2.Checked == true)
                    {
                        lbl_headig.Text = "Detailed Subject Wise Cumulative Report";
                        if (rb_subject.Checked == true)
                        {
                            format2();
                        }
                        else
                        {
                            format2a();
                        }
                    }
                    else if (rb_farmate3.Checked == true)
                    {
                        lbl_headig.Text = "Student Count Report";
                        format3();
                    }
                    else if (rb_farmate4.Checked == true)
                    {
                        lbl_headig.Text = "Staff Percentage Chart";
                        format4();
                    }
                    else if (rb_farmate5.Checked == true)
                    {
                        lbl_headig.Text = "Questionwise Performance Chart";
                        format5();
                    }
                    else if (rb_farmate6.Checked == true)
                    {
                        lbl_headig.Text = " Questionwise Average Chart.";
                        format6();
                    }
                    else if (rb_farmate7.Checked == true)
                    {
                        lbl_headig.Text = " Individual students wise Report.";
                        format7();
                    }
                }
            }
        }
        catch
        {
        }
    }

    protected void btn_goanonymous_Click(object sender, EventArgs e)
    {
        lbl_headig.Text = ""; lbl_headig.Visible = true;
        if (rb_anonymous.Checked == true)
        {
            if (rb_anonyms_farmate1.Checked == true)
            {
                LblSec.Visible = false;//Added by saranya
                UpSec.Visible = false;
                lbl_headig.Text = " Questionwise Total Points";
                gendformat1ananames();
            }
            else if (rb_anonyms_farmate2.Checked == true)
            {
                //gendformat2ananames();
                if (rb_anonymsubject.Checked == true)
                {
                    LblSec.Visible = false;//Added by saranya
                    UpSec.Visible = false;
                    lbl_headig.Text = " Subject Wise Staff Average .";
                    gendformat2ananames();
                }
                else if (rb_anonymcummulativ.Checked == true)
                {
                    LblSec.Visible = false;//Added by saranya
                    UpSec.Visible = false;
                    lbl_headig.Text = " Questionwise and Staffwise  Average Report ";
                    gendformat3ananames();
                }
            }
            else if (rb_anonyms_farmate3.Checked == true)
            {
                LblSec.Visible = false;//Added by saranya
                UpSec.Visible = false;
                lbl_headig.Text = "Student Count Report";
                ananames3();
            }
            //02.08.16
            else if (rb_anonyms_farmate4.Checked == true)
            {
                LblSec.Visible = false;//Added by saranya
                UpSec.Visible = false;
                lbl_headig.Text = "Staff Percentage Chart";
                ananames4();
            }
            else if (rb_anonyms_farmate5.Checked == true)
            {
                LblSec.Visible = false;//Added by saranya
                UpSec.Visible = false;
                lbl_headig.Text = "Questionwise Performance Chart";
                ananames5();
            }
            else if (rb_anonyms_farmate6.Checked == true)
            {
                //lbl_headig.Text = "Staff Evaluation Report ";delsi1703
                lbl_headig.Text = "Department Wise Feedback ";
                LblSec.Visible = false;//Added by saranya
                UpSec.Visible = false;
                if (cbBarChart.Checked)
                {
                    Ananames6barChart();
                    Ananames6barChartspread();//rajasekar
                }
                else
                    ananames6();
            }
            else if (rb_anonyms_farmate7.Checked == true)
            {
                LblSec.Visible = false;//Added by saranya
                UpSec.Visible = false;
                ananames7();
            }
        }
    }

    protected void btn_go1_Click(object sender, EventArgs e)
    {
        lbl_headig.Text = "";
        //div1.Visible = true;
        question_chart.Visible = false;
        staff_chart.Visible = false;
        Total_points.Visible = false;
        rb_linchart.Visible = false;
        rb_barchart.Visible = false;
        if (ddl_Feedbackname.SelectedItem.Text == "--Select--")
        {
            //div1.Visible = false;
            imgdiv2.Visible = true;
            lbl_alert1.Text = "Please Select Feedback Name";
            FpSpread1.Visible = false;
            rptprint1.Visible = false;
            return;
        }
        if (rb_gen_farmate1.Checked == true)
        {
            lbl_headig.Visible = true;
            lbl_headig.Text = " Questionwise Total Points";
            gendformat2();
        }
        else if (rb_gen_farmate2.Checked == true)
        {
            gendformat3();
        }
    }

    //protected void rb_farmate6_CheckedChanged(object sender, EventArgs e)
    //{
    //    //Txt_Subject.Visible=false;
    //    //lbl_staffname.Visible = false;
    //    //txt_staffname.Visible = false;
    //}

    protected void ddl_Feedbackname_SelectedIndexChanged(object sender, EventArgs e)
    {
        load_questions();
        ddlstaff();
        Subject();
        ddlstaff();
        stfSubject();
        load_staffname();
    }

    protected void btn_search_Click(object sender, EventArgs e)
    {
    }

    protected void btn_ind_search_Click(object sender, EventArgs e)
    {
    }

    public void Cb_stafftype_CheckedChanged(object sender, EventArgs e)
    {
        try
        {
            int cout = 0;
            Txt_stafftype.Text = "--Select--";
            if (Cb_stafftype.Checked == true)
            {
                cout++;
                for (int i = 0; i < Cbl_stafftype.Items.Count; i++)
                {
                    Cbl_stafftype.Items[i].Selected = true;
                }
                Txt_stafftype.Text = "staff type(" + (Cbl_stafftype.Items.Count) + ")";
                load_staffname();
            }
            else
            {
                for (int i = 0; i < Cbl_stafftype.Items.Count; i++)
                {
                    Cbl_stafftype.Items[i].Selected = false;
                }
                Txt_stafftype.Text = "--Select--";
            }
        }
        catch (Exception ex)
        {
        }
    }

    public void Cbl_stafftype_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            int commcount = 0;
            Cb_stafftype.Checked = false;
            for (int i = 0; i < Cbl_stafftype.Items.Count; i++)
            {
                if (Cbl_stafftype.Items[i].Selected == true)
                {
                    commcount = commcount + 1;
                    Cb_stafftype.Checked = false;
                }
            }
            if (commcount > 0)
            {
                if (commcount == Cbl_stafftype.Items.Count)
                {
                    Cb_stafftype.Checked = true;
                }
                Txt_stafftype.Text = "Staff_type(" + commcount.ToString() + ")";
            }
            //bindhostelname();
        }
        catch (Exception ex)
        {
        }
        load_staffname();
    }

    public void bindstafftype()
    {
        try
        {
            Txt_stafftype.Text = "---Select---";
            Cb_stafftype.Checked = false;
            string collvalue = college;
            string stafftype = "";
            if (collvalue == "---Select---")
            {
                collvalue = Session["collegecode"].ToString();
            }
            Cbl_stafftype.Items.Clear();
            ds.Clear();
            stafftype = "  select distinct category_code,category_name from staffcategorizer";
            ds = d2.select_method_wo_parameter(stafftype, "Text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                Cbl_stafftype.DataSource = ds;
                Cbl_stafftype.DataTextField = "category_name";
                Cbl_stafftype.DataValueField = "category_code";
                Cbl_stafftype.DataBind();
                for (int i = 0; i < Cbl_stafftype.Items.Count; i++)
                {
                    Cbl_stafftype.Items[i].Selected = true;
                }
                Txt_stafftype.Text = "CategoryName(" + Cbl_stafftype.Items.Count + ")";
                Cb_stafftype.Checked = true;
            }
        }
        catch (Exception)
        {
        }
    }

    public void binddept(string scollege)
    {
        try
        {
            int height = 0;
            ds = d2.loaddepartment(scollege);
            if (ds.Tables[0].Rows.Count > 0)
            {
                cbl_branch1.DataSource = ds;
                cbl_branch1.DataTextField = "dept_name";
                cbl_branch1.DataValueField = "Dept_Code";
                cbl_branch1.DataBind();
            }
            for (int i = 0; i < cbl_branch1.Items.Count; i++)
            {
                cbl_branch1.Items[i].Selected = true;
                height++;
            }
            txt_branch1.Text = "Department(" + cbl_branch1.Items.Count + ")";
            cb_branch1.Checked = true;
        }
        catch (Exception e)
        {
        }
    }

    public void binddesig(string coll)
    {
        try
        {
            //height = 0;
            cbl_Designation.Visible = true;
            cbl_Designation.Items.Clear();
            ds.Clear();
            string col = coll;
            if (col == "---Select---")
            {
                col = Session["collegecode"].ToString();
            }
            txt_Designation.Text = "---Select---";
            cb_Designation.Checked = false;
            ds = d2.loaddesignation(col);
            if (ds.Tables[0].Rows.Count > 0)
            {
                cbl_Designation.DataSource = ds;
                cbl_Designation.DataTextField = "desig_name";
                cbl_Designation.DataValueField = "Desig_Code";
                cbl_Designation.DataBind();
                for (int i = 0; i < cbl_Designation.Items.Count; i++)
                {
                    cbl_Designation.Items[i].Selected = true;
                }
                txt_Designation.Text = "Designation(" + cbl_Designation.Items.Count + ")";
                cb_Designation.Checked = true;
            }
        }
        catch (Exception e)
        {
        }
    }

    public void cb_Designation_CheckedChanged(object sender, EventArgs e)
    {
        try
        {
            int count = 0;
            txt_Designation.Text = "--Select--";
            if (cb_Designation.Checked == true)
            {
                count++;
                for (int i = 0; i < cbl_Designation.Items.Count; i++)
                {
                    cbl_Designation.Items[i].Selected = true;
                }
                txt_Designation.Text = "Degree(" + (cbl_Designation.Items.Count) + ")";
            }
            else
            {
                for (int i = 0; i < cbl_Designation.Items.Count; i++)
                {
                    cbl_Designation.Items[i].Selected = false;
                }
                txt_Designation.Text = "--Select--";
            }
            bindbranch1();
            //Subject();
        }
        catch (Exception ex)
        {
        }
    }

    public void cbl_Designation_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            int i = 0;
            int commcount = 0;
            cb_Designation.Checked = false;
            txt_Designation.Text = "--Select--";
            for (i = 0; i < cbl_Designation.Items.Count; i++)
            {
                if (cbl_Designation.Items[i].Selected == true)
                {
                    commcount = commcount + 1;
                }
            }
            if (commcount > 0)
            {
                if (commcount == cbl_Designation.Items.Count)
                {
                    cb_Designation.Checked = true;
                }
                txt_Designation.Text = "Designation (" + commcount.ToString() + ")";
            }
            bindbranch1();
            //Subject();
        }
        catch (Exception ex)
        {
        }
    }

    public void BindDesignation()
    {
        try
        {
            cbl_Designation.Items.Clear();
            string Year = "";
            if (Year != "")
            {
                ds = d2.BindDegree(singleuser, group_user, collegecode1, usercode);
                int count1 = ds.Tables[0].Rows.Count;
                if (count1 > 0)
                {
                    cbl_Designation.DataSource = ds;
                    cbl_Designation.DataTextField = "course_name";
                    cbl_Designation.DataValueField = "course_id";
                    cbl_Designation.DataBind();
                    if (cbl_Designation.Items.Count > 0)
                    {
                        for (int row = 0; row < cbl_Designation.Items.Count; row++)
                        {
                            cbl_Designation.Items[row].Selected = true;
                        }
                        cb_Designation.Checked = true;
                        txt_Designation.Text = "Department(" + cbl_Designation.Items.Count + ")";
                    }
                }
            }
            else
            {
                cb_Designation.Checked = false;
                txt_Designation.Text = "--Select--";
            }
        }
        catch (Exception ex)
        {
        }
    }

    public void cb_branch1_CheckedChanged(object sender, EventArgs e)
    {
        try
        {
            int cout = 0;
            txt_branch1.Text = "--Select--";
            if (cb_branch1.Checked == true)
            {
                cout++;
                for (int i = 0; i < cbl_branch1.Items.Count; i++)
                {
                    cbl_branch1.Items[i].Selected = true;
                }
                txt_branch1.Text = "Branch(" + (cbl_branch1.Items.Count) + ")";
            }
            else
            {
                for (int i = 0; i < cbl_branch1.Items.Count; i++)
                {
                    cbl_branch1.Items[i].Selected = false;
                }
                txt_branch1.Text = "--Select--";
            }
            load_staffname();
            //Subject();
        }
        catch (Exception ex)
        {
        }
    }

    public void cbl_branch1_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            cbl_staffname.Items.Clear();
            int commcount = 0;
            cb_branch1.Checked = false;
            txt_branch1.Text = "--Select--";
            int commcount1 = 0;
            for (int i = 0; i < cbl_branch1.Items.Count; i++)
            {
                if (cbl_branch1.Items[i].Selected == true)
                {
                    commcount = commcount + 1;
                }
            }
            if (commcount > 0)
            {
                if (commcount == cbl_branch1.Items.Count)
                {
                    cb_branch1.Checked = true;
                }
                txt_branch1.Text = "Branch(" + commcount.ToString() + ")";
            }
            load_staffname();
            //Subject();
        }
        catch (Exception ex)
        {
        }
    }

    public void bindbranch1()
    {
        try
        {
            cbl_branch1.Items.Clear();
            string course_id = "";
            if (cbl_Designation.Items.Count > 0)
            {
                for (int row = 0; row < cbl_Designation.Items.Count; row++)
                {
                    if (cbl_Designation.Items[row].Selected == true)
                    {
                        if (course_id == "")
                        {
                            course_id = Convert.ToString(cbl_Designation.Items[row].Value);
                        }
                        else
                        {
                            course_id = course_id + "," + Convert.ToString(cbl_Designation.Items[row].Value);
                        }
                    }
                }
            }
            if (course_id != "")
            {
                ds = d2.BindBranchMultiple(singleuser, group_user, course_id, collegecode1, usercode);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    cbl_branch1.DataSource = ds;
                    cbl_branch1.DataTextField = "dept_name";
                    cbl_branch1.DataValueField = "degree_code";
                    cbl_branch1.DataBind();
                    if (cbl_branch1.Items.Count > 0)
                    {
                        for (int row = 0; row < cbl_branch1.Items.Count; row++)
                        {
                            cbl_branch1.Items[row].Selected = true;
                        }
                        cb_branch1.Checked = true;
                        txt_branch1.Text = "Branch(" + cbl_branch1.Items.Count + ")";
                    }
                }
            }
            else
            {
                cb_branch1.Checked = false;
                txt_branch1.Text = "--Select--";
            }
        }
        catch (Exception ex)
        {
        }
    }

    public void fair()
    {
        FpSpread1.Visible = false;
        FpSpread3.Visible = false;
        FpSpread2.Visible = false;
    }

    public void visibletr()
    {
        lbl_college.Enabled = true;
        Txt_college.Enabled = true;
        lbl_Batchyear.Enabled = true;
        txt_batch.Enabled = true;
        lbl_Degree.Enabled = true;
        txt_degree.Enabled = true;
        lbl_dpt.Enabled = true;
        txt_branch.Enabled = true;
        lbl_sem.Enabled = true;
        txt_sem.Enabled = true;
        lbl_Sec.Enabled = true;
        txt_sec.Enabled = true;
        Txt_Subject.Enabled = true;
        lbl_subject.Enabled = true;
    }

    public void visiblefalse()
    {
        lbl_college.Enabled = false;
        Txt_college.Enabled = false;
        lbl_Batchyear.Enabled = false;
        txt_batch.Enabled = false;
        lbl_Degree.Enabled = false;
        txt_degree.Enabled = false;
        lbl_dpt.Enabled = false;
        txt_branch.Enabled = false;
        lbl_sem.Enabled = false;
        txt_sem.Enabled = false;
        lbl_Sec.Enabled = false;
        txt_sec.Enabled = false;
        Txt_Subject.Enabled = false;
        lbl_subject.Enabled = false;
    }

    public string ConvertedMark(string txtConvertTo, string maxMark, ref string obtainedMark)
    {
        Double Mark, max;
        bool r = double.TryParse(obtainedMark, out Mark);
        bool maxflag = double.TryParse(txtConvertTo, out max);
        double multiply;
        if (maxflag)
        {
            if (r)
            {
                switch (txtConvertTo)
                {
                    default:
                        //multiply = double.Parse(txtConvertTo) / int.Parse(maxMark);
                        //obtainedMark = Convert.ToString(Mark * multiply);
                        //Double ss = (Math.Round(obtainedMark, 1));
                        //break;
                        multiply = double.Parse(txtConvertTo) / double.Parse(maxMark);
                        Double ss = (Mark * multiply);
                        obtainedMark = Convert.ToString(Math.Round(ss, 2));
                        try
                        {
                            //obtainedMark = obtainedMark.Split('.')[0] + "." + obtainedMark.Split('.')[1].Substring(0, 2);
                        }
                        catch
                        {
                        }
                        break;
                }
            }
            maxMark = txtConvertTo;
        }
        return obtainedMark;
    }

    protected void rb_anonyms_farmate1_CheckedChanged(object sender, EventArgs e)
    {
        anonymousfilter1.Visible = true;
        anonymousfilter2.Visible = true;
        anonymousfilter3.Visible = false;
        txtfrom_range.Text = "";
        txtto_range.Text = "";
        ddl_criter.SelectedIndex = 0;
        Total_points.Visible = false;
        rptprint1.Visible = false;
        chartprint.Visible = false;
        question_chart.Visible = false;
        staff_chart.Visible = false;
        FpSpread1.Visible = false;
        FpSpread2.Visible = false;
        FpSpread3.Visible = false;
        FpSpread4.Visible = false;
        anonym_form1.Visible = false;
        anoynosformate4.Visible = false;
        cb_avgcolumn.Visible = false;
        LblSec.Visible = false;
        UpSec.Visible = false;
    }

    public void stfSubject()
    {
        try
        {
            ds.Clear();
            string Year = "";
            if (cbl_batch.Items.Count > 0)
            {
                for (int i = 0; i < cbl_batch.Items.Count; i++)
                {
                    if (cbl_batch.Items[i].Selected == true)
                    {
                        if (Year == "")
                        {
                            Year = Convert.ToString(cbl_batch.Items[i].Value);
                        }
                        else
                        {
                            Year = Year + "','" + Convert.ToString(cbl_batch.Items[i].Value);
                        }
                    }
                }
            }
            string branchcode = "";
            if (cbl_branch.Items.Count > 0)
            {
                for (int i = 0; i < cbl_branch.Items.Count; i++)
                {
                    if (cbl_branch.Items[i].Selected == true)
                    {
                        if (branchcode == "")
                        {
                            branchcode = Convert.ToString(cbl_branch.Items[i].Value);
                        }
                        else
                        {
                            branchcode = branchcode + "','" + Convert.ToString(cbl_branch.Items[i].Value);
                        }
                    }
                }
            }
            string sem = "";
            if (cbl_sem.Items.Count > 0)
            {
                for (int i = 0; i < cbl_sem.Items.Count; i++)
                {
                    if (cbl_sem.Items[i].Selected == true)
                    {
                        if (sem == "")
                        {
                            sem = Convert.ToString(cbl_sem.Items[i].Value);
                        }
                        else
                        {
                            sem = sem + "','" + Convert.ToString(cbl_sem.Items[i].Value);
                        }
                    }
                }
            }
            string college_cd = "";
            if (Cbl_college.Items.Count > 0)
            {
                for (int i = 0; i < Cbl_college.Items.Count; i++)
                {
                    if (Cbl_college.Items[i].Selected == true)
                    {
                        if (college_cd == "")
                        {
                            college_cd = "" + Cbl_college.Items[i].Value.ToString() + "";
                        }
                        else
                        {
                            college_cd = college_cd + "','" + Convert.ToString(Cbl_college.Items[i].Value);
                        }
                    }
                }
            }
            string section = "";
            if (cbl_sec.Items.Count > 0)
            {
                for (int i = 0; i < cbl_sec.Items.Count; i++)
                {
                    if (cbl_sec.Items[i].Selected == true)
                    {
                        if (section == "")
                        {
                            section = Convert.ToString(cbl_sec.Items[i].Value);
                        }
                        else
                        {
                            section = section + "','" + Convert.ToString(cbl_sec.Items[i].Value);
                        }
                        if (cbl_sec.Items[i].Value == "Empty")
                        {
                            section = "";
                        }
                    }
                }
            }
            if (section.Trim() != "")
            {
                section = section + "','";
            }
            string type = "";
            if (rb_Acad.Checked == true)
            {
                type = "1";
            }
            else if (rb_Gend.Checked == true)
            {
                type = "2";
            }
            Cbl_StfSubject.Items.Clear();
            string sub_name = "";
            if (rb_Acad.Checked == true)
            {
                string st_type = d2.GetFunction(" select top 1 Subject_Type from CO_FeedBackMaster where FeedBackName ='" + ddl_Feedbackname.SelectedItem.Value + "'");
                //string  st_type = ds.Tables[4].Rows[0]["Subject_Type"].ToString();
                string sub_type = "";
                string[] split = st_type.Split(',');
                for (int i = 0; i < split.Length; i++)
                {
                    if (sub_type == "")
                    {
                        sub_type = split[i];
                    }
                    else
                    {
                        sub_type += "','" + split[i];
                    }
                }
                //sub_name = "select distinct su.subject_code,su.subject_name  from staff_selector ss,staffmaster s,subject su,sub_sem sm where ss.staff_code =s.staff_code and ss.subject_no =su.subject_no and su.subType_no =sm.subType_no and ss.staff_code in and ss.batch_year in ('" + Year + "') and ss.Sections in ('" + section + "') and sm.subject_type in ('" + sub_type + "')  ";
                sub_name = "select distinct su.subject_code,su.subject_name  from staff_selector ss,staffmaster s,subject su,sub_sem sm where ss.staff_code =s.staff_code and ss.subject_no =su.subject_no and su.subType_no =sm.subType_no and ss.staff_code in ('" + ddl_staffname.SelectedItem.Value + "')  and ss.batch_year in ('" + Year + "') and ss.Sections in ('" + section + "') and sm.subject_type in ('" + sub_type + "')";
            }
            else if (rb_Gend.Checked == true)
            {
            }
            ds = d2.select_method_wo_parameter(sub_name, "Text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                Cbl_StfSubject.DataSource = ds;
                Cbl_StfSubject.DataTextField = "subject_name";
                Cbl_StfSubject.DataValueField = "subject_code";
                Cbl_StfSubject.DataBind();
            }
            if (Cbl_StfSubject.Items.Count > 0)
            {
                for (int row = 0; row < Cbl_StfSubject.Items.Count; row++)
                {
                    Cbl_StfSubject.Items[row].Selected = true;
                    Cb_StfSubject.Checked = true;
                }
                Txt_stfSubject.Text = "Subject(" + Cbl_StfSubject.Items.Count + ")";
            }
            else
            {
                Txt_stfSubject.Text = "--Select--";
            }
        }
        catch (Exception ex)
        {
        }
    }

    protected void Cb_StfSubject_CheckedChanged(object sender, EventArgs e)
    {
        if (Cb_StfSubject.Checked == true)
        {
            for (int i = 0; i < Cbl_StfSubject.Items.Count; i++)
            {
                Cbl_StfSubject.Items[i].Selected = true;
            }
            Txt_stfSubject.Text = "Subject(" + (Cbl_StfSubject.Items.Count) + ")";
        }
        else
        {
            for (int i = 0; i < Cbl_StfSubject.Items.Count; i++)
            {
                Cbl_StfSubject.Items[i].Selected = false;
            }
            Txt_stfSubject.Text = "--Select--";
        }
        load_staffname();
        load_questions();
    }

    protected void Cbl_StfSubject_SelectedIndexChanged(object sender, EventArgs e)
    {
        Txt_stfSubject.Text = "--Select--";
        Cb_StfSubject.Checked = false;
        int commcount = 0;
        for (int i = 0; i < Cbl_StfSubject.Items.Count; i++)
        {
            if (Cbl_StfSubject.Items[i].Selected == true)
            {
                commcount = commcount + 1;
            }
        }
        if (commcount > 0)
        {
            Txt_stfSubject.Text = "Subject(" + commcount.ToString() + ")";
            if (commcount == Cbl_StfSubject.Items.Count)
            {
                Cb_StfSubject.Checked = true;
            }
        }
        load_questions();
    }

    protected void rb_anonyms_farmate2_CheckedChanged(object sender, EventArgs e)
    {
        anonymousfilter1.Visible = true;
        anonymousfilter2.Visible = true;
        anonymousfilter3.Visible = false;
        txtfrom_range.Text = "";
        txtto_range.Text = "";
        ddl_criter.SelectedIndex = 0;
        Total_points.Visible = false;
        rptprint1.Visible = false;
        chartprint.Visible = false;
        question_chart.Visible = false;
        staff_chart.Visible = false;
        FpSpread1.Visible = false;
        FpSpread2.Visible = false;
        FpSpread3.Visible = false;
        FpSpread4.Visible = false;
        lbl_subject.Visible = true;
        anonym_form1.Visible = true;
        rb_anonymsubject.Checked = true;
        rb_anonymcummulativ.Checked = false;
        UpdatePanel5.Visible = true;
        rb_anonymcummulativ.Checked = false;
        cb_total1.Visible = false;
        cb_avg1.Visible = false;
        anoynosformate4.Visible = false;
        cb_avgcolumn.Visible = true;
        cbl_batch.ClearSelection();
        cbl_batch_SelectedIndexChanged(sender, e);
        LblSec.Visible = false;
        UpSec.Visible = false;
    }

    protected void rb_anonymcummulativ_CheckedChanged(object sender, EventArgs e)
    {
        cb_total1.Visible = true;
        cb_avg1.Visible = true;
        cb_total1.Checked = true;
        cb_avg1.Checked = true;
        cb_avgcolumn.Visible = false;
        //form_2.Visible = true;
    }

    protected void rb_anonymsubject_CheckedChanged(object sender, EventArgs e)
    {
        cb_total1.Visible = false;
        cb_avg1.Visible = false;
        //form_2.Visible = true;
        cb_avgcolumn.Visible = true;
    }

    protected void rb_anonyms_farmate3_CheckedChanged(object sender, EventArgs e)
    {
        anonymousfilter1.Visible = true;
        anonymousfilter2.Visible = true;
        anonymousfilter3.Visible = false;
        txtfrom_range.Text = "";
        txtto_range.Text = "";
        ddl_criter.SelectedIndex = 0;
        Total_points.Visible = false;
        rptprint1.Visible = false;
        chartprint.Visible = false;
        question_chart.Visible = false;
        staff_chart.Visible = false;
        FpSpread1.Visible = false;
        FpSpread2.Visible = false;
        FpSpread3.Visible = false;
        FpSpread4.Visible = false;
        anonym_form1.Visible = false;
        anoynosformate4.Visible = false;
        cb_avgcolumn.Visible = false;
        LblSec.Visible = false;
        UpSec.Visible = false;
    }

    protected void rb_farmate7_CheckedChanged(object sender, EventArgs e)
    {
        rdb_form4staffwise.Visible = false;
        rdb_form4questwise.Visible = false;
        txtfrom_range.Text = "";
        txtto_range.Text = "";
        ddl_criter.SelectedIndex = 0;
        Total_points.Visible = false;
        rptprint1.Visible = false;
        chartprint.Visible = false;
        question_chart.Visible = false;
        staff_chart.Visible = false;
        FpSpread1.Visible = false;
        FpSpread2.Visible = false;
        FpSpread3.Visible = false;
        FpSpread4.Visible = false;
        Panel_question.Visible = true;
        lbl_staffname.Visible = true;
        lbl_question.Visible = true;
        txt_question.Visible = true;
        chart_selct.Visible = false;
        UpdatePanel3.Visible = true;
        UpdatePanel8.Visible = false;
        UpdatePanel5.Visible = false;
        lbl_stfsubject.Visible = true;
        UpdatePanel17.Visible = true;
        Txt_stfSubject.Visible = true;
        cb_avgcolumn.Visible = false;
        ddl_headershow.Visible = false;
        lbl_header.Visible = false;
        btn_printperticulaterstaff.Visible = false;
        LblSec.Visible = false;
        UpSec.Visible = false;
    }

    //02.08.16
    protected void rb_anonyms_farmate4_CheckedChanged(object sender, EventArgs e)
    {
        anonymousfilter1.Visible = true;
        anonymousfilter2.Visible = true;
        anonymousfilter3.Visible = false;
        txtfrom_range.Text = "";
        txtto_range.Text = "";
        ddl_criter.SelectedIndex = 0;
        Total_points.Visible = false;
        rptprint1.Visible = false;
        chartprint.Visible = false;
        question_chart.Visible = false;
        staff_chart.Visible = false;
        FpSpread1.Visible = false;
        FpSpread2.Visible = false;
        FpSpread3.Visible = false;
        FpSpread4.Visible = false;
        chart_staff_chart.Visible = false;
        staff_chart.Visible = false;
        lbl_headig.Visible = true;
        lbl_headig.Text = "Staff Percentage Chart";
        lbl_subject.Visible = true;
        anonym_form1.Visible = false;
        Panel_Subject.Visible = true;
        Txt_Subject.Visible = true;
        UpdatePanel5.Visible = true;
        Panel_staffname.Visible = true;
        lbl_staffname.Visible = true;
        txt_staffname.Visible = true;
        lbl_question.Visible = true;
        txt_question.Visible = true;
        Panel_question.Visible = true;
        anoynosformate4.Visible = true;
        rb_linchart.Visible = true;
        rb_barchart.Visible = true;
        cb_avgcolumn.Visible = false;
        LblSec.Visible = false;
        UpSec.Visible = false;
    }

    protected void rb_anonyms_farmate5_CheckedChanged(object sender, EventArgs e)
    {
        anonymousfilter1.Visible = true;
        anonymousfilter2.Visible = true;
        anonymousfilter3.Visible = false;
        txtfrom_range.Text = "";
        txtto_range.Text = "";
        ddl_criter.SelectedIndex = 0;
        Total_points.Visible = false;
        rptprint1.Visible = false;
        chartprint.Visible = false;
        question_chart.Visible = false;
        staff_chart.Visible = false;
        FpSpread1.Visible = false;
        FpSpread2.Visible = false;
        FpSpread3.Visible = false;
        FpSpread4.Visible = false;
        chart_staff_chart.Visible = false;
        staff_chart.Visible = false;
        lbl_headig.Visible = true;
        lbl_headig.Text = "Questionwise Performance Chart";
        lbl_subject.Visible = true;
        anonym_form1.Visible = false;
        Panel_Subject.Visible = true;
        Txt_Subject.Visible = true;
        UpdatePanel5.Visible = true;
        Panel_staffname.Visible = true;
        lbl_staffname.Visible = true;
        txt_staffname.Visible = true;
        lbl_question.Visible = true;
        txt_question.Visible = true;
        Panel_question.Visible = true;
        anoynosformate4.Visible = true;
        rb_linchart.Visible = true;
        rb_barchart.Visible = true;
        cb_avgcolumn.Visible = false;
        LblSec.Visible = false;
        UpSec.Visible = false;
    }

    protected void ananames4()
    {
        try
        {
            FpSpread3.Visible = false;
            question_chart.Visible = false;
            chartprint.Visible = false;
            chart_staff_chart.Visible = false;
            staff_chart.Visible = false;
            string college_cd = "";
            if (Cbl_college.Items.Count > 0)
            {
                for (int i = 0; i < Cbl_college.Items.Count; i++)
                {
                    if (Cbl_college.Items[i].Selected == true)
                    {
                        if (college_cd == "")
                        {
                            college_cd = "" + Cbl_college.Items[i].Value.ToString() + "";
                        }
                        else
                        {
                            college_cd = college_cd + "','" + Convert.ToString(Cbl_college.Items[i].Value);
                        }
                    }
                }
            }
            string Batch_Year = "";
            for (int i = 0; i < cbl_batch.Items.Count; i++)
            {
                if (cbl_batch.Items[i].Selected == true)
                {
                    if (Batch_Year == "")
                    {
                        Batch_Year = "" + cbl_batch.Items[i].Value.ToString() + "";
                    }
                    else
                    {
                        Batch_Year = Batch_Year + "','" + cbl_batch.Items[i].Value.ToString() + "";
                    }
                }
            }
            string degree_code = "";
            for (int i = 0; i < cbl_branch.Items.Count; i++)
            {
                if (cbl_branch.Items[i].Selected == true)
                {
                    if (degree_code == "")
                    {
                        degree_code = "" + cbl_branch.Items[i].Value.ToString() + "";
                    }
                    else
                    {
                        degree_code = degree_code + "','" + cbl_branch.Items[i].Value.ToString() + "";
                    }
                }
            }
            string section = "";
            for (int i = 0; i < cbl_sec.Items.Count; i++)
            {
                if (cbl_sec.Items[i].Selected == true)
                {
                    if (section == "")
                    {
                        section = "" + cbl_sec.Items[i].Value.ToString() + "";
                    }
                    else
                    {
                        section = section + "','" + cbl_sec.Items[i].Value.ToString() + "";
                    }
                    if (cbl_sec.Items[i].Value == "Empty")
                    {
                        section = "";
                    }
                }
            }
            if (section.Trim() != "")
            {
                section = section + "','";
            }
            string semester = "";
            for (int i = 0; i < cbl_sem.Items.Count; i++)
            {
                if (cbl_sem.Items[i].Selected == true)
                {
                    if (semester == "")
                    {
                        semester = "" + cbl_sem.Items[i].Value.ToString() + "";
                    }
                    else
                    {
                        semester = semester + "','" + cbl_sem.Items[i].Value.ToString() + "";
                    }
                }
            }
            string staffcod = "";
            for (int i = 0; i < cbl_staffname.Items.Count; i++)
            {
                if (cbl_staffname.Items[i].Selected == true)
                {
                    if (staffcod == "")
                    {
                        staffcod = "" + cbl_staffname.Items[i].Value.ToString() + "";
                    }
                    else
                    {
                        staffcod = staffcod + "','" + cbl_staffname.Items[i].Value.ToString() + "";
                    }
                }
            }
            string sub = "";
            for (int i = 0; i < Cbl_Subject.Items.Count; i++)
            {
                if (Cbl_Subject.Items[i].Selected == true)
                {
                    if (sub == "")
                    {
                        sub = "" + Cbl_Subject.Items[i].Value.ToString() + "";
                    }
                    else
                    {
                        sub = sub + "','" + Cbl_Subject.Items[i].Value.ToString() + "";
                    }
                }
            }
            string fbpk = "";
            if (ddl_Feedbackname.SelectedItem.Value != "Select")
            {
                fbpk = " select FeedBackMasterPK from CO_FeedBackMaster where FeedBackName ='" + ddl_Feedbackname.SelectedItem.Value + "' and DegreeCode in ('" + degree_code + "') and semester in ('" + semester + "') and Batch_Year in('" + Batch_Year + "') and section in ('" + section + "')";
            }
            else
            {
                imgdiv2.Visible = true;
                lbl_alert1.Text = "Please Select Feedback Name";
                return;
            }
            dsfb.Clear();
            dsfb = d2.select_method_wo_parameter(fbpk, "Text");
            if (dsfb.Tables[0].Rows.Count > 0)
            {
                string feedbakpk = "";
                if (dsfb.Tables[0].Rows.Count > 0)
                {
                    for (int pk = 0; pk < dsfb.Tables[0].Rows.Count; pk++)
                    {
                        if (feedbakpk == "")
                        {
                            feedbakpk = dsfb.Tables[0].Rows[pk]["FeedBackMasterPK"].ToString();
                        }
                        else
                        {
                            feedbakpk = feedbakpk + "','" + dsfb.Tables[0].Rows[pk]["FeedBackMasterPK"].ToString() + "";
                        }
                    }
                }
                string feedbackq = "  select Staff_Name,sf.subjectno,(Point)as Points,Question,t.staff_code from CO_FeedbackUniCode FU,CO_FeedBackMaster FM,CO_StudFeedBack SF,staff_appl_master A,staffmaster T,CO_MarkMaster M, CO_QuestionMaster Q where Q.QuestionMasterPK =sf.QuestionMasterFK and M.MarkMasterPK =sf.MarkMasterPK and A.appl_no =t.appl_no and a.appl_id =sf.StaffApplNo and fm.FeedBackMasterPK =fu.FeedbackMasterFK and sf.FeedBackMasterFK =fu.FeedbackMasterFK and sf.FeedBackMasterFK =fm.FeedBackMasterPK and sf.FeedbackUnicode =fu.FeedbackUnicode and fu.FeedbackMasterFK in ('" + feedbakpk + "') and FM.CollegeCode in ('" + college_cd + "') and FM.Batch_Year in ('" + Batch_Year + "') and FM.degreecode in ('" + degree_code + "') and FM.semester in ('" + semester + "')  and fm.FeedBackMasterPK in ('" + feedbakpk + "') and staff_code in('" + staffcod + "') ";
                if (section != "")
                {
                    feedbackq = feedbackq + " and FM.Section in ('" + section + "') ";
                }
                ds = d2.select_method_wo_parameter(feedbackq, "Text");
                DataTable dtChart1 = new DataTable();
                DataColumn dc;
                if (ds.Tables[0].Rows.Count > 0)
                {
                    ArrayList arrcol = new ArrayList();
                    for (int q = 0; q < cbl_question.Items.Count; q++)
                    {
                        if (cbl_question.Items[q].Selected == true)
                        {
                            string name = cbl_question.Items[q].Text.ToString();
                            if (!arrcol.Contains(name))
                            {
                                dc = new DataColumn();
                                dc.ColumnName = name;
                                dtChart1.Columns.Add(dc);
                                arrcol.Add(name);
                            }
                        }
                    }
                    DataRow drp;
                    staff_chart.Series.Clear();
                    staff_chart.Titles[0].Text = ("STAFF PERCENTAGE (" + Convert.ToString(ddl_Feedbackname.SelectedItem.Value) + ")");
                    string staffnam = "";
                    string subcode = "";
                    string staffandcode = "";
                    ArrayList stafArr = new ArrayList();
                    ArrayList ques = new ArrayList();
                    int s = 0;
                    for (int r = 0; r < ds.Tables[0].Rows.Count; r++)
                    {
                        staffnam = ds.Tables[0].Rows[r]["Staff_Name"].ToString();
                        subcode = ds.Tables[0].Rows[r]["subjectno"].ToString();
                        staffandcode = staffnam + "-" + subcode;
                        if (!stafArr.Contains(staffandcode))
                        {
                            ds.Tables[0].DefaultView.RowFilter = "Staff_Name='" + staffnam + "' and subjectno='" + subcode + "'";
                            DataView dv = new DataView();
                            DataTable dtques = new DataTable();
                            dv = ds.Tables[0].DefaultView;
                            dtques = dv.ToTable();
                            staff_chart.Series.Add(staffandcode + r.ToString());
                            stafArr.Add(staffandcode);
                            Double poin = 0;
                            Double points1 = 0;
                            Double ps = 0;
                            drp = dtChart1.NewRow();
                            for (int q = 0; q < cbl_question.Items.Count; q++)
                            {
                                ps = 0;
                                if (cbl_question.Items[q].Selected == true)
                                {
                                    dtques.DefaultView.RowFilter = "Question='" + cbl_question.Items[q].Text.ToString() + "'";
                                    dv = dtques.DefaultView;
                                    int w = 0;
                                    for (int pt = 0; pt < dv.Count; pt++)
                                    {
                                        w++;
                                        points1 = Convert.ToDouble(dv[pt]["Points"]);
                                        string points = Convert.ToString(points1);
                                        string needs = "5";
                                        string sum_total = d2.GetFunction("select top 1 Point from CO_MarkMaster  where CollegeCode in('" + college_cd + "') order by Point desc");
                                        ConvertedMark(needs, sum_total, ref points);
                                        string aaa = Convert.ToString(points);
                                        poin = Convert.ToDouble(aaa);
                                        ps = ps + poin;
                                    }
                                    ps = ps / w;
                                    string ps1 = Convert.ToString(Math.Round(ps, 1));
                                    if (ddl_criter.SelectedItem.Value != "0")
                                    {
                                        if (ddl_criter.SelectedItem.Value == "1")
                                        {
                                            if (txtfrom_range.Text.Trim() != "" && txtto_range.Text.Trim() != "")
                                            {
                                                if (Convert.ToDouble(txtfrom_range.Text.Trim()) <= Convert.ToDouble(ps1) && Convert.ToDouble(ps1) <= Convert.ToDouble(txtto_range.Text.Trim()))
                                                {
                                                    drp[cbl_question.Items[q].Text.ToString()] = ps1;
                                                }
                                            }
                                            else
                                            {
                                                staff_chart.Visible = false;
                                                chart_staff_chart.Visible = false;
                                                imgdiv2.Visible = true;
                                                lbl_alert1.Text = "Please Enter Range";
                                            }
                                        }
                                        else
                                        {
                                            drp[cbl_question.Items[q].Text.ToString()] = ps1;
                                        }
                                    }
                                    if (ddl_criter.SelectedItem.Value == "0")
                                    {
                                        drp[cbl_question.Items[q].Text.ToString()] = ps1;
                                    }
                                }
                            }
                            dtChart1.Rows.Add(drp);
                        }
                    }
                    if (dtChart1.Rows.Count > 0)
                    {
                        string srr = ""; Hashtable removedthash = new Hashtable(); string val = ""; double finalvalue = 0;
                        for (int cc = 0; cc < dtChart1.Columns.Count; cc++)
                        {
                            finalvalue = 0;
                            for (int rr = 0; rr < dtChart1.Rows.Count; rr++)
                            {
                                srr = dtChart1.Columns[cc].ToString();
                                val = dtChart1.Rows[rr][cc].ToString();
                                if (val.Trim() == "" || val.Trim().ToUpper() == "NAN")
                                    val = "0";
                                finalvalue = finalvalue + Convert.ToDouble(val);
                            }
                            if (finalvalue == 0)
                            {
                                removedthash.Add(cc, srr);
                            }
                        }
                        if (removedthash.Count > 0)
                        {
                            foreach (DictionaryEntry col in removedthash)
                            {
                                dtChart1.Columns.Remove(Convert.ToString(col.Value));
                            }
                        }
                        staff_chart.RenderType = RenderType.ImageTag;
                        staff_chart.ImageType = ChartImageType.Png;
                        staff_chart.ImageStorageMode = ImageStorageMode.UseImageLocation;
                        staff_chart.ImageLocation = Path.Combine("~/college/", "feedbackchart");
                        if (dtChart1.Columns.Count > 0)
                        {
                            int chartwidth = 0;
                            for (int r = 0; r < dtChart1.Rows.Count; r++)
                            {
                                for (int c = 0; c < dtChart1.Columns.Count; c++)
                                {
                                    staff_chart.Series[r].Points.AddXY(dtChart1.Columns[c].ToString(), dtChart1.Rows[r][c].ToString());
                                    staff_chart.ChartAreas[0].AxisX.LineColor = System.Drawing.Color.Black;
                                    staff_chart.ChartAreas[0].AxisY.LineColor = System.Drawing.Color.Black;
                                    staff_chart.Series[r].IsValueShownAsLabel = true;
                                    staff_chart.Series[r].IsXValueIndexed = true;
                                    if (rdb_line.Checked == true)
                                    {
                                        staff_chart.Series[r].ChartType = SeriesChartType.Line;
                                    }
                                    staff_chart.ChartAreas[0].AxisX.LabelStyle.Angle = -90;
                                    staff_chart.ChartAreas[0].AxisX.LabelStyle.Interval = 1;
                                    chartwidth += 15;
                                }
                            }
                            chart_staff_chart.Visible = true;
                            if (chartwidth <= 1125)
                                staff_chart.Width = chartwidth;
                            else
                                staff_chart.Width = 1125;
                            staff_chart.Visible = true;
                            chartprint.Visible = true;
                        }
                        else
                        {
                            chartprint.Visible = false;
                            imgdiv2.Visible = true;
                            lbl_alert1.Text = "Please Enter Valid Range";
                        }
                    }
                }
                else
                {
                    staff_chart.Visible = false;
                    chart_staff_chart.Visible = false;
                    imgdiv2.Visible = true;
                    lbl_alert1.Text = "No Records Found";
                }
            }
        }
        catch (Exception ex)
        {
            d2.sendErrorMail(ex, collegecode1, "FeedbackReport");
        }
    }

    protected void ananames5()
    {
        try
        {
            FpSpread3.Visible = false;
            question_chart.Visible = false;
            chartprint.Visible = false;
            chart_staff_chart.Visible = false;
            staff_chart.Visible = false;
            string college_cd = "";
            if (Cbl_college.Items.Count > 0)
            {
                for (int i = 0; i < Cbl_college.Items.Count; i++)
                {
                    if (Cbl_college.Items[i].Selected == true)
                    {
                        if (college_cd == "")
                        {
                            college_cd = "" + Cbl_college.Items[i].Value.ToString() + "";
                        }
                        else
                        {
                            college_cd = college_cd + "','" + Convert.ToString(Cbl_college.Items[i].Value);
                        }
                    }
                }
            }
            string Batch_Year = "";
            for (int i = 0; i < cbl_batch.Items.Count; i++)
            {
                if (cbl_batch.Items[i].Selected == true)
                {
                    if (Batch_Year == "")
                    {
                        Batch_Year = "" + cbl_batch.Items[i].Value.ToString() + "";
                    }
                    else
                    {
                        Batch_Year = Batch_Year + "','" + cbl_batch.Items[i].Value.ToString() + "";
                    }
                }
            }
            string degree_code = "";
            for (int i = 0; i < cbl_branch.Items.Count; i++)
            {
                if (cbl_branch.Items[i].Selected == true)
                {
                    if (degree_code == "")
                    {
                        degree_code = "" + cbl_branch.Items[i].Value.ToString() + "";
                    }
                    else
                    {
                        degree_code = degree_code + "','" + cbl_branch.Items[i].Value.ToString() + "";
                    }
                }
            }
            string section = "";
            for (int i = 0; i < cbl_sec.Items.Count; i++)
            {
                if (cbl_sec.Items[i].Selected == true)
                {
                    if (section == "")
                    {
                        section = "" + cbl_sec.Items[i].Value.ToString() + "";
                    }
                    else
                    {
                        section = section + "','" + cbl_sec.Items[i].Value.ToString() + "";
                    }
                    if (cbl_sec.Items[i].Value == "Empty")
                    {
                        section = "";
                    }
                }
            }
            if (section.Trim() != "")
            {
                section = section + "','";
            }
            string semester = "";
            for (int i = 0; i < cbl_sem.Items.Count; i++)
            {
                if (cbl_sem.Items[i].Selected == true)
                {
                    if (semester == "")
                    {
                        semester = "" + cbl_sem.Items[i].Value.ToString() + "";
                    }
                    else
                    {
                        semester = semester + "','" + cbl_sem.Items[i].Value.ToString() + "";
                    }
                }
            }
            string staffcod = "";
            for (int i = 0; i < cbl_staffname.Items.Count; i++)
            {
                if (cbl_staffname.Items[i].Selected == true)
                {
                    if (staffcod == "")
                    {
                        staffcod = "" + cbl_staffname.Items[i].Value.ToString() + "";
                    }
                    else
                    {
                        staffcod = staffcod + "','" + cbl_staffname.Items[i].Value.ToString() + "";
                    }
                }
            }
            string sub = "";
            for (int i = 0; i < Cbl_Subject.Items.Count; i++)
            {
                if (Cbl_Subject.Items[i].Selected == true)
                {
                    if (sub == "")
                    {
                        sub = "" + Cbl_Subject.Items[i].Value.ToString() + "";
                    }
                    else
                    {
                        sub = sub + "','" + Cbl_Subject.Items[i].Value.ToString() + "";
                    }
                }
            }
            string fbpk = "";
            if (ddl_Feedbackname.SelectedItem.Value != "Select")
            {
                fbpk = " select FeedBackMasterPK from CO_FeedBackMaster where FeedBackName ='" + ddl_Feedbackname.SelectedItem.Value + "' and DegreeCode in ('" + degree_code + "') and semester in ('" + semester + "') and Batch_Year in('" + Batch_Year + "') and section in ('" + section + "')";
            }
            else
            {
                imgdiv2.Visible = true;
                lbl_alert1.Text = "Please Select Feedback Name";
                return;
            }
            dsfb.Clear();
            dsfb = d2.select_method_wo_parameter(fbpk, "Text");
            if (dsfb.Tables[0].Rows.Count > 0)
            {
                string feedbakpk = "";
                if (dsfb.Tables[0].Rows.Count > 0)
                {
                    for (int pk = 0; pk < dsfb.Tables[0].Rows.Count; pk++)
                    {
                        if (feedbakpk == "")
                        {
                            feedbakpk = dsfb.Tables[0].Rows[pk]["FeedBackMasterPK"].ToString();
                        }
                        else
                        {
                            feedbakpk = feedbakpk + "','" + dsfb.Tables[0].Rows[pk]["FeedBackMasterPK"].ToString() + "";
                        }
                    }
                }
                string feedbackq = "  select Staff_Name,sf.subjectno,(Point)as Points,Question,t.staff_code from CO_FeedbackUniCode FU,CO_FeedBackMaster FM,CO_StudFeedBack SF,staff_appl_master A,staffmaster T,CO_MarkMaster M, CO_QuestionMaster Q where Q.QuestionMasterPK =sf.QuestionMasterFK and M.MarkMasterPK =sf.MarkMasterPK and A.appl_no =t.appl_no and a.appl_id =sf.StaffApplNo and fm.FeedBackMasterPK =fu.FeedbackMasterFK and sf.FeedBackMasterFK =fu.FeedbackMasterFK and sf.FeedBackMasterFK =fm.FeedBackMasterPK and sf.FeedbackUnicode =fu.FeedbackUnicode and fu.FeedbackMasterFK in ('" + feedbakpk + "') and FM.CollegeCode in ('" + college_cd + "') and FM.Batch_Year in ('" + Batch_Year + "') and FM.degreecode in ('" + degree_code + "') and FM.semester in ('" + semester + "')  and fm.FeedBackMasterPK in ('" + feedbakpk + "') and staff_code in('" + staffcod + "') ";
                if (section != "")
                {
                    feedbackq = feedbackq + " and FM.Section in ('" + section + "') ";
                }
                ds = d2.select_method_wo_parameter(feedbackq, "Text");
                DataTable dtChart2 = new DataTable();
                DataColumn dc;
                int width = 0;
                if (ds.Tables[0].Rows.Count > 0)
                {
                    DataRow drp;
                    //drp = dtChart1.NewRow();
                    staff_chart.Series.Clear();
                    string questionstr = "";
                    string subcode = "";
                    string staffandcode = "";
                    string staffnam = "";
                    ArrayList stafArr = new ArrayList();
                    string question = "";
                    question_chart.Series.Clear();
                    for (int i = 0; i < cbl_question.Items.Count; i++)
                    {
                        if (cbl_question.Items[i].Selected == true)
                        {
                            question = cbl_question.Items[i].Text.ToString();
                            question_chart.Series.Add(question.Trim());
                        }
                    }
                    question_chart.Titles[0].Text = ("Questions Wise Chart (" + Convert.ToString(ddl_Feedbackname.SelectedItem.Value) + ")");
                    for (int r = 0; r < ds.Tables[0].Rows.Count; r++)
                    {
                        staffnam = ds.Tables[0].Rows[r]["Staff_Name"].ToString();
                        subcode = ds.Tables[0].Rows[r]["subjectno"].ToString();
                        questionstr = ds.Tables[0].Rows[r]["Question"].ToString();
                        staffandcode = staffnam + "-" + subcode;
                        if (!stafArr.Contains(staffandcode))
                        {
                            DataView dv = new DataView();
                            dv = ds.Tables[0].DefaultView;
                            dc = new DataColumn();
                            stafArr.Add(staffandcode);
                            dc.ColumnName = staffandcode;
                            dtChart2.Columns.Add(dc);
                        }
                    }
                    DataRow drpq;
                    //drp = dtChart1.NewRow();
                    string quest = "";
                    int s = 0;
                    for (int i = 0; i < cbl_question.Items.Count; i++)
                    {
                        if (cbl_question.Items[i].Selected == true)
                        {
                            DataTable dtstp = new DataTable();
                            question = cbl_question.Items[i].Text.ToString();
                            ds.Tables[0].DefaultView.RowFilter = "Question='" + question + "'";
                            dtstp = ds.Tables[0].DefaultView.ToTable();
                            drpq = dtChart2.NewRow();
                            ArrayList ques = new ArrayList();
                            subcode = "";
                            staffandcode = "";
                            staffnam = "";
                            for (int st = 0; st < dtstp.Rows.Count; st++)
                            {
                                staffnam = dtstp.Rows[st]["Staff_Name"].ToString();
                                subcode = dtstp.Rows[st]["subjectno"].ToString();
                                staffandcode = staffnam + "-" + subcode;
                                if (!ques.Contains(staffandcode))
                                {
                                    ques.Add(staffandcode);
                                    dtstp.DefaultView.RowFilter = "Staff_Name='" + staffnam + "' and subjectno='" + subcode + "' ";
                                    DataView dv = new DataView();
                                    dv = dtstp.DefaultView;
                                    string points = "";
                                    Double poin = 0;
                                    Double ps = 0;
                                    int w = 0;
                                    for (int c = 0; c < dv.Count; c++)
                                    {
                                        w++;
                                        points = Convert.ToString(dv[c]["Points"]);

                                        // string sum_total = "9";
                                        string sum_total = d2.GetFunction("select top 1 Point from CO_MarkMaster  where CollegeCode in('" + collegecode1 + "') order by Point desc");
                                        string needs = sum_total;
                                        ConvertedMark(needs, sum_total, ref points);
                                        string aaa = Convert.ToString(points);
                                        poin = Convert.ToDouble(aaa);
                                        ps = ps + poin;
                                    }
                                    ps = ps / w;
                                    string ps1 = Convert.ToString(Math.Round(ps, 1));
                                    if (ddl_criter.SelectedItem.Value != "0")
                                    {
                                        if (ddl_criter.SelectedItem.Value == "1")
                                        {
                                            if (txtfrom_range.Text.Trim() != "" && txtto_range.Text.Trim() != "")
                                            {
                                                if (Convert.ToDouble(txtfrom_range.Text.Trim()) <= Convert.ToDouble(ps1) && Convert.ToDouble(ps1) <= Convert.ToDouble(txtto_range.Text.Trim()))
                                                {
                                                    drpq[staffandcode] = ps1;
                                                }
                                            }
                                            else
                                            {
                                                staff_chart.Visible = false;
                                                chart_staff_chart.Visible = false;
                                                imgdiv2.Visible = true;
                                                lbl_alert1.Text = "Please Enter Range";
                                            }
                                        }
                                        else
                                        {
                                            drpq[staffandcode] = ps1;
                                        }
                                    }
                                    if (ddl_criter.SelectedItem.Value == "0")
                                    {
                                        drpq[staffandcode] = ps1;
                                    }
                                    //drpq[staffandcode] = ps1;
                                }
                            }
                            dtChart2.Rows.Add(drpq);
                        }
                    }
                    //grd.DataSource = dtChart1;
                    //grd.DataBind();
                    if (dtChart2.Rows.Count > 0)
                    {
                        string srr = ""; Hashtable removedthash = new Hashtable(); string val = ""; double finalvalue = 0;
                        for (int cc = 0; cc < dtChart2.Columns.Count; cc++)
                        {
                            finalvalue = 0;
                            for (int rr = 0; rr < dtChart2.Rows.Count; rr++)
                            {
                                srr = dtChart2.Columns[cc].ToString();
                                val = dtChart2.Rows[rr][cc].ToString();
                                if (val.Trim() == "" || val.Trim().ToUpper() == "NAN")
                                    val = "0";
                                finalvalue = finalvalue + Convert.ToDouble(val);
                            }
                            if (finalvalue == 0)
                            {
                                removedthash.Add(cc, srr);
                            }
                        }
                        if (removedthash.Count > 0)
                        {
                            foreach (DictionaryEntry col in removedthash)
                            {
                                dtChart2.Columns.Remove(Convert.ToString(col.Value));
                            }
                        }
                        question_chart.RenderType = RenderType.ImageTag;
                        question_chart.ImageType = ChartImageType.Png;
                        question_chart.ImageStorageMode = ImageStorageMode.UseImageLocation;
                        question_chart.ImageLocation = Path.Combine("~/college/", "feedbackchart");
                        if (dtChart2.Columns.Count > 0)
                        {
                            int chartwidth = 0;
                            for (int r = 0; r < dtChart2.Rows.Count; r++)
                            {
                                for (int c = 0; c < dtChart2.Columns.Count; c++)
                                {
                                    string sr = dtChart2.Columns[c].ToString();
                                    question_chart.Series[r].Points.AddXY(sr.ToString().Trim(), dtChart2.Rows[r][c].ToString().Trim());
                                    question_chart.ChartAreas[0].AxisX.LineColor = System.Drawing.Color.Black;
                                    question_chart.ChartAreas[0].AxisY.LineColor = System.Drawing.Color.Black;
                                    question_chart.Series[r].IsValueShownAsLabel = true;
                                    question_chart.Series[r].IsXValueIndexed = true;
                                    //if (dtChart2.Rows[r][c].ToString().Trim() == "")
                                    //{
                                    //    question_chart.Series[r].IsValueShownAsLabel = false;
                                    //  //  question_chart.Series[r].IsXValueIndexed = false ;
                                    //}
                                    if (rdb_line.Checked == true)
                                    {
                                        question_chart.Series[r].ChartType = SeriesChartType.Line;
                                    }
                                    question_chart.ChartAreas[0].AxisX.LabelStyle.Angle = -90;
                                    question_chart.ChartAreas[0].AxisX.LabelStyle.Interval = 1;
                                    chartwidth += 15;
                                }
                            }
                            question_chart.Visible = true;
                            chartprint.Visible = true;
                            if (chartwidth <= 1125)
                                question_chart.Width = chartwidth;
                            else
                                question_chart.Width = 1125;
                        }
                        else
                        {
                            chartprint.Visible = false;
                            imgdiv2.Visible = true;
                            lbl_alert1.Text = "Please Enter Valid Range";
                        }
                    }
                }
                else
                {
                    imgdiv2.Visible = true;
                    lbl_alert1.Text = "No Records Found";
                }
            }
        }
        catch (Exception ex)
        {
            d2.sendErrorMail(ex, collegecode1, "FeedbackReport");
        }
    }

    protected void ananames6()
    {
        try
        {
            FpSpread3.Visible = false;
            question_chart.Visible = false;
            chartprint.Visible = false;
            chart_staff_chart.Visible = false;
            staff_chart.Visible = false;
            string college_cd = returnwithsinglecodevalue(cbl_clgnameformat6);
            //string degree_code = returnwithsinglecodevalue(cbl_deptnameformat6);
            string degree_code = Convert.ToString(ddlformate6_deptname.SelectedItem.Value);
            string staffcod = returnwithsinglecodevalue(cbl_staffnameformat6);
            string batchyear = returnwithsinglecodevalue(cbl_formate6batch);
            string sem = returnwithsinglecodevalue(cbl_formate6sem);
            FpSpread2.Visible = false;
            chartfalse();
            if (rb_Acad.Checked == true)
            {
                fair();
                FpSpread1.Visible = true;
                rptprint1.Visible = true;
                dsfb.Clear();
                if (degree_code.Trim() != "" && college_cd.Trim() != "" && sem.Trim() != "")
                {
                    string fbpk = " select FeedBackMasterPK from CO_FeedBackMaster where FeedBackName ='" + ddl_feedbackformate6.SelectedItem.Value + "'  ";//and DegreeCode in ('" + degree_code + "')
                    dsfb = d2.select_method_wo_parameter(fbpk, "Text");
                    string feedbakpk = "";
                    if (dsfb.Tables.Count > 0)
                    {
                        if (dsfb.Tables[0].Rows.Count > 0)
                        {
                            for (int pk = 0; pk < dsfb.Tables[0].Rows.Count; pk++)
                            {
                                if (feedbakpk == "")
                                {
                                    feedbakpk = dsfb.Tables[0].Rows[pk]["FeedBackMasterPK"].ToString();
                                }
                                else
                                {
                                    feedbakpk = feedbakpk + "','" + dsfb.Tables[0].Rows[pk]["FeedBackMasterPK"].ToString() + "";
                                }
                            }
                        }
                    }
                    if (feedbakpk.Trim() != "" && staffcod.Trim() != "")//&& subjectno.Trim() != ""
                    {
                        string type = "";
                        if (rb_Acad.Checked == true)
                        {
                            type = "1";
                        }
                        else if (rb_Gend.Checked == true)
                        {
                            type = "2";
                        }
                        FpSpread1.Sheets[0].RowCount = 0;
                        FpSpread1.Sheets[0].ColumnCount = 0;
                        FpSpread1.CommandBar.Visible = false;
                        FpSpread1.Sheets[0].AutoPostBack = true;
                        FpSpread1.Sheets[0].ColumnHeader.RowCount = 1;
                        FpSpread1.Sheets[0].RowHeader.Visible = false;
                        FpSpread1.Sheets[0].ColumnCount = 10;
                        FarPoint.Web.Spread.StyleInfo darkstyle = new FarPoint.Web.Spread.StyleInfo();
                        darkstyle.BackColor = ColorTranslator.FromHtml("#0CA6CA");
                        darkstyle.ForeColor = System.Drawing.Color.White;
                        FpSpread1.ActiveSheetView.ColumnHeader.DefaultStyle = darkstyle;
                        FpSpread1.Visible = true;
                        FpSpread1.Width = 980;
                        FpSpread1.Height = 500;
                        FpSpread1.SaveChanges();
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 0].Text = "S.No";
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 0].Font.Bold = true;
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 0].HorizontalAlign = HorizontalAlign.Center;
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 0].Font.Name = "Book Antiqua";
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 0].Font.Size = FontUnit.Medium;
                        FpSpread1.Sheets[0].Columns[0].HorizontalAlign = HorizontalAlign.Center;
                        FpSpread1.Sheets[0].SetColumnMerge(0, FarPoint.Web.Spread.Model.MergePolicy.Always);
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 1].Text = "Staff Name";
                        FpSpread1.Sheets[0].SetColumnMerge(1, FarPoint.Web.Spread.Model.MergePolicy.Always);
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 1].Font.Bold = true;
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 1].Font.Name = "Book Antiqua";
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 1].Font.Size = FontUnit.Medium;
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 1].HorizontalAlign = HorizontalAlign.Center;
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 2].Text = "S.NO";
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 2].Font.Bold = true;
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 2].Font.Name = "Book Antiqua";
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 2].Font.Size = FontUnit.Medium;
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 2].HorizontalAlign = HorizontalAlign.Center;
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 3].Text = "Batch ";
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 3].Font.Bold = true;
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 3].Font.Name = "Book Antiqua";
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 3].Font.Size = FontUnit.Medium;
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 3].HorizontalAlign = HorizontalAlign.Center;
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 4].Text = "Department";
                        FpSpread1.Sheets[0].SetColumnMerge(4, FarPoint.Web.Spread.Model.MergePolicy.Always);
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 4].Font.Bold = true;
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 4].Font.Name = "Book Antiqua";
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 4].Font.Size = FontUnit.Medium;
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 4].HorizontalAlign = HorizontalAlign.Center;
                        FpSpread1.Sheets[0].Columns[4].HorizontalAlign = HorizontalAlign.Center;
                        FpSpread1.Columns[4].Visible = false;
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 5].Text = "Subject Code";
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 5].Font.Bold = true;
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 5].Font.Name = "Book Antiqua";
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 5].Font.Size = FontUnit.Medium;
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 5].HorizontalAlign = HorizontalAlign.Center;
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 6].Text = "Subject Name";
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 6].Font.Bold = true;
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 6].Font.Name = "Book Antiqua";
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 6].Font.Size = FontUnit.Medium;
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 6].HorizontalAlign = HorizontalAlign.Center;
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 7].Text = "Total Points";
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 7].Text = ddl_feedbackformate6.SelectedItem.Text;
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 7].Font.Bold = true;
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 7].Font.Name = "Book Antiqua";
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 7].Font.Size = FontUnit.Medium;
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 7].HorizontalAlign = HorizontalAlign.Center;
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 8].Text = "Average";
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 8].Font.Bold = true;
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 8].Font.Name = "Book Antiqua";
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 8].Font.Size = FontUnit.Medium;
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 8].HorizontalAlign = HorizontalAlign.Center;
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 9].Text = "Select";
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 9].Font.Bold = true;
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 9].Font.Name = "Book Antiqua";
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 9].Font.Size = FontUnit.Medium;
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 9].HorizontalAlign = HorizontalAlign.Center;
                        FpSpread1.Sheets[0].ColumnHeader.Columns[0].Width = 39;
                        FpSpread1.Sheets[0].ColumnHeader.Columns[1].Width = 180;
                        FpSpread1.Sheets[0].ColumnHeader.Columns[2].Width = 50;
                        FpSpread1.Sheets[0].ColumnHeader.Columns[3].Width = 200;
                        FpSpread1.Sheets[0].ColumnHeader.Columns[4].Width = 100;
                        FpSpread1.Sheets[0].ColumnHeader.Columns[5].Width = 70;
                        FpSpread1.Sheets[0].ColumnHeader.Columns[6].Width = 200;
                        FpSpread1.Sheets[0].ColumnHeader.Columns[7].Width = 170;
                        FpSpread1.Sheets[0].ColumnHeader.Columns[8].Width = 59;
                        FpSpread1.Sheets[0].ColumnHeader.Columns[9].Width = 50;
                        ds.Clear();
                        string selqry = "";
                        //selqry = " select SUM(M.Point)Points,sm.staff_name ,sbj.subject_name ,count( distinct S.FeedbackUnicode)Strength,sbj.Subject_Code,dt.Dept_Name,f.Batch_Year,f.semester,f.semester,C.Course_Name,((CONVERT(varchar(max), f.Batch_Year)+' - '+C.Course_Name+' - '+dt.dept_acronym+' - '+convert(varchar(20), f.semester)))as Batch,SubjectNo,staff_code,d.Degree_Code from CO_FeedbackUniCode FU,CO_FeedBackMaster F,CO_StudFeedBack S,CO_MarkMaster M,Degree D,Department dt,Course C,staffmaster sm,staff_appl_master sa,Subject sbj  where sbj.Subject_No =s.SubjectNo and sm.appl_no =sa.appl_no and sa.appl_id =s.StaffApplNo and  d.Degree_Code =f.DegreeCode and dt.Dept_Code=d.Dept_Code and c.Course_Id =d.Course_Id and  FU.FeedbackMasterFK=F.FeedBackMasterPK and s.FeedBackMasterFK =f.FeedBackMasterPK and s.FeedBackMasterFK =fu.FeedbackMasterFK and fu.FeedbackUnicode =s.FeedbackUnicode and M.MarkMasterPK =S.MarkMasterPK and Fu.FeedbackMasterFK in ('" + feedbakpk + "')  and f.InclueCommon ='1' group by sm.staff_name ,sbj.subject_name ,dt.Dept_Name , f.Batch_Year,f.semester,f.semester, C.Course_Name, sbj.Subject_Code,dt.dept_acronym,SubjectNo,staff_code,d.Degree_Code order by sm.staff_name ";//and sbj.subject_no in('" + subjectno + "')
                        selqry = " select SUM(M.Point)Points,count( distinct S.FeedbackUnicode)Strength,sm.staff_name ,sbj.subject_name ,sbj.Subject_Code,dt.Dept_Name,f.Batch_Year,f.semester,f.Section,C.Course_Name,((CONVERT(varchar(max), f.Batch_Year)+' - '+C.Course_Name+' - '+dt.dept_acronym+' - '+convert(varchar(20), f.semester)+ case when section='' then '' else ' - '+ (section) end)) as Batch,SubjectNo,staff_code,d.Degree_Code from CO_FeedbackUniCode FU,CO_FeedBackMaster F,CO_StudFeedBack S,CO_MarkMaster M,Degree D,Department dt,Course C,staffmaster sm,staff_appl_master sa,Subject sbj  where sbj.Subject_No =s.SubjectNo and sm.appl_no =sa.appl_no and sa.appl_id =s.StaffApplNo and  d.Degree_Code =f.DegreeCode and dt.Dept_Code=d.Dept_Code and c.Course_Id =d.Course_Id and  FU.FeedbackMasterFK=F.FeedBackMasterPK and s.FeedBackMasterFK =f.FeedBackMasterPK and s.FeedBackMasterFK =fu.FeedbackMasterFK and fu.FeedbackUnicode =s.FeedbackUnicode and M.MarkMasterPK =S.MarkMasterPK and StaffApplNo in ('" + staffcod + "') and f.FeedBackName = '" + ddl_feedbackformate6.SelectedItem.Text + "' and  f.Batch_Year in('" + batchyear + "') and f.incluecommon=1 and f.semester in('" + sem + "') group by sm.staff_name ,sbj.subject_name ,dt.Dept_Name , f.Batch_Year,f.semester,f.semester, C.Course_Name, sbj.Subject_Code,dt.dept_acronym,SubjectNo,staff_code,d.Degree_Code,f.Section order by sm.staff_name ";
                        selqry = selqry + " select COUNT( distinct QuestionMasterFK)question_count from CO_FeedBackQuestions where FeedBackMasterFK in ('" + feedbakpk + "')";
                        selqry = selqry + " select top 1 Point from CO_MarkMaster  where CollegeCode in('" + collegecode1 + "') order by Point desc";
                        //selqry = selqry + " select COUNT(FeedbackUnicode),m.Batch_Year,m.DegreeCode,m.semester,m.Section, FeedBackMasterPK,cr.Course_Id  from CO_FeedbackUniCode c,CO_FeedBackMaster M,Degree d,Department dt,Course cr where cr.Course_Id =d.Course_Id and d.Dept_Code =dt.Dept_Code and d.Degree_Code =m.DegreeCode and c.FeedbackMasterFK =m.FeedBackMasterPK and FeedBackMasterPK in('" + feedbakpk + "')  group by m.DegreeCode,m.Batch_Year,m.semester,m.Section,FeedBackMasterPK,(cr.Course_Name +' - '+dt.Dept_Name) ,cr.Course_Id order by cr.Course_Id asc ";
                        selqry = selqry + "  select COUNT(distinct FeedbackUnicode),m.Batch_Year,m.DegreeCode,m.semester,m.Section, FeedBackMasterPK,cr.Course_Id  from co_studfeedback c,CO_FeedBackMaster M,Degree d,Department dt,Course cr where cr.Course_Id =d.Course_Id and d.Dept_Code =dt.Dept_Code and d.Degree_Code =m.DegreeCode and c.FeedbackMasterFK =m.FeedBackMasterPK and FeedBackMasterPK in('" + feedbakpk + "') and m.semester in('" + sem + "') group by m.DegreeCode,m.Batch_Year,m.semester,m.Section,FeedBackMasterPK,(cr.Course_Name +' - '+dt.Dept_Name) ,cr.Course_Id order by cr.Course_Id asc";
                        ds = d2.select_method_wo_parameter(selqry, "Text");
                        string needs = "5";
                        string question_count = "";
                        if (ds.Tables[1].Rows.Count > 0)
                        {
                            question_count = Convert.ToString(ds.Tables[1].Rows[0][0]);
                        }
                        if (question_count.Trim() == "")
                            question_count = "0";
                        string sum_total = "";
                        if (ds.Tables[2].Rows.Count > 0)
                        {
                            sum_total = Convert.ToString(ds.Tables[2].Rows[0][0]);
                        }
                        if (sum_total.Trim() == "")
                            sum_total = "0";
                        if (ds.Tables.Count > 0)
                        {
                            if (ds.Tables[0].Rows.Count > 0)
                            {
                                Double sum_tot = 0; double sumfbtot = 0;
                                Double sum_avgs = 0; int k = 0; string staffname = ""; int s = 1;
                                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                                {
                                    FpSpread1.Sheets[0].RowCount++;
                                    if (staffname.Trim() == "")
                                    { k++; }
                                    else if (staffname == ds.Tables[0].Rows[i]["staff_name"].ToString())
                                    { k++; }
                                    else { k = 1; s++; }
                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Text = Convert.ToString(s);
                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].VerticalAlign = FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].VerticalAlign = VerticalAlign.Middle;
                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].Text = ds.Tables[0].Rows[i]["staff_name"].ToString();
                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].Tag = ds.Tables[0].Rows[i]["staff_code"].ToString();
                                    staffname = ds.Tables[0].Rows[i]["staff_name"].ToString();
                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].VerticalAlign = FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].VerticalAlign = VerticalAlign.Middle;
                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].Text = k.ToString();
                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].VerticalAlign = FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].VerticalAlign = VerticalAlign.Middle;
                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 3].Text = ds.Tables[0].Rows[i]["Batch"].ToString();
                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 3].Tag = ds.Tables[0].Rows[i]["Batch_Year"].ToString();
                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 3].VerticalAlign = FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 3].VerticalAlign = VerticalAlign.Middle;
                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 4].Text = ds.Tables[0].Rows[i]["Dept_Name"].ToString();
                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 4].Tag = ds.Tables[0].Rows[i]["Course_Name"].ToString();
                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 4].Note = ds.Tables[0].Rows[i]["Degree_Code"].ToString();
                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 4].VerticalAlign = FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 4].VerticalAlign = VerticalAlign.Middle;
                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 5].Text = ds.Tables[0].Rows[i]["Subject_Code"].ToString();
                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 5].Tag = ds.Tables[0].Rows[i]["semester"].ToString();
                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 6].Text = ds.Tables[0].Rows[i]["subject_name"].ToString();
                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 6].Tag = ds.Tables[0].Rows[i]["SubjectNo"].ToString();
                                    Double point = Convert.ToDouble(ds.Tables[0].Rows[i]["Points"]);
                                    Double strength = Convert.ToDouble(ds.Tables[0].Rows[i]["Strength"]);
                                    //17.08.16
                                    string totaluniquecode = "";
                                    if (ds.Tables[3].Rows.Count > 0)
                                    {
                                        DataView dv2 = new DataView();
                                        ds.Tables[3].DefaultView.RowFilter = " Batch_Year='" + ds.Tables[0].Rows[i]["Batch_Year"].ToString() + "' and semester='" + ds.Tables[0].Rows[i]["semester"].ToString() + "' and DegreeCode='" + ds.Tables[0].Rows[i]["Degree_Code"].ToString() + "' and Section='" + ds.Tables[0].Rows[i]["Section"].ToString() + "'";
                                        dv2 = ds.Tables[3].DefaultView;
                                        if (dv2.Count > 0)
                                            totaluniquecode = Convert.ToString(dv2[0][0]);
                                    }
                                    if (totaluniquecode.Trim() == "")
                                        totaluniquecode = "0";
                                    double calfbcal = Convert.ToDouble(strength) * Convert.ToDouble(question_count) * Convert.ToDouble(sum_total);
                                    double fbavg = (point / calfbcal) * 100;
                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 7].Text = Convert.ToString(Math.Round(fbavg, 0));
                                    strength = strength * Convert.ToDouble(sum_total) * Convert.ToDouble(question_count);
                                    Double avg = 0;
                                    avg = point / strength;
                                    avg = avg * 100;
                                    avg = (Math.Round(avg, 1));
                                    sum_tot = sum_tot + Convert.ToDouble(ds.Tables[0].Rows[i]["Points"]);
                                    sum_avgs = sum_avgs + Convert.ToDouble(avg);
                                    sumfbtot = sumfbtot + Convert.ToDouble(fbavg);
                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 8].Text = Convert.ToString(avg);
                                }
                                FpSpread1.Sheets[0].RowCount++;
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 7].Text = Convert.ToString(Math.Round(sumfbtot, 2));
                                sum_avgs = sum_avgs / Convert.ToDouble(FpSpread1.Sheets[0].RowCount - 2);
                                string sum_avg = Convert.ToString(Math.Round(sum_avgs, 2));
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 8].Text = Convert.ToString(sum_avg);
                                FpSpread1.Sheets[0].SpanModel.Add(FpSpread1.Sheets[0].RowCount - 1, 0, 1, 7);
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].Rows.Count - 1, 0].Text = "Grand Total";
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].Rows.Count - 1, 0].HorizontalAlign = HorizontalAlign.Right;
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].Rows.Count - 1, 0].ForeColor = System.Drawing.Color.Blue;
                                FpSpread1.Rows[FpSpread1.Sheets[0].Rows.Count - 1].Visible = false;
                                FpSpread1.Sheets[0].Columns[2].HorizontalAlign = HorizontalAlign.Center;
                                FpSpread1.Sheets[0].Columns[5].HorizontalAlign = HorizontalAlign.Center;
                                FpSpread1.Sheets[0].Columns[7].HorizontalAlign = HorizontalAlign.Center;
                                FpSpread1.Sheets[0].Columns[3].HorizontalAlign = HorizontalAlign.Center;
                                FpSpread1.Sheets[0].Columns[8].HorizontalAlign = HorizontalAlign.Center;
                                FpSpread1.Sheets[0].Columns[9].HorizontalAlign = HorizontalAlign.Center;
                                FpSpread1.Sheets[0].PageSize = FpSpread1.Sheets[0].RowCount;
                                if (cb_avgcolumn.Checked == true)
                                {
                                    FpSpread1.Columns[8].Visible = true;
                                }
                                else
                                {
                                    FpSpread1.Columns[8].Visible = false;
                                }
                                FpSpread1.Columns[9].Visible = false;
                            }
                            else
                            {
                                imgdiv2.Visible = true;
                                lbl_alert1.Text = "No Records Found";
                                FpSpread1.Visible = false;
                                rptprint1.Visible = false;
                            }
                        }
                        else
                        {
                            imgdiv2.Visible = true;
                            lbl_alert1.Text = "No Records Found";
                            FpSpread1.Visible = false;
                            rptprint1.Visible = false;
                        }
                    }
                    else
                    {
                        imgdiv2.Visible = true;
                        lbl_alert1.Text = "Please Select All Fields";
                        FpSpread1.Visible = false;
                        rptprint1.Visible = false;
                    }
                }
                else
                {
                    imgdiv2.Visible = true;
                    lbl_alert1.Text = "Please Select All Fields";
                    FpSpread1.Visible = false;
                    rptprint1.Visible = false;
                }
            }
        }
        catch (Exception ex)
        {
            d2.sendErrorMail(ex, collegecode1, "ananmous format6");
        }
    }

    protected void btnExportPDF_Click(object sender, EventArgs e)
    {
        if (rb_login.Checked == true)
        {
            if (rb_farmate4.Checked == true)
            {
                chartprintpdf(staff_chart, "Staff_Percentage_Chart_" + Convert.ToString(DateTime.Now.ToString("dd_MM_yyyy_hh_mm_ss") + ""));
            }
            if (rb_farmate5.Checked == true)
            {
                chartprintpdf(question_chart, "Questionwise_Performance_Chart_" + Convert.ToString(DateTime.Now.ToString("dd_MM_yyyy_hh_mm_ss") + ""));
            }
        }
        else
        {
            if (rb_anonyms_farmate4.Checked == true)
            {
                chartprintpdf(staff_chart, "Staff_Percentage_Chart_" + Convert.ToString(DateTime.Now.ToString("dd_MM_yyyy_hh_mm_ss") + ""));
            }
            if (rb_anonyms_farmate5.Checked == true)
            {
                chartprintpdf(question_chart, "Questionwise_Performance_Chart_" + Convert.ToString(DateTime.Now.ToString("dd_MM_yyyy_hh_mm_ss") + ""));
            }
        }
    }

    protected void chartprintpdf(Chart ChartName, string PdfName)
    {
        Document pdfDoc = new Document(PageSize.A4.Rotate(), 10f, 10f, 10f, 0f);
        //pdfDoc.SetPageSize(iTextSharp.text.PageSize.A4.Rotate());
        PdfWriter.GetInstance(pdfDoc, Response.OutputStream);
        pdfDoc.Open();
        using (System.IO.MemoryStream stream = new MemoryStream())
        {
            ChartName.SaveImage(stream, ChartImageFormat.Png);
            iTextSharp.text.Image chartImage = iTextSharp.text.Image.GetInstance(stream.GetBuffer());
            chartImage.ScalePercent(75f);
            pdfDoc.Add(chartImage);
            pdfDoc.Close();
            Response.ContentType = "application/pdf";
            Response.AddHeader("content-disposition", "attachment;filename=" + PdfName + ".pdf");
            Response.Cache.SetCacheability(HttpCacheability.NoCache);
            Response.Write(pdfDoc);
            Response.End();
        }
    }

    protected void FpSpread1_Command(object sender, FarPoint.Web.Spread.SpreadCommandEventArgs e)
    {
        try
        {
            string actrow = FpSpread1.Sheets[0].ActiveRow.ToString();
            string actcol = FpSpread1.Sheets[0].ActiveColumn.ToString();
            if (actrow.Trim() == "0" && actcol.Trim() == "8")
            {
                if (FpSpread1.Sheets[0].RowCount > 0)
                {
                    int checkval = Convert.ToInt32(FpSpread1.Sheets[0].Cells[0, 8].Value);
                    if (checkval == 0)
                    {
                        for (int i = 1; i < FpSpread1.Sheets[0].RowCount - 1; i++)
                        {
                            FpSpread1.Sheets[0].Cells[i, 8].Value = 1;
                        }
                    }
                    if (checkval == 1)
                    {
                        for (int i = 1; i < FpSpread1.Sheets[0].RowCount - 1; i++)
                        {
                            FpSpread1.Sheets[0].Cells[i, 8].Value = 0;
                        }
                    }
                }
            }
        }
        catch
        {
        }
    }

    protected void btn_printperticulaterstaff_click(object sender, EventArgs e)
    {
        try
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
                            college_cd = "" + Cbl_college.Items[i].Value.ToString() + "";
                        }
                        else
                        {
                            college_cd = college_cd + "','" + Convert.ToString(Cbl_college.Items[i].Value);
                        }
                    }
                }
            }
            string Batch_Year = "";
            for (int i = 0; i < cbl_batch.Items.Count; i++)
            {
                if (cbl_batch.Items[i].Selected == true)
                {
                    if (Batch_Year == "")
                    {
                        Batch_Year = "" + cbl_batch.Items[i].Value.ToString() + "";
                    }
                    else
                    {
                        Batch_Year = Batch_Year + "','" + cbl_batch.Items[i].Value.ToString() + "";
                    }
                }
            }
            string degree_code = "";
            for (int i = 0; i < cbl_branch.Items.Count; i++)
            {
                if (cbl_branch.Items[i].Selected == true)
                {
                    if (degree_code == "")
                    {
                        degree_code = "" + cbl_branch.Items[i].Value.ToString() + "";
                    }
                    else
                    {
                        degree_code = degree_code + "','" + cbl_branch.Items[i].Value.ToString() + "";
                    }
                }
            }
            string semester = "";
            for (int i = 0; i < cbl_sem.Items.Count; i++)
            {
                if (cbl_sem.Items[i].Selected == true)
                {
                    if (semester == "")
                    {
                        semester = "" + cbl_sem.Items[i].Value.ToString() + "";
                    }
                    else
                    {
                        semester = semester + "','" + cbl_sem.Items[i].Value.ToString() + "";
                    }
                }
            }
            string section = "";
            for (int i = 0; i < cbl_sec.Items.Count; i++)
            {
                if (cbl_sec.Items[i].Selected == true)
                {
                    if (section == "")
                    {
                        section = "" + cbl_sec.Items[i].Value.ToString() + "";
                    }
                    else
                    {
                        section = section + "','" + cbl_sec.Items[i].Value.ToString() + "";
                    }
                }
            }
            string sub = "";
            for (int i = 0; i < Cbl_Subject.Items.Count; i++)
            {
                if (Cbl_Subject.Items[i].Selected == true)
                {
                    if (sub == "")
                    {
                        sub = "" + Cbl_Subject.Items[i].Value.ToString() + "";
                    }
                    else
                    {
                        sub = sub + "','" + Cbl_Subject.Items[i].Value.ToString() + "";
                    }
                }
            }
            if (section.Trim() != "")
            {
                section = section + "','";
            }
            dsfb.Clear();
            string fbpk = " select FeedBackMasterPK from CO_FeedBackMaster where FeedBackName ='" + ddl_Feedbackname.SelectedItem.Value + "' and DegreeCode in ('" + degree_code + "') and semester in ('" + semester + "') and Batch_Year in('" + Batch_Year + "') and section in ('" + section + "')";
            dsfb = d2.select_method_wo_parameter(fbpk, "Text");
            string feedbakpk = "";
            if (dsfb.Tables.Count > 0)
            {
                if (dsfb.Tables[0].Rows.Count > 0)
                {
                    for (int pk = 0; pk < dsfb.Tables[0].Rows.Count; pk++)
                    {
                        if (feedbakpk == "")
                        {
                            feedbakpk = dsfb.Tables[0].Rows[pk]["FeedBackMasterPK"].ToString();
                        }
                        else
                        {
                            feedbakpk = feedbakpk + "','" + dsfb.Tables[0].Rows[pk]["FeedBackMasterPK"].ToString() + "";
                        }
                    }
                }
            }
            FpSpread1.SaveChanges(); int pcount = 0;
            string Collvalue = "";
            int left1 = 40;
            int left2 = 140;
            int left3 = 255;
            int left4 = 330;
            string staffname, degree, dept, subjectcode, sem, subjectname, staffcode, subjectno, BatchYear, avg, degreecode, sectionsingle = "";
            Gios.Pdf.PdfDocument mydoc = new Gios.Pdf.PdfDocument(PdfDocumentFormat.A4);
            Gios.Pdf.PdfPage mypdfpage;
            PdfTextArea collinfo1;
            Gios.Pdf.PdfTable tbl_dept;
            string sec = "";
            if (section.Trim() == "")
            {
                sec = " and FM.Section in ('') ";
            }
            else
            {
                sec = " and FM.Section in ('" + section + "') ";
            }
            string q1 = " select Staff_Name,sf.subjectno,(Point)as Points,Question,sf.StaffApplNo ,Section from CO_FeedBackMaster FM,CO_StudFeedBack SF,staff_appl_master A,staffmaster T,CO_MarkMaster M, CO_QuestionMaster Q where Q.QuestionMasterPK =sf.QuestionMasterFK and M.MarkMasterPK =sf.MarkMasterPK and A.appl_no =t.appl_no and a.appl_id =sf.StaffApplNo  and sf.FeedBackMasterFK =fm.FeedBackMasterPK and sf.FeedbackMasterFK in ('" + feedbakpk + "') and FM.CollegeCode in ('" + college_cd + "') and FM.Batch_Year in ('" + Batch_Year + "') and FM.degreecode in ('" + degree_code + "') and FM.semester in ('" + semester + "')  " + sec + " and isnull (App_No,0)<>0";
            ds1.Clear();
            ds1 = d2.select_method_wo_parameter(q1, "text");
            for (int k = 1; k < FpSpread1.Sheets[0].RowCount; k++)
            {
                int checkval = Convert.ToInt32(FpSpread1.Sheets[0].Cells[k, 8].Value);
                if (checkval == 1)
                {
                    int coltop = 0;
                    mypdfpage = mydoc.NewPage();
                    pcount++;
                    staffname = Convert.ToString(FpSpread1.Sheets[0].Cells[k, 1].Text);
                    staffcode = Convert.ToString(FpSpread1.Sheets[0].Cells[k, 1].Tag);
                    dept = Convert.ToString(FpSpread1.Sheets[0].Cells[k, 3].Note);
                    degree = Convert.ToString(FpSpread1.Sheets[0].Cells[k, 4].Tag);
                    subjectcode = Convert.ToString(FpSpread1.Sheets[0].Cells[k, 4].Text);
                    subjectname = Convert.ToString(FpSpread1.Sheets[0].Cells[k, 5].Text);
                    subjectno = Convert.ToString(FpSpread1.Sheets[0].Cells[k, 6].Tag);
                    BatchYear = Convert.ToString(FpSpread1.Sheets[0].Cells[k, 3].Tag);
                    sem = Convert.ToString(FpSpread1.Sheets[0].Cells[k, 5].Tag);
                    avg = Convert.ToString(FpSpread1.Sheets[0].Cells[k, 7].Text);
                    degreecode = Convert.ToString(FpSpread1.Sheets[0].Cells[k, 4].Note);
                    sectionsingle = Convert.ToString(FpSpread1.Sheets[0].Cells[k, 5].Note);
                    System.Drawing.Font Fontbold = new System.Drawing.Font("Book Antiqua", 12, FontStyle.Bold);
                    System.Drawing.Font Fontsmall = new System.Drawing.Font("Book Antiqua", 12, FontStyle.Regular);
                    System.Drawing.Font Fontsmall1 = new System.Drawing.Font("Book Antiqua", 10, FontStyle.Regular);
                    System.Drawing.Font Fontsmall1bold = new System.Drawing.Font("Book Antiqua", 10, FontStyle.Bold);
                    #region
                    string strquery = "Select * from Collinfo where college_code=" + Session["collegecode"].ToString() + "";
                    ds.Clear();
                    ds = d2.select_method_wo_parameter(strquery, "Text");
                    coltop = coltop + 15;
                    collinfo1 = new PdfTextArea(Fontbold, System.Drawing.Color.Black, new PdfArea(mydoc, 0, coltop, 595, 50), System.Drawing.ContentAlignment.TopCenter, "" + ds.Tables[0].Rows[0]["collname"].ToString() + "");
                    mypdfpage.Add(collinfo1);
                    string address1 = ds.Tables[0].Rows[0]["Address1"].ToString();
                    string address2 = ds.Tables[0].Rows[0]["Address2"].ToString();
                    string address3 = ds.Tables[0].Rows[0]["Address3"].ToString();
                    if (address1.Trim() != "" && address1 != null && address1.Length > 1)
                    {
                        Collvalue = address1;
                    }
                    if (address2.Trim() != "" && address2 != null && address2.Length > 1)
                    {
                        if (Collvalue.Trim() != "" && Collvalue != null)
                        {
                            Collvalue = Collvalue + ',' + address2;
                        }
                        else
                        {
                            Collvalue = address2;
                        }
                    }
                    if (address3.Trim() != "" && address3 != null && address3.Length > 1)
                    {
                        if (Collvalue.Trim() != "" && Collvalue != null)
                        {
                            Collvalue = Collvalue + ',' + address3;
                        }
                        else
                        {
                            Collvalue = address3;
                        }
                    }
                    collinfo1 = new PdfTextArea(Fontsmall1bold, System.Drawing.Color.Black, new PdfArea(mydoc, 0, coltop + 25, 595, 50), System.Drawing.ContentAlignment.TopCenter, "" + Collvalue + "");
                    mypdfpage.Add(collinfo1);
                    string district = ds.Tables[0].Rows[0]["district"].ToString();
                    string state = ds.Tables[0].Rows[0]["State"].ToString();
                    string pincode = ds.Tables[0].Rows[0]["Pincode"].ToString();
                    if (district.Trim() != "" && district != null && district.Length > 1)
                    {
                        Collvalue = district;
                    }
                    if (state.Trim() != "" && state != null && state.Length > 1)
                    {
                        if (Collvalue.Trim() != "" && Collvalue != null)
                        {
                            Collvalue = Collvalue + ',' + state;
                        }
                        else
                        {
                            Collvalue = state;
                        }
                    }
                    if (pincode.Trim() != "" && pincode != null && pincode.Length > 1)
                    {
                        if (Collvalue.Trim() != "" && Collvalue != null)
                        {
                            Collvalue = Collvalue + '-' + pincode;
                        }
                        else
                        {
                            Collvalue = pincode;
                        }
                    }
                    collinfo1 = new PdfTextArea(Fontsmall1bold, System.Drawing.Color.Black, new PdfArea(mydoc, 0, coltop + 35, 595, 50), System.Drawing.ContentAlignment.TopCenter, "" + Collvalue + "");
                    mypdfpage.Add(collinfo1);
                    if (File.Exists(HttpContext.Current.Server.MapPath("~/college/Left_Logo.jpeg")))
                    {
                        Gios.Pdf.PdfImage LogoImage = mydoc.NewImage(HttpContext.Current.Server.MapPath("~/college/Left_Logo.jpeg"));
                        mypdfpage.Add(LogoImage, 25, 25, 500);
                    }
                    if (!File.Exists(HttpContext.Current.Server.MapPath("~/college/Left_Logo.jpeg")))
                    {
                        MemoryStream memoryStream = new MemoryStream();
                        string sellogo = "select logo1,logo2 from collinfo where college_code='" + Session["collegecode"] + "'";
                        ds.Clear();
                        ds = d2.select_method_wo_parameter(sellogo, "Text");
                        if (ds.Tables.Count > 0)
                        {
                            if (ds.Tables[0].Rows.Count > 0)
                            {
                                byte[] file = (byte[])ds.Tables[0].Rows[0]["logo1"];
                                memoryStream.Write(file, 0, file.Length);
                                if (file.Length > 0)
                                {
                                    System.Drawing.Image imgx = System.Drawing.Image.FromStream(memoryStream, true, true);
                                    System.Drawing.Image thumb = imgx.GetThumbnailImage(400, 400, null, IntPtr.Zero);
                                    thumb.Save(HttpContext.Current.Server.MapPath("~/college/Left_Logo.jpeg"), System.Drawing.Imaging.ImageFormat.Jpeg);
                                }
                                memoryStream.Dispose();
                                memoryStream.Close();
                            }
                        }
                        Gios.Pdf.PdfImage LogoImage = mydoc.NewImage(HttpContext.Current.Server.MapPath("~/college/Left_Logo.jpeg"));
                        mypdfpage.Add(LogoImage, 25, coltop + 10, 500);
                    }
                    if (File.Exists(HttpContext.Current.Server.MapPath("~/college/Right_Logo.jpeg")))
                    {
                        Gios.Pdf.PdfImage LogoImage = mydoc.NewImage(HttpContext.Current.Server.MapPath("~/college/Right_Logo.jpeg"));
                        mypdfpage.Add(LogoImage, 510, 25, 500);
                    }
                    if (!File.Exists(HttpContext.Current.Server.MapPath("~/college/Right_Logo.jpeg")))
                    {
                        MemoryStream memoryStream = new MemoryStream();
                        string sellogo = "select logo1,logo2 from collinfo where college_code='" + Session["collegecode"] + "'";
                        ds.Clear();
                        ds = d2.select_method_wo_parameter(sellogo, "Text");
                        if (ds.Tables.Count > 0)
                        {
                            if (ds.Tables[0].Rows.Count > 0)
                            {
                                byte[] file = (byte[])ds.Tables[0].Rows[0]["logo1"];
                                memoryStream.Write(file, 0, file.Length);
                                if (file.Length > 0)
                                {
                                    System.Drawing.Image imgx = System.Drawing.Image.FromStream(memoryStream, true, true);
                                    System.Drawing.Image thumb = imgx.GetThumbnailImage(400, 400, null, IntPtr.Zero);
                                    thumb.Save(HttpContext.Current.Server.MapPath("~/college/Right_Logo.jpeg"), System.Drawing.Imaging.ImageFormat.Jpeg);
                                }
                                memoryStream.Dispose();
                                memoryStream.Close();
                            }
                        }
                        Gios.Pdf.PdfImage LogoImage = mydoc.NewImage(HttpContext.Current.Server.MapPath("~/college/Right_Logo.jpeg"));
                        mypdfpage.Add(LogoImage, 510, coltop + 10, 500);
                    }
                    #endregion
                    coltop = coltop + 30;
                    coltop = coltop + 30;
                    collinfo1 = new PdfTextArea(Fontbold, System.Drawing.Color.Black, new PdfArea(mydoc, 0, coltop, 595, 50), System.Drawing.ContentAlignment.TopCenter, "STUDENTS FEEDBACK SUMMARY");
                    mypdfpage.Add(collinfo1);
                    coltop = coltop + 20;
                    collinfo1 = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                               new PdfArea(mydoc, left1, coltop, 300, 50), System.Drawing.ContentAlignment.MiddleLeft, "Month & Year      :");
                    mypdfpage.Add(collinfo1);
                    string date = d2.GetFunction(" select CONVERT(varchar(11),startdate,103)+' - '+CONVERT(varchar(11),EndDate,103)as date from CO_FeedBackMaster where FeedBackMasterPK in('" + feedbakpk + "')");
                    collinfo1 = new PdfTextArea(Fontsmall1, System.Drawing.Color.Black,
                                           new PdfArea(mydoc, left2, coltop, 300, 50), System.Drawing.ContentAlignment.MiddleLeft, date);
                    mypdfpage.Add(collinfo1);
                    collinfo1 = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                           new PdfArea(mydoc, left3, coltop, 300, 50), System.Drawing.ContentAlignment.MiddleLeft, "Semester    :");
                    mypdfpage.Add(collinfo1);
                    collinfo1 = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                       new PdfArea(mydoc, left4, coltop, 300, 50), System.Drawing.ContentAlignment.MiddleLeft, sem);
                    mypdfpage.Add(collinfo1);
                    collinfo1 = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                              new PdfArea(mydoc, left1, coltop + 20, 300, 50), System.Drawing.ContentAlignment.MiddleLeft, "Degree                :");
                    mypdfpage.Add(collinfo1);
                    collinfo1 = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                      new PdfArea(mydoc, left2, coltop + 20, 300, 50), System.Drawing.ContentAlignment.MiddleLeft, degree);
                    mypdfpage.Add(collinfo1);
                    collinfo1 = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                         new PdfArea(mydoc, left3, coltop + 20, 300, 50), System.Drawing.ContentAlignment.MiddleLeft, "Department :");
                    mypdfpage.Add(collinfo1);
                    collinfo1 = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                    new PdfArea(mydoc, left4, coltop + 20, 250, 50), System.Drawing.ContentAlignment.MiddleLeft, dept);
                    mypdfpage.Add(collinfo1);
                    collinfo1 = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                            new PdfArea(mydoc, left1, coltop + 40, 300, 50), System.Drawing.ContentAlignment.MiddleLeft, "Subject & Code   :");
                    mypdfpage.Add(collinfo1);
                    collinfo1 = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                    new PdfArea(mydoc, left2, coltop + 40, 300, 50), System.Drawing.ContentAlignment.MiddleLeft, subjectcode);
                    mypdfpage.Add(collinfo1);
                    collinfo1 = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                       new PdfArea(mydoc, left3, coltop + 40, 300, 50), System.Drawing.ContentAlignment.MiddleLeft, "Faculty        :");
                    mypdfpage.Add(collinfo1);
                    collinfo1 = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                   new PdfArea(mydoc, left4, coltop + 40, 300, 50), System.Drawing.ContentAlignment.MiddleLeft, staffname);
                    mypdfpage.Add(collinfo1);
                    string se = "";
                    if (sectionsingle.Trim() == "")
                        se = " and Sections =''";
                    else
                        se = " and Sections='" + sectionsingle + "'";
                    DataSet attendcount = new DataSet();
                    string staffapplid = "";
                    staffapplid = d2.GetFunction("select appl_id from staffmaster s ,staff_appl_master sp where s.appl_no=sp.appl_no and staff_code='" + staffcode + "'");
                    string q2 = "  select COUNT(app_no) from Registration where degree_code ='" + degreecode + "' and Batch_Year ='" + BatchYear + "'  and CC=0 and DelFlag =0 and Exam_Flag <>'Debar' " + se + "";
                    q2 = q2 + " select COUNT(distinct sf.App_No ) as total from CO_StudFeedBack sf,Registration r,CO_FeedBackMaster f where f.FeedBackMasterPK =sf.FeedBackMasterFK and sf.App_No =r.App_No and f.FeedBackMasterPK in('" + feedbakpk + "') and f.Batch_Year ='" + BatchYear + "' and f.DegreeCode ='" + degreecode + "' and StaffApplNo ='" + staffapplid + "' and SubjectNo ='" + subjectno + "' " + se + "";
                    attendcount.Clear();
                    attendcount = d2.select_method_wo_parameter(q2, "text");
                    string totalstudent = "";
                    string currentstudent = "";
                    if (attendcount.Tables[0].Rows.Count > 0)
                    {
                        if (attendcount.Tables[0].Rows.Count > 0)
                        {
                            totalstudent = Convert.ToString(attendcount.Tables[0].Rows[0][0]);
                        }
                        if (attendcount.Tables[1].Rows.Count > 0)
                        {
                            currentstudent = Convert.ToString(attendcount.Tables[1].Rows[0][0]);
                        }
                    }
                    if (currentstudent.Trim() == "")
                        currentstudent = "0";
                    if (totalstudent.Trim() == "")
                        totalstudent = "0";
                    string siglesec = "";
                    if (sectionsingle.Trim() == "")
                        siglesec = "and Section in ('')";
                    else
                        siglesec = "and Section in ('" + sectionsingle + "')";
                    collinfo1 = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                      new PdfArea(mydoc, left1, coltop + 60, 300, 50), System.Drawing.ContentAlignment.MiddleLeft, "Total No of students in a class   :   " + totalstudent);
                    mypdfpage.Add(collinfo1);
                    collinfo1 = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                    new PdfArea(mydoc, left3, coltop + 60, 300, 50), System.Drawing.ContentAlignment.MiddleLeft, "No of students given the Feedback :   " + currentstudent);
                    mypdfpage.Add(collinfo1);
                    collinfo1 = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                     new PdfArea(mydoc, left1, coltop + 80, 300, 50), System.Drawing.ContentAlignment.MiddleLeft, "Average Feedback of the class  :  " + avg);
                    mypdfpage.Add(collinfo1);
                    DataView dv = new DataView();
                    ds1.Tables[0].DefaultView.RowFilter = "  StaffApplNo='" + staffapplid + "' and subjectno='" + subjectno + "' " + siglesec + "";
                    dv = ds1.Tables[0].DefaultView;
                    Gios.Pdf.PdfTablePage newpdftabpage2;
                    int tblcount = 0;
                    if (dv.Count > 21)
                        tblcount = 21;
                    else
                        tblcount = dv.Count + 1;
                    tbl_dept = mydoc.NewTable(Fontsmall1, tblcount, 2, 2);
                    tbl_dept.VisibleHeaders = false;
                    tbl_dept.SetBorders(System.Drawing.Color.Black, 1, BorderType.CompleteGrid);
                    tbl_dept.Columns[0].SetWidth(300);
                    tbl_dept.Columns[1].SetWidth(100);
                    int tableheight = 0;
                    int tblheight = 0;
                    if (dv.Count > 0)
                    {
                        tbl_dept.Cell(0, 0).SetContentAlignment(ContentAlignment.MiddleCenter);
                        tbl_dept.Cell(0, 0).SetContent("Factors");
                        tbl_dept.Cell(0, 0).SetFont(Fontsmall1bold);
                        tbl_dept.Cell(0, 1).SetContentAlignment(ContentAlignment.MiddleCenter);
                        tbl_dept.Cell(0, 1).SetContent("Performance");
                        coltop += 130;
                        int row = 1;
                        int modcount = 1;
                        for (int m = 1; m <= dv.Count; m++)
                        {
                            if (m % 21 == 0)
                            {
                                newpdftabpage2 = tbl_dept.CreateTablePage(new Gios.Pdf.PdfArea(mydoc, 77, coltop, 450, 400));
                                mypdfpage.Add(newpdftabpage2);
                                tblheight = (int)newpdftabpage2.Area.Height;
                                coltop += (int)tblheight + 25;
                                mypdfpage.SaveToDocument();
                                mypdfpage = mydoc.NewPage();
                                coltop = 40;
                                tbl_dept = mydoc.NewTable(Fontsmall1, dv.Count + 1 - (modcount * 20), 2, 2);
                                tbl_dept.VisibleHeaders = false;
                                tbl_dept.SetBorders(System.Drawing.Color.Black, 1, BorderType.CompleteGrid);
                                tbl_dept.Columns[0].SetWidth(300);
                                tbl_dept.Columns[1].SetWidth(100);
                                tbl_dept.Cell(0, 0).SetContentAlignment(ContentAlignment.MiddleCenter);
                                tbl_dept.Cell(0, 0).SetContent("Factors");
                                tbl_dept.Cell(0, 0).SetFont(Fontsmall1bold);
                                tbl_dept.Cell(0, 1).SetContentAlignment(ContentAlignment.MiddleCenter);
                                tbl_dept.Cell(0, 1).SetContent("Performance");
                                row = 1;
                                modcount++;
                            }
                            tbl_dept.Cell(row, 0).SetContentAlignment(ContentAlignment.MiddleLeft);
                            tbl_dept.Cell(row, 0).SetContent(dv[m - 1]["Question"]);
                            tbl_dept.Cell(row, 0).SetFont(Fontsmall1bold);
                            tbl_dept.Cell(row, 1).SetContentAlignment(ContentAlignment.MiddleCenter);
                            tbl_dept.Cell(row, 1).SetContent(dv[m - 1]["points"]);
                            row++;
                        }
                    }
                    newpdftabpage2 = tbl_dept.CreateTablePage(new Gios.Pdf.PdfArea(mydoc, 77, coltop, 450, 400));
                    mypdfpage.Add(newpdftabpage2);
                    tblheight = (int)newpdftabpage2.Area.Height;
                    coltop += (int)tblheight;
                    if (coltop >= mydoc.PageHeight - 340)
                    {
                        mypdfpage.SaveToDocument();
                        mypdfpage = mydoc.NewPage();
                        coltop = 20;
                    }
                    collinfo1 = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                   new PdfArea(mydoc, left1, coltop, 300, 50), System.Drawing.ContentAlignment.MiddleLeft, "Deviation from the Quality Plan, if any:");
                    mypdfpage.Add(collinfo1);
                    collinfo1 = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                  new PdfArea(mydoc, left1, coltop + 40, 300, 50), System.Drawing.ContentAlignment.MiddleLeft, "Date:");
                    mypdfpage.Add(collinfo1);
                    collinfo1 = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                new PdfArea(mydoc, 300, coltop + 40, 300, 50), System.Drawing.ContentAlignment.MiddleLeft, "Prepared by: __________________");
                    mypdfpage.Add(collinfo1);
                    collinfo1 = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                               new PdfArea(mydoc, left1, coltop + 70, 300, 50), System.Drawing.ContentAlignment.MiddleLeft, "-------------------------------------------------------------------------------------------------------------------------------");
                    mypdfpage.Add(collinfo1);
                    collinfo1 = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                new PdfArea(mydoc, left1, coltop + 100, 300, 50), System.Drawing.ContentAlignment.MiddleLeft, "Corrective action Planned");
                    mypdfpage.Add(collinfo1);
                    collinfo1 = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                new PdfArea(mydoc, 300, coltop + 100, 300, 50), System.Drawing.ContentAlignment.MiddleLeft, "Target Date: ");
                    mypdfpage.Add(collinfo1);
                    collinfo1 = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                              new PdfArea(mydoc, left1, coltop + tableheight + 135, 300, 50), System.Drawing.ContentAlignment.MiddleLeft, "Date:");
                    mypdfpage.Add(collinfo1);
                    collinfo1 = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                             new PdfArea(mydoc, 150, coltop + tableheight + 135, 300, 50), System.Drawing.ContentAlignment.MiddleLeft, "Proposed by: _______________");
                    mypdfpage.Add(collinfo1);
                    collinfo1 = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                             new PdfArea(mydoc, 350, coltop + tableheight + 135, 300, 50), System.Drawing.ContentAlignment.MiddleLeft, "Approved by: _______________");
                    mypdfpage.Add(collinfo1);
                    collinfo1 = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                             new PdfArea(mydoc, left1, coltop + tableheight + 170, 300, 50), System.Drawing.ContentAlignment.MiddleLeft, "-------------------------------------------------------------------------------------------------------------------------------");
                    mypdfpage.Add(collinfo1);
                    collinfo1 = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                 new PdfArea(mydoc, left1, coltop + tableheight + 200, 300, 50), System.Drawing.ContentAlignment.MiddleLeft, "Verification of Corrective Action");
                    mypdfpage.Add(collinfo1);
                    collinfo1 = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                              new PdfArea(mydoc, left1, coltop + tableheight + 230, 300, 50), System.Drawing.ContentAlignment.MiddleLeft, "Date:");
                    mypdfpage.Add(collinfo1);
                    collinfo1 = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                            new PdfArea(mydoc, 300, coltop + tableheight + 230, 300, 50), System.Drawing.ContentAlignment.MiddleLeft, "Verified by: __________________");
                    mypdfpage.Add(collinfo1);
                    mypdfpage.SaveToDocument();
                }
                string appPath = HttpContext.Current.Server.MapPath("~");
                if (appPath != "")
                {
                    string szPath = appPath + "/Report/";
                    string szFile = "STUDENTS_FEEDBACK" + DateTime.Now.ToString("ddMMyyyy") + DateTime.Now.ToString("HHMMss") + ".pdf";
                    Response.Buffer = true;
                    Response.Clear();
                    mydoc.SaveToFile(szPath + szFile);
                    Response.ClearHeaders();
                    Response.AddHeader("Content-Disposition", "attachment; filename=" + szFile);
                    Response.ContentType = "application/pdf";
                    Response.WriteFile(szPath + szFile);
                }
            }
        }
        catch
        {
        }
    }

    //29.08.16

    protected void rb_anonyms_farmate6_CheckedChanged(object sender, EventArgs e)
    {
        LblSec.Visible = false;
        UpSec.Visible = false;
        anonymousfilter1.Visible = false;
        anonymousfilter2.Visible = false;
        anonymousfilter3.Visible = true;
        lbl_headig.Text = "";
        anoynosformate4.Visible = false;
        staff_chart.Visible = false;

    }

    protected void cbl_clgnameformat6_SelectedIndexChanged(object sender, EventArgs e)
    {
        CallCheckboxListChange(cb_clgnameformat6, cbl_clgnameformat6, txtclgnameformat6, "College");
        bindformate6dept();
        bindformate6feedback();

    }

    protected void cb_clgnameformat6_CheckedChanged(object sender, EventArgs e)
    {
        CallCheckboxChange(cb_clgnameformat6, cbl_clgnameformat6, txtclgnameformat6, "College", "--Select--");
        bindformate6dept();
        bindformate6feedback();
    }

    //protected void cbl_deptnameformat6_SelectedIndexChanged(object sender, EventArgs e)
    //{
    //    CallCheckboxListChange(cb_deptnameformat6, cbl_deptnameformat6, txtdeptnameformat6, "Department");
    //    bindformate6staff();
    //    bindformate6feedback();
    //    bindformate6sem();
    //}
    //protected void cb_deptnameformat6_CheckedChanged(object sender, EventArgs e)
    //{
    //    CallCheckboxChange(cb_deptnameformat6, cbl_deptnameformat6, txtdeptnameformat6, "Department", "--Select--");
    //    bindformate6staff();
    //    bindformate6feedback();
    //    bindformate6sem();
    //}

    protected void ddlformate6_deptname_selectedindex(object sender, EventArgs e)
    {
        bindformate6staff();
        bindformate6feedback();
        bindformate6sem();
        bindsubjectformate6();
    }

    protected void ddl_feedbackformate6_SelectedIndexChanged(object sender, EventArgs e)
    {
        bindformate6staff();
        bindsubjectformate6();
    }

    protected void cbl_staffnameformat6_SelectedIndexChanged(object sender, EventArgs e)
    {
        CallCheckboxListChange(cb_staffnameformat6, cbl_staffnameformat6, txtstaffnameformat6, "Staff");
        bindsubjectformate6();
    }

    protected void cb_staffnameformat6_CheckedChanged(object sender, EventArgs e)
    {
        CallCheckboxChange(cb_staffnameformat6, cbl_staffnameformat6, txtstaffnameformat6, "Staff", "--Select--");
        bindsubjectformate6();
    }

    protected void cbl_formate6batch_SelectedIndexChanged(object sender, EventArgs e)
    {
        CallCheckboxListChange(cb_formate6batch, cbl_formate6batch, txt_formate6batch, "Batch");
    }

    protected void cb_formate6batch_CheckedChanged(object sender, EventArgs e)
    {
        CallCheckboxChange(cb_formate6batch, cbl_formate6batch, txt_formate6batch, "Batch", "--Select--");
    }

    private void CallCheckboxListChange(CheckBox cb, CheckBoxList cbl, TextBox txt, string dipst)
    {
        try
        {
            int sel = 0;
            int count = 0;
            cb.Checked = false;
            for (sel = 0; sel < cbl.Items.Count; sel++)
            {
                if (cbl.Items[sel].Selected == true)
                {
                    count++;
                }
            }
            if (count > 0)
            {
                if (count == 1)
                {
                    txt.Text = dipst + "(" + count + ")";
                }
                else
                {
                    txt.Text = dipst + "(" + count + ")";
                }
                if (cbl.Items.Count == count)
                {
                    cb.Checked = true;
                }
            }
            else
            {
                txt.Text = "--Select--";
            }
        }
        catch { }
    }

    private void CallCheckboxChange(CheckBox cb, CheckBoxList cbl, TextBox txt, string dispst, string deft)
    {
        try
        {
            int sel = 0;
            txt.Text = deft;
            if (cb.Checked == true)
            {
                for (sel = 0; sel < cbl.Items.Count; sel++)
                {
                    cbl.Items[sel].Selected = true;
                }
                if (cbl.Items.Count == 1)
                {
                    txt.Text = dispst + "(" + cbl.Items.Count + ")";
                }
                else
                {
                    txt.Text = dispst + "(" + cbl.Items.Count + ")";
                }
            }
            else
            {
                for (sel = 0; sel < cbl.Items.Count; sel++)
                {
                    cbl.Items[sel].Selected = false;
                }
                txt.Text = deft;
            }
        }
        catch { }
    }

    protected void bindformate6dept()
    {
        try
        {
            ds.Clear();
            //cbl_deptnameformat6.Items.Clear();
            ddlformate6_deptname.Items.Clear();
            string college_cd = returnwithsinglecodevalue(cbl_clgnameformat6);
            string query = " select distinct degree.degree_code,department.dept_name,department.dept_code from degree,department,course where course.course_id=degree.course_id and department.dept_code=degree.dept_code and course.college_code = degree.college_code and department.college_code = degree.college_code and degree.college_code in ('" + college_cd + "')";
            //string query = " select Dept_Code,Dept_Name from Department where college_code in ('" + college_cd + "') ";
            ds = d2.select_method_wo_parameter(query, "Text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                ddlformate6_deptname.DataSource = ds;
                ddlformate6_deptname.DataTextField = "Dept_Name";
                ddlformate6_deptname.DataValueField = "Dept_Code";
                ddlformate6_deptname.DataBind();

                //cbl_deptnameformat6.DataSource = ds;
                //cbl_deptnameformat6.DataTextField = "Dept_Name";
                //cbl_deptnameformat6.DataValueField = "Dept_Code";
                //cbl_deptnameformat6.DataBind();
                //if (cbl_deptnameformat6.Items.Count > 0)
                //{
                //    for (int row = 0; row < cbl_deptnameformat6.Items.Count; row++)
                //    {
                //        cbl_deptnameformat6.Items[row].Selected = true;
                //    }
                //    cb_deptnameformat6.Checked = true;
                //    txtdeptnameformat6.Text = "Department(" + cbl_deptnameformat6.Items.Count + ")";
                //}
            }
            else
            {
                //txtdeptnameformat6.Text = "--Select--";
                ddlformate6_deptname.Items.Add(new System.Web.UI.WebControls.ListItem("Select", "0"));
            }
        }
        catch { }
    }

    protected void bindformate6feedback()
    {
        try
        {
            ddl_feedbackformate6.Items.Clear();
            collegecode = "";
            collegecode = returnwithsinglecodevalue(cbl_clgnameformat6);
            //string degreecode = returnwithsinglecodevalue(cbl_deptnameformat6);
            string degreecode = Convert.ToString(ddlformate6_deptname.SelectedItem.Value);
            string batchyear = returnwithsinglecodevalue(cbl_formate6batch);
            ds.Clear();
            string q1 = ""; string empty = "";
            if (degreecode.Trim() != "")
            {
                q1 = " select d.Degree_Code from Degree d,Department dt where d.Dept_Code =dt.Dept_Code and d.Dept_Code in('" + degreecode + "')";
                ds = d2.select_method_wo_parameter(q1, "text");
                empty = "";
                if (ds.Tables[0].Rows.Count > 0)
                {
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        if (empty == "")
                        {
                            empty = Convert.ToString(ds.Tables[0].Rows[i]["Degree_Code"]);
                        }
                        else
                        {
                            empty = empty + "','" + Convert.ToString(ds.Tables[0].Rows[i]["Degree_Code"]);
                        }
                    }
                }
            }
            q1 = "";
            if (empty.Trim() == "")
            {
                q1 = "select distinct FeedBackName  from CO_FeedBackMaster where CollegeCode in('" + collegecode + "')  and FeedBackType='1' and Batch_Year in('" + batchyear + "') ";
            }
            else
            {
                q1 = "select distinct FeedBackName  from CO_FeedBackMaster where CollegeCode in('" + collegecode + "') and DegreeCode in('" + empty + "') and FeedBackType='1' and Batch_Year in('" + batchyear + "') ";
            }
            ds.Clear();
            ds = d2.select_method_wo_parameter(q1, "Text");
            if (ds.Tables[0].Rows.Count == 0)//13.10.16
            {
                ds.Clear();
                q1 = " select distinct FeedBackName  from CO_FeedBackMaster where CollegeCode in('" + collegecode + "') ";
                ds = d2.select_method_wo_parameter(q1, "Text");
            }
            if (ds.Tables[0].Rows.Count > 0)
            {
                ddl_feedbackformate6.DataSource = ds;
                ddl_feedbackformate6.DataTextField = "FeedBackName";
                ddl_feedbackformate6.DataValueField = "FeedBackName";
                ddl_feedbackformate6.DataBind();
                ddl_feedbackformate6.Items.Insert(0, "--Select--");
            }
            else
            {
                ddl_feedbackformate6.Items.Insert(0, "--Select--");
            }
        }
        catch { }
    }

    protected void bindformate6staff()
    {
        try
        {
            ds.Clear();
            cbl_staffnameformat6.Items.Clear(); string degreecode = "";
            if (ddlformate6_deptname.SelectedItem.Text.Trim() != "0")
            {
                degreecode = Convert.ToString(ddlformate6_deptname.SelectedItem.Value);
            }
            //string degreecode = returnwithsinglecodevalue(cbl_deptnameformat6);
            string query = " select s.staff_code,s.staff_name,sa.appl_id from staff_appl_master sa,staffmaster s,stafftrans t where sa.appl_no =s.appl_no and s.staff_code =t.staff_code and t.latestrec =1 and s.resign =0 and s.settled =0 and t.dept_code in ('" + degreecode + "') order by s.staff_name";
            ds.Clear();
            ds = d2.select_method_wo_parameter(query, "Text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                cbl_staffnameformat6.DataSource = ds;
                cbl_staffnameformat6.DataTextField = "staff_name";
                cbl_staffnameformat6.DataValueField = "appl_id";
                cbl_staffnameformat6.DataBind();
                if (cbl_staffnameformat6.Items.Count > 0)
                {
                    for (int row = 0; row < cbl_staffnameformat6.Items.Count; row++)
                    {
                        cbl_staffnameformat6.Items[row].Selected = true;
                    }
                    cb_staffnameformat6.Checked = true;
                    txtstaffnameformat6.Text = "Staff(" + cbl_staffnameformat6.Items.Count + ")";
                }
            }
            else
            {
                txtstaffnameformat6.Text = "--Select--";
            }
        }
        catch { }
    }

    protected string returnwithsinglecodevalue(CheckBoxList cb)
    {
        string empty = "";
        for (int i = 0; i < cb.Items.Count; i++)
        {
            if (cb.Items[i].Selected == true)
            {
                if (empty == "")
                {
                    empty = Convert.ToString(cb.Items[i].Value);
                }
                else
                {
                    empty = empty + "','" + Convert.ToString(cb.Items[i].Value);
                }
            }
        }
        return empty;
    }

    //07.09.16
    protected void imagebtnpopclose1_Click(object sender, EventArgs e)
    {
        popupquestiondet.Visible = false;
    }

    protected void FpSpread3_OnCellClick(object sender, EventArgs e)
    {
        cellclk = true;
    }

    protected void FpSpread3_Selectedindexchange(object sender, EventArgs e)
    {
        try
        {
            if (cellclk == true)
            {
                string activerow = FpSpread3.ActiveSheetView.ActiveRow.ToString();
                string activecol = FpSpread3.ActiveSheetView.ActiveColumn.ToString();
                string degreecode = Convert.ToString(FpSpread3.Sheets[0].Cells[Convert.ToInt32(activerow), 1].Tag);
                string sem = Convert.ToString(FpSpread3.Sheets[0].Cells[Convert.ToInt32(activerow), 2].Text);
                string sec = Convert.ToString(FpSpread3.Sheets[0].Cells[Convert.ToInt32(activerow), 3].Text);
                string batchyear = Convert.ToString(FpSpread3.Sheets[0].Cells[Convert.ToInt32(activerow), 4].Text);
                string clgcode = returnwithsinglecodevalue(Cbl_college);
                if (activecol == "7" && Convert.ToString(FpSpread3.Sheets[0].Cells[Convert.ToInt32(activerow), Convert.ToInt32(activecol)].Text) != "")
                {
                    if (ddl_Feedbackname.SelectedItem.Text.Trim() != "Select")
                    {
                        string type = "";
                        if (rb_Acad.Checked == true)
                        {
                            type = "1";
                        }
                        else if (rb_Gend.Checked == true)
                        {
                            type = "2";
                        }
                        txt_questionreport.Text = ""; Label2.Visible = false;
                        Fpreadheaderbindmethod("S No-50/Unique Code-150/StaffCode & StaffName-250/Subject Name-250", Fpspread5, "false");
                        //string q1 = "  select  s.FeedbackUnicode ,s.StaffApplNo,sm.staff_code +' - '+staff_name as staff ,f.FeedBackMasterPK,Batch_Year ,f.semester,f.DegreeCode,f.Section,SubjectNo,c.subject_name from CO_FeedBackMaster F,CO_StudFeedBack S,staff_appl_master sa,staffmaster sm ,CO_FeedbackUniCode FU,subject c  where c.subject_no=s.SubjectNo and sa.appl_no=sm.appl_no and fu.FeedbackUnicode =s.FeedbackUnicode and fu.FeedbackMasterFK =f.FeedBackMasterPK and fu.FeedbackMasterFK =s.FeedBackMasterFK and sa.appl_id=s.StaffApplNo and s.FeedBackMasterFK =f.FeedBackMasterPK  and   f.degreecode in ('" + degreecode + "') and f.Batch_Year in('" + batchyear + "') and f.semester in ('" + sem + "')  and f.Section in ('" + sec + "') and f.InclueCommon='1' and s.FeedbackUnicode<>'' group by staff_code,staff_name,f.FeedBackMasterPK  ,StaffApplNo,Batch_Year ,f.semester,f.DegreeCode ,f.Section,s.FeedbackUnicode,subject_name,SubjectNo order by s.FeedbackUnicode";
                        string q1 = " select  s.FeedbackUnicode ,s.StaffApplNo,sm.staff_code +' - '+staff_name as staff ,f.FeedBackMasterPK,Batch_Year ,f.semester,f.DegreeCode,f.Section,SubjectNo,c.subject_name from CO_FeedBackMaster F,CO_StudFeedBack S,staff_appl_master sa,staffmaster sm ,subject c  where c.subject_no=s.SubjectNo and sa.appl_no=sm.appl_no  and sa.appl_id=s.StaffApplNo and s.FeedBackMasterFK =f.FeedBackMasterPK  and   f.degreecode in('" + degreecode + "') and f.Batch_Year in('" + batchyear + "') and f.semester in ('" + sem + "')  and f.Section in ('" + sec + "') and f.InclueCommon='1' and s.FeedbackUnicode<>'' group by staff_code,staff_name,f.FeedBackMasterPK  ,StaffApplNo,Batch_Year ,f.semester,f.DegreeCode ,f.Section,s.FeedbackUnicode,subject_name,SubjectNo order by s.FeedbackUnicode";
                        //q1 = q1 + "  SELECT Question,QuestionMasterPK FROM CO_StudFeedBack F,CO_FeedBackMaster B,CO_QuestionMaster Q,CO_MarkMaster M ,CO_FeedbackUniCode fu WHERE fu.FeedbackUnicode=f.FeedbackUnicode and fu.FeedbackMasterFK =f.FeedBackMasterFK and b.FeedBackMasterPK =fu.FeedbackMasterFK and  F.FeedBackMasterFK = B.FeedBackMasterPK AND F.QuestionMasterFK =  Q.QuestionMasterPK AND F.MarkMasterPK = M.MarkMasterPK AND  b.InclueCommon='1' and FeedBackType = '" + type + "' and B.FeedBackName='" + Convert.ToString(ddl_Feedbackname.SelectedItem.Text) + "'  and B.CollegeCode in ('" + clgcode + "') GROUP BY FeedBackName,Question,QuestionMasterPK";
                        q1 = q1 + "   SELECT distinct Question,QuestionMasterPK,HeaderCode FROM CO_FeedBackMaster B,CO_QuestionMaster Q ,CO_FeedBackQuestions FB WHERE  b.FeedBackMasterPK =fb.FeedBackMasterFK and q.QuestionMasterPK =fb.QuestionMasterFK and  InclueCommon='1' and FeedBackType = '" + type + "' and B.FeedBackName='" + Convert.ToString(ddl_Feedbackname.SelectedItem.Text) + "'  and B.CollegeCode in ('" + clgcode + "') order by HeaderCode";
                        //q1 = q1 + "     SELECT f.FeedbackUnicode,StaffApplNo,Question,M.Point as points,QuestionMasterPK,SubjectNo FROM CO_StudFeedBack F,CO_FeedBackMaster B,CO_QuestionMaster Q,CO_MarkMaster M,CO_FeedbackUniCode fu WHERE fu.FeedbackUnicode=f.FeedbackUnicode and fu.FeedbackMasterFK=f.FeedBackMasterFK and F.FeedBackMasterFK = B.FeedBackMasterPK AND F.QuestionMasterFK =  Q.QuestionMasterPK AND F.MarkMasterPK = M.MarkMasterPK AND  b.InclueCommon='1' and FeedBackType = '1' and B.FeedBackName='" + Convert.ToString(ddl_Feedbackname.SelectedItem.Text) + "' and B.CollegeCode in ('" + clgcode + "') GROUP BY FeedBackName,Question,StaffApplNo,f.FeedbackUnicode,QuestionMasterPK,SubjectNo";
                        q1 = q1 + "     SELECT f.FeedbackUnicode,StaffApplNo,M.Point as points,QuestionMasterfK,SubjectNo FROM CO_StudFeedBack F,CO_FeedBackMaster B,CO_MarkMaster M,CO_FeedbackUniCode fu WHERE fu.FeedbackUnicode=f.FeedbackUnicode and fu.FeedbackMasterFK=f.FeedBackMasterFK and F.FeedBackMasterFK = B.FeedBackMasterPK AND F.MarkMasterPK = M.MarkMasterPK AND  b.InclueCommon='1' and FeedBackType = '" + type + "' and B.FeedBackName ='" + Convert.ToString(ddl_Feedbackname.SelectedItem.Text) + "' and B.CollegeCode in ('" + clgcode + "')  and   b.degreecode in ('" + degreecode + "') and b.Batch_Year in('" + batchyear + "') and b.semester in ('" + sem + "')  and b.Section in ('" + sec + "')";
                        q1 = q1 + "  select max(Point)Point  from CO_MarkMaster ";
                        ds.Clear();
                        ds = d2.select_method_wo_parameter(q1, "text");
                        if (ds.Tables[1].Rows.Count > 0)
                        {
                            for (int i = 0; i < ds.Tables[1].Rows.Count; i++)
                            {
                                Fpspread5.Sheets[0].ColumnCount++;
                                Fpspread5.Sheets[0].ColumnHeader.Cells[0, Fpspread5.Sheets[0].ColumnCount - 1].Text = Convert.ToString(ds.Tables[1].Rows[i]["Question"]);
                                Fpspread5.Sheets[0].ColumnHeader.Cells[0, Fpspread5.Sheets[0].ColumnCount - 1].Tag = Convert.ToString(ds.Tables[1].Rows[i]["QuestionMasterPK"]);
                                Fpspread5.Sheets[0].ColumnHeader.Cells[0, Fpspread5.Sheets[0].ColumnCount - 1].Font.Size = FontUnit.Medium;
                                Fpspread5.Sheets[0].ColumnHeader.Cells[0, Fpspread5.Sheets[0].ColumnCount - 1].Font.Name = "Book Antiqua";
                                Fpspread5.Sheets[0].ColumnHeader.Cells[0, Fpspread5.Sheets[0].ColumnCount - 1].Font.Bold = true;
                                Fpspread5.Columns[Fpspread5.Sheets[0].ColumnCount - 1].Width = 300;
                            }
                            Fpspread5.Sheets[0].ColumnCount++;
                            Fpspread5.Sheets[0].ColumnHeader.Cells[0, Fpspread5.Sheets[0].ColumnCount - 1].Text = "Grant Total";
                            Fpspread5.Sheets[0].ColumnCount++;
                            Fpspread5.Sheets[0].ColumnHeader.Cells[0, Fpspread5.Sheets[0].ColumnCount - 1].Text = "Max Total";
                            Fpspread5.Sheets[0].ColumnCount++;
                            Fpspread5.Sheets[0].ColumnHeader.Cells[0, Fpspread5.Sheets[0].ColumnCount - 1].Text = "Percentage";
                            Fpspread5.Sheets[0].ColumnHeader.Cells[0, Fpspread5.Columns.Count - 1].Font.Size = FontUnit.Medium;
                            Fpspread5.Sheets[0].ColumnHeader.Cells[0, Fpspread5.Columns.Count - 1].Font.Name = "Book Antiqua";
                            Fpspread5.Sheets[0].ColumnHeader.Cells[0, Fpspread5.Columns.Count - 1].Font.Bold = true;
                            Fpspread5.Sheets[0].ColumnHeader.Cells[0, Fpspread5.Columns.Count - 2].Font.Size = FontUnit.Medium;
                            Fpspread5.Sheets[0].ColumnHeader.Cells[0, Fpspread5.Columns.Count - 2].Font.Name = "Book Antiqua";
                            Fpspread5.Sheets[0].ColumnHeader.Cells[0, Fpspread5.Columns.Count - 2].Font.Bold = true;
                            Fpspread5.Sheets[0].ColumnHeader.Cells[0, Fpspread5.Columns.Count - 3].Font.Size = FontUnit.Medium;
                            Fpspread5.Sheets[0].ColumnHeader.Cells[0, Fpspread5.Columns.Count - 3].Font.Name = "Book Antiqua";
                            Fpspread5.Sheets[0].ColumnHeader.Cells[0, Fpspread5.Columns.Count - 3].Font.Bold = true;
                        }
                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            DataView dv = new DataView(); int m = 2;
                            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                            {
                                m++;
                                Fpspread5.Sheets[0].RowCount++;
                                Fpspread5.Sheets[0].Cells[Fpspread5.Sheets[0].RowCount - 1, 0].Text = Convert.ToString(i + 1);
                                Fpspread5.Sheets[0].Cells[Fpspread5.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
                                Fpspread5.Sheets[0].Cells[Fpspread5.Sheets[0].RowCount - 1, 0].Font.Size = FontUnit.Medium;
                                Fpspread5.Sheets[0].Cells[Fpspread5.Sheets[0].RowCount - 1, 0].Font.Name = "Book Antiqua";
                                Fpspread5.Sheets[0].Cells[Fpspread5.Sheets[0].RowCount - 1, 1].Text = Convert.ToString(ds.Tables[0].Rows[i]["FeedbackUnicode"]);
                                Fpspread5.Sheets[0].Cells[Fpspread5.Sheets[0].RowCount - 1, 1].VerticalAlign = VerticalAlign.Middle;
                                Fpspread5.Sheets[0].Cells[Fpspread5.Sheets[0].RowCount - 1, 1].HorizontalAlign = HorizontalAlign.Left;
                                Fpspread5.Sheets[0].Cells[Fpspread5.Sheets[0].RowCount - 1, 1].Font.Size = FontUnit.Medium;
                                Fpspread5.Sheets[0].Cells[Fpspread5.Sheets[0].RowCount - 1, 1].Font.Name = "Book Antiqua";
                                Fpspread5.Sheets[0].Cells[Fpspread5.Sheets[0].RowCount - 1, 2].Text = Convert.ToString(ds.Tables[0].Rows[i]["staff"]);
                                Fpspread5.Sheets[0].Cells[Fpspread5.Sheets[0].RowCount - 1, 2].HorizontalAlign = HorizontalAlign.Left;
                                Fpspread5.Sheets[0].Cells[Fpspread5.Sheets[0].RowCount - 1, 2].Font.Size = FontUnit.Medium;
                                Fpspread5.Sheets[0].Cells[Fpspread5.Sheets[0].RowCount - 1, 2].Font.Name = "Book Antiqua";
                                Fpspread5.Sheets[0].Cells[Fpspread5.Sheets[0].RowCount - 1, 3].Text = Convert.ToString(ds.Tables[0].Rows[i]["subject_name"]);
                                Fpspread5.Sheets[0].Cells[Fpspread5.Sheets[0].RowCount - 1, 3].HorizontalAlign = HorizontalAlign.Left;
                                Fpspread5.Sheets[0].Cells[Fpspread5.Sheets[0].RowCount - 1, 3].Font.Size = FontUnit.Medium;
                                Fpspread5.Sheets[0].Cells[Fpspread5.Sheets[0].RowCount - 1, 3].Font.Name = "Book Antiqua";
                                double gtotal = 0; double mtotal = 0; double avgper = 0;
                                for (int j = 5; j <= Fpspread5.Columns.Count - 3; j++)
                                {
                                    string questionmasterPK = Convert.ToString(Fpspread5.Sheets[0].ColumnHeader.Cells[0, j - 1].Tag);
                                    ds.Tables[2].DefaultView.RowFilter = " QuestionMasterfK ='" + questionmasterPK + "' and StaffApplNo='" + ds.Tables[0].Rows[i]["StaffApplNo"] + "' and FeedbackUnicode='" + ds.Tables[0].Rows[i]["FeedbackUnicode"] + "' and SubjectNo='" + Convert.ToString(ds.Tables[0].Rows[i]["SubjectNo"]) + "'";
                                    dv = ds.Tables[2].DefaultView;
                                    if (dv.Count > 0)
                                    {
                                        Fpspread5.Sheets[0].Cells[Fpspread5.Sheets[0].RowCount - 1, j - 1].Text = Convert.ToString(dv[0]["points"]);
                                        Fpspread5.Sheets[0].Cells[Fpspread5.Sheets[0].RowCount - 1, j - 1].HorizontalAlign = HorizontalAlign.Center;
                                        Fpspread5.Sheets[0].Cells[Fpspread5.Sheets[0].RowCount - 1, j - 1].Font.Size = FontUnit.Medium;
                                        Fpspread5.Sheets[0].Cells[Fpspread5.Sheets[0].RowCount - 1, j - 1].Font.Name = "Book Antiqua";
                                        Fpspread5.Sheets[0].Cells[Fpspread5.Sheets[0].RowCount - 1, j - 1].Font.Bold = true;
                                        string point = Convert.ToString(dv[0]["points"]);
                                        if (point.Trim() == "")
                                            point = "0";
                                        gtotal += Convert.ToDouble(point);
                                    }
                                    else
                                    {
                                        Fpspread5.Sheets[0].Cells[Fpspread5.Sheets[0].RowCount - 1, j - 1].Text = "-";
                                        Fpspread5.Sheets[0].Cells[Fpspread5.Sheets[0].RowCount - 1, j - 1].HorizontalAlign = HorizontalAlign.Center;
                                    }
                                    Fpspread5.Columns[j].Locked = true;
                                    Fpspread5.Columns[4].Locked = true;
                                }
                                if (ds.Tables[3].Rows.Count > 0)
                                {
                                    mtotal = Convert.ToDouble(ds.Tables[1].Rows.Count) * Convert.ToDouble(ds.Tables[3].Rows[0][0]);
                                }
                                Fpspread5.Sheets[0].Cells[Fpspread5.Sheets[0].RowCount - 1, Fpspread5.Columns.Count - 3].Text = Convert.ToString(gtotal);
                                Fpspread5.Sheets[0].Cells[Fpspread5.Sheets[0].RowCount - 1, Fpspread5.Columns.Count - 3].HorizontalAlign = HorizontalAlign.Center;
                                Fpspread5.Sheets[0].Cells[Fpspread5.Sheets[0].RowCount - 1, Fpspread5.Columns.Count - 2].Text = Convert.ToString(Math.Round(mtotal, 0));
                                Fpspread5.Sheets[0].Cells[Fpspread5.Sheets[0].RowCount - 1, Fpspread5.Columns.Count - 2].HorizontalAlign = HorizontalAlign.Center;
                                //avgper = gtotal / Convert.ToDouble(ds.Tables[1].Rows.Count);
                                avgper = gtotal / mtotal * 100;
                                Fpspread5.Sheets[0].Cells[Fpspread5.Sheets[0].RowCount - 1, Fpspread5.Columns.Count - 1].Text = Convert.ToString(Math.Round(avgper, 0));
                                Fpspread5.Sheets[0].Cells[Fpspread5.Sheets[0].RowCount - 1, Fpspread5.Columns.Count - 1].HorizontalAlign = HorizontalAlign.Center;
                                Fpspread5.Sheets[0].Cells[Fpspread5.Sheets[0].RowCount - 1, Fpspread5.Columns.Count - 1].Font.Size = FontUnit.Medium;
                                Fpspread5.Sheets[0].Cells[Fpspread5.Sheets[0].RowCount - 1, Fpspread5.Columns.Count - 1].Font.Name = "Book Antiqua";
                                Fpspread5.Sheets[0].Cells[Fpspread5.Sheets[0].RowCount - 1, Fpspread5.Columns.Count - 1].Font.Bold = true;
                                Fpspread5.Sheets[0].Cells[Fpspread5.Sheets[0].RowCount - 1, Fpspread5.Columns.Count - 2].Font.Size = FontUnit.Medium;
                                Fpspread5.Sheets[0].Cells[Fpspread5.Sheets[0].RowCount - 1, Fpspread5.Columns.Count - 2].Font.Name = "Book Antiqua";
                                Fpspread5.Sheets[0].Cells[Fpspread5.Sheets[0].RowCount - 1, Fpspread5.Columns.Count - 2].Font.Bold = true;
                                Fpspread5.Sheets[0].Cells[Fpspread5.Sheets[0].RowCount - 1, Fpspread5.Columns.Count - 3].Font.Size = FontUnit.Medium;
                                Fpspread5.Sheets[0].Cells[Fpspread5.Sheets[0].RowCount - 1, Fpspread5.Columns.Count - 3].Font.Name = "Book Antiqua";
                                Fpspread5.Sheets[0].Cells[Fpspread5.Sheets[0].RowCount - 1, Fpspread5.Columns.Count - 3].Font.Bold = true;
                                Fpspread5.Columns[Fpspread5.Columns.Count - 1].Locked = true;
                                Fpspread5.Columns[Fpspread5.Columns.Count - 2].Locked = true;
                                Fpspread5.Columns[Fpspread5.Columns.Count - 3].Locked = true;
                                Fpspread5.Columns[0].Locked = true;
                                Fpspread5.Columns[1].Locked = true;
                                Fpspread5.Columns[2].Locked = true;
                                Fpspread5.Columns[3].Locked = true;
                                Fpspread5.Visible = true;
                            }
                            Fpspread5.Sheets[0].PageSize = Fpspread5.Rows.Count;
                            Fpspread5.Sheets[0].SetColumnMerge(1, FarPoint.Web.Spread.Model.MergePolicy.Always);
                            Fpspread5.Sheets[0].SetColumnMerge(2, FarPoint.Web.Spread.Model.MergePolicy.Always);
                            popupquestiondet.Visible = true;
                            reportdivquestiondet.Visible = true;
                        }
                        else
                        {
                            Fpspread5.Visible = false;
                            reportdivquestiondet.Visible = false;
                        }
                    }
                    else
                    {
                        Fpspread5.Visible = false;
                        reportdivquestiondet.Visible = false;
                        imgdiv2.Visible = true;
                        lbl_alert1.Text = "Please Select Feedback Name";
                    }
                }
            }
        }
        catch (Exception ex)
        {
            imgdiv2.Visible = true;
            lbl_alert1.Text = ex.ToString();
            d2.sendErrorMail(ex, collegecode1, "Fpspread 3 Selectedindexchange");
        }
    }

    protected void btn_quest_Click(object sender, EventArgs e)
    {
        if (txt_questionreport.Text.Trim() != "")
        {
            d2.printexcelreport(Fpspread5, txt_questionreport.Text);
        }
        else
        {
            Label2.Visible = true;
        }
    }

    protected void btn_questprint_Click(object sender, EventArgs e)
    {
        try
        {
            // string degreedetails = "Staff Evaluation Report"; delsi1703
            string degreedetails = "Department Wise Feedback";
            string pagename = "Feedback_report.aspx";
            Printmaster1.loadspreaddetails(Fpspread5, pagename, degreedetails);
            Printmaster1.Visible = true;
        }
        catch
        {
        }
    }

    public void Fpreadheaderbindmethod(string headername, FarPoint.Web.Spread.FpSpread spreadname, string AutoPostBack)
    {
        try
        {
            string[] header = headername.Split('/');
            int k = 0;
            if (AutoPostBack.Trim().ToUpper() == "TRUE")
            {
                if (header.Length > 0)
                {
                    spreadname.Sheets[0].RowCount = 0;
                    spreadname.Sheets[0].ColumnCount = 0;
                    spreadname.CommandBar.Visible = false;
                    spreadname.Sheets[0].AutoPostBack = true;
                    spreadname.Sheets[0].ColumnHeader.RowCount = 1;
                    spreadname.Sheets[0].RowHeader.Visible = false;
                    FarPoint.Web.Spread.StyleInfo darkstyle = new FarPoint.Web.Spread.StyleInfo();
                    darkstyle.BackColor = ColorTranslator.FromHtml("#0CA6CA");
                    darkstyle.ForeColor = System.Drawing.Color.White;
                    spreadname.ActiveSheetView.ColumnHeader.DefaultStyle = darkstyle;
                    foreach (string head in header)
                    {
                        k++;
                        spreadname.Sheets[0].ColumnCount = Convert.ToInt32(header.Length);
                        if (head.Trim().ToUpper() == "S.NO")
                        {
                            spreadname.Sheets[0].ColumnHeader.Cells[0, k - 1].Text = head;
                            spreadname.Sheets[0].ColumnHeader.Cells[0, k - 1].Font.Bold = true;
                            spreadname.Sheets[0].ColumnHeader.Cells[0, k - 1].HorizontalAlign = HorizontalAlign.Center;
                            spreadname.Sheets[0].ColumnHeader.Cells[0, k - 1].Font.Name = "Book Antiqua";
                            spreadname.Sheets[0].ColumnHeader.Cells[0, k - 1].Font.Size = FontUnit.Medium;
                            spreadname.Columns[k - 1].Width = 50;
                        }
                        else
                        {
                            spreadname.Sheets[0].ColumnHeader.Cells[0, k - 1].Text = head;
                            spreadname.Sheets[0].ColumnHeader.Cells[0, k - 1].Font.Bold = true;
                            spreadname.Sheets[0].ColumnHeader.Cells[0, k - 1].HorizontalAlign = HorizontalAlign.Center;
                            spreadname.Sheets[0].ColumnHeader.Cells[0, k - 1].Font.Name = "Book Antiqua";
                            spreadname.Sheets[0].ColumnHeader.Cells[0, k - 1].Font.Size = FontUnit.Medium;
                            spreadname.Columns[k - 1].Width = 200;
                        }
                    }
                }
            }
            else if (AutoPostBack.Trim().ToUpper() == "FALSE")
            {
                if (header.Length > 0)
                {
                    spreadname.Sheets[0].RowCount = 0;
                    spreadname.Sheets[0].ColumnCount = 0;
                    spreadname.CommandBar.Visible = false;
                    spreadname.Sheets[0].AutoPostBack = false;
                    spreadname.Sheets[0].ColumnHeader.RowCount = 1;
                    spreadname.Sheets[0].RowHeader.Visible = false;
                    FarPoint.Web.Spread.StyleInfo darkstyle = new FarPoint.Web.Spread.StyleInfo();
                    darkstyle.BackColor = ColorTranslator.FromHtml("#0CA6CA");
                    darkstyle.ForeColor = System.Drawing.Color.White;
                    spreadname.ActiveSheetView.ColumnHeader.DefaultStyle = darkstyle;
                    foreach (string head in header)
                    {
                        k++;
                        string[] width = head.Split('-');
                        spreadname.Sheets[0].ColumnCount = Convert.ToInt32(header.Length);
                        if (Convert.ToString(width[0]).Trim().ToUpper() == "S.NO")
                        {
                            spreadname.Sheets[0].ColumnHeader.Cells[0, k - 1].Text = Convert.ToString(width[0]);
                            spreadname.Sheets[0].ColumnHeader.Cells[0, k - 1].Font.Bold = true;
                            spreadname.Sheets[0].ColumnHeader.Cells[0, k - 1].HorizontalAlign = HorizontalAlign.Center;
                            spreadname.Sheets[0].ColumnHeader.Cells[0, k - 1].Font.Name = "Book Antiqua";
                            spreadname.Sheets[0].ColumnHeader.Cells[0, k - 1].Font.Size = FontUnit.Medium;
                            spreadname.Columns[k - 1].Width = Convert.ToInt32(width[1]);
                        }
                        else
                        {
                            spreadname.Sheets[0].ColumnHeader.Cells[0, k - 1].Text = Convert.ToString(width[0]);
                            spreadname.Sheets[0].ColumnHeader.Cells[0, k - 1].Font.Bold = true;
                            spreadname.Sheets[0].ColumnHeader.Cells[0, k - 1].HorizontalAlign = HorizontalAlign.Center;
                            spreadname.Sheets[0].ColumnHeader.Cells[0, k - 1].Font.Name = "Book Antiqua";
                            spreadname.Sheets[0].ColumnHeader.Cells[0, k - 1].Font.Size = FontUnit.Medium;
                            spreadname.Columns[k - 1].Width = Convert.ToInt32(width[1]);
                        }
                    }
                }
            }
        }
        catch (Exception ex)
        {
            imgdiv2.Visible = true;
            lbl_alert1.Visible = true;
            lbl_alert1.Font.Size = FontUnit.Smaller;
            lbl_alert1.Text = ex.ToString();
        }
    }

    protected void cb_formate6sem_CheckedChanged(object sender, EventArgs e)
    {
        CallCheckboxChange(cb_formate6sem, cbl_formate6sem, txt_formate6sem, "Semester", "--Select--");
    }

    protected void cbl_formate6sem_SelectedIndexChanged(object sender, EventArgs e)
    {
        CallCheckboxListChange(cb_formate6sem, cbl_formate6sem, txt_formate6sem, "Semester");
    }

    protected void bindformate6sem()
    {
        string max = d2.GetFunction("select  distinct MAX(duration) from degree where college_code in('" + collegecode + "')  ");
        if (Convert.ToInt32(max) > 0)
        {
            cbl_formate6sem.Items.Clear();
            for (int row = 0; row < Convert.ToInt32(max); row++)
            {
                cbl_formate6sem.Items.Add(new System.Web.UI.WebControls.ListItem((row + 1).ToString(), (row + 1).ToString()));
                cbl_formate6sem.Items[row].Selected = true;
                cb_formate6sem.Checked = true;
            }
            txt_formate6sem.Text = "Semester(" + cbl_formate6sem.Items.Count + ")";
        }
    }

    protected void rb_anonyms_farmate7_CheckedChanged(object sender, EventArgs e)
    {
        staff_chart.Visible = false;
    }

    protected void ananames7()
    {
        try
        {
            string degreecode = returnwithsinglecodevalue(cbl_branch);
            string sem = returnwithsinglecodevalue(cbl_sem);
            string batchyear = returnwithsinglecodevalue(cbl_batch);
            string clgcode = returnwithsinglecodevalue(Cbl_college);
            string sec = "";
            for (int i = 0; i < cbl_sec.Items.Count; i++)
            {
                if (cbl_sec.Items[i].Selected == true)
                {
                    if (sec == "")
                    {
                        sec = "" + cbl_sec.Items[i].Value.ToString() + "";
                    }
                    else
                    {
                        sec = sec + "','" + cbl_sec.Items[i].Value.ToString() + "";
                    }
                    if (cbl_sec.Items[i].Value == "Empty")
                    {
                        sec = "";
                    }
                }
            }
            if (sec.Trim() != "")
            {
                sec = sec + "','";
            }
            if (ddl_Feedbackname.SelectedItem.Text.Trim() != "Select")
            {
                string type = "";
                if (rb_Acad.Checked == true)
                {
                    type = "1";
                }
                else if (rb_Gend.Checked == true)
                {
                    type = "2";
                }
                string fbpk = " select FeedBackMasterPK,ISNULL(issubjecttype,0)issubjecttype from CO_FeedBackMaster where FeedBackName ='" + ddl_Feedbackname.SelectedItem.Value + "' and DegreeCode in ('" + degreecode + "') and semester in ('" + sem + "') and Batch_Year in('" + batchyear + "') and section in ('" + sec + "')";
                dsfb = d2.select_method_wo_parameter(fbpk, "Text");
                string feedbakpk = string.Empty;
                string issubjecttype = string.Empty;
                if (dsfb.Tables.Count > 0)
                {
                    if (dsfb.Tables[0].Rows.Count > 0)
                    {
                        issubjecttype = Convert.ToString(dsfb.Tables[0].Rows[0]["issubjecttype"]);
                        for (int pk = 0; pk < dsfb.Tables[0].Rows.Count; pk++)
                        {
                            if (feedbakpk == "")
                            {
                                feedbakpk = dsfb.Tables[0].Rows[pk]["FeedBackMasterPK"].ToString();
                            }
                            else
                            {
                                feedbakpk = feedbakpk + "','" + dsfb.Tables[0].Rows[pk]["FeedBackMasterPK"].ToString() + "";
                            }
                        }
                    }
                }
                txt_questionreport.Text = "";
                Label2.Visible = false;
                Fpreadheaderbindmethod("S No-50/Department-200/StaffCode & StaffName-250/Subject Code-150/Subject Name-250/SubjectType-100", FpSpread1, "false");//
                string selqry = " select count( distinct S.FeedbackUnicode)Strength,SUM(M.Point)Points,(convert(varchar(10), f.Batch_Year)+'-'+co.Course_Name+'-'+ dt.dept_acronym+'-'+convert(varchar(10), f.Semester)+'-'+f.Section ) as department,c.subject_code, s.StaffApplNo,sm.staff_code +' - '+staff_name as staff ,f.FeedBackMasterPK,Batch_Year , f.semester,f.DegreeCode,f.Section,SubjectNo,c.subject_name,sm.staff_code,staff_name,c.acronym from CO_FeedBackMaster F,CO_StudFeedBack S,staff_appl_master sa,staffmaster sm ,subject c,Department dt,course co,Degree d  ,CO_MarkMaster M where M.MarkMasterPK =S.MarkMasterPK and  d.Degree_Code =f.degreecode and dt.Dept_Code =d.Dept_Code and co.Course_Id =d.Course_Id and c.subject_no=s.SubjectNo and sa.appl_no=sm.appl_no  and sa.appl_id=s.StaffApplNo and s.FeedBackMasterFK =f.FeedBackMasterPK  and   f.degreecode in('" + degreecode + "') and f.Batch_Year in('" + batchyear + "') and f.semester in ('" + sem + "')  and isnull(f.Section,'') in ('" + sec + "') and f.InclueCommon='1' and s.FeedbackUnicode<>'' and f.FeedBackMasterPK in ('" + feedbakpk + "') group by staff_code,staff_name,f.FeedBackMasterPK  ,StaffApplNo,Batch_Year ,f.semester,f.DegreeCode ,f.Section,subject_name,       SubjectNo,subject_code,Course_Name,dept_acronym,c.acronym order by sm.staff_name ";
                selqry += " SELECT distinct Question,QuestionMasterPK,HeaderCode FROM CO_FeedBackMaster B,CO_QuestionMaster Q ,CO_FeedBackQuestions FB WHERE  b.FeedBackMasterPK =fb.FeedBackMasterFK and q.QuestionMasterPK =fb.QuestionMasterFK and  InclueCommon='1' and FeedBackType = '" + type + "' and B.FeedBackName='" + Convert.ToString(ddl_Feedbackname.SelectedItem.Text) + "'  and B.CollegeCode in ('" + clgcode + "') order by HeaderCode";
                selqry += " SELECT StaffApplNo,sum(M.Point) as points,QuestionMasterfK,SubjectNo FROM CO_StudFeedBack F,CO_FeedBackMaster B,CO_MarkMaster M,CO_FeedbackUniCode fu WHERE fu.FeedbackUnicode=f.FeedbackUnicode and fu.FeedbackMasterFK=f.FeedBackMasterFK and F.FeedBackMasterFK = B.FeedBackMasterPK AND F.MarkMasterPK = M.MarkMasterPK AND  b.InclueCommon='1' and FeedBackType = '" + type + "' and B.FeedBackName ='" + Convert.ToString(ddl_Feedbackname.SelectedItem.Text) + "' and B.CollegeCode in ('" + clgcode + "')  and   b.degreecode in ('" + degreecode + "') and b.Batch_Year in('" + batchyear + "') and b.semester in ('" + sem + "')  and isnull(b.Section,'') in ('" + sec + "') group by StaffApplNo,QuestionMasterfK,SubjectNo";
                selqry += "  select count(App_No)studentcount,degree_code,sections,college_code from Registration where degree_code in('" + degreecode + "') and college_code in('" + clgcode + "') and isnull(Sections,'') in('" + sec + "') group by degree_code,college_code,sections ";
                selqry += " select COUNT( distinct QuestionMasterFK)question_count,isnull(SubjectType,'')SubjectType from CO_FeedBackQuestions where FeedBackMasterFK in ('" + feedbakpk + "') group by isnull(SubjectType,'')";
                ds = d2.select_method_wo_parameter(selqry, "Text");
                //string question_count = d2.GetFunction("select COUNT( distinct QuestionMasterFK)question_count from CO_FeedBackQuestions where FeedBackMasterFK in ('" + feedbakpk + "')");
                double question_count = 0;
                if (ds.Tables[4].Rows.Count > 0)
                {
                    double.TryParse(Convert.ToString(ds.Tables[4].Compute("sum(question_count)", "")), out question_count);
                }
                string sum_total = d2.GetFunction("select top 1 Point from CO_MarkMaster  where CollegeCode in('" + collegecode1 + "') order by Point desc");
                if (ds.Tables[1].Rows.Count > 0)
                {
                    for (int i = 0; i < ds.Tables[1].Rows.Count; i++)
                    {
                        FpSpread1.Sheets[0].ColumnCount++;
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Sheets[0].ColumnCount - 1].Text = Convert.ToString(ds.Tables[1].Rows[i]["Question"]);
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Sheets[0].ColumnCount - 1].Tag = Convert.ToString(ds.Tables[1].Rows[i]["QuestionMasterPK"]);
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Sheets[0].ColumnCount - 1].Font.Size = FontUnit.Medium;
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Sheets[0].ColumnCount - 1].Font.Name = "Book Antiqua";
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Sheets[0].ColumnCount - 1].Font.Bold = true;
                        FpSpread1.Columns[FpSpread1.Sheets[0].ColumnCount - 1].Width = 300;
                    }
                    FpSpread1.Sheets[0].ColumnCount++;
                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Sheets[0].ColumnCount - 1].Text = "Student Total";
                    FpSpread1.Sheets[0].ColumnCount++;
                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Sheets[0].ColumnCount - 1].Text = "Maximum Total";
                    FpSpread1.Sheets[0].ColumnCount++;
                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Sheets[0].ColumnCount - 1].Text = "Percentage";
                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Columns.Count - 1].Font.Size = FontUnit.Medium;
                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Columns.Count - 1].Font.Name = "Book Antiqua";
                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Columns.Count - 1].Font.Bold = true;
                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Columns.Count - 2].Font.Size = FontUnit.Medium;
                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Columns.Count - 2].Font.Name = "Book Antiqua";
                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Columns.Count - 2].Font.Bold = true;
                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Columns.Count - 3].Font.Size = FontUnit.Medium;
                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Columns.Count - 3].Font.Name = "Book Antiqua";
                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Columns.Count - 3].Font.Bold = true;
                }
                if (ds.Tables.Count > 0)
                {
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        int k = 0; string staffname = ""; int s = 1;
                        FarPoint.Web.Spread.CheckBoxCellType cb = new FarPoint.Web.Spread.CheckBoxCellType();
                        cb.AutoPostBack = true;
                        FarPoint.Web.Spread.CheckBoxCellType cb1 = new FarPoint.Web.Spread.CheckBoxCellType();
                        cb1.AutoPostBack = false;
                        FpSpread1.Sheets[0].RowCount++;
                        double staffavg = 0; bool staffinvdiavg = false; double sumavgpoint = 0; int staffrowcount = 0;
                        for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                        {
                            staffinvdiavg = false;
                            FpSpread1.Sheets[0].RowCount++;
                            if (staffname.Trim() == "")
                            { k++; }
                            else if (staffname == ds.Tables[0].Rows[i]["staff_name"].ToString())
                            { k++; staffrowcount++; }
                            else
                            {
                                k = 1; s++;
                                FpSpread1.Sheets[0].RowCount++;
                                //staffavg = (staffavg / Convert.ToDouble(staffrowcount+1));
                                staffavg = ((staffavg / (Convert.ToDouble(staffrowcount + 1) * 100)) * 100);
                                double.TryParse(Convert.ToString(Math.Round(staffavg, 2)), out sumavgpoint);
                                staffinvdiavg = true;
                            }
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Text = Convert.ToString(s);
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].Text = ds.Tables[0].Rows[i]["department"].ToString();
                            staffname = ds.Tables[0].Rows[i]["staff_name"].ToString();
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].VerticalAlign = FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].VerticalAlign = VerticalAlign.Middle;
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].Text = ds.Tables[0].Rows[i]["staff"].ToString();//k.ToString();
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].VerticalAlign = FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].VerticalAlign = VerticalAlign.Middle;
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 3].Text = ds.Tables[0].Rows[i]["subject_code"].ToString();
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 4].Text = ds.Tables[0].Rows[i]["Subject_Name"].ToString();
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 4].VerticalAlign = FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 3].VerticalAlign = VerticalAlign.Middle;
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 5].Text = ds.Tables[0].Rows[i]["acronym"].ToString();
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 5].VerticalAlign = FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 4].VerticalAlign = VerticalAlign.Middle;

                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Font.Size = FontUnit.Medium;
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Font.Name = "Book Antiqua";
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Font.Bold = true;
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].Font.Size = FontUnit.Medium;
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].Font.Name = "Book Antiqua";
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].Font.Bold = true;
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].Font.Size = FontUnit.Medium;
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].Font.Name = "Book Antiqua";
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].Font.Bold = true;
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 3].Font.Size = FontUnit.Medium;
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 3].Font.Name = "Book Antiqua";
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 3].Font.Bold = true;
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 4].Font.Size = FontUnit.Medium;
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 4].Font.Name = "Book Antiqua";
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 4].Font.Bold = true;
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 5].Font.Size = FontUnit.Medium;
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 5].Font.Name = "Book Antiqua";
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 5].Font.Bold = true;


                            double gtotal = 0; double mtotal = 0; double avgper = 0;
                            string filterquery = "";
                            string section = Convert.ToString(ds.Tables[0].Rows[i]["Section"]);
                            filterquery = "degree_code='" + Convert.ToString(ds.Tables[0].Rows[i]["DegreeCode"]) + "' ";
                            if (section.Trim() != "")
                            {
                                filterquery = filterquery + " and Sections='" + section + "'";
                            }
                            ds.Tables[3].DefaultView.RowFilter = "" + filterquery + "";
                            DataView dvnew = ds.Tables[3].DefaultView;
                            string totalstudnent = "";
                            if (dvnew.Count > 0)
                            {
                                totalstudnent = Convert.ToString(dvnew[0]["studentcount"]);
                            }
                            if (totalstudnent.Trim() == "")
                                totalstudnent = "0";
                            //double maximun = Convert.ToDouble(question_count) * Convert.ToDouble(sum_total) * Convert.ToDouble(totalstudnent);
                            Double attendstrength = Convert.ToDouble(ds.Tables[0].Rows[i]["Strength"]);
                            double maximun = Convert.ToDouble(sum_total) * Convert.ToDouble(attendstrength);
                            double QuestionAttendcount = 0;
                            for (int j = 7; j <= FpSpread1.Columns.Count - 3; j++)
                            {
                                string questionmasterPK = Convert.ToString(FpSpread1.Sheets[0].ColumnHeader.Cells[0, j - 1].Tag);
                                ds.Tables[2].DefaultView.RowFilter = " QuestionMasterfK ='" + questionmasterPK + "' and StaffApplNo='" + ds.Tables[0].Rows[i]["StaffApplNo"] + "'  and SubjectNo='" + Convert.ToString(ds.Tables[0].Rows[i]["SubjectNo"]) + "'";//and FeedbackUnicode='" + ds.Tables[0].Rows[i]["FeedbackUnicode"] + "'
                                DataView dv = ds.Tables[2].DefaultView;
                                if (dv.Count > 0)
                                {
                                    QuestionAttendcount++;
                                    string point1 = Convert.ToString(dv[0]["points"]);
                                    if (string.IsNullOrEmpty(point1.Trim()) || point1.Trim() == "-")
                                        point1 = "0";
                                    double questavgpoint = Convert.ToDouble(point1) / maximun * Convert.ToDouble(sum_total);// 100;
                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, j - 1].Text = Convert.ToString(Math.Round(questavgpoint, 2)); //Convert.ToString(dv[0]["points"]);
                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, j - 1].HorizontalAlign = HorizontalAlign.Center;
                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, j - 1].Font.Size = FontUnit.Medium;
                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, j - 1].Font.Name = "Book Antiqua";
                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, j - 1].Font.Bold = true;
                                    gtotal += Convert.ToDouble((Math.Round(questavgpoint, 2)));
                                }
                                else
                                {
                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, j - 1].Text = "-";
                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, j - 1].HorizontalAlign = HorizontalAlign.Center;
                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, j - 1].Font.Bold = true;
                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, j - 1].Font.Size = FontUnit.Medium;
                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, j - 1].Font.Name = "Book Antiqua";
                                }
                                FpSpread1.Columns[j - 1].Locked = true;
                                FpSpread1.Columns[4].Locked = true;
                            }
                            //Double strength = Convert.ToDouble(ds.Tables[0].Rows[i]["Strength"]);
                            //double calfbcal = Convert.ToDouble(totalstudnent) * Convert.ToDouble(question_count) * Convert.ToDouble(sum_total);
                            //double fbavg = (gtotal / calfbcal) * 100;
                            //double avg = Convert.ToDouble(Math.Round(fbavg, 2));
                            //string studentcount = "";
                            //if (Convert.ToString(ds.Tables[0].Rows[i]["Strength"]).Trim() != "")
                            //{
                            //    studentcount = Convert.ToString(ds.Tables[0].Rows[i]["Strength"]);
                            //}
                            //else
                            //{
                            //    studentcount = "-";
                            //}
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, FpSpread1.Columns.Count - 3].CellType = new FarPoint.Web.Spread.TextCellType();
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, FpSpread1.Columns.Count - 3].Text = Convert.ToString(String.Format("{0:0.00}", Math.Round(gtotal, 2)));
                            if (issubjecttype == "1" || issubjecttype.ToUpper() == "TRUE")
                            {
                                question_count = QuestionAttendcount;
                            }
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, FpSpread1.Columns.Count - 2].Text = Convert.ToString(Math.Round((question_count * Convert.ToDouble(sum_total)), 2));
                            double avg = (Math.Round(Math.Round(gtotal, 2) / Math.Round((question_count * Convert.ToDouble(sum_total)), 2) * 100, 2));
                            //barath 31.07.17 *100 added
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, FpSpread1.Columns.Count - 1].CellType = new FarPoint.Web.Spread.TextCellType();
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, FpSpread1.Columns.Count - 1].Text = String.Format("{0:0.00}", Math.Round(avg, 2));
                            staffavg += avg;
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, FpSpread1.Columns.Count - 1].Font.Bold = true;
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, FpSpread1.Columns.Count - 1].Font.Size = FontUnit.Medium;
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, FpSpread1.Columns.Count - 1].Font.Name = "Book Antiqua";
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, FpSpread1.Columns.Count - 3].HorizontalAlign = HorizontalAlign.Right;
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, FpSpread1.Columns.Count - 2].HorizontalAlign = HorizontalAlign.Right;
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, FpSpread1.Columns.Count - 1].HorizontalAlign = HorizontalAlign.Right;
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, FpSpread1.Columns.Count - 1].Font.Name = "Book Antiqua";
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, FpSpread1.Columns.Count - 1].Font.Bold = true;
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, FpSpread1.Columns.Count - 2].Font.Size = FontUnit.Medium;
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, FpSpread1.Columns.Count - 2].Font.Name = "Book Antiqua";
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, FpSpread1.Columns.Count - 2].Font.Bold = true;
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, FpSpread1.Columns.Count - 3].Font.Size = FontUnit.Medium;
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, FpSpread1.Columns.Count - 3].Font.Name = "Book Antiqua";
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, FpSpread1.Columns.Count - 3].Font.Bold = true;
                            if (staffinvdiavg == true)
                            {
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 2, FpSpread1.Columns.Count - 1].CellType = new FarPoint.Web.Spread.TextCellType();
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 2, FpSpread1.Columns.Count - 1].Text = String.Format("{0:0.00}", Math.Round(sumavgpoint, 2));

                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 2, FpSpread1.Columns.Count - 1].ForeColor = System.Drawing.Color.BlueViolet;
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 2, FpSpread1.Columns.Count - 1].Font.Bold = true;
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 2, FpSpread1.Columns.Count - 1].Font.Size = FontUnit.Medium;
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 2, FpSpread1.Columns.Count - 1].Font.Name = "Book Antiqua";
                                sumavgpoint = 0;
                                staffrowcount = 0; staffavg = 0; staffavg += avg;
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 2, FpSpread1.Columns.Count - 1].Font.Size = FontUnit.Medium;
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 2, FpSpread1.Columns.Count - 1].Font.Name = "Book Antiqua";
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 2, FpSpread1.Columns.Count - 1].Font.Bold = true;
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 2, FpSpread1.Columns.Count - 1].HorizontalAlign = HorizontalAlign.Right;
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 2, FpSpread1.Columns.Count - 2].Text = "Average";
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 2, FpSpread1.Columns.Count - 2].ForeColor = System.Drawing.Color.BlueViolet;
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 2, FpSpread1.Columns.Count - 2].Font.Size = FontUnit.Medium;
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 2, FpSpread1.Columns.Count - 2].Font.Name = "Book Antiqua";
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 2, FpSpread1.Columns.Count - 2].Font.Bold = true;
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 2, FpSpread1.Columns.Count - 2].HorizontalAlign = HorizontalAlign.Right;
                            }
                        }
                        FpSpread1.Sheets[0].RowCount++;
                        staffavg = ((staffavg / (Convert.ToDouble(staffrowcount + 1) * 100)) * 100);
                        double.TryParse(Convert.ToString(Math.Round(staffavg, 2)), out sumavgpoint);
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, FpSpread1.Columns.Count - 1].CellType = new FarPoint.Web.Spread.TextCellType();
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, FpSpread1.Columns.Count - 1].Text = Convert.ToString(String.Format("{0:0.00}", sumavgpoint));
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, FpSpread1.Columns.Count - 1].Font.Bold = true;
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, FpSpread1.Columns.Count - 1].Font.Size = FontUnit.Medium;
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, FpSpread1.Columns.Count - 1].Font.Name = "Book Antiqua";
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, FpSpread1.Columns.Count - 1].ForeColor = System.Drawing.Color.BlueViolet;
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, FpSpread1.Columns.Count - 3].HorizontalAlign = HorizontalAlign.Right;
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, FpSpread1.Columns.Count - 2].HorizontalAlign = HorizontalAlign.Right;
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, FpSpread1.Columns.Count - 1].HorizontalAlign = HorizontalAlign.Right;
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, FpSpread1.Columns.Count - 1].Font.Size = FontUnit.Medium;
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, FpSpread1.Columns.Count - 1].Font.Name = "Book Antiqua";
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, FpSpread1.Columns.Count - 1].Font.Bold = true;
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, FpSpread1.Columns.Count - 2].Font.Size = FontUnit.Medium;
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, FpSpread1.Columns.Count - 2].Font.Name = "Book Antiqua";
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, FpSpread1.Columns.Count - 2].Font.Bold = true;
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, FpSpread1.Columns.Count - 3].Font.Size = FontUnit.Medium;
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, FpSpread1.Columns.Count - 3].Font.Name = "Book Antiqua";
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, FpSpread1.Columns.Count - 3].Font.Bold = true;
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, FpSpread1.Columns.Count - 2].Text = "Average";
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, FpSpread1.Columns.Count - 2].ForeColor = System.Drawing.Color.BlueViolet;
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, FpSpread1.Columns.Count - 2].Font.Size = FontUnit.Medium;
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, FpSpread1.Columns.Count - 2].Font.Name = "Book Antiqua";
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, FpSpread1.Columns.Count - 2].Font.Bold = true;
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, FpSpread1.Columns.Count - 2].HorizontalAlign = HorizontalAlign.Right;
                        FpSpread1.Columns[FpSpread1.Columns.Count - 1].Locked = true;
                        FpSpread1.Columns[FpSpread1.Columns.Count - 2].Locked = true;
                        FpSpread1.Columns[FpSpread1.Columns.Count - 3].Locked = true;
                        FpSpread1.Columns[0].Locked = true;
                        FpSpread1.Columns[1].Locked = true;
                        FpSpread1.Columns[2].Locked = true;
                        FpSpread1.Columns[3].Locked = true;
                        FpSpread1.Columns[4].Locked = true;
                        FpSpread1.Columns[5].Locked = true;
                        FpSpread1.Sheets[0].Columns[5].HorizontalAlign = HorizontalAlign.Left;
                        FpSpread1.Sheets[0].Columns[3].HorizontalAlign = HorizontalAlign.Left;
                        FpSpread1.Sheets[0].Columns[4].HorizontalAlign = HorizontalAlign.Left;
                        FpSpread1.Sheets[0].Columns[6].HorizontalAlign = HorizontalAlign.Center;
                        FpSpread1.Sheets[0].Columns[7].HorizontalAlign = HorizontalAlign.Center;
                        FpSpread1.Sheets[0].Columns[8].HorizontalAlign = HorizontalAlign.Center;
                        FpSpread1.Sheets[0].Columns[8].VerticalAlign = VerticalAlign.Middle;
                        FpSpread1.Sheets[0].Columns[2].HorizontalAlign = HorizontalAlign.Left;
                        FpSpread1.Sheets[0].Columns[2].VerticalAlign = VerticalAlign.Middle;
                        FpSpread1.Sheets[0].Columns[0].HorizontalAlign = HorizontalAlign.Center;
                        FpSpread1.Sheets[0].Columns[0].VerticalAlign = VerticalAlign.Middle;
                        FpSpread1.Sheets[0].SetColumnMerge(0, FarPoint.Web.Spread.Model.MergePolicy.Always);
                        FpSpread1.Height = 500;
                        FpSpread1.Sheets[0].PageSize = FpSpread1.Sheets[0].RowCount;
                        rptprint1.Visible = true;
                        FpSpread1.Visible = true;
                    }
                    else
                    {
                        imgdiv2.Visible = true;
                        lbl_alert1.Text = "No Records Found";
                        FpSpread1.Visible = false;
                        rptprint1.Visible = false;
                    }
                }
                else
                {
                    imgdiv2.Visible = true;
                    lbl_alert1.Text = "No Records Found";
                    FpSpread1.Visible = false;
                    rptprint1.Visible = false;
                }
            }
            else
            {
                FpSpread1.Visible = false;
                //reportdivquestiondet.Visible = false;
                imgdiv2.Visible = true;
                lbl_alert1.Text = "Please Select Feedback Name";
            }
        }
        catch
        {
        }
    }

    protected string returndswithsinglecodevalue(string query)
    {
        string empty = "";
        if (query.Trim() != "")
        {
            DataSet dummy = new DataSet();
            dummy = d2.select_method_wo_parameter(query, "text");
            if (dummy.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow dr in dummy.Tables[0].Rows)
                {
                    if (empty == "")
                    {
                        empty = Convert.ToString(dr[0]);
                    }
                    else
                    {
                        empty = empty + "','" + Convert.ToString(dr[0]);
                    }
                }
            }
        }
        return empty;
    }

    public void ddl_SelectLogin_Changed(object sender, EventArgs e)//delsi
    {
        ind_login.Visible = false;
        rb_farmate1.Checked = false;
        rb_farmate2.Checked = false;
        rb_farmate3.Checked = false;
        rb_farmate4.Checked = false;
        rb_farmate5.Checked = false;
        rb_farmate6.Checked = false;
        rb_farmate7.Checked = false;

        string FormateType = Convert.ToString(ddl_Loginbasec.SelectedItem.Value);
        int Value = 0;
        int.TryParse(Convert.ToString(FormateType), out Value);
        switch (Value)
        {
            case 1:
                rb_farmate1.Checked = true;
                rb_farmate1_CheckedChanged(sender, e);
                break;
            case 2:
                rb_farmate2.Checked = true;
                rb_farmate2_CheckedChanged(sender, e);
                break;
            case 3:
                rb_farmate3.Checked = true;
                rb_farmate3_CheckedChanged(sender, e);
                break;
            case 4:
                rb_farmate4.Checked = true;
                rb_farmate4_CheckedChanged(sender, e);
                break;
            case 5:
                rb_farmate5.Checked = true;
                rb_farmate5_CheckedChanged(sender, e);
                break;
            case 6:
                rb_farmate6.Checked = true;
                rb_farmate6_CheckedChanged(sender, e);
                break;
            case 7:
                rb_farmate7.Checked = true;
                rb_farmate7_CheckedChanged(sender, e);
                break;

        }
    }

    public void ddl_SelectAnontomous_Changed(object sender, EventArgs e)//delsi
    {
        acd_anonyms.Visible = false;
        rb_anonyms_farmate1.Checked = false;
        rb_anonyms_farmate2.Checked = false;
        rb_anonyms_farmate3.Checked = false;
        rb_anonyms_farmate4.Checked = false;
        rb_anonyms_farmate5.Checked = false;
        rb_anonyms_farmate6.Checked = false;
        rb_anonyms_farmate7.Checked = false;

        string FormateType = Convert.ToString(ddl_Anonyomous.SelectedItem.Value);
        int Value = 0;
        int.TryParse(Convert.ToString(FormateType), out Value);
        switch (Value)
        {
            case 1:
                rb_anonyms_farmate1.Checked = true;
                rb_anonyms_farmate1_CheckedChanged(sender, e);

                break;
            case 2:
                rb_anonyms_farmate2.Checked = true;
                rb_anonyms_farmate2_CheckedChanged(sender, e);

                break;
            case 3:
                rb_anonyms_farmate3.Checked = true;
                rb_anonyms_farmate3_CheckedChanged(sender, e);

                break;
            case 4:
                rb_anonyms_farmate4.Checked = true;
                rb_anonyms_farmate4_CheckedChanged(sender, e);

                break;
            case 5:
                rb_anonyms_farmate5.Checked = true;
                rb_anonyms_farmate5_CheckedChanged(sender, e);

                break;
            case 6:
                rb_anonyms_farmate6.Checked = true;
                LblSec.Visible = false;
                UpSec.Visible = false;
                rb_anonyms_farmate6_CheckedChanged(sender, e);

                break;
            case 7:
                rb_anonyms_farmate7.Checked = true;
                rb_anonyms_farmate7_CheckedChanged(sender, e);

                break;
        }
    }

    protected void Ananames6barChart()
    {
        try
        {
            FpSpread1.Visible = false;
            FpSpread2.Visible = false;
            FpSpread3.Visible = false;
            question_chart.Visible = false;
            chartprint.Visible = false;
            chart_staff_chart.Visible = false;
            staff_chart.Visible = false;
            string CollegeCode = rs.GetSelectedItemsValue(cbl_clgnameformat6);
            string DeptCode = Convert.ToString(ddlformate6_deptname.SelectedItem.Value);
            string StaffCode = rs.GetSelectedItemsValue(cbl_staffnameformat6);
            string BatchYear = rs.GetSelectedItemsValue(cbl_formate6batch);
            string Sem = rs.GetSelectedItemsValue(cbl_formate6sem);
            string section = rs.GetSelectedItemsValue(cbl_secDeptWise);
            string subjectcode = rs.GetSelectedItemsValue(cbl_subjectnameformat6);
            if (ddl_feedbackformate6.SelectedItem.Value == "Select")
            {
                imgdiv2.Visible = true;
                lbl_alert1.Text = "Please Select Feedback Name";
                return;
            }
            if (!string.IsNullOrEmpty(CollegeCode) && !string.IsNullOrEmpty(DeptCode) && !string.IsNullOrEmpty(StaffCode) && !string.IsNullOrEmpty(BatchYear) && !string.IsNullOrEmpty(Sem))
            {
                ////string selqry = "  select Staff_Name,sf.subjectno,(Point)as Points,Question,t.staff_code from CO_FeedbackUniCode FU,CO_FeedBackMaster FM,CO_StudFeedBack SF,staff_appl_master A,staffmaster T,CO_MarkMaster M, CO_QuestionMaster Q where Q.QuestionMasterPK =sf.QuestionMasterFK and M.MarkMasterPK =sf.MarkMasterPK and A.appl_no =t.appl_no and a.appl_id =sf.StaffApplNo and fm.FeedBackMasterPK =fu.FeedbackMasterFK and sf.FeedBackMasterFK =fu.FeedbackMasterFK and sf.FeedBackMasterFK =fm.FeedBackMasterPK and sf.FeedbackUnicode =fu.FeedbackUnicode and fu.FeedbackMasterFK in ('" + FeedBackPk + "') and FM.CollegeCode in ('" + CollegeCode + "') and FM.Batch_Year in ('" + BatchYear + "') and FM.semester in ('" + Sem + "')  and fm.FeedBackMasterPK in ('" + FeedBackPk + "') and SF.StaffApplNo in ('" + StaffCode + "') ";
                //string selqry = "  select Staff_Name,(Point)as Points,Question,t.staff_code from CO_FeedBackMaster FM,CO_StudFeedBack SF,staff_appl_master A,staffmaster T,CO_MarkMaster M, CO_QuestionMaster Q where Q.QuestionMasterPK =sf.QuestionMasterFK and M.MarkMasterPK =sf.MarkMasterPK and A.appl_no =t.appl_no and a.appl_id =sf.StaffApplNo   and sf.FeedBackMasterFK =fm.FeedBackMasterPK and FM.CollegeCode in ('" + CollegeCode + "') and FM.Batch_Year in ('" + BatchYear + "') and FM.semester in ('" + Sem + "')  and fm.FeedBackMasterPK in ('" + FeedBackPk + "') and SF.StaffApplNo in ('" + StaffCode + "') ";
                //selqry += " SELECT distinct Question,QuestionMasterPK FROM CO_FeedBackMaster B,CO_QuestionMaster Q ,CO_FeedBackQuestions FB WHERE  b.FeedBackMasterPK =fb.FeedBackMasterFK and q.QuestionMasterPK =fb.QuestionMasterFK and  InclueCommon='1' and FeedBackType = '1' and B.FeedBackName='" + Convert.ToString(ddl_feedbackformate6.SelectedItem.Value) + "'  and B.CollegeCode in ('" + CollegeCode + "') order by Question";
                Hashtable hat = new Hashtable();
                //Commented by saranya on 11/9/2018
                //hat.Add("@CollegeCode", CollegeCode);
                //hat.Add("@batchyear", BatchYear);
                //hat.Add("@semester", Sem);
                //hat.Add("@FeedbackName", Convert.ToString(ddl_feedbackformate6.SelectedItem.Text));
                //hat.Add("@StaffAppNo", StaffCode);
                //hat.Add("@section", section);
                //hat.Add("@subjectno", subjectcode);
                //ds = d2.select_method("DepartmentStaffQuestionsWise", hat, "sp");

                //Added by saranya 11/09/2018
                string type = "1";
                string fbpk = " select FeedBackMasterPK,ISNULL(issubjecttype,0)issubjecttype from CO_FeedBackMaster where FeedBackName ='" + ddl_feedbackformate6.SelectedItem.Value + "'";
                DataSet dsfb = d2.select_method_wo_parameter(fbpk, "Text");
                string feedbakpk = string.Empty;
                string issubjecttype = string.Empty;
                if (dsfb.Tables.Count > 0)
                {
                    if (dsfb.Tables[0].Rows.Count > 0)
                    {
                        issubjecttype = Convert.ToString(dsfb.Tables[0].Rows[0]["issubjecttype"]);
                        for (int pk = 0; pk < dsfb.Tables[0].Rows.Count; pk++)
                        {
                            if (string.IsNullOrEmpty(feedbakpk))
                                feedbakpk = dsfb.Tables[0].Rows[pk]["FeedBackMasterPK"].ToString();
                            else
                                feedbakpk = feedbakpk + "," + dsfb.Tables[0].Rows[pk]["FeedBackMasterPK"].ToString() + "";
                        }
                    }
                }
                DataSet dsDegreeCode = new DataSet();
                StringBuilder sbDegreeCode = new StringBuilder();
                string DegCode = "";
                string Appl_id = rs.GetSelectedItemsValueAsString(cbl_staffnameformat6);
                string Query = "select distinct sm.degree_code from Registration r,staff_selector ss,subject s,syllabus_master sm,staff_appl_master sa,staffmaster smm where smm.appl_no=sa.appl_no and  sm.syll_code=s.syll_code and  s.subject_no=ss.subject_no and r.Batch_Year=sm.Batch_Year and r.degree_code=sm.degree_code and r.Current_Semester=sm.semester and r.CC='0' and r.DelFlag<>1 and Exam_Flag='OK' and isRedo<>1 and r.Batch_Year in(" + BatchYear + ") and ss.Sections in('" + section + "','')  and sm.semester in(" + Sem + ") and sa.appl_id in('" + Appl_id + "') and smm.staff_code=ss.staff_code";
                dsDegreeCode.Clear();
                dsDegreeCode = d2.select_method_wo_parameter(Query, "text");
                for (int deg = 0; deg < dsDegreeCode.Tables[0].Rows.Count; deg++)
                {
                    string code = Convert.ToString(dsDegreeCode.Tables[0].Rows[deg]["degree_code"]);
                    sbDegreeCode.Append(code).Append(",");
                }
                DegCode = Convert.ToString(sbDegreeCode);
                DegCode = DegCode.TrimEnd(',');

                hat.Add("@CollegeCode", CollegeCode);
                hat.Add("@batchyear", BatchYear);
                hat.Add("@Degreecode", DegCode);
                hat.Add("@semester", Sem);
                hat.Add("@section", section);
                hat.Add("@FeedbackName", Convert.ToString(ddl_feedbackformate6.SelectedItem.Text));
                hat.Add("@FeedbackMasterFK", feedbakpk);
                hat.Add("@StaffAppNo", StaffCode);
                hat.Add("@FeedbackType", type);
                hat.Add("@subjectno", subjectcode);
                ds = d2.select_method("[DepartmentStaffQuestionsWise]", hat, "sp");
                //=====================================================//
                string CollegeName = d2.GetFunction(" select collname from collinfo where college_code in ('" + CollegeCode + "')");
                DataTable dtChart2 = new DataTable();
                DataColumn dc;
                DataRow drpq;
                if (ds.Tables[0].Rows.Count > 0)
                {
                    staff_chart.Series.Clear();
                    string subcode = string.Empty;
                    string staffandcode = string.Empty;
                    string staffnam = string.Empty;
                    ArrayList stafArr = new ArrayList();
                    ArrayList staffArrayList = new ArrayList();
                    string question = string.Empty;
                    question_chart.Series.Clear();
                    for (int i = 0; i < ds.Tables[1].Rows.Count; i++)
                    {
                        question = Convert.ToString(ds.Tables[1].Rows[i]["Question"]);
                        question_chart.Series.Add(question.Trim());
                    }
                    string departmentname = Convert.ToString(ddlformate6_deptname.SelectedItem.Text);
                    //    question_chart.Titles[0].Text = ("Questions Wise Chart (" + Convert.ToString(ddl_feedbackformate6.SelectedItem.Value) + ")"); Department Wise Feedback
                    question_chart.Titles[0].Text = ('\n' + CollegeName + '\n' + '\n' + "Department Wise Feedback" + '\n' + '\n' + "Department Name" + "" + ":" + departmentname);
                    //Commented by Saranya on12/9/2018
                    // question_chart.Titles[0].Text = "Department Name" + "" + ":" + departmentname;
                    //for (int r = 0; r < ds.Tables[0].Rows.Count; r++)
                    //{
                    //    staffnam = ds.Tables[0].Rows[r]["Staff_Name"].ToString();
                    //    subcode = ds.Tables[0].Rows[r]["staff_code"].ToString();
                    //    staffandcode = staffnam; // modified by saranya on 21/8/2018
                    //    if (!stafArr.Contains(staffandcode))
                    //    {
                    //        DataView dv = new DataView();
                    //        dv = ds.Tables[0].DefaultView;
                    //        dc = new DataColumn();
                    //        stafArr.Add(staffandcode);
                    //        dc.ColumnName = staffandcode;
                    //        dtChart2.Columns.Add(dc);
                    //    }
                    //}
                    //for (int i = 0; i < cbl_staffnameformat6.Items.Count; i++)
                    //{
                    //    if (cbl_staffnameformat6.Items[i].Selected)
                    //    {
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        //string applId = Convert.ToString(cbl_staffnameformat6.Items[i].Value);
                        //string staff_name = Convert.ToString(cbl_staffnameformat6.Items[i].Text);
                        string staff = Convert.ToString(ds.Tables[0].Rows[i]["staff"]);
                        string[] staffSp = staff.Split('-');
                        string staffName = Convert.ToString(staffSp[1]);
                        string applId = Convert.ToString(ds.Tables[0].Rows[i]["StaffApplNo"]);

                        if (!staffArrayList.Contains(staff))
                        {
                            staffArrayList.Add(staff);
                            ds.Tables[0].DefaultView.RowFilter = "StaffApplNo ='" + applId + "'";
                            DataTable dvStaffApplNo = ds.Tables[0].DefaultView.ToTable();
                            if (dvStaffApplNo.Rows.Count > 0)
                            {
                                for (int count = 0; count < dvStaffApplNo.Rows.Count; count++)
                                {
                                    staffnam = dvStaffApplNo.Rows[count]["Staff"].ToString();
                                    string[] staffCode = staffnam.Split('-');
                                    staffandcode = staffCode[1];
                                    if (!stafArr.Contains(staffandcode))
                                    {
                                        dc = new DataColumn();
                                        stafArr.Add(staffandcode);
                                        dc.ColumnName = staffandcode;
                                        dtChart2.Columns.Add(dc);
                                    }
                                }
                            }
                        }
                    }

                    string sum_total = d2.GetFunction("select top 1 Point from CO_MarkMaster  where CollegeCode in('" + collegecode1 + "') order by Point desc");
                    staffArrayList.Clear();
                    for (int j = 2; j < Fpspread6.Columns.Count; j++)
                    {
                        drpq = dtChart2.NewRow();
                        string questionmasterPK = Convert.ToString(Fpspread6.Sheets[0].ColumnHeader.Cells[0, j].Tag);
                        //for (int i = 0; i < cbl_staffnameformat6.Items.Count; i++)
                        //{
                        //    if (cbl_staffnameformat6.Items[i].Selected)
                        //    {
                        for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                        {
                            //string applId = Convert.ToString(cbl_staffnameformat6.Items[i].Value);
                            //string staff_name = Convert.ToString(cbl_staffnameformat6.Items[i].Text);
                            string staff = Convert.ToString(ds.Tables[0].Rows[i]["staff"]);
                            string[] staffSp = staff.Split('-');
                            string staffName = Convert.ToString(staffSp[1]);
                            string applId = Convert.ToString(ds.Tables[0].Rows[i]["StaffApplNo"]);

                            if (!staffArrayList.Contains(staff))
                            {
                                ds.Tables[0].DefaultView.RowFilter = "StaffApplNo ='" + applId + "'";
                                DataTable dvStaffApplNo = ds.Tables[0].DefaultView.ToTable();

                                if (dvStaffApplNo.Rows.Count > 0)
                                {
                                    double finalValue = 0;
                                    double TotalCount = 0;
                                    for (int count = 0; count < dvStaffApplNo.Rows.Count; count++)
                                    {
                                        staffnam = dvStaffApplNo.Rows[count]["Staff"].ToString();
                                        string[] staffCode = staffnam.Split('-');
                                        staffandcode = staffCode[1];

                                        double questavgpoint = 0;
                                        string filterquery = string.Empty;
                                        string sections = Convert.ToString(dvStaffApplNo.Rows[count]["Section"]);
                                        filterquery = "degree_code='" + Convert.ToString(dvStaffApplNo.Rows[count]["DegreeCode"]) + "' ";
                                        if (section.Trim() != "")
                                        {
                                            filterquery = filterquery + " and Sections='" + sections + "'";
                                        }
                                        ds.Tables[3].DefaultView.RowFilter = "" + filterquery + "";
                                        DataView dvnew = ds.Tables[3].DefaultView;
                                        string totalstudnent = "";
                                        if (dvnew.Count > 0)
                                        {
                                            totalstudnent = Convert.ToString(dvnew[0]["studentcount"]);
                                        }
                                        if (totalstudnent.Trim() == "")
                                            totalstudnent = "0";

                                        Double attendstrength = Convert.ToDouble(dvStaffApplNo.Rows[count]["Strength"]);
                                        string feedbackf = Convert.ToString(dvStaffApplNo.Rows[count]["FeedBackMasterPK"]);
                                        double maximun = Convert.ToDouble(sum_total) * Convert.ToDouble(attendstrength);
                                        ds.Tables[2].DefaultView.RowFilter = " QuestionMasterfK ='" + questionmasterPK + "' and StaffApplNo='" + applId + "'  and SubjectNo='" + Convert.ToString(dvStaffApplNo.Rows[count]["SubjectNo"]) + "' and Section='" + Convert.ToString(dvStaffApplNo.Rows[count]["Section"]) + "' and FeedBackMasterFK='" + Convert.ToString(dvStaffApplNo.Rows[count]["FeedBackMasterPK"]) + "'";
                                        DataView dv = ds.Tables[2].DefaultView;
                                        if (dv.Count > 0)
                                        {
                                            TotalCount++;
                                            string point1 = Convert.ToString(dv[0]["points"]);
                                            if (string.IsNullOrEmpty(point1.Trim()) || point1.Trim() == "-")
                                                point1 = "0";
                                            questavgpoint = Convert.ToDouble(point1) / maximun * Convert.ToDouble(sum_total);// 100;
                                            questavgpoint = Math.Round(questavgpoint, 0, MidpointRounding.AwayFromZero);
                                        }
                                        finalValue = finalValue + questavgpoint;
                                    }
                                    finalValue = finalValue / TotalCount;
                                    finalValue = Math.Round(finalValue, 0, MidpointRounding.AwayFromZero);
                                    drpq[staffandcode] = Convert.ToString(finalValue);
                                }
                            }
                        }
                        dtChart2.Rows.Add(drpq);
                    }
                    #region commented by saranya on 11/9/2018
                    //DataRow drpq;
                    //for (int i = 0; i < ds.Tables[1].Rows.Count; i++)
                    //{
                    //    DataTable dtstp = new DataTable();
                    //    question = Convert.ToString(ds.Tables[1].Rows[i]["Question"]);
                    //    ds.Tables[0].DefaultView.RowFilter = "Question='" + question + "'";
                    //    dtstp = ds.Tables[0].DefaultView.ToTable();
                    //    drpq = dtChart2.NewRow();
                    //    ArrayList ques = new ArrayList();
                    //    subcode = string.Empty;
                    //    staffandcode = string.Empty;
                    //    staffnam = string.Empty;
                    //    for (int st = 0; st < dtstp.Rows.Count; st++)
                    //    {
                    //        staffnam = dtstp.Rows[st]["Staff_Name"].ToString();
                    //        subcode = dtstp.Rows[st]["staff_code"].ToString();
                    //        staffandcode = staffnam; //+ "-" + subcode modified by saranya on 21/8/2018
                    //        if (!ques.Contains(staffandcode))
                    //        {
                    //            ques.Add(staffandcode);
                    //            dtstp.DefaultView.RowFilter = "Staff_Name='" + staffnam + "' and staff_code='" + subcode + "' ";
                    //            DataView dv = new DataView();
                    //            dv = dtstp.DefaultView;
                    //            string points = string.Empty;
                    //            Double poin = 0;
                    //            Double ps = 0;
                    //            int w = 0;
                    //            for (int c = 0; c < dv.Count; c++)
                    //            {
                    //                w++;
                    //                points = Convert.ToString(dv[c]["Points"]);
                    //                string sum_total = d2.GetFunction("select top 1 Point from CO_MarkMaster  where CollegeCode in('" + collegecode1 + "') order by Point desc");
                    //                string needs = sum_total;
                    //                ConvertedMark(needs, sum_total, ref points);
                    //                double.TryParse(Convert.ToString(points), out poin);
                    //                ps += poin;
                    //            }
                    //            ps = ps / w;//delsiref
                    //            ps = Math.Round(ps, 0, MidpointRounding.AwayFromZero);
                    //            //   drpq[staffandcode] = Convert.ToString(Math.Round(ps, 1));
                    //            drpq[staffandcode] = Convert.ToString(ps);
                    //        }
                    //    }
                    //    dtChart2.Rows.Add(drpq);
                    //}
                    #endregion

                    if (dtChart2.Rows.Count > 0)
                    {
                        string srr = ""; Hashtable removedthash = new Hashtable(); string val = ""; double finalvalue = 0;
                        for (int cc = 0; cc < dtChart2.Columns.Count; cc++)
                        {
                            finalvalue = 0;
                            for (int rr = 0; rr < dtChart2.Rows.Count; rr++)
                            {
                                srr = dtChart2.Columns[cc].ToString();
                                val = dtChart2.Rows[rr][cc].ToString();
                                if (val.Trim() == "" || val.Trim().ToUpper() == "NAN")
                                    val = "0";
                                finalvalue = finalvalue + Convert.ToDouble(val);
                            }
                            if (finalvalue == 0)
                                removedthash.Add(cc, srr);
                        }
                        if (removedthash.Count > 0)
                        {
                            foreach (DictionaryEntry col in removedthash)
                                dtChart2.Columns.Remove(Convert.ToString(col.Value));
                        }
                        question_chart.RenderType = RenderType.ImageTag;
                        question_chart.ImageType = ChartImageType.Png;
                        question_chart.ImageStorageMode = ImageStorageMode.UseImageLocation;
                        question_chart.ImageLocation = Path.Combine("~/college/", "feedbackchart");
                        question_chart.ChartAreas[0].AxisY.Interval = 10;
                        if (dtChart2.Columns.Count > 0)
                        {
                            int chartwidth = 0;
                            for (int r = 0; r < dtChart2.Rows.Count; r++)
                            {
                                for (int c = 0; c < dtChart2.Columns.Count; c++)
                                {
                                    string sr = dtChart2.Columns[c].ToString();
                                    question_chart.Series[r].Points.AddXY(sr.ToString().Trim(), dtChart2.Rows[r][c].ToString().Trim());
                                    question_chart.ChartAreas[0].AxisX.LineColor = System.Drawing.Color.Black;
                                    question_chart.ChartAreas[0].AxisY.LineColor = System.Drawing.Color.Black;
                                    question_chart.Series[r].IsValueShownAsLabel = false;
                                    question_chart.Series[r].IsXValueIndexed = true;

                                    question_chart.Series[r].ChartType = SeriesChartType.Column;
                                    question_chart.ChartAreas[0].AxisX.LabelStyle.Font = new System.Drawing.Font("Verdana", 9f, System.Drawing.FontStyle.Bold);
                                    question_chart.ChartAreas[0].AxisX.LabelStyle.Angle = -50;//delsi0603 before it was 90
                                    question_chart.ChartAreas[0].AxisX.LabelStyle.Interval = 1;
                                    chartwidth += 15;
                                }
                            }
                            question_chart.Visible = true;
                            chartprint.Visible = true;
                            chartwidth = (chartwidth <= 500) ? 600 : chartwidth;
                            if (chartwidth <= 1000)
                                question_chart.Width = chartwidth;
                            else
                                question_chart.Width = 1000;
                            question_chart.Height = 1000;

                        }
                        else
                        {
                            chartprint.Visible = false;
                            imgdiv2.Visible = true;
                            lbl_alert1.Text = "No Records Found";
                        }
                    }
                }
                else
                {
                    imgdiv2.Visible = true;
                    lbl_alert1.Text = "No Records Found";
                }
            }
            else
            {
                imgdiv2.Visible = true;
                lbl_alert1.Text = "Please Select All Fields";
            }
        }
        catch (Exception ex)
        {
            //d2.sendErrorMail(ex, collegecode1, "FeedbackReport");
        }
    }

    protected void Ananames6barChartspread()//rajasekar
    {
        try
        {
            //Fpspread6.Visible = false;
            //FpSpread2.Visible = false;
            //FpSpread3.Visible = false;
            //question_chart.Visible = false;
            //chartprint.Visible = false;
            //chart_staff_chart.Visible = false;
            //staff_chart.Visible = false;
            rptprint1.Visible = false;
            Fpspread6.Visible = false;
            string CollegeCode = rs.GetSelectedItemsValue(cbl_clgnameformat6);
            string DeptCode = Convert.ToString(ddlformate6_deptname.SelectedItem.Value);
            string StaffCode = rs.GetSelectedItemsValue(cbl_staffnameformat6);
            string BatchYear = rs.GetSelectedItemsValue(cbl_formate6batch);
            string Sem = rs.GetSelectedItemsValue(cbl_formate6sem);
            string section = rs.GetSelectedItemsValue(cbl_secDeptWise);
            string subjectcode = rs.GetSelectedItemsValue(cbl_subjectnameformat6);
            if (ddl_feedbackformate6.SelectedItem.Value == "Select")
            {
                imgdiv2.Visible = true;
                lbl_alert1.Text = "Please Select Feedback Name";
                return;
            }
            if (!string.IsNullOrEmpty(CollegeCode) && !string.IsNullOrEmpty(DeptCode) && !string.IsNullOrEmpty(StaffCode) && !string.IsNullOrEmpty(BatchYear) && !string.IsNullOrEmpty(Sem) && !string.IsNullOrEmpty(section))
            {
                ////string selqry = "  select Staff_Name,sf.subjectno,(Point)as Points,Question,t.staff_code from CO_FeedbackUniCode FU,CO_FeedBackMaster FM,CO_StudFeedBack SF,staff_appl_master A,staffmaster T,CO_MarkMaster M, CO_QuestionMaster Q where Q.QuestionMasterPK =sf.QuestionMasterFK and M.MarkMasterPK =sf.MarkMasterPK and A.appl_no =t.appl_no and a.appl_id =sf.StaffApplNo and fm.FeedBackMasterPK =fu.FeedbackMasterFK and sf.FeedBackMasterFK =fu.FeedbackMasterFK and sf.FeedBackMasterFK =fm.FeedBackMasterPK and sf.FeedbackUnicode =fu.FeedbackUnicode and fu.FeedbackMasterFK in ('" + FeedBackPk + "') and FM.CollegeCode in ('" + CollegeCode + "') and FM.Batch_Year in ('" + BatchYear + "') and FM.semester in ('" + Sem + "')  and fm.FeedBackMasterPK in ('" + FeedBackPk + "') and SF.StaffApplNo in ('" + StaffCode + "') ";
                //string selqry = "  select Staff_Name,(Point)as Points,Question,t.staff_code from CO_FeedBackMaster FM,CO_StudFeedBack SF,staff_appl_master A,staffmaster T,CO_MarkMaster M, CO_QuestionMaster Q where Q.QuestionMasterPK =sf.QuestionMasterFK and M.MarkMasterPK =sf.MarkMasterPK and A.appl_no =t.appl_no and a.appl_id =sf.StaffApplNo   and sf.FeedBackMasterFK =fm.FeedBackMasterPK and FM.CollegeCode in ('" + CollegeCode + "') and FM.Batch_Year in ('" + BatchYear + "') and FM.semester in ('" + Sem + "')  and fm.FeedBackMasterPK in ('" + FeedBackPk + "') and SF.StaffApplNo in ('" + StaffCode + "') ";
                //selqry += " SELECT distinct Question,QuestionMasterPK FROM CO_FeedBackMaster B,CO_QuestionMaster Q ,CO_FeedBackQuestions FB WHERE  b.FeedBackMasterPK =fb.FeedBackMasterFK and q.QuestionMasterPK =fb.QuestionMasterFK and  InclueCommon='1' and FeedBackType = '1' and B.FeedBackName='" + Convert.ToString(ddl_feedbackformate6.SelectedItem.Value) + "'  and B.CollegeCode in ('" + CollegeCode + "') order by Question";

                Hashtable hat = new Hashtable();
                //========Commented by saranya on 11/9/2018=============//
                //hat.Add("@CollegeCode", CollegeCode);
                //hat.Add("@batchyear", BatchYear);
                //hat.Add("@semester", Sem);
                //hat.Add("@FeedbackName", Convert.ToString(ddl_feedbackformate6.SelectedItem.Text));
                //hat.Add("@StaffAppNo", StaffCode);
                //hat.Add("@section", section);
                //hat.Add("@subjectno", subjectcode);
                //ds = d2.select_method("DepartmentStaffQuestionsWise", hat, "sp");

                //Added by saranya 11/09/2018
                string type = "1";
                string fbpk = " select FeedBackMasterPK,ISNULL(issubjecttype,0)issubjecttype from CO_FeedBackMaster where FeedBackName ='" + ddl_feedbackformate6.SelectedItem.Value + "'";
                DataSet dsfb = d2.select_method_wo_parameter(fbpk, "Text");
                string feedbakpk = string.Empty;
                string issubjecttype = string.Empty;
                if (dsfb.Tables.Count > 0)
                {
                    if (dsfb.Tables[0].Rows.Count > 0)
                    {
                        issubjecttype = Convert.ToString(dsfb.Tables[0].Rows[0]["issubjecttype"]);
                        for (int pk = 0; pk < dsfb.Tables[0].Rows.Count; pk++)
                        {
                            if (string.IsNullOrEmpty(feedbakpk))
                                feedbakpk = dsfb.Tables[0].Rows[pk]["FeedBackMasterPK"].ToString();
                            else
                                feedbakpk = feedbakpk + "," + dsfb.Tables[0].Rows[pk]["FeedBackMasterPK"].ToString() + "";
                        }
                    }
                }
                DataSet dsDegreeCode = new DataSet();
                StringBuilder sbDegreeCode = new StringBuilder();
                string DegCode = "";
                string Appl_id = rs.GetSelectedItemsValueAsString(cbl_staffnameformat6);
                string Query = "select distinct sm.degree_code from Registration r,staff_selector ss,subject s,syllabus_master sm,staff_appl_master sa,staffmaster smm where smm.appl_no=sa.appl_no and  sm.syll_code=s.syll_code and  s.subject_no=ss.subject_no and r.Batch_Year=sm.Batch_Year and r.degree_code=sm.degree_code and r.Current_Semester=sm.semester and r.CC='0' and r.DelFlag<>1 and Exam_Flag='OK' and isRedo<>1 and r.Batch_Year in(" + BatchYear + ") and ss.Sections in('" + section + "','')  and sm.semester in(" + Sem + ") and sa.appl_id in('" + Appl_id + "') and smm.staff_code=ss.staff_code";
                dsDegreeCode.Clear();
                dsDegreeCode = d2.select_method_wo_parameter(Query, "text");
                for (int deg = 0; deg < dsDegreeCode.Tables[0].Rows.Count; deg++)
                {
                    string code = Convert.ToString(dsDegreeCode.Tables[0].Rows[deg]["degree_code"]);
                    sbDegreeCode.Append(code).Append(",");
                }
                DegCode = Convert.ToString(sbDegreeCode);
                DegCode = DegCode.TrimEnd(',');

                hat.Add("@CollegeCode", CollegeCode);
                hat.Add("@batchyear", BatchYear);
                hat.Add("@Degreecode", DegCode);
                hat.Add("@semester", Sem);
                hat.Add("@section", section);
                hat.Add("@FeedbackName", Convert.ToString(ddl_feedbackformate6.SelectedItem.Text));
                hat.Add("@FeedbackMasterFK", feedbakpk);
                hat.Add("@StaffAppNo", StaffCode);
                hat.Add("@FeedbackType", type);
                hat.Add("@subjectno", subjectcode);
                ds = d2.select_method("[DepartmentStaffQuestionsWise]", hat, "sp");
                //======================================================//cbl_staffnameformat6
                DataTable dtChart2 = new DataTable();

                if (ds.Tables[0].Rows.Count > 0)
                {
                    //staff_chart.Series.Clear();
                    string subcode = string.Empty;
                    string staffandcode = string.Empty;
                    string staffnam = string.Empty;
                    ArrayList stafArr = new ArrayList();
                    string question = string.Empty;
                    //question_chart.Series.Clear();
                    Fpspread6.Sheets[0].RowCount = 0;
                    Fpspread6.Sheets[0].ColumnCount = 0;
                    Fpspread6.CommandBar.Visible = false;
                    Fpspread6.Sheets[0].AutoPostBack = false;
                    Fpspread6.Sheets[0].ColumnHeader.RowCount = 1;
                    Fpspread6.Sheets[0].RowHeader.Visible = false;

                    FarPoint.Web.Spread.StyleInfo darkstyle = new FarPoint.Web.Spread.StyleInfo();
                    darkstyle.BackColor = ColorTranslator.FromHtml("#0CA6CA");
                    darkstyle.ForeColor = System.Drawing.Color.White;
                    Fpspread6.ActiveSheetView.ColumnHeader.DefaultStyle = darkstyle;
                    Fpspread6.Sheets[0].ColumnCount = Convert.ToInt32(ds.Tables[1].Rows.Count + 2);
                    Fpspread6.Sheets[0].ColumnHeader.Cells[0, 0].Text = "S.No";
                    Fpspread6.Sheets[0].ColumnHeader.Cells[0, 0].Font.Bold = true;
                    Fpspread6.Sheets[0].ColumnHeader.Cells[0, 0].HorizontalAlign = HorizontalAlign.Center;
                    Fpspread6.Sheets[0].ColumnHeader.Cells[0, 0].Font.Name = "Book Antiqua";
                    Fpspread6.Sheets[0].ColumnHeader.Cells[0, 0].Font.Size = FontUnit.Medium;
                    Fpspread6.Columns[0].Width = 100;

                    Fpspread6.Sheets[0].ColumnHeader.Cells[0, 1].Text = "NAME";
                    Fpspread6.Sheets[0].ColumnHeader.Cells[0, 1].Font.Bold = true;
                    Fpspread6.Sheets[0].ColumnHeader.Cells[0, 1].HorizontalAlign = HorizontalAlign.Center;
                    Fpspread6.Sheets[0].ColumnHeader.Cells[0, 1].Font.Name = "Book Antiqua";
                    Fpspread6.Sheets[0].ColumnHeader.Cells[0, 10].Font.Size = FontUnit.Medium;
                    Fpspread6.Columns[1].Width = 200;
                    for (int i = 0; i < ds.Tables[1].Rows.Count; i++)
                    {
                        question = Convert.ToString(ds.Tables[1].Rows[i]["Question"]);
                        Fpspread6.Sheets[0].ColumnHeader.Cells[0, i + 2].Text = Convert.ToString(question);
                        Fpspread6.Sheets[0].ColumnHeader.Cells[0, i + 2].Tag = Convert.ToString(ds.Tables[1].Rows[i]["QuestionMasterPK"]);
                        Fpspread6.Sheets[0].ColumnHeader.Cells[0, i + 2].Font.Bold = true;
                        Fpspread6.Sheets[0].ColumnHeader.Cells[0, i + 2].HorizontalAlign = HorizontalAlign.Center;
                        Fpspread6.Sheets[0].ColumnHeader.Cells[0, i + 2].Font.Name = "Book Antiqua";
                        Fpspread6.Sheets[0].ColumnHeader.Cells[0, i + 2].Font.Size = FontUnit.Medium;
                        Fpspread6.Columns[i + 2].Width = 200;
                    }
                    int sno = 0;

                    #region Added by Saranya on 12/9/2018

                    if (ds.Tables.Count > 0)
                    {
                        string sum_total = d2.GetFunction("select top 1 Point from CO_MarkMaster  where CollegeCode in('" + collegecode1 + "') order by Point desc");
                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            FarPoint.Web.Spread.CheckBoxCellType cb = new FarPoint.Web.Spread.CheckBoxCellType();
                            cb.AutoPostBack = true;
                            FarPoint.Web.Spread.CheckBoxCellType cb1 = new FarPoint.Web.Spread.CheckBoxCellType();
                            cb1.AutoPostBack = false;
                            //for (int i = 0; i < cbl_staffnameformat6.Items.Count; i++)
                            //{
                            //    if (cbl_staffnameformat6.Items[i].Selected)
                            //    {
                            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                            {
                                //string applId = Convert.ToString(cbl_staffnameformat6.Items[i].Value);
                                //string staff_name = Convert.ToString(cbl_staffnameformat6.Items[i].Text);
                                string staff = Convert.ToString(ds.Tables[0].Rows[i]["staff"]);
                                string[] staffSp = staff.Split('-');
                                string staffName = Convert.ToString(staffSp[1]);
                                string applId = Convert.ToString(ds.Tables[0].Rows[i]["StaffApplNo"]);

                                if (!stafArr.Contains(staff))
                                {
                                    stafArr.Add(staff);
                                    Fpspread6.Sheets[0].Rows.Count++;
                                    sno++;
                                    Fpspread6.Rows[Fpspread6.Sheets[0].Rows.Count - 1].Height = 50;
                                    Fpspread6.Sheets[0].Cells[Fpspread6.Sheets[0].Rows.Count - 1, 0].Text = sno.ToString();
                                    Fpspread6.Sheets[0].Cells[Fpspread6.Sheets[0].Rows.Count - 1, 0].Locked = true;
                                    Fpspread6.Sheets[0].Cells[Fpspread6.Sheets[0].Rows.Count - 1, 0].VerticalAlign = VerticalAlign.Middle;
                                    Fpspread6.Sheets[0].Cells[Fpspread6.Sheets[0].Rows.Count - 1, 0].Font.Name = "Book Antiqua";
                                    Fpspread6.Sheets[0].Cells[Fpspread6.Sheets[0].Rows.Count - 1, 0].Font.Size = FontUnit.Medium;
                                    Fpspread6.Sheets[0].Cells[Fpspread6.Sheets[0].Rows.Count - 1, 0].ForeColor = System.Drawing.Color.SeaGreen;
                                    Fpspread6.Sheets[0].Cells[Fpspread6.Sheets[0].Rows.Count - 1, 0].Font.Bold = true;
                                    Fpspread6.Sheets[0].Cells[Fpspread6.Sheets[0].Rows.Count - 1, 0].HorizontalAlign = HorizontalAlign.Center;

                                    Fpspread6.Rows[Fpspread6.Sheets[0].Rows.Count - 1].Height = 50;
                                    Fpspread6.Sheets[0].Cells[Fpspread6.Sheets[0].Rows.Count - 1, 1].Text = Convert.ToString(staffName);
                                    Fpspread6.Sheets[0].Cells[Fpspread6.Sheets[0].Rows.Count - 1, 1].Locked = true;
                                    Fpspread6.Sheets[0].Cells[Fpspread6.Sheets[0].Rows.Count - 1, 1].VerticalAlign = VerticalAlign.Middle;
                                    Fpspread6.Sheets[0].Cells[Fpspread6.Sheets[0].Rows.Count - 1, 1].Font.Name = "Book Antiqua";
                                    Fpspread6.Sheets[0].Cells[Fpspread6.Sheets[0].Rows.Count - 1, 1].Font.Size = FontUnit.Medium;
                                    Fpspread6.Sheets[0].Cells[Fpspread6.Sheets[0].Rows.Count - 1, 1].ForeColor = System.Drawing.Color.SeaGreen;
                                    Fpspread6.Sheets[0].Cells[Fpspread6.Sheets[0].Rows.Count - 1, 1].Font.Bold = true;
                                    Fpspread6.Sheets[0].Cells[Fpspread6.Sheets[0].Rows.Count - 1, 1].HorizontalAlign = HorizontalAlign.Center;


                                    ds.Tables[0].DefaultView.RowFilter = "StaffApplNo ='" + applId + "'";
                                    DataTable dvStaffApplNo = ds.Tables[0].DefaultView.ToTable();

                                    if (dvStaffApplNo.Rows.Count > 0)
                                    {
                                        for (int j = 2; j < Fpspread6.Columns.Count; j++)
                                        {
                                            double finalValue = 0;
                                            string questionmasterPK = Convert.ToString(Fpspread6.Sheets[0].ColumnHeader.Cells[0, j].Tag);
                                            double TotalCount = 0;
                                            for (int count = 0; count < dvStaffApplNo.Rows.Count; count++)
                                            {
                                                double questavgpoint = 0;
                                                string filterquery = string.Empty;
                                                string sections = Convert.ToString(dvStaffApplNo.Rows[count]["Section"]);
                                                filterquery = "degree_code='" + Convert.ToString(dvStaffApplNo.Rows[count]["DegreeCode"]) + "' ";
                                                if (section.Trim() != "")
                                                {
                                                    filterquery = filterquery + " and Sections='" + sections + "'";
                                                }
                                                ds.Tables[3].DefaultView.RowFilter = "" + filterquery + "";
                                                DataView dvnew = ds.Tables[3].DefaultView;
                                                string totalstudnent = "";
                                                if (dvnew.Count > 0)
                                                {
                                                    totalstudnent = Convert.ToString(dvnew[0]["studentcount"]);
                                                }
                                                if (totalstudnent.Trim() == "")
                                                    totalstudnent = "0";

                                                Double attendstrength = Convert.ToDouble(dvStaffApplNo.Rows[count]["Strength"]);
                                                double maximun = Convert.ToDouble(sum_total) * Convert.ToDouble(attendstrength);
                                                ds.Tables[2].DefaultView.RowFilter = " QuestionMasterfK ='" + questionmasterPK + "' and StaffApplNo='" + applId + "'  and SubjectNo='" + Convert.ToString(dvStaffApplNo.Rows[count]["SubjectNo"]) + "' and Section='" + Convert.ToString(dvStaffApplNo.Rows[count]["Section"]) + "'";
                                                DataView dv = ds.Tables[2].DefaultView;
                                                if (dv.Count > 0)
                                                {
                                                    TotalCount++;
                                                    string point1 = Convert.ToString(dv[0]["points"]);
                                                    if (string.IsNullOrEmpty(point1.Trim()) || point1.Trim() == "-")
                                                        point1 = "0";
                                                    questavgpoint = Convert.ToDouble(point1) / maximun * Convert.ToDouble(sum_total);// 100;
                                                    questavgpoint = Math.Round(questavgpoint, 0, MidpointRounding.AwayFromZero);
                                                }
                                                finalValue = finalValue + questavgpoint;
                                            }
                                            finalValue = finalValue / TotalCount;
                                            finalValue = Math.Round(finalValue, 0, MidpointRounding.AwayFromZero);
                                            if (finalValue > 0)
                                            {
                                                Fpspread6.Sheets[0].Cells[Fpspread6.Sheets[0].Rows.Count - 1, j].Text = Convert.ToString(finalValue);
                                                Fpspread6.Sheets[0].Cells[Fpspread6.Sheets[0].Rows.Count - 1, j].Locked = true;
                                                Fpspread6.Sheets[0].Cells[Fpspread6.Sheets[0].Rows.Count - 1, j].VerticalAlign = VerticalAlign.Middle;
                                                Fpspread6.Sheets[0].Cells[Fpspread6.Sheets[0].Rows.Count - 1, j].Font.Name = "Book Antiqua";
                                                Fpspread6.Sheets[0].Cells[Fpspread6.Sheets[0].Rows.Count - 1, j].Font.Size = FontUnit.Medium;
                                                Fpspread6.Sheets[0].Cells[Fpspread6.Sheets[0].Rows.Count - 1, j].ForeColor = System.Drawing.Color.Black;
                                                Fpspread6.Sheets[0].Cells[Fpspread6.Sheets[0].Rows.Count - 1, j].Font.Bold = true;
                                                Fpspread6.Sheets[0].Cells[Fpspread6.Sheets[0].Rows.Count - 1, j].HorizontalAlign = HorizontalAlign.Center;
                                            }
                                            else
                                            {
                                                Fpspread6.Sheets[0].Cells[Fpspread6.Sheets[0].Rows.Count - 1, j].Text = Convert.ToString(0);
                                                Fpspread6.Sheets[0].Cells[Fpspread6.Sheets[0].Rows.Count - 1, j].Locked = true;
                                                Fpspread6.Sheets[0].Cells[Fpspread6.Sheets[0].Rows.Count - 1, j].VerticalAlign = VerticalAlign.Middle;
                                                Fpspread6.Sheets[0].Cells[Fpspread6.Sheets[0].Rows.Count - 1, j].Font.Name = "Book Antiqua";
                                                Fpspread6.Sheets[0].Cells[Fpspread6.Sheets[0].Rows.Count - 1, j].Font.Size = FontUnit.Medium;
                                                Fpspread6.Sheets[0].Cells[Fpspread6.Sheets[0].Rows.Count - 1, j].ForeColor = System.Drawing.Color.Black;
                                                Fpspread6.Sheets[0].Cells[Fpspread6.Sheets[0].Rows.Count - 1, j].Font.Bold = true;
                                                Fpspread6.Sheets[0].Cells[Fpspread6.Sheets[0].Rows.Count - 1, j].HorizontalAlign = HorizontalAlign.Center;
                                            }
                                        }
                                    }
                                }
                            }
                            //}
                            //}
                            Fpspread6.Height = 500;
                            Fpspread6.Sheets[0].PageSize = Fpspread6.Sheets[0].RowCount;
                        }
                        else
                        {
                            imgdiv2.Visible = true;
                            lbl_alert1.Visible = true;
                            lbl_alert1.Text = "No Records Found";
                            Fpspread6.Visible = false;
                        }
                    }
                    else
                    {
                        imgdiv2.Visible = true;
                        lbl_alert1.Visible = true;
                        lbl_alert1.Text = "No Records Found";
                        Fpspread6.Visible = false;
                    }
                    #endregion

                    #region Commented By saranya on 11/9/2018
                    //for (int r = 0; r < ds.Tables[0].Rows.Count; r++)
                    //{
                    //    string pss = string.Empty;
                    //    staffnam = ds.Tables[0].Rows[r]["Staff_Name"].ToString();
                    //    subcode = ds.Tables[0].Rows[r]["staff_code"].ToString();
                    //    staffandcode = staffnam + "-" + subcode;
                    //    if (!stafArr.Contains(staffandcode))
                    //    {
                    //        sno++;
                    //        DataView dv = new DataView();
                    //        dv = ds.Tables[0].DefaultView;
                    //        dc = new DataColumn();
                    //        stafArr.Add(staffandcode);
                    //        //dc.ColumnName = staffandcode;
                    //        //dtChart2.Columns.Add(dc);
                    //        Fpspread6.Sheets[0].Rows.Count++;
                    //        Fpspread6.Rows[Fpspread6.Sheets[0].Rows.Count - 1].Height = 50;
                    //        Fpspread6.Sheets[0].Cells[Fpspread6.Sheets[0].Rows.Count - 1, 0].Text = sno.ToString();
                    //        Fpspread6.Sheets[0].Cells[Fpspread6.Sheets[0].Rows.Count - 1, 0].Locked = true;
                    //        Fpspread6.Sheets[0].Cells[Fpspread6.Sheets[0].Rows.Count - 1, 0].VerticalAlign = VerticalAlign.Middle;
                    //        Fpspread6.Sheets[0].Cells[Fpspread6.Sheets[0].Rows.Count - 1, 0].Font.Name = "Book Antiqua";
                    //        Fpspread6.Sheets[0].Cells[Fpspread6.Sheets[0].Rows.Count - 1, 0].Font.Size = FontUnit.Medium;
                    //        Fpspread6.Sheets[0].Cells[Fpspread6.Sheets[0].Rows.Count - 1, 0].ForeColor = System.Drawing.Color.SeaGreen;
                    //        Fpspread6.Sheets[0].Cells[Fpspread6.Sheets[0].Rows.Count - 1, 0].Font.Bold = true;
                    //        Fpspread6.Sheets[0].Cells[Fpspread6.Sheets[0].Rows.Count - 1, 0].HorizontalAlign = HorizontalAlign.Center;

                    //        Fpspread6.Rows[Fpspread6.Sheets[0].Rows.Count - 1].Height = 50;
                    //        Fpspread6.Sheets[0].Cells[Fpspread6.Sheets[0].Rows.Count - 1, 1].Text = ds.Tables[0].Rows[r]["Staff_Name"].ToString();
                    //        Fpspread6.Sheets[0].Cells[Fpspread6.Sheets[0].Rows.Count - 1, 1].Locked = true;
                    //        Fpspread6.Sheets[0].Cells[Fpspread6.Sheets[0].Rows.Count - 1, 1].VerticalAlign = VerticalAlign.Middle;
                    //        Fpspread6.Sheets[0].Cells[Fpspread6.Sheets[0].Rows.Count - 1, 1].Font.Name = "Book Antiqua";
                    //        Fpspread6.Sheets[0].Cells[Fpspread6.Sheets[0].Rows.Count - 1, 1].Font.Size = FontUnit.Medium;
                    //        Fpspread6.Sheets[0].Cells[Fpspread6.Sheets[0].Rows.Count - 1, 1].ForeColor = System.Drawing.Color.SeaGreen;
                    //        Fpspread6.Sheets[0].Cells[Fpspread6.Sheets[0].Rows.Count - 1, 1].Font.Bold = true;
                    //        Fpspread6.Sheets[0].Cells[Fpspread6.Sheets[0].Rows.Count - 1, 1].HorizontalAlign = HorizontalAlign.Center;

                    //        int ii = 2;
                    //        //DataRow drpq;
                    //        for (int i = 0; i < ds.Tables[1].Rows.Count; i++)
                    //        {
                    //            DataTable dtstp = new DataTable();
                    //            question = Convert.ToString(ds.Tables[1].Rows[i]["Question"]);
                    //            ds.Tables[0].DefaultView.RowFilter = "Question='" + question + "' and Staff_Name='" + staffnam + "' ";
                    //            dtstp = ds.Tables[0].DefaultView.ToTable();
                    //            //drpq = dtChart2.NewRow();
                    //            ArrayList ques = new ArrayList();
                    //            subcode = string.Empty;
                    //            //staffandcode = string.Empty;
                    //            //staffnam = string.Empty;
                    //            string points = string.Empty;
                    //            Double poin = 0;
                    //            Double ps = 0;
                    //            int w = 0;
                    //            for (int c = 0; c < dtstp.Rows.Count; c++)
                    //            {
                    //                w++;
                    //                points = Convert.ToString(dtstp.Rows[c]["Points"]);
                    //                string sum_total = d2.GetFunction("select top 1 Point from CO_MarkMaster  where CollegeCode in('" + collegecode1 + "') order by Point desc");
                    //                string needs = sum_total;
                    //                ConvertedMark(needs, sum_total, ref points);
                    //                double.TryParse(Convert.ToString(points), out poin);
                    //                ps += poin;
                    //            }
                    //            if (ps > 0)
                    //            {
                    //                ps = ps / w;
                    //                //  pss = Convert.ToString(Math.Round(ps, 1));
                    //                ps = Math.Round(ps, 0, MidpointRounding.AwayFromZero);
                    //                Fpspread6.Sheets[0].Cells[Fpspread6.Sheets[0].Rows.Count - 1, ii].Text = Convert.ToString(ps);//delsi1703
                    //                Fpspread6.Sheets[0].Cells[Fpspread6.Sheets[0].Rows.Count - 1, ii].Locked = true;
                    //                Fpspread6.Sheets[0].Cells[Fpspread6.Sheets[0].Rows.Count - 1, ii].VerticalAlign = VerticalAlign.Middle;
                    //                Fpspread6.Sheets[0].Cells[Fpspread6.Sheets[0].Rows.Count - 1, ii].Font.Name = "Book Antiqua";
                    //                Fpspread6.Sheets[0].Cells[Fpspread6.Sheets[0].Rows.Count - 1, ii].Font.Size = FontUnit.Medium;
                    //                Fpspread6.Sheets[0].Cells[Fpspread6.Sheets[0].Rows.Count - 1, ii].ForeColor = System.Drawing.Color.Black;
                    //                Fpspread6.Sheets[0].Cells[Fpspread6.Sheets[0].Rows.Count - 1, ii].Font.Bold = true;
                    //                Fpspread6.Sheets[0].Cells[Fpspread6.Sheets[0].Rows.Count - 1, ii].HorizontalAlign = HorizontalAlign.Center;
                    //                ii++;
                    //            }
                    //            else
                    //            {
                    //                Fpspread6.Sheets[0].Cells[Fpspread6.Sheets[0].Rows.Count - 1, ii].Text = Convert.ToString(0);
                    //                Fpspread6.Sheets[0].Cells[Fpspread6.Sheets[0].Rows.Count - 1, ii].Locked = true;
                    //                Fpspread6.Sheets[0].Cells[Fpspread6.Sheets[0].Rows.Count - 1, ii].VerticalAlign = VerticalAlign.Middle;
                    //                Fpspread6.Sheets[0].Cells[Fpspread6.Sheets[0].Rows.Count - 1, ii].Font.Name = "Book Antiqua";
                    //                Fpspread6.Sheets[0].Cells[Fpspread6.Sheets[0].Rows.Count - 1, ii].Font.Size = FontUnit.Medium;
                    //                Fpspread6.Sheets[0].Cells[Fpspread6.Sheets[0].Rows.Count - 1, ii].ForeColor = System.Drawing.Color.Black;
                    //                Fpspread6.Sheets[0].Cells[Fpspread6.Sheets[0].Rows.Count - 1, ii].Font.Bold = true;
                    //                Fpspread6.Sheets[0].Cells[Fpspread6.Sheets[0].Rows.Count - 1, ii].HorizontalAlign = HorizontalAlign.Center;
                    //                ii++;
                    //            }
                    //        }
                    //        //for (int st = 0; st < dtstp.Rows.Count; st++)
                    //        //{

                    //        //    staffnam = dtstp.Rows[st]["Staff_Name"].ToString();
                    //        //    subcode = dtstp.Rows[st]["staff_code"].ToString();
                    //        //    staffandcode = staffnam + "-" + subcode;
                    //        //    if (!ques.Contains(staffandcode))
                    //        //    {
                    //        //        ques.Add(staffandcode);
                    //        //        dtstp.DefaultView.RowFilter = "Staff_Name='" + staffnam + "' and staff_code='" + subcode + "' ";
                    //        //        DataView dvv = new DataView();
                    //        //        dvv = dtstp.DefaultView;
                    //        //        string points = string.Empty;
                    //        //        Double poin = 0;
                    //        //        Double ps = 0;
                    //        //        int w = 0;
                    //        //        for (int c = 0; c < dvv.Count; c++)
                    //        //        {
                    //        //            w++;
                    //        //            points = Convert.ToString(dvv[c]["Points"]);
                    //        //            string sum_total = d2.GetFunction("select top 1 Point from CO_MarkMaster  where CollegeCode in('" + collegecode1 + "') order by Point desc");
                    //        //            string needs = sum_total;
                    //        //            ConvertedMark(needs, sum_total, ref points);
                    //        //            double.TryParse(Convert.ToString(points), out poin);
                    //        //            ps += poin;
                    //        //        }
                    //        //        ps = ps / w;
                    //        //        pss = Convert.ToString(Math.Round(ps, 1));

                    //        //        //Fpspread6.Sheets[0].Rows.Count++;
                    //        //Fpspread6.Sheets[0].Cells[Fpspread6.Sheets[0].Rows.Count - 1 + aa, ii].Text = pss;
                    //        //Fpspread6.Sheets[0].Cells[Fpspread6.Sheets[0].Rows.Count - 1 + aa, ii].Locked = true;
                    //        //Fpspread6.Sheets[0].Cells[Fpspread6.Sheets[0].Rows.Count - 1 + aa, ii].VerticalAlign = VerticalAlign.Middle;
                    //        //Fpspread6.Sheets[0].Cells[Fpspread6.Sheets[0].Rows.Count - 1 + aa, ii].Font.Name = "Book Antiqua";
                    //        //Fpspread6.Sheets[0].Cells[Fpspread6.Sheets[0].Rows.Count - 1 + aa, ii].Font.Size = FontUnit.Medium;
                    //        //Fpspread6.Sheets[0].Cells[Fpspread6.Sheets[0].Rows.Count - 1 + aa, ii].ForeColor = System.Drawing.Color.Black;
                    //        //Fpspread6.Sheets[0].Cells[Fpspread6.Sheets[0].Rows.Count - 1 + aa, ii].Font.Bold = true;
                    //        //Fpspread6.Sheets[0].Cells[Fpspread6.Sheets[0].Rows.Count - 1 + aa, ii].HorizontalAlign = HorizontalAlign.Center;
                    //        //        aa++;
                    //        //        //dtChart2.Rows.Add(drpq);
                    //        //    }
                    //        //}
                    //    }
                    //}
                    #endregion

                    Fpspread6.Sheets[0].PageSize = Fpspread6.Sheets[0].RowCount;
                    Fpspread6.Visible = true;
                    rptprint1.Visible = true;
                }
            }
        }
        catch (Exception ex)
        {
            d2.sendErrorMail(ex, collegecode1, "FeedbackReport");
        }
    }

    #region Section for departmentwise Added by Saranya 08/09/2018

    public void bindsectionDeptWise()
    {
        try
        {
            cbl_sec.Items.Clear();
            txt_section.Text = "---Select---";
            cb_secDeptWise.Checked = false;
            string batch = rs.GetSelectedItemsValueAsString(cbl_batch);
            //string branchcode1 = rs.GetSelectedItemsValueAsString(ddlformate6_deptname);
            if (batch != "")
            {
                //ds = d2.BindSectionDetail(batch, branchcode1);
                ds = d2.select_method_wo_parameter("select distinct sections from registration where batch_year in('" + batch + "')  and sections<>'-1' and ltrim(sections)<>'' and sections is not null and delflag=0 and exam_flag<>'Debar' and CC=0 order by Sections", "Text");
                if (ds.Tables[0].Rows.Count > 0)
                {
                    cbl_secDeptWise.DataSource = ds;
                    cbl_secDeptWise.DataTextField = "sections";
                    cbl_secDeptWise.DataValueField = "sections";
                    cbl_secDeptWise.DataBind();
                    if (cbl_secDeptWise.Items.Count > 0)
                    {
                        cbl_secDeptWise.Items.Add(new System.Web.UI.WebControls.ListItem("Empty", " "));
                        for (int row = 0; row < cbl_secDeptWise.Items.Count; row++)
                        {
                            cbl_secDeptWise.Items[row].Selected = true;
                            cb_secDeptWise.Checked = true;
                        }
                        txt_section.Text = "Section(" + cbl_secDeptWise.Items.Count + ")";
                    }
                }
                else
                {
                    cbl_secDeptWise.Items.Add(new System.Web.UI.WebControls.ListItem("Empty", " "));
                    for (int row = 0; row < cbl_secDeptWise.Items.Count; row++)
                    {
                        cbl_secDeptWise.Items[row].Selected = true;
                        cb_secDeptWise.Checked = true;
                    }
                    txt_section.Text = "Section(" + cbl_secDeptWise.Items.Count + ")";
                }
            }
            else
            {
                cbl_secDeptWise.Items.Add(new System.Web.UI.WebControls.ListItem("Empty", " "));
                for (int row = 0; row < cbl_secDeptWise.Items.Count; row++)
                {
                    cbl_secDeptWise.Items[row].Selected = true;
                    cb_secDeptWise.Checked = true;
                }
                txt_section.Text = "Section(" + cbl_secDeptWise.Items.Count + ")";
            }
        }
        catch (Exception ex)
        {
            d2.sendErrorMail(ex, collegecode1, "Feedback Report");
        }
    }

    public void cb_secDeptWise_CheckedChanged(object sender, EventArgs e)
    {
        try
        {
            int cout = 0;
            txt_section.Text = "--Select--";
            if (cb_secDeptWise.Checked == true)
            {
                cout++;
                for (int i = 0; i < cbl_secDeptWise.Items.Count; i++)
                {
                    cbl_secDeptWise.Items[i].Selected = true;
                }
                txt_sec.Text = "Semester(" + (cbl_secDeptWise.Items.Count) + ")";
            }
            else
            {
                for (int i = 0; i < cbl_secDeptWise.Items.Count; i++)
                {
                    cbl_secDeptWise.Items[i].Selected = false;
                }
                txt_section.Text = "--Select--";
            }
        }
        catch (Exception ex)
        {
            d2.sendErrorMail(ex, collegecode1, "FeedbackReport");
        }
    }

    public void cbl_secDeptWise_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            int commcount = 0;
            txt_section.Text = "--Select--";
            cb_secDeptWise.Checked = false;
            for (int i = 0; i < cbl_secDeptWise.Items.Count; i++)
            {
                if (cbl_secDeptWise.Items[i].Selected == true)
                {
                    commcount = commcount + 1;
                    cb_secDeptWise.Checked = false;
                }
            }
            if (commcount > 0)
            {
                if (commcount == cbl_secDeptWise.Items.Count)
                {
                    cb_secDeptWise.Checked = true;
                }
                txt_section.Text = "Section(" + commcount.ToString() + ")";
            }
        }
        catch (Exception ex)
        {
        }
    }

    public void cb_subjectnameformat6_CheckedChanged(object sender, EventArgs e)
    {
        rs.CallCheckBoxChangedEvent(cbl_subjectnameformat6, cb_subjectnameformat6, txtsubjectnameformat6, "Subject");
    }

    public void cbl_subjectnameformat6_SelectedIndexChanged(object sender, EventArgs e)
    {
        rs.CallCheckBoxListChangedEvent(cbl_subjectnameformat6, cb_subjectnameformat6, txtsubjectnameformat6, "Subject");
    }

    protected void bindsubjectformate6()
    {
        DataSet dsDegreeCode = new DataSet();
        StringBuilder sbDegreeCode = new StringBuilder();
        string DegCode = "";
        if (ddl_feedbackformate6.Items.Count > 0)
        {
            if (ddl_feedbackformate6.SelectedItem.Text != "Select")
            {
                txtsubjectnameformat6.Text = "--Select--";
                string college_cd = rs.GetSelectedItemsValueAsString(cbl_clgnameformat6);
                string Batch_Year = rs.GetSelectedItemsValueAsString(cbl_formate6batch);
                //string degree_code = Convert.ToString(ddlformate6_deptname.SelectedValue);
                string semester = rs.GetSelectedItemsValueAsString(cbl_formate6sem);
                string section = rs.GetSelectedItemsValueAsString(cbl_secDeptWise);
                string Appl_id = rs.GetSelectedItemsValueAsString(cbl_staffnameformat6);
                string feedbakpk = string.Empty;
                if (Appl_id != "")
                {
                    string Query = "select distinct sm.degree_code from Registration r,staff_selector ss,subject s,syllabus_master sm,staff_appl_master sa,staffmaster smm where smm.appl_no=sa.appl_no and  sm.syll_code=s.syll_code and  s.subject_no=ss.subject_no and r.Batch_Year=sm.Batch_Year and r.degree_code=sm.degree_code and r.Current_Semester=sm.semester and r.CC='0' and r.DelFlag<>1 and Exam_Flag='OK' and isRedo<>1 and r.Batch_Year in('" + Batch_Year + "') and ss.Sections in('" + section + "','')  and sm.semester in('" + semester + "') and sa.appl_id in('" + Appl_id + "') and smm.staff_code=ss.staff_code";
                    dsDegreeCode.Clear();
                    dsDegreeCode = d2.select_method_wo_parameter(Query, "text");
                    for (int deg = 0; deg < dsDegreeCode.Tables[0].Rows.Count; deg++)
                    {
                        string code = Convert.ToString(dsDegreeCode.Tables[0].Rows[deg]["degree_code"]);
                        sbDegreeCode.Append(code).Append("','");
                    }
                    DegCode = Convert.ToString(sbDegreeCode);
                    DegCode = DegCode.TrimEnd(',');

                    //cbl_subjectnameformat6.Items.Clear();
                    //if (ds.Tables[0].Rows.Count > 0)
                    //{
                    //    cbl_subjectnameformat6.DataSource = ds;
                    //    cbl_subjectnameformat6.DataTextField = "subject_name";
                    //    cbl_subjectnameformat6.DataValueField = "subject_no";
                    //    cbl_subjectnameformat6.DataBind();
                    //}
                    //if (cbl_subjectnameformat6.Items.Count > 0)
                    //{
                    //    for (int row = 0; row < cbl_subjectnameformat6.Items.Count; row++)
                    //    {
                    //        cbl_subjectnameformat6.Items[row].Selected = true;
                    //        cb_subjectnameformat6.Checked = true;
                    //    }
                    //    txtsubjectnameformat6.Text = "Subject(" + cbl_subjectnameformat6.Items.Count + ")";
                    //}


                    //string Degree = d2.GetFunction(" select distinct degree.degree_code from degree,department,course where course.course_id=degree.course_id and department.dept_code=degree.dept_code and course.college_code = degree.college_code and department.college_code = degree.college_code and degree.college_code in ('" + college_cd + "') and department.Dept_Code='" + DegCode + "'");
                    if (section.Trim() != "")
                    {
                        section = section + "','";
                    }
                    if (semester.Trim() != "" && Batch_Year.Trim() != "")
                    {
                        string fbpk = " select FeedBackMasterPK,ISNULL(issubjecttype,0)issubjecttype from CO_FeedBackMaster where FeedBackName ='" + ddl_feedbackformate6.SelectedItem.Value + "'";
                        DataSet dsfb = d2.select_method_wo_parameter(fbpk, "Text");

                        string issubjecttype = string.Empty;
                        if (dsfb.Tables.Count > 0)
                        {
                            if (dsfb.Tables[0].Rows.Count > 0)
                            {
                                issubjecttype = Convert.ToString(dsfb.Tables[0].Rows[0]["issubjecttype"]);
                                for (int pk = 0; pk < dsfb.Tables[0].Rows.Count; pk++)
                                {
                                    if (string.IsNullOrEmpty(feedbakpk))
                                        feedbakpk = dsfb.Tables[0].Rows[pk]["FeedBackMasterPK"].ToString();
                                    else
                                        feedbakpk = feedbakpk + "','" + dsfb.Tables[0].Rows[pk]["FeedBackMasterPK"].ToString() + "";
                                }
                                string query = "select distinct s.subject_name,s.subject_no from subject s,CO_StudFeedBack sf where s.subject_no=sf.SubjectNo and sf.FeedBackMasterFK in('" + feedbakpk + "') and StaffApplNo in ('" + Appl_id + "')";
                                ds.Clear();
                                ds = d2.select_method_wo_parameter(query, "text");
                                cbl_subjectnameformat6.Items.Clear();
                                if (ds.Tables[0].Rows.Count > 0)
                                {
                                    cbl_subjectnameformat6.DataSource = ds;
                                    cbl_subjectnameformat6.DataTextField = "subject_name";
                                    cbl_subjectnameformat6.DataValueField = "subject_no";
                                    cbl_subjectnameformat6.DataBind();
                                }
                                if (cbl_subjectnameformat6.Items.Count > 0)
                                {
                                    for (int row = 0; row < cbl_subjectnameformat6.Items.Count; row++)
                                    {
                                        cbl_subjectnameformat6.Items[row].Selected = true;
                                        cb_subjectnameformat6.Checked = true;
                                    }
                                    txtsubjectnameformat6.Text = "Subject(" + cbl_subjectnameformat6.Items.Count + ")";
                                }
                            }
                        }
                        //string q1 = " select FeedBackMasterPK from CO_FeedBackMaster where FeedBackName ='" + ddl_feedbackformate6.SelectedItem.Value + "' and DegreeCode in ('" + DegCode + "') and semester in ('" + semester + "') and Batch_Year in('" + Batch_Year + "')";
                        //if (section.Trim() != "")
                        //{
                        //    q1 += " and section in ('" + section + "')";
                        //}
                        //ds.Clear();
                        //ds = d2.select_method_wo_parameter(q1, "text");
                        //if (ds.Tables[0].Rows.Count > 0)
                        //{
                        // string feedbakpk = GetdatasetRowstring(ds, "FeedBackMasterPK");



                    }
                }
            }
        }
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

    #endregion


}
/*
 14.10.16 formate6 semester fileter added 1.13 PM
 * 18.11.16 ananamous formate7 6.25pm
 * 09.01.17 ddlstaff(),load_staffname() formate4 and formate7
 */
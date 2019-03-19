using System;
using System.Web.UI.WebControls;
using System.Data;
using System.Collections;
using System.Drawing;
using System.Collections.Generic;
public partial class FeedBack_Master : System.Web.UI.Page
{
    bool cellclick = false;
    string usercode = string.Empty;
    string collegecode1 = string.Empty;
    string collegecode = string.Empty;
    string singleuser = string.Empty;
    string group_user = string.Empty;
    string selectQuery = string.Empty;
    DAccess2 d2 = new DAccess2();
    DataSet ds = new DataSet();
    DataSet ds1 = new DataSet();
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
        if (!IsPostBack)
        {
            txt_fromdate.Text = DateTime.Now.ToString("dd/MM/yyyy");
            txt_Enddate.Text = DateTime.Now.ToString("dd/MM/yyyy");
            txt_Enddate.Attributes.Add("readonly", "readonly");
            txt_fromdate.Attributes.Add("readonly", "readonly");
            bindddlclg();
            BindBatch();
            BindDegree();
            bindbranch();
            bindsem();
            bindsec();
            bindclg1();
            BindBatch1();
            BindDegree1();
            bindbranch1();
            bindsem1();
            bindsec1();
            FbName1();
            bind_subjecttype();
            FpSpread1.SaveChanges();
            includecommon();
            FpSpread1.Visible = false;
        }
    }
    protected void btnAdd1_Click(object sender, EventArgs e)
    {
    }
    public void bindddlclg()
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
            }
        }
        catch (Exception ex)
        {
        }
    }
    protected void ddl_college_SelectedIndexChanged(object sender, EventArgs e)
    {
        BindBatch();
        BindDegree();
        bindbranch();
        bindsem();
        bindsec();
        bind_subjecttype();
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
            }
            BindDegree();
            bindbranch();
            bindsem();
            bindsec();
            bind_subjecttype();
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
            string buildvalue = "";
            string build = "";
            cb_batch.Checked = false;
            txt_batch.Text = "--Select--";
            for (int i = 0; i < cbl_batch.Items.Count; i++)
            {
                if (cbl_batch.Items[i].Selected == true)
                {
                    commcount = commcount + 1;
                    //cb_batch.Checked = false;
                    build = cbl_batch.Items[i].Value.ToString();
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
            bind_subjecttype();
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
            string college_cd = "";
            college_cd = "" + ddl_college.SelectedItem.Value.ToString() + "";
            if (college_cd != "")
            {
                ds.Clear();
                ds = d2.BindBatch();
                if (ds.Tables[0].Rows.Count > 0)
                {
                    cbl_batch.DataSource = ds;
                    cbl_batch.DataTextField = "batch_year";
                    cbl_batch.DataValueField = "batch_year";
                    cbl_batch.DataBind();
                }
                if (cbl_batch.Items.Count > 0)
                {
                    //for (int row = 0; row < cbl_batch.Items.Count; row++)
                    //{
                    //    cbl_batch.Items[row].Selected = true;
                    //    cb_batch.Checked = true;
                    //}
                    //txt_batch.Text = "Batch(" + cbl_batch.Items.Count + ")";
                    txt_batch.Text = "--Select--";
                }
                else
                {
                    txt_batch.Text = "--Select--";
                }
            }
            BindDegree();
        }
        catch
        {
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
                }
                txt_degree.Text = "--Select--";
            }
            bindbranch();
            bindsem();
            bindsec();
            bind_subjecttype();
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
            bind_subjecttype();
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
            college_cd = "" + ddl_college.SelectedItem.Value.ToString() + "";
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
                        //    for (int row = 0; row < cbl_degree.Items.Count; row++)
                        //    {
                        //        cbl_degree.Items[row].Selected = true;
                        //    }
                        //    cb_degree.Checked = true;
                        //    txt_degree.Text = "Degree(" + cbl_degree.Items.Count + ")";
                        txt_degree.Text = "--Select--";
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
                txt_branch.Text = "Branch(" + (cbl_branch.Items.Count) + ")";
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
            bind_subjecttype();
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
                txt_branch.Text = "Branch(" + commcount.ToString() + ")";
            }
            bindsem();
            bindsec();
            bind_subjecttype();
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
            //   string college_cd = "";
            college_cd = "" + ddl_college.SelectedItem.Value.ToString() + "";
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
                        //for (int row = 0; row < cbl_branch.Items.Count; row++)
                        //{
                        //    cbl_branch.Items[row].Selected = true;
                        //}
                        //cb_branch.Checked = true;
                        //txt_branch.Text = "Branch(" + cbl_branch.Items.Count + ")";
                        txt_branch.Text = "--Select--";
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
            bind_subjecttype();
        }
        catch (Exception ex)
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
                    cb_sem.Checked = false;
                }
                txt_sem.Text = "Semester(" + commcount.ToString() + ")";
            }
            bindsec();
            bind_subjecttype();
        }
        catch (Exception ex)
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
        college_cd = "" + ddl_college.SelectedItem.Value.ToString() + "";
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
        cbl_sem.Items.Clear();
        if (branch.Trim() != "" && batch.Trim() != "")
        {
            string query = "select distinct Current_Semester from Registration where degree_code in (" + branch + ") and Batch_Year in (" + batch + ") and college_code in ('" + college_cd + "') and CC=0 and DelFlag =0 and Exam_Flag <>'debar' order by Current_Semester";
            ds = d2.select_method_wo_parameter(query, "Text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                cbl_sem.DataSource = ds;
                cbl_sem.DataTextField = "Current_Semester";
                cbl_sem.DataBind();
                if (cbl_sem.Items.Count > 0)
                {
                    //for (int row = 0; row < cbl_sem.Items.Count; row++)
                    //{
                    //    cbl_sem.Items[row].Selected = true;
                    //    cb_sem.Checked = true;
                    //}
                    //txt_sem.Text = "Semester(" + cbl_sem.Items.Count + ")";
                    txt_sem.Text = "--Select--";
                }
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
        catch
        {
        }
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
            //bindhostelname();
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
            ds.Clear();
            if (batch != "" || branchcode1 != "")
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
                        //    for (int row = 0; row < cbl_sec.Items.Count; row++)
                        //    {
                        //        cbl_sec.Items[row].Selected = true;
                        //        cb_sec.Checked = true;
                        //    }
                        //    txt_sec.Text = "Section(" + cbl_sec.Items.Count + ")";
                        txt_sec.Text = "--Select--";
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
            BindBatch1();
            BindDegree1();
            bindbranch1();
            bindsem1();
            bindsec1();
            FbName1();
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
            }
            BindBatch1();
            BindDegree1();
            bindbranch1();
            bindsem1();
            bindsec1();
            FbName1();
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
    public void cb_batch1_CheckedChanged(object sender, EventArgs e)
    {
        try
        {
            int count = 0;
            txt_batch1.Text = "--Select--";
            if (cb_batch1.Checked == true)
            {
                count++;
                for (int i = 0; i < cbl_batch1.Items.Count; i++)
                {
                    cbl_batch1.Items[i].Selected = true;
                }
                txt_batch1.Text = "Batch(" + (cbl_batch1.Items.Count) + ")";
            }
            else
            {
                for (int i = 0; i < cbl_batch1.Items.Count; i++)
                {
                    cbl_batch1.Items[i].Selected = false;
                }
                txt_batch1.Text = "--Select--";
            }
            BindDegree1();
            bindbranch1();
            bindsem1();
            bindsec1();
            FbName1();
        }
        catch (Exception ex)
        {
        }
    }
    public void cbl_batch1_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            int commcount = 0;
            string buildvalue = "";
            string build = "";
            cb_batch1.Checked = false;
            txt_batch1.Text = "--Select--";
            for (int i = 0; i < cbl_batch1.Items.Count; i++)
            {
                if (cbl_batch1.Items[i].Selected == true)
                {
                    commcount = commcount + 1;
                    //cb_batch1.Checked = false;
                    build = cbl_batch1.Items[i].Value.ToString();
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
                txt_batch1.Text = "Batch(" + commcount.ToString() + ")";
                if (commcount == cbl_batch1.Items.Count)
                {
                    cb_batch1.Checked = true;
                }
                txt_batch1.Text = "Batch(" + commcount.ToString() + ")";
            }
            BindDegree1();
            bindbranch1();
            bindsem1();
            bindsec1();
            FbName1();
        }
        catch (Exception ex)
        {
        }
    }
    public void BindBatch1()
    {
        try
        {
            cbl_batch1.Items.Clear();
            txt_batch1.Text = "--Select--";
            cb_batch1.Checked = false;
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
            if (college != "")
            {
                ds.Clear();
                cbl_batch1.Items.Clear();
                ds = d2.BindBatch();
                if (ds.Tables[0].Rows.Count > 0)
                {
                    cbl_batch1.DataSource = ds;
                    cbl_batch1.DataTextField = "batch_year";
                    cbl_batch1.DataValueField = "batch_year";
                    cbl_batch1.DataBind();
                }
                if (cbl_batch1.Items.Count > 0)
                {
                    for (int row = 0; row < cbl_batch1.Items.Count; row++)
                    {
                        cbl_batch1.Items[row].Selected = true;
                        cb_batch1.Checked = true;
                    }
                    txt_batch1.Text = "Batch(" + cbl_batch1.Items.Count + ")";
                }
                else
                {
                    txt_batch1.Text = "--Select--";
                }
                BindDegree1();
            }
        }
        catch
        {
        }
    }
    public void cb_degree1_CheckedChanged(object sender, EventArgs e)
    {
        try
        {
            int count = 0;
            txt_degree1.Text = "--Select--";
            if (cb_degree1.Checked == true)
            {
                count++;
                for (int i = 0; i < cbl_degree1.Items.Count; i++)
                {
                    cbl_degree1.Items[i].Selected = true;
                }
                txt_degree1.Text = "Degree(" + (cbl_degree1.Items.Count) + ")";
            }
            else
            {
                for (int i = 0; i < cbl_degree1.Items.Count; i++)
                {
                    cbl_degree1.Items[i].Selected = false;
                }
                txt_degree1.Text = "--Select--";
            }
            bindbranch1();
            bindsem1();
            bindsec1();
            FbName1();
        }
        catch (Exception ex)
        {
        }
    }
    public void cbl_degree1_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            int i = 0;
            int commcount = 0;
            cb_degree1.Checked = false;
            txt_degree1.Text = "--Select--";
            for (i = 0; i < cbl_degree1.Items.Count; i++)
            {
                if (cbl_degree1.Items[i].Selected == true)
                {
                    commcount = commcount + 1;
                }
            }
            if (commcount > 0)
            {
                if (commcount == cbl_degree1.Items.Count)
                {
                    cb_degree1.Checked = true;
                }
                txt_degree1.Text = "Degree (" + commcount.ToString() + ")";
            }
            bindbranch1();
            bindsem1();
            bindsec1();
            FbName1();
        }
        catch (Exception ex)
        {
        }
    }
    public void BindDegree1()
    {
        try
        {
            cbl_degree1.Items.Clear();
            string build = "";
            if (cbl_batch1.Items.Count > 0)
            {
                for (int i = 0; i < cbl_batch1.Items.Count; i++)
                {
                    if (cbl_batch1.Items[i].Selected == true)
                    {
                        if (build == "")
                        {
                            build = Convert.ToString(cbl_batch1.Items[i].Value);
                        }
                        else
                        {
                            build = build + "'" + "," + "'" + Convert.ToString(cbl_batch1.Items[i].Value);
                        }
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
            string query = "";
            if (build != "")
            {
                ds.Clear();
                query = "select distinct degree.course_id,course.course_name from degree,course where course.course_id=degree.course_id and course.college_code = degree.college_code and degree.college_code in ('" + college + "')";
                ds = d2.select_method_wo_parameter(query, "Text");
                //  ds = d2.BindDegree(singleuser, group_user, collegecode1, usercode);
                int count1 = ds.Tables[0].Rows.Count;
                if (count1 > 0)
                {
                    cbl_degree1.DataSource = ds;
                    cbl_degree1.DataTextField = "course_name";
                    cbl_degree1.DataValueField = "course_id";
                    cbl_degree1.DataBind();
                    if (cbl_degree1.Items.Count > 0)
                    {
                        for (int row = 0; row < cbl_degree1.Items.Count; row++)
                        {
                            cbl_degree1.Items[row].Selected = true;
                        }
                        cb_degree1.Checked = true;
                        txt_degree1.Text = "Degree(" + cbl_degree1.Items.Count + ")";
                    }
                }
            }
            else
            {
                cb_degree1.Checked = false;
                txt_degree1.Text = "--Select--";
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
            bindsem1();
            bindsec1();
            FbName1();
        }
        catch (Exception ex)
        {
        }
    }
    public void cbl_branch1_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            cbl_sem1.Items.Clear();
            int commcount = 0;
            cb_branch1.Checked = false;
            txt_branch1.Text = "--Select--";
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
            bindsem1();
            bindsec1();
            FbName1();
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
            if (cbl_degree1.Items.Count > 0)
            {
                for (int row = 0; row < cbl_degree1.Items.Count; row++)
                {
                    if (cbl_degree1.Items[row].Selected == true)
                    {
                        if (course_id == "")
                        {
                            course_id = Convert.ToString(cbl_degree1.Items[row].Value);
                        }
                        else
                        {
                            course_id = course_id + "," + Convert.ToString(cbl_degree1.Items[row].Value);
                        }
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
            string query = "";
            if (course_id != "")
            {
                ds.Clear();
                query = " select distinct degree.degree_code,department.dept_name,department.dept_code from degree,department,course where course.course_id=degree.course_id and department.dept_code=degree.dept_code and course.college_code = degree.college_code and department.college_code = degree.college_code and degree.course_id in(" + course_id + ") and degree.college_code in ('" + college + "')";
                ds = d2.select_method_wo_parameter(query, "Text");
                // ds = d2.BindBranchMultiple(singleuser, group_user, course_id, collegecode1, usercode);
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
    public void cb_sem1_CheckedChanged(object sender, EventArgs e)
    {
        try
        {
            int cout = 0;
            txt_sem1.Text = "--Select--";
            if (cb_sem1.Checked == true)
            {
                cout++;
                for (int i = 0; i < cbl_sem1.Items.Count; i++)
                {
                    cbl_sem1.Items[i].Selected = true;
                }
                txt_sem1.Text = "Semester(" + (cbl_sem1.Items.Count) + ")";
            }
            else
            {
                for (int i = 0; i < cbl_sem1.Items.Count; i++)
                {
                    cbl_sem1.Items[i].Selected = false;
                }
            }
            bindsec1();
            FbName1();
        }
        catch (Exception ex)
        {
        }
    }
    public void cbl_sem1_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            cb_sem1.Checked = false;
            int commcount = 0;
            txt_sem1.Text = "--Select--";
            for (int i = 0; i < cbl_sem1.Items.Count; i++)
            {
                if (cbl_sem1.Items[i].Selected == true)
                {
                    commcount = commcount + 1;
                    cb_sem1.Checked = false;
                }
            }
            if (commcount > 0)
            {
                if (commcount == cbl_sem1.Items.Count)
                {
                    cb_sem1.Checked = true;
                }
                txt_sem1.Text = "Semester(" + commcount.ToString() + ")";
            }
            bindsec1();
            FbName1();
        }
        catch (Exception ex)
        {
        }
    }
    public void bindsem1()
    {
        cbl_sem1.Items.Clear();
        txt_sem1.Text = "--Select--";
        Boolean first_year;
        first_year = false;
        int duration = 0;
        int i = 0;
        ds.Clear();
        string branch = "";
        string build = "";
        string batch = "";
        if (cbl_branch1.Items.Count > 0)
        {
            for (i = 0; i < cbl_branch1.Items.Count; i++)
            {
                if (cbl_branch1.Items[i].Selected == true)
                {
                    build = cbl_branch1.Items[i].Value.ToString();
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
        string college = "";
        if (Cbl_college1.Items.Count > 0)
        {
            for (int cl = 0; cl < Cbl_college1.Items.Count; cl++)
            {
                if (Cbl_college1.Items[cl].Selected == true)
                {
                    if (college == "")
                    {
                        college = Convert.ToString(Cbl_college1.Items[cl].Value);
                    }
                    else
                    {
                        college = college + "','" + Convert.ToString(Cbl_college1.Items[cl].Value);
                    }
                }
            }
        }
        build = "";
        if (cbl_batch1.Items.Count > 0)
        {
            for (i = 0; i < cbl_batch1.Items.Count; i++)
            {
                if (cbl_batch1.Items[i].Selected == true)
                {
                    build = cbl_batch1.Items[i].Value.ToString();
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
            string query = "select distinct Current_Semester from Registration where degree_code in (" + branch + ") and Batch_Year in (" + batch + ") and college_code in ('" + college + "')  and CC=0 and DelFlag =0 and Exam_Flag <>'debar' order by Current_Semester ";
            ds = d2.select_method_wo_parameter(query, "Text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                cbl_sem1.DataSource = ds;
                cbl_sem1.DataTextField = "Current_Semester";
                cbl_sem1.DataBind();
                if (cbl_sem1.Items.Count > 0)
                {
                    for (int row = 0; row < cbl_sem1.Items.Count; row++)
                    {
                        cbl_sem1.Items[row].Selected = true;
                        cb_sem1.Checked = true;
                    }
                    txt_sem1.Text = "Semester(" + cbl_sem1.Items.Count + ")";
                }
            }
        }
    }
    public void cb_sec1_CheckedChanged(object sender, EventArgs e)
    {
        try
        {
            int cout = 0;
            txt_sec1.Text = "--Select--";
            if (cb_sec1.Checked == true)
            {
                cout++;
                for (int i = 0; i < cbl_sec1.Items.Count; i++)
                {
                    cbl_sec1.Items[i].Selected = true;
                }
                txt_sec1.Text = "Semester(" + (cbl_sec1.Items.Count) + ")";
            }
            else
            {
                for (int i = 0; i < cbl_sec1.Items.Count; i++)
                {
                    cbl_sec1.Items[i].Selected = false;
                }
                txt_sec1.Text = "--Select--";
            }
        }
        catch (Exception ex)
        {
        }
        FbName1();
    }
    public void cbl_sec1_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            int commcount = 0;
            txt_sec1.Text = "--Select--";
            cb_sec1.Checked = false;
            for (int i = 0; i < cbl_sec1.Items.Count; i++)
            {
                if (cbl_sec1.Items[i].Selected == true)
                {
                    commcount = commcount + 1;
                    cb_sec1.Checked = false;
                }
            }
            if (commcount > 0)
            {
                if (commcount == cbl_sec1.Items.Count)
                {
                    cb_sec1.Checked = true;
                }
                txt_sec1.Text = "Section(" + commcount.ToString() + ")";
            }
            //bindhostelname();
        }
        catch (Exception ex)
        {
        }
        FbName1();
    }
    public void bindsec1()
    {
        try
        {
            cbl_sec1.Items.Clear();
            txt_sec1.Text = "---Select---";
            cb_sec1.Checked = false;
            string build = "";
            if (cbl_batch1.Items.Count > 0)
            {
                for (int i = 0; i < cbl_batch1.Items.Count; i++)
                {
                    if (cbl_batch1.Items[i].Selected == true)
                    {
                        if (build == "")
                        {
                            build = Convert.ToString(cbl_batch1.Items[i].Value);
                        }
                        else
                        {
                            build = build + "," + Convert.ToString(cbl_batch1.Items[i].Value);
                        }
                    }
                }
            }
            string branchcode = "";
            if (cbl_branch1.Items.Count > 0)
            {
                for (int i = 0; i < cbl_branch1.Items.Count; i++)
                {
                    if (cbl_branch1.Items[i].Selected == true)
                    {
                        if (branchcode == "")
                        {
                            branchcode = Convert.ToString(cbl_branch1.Items[i].Value);
                        }
                        else
                        {
                            branchcode = branchcode + "," + Convert.ToString(cbl_branch1.Items[i].Value);
                        }
                    }
                }
            }
            if (build != "" || branchcode != "")
            {
                ds.Clear();
                ds = d2.BindSectionDetail(build, branchcode);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    cbl_sec1.DataSource = ds;
                    cbl_sec1.DataTextField = "sections";
                    cbl_sec1.DataValueField = "sections";
                    cbl_sec1.DataBind();
                    if (cbl_sec1.Items.Count > 0)
                    {
                        for (int row = 0; row < cbl_sec1.Items.Count; row++)
                        {
                            cbl_sec1.Items[row].Selected = true;
                            cb_sec1.Checked = true;
                        }
                        txt_sec1.Text = "Section(" + cbl_sec1.Items.Count + ")";
                    }
                    else
                    {
                        ListItem lst = new ListItem("Empty", " ");
                        cbl_sec1.Items.Add(lst);
                        for (int row = 0; row < cbl_sec1.Items.Count; row++)
                        {
                            cbl_sec1.Items[row].Selected = true;
                        }
                        txt_sec1.Text = "Section(" + cbl_sec1.Items.Count + ")";
                    }
                }
                else
                {
                    cbl_sec1.Items.Add("Empty");
                    for (int row = 0; row < cbl_sec1.Items.Count; row++)
                    {
                        cbl_sec1.Items[row].Selected = true;
                    }
                    txt_sec1.Text = "Section(" + cbl_sec1.Items.Count + ")";
                }
            }
            else
            {
                cb_sec1.Checked = false;
                txt_sec1.Text = "--Select--";
            }
        }
        catch (Exception ex)
        {
        }
        FbName1();
    }
    public void btndel_Click(object sender, EventArgs e)
    {
        imgdiv3.Visible = true;
        lbl_warning_alert.Visible = true;
        lbl_warning_alert.Text = "Are You Sure You Want Delete?";
    }
    public void btn_warning_exit_Click(object sender, EventArgs e)
    {
        imgdiv3.Visible = false;
    }
    public void btn_warningmsg_Click(object sender, EventArgs e)
    {
        imgdiv3.Visible = false;
        lbl_warning_alert.Visible = false;
        string question = txt_FBName.Text;
        string activerow = FpSpread1.ActiveSheetView.ActiveRow.ToString();
        string value = FpSpread1.Sheets[0].Cells[Convert.ToInt32(activerow), 1].Tag.ToString();
        string feedback = "";
        if (rb_Acad1.Checked == true)
        {
            feedback = FpSpread1.Sheets[0].Cells[Convert.ToInt32(activerow), 7].Text.ToString();
        }
        else
        {
            feedback = FpSpread1.Sheets[0].Cells[Convert.ToInt32(activerow), 2].Text.ToString();
        }
        if (txt_FBName.Text != "")
        {
            string questionpk = d2.GetFunction("select FeedBackMasterFK from CO_StudFeedBack where FeedBackMasterFK ='" + value + "'");
            //string questionpk1 = d2.GetFunction("select distinct FeedBackMasterFK from CO_FeedBackQuestions where FeedBackMasterFK ='" + value + "'");
            if (questionpk != value)
            {
                int qry = 0;
                if (rb_induvgual.Checked == true)
                {
                    string sql = "delete  from CO_FeedBackMaster where  FeedBackMasterPK = '" + value + "' and collegecode='" + collegecode1 + "'";
                    sql = sql + (" delete  from CO_FeedbackUniCode where  FeedbackMasterFK = '" + value + "'");
                    qry = d2.update_method_wo_parameter(sql, "Text");
                }
                else if (rb_common.Checked == true)
                {
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
                    string build = "";
                    if (cbl_batch1.Items.Count > 0)
                    {
                        for (int i = 0; i < cbl_batch1.Items.Count; i++)
                        {
                            if (cbl_batch1.Items[i].Selected == true)
                            {
                                if (build == "")
                                {
                                    build = Convert.ToString(cbl_batch1.Items[i].Value);
                                }
                                else
                                {
                                    build = build + "','" + Convert.ToString(cbl_batch1.Items[i].Value);
                                }
                            }
                        }
                    }
                    string branchcode = "";
                    if (cbl_branch1.Items.Count > 0)
                    {
                        for (int i = 0; i < cbl_branch1.Items.Count; i++)
                        {
                            if (cbl_branch1.Items[i].Selected == true)
                            {
                                if (branchcode == "")
                                {
                                    branchcode = Convert.ToString(cbl_branch1.Items[i].Value);
                                }
                                else
                                {
                                    branchcode = branchcode + "','" + Convert.ToString(cbl_branch1.Items[i].Value);
                                }
                            }
                        }
                    }
                    string sem = "";
                    if (cbl_sem1.Items.Count > 0)
                    {
                        for (int i = 0; i < cbl_sem1.Items.Count; i++)
                        {
                            if (cbl_sem1.Items[i].Selected == true)
                            {
                                if (sem == "")
                                {
                                    sem = Convert.ToString(cbl_sem1.Items[i].Value);
                                }
                                else
                                {
                                    sem = sem + "','" + Convert.ToString(cbl_sem1.Items[i].Value);
                                }
                            }
                        }
                    }
                    string section = "";
                    if (cbl_sec1.Items.Count > 0)
                    {
                        for (int i = 0; i < cbl_sec1.Items.Count; i++)
                        {
                            if (cbl_sec1.Items[i].Selected == true)
                            {
                                if (section == "")
                                {
                                    section = Convert.ToString(cbl_sec1.Items[i].Value);
                                }
                                else
                                {
                                    section = section + "','" + Convert.ToString(cbl_sec1.Items[i].Value);
                                }
                                if (cbl_sec1.Items[i].Value == "Empty")
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
                    string sql = "delete  from CO_FeedBackMaster where  FeedBackName = '" + feedback + "' and collegecode in('" + collegecode1 + "') and Batch_Year in('" + build + "') and DegreeCode in('" + branchcode + "') and Section  in('" + section + "') and semester in('" + sem + "')";
                    sql = sql + (" delete  from CO_FeedbackUniCode where  FeedbackMasterFK = '" + value + "'");
                    qry = d2.update_method_wo_parameter(sql, "Text");
                }
                if (qry != 0)
                {
                    imgdiv2.Visible = true;
                    lbl_alert1.Visible = true;
                    lbl_alert1.Text = "Deleted Successfully";
                }
                txt_FBName.Text = "";
                Add_FeedBack.Visible = false;
                FbName1();
                btn_Search_Click(sender, e);
            }
            else
            {
                imgdiv2.Visible = true;
                lbl_alert1.Visible = true;
                lbl_alert1.Text = " Sorry You Cannot Able to Delete ";
                Add_FeedBack.Visible = false;
            }
        }
    }
    protected void FpSpread1_OnCellClick(object sender, EventArgs e)
    {
        string activerow = FpSpread1.ActiveSheetView.ActiveRow.ToString();
        string activecol = FpSpread1.ActiveSheetView.ActiveColumn.ToString();
        cellclick = true;
        Add_FeedBack.Visible = true;
        //lbl_Subject_Type.Visible = false;
        //Txt_Subjecttype.Visible = false;
        //Panel_Subjecttype.Visible = false;
        //  FpSpread1_OnButtonCommand(sender,e);
    }
    //string questionpk = d2.GetFunction("select FeedBackMasterFK from CO_StudFeedBack where FeedBackMasterFK ='" + value + "'");
    protected void FpSpread1_OnButtonCommand(object sender, EventArgs e)
    {
        try
        {
            if (cellclick == true)
            {
                // Txt_Subjecttype.Enabled = false;
                Txt_Subjecttype.Enabled = true;
                visiblefalse();
                btn_Save.Text = "Update";
                rb_induvgual.Visible = true;
                rb_common.Visible = true;
                rb_induvgual.Checked = true;
                rb_common.Checked = false;
                txt_total_strength.Text = "";
                btndel.Visible = true;
                chk_random.Visible = false;//Added By Saranyadevi 20.2.2018
              
                string activerow = FpSpread1.ActiveSheetView.ActiveRow.ToString();
                string activecol = FpSpread1.ActiveSheetView.ActiveColumn.ToString();
                string value = FpSpread1.Sheets[0].Cells[Convert.ToInt32(activerow), 0].Tag.ToString();
                btn_creatxl.Visible = false;
                btn_errorclose.Visible = true;
                if (value == "Academic")
                {
                    rb_Acad.Checked = true;
                    rb_Gend.Checked = false;
                    rb_Gend.Enabled = false;
                    Add_FeedBack.Visible = true;
                    string includecomman = FpSpread1.Sheets[0].Cells[Convert.ToInt32(activerow), 10].Text.ToString();
                    if (includecomman == "YES")
                    {
                        chk_includecommon.Checked = false;
                        //chk_includecommon.Enabled = false;//added by Saranyadevi20.2.2018
                        chk_random.Checked = false;
                        lbl_totl_strength.Visible = false;
                        txt_total_strength.Visible = false;
                        lbl_fb_acr.Visible = false;
                        txt_fb_acr.Visible = false;
                        lbl_run_sries.Visible = false;
                        txt_running_series.Visible = false;
                        txt_total_strength.Text = "";
                        txt_fb_acr.Text = "";
                        txt_running_series.Text = "";
                    }
                    else if (includecomman == "NO")
                    {
                        chk_includecommon.Checked = false;
                        chk_includecommon.Enabled = true;
                        chk_random.Checked = false;//Added By Saranyadevi 20.2.2018
                        chk_random.Enabled = true;

                    }
                    string pk = FpSpread1.Sheets[0].Cells[Convert.ToInt32(activerow), 1].Tag.ToString();
                    string fbfk = d2.GetFunction("select FeedbackMasterFK from CO_FeedbackUniCode where  FeedbackMasterFK in('" + pk + "')");
                    if (fbfk == "0")
                    {
                        chk_includecommon.Enabled = true;
                    }
                    else
                    {
                        chk_includecommon.Enabled = false;
                        includecommon();
                    }
                    string FBName = FpSpread1.Sheets[0].Cells[Convert.ToInt32(activerow), 7].Text.ToString();
                    txt_FBName.Text = FBName;
                    txt_FBName.Enabled = false;
                    string startdate = FpSpread1.Sheets[0].Cells[Convert.ToInt32(activerow), 8].Text;
                    string Enddate = FpSpread1.Sheets[0].Cells[Convert.ToInt32(activerow), 9].Text;
                    txt_fromdate.Text = startdate.ToString();
                    txt_Enddate.Text = Enddate.ToString();
                    BindBatch();
                    string year = FpSpread1.Sheets[0].Cells[Convert.ToInt32(activerow), 1].Note;
                    int count1 = 0;
                    for (int i = 0; i < cbl_batch.Items.Count; i++)
                    {
                        cbl_batch.Items[i].Selected = false;
                    }
                    for (int i = 0; i < cbl_batch.Items.Count; i++)
                    {
                        if (cbl_batch.Items[i].Value.ToString() == year)
                        {
                            cbl_batch.Items[i].Selected = true;
                            cb_batch.Checked = false;
                            count1 = count1 + 1;
                        }
                        txt_batch.Text = "Batch(" + count1.ToString() + ")";
                    }
                    //
                    bindddlclg();
                    BindDegree();
                    string dept = FpSpread1.Sheets[0].Cells[Convert.ToInt32(activerow), 9].Tag.ToString();
                    int total = 0;
                    for (int i = 0; i < cbl_degree.Items.Count; i++)
                    {
                        cbl_degree.Items[i].Selected = false;
                    }
                    for (int i = 0; i < cbl_degree.Items.Count; i++)
                    {
                        if (cbl_degree.Items[i].Value.ToString() == dept)
                        {
                            cbl_degree.Items[i].Selected = true;
                            cb_degree.Checked = false;
                            total = total + 1;
                        }
                        txt_degree.Text = "degree(" + total.ToString() + ")";
                    }
                    bindbranch();
                    string branch = FpSpread1.Sheets[0].Cells[Convert.ToInt32(activerow), 2].Note;
                    int total1 = 0;
                    for (int i = 0; i < cbl_branch.Items.Count; i++)
                    {
                        cbl_branch.Items[i].Selected = false;
                    }
                    for (int i = 0; i < cbl_branch.Items.Count; i++)
                    {
                        if (cbl_branch.Items[i].Value.ToString() == branch)
                        {
                            cbl_branch.Items[i].Selected = true;
                            cb_branch.Checked = false;
                            total1 = total1 + 1;
                        }
                        txt_branch.Text = "Branch(" + total1.ToString() + ")";
                    }
                    bindsem();
                    string sem = FpSpread1.Sheets[0].Cells[Convert.ToInt32(activerow), 3].Note;
                    int count = 0;
                    for (int i = 0; i < cbl_sem.Items.Count; i++)
                    {
                        cbl_sem.Items[i].Selected = false;
                    }
                    for (int i = 0; i < cbl_sem.Items.Count; i++)
                    {
                        if (cbl_sem.Items[i].Value.ToString() == sem)
                        {
                            cbl_sem.Items[i].Selected = true;
                            cb_sem.Checked = false;
                            count = count + 1;
                        }
                        txt_sem.Text = "Semester(" + count.ToString() + ")";
                    }
                    bindsec();
                    string Section = FpSpread1.Sheets[0].Cells[Convert.ToInt32(activerow), 4].Note;
                    int commcount = 0;
                    for (int i = 0; i < cbl_sec.Items.Count; i++)
                    {
                        cbl_sec.Items[i].Selected = false;
                    }
                    for (int i = 0; i < cbl_sec.Items.Count; i++)
                    {
                        if (cbl_sec.Items[i].Value.ToString() == Section)
                        {
                            cbl_sec.Items[i].Selected = true;
                            cb_sec.Checked = false;
                            commcount = commcount + 1;
                        }
                        txt_sec.Text = "Section(" + commcount.ToString() + ")";
                    }
                    string subtype = FpSpread1.Sheets[0].Cells[Convert.ToInt32(activerow), 11].Text;
                    string st_type = "";
                    string sub_type = "";
                    Cbl_Subjecttype.Items.Clear();
                    Txt_Subjecttype.Text = "--Select--";
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
                    string stafftype = "select distinct subject_type from sub_sem where subject_type in('" + sub_type + "') order by subject_type ";
                    ds = d2.select_method_wo_parameter(stafftype, "Text");
                    if (ds.Tables.Count > 0)
                    {
                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            Cbl_Subjecttype.DataSource = ds;
                            Cbl_Subjecttype.DataTextField = "subject_type";
                            Cbl_Subjecttype.DataValueField = "subject_type";
                            Cbl_Subjecttype.DataBind();
                            if (Cbl_Subjecttype.Items.Count > 0)
                            {
                                for (int i = 0; i < Cbl_Subjecttype.Items.Count; i++)
                                {
                                    Cbl_Subjecttype.Items[i].Selected = true;
                                }
                                Txt_Subjecttype.Text = "Subject Type(" + Cbl_Subjecttype.Items.Count + ")";
                                Cb_Subjecttype.Checked = true;
                            }
                        }
                    }
                    string subtype1 = FpSpread1.Sheets[0].Cells[Convert.ToInt32(activerow), 12].Text;
                    string st_type1 = "";
                    string sub_typeop = "";
                    Cbl_Subjecttype1.Items.Clear();
                    Txt_Subjecttype1.Text = "--Select--";
                    Cb_Subjecttype1.Checked = false;
                    if (subtype1 != "-")
                    {
                        if (subtype1 != "")
                        {
                            st_type1 = subtype1.ToString();
                            string[] split = st_type1.Split(',');
                            for (int i = 0; i < split.Length; i++)
                            {
                                if (sub_typeop == "")
                                {
                                    sub_typeop = split[i];
                                }
                                else
                                {
                                    sub_typeop += "','" + split[i];
                                }
                            }
                        }
                        string stafftype1 = "select distinct subject_type from sub_sem where subject_type in('" + sub_typeop + "') order by subject_type ";
                        ds = d2.select_method_wo_parameter(stafftype1, "Text");
                        if (ds.Tables.Count > 0)
                        {
                            if (ds.Tables[0].Rows.Count > 0)
                            {
                                Cbl_Subjecttype1.DataSource = ds;
                                Cbl_Subjecttype1.DataTextField = "subject_type";
                                Cbl_Subjecttype1.DataValueField = "subject_type";
                                Cbl_Subjecttype1.DataBind();
                                if (Cbl_Subjecttype1.Items.Count > 0)
                                {
                                    for (int i = 0; i < Cbl_Subjecttype1.Items.Count; i++)
                                    {
                                        Cbl_Subjecttype1.Items[i].Selected = true;
                                    }
                                    Txt_Subjecttype1.Text = "Subject Type(" + Cbl_Subjecttype1.Items.Count + ")";
                                    Cb_Subjecttype1.Checked = true;
                                }
                            }
                        }
                    }
                    string acdamic_isgeneral = FpSpread1.Sheets[0].Cells[Convert.ToInt32(activerow), 13].Text.ToString();
                    if (acdamic_isgeneral.Trim().ToLower() == "yes")
                    {
                        cb_IsGeneral.Checked = true;
                    }
                    else
                    {
                        cb_IsGeneral.Checked = false;
                    }
                    cb_IsGeneral_CheckedChanged(sender, e);
                    string IsType_Individual = FpSpread1.Sheets[0].Cells[Convert.ToInt32(activerow), 14].Text.ToString();
                    if (IsType_Individual.Trim().ToLower() == "yes")
                    {
                        cb_Typeindiviual.Checked = true;
                    }
                    else
                    {
                        cb_Typeindiviual.Checked = false;
                    }
                }
                else if (value == "General")
                {
                    rb_Gend.Checked = true;
                    rb_Acad.Checked = false;
                    rb_Acad.Enabled = false;
                    Add_FeedBack.Visible = true;
                    string includecomman = FpSpread1.Sheets[0].Cells[Convert.ToInt32(activerow), 5].Text.ToString();
                    if (includecomman == "YES")
                    {
                        chk_includecommon.Checked = true;
                    }
                    else if (includecomman == "NO")
                    {
                        chk_includecommon.Checked = false;
                    }
                    string FBName = FpSpread1.Sheets[0].Cells[Convert.ToInt32(activerow), 2].Text.ToString();
                    txt_FBName.Text = FBName;
                    string startdate = FpSpread1.Sheets[0].Cells[Convert.ToInt32(activerow), 3].Text;
                    string Enddate = FpSpread1.Sheets[0].Cells[Convert.ToInt32(activerow), 4].Text;
                    txt_fromdate.Text = startdate.ToString();
                    txt_Enddate.Text = Enddate.ToString();
                    string college = FpSpread1.Sheets[0].Cells[Convert.ToInt32(activerow), 0].Note;
                    for (int i = 0; i < ddl_college.Items.Count; i++)
                    {
                        if (ddl_college.Items[i].Value.ToString() == college)
                        {
                            ddl_college.SelectedItem.Value = college;
                        }
                    }
                }
                chk_includecommon.Enabled = false;//Added By Saranyadevi 20.2.2018
            }
        }
        catch
        {
        }
    }
    protected void btn_Add_Click(object sender, EventArgs e)
    {
        rb_induvgual.Visible = false;
        rb_common.Visible = false;
        Txt_Subjecttype.Enabled = true;
        Add_FeedBack.Visible = true;
        chk_random.Visible = false;//Added By Saranyadevi 19.2.2018
        visibletr();
        btndel.Visible = false;
        btn_Save.Text = "Save";
        bindddlclg();
        BindBatch();
        BindDegree();
        bindbranch();
        bindsem();
        bindsec();
        txt_FBName.Text = "";
        txt_fromdate.Text = DateTime.Now.ToString("dd/MM/yyyy");
        txt_Enddate.Text = DateTime.Now.ToString("dd/MM/yyyy");
        chk_includecommon.Checked = false;
        rb_Gend.Checked = false;
        rb_Acad.Checked = true;
        rb_Acad.Enabled = true;
        rb_Gend.Enabled = true;
        lbl_Subject_Type.Visible = true;
        Txt_Subjecttype.Visible = true;
        Panel_Subjecttype.Visible = true;
        lbl_totl_strength.Visible = false;
        txt_total_strength.Visible = false;
        lbl_fb_acr.Visible = false;
        txt_fb_acr.Visible = false;
        lbl_run_sries.Visible = false;
        txt_running_series.Visible = false;
        txt_total_strength.Text = "";
        txt_fb_acr.Text = "";
        txt_running_series.Text = "";
        chk_includecommon.Enabled = true;
    }
    protected void lb3_Click(object sender, EventArgs e)
    {
        Session.Abandon();
        Session.Clear();
        Session.RemoveAll();
        System.Web.Security.FormsAuthentication.SignOut();
        Response.Redirect("~/Default.aspx", false);
    }
    protected void imagebtnpopclose1_Click(object sender, EventArgs e)
    {
        Add_FeedBack.Visible = false;
    }
    protected void btn_errorclose_Click(object sender, EventArgs e)
    {
        imgdiv2.Visible = false;
    }
    protected void btn_save_Click(object sender, EventArgs e)
    {
        string includecmn = "";
        lbl_firstpk.Text = "";
        if (chk_includecommon.Checked == true)
        {
            includecmn = "1";
        }
        else if (chk_includecommon.Checked == false)
        {
            includecmn = "0";
        }
        if (btn_Save.Text.Trim().ToUpper() == "SAVE")
        {
            string SubjectType = "";
            for (int sub = 0; sub < Cbl_Subjecttype.Items.Count; sub++)
            {
                if (Cbl_Subjecttype.Items[sub].Selected == true)
                {
                    if (SubjectType == "")
                    {
                        SubjectType = "" + Cbl_Subjecttype.Items[sub].Value.ToString() + "";
                    }
                    else
                    {
                        SubjectType = SubjectType + "," + Cbl_Subjecttype.Items[sub].Value.ToString() + "";
                    }
                }
            }
            string optional = "";
            if (cb_optional.Checked == true)
            {
                for (int sub = 0; sub < Cbl_Subjecttype1.Items.Count; sub++)
                {
                    if (Cbl_Subjecttype1.Items[sub].Selected == true)
                    {
                        if (optional == "")
                        {
                            optional = "" + Cbl_Subjecttype1.Items[sub].Value.ToString() + "";
                        }
                        else
                        {
                            optional = optional + "," + Cbl_Subjecttype1.Items[sub].Value.ToString() + "";
                        }
                    }
                }
            }
            string semester = "";
            if (rb_Acad.Checked == true)
            {
                int acadamic_isgeneral = 0;
                int IsType_Individual = 0;
                int Subjectwise = 0;
                if (cb_IsGeneral.Checked)
                    acadamic_isgeneral = 1;
                if (cb_Typeindiviual.Checked)
                    IsType_Individual = 1;
                if (cb_Subjectwise.Checked)
                    Subjectwise = 1;

                string first_uniqfk = d2.GetFunction(" select  top 1 FeedbackUnicodePK from CO_FeedbackUniCode order by   FeedbackUnicodePK desc");
                if (first_uniqfk == "")
                {
                    first_uniqfk = "0";
                }
                int unqid = Convert.ToInt32(first_uniqfk);
                unqid = unqid + 1;
                first_uniqfk = Convert.ToString(unqid);
                lbl_firstpk.Text = first_uniqfk;
                if (SubjectType == " ")
                {
                    imgdiv2.Visible = true;
                    lbl_alert1.Visible = true;
                    lbl_alert1.Text = "Please Select Subject Type";
                    return;
                }
                int runningseries = 0;
                //10.08.16
                int sercount = 0;
                if (txt_running_series.Text.Trim() != "")
                {
                    string seriessize = Convert.ToString(txt_running_series.Text);
                    for (int m = 0; m < seriessize.Length; m++)
                    {
                        if (seriessize.Substring(m).StartsWith("0") == true)
                        {
                            sercount++;
                        }
                        else
                        {
                            goto series;
                        }
                    }
                }
            series:
                int.TryParse(Convert.ToString(txt_running_series.Text), out runningseries);
                string acr = txt_fb_acr.Text.ToString();
                if (acr != "")
                {
                    string acroname = d2.GetFunction(" select UnicodeAcr from CO_FeedbackUniCode  where UnicodeAcr ='" + acr + "'");
                    if (acroname.Trim() != "0")
                    {
                        imgdiv2.Visible = true;
                        lbl_alert1.Visible = true;
                        lbl_alert1.Text = "This Acronames Already Existed";
                        return;
                    }
                }
                //string s = i.ToString().PadLeft(40, '0');
                string college_cd = " ";
                college_cd = "" + ddl_college.SelectedItem.Value.ToString() + "";
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
                //string semester = "";
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
                string section_h = "";
                ArrayList sectionarray = new ArrayList();
                for (int i = 0; i < cbl_sec.Items.Count; i++)
                {
                    if (cbl_sec.Items[i].Selected == true)
                    {
                        if (section_h == "")
                        {
                            section_h = "" + cbl_sec.Items[i].Value.ToString() + "";
                            sectionarray.Add(cbl_sec.Items[i].Value.ToString());
                        }
                        else
                        {
                            section_h = section_h + "','" + cbl_sec.Items[i].Value.ToString() + "";
                            sectionarray.Add(cbl_sec.Items[i].Value.ToString());
                        }
                    }
                }
                int ins = 0;
                int unqgend = 0;
                if (college_cd != "" && degree_code != "" && Batch_Year != "" && semester != "")
                {
                    string type = "1";
                    try
                    {
                        for (int i = 0; i < cbl_batch.Items.Count; i++)
                        {
                            if (cbl_batch.Items[i].Selected == true)
                            {
                                for (int j = 0; j < cbl_branch.Items.Count; j++)
                                {
                                    if (cbl_branch.Items[j].Selected == true)
                                    {
                                        for (int l = 0; l < cbl_sem.Items.Count; l++)
                                        {
                                            if (cbl_sem.Items[l].Selected == true)
                                            {
                                                ds.Clear();
                                                string sectn = "select distinct sections from registration where batch_year in('" + cbl_batch.Items[i].Text + "') and degree_code in('" + cbl_branch.Items[j].Value + "') and sections<>'-1' and ltrim(sections)<>'' and sections is not null and delflag=0 and exam_flag<>'Debar' and Current_Semester ='" + cbl_sem.Items[l].Text + "'";
                                                ds = d2.select_method_wo_parameter(sectn, "Text");
                                                string firstdate = Convert.ToString(txt_fromdate.Text);
                                                string seconddate = Convert.ToString(txt_Enddate.Text);
                                                DateTime dt = new DateTime();
                                                DateTime dt1 = new DateTime();
                                                string[] split = firstdate.Split('/');
                                                dt = Convert.ToDateTime(split[1] + "/" + split[0] + "/" + split[2]);
                                                split = seconddate.Split('/');
                                                dt1 = Convert.ToDateTime(split[1] + "/" + split[0] + "/" + split[2]);
                                                string acroname = "";
                                                if (ds.Tables[0].Rows.Count > 0)
                                                {
                                                    for (int m = 0; m < ds.Tables[0].Rows.Count; m++)
                                                    {
                                                        if (sectionarray.Contains(Convert.ToString(ds.Tables[0].Rows[m][0])))
                                                        {
                                                            string getquery = d2.GetFunction(" select COUNT(r.App_No) from Registration r,applyn a where r.App_No =a.app_no and  r.Batch_Year ='" + cbl_batch.Items[i].Text + "' and r.Current_Semester ='" + cbl_sem.Items[l].Text + "' and r.degree_code in('" + cbl_branch.Items[j].Value + "') and CC=0 and DelFlag =0 and Exam_Flag <>'debar' and Sections in('" + Convert.ToString(ds.Tables[0].Rows[m][0]) + "')");

                                                            string FeedBackName = txt_FBName.Text;
                                                            FeedBackName = System.Threading.Thread.CurrentThread.CurrentCulture.TextInfo.ToTitleCase(FeedBackName);
                                                            string insert = "insert into CO_FeedBackMaster (FeedBackType,Batch_Year,DegreeCode,Section,semester,FeedBackName,StartDate,EndDate,CollegeCode,InclueCommon,Subject_Type,OptionalSubject_type,Acadamic_Isgeneral,IsType_Individual,IsSubjectType) values ('" + type + "','" + cbl_batch.Items[i].Text + "','" + cbl_branch.Items[j].Value + "','" + Convert.ToString(ds.Tables[0].Rows[m][0]) + "','" + cbl_sem.Items[l].Text + "','" + FeedBackName + "','" + dt.ToString("MM/dd/yyyy") + "','" + dt1.ToString("MM/dd/yyyy") + "','" + ddl_college.SelectedItem.Value + "','" + includecmn + "','" + SubjectType + "','" + optional + "','" + acadamic_isgeneral + "','" + IsType_Individual + "','" + Subjectwise + "')";
                                                            ins = d2.update_method_wo_parameter(insert, "Text");
                                                            if (getquery != "" && getquery != "0")
                                                            {
                                                                //Added By SaranyaDevi 19.2.2018
                                                                if (chk_includecommon.Checked == true && chk_random.Checked == true)
                                                                {
                                                                    
                                                                    List<int> Number = new List<int>();
                                                                    Number = RandomFunction(Convert.ToInt32(getquery));
                                                                    for (int n = 0; n < Number.Count; n++)
                                                                    {
                                                                        int uniquecode = Number[n];
                                                                        string feedbackfk = d2.GetFunction(" select  top 1 FeedBackMasterPK from CO_FeedBackMaster order by   FeedBackMasterPK desc");
                                                                        string insertuniq = "insert into CO_FeedbackUniCode (FeedbackUnicode,FeedbackMasterFK,IsCheckFlag) values ('" + uniquecode + "','" + feedbackfk + "','0')";
                                                                        unqgend = d2.update_method_wo_parameter(insertuniq, "Text");
                                                                    }
                                                                }
                                                                if (chk_includecommon.Checked == true && chk_random.Checked == false)
                                                                {
                                                                    acroname = txt_fb_acr.Text.ToUpper();
                                                                    for (int unq = 0; unq < Convert.ToInt32(getquery); unq++)
                                                                    {
                                                                        string acr_rnseries = acroname + Convert.ToString(runningseries);
                                                                        string uniquecode = acroname + runningseries.ToString().PadLeft(sercount + Convert.ToInt32(Convert.ToString(runningseries).Length), '0');
                                                                        string feedbackfk = d2.GetFunction(" select  top 1 FeedBackMasterPK from CO_FeedBackMaster order by   FeedBackMasterPK desc");
                                                                        string insertuniq = "insert into CO_FeedbackUniCode (FeedbackUnicode,FeedbackMasterFK,IsCheckFlag,UnicodeAcr,UniCodeStartNO) values ('" + uniquecode + "','" + feedbackfk + "','0','" + txt_fb_acr.Text.ToString().ToUpper() + "','" + txt_running_series.Text + "')";
                                                                        unqgend = d2.update_method_wo_parameter(insertuniq, "Text");
                                                                        runningseries = runningseries + 1;
                                                                    }
                                                                }
                                                            }
                                                        }
                                                    }
                                                }
                                                else
                                                {
                                                    string getquery = d2.GetFunction(" select COUNT(r.App_No) from Registration r,applyn a where r.App_No =a.app_no and  r.Batch_Year ='" + cbl_batch.Items[i].Text + "' and r.Current_Semester ='" + cbl_sem.Items[l].Text + "' and r.degree_code in('" + cbl_branch.Items[j].Value + "') and CC=0 and DelFlag =0 and Exam_Flag <>'debar' and  isnull(sections,'')=''");//and Sections in('" + Convert.ToString(ds.Tables[0].Rows[m][0]) + "')
                                                    if (getquery != "" && getquery != "0")
                                                    {
                                                        string FeedBackName = txt_FBName.Text;
                                                        FeedBackName = System.Threading.Thread.CurrentThread.CurrentCulture.TextInfo.ToTitleCase(FeedBackName);
                                                        string insert = "insert into CO_FeedBackMaster (FeedBackType,Batch_Year,DegreeCode,Section,semester,FeedBackName,StartDate,EndDate,CollegeCode,InclueCommon,Subject_Type,OptionalSubject_type,acadamic_isgeneral,IsType_Individual,IsSubjectType) values ('" + type + "','" + cbl_batch.Items[i].Text + "','" + cbl_branch.Items[j].Value + "','','" + cbl_sem.Items[l].Text + "','" + FeedBackName + "','" + dt.ToString("MM/dd/yyyy") + "','" + dt1.ToString("MM/dd/yyyy") + "','" + ddl_college.SelectedItem.Value + "','" + includecmn + "','" + SubjectType + "','" + optional + "','" + acadamic_isgeneral + "','" + IsType_Individual + "','" + Subjectwise + "')";
                                                        ins = d2.update_method_wo_parameter(insert, "Text");
                                                        if (chk_includecommon.Checked == true && chk_random.Checked == false)
                                                        {
                                                            acroname = txt_fb_acr.Text.ToUpper();
                                                            for (int unq = 0; unq < Convert.ToInt32(getquery); unq++)
                                                            {
                                                                string acr_rnseries = acroname + Convert.ToString(runningseries);
                                                                string uniquecode = acroname + runningseries.ToString().PadLeft(sercount + Convert.ToInt32(Convert.ToString(runningseries).Length), '0');
                                                                string feedbackfk = d2.GetFunction(" select  top 1 FeedBackMasterPK from CO_FeedBackMaster order by   FeedBackMasterPK desc");
                                                                string insertuniq = "insert into CO_FeedbackUniCode (FeedbackUnicode,FeedbackMasterFK,IsCheckFlag,UnicodeAcr,UniCodeStartNO) values ('" + uniquecode + "','" + feedbackfk + "','0','" + txt_fb_acr.Text.ToString().ToUpper() + "','" + txt_running_series.Text + "')";
                                                                unqgend = d2.update_method_wo_parameter(insertuniq, "Text");
                                                                runningseries = runningseries + 1;
                                                            }
                                                        }
                                                        //Added By Saranyadevi19.2.2018
                                                        if (chk_includecommon.Checked == true && chk_random.Checked == true)
                                                        {
                                                            List<int> Number = new List<int>();
                                                            Number = RandomFunction(Convert.ToInt32(getquery));
                                                            for (int n = 0; n < Number.Count; n++)
                                                            {
                                                                int uniquecode = Number[n];
                                                                string feedbackfk = d2.GetFunction(" select  top 1 FeedBackMasterPK from CO_FeedBackMaster order by   FeedBackMasterPK desc");
                                                                string insertuniq = "insert into CO_FeedbackUniCode (FeedbackUnicode,FeedbackMasterFK,IsCheckFlag) values ('" + uniquecode + "','" + feedbackfk + "','0')";
                                                                unqgend = d2.update_method_wo_parameter(insertuniq, "Text");
                                                            }
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                        if (ins != 0)
                        {
                            imgdiv2.Visible = true;
                            lbl_alert1.Visible = true;
                            lbl_alert1.Text = "Saved successfully";
                            if (unqgend != 0)
                            {
                                btn_creatxl.Visible = true;
                                btn_errorclose.Visible = false;
                            }
                            bindddlclg();
                            BindBatch();
                            BindDegree();
                            bindbranch();
                            bindsem();
                            bindsec();
                            txt_FBName.Text = "";
                            txt_fromdate.Text = DateTime.Now.ToString("dd/MM/yyyy");
                            txt_Enddate.Text = DateTime.Now.ToString("dd/MM/yyyy");
                            rb_Acad.Checked = true;
                        }
                    }
                    catch
                    {
                    }
                }
                else
                {
                    if (semester == "")
                    {
                        imgdiv2.Visible = true;
                        lbl_alert1.Visible = true;
                        lbl_alert1.Text = "Please Update  Semester ";
                    }
                    else
                    {
                        imgdiv2.Visible = true;
                        lbl_alert1.Visible = true;
                        lbl_alert1.Text = "Please Select All Fields ";
                    }
                }
            }
            else if (rb_Gend.Checked == true)
            {
                string type = "2";
                string FeedBackName = txt_FBName.Text;
                FeedBackName = System.Threading.Thread.CurrentThread.CurrentCulture.TextInfo.ToTitleCase(FeedBackName);
                string firstdate = Convert.ToString(txt_fromdate.Text);
                string seconddate = Convert.ToString(txt_Enddate.Text);
                DateTime dt = new DateTime();
                DateTime dt1 = new DateTime();
                string[] split = firstdate.Split('/');
                dt = Convert.ToDateTime(split[1] + "/" + split[0] + "/" + split[2]);
                split = seconddate.Split('/');
                dt1 = Convert.ToDateTime(split[1] + "/" + split[0] + "/" + split[2]);
                string insert = "insert into CO_FeedBackMaster (FeedBackType,FeedBackName,StartDate,EndDate,CollegeCode,InclueCommon) values ('" + type + "','" + FeedBackName + "','" + dt.ToString("MM/dd/yyyy") + "','" + dt1.ToString("MM/dd/yyyy") + "','" + ddl_college.SelectedItem.Value + "','" + includecmn + "')";
                int ins = d2.update_method_wo_parameter(insert, "Text");
                imgdiv2.Visible = true;
                lbl_alert1.Visible = true;
                lbl_alert1.Text = "Saved successfully";
                bindddlclg();
                BindBatch();
                BindDegree();
                bindbranch();
                bindsem();
                bindsec();
                txt_FBName.Text = "";
                txt_fromdate.Text = DateTime.Now.ToString("dd/MM/yyyy");
                txt_Enddate.Text = DateTime.Now.ToString("dd/MM/yyyy");
                chk_includecommon.Checked = false;
                rb_Gend.Checked = false;
                rb_Acad.Checked = true;
                visibletr();
            }
            FbName1();
        }
        else if (btn_Save.Text.Trim().ToUpper() == "UPDATE")
        {
            string FBName = "";
            FBName = txt_FBName.Text.ToString();
            string firstdate = Convert.ToString(txt_fromdate.Text);
            string seconddate = Convert.ToString(txt_Enddate.Text);
            DateTime dt = new DateTime();
            DateTime dt1 = new DateTime();
            string[] split = firstdate.Split('/');
            dt = Convert.ToDateTime(split[1] + "/" + split[0] + "/" + split[2]);
            split = seconddate.Split('/');
            dt1 = Convert.ToDateTime(split[1] + "/" + split[0] + "/" + split[2]);
            string type = ""; int acadamic_isgeneral = 0; string isgeneral = ""; int IsType_Individual = 0; string IsType_Individualpoint = "";
            if (rb_Acad.Checked == true)
            {
                type = "1";
                if (cb_IsGeneral.Checked)
                    acadamic_isgeneral = 1;

                isgeneral = " ,Acadamic_Isgeneral ='" + acadamic_isgeneral + "' ";

                if (cb_Typeindiviual.Checked)
                    IsType_Individual = 1;

                IsType_Individualpoint = " ,IsType_Individual ='" + IsType_Individual + "'";
            }
            else
            {
                type = "2";
            }
            string activerow = FpSpread1.ActiveSheetView.ActiveRow.ToString();
            string activecol = FpSpread1.ActiveSheetView.ActiveColumn.ToString();
            int value = Convert.ToInt32(FpSpread1.Sheets[0].Cells[Convert.ToInt32(activerow), 1].Tag.ToString());
            string FeedBack = FpSpread1.Sheets[0].Cells[Convert.ToInt32(activerow), 7].Text.ToString();
            int college_cd = Convert.ToInt32(FpSpread1.Sheets[0].Cells[Convert.ToInt32(activerow), 0].Note.ToString());
            string updqry = "";
            int qtn = 0;
            string SubjectType = "";
            for (int sub = 0; sub < Cbl_Subjecttype.Items.Count; sub++)
            {
                if (Cbl_Subjecttype.Items[sub].Selected == true)
                {
                    if (SubjectType == "")
                    {
                        SubjectType = "" + Cbl_Subjecttype.Items[sub].Value.ToString() + "";
                    }
                    else
                    {
                        SubjectType = SubjectType + "," + Cbl_Subjecttype.Items[sub].Value.ToString() + "";
                    }
                }
            }
            string optional = "";
            if (cb_optional.Checked == true)
            {
                for (int sub = 0; sub < Cbl_Subjecttype1.Items.Count; sub++)
                {
                    if (Cbl_Subjecttype1.Items[sub].Selected == true)
                    {
                        if (optional == "")
                        {
                            optional = "" + Cbl_Subjecttype1.Items[sub].Value.ToString() + "";
                        }
                        else
                        {
                            optional = optional + "," + Cbl_Subjecttype1.Items[sub].Value.ToString() + "";
                        }
                    }
                }
            }
            string include = "";
            if (chk_includecommon.Enabled == true)
            {
                include = " ,InclueCommon='" + includecmn + "'";
            }
            else
            {
                include = " ,InclueCommon='1'";

            }
            //if (btn_Save.Text.Trim().ToUpper() == "UPDATE")
            //{
            //    include = " ,InclueCommon='0'";
            
            //}
            if (rb_induvgual.Checked == true)
            {
                updqry = "update CO_FeedBackMaster set FeedBackType='" + type + "', FeedBackName='" + FBName + "',StartDate='" + dt.ToString("MM/dd/yyyy") + "', EndDate='" + dt1.ToString("MM/dd/yyyy") + "',OptionalSubject_type='" + optional + "',Subject_Type='" + SubjectType + "' " + include + " " + isgeneral + " " + IsType_Individualpoint + " where FeedBackMasterPK='" + value + "' and collegecode ='" + ddl_college.SelectedItem.Value + "' ";
                qtn = d2.update_method_wo_parameter(updqry, "Text");
                if (qtn != 0)
                {
                    imgdiv2.Visible = true;
                    lbl_alert1.Text = "Updated successfully";
                }
                if (chk_includecommon.Checked == true)
                {
                    if (rb_induvgual.Checked == true)
                    {
                        string dept = FpSpread1.Sheets[0].Cells[Convert.ToInt32(activerow), 9].Tag.ToString();
                        string Section = FpSpread1.Sheets[0].Cells[Convert.ToInt32(activerow), 4].Note;
                        string sem = FpSpread1.Sheets[0].Cells[Convert.ToInt32(activerow), 3].Note;
                        string year = FpSpread1.Sheets[0].Cells[Convert.ToInt32(activerow), 1].Note;
                        string branch = FpSpread1.Sheets[0].Cells[Convert.ToInt32(activerow), 2].Note;
                        //  string FBName = FpSpread1.Sheets[0].Cells[Convert.ToInt32(activerow), 7].Text.ToString();
                        string first_uniqfk = d2.GetFunction(" select  top 1 FeedbackUnicodePK from CO_FeedbackUniCode order by   FeedbackUnicodePK desc");
                        int unqid = Convert.ToInt32(first_uniqfk);
                        unqid = unqid + 1;
                        first_uniqfk = Convert.ToString(unqid);
                        lbl_firstpk.Text = first_uniqfk;
                        string uniq = d2.GetFunction(" select COUNT(app_No) from CO_feedbackMaster f,Registration r where f.DegreeCode =r.degree_code and f.Batch_Year =r.Batch_Year and r.Current_Semester =f.semester and r.Sections =f.Section and f.FeedBackName ='" + FBName + "' and CC=0 and DelFlag =0 and Exam_Flag <>'Debar' and f.Batch_Year='" + year + "' and  f.DegreeCode='" + branch + "' and f.semester='" + sem + "' and f.Section='" + Section + "' ");
                        if (uniq != "")
                        {
                            // txt_total_strength.Text = Convert.ToString(uniq);
                        }
                        //int runningseries = Convert.ToInt32(txt_running_series.Text);
                        int runningseries = 0;
                        int sercount = 0;
                        if (txt_running_series.Text.Trim() != "")
                        {
                            string seriessize = Convert.ToString(txt_running_series.Text);
                            for (int m = 0; m < seriessize.Length; m++)
                            {
                                if (seriessize.Substring(m).StartsWith("0") == true)
                                {
                                    sercount++;
                                }
                                else
                                {
                                    goto series;
                                }
                            }
                        }
                    series:
                        int.TryParse(Convert.ToString(txt_running_series.Text), out runningseries);
                        string acroname = txt_fb_acr.Text.ToUpper();
                        int ins = 0;
                        for (int i = 0; i < Convert.ToInt32(uniq); i++)
                        {
                            string acr_rnseries = acroname + Convert.ToString(runningseries);
                            //runningseries = runningseries + 1;
                            string uniquecode = acroname + runningseries.ToString().PadLeft(sercount + Convert.ToInt32(Convert.ToString(runningseries).Length), '0');//10.08.16
                            string insertuniq = "insert into CO_FeedbackUniCode (FeedbackUnicode,FeedbackMasterFK,IsCheckFlag,UnicodeAcr,UniCodeStartNO) values ('" + uniquecode + "','" + value + "','0','" + txt_fb_acr.Text.ToString().ToUpper() + "','" + txt_running_series.Text + "')";
                            ins = d2.update_method_wo_parameter(insertuniq, "Text");
                            runningseries = runningseries + 1;
                        }
                        if (ins != 0)
                        {
                            imgdiv2.Visible = true;
                            lbl_alert1.Text = "Updated successfully";
                            btn_creatxl.Visible = true;
                            btn_errorclose.Visible = false;
                        }
                    }
                }
            }
            else if (rb_common.Checked == true)
            {
                //*********issu common updation29-02-2016
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
                string build = "";
                if (cbl_batch1.Items.Count > 0)
                {
                    for (int i = 0; i < cbl_batch1.Items.Count; i++)
                    {
                        if (cbl_batch1.Items[i].Selected == true)
                        {
                            if (build == "")
                            {
                                build = Convert.ToString(cbl_batch1.Items[i].Value);
                            }
                            else
                            {
                                build = build + "','" + Convert.ToString(cbl_batch1.Items[i].Value);
                            }
                        }
                    }
                }
                string branchcode = "";
                if (cbl_branch1.Items.Count > 0)
                {
                    for (int i = 0; i < cbl_branch1.Items.Count; i++)
                    {
                        if (cbl_branch1.Items[i].Selected == true)
                        {
                            if (branchcode == "")
                            {
                                branchcode = Convert.ToString(cbl_branch1.Items[i].Value);
                            }
                            else
                            {
                                branchcode = branchcode + "','" + Convert.ToString(cbl_branch1.Items[i].Value);
                            }
                        }
                    }
                }
                string sem = "";
                if (cbl_sem1.Items.Count > 0)
                {
                    for (int i = 0; i < cbl_sem1.Items.Count; i++)
                    {
                        if (cbl_sem1.Items[i].Selected == true)
                        {
                            if (sem == "")
                            {
                                sem = Convert.ToString(cbl_sem1.Items[i].Value);
                            }
                            else
                            {
                                sem = sem + "','" + Convert.ToString(cbl_sem1.Items[i].Value);
                            }
                        }
                    }
                }
                string section = "";
                if (cbl_sec1.Items.Count > 0)
                {
                    for (int i = 0; i < cbl_sec1.Items.Count; i++)
                    {
                        if (cbl_sec1.Items[i].Selected == true)
                        {
                            if (section == "")
                            {
                                section = Convert.ToString(cbl_sec1.Items[i].Value);
                            }
                            else
                            {
                                section = section + "','" + Convert.ToString(cbl_sec1.Items[i].Value);
                            }
                            if (cbl_sec1.Items[i].Value == "Empty")
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
                updqry = "update CO_FeedBackMaster set FeedBackType='" + type + "', FeedBackName='" + FBName + "',StartDate='" + dt.ToString("MM/dd/yyyy") + "', EndDate='" + dt1.ToString("MM/dd/yyyy") + "',InclueCommon='" + includecmn + "',OptionalSubject_type='" + optional + "',Subject_Type='" + SubjectType + "' " + isgeneral + " " + IsType_Individualpoint + " where FeedBackName='" + FeedBack + "' and collegecode in ('" + college + "') and Batch_Year in('" + build + "') and DegreeCode in('" + branchcode + "') and Section  in('" + section + "') and semester in('" + sem + "')   ";
                qtn = d2.update_method_wo_parameter(updqry, "Text");
                if (qtn != 0)
                {
                    imgdiv2.Visible = true;
                    lbl_alert1.Text = "Updated successfully";
                }
                if (chk_includecommon.Checked == true)
                {
                    string first_uniqfk = d2.GetFunction(" select  top 1 FeedbackUnicodePK from CO_FeedbackUniCode order by   FeedbackUnicodePK desc");
                    int unqid = Convert.ToInt32(first_uniqfk);
                    unqid = unqid + 1;
                    first_uniqfk = Convert.ToString(unqid);
                    string updatuniqid = "select COUNT(app_No) as student_count,r.Batch_Year, r.degree_code,f.FeedBackMasterPK,semester,Sections   from CO_feedbackMaster f,Registration r where f.DegreeCode =r.degree_code and f.Batch_Year =r.Batch_Year and r.Current_Semester =f.semester and r.Sections =f.Section and f.FeedBackName ='" + txt_FBName.Text.ToString() + "' and CC=0 and DelFlag =0 and Exam_Flag <>'Debar' group by r.Batch_Year,r.degree_code,f.FeedBackMasterPK ,semester,Sections";
                    //  updatuniqid = updatuniqid + "";
                    ds = d2.select_method_wo_parameter(updatuniqid, "Text");
                    string acroname = txt_fb_acr.Text.ToUpper();
                    if (acroname != "")
                    {
                        string acr = d2.GetFunction(" select UnicodeAcr from CO_FeedbackUniCode  where UnicodeAcr ='" + acroname + "'");
                        if (acr.Trim() != "0")
                        {
                            imgdiv2.Visible = true;
                            lbl_alert1.Visible = true;
                            lbl_alert1.Text = "This Acronames Already Existed";
                            return;
                        }
                    }
                    //int runningseries = Convert.ToInt32(txt_running_series.Text);
                    int runningseries = 0;
                    int sercount = 0;
                    if (txt_running_series.Text.Trim() != "")
                    {
                        string seriessize = Convert.ToString(txt_running_series.Text);
                        for (int m = 0; m < seriessize.Length; m++)
                        {
                            if (seriessize.Substring(m).StartsWith("0") == true)
                            {
                                sercount++;
                            }
                            else
                            {
                                goto series;
                            }
                        }
                    }
                series:
                    int.TryParse(Convert.ToString(txt_running_series.Text), out runningseries);
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        string fbfk = ds.Tables[0].Rows[i]["FeedBackMasterPK"].ToString();
                        int studcount = Convert.ToInt32(ds.Tables[0].Rows[i]["student_count"]);
                        for (int j = 0; j < studcount; j++)
                        {
                            string acr_rnseries = acroname + Convert.ToString(runningseries);
                            //runningseries = runningseries + 1;
                            string uniquecode = acroname + runningseries.ToString().PadLeft(sercount + Convert.ToInt32(Convert.ToString(runningseries).Length), '0');//10.08.16
                            string insertuniq = "insert into CO_FeedbackUniCode (FeedbackUnicode,FeedbackMasterFK,IsCheckFlag,UnicodeAcr,UniCodeStartNO) values ('" + acr_rnseries + "','" + fbfk + "','0','" + txt_fb_acr.Text.ToString().ToUpper() + "','" + txt_running_series.Text + "')";
                            int ins = d2.update_method_wo_parameter(insertuniq, "Text");
                            runningseries = runningseries + 1;
                        }
                    }
                    if (chk_includecommon.Checked == true)
                    {
                        string second_uniqfk = d2.GetFunction(" select  top 1 FeedbackUnicodePK from CO_FeedbackUniCode order by   FeedbackUnicodePK desc");
                        string unic = "select FeedbackUnicode from  CO_FeedbackUniCode where FeedbackUnicodePK between ('" + first_uniqfk + "') and ('" + second_uniqfk + "') ";
                        ds1 = d2.select_method_wo_parameter(unic, "Text");
                        if (ds1.Tables.Count > 0)
                        {
                            try
                            {
                                string degreedetails = "Unicq code Gendration";
                                string pagename = "FeedBack_Master.aspx";
                                ExportTable(ds1.Tables[0]);
                                Printcontrol.Visible = true;
                            }
                            catch (Exception ex)
                            {
                                lbl_alert1.Visible = true;
                                lbl_alert1.Text = ex.ToString();
                            }
                        }
                    }
                }
            }
            FbName1();
            btn_Search_Click(sender, e);
            Add_FeedBack.Visible = false;
            FpSpread1.SaveChanges();
            txt_FBName.Text = "";
        }
    }
    protected void btn_creatxl_Click(object sender, EventArgs e)
    {
        string first_uniqfk = lbl_firstpk.Text;
        if (chk_includecommon.Checked == true)
        {
            string second_uniqfk = d2.GetFunction(" select  top 1 FeedbackUnicodePK from CO_FeedbackUniCode order by   FeedbackUnicodePK desc");
            string unic = "select FeedbackUnicode  from  CO_FeedbackUniCode where FeedbackUnicodePK between ('" + first_uniqfk + "') and ('" + second_uniqfk + "') ";
            ds1 = d2.select_method_wo_parameter(unic, "Text");
            if (ds1.Tables.Count > 0)
            {
                try
                {
                    string degreedetails = "Unicq code Gendration";
                    string pagename = "FeedBack_Master.aspx";
                    ExportTable(ds1.Tables[0]);
                    imgdiv2.Visible = false;
                    btn_creatxl.Visible = false;
                    Printcontrol.Visible = true;
                    txt_fb_acr.Text = "";
                    txt_running_series.Text = "";
                    txt_total_strength.Text = "";
                    chk_includecommon.Checked = false;
                }
                catch (Exception ex)
                {
                    //    lbl_alert1.Visible = true;
                    //    lbl_alert1.Text = ex.ToString();
                }
            }
        }
    }
    protected void btn_exit_Click(object sender, EventArgs e)
    {
        Add_FeedBack.Visible = false;
    }
    protected void rb_Acad_CheckedChanged(object sender, EventArgs e)
    {
        Acad.Visible = true;
        Gendral.Visible = true;
        visibletr();
        lbl_Subject_Type.Visible = true;
        Txt_Subjecttype.Visible = true;
        Panel_Subjecttype.Visible = true;
        chk_includecommon.Enabled = true;
        chk_includecommon.Checked = true;
        includecommon();
        if (btn_Save.Text.Trim().ToUpper() == "UPDATE")
        {
            visiblefalse();
        }
        cb_IsGeneral.Visible = true;
        cb_IsGeneral_CheckedChanged(sender, e);
        cb_Typeindiviual.Visible = true;
        cb_Subjectwise.Visible = true;
    }
    protected void rb_Gend_CheckedChanged(object sender, EventArgs e)
    {
        Gendral.Visible = true;
        visiblefalse();
        lbl_Subject_Type.Visible = false;
        Txt_Subjecttype.Visible = false;
        Panel_Subjecttype.Visible = false;
        chk_includecommon.Enabled = false;
        chk_includecommon.Checked = false;
        includecommon();
        cb_IsGeneral.Visible = false;
        cb_Typeindiviual.Visible = false;
        cb_Subjectwise.Visible = false;
    }
    protected void btn_Search_Click(object sender, EventArgs e)
    {
        if (rb_Acad1.Checked == true)
        {
            acd();
        }
        else if (rb_Gend1.Checked == true)
        {
            gnd();
        }
    }
    public void acd()
    {
        try
        {
            {
                rptprint1.Visible = true;
                //div1.Visible = true;
                Add_FeedBack.Visible = false;
                string feedbackname = "";
                int feedback_code = 0;
                for (int i = 0; i < Cbl_FbName1.Items.Count; i++)
                {
                    if (Cbl_FbName1.Items[i].Selected == true)
                    {
                        if (feedbackname == "")
                        {
                            feedbackname = "" + Cbl_FbName1.Items[i].Value.ToString() + "";
                            feedback_code = 1;
                        }
                        else
                        {
                            feedbackname = feedbackname + "','" + Cbl_FbName1.Items[i].Value.ToString() + "";
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
                string build = "";
                if (cbl_batch1.Items.Count > 0)
                {
                    for (int i = 0; i < cbl_batch1.Items.Count; i++)
                    {
                        if (cbl_batch1.Items[i].Selected == true)
                        {
                            if (build == "")
                            {
                                build = Convert.ToString(cbl_batch1.Items[i].Value);
                            }
                            else
                            {
                                build = build + "','" + Convert.ToString(cbl_batch1.Items[i].Value);
                            }
                        }
                    }
                }
                string branchcode = "";
                if (cbl_branch1.Items.Count > 0)
                {
                    for (int i = 0; i < cbl_branch1.Items.Count; i++)
                    {
                        if (cbl_branch1.Items[i].Selected == true)
                        {
                            if (branchcode == "")
                            {
                                branchcode = Convert.ToString(cbl_branch1.Items[i].Value);
                            }
                            else
                            {
                                branchcode = branchcode + "','" + Convert.ToString(cbl_branch1.Items[i].Value);
                            }
                        }
                    }
                }
                string sem = "";
                if (cbl_sem1.Items.Count > 0)
                {
                    for (int i = 0; i < cbl_sem1.Items.Count; i++)
                    {
                        if (cbl_sem1.Items[i].Selected == true)
                        {
                            if (sem == "")
                            {
                                sem = Convert.ToString(cbl_sem1.Items[i].Value);
                            }
                            else
                            {
                                sem = sem + "','" + Convert.ToString(cbl_sem1.Items[i].Value);
                            }
                        }
                    }
                }
                string section = "";
                if (cbl_sec1.Items.Count > 0)
                {
                    for (int i = 0; i < cbl_sec1.Items.Count; i++)
                    {
                        if (cbl_sec1.Items[i].Selected == true)
                        {
                            if (section == "")
                            {
                                section = Convert.ToString(cbl_sec1.Items[i].Value);
                            }
                            else
                            {
                                section = section + "','" + Convert.ToString(cbl_sec1.Items[i].Value);
                            }
                            if (cbl_sec1.Items[i].Value == "Empty")
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
                string type;
                if (rb_Acad1.Checked == true)
                {
                    type = "1";
                }
                else
                {
                    type = "2";
                }
                ds.Clear();
                FpSpread1.Width = 1000;
                FpSpread1.Sheets[0].RowCount = 0;
                FpSpread1.Sheets[0].ColumnCount = 0;
                FpSpread1.CommandBar.Visible = false;
                FpSpread1.Sheets[0].AutoPostBack = true;
                FpSpread1.Sheets[0].ColumnHeader.RowCount = 1;
                FpSpread1.Sheets[0].RowHeader.Visible = false;
                FpSpread1.Sheets[0].ColumnCount = 15;
                FarPoint.Web.Spread.StyleInfo darkstyle = new FarPoint.Web.Spread.StyleInfo();
                darkstyle.BackColor = ColorTranslator.FromHtml("#0CA6CA");
                darkstyle.ForeColor = Color.White;
                FpSpread1.ActiveSheetView.ColumnHeader.DefaultStyle = darkstyle;
                FpSpread1.Visible = true;
                FpSpread1.SaveChanges();
                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 0].Text = "S.No";
                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 1].Text = "Batch Year";
                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 2].Text = "Degree";
                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 3].Text = "Department";
                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 4].Text = "Semester";
                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 5].Text = "Section";
                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 6].Text = "Type";
                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 7].Text = "FeedBack_Name";
                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 8].Text = "Start Date";
                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 9].Text = "End Date";
                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 10].Text = "Include Common";
                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 11].Text = "Subject Type";
                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 12].Text = "Optional Subject Type";
                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 13].Text = "IsGeneral";
                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 14].Text = "Type Individual";
                for (int i = 0; i < FpSpread1.Sheets[0].Columns.Count; i++)
                {
                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, i].Font.Bold = true;
                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, i].HorizontalAlign = HorizontalAlign.Center;
                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, i].Font.Name = "Book Antiqua";
                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, i].Font.Size = FontUnit.Medium;
                    FpSpread1.Sheets[0].Columns[0].HorizontalAlign = HorizontalAlign.Center;
                    FpSpread1.Sheets[0].Columns[1].HorizontalAlign = HorizontalAlign.Center;
                    FpSpread1.Sheets[0].Columns[2].HorizontalAlign = HorizontalAlign.Center;
                    FpSpread1.Sheets[0].Columns[3].HorizontalAlign = HorizontalAlign.Left;
                    FpSpread1.Sheets[0].Columns[4].HorizontalAlign = HorizontalAlign.Center;
                    FpSpread1.Sheets[0].Columns[5].HorizontalAlign = HorizontalAlign.Center;
                    FpSpread1.Sheets[0].Columns[6].HorizontalAlign = HorizontalAlign.Left;
                    FpSpread1.Sheets[0].Columns[7].HorizontalAlign = HorizontalAlign.Left;
                    FpSpread1.Sheets[0].Columns[8].HorizontalAlign = HorizontalAlign.Center;
                    FpSpread1.Sheets[0].Columns[9].HorizontalAlign = HorizontalAlign.Center;
                    FpSpread1.Sheets[0].Columns[10].HorizontalAlign = HorizontalAlign.Center;
                    FpSpread1.Sheets[0].Columns[11].HorizontalAlign = HorizontalAlign.Center;
                    FpSpread1.Sheets[0].Columns[12].HorizontalAlign = HorizontalAlign.Center;
                    FpSpread1.Sheets[0].Columns[13].HorizontalAlign = HorizontalAlign.Center;
                    FpSpread1.Sheets[0].Columns[14].HorizontalAlign = HorizontalAlign.Center;
                }
                FpSpread1.Sheets[0].ColumnHeader.Columns[0].Width = 59;
                FpSpread1.Sheets[0].ColumnHeader.Columns[1].Width = 100;
                FpSpread1.Sheets[0].ColumnHeader.Columns[2].Width = 70;
                FpSpread1.Sheets[0].ColumnHeader.Columns[3].Width = 170;
                FpSpread1.Sheets[0].ColumnHeader.Columns[4].Width = 70;
                FpSpread1.Sheets[0].ColumnHeader.Columns[5].Width = 70;
                FpSpread1.Sheets[0].ColumnHeader.Columns[6].Width = 100;
                FpSpread1.Sheets[0].ColumnHeader.Columns[7].Width = 300;
                string selqry = "";
                if (rb_Acad1.Checked == true)
                {
                    selqry = "   SELECT FeedBackMasterPK, FeedBackType,case when FeedBackType=1 then 'Academic' when FeedBackType=2 then 'General' end as Type,CollegeCode,Batch_Year,DegreeCode,Section,semester,FeedBackName,CONVERT(varchar(10), StartDate,103) as StartDate,CONVERT(varchar(10), EndDate,103) as EndDate,InclueCommon,dt.Dept_Name,c.Course_Name, c.Course_Id,Subject_Type,OptionalSubject_type,case when isnull(cf.Acadamic_Isgeneral,0)='0' then 'No' when cf.Acadamic_Isgeneral='1' then 'Yes' end  Acadamic_Isgeneral,case when isnull(cf.IsType_Individual,0)='0' then 'No' when cf.IsType_Individual='1' then 'Yes' end  IsType_Individual FROM CO_FeedBackMaster cf,Degree d,Department dt,Course c WHERE cf.DegreeCode =d.Degree_Code and d.Dept_Code =dt.Dept_Code and c.Course_Id =d.Course_Id and  FeedBackType ='" + type + "' and CollegeCode in ('" + college + "' ) and DegreeCode in ('" + branchcode + "') and Batch_Year in ('" + build + "') and semester in ('" + sem + "')  and Section in ('" + section + "') and FeedBackName in ('" + feedbackname + "')";
                }
                ds = d2.select_method_wo_parameter(selqry, "Text");
                if (college != "" && build != "" && branchcode != "")
                {
                    if (ds.Tables.Count > 0)
                    {
                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                            {
                                FpSpread1.Sheets[0].RowCount++;
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Text = Convert.ToString(i + 1);
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Tag = ds.Tables[0].Rows[i]["Type"].ToString();
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Note = ds.Tables[0].Rows[i]["CollegeCode"].ToString();

                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].Tag = ds.Tables[0].Rows[i]["FeedBackMasterPK"].ToString();
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].Text = ds.Tables[0].Rows[i]["Batch_Year"].ToString();
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].Note = ds.Tables[0].Rows[i]["Batch_Year"].ToString();
                                //FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].VerticalAlign = FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].VerticalAlign = VerticalAlign.Middle;
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].Text = ds.Tables[0].Rows[i]["Course_Name"].ToString();
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].Tag = ds.Tables[0].Rows[i]["FeedBackType"].ToString();
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].Note = ds.Tables[0].Rows[i]["DegreeCode"].ToString();

                                //FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].VerticalAlign = FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].VerticalAlign = VerticalAlign.Middle;
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 3].Text = ds.Tables[0].Rows[i]["Dept_Name"].ToString();
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 3].Tag = ds.Tables[0].Rows[i]["Acadamic_Isgeneral"].ToString();
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 3].Note = ds.Tables[0].Rows[i]["semester"].ToString();
                                //FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 3].VerticalAlign = FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 3].VerticalAlign = VerticalAlign.Middle;
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 4].Text = ds.Tables[0].Rows[i]["semester"].ToString();
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 4].Note = ds.Tables[0].Rows[i]["Section"].ToString();
                                //FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 4].VerticalAlign = FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 5].VerticalAlign = VerticalAlign.Middle;
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 5].Text = ds.Tables[0].Rows[i]["Section"].ToString();
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 5].Tag = ds.Tables[0].Rows[i]["InclueCommon"].ToString();
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 6].Text = ds.Tables[0].Rows[i]["Type"].ToString();
                                //FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 6].VerticalAlign = FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 6].VerticalAlign = VerticalAlign.Middle;
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 7].Text = ds.Tables[0].Rows[i]["FeedBackName"].ToString();
                                //FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 7].VerticalAlign = FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 7].VerticalAlign = VerticalAlign.Middle;

                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 8].Text = ds.Tables[0].Rows[i]["StartDate"].ToString();
                                //FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 8].VerticalAlign = FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 8].VerticalAlign = VerticalAlign.Middle;
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 9].Text = ds.Tables[0].Rows[i]["EndDate"].ToString();
                                //FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 9].VerticalAlign = FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 9].VerticalAlign = VerticalAlign.Middle;
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 9].Tag = ds.Tables[0].Rows[i]["Course_Id"].ToString();
                                //FpSpread1.Sheets[0].Cells[i, 4].Tag = ds.Tables[0].Rows[i]["CollegeCode"].ToString();
                                string commanname = ds.Tables[0].Rows[i]["InclueCommon"].ToString();
                                if (commanname == "1" || commanname == "True")
                                {
                                    commanname = "YES";
                                }
                                else
                                {
                                    commanname = "NO";
                                }
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 10].Text = commanname.ToString();
                                //FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 10].VerticalAlign = FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 10].VerticalAlign = VerticalAlign.Middle;
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 11].Text = ds.Tables[0].Rows[i]["Subject_Type"].ToString();
                                //FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 11].VerticalAlign = FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 11].VerticalAlign = VerticalAlign.Middle;
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 12].Text = ds.Tables[0].Rows[i]["OptionalSubject_type"].ToString();
                                string option = ds.Tables[0].Rows[i]["OptionalSubject_type"].ToString();
                                if (option == "")
                                {
                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 12].Text = "-";
                                }
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 13].Text = ds.Tables[0].Rows[i]["Acadamic_Isgeneral"].ToString();
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 14].Text = ds.Tables[0].Rows[i]["IsType_Individual"].ToString();
                            }
                            FpSpread1.Sheets[0].PageSize = FpSpread1.Sheets[0].RowCount;
                            //FpSpread1.Sheets[0].SetColumnMerge(1, FarPoint.Web.Spread.Model.MergePolicy.Always);
                            //FpSpread1.Sheets[0].SetColumnMerge(2, FarPoint.Web.Spread.Model.MergePolicy.Always);
                            //FpSpread1.Sheets[0].SetColumnMerge(3, FarPoint.Web.Spread.Model.MergePolicy.Always);
                            //FpSpread1.Sheets[0].SetColumnMerge(4, FarPoint.Web.Spread.Model.MergePolicy.Always);
                            //FpSpread1.Sheets[0].SetColumnMerge(5, FarPoint.Web.Spread.Model.MergePolicy.Always);
                            //FpSpread1.Sheets[0].SetColumnMerge(6, FarPoint.Web.Spread.Model.MergePolicy.Always);
                            //FpSpread1.Sheets[0].SetColumnMerge(7, FarPoint.Web.Spread.Model.MergePolicy.Always);
                            //FpSpread1.Sheets[0].SetColumnMerge(8, FarPoint.Web.Spread.Model.MergePolicy.Always);
                            //FpSpread1.Sheets[0].SetColumnMerge(9, FarPoint.Web.Spread.Model.MergePolicy.Always);
                            //FpSpread1.Sheets[0].SetColumnMerge(10, FarPoint.Web.Spread.Model.MergePolicy.Always);
                            //FpSpread1.Sheets[0].SetColumnMerge(11, FarPoint.Web.Spread.Model.MergePolicy.Always);
                        }
                        else
                        {
                            rptprint1.Visible = false;
                            imgdiv2.Visible = true;
                            lbl_alert1.Text = "No Records Found";
                            FpSpread1.Visible = false;
                            //div1.Visible = false;
                        }
                    }
                    else
                    {
                        rptprint1.Visible = false;
                        imgdiv2.Visible = true;
                        lbl_alert1.Text = "No Records ";
                        FpSpread1.Visible = false;
                        //div1.Visible = false;
                    }
                }
                else
                {
                    rptprint1.Visible = false;
                    imgdiv2.Visible = true;
                    lbl_alert1.Text = "Please Select All Fields ";
                    FpSpread1.Visible = false;
                    //div1.Visible = false;
                }
            }
        }
        catch
        {
        }
    }
    public void gnd()
    {
        try
        {
            rptprint1.Visible = true;
            //div1.Visible = true;
            Add_FeedBack.Visible = false;
            // FeedBack();
            string feedbackname = "";
            int feedback_code = 0;
            for (int i = 0; i < Cbl_FbName1.Items.Count; i++)
            {
                if (Cbl_FbName1.Items[i].Selected == true)
                {
                    if (feedbackname == "")
                    {
                        feedbackname = "" + Cbl_FbName1.Items[i].Value.ToString() + "";
                        feedback_code = 1;
                    }
                    else
                    {
                        feedbackname = feedbackname + "','" + Cbl_FbName1.Items[i].Value.ToString() + "";
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
            string type;
            if (rb_Acad1.Checked == true)
            {
                type = "1";
            }
            else
            {
                type = "2";
            }
            FpSpread1.Sheets[0].RowCount = 0;
            FpSpread1.Sheets[0].ColumnCount = 0;
            FpSpread1.CommandBar.Visible = false;
            FpSpread1.Sheets[0].AutoPostBack = true;
            FpSpread1.Sheets[0].ColumnHeader.RowCount = 1;
            FpSpread1.Sheets[0].RowHeader.Visible = false;
            FpSpread1.Sheets[0].ColumnCount = 6;
            FarPoint.Web.Spread.StyleInfo darkstyle = new FarPoint.Web.Spread.StyleInfo();
            darkstyle.BackColor = ColorTranslator.FromHtml("#0CA6CA");
            darkstyle.ForeColor = Color.White;
            FpSpread1.ActiveSheetView.ColumnHeader.DefaultStyle = darkstyle;
            FpSpread1.Visible = true;
            FpSpread1.SaveChanges();
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 0].Text = "S.No";
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 1].Text = "Type";
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 2].Text = "FeedBack_Name";
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 3].Text = "Start Date";
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 4].Text = "End Date";
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 5].Text = "Include Common";
            for (int i = 0; i < FpSpread1.Sheets[0].Columns.Count; i++)
            {
                FpSpread1.Sheets[0].ColumnHeader.Cells[0, i].Font.Bold = true;
                FpSpread1.Sheets[0].ColumnHeader.Cells[0, i].HorizontalAlign = HorizontalAlign.Center;
                FpSpread1.Sheets[0].ColumnHeader.Cells[0, i].Font.Name = "Book Antiqua";
                FpSpread1.Sheets[0].ColumnHeader.Cells[0, i].Font.Size = FontUnit.Medium;
                FpSpread1.Sheets[0].Columns[0].HorizontalAlign = HorizontalAlign.Center;
                FpSpread1.Sheets[0].Columns[3].HorizontalAlign = HorizontalAlign.Center;
                FpSpread1.Sheets[0].Columns[4].HorizontalAlign = HorizontalAlign.Center;
                FpSpread1.Sheets[0].Columns[5].HorizontalAlign = HorizontalAlign.Center;
            }
            FpSpread1.Sheets[0].ColumnHeader.Columns[0].Width = 59;
            FpSpread1.Sheets[0].ColumnHeader.Columns[1].Width = 100;
            FpSpread1.Sheets[0].ColumnHeader.Columns[2].Width = 300;
            FpSpread1.Sheets[0].ColumnHeader.Columns[3].Width = 100;
            FpSpread1.Sheets[0].ColumnHeader.Columns[4].Width = 100;
            FpSpread1.Width = 944;
            ////ds.Clear();
            string selqry = "";
            if (rb_Gend1.Checked == true)
            {
                selqry = " SELECT FeedBackMasterPK, FeedBackType,case when FeedBackType=1 then 'Academic' when FeedBackType=2 then 'General' end as Type,CollegeCode,Batch_Year,DegreeCode,Section,semester,FeedBackName,CONVERT(varchar(10), StartDate,103) as StartDate,CONVERT(varchar(10), EndDate,103) as EndDate,InclueCommon FROM CO_FeedBackMaster WHERE CollegeCode in('" + college + "') and FeedBackType='" + type + "' and FeedBackName in ('" + feedbackname + "')  ";
            }
            ds = d2.select_method_wo_parameter(selqry, "Text");
            if (ds.Tables.Count > 0)
            {
                if (ds.Tables[0].Rows.Count > 0)
                {
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        FpSpread1.Sheets[0].RowCount++;
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Text = Convert.ToString(i + 1);
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].Tag = ds.Tables[0].Rows[i]["FeedBackMasterPK"].ToString();
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].Text = ds.Tables[0].Rows[i]["Type"].ToString();
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].VerticalAlign = FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 3].VerticalAlign = VerticalAlign.Middle;
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].Text = ds.Tables[0].Rows[i]["FeedBackName"].ToString();
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].Tag = ds.Tables[0].Rows[i]["FeedBackType"].ToString();
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 3].Text = ds.Tables[0].Rows[i]["StartDate"].ToString();
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 3].VerticalAlign = FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 3].VerticalAlign = VerticalAlign.Middle;
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 4].Text = ds.Tables[0].Rows[i]["EndDate"].ToString();
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 4].VerticalAlign = FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 4].VerticalAlign = VerticalAlign.Middle;
                        string commanname = ds.Tables[0].Rows[i]["InclueCommon"].ToString();
                        if (commanname == "1" || commanname == "True")
                        {
                            commanname = "YES";
                        }
                        else
                        {
                            commanname = "NO";
                        }
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 5].Text = commanname.ToString();
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 5].VerticalAlign = FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 5].VerticalAlign = VerticalAlign.Middle;
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 5].Tag = ds.Tables[0].Rows[i]["InclueCommon"].ToString();
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Note = ds.Tables[0].Rows[i]["CollegeCode"].ToString();
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Tag = ds.Tables[0].Rows[i]["Type"].ToString();
                    }
                    FpSpread1.Sheets[0].SetColumnMerge(1, FarPoint.Web.Spread.Model.MergePolicy.Always);
                    FpSpread1.Sheets[0].SetColumnMerge(3, FarPoint.Web.Spread.Model.MergePolicy.Always);
                    FpSpread1.Sheets[0].SetColumnMerge(4, FarPoint.Web.Spread.Model.MergePolicy.Always);
                    FpSpread1.Sheets[0].SetColumnMerge(5, FarPoint.Web.Spread.Model.MergePolicy.Always);
                }
                else
                {
                    rptprint1.Visible = false;
                    imgdiv2.Visible = true;
                    lbl_alert1.Text = "No Records Found";
                    FpSpread1.Visible = false;
                    //div1.Visible = false;
                }
            }
            else
            {
                rptprint1.Visible = false;
                imgdiv2.Visible = true;
                lbl_alert1.Text = "No Records ";
                FpSpread1.Visible = false;
                //div1.Visible = false;
            }
            FpSpread1.Sheets[0].PageSize = FpSpread1.Sheets[0].RowCount;
        }
        catch
        {
        }
    }
    protected void rb_induvgual_CheckedChanged(object sender, EventArgs e)
    {
        string activerow = FpSpread1.ActiveSheetView.ActiveRow.ToString();
        string activecol = FpSpread1.ActiveSheetView.ActiveColumn.ToString();
        string value = FpSpread1.Sheets[0].Cells[Convert.ToInt32(activerow), 1].Tag.ToString();
        string fbfk = d2.GetFunction("select * from CO_FeedbackUniCode where  FeedbackMasterFK in('" + value + "')");
        if (fbfk == "0")
        {
            chk_includecommon.Enabled = true;
            lbl_totl_strength.Visible = true;
            txt_total_strength.Visible = true;
            lbl_fb_acr.Visible = true;
            txt_fb_acr.Visible = true;
            lbl_run_sries.Visible = true;
            txt_running_series.Visible = true;
        }
        else
        {
            chk_includecommon.Enabled = false;
            includecommon();
        }
        chk_includecommon.Enabled = false;//Added By Saranyadevi20.0.2018
        lbl_totl_strength.Visible = false;
        txt_total_strength.Visible = false;
        lbl_fb_acr.Visible = false;
        txt_fb_acr.Visible = false;
        lbl_run_sries.Visible = false;
        txt_running_series.Visible = false;
    }
    protected void rb_common_CheckedChanged(object sender, EventArgs e)
    {
        chk_includecommon.Enabled = false;
        chk_includecommon.Checked = false;
        includecommon();
    }
    protected void rb_Acad1_CheckedChanged(object sender, EventArgs e)
    {
        Acd.Visible = true;
        visibletrue1();
        FbName1();
    }
    protected void rb_Gend1_CheckedChanged(object sender, EventArgs e)
    {
        Acd.Visible = true;
        visiblefalse1();
        FbName1();
    }
    protected void txt_fromdate_TextChanged(object sender, EventArgs e)
    {
        try
        {
            datevalidate(txt_fromdate, txt_Enddate);
            //{
            //    DateTime dt = new DateTime();
            //    string firstdate = txt_fromdate.Text.ToString();
            //    DateTime dtm = DateTime.Now;
            //    string[] split1 = firstdate.Split('/');
            //    dt = Convert.ToDateTime(split1[1] + "/" + split1[0] + "/" + split1[2]);
            //    if (dt > dtm)
            //    {
            //        imgdiv2.Visible = true;
            //        lbl_alert1.Text = "Please Slect Start Date less Than  Today ";
            //        txt_fromdate.Text = DateTime.Now.ToString("dd/MM/yyyy");
            //    }
            //}
        }
        catch (Exception ex)
        {
        }
    }
    protected void txt_Enddate_TextChanged(object sender, EventArgs e)
    {
        try
        {
            datevalidate(txt_fromdate, txt_Enddate);
            //DateTime dt1 = new DateTime();
            //DateTime dtm = DateTime.Now;
            //string seconddate = txt_Enddate.Text.ToString();
            //string[] split = seconddate.Split('/');
            //dt1 = Convert.ToDateTime(split[1] + "/" + split[0] + "/" + split[2]);
            //if (dtm > dt1)
            //{
            //    imgdiv2.Visible = true;
            //    lbl_alert1.Text = "Please Slect End Date Greater Than  Today ";
            //    txt_Enddate.Text = DateTime.Now.ToString("dd/MM/yyyy");
            //}
        }
        catch (Exception ex)
        {
        }
    }
    public void datevalidate(TextBox txt1, TextBox txt2)
    {
        try
        {
            if (txt1.Text != "" && txt2.Text != "")
            {
                DateTime dt = new DateTime();
                DateTime dt1 = new DateTime();
                string firstdate = Convert.ToString(txt1.Text);
                DateTime dtm = DateTime.Now;
                string seconddate = Convert.ToString(txt2.Text);
                string[] split = firstdate.Split('/');
                dt = Convert.ToDateTime(split[1] + "/" + split[0] + "/" + split[2]);
                split = seconddate.Split('/');
                dt1 = Convert.ToDateTime(split[1] + "/" + split[0] + "/" + split[2]);
                {
                    TimeSpan ts = dt1 - dt;
                    int days = ts.Days;
                    if (dt > dt1)
                    {
                        imgdiv2.Visible = true;
                        lbl_alert1.Text = "Please Slect End Date Greater Than  Start Date ";
                        txt2.Text = DateTime.Now.ToString("dd/MM/yyyy");
                        txt1.Text = DateTime.Now.ToString("dd/MM/yyyy");
                    }
                }
            }
        }
        catch (Exception ex)
        {
        }
    }
    public void Cb_FbName1_CheckedChanged(object sender, EventArgs e)
    {
        try
        {
            int cout = 0;
            Txt_FbName1.Text = "--Select--";
            if (Cb_FbName1.Checked == true)
            {
                cout++;
                for (int i = 0; i < Cbl_FbName1.Items.Count; i++)
                {
                    Cbl_FbName1.Items[i].Selected = true;
                }
                Txt_FbName1.Text = "FeedBack(" + (Cbl_FbName1.Items.Count) + ")";
            }
            else
            {
                for (int i = 0; i < Cbl_FbName1.Items.Count; i++)
                {
                    Cbl_FbName1.Items[i].Selected = false;
                }
                Txt_FbName1.Text = "--Select--";
            }
        }
        catch (Exception ex)
        {
        }
    }
    public void Cbl_FbName1_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            int commcount = 0;
            Txt_FbName1.Text = "--Select--";
            Cb_FbName1.Checked = false;
            for (int i = 0; i < Cbl_FbName1.Items.Count; i++)
            {
                if (Cbl_FbName1.Items[i].Selected == true)
                {
                    commcount = commcount + 1;
                    Cb_FbName1.Checked = false;
                }
            }
            if (commcount > 0)
            {
                if (commcount == Cbl_FbName1.Items.Count)
                {
                    Cb_FbName1.Checked = true;
                }
                Txt_FbName1.Text = "FeedBack(" + commcount.ToString() + ")";
            }
        }
        catch (Exception ex)
        {
        }
    }
    public void FbName1()
    {
        try
        {
            ds.Clear();
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
            string build = "";
            if (cbl_batch1.Items.Count > 0)
            {
                for (int i = 0; i < cbl_batch1.Items.Count; i++)
                {
                    if (cbl_batch1.Items[i].Selected == true)
                    {
                        if (build == "")
                        {
                            build = Convert.ToString(cbl_batch1.Items[i].Value);
                        }
                        else
                        {
                            build = build + "','" + Convert.ToString(cbl_batch1.Items[i].Value);
                        }
                    }
                }
            }
            string branchcode = "";
            if (cbl_branch1.Items.Count > 0)
            {
                for (int i = 0; i < cbl_branch1.Items.Count; i++)
                {
                    if (cbl_branch1.Items[i].Selected == true)
                    {
                        if (branchcode == "")
                        {
                            branchcode = Convert.ToString(cbl_branch1.Items[i].Value);
                        }
                        else
                        {
                            branchcode = branchcode + "','" + Convert.ToString(cbl_branch1.Items[i].Value);
                        }
                    }
                }
            }
            string sem = "";
            if (cbl_sem1.Items.Count > 0)
            {
                for (int i = 0; i < cbl_sem1.Items.Count; i++)
                {
                    if (cbl_sem1.Items[i].Selected == true)
                    {
                        if (sem == "")
                        {
                            sem = Convert.ToString(cbl_sem1.Items[i].Value);
                        }
                        else
                        {
                            sem = sem + "','" + Convert.ToString(cbl_sem1.Items[i].Value);
                        }
                    }
                }
            }
            string section = "";
            if (cbl_sec1.Items.Count > 0)
            {
                for (int i = 0; i < cbl_sec1.Items.Count; i++)
                {
                    if (cbl_sec1.Items[i].Selected == true)
                    {
                        if (section == "")
                        {
                            section = Convert.ToString(cbl_sec1.Items[i].Value);
                        }
                        else
                        {
                            section = section + "','" + Convert.ToString(cbl_sec1.Items[i].Value);
                        }
                        if (cbl_sec1.Items[i].Value == "Empty")
                        {
                            section = "";
                        }
                    }
                }
            }
            if (section.Trim() != "")
            {
                section = section + " ','";
            }
            string type = "";
            if (rb_Acad1.Checked == true)
            {
                type = "1";
            }
            else if (rb_Gend1.Checked == true)
            {
                type = "2";
            }
            Cbl_FbName1.Items.Clear();
            string FBname = "";
            if (rb_Acad1.Checked == true)
            {
                FBname = "select distinct FeedBackName from CO_FeedBackMaster where FeedBackType ='" + type + "' and CollegeCode  in ('" + college + "' ) and DegreeCode in ('" + branchcode + "') and Batch_Year in ('" + build + "') and semester in ('" + sem + "') and Section in ('" + section + "') ";
            }
            else if (rb_Gend1.Checked == true)
            {
                FBname = "select distinct FeedBackName from CO_FeedBackMaster where FeedBackType ='" + type + "' and CollegeCode in('" + college + "') ";
            }
            //select distinct FeedBackName from CO_FeedBackMaster where FeedBackType ='" + type + "' and CollegeCode  in ('" + college + "' ) and DegreeCode in ('" + branchcode + "') and Batch_Year in ('" + build + "') and semester in ('" + sem + "') and Section in ('" + section + "') 
            ds = d2.select_method_wo_parameter(FBname, "Text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                Cbl_FbName1.DataSource = ds;
                Cbl_FbName1.DataTextField = "FeedBackName";
                Cbl_FbName1.DataValueField = "FeedBackName";
                Cbl_FbName1.DataBind();
            }
            if (Cbl_FbName1.Items.Count > 0)
            {
                for (int row = 0; row < Cbl_FbName1.Items.Count; row++)
                {
                    Cbl_FbName1.Items[row].Selected = true;
                    Cb_FbName1.Checked = true;
                }
                Txt_FbName1.Text = "FeedBack(" + Cbl_FbName1.Items.Count + ")";
            }
            else
            {
                Txt_FbName1.Text = "--Select--";
            }
        }
        catch (Exception ex)
        {
        }
    }
    public void Cb_Subjecttype_CheckedChanged(object sender, EventArgs e)
    {
        try
        {
            int cout = 0;
            Txt_Subjecttype.Text = "--Select--";
            if (Cb_Subjecttype.Checked == true)
            {
                cout++;
                for (int i = 0; i < Cbl_Subjecttype.Items.Count; i++)
                {
                    Cbl_Subjecttype.Items[i].Selected = true;
                }
                Txt_Subjecttype.Text = "Subject Type(" + (Cbl_Subjecttype.Items.Count) + ")";
            }
            else
            {
                for (int i = 0; i < Cbl_Subjecttype.Items.Count; i++)
                {
                    Cbl_Subjecttype.Items[i].Selected = false;
                }
                Txt_Subjecttype.Text = "--Select--";
            }
            bind_subjecttype1();
        }
        catch (Exception ex)
        {
        }
    }
    public void Cbl_Subjecttype_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            int commcount = 0;
            Cb_Subjecttype.Checked = false;
            for (int i = 0; i < Cbl_Subjecttype.Items.Count; i++)
            {
                if (Cbl_Subjecttype.Items[i].Selected == true)
                {
                    commcount = commcount + 1;
                    Cb_Subjecttype.Checked = false;
                }
            }
            if (commcount > 0)
            {
                if (commcount == Cbl_Subjecttype.Items.Count)
                {
                    Cb_Subjecttype.Checked = true;
                }
                Txt_Subjecttype.Text = "Subject Type(" + commcount.ToString() + ")";
            }
            //bindhostelname();
            bind_subjecttype1();
        }
        catch (Exception ex)
        {
        }
    }
    public void bind_subjecttype()
    {
        try
        {
            Txt_Subjecttype.Text = "---Select---";
            Cb_Subjecttype.Checked = false;
            string stafftype = "";
            Cbl_Subjecttype.Items.Clear();
            ds.Clear();
            string branchcode = rs.GetSelectedItemsValueAsString(cbl_branch);
            string semester = rs.GetSelectedItemsValueAsString(cbl_sem);
            string batch = rs.GetSelectedItemsValueAsString(cbl_batch);
            if (!string.IsNullOrEmpty(branchcode) && !string.IsNullOrEmpty(semester) && !string.IsNullOrEmpty(batch))
            {
                stafftype = " select distinct subject_type from sub_sem u,syllabus_master y where u.syll_code = y.syll_code and y.degree_code in ('" + branchcode + "')  and semester in ('" + semester + "') and Batch_Year in ('" + batch + "')";
                //stafftype = "select distinct subject_type from sub_sem order by subject_type ";
                ds = d2.select_method_wo_parameter(stafftype, "Text");
                if (ds.Tables.Count > 0)
                {
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        Cbl_Subjecttype.DataSource = ds;
                        Cbl_Subjecttype.DataTextField = "subject_type";
                        Cbl_Subjecttype.DataValueField = "subject_type";
                        Cbl_Subjecttype.DataBind();
                        if (Cbl_Subjecttype.Items.Count > 0)
                        {
                            //for (int i = 0; i < Cbl_Subjecttype.Items.Count; i++)
                            //{
                            //    Cbl_Subjecttype.Items[i].Selected = true;
                            //}
                            //Txt_Subjecttype.Text = "Subject Type(" + Cbl_Subjecttype.Items.Count + ")";
                            //Cb_Subjecttype.Checked = true;
                            Txt_Subjecttype.Text = "---Select---";
                        }
                    }
                }
            }
        }
        catch (Exception)
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
            string dptname = "FeadBackMaster";
            string pagename = "FeedBack_Master.aspx";
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
    public void visibletrue1()
    {
        lbl_Batchyear1.Enabled = true;
        txt_batch1.Enabled = true;
        lbl_Degree1.Enabled = true;
        txt_degree1.Enabled = true;
        lbl_dpt1.Enabled = true;
        txt_branch1.Enabled = true;
        lbl_sem1.Enabled = true;
        txt_sem1.Enabled = true;
        lbl_Sec1.Enabled = true;
        txt_sec1.Enabled = true;
    }
    public void visiblefalse1()
    {
        lbl_Batchyear1.Enabled = false;
        txt_batch1.Enabled = false;
        lbl_Degree1.Enabled = false;
        txt_degree1.Enabled = false;
        lbl_dpt1.Enabled = false;
        txt_branch1.Enabled = false;
        lbl_sem1.Enabled = false;
        txt_sem1.Enabled = false;
        lbl_Sec1.Enabled = false;
        txt_sec1.Enabled = false;
    }
    public void visibletr()
    {
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
    }
    public void visiblefalse()
    {
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
    }
    protected void chk_includecommon_CheckedChanged(object sender, EventArgs e)
    {
        if (btn_Save.Text == "Update")
        {
            if (chk_includecommon.Checked == true)
            {
                txt_total_strength.Text = "";
                txt_fb_acr.Text = "";
                txt_running_series.Text = "";
                lbl_totl_strength.Visible = true;
                txt_total_strength.Visible = true;
                lbl_fb_acr.Visible = true;
                txt_fb_acr.Visible = true;
                lbl_run_sries.Visible = true;
                txt_running_series.Visible = true;
                lbl_fb_acr.Enabled = true;
                txt_fb_acr.Enabled = true;
                lbl_run_sries.Enabled = true;
                txt_running_series.Enabled = true;
                chk_random.Visible = true;//Added By Saranyadevi 19.2.2018
                chk_random.Checked = false;
                string activerow = FpSpread1.ActiveSheetView.ActiveRow.ToString();
                string activecol = FpSpread1.ActiveSheetView.ActiveColumn.ToString();
                if (rb_induvgual.Checked == true)
                {
                    string dept = FpSpread1.Sheets[0].Cells[Convert.ToInt32(activerow), 9].Tag.ToString();
                    string Section = FpSpread1.Sheets[0].Cells[Convert.ToInt32(activerow), 4].Note;
                    string sem = FpSpread1.Sheets[0].Cells[Convert.ToInt32(activerow), 3].Note;
                    string year = FpSpread1.Sheets[0].Cells[Convert.ToInt32(activerow), 1].Note;
                    string branch = FpSpread1.Sheets[0].Cells[Convert.ToInt32(activerow), 2].Note;
                    string FBName = FpSpread1.Sheets[0].Cells[Convert.ToInt32(activerow), 7].Text.ToString();
                    string uniq = d2.GetFunction(" select COUNT(app_No) from CO_feedbackMaster f,Registration r where f.DegreeCode =r.degree_code and f.Batch_Year =r.Batch_Year and r.Current_Semester =f.semester and r.Sections =f.Section and f.FeedBackName ='" + FBName + "' and CC=0 and DelFlag =0 and Exam_Flag <>'Debar' and f.Batch_Year='" + year + "' and  f.DegreeCode='" + branch + "' and f.semester='" + sem + "' and f.Section='" + Section + "' ");
                    if (uniq != "")
                    {
                        txt_total_strength.Text = Convert.ToString(uniq);
                    }
                }
                if (rb_common.Checked == true)
                {
                    if (chk_includecommon.Checked == true)
                    {
                        string FBName = FpSpread1.Sheets[0].Cells[Convert.ToInt32(activerow), 7].Text.ToString();
                        string uniq = d2.GetFunction(" select COUNT(app_No) from CO_feedbackMaster f,Registration r where f.DegreeCode =r.degree_code and f.Batch_Year =r.Batch_Year and r.Current_Semester =f.semester and r.Sections =f.Section and f.FeedBackName ='" + FBName + "' and CC=0 and DelFlag =0 and Exam_Flag <>'Debar'");
                        if (uniq != "")
                        {
                            txt_total_strength.Text = Convert.ToString(uniq);
                        }
                    }
                }
            }
        }
        else if (btn_Save.Text == "Save")
        {
            includecommon();
        }
    }
    public void includecommon()
    {
        if (chk_includecommon.Checked == false)
        {
            lbl_totl_strength.Visible = false;
            txt_total_strength.Visible = false;
            lbl_fb_acr.Visible = false;
            txt_fb_acr.Visible = false;
            lbl_run_sries.Visible = false;
            txt_running_series.Visible = false;
            txt_total_strength.Text = "";
            txt_fb_acr.Text = "";
            txt_running_series.Text = "";
            chk_random.Visible = false;//Added By Saranyadevi 19.2.2018
        }
        else
        {
            txt_total_strength.Text = "";
            txt_fb_acr.Text = "";
            txt_running_series.Text = "";
            lbl_totl_strength.Visible = true;
            txt_total_strength.Visible = true;
            lbl_fb_acr.Visible = true;
            txt_fb_acr.Visible = true;
            lbl_run_sries.Visible = true;
            txt_running_series.Visible = true;
            lbl_fb_acr.Enabled = true;
            txt_fb_acr.Enabled = true;
            lbl_run_sries.Enabled = true;
            txt_running_series.Enabled = true;
            chk_random.Visible = true;//Added By Saranyadevi 19.2.2018
            chk_random.Checked = false;
            string college = "";
            college = ddl_college.SelectedValue;
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
                            build = build + "','" + Convert.ToString(cbl_batch.Items[i].Value);
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
            string totalstrength = d2.GetFunction("select COUNT(app_no) from Registration where CC=0 and Exam_Flag  <>'Debar' and DelFlag =0 and degree_code in ('" + branchcode + "') and Current_Semester in ('" + sem + "') and isnull(Sections,'') in ('" + section + "') and Batch_Year in ('" + build + "')");
            txt_total_strength.Text = Convert.ToString(totalstrength);
        }
    }
    //    Static void Main()
    //{
    //    var excel = new Microsoft.Office.Interop.Excel.Application();
    //    var workbook = excel.Workbooks.Add(true);
    //    AddExcelSheet(dt1, workbook);
    //    AddExcelSheet(dt2, workbook);
    //    workbook.SaveAs(@"C:\MyExcelWorkBook2.xlsx");
    //    workbook.Close();
    //}
    //private static void AddExcelSheet(DataTable dt, Workbook wb)
    //{    
    //    Excel.Sheets sheets = wb.Sheets;
    //    Excel.Worksheet newSheet = sheets.Add();
    //    int iCol = 0;
    //    foreach (DataColumn c in dt.Columns)
    //    {
    //        iCol++;
    //        newSheet.Cells[1, iCol] = c.ColumnName;
    //    }
    //    int iRow = 0;
    //    foreach (DataRow r in dt.Rows)
    //    {
    //        iRow++;
    //        // add each row's cell data...
    //        iCol = 0;
    //        foreach (DataColumn c in dt.Columns)
    //        {
    //            iCol++;
    //            newSheet.Cells[iRow + 1, iCol] = r[c.ColumnName];
    //        }
    //}
    //}
    protected void ExportTable(DataTable dtEx)
    {
        Response.ClearContent();
        Response.Buffer = true;
        Response.AddHeader("content-disposition", string.Format("attachment; filename={0}", "Customers.xls"));
        Response.ContentType = "application/ms-excel";
        DataTable dt = dtEx;
        string str = string.Empty;
        foreach (DataColumn dtcol in dt.Columns)
        {
            Response.Write(str + dtcol.ColumnName);
            str = "\t";
        }
        Response.Write("\n");
        foreach (DataRow dr in dt.Rows)
        {
            str = "";
            for (int j = 0; j < dt.Columns.Count; j++)
            {
                Response.Write(str + Convert.ToString(dr[j]));
                str = "\t";
            }
            Response.Write("\n");
        }
        Response.End();
    }
    protected void img_close_Click(object sender, EventArgs e)
    {
        imgdiv2.Visible = false;
        btn_creatxl.Visible = false;
        btn_errorclose.Visible = true;
        txt_fb_acr.Text = "";
        txt_total_strength.Text = "";
        txt_running_series.Text = "";
        chk_includecommon.Checked = false;
        chk_random.Checked = false;
    }
    public void Cb_Subjecttype1_CheckedChanged(object sender, EventArgs e)
    {
        try
        {
            int cout = 0;
            Txt_Subjecttype1.Text = "--Select--";
            if (Cb_Subjecttype1.Checked == true)
            {
                cout++;
                for (int i = 0; i < Cbl_Subjecttype1.Items.Count; i++)
                {
                    Cbl_Subjecttype1.Items[i].Selected = true;
                }
                Txt_Subjecttype1.Text = "Subject Type(" + (Cbl_Subjecttype1.Items.Count) + ")";
            }
            else
            {
                for (int i = 0; i < Cbl_Subjecttype1.Items.Count; i++)
                {
                    Cbl_Subjecttype1.Items[i].Selected = false;
                }
                Txt_Subjecttype1.Text = "--Select--";
            }
        }
        catch (Exception ex)
        {
        }
    }
    public void Cbl_Subjecttype1_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            int commcount = 0;
            Cb_Subjecttype1.Checked = false;
            for (int i = 0; i < Cbl_Subjecttype1.Items.Count; i++)
            {
                if (Cbl_Subjecttype1.Items[i].Selected == true)
                {
                    commcount = commcount + 1;
                    Cb_Subjecttype1.Checked = false;
                }
            }
            if (commcount > 0)
            {
                if (commcount == Cbl_Subjecttype1.Items.Count)
                {
                    Cb_Subjecttype1.Checked = true;
                }
                Txt_Subjecttype1.Text = "Subject Type(" + commcount.ToString() + ")";
            }
            //bindhostelname();
        }
        catch (Exception ex)
        {
        }
    }
    public void bind_subjecttype1()
    {
        try
        {
            Txt_Subjecttype1.Text = "---Select---";
            Cb_Subjecttype1.Checked = false;
            //string collvalue = college;
            string stafftype = "";
            //if (collvalue == "---Select---")
            //{
            //    collvalue = Session["collegecode"].ToString();
            //}
            string subjecttyp = "";
            if (cbl_batch.Items.Count > 0)
            {
                for (int i = 0; i < Cbl_Subjecttype.Items.Count; i++)
                {
                    if (Cbl_Subjecttype.Items[i].Selected == true)
                    {
                        if (subjecttyp == "")
                        {
                            subjecttyp = Convert.ToString(Cbl_Subjecttype.Items[i].Value);
                        }
                        else
                        {
                            subjecttyp = subjecttyp + "','" + Convert.ToString(Cbl_Subjecttype.Items[i].Value);
                        }
                    }
                }
            }
            Cbl_Subjecttype1.Items.Clear();
            ds.Clear();
            stafftype = "select distinct subject_type from sub_sem where subject_type in('" + subjecttyp + "') order by subject_type ";
            //stafftype = " select distinct subject_type from sub_sem u,syllabus_master y where u.syll_code = y.syll_code and y.degree_code in ('" + branchcode1 + "')  and semester in ('" + semester + "') and Batch_Year in ('" + batch + "')";
            ds = d2.select_method_wo_parameter(stafftype, "Text");
            if (ds.Tables.Count > 0)
            {
                if (ds.Tables[0].Rows.Count > 0)
                {
                    Cbl_Subjecttype1.DataSource = ds;
                    Cbl_Subjecttype1.DataTextField = "subject_type";
                    Cbl_Subjecttype1.DataValueField = "subject_type";
                    Cbl_Subjecttype1.DataBind();
                    if (Cbl_Subjecttype1.Items.Count > 0)
                    {
                        Txt_Subjecttype1.Text = "---Select---";
                    }
                    if (Cbl_Subjecttype1.Items.Count > 0)
                    {
                        //for (int i = 0; i < Cbl_Subjecttype1.Items.Count; i++)
                        //{
                        //    Cbl_Subjecttype1.Items[i].Selected = true;
                        //}
                        Txt_Subjecttype1.Text = "Subject Type(" + Cbl_Subjecttype1.Items.Count + ")";
                        //Cb_Subjecttype1.Checked = true;
                        Txt_Subjecttype1.Text = "---Select---";
                    }
                }
            }
        }
        catch (Exception)
        {
        }
    }
    public void cb_optional_CheckedChanged(object sender, EventArgs e)
    {
        if (cb_optional.Checked == true)
        {
            Txt_Subjecttype1.Enabled = true;
            bind_subjecttype1();
        }
        else
        {
            Txt_Subjecttype1.Enabled = false;
        }
    }
    protected void cb_IsGeneral_CheckedChanged(object sender, EventArgs e)
    {
        if (cb_IsGeneral.Checked)
        {
            lbl_Subject_Type.Visible = false;
            Txt_Subjecttype.Visible = false;
            Panel_Subjecttype.Visible = false;
            cb_optional.Visible = false;
            Txt_Subjecttype1.Visible = false;
            Panel_Subjecttype1.Visible = false;
        }
        else
        {
            lbl_Subject_Type.Visible = true;
            Txt_Subjecttype.Visible = true;
            Panel_Subjecttype.Visible = true;
            cb_optional.Visible = true;
            Panel_Subjecttype1.Visible = true;
            Txt_Subjecttype1.Visible = true;
        }
    }


    //Added by SaranyaDevi 19.2.2018

    protected void chk_random_CheckedChanged(object sender, EventArgs e)
    {
        try
        {
            if (chk_random.Checked == true)
            {
                lbl_fb_acr.Enabled = false;
                txt_fb_acr.Enabled = false;
                lbl_run_sries.Enabled = false;
                txt_running_series.Enabled = false;
            }
            else
            {
                lbl_fb_acr.Enabled = true;
                txt_fb_acr.Enabled = true;
                lbl_run_sries.Enabled = true;
                txt_running_series.Enabled = true;
            }
        }
        catch
        {

        }

    }

   

    public List<int> RandomFunction(int count)
    {
        List<int> randomNumbers = new List<int>();
        Random rnd = new Random();
        for (int i = 0; i < count; i++)
        {
            int number;
            do number = rnd.Next(1,999999);
            while (randomNumbers.Contains(number));

            randomNumbers.Add(number);
        }
        return randomNumbers;
    }
}
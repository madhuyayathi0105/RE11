using System;
using System.Web.UI.WebControls;
using System.Data;
using System.Collections;
using System.Drawing;
public partial class FeedBack_Question_Master : System.Web.UI.Page
{
    string usercode = string.Empty;
    string collegecode1 = string.Empty;
    string collegecode = string.Empty;
    string singleuser = string.Empty;
    string group_user = string.Empty;
    Boolean cellclick = false;
    string selectQuery = "";
    DAccess2 d2 = new DAccess2();
    DataSet ds = new DataSet();
    DataSet ds1 = new DataSet();
    ReuasableMethods rs = new ReuasableMethods();
    Hashtable hat = new Hashtable();
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
            bindddlclg();
            bindclg();
            BindBatch();
            BindDegree();
            bindbranch();
            bindsem();
            bindsec();
            BindBatch1();
            BindDegree1();
            bindbranch1();
            bindsem1();
            bindsec1();
            FbName1();
            FBName2();
            bind_subjecttype();
        }
    }
    public void FbName1()
    {
        try
        {
            ds.Clear();
            Txt_FbName1.Text = "--Select--";
            Cb_FbName1.Checked = false;
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
                            build = build + "','" + Convert.ToString(cbl_batch.Items[i].Value);
                        }
                    }
                }
            }
            string branchcode = "";
            if (cbl_branch1.Items.Count > 0)
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
            string type = "";
            if (rb_Acad.Checked == true)
            {
                type = "1";
            }
            else if (rb_Gend.Checked == true)
            {
                type = "2";
            }
            Cbl_FbName1.Items.Clear();
            string FBName = "";
            if (rb_Acad.Checked == true)
            {
                //FBName = "select distinct FeedBackName  from CO_FeedBackMaster where FeedBackType ='" + type + "' and CollegeCode in ('" + college_cd + "')  and DegreeCode in ('" + branchcode + "') and Batch_Year in ('" + build + "') and semester in ('" + sem + "') and Section in ('" + section + "') ";
                FBName = "select distinct FeedBackName from CO_FeedBackMaster where FeedBackType ='" + type + "' and CollegeCode  in ('" + college_cd + "' ) and DegreeCode in ('" + branchcode + "') and Batch_Year in ('" + build + "') and semester in ('" + sem + "') and Section in ('" + section + "') ";
            }
            else if (rb_Gend.Checked == true)
            {
                FBName = "select distinct FeedBackName from CO_FeedBackMaster where FeedBackType ='" + type + "' and CollegeCode in ('" + college_cd + "')  ";
            }
            //   string clgname = "select FeedBackMasterPK ,FeedBackName from  CO_FeedBackMaster where CollegeCode='"+collegecode1+"'";
            ds = d2.select_method_wo_parameter(FBName, "Text");
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
                Cb_FbName1.Checked = false;
                Txt_FbName1.Text = "--Select--";
            }
        }
        catch (Exception ex)
        {
        }
    }
    public void FBName2()
    {
        try
        {
            Cb_FbName2.Checked = false;
            txt_Fbname2.Text = "--Select--";
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
            string college = "";
            college = "" + ddl_college.SelectedItem.Value.ToString() + "";
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
            string type = "";
            if (rb_Acad1.Checked == true)
            {
                type = "1";
            }
            else if (rb_Gend1.Checked == true)
            {
                type = "2";
            }
            string selqry = "";
            if (rb_Acad1.Checked == true)
            {
                selqry = "select distinct FeedBackName  from CO_FeedBackMaster where FeedBackType ='" + type + "' and CollegeCode in ('" + college + "')  and DegreeCode in ('" + branchcode + "') and Batch_Year in ('" + build + "') and semester in ('" + sem + "') and Section in ('" + section + "') ";
            }
            else if (rb_Gend1.Checked == true)
            {
                selqry = "select distinct FeedBackName  from CO_FeedBackMaster where FeedBackType ='" + type + "' and CollegeCode in ('" + college + "') ";
            }
            ds = d2.select_method_wo_parameter(selqry, "Text");
            Cbl_FbName2.Items.Clear();
            txt_Fbname2.Text = "---Select---";
            Cb_FbName2.Checked = false;
            if (ds.Tables[0].Rows.Count > 0)
            {
                Cbl_FbName2.DataSource = ds;
                Cbl_FbName2.DataTextField = "FeedBackName";
                Cbl_FbName2.DataValueField = "FeedBackName";
                Cbl_FbName2.DataBind();
                if (Cbl_FbName2.Items.Count > 0)
                {
                    for (int row = 0; row < Cbl_FbName2.Items.Count; row++)
                    {
                        Cbl_FbName2.Items[row].Selected = true;
                        Cb_FbName2.Checked = true;
                    }
                    txt_Fbname2.Text = "FeedBackName(" + Cbl_FbName2.Items.Count + ")";
                }
            }
            else
            {
                Cb_FbName2.Checked = false;
                txt_Fbname2.Text = "--Select--";
            }
        }
        catch (Exception ex)
        {
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
            BindBatch();
            BindDegree();
            bindbranch();
            bindsem();
            bindsec();
            FbName1();
        }
        catch
        {
        }
    }
    public void Cbl_college_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            int commcount = 0;
            txt_sec.Text = "--Select--";
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
            BindBatch();
            BindDegree();
            bindbranch();
            bindsem();
            bindsec();
            FbName1();
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
            FbName1();
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
            FbName1();
        }
        catch
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
            FbName1();
            // bindhostelname();
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
            FbName1();
        }
        catch
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
            FbName1();
        }
        catch
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
                txt_branch.Text = "Branch(" + commcount.ToString() + ")";
            }
            bindsem();
            bindsec();
            FbName1();
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
            FbName1();
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
                    cb_sem.Checked = true;
                }
                txt_sem.Text = "Semester(" + commcount.ToString() + ")";
            }
            bindsec();
            FbName1();
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
            string query = "select distinct Current_Semester from Registration where degree_code in (" + branch + ") and Batch_Year in (" + batch + ") and college_code in ('" + college_cd + "') and CC=0 and DelFlag =0 and Exam_Flag <>'debar' order by Current_Semester";
            ds = d2.select_method_wo_parameter(query, "Text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                cbl_sem.DataSource = ds;
                cbl_sem.DataTextField = "Current_Semester";
                cbl_sem.DataBind();
                if (cbl_sem.Items.Count > 0)
                {
                    for (int row = 0; row < cbl_sem.Items.Count; row++)
                    {
                        cbl_sem.Items[row].Selected = true;
                        cb_sem.Checked = true;
                    }
                    txt_sem.Text = "Semester(" + cbl_sem.Items.Count + ")";
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
        FbName1();
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
            FbName1();
        }
        catch
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
        }
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
            //Txt_college.Text = "College(" + ddl_college.Items.Count + ")";
            //else
            //{
            //    Txt_college.Text = "--Select--";
            //}
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
            FBName2();
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
            college = "" + ddl_college.SelectedItem.Value.ToString() + "";
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
                    //txt_degree1.Text = "--Select--";
                    //txtbranch.Text = "--Select--";
                    //chklstbranch.ClearSelection();
                    //chkbranch.Checked = false;
                }
                txt_degree1.Text = "--Select--";
            }
            bindbranch1();
            bindsem1();
            bindsec1();
            // bindhostelname();
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
            FBName2();
            bind_subjecttype();
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
            college = "" + ddl_college.SelectedItem.Value.ToString() + "";
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
            FBName2();
            bind_subjecttype();
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
            college = "" + ddl_college.SelectedItem.Value.ToString() + "";
            string query = "";
            if (course_id != "")
            {
                ds.Clear();
                query = "    select distinct degree.degree_code,department.dept_name,department.dept_code from degree,department,course where course.course_id=degree.course_id and department.dept_code=degree.dept_code and course.college_code = degree.college_code and department.college_code = degree.college_code and degree.course_id in(" + course_id + ") and degree.college_code in ('" + college + "')";
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
            bind_subjecttype();
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
            FBName2();
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
        college = "" + ddl_college.SelectedItem.Value.ToString() + "";
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
            string query = "select distinct Current_Semester from Registration where degree_code in (" + branch + ") and Batch_Year in (" + batch + ") and college_code in ('" + college + "') and CC=0 and DelFlag =0 and Exam_Flag <>'debar' order by Current_Semester";
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
            FBName2();
        }
        catch (Exception ex)
        {
        }
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
    protected void lb3_Click(object sender, EventArgs e)
    {
        Session.Abandon();
        Session.Clear();
        Session.RemoveAll();
        System.Web.Security.FormsAuthentication.SignOut();
        Response.Redirect("~/Default.aspx", false);
    }
    protected void Cb_FbName2_CheckedChanged(object sender, EventArgs e)
    {
        if (Cb_FbName2.Checked == true)
        {
            for (int i = 0; i < Cbl_FbName2.Items.Count; i++)
            {
                Cbl_FbName2.Items[i].Selected = true;
            }
            txt_Fbname2.Text = "FeedBack Name(" + (Cbl_FbName2.Items.Count) + ")";
        }
        else
        {
            for (int i = 0; i < Cbl_FbName2.Items.Count; i++)
            {
                Cbl_FbName2.Items[i].Selected = false;
            }
            txt_Fbname2.Text = "--Select--";
        }
    }
    protected void Cbl_FbName2_SelectedIndexChanged(object sender, EventArgs e)
    {
        txt_Fbname2.Text = "--Select--";
        Cb_FbName2.Checked = false;
        int commcount = 0;
        for (int i = 0; i < Cbl_FbName2.Items.Count; i++)
        {
            if (Cbl_FbName2.Items[i].Selected == true)
            {
                commcount = commcount + 1;
            }
        }
        if (commcount > 0)
        {
            txt_Fbname2.Text = "FeedBack Name(" + commcount.ToString() + ")";
            if (commcount == Cbl_FbName2.Items.Count)
            {
                Cb_FbName2.Checked = true;
            }
        }
        bind_subjecttype();
    }
    protected void Cb_FbName1_CheckedChanged(object sender, EventArgs e)
    {
        if (Cb_FbName1.Checked == true)
        {
            for (int i = 0; i < Cbl_FbName1.Items.Count; i++)
            {
                Cbl_FbName1.Items[i].Selected = true;
            }
            Txt_FbName1.Text = "FeedBack Name(" + (Cbl_FbName1.Items.Count) + ")";
        }
        else
        {
            for (int i = 0; i < Cbl_FbName1.Items.Count; i++)
            {
                Cbl_FbName1.Items[i].Selected = false;
            }
            Txt_FbName1.Text = "--Select--";
        }
        bind_subjecttype();
    }
    protected void Cbl_FbName1_SelectedIndexChanged(object sender, EventArgs e)
    {
        Txt_FbName1.Text = "--Select--";
        Cb_FbName1.Checked = false;
        int commcount = 0;
        for (int i = 0; i < Cbl_FbName1.Items.Count; i++)
        {
            if (Cbl_FbName1.Items[i].Selected == true)
            {
                commcount = commcount + 1;
            }
        }
        if (commcount > 0)
        {
            Txt_FbName1.Text = "FeedBack Name(" + commcount.ToString() + ")";
            if (commcount == Cbl_FbName1.Items.Count)
            {
                Cb_FbName1.Checked = true;
            }
        }
    }
    protected void ddl_college_SelectedIndexChanged(object sender, EventArgs e)
    {
        BindBatch1();
        BindDegree1();
        bindbranch1();
        bindsem1();
        bindsec1();
        FBName2();
    }
    protected void btnAdd_Click(object sender, EventArgs e)
    {
        rb_Acad1.Checked = true;
        rb_Gend1.Checked = false;
        rdbdescriptive.Visible = false;
        rdbobjective.Visible = false;
        visibletrue1();
        // btnsearch_Click(sender, e);
        bindddlclg();
        BindBatch1();
        BindDegree1();
        bindbranch1();
        bindsem1();
        bindsec1();
        FBName2();
        addnew.Visible = true;
        FpSpread2.Visible = false;
        btn_save.Visible = false;
        btn_exit.Visible = false;
    }
    protected void rb_Acad_CheckedChanged(object sender, EventArgs e)
    {
        Acad.Visible = true;
        visibletr();
        FbName1();
        FpSpread1.Visible = false;
        rptprint1.Visible = false;
    }
    protected void rb_Gend_CheckedChanged(object sender, EventArgs e)
    {
        Acad.Visible = true;
        visiblefalse();
        FbName1();
        FpSpread1.Visible = false;
        rptprint1.Visible = false;
    }
    protected void rb_Acad1_CheckedChanged(object sender, EventArgs e)
    {
        Acad1.Visible = true;
        visibletrue1();
        FBName2();
        if (rb_Acad1.Checked == true)
        {
            rdbobjective.Visible = false;
            rdbdescriptive.Visible = false;

        }
        btnsearch_Click(sender, e);
    }
    protected void rb_Gend1_CheckedChanged(object sender, EventArgs e)
    {
        Acad1.Visible = true;
        visiblefalse1();
        FBName2();
        if (rb_Gend1.Checked == true)
        {
            rdbobjective.Visible = true;
            rdbdescriptive.Visible = true;
            rdbobjective.Checked = true;
            rdbdescriptive.Checked = false;


        }
        btnsearch_Click(sender, e);
    }
    //public void QuestionMaster()
    //{
    //    div1.Visible = true;
    //    rptprint1.Visible = true;
    //    FpSpread1.Sheets[0].RowCount = 0;
    //    FpSpread1.Sheets[0].ColumnCount = 0;
    //    FpSpread1.CommandBar.Visible = false;
    //    FpSpread1.Sheets[0].AutoPostBack = true;
    //    FpSpread1.Sheets[0].ColumnHeader.RowCount = 1;
    //    FpSpread1.Sheets[0].RowHeader.Visible = false;
    //    FpSpread1.Sheets[0].ColumnCount = 3;
    //    FarPoint.Web.Spread.StyleInfo darkstyle = new FarPoint.Web.Spread.StyleInfo();
    //    darkstyle.BackColor = ColorTranslator.FromHtml("#0CA6CA");
    //    darkstyle.ForeColor = Color.White;
    //    FpSpread1.ActiveSheetView.ColumnHeader.DefaultStyle = darkstyle;
    //    FpSpread1.Visible = true;
    //    FpSpread1.SaveChanges();
    //    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 0].Text = "S.No";
    //    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 0].Font.Bold = true;
    //    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 0].HorizontalAlign = HorizontalAlign.Center;
    //    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 0].Font.Name = "Book Antiqua";
    //    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 0].Font.Size = FontUnit.Medium;
    //    FpSpread1.Sheets[0].Columns[0].HorizontalAlign = HorizontalAlign.Center;
    //    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 1].Text = "FeedBack Name";
    //    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 1].Font.Bold = true;
    //    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 1].Font.Name = "Book Antiqua";
    //    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 1].Font.Size = FontUnit.Medium;
    //    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 1].HorizontalAlign = HorizontalAlign.Center;
    //    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 2].Text = "Questions";
    //    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 2].Font.Bold = true;
    //    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 2].Font.Name = "Book Antiqua";
    //    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 2].Font.Size = FontUnit.Medium;
    //    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 2].HorizontalAlign = HorizontalAlign.Center;
    //    FpSpread1.Sheets[0].ColumnHeader.Columns[0].Width = 20;
    //    FpSpread1.Sheets[0].ColumnHeader.Columns[1].Width = 144;
    //    FpSpread1.Sheets[0].ColumnHeader.Columns[2].Width = 228;
    //    ////ds.Clear();
    //    string activerow = FpSpread2.ActiveSheetView.ActiveRow.ToString();
    //    string activecol = FpSpread2.ActiveSheetView.ActiveColumn.ToString();
    //    string selqry = "";
    //    string value = "0";
    //    for (int i = 1; i < FpSpread2.Rows.Count; i++)
    //    {
    //        int fbvalue = Convert.ToInt32(FpSpread2.Sheets[0].Cells[i, 3].Value);
    //        if (fbvalue == 1)
    //        {
    //            if (value == "0")
    //            {
    //                value = Convert.ToString(FpSpread2.Sheets[0].Cells[i, 2].Tag);
    //            }
    //            else
    //            {
    //                value = value + "','" + Convert.ToInt32(FpSpread2.Sheets[0].Cells[Convert.ToInt32(i), 2].Tag) + "";
    //            }
    //        }
    //    }
    //    string type = "";
    //    //for (int i = 0; i < FpSpread2.Rows.Count; i++)
    //    //{
    //    //    type = Convert.ToString(FpSpread2.Sheets[0].Cells[(activerow), 3].Tag);
    //    //}
    //    if (rb_Acad1.Checked == true)
    //    {
    //        type = "1";
    //    }
    //    else if (rb_Gend1.Checked == true)
    //    {
    //        type = "2";
    //    }
    //    selqry = "select HeaderCode,TextVal as HeaderName, FeedBackName,FeedBackMasterPK,Question,QuestType,QuestionMasterPK from CO_QuestionMaster q,TextValTable t,CO_FeedBackMaster F where t.TextCode=q.HeaderCode and F.FeedBackType =q.QuestType and QuestionMasterPK in('" + value + "' ) and QuestType ='" + type + "'";
    //    ds = d2.select_method_wo_parameter(selqry, "Text");
    //    if (ds.Tables.Count > 0)
    //    {
    //        if (ds.Tables[0].Rows.Count > 0)
    //        {
    //            FpSpread1.Sheets[0].Rows.Count = ds.Tables[0].Rows.Count;
    //            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
    //            {
    //                FpSpread1.Sheets[0].Cells[i, 0].Text = Convert.ToString(i + 1);
    //                FpSpread1.Sheets[0].Cells[i, 1].Text = ds.Tables[0].Rows[i]["HeaderName"].ToString();
    //                FpSpread1.Sheets[0].Cells[i, 2].Text = ds.Tables[0].Rows[i]["Question"].ToString();
    //            }
    //        }
    //        else
    //        {
    //            imgdiv2.Visible = true;
    //            lbl_alert1.Text = "No Records Found";
    //            FpSpread1.Visible = false;
    //            div1.Visible = false;
    //            rptprint1.Visible = false;
    //        }
    //    }
    //    else
    //    {
    //        imgdiv2.Visible = true;
    //        lbl_alert1.Text = "No Records Found";
    //        FpSpread1.Visible = false;
    //        rptprint1.Visible = false;
    //        div1.Visible = false;
    //    }
    //    FpSpread1.Sheets[0].PageSize = ds.Tables[0].Rows.Count;
    //    FpSpread1.SaveChanges();
    //}
    protected void btn_errorclose_Click(object sender, EventArgs e)
    {
        imgdiv2.Visible = false;
    }
    protected void btnsearch_Click(object sender, EventArgs e)
    {
        try
        {
            string type = "";
            string objdes = string.Empty;
            if (rb_Acad1.Checked == true)
            {
                type = "1";
            }
            else if (rb_Gend1.Checked == true)
            {
                type = "2";
                if (rdbobjective.Checked == true)
                {
                    objdes = "1";
                }
                if (rdbdescriptive.Checked == true)
                {
                    objdes = "2";
                }
            }

            FpSpread2.SaveChanges();
            FpSpread2.Sheets[0].RowCount = 0;
            FpSpread2.Sheets[0].ColumnCount = 0;
            FpSpread2.CommandBar.Visible = false;
            FpSpread2.Sheets[0].AutoPostBack = false;
            FpSpread2.Sheets[0].ColumnHeader.RowCount = 1;
            FpSpread2.Sheets[0].RowHeader.Visible = false;
            FpSpread2.Sheets[0].ColumnCount = 4;
            FarPoint.Web.Spread.StyleInfo darkstyle = new FarPoint.Web.Spread.StyleInfo();
            darkstyle.BackColor = ColorTranslator.FromHtml("#0CA6CA");
            darkstyle.ForeColor = Color.White;
            FpSpread2.ActiveSheetView.ColumnHeader.DefaultStyle = darkstyle;
            FpSpread2.Visible = true;
            btn_save.Visible = true;
            btn_exit.Visible = true;
            FpSpread2.Sheets[0].ColumnHeader.Cells[0, 0].Text = "S.No";
            FpSpread2.Sheets[0].ColumnHeader.Cells[0, 0].Font.Bold = true;
            FpSpread2.Sheets[0].ColumnHeader.Cells[0, 0].HorizontalAlign = HorizontalAlign.Center;
            FpSpread2.Sheets[0].ColumnHeader.Cells[0, 0].Font.Name = "Book Antiqua";
            FpSpread2.Sheets[0].ColumnHeader.Cells[0, 0].Font.Size = FontUnit.Medium;
            FpSpread2.Sheets[0].Columns[0].HorizontalAlign = HorizontalAlign.Center;
            FpSpread2.Sheets[0].ColumnHeader.Cells[0, 1].Text = "Header";
            FpSpread2.Sheets[0].ColumnHeader.Cells[0, 1].Font.Bold = true;
            FpSpread2.Sheets[0].ColumnHeader.Cells[0, 1].Font.Name = "Book Antiqua";
            FpSpread2.Sheets[0].ColumnHeader.Cells[0, 1].Font.Size = FontUnit.Medium;
            FpSpread2.Sheets[0].ColumnHeader.Cells[0, 1].HorizontalAlign = HorizontalAlign.Center;
            FpSpread2.Sheets[0].ColumnHeader.Cells[0, 2].Text = "Questions";
            FpSpread2.Sheets[0].ColumnHeader.Cells[0, 2].Font.Bold = true;
            FpSpread2.Sheets[0].ColumnHeader.Cells[0, 2].Font.Name = "Book Antiqua";
            FpSpread2.Sheets[0].ColumnHeader.Cells[0, 2].Font.Size = FontUnit.Medium;
            FpSpread2.Sheets[0].ColumnHeader.Cells[0, 2].HorizontalAlign = HorizontalAlign.Center;
            FpSpread2.Sheets[0].ColumnHeader.Cells[0, 3].Text = "Select";
            FpSpread2.Sheets[0].ColumnHeader.Cells[0, 3].Font.Bold = true;
            FpSpread2.Sheets[0].ColumnHeader.Cells[0, 3].Font.Name = "Book Antiqua";
            FpSpread2.Sheets[0].ColumnHeader.Cells[0, 3].Font.Size = FontUnit.Medium;
            FpSpread2.Sheets[0].ColumnHeader.Cells[0, 3].HorizontalAlign = HorizontalAlign.Center;
            FpSpread2.Sheets[0].ColumnHeader.Columns[0].Width = 59;
            FpSpread2.Sheets[0].ColumnHeader.Columns[1].Width = 244;
            FpSpread2.Sheets[0].ColumnHeader.Columns[2].Width = 257;
            FpSpread2.Sheets[0].ColumnHeader.Columns[3].Width = 59;
            FarPoint.Web.Spread.CheckBoxCellType chkall = new FarPoint.Web.Spread.CheckBoxCellType();
            chkall.AutoPostBack = true;
            FarPoint.Web.Spread.CheckBoxCellType chk = new FarPoint.Web.Spread.CheckBoxCellType();
            chk.AutoPostBack = false;

            string feedbackname = rs.GetSelectedItemsValue(Cbl_FbName2);
            string SubjectType = rs.GetSelectedItemsValue(Cbl_Subjecttype);
            string college = "";
            college = "" + ddl_college.SelectedItem.Value.ToString() + "";
            //string selqry = "select HeaderCode,TextVal as HeaderName,Question,QuestionMasterPK , QuestType from CO_QuestionMaster q,TextValTable t where t.TextCode=q.HeaderCode and QuestType ='" + type + "'  and CollegeCode in ('" + college + "')   ";
            //string selqry = " select distinct HeaderCode,TextVal as HeaderName,Question,isnull(fb.SubjectType,'')SubjectType, QuestionMasterPK , QuestType from CO_QuestionMaster q,CO_FeedBackQuestions fb,TextValTable t where q.QuestionMasterPK=fb.QuestionMasterFK and t.TextCode=q.HeaderCode and QuestType ='" + type + "'  and CollegeCode in ('" + college + "') ";
            //string selqry = " select distinct HeaderCode,TextVal as HeaderName,Question,isnull(fb.SubjectType,'')SubjectType, QuestionMasterPK , QuestType from TextValTable t,CO_QuestionMaster q left join CO_FeedBackQuestions fb on q.QuestionMasterPK=fb.QuestionMasterFK where t.TextCode=q.HeaderCode and QuestType ='" + type + "'  and CollegeCode in ('" + college + "')";
            //selqry = selqry + " select distinct QuestionMasterFK,fm.IsSubjectType from CO_FeedBackQuestions fb,CO_FeedBackMaster fm where fb.FeedBackMasterFK =fm.FeedBackMasterPK and fm.FeedBackName in ('" + feedbackname + "')";
            //and F.FeedBackMasterPK in ('"+feedbackname+"')
            //ds = d2.select_method_wo_parameter(selqry, "Text");
            if (objdes != "2")
            {
                hat.Add("QuestionType", type);
                hat.Add("collegecode", college);
                hat.Add("feedbackName", feedbackname);
                ds.Clear();
                ds = d2.select_method("FeedbackQuestion", hat, "sp");
            }
            if (objdes == "2")
            {
                hat.Add("QuestionType", type);
                hat.Add("collegecode", college);
                hat.Add("feedbackName", feedbackname);
                hat.Add("optques", objdes);
                ds.Clear();
                ds = d2.select_method("FeedbackQuestionDesc", hat, "sp");
            }
            DataView dv = new DataView();
            if (ds.Tables.Count > 0)
            {
                if (ds.Tables[0].Rows.Count > 0)
                {
                    FpSpread2.Rows.Count++;
                    FpSpread2.Sheets[0].Cells[0, 3].CellType = chkall;
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        ds.Tables[1].DefaultView.RowFilter = "QuestionMasterFK='" + Convert.ToString(ds.Tables[0].Rows[i]["QuestionMasterPK"]) + "'";
                        dv = ds.Tables[1].DefaultView;
                        FpSpread2.Rows.Count++;
                        FpSpread2.Sheets[0].Cells[FpSpread2.Rows.Count - 1, 0].Text = Convert.ToString(i + 1);
                        FpSpread2.Sheets[0].Cells[FpSpread2.Rows.Count - 1, 1].Text = ds.Tables[0].Rows[i]["HeaderName"].ToString();
                        //FpSpread2.Sheets[0].Cells[FpSpread2.Rows.Count - 1, 2].Text = Convert.ToString(ds.Tables[0].Rows[i]["Question"]);

                        FpSpread2.Sheets[0].Cells[FpSpread2.Rows.Count - 1, 2].Text = Convert.ToString(ds.Tables[0].Rows[i]["SubjectType"]) == "" ? Convert.ToString(ds.Tables[0].Rows[i]["Question"]) : Convert.ToString(ds.Tables[0].Rows[i]["Question"]) + " - " + Convert.ToString(ds.Tables[0].Rows[i]["SubjectType"]);
                        //ds.Tables[0].Rows[i]["Question"].ToString() + " - " + Convert.ToString(ds.Tables[0].Rows[i]["SubjectType"]);
                        FpSpread2.Sheets[0].Cells[FpSpread2.Rows.Count - 1, 2].Tag = ds.Tables[0].Rows[i]["QuestionMasterPK"].ToString();
                        FpSpread2.Sheets[0].Cells[FpSpread2.Rows.Count - 1, 3].CellType = chk;
                        if (dv.Count > 0)
                        {
                            if (Convert.ToString(ds.Tables[0].Rows[i]["QuestionMasterPK"]) == Convert.ToString(dv[0]["QuestionMasterFK"]))
                            {
                                string IsSubjectType = Convert.ToString(dv[0]["IsSubjectType"]);
                                if (IsSubjectType == "1" || IsSubjectType == "True")
                                {
                                    if (Convert.ToString(ds.Tables[0].Rows[i]["SubjectType"]) == SubjectType)
                                    {
                                        FpSpread2.Sheets[0].Cells[FpSpread2.Rows.Count - 1, 3].Value = 1;
                                    }
                                }
                                else
                                    FpSpread2.Sheets[0].Cells[FpSpread2.Rows.Count - 1, 3].Value = 1;
                            }
                        }
                        FpSpread2.Sheets[0].Cells[FpSpread2.Rows.Count - 1, 3].Tag = ds.Tables[0].Rows[i]["QuestType"].ToString();
                    }
                }
                else
                {
                    imgdiv2.Visible = true;
                    lbl_alert1.Text = "No Records Found";
                    FpSpread2.Visible = false;
                    // div1.Visible = false;
                    rptprint1.Visible = false;
                    btn_save.Visible = false;
                    btn_exit.Visible = false;
                }
            }
            else
            {
                imgdiv2.Visible = true;
                lbl_alert1.Text = "No Records Found";
                FpSpread2.Visible = false;
                // div1.Visible = false;
                rptprint1.Visible = false;
                btn_save.Visible = false;
                btn_exit.Visible = false;
            }
            FpSpread2.Sheets[0].PageSize = FpSpread2.Sheets[0].RowCount;
        }
        catch
        {
        }
    }
    protected void FpSpread1_OnButtonCommand(object sender, EventArgs e)
    {
        try
        {
            FpSpread1.SaveChanges();
            int activerow = FpSpread1.ActiveSheetView.ActiveRow;
            int activecol = FpSpread1.ActiveSheetView.ActiveColumn;
            if (activerow != -1 && activecol != -1)
            {
                int checkval = Convert.ToInt32(FpSpread1.Sheets[0].Cells[0, 1].Value);
                if (checkval == 1)
                {
                    for (int i = 0; i < FpSpread1.Sheets[0].RowCount; i++)
                    {
                        FpSpread1.Sheets[0].Cells[i, 1].Value = 1;
                    }
                }
                else if (checkval == 0)
                {
                    for (int i = 0; i < FpSpread1.Sheets[0].RowCount; i++)
                    {
                        FpSpread1.Sheets[0].Cells[i, 1].Value = 0;
                    }
                }
            }
        }
        catch
        {
        }
    }
    protected void FpSpread2_OnButtonCommand(object sender, EventArgs e)
    {
        FpSpread2.SaveChanges();
        int activerow = FpSpread2.ActiveSheetView.ActiveRow;
        int activecol = FpSpread2.ActiveSheetView.ActiveColumn;
        if (activerow != -1 && activecol != -1)
        {
            int checkval = Convert.ToInt32(FpSpread2.Sheets[0].Cells[0, 3].Value);
            if (checkval == 1)
            {
                for (int i = 0; i < FpSpread2.Sheets[0].RowCount; i++)
                {
                    FpSpread2.Sheets[0].Cells[i, 3].Value = 1;
                }
            }
            else if (checkval == 0)
            {
                for (int i = 0; i < FpSpread2.Sheets[0].RowCount; i++)
                {
                    FpSpread2.Sheets[0].Cells[i, 3].Value = 0;
                }
            }
        }
    }
    protected void btn_savequstion_Click(object sender, EventArgs e)
    {
        try
        {
            int value = 0;
            FpSpread2.SaveChanges();
            string FBnameCode = "";
            string optdesc = string.Empty;
            string type = "";
            string activerow = FpSpread2.ActiveSheetView.ActiveRow.ToString();
            string activecol = FpSpread2.ActiveSheetView.ActiveColumn.ToString();
            for (int i = 0; i < Cbl_FbName2.Items.Count; i++)
            {
                if (Cbl_FbName2.Items[i].Selected == true)
                {
                    if (FBnameCode == "")
                    {
                        FBnameCode = "" + Cbl_FbName2.Items[i].Text.ToString() + "";
                    }
                    else
                    {
                        FBnameCode = FBnameCode + "','" + Cbl_FbName2.Items[i].Text.ToString() + "";
                    }
                }
            }
            string college = "";
            college = "" + ddl_college.SelectedItem.Value.ToString() + "";
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

            if (rb_Acad1.Checked == true)
            {
                type = "1";
            }
            else if (rb_Gend1.Checked == true)
            {
                type = "2";
                if (rdbobjective.Checked == true)
                {
                    optdesc = "1";
                }
                if (rdbdescriptive.Checked == true)
                {
                    optdesc = "2";
                }
            }
            string selectquery = "";
            if (rb_Acad1.Checked == true)
            {
                selectquery = " select FeedBackMasterPK,isnull(IsSubjectType,0)IsSubjectType  from CO_FeedBackMaster where FeedBackType ='" + type + "' and FeedBackName in ('" + FBnameCode + "') and batch_year in ('" + build + "')  and degreeCode in ('" + branchcode + "')  and semester  in ('" + sem + "')  and section in ('" + section + "')  and collegecode in ('" + college + "') ";
            }
            else if (rb_Gend1.Checked == true)
            {
                selectquery = " select FeedBackMasterPK ,isnull(IsSubjectType,0)IsSubjectType from CO_FeedBackMaster where FeedBackType ='" + type + "' and FeedBackName in ('" + FBnameCode + "')  and collegecode in ('" + college + "') ";
            }
            ds.Clear();
            ds = d2.select_method_wo_parameter(selectquery, "Text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                for (int ro = 0; ro < ds.Tables[0].Rows.Count; ro++)
                {
                    string feedbackpk = Convert.ToString(ds.Tables[0].Rows[ro]["FeedBackMasterPK"]);
                    bool IsSubjectType = Convert.ToBoolean(ds.Tables[0].Rows[ro]["IsSubjectType"]);
                    string deleteQuestion = string.Empty;
                    string SubjectType = string.Empty;
                    if (!IsSubjectType)
                    {
                        deleteQuestion = "if exists (select * from CO_FeedBackQuestions where FeedBackMasterFK ='" + feedbackpk + "' and IsSubjectType='" + IsSubjectType + "' ) delete from CO_FeedBackQuestions where FeedBackMasterFK ='" + feedbackpk + "' and IsSubjectType='" + IsSubjectType + "' ";
                        int chkfb = d2.update_method_wo_parameter(deleteQuestion, "Text");
                    }
                    else { SubjectType = rs.GetSelectedItemsValue(Cbl_Subjecttype); }
                    for (int i = 1; i < FpSpread2.Rows.Count; i++)
                    {
                        int fbvalue = Convert.ToInt32(FpSpread2.Sheets[0].Cells[i, 3].Value);
                        if (fbvalue == 1)
                        {
                            value = Convert.ToInt32(FpSpread2.Sheets[0].Cells[Convert.ToInt32(i), 2].Tag);
                           // string inserquery = "if not exists(select * from CO_FeedBackQuestions where FeedBackMasterFK='" + feedbackpk + "' and QuestionMasterFK='" + value + "' and IsSubjectType='" + IsSubjectType + "' and SubjectType='" + SubjectType + "') insert into CO_FeedBackQuestions (FeedBackMasterFK,QuestionMasterFK,IsSubjectType,SubjectType) values ('" + feedbackpk + "','" + value + "','" + IsSubjectType + "','" + SubjectType + "')";
                            string inserquery = "if not exists(select * from CO_FeedBackQuestions where FeedBackMasterFK='" + feedbackpk + "' and QuestionMasterFK='" + value + "' and IsSubjectType='" + IsSubjectType + "' and SubjectType='" + SubjectType + "') insert into CO_FeedBackQuestions (FeedBackMasterFK,QuestionMasterFK,IsSubjectType,SubjectType,objdes) values ('" + feedbackpk + "','" + value + "','" + IsSubjectType + "','" + SubjectType + "','" + optdesc + "')";
                            int abd = d2.update_method_wo_parameter(inserquery, "Text");
                            imgdiv2.Visible = true;
                            lbl_alert1.Text = "Saved Successfully";
                        }
                    }
                }
            }
            //if(rb_Acad1.Checked==true )
            //{
            //    rb_Acad.Checked = true; 
            //}
            //else if (rb_Gend1.Checked ==true)
            //{
            //    rb_Gend1.Checked = true;
            //}
            btn_Search1_Click(sender, e);
            addnew.Visible = false;
        }
        catch
        {
        }
    }
    protected void btn_exit_Click(object sender, EventArgs e)
    {
        addnew.Visible = false;
    }
    protected void btndel_Click(object sender, EventArgs e)
    {
        imgdiv3.Visible = true;
        lbl_warning_alert.Visible = true;
        lbl_warning_alert.Text = "Are you sure you want delete";
    }
    public void btn_warningmsg_Click(object sender, EventArgs e)
    {
        imgdiv3.Visible = false;
        lbl_warning_alert.Visible = false;
        try
        {
            FpSpread1.SaveChanges();
            bool textfale = false;
            string value = "";
            for (int i = 0; i < FpSpread1.Rows.Count; i++)
            {
                if (Convert.ToInt32(FpSpread1.Sheets[0].Cells[i, 1].Value) == 1)
                {
                    string qmpk = FpSpread1.Sheets[0].Cells[i, 0].Tag.ToString();
                    string fbpk = FpSpread1.Sheets[0].Cells[i, 1].Tag.ToString();
                    if (value == "")
                    {
                        string questionpk = d2.GetFunction("select distinct QuestionMasterFK from CO_StudFeedBack where QuestionMasterFK ='" + qmpk + "' and FeedBackMasterFK='" + fbpk + "'");
                        if (questionpk == "" || questionpk == "0")
                        {
                            string sql = "delete from CO_FeedBackQuestions where FeedBackMasterFK ='" + fbpk + "' and QuestionMasterFK ='" + qmpk + "'";
                            int qry = d2.update_method_wo_parameter(sql, "Text");
                            if (qry > 0)
                            {
                                textfale = true;
                            }
                        }
                        else
                        {
                            imgdiv2.Visible = true;
                            lbl_alert1.Visible = true;
                            lbl_alert1.Text = "Sorry This Question Added in FeedBack ";
                        }
                    }
                }
            }
            //string value = FpSpread1.Sheets[0].Cells[Convert.ToInt32(activerow), 1].Tag.ToString();
            if (textfale == true)
            {
                imgdiv2.Visible = true;
                lbl_alert1.Visible = true;
                lbl_alert1.Text = "Deleted Successfully";
            }
            //  FbName1();
            btn_Search1_Click(sender, e);
        }
        catch
        {
        }
    }
    public void btn_warning_exit_Click(object sender, EventArgs e)
    {
        imgdiv3.Visible = false;
    }
    protected void btn_Search1_Click(object sender, EventArgs e)
    {
        if (rb_Acad.Checked == true)
        {
            acdquestion();
        }
        else if (rb_Gend.Checked == true)
        {
            // acdquestion();
            gendquestion();
        }
    }
    public void acdquestion()
    {
        try
        {
            // div1.Visible = true;
            rptprint1.Visible = true;
            //QuestionMaster();
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
            FpSpread1.Sheets[0].RowCount = 0;
            FpSpread1.Sheets[0].ColumnCount = 0;
            FpSpread1.CommandBar.Visible = false;
            FpSpread1.Sheets[0].AutoPostBack = false;
            FpSpread1.Sheets[0].ColumnHeader.RowCount = 1;
            FpSpread1.Sheets[0].RowHeader.Visible = false;
            FpSpread1.Sheets[0].ColumnCount = 3;
            FpSpread1.Columns[0].Locked = true;
            FpSpread1.Columns[2].Locked = true;
            FarPoint.Web.Spread.StyleInfo darkstyle = new FarPoint.Web.Spread.StyleInfo();
            darkstyle.BackColor = ColorTranslator.FromHtml("#0CA6CA");
            darkstyle.ForeColor = Color.White;
            FpSpread1.ActiveSheetView.ColumnHeader.DefaultStyle = darkstyle;
            FpSpread1.Visible = true;
            FpSpread1.SaveChanges();
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 0].Text = "S.No";
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 0].Font.Bold = true;
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 0].HorizontalAlign = HorizontalAlign.Center;
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 0].Font.Name = "Book Antiqua";
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 0].Font.Size = FontUnit.Medium;
            FpSpread1.Sheets[0].Columns[0].HorizontalAlign = HorizontalAlign.Center;
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 1].Text = "Select";
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 1].Font.Bold = true;
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 1].Font.Name = "Book Antiqua";
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 1].Font.Size = FontUnit.Medium;
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 1].HorizontalAlign = HorizontalAlign.Center;
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 2].Text = "Questions";
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 2].Font.Bold = true;
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 2].Font.Name = "Book Antiqua";
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 2].Font.Size = FontUnit.Medium;
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 2].HorizontalAlign = HorizontalAlign.Center;
            FpSpread1.Sheets[0].ColumnHeader.Columns[0].Width = 60;
            FpSpread1.Sheets[0].ColumnHeader.Columns[1].Width = 74;
            FpSpread1.Sheets[0].ColumnHeader.Columns[2].Width = 473;
            FpSpread1.Sheets[0].Columns[0].HorizontalAlign = HorizontalAlign.Center;
            FpSpread1.Sheets[0].Columns[1].HorizontalAlign = HorizontalAlign.Center;
            FpSpread1.Sheets[0].Columns[2].HorizontalAlign = HorizontalAlign.Left;
            string selqry = "";
            string type = "";
            if (rb_Acad.Checked == true)
            {
                type = "1";
            }
            else if (rb_Gend.Checked == true)
            {
                type = "2";
            }
            string feedbackname = "";
            for (int i = 0; i < Cbl_FbName1.Items.Count; i++)
            {
                if (Cbl_FbName1.Items[i].Selected == true)
                {
                    if (feedbackname == "")
                    {
                        feedbackname = "" + Cbl_FbName1.Items[i].Value.ToString() + "";
                    }
                    else
                    {
                        feedbackname = feedbackname + "','" + Cbl_FbName1.Items[i].Value.ToString() + "";
                    }
                }
            }
            if (rb_Acad.Checked == true)
            {
                selqry = " select distinct f.FeedBackMasterPK,f.FeedBackName  from CO_QuestionMaster q,TextValTable t,CO_FeedBackMaster F , Degree d, Department dt,Course c,CO_FeedBackQuestions fq WHERE F.DegreeCode =d.Degree_Code and d.Dept_Code =dt.Dept_Code and c.Course_Id =d.Course_Id and  t.TextCode=q.HeaderCode and F.FeedBackType =q.QuestType and fq.FeedBackMasterFK =f.FeedBackMasterPK and q.QuestionMasterPK =fq.QuestionMasterFK and   q.QuestType='" + type + "' and F.collegecode in ('" + college_cd + "')   and F.degreecode in ('" + degree_code + "') and  F.Semester in ('" + semester + "') and F.Section in('" + section + "') and F.FeedBackName in ('" + feedbackname + "') ";
                selqry = selqry + " select ( CONVERT(varchar(10), F.Batch_Year)+'-'+ c.Course_Name +'-'+ dt.Dept_Name +'-'+ CONVERT(varchar(10), F.Semester)+'-'+ F.Section) as degreename, FeedBackMasterPK ,d.Degree_Code from CO_FeedBackMaster F , Degree d, Department dt,Course c WHERE F.DegreeCode =d.Degree_Code and d.Dept_Code =dt.Dept_Code and c.Course_Id =d.Course_Id  and F.collegecode in ('" + college_cd + "') and  F.Batch_Year in ('" + Batch_Year + "')    and F.degreecode in ('" + degree_code + "') and  F.Semester in ('" + semester + "') and F.Section in('" + section + "') and F.FeedBackName in ('" + feedbackname + "') and FeedBackType ='" + type + "' order by d.Degree_Code,f.semester ";
                selqry = selqry + " select distinct q.HeaderCode,FeedBackMasterPK,t.TextVal,d.Degree_Code,f.semester,f.Section from CO_QuestionMaster q,TextValTable t,CO_FeedBackMaster F , Degree d, Department dt,Course c,CO_FeedBackQuestions fq WHERE F.DegreeCode =d.Degree_Code and d.Dept_Code =dt.Dept_Code and c.Course_Id =d.Course_Id and  t.TextCode=q.HeaderCode and F.FeedBackType =q.QuestType and fq.FeedBackMasterFK =f.FeedBackMasterPK and q.QuestionMasterPK =fq.QuestionMasterFK  and   QuestType='" + type + "' and F.collegecode in ('" + college_cd + "') and  F.Batch_Year in ('" + Batch_Year + "')   and F.degreecode in ('" + degree_code + "') and  F.Semester in ('" + semester + "') and F.Section in('" + section + "') and F.FeedBackName in ('" + feedbackname + "') order by d.Degree_Code,f.semester";
                selqry = selqry + " select FeedBackMasterPK,t.TextVal,q.HeaderCode ,Question,QuestType, QuestionMasterPK,d.Degree_Code,f.semester,f.Section from CO_QuestionMaster q,TextValTable t,CO_FeedBackMaster F , Degree d, Department dt,Course c,CO_FeedBackQuestions fq WHERE F.DegreeCode =d.Degree_Code and d.Dept_Code =dt.Dept_Code and c.Course_Id =d.Course_Id and  t.TextCode=q.HeaderCode and F.FeedBackType =q.QuestType and fq.FeedBackMasterFK =f.FeedBackMasterPK and q.QuestionMasterPK =fq.QuestionMasterFK  and   QuestType='" + type + "' and F.collegecode in ('" + college_cd + "') and  F.Batch_Year in ('" + Batch_Year + "')  and F.degreecode in ('" + degree_code + "') and  F.Semester in ('" + semester + "') and F.Section in('" + section + "') and F.FeedBackName in ('" + feedbackname + "') order by d.Degree_Code,f.semester";
            }
            else if (rb_Gend.Checked == true)
            {
                //selqry = "select HeaderCode,TextVal as HeaderName, FeedBackName,FeedBackMasterPK,Question,QuestType,QuestionMasterPK,  F.Batch_Year, F.Semester, F.Section from CO_QuestionMaster q,TextValTable t,CO_FeedBackMaster F where t.TextCode=q.HeaderCode and F.FeedBackType =q.QuestType and   QuestType='" + type + "' and f.collegecode in ('" + college_cd + "')  ";
                selqry = "select HeaderCode,TextVal as HeaderName, FeedBackName,FeedBackMasterPK,Question,QuestType,QuestionMasterPK from CO_QuestionMaster q,TextValTable t,CO_FeedBackMaster F , Degree d, Department dt,Course c,CO_FeedBackQuestions fq WHERE F.DegreeCode =d.Degree_Code and d.Dept_Code =dt.Dept_Code and c.Course_Id =d.Course_Id and  t.TextCode=q.HeaderCode and F.FeedBackType =q.QuestType and fq.FeedBackMasterFK =f.FeedBackMasterPK and q.QuestionMasterPK =fq.QuestionMasterFK  and  QuestType='" + type + "' and F.FeedBackName in ('" + feedbackname + "') ";
            }
            ds.Clear();
            ds = d2.select_method_wo_parameter(selqry, "Text");
            FarPoint.Web.Spread.CheckBoxCellType chk1 = new FarPoint.Web.Spread.CheckBoxCellType();
            chk1.AutoPostBack = false;
            DataView dvnew = new DataView();
            DataView dv1 = new DataView();
            DataView dv2 = new DataView();
            DataView dv3 = new DataView();
            int s_no = 1;
            if (ds.Tables.Count > 0)
            {
                if (ds.Tables[0].Rows.Count > 0)
                {
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        FpSpread1.Sheets[0].RowCount++;
                        FpSpread1.Sheets[0].Cells[FpSpread1.Rows.Count - 1, 0].Text = ds.Tables[0].Rows[i]["FeedBackName"].ToString();
                        FpSpread1.Sheets[0].SpanModel.Add(FpSpread1.Sheets[0].RowCount - 1, 0, 1, 3);
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].Rows.Count - 1, 0].ForeColor = Color.Green;
                        if (ds.Tables[1].Rows.Count > 0)
                        {
                            //ds.Tables[1].DefaultView.RowFilter =
                            ds.Tables[1].DefaultView.RowFilter = "FeedBackMasterPK='" + Convert.ToString(ds.Tables[0].Rows[i]["FeedBackMasterPK"]) + "'";
                            dv1 = ds.Tables[1].DefaultView;
                            if (dv1.Count > 0)
                            {
                                for (int j = 0; j < dv1.Count; j++)
                                {
                                    FpSpread1.Sheets[0].RowCount++;
                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Text = Convert.ToString(dv1[j]["degreename"]);
                                    FpSpread1.Sheets[0].SpanModel.Add(FpSpread1.Sheets[0].RowCount - 1, 0, 1, 3);
                                    FpSpread1.Sheets[0].Cells[FpSpread1.Rows.Count - 1, 0].HorizontalAlign = HorizontalAlign.Left;
                                    if (ds.Tables[2].Rows.Count > 0)
                                    {
                                        ds.Tables[2].DefaultView.RowFilter = "Degree_Code='" + Convert.ToString(dv1[j]["Degree_Code"]) + "' and FeedBackMasterPK='" + Convert.ToString(ds.Tables[0].Rows[i]["FeedBackMasterPK"]) + "'";
                                        dv2 = ds.Tables[2].DefaultView;
                                        if (dv2.Count > 0)
                                        {
                                            for (int k = 0; k < dv2.Count; k++)
                                            {
                                                FpSpread1.Sheets[0].RowCount++;
                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Text = Convert.ToString(dv2[k]["TextVal"]);
                                                FpSpread1.Sheets[0].SpanModel.Add(FpSpread1.Sheets[0].RowCount - 1, 0, 1, 3);
                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].Rows.Count - 1, 0].ForeColor = Color.Blue;
                                                FpSpread1.Sheets[0].Cells[FpSpread1.Rows.Count - 1, 0].HorizontalAlign = HorizontalAlign.Center;
                                                if (ds.Tables[3].Rows.Count > 0)
                                                {
                                                    ds.Tables[3].DefaultView.RowFilter = "HeaderCode='" + Convert.ToString(dv2[k]["HeaderCode"]) + "' and Degree_Code='" + Convert.ToString(dv1[j]["Degree_Code"]) + "' and FeedBackMasterPK='" + Convert.ToString(ds.Tables[0].Rows[i]["FeedBackMasterPK"]) + "'";
                                                    dv3 = ds.Tables[3].DefaultView;
                                                    if (dv3.Count > 0)
                                                    {
                                                        for (int m = 0; m < dv3.Count; m++)
                                                        {
                                                            FpSpread1.Sheets[0].RowCount++;
                                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Text = Convert.ToString(s_no++);
                                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].CellType = chk1;
                                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].Text = Convert.ToString(dv3[m]["Question"]);
                                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Tag = Convert.ToString(dv3[m]["QuestionMasterPK"]);
                                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].Tag = Convert.ToString(dv3[m]["FeedBackMasterPK"]);
                                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].Tag = Convert.ToString(dv3[m]["HeaderCode"]);
                                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Note = Convert.ToString(dv3[m]["Degree_Code"]);
                                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Note = Convert.ToString(dv3[m]["Section"]);
                                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Note = Convert.ToString(dv3[m]["Semester"]);
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
                }
                else
                {
                    imgdiv2.Visible = true;
                    lbl_alert1.Text = "No Records Found";
                    FpSpread1.Visible = false;
                    //div1.Visible = false;
                    rptprint1.Visible = false;
                }
            }
            else
            {
                imgdiv2.Visible = true;
                lbl_alert1.Text = "No Records Found";
                FpSpread1.Visible = false;
                // div1.Visible = false;
                rptprint1.Visible = false;
            }
            FpSpread1.Sheets[0].PageSize = FpSpread1.Rows.Count;
        }
        catch
        {
        }
    }
    public void gendquestion()
    {
        try
        {
            rptprint1.Visible = true;
            FpSpread1.Sheets[0].RowCount = 0;
            FpSpread1.Sheets[0].ColumnCount = 0;
            FpSpread1.CommandBar.Visible = false;
            FpSpread1.Sheets[0].AutoPostBack = false;
            FpSpread1.Sheets[0].ColumnHeader.RowCount = 1;
            FpSpread1.Sheets[0].RowHeader.Visible = false;
            FpSpread1.Sheets[0].ColumnCount = 3;
            FpSpread1.Columns[0].Locked = true;
            FpSpread1.Columns[2].Locked = true;
            FarPoint.Web.Spread.StyleInfo darkstyle = new FarPoint.Web.Spread.StyleInfo();
            darkstyle.BackColor = ColorTranslator.FromHtml("#0CA6CA");
            darkstyle.ForeColor = Color.White;
            FpSpread1.ActiveSheetView.ColumnHeader.DefaultStyle = darkstyle;
            FpSpread1.Visible = true;
            FpSpread1.SaveChanges();
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 0].Text = "S.No";
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 0].Font.Bold = true;
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 0].HorizontalAlign = HorizontalAlign.Center;
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 0].Font.Name = "Book Antiqua";
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 0].Font.Size = FontUnit.Medium;
            FpSpread1.Sheets[0].Columns[0].HorizontalAlign = HorizontalAlign.Center;
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 1].Text = "Select";
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 1].Font.Bold = true;
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 1].Font.Name = "Book Antiqua";
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 1].Font.Size = FontUnit.Medium;
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 1].HorizontalAlign = HorizontalAlign.Center;
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 2].Text = "Questions";
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 2].Font.Bold = true;
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 2].Font.Name = "Book Antiqua";
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 2].Font.Size = FontUnit.Medium;
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 2].HorizontalAlign = HorizontalAlign.Center;
            FpSpread1.Sheets[0].ColumnHeader.Columns[0].Width = 60;
            FpSpread1.Sheets[0].ColumnHeader.Columns[1].Width = 74;
            FpSpread1.Sheets[0].ColumnHeader.Columns[2].Width = 473;
            FpSpread1.Sheets[0].Columns[0].HorizontalAlign = HorizontalAlign.Center;
            FpSpread1.Sheets[0].Columns[1].HorizontalAlign = HorizontalAlign.Center;
            FpSpread1.Sheets[0].Columns[2].HorizontalAlign = HorizontalAlign.Left;
            ////ds.Clear();
            string activerow = FpSpread2.ActiveSheetView.ActiveRow.ToString();
            string activecol = FpSpread2.ActiveSheetView.ActiveColumn.ToString();
            string selqry = "";
            string type = "";
            if (rb_Acad.Checked == true)
            {
                type = "1";
            }
            else if (rb_Gend.Checked == true)
            {
                type = "2";
            }
            string feedbackname = "";
            for (int i = 0; i < Cbl_FbName1.Items.Count; i++)
            {
                if (Cbl_FbName1.Items[i].Selected == true)
                {
                    if (feedbackname == "")
                    {
                        feedbackname = "" + Cbl_FbName1.Items[i].Value.ToString() + "";
                    }
                    else
                    {
                        feedbackname = feedbackname + "','" + Cbl_FbName1.Items[i].Value.ToString() + "";
                    }
                }
            }
            ds.Clear();
            selqry = "select distinct FeedBackMasterPK,FeedBackName from CO_QuestionMaster q,TextValTable t,CO_FeedBackMaster F,CO_FeedBackQuestions fq where t.TextCode=q.HeaderCode and F.FeedBackType =q.QuestType and fq.FeedBackMasterFK =f.FeedBackMasterPK and fq.QuestionMasterFK =fq.QuestionMasterFK and  q.QuestType='" + type + "' and f.FeedBackType ='" + type + "'  and F.FeedBackName in ('" + feedbackname + "') ";
            selqry = selqry + "select distinct HeaderCode,TextVal as HeaderName,FeedBackMasterPK  from CO_FeedBackMaster c,CO_FeedBackQuestions cq,CO_QuestionMaster q,TextValTable t where c.FeedBackMasterPK =cq.FeedBackMasterFK and q.QuestionMasterPK =cq.QuestionMasterFK and t.TextCode =q.HeaderCode  and c.FeedBackType =q.QuestType  and FeedBackType ='" + type + "' and c.FeedBackName in ('" + feedbackname + "') and q.QuestType ='" + type + "'";
            selqry = selqry + "select HeaderCode,TextVal as HeaderName, FeedBackName,FeedBackMasterPK,Question,QuestType,QuestionMasterPK   from CO_FeedBackMaster c,CO_FeedBackQuestions cq,CO_QuestionMaster q,TextValTable t where c.FeedBackMasterPK =cq.FeedBackMasterFK and q.QuestionMasterPK =cq.QuestionMasterFK and t.TextCode =q.HeaderCode  and c.FeedBackType =q.QuestType  and FeedBackType ='" + type + "' and c.FeedBackName in ('" + feedbackname + "') and q.QuestType ='" + type + "' ";
            ds = d2.select_method_wo_parameter(selqry, "Text");
            FarPoint.Web.Spread.CheckBoxCellType chk1 = new FarPoint.Web.Spread.CheckBoxCellType();
            chk1.AutoPostBack = false;
            int s_no = 1;
            DataView dv3 = new DataView();
            if (ds.Tables[0].Rows.Count > 0 && ds.Tables[1].Rows.Count > 0 && ds.Tables[2].Rows.Count > 0)
            {
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    FpSpread1.Sheets[0].RowCount++;
                    FpSpread1.Sheets[0].Cells[FpSpread1.Rows.Count - 1, 0].Text = ds.Tables[0].Rows[i]["FeedBackName"].ToString();
                    FpSpread1.Sheets[0].SpanModel.Add(FpSpread1.Sheets[0].RowCount - 1, 0, 1, 3);
                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].Rows.Count - 1, 0].ForeColor = Color.Green;
                    DataView dv1 = new DataView();
                    if (ds.Tables[1].Rows.Count > 0)
                    {
                        ds.Tables[1].DefaultView.RowFilter =
                             ds.Tables[1].DefaultView.RowFilter = "FeedBackMasterPK='" + Convert.ToString(ds.Tables[0].Rows[i]["FeedBackMasterPK"]) + "'";
                        dv1 = ds.Tables[1].DefaultView;
                        if (dv1.Count > 0)
                        {
                            for (int j = 0; j < dv1.Count; j++)
                            {
                                FpSpread1.Sheets[0].RowCount++;
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Text = Convert.ToString(dv1[j]["HeaderName"]);
                                FpSpread1.Sheets[0].SpanModel.Add(FpSpread1.Sheets[0].RowCount - 1, 0, 1, 3);
                                FpSpread1.Sheets[0].Cells[FpSpread1.Rows.Count - 1, 0].HorizontalAlign = HorizontalAlign.Center;
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].Rows.Count - 1, 0].ForeColor = Color.Blue;
                                DataView dv2 = new DataView();
                                if (ds.Tables[2].Rows.Count > 0)
                                {
                                    ds.Tables[2].DefaultView.RowFilter = "HeaderCode='" + Convert.ToString(dv1[j]["HeaderCode"]) + "' and FeedBackMasterPK='" + Convert.ToString(ds.Tables[0].Rows[i]["FeedBackMasterPK"]) + "'";
                                    dv2 = ds.Tables[2].DefaultView;
                                    if (dv2.Count > 0)
                                    {
                                        for (int k = 0; k < dv2.Count; k++)
                                        {
                                            FpSpread1.Sheets[0].RowCount++;
                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Text = Convert.ToString(s_no++);
                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].CellType = chk1;
                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].Text = Convert.ToString(dv2[k]["Question"]);
                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Tag = Convert.ToString(dv2[k]["QuestionMasterPK"]);
                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].Tag = Convert.ToString(dv2[k]["FeedBackMasterPK"]);
                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].Tag = Convert.ToString(dv2[k]["HeaderCode"]);
                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Note = Convert.ToString(dv2[k]["QuestType"]);
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
            else
            {
                imgdiv2.Visible = true;
                lbl_alert1.Text = "No Records Found";
                FpSpread1.Visible = false;
                //div1.Visible = false;
                rptprint1.Visible = false;
            }
            FpSpread1.Sheets[0].PageSize = FpSpread1.Sheets[0].RowCount;
        }
        catch
        {
        }
    }
    protected void imagebtnpopclose1_Click(object sender, EventArgs e)
    {
        addnew.Visible = false;
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
            string dptname = "FeedBack Question Report ";
            string pagename = "FeedBack_Question_Master.aspx";
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
    //09.08.17 barath
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
            string branchcode = rs.GetSelectedItemsValueAsString(cbl_branch1);
            string batchyear = rs.GetSelectedItemsValueAsString(cbl_batch1);
            string semester = rs.GetSelectedItemsValueAsString(cbl_sem1);
            if (!string.IsNullOrEmpty(branchcode) && !string.IsNullOrEmpty(semester) && !string.IsNullOrEmpty(batchyear))
            {
                stafftype = " select distinct subject_type from sub_sem u,syllabus_master y where u.syll_code = y.syll_code and y.degree_code in ('" + branchcode + "')  and semester in ('" + semester + "') and Batch_Year in ('" + batchyear + "')";
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
                            for (int i = 0; i < Cbl_Subjecttype.Items.Count; i++)
                            {
                                Cbl_Subjecttype.Items[i].Selected = true;
                            }
                            Txt_Subjecttype.Text = "Subject Type(" + Cbl_Subjecttype.Items.Count + ")";
                            Cb_Subjecttype.Checked = true;
                        }
                    }
                }
            }
        }
        catch (Exception)
        {
        }
    }

    protected void rdbobjective_checkedChange(object sender, EventArgs e)
    {
        if (rdbobjective.Checked == true)
        {
            rdbdescriptive.Checked = false;
        }
    }
    protected void rdbdescriptive_checkedChange(object sender, EventArgs e)
    {
        if (rdbdescriptive.Checked == true)
        {
            rdbobjective.Checked = false;
        }
    
    }
}
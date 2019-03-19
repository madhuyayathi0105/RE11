using System;
using System.Collections;
using System.Data;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using wc = System.Web.UI.WebControls;

public partial class SubjectWiseBatchAllocation : System.Web.UI.Page
{
    DAccess2 dacces2 = new DAccess2();
    Hashtable hat = new Hashtable();

    #region
    DataTable dtable = new DataTable();
    DataRow dtrow = null;
    Hashtable htable = new Hashtable();
    DataTable dtable2 = new DataTable();
    DataRow dtrow2 = null;
    #endregion

    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["collegecode"] == null)
        {
            Response.Redirect("~/Default.aspx");
        }
        errmsg.Visible = false;
        if (!IsPostBack)
        {
            bindstram();
            BindBatch();
            bindeducation();
            bindsem();            
            binddegree();            
            bindbranch();
            bindsubtype();
            bindsubject();
            bindsec();
            clear();
            fromno.Attributes.Add("autocomplete", "off");
            tono.Attributes.Add("autocomplete", "off");
            txtnoofbatch.Attributes.Add("autocomplete", "off");
            Session["Rollflag"] = "0";
            Session["Regflag"] = "0";
            Session["Studflag"] = "0";
            string grouporusercode = string.Empty;
            if ((Session["group_code"].ToString().Trim() != "") && (Session["group_code"].ToString().Trim() != "0") && (Session["group_code"].ToString().Trim() != "-1"))
            {
                grouporusercode = " group_code=" + Session["group_code"].ToString().Trim() + "";
            }
            else
            {
                grouporusercode = " usercode=" + Session["usercode"].ToString().Trim() + "";
            }
            string Master = "select * from Master_Settings where " + grouporusercode + "";
            DataSet ds = dacces2.select_method(Master, hat, "Text");
            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            {
                if (ds.Tables[0].Rows[i]["settings"].ToString() == "Roll No" && ds.Tables[0].Rows[i]["value"].ToString() == "1")
                {
                    Session["Rollflag"] = "1";
                }
                if (ds.Tables[0].Rows[i]["settings"].ToString() == "Register No" && ds.Tables[0].Rows[i]["value"].ToString() == "1")
                {
                    Session["Regflag"] = "1";
                }
                if (ds.Tables[0].Rows[i]["settings"].ToString() == "Student_Type" && ds.Tables[0].Rows[i]["value"].ToString() == "1")
                {
                    Session["Studflag"] = "1";
                }
            }
        }
    }

    public void bindstram()
    {
        try
        {
            string collegecode = Session["collegecode"].ToString();
            DataSet ds = dacces2.select_method_wo_parameter("select distinct type from Course where isnull(type,'')<>'' and  college_code='" + collegecode + "'", "Text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                ddlstream.DataSource = ds;
                ddlstream.DataTextField = "type";
                ddlstream.DataValueField = "type";
                ddlstream.DataBind();
            }
            else
            {
                ddlstream.Enabled = false;
            }
        }
        catch (Exception ex)
        {
            errmsg.Visible = true;
            errmsg.Text = ex.ToString();
        }
    }

    public void BindBatch()
    {
        try
        {
            string Master1 = string.Empty;
            if ((Session["group_code"].ToString().Trim() != "") && (Session["group_code"].ToString().Trim() != "0") && (Session["group_code"].ToString().Trim() != "-1"))
            {
                string group = Session["group_code"].ToString();
                if (group.Contains(';'))
                {
                    string[] group_semi = group.Split(';');
                    Master1 = group_semi[0].ToString();
                }
            }
            else
            {
                Master1 = Session["usercode"].ToString();
            }
            string collegecode = Session["collegecode"].ToString();
            string strbinddegree = "select distinct batch_year from tbl_attendance_rights where user_id='" + Master1 + "'";
            DataSet ds = dacces2.select_method_wo_parameter(strbinddegree, "Text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                ddlbatch.DataSource = ds;
                ddlbatch.DataTextField = "Batch_year";
                ddlbatch.DataValueField = "Batch_year";
                ddlbatch.DataBind();
                ddlbatch.SelectedIndex = ddlbatch.Items.Count - 1;
            }
        }
        catch (Exception ex)
        {
            errmsg.Visible = true;
            errmsg.Text = ex.ToString();
        }
    }

    public void bindeducation()
    {
        try
        {
            ddlcourse.Items.Clear();
            string collegecode = Session["collegecode"].ToString();
            string usercode = Session["usercode"].ToString();
            string group_code = Session["group_code"].ToString();
            if (group_code.Contains(';'))
            {
                string[] group_semi = group_code.Split(';');
                group_code = group_semi[0].ToString();
            }
            string typeval = string.Empty;
            if (ddlstream.Items.Count > 0 && ddlstream.Enabled == true)
            {
                typeval = " and course.type='" + ddlstream.SelectedItem.ToString() + "'";
            }
            string query = string.Empty;
            if ((group_code.ToString().Trim() != "") && (group_code.Trim() != "0") && (group_code.ToString().Trim() != "-1"))
            {
                query = "select distinct course.Edu_Level from degree,course,deptprivilages where course.course_id=degree.course_id and course.college_code = degree.college_code and degree.college_code=" + collegecode + "  and deptprivilages.Degree_code=degree.Degree_code and group_code=" + group_code + " " + typeval + "";
            }
            else
            {
                query = "select distinct course.Edu_Level from degree,course,deptprivilages where course.course_id=degree.course_id and course.college_code = degree.college_code and degree.college_code=" + collegecode + "  and deptprivilages.Degree_code=degree.Degree_code and user_code=" + usercode + " " + typeval + "";
            }
            DataSet ds = new DataSet();
            ds = dacces2.select_method(query, hat, "Text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                ddlcourse.DataSource = ds;
                ddlcourse.DataValueField = "Edu_Level";
                ddlcourse.DataTextField = "Edu_Level";
                ddlcourse.DataBind();
            }
        }
        catch (Exception ex)
        {
            errmsg.Visible = true;
            errmsg.Text = ex.ToString();
        }
    }

    public void bindsubtype()
    {
        try
        {
            ddlsubtype.Items.Clear();
            string typeval = string.Empty;
            if (ddlstream.Items.Count > 0 && ddlstream.Enabled == true)
            {
                typeval = " and c.type='" + ddlstream.SelectedItem.ToString() + "'";
            }
            string degreecode = string.Empty;
            if(!string.IsNullOrEmpty(cblbranch.SelectedValue))
            {
                for (int i = 0; i < cblbranch.Items.Count; i++)
                {
                    if (cblbranch.Items[i].Selected)
                    {
                        if (string.IsNullOrEmpty(degreecode))
                        {
                            degreecode = cblbranch.Items[i].Value;
                        }
                        else
                        {
                            degreecode = degreecode + "','" + cblbranch.Items[i].Value;
                        }
                    }
                }
            }
            string sctsre = "select distinct ss.subject_type from syllabus_master sy,sub_sem ss,Degree d,Course c where sy.degree_code=d.Degree_Code and d.Course_Id=c.Course_Id and sy.syll_code=ss.syll_code and sy.Batch_Year='" + ddlbatch.SelectedItem.ToString() + "' " + typeval + " and d.Degree_Code in('" + degreecode + "') and c.Edu_Level='" + ddlcourse.SelectedItem.ToString() + "' and sy.semester='" + ddlsem.SelectedValue.ToString() + "'";
            DataSet ds = dacces2.select_method_wo_parameter(sctsre, "Text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                ddlsubtype.DataSource = ds;
                ddlsubtype.DataTextField = "subject_type";
                ddlsubtype.DataValueField = "subject_type";
                ddlsubtype.DataBind();
            }
        }
        catch (Exception ex)
        {
            errmsg.Visible = true;
            errmsg.Text = ex.ToString();
        }
    }

    public void bindsem()
    {
        try
        {
            string typeval = string.Empty;
            if (ddlstream.Items.Count > 0 && ddlstream.Enabled == true)
            {
                typeval = " and c.type='" + ddlstream.SelectedItem.ToString() + "'";
            }
            ddlsem.Items.Clear();
            Boolean first_year;
            first_year = false;
            int duration = 0;
            int i = 0;
            string strquery = "select distinct n.ndurations,n.first_year_nonsemester from ndegree n,Degree d,Course c where n.Degree_code=d.Degree_Code and  d.Course_Id=c.Course_Id " + typeval + " and c.Edu_Level='" + ddlcourse.SelectedItem.ToString() + "' and n.batch_year=" + ddlbatch.Text.ToString() + " and n.college_code=" + Session["collegecode"] + "  order by ndurations desc";
            DataSet dssem = dacces2.select_method_wo_parameter(strquery, "Text");
            if (dssem.Tables[0].Rows.Count > 0)
            {
                first_year = Convert.ToBoolean(dssem.Tables[0].Rows[0][1].ToString());
                duration = Convert.ToInt16(dssem.Tables[0].Rows[0][0].ToString());
                for (i = 1; i <= duration; i++)
                {
                    if (first_year == false)
                    {
                        ddlsem.Items.Add(i.ToString());
                    }
                    else if (first_year == true && i != 2)
                    {
                        ddlsem.Items.Add(i.ToString());
                    }
                }
            }
            else
            {
                strquery = "select distinct duration,first_year_nonsemester from degree d,Course c where d.Course_Id=c.Course_Id " + typeval + " and c.Edu_Level='" + ddlcourse.SelectedItem.ToString() + "' and d.college_code='" + Session["collegecode"] + "' order by duration desc";
                dssem = dacces2.select_method_wo_parameter(strquery, "Text");
                if (dssem.Tables[0].Rows.Count > 0)
                {
                    first_year = Convert.ToBoolean(dssem.Tables[0].Rows[0][1].ToString());
                    duration = Convert.ToInt16(dssem.Tables[0].Rows[0][0].ToString());
                    for (i = 1; i <= duration; i++)
                    {
                        if (first_year == false)
                        {
                            ddlsem.Items.Add(i.ToString());
                        }
                        else if (first_year == true && i != 2)
                        {
                            ddlsem.Items.Add(i.ToString());
                        }
                    }
                }
            }
            if (ddlsem.Items.Count > 0)
            {
                ddlsem.SelectedIndex = 0;
            }
        }
        catch (Exception ex)
        {
            errmsg.Visible = true;
            errmsg.Text = ex.ToString();
        }
    }

    public void bindsubject()
    {
        try
        {
            ddlsubject.Items.Clear();
            if (ddlsubtype.Items.Count > 0)
            {
                string typeval = string.Empty;
                if (ddlstream.Items.Count > 0 && ddlstream.Enabled == true)
                {
                    typeval = " and c.type='" + ddlstream.SelectedItem.ToString() + "'";
                }
                string degreecode = string.Empty;
            if(!string.IsNullOrEmpty(cblbranch.SelectedValue))
            {
                for (int i = 0; i < cblbranch.Items.Count; i++)
                {
                    if (cblbranch.Items[i].Selected)
                    {
                        if (string.IsNullOrEmpty(degreecode))
                        {
                            degreecode = cblbranch.Items[i].Value;
                        }
                        else
                        {
                            degreecode = degreecode + "','" + cblbranch.Items[i].Value;
                        }
                    }
                }
            }
            string sctsre = "select distinct s.subject_name,s.subject_code from syllabus_master sy,sub_sem ss,Degree d,Course c,subject s  where sy.degree_code=d.Degree_Code and d.Course_Id=c.Course_Id and sy.syll_code=ss.syll_code and ss.subType_no=s.subType_no and sy.Batch_Year='" + ddlbatch.SelectedValue.ToString() + "' " + typeval + " and d.Degree_Code in('" + degreecode + "') and c.Edu_Level='" + ddlcourse.SelectedValue.ToString() + "' and sy.semester='" + ddlsem.SelectedValue.ToString() + "' and ss.subject_type='" + ddlsubtype.SelectedItem.ToString() + "'";
                DataSet ds = dacces2.select_method_wo_parameter(sctsre, "Text");
                if (ds.Tables[0].Rows.Count > 0)
                {
                    ddlsubject.DataSource = ds;
                    ddlsubject.DataTextField = "subject_name";
                    ddlsubject.DataValueField = "subject_code";
                    ddlsubject.DataBind();
                }
            }
        }
        catch (Exception ex)
        {
            errmsg.Visible = true;
            errmsg.Text = ex.ToString();
        }
    }

    public void binddegree()
    {
        try
        {
            cbldegree.Items.Clear();
            txtdegree.Text = "--Select--";
            cblbranch.Items.Clear();
            chkbranch.Checked = false;
            txtbranch.Text = "--Select--";
            string collegecode = Session["collegecode"].ToString();
            string eduLvl = string.Empty;
            if (!string.IsNullOrEmpty(ddlcourse.SelectedValue))
            {
                eduLvl = ddlcourse.SelectedValue;
            }

            string qry = "select distinct d.Course_Id,c.Course_Name from Degree d,course c ,DeptPrivilages p where p.degree_code=d.Degree_Code and d.Course_Id=c.Course_Id and d.college_code=c.college_code and d.college_code='" + collegecode + "' and Edu_Level in('" + eduLvl + "') ";
            DataSet ds = new DataSet();
            ds = dacces2.select_method(qry, hat, "Text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                cbldegree.DataSource = ds;
                cbldegree.DataTextField = "Course_Name";
                cbldegree.DataValueField = "Course_Id";
                cbldegree.DataBind();
                checkBoxListselectOrDeselect(cbldegree, true);
                CallCheckboxListChange(chkdegree, cbldegree, txtdegree, lbldegree.Text, "--Select--");
            }
        }
        catch (Exception ex)
        {
            errmsg.Visible = true;
            errmsg.Text = ex.ToString();
        }
    }

    public void bindbranch()
    {
        try
        {
            cblbranch.Items.Clear();
            chkbranch.Checked = false;
            string collegecode = Session["collegecode"].ToString();
            string branch = string.Empty;
            if (cbldegree.Items.Count > 0)
            {
                for (int i = 0; i < cbldegree.Items.Count; i++)
                {
                    if (cbldegree.Items[i].Selected)
                    {
                        if (string.IsNullOrEmpty(branch))
                        {
                            branch = cbldegree.Items[i].Value;
                        }
                        else
                        {
                            branch = branch + "','" + cbldegree.Items[i].Value;
                        }
                    }
                }
            }
            string qry = " select distinct degree.degree_code,department.dept_name,degree.Acronym  from degree,department,course,deptprivilages where course.course_id=degree.course_id  and department.dept_code=degree.dept_code and course.college_code = degree.college_code and department.college_code = degree.college_code and degree.course_id in('" + branch + "') and deptprivilages.Degree_code=degree.Degree_code and degree.college_code='" + collegecode + "'";
            DataSet ds = new DataSet();
            ds = dacces2.select_method(qry, hat, "Text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                cblbranch.DataSource = ds;
                cblbranch.DataTextField = "dept_name";
                cblbranch.DataValueField = "degree_code";
                cblbranch.DataBind();
                if (cblbranch.Items.Count > 0)
                {
                    for (int i = 0; i < cblbranch.Items.Count; i++)
                    {
                        cblbranch.Items[i].Selected = true;
                    }
                    txtbranch.Text = "Branch(" + cblbranch.Items.Count + ")";
                    chkbranch.Checked = true;
                }
            }
        }
        catch
        {

        }
    }

    public void bindsec()
    {
        string year = string.Empty;
        string degree = string.Empty;
        DataSet ds = new DataSet();
        if (!string.IsNullOrEmpty(ddlbatch.SelectedValue))
        {
            year = ddlbatch.SelectedValue;
        }
        for (int i = 0; i < cblbranch.Items.Count; i++)
        {
            if (cblbranch.Items[i].Selected)
            {
                if (degree == "")
                {
                    degree = cblbranch.Items[i].Value;
                }
                else
                {
                    degree = degree + "','" + cblbranch.Items[i].Value;
                }
            }
        }
        if (!string.IsNullOrEmpty(degree))
        {
            string qry = "select distinct sections from registration where batch_year=" + year + " and degree_code in ('" + degree + "') and sections<>'-1' and sections<>' ' and delflag=0 and exam_flag<>'Debar'";
            ds = dacces2.select_method(qry, hat, "Text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                cblsec.DataSource = ds;
                cblsec.DataTextField = "sections";
                cblsec.DataValueField = "sections";
                cblsec.DataBind();
                txtsec.Enabled = true;
                checkBoxListselectOrDeselect(cblsec, true);
                CallCheckboxListChange(chksec, cblsec, txtsec, lblsec.Text, "--Select--");
            }
            else
            {
                txtsec.Enabled = false;
            }
        }
        else
        {
            txtsec.Text = "--Select--";
            txtsec.Enabled = false;
        }
    }

    public void clear()
    {
        errmsg.Visible = false;
        gview.Visible = false;
        gview1.Visible = false;
        CheckBox1.Visible = false;
        CheckBox1.Checked = false;
        lblfrom.Visible = false;
        fromno.Visible = false;
        lblto.Visible = false;
        tono.Visible = false;
        Button2.Visible = false;
        Btnsave.Visible = false;
        Btndelete.Visible = false;
        lblfrom.Enabled = false;
        fromno.Enabled = false;
        lblto.Enabled = false;
        tono.Enabled = false;
        fromno.Text = string.Empty;
        tono.Text = string.Empty;
        txtnoofbatch.Text = string.Empty;
        ddlnobatch.Items.Clear();
        btnsatff.Visible = false;
    }

    protected void chksec_CheckedChanged(object sender, EventArgs e)
    {
        if (chksec.Checked)
        {
            int count = 0;
            if (cblsec.Items.Count > 0)
            {
                for (int i = 0; i < cblsec.Items.Count; i++)
                {
                    cblsec.Items[i].Selected = true;
                    count++;
                }
                txtsec.Text = "Sec(" + count + ")";
            }
        }
        else
        {
            for (int i = 0; i < cblsec.Items.Count; i++)
            {
                cblsec.Items[i].Selected = false;
            }
            txtsec.Text = "--Select--";
        }
    }

    protected void chklstsec_SelectedIndexChanged(object sender, EventArgs e)
    {
        int count = 0;
        for (int i = 0; i < cblsec.Items.Count; i++)
        {
            if (cblsec.Items[i].Selected)
            {
                cblsec.Items[i].Selected = true;
                count++;
            }
        }
        if (count != 0)
        {
            txtsec.Text = "Sec(" + count + ")";
        }
        else
        {
            txtsec.Text = "--Select--";
        }
        if (count == cblsec.Items.Count)
        {
            chksec.Checked = true;
        }
        else
        {
            chksec.Checked = false;
        }
    }

    protected void chklstdegree_SelectedIndexChanged(object sender, EventArgs e)
    {
        int count = 0;
        for (int i = 0; i < cbldegree.Items.Count; i++)
        {
            if (cbldegree.Items[i].Selected)
            {
                cbldegree.Items[i].Selected = true;
                count++;
            }
        }
        if (count != 0)
        {
            txtdegree.Text = "Degree(" + count + ")";
            bindbranch();
        }
        else
        {
            txtdegree.Text = "--Select--";
        }
        if (count == cbldegree.Items.Count)
        {
            chkdegree.Checked = true;
        }
        else
        {
            chkdegree.Checked = false;
        }
    }

    protected void chkdegree_CheckedChanged(object sender, EventArgs e)
    {
        if (chkdegree.Checked)
        {
            int count = 0;
            if (cbldegree.Items.Count > 0)
            {
                for (int i = 0; i < cbldegree.Items.Count; i++)
                {
                    cbldegree.Items[i].Selected = true;
                    count++;
                }
                txtdegree.Text = "Degree(" + count + ")";
            }
            bindbranch();
        }
        else
        {
            for (int i = 0; i < cbldegree.Items.Count; i++)
            {
                cbldegree.Items[i].Selected = false;
            }
            txtdegree.Text = "--Select--";
        }
    }

    protected void chklstbranch_SelectedIndexChanged(object sender, EventArgs e)
    {
        int count = 0;
        for (int i = 0; i < cblbranch.Items.Count; i++)
        {
            if (cblbranch.Items[i].Selected)
            {
                cblbranch.Items[i].Selected = true;
                count++;
            }
        }
        if (count != 0)
        {
            txtbranch.Text = "Branch(" + count + ")";
            bindsubtype();
            bindsubject();
            bindsec();
        }
        else
        {
            txtdegree.Text = "--Select--";
            ddlsubtype.Items.Clear();
        }
        if (count == cblbranch.Items.Count)
        {
            chkbranch.Checked = true;
        }
        else
        {
            chkbranch.Checked = false;
        }
    }

    protected void chkbranch_CheckedChanged(object sender, EventArgs e)
    {
        try
        {
            if (chkbranch.Checked)
            {
                int count = 0;
                if (cblbranch.Items.Count > 0)
                {
                    for (int i = 0; i < cblbranch.Items.Count; i++)
                    {
                        cblbranch.Items[i].Selected = true;
                        count++;
                    }
                    txtbranch.Text = "Branch(" + count + ")";
                }
                bindsubtype();
                bindsubject();
                bindsec();
            }
            else
            {
                for (int i = 0; i < cblbranch.Items.Count; i++)
                {
                    cblbranch.Items[i].Selected = false;
                }
                txtbranch.Text = "--Select--";
                bindsec();
                ddlsubtype.Items.Clear();
                ddlsubject.Items.Clear();
            }
        }
        catch
        {

        }
    }

    protected void ddlstream_SelectedIndexChanged(object sender, EventArgs e)
    {
        BindBatch();
        bindeducation();
        bindsem();
        bindsubtype();
        bindsubject();
        bindsec();
        bindbranch();
        BindBatch();
        clear();
    }

    protected void ddlbatch_SelectedIndexChanged(object sender, EventArgs e)
    {
        bindeducation();
        bindsem();
        binddegree();
        bindbranch();
        bindsubtype();
        bindsubject();
        bindsec();
        clear();
    }

    protected void ddlcourse_SelectedIndexChanged(object sender, EventArgs e)
    {
        bindsem();
        binddegree();
        bindbranch();
        bindsubtype();
        bindsubject();
        bindsec();
        
        clear();
    }

    protected void ddlsem_SelectedIndexChanged(object sender, EventArgs e)
    {
      //  binddegree();
       // bindbranch();
        bindsubtype();
        bindsubject();
        bindsec();
        clear();
    }

    protected void ddlsubtype_SelectedIndexChanged(object sender, EventArgs e)
    {
        bindsubject();
        clear();
    }

    protected void ddlsubject_SelectedIndexChanged(object sender, EventArgs e)
    {
        clear();
    }

    protected void Btngo_Click(object sender, EventArgs e)
    {
        try
        {
            clear();
            //staffloadspread();
            loadspread();
        }
        catch (Exception ex)
        {
            errmsg.Visible = true;
            errmsg.Text = ex.ToString();
        }
    }

    public void loadspread()
    {
        try
        {
            int count = 0;
            int count1 = 0;
            for (int i = 0; i < cbldegree.Items.Count; i++)
            {
                if (cbldegree.Items[i].Selected)
                {
                    count++;
                }
            }
            if (count == 0)
            {
                errmsg.Text = "Choose atleast one Degree";
                errmsg.Visible = true;
                return;
            }

            for (int i = 0; i < cblbranch.Items.Count; i++)
            {
                if (cblbranch.Items[i].Selected)
                {
                    count1++;
                }
            }
            if (count1 == 0)
            {
                errmsg.Text = "Choose atleast one Branch";
                errmsg.Visible = true;
                return;
            }

            dtable2.Columns.Add("Sno");
            dtable2.Columns.Add("degreedetails");
            dtable2.Columns.Add("roll");
            dtable2.Columns.Add("tagroll");
            dtable2.Columns.Add("Reg");
            dtable2.Columns.Add("stdtype");
            dtable2.Columns.Add("stdname");
            dtable2.Columns.Add("Batch");

            if (Session["Rollflag"].ToString() == "1")
            {
                gview1.Columns[2].Visible = true;
            }
            else
            {
                gview1.Columns[2].Visible = false;
            }
            if (Session["Regflag"].ToString() == "1")
            {
                gview1.Columns[3].Visible = true;
            }
            else
            {
                gview1.Columns[2].Visible = false;
            }
            if (Session["Studflag"].ToString() == "1")
            {
                gview1.Columns[4].Visible = true;
            }
            else
            {
                gview1.Columns[4].Visible = false;
            }

            string batchyear = ddlbatch.SelectedItem.ToString();
            string sem = ddlsem.SelectedValue.ToString();
            string typeval = string.Empty;
            if (ddlstream.Items.Count > 0 && ddlstream.Enabled == true)
            {
                typeval = " and c.type='" + ddlstream.SelectedItem.ToString() + "'";
            }
            string strorder = ",r.Roll_No";
            string serialno = dacces2.GetFunction("select LinkValue from inssettings where college_code=" + Session["collegecode"].ToString() + " and linkname='Student Attendance'");
            if (serialno.Trim() == "1")
            {
                strorder = ",r.serialno";
            }
            else
            {
                string orderby_Setting = dacces2.GetFunction("select value from master_Settings where settings='order_by'");
                if (orderby_Setting == "0")
                {
                    strorder = ",r.Roll_No";
                }
                else if (orderby_Setting == "1")
                {
                    strorder = ",r.Reg_No";
                }
                else if (orderby_Setting == "2")
                {
                    strorder = ",r.Stud_Name";
                }
                else if (orderby_Setting == "0,1,2")
                {
                    strorder = ",r.Roll_No,r.Reg_No,r.Stud_Name";
                }
                else if (orderby_Setting == "0,1")
                {
                    strorder = ",r.Roll_No,r.Reg_No";
                }
                else if (orderby_Setting == "1,2")
                {
                    strorder = ",r.Reg_No,r.Stud_Name";
                }
                else if (orderby_Setting == "0,2")
                {
                    strorder = ",r.Roll_No,r.Stud_Name";
                }
            }
            string secVal = string.Empty;
            string secstr = string.Empty;
            if (txtsec.Enabled == true)
            {
                for (int i = 0; i < cblsec.Items.Count; i++)
                {
                    if (cblsec.Items[i].Selected)
                    {
                        if (string.IsNullOrEmpty(secstr))
                        {
                            secstr = cblsec.Items[i].Text;
                        }
                        else
                        {
                            secstr = secstr + "','" + cblsec.Items[i].Text;
                        }
                    }
                }
                secVal = " and sections in('" + secstr + "')";
            }
            string degcod = string.Empty;
            for (int j1 = 0; j1 < cblbranch.Items.Count; j1++)
            {
                if (cblbranch.Items[j1].Selected)
                {
                    if (string.IsNullOrEmpty(degcod))
                        degcod = cblbranch.Items[j1].Value;
                    else
                        degcod = degcod + "','" + cblbranch.Items[j1].Value;
                }
            }

            
            htable.Add("subject", ddlsubject.SelectedValue.ToString());
            htable.Add("batch", ddlbatch.SelectedValue.ToString());
            htable.Add("currentsem", ddlsem.SelectedValue.ToString());
            htable.Add("strorder", strorder);

            string sqlstr = "select r.Batch_Year,c.Course_Name,de.dept_acronym,sy.semester,r.Sections,r.roll_no,r.reg_no, r.stud_name,r.stud_type,r.serialno,r.Adm_Date,s.subject_no,sc.batch from Registration r,subject s,subjectChooser sc,syllabus_master sy,Degree d,Course c,Department de where r.Roll_No=sc.roll_no and sc.subject_no=s.subject_no and r.Current_Semester=sc.semester and d.Degree_Code=sy.degree_code and d.Course_Id=c.Course_Id and d.Dept_Code=de.Dept_Code and sy.syll_code=s.syll_code and sy.semester=r.Current_Semester and sy.degree_code=r.degree_code  and s.subject_code='" + ddlsubject.SelectedValue.ToString() + "' and r.Batch_Year='" + ddlbatch.SelectedValue.ToString() + "' and r.Current_Semester='" + ddlsem.SelectedValue.ToString() + "' and r.cc=0 and r.delflag=0 " + secVal + " and r.Exam_Flag<>'debar' and r.degree_code in ('" + degcod + "') and r.college_code='" + Convert.ToString(Session["collegecode"]) + "' ORDER BY r.Batch_Year,c.Course_Name,de.dept_acronym,sy.semester,r.Sections " + strorder + "";  //modified by Mullai

            DataSet gvds = dacces2.select_method_wo_parameter(sqlstr, "text");

            //string sqlstr = "select r.Batch_Year,c.Course_Name,de.dept_acronym,sy.semester,r.Sections,r.roll_no,r.reg_no, r.stud_name,r.stud_type,r.serialno,r.Adm_Date,s.subject_no,sc.batch from Registration r,subject s,subjectChooser sc,syllabus_master sy,Degree d,Course c,Department de where r.Roll_No=sc.roll_no and sc.subject_no=s.subject_no and r.Current_Semester=sc.semester and d.Degree_Code=sy.degree_code and d.Course_Id=c.Course_Id and d.Dept_Code=de.Dept_Code and sy.syll_code=s.syll_code and sy.semester=r.Current_Semester and sy.degree_code=r.degree_code  and s.subject_code='15IT35C' and r.Batch_Year='2017' and r.Current_Semester='3' and r.cc=0 and r.delflag=0 and r.Exam_Flag<>'debar' ORDER BY r.Batch_Year,c.Course_Name,de.dept_acronym,sy.semester,r.Sections ,r.Reg_No";
            //DataSet gvds = dacces2.select_method("subwisebatchallocation", htable, "sp");


            int srno = 0;
            if (gvds.Tables.Count > 0 && gvds.Tables[0].Rows.Count > 0)
            {
                //FpSpread1.Visible = true;
                gview1.Visible = true;
                CheckBox1.Visible = true;
                lblfrom.Visible = true;
                fromno.Visible = true;
                lblto.Visible = true;
                tono.Visible = true;
                Button2.Visible = true;
                Btnsave.Visible = true;
                Btndelete.Visible = true;
                for (int i = 0; i < gvds.Tables[0].Rows.Count; i++)
                {
                    dtrow2 = dtable2.NewRow();
                    srno++;
                    
                    string rollno = gvds.Tables[0].Rows[i]["Roll_No"].ToString();
                    string regno = gvds.Tables[0].Rows[i]["Reg_No"].ToString();
                    string stype = gvds.Tables[0].Rows[i]["Stud_Type"].ToString();
                    string sname = gvds.Tables[0].Rows[i]["Stud_Name"].ToString();
                    string subno = gvds.Tables[0].Rows[i]["subject_no"].ToString();
                    string batch = gvds.Tables[0].Rows[i]["batch"].ToString();
                    string batch_year = gvds.Tables[0].Rows[i]["Batch_Year"].ToString();
                    string course = gvds.Tables[0].Rows[i]["Course_Name"].ToString();
                    string deptacr = gvds.Tables[0].Rows[i]["dept_acronym"].ToString();
                    string semester = gvds.Tables[0].Rows[i]["semester"].ToString();
                    string section = gvds.Tables[0].Rows[i]["Sections"].ToString();
                    string degreedetails = string.Empty;
                    if (section.Trim() != "")
                    {
                        degreedetails = batch_year + "-" + course + "-" + deptacr + "-" + semester + "-" + section;
                    }
                    else
                    {
                        degreedetails = batch_year + "-" + course + "-" + deptacr + "-" + semester;
                    }
                    if ((srno % 2) == 0)
                    {
                        //FpSpread1.Sheets[0].Rows[FpSpread1.Sheets[0].RowCount - 2].BackColor = System.Drawing.Color.LightGray;
                    }

                    dtrow2["Sno"] = srno;
                    dtrow2["degreedetails"] = degreedetails;
                    dtrow2["roll"] = rollno;
                    dtrow2["tagroll"] = subno;
                    dtrow2["Reg"] = regno;
                    dtrow2["stdtype"] = stype;
                    dtrow2["stdname"] = sname;
                    dtrow2["Batch"] = batch;

                    
                    dtable2.Rows.Add(dtrow2);
                }
                staffloadspread();
            }
            else
            {
                errmsg.Visible = true;
                errmsg.Text = "No Records Found";
            }
            //FpSpread1.Sheets[0].AutoPostBack = false;
            //FpSpread1.SaveChanges();
            //FpSpread1.Height = 1000;
            //FpSpread1.Sheets[0].PageSize = FpSpread1.Sheets[0].RowCount;

            gview1.DataSource = dtable2;
            gview1.DataBind();
            gview1.Visible = true;

            for (int i = 0; i < gview1.Rows.Count; i++)
            {
                for (int cell = 0; cell < gview1.Rows[i].Cells.Count; cell++)
                {
                    if (gview1.HeaderRow.Cells[cell].Text.Trim() == "S.No")
                    {
                        Label lab = (Label)gview1.Rows[i].FindControl("lbl_sno");
                        int num = Convert.ToInt32(lab.Text);
                        if (num % 2 == 0)
                        {
                            gview1.Rows[i].BackColor = System.Drawing.Color.LightGray;
                        }
                        gview1.Rows[i].Cells[cell].HorizontalAlign = HorizontalAlign.Center;
                    }
                }
            }
        }
        catch (Exception ex)
        {
            errmsg.Visible = true;
            errmsg.Text = ex.ToString();
        }
    }

    protected void CheckBox1_CheckedChanged(object sender, EventArgs e)
    {
        lblfrom.Enabled = false;
        fromno.Enabled = false;
        lblto.Enabled = false;
        tono.Enabled = false;
        if (CheckBox1.Checked == true)
        {
            lblfrom.Enabled = true;
            fromno.Enabled = true;
            lblto.Enabled = true;
            tono.Enabled = true;
        }
    }

    protected void txtnoofbatch_TextChanged(object sender, EventArgs e)
    {
        try
        {
            ddlnobatch.Items.Clear();
            string strnobatch = txtnoofbatch.Text.ToString();
            if (strnobatch.Trim() == "")
            {
                errmsg.Visible = true;
                errmsg.Text = "Please Enter The No Of Batch";
                return;
            }
            int noofbatch = Convert.ToInt32(strnobatch);
            if (noofbatch == 0 || noofbatch > 26)
            {
                errmsg.Visible = true;
                errmsg.Text = "Please Enter The No Of Batch Value Between 0 To 26 Range";
                return;
            }
            for (int i = noofbatch; i >= 1; i--)
            {
                string batch = string.Empty;
                if (i == 1)
                {
                    batch = "A";
                }
                else if (i == 2)
                {
                    batch = "B";
                }
                else if (i == 3)
                {
                    batch = "C";
                }
                else if (i == 4)
                {
                    batch = "D";
                }
                else if (i == 5)
                {
                    batch = "E";
                }
                else if (i == 6)
                {
                    batch = "F";
                }
                else if (i == 7)
                {
                    batch = "G";
                }
                else if (i == 8)
                {
                    batch = "H";
                }
                else if (i == 9)
                {
                    batch = "I";
                }
                else if (i == 10)
                {
                    batch = "J";
                }
                else if (i == 11)
                {
                    batch = "K";
                }
                else if (i == 12)
                {
                    batch = "L";
                }
                else if (i == 13)
                {
                    batch = "M";
                }
                else if (i == 14)
                {
                    batch = "N";
                }
                else if (i == 15)
                {
                    batch = "0";
                }
                else if (i == 16)
                {
                    batch = "P";
                }
                else if (i == 17)
                {
                    batch = "Q";
                }
                else if (i == 18)
                {
                    batch = "R";
                }
                else if (i == 19)
                {
                    batch = "S";
                }
                else if (i == 20)
                {
                    batch = "T";
                }
                else if (i == 21)
                {
                    batch = "U";
                }
                else if (i == 22)
                {
                    batch = "V";
                }
                else if (i == 23)
                {
                    batch = "W";
                }
                else if (i == 24)
                {
                    batch = "X";
                }
                else if (i == 25)
                {
                    batch = "Y";
                }
                else if (i == 26)
                {
                    batch = "Z";
                }
                ddlnobatch.Items.Insert(0, batch);
            }
        }
        catch (Exception ex)
        {
            errmsg.Visible = true;
            errmsg.Text = ex.ToString();
        }
    }

    protected void ddlnobatch_SelectedIndexChanged(object sender, EventArgs e)
    {
        fromno.Text = string.Empty;
        tono.Text = string.Empty;
    }

    protected void selectgo_Click(object sender, EventArgs e)
    {
        try
        {
            //FpSpread1.SaveChanges();
            //int noorstu = FpSpread1.Sheets[0].RowCount;
            int noorstu = gview1.Rows.Count;
            if (ddlnobatch.Items.Count == 0)
            {
                errmsg.Visible = true;
                errmsg.Text = "Please Enter The No Of Batch";
                return;
            }
            string strbatch = ddlnobatch.Text.ToString();
            string strfrange = fromno.Text.ToString();
            string strtrange = tono.Text.ToString();
            if (strfrange.Trim() == "")
            {
                errmsg.Visible = true;
                errmsg.Text = "Please Enter The From Range";
                return;
            }
            if (strtrange.Trim() == "")
            {
                errmsg.Visible = true;
                errmsg.Text = "Please Enter The To Range";
                return;
            }
            int frange = Convert.ToInt32(strfrange);
            if (frange == 0)
            {
                errmsg.Visible = true;
                errmsg.Text = "Please Enter The From Value Greater Than Zero Range";
                return;
            }
            int trange = Convert.ToInt32(strtrange);
            if (trange == 0)
            {
                errmsg.Visible = true;
                errmsg.Text = "Please Enter The To Value Greater Than Zero Range";
                return;
            }
            if (trange < frange)
            {
                errmsg.Visible = true;
                errmsg.Text = "Please Enter The To Value Greater Than Or Equal To From Range Range";
                return;
            }
            if (noorstu < trange || noorstu < frange)
            {
                errmsg.Visible = true;
                errmsg.Text = "Please Enter The Range Value Must Be Less Than Or Equal To Student Count Range Range";
                return;
            }
            for (int r = 0; r < noorstu; r++)
            {
                if (r + 1 >= frange && r < trange)
                {
                    CheckBox cbk = (CheckBox)gview1.Rows[r].FindControl("selectchk");
                    cbk.Checked = true;
                }
            }
        }
        catch (Exception ex)
        {
            errmsg.Visible = true;
            errmsg.Text = ex.ToString();
        }
    }

    protected void Btnsave_Click(object sender, EventArgs e)
    {
        try
        {
            int savevalue = 1;
            string noallostu = string.Empty;
            string two = string.Empty;
            string four = string.Empty;

            if (ddlnobatch.Items.Count == 0)
            {
                errmsg.Visible = true;
                errmsg.Text = "Please Enter The No Of Batch";
                return;
            }
            string batchval = ddlnobatch.Text.ToString();
            Boolean delg = false;
            for (int r = 0; r < gview1.Rows.Count; r++)
            {
                CheckBox cbk = (CheckBox)gview1.Rows[r].FindControl("selectchk");
                if (cbk.Checked == true)
                {
                    delg = true;
                }
            }

            if (delg == false)
            {
                errmsg.Visible = true;
                errmsg.Text = "Please Select The Student And The Proceed";
                return;
            }
            //for (int r = 0; r < gview1.Rows.Count; r++)
            //{
            //    CheckBox cbk = (CheckBox)gview1.Rows[r].FindControl("selectchk");
            //    if (cbk.Checked == true)
            //    {
            //        Label batchval1 = (Label)gview1.Rows[r].FindControl("lbl_batch");
            //        batchval = batchval1.Text;
            //        if (batchval.Trim() == "")
            //        {
            //            if (noallostu == "")
            //            {
            //                //noallostu = FpSpread1.Sheets[0].Cells[r, 2].Text + "- " + FpSpread1.Sheets[0].Cells[r, 4].Text;
            //                Label twoo = (Label)gview1.Rows[r].FindControl("lbl_Roll");
            //                two = twoo.Text;
            //                Label fou = (Label)gview1.Rows[r].FindControl("lbl_stdname");
            //                four = fou.Text;

            //                noallostu = two + "-" + four;
            //            }
            //            else
            //            {
            //                //noallostu = noallostu + ", " + FpSpread1.Sheets[0].Cells[r, 2].Text + "- " + FpSpread1.Sheets[0].Cells[r, 4].Text;
            //                noallostu = noallostu + ", " + two + "-" + four;
            //            }
            //        }
            //    }
            //}
            //if (noallostu.Trim() != "")
            //{
            //    errmsg.Visible = true;
            //    errmsg.Text = "Please Allot The Batch For Following Student's : " + noallostu + " ";
            //    return;
            //}
            string strbatch = dacces2.GetFunction("Select Distinct Batch From SubjectChooser sc,subject s where sc.subject_no=s.subject_no and s.subject_code='" + ddlsubject.SelectedValue.ToString() + "' and semester='" + ddlsem.SelectedItem.ToString() + "' and isnull(Batch,'')<>''");
            if (strbatch.Trim() != "" && strbatch.Trim() != "0")
            {
                savevalue = 2;
            }
            string updatequety = string.Empty;
            int insupdval = 0;
            //for (int r = 0; r < FpSpread1.Sheets[0].RowCount; r++)
            for (int r = 0; r < gview1.Rows.Count; r++)
            {
                CheckBox cbk = (CheckBox)gview1.Rows[r].FindControl("selectchk");
                if (cbk.Checked == true)
                {
                    //string subno = FpSpread1.Sheets[0].Cells[r, 2].Tag.ToString();
                    //string rollno = FpSpread1.Sheets[0].Cells[r, 2].Text.ToString();

                    Label labtag = (Label)gview1.Rows[r].FindControl("lbl_tagroll");
                    string subno = labtag.Text;
                    Label labroll = (Label)gview1.Rows[r].FindControl("lbl_Roll");
                    string rollno = labroll.Text;

                    updatequety = "Update SubjectChooser set Batch='" + batchval + "' where subject_no='" + subno + "' and roll_no='" + rollno + "' and semester='" + ddlsem.SelectedItem.ToString() + "'";
                    insupdval = dacces2.update_method_wo_parameter(updatequety, "Text");
                }
            }
            string entrycode = Session["Entry_Code"].ToString();
            string formname = "Subject Wise Batch Allocation";
            string toa = DateTime.Now.ToString("MM/dd/yyy hh:mm:ss tt");
            string doa = DateTime.Now.ToString("MM/dd/yyy");
            string details = "Stream :" + ddlstream.Text + "; Batch :" + ddlbatch.Text + "; Course:" + ddlcourse.Text + "; Sem :" + ddlsem.Text + " ; Subject Type :" + ddlsubtype.Text + " ;Subject :" + ddlsubject.Text;
            string modules = "0";
            string act_diff = " ";
            string ctsname = "Update The Subject Batch Allocation";
            if (savevalue == 1)
            {
                ctsname = "Save the Subject Batch Allocation";
            }
            string strlogdetails = "insert into UserLog (Entry_Code,Form_Name,UsrAction,TOA,DOA,Details,Module,Act_Diff,ctrNam) values ('" + entrycode + "','" + formname + "','" + savevalue + "','" + toa + "','" + doa + "','" + details + "','" + modules + "','" + act_diff + "','" + ctsname + "')";
            int a = dacces2.update_method_wo_parameter(strlogdetails, "Text");
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "clientScript", "alert('Saved Successfully')", true);
            loadspread();
        }
        catch (Exception ex)
        {
            errmsg.Visible = true;
            errmsg.Text = ex.ToString();
        }
    }

    protected void Btndelete_Click(object sender, EventArgs e)
    {
        try
        {
            Boolean delg = false;
            for (int r = 0; r < gview1.Rows.Count; r++)
            {
                CheckBox cbk = (CheckBox)gview1.Rows[r].FindControl("selectchk");
                if (cbk.Checked == true)
                {
                    delg = true;
                }
            }
            if (delg == false)
            {
                errmsg.Visible = true;
                errmsg.Text = "Please Select The Student For Delete";
                return;
            }
            string updatequety = string.Empty;
            int insupdval = 0;
            //for (int r = 0; r < FpSpread1.Sheets[0].RowCount; r++)
            for (int r = 0; r < gview1.Rows.Count; r++)
            {
                CheckBox cbk = (CheckBox)gview1.Rows[r].FindControl("selectchk");
                if (cbk.Checked == true)
                {
                    //string subno = FpSpread1.Sheets[0].Cells[r, 2].Tag.ToString();
                    //string rollno = FpSpread1.Sheets[0].Cells[r, 2].Text.ToString();

                    Label labtag = (Label)gview1.Rows[r].FindControl("lbl_tagroll");
                    string subno = labtag.Text;
                    Label labroll = (Label)gview1.Rows[r].FindControl("lbl_Roll");
                    string rollno = labroll.Text;

                    updatequety = "Update SubjectChooser set Batch='' where subject_no='" + subno + "' and roll_no='" + rollno + "' and semester='" + ddlsem.SelectedItem.ToString() + "'";
                    insupdval = dacces2.update_method_wo_parameter(updatequety, "Text");
                }
            }
            string entrycode = Session["Entry_Code"].ToString();
            string formname = "Subject Wise Batch Allocation";
            string toa = DateTime.Now.ToString("MM/dd/yyy hh:mm:ss tt");
            string doa = DateTime.Now.ToString("MM/dd/yyy");
            string details = "Stream :" + ddlstream.Text + "; Batch :" + ddlbatch.Text + "; Course:" + ddlcourse.Text + "; Sem :" + ddlsem.Text + " ; Subject Type :" + ddlsubtype.Text + " ;Subject :" + ddlsubject.Text;
            string modules = "0";
            string act_diff = " ";
            string ctsname = "Delete The Subject Batch Allocation";
            string strlogdetails = "insert into UserLog (Entry_Code,Form_Name,UsrAction,TOA,DOA,Details,Module,Act_Diff,ctrNam) values ('" + entrycode + "','" + formname + "','2','" + toa + "','" + doa + "','" + details + "','" + modules + "','" + act_diff + "','" + ctsname + "')";
            int a = dacces2.update_method_wo_parameter(strlogdetails, "Text");
            //ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "clientScript", "alert('Saved Successfully')", true);
            //loadspread();
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "clientScript", "alert('Deleted Successfully')", true);
            loadspread();
        }
        catch (Exception ex)
        {
            errmsg.Visible = true;
            errmsg.Text = ex.ToString();
        }
    }

    public void staffloadspread()
    {
        try
        {
            dtable.Columns.Add("SNo");
            dtable.Columns.Add("Staff_Code");
            dtable.Columns.Add("Staff_Name");
            dtable.Columns.Add("Batch");
            DataSet dsbatch = new DataSet();
            dsbatch.Clear();

            btnsatff.Visible = true;

            string sqlstr = "select distinct st.staff_code,sm.staff_name,LTRIM(RTRIM(ISNULL(st.Stud_batch,''))) as Stud_batch from subject s,staff_selector st ,syllabus_master sy,staffmaster sm where  st.subject_no=s.subject_no and st.batch_year=sy.Batch_Year and s.syll_code=s.syll_code and sm.staff_code=st.staff_code ";
            sqlstr = sqlstr + " and s.subject_code='" + ddlsubject.SelectedValue.ToString() + "' and sy.Batch_Year='" + ddlbatch.SelectedValue.ToString() + "' and sy.Semester='" + ddlsem.SelectedValue.ToString() + "'  order by st.staff_code,sm.staff_name,Stud_batch";
            DataSet gvds = dacces2.select_method_wo_parameter(sqlstr, "text");
            int srno = 0;
            int scont = 0;
            if (gvds.Tables.Count > 0 && gvds.Tables[0].Rows.Count > 0)
            {
                sqlstr = "select distinct LTRIM(RTRIM(ISNULL(sc.batch,''))) as batch from Registration r,subject s,subjectChooser sc,syllabus_master sy where r.Roll_No=sc.roll_no and sc.subject_no=s.subject_no and r.Current_Semester=sc.semester and sy.syll_code=s.syll_code and sy.semester=r.Current_Semester and sy.degree_code=r.degree_code  and s.subject_code='" + ddlsubject.SelectedValue.ToString() + "' and sy.Batch_Year='" + ddlbatch.SelectedValue.ToString() + "' and r.Current_Semester='" + ddlsem.SelectedValue.ToString() + "' and r.cc=0 and r.delflag=0 and r.Exam_Flag<>'debar' and LTRIM(RTRIM(ISNULL(sc.batch,'')))<>''";
                dsbatch = dacces2.select_method_wo_parameter(sqlstr, "text");
                string[] stuubatchva = new string[1];
                if (dsbatch.Tables.Count > 0 && dsbatch.Tables[0].Rows.Count > 0)
                {
                    scont++;
                    stuubatchva = new string[dsbatch.Tables[0].Rows.Count + 1];
                    for (int s = 0; s < dsbatch.Tables[0].Rows.Count; s++)
                    {
                        stuubatchva[s] = Convert.ToString(dsbatch.Tables[0].Rows[s]["batch"]).Trim();
                    }
                }
                stuubatchva[stuubatchva.Length - 1] = string.Empty;
                for (int i = 0; i < gvds.Tables[0].Rows.Count; i++)
                {
                    dtrow = dtable.NewRow();
                    srno++;
                    dtrow[0] = srno.ToString();

                    string scode = Convert.ToString(gvds.Tables[0].Rows[i]["staff_code"]).Trim();
                    string sname = Convert.ToString(gvds.Tables[0].Rows[i]["staff_name"]).Trim();
                    string stubatch = Convert.ToString(gvds.Tables[0].Rows[i]["Stud_batch"]).Trim();

                    dtrow[1] = scode;
                    dtrow[2] = sname;
                    dtrow[3] = stubatch;
                    dtable.Rows.Add(dtrow);
                }
            }
            if (srno == 0)
            {
                btnsatff.Visible = false;
            }

            gview.DataSource = dtable;
            gview.DataBind();
            gview.Visible = true;

            if (scont > 0)
            {
                for (int ji = 0; ji < gview.Rows.Count; ji++)
                {
                    DropDownList ddl = (gview.Rows[ji].FindControl("lblddlbatch") as DropDownList);
                    ddl.DataSource = dsbatch;
                    ddl.DataTextField = "batch";
                    ddl.DataValueField = "batch";
                    ddl.DataBind();
                    ddl.Items.Insert(0, "");
                }
            }

                for (int i = 0; i < gview.Rows.Count; i++)
                {
                    string btch = dtable.Rows[i][3].ToString();
                    (gview.Rows[i].FindControl("lblddlbatch") as DropDownList).SelectedIndex = (gview.Rows[i].FindControl("lblddlbatch") as DropDownList).Items.IndexOf((gview.Rows[i].FindControl("lblddlbatch") as DropDownList).Items.FindByValue(btch));
                    for (int cell = 0; cell < gview.Rows[i].Cells.Count; cell++)
                    {
                        if (gview.HeaderRow.Cells[cell].Text.Trim() == "S.No")
                        {
                            string strnum = (gview.Rows[i].FindControl("lblsno") as Label).Text;
                            int num = Convert.ToInt16(strnum); //Convert.ToInt32(gview.Rows[i].Cells[cell].Text);
                            if (num % 2 == 0)
                            {
                                gview.Rows[i].BackColor = System.Drawing.Color.LightGray;
                            }
                            //gview.Rows[i].Cells[cell].HorizontalAlign = HorizontalAlign.Center;
                        }
                    }
                }
        }
        catch (Exception ex)
        {
            errmsg.Visible = true;
            errmsg.Text = ex.ToString();
        }
    }

    protected void gview_OnRowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.Header)
        {
            e.Row.Cells[0].Width = 100;
            e.Row.Cells[1].Width = 100;
            e.Row.Cells[2].Width = 100;
            e.Row.Cells[3].Width = 100;
        }
    }

    protected void gview_OnDataBinding(object sender, EventArgs e)
    {

    }

    protected void btnsatff_Click(object sender, EventArgs e)
    {
        try
        {
            for (int r = 0; r < gview.Rows.Count; r++)
            {
                string staffcode = (gview.Rows[r].FindControl("lblstaffcode") as Label).Text; //Convert.ToString(gview.Rows[r].Cells[1].Text).Trim();
                DropDownList ddl = (gview.Rows[r].FindControl("lblddlbatch") as DropDownList);
                string stubatch = ddl.SelectedValue;//Convert.ToString(gview.Rows[r].Cells[3].Text).Trim();
                string insertquery = "";
                if (stubatch.Trim() == "&nbsp;")
                {
                    stubatch = string.Empty;
                    insertquery = "update st set Stud_batch='" + stubatch + "' from subject s,staff_selector st ,syllabus_master sy,staffmaster sm where  st.subject_no=s.subject_no and s.syll_code=s.syll_code and sm.staff_code=st.staff_code and s.subject_code='" + ddlsubject.SelectedValue.ToString() + "' and sy.Batch_Year='" + ddlbatch.SelectedValue.ToString() + "' and sy.Semester='" + ddlsem.SelectedValue.ToString() + "' and st.staff_code='" + staffcode + "'";
                }
                else
                {
                    insertquery = "update st set Stud_batch='" + stubatch + "' from subject s,staff_selector st ,syllabus_master sy,staffmaster sm where  st.subject_no=s.subject_no and s.syll_code=s.syll_code and sm.staff_code=st.staff_code and s.subject_code='" + ddlsubject.SelectedValue.ToString() + "' and sy.Batch_Year='" + ddlbatch.SelectedValue.ToString() + "' and sy.Semester='" + ddlsem.SelectedValue.ToString() + "' and st.staff_code='" + staffcode + "'";
                }

                int insval = dacces2.update_method_wo_parameter(insertquery, "text");
            }
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "clientScript", "alert('Saved Successfully')", true);
        }
        catch (Exception ex)
        {
            errmsg.Visible = true;
            errmsg.Text = ex.ToString();
        }
    }

    private void checkBoxListselectOrDeselect(CheckBoxList cbl, bool selected = true)
    {
        try
        {
            foreach (wc.ListItem li in cbl.Items)
            {
                li.Selected = selected;
            }
        }
        catch
        {
        }
    }

    private void CallCheckboxListChange(CheckBox cb, CheckBoxList cbl, TextBox txt, string dipst, string deft)
    {
        try
        {
            int sel = 0;
            int count = 0;
            string name = string.Empty;
            cb.Checked = false;
            txt.Text = deft;
            for (sel = 0; sel < cbl.Items.Count; sel++)
            {
                if (cbl.Items[sel].Selected == true)
                {
                    count++;
                    name = Convert.ToString(cbl.Items[sel].Text);
                }
            }
            if (count > 0)
            {
                if (count == 1)
                {
                    txt.Text = "" + name + "";
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
        }
        catch { }
    }
}
using System;
using System.Linq;
using System.Web.UI;
using System.Data;
using System.Collections;

public partial class Foundationsubjectselection : System.Web.UI.Page
{
    DataSet ds = new DataSet();
    DAccess2 d2 = new DAccess2();
    Hashtable hat = new Hashtable();
    string user_code = "";
    string college_code = "";
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["collegecode"] == null)
        {
            Response.Redirect("~/Default.aspx");
        }
        user_code = Session["usercode"].ToString();
        college_code = Session["collegecode"].ToString();
        if (!IsPostBack)
        {
            bindcollege();
            BindBatch();
            degree();
            bindbranch();
            bindsem();
        }
    }

    public void bindcollege()
    {
        try
        {
            string group_code = Session["group_code"].ToString();
            string columnfield = "";
            if (group_code.Contains(';'))
            {
                string[] group_semi = group_code.Split(';');
                group_code = group_semi[0].ToString();
            }
            if ((group_code.ToString().Trim() != "") && (Session["single_user"].ToString() != "1" && Session["single_user"].ToString() != "true" && Session["single_user"].ToString() != "TRUE" && Session["single_user"].ToString() != "True"))
            {
                columnfield = " and group_code='" + group_code + "'";
            }
            else
            {
                columnfield = " and user_code='" + Session["usercode"] + "'";
            }
            hat.Clear();
            hat.Add("column_field", columnfield.ToString());
            ds = d2.select_method("bind_college", hat, "sp");
            ddlcollege.Items.Clear();
            if (ds.Tables[0].Rows.Count > 0)
            {
                ddlcollege.Enabled = true;
                ddlcollege.DataSource = ds;
                ddlcollege.DataTextField = "collname";
                ddlcollege.DataValueField = "college_code";
                ddlcollege.DataBind();
            }
        }
        catch
        {
        }
    }

    public void BindBatch()
    {
        try
        {
            ddlbatch.Items.Clear();
            ds = d2.BindBatch();
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

        }
    }

    public void degree()
    {
        try
        {

            ds = d2.select_method_wo_parameter("select distinct degree.course_id,course.course_name from degree,course,deptprivilages where    course.course_id=degree.course_id and course.college_code = degree.college_code and  degree.college_code='" + ddlcollege.SelectedItem.Value + "' and deptprivilages.Degree_code=degree.Degree_code", "Text");
            ddldegree.Items.Clear();
            int count = ds.Tables[0].Rows.Count;
            if (count > 0)
            {
                ddldegree.DataSource = ds;
                ddldegree.DataTextField = "course_name";
                ddldegree.DataValueField = "course_id";
                ddldegree.DataBind();

            }

        }
        catch (Exception ex)
        {
        }
    }

    public void bindbranch()
    {
        try
        {
            ddldepartment.Items.Clear();
            string commname = "";
            string branch = ddldegree.SelectedItem.Value;
            if (branch != "")
            {
                commname = "select distinct degree.degree_code,department.dept_name,degree.Acronym  from degree,department,course,deptprivilages where course.course_id=degree.course_id  and department.dept_code=degree.dept_code and course.college_code = degree.college_code and department.college_code = degree.college_code and degree.course_id in('" + branch + "') and deptprivilages.Degree_code=degree.Degree_code ";
            }
            else
            {
                commname = " select distinct degree.degree_code,department.dept_name,degree.Acronym  from degree,department,course,deptprivilages where course.course_id=degree.course_id  and department.dept_code=degree.dept_code and course.college_code = degree.college_code and department.college_code = degree.college_code and degree.course_id in('" + branch + "') and deptprivilages.Degree_code=degree.Degree_code";
            }
            ds = d2.select_method_wo_parameter(commname, "Text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                ddldepartment.DataSource = ds;
                ddldepartment.DataTextField = "dept_name";
                ddldepartment.DataValueField = "degree_code";
                ddldepartment.DataBind();
            }


        }
        catch (Exception ex)
        {

        }
    }

    public void bindsem()
    {
        try
        {
            ds.Clear();
            ddlsem.Items.Clear();
            ds = d2.BindSem(ddldepartment.SelectedItem.Value, ddlbatch.SelectedItem.Text, ddlcollege.SelectedItem.Value);
            if (ds.Tables[0].Rows.Count > 0)
            {
                string duration = Convert.ToString(ds.Tables[0].Rows[0][0]);
                if (duration.Trim() != "")
                {
                    for (int i = 1; i <= Convert.ToInt32(duration); i++)
                    {
                        ddlsem.Items.Add(Convert.ToString(i));
                    }
                }

            }

        }
        catch
        {

        }
    }

    protected void ddlbatch_Change(object sender, EventArgs e)
    {
        try
        {
            bindsem();
        }
        catch
        {

        }
    }

    protected void ddldepartment_Change(object sender, EventArgs e)
    {

    }

    protected void ddldegree_Change(object sender, EventArgs e)
    {
        bindbranch();
        bindsem();
    }

    protected void btngo_click(object sender, EventArgs e)
    {
        try
        {
            rdblist.Items.Clear();
            rdblist.Visible = false;
            rdblist2.Visible = false;
            rdbsingsubject.Visible = false;
            cbmultiplesubject.Visible = false;
            btnok.Visible = false;
            btnsave.Visible = false;
            cbdepanttamil.Visible = false;
            string query = "select subType_no,subject_type  from syllabus_master sy,sub_sem s where sy.syll_code =s.syll_code and s.ElectivePap =1 and Batch_Year =" + ddlbatch.SelectedItem.Text + " and degree_code =" + ddldepartment.SelectedItem.Value + " and semester =" + ddlsem.SelectedItem.Text + "";
            ds.Clear();
            ds = d2.select_method_wo_parameter(query, "Text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                rdblist.DataSource = ds;
                rdblist.DataTextField = "subject_type";
                rdblist.DataValueField = "subType_no";
                rdblist.DataBind();
                rdblist.Visible = true;
                btnok.Visible = true;
            }
            else
            {
                rdblist.Visible = false;
                ScriptManager.RegisterStartupScript(base.Page, this.GetType(), ("dialogJavascript" + this.ID), "alert(\"No Records Found\");", true);
            }

        }
        catch
        {

        }
    }

    protected void rdblist_Change(object sender, EventArgs e)
    {
        try
        {
            rdblist2.Items.Clear();
            string query = "select subType_no,subject_type  from syllabus_master sy,sub_sem s where sy.syll_code =s.syll_code and s.ElectivePap =1 and Batch_Year =" + ddlbatch.SelectedItem.Text + " and degree_code =" + ddldepartment.SelectedItem.Value + " and semester =" + ddlsem.SelectedItem.Text + " and subType_no <>'" + rdblist.SelectedItem.Value + "'";
            ds.Clear();
            ds = d2.select_method_wo_parameter(query, "Text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                rdblist2.DataSource = ds;
                rdblist2.DataTextField = "subject_type";
                rdblist2.DataValueField = "subType_no";
                rdblist2.DataBind();
                rdblist2.Visible = true;

            }
            else
            {
                rdblist2.Visible = false;

            }
        }
        catch
        {

        }
    }

    protected void btn_ok(object sender, EventArgs e)
    {
        try
        {
            btnsave.Visible = true;
            cbdepanttamil.Visible = true;
            cbdepanttamil.Checked = false;
            string subjectquery = " select subject_name,subject_no  from syllabus_master sy,sub_sem sm,subject s where sy.syll_code =sm.syll_code   and sm.subType_no =s.subType_no and sm.subType_no ='" + rdblist.SelectedItem.Value + "' and sm.ElectivePap =1 and Batch_Year =" + ddlbatch.SelectedItem.Text + " and degree_code =" + ddldepartment.SelectedItem.Value + " and semester =" + ddlsem.SelectedItem.Text + "";
            subjectquery = subjectquery + "  select subject_name,subject_no  from syllabus_master sy,sub_sem sm,subject s where sy.syll_code =sm.syll_code   and sm.subType_no =s.subType_no and sm.subType_no ='" + rdblist2.SelectedItem.Value + "' and sm.ElectivePap =1 and Batch_Year =" + ddlbatch.SelectedItem.Text + " and degree_code =" + ddldepartment.SelectedItem.Value + " and semester =" + ddlsem.SelectedItem.Text + "";
            ds.Clear();
            ds = d2.select_method_wo_parameter(subjectquery, "Text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                rdbsingsubject.DataSource = ds;
                rdbsingsubject.DataTextField = "subject_name";
                rdbsingsubject.DataValueField = "subject_no";
                rdbsingsubject.DataBind();
                rdbsingsubject.Visible = true;
            }
            else
            {
                rdbsingsubject.Visible = false;
            }
            if (ds.Tables[1].Rows.Count > 0)
            {
                cbmultiplesubject.DataSource = ds.Tables[1];
                cbmultiplesubject.DataTextField = "subject_name";
                cbmultiplesubject.DataValueField = "subject_no";
                cbmultiplesubject.DataBind();
                cbmultiplesubject.Visible = true;
            }
            else
            {
                cbmultiplesubject.Visible = false;
            }
        }
        catch
        {

        }
    }

    protected void rdbsingsubject_Change(object sender, EventArgs e)
    {
        try
        {
            if (cbmultiplesubject.Items.Count > 0)
            {
                cbmultiplesubject.ClearSelection();
                cbdepanttamil.Checked = false;
            }
        }
        catch
        {

        }
    }

    protected void btnsave_ok(object sender, EventArgs e)
    {
        try
        {
            bool flage = false;
            string checkvalue = "";
            string deletquery = " delete from FounSubTypeSetting where SourceSubTypeNo='" + rdblist.SelectedItem.Value + "' and DestSubTypeNo ='" + rdblist2.SelectedItem.Value + "' and SourseSubNo ='" + rdbsingsubject.SelectedItem.Value + "' and College_Code='" + ddlcollege.SelectedItem.Value + "' and Degree_Code ='" + ddldepartment.SelectedItem.Value + "' and Semester ='" + ddlsem.SelectedItem.Text + "' and Batch_year ='" + ddlbatch.SelectedItem.Text + "' ";
            int del = d2.update_method_wo_parameter(deletquery, "Text");
            if (cbdepanttamil.Checked == true)
            {
                checkvalue = "1";
            }
            else
            {
                checkvalue = "0";
            }
            if (cbmultiplesubject.Items.Count > 0)
            {
                for (int r = 0; r < cbmultiplesubject.Items.Count; r++)
                {
                    if (cbmultiplesubject.Items[r].Selected == true)
                    {
                        string insertquery = "insert into FounSubTypeSetting (SourceSubTypeNo,DestSubTypeNo ,SourseSubNo,DestSubNo,Degree_Code ,Semester,Batch_year,College_Code,is_tamil) values ('" + rdblist.SelectedItem.Value + "','" + rdblist2.SelectedItem.Value + "','" + rdbsingsubject.SelectedItem.Value + "','" + cbmultiplesubject.Items[r].Value + "','" + ddldepartment.SelectedItem.Value + "','" + ddlsem.SelectedItem.Text + "','" + ddlbatch.SelectedItem.Text + "','" + ddlcollege.SelectedItem.Value + "','" + checkvalue + "')";
                        int ins = d2.update_method_wo_parameter(insertquery, "Text");
                        flage = true;
                    }
                }

            }
            if (flage == true)
            {
                ScriptManager.RegisterStartupScript(base.Page, this.GetType(), ("dialogJavascript" + this.ID), "alert(\"Saved Successfully\");", true);
            }
        }
        catch
        {

        }
    }

}

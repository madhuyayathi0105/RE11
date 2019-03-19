using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;
using Gios.Pdf;
public partial class LateAttendanceReport : System.Web.UI.Page
{
   
    DataSet ds = new DataSet();
    static ArrayList ItemList = new ArrayList();
    DAccess2 d2 = new DAccess2();
    Hashtable hat = new Hashtable();
    static ArrayList Itemindex = new ArrayList();
    static ArrayList ItemList1 = new ArrayList();
    static ArrayList Itemindex1 = new ArrayList();
    static ArrayList ItemListstu = new ArrayList();
    static ArrayList Itemindexstu = new ArrayList();
    static ArrayList ItemListguest = new ArrayList();
    static ArrayList Itemindexguest = new ArrayList();
    string usercode = string.Empty;
    string collegecode1 = string.Empty;
    string collegecode = string.Empty;
    string singleuser = string.Empty;
    string group_user = string.Empty;
    ReuasableMethods rs = new ReuasableMethods();
    Boolean Cellclick = false;
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
           
            BindCollege();
            bindbatch();
            edu_level();
            degree();
            //bindsem();
            BindSectionDetail();
          

           

           
     
           

            //CalendarExtender10.EndDate = DateTime.Now;
            //CalendarExtender1.EndDate = DateTime.Now;
            txt_fromdate.Text = DateTime.Now.ToString("dd/MM/yyyy");
            txt_fromdate.Attributes.Add("readonly", "readonly");
            txt_todate.Text = DateTime.Now.ToString("dd/MM/yyyy");
            txt_todate.Attributes.Add("readonly", "readonly");
           
          
           
           
        }
        
    }
    void BindCollege()
    {
        try
        {
            //string srisql = "select collname,college_code from collinfo";
            ds.Clear();
            //ds = d2.select_method_wo_parameter(srisql, "Text");
            byte userType = 0;
            string userOrGroupCode = string.Empty;
            if (Session["group_code"] != null && (Convert.ToString(Session["group_code"]).Trim() != "") && (Convert.ToString(Session["group_code"]).Trim() != "0") && (Convert.ToString(Session["group_code"]).Trim() != "-1"))
            {
                userOrGroupCode = Convert.ToString(Session["group_code"]).Trim();
                userType = 0;
            }
            else if (Session["usercode"] != null)
            {
                userOrGroupCode = Convert.ToString(Session["usercode"]).Trim();
                userType = 1;
            }
            ds.Clear();
            ds = d2.BindCollegebaseonrights(userOrGroupCode, userType);
            ddlcollege.DataSource = ds;
            ddlcollege.DataTextField = "collname";
            ddlcollege.DataValueField = "college_code";
            ddlcollege.DataBind();
        }
        catch
        {
        }
    }
   
    public void ddlcollege_SelectedIndexChanged(object sender, EventArgs e)
    {
        edu_level();
        degree();
        bindbatch();
        //bindsem();
        BindSectionDetail();
        //loadstream();
       
      
    }
    public void bindbatch()
    {
        try
        {
            cbl_batch.Items.Clear();
            hat.Clear();
            ds = d2.BindBatch();
            if (ds.Tables[0].Rows.Count > 0)
            {
                cbl_batch.DataSource = ds;
                cbl_batch.DataTextField = "batch_year";
                cbl_batch.DataValueField = "batch_year";
                cbl_batch.DataBind();
                //ddl_batch1.SelectedIndex = 3;
                if (cbl_batch.Items.Count > 0)
                {
                    for (int i = 0; i < cbl_batch.Items.Count; i++)
                    {
                        cbl_batch.Items[0].Selected = true;
                    }
                    // txt_batch.Text = "Batch(" + cbl_batch.Items.Count + ")";
                    txt_batch.Text = "Batch(" + 1 + ")";
                    //cb_batch.Checked = true;
                }
                else
                {
                    txt_batch.Text = "--Select--";
                    cb_batch.Checked = false;
                }
            }
        }
        catch
        {
        }
    }
    public void degree()
    {
        try
        {
            string edulvl = string.Empty;
            for (int i = 0; i < cbl_graduation.Items.Count; i++)
            {
                if (cbl_graduation.Items[i].Selected == true)
                {
                    string build = cbl_graduation.Items[i].Value.ToString();
                    if (edulvl == "")
                    {
                        edulvl = build;
                    }
                    else
                    {
                        edulvl = edulvl + "','" + build;
                    }
                }
            }
            string query = string.Empty;
            
            string rights = string.Empty;
            if (group_user.Contains(";"))
            {
                string[] group_semi = group_user.Split(';');
                group_user = group_semi[0].ToString();
            }
            if ((group_user.ToString().Trim() != "") && (group_user.Trim() != "0") && (group_user.ToString().Trim() != "-1"))
            {
                rights = "and group_code='" + group_user + "'";
            }
            else
            {
                rights = " and user_code='" + usercode + "'";
            }
           
                query = "select distinct d.Course_Id,c.Course_Name from Degree d,course c ,DeptPrivilages p where p.degree_code=d.Degree_Code and d.Course_Id=c.Course_Id and d.college_code=c.college_code and d.college_code='" + ddlcollege.SelectedItem.Value + "' and Edu_Level in('" + edulvl + "') " + rights + "";
            
            ds.Clear();
            ds = d2.select_method_wo_parameter(query, "Text");
            int count1 = ds.Tables[0].Rows.Count;
            if (count1 > 0)
            {
                cbl_degree.DataSource = ds;
                cbl_degree.DataTextField = "course_name";
                cbl_degree.DataValueField = "course_id";
                cbl_degree.DataBind();
                if (cbl_degree.Items.Count > 0)
                {
                    //    for (int i = 0; i < cbl_degree.Items.Count; i++)
                    //    {
                    cbl_degree.Items[0].Selected = true;
                }
                txt_degree.Text = lbl_degree.Text + "(" + 1 + ")";
                // cb_degree.Checked = true;
                //}
                //else
                //{
                //    txt_degree.Text = "--Select--";
                //    cb_degree.Checked = false;
                //}
                string deg = string.Empty;
                if (cbl_degree.Items.Count > 0)
                {
                    for (int i = 0; i < cbl_degree.Items.Count; i++)
                    {
                        if (cbl_degree.Items[i].Selected == true)
                        {
                            string build = cbl_degree.Items[i].Value.ToString();
                            if (deg == "")
                            {
                                deg = build;
                            }
                            else
                            {
                                deg = deg + "','" + build;
                            }
                        }
                    }
                }
                bindbranch(deg);
            }
            else
            {
                txt_degree.Text = "--Select--";
                cb_degree.Checked = false;
                cbl_degree.Items.Clear();
                txt_branch.Text = "--Select--";
                cb_branch.Checked = false;
                cbl_branch.Items.Clear();
                //cb_sem.Checked = false;
                //txt_sem.Text = "--Select--";
                //cbl_sem.Items.Clear();
                cb_sec.Checked = false;
                txt_sec.Text = "--Select--";
                cbl_sec.Items.Clear();
            }
        }
        catch (Exception ex)
        {
        }
    }
    public void bindbranch(string branch)
    {
        try
        {
            branch = string.Empty;
            if (cbl_degree.Items.Count > 0)
            {
                for (int i = 0; i < cbl_degree.Items.Count; i++)
                {
                    if (cbl_degree.Items[i].Selected == true)
                    {
                        string build = cbl_degree.Items[i].Value.ToString();
                        if (branch == "")
                        {
                            branch = build;
                        }
                        else
                        {
                            branch = branch + "','" + build;
                        }
                    }
                }
            }
            string rights = string.Empty;
            if (group_user.Contains(";"))
            {
                string[] group_semi = group_user.Split(';');
                group_user = group_semi[0].ToString();
            }
            if ((group_user.ToString().Trim() != "") && (group_user.Trim() != "0") && (group_user.ToString().Trim() != "-1"))
            {
                rights = "and group_code='" + group_user + "'";
            }
            else
            {
                rights = " and user_code='" + usercode + "'";
            }
            cb_branch.Checked = false;
            string commname = string.Empty;
            if (branch != "")
            {
                commname = "select distinct degree.degree_code,department.dept_name,degree.Acronym  from degree,department,course,deptprivilages where course.course_id=degree.course_id  and department.dept_code=degree.dept_code and course.college_code = degree.college_code and department.college_code = degree.college_code and degree.course_id in('" + branch + "') and deptprivilages.Degree_code=degree.Degree_code and degree.college_code='" + ddlcollege.SelectedItem.Value + "' " + rights + "";
            }
            else
            {
                commname = " select distinct degree.degree_code,department.dept_name,degree.Acronym  from degree,department,course,deptprivilages where course.course_id=degree.course_id  and department.dept_code=degree.dept_code and course.college_code = degree.college_code and department.college_code = degree.college_code and deptprivilages.Degree_code=degree.Degree_code and degree.college_code='" + ddlcollege.SelectedItem.Value + "' " + rights + "";
            }
            ds.Clear();
            cbl_branch.Items.Clear();
            ds = d2.select_method(commname, hat, "Text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                cbl_branch.DataSource = ds;
                cbl_branch.DataTextField = "dept_name";
                cbl_branch.DataValueField = "degree_code";
                cbl_branch.DataBind();
                if (cbl_branch.Items.Count > 0)
                {
                    //    for (int i = 0; i < cbl_branch.Items.Count; i++)
                    //    {
                    cbl_branch.Items[0].Selected = true;
                }
                txt_branch.Text = lbl_branch.Text + "(" + 1 + ")";
                //}
            }
        }
        catch (Exception ex)
        {
        }
    }
    //public void bindsem()
    //{
    //    try
    //    {
    //        string branch = string.Empty;
    //        string build = string.Empty;
    //        string build1 = string.Empty;
    //        string batch = string.Empty;
    //        int j = 0;
    //        cbl_sem.Items.Clear();
    //        string studtype = string.Empty;
    //        if (cbl_branch.Items.Count > 0)
    //        {
    //            for (j = 0; j < cbl_branch.Items.Count; j++)
    //            {
    //                if (cbl_branch.Items[j].Selected == true)
    //                {
    //                    build = cbl_branch.Items[j].Value.ToString();
    //                    if (branch == "")
    //                    {
    //                        branch = build;
    //                    }
    //                    else
    //                    {
    //                        branch = branch + "," + build;
    //                    }
    //                }
    //            }
    //        }
    //        if (branch.Trim() != "")
    //        {
    //            string deptquery = "select distinct Current_Semester from Registration where degree_code in (" + branch + ")  order by Current_Semester";
    //            ds.Clear();
    //            ds = d2.select_method_wo_parameter(deptquery, "Text");
    //            if (ds.Tables[0].Rows.Count > 0)
    //            {
    //                cbl_sem.DataSource = ds;
    //                cbl_sem.DataTextField = "Current_Semester";
    //                cbl_sem.DataBind();
    //                if (cbl_sem.Items.Count > 0)
    //                {
    //                    for (int i = 0; i < cbl_sem.Items.Count; i++)
    //                    {
    //                        cbl_sem.Items[i].Selected = true;
    //                        studtype = Convert.ToString(cbl_sem.Items[i].Text);
    //                    }
    //                    if (cbl_sem.Items.Count == 1)
    //                    {
    //                        txt_sem.Text = "Semester(" + studtype + ")";
    //                    }
                       
    //                    cb_sem.Checked = true;
    //                }
    //            }
    //            else
    //            {
    //                txt_sem.Text = "--Select--";
    //            }
    //        }
    //    }
    //    catch
    //    {
    //    }
    //}
   
    public void BindSectionDetail()
    {
        try
        {
            cbl_sec.Items.Clear();
            string batch = string.Empty;
            string branch = string.Empty;
            int i = 0;
            if (cbl_branch.Items.Count > 0)
            {
                for (i = 0; i < cbl_branch.Items.Count; i++)
                {
                    if (cbl_branch.Items[i].Selected == true)
                    {
                        string build = cbl_branch.Items[i].Value.ToString();
                        if (branch == "")
                        {
                            branch = build;
                        }
                        else
                        {
                            branch = branch + "','" + build;
                        }
                    }
                }
            }
            if (cbl_batch.Items.Count > 0)
            {
                for (i = 0; i < cbl_batch.Items.Count; i++)
                {
                    if (cbl_batch.Items[i].Selected == true)
                    {
                        string build = cbl_batch.Items[i].Value.ToString();
                        if (batch == "")
                        {
                            batch = build;
                        }
                        else
                        {
                            batch = batch + "','" + build;
                        }
                    }
                }
            }
            if (batch.Trim() != "" && branch.Trim() != "")
            {
                string sqlquery = "select distinct sections from registration where batch_year in('" + batch + "') and degree_code in('" + branch + "') and sections<>'-1' and sections<>' ' and delflag=0 and exam_flag<>'Debar' order by sections";
                DataSet ds = new DataSet();
                ds = d2.select_method_wo_parameter(sqlquery, "text");
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
                    else
                    {
                        txt_sec.Text = "--Select--";
                    }
                }
                else
                {
                    txt_sec.Text = "--Select--";
                }
            }
            cbl_sec.Items.Add("Empty");
        }
        catch
        {
        }
    }
  
   
    public void edu_level()
    {
        string st = string.Empty;
       
            st = "select distinct edu_level,priority from course where college_code='" + ddlcollege.SelectedItem.Value + "' order by priority";
        
        ds = d2.select_method_wo_parameter(st, "text");
        if (ds.Tables[0].Rows.Count > 0)
        {
            cbl_graduation.DataSource = ds;
            cbl_graduation.DataTextField = "edu_level";
            cbl_graduation.DataValueField = "edu_level";
            cbl_graduation.DataBind();
            if (cbl_graduation.Items.Count > 0)
            {
                //    for (int i = 0; i < cbl_graduation.Items.Count; i++)
                //    {
                cbl_graduation.Items[0].Selected = true;
            }
            txt_graduation.Text = "Graduation(" + 1 + ")";
            //    //cb_graduation.Checked = true;
            //}
            //else
            //{
            //    txt_graduation.Text = "--Select--";
            //    cb_graduation.Checked = false;
            //    txt_degree.Text = "--Select--";
            //    cb_degree.Checked = false;
            //    cbl_degree.Items.Clear();
            //    txt_branch.Text = "--Select--";
            //    cb_branch.Checked = false;
            //    cbl_branch.Items.Clear();
            //    cb_sem.Checked = false;
            //    txt_sem.Text = "--Select--";
            //    cbl_sem.Items.Clear();
            //    cb_sec.Checked = false;
            //    txt_sec.Text = "--Select--";
            //    cbl_sec.Items.Clear();
            //}
        }
    }
   
   
   
    public void cb_graduation_checkedchange(object sender, EventArgs e)
    {
        try
        {
            string deg = string.Empty;
            if (cb_graduation.Checked == true)
            {
                for (int i = 0; i < cbl_graduation.Items.Count; i++)
                {
                    cbl_graduation.Items[i].Selected = true;
                }
                txt_graduation.Text = "Edu Level(" + (cbl_graduation.Items.Count) + ")";
                if (cbl_degree.Items.Count > 0)
                {
                    for (int i = 0; i < cbl_degree.Items.Count; i++)
                    {
                        if (cbl_degree.Items[i].Selected == true)
                        {
                            string build = cbl_degree.Items[i].Value.ToString();
                            if (deg == "")
                            {
                                deg = build;
                            }
                            else
                            {
                                deg = deg + "','" + build;
                            }
                        }
                    }
                }
                degree();
                bindbranch(deg);
                //bindsem();
                BindSectionDetail();
            }
            else
            {
                for (int i = 0; i < cbl_graduation.Items.Count; i++)
                {
                    cbl_graduation.Items[i].Selected = false;
                }
                txt_graduation.Text = "--Select--";
                txt_degree.Text = "--Select--";
                cb_degree.Checked = false;
                cbl_degree.Items.Clear();
                txt_branch.Text = "--Select--";
                cb_branch.Checked = false;
                cbl_branch.Items.Clear();
                //cb_sem.Checked = false;
                //txt_sem.Text = "--Select--";
                //cbl_sem.Items.Clear();
                cb_sec.Checked = false;
                txt_sec.Text = "--Select--";
                cbl_sec.Items.Clear();
            }
        }
        catch
        {
        }
    }
    public void cbl_graduation_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            int commcount = 0;
            string deg = string.Empty;
            txt_graduation.Text = "--Select--";
            cb_graduation.Checked = false;
            for (int i = 0; i < cbl_graduation.Items.Count; i++)
            {
                if (cbl_graduation.Items[i].Selected == true)
                {
                    commcount = commcount + 1;
                }
            }
            if (commcount == cbl_graduation.Items.Count)
            {
                txt_graduation.Text = "Edu Level(" + commcount.ToString() + ")";
                cb_graduation.Checked = true;
            }
            else if (commcount == 0)
            {
                txt_graduation.Text = "--Select--";
                txt_graduation.Text = "--Select--";
                txt_degree.Text = "--Select--";
                cb_degree.Checked = false;
                cbl_degree.Items.Clear();
                txt_branch.Text = "--Select--";
                cb_branch.Checked = false;
                cbl_branch.Items.Clear();
                //cb_sem.Checked = false;
                //txt_sem.Text = "--Select--";
                //cbl_sem.Items.Clear();
                cb_sec.Checked = false;
                txt_sec.Text = "--Select--";
                cbl_sec.Items.Clear();
            }
            else
            {
                txt_graduation.Text = "Edu Level(" + commcount.ToString() + ")";
                if (cbl_degree.Items.Count > 0)
                {
                    for (int i = 0; i < cbl_degree.Items.Count; i++)
                    {
                        if (cbl_degree.Items[i].Selected == true)
                        {
                            string build = cbl_degree.Items[i].Value.ToString();
                            if (deg == "")
                            {
                                deg = build;
                            }
                            else
                            {
                                deg = deg + "','" + build;
                            }
                        }
                    }
                }
                //degree();
                //bindbranch(deg);
                //bindsem();
                //BindSectionDetail();
            }
            degree();
            bindbranch(deg);
           // bindsem();
            BindSectionDetail();
        }
        catch
        {
        }
    }
    public void cb_batch_checkedchange(object sender, EventArgs e)
    {
        try
        {
            string buildvalue1 = string.Empty;
            string build1 = string.Empty;
            if (cb_batch.Checked == true)
            {
                for (int i = 0; i < cbl_batch.Items.Count; i++)
                {
                    if (cb_batch.Checked == true)
                    {
                        cbl_batch.Items[i].Selected = true;
                        txt_batch.Text = "Batch(" + (cbl_batch.Items.Count) + ")";
                        build1 = cbl_batch.Items[i].Value.ToString();
                        if (buildvalue1 == "")
                        {
                            buildvalue1 = build1;
                        }
                        else
                        {
                            buildvalue1 = buildvalue1 + "'" + "," + "'" + build1;
                        }
                    }
                }
            }
            else
            {
                for (int i = 0; i < cbl_batch.Items.Count; i++)
                {
                    cbl_batch.Items[i].Selected = false;
                    txt_batch.Text = "--Select--";
                }
            }
           // bindsem();
            BindSectionDetail();
        }
        catch (Exception ex)
        {
        }
    }
    public void cbl_batch_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            int seatcount = 0;
            cb_batch.Checked = false;
            string buildvalue = string.Empty;
            string build = string.Empty;
            for (int i = 0; i < cbl_batch.Items.Count; i++)
            {
                if (cbl_batch.Items[i].Selected == true)
                {
                    seatcount = seatcount + 1;
                    txt_batch.Text = "--Select--";
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
            if (seatcount == cbl_batch.Items.Count)
            {
                txt_batch.Text = "Batch(" + seatcount.ToString() + ")";
                cb_batch.Checked = true;
            }
            else if (seatcount == 0)
            {
                txt_batch.Text = "--Select--";
                cb_batch.Text = "--Select--";
            }
            else
            {
                txt_batch.Text = "Batch(" + seatcount.ToString() + ")";
            }
           // bindsem();
            BindSectionDetail();
        }
        catch (Exception ex)
        {
        }
    }
    public void cb_degree_checkedchange(object sender, EventArgs e)
    {
        try
        {
            string buildvalue1 = string.Empty;
            string build1 = string.Empty;
            if (cb_degree.Checked == true)
            {
                for (int i = 0; i < cbl_degree.Items.Count; i++)
                {
                    if (cb_degree.Checked == true)
                    {
                        cbl_degree.Items[i].Selected = true;
                        txt_degree.Text = lbl_degree.Text + "(" + (cbl_degree.Items.Count) + ")";
                        build1 = cbl_degree.Items[i].Value.ToString();
                        if (buildvalue1 == "")
                        {
                            buildvalue1 = build1;
                        }
                        else
                        {
                            buildvalue1 = buildvalue1 + "'" + "," + "'" + build1;
                        }
                    }
                }
                bindbranch(buildvalue1);
            }
            else
            {
                for (int i = 0; i < cbl_degree.Items.Count; i++)
                {
                    cbl_degree.Items[i].Selected = false;
                    txt_degree.Text = "--Select--";
                    txt_branch.Text = "--Select--";
                    cbl_branch.ClearSelection();
                    cb_branch.Checked = false;
                }
            }
           // bindsem();
            BindSectionDetail();

        }
        catch (Exception ex)
        {
        }
    }
    public void cbl_degree_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            int seatcount = 0;
            cb_degree.Checked = false;
            string buildvalue = string.Empty;
            string build = string.Empty;
            for (int i = 0; i < cbl_degree.Items.Count; i++)
            {
                if (cbl_degree.Items[i].Selected == true)
                {
                    seatcount = seatcount + 1;
                    txt_branch.Text = "--Select--";
                    build = cbl_degree.Items[i].Value.ToString();
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
            bindbranch(buildvalue);
            if (seatcount == cbl_degree.Items.Count)
            {
                txt_degree.Text = lbl_degree.Text + "(" + seatcount.ToString() + ")";
                cb_degree.Checked = true;
            }
            else if (seatcount == 0)
            {
                txt_degree.Text = "--Select--";
            }
            else
            {
                txt_degree.Text = lbl_degree.Text + "(" + seatcount.ToString() + ")";
            }
            //bindsem();
            BindSectionDetail();
        }
        catch (Exception ex)
        {
        }
    }
    public void cb_branch_checkedchange(object sender, EventArgs e)
    {
        try
        {
            if (cb_branch.Checked == true)
            {
                for (int i = 0; i < cbl_branch.Items.Count; i++)
                {
                    cbl_branch.Items[i].Selected = true;
                }
                txt_branch.Text = lbl_branch.Text + "(" + (cbl_branch.Items.Count) + ")";
            }
            else
            {
                for (int i = 0; i < cbl_branch.Items.Count; i++)
                {
                    cbl_branch.Items[i].Selected = false;
                }
                txt_branch.Text = "--Select--";
            }
            //bindsem();
            BindSectionDetail();
        }
        catch
        {
        }
    }
    public void cbl_branch_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            int commcount = 0;
            txt_branch.Text = "--Select--";
            cb_branch.Checked = false;
            for (int i = 0; i < cbl_branch.Items.Count; i++)
            {
                if (cbl_branch.Items[i].Selected == true)
                {
                    commcount = commcount + 1;
                }
            }
            if (commcount == cbl_branch.Items.Count)
            {
                txt_branch.Text = lbl_branch.Text + "(" + commcount.ToString() + ")";
                cb_branch.Checked = true;
            }
            else if (commcount == 0)
            {
                txt_degree.Text = "--Select--";
            }
            else
            {
                txt_branch.Text = lbl_branch.Text + "(" + commcount.ToString() + ")";
            }
            //bindsem();
            BindSectionDetail();
        }
        catch
        {
        }
    }
    //public void cb_sem_checkedchange(object sender, EventArgs e)
    //{
    //    try
    //    {
    //        int cout = 0;
    //        txt_sem.Text = "--Select--";
    //        if (cb_sem.Checked == true)
    //        {
    //            cout++;
    //            for (int i = 0; i < cbl_sem.Items.Count; i++)
    //            {
    //                cbl_sem.Items[i].Selected = true;
    //            }
    //            txt_sem.Text = lbl_org_sem.Text + "(" + (cbl_sem.Items.Count) + ")";
    //        }
    //        else
    //        {
    //            for (int i = 0; i < cbl_sem.Items.Count; i++)
    //            {
    //                cbl_sem.Items[i].Selected = false;
    //            }
    //        }
    //    }
    //    catch (Exception ex)
    //    {
    //    }
    //}
    //public void cbl_sem_SelectedIndexChanged(object sender, EventArgs e)
    //{
    //    try
    //    {
    //        cb_sem.Checked = false;
    //        int commcount = 0;
    //        txt_sem.Text = "--Select--";
    //        for (int i = 0; i < cbl_sem.Items.Count; i++)
    //        {
    //            if (cbl_sem.Items[i].Selected == true)
    //            {
    //                commcount = commcount + 1;
    //                cb_sem.Checked = false;
    //            }
    //        }
    //        if (commcount > 0)
    //        {
    //            if (commcount == cbl_sem.Items.Count)
    //            {
    //                cb_sem.Checked = true;
    //            }
    //            txt_sem.Text = lbl_org_sem.Text + "(" + commcount.ToString() + ")";
    //        }
    //    }
    //    catch (Exception ex)
    //    {
    //    }
    //}
    public void cb_sec_checkedchange(object sender, EventArgs e)
    {
        try
        {
            if (cb_sec.Checked == true)
            {
                for (int i = 0; i < cbl_sec.Items.Count; i++)
                {
                    cbl_sec.Items[i].Selected = true;
                }
                txt_sec.Text = "Section(" + (cbl_sec.Items.Count) + ")";
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
                }
            }
            if (commcount == cbl_sec.Items.Count)
            {
                txt_sec.Text = "Section(" + commcount.ToString() + ")";
                cb_sec.Checked = true;
            }
            else if (commcount == 0)
            {
                txt_sec.Text = "--Select--";
            }
            else
            {
                txt_sec.Text = "Section(" + commcount.ToString() + ")";
            }
        }
        catch
        {
        }
    }
    public void btndetailgo_Click(object sender, EventArgs e)
    {
        try
        {
            if (rdbmess.Checked == false)
            {
                string addbatch = string.Empty;
                string adddeg = string.Empty;
                string addsem = string.Empty;
                string adddsec = string.Empty;
                string bran = string.Empty;
                string Edu_Level = string.Empty;
                string course = string.Empty;
                Fpspread1.Visible = true;
                Fpspread1.Sheets[0].AutoPostBack = true;
                Fpspread1.Sheets[0].RowHeader.Visible = false;
                FarPoint.Web.Spread.StyleInfo style2 = new FarPoint.Web.Spread.StyleInfo();
                style2.Font.Size = 14;
                style2.Font.Name = "Book Antiqua";
                style2.Font.Bold = true;
                style2.HorizontalAlign = HorizontalAlign.Center;
                style2.ForeColor = Color.Black;
                style2.BackColor = Color.AliceBlue;
                Fpspread2.Visible = false;
                pheaderfilter.Visible = false;
                pcolumnorder.Visible = false;
                FarPoint.Web.Spread.StyleInfo darkstyle = new FarPoint.Web.Spread.StyleInfo();
                darkstyle.BackColor = ColorTranslator.FromHtml("#0CA6CA");
                darkstyle.ForeColor = System.Drawing.Color.White;
                Fpspread1.ActiveSheetView.ColumnHeader.DefaultStyle = darkstyle;

                Fpspread1.CommandBar.Visible = false;
                Fpspread1.Sheets[0].RowCount = 0;
                Fpspread1.Sheets[0].ColumnCount = 4;
                Fpspread1.Sheets[0].ColumnHeader.RowCount = 2;
                Fpspread1.ActiveSheetView.ColumnHeader.DefaultStyle.Font.Size = FontUnit.Medium;
                Fpspread1.ActiveSheetView.ColumnHeader.DefaultStyle.Font.Bold = true;
                Fpspread1.ActiveSheetView.ColumnHeader.DefaultStyle.Font.Name = "Book Antiqua";
                Fpspread1.Sheets[0].ColumnHeader.Cells[0, 0].Text = "S.No";
                Fpspread1.Sheets[0].ColumnHeader.Cells[0, 0].Font.Bold = true;
                Fpspread1.Sheets[0].ColumnHeader.Cells[0, 0].Font.Name = "Book Antiqua";
                Fpspread1.Sheets[0].ColumnHeader.Cells[0, 0].Font.Size = FontUnit.Medium;
                Fpspread1.Sheets[0].ColumnHeader.Cells[0, 0].HorizontalAlign = HorizontalAlign.Center;

                Fpspread1.Sheets[0].ColumnHeader.Cells[0, 1].Text = "Batch Year";
                Fpspread1.Sheets[0].ColumnHeader.Cells[0, 1].Font.Bold = true;
                Fpspread1.Sheets[0].ColumnHeader.Cells[0, 1].Font.Name = "Book Antiqua";
                Fpspread1.Sheets[0].ColumnHeader.Cells[0, 1].Font.Size = FontUnit.Medium;
                Fpspread1.Sheets[0].ColumnHeader.Cells[0, 1].HorizontalAlign = HorizontalAlign.Center;
                Fpspread1.Sheets[0].ColumnHeader.Cells[0, 2].Text = "Degree";
                Fpspread1.Sheets[0].ColumnHeader.Cells[0, 2].Font.Bold = true;
                Fpspread1.Sheets[0].ColumnHeader.Cells[0, 2].Font.Name = "Book Antiqua";
                Fpspread1.Sheets[0].ColumnHeader.Cells[0, 2].Font.Size = FontUnit.Medium;
                Fpspread1.Sheets[0].ColumnHeader.Cells[0, 2].HorizontalAlign = HorizontalAlign.Center;
                Fpspread1.Sheets[0].ColumnHeader.Cells[0, 3].Text = "Branch";
                Fpspread1.Sheets[0].ColumnHeader.Cells[0, 3].Font.Bold = true;
                Fpspread1.Sheets[0].ColumnHeader.Cells[0, 3].Font.Name = "Book Antiqua";
                Fpspread1.Sheets[0].ColumnHeader.Cells[0, 3].Font.Size = FontUnit.Medium;
                Fpspread1.Sheets[0].ColumnHeader.Cells[0, 3].HorizontalAlign = HorizontalAlign.Center;
                //Fpspread1.Sheets[0].ColumnHeader.Cells[0, 4].Text = "Count";
                //Fpspread1.Sheets[0].ColumnHeader.Cells[0, 4].Font.Bold = true;
                //Fpspread1.Sheets[0].ColumnHeader.Cells[0, 4].Font.Name = "Book Antiqua";
                //Fpspread1.Sheets[0].ColumnHeader.Cells[0, 4].Font.Size = FontUnit.Medium;
                //Fpspread1.Sheets[0].ColumnHeader.Cells[0, 4].HorizontalAlign = HorizontalAlign.Center;
                int colcount =3;
                addbatch = rs.GetSelectedItemsValueAsString(cbl_batch);
                adddeg = rs.GetSelectedItemsValueAsString(cbl_branch);
                //addsem = rs.GetSelectedItemsValueAsString(cbl_sem);
               // adddsec = rs.GetSelectedItemsValueAsString(cbl_sec);
                bran = rs.GetSelectedItemsValueAsString(cbl_branch);
                Edu_Level=rs.GetSelectedItemsValueAsString(cbl_graduation);
                course = rs.GetSelectedItemsValueAsString(cbl_degree);


                string typ = string.Empty;
                if (cbl_sec.Items.Count > 0)
                {
                    for (int i = 0; i < cbl_sec.Items.Count; i++)
                    {
                        //if (cbl_sec.Items[i].Text == "Empty")
                        //{
                        //    cbl_sec.Items[i].Text = "''";
                        //}
                        if (cbl_sec.Items[i].Selected == true)
                        {
                            if (typ == "")
                            {
                                typ = "" + cbl_sec.Items[i].Text + "";
                            }
                            else if (cbl_sec.Items[i].Text == "Empty")
                            {
                                typ = typ + "'" + "," + "'";
                            }
                            else
                            {
                                typ = typ + "'" + "," + "'" + cbl_sec.Items[i].Text + "";

                            }
                        }


                    }
                }
                adddsec = typ;
                string fromdate = string.Empty;
                string todate = string.Empty;
                string date = string.Empty;
                string date1 = string.Empty;
                if (txt_fromdate.Text != "" && txt_todate.Text != "")
                {
                    fromdate = txt_fromdate.Text;
                    todate = txt_todate.Text;
                    string[] splitti = fromdate.Split('/');
                    if (splitti.Length == 3)
                    {
                        string dats = splitti[2];
                        string hrs = splitti[0];
                        string mins = splitti[1];
                        date = dats + '-' + mins + '-' + hrs;
                    }
                    string[] split2 = todate.Split('/');
                    if (split2.Length == 3)
                    {
                        string hr2 = split2[0];
                        string min2 = split2[1];
                        string day2 = split2[2];
                        date1 = day2 + '-' + min2 + '-' + hr2;
                    }


                    string fdmonth = "";
                    string sdmonth = "";
                    string fdyear = "";
                    string sdyear = "";
                    string month = "";
                    string year = "";
                    string monthyear = "";
                    string fdday = "";
                    string sdday = "";

                    int monthvalue = 0;

                    string[] splitt = fromdate.Split('/');
                    string[] splitt1 = todate.Split('/');
                    fdmonth = Convert.ToString(splitt[1]);
                    sdmonth = Convert.ToString(splitt1[1]);
                    fdyear = Convert.ToString(splitt[2]);
                    sdyear = Convert.ToString(splitt1[2]);
                    fdday = Convert.ToString(splitt[0]);
                    sdday = Convert.ToString(splitt1[0]);

                    int dd = 0, d = 0;
                    int fdmonth1 = Convert.ToInt32(fdmonth);
                    int sdmonth1 = Convert.ToInt32(sdmonth);
                    int fdday1 = Convert.ToInt32(fdday);
                    int fdday2 = Convert.ToInt32(fdday);
                    int sdday1 = Convert.ToInt32(sdday);
                    int col = 4;
                    //month = dsroom.Tables[0].Rows[0]["AttnMonth"].ToString();
                    //year = dsroom.Tables[0].Rows[0]["AttnYear"].ToString();
                    do
                    {

                        month = Convert.ToString(fdmonth1);
                        year = Convert.ToString(fdyear);
                        switch (month)
                        {
                            //case "1": monthyear = "January " + year;

                            //    break;
                            //case "2": monthyear = "February " + year;
                            //    break;
                            //case "3": monthyear = "March " + year;
                            //    break;
                            //case "4": monthyear = "April " + year;
                            //    break;
                            //case "5": monthyear = "May " + year;
                            //    break;
                            //case "6": monthyear = "June " + year;
                            //    break;
                            //case "7": monthyear = "July " + year;
                            //    break;
                            //case "8": monthyear = "August " + year;
                            //    break;
                            //case "9": monthyear = "September " + year;
                            //    break;
                            //case "10": monthyear = "October " + year;
                            //    break;
                            //case "11": monthyear = "November " + year;
                            //    break;
                            //case "12": monthyear = "December " + year;
                            //    break;


                            case "1": monthyear = "January " + year;
                                monthvalue = 31;
                                break;
                            case "2": monthyear = "February " + year;
                                monthvalue = 28;
                                break;
                            case "3": monthyear = "March " + year;
                                monthvalue = 31;
                                break;
                            case "4": monthyear = "April " + year;
                                monthvalue = 30;
                                break;
                            case "5": monthyear = "May " + year;
                                monthvalue = 31;
                                break;
                            case "6": monthyear = "June " + year;
                                monthvalue = 30;
                                break;
                            case "7": monthyear = "July " + year;
                                monthvalue = 31;
                                break;
                            case "8": monthyear = "August " + year;
                                monthvalue = 31;
                                break;
                            case "9": monthyear = "September " + year;
                                monthvalue = 30;
                                break;
                            case "10": monthyear = "October " + year;
                                monthvalue = 31;
                                break;
                            case "11": monthyear = "November " + year;
                                monthvalue = 30;
                                break;
                            case "12": monthyear = "December " + year;
                                monthvalue = 31;
                                break;
                        }





                        if (fdmonth == sdmonth)
                        {
                            d = sdday1 - fdday1; ;
                            d++;
                            dd += d;
                            Fpspread1.Sheets[0].ColumnCount += d;
                        }
                        else if (fdday1 != 1)
                        {
                            d = monthvalue - fdday1; ;
                            d++;
                            dd += d;
                            Fpspread1.Sheets[0].ColumnCount += d;

                        }
                        else
                        {
                            d = monthvalue;
                            dd += monthvalue;
                            Fpspread1.Sheets[0].ColumnCount += monthvalue;

                        }
                        Fpspread1.Sheets[0].ColumnHeaderSpanModel.Add(0, 0, 2, 1);
                        Fpspread1.Sheets[0].ColumnHeaderSpanModel.Add(0, 1, 2, 1);
                        Fpspread1.Sheets[0].ColumnHeaderSpanModel.Add(0, 2, 2, 1);
                        Fpspread1.Sheets[0].ColumnHeaderSpanModel.Add(0, 3, 2, 1);

                        //  Fpspread1.Sheets[0].ColumnHeaderSpanModel.Add(1, colcount, 1, Fpspread1.Sheets[0].ColumnCount);

                        int ii = fdday1;
                        colcount = 4;

                        for (int i = colcount; i < Fpspread1.Sheets[0].ColumnCount ; i++)
                        {


                            Fpspread1.Sheets[0].ColumnHeader.Cells[1, colcount].Text = ii.ToString();
                            Fpspread1.Sheets[0].ColumnHeader.Cells[1, colcount].Font.Bold = true;
                            Fpspread1.Sheets[0].ColumnHeader.Cells[1, colcount].HorizontalAlign = HorizontalAlign.Center;
                            Fpspread1.Sheets[0].ColumnHeader.Cells[1, colcount].Tag = monthyear;
                            Fpspread1.Sheets[0].ColumnHeader.Cells[1, colcount].Font.Name = "Book Antiqua";
                            Fpspread1.Columns[colcount].Width = 40;
                            colcount++;
                            ii++;
                            fdday1 = 1;


                        }
                        fdmonth1++;

                        Fpspread1.Sheets[0].ColumnHeader.Cells[0, col].Text = monthyear;

                        Fpspread1.Sheets[0].ColumnHeader.Cells[0, col].Font.Bold = true;
                        Fpspread1.Sheets[0].ColumnHeader.Cells[0, col].HorizontalAlign = HorizontalAlign.Center;
                        Fpspread1.Sheets[0].ColumnHeader.Cells[0, col].Font.Name = "Book Antiqua";
                        Fpspread1.Sheets[0].ColumnHeader.Cells[0, col].Font.Size = FontUnit.Medium;
                        Fpspread1.Sheets[0].ColumnHeaderSpanModel.Add(0, col, 1, d);

                        col += d;


                    } while (fdmonth1 <= sdmonth1);




                }


                Fpspread1.Columns[0].Width = 20;
                Fpspread1.Columns[1].Width = 30;
                Fpspread1.Columns[2].Width = 20;


                string q1 = "select distinct r.app_no,r.college_code ,roll_no,r.Stud_Name,Stud_Type,r.degree_code,Branch_code,Batch_Year,C.Course_Name,c.Course_Id,LTRIM(RTRIM(isnull(r.Sections,''))),((CONVERT(varchar(max), r.Batch_Year)+' - '+C.Course_Name+' - '+dt.dept_acronym+ case when sections='' then '' else ' - '+ (sections) end)) as batch,dt.Dept_Name,attnyear,attnmonth,FromDate from Registration r,Degree d,Department dt,course c,Late_attendance La where d.Dept_Code=dt.Dept_Code and c.Course_Id=d.Course_Id and r.degree_code=d.Degree_Code and r.college_code='" + Convert.ToString(ddlcollege.SelectedValue) + "' and r.Batch_Year in('" + addbatch + "') and c.Edu_Level in('" + Edu_Level + "') and c.Course_Id in('" + course + "') and d.degree_code in('" + adddeg + "') and  LTRIM(RTRIM(isnull( r.sections,''))) in('" + adddsec + "')  and  r.app_no=La.App_No and r.college_code=La.college_code  and fromdate between '" + date + "' and '" + date1 + "' order by r.Batch_Year,r.degree_code";
                ds = d2.select_method_wo_parameter(q1, "Text");//LTRIM(RTRIM(isnull(sections,''))) in('A','B','')
                if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {

                    int sno = 1;
                    int batspan = 0;
                    Hashtable hatbatch = new Hashtable();
                    Hashtable hatdeg = new Hashtable();
                    string subjbatch = string.Empty;
                    int degspan = 0;
                    int depspan = 0;
                    int spanrow = 0;
                    string subjbranch = string.Empty;
                    int m = 0;
                    Fpspread1.Sheets[0].RowCount++;
                    int fnsp = 0;
                    int rows = 0;
                    int batcon = 0;
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {

                        int a = i;


                        DataView dv = new DataView();
                        DataView deg = new DataView();
                        DataView dep = new DataView();
                        int batspans = 0;
                        int fnspa = Fpspread1.Sheets[0].RowCount;
                        hatdeg.Clear();
                        rows = 0;

                        string subj = Convert.ToString(ds.Tables[0].Rows[i]["Batch_Year"]);
                        if (!hatbatch.ContainsKey(subj))
                        {
                            ds.Tables[0].DefaultView.RowFilter = " Batch_Year='" + Convert.ToString(ds.Tables[0].Rows[i]["Batch_Year"]) + "'";
                            dv = ds.Tables[0].DefaultView;
                            hatbatch.Add(Convert.ToString(ds.Tables[0].Rows[i]["Batch_Year"]), Convert.ToString(ds.Tables[0].Rows[i]["Batch_Year"]));
                            DataTable dt = new DataTable();
                            dt = dv.ToTable();
                            int mg = i;
                            a = fnspa - 1;
                            if (dt.Rows.Count > 0)
                            {
                                for (int batch = 0; batch < dt.Rows.Count; batch++)
                                {


                                    degspan = 0;

                                    ds.Tables[0].DefaultView.RowFilter = "Batch_Year='" + Convert.ToString(ds.Tables[0].Rows[i]["Batch_Year"]) + "' and  Course_Id='" + Convert.ToString(dt.Rows[batch]["Course_Id"]) + "'";
                                    deg = ds.Tables[0].DefaultView;
                                    DataTable dt1 = new DataTable();
                                    dt1 = deg.ToTable();

                                    subjbatch = Convert.ToString(dt.Rows[batch]["Course_Id"]);

                                    if (!hatdeg.ContainsKey(subjbatch))
                                    {
                                        hatdeg.Add(Convert.ToString(dt.Rows[batch]["Course_Id"]), Convert.ToString(dt.Rows[batch]["Course_Name"]));

                                        mg++;
                                        //  a = mg-1;
                                        if (dt1.Rows.Count > 0)
                                        {
                                            for (int degr = 0; degr < dt1.Rows.Count; degr++)
                                            {

                                                Fpspread1.Sheets[0].Cells[a, 2].Text = Convert.ToString(dt1.Rows[degr]["Course_Name"]);
                                                Fpspread1.Sheets[0].Cells[a, 2].HorizontalAlign = HorizontalAlign.Center;
                                                Fpspread1.Sheets[0].Cells[a, 2].VerticalAlign = VerticalAlign.Middle;
                                                Fpspread1.Sheets[0].Cells[a, 2].Tag = Convert.ToString(dt1.Rows[degr]["Course_Id"]);
                                                Fpspread1.Sheets[0].Cells[a, 2].Locked = true;





                                                m++;
                                                depspan = 0;

                                                ds.Tables[0].DefaultView.RowFilter = "Batch_Year='" + Convert.ToString(ds.Tables[0].Rows[i]["Batch_Year"]) + "' and  Course_Id='" + Convert.ToString(dt.Rows[batch]["Course_Id"]) + "' and degree_code='" + Convert.ToString(dt1.Rows[degr]["degree_code"]) + "'";
                                                dep = ds.Tables[0].DefaultView;
                                                DataTable dt2 = new DataTable();
                                                dt2 = dep.ToTable();
                                                if (dt2.Rows.Count > 0)
                                                {
                                                    for (int ms = 0; ms < dt2.Rows.Count; ms++)
                                                    {
                                                        subjbranch = Convert.ToString(dt2.Rows[ms]["degree_code"]);

                                                        if (!hatdeg.ContainsKey(subjbranch))
                                                        {
                                                            hatdeg.Add(Convert.ToString(dt2.Rows[ms]["degree_code"]), Convert.ToString(dt2.Rows[ms]["Dept_Name"]));
                                                            depspan++;
                                                            if (dt2.Rows.Count > 0)
                                                            {


                                                                Fpspread1.Sheets[0].Cells[spanrow, 3].Text = Convert.ToString(dt2.Rows[0]["Dept_Name"]);
                                                                Fpspread1.Sheets[0].Cells[spanrow, 3].Tag = Convert.ToString(dt1.Rows[0]["degree_code"]);
                                                                Fpspread1.Sheets[0].Cells[spanrow, 3].HorizontalAlign = HorizontalAlign.Center;

                                                                Fpspread1.Sheets[0].Cells[spanrow, 3].VerticalAlign = VerticalAlign.Middle;
                                                                //   a++;
                                                                Fpspread1.Sheets[0].Cells[spanrow, 3].Locked = true;


                                                                // m++;


                                                                //  DateTime dtm;
                                                                string[] dm = fromdate.Split('/');
                                                                string newdate = dm[1] + '/' + dm[0] + '/' + dm[2];
                                                                //  DateTime.TryParse(fromdate, out  dtm);
                                                                //DateTime dtm = DateTime.Parse(fromdate);
                                                                DateTime dtm = Convert.ToDateTime(newdate.ToString());
                                                                //  DateTime dtm = DateTime.ParseExact(fromdate, "dd/MM/YYYY", null);
                                                                string[] dm1 = todate.Split('/');
                                                                string newdate1 = dm1[1] + '/' + dm1[0] + '/' + dm1[2];
                                                                //  DateTime.TryParse(fromdate, out  dtm);
                                                                //DateTime dtm = DateTime.Parse(fromdate);
                                                                DateTime dtto = Convert.ToDateTime(newdate1.ToString());
                                                                int fpsta = 4;
                                                                int cm = 0;
                                                                for (DateTime dt7 = dtm; dt7 <= dtto; dt7 = dt7.AddDays(1))
                                                                {

                                                                    cm++;
                                                


                                                                    string mo = dt7.Month.ToString();
                                                                    string day = dt7.Date.ToString();
                                                                    string yer = dt7.Year.ToString();
                                                                    ds.Tables[0].DefaultView.RowFilter = "Batch_Year='" + Convert.ToString(ds.Tables[0].Rows[i]["Batch_Year"]) + "' and  Course_Id='" + Convert.ToString(dt.Rows[batch]["Course_Id"]) + "' and degree_code='" + Convert.ToString(dt1.Rows[degr]["degree_code"]) + "' and FromDate='" + day + "'";
                                                                    DataView cont = ds.Tables[0].DefaultView;
                                                                    DataTable dt3 = new DataTable();
                                                                    dt3 = cont.ToTable();

                                                                    Fpspread1.Sheets[0].Cells[spanrow, fpsta].Text = Convert.ToString(dt3.Rows.Count);
                                                                    Fpspread1.Sheets[0].Cells[spanrow, fpsta].Tag = Convert.ToString(dt1.Rows[degr]["degree_code"]);
                                                                    Fpspread1.Sheets[0].Cells[spanrow, fpsta].HorizontalAlign = HorizontalAlign.Center;
                                                                    Fpspread1.Sheets[0].Cells[spanrow, fpsta].VerticalAlign = VerticalAlign.Middle;
                                                                    Fpspread1.Sheets[0].Cells[spanrow, fpsta].Locked = false;
                                                                    Fpspread1.Sheets[0].SpanModel.Add(spanrow, fpsta, depspan, 1);

                                                                    Fpspread1.Sheets[0].SpanModel.Add(spanrow, 0, depspan, 1);

                                                                    fpsta++;
                                                                }



                                                                Fpspread1.Sheets[0].RowCount++;
                                                                rows++;

                                                                Fpspread1.Sheets[0].SpanModel.Add(spanrow, 3, depspan, 1);
                                                                Fpspread1.Sheets[0].Cells[spanrow, 0].Text = Convert.ToString(sno);
                                                                Fpspread1.Sheets[0].Cells[spanrow, 0].Note = Convert.ToString(ds.Tables[0].Rows[i]["Batch_Year"]);
                                                               Fpspread1.Sheets[0].Cells[spanrow, 0].Tag =Convert.ToString(dt1.Rows[degr]["Course_id"]);
                                                                Fpspread1.Sheets[0].Cells[spanrow, 0].HorizontalAlign = HorizontalAlign.Center;
                                                                Fpspread1.Sheets[0].Cells[spanrow, 0].VerticalAlign = VerticalAlign.Middle;
                                                                sno++;
                                                            }
                                                            degspan++;
                                                            spanrow++;

                                                        }


                                                    }
                                                }

                                            }
                                            a++;
                                            Fpspread1.Sheets[0].SpanModel.Add(a - 1, 2, degspan, 1);
                                            a = degspan + a - 1;
                                        }


                                    }


                                    Fpspread1.Sheets[0].Cells[fnspa - 1, 1].Text = Convert.ToString(ds.Tables[0].Rows[i]["Batch_Year"]);
                                    Fpspread1.Sheets[0].Cells[fnspa - 1, 1].HorizontalAlign = HorizontalAlign.Center;
                                    Fpspread1.Sheets[0].Cells[fnspa - 1, 1].VerticalAlign = VerticalAlign.Middle;
                                    Fpspread1.Sheets[0].Cells[fnspa - 1, 1].Locked = true;
                                    batspans++;
                                    batspan++;
                                    Fpspread1.Sheets[0].SpanModel.Add(fnspa - 1, 1, rows, 1);

                                }
                            }
                        }


                    }

                    Fpspread1.Sheets[0].PageSize = Fpspread1.Sheets[0].RowCount;
                    Fpspread1.Width = 900;
                    Fpspread1.Height = 900;
                    Fpspread1.SaveChanges();
                    rptprint1.Visible = true;
                }
                else
                {
                    Fpspread1.Visible = false;
                    rptprint1.Visible = false;
                }
            }
            else
            {
                Fpspread1.Visible = false;
                countwise();
            }

        }
        catch
        {
        }
    }
    protected void FpSpread1_CellClick(object sender, EventArgs e)
    {
        try
        {
            Cellclick = true;
        }
        catch
        {
        }

    }
    protected void FpSpread1_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            if (Cellclick == true)
            {
              string   activerow = Fpspread1.ActiveSheetView.ActiveRow.ToString();
              string activecol = Fpspread1.ActiveSheetView.ActiveColumn.ToString();
              string monthyear = "";
              int monthvalue = 0;
               int col = 0;
               pheaderfilter.Visible = true;
               pcolumnorder.Visible = true;
               Fpspread2.Visible = true;
                        int.TryParse(activecol, out col);
                        if (activerow.Trim() != "" && activecol.Trim() != "")
                        {
                            if (col > 3)
                            {
                                string fptext = Fpspread1.Sheets[0].ColumnHeader.Cells[1, Convert.ToInt32(activecol)].Text;
                                string fptext1 = Convert.ToString(Fpspread1.Sheets[0].ColumnHeader.Cells[1, Convert.ToInt32(activecol)].Tag);
                                string adddeg = Convert.ToString(Fpspread1.Sheets[0].Cells[Convert.ToInt32(activerow), Convert.ToInt32(activecol)].Tag);
                                string course = Convert.ToString(Fpspread1.Sheets[0].Cells[Convert.ToInt32
(activerow), 0].Tag);
                                string addbatch = Convert.ToString(Fpspread1.Sheets[0].Cells[Convert.ToInt32(activerow), 0].Note);
                                string[] dm1 = fptext1.Split();
                                switch (dm1[0])
                                {



                                    case "January": dm1[0] = "January ";
                                        monthvalue = 01;
                                        break;
                                    case "February": monthyear = "February ";
                                        monthvalue = 02;
                                        break;
                                    case "March": monthyear = "March ";
                                        monthvalue = 03;
                                        break;
                                    case "April": monthyear = "April ";
                                        monthvalue = 04;
                                        break;
                                    case "May": monthyear = "May ";
                                        monthvalue = 05;
                                        break;
                                    case "June": monthyear = "June ";
                                        monthvalue = 06;
                                        break;
                                    case "July": monthyear = "July ";
                                        monthvalue = 07;
                                        break;
                                    case "August": monthyear = "August ";
                                        monthvalue = 08;
                                        break;
                                    case "September": monthyear = "September ";
                                        monthvalue = 09;
                                        break;
                                    case "October": monthyear = "October ";
                                        monthvalue = 10;
                                        break;
                                    case "November": monthyear = "November ";
                                        monthvalue = 11;
                                        break;
                                    case "December": monthyear = "December ";
                                        monthvalue = 12;
                                        break;
                                }
                                string q1 = "select distinct r.app_no,r.college_code ,r.reg_no,roll_no,r.Stud_Name,Stud_Type,r.degree_code,Branch_code,r.Batch_Year,C.Course_Name,c.Course_Id,Sections,((CONVERT(varchar(max), r.Batch_Year)+' - '+C.Course_Name+' - '+dt.dept_acronym+ case when sections='' then '' else ' - '+ (sections) end)) as batch,dt.Dept_Name,attnyear,attnmonth,FromDate from Registration r,Degree d,Department dt,course c,Late_attendance La where d.Dept_Code=dt.Dept_Code and c.Course_Id=d.Course_Id and r.degree_code=d.Degree_Code and r.college_code='" + Convert.ToString(ddlcollege.SelectedValue) + "' and r.Batch_Year in('" + addbatch + "')  and d.degree_code in('" + adddeg + "') and  c.Course_Id='" + course + "' and r.app_no=La.App_No and r.college_code=La.college_code  and fromdate='" + dm1[1] + '-' + monthvalue + '-' + fptext + "' order by r.Batch_Year,r.degree_code";
                                ds = d2.select_method_wo_parameter(q1, "Text");

                                Fpspread2.Visible = true;
                                Fpspread2.Sheets[0].AutoPostBack = true;
                                Fpspread2.Sheets[0].RowHeader.Visible = false;
                                FarPoint.Web.Spread.StyleInfo style2 = new FarPoint.Web.Spread.StyleInfo();
                                style2.Font.Size = 14;
                                style2.Font.Name = "Book Antiqua";
                                style2.Font.Bold = true;
                                style2.HorizontalAlign = HorizontalAlign.Center;
                                style2.ForeColor = Color.Black;
                                style2.BackColor = Color.AliceBlue;

                                Fpspread2.Sheets[0].RowCount = 0;
                                Fpspread2.Sheets[0].ColumnCount = 5;
                                Fpspread2.Sheets[0].ColumnHeader.RowCount = 2;
                                Fpspread2.ActiveSheetView.ColumnHeader.DefaultStyle.Font.Size = FontUnit.Medium;
                                Fpspread2.ActiveSheetView.ColumnHeader.DefaultStyle.Font.Bold = true;
                                Fpspread2.ActiveSheetView.ColumnHeader.DefaultStyle.Font.Name = "Book Antiqua";
                                Fpspread2.Sheets[0].ColumnHeader.Cells[0, 0].Text = "S.No";
                                Fpspread2.Sheets[0].ColumnHeader.Cells[0, 0].Font.Bold = true;
                                Fpspread2.Sheets[0].ColumnHeader.Cells[0, 0].Font.Name = "Book Antiqua";
                                Fpspread2.Sheets[0].ColumnHeader.Cells[0, 0].Font.Size = FontUnit.Medium;
                                Fpspread2.Sheets[0].ColumnHeader.Cells[0, 0].HorizontalAlign = HorizontalAlign.Center;


                                DataView dv1 = new DataView();
                                Hashtable hat = new Hashtable();
                                hat.Add("Roll_No", "Roll No");
                                hat.Add("reg_no", "Reg No");
                                hat.Add("Stud_Name", "Student Name");
                                hat.Add("Batch_Year", "Batch Year");
                                hat.Add("Course_Name", "Degree");
                                hat.Add("Dept_Name", "Department");
                               

                                if (ds.Tables[0].Rows.Count > 0)
                                {
                                    Fpspread2.Sheets[0].RowCount = 0;
                                    Fpspread2.Sheets[0].ColumnCount = 0;
                                    Fpspread2.CommandBar.Visible = false;
                                    Fpspread2.Sheets[0].AutoPostBack = true;
                                    Fpspread2.Sheets[0].ColumnHeader.RowCount = 1;
                                    Fpspread2.Sheets[0].RowHeader.Visible = false;
                                    Fpspread2.Sheets[0].ColumnCount = Itemindex.Count + 1;
                                    Fpspread2.Sheets[0].RowCount = ds.Tables[0].Rows.Count;
                                    FarPoint.Web.Spread.StyleInfo darkstyle = new FarPoint.Web.Spread.StyleInfo();
                                    darkstyle.BackColor = ColorTranslator.FromHtml("#0CA6CA");
                                    darkstyle.ForeColor = Color.White;
                                    Fpspread2.ActiveSheetView.ColumnHeader.DefaultStyle = darkstyle;
                                    Fpspread2.Sheets[0].ColumnHeader.Cells[0, 0].Text = "S.No";
                                    Fpspread2.Sheets[0].ColumnHeader.Cells[0, 0].Font.Bold = true;
                                    Fpspread2.Sheets[0].ColumnHeader.Cells[0, 0].HorizontalAlign = HorizontalAlign.Center;
                                    Fpspread2.Sheets[0].ColumnHeader.Cells[0, 0].Font.Name = "Book Antiqua";
                                    Fpspread2.Sheets[0].ColumnHeader.Cells[0, 0].Font.Size = FontUnit.Medium;
                                    Fpspread2.Columns[0].Width = 50;
                                    int count = 0;
                                    for (int i = 0; i < cblcolumnorder.Items.Count; i++)
                                    {
                                        if (cblcolumnorder.Items[i].Selected == true)
                                        {
                                            hat.Add(cblcolumnorder.Items[i].Text, cblcolumnorder.Items[i].Value);
                                            string colvalue = cblcolumnorder.Items[i].Text;
                                            if (ItemList.Contains(colvalue) == false)
                                            {
                                                ItemList.Add(cblcolumnorder.Items[i].Text);
                                            }
                                            tborder.Text = "";
                                            for (int j = 0; j < ItemList.Count; j++)
                                            {
                                                tborder.Text = tborder.Text + ItemList[j].ToString();
                                                tborder.Text = tborder.Text + "(" + (j + 1).ToString() + ")  ";
                                            }
                                        }
                                        cblcolumnorder.Items[0].Enabled = false;
                                    }
                                    if (ItemList.Count == 0)
                                    {
                                        for (int i = 0; i < 3; i++)
                                        {
                                            cblcolumnorder.Items[i].Selected = true;
                                            hat.Add(cblcolumnorder.Items[i].Text, cblcolumnorder.Items[i].Value);
                                            string colvalue = cblcolumnorder.Items[i].Text;
                                            if (ItemList.Contains(colvalue) == false)
                                            {
                                                ItemList.Add(cblcolumnorder.Items[i].Text);
                                            }
                                            tborder.Text = "";
                                            for (int j = 0; j < ItemList.Count; j++)
                                            {
                                                tborder.Text = tborder.Text + ItemList[j].ToString();
                                                tborder.Text = tborder.Text + "(" + (j + 1).ToString() + ")  ";
                                            }
                                        }
                                    }
                                    Fpspread2.Sheets[0].ColumnHeader.RowCount = 1;
                                    Fpspread2.Sheets[0].ColumnCount = 1;
                                    Fpspread2.Sheets[0].ColumnHeader.Cells[0, 0].Text = "S.No";
                                    Fpspread2.Sheets[0].ColumnHeader.Cells[0, 0].Font.Bold = true;
                                    Fpspread2.Sheets[0].ColumnHeader.Cells[0, 0].Font.Name = "Book Antiqua";
                                    Fpspread2.Sheets[0].ColumnHeader.Cells[0, 0].Font.Size = FontUnit.Medium;
                                    Fpspread2.Sheets[0].ColumnHeader.Cells[0, 0].HorizontalAlign = HorizontalAlign.Center;
                                    FarPoint.Web.Spread.StyleInfo style3 = new FarPoint.Web.Spread.StyleInfo();
                                    style2.Font.Size = 13;
                                    style2.Font.Name = "Book Antiqua";
                                    style2.Font.Bold = true;
                                    style2.HorizontalAlign = HorizontalAlign.Center;
                                    style2.ForeColor = Color.Black;
                                    style2.BackColor = Color.AliceBlue;
                                    Fpspread2.Sheets[0].PageSize = Fpspread2.Sheets[0].RowCount;
                                    for (int i = 0; i < ItemList.Count; i++)
                                    {
                                        string value1 = ItemList[i].ToString();
                                        int a = value1.Length;
                                        Fpspread2.Sheets[0].ColumnCount++;
                                        Fpspread2.Sheets[0].ColumnHeader.Cells[Fpspread2.Sheets[0].ColumnHeader.RowCount - 1, Fpspread2.Sheets[0].ColumnCount - 1].Text = ItemList[i].ToString();
                                        Fpspread2.Sheets[0].ColumnHeader.Cells[Fpspread2.Sheets[0].ColumnHeader.RowCount - 1, Fpspread2.Sheets[0].ColumnCount - 1].Font.Name = "Book Antiqua";
                                        Fpspread2.Sheets[0].ColumnHeader.Cells[Fpspread2.Sheets[0].ColumnHeader.RowCount - 1, Fpspread2.Sheets[0].ColumnCount - 1].Font.Bold = true;
                                        Fpspread2.Sheets[0].ColumnHeader.Cells[Fpspread2.Sheets[0].ColumnHeader.RowCount - 1, Fpspread2.Sheets[0].ColumnCount - 1].Font.Size = FontUnit.Medium;
                                        Fpspread2.Sheets[0].ColumnHeader.Cells[Fpspread2.Sheets[0].ColumnHeader.RowCount - 1, Fpspread2.Sheets[0].ColumnCount - 1].HorizontalAlign = HorizontalAlign.Center;
                                    }
                                    //Fpspread2.Sheets[0].ColumnCount++;
                                    //Fpspread2.Sheets[0].ColumnHeader.Cells[Fpspread2.Sheets[0].ColumnHeader.RowCount - 1, Fpspread2.Sheets[0].ColumnCount - 1].Text = "Leave Count";
                                    Fpspread2.Sheets[0].RowCount = 0;

                                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                                    {
                                        Fpspread2.Sheets[0].RowCount++;
                                        count++;
                                        Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 0].Text = count.ToString();
                                        Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
                                        Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 0].Font.Name = "Book Antiqua";
                                        Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 0].Font.Size = FontUnit.Medium;
                                        int c = 0;

                                        for (int j = 0; j < ItemList.Count; j++)
                                        {
                                            string k = Convert.ToString(ItemList[j].ToString());
                                            string value = Convert.ToString(hat[k].ToString());
                                            c++;
                                            FarPoint.Web.Spread.TextCellType textcel_type = new FarPoint.Web.Spread.TextCellType();
                                            Fpspread2.Sheets[0].Columns[2].CellType = textcel_type;
                                            Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, c].Text = ds.Tables[0].Rows[i][value].ToString();
                                            Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, c].Font.Name = "Book Antiqua";
                                            Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, c].Font.Size = FontUnit.Medium;
                                            Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, c].HorizontalAlign = HorizontalAlign.Center;
                                        }
                                        //c++;
                                        //Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, c].Text = ds.Tables[0].Rows[i][value].ToString();
                                        //Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, c].Font.Name = "Book Antiqua";
                                        //Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, c].Font.Size = FontUnit.Medium;
                                        //Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, c].HorizontalAlign = HorizontalAlign.Left;
                                    }
                                    Div1.Visible = true;
                                    Fpspread2.Visible = true;
                                }
                                else
                                {
                                    Div1.Visible = false;
                                    Fpspread2.Visible = false;
                                }
                            }
                        }
                                     
                                //Fpspread2.Sheets[0].ColumnHeader.Cells[0, 1].Text = "Roll Year";
                                //Fpspread2.Sheets[0].ColumnHeader.Cells[0, 1].Font.Bold = true;
                                //Fpspread2.Sheets[0].ColumnHeader.Cells[0, 1].Font.Name = "Book Antiqua";
                                //Fpspread2.Sheets[0].ColumnHeader.Cells[0, 1].Font.Size = FontUnit.Medium;
                                //Fpspread2.Sheets[0].ColumnHeader.Cells[0, 1].HorizontalAlign = HorizontalAlign.Center;
                                //Fpspread2.Sheets[0].ColumnHeader.Cells[0, 1].Text = "Batch Year";
                                //Fpspread2.Sheets[0].ColumnHeader.Cells[0, 1].Font.Bold = true;
                                //Fpspread2.Sheets[0].ColumnHeader.Cells[0, 1].Font.Name = "Book Antiqua";
                                //Fpspread2.Sheets[0].ColumnHeader.Cells[0, 1].Font.Size = FontUnit.Medium;
                                //Fpspread2.Sheets[0].ColumnHeader.Cells[0, 1].HorizontalAlign = HorizontalAlign.Center;
                                //Fpspread2.Sheets[0].ColumnHeader.Cells[0, 2].Text = "Degree";
                                //Fpspread2.Sheets[0].ColumnHeader.Cells[0, 2].Font.Bold = true;
                                //Fpspread2.Sheets[0].ColumnHeader.Cells[0, 2].Font.Name = "Book Antiqua";
                                //Fpspread2.Sheets[0].ColumnHeader.Cells[0, 2].Font.Size = FontUnit.Medium;
                                //Fpspread2.Sheets[0].ColumnHeader.Cells[0, 2].HorizontalAlign = HorizontalAlign.Center;
                                //Fpspread2.Sheets[0].ColumnHeader.Cells[0, 3].Text = "Branch";
                                //Fpspread2.Sheets[0].ColumnHeader.Cells[0, 3].Font.Bold = true;
                                //Fpspread2.Sheets[0].ColumnHeader.Cells[0, 3].Font.Name = "Book Antiqua";
                                //Fpspread2.Sheets[0].ColumnHeader.Cells[0, 3].Font.Size = FontUnit.Medium;
                                //Fpspread2.Sheets[0].ColumnHeader.Cells[0, 3].HorizontalAlign = HorizontalAlign.Center;
                                //Fpspread2.Sheets[0].ColumnHeader.Cells[0, 4].Text = "Count";
                                //Fpspread2.Sheets[0].ColumnHeader.Cells[0, 4].Font.Bold = true;
                                //Fpspread2.Sheets[0].ColumnHeader.Cells[0, 4].Font.Name = "Book Antiqua";
                                //Fpspread2.Sheets[0].ColumnHeader.Cells[0, 4].Font.Size = FontUnit.Medium;
                                //Fpspread2.Sheets[0].ColumnHeader.Cells[0, 4].HorizontalAlign = HorizontalAlign.Center;
                        Fpspread2.Sheets[0].PageSize = Fpspread2.Sheets[0].RowCount;
                        Fpspread2.Width = 900;
                        Fpspread2.Height = 900;
                        Fpspread2.SaveChanges();
                            
                       
            }
        }
        catch
        {
        }
    }


    public void cblcolumnorder_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            CheckBox_column.Checked = false;
            string value = "";
            int index;
            cblcolumnorder.Items[0].Selected = true;
            // cblcolumnorder.Items[0].Enabled = false;
            value = string.Empty;
            string result = Request.Form["__EVENTTARGET"];
            string[] checkedBox = result.Split('$');
            index = int.Parse(checkedBox[checkedBox.Length - 1]);
            string sindex = Convert.ToString(index);
            if (cblcolumnorder.Items[index].Selected)
            {
                if (!Itemindex.Contains(sindex))
                {
                    //if (tborder.Text == "")
                    //{
                    //    ItemList.Add("Company Code");
                    //}
                    //ItemList.Add("Admission No");
                    //ItemList.Add("Name");
                    ItemList.Add(cblcolumnorder.Items[index].Text.ToString());
                    Itemindex.Add(sindex);
                }
            }
            else
            {
                ItemList.Remove(cblcolumnorder.Items[index].Text.ToString());
                Itemindex.Remove(sindex);
            }
            for (int i = 0; i < cblcolumnorder.Items.Count; i++)
            {
                //if (i == 0 || i == 1 || i == 2)
                //{
                //    cblcolumnorder.Items[0].Selected = true;
                //    cblcolumnorder.Items[1].Selected = true;
                //    cblcolumnorder.Items[2].Selected = true;
                //}
                if (cblcolumnorder.Items[i].Selected == false)
                {
                    sindex = Convert.ToString(i);
                    ItemList.Remove(cblcolumnorder.Items[i].Text.ToString());
                    Itemindex.Remove(sindex);
                }
            }
            lnk_columnorder.Visible = true;
            tborder.Visible = true;
            tborder.Text = "";
            string colname12 = "";
            for (int i = 0; i < ItemList.Count; i++)
            {
                if (colname12 == "")
                {
                    colname12 = ItemList[i].ToString() + "(" + (i + 1).ToString() + ")";
                }
                else
                {
                    colname12 = colname12 + "," + ItemList[i].ToString() + "(" + (i + 1).ToString() + ")";
                }
                //tborder.Text = tborder.Text + ItemList[i].ToString();
                //tborder.Text = tborder.Text + "(" + (i + 1).ToString() + ")  ";
            }
            tborder.Text = colname12;
            if (ItemList.Count == 14)
            {
                CheckBox_column.Checked = true;
            }
            if (ItemList.Count == 0)
            {
                tborder.Visible = false;
                lnk_columnorder.Visible = false;
            }
            //  Button2.Focus();
        }
        catch (Exception ex)
        {
        }
    }

    public void LinkButtonsremove_Click(object sender, EventArgs e)
    {
        try
        {
            cblcolumnorder.ClearSelection();
            CheckBox_column.Checked = false;
            lnk_columnorder.Visible = false;
            //cblcolumnorder.Items[0].Selected = true;
            ItemList.Clear();
            Itemindex.Clear();
            tborder.Text = "";
            tborder.Visible = false;
            //Button2.Focus();
        }
        catch (Exception ex)
        {
        }
    }


    public void CheckBox_column_CheckedChanged(object sender, EventArgs e)
    {
        try
        {
            if (CheckBox_column.Checked == true)
            {
                tborder.Text = "";
                ItemList.Clear();
                for (int i = 0; i < cblcolumnorder.Items.Count; i++)
                {
                    string si = Convert.ToString(i);
                    cblcolumnorder.Items[i].Selected = true;
                    lnk_columnorder.Visible = true;
                    ItemList.Add(cblcolumnorder.Items[i].Text.ToString());
                    Itemindex.Add(si);
                }
                lnk_columnorder.Visible = true;
                tborder.Visible = true;
                tborder.Text = "";
                int j = 0;
                string colname12 = "";
                for (int i = 0; i < ItemList.Count; i++)
                {
                    j = j + 1;
                    if (colname12 == "")
                    {
                        colname12 = ItemList[i].ToString() + "(" + (j).ToString() + ")";
                    }
                    else
                    {
                        colname12 = colname12 + "," + ItemList[i].ToString() + "(" + (j).ToString() + ")";
                    }
                    // tborder.Text = tborder.Text + ItemList[i].ToString();
                }
                tborder.Text = colname12;
            }
            else
            {
                for (int i = 0; i < cblcolumnorder.Items.Count; i++)
                {
                    cblcolumnorder.Items[i].Selected = false;
                    lnk_columnorder.Visible = false;
                    ItemList.Clear();
                    Itemindex.Clear();
                    //cblcolumnorder.Items[0].Selected = true;
                }
                tborder.Text = "";
                tborder.Visible = false;
            }
            //  Button2.Focus();
        }
        catch (Exception ex)
        {
        }
    }

    public void rdbmess_CheckedChange(object sender, EventArgs e)
    {
        try
        {
            if (rdbmess.Checked == true)
            {
                Label2.Visible = true;
                Label1.Visible = true;
                TextBox1.Visible = true;
                TextBox2.Visible = true;
            }
            else
            {
                Label2.Visible = false;
                Label1.Visible = false;
                TextBox1.Visible = false;
                TextBox2.Visible = false;
            }
        }
        catch
        {
        }
    }
    public void countwise()
    {
        try
        {
               string adddeg = string.Empty;
                string addsem = string.Empty;
                string adddsec = string.Empty;
                            string addbatch = string.Empty;
               
                int colcount = 4;
                addbatch = rs.GetSelectedItemsValueAsString(cbl_batch);
                adddeg = rs.GetSelectedItemsValueAsString(cbl_branch);
                //addsem = rs.GetSelectedItemsValueAsString(cbl_sem);
               // adddsec = rs.GetSelectedItemsValueAsString(cbl_sec);

                string typ = string.Empty;
                if (cbl_sec.Items.Count > 0)
                {
                    for (int i = 0; i < cbl_sec.Items.Count; i++)
                    {
                        //if (cbl_sec.Items[i].Text == "Empty")
                        //{
                        //    cbl_sec.Items[i].Text = "''";
                        //}
                        if (cbl_sec.Items[i].Selected == true)
                        {
                            if (typ == "")
                            {
                                typ = "" + cbl_sec.Items[i].Text + "";
                            }
                            else if (cbl_sec.Items[i].Text == "Empty")
                            {
                                typ = typ + "'" + "," + "'";
                            }
                            else
                            {
                                typ = typ + "'" + "," + "'" + cbl_sec.Items[i].Text + "";

                            }
                        }
                        
                       
                    }
                }
                adddsec = typ;
            string    bran = rs.GetSelectedItemsValueAsString(cbl_branch);
            string Edu_Level = rs.GetSelectedItemsValueAsString(cbl_graduation);
            string course = rs.GetSelectedItemsValueAsString(cbl_degree);
             string fromdate = string.Empty;
                string todate = string.Empty;
                string date = string.Empty;
                string date1 = string.Empty;
                if (txt_fromdate.Text != "" && txt_todate.Text != "")
                {
                    fromdate = txt_fromdate.Text;
                    todate = txt_todate.Text;
                    string[] splitti = fromdate.Split('/');
                    if (splitti.Length == 3)
                    {
                        string dats = splitti[2];
                        string hrs = splitti[0];
                        string mins = splitti[1];
                        date = dats + '-' + mins + '-' + hrs;
                    }
                    string[] split2 = todate.Split('/');
                    if (split2.Length == 3)
                    {
                        string hr2 = split2[0];
                        string min2 = split2[1];
                        string day2 = split2[2];
                        date1 = day2 + '-' + min2 + '-' + hr2;
                    }
                }
                string q1 = "select  r.app_no,r.reg_no,r.college_code ,roll_no,r.Stud_Name,Stud_Type,r.degree_code,Branch_code,r.Batch_Year,C.Course_Name,c.Course_Id,Sections,((CONVERT(varchar(max), r.Batch_Year)+' - '+C.Course_Name+' - '+dt.dept_acronym+ case when sections='' then '' else ' - '+ (sections) end)) as batch,dt.Dept_Name,attnyear,attnmonth,FromDate from Registration r,Degree d,Department dt,course c,Late_attendance La where d.Dept_Code=dt.Dept_Code and c.Course_Id=d.Course_Id and r.degree_code=d.Degree_Code and r.college_code='" + Convert.ToString(ddlcollege.SelectedValue) + "'  and r.Batch_Year in('" + addbatch + "') and c.Edu_Level in('" + Edu_Level + "') and c.Course_Id in('" + course + "') and d.degree_code in('" + adddeg + "') and  LTRIM(RTRIM(isnull(r.sections,''))) in('" + adddsec + "') and  r.app_no=La.App_No and r.college_code=La.college_code  and fromdate between '" + date + "' and '" + date1 + "' order by r.Batch_Year,r.degree_code";
                                ds = d2.select_method_wo_parameter(q1, "Text");
                               
                                pheaderfilter.Visible = true;
                                pcolumnorder.Visible = true;
                                Fpspread2.Visible = true;
                                Fpspread2.Sheets[0].AutoPostBack = true;
                                Fpspread2.Sheets[0].RowHeader.Visible = false;
                                FarPoint.Web.Spread.StyleInfo style2 = new FarPoint.Web.Spread.StyleInfo();
                                style2.Font.Size = 14;
                                style2.Font.Name = "Book Antiqua";
                                style2.Font.Bold = true;
                                style2.HorizontalAlign = HorizontalAlign.Center;
                                style2.ForeColor = Color.Black;
                                style2.BackColor = Color.AliceBlue;

                                Fpspread2.Sheets[0].RowCount = 0;
                                Fpspread2.Sheets[0].ColumnCount = 5;
                                Fpspread2.Sheets[0].ColumnHeader.RowCount = 2;
                                Fpspread2.ActiveSheetView.ColumnHeader.DefaultStyle.Font.Size = FontUnit.Medium;
                                Fpspread2.ActiveSheetView.ColumnHeader.DefaultStyle.Font.Bold = true;
                                Fpspread2.ActiveSheetView.ColumnHeader.DefaultStyle.Font.Name = "Book Antiqua";
                                Fpspread2.Sheets[0].ColumnHeader.Cells[0, 0].Text = "S.No";
                                Fpspread2.Sheets[0].ColumnHeader.Cells[0, 0].Font.Bold = true;
                                Fpspread2.Sheets[0].ColumnHeader.Cells[0, 0].Font.Name = "Book Antiqua";
                                Fpspread2.Sheets[0].ColumnHeader.Cells[0, 0].Font.Size = FontUnit.Medium;
                                Fpspread2.Sheets[0].ColumnHeader.Cells[0, 0].HorizontalAlign = HorizontalAlign.Center;
             int stacun=0;
             int con=0;
            int endcun=0;
            int.TryParse(TextBox1.Text, out stacun);
            int.TryParse(TextBox2.Text, out endcun);
                                DataView dv1 = new DataView();
                                Hashtable hat = new Hashtable();
                                hat.Add("Roll_No", "Roll No");
                                hat.Add("reg_no", "Reg No");
                                hat.Add("Stud_Name", "Student Name");
                                hat.Add("Batch_Year", "Batch Year");
                                hat.Add("Course_Name", "Degree");
                                hat.Add("Dept_Name", "Department");
                               
          Hashtable  sturoll=new Hashtable();
            DataView dv=new DataView();
            if (ds.Tables[0].Rows.Count > 0)
            {
                Fpspread2.Sheets[0].RowCount = 0;
                Fpspread2.Sheets[0].ColumnCount = 0;
                Fpspread2.CommandBar.Visible = false;
                Fpspread2.Sheets[0].AutoPostBack = true;
                Fpspread2.Sheets[0].ColumnHeader.RowCount = 1;
                Fpspread2.Sheets[0].RowHeader.Visible = false;
                Fpspread2.Sheets[0].ColumnCount = Itemindex.Count + 1;
                Fpspread2.Sheets[0].RowCount = ds.Tables[0].Rows.Count;
                FarPoint.Web.Spread.StyleInfo darkstyle = new FarPoint.Web.Spread.StyleInfo();
                darkstyle.BackColor = ColorTranslator.FromHtml("#0CA6CA");
                darkstyle.ForeColor = Color.White;
                Fpspread2.ActiveSheetView.ColumnHeader.DefaultStyle = darkstyle;
                Fpspread2.Sheets[0].ColumnHeader.Cells[0, 0].Text = "S.No";
                Fpspread2.Sheets[0].ColumnHeader.Cells[0, 0].Font.Bold = true;
                Fpspread2.Sheets[0].ColumnHeader.Cells[0, 0].HorizontalAlign = HorizontalAlign.Center;
                Fpspread2.Sheets[0].ColumnHeader.Cells[0, 0].Font.Name = "Book Antiqua";
                Fpspread2.Sheets[0].ColumnHeader.Cells[0, 0].Font.Size = FontUnit.Medium;
                Fpspread2.Columns[0].Width = 50;
                int count = 0;
                for (int i = 0; i < cblcolumnorder.Items.Count; i++)
                {
                    if (cblcolumnorder.Items[i].Selected == true)
                    {
                        hat.Add(cblcolumnorder.Items[i].Text, cblcolumnorder.Items[i].Value);
                        string colvalue = cblcolumnorder.Items[i].Text;
                        if (ItemList.Contains(colvalue) == false)
                        {
                            ItemList.Add(cblcolumnorder.Items[i].Text);
                        }
                        tborder.Text = "";
                        for (int j = 0; j < ItemList.Count; j++)
                        {
                            tborder.Text = tborder.Text + ItemList[j].ToString();
                            tborder.Text = tborder.Text + "(" + (j + 1).ToString() + ")  ";
                        }
                    }
                    cblcolumnorder.Items[0].Enabled = false;
                }
                if (ItemList.Count == 0)
                {
                    for (int i = 0; i < 3; i++)
                    {
                        cblcolumnorder.Items[i].Selected = true;
                        hat.Add(cblcolumnorder.Items[i].Text, cblcolumnorder.Items[i].Value);
                        string colvalue = cblcolumnorder.Items[i].Text;
                        if (ItemList.Contains(colvalue) == false)
                        {
                            ItemList.Add(cblcolumnorder.Items[i].Text);
                        }
                        tborder.Text = "";
                        for (int j = 0; j < ItemList.Count; j++)
                        {
                            tborder.Text = tborder.Text + ItemList[j].ToString();
                            tborder.Text = tborder.Text + "(" + (j + 1).ToString() + ")  ";
                        }
                    }
                }
                Fpspread2.Sheets[0].ColumnHeader.RowCount = 1;
                Fpspread2.Sheets[0].ColumnCount = 1;
                Fpspread2.Sheets[0].ColumnHeader.Cells[0, 0].Text = "S.No";
                Fpspread2.Sheets[0].ColumnHeader.Cells[0, 0].Font.Bold = true;
                Fpspread2.Sheets[0].ColumnHeader.Cells[0, 0].Font.Name = "Book Antiqua";
                Fpspread2.Sheets[0].ColumnHeader.Cells[0, 0].Font.Size = FontUnit.Medium;
                Fpspread2.Sheets[0].ColumnHeader.Cells[0, 0].HorizontalAlign = HorizontalAlign.Center;
                FarPoint.Web.Spread.StyleInfo style3 = new FarPoint.Web.Spread.StyleInfo();
                style2.Font.Size = 13;
                style2.Font.Name = "Book Antiqua";
                style2.Font.Bold = true;
                style2.HorizontalAlign = HorizontalAlign.Center;
                style2.ForeColor = Color.Black;
                style2.BackColor = Color.AliceBlue;
                Fpspread2.Sheets[0].PageSize = Fpspread2.Sheets[0].RowCount;
                for (int i = 0; i < ItemList.Count; i++)
                {
                    string value1 = ItemList[i].ToString();
                    int a = value1.Length;
                    Fpspread2.Sheets[0].ColumnCount++;
                    Fpspread2.Sheets[0].ColumnHeader.Cells[Fpspread2.Sheets[0].ColumnHeader.RowCount - 1, Fpspread2.Sheets[0].ColumnCount - 1].Text = ItemList[i].ToString();
                    Fpspread2.Sheets[0].ColumnHeader.Cells[Fpspread2.Sheets[0].ColumnHeader.RowCount - 1, Fpspread2.Sheets[0].ColumnCount - 1].Font.Name = "Book Antiqua";
                    Fpspread2.Sheets[0].ColumnHeader.Cells[Fpspread2.Sheets[0].ColumnHeader.RowCount - 1, Fpspread2.Sheets[0].ColumnCount - 1].Font.Bold = true;
                    Fpspread2.Sheets[0].ColumnHeader.Cells[Fpspread2.Sheets[0].ColumnHeader.RowCount - 1, Fpspread2.Sheets[0].ColumnCount - 1].Font.Size = FontUnit.Medium;
                    Fpspread2.Sheets[0].ColumnHeader.Cells[Fpspread2.Sheets[0].ColumnHeader.RowCount - 1, Fpspread2.Sheets[0].ColumnCount - 1].HorizontalAlign = HorizontalAlign.Center;
                }
                Fpspread2.Sheets[0].ColumnCount++;
                Fpspread2.Sheets[0].ColumnHeader.Cells[Fpspread2.Sheets[0].ColumnHeader.RowCount - 1, Fpspread2.Sheets[0].ColumnCount - 1].Text = "Late Count";
                Fpspread2.Sheets[0].ColumnHeader.Cells[Fpspread2.Sheets[0].ColumnHeader.RowCount - 1, Fpspread2.Sheets[0].ColumnCount - 1].HorizontalAlign = HorizontalAlign.Center;
                Fpspread2.Sheets[0].RowCount = 0;

                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {

                    int c = 0;
                    string App = Convert.ToString(ds.Tables[0].Rows[i]["App_no"]);
                    if (!sturoll.ContainsKey(App))
                    {
                        ds.Tables[0].DefaultView.RowFilter = " App_no='" + Convert.ToString(ds.Tables[0].Rows[i]["App_no"]) + "'";
                        dv = ds.Tables[0].DefaultView;
                        sturoll.Add(Convert.ToString(ds.Tables[0].Rows[i]["App_no"]), Convert.ToString(ds.Tables[0].Rows[i]["Roll_no"]));
                        DataTable dt = new DataTable();
                        dt = dv.ToTable();
                        con = dt.Rows.Count;
                        if (dt.Rows.Count > 0)
                        {
                            if (stacun <= con && con <= endcun)
                            {
                                Fpspread2.Sheets[0].RowCount++;
                                count++;


                                Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
                                Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 0].Font.Name = "Book Antiqua";
                                Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 0].Font.Size = FontUnit.Medium;
                                Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 0].Text = count.ToString();
                                for (int j = 0; j < ItemList.Count; j++)
                                {

                                    string k = Convert.ToString(ItemList[j].ToString());
                                    string value = Convert.ToString(hat[k].ToString());
                                    c++;
                                    FarPoint.Web.Spread.TextCellType textcel_type = new FarPoint.Web.Spread.TextCellType();
                                    Fpspread2.Sheets[0].Columns[2].CellType = textcel_type;
                                    Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, c].Text = Convert.ToString(ds.Tables[0].Rows[i][value].ToString());
                                    Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, c].Font.Name = "Book Antiqua";
                                    Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, c].Font.Size = FontUnit.Medium;
                                    if (value == "Stud_Name")
                                        Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, c].HorizontalAlign = HorizontalAlign.Left;
                                    else
                                        Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, c].HorizontalAlign = HorizontalAlign.Center;
                                }

                                c++;
                                Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, c].Text = Convert.ToString(dt.Rows.Count);
                                Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, c].Font.Name = "Book Antiqua";
                                Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, c].Font.Size = FontUnit.Medium;
                                Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, c].HorizontalAlign = HorizontalAlign.Center;
                            }
                        }


                    }
                }


                //Fpspread2.Sheets[0].ColumnHeader.Cells[0, 1].Text = "Roll Year";
                //Fpspread2.Sheets[0].ColumnHeader.Cells[0, 1].Font.Bold = true;
                //Fpspread2.Sheets[0].ColumnHeader.Cells[0, 1].Font.Name = "Book Antiqua";
                //Fpspread2.Sheets[0].ColumnHeader.Cells[0, 1].Font.Size = FontUnit.Medium;
                //Fpspread2.Sheets[0].ColumnHeader.Cells[0, 1].HorizontalAlign = HorizontalAlign.Center;
                //Fpspread2.Sheets[0].ColumnHeader.Cells[0, 1].Text = "Batch Year";
                //Fpspread2.Sheets[0].ColumnHeader.Cells[0, 1].Font.Bold = true;
                //Fpspread2.Sheets[0].ColumnHeader.Cells[0, 1].Font.Name = "Book Antiqua";
                //Fpspread2.Sheets[0].ColumnHeader.Cells[0, 1].Font.Size = FontUnit.Medium;
                //Fpspread2.Sheets[0].ColumnHeader.Cells[0, 1].HorizontalAlign = HorizontalAlign.Center;
                //Fpspread2.Sheets[0].ColumnHeader.Cells[0, 2].Text = "Degree";
                //Fpspread2.Sheets[0].ColumnHeader.Cells[0, 2].Font.Bold = true;
                //Fpspread2.Sheets[0].ColumnHeader.Cells[0, 2].Font.Name = "Book Antiqua";
                //Fpspread2.Sheets[0].ColumnHeader.Cells[0, 2].Font.Size = FontUnit.Medium;
                //Fpspread2.Sheets[0].ColumnHeader.Cells[0, 2].HorizontalAlign = HorizontalAlign.Center;
                //Fpspread2.Sheets[0].ColumnHeader.Cells[0, 3].Text = "Branch";
                //Fpspread2.Sheets[0].ColumnHeader.Cells[0, 3].Font.Bold = true;
                //Fpspread2.Sheets[0].ColumnHeader.Cells[0, 3].Font.Name = "Book Antiqua";
                //Fpspread2.Sheets[0].ColumnHeader.Cells[0, 3].Font.Size = FontUnit.Medium;
                //Fpspread2.Sheets[0].ColumnHeader.Cells[0, 3].HorizontalAlign = HorizontalAlign.Center;
                //Fpspread2.Sheets[0].ColumnHeader.Cells[0, 4].Text = "Count";
                //Fpspread2.Sheets[0].ColumnHeader.Cells[0, 4].Font.Bold = true;
                //Fpspread2.Sheets[0].ColumnHeader.Cells[0, 4].Font.Name = "Book Antiqua";
                //Fpspread2.Sheets[0].ColumnHeader.Cells[0, 4].Font.Size = FontUnit.Medium;
                //Fpspread2.Sheets[0].ColumnHeader.Cells[0, 4].HorizontalAlign = HorizontalAlign.Center;
                Fpspread2.Sheets[0].PageSize = Fpspread2.Sheets[0].RowCount;
                Fpspread2.Width = 900;
                Fpspread2.Height = 900;
                Fpspread2.SaveChanges();
                Div1.Visible = true;
                Fpspread2.Visible = true;
                rptprint1.Visible = false;


            }
            else
            {
                Div1.Visible = false;
                Fpspread2.Visible = false;
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

            string Hostel = "Late Attendance Report ";
            string pagename = "Late Attendance Report.aspx.aspx";


            if (Fpspread1.Visible == true)
            {
                Printcontrol1.loadspreaddetails(Fpspread1, pagename, Hostel);
            }
            Printcontrol1.Visible = true;
            lbl_norec1.Visible = false;
        }

        catch
        {
        }
    }
    protected void btnExcel1_Click(object sender, EventArgs e)
    {
        lbl_norec1.Visible = false;
        try
        {
            string reportname = txtexcelname1.Text;
            if (reportname.ToString().Trim() != "")
            {

                if (Fpspread1.Visible == true)
                {
                    d2.printexcelreport(Fpspread1, reportname);
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
    protected void btnprintmaster2_Click(object sender, EventArgs e)
    {
        try
        {

            string Hostel = "Late Attendance Report ";
            string pagename = "Late Attendance Report.aspx";


            if (Fpspread2.Visible == true)
            {
                Printmaster1.loadspreaddetails(Fpspread2, pagename, Hostel);
            }
            Printmaster1.Visible = true;
            Label3.Visible = false;
        }

        catch
        {
        }
    }
    protected void btnExcel2_Click(object sender, EventArgs e)
    {
        Label3.Visible = false;
        try
        {
            string reportname = TextBox3.Text;
            if (reportname.ToString().Trim() != "")
            {

                if (Fpspread2.Visible == true)
                {
                    d2.printexcelreport(Fpspread2, reportname);
                }
                Label3.Visible = false;
            }
            else
            {
                Label3.Text = "Please Enter Your Report Name";
                Label3.Visible = true;
                TextBox3.Focus();
            }
        }
        catch
        {
        }
    }
}
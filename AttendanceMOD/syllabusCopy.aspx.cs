using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Collections;
using System.Configuration;

public partial class syllabusCopy : System.Web.UI.Page
{
    DataSet ds = new DataSet();
    DataSet ds2 = new DataSet();
    Hashtable hat = new Hashtable();
    DAccess2 dal = new DAccess2();
    DAccess2 d2 = new DAccess2();
    SqlConnection con = new SqlConnection(ConfigurationManager.AppSettings["DSN"].ToString());
    SqlCommand cmd;
    string usercode = string.Empty;
    string collegecode = string.Empty;
    string singleuser = string.Empty;
    string group_user = string.Empty;
    string group_code = string.Empty;
    string course_id = string.Empty;
    static int batchcnt = 0;
    static int degreecnt = 0;
    static int branchcnt = 0;
    static int sectioncnt = 0;
    static int subjectcnt = 0;


    string strbatch = string.Empty;
    string strbatchyear = string.Empty;
    string strbranch = string.Empty;
    string strsem = string.Empty;
    string strsec = string.Empty;

    int count = 0;
    int count1 = 0;
    int count2 = 0;
    int count3 = 0;
    int count4 = 0;
    protected void Page_Load(object sender, EventArgs e)
    {
        usercode = Session["usercode"].ToString();
        collegecode = Session["collegecode"].ToString();
        singleuser = Session["single_user"].ToString();
        group_user = Session["group_code"].ToString();
        if (!IsPostBack)
        {
            BindBatch();

            BindDegree();
            bindbranch();
            bindsem();
            GetSubject();

            toBindBatch();

            toBindDegree();
            tobindbranch();
            tobindsem();
            toGetSubject();


            panel_tree.Visible = false;
            paneltodesc.Visible = false;
            btnmove.Visible = false;
        }

    }

    #region bindvalues
    protected void ddlbatch_SelectedIndexChanged(object sender, EventArgs e)
    {
        GetSubject();
    }
    protected void toddlbatch_SelectedIndexChanged(object sender, EventArgs e)
    {
        toGetSubject();
    }

    public void GetSubject()
    {

        ddlsubject.Visible = true;
        ddlsubject.Enabled = true;

        try
        {
            ddlsubject.Items.Clear();
            string subjectquery = string.Empty;
            string sems = "";
            if (ddlsem.SelectedValue != "")
            {
                if (ddlsem.SelectedValue == "")
                {
                    sems = "";
                }
                else
                {
                    sems = "and SM.semester=" + ddlsem.SelectedValue.ToString() + "";
                }

                //subjectquery = "select distinct S.subject_no,subject_name from subject as S,syllabus_master  as SM,subjectchooser as SC,Sub_sem as Sem,staff_selector st where S.subject_no=SC.Subject_no and  s.syll_code=SM.syll_code and  st.subject_no=s.subject_no  and SM.degree_code=" + ddlbranch.SelectedValue.ToString() + " " + sems.ToString() + " and  S.subtype_no = Sem.subtype_no and promote_count=1 and SM.batch_year='" + ddlbatch.SelectedValue.ToString() + "' order by S.subject_no ";

                //added by rajasekar 12/07/2018
                string logstaffcode = "";
                if (Convert.ToString(Session["Staff_Code"]) != "")
                {
                    logstaffcode = " and st.staff_code='" + Convert.ToString(Session["Staff_Code"]) + "'";
                }
                //==================================//

                subjectquery = "select distinct S.subject_no,subject_name from subject as S,syllabus_master  as SM,subjectchooser as SC,Sub_sem as Sem,staff_selector st where S.subject_no=SC.Subject_no and  s.syll_code=SM.syll_code and  st.subject_no=s.subject_no  and SM.degree_code=" + ddlbranch.SelectedValue.ToString() + " " + sems.ToString() + " and  S.subtype_no = Sem.subtype_no and promote_count=1 and SM.batch_year='" + ddlbatch.SelectedValue.ToString() + "' " + logstaffcode + " order by S.subject_no ";

                if (subjectquery != "")
                {
                    ds.Dispose();
                    ds.Reset();
                    // ds = dal.select_method(subjectquery, hat, "Text");//===========Modified by Venkat=================
                    ds = dal.select_method_wo_parameter(subjectquery, "Text");
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        ddlsubject.Visible = true;
                        ddlsubject.Enabled = true;
                        ddlsubject.DataSource = ds;
                        ddlsubject.DataValueField = "Subject_No";
                        ddlsubject.DataTextField = "Subject_Name";
                        ddlsubject.DataBind();

                    }
                    else
                    {
                        ddlsubject.Enabled = false;
                    }

                }
            }
        }
        catch (Exception ex)
        {
            // throw ex;
        }
    }

    public void toGetSubject()
    {

        toddlsubject.Visible = true;
        toddlsubject.Enabled = true;

        try
        {
            toddlsubject.Items.Clear();
            string subjectquery = string.Empty;
            string sems = "";
            if (toddlsem.SelectedValue != "")
            {
                if (toddlsem.SelectedValue == "")
                {
                    sems = "";
                }
                else
                {
                    sems = "and SM.semester=" + toddlsem.SelectedValue.ToString() + "";
                }

                //subjectquery = "select distinct S.subject_no,subject_name from subject as S,syllabus_master  as SM,subjectchooser as SC,Sub_sem as Sem,staff_selector st where S.subject_no=SC.Subject_no and  s.syll_code=SM.syll_code and  st.subject_no=s.subject_no  and SM.degree_code=" + toddlbranch.SelectedValue.ToString() + " " + sems.ToString() + " and  S.subtype_no = Sem.subtype_no and promote_count=1 and SM.batch_year='" + toddlbatch.SelectedValue.ToString() + "' order by S.subject_no ";


                //added by rajasekar 12/07/2018
                string logstaffcode = "";
                if (Convert.ToString(Session["Staff_Code"]) != "")
                {
                    logstaffcode = " and st.staff_code='" + Convert.ToString(Session["Staff_Code"]) + "'";
                }
                //==================================//


                subjectquery = "select distinct S.subject_no,subject_name from subject as S,syllabus_master  as SM,subjectchooser as SC,Sub_sem as Sem,staff_selector st where S.subject_no=SC.Subject_no and  s.syll_code=SM.syll_code and  st.subject_no=s.subject_no  and SM.degree_code=" + toddlbranch.SelectedValue.ToString() + " " + sems.ToString() + " and  S.subtype_no = Sem.subtype_no and promote_count=1 and SM.batch_year='" + toddlbatch.SelectedValue.ToString() + "' " + logstaffcode + " order by S.subject_no ";




                if (subjectquery != "")
                {
                    ds.Dispose();
                    ds.Reset();
                    // ds = dal.select_method(subjectquery, hat, "Text");//===========Modified by Venkat=================
                    ds = dal.select_method_wo_parameter(subjectquery, "Text");
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        toddlsubject.Visible = true;
                        toddlsubject.Enabled = true;
                        toddlsubject.DataSource = ds;
                        toddlsubject.DataValueField = "Subject_No";
                        toddlsubject.DataTextField = "Subject_Name";
                        toddlsubject.DataBind();

                    }
                    else
                    {
                        toddlsubject.Enabled = false;
                    }

                }
            }
        }
        catch (Exception ex)
        {
            // throw ex;
        }
    }

    protected void ddldegree_SelectedIndexChanged(object sender, EventArgs e)
    {
        bindbranch();
        bindsem();
        GetSubject();

    }
    public void bindbranch()
    {

        ddlbranch.Items.Clear();
        hat.Clear();
        usercode = Session["usercode"].ToString();
        collegecode = Session["collegecode"].ToString();
        singleuser = Session["single_user"].ToString();
        group_user = Session["group_code"].ToString();
        if (group_user.Contains(';'))
        {
            string[] group_semi = group_user.Split(';');
            group_user = group_semi[0].ToString();
        }
        hat.Add("single_user", singleuser);
        hat.Add("group_code", group_user);
        hat.Add("course_id", ddldegree.SelectedValue);
        hat.Add("college_code", collegecode);
        hat.Add("user_code", usercode);

        ds.Dispose();
        ds.Reset();


        ds = dal.BindBranchMultiple(singleuser, group_user, ddldegree.SelectedValue.ToString(), collegecode, usercode);

        if (ds.Tables.Count > 0)
        {
            if (ds.Tables[0].Rows.Count > 0)
            {
                ddlbranch.DataSource = ds;
                ddlbranch.DataTextField = "dept_name";
                ddlbranch.DataValueField = "degree_code";
                ddlbranch.DataBind();
            }
        }
    }

    public void bindsem()
    {
        ddlsem.Items.Clear();
        Boolean first_year;
        first_year = false;
        int duration = 0;
        int i = 0;
        con.Close();
        con.Open();
        SqlDataReader dr;
        if (ddlbranch.SelectedValue.ToString() != string.Empty && ddlbatch.SelectedItem.ToString() != string.Empty)
        {
            cmd = new SqlCommand("select distinct ndurations,first_year_nonsemester from ndegree where degree_code='" + ddlbranch.SelectedValue.ToString() + "' and batch_year='" + ddlbatch.SelectedItem.ToString() + "' and college_code='" + Session["collegecode"].ToString() + "'", con);

            dr = cmd.ExecuteReader();
            dr.Read();
            if (dr.HasRows == true)
            {
                first_year = Convert.ToBoolean(dr[1].ToString());
                duration = Convert.ToInt16(dr[0].ToString());
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
                dr.Close();
                SqlDataReader dr1;
                cmd = new SqlCommand("select distinct duration,first_year_nonsemester  from degree where degree_code=" + ddlbranch.Text.ToString() + " and college_code=" + Session["collegecode"].ToString() + "", con);
                ddlsem.Items.Clear();
                dr1 = cmd.ExecuteReader();
                dr1.Read();
                if (dr1.HasRows == true)
                {
                    first_year = Convert.ToBoolean(dr1[1].ToString());
                    duration = Convert.ToInt16(dr1[0].ToString());

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

                dr1.Close();
            }
        }
    }

    public void BindDegree()
    {

        ddldegree.Items.Clear();

        collegecode = Session["collegecode"].ToString();
        singleuser = Session["single_user"].ToString();
        group_user = Session["group_code"].ToString();


        //con.Close();//==========================Modified by Venkat=======================
        //con.Open();

        //ds.Dispose();
        //ds.Reset();

        //cmd = new SqlCommand("select distinct course_name,course_id from course where college_code='" + collegecode + "'", con);
        //SqlDataAdapter da_degree = new SqlDataAdapter(cmd);
        //da_degree.Fill(ds);

        ds.Dispose();
        ds.Reset();
        string str = "select distinct course_name,course_id from course where college_code='" + collegecode + "'";
        ds = dal.select_method_wo_parameter(str, "Text");
        if (ds.Tables[0].Rows.Count > 0)
        {
            ddldegree.DataSource = ds;
            ddldegree.DataTextField = "course_name";
            ddldegree.DataValueField = "course_id";
            ddldegree.DataBind();
        }
    }

    public void BindBatch()
    {
        ddlbatch.Items.Clear();

        ds.Dispose();
        ds.Reset();

        ds = dal.select_method_wo_parameter("bind_batch", "sp");
        int count = ds.Tables[0].Rows.Count;
        if (count > 0)
        {
            ddlbatch.DataSource = ds;
            ddlbatch.DataTextField = "batch_year";
            ddlbatch.DataValueField = "batch_year";
            ddlbatch.DataBind();



        }
        int count1 = ds.Tables[1].Rows.Count;
        if (count > 0)
        {
            int max_bat = 0;
            max_bat = Convert.ToInt32(ds.Tables[1].Rows[0][0].ToString());
            ddlbatch.SelectedValue = max_bat.ToString();
        }
    }
    protected void ddlbranch_SelectedIndexChanged(object sender, EventArgs e)
    {
        bindsem();
        GetSubject();
    }
    protected void ddlsem_SelectedIndexChanged(object sender, EventArgs e)
    {
        GetSubject();

    }
    protected void ddlsubject_SelectedIndexChanged(object sender, EventArgs e)
    {

    }
    #endregion bindvalues

    #region tobind

    public void toBindBatch()
    {
        toddlbatch.Items.Clear();

        ds.Dispose();
        ds.Reset();

        ds = dal.select_method_wo_parameter("bind_batch", "sp");
        int count = ds.Tables[0].Rows.Count;
        if (count > 0)
        {
            toddlbatch.DataSource = ds;
            toddlbatch.DataTextField = "batch_year";
            toddlbatch.DataValueField = "batch_year";
            toddlbatch.DataBind();



        }
        int count1 = ds.Tables[1].Rows.Count;
        if (count > 0)
        {
            int max_bat = 0;
            max_bat = Convert.ToInt32(ds.Tables[1].Rows[0][0].ToString());
            toddlbatch.SelectedValue = max_bat.ToString();
        }
    }

    public void toBindDegree()
    {

        toddldegree.Items.Clear();

        collegecode = Session["collegecode"].ToString();
        singleuser = Session["single_user"].ToString();
        group_user = Session["group_code"].ToString();


        //con.Close();====================Modified by Venkat======================
        //con.Open();

        //ds.Dispose();
        //ds.Reset();

        //cmd = new SqlCommand("select distinct course_name,course_id from course where college_code='" + collegecode + "'", con);
        //SqlDataAdapter da_degree = new SqlDataAdapter(cmd);
        //da_degree.Fill(ds);

        ds.Dispose();
        ds.Reset();
        string str = "select distinct course_name,course_id from course where college_code='" + collegecode + "'";
        ds = dal.select_method_wo_parameter(str, "Text");
        if (ds.Tables[0].Rows.Count > 0)
        {
            toddldegree.DataSource = ds;
            toddldegree.DataTextField = "course_name";
            toddldegree.DataValueField = "course_id";
            toddldegree.DataBind();
        }
    }

    public void tobindsem()
    {
        toddlsem.Items.Clear();
        Boolean first_year;
        first_year = false;
        int duration = 0;
        int i = 0;
        con.Close();
        con.Open();
        SqlDataReader dr;
        if (toddlbranch.SelectedValue.ToString() != string.Empty && toddlbatch.SelectedItem.ToString() != string.Empty)
        {
            cmd = new SqlCommand("select distinct ndurations,first_year_nonsemester from ndegree where degree_code='" + toddlbranch.SelectedValue.ToString() + "' and batch_year='" + toddlbatch.SelectedItem.ToString() + "' and college_code='" + Session["collegecode"].ToString() + "'", con);

            dr = cmd.ExecuteReader();
            dr.Read();
            if (dr.HasRows == true)
            {
                first_year = Convert.ToBoolean(dr[1].ToString());
                duration = Convert.ToInt16(dr[0].ToString());
                for (i = 1; i <= duration; i++)
                {
                    if (first_year == false)
                    {
                        toddlsem.Items.Add(i.ToString());
                    }
                    else if (first_year == true && i != 2)
                    {
                        toddlsem.Items.Add(i.ToString());
                    }

                }
            }
            else
            {
                dr.Close();
                SqlDataReader dr1;
                cmd = new SqlCommand("select distinct duration,first_year_nonsemester  from degree where degree_code=" + toddlbranch.Text.ToString() + " and college_code=" + Session["collegecode"].ToString() + "", con);
                toddlsem.Items.Clear();
                dr1 = cmd.ExecuteReader();
                dr1.Read();
                if (dr1.HasRows == true)
                {
                    first_year = Convert.ToBoolean(dr1[1].ToString());
                    duration = Convert.ToInt16(dr1[0].ToString());

                    for (i = 1; i <= duration; i++)
                    {
                        if (first_year == false)
                        {
                            toddlsem.Items.Add(i.ToString());
                        }
                        else if (first_year == true && i != 2)
                        {
                            toddlsem.Items.Add(i.ToString());
                        }
                    }
                }

                dr1.Close();
            }
        }
    }

    public void tobindbranch()
    {

        toddlbranch.Items.Clear();
        hat.Clear();
        usercode = Session["usercode"].ToString();
        collegecode = Session["collegecode"].ToString();
        singleuser = Session["single_user"].ToString();
        group_user = Session["group_code"].ToString();
        if (group_user.Contains(';'))
        {
            string[] group_semi = group_user.Split(';');
            group_user = group_semi[0].ToString();
        }
        hat.Add("single_user", singleuser);
        hat.Add("group_code", group_user);
        hat.Add("course_id", toddldegree.SelectedValue);
        hat.Add("college_code", collegecode);
        hat.Add("user_code", usercode);

        ds.Dispose();
        ds.Reset();


        ds = dal.BindBranchMultiple(singleuser, group_user, toddldegree.SelectedValue.ToString(), collegecode, usercode);

        if (ds.Tables.Count > 0)
        {
            if (ds.Tables[0].Rows.Count > 0)
            {
                toddlbranch.DataSource = ds;
                toddlbranch.DataTextField = "dept_name";
                toddlbranch.DataValueField = "degree_code";
                toddlbranch.DataBind();
            }
        }
    }

    protected void toddldegree_SelectedIndexChanged(object sender, EventArgs e)
    {
        tobindbranch();
        tobindsem();
        toGetSubject();

    }

    protected void toddlbranch_SelectedIndexChanged(object sender, EventArgs e)
    {
        tobindsem();
        toGetSubject();
    }

    protected void toddlsem_SelectedIndexChanged(object sender, EventArgs e)
    {
        toGetSubject();

    }

    protected void toddlsubject_SelectedIndexChanged(object sender, EventArgs e)
    {

    }

    #endregion tobind

    #region  goclick

    protected void GO_Click(object sender, EventArgs e)
    {
        if (ddlsubject.Enabled == true)
        {
            lblmoveerror.Text = "";
            lblmoveerror.Visible = false;

            //con.Close();=========================Modified by Venkat==========================
            //con.Open();

            //SqlCommand cmd_subdetails = new SqlCommand("select subject_code,exam_system,duration,First_Year_Nonsemester,semester,syllabus_master.syllabus_year,subject_type,subject_name,subject.acronym,min_ext_marks,max_ext_marks,min_int_marks,max_int_marks,mintotal,maxtotal,noofhrsperweek,degree.degree_code from subject,sub_sem,syllabus_master,degree where degree.degree_code=syllabus_master.degree_code and sub_sem.subType_no=subject.subType_no and syllabus_master.syll_code=subject.syll_code and subject_no='" + ddlsubject.SelectedValue.ToString() + "'", con);
            //SqlDataAdapter da_subdetails = new SqlDataAdapter(cmd_subdetails);
            //
            //da_subdetails.Fill(ds_subdetails);


            DataSet ds_subdetails = new DataSet();
            string str = "select subject_code,exam_system,duration,First_Year_Nonsemester,semester,syllabus_master.syllabus_year,subject_type,subject_name,subject.acronym,min_ext_marks,max_ext_marks,min_int_marks,max_int_marks,mintotal,maxtotal,noofhrsperweek,degree.degree_code from subject,sub_sem,syllabus_master,degree where degree.degree_code=syllabus_master.degree_code and sub_sem.subType_no=subject.subType_no and syllabus_master.syll_code=subject.syll_code and subject_no='" + ddlsubject.SelectedValue.ToString() + "'";
            ds_subdetails = dal.select_method_wo_parameter(str, "Text");

            this.TreeView1.Nodes.Clear();
            string parentnode = ddlsubject.SelectedItem.ToString();
            TreeNode tn = new TreeNode(parentnode);
            TreeView1.Nodes.Add(tn);

            PopulateTreeview();

            TreeView1.Visible = true;
        }
        else
        {
            lblmoveerror.Text = "No Subjects Available";
            lblmoveerror.Visible = true;
        }

    }

    private void PopulateTreeview()
    {

        string dt_topics = "";
        string dt_topics1 = "";

        string subjectno = ddlsubject.SelectedValue.ToString();
        if (!string.IsNullOrEmpty(subjectno))
        {
            con.Close();
            con.Open();

            //this.TreeView1.Nodes.Clear();
            HierarchyTrees hierarchyTrees = new HierarchyTrees();
            HierarchyTrees.HTree objHTree = null;

            //start=======common tree load
            using (SqlCommand command = new SqlCommand("select topic_no,parent_code,unit_name from sub_unit_details where subject_no='" + subjectno + "' order by parent_code,topic_no ", con))
            {
                //this.TreeView1.Nodes.Clear();
                hierarchyTrees.Clear();
                SqlDataReader reader = command.ExecuteReader(System.Data.CommandBehavior.CloseConnection);
                while (reader.Read())
                {
                    objHTree = new HierarchyTrees.HTree();
                    objHTree.topic_no = int.Parse(reader["Topic_no"].ToString());
                    objHTree.parent_code = int.Parse(reader["parent_code"].ToString());
                    objHTree.unit_name = reader["unit_name"].ToString();
                    hierarchyTrees.Add(objHTree);
                }
            }

            foreach (HierarchyTrees.HTree hTree in hierarchyTrees)
            {
                HierarchyTrees.HTree parentNode = hierarchyTrees.Find(delegate(HierarchyTrees.HTree emp)
                {
                    return emp.topic_no == hTree.parent_code;
                });
                if (parentNode != null)
                {
                    foreach (TreeNode tn in TreeView1.Nodes)
                    {
                        if (tn.Value == parentNode.topic_no.ToString())
                        {
                            tn.ChildNodes.Add(new TreeNode(hTree.unit_name.ToString(), hTree.topic_no.ToString()));
                            //Session["session_tnValue"] = tn;
                        }
                        if (tn.ChildNodes.Count > 0)
                        {
                            foreach (TreeNode ctn in tn.ChildNodes)
                            {
                                RecursiveChild(ctn, parentNode.topic_no.ToString(), hTree);
                            }
                        }
                    }
                }
                else
                {
                    TreeView1.Nodes.Add(new TreeNode(hTree.unit_name, hTree.topic_no.ToString()));
                }

                TreeView1.ExpandAll();

            }

            TreeView1.ExpandAll();
            if (TreeView1.Nodes.Count < 1)
            {

            }
            else
            {
                panel_tree.Visible = true;
                btnmove.Visible = true;
            }
        }
    }

    public void RecursiveChild(TreeNode tn, string searchValue, HierarchyTrees.HTree hTree)
    {
        if (tn.Value == searchValue)
        {
            tn.ChildNodes.Add(new TreeNode(hTree.unit_name.ToString(), hTree.topic_no.ToString()));
        }
        if (tn.ChildNodes.Count > 0)
        {
            foreach (TreeNode ctn in tn.ChildNodes)
            {
                RecursiveChild(ctn, searchValue, hTree);
            }
        }
    }

    protected void TreeView1_SelectedNodeChanged(object sender, EventArgs e)
    {


    }
    protected void TreeView1_TreeNodeCheckChanged(object sender, EventArgs e)
    {
    }

    protected void TreeView2_SelectedNodeChanged(object sender, EventArgs e)
    {


    }
    protected void TreeView2_TreeNodeCheckChanged(object sender, EventArgs e)
    {
    }

    protected void btnmove_click(object sender, EventArgs e)
    {
        if (toddlsubject.Enabled == true)
        {
            lblmoveerror.Text = "";
            lblmoveerror.Visible = false;
            if (ddlsubject.SelectedValue != toddlsubject.SelectedValue)
            {
                lblmoveerror.Text = "";
                lblmoveerror.Visible = false;
                paneltodesc.Visible = true;
                this.TreeView2.Nodes.Clear();
                string parentnode = toddlsubject.SelectedItem.ToString();
                TreeNode tn = new TreeNode(parentnode);
                TreeView2.Nodes.Add(tn);
                toPopulateTreeview();
            }
            else
            {

                lblmoveerror.Text = "Kindly Select Different Subject";
                lblmoveerror.Visible = true;
            }
        }
        else
        {
            lblmoveerror.Text = "Subject Not Available";
            lblmoveerror.Visible = true;
        }
    }

    public class HierarchyTrees : List<HierarchyTrees.HTree>
    {
        public class HTree
        {
            private int m_topic_no;
            private int m_parent_code;
            private string m_unit_name;

            public int topic_no
            {
                get { return m_topic_no; }
                set { m_topic_no = value; }
            }

            public int parent_code
            {
                get { return m_parent_code; }
                set { m_parent_code = value; }
            }
            public string unit_name
            {
                get { return m_unit_name; }
                set { m_unit_name = value; }
            }

        }
    }

    private void toPopulateTreeview()
    {

        string dt_topics = "";
        string dt_topics1 = "";

        string subjectno = ddlsubject.SelectedValue.ToString();
        if (!string.IsNullOrEmpty(subjectno))
        {
            con.Close();
            con.Open();

            //this.TreeView1.Nodes.Clear();
            HierarchyTrees hierarchyTrees = new HierarchyTrees();
            HierarchyTrees.HTree objHTree = null;

            //start=======common tree load
            using (SqlCommand command = new SqlCommand("select topic_no,parent_code,unit_name from sub_unit_details where subject_no='" + subjectno + "' order by parent_code,topic_no ", con))
            {
                //this.TreeView1.Nodes.Clear();
                hierarchyTrees.Clear();
                SqlDataReader reader = command.ExecuteReader(System.Data.CommandBehavior.CloseConnection);
                while (reader.Read())
                {
                    objHTree = new HierarchyTrees.HTree();
                    objHTree.topic_no = int.Parse(reader["Topic_no"].ToString());
                    objHTree.parent_code = int.Parse(reader["parent_code"].ToString());
                    objHTree.unit_name = reader["unit_name"].ToString();
                    hierarchyTrees.Add(objHTree);
                }
            }

            foreach (HierarchyTrees.HTree hTree in hierarchyTrees)
            {
                HierarchyTrees.HTree parentNode = hierarchyTrees.Find(delegate(HierarchyTrees.HTree emp)
                {
                    return emp.topic_no == hTree.parent_code;
                }
                );
                if (parentNode != null)
                {
                    foreach (TreeNode tn in TreeView2.Nodes)
                    {
                        if (tn.Value == parentNode.topic_no.ToString())
                        {
                            tn.ChildNodes.Add(new TreeNode(hTree.unit_name.ToString(), hTree.topic_no.ToString()));
                            //Session["session_tnValue"] = tn;
                        }
                        if (tn.ChildNodes.Count > 0)
                        {
                            foreach (TreeNode ctn in tn.ChildNodes)
                            {
                                RecursiveChild(ctn, parentNode.topic_no.ToString(), hTree);
                            }
                        }
                    }
                }
                else
                {
                    TreeView2.Nodes.Add(new TreeNode(hTree.unit_name, hTree.topic_no.ToString()));
                }

                TreeView2.ExpandAll();

            }

            TreeView2.ExpandAll();
            if (TreeView2.Nodes.Count < 1)
            {

            }
            else
            {
                TreeView2.Visible = true;
                btnsave.Visible = true;
            }
        }
    }

    protected void btnsave_click(object sender, EventArgs e)
    {
        DataSet dscheck = new DataSet();
        string checksubjectnumber = "select * from sub_unit_details where subject_no='" + toddlsubject.SelectedValue + "'";
        // dscheck = d2.select_method_wo_parameter(checksubjectnumber, "n");===========Modified by VENKAT===============
        dscheck = d2.select_method_wo_parameter(checksubjectnumber, "Text");

        if (dscheck.Tables[0].Rows.Count > 0)
        {
            ModalPopupExtender2.Show();
        }
        else
        {
            bindandsavesubject();
        }

    }

    public void bindandsavesubject()
    {
        string subjectno = "";
        string parentcode = "";
        string unitname = "";
        string topicno = "";
        string description = "";
        string noofhrs = "";
        string bookref = "";
        string steachaid = "";
        string bookdetails = "";
        string methogology = "";
        string instruction = "";
        int count = 0;
        int firsttopicnumber = 0;
        int inctopicnum = 0;
        string bindparentcode = "";
        string bindtempparentcode = "";
        string bindpreviouspcode = "";
        ArrayList alunitname = new ArrayList();
        DataSet dsvalue = new DataSet();
        DataView dvvalue = new DataView();
        if (ddlsubject.SelectedValue != toddlsubject.SelectedValue)
        {
            lblerrormsg.Text = "";
            lblerrormsg.Visible = false;

            string query = "select * from sub_unit_details where subject_no='" + ddlsubject.SelectedValue + "'";
            dsvalue = d2.select_method_wo_parameter(query, "n");


            string dt_topics = "";
            string dt_topics1 = "";

            string subjectsno = toddlsubject.SelectedValue.ToString();
            subjectno = ddlsubject.SelectedValue.ToString();
            if (!string.IsNullOrEmpty(subjectsno))
            {
                con.Close();
                con.Open();

                //this.TreeView1.Nodes.Clear();
                HierarchyTrees hierarchyTrees = new HierarchyTrees();
                HierarchyTrees.HTree objHTree = null;

                // TreeView2.Nodes.Clear();
                //start=======common tree load
                using (SqlCommand command = new SqlCommand("select topic_no,parent_code,unit_name from sub_unit_details where subject_no='" + subjectno + "' order by parent_code,topic_no ", con))
                {
                    //this.TreeView1.Nodes.Clear();
                    hierarchyTrees.Clear();
                    SqlDataReader reader = command.ExecuteReader(System.Data.CommandBehavior.CloseConnection);
                    while (reader.Read())
                    {
                        objHTree = new HierarchyTrees.HTree();
                        objHTree.topic_no = int.Parse(reader["Topic_no"].ToString());
                        objHTree.parent_code = int.Parse(reader["parent_code"].ToString());
                        objHTree.unit_name = reader["unit_name"].ToString();
                        hierarchyTrees.Add(objHTree);
                    }
                }
                int i = 0;
                foreach (HierarchyTrees.HTree hTree in hierarchyTrees)
                {

                    HierarchyTrees.HTree parentNode = hierarchyTrees.Find(delegate(HierarchyTrees.HTree emp) { return emp.topic_no == hTree.parent_code; });
                    if (parentNode != null)
                    {
                        foreach (TreeNode tn in TreeView2.Nodes)
                        {

                            if (tn.Value == parentNode.topic_no.ToString())
                            {
                                //tn.ChildNodes.Add(new TreeNode(hTree.unit_name.ToString(), hTree.topic_no.ToString()));
                                //Session["session_tnValue"] = tn;
                            }
                            if (tn.ChildNodes.Count > 0)
                            {
                                foreach (TreeNode ctn in tn.ChildNodes)
                                {

                                    string checkunitname = parentNode.unit_name.ToString();
                                    if (alunitname.Contains(hTree.unit_name) == false)
                                    {

                                        alunitname.Add(hTree.unit_name);

                                        dsvalue.Tables[0].DefaultView.RowFilter = "unit_name='" + hTree.unit_name + "'";
                                        dvvalue = dsvalue.Tables[0].DefaultView;
                                        if (dvvalue.Count > 0)
                                        {
                                            description = Convert.ToString(dvvalue[0]["description"]);
                                        }

                                        string parentsNode = parentNode.topic_no.ToString();
                                        string bindpparentcode = "select topic_no from sub_unit_details where  subject_no='" + subjectsno + "' and unit_name='" + checkunitname + "' ";
                                        con.Close();
                                        con.Open();
                                        SqlCommand findparentcmd = new SqlCommand(bindpparentcode, con);
                                        string preparentcode = Convert.ToString(findparentcmd.ExecuteScalar());
                                        unitname = hTree.unit_name;
                                        //=================Modified by Venkat=======================
                                        //string insertvalue = "insert into sub_unit_details(subject_no,Parent_code,unit_name)values('" + subjectsno + "','" + preparentcode + "','" + unitname + "')";
                                        //con.Close();
                                        //con.Open();
                                        //SqlCommand sqlcmd = new SqlCommand(insertvalue, con);
                                        //count = sqlcmd.ExecuteNonQuery();
                                        string insertvalue = "insert into sub_unit_details(subject_no,Parent_code,unit_name)values('" + subjectsno + "','" + preparentcode + "','" + unitname + "')";
                                        int a = dal.update_method_wo_parameter(insertvalue, "Text");
                                    }

                                    // tn.ChildNodes.Remove(new TreeNode(hTree.unit_name.ToString(), hTree.topic_no.ToString()));
                                }
                            }
                        }
                    }
                    else
                    {
                        // TreeView1.Nodes.Add(new TreeNode(hTree.unit_name, hTree.topic_no.ToString()));
                        unitname = hTree.unit_name;
                        string pparentNode = "0";

                        //=================Modified by Venkat=======================
                        //string insertvalue = "insert into sub_unit_details(subject_no,Parent_code,unit_name)values('" + subjectsno + "','" + pparentNode + "','" + unitname + "')";
                        //con.Close();
                        //con.Open();
                        //SqlCommand sqlcmd = new SqlCommand(insertvalue, con);
                        //count = sqlcmd.ExecuteNonQuery();

                        string insertvalue = "insert into sub_unit_details(subject_no,Parent_code,unit_name)values('" + subjectsno + "','" + pparentNode + "','" + unitname + "')";
                        count = dal.update_method_wo_parameter(insertvalue, "Text");
                    }




                }

                if (count != 0)
                {
                    ScriptManager.RegisterStartupScript(this, typeof(Page), UniqueID, "alert('Syllabus copied Successfully')", true);
                    clear();
                }



            }


        }
        else
        {
            lblerrormsg.Text = "Kindly Select Different Subject";
            lblerrormsg.Visible = true;
        }
    }



    protected void btnOk_Click(object sender, EventArgs e)
    {
        pnlVerifysave.Visible = false;
        int check = 0;
        //======================Modified By Venkat==========================
        //string deletequery = "delete from sub_unit_details where subject_no='" + toddlsubject.SelectedValue + "'";
        //con.Close();
        //con.Open();
        //SqlCommand delcmd = new SqlCommand(deletequery, con);
        //check = delcmd.ExecuteNonQuery();

        string deletequery = "delete from sub_unit_details where subject_no='" + toddlsubject.SelectedValue + "'";
        check = dal.update_method_wo_parameter(deletequery, "Text");
        if (check != 0)
        {
            bindandsavesubject();

        }
    }
    protected void btnCancel_Click(object sender, EventArgs e)
    {
        ModalPopupExtender2.Hide();
    }

    public void clear()
    {
        BindBatch();

        BindDegree();
        bindbranch();
        bindsem();
        GetSubject();

        toBindBatch();

        toBindDegree();
        tobindbranch();
        tobindsem();
        toGetSubject();


        panel_tree.Visible = false;
        paneltodesc.Visible = false;
        btnmove.Visible = false;
        btnsave.Visible = false;
    }
    #endregion goclick
}
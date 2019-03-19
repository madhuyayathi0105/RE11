using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Collections;
using InsproDataAccess;

public partial class Syllabus_Entry : System.Web.UI.Page
{
    SqlConnection con = new SqlConnection(ConfigurationManager.AppSettings["DSN"].ToString());
    InsproDirectAccess dir = new InsproDirectAccess();
    DAccess2 dal = new DAccess2();
    DataSet srids = new DataSet();
    DataSet ds = new DataSet();
    DataSet ds2 = new DataSet();
    SqlCommand cmd;
    string sql_s = "";
    Boolean checkupdate = false;
    Hashtable hat = new Hashtable();

    #region
    DataTable dtable1 = new DataTable();
    DataRow dtrow = null;
    #endregion

    string usercode = string.Empty;
    string collegecode = string.Empty;
    string singleuser = string.Empty;
    string group_user = string.Empty;
    string group_code = string.Empty;


    //Boolean spread_boolean;  // hide by sridhar

    protected void lb2_Click(object sender, EventArgs e) //Aruna For Back Button
    {
        Session.Abandon();
        Session.Clear();
        Session.RemoveAll();
        System.Web.Security.FormsAuthentication.SignOut();
        Response.Redirect("~/Default.aspx", false);

    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            if (Session["collegecode"] == null) //Aruna For Back Button
            {
                Response.Redirect("~/Default.aspx");
            }
            newrowdiv.Visible = false;
            table_tree.Visible = false;
            lblsavevalidate.Visible = false;
            BindBatch();
            BindDegree();
            bindbranch();
            bindsem();
            GetSubject();
            ddl_selectValue.Attributes.Add("onfocus", "addvalue()");
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

    public void BindDegree()
    {

        ddldegree.Items.Clear();
        ////usercode = Session["usercode"].ToString();
        collegecode = Session["collegecode"].ToString();
        singleuser = Session["single_user"].ToString();
        group_user = Session["group_code"].ToString();
        //if (group_user.Contains(';'))
        //{
        //    string[] group_semi = group_user.Split(';');
        //    group_user = group_semi[0].ToString();
        //}
        //hat.Clear();
        //hat.Add("single_user", singleuser);
        //hat.Add("group_code", group_user);
        //hat.Add("college_code", collegecode);
        //hat.Add("user_code", usercode);
        ////ds = dal.select_method("bind_degree", hat, "sp");
        //ds = dal.BindDegree(singleuser, group_user, collegecode, usercode);

        //con.Close();
        //con.Open();

        ds.Dispose();
        ds.Reset();
        // add by sridharan
        sql_s = "select distinct course_name,course_id from course where college_code='" + collegecode + "'";
        // hide by sridhar  ////cmd = new SqlCommand("select distinct course_name,course_id from course where college_code='" + collegecode + "'", con);
        //SqlDataAdapter da_degree = new SqlDataAdapter(cmd);
        //da_degree.Fill(ds);
        ds = dal.select_method_wo_parameter(sql_s, "Text");

        if (ds.Tables[0].Rows.Count > 0)
        {
            ddldegree.DataSource = ds;
            ddldegree.DataTextField = "course_name";
            ddldegree.DataValueField = "course_id";
            ddldegree.DataBind();
        }
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

        //ds = dal.select_method("bind_branch", hat, "sp");
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

    public void GetSubject()
    {

        ddlsubject.Visible = true;
        ddlsubject.Enabled = true;

        try
        {
            ddlsubject.Items.Clear();

            string subjectquery = string.Empty;
            //chklstsubject.Items.Clear();               


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

                //if (Session["Staff_Code"].ToString() == "")
                //{

                //subjectquery = "select distinct S.subject_no,subject_name from subject as S,syllabus_master  as SM,subjectchooser as SC,Sub_sem as Sem,staff_selector st where S.subject_no=SC.Subject_no and  s.syll_code=SM.syll_code and  st.subject_no=s.subject_no  and SM.degree_code=" + ddlbranch.SelectedValue.ToString() + " " + sems.ToString() + " and  S.subtype_no = Sem.subtype_no and promote_count=1 and SM.batch_year='" + ddlbatch.SelectedValue.ToString() + "' order by S.subject_no ";
                //added by rajasekar 12/07/2018
                string logstaffcode = "";
                if (Convert.ToString(Session["Staff_Code"]) != "")
                {
                    logstaffcode = " and st.staff_code='" + Convert.ToString(Session["Staff_Code"]) + "'";
                }
                //==================================//
                subjectquery = "select distinct S.subject_no,subject_name from subject as S,syllabus_master  as SM,subjectchooser as SC,Sub_sem as Sem,staff_selector st where S.subject_no=SC.Subject_no and  s.syll_code=SM.syll_code and  st.subject_no=s.subject_no  and SM.degree_code=" + ddlbranch.SelectedValue.ToString() + " " + sems.ToString() + " and  S.subtype_no = Sem.subtype_no and promote_count=1 and SM.batch_year='" + ddlbatch.SelectedValue.ToString() + "' " + logstaffcode + " order by S.subject_no ";//rajasekar 12/07/2018


                //}
                //else if (Session["Staff_Code"].ToString() != "")
                //{
                //    subjectquery = "select distinct S.subject_no,subject_name from subject as S,syllabus_master  as SM,subjectchooser as SC,Sub_sem as Sem,staff_selector st where S.subject_no=SC.Subject_no and st.subject_no=s.subject_no and s.syll_code=SM.syll_code and  S.subtype_no = Sem.subtype_no and promote_count=1 and SM.degree_code=" + ddlbranch.SelectedValue.ToString() + " " + sems.ToString() + " and  SM.batch_year='" + ddlbatch.SelectedValue.ToString() + "'  and staff_code='" + Session["Staff_Code"].ToString() + "'  order by S.subject_no ";
                //}

                if (subjectquery != "")
                {
                    ds.Dispose();
                    ds.Reset();
                    ds = dal.select_method(subjectquery, hat, "Text");
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
                        //ddlsubParent.Enabled = false;
                        //psubject.Visible = false;
                        //txtsub.Enabled = false;
                    }
                }
            }
        }
        catch (Exception ex)
        {
            // throw ex;
        }
    }

    protected void ddlbatch_SelectedIndexChanged(object sender, EventArgs e)
    {
        GetSubject();
        panel_content.Visible = false;
        table_tree.Visible = false;
        lbl_add_row_type.Visible = false;
        ddl_addrow_type.Visible = false;
        btn_addvalue.Visible = false;
        gview.Visible = false;
        Btn_Save.Visible = false;
        Btn_Cancel.Visible = false;
        Btn_delete.Visible = false;
        Btn_AddNewRow.Visible = false;
    }

    protected void ddldegree_SelectedIndexChanged(object sender, EventArgs e)
    {
        bindbranch();
        bindsem();
        GetSubject();
        panel_content.Visible = false;
        table_tree.Visible = false;
        lbl_add_row_type.Visible = false;
        ddl_addrow_type.Visible = false;
        btn_addvalue.Visible = false;
        gview.Visible = false;
        Btn_Save.Visible = false;
        Btn_Cancel.Visible = false;
        Btn_delete.Visible = false;
        Btn_AddNewRow.Visible = false;
    }

    protected void ddlbranch_SelectedIndexChanged(object sender, EventArgs e)
    {
        bindsem();
        GetSubject();
        panel_content.Visible = false;
        table_tree.Visible = false;
        lbl_add_row_type.Visible = false;
        ddl_addrow_type.Visible = false;
        btn_addvalue.Visible = false;
        gview.Visible = false;
        Btn_Save.Visible = false;
        Btn_Cancel.Visible = false;
        Btn_delete.Visible = false;
        Btn_AddNewRow.Visible = false;

    }

    protected void ddlsem_SelectedIndexChanged(object sender, EventArgs e)
    {
        GetSubject();
        panel_content.Visible = false;
        table_tree.Visible = false;
        lbl_add_row_type.Visible = false;
        ddl_addrow_type.Visible = false;
        btn_addvalue.Visible = false;
        gview.Visible = false;
        Btn_Save.Visible = false;
        Btn_Cancel.Visible = false;
        Btn_delete.Visible = false;
        Btn_AddNewRow.Visible = false;
    }

    protected void ddlsubject_SelectedIndexChanged(object sender, EventArgs e)
    {
        panel_content.Visible = false;
        table_tree.Visible = false;
        lbl_add_row_type.Visible = false;
        ddl_addrow_type.Visible = false;
        btn_addvalue.Visible = false;
        gview.Visible = false;
        Btn_Save.Visible = false;
        Btn_Cancel.Visible = false;
        Btn_delete.Visible = false;
        Btn_AddNewRow.Visible = false;
    }

    protected void GO_Click(object sender, EventArgs e)
    {
        try
        {
            //=============================Modified by Venkat==========================
            //con.Close();
            //con.Open();

            //SqlCommand cmd_subdetails = new SqlCommand("select subject_code,exam_system,duration,First_Year_Nonsemester,semester,syllabus_master.syllabus_year,subject_type,subject_name,subject.acronym,min_ext_marks,max_ext_marks,min_int_marks,max_int_marks,mintotal,maxtotal,noofhrsperweek,degree.degree_code from subject,sub_sem,syllabus_master,degree where degree.degree_code=syllabus_master.degree_code and sub_sem.subType_no=subject.subType_no and syllabus_master.syll_code=subject.syll_code and subject_no='" + ddlsubject.SelectedValue.ToString() + "'", con);
            //SqlDataAdapter da_subdetails = new SqlDataAdapter(cmd_subdetails);
            DataSet ds_subdetails = new DataSet();
            //da_subdetails.Fill(ds_subdetails);

            string sql = "select subject_code,exam_system,duration,First_Year_Nonsemester,semester,syllabus_master.syllabus_year,subject_type,subject_name,subject.acronym,min_ext_marks,max_ext_marks,min_int_marks,max_int_marks,mintotal,maxtotal,noofhrsperweek,degree.degree_code from subject,sub_sem,syllabus_master,degree where degree.degree_code=syllabus_master.degree_code and sub_sem.subType_no=subject.subType_no and syllabus_master.syll_code=subject.syll_code and subject_no='" + ddlsubject.SelectedValue.ToString() + "'";
            ds_subdetails = dal.select_method_wo_parameter(sql, "Text");
            //lbl_subjectcode_display.Text = ddlsubject.SelectedValue.ToString();
            //===========================================================
            if (ds_subdetails.Tables[0].Rows.Count > 0)
            {
                lbl_subjectcode_display.Text = ds_subdetails.Tables[0].Rows[0]["subject_code"].ToString();
                lbl_min_intmark_display.Text = ds_subdetails.Tables[0].Rows[0]["min_int_marks"].ToString();
                lbl_max_intmark_display.Text = ds_subdetails.Tables[0].Rows[0]["max_int_marks"].ToString();
                lbl_min_extmark_display.Text = ds_subdetails.Tables[0].Rows[0]["min_ext_marks"].ToString();
                lbl_max_extmark_display.Text = ds_subdetails.Tables[0].Rows[0]["max_ext_marks"].ToString();
                lbl_total_minmark_display.Text = ds_subdetails.Tables[0].Rows[0]["mintotal"].ToString();
            }

            table_tree.Visible = true;
            newrowdiv.Visible = true;

            gview.Visible = false;
            panel_content.Visible = true;
            Btn_AddNewRow.Visible = false;
            Btn_Save.Visible = false;
            Btn_Cancel.Visible = false;
            Btn_delete.Visible = false;
            lbl_add_row_type.Visible = false;
            ddl_addrow_type.Visible = false;
            lblsavevalidate.Visible = false;

            this.TreeView1.Nodes.Clear();
            string parentnode = ddlsubject.SelectedItem.ToString();

            TreeNode tn = new TreeNode(parentnode, "0");
            TreeView1.Nodes.Add(tn);

            PopulateTreeview();
            //TreeView1.ExpandAll();
            TreeView1.Visible = true;
            bind_criteria();
        }
        catch (Exception ex)
        {
        }
    }

    protected void TreeView1_SelectedNodeChanged(object sender, EventArgs e)
    {
        try
        {
            string acdYear = string.Empty;
            string method = string.Empty;
            string medium = string.Empty;
            string coName = string.Empty;

            Btn_AddNewRow.Visible = true;
            Btn_Save.Visible = true;
            Btn_Cancel.Visible = true;
            Btn_delete.Visible = true;
            lbl_add_row_type.Visible = true;
            ddl_addrow_type.Visible = true;

            Session["nodevalue"] = TreeView1.SelectedNode.Value;
            Session["nodetext"] = TreeView1.SelectedNode.Text;

            con.Close();
            con.Open();

            bind_spread_ddl();

            dtable1.Columns.Add("Topic");
            dtable1.Columns.Add("toptag");
            dtable1.Columns.Add("Desc");
            dtable1.Columns.Add("Hours");
            dtable1.Columns.Add("ST");
            dtable1.Columns.Add("Met");
            dtable1.Columns.Add("Insmed");
            dtable1.Columns.Add("Refbook"); 
            dtable1.Columns.Add("Coursecome");

            string subj_no = ddlsubject.SelectedValue;
            string srisql = " select * from sub_unit_details  where parent_code='" + Session["nodevalue"] + "' and subject_no='" + subj_no + "'  order by topic_no";
            ds.Clear();
            ds = dal.select_method_wo_parameter(srisql, "Text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    dtrow = dtable1.NewRow();
                    dtrow["Topic"] = ds.Tables[0].Rows[i]["unit_name"].ToString();
                    dtrow["toptag"] = ds.Tables[0].Rows[i]["topic_no"].ToString();
                    dtrow["Desc"] = ds.Tables[0].Rows[i]["description"].ToString();
                    dtrow["Hours"] = ds.Tables[0].Rows[i]["noofhrs"].ToString();
                    dtrow["ST"] = ds.Tables[0].Rows[i]["steach_aid"].ToString();
                    dtrow["Met"] = ds.Tables[0].Rows[i]["methogology"].ToString();
                    dtrow["Insmed"] = ds.Tables[0].Rows[i]["instructional_media"].ToString();
                    dtrow["Refbook"] = ds.Tables[0].Rows[i]["bookref"].ToString();
                    dtrow["Coursecome"] = ds.Tables[0].Rows[i]["courseOutCome"].ToString();

                    acdYear = ds.Tables[0].Rows[i]["steach_aid"].ToString();
                    method = ds.Tables[0].Rows[i]["methogology"].ToString();
                    medium = ds.Tables[0].Rows[i]["instructional_media"].ToString();
                    coName = ds.Tables[0].Rows[i]["courseOutCome"].ToString();

                    dtable1.Rows.Add(dtrow);

                }
            }
            else
            {
                dtrow = dtable1.NewRow();
                dtrow["Topic"] = "";
                dtable1.Rows.Add(dtrow);
            }
            gview.DataSource = dtable1;

            gview.DataBind();

            DataTable dtCoS = dir.selectDataTable("select * from Master_Settings where settings='COSettings'");
            if (dtCoS.Rows.Count > 0)
            {
                foreach (GridViewRow gr in gview.Rows)
                {
                    DropDownList ddlco = (gr.FindControl("ddlcourseout") as DropDownList);
                    ddlco.Items.Clear();
                    ddlco.DataSource = dtCoS;
                    ddlco.DataTextField = "template";
                    ddlco.DataValueField = "masterno";
                    ddlco.DataBind();
                    ddlco.Items.Insert(0, " ");
                }
            }

            if (ds.Tables[0].Rows.Count > 0)
            {
                for (int k = 0; k < dtable1.Rows.Count; k++)
                {
                    acdYear = ds.Tables[0].Rows[k]["steach_aid"].ToString();
                    method = ds.Tables[0].Rows[k]["methogology"].ToString();
                    medium = ds.Tables[0].Rows[k]["instructional_media"].ToString();
                    coName = ds.Tables[0].Rows[k]["courseOutCome"].ToString();
                    DropDownList teach = (DropDownList)gview.Rows[k].FindControl("ddlteach");
                    DropDownList metho = (DropDownList)gview.Rows[k].FindControl("ddlmethod");
                    DropDownList mediu = (DropDownList)gview.Rows[k].FindControl("ddlmedium");
                    DropDownList ddlco = (DropDownList)gview.Rows[k].FindControl("ddlcourseout");
                    
                    teach.SelectedIndex = teach.Items.IndexOf(teach.Items.FindByValue(acdYear));
                    metho.SelectedIndex = metho.Items.IndexOf(metho.Items.FindByValue(method));
                    mediu.SelectedIndex = mediu.Items.IndexOf(mediu.Items.FindByValue(medium));
                    ddlco.ClearSelection();
                    ddlco.SelectedIndex = ddlco.Items.IndexOf(ddlco.Items.FindByText(coName));
                    //ddlco.Items.FindByText(coName.Trim()).Selected = true;
                }
            }
            gview.Visible = true;
        }
        catch
        {
        }
    }

    protected void TreeView1_TreeNodeCheckChanged(object sender, EventArgs e)
    {
    }

    protected void Btn_AddNewRow_Click(object sender, EventArgs e)
    {

        DataTable dt_newrow = new DataTable();
        DataRow dr;
        //dt_newrow.Columns.Add("sno");
        dt_newrow.Columns.Add("Topic");
        dt_newrow.Columns.Add("toptag");
        dt_newrow.Columns.Add("Desc");
        dt_newrow.Columns.Add("Hours");
        dt_newrow.Columns.Add("ST");
        dt_newrow.Columns.Add("Met");
        dt_newrow.Columns.Add("Insmed");
        dt_newrow.Columns.Add("Refbook");
        dt_newrow.Columns.Add("cc");



        if (ddl_addrow_type.SelectedValue == "NewRow")
        {
            for (int i = 0; i < gview.Rows.Count; i++)
            {
                dr = dt_newrow.NewRow();

                TextBox top = (TextBox)gview.Rows[i].Cells[1].FindControl("txt_Topic");
                string topic = top.Text;
                dr["Topic"] = topic;

                Label labtop = (Label)gview.Rows[i].Cells[2].FindControl("lbltag");
                string laptopi = labtop.Text;
                dr["toptag"] = laptopi;

                TextBox descp = (TextBox)gview.Rows[i].Cells[3].FindControl("txt_Desc");
                string desc = descp.Text;
                dr["Desc"] = desc;

                TextBox hours = (TextBox)gview.Rows[i].Cells[4].FindControl("txt_Hours");
                string hour = hours.Text;
                dr["Hours"] = hour;

                DropDownList tech = (DropDownList)gview.Rows[i].Cells[5].FindControl("ddlteach");
                if (tech.SelectedIndex != -1)
                {
                    string teach = tech.SelectedItem.Text;
                    dr["ST"] = teach;
                }

                DropDownList meth = (DropDownList)gview.Rows[i].Cells[6].FindControl("ddlmethod");
                if (meth.SelectedIndex != -1)
                {
                    string method = meth.SelectedItem.Text;
                    dr["met"] = method;
                }

                DropDownList med = (DropDownList)gview.Rows[i].Cells[7].FindControl("ddlmedium");
                if (med.SelectedIndex != -1)
                {
                    string medium = med.SelectedItem.Text;
                    dr["Insmed"] = medium;
                }

                TextBox refb = (TextBox)gview.Rows[i].Cells[8].FindControl("txt_Refbook");
                string refbook = refb.Text;
                dr["Refbook"] = refbook;

                DropDownList ddll = (DropDownList)gview.Rows[i].FindControl("ddlcourseout");
                if (ddll.SelectedIndex != -1)
                {
                    string ddcours = ddll.SelectedItem.Text;
                    dr["cc"] = ddcours;
                }

                dt_newrow.Rows.Add(dr);
            }
            ViewState["CurrentTable"] = dt_newrow;

            dr = dt_newrow.NewRow();

            dr["Topic"] = "";
            dr["Desc"] = "";
            dr["Hours"] = "";
            dr["ST"] = "";
            dr["met"] = "";
            dr["Insmed"] = "";
            dr["Refbook"] = "";
            dt_newrow.Rows.Add(dr);
        }
        #region command
        //else if (ddl_addrow_type.SelectedValue == "Above")
        //{
        //    for (int i = 0; i < Spread_Entry.Sheets[0].RowCount; i++)
        //    {
        //        if (Convert.ToInt32(active_row) == i)
        //        {
        //            dr = dt_newrow.NewRow();
        //            //dr["sno"] = dt_newrow.Rows.Count + 1;
        //            dr["Topic"] = "";
        //            dr["Desc"] = "";
        //            dr["Hours"] = "";
        //            dr["ST"] = "";
        //            dr["met"] = "";
        //            dr["Insmed"] = "";
        //            dr["Refbook"] = "";
        //            dt_newrow.Rows.Add(dr);
        //            //Spread_Entry.Sheets[0].DataSource = dt_newrow;                    
        //        }
        //        dr = dt_newrow.NewRow();
        //        //dr["sno"] = dt_newrow.Rows.Count + 1;
        //        dr["Topic"] = Spread_Entry.Sheets[0].Cells[i, 1].Text.ToString();
        //        dr["Desc"] = Spread_Entry.Sheets[0].Cells[i, 2].Text.ToString();
        //        dr["Hours"] = Spread_Entry.Sheets[0].Cells[i, 3].Text.ToString();
        //        dr["ST"] = Spread_Entry.Sheets[0].Cells[i, 4].Text.ToString();
        //        dr["met"] = Spread_Entry.Sheets[0].Cells[i, 5].Text.ToString();
        //        dr["Insmed"] = Spread_Entry.Sheets[0].Cells[i, 6].Text.ToString();
        //        dr["Refbook"] = Spread_Entry.Sheets[0].Cells[i, 7].Text.ToString();
        //        dt_newrow.Rows.Add(dr);
        //    }
        //}
        //else
        //{
        //    for (int i = 0; i < Spread_Entry.Sheets[0].RowCount; i++)
        //    {
        //        dr = dt_newrow.NewRow();
        //        //dr["sno"] = dt_newrow.Rows.Count + 1;
        //        dr["Topic"] = Spread_Entry.Sheets[0].Cells[i, 1].Text.ToString();
        //        dr["Desc"] = Spread_Entry.Sheets[0].Cells[i, 2].Text.ToString();
        //        dr["Hours"] = Spread_Entry.Sheets[0].Cells[i, 3].Text.ToString();
        //        dr["ST"] = Spread_Entry.Sheets[0].Cells[i, 4].Text.ToString();
        //        dr["met"] = Spread_Entry.Sheets[0].Cells[i, 5].Text.ToString();
        //        dr["Insmed"] = Spread_Entry.Sheets[0].Cells[i, 6].Text.ToString();
        //        dr["Refbook"] = Spread_Entry.Sheets[0].Cells[i, 7].Text.ToString();
        //        dt_newrow.Rows.Add(dr);

        //        if (Convert.ToInt32(active_row) == i)
        //        {
        //            dr = dt_newrow.NewRow();
        //            //dr["sno"] = dt_newrow.Rows.Count + 1;
        //            dr["Topic"] = "";
        //            dr["Desc"] = "";
        //            dr["Hours"] = "";
        //            dr["ST"] = "";
        //            dr["met"] = "";
        //            dr["Insmed"] = "";
        //            dr["Refbook"] = "";
        //            dt_newrow.Rows.Add(dr);
        //            //Spread_Entry.Sheets[0].DataSource = dt_newrow;                    
        //        }
        //    }
        //}
        #endregion

        gview.DataSource = dt_newrow;
        gview.DataBind();
        DataTable dtCoS = dir.selectDataTable("select * from Master_Settings where settings='COSettings'");
        if (dtCoS.Rows.Count > 0)
        {
            foreach (GridViewRow gr in gview.Rows)
            {
                DropDownList ddlco = (gr.FindControl("ddlcourseout") as DropDownList);
                ddlco.Items.Clear();
                ddlco.DataSource = dtCoS;
                ddlco.DataTextField = "template";
                ddlco.DataValueField = "masterno";
                ddlco.DataBind();
                ddlco.Items.Insert(0, " ");
            }
        }


        SetPreviousData();
        gview.Visible = true;
        //Spread_Entry.Height = 50 + (dt_newrow.Rows.Count * 25);
        //Spread_Entry.Sheets[0].PageSize = Spread_Entry.Sheets[0].RowCount;

        con.Close();
        con.Open();

        bind_spread_ddl();

        checkupdate = true;

    }

    public void SetPreviousData()
    {
        try
        {
            int rowIndex = 0;
            if (ViewState["CurrentTable"] != null)
            {
                DataTable dt = (DataTable)ViewState["CurrentTable"];
                Hashtable hashlist = new Hashtable();
                if (dt.Rows.Count > 0)
                {
                    DropDownList box1 = new DropDownList();
                    DropDownList box2 = new DropDownList();
                    DropDownList box3 = new DropDownList();
                    DropDownList box4 = new DropDownList();

                    //hashlist.Add(0, "Sno");
                    //hashlist.Add(1, "Batch");
                    //hashlist.Add(2, "Feecategory");

                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        box1 = (DropDownList)gview.Rows[i].Cells[4].FindControl("ddlteach");
                        box2 = (DropDownList)gview.Rows[i].Cells[5].FindControl("ddlmethod");
                        box3 = (DropDownList)gview.Rows[i].Cells[6].FindControl("ddlmedium");
                        box4 = (DropDownList)gview.Rows[i].Cells[6].FindControl("ddlcourseout");

                        //string val_file = Convert.ToString(hashlist[i]);

                        string academicyear = dt.Rows[i][3].ToString();
                        string batch = dt.Rows[i][4].ToString();
                        string feecat = dt.Rows[i][5].ToString();
                        box1.SelectedIndex = box1.Items.IndexOf(box1.Items.FindByValue(Convert.ToString(dt.Rows[i][4])));
                        box2.SelectedIndex = box2.Items.IndexOf(box2.Items.FindByValue(Convert.ToString(dt.Rows[i][5])));
                        box3.SelectedIndex = box3.Items.IndexOf(box3.Items.FindByValue(Convert.ToString(dt.Rows[i][6])));
                        box4.SelectedIndex = box4.Items.IndexOf(box4.Items.FindByText(Convert.ToString(dt.Rows[i][8])));
                        //academic.SelectedIndex = academic.Items.IndexOf(academic.Items.FindByValue(Convert.ToString(dt.Rows[i][1])));
                        rowIndex++;
                    }
                }
            }
        }
        catch (Exception)
        {
            throw;
        }
    }

    protected void Btn_Save_Click(object sender, EventArgs e)
    {
        string sub_no = string.Empty;
        string parent_code = string.Empty;
        string noofhrs = string.Empty;
        string unit_name = string.Empty;
        string validate_unitname = string.Empty;
        string topic_no = string.Empty;
        string description = string.Empty;
        string book_ref = string.Empty;
        string teach_aid = string.Empty;
        string book_details = string.Empty;
        string methedology = string.Empty;
        string instruction = string.Empty;
        string sref = string.Empty;
        string co = string.Empty;
        string value = Session["nodetext"].ToString();
        string activerow = "";
        string topicnoo = "";
        parent_code = Convert.ToString(Session["nodevalue"]);
        sub_no = ddlsubject.SelectedValue.ToString();
        string srisql = " select * from sub_unit_details  where parent_code='" + Session["nodevalue"] + "' and subject_no='" + sub_no + "'  order by topic_no";
        ds.Clear();
        ds = dal.select_method_wo_parameter(srisql, "Text");



        //  con.Close();
        //  con.Open();

        //for (int i = 0; i < Spread_Entry.Sheets[0].RowCount; i++)
        for (int i = 0; i < gview.Rows.Count; i++)
        {

            TextBox validate = (TextBox)gview.Rows[i].Cells[1].FindControl("txt_Topic");
            validate_unitname = validate.Text;
            if (validate_unitname == string.Empty)
            {
                lblsavevalidate.Text = "Please enter the data";
                lblsavevalidate.Visible = true;
                return;
            }

        }

        //topicnoo = Spread_Entry.Sheets[0].Cells[Convert.ToInt32(activerow), 1].Tag.ToString();

        for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
        {
            TextBox validate = (TextBox)gview.Rows[i].FindControl("txt_Topic");
            unit_name = validate.Text;

            TextBox des = (TextBox)gview.Rows[i].FindControl("txt_Desc");
            description = des.Text;
            TextBox hrs = (TextBox)gview.Rows[i].FindControl("txt_Hours");
            noofhrs = hrs.Text;

            Boolean val = isNumeric(noofhrs, System.Globalization.NumberStyles.Integer);
            if (val)
            {
                hrs = (TextBox)gview.Rows[i].FindControl("txt_Hours");
                noofhrs = hrs.Text;
            }
            else
            {
                noofhrs = "0";
            }

            DropDownList tech = (DropDownList)gview.Rows[i].FindControl("ddlteach");
            if (tech.SelectedIndex != -1)
            {
                teach_aid = tech.SelectedItem.Text;
            }

            DropDownList meth = (DropDownList)gview.Rows[i].FindControl("ddlmethod");
            if (meth.SelectedIndex != -1)
            {
                methedology = meth.SelectedItem.Text;
            }

            DropDownList med = (DropDownList)gview.Rows[i].FindControl("ddlmedium");
            if (med.SelectedIndex != -1)
            {
                instruction = med.SelectedItem.Text;
            }

            TextBox rbook = (TextBox)gview.Rows[i].FindControl("txt_Refbook");            
                sref = rbook.Text;

            DropDownList courseout = (DropDownList)gview.Rows[i].FindControl("ddlcourseout");
            if (courseout.SelectedIndex != -1)
            {
                co = courseout.SelectedItem.Text;
            }

            //teach_aid = Spread_Entry.Sheets[0].Cells[i, 4].Text.ToString();
            //methedology = Spread_Entry.Sheets[0].Cells[i, 5].Text.ToString();
            //instruction = Spread_Entry.Sheets[0].Cells[i, 6].Text.ToString();
            //string sref = Spread_Entry.Sheets[0].Cells[i, 7].Text.ToString();


            Label labtop = (Label)gview.Rows[i].Cells[2].FindControl("lbltag");
            topicnoo = labtop.Text;

            srisql = "if exists (select * from sub_unit_details where topic_no='" + topicnoo + "' and subject_no='" + sub_no + "' ) begin ";
            srisql = srisql + "update sub_unit_details set unit_name='" + unit_name + "', description='" + description + "' , noofhrs='" + noofhrs + "' , bookref='" + sref + "' , steach_aid='" + teach_aid + "' , methogology='" + methedology + "' , instructional_media='" + instruction + "',courseOutCome='" + co + "' where topic_no='" + topicnoo + "' and subject_no='" + sub_no + "' end  else   begin  ";

            srisql = srisql + "Insert into sub_unit_details(subject_no,parent_code,unit_name,description,noofhrs,steach_aid,methogology,instructional_media,bookref,courseOutCome)values('" + ddlsubject.SelectedValue.ToString() + "','0','" + unit_name + "','" + description + "','" + noofhrs + "','" + teach_aid + "','" + methedology + "','" + instruction + "','" + sref + "','" + co + "')  end ";

            srids.Clear();
            srids = dal.select_method_wo_parameter(srisql, "Text");


        }


        for (int i = ds.Tables[0].Rows.Count; i < gview.Rows.Count; i++)
        {
            sub_no = ddlsubject.SelectedValue.ToString();
            //  parent_code = Convert.ToString(Session["nodevalue"]);

            TextBox validate = (TextBox)gview.Rows[i].FindControl("txt_Topic");
            unit_name = validate.Text;

            TextBox des = (TextBox)gview.Rows[i].FindControl("txt_Desc");
            description = des.Text;

            TextBox hrs = (TextBox)gview.Rows[i].FindControl("txt_Hours");
            noofhrs = hrs.Text;

            Boolean val = isNumeric(noofhrs, System.Globalization.NumberStyles.Integer);
            if (val)
            {
                hrs = (TextBox)gview.Rows[i].FindControl("txt_Hours");
                noofhrs = hrs.Text;
            }
            else
            {
                noofhrs = "0";
            }

            DropDownList tech = (DropDownList)gview.Rows[i].FindControl("ddlteach");
            if(tech.SelectedIndex!=-1)
            teach_aid = tech.SelectedItem.Text;

            DropDownList meth = (DropDownList)gview.Rows[i].FindControl("ddlmethod");
            if(meth.SelectedIndex!=-1)
            methedology = meth.SelectedItem.Text;

            DropDownList med = (DropDownList)gview.Rows[i].FindControl("ddlmedium");
            if (med.SelectedIndex != -1)
            instruction = med.SelectedItem.Text;

            TextBox rbook = (TextBox)gview.Rows[i].FindControl("txt_Refbook");
            sref = rbook.Text;

            DropDownList courseout = (DropDownList)gview.Rows[i].Cells[9].FindControl("ddlcourseout");
            if (courseout.SelectedIndex != -1)
            {
                co = courseout.SelectedItem.Text;
            }

            srisql = "Insert into sub_unit_details(subject_no,parent_code,unit_name,description,noofhrs,steach_aid,methogology,instructional_media,bookref,courseOutCome)values('" + ddlsubject.SelectedValue.ToString() + "','" + parent_code + "','" + unit_name + "','" + description + "','" + noofhrs + "','" + teach_aid + "','" + methedology + "','" + instruction + "','" + sref + "','" + co + "')";
            Hashtable hat = new Hashtable();
            srids.Clear();
            //int k = dal.insert_method(srisql, hat, "Text");
            srids = dal.select_method_wo_parameter(srisql, "Text");
        }


        //panel_content.Visible = true;
        this.TreeView1.Nodes.Clear();
        string parentnode = ddlsubject.SelectedItem.ToString();
        TreeNode tn = new TreeNode(parentnode, "0");
        TreeView1.Nodes.Add(tn);

        PopulateTreeview();


        gview.Visible = false;
        Btn_AddNewRow.Visible = false;
        Btn_Save.Visible = false;
        Btn_Cancel.Visible = false;
        Btn_delete.Visible = false;
        lblsavevalidate.Visible = false;
        lbl_add_row_type.Visible = false;

        ddl_addrow_type.SelectedIndex = 0;
        ddl_addrow_type.Visible = false;
        TreeView1.Visible = true;

        ScriptManager.RegisterStartupScript(this, typeof(Page), UniqueID, "alert('Successfully Saved')", true);

    }

    protected void Btn_Cancel_Click(object sender, EventArgs e)
    {

        gview.Visible = false;
        panel_content.Visible = true;
        Btn_AddNewRow.Visible = false;
        Btn_Save.Visible = false;
        Btn_Cancel.Visible = false;
        Btn_delete.Visible = false;
        lbl_add_row_type.Visible = false;
        ddl_addrow_type.Visible = false;
    }

    protected void Btn_Delete_Click(object sender, EventArgs e)
    {
        //con.Close();=============Modified By Venkat=================
        //con.Open();

        //SqlCommand cmd_delete = new SqlCommand("delete from sub_unit_details where subject_no='" + ddlsubject.SelectedValue.ToString() + "' and topic_no='" + Session["nodevalue"].ToString()+"'",con);
        //cmd_delete.ExecuteNonQuery();

        //SqlCommand cmd_delete_1 = new SqlCommand("delete from sub_unit_details where subject_no='" + ddlsubject.SelectedValue.ToString() + "' and parent_code='" + Session["nodevalue"].ToString() + "'", con);
        //cmd_delete_1.ExecuteNonQuery();


        string cmd_delete = "delete from sub_unit_details where subject_no='" + ddlsubject.SelectedValue.ToString() + "' and topic_no='" + Session["nodevalue"].ToString() + "'";
        int a = dal.update_method_wo_parameter(cmd_delete, "Text");

        string cmd_delete_1 = "delete from sub_unit_details where subject_no='" + ddlsubject.SelectedValue.ToString() + "' and parent_code='" + Session["nodevalue"].ToString() + "'";
        int b = dal.update_method_wo_parameter(cmd_delete_1, "Text");

        this.TreeView1.Nodes.Clear();
        string parentnode = ddlsubject.SelectedItem.ToString();
        TreeNode tn = new TreeNode(parentnode, "0");
        TreeView1.Nodes.Add(tn);

        PopulateTreeview();


        gview.Visible = false;
        Btn_AddNewRow.Visible = false;
        Btn_Save.Visible = false;
        Btn_Cancel.Visible = false;
        Btn_delete.Visible = false;
        lbl_add_row_type.Visible = false;
        ddl_addrow_type.Visible = false;

        TreeView1.Visible = true;

        ScriptManager.RegisterStartupScript(this, typeof(Page), UniqueID, "alert('Successfully Deleted')", true);
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
                HierarchyTrees.HTree parentNode = hierarchyTrees.Find(delegate(HierarchyTrees.HTree emp) { return emp.topic_no == hTree.parent_code; });
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

    public bool isNumeric(string val, System.Globalization.NumberStyles NumberStyle)
    {
        Double result;
        return Double.TryParse(val, NumberStyle,
            System.Globalization.CultureInfo.CurrentCulture, out result);
    }

    protected void btn_plus_Click(object sender, EventArgs e)
    {
        panel_addvalue.Visible = true;
    }

    protected void btn_minus_Click(object sender, EventArgs e)
    {
        if (ddl_selectValue.SelectedItem.ToString() != string.Empty)
        {
            //con.Close();=======================Modified by Venkat=========================
            //con.Open();

            //SqlCommand cmd_delete_criteria = new SqlCommand("delete from textvaltable where college_code='" + Session["collegecode"].ToString() + "' and TextCriteria='" + ddl_all.SelectedValue.ToString() + "' and textval='" + ddl_selectValue.SelectedItem.ToString() + "'", con);
            //cmd_delete_criteria.ExecuteNonQuery();

            string cmd_delete_criteria = "delete from textvaltable where college_code='" + Session["collegecode"].ToString() + "' and TextCriteria='" + ddl_all.SelectedValue.ToString() + "' and textval='" + ddl_selectValue.SelectedItem.ToString() + "'";
            int a = dal.update_method_wo_parameter(cmd_delete_criteria, "Text");


            bind_criteria();
            bind_spread_ddl();

            ScriptManager.RegisterStartupScript(this, typeof(Page), UniqueID, "alert('Successfully Removed')", true);
        }
    }

    protected void btn_addvalue_Click(object sender, EventArgs e)
    {
        string textval = txt_enter_val.Text;

        if (!string.IsNullOrEmpty(textval))
        {
            //con.Close();
            //con.Open();
            //SqlCommand cmd_inserttextval = new SqlCommand("insert into textvaltable(TextVal,TextCriteria,college_code) values('"+textval+"','"+ddl_all.SelectedValue.ToString()+"','"+Session["collegecode"].ToString()+"')",con);
            //cmd_inserttextval.ExecuteNonQuery();


            string cmd_inserttextval = "insert into textvaltable(TextVal,TextCriteria,college_code) values('" + textval + "','" + ddl_all.SelectedValue.ToString() + "','" + Session["collegecode"].ToString() + "')";
            int a = dal.update_method_wo_parameter(cmd_inserttextval, "Text");

            bind_criteria();

            bind_spread_ddl();

            ScriptManager.RegisterStartupScript(this, typeof(Page), UniqueID, "alert('Successfully Saved')", true);
        }

        txt_enter_val.Text = string.Empty;
        //checkupdate = true;
        //txt_enter_val.Enabled = false;        
        //btn_addvalue.Enabled = false;
        //btnexit.Enabled = false;
    }

    protected void btnexit_Click(object sender, EventArgs e)
    {
        txt_enter_val.Text = string.Empty;
        panel_addvalue.Visible = false;
    }

    protected void ddl_all_SelectedIndexChanged(object sender, EventArgs e)
    {
        bind_criteria();
        panel_addvalue.Visible = false;
    }

    void bind_criteria()
    {
        //=========================Modified by Venkat=============================
        //con.Close();
        //con.Open();

        //SqlCommand cmd_criteria = new SqlCommand("Select textval from textvaltable where college_code='"+Session["collegecode"].ToString()+"' and TextCriteria='"+ddl_all.SelectedValue.ToString()+"'",con);
        //SqlDataAdapter da_criteria = new SqlDataAdapter(cmd_criteria);
        //DataTable dt_criteria = new DataTable();
        //da_criteria.Fill(dt_criteria);

        string query = "Select textval from textvaltable where college_code='" + Session["collegecode"].ToString() + "' and TextCriteria='" + ddl_all.SelectedValue.ToString() + "'";
        ds = dal.select_method_wo_parameter(query, "Text");
        ddl_selectValue.Items.Clear();

        ddl_selectValue.DataSource = ds;
        ddl_selectValue.DataTextField = "textval";
        ddl_selectValue.DataValueField = "textval";
        ddl_selectValue.DataBind();
        //======================================================

    }

    void bind_spread_ddl()
    {
        //================================Venkat==========================================
        //SqlCommand cmd_specialteach = new SqlCommand("Select textval from textvaltable where college_code=13 and TextCriteria='AID';Select textval from textvaltable where college_code=13 and TextCriteria='METH';Select textval from textvaltable where college_code=13 and TextCriteria='INST'", con);
        //SqlDataAdapter da_specialteach = new SqlDataAdapter(cmd_specialteach);
        //da_specialteach.Fill(ds2);


        ////string str = "Select textval from textvaltable where college_code=13 and TextCriteria='AID';Select textval from textvaltable where college_code=13 and TextCriteria='METH';Select textval from textvaltable where college_code=13 and TextCriteria='INST'";
        ////ds2 = dal.select_method_wo_parameter(str, "Text");
        //=========================================================
    }

    protected void gview_OnDataBound(object sender, EventArgs e)
    {
        try
        {

            if (gview.Rows.Count > 0)
            {
                string str = "Select textval from textvaltable where college_code=13 and TextCriteria='AID';Select textval from textvaltable where college_code=13 and TextCriteria='METH';Select textval from textvaltable where college_code=13 and TextCriteria='INST'";
                ds2 = dal.select_method_wo_parameter(str, "Text");

                for (int a = 0; a < gview.Rows.Count; a++)
                {
                    (gview.Rows[a].FindControl("ddlteach") as DropDownList).Items.Clear();
                    if (ds2.Tables.Count > 0 && ds2.Tables[0].Rows.Count > 0)
                    {
                        (gview.Rows[a].FindControl("ddlteach") as DropDownList).DataSource = ds2.Tables[0];
                        (gview.Rows[a].FindControl("ddlteach") as DropDownList).DataTextField = "textval";
                        (gview.Rows[a].FindControl("ddlteach") as DropDownList).DataBind();
                    }


                    (gview.Rows[a].FindControl("ddlmethod") as DropDownList).Items.Clear();
                    if (ds2.Tables.Count > 0 && ds2.Tables[1].Rows.Count > 0)
                    {
                        (gview.Rows[a].FindControl("ddlmethod") as DropDownList).DataSource = ds2.Tables[1];
                        (gview.Rows[a].FindControl("ddlmethod") as DropDownList).DataTextField = "textval";
                        (gview.Rows[a].FindControl("ddlmethod") as DropDownList).DataBind();
                    }


                    (gview.Rows[a].FindControl("ddlmedium") as DropDownList).Items.Clear();
                    if (ds2.Tables.Count > 0 && ds2.Tables[2].Rows.Count > 0)
                    {
                        (gview.Rows[a].FindControl("ddlmedium") as DropDownList).DataSource = ds2.Tables[2];
                        (gview.Rows[a].FindControl("ddlmedium") as DropDownList).DataTextField = "textval";
                        (gview.Rows[a].FindControl("ddlmedium") as DropDownList).DataBind();
                    }
                }
            }
        }
        catch
        {
        }
    }


    //protected void Spread_Entry_CellClick(object sender, FarPoint.Web.Spread.SpreadCommandEventArgs e)
    //{Btn_Update_Click
    //    spread_boolean = true;
    //}
    //protected void Spread_Entry_SelectedIndexChanged(Object sender, EventArgs e)
    //{
    //    if (spread_boolean == true)
    //    {
    //        int active_row = Spread_Entry.Sheets[0].ActiveRow;
    //        Session["active_row"] = active_row;
    //        spread_boolean = false;
    //    }
    //}
}
using System;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Collections;
using System.Collections.Generic;
using InsproDataAccess;
using wc = System.Web.UI.WebControls;
using System.Drawing;
using System.Text;

public partial class MatserSubjectChooser : System.Web.UI.Page
{
    Hashtable hat = new Hashtable();
    string usercode = "", collegecode = "", singleuser = "", group_user = "";
    DAccess2 d2 = new DAccess2();
    DataSet ds = new DataSet();
    InsproDirectAccess dirAcc = new InsproDirectAccess();
    DataRow dr;
    DataRow drs;
    DataTable dscount = new DataTable();
    DataTable dscounts = new DataTable();
    ArrayList rowarray = new ArrayList();
    int count = 0;

    protected void Page_Init(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            if (Session["dscount"] != null)
            {
                Session.Remove("dscount");
            }
            if (Session["arrColHdrNames2"] != null)
            {
                Session.Remove("arrColHdrNames2");
            }
        }
        callGridBind();
    }

    public void callGridBind()
    {
        //string uid = this.Page.Request.Params.Get("__EVENTTARGET");
        //if (uid != null && uid.Contains("gridLedgeDetails"))
        //{
        if (Session["dscount"] != null)
        {
            DataTable dtGrid = (DataTable)Session["dscount"];
            GridView1.DataSource = dtGrid;
            GridView1.DataBind();
            GridView1.HeaderRow.Visible = false;
        }
        else
        {
            GridView1.DataSource = null;
            GridView1.DataBind();
        }

        //}
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["collegecode"] == null)
        {
            Response.Redirect("~/Default.aspx");
        }
        //mainpanel.d
        usercode = Session["usercode"].ToString();
        collegecode = Session["collegecode"].ToString();
        singleuser = Session["single_user"].ToString();
        group_user = Session["group_code"].ToString();
        if (group_user.Contains(';'))
        {
            string[] group_semi = group_user.Split(';');
            group_user = group_semi[0].ToString();
        }
        errmsg.Visible = false;
        if (!IsPostBack)
        {
            txtRollNo.Text = string.Empty;
            txtRegNo.Text = string.Empty;
            txtAdmissionNo.Text = string.Empty;
            bindstram();
            BindBatch();
            BindDegree();
            bindbranch();
            bindsem();
            BindsubType();
            Bindsub();
            BindSection();
            //BindsubType();
            clear();

            Session["Rollflag"] = "0";
            Session["Regflag"] = "0";
            Session["Studflag"] = "0";

            string grouporusercode = "";
            if ((Session["group_code"].ToString().Trim() != "") && (Session["group_code"].ToString().Trim() != "0") && (Session["group_code"].ToString().Trim() != "-1"))
            {
                grouporusercode = " group_code=" + Session["group_code"].ToString().Trim() + "";
            }
            else
            {
                grouporusercode = " usercode=" + Session["usercode"].ToString().Trim() + "";
            }

            string strgeteleonly = d2.GetFunction("select value from Master_Settings where settings='Elective Subject only allot' and " + grouporusercode + " ");
            Session["electiveonly"] = "";
            if (strgeteleonly.Trim() == "1")
            {
                Session["electiveonly"] = "1";
            }
            string Master = "select * from Master_Settings where " + grouporusercode + "";
            DataSet ds = d2.select_method(Master, hat, "Text");
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
            loadsubtype();
            loadsubject();
            SetStudentWiseSettings();
            //ScriptManager.RegisterStartupScript(
            //            up,
            //            this.GetType(),
            //            "MyAction",
            //            "gridviewScroll();",
            //            true);
        }
    }

    public void bindstram()
    {
        try
        {
            DataSet ds = d2.select_method_wo_parameter("select distinct type from Course where isnull(type,'')<>'' and  college_code='" + collegecode + "'", "Text");
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

    public void loadsubtype()
    {
        try
        {
            string elective = "";
            if (Session["electiveonly"].ToString().Trim() == "1")
            {
                elective = "and ss.ElectivePap=1 ";
            }
            string typeval = "";
            if (ddlstream.Items.Count > 0 && ddlstream.Enabled == true)
            {
                typeval = " and c.type='" + ddlstream.SelectedItem.ToString() + "'";
            }
            string batchyear = "", semwise = "";
            if (chkbatch.Checked == true)
            {
                batchyear = " and sy.batch_year='" + ddlbatch.SelectedItem.ToString() + "'";
            }
            if (chksem.Checked == true)
            {
                semwise = " and sy.semester='" + ddlsemester.SelectedItem.ToString() + "'";
            }

            string subcate = "select distinct ss.subject_type from sub_sem ss,subject s,syllabus_master sy,Degree d,Course c where sy.syll_code=ss.syll_code and sy.syll_code=s.syll_code and ss.syll_code=s.syll_code and ss.subType_no=s.subType_no and sy.degree_code=d.Degree_Code and d.Course_Id=c.Course_Id " + typeval + " " + batchyear + " " + semwise + " " + elective + " order by ss.subject_type";
            DataSet ds = d2.select_method_wo_parameter(subcate, "Text");
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

    public void loadsubject()
    {
        try
        {
            ddlsubject.Items.Clear();
            if (ddlsubtype.Items.Count > 0)
            {
                string typeval = "";
                if (ddlstream.Items.Count > 0 && ddlstream.Enabled == true)
                {
                    typeval = " and c.type='" + ddlstream.SelectedItem.ToString() + "'";
                }
                string batchyear = "", semwise = "";
                if (chkbatch.Checked == true)
                {
                    batchyear = " and sy.batch_year='" + ddlbatch.SelectedItem.ToString() + "'";
                }
                if (chksem.Checked == true)
                {
                    semwise = " and sy.semester='" + ddlsemester.SelectedItem.ToString() + "'";
                }

                string subcate = "select distinct s.subject_name,s.subject_code,s.subject_name+' - '+s.subject_code as subnamecode from sub_sem ss,subject s,syllabus_master sy,Degree d,Course c where sy.syll_code=ss.syll_code and sy.syll_code=s.syll_code and ss.syll_code=s.syll_code and ss.subType_no=s.subType_no and sy.degree_code=d.Degree_Code and d.Course_Id=c.Course_Id " + typeval + " and ss.subject_type='" + ddlsubtype.SelectedItem.ToString() + "' " + batchyear + " " + semwise + " order by s.subject_name asc,s.subject_code desc";
                DataSet ds = d2.select_method_wo_parameter(subcate, "Text");
                if (ds.Tables[0].Rows.Count > 0)
                {
                    ddlsubject.DataSource = ds;
                    ddlsubject.DataTextField = "subnamecode";
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

    public void BindBatch()
    {
        try
        {
            ddlbatch.Items.Clear();
            //DataSet ds = d2.BindBatch();
            string Master1 = "";
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
            string strbinddegree = "select distinct batch_year from tbl_attendance_rights where user_id='" + Master1 + "' order by batch_year desc";
            DataSet ds = d2.select_method_wo_parameter(strbinddegree, "Text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                ddlbatch.DataSource = ds;
                ddlbatch.DataTextField = "Batch_year";
                ddlbatch.DataValueField = "Batch_year";
                ddlbatch.DataBind();
            }
        }
        catch (Exception ex)
        {
            errmsg.Visible = true;
            errmsg.Text = ex.ToString();
        }
    }

    public void BindDegree()
    {
        try
        {
            ddldegree.Items.Clear();
            group_user = Session["group_code"].ToString();
            if (group_user.Contains(';'))
            {
                string[] group_semi = group_user.Split(';');
                group_user = group_semi[0].ToString();
            }
            usercode = Session["usercode"].ToString();
            collegecode = Session["collegecode"].ToString();
            singleuser = Session["single_user"].ToString();

            string typeval = "";
            if (ddlstream.Items.Count > 0 && ddlstream.Enabled == true)
            {
                typeval = " and type='" + ddlstream.SelectedItem.ToString() + "'";
            }

            string strquery = "select distinct degree.course_id,course.course_name from degree,course, deptprivilages where course.course_id=degree.course_id and course.college_code = degree.college_code and degree.college_code='" + collegecode + "' and deptprivilages.Degree_code=degree.Degree_code and user_code='" + usercode + "' " + typeval + "";
            if (singleuser.Trim().ToLower() == "false" || singleuser.Trim() == "0")
            {
                strquery = "select distinct degree.course_id,course.course_name from degree,course, deptprivilages where course.course_id=degree.course_id and course.college_code = degree.college_code and degree.college_code='" + collegecode + "' and deptprivilages.Degree_code=degree.Degree_code and group_code='" + group_user + "' " + typeval + " ";
            }
            ds = d2.select_method_wo_parameter(strquery, "Text");
            int count1 = ds.Tables[0].Rows.Count;
            if (count1 > 0)
            {
                ddldegree.DataSource = ds;
                ddldegree.DataTextField = "course_name";
                ddldegree.DataValueField = "course_id";
                ddldegree.DataBind();
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
            ddlbranch.Items.Clear();
            hat.Clear();
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

            ds = d2.select_method("bind_branch", hat, "sp");
            int count2 = ds.Tables[0].Rows.Count;
            if (count2 > 0)
            {
                ddlbranch.DataSource = ds;
                ddlbranch.DataTextField = "dept_name";
                ddlbranch.DataValueField = "degree_code";
                ddlbranch.DataBind();
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
            ddlsemester.Items.Clear();
            Boolean first_year;
            first_year = false;
            int duration = 0;
            int i = 0;
            string strgetsem = "select distinct ndurations,first_year_nonsemester from ndegree where degree_code=" + ddlbranch.SelectedValue.ToString() + " and batch_year=" + ddlbatch.Text.ToString() + " and college_code=" + Session["collegecode"] + "";
            ds.Dispose();
            ds.Reset();
            ds = d2.select_method_wo_parameter(strgetsem, "Text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                first_year = Convert.ToBoolean(ds.Tables[0].Rows[0][1].ToString());
                duration = Convert.ToInt16(ds.Tables[0].Rows[0][0].ToString());
                for (i = 1; i <= duration; i++)
                {
                    if (first_year == false)
                    {
                        ddlsemester.Items.Add(i.ToString());
                    }
                    else if (first_year == true && i != 2)
                    {
                        ddlsemester.Items.Add(i.ToString());
                    }

                }
            }
            else
            {
                strgetsem = "select distinct duration,first_year_nonsemester  from degree where degree_code=" + ddlbranch.Text.ToString() + " and college_code=" + Session["collegecode"] + "";
                ddlsemester.Items.Clear();
                ds.Dispose();
                ds.Reset();
                ds = d2.select_method_wo_parameter(strgetsem, "Text");
                if (ds.Tables[0].Rows.Count > 0)
                {
                    first_year = Convert.ToBoolean(ds.Tables[0].Rows[0][1].ToString());
                    duration = Convert.ToInt16(ds.Tables[0].Rows[0][0].ToString());

                    for (i = 1; i <= duration; i++)
                    {
                        if (first_year == false)
                        {
                            ddlsemester.Items.Add(i.ToString());
                        }
                        else if (first_year == true && i != 2)
                        {
                            ddlsemester.Items.Add(i.ToString());
                        }
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

    public void BindSection()
    {
        try
        {
            ddlsection.Items.Clear();
            string strect = "select distinct sections from registration where batch_year=" + ddlbatch.SelectedValue.ToString() + " and degree_code=" + ddlbranch.SelectedValue.ToString() + " and sections<>'-1' and sections<>' ' and delflag=0 and exam_flag<>'Debar'";
            DataSet ds = d2.select_method_wo_parameter(strect, "Text");
            ddlsection.DataSource = ds;
            ddlsection.DataTextField = "sections";
            ddlsection.DataBind();
            ddlsection.Items.Insert(0, "All");
            if (ds.Tables[0].Rows.Count > 0)
            {
                if (ds.Tables[0].Rows[0]["sections"].ToString() == string.Empty)
                {
                    ddlsection.Enabled = false;
                }
                else
                {
                    ddlsection.Enabled = true;
                }
            }
            else
            {
                ddlsection.Enabled = false;
            }
        }
        catch (Exception ex)
        {
            errmsg.Visible = true;
            errmsg.Text = ex.ToString();
        }
    }

    public void BindsubType()
    {
        try
        {

            string batch = string.Empty;
            string degCode = string.Empty;
            string sem = string.Empty;
            CheckBoxList1.Items.Clear();
            txtSubType.Text = "--Select--";
            CheckBox1.Checked = false;
            if (!string.IsNullOrEmpty(ddlbatch.SelectedValue))
            {
                batch = ddlbatch.SelectedValue;
            }
            if (!string.IsNullOrEmpty(ddlbranch.SelectedValue))
            {
                degCode = ddlbranch.SelectedValue;
            }
            if (!string.IsNullOrEmpty(ddlsemester.SelectedValue))
            {
                sem = ddlsemester.SelectedValue;
            }

            string qrys = "select distinct ss.subject_type from syllabus_master sy,sub_sem ss,subject s where sy.syll_code=s.syll_code and ss.subType_no=s.subType_no and  sy.Batch_Year='" + batch + "' and sy.degree_code in('" + degCode + "') and sy.semester='" + sem + "'  order by ss.subject_type";//promote_count=1
            DataSet dsset = d2.select_method_wo_parameter(qrys, "Text");
            if (dsset.Tables.Count > 0 && dsset.Tables[0].Rows.Count > 0)
            {
                CheckBoxList1.DataSource = dsset;
                CheckBoxList1.DataTextField = "subject_type";
                CheckBoxList1.DataValueField = "subject_type";
                CheckBoxList1.DataBind();
                checkBoxListselectOrDeselect(CheckBoxList1, true);
                CallCheckboxListChange(CheckBox1, CheckBoxList1, txtSubType, lblSuType.Text, "--Select--");
            }
        }
        catch
        {
        }
    }

    public void Bindsub()
    {
        try
        {
            txtSubject.Text = "";

            string sem = ddlsemester.SelectedValue.ToString();
            string batch = ddlbatch.SelectedValue.ToString();
            string degCode = string.Empty;
            string subtype = string.Empty;
            if (CheckBoxList1.Items.Count > 0)
                subtype = getCblSelectedText(CheckBoxList1);

            degCode = ddlbranch.SelectedValue;
            cblSubject.Items.Clear();
            txtSubject.Text = "--Select--";
            cbSubjet.Checked = false;

            #region hide
            //string secval = "";
            //string secvals = "";
            //string redoSec = "";
            //if (ddlsection.Enabled == true && ddlsection.Items.Count > 0)
            //{
            //    if (ddlsection.SelectedItem.ToString() != "All")
            //    {
            //        secval = ddlsection.SelectedItem.ToString();
            //        secvals = "and sections='" + secval + "'";
            //        redoSec = " and r.sections='" + secval + "'";
            //    }
            //}
            //string strorder = "ORDER BY Roll_No";
            //string serialno = d2.GetFunction("select LinkValue from inssettings where college_code=" + Session["collegecode"].ToString() + " and linkname='Student Attendance'");
            //if (serialno.Trim() == "1")
            //{
            //    strorder = "ORDER BY serialno";
            //}
            //else
            //{
            //    string orderby_Setting = d2.GetFunction("select value from master_Settings where settings='order_by'");
            //    if (orderby_Setting == "0")
            //    {
            //        strorder = "ORDER BY Roll_No";
            //    }
            //    else if (orderby_Setting == "1")
            //    {
            //        strorder = "ORDER BY Reg_No";
            //    }
            //    else if (orderby_Setting == "2")
            //    {
            //        strorder = "ORDER BY Stud_Name";
            //    }
            //    else if (orderby_Setting == "0,1,2")
            //    {
            //        strorder = "ORDER BY Roll_No,Reg_No,Stud_Name";
            //    }
            //    else if (orderby_Setting == "0,1")
            //    {
            //        strorder = "ORDER BY Roll_No,Reg_No";
            //    }
            //    else if (orderby_Setting == "1,2")
            //    {
            //        strorder = "ORDER BY Reg_No,Stud_Name";
            //    }
            //    else if (orderby_Setting == "0,2")
            //    {
            //        strorder = "ORDER BY Roll_No,Stud_Name";
            //    }
            //}



            //string strredo = "select r.App_No,r.Reg_No,r.Roll_No,r.Stud_Name from Registration r,StudentRedoDetails s where r.App_No=s.Stud_AppNo and r.Batch_Year='" + batch + "' and r.degree_code='" + degCode + "'" + redoSec;

            //DataSet dsredo = d2.select_method_wo_parameter(strredo, "text");
            ////Aruna 15oct2018===============================================
            ////string strquery = "select Roll_No,Reg_No,Stud_Name,Stud_Type from Registration where Batch_Year='" + batchyear + "' and degree_code='" + degreecode + "' " + secvals + " and DelFlag=0 and Exam_Flag<>'debar' " + strorder + "";
            //string stud_mode = "";
            //string strquery = "";
            //if (Convert.ToString(sem) == "1" || Convert.ToString(sem) == "2") //Changed by   Aruna
            //{
            //    stud_mode = " and mode!=3"; //No need to display Lateral students
            //}
            //strquery = "select Roll_No,Reg_No,Stud_Name,Stud_Type from Registration where Batch_Year='" + batch + "' and degree_code='" + degCode + "' " + secvals + " and DelFlag=0 and Exam_Flag<>'debar' " + stud_mode + " " + strorder + " "; //Changed by   Aruna
            ////if (Convert.ToString(sem) != "") add by Aruna
            ////{
            ////    strquery = "select Roll_No,Reg_No,Stud_Name,Stud_Type from Registration where Batch_Year='" + batchyear + "' and degree_code='" + degreecode + "' " + secvals + " and DelFlag=0 and Exam_Flag<>'debar' and Current_Semester =" + sem + " " + strorder + "";
            ////}
            //ds.Dispose();
            //ds.Reset();
            //ds = d2.select_method_wo_parameter(strquery, "Text");
            #endregion


            if (!string.IsNullOrEmpty(sem) && !string.IsNullOrEmpty(degCode) && !string.IsNullOrEmpty(batch) && !string.IsNullOrEmpty(subtype))
            {
                string SelectQ = "select distinct s.subject_code,s.subject_name,CONVERT(nvarchar(max),isnull(s.subject_code,'')+'-'+isnull(s.subject_name,'')) as text from syllabus_master sy,sub_sem ss,subject s where s.syll_code=sy.syll_code and  ss.subType_no=s.subType_no and  sy.Batch_Year='" + batch + "' and sy.degree_code in('" + degCode + "') and sy.semester='" + sem + "' and ss.subject_type in(" + subtype + ") order by s.subject_code,s.subject_name,CONVERT(nvarchar(max),isnull(s.subject_code,'')+'-'+isnull(s.subject_name,''))";//and ss.promote_count=1

                //string SelectQ = "select  distinct subject.subject_code,subject.subject_name,CONVERT(nvarchar(max),isnull(subject.subject_code,'')+'-'+isnull(subject.subject_name,'')) as text from subject,sub_sem,syllabus_master,subjectchooser,registration where  subject.subtype_no = sub_sem.subtype_no and sub_sem.promote_count =1 and subject.syll_code=syllabus_master.syll_code and syllabus_master.degree_code=registration.degree_code and syllabus_master.semester =registration.current_semester and syllabus_master.batch_year=registration.batch_year  and subject.subject_no =subjectchooser.subject_no and subjectchooser.roll_no=registration.roll_no  and  registration.degree_code in('" + degCode + "') and registration.batch_year in('" + batch + "') and registration.current_semester='" + sem + "' and RollNo_Flag<>'0' and cc='0' and DelFlag<>1  and exam_flag <> 'DEBAR' and sub_sem.subject_type in(" + subtype + ") order by subject.subject_name";
                DataSet dtsubject = d2.select_method_wo_parameter(SelectQ, "Text");

                if (dtsubject.Tables.Count > 0 && dtsubject.Tables[0].Rows.Count > 0)
                {
                    cblSubject.DataSource = dtsubject;
                    cblSubject.DataTextField = "text";
                    cblSubject.DataValueField = "subject_code";
                    cblSubject.DataBind();
                    if (cblSubject.Items.Count > 0)
                    {
                        for (int i = 0; i < cblSubject.Items.Count; i++)
                        {
                            cblSubject.Items[i].Selected = true;
                        }
                        txtSubject.Text = "Subject(" + cblSubject.Items.Count + ")";
                        cbSubjet.Checked = true;
                    }
                }
            }
        }
        catch
        {
        }
    }

    public void clear()
    {
        //txtRollNo.Text = string.Empty;
        errmsg.Visible = false;
        GridView1.Visible = false;
        btnsave.Visible = false;
        chkexammrk.Visible = false;
    }

    protected void ddlstream_SelectedIndexChanged(object sender, EventArgs e)
    {
        txtRollNo.Text = string.Empty;
        txtRegNo.Text = string.Empty;
        txtAdmissionNo.Text = string.Empty;
        BindBatch();
        BindDegree();
        bindbranch();
        bindsem();
        BindSection();
        loadsubtype();
        loadsubject();
        clear();
    }

    protected void ddlbatch_SelectedIndexChanged(object sender, EventArgs e)
    {
        txtRollNo.Text = string.Empty;
        txtRegNo.Text = string.Empty;
        txtAdmissionNo.Text = string.Empty;
        BindDegree();
        bindbranch();
        bindsem();
        BindSection();
        BindsubType();
        Bindsub();
        if (chkbatch.Checked == true || chksem.Checked == true)
        {
            loadsubtype();
            loadsubject();
        }
        clear();
    }

    protected void ddldegree_SelectedIndexChanged(object sender, EventArgs e)
    {
        txtRollNo.Text = string.Empty;
        txtRegNo.Text = string.Empty;
        txtAdmissionNo.Text = string.Empty;
        bindbranch();
        bindsem();
        BindSection();
        BindsubType();
        Bindsub();
        if (chkbatch.Checked == true || chksem.Checked == true)
        {
            loadsubtype();
            loadsubject();
        }
        clear();
    }

    protected void ddlbranch_SelectedIndexChanged(object sender, EventArgs e)
    {
        txtRollNo.Text = string.Empty;
        txtRegNo.Text = string.Empty;
        txtAdmissionNo.Text = string.Empty;
        bindsem();
        BindSection();
        BindsubType();
        Bindsub();
        if (chkbatch.Checked == true || chksem.Checked == true)
        {
            loadsubtype();
            loadsubject();
        }
        clear();
    }

    protected void ddlsemester_SelectedIndexChanged(object sender, EventArgs e)
    {
        txtRollNo.Text = string.Empty;
        txtRegNo.Text = string.Empty;
        txtAdmissionNo.Text = string.Empty;
        BindSection();
        BindsubType();
        Bindsub();
        if (chkbatch.Checked == true || chksem.Checked == true)
        {
            loadsubtype();
            loadsubject();
        }
        clear();
    }

    protected void ddlsection_SelectedIndexChanged(object sender, EventArgs e)
    {
        txtRollNo.Text = string.Empty;
        txtRegNo.Text = string.Empty;
        txtAdmissionNo.Text = string.Empty;
        clear();
    }

    protected void subwisefiler(object sender, EventArgs e)
    {
        clear();
        if (chkbatch.Checked == true || chksem.Checked == true)
        {
            loadsubtype();
            loadsubject();
        }
        else
        {
            loadsubtype();
            loadsubject();
        }
    }

    protected void ddlsubtype_SelectedIndexChanged(object sender, EventArgs e)
    {
        loadsubject();
    }

    //protected void btnaddsub_Click(object sender, EventArgs e)
    //{
    //    try
    //    {
    //        string insertupdatequery = "";
    //        int insupdval = 0;
    //        string batchyear = ddlbatch.SelectedItem.ToString();
    //        string degreecode = ddlbranch.SelectedValue.ToString();
    //        string sem = ddlsemester.SelectedValue.ToString();
    //        string subjecttype = ddlsubtype.SelectedItem.ToString();
    //        if (ddlsubject.Items.Count > 0)
    //        {
    //            string subjectnam = ddlsubject.SelectedItem.ToString();
    //            string subcode = ddlsubject.SelectedValue.ToString();
    //            string[] spval = subjectnam.Split('-');
    //            string getsubname = "";
    //            for (int g = 0; g < spval.GetUpperBound(0); g++)
    //            {
    //                if (getsubname == "")
    //                {
    //                    getsubname = spval[g].ToString();
    //                }
    //                else
    //                {
    //                    getsubname = getsubname + "-" + spval[g].ToString();
    //                }
    //            }
    //            getsubname = getsubname.Trim();

    //            string sylcode = d2.GetFunction("select syll_code from syllabus_master where Batch_Year='" + batchyear + "' and degree_code='" + degreecode + "' and semester='" + sem + "'");
    //            if (sylcode.Trim() == "0" || sylcode.Trim() == "")
    //            {
    //                insertupdatequery = "if not exists (select syll_code from syllabus_master where Batch_Year='" + batchyear + "' and degree_code='" + degreecode + "' and semester='" + sem + "')";
    //                insertupdatequery = insertupdatequery + " insert into syllabus_master(Batch_Year,degree_code,semester,syllabus_year) values('" + batchyear + "','" + degreecode + "','" + sem + "','" + batchyear + "')";
    //                insupdval = d2.update_method_wo_parameter(insertupdatequery, "Text");

    //                sylcode = d2.GetFunction("select syll_code from syllabus_master where Batch_Year='" + batchyear + "' and degree_code='" + degreecode + "' and semester='" + sem + "'");
    //            }

    //            string subtypeno = d2.GetFunction("select subtype_no from sub_sem where subject_type='" + subjecttype + "' and syll_code='" + sylcode + "'");
    //            if (subtypeno.Trim() == "" || subtypeno.Trim() == "0")
    //            {
    //                string subjecttypequery = "if not exists(select subtype_no from sub_sem where subject_type='" + subjecttype + "' and syll_code='" + sylcode + "')";
    //                subjecttypequery = subjecttypequery + " insert into sub_sem (syll_code,subject_type,no_of_papers,promote_count,Lab,markOrGrade,pre_schedule) values ('" + sylcode + "','" + subjecttype + "','6','1','0','1','0')";

    //                insupdval = d2.update_method_wo_parameter(subjecttypequery, "text");

    //                subtypeno = d2.GetFunction("select subtype_no from sub_sem where subject_type='" + subjecttype + "' and syll_code='" + sylcode + "'");
    //            }

    //            string subjectcode = ddlsubject.SelectedValue.ToString();

    //            string subjectnoval = d2.GetFunction("select subject_no from subject where subject_code='" + subjectcode + "' and subType_no='" + subtypeno + "' and syll_code='" + sylcode + "'");
    //            if (subjectnoval.ToString().Trim() == "0" || subjectnoval.ToString().Trim() == "")
    //            {
    //                string subquery = "select * from subject where subject_code='" + subjectcode + "'";
    //                DataSet dssubje = d2.select_method_wo_parameter(subquery, "Text");
    //                if (dssubje.Tables[0].Rows.Count > 0)
    //                {
    //                    string subjectname = dssubje.Tables[0].Rows[0]["subject_name"].ToString();
    //                    string credit = dssubje.Tables[0].Rows[0]["credit_points"].ToString();
    //                    string paperno = dssubje.Tables[0].Rows[0]["PaperID"].ToString();
    //                    string inmin = dssubje.Tables[0].Rows[0]["min_int_marks"].ToString();
    //                    string inmax = dssubje.Tables[0].Rows[0]["max_int_marks"].ToString();
    //                    string extmin = dssubje.Tables[0].Rows[0]["min_ext_marks"].ToString();
    //                    string extmax = dssubje.Tables[0].Rows[0]["max_ext_marks"].ToString();
    //                    string mintotal = dssubje.Tables[0].Rows[0]["mintotal"].ToString();
    //                    string maxtotal = dssubje.Tables[0].Rows[0]["maxtotal"].ToString();
    //                    string curfee = dssubje.Tables[0].Rows[0]["curfee"].ToString();
    //                    string arrfee = dssubje.Tables[0].Rows[0]["arrfee"].ToString();
    //                    if (paperno.Trim() == "")
    //                    {
    //                        paperno = "0";
    //                    }
    //                    if (curfee.Trim().ToLower() == "")
    //                    {
    //                        curfee = "0";
    //                    }
    //                    if (arrfee.Trim().ToLower() == "")
    //                    {
    //                        arrfee = "0";
    //                    }

    //                    string subjectsavequery = "if not exists(select subject_no from subject where subject_code='" + subjectcode + "' and subType_no='" + subtypeno + "' and syll_code='" + sylcode + "')";
    //                    subjectsavequery = subjectsavequery + " insert into subject (subject_code,subject_name,subType_no,min_int_marks,max_int_marks,min_ext_marks,max_ext_marks,mintotal,maxtotal,credit_points,syll_code,curfee,arrfee)";
    //                    subjectsavequery = subjectsavequery + " values('" + subjectcode + "','" + subjectname + "','" + subtypeno + "','" + inmin + "','" + inmax + "','" + extmin + "','" + extmax + "','" + mintotal + "','" + maxtotal + "','" + credit + "','" + sylcode + "','" + curfee + "','" + arrfee + "')";

    //                    insupdval = d2.update_method_wo_parameter(subjectsavequery, "text");

    //                    int subjectno = Convert.ToInt32(d2.GetFunction("select subject_no from subject where subject_code='" + subjectcode + "' and subType_no='" + subtypeno + "' and syll_code='" + sylcode + "'"));

    //                    FarPoint.Web.Spread.CheckBoxCellType chk = new FarPoint.Web.Spread.CheckBoxCellType();
    //                    chk.AutoPostBack = false;

    //                    FarPoint.Web.Spread.CheckBoxCellType chk1 = new FarPoint.Web.Spread.CheckBoxCellType();
    //                    chk1.AutoPostBack = true;


    //                    FpSpread1.Sheets[0].ColumnCount++;
    //                    FpSpread1.Sheets[0].Cells[0, FpSpread1.Sheets[0].ColumnCount - 1].CellType = chk1;
    //                    FpSpread1.Sheets[0].Columns[FpSpread1.Sheets[0].ColumnCount - 1].HorizontalAlign = HorizontalAlign.Center;
    //                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Sheets[0].ColumnCount - 1].Text = subjectname + " - " + subjectcode;
    //                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Sheets[0].ColumnCount - 1].Tag = subjectno;
    //                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Sheets[0].ColumnCount - 1].Note = subtypeno;

    //                    for (int r = 1; r < FpSpread1.Sheets[0].RowCount; r++)
    //                    {
    //                        FpSpread1.Sheets[0].Cells[r, FpSpread1.Sheets[0].ColumnCount - 1].CellType = chk;
    //                    }
    //                }
    //            }
    //            else
    //            {
    //                errmsg.Visible = true;
    //                errmsg.Text = "Already Subject Exists!!!";
    //                return;
    //            }
    //        }
    //        else
    //        {
    //            errmsg.Visible = true;
    //            errmsg.Text = "No Subject Available";
    //        }
    //    }
    //    catch (Exception ex)
    //    {
    //        errmsg.Visible = true;
    //        errmsg.Text = ex.ToString();
    //    }
    //}

    //protected void btngo_Click(object sender, EventArgs e)
    //{
    //    try
    //    {
    //        DataTable dscount = new DataTable();
    //        dscount.Columns.Add("Roll.No");
    //        dscount.Columns.Add("Reg.No");
    //        dscount.Columns.Add("Student Type");
    //        dscount.Columns.Add("Student Name");
    //        DataRow dr = new DataRow();
    //        clear();
    //        FpSpread1.Sheets[0].ColumnCount = 0;
    //        FpSpread1.Sheets[0].RowCount = 0;
    //        FpSpread1.Sheets[0].SheetCorner.ColumnCount = 0;
    //        FpSpread1.CommandBar.Visible = false;

    //        FpSpread1.Sheets[0].ColumnCount = 5;
    //        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 0].Text = "S.No";
    //        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 1].Text = "Roll.No";
    //        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 2].Text = "Reg.No";
    //        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 3].Text = "Student Type";
    //        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 4].Text = "Student Name";

    //        FpSpread1.Sheets[0].Columns[0].Width = 50;
    //        FpSpread1.Sheets[0].Columns[1].Width = 100;
    //        FpSpread1.Sheets[0].Columns[2].Width = 100;
    //        FpSpread1.Sheets[0].Columns[3].Width = 100;
    //        FpSpread1.Sheets[0].Columns[4].Width = 200;

    //        FpSpread1.Sheets[0].Columns[0].HorizontalAlign = HorizontalAlign.Center;
    //        FpSpread1.Sheets[0].Columns[1].HorizontalAlign = HorizontalAlign.Left;
    //        FpSpread1.Sheets[0].Columns[2].HorizontalAlign = HorizontalAlign.Left;
    //        FpSpread1.Sheets[0].Columns[3].HorizontalAlign = HorizontalAlign.Left;
    //        FpSpread1.Sheets[0].Columns[4].HorizontalAlign = HorizontalAlign.Left;

    //        if (Session["Rollflag"].ToString() == "1")
    //        {
    //            FpSpread1.Sheets[0].Columns[1].Visible = true;
    //        }
    //        else
    //        {
    //            FpSpread1.Sheets[0].Columns[1].Visible = false;
    //        }

    //        if (Session["Regflag"].ToString() == "1")
    //        {
    //            FpSpread1.Sheets[0].Columns[2].Visible = true;
    //        }
    //        else
    //        {
    //            FpSpread1.Sheets[0].Columns[2].Visible = false;
    //        }

    //        if (Session["Studflag"].ToString() == "1")
    //        {
    //            FpSpread1.Sheets[0].Columns[3].Visible = true;
    //        }
    //        else
    //        {
    //            FpSpread1.Sheets[0].Columns[3].Visible = false;
    //        }

    //        FpSpread1.Sheets[0].ColumnHeader.DefaultStyle.Font.Name = "Book Antiqua";
    //        FpSpread1.Sheets[0].ColumnHeader.DefaultStyle.Font.Bold = true;
    //        FpSpread1.Sheets[0].ColumnHeader.DefaultStyle.Font.Size = FontUnit.Medium;
    //        FarPoint.Web.Spread.StyleInfo style = new FarPoint.Web.Spread.StyleInfo();
    //        style.Font.Size = 12;
    //        style.Font.Bold = true;
    //        style.Font.Name = "Book Antiqua";
    //        style.BackColor = FpSpread1.Sheets[0].ColumnHeader.DefaultStyle.BackColor;
    //        FpSpread1.SheetCorner.Rows.Default.Font.Size = FontUnit.Medium;
    //        FpSpread1.SheetCorner.Rows.Default.Font.Name = "Book Antiqua";
    //        FpSpread1.SheetCorner.Rows.Default.Font.Bold = true;
    //        FpSpread1.Sheets[0].SheetCornerStyle = new FarPoint.Web.Spread.StyleInfo(style);
    //        FpSpread1.Sheets[0].AllowTableCorner = true;
    //        FpSpread1.Sheets[0].Columns.Default.HorizontalAlign = HorizontalAlign.Center;
    //        FpSpread1.Sheets[0].Columns.Default.VerticalAlign = VerticalAlign.Middle;
    //        FpSpread1.VerticalScrollBarPolicy = FarPoint.Web.Spread.ScrollBarPolicy.AsNeeded;
    //        FpSpread1.HorizontalScrollBarPolicy = FarPoint.Web.Spread.ScrollBarPolicy.AsNeeded;
    //        FpSpread1.Sheets[0].Columns.Default.HorizontalAlign = HorizontalAlign.Center;
    //        FpSpread1.Sheets[0].Rows.Default.HorizontalAlign = HorizontalAlign.Center;
    //        FpSpread1.Sheets[0].Columns.Default.VerticalAlign = VerticalAlign.Middle;
    //        FpSpread1.Sheets[0].Rows.Default.VerticalAlign = VerticalAlign.Middle;
    //        FpSpread1.ActiveSheetView.SheetCorner.DefaultStyle.BackColor = FpSpread1.Sheets[0].ColumnHeader.DefaultStyle.BackColor;
    //        FpSpread1.RowHeader.Width = 50;

    //        FarPoint.Web.Spread.StyleInfo style2 = new FarPoint.Web.Spread.StyleInfo();
    //        style2.Font.Size = 13;
    //        style2.Font.Name = "Book Antiqua";
    //        style2.Font.Bold = true;
    //        style2.HorizontalAlign = HorizontalAlign.Center;
    //        style2.ForeColor = System.Drawing.Color.Black;
    //        style2.BackColor = System.Drawing.ColorTranslator.FromHtml("#0CA6CA");
    //        FpSpread1.Sheets[0].ColumnHeader.DefaultStyle = new FarPoint.Web.Spread.StyleInfo(style2);
    //        FpSpread1.Sheets[0].SheetName = " ";
    //        FpSpread1.Sheets[0].Columns.Default.VerticalAlign = VerticalAlign.Middle;
    //        FpSpread1.Sheets[0].Rows.Default.VerticalAlign = VerticalAlign.Middle;
    //        FpSpread1.Sheets[0].DefaultStyle.Font.Name = "Book Antiqua";
    //        FpSpread1.Sheets[0].DefaultStyle.Font.Size = FontUnit.Medium;
    //        FpSpread1.Sheets[0].DefaultStyle.Font.Bold = false;

    //        FpSpread1.Sheets[0].AutoPostBack = false;
    //        FpSpread1.Sheets[0].FrozenRowCount = 1;
    //        string batchyear = ddlbatch.SelectedItem.ToString();
    //        string degreecode = ddlbranch.SelectedValue.ToString();
    //        string sem = ddlsemester.SelectedValue.ToString();

    //        group_user = Session["group_code"].ToString();
    //        if (group_user.Contains(';'))
    //        {
    //            string[] group_semi = group_user.Split(';');
    //            group_user = group_semi[0].ToString();
    //        }
    //        string grouporusercode = "";
    //        if ((Session["group_code"].ToString().Trim() != "") && (Session["group_code"].ToString().Trim() != "0") && (Session["group_code"].ToString().Trim() != "-1"))
    //        {
    //            grouporusercode = " and group_code=" + Session["group_code"].ToString().Trim() + "";
    //        }
    //        else
    //        {
    //            grouporusercode = " and usercode=" + Session["usercode"].ToString().Trim() + "";
    //        }

    //        string Master1 = "select * from Master_Settings where " + grouporusercode + "";
    //        DataSet dss = GetSettings();


    //        string semlico = d2.GetFunction("select value from Master_Settings where settings='previous sem subject allotment' " + grouporusercode + "");
    //        int stusemester = Convert.ToInt32(d2.GetFunction("select distinct isnull(Current_Semester,'0') sem from Registration where Batch_Year='" + batchyear + "' and degree_code=" + degreecode + " and cc=0 and DelFlag=0 and Exam_Flag<>'debar' order by sem"));
    //        if (stusemester != Convert.ToInt32(sem) && semlico == "0")
    //        {
    //            clear();
    //            errmsg.Visible = true;
    //            errmsg.Text = "Please Update Student Semster and Student's in " + stusemester + " Semester ";
    //            return;
    //        }


    //        string secval = "";
    //        string secvals = "";
    //        string redoSec = "";
    //        if (ddlsection.Enabled == true && ddlsection.Items.Count > 0)
    //        {
    //            if (ddlsection.SelectedItem.ToString() != "All")
    //            {
    //                secval = ddlsection.SelectedItem.ToString();
    //                secvals = "and sections='" + secval + "'";
    //                redoSec = " and r.sections='" + secval + "'";
    //            }
    //        }

    //        string strorder = "ORDER BY Roll_No";
    //        string serialno = d2.GetFunction("select LinkValue from inssettings where college_code=" + Session["collegecode"].ToString() + " and linkname='Student Attendance'");
    //        if (serialno.Trim() == "1")
    //        {
    //            strorder = "ORDER BY serialno";
    //        }
    //        else
    //        {
    //            string orderby_Setting = d2.GetFunction("select value from master_Settings where settings='order_by'");
    //            if (orderby_Setting == "0")
    //            {
    //                strorder = "ORDER BY Roll_No";
    //            }
    //            else if (orderby_Setting == "1")
    //            {
    //                strorder = "ORDER BY Reg_No";
    //            }
    //            else if (orderby_Setting == "2")
    //            {
    //                strorder = "ORDER BY Stud_Name";
    //            }
    //            else if (orderby_Setting == "0,1,2")
    //            {
    //                strorder = "ORDER BY Roll_No,Reg_No,Stud_Name";
    //            }
    //            else if (orderby_Setting == "0,1")
    //            {
    //                strorder = "ORDER BY Roll_No,Reg_No";
    //            }
    //            else if (orderby_Setting == "1,2")
    //            {
    //                strorder = "ORDER BY Reg_No,Stud_Name";
    //            }
    //            else if (orderby_Setting == "0,2")
    //            {
    //                strorder = "ORDER BY Roll_No,Stud_Name";
    //            }
    //        }

    //        string strredo = "select r.App_No,r.Reg_No,r.Roll_No,r.Stud_Name from Registration r,StudentRedoDetails s where r.App_No=s.Stud_AppNo and r.Batch_Year='" + batchyear + "' and r.degree_code='" + degreecode + "'" + redoSec;

    //        DataSet dsredo = d2.select_method_wo_parameter(strredo, "text");
    //        string strquery = "select Roll_No,Reg_No,Stud_Name,Stud_Type from Registration where Batch_Year='" + batchyear + "' and degree_code='" + degreecode + "' " + secvals + " and DelFlag=0 and Exam_Flag<>'debar' " + strorder + "";
    //        ds.Dispose();
    //        ds.Reset();
    //        ds = d2.select_method_wo_parameter(strquery, "Text");
    //        FpSpread1.Sheets[0].RowCount++;
    //        FpSpread1.Sheets[0].SpanModel.Add(FpSpread1.Sheets[0].RowCount - 1, 0, 1, 5);
    //        int srno = 0;


    //        #region modified for adding roll no filter

    //        string[] Rollnoarr = new string[0];
    //        string Rollqry = string.Empty;
    //        string searchValue = string.Empty;
    //        string searchField = string.Empty;
    //        string qrySearchValue = string.Empty;
    //        string selectedText = string.Empty;
    //        if (ddlSearchBy.Items.Count > 0)
    //        {
    //            string selectedValue = Convert.ToString(ddlSearchBy.SelectedValue).Trim();
    //            selectedText = Convert.ToString(ddlSearchBy.SelectedItem.Text).Trim();
    //            switch (selectedValue)
    //            {
    //                case "1":
    //                    searchValue = txtAdmissionNo.Text.Trim();
    //                    searchField = " Roll_Admit ";
    //                    break;
    //                case "2":
    //                    searchValue = txtRegNo.Text.Trim();
    //                    searchField = " Reg_No ";
    //                    break;
    //                case "3":
    //                    searchValue = txtRollNo.Text.Trim();
    //                    searchField = " Roll_no ";
    //                    break;
    //            }
    //            if (!string.IsNullOrEmpty(searchValue) && !string.IsNullOrEmpty(searchField))
    //            {
    //                Rollnoarr = searchValue.Split(',');
    //                Rollqry = string.Empty;

    //                if (Rollnoarr.Length > 0)
    //                {
    //                    for (int i = 0; i < Rollnoarr.Length; i++)
    //                    {
    //                        Rollqry += "'" + Rollnoarr[i] + "',";
    //                    }
    //                    qrySearchValue = " and " + searchField + " in(" + Rollqry.Trim(',') + ")";
    //                }
    //                else
    //                {
    //                    errmsg.Text = " Enter the Valid Student Roll No";
    //                    errmsg.Visible = true;
    //                }
    //            }
    //            else
    //            {
    //                qrySearchValue = "";
    //            }
    //        }

    //        if (Rollnoarr.Length > 0 && Rollnoarr.Length < Rollqry.Length)
    //        {
    //            strquery = "select Roll_No,Reg_No,Stud_Name,Stud_Type from Registration where Batch_Year='" + batchyear + "' and degree_code='" + degreecode + "' " + secvals + " and DelFlag=0 and Exam_Flag<>'debar' " + qrySearchValue + "  " + strorder + "";
    //            ds.Dispose();
    //            ds.Reset();
    //            ds = d2.select_method_wo_parameter(strquery, "Text");

    //            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count == 0)
    //            {
    //                errmsg.Visible = true;
    //                errmsg.Text = selectedText + " : " + Rollqry.Replace("'", "").Trim(',') + " Not Belong to " + batchyear + "-" + Convert.ToString(ddldegree.SelectedItem.Text).Trim() + "-" + Convert.ToString(ddlbranch.SelectedItem.Text).Trim() + (!string.IsNullOrEmpty(secval) ? "-" + secval : "");
    //                FpSpread1.Visible = false;
    //                return;
    //            }
    //        }

    //        #endregion


    //        FarPoint.Web.Spread.TextCellType txt = new FarPoint.Web.Spread.TextCellType();
    //        if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
    //        {
    //            chkexammrk.Visible = true;
    //            FpSpread1.Visible = true;
    //            btnsave.Visible = true;
    //            bool isredo = false;
    //            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
    //            {

    //                isredo = false;
    //                string rollno = ds.Tables[0].Rows[i]["Roll_No"].ToString();
    //                if (dsredo.Tables.Count > 0 && dsredo.Tables[0].Rows.Count > 0)
    //                {
    //                    dsredo.Tables[0].DefaultView.RowFilter = "roll_no='" + rollno + "'";
    //                    DataTable dtisRedo = dsredo.Tables[0].DefaultView.ToTable();
    //                    if (dtisRedo.Rows.Count > 0)
    //                        isredo = true;
    //                }
    //                if (!isredo)
    //                {
    //                    srno++;
    //                    FpSpread1.Sheets[0].RowCount++;
    //                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Text = srno.ToString();
    //                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].CellType = txt;
    //                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Locked = true;
    //                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
    //                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Font.Name = "Book Antiqua";
    //                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Font.Size = FontUnit.Medium;
    //                    #region magesh

    //                    dr["Reg.No"] = ds.Tables[0].Rows[i]["Reg_No"].ToString();
    //                    dr["Student Type"] = ds.Tables[0].Rows[i]["Stud_Type"].ToString();
    //                    dr["Student Name"] = ds.Tables[0].Rows[i]["Stud_Name"].ToString();
    //                    #endregion
    //                    //dr["StudentName"] = ds.Tables[0].Rows[count]["STUD NAME"].ToString();
    //                    //dr["Total_Absent_Hours"] = perabsenthrs1.ToString();
    //                    string regno = ds.Tables[0].Rows[i]["Reg_No"].ToString();
    //                    string stype = ds.Tables[0].Rows[i]["Stud_Type"].ToString();
    //                    string sname = ds.Tables[0].Rows[i]["Stud_Name"].ToString();
    //                    if ((srno % 2) == 0)
    //                    {
    //                        FpSpread1.Sheets[0].Rows[FpSpread1.Sheets[0].RowCount - 1].BackColor = System.Drawing.Color.LightGray;
    //                    }

    //                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].CellType = txt;
    //                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].Text = rollno;
    //                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].Locked = true;
    //                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].Font.Name = "Book Antiqua";
    //                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].Font.Size = FontUnit.Medium;

    //                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].CellType = txt;
    //                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].Text = regno;
    //                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].Locked = true;
    //                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].Font.Name = "Book Antiqua";
    //                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].Font.Size = FontUnit.Medium;

    //                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 3].CellType = txt;
    //                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 3].Text = stype;
    //                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 3].Locked = true;
    //                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 3].Font.Name = "Book Antiqua";
    //                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 3].Font.Size = FontUnit.Medium;

    //                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 4].CellType = txt;
    //                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 4].Text = sname;
    //                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 4].Locked = true;
    //                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 4].Font.Name = "Book Antiqua";
    //                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 4].Font.Size = FontUnit.Medium;
    //                }
    //            }

    //            FarPoint.Web.Spread.CheckBoxCellType chk = new FarPoint.Web.Spread.CheckBoxCellType();
    //            chk.AutoPostBack = false;

    //            FarPoint.Web.Spread.CheckBoxCellType chk1 = new FarPoint.Web.Spread.CheckBoxCellType();
    //            chk1.AutoPostBack = true;



    //            string strstusub = "select r.Roll_No,r.Reg_No,sc.subject_no  from Registration r,subjectChooser sc where r.Roll_No=sc.roll_no and r.Batch_Year='" + batchyear + "' and r.degree_code='" + degreecode + "' and sc.semester='" + sem + "' " + secvals + " and r.DelFlag=0 and r.Exam_Flag<>'debar'";
    //            DataSet dsstusub = d2.select_method_wo_parameter(strstusub, "Text");

    //            string strgetsubjectdetails = "select ss.subject_type,ss.subType_no,s.subject_name,s.subject_code,s.subject_no,ss.electivepap from syllabus_master sy,sub_sem ss,subject s where sy.syll_code=ss.syll_code and sy.syll_code=s.syll_code and ss.syll_code=s.syll_code and ss.subType_no=s.subType_no and sy.Batch_Year='" + batchyear + "' and sy.degree_code='" + degreecode + "' and sy.semester='" + sem + "' order by ss.subject_type,s.subject_name,s.subject_code";
    //            DataSet dssubj = d2.select_method_wo_parameter(strgetsubjectdetails, "Text");
    //            if (dssubj.Tables.Count > 0 && dssubj.Tables[0].Rows.Count > 0)
    //            {
    //                for (int s = 0; s < dssubj.Tables[0].Rows.Count; s++)
    //                {
    //                    dscount.Columns.Add(Convert.ToString(dssubj.Tables[0].Rows[s]["subject_name"]) + "-" + Convert.ToString(dssubj.Tables[0].Rows[s]["subject_code"]));
    //                    dscount.NewRow();
    //                    dscount.Columns.Add("Reg.No");
    //                    dscount.Columns.Add("Student Type");
    //                    dscount.Columns.Add("Student Name");
    //                    string subjectname = dssubj.Tables[0].Rows[s]["subject_name"].ToString();
    //                    string scode = dssubj.Tables[0].Rows[s]["subject_code"].ToString();
    //                    string subno = dssubj.Tables[0].Rows[s]["subject_no"].ToString();
    //                    string subtyno = dssubj.Tables[0].Rows[s]["subType_no"].ToString();
    //                    string elective = dssubj.Tables[0].Rows[s]["electivepap"].ToString();
    //                    FpSpread1.Sheets[0].ColumnCount++;
    //                    FpSpread1.Sheets[0].Cells[0, FpSpread1.Sheets[0].ColumnCount - 1].CellType = chk1;
    //                    FpSpread1.Sheets[0].Columns[FpSpread1.Sheets[0].ColumnCount - 1].HorizontalAlign = HorizontalAlign.Center;
    //                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Sheets[0].ColumnCount - 1].Text = subjectname + " - " + scode;
    //                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Sheets[0].ColumnCount - 1].Tag = subno;
    //                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Sheets[0].ColumnCount - 1].Note = subtyno;

    //                    if (Session["electiveonly"].ToString().Trim() == "1")
    //                    {
    //                        if (elective.Trim() == "1" || elective.Trim().ToLower() == "true")
    //                        {
    //                            FpSpread1.Sheets[0].Columns[FpSpread1.Sheets[0].ColumnCount - 1].Visible = true;
    //                        }
    //                        else
    //                        {
    //                            FpSpread1.Sheets[0].Columns[FpSpread1.Sheets[0].ColumnCount - 1].Visible = false;
    //                        }
    //                    }

    //                    for (int r = 1; r < FpSpread1.Sheets[0].RowCount; r++)
    //                    {
    //                        FpSpread1.Sheets[0].Cells[r, FpSpread1.Sheets[0].ColumnCount - 1].CellType = chk;
    //                        string rollno = FpSpread1.Sheets[0].Cells[r, 1].Text.ToString();
    //                        dsstusub.Tables[0].DefaultView.RowFilter = "Roll_No='" + rollno + "' and subject_no='" + subno + "'";
    //                        DataView dvstusub = dsstusub.Tables[0].DefaultView;
    //                        if (dvstusub.Count > 0)
    //                        {
    //                            FpSpread1.Sheets[0].Cells[r, FpSpread1.Sheets[0].ColumnCount - 1].Value = 1;
    //                        }
    //                        else
    //                        {
    //                            FpSpread1.Sheets[0].Cells[r, FpSpread1.Sheets[0].ColumnCount - 1].Value = 0;
    //                        }

    //                    }
    //                }
    //            }

    //        }
    //        else
    //        {
    //            errmsg.Visible = true;
    //            errmsg.Text = " No Records Found";
    //            FpSpread1.Visible = false;
    //        }
    //        FpSpread1.Sheets[0].FrozenColumnCount = 5;
    //        FpSpread1.Sheets[0].AutoPostBack = false;
    //        FpSpread1.SaveChanges();
    //        FpSpread1.Height = 600;
    //        FpSpread1.Width = 980;
    //        FpSpread1.Sheets[0].PageSize = FpSpread1.Sheets[0].RowCount;
    //    }
    //    catch (Exception ex)
    //    {
    //        errmsg.Visible = true;
    //        errmsg.Text = ex.ToString();
    //    }
    //}

    //protected void FpSpread1_UpdateCommand(object sender, FarPoint.Web.Spread.SpreadCommandEventArgs e)
    //{
    //    try
    //    {

    //        string spread = "";
    //        Control control = null;
    //        string ctrlname = Page.Request.Params["__EVENTTARGET"];
    //        if (ctrlname != null && ctrlname != String.Empty)
    //        {
    //            string[] spiltspreadname = ctrlname.Split('$');
    //            if (spiltspreadname.GetUpperBound(0) > 1)
    //            {
    //                string getrowxol = spiltspreadname[3].ToString().Trim();
    //                string[] spr = getrowxol.Split(',');
    //                if (spr.GetUpperBound(0) == 1)
    //                {
    //                    int arow = Convert.ToInt32(spr[0]);
    //                    int acol = Convert.ToInt32(spr[1]);
    //                    if (arow == 0 && acol > 4)
    //                    {
    //                        string setval = e.EditValues[acol].ToString();
    //                        int setvalcel = 0;
    //                        if (setval.Trim().ToLower() == "true" || setval.Trim() == "1")
    //                        {
    //                            setvalcel = 1;
    //                        }
    //                        for (int r = 1; r < FpSpread1.Sheets[0].RowCount; r++)
    //                        {
    //                            FpSpread1.Sheets[0].Cells[r, acol].Value = setvalcel;
    //                        }
    //                    }
    //                }
    //            }
    //        }
    //    }
    //    catch (Exception ex)
    //    {
    //        errmsg.Visible = true;
    //        errmsg.Text = ex.ToString();
    //    }
    //}

    protected void btnsave_Click(object sender, EventArgs e)
    {
        try
        {
            //int m = 0;
            //foreach (GridViewRow row in GridView1.Rows)
            //{
            //    m++;

            //    if (m < dscount.Rows.Count)
            //    {
            //        for (int c = 5; c < dscount.Columns.Count; c++)
            //        {
            //            string chkname = "chkb" + c + "";
            //            CheckBox stud_rollno = (CheckBox)row.FindControl(chkname);
            //            stud_rollno = (GridView1.Rows[m].Cells[4].FindControl(chkname) as CheckBox);
            //            stud_rollno = (GridView1.Rows[m].FindControl(chkname) as CheckBox);
            //            CheckBox chkRow = (row.Cells[4].FindControl(chkname) as CheckBox);
            //            Boolean sm = chkRow.Checked;
            //            if ((GridView1.Rows[m].Cells[4].FindControl(chkname) as CheckBox).Checked == false)
            //            {
            //            }
            //        }
            //    }
            //}
            //addchk();
            string batchyear = ddlbatch.SelectedItem.ToString();
            string degreecode = ddlbranch.SelectedValue.ToString();
            string sem = ddlsemester.SelectedValue.ToString();

            string secvals = "";
            if (ddlsection.Enabled == true && ddlsection.Items.Count > 0)
            {
                if (ddlsection.SelectedItem.ToString() != "All")
                {
                    secvals = "and r.sections='" + ddlsection.SelectedItem.ToString() + "'";
                }
            }
            string strstusub = "select distinct sc.batch from Registration r,subjectChooser sc where r.Roll_No=sc.roll_no and r.Batch_Year='" + batchyear + "' and r.degree_code='" + degreecode + "' and sc.semester='" + sem + "' " + secvals + " and isnull(sc.batch,'')<>'' and r.cc=0 and r.DelFlag=0 and r.Exam_Flag<>'debar'";
            DataSet dsstusub = d2.select_method_wo_parameter(strstusub, "Text");
            if (dsstusub.Tables[0].Rows.Count > 0)
            {
                mpesave.Show();
            }
            else
            {
                savefunction();
                btngo_Click(sender, e);
            }
        }
        catch (Exception ex)
        {
            errmsg.Visible = true;
            errmsg.Text = ex.ToString();
        }
    }

    //public void savefunction()
    //{
    //    try
    //    {
    //        int savevalue = 0;
    //        Boolean saveflag = false;
    //        string insertupdatequery = "";
    //        int insupdval = 0;
    //        string batchyear = ddlbatch.SelectedItem.ToString();
    //        string degreecode = ddlbranch.SelectedValue.ToString();
    //        string sem = ddlsemester.SelectedValue.ToString();

    //        string secval = "";
    //        string secvals = "";
    //        if (ddlsection.Enabled == true && ddlsection.Items.Count > 0)
    //        {
    //            if (ddlsection.SelectedItem.ToString() != "All")
    //            {
    //                secval = ddlsection.SelectedItem.ToString();
    //                secvals = "and sections='" + secval + "'";
    //            }
    //        }

    //        string deletequery = "select r.Roll_No,r.Reg_No,sc.subject_no,sc.subtype_no,sc.semester from subjectchooser sc,Registration r where sc.roll_no=r.roll_no and r.batch_year='" + batchyear + "' and r.degree_code='" + degreecode + "' and sc.semester='" + sem + "' " + secvals + "";
    //        DataSet dssnu = d2.select_method_wo_parameter(deletequery, "Text");
    //        if (dssnu.Tables[0].Rows.Count > 0)
    //        {
    //            savevalue = 2;
    //        }
    //        Hashtable ht = new Hashtable();
    //        FpSpread1.SaveChanges();

    //        for (int r = 1; r < FpSpread1.Sheets[0].RowCount; r++)
    //        {
    //            string rollno = FpSpread1.Sheets[0].Cells[r, 1].Text.ToString();

    //            Hashtable hatsubroll = new Hashtable();
    //            for (int c = 5; c < FpSpread1.Sheets[0].ColumnCount; c++)
    //            {
    //                if (FpSpread1.Sheets[0].Columns[c].Visible == true)
    //                {
    //                    string subno = FpSpread1.Sheets[0].ColumnHeader.Cells[0, c].Tag.ToString();
    //                    string subtypeno = FpSpread1.Sheets[0].ColumnHeader.Cells[0, c].Note.ToString();
    //                    dssnu.Tables[0].DefaultView.RowFilter = " roll_no='" + rollno + "' and subject_no='" + subno + "'";
    //                    DataView dvsub = dssnu.Tables[0].DefaultView;

    //                    saveflag = true;
    //                    int stva = Convert.ToInt32(FpSpread1.Sheets[0].Cells[r, c].Value);
    //                    if (stva == 1)
    //                    {
    //                        int paper = 0;
    //                        if (hatsubroll.Contains(rollno.Trim().ToLower() + '-' + subtypeno))
    //                        {
    //                            hatsubroll[rollno.Trim().ToLower() + '-' + subtypeno] = Convert.ToInt32(hatsubroll[rollno.Trim().ToLower() + '-' + subtypeno]) + 1;
    //                        }
    //                        else
    //                        {
    //                            hatsubroll.Add(rollno.Trim().ToLower() + '-' + subtypeno, 1);
    //                        }
    //                        if (dvsub.Count == 0)
    //                        {
    //                            paper = Convert.ToInt32(hatsubroll[rollno.Trim().ToLower() + '-' + subtypeno]);
    //                            ht.Clear();
    //                            ht.Add("roll_no", rollno);
    //                            ht.Add("semester", sem.ToString());
    //                            ht.Add("subject_no", subno.ToString());
    //                            ht.Add("subtype_no", subtypeno.ToString());
    //                            ht.Add("paper_order", paper.ToString());
    //                            ht.Add("batch", "");
    //                            ht.Add("grp_cell", 0);
    //                            insupdval = d2.insert_method("sp_ins_upd_Subjectchooser", ht, "sp");
    //                            insertupdatequery = " if not exists (select * from subjectChooser where roll_no='" + rollno + "' and subject_no='" + subno + "' and semester='" + sem + "')";
    //                            insertupdatequery = insertupdatequery + " insert into subjectChooser(roll_no,semester,subtype_no,subject_no,paper_order) values('" + rollno + "','" + sem + "','" + subtypeno + "','" + subno + "','1')";
    //                            insupdval = d2.update_method_wo_parameter(insertupdatequery, "Text");
    //                        }
    //                    }
    //                    else
    //                    {
    //                        if (dvsub.Count > 0)
    //                        {
    //                            ht.Clear();
    //                            ht.Add("roll_no", rollno);
    //                            ht.Add("semester", sem.ToString());
    //                            ht.Add("subject_no", subno.ToString());
    //                            insupdval = d2.insert_method("sp_delete_Subjectchooser", ht, "sp");
    //                            if (chkexammrk.Checked == true)
    //                            {
    //                                insertupdatequery = " delete from mark_entry where roll_no='" + rollno + "' and subject_no='" + subno + "'";
    //                                insupdval = d2.update_method_wo_parameter(insertupdatequery, "Text");
    //                            }
    //                        }
    //                    }
    //                }
    //            }
    //        }

    //        string sylcode = d2.GetFunction("select syll_code from syllabus_master where Batch_Year='" + batchyear + "' and degree_code='" + degreecode + "' and semester='" + sem + "'");
    //        insertupdatequery = " delete subject where syll_code='" + sylcode + "' and subject_no not in(select subject_no from subjectChooser )";
    //        insupdval = d2.update_method_wo_parameter(insertupdatequery, "Text");

    //        insertupdatequery = " delete from sub_sem where syll_code='" + sylcode + "' and subType_no not in(select subType_no from subject)";
    //        insupdval = d2.update_method_wo_parameter(insertupdatequery, "Text");

    //        if (saveflag == true)
    //        {
    //            ScriptManager.RegisterStartupScript(this, typeof(Page), UniqueID, "alert('saved successfully')", true);
    //            divPopAlert.Visible = true;
    //            lblAlertMsg.Text = "Saved Successfully";

    //            string entrycode = Session["Entry_Code"].ToString();
    //            string formname = "Matser Student Subject Allotment";
    //            string toa = DateTime.Now.ToString("MM/dd/yyy hh:mm:ss tt");
    //            string doa = DateTime.Now.ToString("MM/dd/yyy");
    //            string section = "";
    //            if (ddlsection.SelectedValue.ToString() != "" && ddlsection.SelectedValue.ToString() != "All" && ddlsection.SelectedValue.ToString() != null && ddlsection.SelectedValue.ToString() != "0")
    //            {
    //                section = ":Sections -" + ddlsection.SelectedValue.ToString();
    //            }
    //            string details = "" + ddlbranch.SelectedValue.ToString() + ":Sem - " + ddlsemester.SelectedValue.ToString() + ":Batch Year -" + ddlbatch.SelectedValue.ToString() + " " + section + "";
    //            string modules = "0";
    //            string act_diff = " ";
    //            string ctsname = "Update The Student Subject Allotment";
    //            if (savevalue == 1)
    //            {
    //                ctsname = "Save the Student Subject Allotment";
    //            }

    //            string strlogdetails = "insert into UserLog (Entry_Code,Form_Name,UsrAction,TOA,DOA,Details,Module,Act_Diff,ctrNam) values ('" + entrycode + "','" + formname + "','" + savevalue + "','" + toa + "','" + doa + "','" + details + "','" + modules + "','" + act_diff + "','" + ctsname + "')";
    //            int a = d2.update_method_wo_parameter(strlogdetails, "Text");
    //        }
    //    }
    //    catch (Exception ex)
    //    {
    //        errmsg.Visible = true;
    //        errmsg.Text = ex.ToString();
    //    }
    //}

    //public void btnsaveok_Click(object sender, EventArgs e)
    //{

    //    savefunction();
    //    btngo_Click(sender, e);
    //}

    protected void btnsaveCancel_Click(object sender, EventArgs e)
    {
        mpesave.Hide();
    }

    #region modified on 16 Nov 2017

    string rollorReg = string.Empty;

    [System.Web.Services.WebMethod]
    [System.Web.Script.Services.ScriptMethod()]
    public static List<string> GetRollNo(string prefixText, string contextKey)
    {
        WebService ws = new WebService();
        List<string> name = new List<string>();
        string query = string.Empty;

        string[] values = contextKey.Split('-');
        string degree = values[0];
        string branch = values[2];
        string section = values[3];
        string semester = values[4];
        string batchyr = values[1];

        if (section.Trim().ToLower() == "all")
        {
            query = "select Roll_No from Registration r where  Roll_No Like '" + prefixText + "%' and Batch_Year='" + batchyr + "' and  DelFlag=0 and Exam_Flag <>'Debar' and Current_Semester='" + semester + "' and degree_code='" + branch + "' order by Roll_No";
        }
        else
        {
            query = "select Roll_No from Registration r where  Roll_No Like '" + prefixText + "%' and Batch_Year='" + batchyr + "' and  DelFlag=0 and Exam_Flag <>'Debar' and Current_Semester='" + semester + "' and degree_code='" + branch + "' and Sections='" + section + "' order by Roll_No";
        }

        name = ws.Getname(query);
        return name;
    }

    [System.Web.Services.WebMethod]
    [System.Web.Script.Services.ScriptMethod()]
    public static List<string> GetRegNo(string prefixText, string contextKey)
    {
        WebService ws = new WebService();
        List<string> name = new List<string>();
        string query = string.Empty;
        //query = "select Reg_No from Registration where DelFlag=0 and Exam_Flag <>'Debar' and Reg_No Like '" + prefixText + "%'   order by Reg_No";

        string[] values = contextKey.Split('-');
        string degree = values[0];
        string branch = values[2];
        string section = values[3];
        string semester = values[4];
        string batchyr = values[1];

        if (section.Trim().ToLower() == "all")
        {
            query = "select Reg_No from Registration r where  Reg_No Like '" + prefixText + "%' and Batch_Year='" + batchyr + "' and  DelFlag=0 and Exam_Flag <>'Debar' and Current_Semester='" + semester + "' and degree_code='" + branch + "' order by Reg_No";
        }
        else
        {
            query = "select Reg_No from Registration r where  Reg_No Like '" + prefixText + "%' and Batch_Year='" + batchyr + "' and  DelFlag=0 and Exam_Flag <>'Debar' and Current_Semester='" + semester + "' and degree_code='" + branch + "' and Sections='" + section + "' order by Reg_No";
        }

        name = ws.Getname(query);
        return name;
    }

    [System.Web.Services.WebMethod]
    [System.Web.Script.Services.ScriptMethod()]
    public static List<string> GetAdmitNo(string prefixText, string contextKey)
    {
        WebService ws = new WebService();
        List<string> name = new List<string>();
        string query = string.Empty;
        //query = "select Roll_Admit from Registration where DelFlag=0 and Exam_Flag <>'Debar' and Roll_Admit Like '" + prefixText + "%'   order by Roll_Admit";

        string[] values = contextKey.Split('-');
        string degree = values[0];
        string branch = values[2];
        string section = values[3];
        string semester = values[4];
        string batchyr = values[1];

        if (section.Trim().ToLower() == "all")
        {
            query = "select Roll_Admit from Registration r where  Roll_Admit Like '" + prefixText + "%' and Batch_Year='" + batchyr + "' and  DelFlag=0 and Exam_Flag <>'Debar' and Current_Semester='" + semester + "' and degree_code='" + branch + "' order by Roll_Admit";
        }
        else
        {
            query = "select Roll_Admit from Registration r where  Roll_Admit Like '" + prefixText + "%' and Batch_Year='" + batchyr + "' and  DelFlag=0 and Exam_Flag <>'Debar' and Current_Semester='" + semester + "' and degree_code='" + branch + "' and Sections='" + section + "' order by Roll_Admit";
        }

        name = ws.Getname(query);
        return name;
    }

    private DataSet GetSettings()
    {
        DataSet dsSettings = new DataSet();
        Hashtable ht = new Hashtable();

        try
        {
            string grouporusercode = string.Empty;
            if (Session["group_code"] != null && (Convert.ToString(Session["group_code"]).Trim() != "") && (Convert.ToString(Session["group_code"]).Trim() != "0") && (Convert.ToString(Session["group_code"]).Trim() != "-1"))
            {
                string groupCode = Convert.ToString(Session["group_code"]).Trim();
                string[] groupUser = Convert.ToString(groupCode).Trim().Split(';');
                if (groupUser.Length > 0)
                {
                    groupCode = groupUser[0].Trim();
                }
                if (!string.IsNullOrEmpty(groupCode.Trim()))
                {
                    grouporusercode = " and  group_code=" + Convert.ToString(groupCode).Trim() + "";
                }
            }
            else if (Session["usercode"] != null)
            {
                grouporusercode = " and usercode=" + Convert.ToString(Session["usercode"]).Trim() + "";
            }
            if (!string.IsNullOrEmpty(grouporusercode))
            {
                string Master1 = "select distinct settings,value,ROW_NUMBER() over (ORDER BY settings DESC) as SetValue1,Case when settings='Admission No' then '1' when settings='Register No' then '2' when settings='Roll No' then '3' end as SetValue from Master_Settings where settings in('Roll No','Register No','Admission No') and value='1' " + grouporusercode + "";
                dsSettings = dirAcc.selectDataSet(Master1);
            }
            else
            {
                dsSettings.Clear();
                DataTable dt = new DataTable();
                dt.Columns.Clear();
                dt.Rows.Clear();
                dt.Columns.Add("settings");
                dt.Columns.Add("SetValue");
                dt.Rows.Add("Admission No", "1");
                dt.Rows.Add("Register No", "2");
                dt.Rows.Add("Roll No", "3");
                dsSettings.Tables.Add(dt);
            }
        }
        catch (Exception ex)
        {

        }
        return dsSettings;
    }

    private void SetStudentWiseSettings()
    {
        try
        {
            DataSet dsSearchBy = new DataSet();
            dsSearchBy = GetSettings();
            if (dsSearchBy.Tables.Count > 0 && dsSearchBy.Tables[0].Rows.Count > 0)
            {
                ddlSearchBy.DataSource = dsSearchBy;
                ddlSearchBy.DataTextField = "settings";
                ddlSearchBy.DataValueField = "SetValue";
                ddlSearchBy.DataBind();
                ddlSearchBy.SelectedIndex = 0;
                SelectSearchBy(ddlSearchBy);
            }
        }
        catch (Exception ex)
        {

        }
    }

    private void SelectSearchBy(DropDownList ddlSearch)
    {
        try
        {
            txtAdmissionNo.Text = string.Empty;
            txtRollNo.Text = string.Empty;
            txtRegNo.Text = string.Empty;

            txtAdmissionNo.Visible = false;
            txtRollNo.Visible = false;
            txtRegNo.Visible = false;
            if (ddlSearch.Items.Count > 0)
            {
                string selectedValue = Convert.ToString(ddlSearch.SelectedValue).Trim();
                switch (selectedValue)
                {
                    case "1":
                        txtAdmissionNo.Visible = true;
                        break;
                    case "2":
                        txtRegNo.Visible = true;
                        break;
                    case "3":
                        txtRollNo.Visible = true;
                        break;
                }
            }
        }
        catch
        {
        }
    }

    protected void ddlSearchBy_SelectedIndexChanged(object sender, EventArgs e)
    {
        SelectSearchBy(ddlSearchBy);
    }

    protected void btnPopAlertClose_Click(object sender, EventArgs e)
    {
        try
        {
            lblAlertMsg.Text = string.Empty;
            divPopAlert.Visible = false;
        }
        catch (Exception ex)
        {

        }
    }
    #endregion


    #region magesh 3.9.18

    //public void addchk()
    //{

    //    //try
    //    //{


    //    //    // if (optradio.Items[1].Selected == true)
    //    //    // {
    //    //    foreach (GridViewRow row in GridView1.Rows)
    //    //    {
    //    //        if (row.RowType == DataControlRowType.DataRow)
    //    //        {
    //    //            {
    //    //                for (int i = 5; i <= dscount.Columns.Count; i++)
    //    //                {


    //    //                    CheckBox chk = new CheckBox();
    //    //                    chk.EnableViewState = false;
    //    //                    chk.Enabled = true;
    //    //                    chk.ID = "chkb" + i + "";




    //    //                    row.Cells[i].Controls.Add(chk);
    //    //                }
    //    //            }


    //    //            //GridView HeaderGrid = (GridView)sender;
    //    //            //GridViewRow HeaderGridRow = new GridViewRow(0, 0, DataControlRowType.Header, DataControlRowState.Insert);
    //    //            //TableCell HeaderCell = new TableCell();
    //    //            //HeaderCell.Text = "";
    //    //            //HeaderCell.ColumnSpan = 5;
    //    //            //TableCell HeaderCell5 = new TableCell();
    //    //            //HeaderCell5.Text = "Absent Hours";
    //    //            //HeaderCell5.HorizontalAlign = HorizontalAlign.Center;

    //    //            //DateTime datfr = new DateTime();
    //    //            //DateTime datto = new DateTime();
    //    //            //datfr = Convert.ToDateTime(datefrom);
    //    //            //datto = Convert.ToDateTime(dateto);
    //    //            //int count = 0;
    //    //            //while (datfr <= datto)
    //    //            //{
    //    //            //    count++;
    //    //            //    datfr = datfr.AddDays(1);
    //    //            //}

    //    //            //HeaderCell5.ColumnSpan = count;
    //    //            //TableCell HeaderCell6 = new TableCell();
    //    //            //HeaderCell6.Text = "";
    //    //            //HeaderCell6.ColumnSpan = 2;
    //    //            //HeaderGridRow.Cells.Add(HeaderCell);
    //    //            //HeaderGridRow.Cells.Add(HeaderCell5);
    //    //            //HeaderGridRow.Cells.Add(HeaderCell6);
    //    //            //GridView1.Controls[0].Controls.AddAt(0, HeaderGridRow);



    //    //        }
    //    //    }

    //    //    // }
    //    //}
    //    //catch
    //    //{
    //    //}
    //}

    protected void stud_rollno_Indexchanged(object sender, EventArgs e)
    {
        string uid = this.Page.Request.Params.Get("__EVENTTARGET");
        if (uid != null && uid.Contains("ctl"))
        {
            string[] values = uid.Split('$');

            CheckBox chkall = (CheckBox)GridView1.Rows[1].FindControl(values[4]);
            if (chkall.Checked)
            {
                for (int row = 2; row < GridView1.Rows.Count; row++)
                {
                    CheckBox chk = (CheckBox)GridView1.Rows[row].FindControl(values[4]);
                    chk.Checked = true;
                }
            }
            else
            {
                for (int row = 2; row < GridView1.Rows.Count; row++)
                {
                    CheckBox chk = (CheckBox)GridView1.Rows[row].FindControl(values[4]);
                    chk.Checked = false;
                }
            }
        }
    }

    protected void btngo_Click(object sender, EventArgs e)
    {
        try
        {
            errmsg.Visible = false;
            ArrayList listt = new ArrayList();
            Hashtable hat=new Hashtable();
            rowarray.Clear();
            GridView1.Visible = false;
            rowarray.Clear();
            dscount.Clear();
            dscounts.Clear();
            dscounts.Rows.Clear();
            dscount.Rows.Clear();
            dscount.Columns.Clear();
            dscounts.Columns.Clear();

            dscount.Columns.Add("S.No");
            dscount.Columns.Add("Roll.No");
            dscount.Columns.Add("Reg.No");
            dscount.Columns.Add("Student Type");
            dscount.Columns.Add("Student Name");

            dr = dscount.NewRow();
            dscount.Rows.Add(dr);

            rowarray.Add("S.No");
            rowarray.Add("Roll.No");
            rowarray.Add("Reg.No");
            rowarray.Add("Student Type");
            rowarray.Add("Student Name");

            dscounts.Columns.Add("Roll.No");
            dscounts.Columns.Add("Reg.No");
            dscounts.Columns.Add("Student Type");
            dscounts.Columns.Add("Student Name");

            GridView1.Visible = true;
            string batchyear = ddlbatch.SelectedItem.ToString();
            string degreecode = ddlbranch.SelectedValue.ToString();
            string sem = ddlsemester.SelectedValue.ToString();

            group_user = Session["group_code"].ToString();
            if (group_user.Contains(';'))
            {
                string[] group_semi = group_user.Split(';');
                group_user = group_semi[0].ToString();
            }
            string grouporusercode = "";
            if ((Session["group_code"].ToString().Trim() != "") && (Session["group_code"].ToString().Trim() != "0") && (Session["group_code"].ToString().Trim() != "-1"))
            {
                grouporusercode = " and group_code=" + Session["group_code"].ToString().Trim() + "";
            }
            else
            {
                grouporusercode = " and usercode=" + Session["usercode"].ToString().Trim() + "";
            }

            string Master1 = "select * from Master_Settings where " + grouporusercode + "";
            DataSet dss = GetSettings();


            string semlico = d2.GetFunction("select value from Master_Settings where settings='previous sem subject allotment' " + grouporusercode + "");
            int stusemester = Convert.ToInt32(d2.GetFunction("select distinct isnull(Current_Semester,'0') sem from Registration where Batch_Year='" + batchyear + "' and degree_code=" + degreecode + " and cc=0 and DelFlag=0 and Exam_Flag<>'debar' order by sem"));
            if (stusemester != Convert.ToInt32(sem) && semlico == "0")
            {
                clear();
                errmsg.Visible = true;
                errmsg.Text = "Please Update Student Semster and Student's in " + stusemester + " Semester ";
                return;
            }

            string subcde = string.Empty;
            int cnt = 0;
            for (int i = 0; i < cblSubject.Items.Count; i++)
            {
                if (cblSubject.Items[i].Selected)
                {
                    cnt++;
                    if (subcde == "")
                    {
                        subcde = cblSubject.Items[i].Value;
                    }
                    else
                    {
                        subcde = subcde + "','" + cblSubject.Items[i].Value;
                    }
                }
            }
            if (cnt == 0)
            {
                errmsg.Text = "Please Select Subject";
                return;
            }

            string secval = "";
            string secvals = "";
            string redoSec = "";
            if (ddlsection.Enabled == true && ddlsection.Items.Count > 0)
            {
                if (ddlsection.SelectedItem.ToString() != "All")
                {
                    secval = ddlsection.SelectedItem.ToString();
                    secvals = "and sections='" + secval + "'";
                    redoSec = " and r.sections='" + secval + "'";
                }
            }
            string a = (string)Session["collegecode"];
            string strorder = "ORDER BY Roll_No";
            string serialno = d2.GetFunction("select LinkValue from inssettings where college_code=" + Session["collegecode"].ToString() + " and linkname='Student Attendance'");
            if (serialno.Trim() == "1")
            {
                strorder = "ORDER BY serialno";
            }
            else
            {
                string orderby_Setting = d2.GetFunction("select value from master_Settings where settings='order_by'");
                if (orderby_Setting == "0")
                {
                    strorder = "ORDER BY Roll_No";
                }
                else if (orderby_Setting == "1")
                {
                    strorder = "ORDER BY Reg_No";
                }
                else if (orderby_Setting == "2")
                {
                    strorder = "ORDER BY Stud_Name";
                }
                else if (orderby_Setting == "0,1,2")
                {
                    strorder = "ORDER BY Roll_No,Reg_No,Stud_Name";
                }
                else if (orderby_Setting == "0,1")
                {
                    strorder = "ORDER BY Roll_No,Reg_No";
                }
                else if (orderby_Setting == "1,2")
                {
                    strorder = "ORDER BY Reg_No,Stud_Name";
                }
                else if (orderby_Setting == "0,2")
                {
                    strorder = "ORDER BY Roll_No,Stud_Name";
                }
            }



            string strredo = "select r.App_No,r.Reg_No,r.Roll_No,r.Stud_Name from Registration r,StudentRedoDetails s where r.App_No=s.Stud_AppNo and r.Batch_Year='" + batchyear + "' and r.degree_code='" + degreecode + "'" + redoSec;

            DataSet dsredo = d2.select_method_wo_parameter(strredo, "text");
            //Aruna 15oct2018===============================================
            //string strquery = "select Roll_No,Reg_No,Stud_Name,Stud_Type from Registration where Batch_Year='" + batchyear + "' and degree_code='" + degreecode + "' " + secvals + " and DelFlag=0 and Exam_Flag<>'debar' " + strorder + "";
            string stud_mode = "";
            string strquery = "";
            if (Convert.ToString(sem) == "1" || Convert.ToString(sem) == "2") //Changed by   Aruna
            {
                stud_mode = " and mode!=3"; //No need to display Lateral students
            }
            strquery = "select Roll_No,Reg_No,Stud_Name,Stud_Type from Registration where Batch_Year='" + batchyear + "' and degree_code='" + degreecode + "' " + secvals + " and DelFlag=0 and Exam_Flag<>'debar' " + stud_mode + " " + strorder + " "; //Changed by   Aruna
            //if (Convert.ToString(sem) != "") add by Aruna
            //{
            //    strquery = "select Roll_No,Reg_No,Stud_Name,Stud_Type from Registration where Batch_Year='" + batchyear + "' and degree_code='" + degreecode + "' " + secvals + " and DelFlag=0 and Exam_Flag<>'debar' and Current_Semester =" + sem + " " + strorder + "";
            //}


            //==============================================================






            ds.Dispose();
            ds.Reset();
            ds = d2.select_method_wo_parameter(strquery, "Text");
            int srno = 0;

            #region modified for adding roll no filter

            string[] Rollnoarr = new string[0];
            string Rollqry = string.Empty;
            string searchValue = string.Empty;
            string searchField = string.Empty;
            string qrySearchValue = string.Empty;
            string selectedText = string.Empty;
            if (ddlSearchBy.Items.Count > 0)
            {
                string selectedValue = Convert.ToString(ddlSearchBy.SelectedValue).Trim();
                selectedText = Convert.ToString(ddlSearchBy.SelectedItem.Text).Trim();
                switch (selectedValue)
                {
                    case "1":
                        searchValue = txtAdmissionNo.Text.Trim();
                        searchField = " Roll_Admit ";
                        break;
                    case "2":
                        searchValue = txtRegNo.Text.Trim();
                        searchField = " Reg_No ";
                        break;
                    case "3":
                        searchValue = txtRollNo.Text.Trim();
                        searchField = " Roll_no ";
                        break;
                }
                if (!string.IsNullOrEmpty(searchValue) && !string.IsNullOrEmpty(searchField))
                {
                    Rollnoarr = searchValue.Split(',');
                    Rollqry = string.Empty;

                    if (Rollnoarr.Length > 0)
                    {
                        for (int i = 0; i < Rollnoarr.Length; i++)
                        {
                            Rollqry += "'" + Rollnoarr[i] + "',";
                        }
                        qrySearchValue = " and " + searchField + " in(" + Rollqry.Trim(',') + ")";
                    }
                    else
                    {
                        errmsg.Text = " Enter the Valid Student Roll No";
                        errmsg.Visible = true;
                    }
                }
                else
                {
                    qrySearchValue = "";
                }
            }

            if (Rollnoarr.Length > 0 && Rollnoarr.Length < Rollqry.Length)
            {
                strquery = "select Roll_No,Reg_No,Stud_Name,Stud_Type from Registration where Batch_Year='" + batchyear + "' and degree_code='" + degreecode + "' " + secvals + " and DelFlag=0 and Exam_Flag<>'debar' " + qrySearchValue + "  " + strorder + "";
                ds.Dispose();
                ds.Reset();
                ds = d2.select_method_wo_parameter(strquery, "Text");

                if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count == 0)
                {
                    errmsg.Visible = true;
                    errmsg.Text = selectedText + " : " + Rollqry.Replace("'", "").Trim(',') + " Not Belong to " + batchyear + "-" + Convert.ToString(ddldegree.SelectedItem.Text).Trim() + "-" + Convert.ToString(ddlbranch.SelectedItem.Text).Trim() + (!string.IsNullOrEmpty(secval) ? "-" + secval : "");
                    GridView1.Visible = false;
                    return;
                }
            }
            #endregion

            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                chkexammrk.Visible = true;
                GridView1.Visible = true;
                btnsave.Visible = true;
                bool isredo = false;
                dr = dscount.NewRow();
                dscount.Rows.Add(dr);
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {

                    isredo = false;
                    string rollno = ds.Tables[0].Rows[i]["Roll_No"].ToString();
                    if (dsredo.Tables.Count > 0 && dsredo.Tables[0].Rows.Count > 0)
                    {
                        dsredo.Tables[0].DefaultView.RowFilter = "roll_no='" + rollno + "'";
                        DataTable dtisRedo = dsredo.Tables[0].DefaultView.ToTable();
                        if (dtisRedo.Rows.Count > 0)
                            isredo = true;
                    }
                    if (!isredo)
                    {
                        srno++;
                        #region magesh
                        dr = dscount.NewRow();
                        //drs = dscounts.NewRow();
                        dr["S.No"] = srno;
                        dr["Roll.No"] = ds.Tables[0].Rows[i]["Roll_No"].ToString();
                        dr["Reg.No"] = ds.Tables[0].Rows[i]["Reg_No"].ToString();
                        dr["Student Type"] = ds.Tables[0].Rows[i]["Stud_Type"].ToString();
                        dr["Student Name"] = ds.Tables[0].Rows[i]["Stud_Name"].ToString();
                        #endregion

                        //dr["Total_Absent_Hours"] = perabsenthrs1.ToString();
                        string regno = ds.Tables[0].Rows[i]["Reg_No"].ToString();
                        string stype = ds.Tables[0].Rows[i]["Stud_Type"].ToString();
                        string sname = ds.Tables[0].Rows[i]["Stud_Name"].ToString();
                        // dscount.Columns[5].ReadOnly = false;
                        dscount.Rows.Add(dr);
                        //dscounts.Rows.Add(drs);
                        //dscount.Columns["Student Type"].ReadOnly = false;
                    }
                }

                string strstusub = "select r.Roll_No,r.Reg_No,sc.subject_no  from Registration r,subjectChooser sc where r.Roll_No=sc.roll_no and r.Batch_Year='" + batchyear + "' and r.degree_code='" + degreecode + "' and sc.semester='" + sem + "' " + secvals + " and r.DelFlag=0 and r.Exam_Flag<>'debar'";
                DataSet dsstusub = d2.select_method_wo_parameter(strstusub, "Text");

                //string strgetsubjectdetails = "select ss.subject_type,ss.subType_no,s.subject_name,s.subject_code,s.subject_no,ss.electivepap from syllabus_master sy,sub_sem ss,subject s where sy.syll_code=ss.syll_code and sy.syll_code=s.syll_code and ss.syll_code=s.syll_code and ss.subType_no=s.subType_no and sy.Batch_Year='" + batchyear + "' and sy.degree_code='" + degreecode + "' and sy.semester='" + sem + "' order by ss.subject_type,s.subject_name,s.subject_code";

                string strgetsubjectdetails = "select ss.subject_type,ss.subType_no,s.subject_name,s.subject_code,s.subject_no,ss.electivepap from syllabus_master sy,sub_sem ss,subject s where sy.syll_code=ss.syll_code and sy.syll_code=s.syll_code and ss.syll_code=s.syll_code and ss.subType_no=s.subType_no and s.subject_code in('" + subcde + "') and sy.Batch_Year='" + batchyear + "' and sy.degree_code='" + degreecode + "' and sy.semester='" + sem + "' order by ss.subject_type,s.subject_name,s.subject_code";

                //string strgetsubjectdetails = "select ss.subject_type,ss.subType_no,s.subject_name,s.subject_code,s.subject_no,ss.electivepap from syllabus_master sy,sub_sem ss,subject s where sy.syll_code=ss.syll_code and sy.syll_code=s.syll_code and ss.syll_code=s.syll_code and ss.subType_no=s.subType_no and s.subject_code in('') and sy.Batch_Year='2017' and sy.degree_code='45' and sy.semester='3' order by ss.subject_type,s.subject_name,s.subject_code";
                DataSet dssubj = d2.select_method_wo_parameter(strgetsubjectdetails, "Text");
                if (dssubj.Tables.Count > 0 && dssubj.Tables[0].Rows.Count > 0)
                {
                    for (int s = 0; s < dssubj.Tables[0].Rows.Count; s++)
                    {
                        
                        DataColumn dc = new DataColumn();
                        // dr = dscount.NewRow();
                        // dscount.Columns.Add(Convert.ToString(dssubj.Tables[0].Rows[s]["subject_name"]) + "-" + Convert.ToString(dssubj.Tables[0].Rows[s]["subject_code"]));


                        string colun = Convert.ToString(dssubj.Tables[0].Rows[s]["subject_name"]) + "-" + Convert.ToString(dssubj.Tables[0].Rows[s]["subject_code"]);
                        string subjectname = dssubj.Tables[0].Rows[s]["subject_name"].ToString();
                        string scode = dssubj.Tables[0].Rows[s]["subject_code"].ToString();
                        string subno = dssubj.Tables[0].Rows[s]["subject_no"].ToString();
                        string subtyno = dssubj.Tables[0].Rows[s]["subType_no"].ToString();
                        string elective = dssubj.Tables[0].Rows[s]["electivepap"].ToString();

                        dscounts.Columns.Add(Convert.ToString(dssubj.Tables[0].Rows[s]["subType_no"]) + "-" + Convert.ToString(dssubj.Tables[0].Rows[s]["subject_no"]));
                        dscount.Columns.Add(colun, System.Type.GetType("System.Boolean"));
                        rowarray.Add(colun);

                        if (Session["electiveonly"].ToString().Trim() == "1")
                        {
                            if (elective.Trim() == "1" || elective.Trim().ToLower() == "true")
                            {
                            }
                            else
                            {
                                if(!hat.ContainsKey(colun))
                                 hat.Add(colun,colun);
                            }
                        }


                        for (int r = 1; r < dscount.Rows.Count; r++)
                        {
                            string rollno = dscount.Rows[r][1].ToString();
                            dsstusub.Tables[0].DefaultView.RowFilter = "Roll_No='" + rollno + "' and subject_no='" + subno + "'";
                            DataView dvstusub = dsstusub.Tables[0].DefaultView;
                            if (dvstusub.Count > 0)
                            {                                
                                dscount.Rows[r][colun] = true;                                
                            }
                            else
                            {
                                dscount.Rows[r][colun] = false;
                            }
                        }
                    }
                }
            }
            
            Session["dscount"] = dscount;
            Session["rowarray"] = rowarray;
            ViewState["CurrentTable"] = dscount;
            ViewState["CurrentTables"] = dscounts;
            GridView1.DataSource = dscount;
            GridView1.DataBind();
            int m = GridView1.Rows.Count;
            int n = dscount.Columns.Count;

            for (int i = 5; i < GridView1.HeaderRow.Cells.Count;i++ )
            {
                if (hat.ContainsKey(GridView1.HeaderRow.Cells[i].Text))
                {
                    foreach (GridViewRow grid in GridView1.Rows)
                    {
                        GridView1.HeaderRow.Cells[i].Visible = false;
                        grid.Cells[i].Visible = false;
                    }
                }
            }


            int cun = 2;



            for (int row = 0; row < GridView1.Rows.Count; row++)
            {
                if (Session["Rollflag"].ToString() == "1")
                {
                    if (row == 0)
                    {
                        cun++;
                    }

                    GridView1.HeaderRow.Cells[1].Visible = true;
                    GridView1.Rows[row].Cells[1].Visible = true;
                }
                else
                {
                    GridView1.HeaderRow.Cells[1].Visible = false;
                    GridView1.Rows[row].Cells[1].Visible = false;
                }

                if (Session["Regflag"].ToString() == "1")
                {
                    if (row == 0)
                    {
                        cun++;
                    }
                    GridView1.HeaderRow.Cells[2].Visible = true;
                    GridView1.Rows[row].Cells[2].Visible = true;
                }
                else
                {
                    GridView1.HeaderRow.Cells[2].Visible = false;
                    GridView1.Rows[row].Cells[2].Visible = false;
                }

                if (Session["Studflag"].ToString() == "1")
                {
                    if (row == 0)
                    {
                        cun++;
                    }
                    GridView1.HeaderRow.Cells[3].Visible = true;
                    GridView1.Rows[row].Cells[3].Visible = true;
                }
                else
                {
                    GridView1.HeaderRow.Cells[3].Visible = false;
                    GridView1.Rows[row].Cells[3].Visible = false;
                }
            }
            hid.Value = Convert.ToString(cun);
                        

                Grid_Alignment();
            RowHead(GridView1, 1);

            if (GridView1.Rows.Count == 1)
            {
                GridView1.Visible = false;
                errmsg.Text = "No Record..!";
                errmsg.Visible = true;
            }
        }
        catch
        {
        }
    }

    protected void RowHead(GridView gview, int count)
    {
        for (int head = 0; head < count; head++)
        {
            gview.Rows[head].BackColor = ColorTranslator.FromHtml("#0CA6CA");
            gview.Rows[head].Font.Bold = true;
            gview.Rows[head].Font.Name = "Book Antique";
            gview.Rows[head].HorizontalAlign = HorizontalAlign.Center;
        }
    }

    public void Grid_Alignment()
    {
        for (int row = 0; row < GridView1.Rows.Count; row++)
        {
            for (int cell = 0; cell < GridView1.HeaderRow.Cells.Count; cell++)
            {
                if (GridView1.HeaderRow.Cells[cell].Text.Trim() != "Student Name")
                {
                    GridView1.Rows[row].Cells[cell].HorizontalAlign = HorizontalAlign.Center;
                }
                else
                {
                    // GridView1.Rows[row].Cells[cell].HorizontalAlign = HorizontalAlign.Center;
                    GridView1.Rows[row].Cells[cell].Width = 200;
                }
            }
        }
    }

    public void savefunction()
    {
        //try
        //{
        // addchk();
        DataTable dscountVal = (DataTable)ViewState["CurrentTables"];
        int savevalue = 0;
        Boolean saveflag = false;
        string insertupdatequery = "";
        int insupdval = 0;
        string batchyear = ddlbatch.SelectedItem.ToString();
        string degreecode = ddlbranch.SelectedValue.ToString();
        string sem = ddlsemester.SelectedValue.ToString();

        string secval = "";
        string secvals = "";
        if (ddlsection.Enabled == true && ddlsection.Items.Count > 0)
        {
            if (ddlsection.SelectedItem.ToString() != "All")
            {
                secval = ddlsection.SelectedItem.ToString();
                secvals = "and sections='" + secval + "'";
            }
        }

        string deletequery = "select r.Roll_No,r.Reg_No,sc.subject_no,sc.subtype_no,sc.semester from subjectchooser sc,Registration r where sc.roll_no=r.roll_no and r.batch_year='" + batchyear + "' and r.degree_code='" + degreecode + "' and sc.semester='" + sem + "' " + secvals + "";
        DataSet dssnu = d2.select_method_wo_parameter(deletequery, "Text");
        if (dssnu.Tables[0].Rows.Count > 0)
        {
            savevalue = 2;
        }
        Hashtable ht = new Hashtable();
        int m = 2;
        string val = string.Empty;
        //foreach (GridViewRow row in GridView1.Rows)
        for (int row = 2; row < GridView1.Rows.Count; row++)
        {
            // m++;

            // if (m < dscount.Rows.Count)
            // {

            string rollno = GridView1.Rows[m].Cells[1].Text;//dscount.Rows[m][1].ToString()
            //do what you want with the value




            //  for (int r = 1; r < dscount.Columns.Count; r++)
            // {

            // string rollno = FpSpread1.Sheets[0].Cells[r, 1].Text.ToString();
            //string rollno = dr[1].ToString();

            Hashtable hatsubroll = new Hashtable();
            
            for (int c = 4; c < dscountVal.Columns.Count; c++)
            {
                string subj = dscountVal.Columns[c].ToString();
                string[] spl = subj.Split('-');
                if (spl.Length == 2)
                {
                    string subno = spl[1];
                    string subtypeno = spl[0];


                    dssnu.Tables[0].DefaultView.RowFilter = " roll_no='" + rollno + "' and subject_no='" + subno + "'";
                    DataView dvsub = dssnu.Tables[0].DefaultView;

                    saveflag = true;
                    if (c < 14)
                    {
                        int a = c - 4;
                        val = "0" + a + "";
                    }
                    else
                        val = Convert.ToString(c - 4);
                    string chkname = "ctl" + val + "";
                    
                    System.Web.UI.WebControls.CheckBox chkRow = (System.Web.UI.WebControls.CheckBox)GridView1.Rows[row].FindControl(chkname);
                    
                    if (chkRow.Checked == true)
                    {
                        int paper = 0;
                        if (hatsubroll.Contains(rollno.Trim().ToLower() + '-' + subtypeno))
                        {
                            hatsubroll[rollno.Trim().ToLower() + '-' + subtypeno] = Convert.ToInt32(hatsubroll[rollno.Trim().ToLower() + '-' + subtypeno]) + 1;
                        }
                        else
                        {
                            hatsubroll.Add(rollno.Trim().ToLower() + '-' + subtypeno, 1);
                        }
                        if (dvsub.Count == 0)
                        {
                            paper = Convert.ToInt32(hatsubroll[rollno.Trim().ToLower() + '-' + subtypeno]);
                            ht.Clear();
                            ht.Add("roll_no", rollno);
                            ht.Add("semester", sem.ToString());
                            ht.Add("subject_no", subno.ToString());
                            ht.Add("subtype_no", subtypeno.ToString());
                            ht.Add("paper_order", paper.ToString());
                            ht.Add("batch", "");
                            ht.Add("grp_cell", 0);
                            insupdval = d2.insert_method("sp_ins_upd_Subjectchooser", ht, "sp");
                            //insertupdatequery = " if not exists (select * from subjectChooser where roll_no='" + rollno + "' and subject_no='" + subno + "' and semester='" + sem + "')";
                            //insertupdatequery = insertupdatequery + " insert into subjectChooser(roll_no,semester,subtype_no,subject_no,paper_order) values('" + rollno + "','" + sem + "','" + subtypeno + "','" + subno + "','1')";
                            //insupdval = d2.update_method_wo_parameter(insertupdatequery, "Text");
                        }
                    }
                    else
                    {
                        if (dvsub.Count > 0)
                        {
                            ht.Clear();
                            ht.Add("roll_no", rollno);
                            ht.Add("semester", sem.ToString());
                            ht.Add("subject_no", subno.ToString());
                            insupdval = d2.insert_method("sp_delete_Subjectchooser", ht, "sp");
                            if (chkexammrk.Checked == true)
                            {
                                insertupdatequery = " delete from mark_entry where roll_no='" + rollno + "' and subject_no='" + subno + "'";
                                insupdval = d2.update_method_wo_parameter(insertupdatequery, "Text");
                            }
                        }
                    }
                }
            }
            //}
            m++;
        }
        if (saveflag == true)
        {
            divPopAlert.Visible = true;
            lblAlertMsg.Text = "Saved Successfully";

            string entrycode = Session["Entry_Code"].ToString();
            string formname = "Matser Student Subject Allotment";
            string toa = DateTime.Now.ToString("MM/dd/yyy hh:mm:ss tt");
            string doa = DateTime.Now.ToString("MM/dd/yyy");
            string section = "";
            if (ddlsection.SelectedValue.ToString() != "" && ddlsection.SelectedValue.ToString() != "All" && ddlsection.SelectedValue.ToString() != null && ddlsection.SelectedValue.ToString() != "0")
            {
                section = ":Sections -" + ddlsection.SelectedValue.ToString();
            }
            string details = "" + ddlbranch.SelectedValue.ToString() + ":Sem - " + ddlsemester.SelectedValue.ToString() + ":Batch Year -" + ddlbatch.SelectedValue.ToString() + " " + section + "";
            string modules = "0";
            string act_diff = " ";
            string ctsname = "Update The Student Subject Allotment";
            if (savevalue == 1)
            {
                ctsname = "Save the Student Subject Allotment";
            }

            string strlogdetails = "insert into UserLog (Entry_Code,Form_Name,UsrAction,TOA,DOA,Details,Module,Act_Diff,ctrNam) values ('" + entrycode + "','" + formname + "','" + savevalue + "','" + toa + "','" + doa + "','" + details + "','" + modules + "','" + act_diff + "','" + ctsname + "')";
            int a = d2.update_method_wo_parameter(strlogdetails, "Text");
        }
    }

    public void btnsaveok_Click(object sender, EventArgs e)
    {
        int m = dscount.Rows.Count;
        savefunction();
        btngo_Click(sender, e);
    }

    protected void btnaddsub_Click(object sender, EventArgs e)
    {
        try
        {
            string insertupdatequery = "";
            int insupdval = 0;
            string batchyear = ddlbatch.SelectedItem.ToString();
            string degreecode = ddlbranch.SelectedValue.ToString();
            string sem = ddlsemester.SelectedValue.ToString();
            string subjecttype = ddlsubtype.SelectedItem.ToString();
            if (ddlsubject.Items.Count > 0)
            {
                string subjectnam = ddlsubject.SelectedItem.ToString();
                string subcode = ddlsubject.SelectedValue.ToString();
                string[] spval = subjectnam.Split('-');
                string getsubname = "";
                for (int g = 0; g < spval.GetUpperBound(0); g++)
                {
                    if (getsubname == "")
                    {
                        getsubname = spval[g].ToString();
                    }
                    else
                    {
                        getsubname = getsubname + "-" + spval[g].ToString();
                    }
                }
                getsubname = getsubname.Trim();

                string sylcode = d2.GetFunction("select syll_code from syllabus_master where Batch_Year='" + batchyear + "' and degree_code='" + degreecode + "' and semester='" + sem + "'");
                if (sylcode.Trim() == "0" || sylcode.Trim() == "")
                {
                    insertupdatequery = "if not exists (select syll_code from syllabus_master where Batch_Year='" + batchyear + "' and degree_code='" + degreecode + "' and semester='" + sem + "')";
                    insertupdatequery = insertupdatequery + " insert into syllabus_master(Batch_Year,degree_code,semester,syllabus_year) values('" + batchyear + "','" + degreecode + "','" + sem + "','" + batchyear + "')";
                    insupdval = d2.update_method_wo_parameter(insertupdatequery, "Text");

                    sylcode = d2.GetFunction("select syll_code from syllabus_master where Batch_Year='" + batchyear + "' and degree_code='" + degreecode + "' and semester='" + sem + "'");
                }

                string subtypeno = d2.GetFunction("select subtype_no from sub_sem where subject_type='" + subjecttype + "' and syll_code='" + sylcode + "'");
                if (subtypeno.Trim() == "" || subtypeno.Trim() == "0")
                {
                    string subjecttypequery = "if not exists(select subtype_no from sub_sem where subject_type='" + subjecttype + "' and syll_code='" + sylcode + "')";
                    subjecttypequery = subjecttypequery + " insert into sub_sem (syll_code,subject_type,no_of_papers,promote_count,Lab,markOrGrade,pre_schedule) values ('" + sylcode + "','" + subjecttype + "','6','1','0','1','0')";

                    insupdval = d2.update_method_wo_parameter(subjecttypequery, "text");

                    subtypeno = d2.GetFunction("select subtype_no from sub_sem where subject_type='" + subjecttype + "' and syll_code='" + sylcode + "'");
                }

                string subjectcode = ddlsubject.SelectedValue.ToString();

                string subjectnoval = d2.GetFunction("select subject_no from subject where subject_code='" + subjectcode + "' and subType_no='" + subtypeno + "' and syll_code='" + sylcode + "'");
                if (subjectnoval.ToString().Trim() == "0" || subjectnoval.ToString().Trim() == "")
                {
                    string subquery = "select * from subject where subject_code='" + subjectcode + "'";
                    DataSet dssubje = d2.select_method_wo_parameter(subquery, "Text");
                    if (dssubje.Tables[0].Rows.Count > 0)
                    {
                        string subjectname = dssubje.Tables[0].Rows[0]["subject_name"].ToString();
                        string credit = dssubje.Tables[0].Rows[0]["credit_points"].ToString();
                        string paperno = dssubje.Tables[0].Rows[0]["PaperID"].ToString();
                        string inmin = dssubje.Tables[0].Rows[0]["min_int_marks"].ToString();
                        string inmax = dssubje.Tables[0].Rows[0]["max_int_marks"].ToString();
                        string extmin = dssubje.Tables[0].Rows[0]["min_ext_marks"].ToString();
                        string extmax = dssubje.Tables[0].Rows[0]["max_ext_marks"].ToString();
                        string mintotal = dssubje.Tables[0].Rows[0]["mintotal"].ToString();
                        string maxtotal = dssubje.Tables[0].Rows[0]["maxtotal"].ToString();
                        string curfee = dssubje.Tables[0].Rows[0]["curfee"].ToString();
                        string arrfee = dssubje.Tables[0].Rows[0]["arrfee"].ToString();
                        if (paperno.Trim() == "")
                        {
                            paperno = "0";
                        }
                        if (curfee.Trim().ToLower() == "")
                        {
                            curfee = "0";
                        }
                        if (arrfee.Trim().ToLower() == "")
                        {
                            arrfee = "0";
                        }

                        string subjectsavequery = "if not exists(select subject_no from subject where subject_code='" + subjectcode + "' and subType_no='" + subtypeno + "' and syll_code='" + sylcode + "')";
                        subjectsavequery = subjectsavequery + " insert into subject (subject_code,subject_name,subType_no,min_int_marks,max_int_marks,min_ext_marks,max_ext_marks,mintotal,maxtotal,credit_points,syll_code,curfee,arrfee)";
                        subjectsavequery = subjectsavequery + " values('" + subjectcode + "','" + subjectname + "','" + subtypeno + "','" + inmin + "','" + inmax + "','" + extmin + "','" + extmax + "','" + mintotal + "','" + maxtotal + "','" + credit + "','" + sylcode + "','" + curfee + "','" + arrfee + "')";

                        insupdval = d2.update_method_wo_parameter(subjectsavequery, "text");

                        int subjectno = Convert.ToInt32(d2.GetFunction("select subject_no from subject where subject_code='" + subjectcode + "' and subType_no='" + subtypeno + "' and syll_code='" + sylcode + "'"));

                        BindsubType();
                        Bindsub();
                        btngo_Click(sender, e);
                    }
                }
                else
                {
                    errmsg.Visible = true;
                    errmsg.Text = "Already Subject Exists!!!";
                    return;
                    
                }
            }
            else
            {
                errmsg.Visible = true;
                errmsg.Text = "No Subject Available";
            }
        }
        catch (Exception ex)
        {
            errmsg.Visible = true;
            errmsg.Text = ex.ToString();
        }
    }

    protected void gridview1_OnRowDataBound(object sender, GridViewRowEventArgs e)
    {
        count++;
        try
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                ArrayList rowaray = (ArrayList)Session["rowarray"];
                int m = 0;
                e.Row.Cells[3].Width = 50;
                if (e.Row.RowIndex == 0)
                {
                    e.Row.BackColor = ColorTranslator.FromHtml("#0CA6CA");
                    e.Row.Font.Bold = true;
                    e.Row.HorizontalAlign = HorizontalAlign.Center;
                    e.Row.Font.Name = "Book Antique";
                    for (int cell = 0; cell < e.Row.Cells.Count; cell++)
                    {   //e.Row.Cells[cell].Text;
                        e.Row.Cells[cell].Text = Convert.ToString(rowaray[cell]);
                    }
                }

                if (e.Row.RowIndex == 1)
                {
                    string val = string.Empty;
                    for (int c = 0; c < dscount.Columns.Count - 4; c++)
                    {
                        if (c < 10)
                        {
                            //int a = c - 4;
                            int a = c;
                            val = "0" + a + "";
                        }
                        else
                        {
                            val = Convert.ToString(c);
                        }
                        string chkname = "ctl" + val + "";
                        CheckBox stud_rollno = (CheckBox)e.Row.FindControl(chkname);
                        stud_rollno.Enabled = true;
                        //stud_rollno.AutoPostBack = true;

                        ((CheckBox)e.Row.FindControl(chkname)).Attributes.Add("onclick",
                            "javascript:SelectAll('" +
                            ((CheckBox)e.Row.FindControl(chkname)).ClientID + "')");

                        
                    }
                }

                string val1 = string.Empty;
                if (e.Row.RowIndex != 0 && e.Row.RowIndex != 1)
                {
                    for (int c = 0; c < dscount.Columns.Count - 4; c++)
                    {
                        if (c < 10)
                        {
                            //int a = c - 4;
                            int a = c;
                            val1 = "0" + a + "";
                        }
                        else
                        {
                            val1 = Convert.ToString(c);
                        }
                        string chkname = "ctl" + val1 + "";
                        CheckBox stud_rollno = (CheckBox)e.Row.FindControl(chkname);
                        stud_rollno.Enabled = true;
                        if (stud_rollno.Checked)
                        {
                            stud_rollno.Attributes.Add("onclick", "HeaderCheckBoxClick(this);");
                        }
                    }
                }
            }
        }
        catch
        {
        }
    }

    private string getCblSelectedText(CheckBoxList cblSelected)
    {
        StringBuilder selectedText = new StringBuilder();
        try
        {
            for (int sel = 0; sel < cblSelected.Items.Count; sel++)
            {
                if (cblSelected.Items[sel].Selected == true)
                {
                    if (selectedText.Length == 0)
                    {
                        selectedText.Append("'" + Convert.ToString(cblSelected.Items[sel].Text) + "'");
                    }
                    else
                    {
                        selectedText.Append(",'" + Convert.ToString(cblSelected.Items[sel].Text) + "'");
                    }
                }
            }
        }
        catch { }
        return selectedText.ToString();
    }

    protected void CheckBoxList1_SelectedIndexChanged(object sender, EventArgs e)
    {
        CallCheckboxListChange(CheckBox1, CheckBoxList1, txtSubType, lblSuType.Text, "--Select--");
        Bindsub();
    }

    public void CheckBox1_checkedchange(object sender, EventArgs e)
    {
        CallCheckboxChange(CheckBox1, CheckBoxList1, txtSubType, lblSuType.Text, "--Select--");
        Bindsub();
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

    private void CallCheckboxChange(CheckBox cb, CheckBoxList cbl, TextBox txt, string dispst, string deft)
    {
        try
        {
            int sel = 0;
            string name = string.Empty;
            txt.Text = deft;
            if (cb.Checked == true)
            {
                for (sel = 0; sel < cbl.Items.Count; sel++)
                {
                    cbl.Items[sel].Selected = true;
                    name = Convert.ToString(cbl.Items[sel].Text);
                }
                if (cbl.Items.Count == 1)
                {
                    txt.Text = "" + name + "";
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

    public void cbSubjet_checkedchange(object sender, EventArgs e)
    {
        try
        {
            clear();
            txtSubject.Text = "--Select--";
            if (cbSubjet.Checked == true)
            {
                for (int i = 0; i < cblSubject.Items.Count; i++)
                {
                    cblSubject.Items[i].Selected = true;
                }
                txtSubject.Text = "Subjects(" + (cblSubject.Items.Count) + ")";
            }
            else
            {
                for (int i = 0; i < cblSubject.Items.Count; i++)
                {
                    cblSubject.Items[i].Selected = false;
                }
            }
        }
        catch { }
    }

    protected void cblSubject_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            clear();
            int i = 0;
            cbSubjet.Checked = false;
            int commcount = 0;
            txtSubject.Text = "--Select--";
            for (i = 0; i < cblSubject.Items.Count; i++)
            {
                if (cblSubject.Items[i].Selected == true)
                {
                    commcount = commcount + 1;
                }
            }
            if (commcount > 0)
            {
                if (commcount == cblSubject.Items.Count)
                {
                    cbSubjet.Checked = true;
                }
                txtSubject.Text = "Subjects(" + commcount.ToString() + ")";
            }
        }
        catch { }
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

    #endregion
}
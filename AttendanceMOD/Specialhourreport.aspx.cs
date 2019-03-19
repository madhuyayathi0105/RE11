using System;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Collections;
using System.Configuration;
using System.Drawing;
using System.Collections.Generic;

public partial class Specialhourreport : System.Web.UI.Page
{
    string usercode = string.Empty;
    string collegecode = string.Empty;
    string singleuser = string.Empty;
    string group_user = string.Empty;
    string course_id = string.Empty;
    string strbranch = string.Empty;
    string strbatchyear = string.Empty;
    string strbatch = string.Empty;

    DAccess2 d2 = new DAccess2();
    DataSet ds2 = new DataSet1();
    Hashtable hat = new Hashtable();

    SqlConnection con = new SqlConnection(ConfigurationManager.AppSettings["DSN"]);
    //   SqlConnection con1 = new SqlConnection(ConfigurationManager.AppSettings["DSN"]);

    DataTable data = new DataTable();
    DataRow drow;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["collegecode"] == null) //Aruna For Back Button
        {
            Response.Redirect("~/Default.aspx");
        }
        norecordlbl.Visible = false;
        usercode = Session["usercode"].ToString();
        collegecode = Session["collegecode"].ToString();
        singleuser = Session["single_user"].ToString();
        group_user = Session["group_code"].ToString();
        norecordlbl.Visible = false;
        if (!Page.IsPostBack)
        {
            btnprintmaster.Visible = false;
            btnPrint.Visible = false;
            txtfromdate.Attributes.Add("readonly", "readonly");
            txttodate.Attributes.Add("readonly", "readonly");

            Showgrid.Visible = false;
            norecordlbl.Visible = false;
            txtexcelname.Visible = false;
            lblrptname.Visible = false;
            btnexcel.Visible = false;
            lblrptname.Visible = false;
            txtexcelname.Visible = false;
            rbdetail.Checked = true;
            btnprintmaster.Visible = false;
            btnPrint.Visible = false;
            divMainContents.Visible = false;

            BindBatch();
            BindDegree(singleuser, group_user, collegecode, usercode);
            if (ddldegree.Items.Count > 0)
            {
                ddldegree.Enabled = true;
                ddlbranch.Enabled = true;
                ddlsemester.Enabled = true;
                ddlsection.Enabled = true;
                btngo.Enabled = true;
                txtfromdate.Enabled = true;
                txttodate.Enabled = true;
                BindBranch(singleuser, group_user, course_id, collegecode, usercode);
                BindSem(strbranch, strbatchyear, collegecode);
                BindSectionDetail(strbatch, strbranch);
                GetSubject();
                txtfromdate.Text = DateTime.Now.ToString("dd/MM/yyyy");
                txttodate.Text = DateTime.Now.ToString("dd/MM/yyyy");
            }
            else
            {
                ddldegree.Enabled = false;
                ddlbranch.Enabled = false;
                ddlsemester.Enabled = false;
                ddlsection.Enabled = false;
                btngo.Enabled = false;
                txtfromdate.Enabled = false;
                txttodate.Enabled = false;
            }

           

        }
    }
    public void BindBatch()
    {
        try
        {
            ds2.Dispose();
            ds2.Reset();
            ds2 = d2.BindBatch();
            if (ds2.Tables[0].Rows.Count > 0)
            {
                ddlbatch.DataSource = ds2;
                ddlbatch.DataTextField = "Batch_year";
                ddlbatch.DataValueField = "Batch_year";
                ddlbatch.DataBind();
                ddlbatch.SelectedIndex = ddlbatch.Items.Count - 1;
            }
        }
        catch (Exception ex)
        {
            norecordlbl.Text = ex.ToString();
        }
    }
    public void BindDegree(string singleuser, string group_user, string collegecode, string usercode)
    {
        try
        {
            ddldegree.Items.Clear();
            if (group_user.Contains(';'))
            {
                string[] group_semi = group_user.Split(';');
                group_user = group_semi[0].ToString();
            }
            ds2.Dispose();
            ds2.Reset();
            ds2 = d2.BindDegree(singleuser, group_user, collegecode, usercode);
            if (ds2.Tables[0].Rows.Count > 0)
            {
                ddldegree.DataSource = ds2;
                ddldegree.DataTextField = "course_name";
                ddldegree.DataValueField = "course_id";
                ddldegree.DataBind();

            }
        }
        catch (Exception ex)
        {
            norecordlbl.Text = ex.ToString();
        }
    }
    public void BindBranch(string singleuser, string group_user, string course_id, string collegecode, string usercode)
    {
        try
        {
            course_id = ddldegree.SelectedValue.ToString();
            ddlbranch.Items.Clear();
            if (group_user.Contains(';'))
            {
                string[] group_semi = group_user.Split(';');
                group_user = group_semi[0].ToString();
            }
            ds2.Dispose();
            ds2.Reset();
            ds2 = d2.BindBranch(singleuser, group_user, course_id, collegecode, usercode);
            if (ds2.Tables[0].Rows.Count > 0)
            {
                ddlbranch.DataSource = ds2;
                ddlbranch.DataTextField = "dept_name";
                ddlbranch.DataValueField = "degree_code";
                ddlbranch.DataBind();
            }
        }
        catch (Exception ex)
        {
            norecordlbl.Text = ex.ToString();
        }
    }
    public void BindSem(string strbranch, string strbatchyear, string collegecode)
    {

        try
        {
            strbatchyear = ddlbatch.Text.ToString();
            strbranch = ddlbranch.SelectedValue.ToString();

            ddlsemester.Items.Clear();
            Boolean first_year;
            first_year = false;
            int duration = 0;
            int i = 0;
            ds2.Dispose();
            ds2.Reset();
            ds2 = d2.BindSem(strbranch, strbatchyear, collegecode);
            if (ds2.Tables[0].Rows.Count > 0)
            {
                first_year = Convert.ToBoolean(Convert.ToString(ds2.Tables[0].Rows[0][1]).ToString());
                duration = Convert.ToInt32(Convert.ToString(ds2.Tables[0].Rows[0][0]).ToString());
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
        catch (Exception ex)
        {
            norecordlbl.Text = ex.ToString();
        }
    }
    public void BindSectionDetail(string strbatch, string strbranch)
    {
        try
        {
            strbatch = ddlbatch.SelectedValue.ToString();
            strbranch = ddlbranch.SelectedValue.ToString();

            ddlsection.Items.Clear();
            ds2.Dispose();
            ds2.Reset();
            ds2 = d2.BindSectionDetail(strbatch, strbranch);
            if (ds2.Tables[0].Rows.Count > 0)
            {
                ddlsection.DataSource = ds2;
                ddlsection.DataTextField = "sections";
                ddlsection.DataBind();
                ddlsection.Items.Insert(0, "All");
                if (Convert.ToString(ds2.Tables[0].Columns["sections"]) == string.Empty)
                {
                    ddlsection.Enabled = false;
                    ddlsection.Items.Insert(0, "All");
                }
                else
                {
                    ddlsection.Enabled = true;
                }
            }
            else
            {
                ddlsection.Enabled = false;
                ddlsection.Items.Insert(0, "All");
            }
        }
        catch (Exception ex)
        {
            norecordlbl.Text = ex.ToString();
        }

    }
    public void GetSubject()
    {
        string strsec = "";

        ddlsubject.Items.Clear();
        string sections = ddlsection.SelectedValue.ToString();
        strsec = "";
        if (ddlsection.Text.ToString() == "All" || ddlsection.Text.ToString() == "")
        {
            strsec = "";
        }
        else
        {
            strsec = " and exam_type.Sections='" + ddlsection.ToString() + "'";
        }
        string strsem = "";
        string strsem1 = "";
        string regsem = "";
        string sems = "";
        if (ddlsemester.SelectedValue != "")
        {
            if (ddlsemester.SelectedValue == "")
            {
                strsem = "";
                strsem1 = "";
                regsem = "";
                sems = "";
            }
            else
            {
                strsem = " and semester =" + ddlsemester.SelectedValue.ToString() + "";
                strsem1 = "and syllabus_master.semester=" + ddlsemester.SelectedValue.ToString() + "";
                regsem = " and registration.current_semester>=" + ddlsemester.SelectedValue.ToString() + "";
                sems = "and SM.semester=" + ddlsemester.SelectedValue.ToString() + "";
            }
            string Sqlstr = "select distinct S.subject_no,subject_code,subject_name,sem.subject_type from subject as S,syllabus_master  as SM,subjectchooser as SC,Sub_sem as Sem,staff_selector st where S.subject_no=SC.Subject_no and  s.syll_code=SM.syll_code and SM.degree_code=" + ddlbranch.SelectedValue.ToString() + " " + sems.ToString() + " and st.subject_no=s.subject_no  and  SM.batch_year='" + ddlbatch.SelectedValue.ToString() + "' and  S.subtype_no = Sem.subtype_no and promote_count=1  order by subject_code "; ;
            ds2 = d2.select_method(Sqlstr, hat, "Text");
            if (Sqlstr != "")
            {
                if (ds2.Tables[0].Rows.Count > 0)
                {
                    ddlsubject.DataSource = ds2;
                    ddlsubject.DataValueField = "Subject_No";
                    ddlsubject.DataTextField = "Subject_Name";
                    ddlsubject.DataBind();
                }
            }
        }
        else
        {
            ddlsubject.SelectedIndex = 0;
        }
    }
    protected void ddlbatch_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            norecordlbl.Visible = false;
            BindBranch(singleuser, group_user, course_id, collegecode, usercode);
            BindSem(strbranch, strbatchyear, collegecode);
            BindSectionDetail(strbatch, strbranch);
            GetSubject();
        }
        catch (Exception ex)
        {
            norecordlbl.Text = ex.ToString();
        }
    }
    protected void ddldegree_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {

            norecordlbl.Visible = false;
            BindBranch(singleuser, group_user, course_id, collegecode, usercode);
            BindSem(strbranch, strbatchyear, collegecode);
            BindSectionDetail(strbatch, strbranch);
            GetSubject();
        }
        catch (Exception ex)
        {
            norecordlbl.Text = ex.ToString();
        }
    }
    protected void ddlbranch_SelectedIndexChanged(object sender, EventArgs e)
    {
        divMainContents.Visible = false;
        Showgrid.Visible = false;
        norecordlbl.Visible = false;
        btnexcel.Visible = false;
        txtexcelname.Visible = false;
        lblrptname.Visible = false;
        btnprintmaster.Visible = false;
        btnPrint.Visible = false;
        if (!Page.IsPostBack == false)
        {
            ddlsemester.Items.Clear();
        }
        try
        {
            if ((ddlbranch.SelectedIndex.ToString() !=""))
            {
                BindSem(strbranch, strbatchyear, collegecode);
                BindSectionDetail(strbatch, strbranch);
                GetSubject();
            }

        }
        catch (Exception ex)
        {
            norecordlbl.Text = ex.ToString();
        }
    }

    protected void ddlsemester_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            divMainContents.Visible = false;
            norecordlbl.Visible = false;
            Showgrid.Visible = false;
            lblrptname.Visible = false;
            txtexcelname.Visible = false;
            btnexcel.Visible = false;
            btnprintmaster.Visible = false;
            btnPrint.Visible = false;
            if (!Page.IsPostBack == false)
            {
                ddlsection.Items.Clear();
            }
            BindSectionDetail(strbatch, strbranch);
            GetSubject();
        }
        catch (Exception ex)
        {
            norecordlbl.Text = ex.ToString();
        }
    }
    protected void ddlsection_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            GetSubject();

            lblrptname.Visible = false;
            txtexcelname.Visible = false;
            btnexcel.Visible = false;
            //*******modified by annyutha 2/9/2014*****//
            btnprintmaster.Visible = false;
            btnPrint.Visible = false;
            //**end****//
            norecordlbl.Visible = false;
            Showgrid.Visible = false;
            divMainContents.Visible = false;
        }
        catch (Exception ex)
        {
            norecordlbl.Text = ex.ToString();
        }
    }
    protected void txtsubject_TextChanged(object sender, EventArgs e)
    {
        Showgrid.Visible = false;
        btnexcel.Visible = false;
        lblrptname.Visible = false;
        txtexcelname.Visible = false;
        btnexcel.Visible = false;
        btnprintmaster.Visible = false;
        btnPrint.Visible = false;
        divMainContents.Visible = false;
    }
    protected void chksubject_CheckedChanged(object sender, EventArgs e)
    {
        if (chksubject.Checked == true)
        {
            foreach (System.Web.UI.WebControls.ListItem li in ddlsubject.Items)
            {
                li.Selected = true;
                txtsubject.Text = "Subject(" + (ddlsubject.Items.Count) + ")";
                Showgrid.Visible = false;
                btnexcel.Visible = false;
                lblrptname.Visible = false;
                txtexcelname.Visible = false;
                btnprintmaster.Visible = false;
                btnPrint.Visible = false;
                divMainContents.Visible = false;
            }
        }
        else
        {
            foreach (System.Web.UI.WebControls.ListItem li in ddlsubject.Items)
            {
                li.Selected = false;
                txtsubject.Text = "--Select--";
                Showgrid.Visible = false;
                btnexcel.Visible = false;
                lblrptname.Visible = false;
                txtexcelname.Visible = false;
                btnprintmaster.Visible = false;
                btnPrint.Visible = false;
                divMainContents.Visible = false;
            }
        }
    }
    protected void ddlsubject_SelectedIndexChanged(object sender, EventArgs e)
    {
        int selectcout = 0;
        for (int i = 0; i < ddlsubject.Items.Count; i++)
        {
            if (ddlsubject.Items[i].Selected == true)
            {
                selectcout = selectcout + 1;
            }

        }

        txtsubject.Text = "Subject(" + (selectcout) + ")";
        //ddlTest.SelectedIndex = -1;
    }
    protected void btnexcel_Click(object sender, EventArgs e)
    {
        try
        {
            string reportname = txtexcelname.Text;
            norecordlbl.Visible = false;
            if (reportname.ToString().Trim() != "")
            {
                d2.printexcelreportgrid(Showgrid, reportname);
            }
            else
            {
                norecordlbl.Text = "Please Enter Your Report Name";
                norecordlbl.Visible = true;
            }
        }
        catch
        {
        }

    }

    public override void VerifyRenderingInServerForm(Control control)
    { }

    protected void txttodate_TextChanged(object sender, EventArgs e)
    {
        if (txtfromdate.Text != "" && txttodate.Text != "")
        {
            string fromdate = string.Empty;
            fromdate = txtfromdate.Text.ToString();
            string[] splitfrom = fromdate.Split(new Char[] { '/' });
            int splitdatefrom = Convert.ToInt32(splitfrom[1]);
            int splitmonthfrom = Convert.ToInt32(splitfrom[0]);
            int splityearfrom = Convert.ToInt32(splitfrom[2]);


            string todate = string.Empty;
            todate = txttodate.Text.ToString();
            string[] splitto = todate.Split(new Char[] { '/' });

            int splitdate = Convert.ToInt32(splitto[1]);
            int splitmonth = Convert.ToInt32(splitto[0]);
            int splityear = Convert.ToInt32(splitto[2]);

            if (splityear > splityearfrom)
            {
                // errmsg.Visible = false;
            }
            else if (splityear == splityearfrom)
            {
                if (splitmonth > splitmonthfrom)
                {
                    //errmsg.Visible = false;
                }
                else if (splitmonth == splitmonthfrom)
                {
                    if (splitdate >= splitdatefrom)
                    {
                        //  errmsg.Visible = false;
                    }
                    else
                    {

                        txttodate.Text = "";
                    }
                }
                else
                {

                    txttodate.Text = "";
                }
            }
            else
            {

                txttodate.Text = "";
            }
        }
    }
    protected void btngo_Click(object sender, EventArgs e)
    {
        if (txttodate.Text != "" && txtfromdate.Text != "")
        {

            btnPrint11();
            string query = "";
            string fromdate = string.Empty;
            fromdate = txtfromdate.Text.ToString();
            string[] splitfrom = fromdate.Split(new Char[] { '/' });
            string splitdatefrom = Convert.ToString(splitfrom[0]);
            string splitmonthfrom = Convert.ToString(splitfrom[1]);
            string splityearfrom = Convert.ToString(splitfrom[2]);

            fromdate = splitmonthfrom + '/' + splitdatefrom + '/' + splityearfrom;

            string todate = string.Empty;
            todate = txttodate.Text.ToString();
            string[] splitto = todate.Split(new Char[] { '/' });
            string splitdateto = Convert.ToString(splitto[0]);
            string splitmonthto = Convert.ToString(splitto[1]);
            string splityearto = Convert.ToString(splitto[2]);

            todate = splitmonthto + '/' + splitdateto + '/' + splityearto;


            string subject = "";
            if (txtsubject.Text != "--Select--" || ddlsubject.Items.Count != null)
            {
                int itemcount = 0;


                for (itemcount = 0; itemcount < ddlsubject.Items.Count; itemcount++)
                {
                    if (ddlsubject.Items[itemcount].Selected == true)
                    {
                        if (subject == "")
                            subject = "'" + ddlsubject.Items[itemcount].Value.ToString() + "'";
                        else
                            subject = subject + "," + "'" + ddlsubject.Items[itemcount].Value.ToString() + "'";
                    }
                }
            }
            if (subject != "")
            {
                subject = " in(" + subject + ")";
                subject = " and shd.subject_no  " + subject + "";
            }
            else
            {
                subject = " ";
            }
            Showgrid.Visible = true;
            divMainContents.Visible = true;
            btnexcel.Visible = true;
            txtexcelname.Visible = true;
            lblrptname.Visible = true;

            btnprintmaster.Visible = true;
            btnPrint.Visible = true;

            Dictionary<int, string> dicsubjspan = new Dictionary<int, string>();
            int span = 0;
            ArrayList arrColHdrNames1 = new ArrayList();
            if (rbcount.Checked == true)
            {
                arrColHdrNames1.Add("S.No");
                data.Columns.Add("col0");
                arrColHdrNames1.Add("Subject");
                data.Columns.Add("col1");
                arrColHdrNames1.Add("Date");
                data.Columns.Add("col2");
                arrColHdrNames1.Add("No.of Periods");
                data.Columns.Add("col3");
                arrColHdrNames1.Add("Topics");
                data.Columns.Add("col4");

                DataRow drHdr1 = data.NewRow();
                for (int grCol = 0; grCol < data.Columns.Count; grCol++)
                    drHdr1["col" + grCol] = arrColHdrNames1[grCol];
                data.Rows.Add(drHdr1);

                //query = "select s.subject_name,shm.date,datediff (hour,shd.start_time ,shd.end_time ) as nohour,shd.topic_no from specialhr_details shd,specialhr_master shm,subject s where  shd.hrentry_no=shm.hrentry_no and shd.subject_no=s.subject_no and shm.degree_code=" + ddlbranch.SelectedValue.ToString() + " and shm.semester=" + ddlsemester.SelectedValue.ToString() + " and shm.batch_year=" + ddlbatch.SelectedValue.ToString() + "  " + subject + " and shm.date between '" + fromdate + "' and '" + todate + "' order by  shm.date";
                query = " select COUNT(s.subject_name) as periods, s.subject_name,shm.date,s.subject_no from   specialhr_details shd,specialhr_master shm,subject s where  shd.hrentry_no=shm.hrentry_no and shd.subject_no=s.subject_no and shm.degree_code=" + ddlbranch.SelectedValue.ToString() + " and shm.semester=" + ddlsemester.SelectedValue.ToString() + " and shm.batch_year=" + ddlbatch.SelectedValue.ToString() + "  " + subject + " and shm.date between '" + fromdate + "' and '" + todate + "' group by shm.date,s.subject_no,s.subject_name order by  shm.date";
                ds2 = d2.select_method(query, hat, "Text");

                int sno = 0;
                if (ds2.Tables[0].Rows.Count > 0)
                {
                    for (int rolcount = 0; rolcount < ds2.Tables[0].Rows.Count; rolcount++)
                    {
                        string date = ds2.Tables[0].Rows[rolcount]["date"].ToString();
                        string[] spiltdate = date.Split(new Char[] { ' ' });
                        string splitdate = Convert.ToString(spiltdate[0]);

                        string[] spiltexact = splitdate.Split(new Char[] { '/' });
                        string splitexactdate = spiltexact[1].ToString() + '/' + spiltexact[0].ToString() + '/' + spiltexact[2].ToString();
                        string subjectno = ds2.Tables[0].Rows[rolcount]["subject_no"].ToString();
                        sno++;
                        norecordlbl.Visible = false;
                        drow = data.NewRow();
                        data.Rows.Add(drow);

                        data.Rows[data.Rows.Count - 1][0] = Convert.ToString(sno);
                        data.Rows[data.Rows.Count - 1][1] = ds2.Tables[0].Rows[rolcount]["subject_name"].ToString();
                        data.Rows[data.Rows.Count - 1][2] = Convert.ToString(splitexactdate);
                        data.Rows[data.Rows.Count - 1][3] = ds2.Tables[0].Rows[rolcount]["periods"].ToString();

                        string topicname = "";
                        string topicquery = " select sd.topic_no from specialhr_master sm,specialhr_details sd where sm.hrentry_no=sd.hrentry_no and sm.date='" + splitdate + "' and sd.subject_no='" + subjectno + "'";
                        DataSet dstopic = d2.select_method_wo_parameter(topicquery, "Text");
                        for (int tpn = 0; tpn < dstopic.Tables[0].Rows.Count; tpn++)
                        {
                            // string unitname = Convert.ToString(ds2.Tables[0].Rows[rolcount]["topic_no"]);
                            string unitname = Convert.ToString(dstopic.Tables[0].Rows[tpn]["topic_no"].ToString());
                            string[] unitname1;
                            string unitnamespilt;
                            unitname1 = unitname.Split('/');
                            for (int i = 0; i <= unitname1.GetUpperBound(0); i++)
                            {
                                unitnamespilt = unitname1[i];
                                if (unitnamespilt != "" && unitnamespilt != null && unitnamespilt != " ")//condition Added by Manikandan 24/07/2013
                                {
                                    string gettopicname = d2.GetFunction("select unit_name from sub_unit_details where topic_no='" + unitnamespilt + "'");
                                    if (topicname == "")
                                    {
                                        topicname = gettopicname;
                                    }
                                    else
                                    {
                                        topicname = topicname + '/' + gettopicname;
                                    }
                                    //con1.Open();
                                    //SqlCommand cmdunit = new SqlCommand("select unit_name from sub_unit_details where topic_no='" + unitnamespilt + "'", con1);
                                    //SqlDataReader drunit = cmdunit.ExecuteReader();
                                    //if (drunit.Read())
                                    //{
                                    //    if (topicname == "")
                                    //    {
                                    //        topicname = drunit["unit_name"].ToString();
                                    //    }
                                    //    else
                                    //    {
                                    //        topicname = topicname + '/' + drunit["unit_name"].ToString();
                                    //    }
                                    //}
                                    //drunit.Close();
                                    //con1.Close();
                                }
                            }
                        }

                        data.Rows[data.Rows.Count - 1][4] = topicname;
                    }

                }
                else
                {
                    divMainContents.Visible = false;
                    norecordlbl.Visible = true;
                    Showgrid.Visible = false;
                    btnprintmaster.Visible = false;
                    btnPrint.Visible = false;
                    norecordlbl.Text = "No Records Found";
                    btnexcel.Visible = false;
                    lblrptname.Visible = false;
                    txtexcelname.Visible = false;
                }
            }

            else if (rbdetail.Checked == true)
            {
                dicsubjspan.Clear();
                norecordlbl.Visible = false;

                arrColHdrNames1.Add("S.No");
                data.Columns.Add("col0");
                arrColHdrNames1.Add("Date");
                data.Columns.Add("col1");
                arrColHdrNames1.Add("No.of Periods");
                data.Columns.Add("col2");
                arrColHdrNames1.Add("Topics");
                data.Columns.Add("col3");

                DataRow drHdr1 = data.NewRow();
                for (int grCol = 0; grCol < data.Columns.Count; grCol++)
                    drHdr1["col" + grCol] = arrColHdrNames1[grCol];
                data.Rows.Add(drHdr1);



                int intcrow = 0;
                string detailsubject = "";
                int sno = 0;
                //query = "select s.subject_name,shm.date,datediff (hour,shd.start_time ,shd.end_time ) as nohour,shd.topic_no from specialhr_details shd,specialhr_master shm,subject s where  shd.hrentry_no=shm.hrentry_no and shd.subject_no=s.subject_no and shm.degree_code=" + ddlbranch.SelectedValue.ToString() + " and shm.semester=" + ddlsemester.SelectedValue.ToString() + " and shm.batch_year=" + ddlbatch.SelectedValue.ToString() + "  " + subject + " and shm.date between '" + fromdate + "' and '" + todate + "' order by  shd.subject_no";
                query = "select COUNT(s.subject_name) as periods,s.subject_no,s.subject_name,shm.date from specialhr_details shd,specialhr_master shm,subject s where  shd.hrentry_no=shm.hrentry_no and shd.subject_no=s.subject_no and shm.degree_code=" + ddlbranch.SelectedValue.ToString() + " and shm.semester=" + ddlsemester.SelectedValue.ToString() + " and shm.batch_year=" + ddlbatch.SelectedValue.ToString() + "  " + subject + " and shm.date between '" + fromdate + "' and '" + todate + "' group by s.subject_no,s.subject_name,shm.date order by shm.date,s.subject_no,s.subject_name";
                ds2 = d2.select_method(query, hat, "Text");

                con.Open();
                if (ds2.Tables[0].Rows.Count > 0)
                {
                    for (int rolcount = 0; rolcount < ds2.Tables[0].Rows.Count; rolcount++)
                    {


                        string topicname = "";
                        string date = ds2.Tables[0].Rows[rolcount]["date"].ToString();
                        string[] spiltdate = date.Split(new Char[] { ' ' });
                        string splitdate = Convert.ToString(spiltdate[0]);

                        string[] spiltexact = splitdate.Split(new Char[] { '/' });
                        string splitexactdate = spiltexact[1].ToString() + '/' + spiltexact[0].ToString() + '/' + spiltexact[2].ToString();
                        string subjectno = ds2.Tables[0].Rows[rolcount]["subject_no"].ToString();
                        string topicquery = " select sd.topic_no from specialhr_master sm,specialhr_details sd where sm.hrentry_no=sd.hrentry_no and sm.date='" + splitdate + "' and sd.subject_no='" + subjectno + "'";
                        DataSet dstopic = d2.select_method_wo_parameter(topicquery, "Text");
                        for (int tpn = 0; tpn < dstopic.Tables[0].Rows.Count; tpn++)
                        {
                            //string unitname = ds2.Tables[0].Rows[rolcount]["topic_no"].ToString();
                            string unitname = dstopic.Tables[0].Rows[tpn]["topic_no"].ToString();
                            string[] unitname1;
                            string unitnamespilt;
                            if (unitname.ToString().Trim() != "")
                            {
                                unitname1 = unitname.Split('/');
                                for (int i = 0; i <= unitname1.GetUpperBound(0); i++)
                                {
                                    unitnamespilt = unitname1[i];
                                    string gettopicname = d2.GetFunction("select unit_name from sub_unit_details where topic_no='" + unitnamespilt + "'");
                                    if (topicname == "")
                                    {
                                        topicname = gettopicname;
                                    }
                                    else
                                    {
                                        topicname = topicname + '/' + gettopicname;
                                    }
                                    //con1.Open();
                                    //SqlCommand cmdunit = new SqlCommand("select unit_name from sub_unit_details where topic_no='" + unitnamespilt + "'", con1);
                                    //SqlDataReader drunit = cmdunit.ExecuteReader();
                                    //if (drunit.Read())
                                    //{
                                    //    if (topicname == "")
                                    //    {
                                    //        topicname = drunit["unit_name"].ToString();
                                    //    }
                                    //    else
                                    //    {
                                    //        topicname = topicname + '/' + drunit["unit_name"].ToString();
                                    //    }
                                    //}
                                    //drunit.Close();
                                    //con1.Close();
                                }
                            }
                        }


                        if (detailsubject != Convert.ToString(ds2.Tables[0].Rows[rolcount]["subject_name"]))
                        {

                            drow = data.NewRow();
                            data.Rows.Add(drow);
                            data.Rows[data.Rows.Count - 1][0] = ds2.Tables[0].Rows[rolcount]["subject_name"].ToString();

                            dicsubjspan.Add(data.Rows.Count - 1, ds2.Tables[0].Rows[rolcount]["subject_name"].ToString());

                        }
                        sno++;

                        drow = data.NewRow();
                        data.Rows.Add(drow);
                        data.Rows[data.Rows.Count - 1][0] = Convert.ToString(sno);
                        data.Rows[data.Rows.Count - 1][1] = Convert.ToString(splitexactdate);
                        data.Rows[data.Rows.Count - 1][2] = ds2.Tables[0].Rows[rolcount]["periods"].ToString();
                        data.Rows[data.Rows.Count - 1][3] = topicname;


                        detailsubject = ds2.Tables[0].Rows[rolcount]["subject_name"].ToString();


                    }
                }
                else
                {
                    norecordlbl.Visible = true;
                    Showgrid.Visible = false;
                    btnprintmaster.Visible = false;
                    btnPrint.Visible = false;
                    norecordlbl.Text = "No Records Found";
                    btnexcel.Visible = false;
                    lblrptname.Visible = false;
                    txtexcelname.Visible = false;
                    divMainContents.Visible = false;
                }

            }
            if (data.Columns.Count > 0 && data.Rows.Count > 1)//===========on 9/4/12
            {
                divMainContents.Visible = true;
                Showgrid.DataSource = data;
                Showgrid.DataBind();
                Showgrid.Visible = true;
                Showgrid.Width = 500;
                if (Showgrid.Rows.Count > 0)
                {
                    Showgrid.Rows[0].BackColor = ColorTranslator.FromHtml("#0CA6CA");
                    Showgrid.Rows[0].Font.Bold = true;
                    if (dicsubjspan.Count > 0)
                    {
                        foreach (KeyValuePair<int, string> dr in dicsubjspan)
                        {
                            int rowcnt = dr.Key;

                            int d = Convert.ToInt32(data.Columns.Count);
                            Showgrid.Rows[rowcnt].Cells[0].HorizontalAlign = HorizontalAlign.Center;
                            Showgrid.Rows[rowcnt].Cells[0].Font.Bold = true;
                            Showgrid.Rows[rowcnt].Cells[0].ColumnSpan = d;
                            for (int a = 1; a < d; a++)
                            {
                                Showgrid.Rows[rowcnt].Cells[a].Visible = false;
                            }
                        }
                    }

                }
            }
            else
            {
                divMainContents.Visible = false;
                norecordlbl.Visible = true;
                Showgrid.Visible = false;
                btnprintmaster.Visible = false;
                btnPrint.Visible = false;
                norecordlbl.Text = "No Records Found";
                btnexcel.Visible = false;
                lblrptname.Visible = false;
                txtexcelname.Visible = false;
            }


        }
        else
        {
            divMainContents.Visible = false;
            norecordlbl.Visible = true;
            Showgrid.Visible = false;
            btnprintmaster.Visible = false;
            btnPrint.Visible = false;
            btnexcel.Visible = false;
            lblrptname.Visible = false;
            txtexcelname.Visible = false;
            norecordlbl.Text = "Please Enter From and To Date";
        }
    }

    public void btnPrint11()
    {
        DAccess2 ddd2 = new DAccess2();
        string college_code = Convert.ToString(Session["collegecode"].ToString());
        string colQ = "select * from collinfo where college_code='" + college_code + "'";
        DataSet dsCol = new DataSet();
        dsCol = ddd2.select_method_wo_parameter(colQ, "Text");
        string collegeName = string.Empty;
        string collegeCateg = string.Empty;
        string collegeAff = string.Empty;
        string collegeAdd = string.Empty;
        string collegePhone = string.Empty;
        string collegeFax = string.Empty;
        string collegeWeb = string.Empty;
        string collegeEmai = string.Empty;
        string collegePin = string.Empty;
        string acr = string.Empty;
        string City = string.Empty;
        if (dsCol.Tables.Count > 0 && dsCol.Tables[0].Rows.Count > 0)
        {
            collegeName = Convert.ToString(dsCol.Tables[0].Rows[0]["Collname"]);
            City = Convert.ToString(dsCol.Tables[0].Rows[0]["address3"]);
            collegeAff = "(Affiliated to " + Convert.ToString(dsCol.Tables[0].Rows[0]["university"]) + ")";
            collegeAdd = Convert.ToString(dsCol.Tables[0].Rows[0]["address1"]) + " , " + Convert.ToString(dsCol.Tables[0].Rows[0]["address2"]) + " , " + Convert.ToString(dsCol.Tables[0].Rows[0]["district"]) + " - " + Convert.ToString(dsCol.Tables[0].Rows[0]["pincode"]);
            collegePin = Convert.ToString(dsCol.Tables[0].Rows[0]["pincode"]);
            collegePhone = "OFFICE: " + Convert.ToString(dsCol.Tables[0].Rows[0]["phoneno"]);
            collegeFax = "FAX: " + Convert.ToString(dsCol.Tables[0].Rows[0]["faxno"]);
            collegeWeb = "Website: " + Convert.ToString(dsCol.Tables[0].Rows[0]["website"]);
            collegeEmai = "E-Mail: " + Convert.ToString(dsCol.Tables[0].Rows[0]["email"]);
            collegeCateg = "(" + Convert.ToString(dsCol.Tables[0].Rows[0]["category"]) + ")";
        }
        DateTime dt = DateTime.Now;
        int year = dt.Year;
        spCollegeName.InnerHtml = collegeName;
        spAddr.InnerHtml = collegeAdd;
        spDegreeName.InnerHtml = acr;
        spReportName.InnerHtml = "Special Hour Report";
        // spSection.InnerHtml ="Satff: "+ Convert.ToString(ddlSearchOption.SelectedItem.Text);


    }

    protected void Showgrid_OnRowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {
            if (e.Row.RowType == DataControlRowType.Header)
            {
                for (int grCol = 0; grCol < data.Columns.Count; grCol++)
                    e.Row.Cells[grCol].Visible = false;

            }
            if (e.Row.RowType == DataControlRowType.DataRow)
            {

                for (int j = 0; j < data.Columns.Count; j++)
                    e.Row.Cells[j].HorizontalAlign = HorizontalAlign.Center;
            }



        }
        catch
        {
        }
    }

    protected void btnprintmaster_Click(object sender, EventArgs e)
    {

        // Session["column_header_row_count"] = Convert.ToString(FpSpread1.ColumnHeader.RowCount);
        if (rbdetail.Checked == true)
        {
            // FpSpread1.Sheets[0].ColumnHeader.Cells[0, 3].HorizontalAlign = HorizontalAlign.Center;
        }
        else if (rbcount.Checked == true)
        {
            // FpSpread1.Sheets[0].ColumnHeader.Cells[0, 4].HorizontalAlign = HorizontalAlign.Center;
        }
        string ss = null;
        string degreedetails = "Special Hour Report " + "@Batch : " + ddlbatch.SelectedItem.Text.ToString() + "-" + ddldegree.SelectedItem.Text.ToString() + "[ " + ddlbranch.SelectedItem.Text.ToString() + " ]" + "-" + ddlsemester.SelectedItem.Text.ToString() + "-" + ddlsection.SelectedItem.Text.ToString() + "@Date : " + txtfromdate.Text.ToString() + " - " + txttodate.Text.ToString();
        string pagename = "Specialhourreport.aspx";
        Printcontrol.loadspreaddetails(Showgrid, pagename, degreedetails,0,ss);
        Printcontrol.Visible = true;
    }


}
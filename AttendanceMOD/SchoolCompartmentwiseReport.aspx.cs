using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Text;
using System.Collections;
using System.Drawing;
public partial class SchoolCompartmentwiseReport : System.Web.UI.Page
{
    DataSet ds = new DataSet();
    DAccess2 d2 = new DAccess2();
    Hashtable hat = new Hashtable();
    string usercode = string.Empty;
    string collegecode = string.Empty;
    string singleuser = string.Empty;
    string group_user = string.Empty;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["collegecode"] == null)
        {
            Response.Redirect("~/Default.aspx");
        }
        usercode = Session["usercode"].ToString();
        // collegecode1 = Session["collegecode"].ToString();
        singleuser = Session["single_user"].ToString();
        group_user = Session["group_code"].ToString();
        if (!IsPostBack)
        {
            setLabelText();
            loadcollege();
            if (ddlcollege.Items.Count > 0)
            {
                collegecode = Convert.ToString(ddlcollege.SelectedItem.Value);
            }
            bindbatch();
            binddeg();
            binddept();
            loadLeave();
            txt_fromdate.Text = DateTime.Now.ToString("dd/MM/yyyy");
            txt_fromdate.Attributes.Add("readonly", "readonly");
            lblCount.Visible = false;
        }
        if (ddlcollege.Items.Count > 0)
        {
            collegecode = Convert.ToString(ddlcollege.SelectedItem.Value);
        }
    }

    #region college

    //public void loadcollege()
    //{
    //    try
    //    {
    //        ddlcollege.Items.Clear();
    //        ds.Clear();
    //        ds = d2.BindCollege();
    //        if (ds.Tables[0].Rows.Count > 0)
    //        {
    //            ddlcollege.DataSource = ds;
    //            ddlcollege.DataTextField = "collname";
    //            ddlcollege.DataValueField = "college_code";
    //            ddlcollege.DataBind();
    //        }
    //    }
    //    catch
    //    { }
    //}

    public void loadcollege()
    {
        try
        {
            string columnfield = string.Empty;
            group_user = Session["group_code"].ToString();
            if (group_user.Contains(';'))
            {
                string[] group_semi = group_user.Split(';');
                group_user = group_semi[0].ToString();
            }
            if ((group_user.ToString().Trim() != "") && (Session["single_user"].ToString() != "1" && Session["single_user"].ToString() != "true" && Session["single_user"].ToString() != "TRUE" && Session["single_user"].ToString() != "True"))
            {
                columnfield = " and group_code='" + group_user + "'";
            }
            else
            {
                columnfield = " and user_code='" + Session["usercode"] + "'";
            }
            hat.Clear();
            hat.Add("column_field", columnfield.ToString());
            DataSet dsprint = d2.select_method("bind_college", hat, "sp");
            ddlcollege.Items.Clear();
            if (dsprint.Tables[0].Rows.Count > 0)
            {
                ddlcollege.DataSource = dsprint;
                ddlcollege.DataTextField = "collname";
                ddlcollege.DataValueField = "college_code";
                ddlcollege.DataBind();
            }
            else
            {
                //errmsg.Text = "Set college rights to the staff";
                //errmsg.Visible = true;
                //return;
            }
        }
        catch (Exception ex)
        {
            //errmsg.Text = ex.ToString();
            //errmsg.Visible = true;
        }
    }

    protected void ddlcollege_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddlcollege.Items.Count > 0)
        {
            collegecode = Convert.ToString(ddlcollege.SelectedItem.Value);
            binddeg();
            binddept();
            loadLeave();
            lblCount.Visible = false;
        }
    }

    #endregion

    #region degree

    public void binddeg()
    {
        try
        {
            cbl_degree.Items.Clear();
            cb_degree.Checked = false;
            txt_degree.Text = "---Select---";
            cbl_degree.Items.Clear();
            ds.Clear();
            string selqry = "select distinct  c.Course_Name,c.Course_Id  from Degree d,Department dt,Course c where d.Dept_Code =dt.Dept_Code and d.Course_Id =c.Course_Id  and d.college_code='" + collegecode + "'";
            ds = d2.select_method_wo_parameter(selqry, "Text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                cbl_degree.DataSource = ds;
                cbl_degree.DataTextField = "course_name";
                cbl_degree.DataValueField = "course_id";
                cbl_degree.DataBind();
                if (cbl_degree.Items.Count > 0)
                {
                    for (int i = 0; i < cbl_degree.Items.Count; i++)
                    {
                        cbl_degree.Items[i].Selected = true;
                    }
                    txt_degree.Text = lbldeg.Text + "(" + cbl_degree.Items.Count + ")";
                    cb_degree.Checked = true;
                }
            }
        }
        catch { }
    }

    protected void cb_degree_OnCheckedChanged(object sender, EventArgs e)
    {
        try
        {
            CallCheckboxChange(cb_degree, cbl_degree, txt_degree, lbldeg.Text, "--Select--");
            binddept();
            lblCount.Visible = false;
        }
        catch { }
    }

    protected void cbl_degree_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            CallCheckboxListChange(cb_degree, cbl_degree, txt_degree, lbldeg.Text, "--Select--");
            binddept();
            lblCount.Visible = false;
        }
        catch { }
    }

    #endregion

    #region batch
    public void bindbatch()
    {
        cbl_batch.Items.Clear();
        cb_batch.Checked = false;
        txt_batch.Text = "---Select---";
        string batch = string.Empty;
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
                    batch += "," + Convert.ToString(cbl_batch.Items[i].Value);
                }
            }
        }
        ds.Clear();
        ds = d2.select_method_wo_parameter("bind_batch", "sp");
        int count = ds.Tables[0].Rows.Count;
        if (count > 0)
        {
            cbl_batch.DataSource = ds;
            cbl_batch.DataTextField = "batch_year";
            cbl_batch.DataValueField = "batch_year";
            cbl_batch.DataBind();
            if (cbl_batch.Items.Count > 0)
            {
                for (int i = 0; i < cbl_batch.Items.Count; i++)
                {
                    cbl_batch.Items[i].Selected = true;
                }
                txt_batch.Text = lbl_batch.Text + "(" + cbl_batch.Items.Count + ")";
                cb_batch.Checked = true;
            }
        }
       
    }

    protected void cb_batch_OnCheckedChanged(object sender, EventArgs e)
    {
        try
        {
            CallCheckboxChange(cb_batch, cbl_batch, txt_batch, lbl_batch.Text, "--Select--");
            lblCount.Visible = false;
        }
        catch { }
    }

    protected void cbl_batch_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            CallCheckboxListChange(cb_batch, cbl_batch, txt_batch, "Batch", "--Select--");
            lblCount.Visible = false;
        }
        catch { }
    }
    #endregion

    #region dept

    public void binddept()
    {
        try
        {
            cbl_dept.Items.Clear();
            cb_dept.Checked = false;
            txt_dept.Text = "---Select---";
            string degree = string.Empty;
            for (int i = 0; i < cbl_degree.Items.Count; i++)
            {
                if (cbl_degree.Items[i].Selected == true)
                {
                    if (degree == "")
                    {
                        degree = Convert.ToString(cbl_degree.Items[i].Value);
                    }
                    else
                    {
                        degree += "," + Convert.ToString(cbl_degree.Items[i].Value);
                    }
                }
            }
            if (degree != "")
            {
                ds.Clear();
                ds = d2.BindBranchMultiple(singleuser, group_user, degree, collegecode, usercode);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    cbl_dept.DataSource = ds;
                    cbl_dept.DataTextField = "dept_name";
                    cbl_dept.DataValueField = "degree_code";
                    cbl_dept.DataBind();
                    if (cbl_dept.Items.Count > 0)
                    {
                        for (int i = 0; i < cbl_dept.Items.Count; i++)
                        {
                            cbl_dept.Items[i].Selected = true;
                        }
                        txt_dept.Text = lbldept.Text + "(" + cbl_dept.Items.Count + ")";
                        cb_dept.Checked = true;
                    }
                }
            }
        }
        catch { }
    }

    protected void cb_dept_OnCheckedChanged(object sender, EventArgs e)
    {
        try
        {
            CallCheckboxChange(cb_dept, cbl_dept, txt_dept, lbldept.Text, "--Select--");
            lblCount.Visible = false;
        }
        catch { }
    }

    protected void cbl_dept_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            CallCheckboxListChange(cb_dept, cbl_dept, txt_dept, "Department", "--Select--");
            lblCount.Visible = false;
        }
        catch { }
    }

    #endregion

    #region Leave

    public void loadLeave()
    {
        try
        {
            cblleve.Items.Clear();
            txtleve.Text = "--Select--";
            string selqry = "select leavecode,disptext from attmastersetting where collegecode='" + collegecode + "'";
            ds = d2.select_method_wo_parameter(selqry, "Text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                cblleve.DataSource = ds;
                cblleve.DataTextField = "disptext";
                cblleve.DataValueField = "leavecode";
                cblleve.DataBind();
                if (cblleve.Items.Count > 0)
                {
                    for (int i = 0; i < cblleve.Items.Count; i++)
                    {
                        cblleve.Items[i].Selected = true;
                    }
                    txtleve.Text = "Leave(" + cblleve.Items.Count + ")";
                    cbleve.Checked = true;
                }
            }
        }
        catch { }
    }

    protected void cbleve_OnCheckedChanged(object sender, EventArgs e)
    {
        try
        {
            CallCheckboxChange(cbleve, cblleve, txtleve, "Leave", "--Select--");
        }
        catch { }
    }

    protected void cblleve_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            CallCheckboxListChange(cbleve, cblleve, txtleve, "Leave", "--Select--");
        }
        catch { }
    }

    #endregion

    protected void rbdeg_Changed(object sender, EventArgs e)
    {
    }

    protected void rbdept_Changed(object sender, EventArgs e)
    {
    }

    protected void btngo_Click(object sender, EventArgs e)
    {
        try
        {
            lblCount.Visible = false;
            ds.Clear();
            ds = loadDatasetval();
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                loadGridDetail(ds);
            }
            else
            {
                gdattrpt.Visible = false;
                btnExport.Visible = false;
                Response.Write("<script>alert('No Record Found')</script>");
            }
            //lblCount.Visible = false;
            //string deptcode = getCblSelectedValue(cbl_dept);
            //string stuCount = "select count(r.roll_no) as SecWiseStrength from registration r,degree d,Course c,department dt where r.degree_code =d.degree_code and c.course_id =d.course_id and d.dept_code = dt.dept_code and d.college_code ='" + collegecode + "' and  cc=0 and exam_flag <> 'DEBAR' and delflag=0  and d.degree_code in('" + deptcode + "')";
            //DataSet dtstuCount = d2.select_method_wo_parameter(stuCount,"text");
            //if (dtstuCount.Tables[0].Rows.Count > 0)
            //{
            //    lblCount.Text = "Total Strenth of the Students:" + " " +Convert.ToString(dtstuCount.Tables[0].Rows[0]["SecWiseStrength"]);
            //    lblCount.Visible = true;
            //}
        }
        catch { }
    }

    protected DataSet loadDatasetval()
    {
        DataSet dsload = new DataSet();
        try
        {
            string SelQ = string.Empty;
            if (ddlcollege.Items.Count > 0)
                collegecode = Convert.ToString(ddlcollege.SelectedItem.Value);
            string batchyear = getCblSelectedValue(cbl_batch);//magesh 5.3.18
            string deptcode = getCblSelectedValue(cbl_dept);
            string levecode = getCblSelectedValue(cblleve);
            string curYear = DateTime.Now.ToString("yyyy");
            string date = Convert.ToString(txt_fromdate.Text);
            string[] frdate = date.Split('/');
            if (frdate.Length == 3)
                date = frdate[1].ToString() + "/" + frdate[0].ToString() + "/" + frdate[2].ToString();
            string mnthYear = Convert.ToString((Convert.ToInt32(curYear) * 12) + Convert.ToInt32(frdate[1]));
            //if (true)
            //{
            //    SelQ = "  select count(r.roll_no) as cnt,C.course_id as code,C.Course_name as name from registration r,degree d,Course c where r.degree_code =d.degree_code and c.course_id =d.course_id and d.college_code ='" + collegecode + "' group by C.course_id,C.Course_name";
            //    SelQ += " select count(al.appno) as cnt,c.course_id ,mleavecode,eleavecode  from AllStudentAttendanceReport al, registration r,degree d,Course c where al.appno=r.app_no and r.degree_code =d.degree_code and c.course_id =d.course_id and d.college_code ='" + collegecode + "' and al.DateofAttendance='" + date + "'  group by c.course_id ,mleavecode,eleavecode  ";
            //}
            //else
            //{
            SelQ = " select count(r.roll_no) as cnt,C.course_id,C.Course_name,d.degree_code ,dt.dept_name as name,r.Batch_year from registration r,degree d,Course c,department dt where r.degree_code =d.degree_code and c.course_id =d.course_id and d.dept_code = dt.dept_code and d.college_code ='" + collegecode + "' and  cc=0 and exam_flag <> 'DEBAR' and delflag=0"; // and r.batch_year=2016 ";
            if (deptcode != "" && batchyear!="")
                SelQ += "and r.Batch_year in('" + batchyear + "') and d.degree_code in('" + deptcode + "')";
            SelQ += " group by C.course_id,C.Course_name,d.degree_code,dt.dept_name,r.Batch_year order by d.degree_code";
            SelQ += " select distinct C.course_id,C.Course_name,d.degree_code ,dt.dept_name as name,(dept_name+'-'+ LTRIM(RTRIM(isnull(r.sections,'')))) as section,(cast(r.degree_code as varchar(10))+'-'+ LTRIM(RTRIM(isnull(r.sections,'')))) as sections,r.Batch_year  from registration r,degree d,Course c,department dt where r.degree_code =d.degree_code and c.course_id =d.course_id and d.dept_code = dt.dept_code and d.college_code ='" + collegecode + "' and  cc=0 and exam_flag <> 'DEBAR' and delflag=0"; // and r.batch_year=2016 ";  //modified by Mullai
            if (deptcode != "" && batchyear != "")
                SelQ += " and r.Batch_year in('" + batchyear + "') and d.degree_code in('" + deptcode + "')";
            //SelQ += " select count(al.appno) as cnt,c.course_id,d.degree_code,(dept_name+'-'+ isnull(sections,'')) as sections ,mleavecode,eleavecode  from AllStudentAttendanceReport al, registration r,degree d,Course c,department dt where al.appno=r.app_no and r.degree_code =d.degree_code and c.course_id =d.course_id and d.dept_code = dt.dept_code and d.college_code ='" + collegecode + "' and r.batch_year=2016 and al.DateofAttendance='" + date + "'";
            //if (deptcode != "")
            //    SelQ += " and d.degree_code in('" + deptcode + "')";
            //SelQ += "  group by c.course_id,d.degree_code,sections ,mleavecode,eleavecode,dept_name ";//LTRIM(RTRIM(isnull(sections,'')))
            SelQ += " select count(al.appno) as cnt,r.degree_code,(cast(r.degree_code as varchar(10))+'-'+ LTRIM(RTRIM(isnull(r.sections,'')))) as sections ,mleavecode,eleavecode,r.Batch_year  from AllStudentAttendanceReport al, registration r where al.appno=r.app_no and r.college_code ='" + collegecode + "' and al.DateofAttendance='" + date + "'"; //and r.batch_year=2016 
            if (deptcode != "" && batchyear != "")
                SelQ += " and r.Batch_year in('" + batchyear + "') and r.degree_code in('" + deptcode + "')";
            SelQ += "  group by r.degree_code,LTRIM(RTRIM(isnull(r.sections,''))) ,mleavecode,eleavecode,r.Batch_year";
            SelQ = SelQ + " select d.Degree_Code,(c.Course_Name +'-'+ dt.Dept_Name) as degreename,(c.Course_Name +'-'+dt.dept_acronym)as dept_acronym,dept_name from Degree d,Department dt,Course c where d.Dept_Code =dt.Dept_Code and c.Course_Id =d.Course_Id and d.college_code ='" + collegecode + "'";
            SelQ += " select count(r.roll_no) as SecWiseStrength,C.course_id,C.Course_name,d.degree_code ,dt.dept_name as name,(cast(d.degree_code as varchar(10))+'-'+ LTRIM(RTRIM(isnull(r.sections,'')))) as sections,r.Batch_year from registration r,degree d,Course c,department dt where r.degree_code =d.degree_code and c.course_id =d.course_id and d.dept_code = dt.dept_code and d.college_code ='" + collegecode + "' and  cc=0 and exam_flag <> 'DEBAR' and delflag=0 "; // and r.batch_year=2016 ";
            if (deptcode != "" && batchyear != "")
                SelQ += " and r.Batch_year in('" + batchyear + "') and d.degree_code in('" + deptcode + "')";
            SelQ += " group by C.course_id,C.Course_name,d.degree_code,dt.dept_name,LTRIM(RTRIM(isnull(r.sections,''))),r.Batch_year order by d.degree_code";
            //}
            dsload = d2.select_method_wo_parameter(SelQ, "Text");
        }
        catch { }
        return dsload;
    }

    protected void loadGridDetail(DataSet ds)
    {
        try
        {
            DataTable dtstud = new DataTable();
            DataView dvsec = new DataView();
            DataView dvfull = new DataView();
            DataView dvfst = new DataView();
            DataView dvsnd = new DataView();
            ArrayList arcol = new ArrayList();
            ArrayList arcolCrs = new ArrayList();
            Dictionary<string, double> FnlattCount = new Dictionary<string, double>();
            Dictionary<string, int> dictcol = new Dictionary<string, int>();
            double totStudCnt = 0;
            string compname = string.Empty;
            dtstud.Columns.Add("Sno");
            dtstud.Columns.Add(lbldeg.Text);
            dtstud.Columns.Add("Strength");
            dtstud.Columns.Add("Section Strength");
            for (int col = 0; col < cblleve.Items.Count; col++)
            {
                if (cblleve.Items[col].Selected)
                {
                    dtstud.Columns.Add(cblleve.Items[col].Text);
                    dtstud.Columns.Add(cblleve.Items[col].Text + "%");
                }
            }
            dtstud.Columns.Add("Leave Category(%)");
            int serialNo = 0;
            for (int dsf = 0; dsf < ds.Tables[0].Rows.Count; dsf++)
            {
                double.TryParse(Convert.ToString(ds.Tables[0].Rows[dsf]["cnt"]), out totStudCnt);
                // compname = Convert.ToString(ds.Tables[0].Rows[dsf]["name"]);
                if (!arcol.Contains(Convert.ToString(ds.Tables[0].Rows[dsf]["course_id"])))
                {
                    DataRow drstud;
                    drstud = dtstud.NewRow();
                    drstud["Sno"] = Convert.ToString(ds.Tables[0].Rows[dsf]["Course_name"]);
                    drstud["Strength"] = "Course";
                    dtstud.Rows.Add(drstud);
                    arcol.Add(Convert.ToString(ds.Tables[0].Rows[dsf]["course_id"]));
                    dictcol.Add(Convert.ToString(ds.Tables[0].Rows[dsf]["Course_name"]), Convert.ToInt32(dtstud.Rows.Count - 1));
                }
                double totLveCnt = 0;
                double lveCnt = 0;
                double LvePer = 0;
                if (ds.Tables.Count > 1 && ds.Tables[1].Rows.Count > 0)
                {
                    ds.Tables[1].DefaultView.RowFilter = "degree_code='" + Convert.ToString(ds.Tables[0].Rows[dsf]["degree_code"]) + "'";
                    dvsec = ds.Tables[1].DefaultView;
                    if (dvsec.Count > 0)
                    {
                        Dictionary<string, double> dicCategoryWisePercentage = new Dictionary<string, double>();
                        for (int dvs = 0; dvs < dvsec.Count; dvs++)
                        {
                            double tottempperc = 0;
                            if (!arcol.Contains(Convert.ToString(dvsec[dvs]["degree_code"])))
                            {
                                DataRow drstud;
                                drstud = dtstud.NewRow();
                                drstud["Sno"] = Convert.ToString(ds.Tables[0].Rows[dsf]["name"]);
                                dtstud.Rows.Add(drstud);
                                arcol.Add(Convert.ToString(dvsec[dvs]["degree_code"]));
                                dictcol.Add(Convert.ToString(ds.Tables[0].Rows[dsf]["name"]), Convert.ToInt32(dtstud.Rows.Count - 1));
                            }

                            #region

                            int totalSecWiseStrength = 0;
                            object sectionWiseStudents = 0;
                            DataTable dtSecWiseStrength = new DataTable();
                            if (ds.Tables.Count > 4 && ds.Tables[4].Rows.Count > 0)
                            {
                                ds.Tables[4].DefaultView.RowFilter = "degree_code='" + Convert.ToString(ds.Tables[0].Rows[dsf]["degree_code"]) + "' and sections='" + Convert.ToString(dvsec[dvs]["sections"]).Trim() + "'";
                                dtSecWiseStrength = ds.Tables[4].DefaultView.ToTable();
                                sectionWiseStudents = dtSecWiseStrength.Compute("SUM(SecWiseStrength)", string.Empty);
                                int.TryParse(Convert.ToString(sectionWiseStudents), out totalSecWiseStrength);
                            }

                            DataRow drsstud;
                            drsstud = dtstud.NewRow();
                            serialNo++;
                            drsstud["Sno"] = Convert.ToString(serialNo);
                            drsstud[lbldeg.Text] = Convert.ToString(dvsec[dvs]["section"]);
                            drsstud["Strength"] = Convert.ToString(totStudCnt);
                            drsstud["Section Strength"] = Convert.ToString(totalSecWiseStrength).Trim();
                            dtstud.Rows.Add(drsstud);
                            for (int col = 0; col < cblleve.Items.Count; col++)
                            {
                                double TempStudCnt = 0;
                                double perCnt = 0;
                                if (cblleve.Items[col].Selected)
                                {
                                    string colname = cblleve.Items[col].Text;
                                    double TempCnt = 0;
                                    double fstCnt = 0;
                                    double sndCnt = 0;
                                    ds.Tables[2].DefaultView.RowFilter = "Degree_code='" + Convert.ToString(dvsec[dvs]["Degree_code"]) + "' and sections='" + Convert.ToString(dvsec[dvs]["sections"]) + "'  and  mleavecode='" + Convert.ToString(cblleve.Items[col].Value) + "' and eleavecode='" + Convert.ToString(cblleve.Items[col].Value) + "' ";
                                    dvfull = ds.Tables[2].DefaultView;
                                    if (dvfull.Count > 0)
                                    {
                                        double.TryParse(Convert.ToString(dvfull[0]["cnt"]), out TempCnt);
                                    }
                                    ds.Tables[2].DefaultView.RowFilter = "Degree_code='" + Convert.ToString(dvsec[dvs]["Degree_code"]) + "' and sections='" + Convert.ToString(dvsec[dvs]["sections"]) + "' and mleavecode='" + Convert.ToString(cblleve.Items[col].Value) + "' and eleavecode<>'" + Convert.ToString(cblleve.Items[col].Value) + "' ";
                                    dvfst = ds.Tables[2].DefaultView;
                                    if (dvfst.Count > 0)
                                    {
                                        double.TryParse(Convert.ToString(dvfst[0]["cnt"]), out fstCnt);
                                        TempStudCnt += fstCnt;
                                    }
                                    ds.Tables[2].DefaultView.RowFilter = "Degree_code='" + Convert.ToString(dvsec[dvs]["Degree_code"]) + "' and sections='" + Convert.ToString(dvsec[dvs]["sections"]) + "' and mleavecode<>'" + Convert.ToString(cblleve.Items[col].Value) + "' and eleavecode='" + Convert.ToString(cblleve.Items[col].Value) + "' ";
                                    dvsnd = ds.Tables[2].DefaultView;
                                    if (dvsnd.Count > 0)
                                    {
                                        double.TryParse(Convert.ToString(dvsnd[0]["cnt"]), out sndCnt);
                                        TempStudCnt += sndCnt;
                                    }
                                    if (TempStudCnt != 0 || TempStudCnt != 0.0)
                                        TempCnt += TempStudCnt / 2;
                                    perCnt = (TempCnt / totalSecWiseStrength) * 100;
                                    drsstud[colname] = Convert.ToString(TempCnt);
                                    string colper = colname + "%";
                                    drsstud[colper] = Convert.ToString(Math.Round(perCnt, 2));
                                    if (!colper.ToUpper().Trim().Contains("P%"))
                                        tottempperc += perCnt;
                                    if (!dicCategoryWisePercentage.ContainsKey(colper.ToUpper().Trim()))
                                        dicCategoryWisePercentage.Add(colper.ToUpper().Trim(), perCnt);
                                    else
                                        dicCategoryWisePercentage[colper.ToUpper().Trim()] += perCnt;
                                    //student count add
                                    if (!FnlattCount.ContainsKey(colname))
                                        FnlattCount.Add(Convert.ToString(colname), TempCnt);
                                    else
                                    {
                                        double Cnt = 0;
                                        double.TryParse(Convert.ToString(FnlattCount[colname]), out Cnt);
                                        Cnt += TempCnt;
                                        FnlattCount.Remove(colname);
                                        FnlattCount.Add(Convert.ToString(colname), Cnt);
                                    }
                                    //student percentage add
                                    if (!FnlattCount.ContainsKey(colper))
                                        FnlattCount.Add(Convert.ToString(colper), perCnt);
                                    else
                                    {
                                        double Cnt = 0;
                                        double.TryParse(Convert.ToString(FnlattCount[colper]), out Cnt);
                                        Cnt += perCnt;
                                        FnlattCount.Remove(colper);
                                        FnlattCount.Add(Convert.ToString(colper), Cnt);
                                    }
                                    //lveCnt += TempCnt;
                                    //LvePer += perCnt;
                                }
                            }
                            drsstud["Leave Category(%)"] = Convert.ToString(Math.Round(tottempperc, 2));
                            //total percentage add
                            if (!FnlattCount.ContainsKey("Leave Category(%)"))
                                FnlattCount.Add(Convert.ToString("Leave Category(%)"), tottempperc);
                            else
                            {
                                double Cnt = 0;
                                double.TryParse(Convert.ToString(FnlattCount["Leave Category(%)"]), out Cnt);
                                Cnt += tottempperc;
                                FnlattCount.Remove("Leave Category(%)");
                                FnlattCount.Add(Convert.ToString("Leave Category(%)"), Cnt);
                            }
                            // totLveCnt += tottempperc;
                            #endregion
                        }
                        //
                        #region grand total

                        DataRow drstuds;
                        drstuds = dtstud.NewRow();
                        drstuds["Sno"] = Convert.ToString("Total");
                        dictcol.Add(Convert.ToString("Total" + "-" + Convert.ToInt32(dtstud.Rows.Count)), Convert.ToInt32(dtstud.Rows.Count));
                        drstuds[lbldeg.Text] = Convert.ToString("");
                        drstuds["Strength"] = Convert.ToString(totStudCnt);
                        drstuds["Section Strength"] = Convert.ToString(totStudCnt).Trim();
                        dtstud.Rows.Add(drstuds);
                        double tempperc = 0;
                        double tempStudCnt = 0;
                        for (int i = 4; i < dtstud.Columns.Count - 1; i++)
                        {
                            string colname = dtstud.Columns[i].ColumnName;
                            if (!colname.Contains('%'))
                            {
                                double.TryParse(Convert.ToString(FnlattCount.ContainsKey(colname) ? FnlattCount[colname].ToString() : ""), out tempStudCnt);
                                drstuds[colname] = Convert.ToString(tempStudCnt);
                            }
                            else
                            {
                                double perCnt = 0;// (tempStudCnt / totStudCnt) * 100;
                                if (dicCategoryWisePercentage.ContainsKey(colname.ToUpper().Trim()))
                                    perCnt = dicCategoryWisePercentage[colname.ToUpper().Trim()] / dvsec.Count;
                                drstuds[colname] = Convert.ToString(Math.Round(perCnt, 2));
                                if (!colname.Trim().ToUpper().Contains("P%"))
                                {
                                    tempperc += perCnt;
                                }
                            }
                            //  drstud[colname] = FnlattCount.ContainsKey(colname) ? FnlattCount[colname].ToString() : "";
                        }
                        drstuds["Leave Category(%)"] = Convert.ToString(Math.Round(tempperc, 2));
                        FnlattCount.Clear();

                        #endregion
                    }
                }
            }
            if (dtstud.Rows.Count > 0)
            {
                DataTable dvgrandtot = new DataTable();
                DataTable dttemp = dtstud.DefaultView.ToTable();
                dttemp.DefaultView.RowFilter = "Sno='Total'";
                dvgrandtot = dttemp.DefaultView.ToTable();
                if (dvgrandtot.Rows.Count > 0)
                {
                    int totstu = 0;
                    int pcount = 0;
                    double pper = 0;
                    int acount = 0;
                    double Aper = 0;
                    int ocount = 0;
                    double oper = 0;
                    double leaveper = 0;
                    for (int i = 0; i < dvgrandtot.Rows.Count; i++)
                    {
                        int temptotstu = 0;
                        int temppcount = 0;
                        double temppper = 0;
                        int tempacount = 0;
                        double tempAper = 0;
                        int tempocount = 0;
                        double tempoper = 0;
                        double templeaveper = 0;

                        int.TryParse(Convert.ToString(dvgrandtot.Rows[i]["Strength"]), out temptotstu);
                        int.TryParse(Convert.ToString(dvgrandtot.Rows[i]["P"]), out temppcount);
                        int.TryParse(Convert.ToString(dvgrandtot.Rows[i]["A"]), out tempacount);
                        int.TryParse(Convert.ToString(dvgrandtot.Rows[i]["OD"]), out tempocount);

                        double.TryParse(Convert.ToString(dvgrandtot.Rows[i]["P%"]), out temppper);
                        double.TryParse(Convert.ToString(dvgrandtot.Rows[i]["A%"]), out tempAper);
                        double.TryParse(Convert.ToString(dvgrandtot.Rows[i]["OD%"]), out tempoper);
                        double.TryParse(Convert.ToString(dvgrandtot.Rows[i]["Leave Category(%)"]), out templeaveper);

                        totstu = totstu + temptotstu;
                        pcount = pcount + temppcount;
                        acount = acount + tempacount;
                        ocount = ocount + tempocount;

                        pper = pper + temppper;
                        Aper = Aper + tempAper;
                        oper = oper + tempoper;

                        leaveper = leaveper + templeaveper;
                    }
                    double tpper = pper / Convert.ToInt32(dvgrandtot.Rows.Count);
                    double taper = Aper / Convert.ToInt32(dvgrandtot.Rows.Count);
                    double toper = oper / Convert.ToInt32(dvgrandtot.Rows.Count);
                    double tleaveper = leaveper / Convert.ToInt32(dvgrandtot.Rows.Count);

                    DataRow drrow;
                    drrow = dtstud.NewRow();
                    drrow["Sno"] = "Grand Total";
                    dictcol.Add(Convert.ToString("Grand Total" + "-" + Convert.ToInt32(dtstud.Rows.Count)), Convert.ToInt32(dtstud.Rows.Count));
                    drrow["Strength"] = Convert.ToString(totstu);
                    drrow["Section Strength"] = Convert.ToString(totstu); 
                    drrow["P"] = Convert.ToString(pcount);
                    drrow["A"] = Convert.ToString(acount);
                    drrow["OD"] = Convert.ToString(ocount);
                    drrow["P%"] = Convert.ToString(Math.Round(tpper, 2));//
                    drrow["A%"] = Convert.ToString(Math.Round(taper, 2));//Convert.ToString(Math.Round(taper, 2))
                    drrow["OD%"] = Convert.ToString(Math.Round(toper, 2));//Convert.ToString(Math.Round(toper, 2))
                    drrow["Leave Category(%)"] = Convert.ToString(Math.Round(tleaveper, 2));//Convert.ToString(Math.Round(tleaveper, 2))
                    dtstud.Rows.Add(drrow);
                }

            }
            if (dtstud.Rows.Count > 0)
            {
                gdattrpt.DataSource = dtstud;
                gdattrpt.DataBind();
                gdattrpt.Visible = true;
                btnExport.Visible = true;
                pnlContents.Visible = true;
                printCollegeDet();
                columnCount();
                spanGridColumnns(dictcol);
                notEnteredAttendance();
            }
        }
        catch { }
    }

    protected void gdattrpt_OnRowDataBound(object sender, GridViewRowEventArgs e)
    {
        //// percentage column visible true or false
        if (e.Row.RowType == DataControlRowType.Header)
        {
            e.Row.CssClass = "header";
            e.Row.Cells[1].Width = 250;
            for (int i = 4; i < e.Row.Cells.Count - 1; i++)
            {
                e.Row.Cells[i].Width = 60;
                if (cbperct.Checked)
                {
                    if (i % 2 != 0)
                        e.Row.Cells[i].Visible = true;
                }
                else
                    if (i % 2 != 0)
                        e.Row.Cells[i].Visible = false;
            }
            e.Row.Cells[e.Row.Cells.Count - 1].Width = 150;
        }
        int col = gdattrpt.Columns.Count - 1;
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            e.Row.Cells[2].HorizontalAlign = HorizontalAlign.Center;
            e.Row.Cells[0].HorizontalAlign = HorizontalAlign.Center;
            for (int i = 4; i < e.Row.Cells.Count - 1; i++)
            {
                e.Row.Cells[i].HorizontalAlign = HorizontalAlign.Center;
                if (cbperct.Checked)
                {
                    if (i % 2 != 0)
                        e.Row.Cells[i].Visible = true;
                }
                else
                    if (i % 2 != 0)
                        e.Row.Cells[i].Visible = false;
            }
            e.Row.Cells[e.Row.Cells.Count - 1].HorizontalAlign = HorizontalAlign.Center;
        }
        //column merge first and last
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            int cnt = e.Row.Cells.Count;
            if (e.Row.Cells[1].Text.Trim() == "&nbsp;")
            {
                
                if (e.Row.Cells[0].Text.Trim() != "Total")
                {
                    //e.Row.Cells[0].ColumnSpan = 2;
                    //e.Row.Cells.RemoveAt(2);
                    e.Row.Cells[0].BackColor = Color.Gold;
                    e.Row.Cells[0].Font.Bold = true;
                    e.Row.Cells[0].Font.Size = 14;
                }
                else
                {
                    e.Row.Cells[0].ColumnSpan = 2;
                    e.Row.Cells.RemoveAt(1);
                    e.Row.Cells[0].BackColor = Color.YellowGreen;
                    e.Row.Cells[0].Font.Bold = true;
                    e.Row.Cells[0].Font.Size = 12;
                }
               
            }
            if (e.Row.Cells[1].Text.Trim() == "&nbsp;" && e.Row.Cells[2].Text.Trim() == "Course")
            {
                if (e.Row.Cells[0].Text.Trim() != "Total")
                {
                    //e.Row.Cells[0].ColumnSpan = 2;
                    //e.Row.Cells.RemoveAt(2);
                    e.Row.Cells[0].BackColor = Color.RoyalBlue;
                    e.Row.Cells[0].ForeColor = Color.White;
                    e.Row.Cells[0].Font.Bold = true;
                    e.Row.Cells[0].Font.Size = 14;
                }
            }
            if (e.Row.Cells[0].Text.Trim() == "Grand Total")
            {
                e.Row.Cells[0].ColumnSpan = 2;
                e.Row.Cells.RemoveAt(1);
                e.Row.Cells[0].BackColor = Color.RoyalBlue;
                //e.Row.Cells[0].ForeColor = Color.White;
                e.Row.Cells[0].Font.Bold = true;
                e.Row.Cells[0].Font.Size = 12;
            }
        }
    }

    protected void gdattrpt_OnRowCreated(object sender, GridViewRowEventArgs e)
    {
    }

    protected void gdattrpt_OnDataBound(object sender, EventArgs e)
    {
        try
        {
            for (int i = gdattrpt.Rows.Count - 1; i > 0; i--)
            {
                GridViewRow row = gdattrpt.Rows[i];
                GridViewRow previousrow = gdattrpt.Rows[i - 1];
                for (int j = 2; j <= 2; j++)
                {
                    if (row.Cells[0].Text.Trim() != "Total")
                    {
                        if (row.Cells[j].Text == previousrow.Cells[j].Text)
                        {
                            if (previousrow.Cells[j].RowSpan == 0)
                            {
                                if (row.Cells[j].RowSpan == 0)
                                {
                                    previousrow.Cells[j].RowSpan = 2;
                                }
                                else
                                {
                                    previousrow.Cells[j].RowSpan = row.Cells[j].RowSpan + 1;
                                }
                                row.Cells[j].Visible = false;
                            }
                        }
                    }
                }
            }
        }
        catch
        {
        }
    }

    protected void columnCount()
    {
        try
        {
            int Cnt = gdattrpt.Rows[1].Cells.Count;
            if (Cnt > 10)
                btnExport.Text = "Print A3 Format";
            else
                btnExport.Text = "Print A4 Format";
        }
        catch { }
    }

    protected void printCollegeDet()
    {
        try
        {
            string colquery = "select collname,university,address1 ,address2,address3+' - '+pincode as address3 from collinfo where college_code=" + ddlcollege.SelectedItem.Value + " ";
            string academicyear = d2.GetFunctionv("select value from master_settings where settings='Academic year'");
            academicyear = academicyear.Trim().Trim(',').Replace(",", "-");
            string collegename = string.Empty;
            string add1 = string.Empty;
            string add2 = string.Empty;
            string add3 = string.Empty;
            string univ = string.Empty;
            string feedet = string.Empty;
            ds = d2.select_method_wo_parameter(colquery, "Text");
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                collegename = Convert.ToString(ds.Tables[0].Rows[0]["collname"]);
                add1 = Convert.ToString(ds.Tables[0].Rows[0]["address1"]);
                add2 = Convert.ToString(ds.Tables[0].Rows[0]["address2"]);
                add3 = Convert.ToString(ds.Tables[0].Rows[0]["address3"]);
                univ = Convert.ToString(ds.Tables[0].Rows[0]["university"]);
                add1 += " " + add2;
                spCollege.InnerText = collegename;
                spAffBy.InnerText = add1;
                spController.InnerText = add3;
                spSeating.InnerText = univ;
                // spDateSession.InnerText = "PRE-PRIMARY COMPARTMENT";
                sprptnamedt.InnerText = "STUDENTS ATTENDANCE CONSOLIDATION--" + academicyear + "";
                spdate.InnerText = txt_fromdate.Text.Replace('/', '.');
                //spdate.InnerText = "STUDENTS ATTENDANCE CONSOLIDATION--" + academicyear + "";
            }
        }
        catch { }
    }

    protected void spanGridColumnns(Dictionary<string, int> gdcol)
    {
        try
        {
            foreach (KeyValuePair<string, int> gdval in gdcol)
            {
                int rowCnt = Convert.ToInt32(gdval.Value.ToString());
                string rowVal = gdval.Key.ToString();
                string spltxt = rowVal.Contains('-') ? rowVal.Split('-')[0].ToString() : "";
                int Cnt = gdattrpt.Rows[rowCnt].Cells.Count;
                if (gdattrpt.Rows[rowCnt].Cells[1].Text.Trim() == "&nbsp;")
                {
                    if (gdattrpt.Rows[rowCnt].Cells[0].Text.Trim() != "Total")
                    {
                        gdattrpt.Rows[rowCnt].Cells[0].ColumnSpan = Cnt;
                        for (int i = 1; i < gdattrpt.Rows[rowCnt].Cells.Count; i++)
                        {
                            gdattrpt.Rows[rowCnt].Cells[i].Visible = false;
                        }
                    }
                }
                else if (gdattrpt.Rows[rowCnt].Cells[0].Text.Trim() == spltxt)
                {
                    for (int i = 0; i < gdattrpt.Rows[rowCnt].Cells.Count; i++)
                    {
                        gdattrpt.Rows[rowCnt].Cells[i].BackColor = Color.YellowGreen;
                        gdattrpt.Rows[rowCnt].Cells[i].Font.Bold = true;
                        gdattrpt.Rows[rowCnt].Cells[i].Font.Size = 12;
                    }
                }
            }
        }
        catch { }
    }

    protected void notEnteredAttendance()
    {
        try
        {
            bool check = false;
            for (int row = 0; row < gdattrpt.Rows.Count - 1; row++)
            {
                if (gdattrpt.Rows[row].Cells[1].Text.Trim() != "&nbsp;")
                {
                    double colAttTotVal = 0;
                    for (int col = 3; col < gdattrpt.Rows[row].Cells.Count - 1; col++)
                    {
                        double colAttValue = 0;
                        if (col % 2 != 0)
                        {
                            double.TryParse(Convert.ToString(gdattrpt.Rows[row].Cells[col].Text), out colAttValue);
                            colAttTotVal += colAttValue;
                        }
                    }
                    if (colAttTotVal == 0)
                    {
                        gdattrpt.Rows[row].Cells[1].BackColor = Color.LightPink;
                        check = true;
                    }
                }
            }
            if (check)
            {
                lblmark.Visible = true;
                lblmark.Attributes.Add("Style", "background-color:LightPink;");
            }
            else
                lblmark.Visible = false;
        }
        catch { }
    }

    private void setLabelText()
    {
        string grouporusercode = string.Empty;
        if (Session["group_code"] != null && (Convert.ToString(Session["group_code"]).Trim() != "") && (Convert.ToString(Session["group_code"]).Trim() != "0") && (Convert.ToString(Session["group_code"]).Trim() != "-1"))
        {
            grouporusercode = " group_code=" + Convert.ToString(Session["group_code"]).Trim() + "";
        }
        else if (Session["usercode"] != null)
        {
            grouporusercode = " usercode=" + Convert.ToString(Session["usercode"]).Trim() + "";
        }
        List<Label> lbl = new List<Label>();
        List<byte> fields = new List<byte>();
        lbl.Add(lblclg);
        lbl.Add(lbldeg);
        lbl.Add(lbldept);
        fields.Add(0);
        fields.Add(2);
        fields.Add(3);
        new HeaderLabelText().setLabels(grouporusercode, ref lbl, fields);
    }

    #region Common Checkbox and Checkboxlist Event

    private string getCblSelectedValue(CheckBoxList cblSelected)
    {
        System.Text.StringBuilder selectedvalue = new System.Text.StringBuilder();
        try
        {
            for (int sel = 0; sel < cblSelected.Items.Count; sel++)
            {
                if (cblSelected.Items[sel].Selected == true)
                {
                    if (selectedvalue.Length == 0)
                    {
                        selectedvalue.Append(Convert.ToString(cblSelected.Items[sel].Value));
                    }
                    else
                    {
                        selectedvalue.Append("','" + Convert.ToString(cblSelected.Items[sel].Value));
                    }
                }
            }
        }
        catch { cblSelected.Items.Clear(); }
        return selectedvalue.ToString();
    }

    private string getCblSelectedText(CheckBoxList cblSelected)
    {
        System.Text.StringBuilder selectedText = new System.Text.StringBuilder();
        try
        {
            for (int sel = 0; sel < cblSelected.Items.Count; sel++)
            {
                if (cblSelected.Items[sel].Selected == true)
                {
                    if (selectedText.Length == 0)
                    {
                        selectedText.Append(Convert.ToString(cblSelected.Items[sel].Text));
                    }
                    else
                    {
                        selectedText.Append("','" + Convert.ToString(cblSelected.Items[sel].Text));
                    }
                }
            }
        }
        catch { cblSelected.Items.Clear(); }
        return selectedText.ToString();
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

    private void CallCheckboxListChange(CheckBox cb, CheckBoxList cbl, TextBox txt, string dipst, string deft)
    {
        try
        {
            int sel = 0;
            int count = 0;
            string name = string.Empty;
            cb.Checked = false;
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

    #endregion

}
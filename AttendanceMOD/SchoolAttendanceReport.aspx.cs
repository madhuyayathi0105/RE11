using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class SchoolAttendanceReport : System.Web.UI.Page
{
    DataSet ds = new DataSet();
    DAccess2 d2 = new DAccess2();
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
            bindbatch();
            if (ddlcollege.Items.Count > 0)
            {
                collegecode = Convert.ToString(ddlcollege.SelectedItem.Value);
            }
            loadLeave();
            txt_fromdate.Text = DateTime.Now.ToString("dd/MM/yyyy");
            txt_fromdate.Attributes.Add("readonly", "readonly");
            
        }
        if (ddlcollege.Items.Count > 0)
        {
            collegecode = Convert.ToString(ddlcollege.SelectedItem.Value);
        }
    }

    #region college

    public void loadcollege()
    {
        try
        {
            ddlcollege.Items.Clear();
            ds.Clear();
            ds = d2.BindCollege();
            if (ds.Tables[0].Rows.Count > 0)
            {
                ddlcollege.DataSource = ds;
                ddlcollege.DataTextField = "collname";
                ddlcollege.DataValueField = "college_code";
                ddlcollege.DataBind();
            }
        }
        catch
        { }
    }

    protected void ddlcollege_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddlcollege.Items.Count > 0)
            collegecode = Convert.ToString(ddlcollege.SelectedItem.Value);
        loadLeave();
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
        }
        catch { }
    }

    protected void cbl_batch_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            CallCheckboxListChange(cb_batch, cbl_batch, txt_batch, "Batch", "--Select--");
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
        pnlContents.Visible = false;
        btnExport.Visible = false;
    }

    protected void rbdept_Changed(object sender, EventArgs e)
    {
        pnlContents.Visible = false;
        btnExport.Visible = false;
    }

    protected void btngo_Click(object sender, EventArgs e)
    {
        try
        {
            ds.Clear();
            ds = loadDatasetval();
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0 && ds.Tables[1].Rows.Count > 0)
            {
                loadGridDetail(ds);
            }
            else
            {
                gdattrpt.Visible = false;
                btnExport.Visible = false;
                ScriptManager.RegisterStartupScript(this, typeof(Page), UniqueID, "alert('No Record Found')", true);
            }
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
            lblerror.Visible = false;
            string levecode = getCblSelectedValue(cblleve);
            string curYear = DateTime.Now.ToString("yyyy");
            string date = Convert.ToString(txt_fromdate.Text);
            string[] frdate = date.Split('/');
            if (frdate.Length == 3)
                date = frdate[1].ToString() + "/" + frdate[0].ToString() + "/" + frdate[2].ToString();
            string mnthYear = Convert.ToString((Convert.ToInt32(curYear) * 12) + Convert.ToInt32(frdate[1]));
            if (batchyear != "")
            {
                if (rbdeg.Checked)
                {
                    SelQ = "  select count(r.roll_no) as cnt,C.course_id as code,C.Course_name as name,r.batch_year from registration r,degree d,Course c ,department dt where  dt.dept_code=d.dept_code and r.degree_code =d.degree_code and c.course_id =d.course_id and r.cc='0' and r.delflag='0' and r.exam_flag<>'debar' and d.college_code ='" + collegecode + "' and r.Batch_year in('" + batchyear + "')  group by C.course_id,C.Course_name,r.batch_year "; //and r.batch_year=2016
                    //SelQ += " select count(al.appno) as cnt,c.course_id ,mleavecode,eleavecode  from AllStudentAttendanceReport al, registration r,degree d,Course c ,department dt where  dt.dept_code=d.dept_code and al.appno=r.app_no and r.degree_code =d.degree_code and r.college_code=d.college_code and c.course_id =d.course_id and d.college_code ='" + collegecode + "' and al.DateofAttendance='" + date + "'  group by c.course_id ,mleavecode,eleavecode  ";
                    SelQ += " select count(r.app_no) as cnt,mleavecode,eleavecode,d.course_id,r.batch_year from AllStudentAttendanceReport al,registration r,degree d where  al.appno=r.app_no and r.college_code ='" + collegecode + "'and r.Batch_year in('" + batchyear + "') and d.degree_code=r.degree_code and r.cc='0' and r.delflag='0' and r.exam_flag<>'debar' and al.DateofAttendance='" + date + "'  group by mleavecode,eleavecode,d.course_id,r.batch_year"; //and r.batch_year=2016
                    //select count(r.app_no) as cnt,mleavecode,eleavecode,d.course_id from AllStudentAttendanceReport al,registration r,degree d where  al.appno=r.app_no and r.college_code ='13' and d.degree_code=r.degree_code and  al.DateofAttendance='11/25/2016' group by mleavecode,eleavecode,d.course_id
                }
                else
                {
                    SelQ = " select count(r.roll_no) as cnt,isnull(r.Sections,'')as sec,C.course_id,C.Course_name,d.degree_code as code,(dt.dept_name+'-'+isnull(r.Sections,'')) as name,r.batch_year from registration r,degree d,Course c,department dt where r.degree_code =d.degree_code and c.course_id =d.course_id and d.dept_code = dt.dept_code and r.cc='0' and r.delflag='0' and r.exam_flag<>'debar' and d.college_code ='" + collegecode + "' and r.Batch_year in('" + batchyear + "')  group by C.course_id,C.Course_name,d.degree_code,dt.dept_name,(dt.dept_name+'-'+isnull(r.Sections,'')),isnull(r.Sections,''),r.batch_year order by d.degree_code,isnull(r.Sections,'')"; //and r.batch_year=2016
                    //SelQ += " select count(al.appno) as cnt,c.course_id,d.degree_code ,mleavecode,eleavecode  from AllStudentAttendanceReport al, registration r,degree d,Course c ,department dt where  dt.dept_code=d.dept_code and al.appno=r.app_no and r.degree_code =d.degree_code and r.college_code=d.college_code and c.course_id =d.course_id and d.college_code ='" + collegecode + "' and al.DateofAttendance='" + date + "'  group by c.course_id,d.degree_code ,mleavecode,eleavecode  ";
                    SelQ += " select count(al.appno) as cnt,d.course_id,d.degree_code ,mleavecode,eleavecode,isnull(r.Sections,'')as sec,r.batch_year  from AllStudentAttendanceReport al, registration r,degree d where  al.appno=r.app_no and r.degree_code =d.degree_code and r.college_code=d.college_code and r.cc='0' and r.delflag='0' and r.exam_flag<>'debar' and d.college_code ='" + collegecode + "'and r.Batch_year in('" + batchyear + "') and al.DateofAttendance='" + date + "'   group by d.course_id,d.degree_code ,mleavecode,eleavecode,isnull(r.Sections,''),r.batch_year"; //and r.batch_year=2016
                    //select count(al.appno) as cnt,d.course_id,d.degree_code ,mleavecode,eleavecode  from AllStudentAttendanceReport al, registration r,degree d where  al.appno=r.app_no and r.degree_code =d.degree_code and r.college_code=d.college_code  and d.college_code ='13' and al.DateofAttendance='11/28/2016'  group by d.course_id,d.degree_code ,mleavecode,eleavecode  
                }
                dsload = d2.select_method_wo_parameter(SelQ, "Text");
            }
            else
            {
                lblerror.Text = "Please select the batch year";
            }
        }
        catch { }
        return dsload;
    }

    protected void loadGridDetail(DataSet ds)
    {
        try
        {
            bool grandFlag = false;
            DataTable dtstud = new DataTable();
            DataView dvfull = new DataView();
            DataView dvfst = new DataView();
            DataView dvsnd = new DataView();
            Dictionary<string, double> FnlattCount = new Dictionary<string, double>();
            Dictionary<string, double> grandTotal = new Dictionary<string, double>();
            Dictionary<string, int> dictcol = new Dictionary<string, int>();

            string lblName = string.Empty;
            if (rbdeg.Checked)
                lblName = lbldeg.Text;
            else
                lblName = lbldept.Text;
            double totStudCnt = 0;
            string compname = string.Empty;
            string tempName = string.Empty;
           
            if (rbdeg.Checked)
                tempName = "Course_id=";
            else
            {
                tempName = "Degree_code=";
            }

            dtstud.Columns.Add("Sno");
            dtstud.Columns.Add(lblName);
            dtstud.Columns.Add("Strength");
            for (int col = 0; col < cblleve.Items.Count; col++)
            {
                if (cblleve.Items[col].Selected)
                {
                    dtstud.Columns.Add(cblleve.Items[col].Text);
                    dtstud.Columns.Add(cblleve.Items[col].Text + "%");
                }
            }
            dtstud.Columns.Add("Leave Category(%)");
            dtstud.Columns.Add("Not Entered");
            Dictionary<string, double> dicTotal = new Dictionary<string, double>();
            Dictionary<string, double> dicGrandTotal = new Dictionary<string, double>();
            for (int dsf = 0; dsf < ds.Tables[0].Rows.Count; dsf++)
            {
                double totalReming = 0;
                double.TryParse(Convert.ToString(ds.Tables[0].Rows[dsf]["cnt"]), out totStudCnt);
                compname = Convert.ToString(ds.Tables[0].Rows[dsf]["name"]);
                string Sec = string.Empty;
                Sec = Convert.ToString(ds.Tables[0].Rows[dsf]["sec"]);
                DataRow drstud;
                drstud = dtstud.NewRow();
                drstud["Sno"] = Convert.ToString(dtstud.Rows.Count + 1);
                drstud[lblName] = compname;
                drstud["Strength"] = Convert.ToString(totStudCnt);
                dtstud.Rows.Add(drstud);
                //student count add
                if (!FnlattCount.ContainsKey("Strength"))
                    FnlattCount.Add(Convert.ToString("Strength"), totStudCnt);
                else
                {
                    double Cnt = 0;
                    double.TryParse(Convert.ToString(FnlattCount["Strength"]), out Cnt);
                    Cnt += totStudCnt;
                    FnlattCount.Remove("Strength");
                    FnlattCount.Add(Convert.ToString("Strength"), Cnt);
                }
                string sections=string.Empty;
                 sections = "and sec='" + Sec + "'";
                double tottempperc = 0;
                Dictionary<string, double> dicCategoryWisePercentage = new Dictionary<string, double>();
                if (ds.Tables[1].Rows.Count > 0)
                {
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
                            ds.Tables[1].DefaultView.RowFilter = "" + tempName + "'" + Convert.ToString(ds.Tables[0].Rows[dsf]["code"]) + "' and  mleavecode='" + Convert.ToString(cblleve.Items[col].Value) + "' and eleavecode='" + Convert.ToString(cblleve.Items[col].Value) + "' " + sections + "";
                            dvfull = ds.Tables[1].DefaultView;
                            if (dvfull.Count > 0)
                            {
                                double.TryParse(Convert.ToString(dvfull[0]["cnt"]), out TempCnt);
                            }
                            ds.Tables[1].DefaultView.RowFilter = "" + tempName + "'" + Convert.ToString(ds.Tables[0].Rows[dsf]["code"]) + "' and mleavecode='" + Convert.ToString(cblleve.Items[col].Value) + "' and eleavecode<>'" + Convert.ToString(cblleve.Items[col].Value) + "' " + sections + "";
                            dvfst = ds.Tables[1].DefaultView;
                            if (dvfst.Count > 0)
                            {
                                double.TryParse(Convert.ToString(dvfst[0]["cnt"]), out fstCnt);
                                TempStudCnt += fstCnt;
                            }

                            ds.Tables[1].DefaultView.RowFilter = "" + tempName + "'" + Convert.ToString(ds.Tables[0].Rows[dsf]["code"]) + "' and mleavecode<>'" + Convert.ToString(cblleve.Items[col].Value) + "' and eleavecode='" + Convert.ToString(cblleve.Items[col].Value) + "' " + sections + "";
                            dvsnd = ds.Tables[1].DefaultView;
                            if (dvsnd.Count > 0)
                            {
                                double.TryParse(Convert.ToString(dvsnd[0]["cnt"]), out sndCnt);
                                TempStudCnt += sndCnt;
                            }
                            if (TempStudCnt != 0 || TempStudCnt != 0.0)
                                TempCnt += TempStudCnt / 2;

                            perCnt = (TempCnt / totStudCnt) * 100;
                            drstud[colname] = Convert.ToString(TempCnt);
                            string colper = colname + "%";
                            drstud[colper] = Convert.ToString(Math.Round(perCnt, 2));
                            if (!colper.ToUpper().Trim().Contains("P%"))
                                tottempperc += perCnt;

                            if (!dicCategoryWisePercentage.ContainsKey(colname.ToUpper().Trim()))
                                dicCategoryWisePercentage.Add(colname.ToUpper().Trim(), TempCnt);
                            else
                                dicCategoryWisePercentage[colname.ToUpper().Trim()] += TempCnt;

                            if (!dicCategoryWisePercentage.ContainsKey(colper.ToUpper().Trim()))
                                dicCategoryWisePercentage.Add(colper.ToUpper().Trim(), perCnt);
                            else
                                dicCategoryWisePercentage[colper.ToUpper().Trim()] += perCnt;

                            if (!dicGrandTotal.ContainsKey(colname.ToUpper().Trim()))
                                dicGrandTotal.Add(colname.ToUpper().Trim(), TempCnt);
                            else
                                dicGrandTotal[colname.ToUpper().Trim()] += TempCnt;

                            if (!dicGrandTotal.ContainsKey(colper.ToUpper().Trim()))
                                dicGrandTotal.Add(colper.ToUpper().Trim(), perCnt);
                            else
                                dicGrandTotal[colper.ToUpper().Trim()] += perCnt;
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

                            totalReming += TempCnt;
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
                        }
                    }
                    drstud["Leave Category(%)"] = Convert.ToString(Math.Round(tottempperc, 2));
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

                    //remainging count

                    double balremain = totStudCnt - totalReming;
                    drstud["Not Entered"] = Convert.ToString(Math.Round(balremain, 2));
                    if (!FnlattCount.ContainsKey("Not Entered"))
                        FnlattCount.Add(Convert.ToString("Not Entered"), balremain);
                    else
                    {
                        double Cnt = 0;
                        double.TryParse(Convert.ToString(FnlattCount["Not Entered"]), out Cnt);
                        Cnt += balremain;
                        FnlattCount.Remove("Not Entered");
                        FnlattCount.Add(Convert.ToString("Not Entered"), Cnt);
                    }

                    if (!dicGrandTotal.ContainsKey("NOT ENTERED"))
                        dicGrandTotal.Add("NOT ENTERED", balremain);
                    else
                        dicGrandTotal["NOT ENTERED"] += balremain;
                    //totStudCnt
                    //totalReming
                }
                //
                if (rbdeg.Checked)
                {
                    #region grand total

                    DataRow drstuds;
                    drstuds = dtstud.NewRow();
                    drstuds["Sno"] = Convert.ToString("Total");
                    //  dictcol.Add(Convert.ToString("Total" + "-" + Convert.ToInt32(dtstud.Rows.Count)), Convert.ToInt32(dtstud.Rows.Count));
                    dictcol.Add(Convert.ToString("Total" + "-" + Convert.ToInt32(dtstud.Rows.Count)), Convert.ToInt32(dtstud.Rows.Count));
                    //  double.TryParse(Convert.ToString(grandTotal.ContainsKey("Strength") ? grandTotal["Strength"].ToString() : ""), out totStudCnt);
                    drstuds[lblName] = Convert.ToString("");
                    drstuds["Strength"] = Convert.ToString(totStudCnt);
                    dtstud.Rows.Add(drstuds);
                    //student count add
                    if (!grandTotal.ContainsKey("Strength"))
                        grandTotal.Add(Convert.ToString("Strength"), totStudCnt);
                    else
                    {
                        double Cnt = 0;
                        double.TryParse(Convert.ToString(grandTotal["Strength"]), out Cnt);
                        Cnt += totStudCnt;
                        grandTotal.Remove("Strength");
                        grandTotal.Add(Convert.ToString("Strength"), Cnt);
                    }
                    double tempperc = 0;
                    double tempStudCnt = 0;
                    double totreming = 0;
                    for (int i = 3; i < dtstud.Columns.Count - 2; i++)
                    {
                        string colname = dtstud.Columns[i].ColumnName;
                        if (!colname.Contains('%'))
                        {
                            double.TryParse(Convert.ToString(FnlattCount.ContainsKey(colname) ? FnlattCount[colname].ToString() : ""), out tempStudCnt);
                            drstuds[colname] = Convert.ToString(tempStudCnt);
                            // Grand total Add   student count add
                            if (!grandTotal.ContainsKey(colname))
                                grandTotal.Add(Convert.ToString(colname), tempStudCnt);
                            else
                            {
                                double Cnt = 0;
                                double.TryParse(Convert.ToString(grandTotal[colname]), out Cnt);
                                Cnt += tempStudCnt;
                                grandTotal.Remove(colname);
                                grandTotal.Add(Convert.ToString(colname), Cnt);
                            }
                            totreming += tempStudCnt;
                        }
                        else
                        {
                            double perCnt = (tempStudCnt / totStudCnt) * 100;
                            drstuds[colname] = Convert.ToString(Math.Round(perCnt, 2));
                            if (!colname.Trim().ToUpper().Contains("P%"))
                            {
                                tempperc += perCnt;
                            }
                            //student percentage add
                            if (!grandTotal.ContainsKey(colname))
                                grandTotal.Add(Convert.ToString(colname), perCnt);
                            else
                            {
                                double Cnt = 0;
                                double.TryParse(Convert.ToString(grandTotal[colname]), out Cnt);
                                Cnt += perCnt;
                                grandTotal.Remove(colname);
                                grandTotal.Add(Convert.ToString(colname), Cnt);
                            }
                        }
                        grandFlag = true;
                        //  drstud[colname] = FnlattCount.ContainsKey(colname) ? FnlattCount[colname].ToString() : "";
                    }
                    drstuds["Leave Category(%)"] = Convert.ToString(Math.Round(tempperc, 2));
                    //grand total add
                    if (!grandTotal.ContainsKey("Leave Category(%)"))
                        grandTotal.Add(Convert.ToString("Leave Category(%)"), tempperc);
                    else
                    {
                        double Cnt = 0;
                        double.TryParse(Convert.ToString(grandTotal["Leave Category(%)"]), out Cnt);
                        Cnt += tempperc;
                        grandTotal.Remove("Leave Category(%)");
                        grandTotal.Add(Convert.ToString("Leave Category(%)"), Cnt);
                    }

                    //Not Entered
                    double baltot = totStudCnt - totreming;
                    drstuds["Not Entered"] = Convert.ToString(Math.Round(baltot, 2));
                    //grand total add
                    if (!grandTotal.ContainsKey("Not Entered"))
                        grandTotal.Add(Convert.ToString("Not Entered"), baltot);
                    else
                    {
                        double Cnt = 0;
                        double.TryParse(Convert.ToString(grandTotal["Not Entered"]), out Cnt);
                        Cnt += baltot;
                        grandTotal.Remove("Not Entered");
                        grandTotal.Add(Convert.ToString("Not Entered"), Cnt);
                    }

                    if (!dicGrandTotal.ContainsKey("NOT ENTERED"))
                        dicGrandTotal.Add("NOT ENTERED", baltot);
                    else
                        dicGrandTotal["NOT ENTERED"] += baltot;
                    FnlattCount.Clear();
                    #endregion
                }
            }
            if (rbdept.Checked)
            {
                #region grand total

                double totalcount = 0;
                DataRow drstuds;
                drstuds = dtstud.NewRow();
                drstuds["Sno"] = Convert.ToString("Total");
                dictcol.Add(Convert.ToString("Total" + "-" + Convert.ToInt32(dtstud.Rows.Count)), Convert.ToInt32(dtstud.Rows.Count));
                drstuds[lblName] = Convert.ToString("");
                double.TryParse(Convert.ToString(FnlattCount.ContainsKey("Strength") ? FnlattCount["Strength"].ToString() : ""), out totalcount);
                drstuds["Strength"] = Convert.ToString(totalcount);
                dtstud.Rows.Add(drstuds);
                double tempperc = 0;
                double tempStudCnt = 0;
                double totreming = 0;
                for (int i = 3; i < dtstud.Columns.Count - 2; i++)
                {
                    string colname = dtstud.Columns[i].ColumnName;
                    if (!colname.Contains('%'))
                    {
                        double.TryParse(Convert.ToString(FnlattCount.ContainsKey(colname) ? FnlattCount[colname].ToString() : ""), out tempStudCnt);
                        drstuds[colname] = Convert.ToString(tempStudCnt);
                        totreming += tempStudCnt;
                    }
                    else
                    {
                        double perCnt = 0;// (tempStudCnt / totStudCnt) * 100;
                        if (dicGrandTotal.ContainsKey(colname.ToUpper().Trim()))
                            perCnt = dicGrandTotal[colname.ToUpper().Trim()] / ds.Tables[0].Rows.Count;
                        drstuds[colname] = Convert.ToString(Math.Round(perCnt, 2));
                        if (!colname.Trim().ToUpper().Contains("P%"))
                        {
                            tempperc += perCnt;
                        }
                    }
                    //  drstud[colname] = FnlattCount.ContainsKey(colname) ? FnlattCount[colname].ToString() : "";
                }
                drstuds["Leave Category(%)"] = Convert.ToString(Math.Round(tempperc, 2));
                //Not Entered
                double baltot = totalcount - totreming;
                drstuds["Not Entered"] = Convert.ToString(Math.Round(baltot, 2));
                FnlattCount.Clear();

                #endregion
            }
            if (grandFlag)
            {
                //scholl type wise
                #region grand total
                double totalcount = 0;
                double tremin = 0;
                DataRow drstuds;
                drstuds = dtstud.NewRow();
                drstuds["Sno"] = Convert.ToString("Grand Total");
                dictcol.Add(Convert.ToString("Grand Total" + "-" + Convert.ToInt32(dtstud.Rows.Count)), Convert.ToInt32(dtstud.Rows.Count));
                drstuds[lblName] = Convert.ToString("");
                double.TryParse(Convert.ToString(grandTotal.ContainsKey("Strength") ? grandTotal["Strength"].ToString() : ""), out totalcount);
                drstuds["Strength"] = Convert.ToString(totalcount);
                dtstud.Rows.Add(drstuds);
                double tempperc = 0;
                double tempStudCnt = 0;
                for (int i = 3; i < dtstud.Columns.Count - 2; i++)
                {
                    string colname = dtstud.Columns[i].ColumnName;
                    if (!colname.Contains('%'))
                    {
                        double.TryParse(Convert.ToString(grandTotal.ContainsKey(colname) ? grandTotal[colname].ToString() : ""), out tempStudCnt);
                        drstuds[colname] = Convert.ToString(tempStudCnt);
                        tremin += tempStudCnt;
                    }
                    else
                    {
                        double perCnt = 0;// (tempStudCnt / totalcount) * 100;
                        if (dicGrandTotal.ContainsKey(colname.ToUpper().Trim()))
                            perCnt = dicGrandTotal[colname.ToUpper().Trim()] / ds.Tables[0].Rows.Count;
                        drstuds[colname] = Convert.ToString(Math.Round(perCnt, 2));
                        if (!colname.Trim().ToUpper().Contains("P%"))
                        {
                            tempperc += perCnt;
                        }
                    }
                    //  drstud[colname] = FnlattCount.ContainsKey(colname) ? FnlattCount[colname].ToString() : "";
                }
                drstuds["Leave Category(%)"] = Convert.ToString(Math.Round(tempperc, 2));
                drstuds["Not Entered"] = Convert.ToString(totalcount - tremin);
                grandTotal.Clear();
                #endregion
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
            }
        }
        catch { }
    }

    protected void columnCount()
    {
        try
        {
            int Cnt = gdattrpt.Rows[0].Cells.Count;
            if (Cnt > 10)
                btnExport.Text = "Print A3 Format";
            else
                btnExport.Text = "Print A4 Format";
        }
        catch { }
    }

    protected void gdattrpt_OnRowDataBound(object sender, GridViewRowEventArgs e)
    {
        ////Add CSS class on header row.
        if (e.Row.RowType == DataControlRowType.Header)
        {
            e.Row.CssClass = "header";
            e.Row.Cells[1].Width = 250;
            for (int i = 3; i < e.Row.Cells.Count - 1; i++)
            {
                e.Row.Cells[i].Width = 60;
                if (cbperct.Checked)
                {
                    if (i % 2 == 0)
                        e.Row.Cells[i].Visible = true;
                }
                else
                    if (i % 2 == 0)
                        e.Row.Cells[i].Visible = false;
            }
            e.Row.Cells[e.Row.Cells.Count - 1].Width = 150;
        }
        int col = gdattrpt.Columns.Count - 1;
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            e.Row.Cells[2].HorizontalAlign = HorizontalAlign.Center;
            e.Row.Cells[0].HorizontalAlign = HorizontalAlign.Center;
            for (int i = 3; i < e.Row.Cells.Count - 1; i++)
            {
                e.Row.Cells[i].HorizontalAlign = HorizontalAlign.Center;
                if (cbperct.Checked)
                {
                    if (i % 2 == 0)
                        e.Row.Cells[i].Visible = true;
                }
                else
                    if (i % 2 == 0)
                        e.Row.Cells[i].Visible = false;
            }
            e.Row.Cells[e.Row.Cells.Count - 1].HorizontalAlign = HorizontalAlign.Center;
        }
        //column merge first and last
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            if (e.Row.Cells[0].Text.Trim() == "Total")
            {
                e.Row.Cells[0].ColumnSpan = 2;
                e.Row.Cells.RemoveAt(1);
                e.Row.Cells[0].BackColor = Color.YellowGreen;
                e.Row.Cells[0].Font.Bold = true;
                e.Row.Cells[0].Font.Size = 12;
            }
            if (e.Row.Cells[0].Text.Trim() == "Grand Total")
            {
                e.Row.Cells[0].ColumnSpan = 2;
                e.Row.Cells.RemoveAt(1);
                e.Row.Cells[0].BackColor = Color.Gold;
                e.Row.Cells[0].Font.Bold = true;
                e.Row.Cells[0].Font.Size = 12;
            }
        }
    }

    protected void printCollegeDet()
    {
        try
        {
            string colquery = "select collname,university,address1 ,address2,address3+' - '+pincode as address3 from collinfo where college_code=" + ddlcollege.SelectedItem.Value + " ";
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
                spDateSession.InnerText = "STUDENT CONSOLIDATION - " + txt_fromdate.Text.Replace("/", ".").Trim() + "";//DateTime.Now.ToString("dd.MM.yyyy")
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
                if (gdattrpt.Rows[rowCnt].Cells[0].Text.Trim() == "Grand Total")
                {
                    for (int i = 0; i < gdattrpt.Rows[rowCnt].Cells.Count; i++)
                    {
                        gdattrpt.Rows[rowCnt].Cells[i].BackColor = Color.Gold;
                        gdattrpt.Rows[rowCnt].Cells[i].Font.Bold = true;
                        gdattrpt.Rows[rowCnt].Cells[i].Font.Size = 12;
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

    #region old

    //protected DataSet loadDataset()
    //{
    //    DataSet dsload = new DataSet();
    //    try
    //    {
    //        string SelQ =string.Empty;
    //        if (ddlcollege.Items.Count > 0)
    //            collegecode = Convert.ToString(ddlcollege.SelectedItem.Value);
    //        string levecode = getCblSelectedValue(cblleve);
    //        string curYear = DateTime.Now.ToString("yyyy");
    //        string date = Convert.ToString(txt_fromdate.Text);
    //        string[] frdate = date.Split('/');
    //        if (frdate.Length == 3)
    //            date = frdate[1].ToString() + "/" + frdate[0].ToString() + "/" + frdate[2].ToString();
    //        string mnthYear = Convert.ToString((Convert.ToInt32(curYear) * 12) + Convert.ToInt32(frdate[1]));
    //        if (rbdeg.Checked)
    //        {
    //            int dayCnt = 0;
    //            int.TryParse(d2.GetFunction("select max(No_of_Hrs_Per_day) from PeriodAttndSchedule p,degree r where p.degree_code=r.degree_code and r.college_code='" + collegecode + "' "), out dayCnt); ;
    //            StringBuilder strLgth = new StringBuilder();
    //            if (dayCnt != 0)
    //            {
    //                for (int i = 1; i <= dayCnt; i++)
    //                {
    //                    strLgth.Append("d" + frdate[0].TrimStart('0') + "d" + i + ",");
    //                }
    //            }
    //            if (strLgth.Length > 0)
    //                strLgth.Remove(strLgth.Length - 1, 1);
    //            if (strLgth.Length > 0)
    //            {
    //                SelQ = " select count(r.roll_no) as cnt,C.course_id,C.Course_name from registration r,degree d,Course c where r.degree_code =d.degree_code and c.course_id =d.course_id and d.college_code ='" + collegecode + "' group by C.course_id,C.Course_name";
    //                SelQ += " select r.roll_no ,C.course_id,C.Course_name ,r.degree_code,r.current_semester from registration r,degree d,Course c where r.degree_code =d.degree_code and c.course_id =d.course_id and d.college_code ='" + collegecode + "'";
    //                // and c.course_id='23'
    //                SelQ += " select p.degree_code,No_of_Hrs_Per_day,no_of_hrs_I_half_day,no_of_hrs_II_half_day,min_pres_I_half_day,min_pres_II_half_day,min_hrs_per_day from PeriodAttndSchedule p,degree r where p.degree_code=r.degree_code and r.college_code='" + collegecode + "' ";
    //                SelQ += " select " + strLgth + ",A.ROLL_NO from attendance a,registration r where r.roll_no =a.roll_no and r.college_code='" + collegecode + "' AND month_year='" + mnthYear + "' ";
    //            }
    //        }
    //        else
    //        {
    //        }
    //        dsload = d2.select_method_wo_parameter(SelQ, "Text");
    //    }
    //    catch { }
    //    return dsload;
    //}
    //protected void loadGridDet(DataSet ds)
    //{
    //    try
    //    {
    //        DataTable dt = new DataTable();
    //        DataTable dtstud = new DataTable();
    //        DataView dvcours = new DataView();
    //        DataView dvstud = new DataView();
    //        DataView dvCnt = new DataView();
    //        DataView dvpercnt = new DataView();
    //        ArrayList archk = new ArrayList();
    //        Dictionary<int, string> leaveDet = new Dictionary<int, string>();
    //        Dictionary<string, int> MngattCount = new Dictionary<string, int>();
    //        Dictionary<string, int> EveattCount = new Dictionary<string, int>();
    //        Dictionary<string, double> FnlattCount = new Dictionary<string, double>();
    //        int noMaxHrsDay = 0;
    //        int noFstHrsDay = 0;
    //        int noSndHrsDay = 0;
    //        int noMinFstHrsDay = 0;
    //        int noMinSndHrsDay = 0;
    //        int noMinHrsDay = 0;
    //        double totStudCnt = 0;
    //        string compname =string.Empty;
    //        dtstud.Columns.Add("Sno");
    //        dtstud.Columns.Add("Compartment");
    //        dtstud.Columns.Add("Strength");
    //        for (int col = 0; col < cblleve.Items.Count; col++)
    //        {
    //            if (cblleve.Items[col].Selected)
    //            {
    //                dtstud.Columns.Add(cblleve.Items[col].Text);
    //                dtstud.Columns.Add(cblleve.Items[col].Text + "%");
    //                leaveDet.Add(Convert.ToInt32(cblleve.Items[col].Value), cblleve.Items[col].Text);
    //            }
    //        }
    //        dtstud.Columns.Add("Leave Category(%)");
    //        bool TotalHourChk = false;
    //        for (int dsf = 0; dsf < ds.Tables[0].Rows.Count; dsf++)
    //        {
    //            double.TryParse(Convert.ToString(ds.Tables[0].Rows[dsf]["cnt"]), out totStudCnt);
    //            compname = Convert.ToString(ds.Tables[0].Rows[dsf]["Course_name"]);
    //            if (ds.Tables[1].Rows.Count > 0)
    //            {
    //                ds.Tables[1].DefaultView.RowFilter = "course_id='" + Convert.ToString(ds.Tables[0].Rows[dsf]["course_id"]) + "'";
    //                dvcours = ds.Tables[1].DefaultView;
    //                if (dvcours.Count > 0)
    //                {
    //                    for (int dvcf = 0; dvcf < dvcours.Count; dvcf++)
    //                    {
    //                        #region Student detail
    //                        if (ds.Tables[2].Rows.Count > 0)
    //                        {
    //                            if (!archk.Contains(Convert.ToString(dvcours[dvcf]["degree_code"])))
    //                            {
    //                                ds.Tables[2].DefaultView.RowFilter = "degree_code='" + Convert.ToString(dvcours[dvcf]["degree_code"]) + "'";
    //                                dvpercnt = ds.Tables[2].DefaultView;
    //                                archk.Add(Convert.ToString(dvcours[dvcf]["degree_code"]));
    //                                if (dvpercnt.Count > 0)
    //                                {
    //                                    int.TryParse(Convert.ToString(dvpercnt[0]["No_of_Hrs_Per_day"]), out noMaxHrsDay);
    //                                    int.TryParse(Convert.ToString(dvpercnt[0]["no_of_hrs_I_half_day"]), out noFstHrsDay);
    //                                    int.TryParse(Convert.ToString(dvpercnt[0]["no_of_hrs_II_half_day"]), out noSndHrsDay);
    //                                    int.TryParse(Convert.ToString(dvpercnt[0]["min_pres_I_half_day"]), out noMinFstHrsDay);
    //                                    int.TryParse(Convert.ToString(dvpercnt[0]["min_pres_II_half_day"]), out noMinSndHrsDay);
    //                                    int.TryParse(Convert.ToString(dvpercnt[0]["min_hrs_per_day"]), out noMinHrsDay);
    //                                    TotalHourChk = true;
    //                                }
    //                            }
    //                            //student details
    //                            double attVal = 0;
    //                            if (TotalHourChk)
    //                            {
    //                                if (ds.Tables[3].Rows.Count > 0)
    //                                {
    //                                    ds.Tables[3].DefaultView.RowFilter = "roll_no='" + Convert.ToString(dvcours[dvcf]["roll_no"]) + "'";
    //                                    dvstud = ds.Tables[3].DefaultView;
    //                                    if (dvstud.Count > 0)
    //                                    {
    //                                        for (int sel = 0; sel < noMaxHrsDay; sel++)
    //                                        {
    //                                            if (sel < noFstHrsDay)
    //                                            {
    //                                                double.TryParse(Convert.ToString(dvstud[0][sel]), out attVal);
    //                                                if (attVal != 0 || attVal != 0.0)
    //                                                {
    //                                                    string val = leaveDet[Convert.ToInt32(attVal)];
    //                                                    if (!MngattCount.ContainsKey(val))
    //                                                        MngattCount.Add(Convert.ToString(val), 1);
    //                                                    else
    //                                                    {
    //                                                        int Cnt = 0;
    //                                                        int.TryParse(Convert.ToString(MngattCount[val]), out Cnt);
    //                                                        Cnt += 1;
    //                                                        MngattCount.Remove(val);
    //                                                        MngattCount.Add(Convert.ToString(val), Cnt);
    //                                                    }
    //                                                }
    //                                            }
    //                                            else if (sel >= noSndHrsDay)
    //                                            {
    //                                                double.TryParse(Convert.ToString(dvstud[0][sel]), out attVal);
    //                                                if (attVal != 0 || attVal != 0.0)
    //                                                {
    //                                                    string val = leaveDet[Convert.ToInt32(attVal)];
    //                                                    if (!EveattCount.ContainsKey(val))
    //                                                        EveattCount.Add(Convert.ToString(val), 1);
    //                                                    else
    //                                                    {
    //                                                        int Cnt = 0;
    //                                                        int.TryParse(Convert.ToString(EveattCount[val]), out Cnt);
    //                                                        Cnt += 1;
    //                                                        EveattCount.Remove(val);
    //                                                        EveattCount.Add(Convert.ToString(val), Cnt);
    //                                                    }
    //                                                }
    //                                            }
    //                                        }
    //                                        //every single student attendance adding here
    //                                        string getAttVal = string.Empty;
    //                                        getAttVal = attendanceVal(MngattCount, noMinFstHrsDay);
    //                                        if (!FnlattCount.ContainsKey(getAttVal))
    //                                            FnlattCount.Add(Convert.ToString(getAttVal), 0.5);
    //                                        else
    //                                        {
    //                                            double Cnt = 0;
    //                                            double.TryParse(Convert.ToString(FnlattCount[getAttVal]), out Cnt);
    //                                            Cnt += 0.5;
    //                                            FnlattCount.Remove(getAttVal);
    //                                            FnlattCount.Add(Convert.ToString(getAttVal), Cnt);
    //                                        }
    //                                        getAttVal = attendanceVal(EveattCount, noMinSndHrsDay);
    //                                        if (!FnlattCount.ContainsKey(getAttVal))
    //                                            FnlattCount.Add(Convert.ToString(getAttVal), 0.5);
    //                                        else
    //                                        {
    //                                            double Cnt = 0;
    //                                            double.TryParse(Convert.ToString(FnlattCount[getAttVal]), out Cnt);
    //                                            Cnt += 0.5;
    //                                            FnlattCount.Remove(getAttVal);
    //                                            FnlattCount.Add(Convert.ToString(getAttVal), Cnt);
    //                                        }
    //                                        //clear
    //                                        MngattCount.Clear();
    //                                        EveattCount.Clear();
    //                                    }
    //                                }
    //                            }
    //                        }
    //                        #endregion
    //                    }
    //                    /////
    //                    //value bind in datatable
    //                    if (dtstud.Columns.Count > 0 && FnlattCount.Count > 0)
    //                    {
    //                        dt = loadDatatable(dtstud, FnlattCount, totStudCnt, compname);
    //                    }
    //                }
    //            }
    //        }
    //        if (dt.Rows.Count > 0)
    //        {
    //            gdattrpt.DataSource = dt;
    //            gdattrpt.DataBind();
    //            gdattrpt.Visible = true;
    //            btnExport.Visible = true;
    //            pnlContents.Visible = true;
    //        }
    //    }
    //    catch { }
    //}
    //protected string attendanceVal(Dictionary<string, int> attVal, int hrsCnt)
    //{
    //    //double Cnt = 0;
    //    string stratts =string.Empty;
    //    try
    //    {
    //        int aCnt = 0;
    //        int odCnt = 0;
    //        int mlCnt = 0;
    //        int sodCnt = 0;
    //        int nssCnt = 0;
    //        int lCnt = 0;
    //        foreach (KeyValuePair<string, int> val in attVal)
    //        {
    //            stratts = val.Key.ToString();
    //            string strval = val.Value.ToString();
    //            if (stratts == "P")
    //            {
    //                if (hrsCnt >= Convert.ToInt32(strval))
    //                    break;
    //            }
    //            else if (stratts == "A")
    //            {
    //                if (hrsCnt >= Convert.ToInt32(strval))
    //                    break;
    //                else
    //                    aCnt++;
    //            }
    //            else if (stratts == "OD")
    //            {
    //                if (hrsCnt >= Convert.ToInt32(strval))
    //                    break;
    //                else
    //                    odCnt++;
    //            }
    //            else if (stratts == "ML")
    //            {
    //                if (hrsCnt >= Convert.ToInt32(strval))
    //                    break;
    //                else
    //                    mlCnt++;
    //            }
    //            else if (stratts == "SOD")
    //            {
    //                if (hrsCnt >= Convert.ToInt32(strval))
    //                    break;
    //                else
    //                    sodCnt++;
    //            }
    //            else if (stratts == "NSS")
    //            {
    //                if (hrsCnt >= Convert.ToInt32(strval))
    //                    break;
    //                else
    //                    nssCnt++;
    //            }
    //            else if (stratts == "L")
    //            {
    //                if (hrsCnt >= Convert.ToInt32(strval))
    //                    break;
    //                else
    //                    lCnt++;
    //            }
    //        }
    //        //final value
    //        if (aCnt > odCnt && aCnt > mlCnt && aCnt > sodCnt && aCnt > nssCnt && aCnt > lCnt)
    //            stratts = "A";
    //        else if (odCnt > aCnt && odCnt > mlCnt && odCnt > sodCnt && odCnt > nssCnt && odCnt > lCnt)
    //            stratts = "OD";
    //        else if (mlCnt > aCnt && mlCnt > odCnt && mlCnt > sodCnt && mlCnt > nssCnt && mlCnt > lCnt)
    //            stratts = "ML";
    //        else if (sodCnt > aCnt && sodCnt > odCnt && sodCnt > mlCnt && sodCnt > nssCnt && sodCnt > lCnt)
    //            stratts = "SOD";
    //        else if (nssCnt > aCnt && nssCnt > odCnt && nssCnt > mlCnt && nssCnt > sodCnt && nssCnt > lCnt)
    //            stratts = "NSS";
    //        else if (lCnt > aCnt && lCnt > odCnt && lCnt > mlCnt && lCnt > sodCnt && lCnt > nssCnt)
    //            stratts = "NSS";
    //    }
    //    catch { }
    //    return stratts;
    //}
    //protected DataTable loadDatatable(DataTable dtstud, Dictionary<string, double> FnlattCount, double totStudCnt, string compname)
    //{
    //    try
    //    {
    //        DataRow drstud;
    //        drstud = dtstud.NewRow();
    //        drstud["Sno"] = Convert.ToString(dtstud.Rows.Count + 1);
    //        drstud["Compartment"] = compname;
    //        drstud["Strength"] = Convert.ToString(totStudCnt);
    //        dtstud.Rows.Add(drstud);
    //        double tempStudCnt = 0;
    //        double tempperc = 0;
    //        for (int i = 3; i < dtstud.Columns.Count; i++)
    //        {
    //            string colname = dtstud.Columns[i].ColumnName;
    //            if (!colname.Contains('%'))
    //            {
    //                double.TryParse(Convert.ToString(FnlattCount.ContainsKey(colname) ? FnlattCount[colname].ToString() : ""), out tempStudCnt);
    //                drstud[colname] = Convert.ToString(tempStudCnt);
    //            }
    //            else
    //            {
    //                double perCnt = (tempStudCnt / totStudCnt) * 100;
    //                drstud[colname] = Convert.ToString(Math.Round(perCnt, 2));
    //                if (!colname.Contains("P%"))
    //                {
    //                    tempperc += perCnt;
    //                }
    //            }
    //            //  drstud[colname] = FnlattCount.ContainsKey(colname) ? FnlattCount[colname].ToString() : "";
    //        }
    //        drstud["Leave Category(%)"] = Convert.ToString(Math.Round(tempperc, 2));
    //    }
    //    catch { }
    //    return dtstud;
    //}

    #endregion

}
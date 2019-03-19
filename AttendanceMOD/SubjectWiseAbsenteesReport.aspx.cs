using System;
using System.Linq;
using System.Web.UI.WebControls;
using System.Collections;
using System.Data;
using System.Drawing;
using System.Web.UI;

public partial class SubjectWiseAbsenteesReport : System.Web.UI.Page
{
    DAccess2 d2 = new DAccess2();
    DataSet ds = new DataSet();
    string usercode = string.Empty;
    string collegecode = string.Empty;
    string singleuser = string.Empty;
    string group_user = string.Empty;
    string course_id = string.Empty;
    string strbatch = string.Empty;
    string strbranch = string.Empty;
    string[] stringArray;
    string[] arrsubno;
    //added by rajasekar 22/08/2018
    DataTable dtl = new DataTable();
    DataRow dtrow = null;

    //=============================//

    protected void Page_Load(object sender, EventArgs e)
    {

        if (Session["collegecode"] == null)
        {
            Response.Redirect("~/Default.aspx");
        }
        usercode = Session["usercode"].ToString();
        collegecode = Session["collegecode"].ToString();
        singleuser = Session["single_user"].ToString();
        group_user = Session["group_code"].ToString();
        if (!IsPostBack)
        {
            txtfdate.Attributes.Add("readonly", "readonly");
            txttdate.Attributes.Add("readonly", "readonly");
            txtfdate.Text = DateTime.Now.ToString("dd/MM/yyyy");
            txttdate.Text = DateTime.Now.ToString("dd/MM/yyyy");
            BindBatch();
            BindDegree();
            BindBranch();
            BindSem();
            binddate();
            BindSectionDetail(strbatch, strbranch);
            clear();
        }
    }
    public void BindBatch()
    {
        try
        {
            ds.Dispose();
            ds.Reset();
            ds = d2.BindBatch();
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
            errmsg.Text = ex.ToString();
        }
    }
    public void BindDegree()
    {
        try
        {
            ddldegree.Items.Clear();
            collegecode = Session["collegecode"].ToString();
            if (group_user.Contains(';'))
            {
                string[] group_semi = group_user.Split(';');
                group_user = group_semi[0].ToString();
            }
            ds.Dispose();
            ds.Reset();
            ds = d2.BindDegree(singleuser, group_user, collegecode, usercode);
            if (ds.Tables[0].Rows.Count > 0)
            {
                ddldegree.DataSource = ds;
                ddldegree.DataTextField = "course_name";
                ddldegree.DataValueField = "course_id";
                ddldegree.DataBind();
            }
        }
        catch (Exception ex)
        {
            errmsg.Text = ex.ToString();
        }
    }

    public void BindBranch()
    {
        try
        {
            course_id = ddldegree.SelectedValue.ToString();
            collegecode = Session["collegecode"].ToString();
            ddlbranch.Items.Clear();
            if (group_user.Contains(';'))
            {
                string[] group_semi = group_user.Split(';');
                group_user = group_semi[0].ToString();
            }
            ds.Dispose();
            ds.Reset();
            ds = d2.BindBranch(singleuser, group_user, course_id, collegecode, usercode);
            if (ds.Tables[0].Rows.Count > 0)
            {
                ddlbranch.DataSource = ds;
                ddlbranch.DataTextField = "dept_name";
                ddlbranch.DataValueField = "degree_code";
                ddlbranch.DataBind();
            }
        }
        catch (Exception ex)
        {
            errmsg.Text = ex.ToString();
        }
    }


    public void BindSem()
    {

        try
        {
            strbatch = ddlbatch.Text.ToString();
            strbranch = ddlbranch.SelectedValue.ToString();
            collegecode = Session["collegecode"].ToString();
            ddlsem.Items.Clear();
            Boolean first_year;
            first_year = false;
            int duration = 0;
            int i = 0;
            ds.Dispose();
            ds.Reset();
            ds = d2.BindSem(strbranch, strbatch, collegecode);
            if (ds.Tables[0].Rows.Count > 0)
            {
                first_year = Convert.ToBoolean(Convert.ToString(ds.Tables[0].Rows[0][1]).ToString());
                duration = Convert.ToInt32(Convert.ToString(ds.Tables[0].Rows[0][0]).ToString());
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
        catch (Exception ex)
        {
            errmsg.Text = ex.ToString();
        }
    }
    public void binddate()
    {
        try
        {

            ds.Dispose();
            ds.Reset();
            ds = d2.select_method_wo_parameter("select convert(nvarchar(15),start_date,103) as sdate,convert(nvarchar(15),end_date,103) as edate from seminfo where degree_code=" + ddlbranch.SelectedValue.ToString() + " and semester=" + ddlsem.SelectedValue.ToString() + " and batch_year=" + ddlbatch.SelectedValue.ToString() + "", "Text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                txtfdate.Text = ds.Tables[0].Rows[0]["sdate"].ToString();
                txttdate.Text = ds.Tables[0].Rows[0]["edate"].ToString();
            }
            else
            {
                txtfdate.Text = DateTime.Now.ToString("dd/MM/yyyy");
                txttdate.Text = DateTime.Now.ToString("dd/MM/yyyy");
            }
        }
        catch
        {
        }
    }

    public void BindSectionDetail(string strbatch, string strbranch)
    {
        try
        {
            errmsg.Visible = false;
            strbatch = ddlbatch.SelectedValue.ToString();
            strbranch = ddlbranch.SelectedValue.ToString();

            ddlsec.Items.Clear();
            ds.Dispose();
            ds.Reset();
            ds = d2.BindSectionDetail(strbatch, strbranch);
            if (ds.Tables[0].Rows.Count > 0)
            {
                ddlsec.DataSource = ds;
                ddlsec.DataTextField = "sections";
                ddlsec.DataBind();
                if (Convert.ToString(ds.Tables[0].Columns["sections"]) == string.Empty)
                {
                    ddlsec.Enabled = false;
                }
                else
                {
                    ddlsec.Enabled = true;
                }
            }
            else
            {
                ddlsec.Enabled = false;
            }
        }
        catch (Exception ex)
        {
            errmsg.Text = ex.ToString();
            errmsg.Visible = true;
        }
    }
    protected void ddlbatch_SelectedIndexChanged(object sender, EventArgs e)
    {
        clear();
        BindDegree();
        BindBranch();
        BindSem();
        binddate();
        BindSectionDetail(strbatch, strbranch);
    }
    protected void ddldegree_SelectedIndexChanged(object sender, EventArgs e)
    {
        clear();
        BindBranch();
        BindSem();
        binddate();
        BindSectionDetail(strbatch, strbranch);
    }
    protected void ddlbranch_SelectedIndexChanged(object sender, EventArgs e)
    {
        clear();
        BindSem();
        binddate();
        BindSectionDetail(strbatch, strbranch);
    }
    protected void ddlsem_SelectedIndexChanged(object sender, EventArgs e)
    {
        clear();
        binddate();
        BindSectionDetail(strbatch, strbranch);
    }
    protected void ddlsec_SelectedIndexChanged(object sender, EventArgs e)
    {
        clear();
    }

    public void clear()
    {
        

        grdover.Visible = false;
        lblrptname.Visible = false;
        txtexcelname.Visible = false;
        btnxl.Visible = false;
        btnmasterprint.Visible = false;
        btnPrint.Visible = false;
        Printcontrol.Visible = false;
        txtexcelname.Text = "";
        errmsg.Visible = false;
    }
    protected void txtfdate_TextChanged(object sender, EventArgs e)
    {
        try
        {
            clear();
            string fdtae = txtfdate.Text.ToString();
            string[] spf = fdtae.Split('/');
            DateTime dtf = Convert.ToDateTime(spf[1] + '/' + spf[0] + '/' + spf[2]);
            string tdtae = txttdate.Text.ToString();
            string[] spt = tdtae.Split('/');
            DateTime dtt = Convert.ToDateTime(spt[1] + '/' + spt[0] + '/' + spt[2]);
            string currdate = DateTime.Now.ToString("MM/dd/yyyy");
            DateTime dtnow = Convert.ToDateTime(currdate);

            if (dtf > dtnow)
            {
                txtfdate.Text = dtnow.ToString("dd/MM/yyyy");
                txttdate.Text = dtnow.ToString("dd/MM/yyyy");
                errmsg.Visible = true;
                errmsg.Text = "From Date Must Be Lesser Than Or Equal To Current Date";
                return;
            }
            if (dtt > dtnow)
            {
                txtfdate.Text = dtnow.ToString("dd/MM/yyyy");
                txttdate.Text = dtnow.ToString("dd/MM/yyyy");
                errmsg.Visible = true;
                errmsg.Text = "From Date Must Be Lesser Than Or Equal To Current Date";
                return;
            }


            if (dtt < dtf)
            {
                txtfdate.Text = dtt.ToString("dd/MM/yyyy");
                errmsg.Visible = true;
                errmsg.Text = "From Date Must Be Lesser Than To Date";
            }
        }
        catch (Exception ex)
        {
            errmsg.Visible = true;
            errmsg.Text = ex.ToString();
        }
    }
    protected void txttdate_TextChanged(object sender, EventArgs e)
    {
        try
        {
            clear();
            string fdtae = txtfdate.Text.ToString();
            string[] spf = fdtae.Split('/');
            DateTime dtf = Convert.ToDateTime(spf[1] + '/' + spf[0] + '/' + spf[2]);
            string tdtae = txttdate.Text.ToString();
            string[] spt = tdtae.Split('/');
            DateTime dtt = Convert.ToDateTime(spt[1] + '/' + spt[0] + '/' + spt[2]);
            string currdate = DateTime.Now.ToString("MM/dd/yyyy");
            DateTime dtnow = Convert.ToDateTime(currdate);

            if (dtf > dtnow)
            {
                txtfdate.Text = dtnow.ToString("dd/MM/yyyy");
                txttdate.Text = dtnow.ToString("dd/MM/yyyy");
                errmsg.Visible = true;
                errmsg.Text = "From Date Must Be Lesser Than Or Equal To Current Date";
                return;
            }
            if (dtt > dtnow)
            {
                txtfdate.Text = dtnow.ToString("dd/MM/yyyy");
                txttdate.Text = dtnow.ToString("dd/MM/yyyy");
                errmsg.Visible = true;
                errmsg.Text = "From Date Must Be Lesser Than Or Equal To Current Date";
                return;
            }
            if (dtt < dtf)
            {
                txtfdate.Text = dtt.ToString("dd/MM/yyyy");
                errmsg.Visible = true;
                errmsg.Text = "From Date Must Be Lesser Than To Date";
            }
        }
        catch (Exception ex)
        {
            errmsg.Visible = true;
            errmsg.Text = ex.ToString();
        }
    }
    protected void btngo_Click(object sender, EventArgs e)
    {
        try
        {
            clear();
            btnPrint11();
            
            dtrow = dtl.NewRow();
            dtl.Rows.Add(dtrow);
            int coln = 0;

            //added by rajasekar 28/08/2018
            
            dtl.Columns.Add("S.No", typeof(string));

            dtl.Rows[0][coln] = "S.No";
            coln++;

            dtl.Columns.Add("Date", typeof(string));
            
            dtl.Rows[0][coln] = "Date";
            coln++;
            //=========================//


            string batchyear = ddlbatch.Text.ToString();
            string degreecode = ddlbranch.Text.ToString();
            string sem = ddlsem.SelectedValue.ToString();
            string strsec = ddlsec.SelectedValue.ToString();
            string secval = "";
            if (strsec != "" && strsec != "-1" && strsec != "All")
            {
                strsec = " and Sections='" + strsec + "'";
                secval = strsec;
            }
            else
            {
                strsec = "";
            }
            Hashtable hatvalue = new Hashtable();
            Hashtable hatstudet = new Hashtable();
            DataSet dsstuatt = new DataSet();

            string fdtae = txtfdate.Text.ToString();
            string[] spf = fdtae.Split('/');
            DateTime dtf = Convert.ToDateTime(spf[1] + '/' + spf[0] + '/' + spf[2]);
            string tdtae = txttdate.Text.ToString();
            string[] spt = tdtae.Split('/');
            DateTime dtt = Convert.ToDateTime(spt[1] + '/' + spt[0] + '/' + spt[2]);
            Hashtable hatlabsub = new Hashtable();

            string strquery = "select ss.subject_type,ss.subType_no,ss.lab,s.subject_name,s.subject_code,s.subject_no from syllabus_master sy,sub_sem ss,subject s where sy.syll_code=ss.syll_code and ss.syll_code=s.syll_code and sy.syll_code=s.syll_code and ss.subType_no=s.subType_no and sy.Batch_Year='" + batchyear + "' and sy.degree_code='" + degreecode + "' and sy.semester='" + sem + "' order by ss.subType_no,s.subject_name,s.subject_code";
            ds.Dispose();
            ds.Reset();
            ds = d2.select_method_wo_parameter(strquery, "Text");
            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            {
                
                //added by rajasekar 28/08/2018

                dtl.Columns.Add(ds.Tables[0].Rows[i]["subject_code"].ToString(), typeof(string));

                dtl.Rows[0][coln] = ds.Tables[0].Rows[i]["subject_code"].ToString();
                coln++;
                if(i==0)
                    arrsubno = new string[ds.Tables[0].Rows.Count+3];
                arrsubno[i+2] = ds.Tables[0].Rows[i]["subject_no"].ToString();
               
                

                //======================//

                if (!hatlabsub.ContainsKey(ds.Tables[0].Rows[i]["subject_no"].ToString()))
                {
                    hatlabsub.Add(ds.Tables[0].Rows[i]["subject_no"].ToString(), ds.Tables[0].Rows[i]["lab"].ToString());
                }
            }
            

            dtl.Columns.Add("Remarks", typeof(string));//added by rajasekar 28/08/2018

             dtl.Rows[0][coln] = "Remarks";
                coln++;
            

            grdover.Visible = true;
            
            string[] Days = new string[7] { "mon", "tue", "wed", "thu", "fri", "sat", "sun" };

            strquery = "select start_date sdate,end_date edate,starting_dayorder from seminfo where batch_year='" + batchyear + "' and degree_code='" + degreecode + "' and semester='" + sem + "';";
            strquery = strquery + " select * from Semester_Schedule where batch_year='" + batchyear + "' and degree_code='" + degreecode + "' and semester='" + sem + "' " + strsec + " order by FromDate desc;";
            strquery = strquery + " select * from Alternate_Schedule where batch_year='" + batchyear + "' and degree_code='" + degreecode + "' and semester='" + sem + "' " + strsec + " order by FromDate desc;";
            strquery = strquery + " select * from holidayStudents where degree_code='" + degreecode + "' and semester='" + sem + "' and holiday_date between '" + dtf + "' and '" + dtt + "'";
            strquery = strquery + " select schorder,nodays,No_of_hrs_per_day,no_of_hrs_I_half_day,no_of_hrs_II_half_day from PeriodAttndSchedule where degree_code='" + degreecode + "' and semester='" + sem + "'";
            ds.Dispose();
            ds.Reset();
            ds = d2.select_method_wo_parameter(strquery, "Text");

            string strsubstucount = " select count(distinct r.Roll_No) as stucount,r.Batch_Year,r.degree_code,s.Semester,r.Sections,s.subject_no,r.adm_date from registration r,subjectchooser s where  r.roll_no=s.roll_no and  r.current_semester=s.semester";
            strsubstucount = strsubstucount + " and batch_year='" + batchyear + "' and  degree_code='" + degreecode + "'  and semester='" + sem + "'  " + strsec + " and cc=0 and delflag=0 and exam_flag<>'debar' group by r.Batch_Year,r.degree_code,s.semester,r.Sections,s.subject_no,r.adm_date";
            DataSet dssubstucount = d2.select_method_wo_parameter(strsubstucount, "Text");

            hatvalue.Clear();
            hatvalue.Add("colege_code", Session["collegecode"].ToString());
            DataSet ds1 = d2.select_method("ATT_MASTER_SETTING", hatvalue, "sp");

            string absenteescode = "";

            for (int i = 0; i < ds1.Tables[0].Rows.Count; i++)
            {
                if (ds1.Tables[0].Rows[i]["calcflag"].ToString() == "1")
                {
                    if (absenteescode == "")
                    {
                        absenteescode = ds1.Tables[0].Rows[i]["leavecode"].ToString();
                    }
                    else
                    {
                        absenteescode = absenteescode + "," + ds1.Tables[0].Rows[i]["leavecode"].ToString();
                    }
                }
            }


            if (ds.Tables[0].Rows.Count > 0)
            {
                DateTime dtsdate = Convert.ToDateTime(ds.Tables[0].Rows[0]["sdate"].ToString());
                DateTime dtedate = Convert.ToDateTime(ds.Tables[0].Rows[0]["edate"].ToString());
                string start_dayorder = ds.Tables[0].Rows[0]["starting_dayorder"].ToString();

                if (dtsdate <= dtf || dtedate >= dtf)
                {

                    if (ds.Tables[4].Rows.Count > 0)
                    {
                        string SchOrder = ds.Tables[4].Rows[0]["schorder"].ToString();
                        int noofdays = Convert.ToInt32(ds.Tables[4].Rows[0]["nodays"].ToString());

                        int noofhrs = Convert.ToInt32(ds.Tables[4].Rows[0]["No_of_hrs_per_day"].ToString());
                        int frshrs = Convert.ToInt32(ds.Tables[4].Rows[0]["no_of_hrs_I_half_day"].ToString());
                        int schrs = Convert.ToInt32(ds.Tables[4].Rows[0]["no_of_hrs_II_half_day"].ToString());

                        int srno = 0;
                        for (DateTime dt = dtf; dt <= dtt; dt = dt.AddDays(1))
                        {
                            if (dtedate >= dt && dtsdate <= dt)
                            {


                                srno++;
                                

                                dtrow = dtl.NewRow();
                                int col = 0;


                                dtrow[col] = srno.ToString();
                                col++;

                                dtrow[col] = dt.ToString("dd/MM/yyyy");
                                col++;

                                string strday = dt.ToString("ddd");
                                long strdate = (Convert.ToInt32(dt.ToString("MM")) + Convert.ToInt32(dt.ToString("yyyy")) * 12);
                                if (SchOrder == "0")
                                {
                                    string[] sps = dtt.ToString().Split('/');
                                    string curdate = sps[0] + '/' + sps[1] + '/' + sps[2];
                                    strday = d2.findday(dt.ToString(), degreecode, sem, batchyear, dtsdate.ToString(), noofdays.ToString(), start_dayorder);
                                }
                                Boolean moringleav = false;
                                Boolean evenleave = false;
                                string holidayreson = "";

                                int starthour = 1;
                                int endhour = noofhrs;

                                ds.Tables[3].DefaultView.RowFilter = "holiday_date='" + dt + "' ";
                                DataView dvholiday = ds.Tables[3].DefaultView;
                                if (dvholiday.Count > 0)
                                {
                                    if (dvholiday[0]["morning"].ToString() == "1" || dvholiday[0]["morning"].ToString().Trim().ToLower() == "true")
                                    {
                                        moringleav = true;
                                        starthour = frshrs + 1;
                                    }
                                    if (dvholiday[0]["evening"].ToString() == "1" || dvholiday[0]["evening"].ToString().Trim().ToLower() == "true")
                                    {
                                        evenleave = true;
                                        endhour = frshrs;
                                    }
                                    if (dvholiday[0]["halforfull"].ToString() == "0" || dvholiday[0]["halforfull"].ToString().Trim().ToLower() == "false")
                                    {
                                        evenleave = true;
                                        moringleav = true;
                                    }
                                    holidayreson = dvholiday[0]["holiday_desc"].ToString();
                                }
                                if (strday.Trim().ToLower() == "sun")
                                {
                                    holidayreson = "Sunday";
                                }
                                if ((moringleav == false || evenleave == false) && strday.Trim().ToLower() != "sun")
                                {
                                    ds.Tables[1].DefaultView.RowFilter = " fromdate<='" + dtt + "'";
                                    DataView dvsemsched = ds.Tables[1].DefaultView;

                                    ds.Tables[2].DefaultView.RowFilter = " fromdate='" + dtt + "'";
                                    DataView dvalternaet = ds.Tables[2].DefaultView;

                                    for (int hr = starthour; hr <= endhour; hr++)
                                    {
                                        string Att_dcolumn = "d" + dt.Day.ToString() + "d" + hr;
                                        string sp_rd = "";
                                        if (dvalternaet.Count > 0)
                                        {
                                            sp_rd = dvalternaet[0][strday + hr].ToString();
                                        }
                                        if (sp_rd.Trim() != "" && sp_rd.Trim() != "0" && sp_rd != null)
                                        {
                                            string[] sp_rd_split = sp_rd.Split(';');
                                            for (int index = 0; index <= sp_rd_split.GetUpperBound(0); index++)
                                            {
                                                string[] sp2 = sp_rd_split[index].Split(new Char[] { '-' });
                                                if (sp2.GetUpperBound(0) >= 1)
                                                {
                                                    string subno = sp2[0].ToString();
                                                    if (hatlabsub.ContainsKey(subno))
                                                    {
                                                        string lab = hatlabsub[subno].ToString();
                                                        if (lab == "1" || lab.Trim().ToLower() == "true")
                                                        {
                                                            string strgetatt = "select distinct r.Roll_No from registration r,attendance a,subjectchooser_new s where degree_code='" + degreecode + "' and current_semester='" + sem + "' and batch_year='" + batchyear + "' and cc=0 and delflag=0 and  exam_flag<>'debar' and month_year=" + strdate + " and r.roll_no=a.roll_no and  r.roll_no=s.roll_no and";
                                                            strgetatt = strgetatt + " r.current_semester=s.semester and subject_no='" + sp2[0].ToString() + "' " + strsec + " and(" + Att_dcolumn + " is not null and " + Att_dcolumn + "<>'0' and " + Att_dcolumn + "<>'') and fromdate='" + dt + "' and batch    in(select stu_batch from laballoc_new where subject_no='" + sp2[0].ToString() + "'  and batch_year='" + batchyear + "'  and hour_value='" + hr + "'  and    degree_code='" + degreecode + "' ";
                                                            strgetatt = strgetatt + " and day_value='" + strday + "' and semester='" + sem + "' " + strsec + " and fdate='" + dt + "') and adm_date<='" + dt + "'";
                                                            dsstuatt = d2.select_method_wo_parameter(strgetatt, "Text");
                                                            if (dsstuatt.Tables[0].Rows.Count > 0)
                                                            {
                                                                strgetatt = "select distinct r.Roll_No,r.reg_no,r.stud_name from registration r,attendance a,subjectchooser_new s where degree_code='" + degreecode + "' and current_semester='" + sem + "' and batch_year='" + batchyear + "' and cc=0 and delflag=0 and  exam_flag<>'debar' and month_year=" + strdate + " and r.roll_no=a.roll_no and  r.roll_no=s.roll_no and";
                                                                strgetatt = strgetatt + " r.current_semester=s.semester and subject_no='" + sp2[0].ToString() + "' " + strsec + "  and " + Att_dcolumn + " in(" + absenteescode + ") and fromdate='" + dt + "' and batch    in(select stu_batch from laballoc_new where subject_no='" + sp2[0].ToString() + "'  and batch_year='" + batchyear + "'  and hour_value='" + hr + "'  and    degree_code='" + degreecode + "' ";
                                                                strgetatt = strgetatt + " and day_value='" + strday + "' and semester='" + sem + "' " + strsec + " and fdate='" + dt + "') and adm_date<='" + dt + "'";
                                                                dsstuatt = d2.select_method_wo_parameter(strgetatt, "Text");
                                                                if (dsstuatt.Tables[0].Rows.Count > 0)
                                                                {
                                                                    for (int s = 0; s < dsstuatt.Tables[0].Rows.Count; s++)
                                                                    {
                                                                        if (hatstudet.ContainsKey(subno))
                                                                        {
                                                                            if (s == 0)
                                                                            {
                                                                                hatstudet[subno] = hatstudet[subno] + " (Lab Period: " + hr + ") " + dsstuatt.Tables[0].Rows[s]["roll_no"].ToString();
                                                                            }
                                                                            else
                                                                            {
                                                                                hatstudet[subno] = hatstudet[subno] + ", " + dsstuatt.Tables[0].Rows[s]["roll_no"].ToString();
                                                                            }
                                                                        }
                                                                        else
                                                                        {
                                                                            hatstudet.Add(subno, "(Lab Period: " + hr + ") " + dsstuatt.Tables[0].Rows[s]["roll_no"].ToString());
                                                                        }
                                                                    }
                                                                }
                                                                else
                                                                {
                                                                    if (hatstudet.ContainsKey(subno))
                                                                    {
                                                                        hatstudet[subno] = hatstudet[subno] + " (Lab Period: " + hr + ") Nil";
                                                                    }
                                                                    else
                                                                    {
                                                                        hatstudet.Add(subno, "(Lab Period: " + hr + ") Nil");
                                                                    }
                                                                }
                                                            }
                                                            else
                                                            {
                                                                if (hatstudet.ContainsKey(subno))
                                                                {
                                                                    hatstudet[subno] = hatstudet[subno] + " (Lab Period: " + hr + "  Please Allot the Subject and Batch For Student)";
                                                                }
                                                                else
                                                                {
                                                                    hatstudet.Add(subno, "(Lab Period: " + hr + " Please Allot the Subject and Batch For Student)");
                                                                }
                                                            }
                                                        }
                                                        else
                                                        {
                                                            hatvalue.Clear();
                                                            hatvalue.Add("batch_year", batchyear);
                                                            hatvalue.Add("degree_code", degreecode);
                                                            hatvalue.Add("sem", sem);
                                                            hatvalue.Add("sections", secval);
                                                            hatvalue.Add("month_year", strdate);
                                                            hatvalue.Add("date", dt);
                                                            hatvalue.Add("subject_no", sp2[0]);
                                                            dssubstucount.Tables[0].DefaultView.RowFilter = "subject_no='" + sp2[0] + "' and adm_date<='" + dtt.ToString("MM/dd/yyyy").ToString() + "' ";//added admissiondare
                                                            DataView dvsubstucount = dssubstucount.Tables[0].DefaultView;
                                                            if (dvsubstucount.Count > 0)
                                                            {
                                                                string strgetatt = "select distinct registration.roll_no," + Att_dcolumn + " from registration,attendance,subjectchooser s where registration.roll_no=attendance.roll_no and  registration.roll_no=s.roll_no ";
                                                                strgetatt = strgetatt + " and registration.current_semester=s.semester and subject_no='" + sp2[0] + "' and degree_code='" + degreecode + "' and s.semester='" + sem + "' and batch_year='" + batchyear + "' " + strsec + " ";
                                                                strgetatt = strgetatt + " and cc=0 and delflag=0 and exam_flag<>'debar' and month_year='" + strdate + "' and (" + Att_dcolumn + " is not null and " + Att_dcolumn + "<>'0' and " + Att_dcolumn + "<>'') and adm_date<='" + dt + "' ";
                                                                dsstuatt = d2.select_method_wo_parameter(strgetatt, "Text");
                                                                if (dsstuatt.Tables[0].Rows.Count > 0)
                                                                {
                                                                    strgetatt = "select distinct registration.Roll_No,registration.reg_no,registration.stud_name," + Att_dcolumn + " from registration,attendance,subjectchooser s where registration.roll_no=attendance.roll_no and  registration.roll_no=s.roll_no ";
                                                                    strgetatt = strgetatt + " and registration.current_semester=s.semester and subject_no='" + sp2[0] + "' and degree_code='" + degreecode + "' and s.semester='" + sem + "' and batch_year='" + batchyear + "' " + strsec + " ";
                                                                    strgetatt = strgetatt + " and cc=0 and delflag=0 and exam_flag<>'debar' and month_year='" + strdate + "' and " + Att_dcolumn + " in(" + absenteescode + ") and adm_date<='" + dt + "' ";
                                                                    dsstuatt = d2.select_method_wo_parameter(strgetatt, "Text");
                                                                    if (dsstuatt.Tables[0].Rows.Count > 0)
                                                                    {
                                                                        for (int s = 0; s < dsstuatt.Tables[0].Rows.Count; s++)
                                                                        {
                                                                            if (hatstudet.ContainsKey(subno))
                                                                            {
                                                                                hatstudet[subno] = hatstudet[subno] + ", " + dsstuatt.Tables[0].Rows[s]["reg_no"].ToString();
                                                                            }
                                                                            else
                                                                            {
                                                                                hatstudet.Add(subno, "(Period: " + hr + ")  " + dsstuatt.Tables[0].Rows[s]["reg_no"].ToString());
                                                                            }
                                                                        }
                                                                    }
                                                                    else
                                                                    {
                                                                        if (hatstudet.ContainsKey(subno))
                                                                        {
                                                                            hatstudet[subno] = hatstudet[subno] + " (Lab Period: " + hr + ") Nil";
                                                                        }
                                                                        else
                                                                        {
                                                                            hatstudet.Add(subno, "(Lab Period: " + hr + ") Nil");
                                                                        }
                                                                    }
                                                                }
                                                                else
                                                                {
                                                                    if (hatstudet.ContainsKey(subno))
                                                                    {
                                                                        hatstudet[subno] = hatstudet[subno] + " (Period: " + hr + "  Attendance Not Entered)";
                                                                    }
                                                                    else
                                                                    {
                                                                        hatstudet.Add(subno, "(Period: " + hr + " Attendance Not Entered)");
                                                                    }
                                                                }
                                                            }
                                                            else
                                                            {
                                                                if (hatstudet.ContainsKey(subno))
                                                                {
                                                                    hatstudet[subno] = hatstudet[subno] + " (Period: " + hr + "  Please Allot the Subject For Student)";
                                                                }
                                                                else
                                                                {
                                                                    hatstudet.Add(subno, "(Period: " + hr + "  Please Allot the Subject For Student)");
                                                                }
                                                            }
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                        else
                                        {
                                            string timetable = "";
                                            if (dvsemsched.Count > 0)
                                            {
                                                sp_rd = dvsemsched[0][strday + hr].ToString();
                                                timetable = dvsemsched[0]["ttname"].ToString();
                                            }
                                            if (sp_rd.Trim() != "" && sp_rd.Trim() != "0" && sp_rd != null)
                                            {
                                                string[] sp_rd_split = sp_rd.Split(';');
                                                for (int index = 0; index <= sp_rd_split.GetUpperBound(0); index++)
                                                {
                                                    string[] sp2 = sp_rd_split[index].Split(new Char[] { '-' });
                                                    if (sp2.GetUpperBound(0) >= 1)
                                                    {
                                                        string subno = sp2[0].ToString();
                                                        if (hatlabsub.ContainsKey(subno))
                                                        {
                                                            string lab = hatlabsub[subno].ToString();
                                                            if (lab == "1" || lab.Trim().ToLower() == "true")
                                                            {
                                                                string strgetatt = "select distinct r.Roll_No from registration r,attendance a,subjectchooser s where degree_code='" + degreecode + "' and semester='" + sem + "' and batch_year='" + batchyear + "' and cc=0 and delflag=0 and exam_flag<>'debar' and month_year=" + strdate + "";
                                                                strgetatt = strgetatt + " and r.roll_no=a.roll_no and  r.roll_no=s.roll_no and r.current_semester=s.semester and subject_no='" + sp2[0].ToString() + "' " + strsec + " and(" + Att_dcolumn + " is not null and " + Att_dcolumn + "<>'0' and " + Att_dcolumn + "<>'') and batch in(select stu_batch from laballoc ";
                                                                strgetatt = strgetatt + " where subject_no='" + sp2[0].ToString() + "' and Timetablename='" + timetable + "' and batch_year='" + batchyear + "'  and hour_value='" + hr + "'  and    degree_code='" + degreecode + "' and day_value='" + strday + "' and semester='" + sem + "' " + strsec + ") and adm_date<='" + dt + "' ";
                                                                dsstuatt = d2.select_method_wo_parameter(strgetatt, "Text");
                                                                if (dsstuatt.Tables[0].Rows.Count > 0)
                                                                {
                                                                    strgetatt = "select distinct r.Roll_No,r.reg_no,r.stud_name from registration r,attendance a,subjectchooser s where degree_code='" + degreecode + "' and semester='" + sem + "' and batch_year='" + batchyear + "' and cc=0 and delflag=0 and exam_flag<>'debar' and month_year=" + strdate + "";
                                                                    strgetatt = strgetatt + " and r.roll_no=a.roll_no and  r.roll_no=s.roll_no and r.current_semester=s.semester and subject_no='" + sp2[0].ToString() + "' " + strsec + "  and " + Att_dcolumn + " in(" + absenteescode + ") and batch in(select stu_batch from laballoc ";
                                                                    strgetatt = strgetatt + " where subject_no='" + sp2[0].ToString() + "' and Timetablename='" + timetable + "' and batch_year='" + batchyear + "'  and hour_value='" + hr + "'  and    degree_code='" + degreecode + "' and day_value='" + strday + "' and semester='" + sem + "' " + strsec + ") and adm_date<='" + dt + "' ";
                                                                    dsstuatt = d2.select_method_wo_parameter(strgetatt, "Text");
                                                                    if (dsstuatt.Tables[0].Rows.Count > 0)
                                                                    {
                                                                        for (int s = 0; s < dsstuatt.Tables[0].Rows.Count; s++)
                                                                        {
                                                                            if (hatstudet.ContainsKey(subno))
                                                                            {
                                                                                if (s == 0)
                                                                                {
                                                                                    hatstudet[subno] = hatstudet[subno] + " (Lab Period: " + hr + ") " + dsstuatt.Tables[0].Rows[s]["reg_no"].ToString();
                                                                                }
                                                                                else
                                                                                {
                                                                                    hatstudet[subno] = hatstudet[subno] + ", " + dsstuatt.Tables[0].Rows[s]["reg_no"].ToString();
                                                                                }
                                                                            }
                                                                            else
                                                                            {
                                                                                hatstudet.Add(subno, "(Lab Period: " + hr + ") " + dsstuatt.Tables[0].Rows[s]["reg_no"].ToString());
                                                                            }
                                                                        }
                                                                    }
                                                                    else
                                                                    {
                                                                        if (hatstudet.ContainsKey(subno))
                                                                        {
                                                                            hatstudet[subno] = hatstudet[subno] + " (Lab Period: " + hr + ") Nil";
                                                                        }
                                                                        else
                                                                        {
                                                                            hatstudet.Add(subno, "(Lab Period: " + hr + ") Nil");
                                                                        }
                                                                    }
                                                                }
                                                                else
                                                                {
                                                                    if (hatstudet.ContainsKey(subno))
                                                                    {
                                                                        hatstudet[subno] = hatstudet[subno] + " (Lab Period: " + hr + "  Please Allot the Subject and Batch For Student)";
                                                                    }
                                                                    else
                                                                    {
                                                                        hatstudet.Add(subno, "(Lab Period: " + hr + " Please Allot the Subject and Batch For Student)");
                                                                    }
                                                                }
                                                            }
                                                            else
                                                            {
                                                                hatvalue.Clear();
                                                                hatvalue.Add("batch_year", batchyear);
                                                                hatvalue.Add("degree_code", degreecode);
                                                                hatvalue.Add("sem", sem);
                                                                hatvalue.Add("sections", secval);
                                                                hatvalue.Add("month_year", strdate);
                                                                hatvalue.Add("date", dt);
                                                                hatvalue.Add("subject_no", sp2[0]);
                                                                dssubstucount.Tables[0].DefaultView.RowFilter = "subject_no='" + sp2[0] + "' and adm_date<='" + dt.ToString("MM/dd/yyyy").ToString() + "' ";//added admissiondare
                                                                DataView dvsubstucount = dssubstucount.Tables[0].DefaultView;
                                                                if (dvsubstucount.Count > 0)
                                                                {
                                                                    string strgetatt = "select distinct registration.roll_no," + Att_dcolumn + " from registration,attendance,subjectchooser s where registration.roll_no=attendance.roll_no and  registration.roll_no=s.roll_no ";
                                                                    strgetatt = strgetatt + " and registration.current_semester=s.semester and subject_no='" + sp2[0] + "' and degree_code='" + degreecode + "' and s.semester='" + sem + "' and batch_year='" + batchyear + "' " + strsec + " ";
                                                                    strgetatt = strgetatt + " and cc=0 and delflag=0 and exam_flag<>'debar' and month_year='" + strdate + "' and (" + Att_dcolumn + " is not null and " + Att_dcolumn + "<>'0' and " + Att_dcolumn + "<>'') and adm_date<='" + dt + "' ";
                                                                    dsstuatt = d2.select_method_wo_parameter(strgetatt, "Text");
                                                                    if (dsstuatt.Tables[0].Rows.Count > 0)
                                                                    {
                                                                        strgetatt = "select distinct registration.roll_no,registration.reg_no,registration.stud_name," + Att_dcolumn + " from registration,attendance,subjectchooser s where registration.roll_no=attendance.roll_no and  registration.roll_no=s.roll_no ";
                                                                        strgetatt = strgetatt + " and registration.current_semester=s.semester and subject_no='" + sp2[0] + "' and degree_code='" + degreecode + "' and s.semester='" + sem + "' and batch_year='" + batchyear + "' " + strsec + " ";
                                                                        strgetatt = strgetatt + " and cc=0 and delflag=0 and exam_flag<>'debar' and month_year='" + strdate + "' and " + Att_dcolumn + " in(" + absenteescode + ") and adm_date<='" + dt + "' ";
                                                                        dsstuatt = d2.select_method_wo_parameter(strgetatt, "Text");
                                                                        if (dsstuatt.Tables[0].Rows.Count > 0)
                                                                        {
                                                                            for (int s = 0; s < dsstuatt.Tables[0].Rows.Count; s++)
                                                                            {
                                                                                if (hatstudet.ContainsKey(subno))
                                                                                {
                                                                                    if (s == 0)
                                                                                    {
                                                                                        hatstudet[subno] = hatstudet[subno] + " (Period: " + hr + ") " + dsstuatt.Tables[0].Rows[s]["reg_no"].ToString();
                                                                                    }
                                                                                    else
                                                                                    {
                                                                                        hatstudet[subno] = hatstudet[subno] + ", " + dsstuatt.Tables[0].Rows[s]["reg_no"].ToString();
                                                                                    }
                                                                                }
                                                                                else
                                                                                {
                                                                                    hatstudet.Add(subno, "(Period: " + hr + ") " + dsstuatt.Tables[0].Rows[s]["reg_no"].ToString());
                                                                                }
                                                                            }
                                                                        }
                                                                        else
                                                                        {
                                                                            if (hatstudet.ContainsKey(subno))
                                                                            {
                                                                                hatstudet[subno] = hatstudet[subno] + " (Period: " + hr + ") Nil";
                                                                            }
                                                                            else
                                                                            {
                                                                                hatstudet.Add(subno, "(Period: " + hr + ") Nil");
                                                                            }
                                                                        }
                                                                    }
                                                                    else
                                                                    {
                                                                        if (hatstudet.ContainsKey(subno))
                                                                        {
                                                                            hatstudet[subno] = hatstudet[subno] + " (Period: " + hr + "  Attendance Not Entered)";
                                                                        }
                                                                        else
                                                                        {
                                                                            hatstudet.Add(subno, "(Period: " + hr + " Attendance Not Entered)");
                                                                        }
                                                                    }
                                                                }
                                                                else
                                                                {
                                                                    if (hatstudet.ContainsKey(subno))
                                                                    {
                                                                        hatstudet[subno] = hatstudet[subno] + " (Period: " + hr + "  Please Allot the Subject For Student)";
                                                                    }
                                                                    else
                                                                    {
                                                                        hatstudet.Add(subno, "(Period: " + hr + " Please Allot the Subject For Student)");
                                                                    }
                                                                }
                                                            }
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                    }

                                    for (int c = 2; c < dtl.Columns.Count - 1; c++)
                                    {
                                        string subno = arrsubno[c];

                                        
                                        if (hatstudet.ContainsKey(subno))
                                        {
                                            
                                            //added by rajasekar 28/08/2018
                                            dtrow[col] = hatstudet[subno].ToString();
                                            col++;
                                            //==========================//
                                        }
                                        else
                                        {
                                            
                                            //added by rajasekar 28/08/2018
                                            dtrow[col] = "---";
                                            col++;
                                            //============================//
                                           
                                        }
                                    }
                                    dtl.Rows.Add(dtrow);//added by rajasekar 28/08/2018
                                    hatstudet.Clear();
                                }
                                else
                                {
                                    

                                    //added by rajasekar 28/08/2018
                                    dtrow[col] = "Holiday Reason: " + holidayreson;
                                    col++;
                                  
                                    dtl.Rows.Add(dtrow);

                                    //====================//
                                }
                            }
                        }
                    }
                }
                else
                {
                    errmsg.Text = "Please Enter valid semester date";
                    errmsg.Visible = true;
                }
            }
            else
            {
                errmsg.Text = "Please Update Semester Information";
                errmsg.Visible = true;
            }

            grdover.DataSource = dtl;
            grdover.DataBind();
            grdover.HeaderRow.Visible = false;
            for (int i = 0; i < grdover.Rows.Count; i++)
            {
                
                
                for (int j = 0; j < grdover.HeaderRow.Cells.Count; j++)
                {
                            if (i == 0)
                            {
                                grdover.Rows[i].Cells[j].HorizontalAlign = HorizontalAlign.Center;
                                grdover.Rows[i].Cells[j].BackColor = ColorTranslator.FromHtml("#0CA6CA");
                                grdover.Rows[i].Cells[j].BorderColor = Color.Black;
                                grdover.Rows[i].Cells[j].Font.Bold = true;
                            }
                            else
                            {
                                if (grdover.Rows[i].Cells[j].Text == "---" || grdover.HeaderRow.Cells[j].Text == "S.No" || grdover.Rows[i].Cells[j].Text == "&nbsp;")
                                {
                                    grdover.Rows[i].Cells[j].HorizontalAlign = HorizontalAlign.Center;
                                    if (grdover.Rows[i].Cells[j].Text == "&nbsp;" && j == 3)
                                    {
                                        grdover.Rows[i].Cells[j-1].HorizontalAlign = HorizontalAlign.Center;
                                        grdover.Rows[i].Cells[j - 1].ForeColor = System.Drawing.Color.Red;
                                        grdover.Rows[i].Cells[j - 1].BorderColor = System.Drawing.Color.Black;
                                        grdover.Rows[i].Cells[j - 1].ColumnSpan = grdover.Rows[i].Cells.Count - 2;//rrrr
                                        for (int a = 3; a < grdover.Rows[i].Cells.Count; a++)
                                            grdover.Rows[i].Cells[a].Visible = false;


                                    }
                                }

                                else
                                    grdover.Rows[i].Cells[j].HorizontalAlign = HorizontalAlign.Left;

                            }
                }

            }

            if (grdover.Rows.Count > 0)
            {
                

                grdover.Visible = true;
                lblrptname.Visible = true;
                txtexcelname.Visible = true;
                btnxl.Visible = true;
                btnmasterprint.Visible = true;
                btnPrint.Visible = true;
                Printcontrol.Visible = false;
            }
            else
            {
                clear();
                errmsg.Text = "No Records Found";
                errmsg.Visible = true;
            }
            
        }
        catch (Exception ex)
        {
            errmsg.Text = ex.ToString();
            errmsg.Visible = true;
        }
    }
    protected void btnxl_Click(object sender, EventArgs e)
    {
        try
        {
            string reportname = txtexcelname.Text.ToString().Trim();
            if (reportname != "")
            {

                d2.printexcelreportgrid(grdover, reportname);
                errmsg.Visible = false;
            }
            else
            {
                errmsg.Text = "Please Enter Your Report Name";
                errmsg.Visible = true;
            }
        }
        catch (Exception ex)
        {
            errmsg.Visible = true;
            errmsg.Text = ex.ToString();
        }
    }
    protected void btnmasterprint_Click(object sender, EventArgs e)
    {
        string degreedetails = "Consolidate Absentees Report@Date : " + txtfdate.Text + " to " + txttdate.Text + "@Branch : " + ddldegree.SelectedItem.ToString() + " - " + ddlbranch.SelectedItem.ToString() + "@Sem : " + ddlsem.SelectedItem.ToString();
        string strsec = ddlsec.SelectedValue.ToString();
        if (strsec != "" && strsec != "-1" && strsec != "All")
        {
            degreedetails = degreedetails + "@Sec : " + strsec;
        }
        string pagename = "SubjectWiseAbsenteesReport.aspx";
        

        string ss = null;
        Printcontrol.loadspreaddetails(grdover, pagename, degreedetails, 0, ss);
        ////Printcontrol.loadspreaddetails(attnd_report, pagename, degreedetails);
        Printcontrol.Visible = true;
    }
      

    
    
    

     public void btnPrint11()
    {
        DAccess2 d2 = new DAccess2();
        string college_code = Convert.ToString(Session["collegecode"].ToString());
        string colQ = "select * from collinfo where college_code='" + college_code + "'";
        DataSet dsCol = new DataSet();
        dsCol = d2.select_method_wo_parameter(colQ, "Text");
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
        spReportName.InnerHtml = "Consolidate Absentees Report";
        // spSection.InnerHtml ="Satff: "+ Convert.ToString(ddlSearchOption.SelectedItem.Text);


    }
    public override void VerifyRenderingInServerForm(Control control)
    { }
}
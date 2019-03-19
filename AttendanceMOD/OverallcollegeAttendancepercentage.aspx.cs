using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;
using Gios.Pdf;
using InsproDataAccess;
using Farpoint = FarPoint.Web.Spread;
using wc = System.Web.UI.WebControls;

public partial class OverallcollegeAttendancepercentage : System.Web.UI.Page
{
    DataTable dtCommon = new DataTable();
    ReuasableMethods rs = new ReuasableMethods();
    DAccess2 da = new DAccess2();
    DataSet ds=new DataSet();
    InsproStoreAccess storeAcc = new InsproStoreAccess();
    Dictionary<string, string> dicQueryParameter = new Dictionary<string, string>();
    Hashtable has=new Hashtable();
    #region veriable
    string college_code = string.Empty;
    string collegeCode = string.Empty;
    string userCollegeCode = string.Empty;
    string userCode = string.Empty;
    string singleUser = string.Empty;
    string groupUserCode = string.Empty;
    string frdate, todate, new_header_name = string.Empty;
    string frmdate, toddate = string.Empty;
    int cal_from_date, cal_from_date_tmp, cal_from_attdate_tmp;
    int cal_to_date, start_column = 0, cal_to_date_tmp, cal_to_attdate_tmp;
    int demfcal, demtcal;
    DateTime per_from_attdate;
    DateTime per_to_attdate;
    string monthcal;
    string date;
    FarPoint.Web.Spread.StyleInfo MyStyle = new FarPoint.Web.Spread.StyleInfo();
#endregion

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {

            if (Session["collegecode"] == null)
            {
                Response.Redirect("~/Default.aspx");
            }
            else
            {
                userCollegeCode = Convert.ToString(Session["collegecode"]).Trim();
                userCode = (Session["usercode"] != null) ? Convert.ToString(Session["usercode"]).Trim() : "";
                singleUser = (Session["single_user"] != null) ? Convert.ToString(Session["single_user"]).Trim() : "";
                groupUserCode = (Session["group_code"] != null) ? Convert.ToString(Session["group_code"]).Trim() : "";
            }
            if (!IsPostBack)
            {
                //Fpspread.Sheets[0].Visible = false;
                Bindcollege();
                lblnorec.Visible = false;
                pnlContent1.Visible = false;
                btnxl.Visible = false;
                lblrptname.Visible = false;
                txtexcelname.Visible = false;
                Printcontrol.Visible = false;
                btnprintmaster.Visible = false;
                txtfromDate.Text = DateTime.Today.ToString("d-MM-yyyy");
                txttoDate.Text = DateTime.Today.ToString("d-MM-yyyy");

               // divfpspread.Visible = false;
               
            }

        }
        catch (Exception ex)
        {
            da.sendErrorMail(ex, collegeCode, "OverallcollegeAttendancepercentage"); 
        }
    
    }
    public void Bindcollege()
    {
        try
        {
            cblCollege.Items.Clear();
            chkCollege.Checked = false;
            dtCommon.Clear();
            DataSet dsprint = new DataSet();
            dsprint.Clear();
            string qryUserCodeOrGroupCode = string.Empty;
            string group_user = ((Session["group_code"] != null) ? Convert.ToString(Session["group_code"]) : string.Empty);
            if (group_user.Contains(';'))
            {
                string[] group_semi = group_user.Split(';');
                group_user = Convert.ToString(group_semi[0]);
            }
            if ((Convert.ToString(group_user).Trim() != "") && Session["group_code"] != null && Session["single_user"] != null && Convert.ToString(Session["single_user"]).Trim() != "1" && Convert.ToString(Session["single_user"]).Trim().ToLower() != "true")
            {
                qryUserCodeOrGroupCode = " and group_code='" + group_user + "'";
            }
            else if (Session["usercode"] != null && !string.IsNullOrEmpty(Convert.ToString(Session["usercode"]).Trim()))
            {
                qryUserCodeOrGroupCode = " and user_code='" + Convert.ToString(Session["usercode"]).Trim() + "'";
            }
            if (!string.IsNullOrEmpty(qryUserCodeOrGroupCode))
            {
                dicQueryParameter.Clear();
                dicQueryParameter.Add("column_field", Convert.ToString(qryUserCodeOrGroupCode));
                dtCommon = storeAcc.selectDataTable("bind_college", dicQueryParameter);
            }
            if (dtCommon.Rows.Count > 0)
            {
                cblCollege.DataSource = dtCommon;
                cblCollege.DataTextField = "collname";
                cblCollege.DataValueField = "college_code";
                cblCollege.DataBind();

            }
        }
        catch (Exception ex)
        {
            da.sendErrorMail(ex, collegeCode, "OverallcollegeAttendancepercentage"); 
        }
    }
    protected void chkCollege_CheckedChanged(object sender, EventArgs e)
    {
        try
        {

            CallCheckboxChange(chkCollege, cblCollege, txtCollege, lblCollege.Text, "--Select--");

        }
        catch (Exception ex)
        {
            da.sendErrorMail(ex, collegeCode, "OverallcollegeAttendancepercentage"); 
        }
    }
    protected void cblCollege_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {

            CallCheckboxListChange(chkCollege, cblCollege, txtCollege, lblCollege.Text, "--Select--");


        }
        catch (Exception ex)
        {
            da.sendErrorMail(ex, collegeCode, "OverallcollegeAttendancepercentage"); 
        }
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
        catch (Exception ex)
        {
            da.sendErrorMail(ex, collegeCode, "OverallcollegeAttendancepercentage"); 
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
        catch (Exception ex)
        {
            da.sendErrorMail(ex, collegeCode, "OverallcollegeAttendancepercentage"); 
        }
    }
    protected void txtfromDate_TextChanged(object sender, EventArgs e)
    {
        try
        {
            //Fpspread.Visible = false;
            lblnorec.Visible = false;
            
        }
        catch (Exception ex)
        {
            da.sendErrorMail(ex, collegeCode, "OverallcollegeAttendancepercentage"); 
        }
    }
    protected void txttoDate_TextChanged(object sender, EventArgs e)
    {
        try
        {
           // Fpspread.Visible = false;
           
        }
        catch (Exception ex)
        {
            da.sendErrorMail(ex, collegeCode, "OverallcollegeAttendancepercentage"); 
        }
    }

    protected void btngo_Click(object sender, EventArgs e)
    {
        try
        {
            //divfpspread.Visible = true;
            btnxl.Visible = true;
            lblrptname.Visible = true;
            txtexcelname.Visible = true;
            Printcontrol.Visible = false;
            btnprintmaster.Visible = true;
            pnlContent1.Visible = true;
            Fpspread.Sheets[0].Visible = true;
            string valCollege = string.Empty;
            lblnorec.Visible = false;
            string sumpart = string.Empty;
            int value = 0;
            int sno = 0;
            valCollege = rs.GetSelectedItemsValueAsString(cblCollege);
            if (valCollege != "")
            {
            if (txtfromDate.Text != "" && txttoDate.Text != "")
            {
                frdate = txtfromDate.Text;
                todate = txttoDate.Text;
                string dt = frdate;
                string[] dsplit = dt.Split(new Char[] { '-' });
                frdate = dsplit[2].ToString() + "-" + dsplit[1].ToString() + "-" + dsplit[0].ToString();
                demfcal = int.Parse(dsplit[2].ToString());
                demfcal = demfcal * 12;
                cal_from_date = demfcal + int.Parse(dsplit[1].ToString());
                cal_from_attdate_tmp = demfcal + int.Parse(dsplit[1].ToString());

                monthcal = cal_from_date.ToString();
                dt = todate;
                dsplit = dt.Split(new Char[] { '-' });
                todate = dsplit[2].ToString() + "-" + dsplit[1].ToString() + "-" + dsplit[0].ToString();
                demtcal = int.Parse(dsplit[2].ToString());
                demtcal = demtcal * 12;
                cal_to_date = demtcal + int.Parse(dsplit[1].ToString());
                cal_to_attdate_tmp = demtcal + int.Parse(dsplit[1].ToString());
                per_from_attdate = Convert.ToDateTime(frdate);
                per_to_attdate = Convert.ToDateTime(todate);
                if (per_from_attdate <= per_to_attdate)
                {
                    #region colbinding
                    Fpspread.Sheets[0].AutoPostBack = true;
                    Fpspread.Sheets[0].RowHeader.Visible = false;
                    Fpspread.Sheets[0].ColumnHeader.Visible = true;
                    MyStyle.Font.Size = FontUnit.Medium;
                    MyStyle.Font.Name = "Book Antiqua";
                    MyStyle.Font.Bold = true;
                    MyStyle.HorizontalAlign = HorizontalAlign.Center;
                    MyStyle.ForeColor = Color.White;
                    MyStyle.BackColor = ColorTranslator.FromHtml("#0CA6CA");
                    Fpspread.Sheets[0].ColumnHeader.DefaultStyle = MyStyle;
                    Fpspread.CommandBar.Visible = false;
                    Fpspread.Sheets[0].ColumnCount = 5;
                    Fpspread.Sheets[0].RowCount = 0;
                    Fpspread.BorderWidth = 2;
                    Fpspread.Sheets[0].ColumnHeader.Cells[0, 0].Text = "S.No";
                    Fpspread.Sheets[0].ColumnHeader.Cells[0, 0].Font.Bold = true;
                    Fpspread.Sheets[0].ColumnHeader.Cells[0, 0].Font.Name = "Book Antiqua";
                    Fpspread.Sheets[0].ColumnHeader.Cells[0, 0].Font.Size = FontUnit.Medium;
                    Fpspread.Sheets[0].ColumnHeader.Cells[0, 0].HorizontalAlign = HorizontalAlign.Center;
                    Fpspread.Sheets[0].ColumnHeader.Cells[0, 1].Text = "College Name";
                    Fpspread.Sheets[0].ColumnHeader.Cells[0, 1].Font.Bold = true;
                    Fpspread.Sheets[0].ColumnHeader.Cells[0, 1].Font.Name = "Book Antiqua";
                    Fpspread.Sheets[0].ColumnHeader.Cells[0, 1].Font.Size = FontUnit.Medium;
                    Fpspread.Sheets[0].ColumnHeader.Cells[0, 1].HorizontalAlign = HorizontalAlign.Center;
                    Fpspread.Sheets[0].ColumnHeader.Cells[0, 2].Text = "Total Strenth";
                    Fpspread.Sheets[0].ColumnHeader.Cells[0, 2].Font.Bold = true;
                    Fpspread.Sheets[0].ColumnHeader.Cells[0, 2].Font.Name = "Book Antiqua";
                    Fpspread.Sheets[0].ColumnHeader.Cells[0, 2].Font.Size = FontUnit.Medium;
                    Fpspread.Sheets[0].ColumnHeader.Cells[0, 2].HorizontalAlign = HorizontalAlign.Center;
                    Fpspread.Sheets[0].ColumnHeader.Cells[0, 3].Text = "Present %";
                    Fpspread.Sheets[0].ColumnHeader.Cells[0, 3].Font.Bold = true;
                    Fpspread.Sheets[0].ColumnHeader.Cells[0, 3].Font.Name = "Book Antiqua";
                    Fpspread.Sheets[0].ColumnHeader.Cells[0, 3].Font.Size = FontUnit.Medium;
                    Fpspread.Sheets[0].ColumnHeader.Cells[0, 3].HorizontalAlign = HorizontalAlign.Center;
                    Fpspread.Sheets[0].ColumnHeader.Cells[0, 4].Text = "Absent %";
                    Fpspread.Columns[4].Width = 25;
                    Fpspread.Sheets[0].ColumnHeader.Cells[0, 4].Font.Bold = true;
                    Fpspread.Sheets[0].ColumnHeader.Cells[0, 4].Font.Name = "Book Antiqua";
                    Fpspread.Sheets[0].ColumnHeader.Cells[0, 4].Font.Size = FontUnit.Medium;
                    Fpspread.Sheets[0].ColumnHeader.Cells[0, 4].HorizontalAlign = HorizontalAlign.Center;
                    #endregion
                     //  // string sql = "select * from Registration r,attendance a where r.Roll_No=a.roll_no and r.App_No=a.Att_App_no and a.Att_CollegeCode= r.college_code and r.CC='0' and r.DelFlag='0' and r.Exam_Flag <>'debar' and a.month_year between '" + cal_from_attdate_tmp + "' and '" + cal_to_attdate_tmp + "' and r.college_code in('" + valCollege + "')";
                    string sql = "select * from Registration r,attendance a where r.Roll_No=a.roll_no and r.App_No=a.Att_App_no and a.Att_CollegeCode= r.college_code and r.CC='0' and r.DelFlag='0' and r.Exam_Flag <>'debar' and a.month_year between '" + cal_from_attdate_tmp + "' and '" + cal_to_attdate_tmp + "' and r.college_code in('" + valCollege + "') order by r.degree_code,r.Roll_No";

                    ds = da.select_method_wo_parameter(sql, "text");
                    int fprow = 0;
                        for (int clg = 0; clg < cblCollege.Items.Count; clg++)
                        {
                           
                            if (cblCollege.Items[clg].Selected == true)
                            {
                                double present = 0;
                                double absent = 0;
                                int hour=0;
                                double tolhour_perclg = 0;
                                int cunstu=0;
                                //int tolhour_perclg = 0;
                                Fpspread.Sheets[0].RowCount++;
                                sno++;
                               string stucon = da.GetFunction("select count(distinct r.app_no) from Registration r where r.CC='0' and r.DelFlag='0' and r.Exam_Flag <>'debar' and r.college_code='" + cblCollege.Items[clg].Value + "'");


                                //string stucon = da.GetFunction("select count(distinct r.roll_no) from Registration r,attendance a where r.Roll_No=a.roll_no and r.App_No=a.Att_App_no and a.Att_CollegeCode= r.college_code and r.CC='0' and r.DelFlag='0' and r.Exam_Flag <>'debar' and a.month_year between '" + cal_from_attdate_tmp + "' and '" + cal_to_attdate_tmp + "' and r.college_code in('" + cblCollege.Items[clg].Value + "')");
                                //ds = da.select_method("bind_branch", has, "sp");
                                //string gendegree = "select distinct r.degree_code from Registration r,attendance a where r.Roll_No=a.roll_no and r.App_No=a.Att_App_no and a.Att_CollegeCode= r.college_code  and r.CC='0' and r.DelFlag='0' and r.Exam_Flag <>'debar' and a.month_year between '" + cal_from_attdate_tmp + "'  and '" + cal_to_attdate_tmp + "' and  r.college_code='" + cblCollege.Items[clg].Value +"' order by r.degree_code";
                                string valcolle_code = Convert.ToString(cblCollege.Items[clg].Value).Trim();
                                int valcoll_code;
                                int.TryParse(valcolle_code, out valcoll_code);
                                has.Clear();
                                has.Add("valcoll_code", valcoll_code);
                                has.Add("cal_from_attdate_tmp", cal_from_attdate_tmp);
                                has.Add("cal_to_attdate_tmp", cal_to_attdate_tmp);
                                DataSet degreegener = da.select_method("attoverallclg_coll",has, "sp");
                                //DataSet degreegener = da.select_method_wo_parameter(gendegree, "text");
                                for (int degrow = 0; degrow < degreegener.Tables[0].Rows.Count; degrow++)
                                {
                                    int tolhour_perdeg = 0;
                                    string hrs = da.GetFunction("select No_of_hrs_per_day from PeriodAttndSchedule where degree_code='" + degreegener.Tables[0].Rows[degrow]["degree_code"] + "'");
                                    int.TryParse(hrs, out hour);
                               //string stud =da.GetFunction(" select count(distinct r.roll_no) from Registration r,attendance a where r.Roll_No=a.roll_no and r.App_No=a.Att_App_no and a.Att_CollegeCode= r.college_code  and r.CC='0' and r.DelFlag='0' and r.Exam_Flag <>'debar' and a.month_year between '" + cal_from_attdate_tmp + "'  and '" + cal_to_attdate_tmp + "'and r.degree_code='" + degreegener.Tables[0].Rows[degrow]["degree_code"] + "' and  r.college_code='" + cblCollege.Items[clg].Value + "'");
                                   
                                    ds.Tables[0].DefaultView.RowFilter = "college_code='" + cblCollege.Items[clg].Value + "' and degree_code='" + degreegener.Tables[0].Rows[degrow]["degree_code"] + "'";
                                    DataView clgfilter = ds.Tables[0].DefaultView;
                                    for (DateTime dtt = per_from_attdate; dtt <= per_to_attdate; dtt = dtt.AddDays(1))
                                    {
                                        //for (int degrow = 0; degrow < clgfilter.Count; degrow++)
                                        // {

                                        //ds.Tables[0].DefaultView.RowFilter = "month_year='" + cal_from_date + "'";

                                        for (int i = 1; i <= hour; i++)
                                        {
                                            for (int sturow = 0; sturow < clgfilter.Count; sturow++)
                                            {

                                                date = "d" + dtt.Day.ToString("") + "d" + i.ToString();
                                                string maxval = Convert.ToString(clgfilter[sturow][date]).Trim();
                                                if (maxval != "" && maxval != "0" && maxval != "NULL" && maxval != "Null" && maxval != "null" && maxval != "H" && maxval != "NJ" && maxval != "Null")
                                                {
                                                    int.TryParse(maxval, out value);

                                                    string valqu = da.GetFunction("select CalcFlag  from AttMasterSetting where collegecode=13 and LeaveCode='" + value + "'  group by CalcFlag");
                                                    int.TryParse(valqu, out value);

                                                    if (value == 0)
                                                    {
                                                        present = present + 1;
                                                    }
                                                    else
                                                    {
                                                        absent = absent + 1;
                                                    }

                                                }
                                                else
                                                {

                                                }
                                            }
                                        }
                                        tolhour_perdeg += hour;
                                    }
                                    //int.TryParse(stud, out cunstu);
                                    //tolhour_perdeg *= cunstu;
                                    //tolhour_perclg += tolhour_perdeg;
                                    tolhour_perclg = present + absent;
                                }
                                
                                if (present != 0)
                                {
                                    present = (present / tolhour_perclg);
                                    present = Math.Round(present, 3);
                                    present = present * 100;
                                    sumpart = String.Format("{0:0.00}", present);

                                }
                                if (absent != 0)
                                {
                                    absent = (absent / tolhour_perclg);
                                    absent = Math.Round(absent, 3);
                                    absent = absent * 100;
                                    sumpart = String.Format("{0:0.00}", absent);

                                }
                                Fpspread.Sheets[0].Cells[fprow, 0].Text = sno.ToString();
                                Fpspread.Columns[0].Width = 10;
                                Fpspread.Sheets[0].Cells[fprow, 0].Font.Bold = true;
                                Fpspread.Sheets[0].Cells[fprow, 0].Font.Name = "Book Antiqua";
                                Fpspread.Sheets[0].Cells[fprow, 0].Font.Size = FontUnit.Medium;
                                Fpspread.Sheets[0].Cells[fprow, 0].HorizontalAlign = HorizontalAlign.Center;
                                Fpspread.Sheets[0].Cells[fprow, 1].Text = cblCollege.Items[clg].ToString();
                                Fpspread.Columns[1].Width = 100;
                                Fpspread.Sheets[0].Cells[fprow, 1].Font.Bold = true;
                                Fpspread.Sheets[0].Cells[fprow, 1].Font.Name = "Book Antiqua";
                                Fpspread.Sheets[0].Cells[fprow, 1].Font.Size = FontUnit.Medium;
                                Fpspread.Sheets[0].Cells[fprow, 1].HorizontalAlign = HorizontalAlign.Left;
                                Fpspread.Sheets[0].Cells[fprow, 2].Text = stucon.ToString(); 
                                Fpspread.Columns[2].Width = 25;
                                Fpspread.Sheets[0].Cells[fprow, 2].Font.Bold = true;
                                Fpspread.Sheets[0].Cells[fprow, 2].Font.Name = "Book Antiqua";
                                Fpspread.Sheets[0].Cells[fprow, 2].Font.Size = FontUnit.Medium;
                                Fpspread.Sheets[0].Cells[fprow, 2].HorizontalAlign = HorizontalAlign.Center;
                                Fpspread.Sheets[0].Cells[fprow, 3].Text = present.ToString();
                                Fpspread.Columns[3].Width = 25;
                                Fpspread.Sheets[0].Cells[fprow, 3].Font.Bold = true;
                                Fpspread.Sheets[0].Cells[fprow, 3].Font.Name = "Book Antiqua";
                                Fpspread.Sheets[0].Cells[fprow, 3].Font.Size = FontUnit.Medium;
                                Fpspread.Sheets[0].Cells[fprow, 3].HorizontalAlign = HorizontalAlign.Center;
                                Fpspread.Sheets[0].Cells[fprow, 4].Text = absent.ToString();
                                Fpspread.Sheets[0].Cells[fprow, 4].Font.Bold = true;
                                Fpspread.Sheets[0].Cells[fprow, 4].Font.Name = "Book Antiqua";
                                Fpspread.Sheets[0].Cells[fprow, 4].Font.Size = FontUnit.Medium;
                                Fpspread.Sheets[0].Cells[fprow, 4].HorizontalAlign = HorizontalAlign.Center;
                                Fpspread.Sheets[0].PageSize = Fpspread.Sheets[0].RowCount;
                                Fpspread.Width = 900;
                                Fpspread.Height = 700;
                                Fpspread.SaveChanges();
                                fprow++;


                            }

                        }
                    }
                    else
                    {
                        Fpspread.Visible = false;
                        btnxl.Visible = false;
                        lblrptname.Visible = false;
                        txtexcelname.Visible = false;
                        Printcontrol.Visible = false;
                        btnprintmaster.Visible = false;
                        lblnorec.Visible = true;
                        lblnorec.Text = "Please select the To date  is greater then From date";
                    }
                }
                else
                {
                    Fpspread.Visible = false;
                    btnxl.Visible = false;
                    lblrptname.Visible = false;
                    txtexcelname.Visible = false;
                    Printcontrol.Visible = false;
                    btnprintmaster.Visible = false;
                    lblnorec.Visible = true;
                    lblnorec.Text = "Please select the From date and To date";
                   
                }
              }
            else
            {
                btnxl.Visible = false;
                lblrptname.Visible = false;
                txtexcelname.Visible = false;
                Printcontrol.Visible = false;
                btnprintmaster.Visible = false;
                Printcontrol.Visible = false;
                Fpspread.Visible = false;
                lblnorec.Visible = true;
                lblnorec.Text = "Please select the College";
            }

        }
        catch (Exception ex)
        {
           da.sendErrorMail(ex, collegeCode, "OverallcollegeAttendancepercentage");
        }
    }
    protected void btnprintmaster_Click(object sender, EventArgs e)
    {
        try
        {
            Session["column_header_row_count"] = Fpspread.Sheets[0].ColumnHeader.RowCount;

            Fpspread.Sheets[0].Columns[0].HorizontalAlign = HorizontalAlign.Center;
            string degreedetails = "Date :" + txtfromDate.Text.ToString() + " To " + txttoDate.Text.ToString();
            string pagename = "OverallcollegeAttendancepercentage.aspx";

            Printcontrol.loadspreaddetails(Fpspread, pagename, degreedetails);
            Printcontrol.Visible = true;
        }
        catch (Exception ex)
        {
            da.sendErrorMail(ex, collegeCode, "OverallcollegeAttendancepercentage");
        }
    }

    protected void btnxl_Click(object sender, EventArgs e)
    {
        try
        {
            lblnorec.Visible = false;
            string reportname = txtexcelname.Text;

            if (reportname.ToString().Trim() != "")
            {
                da.printexcelreport(Fpspread, reportname);
            }
            else
            {
                lblnorec.Text = "Please Enter Your Report Name";
                lblnorec.Visible = true;
            }
        }
        catch (Exception ex)
        {
            da.sendErrorMail(ex, collegeCode, "OverallcollegeAttendancepercentage");
            lblnorec.Text = ex.ToString();
        }
    }
}
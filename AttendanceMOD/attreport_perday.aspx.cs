//==========MANIPRABHA A.

using System;//=====================================modified on 11/1/12,24/1/12,13/2/12, 29/2/12(border,spread width,XL)
//====================21/3/12(convert function into another way),5/4/12(complete print setting),2/7/12(printmaster setting,iso,p_m_s_n,header index)
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Drawing;
using FarPoint.Web.Spread;
using System.Reflection;

public partial class Ovrall_Attreport_perday : System.Web.UI.Page
{

    [Serializable()]
    public class MyImg : ImageCellType
    {

        //public override Control paintcell(string id, System.Web.UI.WebControls.TableCell parent, FarPoint.Web.Spread.Appearance style, FarPoint.Web.Spread.Inset margin, object value, Boolean upperLevel)
        public override Control PaintCell(String id, TableCell parent, FarPoint.Web.Spread.Appearance style, FarPoint.Web.Spread.Inset margin, object val, bool ul)
        {
            System.Web.UI.WebControls.Image img = new System.Web.UI.WebControls.Image();
            img.ImageUrl = this.ImageUrl; //base.ImageUrl;  

            img.Width = Unit.Percentage(80);
            return img;


        }
    }

    FarPoint.Web.Spread.StyleInfo style = new FarPoint.Web.Spread.StyleInfo();
    SqlConnection con = new SqlConnection(ConfigurationManager.AppSettings["DSN"]);
    SqlConnection con1 = new SqlConnection(ConfigurationManager.AppSettings["DSN"]);
    SqlConnection con2 = new SqlConnection(ConfigurationManager.AppSettings["DSN"]);
    SqlConnection con_deg = new SqlConnection(ConfigurationManager.AppSettings["DSN"]);
    SqlConnection con_bind = new SqlConnection(ConfigurationManager.AppSettings["DSN"]);
    SqlConnection con_chkSet = new SqlConnection(ConfigurationManager.AppSettings["DSN"]);
    SqlConnection con_getfunc = new SqlConnection(ConfigurationManager.AppSettings["DSN"]);
    SqlConnection con_Attn = new SqlConnection(ConfigurationManager.AppSettings["DSN"]);
    SqlConnection con_colname = new SqlConnection(ConfigurationManager.AppSettings["DSN"]);

    SqlConnection setcon = new SqlConnection(ConfigurationManager.AppSettings["DSN"]);
    SqlCommand cmd = new SqlCommand();
    string coll_name = "", address1 = "", address2 = "", address3 = "";
    int end_column = 0;
    string phoneno = "", phone = "", faxno = "", fax = "", email = "", email_id = "";
    string website = "", web_add = "", form_name = "", header_alignment = "", isonumber = "";
    string new_header_string = "";
    Hashtable has = new Hashtable();
    int start_column = 0;
    string[] new_header_string_split;

    Hashtable hat = new Hashtable();

    DAccess2 dacces2 = new DAccess2();
    DAccess2 d2 = new DAccess2();

    DataSet ds = new DataSet();
    DataSet ds2 = new DataSet();
    DataSet ds_value = new DataSet();
    DataSet ds_final = new DataSet();

    Dictionary<string, string> dicdegreeattper = new Dictionary<string, string>();

    static string view_header = "", view_footer = "", view_footer_text = "";
    DataSet ds_date = new DataSet();
    Boolean norecflag = false;
    string GetChar = "";
    string todaydate = "";
    Boolean fflag = false;
    int NAbsent = 0;
    int rowhead = 0;
    double over_all_per = 0;
    int tot_strength_temp = 0;
    double ind_tot = 0;

    double tot_tot = 0;

    int tot_strength = 0;
    int MthYear = 0;
    int count = 0;
    int degcount = 0;
    int noofhrs = 0;
    int first_hrs = 0;
    int intDCnt = 0;
    int sec_hrs = 0;

    double temp_val = 0;
    double temp_tot = 0;
    double temp_tot_pres = 0;
    double temp_tot_lea = 0;
    double temp_tot_abs = 0;
    double temp_tot_sus = 0;
    double temp_tot_od = 0;
    double temp_tot_sod = 0;
    double fin_str = 0;
    double fin_pres = 0;
    double fin_abs = 0;
    double fin_sus = 0;
    double fin_od = 0;
    double fin_sod = 0;
    double fin_tot = 0;
    double fin_lev = 0;


    string mng_present = "";
    string mng_proj = "";
    string mng_od = "";
    string mng_sus = "";
    string mng_leav = "";
    string mng_absent = "";
    string eng_present = "";
    string eng_proj = "";
    string eng_od = "";
    string eng_sus = "";
    string eng_leav = "";
    string eng_absent = "";
    string date_concat = "";
    string collegename = "";
    string strDegree = "";
    string deg_code = "";
    string acronym = "";
    string att = "";
    string date = "";
    string sections = "";
    string getsec = "";
    string current_sem = "";
    string roman_val = "";
    string batch_year = "";
    string AttndSch = "";
    string AttnDay = "";
    string strsec = "";
    string Atmnth = "";
    string Atyr = "";
    string Atday = "";
    string inttot = "";
    string AttnDay1 = "";
    string AttnDay2 = "";
    string AttnDay3 = "";
    string AttnDay4 = "";
    string AttnDay5 = "";
    string AttnDay6 = "";
    //
    static Boolean btnflag = false;
    static Boolean forschoolsetting = false;
    //int temp_count = 0;
    int totaldeg = 0;
    int inirowcnt = 0;
    string lperc = "";
    string aperc = "";
    string totaperc = "";
    string prsntperc = "";
    string sperc = "";
    string odperc = "";
    string properc = "";
    double getdata10 = 0;
    double getdata9 = 0;
    double getdata3 = 0;
    double getdata4 = 0;
    double getdata5 = 0;
    double getdata6 = 0;
    double getdata7 = 0;
    double getdata8 = 0;
    double addval = 0;
    double addsusval = 0;
    double addodval = 0;
    double addproval = 0;
    DateTime date_today;
    string temp1 = "", temp2 = "", temp3 = "", temp4 = "", temp5 = "", temp6 = "";

    // DAccess2 dacces2 = new DAccess2();
    int temp_count = 0;
    //DAccess2 dacces2 = new DAccess2();
    int final_print_col_cnt = 0;
    Boolean check_col_count_flag = false;
    DataSet dsprint = new DataSet();
    //  DAccess2 dacces2 = new DAccess2();
    string column_field = "";
    int col_count_all = 0;
    string printvar = "";
    int span_cnt = 0;
    int col_count = 0;
    int child_span_count = 0;
    int footer_count = 0;
    string footer_text = "";
    // int temp_count = 0;
    int split_col_for_footer = 0;
    int footer_balanc_col = 0;
    int tf = 0;
    Boolean payflag = false;
    static string[] string_session_values;
    string group_code = "", columnfield = "";
    Double totmaorp = 0, totevep = 0;
    Double totmaora = 0, totevea = 0;
    Double totmaorl = 0, totevel = 0;
    Double totmaors = 0, toteves = 0;
    Double totmaorod = 0, toteveod = 0;
    Double totmaorsod = 0, totevesod = 0;
    Double totmaorall = 0, toteveall = 0;

    string course_id = string.Empty;
    string usercode = string.Empty;
    string collegecode = string.Empty;
    string singleuser = string.Empty;
    string group_user = string.Empty;

    string strbatch = string.Empty;
    string strbranch = string.Empty;
    static string grouporusercode = "";

    int grandsritotal = 0;
    Double yeardepttostud = 0;
    Double yeardepttostudpresent = 0;
    Double yeardepttostudpresenteve = 0;
    Dictionary<string, int> dicdegree = new Dictionary<string, int>();

    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["collegecode"] == null) //Aruna For Back Button
        {
            Response.Redirect("~/Default.aspx");
        }
        usercode = Session["usercode"].ToString();
        collegecode = Session["collegecode"].ToString();
        singleuser = Session["single_user"].ToString();
        group_user = Session["group_code"].ToString();
        errlbl.Visible = false;
        if (!IsPostBack)
        {
            BindBatch();
            BindDegree(singleuser, group_user, collegecode, usercode);
            if (txtdegree.Enabled == true)
            {
                txtdegree.Enabled = true;
                txtbranch.Enabled = true;
                BindBranch(singleuser, group_user, course_id, collegecode, usercode);
                BindSectransport(strbatch, strbranch);
            }
            else
            {
                txtdegree.Enabled = false;
                txtbranch.Enabled = false;
            }

            txtFromDate.Attributes.Add("readonly", "readonly");
            Session["QueryString"] = "";
            group_code = Session["group_code"].ToString();
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
            dsprint = dacces2.select_method("bind_college", hat, "sp");
            ddlcollege.Items.Clear();
            if (dsprint.Tables[0].Rows.Count > 0)
            {
                ddlcollege.DataSource = dsprint;
                ddlcollege.DataTextField = "collname";
                ddlcollege.DataValueField = "college_code";
                ddlcollege.DataBind();
                ddlcollege_SelectedIndexChanged(sender, e);

                ddlcollege.Enabled = true;
                txtFromDate.Enabled = true;
                btnGo.Enabled = true;
            }
            else
            {
                ddlcollege.Enabled = false;
                txtFromDate.Enabled = false;
                btnGo.Enabled = false;
            }
            Pageload(sender, e);
            btnprintmaster.Visible = false;
            ddlperiod.Visible = false;
            //BindSemster();
            loadperiods();

            string grouporusercodeschool = "";
            if ((Session["group_code"].ToString().Trim() != "") && (Session["group_code"].ToString().Trim() != "0") && (Session["group_code"].ToString().Trim() != "-1"))
            {
                grouporusercodeschool = " group_code=" + Session["group_code"].ToString().Trim() + "";
            }
            else
            {
                grouporusercodeschool = " usercode=" + Session["usercode"].ToString().Trim() + "";
            }
            // Added By Sridharan 12 Mar 2015
            //{
            DataSet schoolds = new DataSet();
            string sqlschool = "select * from Master_Settings where settings='schoolorcollege' and " + grouporusercodeschool + "";
            schoolds.Clear();
            schoolds.Dispose();
            schoolds = d2.select_method_wo_parameter(sqlschool, "Text");
            if (schoolds.Tables[0].Rows.Count > 0)
            {
                string schoolvalue = schoolds.Tables[0].Rows[0]["value"].ToString();
                if (schoolvalue.Trim() == "0")
                {
                    forschoolsetting = true;
                    Label4.Text = "School";
                    //lblbatch.Text = "Year";
                    //lbldegree.Text = "School Type";
                    //lblbranch.Text = "Standard";
                    //lblsem.Text = "Term";
                    //lblDegree.Attributes.Add("style", " width: 95px;");
                    //lblBranch.Attributes.Add("style", " width: 67px;");
                    //ddlBranch.Attributes.Add("style", " width: 241px;");
                }
                else
                {
                    forschoolsetting = false;
                }
            }
            //} Sridharan
        }
    }

    protected void btnGo_Click(object sender, EventArgs e)
    {
        try
        {
            load_btn_click();
            if (attnd_report.Sheets[0].ColumnCount > 0 && attnd_report.Sheets[0].RowCount > 0)//===========on 9/4/12
            {
                final_print_col_cnt = 0;
                for (int temp_col = 0; temp_col < attnd_report.Sheets[0].ColumnCount; temp_col++)
                {
                    if (attnd_report.Sheets[0].Columns[temp_col].Visible == true)
                    {
                        final_print_col_cnt++;
                    }
                }
                //    attnd_report.Width = final_print_col_cnt * 120;

                //4.college information setting

                attnd_report.Width = final_print_col_cnt * 75;
                //setheader_print();
                view_header_setting();
                //4 end.college information setting
            }
            if (attnd_report.Sheets[0].RowCount > 3)
            {
                attnd_report.Visible = true;
            }
            else
            {
                attnd_report.Visible = false;
                errlbl.Visible = true;
                errlbl.Text = "No Records Found";
            }

        }
        catch
        {
        }
    }

    public void load_btn_click()
    {
        if (txtFromDate.Text.Trim() != "")
        {
            string date1 = "", datefrom = "";
            date1 = txtFromDate.Text.ToString();
            string[] split1 = date1.Split(new Char[] { '/' });
            datefrom = split1[1].ToString() + "/" + split1[0].ToString() + "/" + split1[2].ToString();
            DateTime dt1 = Convert.ToDateTime(datefrom.ToString());
            //==check holiday
            //cmd.CommandText = "select top 1 holiday_desc from holidaystudents where holiday_date='" + dt1 + "'";
            //cmd.Connection = con ;
            //con.Close();
            //con.Open();
            //SqlDataReader dr_holday = cmd.ExecuteReader();
            //dr_holday.Read();
            //===================
            //   if (dr_holday.HasRows == false)
            {
                string date = txtFromDate.Text;
                string[] split = date.Split('/');
                if (split.GetUpperBound(0) == 2)//-------date valid
                {
                    if (Convert.ToInt16(split[0].ToString()) <= 31 && Convert.ToInt16(split[1].ToString()) <= 12 && Convert.ToInt16(split[0].ToString()) <= Convert.ToInt16(Session["curr_year"]))
                    {
                        loadvalues();
                        lblFromDate.Visible = false;
                        errlbl.Visible = false;
                    }
                    else
                    {
                        lblFromDate.Visible = true;
                        lblFromDate.Text = "Enter Valid Date";
                    }
                }

                else
                {
                    lblFromDate.Visible = true;
                    lblFromDate.Text = "Enter Valid Date";
                }
            }
            //else
            //{
            //    errlbl.Visible = true;
            //    errlbl.Text = "Selected Day Is Holiday. Reason-" + dr_holday["holiday_desc"].ToString();
            //}
        }
        else
        {
            lblFromDate.Visible = true;
            lblFromDate.Text = "Enter Date";
        }
    }

    public void findhours()
    {
        eng_present = "";
        eng_leav = "";
        eng_absent = "";
        eng_sus = "";
        eng_od = "";
        eng_proj = "";

        mng_present = "";
        mng_leav = "";
        mng_absent = "";
        mng_sus = "";
        mng_od = "";
        mng_proj = "";



        temp1 = "";
        temp2 = "";
        temp3 = "";
        temp4 = "";
        temp5 = "";
        temp6 = "";

        ////==================
        // mng_present = "d" + Atday + "d1=1" + " and d" + Atday + "d2=1" + " and d" + Atday + "d3=1" + " and d" + Atday + "d4=1";
        // mng_leav = "(d" + Atday + "d1=10" + " or d" + Atday + "d2=10" + " or d" + Atday + "d3=10" + " or d" + Atday + "d4=10)";
        // mng_absent = "(d" + Atday + "d1=2" + " or d" + Atday + "d2=2" + " or d" + Atday + "d3=2" + " or d" + Atday + "d4=2)";
        // mng_sus = "(d" + Atday + "d1=9" + " or d" + Atday + "d2=9" + " or d" + Atday + "d3=9" + " or d" + Atday + "d4=9)";
        // mng_od = "(d" + Atday + "d1=3" + " or d" + Atday + "d2=3" + " or d" + Atday + "d3=3" + " or d" + Atday + "d4=3)";
        // mng_proj = "(d" + Atday + "d1=5" + " or d" + Atday + "d2=5" + " or d" + Atday + "d3=5" + " or d" + Atday + "d4=5)";

        // eng_present = "d" + Atday + "d5=1" + " and d" + Atday + "d6=1" + " and d" + Atday + "d7=1";
        // eng_leav = "(d" + Atday + "d5=10" + " or d" + Atday + "d6=10" + " or d" + Atday + "d7=10)";
        // eng_absent = "(d" + Atday + "d5=2" + " or d" + Atday + "d6=2" + " or d" + Atday + "d7=2)";
        // eng_sus = "(d" + Atday + "d5=9" + " or d" + Atday + "d6=9" + " or d" + Atday + "d7=9)";
        // eng_od = "(d" + Atday + "d5=3" + " or d" + Atday + "d6=3" + " or d" + Atday + "d7=3)";
        // eng_proj = "(d" + Atday + "d5=5" + " or d" + Atday + "d6=5" + " or d" + Atday + "d7=5)";
        ////========================

        if (chkPeriod.Checked == false)
        {

            for (int mng_hr = 1; mng_hr <= first_hrs; mng_hr++)
            {
                temp1 = "d" + Atday + "d" + mng_hr + "=1";
                temp2 = "d" + Atday + "d" + mng_hr + "=10";
                temp3 = "d" + Atday + "d" + mng_hr + "=2";
                temp4 = "d" + Atday + "d" + mng_hr + "=9";
                temp5 = "d" + Atday + "d" + mng_hr + "=3";
                temp6 = "d" + Atday + "d" + mng_hr + "=5";
                if (mng_present == "")
                {
                    mng_present = temp1;
                    mng_leav = temp2;
                    mng_absent = temp3;
                    mng_sus = temp4;
                    mng_od = temp5;
                    mng_proj = temp6;
                }
                else
                {
                    mng_present = mng_present + " and " + temp1;
                    mng_leav = mng_leav + " or " + temp2;
                    mng_absent = mng_absent + " or " + temp3;
                    mng_sus = mng_sus + " or " + temp4;
                    mng_od = mng_od + " or " + temp5;
                    mng_proj = mng_proj + " or " + temp6;
                }
            }
            if (mng_present != "")
            {
                mng_present = " ( " + mng_present + " ) ";
            }
            else
            {
                mng_present = "";
            }
            if (mng_leav != "")
            {
                mng_leav = " ( " + mng_leav + " ) ";
            }
            else
            {
                mng_leav = "";
            }
            if (mng_absent != "")
            {
                mng_absent = " ( " + mng_absent + " ) ";
            }
            else
            {
                mng_absent = "";
            }
            if (mng_sus != "")
            {
                mng_sus = " ( " + mng_sus + " ) ";
            }
            else
            {
                mng_sus = "";
            }
            if (mng_od != "")
            {
                mng_od = " ( " + mng_od + " ) ";
            }
            else
            {
                mng_od = "";
            }
            if (mng_proj != "")
            {
                mng_proj = " ( " + mng_proj + " ) ";
            }
            else
            {
                mng_proj = "";
            }
            temp1 = "";
            temp2 = "";
            temp3 = "";
            temp4 = "";
            temp5 = "";
            temp6 = "";

            for (int mng_hr = first_hrs + 1; mng_hr <= noofhrs; mng_hr++)
            {
                temp1 = "d" + Atday + "d" + mng_hr + "=1";
                temp2 = "d" + Atday + "d" + mng_hr + "=10";
                temp3 = "d" + Atday + "d" + mng_hr + "=2";
                temp4 = "d" + Atday + "d" + mng_hr + "=9";
                temp5 = "d" + Atday + "d" + mng_hr + "=3";
                temp6 = "d" + Atday + "d" + mng_hr + "=5";
                if (eng_present == "")
                {
                    eng_present = temp1;
                    eng_leav = temp2;
                    eng_absent = temp3;
                    eng_sus = temp4;
                    eng_od = temp5;
                    eng_proj = temp6;
                }
                else
                {
                    eng_present = eng_present + " and " + temp1;
                    eng_leav = eng_leav + " or " + temp2;
                    eng_absent = eng_absent + " or " + temp3;
                    eng_sus = eng_sus + " or " + temp4;
                    eng_od = eng_od + " or " + temp5;
                    eng_proj = eng_proj + " or " + temp6;
                }
            }
            if (eng_present != "")
            {
                eng_present = " ( " + eng_present + " ) ";
            }
            else
            {
                eng_present = "";
            }
            if (eng_leav != "")
            {
                eng_leav = " ( " + eng_leav + " ) ";
            }
            else
            {
                eng_leav = "";
            }

            if (eng_absent != "")
            {
                eng_absent = " ( " + eng_absent + " ) ";
            }
            else
            {
                eng_absent = "";
            }
            if (eng_sus != "")
            {
                eng_sus = " ( " + eng_sus + " ) ";
            }
            else
            {
                eng_sus = "";
            }
            if (eng_od != "")
            {
                eng_od = " ( " + eng_od + " ) ";
            }
            else
            {
                eng_od = "";
            }
            if (eng_proj != "")
            {
                eng_proj = " ( " + eng_proj + " ) ";
            }
            else
            {
                eng_proj = "";
            }

        }
        else if (chkPeriod.Checked == true)
        {
            int period = int.Parse(ddlperiod.SelectedItem.ToString());

            if (period <= first_hrs)
            {
                for (int mng_hr = period; mng_hr <= period; mng_hr++)
                {
                    temp1 = "d" + Atday + "d" + mng_hr + "=1";
                    temp2 = "d" + Atday + "d" + mng_hr + "=10";
                    temp3 = "d" + Atday + "d" + mng_hr + "=2";
                    temp4 = "d" + Atday + "d" + mng_hr + "=9";
                    temp5 = "d" + Atday + "d" + mng_hr + "=3";
                    temp6 = "d" + Atday + "d" + mng_hr + "=5";
                    if (mng_present == "")
                    {
                        mng_present = temp1;
                        mng_leav = temp2;
                        mng_absent = temp3;
                        mng_sus = temp4;
                        mng_od = temp5;
                        mng_proj = temp6;
                    }
                    else
                    {
                        mng_present = mng_present + " and " + temp1;
                        mng_leav = mng_leav + " or " + temp2;
                        mng_absent = mng_absent + " or " + temp3;
                        mng_sus = mng_sus + " or " + temp4;
                        mng_od = mng_od + " or " + temp5;
                        mng_proj = mng_proj + " or " + temp6;
                    }
                }
                if (mng_present != "")
                {
                    mng_present = " ( " + mng_present + " ) ";
                }
                else
                {
                    mng_present = "";
                }
                if (mng_leav != "")
                {
                    mng_leav = " ( " + mng_leav + " ) ";
                }
                else
                {
                    mng_leav = "";
                }
                if (mng_absent != "")
                {
                    mng_absent = " ( " + mng_absent + " ) ";
                }
                else
                {
                    mng_absent = "";
                }
                if (mng_sus != "")
                {
                    mng_sus = " ( " + mng_sus + " ) ";
                }
                else
                {
                    mng_sus = "";
                }
                if (mng_od != "")
                {
                    mng_od = " ( " + mng_od + " ) ";
                }
                else
                {
                    mng_od = "";
                }
                if (mng_proj != "")
                {
                    mng_proj = " ( " + mng_proj + " ) ";
                }
                else
                {
                    mng_proj = "";
                }
            }
            else
            {
                for (int mng_hr = period; mng_hr <= period; mng_hr++)
                {
                    temp1 = "d" + Atday + "d" + mng_hr + "=1";
                    temp2 = "d" + Atday + "d" + mng_hr + "=10";
                    temp3 = "d" + Atday + "d" + mng_hr + "=2";
                    temp4 = "d" + Atday + "d" + mng_hr + "=9";
                    temp5 = "d" + Atday + "d" + mng_hr + "=3";
                    temp6 = "d" + Atday + "d" + mng_hr + "=5";
                    if (eng_present == "")
                    {
                        eng_present = temp1;
                        eng_leav = temp2;
                        eng_absent = temp3;
                        eng_sus = temp4;
                        eng_od = temp5;
                        eng_proj = temp6;
                    }
                    else
                    {
                        eng_present = eng_present + " and " + temp1;
                        eng_leav = eng_leav + " or " + temp2;
                        eng_absent = eng_absent + " or " + temp3;
                        eng_sus = eng_sus + " or " + temp4;
                        eng_od = eng_od + " or " + temp5;
                        eng_proj = eng_proj + " or " + temp6;
                    }
                }
                if (eng_present != "")
                {
                    eng_present = " ( " + eng_present + " ) ";
                }
                else
                {
                    eng_present = "";
                }
                if (eng_leav != "")
                {
                    eng_leav = " ( " + eng_leav + " ) ";
                }
                else
                {
                    eng_leav = "";
                }

                if (eng_absent != "")
                {
                    eng_absent = " ( " + eng_absent + " ) ";
                }
                else
                {
                    eng_absent = "";
                }
                if (eng_sus != "")
                {
                    eng_sus = " ( " + eng_sus + " ) ";
                }
                else
                {
                    eng_sus = "";
                }
                if (eng_od != "")
                {
                    eng_od = " ( " + eng_od + " ) ";
                }
                else
                {
                    eng_od = "";
                }
                if (eng_proj != "")
                {
                    eng_proj = " ( " + eng_proj + " ) ";
                }
                else
                {
                    eng_proj = "";
                }
            }
        }

        if (sections.Trim() != "")
        {
            sections = "'" + sections + "'";
        }
        hat.Clear();
        hat.Add("monthyear", MthYear);
        hat.Add("degree_code", deg_code);
        hat.Add("curr_sem", current_sem);
        hat.Add("strsec", sections);
        hat.Add("date", date_concat);
        hat.Add("batch_year", batch_year);
        hat.Add("field_val_mng1", mng_present);
        hat.Add("field_val_mng2", mng_leav);
        hat.Add("field_val_mng3", mng_absent);
        hat.Add("field_val_mng4", mng_sus);
        hat.Add("field_val_mng5", mng_od);
        hat.Add("field_val_mng6", mng_proj);
        hat.Add("field_val_eng1", eng_present);
        hat.Add("field_val_eng2", eng_leav);
        hat.Add("field_val_eng3", eng_absent);
        hat.Add("field_val_eng4", eng_sus);
        hat.Add("field_val_eng5", eng_od);
        hat.Add("field_val_eng6", eng_proj);
        ds_value = dacces2.select_method("find_value_overall", hat, "sp");
        Double morpre = 0, evepre = 0;
        Double morcalc = 0, evecolc = 0;
        if (mng_present != "" && eng_present != "")
        {
            if (ds_value.Tables[0].Rows.Count > 0 && ds_value.Tables[6].Rows.Count > 0)
            {
                if (chkPeriod.Checked == false)
                {
                    temp_val = (double.Parse(ds_value.Tables[0].Rows[0]["Count"].ToString()) / 2) + (double.Parse(ds_value.Tables[6].Rows[0]["Count"].ToString()) / 2);
                }
                else
                {
                    temp_val = (double.Parse(ds_value.Tables[0].Rows[0]["Count"].ToString())) + (double.Parse(ds_value.Tables[6].Rows[0]["Count"].ToString()));
                    attnd_report.Sheets[0].Cells[inirowcnt, 4].Text = temp_val.ToString();
                }
                morpre = Double.Parse(ds_value.Tables[0].Rows[0]["Count"].ToString());
                evepre = Double.Parse(ds_value.Tables[6].Rows[0]["Count"].ToString());
                temp_tot_pres += temp_val;
            }
        }
        else if (mng_present != "" && eng_present == "")
        {
            if (ds_value.Tables[0].Rows.Count > 0)
            {
                if (chkPeriod.Checked == false)
                {
                    temp_val = (double.Parse(ds_value.Tables[0].Rows[0]["Count"].ToString()) / 2);
                }
                else
                {
                    temp_val = (double.Parse(ds_value.Tables[0].Rows[0]["Count"].ToString()));
                    attnd_report.Sheets[0].Cells[inirowcnt, 4].Text = temp_val.ToString();
                }
                morpre = Double.Parse(ds_value.Tables[0].Rows[0]["Count"].ToString());
                evepre = Double.Parse(ds_value.Tables[6].Rows[0]["Count"].ToString());
                temp_tot_pres += temp_val;

            }
        }
        else if (mng_present == "" && eng_present != "")
        {
            if (ds_value.Tables[6].Rows.Count > 0)
            {
                if (chkPeriod.Checked == false)
                {
                    temp_val = (double.Parse(ds_value.Tables[6].Rows[0]["Count"].ToString()) / 2);
                }
                else
                {
                    temp_val = (double.Parse(ds_value.Tables[6].Rows[0]["Count"].ToString()));
                    attnd_report.Sheets[0].Cells[inirowcnt, 4].Text = temp_val.ToString();
                }
                morpre = Double.Parse(ds_value.Tables[0].Rows[0]["Count"].ToString());
                evepre = Double.Parse(ds_value.Tables[6].Rows[0]["Count"].ToString());
                temp_tot_pres += temp_val;
            }
        }
        else
        {
            morpre = 0;
            evepre = 0;
            temp_val = 0;
            temp_tot_pres += temp_val;
            attnd_report.Sheets[0].Cells[inirowcnt, 4].Text = temp_val.ToString();
        }



        if (chkPeriod.Checked == false)
        {
            totmaorp = totmaorp + morpre;
            totevep = totevep + evepre;
            attnd_report.Sheets[0].Cells[inirowcnt, 4].Text = morpre.ToString();
            attnd_report.Sheets[0].Cells[inirowcnt, 5].Text = evepre.ToString();
            attnd_report.Sheets[0].Cells[inirowcnt, 4].HorizontalAlign = HorizontalAlign.Center;
            attnd_report.Sheets[0].Cells[inirowcnt, 5].HorizontalAlign = HorizontalAlign.Center;
            tot_strength_temp = Convert.ToInt32(morpre);
        }
        else
        {
            tot_strength_temp = Convert.ToInt32(temp_val);
        }
        Double morle = 0, evele = 0;
        if (mng_leav != "" && eng_leav != "")
        {
            if (ds_value.Tables[1].Rows.Count > 0 && ds_value.Tables[7].Rows.Count > 0)
            {
                if (chkPeriod.Checked == false)
                {
                    temp_tot = (double.Parse(ds_value.Tables[1].Rows[0]["Count"].ToString()) / 2) + (double.Parse(ds_value.Tables[7].Rows[0]["Count"].ToString()) / 2);
                }
                else
                {
                    temp_tot = (double.Parse(ds_value.Tables[1].Rows[0]["Count"].ToString())) + (double.Parse(ds_value.Tables[7].Rows[0]["Count"].ToString()));
                    attnd_report.Sheets[0].Cells[inirowcnt, 6].Text = temp_tot.ToString();
                }
                morle = Double.Parse(ds_value.Tables[1].Rows[0]["Count"].ToString());
                evele = Double.Parse(ds_value.Tables[7].Rows[0]["Count"].ToString());
                temp_tot_lea += temp_tot;
                ind_tot = temp_tot;
            }
        }
        else if (mng_leav != "" && eng_leav == "")
        {
            if (ds_value.Tables[1].Rows.Count > 0)
            {
                if (chkPeriod.Checked == false)
                {
                    temp_tot = (double.Parse(ds_value.Tables[1].Rows[0]["Count"].ToString()) / 2);
                }
                else
                {
                    temp_tot = (double.Parse(ds_value.Tables[1].Rows[0]["Count"].ToString()));
                    attnd_report.Sheets[0].Cells[inirowcnt, 6].Text = temp_tot.ToString();
                }
                morle = Double.Parse(ds_value.Tables[1].Rows[0]["Count"].ToString());
                evele = Double.Parse(ds_value.Tables[7].Rows[0]["Count"].ToString());
                temp_tot_lea += temp_tot;
                ind_tot = temp_tot;
            }
        }
        else if (mng_leav == "" && eng_leav != "")
        {
            if (ds_value.Tables[7].Rows.Count > 0)
            {
                if (chkPeriod.Checked == false)
                {
                    temp_tot = (double.Parse(ds_value.Tables[7].Rows[0]["Count"].ToString()) / 2);
                }
                else
                {
                    temp_tot = (double.Parse(ds_value.Tables[7].Rows[0]["Count"].ToString()));
                    attnd_report.Sheets[0].Cells[inirowcnt, 6].Text = temp_tot.ToString();
                }
                morle = Double.Parse(ds_value.Tables[1].Rows[0]["Count"].ToString());
                evele = Double.Parse(ds_value.Tables[7].Rows[0]["Count"].ToString());
                temp_tot_lea += temp_tot;
                ind_tot = temp_tot;
            }
        }
        else
        {
            temp_tot = 0;
            temp_tot_lea += temp_tot;
            attnd_report.Sheets[0].Cells[inirowcnt, 6].Text = temp_tot.ToString();
            ind_tot = temp_tot;
        }
        if (chkPeriod.Checked == false)
        {
            morcalc = morcalc + morle;
            evecolc = evecolc + evele;
            totmaorl = totmaorl + morle;
            totevel = totevel + evele;
            attnd_report.Sheets[0].Cells[inirowcnt, 8].Text = morle.ToString();
            attnd_report.Sheets[0].Cells[inirowcnt, 9].Text = evele.ToString();
            attnd_report.Sheets[0].Cells[inirowcnt, 8].HorizontalAlign = HorizontalAlign.Center;
            attnd_report.Sheets[0].Cells[inirowcnt, 9].HorizontalAlign = HorizontalAlign.Center;

        }

        Double morabs = 0, eveabs = 0;
        if (mng_absent != "" && eng_absent != "")
        {
            if (ds_value.Tables[2].Rows.Count > 0 && ds_value.Tables[8].Rows.Count > 0)
            {
                if (chkPeriod.Checked == false)
                {
                    temp_tot = (double.Parse(ds_value.Tables[2].Rows[0]["Count"].ToString()) / 2) + (double.Parse(ds_value.Tables[8].Rows[0]["Count"].ToString()) / 2);
                }
                else
                {
                    temp_tot = (double.Parse(ds_value.Tables[2].Rows[0]["Count"].ToString())) + (double.Parse(ds_value.Tables[8].Rows[0]["Count"].ToString()));
                    attnd_report.Sheets[0].Cells[inirowcnt, 7].Text = temp_tot.ToString();
                }
                morabs = double.Parse(ds_value.Tables[2].Rows[0]["Count"].ToString());
                eveabs = double.Parse(ds_value.Tables[8].Rows[0]["Count"].ToString());
                temp_tot_abs += temp_tot;
                ind_tot += temp_tot;
            }
        }
        else if (mng_absent != "" && eng_absent == "")
        {
            if (ds_value.Tables[2].Rows.Count > 0)
            {
                if (chkPeriod.Checked == false)
                {
                    temp_tot = (double.Parse(ds_value.Tables[2].Rows[0]["Count"].ToString()) / 2);
                }
                else
                {
                    temp_tot = (double.Parse(ds_value.Tables[2].Rows[0]["Count"].ToString()));
                    attnd_report.Sheets[0].Cells[inirowcnt, 7].Text = temp_tot.ToString();
                }
                morabs = double.Parse(ds_value.Tables[2].Rows[0]["Count"].ToString());
                eveabs = double.Parse(ds_value.Tables[8].Rows[0]["Count"].ToString());
                temp_tot_abs += temp_tot;
                ind_tot += temp_tot;
            }
        }

        else if (mng_absent == "" && eng_absent != "")
        {
            if (ds_value.Tables[8].Rows.Count > 0)
            {
                if (chkPeriod.Checked == false)
                {
                    temp_tot = (double.Parse(ds_value.Tables[8].Rows[0]["Count"].ToString()) / 2);
                }
                else
                {
                    temp_tot = (double.Parse(ds_value.Tables[8].Rows[0]["Count"].ToString()));
                    attnd_report.Sheets[0].Cells[inirowcnt, 7].Text = temp_tot.ToString();
                }
                morabs = double.Parse(ds_value.Tables[2].Rows[0]["Count"].ToString());
                eveabs = double.Parse(ds_value.Tables[8].Rows[0]["Count"].ToString());
                temp_tot_abs += temp_tot;
                ind_tot += temp_tot;
            }
        }
        else
        {
            temp_tot = 0;
            temp_tot_abs += temp_tot;
            ind_tot += temp_tot;
            attnd_report.Sheets[0].Cells[inirowcnt, 7].Text = temp_tot.ToString();
        }
        if (chkPeriod.Checked == false)
        {
            morcalc = morcalc + morabs;
            evecolc = evecolc + eveabs;
            totmaora = totmaora + morabs;
            totevea = totevea + eveabs;
            attnd_report.Sheets[0].Cells[inirowcnt, 10].Text = morabs.ToString();
            attnd_report.Sheets[0].Cells[inirowcnt, 11].Text = eveabs.ToString();
            attnd_report.Sheets[0].Cells[inirowcnt, 10].HorizontalAlign = HorizontalAlign.Center;
            attnd_report.Sheets[0].Cells[inirowcnt, 11].HorizontalAlign = HorizontalAlign.Center;
        }
        Double morsus = 0, evesus = 0;
        if (mng_sus != "" && eng_sus != "")
        {
            if (ds_value.Tables[3].Rows.Count > 0 && ds_value.Tables[9].Rows.Count > 0)
            {
                if (chkPeriod.Checked == false)
                {
                    temp_tot = (double.Parse(ds_value.Tables[3].Rows[0]["Count"].ToString()) / 2) + (double.Parse(ds_value.Tables[9].Rows[0]["Count"].ToString()) / 2);
                }
                else
                {
                    temp_tot = (double.Parse(ds_value.Tables[3].Rows[0]["Count"].ToString())) + (double.Parse(ds_value.Tables[9].Rows[0]["Count"].ToString()));
                }
                morsus = Double.Parse(ds_value.Tables[3].Rows[0]["Count"].ToString());
                evesus = Double.Parse(ds_value.Tables[9].Rows[0]["Count"].ToString());
                temp_tot_sus += temp_tot;
                ind_tot += temp_tot;
            }
        }
        else if (mng_sus != "" && eng_sus == "")
        {
            if (ds_value.Tables[3].Rows.Count > 0)
            {
                if (chkPeriod.Checked == false)
                {
                    temp_tot = (double.Parse(ds_value.Tables[3].Rows[0]["Count"].ToString()) / 2);
                }
                else
                {
                    temp_tot = (double.Parse(ds_value.Tables[3].Rows[0]["Count"].ToString()));
                    attnd_report.Sheets[0].Cells[inirowcnt, 8].Text = temp_tot.ToString();
                }
                morsus = Double.Parse(ds_value.Tables[3].Rows[0]["Count"].ToString());
                evesus = Double.Parse(ds_value.Tables[9].Rows[0]["Count"].ToString());
                temp_tot_sus += temp_tot;
                ind_tot += temp_tot;
            }
        }
        else if (mng_sus == "" && eng_sus != "")
        {
            if (ds_value.Tables[9].Rows.Count > 0)
            {
                if (chkPeriod.Checked == false)
                {
                    temp_tot = (double.Parse(ds_value.Tables[9].Rows[0]["Count"].ToString()) / 2);
                }
                else
                {
                    temp_tot = (double.Parse(ds_value.Tables[9].Rows[0]["Count"].ToString()));
                    attnd_report.Sheets[0].Cells[inirowcnt, 8].Text = temp_tot.ToString();
                }
                morsus = Double.Parse(ds_value.Tables[3].Rows[0]["Count"].ToString());
                evesus = Double.Parse(ds_value.Tables[9].Rows[0]["Count"].ToString());
                temp_tot_sus += temp_tot;
                ind_tot += temp_tot;
            }
        }
        else
        {
            temp_tot = 0;
            temp_tot_sus += temp_tot;
            ind_tot += temp_tot;
            attnd_report.Sheets[0].Cells[inirowcnt, 8].Text = temp_tot.ToString();
        }
        if (chkPeriod.Checked == false)
        {
            morcalc = morcalc + morsus;
            evecolc = evecolc + evesus;
            totmaors = totmaors + morsus;
            toteves = toteves + evesus;
            attnd_report.Sheets[0].Cells[inirowcnt, 12].Text = morsus.ToString();
            attnd_report.Sheets[0].Cells[inirowcnt, 13].Text = evesus.ToString();
            attnd_report.Sheets[0].Cells[inirowcnt, 12].HorizontalAlign = HorizontalAlign.Center;
            attnd_report.Sheets[0].Cells[inirowcnt, 13].HorizontalAlign = HorizontalAlign.Center;
        }
        Double morod = 0, eveod = 0;
        if (mng_od != "" && eng_od != "")
        {
            if (ds_value.Tables[4].Rows.Count > 0 && ds_value.Tables[10].Rows.Count > 0)
            {
                if (chkPeriod.Checked == false)
                {
                    temp_tot = (double.Parse(ds_value.Tables[4].Rows[0]["Count"].ToString()) / 2) + (double.Parse(ds_value.Tables[10].Rows[0]["Count"].ToString()) / 2);
                }
                else
                {
                    temp_tot = (double.Parse(ds_value.Tables[4].Rows[0]["Count"].ToString())) + (double.Parse(ds_value.Tables[10].Rows[0]["Count"].ToString()));
                }
                morod = double.Parse(ds_value.Tables[4].Rows[0]["Count"].ToString());
                eveod = double.Parse(ds_value.Tables[10].Rows[0]["Count"].ToString());
                temp_tot_od += temp_tot;
                ind_tot += temp_tot;
            }
        }
        else if (mng_od != "" && eng_od == "")
        {
            if (ds_value.Tables[4].Rows.Count > 0)
            {
                if (chkPeriod.Checked == false)
                {
                    temp_tot = (double.Parse(ds_value.Tables[4].Rows[0]["Count"].ToString()) / 2);
                }
                else
                {
                    temp_tot = (double.Parse(ds_value.Tables[4].Rows[0]["Count"].ToString()));
                    attnd_report.Sheets[0].Cells[inirowcnt, 9].Text = temp_tot.ToString();
                }
                morod = double.Parse(ds_value.Tables[4].Rows[0]["Count"].ToString());
                eveod = double.Parse(ds_value.Tables[10].Rows[0]["Count"].ToString());
                temp_tot_od += temp_tot;
                ind_tot += temp_tot;
            }
        }
        else if (mng_od == "" && eng_od != "")
        {
            if (ds_value.Tables[10].Rows.Count > 0)
            {
                if (chkPeriod.Checked == false)
                {
                    temp_tot = (double.Parse(ds_value.Tables[10].Rows[0]["Count"].ToString()) / 2);
                }
                else
                {
                    temp_tot = (double.Parse(ds_value.Tables[10].Rows[0]["Count"].ToString()));
                    attnd_report.Sheets[0].Cells[inirowcnt, 9].Text = temp_tot.ToString();
                }
                morod = double.Parse(ds_value.Tables[4].Rows[0]["Count"].ToString());
                eveod = double.Parse(ds_value.Tables[10].Rows[0]["Count"].ToString());
                temp_tot_od += temp_tot;
                ind_tot += temp_tot;
            }
        }
        else
        {
            temp_tot = 0;
            temp_tot_od += temp_tot;
            ind_tot += temp_tot;
            attnd_report.Sheets[0].Cells[inirowcnt, 9].Text = temp_tot.ToString();
        }
        if (chkPeriod.Checked == false)
        {
            morcalc = morcalc + morod;
            evecolc = evecolc + eveod;
            totmaorod = totmaorod + morod;
            toteveod = toteveod + eveod;
            attnd_report.Sheets[0].Cells[inirowcnt, 14].Text = morod.ToString();
            attnd_report.Sheets[0].Cells[inirowcnt, 15].Text = eveod.ToString();
            attnd_report.Sheets[0].Cells[inirowcnt, 14].HorizontalAlign = HorizontalAlign.Center;
            attnd_report.Sheets[0].Cells[inirowcnt, 15].HorizontalAlign = HorizontalAlign.Center;
        }
        Double morpro = 0, evepro = 0;
        if (mng_proj != "" && eng_proj != "")
        {
            if (ds_value.Tables[5].Rows.Count > 0 && ds_value.Tables[11].Rows.Count > 0)
            {
                if (chkPeriod.Checked == false)
                {
                    temp_tot = (double.Parse(ds_value.Tables[5].Rows[0]["Count"].ToString()) / 2) + (double.Parse(ds_value.Tables[11].Rows[0]["Count"].ToString()) / 2);
                }
                else
                {
                    temp_tot = (double.Parse(ds_value.Tables[5].Rows[0]["Count"].ToString())) + (double.Parse(ds_value.Tables[11].Rows[0]["Count"].ToString()));
                }
                morpro = Double.Parse(ds_value.Tables[5].Rows[0]["Count"].ToString());
                evepro = Double.Parse(ds_value.Tables[11].Rows[0]["Count"].ToString());
                temp_tot_sod += temp_tot;
                ind_tot += temp_tot;
            }
        }
        else if (mng_proj != "" && eng_proj == "")
        {
            if (ds_value.Tables[5].Rows.Count > 0)
            {
                if (chkPeriod.Checked == false)
                {
                    temp_tot = (double.Parse(ds_value.Tables[5].Rows[0]["Count"].ToString()) / 2);
                }
                else
                {
                    temp_tot = (double.Parse(ds_value.Tables[5].Rows[0]["Count"].ToString()));
                    attnd_report.Sheets[0].Cells[inirowcnt, 10].Text = temp_tot.ToString();
                }
                morpro = Double.Parse(ds_value.Tables[5].Rows[0]["Count"].ToString());
                evepro = Double.Parse(ds_value.Tables[11].Rows[0]["Count"].ToString());
                temp_tot_sod += temp_tot;
                ind_tot += temp_tot;
            }
        }
        else if (mng_proj == "" && eng_proj != "")
        {
            if (ds_value.Tables[11].Rows.Count > 0)
            {
                if (chkPeriod.Checked == false)
                {
                    temp_tot = (double.Parse(ds_value.Tables[11].Rows[0]["Count"].ToString()) / 2);
                }
                else
                {
                    temp_tot = (double.Parse(ds_value.Tables[11].Rows[0]["Count"].ToString()));
                    attnd_report.Sheets[0].Cells[inirowcnt, 10].Text = temp_tot.ToString();
                }
                morpro = Double.Parse(ds_value.Tables[5].Rows[0]["Count"].ToString());
                evepro = Double.Parse(ds_value.Tables[11].Rows[0]["Count"].ToString());
                temp_tot_sod += temp_tot;
                ind_tot += temp_tot;
            }
        }
        else
        {
            temp_tot = 0;
            temp_tot_sod += temp_tot;
            ind_tot += temp_tot;
            attnd_report.Sheets[0].Cells[inirowcnt, 10].Text = temp_tot.ToString();
        }
        if (chkPeriod.Checked == false)
        {
            morcalc = morcalc + morpro;
            evecolc = evecolc + evepro;
            totmaorsod = totmaorsod + morpro;
            totevesod = totevesod + evepro;
            attnd_report.Sheets[0].Cells[inirowcnt, 16].Text = morpro.ToString();
            attnd_report.Sheets[0].Cells[inirowcnt, 17].Text = evepro.ToString();
            attnd_report.Sheets[0].Cells[inirowcnt, 16].HorizontalAlign = HorizontalAlign.Center;
            attnd_report.Sheets[0].Cells[inirowcnt, 17].HorizontalAlign = HorizontalAlign.Center;
        }
        if (chkPeriod.Checked == false)
        {
            morcalc = morcalc + morpro;
            evecolc = evecolc + evepro;
            totmaorall = totmaorall + morcalc;
            toteveall = toteveall + evecolc;
            attnd_report.Sheets[0].Cells[inirowcnt, 6].Text = morcalc.ToString();
            attnd_report.Sheets[0].Cells[inirowcnt, 7].Text = evecolc.ToString();
            attnd_report.Sheets[0].Cells[inirowcnt, 6].HorizontalAlign = HorizontalAlign.Center;
            attnd_report.Sheets[0].Cells[inirowcnt, 7].HorizontalAlign = HorizontalAlign.Center;
            tot_strength_temp = tot_strength_temp + Convert.ToInt32(morcalc);
        }
        else
        {
            attnd_report.Sheets[0].Cells[inirowcnt, 5].Text = ind_tot.ToString();
            tot_strength_temp = tot_strength_temp + Convert.ToInt32(ind_tot);
        }

        //Modified by sridhar 31/july/2014
        if (ds_value.Tables[12].Rows.Count > 0)
        {
            //tot_strength_temp = int.Parse(ds_value.Tables[12].Rows[0]["Count"].ToString());
            //attnd_report.Sheets[0].Cells[inirowcnt, 3].Text = tot_strength_temp.ToString();
            //tot_strength += tot_strength_temp;
            //attnd_report.Sheets[0].Cells[inirowcnt, 3].HorizontalAlign = HorizontalAlign.Center;

            //   tot_strength_temp = int.Parse(ds_value.Tables[12].Rows[0]["Count"].ToString());
            attnd_report.Sheets[0].Cells[inirowcnt, 3].Text = tot_strength_temp.ToString();
            tot_strength += tot_strength_temp;
            attnd_report.Sheets[0].Cells[inirowcnt, 3].HorizontalAlign = HorizontalAlign.Center;

        }
        if (chkPeriod.Checked == true)
        {
            morpre = temp_val;
        }


        string year = sem_roman(int.Parse(current_sem) / 2);
        if (!dicdegree.ContainsKey(year + '-' + acronym))
        {
            dicdegree.Add(year + '-' + acronym, inirowcnt);
            yeardepttostud = Convert.ToDouble(tot_strength_temp);
            yeardepttostudpresent = Convert.ToDouble(morpre);
            yeardepttostudpresenteve = Convert.ToDouble(evepre);
        }
        else
        {
            yeardepttostud = yeardepttostud + Convert.ToDouble(tot_strength_temp);
            yeardepttostudpresent = yeardepttostudpresent + Convert.ToDouble(morpre);
            yeardepttostudpresenteve = yeardepttostudpresenteve + Convert.ToDouble(evepre);
        }
        Double precentvalue = yeardepttostudpresent / yeardepttostud * 100;
        if (precentvalue.ToString() == "NaN")
        {
            precentvalue = 0;
        }
        precentvalue = Math.Round(precentvalue, 2, MidpointRounding.AwayFromZero);
        if (chkPeriod.Checked == true)
        {
            Double morningsecper = Convert.ToDouble(morpre) / Convert.ToDouble(tot_strength_temp) * 100;
            if (morningsecper.ToString() == "NaN")
            {
                morningsecper = 0;
            }
            morningsecper = Math.Round(morningsecper, 2, MidpointRounding.AwayFromZero);
            attnd_report.Sheets[0].Cells[inirowcnt, 11].Text = morningsecper.ToString();
        }
        int setrow = dicdegree[year + '-' + acronym];


        if (chkPeriod.Checked == false)
        {
            Double morningsecper = Convert.ToDouble(morpre) / Convert.ToDouble(tot_strength_temp) * 100;
            if (morningsecper.ToString() == "NaN")
            {
                morningsecper = 0;
            }
            morningsecper = Math.Round(morningsecper, 2, MidpointRounding.AwayFromZero);
            FarPoint.Web.Spread.TextCellType txt = new FarPoint.Web.Spread.TextCellType();
            attnd_report.Sheets[0].Cells[inirowcnt, 18].CellType = txt;
            attnd_report.Sheets[0].Cells[inirowcnt, 18].Text = morningsecper.ToString();

            Double evesecper = Convert.ToDouble(evepre) / Convert.ToDouble(tot_strength_temp) * 100;
            if (evesecper.ToString() == "NaN")
            {
                evesecper = 0;
            }
            evesecper = Math.Round(evesecper, 2, MidpointRounding.AwayFromZero);
            attnd_report.Sheets[0].Cells[inirowcnt, 19].CellType = txt;
            attnd_report.Sheets[0].Cells[inirowcnt, 19].Text = evesecper.ToString();

            attnd_report.Sheets[0].Cells[setrow, 20].Text = precentvalue.ToString();
            attnd_report.Sheets[0].SpanModel.Add(setrow, 20, inirowcnt - setrow + 1, 1);

            precentvalue = yeardepttostudpresenteve / yeardepttostud * 100;
            if (precentvalue.ToString() == "NaN")
            {
                precentvalue = 0;
            }
            precentvalue = Math.Round(precentvalue, 2, MidpointRounding.AwayFromZero);
            attnd_report.Sheets[0].Cells[setrow, 21].Text = precentvalue.ToString();
            attnd_report.Sheets[0].SpanModel.Add(setrow, 21, inirowcnt - setrow + 1, 1);

            for (int ev = 0; ev < 2; ev++)
            {
                string sedegre = deg_code + ":M";
                if (ev == 1)
                {
                    sedegre = deg_code + ":E";
                }
                if (dicdegreeattper.ContainsKey(sedegre))
                {
                    string[] spva = dicdegreeattper[sedegre].ToString().Split(':');
                    int noofsub = 0;
                    if (spva.GetUpperBound(0) > 0)
                    {
                        noofsub = Convert.ToInt32(spva[1]);
                    }
                    noofsub++;
                    Double stuspers = 0;
                    if (ev == 0)
                    {
                        stuspers = Convert.ToDouble(spva[0]) + Convert.ToDouble(morningsecper);
                    }
                    else
                    {
                        stuspers = Convert.ToDouble(spva[0]) + Convert.ToDouble(evesecper);
                    }
                    dicdegreeattper[sedegre] = stuspers + ":" + noofsub;
                }
            }
        }
        else
        {
            attnd_report.Sheets[0].Cells[setrow, 12].Text = precentvalue.ToString();
            attnd_report.Sheets[0].SpanModel.Add(setrow, 12, inirowcnt - setrow + 1, 1);

            if (dicdegreeattper.ContainsKey(deg_code))
            {
                string[] spva = dicdegreeattper[deg_code].ToString().Split(':');
                int noofsub = 0;
                if (spva.GetUpperBound(0) > 0)
                {
                    noofsub = Convert.ToInt32(spva[1]);
                }
                noofsub++;
                Double stuspers = Convert.ToDouble(spva[0]) + Convert.ToDouble(precentvalue);
                dicdegreeattper[deg_code] = stuspers + ":" + noofsub;
            }
        }


        tot_tot += ind_tot;
        ind_tot = 0;


    }

    public void loadvalues()
    {
        //  try
        {
            int row_hearder_count = 0;

            attnd_report.Visible = true;
            btnprintmaster.Visible = true;
            btnxl.Visible = true;
            //Added by Srinath 27/2/2
            lblrptname.Visible = true;
            txtexcelname.Visible = true;
            lblpages.Visible = true;
            ddlpage.Visible = true;
            //'----------------------- design

            attnd_report.Sheets[0].RowCount = 0;
            attnd_report.Sheets[0].ColumnHeader.RowCount = 0; //Modified by Manikandan 18/05/2013
            //'----------------------------------------- Split the date
            date = txtFromDate.Text.ToString();
            string[] split_date = date.Split(new char[] { '/' });
            Atday = split_date[0].ToString();
            Atmnth = split_date[1].ToString();
            Atyr = split_date[2].ToString();
            todaydate = Atmnth + "/" + Atday + "/" + Atyr;
            DateTime input_date = Convert.ToDateTime(todaydate.ToString());
            date_concat = "'" + date + "'";
            MthYear = (Convert.ToInt32(Atyr) * 12) + Convert.ToInt32(Atmnth);
            //'---------------------------------------------           
            {
                //=============================0n 02/07/12
                has.Clear();
                has.Add("college_code", Session["InternalCollegeCode"].ToString());
                has.Add("form_name", "ovrall_attreport_perday.aspx");
                dsprint = dacces2.select_method("PROC_PRINT_MASTER_SETTINGS", has, "sp");
                //===========================================

                //======================0n 02/07/12 PRABHA
                if (dsprint.Tables[0].Rows.Count > 0)
                {
                    if (dsprint.Tables[0].Rows[0]["new_header_name"].ToString() != null && dsprint.Tables[0].Rows[0]["new_header_name"].ToString() != "")
                    {
                        //attnd_report.Sheets[0].ColumnHeader.Cells[6, 0].Border.BorderColorBottom = Color.White;
                        new_header_string = dsprint.Tables[0].Rows[0]["new_header_name"].ToString();
                        new_header_string_split = new_header_string.Split(',');
                        //attnd_report.Sheets[0].SheetCorner.RowCount = attnd_report.Sheets[0].SheetCorner.RowCount + new_header_string_split.GetUpperBound(0) + 1;
                    }
                }
                //=====================================
                attnd_report.Sheets[0].ColumnCount = 0;
                if (chkPeriod.Checked == false)
                {
                    attnd_report.Sheets[0].ColumnCount = 23;
                    attnd_report.Sheets[0].ColumnHeader.RowCount = 2;

                    attnd_report.Sheets[0].ColumnHeader.Cells[(attnd_report.Sheets[0].ColumnHeader.RowCount - 2), 0].Text = "S.No";
                    attnd_report.Sheets[0].ColumnHeaderSpanModel.Add(attnd_report.Sheets[0].ColumnHeader.RowCount - 2, 0, 2, 1);
                    attnd_report.Sheets[0].ColumnHeader.Cells[(attnd_report.Sheets[0].ColumnHeader.RowCount - 2), 1].Text = "Dept";
                    if (forschoolsetting == true)
                    {
                        attnd_report.Sheets[0].ColumnHeader.Cells[(attnd_report.Sheets[0].ColumnHeader.RowCount - 2), 1].Text = "Standard";
                    }
                    attnd_report.Sheets[0].ColumnHeaderSpanModel.Add(attnd_report.Sheets[0].ColumnHeader.RowCount - 2, 1, 2, 1);
                    attnd_report.Sheets[0].ColumnHeader.Cells[(attnd_report.Sheets[0].ColumnHeader.RowCount - 2), 2].Text = "Year";

                    attnd_report.Sheets[0].ColumnHeaderSpanModel.Add(attnd_report.Sheets[0].ColumnHeader.RowCount - 2, 2, 2, 1);
                    attnd_report.Sheets[0].ColumnHeader.Cells[(attnd_report.Sheets[0].ColumnHeader.RowCount - 2), 3].Text = "No. on ";
                    attnd_report.Sheets[0].ColumnHeaderSpanModel.Add(attnd_report.Sheets[0].ColumnHeader.RowCount - 2, 3, 2, 1);
                    attnd_report.Sheets[0].ColumnHeader.Cells[(attnd_report.Sheets[0].ColumnHeader.RowCount - 2), 4].Text = "P";
                    attnd_report.Sheets[0].ColumnHeader.Cells[(attnd_report.Sheets[0].ColumnHeader.RowCount - 1), 4].Text = "M";
                    attnd_report.Sheets[0].ColumnHeader.Cells[(attnd_report.Sheets[0].ColumnHeader.RowCount - 1), 5].Text = "E";
                    attnd_report.Sheets[0].ColumnHeaderSpanModel.Add(attnd_report.Sheets[0].ColumnHeader.RowCount - 2, 4, 1, 2);

                    attnd_report.Sheets[0].ColumnHeader.Cells[(attnd_report.Sheets[0].ColumnHeader.RowCount - 2), 6].Text = "Total(L-A-S-OD-SOD)";
                    attnd_report.Sheets[0].ColumnHeader.Cells[(attnd_report.Sheets[0].ColumnHeader.RowCount - 1), 6].Text = "M";
                    attnd_report.Sheets[0].ColumnHeader.Cells[(attnd_report.Sheets[0].ColumnHeader.RowCount - 1), 7].Text = "E";
                    attnd_report.Sheets[0].ColumnHeaderSpanModel.Add(attnd_report.Sheets[0].ColumnHeader.RowCount - 2, 6, 1, 2);
                    attnd_report.Sheets[0].ColumnHeader.Cells[(attnd_report.Sheets[0].ColumnHeader.RowCount - 2), 8].Text = "L";
                    attnd_report.Sheets[0].ColumnHeader.Cells[(attnd_report.Sheets[0].ColumnHeader.RowCount - 1), 8].Text = "M";
                    attnd_report.Sheets[0].ColumnHeader.Cells[(attnd_report.Sheets[0].ColumnHeader.RowCount - 1), 9].Text = "E";
                    attnd_report.Sheets[0].ColumnHeaderSpanModel.Add(attnd_report.Sheets[0].ColumnHeader.RowCount - 2, 8, 1, 2);
                    attnd_report.Sheets[0].ColumnHeader.Cells[(attnd_report.Sheets[0].ColumnHeader.RowCount - 2), 10].Text = "A";
                    attnd_report.Sheets[0].ColumnHeader.Cells[(attnd_report.Sheets[0].ColumnHeader.RowCount - 1), 10].Text = "M";
                    attnd_report.Sheets[0].ColumnHeader.Cells[(attnd_report.Sheets[0].ColumnHeader.RowCount - 1), 11].Text = "E";
                    attnd_report.Sheets[0].ColumnHeaderSpanModel.Add(attnd_report.Sheets[0].ColumnHeader.RowCount - 2, 10, 1, 2);
                    attnd_report.Sheets[0].ColumnHeader.Cells[(attnd_report.Sheets[0].ColumnHeader.RowCount - 2), 12].Text = "S";
                    attnd_report.Sheets[0].ColumnHeader.Cells[(attnd_report.Sheets[0].ColumnHeader.RowCount - 1), 12].Text = "M";
                    attnd_report.Sheets[0].ColumnHeader.Cells[(attnd_report.Sheets[0].ColumnHeader.RowCount - 1), 13].Text = "E";
                    attnd_report.Sheets[0].ColumnHeaderSpanModel.Add(attnd_report.Sheets[0].ColumnHeader.RowCount - 2, 12, 1, 2);
                    attnd_report.Sheets[0].ColumnHeader.Cells[(attnd_report.Sheets[0].ColumnHeader.RowCount - 2), 14].Text = "OD";
                    attnd_report.Sheets[0].ColumnHeader.Cells[(attnd_report.Sheets[0].ColumnHeader.RowCount - 1), 14].Text = "M";
                    attnd_report.Sheets[0].ColumnHeader.Cells[(attnd_report.Sheets[0].ColumnHeader.RowCount - 1), 15].Text = "E";
                    attnd_report.Sheets[0].ColumnHeaderSpanModel.Add(attnd_report.Sheets[0].ColumnHeader.RowCount - 2, 14, 1, 2);
                    attnd_report.Sheets[0].ColumnHeader.Cells[(attnd_report.Sheets[0].ColumnHeader.RowCount - 2), 16].Text = "SOD";
                    attnd_report.Sheets[0].ColumnHeader.Cells[(attnd_report.Sheets[0].ColumnHeader.RowCount - 1), 16].Text = "M";
                    attnd_report.Sheets[0].ColumnHeader.Cells[(attnd_report.Sheets[0].ColumnHeader.RowCount - 1), 17].Text = "E";
                    attnd_report.Sheets[0].ColumnHeaderSpanModel.Add(attnd_report.Sheets[0].ColumnHeader.RowCount - 2, 16, 1, 2);

                    attnd_report.Sheets[0].ColumnHeader.Cells[(attnd_report.Sheets[0].ColumnHeader.RowCount - 2), 18].Text = "%";
                    attnd_report.Sheets[0].ColumnHeaderSpanModel.Add(attnd_report.Sheets[0].ColumnHeader.RowCount - 2, 18, 1, 2);
                    attnd_report.Sheets[0].ColumnHeader.Cells[(attnd_report.Sheets[0].ColumnHeader.RowCount - 1), 18].Text = "M";
                    attnd_report.Sheets[0].ColumnHeader.Cells[(attnd_report.Sheets[0].ColumnHeader.RowCount - 1), 19].Text = "E";

                    attnd_report.Sheets[0].ColumnHeader.Cells[(attnd_report.Sheets[0].ColumnHeader.RowCount - 2), 20].Text = "Dept Wise Over All %";
                    attnd_report.Sheets[0].ColumnHeaderSpanModel.Add(attnd_report.Sheets[0].ColumnHeader.RowCount - 2, 20, 1, 2);
                    attnd_report.Sheets[0].ColumnHeader.Cells[(attnd_report.Sheets[0].ColumnHeader.RowCount - 1), 20].Text = "M";
                    attnd_report.Sheets[0].ColumnHeader.Cells[(attnd_report.Sheets[0].ColumnHeader.RowCount - 1), 21].Text = "E";

                    attnd_report.Sheets[0].ColumnHeader.Cells[(attnd_report.Sheets[0].ColumnHeader.RowCount - 2), 22].Text = "Remarks";
                    attnd_report.Sheets[0].ColumnHeaderSpanModel.Add(attnd_report.Sheets[0].ColumnHeader.RowCount - 2, 22, 2, 1);
                    attnd_report.Sheets[0].Columns[18].VerticalAlign = VerticalAlign.Middle;
                    attnd_report.Sheets[0].Columns[19].VerticalAlign = VerticalAlign.Middle;

                    attnd_report.Sheets[0].Columns[6].Visible = false;
                    attnd_report.Sheets[0].Columns[7].Visible = false;
                    attnd_report.Sheets[0].Columns[12].Visible = false;
                    attnd_report.Sheets[0].Columns[13].Visible = false;
                    attnd_report.Sheets[0].Columns[16].Visible = false;
                    attnd_report.Sheets[0].Columns[17].Visible = false;

                }
                else
                {
                    attnd_report.Sheets[0].ColumnCount = 14;
                    attnd_report.Sheets[0].ColumnHeader.RowCount++;
                    attnd_report.Sheets[0].ColumnHeader.Cells[(attnd_report.Sheets[0].ColumnHeader.RowCount - 1), 0].Text = "S.No";
                    attnd_report.Sheets[0].ColumnHeader.Cells[(attnd_report.Sheets[0].ColumnHeader.RowCount - 1), 1].Text = "Dept";
                    attnd_report.Sheets[0].ColumnHeader.Cells[(attnd_report.Sheets[0].ColumnHeader.RowCount - 1), 2].Text = "Year";
                    attnd_report.Sheets[0].ColumnHeader.Cells[(attnd_report.Sheets[0].ColumnHeader.RowCount - 1), 3].Text = "No.on";
                    attnd_report.Sheets[0].ColumnHeader.Cells[(attnd_report.Sheets[0].ColumnHeader.RowCount - 1), 4].Text = "P";
                    attnd_report.Sheets[0].ColumnHeader.Cells[(attnd_report.Sheets[0].ColumnHeader.RowCount - 1), 5].Text = "Total (L-A-S-OD-SOD)";
                    attnd_report.Sheets[0].ColumnHeader.Cells[(attnd_report.Sheets[0].ColumnHeader.RowCount - 1), 6].Text = "L";
                    attnd_report.Sheets[0].ColumnHeader.Cells[(attnd_report.Sheets[0].ColumnHeader.RowCount - 1), 7].Text = "A";
                    attnd_report.Sheets[0].ColumnHeader.Cells[(attnd_report.Sheets[0].ColumnHeader.RowCount - 1), 8].Text = "S";
                    attnd_report.Sheets[0].ColumnHeader.Cells[(attnd_report.Sheets[0].ColumnHeader.RowCount - 1), 9].Text = "OD";
                    attnd_report.Sheets[0].ColumnHeader.Cells[(attnd_report.Sheets[0].ColumnHeader.RowCount - 1), 10].Text = "SOD";
                    attnd_report.Sheets[0].ColumnHeader.Cells[(attnd_report.Sheets[0].ColumnHeader.RowCount - 1), 11].Text = "%";
                    attnd_report.Sheets[0].ColumnHeader.Cells[(attnd_report.Sheets[0].ColumnHeader.RowCount - 1), 12].Text = "Year Wise Pecentage";
                    attnd_report.Sheets[0].ColumnHeader.Cells[(attnd_report.Sheets[0].ColumnHeader.RowCount - 1), 13].Text = "Remarks";
                    attnd_report.Sheets[0].Columns[11].VerticalAlign = VerticalAlign.Middle;

                    attnd_report.Sheets[0].Columns[5].Visible = false;
                    attnd_report.Sheets[0].Columns[8].Visible = false;
                    attnd_report.Sheets[0].Columns[10].Visible = false;
                }

                for (int c = 0; c < attnd_report.Sheets[0].ColumnCount; c++)
                {
                    attnd_report.Sheets[0].Columns[c].VerticalAlign = VerticalAlign.Middle;
                }
                Double gmp = 0, gep = 0;
                Double gmal = 0, geall = 0;
                Double gml = 0, gel = 0;
                Double gma = 0, gea = 0;
                Double gms = 0, ges = 0;
                Double gmod = 0, geod = 0;
                Double gmsod = 0, gesod = 0;
                string semes = "";
                errlbl.Text = "";
                errlbl.Visible = false;
                int selsem = 0;
                //for (int sem = 0; sem < chklstsem.Items.Count; sem++)
                //{
                //    if (chklstsem.Items[sem].Selected == true)
                //    {
                //        selsem++;
                //        if (semes == "")
                //        {
                //            semes = Convert.ToString(chklstsem.Items[sem].Text);
                //        }
                //        else
                //        {
                //            semes += "," + Convert.ToString(chklstsem.Items[sem].Text);
                //        }
                //    }
                //}
                if (txtbatch.Text != "---Select---" && txtdegree.Text != "---Select---" && txtbranch.Text != "---Select---")
                {
                    string batchyear = "";
                    if (txtbatch.Text != "--Select--")
                    {
                        for (int j = 0; j < chklsbatch.Items.Count; j++)
                        {
                            if (chklsbatch.Items[j].Selected == true)
                            {
                                if (batchyear == "")
                                    batchyear = "'" + chklsbatch.Items[j].Value.ToString() + "'";
                                else
                                    batchyear = batchyear + "," + "'" + chklsbatch.Items[j].Value.ToString() + "'";
                            }
                        }
                    }
                    string degree = "";
                    if (txtdegree.Text != "--Select--")
                    {
                        for (int s = 0; s < chklstdegree.Items.Count; s++)
                        {
                            if (chklstdegree.Items[s].Selected == true)
                            {
                                if (degree == "")
                                    degree = "'" + chklstdegree.Items[s].Value.ToString() + "'";
                                else
                                    degree = degree + "," + "'" + chklstdegree.Items[s].Value.ToString() + "'";
                            }
                        }
                    }

                    string branch = "";
                    if (txtbranch.Text != "--Select--")
                    {
                        for (int k = 0; k < chklstbranch.Items.Count; k++)
                        {
                            if (chklstbranch.Items[k].Selected == true)
                            {
                                if (branch == "")
                                    branch = "'" + chklstbranch.Items[k].Value.ToString() + "'";
                                else
                                    branch = branch + "," + "'" + chklstbranch.Items[k].Value.ToString() + "'";
                            }
                        }
                    }


                    //if (selsem == 0)
                    //{
                    //    errlbl.Text = "Please Select Atleast One Semester.";
                    //    errlbl.Visible = true;
                    //    return;
                    //}
                    attnd_report.Sheets[0].Columns[1].VerticalAlign = VerticalAlign.Middle;
                    attnd_report.Sheets[0].Columns[0].VerticalAlign = VerticalAlign.Middle;
                    attnd_report.Sheets[0].Columns[0].HorizontalAlign = HorizontalAlign.Center;
                    attnd_report.Sheets[0].Columns[1].HorizontalAlign = HorizontalAlign.Left;
                    attnd_report.Sheets[0].Columns[2].HorizontalAlign = HorizontalAlign.Left;
                    attnd_report.Sheets[0].Columns[1].Font.Bold = true;
                    //'--------------------------------------------------------------- Query for select degree
                    string strsemval = "select * from seminfo ";
                    DataSet dssem = d2.select_method_wo_parameter(strsemval, "Text");
                    DataView dvsem = new DataView();
                    strDegree = "select * from degree where college_code='" + Session["InternalCollegeCode"] + "' ORDER BY DEGREE_CODE";
                    con_deg.Close();
                    con_deg.Open();
                    SqlCommand cmddeg = new SqlCommand(strDegree, con_deg);
                    SqlDataReader drdeg;
                    drdeg = cmddeg.ExecuteReader();

                    for (int sk = 0; sk < chklsbatch.Items.Count; sk++)
                    {

                        if (chklsbatch.Items[sk].Selected == true)
                        {
                            row_hearder_count = 0;
                            string year = chklsbatch.Items[sk].Value.ToString();
                            ds = dacces2.select_method_wo_parameter("Select distinct registration.degree_code as 'DegreeCode', current_semester as 'Current_Semester', sections as 'Section', registration.batch_year as 'BatchYear' , no_of_hrs_per_day as 'NoOfHours', no_of_hrs_I_half_day as 'FirstHalf', no_of_hrs_II_half_day as 'SecondHalf', start_date as 'StartDate' from registration,PeriodAttndSchedule,seminfo where cc = 0  and delflag = 0 and exam_flag <> 'debar' and current_semester is not null   and registration.degree_code=PeriodAttndSchedule.degree_code  and registration.current_semester = PeriodAttndSchedule.semester And registration.current_semester = seminfo.semester and registration.degree_code=seminfo.degree_code and registration.batch_year=seminfo.batch_year and registration.degree_code in(" + branch + ") and registration.Batch_Year in(" + year + ") and  start_date<='" + input_date + "' and no_of_hrs_per_day<>0  order by current_semester", "text");
                            //if (ds.Tables[0].Rows.Count == 0)
                            //{
                            //    row_hearder_count--;
                            //}and registration.current_semester in (" + semes + ")

                            if (ds.Tables[0].Rows.Count > 0)
                            {
                                row_hearder_count++;
                                deg_code = "";
                                for (count = 0; count < ds.Tables[0].Rows.Count; count++)
                                {
                                    rowhead++;

                                    fflag = false;
                                    if (drdeg.HasRows == true)
                                    {
                                        //if (deg_code != ds.Tables[0].Rows[count]["DegreeCode"].ToString())
                                        //{
                                        deg_code = ds.Tables[0].Rows[count]["DegreeCode"].ToString();
                                        acronym = d2.GetFunction("select Acronym from degree where Degree_Code='" + deg_code + "'");
                                        if (chkPeriod.Checked == false)
                                        {
                                            if (!dicdegreeattper.ContainsKey(deg_code + ":M"))
                                            {
                                                dicdegreeattper.Add(deg_code + ":M", "0");
                                                dicdegreeattper.Add(deg_code + ":E", "0");
                                            }
                                        }
                                        else
                                        {
                                            if (!dicdegreeattper.ContainsKey(deg_code))
                                            {
                                                dicdegreeattper.Add(deg_code, "0");
                                            }
                                        }
                                        batch_year = ds.Tables[0].Rows[count]["BatchYear"].ToString();
                                        sections = ds.Tables[0].Rows[count]["Section"].ToString();
                                        current_sem = ds.Tables[0].Rows[count]["Current_Semester"].ToString();
                                        noofhrs = int.Parse(ds.Tables[0].Rows[count]["NoOfHours"].ToString());
                                        first_hrs = int.Parse(ds.Tables[0].Rows[count]["FirstHalf"].ToString());
                                        sec_hrs = int.Parse(ds.Tables[0].Rows[count]["SecondHalf"].ToString());

                                        fflag = true;
                                        norecflag = true;
                                        //Added by srinath 1/8/2014 
                                        dssem.Tables[0].DefaultView.RowFilter = " batch_year='" + batch_year + "' and degree_code='" + deg_code + "' and semester='" + current_sem + "'";
                                        dvsem = dssem.Tables[0].DefaultView;
                                        string endate = "";
                                        string startdate = "";
                                        if (dvsem.Count > 0)
                                        {
                                            startdate = dvsem[0]["start_date"].ToString();
                                            endate = dvsem[0]["end_date"].ToString();
                                            DateTime dtstart = Convert.ToDateTime(startdate);
                                            DateTime dtendate = Convert.ToDateTime(endate);
                                            if (dtstart <= input_date && dtendate >= input_date)
                                            {
                                                attnd_report.Sheets[0].RowCount++;
                                                inirowcnt = attnd_report.Sheets[0].RowCount - 1;

                                                // if (count == 0)
                                                {
                                                    attnd_report.Sheets[0].Cells[(attnd_report.Sheets[0].RowCount - 1), 1].Text = acronym;
                                                    attnd_report.Sheets[0].Cells[(attnd_report.Sheets[0].RowCount - 1), 2].HorizontalAlign = HorizontalAlign.Center;
                                                    attnd_report.Sheets[0].Cells[(attnd_report.Sheets[0].RowCount - 1), 2].VerticalAlign = VerticalAlign.Middle;
                                                }

                                                if (sections.ToString() != string.Empty)
                                                {
                                                    getsec = "-" + sections.ToString();
                                                }
                                                else
                                                {
                                                    getsec = "";
                                                }

                                                if (Convert.ToInt32(current_sem) % 2 == 0)
                                                {
                                                    roman_val = sem_roman(int.Parse(current_sem) / 2) + " Year" + getsec;
                                                    attnd_report.Sheets[0].SetText(attnd_report.Sheets[0].RowCount - 1, 2, roman_val);
                                                }
                                                else
                                                {
                                                    roman_val = sem_roman(((int.Parse(current_sem)) + 1) / 2) + " Year" + getsec;
                                                    attnd_report.Sheets[0].SetText(attnd_report.Sheets[0].RowCount - 1, 2, roman_val);

                                                }
                                                findhours();//----------function    
                                                attnd_report.Sheets[0].Cells[attnd_report.Sheets[0].RowCount - 1, 0].Text = row_hearder_count.ToString();
                                                row_hearder_count++;
                                            }
                                        }
                                        //}
                                    }

                                }
                                if (fflag == true)
                                {
                                    if (chkPeriod.Checked == false)
                                    {
                                        //                attnd_report.Sheets[0].RowCount = attnd_report.Sheets[0].RowCount + 2;
                                        //                attnd_report.Sheets[0].Cells[attnd_report.Sheets[0].RowCount - 2, 2].Font.Bold = true;
                                        //                attnd_report.Sheets[0].Cells[attnd_report.Sheets[0].RowCount - 1, 2].Font.Bold = true;
                                        //                attnd_report.Sheets[0].SetText(attnd_report.Sheets[0].RowCount - 2, 2, "Total");
                                        //                attnd_report.Sheets[0].SetText(attnd_report.Sheets[0].RowCount - 1, 2, "Percentage");
                                        //                //attnd_report.Sheets[0].SetText(attnd_report.Sheets[0].RowCount - 2, 1, "Total");
                                        //                //attnd_report.Sheets[0].SetText(attnd_report.Sheets[0].RowCount - 1, 1, "Percentage");


                                        //                //attnd_report.Sheets[0].Cells[attnd_report.Sheets[0].RowCount - 1, 0].Text = row_hearder_count.ToString();
                                        //                //attnd_report.Sheets[0].Cells[attnd_report.Sheets[0].RowCount - 2, 0].Text = row_hearder_count.ToString();
                                        //                attnd_report.Sheets[0].Cells[(attnd_report.Sheets[0].RowCount - 2), 2].HorizontalAlign = HorizontalAlign.Center;
                                        //                attnd_report.Sheets[0].Cells[(attnd_report.Sheets[0].RowCount - 2), 2].VerticalAlign = VerticalAlign.Middle;
                                        //                attnd_report.Sheets[0].Cells[(attnd_report.Sheets[0].RowCount - 1), 2].HorizontalAlign = HorizontalAlign.Center;
                                        //                attnd_report.Sheets[0].Cells[(attnd_report.Sheets[0].RowCount - 1), 2].VerticalAlign = VerticalAlign.Middle;

                                        //                attnd_report.Sheets[0].Cells[(attnd_report.Sheets[0].RowCount - 2), 3].Text = tot_strength.ToString();
                                        //                attnd_report.Sheets[0].Cells[(attnd_report.Sheets[0].RowCount - 2), 3].HorizontalAlign = HorizontalAlign.Center;
                                        fin_str += tot_strength;
                                        //                attnd_report.Sheets[0].Cells[(attnd_report.Sheets[0].RowCount - 2), 4].Text = temp_tot_pres.ToString();
                                        //                attnd_report.Sheets[0].Cells[(attnd_report.Sheets[0].RowCount - 2), 4].HorizontalAlign = HorizontalAlign.Center;
                                        //                fin_pres += temp_tot_pres;
                                        //                attnd_report.Sheets[0].Cells[(attnd_report.Sheets[0].RowCount - 2), 5].Text = tot_tot.ToString();
                                        //                attnd_report.Sheets[0].Cells[(attnd_report.Sheets[0].RowCount - 2), 5].HorizontalAlign = HorizontalAlign.Center;
                                        //                fin_tot += tot_tot;
                                        //                attnd_report.Sheets[0].Cells[(attnd_report.Sheets[0].RowCount - 2), 6].Text = temp_tot_lea.ToString();
                                        //                attnd_report.Sheets[0].Cells[(attnd_report.Sheets[0].RowCount - 2), 6].HorizontalAlign = HorizontalAlign.Center;
                                        //                fin_lev += temp_tot_lea;
                                        //                attnd_report.Sheets[0].Cells[(attnd_report.Sheets[0].RowCount - 2), 7].Text = temp_tot_abs.ToString();
                                        //                attnd_report.Sheets[0].Cells[(attnd_report.Sheets[0].RowCount - 2), 7].HorizontalAlign = HorizontalAlign.Center;
                                        //                fin_abs += temp_tot_abs;
                                        //                attnd_report.Sheets[0].Cells[(attnd_report.Sheets[0].RowCount - 2), 8].Text = temp_tot_sus.ToString();
                                        //                attnd_report.Sheets[0].Cells[(attnd_report.Sheets[0].RowCount - 2), 8].HorizontalAlign = HorizontalAlign.Center;
                                        //                fin_sus += temp_tot_sus;
                                        //                attnd_report.Sheets[0].Cells[(attnd_report.Sheets[0].RowCount - 2), 9].Text = temp_tot_od.ToString();
                                        //                attnd_report.Sheets[0].Cells[(attnd_report.Sheets[0].RowCount - 2), 9].HorizontalAlign = HorizontalAlign.Center;
                                        //                fin_od += temp_tot_od;
                                        //                attnd_report.Sheets[0].Cells[(attnd_report.Sheets[0].RowCount - 2), 10].Text = temp_tot_sod.ToString();
                                        //                attnd_report.Sheets[0].Cells[(attnd_report.Sheets[0].RowCount - 2), 10].HorizontalAlign = HorizontalAlign.Center;
                                        //                fin_sod += temp_tot_sod;


                                        //                attnd_report.Sheets[0].Cells[(attnd_report.Sheets[0].RowCount - 2), 4].Text = totmaorp.ToString();
                                        //                attnd_report.Sheets[0].Cells[(attnd_report.Sheets[0].RowCount - 2), 4].HorizontalAlign = HorizontalAlign.Center;
                                        gmp = gmp + totmaorp;
                                        //                Double precentvalue = totmaorp / tot_strength * 100;
                                        //                if (precentvalue.ToString() == "NaN")
                                        //                {
                                        //                    precentvalue = 0;
                                        //                }
                                        //                precentvalue = Math.Round(precentvalue, 2, MidpointRounding.AwayFromZero);
                                        //                attnd_report.Sheets[0].Cells[(attnd_report.Sheets[0].RowCount - 1), 4].Text = precentvalue.ToString();
                                        //                attnd_report.Sheets[0].Cells[(attnd_report.Sheets[0].RowCount - 1), 4].HorizontalAlign = HorizontalAlign.Center;

                                        //                attnd_report.Sheets[0].Cells[(attnd_report.Sheets[0].RowCount - 2), 5].Text = totevep.ToString();
                                        //                attnd_report.Sheets[0].Cells[(attnd_report.Sheets[0].RowCount - 2), 5].HorizontalAlign = HorizontalAlign.Center;
                                        gep = gep + totevep;
                                        //                precentvalue = totevep / tot_strength * 100;
                                        //                if (precentvalue.ToString() == "NaN")
                                        //                {
                                        //                    precentvalue = 0;
                                        //                }
                                        //                precentvalue = Math.Round(precentvalue, 2, MidpointRounding.AwayFromZero);
                                        //                attnd_report.Sheets[0].Cells[(attnd_report.Sheets[0].RowCount - 1), 5].Text = precentvalue.ToString();
                                        //                attnd_report.Sheets[0].Cells[(attnd_report.Sheets[0].RowCount - 1), 5].HorizontalAlign = HorizontalAlign.Center;

                                        //                attnd_report.Sheets[0].Cells[(attnd_report.Sheets[0].RowCount - 2), 6].Text = totmaorall.ToString();
                                        //                attnd_report.Sheets[0].Cells[(attnd_report.Sheets[0].RowCount - 2), 6].HorizontalAlign = HorizontalAlign.Center;
                                        gmal = gmal + totmaorall;
                                        //                precentvalue = totmaorall / tot_strength * 100;
                                        //                if (precentvalue.ToString() == "NaN")
                                        //                {
                                        //                    precentvalue = 0;
                                        //                }
                                        //                precentvalue = Math.Round(precentvalue, 2, MidpointRounding.AwayFromZero);
                                        //                attnd_report.Sheets[0].Cells[(attnd_report.Sheets[0].RowCount - 1), 6].Text = precentvalue.ToString();
                                        //                attnd_report.Sheets[0].Cells[(attnd_report.Sheets[0].RowCount - 1), 6].HorizontalAlign = HorizontalAlign.Center;

                                        //                attnd_report.Sheets[0].Cells[(attnd_report.Sheets[0].RowCount - 2), 7].Text = toteveall.ToString();
                                        //                attnd_report.Sheets[0].Cells[(attnd_report.Sheets[0].RowCount - 2), 7].HorizontalAlign = HorizontalAlign.Center;
                                        geall = geall + toteveall;
                                        //                precentvalue = toteveall / tot_strength * 100;
                                        //                if (precentvalue.ToString() == "NaN")
                                        //                {
                                        //                    precentvalue = 0;
                                        //                }
                                        //                precentvalue = Math.Round(precentvalue, 2, MidpointRounding.AwayFromZero);
                                        //                attnd_report.Sheets[0].Cells[(attnd_report.Sheets[0].RowCount - 1), 7].Text = precentvalue.ToString();
                                        //                attnd_report.Sheets[0].Cells[(attnd_report.Sheets[0].RowCount - 1), 7].HorizontalAlign = HorizontalAlign.Center;

                                        //                attnd_report.Sheets[0].Cells[(attnd_report.Sheets[0].RowCount - 2), 8].Text = totmaorl.ToString();
                                        //                attnd_report.Sheets[0].Cells[(attnd_report.Sheets[0].RowCount - 2), 8].HorizontalAlign = HorizontalAlign.Center;
                                        gml = gml + totmaorl;
                                        //                precentvalue = totmaorl / tot_strength * 100;
                                        //                if (precentvalue.ToString() == "NaN")
                                        //                {
                                        //                    precentvalue = 0;
                                        //                }
                                        //                precentvalue = Math.Round(precentvalue, 2, MidpointRounding.AwayFromZero);
                                        //                attnd_report.Sheets[0].Cells[(attnd_report.Sheets[0].RowCount - 1), 8].Text = precentvalue.ToString();
                                        //                attnd_report.Sheets[0].Cells[(attnd_report.Sheets[0].RowCount - 1), 8].HorizontalAlign = HorizontalAlign.Center;

                                        //                attnd_report.Sheets[0].Cells[(attnd_report.Sheets[0].RowCount - 2), 9].Text = totevel.ToString();
                                        //                attnd_report.Sheets[0].Cells[(attnd_report.Sheets[0].RowCount - 2), 9].HorizontalAlign = HorizontalAlign.Center;
                                        gel = gel + totevel;
                                        //                precentvalue = totevel / tot_strength * 100;
                                        //                if (precentvalue.ToString() == "NaN")
                                        //                {
                                        //                    precentvalue = 0;
                                        //                }
                                        //                precentvalue = Math.Round(precentvalue, 2, MidpointRounding.AwayFromZero);
                                        //                attnd_report.Sheets[0].Cells[(attnd_report.Sheets[0].RowCount - 1), 9].Text = precentvalue.ToString();
                                        //                attnd_report.Sheets[0].Cells[(attnd_report.Sheets[0].RowCount - 1), 9].HorizontalAlign = HorizontalAlign.Center;

                                        //                attnd_report.Sheets[0].Cells[(attnd_report.Sheets[0].RowCount - 2), 10].Text = totmaora.ToString();
                                        //                attnd_report.Sheets[0].Cells[(attnd_report.Sheets[0].RowCount - 2), 10].HorizontalAlign = HorizontalAlign.Center;
                                        gma = gma + totmaora;
                                        //                precentvalue = totmaora / tot_strength * 100;
                                        //                if (precentvalue.ToString() == "NaN")
                                        //                {
                                        //                    precentvalue = 0;
                                        //                }
                                        //                precentvalue = Math.Round(precentvalue, 2, MidpointRounding.AwayFromZero);
                                        //                attnd_report.Sheets[0].Cells[(attnd_report.Sheets[0].RowCount - 1), 10].Text = precentvalue.ToString();
                                        //                attnd_report.Sheets[0].Cells[(attnd_report.Sheets[0].RowCount - 1), 10].HorizontalAlign = HorizontalAlign.Center;

                                        //                attnd_report.Sheets[0].Cells[(attnd_report.Sheets[0].RowCount - 2), 11].Text = totevea.ToString();
                                        //                attnd_report.Sheets[0].Cells[(attnd_report.Sheets[0].RowCount - 2), 11].HorizontalAlign = HorizontalAlign.Center;
                                        gea = gea + totevea;
                                        //                precentvalue = totevea / tot_strength * 100;
                                        //                if (precentvalue.ToString() == "NaN")
                                        //                {
                                        //                    precentvalue = 0;
                                        //                }
                                        //                precentvalue = Math.Round(precentvalue, 2, MidpointRounding.AwayFromZero);
                                        //                attnd_report.Sheets[0].Cells[(attnd_report.Sheets[0].RowCount - 1), 11].Text = precentvalue.ToString();
                                        //                attnd_report.Sheets[0].Cells[(attnd_report.Sheets[0].RowCount - 1), 11].HorizontalAlign = HorizontalAlign.Center;

                                        //                attnd_report.Sheets[0].Cells[(attnd_report.Sheets[0].RowCount - 2), 12].Text = totmaors.ToString();
                                        //                attnd_report.Sheets[0].Cells[(attnd_report.Sheets[0].RowCount - 2), 12].HorizontalAlign = HorizontalAlign.Center;
                                        gms = gms + totmaors;
                                        //                precentvalue = totmaors / tot_strength * 100;
                                        //                if (precentvalue.ToString() == "NaN")
                                        //                {
                                        //                    precentvalue = 0;
                                        //                }
                                        //                precentvalue = Math.Round(precentvalue, 2, MidpointRounding.AwayFromZero);
                                        //                attnd_report.Sheets[0].Cells[(attnd_report.Sheets[0].RowCount - 1), 12].Text = precentvalue.ToString();
                                        //                attnd_report.Sheets[0].Cells[(attnd_report.Sheets[0].RowCount - 1), 12].HorizontalAlign = HorizontalAlign.Center;

                                        //                attnd_report.Sheets[0].Cells[(attnd_report.Sheets[0].RowCount - 2), 13].Text = toteves.ToString();
                                        //                attnd_report.Sheets[0].Cells[(attnd_report.Sheets[0].RowCount - 2), 13].HorizontalAlign = HorizontalAlign.Center;
                                        ges = ges + toteves;
                                        //                precentvalue = toteves / tot_strength * 100;
                                        //                if (precentvalue.ToString() == "NaN")
                                        //                {
                                        //                    precentvalue = 0;
                                        //                }
                                        //                precentvalue = Math.Round(precentvalue, 2, MidpointRounding.AwayFromZero);
                                        //                attnd_report.Sheets[0].Cells[(attnd_report.Sheets[0].RowCount - 1), 13].Text = precentvalue.ToString();
                                        //                attnd_report.Sheets[0].Cells[(attnd_report.Sheets[0].RowCount - 1), 13].HorizontalAlign = HorizontalAlign.Center;

                                        //                attnd_report.Sheets[0].Cells[(attnd_report.Sheets[0].RowCount - 2), 14].Text = totmaorod.ToString();
                                        //                attnd_report.Sheets[0].Cells[(attnd_report.Sheets[0].RowCount - 2), 14].HorizontalAlign = HorizontalAlign.Center;
                                        gmod = gmod + totmaorod;
                                        //                precentvalue = totmaorod / tot_strength * 100;
                                        //                if (precentvalue.ToString() == "NaN")
                                        //                {
                                        //                    precentvalue = 0;
                                        //                }
                                        //                precentvalue = Math.Round(precentvalue, 2, MidpointRounding.AwayFromZero);
                                        //                attnd_report.Sheets[0].Cells[(attnd_report.Sheets[0].RowCount - 1), 14].Text = precentvalue.ToString();
                                        //                attnd_report.Sheets[0].Cells[(attnd_report.Sheets[0].RowCount - 1), 14].HorizontalAlign = HorizontalAlign.Center;

                                        //                attnd_report.Sheets[0].Cells[(attnd_report.Sheets[0].RowCount - 2), 15].Text = toteveod.ToString();
                                        //                attnd_report.Sheets[0].Cells[(attnd_report.Sheets[0].RowCount - 2), 15].HorizontalAlign = HorizontalAlign.Center;
                                        geod = geod + toteveod;
                                        //                precentvalue = toteveod / tot_strength * 100;
                                        //                if (precentvalue.ToString() == "NaN")
                                        //                {
                                        //                    precentvalue = 0;
                                        //                }
                                        //                precentvalue = Math.Round(precentvalue, 2, MidpointRounding.AwayFromZero);
                                        //                attnd_report.Sheets[0].Cells[(attnd_report.Sheets[0].RowCount - 1), 15].Text = precentvalue.ToString();
                                        //                attnd_report.Sheets[0].Cells[(attnd_report.Sheets[0].RowCount - 1), 15].HorizontalAlign = HorizontalAlign.Center;

                                        //                attnd_report.Sheets[0].Cells[(attnd_report.Sheets[0].RowCount - 2), 16].Text = totmaorsod.ToString();
                                        //                attnd_report.Sheets[0].Cells[(attnd_report.Sheets[0].RowCount - 2), 16].HorizontalAlign = HorizontalAlign.Center;
                                        //                precentvalue = totmaorsod / tot_strength * 100;
                                        gmsod = gmsod + totmaorsod;
                                        //                if (precentvalue.ToString() == "NaN")
                                        //                {
                                        //                    precentvalue = 0;
                                        //                }
                                        //                precentvalue = Math.Round(precentvalue, 2, MidpointRounding.AwayFromZero);
                                        //                attnd_report.Sheets[0].Cells[(attnd_report.Sheets[0].RowCount - 1), 16].Text = precentvalue.ToString();
                                        //                attnd_report.Sheets[0].Cells[(attnd_report.Sheets[0].RowCount - 1), 16].HorizontalAlign = HorizontalAlign.Center;

                                        //                attnd_report.Sheets[0].Cells[(attnd_report.Sheets[0].RowCount - 2), 17].Text = totevesod.ToString();
                                        //                attnd_report.Sheets[0].Cells[(attnd_report.Sheets[0].RowCount - 2), 17].HorizontalAlign = HorizontalAlign.Center; precentvalue = totmaorsod / tot_strength * 100;
                                        //                precentvalue = totevesod / tot_strength * 100;
                                        gesod = gesod + totevesod;
                                        //                if (precentvalue.ToString() == "NaN")
                                        //                {
                                        //                    precentvalue = 0;
                                        //                }
                                        //                precentvalue = Math.Round(precentvalue, 2, MidpointRounding.AwayFromZero);
                                        //                attnd_report.Sheets[0].Cells[(attnd_report.Sheets[0].RowCount - 1), 17].Text = precentvalue.ToString();
                                        //                attnd_report.Sheets[0].Cells[(attnd_report.Sheets[0].RowCount - 1), 17].HorizontalAlign = HorizontalAlign.Center;

                                        //                totmaorp = 0; totevep = 0;
                                        //                totmaora = 0; totevea = 0;
                                        //                totmaorl = 0; totevel = 0;
                                        //                totmaors = 0; toteves = 0;
                                        //                totmaorod = 0; toteveod = 0;
                                        //                totmaorsod = 0; totevesod = 0;
                                        //                totmaorall = 0; toteveall = 0;


                                    }
                                    else
                                    {
                                        //                attnd_report.Sheets[0].RowCount = attnd_report.Sheets[0].RowCount + 2;
                                        //                attnd_report.Sheets[0].Cells[attnd_report.Sheets[0].RowCount - 2, 2].Font.Bold = true;
                                        //                attnd_report.Sheets[0].Cells[attnd_report.Sheets[0].RowCount - 1, 2].Font.Bold = true;
                                        //                attnd_report.Sheets[0].SetText(attnd_report.Sheets[0].RowCount - 2, 2, "Total");
                                        //                attnd_report.Sheets[0].SetText(attnd_report.Sheets[0].RowCount - 1, 2, "Percentage");
                                        //                //attnd_report.Sheets[0].SetText(attnd_report.Sheets[0].RowCount - 2, 1, "Total");
                                        //                //attnd_report.Sheets[0].SetText(attnd_report.Sheets[0].RowCount - 1, 1, "Percentage");


                                        //                //attnd_report.Sheets[0].Cells[attnd_report.Sheets[0].RowCount - 1, 0].Text = row_hearder_count.ToString();
                                        //                //attnd_report.Sheets[0].Cells[attnd_report.Sheets[0].RowCount - 2, 0].Text = row_hearder_count.ToString();

                                        //                attnd_report.Sheets[0].Cells[(attnd_report.Sheets[0].RowCount - 2), 1].Text = acronym;
                                        //                attnd_report.Sheets[0].Cells[(attnd_report.Sheets[0].RowCount - 1), 1].Text = acronym;
                                        //                attnd_report.Sheets[0].Cells[(attnd_report.Sheets[0].RowCount - 2), 2].HorizontalAlign = HorizontalAlign.Center;
                                        //                attnd_report.Sheets[0].Cells[(attnd_report.Sheets[0].RowCount - 2), 2].VerticalAlign = VerticalAlign.Middle;
                                        //                attnd_report.Sheets[0].Cells[(attnd_report.Sheets[0].RowCount - 1), 2].HorizontalAlign = HorizontalAlign.Center;
                                        //                attnd_report.Sheets[0].Cells[(attnd_report.Sheets[0].RowCount - 1), 2].VerticalAlign = VerticalAlign.Middle;

                                        //                attnd_report.Sheets[0].Cells[(attnd_report.Sheets[0].RowCount - 2), 3].Text = tot_strength.ToString();
                                        //                attnd_report.Sheets[0].Cells[(attnd_report.Sheets[0].RowCount - 2), 3].HorizontalAlign = HorizontalAlign.Center;
                                        fin_str += tot_strength;
                                        //                attnd_report.Sheets[0].Cells[(attnd_report.Sheets[0].RowCount - 2), 4].Text = temp_tot_pres.ToString();
                                        //                attnd_report.Sheets[0].Cells[(attnd_report.Sheets[0].RowCount - 2), 4].HorizontalAlign = HorizontalAlign.Center;
                                        fin_pres += temp_tot_pres;
                                        //                attnd_report.Sheets[0].Cells[(attnd_report.Sheets[0].RowCount - 2), 5].Text = tot_tot.ToString();
                                        //                attnd_report.Sheets[0].Cells[(attnd_report.Sheets[0].RowCount - 2), 5].HorizontalAlign = HorizontalAlign.Center;
                                        fin_tot += tot_tot;
                                        //                attnd_report.Sheets[0].Cells[(attnd_report.Sheets[0].RowCount - 2), 6].Text = temp_tot_lea.ToString();
                                        //                attnd_report.Sheets[0].Cells[(attnd_report.Sheets[0].RowCount - 2), 6].HorizontalAlign = HorizontalAlign.Center;
                                        fin_lev += temp_tot_lea;
                                        //                attnd_report.Sheets[0].Cells[(attnd_report.Sheets[0].RowCount - 2), 7].Text = temp_tot_abs.ToString();
                                        //                attnd_report.Sheets[0].Cells[(attnd_report.Sheets[0].RowCount - 2), 7].HorizontalAlign = HorizontalAlign.Center;
                                        fin_abs += temp_tot_abs;
                                        //                attnd_report.Sheets[0].Cells[(attnd_report.Sheets[0].RowCount - 2), 8].Text = temp_tot_sus.ToString();
                                        //                attnd_report.Sheets[0].Cells[(attnd_report.Sheets[0].RowCount - 2), 8].HorizontalAlign = HorizontalAlign.Center;
                                        fin_sus += temp_tot_sus;
                                        //                attnd_report.Sheets[0].Cells[(attnd_report.Sheets[0].RowCount - 2), 9].Text = temp_tot_od.ToString();
                                        //                attnd_report.Sheets[0].Cells[(attnd_report.Sheets[0].RowCount - 2), 9].HorizontalAlign = HorizontalAlign.Center;
                                        fin_od += temp_tot_od;
                                        //                attnd_report.Sheets[0].Cells[(attnd_report.Sheets[0].RowCount - 2), 10].Text = temp_tot_sod.ToString();
                                        //                attnd_report.Sheets[0].Cells[(attnd_report.Sheets[0].RowCount - 2), 10].HorizontalAlign = HorizontalAlign.Center;
                                        fin_sod += temp_tot_sod;
                                        //                //---------------------percentage
                                        //                double temp = 0;
                                        //                temp = double.Parse((((temp_tot_lea + temp_tot_abs) / tot_strength) * 100).ToString());
                                        //                if (temp.ToString() == "NaN")
                                        //                {
                                        //                    temp = 0;
                                        //                }
                                        //                over_all_per += temp;
                                        //                //attnd_report.Sheets[0].Cells[(attnd_report.Sheets[0].RowCount - 1), 3].Text = String.Format("{0:0.00}", temp);
                                        //                attnd_report.Sheets[0].Cells[(attnd_report.Sheets[0].RowCount - 1), 3].HorizontalAlign = HorizontalAlign.Center;
                                        //                grandsritotal = grandsritotal + 100;

                                        //                temp = double.Parse(((temp_tot_pres * 100) / tot_strength).ToString());
                                        //                if (temp.ToString() == "NaN")
                                        //                {
                                        //                    temp = 0;
                                        //                }
                                        //                attnd_report.Sheets[0].Cells[(attnd_report.Sheets[0].RowCount - 1), 4].Text = String.Format("{0:0.00}", temp);
                                        //                attnd_report.Sheets[0].Cells[(attnd_report.Sheets[0].RowCount - 1), 4].HorizontalAlign = HorizontalAlign.Center;


                                        //                temp = double.Parse(((tot_tot * 100) / tot_strength).ToString());
                                        //                if (temp.ToString() == "NaN")
                                        //                {
                                        //                    temp = 0;
                                        //                }

                                        //                attnd_report.Sheets[0].Cells[(attnd_report.Sheets[0].RowCount - 1), 5].Text = String.Format("{0:0.00}", temp);
                                        //                attnd_report.Sheets[0].Cells[(attnd_report.Sheets[0].RowCount - 1), 5].HorizontalAlign = HorizontalAlign.Center;
                                        //                temp = double.Parse(((temp_tot_lea * 100) / tot_strength).ToString());


                                        //                if (temp.ToString() == "NaN")
                                        //                {
                                        //                    temp = 0;
                                        //                }

                                        //                attnd_report.Sheets[0].Cells[(attnd_report.Sheets[0].RowCount - 1), 6].Text = String.Format("{0:0.00}", temp);
                                        //                attnd_report.Sheets[0].Cells[(attnd_report.Sheets[0].RowCount - 1), 6].HorizontalAlign = HorizontalAlign.Center;
                                        //                temp = double.Parse(((temp_tot_abs * 100) / tot_strength).ToString());

                                        //                if (temp.ToString() == "NaN")
                                        //                {
                                        //                    temp = 0;
                                        //                }

                                        //                attnd_report.Sheets[0].Cells[(attnd_report.Sheets[0].RowCount - 1), 7].Text = String.Format("{0:0.00}", temp);
                                        //                attnd_report.Sheets[0].Cells[(attnd_report.Sheets[0].RowCount - 1), 7].HorizontalAlign = HorizontalAlign.Center;
                                        //                temp = double.Parse(((temp_tot_sus * 100) / tot_strength).ToString());

                                        //                if (temp.ToString() == "NaN")
                                        //                {
                                        //                    temp = 0;
                                        //                }

                                        //                attnd_report.Sheets[0].Cells[(attnd_report.Sheets[0].RowCount - 1), 8].Text = String.Format("{0:0.00}", temp);
                                        //                attnd_report.Sheets[0].Cells[(attnd_report.Sheets[0].RowCount - 1), 8].HorizontalAlign = HorizontalAlign.Center;
                                        //                temp = double.Parse(((temp_tot_od * 100) / tot_strength).ToString());

                                        //                if (temp.ToString() == "NaN")
                                        //                {
                                        //                    temp = 0;
                                        //                }

                                        //                attnd_report.Sheets[0].Cells[(attnd_report.Sheets[0].RowCount - 1), 9].Text = String.Format("{0:0.00}", temp);
                                        //                attnd_report.Sheets[0].Cells[(attnd_report.Sheets[0].RowCount - 1), 9].HorizontalAlign = HorizontalAlign.Center;
                                        //                temp = double.Parse(((temp_tot_sod * 100) / tot_strength).ToString());

                                        //                if (temp.ToString() == "NaN")
                                        //                {
                                        //                    temp = 0;
                                        //                }

                                        //                attnd_report.Sheets[0].Cells[(attnd_report.Sheets[0].RowCount - 1), 10].Text = String.Format("{0:0.00}", temp);
                                        //                attnd_report.Sheets[0].Cells[(attnd_report.Sheets[0].RowCount - 1), 10].HorizontalAlign = HorizontalAlign.Center;

                                    }

                                    //            temp_tot = 0;
                                    //            temp_tot_pres = 0;
                                    //            temp_tot_lea = 0;
                                    //            temp_tot_abs = 0;
                                    //            temp_tot_sus = 0;
                                    //            temp_tot_od = 0;
                                    //            temp_tot_sod = 0;
                                    //            tot_strength = 0;
                                    //            tot_tot = 0;
                                    attnd_report.Sheets[0].RowCount++;
                                    attnd_report.Sheets[0].SpanModel.Add((attnd_report.Sheets[0].RowCount - 1), 0, 1, attnd_report.Sheets[0].ColumnCount);
                                }
                            }
                        }
                    }


                    if (chkPeriod.Checked == false)
                    {
                        attnd_report.Sheets[0].RowCount = attnd_report.Sheets[0].RowCount + 2;
                        //attnd_report.Sheets[0].SpanModel.Add((attnd_report.Sheets[0].RowCount - 3), 1, 1, 18);
                        attnd_report.Sheets[0].SetText(attnd_report.Sheets[0].RowCount - 2, 0, "Grand Total");
                        attnd_report.Sheets[0].SetText(attnd_report.Sheets[0].RowCount - 1, 0, "Total Percentage");
                        attnd_report.Sheets[0].SpanModel.Add((attnd_report.Sheets[0].RowCount - 1), 0, 1, 3);
                        attnd_report.Sheets[0].SpanModel.Add((attnd_report.Sheets[0].RowCount - 2), 0, 1, 3);
                        attnd_report.Sheets[0].Rows[attnd_report.Sheets[0].RowCount - 2].Font.Bold = true;
                        attnd_report.Sheets[0].Rows[attnd_report.Sheets[0].RowCount - 1].Font.Bold = true;
                        attnd_report.Sheets[0].Cells[(attnd_report.Sheets[0].RowCount - 2), 3].Text = (fin_str).ToString();
                        attnd_report.Sheets[0].Cells[(attnd_report.Sheets[0].RowCount - 2), 4].Text = gmp.ToString();
                        attnd_report.Sheets[0].Cells[(attnd_report.Sheets[0].RowCount - 2), 5].Text = gep.ToString();
                        attnd_report.Sheets[0].Cells[(attnd_report.Sheets[0].RowCount - 2), 6].Text = gmal.ToString();
                        attnd_report.Sheets[0].Cells[(attnd_report.Sheets[0].RowCount - 2), 7].Text = geall.ToString();
                        attnd_report.Sheets[0].Cells[(attnd_report.Sheets[0].RowCount - 2), 8].Text = gml.ToString();
                        attnd_report.Sheets[0].Cells[(attnd_report.Sheets[0].RowCount - 2), 9].Text = gel.ToString();
                        attnd_report.Sheets[0].Cells[(attnd_report.Sheets[0].RowCount - 2), 10].Text = gma.ToString();
                        attnd_report.Sheets[0].Cells[(attnd_report.Sheets[0].RowCount - 2), 11].Text = gea.ToString();
                        attnd_report.Sheets[0].Cells[(attnd_report.Sheets[0].RowCount - 2), 12].Text = gms.ToString();
                        attnd_report.Sheets[0].Cells[(attnd_report.Sheets[0].RowCount - 2), 13].Text = ges.ToString();
                        attnd_report.Sheets[0].Cells[(attnd_report.Sheets[0].RowCount - 2), 14].Text = gmod.ToString();
                        attnd_report.Sheets[0].Cells[(attnd_report.Sheets[0].RowCount - 2), 15].Text = geod.ToString();
                        attnd_report.Sheets[0].Cells[(attnd_report.Sheets[0].RowCount - 2), 16].Text = gmsod.ToString();
                        attnd_report.Sheets[0].Cells[(attnd_report.Sheets[0].RowCount - 2), 17].Text = gesod.ToString();

                        attnd_report.Sheets[0].Rows[attnd_report.Sheets[0].RowCount - 2].Font.Bold = true;
                        attnd_report.Sheets[0].Rows[attnd_report.Sheets[0].RowCount - 1].Font.Bold = true;
                        attnd_report.Sheets[0].Rows[attnd_report.Sheets[0].RowCount - 1].Font.Size = FontUnit.Medium;
                        attnd_report.Sheets[0].Rows[attnd_report.Sheets[0].RowCount - 2].Font.Size = FontUnit.Medium;

                        attnd_report.Sheets[0].Cells[(attnd_report.Sheets[0].RowCount - 2), 3].HorizontalAlign = HorizontalAlign.Center;
                        attnd_report.Sheets[0].Cells[(attnd_report.Sheets[0].RowCount - 2), 4].HorizontalAlign = HorizontalAlign.Center;
                        attnd_report.Sheets[0].Cells[(attnd_report.Sheets[0].RowCount - 2), 5].HorizontalAlign = HorizontalAlign.Center;
                        attnd_report.Sheets[0].Cells[(attnd_report.Sheets[0].RowCount - 2), 6].HorizontalAlign = HorizontalAlign.Center;
                        attnd_report.Sheets[0].Cells[(attnd_report.Sheets[0].RowCount - 2), 7].HorizontalAlign = HorizontalAlign.Center;
                        attnd_report.Sheets[0].Cells[(attnd_report.Sheets[0].RowCount - 2), 8].HorizontalAlign = HorizontalAlign.Center;
                        attnd_report.Sheets[0].Cells[(attnd_report.Sheets[0].RowCount - 2), 9].HorizontalAlign = HorizontalAlign.Center;
                        attnd_report.Sheets[0].Cells[(attnd_report.Sheets[0].RowCount - 2), 10].HorizontalAlign = HorizontalAlign.Center;
                        attnd_report.Sheets[0].Cells[(attnd_report.Sheets[0].RowCount - 2), 3].HorizontalAlign = HorizontalAlign.Center;
                        attnd_report.Sheets[0].Cells[(attnd_report.Sheets[0].RowCount - 2), 11].HorizontalAlign = HorizontalAlign.Center;
                        attnd_report.Sheets[0].Cells[(attnd_report.Sheets[0].RowCount - 2), 12].HorizontalAlign = HorizontalAlign.Center;
                        attnd_report.Sheets[0].Cells[(attnd_report.Sheets[0].RowCount - 2), 13].HorizontalAlign = HorizontalAlign.Center;
                        attnd_report.Sheets[0].Cells[(attnd_report.Sheets[0].RowCount - 2), 14].HorizontalAlign = HorizontalAlign.Center;
                        attnd_report.Sheets[0].Cells[(attnd_report.Sheets[0].RowCount - 2), 15].HorizontalAlign = HorizontalAlign.Center;
                        attnd_report.Sheets[0].Cells[(attnd_report.Sheets[0].RowCount - 2), 16].HorizontalAlign = HorizontalAlign.Center;
                        attnd_report.Sheets[0].Cells[(attnd_report.Sheets[0].RowCount - 2), 17].HorizontalAlign = HorizontalAlign.Center;

                        Double gperval = 0;
                        gperval = gmp / fin_str * 100;
                        if (gperval.ToString() == "NaN")
                        {
                            gperval = 0;
                        }
                        gperval = Math.Round(gperval, 2, MidpointRounding.AwayFromZero);
                        attnd_report.Sheets[0].Cells[(attnd_report.Sheets[0].RowCount - 1), 4].Text = gperval.ToString();

                        gperval = gep / fin_str * 100;
                        if (gperval.ToString() == "NaN")
                        {
                            gperval = 0;
                        }
                        gperval = Math.Round(gperval, 2, MidpointRounding.AwayFromZero);
                        attnd_report.Sheets[0].Cells[(attnd_report.Sheets[0].RowCount - 1), 5].Text = gperval.ToString();

                        gperval = gmal / fin_str * 100;
                        if (gperval.ToString() == "NaN")
                        {
                            gperval = 0;
                        }
                        gperval = Math.Round(gperval, 2, MidpointRounding.AwayFromZero);
                        attnd_report.Sheets[0].Cells[(attnd_report.Sheets[0].RowCount - 1), 6].Text = gperval.ToString();

                        gperval = geall / fin_str * 100;
                        if (gperval.ToString() == "NaN")
                        {
                            gperval = 0;
                        }
                        gperval = Math.Round(gperval, 2, MidpointRounding.AwayFromZero);
                        attnd_report.Sheets[0].Cells[(attnd_report.Sheets[0].RowCount - 1), 7].Text = gperval.ToString();

                        gperval = gml / fin_str * 100;
                        if (gperval.ToString() == "NaN")
                        {
                            gperval = 0;
                        }
                        gperval = Math.Round(gperval, 2, MidpointRounding.AwayFromZero);
                        attnd_report.Sheets[0].Cells[(attnd_report.Sheets[0].RowCount - 1), 8].Text = gperval.ToString();

                        gperval = gel / fin_str * 100;
                        if (gperval.ToString() == "NaN")
                        {
                            gperval = 0;
                        }
                        gperval = Math.Round(gperval, 2, MidpointRounding.AwayFromZero);
                        attnd_report.Sheets[0].Cells[(attnd_report.Sheets[0].RowCount - 1), 9].Text = gperval.ToString();

                        gperval = gma / fin_str * 100;
                        if (gperval.ToString() == "NaN")
                        {
                            gperval = 0;
                        }
                        gperval = Math.Round(gperval, 2, MidpointRounding.AwayFromZero);
                        attnd_report.Sheets[0].Cells[(attnd_report.Sheets[0].RowCount - 1), 10].Text = gperval.ToString();

                        gperval = gea / fin_str * 100;
                        if (gperval.ToString() == "NaN")
                        {
                            gperval = 0;
                        }
                        gperval = Math.Round(gperval, 2, MidpointRounding.AwayFromZero);
                        attnd_report.Sheets[0].Cells[(attnd_report.Sheets[0].RowCount - 1), 11].Text = gperval.ToString();

                        gperval = gms / fin_str * 100;
                        if (gperval.ToString() == "NaN")
                        {
                            gperval = 0;
                        }
                        gperval = Math.Round(gperval, 2, MidpointRounding.AwayFromZero);
                        attnd_report.Sheets[0].Cells[(attnd_report.Sheets[0].RowCount - 1), 12].Text = gperval.ToString();

                        gperval = ges / fin_str * 100;
                        if (gperval.ToString() == "NaN")
                        {
                            gperval = 0;
                        }
                        gperval = Math.Round(gperval, 2, MidpointRounding.AwayFromZero);
                        attnd_report.Sheets[0].Cells[(attnd_report.Sheets[0].RowCount - 1), 13].Text = gperval.ToString();

                        gperval = gmod / fin_str * 100;
                        if (gperval.ToString() == "NaN")
                        {
                            gperval = 0;
                        }
                        gperval = Math.Round(gperval, 2, MidpointRounding.AwayFromZero);
                        attnd_report.Sheets[0].Cells[(attnd_report.Sheets[0].RowCount - 1), 14].Text = gperval.ToString();

                        gperval = geod / fin_str * 100;
                        if (gperval.ToString() == "NaN")
                        {
                            gperval = 0;
                        }
                        gperval = Math.Round(gperval, 2, MidpointRounding.AwayFromZero);
                        attnd_report.Sheets[0].Cells[(attnd_report.Sheets[0].RowCount - 1), 15].Text = gperval.ToString();

                        gperval = gmsod / fin_str * 100;
                        if (gperval.ToString() == "NaN")
                        {
                            gperval = 0;
                        }
                        gperval = Math.Round(gperval, 2, MidpointRounding.AwayFromZero);
                        attnd_report.Sheets[0].Cells[(attnd_report.Sheets[0].RowCount - 1), 16].Text = gperval.ToString();

                        gperval = gesod / fin_str * 100;
                        if (gperval.ToString() == "NaN")
                        {
                            gperval = 0;
                        }
                        gperval = Math.Round(gperval, 2, MidpointRounding.AwayFromZero);
                        attnd_report.Sheets[0].Cells[(attnd_report.Sheets[0].RowCount - 1), 17].Text = gperval.ToString();


                        //attnd_report.Sheets[0].Cells[(attnd_report.Sheets[0].RowCount - 1), 0].Text = " ";
                        //attnd_report.Sheets[0].Cells[(attnd_report.Sheets[0].RowCount - 2), 0].Text = " ";
                        //attnd_report.Sheets[0].Cells[(attnd_report.Sheets[0].RowCount - 3), 0].Text = " ";
                        //attnd_report.Sheets[0].SpanModel.Add((attnd_report.Sheets[0].RowCount - 1), 3, 1, 9);
                    }
                    else
                    {
                        attnd_report.Sheets[0].RowCount = attnd_report.Sheets[0].RowCount + 2;
                        attnd_report.Sheets[0].SetText(attnd_report.Sheets[0].RowCount - 2, 0, "Grand Total");
                        attnd_report.Sheets[0].SetText(attnd_report.Sheets[0].RowCount - 1, 0, "Total Percentage");
                        attnd_report.Sheets[0].SpanModel.Add((attnd_report.Sheets[0].RowCount - 1), 0, 1, 3);
                        attnd_report.Sheets[0].SpanModel.Add((attnd_report.Sheets[0].RowCount - 2), 0, 1, 3);
                        // attnd_report.Sheets[0].SpanModel.Add((attnd_report.Sheets[0].RowCount - 3), 1, 1, 11);
                        attnd_report.Sheets[0].Rows[attnd_report.Sheets[0].RowCount - 2].Font.Bold = true;
                        attnd_report.Sheets[0].Rows[attnd_report.Sheets[0].RowCount - 1].Font.Bold = true;
                        fin_str = fin_pres + fin_tot;
                        attnd_report.Sheets[0].Cells[(attnd_report.Sheets[0].RowCount - 2), 3].Text = (fin_str).ToString();
                        attnd_report.Sheets[0].SpanModel.Add((attnd_report.Sheets[0].RowCount - 2), 3, 2, 1);
                        attnd_report.Sheets[0].Cells[(attnd_report.Sheets[0].RowCount - 2), 4].Text = (fin_pres).ToString();
                        attnd_report.Sheets[0].Cells[(attnd_report.Sheets[0].RowCount - 2), 5].Text = (fin_tot).ToString();
                        attnd_report.Sheets[0].Cells[(attnd_report.Sheets[0].RowCount - 2), 6].Text = (fin_lev).ToString();
                        attnd_report.Sheets[0].Cells[(attnd_report.Sheets[0].RowCount - 2), 7].Text = (fin_abs).ToString();
                        attnd_report.Sheets[0].Cells[(attnd_report.Sheets[0].RowCount - 2), 8].Text = (fin_sus).ToString();
                        attnd_report.Sheets[0].Cells[(attnd_report.Sheets[0].RowCount - 2), 9].Text = (fin_od).ToString();
                        attnd_report.Sheets[0].Cells[(attnd_report.Sheets[0].RowCount - 2), 10].Text = (fin_sod).ToString();

                        attnd_report.Sheets[0].Cells[(attnd_report.Sheets[0].RowCount - 2), 3].HorizontalAlign = HorizontalAlign.Center;
                        attnd_report.Sheets[0].Cells[(attnd_report.Sheets[0].RowCount - 2), 4].HorizontalAlign = HorizontalAlign.Center;
                        attnd_report.Sheets[0].Cells[(attnd_report.Sheets[0].RowCount - 2), 5].HorizontalAlign = HorizontalAlign.Center;
                        attnd_report.Sheets[0].Cells[(attnd_report.Sheets[0].RowCount - 2), 6].HorizontalAlign = HorizontalAlign.Center;
                        attnd_report.Sheets[0].Cells[(attnd_report.Sheets[0].RowCount - 2), 7].HorizontalAlign = HorizontalAlign.Center;
                        attnd_report.Sheets[0].Cells[(attnd_report.Sheets[0].RowCount - 2), 8].HorizontalAlign = HorizontalAlign.Center;
                        attnd_report.Sheets[0].Cells[(attnd_report.Sheets[0].RowCount - 2), 9].HorizontalAlign = HorizontalAlign.Center;
                        attnd_report.Sheets[0].Cells[(attnd_report.Sheets[0].RowCount - 2), 10].HorizontalAlign = HorizontalAlign.Center;

                        attnd_report.Sheets[0].Rows[attnd_report.Sheets[0].RowCount - 2].Font.Bold = true;
                        attnd_report.Sheets[0].Rows[attnd_report.Sheets[0].RowCount - 1].Font.Bold = true;
                        attnd_report.Sheets[0].Rows[attnd_report.Sheets[0].RowCount - 1].Font.Size = FontUnit.Medium;
                        attnd_report.Sheets[0].Rows[attnd_report.Sheets[0].RowCount - 2].Font.Size = FontUnit.Medium;

                        over_all_per = (fin_pres / fin_str) * 100;
                        if (over_all_per.ToString() == "NaN")
                        {
                            over_all_per = 0;
                        }
                        over_all_per = Math.Round(over_all_per, 2, MidpointRounding.AwayFromZero);
                        attnd_report.Sheets[0].Cells[(attnd_report.Sheets[0].RowCount - 1), 4].Text = String.Format("{0:0.00}", over_all_per);

                        over_all_per = (fin_tot / fin_str) * 100;
                        if (over_all_per.ToString() == "NaN")
                        {
                            over_all_per = 0;
                        }
                        over_all_per = Math.Round(over_all_per, 2, MidpointRounding.AwayFromZero);
                        attnd_report.Sheets[0].Cells[(attnd_report.Sheets[0].RowCount - 1), 5].Text = String.Format("{0:0.00}", over_all_per);


                        over_all_per = (fin_lev / fin_str) * 100;
                        if (over_all_per.ToString() == "NaN")
                        {
                            over_all_per = 0;
                        }
                        over_all_per = Math.Round(over_all_per, 2, MidpointRounding.AwayFromZero);
                        attnd_report.Sheets[0].Cells[(attnd_report.Sheets[0].RowCount - 1), 6].Text = String.Format("{0:0.00}", over_all_per);


                        over_all_per = (fin_abs / fin_str) * 100;
                        if (over_all_per.ToString() == "NaN")
                        {
                            over_all_per = 0;
                        }
                        over_all_per = Math.Round(over_all_per, 2, MidpointRounding.AwayFromZero);
                        attnd_report.Sheets[0].Cells[(attnd_report.Sheets[0].RowCount - 1), 7].Text = String.Format("{0:0.00}", over_all_per);


                        over_all_per = (fin_sus / fin_str) * 100;
                        if (over_all_per.ToString() == "NaN")
                        {
                            over_all_per = 0;
                        }
                        over_all_per = Math.Round(over_all_per, 2, MidpointRounding.AwayFromZero);
                        attnd_report.Sheets[0].Cells[(attnd_report.Sheets[0].RowCount - 1), 8].Text = String.Format("{0:0.00}", over_all_per);


                        over_all_per = (fin_od / fin_str) * 100;
                        if (over_all_per.ToString() == "NaN")
                        {
                            over_all_per = 0;
                        }
                        over_all_per = Math.Round(over_all_per, 2, MidpointRounding.AwayFromZero);
                        attnd_report.Sheets[0].Cells[(attnd_report.Sheets[0].RowCount - 1), 9].Text = String.Format("{0:0.00}", over_all_per);

                        over_all_per = (fin_sod / fin_str) * 100;
                        if (over_all_per.ToString() == "NaN")
                        {
                            over_all_per = 0;
                        }
                        over_all_per = Math.Round(over_all_per, 2, MidpointRounding.AwayFromZero);
                        attnd_report.Sheets[0].Cells[(attnd_report.Sheets[0].RowCount - 1), 10].Text = String.Format("{0:0.00}", over_all_per);
                        attnd_report.Sheets[0].Cells[(attnd_report.Sheets[0].RowCount - 1), 1].HorizontalAlign = HorizontalAlign.Center;
                        attnd_report.Sheets[0].Cells[(attnd_report.Sheets[0].RowCount - 1), 2].HorizontalAlign = HorizontalAlign.Center;
                        attnd_report.Sheets[0].Cells[(attnd_report.Sheets[0].RowCount - 1), 3].HorizontalAlign = HorizontalAlign.Center;
                        attnd_report.Sheets[0].Cells[(attnd_report.Sheets[0].RowCount - 1), 4].HorizontalAlign = HorizontalAlign.Center;
                        attnd_report.Sheets[0].Cells[(attnd_report.Sheets[0].RowCount - 1), 5].HorizontalAlign = HorizontalAlign.Center;
                        attnd_report.Sheets[0].Cells[(attnd_report.Sheets[0].RowCount - 1), 6].HorizontalAlign = HorizontalAlign.Center;
                        attnd_report.Sheets[0].Cells[(attnd_report.Sheets[0].RowCount - 1), 7].HorizontalAlign = HorizontalAlign.Center;
                        attnd_report.Sheets[0].Cells[(attnd_report.Sheets[0].RowCount - 1), 8].HorizontalAlign = HorizontalAlign.Center;
                        attnd_report.Sheets[0].Cells[(attnd_report.Sheets[0].RowCount - 1), 9].HorizontalAlign = HorizontalAlign.Center;
                        attnd_report.Sheets[0].Cells[(attnd_report.Sheets[0].RowCount - 1), 10].HorizontalAlign = HorizontalAlign.Center;
                        attnd_report.Sheets[0].Cells[(attnd_report.Sheets[0].RowCount - 1), 11].HorizontalAlign = HorizontalAlign.Center;

                        // attnd_report.Sheets[0].SpanModel.Add((attnd_report.Sheets[0].RowCount - 1), 3, 1, 9);
                    }
                    attnd_report.Sheets[0].RowCount++;
                    attnd_report.Sheets[0].SpanModel.Add((attnd_report.Sheets[0].RowCount - 1), 0, 1, attnd_report.Sheets[0].ColumnCount);

                    attnd_report.Sheets[0].RowCount++;
                    attnd_report.Sheets[0].SpanModel.Add((attnd_report.Sheets[0].RowCount - 1), 0, 1, attnd_report.Sheets[0].ColumnCount);
                    attnd_report.Sheets[0].Cells[attnd_report.Sheets[0].RowCount - 1, 0].Text = "Dept. wise Absentees %";
                    attnd_report.Sheets[0].Rows[attnd_report.Sheets[0].RowCount - 1].Font.Bold = true;
                    attnd_report.Sheets[0].Rows[attnd_report.Sheets[0].RowCount - 1].Font.Size = FontUnit.Medium;

                    foreach (var kvp in dicdegreeattper)
                    {
                        string setval = kvp.Key.ToString();
                        string setvalk = kvp.Value.ToString();
                        attnd_report.Sheets[0].RowCount++;
                        if (chkPeriod.Checked == false)
                        {
                            string[] spv = setval.Split(':');
                            setval = spv[0].ToString();
                            acronym = d2.GetFunction("select Acronym from degree where Degree_Code='" + setval + "'");
                            if (spv[1].ToString().ToUpper() == "M")
                            {
                                acronym = acronym + " - Morning";
                            }
                            else
                            {
                                acronym = acronym + " - Evening";
                            }
                        }
                        else
                        {
                            acronym = d2.GetFunction("select Acronym from degree where Degree_Code='" + setval + "'");
                        }

                        attnd_report.Sheets[0].Cells[attnd_report.Sheets[0].RowCount - 1, 0].Text = acronym;
                        attnd_report.Sheets[0].Cells[attnd_report.Sheets[0].RowCount - 1, 0].Font.Bold = true;
                        attnd_report.Sheets[0].SpanModel.Add(attnd_report.Sheets[0].RowCount - 1, 0, 1, 4);
                        Double perse = 0;
                        string[] spget = setvalk.Split(':');
                        if (spget.GetUpperBound(0) > 0)
                        {
                            perse = Convert.ToDouble(spget[0]) / Convert.ToDouble(spget[1]);
                            perse = Math.Round(perse, 2, MidpointRounding.AwayFromZero);
                        }

                        attnd_report.Sheets[0].Cells[attnd_report.Sheets[0].RowCount - 1, 4].Text = perse.ToString();


                    }
                }
                else
                {
                    attnd_report.Visible = false;
                    btnxl.Visible = false;
                    lblrptname.Visible = false;
                    txtexcelname.Visible = false;
                    errlbl.Text = "Please Select The Required Fields And Then Proceed !!!";
                    errlbl.Visible = true;
                }
            }
        }
        //   catch(Exception e)
        {

        }
        if (norecflag == false)
        {
            pageset_pnl.Visible = false;
            errlbl.Visible = false;
            //pagesetpanel.Visible = false;
            attnd_report.Visible = false;
            btnprintmaster.Visible = false;
            btnxl.Visible = false;
            //Added by Srinath 27/2/2
            lblrptname.Visible = false;
            txtexcelname.Visible = false;
        }
        else
        {
            pageset_pnl.Visible = false;
            errlbl.Visible = true;
            attnd_report.Visible = true;
            btnprintmaster.Visible = true;
            btnxl.Visible = true;
            //Added by Srinath 27/2/2
            lblrptname.Visible = true;
            txtexcelname.Visible = true;
            //pagesetpanel.Visible = true;
            //setheader_print();
        }

        if (Convert.ToInt32(attnd_report.Sheets[0].RowCount) > 2)
        {
            pageset_pnl.Visible = false;
            Double totalRows = 0;
            totalRows = Convert.ToInt32(attnd_report.Sheets[0].RowCount);
            DropDownListpage.Items.Clear();
            if (totalRows >= 10)
            {
                attnd_report.Sheets[0].PageSize = 10;
                for (int k = 0; k < Convert.ToInt32(totalRows); k = k + 10)
                {
                    DropDownListpage.Items.Add((k + 10).ToString());
                }
                DropDownListpage.Items.Add("Others");
                attnd_report.Height = 10 + (10 * Convert.ToInt32(totalRows));
                attnd_report.VerticalScrollBarPolicy = FarPoint.Web.Spread.ScrollBarPolicy.AsNeeded;
                attnd_report.HorizontalScrollBarPolicy = FarPoint.Web.Spread.ScrollBarPolicy.AsNeeded;

            }
            else if (totalRows == 0)
            {
                DropDownListpage.Items.Add("0");
                attnd_report.Height = 200;
            }
            else
            {
                attnd_report.Sheets[0].PageSize = Convert.ToInt32(totalRows);
                DropDownListpage.Items.Add(attnd_report.Sheets[0].PageSize.ToString());

            }
            if (Convert.ToInt32(attnd_report.Sheets[0].RowCount) > 10)
            {
                DropDownListpage.Text = DropDownListpage.Items[DropDownListpage.Items.Count - 2].Text;
                attnd_report.Sheets[0].PageSize = int.Parse(DropDownListpage.Items[DropDownListpage.Items.Count - 2].Text);
                CalculateTotalPages();
            }
            Session["totalPages"] = (int)Math.Ceiling(totalRows / attnd_report.Sheets[0].PageSize);
            Buttontotal.Text = "Records : " + totalRows + "          Pages : " + Session["totalPages"];

        }
    }

    //public void BindSemster()
    //{
    //    try
    //    {
    //        int max_sem = 0;
    //        string qry = "select Max(NDurations) from Ndegree where college_code='" + Convert.ToString(Session["collegecode"]) + "'";
    //        string maxsem = d2.GetFunctionv(qry);
    //        if (maxsem == "" || maxsem == null)
    //        {
    //            maxsem = d2.GetFunctionv("select Max(Duration) from Degree where college_code='" + Convert.ToString(Session["collegecode"]) + "'");
    //        }
    //        int.TryParse(maxsem, out max_sem);
    //        for (int s = 0; s < max_sem; s++)
    //        {
    //            //ddlSem.Items.Insert(s, Convert.ToString((s + 1)));
    //            chklstsem.Items.Insert(s, Convert.ToString((s + 1)));
    //        }
    //        //ddlSem.SelectedIndex = 0;

    //    }
    //    catch (Exception ex)
    //    {

    //    }

    //}

    public void BindBatch()
    {
        try
        {
            ds2.Dispose();
            ds2.Reset();
            //string strsql = "select distinct batch_year from applyn where batch_year<>'-1' and batch_year<>'' order by batch_year asc";
            ds2 = d2.BindBatch();
            //ds2 = d2.select_method_wo_parameter(strsql, "Text");
            if (ds2.Tables[0].Rows.Count > 0)
            {
                chklsbatch.DataSource = ds2;
                chklsbatch.DataTextField = "Batch_year";
                chklsbatch.DataValueField = "Batch_year";
                chklsbatch.DataBind();
                for (int i = 0; i < chklsbatch.Items.Count; i++)
                {
                    if (chklsbatch.Items[i].Selected == true)
                    {
                        count += 1;
                    }
                    if (chklsbatch.Items.Count == count)
                    {
                        chkbatch.Checked = true;
                    }
                }
            }


        }
        catch (Exception ex)
        {
            //imgAlert.Visible = true;
            //lbl_alert.Text = ex.ToString();
        }
    }

    public void BindDegree(string singleuser, string group_user, string collegecode, string usercode)
    {
        try
        {
            count = 0;

            chklstdegree.Items.Clear();
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
                chklstdegree.DataSource = ds2;
                chklstdegree.DataTextField = "course_name";
                chklstdegree.DataValueField = "course_id";
                chklstdegree.DataBind();
                for (int i = 0; i < chklstdegree.Items.Count; i++)
                {
                    if (chklstdegree.Items[i].Selected == true)
                    {
                        count += 1;
                    }
                    if (chklstdegree.Items.Count == count)
                    {
                        chkdegree.Checked = true;
                    }
                }
                txtdegree.Enabled = true;
            }
            else
            {
                txtdegree.Enabled = false;
            }
        }
        catch (Exception ex)
        {
            //imgAlert.Visible = true;
            //lbl_alert.Text = ex.ToString();
        }

    }

    public void BindBranch(string singleuser, string group_user, string course_id, string collegecode, string usercode)
    {
        try
        {
            count = 0;

            for (int i = 0; i < chklstdegree.Items.Count; i++)
            {
                if (chklstdegree.Items[i].Selected == true)
                {
                    if (course_id == "")
                    {
                        course_id = "" + chklstdegree.Items[i].Value.ToString() + "";
                    }
                    else
                    {
                        course_id = course_id + "," + "" + chklstdegree.Items[i].Value.ToString() + "";
                    }
                }
            }
            chklstbranch.Items.Clear();
            if (group_user.Contains(';'))
            {
                string[] group_semi = group_user.Split(';');
                group_user = group_semi[0].ToString();
            }
            ds2.Dispose();
            ds2.Reset();
            ds2 = d2.BindBranchMultiple(singleuser, group_user, course_id, collegecode, usercode);
            if (ds2.Tables[0].Rows.Count > 0)
            {
                chklstbranch.DataSource = ds2;
                chklstbranch.DataTextField = "dept_name";
                chklstbranch.DataValueField = "degree_code";
                chklstbranch.DataBind();
                chklstbranch.Items[0].Selected = true;
                for (int i = 0; i < chklstbranch.Items.Count; i++)
                {
                    chklstbranch.Items[i].Selected = true;
                    if (chklstbranch.Items[i].Selected == true)
                    {
                        count += 1;
                    }
                    if (chklstbranch.Items.Count == count)
                    {
                        chkbranch.Checked = true;
                    }
                }
            }
        }
        catch (Exception ex)
        {
            //imgAlert.Visible = true;
            //lbl_alert.Text = ex.ToString();
        }

    }

    public void BindSectransport(string strbatch, string strbranch)
    {
        try
        {
            count = 0;

            for (int i = 0; i < chklsbatch.Items.Count; i++)
            {
                if (chklsbatch.Items[i].Selected == true)
                {
                    if (strbatch == "")
                    {
                        strbatch = "'" + chklsbatch.Items[i].Value.ToString() + "'";
                    }
                    else
                    {
                        strbatch = strbatch + "," + "'" + chklsbatch.Items[i].Value.ToString() + "'";
                    }
                }
            }

            for (int i = 0; i < chklstbranch.Items.Count; i++)
            {
                if (chklstbranch.Items[i].Selected == true)
                {
                    if (strbranch == "")
                    {
                        strbranch = "'" + chklstbranch.Items[i].Value.ToString() + "'";
                    }
                    else
                    {
                        strbranch = strbranch + "," + "'" + chklstbranch.Items[i].Value.ToString() + "'";
                    }
                }
            }
        }
        catch (Exception ex)
        {
            //imgAlert.Visible = true;
            //lbl_alert.Text = ex.ToString();
        }
    }

    public void BindBranchMultiple(string singleuser, string group_user, string course_id, string collegecode, string usercode)
    {
        try
        {
            count = 0;

            for (int i = 0; i < chklstdegree.Items.Count; i++)
            {
                if (chklstdegree.Items[i].Selected == true)
                {
                    if (course_id == "")
                    {
                        course_id = "" + chklstdegree.Items[i].Value.ToString() + "";
                    }
                    else
                    {
                        course_id = course_id + "," + "" + chklstdegree.Items[i].Value.ToString() + "";
                    }
                }
            }
            chklstbranch.Items.Clear();
            if (group_user.Contains(';'))
            {
                string[] group_semi = group_user.Split(';');
                group_user = group_semi[0].ToString();
            }
            ds2.Dispose();
            ds2.Reset();
            ds2 = d2.BindBranchMultiple(singleuser, group_user, course_id, collegecode, usercode);
            if (ds2.Tables[0].Rows.Count > 0)
            {
                chklstbranch.DataSource = ds2;
                chklstbranch.DataTextField = "dept_name";
                chklstbranch.DataValueField = "degree_code";
                chklstbranch.DataBind();
                chklstbranch.Items[0].Selected = true;
                for (int i = 0; i < chklstbranch.Items.Count; i++)
                {
                    chklstbranch.Items[i].Selected = true;
                    if (chklstbranch.Items[i].Selected == true)
                    {
                        count += 1;
                    }
                    if (chklstbranch.Items.Count == count)
                    {
                        chkbranch.Checked = true;
                    }
                }
            }
        }

        catch (Exception ex)
        {
            //imgAlert.Visible = true;
            //lbl_alert.Text = ex.ToString();
        }
    }


    # region Batch CheckChange -Events
    protected void chkbatch_ChekedChange(object sender, EventArgs e)
    {
        try
        {
            if (chkbatch.Checked == true)
            {
                for (int i = 0; i < chklsbatch.Items.Count; i++)
                {
                    chklsbatch.Items[i].Selected = true;
                    txtbatch.Text = "Batch(" + (chklsbatch.Items.Count) + ")";
                }
            }
            else
            {
                for (int i = 0; i < chklsbatch.Items.Count; i++)
                {
                    chklsbatch.Items[i].Selected = false;
                    txtbatch.Text = "---Select---";
                }
            }
        }
        catch (Exception ex)
        {
            //imgAlert.Visible = true;
            //lbl_alert.Text = ex.ToString();
        }
    }
    protected void chklstbatch_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {

            string clg = "";
            int commcount = 0;
            for (int i = 0; i < chklsbatch.Items.Count; i++)
            {
                if (chklsbatch.Items[i].Selected == true)
                {
                    commcount = commcount + 1;
                    txtbatch.Text = "Batch(" + commcount.ToString() + ")";
                    if (clg == "")
                    {
                        clg = chklsbatch.Items[i].Value.ToString();
                    }
                    else
                    {
                        clg = clg + "','" + chklsbatch.Items[i].Value;
                    }
                }
            }

            if (commcount == 0)
            {
                txtbatch.Text = "--Select--";
            }
        }
        catch (Exception ex)
        {
            //imgAlert.Visible = true;
            //lbl_alert.Text = ex.ToString();
        }

    }
    #endregion

    # region Degree CheckChange -Events
    protected void chkdegree_CheckedChanged(object sender, EventArgs e)
    {
        try
        {
            if (chkdegree.Checked == true)
            {
                for (int i = 0; i < chklstdegree.Items.Count; i++)
                {
                    chklstdegree.Items[i].Selected = true;
                    txtdegree.Text = "Degree(" + (chklstdegree.Items.Count) + ")";
                }
            }
            else
            {
                for (int i = 0; i < chklstdegree.Items.Count; i++)
                {
                    chklstdegree.Items[i].Selected = false;
                    txtdegree.Text = "---Select---";
                }
            }
            BindBranchMultiple(singleuser, group_user, course_id, collegecode, usercode);
        }
        catch (Exception ex)
        {
            //imgAlert.Visible = true;
            //lbl_alert.Text = ex.ToString();
        }
    }
    protected void chklstdegree_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            string clg = "";
            int commcount = 0;
            for (int i = 0; i < chklstdegree.Items.Count; i++)
            {
                if (chklstdegree.Items[i].Selected == true)
                {
                    commcount = commcount + 1;
                    txtdegree.Text = "Degree(" + commcount.ToString() + ")";
                    if (clg == "")
                    {
                        clg = chklstdegree.Items[i].Value.ToString();
                    }
                    else
                    {
                        clg = clg + "','" + chklstdegree.Items[i].Value;
                    }
                }
            }
            if (commcount == 0)
            {
                txtbatch.Text = "--Select--";
            }
            BindBranchMultiple(singleuser, group_user, course_id, collegecode, usercode);
            chklstbranch_SelectedIndexChanged(sender, e);
        }
        catch (Exception ex)
        {
            //imgAlert.Visible = true;
            //lbl_alert.Text = ex.ToString();
        }
    }
    #endregion

    # region Branch CheckChange -Events
    protected void chkbranch_CheckedChanged(object sender, EventArgs e)
    {
        try
        {
            if (chkbranch.Checked == true)
            {
                for (int i = 0; i < chklstbranch.Items.Count; i++)
                {
                    chklstbranch.Items[i].Selected = true;
                    txtbranch.Text = "Branch(" + (chklstbranch.Items.Count) + ")";
                }
            }
            else
            {
                for (int i = 0; i < chklstbranch.Items.Count; i++)
                {
                    chklstbranch.Items[i].Selected = false;
                    txtbranch.Text = "---Select---";
                }
            }
        }
        catch (Exception ex)
        {
            //imgAlert.Visible = true;
            //lbl_alert.Text = ex.ToString();
        }
    }
    protected void chklstbranch_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            string clg = "";
            int commcount = 0;
            for (int i = 0; i < chklstbranch.Items.Count; i++)
            {
                if (chklstbranch.Items[i].Selected == true)
                {
                    commcount = commcount + 1;
                    txtbranch.Text = "Branch(" + commcount.ToString() + ")";
                    if (clg == "")
                    {
                        clg = chklstbranch.Items[i].Value.ToString();
                    }
                    else
                    {
                        clg = clg + "','" + chklstbranch.Items[i].Value;
                    }
                }
            }

            if (commcount == 0)
            {
                txtbatch.Text = "--Select--";
            }
        }
        catch (Exception ex)
        {
            //imgAlert.Visible = true;
            //lbl_alert.Text = ex.ToString();
        }
    }
    #endregion

    //protected void chksem_CheckedChanged(object sender, EventArgs e)
    //{
    //    if (chkSem.Checked == true)
    //    {
    //        for (int i = 0; i < chklstsem.Items.Count; i++)
    //        {
    //            chklstsem.Items[i].Selected = true;
    //            txtSem.Text = "Semester(" + (chklstsem.Items.Count) + ")";
    //        }
    //    }
    //    else
    //    {
    //        for (int i = 0; i < chklstsem.Items.Count; i++)
    //        {
    //            chklstsem.Items[i].Selected = false;
    //            txtSem.Text = "---Select---";
    //        }
    //    }
    //}

    //protected void chklstsem_SelectedIndexChanged(object sender, EventArgs e)
    //{
    //    try
    //    {
    //        string clg = "";
    //        int commcount = 0;
    //        for (int i = 0; i < chklstsem.Items.Count; i++)
    //        {
    //            if (chklstsem.Items[i].Selected == true)
    //            {
    //                commcount = commcount + 1;
    //                txtSem.Text = "Semester(" + commcount.ToString() + ")";
    //                if (clg == "")
    //                {
    //                    clg = chklstsem.Items[i].Value.ToString();
    //                }
    //                else
    //                {
    //                    clg = clg + "','" + chklstsem.Items[i].Value;
    //                }
    //            }
    //        }
    //        if (chklstsem.Items.Count == commcount)
    //        {
    //            chkSem.Checked = true;
    //        }
    //        else
    //        {
    //            chkSem.Checked = false;
    //        }
    //        if (commcount == 0)
    //        {
    //            txtSem.Text = "--Select--";
    //            chkSem.Checked = false;
    //        }
    //    }
    //    catch (Exception ex)
    //    {
    //        //lblset.Text = ex.ToString();
    //    }
    //}

    //===============Hided by Manikandan 18/05/2013
    //public void print_btngo()
    //{
    //    final_print_col_cnt = 0;
    //    errmsg.Visible = false;
    //    check_col_count_flag = false;

    //    attnd_report.Sheets[0].SheetCorner.RowCount = 0;
    //    attnd_report.Sheets[0].ColumnCount = 0;
    //    attnd_report.Sheets[0].RowCount = 0;
    //    attnd_report.Sheets[0].SheetCorner.RowCount = 8;
    //    attnd_report.Sheets[0].ColumnCount = 5;


    //    has.Clear();
    //    has.Add("college_code", Session["InternalCollegeCode"].ToString());
    //    has.Add("form_name", "ovrall_attreport_perday.aspx");
    //    dsprint = dacces2.select_method("PROC_PRINT_MASTER_SETTINGS", has, "sp");
    //    if (dsprint.Tables[0].Rows.Count > 0)
    //    {
    //        lblpages.Visible = true;
    //        ddlpage.Visible = true;

    //        //3. header add
    //        //if (dsprint.Tables[0].Rows[0]["new_header_name"].ToString() != null && dsprint.Tables[0].Rows[0]["new_header_name"].ToString() != "")
    //        //{
    //        //    new_header_string = dsprint.Tables[0].Rows[0]["new_header_name"].ToString();
    //        //    new_header_string_split = new_header_string.Split(',');
    //        //    attnd_report.Sheets[0].SheetCorner.RowCount = attnd_report.Sheets[0].SheetCorner.RowCount + new_header_string_split.GetUpperBound(0) + 1;
    //        //}
    //        //3. end header add


    //        load_btn_click();



    //        //1.set visible columns
    //        column_field = dsprint.Tables[0].Rows[0]["column_fields"].ToString();//--------------------get column field
    //        if (column_field != "" && column_field != null)
    //        {
    //            //  check_col_count_flag = true;

    //            for (col_count_all = 0; col_count_all < attnd_report.Sheets[0].ColumnCount; col_count_all++)
    //            {
    //                attnd_report.Sheets[0].Columns[col_count_all].Visible = false;//------------invisible all column                                
    //            }


    //            printvar = dsprint.Tables[0].Rows[0]["column_fields"].ToString();//--------------visible setting columns
    //            string[] split_printvar = printvar.Split(',');
    //            for (int splval = 0; splval <= split_printvar.GetUpperBound(0); splval++)
    //            {
    //                span_cnt = 0;
    //                string[] split_star = split_printvar[splval].Split('*');


    //                {
    //                    for (col_count = 0; col_count < attnd_report.Sheets[0].ColumnCount; col_count++)
    //                    {
    //                        if (attnd_report.Sheets[0].ColumnHeader.Cells[(attnd_report.Sheets[0].ColumnHeader.RowCount - 1), col_count].Text == split_printvar[splval])
    //                        {
    //                            attnd_report.Sheets[0].Columns[col_count].Visible = true;
    //                            final_print_col_cnt++;
    //                            break;
    //                        }
    //                    }
    //                }
    //            }
    //            //1 end.set visible columns
    //        }
    //        else
    //        {
    //            attnd_report.Visible = false;
    //            btnxl.Visible = false;
    //            //Added by Srinath 27/2/2
    //            lblrptname.Visible = false;
    //            txtexcelname.Visible = false;
    //            pageset_pnl.Visible = false;
    //            lblpages.Visible = false;
    //            ddlpage.Visible = false;
    //            errlbl.Visible = true;
    //            errlbl.Text = "Select Atleast One Column Field From The Treeview";
    //        }
    //    }
    //    // attnd_report.Width = final_print_col_cnt * 100;
    //}

    //======================

    void CalculateTotalPages()
    {
        Double totalRows = 0;
        totalRows = Convert.ToInt32(attnd_report.Sheets[0].RowCount);
        Session["totalPages"] = (int)Math.Ceiling(totalRows / attnd_report.Sheets[0].PageSize);
        Buttontotal.Text = "Records : " + totalRows + "          Pages : " + Session["totalPages"];
        Buttontotal.Visible = true;
    }

    protected void pageddltxt_TextChanged(object sender, EventArgs e)
    {
        errmsg.Visible = false;
        attnd_report.CurrentPage = 0;
        pagesearch_txt.Text = "";
        try
        {
            if (pageddltxt.Text != string.Empty)
            {
                if (attnd_report.Sheets[0].RowCount >= Convert.ToInt16(pageddltxt.Text.ToString()) && Convert.ToInt16(pageddltxt.Text.ToString()) != 0)
                {
                    attnd_report.Sheets[0].PageSize = Convert.ToInt16(pageddltxt.Text.ToString());
                    attnd_report.Height = 30 + (25 * Convert.ToInt32(pageddltxt.Text.ToString()));
                    CalculateTotalPages();
                }
                else
                {
                    errmsg.Visible = true;
                    errmsg.Text = "Enter valid Record count";
                    pageddltxt.Text = "";
                }
            }
        }
        catch
        {
            errmsg.Visible = true;
            errmsg.Text = "Enter valid Record count";
            pageddltxt.Text = "";
        }
    }

    protected void DropDownListpage_SelectedIndexChanged(object sender, EventArgs e)
    {
        errmsg.Visible = false;
        attnd_report.CurrentPage = 0;
        pagesearch_txt.Text = "";
        errmsg.Visible = false;
        pagesearch_txt.Text = "";
        pageddltxt.Text = "";
        pageddltxt.Text = "";
        if (DropDownListpage.Text == "Others")
        {

            pageddltxt.Visible = true;
            pageddltxt.Focus();

        }
        else
        {
            pageddltxt.Visible = false;
            attnd_report.Sheets[0].PageSize = Convert.ToInt16(DropDownListpage.Text.ToString());
            attnd_report.Height = 30 + (25 * Convert.ToInt32(DropDownListpage.Text.ToString()));
            CalculateTotalPages();
        }
    }

    protected void pagesearch_txt_TextChanged(object sender, EventArgs e)
    {
        errmsg.Visible = false;
        if (pagesearch_txt.Text.Trim() != string.Empty)
        {
            if (Convert.ToInt64(pagesearch_txt.Text) > Convert.ToInt64(Session["totalPages"]))
            {
                errmsg.Visible = true;
                errmsg.Text = "Exceed The Page Limit";
                pagesearch_txt.Text = "";
                attnd_report.Visible = true;
                btnprintmaster.Visible = true;
                btnxl.Visible = true;
                //Added by Srinath 27/2/2
                lblrptname.Visible = true;
                txtexcelname.Visible = true;
            }
            else if (Convert.ToInt64(pagesearch_txt.Text) == 0)
            {
                errmsg.Visible = true;
                errmsg.Text = " Search Should Be Greater Than '0'";
                pagesearch_txt.Text = "";
                attnd_report.Visible = true;
                btnprintmaster.Visible = true;
                btnxl.Visible = true;
                //Added by Srinath 27/2/2
                lblrptname.Visible = true;
                txtexcelname.Visible = true;
            }

            else
            {
                errmsg.Visible = false;
                attnd_report.CurrentPage = Convert.ToInt16(pagesearch_txt.Text) - 1;
                attnd_report.Visible = true;
                btnprintmaster.Visible = true;
                btnxl.Visible = true;
                //Added by Srinath 27/2/2
                lblrptname.Visible = true;
                txtexcelname.Visible = true;
            }
        }
    }

    public string GetFunction(string sqlQuery)
    {
        string sqlstr = "";
        sqlstr = sqlQuery;
        con_getfunc.Close();
        con_getfunc.Open();
        SqlDataAdapter sqlAdapter1 = new SqlDataAdapter(sqlstr, con_getfunc);
        SqlDataReader drnew;
        SqlCommand funcmd = new SqlCommand(sqlstr);
        funcmd.Connection = con_getfunc;
        drnew = funcmd.ExecuteReader();
        drnew.Read();
        if (drnew.HasRows == true)
        {
            return drnew[0].ToString();
        }
        else
        {
            return "";
        }
    }

    public string GetRomanChar(int intsem)
    {
        string strChkSet = "";
        string linkvalue = "";

        strChkSet = "select * from inssettings where college_code=" + Session["InternalCollegeCode"] + " and LinkName ='Semester Display'";
        con_chkSet.Close();
        con_chkSet.Open();
        SqlCommand cmdChkset = new SqlCommand(strChkSet, con_chkSet);
        SqlDataReader drChkset;
        drChkset = cmdChkset.ExecuteReader();
        while (drChkset.Read())
        {
            if (drChkset.HasRows == true)
            {

                linkvalue = drChkset["LinkValue"].ToString();
                if (linkvalue == "1")
                {
                    switch (intsem)
                    {
                        case 1:
                            GetChar = "1";
                            break;
                        case 2:
                            GetChar = "1-II";
                            break;
                        case 3:
                            GetChar = "2-I";
                            break;
                        case 4:
                            GetChar = "2-II";
                            break;
                        case 5:
                            GetChar = "3-I";
                            break;
                        case 6:
                            GetChar = "3-II";
                            break;
                        case 7:
                            GetChar = "4-I";
                            break;
                        case 8:
                            GetChar = "4-II";
                            break;
                        default:
                            GetChar = " ";
                            break;
                    }//'--- end switch
                } //'--- end linkvalue=1
                else
                {
                    switch (intsem)
                    {
                        case 1:
                            GetChar = "I";
                            break;
                        case 2:
                            GetChar = "II";
                            break;
                        case 3:
                            GetChar = "III";
                            break;
                        case 4:
                            GetChar = "IV";
                            break;
                        case 5:
                            GetChar = "V";
                            break;
                        case 6:
                            GetChar = "VI";
                            break;
                        case 7:
                            GetChar = "VII";
                            break;
                        case 8:
                            GetChar = "VIII";
                            break;
                        case 9:
                            GetChar = "IX";
                            break;
                        case 10:
                            GetChar = "X";
                            break;
                        default:
                            GetChar = " ";
                            break;
                    }//'-- end else switch
                }//'---- end else
            }// end hasrows
        } //'---- end if
        return GetChar;
    } //'---- end while

    protected void attnd_report_SelectedIndexChanged(Object sender, EventArgs e)
    {
        //if (Cellclick == false)
        //{
        //    attnd_report.Sheets[0].AutoPostBack = true;
        //}

    }

    protected void attnd_report_CellClick(object sender, FarPoint.Web.Spread.SpreadCommandEventArgs e)
    {

        //Cellclick = true;
        //if (Cellclick == true)
        //{
        //    attnd_report.Sheets[0].AutoPostBack = false;
        //}
        //Cellclick = false;

    }

    protected void LinkButton3_Click(object sender, EventArgs e)
    {

    }

    public string sem_roman(int sem)
    {
        string sql = "";
        string sem_roman = "";
        SqlDataReader rsChkSet;
        con1.Close();
        con1.Open();
        sql = "select * from inssettings where college_code=" + Session["InternalCollegeCode"] + " and LinkName ='Semester Display'";
        SqlCommand cmd1 = new SqlCommand(sql, con1);
        rsChkSet = cmd1.ExecuteReader();
        rsChkSet.Read();
        if (rsChkSet.HasRows == true)
        {
            if (rsChkSet["linkvalue"].ToString() == "1")
            {
                switch (sem)
                {
                    case 1:
                        sem_roman = "1";
                        break;
                    case 2:
                        sem_roman = "1-II";
                        break;
                    case 3:
                        sem_roman = "2-I";
                        break;
                    case 4:
                        sem_roman = "2-II";
                        break;
                    case 5:
                        sem_roman = "3-I";
                        break;
                    case 6:
                        sem_roman = "3-II";
                        break;
                    case 7:
                        sem_roman = "4-I";
                        break;
                    case 8:
                        sem_roman = "4-II";
                        break;
                    default:
                        sem_roman = " ";
                        break;
                }
            }
            else
            {
                switch (sem)
                {
                    case 1:
                        sem_roman = "I";
                        break;
                    case 2:
                        sem_roman = "II";
                        break;
                    case 3:
                        sem_roman = "III";
                        break;
                    case 4:
                        sem_roman = "IV";
                        break;
                    case 5:
                        sem_roman = "V";
                        break;
                    case 6:
                        sem_roman = "VI";
                        break;
                    case 7:
                        sem_roman = "VII";
                        break;
                    case 8:
                        sem_roman = "VIII";
                        break;
                    case 9:
                        sem_roman = "IX";
                        break;
                    case 10:
                        sem_roman = "X";
                        break;
                    default:
                        sem_roman = " ";
                        break;

                }
            }
        }
        return sem_roman;
    }

    public void GiveCourseName(string deg_code, out string course_value, out string course_namevalue)
    {
        string course_val = "";
        string course_name = "";
        SqlDataReader RsCName;
        con2.Close();
        con2.Open();
        SqlCommand cmd2 = new SqlCommand("select distinct ltrim(Dept_acronym) as CName,dept_name from Course,Department,Degree where  Degree.Course_Id = Course.Course_Id And Department.Dept_Code = Degree.Dept_Code  and Degree.Degree_Code = " + deg_code + " ", con2);
        RsCName = cmd2.ExecuteReader();
        RsCName.Read();
        if (RsCName.HasRows == true)
        {
            if (RsCName["CName"].ToString() != "")
            {
                course_val = RsCName["CName"].ToString();
                course_name = RsCName["dept_name"].ToString();
            }
        }
        course_value = course_val;
        course_namevalue = course_name;
        // return course_val;
    }

    protected void txtFromDate_TextChanged(object sender, EventArgs e)
    {
        lblpages.Visible = false;
        ddlpage.Visible = false;
        // pagesetpanel.Visible = false;
        attnd_report.Visible = false;
        btnprintmaster.Visible = false;
        btnxl.Visible = false;
        //Added by Srinath 27/2/2
        lblrptname.Visible = false;
        txtexcelname.Visible = false;
        pageset_pnl.Visible = false;
        errlbl.Visible = false;
    }

    //public void setheader()
    //{

    //    string coll_name = "", address1 = "", address2 = "", address3 = "", phoneno = "", faxno = "", email = "", website = "";

    //    MyImg mi = new MyImg();
    //    mi.ImageUrl = "~/images/10BIT001.jpeg";
    //    mi.ImageUrl = "Handler/Handler2.ashx?";
    //    MyImg mi2 = new MyImg();
    //    mi2.ImageUrl = "~/images/10BIT001.jpeg";
    //    mi2.ImageUrl = "Handler/Handler5.ashx?";


    //    if (Session["collegecode"].ToString() != null && Session["collegecode"].ToString() != "")
    //    {
    //        SqlDataReader dr_collinfo;
    //        con.Close();
    //        con.Open();
    //        cmd = new SqlCommand("select isnull(collname,'') as collname,isnull(address1,'') as address1,isnull(address2,'') as address2,isnull(address3,'') as address3,isnull(phoneno,'') as phoneno,isnull(faxno,'') as faxno,isnull(email,'') as email,isnull(website,'') as website from collinfo  where college_code=" + Session["collegecode"] + "", con);
    //        dr_collinfo = cmd.ExecuteReader();
    //        while (dr_collinfo.Read())
    //        {
    //            if (dr_collinfo.HasRows == true)
    //            {

    //                coll_name = dr_collinfo["collname"].ToString();
    //                address1 = dr_collinfo["address1"].ToString();
    //                address2 = dr_collinfo["address2"].ToString();
    //                address3 = dr_collinfo["address3"].ToString();
    //                phoneno = dr_collinfo["phoneno"].ToString();
    //                faxno = dr_collinfo["faxno"].ToString();
    //                email = dr_collinfo["email"].ToString();
    //                website = dr_collinfo["website"].ToString();
    //            }
    //        }


    //        attnd_report.Sheets[0].SheetCorner.Cells[0, 0].Border.BorderColorRight = Color.White;


    //        if (attnd_report.Sheets[0].Columns[0].Visible == true)
    //        {
    //            attnd_report.Sheets[0].ColumnHeaderSpanModel.Add(0, 2, 1, (attnd_report.Sheets[0].ColumnCount - 4));
    //            attnd_report.Sheets[0].ColumnHeader.Cells[0, 2].Text = coll_name;

    //            attnd_report.Sheets[0].ColumnHeader.Cells[0, 2].Border.BorderColorBottom = Color.White;
    //            attnd_report.Sheets[0].ColumnHeader.Cells[1, 2].Border.BorderColorTop = Color.White;
    //            attnd_report.Sheets[0].ColumnHeader.Cells[1, 2].Border.BorderColorBottom = Color.White;
    //            attnd_report.Sheets[0].ColumnHeader.Cells[2, 2].Border.BorderColorTop = Color.White;
    //            attnd_report.Sheets[0].ColumnHeader.Cells[2, 2].Border.BorderColorBottom = Color.White;
    //            attnd_report.Sheets[0].ColumnHeader.Cells[3, 2].Border.BorderColorTop = Color.White;
    //            attnd_report.Sheets[0].ColumnHeader.Cells[3, 2].Border.BorderColorBottom = Color.White;
    //            attnd_report.Sheets[0].ColumnHeader.Cells[4, 2].Border.BorderColorTop = Color.White;
    //            attnd_report.Sheets[0].ColumnHeader.Cells[4, 2].Border.BorderColorBottom = Color.White;
    //            attnd_report.Sheets[0].ColumnHeader.Cells[5, 2].Border.BorderColorTop = Color.White;
    //            attnd_report.Sheets[0].ColumnHeader.Cells[5, 2].Border.BorderColorBottom = Color.White;
    //            attnd_report.Sheets[0].ColumnHeader.Cells[6, 2].Border.BorderColorTop = Color.White;


    //            attnd_report.Sheets[0].ColumnHeader.Cells[0, 2].Border.BorderColorLeft = Color.White;
    //            attnd_report.Sheets[0].ColumnHeader.Cells[1, 2].Border.BorderColorLeft = Color.White;
    //            attnd_report.Sheets[0].ColumnHeader.Cells[2, 2].Border.BorderColorLeft = Color.White;
    //            attnd_report.Sheets[0].ColumnHeader.Cells[3, 2].Border.BorderColorLeft = Color.White;
    //            attnd_report.Sheets[0].ColumnHeader.Cells[4, 2].Border.BorderColorLeft = Color.White;
    //            attnd_report.Sheets[0].ColumnHeader.Cells[5, 2].Border.BorderColorLeft = Color.White;
    //            attnd_report.Sheets[0].ColumnHeader.Cells[6, 2].Border.BorderColorLeft = Color.White;
    //            attnd_report.Sheets[0].ColumnHeader.Cells[0, 0].Border.BorderColorRight = Color.White;
    //            attnd_report.Sheets[0].ColumnHeader.Cells[1, 0].Border.BorderColorRight = Color.White;
    //            attnd_report.Sheets[0].ColumnHeader.Cells[2, 0].Border.BorderColorRight = Color.White;
    //            attnd_report.Sheets[0].ColumnHeader.Cells[3, 0].Border.BorderColorRight = Color.White;
    //            attnd_report.Sheets[0].ColumnHeader.Cells[4, 0].Border.BorderColorRight = Color.White;
    //            attnd_report.Sheets[0].ColumnHeader.Cells[5, 0].Border.BorderColorRight = Color.White;
    //            attnd_report.Sheets[0].ColumnHeader.Cells[6, 0].Border.BorderColorRight = Color.White;

    //            attnd_report.Sheets[0].ColumnHeader.Cells[0, 9].Border.BorderColorLeft = Color.White;
    //            attnd_report.Sheets[0].ColumnHeader.Cells[1, 9].Border.BorderColorLeft = Color.White;
    //            attnd_report.Sheets[0].ColumnHeader.Cells[2, 9].Border.BorderColorLeft = Color.White;
    //            attnd_report.Sheets[0].ColumnHeader.Cells[3, 9].Border.BorderColorLeft = Color.White;
    //            attnd_report.Sheets[0].ColumnHeader.Cells[4, 9].Border.BorderColorLeft = Color.White;
    //            attnd_report.Sheets[0].ColumnHeader.Cells[5, 9].Border.BorderColorLeft = Color.White;
    //            attnd_report.Sheets[0].ColumnHeader.Cells[6, 9].Border.BorderColorLeft = Color.White;
    //            attnd_report.Sheets[0].ColumnHeader.Cells[0, 2].Border.BorderColorRight = Color.White;
    //            attnd_report.Sheets[0].ColumnHeader.Cells[1, 2].Border.BorderColorRight = Color.White;
    //            attnd_report.Sheets[0].ColumnHeader.Cells[2, 2].Border.BorderColorRight = Color.White;
    //            attnd_report.Sheets[0].ColumnHeader.Cells[3, 2].Border.BorderColorRight = Color.White;
    //            attnd_report.Sheets[0].ColumnHeader.Cells[4, 2].Border.BorderColorRight = Color.White;
    //            attnd_report.Sheets[0].ColumnHeader.Cells[5, 2].Border.BorderColorRight = Color.White;
    //            attnd_report.Sheets[0].ColumnHeader.Cells[6, 2].Border.BorderColorRight = Color.White;


    //            attnd_report.Sheets[0].ColumnHeaderSpanModel.Add(1, 2, 1, (attnd_report.Sheets[0].ColumnCount - 4));
    //            attnd_report.Sheets[0].ColumnHeader.Cells[1, 2].Text = address1 + "-" + address2 + "-" + address3;
    //            attnd_report.Sheets[0].ColumnHeaderSpanModel.Add(2, 2, 1, (attnd_report.Sheets[0].ColumnCount - 4));
    //            attnd_report.Sheets[0].ColumnHeader.Cells[2, 2].Text = "Phone:" + phoneno + "  Fax:" + faxno;
    //            attnd_report.Sheets[0].ColumnHeaderSpanModel.Add(3, 2, 1, (attnd_report.Sheets[0].ColumnCount - 4));
    //            attnd_report.Sheets[0].ColumnHeader.Cells[3, 2].Text = "Email:" + email + "  Web Site:" + website;
    //            attnd_report.Sheets[0].ColumnHeaderSpanModel.Add(4, 2, 1, (attnd_report.Sheets[0].ColumnCount - 4));
    //            attnd_report.Sheets[0].ColumnHeader.Cells[4, 2].Text = "Over All Attendance Report For Particular Day   ";
    //            attnd_report.Sheets[0].ColumnHeaderSpanModel.Add(5, 2, 1, (attnd_report.Sheets[0].ColumnCount - 4));
    //            attnd_report.Sheets[0].ColumnHeader.Cells[5, 2].Text = "----------------------------------------------------------------";
    //            //attnd_report.Sheets[0].ColumnHeader.Cells[4, 0].Border.BorderColorBottom = Color.White;
    //            //attnd_report.Sheets[0].ColumnHeader.Cells[5, 0].Border.BorderColorTop = Color.White;
    //            //attnd_report.Sheets[0].ColumnHeader.Cells[5, 0].Border.BorderColorRight = Color.White;
    //            //attnd_report.Sheets[0].ColumnHeader.Cells[5, 0].Border.BorderColorBottom = Color.White;

    //            string dt = DateTime.Today.ToShortDateString();
    //            string[] dsplit = dt.Split(new Char[] { '/' });


    //            attnd_report.Sheets[0].ColumnHeaderSpanModel.Add(6, 2, 1, (attnd_report.Sheets[0].ColumnCount - 4));
    //            attnd_report.Sheets[0].ColumnHeader.Cells[6, 2].Text ="Attendance Date: "+txtFromDate.Text + " Date On: " + dsplit[1].ToString() + "/" + dsplit[0].ToString() + "/" + dsplit[2].ToString();




    //        }


    //        attnd_report.Sheets[0].ColumnHeaderSpanModel.Add(0, 0, 7, 2);
    //        attnd_report.Sheets[0].ColumnHeaderSpanModel.Add(0, ((attnd_report.Sheets[0].ColumnCount - 2)), 7, 2);
    //        attnd_report.Sheets[0].ColumnHeader.Cells[0, 0].CellType = mi;
    //        attnd_report.Sheets[0].ColumnHeader.Cells[0, (attnd_report.Sheets[0].ColumnCount - 2)].CellType = mi2;


    //    }



    //    int overall_colcount = 0;
    //    attnd_report.Sheets[0].PageSize = attnd_report.Sheets[0].RowCount;
    //    overall_colcount = attnd_report.Sheets[0].ColumnCount;
    //    attnd_report.Width = (overall_colcount * 80);

    //}
    //protected void RadioHeader_CheckedChanged(object sender, EventArgs e)
    //{
    //    attnd_report.Sheets[0].Rows[0].Visible = true;
    //    attnd_report.Sheets[0].Rows[1].Visible = true;
    //    attnd_report.Sheets[0].Rows[2].Visible = true;
    //    attnd_report.Sheets[0].Rows[3].Visible = true;
    //    attnd_report.Sheets[0].Rows[4].Visible = true;
    //    attnd_report.Sheets[0].Rows[5].Visible = true;
    //    attnd_report.Sheets[0].Rows[6].Visible = true;
    //    attnd_report.Sheets[0].Rows[7].Visible = true;

    //    int i = 0;
    //    ddlpage.Items.Clear();
    //    int totrowcount = attnd_report.Sheets[0].RowCount;
    //    int pages = totrowcount / 25;
    //    int intialrow = 1;
    //    int remainrows = totrowcount % 25;
    //    if (attnd_report.Sheets[0].RowCount > 0)
    //    {
    //        int i5 = 0;
    //        ddlpage.Items.Insert(0, new System.Web.UI.WebControls.ListItem("", "0"));
    //        for (i = 1; i <= pages; i++)
    //        {
    //            i5 = i;

    //            ddlpage.Items.Insert(i, new System.Web.UI.WebControls.ListItem(i.ToString(), intialrow.ToString()));
    //            intialrow = intialrow + 25;
    //        }
    //        if (remainrows > 0)
    //        {
    //            i = i5 + 1;
    //            ddlpage.Items.Insert(i, new System.Web.UI.WebControls.ListItem(i.ToString(), intialrow.ToString()));
    //        }
    //    }
    //    if ((ddlpage.SelectedValue.ToString() == string.Empty) || (ddlpage.SelectedValue.ToString() == "0"))
    //    {
    //        for (i = 0; i < attnd_report.Sheets[0].RowCount; i++)
    //        {
    //            attnd_report.Sheets[0].Rows[i].Visible = true;
    //        }
    //        Double totalRows = 0;
    //        totalRows = Convert.ToInt32(attnd_report.Sheets[0].RowCount);
    //        Session["totalPages"] = (int)Math.Ceiling(totalRows / attnd_report.Sheets[0].PageSize);
    //        Buttontotal.Text = "Records : " + totalRows + "          Pages : " + Session["totalPages"];
    //        DropDownListpage.Items.Clear();
    //        if (totalRows >= 10)
    //        {
    //            attnd_report.Sheets[0].PageSize = Convert.ToInt32(totalRows);
    //            for (int k = 0; k < Convert.ToInt32(totalRows); k = k + 10)
    //            {
    //                DropDownListpage.Items.Add((k + 10).ToString());
    //            }
    //            DropDownListpage.Items.Add("Others");
    //            DropDownListpage.Text = DropDownListpage.Items[DropDownListpage.Items.Count - 2].Text;
    //            attnd_report.Height = 335;

    //        }
    //        else if (totalRows == 0)
    //        {
    //            DropDownListpage.Items.Add("0");
    //            attnd_report.Height = 100;
    //        }
    //        else
    //        {
    //            attnd_report.Sheets[0].PageSize = Convert.ToInt32(totalRows);
    //            DropDownListpage.Items.Add(attnd_report.Sheets[0].PageSize.ToString());
    //            attnd_report.Height = 100 + (10 * Convert.ToInt32(totalRows));
    //        }
    //        if (Convert.ToInt32(attnd_report.Sheets[0].RowCount) > 10)
    //        {
    //            DropDownListpage.Text = DropDownListpage.Items[DropDownListpage.Items.Count - 2].Text;
    //            attnd_report.Sheets[0].PageSize = int.Parse(DropDownListpage.Items[DropDownListpage.Items.Count - 2].Text);
    //            //   attnd_report.Height = 100 + (10 * Convert.ToInt32(totalRows));
    //            CalculateTotalPages();
    //        }


    //        pageset_pnl .Visible = true;


    //    }
    //    else
    //    {

    //        errlbl .Visible = false;
    //        pageset_pnl.Visible = false;
    //    }
    //}
    //protected void Radiowithoutheader_CheckedChanged(object sender, EventArgs e)
    //{
    //    int i = 0;
    //    errlbl .Visible = false;
    //    ddlpage.Items.Clear();
    //    int totrowcount = attnd_report.Sheets[0].RowCount;
    //    int pages = totrowcount / 25;
    //    int intialrow = 1;
    //    int remainrows = totrowcount % 25;
    //    if (attnd_report.Sheets[0].RowCount > 0)
    //    {
    //        int i5 = 0;
    //        ddlpage.Items.Insert(0, new System.Web.UI.WebControls.ListItem("", "0"));
    //        for (i = 1; i <= pages; i++)
    //        {
    //            i5 = i;

    //            ddlpage.Items.Insert(i, new System.Web.UI.WebControls.ListItem(i.ToString(), intialrow.ToString()));
    //            intialrow = intialrow + 25;
    //        }
    //        if (remainrows > 0)
    //        {
    //            i = i5 + 1;
    //            ddlpage.Items.Insert(i, new System.Web.UI.WebControls.ListItem(i.ToString(), intialrow.ToString()));
    //        }
    //    }
    //    if ((ddlpage.SelectedValue.ToString() == string.Empty) || (ddlpage.SelectedValue.ToString() == "0"))
    //    {
    //        for (i = 0; i < attnd_report.Sheets[0].RowCount; i++)
    //        {
    //            attnd_report.Sheets[0].Rows[i].Visible = true;
    //        }
    //        Double totalRows = 0;
    //        totalRows = Convert.ToInt32(attnd_report.Sheets[0].RowCount);
    //        Session["totalPages"] = (int)Math.Ceiling(totalRows / attnd_report.Sheets[0].PageSize);
    //        Buttontotal.Text = "Records : " + totalRows + "          Pages : " + Session["totalPages"];
    //        DropDownListpage.Items.Clear();
    //        if (totalRows >= 10)
    //        {
    //            attnd_report.Sheets[0].PageSize = Convert.ToInt32(totalRows);
    //            for (int k = 0; k < Convert.ToInt32(totalRows); k = k + 10)
    //            {
    //                DropDownListpage.Items.Add((k + 10).ToString());
    //            }
    //            DropDownListpage.Items.Add("Others");
    //            DropDownListpage.Text = DropDownListpage.Items[DropDownListpage.Items.Count - 2].Text;
    //            attnd_report.Height = 335;

    //        }
    //        else if (totalRows == 0)
    //        {
    //            DropDownListpage.Items.Add("0");
    //            attnd_report.Height = 100;
    //        }
    //        else
    //        {
    //            attnd_report.Sheets[0].PageSize = Convert.ToInt32(totalRows);
    //            DropDownListpage.Items.Add(attnd_report.Sheets[0].PageSize.ToString());
    //            attnd_report.Height = 100 + (10 * Convert.ToInt32(totalRows));
    //        }
    //        if (Convert.ToInt32(attnd_report.Sheets[0].RowCount) > 10)
    //        {
    //            DropDownListpage.Text = DropDownListpage.Items[DropDownListpage.Items.Count - 2].Text;
    //            attnd_report.Sheets[0].PageSize = int.Parse(DropDownListpage.Items[DropDownListpage.Items.Count - 2].Text);
    //            //  attnd_report.Height = 100 + (10 * Convert.ToInt32(totalRows));
    //            CalculateTotalPages();
    //        }
    //        pageset_pnl.Visible = false;
    //    }
    //    else
    //    {
    //        pageset_pnl.Visible = false;
    //    }
    //}

    //public void load_pageddl()
    //{
    //    int i = 0;
    //    errlbl.Visible = false;
    //    ddlpage.Items.Clear();
    //    int totrowcount = attnd_report.Sheets[0].RowCount;
    //    int pages = totrowcount / 25;
    //    int intialrow = 1;
    //    int remainrows = totrowcount % 25;
    //    if (attnd_report.Sheets[0].RowCount > 0)
    //    {
    //        int i5 = 0;
    //        ddlpage.Items.Insert(0, new System.Web.UI.WebControls.ListItem("", "0"));
    //        for (i = 1; i <= pages; i++)
    //        {
    //            i5 = i;

    //            ddlpage.Items.Insert(i, new System.Web.UI.WebControls.ListItem(i.ToString(), intialrow.ToString()));
    //            intialrow = intialrow + 25;
    //        }
    //        if (remainrows > 0)
    //        {
    //            i = i5 + 1;
    //            ddlpage.Items.Insert(i, new System.Web.UI.WebControls.ListItem(i.ToString(), intialrow.ToString()));
    //        }
    //    }

    //}

    protected void ddlpage_SelectedIndexChanged(object sender, EventArgs e)
    {
        //loadvalues_pagesetting();

        //if (RadioHeader.Checked == true)
        //{

        //    for (int i = 0; i < attnd_report.Sheets[0].RowCount; i++)
        //    {
        //        attnd_report.Sheets[0].Rows[i].Visible = false;
        //    }
        //    int start = Convert.ToInt32(ddlpage.SelectedValue.ToString());
        //    int end = start + 24;
        //    if (end >= attnd_report.Sheets[0].RowCount)
        //    {
        //        end = attnd_report.Sheets[0].RowCount;
        //    }
        //    int rowstart = attnd_report.Sheets[0].RowCount - Convert.ToInt32(start);
        //    int rowend = attnd_report.Sheets[0].RowCount - Convert.ToInt32(end);
        //    for (int i = start - 1; i < end; i++)
        //    {
        //        attnd_report.Sheets[0].Rows[i].Visible = true;
        //    }
        //    attnd_report.Sheets[0].ColumnHeader.Rows[0].Visible = true;
        //    attnd_report.Sheets[0].ColumnHeader.Rows[1].Visible = true;
        //    attnd_report.Sheets[0].ColumnHeader.Rows[2].Visible = true;
        //    attnd_report.Sheets[0].ColumnHeader.Rows[3].Visible = true;
        //    attnd_report.Sheets[0].ColumnHeader.Rows[4].Visible = true;
        //    attnd_report.Sheets[0].ColumnHeader.Rows[5].Visible = true;
        //    attnd_report.Sheets[0].ColumnHeader.Rows[6].Visible = true;
        //}
        //else if (Radiowithoutheader.Checked == true)
        //{

        //    for (int i = 0; i < attnd_report.Sheets[0].RowCount; i++)
        //    {
        //        attnd_report.Sheets[0].Rows[i].Visible = false;
        //    }
        //    int start = Convert.ToInt32(ddlpage.SelectedValue.ToString());
        //    int end = start + 24;
        //    if (end >= attnd_report.Sheets[0].RowCount)
        //    {
        //        end = attnd_report.Sheets[0].RowCount;
        //    }
        //    int rowstart = attnd_report.Sheets[0].RowCount - Convert.ToInt32(start);
        //    int rowend = attnd_report.Sheets[0].RowCount - Convert.ToInt32(end);
        //    for (int i = start - 1; i < end; i++)
        //    {
        //        attnd_report.Sheets[0].Rows[i].Visible = true;
        //    }
        //    if (Convert.ToInt32(ddlpage.SelectedValue.ToString()) == 1)
        //    {
        //        attnd_report.Sheets[0].ColumnHeader.Rows[0].Visible = true;
        //        attnd_report.Sheets[0].ColumnHeader.Rows[1].Visible = true;
        //        attnd_report.Sheets[0].ColumnHeader.Rows[2].Visible = true;
        //        attnd_report.Sheets[0].ColumnHeader.Rows[3].Visible = true;
        //        attnd_report.Sheets[0].ColumnHeader.Rows[4].Visible = true;
        //        attnd_report.Sheets[0].ColumnHeader.Rows[5].Visible = true;
        //        attnd_report.Sheets[0].ColumnHeader.Rows[6].Visible = true;
        //    }
        //    else
        //    {
        //        attnd_report.Sheets[0].ColumnHeader.Rows[0].Visible = false ;
        //        attnd_report.Sheets[0].ColumnHeader.Rows[1].Visible = false;
        //        attnd_report.Sheets[0].ColumnHeader.Rows[2].Visible = false;
        //        attnd_report.Sheets[0].ColumnHeader.Rows[3].Visible = false;
        //        attnd_report.Sheets[0].ColumnHeader.Rows[4].Visible = false;
        //        attnd_report.Sheets[0].ColumnHeader.Rows[5].Visible = false;
        //        attnd_report.Sheets[0].ColumnHeader.Rows[6].Visible = false;
        //    }

        //}
        //if ((ddlpage.SelectedValue.ToString() == string.Empty) || (ddlpage.SelectedValue.ToString() == "0"))
        //{
        //    attnd_report.Sheets[0].ColumnHeader.Rows[0].Visible = true;
        //    attnd_report.Sheets[0].ColumnHeader.Rows[1].Visible = true;
        //    attnd_report.Sheets[0].ColumnHeader.Rows[2].Visible = true;
        //    attnd_report.Sheets[0].ColumnHeader.Rows[3].Visible = true;
        //    attnd_report.Sheets[0].ColumnHeader.Rows[4].Visible = true;
        //    attnd_report.Sheets[0].ColumnHeader.Rows[5].Visible = true;
        //    attnd_report.Sheets[0].ColumnHeader.Rows[6].Visible = true;
        //    for (int i = 0; i < attnd_report.Sheets[0].RowCount; i++)
        //    {
        //        attnd_report.Sheets[0].Rows[i].Visible = true;
        //    }
        //    Double totalRows = 0;
        //    totalRows = Convert.ToInt32(attnd_report.Sheets[0].RowCount);
        //    Session["totalPages"] = (int)Math.Ceiling(totalRows / attnd_report.Sheets[0].PageSize);
        //    Buttontotal.Text = "Records : " + totalRows + "          Pages : " + Session["totalPages"];
        //    DropDownListpage.Items.Clear();
        //    if (totalRows >= 10)
        //    {
        //        attnd_report.Sheets[0].PageSize = Convert.ToInt32(totalRows);
        //        for (int k = 0; k < Convert.ToInt32(totalRows); k = k + 10)
        //        {
        //            DropDownListpage.Items.Add((k + 10).ToString());
        //        }
        //        DropDownListpage.Items.Add("Others");
        //        DropDownListpage.Text = DropDownListpage.Items[DropDownListpage.Items.Count - 2].Text;
        //        attnd_report.Height = 335;

        //    }
        //    else if (totalRows == 0)
        //    {
        //        DropDownListpage.Items.Add("0");
        //        attnd_report.Height = 100;
        //    }
        //    else
        //    {
        //        attnd_report.Sheets[0].PageSize = Convert.ToInt32(totalRows);
        //        DropDownListpage.Items.Add(attnd_report.Sheets[0].PageSize.ToString());
        //        attnd_report.Height = 100 + (10 * Convert.ToInt32(totalRows));
        //    }
        //    if (Convert.ToInt32(attnd_report.Sheets[0].RowCount) > 10)
        //    {
        //        DropDownListpage.Text = DropDownListpage.Items[DropDownListpage.Items.Count - 2].Text;
        //        attnd_report.Sheets[0].PageSize = int.Parse(DropDownListpage.Items[DropDownListpage.Items.Count - 2].Text);
        //        //  attnd_report.Height = 100 + (10 * Convert.ToInt32(totalRows));
        //        CalculateTotalPages();
        //    }
        //    pageset_pnl.Visible = false;
        //}
        //else
        //{
        //    pageset_pnl.Visible = false;

        //}

        errlbl.Visible = false;
        if (view_header == "0")
        {

            for (int i = 0; i < attnd_report.Sheets[0].RowCount; i++)
            {
                attnd_report.Sheets[0].Rows[i].Visible = false;
            }
            int start = Convert.ToInt32(ddlpage.SelectedValue.ToString());
            int end = start + 24;
            if (end >= attnd_report.Sheets[0].RowCount)
            {
                end = attnd_report.Sheets[0].RowCount;
            }
            int rowstart = attnd_report.Sheets[0].RowCount - Convert.ToInt32(start);
            int rowend = attnd_report.Sheets[0].RowCount - Convert.ToInt32(end);
            for (int i = start - 1; i < end; i++)
            {
                attnd_report.Sheets[0].Rows[i].Visible = true;
            }
            for (int row_cnt = 0; row_cnt < attnd_report.Sheets[0].ColumnHeader.RowCount; row_cnt++)
            {
                attnd_report.Sheets[0].ColumnHeader.Rows[row_cnt].Visible = true;
            }

        }
        else if (view_header == "1")
        {

            for (int i = 0; i < attnd_report.Sheets[0].RowCount; i++)
            {
                attnd_report.Sheets[0].Rows[i].Visible = false;
            }
            int start = Convert.ToInt32(ddlpage.SelectedValue.ToString());
            int end = start + 24;
            if (end >= attnd_report.Sheets[0].RowCount)
            {
                end = attnd_report.Sheets[0].RowCount;
            }
            int rowstart = attnd_report.Sheets[0].RowCount - Convert.ToInt32(start);
            int rowend = attnd_report.Sheets[0].RowCount - Convert.ToInt32(end);
            for (int i = start - 1; i < end; i++)
            {
                attnd_report.Sheets[0].Rows[i].Visible = true;
            }
            if (Convert.ToInt32(ddlpage.SelectedValue.ToString()) == 1)
            {
                for (int row_cnt = 0; row_cnt < attnd_report.Sheets[0].ColumnHeader.RowCount; row_cnt++)
                {
                    attnd_report.Sheets[0].ColumnHeader.Rows[row_cnt].Visible = true;
                }
            }
            else
            {
                for (int row_cnt = 0; row_cnt < attnd_report.Sheets[0].ColumnHeader.RowCount; row_cnt++)
                {
                    attnd_report.Sheets[0].ColumnHeader.Rows[row_cnt].Visible = false;
                }
            }
        }
        else
        {
            for (int i = 0; i < attnd_report.Sheets[0].RowCount; i++)
            {
                attnd_report.Sheets[0].Rows[i].Visible = false;
            }
            int start = Convert.ToInt32(ddlpage.SelectedValue.ToString());
            int end = start + 24;
            if (end >= attnd_report.Sheets[0].RowCount)
            {
                end = attnd_report.Sheets[0].RowCount;
            }
            int rowstart = attnd_report.Sheets[0].RowCount - Convert.ToInt32(start);
            int rowend = attnd_report.Sheets[0].RowCount - Convert.ToInt32(end);
            for (int i = start - 1; i < end; i++)
            {
                attnd_report.Sheets[0].Rows[i].Visible = true;
            }

            {
                for (int row_cnt = 0; row_cnt < attnd_report.Sheets[0].ColumnHeader.RowCount; row_cnt++)
                {
                    attnd_report.Sheets[0].ColumnHeader.Rows[row_cnt].Visible = false;
                }
            }
        }
        if ((ddlpage.SelectedValue.ToString() == string.Empty) || (ddlpage.SelectedValue.ToString() == "0"))
        {

            if (view_header == "1" || view_header == "0")
            {
                for (int row_cnt = 0; row_cnt < attnd_report.Sheets[0].ColumnHeader.RowCount; row_cnt++)
                {
                    attnd_report.Sheets[0].ColumnHeader.Rows[row_cnt].Visible = true;
                }
            }
            else
            {
                for (int row_cnt = 0; row_cnt < attnd_report.Sheets[0].ColumnHeader.RowCount; row_cnt++)
                {
                    attnd_report.Sheets[0].ColumnHeader.Rows[row_cnt].Visible = false;
                }
            }

            for (int i = 0; i < attnd_report.Sheets[0].RowCount; i++)
            {
                attnd_report.Sheets[0].Rows[i].Visible = true;
            }
            Double totalRows = 0;
            totalRows = Convert.ToInt32(attnd_report.Sheets[0].RowCount);
            Session["totalPages"] = (int)Math.Ceiling(totalRows / attnd_report.Sheets[0].PageSize);
            Buttontotal.Text = "Records : " + totalRows + "          Pages : " + Session["totalPages"];
            DropDownListpage.Items.Clear();
            if (totalRows >= 10)
            {
                attnd_report.Sheets[0].PageSize = Convert.ToInt32(totalRows);
                for (int k = 0; k < Convert.ToInt32(totalRows); k = k + 10)
                {
                    DropDownListpage.Items.Add((k + 10).ToString());
                }
                DropDownListpage.Items.Add("Others");
                DropDownListpage.Text = DropDownListpage.Items[DropDownListpage.Items.Count - 2].Text;
                attnd_report.Height = 335;
            }
            else if (totalRows == 0)
            {
                DropDownListpage.Items.Add("0");
                attnd_report.Height = 100;
            }
            else
            {
                attnd_report.Sheets[0].PageSize = Convert.ToInt32(totalRows);
                DropDownListpage.Items.Add(attnd_report.Sheets[0].PageSize.ToString());
                attnd_report.Height = 100 + (10 * Convert.ToInt32(totalRows));
            }
            if (Convert.ToInt32(attnd_report.Sheets[0].RowCount) > 10)
            {
                DropDownListpage.Text = DropDownListpage.Items[DropDownListpage.Items.Count - 2].Text;
                attnd_report.Sheets[0].PageSize = int.Parse(DropDownListpage.Items[DropDownListpage.Items.Count - 2].Text);
                //  attnd_report.Height = 100 + (10 * Convert.ToInt32(totalRows));
                CalculateTotalPages();
            }

            pageset_pnl.Visible = false;
        }
        else
        {
            pageset_pnl.Visible = false;
        }
        hat.Clear();
        hat.Add("college_code", Session["InternalCollegeCode"].ToString());
        hat.Add("form_name", "ovrall_attreport_perday.aspx");
        dsprint = dacces2.select_method("PROC_PRINT_MASTER_SETTINGS", hat, "sp");
        if (dsprint.Tables[0].Rows.Count > 0)

            if (dsprint.Tables[0].Rows.Count > 0)
            {
                view_footer = dsprint.Tables[0].Rows[0]["footer_flag_value"].ToString();
                view_header = dsprint.Tables[0].Rows[0]["header_flag_value"].ToString();
                view_footer_text = dsprint.Tables[0].Rows[0]["footer_name"].ToString();
            }
        if (view_footer_text != "")
        {
            if (view_footer == "0")
            {
                attnd_report.Sheets[0].Rows[(attnd_report.Sheets[0].RowCount - 1)].Visible = true;
                attnd_report.Sheets[0].Rows[(attnd_report.Sheets[0].RowCount - 2)].Visible = true;
                attnd_report.Sheets[0].Rows[(attnd_report.Sheets[0].RowCount - 3)].Visible = true;
            }
            else
            {
                if (ddlpage.Text != "")
                {
                    if (ddlpage.SelectedIndex != ddlpage.Items.Count - 1)
                    {
                        attnd_report.Sheets[0].Rows[(attnd_report.Sheets[0].RowCount - 1)].Visible = false;
                        attnd_report.Sheets[0].Rows[(attnd_report.Sheets[0].RowCount - 2)].Visible = false;
                        attnd_report.Sheets[0].Rows[(attnd_report.Sheets[0].RowCount - 3)].Visible = false;
                    }
                }
            }
        }
    }

    public void loadvalues_pagesetting()
    {
        //  try
        {

            attnd_report.Visible = true;
            btnprintmaster.Visible = true;
            btnxl.Visible = true;
            //Added by Srinath 27/2/2
            lblrptname.Visible = true;
            txtexcelname.Visible = true;
            //'----------------------- design


            attnd_report.Sheets[0].ColumnCount = 11;
            attnd_report.Sheets[0].RowCount = 0;

            //'----------------------------------------- Split the date
            date = txtFromDate.Text.ToString();
            string[] split_date = date.Split(new char[] { '/' });
            Atday = split_date[0].ToString();
            Atmnth = split_date[1].ToString();
            Atyr = split_date[2].ToString();
            todaydate = Atmnth + "/" + Atday + "/" + Atyr;
            DateTime input_date = Convert.ToDateTime(todaydate.ToString());
            date_concat = "'" + date + "'";
            MthYear = (Convert.ToInt32(Atyr) * 12) + Convert.ToInt32(Atmnth);
            //'---------------------------------------------
            attnd_report.Sheets[0].ColumnHeader.RowCount = 0;

            {

                //=============================0n 02/07/12
                has.Clear();
                has.Add("college_code", Session["InternalCollegeCode"].ToString());
                has.Add("form_name", "ovrall_attreport_perday.aspx");
                dsprint = dacces2.select_method("PROC_PRINT_MASTER_SETTINGS", has, "sp");
                //===========================================

                //======================0n 02/07/12 PRABHA
                if (dsprint.Tables[0].Rows.Count > 0)
                {
                    if (dsprint.Tables[0].Rows[0]["new_header_name"].ToString() != null && dsprint.Tables[0].Rows[0]["new_header_name"].ToString() != "")
                    {
                        //attnd_report.Sheets[0].ColumnHeader.Cells[6, 0].Border.BorderColorBottom = Color.White;
                        new_header_string = dsprint.Tables[0].Rows[0]["new_header_name"].ToString();
                        new_header_string_split = new_header_string.Split(',');
                        //attnd_report.Sheets[0].SheetCorner.RowCount = attnd_report.Sheets[0].SheetCorner.RowCount + new_header_string_split.GetUpperBound(0) + 1;
                    }
                }
                //=====================================


                attnd_report.Sheets[0].ColumnHeader.RowCount++;

                attnd_report.Sheets[0].ColumnHeader.Cells[(attnd_report.Sheets[0].ColumnHeader.RowCount - 1), 0].Text = "S.No";
                attnd_report.Sheets[0].ColumnHeader.Cells[(attnd_report.Sheets[0].ColumnHeader.RowCount - 1), 1].Text = "Dept";
                attnd_report.Sheets[0].ColumnHeader.Cells[(attnd_report.Sheets[0].ColumnHeader.RowCount - 1), 2].Text = "Year";
                attnd_report.Sheets[0].ColumnHeader.Cells[(attnd_report.Sheets[0].ColumnHeader.RowCount - 1), 3].Text = "Strength";
                attnd_report.Sheets[0].ColumnHeader.Cells[(attnd_report.Sheets[0].ColumnHeader.RowCount - 1), 4].Text = "P";
                attnd_report.Sheets[0].ColumnHeader.Cells[(attnd_report.Sheets[0].ColumnHeader.RowCount - 1), 5].Text = "Total(L-A-S-OD-P)";
                attnd_report.Sheets[0].ColumnHeader.Cells[(attnd_report.Sheets[0].ColumnHeader.RowCount - 1), 6].Text = "L";
                attnd_report.Sheets[0].ColumnHeader.Cells[(attnd_report.Sheets[0].ColumnHeader.RowCount - 1), 7].Text = "A";
                attnd_report.Sheets[0].ColumnHeader.Cells[(attnd_report.Sheets[0].ColumnHeader.RowCount - 1), 8].Text = "S";
                attnd_report.Sheets[0].ColumnHeader.Cells[(attnd_report.Sheets[0].ColumnHeader.RowCount - 1), 9].Text = "OD";
                attnd_report.Sheets[0].ColumnHeader.Cells[(attnd_report.Sheets[0].ColumnHeader.RowCount - 1), 10].Text = "SOD";
                attnd_report.Sheets[0].ColumnHeader.Cells[(attnd_report.Sheets[0].ColumnHeader.RowCount - 1), 11].Text = "Remarks";


                attnd_report.Sheets[0].Columns[0].HorizontalAlign = HorizontalAlign.Left;
                attnd_report.Sheets[0].Columns[1].HorizontalAlign = HorizontalAlign.Left;
                //'--------------------------------------------------------------- Query for select degree
                string strsemval = "select * from seminfo ";
                DataSet dssem = d2.select_method_wo_parameter(strsemval, "Text");
                DataView dvsem = new DataView();
                strDegree = "select * from degree where college_code='" + Session["InternalCollegeCode"] + "' ORDER BY DEGREE_CODE";
                con_deg.Close();
                con_deg.Open();
                SqlCommand cmddeg = new SqlCommand(strDegree, con_deg);
                SqlDataReader drdeg;
                drdeg = cmddeg.ExecuteReader();
                while (drdeg.Read())
                {
                    // temp_count = 0;
                    rowhead++;
                    fflag = false;
                    if (drdeg.HasRows == true)
                    {
                        acronym = drdeg["Acronym"].ToString();
                        deg_code = drdeg["Degree_Code"].ToString();
                        hat.Clear();
                        hat.Add("degree_val", deg_code);
                        hat.Add("input_date", input_date);
                        ds = dacces2.select_method("bind_degree_detail", hat, "sp");

                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            for (count = 0; count < ds.Tables[0].Rows.Count; count++)
                            {
                                batch_year = ds.Tables[0].Rows[count]["Batch Year"].ToString();
                                sections = ds.Tables[0].Rows[count]["Section"].ToString();
                                current_sem = ds.Tables[0].Rows[count]["Current Semester"].ToString();
                                noofhrs = int.Parse(ds.Tables[0].Rows[count]["No Of Hours"].ToString());
                                first_hrs = int.Parse(ds.Tables[0].Rows[count]["First Half"].ToString());
                                sec_hrs = int.Parse(ds.Tables[0].Rows[count]["Second Half"].ToString());

                                fflag = true;
                                norecflag = true;
                                attnd_report.Sheets[0].RowCount++;
                                inirowcnt = attnd_report.Sheets[0].RowCount - 1;
                                //      temp_count++;
                                //Added by srinath 1/8/2014 
                                dssem.Tables[0].DefaultView.RowFilter = " batch_year='" + batch_year + "' and degree_code='" + deg_code + "' and semester='" + current_sem + "'";
                                dvsem = dssem.Tables[0].DefaultView;
                                string endate = "";
                                string startdate = "";
                                if (dvsem.Count > 0)
                                {
                                    startdate = dvsem[0]["start_date"].ToString();
                                    endate = dvsem[0]["end_date"].ToString();
                                    DateTime dtstart = Convert.ToDateTime(startdate);
                                    DateTime dtendate = Convert.ToDateTime(endate);
                                    if (dtstart <= input_date && dtendate >= input_date)
                                    {
                                        if (count == 0)
                                        {
                                            attnd_report.Sheets[0].Cells[(attnd_report.Sheets[0].RowCount - 1), 0].Text = acronym;
                                        }

                                        if (sections.ToString() != string.Empty)
                                        {
                                            getsec = "-" + sections.ToString();
                                        }
                                        else
                                        {
                                            getsec = "";
                                        }

                                        if (Convert.ToInt32(current_sem) % 2 == 0)
                                        {
                                            roman_val = sem_roman(int.Parse(current_sem) / 2) + "Year" + getsec;
                                            attnd_report.Sheets[0].SetText(attnd_report.Sheets[0].RowCount - 1, 1, roman_val);
                                        }
                                        else
                                        {
                                            roman_val = sem_roman(((int.Parse(current_sem)) + 1) / 2) + "Year" + getsec;
                                            attnd_report.Sheets[0].SetText(attnd_report.Sheets[0].RowCount - 1, 1, roman_val);

                                        }
                                        findhours();
                                    }
                                }
                            }
                        }
                        if (fflag == true)
                        {
                            attnd_report.Sheets[0].RowCount = attnd_report.Sheets[0].RowCount + 2;
                            attnd_report.Sheets[0].Cells[attnd_report.Sheets[0].RowCount - 2, 1].Font.Bold = true;
                            attnd_report.Sheets[0].Cells[attnd_report.Sheets[0].RowCount - 1, 1].Font.Bold = true;
                            attnd_report.Sheets[0].SetText(attnd_report.Sheets[0].RowCount - 2, 1, "Total");
                            attnd_report.Sheets[0].SetText(attnd_report.Sheets[0].RowCount - 1, 1, "Percentage");
                            //--------------------total


                            //hat.Clear();
                            //hat.Add("monthyear", MthYear);
                            //hat.Add("degree_code", deg_code);
                            //hat.Add("input_date", input_date);
                            attnd_report.Sheets[0].Cells[(attnd_report.Sheets[0].RowCount - 2), 2].Text = tot_strength.ToString();
                            fin_str += tot_strength;
                            attnd_report.Sheets[0].Cells[(attnd_report.Sheets[0].RowCount - 2), 3].Text = temp_tot_pres.ToString();
                            fin_pres += temp_tot_pres;
                            attnd_report.Sheets[0].Cells[(attnd_report.Sheets[0].RowCount - 2), 4].Text = tot_tot.ToString();
                            fin_tot += tot_tot;
                            attnd_report.Sheets[0].Cells[(attnd_report.Sheets[0].RowCount - 2), 5].Text = temp_tot_lea.ToString();
                            fin_lev += temp_tot_lea;
                            attnd_report.Sheets[0].Cells[(attnd_report.Sheets[0].RowCount - 2), 6].Text = temp_tot_abs.ToString();
                            fin_abs += temp_tot_abs;
                            attnd_report.Sheets[0].Cells[(attnd_report.Sheets[0].RowCount - 2), 7].Text = temp_tot_sus.ToString();
                            fin_sus += temp_tot_sus;
                            attnd_report.Sheets[0].Cells[(attnd_report.Sheets[0].RowCount - 2), 8].Text = temp_tot_od.ToString();
                            fin_od += temp_tot_od;
                            attnd_report.Sheets[0].Cells[(attnd_report.Sheets[0].RowCount - 2), 9].Text = temp_tot_sod.ToString();
                            fin_sod += temp_tot_sod;
                            //---------------------percentage
                            double temp = 0;
                            temp = double.Parse((((temp_tot_lea + temp_tot_abs) / tot_strength) * 100).ToString());
                            if (temp.ToString() == "NaN")
                            {
                                temp = 0;
                            }

                            over_all_per += temp;
                            attnd_report.Sheets[0].Cells[(attnd_report.Sheets[0].RowCount - 1), 2].Text = String.Format("{0:0.00}", temp);



                            temp = double.Parse(((temp_tot_pres * 100) / tot_strength).ToString());
                            if (temp.ToString() == "NaN")
                            {
                                temp = 0;
                            }
                            attnd_report.Sheets[0].Cells[(attnd_report.Sheets[0].RowCount - 1), 3].Text = String.Format("{0:0.00}", temp);



                            temp = double.Parse(((tot_tot * 100) / tot_strength).ToString());
                            if (temp.ToString() == "NaN")
                            {
                                temp = 0;
                            }

                            attnd_report.Sheets[0].Cells[(attnd_report.Sheets[0].RowCount - 1), 4].Text = String.Format("{0:0.00}", temp);
                            temp = double.Parse(((temp_tot_lea * 100) / tot_strength).ToString());

                            if (temp.ToString() == "NaN")
                            {
                                temp = 0;
                            }

                            attnd_report.Sheets[0].Cells[(attnd_report.Sheets[0].RowCount - 1), 5].Text = String.Format("{0:0.00}", temp);
                            temp = double.Parse(((temp_tot_abs * 100) / tot_strength).ToString());

                            if (temp.ToString() == "NaN")
                            {
                                temp = 0;
                            }

                            attnd_report.Sheets[0].Cells[(attnd_report.Sheets[0].RowCount - 1), 6].Text = String.Format("{0:0.00}", temp);
                            temp = double.Parse(((temp_tot_sus * 100) / tot_strength).ToString());

                            if (temp.ToString() == "NaN")
                            {
                                temp = 0;
                            }

                            attnd_report.Sheets[0].Cells[(attnd_report.Sheets[0].RowCount - 1), 7].Text = String.Format("{0:0.00}", temp);
                            temp = double.Parse(((temp_tot_od * 100) / tot_strength).ToString());

                            if (temp.ToString() == "NaN")
                            {
                                temp = 0;
                            }

                            attnd_report.Sheets[0].Cells[(attnd_report.Sheets[0].RowCount - 1), 8].Text = String.Format("{0:0.00}", temp);
                            temp = double.Parse(((temp_tot_sod * 100) / tot_strength).ToString());

                            if (temp.ToString() == "NaN")
                            {
                                temp = 0;
                            }

                            attnd_report.Sheets[0].Cells[(attnd_report.Sheets[0].RowCount - 1), 9].Text = String.Format("{0:0.00}", temp);

                            temp_tot = 0;
                            temp_tot_pres = 0;
                            temp_tot_lea = 0;
                            temp_tot_abs = 0;
                            temp_tot_sus = 0;
                            temp_tot_od = 0;
                            temp_tot_sod = 0;
                            tot_strength = 0;
                            tot_tot = 0;
                            attnd_report.Sheets[0].SpanModel.Add((attnd_report.Sheets[0].RowCount - (count + 2)), 0, (count + 2), 1);

                            attnd_report.Sheets[0].RowHeaderSpanModel.Add((attnd_report.Sheets[0].RowCount - (count + 2)), 0, (count + 2), 1);
                            attnd_report.Sheets[0].RowHeader.Cells[(attnd_report.Sheets[0].RowCount - (count + 2)), 0].Text = rowhead.ToString();
                        }


                    }   //'---- end while(drbind)

                }   //'------ end hasrows(drdeg)


                attnd_report.Sheets[0].RowCount = attnd_report.Sheets[0].RowCount + 3;
                attnd_report.Sheets[0].SetText(attnd_report.Sheets[0].RowCount - 2, 0, "Grand Total");
                attnd_report.Sheets[0].SetText(attnd_report.Sheets[0].RowCount - 1, 0, "Total Percentage");
                attnd_report.Sheets[0].SpanModel.Add((attnd_report.Sheets[0].RowCount - 1), 0, 1, 2);
                attnd_report.Sheets[0].SpanModel.Add((attnd_report.Sheets[0].RowCount - 2), 0, 1, 2);
                attnd_report.Sheets[0].Rows[attnd_report.Sheets[0].RowCount - 2].Font.Bold = true;
                attnd_report.Sheets[0].Rows[attnd_report.Sheets[0].RowCount - 1].Font.Bold = true;


                attnd_report.Sheets[0].Cells[(attnd_report.Sheets[0].RowCount - 2), 2].Text = (fin_str).ToString();
                attnd_report.Sheets[0].Cells[(attnd_report.Sheets[0].RowCount - 2), 3].Text = (fin_pres).ToString();
                attnd_report.Sheets[0].Cells[(attnd_report.Sheets[0].RowCount - 2), 4].Text = (fin_tot).ToString();
                attnd_report.Sheets[0].Cells[(attnd_report.Sheets[0].RowCount - 2), 5].Text = (fin_lev).ToString();
                attnd_report.Sheets[0].Cells[(attnd_report.Sheets[0].RowCount - 2), 6].Text = (fin_abs).ToString();
                attnd_report.Sheets[0].Cells[(attnd_report.Sheets[0].RowCount - 2), 7].Text = (fin_sus).ToString();
                attnd_report.Sheets[0].Cells[(attnd_report.Sheets[0].RowCount - 2), 8].Text = (fin_od).ToString();
                attnd_report.Sheets[0].Cells[(attnd_report.Sheets[0].RowCount - 2), 9].Text = (fin_sod).ToString();

                attnd_report.Sheets[0].Cells[(attnd_report.Sheets[0].RowCount - 1), 2].Text = String.Format("{0:0.00}", over_all_per);

                attnd_report.Sheets[0].RowHeader.Cells[(attnd_report.Sheets[0].RowCount - 1), 0].Text = " ";
                attnd_report.Sheets[0].RowHeader.Cells[(attnd_report.Sheets[0].RowCount - 2), 0].Text = " ";
                attnd_report.Sheets[0].RowHeader.Cells[(attnd_report.Sheets[0].RowCount - 3), 0].Text = " ";

                attnd_report.Sheets[0].SpanModel.Add((attnd_report.Sheets[0].RowCount - 3), 0, 1, 11);
                attnd_report.Sheets[0].SpanModel.Add((attnd_report.Sheets[0].RowCount - 1), 2, 1, 9);


            }     //'------ end while (drdeg)

        }
        //   catch(Exception e)
        {

        }
        if (norecflag == false)
        {
            pageset_pnl.Visible = false;
            errlbl.Visible = false;
            // pagesetpanel.Visible = false;
            attnd_report.Visible = false;
            btnprintmaster.Visible = false;
            btnxl.Visible = false;
            //Added by Srinath 27/2/2
            lblrptname.Visible = false;
            txtexcelname.Visible = false;
        }
        else
        {
            pageset_pnl.Visible = false;
            errlbl.Visible = true;
            attnd_report.Visible = true;
            btnxl.Visible = true;
            //Added by Srinath 27/2/2
            lblrptname.Visible = false;
            txtexcelname.Visible = false;
            //  pagesetpanel.Visible = true;
            //setheader_print();
        }

        if (Convert.ToInt32(attnd_report.Sheets[0].RowCount) > 2)
        {
            pageset_pnl.Visible = false;
            attnd_report.Visible = true;
            btnprintmaster.Visible = true;
            Double totalRows = 0;
            totalRows = Convert.ToInt32(attnd_report.Sheets[0].RowCount);
            DropDownListpage.Items.Clear();
            if (totalRows >= 10)
            {
                attnd_report.Sheets[0].PageSize = 10;
                for (int k = 0; k < Convert.ToInt32(totalRows); k = k + 10)
                {
                    DropDownListpage.Items.Add((k + 10).ToString());
                }
                DropDownListpage.Items.Add("Others");
                attnd_report.Height = 10 + (10 * Convert.ToInt32(totalRows));
                attnd_report.VerticalScrollBarPolicy = FarPoint.Web.Spread.ScrollBarPolicy.AsNeeded;
                attnd_report.HorizontalScrollBarPolicy = FarPoint.Web.Spread.ScrollBarPolicy.AsNeeded;

            }
            else if (totalRows == 0)
            {
                DropDownListpage.Items.Add("0");
                attnd_report.Height = 200;
            }
            else
            {
                attnd_report.Sheets[0].PageSize = Convert.ToInt32(totalRows);
                DropDownListpage.Items.Add(attnd_report.Sheets[0].PageSize.ToString());

            }
            if (Convert.ToInt32(attnd_report.Sheets[0].RowCount) > 10)
            {
                DropDownListpage.Text = DropDownListpage.Items[DropDownListpage.Items.Count - 2].Text;
                attnd_report.Sheets[0].PageSize = int.Parse(DropDownListpage.Items[DropDownListpage.Items.Count - 2].Text);
                CalculateTotalPages();
            }
            Session["totalPages"] = (int)Math.Ceiling(totalRows / attnd_report.Sheets[0].PageSize);
            Buttontotal.Text = "Records : " + totalRows + "          Pages : " + Session["totalPages"];

        }
    }

    protected override void Render(System.Web.UI.HtmlTextWriter writer)
    {

        Control cntUpdateBtn = attnd_report.FindControl("Update");
        Control cntCancelBtn = attnd_report.FindControl("Cancel");
        Control cntedit = attnd_report.FindControl("Edit");
        //Control cntCopyBtn = attnd_report.FindControl("Copy");
        //Control cntCutBtn = attnd_report.FindControl("Clear");
        //Control cntPasteBtn = attnd_report.FindControl("Paste");
        //Control cntPagePrintBtn = attnd_report.FindControl("Print");

        if ((cntUpdateBtn != null))
        {

            TableCell tc = (TableCell)cntUpdateBtn.Parent;
            TableRow tr = (TableRow)tc.Parent;

            tr.Cells.Remove(tc);

            tc = (TableCell)cntCancelBtn.Parent;
            tr.Cells.Remove(tc);

            tc = (TableCell)cntedit.Parent;
            tr.Cells.Remove(tc);

            //tc = (TableCell)cntCutBtn.Parent;
            //tr.Cells.Remove(tc);

            //tc = (TableCell)cntPasteBtn.Parent;
            //tr.Cells.Remove(tc);
            //tc = (TableCell)cntPagePrintBtn.Parent;
            //tr.Cells.Remove(tc);

        }

        base.Render(writer);
    }

    protected void btnxl_Click(object sender, EventArgs e)
    {

        //Modified by Srinath 27/2/2013
        //string appPath = HttpContext.Current.Server.MapPath("~");
        //string print = "";
        //if (appPath != "")
        //{
        //    int i = 1;
        //    appPath = appPath.Replace("\\", "/");
        //e:
        try
        {
            string reportname = txtexcelname.Text;

            if (reportname.ToString().Trim() != "")
            {
                dacces2.printexcelreport(attnd_report, reportname);
                txtexcelname.Text = "";
            }
            else
            {
                errlbl.Text = "Please Enter Your Report Name";
                errlbl.Visible = true;
            }
            //print = "Overall Attendance Report Per Day" + i;
            ////attnd_report.SaveExcel(appPath + "/" + print + ".xls", FarPoint.Web.Spread.Model.IncludeHeaders.BothCustomOnly); //Print the sheet
            ////Aruna on 26feb2013============================
            //string szPath = appPath + "/Report/";
            //string szFile = print + ".xls"; // + DateTime.Now.ToString("yyyyMMddHHmmss")

            //attnd_report.SaveExcel(szPath + szFile, FarPoint.Web.Spread.Model.IncludeHeaders.ColumnHeadersCustomOnly);
            //Response.Clear();
            //Response.ClearHeaders();
            //Response.ClearContent();
            //Response.AddHeader("Content-Disposition", "attachment; filename=" + szFile);
            //Response.ContentType = "application/vnd.ms-excel";
            //Response.Flush();
            //Response.WriteFile(szPath + szFile);
            ////=============================================

        }
        catch
        {
            //i++;
            //goto e;

        }
        // }
        //ScriptManager.RegisterStartupScript(this, typeof(Page), UniqueID, "alert('" + print + ".xls" + " saved in" + " " + appPath + "/Report" + " successfully')", true);

    }

    //===============Hided by Manikandan 18/05/2013

    //protected void btn_print_setting_Click(object sender, EventArgs e)
    //{
    //    string batch = "", sections = "", semester = "", degreecode = "", subcolumntext = "", strsec = "";
    //    Boolean child_flag = false;
    //    int sec_index = 0, sem_index = 0;
    //    string clmnheadrname = "";

    //    Session["page_redirect_value"] = txtFromDate.Text+","+ ddlcollege.SelectedIndex.ToString();
    //    if (btnflag == false)
    //    {
    //        btnGo_Click(sender, e);
    //    }
    //    int total_clmn_count = attnd_report.Sheets[0].ColumnCount;
    //    //if (ddlr_type.SelectedIndex == 1)
    //    //{
    //        for (int srtcnt = 0; srtcnt < total_clmn_count; srtcnt++)
    //        {
    //            if (attnd_report.Sheets[0].ColumnHeader.Cells[attnd_report.Sheets[0].ColumnHeader.RowCount - 1, srtcnt].Text != "")
    //            {
    //                subcolumntext = "";
    //                if (clmnheadrname == "")
    //                {
    //                    clmnheadrname = attnd_report.Sheets[0].ColumnHeader.Cells[attnd_report.Sheets[0].ColumnHeader.RowCount - 1, srtcnt].Text;
    //                }
    //                else
    //                {
    //                    if (child_flag == false)
    //                    {
    //                        clmnheadrname = clmnheadrname + "," + attnd_report.Sheets[0].ColumnHeader.Cells[attnd_report.Sheets[0].ColumnHeader.RowCount - 1, srtcnt].Text ;
    //                    }
    //                    else
    //                    {
    //                        clmnheadrname = clmnheadrname + "$)," + attnd_report.Sheets[0].ColumnHeader.Cells[attnd_report.Sheets[0].ColumnHeader.RowCount - 1, srtcnt].Text;
    //                    }

    //                }
    //                child_flag = false;
    //            }

    //            else
    //            {
    //                child_flag = true;
    //                if (subcolumntext == "")
    //                {
    //                    for (int te = srtcnt - 1; te <= srtcnt; te++)
    //                    {
    //                        if (te == srtcnt - 1)
    //                        {
    //                            clmnheadrname = clmnheadrname + "* ($" + attnd_report.Sheets[0].ColumnHeader.Cells[attnd_report.Sheets[0].ColumnHeader.RowCount - 1, te].Text;
    //                            subcolumntext = clmnheadrname + "* ($" + attnd_report.Sheets[0].ColumnHeader.Cells[attnd_report.Sheets[0].ColumnHeader.RowCount - 1, te].Text;
    //                        }
    //                        else
    //                        {
    //                            clmnheadrname = clmnheadrname + "$" + attnd_report.Sheets[0].ColumnHeader.Cells[attnd_report.Sheets[0].ColumnHeader.RowCount - 1, te].Text;
    //                            subcolumntext = clmnheadrname + "$" + attnd_report.Sheets[0].ColumnHeader.Cells[attnd_report.Sheets[0].ColumnHeader.RowCount - 1, te].Text;

    //                        }
    //                    }
    //                }
    //                else
    //                {
    //                    subcolumntext = subcolumntext + "$" + attnd_report.Sheets[0].ColumnHeader.Cells[attnd_report.Sheets[0].ColumnHeader.RowCount - 1, srtcnt].Text;
    //                    clmnheadrname = clmnheadrname + "$" + attnd_report.Sheets[0].ColumnHeader.Cells[attnd_report.Sheets[0].ColumnHeader.RowCount - 1, srtcnt].Text;
    //                }
    //            }
    //        }


    //        Response.Redirect("Print_Master_Setting_New.aspx?ID=" + clmnheadrname.ToString() + ":" + "Ovrall_Attreport_perday.aspx" + ":" + ":" + "Over All Attendance For Pariticular Day");

    //   // }

    //}



    //public void setheader_print()
    //{
    //    // attnd_report.Sheets[0].RemoveSpanCell
    //    //================header
    //    temp_count = 0;


    //    MyImg mi = new MyImg();
    //    mi.ImageUrl = "~/images/10BIT001.jpeg";
    //    mi.ImageUrl = "Handler/Handler2.ashx?";
    //    MyImg mi2 = new MyImg();
    //    mi2.ImageUrl = "~/images/10BIT001.jpeg";
    //    mi2.ImageUrl = "Handler/Handler5.ashx?";
    //    final_print_col_cnt = 0;
    //    for (col_count = 0; col_count < attnd_report.Sheets[0].ColumnCount; col_count++)
    //    {
    //        if (attnd_report.Sheets[0].Columns[col_count].Visible == true)
    //        {
    //            final_print_col_cnt++;
    //        }
    //    }
    //    if (final_print_col_cnt == 1)
    //    {
    //        for (col_count = 0; col_count < attnd_report.Sheets[0].ColumnCount; col_count++)
    //        {
    //            if (attnd_report.Sheets[0].Columns[col_count].Visible == true)
    //            {
    //                // one_column();
    //                //more_column();
    //                break;
    //            }
    //        }

    //    }

    //    else if (final_print_col_cnt == 2)
    //    {
    //        for (col_count = 0; col_count < attnd_report.Sheets[0].ColumnCount; col_count++)
    //        {
    //            if (attnd_report.Sheets[0].Columns[col_count].Visible == true)
    //            {
    //                if (temp_count == 0)
    //                {
    //                    start_column = col_count;
    //                    //   attnd_report.Sheets[0].ColumnHeader.Columns[col_count].Width = 100;//
    //                    attnd_report.Sheets[0].ColumnHeaderSpanModel.Add(0, col_count, (attnd_report.Sheets[0].ColumnHeader.RowCount - 2), 1);
    //                    attnd_report.Sheets[0].ColumnHeader.Cells[0, col_count].CellType = mi;
    //                    attnd_report.Sheets[0].ColumnHeader.Cells[0, col_count].Border.BorderColorRight = Color.White;
    //                }
    //                else
    //                {
    //                    //  one_column();
    //                    //more_column();
    //                    for (int row_cnt = 0; row_cnt < attnd_report.Sheets[0].ColumnHeader.RowCount - 1; row_cnt++)
    //                    {
    //                        attnd_report.Sheets[0].ColumnHeader.Cells[row_cnt, col_count].Border.BorderColorLeft = Color.White;
    //                    }
    //                }
    //                temp_count++;
    //                if (temp_count == 2)
    //                {
    //                    break;
    //                }
    //            }
    //        }
    //    }
    //    else if (final_print_col_cnt == 3)
    //    {
    //        for (col_count = 0; col_count < attnd_report.Sheets[0].ColumnCount; col_count++)
    //        {
    //            if (attnd_report.Sheets[0].Columns[col_count].Visible == true)
    //            {
    //                if (temp_count == 0)
    //                {
    //                    start_column = col_count;
    //                    //   attnd_report.Sheets[0].ColumnHeader.Columns[col_count].Width = 100;//
    //                    attnd_report.Sheets[0].ColumnHeaderSpanModel.Add(0, col_count,7, 1);
    //                    attnd_report.Sheets[0].ColumnHeader.Cells[0, col_count].CellType = mi;
    //                    attnd_report.Sheets[0].ColumnHeader.Cells[0, col_count].Border.BorderColorRight = Color.White;
    //                }
    //                else if (temp_count == 1)
    //                {
    //                    // one_column();
    //                    //more_column();
    //                    for (int row_cnt = 0; row_cnt < attnd_report.Sheets[0].ColumnHeader.RowCount - 1; row_cnt++)
    //                    {
    //                        attnd_report.Sheets[0].ColumnHeader.Cells[row_cnt, col_count].Border.BorderColorLeft = Color.White;
    //                        attnd_report.Sheets[0].ColumnHeader.Cells[row_cnt, col_count].Border.BorderColorRight = Color.White;
    //                    }
    //                }
    //                else if (temp_count == 2)
    //                {
    //                    if (isonumber != string.Empty)
    //                    {
    //                        attnd_report.Sheets[0].ColumnHeader.Cells[0, col_count].Text = "ISO CODE:" + isonumber;
    //                        attnd_report.Sheets[0].ColumnHeaderSpanModel.Add(1, col_count,6, 1);
    //                        attnd_report.Sheets[0].ColumnHeader.Cells[1, col_count].CellType = mi2;
    //                        attnd_report.Sheets[0].ColumnHeader.Columns[col_count].Width = 150;
    //                        attnd_report.Sheets[0].ColumnHeader.Cells[1, col_count].Border.BorderColorRight = Color.Black;
    //                    }
    //                    else
    //                    {
    //                        attnd_report.Sheets[0].ColumnHeaderSpanModel.Add(0, col_count, 7, 1);
    //                        attnd_report.Sheets[0].ColumnHeader.Cells[0, col_count].CellType = mi2;
    //                        attnd_report.Sheets[0].ColumnHeader.Columns[col_count].Width = 150;
    //                        attnd_report.Sheets[0].ColumnHeader.Cells[0, col_count].Border.BorderColorRight = Color.Black;
    //                    }
    //                    //attnd_report.Sheets[0].ColumnHeaderSpanModel.Add(0, col_count, (attnd_report.Sheets[0].ColumnHeader.RowCount - 2), 1);
    //                    //attnd_report.Sheets[0].ColumnHeader.Cells[0, col_count].CellType = mi2;
    //                    //attnd_report.Sheets[0].ColumnHeader.Cells[0, col_count].Border.BorderColorRight = Color.White;
    //                }
    //                temp_count++;
    //                if (temp_count == 3)
    //                {
    //                    break;
    //                }
    //            }
    //        }

    //    }
    //    else//-----------column count more than 3
    //    {
    //        for (col_count = 0; col_count < attnd_report.Sheets[0].ColumnCount; col_count++)
    //        {
    //            if (attnd_report.Sheets[0].Columns[col_count].Visible == true)
    //            {
    //                if (temp_count == 0)
    //                {
    //                    start_column = col_count;
    //                    attnd_report.Sheets[0].ColumnHeaderSpanModel.Add(0, col_count, (7), 1);
    //                    attnd_report.Sheets[0].ColumnHeader.Cells[0, col_count].CellType = mi;
    //                    attnd_report.Sheets[0].ColumnHeader.Cells[0, col_count].Border.BorderColorRight = Color.White;
    //                    if (dsprint.Tables[0].Rows.Count > 0)
    //                    {
    //                        if (dsprint.Tables[0].Rows[0]["header_align_index"].ToString() != "")
    //                        {
    //                            attnd_report.Sheets[0].ColumnHeader.Cells[0, col_count].Border.BorderColorBottom = Color.White;
    //                        }
    //                    }
    //                }

    //                end_column = col_count;

    //                temp_count++;
    //                if (final_print_col_cnt == temp_count)
    //                {
    //                    break;
    //                }
    //            }
    //        }

    //        if (isonumber != string.Empty)
    //        {
    //            attnd_report.Sheets[0].ColumnHeader.Cells[0, end_column - 1].Text = "ISO CODE:";// +isonumber;
    //            attnd_report.Sheets[0].ColumnHeader.Cells[0, end_column].Text = isonumber;
    //            attnd_report.Sheets[0].ColumnHeader.Cells[0, end_column - 1].Border.BorderColorRight = Color.White;
    //            attnd_report.Sheets[0].ColumnHeader.Cells[0, end_column].Border.BorderColorLeft = Color.White;
    //            attnd_report.Sheets[0].ColumnHeader.Cells[0, end_column].HorizontalAlign = HorizontalAlign.Left;
    //            attnd_report.Sheets[0].ColumnHeader.Cells[0, end_column].Border.BorderColorBottom = Color.White;
    //            attnd_report.Sheets[0].ColumnHeaderSpanModel.Add(1, end_column, (6), 1);
    //            attnd_report.Sheets[0].ColumnHeader.Cells[1, end_column].CellType = mi2;
    //            attnd_report.Sheets[0].ColumnHeader.Columns[end_column].Width = 150;
    //            attnd_report.Sheets[0].ColumnHeader.Cells[1, end_column].Border.BorderColorTop = Color.White;
    //            attnd_report.Sheets[0].ColumnHeader.Cells[1, end_column].Border.BorderColorRight = Color.Black;
    //            attnd_report.Sheets[0].ColumnHeader.Cells[0, end_column - 1].Border.BorderColorBottom = Color.White;
    //            attnd_report.Sheets[0].ColumnHeader.Cells[1, end_column - 1].Border.BorderColorTop = Color.White;
    //            attnd_report.Sheets[0].ColumnHeader.Cells[1, end_column - 1].Border.BorderColorBottom = Color.White;
    //            if (dsprint.Tables[0].Rows.Count > 0)
    //            {
    //                if (dsprint.Tables[0].Rows[0]["header_align_index"].ToString() != "")
    //                {
    //                    attnd_report.Sheets[0].ColumnHeader.Cells[1, end_column].Border.BorderColorBottom = Color.White;
    //                }
    //            }
    //            attnd_report.Sheets[0].ColumnHeader.Cells[0, end_column].Border.BorderColorLeft = Color.White;

    //            attnd_report.Sheets[0].ColumnHeader.Cells[(attnd_report.Sheets[0].ColumnHeader.RowCount - 1), end_column].Border.BorderColorTop = Color.Black;
    //        }
    //        else
    //        {
    //            attnd_report.Sheets[0].ColumnHeaderSpanModel.Add(0, end_column, (7), 1);
    //            attnd_report.Sheets[0].ColumnHeader.Cells[0, end_column].CellType = mi2;
    //            attnd_report.Sheets[0].ColumnHeader.Columns[end_column].Width = 150;
    //            attnd_report.Sheets[0].ColumnHeader.Cells[0, end_column].Border.BorderColorRight = Color.Black;
    //            if (dsprint.Tables[0].Rows.Count > 0)
    //            {
    //                if (dsprint.Tables[0].Rows[0]["header_align_index"].ToString() != "")
    //                {
    //                    attnd_report.Sheets[0].ColumnHeader.Cells[0, end_column].Border.BorderColorBottom = Color.White;
    //                }
    //            }
    //            attnd_report.Sheets[0].ColumnHeader.Cells[(attnd_report.Sheets[0].ColumnHeader.RowCount - 2), end_column].Border.BorderColorTop = Color.Black;
    //        }
    //        //attnd_report.Sheets[0].ColumnHeaderSpanModel.Add(0, end_column, (6), 1);
    //        //attnd_report.Sheets[0].ColumnHeader.Cells[0, end_column].CellType = mi2;
    //        //attnd_report.Sheets[0].ColumnHeader.Cells[0, end_column].Border.BorderColorLeft = Color.White;
    //        //attnd_report.Sheets[0].ColumnHeader.Cells[0, end_column].Border.BorderColorBottom = Color.White;

    //        temp_count = 0;
    //        for (col_count = 0; col_count < attnd_report.Sheets[0].ColumnCount; col_count++)
    //        {
    //            if (attnd_report.Sheets[0].Columns[col_count].Visible == true)
    //            {
    //                if (temp_count == 1)
    //                {
    //                    //more_column();
    //                    for (int row_cnt = 0; row_cnt < attnd_report.Sheets[0].ColumnHeader.RowCount - 1; row_cnt++)
    //                    {
    //                        attnd_report.Sheets[0].ColumnHeader.Cells[row_cnt, col_count].Border.BorderColorLeft = Color.White;
    //                        attnd_report.Sheets[0].ColumnHeader.Cells[row_cnt, col_count].Border.BorderColorRight = Color.White;
    //                    }
    //                }
    //                temp_count++;
    //            }
    //        }
    //    }
    //    //=========================



    //    //2.Footer setting

    //    if (dsprint.Tables[0].Rows.Count > 0)
    //    {
    //        if (dsprint.Tables[0].Rows[0]["footer"].ToString() != null && dsprint.Tables[0].Rows[0]["footer"].ToString() != "")
    //        {
    //            footer_count = Convert.ToInt16(dsprint.Tables[0].Rows[0]["footer"].ToString());
    //            attnd_report.Sheets[0].RowCount = attnd_report.Sheets[0].RowCount + 3;

    //            attnd_report.Sheets[0].Cells[(attnd_report.Sheets[0].RowCount - 3), start_column].ColumnSpan = attnd_report.Sheets[0].ColumnCount - start_column;
    //            attnd_report.Sheets[0].Cells[(attnd_report.Sheets[0].RowCount - 2), start_column].ColumnSpan = attnd_report.Sheets[0].ColumnCount - start_column;

    //            attnd_report.Sheets[0].Cells[(attnd_report.Sheets[0].RowCount - 3), start_column].Border.BorderColorBottom = Color.White;
    //            attnd_report.Sheets[0].Cells[(attnd_report.Sheets[0].RowCount - 2), start_column].Border.BorderColorTop = Color.White;
    //            attnd_report.Sheets[0].Cells[(attnd_report.Sheets[0].RowCount - 2), start_column].Border.BorderColorBottom = Color.White;
    //            attnd_report.Sheets[0].Cells[(attnd_report.Sheets[0].RowCount - 1), start_column].Border.BorderColorTop = Color.White;


    //            footer_text = dsprint.Tables[0].Rows[0]["footer_name"].ToString();
    //            string[] footer_text_split = footer_text.Split(',');
    //            footer_text = "";




    //            if (final_print_col_cnt < footer_count)
    //            {
    //                for (int concod_footer = 0; concod_footer < footer_count; concod_footer++)
    //                {
    //                    if (footer_text == "")
    //                    {
    //                        footer_text = footer_text_split[concod_footer].ToString();
    //                    }
    //                    else
    //                    {
    //                        footer_text = footer_text + "   " + footer_text_split[concod_footer].ToString();
    //                    }
    //                }

    //                for (col_count = 0; col_count < attnd_report.Sheets[0].ColumnCount; col_count++)
    //                {
    //                    if (attnd_report.Sheets[0].Columns[col_count].Visible == true)
    //                    {
    //                        attnd_report.Sheets[0].SpanModel.Add((attnd_report.Sheets[0].RowCount - 1), col_count, 1, final_print_col_cnt);
    //                        attnd_report.Sheets[0].Cells[(attnd_report.Sheets[0].RowCount - 1), col_count].Text = footer_text;
    //                        attnd_report.Sheets[0].Cells[(attnd_report.Sheets[0].RowCount - 1), col_count].Font.Bold = true;
    //                        break;
    //                    }
    //                }

    //            }

    //            else if (final_print_col_cnt == footer_count)
    //            {
    //                for (col_count = 0; col_count < attnd_report.Sheets[0].ColumnCount; col_count++)
    //                {
    //                    if (attnd_report.Sheets[0].Columns[col_count].Visible == true)
    //                    {
    //                        attnd_report.Sheets[0].Cells[(attnd_report.Sheets[0].RowCount - 1), col_count].Text = footer_text_split[temp_count].ToString();
    //                        attnd_report.Sheets[0].Cells[(attnd_report.Sheets[0].RowCount - 1), col_count].Border.BorderColorRight = Color.White;
    //                        attnd_report.Sheets[0].Cells[(attnd_report.Sheets[0].RowCount - 1), col_count].Border.BorderColorLeft = Color.White;
    //                        attnd_report.Sheets[0].Cells[(attnd_report.Sheets[0].RowCount - 1), col_count].Font.Bold = true;
    //                        temp_count++;
    //                        if (temp_count == footer_count)
    //                        {
    //                            break;
    //                        }
    //                    }
    //                }

    //            }

    //            else
    //            {
    //                temp_count = 0;
    //                split_col_for_footer = final_print_col_cnt / footer_count;
    //                footer_balanc_col = final_print_col_cnt % footer_count;

    //                for (col_count = 0; col_count < attnd_report.Sheets[0].ColumnCount; col_count++)
    //                {
    //                    if (attnd_report.Sheets[0].Columns[col_count].Visible == true)
    //                    {
    //                        if (temp_count == 0)
    //                        {
    //                            attnd_report.Sheets[0].SpanModel.Add((attnd_report.Sheets[0].RowCount - 1), col_count, 1, split_col_for_footer + footer_balanc_col);
    //                        }
    //                        else
    //                        {

    //                            attnd_report.Sheets[0].SpanModel.Add((attnd_report.Sheets[0].RowCount - 1), col_count, 1, split_col_for_footer);

    //                        }
    //                        attnd_report.Sheets[0].Cells[(attnd_report.Sheets[0].RowCount - 1), col_count].Text = footer_text_split[temp_count].ToString();
    //                        attnd_report.Sheets[0].Cells[(attnd_report.Sheets[0].RowCount - 1), col_count].Font.Bold = true;
    //                        if (col_count - 1 >= 0)
    //                        {
    //                            attnd_report.Sheets[0].Cells[(attnd_report.Sheets[0].RowCount - 1), col_count - 1].Border.BorderColorLeft = Color.White;
    //                            attnd_report.Sheets[0].Cells[(attnd_report.Sheets[0].RowCount - 1), col_count - 1].Border.BorderColorRight = Color.White;
    //                        }
    //                        attnd_report.Sheets[0].Cells[(attnd_report.Sheets[0].RowCount - 1), col_count].Border.BorderColorLeft = Color.White;
    //                        attnd_report.Sheets[0].Cells[(attnd_report.Sheets[0].RowCount - 1), col_count].Border.BorderColorRight = Color.White;
    //                        if (col_count + 1 < attnd_report.Sheets[0].ColumnCount)
    //                        {
    //                            attnd_report.Sheets[0].Cells[(attnd_report.Sheets[0].RowCount - 1), col_count + 1].Border.BorderColorLeft = Color.White;
    //                            attnd_report.Sheets[0].Cells[(attnd_report.Sheets[0].RowCount - 1), col_count + 1].Border.BorderColorRight = Color.White;
    //                        }


    //                        temp_count++;
    //                        if (temp_count == 0)
    //                        {
    //                            col_count = col_count + split_col_for_footer + footer_balanc_col;
    //                        }
    //                        else
    //                        {
    //                            col_count = col_count + split_col_for_footer;
    //                        }
    //                        if (temp_count == footer_count)
    //                        {
    //                            break;
    //                        }
    //                    }
    //                }
    //            }



    //        }
    //    }

    //    //2 end.Footer setting
    //}

    //=======================================

    public void view_header_setting()
    {
        if (dsprint.Tables[0].Rows.Count > 0)
        {

            ddlpage.Visible = true;
            lblpages.Visible = true;

            view_footer = dsprint.Tables[0].Rows[0]["footer_flag_value"].ToString();
            view_header = dsprint.Tables[0].Rows[0]["header_flag_value"].ToString();
            view_footer_text = dsprint.Tables[0].Rows[0]["footer_name"].ToString();
            if (view_header == "0" || view_header == "1")
            {
                errmsg.Visible = false;

                for (int row_cnt = 0; row_cnt < attnd_report.Sheets[0].ColumnHeader.RowCount; row_cnt++)
                {
                    attnd_report.Sheets[0].ColumnHeader.Rows[row_cnt].Visible = true;
                }

                int i = 0;
                ddlpage.Items.Clear();
                int totrowcount = attnd_report.Sheets[0].RowCount;
                int pages = totrowcount / 25;
                int intialrow = 1;
                int remainrows = totrowcount % 25;
                if (attnd_report.Sheets[0].RowCount > 0)
                {
                    int i5 = 0;
                    ddlpage.Items.Insert(0, new System.Web.UI.WebControls.ListItem("", "0"));
                    for (i = 1; i <= pages; i++)
                    {
                        i5 = i;

                        ddlpage.Items.Insert(i, new System.Web.UI.WebControls.ListItem(i.ToString(), intialrow.ToString()));
                        intialrow = intialrow + 25;
                    }
                    if (remainrows > 0)
                    {
                        i = i5 + 1;
                        ddlpage.Items.Insert(i, new System.Web.UI.WebControls.ListItem(i.ToString(), intialrow.ToString()));
                    }
                }
                if ((ddlpage.SelectedValue.ToString() == string.Empty) || (ddlpage.SelectedValue.ToString() == "0"))
                {
                    for (i = 0; i < attnd_report.Sheets[0].RowCount; i++)
                    {
                        attnd_report.Sheets[0].Rows[i].Visible = true;
                    }
                    Double totalRows = 0;
                    totalRows = Convert.ToInt32(attnd_report.Sheets[0].RowCount);
                    Session["totalPages"] = (int)Math.Ceiling(totalRows / attnd_report.Sheets[0].PageSize);
                    Buttontotal.Text = "Records : " + totalRows + "          Pages : " + Session["totalPages"];
                    DropDownListpage.Items.Clear();
                    if (totalRows >= 10)
                    {
                        attnd_report.Sheets[0].PageSize = Convert.ToInt32(totalRows);
                        for (int k = 0; k < Convert.ToInt32(totalRows); k = k + 10)
                        {
                            DropDownListpage.Items.Add((k + 10).ToString());
                        }
                        DropDownListpage.Items.Add("Others");
                        DropDownListpage.Text = DropDownListpage.Items[DropDownListpage.Items.Count - 2].Text;
                        attnd_report.Height = 335;

                    }
                    else if (totalRows == 0)
                    {
                        DropDownListpage.Items.Add("0");
                        attnd_report.Height = 100;
                    }
                    else
                    {
                        attnd_report.Sheets[0].PageSize = Convert.ToInt32(totalRows);
                        DropDownListpage.Items.Add(attnd_report.Sheets[0].PageSize.ToString());
                        attnd_report.Height = 100 + (10 * Convert.ToInt32(totalRows));
                    }
                    if (Convert.ToInt32(attnd_report.Sheets[0].RowCount) > 10)
                    {
                        DropDownListpage.Text = DropDownListpage.Items[DropDownListpage.Items.Count - 2].Text;
                        attnd_report.Sheets[0].PageSize = int.Parse(DropDownListpage.Items[DropDownListpage.Items.Count - 2].Text);
                        attnd_report.Height = 100 + (10 * Convert.ToInt32(totalRows));
                        CalculateTotalPages();
                    }


                    pageset_pnl.Visible = false;
                    attnd_report.Visible = true;

                }
                else
                {
                    errmsg.Visible = false;
                    pageset_pnl.Visible = false;
                    attnd_report.Visible = false;
                    btnprintmaster.Visible = false;
                }
            }
            else if (view_header == "2")
            {

                for (int row_cnt = 0; row_cnt < attnd_report.Sheets[0].ColumnHeader.RowCount; row_cnt++)
                {
                    attnd_report.Sheets[0].ColumnHeader.Rows[row_cnt].Visible = false;
                }

                errmsg.Visible = false;
                int i = 0;
                ddlpage.Items.Clear();
                int totrowcount = attnd_report.Sheets[0].RowCount;
                int pages = totrowcount / 25;
                int intialrow = 1;
                int remainrows = totrowcount % 25;
                if (attnd_report.Sheets[0].RowCount > 0)
                {
                    int i5 = 0;
                    ddlpage.Items.Insert(0, new System.Web.UI.WebControls.ListItem("", "0"));
                    for (i = 1; i <= pages; i++)
                    {
                        i5 = i;

                        ddlpage.Items.Insert(i, new System.Web.UI.WebControls.ListItem(i.ToString(), intialrow.ToString()));
                        intialrow = intialrow + 25;
                    }
                    if (remainrows > 0)
                    {
                        i = i5 + 1;
                        ddlpage.Items.Insert(i, new System.Web.UI.WebControls.ListItem(i.ToString(), intialrow.ToString()));
                    }
                }
                if ((ddlpage.SelectedValue.ToString() == string.Empty) || (ddlpage.SelectedValue.ToString() == "0"))
                {
                    for (i = 0; i < attnd_report.Sheets[0].RowCount; i++)
                    {
                        attnd_report.Sheets[0].Rows[i].Visible = true;
                    }
                    Double totalRows = 0;
                    totalRows = Convert.ToInt32(attnd_report.Sheets[0].RowCount);
                    Session["totalPages"] = (int)Math.Ceiling(totalRows / attnd_report.Sheets[0].PageSize);
                    Buttontotal.Text = "Records : " + totalRows + "          Pages : " + Session["totalPages"];
                    DropDownListpage.Items.Clear();
                    if (totalRows >= 10)
                    {
                        attnd_report.Sheets[0].PageSize = Convert.ToInt32(totalRows);
                        for (int k = 0; k < Convert.ToInt32(totalRows); k = k + 10)
                        {
                            DropDownListpage.Items.Add((k + 10).ToString());
                        }
                        DropDownListpage.Items.Add("Others");
                        DropDownListpage.Text = DropDownListpage.Items[DropDownListpage.Items.Count - 2].Text;
                        attnd_report.Height = 335;

                    }
                    else if (totalRows == 0)
                    {
                        DropDownListpage.Items.Add("0");
                        attnd_report.Height = 100;
                    }
                    else
                    {
                        attnd_report.Sheets[0].PageSize = Convert.ToInt32(totalRows);
                        DropDownListpage.Items.Add(attnd_report.Sheets[0].PageSize.ToString());
                        attnd_report.Height = 100 + (10 * Convert.ToInt32(totalRows));
                    }
                    if (Convert.ToInt32(attnd_report.Sheets[0].RowCount) > 10)
                    {
                        DropDownListpage.Text = DropDownListpage.Items[DropDownListpage.Items.Count - 2].Text;
                        attnd_report.Sheets[0].PageSize = int.Parse(DropDownListpage.Items[DropDownListpage.Items.Count - 2].Text);
                        //  attnd_report.Height = 100 + (10 * Convert.ToInt32(totalRows));
                        CalculateTotalPages();
                    }
                    pageset_pnl.Visible = false;
                    attnd_report.Visible = true;
                }
                else
                {
                    pageset_pnl.Visible = false;
                    attnd_report.Visible = false;
                }
            }
            else
            {

            }
            lblpages.Visible = true;
            ddlpage.Visible = true;
        }
        else
        {
            lblpages.Visible = false;
            ddlpage.Visible = false;

        }
    }

    //===============Hided  by Manikandan 18/05/2013

    //public void more_column()
    //{
    //    header_text();

    //    attnd_report.Sheets[0].ColumnHeader.Cells[0, col_count].Text = coll_name;
    //    attnd_report.Sheets[0].ColumnHeader.Cells[0, col_count].Border.BorderColorBottom = Color.White;
    //    //  attnd_report.Sheets[0].ColumnHeaderSpanModel.Add(0, col_count, 1, final_print_col_cnt - 2);
    //    if (final_print_col_cnt > 3)
    //    {
    //        attnd_report.Sheets[0].ColumnHeaderSpanModel.Add(0, col_count, 1, (end_column - col_count));
    //        attnd_report.Sheets[0].ColumnHeaderSpanModel.Add(1, col_count, 1, (end_column - col_count));
    //        attnd_report.Sheets[0].ColumnHeaderSpanModel.Add(2, col_count, 1, (end_column - col_count));
    //        attnd_report.Sheets[0].ColumnHeaderSpanModel.Add(3, col_count, 1, (end_column - col_count));
    //        attnd_report.Sheets[0].ColumnHeaderSpanModel.Add(6, col_count, 1, (end_column - col_count));
    //        attnd_report.Sheets[0].ColumnHeaderSpanModel.Add(4, col_count, 1, (end_column - col_count));
    //        attnd_report.Sheets[0].ColumnHeaderSpanModel.Add(5, col_count, 1, (end_column - col_count));
    //    }
    //    attnd_report.Sheets[0].ColumnHeader.Cells[1, col_count].Text = address1 + "-" + address2 + "-" + address3;
    //    attnd_report.Sheets[0].ColumnHeader.Cells[1, col_count].Border.BorderColorTop = Color.White;
    //    attnd_report.Sheets[0].ColumnHeader.Cells[1, col_count].Border.BorderColorBottom = Color.White;

    //    if (phoneno != "" && phoneno != null)
    //    {
    //        phone = "Phone:" + phoneno;
    //    }
    //    else
    //    {
    //        phone = "";
    //    }

    //    if (faxno != "" && faxno != null)
    //    {
    //        fax = "  Fax:" + faxno;
    //    }
    //    else
    //    {
    //        fax = "";
    //    }

    //    attnd_report.Sheets[0].ColumnHeader.Cells[2, col_count].Text = phone + fax;
    //    attnd_report.Sheets[0].ColumnHeader.Cells[2, col_count].Border.BorderColorTop = Color.White;
    //    attnd_report.Sheets[0].ColumnHeader.Cells[2, col_count].Border.BorderColorBottom = Color.White;

    //    if (email != "" && faxno != null)
    //    {
    //        email_id = "Email:" + email;
    //    }
    //    else
    //    {
    //        email_id = "";
    //    }


    //    if (website != "" && website != null)
    //    {
    //        web_add = "  Web Site:" + website;
    //    }
    //    else
    //    {
    //        web_add = "";
    //    }

    //    attnd_report.Sheets[0].ColumnHeader.Cells[3, col_count].Text = email_id + web_add;
    //    attnd_report.Sheets[0].ColumnHeader.Cells[3, col_count].Border.BorderColorTop = Color.White;
    //    attnd_report.Sheets[0].ColumnHeader.Cells[3, col_count].Border.BorderColorBottom = Color.White;

    //    if (form_name != "" && form_name != null)
    //    {
    //        attnd_report.Sheets[0].ColumnHeader.Cells[4, col_count].Text = form_name;
    //        attnd_report.Sheets[0].ColumnHeader.Cells[5, col_count].Text = "----------------------------------------------------";
    //        attnd_report.Sheets[0].ColumnHeader.Rows[5].Visible = false;
    //    }
    //        attnd_report.Sheets[0].ColumnHeader.Cells[4, col_count].Border.BorderColorTop = Color.White;
    //        attnd_report.Sheets[0].ColumnHeader.Cells[4, col_count].Border.BorderColorBottom = Color.White;
    //        attnd_report.Sheets[0].ColumnHeader.Cells[5, col_count].Border.BorderColorTop = Color.White;
    //        attnd_report.Sheets[0].ColumnHeader.Cells[5, col_count].Border.BorderColorBottom = Color.White;

    //    string dt = DateTime.Today.ToShortDateString();
    //    string[] dsplit = dt.Split(new Char[] { '/' });
    //    attnd_report.Sheets[0].ColumnHeader.Cells[6, col_count].Text = "Attendance Date: " + txtFromDate.Text + " Date On: " + dsplit[1].ToString() + "/" + dsplit[0].ToString() + "/" + dsplit[2].ToString();
    //    int temp_count_temp = 0;
    //    string[] header_align_index;

    //    if (dsprint.Tables[0].Rows.Count > 0)
    //    {

    //        if (dsprint.Tables[0].Rows[0]["new_header_name"].ToString() != null && dsprint.Tables[0].Rows[0]["new_header_name"].ToString() != "")
    //        {
    //            header_align_index = dsprint.Tables[0].Rows[0]["header_align_index"].ToString().Split(',');

    //            attnd_report.Sheets[0].ColumnHeader.Cells[6, start_column].Border.BorderColorBottom = Color.White;
    //            attnd_report.Sheets[0].ColumnHeader.Cells[6, end_column].Border.BorderColorBottom = Color.White;
    //            attnd_report.Sheets[0].ColumnHeader.Cells[6, col_count].Border.BorderColorBottom = Color.White;


    //            for (int row_head_count = 7; row_head_count < (7 + new_header_string_split.GetUpperBound(0) + 1); row_head_count++)
    //            {
    //                attnd_report.Sheets[0].ColumnHeader.Cells[row_head_count, start_column].Text = new_header_string_split[temp_count_temp].ToString();
    //                attnd_report.Sheets[0].ColumnHeader.Cells[row_head_count, start_column].Border.BorderColorTop = Color.White;
    //                attnd_report.Sheets[0].ColumnHeader.Cells[row_head_count - 1, start_column].Border.BorderColorBottom = Color.White;
    //                //if (final_print_col_cnt > 3)
    //                {
    //                    attnd_report.Sheets[0].ColumnHeaderSpanModel.Add(row_head_count, start_column, 1, (attnd_report.Sheets[0].ColumnCount - start_column + 1));
    //                }
    //                attnd_report.Sheets[0].ColumnHeader.Cells[row_head_count, start_column].Border.BorderColorTop = Color.White;
    //                if (row_head_count != (7 + new_header_string_split.GetUpperBound(0)))
    //                {
    //                    attnd_report.Sheets[0].ColumnHeader.Cells[row_head_count, start_column].Border.BorderColorBottom = Color.White;
    //                }

    //                if (temp_count_temp <= header_align_index.GetUpperBound(0))
    //                {
    //                    if (header_align_index[temp_count_temp].ToString() != string.Empty)
    //                    {
    //                        header_alignment = header_align_index[temp_count_temp].ToString();
    //                        if (header_alignment == "2")
    //                        {
    //                            attnd_report.Sheets[0].ColumnHeader.Cells[row_head_count, start_column].HorizontalAlign = HorizontalAlign.Center;
    //                        }
    //                        else if (header_alignment == "1")
    //                        {
    //                            attnd_report.Sheets[0].ColumnHeader.Cells[row_head_count, start_column].HorizontalAlign = HorizontalAlign.Left;
    //                        }
    //                        else
    //                        {
    //                            attnd_report.Sheets[0].ColumnHeader.Cells[row_head_count, start_column].HorizontalAlign = HorizontalAlign.Right;
    //                        }
    //                    }
    //                }

    //                temp_count_temp++;
    //            }
    //        }
    //    }
    //}


    //public void header_text()
    //{

    //    Boolean check_print_row = false;

    //    SqlDataReader dr_collinfo;
    //    con.Close();
    //    con.Open();
    //    cmd = new SqlCommand("select isnull(college_name,'') as collname,isnull(address1,'') as address1,isnull(address2,'') as address2,isnull(address3,'') as address3,isnull(phoneno,'') as phoneno,isnull(faxno,'') as faxno,isnull(email,'') as email,isnull(website,'') as website,isnull(form_heading_name,'') as form_name,isnull(batch_degree_branch,'') as degree_deatil,isnull(header_align,'') as header_alignment,isnull(header_flag_value,'') as view_header from print_master_setting  where form_name='ovrall_attreport_perday.aspx'", con);
    //    dr_collinfo = cmd.ExecuteReader();
    //    while (dr_collinfo.Read())
    //    {
    //        if (dr_collinfo.HasRows == true)
    //        {
    //            check_print_row = true;
    //            coll_name = dr_collinfo["collname"].ToString();
    //            address1 = dr_collinfo["address1"].ToString();
    //            address2 = dr_collinfo["address2"].ToString();
    //            address3 = dr_collinfo["address3"].ToString();
    //            phoneno = dr_collinfo["phoneno"].ToString();
    //            faxno = dr_collinfo["faxno"].ToString();
    //            email = dr_collinfo["email"].ToString();
    //            website = dr_collinfo["website"].ToString();
    //            form_name = dr_collinfo["form_name"].ToString();           
    //            header_alignment = dr_collinfo["header_alignment"].ToString();
    //            view_header = dr_collinfo["view_header"].ToString();
    //        }

    //    }
    //    if (check_print_row == false)
    //    {

    //        con.Close();
    //        con.Open();
    //        cmd = new SqlCommand("select isnull(collname,'') as collname,isnull(address1,'') as address1,isnull(address2,'') as address2,isnull(address3,'') as address3,isnull(phoneno,'') as phoneno,isnull(faxno,'') as faxno,isnull(email,'') as email,isnull(website,'') as website from collinfo  where college_code=" + Session["InternalCollegeCode"] + "", con);
    //        dr_collinfo = cmd.ExecuteReader();
    //        while (dr_collinfo.Read())
    //        {
    //            if (dr_collinfo.HasRows == true)
    //            {
    //                check_print_row = true;
    //                coll_name = dr_collinfo["collname"].ToString();
    //                address1 = dr_collinfo["address1"].ToString();
    //                address2 = dr_collinfo["address2"].ToString();
    //                address3 = dr_collinfo["address3"].ToString();
    //                phoneno = dr_collinfo["phoneno"].ToString();
    //                faxno = dr_collinfo["faxno"].ToString();
    //                email = dr_collinfo["email"].ToString();
    //                website = dr_collinfo["website"].ToString();
    //                form_name = "Over All Attendance Report For Particular Day  ";
    //               // degree_deatil = ddldegree.SelectedItem.ToString() + "-" + ddlbranch.SelectedItem.ToString() + "[ " + sem_roman(Convert.ToInt16(ddlduration.SelectedItem.ToString())) + "  Semester ] -" + sec_val + " ";
    //                // header_alignment = ddldegree.SelectedItem.ToString() + "-" + ddlbranch.SelectedItem.ToString() + "[ " + sem_roman(Convert.ToInt16(ddlduration.SelectedItem.ToString())) + "  Semester ] -" + sec_val + " ";
    //                // view_header = dr_collinfo["view_header"].ToString();
    //            }

    //        }
    //    }
    //}

    //======================================

    protected void ddlcollege_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (Convert.ToString(Session["QueryString"]) != "")
        {

            PropertyInfo isreadonly = typeof(System.Collections.Specialized.NameValueCollection).GetProperty("IsReadOnly", BindingFlags.Instance | BindingFlags.NonPublic);
            // make collection editable
            isreadonly.SetValue(this.Request.QueryString, false, null);
            // remove
            this.Request.QueryString.Remove(Convert.ToString(Session["QueryString"]));
            Request.QueryString.Clear();

        }

        Session["InternalCollegeCode"] = ddlcollege.SelectedValue.ToString();
        Pageload(sender, e);
    }

    public void Pageload(object sender, EventArgs e)
    {
        //if (Convert.ToString(Session["value"]) == "1")
        //{
        //    LinkButton3.Visible = false;
        //    LinkButtonb2.Visible = true;
        //}
        //else
        //{
        //    LinkButton3.Visible = true;
        //    LinkButtonb2.Visible = false;
        //}
        attnd_report.Sheets[0].SheetName = " ";
        attnd_report.Sheets[0].AutoPostBack = false;
        attnd_report.Visible = false;
        btnprintmaster.Visible = false;
        btnxl.Visible = false;
        //Added by Srinath 27/2/2
        lblrptname.Visible = false;
        txtexcelname.Visible = false;
        lblpages.Visible = false;
        ddlpage.Visible = false;
        attnd_report.Sheets[0].ColumnCount = 12;
        attnd_report.Sheets[0].SetColumnMerge(0, FarPoint.Web.Spread.Model.MergePolicy.Always);
        attnd_report.Sheets[0].SetColumnMerge(1, FarPoint.Web.Spread.Model.MergePolicy.Always);
        attnd_report.Sheets[0].Columns[0].Font.Bold = true;
        attnd_report.Sheets[0].SheetCorner.Columns[0].Visible = false;
        pageddltxt.Visible = false;
        pageset_pnl.Visible = false;
        lblFromDate.Visible = false;
        //  pagesetpanel.Visible = false;
        //------------initial date picker value
        date_today = Convert.ToDateTime(DateTime.Today.ToShortDateString());
        txtFromDate.Text = date_today.ToString("dd") + "/" + date_today.ToString("MM") + "/" + date_today.ToString("yyyy");

        Session["curr_year"] = date_today.ToString("yyyy");


        //-----------------------spread design
        attnd_report.Sheets[0].ColumnHeader.DefaultStyle.Font.Bold = true;
        attnd_report.Sheets[0].ColumnHeader.DefaultStyle.Font.Size = FontUnit.Medium;
        attnd_report.Sheets[0].ColumnHeader.DefaultStyle.Font.Name = "Book Antiqua";
        attnd_report.Sheets[0].RowHeader.DefaultStyle.Font.Bold = true;
        attnd_report.Sheets[0].RowHeader.DefaultStyle.Font.Size = FontUnit.Medium;
        attnd_report.Sheets[0].RowHeader.DefaultStyle.Font.Name = "Book Antiqua";
        attnd_report.Sheets[0].DefaultStyle.Font.Name = "Book Antiqua";
        attnd_report.Sheets[0].DefaultStyle.Font.Bold = false;
        attnd_report.Sheets[0].DefaultStyle.Font.Size = FontUnit.Medium;
        attnd_report.Sheets[0].DefaultStyle.HorizontalAlign = HorizontalAlign.Center;

        attnd_report.Sheets[0].ColumnHeader.RowCount = 8;
        //attnd_report.Sheets[0].SheetCornerSpanModel.Add(0, 0, 7, 1);
        //attnd_report.Sheets[0].SheetCorner.Cells[7, 0].Text = "S.No";
        FarPoint.Web.Spread.StyleInfo style1 = new FarPoint.Web.Spread.StyleInfo();
        style1.Font.Size = 12;
        style1.Font.Bold = true;
        style1.Font.Name = "Book Antiqua";
        style1.HorizontalAlign = HorizontalAlign.Center;
        style1.ForeColor = Color.Black;
        style1.BackColor = ColorTranslator.FromHtml("#0CA6CA");
        attnd_report.Sheets[0].SheetCornerStyle = new FarPoint.Web.Spread.StyleInfo(style1);
        attnd_report.Sheets[0].ColumnHeader.DefaultStyle = new FarPoint.Web.Spread.StyleInfo(style1);
        attnd_report.Sheets[0].ColumnHeader.DefaultStyle.HorizontalAlign = HorizontalAlign.Center;
        attnd_report.Sheets[0].AllowTableCorner = true;
        attnd_report.Sheets[0].DefaultColumnWidth = 80;

        attnd_report.Pager.Position = FarPoint.Web.Spread.PagerPosition.Bottom;
        attnd_report.Pager.Mode = FarPoint.Web.Spread.PagerMode.Both;
        attnd_report.Pager.Align = HorizontalAlign.Right;
        attnd_report.Pager.Font.Bold = true;
        attnd_report.Pager.Font.Name = "Book Antiqua";
        attnd_report.Pager.ForeColor = Color.DarkGreen;
        attnd_report.Pager.BackColor = Color.Beige;
        attnd_report.Pager.BackColor = Color.AliceBlue;
        attnd_report.Pager.PageCount = 5;
        attnd_report.CommandBar.Visible = false;
        if (Session["prntvissble"].ToString() == "true")
        {
            //btn_print_setting.Visible = true;
        }
        else
        {
            //btn_print_setting.Visible = false;
        }
        if (Request.QueryString["val"] == null)
        {
            Session["QueryString"] = "";
        }
        else
        {
            try
            {
                Session["QueryString"] = Convert.ToString(Request.QueryString["val"]);
                string_session_values = Request.QueryString["val"].Split(',');
                txtFromDate.Text = string_session_values[0].ToString();
                ddlcollege.SelectedIndex = Convert.ToInt16(string_session_values[1].ToString());
                Session["InternalCollegeCode"] = ddlcollege.SelectedValue.ToString();

                //print_btngo();
                //setheader_print();
                view_header_setting();

                if (attnd_report.Sheets[0].RowCount > 0 && final_print_col_cnt > 0)
                {
                    attnd_report.Sheets[0].Visible = true;
                }
                attnd_report.Width = final_print_col_cnt * 75;
            }
            catch
            {
            }
        }

    }

    protected void btnprintmaster_Click(object sender, EventArgs e)
    {
        Session["column_header_row_count"] = Convert.ToString(attnd_report.ColumnHeader.RowCount);
        string degreedetails = string.Empty;

        //SqlDataAdapter da_acronym = new SqlDataAdapter(deg_acronym, con);
        //DataTable dt_acronym = new DataTable();
        //da_acronym.Fill(dt_acronym);

        degreedetails = "Overall Attendance Percentage for Particular Day@Attendance Date: " + txtFromDate.Text.ToString();
        if (chkPeriod.Checked == true)
        {
            string hour = ddlperiod.SelectedItem.ToString();
            if (hour.Trim() == "1")
            {
                degreedetails = "1st Hour Attendance Report@Attendance Date: " + txtFromDate.Text.ToString();
            }
            else if (hour.Trim() == "2")
            {
                degreedetails = "2nd Hour Attendance Report@Attendance Date: " + txtFromDate.Text.ToString();
            }
            else if (hour.Trim() == "3")
            {
                degreedetails = "3rd Hour Attendance Report@Attendance Date: " + txtFromDate.Text.ToString();
            }
            else
            {
                degreedetails = hour + "th Hour Attendance Report@Attendance Date: " + txtFromDate.Text.ToString();
            }
        }
        string pagename = "StudentTestReport.aspx";

        Printcontrol.loadspreaddetails(attnd_report, pagename, degreedetails);
        Printcontrol.Visible = true;
    }

    protected void chkPeriod_CheckedChange(object sender, EventArgs e)
    {
        if (chkPeriod.Checked == true)
        {
            ddlperiod.Visible = true;
        }
        else
        {
            ddlperiod.Visible = false;
        }

    }

    public void loadperiods()
    {
        int hour = int.Parse(dacces2.GetFunction("select MAX(no_of_hrs_per_day) from PeriodAttndSchedule"));
        ddlperiod.Items.Clear();
        if (hour > 0)
        {
            for (int i = 1; i <= hour; i++)
            {
                ddlperiod.Items.Add(i.ToString());
            }
        }
    }

}


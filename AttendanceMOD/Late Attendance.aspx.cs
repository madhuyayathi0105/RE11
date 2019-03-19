using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Collections;
using System.Web.Services;
using System.Net;
using System.IO;

public partial class LateAttendance : System.Web.UI.Page
{
  static  string usercode = string.Empty;
    string collegecode1 = string.Empty;
    string singleuser = string.Empty;
   static string group_user = string.Empty;
   static string sms_mom = "";
   static string sms_dad = "";
   static string sms_stud = "";
    DataSet ds = new DataSet();
    DAccess2 d2 = new DAccess2();
    static int chosedmode = 0;
    static int personmode = 0;
    static string query = "";
   static string coll_code = string.Empty;
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
        coll_code = Convert.ToString(ddl_College.SelectedValue);
        if (!IsPostBack)
        {
            bindcolege();
            coll_code = Convert.ToString(ddl_College.SelectedValue);
          lbl_studimage.ImageUrl = "../images/dummyimg.png";
           // CalendarExtender1.EndDate = DateTime.Now;
          txt_smartno.Focus();
        }
    }
    public void bindcolege()
    {
        try
        {
            ds.Clear();
            //ddl_college.Items.Clear();
            string query = "select cp.college_code,cf.collname from collegeprivilages cp,collinfo cf where user_code=" + Session["usercode"] + " and cp.college_code=cf.college_code";
            ds = d2.select_method_wo_parameter(query, "Text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                //ddl_college.DataSource = ds;
                //ddl_college.DataTextField = "collname";
                //ddl_college.DataValueField = "college_code";
                //ddl_college.DataBind();
                ddl_College.DataSource = ds;
                ddl_College.DataTextField = "collname";
                ddl_College.DataValueField = "college_code";
                ddl_College.DataBind();
               
            }
        }
        catch
        {
        }
    }
    [System.Web.Services.WebMethod]
    [System.Web.Script.Services.ScriptMethod()]
    public static List<string> Getrno(string prefixText)
    {
        List<string> name = new List<string>();
        try
        {

            WebService ws = new WebService();
            if (personmode == 0)
            {
                //student query
                if (chosedmode == 1)
                {
                    query = "select Roll_No from Registration where CC=0 and DelFlag =0 and Exam_Flag <>'DEBAR' and college_code='" + coll_code + "' and  Roll_No like '" + prefixText + "%'";
                  

                }
                else if (chosedmode == 2)
                {

                    query = "select  Reg_No from Registration where CC=0 and DelFlag =0 and Exam_Flag <>'DEBAR' and college_code='" + coll_code + "' and Reg_No like '" + prefixText + "%' order by Reg_No";
                }

                //else if (chosedmode == 2)
                //{

                //    query = "select distinct r.Roll_Admit from HT_HostelRegistration h,Registration r where r.App_No=h.APP_No and r.Roll_Admit like '" + prefixText + "%' order by Roll_Admit";

                //}
                //else if (chosedmode == 3)
                //{
                //    query = "select distinct r.App_No from HT_HostelRegistration h,Registration r where r.App_No=h.APP_No and r.App_No like '" + prefixText + "%'";

                //}
                else if (chosedmode == 3)
                {

                    query = "select distinct Stud_Name from Registration where CC=0 and DelFlag =0 and Exam_Flag <>'DEBAR' and college_code='" + coll_code + "' and Stud_Name like '" + prefixText + "%'";

                }
                //else if (chosedmode == 5)
                //{
                //    query = "select distinct h.id from HT_HostelRegistration h,Registration r where r.App_No=h.APP_No and h.id like '" + prefixText + "%'";
                //}
            }

            name = ws.Getname(query);
            return name;
        }
        catch { return name; }
    }

    protected void ddlrollno_selectedindexchanged(object sender, EventArgs e)
    {
        //Error.Visible = false;
        //FpSpread1.Visible = false;
        //btnprintmaster.Visible = false;
        //txtno.Text = "";
        //lblnum.Text = ddlrollno.SelectedItem.ToString();


        if (Convert.ToUInt32(ddlrollno.SelectedItem.Value) == 0)
            chosedmode = 0;
             if (Convert.ToUInt32(ddlrollno.SelectedItem.Value)==1)
                  chosedmode = 1;
         if (Convert.ToUInt32(ddlrollno.SelectedItem.Value)==2)
                  chosedmode = 2;
         if (Convert.ToUInt32(ddlrollno.SelectedItem.Value)==3)
                  chosedmode = 3;

         if (Convert.ToUInt32(ddlrollno.SelectedItem.Value) == 0)
         {
             txt_smartno.Focus();
             txt_smartno.Visible = true; txt_rollno.Visible = false;
         }
         else
         {
             txt_rollno.Focus();
             txt_smartno.Visible = false; txt_rollno.Visible = true;
         }
        
    }

   

    [WebMethod]
    public static List<string> studroll(string Smart_No)
    {
        string data = string.Empty;
        List<string> details = new List<string>();
        try
        {
            if (Smart_No.Trim() != "")
            {
                DataSet ds = new DataSet(); DataSet ds1 = new DataSet();
                DAccess2 DA = new DAccess2();
               
              
                string stud_condition = ""; string staff_condition = "";
               
      
                if (chosedmode==1)
                {
                    stud_condition = " and Roll_No='" + Smart_No + "'";
                }
                else if (chosedmode == 2)
                {
                    stud_condition = " and Reg_No='" + Smart_No + "'";
                }
                else if (chosedmode== 3)
                {
                    stud_condition = " and Stud_Name='" + Smart_No + "'";
                }


                Smart_No = "";
                string q1 = "";
                    System.Web.UI.WebControls.Image lbl_studimage = new System.Web.UI.WebControls.Image();
                    lbl_studimage.ImageUrl = "../images/dummyimg.png"; lbl_studimage.Visible = true;
                 
                    if (stud_condition.Trim() != "")
                    {
                        q1 = " select r.app_no,r.Reg_No,r.college_code ,roll_no,Stud_Name,Stud_Type,r.degree_code,Branch_code,Batch_Year,Sections,((CONVERT(varchar(max), r.Batch_Year)+' - '+C.Course_Name+' - '+dt.dept_acronym+ case when sections='' then '' else ' - '+ (sections) end)) as batch from Registration r,Degree d,Department dt,course c where d.Dept_Code=dt.Dept_Code and c.Course_Id=d.Course_Id and r.degree_code=d.Degree_Code " + stud_condition + " and r.college_code='" + coll_code + "'";
                                    ds = DA.select_method_wo_parameter(q1, "Text"); 
                                }
                         
                          
                    //q1 = " select * from Late_attendance ";
                    //ds1.Clear();
                    //ds1 = DA.select_method_wo_parameter(q1, "Text");

                            if (ds.Tables[0].Rows.Count > 0)
                            {
                                string rollno = Convert.ToString(ds.Tables[0].Rows[0]["roll_no"]);
                                details.Add(rollno);
                                details.Add(Convert.ToString(ds.Tables[0].Rows[0]["Stud_Name"]));
                                details.Add(Convert.ToString(ds.Tables[0].Rows[0]["Stud_Type"]));
                                details.Add(Convert.ToString(ds.Tables[0].Rows[0]["batch"]));

                                lbl_studimage.Visible = true; string type = "";



                                lbl_studimage.ImageUrl = "../Handler/Handler4.ashx?rollno=" + rollno + "";
                                details.Add(Convert.ToString(lbl_studimage.ImageUrl));
                           
                                string app_no = Convert.ToString(ds.Tables[0].Rows[0]["app_no"]);
                                string clgcode = Convert.ToString(ds.Tables[0].Rows[0]["college_code"]);
                                DataView alreadyregstud = new DataView();
                                DataView alreadyregstud1 = new DataView();
                               
                               
                                
                                if (app_no.Trim() != "")
                                {
                                    q1 = "";
                                    q1 = "if exists(select * from Late_attendance where App_No='" + app_no + "' and FromDate='" + DateTime.Now.ToString("MM/dd/yyyy") + "' and college_code='" + clgcode + "') update Late_attendance set FromTime='" + DateTime.Now.ToString("h:mm:ss tt") + "' where App_No='" + app_no + "' and FromDate='" + DateTime.Now.ToString("MM/dd/yyyy") + "' and college_code='" + clgcode + "' else    insert into Late_attendance (App_No,FromDate,FromTime,college_code,AttnMonth,AttnYear)values('" + app_no + "','" + DateTime.Now.ToString("MM/dd/yyyy") + "','" + DateTime.Now.ToString("h:mm:ss tt") + "','" + clgcode + "','" + DateTime.Now.ToString("MM") + "','" + DateTime.Now.ToString("yyyy") + "')";
                                    int up = DA.update_method_wo_parameter(q1, "text");
                                    if (up != 0)
                                    {
                                        details.Add("1");
                                        details.Add(type);
                                    }
                                }
                               
                                q1 = " select * from Late_attendance ";
                                ds1.Clear();
                                ds1 = DA.select_method_wo_parameter(q1, "Text");
                                ds1.Tables[0].DefaultView.RowFilter = "AttnMonth='" + DateTime.Now.ToString("MM") + "' and AttnYear ='" + DateTime.Now.ToString("yyyy") + "' and College_Code='" + clgcode + "' and App_No='" + app_no + "'  ";
                                alreadyregstud1 = ds1.Tables[0].DefaultView;
                                details.Add(Convert.ToString(ds.Tables[0].Rows[0]["Reg_no"]));
                                details.Add(Convert.ToString(alreadyregstud1.Count));
                                if (alreadyregstud1.Count >= 4)
                                {
                                    sms(clgcode, app_no, Convert.ToString(ds.Tables[0].Rows[0]["Stud_Name"]), DateTime.Now.ToString("h:mm:ss tt"), DateTime.Now.ToString("MM/dd/yyyy"));
                                }
                            }
                            else
                            {
                                details.Clear();
                                details.Add("");
                                details.Add("");
                                details.Add("");
                                details.Add("");
                                details.Add(Convert.ToString(lbl_studimage.ImageUrl));
                                details.Add("0"); details.Add("");
                            }
                        }
                        
                    
            return details;
        }
        catch
        {
            details.Clear();
            details.Add("");
            details.Add("");
            details.Add("");
            details.Add(""); details.Add("");
            details.Add("0");
            return details;
        }
    }
    [WebMethod]
    public static List<string> studsmartno(string Smart_No)
    {
        string data = string.Empty;
       List<string> details = new List<string>();
        try
        {
            if (Smart_No.Trim() != "" && Smart_No.Length >= 10)
            {
                DataSet ds = new DataSet(); DataSet ds1 = new DataSet(); DataSet ds2 = new DataSet();
                DAccess2 DA = new DAccess2();
                
                string stud_condition = ""; string staff_condition = "";
                if (chosedmode == 0)
                {
                    stud_condition = " and smart_serial_no='" + Smart_No + "'";
                    staff_condition = " and Smartcard_serial_no='" + Smart_No + "'";
                    string messatt_settings = "";
                    string q1 = "";
                    System.Web.UI.WebControls.Image lbl_studimage = new System.Web.UI.WebControls.Image();
                    lbl_studimage.ImageUrl = "../images/dummyimg.png"; lbl_studimage.Visible = true;
                 
                    if (stud_condition.Trim() != "")
                    {
                        q1 = " select r.app_no,r.Reg_No,r.college_code ,roll_no,Stud_Name,Stud_Type,r.degree_code,Branch_code,Batch_Year,Sections,((CONVERT(varchar(max), r.Batch_Year)+' - '+C.Course_Name+' - '+dt.dept_acronym+ case when sections='' then '' else ' - '+ (sections) end)) as batch from Registration r,Degree d,Department dt,course c where d.Dept_Code=dt.Dept_Code and c.Course_Id=d.Course_Id and r.degree_code=d.Degree_Code " + stud_condition + " and r.college_code='" + coll_code + "'";
                                    ds = DA.select_method_wo_parameter(q1, "Text"); 
                                }
                         
                          
                 

                            if (ds.Tables[0].Rows.Count > 0)
                            {
                                string rollno = Convert.ToString(ds.Tables[0].Rows[0]["roll_no"]);
                                details.Add(rollno);
                                details.Add(Convert.ToString(ds.Tables[0].Rows[0]["Stud_Name"]));
                                details.Add(Convert.ToString(ds.Tables[0].Rows[0]["Stud_Type"]));
                                details.Add(Convert.ToString(ds.Tables[0].Rows[0]["batch"]));

                                lbl_studimage.Visible = true; string type = "";

                                lbl_studimage.ImageUrl = "../Handler/Handler4.ashx?rollno=" + rollno + "";
                               
                               
                                details.Add(Convert.ToString(lbl_studimage.ImageUrl));
                           
                                string app_no = Convert.ToString(ds.Tables[0].Rows[0]["app_no"]);
                                string clgcode = Convert.ToString(ds.Tables[0].Rows[0]["college_code"]);
                                DataView alreadyregstud = new DataView();
                                DataView alreadyregstud1 = new DataView();
                               
                              
                                if (app_no.Trim() != "")
                                {
                                    q1 = "";
                                    q1 = "if exists(select * from Late_attendance where App_No='" + app_no + "' and FromDate='" + DateTime.Now.ToString("MM/dd/yyyy") + "' and college_code='" + clgcode + "') update Late_attendance set FromTime='" + DateTime.Now.ToString("h:mm:ss tt") + "' where App_No='" + app_no + "' and FromDate='" + DateTime.Now.ToString("MM/dd/yyyy") + "' and college_code='" + clgcode + "' else    insert into Late_attendance (App_No,FromDate,FromTime,college_code,AttnMonth,AttnYear)values('" + app_no + "','" + DateTime.Now.ToString("MM/dd/yyyy") + "','" + DateTime.Now.ToString("h:mm:ss tt") + "','" + clgcode + "','" + DateTime.Now.ToString("MM") + "','" + DateTime.Now.ToString("yyyy") + "')";
                                    //q1 = "insert into Late_attendance (App_No,FromDate,FromTime,college_code,AttnMonth,AttnYear)values('" + app_no + "','" + DateTime.Now.ToString("MM/dd/yyyy") + "','" + DateTime.Now.ToString("h:mm:ss tt") + "','" + clgcode + "','" + DateTime.Now.ToString("MM") + "','" + DateTime.Now.ToString("yyyy") + "')";
                                    int up = DA.update_method_wo_parameter(q1, "text");
                                    if (up != 0)
                                    {
                                        details.Add("1");
                                        details.Add(type);
                                    }
                                }


                                q1 = " select * from Late_attendance ";
                                ds1.Clear();
                                ds1 = DA.select_method_wo_parameter(q1, "Text");
                                ds1.Tables[0].DefaultView.RowFilter = "AttnMonth='" + DateTime.Now.ToString("MM") + "' and AttnYear ='" + DateTime.Now.ToString("yyyy") + "' and College_Code='" + clgcode + "' and App_No='" + app_no + "'  ";
                                alreadyregstud1 = ds1.Tables[0].DefaultView;
                                details.Add(Convert.ToString(ds.Tables[0].Rows[0]["Reg_no"]));

                                details.Add(Convert.ToString(alreadyregstud1.Count));
                                if (alreadyregstud1.Count >= 4)
                                {
                                    sms(clgcode, app_no, Convert.ToString(ds.Tables[0].Rows[0]["Stud_Name"]), DateTime.Now.ToString("h:mm:ss tt"), DateTime.Now.ToString("MM/dd/yyyy"));
                                }
                              
                            }
                            else
                            {
                                details.Clear();
                                details.Add("");
                                details.Add("");
                                details.Add("");
                                details.Add("");
                                details.Add(Convert.ToString(lbl_studimage.ImageUrl));
                                details.Add("0"); details.Add("");
                            }
                        }
                        
                    }
            return details;
        }
        catch
        {
            details.Clear();
            details.Add("");
            details.Add("");
            details.Add("");
            details.Add(""); details.Add("");
            details.Add("0");
            return details;
        }
    }

    //public void load_ddlrollno()
    //{
    //    try
    //    {
    //        System.Web.UI.WebControls.ListItem lst1 = new System.Web.UI.WebControls.ListItem("Roll No", "0");
    //        System.Web.UI.WebControls.ListItem lst2 = new System.Web.UI.WebControls.ListItem("Reg No", "1");
    //        System.Web.UI.WebControls.ListItem lst3 = new System.Web.UI.WebControls.ListItem("Admin No", "2");
    //        System.Web.UI.WebControls.ListItem lst4 = new System.Web.UI.WebControls.ListItem("App No", "3");
    //        System.Web.UI.WebControls.ListItem lst5 = new System.Web.UI.WebControls.ListItem("Name", "4");
    //        System.Web.UI.WebControls.ListItem lst51 = new System.Web.UI.WebControls.ListItem("Hostel Id", "5");

    //        //Roll Number or Reg Number or Admission No or Application Number
    //        ddlrollno.Items.Clear();
    //        string insqry1 = "select value from Master_Settings where settings='Roll No' and usercode ='" + usercode + "' --and college_code ='" + collegecode + "'";

    //        int save1 = Convert.ToInt32(d2.GetFunction(insqry1));

    //        if (save1 == 1)
    //        {
    //            //Roll No
    //            ddlrollno.Items.Add(lst1);
    //        }


    //        insqry1 = "select value from Master_Settings where settings='Register No' and usercode ='" + usercode + "' --and college_code ='" + collegecode + "'";
    //        save1 = Convert.ToInt32(d2.GetFunction(insqry1));
    //        if (save1 == 1)
    //        {
    //            //RegNo
    //            ddlrollno.Items.Add(lst2);
    //        }

    //        insqry1 = "select value from Master_Settings where settings='Admission No' and usercode ='" + usercode + "' --and college_code ='" + collegecode + "'";
    //        save1 = Convert.ToInt32(d2.GetFunction(insqry1));
    //        if (save1 == 1)
    //        {
    //            //Admission No - Roll Admit
    //            ddlrollno.Items.Add(lst3);
    //        }

    //        insqry1 = "select value from Master_Settings where settings='Application No' and usercode ='" + usercode + "' --and college_code ='" + collegecode + "'";
    //        save1 = Convert.ToInt32(d2.GetFunction(insqry1));

    //        if (save1 == 1)
    //        {
    //            //App Form Number - Application Number
    //            ddlrollno.Items.Add(lst4);

    //        }
    //        insqry1 = "select value from Master_Settings where settings='Hostel Id' and usercode ='" + usercode + "' --and college_code ='" + collegecode + "'";
    //        save1 = Convert.ToInt32(d2.GetFunction(insqry1));

    //        if (save1 == 1)
    //        {
    //            //App Form Number - Application Number
    //            ddlrollno.Items.Add(lst51);

    //        }

    //        if (ddlrollno.Items.Count == 0)
    //        {
    //            ddlrollno.Items.Add(lst1);
    //        }
    //        ddlrollno.Items.Add(lst5);
    //        switch (Convert.ToUInt32(ddlrollno.SelectedItem.Value))
    //        {
    //            case 0:
    //                txtno.Attributes.Add("placeholder", "Roll No");
    //                chosedmode = 0;
    //                break;
    //            case 1:
    //                txtno.Attributes.Add("placeholder", "Reg No");
    //                chosedmode = 1;
    //                break;
    //            case 2:
    //                txtno.Attributes.Add("placeholder", "Admin No");
    //                chosedmode = 2;
    //                break;
    //            case 3:
    //                txtno.Attributes.Add("placeholder", "App No");
    //                chosedmode = 3;
    //                break;
    //            case 5:
    //                txtno.Attributes.Add("placeholder", "Hostel Id");
    //                chosedmode = 5;
    //                break;
    //        }


    //    }
    //    catch { }
    //}

    public static void sms(string clgcode, string appno, string outin, string time, string date)
    {
        DAccess2 d2 = new DAccess2();
        DataSet ds = new DataSet();//barath21.04.17
        string user_id = d2.GetFunction("select SMS_User_ID from Track_Value where college_code='" + clgcode + "'");
        //string getval = d2.GetUserapi(user_id);
        //string[] spret = getval.Split('-');
        //if (spret.GetUpperBound(0) == 1)
        //{
        //    SenderID = spret[0].ToString();
        //    Password = spret[1].ToString();
        //    Session["api"] = user_id;
        //    Session["senderid"] = SenderID;
        //}
        string mobilenos = string.Empty;

        string strmsg = " Your Son/Daughter Mr/Miss." + outin + " Entered  from College on " + date + " at " + time;
           
             
        accessNew();
        if (sms_mom == "1")
        {
            string momnum = d2.GetFunction("select parentM_Mobile from applyn where app_no='" + appno + "'");
            mobilenos = momnum;
            // mobilenos = "9585698019";
           // mobilenos = "9751471583";
            if (mobilenos != "")//barath21.04.17
            {
                int m = d2.send_sms(user_id, clgcode, usercode, mobilenos, strmsg, "0");
                //barath 20.04.17
                //string strpath = "http://dnd.airsmsmarketing.info/api/sendmsg.php?user=" + user_id + "&pass=" + Password + "&sender=" + SenderID + "&phone=" + mobilenos + "&text=" + strmsg + "&priority=ndnd&stype=normal";
                //smsreport(strpath, isst);
            }
        }
        if (sms_dad == "2")
        {
            string fathernum = d2.GetFunction("select parentF_Mobile from applyn where app_no='" + appno + "'");
            mobilenos = fathernum;
            // mobilenos = "9585698019";
          //  mobilenos = "9751471583";
            if (mobilenos != "")//barath21.04.17
            {
             int m= d2.send_sms(user_id, clgcode, usercode, mobilenos, strmsg, "0");  
                
                //barath 20.04.17
                //string strpath = "http://dnd.airsmsmarketing.info/api/sendmsg.php?user=" + user_id + "&pass=" + Password + "&sender=" + SenderID + "&phone=" + mobilenos + "&text=" + strmsg + "&priority=ndnd&stype=normal";
                //smsreport(strpath, isst);
            }
        }
        if (sms_stud == "3")
        {
            string studnum = d2.GetFunction("select Student_Mobile from applyn where app_no='" + appno + "'");
            mobilenos = studnum;
            // mobilenos = "9585698019";
         //   mobilenos = "9751471583";
            if (mobilenos != "")//barath21.04.17
            {
                int m = d2.send_sms(user_id, clgcode, usercode, mobilenos, strmsg, "0");   //barath 20.04.17
                //string strpath = "http://dnd.airsmsmarketing.info/api/sendmsg.php?user=" + user_id + "&pass=" + Password + "&sender=" + SenderID + "&phone=" + mobilenos + "&text=" + strmsg + "&priority=ndnd&stype=normal";
                //smsreport(strpath, isst);
            }
        }
    }

    public static void accessNew()
    {
        try
        {
            DAccess2 dnew = new DAccess2();
            DataSet dsms = new DataSet();
            string query = "";
            string Master1 = "";
            string stud = "";
            string values = "";
            string sms = "";
            string sms1 = "";
            string sms2 = "";
            sms_mom = "";
            sms_dad = "";
            sms_stud = "";
            if (group_user.Trim() != "" && group_user.Trim() != "0")
            {
                Master1 = group_user;
                query = "select * from Master_Settings where settings ='SMS Mobile Rights' and Group_code ='" + Master1 + "'";
            }
            else if (usercode.Trim() != "")
            {
                Master1 = usercode;
                query = "select * from Master_Settings where settings ='SMS Mobile Rights' and usercode ='" + Master1 + "'";
            }
            dsms = dnew.select_method_wo_parameter(query, "Text");
            if (dsms.Tables[0].Rows.Count > 0)
            {
                for (int i = 0; i < dsms.Tables[0].Rows.Count; i++)
                {
                    string val = Convert.ToString(dsms.Tables[0].Rows[i]["value"]);
                    string[] split = val.Split(',');
                    if (split.Length == 1)
                    {
                        sms = split[0];
                        if (sms == "1")
                        {
                            sms_mom = sms;
                        }
                        else if (sms == "2")
                        {
                            sms_dad = sms;
                        }
                        else if (sms == "3")
                        {
                            sms_stud = sms;
                        }
                    }
                    else if (split.Length == 2)
                    {
                        sms = split[0];
                        sms1 = split[1];
                        if (sms == "1")
                        {
                            sms_mom = sms;
                        }
                        else if (sms == "2")
                        {
                            sms_dad = sms;
                        }
                        else if (sms == "3")
                        {
                            sms_stud = sms;
                        }
                        if (sms1 == "1")
                        {
                            sms_mom = sms1;
                        }
                        else if (sms1 == "2")
                        {
                            sms_dad = sms1;
                        }
                        else if (sms1 == "3")
                        {
                            sms_stud = sms1;
                        }
                    }
                    else
                    {
                        sms = split[0];
                        sms1 = split[1];
                        sms2 = split[2];
                        sms_mom = "1";
                        sms_dad = "2";
                        sms_stud = "3";
                    }
                }
            }
        }
        catch
        {
        }
    }
}
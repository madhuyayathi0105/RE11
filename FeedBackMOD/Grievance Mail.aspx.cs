using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;
using InsproDataAccess;
using System.Net.Mail;


public partial class AttendanceMOD_Grievance_Mail : System.Web.UI.Page
{
    string usercode = string.Empty;
    string collegecode1 = string.Empty;
    static string colleg = string.Empty;
    string collegecode = string.Empty;
    static string clgcode = string.Empty;
    string singleuser = string.Empty;
    string group_user = string.Empty;
    string staffcodesession = string.Empty;
      DAccess2 d2 = new DAccess2();
    protected void Page_Load(object sender, EventArgs e)
    {
         if (Session["collegecode"] == null)
        {
            Response.Redirect("~/Default.aspx");
        }
        usercode = Session["usercode"].ToString();
        collegecode1 = Session["collegecode"].ToString();
        colleg = collegecode1;
        singleuser = Session["single_user"].ToString();
        group_user = Session["group_code"].ToString();
        collegecode1 = Session["collegecode"].ToString();
        clgcode = Session["collegecode"].ToString();
        staffcodesession = Session["Staff_Code"].ToString();
        if (!IsPostBack)
        {
            if (staffcodesession == "" || staffcodesession == null)
            {
                lblsub.Visible = false;
                txtsub.Visible = false;
                lblbody.Visible = false;
                txtbody.Visible = false;
                btnsend.Visible = false;
                Response.Write("You Are not a Valid Staff");
                return;
            }
            else
            {
                lblsub.Visible = true;
                txtsub.Visible = true;
                lblbody.Visible = true;
                txtbody.Visible = true;
                btnsend.Visible = true;
            }

        }
    }
 
     protected void btnsend_Click(object sender, EventArgs e)
    {
        try
        {
            
          string  strmsg = txtbody.Text;
          string send_mail = string.Empty;
          string send_pw = string.Empty;
            //string strquery = "select massemail,masspwd from collinfo where college_code ='' ";
            //ds1.Dispose();
            //ds1.Reset();
            //ds1 = d2.select_method(strquery, hat, "Text");
           // if (ds1.Tables[0].Rows.Count > 0)
          //  {
                //send_mail = Convert.ToString(ds1.Tables[0].Rows[0]["massemail"]);
                //send_pw = Convert.ToString(ds1.Tables[0].Rows[0]["masspwd"]);
          //send_mail = "palpaporange@gmail.com";
          //send_pw = "palpap1234";
                //send_mail = "bmageshwariece96@gmail.com";
                //send_pw = "mageshsuba";
           // }
            
            //else
            //{
              
                //lblmsg.Visible = true;
                //lblmsg.Text = "Please Set From EMail ID And Password First And Then Proceed.";
                ////lblsendmail.Text = "Please Set From EMail ID And Password First And Then Proceed.";
                ////lblsendmail.Visible = true;
                //return;
          //  }
           send_mail = d2.GetFunction("select MasterValue from CO_MasterValues where MasterCriteria ='Grievance From Mail' and CollegeCode  in (select college_code from staffmaster where staff_code='" + staffcodesession + "')");//select * from staff_appl_master where appl_no in(select appl_no from staffmaster where staff_code='" + staffcodesession + "')
           send_pw = d2.GetFunction("select MasterValue from CO_MasterValues where MasterCriteria ='Grievance From Mail Password' and CollegeCode  in (select college_code from staffmaster where staff_code='" + staffcodesession + "')");
           
            string tomail = d2.GetFunction("select MasterValue from CO_MasterValues where MasterCriteria ='Grievance To Mail' and CollegeCode  in (select college_code from staffmaster where staff_code='" + staffcodesession + "')");
            if (tomail != "")
            {
                string[] spl = tomail.Split(',');
                if (spl.Length > 0)
                {
                    for (int mai = 0; mai < spl.Length; mai++)
                    {


                        if (send_mail != "")
                        {
                            string strstuname = d2.GetFunction("select staff_name from staffmaster where staff_code='" + staffcodesession + "'");
                            SmtpClient Mail = new SmtpClient("smtp.gmail.com", 587);
                            Mail.EnableSsl = true;
                            MailMessage mailmsg = new MailMessage();
                            MailAddress mfrom = new MailAddress(send_mail);

                            mailmsg.From = mfrom;
                            mailmsg.To.Add(spl[mai]);
                            mailmsg.Subject = txtsub.Text.ToString();
                            //magesh
                            mailmsg.IsBodyHtml = false;
                            //mailmsg.Body = "First line" + Environment.NewLine + "Second line";
                            //mailmsg.Body = "First line <br /> Second line";
                            // mailmsg.Body = "Dear";
                            // mailmsg.Body = mailmsg.Body + strstuname ;
                            mailmsg.Body = "Dear Sir/Mam," + "\n\n" + "I am " + strstuname + "-" + staffcodesession + " ";
                          //  mailmsg.Body = strstuname;
                            mailmsg.Body = mailmsg.Body + ',' + strmsg;
                            mailmsg.Body = mailmsg.Body + "\n\n" + "Thank You..";//magesh
                            // mailmsg.Body = mailmsg.Body + "<br/><br/>Thank You...<br/><br/>";
                            byte[] documentBinary = new byte[0];
                            byte[] attchementfile = new byte[0];
                            string filenameMail = "";



                            //====================//
                            Mail.EnableSsl = true;
                            Mail.UseDefaultCredentials = false;
                            NetworkCredential credentials = new NetworkCredential(send_mail, send_pw);
                            Mail.Credentials = credentials;
                            Mail.Send(mailmsg);

                        }
                    }
                }
            }
        }
        catch
        {
            ScriptManager.RegisterStartupScript(this, typeof(Page), UniqueID, "alert('Send Email Failed')", true);
        }

    }
}
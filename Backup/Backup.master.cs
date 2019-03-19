using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Backup : System.Web.UI.MasterPage
{
    DAccess2 da = new DAccess2();
    string sql = string.Empty;
    protected void Page_Load(object sender, EventArgs e)
    {
        //string strPreviousPage = "";
        //if (Request.UrlReferrer != null)
        //{
        //    strPreviousPage = Request.UrlReferrer.Segments[Request.UrlReferrer.Segments.Length - 1];
        //}
        //if (strPreviousPage == "")
        //{
        //    Session["IsLogin"] = "0";
        //    Response.Redirect("~/Default.aspx");
        //}

        if (Session["collegecode"] == null)
        {
            Response.Redirect("~/Default.aspx");
        }
        if (!IsPostBack)
        {
            string collegecode = Session["Collegecode"].ToString();

            string collegeName = da.GetFunction("select collname from collinfo where  college_code='" + collegecode + "'");

            if (da.GetFunction("select LinkValue from New_InsSettings where LinkName='UseCommonCollegeCode' and user_code ='" + Session["UserCode"].ToString() + "'") == "1")
            {
                string comCOde = da.GetFunction("select com_name from collinfo where  college_code='" + collegecode + "'").Trim();
                collegeName = (comCOde.Length > 1) ? comCOde : collegeName;
            }
            lblcolname.Text = collegeName;

            //lblcolname.Text = da.GetFunction("select collname from collinfo where  college_code='" + collegecode + "'");
            string color = da.GetFunction("select Farvour_color from user_color where user_code='" + Session["UserCode"].ToString() + "' and college_code='" + collegecode + "'");
            string colornew = "";
            if (color.Trim() == "0")
            {
                colornew = "#06d995";
            }
            else
            {
                colornew = color;
                //prewcolor.Attributes.Add("style", "background-color:" + colornew + ";");
            }
            MainDivIdValue.Attributes.Add("style", "background-color:" + colornew + ";border-bottom: 6px solid lightyellow; box-shadow: 0 0 11px -4px; height: 58px; left: 0; position: fixed; z-index: 2; top: 0; width: 100%;");
            if (Convert.ToString(Session["Staff_Code"]) != "")
            {
                img_stfphoto.ImageUrl = "~/Handler/staffphoto.ashx?staff_code=" + Session["Staff_Code"];
                imgstdphoto.ImageUrl = "~/Handler/staffphoto.ashx?staff_code=" + Session["Staff_Code"];
                string stfdescode = "";
                sql = "select desig_code from stafftrans where staff_code='" + Convert.ToString(Session["Staff_Code"]) + "' and latestrec=1";
                stfdescode = da.GetFunction(sql);


                if (stfdescode != "" && stfdescode != null)
                {
                    string stfdesigname = "";
                    sql = "select dm.desig_name from desig_master dm where dm.desig_code='" + stfdescode.ToString() + "' and collegecode=" + Session["collegecode"].ToString();
                    stfdesigname = da.GetFunction(sql);



                    string staffname = "";
                    sql = "select staff_name from staffmaster where staff_code='" + Session["staff_code"] + "'";
                    staffname = da.GetFunction(sql);

                    string deptname = "";
                    sql = "select dt.dept_acronym from Department dt,stafftrans st where dt.Dept_code=st.dept_code and staff_code='" + Session["staff_code"] + "' and latestrec=1";
                    deptname = da.GetFunction(sql);
                    lbslstaffname.Text = Convert.ToString(staffname);
                    lbldesignation.Text = Convert.ToString(stfdesigname);
                    lbldept.Text = Convert.ToString(deptname);

                }
            }
            else
            {


                string staffname = "";
                sql = "select full_name from usermaster where user_code='" + Session["UserCode"] + "'";
                staffname = da.GetFunction(sql);
                lbslstaffname.Text = Convert.ToString(staffname);

            }
        }

    }

    protected void lb2_Click(object sender, EventArgs e)
    {
        if (Session["Entry_Code"] != null)
        {
            string entryCode = Session["Entry_Code"].ToString();
            da.userTimeOut(entryCode);
        }
        Session.Abandon();
        Session.Clear();
        Session.RemoveAll();
        System.Web.Security.FormsAuthentication.SignOut();
        Response.Redirect("~/Default.aspx", false);

    }

}


using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.IO;


public partial class DatabaseBackup : System.Web.UI.Page
{
    
    protected void Page_Load(object sender, EventArgs e)
    {
        
        
    }
    protected void btnbackup_Click(object sender, EventArgs e)
    {
        //string path = Server.MapPath("~" + System.IO.Path.GetFileName(FileUpload1.FileName));
        //FileUpload1.SaveAs(path);
        //string appPath = HttpContext.Current.Server.MapPath("~");        
        string backupDIR = txtlocation.Text;
        if (!System.IO.Directory.Exists(backupDIR))
        {
            System.IO.Directory.CreateDirectory(backupDIR);
        }
        try
        {
            string connectionstring = System.Configuration.ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
            SqlConnection con = new SqlConnection();
            SqlCommand sqlcmd = new SqlCommand();
            SqlDataAdapter da = new SqlDataAdapter();
            DataTable dt = new DataTable();
            con = new SqlConnection(connectionstring);

            con.Open();
            sqlcmd = new SqlCommand("backup database insproplus to disk='" + backupDIR + "\\" + DateTime.Now.ToString("ddMMyyyy_HHmmss") + ".Bak'", con);
            sqlcmd.CommandTimeout = 500000;
            sqlcmd.ExecuteNonQuery();
            con.Close();
           
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "Myscript1", @"alert('Database Backup done successfully.');", true);

        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "Myscript1", @"alert('Error Occured During DB backup process.');", true);

        }
    }

    protected void btnrestore_Click(object sender, EventArgs e)
    {
        try
        {
            string connectionstring = System.Configuration.ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
            SqlConnection con = new SqlConnection();
            SqlCommand sqlcmd = new SqlCommand();
            SqlDataAdapter da = new SqlDataAdapter();
            DataTable dt = new DataTable();
            con = new SqlConnection(connectionstring);
            if (FileUpload1.HasFile)
            {
                if (System.IO.Path.GetExtension(FileUpload1.FileName) == ".Bak" || System.IO.Path.GetExtension(FileUpload1.FileName) == ".bak")
                {
                    string fname = Server.MapPath("~" + System.IO.Path.GetExtension(FileUpload1.FileName));
                    FileUpload1.SaveAs(fname);
                    
                    con.Open();
                    sqlcmd = new SqlCommand("restore database insproplus from disk='" + fname + "\\" + DateTime.Now.ToString("ddMMyyyy_HHmmss") + ".Bak'", con);
                    sqlcmd.CommandTimeout = 500000;
                    sqlcmd.ExecuteNonQuery();
                    con.Close();
                    ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "Myscript1", @"alert('Database Restore done successfully.');", true);
                }
            }
        }
        catch
        {
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "Myscript1", @"alert('Error Occured During DB restore process.');", true);
        }

    }
}
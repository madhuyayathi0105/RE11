using System;
using System.Data;
using System.Web.UI.WebControls;
using InsproDataAccess;
using System.Drawing;

public partial class Transport_availability : System.Web.UI.Page
{
    InsproDirectAccess dirAcc = new InsproDirectAccess();
    string collegecode = string.Empty;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["collegecode"] == null)
            Session["collegecode"] = lbl_collegecode.Text;
        if (!IsPostBack)
            lbl_collegecode.Text = Convert.ToString(Session["collegecode"]);
        collegecode = Convert.ToString(Session["collegecode"]);
        lblSeatDateTime.Text = "Date : " + DateTime.Now.ToString("dd/MM/yyyy") + " Time : " + DateTime.Now.ToLongTimeString();
        loadSearch();
    }
    private void loadSearch()
    {
        try
        {
            transport_grid.Visible = false;
            transport_grid.DataSource = null;
            transport_grid.DataBind();
            DataTable transport_dt = new DataTable();
            string[] headername = { "Stage Name", "Route Name", "Boarding Time", "Availability" };
            foreach (string header in headername)
            { transport_dt.Columns.Add(header); }
            DataSet transportdet = dirAcc.selectDataSet(" select s.Stage_Name,r.Route_ID,Arr_Time,r.Veh_ID, (v.firstyearstudent-COUNT(distinct re.App_No))currentavailable,convert(varchar(50),(COUNT(distinct re.App_No)))available,convert(varchar, v.firstyearstudent)allot  from Stage_Master s join RouteMaster r on CONVERT(nvarchar(max), s.Stage_id)=CONVERT(varchar(max), r.Stage_Name) join vehicle_master v  on v.Veh_ID=r.Veh_ID and r.Route_ID =v.Route left join Registration re on v.Veh_ID=re.VehID where r.sess='M' group by s.Stage_Name,r.Route_ID,Arr_Time,r.Veh_ID,v.Veh_ID,v.firstyearstudent order by s.Stage_Name,r.Route_ID");
            if (transportdet.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow dr1 in transportdet.Tables[0].Rows)
                {
                    DataRow dr = transport_dt.NewRow();
                    dr[0] = Convert.ToString(dr1["Stage_Name"]);
                    dr[1] = Convert.ToString(dr1["Route_ID"]);
                    dr[2] = Convert.ToString(dr1["Arr_Time"]);
                    dr[3] = Convert.ToString(dr1["currentavailable"]);
                    transport_dt.Rows.Add(dr);
                }
            }
            transport_grid.Visible = true;
            transport_grid.DataSource = transport_dt;
            transport_grid.DataBind();
        }
        catch { }
    }
    protected void transport_grid_OnRowDataBound(object sender, GridViewRowEventArgs w)
    {
        try
        {
            if (w.Row.RowType == DataControlRowType.DataRow)
            {
                for (int colI = 2; colI < w.Row.Cells.Count; colI++)
                {
                    w.Row.Cells[colI].HorizontalAlign = HorizontalAlign.Center;
                }
            }
        }
        catch { }
    }
}
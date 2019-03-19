using System;
using System.Data;
using System.Web.UI.WebControls;
using InsproDataAccess;
using System.Drawing;
using System.Text;
public partial class AdmissionMod_HostelStatus : System.Web.UI.Page
{
    InsproDirectAccess dirAcc = new InsproDirectAccess();
    string collegecode = string.Empty;
    protected void Page_Load(object sender, EventArgs e)
    {
        //if (Session["collegecode"] == null)
        //{
        //   // Response.Redirect("~/Default.aspx");
        //}
        //collegecode = Convert.ToString(Session["collegecode"]);

        lblSeatDateTime.Text = "Date : " + DateTime.Now.ToString("dd/MM/yyyy") + " Time : " + DateTime.Now.ToLongTimeString();
        loadSearch();
    }
    private void loadSearch()
    {
        try
        {
            divHostel.InnerHtml = string.Empty;

            string collegeCode = string.Empty;
            string batchYear = string.Empty;
            string eduLevel = string.Empty;
            string courseCode = string.Empty;
            string categCode = string.Empty;
            string criteriaCode = string.Empty;

            string[] resVal = dirAcc.selectScalarString("SELECT LinkValue FROM New_InsSettings WHERE LinkName='ADMISSIONCOURSESELECTIONSETTINGS' ").Split('$');//AND college_code='" + collegecode + "'

            if (resVal.Length == 6)
            {
                collegeCode = resVal[0];
                batchYear = resVal[1];
                eduLevel = resVal[2];
                courseCode = resVal[3];
                categCode = resVal[4];
                criteriaCode = resVal[5];

                string selQ = "select Hostelbuildingfk,HostelName,HostelMasterPK from HM_HostelMaster where CollegeCode=" + collegeCode + " ";
                DataTable dtHostel = dirAcc.selectDataTable(selQ);
                if (dtHostel.Rows.Count > 0)
                {
                    StringBuilder sbHost = new StringBuilder();
                    sbHost.Append("<table rules='all' cellpadding=2  cellspacing=0 style=' border:1px solid black; font-weight:bold; font-size:20px; width:900px;'>");
                    sbHost.Append("<tr style=' background-color:#0CA6CA; '><td style='width:60px;'><center>S.No</center></td><td><center>Hostel</center></td></tr>");
                    int sNo = 1;
                    foreach (DataRow drHostel in dtHostel.Rows)
                    {
                        string hostelName = Convert.ToString(drHostel["HostelName"]);
                        string hostelbuildingFk = Convert.ToString(drHostel["Hostelbuildingfk"]);
                        sbHost.Append("<tr><td><center>" + sNo + "</center></td><td><center>" + hostelName + "</center>");

                        DataTable dtBuilding = new DataTable();
                        dtBuilding = dirAcc.selectDataTable("select b.Building_Name as [Building Name], Room_type as [Room Type],COUNT(isnull(r.students_allowed,0) - isnull(Avl_Student,0)) as Total  from Building_Master b, Floor_Master f, Room_Detail r where b.Building_Name=f.Building_Name and f.Floor_Name=r.Floor_Name and r.Building_Name=b.Building_Name and b.Code in (" + hostelbuildingFk + ") and b.College_Code=" + collegeCode + " group by  Room_type, b.Building_Name order by b.Building_Name");
                        sbHost.Append("<br><table style='width:840px;' cellspacing=0>");
                        sbHost.Append("<tr  style=' background-color:#06D995; text-align:center; '><td>S.No</td><td>Building Name</td><td>Room Type</td><td>Available</td></tr>");
                        if (dtBuilding.Rows.Count > 0)
                        {
                            for (int builI = 0; builI < dtBuilding.Rows.Count; builI++)
                            {
                                sbHost.Append("<tr><td><center>" + (builI + 1) + "</center></td><td>" + Convert.ToString(dtBuilding.Rows[builI]["Building Name"]) + "</td><td><center>" + Convert.ToString(dtBuilding.Rows[builI]["Room Type"]) + "</center></td><td style='text-align:right;'>" + Convert.ToString(dtBuilding.Rows[builI]["Total"]) + "</td></tr>");
                            }
                        }
                        sbHost.Append("</table></td></tr>");
                        sNo++;
                    }
                    sbHost.Append("</table>");

                    divHostel.InnerHtml = sbHost.ToString();
                }
            }
        }
        catch
        {
        }
    }
}
using System;
using System.Data;
using System.Web.UI.WebControls;
using InsproDataAccess;
using System.Drawing;
using System.Text;
public partial class AdmissionMod_SeatStatus : System.Web.UI.Page
{
    InsproDirectAccess dirAcc = new InsproDirectAccess();
    string collegecode = string.Empty;
    protected void Page_Load(object sender, EventArgs e)
    {

        //collegecode = Convert.ToString(Session["collegecode"]);
        if (!IsPostBack)
        {
            if (Session["collegecode"] == null)
            {
                Response.Redirect("~/Default.aspx");
            }
        }

        lblSeatDateTime.Text = "Date : " + DateTime.Now.ToString("dd/MM/yyyy") + " Time : " + DateTime.Now.ToLongTimeString();
        loadSearch();
    }
    private void loadSearch()
    {
        try
        {
            gridBranSeat.Visible = false;
            gridBranSeat.DataSource = null;
            gridBranSeat.DataBind();

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

                string collegeName = dirAcc.selectScalarString("select collname from collinfo where college_code='" + collegeCode + "'").ToUpper();
                StringBuilder courseName = new StringBuilder();
                DataTable DtCourseName = dirAcc.selectDataTable("select course_name from course where Course_Id in (" + courseCode + ")");
                if (DtCourseName.Rows.Count > 0)
                {
                    for (int dtcoursname = 0; dtcoursname < DtCourseName.Rows.Count; dtcoursname++)
                    {
                        courseName.Append(DtCourseName.Rows[dtcoursname]["course_name"] + " / ");
                    }
                    if (courseName.Length > 0)
                    {
                        courseName.Remove(courseName.Length - 1, 1);
                    }
                }
                string Stream = dirAcc.selectScalarString("select TextVal  from TextValTable where TextCode ='" + categCode + "'");
                string Cous = courseName.ToString().Trim('/');
                if (Cous.Trim().ToUpper() == "LAW")
                {
                    Stream = "";
                }
                else
                {
                    Stream = ":  " + Stream;
                }
                ShowSpan.InnerHtml = "Admission to " + courseName.ToString().Trim('/') + " Programmes 2017-18" + Stream + "";
                divHdr.InnerHtml = "";
                //"<table cellspacing=10 style='font-size:22px; font-weight:bold;'><tr><td  style='color:#EC8033;'>Batch :</td><td style='color:#008000;'> " + batchYear + "</td><td  style='color:#EC8033;'>Course :</td><td style='color:#008000;'>" + courseName + "</td></tr></table>";
                DataTable dtBran = dirAcc.selectDataTable("select d.Degree_Code,dt.dept_name,isnull(d.No_Of_seats,0) as NoOfSeats,course_Name,c.course_id  from Degree d, Department dt,course c where dt.Dept_Code=d.Dept_Code and c.Course_Id=d.Course_Id and d.college_code='" + collegeCode + "' and c.Edu_Level='" + eduLevel + "' and d.Course_Id in (" + courseCode + ")  and d.Degree_Code not in (68,70) order by c.course_id asc,Dept_Name asc ");

                DataTable dtStudRankCrit = dirAcc.selectDataTable("select MasterCode,MasterValue from CO_MasterValues where MasterCriteria='StudRankCriteria' and collegeCode ='" + collegeCode + "'");
                //and MasterValue='AIR'

                DataTable dtBranSeat = new DataTable();
                dtBranSeat.Columns.Add("PROGRAMMES");
                //dtBranSeat.Columns.Add("Max Seat");
                //dtBranSeat.Columns.Add("Alloted");
                //dtBranSeat.Columns.Add("Alloted");

                for (int i = 0; i < dtStudRankCrit.Rows.Count; i++)
                {
                    string criteriaVal = Convert.ToString(dtStudRankCrit.Rows[i]["MasterValue"]) + "#" + Convert.ToString(dtStudRankCrit.Rows[i]["MasterCode"]);
                    dtBranSeat.Columns.Add(criteriaVal);
                }

                //dtBranSeat.Columns.Add("Available");

                DataTable dtPrevSaved = dirAcc.selectDataTable("SELECT Tot_Seat,Quota,Degree_Code,NoOfSeats,allotedSeats FROM seattype_cat WHERE  Batch_Year='" + batchYear + "' AND collegeCode='" + collegeCode + "' AND Category_Code='" + categCode + "'");
                DataTable dtPrevSavedAll = dirAcc.selectDataTable("SELECT Tot_Seat,Quota,Degree_Code,NoOfSeats,allotedSeats FROM seattype_cat WHERE  Batch_Year='" + batchYear + "' AND collegeCode='" + collegeCode + "' ");

                if (dtBran.Rows.Count > 0)
                {
                    for (int i = 0; i < dtBran.Rows.Count; i++)
                    {
                        string degCode = Convert.ToString(dtBran.Rows[i]["Degree_Code"]);

                        DataRow dr = dtBranSeat.NewRow();

                        dr["PROGRAMMES"] = "" + Convert.ToString(dtBran.Rows[i]["course_Name"]) + " " + Convert.ToString(dtBran.Rows[i]["dept_name"]);// +"#" + degCode;
                        // dr["Max Seat"] = Convert.ToString(dtBran.Rows[i]["NoOfSeats"]);
                        string allotedVal = "0";
                        dtPrevSavedAll.DefaultView.RowFilter = " Degree_Code='" + degCode + "' ";
                        DataTable dtCurSum = dtPrevSavedAll.DefaultView.ToTable();

                        if (dtCurSum.Rows.Count > 0)
                        {
                            var obj = dtCurSum.Compute("SUM(Tot_Seat)", string.Empty);
                            allotedVal = obj.ToString();
                        }

                        //dr["Alloted"] = allotedVal;

                        if (dtPrevSaved.Rows.Count > 0)
                        {
                            int allotTotal = 0;
                            int totalSeats = 0;
                            for (int colI = 1; colI < (dtBranSeat.Columns.Count); colI++)
                            {
                                string[] curColName = Convert.ToString(dtBranSeat.Columns[colI].ColumnName).Split('#');

                                dtPrevSaved.DefaultView.RowFilter = "Quota ='" + curColName[1] + "' and Degree_Code='" + degCode + "'";
                                DataTable dtCurCrit = dtPrevSaved.DefaultView.ToTable();
                                if (dtCurCrit.Rows.Count > 0)
                                {
                                    string totalVal = Convert.ToString(dtCurCrit.Rows[0]["NoOfSeats"]).Trim();
                                    int.TryParse(totalVal, out allotTotal);

                                    string allocVal = Convert.ToString(dtCurCrit.Rows[0]["allotedSeats"]).Trim();
                                    int allocIntVal = 0; int.TryParse(allocVal, out allocIntVal);



                                    string critVal = Convert.ToString(dtCurCrit.Rows[0]["Tot_Seat"]).Trim();
                                    int critIntVal = 0; int.TryParse(critVal, out critIntVal);
                                    critIntVal -= allocIntVal;

                                    if (critIntVal < 0)
                                        critIntVal = 0;

                                    totalSeats += critIntVal;
                                    dr[Convert.ToString(dtBranSeat.Columns[colI].ColumnName)] = critIntVal;
                                }

                                //Total Alloted
                            }

                            //dr["Alloted"] = allotTotal;
                            //dr["Available"] = totalSeats;
                        }
                        dtBranSeat.Rows.Add(dr);
                    }
                    for (int colI = 1; colI < (dtBranSeat.Columns.Count); colI++)
                    {
                        string[] curColName = Convert.ToString(dtBranSeat.Columns[colI].ColumnName).Split('#');
                        dtBranSeat.Columns[colI].ColumnName = curColName[0];
                    }

                    gridBranSeat.Visible = true;
                    gridBranSeat.DataSource = dtBranSeat;
                    gridBranSeat.DataBind();
                }
            }
        }
        catch
        {
        }
    }
    protected void gridBranSeat_OnRowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {
            if (e.Row.RowType == DataControlRowType.Header)
            {
                e.Row.Cells[1].ForeColor = Color.White;
                e.Row.Cells[2].ForeColor = Color.White;
                e.Row.Cells[3].ForeColor = Color.White;
                e.Row.Cells[4].ForeColor = Color.White;
                e.Row.Cells[5].ForeColor = Color.White;
            }
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                e.Row.HorizontalAlign = HorizontalAlign.Center;
                e.Row.Cells[1].HorizontalAlign = HorizontalAlign.Left;
                e.Row.Cells[1].Attributes.Add("style", "margin-left:20px;");
                //e.Row.Cells[1].BackColor = ColorTranslator.FromHtml("#48B04D");
                //e.Row.Cells[2].BackColor = ColorTranslator.FromHtml("#FE6598");
                //e.Row.Cells[e.Row.Cells.Count - 1].BackColor = ColorTranslator.FromHtml("#06D995");
            }
        }
        catch { }
    }
}
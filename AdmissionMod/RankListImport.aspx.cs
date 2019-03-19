using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Collections;
using System.Configuration;
using System.Drawing;
using System.IO;
using System.Data.OleDb;

public partial class AdmissionMod_RankListImport : System.Web.UI.Page
{
    DataSet ds = new DataSet();
    DAccess2 d2 = new DAccess2();
    protected void Page_Load(object sender, EventArgs e)
    {

    }

    protected void btnUpload_Click(object sender, EventArgs e)
    {
        try
        {
            SetMethod();
            //using (Stream stream = this.fileUpload.FileContent as Stream)
            //{
            //    string extension = Path.GetFileName(fileUpload.PostedFile.FileName);
            //    if (extension.Trim() != "")
            //    {
            //        string path = Server.MapPath("~/Report/" + System.IO.Path.GetFileName(fileUpload.FileName));
            //        fileUpload.SaveAs(path);
            //        ds.Clear();
            //        ds = Excelconvertdataset(path);
            //        DataView dview = new DataView();
            //        if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            //        {
            //            string Query = "select TextVal,TextCode from TextValTable where TextCriteria ='ADMst'";
            //            Query += "  select MasterCode,MasterValue from CO_MasterValues where MasterCriteria ='StudRankCriteria'";
            //            DataSet dset = d2.select_method_wo_parameter(Query, "Text");
            //            DataView DNew = new DataView();
            //            DataView DRnk = new DataView();
            //            for (int introw = 0; introw < ds.Tables[0].Rows.Count; introw++)
            //            {
            //                string ApplicationNo = Convert.ToString(ds.Tables[0].Rows[introw][0]);
            //                string CombinedScore = Convert.ToString(ds.Tables[0].Rows[introw][3]);
            //                string CombinedScoreII = Convert.ToString(ds.Tables[0].Rows[introw][4]);
            //                string AppNo = d2.GetFunction(" select app_no  from applyn a,ST_Student_Mark_Detail st where a.app_no =st.ST_AppNo and a.app_formno ='" + ApplicationNo + "' ");
            //                if (AppNo.Trim() != "" && AppNo.Trim() != "0")
            //                {
            //                    double Com = 0;
            //                    double ComII = 0;
            //                    double.TryParse(Convert.ToString(CombinedScore), out Com);
            //                    double.TryParse(Convert.ToString(CombinedScoreII), out ComII);

            //                    string InsertQury = "  update ST_Student_Mark_Detail set CombinedScore ='" + Convert.ToString(Math.Round(Com, 4)) + "',CombinedScoreSII ='" + Convert.ToString(Math.Round(ComII, 4)) + "' where ST_AppNo ='" + AppNo + "'";
            //                    int upd = d2.update_method_wo_parameter(InsertQury, "Text");
            //                    string StreamI = string.Empty;
            //                    dset.Tables[0].DefaultView.RowFilter = "TextVal='Stream I'";
            //                    DNew = dset.Tables[0].DefaultView;
            //                    if (DNew.Count > 0)
            //                    {
            //                        StreamI = Convert.ToString(DNew[0]["TextCode"]);
            //                    }
            //                    int Row = 4;
            //                    DataTable dt = ds.Tables[0].DefaultView.ToTable();
            //                    for (int intCol = 0; intCol < 4; intCol++)
            //                    {
            //                        Row++;
            //                        string ColName = dt.Columns[Row].ColumnName;

            //                        dset.Tables[1].DefaultView.RowFilter = "MasterValue='" + ColName.Trim() + "'";
            //                        DRnk = dset.Tables[1].DefaultView;
            //                        if (DRnk.Count > 0)
            //                        {
            //                            string Criteriacode = Convert.ToString(DRnk[0]["MasterCode"]);
            //                            string Rank = Convert.ToString(ds.Tables[0].Rows[introw][Row]);
            //                            if (Criteriacode.Trim() != "" && Rank.Trim() != "" && Rank.Trim() != "-")
            //                            {
            //                                InsertQury = "if exists (select ST_AppNo from ST_RankTable where ST_RankCriteria ='" + Criteriacode + "' and ST_AppNo ='" + AppNo + "' and ST_Stream ='" + StreamI + "') update ST_RankTable set ST_Rank ='" + Rank + "' where ST_RankCriteria ='" + Criteriacode + "' and ST_AppNo ='" + AppNo + "' and ST_Stream ='" + StreamI + "' else insert into ST_RankTable (ST_AppNo,ST_RankCriteria,ST_Rank,ST_Stream) values ('" + AppNo + "','" + Criteriacode + "','" + Rank + "','" + StreamI + "')";

            //                                upd = d2.update_method_wo_parameter(InsertQury, "Text");
            //                            }
            //                        }
            //                    }
            //                    string StreamII = string.Empty;
            //                    dset.Tables[0].DefaultView.RowFilter = "TextVal='Stream II'";
            //                    DNew = dset.Tables[0].DefaultView;
            //                    if (DNew.Count > 0)
            //                    {
            //                        StreamII = Convert.ToString(DNew[0]["TextCode"]);
            //                    }
            //                    for (int intCol = 0; intCol < 4; intCol++)
            //                    {
            //                        Row++;
            //                        string ColName = dt.Columns[Row].ColumnName;
            //                        ColName = ColName.Trim('1');
            //                        dset.Tables[1].DefaultView.RowFilter = "MasterValue='" + ColName.Trim() + "'";
            //                        DRnk = dset.Tables[1].DefaultView;
            //                        if (DRnk.Count > 0)
            //                        {
            //                            string Criteriacode = Convert.ToString(DRnk[0]["MasterCode"]);
            //                            string Rank = Convert.ToString(ds.Tables[0].Rows[introw][Row]);
            //                            if (Criteriacode.Trim() != "" && Rank.Trim() != "" && Rank.Trim() != "-" && StreamII.Trim() != "")
            //                            {
            //                                InsertQury = "if exists (select ST_AppNo from ST_RankTable where ST_RankCriteria ='" + Criteriacode + "' and ST_AppNo ='" + AppNo + "' and ST_Stream ='" + StreamII + "') update ST_RankTable set ST_Rank ='" + Rank + "' where ST_RankCriteria ='" + Criteriacode + "' and ST_AppNo ='" + AppNo + "' and ST_Stream ='" + StreamII + "' else insert into ST_RankTable (ST_AppNo,ST_RankCriteria,ST_Rank,ST_Stream) values ('" + AppNo + "','" + Criteriacode + "','" + Rank + "','" + StreamII + "')";

            //                                upd = d2.update_method_wo_parameter(InsertQury, "Text");
            //                            }
            //                        }
            //                    }
            //                }
            //            }
            //        }
            //    }
            //}
        }
        catch
        {

        }
    }

    public void SetMethod()
    {
        try
        {
            using (Stream stream = this.fileUpload.FileContent as Stream)
            {
                string extension = Path.GetFileName(fileUpload.PostedFile.FileName);
                if (extension.Trim() != "")
                {
                    string path = Server.MapPath("~/Report/" + System.IO.Path.GetFileName(fileUpload.FileName));
                    fileUpload.SaveAs(path);
                    ds.Clear();
                    ds = Excelconvertdataset(path);
                    DataView dview = new DataView();
                    if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                    {
                        string Query = "select TextVal,TextCode from TextValTable where TextCriteria ='ADMst'";
                        Query += "  select MasterCode,MasterValue from CO_MasterValues where MasterCriteria ='StudRankCriteria'";
                        DataSet dset = d2.select_method_wo_parameter(Query, "Text");
                        DataView DNew = new DataView();
                        DataView DRnk = new DataView();
                        for (int introw = 0; introw < ds.Tables[0].Rows.Count; introw++)
                        {
                            string ApplicationNo = Convert.ToString(ds.Tables[0].Rows[introw][0]);
                            string CombinedScore = Convert.ToString(ds.Tables[0].Rows[introw][3]);
                            string AppNo = d2.GetFunction(" select app_no  from applyn a,ST_Student_Mark_Detail st where a.app_no =st.ST_AppNo and a.app_formno ='" + ApplicationNo + "' ");
                            if (AppNo.Trim() != "" && AppNo.Trim() != "0")
                            {
                                double Com = 0;
                                double.TryParse(Convert.ToString(CombinedScore), out Com);

                                string InsertQury = "  update ST_Student_Mark_Detail set CombinedScore ='" + Convert.ToString(Math.Round(Com, 4)) + "' where ST_AppNo ='" + AppNo + "'";
                                int upd = d2.update_method_wo_parameter(InsertQury, "Text");
                                string StreamI = string.Empty;
                                dset.Tables[0].DefaultView.RowFilter = "TextVal='Stream I'";
                                DNew = dset.Tables[0].DefaultView;
                                if (DNew.Count > 0)
                                {
                                    StreamI = Convert.ToString(DNew[0]["TextCode"]);
                                }
                                int Row = 3;
                                DataTable dt = ds.Tables[0].DefaultView.ToTable();
                                for (int intCol = 0; intCol < 1; intCol++)
                                {
                                    Row++;
                                    string ColName = dt.Columns[Row].ColumnName;
                                    dset.Tables[1].DefaultView.RowFilter = "MasterValue='" + ColName.Trim() + "'";
                                    DRnk = dset.Tables[1].DefaultView;
                                    if (DRnk.Count > 0)
                                    {
                                        string Criteriacode = Convert.ToString(DRnk[0]["MasterCode"]);
                                        string Rank = Convert.ToString(ds.Tables[0].Rows[introw][Row]);
                                        if (Criteriacode.Trim() != "" && Rank.Trim() != "" && Rank.Trim() != "-")
                                        {
                                            InsertQury = "if exists (select ST_AppNo from ST_RankTable where ST_RankCriteria ='" + Criteriacode + "' and ST_AppNo ='" + AppNo + "' and ST_Stream ='" + StreamI + "') update ST_RankTable set ST_Rank ='" + Rank + "' where ST_RankCriteria ='" + Criteriacode + "' and ST_AppNo ='" + AppNo + "' and ST_Stream ='" + StreamI + "' else insert into ST_RankTable (ST_AppNo,ST_RankCriteria,ST_Rank,ST_Stream) values ('" + AppNo + "','" + Criteriacode + "','" + Rank + "','" + StreamI + "')";

                                            upd = d2.update_method_wo_parameter(InsertQury, "Text");
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }
        catch
        {

        }
    }

    public static DataSet Excelconvertdataset(string path)
    {
        DataSet ds3 = new DataSet();
        string StrSheetName = "";

        string excelConnectionString = @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + path + ";Extended Properties='Excel 12.0;HDR=YES;IMEX=1;';";
        OleDbConnection excelConnection = new OleDbConnection(excelConnectionString);
        try
        {
            OleDbDataAdapter adapter = new OleDbDataAdapter();
            if (excelConnection.State == ConnectionState.Closed)
                excelConnection.Open();
            DataTable dtSheets = excelConnection.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);
            if (dtSheets != null && dtSheets.Rows.Count > 0)
            {
                StrSheetName = dtSheets.Rows[0].ItemArray[2].ToString();
            }
            if (!string.IsNullOrEmpty(StrSheetName))
            {
                OleDbCommand cmd = new OleDbCommand("Select * from [" + StrSheetName + "]", excelConnection);
                adapter = new OleDbDataAdapter(cmd);
                adapter.Fill(ds3, "excelData");
            }
        }
        catch
        {
        }
        finally
        {
            if (excelConnection.State != ConnectionState.Closed)
                excelConnection.Close();
        }
        return ds3;
    }
}
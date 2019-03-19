using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Collections;
using System.Data;
using System.Threading;
using System.Drawing;

public partial class OfficeMOD_HostelAllotmentToBatchaspx : System.Web.UI.Page
{

    #region Field Declaration

    Hashtable ht = new Hashtable();

    string userCode = string.Empty;
    string collegeCode = string.Empty;
    string singleUser = string.Empty;
    string groupUserCode = string.Empty;
    string qry = string.Empty;
    string qryCollegeCode = string.Empty;

    string batchYear = string.Empty;
    string buildingName = string.Empty;
    string floorName = string.Empty;
    string roomName = string.Empty;

    string buildingCode = string.Empty;
    string floorCode = string.Empty;
    string roomCode = string.Empty;

    bool isSchool = false;
    int selected = 0;

    DAccess2 d2 = new DAccess2();
    DataSet ds = new DataSet();

    #endregion

    #region Page Load

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (Session["collegecode"] == null)
            {
                Response.Redirect("~/Default.aspx");
            }
            userCode = Convert.ToString(Session["usercode"]).Trim();
            collegeCode = Convert.ToString(Session["collegecode"]).Trim();
            singleUser = Convert.ToString(Session["single_user"]).Trim();
            groupUserCode = Convert.ToString(Session["group_code"]).Trim();
            if (!IsPostBack)
            {
                lblErrSearch.Text = string.Empty;
                lblErrSearch.Visible = false;
                divPopupAlert.Visible = false;
                lblAlertMsg.Text = string.Empty;
                divMainContent.Visible = false;
                chkSelectAllRooms.Checked = false;
                BindCollege();
                BindBuilding();
                BindFloorName();
                BindBatch();
            }
        }
        catch (ThreadAbortException tt)
        {

        }
        catch (Exception ex)
        {
            lblErrSearch.Text = Convert.ToString(ex);
            lblErrSearch.Visible = true;
            d2.sendErrorMail(ex, (ddlCollege.Items.Count > 0 ? Convert.ToString(ddlCollege.SelectedValue).Trim() : ((Session["collegecode"] != null) ? Convert.ToString(Session["collegecode"]).Trim() : "13")), System.IO.Path.GetFileName(Request.Url.AbsolutePath).ToString());
        }
    }

    #endregion

    #region Bind Header

    private void BindCollege()
    {
        try
        {
            lblErrSearch.Text = string.Empty;
            lblErrSearch.Visible = false;
            divMainContent.Visible = false;
            chkSelectAllRooms.Checked = false;
            string columnfield = string.Empty;
            groupUserCode = (Session["group_code"] != null) ? Convert.ToString(Session["group_code"]).Trim() : "";
            if (groupUserCode.Contains(';'))
            {
                string[] group_semi = groupUserCode.Split(';');
                groupUserCode = Convert.ToString(group_semi[0]);
            }
            if ((Convert.ToString(groupUserCode).Trim() != "") && Session["group_code"] != null && (Convert.ToString(Session["group_code"]).Trim() != "") && (Convert.ToString(Session["group_code"]).Trim() != "0") && (Convert.ToString(Session["group_code"]).Trim() != "-1"))
            {
                columnfield = " and group_code='" + groupUserCode + "'";
            }
            else if (Session["usercode"] != null)
            {
                columnfield = " and user_code='" + Convert.ToString(Session["usercode"]).Trim() + "'";
            }
            ht.Clear();
            ht.Add("column_field", Convert.ToString(columnfield));
            ds.Dispose();
            ds.Clear();
            ds.Reset();
            ds = d2.select_method("bind_college", ht, "sp");
            ddlCollege.Items.Clear();
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                ddlCollege.DataSource = ds;
                ddlCollege.DataTextField = "collname";
                ddlCollege.DataValueField = "college_code";
                ddlCollege.DataBind();
                ddlCollege.SelectedIndex = 0;
            }
            else
            {
                lblErrSearch.Text = "Set college rights to the staff";
                lblErrSearch.Visible = true;
                return;
            }
        }
        catch (Exception ex)
        {
            lblErrSearch.Text = Convert.ToString(ex);
            lblErrSearch.Visible = true;
            d2.sendErrorMail(ex, (ddlCollege.Items.Count > 0 ? Convert.ToString(ddlCollege.SelectedValue).Trim() : ((Session["collegecode"] != null) ? Convert.ToString(Session["collegecode"]).Trim() : "13")), System.IO.Path.GetFileName(Request.Url.AbsolutePath).ToString());
        }
    }

    private void BindBuilding()
    {
        try
        {
            lblErrSearch.Text = string.Empty;
            lblErrSearch.Visible = false;
            divMainContent.Visible = false;
            chkSelectAllRooms.Checked = false;
            ds.Clear();
            ht.Clear();
            collegeCode = string.Empty;
            ddlBuildName.Items.Clear();
            ddlBuildName.Enabled = false;
            if (ddlCollege.Items.Count > 0)
            {
                collegeCode = Convert.ToString(ddlCollege.SelectedValue).Trim();
            }
            else
                collegeCode = ((Session["collegecode"] != null) ? Convert.ToString(Session["collegecode"]).Trim() : "");
            if (!string.IsNullOrEmpty(collegeCode))
            {
                qry = "select distinct Building_Name,Code from Building_Master where College_Code='" + collegeCode + "' order by Building_Name";
                ds.Clear();
                ds = d2.select_method_wo_parameter(qry, "text");
            }
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                ddlBuildName.DataSource = ds;
                ddlBuildName.DataTextField = "Building_Name";
                ddlBuildName.DataValueField = "Code";
                ddlBuildName.DataBind();
                ddlBuildName.Enabled = true;
            }
        }
        catch (Exception ex)
        {
            lblErrSearch.Text = Convert.ToString(ex);
            lblErrSearch.Visible = true;
            d2.sendErrorMail(ex, (ddlCollege.Items.Count > 0 ? Convert.ToString(ddlCollege.SelectedValue).Trim() : ((Session["collegecode"] != null) ? Convert.ToString(Session["collegecode"]).Trim() : "13")), System.IO.Path.GetFileName(Request.Url.AbsolutePath).ToString());
        }
    }

    private void BindFloorName()
    {
        try
        {
            lblErrSearch.Text = string.Empty;
            lblErrSearch.Visible = false;
            divMainContent.Visible = false;
            chkSelectAllRooms.Checked = false;
            ds.Clear();
            ht.Clear();
            collegeCode = string.Empty;
            buildingName = string.Empty;
            buildingCode = string.Empty;
            ddlFloorName.Items.Clear();
            ddlFloorName.Enabled = false;
            if (ddlCollege.Items.Count > 0)
            {
                collegeCode = Convert.ToString(ddlCollege.SelectedValue).Trim();
            }
            else
                collegeCode = ((Session["collegecode"] != null) ? Convert.ToString(Session["collegecode"]).Trim() : "");
            if (ddlBuildName.Items.Count > 0)
            {
                buildingCode = Convert.ToString(ddlBuildName.SelectedValue).Trim();
                buildingName = Convert.ToString(ddlBuildName.SelectedItem.Text).Trim();
            }
            if (!string.IsNullOrEmpty(collegeCode) && !string.IsNullOrEmpty(buildingName))
            {
                qry = "select distinct LEN(Floor_Name),Floor_Name,Floorpk from Floor_Master where Building_Name='" + buildingName + "' and College_Code='" + collegeCode + "' order by LEN(Floor_Name)";
                ds.Clear();
                ds = d2.select_method_wo_parameter(qry, "text");
            }
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                ddlFloorName.DataSource = ds;
                ddlFloorName.DataTextField = "Floor_Name";
                ddlFloorName.DataValueField = "Floorpk";
                ddlFloorName.DataBind();
                ddlFloorName.Enabled = true;
            }
        }
        catch (Exception ex)
        {
            lblErrSearch.Text = Convert.ToString(ex);
            lblErrSearch.Visible = true;
            d2.sendErrorMail(ex, (ddlCollege.Items.Count > 0 ? Convert.ToString(ddlCollege.SelectedValue).Trim() : ((Session["collegecode"] != null) ? Convert.ToString(Session["collegecode"]).Trim() : "13")), System.IO.Path.GetFileName(Request.Url.AbsolutePath).ToString());
        }
    }

    private void BindBatch()
    {
        try
        {
            lblErrSearch.Text = string.Empty;
            lblErrSearch.Visible = false;
            divMainContent.Visible = false;
            chkSelectAllRooms.Checked = false;
            ds.Dispose();
            ds.Reset();
            ds.Clear();
            ddlBatch.Items.Clear();
            if (ddlCollege.Items.Count > 0)
            {
                selected = 0;
                qryCollegeCode = string.Empty;
                string collegeCodeNew = string.Empty;
                foreach (ListItem li in ddlCollege.Items)
                {
                    if (li.Selected)
                    {
                        selected++;
                        if (string.IsNullOrEmpty(collegeCodeNew.Trim()))
                        {
                            collegeCodeNew = "'" + li.Value.Trim() + "'";
                        }
                        else
                        {
                            collegeCodeNew += ",'" + li.Value.Trim() + "'";
                        }
                    }
                }
                if (!string.IsNullOrEmpty(collegeCodeNew) && selected > 0)
                {
                    qryCollegeCode = " and r.college_code in(" + collegeCodeNew + ")";
                }
                ds = d2.select_method_wo_parameter("select distinct r.Batch_Year from applyn r where r.batch_year<>'-1' and r.batch_year<>'' " + qryCollegeCode + " order by r.Batch_Year desc", "text");
                if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    ddlBatch.DataSource = ds;
                    ddlBatch.DataTextField = "Batch_Year";
                    ddlBatch.DataValueField = "Batch_Year";
                    ddlBatch.DataBind();
                    ddlBatch.SelectedIndex = 0;
                }
            }
        }
        catch (Exception ex)
        {
            lblErrSearch.Text = Convert.ToString(ex);
            lblErrSearch.Visible = true;
            d2.sendErrorMail(ex, (ddlCollege.Items.Count > 0 ? Convert.ToString(ddlCollege.SelectedValue).Trim() : ((Session["collegecode"] != null) ? Convert.ToString(Session["collegecode"]).Trim() : "13")), System.IO.Path.GetFileName(Request.Url.AbsolutePath).ToString());
        }
    }

    #endregion

    #region Index ChangeEvent

    protected void ddlCollege_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            lblErrSearch.Text = string.Empty;
            lblErrSearch.Visible = false;
            divPopupAlert.Visible = false;
            lblAlertMsg.Text = string.Empty;
            divMainContent.Visible = false;
            chkSelectAllRooms.Checked = false;
            BindBuilding();
            BindFloorName();
            BindBatch();
        }
        catch (Exception ex)
        {
            lblErrSearch.Text = Convert.ToString(ex);
            lblErrSearch.Visible = true;
            d2.sendErrorMail(ex, (ddlCollege.Items.Count > 0 ? Convert.ToString(ddlCollege.SelectedValue).Trim() : ((Session["collegecode"] != null) ? Convert.ToString(Session["collegecode"]).Trim() : "13")), System.IO.Path.GetFileName(Request.Url.AbsolutePath).ToString());
        }
    }

    protected void ddlBuildName_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            lblErrSearch.Text = string.Empty;
            lblErrSearch.Visible = false;
            divPopupAlert.Visible = false;
            lblAlertMsg.Text = string.Empty;
            divMainContent.Visible = false;
            chkSelectAllRooms.Checked = false;
            BindFloorName();
        }
        catch (Exception ex)
        {
            lblErrSearch.Text = Convert.ToString(ex);
            lblErrSearch.Visible = true;
            d2.sendErrorMail(ex, (ddlCollege.Items.Count > 0 ? Convert.ToString(ddlCollege.SelectedValue).Trim() : ((Session["collegecode"] != null) ? Convert.ToString(Session["collegecode"]).Trim() : "13")), System.IO.Path.GetFileName(Request.Url.AbsolutePath).ToString());
        }
    }

    protected void ddlFloorName_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            lblErrSearch.Text = string.Empty;
            lblErrSearch.Visible = false;
            divPopupAlert.Visible = false;
            lblAlertMsg.Text = string.Empty;
            divMainContent.Visible = false;
            chkSelectAllRooms.Checked = false;
        }
        catch (Exception ex)
        {
            lblErrSearch.Text = Convert.ToString(ex);
            lblErrSearch.Visible = true;
            d2.sendErrorMail(ex, (ddlCollege.Items.Count > 0 ? Convert.ToString(ddlCollege.SelectedValue).Trim() : ((Session["collegecode"] != null) ? Convert.ToString(Session["collegecode"]).Trim() : "13")), System.IO.Path.GetFileName(Request.Url.AbsolutePath).ToString());
        }
    }

    protected void ddlBatch_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            lblErrSearch.Text = string.Empty;
            lblErrSearch.Visible = false;
            divPopupAlert.Visible = false;
            lblAlertMsg.Text = string.Empty;
            btnShowRooms_Click(sender, e);
        }
        catch (Exception ex)
        {
            lblErrSearch.Text = Convert.ToString(ex);
            lblErrSearch.Visible = true;
            d2.sendErrorMail(ex, (ddlCollege.Items.Count > 0 ? Convert.ToString(ddlCollege.SelectedValue).Trim() : ((Session["collegecode"] != null) ? Convert.ToString(Session["collegecode"]).Trim() : "13")), System.IO.Path.GetFileName(Request.Url.AbsolutePath).ToString());
        }
    }

    #endregion

    #region Click

    #region Close Popup

    protected void btnPopAlertClose_Click(object sender, EventArgs e)
    {
        try
        {
            lblErrSearch.Text = string.Empty;
            lblErrSearch.Visible = false;
            divPopupAlert.Visible = false;
            lblAlertMsg.Text = string.Empty;
        }
        catch (Exception ex)
        {
            lblErrSearch.Text = Convert.ToString(ex);
            lblErrSearch.Visible = true;
            d2.sendErrorMail(ex, (ddlCollege.Items.Count > 0 ? Convert.ToString(ddlCollege.SelectedValue).Trim() : ((Session["collegecode"] != null) ? Convert.ToString(Session["collegecode"]).Trim() : "13")), System.IO.Path.GetFileName(Request.Url.AbsolutePath).ToString());
        }
    }

    #endregion Close Popup

    #region Show Rooms

    protected void dlRoomDetails_ItemDataBound(object sender, DataListItemEventArgs e)
    {
        try
        {
            if (e.Item.ItemType == ListItemType.Item ||
                e.Item.ItemType == ListItemType.AlternatingItem)
            {
                string batchYearSet = string.Empty;
                if (ddlBatch.Items.Count > 0)
                {
                    batchYearSet = Convert.ToString(ddlBatch.SelectedItem.Text).Trim();
                }
                string value = Convert.ToString((e.Item.FindControl("lblCheckedBatch") as Label).Text).Trim();
                ((System.Web.UI.WebControls.CheckBox)(e.Item.FindControl("chkRoomChecked"))).Checked = false;
                ((System.Web.UI.WebControls.Label)(e.Item.FindControl("lblRoomName"))).ForeColor = Color.Green;
                if (!string.IsNullOrEmpty(batchYearSet) && string.Equals(value, batchYearSet) && value != "0")
                {
                    ((System.Web.UI.WebControls.CheckBox)(e.Item.FindControl("chkRoomChecked"))).Checked = true;
                    ((System.Web.UI.WebControls.Label)(e.Item.FindControl("lblRoomName"))).ForeColor = Color.Red;
                }
                else if (!string.IsNullOrEmpty(value) && value != "0")
                {
                    ((System.Web.UI.WebControls.Label)(e.Item.FindControl("lblRoomName"))).ForeColor = Color.Blue;
                }
            }
        }
        catch (Exception ex)
        {
            lblErrSearch.Text = Convert.ToString(ex);
            lblErrSearch.Visible = true;
            d2.sendErrorMail(ex, (ddlCollege.Items.Count > 0 ? Convert.ToString(ddlCollege.SelectedValue).Trim() : ((Session["collegecode"] != null) ? Convert.ToString(Session["collegecode"]).Trim() : "13")), System.IO.Path.GetFileName(Request.Url.AbsolutePath).ToString());
        }
    }

    protected void chkSelectAllRooms_CheckedChanged(object sender, EventArgs e)
    {
        try
        {
            if (dlRoomDetails.Items.Count > 0)
            {
                for (int rows = 0; rows < dlRoomDetails.Items.Count; rows++)
                {
                    if (((System.Web.UI.WebControls.CheckBox)(dlRoomDetails.Items[rows].FindControl("chkRoomChecked"))).Checked != chkSelectAllRooms.Checked)
                        ((System.Web.UI.WebControls.CheckBox)(dlRoomDetails.Items[rows].FindControl("chkRoomChecked"))).Checked = chkSelectAllRooms.Checked;
                }
            }
        }
        catch (Exception ex)
        {
            lblErrSearch.Text = Convert.ToString(ex);
            lblErrSearch.Visible = true;
            d2.sendErrorMail(ex, (ddlCollege.Items.Count > 0 ? Convert.ToString(ddlCollege.SelectedValue).Trim() : ((Session["collegecode"] != null) ? Convert.ToString(Session["collegecode"]).Trim() : "13")), System.IO.Path.GetFileName(Request.Url.AbsolutePath).ToString());
        }
    }

    protected void btnShowRooms_Click(object sender, EventArgs e)
    {
        try
        {
            lblErrSearch.Text = string.Empty;
            lblErrSearch.Visible = false;
            divPopupAlert.Visible = false;
            lblAlertMsg.Text = string.Empty;
            divMainContent.Visible = false;
            chkSelectAllRooms.Checked = false;
            collegeCode = string.Empty;
            buildingName = string.Empty;
            buildingCode = string.Empty;
            batchYear = string.Empty;
            floorName = string.Empty;
            floorCode = string.Empty;
            DataSet dsRoomDetails = new DataSet();
            if (ddlCollege.Items.Count > 0)
            {
                collegeCode = Convert.ToString(ddlCollege.SelectedValue).Trim();
            }
            else
            {
                lblAlertMsg.Text = "No " + lblCollege.Text + " were Found";
                lblAlertMsg.Visible = true;
                divPopupAlert.Visible = true;
                return;
            }
            if (ddlBuildName.Items.Count > 0)
            {
                buildingName = Convert.ToString(ddlBuildName.SelectedItem.Text).Trim();
                buildingCode = Convert.ToString(ddlBuildName.SelectedValue).Trim();
            }
            else
            {
                lblAlertMsg.Text = "No " + lblBuilding.Text + " were Found";
                lblAlertMsg.Visible = true;
                divPopupAlert.Visible = true;
                return;
            }
            if (ddlFloorName.Items.Count > 0)
            {
                floorName = Convert.ToString(ddlFloorName.SelectedItem.Text).Trim();
                floorCode = Convert.ToString(ddlFloorName.SelectedValue).Trim();
            }
            else
            {
                lblAlertMsg.Text = "No " + lblFloorName.Text + " were Found";
                lblAlertMsg.Visible = true;
                divPopupAlert.Visible = true;
                return;
            }
            //if (ddlBatch.Items.Count > 0)
            //{
            //}
            //else
            //{
            //    lblAlertMsg.Text = "No " + lblBatch.Text + " were Found";
            //    lblAlertMsg.Visible = true;
            //    divPopupAlert.Visible = true;
            //    return;
            //}
            if (!string.IsNullOrEmpty(collegeCode) && !string.IsNullOrEmpty(buildingCode) && !string.IsNullOrEmpty(buildingName) && !string.IsNullOrEmpty(floorCode) && !string.IsNullOrEmpty(floorName))
            {
                qry = "select distinct rm.Roompk,rm.Room_Name as Room,rm.Room_Name+' ('+case when LTRIM(RTRIM(isnull(rm.Room_type,'')))='' then '--' else LTRIM(RTRIM(isnull(rm.Room_type,''))) end+')' as Room_Name,LEN(rm.Room_Name),batchYear from Room_Detail rm where rm.College_Code='" + collegeCode + "' and rm.Building_Name='" + buildingName + "' and rm.Floor_Name='" + floorName + "' order by LEN(rm.Room_Name) asc,rm.Room_Name";
                dsRoomDetails = d2.select_method_wo_parameter(qry, "text");
            }
            if (dsRoomDetails.Tables.Count > 0 && dsRoomDetails.Tables[0].Rows.Count > 0)
            {
                dlRoomDetails.DataSource = dsRoomDetails.Tables[0];
                dlRoomDetails.DataBind();
                dlRoomDetails.Visible = true;
                divMainContent.Visible = true;
            }
            else
            {
                lblAlertMsg.Text = "No Record(s) were Found";
                lblAlertMsg.Visible = true;
                divPopupAlert.Visible = true;
                return;
            }
        }
        catch (Exception ex)
        {
            lblErrSearch.Text = Convert.ToString(ex);
            lblErrSearch.Visible = true;
            d2.sendErrorMail(ex, (ddlCollege.Items.Count > 0 ? Convert.ToString(ddlCollege.SelectedValue).Trim() : ((Session["collegecode"] != null) ? Convert.ToString(Session["collegecode"]).Trim() : "13")), System.IO.Path.GetFileName(Request.Url.AbsolutePath).ToString());
        }
    }

    #endregion Show Rooms

    #region Allot Rooms

    protected void btnSet_Click(object sender, EventArgs e)
    {
        try
        {
            lblErrSearch.Text = string.Empty;
            lblErrSearch.Visible = false;
            divPopupAlert.Visible = false;
            lblAlertMsg.Text = string.Empty;
            bool isSaved = false;
            batchYear = string.Empty;
            if (ddlBatch.Items.Count > 0)
            {
                batchYear = Convert.ToString(ddlBatch.SelectedItem.Text).Trim();
            }
            else
            {
                lblAlertMsg.Text = "No " + lblBatch.Text + " were Found";
                lblAlertMsg.Visible = true;
                divPopupAlert.Visible = true;
                return;
            }

            if (dlRoomDetails.Items.Count > 0)
            {
                int count = 0;
                for (int rows = 0; rows < dlRoomDetails.Items.Count; rows++)
                {
                    count++;
                    string roomCode = string.Empty;
                    roomCode = Convert.ToString((dlRoomDetails.Items[rows].FindControl("lblRoomId") as Label).Text).Trim();
                    if (((System.Web.UI.WebControls.CheckBox)(dlRoomDetails.Items[rows].FindControl("chkRoomChecked"))).Checked && !string.IsNullOrEmpty(roomCode) && !string.IsNullOrEmpty(batchYear))
                    {
                        qry = "if exists(select Roompk from Room_Detail where Roompk='" + roomCode + "') update Room_Detail set batchYear='" + batchYear + "' where Roompk='" + roomCode + "'";
                        int roomUpdate = d2.update_method_wo_parameter(qry, "text");
                        if (roomUpdate != 0)
                        {
                            isSaved = true;
                        }
                    }
                }
            }
            btnShowRooms_Click(sender, e);
            if (isSaved)
            {
                lblAlertMsg.Text = "Saved Successfully!!!";
                lblAlertMsg.Visible = true;
                divPopupAlert.Visible = true;
                return;
            }
            else
            {
                lblAlertMsg.Text = "Not Saved!!!";
                lblAlertMsg.Visible = true;
                divPopupAlert.Visible = true;
                return;
            }
        }
        catch (Exception ex)
        {
            lblErrSearch.Text = Convert.ToString(ex);
            lblErrSearch.Visible = true;
            d2.sendErrorMail(ex, (ddlCollege.Items.Count > 0 ? Convert.ToString(ddlCollege.SelectedValue).Trim() : ((Session["collegecode"] != null) ? Convert.ToString(Session["collegecode"]).Trim() : "13")), System.IO.Path.GetFileName(Request.Url.AbsolutePath).ToString());
        }
    }

    #endregion Allot Rooms

    #endregion

}
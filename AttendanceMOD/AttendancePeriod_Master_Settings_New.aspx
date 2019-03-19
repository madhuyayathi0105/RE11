<%@ Page Title="" Language="C#" MasterPageFile="~/AttendanceMOD/AttendanceSubSiteMaster.master"
    AutoEventWireup="true" CodeFile="AttendancePeriod_Master_Settings_New.aspx.cs"
    Inherits="AttendancePeriod_Master_Settings_New" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Assembly="FarPoint.Web.Spread" Namespace="FarPoint.Web.Spread" TagPrefix="FarPoint" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">
    <!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
    <html xmlns="http://www.w3.org/1999/xhtml">
    <head id="Head1">
        <title>Attendance Period Master Settings New</title>
        <link href="~/Styles/css/Commoncss.css" rel="Stylesheet" type="text/css" />
        <script src="//ajax.googleapis.com/ajax/libs/jquery/1.11.1/jquery.min.js"></script>
    </head>
    <body>
        <form id="form1">
        <asp:ScriptManager ID="ScriptManager1" runat="server">
        </asp:ScriptManager>
        <br />
        <center>
            <div>
                <span class="fontstyleheader" style="color: Green">Attendance Period Master Settings</span>
            </div>
        </center>
        <br />
        <center>
            <div id="div2" runat="server" visible="true">
                <center>
                    <table class="maintablestyle" style="width: 971px; height: 40px;">
                        <tr>
                            <td>
                                <asp:RadioButtonList ID="rblCourse" runat="server" Font-Bold="true" Font-Names="Book Antiqua"
                                    Font-Size="Medium" AutoPostBack="true" OnSelectedIndexChanged="rbCourse_SelectedIndexChanged"
                                    RepeatDirection="Horizontal">
                                </asp:RadioButtonList>
                            </td>
                            <td>
                                <asp:Label ID="lblBatch" Text="Batch" runat="server" Font-Bold="true" Font-Names="Book Antiqua"
                                    Font-Size="Medium"></asp:Label>
                            </td>
                            <td>
                                <asp:DropDownList ID="ddlBatch" runat="server" Font-Bold="true" Font-Names="Book Antiqua"
                                    Font-Size="Medium" AutoPostBack="true" OnSelectedIndexChanged="ddlBatch__SelectedIndexChanged">
                                </asp:DropDownList>
                            </td>
                            <td>
                                <asp:Label ID="lblDay" runat="server" Text="Day" Font-Bold="True" Font-Size="Medium"
                                    Font-Names="Book Antiqua"></asp:Label>
                            </td>
                            <td>
                                <asp:DropDownList ID="ddlDays" runat="server" Font-Bold="true" Font-Names="Book Antiqua"
                                    Font-Size="Medium" Width="100px" OnSelectedIndexChanged="ddlDays_SelectedIndexChanged"
                                    AutoPostBack="true">
                                </asp:DropDownList>
                            </td>
                        </tr>
                    </table>
                </center>
            </div>
        </center>
        <br />
        <asp:Label ID="lblerrmsg" runat="server" Text="" ForeColor="Red" Visible="False"
            Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium"></asp:Label>
        <br />
        <center>
            <FarPoint:FpSpread ID="FpDeptment" AutoPostBack="False" runat="server" Visible="false"
                BorderStyle="Solid" BorderWidth="0px" CssClass="spreadborder" ShowHeaderSelection="false"
                OnButtonCommand="FpDeptment_UpdateCommand">
                <Sheets>
                    <FarPoint:SheetView SheetName="Sheet1">
                    </FarPoint:SheetView>
                </Sheets>
            </FarPoint:FpSpread>
            <br />
            <table>
                <tr>
                    <td>
                        <asp:Button ID="btnSave" runat="server" Visible="false" Font-Bold="true" Font-Size="Medium"
                            Font-Names="Book Antiqua" Text="Save" OnClick="btnSave_Click" />
                    </td>
                </tr>
            </table>
        </center>
        <div id="imgdiv2" runat="server" visible="false" style="height: 100%; z-index: 1000;
            width: 100%; background-color: rgba(54, 25, 25, .2); position: absolute; top: 0;
            left: 0px;">
            <center>
                <div id="pnl2" runat="server" class="table" style="background-color: White; height: 120px;
                    width: 238px; border: 5px solid #0CA6CA; border-top: 25px solid #0CA6CA; margin-top: 200px;
                    border-radius: 10px;">
                    <center>
                        <table style="height: 100px; width: 100%">
                            <tr>
                                <td align="center">
                                    <asp:Label ID="lbl_popuperr" runat="server" Text="" Style="color: Red;" Font-Bold="true"
                                        Font-Size="Medium" Font-Names="Book Antiqua"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <center>
                                        <asp:Button ID="btn_errorclose" runat="server" CssClass=" textbox btn1 comm" Font-Size="Medium"
                                            Font-Bold="True" Font-Names="Book Antiqua" Style="height: 28px; width: 65px;"
                                            OnClick="btn_errorclose_Click" Text="Ok" />
                                    </center>
                                </td>
                            </tr>
                        </table>
                    </center>
                </div>
            </center>
        </div>
        </form>
    </body>
    </html>
</asp:Content>

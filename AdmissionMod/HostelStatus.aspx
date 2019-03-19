<%@ Page Language="C#" AutoEventWireup="true" CodeFile="HostelStatus.aspx.cs" Inherits="AdmissionMod_HostelStatus" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body style="background-color: #F0F0F0;">
    <form id="frmStatus" runat="server">
    <asp:ScriptManager ID="scrptMgr" runat="server">
    </asp:ScriptManager>
    <div>
        <center>
            <div>
                <asp:Image ID="img_college" runat="server" Visible="true" ImageUrl="~/Handler/Handler6.ashx"
                    CssClass="big" />
            </div>
            <span style='color: #373C8B; font-size: 28px; font-weight: bold;'>Seat Availability</span>
            <asp:UpdatePanel ID="updSeatStat" runat="server">
                <ContentTemplate>
                    <asp:Timer ID="tmrSeatStat" runat="server" Interval="1000">
                    </asp:Timer>
                    <table style="width: 100%;">
                        <tr>
                            <td style="text-align: right;">
                                <asp:Label ID="lblSeatDateTime" runat="server" Font-Bold="true" ForeColor="Red"></asp:Label>
                            </td>
                        </tr>
                    </table>
                    <div id="divHostel" runat="server">
                    </div>
                </ContentTemplate>
            </asp:UpdatePanel>
        </center>
    </div>
    </form>
</body>
</html>

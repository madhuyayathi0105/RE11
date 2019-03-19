<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Transport_availability.aspx.cs"
    Inherits="Transport_availability" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body style="background-color: #F0F0F0;">
    <form id="form1" runat="server">
    <asp:ScriptManager ID="scrptMgr" runat="server">
    </asp:ScriptManager>
    <div>
        <center>
            <asp:Label ID="lbl_collegecode" Visible="false" runat="server"></asp:Label>
            <div>
                <asp:Image ID="img_college" runat="server" Visible="false" ImageUrl="~/Handler/Handler6.ashx"
                    Style="width: 100%; height: 100px;" />
            </div>
            <span style='color: #373C8B; font-size: 28px; font-weight: bold;'>Transport Seat Availability</span>
            <asp:UpdatePanel ID="updSeatStat" runat="server">
                <contenttemplate>
                    <asp:Timer ID="tmrSeatStat" runat="server" Interval="1000">
                    </asp:Timer>
                    <table style="width: 100%;">
                        <tr>
                            <td style="text-align: right;">
                                <asp:Label ID="lblSeatDateTime" runat="server" Font-Bold="true" ForeColor="Red"></asp:Label>
                            </td>
                        </tr>
                    </table>
                    <asp:GridView ID="transport_grid" runat="server" AutoGenerateColumns="true" HeaderStyle-BackColor="#0CA6CA"
                        Style="font-size:large; font-weight: bold;" Width="950px" OnRowDataBound="transport_grid_OnRowDataBound">
                        <Columns>
                            <asp:TemplateField HeaderText="S.No">
                                <ItemTemplate>
                                    <asp:Label ID="lblSno" runat="server" Text='<%#Container.DisplayIndex+1 %>'></asp:Label>
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:TemplateField>
                        </Columns>
                        <AlternatingRowStyle BackColor="#E4E4E4" />
                    </asp:GridView>
                </contenttemplate>
            </asp:UpdatePanel>
        </center>
    </div>
    </form>
</body>
</html>

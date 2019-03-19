<%@ Page Language="C#" AutoEventWireup="true" CodeFile="SeatStatus.aspx.cs" Inherits="AdmissionMod_SeatStatus" %>

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
                <%--<asp:Image ID="img_college" runat="server" Visible="true" ImageUrl="~/Handler/Handler6.ashx"
                    CssClass="big" />--%>
            </div>
            <span style='color: #373C8B; font-size: 28px; font-weight: bold; font-family: Arial Narrow;'>
                SASTRA University</span>
            <br />
            <span id="ShowSpan" runat="server" style='font-size: large; font-weight: bold; font-family: Arial Narrow;'>
                Admission to B.Tech. / M.Tech.(5-year Intg.) Programmes 2017-18: STREAM II</span>
            <asp:UpdatePanel ID="updSeatStat" runat="server">
                <contenttemplate>
                    <asp:Timer ID="tmrSeatStat" runat="server" Interval="1000">
                    </asp:Timer>
                    <table style="width: 100%;">
                        <tr>
                            <td style="text-align: center;">
                                <div id="divHdr" runat="server">
                                
                                </div>
                            </td>
                            <td style="text-align: right;">
                                <asp:Label ID="lblSeatDateTime" runat="server" Visible="false" Font-Bold="true" ForeColor="Red"></asp:Label>
                            </td>
                        </tr>
                    </table>
                    <asp:GridView ID="gridBranSeat" runat="server" AutoGenerateColumns="true" HeaderStyle-BackColor="#0CA6CA"
                        Style="font-size:large; font-weight: bold; font-family:Arial Narrow;" Width="950px" OnRowDataBound="gridBranSeat_OnRowDataBound">
                        
                        <Columns>
                            <asp:TemplateField HeaderText="S.No" visible="false">
                                <ItemTemplate>
                                    <asp:Label ID="lblSno" runat="server" Text='<%#Container.DisplayIndex+1 %>'></asp:Label>
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                </contenttemplate>
            </asp:UpdatePanel>
        </center>
    </div>
    </form>
</body>
</html>

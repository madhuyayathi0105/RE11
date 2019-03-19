<%@ Page Title="" Language="C#" MasterPageFile="~/AttendanceMOD/AttendanceSubSiteMaster.master"
    AutoEventWireup="true" CodeFile="MasterTimeTable.aspx.cs" Inherits="MasterTimeTable" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Assembly="FarPoint.Web.Spread" Namespace="FarPoint.Web.Spread" TagPrefix="FarPoint" %>
<%@ Register Src="~/Usercontrols/PrintMaster.ascx" TagName="printmaster" TagPrefix="Insproplus" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">
<script type="text/javascript">
    function display() {
        document.getElementById('<%=lblexer.ClientID %>').innerHTML = "";
    }
    function display1() {
        document.getElementById('<%=lblexer.ClientID %>').innerHTML = "";
    }
</script>
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <br />
    <center>
        <asp:Label ID="Label2" runat="server" Text="Master  Time Table" Font-Bold="True"
            Font-Names="Book Antiqua" Font-Size="Large" ForeColor="Green"></asp:Label>
    </center>
    <br />
    <center>
        <table class="maintablestyle" style="width: 700px; height: 40px; background-color: #0CA6CA;">
            <tr>
                <td>
                    <asp:Label ID="Lblclg" runat="server" Text="College" Font-Bold="True" Font-Size="Medium"
                        Font-Names="Book Antiqua"></asp:Label>
                </td>
                <td>
                    <asp:DropDownList ID="ddlclg" runat="server" AutoPostBack="True" Font-Bold="True"
                        Font-Names="Book Antiqua" Font-Size="Medium" Width="200px" CausesValidation="True">
                    </asp:DropDownList>
                </td>
                <td>
                    <asp:Label ID="lblBatch" runat="server" Text="Batch" Font-Bold="True" Font-Size="Medium"
                        Font-Names="Book Antiqua"></asp:Label>
                </td>
                <td>
                    <asp:DropDownList ID="ddlBatch" runat="server" AutoPostBack="True" Font-Bold="True"
                        Font-Names="Book Antiqua" Font-Size="Medium" Width="80px" CausesValidation="True">
                    </asp:DropDownList>
                </td>
                <td>
                    <asp:Label ID="lblDegree" runat="server" Text="Degree " Font-Bold="True" Font-Size="Medium"
                        Font-Names="Book Antiqua">
                    </asp:Label>
                </td>
                <td>
                    <asp:DropDownList ID="ddlDegree" runat="server" AutoPostBack="True" Height="21px"
                        Font-Bold="True" Font-Names="Book Antiqua" Width="100px" Font-Size="Medium" CausesValidation="True"
                        OnSelectedIndexChanged="ddlDegree_SelectedIndexChanged">
                    </asp:DropDownList>
                </td>
                <td>
                    <asp:Label ID="lblBranch" runat="server" Text="Branch " Font-Bold="True" Font-Size="Medium"
                        Font-Names="Book Antiqua"></asp:Label>
                </td>
                <td>
                    <asp:DropDownList ID="ddlBranch" runat="server" AutoPostBack="True" Font-Bold="True"
                        Font-Names="Book Antiqua" Font-Size="Medium" CausesValidation="True" Width="245px">
                    </asp:DropDownList>
                </td>
                <td>
                    <asp:Button ID="btnGo" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                        Font-Size="Medium" Text="Go" OnClick="btnGo_Click" />
                </td>
            </tr>
        </table>
    </center>
    <br />
    <center>
        <asp:Label ID="lblexer" runat="server" Font-Size="Medium" Font-Bold="true" ForeColor="Red"></asp:Label>
    </center>
    <br />
    <center>
        <FarPoint:FpSpread ID="Fptimetable" runat="server" ActiveSheetViewIndex="0" ShowHeaderSelection="false">
            <Sheets>
                <FarPoint:SheetView SheetName="Sheet1" AllowPage="false" AutoPostBack="true">
                </FarPoint:SheetView>
            </Sheets>
            <TitleInfo BackColor="#E7EFF7" ForeColor="" HorizontalAlign="Center" VerticalAlign="NotSet"
                Font-Size="X-Large">
            </TitleInfo>
            <CommandBar Visible="false">
            </CommandBar>
        </FarPoint:FpSpread>
    </center>
    <br />
    <br />
    <center>
        <asp:Label ID="Lblreport" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
            Font-Size="Medium" Text="Report Name"></asp:Label>
        <asp:TextBox ID="txtexcl" runat="server" onkeypress="display()" Height="20px" Width="180px" Style="font-family: 'Book Antiqua'"
            Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium"></asp:TextBox>
        <asp:Button ID="btnxcl" runat="server" Text="Export Excel" Font-Bold="True" Font-Names="Book Antiqua"
            OnClick="btnxcl_click" Font-Size="Medium" />
        <asp:Button ID="btnprnt" runat="server" Text="Print" Font-Names="Book Antiqua" Font-Size="Medium"
            Font-Bold="true" OnClick="btnprnt_Click" />
        <Insproplus:printmaster runat="server" ID="Printmaster1" Visible="false" />
        <asp:Label ID="lblerr" runat="server" Font-Size="Medium" Font-Bold="true" ForeColor="Red"></asp:Label>
    </center>
    <br />
    <br />
    <center>
        <FarPoint:FpSpread ID="Fpstaff" runat="server" ShowHeaderSelection="false">
            <Sheets>
                <FarPoint:SheetView SheetName="sheet2" AllowPage="false" AutoPostBack="true">
                </FarPoint:SheetView>
            </Sheets>
            <CommandBar Visible="false">
            </CommandBar>
        </FarPoint:FpSpread>
    </center>
    <br />
    <br />
    <center>
        <asp:Label ID="lblrptname" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
            Font-Size="Medium" Text="Report Name"></asp:Label>
        <asp:TextBox ID="txtexcelname" runat="server" onkeypress="display1()" Height="20px" Width="180px" Style="font-family: 'Book Antiqua'"
            Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium"></asp:TextBox>
        <asp:Button ID="btnxl" runat="server" Text="Export Excel" Font-Bold="True" Font-Names="Book Antiqua"
            Font-Size="Medium" OnClick="btnxl_Click" />
        <asp:Button ID="btnprintmaster" runat="server" Text="Print" Font-Names="Book Antiqua"
            Font-Size="Medium" Font-Bold="true" OnClick="btnprintmaster_Click" />
        <Insproplus:printmaster runat="server" ID="Printcontrol" Visible="false" />
    </center>
    </div>
</asp:Content>

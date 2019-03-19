<%@ Page Title="" Language="C#" MasterPageFile="~/AttendanceMOD/AttendanceSubSiteMaster.master"
    AutoEventWireup="true" CodeFile="AttendanceReason.aspx.cs" Inherits="AttendanceReason" %>

<%@ Register Assembly="FarPoint.Web.Spread" Namespace="FarPoint.Web.Spread" TagPrefix="FarPoint" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Src="~/Usercontrols/PrintMaster.ascx" TagName="PRINTPDF" TagPrefix="Insproplus" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">
    <!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN">
    <html>
    <body>
        <script type="text/javascript">
            function display() {
                document.getElementById('MainContent_errmsg').innerHTML = "";
            }
        </script>
        <br />
        <center>
            <span class="fontstyleheader" style="color: Green;">AT22-Attendance With Reason</span>
        </center>
        <br />
        <div style="width: 1072px">
            <asp:ScriptManager ID="ScriptManager1" runat="server">
            </asp:ScriptManager>
            <table class="maintablestyle" style="border-bottom-style: solid; border-top-style: solid; border-left-style: solid;
                background-color: #0CA6CA; border-width: 1px; text-align: left">
                <tr>
                    <td>
                        <asp:Label ID="Label1" runat="server" Text="Batch" Font-Bold="True" Font-Names="Book Antiqua"
                            Font-Size="Medium"></asp:Label>
                    </td>
                    <td>
                        <asp:DropDownList ID="ddlbatch" runat="server" OnSelectedIndexChanged="ddlbatch_SelectedIndexChanged"
                            AutoPostBack="True" Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium"
                            Height="25px" Width="69px">
                        </asp:DropDownList>
                    </td>
                    <td>
                        <asp:Label ID="Label2" runat="server" Text="Degree" Font-Bold="True" Font-Names="Book Antiqua"
                            Font-Size="Medium"></asp:Label>
                    </td>
                    <td>
                        <asp:DropDownList ID="ddldegree" runat="server" OnSelectedIndexChanged="ddldegree_SelectedIndexChanged"
                            AutoPostBack="True" Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium"
                            Height="25px">
                        </asp:DropDownList>
                    </td>
                    <td>
                        <asp:Label ID="lblbranch" runat="server" Text="Branch" Font-Bold="True" Font-Names="Book Antiqua"
                            Font-Size="Medium"></asp:Label>
                    </td>
                    <td>
                        <asp:DropDownList ID="ddlbranch" runat="server" OnSelectedIndexChanged="ddlbranch_SelectedIndexChanged"
                            AutoPostBack="True" Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium"
                            Height="25px" Width="250px">
                        </asp:DropDownList>
                    </td>
                    <td>
                        <asp:Label ID="lblsem" runat="server" Text="Sem" Font-Bold="True" Font-Names="Book Antiqua"
                            Font-Size="Medium"></asp:Label>
                    </td>
                    <td>
                        <asp:DropDownList ID="ddlsemester" runat="server" OnSelectedIndexChanged="ddlsemester_SelectedIndexChanged"
                            AutoPostBack="True" Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium"
                            Height="25px" Width="41px">
                        </asp:DropDownList>
                    </td>
                    <td>
                        <asp:Label ID="lblsec" runat="server" Text="Sec" Font-Bold="True" Font-Names="Book Antiqua"
                            Font-Size="Medium"></asp:Label>
                    </td>
                    <td>
                        <asp:DropDownList ID="ddlsection" runat="server" AutoPostBack="True" Font-Bold="True"
                            Font-Names="Book Antiqua" Font-Size="Medium" Height="25px" Width="45px" OnSelectedIndexChanged="ddlsection_SelectedIndexChanged">
                        </asp:DropDownList>
                    </td>
                    <td>
                        <asp:Label ID="Label6" runat="server" Text="From" Font-Bold="True" Font-Names="Book Antiqua"
                            Font-Size="Medium"></asp:Label>
                    </td>
                    <td>
                        <asp:TextBox ID="txtfromdate" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                            Font-Size="Medium" Height="24px" Width="80px" OnTextChanged="txtfromdate_TextChanged"
                            AutoPostBack="True"></asp:TextBox>
                        <asp:CalendarExtender ID="CalendarExtender1" Format="d/MM/yyyy" TargetControlID="txtfromdate"
                            runat="server">
                        </asp:CalendarExtender>
                    </td>
                    <td>
                        <asp:Label ID="Label3" runat="server" Text="To" Font-Bold="True" Font-Names="Book Antiqua"
                            Font-Size="Medium"></asp:Label>
                    </td>
                    <td>
                        <asp:TextBox ID="txttodate" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                            Font-Size="Medium" Height="24px" Width="80px" OnTextChanged="txttodate_TextChanged"
                            AutoPostBack="True"></asp:TextBox>
                        <asp:CalendarExtender ID="CalendarExtender2" Format="d/MM/yyyy" TargetControlID="txttodate"
                            runat="server">
                        </asp:CalendarExtender>
                    </td>
                </tr>
                <tr>
                    <td colspan="6">
                        <asp:Label ID="lblreason" runat="server" Text="Reason" Font-Bold="True" Font-Names="Book Antiqua"
                            Font-Size="Medium"></asp:Label>
                        <asp:DropDownList ID="ddlreason" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                            Font-Size="Medium" Height="25px" Width="150px">
                        </asp:DropDownList>
                        <asp:RadioButton ID="rbhour" Text="Period Wise" runat="server" GroupName="hour" Font-Bold="True"
                            Font-Names="Book Antiqua" Font-Size="Medium" />
                        <asp:RadioButton ID="rbday" Text="Day Wise" runat="server" GroupName="hour" Font-Bold="True"
                            Font-Names="Book Antiqua" Font-Size="Medium" />
                        <asp:Button ID="btngo" runat="server" Text="Go" OnClick="btngo_Click" Font-Bold="True"
                            Font-Names="Book Antiqua" Font-Size="Medium" />
                    </td>
                </tr>
            </table>
            <br />
            <center>
                <asp:Label ID="errmsg" runat="server" Text="" ForeColor="Red" Font-Bold="True" Font-Names="Book Antiqua"
                    Font-Size="Medium"></asp:Label>
            </center>
            <br />
            <FarPoint:FpSpread ID="Fpreport" runat="server" Height="250px" Width="400px" ActiveSheetViewIndex="0"
                currentPageIndex="0" DesignString="&lt;?xml version=&quot;1.0&quot; encoding=&quot;utf-8&quot;?&gt;&lt;Spread /&gt;"
                EnableClientScript="False" CssClass="cursorptr" BorderColor="Black" BorderWidth="0.5"
                ShowHeaderSelection="false">
                <CommandBar BackColor="Control" ButtonType="PushButton">
                    <Background BackgroundImageUrl="SPREADCLIENTPATH:/img/cbbg.gif" />
                </CommandBar>
                <Pager Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                    Font-Underline="False" />
                <HierBar Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                    Font-Underline="False" />
                <Sheets>
                    <FarPoint:SheetView SheetName="Sheet1" EditTemplateColumnCount="2" GridLineColor="Black"
                        GroupBarText="Drag a column to group by that column." SelectionBackColor="#CE5D5A"
                        SelectionForeColor="White">
                    </FarPoint:SheetView>
                </Sheets>
                <TitleInfo BackColor="#E7EFF7" Font-Size="X-Large" ForeColor="" HorizontalAlign="Center"
                    VerticalAlign="NotSet" Font-Bold="False" Font-Italic="False" Font-Overline="False"
                    Font-Strikeout="False" Font-Underline="False">
                </TitleInfo>
            </FarPoint:FpSpread>
            <br />
            <center>
                <asp:Label ID="lblrptname" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                    Font-Size="Medium" Text="Report Name"></asp:Label>
                <asp:TextBox ID="txtexcelname" runat="server" Height="20px" Width="180px" Style="font-family: 'Book Antiqua'"
                    Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium" onkeypress="display()"></asp:TextBox>
                <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender2" runat="server" TargetControlID="txtexcelname"
                    FilterType="LowercaseLetters,UppercaseLetters,Numbers,Custom" ValidChars="!@$%^&*()_+|\}{][':;?><,./">
                </asp:FilteredTextBoxExtender>
                <asp:Button ID="btnxl" runat="server" Text="Export Excel" Font-Bold="True" Font-Names="Book Antiqua"
                    Font-Size="Medium" OnClick="btnxl_Click" />
                <asp:Button ID="btnprintmaster" runat="server" Text="Print" OnClick="btnprintmaster_Click"
                    Font-Names="Book Antiqua" Font-Size="Medium" Font-Bold="true" />
                <Insproplus:PRINTPDF runat="server" ID="Printcontrol" Visible="false" />
            </center>
    </body>
    </html>
</asp:Content>

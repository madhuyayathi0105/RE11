<%@ Page Title="" Language="C#" MasterPageFile="~/AttendanceMOD/AttendanceSubSiteMaster.master"
    AutoEventWireup="true" CodeFile="DailyEntry LessonPlanner Report.aspx.cs" Inherits="DailyEntry_LessonPlanner_Report" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Assembly="FarPoint.Web.Spread" Namespace="FarPoint.Web.Spread" TagPrefix="FarPoint" %>
<%@ Register Src="~/Usercontrols/PrintMaster.ascx" TagName="printmaster" TagPrefix="Insproplus" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">
    <body>
        <script type="text/javascript">
            function display() {
                document.getElementById('MainContent_lblnorec').innerHTML = "";
            }
        </script>
        <asp:ScriptManager ID="ScriptManager1" runat="server">
        </asp:ScriptManager>
        <br />
        <center>
            <span class="fontstyleheader" style="color: Green;">AT20-Daily Entry and Lesson Planner
                Report</span>
        </center>
        <br />
        <center>
            <table class="maintablestyle">
                <tr>
                    <td>
                        <asp:Label ID="lblbatch" runat="server" Text="Batch" Width="60px" Font-Bold="true"
                            ForeColor="Black" Font-Size="Medium" Font-Names="Book Antiqua"></asp:Label>
                    </td>
                    <td>
                        <asp:DropDownList ID="ddlbatch" runat="server" Width="70px" Font-Bold="true" ForeColor="Black"
                            Font-Size="Medium" Font-Names="Book Antiqua" AutoPostBack="true" OnSelectedIndexChanged="ddlbatch_SelectedIndexChanged">
                        </asp:DropDownList>
                    </td>
                    <td>
                        <asp:Label ID="lbldegree" Width="100px" runat="server" Text="Degree" Font-Bold="true"
                            ForeColor="Black" Font-Size="Medium" Font-Names="Book Antiqua"></asp:Label>
                    </td>
                    <td style="width: 70px;">
                        <asp:DropDownList ID="ddldegree" Width="60px" runat="server" Font-Bold="true" ForeColor="Black"
                            Font-Size="Medium" Font-Names="Book Antiqua" AutoPostBack="true" OnSelectedIndexChanged="ddldegree_SelectedIndexChanged">
                        </asp:DropDownList>
                    </td>
                    <td>
                        <asp:Label ID="lblbranch" Width="60px" runat="server" Text="Branch" Font-Bold="true"
                            ForeColor="Black" Font-Size="Medium" Font-Names="Book Antiqua"></asp:Label>
                    </td>
                    <td>
                        <asp:DropDownList ID="ddlbranch" runat="server" Width="150px" Style="margin-left: 13px;"
                            Font-Bold="true" ForeColor="Black" Font-Size="Medium" Font-Names="Book Antiqua"
                            AutoPostBack="true" OnSelectedIndexChanged="ddlbranch_SelectedIndexChanged">
                        </asp:DropDownList>
                    </td>
                    <td>
                        <asp:Label ID="lblsemester" Width="50px" runat="server" Text="Sem" Font-Bold="True"
                            ForeColor="Black" Font-Size="Medium" Font-Names="Book Antiqua"></asp:Label>
                    </td>
                    <td>
                        <asp:DropDownList ID="ddlsemseter" runat="server" Font-Bold="true" ForeColor="Black"
                            Font-Size="Medium" Font-Names="Book Antiqua" AutoPostBack="true" OnSelectedIndexChanged="ddlsemseter_SelectedIndexChanged">
                        </asp:DropDownList>
                    </td>
                    <td>
                        <asp:Label ID="lblsection" Width="40px" runat="server" Text="Sec" Font-Bold="true"
                            ForeColor="Black" Font-Size="Medium" Font-Names="Book Antiqua"></asp:Label>
                    </td>
                    <td>
                        <asp:DropDownList ID="ddlsection" runat="server" Width="50px" Font-Bold="true" ForeColor="Black"
                            Font-Size="Medium" Font-Names="Book Antiqua" AutoPostBack="true" OnSelectedIndexChanged="ddlsection_SelectedIndexChanged">
                        </asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <td class="style1">
                        <asp:Label ID="lblstaffregister" runat="server" Text="Lesson" Width="60px" Font-Bold="true"
                            ForeColor="Black" Font-Size="Medium" Font-Names="Book Antiqua"></asp:Label>
                    </td>
                    <td>
                        <asp:DropDownList ID="ddldaily" runat="server" Width="70px" Font-Bold="true" ForeColor="Black"
                            Font-Size="Medium" Font-Names="Book Antiqua">
                            <asp:ListItem Value="0">Differance</asp:ListItem>
                            <asp:ListItem Value="1">Planner</asp:ListItem>
                            <asp:ListItem Value="2">Entry</asp:ListItem>
                        </asp:DropDownList>
                    </td>
                    <td>
                        <asp:Label ID="lblsubject" runat="server" Width="60px" Text="Subject" Font-Bold="true"
                            ForeColor="Black" Font-Size="Medium" Font-Names="Book Antiqua"></asp:Label>
                    </td>
                    <td>
                        <asp:DropDownList ID="ddlsubject" AutoPostBack="true" Width="113px" runat="server"
                            Font-Bold="true" ForeColor="Black" Font-Size="Medium" Font-Names="Book Antiqua"
                            OnSelectedIndexChanged="ddlsubject_SelectedIndexChanged">
                        </asp:DropDownList>
                    </td>
                    <td>
                        <asp:Label ID="lblstaff" runat="server" Width="60px" Text="Staff" Font-Bold="true"
                            ForeColor="Black" Font-Size="Medium" Font-Names="Book Antiqua"></asp:Label>
                    </td>
                    <td>
                        <asp:DropDownList ID="ddlstaff" Width="100px" Style="margin-left: 13px;" runat="server"
                            Font-Bold="true" ForeColor="Black" Font-Size="Medium" Font-Names="Book Antiqua">
                        </asp:DropDownList>
                    </td>
                    <td>
                        <asp:Label ID="lblfrom" runat="server" Text="From Date" Width="80px" Font-Bold="true"
                            ForeColor="Black" Font-Size="Medium" Font-Names="Book Antiqua"></asp:Label>
                    </td>
                    <td>
                        <asp:TextBox ID="txtfrom" runat="server" Font-Bold="true" AutoPostBack="true" Width="80px"
                            ForeColor="Black" Font-Size="Medium" Font-Names="Book Antiqua" OnTextChanged="txtfrom_TextChanged"></asp:TextBox>
                        <asp:CalendarExtender ID="CalendarExtender1" TargetControlID="txtfrom" runat="server"
                            Format="dd/MM/yyyy">
                        </asp:CalendarExtender>
                        <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender2" runat="server" TargetControlID="txtfrom"
                            FilterType="Numbers,Custom" ValidChars="/">
                        </asp:FilteredTextBoxExtender>
                    </td>
                    <td>
                        <asp:Label ID="lblto" runat="server" Text="To Date" Font-Bold="true" Width="80px"
                            ForeColor="Black" Font-Size="Medium" Font-Names="Book Antiqua"></asp:Label>
                    </td>
                    <td>
                        <asp:TextBox ID="txtto" runat="server" Font-Bold="true" AutoPostBack="true" Width="80px"
                            ForeColor="Black" Font-Size="Medium" Font-Names="Book Antiqua" OnTextChanged="txtto_TextChanged"></asp:TextBox>
                        <asp:CalendarExtender ID="CalendarExtender2" TargetControlID="txtto" runat="server"
                            Format="dd/MM/yyyy">
                        </asp:CalendarExtender>
                        <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender3" runat="server" TargetControlID="txtto"
                            FilterType="Numbers,Custom" ValidChars="/">
                        </asp:FilteredTextBoxExtender>
                    </td>
                    <td>
                        <asp:Button ID="btngo" runat="server" Text="Go" Font-Bold="true" ForeColor="Black"
                            Font-Size="Medium" Font-Names="Book Antiqua" OnClick="btngo_Click" />
                    </td>
                </tr>
            </table>
            <br />
            <asp:Label ID="errmsg" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                Font-Size="Medium" ForeColor="Red" Width="676px"></asp:Label>
            <asp:Label ID="lblnorec" runat="server" Text="No Records Found" ForeColor="Red" Font-Bold="True"
                Font-Names="Book Antiqua" Font-Size="Medium"></asp:Label>
            <br />
            <FarPoint:FpSpread ID="Fpdailyplanner" runat="server" ActiveSheetViewIndex="0" currentPageIndex="0"
                DesignString="&lt;?xml version=&quot;1.0&quot; encoding=&quot;utf-8&quot;?&gt;&lt;Spread /&gt;"
                EnableClientScript="False" CssClass="cursorptr" BorderColor="Black" BorderWidth="0.5">
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
            <asp:Label ID="lblrptname" runat="server" Width="120px" Font-Bold="True" Font-Names="Book Antiqua"
                Font-Size="Medium" Text="Report Name"></asp:Label>
            <asp:TextBox ID="txtexcelname" runat="server" Height="20px" Width="180px" Style="font-family: 'Book Antiqua'"
                Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium" onkeypress="display()"></asp:TextBox>
            <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" runat="server" TargetControlID="txtexcelname"
                FilterType="LowercaseLetters,UppercaseLetters,Numbers,Custom" ValidChars="!@$%^&*()_+|\}{][':;?><,./">
            </asp:FilteredTextBoxExtender>
            <asp:Button ID="btnxl" runat="server" Text="Export Excel" Font-Bold="True" Font-Names="Book Antiqua"
                Font-Size="Medium" OnClick="btnxl_Click" />
            <asp:Button ID="btnprintmaster" runat="server" Text="Print" OnClick="btnprintmaster_Click" Font-Names="Book Antiqua" Font-Size="Medium"
                Font-Bold="true" />
            <Insproplus:printmaster ID="Printcontrol" runat="server" Visible="false" />
        </center>
    </body>
</asp:Content>

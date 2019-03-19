<%@ Page Title="" Language="C#" MasterPageFile="~/AttendanceMOD/AttendanceSubSiteMaster.master"
    AutoEventWireup="true" CodeFile="attreport_perday.aspx.cs" Inherits="Ovrall_Attreport_perday" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Assembly="FarPoint.Web.Spread" Namespace="FarPoint.Web.Spread" TagPrefix="FarPoint" %>
<%@ Register Src="~/Usercontrols/PrintMaster.ascx" TagName="printmaster" TagPrefix="Insproplus" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">
    <html>
    <body>
        <script type="text/javascript">
            function display() {
                document.getElementById('MainContent_errlbl').innerHTML = "";
            }
        </script>
        <asp:ScriptManager ID="ScriptManager1" runat="server">
        </asp:ScriptManager>
        <br />
        <center>
            <span class="fontstyleheader" style="color: Green;">AT 33 - Attendance Report PerDay</span>
        </center>
        <br />
        <center>
            <table class="maintablestyle">
                <tr>
                    <td>
                        <asp:Label ID="Label4" runat="server" Text="College" Font-Bold="True" Font-Names="Book Antiqua"
                            Font-Size="Medium"></asp:Label>
                    </td>
                    <td>
                        <asp:DropDownList ID="ddlcollege" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                            Font-Size="Medium" Width="400px" OnSelectedIndexChanged="ddlcollege_SelectedIndexChanged"
                            AutoPostBack="True">
                        </asp:DropDownList>
                    </td>
                    <td class="style1">
                        <asp:Label ID="lblbach" runat="server" Text="Batch" Font-Bold="True" ForeColor="Black"
                            Font-Size="Medium" Font-Names="Book Antiqua"></asp:Label>
                    </td>
                    <td class="style2">
                        <div style="position: relative;">
                            <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                                <ContentTemplate>
                                    <asp:TextBox ID="txtbatch" runat="server" Height="20px" CssClass="Dropdown_Txt_Box"
                                        ReadOnly="true" Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium">---Select---</asp:TextBox>
                                    <asp:Panel ID="pbatch" runat="server" CssClass="multxtpanel" Width="125px" BackColor="White"
                                        ScrollBars="Auto" Style="font-family: 'Book Antiqua'">
                                        <asp:CheckBox ID="chkbatch" runat="server" Width="100px" Font-Bold="True" OnCheckedChanged="chkbatch_ChekedChange"
                                            Font-Names="Book Antiqua" Font-Size="Medium" ForeColor="Black" Text="Select All"
                                            AutoPostBack="True" />
                                        <asp:CheckBoxList ID="chklsbatch" runat="server" Font-Size="Medium" AutoPostBack="True"
                                            Width="100px" Height="58px" Font-Bold="True" ForeColor="Black" Font-Names="Book Antiqua"
                                            OnSelectedIndexChanged="chklstbatch_SelectedIndexChanged">
                                        </asp:CheckBoxList>
                                    </asp:Panel>
                                    <asp:PopupControlExtender ID="PopupControlExtender1" runat="server" TargetControlID="txtbatch"
                                        PopupControlID="pbatch" Position="Bottom">
                                    </asp:PopupControlExtender>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </div>
                    </td>
                    <td class="style1">
                        <asp:Label ID="lbldegree" runat="server" Text="Degree" Font-Bold="True" ForeColor="Black"
                            Font-Names="Book Antiqua" Font-Size="Medium"></asp:Label>
                    </td>
                    <td class="style2">
                        <div style="position: relative;">
                            <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                                <ContentTemplate>
                                    <asp:TextBox ID="txtdegree" runat="server" Height="20px" ReadOnly="true" CssClass="Dropdown_Txt_Box"
                                        Width="120px" Style="font-family: 'Book Antiqua';" Font-Bold="True" Font-Names="Book Antiqua"
                                        Font-Size="Medium">---Select---</asp:TextBox>
                                    <asp:Panel ID="pdegree" runat="server" CssClass="multxtpanel" Width="125px" Height="200px"
                                        BackColor="White" ScrollBars="Auto" Style="font-family: 'Book Antiqua'">
                                        <asp:CheckBox ID="chkdegree" runat="server" Width="100px" ForeColor="Black" Font-Bold="True"
                                            Font-Names="Book Antiqua" Font-Size="Medium" Text="Select All" AutoPostBack="True"
                                            OnCheckedChanged="chkdegree_CheckedChanged" />
                                        <asp:CheckBoxList ID="chklstdegree" runat="server" ForeColor="Black" Font-Size="Medium"
                                            AutoPostBack="True" Width="100px" Height="58px" Font-Bold="True" Font-Names="Book Antiqua"
                                            OnSelectedIndexChanged="chklstdegree_SelectedIndexChanged">
                                        </asp:CheckBoxList>
                                    </asp:Panel>
                                    <asp:PopupControlExtender ID="PopupControlExtender2" runat="server" TargetControlID="txtdegree"
                                        PopupControlID="pdegree" Position="Bottom">
                                    </asp:PopupControlExtender>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </div>
                    </td>
                </tr>
                <tr>
                    <td class="style1">
                        <asp:Label ID="lblbranch" runat="server" Text="Department" Font-Bold="True" ForeColor="Black"
                            Font-Names="Book Antiqua" Font-Size="Medium"></asp:Label>
                    </td>
                    <td class="style2">
                        <div style="position: relative;">
                            <asp:UpdatePanel ID="UpdatePanel3" runat="server">
                                <ContentTemplate>
                                    <asp:TextBox ID="txtbranch" runat="server" Height="20px" CssClass="Dropdown_Txt_Box"
                                        ReadOnly="true" Width="120px" Style="font-family: 'Book Antiqua';" Font-Bold="True"
                                        Font-Names="Book Antiqua" Font-Size="Medium">---Select---</asp:TextBox>
                                    <asp:Panel ID="pbranch" runat="server" CssClass="multxtpanel" Height="250px" BackColor="White"
                                        ScrollBars="Auto" Style="font-family: 'Book Antiqua'">
                                        <asp:CheckBox ID="chkbranch" runat="server" ForeColor="Black" Font-Bold="True" Font-Names="Book Antiqua"
                                            Font-Size="Medium" Text="Select All" AutoPostBack="True" OnCheckedChanged="chkbranch_CheckedChanged" />
                                        <asp:CheckBoxList ID="chklstbranch" runat="server" ForeColor="Black" Font-Size="Medium"
                                            AutoPostBack="True" Width="350px" Style="font-family: 'Book Antiqua'" Font-Bold="True"
                                            Font-Names="Book Antiqua" Height="58px" OnSelectedIndexChanged="chklstbranch_SelectedIndexChanged">
                                        </asp:CheckBoxList>
                                    </asp:Panel>
                                    <asp:PopupControlExtender ID="PopupControlExtender3" runat="server" TargetControlID="txtbranch"
                                        PopupControlID="pbranch" Position="Bottom">
                                    </asp:PopupControlExtender>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </div>
                    </td>
                    <td>
                        <asp:Label ID="datelbl" runat="server" Text="Date" Font-Bold="True" Font-Names="Book Antiqua"
                            Font-Size="Medium"></asp:Label>
                    </td>
                    <td>
                        <asp:TextBox ID="txtFromDate" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                            Font-Size="Medium" OnTextChanged="txtFromDate_TextChanged" Width="80px" AutoPostBack="True"> </asp:TextBox>
                        <asp:CalendarExtender ID="CalendarExtender1" TargetControlID="txtFromDate" Format="d/MM/yyyy"
                            runat="server">
                        </asp:CalendarExtender>
                        <asp:FilteredTextBoxExtender ID="txtFromDate_FilteredTextBoxExtender" FilterType="Custom,Numbers"
                            ValidChars="/" runat="server" TargetControlID="txtFromDate">
                        </asp:FilteredTextBoxExtender>
                    </td>
                    <td>
                        <asp:CheckBox ID="chkPeriod" runat="server" Text="Period" Font-Bold="True" Font-Names="Book Antiqua"
                            Font-Size="Medium" AutoPostBack="true" OnCheckedChanged="chkPeriod_CheckedChange"
                            Width="100px" />
                    </td>
                    <td>
                        <asp:DropDownList ID="ddlperiod" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                            Font-Size="Medium" Width="50px">
                        </asp:DropDownList>
                    </td>
                    <td>
                        <asp:Button ID="btnGo" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                            Font-Size="Medium" Text="Go" OnClick="btnGo_Click" />
                    </td>
                    <td>
                        <asp:Label ID="lblFromDate" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                            Font-Size="Medium" ForeColor="Red"></asp:Label>
                    </td>
                    <td>
                        <asp:Label ID="lblpages" runat="server" Text="Page" Font-Names="Book Antiqua" Font-Size="Medium"
                            Font-Bold="true"></asp:Label>
                    </td>
                    <td>
                        <asp:DropDownList ID="ddlpage" runat="server" OnSelectedIndexChanged="ddlpage_SelectedIndexChanged"
                            Font-Names="Book Antiqua" Font-Size="Medium" Font-Bold="true" AutoPostBack="True"
                            Width="48px">
                        </asp:DropDownList>
                    </td>
                </tr>
            </table>
            <table>
                <tr>
                    <td>
                        <br />
                        <asp:Label ID="errlbl" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                            Font-Size="Medium" ForeColor="Red"></asp:Label>
                        <br />
                        <asp:Panel ID="pageset_pnl" runat="server" BorderStyle="None" Width="1026px">
                            <asp:Label ID="Buttontotal" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                Font-Size="Medium"></asp:Label>
                            &nbsp;&nbsp;
                            <asp:Label ID="ddlpagelbl" runat="server" Font-Bold="True" Text="     Records Per Page"
                                Font-Names="Book Antiqua" Font-Size="Medium"></asp:Label>
                            <asp:DropDownList ID="DropDownListpage" runat="server" Height="25px" Width="65px"
                                Font-Bold="True" Font-Size="Medium" Font-Names="Book Antiqua" OnSelectedIndexChanged="DropDownListpage_SelectedIndexChanged"
                                AutoPostBack="True">
                            </asp:DropDownList>
                            <asp:TextBox ID="pageddltxt" runat="server" Height="22px" Width="40px" Font-Bold="True"
                                Font-Size="Medium" Font-Names="Book Antiqua" OnTextChanged="pageddltxt_TextChanged"
                                AutoPostBack="True"></asp:TextBox>
                            <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender3" runat="server" FilterType="Numbers"
                                TargetControlID="pageddltxt">
                            </asp:FilteredTextBoxExtender>
                            <asp:Label ID="pgsearch_lbl" runat="server" Font-Bold="True" Text="Page Search" Width="95px"
                                Font-Names="Book Antiqua" Font-Size="Medium" Height="24px"></asp:Label>
                            <asp:TextBox ID="pagesearch_txt" runat="server" AutoPostBack="True" Font-Bold="True"
                                Font-Names="Book Antiqua" Font-Size="Medium" Height="22px" Width="40px" OnTextChanged="pagesearch_txt_TextChanged"></asp:TextBox>
                            <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender2" runat="server" TargetControlID="pagesearch_txt"
                                FilterType="Numbers">
                            </asp:FilteredTextBoxExtender>
                            <asp:Label ID="errmsg" runat="server" ForeColor="Red" Font-Bold="True" Font-Names="Book Antiqua"
                                Font-Size="Medium" Height="19px" Width="301px"></asp:Label>
                        </asp:Panel>
                    </td>
                </tr>
                <tr>
                    <td>
                    </td>
                </tr>
                <tr>
                    <td>
                        <center>
                            <FarPoint:FpSpread ID="attnd_report" runat="server" Height="250px" Width="400px"
                                ActiveSheetViewIndex="0" DesignString="&lt;?xml version=&quot;1.0&quot; encoding=&quot;utf-8&quot;?&gt;&lt;Spread /&gt;"
                                EnableClientScript="False" CssClass="cursor" BorderWidth="0.5" ShowHeaderSelection="false">
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
                        </center>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="lblrptname" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                            Font-Size="Medium" Text="Report Name"></asp:Label>
                        <asp:TextBox ID="txtexcelname" runat="server" Height="20px" Width="180px" Style="font-family: 'Book Antiqua'"
                            Font-Bold="True" onkeypress="display()" Font-Names="Book Antiqua" Font-Size="Medium"></asp:TextBox>
                        <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" runat="server" TargetControlID="txtexcelname"
                            FilterType="LowercaseLetters,UppercaseLetters,Numbers,Custom" ValidChars="_-">
                        </asp:FilteredTextBoxExtender>
                        <asp:Button ID="btnxl" runat="server" Text="Export Excel" Font-Bold="True" Font-Names="Book Antiqua"
                            Font-Size="Medium" OnClick="btnxl_Click" /><asp:Button ID="btnprintmaster" runat="server"
                                Text="Print" OnClick="btnprintmaster_Click" Font-Names="Book Antiqua" Font-Size="Medium"
                                Font-Bold="true" />
                        <Insproplus:printmaster runat="server" ID="Printcontrol" Visible="false" />
                    </td>
                </tr>
            </table>
        </center>
    </body>
    </html>
</asp:Content>

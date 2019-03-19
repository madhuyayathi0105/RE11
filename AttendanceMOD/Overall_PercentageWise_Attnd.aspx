<%@ Page Title="" Language="C#" MasterPageFile="~/AttendanceMOD/AttendanceSubSiteMaster.master"
    AutoEventWireup="true" CodeFile="Overall_PercentageWise_Attnd.aspx.cs" Inherits="Overall_PercentageWise_Attnd" %>

    <%@ Register Src="~/Usercontrols/NewPrintMaster.ascx" TagName="NEWPrintMater" TagPrefix="NEW" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Src="~/Usercontrols/PrintMaster.ascx" TagName="printmaster" TagPrefix="Insproplus" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <style runat="server" id="font_css" type="text/css">
        .css_font
        {
            font-family: Book Antiqua;
            font-size: medium;
            font-weight: bold;
        }
        .pnlborder
        {
            border-style: solid;
            border-width: 0.5;
            border-color: Black;
        }
        #gview
        {
            padding: 0;
            margin: 0;
            border: 1px solid #333;
            font-family: Arial;
        }
    </style>
    <script type="text/javascript">
        function display() {

            document.getElementById('MainContent_lblerr').innerHTML = "";

        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">
    <html>
    <head>
    </head>
    <body>
        <asp:ScriptManager ID="ScriptManager1" runat="server">
        </asp:ScriptManager>
        <br />
        <center>
            <span class="fontstyleheader" style="color: Green;">AT17-Overall Percentagewise Attendance
                Report</span>
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
                            Font-Size="Medium" Width="250px" OnSelectedIndexChanged="ddlcollege_SelectedIndexChanged"
                            AutoPostBack="True">
                        </asp:DropDownList>
                    </td>
                    <td>
                        <asp:Label ID="lblgraduate" runat="server" Text="Graduation Level" Style="" CssClass="css_font"></asp:Label>
                    </td>
                    <td>
                        <asp:UpdatePanel runat="server" ID="upd1">
                            <ContentTemplate>
                                <asp:TextBox ID="txtgraduate" runat="server" CssClass="Dropdown_Txt_Box" ReadOnly="true"
                                    Style="font-family: 'Book Antiqua';" Font-Bold="True" Font-Names="Book Antiqua"
                                    Font-Size="Medium" Width="120px"></asp:TextBox>
                                <asp:Panel ID="pnlgraduate" runat="server" CssClass="multxtpanel" Width="120px"><%--Style="overflow-x: hidden;overflow-y: hidden;"--%>
                                    <asp:CheckBox ID="chkgraduate" runat="server" CssClass="css_font" Text="All" AutoPostBack="True"
                                        OnCheckedChanged="chkgraduate_CheckedChanged" />
                                    <asp:CheckBoxList ID="chkbxlist_graduate" runat="server" CssClass="css_font" AutoPostBack="True"
                                        OnSelectedIndexChanged="chkbxlist_graduate_SelectedIndexChanged">
                                    </asp:CheckBoxList>
                                    <asp:PopupControlExtender ID="PopupControlExtender1" runat="server" TargetControlID="txtgraduate"
                                        PopupControlID="pnlgraduate" Position="Bottom">
                                    </asp:PopupControlExtender>
                                </asp:Panel>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </td>
                    <td>
                        <asp:Label ID="lbldegree" runat="server" Text="Degree" CssClass="css_font" Style=""></asp:Label>
                    </td>
                    <td>
                        <asp:UpdatePanel runat="server" ID="UpdatePanel1">
                            <ContentTemplate>
                                <asp:TextBox ID="txtdegree" runat="server" CssClass="Dropdown_Txt_Box" ReadOnly="true"
                                    Style="font-family: 'Book Antiqua';" Font-Bold="True" Font-Names="Book Antiqua"
                                    Font-Size="Medium" Width="100px"></asp:TextBox>
                                <asp:Panel ID="pnpdegree" runat="server" Height="150px" Width="125px" CssClass="multxtpanel">
                                    <asp:CheckBox ID="chkdegree" runat="server" CssClass="css_font" Text="All" OnCheckedChanged="chkdegree_CheckedChanged"
                                        AutoPostBack="True" />
                                    <asp:CheckBoxList ID="chkbxlistDegree" runat="server" CssClass="css_font" OnSelectedIndexChanged="chkbxlistDegree_SelectedIndexChanged"
                                        AutoPostBack="True">
                                    </asp:CheckBoxList>
                                    <asp:PopupControlExtender ID="PopupControlExtender2" runat="server" TargetControlID="txtdegree"
                                        PopupControlID="pnpdegree" Position="Bottom">
                                    </asp:PopupControlExtender>
                                </asp:Panel>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </td>
                    <td>
                        <asp:Label ID="lblbranch" runat="server" Text="Branch" CssClass="css_font" Style=""></asp:Label>
                    </td>
                    <td>
                        <asp:UpdatePanel runat="server" ID="UpdatePanel5">
                            <ContentTemplate>
                                <asp:TextBox ID="txtbranch" runat="server" CssClass="Dropdown_Txt_Box" ReadOnly="true"
                                    Style="font-family: 'Book Antiqua';" Font-Bold="True" Font-Names="Book Antiqua"
                                    Font-Size="Medium" Width="100px"></asp:TextBox>
                                <asp:Panel ID="pnlbranch" runat="server" Width="250px" CssClass="multxtpanel">
                                    <asp:CheckBox ID="chkbranch" runat="server" CssClass="css_font" Text="All" OnCheckedChanged="chkbranch_CheckedChanged"
                                        AutoPostBack="True" />
                                    <asp:CheckBoxList ID="chkbxlistbranch" runat="server" CssClass="css_font" AutoPostBack="True"
                                        OnSelectedIndexChanged="chkbxlistbranch_SelectedIndexChanged">
                                    </asp:CheckBoxList>
                                    <asp:PopupControlExtender ID="PopupControlExtender3" runat="server" TargetControlID="txtbranch"
                                        PopupControlID="pnlbranch" Position="Bottom">
                                    </asp:PopupControlExtender>
                                </asp:Panel>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </td>
                </tr>
                <tr>
                    <td colspan="7">
                        <asp:Label ID="lblyear" runat="server" Text="Year" CssClass="css_font"></asp:Label>
                        <asp:DropDownList ID="ddlyear" runat="server" CssClass="css_font" OnSelectedIndexChanged="ddlyear_SelectedIndexChanged"
                            AutoPostBack="True">
                        </asp:DropDownList>
                        <asp:Label ID="lblfromdate" runat="server" Text="From Date" CssClass="css_font"></asp:Label>
                        <asp:TextBox ID="txtfromdate" runat="server" CssClass="css_font" Height="24px" Width="89px"></asp:TextBox>
                        <asp:CalendarExtender ID="calext_fromdate" runat="server" TargetControlID="txtfromdate"
                            Format="dd/MM/yyyy">
                        </asp:CalendarExtender>
                        <asp:Label ID="lbltodate" runat="server" Text="To Date" CssClass="css_font"></asp:Label>
                        <asp:TextBox ID="txttodate" runat="server" CssClass="css_font" Height="24px" Width="89px"></asp:TextBox>
                        <asp:CalendarExtender ID="calext_todate" runat="server" TargetControlID="txttodate"
                            Format="dd/MM/yyyy">
                        </asp:CalendarExtender>
                        <asp:Button ID="btngo" runat="server" Text="Go" CssClass="css_font" Height="33px"
                            OnClick="btngo_Click" Width="43px" />
                    </td>
                </tr>
            </table>
        </center>
        <br />
        <center>
            <asp:Label ID="lblerr" runat="server" CssClass="css_font" ForeColor="Red"></asp:Label>
        </center>
        <br />
        <table>
            <tr>
                <td>
                    <asp:Panel ID="pageset_pnl" runat="server" Width="1129px">
                        <asp:Label ID="Buttontotal" runat="server" Font-Bold="True" Font-Size="Medium" Visible="False"
                            Font-Names="Book Antiqua"></asp:Label>
                        <asp:Label ID="lblrecord" runat="server" Font-Bold="True" Text="     Records Per Page"
                            Font-Names="Book Antiqua" Font-Size="Medium"></asp:Label>
                        <asp:DropDownList ID="DropDownListpage" runat="server" AutoPostBack="True" OnSelectedIndexChanged="DropDownListpage_SelectedIndexChanged"
                            Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium" Height="24px" Width="58px">
                        </asp:DropDownList>
                        <asp:TextBox ID="TextBoxother" runat="server" Height="16px" Width="34px" AutoPostBack="True"
                            OnTextChanged="TextBoxother_TextChanged" Font-Bold="True" Font-Names="Book Antiqua"
                            Font-Size="Medium"></asp:TextBox>
                        <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" runat="server" TargetControlID="TextBoxother"
                            FilterType="Numbers" />
                        &nbsp;&nbsp;
                        <asp:Label ID="lblpage" runat="server" Font-Bold="True" Text="Page Search:" Width="97px"
                            Font-Names="Book Antiqua" Font-Size="Medium"></asp:Label>
                        <asp:TextBox ID="TextBoxpage" runat="server" AutoPostBack="True" OnTextChanged="TextBoxpage_TextChanged"
                            Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium" Height="17px" Width="34px"></asp:TextBox>
                        <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender9" runat="server" TargetControlID="TextBoxpage"
                            FilterType="Numbers" />
                        <asp:Label ID="LabelE" runat="server" ForeColor="Red" Font-Bold="True" Font-Names="Book Antiqua"
                            Font-Size="Medium"></asp:Label>&nbsp;
                        <asp:Label ID="lblother" runat="server" ForeColor="Red" Font-Bold="True" Font-Names="Book Antiqua"
                            Font-Size="Medium"></asp:Label>
                    </asp:Panel>
                </td>
            </tr>
        </table>
        <center>
        
        <asp:GridView ID="gview" runat="server" BorderStyle="Double" CssClass="grid-view" AutoGenerateColumns="true"
        Font-Names="Book Antiqua" Font-Size="Medium" GridLines="Both" ShowFooter="false" ShowHeader="false">
            <Columns>
            </Columns>
            <HeaderStyle BackColor="#0CA6CA" Font-Bold="True" ForeColor="Black" Font-Size="Medium" />
            <FooterStyle BackColor="White" ForeColor="#333333" />            
            <PagerStyle BackColor="#336666"  HorizontalAlign="Center" />
            <RowStyle  ForeColor="#333333" />
            <SelectedRowStyle BackColor="#339966" Font-Bold="True" />
        </asp:GridView>
            <br />
            <asp:Label ID="lblrptname" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                Font-Size="Medium" Text="Report Name"></asp:Label>
            <asp:TextBox ID="txtexcelname" runat="server" Height="20px" Width="180px" Style="font-family: 'Book Antiqua'"
                Font-Bold="True" Font-Names="Book Antiqua" onkeypress="display()" Font-Size="Medium"></asp:TextBox>
            <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender2" runat="server" TargetControlID="txtexcelname"
                FilterType="LowercaseLetters,UppercaseLetters,Numbers,Custom" ValidChars="!@$%^&*()_+|}{][':;?><,."
                InvalidChars="/\">
            </asp:FilteredTextBoxExtender>
            <asp:Button ID="btnxl" runat="server" Text="Export Excel" CssClass="css_font" OnClick="btnxl_Click" />
            <asp:Button ID="btnprintmaster" runat="server" Text="Print" OnClick="btnprintmaster_Click"
                Font-Names="Book Antiqua" Font-Size="Medium" Font-Bold="true" />
                 <NEW:NEWPrintMater runat="server" ID="NEWPrintMater1" Visible="false" />
            <Insproplus:printmaster runat="server" ID="Printcontrol" Visible="false" />
        </center>
    </body>
    </html>
</asp:Content>

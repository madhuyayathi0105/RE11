<%@ Page Title="" Language="C#" MasterPageFile="~/AttendanceMOD/AttendanceSubSiteMaster.master"
    AutoEventWireup="true" CodeFile="Ovrall_Attreport_perday.aspx.cs" Inherits="Ovrall_Attreport_perday"
    EnableEventValidation="false" %>

<%@ Register Src="~/Usercontrols/NewPrintMaster.ascx" TagName="NEWPrintMater" TagPrefix="NEW" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Src="~/Usercontrols/PrintMaster.ascx" TagName="printmaster" TagPrefix="Insproplus" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">
    <!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
    <html xmlns="http://www.w3.org/1999/xhtml">
    <title></title>
    <link href="~/Styles/css/Registration.css" rel="stylesheet" type="text/css" />
    <script src="//ajax.googleapis.com/ajax/libs/jquery/1.11.1/jquery.min.js"></script>
    <link href="~/Styles/css/Commoncss.css" rel="Stylesheet" type="text/css" />
    <body oncontextmenu="return false">
        <script type="text/javascript">
            function display() {
                document.getElementById('MainContent_errlbl').innerHTML = "";
            }
        </script>
        <div>
            <asp:ScriptManager ID="ScriptManager1" runat="server">
            </asp:ScriptManager>
            <br />
            <center>
                <span class="fontstyleheader" style="color: Green;">AT04-Over All Attendance Report
                    For Particular Day</span>
            </center>
            <br />
        </div>
        <div>
            <asp:Label ID="lbl_branchT" runat="server" Visible="false"></asp:Label>
            <center>
                <asp:UpdatePanel ID="UP1" runat="server">
                    <ContentTemplate>
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
                                <td>
                                    <asp:Label ID="lblSemester" runat="server" Text="Semester" Font-Bold="True" Font-Names="Book Antiqua"
                                        Font-Size="Medium"></asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtSem" runat="server" Height="20px" ReadOnly="true" Width="180px"
                                        Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium" Style="font-size: medium;
                                        font-weight: bold; height: 20px; width: 100px; font-family: 'Book Antiqua';">---Select---</asp:TextBox>
                                    <asp:Panel ID="pbranch" runat="server" CssClass="multxtpanel" Width="100px">
                                        <asp:CheckBox ID="chkSem" runat="server" Font-Bold="True" Width="180px" Font-Names="Book Antiqua"
                                            Font-Size="Medium" OnCheckedChanged="chksem_CheckedChanged" Text="Select All"
                                            AutoPostBack="True" />
                                        <asp:CheckBoxList ID="chklstsem" runat="server" Font-Size="Medium" AutoPostBack="True"
                                            Width="350px" OnSelectedIndexChanged="chklstsem_SelectedIndexChanged" Style="font-family: 'Book Antiqua'"
                                            Font-Bold="True" Font-Names="Book Antiqua" Height="58px">
                                        </asp:CheckBoxList>
                                    </asp:Panel>
                                    <asp:PopupControlExtender ID="PopupControlExtender3" runat="server" TargetControlID="txtSem"
                                        PopupControlID="pbranch" Position="Bottom">
                                    </asp:PopupControlExtender>
                                </td>
                                <td>
                                    <asp:Label ID="datelbl" runat="server" Text=" From Date" Font-Bold="True" Font-Names="Book Antiqua"
                                        Font-Size="Medium"></asp:Label>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                </td>
                                <td>
                                    <asp:TextBox ID="txtFromDate" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                        Font-Size="Medium" OnTextChanged="txtFromDate_TextChanged" Height="25px" Width="80px"
                                        AutoPostBack="True"></asp:TextBox>
                                    <asp:CalendarExtender ID="CalendarExtender1" TargetControlID="txtFromDate" Format="d/MM/yyyy"
                                        runat="server">
                                    </asp:CalendarExtender>
                                    <asp:FilteredTextBoxExtender ID="txtFromDate_FilteredTextBoxExtender" FilterType="Custom,Numbers"
                                        ValidChars="/" runat="server" TargetControlID="txtFromDate">
                                    </asp:FilteredTextBoxExtender>
                                </td>
                                <td>
                                    <asp:Label ID="lblTodate" runat="server" Text="To Date" Font-Bold="True" Font-Names="Book Antiqua"
                                        Font-Size="Medium"></asp:Label>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                </td>
                                <td>
                                    <asp:TextBox ID="txttoDate" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                        Font-Size="Medium" OnTextChanged="txttoDate_TextChanged" Height="25px" Width="80px"
                                        AutoPostBack="True"></asp:TextBox>
                                    <asp:CalendarExtender ID="CalendarExtender2" TargetControlID="txttoDate" Format="d/MM/yyyy"
                                        runat="server">
                                    </asp:CalendarExtender>
                                    <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender4" FilterType="Custom,Numbers"
                                        ValidChars="/" runat="server" TargetControlID="txttoDate">
                                    </asp:FilteredTextBoxExtender>
                                </td>
                                <td>
                                    <asp:CheckBox ID="chkPeriod" runat="server" Text="Period" Font-Bold="True" Font-Names="Book Antiqua"
                                        Font-Size="Medium" AutoPostBack="true" OnCheckedChanged="chkPeriod_CheckedChange" />
                                </td>
                                <td>
                                    <asp:DropDownList ID="ddlperiod" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                        Font-Size="Medium" Width="50px">
                                    </asp:DropDownList>
                                </td>
                                <td>
                                    <asp:UpdatePanel ID="UPgo" runat="server">
                                        <ContentTemplate>
                                            <asp:Button ID="btnGo" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                                Font-Size="Medium" Text="Go" OnClick="btnGo_Click" />
                                        </ContentTemplate>
                                        <Triggers>
                                            <asp:PostBackTrigger ControlID="btnGo" />
                                        </Triggers>
                                    </asp:UpdatePanel>
                                </td>
                                <td>
                                    <asp:Label ID="lblFromDate" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                        Font-Size="Medium" ForeColor="Red"></asp:Label>
                                </td>
                                <td>
                                    <asp:Label ID="lblpages" runat="server" Text="Page" Font-Names="Book Antiqua" Font-Size="Medium"
                                        Font-Bold="true"></asp:Label>
                                    <asp:DropDownList ID="ddlpage" runat="server" OnSelectedIndexChanged="ddlpage_SelectedIndexChanged"
                                        Font-Names="Book Antiqua" Font-Size="Medium" Font-Bold="true" AutoPostBack="True"
                                        Width="48px">
                                    </asp:DropDownList>
                                </td>
                            </tr>
                        </table>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </center>
            <br />
            <asp:UpdatePanel ID="UP2" runat="server">
                <ContentTemplate>
                    <table>
                        <tr>
                            <td>
                            </td>
                        </tr>
                        <tr>
                            <td>
                            </td>
                        </tr>
                        <tr>
                            <td>
                            </td>
                        </tr>
                        <tr>
                            <td>
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
                                    <asp:GridView ID="gview" runat="server" BorderStyle="Double" CssClass="grid-view"
                                        GridLines="Both" Font-Names="Book Antique" ShowFooter="false" Visible="true"
                                        OnRowCreated="gview_RowCreated" ShowHeader="false" OnRowDataBound="gridview1_DataBound"
                                        AllowCellMerging="true">
                                        <Columns>
                                        </Columns>
                                        <HeaderStyle BackColor="#0CA6CA" Font-Bold="true" ForeColor="Black" Font-Size="Large" />
                                        <FooterStyle BackColor="White" ForeColor="#333333" />
                                        <PagerStyle BackColor="#336666" HorizontalAlign="Center" />
                                        <RowStyle ForeColor="#333333" />
                                        <SelectedRowStyle BackColor="#339966" Font-Bold="True" />
                                    </asp:GridView>
                                    <%--<asp:GridView ID="gviewsamp" runat="server" BorderStyle="Double" CssClass="grid-view"
                                        GridLines="Both" Font-Names="Book Antique" ShowFooter="false" ShowHeader="false"
                                        AutoGenerateColumns="true" AllowCellMerging="true" OnRowDataBound="gviewsamp_OnRowDataBound">
                                        <Columns>
                                        </Columns>
                                        <HeaderStyle BackColor="#0CA6CA" Font-Bold="true" ForeColor="Black" Font-Size="Large" />
                                        <FooterStyle BackColor="White" ForeColor="#333333" />
                                        <PagerStyle BackColor="#336666" HorizontalAlign="Center" />
                                        <RowStyle ForeColor="#333333" />
                                        <SelectedRowStyle BackColor="#339966" Font-Bold="True" />
                                    </asp:GridView>--%>
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
                                    Font-Size="Medium" OnClick="btnxl_Click" />
                                <asp:Button ID="btnprintmaster" runat="server" Text="Print" OnClick="btnprintmaster_Click"
                                    Font-Names="Book Antiqua" Font-Size="Medium" Font-Bold="true" />
                                <NEW:NEWPrintMater runat="server" ID="NEWPrintMater1" Visible="false" />
                                <Insproplus:printmaster runat="server" ID="Printcontrol" Visible="false" />
                            </td>
                        </tr>
                    </table>
                </ContentTemplate>
                <Triggers>
                    <asp:PostBackTrigger ControlID="btnxl" />
                    <asp:PostBackTrigger ControlID="btnprintmaster" />
                </Triggers>
            </asp:UpdatePanel>
        </div>
    </body>
    </html>
    <center>
        <asp:UpdateProgress ID="UpdateProgress1" runat="server" AssociatedUpdatePanelID="UPgo">
            <ProgressTemplate>
                <center>
                    <div style="height: 40px; width: 150px;">
                        <img src="../gv images/cloud_loading_256.gif" style="height: 150px;" />
                        <br />
                        <span style="font-family: Book Antiqua; font-size: medium; font-weight: bold; color: Black;">
                            Processing Please Wait...</span>
                    </div>
                </center>
            </ProgressTemplate>
        </asp:UpdateProgress>
        <asp:ModalPopupExtender ID="ModalPopupExtender1" runat="server" TargetControlID="UpdateProgress1"
            PopupControlID="UpdateProgress1">
        </asp:ModalPopupExtender>
    </center>
</asp:Content>

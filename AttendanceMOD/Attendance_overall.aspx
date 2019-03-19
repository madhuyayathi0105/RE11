<%@ Page Title="" Language="C#" MasterPageFile="~/AttendanceMOD/AttendanceSubSiteMaster.master"
    AutoEventWireup="true" CodeFile="Attendance_overall.aspx.cs" Inherits="Attendance_overall" %>
    <%@ Register Src="~/Usercontrols/NewPrintMaster.ascx" TagName="NEWPrintMater" TagPrefix="NEW" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Src="~/Usercontrols/PrintMaster.ascx" TagName="PRINTPDF" TagPrefix="Insproplus" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
    <script runat="server">

        
    </script>
    <html>
    <style type="text/css">
        .style1
        {
        }
        .style2
        {
            width: 30px;
        }
    </style>
    <script type="text/javascript">
        function display() {
            document.getElementById('MainContent_errmsg').innerHTML = "";
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">
    <body>
        <asp:ScriptManager ID="ScriptManager1" runat="server">
        </asp:ScriptManager>
        <br />
        <center>
            <span class="fontstyleheader" style="color: Green;">AT14 - Overall Attendance Details
                -Splitup Report</span>
        </center>
        <br />
        <asp:UpdatePanel runat="server" ID="upanel1">
            <ContentTemplate>
                <div style="height: 89px">
                    <center>
                        <table class="maintablestyle">
                            <tr>
                                <td>
                                    <asp:Label ID="lblYear" runat="server" Text="Batch" Font-Bold="True" Font-Size="Medium"
                                        Font-Names="Book Antiqua"></asp:Label>
                                </td>
                                <td>
                                    <asp:DropDownList ID="ddlBatch" runat="server" AutoPostBack="True" Font-Bold="True"
                                        Font-Names="Book Antiqua" Font-Size="Medium" CausesValidation="True" OnSelectedIndexChanged="ddlBatch_SelectedIndexChanged1"
                                        Height="21px" Width="75px">
                                    </asp:DropDownList>
                                    <br />
                                </td>
                                <td>
                                    <asp:Label ID="lblDegree" runat="server" Text="Degree " Font-Bold="True" Font-Size="Medium"
                                        Font-Names="Book Antiqua">
                                    </asp:Label>
                                </td>
                                <td>
                                    <asp:DropDownList ID="ddlDegree" runat="server" AutoPostBack="True" Height="21px"
                                        Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium" CausesValidation="True"
                                        OnSelectedIndexChanged="ddlDegree_SelectedIndexChanged1">
                                    </asp:DropDownList>
                                </td>
                                <td class="style1">
                                    <asp:Label ID="lblBranch" runat="server" Text="Branch " Font-Bold="True" Font-Size="Medium"
                                        Font-Names="Book Antiqua"></asp:Label>
                                </td>
                                <td>
                                    <asp:DropDownList ID="ddlBranch" runat="server" AutoPostBack="True" Font-Bold="True"
                                        Font-Names="Book Antiqua" Font-Size="Medium" CausesValidation="True" Width="277px"
                                        OnSelectedIndexChanged="ddlBranch_SelectedIndexChanged1" Height="21px">
                                    </asp:DropDownList>
                                </td>
                                <td class="style2">
                                    <asp:Label ID="lblDuration" runat="server" Text="Sem" Font-Bold="True" Font-Size="Medium"
                                        Font-Names="Book Antiqua"> </asp:Label>
                                </td>
                                <td>
                                    <asp:DropDownList ID="ddlSemYr" runat="server" AutoPostBack="True" Height="21px"
                                        Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium" CausesValidation="True"
                                        OnSelectedIndexChanged="ddlSemYr_SelectedIndexChanged1" Width="48px">
                                    </asp:DropDownList>
                                </td>
                                <td>
                                    <asp:Label ID="lblSec" runat="server" Text="Sec" Font-Bold="True" Font-Size="Medium"
                                        Font-Names="Book Antiqua">
                                    </asp:Label>
                                </td>
                                <td>
                                    <asp:DropDownList ID="ddlSec" runat="server" AutoPostBack="true" Height="21px" Font-Bold="True"
                                        Font-Names="Book Antiqua" Font-Size="Medium" Width="49px">
                                    </asp:DropDownList>
                                    <br />
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="frmdate" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                        Font-Size="Medium" Text="From Date"></asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtFromDate" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                        Font-Size="Medium" Width="75px" OnTextChanged="txtFromDate_TextChanged"></asp:TextBox>
                                    <asp:CalendarExtender ID="CalendarExtender1" TargetControlID="txtFromDate" Format="dd/MM/yyyy"
                                        runat="server">
                                    </asp:CalendarExtender>
                                </td>
                                <td>
                                    &nbsp;
                                    <asp:Label ID="Todate" runat="server" Text="To Date" Font-Bold="True" Font-Size="Medium"
                                        Font-Names="Book Antiqua"></asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtToDate" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                        Font-Size="Medium" Width="80px" OnTextChanged="txtToDate_TextChanged"></asp:TextBox>
                                    <asp:CalendarExtender ID="CalendarExtender2" TargetControlID="txtToDate" Format="d/MM/yyyy"
                                        runat="server">
                                    </asp:CalendarExtender>
                                </td>
                                <td class="style1" colspan="5">
                                    <asp:UpdatePanel runat="server" ID="UPgo">
                                        <ContentTemplate>
                                            <asp:Button ID="Button1" runat="server" Text="Go" OnClick="btnGo_Click" Font-Bold="True"
                                                Font-Names="Book Antiqua" Font-Size="Medium" />
                                            <asp:Button ID="btnPrint" runat="server" Font-Bold="True" Text="Print Master Setting"
                                                Visible="False" Font-Names="Book Antiqua" Font-Size="Medium" OnClick="btnPrint_Click"
                                                Width="160px" />
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                            </tr>
                        </table>
                        <br />
                        <table>
                            <tr>
                                <td class="style14" colspan="2">
                                    <asp:Label ID="frmlbl" runat="server" Text="Select From Date" ForeColor="Red" Font-Bold="True"
                                        Font-Names="Book Antiqua"></asp:Label>
                                </td>
                                <td class="style15" colspan="2">
                                    <asp:Label ID="tolbl" runat="server" Text="Select To Date" ForeColor="Red" Font-Bold="True"
                                        Font-Names="Book Antiqua"></asp:Label>
                                </td>
                                <td colspan="3" class="style10">
                                    <asp:Label ID="tofromlbl" runat="server" Text="From date should not be greater than To date"
                                        ForeColor="Red" Font-Bold="True" Font-Names="Book Antiqua"></asp:Label>
                                </td>
                            </tr>
                        </table>
                    </center>
                    <br />
                    <center>
                        <asp:Label ID="lblnorec" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                            Font-Size="Medium" ForeColor="#FF3300" Text="No Record(s) Found" Visible="false"></asp:Label>
                        <asp:Label ID="errmsg" runat="server" ForeColor="Red" Font-Bold="True" Font-Names="Book Antiqua"
                            Font-Size="Medium" Height="16px" Width="280px"></asp:Label>
                    </center>
                    <br />
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
        <asp:UpdatePanel runat="server" ID="upanel2">
            <ContentTemplate>
                <table>
                    <tr>
                        <td>
                            <center>
                                <asp:GridView ID="gview" runat="server" BorderStyle="Double" CssClass="grid-view"
                                    AutoGenerateColumns="true" GridLines="Both" Font-Names="Book Antiqua" ShowFooter="false"
                                    ShowHeader="false">
                                    <Columns>
                                    </Columns>
                                    <HeaderStyle BackColor="#0CA6CA" Font-Bold="true" ForeColor="Black" Font-Size="Medium" />
                                    <FooterStyle BackColor="White" ForeColor="#333333" />
                                    <PagerStyle BackColor="#336666" HorizontalAlign="Center" />
                                    <RowStyle ForeColor="#333333" />
                                    <SelectedRowStyle BackColor="#339966" Font-Bold="True" />
                                </asp:GridView>
                            </center>
                        </td>
                    </tr>
                    <tr>
                        <td align="center">
                            <br />
                            <asp:Label ID="lblrptname" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                Font-Size="Medium" Text="Report Name"></asp:Label>
                            <asp:TextBox ID="txtexcelname" runat="server" Height="20px" Width="180px" Style="font-family: 'Book Antiqua'"
                                Font-Bold="True" Font-Names="Book Antiqua" onkeypress="display()" Font-Size="Medium"></asp:TextBox>
                            <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" runat="server" TargetControlID="txtexcelname"
                                FilterType="LowercaseLetters,UppercaseLetters,Numbers,Custom" ValidChars="!@$%^&*()_+|}{][':;?><,."
                                InvalidChars="/\">
                            </asp:FilteredTextBoxExtender>
                            <asp:Button ID="btnxl" runat="server" Text="Export Excel" Font-Bold="True" Font-Names="Book Antiqua"
                                Font-Size="Medium" OnClick="btnxl_Click" />
                            <asp:Button ID="btnprintmaster" runat="server" Text="Print" OnClick="btnprintmaster_Click"
                                Font-Names="Book Antiqua" Font-Size="Medium" Font-Bold="true" />
                                 <NEW:NEWPrintMater runat="server" ID="Printcontrol1" Visible="false" />
                            <Insproplus:PRINTPDF runat="server" ID="Printcontrol" Visible="false" />
                        </td>
                    </tr>
                </table>
            </ContentTemplate>
            <Triggers>
                <asp:PostBackTrigger ControlID="btnxl" />
                <asp:PostBackTrigger ControlID="btnprintmaster" />
            </Triggers>
        </asp:UpdatePanel>
    </body>
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

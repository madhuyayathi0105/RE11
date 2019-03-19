<%@ Page Title="AT11-Individual Subject Wise Attendance Report" Language="C#" MasterPageFile="~/AttendanceMOD/AttendanceSubSiteMaster.master"
    AutoEventWireup="true" CodeFile="singlesubject_wise_attendance.aspx.cs" Inherits="singlesubject_wise_attendance" %>
    <%@ Register Src="~/Usercontrols/NewPrintMaster.ascx" TagName="NEWPrintMater" TagPrefix="NEW" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Assembly="FarPoint.Web.Spread" Namespace="FarPoint.Web.Spread" TagPrefix="FarPoint" %>
<%@ Register Src="~/Usercontrols/PrintMaster.ascx" TagName="PRINTPDF" TagPrefix="Insproplus" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <style type="text/css">
        .style1
        {
            width: 109px;
        }
        .style2
        {
            width: 189px;
        }
        .style3
        {
            width: 419px;
        }
        .style4
        {
            width: 100px;
        }
        .style7
        {
            width: 84px;
        }
        .style9
        {
            width: 83px;
        }
        .style10
        {
            width: 301px;
        }
        .style11
        {
            width: 314px;
        }
        .txt
        {
        }
        .style12
        {
            width: 77px;
        }
        .style13
        {
            width: 326px;
        }
        .style14
        {
            width: 299px;
        }
        .style15
        {
            width: 297px;
        }
        .style16
        {
            width: 309px;
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

            document.getElementById('MainContent_errlbl').innerHTML = "";

        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <center>
        <span class="fontstyleheader" style="color: Green; margin: 0px; margin-bottom: 10px;
            margin-top: 10px;">AT11-Individual Subject Wise Attendance Report</span>
    </center>
    <center>
        <div class="maintablestyle" style="width: 900px; margin: 0px; margin-bottom: 10px;
            margin-top: 10px; text-align:left;">
            <table cellpadding="0px" cellspacing="0px" style="height: 100%; width: 103%; margin: 0px;
                margin-bottom: 10px; margin-top: 10px;">
                <tr>
                    <td class="style1">
                        <asp:Label runat="server" ID="lblbatch" Text="Batch" Font-Bold="True" Font-Names="Book Antiqua"
                            Font-Size="Medium"></asp:Label>
                        <asp:DropDownList ID="ddlbatch" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                            Font-Size="Medium" OnSelectedIndexChanged="ddlbatch_SelectedIndexChanged" Height="25px"
                            Width="60px" AutoPostBack="True">
                        </asp:DropDownList>
                    </td>
                    <td class="style2">
                        <asp:Label runat="server" ID="lbldegree" Text="Degree" Font-Bold="True" Font-Names="Book Antiqua"
                            Font-Size="Medium"></asp:Label>
                        <asp:DropDownList runat="server" ID="ddldegree" Height="24px" Width="88px" AutoPostBack="True"
                            Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium" OnSelectedIndexChanged="ddldegree_SelectedIndexChanged">
                        </asp:DropDownList>
                    </td>
                    <td class="style3">
                        <asp:Label runat="server" ID="lblbranch" Text="Branch" Font-Bold="True" Font-Names="Book Antiqua"
                            Font-Size="Medium"></asp:Label>
                        <asp:DropDownList runat="server" ID="ddlbranch" Font-Bold="True" Height="25px" Width="328px"
                            Font-Names="Book Antiqua" Font-Size="Medium" OnSelectedIndexChanged="ddlbranch_SelectedIndexChanged"
                            AutoPostBack="True">
                        </asp:DropDownList>
                    </td>
                    <td class="style4">
                        <asp:Label runat="server" ID="lblduration" Text="Sem" Font-Bold="True" Font-Names="Book Antiqua"
                            Font-Size="Medium"></asp:Label>
                        <asp:DropDownList runat="server" ID="ddlduration" Font-Bold="True" Font-Names="Book Antiqua"
                            Font-Size="Medium" AutoPostBack="True" Height="25px" Width="47px" OnSelectedIndexChanged="ddlduration_SelectedIndexChanged">
                        </asp:DropDownList>
                    </td>
                    <td class="style42">
                        <asp:Label runat="server" ID="lblsec" Text="Sec" Font-Bold="True" Font-Names="Book Antiqua"
                            Font-Size="Medium"></asp:Label>
                        <asp:DropDownList runat="server" ID="ddlsec" Font-Bold="True" Font-Names="Book Antiqua"
                            Font-Size="Medium" Height="25px" Width="49px" AutoPostBack="True" OnSelectedIndexChanged="ddlsec_SelectedIndexChanged">
                        </asp:DropDownList>
                    </td>
                </tr>
            </table>
            <table>
                <tr>
                    <td>
                        <asp:Label ID="lblsubject" runat="server" Text="Subject" Font-Bold="True" Font-Names="Book Antiqua"
                            Font-Size="Medium"></asp:Label>
                        <asp:DropDownList ID="ddlsubject" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                            Font-Size="Medium" Width="264px" OnSelectedIndexChanged="ddlsubject_SelectedIndexChanged"
                            AutoPostBack="True">
                        </asp:DropDownList>
                    </td>
                    <td class="style12">
                        <asp:Label ID="lblFromdate" runat="server" Text="From Date" Font-Bold="True" Font-Names="Book Antiqua"
                            Font-Size="Medium"></asp:Label>
                    </td>
                    <td class="style9">
                        <asp:TextBox ID="txtFromDate" CssClass="txt" runat="server" Height="20px" Width="75px"
                            Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium" OnTextChanged="txtFromDate_TextChanged"
                            AutoPostBack="True"></asp:TextBox>
                        <asp:FilteredTextBoxExtender ID="txtFromDate_FilteredTextBoxExtender" FilterType="Custom,Numbers"
                            ValidChars="/" runat="server" TargetControlID="txtFromDate">
                        </asp:FilteredTextBoxExtender>
                        <asp:CalendarExtender ID="calfromdate" TargetControlID="txtFromDate" Format="d/MM/yyyy"
                            runat="server">
                        </asp:CalendarExtender>
                    </td>
                    <td class="style39">
                        <asp:Label ID="lbltodate" runat="server" Text="To Date" Font-Bold="True" Font-Names="Book Antiqua"
                            Font-Size="Medium"></asp:Label>
                    </td>
                    <td class="style7">
                        <asp:TextBox ID="txtToDate" CssClass="txt" runat="server" Height="20px" Width="75px"
                            Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium" OnTextChanged="txtToDate_TextChanged"
                            AutoPostBack="True"></asp:TextBox>
                        <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" FilterType="Custom,Numbers"
                            ValidChars="/" runat="server" TargetControlID="txtToDate">
                        </asp:FilteredTextBoxExtender>
                        <asp:CalendarExtender ID="CalendarExtender1" TargetControlID="txtToDate" Format="d/MM/yyyy"
                            runat="server">
                        </asp:CalendarExtender>
                    </td>
                      <td>
                                    <asp:CheckBox ID="chkincludepastout" runat="server" Text="Include PassedOut" Font-Bold="True" Font-Names="Book Antiqua"
                                        Font-Size="Medium" OnCheckedChanged="includepastout_CheckedChanged" 
                                            AutoPostBack="True" />
                                </td>
                </tr>
            </table>
            <table>
                <tr>
                    <td>
                        <asp:Label ID="lbltest" runat="server" Text="Test Name" Font-Bold="True" Font-Names="Book Antiqua"
                            Font-Size="Medium">
                        </asp:Label>
                        <asp:DropDownList ID="ddltest" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                            Font-Size="Medium" Width="150px">
                        </asp:DropDownList>
                    </td>
                    <td>
                        <asp:CheckBox ID="chkonduty" runat="server" Text="Include On Duty Periods" Font-Bold="True"
                            Font-Names="Book Antiqua" Font-Size="Medium" />
                    </td>
                    <td>
                        <asp:CheckBox ID="chkondutyspit" runat="server" Text="On Duty Periods Details" Font-Bold="True"
                            OnCheckedChanged="chkondutyspit_CheckedChanged" Font-Names="Book Antiqua" Font-Size="Medium"
                            AutoPostBack="true" Width="200px" />
                    </td>
                    <td>
                        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                            <ContentTemplate>
                                <asp:TextBox ID="txtonduty" runat="server" CssClass="Dropdown_Txt_Box" ReadOnly="true"
                                    Width="110px" Style="font-family: 'Book Antiqua';" Font-Bold="True" Font-Names="Book Antiqua"
                                    Font-Size="Medium">---Select---</asp:TextBox>
                                <asp:Panel ID="ponduty" runat="server" Width="300px" CssClass="multxtpanel" Height="250px">
                                    <asp:CheckBox ID="chksonduty" runat="server" Font-Bold="True" OnCheckedChanged="chksonduty_ChekedChange"
                                        Font-Names="Book Antiqua" Font-Size="Medium" Text="Select All" AutoPostBack="True" />
                                    <asp:CheckBoxList ID="chklsonduty" runat="server" Font-Size="Medium" AutoPostBack="True"
                                        Font-Bold="True" Font-Names="Book Antiqua" OnSelectedIndexChanged="chklsonduty_SelectedIndexChanged">
                                    </asp:CheckBoxList>
                                </asp:Panel>
                                <asp:PopupControlExtender ID="PopupControlExtender1" runat="server" TargetControlID="txtonduty"
                                    PopupControlID="ponduty" Position="Bottom">
                                </asp:PopupControlExtender>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </td>
                    <td>
                        <asp:Button ID="btnGo" runat="server" Text="Go" OnClick="btnGo_Click" Font-Bold="True"
                            Font-Names="Book Antiqua" Font-Size="Medium" Style="width: auto; height: auto;"
                            CssClass="textbox textbox1" />
                    </td>
                    <td>
                    </td>
                </tr>
            </table>
        </div>
        <table>
            <tr>
                <td class="style13">
                </td>
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
                <td>
                    <asp:Label ID="norecordlbl" runat="server" ForeColor="Red"></asp:Label>
                </td>
            </tr>
        </table>
    </center>
    <center>
        <asp:Label ID="errlbl" runat="server" ForeColor="Red" Font-Bold="True" Font-Names="Book Antiqua"
            Font-Size="Medium" Height="20px"></asp:Label>
        <asp:Label ID="errmsg" runat="server" ForeColor="Red" Font-Bold="True" Font-Names="Book Antiqua"
            Font-Size="Medium" Height="16px"></asp:Label>
    </center>
    <br />
    <div>
    <asp:GridView ID="gview" runat="server" OnRowCreated="gview_RowCreated" ShowHeader="false" OnRowDataBound="gview_OnRowDataBound">
    <Columns>
    </Columns>
    <FooterStyle BackColor="White" ForeColor="#333333" />
    <HeaderStyle BackColor="#0CA6CA" Font-Bold="True" ForeColor="Black" />
    <PagerStyle BackColor="#336666"  HorizontalAlign="Center" />
    <RowStyle  ForeColor="#333333" />
    <SelectedRowStyle BackColor="#339966" Font-Bold="True" />
    </asp:GridView>
    </div>
    <br />
    <center>
        <table>
            <tr>
                <td>
                    <asp:Label ID="lblrptname" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                        Font-Size="Medium" Text="Report Name"></asp:Label>
                    <asp:TextBox ID="txtexcelname" runat="server" Height="20px" Width="180px" Style="font-family: 'Book Antiqua'"
                        Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium" onkeypress="display()"></asp:TextBox>
                    <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender4" runat="server" TargetControlID="txtexcelname"
                        FilterType="LowercaseLetters,UppercaseLetters,Numbers,Custom" ValidChars="!@$%^&*()_+|}{][':;?><,."
                        InvalidChars="/\">
                    </asp:FilteredTextBoxExtender>
                    <asp:Button ID="btnxl" Style="width: auto; height: auto;" CssClass="textbox textbox1"
                        runat="server" Text="Export Excel" Font-Bold="True" Font-Names="Book Antiqua"
                        Font-Size="Medium" OnClick="btnxl_Click" />
                    <asp:Button ID="btnprintmaster" Style="width: auto; height: auto;" CssClass="textbox textbox1"
                        runat="server" Text="Print" OnClick="btnprintmaster_Click" Font-Names="Book Antiqua"
                        Font-Size="Medium" Font-Bold="true" />
                        <NEW:NEWPrintMater runat="server" ID="Printcontrol1" Visible="false" />
                    <Insproplus:PRINTPDF runat="server" ID="Printcontrol" Visible="false" />
                </td>
            </tr>
        </table>
    </center>
    <center>
        <div id="divPopAlert" runat="server" visible="false" style="height: 550em; z-index: 2000;
            width: 100%; background-color: rgba(54, 25, 25, .2); position: absolute; top: 0%;
            left: 0%;">
            <center>
                <div id="divPopAlertContent" runat="server" class="table" style="background-color: White;
                    height: auto; width: auto; border: 5px solid #0CA6CA; border-top: 25px solid #0CA6CA;
                    left: 39%; right: 39%; top: 35%; padding: 5px; position: fixed; border-radius: 10px;">
                    <center>
                        <table style="height: 100px; width: 100%; padding: 5px;">
                            <tr>
                                <td align="center">
                                    <asp:Label ID="lblAlertMsg" runat="server" Style="color: Red;" Font-Bold="true" Font-Size="Medium"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <center>
                                        <asp:Button ID="btnPopAlertClose" Font-Bold="true" Font-Size="Medium" Font-Names="Book Antiqua"
                                            CssClass="textbox textbox1" Style="height: auto; width: auto;" OnClick="btnPopAlertClose_Click"
                                            Text="Ok" runat="server" />
                                    </center>
                                </td>
                            </tr>
                        </table>
                    </center>
                </div>
            </center>
        </div>
    </center>
</asp:Content>

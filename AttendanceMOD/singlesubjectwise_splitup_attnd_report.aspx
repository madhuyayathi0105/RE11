<%@ Page Title="AT13-Subject Wise Attendance Details – Splitup Report" Language="C#"
    MasterPageFile="~/AttendanceMOD/AttendanceSubSiteMaster.master" AutoEventWireup="true"
    CodeFile="singlesubjectwise_splitup_attnd_report.aspx.cs" Inherits="singlesubjectwise_splitup_attnd_report"
    EnableEventValidation="false" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Src="~/Usercontrols/NewPrintMaster.ascx" TagName="NEWPrintMater" TagPrefix="NEW" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <link href="~/Styles/css/Registration.css" rel="stylesheet" type="text/css" />
    <script src="//ajax.googleapis.com/ajax/libs/jquery/1.11.1/jquery.min.js"></script>
    <link href="~/Styles/css/Commoncss.css" rel="Stylesheet" type="text/css" />
    <style type="text/css">
        .style1
        {
            width: 109px;
        }
        .style2
        {
            width: 147px;
        }
        .style3
        {
            width: 389px;
        }
        .style4
        {
            width: 88px;
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
            width: 335px;
        }
        
        .printclass
        {
            display: none;
        }
        .marginSet
        {
            margin: 0px;
            padding: 0px;
        }
        .headerDisp
        {
            font-size: 25px;
            font-weight: bold;
        }
        .headerDisp1
        {
            font-family: Book Antiqua;
            font-size: medium;
        }
        @media print
        {
            #divMainContents
            {
                display: block;
            }
            .printclass
            {
                display: block;
                font-family: Book Antiqua;
            }
            .noprint
            {
                display: none;
            }
        }
        @media screen,print
        {
        
        }
        @page
        {
            size: A4;
        }
    </style>
    <script type="text/javascript">
        function display() {
            document.getElementById('MainContent_errmsg').innerHTML = "";
        }
        function PrintPanel() {
            var panel = document.getElementById("<%=divMainContents.ClientID %>");
            var printWindow = window.open('', '', 'height=842,width=1191');
            printWindow.document.write('<html');
            printWindow.document.write('<head>');
            printWindow.document.write('</head><body >');
            printWindow.document.write('<form>');
            printWindow.document.write(panel.innerHTML);
            printWindow.document.write(' </form>');
            printWindow.document.write('</body></html>');
            printWindow.document.close();
            setTimeout(function () {
                printWindow.print();
            }, 500);
            return false;
        }
    </script>
    <script type="text/javascript">
        function minmax(value, min, max) {
            if (parseFloat(value) < min || isNaN(parseFloat(value)))
                return "";
            else if (parseFloat(value) > max)
                return 100;
            else return value;
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <center>
        <span class="fontstyleheader" style="color: Green; margin: 0px; margin-bottom: 10px;
            margin-top: 10px; position: relative;">AT13-Subject Wise Attendance Details – Splitup
            Report</span>
        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
            <ContentTemplate>
                <div class="maintablestyle" style="width: 950px; padding: 5px; margin: 0px; margin-bottom: 10px;
                    margin-top: 10px; position: relative;">
                    <table cellpadding="0px" cellspacing="0px" style="height: 100%; width: 103%;">
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
                                <asp:UpdatePanel ID="UpdatePanel_sem" runat="server">
                                    <ContentTemplate>
                                        <asp:Label runat="server" ID="lblduration" Text="Sem" Font-Bold="True" Font-Names="Book Antiqua"
                                            Font-Size="Medium"></asp:Label>
                                        <asp:DropDownList runat="server" ID="ddlduration" Font-Bold="True" Font-Names="Book Antiqua"
                                            Font-Size="Medium" AutoPostBack="True" Height="25px" Width="47px" OnSelectedIndexChanged="ddlduration_SelectedIndexChanged">
                                        </asp:DropDownList>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
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
                            <td class="style13">
                                <asp:Label ID="lblsubject" runat="server" Text="Subject" Font-Bold="True" Font-Names="Book Antiqua"
                                    Font-Size="Medium"></asp:Label>
                                <asp:DropDownList ID="ddlsubject" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                    Font-Size="Medium" Width="264px" OnSelectedIndexChanged="ddlsubject_SelectedIndexChanged"
                                    AutoPostBack="True">
                                </asp:DropDownList>
                            </td>
                            <td>
                                <asp:Label ID="lblFromdate" runat="server" Text="From Date" Width="90px" Font-Bold="True"
                                    Font-Names="Book Antiqua" Font-Size="Medium"></asp:Label>
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
                            <td>
                                <asp:Label ID="lbltodate" runat="server" Text="To Date" Width="75px" Font-Bold="True"
                                    Font-Names="Book Antiqua" Font-Size="Medium"></asp:Label>
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
                            <td class="style10">
                                <asp:UpdatePanel ID="UpdatePanel_go" runat="server">
                                    <ContentTemplate>
                                        <asp:Button ID="btnGo" runat="server" Text="Go" Style="width: auto; height: auto;"
                                            CssClass="textbox textbox1" OnClick="btnGo_Click" Font-Bold="True" Font-Names="Book Antiqua"
                                            Font-Size="Medium" />
                                        <asp:Button ID="btnPrint" Style="width: auto; height: auto;" CssClass="textbox textbox1"
                                            runat="server" Font-Bold="True" Text="Print Master Setting" Visible="False" Font-Names="Book Antiqua"
                                            Font-Size="Medium" OnClick="btnPrint_Click" />
                                        <asp:Label ID="lblpages" runat="server" Text="Page" Font-Names="Book Antiqua" Font-Size="Medium"
                                            Font-Bold="true"></asp:Label>
                                        <asp:DropDownList ID="ddlpage" runat="server" OnSelectedIndexChanged="ddlpage_SelectedIndexChanged"
                                            Font-Names="Book Antiqua" Font-Size="Medium" Font-Bold="true" AutoPostBack="True"
                                            Width="47px">
                                        </asp:DropDownList>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="6">
                                <table>
                                    <tr>
                                        <td>
                                            <asp:Label ID="lblShortagePercentage" runat="server" Text="Attendance Shortage Percentage"
                                                Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtAttShoratgePecentage" Text="100" runat="server" Font-Bold="true"
                                                Font-Names="Book Antiqua" Font-Size="Medium" Width="80px" MaxLength="5" onkeyup="this.value = minmax(this.value, 0, 100)"></asp:TextBox>
                                            <asp:FilteredTextBoxExtender ID="filterAttShoratgePecentage" runat="server" FilterType="Numbers,Custom"
                                                ValidChars="." TargetControlID="txtAttShoratgePecentage" />
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
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
                        </tr>
                    </table>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
        <div>
            <table style="margin: 0px; margin-bottom: 10px; margin-top: 10px;">
                <tr>
                    <td colspan="4" align="center">
                        <asp:Label ID="errlbl" runat="server" Style="margin: 0px; margin-bottom: 10px; margin-top: 10px;
                            position: relative;" ForeColor="Red" Font-Bold="True" Font-Names="Book Antiqua"
                            Font-Size="Medium"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td colspan="4" align="center">
                        <asp:Panel ID="pnl_pagesetting" runat="server" BorderStyle="None" Width="949px">
                            <asp:Label ID="Buttontotal" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                Font-Size="Medium"></asp:Label>
                            <asp:Label ID="errmsg" runat="server" ForeColor="Red" Font-Bold="True" Font-Names="Book Antiqua"
                                Font-Size="Medium" Height="16px" Width="280px"></asp:Label>
                        </asp:Panel>
                    </td>
                </tr>
            </table>
        </div>
        <asp:UpdatePanel ID="UpdatePanel3" runat="server">
            <ContentTemplate>
                <div id="divMainContents" runat="server" style="display: table; margin: 0px; height: auto;
                    margin-bottom: 20px; margin-top: 10px; position: relative; width: auto; text-align: left;">
                    <table class="printclass" style="width: 98%; height: auto; margin: 0px; padding: 0px;">
                        <tr>
                            <td rowspan="5" style="width: 100px; margin: 0px; border: 0px;">
                                <asp:Image ID="imgLeftLogo2" runat="server" AlternateText="" ImageUrl="~/college/Left_Logo.jpeg"
                                    Width="100px" Height="100px" />
                            </td>
                            <th class="marginSet" align="center" colspan="6">
                                <span id="spCollegeName" class="headerDisp" runat="server"></span>
                            </th>
                        </tr>
                        <tr>
                            <th class="marginSet" align="center" colspan="6">
                                <span id="spAddr" class="headerDisp1" runat="server"></span>
                            </th>
                        </tr>
                        <tr>
                            <th class="marginSet" align="center" colspan="6">
                                <span id="spReportName" class="headerDisp1" runat="server"></span>
                            </th>
                        </tr>
                        <tr>
                            <td class="marginSet" colspan="3" align="center">
                                <span id="spDegreeName" class="headerDisp1" runat="server"></span>
                            </td>
                            <td class="marginSet" colspan="3" align="right">
                                <span id="spSem" class="headerDisp1" runat="server"></span>
                            </td>
                        </tr>
                        <tr>
                            <td class="marginSet" colspan="3" align="left">
                                <span id="spProgremme" class="headerDisp1" runat="server"></span>
                            </td>
                            <td class="marginSet" colspan="3" align="right">
                                <span id="spSection" class="headerDisp1" runat="server"></span>
                            </td>
                        </tr>
                    </table>
                    <asp:GridView ID="Showgrid" Style="height: auto;" runat="server" Visible="false"
                        HeaderStyle-ForeColor="Black" HeaderStyle-BackColor="#0CA6CA" AutoGenerateColumns="true"
                        ShowHeaderWhenEmpty="true" OnRowDataBound="Showgrid_OnRowDataBound">
                    </asp:GridView>
                </div>
                <div>
                    <table style="margin: 0px; margin-bottom: 10px; margin-top: 10px;">
                        <tr>
                            <td>
                                <asp:Label ID="lblexcl" runat="server" Text="Report Name" Font-Names="Book Antiqua"
                                    Font-Size="Medium" Font-Bold="True"></asp:Label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtexcel" runat="server" Font-Names="Book Antiqua" Font-Size="Medium"
                                    Font-Bold="True" onkeypress="display()"></asp:TextBox>
                                <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender4" runat="server" TargetControlID="txtexcel"
                                    FilterType="LowercaseLetters,UppercaseLetters,Numbers,Custom" ValidChars="!@$%^&*()_+|}{][':;?><,."
                                    InvalidChars="/\">
                                </asp:FilteredTextBoxExtender>
                            </td>
                            <td>
                                <asp:Button ID="btnxl" Style="width: auto; height: auto;" CssClass="textbox textbox1"
                                    runat="server" Text="Export Excel" Font-Bold="True" Font-Names="Book Antiqua"
                                    Font-Size="Medium" OnClick="btnxl_Click" />
                            </td>
                            <td>
                                <asp:Button ID="btnprintmaster" Style="width: auto; height: auto;" CssClass="textbox textbox1"
                                    runat="server" Text="Print" OnClick="btnprintmaster_Click" Font-Names="Book Antiqua"
                                    Font-Size="Medium" Font-Bold="true" />
                                <NEW:NEWPrintMater runat="server" ID="Printcontrol" Visible="false" />
                                <asp:Button ID="btnprintdirect" runat="server" Text="Direct Print" OnClientClick="return PrintPanel();"
                                    Font-Names="Book Antiqua" Font-Size="Medium" Font-Bold="true" Height="35px" CssClass="textbox textbox1" />
                            </td>
                        </tr>
                    </table>
                </div>
            </ContentTemplate>
            <Triggers>
                <asp:PostBackTrigger ControlID="btnxl" />
                <asp:PostBackTrigger ControlID="btnprintmaster" />
                <asp:PostBackTrigger ControlID="btnGo" />
            </Triggers>
        </asp:UpdatePanel>
    </center>
    <%--progressBar for Upbook_go--%>
    <center>
        <asp:UpdateProgress ID="UpdateProgress1" runat="server" AssociatedUpdatePanelID="UpdatePanel_go">
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
    <%--progressBar for Sem--%>
    <center>
        <asp:UpdateProgress ID="UpdateProgress2" runat="server" AssociatedUpdatePanelID="UpdatePanel_sem">
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
        <asp:ModalPopupExtender ID="ModalPopupExtender2" runat="server" TargetControlID="UpdateProgress2"
            PopupControlID="UpdateProgress2">
        </asp:ModalPopupExtender>
    </center>
</asp:Content>

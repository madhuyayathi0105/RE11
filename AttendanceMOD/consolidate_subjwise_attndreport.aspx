<%@ Page Title="AT12-Consolidate Attendance Details - Subject Wise Report" Language="C#"
    MasterPageFile="~/AttendanceMOD/AttendanceSubSiteMaster.master" AutoEventWireup="true"
    CodeFile="consolidate_subjwise_attndreport.aspx.cs" Inherits="consolidate_subjwise_attndreport" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Src="~/Usercontrols/NewPrintMaster.ascx" TagName="NEWPrintMater" TagPrefix="NEW" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <link href="~/Styles/css/Commoncss.css" rel="Stylesheet" type="text/css" />
    <script type="text/javascript">
        function display() {
            document.getElementById('MainContent_Label1').innerHTML = "";
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
    <style type="text/css">
        .style1
        {
            width: 122px;
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
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <center>
        <span class="fontstyleheader" style="color: Green; margin: 0px; margin-bottom: 10px;
            margin-top: 10px; position: relative;">AT12-Consolidate Attendance Details- Subject
            Wise Report</span>
    </center>
    <center>
        <div>
            <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                <ContentTemplate>
                    <table class="maintablestyle" style="width: auto; margin: 0px; margin-bottom: 10px;
                        margin-top: 10px; position: relative; text-align: left;">
                        <tr>
                            <td colspan="10">
                                <table>
                                    <tr>
                                        <td>
                                            <asp:Label runat="server" ID="lblbatch" Text="Batch" Font-Bold="True" Font-Names="Book Antiqua"
                                                Font-Size="Medium"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:DropDownList ID="ddlbatch" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                                Font-Size="Medium" OnSelectedIndexChanged="ddlbatch_SelectedIndexChanged" Width="80px"
                                                AutoPostBack="True">
                                            </asp:DropDownList>
                                        </td>
                                        <td>
                                            <asp:Label runat="server" ID="lbldegree" Text="Degree" Font-Bold="True" Font-Names="Book Antiqua"
                                                Font-Size="Medium"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:DropDownList runat="server" ID="ddldegree" AutoPostBack="True" Width="80px"
                                                OnSelectedIndexChanged="ddldegree_SelectedIndexChanged" Font-Bold="True" Font-Names="Book Antiqua"
                                                Font-Size="Medium">
                                            </asp:DropDownList>
                                        </td>
                                        <td>
                                            <asp:Label runat="server" ID="lblbranch" Text="Branch" Font-Bold="True" Font-Names="Book Antiqua"
                                                Font-Size="Medium"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:DropDownList runat="server" ID="ddlbranch" Font-Bold="True" Width="260px" Font-Names="Book Antiqua"
                                                Font-Size="Medium" OnSelectedIndexChanged="ddlbranch_SelectedIndexChanged" AutoPostBack="True">
                                            </asp:DropDownList>
                                        </td>
                                        <td>
                                            <asp:Label runat="server" ID="lblduration" Text="Sem" Font-Bold="True" Font-Names="Book Antiqua"
                                                Font-Size="Medium"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:UpdatePanel ID="UpdatePanel_sem" runat="server">
                                                <ContentTemplate>
                                                    <asp:DropDownList runat="server" ID="ddlduration" Font-Bold="True" Font-Names="Book Antiqua"
                                                        Width="50px" Font-Size="Medium" AutoPostBack="True" OnSelectedIndexChanged="ddlduration_SelectedIndexChanged">
                                                    </asp:DropDownList>
                                                </ContentTemplate>
                                            </asp:UpdatePanel>
                                        </td>
                                        <td>
                                            <asp:Label runat="server" ID="lblsec" Text="Sec" Font-Bold="True" Font-Names="Book Antiqua"
                                                Font-Size="Medium"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:DropDownList runat="server" ID="ddlsec" Font-Bold="True" Font-Names="Book Antiqua"
                                                Font-Size="Medium" Width="50px" AutoPostBack="True" OnSelectedIndexChanged="ddlsec_SelectedIndexChanged">
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="10">
                                <table>
                                    <tr>
                                        <td>
                                            <asp:Label ID="lblFromdate" runat="server" Text="From Date" Font-Bold="True" Font-Names="Book Antiqua"
                                                Font-Size="Medium" Width="80px"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtFromDate" CssClass="txt" runat="server" Height="20px" Width="75px"
                                                OnTextChanged="txtFromDate_TextChanged" Font-Bold="True" Font-Names="Book Antiqua"
                                                Font-Size="Medium" AutoPostBack="True"></asp:TextBox>
                                            <asp:FilteredTextBoxExtender ID="txtFromDate_FilteredTextBoxExtender" FilterType="Custom,Numbers"
                                                ValidChars="/" runat="server" TargetControlID="txtFromDate">
                                            </asp:FilteredTextBoxExtender>
                                            <asp:CalendarExtender ID="calfromdate" TargetControlID="txtFromDate" Format="d/MM/yyyy"
                                                runat="server">
                                            </asp:CalendarExtender>
                                        </td>
                                        <td>
                                            <asp:Label ID="lbltodate" runat="server" Text="To Date" Font-Bold="True" Font-Names="Book Antiqua"
                                                Font-Size="Medium" Width="60px"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtToDate" CssClass="txt" runat="server" Height="20px" Width="75px"
                                                Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium" OnTextChanged="txtToDate_TextChanged"
                                                AutoPostBack="True"></asp:TextBox>
                                            <asp:FilteredTextBoxExtender ID="txtToDate_FilteredTextBoxExtender" runat="server"
                                                TargetControlID="txtToDate" FilterType="Custom,Numbers" ValidChars="/">
                                            </asp:FilteredTextBoxExtender>
                                            <asp:CalendarExtender ID="caltodate" TargetControlID="txtToDate" Format="d/MM/yyyy"
                                                runat="server">
                                            </asp:CalendarExtender>
                                        </td>
                                        <td>
                                            <asp:CheckBox ID="chkonduty" runat="server" Text="Include On Duty Periods" Font-Bold="true"
                                                Font-Names="Book Antiqua" Font-Size="Medium" Width="203px" />
                                        </td>
                                        <td>
                                            <asp:CheckBox ID="chkondutyvisble" runat="server" Text="Duty Periods Visible" Font-Bold="true"
                                                Font-Names="Book Antiqua" Font-Size="Medium" Width="175px" AutoPostBack="true"
                                                OnCheckedChanged="chksorthead_CheckedChanged" />
                                        </td>
                                        <td>
                                            <asp:CheckBox ID="chkabsentvisble" runat="server" Text="Absent Periods Visible" Font-Bold="true"
                                                Font-Names="Book Antiqua" Font-Size="Medium" Width="203px" AutoPostBack="true"
                                                OnCheckedChanged="chksorthead_CheckedChanged" />
                                        </td>
                                        <td>
                                            <asp:UpdatePanel ID="UpdatePanel_go" runat="server">
                                                <ContentTemplate>
                                                    <asp:Button ID="btnGo" Style="width: auto; height: auto;" CssClass="textbox textbox1"
                                                        runat="server" Text="Go" OnClick="btnGo_Click" Font-Bold="True" Font-Names="Book Antiqua"
                                                        Font-Size="Medium" />
                                                </ContentTemplate>
                                            </asp:UpdatePanel>
                                        </td>
                                        <td>
                                            <asp:CheckBox ID="chksorthead" runat="server" Text="Short The Header" Font-Bold="true"
                                                Font-Names="Book Antiqua" Font-Size="Medium" Width="150px" AutoPostBack="true"
                                                OnCheckedChanged="chksorthead_CheckedChanged" />
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                    </table>
                    <div>
                        <table>
                        </table>
                    </div>
                    <asp:Label ID="errmsg" runat="server" Style="margin: 0px; margin-bottom: 10px; margin-top: 10px;"
                        ForeColor="Red" Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium"></asp:Label>
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
                        <asp:GridView ID="Showgrid" runat="server" Visible="false" HeaderStyle-ForeColor="Black"
                            HeaderStyle-BackColor="#0CA6CA" Font-Names="Book Antiqua" ShowHeaderWhenEmpty="true"
                            OnRowDataBound="Showgrid_OnRowDataBound">
                        </asp:GridView>
                    </div>
                    <div>
                        <table>
                            <tr>
                                <td>
                                    <asp:Label ID="lblrptname" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                        Font-Size="Medium" Text="Report Name"></asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtexcelname" runat="server" Height="20px" Width="180px" Style="font-family: 'Book Antiqua'"
                                        Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium"></asp:TextBox>
                                </td>
                                <td>
                                    <asp:Button ID="btnxl" runat="server" Style="width: auto; height: auto;" CssClass="textbox textbox1"
                                        Text="Export Excel" Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium"
                                        OnClick="btnxl_Click" />
                                </td>
                                <td>
                                    <asp:Button ID="btnprintmaster" Style="width: auto; height: auto;" CssClass="textbox textbox1"
                                        runat="server" Text="Print" OnClick="btnprintmaster_Click" Font-Names="Book Antiqua"
                                        Font-Size="Medium" Font-Bold="true" />
                                    <NEW:NEWPrintMater runat="server" ID="Printcontrol" Visible="false" />
                                </td>
                                <td>
                                    <asp:Button ID="btnPrint" runat="server" Text="Direct Print" OnClientClick="return PrintPanel();"
                                        Font-Names="Book Antiqua" Font-Size="Medium" Font-Bold="true" Height="35px" CssClass="textbox textbox1" />
                                </td>
                            </tr>
                        </table>
                    </div>
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
                </ContentTemplate>
                <Triggers>
                    <asp:PostBackTrigger ControlID="btnxl" />
                    <asp:PostBackTrigger ControlID="btnprintmaster" />
                    <asp:PostBackTrigger ControlID="btnPrint" />
                    <asp:PostBackTrigger ControlID="btnGo" />
                </Triggers>
            </asp:UpdatePanel>
        </div>
    </center>
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
</asp:Content>

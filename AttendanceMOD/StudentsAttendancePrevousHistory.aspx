<%@ Page Title="" Language="C#" MasterPageFile="~/AttendanceMOD/AttendanceSubSiteMaster.master"
    AutoEventWireup="true" CodeFile="StudentsAttendancePrevousHistory.aspx.cs" Inherits="AttendanceMOD_StudentsAttendancePrevousHistory" %>

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
        <div>
            <span class="fontstyleheader" style="color: Green; margin: 0px; margin-bottom: 10px;
                margin-top: 10px; position: relative;">Student's Previous Attendance Report</span>
        </div>
        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
            <ContentTemplate>
                <div id="divSearch" runat="server" visible="true" class="maindivstyle" style="width: 100%;
                    height: auto; margin: 0px; margin-bottom: 20px; margin-top: 10px; padding: 5px;
                    position: relative;">
                    <table class="maintablestyle" style="height: auto; margin-left: 0px; margin-top: 5px;
                        margin-bottom: 5px; padding: 5px;">
                        <tr>
                            <td>
                                <asp:Label ID="lblCollege" runat="server" Text="College" Font-Bold="True" Style="font-family: 'Book Antiqua';"
                                    ForeColor="Black" Font-Size="Medium" Font-Names="Book Antiqua"></asp:Label>
                            </td>
                            <td>
                                <asp:DropDownList ID="ddlCollege" runat="server" CssClass="dropdown" Style="font-family: 'Book Antiqua';"
                                    Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium" Width="200px" AutoPostBack="True"
                                    OnSelectedIndexChanged="ddlCollege_SelectedIndexChanged">
                                </asp:DropDownList>
                            </td>
                            <td>
                                <asp:Label ID="lblSearchBy" runat="server" Text="Search By" Font-Bold="True" Style="font-family: 'Book Antiqua';"
                                    ForeColor="Black" Font-Size="Medium" Font-Names="Book Antiqua"></asp:Label>
                            </td>
                            <td>
                                <asp:DropDownList ID="ddlSearchBy" runat="server" CssClass="dropdown" Style="font-family: 'Book Antiqua';"
                                    Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium" AutoPostBack="True"
                                    OnSelectedIndexChanged="ddlSearchBy_SelectedIndexChanged">
                                </asp:DropDownList>
                            </td>
                            <td>
                                <asp:Label ID="lblSearchStudent" AssociatedControlID="txtSearchStudent" runat="server"
                                    Text="Roll No" Font-Bold="True" Style="font-family: 'Book Antiqua';" ForeColor="Black"
                                    Font-Size="Medium" Font-Names="Book Antiqua"></asp:Label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtSearchStudent" runat="server" CssClass="font" Text=""></asp:TextBox>
                                <asp:FilteredTextBoxExtender ID="filterStudent" runat="server" TargetControlID="txtSearchStudent"
                                    FilterType="LowercaseLetters,UppercaseLetters,Numbers,custom" ValidChars=" -/">
                                </asp:FilteredTextBoxExtender>
                            </td>
                            <td>
                                <asp:Button ID="btnSearchStudent" CssClass="textbox textbox1" Font-Bold="True" Font-Size="Medium"
                                    Font-Names="Book Antiqua" runat="server" OnClick="btnSearchStudent_Click" Text="Search Student"
                                    Style="width: auto; height: auto;" />
                            </td>
                        </tr>
                    </table>
                    <div id="divStudentDetail" runat="server" style="border: solid 1px gray; width: 600px;"
                        visible="true">
                        <table>
                            <tr>
                                <td>
                                    <span style="color: #0CA6CA; font-weight: bold;">Student Name</span>
                                </td>
                                <td>
                                    <span>:</span>
                                </td>
                                <td>
                                    <asp:Label ID="lblStudentName" runat="server" Font-Bold="true" Text=""></asp:Label>
                                </td>
                                <td>
                                    <span style="color: #0CA6CA; font-weight: bold;">Roll No</span>
                                </td>
                                <td>
                                    <span>:</span>
                                </td>
                                <td>
                                    <asp:Label ID="lblStudentRollNo" runat="server" Font-Bold="true" Text=""></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <span style="color: #0CA6CA; font-weight: bold;">Admission No</span>
                                </td>
                                <td>
                                    <span>:</span>
                                </td>
                                <td>
                                    <asp:Label ID="lblAdmissionNo" runat="server" Font-Bold="true" Text=""></asp:Label>
                                </td>
                                <td>
                                    <span style="color: #0CA6CA; font-weight: bold;">Reg. No</span>
                                </td>
                                <td>
                                    <span>:</span>
                                </td>
                                <td>
                                    <asp:Label ID="lblRegNo" runat="server" Font-Bold="true" Text=""></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <span style="color: #0CA6CA; font-weight: bold;">Class</span>
                                </td>
                                <td>
                                    <span>:</span>
                                </td>
                                <td>
                                    <asp:Label ID="lblClassName" runat="server" Font-Bold="true" Text=""></asp:Label>
                                </td>
                            </tr>
                        </table>
                    </div>
                    <br />
                    <div id="divShowPrivious" runat="server" visible="false">
                        <table class="maindivstyle">
                            <tr>
                                <td>
                                    <asp:Label ID="lblyear" Text="Batch" runat="server"></asp:Label>
                                    <asp:Label ID="lblAppNo" runat="server" Visible="false"></asp:Label>
                                </td>
                                <td>
                                    <asp:DropDownList ID="ddlYear" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlYear_SelectedIndexChange">
                                    </asp:DropDownList>
                                </td>
                                <td>
                                    <asp:Label ID="lbldegree" Text="Degree" runat="server"></asp:Label>
                                </td>
                                <td>
                                    <asp:DropDownList ID="ddldegree" runat="server" Width="200px" AutoPostBack="true"
                                        OnSelectedIndexChanged="ddldegree_SelectedIndexChange">
                                    </asp:DropDownList>
                                </td>
                                <td>
                                    <asp:Label ID="lblsemester" Text="Semester" runat="server"></asp:Label>
                                </td>
                                <td>
                                    <asp:UpdatePanel ID="UpdatePanel_sem" runat="server">
                                        <ContentTemplate>
                                            <asp:DropDownList ID="ddlsemester" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlsemester_SelectedIndexChange">
                                            </asp:DropDownList>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                                <td>
                                    <asp:Label ID="lblsection" Text="Section" runat="server"></asp:Label>
                                </td>
                                <td>
                                    <asp:DropDownList ID="ddlsection" runat="server">
                                    </asp:DropDownList>
                                </td>
                                <td>
                                    <asp:Label ID="lblfrom" Text="From" runat="server"></asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txt_from" runat="server" Width="80px"></asp:TextBox>
                                    <asp:CalendarExtender ID="CalendarExtender1" TargetControlID="txt_from" runat="server"
                                        Format="dd/MM/yyyy" CssClass="cal_Theme1 ajax__calendar_active">
                                    </asp:CalendarExtender>
                                </td>
                                <td>
                                    <asp:Label ID="lblto" Text="To" runat="server"></asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txt_to" runat="server" Width="80px"></asp:TextBox>
                                    <asp:CalendarExtender ID="CalendarExtender2" TargetControlID="txt_to" runat="server"
                                        Format="dd/MM/yyyy" CssClass="cal_Theme1 ajax__calendar_active">
                                    </asp:CalendarExtender>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="lblReport" Text="Report Type" runat="server"></asp:Label>
                                </td>
                                <td>
                                    <asp:DropDownList ID="ddlReportType" runat="server">
                                        <asp:ListItem>Detailed</asp:ListItem>
                                        <asp:ListItem>Cumulative</asp:ListItem>
                                    </asp:DropDownList>
                                </td>
                                <td>
                                    <asp:UpdatePanel ID="UpdatePanel_go" runat="server">
                                        <ContentTemplate>
                                            <asp:Button ID="btnGo" runat="server" Text="Go" OnClick="btnGo_Click" />
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                            </tr>
                        </table>
                    </div>
                </div>
                <asp:Label ID="lblErrSearch" runat="server" Text="" ForeColor="Red" Visible="False"
                    Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium" Style="margin: 0px;
                    margin-bottom: 15px; margin-top: 10px;">
                </asp:Label>
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
                        HeaderStyle-ForeColor="Black" font-name="Book Antiqua" HeaderStyle-BackColor="#0CA6CA"
                        AutoGenerateColumns="true" ShowHeaderWhenEmpty="true" OnRowDataBound="Showgrid_OnRowDataBound">
                    </asp:GridView>
                </div>
                <div id="print" runat="server" visible="false">
                    <asp:Label ID="lblvalidation1" runat="server" Font-Names="Book Antiqua" Font-Size="Medium"
                        ForeColor="Red" Text="" Visible="false"></asp:Label>
                    <asp:Label ID="lblrptname" runat="server" Font-Names="Book Antiqua" Font-Size="Medium"
                        Text="Report Name"></asp:Label>
                    <asp:TextBox ID="txtexcelname" runat="server" Width="180px" onkeypress="display()"
                        CssClass="textbox textbox1 txtheight4"></asp:TextBox>
                    <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender4" runat="server" TargetControlID="txtexcelname"
                        FilterType="LowercaseLetters,UppercaseLetters,Numbers,Custom" ValidChars="(),.[]_"
                        InvalidChars="/\">
                    </asp:FilteredTextBoxExtender>
                    <asp:Button ID="btnExcel" runat="server" Font-Names="Book Antiqua" Font-Size="Medium"
                        OnClick="btnExcel_Click" Text="Export To Excel" Width="127px" Height="32px" CssClass="textbox textbox1" />
                    <asp:Button ID="btnprintmasterhed" runat="server" Font-Names="Book Antiqua" Font-Size="Medium"
                        Text="Print" OnClick="btnprintmaster_Click" Height="32px" Style="margin-top: 10px;"
                        CssClass="textbox textbox1" Width="60px" />
                    <NEW:NEWPrintMater runat="server" ID="Printcontrol" Visible="false" />
                    <asp:Button ID="btnPrint" runat="server" Text="Direct Print" OnClientClick="return PrintPanel();"
                        Font-Names="Book Antiqua" Font-Size="Medium" Font-Bold="true" Height="35px" CssClass="textbox textbox1" />
                </div>
            </ContentTemplate>
            <Triggers>
                <asp:PostBackTrigger ControlID="btnExcel" />
                <asp:PostBackTrigger ControlID="btnprintmasterhed" />
                <asp:PostBackTrigger ControlID="btnPrint" />
            </Triggers>
        </asp:UpdatePanel>
    </center>
    <%-- Confirmation --%>
    <center>
        <asp:UpdatePanel ID="UpdatePanel2" runat="server">
            <ContentTemplate>
                <div id="divConfirmBox" runat="server" visible="false" style="height: 550em; z-index: 1000;
                    width: 100%; background-color: rgba(54, 25, 25, .2); position: absolute; top: 0;
                    left: 0px;">
                    <center>
                        <div id="divConfirm" runat="server" class="table" style="background-color: White;
                            height: auto; width: 38%; border: 5px solid #0CA6CA; border-top: 25px solid #0CA6CA;
                            left: 30%; right: 30%; top: 40%; position: fixed; border-radius: 10px;">
                            <center>
                                <table style="height: auto; width: 100%; padding: 3px;">
                                    <tr>
                                        <td align="center">
                                            <asp:Label ID="lblConfirmMsg" runat="server" Text="Do You Want To Delete All Subject Remarks?"
                                                Style="color: Red;" Font-Bold="true" Font-Size="Medium"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <center>
                                                <asp:Button ID="btnYes" CssClass=" textbox btn1 textbox1" Style="height: 28px; width: 65px;"
                                                    OnClick="btnYes_Click" Text="Yes" runat="server" />
                                                <asp:Button ID="btnNo" CssClass=" textbox btn1 textbox1" Style="height: 28px; width: 65px;"
                                                    OnClick="btnNo_Click" Text="No" runat="server" />
                                            </center>
                                        </td>
                                    </tr>
                                </table>
                            </center>
                        </div>
                    </center>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
    </center>
    <%-- Alert Box --%>
    <center>
        <asp:UpdatePanel ID="UpdatePanel3" runat="server">
            <ContentTemplate>
                <div id="divPopAlert" runat="server" visible="false" style="height: 550em; z-index: 2000;
                    width: 100%; background-color: rgba(54, 25, 25, .2); position: absolute; top: 0%;
                    left: 0%;">
                    <center>
                        <div id="divPopAlertContent" runat="server" class="table" style="background-color: White;
                            height: 120px; width: 23%; border: 5px solid #0CA6CA; border-top: 25px solid #0CA6CA;
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
            </ContentTemplate>
        </asp:UpdatePanel>
        <%--progressBar for Go--%>
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
    </center>
</asp:Content>

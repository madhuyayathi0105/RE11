<%@ Page Title="Subject Wise Attendance Report" Language="C#" MasterPageFile="~/AttendanceMOD/AttendanceSubSiteMaster.master"
    AutoEventWireup="true" CodeFile="SubjectWiseAttendanceReport.aspx.cs" Inherits="SubjectWiseAttendanceReport"
    EnableEventValidation="false" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Src="~/Usercontrols/NewPrintMaster.ascx" TagName="NEWPrintMater" TagPrefix="NEW" %>
<%@ Register Src="~/Usercontrols/PrintMaster.ascx" TagName="Printcontrol" TagPrefix="Insproplus" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <script type="text/javascript">
        function display1() {
            document.getElementById("<%=lbl_norec1.ClientID %>").innerHTML = "";
        }
    </script>
    <script type="text/javascript">
        function printTTOutput() {
            var panel = document.getElementById("<%=printdiv.ClientID %>");
            var printWindow = window.open('', '', 'height=816,width=980');
            printWindow.document.write('<html><head>');
            printWindow.document.write('</head><body >');
            printWindow.document.write(panel.innerHTML);
            printWindow.document.write('</body></html>');
            printWindow.document.close();
            setTimeout(function () {
                printWindow.print();
            }, 500);
            return false;
        }
    </script>
    <style tyle="text/css">
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
            #printdiv
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
    <asp:UpdatePanel ID="UpdatePanel3" runat="server">
            <ContentTemplate>
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <center>
        <span class="fontstyleheader" style="color: Green; margin: 0px; margin-bottom: 10px;
            margin-top: 10px; position: relative;">Subject Wise Attendance Report</span>
        <div id="divSearch" class="maintablestyle" runat="server" visible="true" style="color: black;
            font-family: Book Antiqua; height: auto; width: 955px; padding-bottom: 10px;
            margin: 0px; margin-bottom: 10px; margin-top: 10px; padding: 6px; position: relative;">
            <table id="tblsearch" runat="server">
                <tr>
                    <td>
                        <asp:Label ID="lblCollege" runat="server" Text="College" Font-Bold="true" Font-Names="Book Antiqua"
                            Font-Size="Medium"></asp:Label>
                    </td>
                    <td>
                        <asp:DropDownList ID="ddlCollege" runat="server" Font-Names="Book Antiqua" Font-Size="Medium"
                            Font-Bold="true" Width="250px" AutoPostBack="True" OnSelectedIndexChanged="ddlCollege_SelectedIndexChanged">
                        </asp:DropDownList>
                    </td>
                    <td>
                        <asp:Label ID="lblBatch" runat="server" Text="Batch" Font-Bold="true" Font-Names="Book Antiqua"
                            Font-Size="Medium"></asp:Label>
                    </td>
                    <td>
                        <asp:DropDownList ID="ddlBatch" runat="server" Font-Bold="true" Font-Names="Book Antiqua"
                            Font-Size="Medium" Width="80px" CssClass="arrow" AutoPostBack="true" OnSelectedIndexChanged="ddlBatch_SelectedIndexChanged">
                        </asp:DropDownList>
                    </td>
                    <td>
                        <asp:Label ID="lblDegree" runat="server" Text="Degree" Font-Bold="True" Font-Names="Book Antiqua"
                            Font-Size="Medium"></asp:Label>
                    </td>
                    <td>
                        <asp:DropDownList ID="ddlDegree" runat="server" Visible="true" Font-Bold="true" Font-Names="Book Antiqua"
                            Font-Size="Medium" Width="120px" AutoPostBack="true" CssClass="arrow" OnSelectedIndexChanged="ddlDegree_SelectedIndexChanged">
                        </asp:DropDownList>
                        <asp:UpdatePanel ID="UpnlDegree" runat="server" Visible="false">
                            <ContentTemplate>
                                <asp:TextBox ID="txtDegree" Width=" 100px" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                    Font-Size="Medium" CssClass="textbox  txtheight2" ReadOnly="true">-- Select --</asp:TextBox>
                                <asp:Panel ID="pnlDegree" runat="server" CssClass="multxtpanel" Height="200px">
                                    <asp:CheckBox ID="chkDegree" Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium"
                                        runat="server" Text="Select All" AutoPostBack="True" OnCheckedChanged="chkDegree_CheckedChanged" />
                                    <asp:CheckBoxList ID="cblDegree" Font-Bold="True" Font-Size="Medium" Font-Names="Book Antiqua"
                                        runat="server" AutoPostBack="True" OnSelectedIndexChanged="cblDegree_SelectedIndexChanged">
                                    </asp:CheckBoxList>
                                </asp:Panel>
                                <asp:PopupControlExtender ID="popupExtDegree" runat="server" TargetControlID="txtDegree"
                                    PopupControlID="pnlDegree" Position="Bottom">
                                </asp:PopupControlExtender>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </td>
                    <td>
                        <asp:Label ID="lblBranch" runat="server" Text="Branch" Font-Bold="True" Font-Names="Book Antiqua"
                            Font-Size="Medium" AssociatedControlID=""></asp:Label>
                    </td>
                    <td>
                        <asp:DropDownList ID="ddlBranch" Visible="true" runat="server" Font-Bold="true" Font-Names="Book Antiqua"
                            Font-Size="Medium" Width="120Px" AutoPostBack="true" CssClass="arrow" OnSelectedIndexChanged="ddlBranch_SelectedIndexChanged">
                        </asp:DropDownList>
                        <asp:UpdatePanel ID="upnlBranch" runat="server" Visible="false">
                            <ContentTemplate>
                                <asp:TextBox ID="txtBranch" Width=" 100px" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                    Font-Size="Medium" CssClass="textbox  txtheight2" ReadOnly="true">-- Select --</asp:TextBox>
                                <asp:Panel ID="pnlBranch" runat="server" CssClass="multxtpanel" Height="200px">
                                    <asp:CheckBox ID="chkBranch" Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium"
                                        runat="server" Text="Select All" AutoPostBack="True" OnCheckedChanged="chkBranch_CheckedChanged" />
                                    <asp:CheckBoxList ID="cblBranch" Font-Bold="True" Font-Size="Medium" Font-Names="Book Antiqua"
                                        runat="server" AutoPostBack="True" OnSelectedIndexChanged="cblBranch_SelectedIndexChanged">
                                    </asp:CheckBoxList>
                                </asp:Panel>
                                <asp:PopupControlExtender ID="popupExtBranch" runat="server" TargetControlID="txtBranch"
                                    PopupControlID="pnlBranch" Position="Bottom">
                                </asp:PopupControlExtender>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </td>
                    <td>
                        <asp:Label ID="lblSem" runat="server" Text="Sem" Font-Bold="True" Font-Names="Book Antiqua"
                            Font-Size="Medium" AssociatedControlID=""></asp:Label>
                    </td>
                    <td>
                        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                            <ContentTemplate>
                                <asp:DropDownList ID="ddlSem" runat="server" Font-Bold="true" Font-Names="Book Antiqua"
                                    Font-Size="Medium" Width="60Px" AutoPostBack="true" CssClass="arrow" OnSelectedIndexChanged="ddlSem_SelectedIndexChanged">
                                </asp:DropDownList>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </td>
                </tr>
                <tr>
                    <td colspan="10">
                        <table>
                            <tr>
                                <td>
                                    <asp:Label runat="server" ID="lblSec" Text="Sec" Font-Bold="True" Font-Names="Book Antiqua"
                                        Font-Size="Medium"></asp:Label>
                                </td>
                                <td>
                                    <asp:DropDownList runat="server" ID="ddlSec" Font-Bold="True" Font-Names="Book Antiqua"
                                        Font-Size="Medium" Height="25px" Width="49px" AutoPostBack="True" OnSelectedIndexChanged="ddlSec_SelectedIndexChanged">
                                    </asp:DropDownList>
                                </td>
                                <td>
                                    <asp:Label ID="lblSubject" runat="server" Text="Subject" Font-Bold="True" Font-Names="Book Antiqua"
                                        Font-Size="Medium" AssociatedControlID=""></asp:Label>
                                </td>
                                <td>
                                    <asp:UpdatePanel ID="upnlSubject" runat="server">
                                        <ContentTemplate>
                                            <asp:TextBox ID="txtSubject" Width=" 150px" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                                Font-Size="Medium" CssClass="textbox  txtheight2" ReadOnly="true">-- Select --</asp:TextBox>
                                            <asp:Panel ID="pnlSubject" runat="server" CssClass="multxtpanel" Height="250px">
                                                <asp:CheckBox ID="chkSubject" Checked="true" Font-Bold="True" Font-Names="Book Antiqua"
                                                    Font-Size="Medium" runat="server" Text="Select All" AutoPostBack="True" OnCheckedChanged="chkSubject_CheckedChanged" />
                                                <asp:CheckBoxList ID="cblSubject" Font-Bold="True" Font-Size="Medium" Font-Names="Book Antiqua"
                                                    runat="server" AutoPostBack="True" OnSelectedIndexChanged="cblSubject_SelectedIndexChanged">
                                                </asp:CheckBoxList>
                                            </asp:Panel>
                                            <asp:PopupControlExtender ID="popupExtSubject" runat="server" TargetControlID="txtSubject"
                                                PopupControlID="pnlSubject" Position="Bottom">
                                            </asp:PopupControlExtender>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                                <td>
                                    <asp:Label ID="lblFromDate" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                        Font-Size="Medium" Text="From Date"></asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtFromDate" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                        Font-Size="Medium" Width="75px" AutoPostBack="True" OnTextChanged="txtFromDate_TextChanged"></asp:TextBox>
                                    <asp:CalendarExtender ID="calExtFromDate" TargetControlID="txtFromDate" Format="dd/MM/yyyy"
                                        runat="server">
                                    </asp:CalendarExtender>
                                </td>
                                <td>
                                    <asp:Label ID="lblToDate" runat="server" Text="To Date" Font-Bold="True" Font-Size="Medium"
                                        Font-Names="Book Antiqua"></asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtToDate" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                        Font-Size="Medium" Width="80px" AutoPostBack="True" OnTextChanged="txtToDate_TextChanged"></asp:TextBox>
                                    <asp:CalendarExtender ID="calExtToDate" TargetControlID="txtToDate" Format="dd/MM/yyyy"
                                        runat="server">
                                    </asp:CalendarExtender>
                                </td>
                                <td>

                                
                                    <asp:Button ID="btnGo" Style="width: auto; height: auto;" CssClass="textbox textbox1"
                                        runat="server" Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium"
                                        OnClick="btnGo_Click" Text="Go" />

                                        
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
            </table>
        </div>
        <asp:Label ID="lblErrmsg" runat="server" Font-Names="Book Antiqua" Font-Size="Medium"
            Font-Bold="true" ForeColor="Red" Text="" Visible="false" Style="margin: 0px;
            margin-bottom: 10px; margin-top: 10px; position: relative;"></asp:Label>
    </center>
    <center>
        <div id="divAttendanceReport" runat="server" visible="false" style="margin: 0px;
            margin-bottom: 10px; margin-top: 10px; width: auto; height: auto;">
            <center>
                 <div id="printdiv" runat="server">
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
     <center>
                                   <asp:GridView ID="Showgrid" runat="server" Visible="false" HeaderStyle-ForeColor="Black"
                    HeaderStyle-BackColor="#0CA6CA" >
                </asp:GridView>
                        </center>
            <table class="printclass" style="width: 98%; height: auto; margin-top: 100px; padding: 0px;">
                <tr>
                    <td>
                        
                    </td>
                    <td style="text-align: right">
                        
                    </td>
                </tr>
            </table>
        </div>
                
            </center>
            <div id="rptprint1" runat="server" visible="false" style="margin: 0px; margin-top: 20px;
                margin-bottom: 20px;">
                <center>
                    <table>
                        <tr>
                            <td>
                                <asp:Label ID="lbl_norec1" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                    Font-Size="Medium" ForeColor="Red" Text="Please Enter Your Report Name" Visible="false"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="lblrptname1" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                    Font-Size="Medium" Text="Report Name"></asp:Label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtexcelname1" runat="server" CssClass="textbox textbox1" Height="20px"
                                    Width="180px" Style="font-family: 'Book Antiqua'" Font-Bold="True" Font-Names="Book Antiqua"
                                    onkeypress="display1()" Font-Size="Medium"></asp:TextBox>
                                <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender9" runat="server" TargetControlID="txtexcelname1"
                                    FilterType="LowercaseLetters,UppercaseLetters,Numbers,Custom" ValidChars="(),.[]_"
                                    InvalidChars="/\">
                                </asp:FilteredTextBoxExtender>
                            </td>
                            <td>
                                <asp:Button ID="btnExcel1" runat="server" Font-Bold="true" Font-Names="Book Antiqua"
                                    OnClick="btnExcel1_Click" Font-Size="Medium" Text="Export To Excel" Style="width: auto;
                                    height: auto;" CssClass="textbox textbox1" />
                            </td>
                            <td>
                                <asp:Button ID="btnprintmaster1" runat="server" Text="Print" OnClick="btnprintmaster1_Click"
                                    Font-Names="Book Antiqua" Font-Size="Medium" Font-Bold="true" Style="width: auto;
                                    height: auto;" CssClass="textbox textbox1" />
                                <NEW:NEWPrintMater runat="server" ID="Printcontrol" Visible="false" />
                            </td>
                            <td>
                                <button id="btnPrint" runat="server"  height="29px" width="62px" onclick="return printTTOutput();"
            style=" font-weight: bold; font-size: medium; font-family: Book Antiqua;">
            Direct Print
        </button>
                            </td>
                        </tr>
                    </table>
                </center>
            </div>
            <%--<table style="margin: 0px; margin-bottom: 10px; margin-top: 10px; width: auto; height: auto;">
                <tr>
                    <td colspan="4" align="center">
                    </td>
                </tr>
                <tr>
                    <td colspan="4" align="center">
                    </td>
                </tr>
            </table>--%>
        </div>
    </center>
    <center>
        <div id="divPopupAlert" runat="server" visible="false" style="height: 100em; z-index: 1000;
            width: 100%; background-color: rgba(54, 25, 25, .2); position: absolute; top: 0;
            left: 0px;">
            <center>
                <div id="pnlPopupAlert" runat="server" class="table" style="background-color: White;
                    height: 120px; width: 238px; border: 5px solid #0CA6CA; border-top: 25px solid #0CA6CA;
                    margin-top: 200px; border-radius: 10px;">
                    <center>
                        <table style="height: 100px; width: 100%">
                            <tr>
                                <td align="center">
                                    <asp:Label ID="lblPopupAlert" runat="server" Text="" Style="color: Red;" Font-Bold="true"
                                        Font-Size="Medium"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <center>
                                        <asp:Button ID="btnPopupClose" CssClass=" textbox textbox1 btn1" Style="height: 28px;
                                            width: 65px;" OnClick="btnPopupClose_Click" Text="Ok" runat="server" />
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
            <asp:PostBackTrigger ControlID="btnExcel1" />
            <asp:PostBackTrigger ControlID="btnGo" />
        </Triggers>
    </asp:UpdatePanel>
     

</asp:Content>

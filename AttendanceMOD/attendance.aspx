<%@ Page Title="" Language="C#" MasterPageFile="~/AttendanceMOD/AttendanceSubSiteMaster.master"
    AutoEventWireup="true" CodeFile="attendance.aspx.cs" Inherits="Attendance" %>

<%@ Register Src="~/Usercontrols/NewPrintMaster.ascx" TagName="NEWPrintMater" TagPrefix="NEW" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Src="~/Usercontrols/PrintMaster.ascx" TagName="printmaster" TagPrefix="Insproplus" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">

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
    <!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN">
    <html>
    <style type="text/css">
        .ModalPopupBG
        {
            background-color: #666699;
            filter: alpha(opacity=50);
            opacity: 0.7;
        }
        
        .HellowWorldPopup
        {
            min-width: 600px;
            min-height: 400px;
            background: white;
        }
        .style41
        {
            width: 62px;
        }
        .style42
        {
            width: 94px;
        }
    </style>
    <script type="text/javascript">
        function display() {

            document.getElementById('MainContent_lblnorec').innerHTML = "";

        }
    </script>
    <body>
    <asp:UpdatePanel ID="UpdatePanel3" runat="server">
            <ContentTemplate>
        <asp:ScriptManager ID="ScriptManager1" runat="server">
        </asp:ScriptManager>
        <br />
        <center>
            <span class="fontstyleheader" style="color: Green;">AT16 - Overall Attendance
                Percentage Report</span>
        </center>
        <br />
        <div>
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
                        <td>
                            <asp:Label ID="lblFromDate" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                Font-Size="Medium" Text="From">
                            </asp:Label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtFromDate" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                Font-Size="Medium" Width="75px" Height="23px" AutoPostBack="True" OnTextChanged="txtFromDate_TextChanged"></asp:TextBox>
                            <asp:CalendarExtender ID="CalendarExtender1" runat="server" Format="d/MM/yyyy" TargetControlID="txtFromDate">
                            </asp:CalendarExtender>
                            <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender2" runat="server" TargetControlID="txtFromDate"
                                FilterType="Numbers,Custom" ValidChars="/" />
                        </td>
                        <td>
                            <asp:Label ID="lblToDate" runat="server" Width="25px" Font-Bold="True" Font-Names="Book Antiqua"
                                Font-Size="Medium" Text="To">
                            </asp:Label>
                        </td>
                        <td class="style42">
                            <asp:TextBox ID="txtToDate" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                Font-Size="Medium" Width="80px" Height="23px" AutoPostBack="True" OnTextChanged="txtToDate_TextChanged"></asp:TextBox>
                            <asp:CalendarExtender ID="CalendarExtender2" runat="server" Format="d/MM/yyyy" TargetControlID="txtToDate">
                            </asp:CalendarExtender>
                            <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender4" runat="server" TargetControlID="txtToDate"
                                FilterType="Numbers,Custom" ValidChars="/" />
                        </td>
                        <td>
                            <asp:Label ID="lblTest" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                Font-Size="Medium" Text="%From">
                            </asp:Label>
                        </td>
                        <td>
                            <asp:TextBox ID="TextBox1" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                Font-Size="Medium" AutoPostBack="true" OnTextChanged="TextBox1_TextChanged" Height="23px"
                                Width="38px"></asp:TextBox>
                            <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" runat="server" TargetControlID="TextBox1"
                                FilterType="Numbers" />
                        </td>
                        <td>
                            <asp:Label ID="Label2" runat="server" Text="To" Font-Bold="True" Font-Names="Book Antiqua"
                                Font-Size="Medium"></asp:Label>
                        </td>
                        <td>
                            <asp:TextBox ID="TextBox2" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                Font-Size="Medium" AutoPostBack="true" OnTextChanged="TextBox2_TextChanged" Height="23px"
                                Width="36px"></asp:TextBox>
                            <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender3" runat="server" TargetControlID="TextBox2"
                                FilterType="Numbers" />
                        </td>
                        <td>
                        <asp:UpdatePanel ID="btngoUpdatePanel" runat="server">
                                <ContentTemplate>
                            <asp:Button ID="btnGo" runat="server" OnClick="btnGo_Click" Text="Go" Font-Bold="True"
                                Font-Names="Book Antiqua" Font-Size="Medium" />

                                </ContentTemplate>
                        </asp:UpdatePanel>
                        </td>
                    </tr>
                </table>
                <table>
                    <tr>
                        <td>
                        </td>
                        <td>
                        </td>
                        <td>
                            <asp:Label ID="frmlbl" runat="server" Text="Select From Date" ForeColor="Red" Font-Bold="True"
                                Font-Names="Book Antiqua"></asp:Label>
                        </td>
                        <td>
                            <asp:Label ID="tolbl" runat="server" Text="Select To Date" ForeColor="Red" Font-Bold="True"
                                Font-Names="Book Antiqua"></asp:Label>
                        </td>
                        <td>
                            <asp:Label ID="tofromlbl" runat="server" Text="From date should not be greater than To date"
                                ForeColor="Red" Font-Bold="True" Font-Names="Book Antiqua"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="6">
                            <asp:Label ID="Label3" runat="server" Text="Percentage Must be Between 0 to 100"
                                ForeColor="Red" Font-Underline="False" Visible="false" Font-Bold="True" Font-Names="Book Antiqua"
                                Font-Size="Medium"></asp:Label>
                        </td>
                    </tr>
                </table>
            </center>
            <br />
            <center>
                <asp:Label ID="lblnorec" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                    Font-Size="Medium" ForeColor="#FF3300" Text="No Record(s) Found" Visible="False"></asp:Label>
            </center>
            <br />

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
            
            <br />
            <center>
                <asp:Label ID="lblrptname" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                    Font-Size="Medium" Text="Report Name"></asp:Label>
                <asp:TextBox ID="txtexcelname" runat="server" Height="20px" Width="180px" Style="font-family: 'Book Antiqua'"
                    Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium" onkeypress="display()"></asp:TextBox>
                <asp:Button ID="btnxl" runat="server" Text="Export Excel" Font-Bold="True" Font-Names="Book Antiqua"
                    Font-Size="Medium" OnClick="btnxl_Click" />
                <asp:Button ID="btnprintmaster" runat="server" Text="Print" OnClick="btnprintmaster_Click" Font-Names="Book Antiqua" Font-Size="Medium"
                    Font-Bold="true" />
                 <NEW:NEWPrintMater runat="server" ID="Printcontrol" Visible="false" />

                <button id="btnPrint" runat="server"  height="29px" width="62px" onclick="return printTTOutput();"
            style=" font-weight: bold; font-size: medium; font-family: Book Antiqua;">
            Direct Print
        </button>
            </center>
        </div>

        </ContentTemplate>
        <Triggers>
            <asp:PostBackTrigger ControlID="btnxl" />
            <asp:PostBackTrigger ControlID="btnGo" />
            
        </Triggers>
    </asp:UpdatePanel>
     <center>
        <asp:UpdateProgress ID="UpdateProgress1" runat="server" AssociatedUpdatePanelID="btngoUpdatePanel">
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

    </body>
    </html>
</asp:Content>

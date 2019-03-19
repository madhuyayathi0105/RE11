<%@ Page Title="" Language="C#" MasterPageFile="~/AttendanceMOD/AttendanceSubSiteMaster.master"
    AutoEventWireup="true" CodeFile="attnd_fine_report.aspx.cs" Inherits="attnd_fine_report" %>

<%@ Register Src="~/Usercontrols/NewPrintMaster.ascx" TagName="NEWPrintMater" TagPrefix="NEW" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Src="~/Usercontrols/PrintMaster.ascx" TagName="PRINTPDF" TagPrefix="Insproplus" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <style type="text/css">
        .style1
        {
            width: 110px;
        }
        .style2
        {
            width: 128px;
        }
        .style3
        {
            width: 322px;
        }
        .style4
        {
            width: 80px;
        }
        .style6
        {
            width: 744px;
        }
        .style7
        {
            width: 397px;
        }
    </style>
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
    <html>
    <body>
        <script type="text/javascript">
            function display() {
                document.getElementById('MainContent_errlbl').innerHTML = "";
            }
        </script>
        <asp:UpdatePanel ID="UpdatePanel3" runat="server">
            <ContentTemplate>
        <asp:ScriptManager ID="ScriptManager1" runat="server">
        </asp:ScriptManager>
        <br />
        <center>
            <span class="fontstyleheader" style="color: Green;">AT08-Attendance Fine Report</span>
        </center>
        <br />
        <div>
            <center>
                <div class="maintablestyle" style="width: 800px;">
                    <table>
                        <tr>
                            <td class="style1">
                                <asp:Label runat="server" ID="lblbatch" Text="Batch" Font-Bold="True" Font-Names="Book Antiqua"
                                    Font-Size="Medium"></asp:Label>
                                <asp:DropDownList ID="ddlbatch" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                    Font-Size="Medium" OnSelectedIndexChanged="ddlbatch_SelectedIndexChanged" Height="25px"
                                    Width="56px" AutoPostBack="True">
                                </asp:DropDownList>
                            </td>
                            <td class="style2">
                                <asp:Label runat="server" ID="lbldegree" Text="Degree" Font-Bold="True" Font-Names="Book Antiqua"
                                    Font-Size="Medium"></asp:Label>
                                <asp:DropDownList runat="server" ID="ddldegree" Height="25px" Width="66px" AutoPostBack="True"
                                    OnSelectedIndexChanged="ddldegree_SelectedIndexChanged" Font-Bold="True" Font-Names="Book Antiqua"
                                    Font-Size="Medium">
                                </asp:DropDownList>
                            </td>
                            <td class="style3">
                                <asp:Label runat="server" ID="lblbranch" Text="Branch" Font-Bold="True" Font-Names="Book Antiqua"
                                    Font-Size="Medium"></asp:Label>
                                <asp:DropDownList runat="server" ID="ddlbranch" Font-Bold="True" Height="25px" Width="260px"
                                    Font-Names="Book Antiqua" Font-Size="Medium" OnSelectedIndexChanged="ddlbranch_SelectedIndexChanged"
                                    AutoPostBack="True">
                                </asp:DropDownList>
                            </td>
                            <td class="style4">
                                <asp:Label runat="server" ID="lblduration" Text="Sem" Font-Bold="True" Font-Names="Book Antiqua"
                                    Font-Size="Medium"></asp:Label>
                                <asp:DropDownList runat="server" ID="ddlduration" Font-Bold="True" Font-Names="Book Antiqua"
                                    Font-Size="Medium" AutoPostBack="True" Height="25px" Width="40px" OnSelectedIndexChanged="ddlduration_SelectedIndexChanged">
                                </asp:DropDownList>
                            </td>
                            <td class="style17">
                                <asp:Label runat="server" ID="lblsec" Text="Sec" Font-Bold="True" Font-Names="Book Antiqua"
                                    Font-Size="Medium"></asp:Label>
                                <asp:DropDownList runat="server" ID="ddlsec" Font-Bold="True" Font-Names="Book Antiqua"
                                    Font-Size="Medium" Height="25px" Width="61px" AutoPostBack="True" OnSelectedIndexChanged="ddlsec_SelectedIndexChanged">
                                </asp:DropDownList>
                            </td>
                            <td>
                            </td>
                            <td>
                            </td>
                        </tr>
                    </table>
                    <table>
                        <tr>
                            <td class="style9">
                                <asp:Label ID="lblFromdate" runat="server" Text="From Date" Font-Bold="True" Font-Names="Book Antiqua"
                                    Font-Size="Medium" Width="92px"></asp:Label>
                            </td>
                            <td class="style8">
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
                            <td class="style10">
                                <asp:Label ID="lbltodate" runat="server" Text="To Date" Font-Bold="True" Font-Names="Book Antiqua"
                                    Font-Size="Medium" Width="73px"></asp:Label>
                            </td>
                            <td class="style12">
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
                            <td class="style13">
                            
                                <asp:Button ID="btnGo" runat="server" Text="Go" Style="font-weight: 700" OnClick="btnGo_Click"
                                    Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium" Height="27px" Width="36px" />
                                 
                            </td>
                            <td class="style7">
                                <asp:Button ID="btn_print_setting" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                    Text="Print Master Setting" Visible="False" OnClick="btn_print_setting_Click"
                                    Font-Size="Medium" />
                            </td>
                            <td>
                                <asp:Label ID="errmsg" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                    ForeColor="Red" Width="151px" Height="16px" Font-Size="Medium" Style="margin-left: 4px"></asp:Label>
                            </td>
                        </tr>
                    </table>
                </div>
            </center>
            <table>
                <tr>
                    <td class="style6">
                    </td>
                </tr>
                <tr>
                    <td class="style6">
                    </td>
                </tr>
                <tr>
                    <td class="style6">
                    </td>
                </tr>
                <tr>
                    <td class="style6">
                    </td>
                </tr>
                <tr>
                    <td class="style6">
                    </td>
                </tr>
                <tr>
                    <td class="style6">
                    </td>
                </tr>
                <tr>
                    <td class="style6">
                    </td>
                </tr>
                <tr>
                    <td>
                        <center>
                            <asp:Label ID="noreclbl" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                ForeColor="Red" Font-Size="Medium"></asp:Label>
                        </center>
                    </td>
                </tr>
                <tr>
                    <td class="style6">
                        <asp:Panel ID="Panel3" runat="server" BorderStyle="None" Width="1026px">
                            <asp:Label ID="Buttontotal" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                Font-Size="Medium"></asp:Label>
                            <asp:Label ID="errlbl" runat="server" ForeColor="Red" Font-Bold="True" Font-Names="Book Antiqua"
                                Height="19px" Width="234px" Font-Size="Medium"></asp:Label>
                        </asp:Panel>
                    </td>
                </tr>
                <tr>
                    <td class="style6">

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
            <asp:GridView ID="grdover" runat="server" Width="500px" BorderStyle="Double" Font-Bold="true"
                            Font-Names="Book Antiqua" Font-Size="Medium" GridLines="Both" CellPadding="4"
                            ShowFooter="false" ShowHeader="true">
                            <Columns>
                            </Columns>
                            <FooterStyle BackColor="White" ForeColor="#333333" />
                            <HeaderStyle BackColor="#0CA6CA" Font-Bold="True" ForeColor="Control" />
                            <PagerStyle BackColor="#336666"  HorizontalAlign="Center" />
                            <RowStyle BackColor="White" ForeColor="#333333" />
                            <SelectedRowStyle BackColor="#339966" Font-Bold="True"  />
                            <SortedAscendingCellStyle BackColor="#F7F7F7" />
                            <SortedAscendingHeaderStyle BackColor="#487575" />
                            <SortedDescendingCellStyle BackColor="#E5E5E5" />
                            <SortedDescendingHeaderStyle BackColor="#275353" />
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

                        <center>
                        <div id="Div1" runat="server">
            <table class="printclass" style="width: 98%; height: auto; margin: 0px; padding: 0px;">
                <tr>
                    <td rowspan="5" style="width: 100px; margin: 0px; border: 0px;">
                        <asp:Image ID="Image1" runat="server" AlternateText="" ImageUrl="~/college/Left_Logo.jpeg"
                            Width="100px" Height="100px" />
                    </td>
                    <th class="marginSet" align="center" colspan="6">
                        <span id="Span1" class="headerDisp" runat="server"></span>
                    </th>
                </tr>
                <tr>
                    <th class="marginSet" align="center" colspan="6">
                        <span id="Span2" class="headerDisp1" runat="server"></span>
                    </th>
                </tr>
                <tr>
                    <th class="marginSet" align="center" colspan="6">
                        <span id="Span3" class="headerDisp1" runat="server"></span>
                    </th>
                </tr>
                <tr>
                    <td class="marginSet" colspan="3" align="center">
                        <span id="Span4" class="headerDisp1" runat="server"></span>
                    </td>
                    <td class="marginSet" colspan="3" align="right">
                        <span id="Span5" class="headerDisp1" runat="server"></span>
                    </td>
                </tr>
                <tr>
                    <td class="marginSet" colspan="3" align="left">
                        <span id="Span6" class="headerDisp1" runat="server"></span>
                    </td>
                    <td class="marginSet" colspan="3" align="right">
                        <span id="Span7" class="headerDisp1" runat="server"></span>
                    </td>
                </tr>
            </table>
             

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
                    </td>
                </tr>
                <tr>
                    <td align="center">
                        <asp:Label ID="lblrptname" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                            Font-Size="Medium" Text="Report Name"></asp:Label>
                        <asp:TextBox ID="txtexcelname" runat="server" onkeypress="display()" Height="20px"
                            Width="180px" Style="font-family: 'Book Antiqua'" Font-Bold="True" Font-Names="Book Antiqua"
                            Font-Size="Medium"></asp:TextBox>
                        <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" runat="server" TargetControlID="txtexcelname"
                            FilterType="LowercaseLetters,UppercaseLetters,Numbers,Custom" ValidChars="!@$%^&*()_+|\}{][':;?><,./">
                        </asp:FilteredTextBoxExtender>
                        <asp:Button ID="btnxl" runat="server" Text="Export Excel" Font-Bold="True" Font-Names="Book Antiqua"
                            Font-Size="Medium" OnClick="btnxl_Click" />
                        <asp:Button ID="btnprintmaster" runat="server" Text="Print" OnClick="btnprintmaster_Click"
                            Font-Names="Book Antiqua" Font-Size="Medium" Font-Bold="true" />
                             <NEW:NEWPrintMater runat="server" ID="Printcontrol" Visible="false" />
                        

                        <button id="btnPrint" runat="server"  height="29px" width="62px" onclick="return printTTOutput();"
            style=" font-weight: bold; font-size: medium; font-family: Book Antiqua;">
            Direct Print
        </button>
                    </td>
                </tr>
            </table>
        </div>

         </ContentTemplate>
        <Triggers>
            <asp:PostBackTrigger ControlID="btnxl" />
            <asp:PostBackTrigger ControlID="btnGo" />
            <asp:PostBackTrigger ControlID="btnprintmaster" />
        </Triggers>
    </asp:UpdatePanel>
     

    </body>
    </html>
</asp:Content>

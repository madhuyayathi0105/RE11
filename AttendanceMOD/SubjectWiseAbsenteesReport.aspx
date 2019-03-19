<%@ Page Title="" Language="C#" MasterPageFile="~/AttendanceMOD/AttendanceSubSiteMaster.master"
    AutoEventWireup="true" CodeFile="SubjectWiseAbsenteesReport.aspx.cs" Inherits="SubjectWiseAbsenteesReport" %>

 <%@ Register Src="~/Usercontrols/NewPrintMaster.ascx" TagName="NEWPrintMater" TagPrefix="NEW" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Assembly="FarPoint.Web.Spread" Namespace="FarPoint.Web.Spread" TagPrefix="FarPoint" %>
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
    <script>
        function display() {
            document.getElementById('MainContent_errmsg').innerHTML = "";
        }
    </script>
    <asp:UpdatePanel ID="UpdatePanel3" runat="server">
            <ContentTemplate>
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <center>
        <br />
        <span class="fontstyleheader" style="color: Green;">Consolidate Absentees Report</span>
        <br />
        <br />
        <table class="maintablestyle">
            <tr>
                <td>
                    <asp:Label ID="lblbatch" runat="server" Text="Batch " Font-Bold="True" Font-Names="Book Antiqua"
                        Font-Size="Medium"></asp:Label>
                </td>
                <td>
                    <asp:DropDownList ID="ddlbatch" runat="server" Width="80px" Font-Bold="True" Font-Names="Book Antiqua"
                        Font-Size="Medium" AutoPostBack="true" OnSelectedIndexChanged="ddlbatch_SelectedIndexChanged">
                    </asp:DropDownList>
                </td>
                <td>
                    <asp:Label ID="lbldegree" runat="server" Text="Degree " Font-Bold="True" Font-Names="Book Antiqua"
                        Font-Size="Medium"></asp:Label>
                </td>
                <td>
                    <asp:DropDownList ID="ddldegree" runat="server" Font-Bold="True" Width="80" Font-Names="Book Antiqua"
                        Font-Size="Medium" AutoPostBack="true" OnSelectedIndexChanged="ddldegree_SelectedIndexChanged">
                    </asp:DropDownList>
                </td>
                <td>
                    <asp:Label ID="lblbranch" runat="server" Text="Branch " Font-Bold="True" Font-Names="Book Antiqua"
                        Font-Size="Medium"></asp:Label>
                </td>
                <td id="tdbranch" runat="server" colspan="2">
                    <asp:DropDownList ID="ddlbranch" runat="server" Width="220px" Font-Bold="True" Font-Names="Book Antiqua"
                        Font-Size="Medium" AutoPostBack="true" OnSelectedIndexChanged="ddlbranch_SelectedIndexChanged">
                    </asp:DropDownList>
                </td>
                <td>
                    <asp:Label ID="lblsem" runat="server" Text="Sem" Font-Bold="True" Font-Names="Book Antiqua"
                        Font-Size="Medium"></asp:Label>
                </td>
                <td>
                    <asp:DropDownList ID="ddlsem" runat="server" Width="50px" Font-Bold="True" Font-Names="Book Antiqua"
                        Font-Size="Medium" AutoPostBack="true" OnSelectedIndexChanged="ddlsem_SelectedIndexChanged">
                    </asp:DropDownList>
                </td>
                <td>
                    <asp:Label ID="lblsec" runat="server" Text="Sec " Font-Bold="True" Font-Names="Book Antiqua"
                        Font-Size="Medium"></asp:Label>
                </td>
                <td>
                    <asp:DropDownList ID="ddlsec" runat="server" Width="50px" Font-Bold="True" Font-Names="Book Antiqua"
                        Font-Size="Medium" AutoPostBack="true" OnSelectedIndexChanged="ddlsec_SelectedIndexChanged">
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Label ID="lblfdate" runat="server" Text="From Date " Font-Bold="True" Font-Names="Book Antiqua"
                        Font-Size="Medium"></asp:Label>
                </td>
                <td>
                    <asp:TextBox ID="txtfdate" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                        Font-Size="Medium" Width="80px" AutoPostBack="true" OnTextChanged="txtfdate_TextChanged"></asp:TextBox>
                    <asp:CalendarExtender ID="CalendarExtender1" TargetControlID="txtfdate" runat="server"
                        Format="dd/MM/yyyy">
                    </asp:CalendarExtender>
                </td>
                <td>
                    <asp:Label ID="lbltdate" runat="server" Text="To Date " Font-Bold="True" Font-Names="Book Antiqua"
                        Font-Size="Medium"></asp:Label>
                </td>
                <td>
                    <asp:TextBox ID="txttdate" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                        Font-Size="Medium" Width="80px" AutoPostBack="true" OnTextChanged="txttdate_TextChanged"></asp:TextBox>
                    <asp:CalendarExtender ID="CalendarExtender2" TargetControlID="txttdate" runat="server"
                        Format="dd/MM/yyyy">
                    </asp:CalendarExtender>
                </td>
                <td>
                
                    <asp:Button ID="btngo" runat="server" Text="Go" Font-Bold="True" Font-Names="Book Antiqua"
                        Font-Size="Medium" OnClick="btngo_Click" />
                        
                </td>
            </tr>
        </table>
        <br />
        <asp:Label ID="errmsg" runat="server" ForeColor="Red" Font-Bold="True" Font-Names="Book Antiqua"
            Font-Size="Medium"></asp:Label>
        <br />
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
        


        <br />
        <asp:Label ID="lblrptname" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
            Font-Size="Medium" Text="Report Name"></asp:Label>
        <asp:TextBox ID="txtexcelname" runat="server" Height="20px" Width="180px" Style="font-family: 'Book Antiqua'"
            Font-Bold="True" onkeypress="display()" Font-Names="Book Antiqua" Font-Size="Medium"></asp:TextBox>
        <asp:FilteredTextBoxExtender ID="Filterspace" runat="server" TargetControlID="txtexcelname"
            FilterType="LowercaseLetters,UppercaseLetters,Numbers,Custom" ValidChars="()}{][ ,/">
        </asp:FilteredTextBoxExtender>
        <asp:Button ID="btnxl" runat="server" Text="Export Excel" Font-Bold="True" Font-Names="Book Antiqua"
            Font-Size="Medium" OnClick="btnxl_Click" />
        <asp:Button ID="btnmasterprint" runat="server" Text="Print" Font-Bold="True" Font-Names="Book Antiqua"
            Font-Size="Medium" OnClick="btnmasterprint_Click" />
         <NEW:NEWPrintMater runat="server" ID="Printcontrol" Visible="false" />
        <button id="btnPrint" runat="server" height="29px" width="62px" onclick="return printTTOutput();"
            style=" font-weight: bold; font-size: medium; font-family: Book Antiqua;">
            Direct Print
        </button>

    </center>
    </ContentTemplate>
        <Triggers>
            <asp:PostBackTrigger ControlID="btnxl" />
            <asp:PostBackTrigger ControlID="btngo" />
            
        </Triggers>
    </asp:UpdatePanel>
    
</asp:Content>

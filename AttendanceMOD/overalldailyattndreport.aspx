<%@ Page Title="" Language="C#" MasterPageFile="~/AttendanceMOD/AttendanceSubSiteMaster.master"
    AutoEventWireup="true" CodeFile="overalldailyattndreport.aspx.cs" Inherits="ksrattndreport" %>

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
    <html>
     <asp:UpdatePanel ID="UpdatePanel3" runat="server">
            <ContentTemplate>
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <br />
    <center>
        <span class="fontstyleheader" style="color: Green;">AT06-Overall Daily Attendance Report</span>
    </center>
    <br />
    <script type="text/javascript">
        function display() {
            document.getElementById('MainContent_lblerr').innerHTML = "";
        }
    </script>
    <div style="height: 319px; width: 1000px">
        <center>
            <div class="maintablestyle" style="width: 800px;">
                <asp:Label ID="Label4" runat="server" Text="College" Font-Bold="True" Font-Names="Book Antiqua"
                    Font-Size="Medium"></asp:Label>
                <asp:DropDownList ID="ddlcollege" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                    Font-Size="Medium" Width="400px" OnSelectedIndexChanged="ddlcollege_SelectedIndexChanged"
                    AutoPostBack="True">
                </asp:DropDownList>
                <asp:Label ID="date_lbl" runat="server" Text="Date" Font-Bold="True" Font-Names="Book Antiqua"
                    Font-Size="Medium"></asp:Label>
                <asp:TextBox ID="datetxt" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                    Font-Size="Medium" Height="18px" Width="79px" OnTextChanged="datetxt_TextChanged"
                    AutoPostBack="True"></asp:TextBox>
                &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;

                 <asp:UpdatePanel ID="btngoUpdatePanel" runat="server">
                                <ContentTemplate>
                <asp:Button ID="gobtn" runat="server" Text="Go" Font-Bold="True" Font-Names="Book Antiqua"
                    Font-Size="Medium" Height="29px" OnClick="gobtn_Click" />
                    </ContentTemplate>
                </asp:UpdatePanel>
                <asp:FilteredTextBoxExtender ID="datefilter" FilterType="Custom,Numbers" ValidChars="/"
                    runat="server" TargetControlID="datetxt">
                </asp:FilteredTextBoxExtender>
                <asp:CalendarExtender ID="dateext" TargetControlID="datetxt" Format="dd/MM/yyyy"
                    runat="server">
                </asp:CalendarExtender>
                <asp:Label ID="datelbl" runat="server" ForeColor="Red" Font-Bold="True" Font-Names="Book Antiqua"
                    Font-Size="Medium"></asp:Label>
                <asp:Label ID="lblpages" runat="server" Font-Bold="true" Font-Names="Book Antiqua"
                    Font-Size="Medium" Text="Page"></asp:Label>
                <asp:DropDownList ID="ddlpage" runat="server" AutoPostBack="True" Font-Bold="true"
                    Font-Names="Book Antiqua" Font-Size="Medium" Height="21px" OnSelectedIndexChanged="ddlpage_SelectedIndexChanged"
                    Width="47px">
                </asp:DropDownList>
            </div>
        </center>
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
                    <asp:Label ID="errlbl" runat="server" ForeColor="Red" Font-Bold="True" Font-Names="Book Antiqua"
                        Font-Size="Medium"></asp:Label>
                    <asp:Panel ID="Panel3" runat="server" BorderStyle="None" Width="1022px">
                        <asp:Label ID="Buttontotal" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                            Font-Size="Medium"></asp:Label>
                        &nbsp;&nbsp;
                        <asp:Label ID="ddlpagelbl" runat="server" Font-Bold="True" Text="     Records Per Page:"
                            Font-Names="Book Antiqua" Font-Size="Medium"></asp:Label>
                        <asp:DropDownList ID="DropDownListpage" runat="server" Height="25px" Width="65px"
                            Font-Bold="True" Font-Size="Medium" Font-Names="Book Antiqua" OnSelectedIndexChanged="DropDownListpage_SelectedIndexChanged"
                            AutoPostBack="True">
                        </asp:DropDownList>
                        <asp:TextBox ID="pageddltxt" runat="server" Height="21px" Width="40px" Font-Bold="True"
                            Font-Size="Medium" Font-Names="Book Antiqua" 
                            AutoPostBack="True"></asp:TextBox>
                        <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender3" runat="server" FilterType="Numbers"
                            TargetControlID="pageddltxt">
                        </asp:FilteredTextBoxExtender>
                        <asp:Label ID="pgsearch_lbl" runat="server" Font-Bold="True" Text="Page Search:"
                            Width="95px" Font-Names="Book Antiqua" Font-Size="Medium" Height="24px"></asp:Label>
                        <asp:TextBox ID="pagesearch_txt" runat="server" AutoPostBack="True" Font-Bold="True"
                            Font-Names="Book Antiqua" Font-Size="Medium" Height="21px" Width="40px" OnTextChanged="pagesearch_txt_TextChanged"></asp:TextBox>
                        <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender2" runat="server" TargetControlID="pagesearch_txt"
                            FilterType="Numbers">
                        </asp:FilteredTextBoxExtender>
                        <asp:Label ID="errmsg" runat="server" ForeColor="Red" Font-Bold="True" Font-Names="Book Antiqua"
                            Font-Size="Medium" Height="21px" Width="385px"></asp:Label>
                    </asp:Panel>
                </td>
            </tr>
            <tr>
                <td>


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
                    
                </td>
            </tr>
            <tr>
                <td align="center">
                    <br />
                    <asp:Label ID="lblerr" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                        Font-Size="Medium" Text="" ForeColor="Red"></asp:Label>
                    <asp:Label ID="lblrptname" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                        Font-Size="Medium" Text="Report Name"></asp:Label>
                    <asp:TextBox ID="txtexcelname" runat="server" Height="20px" Width="180px" Style="font-family: 'Book Antiqua'"
                        Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium" onkeypress="display()"></asp:TextBox>
                    <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" runat="server" TargetControlID="txtexcelname"
                        FilterType="LowercaseLetters,UppercaseLetters,Numbers,Custom" ValidChars="()_-.">
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

    </html>
</asp:Content>

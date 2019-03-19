<%@ Page Title="" Language="C#" MasterPageFile="~/AttendanceMOD/AttendanceSubSiteMaster.master"
    AutoEventWireup="true" CodeFile="monthattndreport.aspx.cs" Inherits="NewAttendance"
    EnableEventValidation="false" %>

<%@ Register Src="~/Usercontrols/NewPrintMaster.ascx" TagName="NEWPrintMater" TagPrefix="NEW" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Src="~/Usercontrols/PrintMaster.ascx" TagName="PRINTPDF" TagPrefix="Insproplus" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <link href="~/Styles/css/Registration.css" rel="stylesheet" type="text/css" />
    <script src="//ajax.googleapis.com/ajax/libs/jquery/1.11.1/jquery.min.js"></script>
    <link href="~/Styles/css/Commoncss.css" rel="Stylesheet" type="text/css" />
    
    <style type="text/css">  
        .cursorptr
        {
            cursor: pointer;
        }
        .cursordflt
        {
            cursor: default;
        }
        
        #clsbtn
        {
            height: 26px;
            width: 72px;
        }
        
        .txt
        {
        }
        
        .style16
        {
            width: 79px;
            height: 24px;
        }
        .style17
        {
            width: 64px;
            height: 24px;
        }
        
        .style22
        {
            width: 190px;
        }
        .style23
        {
            width: 343px;
        }
        .style25
        {
            width: 110px;
        }
        
        .style29
        {
            width: 83px;
            height: 24px;
        }
        .style33
        {
            width: 147px;
            height: 24px;
        }
        .style36
        {
            width: 87px;
            height: 24px;
        }
        
        .style39
        {
            width: 62px;
            height: 62px;
        }
        .style40
        {
            height: 24px;
        }
        
        .style41
        {
            width: 97px;
        }
        .style42
        {
            width: 81px;
        }
        
        .style43
        {
            width: 78px;
            height: 24px;
        }
    </style>
    
   
    

    <script type="text/javascript">
        function display() {
            document.getElementById('MainContent_norecordlbl').innerHTML = "";
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
        <span class="fontstyleheader" style="color: Green; text-align: center; margin: 0px;
            margin-bottom: 15px; margin-top: 10px; position: relative;">AT03 - Monthly Student
            Attendance Report</span>
    </center>
    <div>
        <center>
            <div style="width: auto; height:auto; margin: 0px; margin-bottom: 15px;
                margin-top: 15px; position: relative; padding: 3px;">
                <table class="maintablestyle" style="height: auto; width: auto;">
                    <tr>
                        <td>
                            <asp:Label runat="server" ID="lblbatch" Text="Batch" Font-Bold="True" Font-Names="Book Antiqua"
                                Font-Size="Medium"></asp:Label>
                        </td>
                        <td>
                            <asp:DropDownList ID="ddlbatch" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                Font-Size="Medium" OnSelectedIndexChanged="ddlbatch_SelectedIndexChanged" Height="25px"
                                Width="60px" AutoPostBack="True">
                            </asp:DropDownList>
                        </td>
                        <td>
                            <asp:Label runat="server" ID="lbldegree" Text="Degree" Font-Bold="True" Font-Names="Book Antiqua"
                                Font-Size="Medium"></asp:Label>
                        </td>
                        <td>
                            <asp:DropDownList runat="server" ID="ddldegree" Height="24px" Width="80px" AutoPostBack="True"
                                Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium" OnSelectedIndexChanged="ddldegree_SelectedIndexChanged">
                            </asp:DropDownList>
                        </td>
                        <td>
                            <asp:Label runat="server" ID="lblbranch" Text="Branch" Font-Bold="True" Font-Names="Book Antiqua"
                                Font-Size="Medium"></asp:Label>
                        </td>
                        <td>
                            <asp:DropDownList runat="server" ID="ddlbranch" Font-Bold="True" Height="25px" Width="200px"
                                Font-Names="Book Antiqua" Font-Size="Medium" OnSelectedIndexChanged="ddlbranch_SelectedIndexChanged"
                                AutoPostBack="True">
                            </asp:DropDownList>
                        </td>
                        <td>
                            <asp:Label runat="server" ID="lblduration" Text="Sem" Font-Bold="True" Font-Names="Book Antiqua"
                                Font-Size="Medium"></asp:Label>
                        </td>
                        <td>
                            <asp:DropDownList runat="server" ID="ddlduration" Font-Bold="True" Font-Names="Book Antiqua"
                                Font-Size="Medium" AutoPostBack="True" Height="25px" Width="47px" OnSelectedIndexChanged="ddlduration_SelectedIndexChanged">
                            </asp:DropDownList>
                        </td>
                        <td>
                            <asp:Label runat="server" ID="lblsec" Text="Sec" Font-Bold="True" Font-Names="Book Antiqua"
                                Font-Size="Medium"></asp:Label>
                        </td>
                        <td>
                            <asp:DropDownList runat="server" ID="ddlsec" Font-Bold="True" Font-Names="Book Antiqua"
                                Font-Size="Medium" Height="25px" Width="49px" AutoPostBack="True" OnSelectedIndexChanged="ddlsec_SelectedIndexChanged">
                            </asp:DropDownList>
                        </td>
                        <td>
                            <asp:CheckBoxList ID="searchcheck" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                Font-Size="Medium" RepeatDirection="Horizontal">
                                <asp:ListItem Value="hour">HourWise</asp:ListItem>
                                <asp:ListItem Value="day">DayWise</asp:ListItem>
                            </asp:CheckBoxList>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="11">
                            <table>
                                <tr>
                                    <td>
                                        <asp:Label ID="lblFromdate" runat="server" Text="From Date" Font-Bold="True" Font-Names="Book Antiqua"
                                            Font-Size="Medium"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtFromDate" CssClass="txt" runat="server" Height="25px" Width="75px"
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
                                        <asp:Label ID="lbltodate" runat="server" Text="To Date" Font-Bold="True" Font-Names="Book Antiqua"
                                            Font-Size="Medium"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtToDate" CssClass="txt" runat="server" Height="25px" Width="75px"
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
                                    
                                        <asp:Button ID="btnGo" runat="server" Text="Go" OnClick="btnGo_Click" Font-Bold="True"
                                            Font-Names="Book Antiqua" Font-Size="Medium" Style="width: auto; height: auto;" />

                                            
                                    </td>
                                    <td>
                                        <asp:CheckBox ID="cumcheck" runat="server" Text="Cummulative %" Font-Bold="True"
                                            Font-Names="Book Antiqua" Font-Size="Medium" AutoPostBack="True" OnCheckedChanged="cumcheck_CheckedChanged" />
                                    </td>
                                    <td>
                                        <asp:Label ID="cumfromlbl" runat="server" Text="From Date" Font-Bold="True" Font-Names="Book Antiqua"
                                            Font-Size="Medium"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="cumfromtxt" CssClass="txt" runat="server" Height="25px" Width="75px"
                                            Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium" OnTextChanged="cumfromtxt_TextChanged"
                                            AutoPostBack="True"></asp:TextBox>
                                        <asp:FilteredTextBoxExtender ID="cumfromfilt" FilterType="Custom,Numbers" ValidChars="/"
                                            runat="server" TargetControlID="cumfromtxt">
                                        </asp:FilteredTextBoxExtender>
                                        <asp:CalendarExtender ID="cumfromcal" TargetControlID="cumfromtxt" Format="d/MM/yyyy"
                                            runat="server">
                                        </asp:CalendarExtender>
                                    </td>
                                    <td>
                                        <asp:Label ID="cumtolbl" runat="server" Text="To Date" Font-Bold="True" Font-Names="Book Antiqua"
                                            Font-Size="Medium"></asp:Label>
                                        <asp:FilteredTextBoxExtender ID="cumtofilt" FilterType="Custom,Numbers" ValidChars="/"
                                            runat="server" TargetControlID="cumtotxt">
                                        </asp:FilteredTextBoxExtender>
                                        <asp:CalendarExtender ID="cumtocal" TargetControlID="cumtotxt" Format="d/MM/yyyy"
                                            runat="server">
                                        </asp:CalendarExtender>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="cumtotxt" CssClass="txt" runat="server" Height="25px" Width="83px"
                                            Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium" OnTextChanged="cumtotxt_TextChanged"
                                            AutoPostBack="True"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:CheckBox ID="pointchk" runat="server" Text="Att Point" Font-Bold="True" Font-Names="Book Antiqua"
                                            Font-Size="Medium" />
                                    </td>
                                    <td>
                                    </td>
                                </tr>
                                 <tr>
                                    <td colspan="2">
                                        <asp:CheckBox ID="cbincround" runat="server" Text="Include Round" Font-Bold="True"
                                            Font-Names="Book Antiqua" Font-Size="Medium" />
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="11">
                            <table>
                                <tr>
                                    <td colspan="2">
                                        <asp:Label ID="frmlbl" runat="server" Text="Select From Date" ForeColor="Red" Font-Bold="True"
                                            Font-Names="Book Antiqua"></asp:Label>
                                    </td>
                                    <td colspan="2">
                                        <asp:Label ID="tolbl" runat="server" Text="Select To Date" ForeColor="Red" Font-Bold="True"
                                            Font-Names="Book Antiqua"></asp:Label>
                                    </td>
                                    <td colspan="8">
                                        <asp:Label ID="tofromlbl" runat="server" Text="From date should not be greater than To date"
                                            ForeColor="Red" Font-Bold="True" Font-Names="Book Antiqua"></asp:Label>
                                        <asp:Button ID="btn_print_setting" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                            Text="Print Master Setting" Visible="False" OnClick="btn_print_setting_Click"
                                            Font-Size="Medium" />
                                        <asp:Label ID="lablepage" runat="server" Font-Names="Book Antiqua" Font-Size="Medium"
                                            Font-Bold="true" Text="Page"></asp:Label>
                                        <asp:DropDownList ID="ddlpage_new" runat="server" Font-Names="Book Antiqua" Font-Size="Medium"
                                            Font-Bold="true" OnSelectedIndexChanged="ddlpage_new_SelectedIndexChanged" AutoPostBack="true">
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                </table>
            </div>
        </center>
        <table style="width: auto; margin: 0px; margin-bottom: 15px; margin-top: 15px; position: relative;">
            <tr>
                <td align="left">
                    <asp:Label ID="norecordlbl" runat="server" Font-Bold="true" Font-Size="Medium" ForeColor="Red"></asp:Label>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Panel ID="Panel3" runat="server" BorderStyle="None" Width="949px" Style="margin: 0px;
                        margin-bottom: 10px; margin-top: 10px; position: relative;">
                        <table>
                            <tr>
                                <td>
                                    <asp:Label ID="Buttontotal" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                        Font-Size="Medium"></asp:Label>
                                </td>
                                <td>
                                    <asp:Label ID="ddlpagelbl" runat="server" Font-Bold="True" Text="     Records Per Page"
                                        Font-Names="Book Antiqua" Font-Size="Medium"></asp:Label>
                                </td>
                                <td>
                                    <asp:DropDownList ID="DropDownListpage" runat="server" Height="25px" Width="65px"
                                        Font-Bold="True" Font-Size="Medium" Font-Names="Book Antiqua" OnSelectedIndexChanged="DropDownListpage_SelectedIndexChanged"
                                        AutoPostBack="True">
                                    </asp:DropDownList>
                                </td>
                                <td>
                                    <asp:TextBox ID="pageddltxt" runat="server" Height="22px" Width="40px" Font-Bold="True"
                                        Font-Size="Medium" Font-Names="Book Antiqua" OnTextChanged="pageddltxt_TextChanged"
                                        AutoPostBack="True"></asp:TextBox>
                                    <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender3" runat="server" FilterType="Numbers"
                                        TargetControlID="pageddltxt">
                                    </asp:FilteredTextBoxExtender>
                                </td>
                                <td>
                                    <asp:Label ID="pgsearch_lbl" runat="server" Font-Bold="True" Text="Page Search:"
                                        Width="95px" Font-Names="Book Antiqua" Font-Size="Medium" Height="24px"></asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox ID="pagesearch_txt" runat="server" AutoPostBack="True" Font-Bold="True"
                                        Font-Names="Book Antiqua" Font-Size="Medium" Height="22px" Width="40px" OnTextChanged="pagesearch_txt_TextChanged"></asp:TextBox>
                                    <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender2" runat="server" TargetControlID="pagesearch_txt"
                                        FilterType="Numbers">
                                    </asp:FilteredTextBoxExtender>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="6">
                                    <asp:Label ID="errmsg" runat="server" ForeColor="Red" Font-Bold="True" Font-Names="Book Antiqua"
                                        Font-Size="Medium" Height="16px" Width="429px"></asp:Label>
                                </td>
                            </tr>
                        </table>
                    </asp:Panel>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Panel ID="pagesetpanel" runat="server">
                        <asp:RadioButton ID="RadioHeader" runat="server" Font-Names="Book Antiqua" Font-Size="Medium"
                            Font-Bold="true" Text="Header in All Pages" OnCheckedChanged="RadioHeader_CheckedChanged"
                            GroupName="header" AutoPostBack="True" />
                        <asp:RadioButton ID="Radiowithoutheader" runat="server" OnCheckedChanged="Radiowithoutheader_CheckedChanged"
                            Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium" GroupName="header"
                            Text="Header in 1st Page" AutoPostBack="True" />
                        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                        <asp:Label ID="lblpages" runat="server" Text="Page" Font-Names="Book Antiqua" Font-Size="Medium"
                            Font-Bold="true"></asp:Label>
                        <asp:DropDownList ID="ddlpage" runat="server" OnSelectedIndexChanged="ddlpage_SelectedIndexChanged"
                            Font-Names="Book Antiqua" Font-Size="Medium" Font-Bold="true" AutoPostBack="True"
                            Width="48px">
                        </asp:DropDownList>
                    </asp:Panel>
                </td>
            </tr>
        </table>
        <center>
            <table style="margin: 0px; margin-bottom: 20px; margin-top: 15px; position: relative;">
                <tr>
                    <td>
                        <div runat="server" id="divNote" visible="false">
                            <table style="width: auto; height: auto; margin: 0px; margin-bottom: 10px; margin-top: 10px;">
                                <tr>
                                    <td style="width: 10px; padding: 5px; background-color: #035523;">
                                    </td>
                                    <td style="width: auto; padding-left: 3px; padding-right: 3px;">
                                        -
                                    </td>
                                    <td style="width: auto; padding-left: 3px; padding-right: 10px;">
                                        P - Present
                                    </td>
                                    <td style="width: 10px; padding: 5px; background-color: #F21C03;">
                                    </td>
                                    <td style="width: auto; padding-left: 3px; padding-right: 3px;">
                                        -
                                    </td>
                                    <td style="width: auto; padding-left: 3px; padding-right: 10px;">
                                        A - Absent
                                    </td>
                                    <td style="width: 10px; padding: 5px; background-color: #3000D3;">
                                    </td>
                                    <td style="width: auto; padding-left: 3px; padding-right: 3px;">
                                        -
                                    </td>
                                    <td style="width: auto; padding-left: 3px; padding-right: 10px;">
                                        OD -Onduty
                                    </td>
                                    <td style="width: 10px; padding: 5px; background-color: #000000;">
                                    </td>
                                    <td style="width: auto; padding-left: 3px; padding-right: 3px;">
                                        -
                                    </td>
                                    <td style="width: auto; padding-left: 3px; padding-right: 10px;">
                                        NE -Not Enter
                                    </td>
                                    <td style="width: 10px; padding: 5px; background-color: #9057C3;">
                                    </td>
                                    <td style="width: auto; padding-left: 3px; padding-right: 3px;">
                                        -
                                    </td>
                                    <td style="width: auto; padding-left: 3px; padding-right: 10px;">
                                        NJ - Not Join
                                    </td>
                                    <td style="width: 10px; padding: 5px; background-color: #C41D9E;">
                                    </td>
                                    <td style="width: auto; padding-left: 3px; padding-right: 3px;">
                                        -
                                    </td>
                                    <td style="width: auto; padding-left: 3px; padding-right: 10px;">
                                        H - Holiday
                                    </td>
                                </tr>
                            </table>
                        </div>
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
                         <div id="divgrid" style="width: 900px; height: 500px; overflow: scroll">
                            <asp:GridView ID="Showgrid" runat="server"  Visible="false" HeaderStyle-ForeColor="Black"
                        HeaderStyle-BackColor="#0CA6CA"   CssClass="myGrid" AlternatingRowStyle-CssClass="alt">
                            </asp:GridView>

                            </div>
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
                    <td align="center" style="margin: 0px; margin-bottom: 20px; margin-top: 15px; position: relative;">
                        <asp:Label ID="lblrptname" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                            Font-Size="Medium" Text="Report Name"></asp:Label>
                        <asp:TextBox ID="txtexcelname" runat="server" onkeypress="display()" Height="20px"
                            Width="180px" Style="font-family: 'Book Antiqua'" Font-Bold="True" Font-Names="Book Antiqua"
                            Font-Size="Medium"></asp:TextBox>
                        <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender4" runat="server" TargetControlID="txtexcelname"
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
                        <asp:CheckBox ID="cbhourwise" runat="server" Text="Hourwise" Font-Bold="True" Font-Names="Book Antiqua"
                            Font-Size="Medium" Visible="False" />
                    </td>
                </tr>
                <tr>
                    <td align="left">
                        <asp:Panel runat="server" ID="ne">
                            <asp:Label ID="Label1" runat="server" Text="NE" BackColor="AliceBlue" ForeColor="DarkRed"></asp:Label>&nbsp;&nbsp;&nbsp;
                            <asp:Label ID="Label2" runat="server" Text="No Entry"></asp:Label>
                        </asp:Panel>
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
    </div>

    </ContentTemplate>
        <Triggers>
            <asp:PostBackTrigger ControlID="btnxl" />
            <asp:PostBackTrigger ControlID="btnGo" />
        </Triggers>
    </asp:UpdatePanel>


</asp:Content>

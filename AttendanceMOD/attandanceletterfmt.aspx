<%@ Page Title="" Language="C#" MasterPageFile="~/AttendanceMOD/AttendanceSubSiteMaster.master"
    AutoEventWireup="true" CodeFile="attandanceletterfmt.aspx.cs" Inherits="attandance_letterfmt"
    EnableEventValidation="false" %>

<%@ Register Src="~/Usercontrols/NewPrintMaster.ascx" TagName="NEWPrintMater" TagPrefix="NEW" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Src="~/Usercontrols/PrintMaster.ascx" TagName="PRINTPDF" TagPrefix="Insproplus" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    
<script type="text/javascript">
    function PrintDiv() {
        var panel = document.getElementById("<%=contentDiv.ClientID %>");
        var printWindow = window.open('', '', 'height=auto,width=1191');
        printWindow.document.write('<html');
        printWindow.document.write('<head> <style type="text/css"> p{ font-size: x-small;margin: 0px; padding: 0px; border: 0px;  } body{ margin:0px;}</style>');
        printWindow.document.write('</head><body>');
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
                document.getElementById('MainContent_lblnorec').innerHTML = "";
            }
        </script>
        <asp:UpdatePanel ID="UpdatePanel3" runat="server">
            <ContentTemplate>
        <br />
        <center>
            <span class="fontstyleheader" style="color: Green;">Attendance Letter Report</span>
        </center>
        <br />
        <div style="width: 1072px">
            <asp:ScriptManager ID="ScriptManager1" runat="server">
            </asp:ScriptManager>
            <center>
                <table class="maintablestyle">
                    <tr>
                        <td class="style2">
                            <asp:Label ID="Label1" runat="server" Text="Batch" Font-Bold="True" Font-Names="Book Antiqua"
                                Font-Size="Medium"></asp:Label>
                        </td>
                        <td class="style19">
                            <asp:DropDownList ID="ddlbatch" runat="server" OnSelectedIndexChanged="ddlbatch_SelectedIndexChanged"
                                AutoPostBack="True" Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium"
                                Height="25px" Width="69px">
                            </asp:DropDownList>
                        </td>
                        <td class="style3">
                            <asp:Label ID="Label2" runat="server" Text="Degree" Font-Bold="True" Font-Names="Book Antiqua"
                                Font-Size="Medium"></asp:Label>
                        </td>
                        <td class="style21">
                            <asp:DropDownList ID="ddldegree" runat="server" OnSelectedIndexChanged="ddldegree_SelectedIndexChanged"
                                AutoPostBack="True" Font-Bold="True" Width="106px" Font-Names="Book Antiqua"
                                Font-Size="Medium" Height="25px">
                            </asp:DropDownList>
                        </td>
                        <td class="style3">
                            <asp:Label ID="lblbranch" runat="server" Text="Branch" Font-Bold="True" Font-Names="Book Antiqua"
                                Font-Size="Medium"></asp:Label>
                        </td>
                        <td class="style4">
                            <asp:DropDownList ID="ddlbranch" runat="server" OnSelectedIndexChanged="ddlbranch_SelectedIndexChanged"
                                AutoPostBack="True" Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium"
                                Height="25px" Width="271px">
                            </asp:DropDownList>
                        </td>
                        <td class="style6">
                            <asp:Label ID="lblsem" runat="server" Text="Sem" Font-Bold="True" Font-Names="Book Antiqua"
                                Font-Size="Medium"></asp:Label>
                        </td>
                        <td class="style20">
                            <asp:DropDownList ID="ddlsemester" runat="server" OnSelectedIndexChanged="ddlsemester_SelectedIndexChanged"
                                AutoPostBack="True" Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium"
                                Height="25px" Width="32px">
                            </asp:DropDownList>
                        </td>
                        <td class="style8">
                            <asp:Label ID="lblsec" runat="server" Text="Sec" Font-Bold="True" Font-Names="Book Antiqua"
                                Font-Size="Medium"></asp:Label>
                        </td>
                        <td>
                            <asp:DropDownList ID="ddlsection" runat="server" AutoPostBack="True" Font-Bold="True"
                                Font-Names="Book Antiqua" Font-Size="Medium" Height="25px" Width="52px" OnSelectedIndexChanged="ddlsection_SelectedIndexChanged">
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="12">
                            <asp:Label ID="Label6" runat="server" Text="From" Font-Bold="True" Font-Names="Book Antiqua"
                                Font-Size="Medium"></asp:Label>
                            <asp:TextBox ID="txtfromdate" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                Font-Size="Medium" Height="24px" Width="75px" OnTextChanged="txtfromdate_TextChanged"
                                AutoPostBack="True"></asp:TextBox>
                            <asp:CalendarExtender ID="CalendarExtender1" Format="d/MM/yyyy" TargetControlID="txtfromdate"
                                runat="server">
                            </asp:CalendarExtender>
                            <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender4" runat="server" TargetControlID="txtfromdate"
                                FilterType="Numbers,Custom" ValidChars="/">
                            </asp:FilteredTextBoxExtender>
                            <asp:Label ID="Label3" runat="server" Text="To" Font-Bold="True" Font-Names="Book Antiqua"
                                Font-Size="Medium"></asp:Label>
                            <asp:TextBox ID="txttodate" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                Font-Size="Medium" Height="24px" Width="75px" OnTextChanged="txttodate_TextChanged"
                                AutoPostBack="True"></asp:TextBox>
                            <asp:CalendarExtender ID="CalendarExtender2" Format="d/MM/yyyy" TargetControlID="txttodate"
                                runat="server">
                            </asp:CalendarExtender>
                            <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender5" runat="server" TargetControlID="txttodate"
                                FilterType="Numbers,Custom" ValidChars="/">
                            </asp:FilteredTextBoxExtender>
                            <asp:Label ID="lblfrom" runat="server" Text="Range From" Font-Bold="True" Font-Names="Book Antiqua"
                                Font-Size="Medium"></asp:Label>
                            <asp:TextBox ID="txtfromrange" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                Font-Size="Medium" AutoPostBack="true" OnTextChanged="txtfromrange_TextChanged"
                                MaxLength="3" Width="40px"></asp:TextBox>
                            <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" runat="server" TargetControlID="txtfromrange"
                                FilterType="Numbers" />
                            <asp:Label ID="lbltorange" runat="server" Text="To" Font-Bold="True" Font-Names="Book Antiqua"
                                Font-Size="Medium"></asp:Label>
                            <asp:TextBox ID="txttorange" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                Font-Size="Medium" AutoPostBack="true" OnTextChanged="txttorange_TextChanged"
                                MaxLength="3" Width="40px"></asp:TextBox>
                            <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender3" runat="server" TargetControlID="txttorange"
                                FilterType="Numbers" />
                            <asp:Button ID="btngo" runat="server" Text="Go" OnClick="btngo_Click" Font-Bold="True"
                                Font-Names="Book Antiqua" Font-Size="Medium" />
                        </td>
                    </tr>
                </table>
                <br />
                <asp:Label ID="dateerr" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                    Font-Size="Medium" ForeColor="Red"></asp:Label>
                <asp:Label ID="lblnorec" runat="server" Text="No Record(s) Found" ForeColor="Red"
                    Visible="False" Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium"></asp:Label>
                <br />
                <br />
                 <center>
                            <asp:Panel ID="Pnltamilformat" runat="server" Height="200px" BackColor="Lightblue"
                                BorderColor="black" BorderWidth="1px" BorderStyle="Solid" Visible="false" ClientIDMode="Static"
                                Width="769px" Style="margin-bottom: 66px">
                                <table>
                                    <tr>
                                        <td>
                                            <asp:Label ID="lbltamilnote" runat="server" Font-Bold="true"
                                                Font-Names="Book Antiqua" Text="Enter Note" Font-Size="Medium"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txttamilnote" TextMode="MultiLine" runat="server" Font-Bold="true" Font-Names="Book Antiqua"
                                                Font-Size="Medium" Height="150px" Width="400"></asp:TextBox>

                                                
                                        </td>

                                        <td>
                                            <asp:Button ID="btnnotesave" runat="server" Text="Save" OnClick="btnnotesave_Click"                                                               CssClass="textbox btn1" Font-Names="Book Antiqua" Font-Size="Medium" 
                                                         Font-Bold="true" Width="100"/>
                                        </td>
                                        <td>
                                            <asp:Button ID="btntamilprint" runat="server" Text="Print" OnClick="btntamilprint_Click"                                                               CssClass="textbox btn1" Font-Names="Book Antiqua" Font-Size="Medium" 
                                                        Font-Bold="true" Width="100" />
                                        </td>
                                    </tr>
                                    <tr>
                                    <td colspan="4" style="text-align: center;">
                                            <asp:Label ID="lblsave" runat="server" Visible="false" Font-Bold="true"
                                                Font-Names="Book Antiqua" ForeColor="Red" Font-Size="Medium"></asp:Label>
                                        </td>
                                    </tr>
                                </table>
                            </asp:Panel>
                           
                        </center>
                <br />
               


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
                          
                            <asp:GridView ID="grdover" runat="server" Width="500px" BorderStyle="Double" Font-Bold="true"
                            Font-Names="Book Antiqua" Font-Size="Medium" GridLines="Both" CellPadding="4"  
                             ShowFooter="false" ShowHeader="true">
                            <Columns>
                            <asp:TemplateField HeaderText="Select" HeaderStyle-BackColor="#0CA6CA" HeaderStyle-HorizontalAlign="center"
                            HeaderStyle-Width="30px">
                            <ItemTemplate>
                                <center>
                                    <asp:CheckBox ID="chkselectall" runat="server" Width="30px" AutoPostBack="true" OnCheckedChanged="chkselectall_CheckedChanged"></asp:CheckBox>
                                    <asp:CheckBox ID="lbl_cb" runat="server" Width="30px" ></asp:CheckBox>
                                </center>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="right" />
                        </asp:TemplateField>
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
                           
                    
                </center>
                <br />
                <asp:Button ID="btnletter" runat="server" Text="Letter Report" Font-Bold="True" Font-Names="Book Antiqua"
                    Font-Size="Medium" OnClick="btnletter_Click" Visible="False" />
                <asp:Button ID="btntamilletter" runat="server" Text="Tamil Letter Report" Font-Bold="True" Font-Names="Book Antiqua"
                    Font-Size="Medium" OnClick="btntamilletter_Click" Visible="False" />
                <asp:Label ID="lblrptname" runat="server" Visible="False" Font-Bold="True" Font-Names="Book Antiqua"
                    Font-Size="Medium" Text="Report Name"></asp:Label>
                <asp:TextBox ID="txtexcelname" Visible="False" runat="server" Height="20px" Width="180px"
                    Style="font-family: 'Book Antiqua'" Font-Bold="True" Font-Names="Book Antiqua"
                    Font-Size="Medium" onkeypress="display()"></asp:TextBox>
                <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender2" runat="server" TargetControlID="txtexcelname"
                    FilterType="LowercaseLetters,UppercaseLetters,Numbers,Custom" ValidChars="!@$%^&*()_+|}{][':;?><,."
                    InvalidChars="/\">
                </asp:FilteredTextBoxExtender>
                <asp:Button ID="btnxl" runat="server" Visible="False" Text="Export Excel" Font-Bold="True"
                    Font-Names="Book Antiqua" Font-Size="Medium" OnClick="btnxl_Click" />
                <asp:Button ID="btnprintmaster" runat="server" Visible="False" Text="Print" OnClick="btnprintmaster_Click"
                    Font-Names="Book Antiqua" Font-Size="Medium" Font-Bold="true" />
                     <NEW:NEWPrintMater runat="server" ID="Printcontrol" Visible="false" />
                

                <button id="btnPrint" runat="server" Visible="False"  height="29px" width="62px" onclick="return printTTOutput();"
            style=" font-weight: bold; font-size: medium; font-family: Book Antiqua;">
            Direct Print
        </button>
                <asp:CheckBox ID="cbhourwise" runat="server" Text="Hourwise" Font-Bold="True" Font-Names="Book Antiqua"
                    Font-Size="Medium" Visible="False" />
                <asp:CheckBox ID="cbdaywise" runat="server" Text="Daywise" Font-Bold="True" Font-Names="Book Antiqua"
                    Font-Size="Medium" Visible="False" />
            </center>
        </div>
         <div style="height: 1px; width: 1px; overflow: auto;">
        <div id="contentDiv" runat="server" style="height: auto; width: 1344px;" visible="false">
        </div>
    </div>

    </ContentTemplate>
        <Triggers>
            <asp:PostBackTrigger ControlID="btnxl" />
            <asp:PostBackTrigger ControlID="btnGo" />
            <asp:PostBackTrigger ControlID="btntamilprint" />
            <asp:PostBackTrigger ControlID="btnnotesave" />
            <asp:PostBackTrigger ControlID="btntamilletter" />
            <asp:PostBackTrigger ControlID="btnletter" />
            <asp:PostBackTrigger ControlID="btnprintmaster" />
            
        </Triggers>
    </asp:UpdatePanel>
    </body>
    </html>
</asp:Content>

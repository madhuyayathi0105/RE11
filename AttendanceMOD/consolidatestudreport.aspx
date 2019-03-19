<%@ Page Title="" Language="C#" MasterPageFile="~/AttendanceMOD/AttendanceSubSiteMaster.master"
    AutoEventWireup="true" CodeFile="consolidatestudreport.aspx.cs" Inherits="NewAttendance" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Assembly="FarPoint.Web.Spread" Namespace="FarPoint.Web.Spread" TagPrefix="FarPoint" %>
<%@ Register Src="~/Usercontrols/PrintMaster.ascx" TagName="PRINTPDF" TagPrefix="Insproplus" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">
    <!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN">
    <html>
    <style type="text/css">
        .style1
        {
            width: 45%;
        }
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
        
        .style19
        {
            width: 106px;
        }
        
        .style22
        {
            width: 182px;
        }
        .style23
        {
            width: 346px;
        }
        .style25
        {
            width: 136px;
        }
        
        .style26
        {
            width: 104px;
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
            width: 86px;
            height: 24px;
        }
        
        .style38
        {
            width: 101px;
            height: 24px;
        }
        
        .style39
        {
            width: 60px;
            height: 24px;
        }
        .style40
        {
            height: 24px;
        }
        
        .style41
        {
            width: 338px;
            height: 26px;
            position: absolute;
            left: 8px;
            top: 193px;
        }
        .style42
        {
            width: 151px;
            height: 18px;
            position: absolute;
            left: 183px;
            top: 195px;
        }
        .style43
        {
            width: 154px;
            height: 18px;
            position: absolute;
            left: 9px;
            top: 193px;
        }
    </style>
    <script type="text/javascript">
        function display() {

            document.getElementById('MainContent_norecordlbl').innerHTML = "";

        }
    </script>
    <body>
        <asp:ScriptManager ID="ScriptManager1" runat="server">
        </asp:ScriptManager>
        <div style="height: 55px; margin-left: 0px;">
            <br />
            <center>
                <span class="fontstyleheader" style="color: Green;">AT05-Consolidate Student Attendance
                    Report</span>
            </center>
            <br />
            <div>
                <div class="maintablestyle" style="width: 1100px;">
                    <table cellpadding="0px" cellspacing="0px" style="height: 100%; width: 103%;">
                        <tr>
                            <td class="style25">
                                <asp:Label runat="server" ID="lblbatch" Text="Batch" Font-Bold="True" Font-Names="Book Antiqua"
                                    Font-Size="Medium"></asp:Label>
                                <asp:DropDownList ID="ddlbatch" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                    Font-Size="Medium" OnSelectedIndexChanged="ddlbatch_SelectedIndexChanged" Height="25px"
                                    Width="60px" AutoPostBack="True">
                                </asp:DropDownList>
                            </td>
                            <td class="style22">
                                <asp:Label runat="server" ID="lbldegree" Text="Degree" Font-Bold="True" Font-Names="Book Antiqua"
                                    Font-Size="Medium"></asp:Label>
                                <asp:DropDownList runat="server" ID="ddldegree" Height="24px" Width="88px" AutoPostBack="True"
                                    Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium" OnSelectedIndexChanged="ddldegree_SelectedIndexChanged">
                                </asp:DropDownList>
                            </td>
                            <td class="style23">
                                <asp:Label runat="server" ID="lblbranch" Text="Branch" Font-Bold="True" Font-Names="Book Antiqua"
                                    Font-Size="Medium"></asp:Label>
                                <asp:DropDownList runat="server" ID="ddlbranch" Font-Bold="True" Height="25px" Width="260px"
                                    Font-Names="Book Antiqua" Font-Size="Medium" OnSelectedIndexChanged="ddlbranch_SelectedIndexChanged"
                                    AutoPostBack="True">
                                </asp:DropDownList>
                            </td>
                            <td class="style19">
                                <asp:Label runat="server" ID="lblduration" Text="Sem" Font-Bold="True" Font-Names="Book Antiqua"
                                    Font-Size="Medium"></asp:Label>
                                <asp:DropDownList runat="server" ID="ddlduration" Font-Bold="True" Font-Names="Book Antiqua"
                                    Font-Size="Medium" AutoPostBack="True" Height="25px" Width="47px" OnSelectedIndexChanged="ddlduration_SelectedIndexChanged">
                                </asp:DropDownList>
                            </td>
                            <td class="style26">
                                <asp:Label runat="server" ID="lblsec" Text="Sec" Font-Bold="True" Font-Names="Book Antiqua"
                                    Font-Size="Medium"></asp:Label>
                                <asp:DropDownList runat="server" ID="ddlsec" Font-Bold="True" Font-Names="Book Antiqua"
                                    Font-Size="Medium" Height="25px" Width="49px" AutoPostBack="True" OnSelectedIndexChanged="ddlsec_SelectedIndexChanged">
                                </asp:DropDownList>
                            </td>
                            <td>
                                <asp:RadioButtonList ID="optionbtn" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                    Font-Size="Medium" Width="223px" Style="margin-left: 0px" RepeatDirection="Horizontal"
                                    AutoPostBack="True" OnSelectedIndexChanged="optionbtn_SelectedIndexChanged">
                                    <asp:ListItem Text="P/P <br/> Mode" Value="pp" Selected="True"></asp:ListItem>
                                    <asp:ListItem Text="1 or 0.5 <br/> Mode" Value="oo"></asp:ListItem>
                                    <asp:ListItem Text="P or A <br/> Mode" Value="pa"></asp:ListItem>
                                </asp:RadioButtonList>
                            </td>
                        </tr>
                    </table>
                    <table>
                        <tr>
                            <td class="style29">
                                <asp:Label ID="lblFromdate" runat="server" Text="From Date" Font-Bold="True" Font-Names="Book Antiqua"
                                    Font-Size="Medium"></asp:Label>
                            </td>
                            <td class="style40">
                                <asp:TextBox ID="txtFromDate" CssClass="txt" runat="server" Height="25px" Width="75px"
                                    Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium" OnTextChanged="txtFromDate_TextChanged"></asp:TextBox>
                                <asp:FilteredTextBoxExtender ID="txtFromDate_FilteredTextBoxExtender" FilterType="Custom,Numbers"
                                    ValidChars="/" runat="server" TargetControlID="txtFromDate">
                                </asp:FilteredTextBoxExtender>
                                <asp:CalendarExtender ID="calfromdate" TargetControlID="txtFromDate" Format="d/MM/yyyy"
                                    runat="server">
                                </asp:CalendarExtender>
                            </td>
                            <td class="style39">
                                <asp:Label ID="lbltodate" runat="server" Text="To Date" Font-Bold="True" Font-Names="Book Antiqua"
                                    Font-Size="Medium"></asp:Label>
                            </td>
                            <td class="style38">
                                <asp:TextBox ID="txtToDate" CssClass="txt" runat="server" Height="25px" Width="75px"
                                    Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium" OnTextChanged="txtToDate_TextChanged"></asp:TextBox>
                                <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" FilterType="Custom,Numbers"
                                    ValidChars="/" runat="server" TargetControlID="txtToDate">
                                </asp:FilteredTextBoxExtender>
                                <asp:CalendarExtender ID="CalendarExtender1" TargetControlID="txtToDate" Format="d/MM/yyyy"
                                    runat="server">
                                </asp:CalendarExtender>
                            </td>
                            <td class="style40">
                                <asp:Button ID="btnGo" runat="server" Text="Go" OnClick="btnGo_Click" Font-Bold="True"
                                    Font-Names="Book Antiqua" Font-Size="Medium" Height="26px" Width="40px" Style="top: 241px;
                                    left: 320px" />
                            </td>
                            <td class="style33">
                                <asp:CheckBox ID="cumcheck" runat="server" Text="Cummulative %" Font-Bold="True"
                                    Font-Names="Book Antiqua" Font-Size="Medium" AutoPostBack="True" OnCheckedChanged="cumcheck_CheckedChanged" />
                            </td>
                            <td class="style40">
                                <asp:Label ID="cumfromlbl" runat="server" Text="From Date" Font-Bold="True" Font-Names="Book Antiqua"
                                    Font-Size="Medium"></asp:Label>
                            </td>
                            <td class="style16">
                                <asp:TextBox ID="cumfromtxt" CssClass="txt" runat="server" Height="25px" Width="75px"
                                    Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium" OnTextChanged="cumfromtxt_TextChanged"></asp:TextBox>
                                <asp:FilteredTextBoxExtender ID="cumfromfilt" FilterType="Custom,Numbers" ValidChars="/"
                                    runat="server" TargetControlID="cumfromtxt">
                                </asp:FilteredTextBoxExtender>
                                <asp:CalendarExtender ID="cumfromcal" TargetControlID="cumfromtxt" Format="d/MM/yyyy"
                                    runat="server">
                                </asp:CalendarExtender>
                            </td>
                            <td class="style17">
                                <asp:Label ID="cumtolbl" runat="server" Text="To Date" Font-Bold="True" Font-Names="Book Antiqua"
                                    Font-Size="Medium"></asp:Label>
                                <asp:FilteredTextBoxExtender ID="cumtofilt" FilterType="Custom,Numbers" ValidChars="/"
                                    runat="server" TargetControlID="cumtotxt">
                                </asp:FilteredTextBoxExtender>
                                <asp:CalendarExtender ID="cumtocal" TargetControlID="cumtotxt" Format="d/MM/yyyy"
                                    runat="server">
                                </asp:CalendarExtender>
                            </td>
                            <td class="style40">
                                <asp:TextBox ID="cumtotxt" CssClass="txt" runat="server" Height="25px" Width="83px"
                                    Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium" OnTextChanged="cumtotxt_TextChanged"></asp:TextBox>
                            </td>
                            <td class="style36">
                                <asp:CheckBox ID="pointchk" runat="server" Text="Att Point" Font-Bold="True" Font-Names="Book Antiqua"
                                    Font-Size="Medium" />
                            </td>
                            <td class="style40">
                            </td>
                        </tr>
                        <tr>
                            <td class="style14" colspan="2">
                                <asp:Label ID="frmlbl" runat="server" Text="Select From Date" ForeColor="Red" Font-Bold="True"
                                    Font-Names="Book Antiqua" Font-Size="Small"></asp:Label>
                            </td>
                            <td class="style15" colspan="2">
                                <asp:Label ID="tolbl" runat="server" Text="Select To Date" ForeColor="Red" Font-Bold="True"
                                    Font-Names="Book Antiqua" Font-Size="Small"></asp:Label>
                            </td>
                            <td colspan="7">
                                <asp:Label ID="tofromlbl" runat="server" Text="From date should not be greater than To date"
                                    ForeColor="Red" Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Small"></asp:Label>
                                <asp:Button ID="btnPrint" runat="server" Font-Bold="True" Text="Print Master Setting"
                                    Visible="False" Font-Names="Book Antiqua" Font-Size="Medium" OnClick="btnPrint_Click"
                                    Width="160px" />
                                <asp:Label ID="lblpages" runat="server" Text="Page" Font-Names="Book Antiqua" Font-Size="Medium"
                                    Font-Bold="true"></asp:Label>
                                <asp:DropDownList ID="ddlpage" runat="server" OnSelectedIndexChanged="ddlpage_SelectedIndexChanged"
                                    Font-Names="Book Antiqua" Font-Size="Medium" Font-Bold="true" AutoPostBack="True"
                                    Width="47px">
                                </asp:DropDownList>
                            </td>
                        </tr>
                    </table>
                </div>
                <table>
                    <tr>
                        <td class="style1">
                        </td>
                    </tr>
                    <tr>
                        <td class="style1">
                        </td>
                    </tr>
                    <tr>
                        <td class="style1">
                        </td>
                    </tr>
                    <tr>
                        <td class="style1">
                        </td>
                    </tr>
                    <tr>
                        <td class="style1">
                        </td>
                    </tr>
                    <tr>
                        <td class="style1">
                        </td>
                    </tr>
                    <tr>
                        <td class="style1">
                        </td>
                    </tr>
                    <tr>
                        <td class="style1"  align="center">
                            <asp:Label ID="norecordlbl" runat="server" Font-Bold="true" Font-Size="Medium" ForeColor="Red"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td class="style1">
                            <asp:Panel ID="Panel3" runat="server" BorderStyle="None" Width="949px">
                                <asp:Label ID="Buttontotal" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                    Font-Size="Medium"></asp:Label>
                                &nbsp;&nbsp;
                                <asp:Label ID="ddlpagelbl" runat="server" Font-Bold="True" Text="     Records Per Page"
                                    Font-Names="Book Antiqua" Font-Size="Medium"></asp:Label>
                                <asp:DropDownList ID="DropDownListpage" runat="server" Height="25px" Width="65px"
                                    Font-Bold="True" Font-Size="Medium" Font-Names="Book Antiqua" OnSelectedIndexChanged="DropDownListpage_SelectedIndexChanged"
                                    AutoPostBack="True">
                                </asp:DropDownList>
                                <asp:TextBox ID="pageddltxt" runat="server" Height="22px" Width="40px" Font-Bold="True"
                                    Font-Size="Medium" Font-Names="Book Antiqua" OnTextChanged="pageddltxt_TextChanged"
                                    AutoPostBack="True"></asp:TextBox>
                                <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender3" runat="server" FilterType="Numbers"
                                    TargetControlID="pageddltxt">
                                </asp:FilteredTextBoxExtender>
                                <asp:Label ID="pgsearch_lbl" runat="server" Font-Bold="True" Text="Page Search:"
                                    Width="95px" Font-Names="Book Antiqua" Font-Size="Medium" Height="24px"></asp:Label>
                                <asp:TextBox ID="pagesearch_txt" runat="server" AutoPostBack="True" Font-Bold="True"
                                    Font-Names="Book Antiqua" Font-Size="Medium" Height="22px" Width="40px" OnTextChanged="pagesearch_txt_TextChanged"></asp:TextBox>
                                <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender2" runat="server" TargetControlID="pagesearch_txt"
                                    FilterType="Numbers">
                                </asp:FilteredTextBoxExtender>
                                <asp:Label ID="errmsg" runat="server" ForeColor="Red" Font-Bold="True" Font-Names="Book Antiqua"
                                    Font-Size="Medium" Height="16px" Width="355px"></asp:Label>
                            </asp:Panel>
                        </td>
                    </tr>
                    <tr>
                        <td class="style1">
                            <FarPoint:FpSpread ID="FpSpread1" runat="server" ActiveSheetViewIndex="0" currentPageIndex="0"
                                DesignString="&lt;?xml version=&quot;1.0&quot; encoding=&quot;utf-8&quot;?&gt;&lt;Spread /&gt;"
                                EnableClientScript="False" CssClass="cursorptr" BorderWidth="0.5" ShowHeaderSelection="false">
                                <CommandBar BackColor="Control" ButtonType="PushButton">
                                    <Background BackgroundImageUrl="SPREADCLIENTPATH:/img/cbbg.gif" />
                                </CommandBar>
                                <Pager Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                    Font-Underline="False" />
                                <HierBar Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                    Font-Underline="False" />
                                <Sheets>
                                    <FarPoint:SheetView SheetName="Sheet1" EditTemplateColumnCount="2" GridLineColor="Black"
                                        GroupBarText="Drag a column to group by that column." SelectionBackColor="#CE5D5A"
                                        SelectionForeColor="White">
                                    </FarPoint:SheetView>
                                </Sheets>
                                <TitleInfo BackColor="#E7EFF7" Font-Size="X-Large" ForeColor="" HorizontalAlign="Center"
                                    VerticalAlign="NotSet" Font-Bold="False" Font-Italic="False" Font-Overline="False"
                                    Font-Strikeout="False" Font-Underline="False">
                                </TitleInfo>
                            </FarPoint:FpSpread>
                        </td>
                    </tr>
                    <tr>
                        <td class="style1">
                            <asp:Panel runat="server" ID="ne">
                                <asp:Label ID="Label1" runat="server" Text="NE" BackColor="AliceBlue" ForeColor="DarkRed"></asp:Label>&nbsp;&nbsp;&nbsp;
                                <asp:Label ID="Label2" runat="server" Text="No Entry"></asp:Label>
                            </asp:Panel>
                        </td>
                    </tr>
                    <tr>
                        <td>
                        <br />
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
                            <Insproplus:PRINTPDF runat="server" ID="Printcontrol" Visible="false" />
                        </td>
                    </tr>
                </table>
            </div>
    </body>
    </html>
</asp:Content>

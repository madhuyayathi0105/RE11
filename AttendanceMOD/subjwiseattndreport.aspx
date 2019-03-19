<%@ Page Title="AT07-Subject Wise Attendance With Percentage Report" Language="C#"
    MasterPageFile="~/AttendanceMOD/AttendanceSubSiteMaster.master" AutoEventWireup="true"
    CodeFile="subjwiseattndreport.aspx.cs" Inherits="subjwiseattndreport" EnableEventValidation="false" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Src="~/Usercontrols/NewPrintMaster.ascx" TagName="NEWPrintMater" TagPrefix="NEW" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
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
    <script type="text/javascript">
        function display() {
            document.getElementById('MainContent_errmsg').innerHTML = "";
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
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <div>
                <center>
                    <span class="fontstyleheader" style="color: Green; margin: 0px; margin-bottom: 10px;
                        margin-top: 10px; position: relative;">AT07-Subject Wise Attendance With Percentage
                        Report </span>
                    <div>
                        <table class="maintablestyle" style="margin: 0px; margin-bottom: 10px; margin-top: 10px;
                            position: relative; text-align: left; padding: 5px;">
                            <tr>
                                <td colspan="11">
                                    <table>
                                        <tr>
                                            <td>
                                                <asp:Label runat="server" ID="lblbatch" Text="Batch" Font-Bold="True" Font-Names="Book Antiqua"
                                                    Font-Size="Medium"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:DropDownList ID="ddlbatch" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                                    Font-Size="Medium" OnSelectedIndexChanged="ddlbatch_SelectedIndexChanged" Height="25px"
                                                    Width="61px" AutoPostBack="True">
                                                </asp:DropDownList>
                                            </td>
                                            <td>
                                                <asp:Label runat="server" ID="lbldegree" Text="Degree" Font-Bold="True" Font-Names="Book Antiqua"
                                                    Font-Size="Medium"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:DropDownList runat="server" ID="ddldegree" Height="25px" Width="90px" AutoPostBack="True"
                                                    OnSelectedIndexChanged="ddldegree_SelectedIndexChanged" Font-Bold="True" Font-Names="Book Antiqua"
                                                    Font-Size="Medium">
                                                </asp:DropDownList>
                                            </td>
                                            <td>
                                                <asp:Label runat="server" ID="lblbranch" Text="Branch" Font-Bold="True" Font-Names="Book Antiqua"
                                                    Font-Size="Medium"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:DropDownList runat="server" ID="ddlbranch" Font-Bold="True" Height="25px" Width="237px"
                                                    Font-Names="Book Antiqua" Font-Size="Medium" OnSelectedIndexChanged="ddlbranch_SelectedIndexChanged"
                                                    AutoPostBack="True">
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
                                                            Font-Size="Medium" AutoPostBack="True" Height="25px" Width="40px" OnSelectedIndexChanged="ddlduration_SelectedIndexChanged">
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
                                                    Font-Size="Medium" Width="40px" AutoPostBack="True" OnSelectedIndexChanged="ddlsec_SelectedIndexChanged">
                                                </asp:DropDownList>
                                            </td>
                                            <td>
                                                <td>
                                                    <asp:CheckBox ID="chkincludepastout" runat="server" Text="Include PassedOut" Font-Bold="True"
                                                        Font-Names="Book Antiqua" Font-Size="Medium" OnCheckedChanged="includepastout_CheckedChanged"
                                                        AutoPostBack="True" />
                                                </td>
                                                <td>
                                                </td>
                                                <td>
                                                    <asp:Button ID="btnPrint" runat="server" CssClass="textbox textbox1" Style="width: auto;
                                                        height: auto;" Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium"
                                                        OnClick="btnPrint_Click" Text="Print Master Setting" Visible="False" />
                                                </td>
                                            </td>
                                        </tr>
                                    </table>
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
                                                    Font-Size="Medium"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtToDate" CssClass="txt" runat="server" Width="75px" Font-Bold="True"
                                                    Font-Names="Book Antiqua" Font-Size="Medium" OnTextChanged="txtToDate_TextChanged"
                                                    AutoPostBack="True"></asp:TextBox>
                                                <asp:FilteredTextBoxExtender ID="txtToDate_FilteredTextBoxExtender" runat="server"
                                                    TargetControlID="txtToDate" FilterType="Custom,Numbers" ValidChars="/">
                                                </asp:FilteredTextBoxExtender>
                                                <asp:CalendarExtender ID="caltodate" TargetControlID="txtToDate" Format="d/MM/yyyy"
                                                    runat="server">
                                                </asp:CalendarExtender>
                                            </td>
                                            <td>
                                                <asp:UpdatePanel ID="UpdatePanel_go" runat="server">
                                                    <ContentTemplate>
                                                        <asp:Button ID="btnGo" runat="server" Text="Go" Style="font-weight: 700; width: auto;
                                                            height: auto;" OnClick="btnGo_Click" Font-Bold="True" Font-Names="Book Antiqua"
                                                            Font-Size="Medium" CssClass="textbox textbox1" />
                                                    </ContentTemplate>
                                                </asp:UpdatePanel>
                                            </td>
                                            <td>
                                                <asp:Label ID="basedlbl" runat="server" Text="Based On" Font-Size="Medium" Font-Bold="True"
                                                    Font-Names="Book Antiqua"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:DropDownList ID="basedddl" runat="server" Font-Size="Medium" Font-Bold="True"
                                                    Font-Names="Book Antiqua" AutoPostBack="True" OnSelectedIndexChanged="basedddl_SelectedIndexChanged">
                                                    <asp:ListItem Value="0">Daily Entry</asp:ListItem>
                                                    <asp:ListItem Value="1">Semester Schedule</asp:ListItem>
                                                </asp:DropDownList>
                                            </td>
                                            <td>
                                                <asp:CheckBox ID="percheck" runat="server" Text="Percentage" Font-Bold="True" Font-Names="Book Antiqua"
                                                    Font-Size="Medium" OnCheckedChanged="percheck_CheckedChanged" AutoPostBack="True" />
                                            </td>
                                            <td>
                                                <asp:DropDownList ID="perddl" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                                    Font-Size="Medium" Width="37px" AutoPostBack="True" OnSelectedIndexChanged="perddl_SelectedIndexChanged">
                                                    <asp:ListItem Value="0">&gt;</asp:ListItem>
                                                    <asp:ListItem Value="1">&gt;=</asp:ListItem>
                                                    <asp:ListItem Value="2">&lt;</asp:ListItem>
                                                    <asp:ListItem Value="3">&lt;=</asp:ListItem>
                                                    <asp:ListItem Value="4">&lt;&gt;</asp:ListItem>
                                                    <asp:ListItem Value="5">=</asp:ListItem>
                                                </asp:DropDownList>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="pertxt" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                                    Font-Size="Medium" Width="35px"></asp:TextBox>
                                                <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender4" FilterType="Numbers" runat="server"
                                                    TargetControlID="pertxt">
                                                </asp:FilteredTextBoxExtender>
                                            </td>
                                            <td>
                                                <asp:CheckBox ID="chklabsubbatch" runat="server" Text="Lab Subject Batch Wise Maximum Conducted Hours"
                                                    Font-Size="Medium" Font-Bold="True" Font-Names="Book Antiqua" AutoPostBack="true"
                                                    OnCheckedChanged="chklabsubbatch_CheckedChanged" />
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                        </table>
                    </div>
                </center>
                <table>
                    <tr>
                        <td align="center">
                            <asp:Label ID="errmsg" runat="server" Style="margin: 0px; margin-bottom: 10px; margin-top: 10px;
                                position: relative;" ForeColor="Red" Font-Bold="True" Font-Names="Book Antiqua"
                                Font-Size="Medium"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td align="center">
                            <asp:Label ID="frmlbl" runat="server" Style="margin: 0px; margin-bottom: 10px; margin-top: 10px;
                                position: relative;" Text="Select From Date" ForeColor="Red" Font-Bold="True"
                                Font-Names="Book Antiqua"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td align="center">
                            <asp:Label ID="tolbl" runat="server" Style="margin: 0px; margin-bottom: 10px; margin-top: 10px;
                                position: relative;" Text="Select To Date" ForeColor="Red" Font-Bold="True" Font-Names="Book Antiqua"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td align="center">
                            <asp:Label ID="tofromlbl" runat="server" Style="margin: 0px; margin-bottom: 10px;
                                margin-top: 10px; position: relative;" Text="From date should not be greater than To date"
                                ForeColor="Red" Font-Bold="True" Font-Names="Book Antiqua"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Panel ID="setpanel" runat="server" BorderStyle="None" Width="944px" Style="margin: 0px;
                                margin-bottom: 10px; margin-top: 10px; position: relative;">
                                <table>
                                    <tr>
                                        <td>
                                            <asp:Label ID="Buttontotal" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                                Font-Size="Medium"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:Label ID="ddlpagelbl" runat="server" Font-Bold="True" Text="Records Per Page"
                                                Font-Names="Book Antiqua" Font-Size="Medium"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:DropDownList ID="DropDownListpage" runat="server" Height="25px" Width="65px"
                                                Font-Bold="True" Font-Size="Medium" Font-Names="Book Antiqua" OnSelectedIndexChanged="DropDownListpage_SelectedIndexChanged"
                                                AutoPostBack="True">
                                            </asp:DropDownList>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="pageddltxt" runat="server" Height="21px" Width="40px" Font-Bold="True"
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
                                                Font-Names="Book Antiqua" Font-Size="Medium" Height="20px" Width="40px" OnTextChanged="pagesearch_txt_TextChanged"></asp:TextBox>
                                            <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender2" runat="server" TargetControlID="pagesearch_txt"
                                                FilterType="Numbers">
                                            </asp:FilteredTextBoxExtender>
                                        </td>
                                        <td>
                                            <asp:Label ID="seterr" runat="server" ForeColor="Red" Font-Bold="True" Font-Names="Book Antiqua"
                                                Font-Size="Medium" Width="329px"></asp:Label>
                                        </td>
                                    </tr>
                                </table>
                            </asp:Panel>
                        </td>
                    </tr>
                </table>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
    <br />
    <br />
    <asp:UpdatePanel ID="UpdatePanel2" runat="server">
        <ContentTemplate>
            <center>
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
                        HeaderStyle-BackColor="#0CA6CA" OnRowDataBound="Showgrid_OnRowDataBound">
                    </asp:GridView>
                    <asp:Label ID="lbl_sno" runat="server" Visible="false" Text='<%#Eval("appno") %>'>
                    </asp:Label>
                </div>
            </center>
            <center>
                <table style="margin: 0px; margin-bottom: 10px; margin-top: 10px;">
                    <tr>
                        <td>
                            <asp:Label ID="lblrptname" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                Font-Size="Medium" Text="Report Name"></asp:Label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtexcelname" runat="server" onkeypress="display()" Height="20px"
                                Width="180px" Style="font-family: 'Book Antiqua'" Font-Bold="True" Font-Names="Book Antiqua"
                                Font-Size="Medium"></asp:TextBox>
                            <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" runat="server" TargetControlID="txtexcelname"
                                FilterType="LowercaseLetters,UppercaseLetters,Numbers,Custom" ValidChars="!@$%^&*()_+|\}{][':;?><,./">
                            </asp:FilteredTextBoxExtender>
                        </td>
                        <td>
                            <asp:Button ID="btnxl" CssClass="textbox textbox1" Style="width: auto; height: auto;"
                                runat="server" Text="Export Excel" Font-Bold="True" Font-Names="Book Antiqua"
                                Font-Size="Medium" OnClick="btnxl_Click" />
                        </td>
                        <td>
                            <asp:Button ID="btnprintmaster" CssClass="textbox textbox1" runat="server" Text="Print"
                                OnClick="btnprintmaster_Click" Font-Names="Book Antiqua" Font-Size="Medium" Font-Bold="true"
                                Style="width: auto; height: auto;" />
                            <NEW:NEWPrintMater runat="server" ID="Printcontrol" Visible="false" />
                            <asp:Button ID="btndirtPrint" runat="server" Text="Direct Print" OnClientClick="return PrintPanel();"
                                Font-Names="Book Antiqua" Font-Size="Medium" Font-Bold="true" Height="35px" CssClass="textbox textbox1" />
                        </td>
                    </tr>
                </table>
            </center>
            </div>
        </ContentTemplate>
        <Triggers>
            <asp:PostBackTrigger ControlID="btnxl" />
            <asp:PostBackTrigger ControlID="btnprintmaster" />
            <asp:PostBackTrigger ControlID="btnGo" />
        </Triggers>
    </asp:UpdatePanel>
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
    <%--progressBar for Upbook_go--%>
    <%-- <center>
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
    </center>--%>
</asp:Content>

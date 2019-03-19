<%@ Page Title="" Language="C#" MasterPageFile="~/AttendanceMOD/AttendanceSubSiteMaster.master"
    AutoEventWireup="true" CodeFile="cumreport.aspx.cs" Inherits="cumreport" EnableEventValidation="false" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Src="~/Usercontrols/NewPrintMaster.ascx" TagName="NEWPrintMater" TagPrefix="NEW" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">
    <!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN">
    <html>
    <body>
        <style type="text/css">
            .style2
            {
                width: 43px;
            }
            .style3
            {
                width: 95px;
            }
            .style4
            {
                width: 154px;
            }
            .style5
            {
                width: 77px;
                top: 157px;
                left: 789px;
            }
            .style6
            {
                width: 36px;
            }
            .style8
            {
                width: 28px;
            }
            .style9
            {
                width: 45px;
            }
            .style10
            {
                width: 90px;
            }
            .style11
            {
                width: 24px;
            }
            .style12
            {
                width: 98px;
            }
            .style13
            {
            }
            .style15
            {
                width: 42px;
            }
            .style19
            {
                width: 69px;
            }
            .style20
            {
                width: 47px;
            }
            .style21
            {
                width: 95px;
            }
            .style22
            {
                width: 979px;
            }
            .style24
            {
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
        <script type="text/javascript">
            function display() {
                document.getElementById('MainContent_lblnorec').innerHTML = "";
            }

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
        <br />
        <center>
            <span class="fontstyleheader" style="color: Green;">AT02-Cumulative Attendance Report</span>
        </center>
        <br />
        <center>
            <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                <ContentTemplate>
                    <div style="width: 1000px; height: 150px;">
                        <asp:ScriptManager ID="ScriptManager1" runat="server">
                        </asp:ScriptManager>
                        <div class="maintablestyle">
                            <table style="width: 987px">
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
                                            AutoPostBack="True" Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium"
                                            Height="25px">
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
                                        <asp:UpdatePanel ID="UpdatePanel_sem" runat="server">
                                            <ContentTemplate>
                                                <asp:DropDownList ID="ddlsemester" runat="server" OnSelectedIndexChanged="ddlsemester_SelectedIndexChanged"
                                                    AutoPostBack="True" Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium"
                                                    Height="25px" Width="41px">
                                                </asp:DropDownList>
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
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
                            </table>
                            <table style="width: 1138px">
                                <tr>
                                    <td>
                                        <table>
                                            <tr>
                                                <td>
                                                    <asp:Label ID="Label6" runat="server" Text="From" Font-Bold="True" Font-Names="Book Antiqua"
                                                        Font-Size="Medium"></asp:Label>
                                                    <asp:TextBox ID="txtfromdate" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                                        Font-Size="Medium" Height="24px" Width="85px" OnTextChanged="txtfromdate_TextChanged"
                                                        AutoPostBack="True"></asp:TextBox>
                                                    <asp:CalendarExtender ID="CalendarExtender1" Format="d/MM/yyyy" TargetControlID="txtfromdate"
                                                        runat="server">
                                                    </asp:CalendarExtender>
                                                    <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender4" runat="server" TargetControlID="txtfromdate"
                                                        FilterType="Numbers,Custom" ValidChars="/">
                                                    </asp:FilteredTextBoxExtender>
                                                </td>
                                                <td class="style11">
                                                    <asp:Label ID="Label3" runat="server" Text="To" Font-Bold="True" Font-Names="Book Antiqua"
                                                        Font-Size="Medium"></asp:Label>
                                                </td>
                                                <td class="style12">
                                                    <asp:TextBox ID="txttodate" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                                        Font-Size="Medium" Height="24px" Width="92px" OnTextChanged="txttodate_TextChanged"
                                                        AutoPostBack="True"></asp:TextBox>
                                                    <asp:CalendarExtender ID="CalendarExtender2" Format="d/MM/yyyy" TargetControlID="txttodate"
                                                        runat="server">
                                                    </asp:CalendarExtender>
                                                    <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender5" runat="server" TargetControlID="txttodate"
                                                        FilterType="Numbers,Custom" ValidChars="/">
                                                    </asp:FilteredTextBoxExtender>
                                                </td>
                                                <td>
                                                    <asp:UpdatePanel ID="UpdatePanel_go" runat="server">
                                                        <ContentTemplate>
                                                            <asp:Button ID="btngo" runat="server" Text="Go" OnClick="btngo_Click" Font-Bold="True"
                                                                Font-Names="Book Antiqua" Font-Size="Medium" />
                                                        </ContentTemplate>
                                                    </asp:UpdatePanel>
                                                </td>
                                                <td>
                                                    <asp:CheckBox ID="cbcumpercent" runat="server" Text="Cumulative %" Font-Bold="True"
                                                        Font-Names="Book Antiqua" Font-Size="Medium" OnCheckedChanged="cbcumpercent_CheckedChanged"
                                                        AutoPostBack="True" Style="height: 21px; width: 145px" />
                                                    <asp:Label ID="lblcumfrm" runat="server" Text="From" Font-Bold="True" Font-Names="Book Antiqua"
                                                        Font-Size="Medium" Style="height: 21px; width: 38px"></asp:Label>
                                                    <asp:TextBox ID="tbfmcumdate" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                                        Font-Size="Medium" Height="24px" Width="91px" OnTextChanged="tbfmcumdate_TextChanged"
                                                        AutoPostBack="True"></asp:TextBox>
                                                    <asp:CalendarExtender ID="CalendarExtender4" Format="d/MM/yyyy" TargetControlID="tbfmcumdate"
                                                        runat="server">
                                                    </asp:CalendarExtender>
                                                    <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender7" runat="server" TargetControlID="tbfmcumdate"
                                                        FilterType="Numbers,Custom" ValidChars="/">
                                                    </asp:FilteredTextBoxExtender>
                                                    <asp:Label ID="lblcumto" runat="server" Text="To" Font-Bold="True" Font-Names="Book Antiqua"
                                                        Font-Size="Medium" Style="height: 21px; width: 20px"></asp:Label>
                                                    <asp:TextBox ID="tbtocumdate" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                                        Font-Size="Medium" Height="24px" Width="90px" OnTextChanged="tbtocumdate_TextChanged"
                                                        AutoPostBack="True"></asp:TextBox>
                                                    <asp:CalendarExtender ID="CalendarExtender3" Format="d/MM/yyyy" TargetControlID="tbtocumdate"
                                                        runat="server">
                                                    </asp:CalendarExtender>
                                                    <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender6" runat="server" TargetControlID="tbtocumdate"
                                                        FilterType="Numbers,Custom" ValidChars="/">
                                                    </asp:FilteredTextBoxExtender>
                                                    <asp:CheckBox ID="cbpoints" runat="server" Text="Attnd Points" Font-Bold="True" Width="115px"
                                                        Font-Names="Book Antiqua" Font-Size="Medium" Height="21px" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <asp:Label ID="lblfrom" runat="server" Text="Range From" Font-Bold="True" Font-Names="Book Antiqua"
                                                        Font-Size="Medium"></asp:Label>
                                                    <asp:TextBox ID="txtfromrange" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                                        Font-Size="Medium" AutoPostBack="true" OnTextChanged="txtfromrange_TextChanged"
                                                        MaxLength="3" Width="30px"></asp:TextBox>
                                                    <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" runat="server" TargetControlID="txtfromrange"
                                                        FilterType="Numbers" />
                                                    <asp:Label ID="lbltorange" runat="server" Text="To" Font-Bold="True" Font-Names="Book Antiqua"
                                                        Font-Size="Medium"></asp:Label>
                                                    <asp:TextBox ID="txttorange" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                                        Font-Size="Medium" AutoPostBack="true" OnTextChanged="txttorange_TextChanged"
                                                        MaxLength="3" Width="30px"></asp:TextBox>
                                                    <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender3" runat="server" TargetControlID="txttorange"
                                                        FilterType="Numbers" />
                                                </td>
                                                <td>
                                                    <asp:CheckBox ID="chkonduty" runat="server" Text="Include Onduty Periods(Attend Periods+Onduty Periods)"
                                                        Font-Bold="True" Font-Names="Book Antiqua" Font-Size="15px" Style="margin-left: -5px;
                                                        margin-top: -10px; position: absolute;" />
                                                </td>
                                                <td>
                                                    <asp:CheckBox ID="chkondutyspit" runat="server" Text="Onduty Periods Details" Font-Bold="True"
                                                        Font-Names="Book Antiqua" Font-Size="15px" AutoPostBack="true" OnCheckedChanged="chkondutyspit_CheckedChanged"
                                                        Style="margin-left: 408px; margin-top: -9px; position: absolute;" />
                                                    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                                                        <ContentTemplate>
                                                            <asp:TextBox ID="txtonduty" runat="server" CssClass="Dropdown_Txt_Box" ReadOnly="true"
                                                                Width="150px" Style="margin-left: 590px; margin-top: -14px; position: absolute;
                                                                font-family: 'Book Antiqua';" Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium">---Select---</asp:TextBox>
                                                            <asp:Panel ID="ponduty" runat="server" Width="300px" CssClass="multxtpanel" Height="250px">
                                                                <asp:CheckBox ID="chksonduty" runat="server" Font-Bold="True" OnCheckedChanged="chksonduty_ChekedChange"
                                                                    Font-Names="Book Antiqua" Font-Size="Medium" Text="Select All" AutoPostBack="True" />
                                                                <asp:CheckBoxList ID="chklsonduty" runat="server" Font-Size="Medium" AutoPostBack="True"
                                                                    Font-Bold="True" Font-Names="Book Antiqua" OnSelectedIndexChanged="chklsonduty_SelectedIndexChanged">
                                                                </asp:CheckBoxList>
                                                            </asp:Panel>
                                                            <asp:PopupControlExtender ID="PopupControlExtender1" runat="server" TargetControlID="txtonduty"
                                                                PopupControlID="ponduty" Position="Bottom">
                                                            </asp:PopupControlExtender>
                                                        </ContentTemplate>
                                                    </asp:UpdatePanel>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td colspan="4">
                                                    <asp:CheckBox ID="cbincround" runat="server" Text="Include Round" Font-Bold="True"
                                                        Font-Names="Book Antiqua" Font-Size="Medium" />
                                                    <asp:CheckBox ID="CheckBox1" runat="server" Text="Include Discontinue" Font-Bold="True"
                                                        Font-Names="Book Antiqua" Font-Size="Medium" />
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                            </table>
                        </div>
                        <br />
                        <center>
                            <asp:Label ID="dateerr" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                Font-Size="Medium" ForeColor="Red" Width="676px"></asp:Label>
                            <asp:Label ID="lblnorec" runat="server" Text="No Record(s) Found" ForeColor="Red"
                                Visible="False" Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium"></asp:Label>
                        </center>
                        <br />
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
                                <asp:HiddenField ID="SelectedGridCellIndex" runat="server" Value="-1" />
                                <asp:GridView ID="Showgrid" runat="server" Visible="false" HeaderStyle-ForeColor="Black"
                                    HeaderStyle-BackColor="#0CA6CA" ShowHeaderWhenEmpty="true" OnRowDataBound="Showgrid_OnRowDataBound"
                                    OnRowCreated="OnRowCreated" OnSelectedIndexChanged="SelectedIndexChanged">
                                </asp:GridView>
                            </div>
                        </center>
                        <br />
                        <center>
                            <asp:Button ID="btnclose" runat="server" Text="Close" OnClick="btnclose_Click" />
                            <br />
                            <asp:Button ID="btnletter" runat="server" Text="Letter Report" Font-Bold="True" Font-Names="Book Antiqua"
                                Font-Size="Medium" OnClick="btnletter_Click" Visible="False" />
                            <asp:Label ID="lblrptname" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                Font-Size="Medium" Text="Report Name"></asp:Label>
                            <asp:TextBox ID="txtexcelname" runat="server" Height="20px" Width="180px" Style="font-family: 'Book Antiqua'"
                                Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium" onkeypress="display()"></asp:TextBox>
                            <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender2" runat="server" TargetControlID="txtexcelname"
                                FilterType="LowercaseLetters,UppercaseLetters,Numbers,Custom" ValidChars="!@$%^&*()_+|}{][':;?><,."
                                InvalidChars="/\">
                            </asp:FilteredTextBoxExtender>
                            <asp:Button ID="btnxl" runat="server" Text="Export Excel" Font-Bold="True" Font-Names="Book Antiqua"
                                Font-Size="Medium" OnClick="btnxl_Click" />
                            <asp:Button ID="btnprintmaster" runat="server" Text="Print" OnClick="btnprintmaster_Click"
                                Font-Names="Book Antiqua" Font-Size="Medium" Font-Bold="true" />
                            <asp:Button ID="btnPrint" runat="server" Text="Direct Print" OnClientClick="return PrintPanel();"
                                Font-Names="Book Antiqua" Font-Size="Medium" Font-Bold="true" Height="35px" CssClass="textbox textbox1" />
                            <NEW:NEWPrintMater runat="server" ID="Printcontrol" Visible="false" />
                            <asp:CheckBox ID="cbhourwise" runat="server" Text="Hourwise" Font-Bold="True" Font-Names="Book Antiqua"
                                Font-Size="Medium" Visible="False" />
                            <br />
                            <asp:CheckBox ID="cbdaywise" runat="server" Text="Daywise" Font-Bold="True" Font-Names="Book Antiqua"
                                Font-Size="Medium" Visible="False" />
                            <asp:Label ID="norecordlbl" runat="server" Text=""></asp:Label>
                            <asp:Label ID="errmsg" runat="server" Text=""></asp:Label>
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
                    <asp:PostBackTrigger ControlID="btnprintmaster" />
                    <asp:PostBackTrigger ControlID="btnPrint" />
                    <asp:PostBackTrigger ControlID="btngo" />
                </Triggers>
            </asp:UpdatePanel>
        </center>
        <%--progressBar for go--%>
        <%--  <center>
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
        <asp:UpdatePanel ID="UpdatePanel3" runat="server">
            <ContentTemplate>
                <div style="height: 1px; width: 1px; overflow: auto;">
                    <div id="contentDiv" runat="server" style="height: auto; width: 1344px;" visible="false">
                    </div>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
    </body>
    </html>
</asp:Content>

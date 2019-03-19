<%@ Page Title="Student Condonation" Language="C#" MasterPageFile="~/AttendanceMOD/AttendanceSubSiteMaster.master"
    AutoEventWireup="true" CodeFile="Condonation.aspx.cs" Inherits="Condonation"
    EnableEventValidation="false" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Src="~/Usercontrols/NewPrintMaster.ascx" TagName="NEWPrintMater" TagPrefix="NEW" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <link href="~/Styles/css/Registration.css" rel="stylesheet" type="text/css" />
    <%--<script src="//ajax.googleapis.com/ajax/libs/jquery/1.11.1/jquery.min.js"></script>--%>
    <script src="../Scripts/jquery-latest.min.js" type="text/javascript"></script>
    <link href="~/Styles/css/Commoncss.css" rel="Stylesheet" type="text/css" />
    <style type="text/css">
        .table2
        {
            border: 1px solid #0CA6CA;
            border-radius: 10px;
            background-color: #0CA6CA;
            box-shadow: 0px 0px 8px #7bc1f7;
            padding: 3px;
        }
        .noresize
        {
            resize: none;
        }
    </style>
    <script type="text/javascript">
        function display() {
            document.getElementById('MainContent_errmsg').innerHTML = "";
        }

        function SelectAll(id) {
            var grid = document.getElementById("<%= Showgrid1.ClientID %>");
            var cell;

            if (grid.rows.length > 0) {

                for (i = 1; i < grid.rows.length; i++) {

                    cell = grid.rows[i].cells[1];
                    if (cell != null) {

                        for (j = 0; j < cell.childNodes.length; j++) {

                            if (cell.childNodes[j].type == "checkbox") {

                                cell.childNodes[j].checked = document.getElementById(id).checked;

                            }
                        }
                    }
                }
            }
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
        function SelLedgers1() {
            var chkSelAll = document.getElementById("<%=chkselecall.ClientID %>");
            var tbl = document.getElementById("<%=Showgrid1.ClientID %>");
            var gridViewControls = tbl.getElementsByTagName("input");

            for (var i = 1; i < (tbl.rows.length); i++) {
                var chkSelectid = document.getElementById('MainContent_Showgrid1_selectchk_' + i.toString());

                if (chkSelAll.checked == false) {
                    chkSelectid.checked = false;
                } else {
                    chkSelectid.checked = true;
                }
            }
        }
       
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <center>
        <span class="fontstyleheader" style="color: Green; margin: 0px; margin-bottom: 10px;
            margin-top: 10px;">Student Condonation</span>
    </center>
    <center>
        <%--  <asp:UpdatePanel ID="upd" runat="server">
            <ContentTemplate>--%>
        <table style="position: relative; margin: 0px; margin-bottom: 10px; margin-top: 10px;
            background-color: #0CA6CA; width: auto; border-radius: 10px;">
            <tr>
                <td>
                    <asp:Label ID="lblstream" runat="server" Text="Stream" Font-Bold="True" Font-Names="Book Antiqua"
                        Font-Size="Medium"></asp:Label>
                </td>
                <td>
                    <asp:DropDownList ID="ddlstream" runat="server" OnSelectedIndexChanged="ddlstream_SelectedIndexChanged"
                        AutoPostBack="True" Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium"
                        Height="25px" Width="85px">
                    </asp:DropDownList>
                </td>
                <td>
                    <asp:Label ID="Label1" runat="server" Text="Batch" Font-Bold="True" Font-Names="Book Antiqua"
                        Font-Size="Medium"></asp:Label>
                </td>
                <td>
                    <%--  <asp:DropDownList ID="ddlbatch" runat="server" OnSelectedIndexChanged="ddlbatch_SelectedIndexChanged"
                                AutoPostBack="True" Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium"
                                Height="25px" Width="69px">
                            </asp:DropDownList>--%>
                    <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                        <ContentTemplate>
                            <asp:TextBox ID="txtbatch" runat="server" Height="20px" CssClass="Dropdown_Txt_Box"
                                ReadOnly="true" Width="120px" Style="font-family: 'Book Antiqua';" Font-Bold="True"
                                Font-Names="Book Antiqua" Font-Size="Medium">---Select---</asp:TextBox>
                            <asp:Panel ID="pbatch" runat="server" CssClass="multxtpanel" Width="125px">
                                <asp:CheckBox ID="chkbatch" runat="server" Width="100px" Font-Bold="True" OnCheckedChanged="chkbatch_ChekedChange"
                                    Font-Names="Book Antiqua" Font-Size="Medium" Text="Select All" AutoPostBack="True" />
                                <asp:CheckBoxList ID="chklsbatch" runat="server" Font-Size="Medium" AutoPostBack="True"
                                    Width="100px" Height="58px" Font-Bold="True" Font-Names="Book Antiqua" OnSelectedIndexChanged="chklstbatch_SelectedIndexChanged">
                                </asp:CheckBoxList>
                            </asp:Panel>
                            <asp:PopupControlExtender ID="PopupControlExtender1" runat="server" TargetControlID="txtbatch"
                                PopupControlID="pbatch" Position="Bottom">
                            </asp:PopupControlExtender>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </td>
                <td>
                    <asp:Label ID="Label2" runat="server" Text="Degree" Font-Bold="True" Font-Names="Book Antiqua"
                        Font-Size="Medium"></asp:Label>
                </td>
                <td>
                    <asp:DropDownList ID="ddldegree" runat="server" OnSelectedIndexChanged="ddldegree_SelectedIndexChanged"
                        AutoPostBack="True" Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium"
                        Height="25px" Visible="false">
                    </asp:DropDownList>
                    <asp:UpdatePanel ID="UpnlDegree" runat="server">
                        <ContentTemplate>
                            <asp:TextBox ID="txtDegree" Width=" 100px" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                Font-Size="Medium" CssClass="textbox  txtheight2" ReadOnly="true">-- Select --</asp:TextBox>
                            <asp:Panel ID="pnlDegree" runat="server" CssClass="multxtpanel">
                                <%--Height="200px"--%>
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
                    <asp:Label ID="lblbranch" runat="server" Text="Branch" Font-Bold="True" Font-Names="Book Antiqua"
                        Font-Size="Medium"></asp:Label>
                </td>
                <td>
                    <asp:DropDownList ID="ddlbranch" runat="server" OnSelectedIndexChanged="ddlbranch_SelectedIndexChanged"
                        AutoPostBack="True" Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium"
                        Height="25px" Width="271px" Visible="false">
                    </asp:DropDownList>
                    <asp:UpdatePanel ID="upnlBranch" runat="server">
                        <ContentTemplate>
                            <asp:TextBox ID="txtBranch" Width=" 100px" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                Font-Size="Medium" CssClass="textbox  txtheight2" ReadOnly="true">-- Select --</asp:TextBox>
                            <asp:Panel ID="pnlBranch" runat="server" CssClass="multxtpanel">
                                <%--Height="200px"--%>
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
                    <asp:Label ID="lblsem" runat="server" Text="Sem" Font-Bold="True" Font-Names="Book Antiqua"
                        Font-Size="Medium"></asp:Label>
                </td>
                <td>
                    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                        <ContentTemplate>
                            <asp:DropDownList ID="ddlsemester" runat="server" OnSelectedIndexChanged="ddlsemester_SelectedIndexChanged"
                                AutoPostBack="True" Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium"
                                Height="25px" Width="41px">
                            </asp:DropDownList>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </td>
                <td>
                    <asp:Label ID="lblsec" runat="server" Text="Sec" Font-Bold="True" Font-Names="Book Antiqua"
                        Font-Size="Medium"></asp:Label>
                </td>
                <td>
                    <asp:DropDownList ID="ddlsection" runat="server" AutoPostBack="True" Font-Bold="True"
                        Font-Names="Book Antiqua" Font-Size="Medium" Height="25px" Width="52px" OnSelectedIndexChanged="ddlsection_SelectedIndexChanged"
                        Visible="false">
                    </asp:DropDownList>
                    <asp:UpdatePanel ID="upnlSec" runat="server">
                        <ContentTemplate>
                            <asp:TextBox ID="txtSec" Width="80px" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                Font-Size="Medium" CssClass="textbox  txtheight2" ReadOnly="true">-- Select --</asp:TextBox>
                            <asp:Panel ID="pnlSec" runat="server" CssClass="multxtpanel">
                                <asp:CheckBox ID="chkSec" Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium"
                                    runat="server" Text="Select All" AutoPostBack="True" OnCheckedChanged="chkSec_CheckedChanged" />
                                <asp:CheckBoxList ID="cblSec" Font-Bold="True" Font-Size="Medium" Font-Names="Book Antiqua"
                                    runat="server" AutoPostBack="True" OnSelectedIndexChanged="cblSec_SelectedIndexChanged">
                                </asp:CheckBoxList>
                            </asp:Panel>
                            <asp:PopupControlExtender ID="popupExtSec" runat="server" TargetControlID="txtSec"
                                PopupControlID="pnlSec" Position="Bottom">
                            </asp:PopupControlExtender>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </td>
            </tr>
            <tr>
                <td colspan="12">
                    <table>
                        <tr>
                            <td>
                                <asp:Label ID="lblfdate" runat="server" Text="From Date" Font-Bold="True" Font-Names="Book Antiqua"
                                    Font-Size="Medium"></asp:Label>
                            </td>
                            <td>
                                <asp:UpdatePanel ID="UpdatePanel3" runat="server">
                                    <ContentTemplate>
                                        <asp:TextBox ID="txtfdate" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                            Font-Size="Medium" Width="80px" AutoPostBack="true" OnTextChanged="txtfdate_TextChanged"></asp:TextBox>
                                        <asp:CalendarExtender ID="CalendarExtender1" Format="d/MM/yyyy" TargetControlID="txtfdate"
                                            runat="server">
                                        </asp:CalendarExtender>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>
                            <td>
                                <asp:Label ID="lbltodate" runat="server" Text="To Date" Font-Bold="True" Font-Names="Book Antiqua"
                                    Font-Size="Medium"></asp:Label>
                            </td>
                            <td>
                                <asp:UpdatePanel ID="UpdatePanel4" runat="server">
                                    <ContentTemplate>
                                        <asp:TextBox ID="txttodate" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                            Font-Size="Medium" Width="80px" AutoPostBack="true" OnTextChanged="txttodate_TextChanged"></asp:TextBox>
                                        <asp:CalendarExtender ID="CalendarExtender2" Format="d/MM/yyyy" TargetControlID="txttodate"
                                            runat="server">
                                        </asp:CalendarExtender>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>
                            <td>
                                <asp:Label ID="lblattminper" runat="server" Text="Min.Attendance % For Eligibility To Write Exam"
                                    Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium" Width="360px"></asp:Label>
                            </td>
                            <td>
                                <%-- <asp:UpdatePanel ID="UpdatePanel3" runat="server">
                                            <ContentTemplate>--%>
                                <asp:TextBox ID="txtminattpercentage" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                    Font-Size="Medium" Width="80px" MaxLength="5"></asp:TextBox>
                                <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender3" runat="server" TargetControlID="txtminattpercentage"
                                    FilterType="Custom,Numbers" ValidChars="." />
                                <%--  </ContentTemplate>
                                        </asp:UpdatePanel>--%>
                            </td>
                            <td>
                                <%--<asp:UpdatePanel ID="UpGo" runat="server">
                                    <ContentTemplate>--%>
                                <asp:Button ID="btngo" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                    Font-Size="Medium" Text="Go" OnClick="btngo_Click" />
                                <%--  </ContentTemplate>
                                </asp:UpdatePanel>--%>
                            </td>
                            <td>
                                <asp:Button ID="btncondonationfee" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                    Font-Size="Medium" Text="Condonation Settings" OnClick="btncondonationfee_Click" />
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td colspan="12">
                    <table>
                        <tr>
                            <td>
                                <asp:Label ID="lblHrDaywise" runat="server" Text="Percentage" Font-Bold="True" Font-Names="Book Antiqua"
                                    Font-Size="Medium"></asp:Label>
                            </td>
                            <td>
                                <asp:RadioButtonList ID="rblHrDaywise" runat="server" AutoPostBack="true" OnSelectedIndexChanged="rblHrDaywise_OnSelectedIndexChanged"
                                    RepeatDirection="Horizontal" Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium">
                                    <asp:ListItem Selected="True">Daywise</asp:ListItem>
                                    <asp:ListItem>Hourwise</asp:ListItem>
                                </asp:RadioButtonList>
                            </td>
                            <td>
                                <asp:Label ID="lblPercDays" runat="server" Text="Percentage/Days" Font-Bold="True"
                                    Font-Names="Book Antiqua" Font-Size="Medium"></asp:Label>
                            </td>
                            <td>
                                <asp:RadioButtonList ID="rblPercDays" runat="server" AutoPostBack="true" OnSelectedIndexChanged="rblPercDays_OnSelectedIndexChanged"
                                    RepeatDirection="Horizontal" Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium">
                                    <asp:ListItem Selected="True">Percentage</asp:ListItem>
                                    <asp:ListItem>Days</asp:ListItem>
                                </asp:RadioButtonList>
                            </td>
                            <td>
                                <asp:Label ID="lblCondType" runat="server" Text="Condonation Type" Font-Bold="True"
                                    Font-Names="Book Antiqua" Font-Size="Medium"></asp:Label>
                            </td>
                            <td>
                                <asp:RadioButtonList ID="rblCondType" runat="server" RepeatDirection="Horizontal"
                                    AutoPostBack="true" OnSelectedIndexChanged="rblCondType_OnSelectedIndexChanged"
                                    Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium">
                                    <asp:ListItem Selected="True">With Fees</asp:ListItem>
                                    <asp:ListItem>Without Fees</asp:ListItem>
                                </asp:RadioButtonList>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td colspan="12">
                    <table>
                        <tr>
                            <td colspan="2">
                                <asp:CheckBox ID="chkShowDetails" AutoPostBack="true" Checked="false" Text="Show Details"
                                    runat="server" OnCheckedChanged="chkShowDetails_CheckedChanged" Font-Bold="true"
                                    Font-Names="Book Antiqua" Font-Size="Medium" />
                            </td>
                            <td colspan="2">
                                <asp:CheckBox ID="cbincround" runat="server" Text="Include Round" Font-Bold="True"
                                    Font-Names="Book Antiqua" Font-Size="Medium" />
                            </td>
                            <%-- <td colspan="2">
                                <asp:Label ID="Label12" runat="server" Text="NoOfRecords" Style="margin-left: 6px;">
                                </asp:Label>
                                <asp:TextBox ID="Txt_PageNo" runat="server" Style="width: 60px; margin-left: -4px;"
                                    CssClass="textbox txtheight2" AutoPostBack="true" OnTextChanged="Txt_PageNo_OnTextChanged"></asp:TextBox>
                            </td>--%>
                            <%-- <td>
                                <asp:Label ID="lblpge" runat="server" Text="Page No">
                                </asp:Label>
                            </td>
                            <td>
                                <asp:Button ID="btn_Previous" Width="30px" runat="server" CssClass="textbox btn2"
                                    Text="<<" OnClick="btn_Previous_Click" />
                            </td>--%>
                            <%--<td>
                                <%-- <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                                    <ContentTemplate>--%>
                            <%--  <asp:DropDownList ID="ddl_Txt_PageNo" runat="server" CssClass="textbox ddlstyle ddlheight3"
                                    Style="width: -2px; margin-left: -1px;" AutoPostBack="True" OnSelectedIndexChanged="ddl_Txt_PageNo_OnSelectedIndexChanged">
                                </asp:DropDownList>--%>
                            <%--</ContentTemplate>
                                </asp:UpdatePanel>--%>
                            <%--</td>--%>
                            <%-- <td>
                                <asp:Button ID="btn_Next" Style="width: 32px; margin-left: -3px;" runat="server"
                                    CssClass="textbox btn2" Text=">>" OnClick="btn_Next_Click" />
                            </td>--%>
                            <td>
                                <asp:CheckBox ID="chbcategory" runat="server" Text="Category Wise" />
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
        </table>
        <%--</ContentTemplate>
        </asp:UpdatePanel>--%>
    </center>
    <div>
        <center>
            <asp:Panel ID="pnlHeaderFilter" runat="server" BackImageUrl="~/Menu/Top Band-2.jpg"
                CssClass="table2" Height="22px" Width="850px" Style="margin-top: -0.1%; margin: 0px;
                margin-bottom: 10px; margin-top: 10px;">
                <asp:Label ID="lblFilter" Text="Column Order" runat="server" Font-Size="Medium" Font-Bold="True"
                    Font-Names="Book Antiqua" Style="margin-left: 0%;" />
                <asp:Image ID="imgFilter" runat="server" CssClass="cpimage" AlternateText="" ImageAlign="Right" />
            </asp:Panel>
        </center>
    </div>
    <%--<asp:UpdatePanel ID="UpdatePanel2" runat="server">
        <ContentTemplate>--%>
    <center>
        <asp:Panel ID="pnlColumnOrder" runat="server" BackImageUrl="~/Menu/Top Band-2.jpg"
            CssClass="table2" Width="850px" Style="margin: 0px; margin-bottom: 10px; margin-top: 10px;">
            <table>
                <tr>
                    <td>
                        <asp:CheckBox ID="chkColumnOrderAll" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                            Font-Size="Medium" Text="Select All" AutoPostBack="true" OnCheckedChanged="chkColumnOrderAll_CheckedChanged" />
                    </td>
                    <td>
                        <asp:LinkButton ID="lbtnRemoveAll" runat="server" Font-Size="X-Small" Height="16px"
                            Style="font-family: 'Book Antiqua'; color: #ffffff; font-weight: 700; font-size: small;
                            margin-left: -599px;" Visible="false" Width="111px" OnClick="lbtnRemoveAll_Click">Remove All</asp:LinkButton>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:TextBox ID="txtOrder" Visible="false" Width="837px" TextMode="MultiLine" CssClass="noresize"
                            AutoPostBack="true" runat="server" Enabled="false">
                        </asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:CheckBoxList ID="cblColumnOrder" runat="server" Height="43px" AutoPostBack="true"
                            Width="850px" Style="font-family: 'Book Antiqua'; font-weight: 700; font-size: medium;"
                            RepeatColumns="5" RepeatDirection="Horizontal" OnSelectedIndexChanged="cblColumnOrder_SelectedIndexChanged">
                            <asp:ListItem Value="0" Selected="True">S.No</asp:ListItem>
                            <asp:ListItem Value="1" Selected="True">Student Name</asp:ListItem>
                            <asp:ListItem Value="2" Selected="True">Present Percentage</asp:ListItem>
                            <asp:ListItem Value="3" Selected="True">Absent Percentage</asp:ListItem>
                            <asp:ListItem Value="4" Selected="True">Conducted Days</asp:ListItem>
                            <asp:ListItem Value="5" Selected="True">Present Days</asp:ListItem>
                            <asp:ListItem Value="6" Selected="True">Absent Days</asp:ListItem>
                            <asp:ListItem Value="7" Selected="True">Conducted Hours</asp:ListItem>
                            <asp:ListItem Value="8" Selected="True">Present Hours</asp:ListItem>
                            <asp:ListItem Value="9" Selected="True">Absent Hours</asp:ListItem>
                            <asp:ListItem Value="10" Selected="True">Fine Amount</asp:ListItem>
                            <asp:ListItem Value="11" Selected="True">Remarks</asp:ListItem>
                        </asp:CheckBoxList>
                    </td>
                </tr>
            </table>
        </asp:Panel>
    </center>
    <asp:CollapsiblePanelExtender ID="cpecolumnorder" runat="server" TargetControlID="pnlColumnOrder"
        CollapseControlID="pnlHeaderFilter" ExpandControlID="pnlHeaderFilter" Collapsed="true"
        TextLabelID="lblFilter" CollapsedSize="0" ImageControlID="imgFilter" CollapsedImage="~/images/right.jpeg"
        ExpandedImage="~/images/down.jpeg">
    </asp:CollapsiblePanelExtender>
    <%-- </ContentTemplate>
    </asp:UpdatePanel>--%>
    <%-- <asp:UpdatePanel ID="UpdatePanel3" runat="server">
        <ContentTemplate>--%>
    <asp:Label ID="errmsg" runat="server" Text="No Record(s) Found" ForeColor="Red" Visible="False"
        Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium" Style="margin: 0px;
        margin-bottom: 10px; margin-top: 10px;"></asp:Label>
    <%-- </ContentTemplate>
    </asp:UpdatePanel>--%>
    <%--<asp:UpdatePanel ID="updMain" runat="server">
                <ContentTemplate>--%>
    <%-- <asp:UpdatePanel ID="UpdatePanel11" runat="server">
        <ContentTemplate>--%>
    <asp:Panel ID="PWrite" runat="server" BackImageUrl="~/Menu/Top Band-2.jpg" Style="margin: 0px;
        position: relative; margin-bottom: 10px; margin-top: 10px; height: 20px; width: 95%">
        <center>
            <asp:Label ID="lblwriteexam" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                Font-Size="Medium" ForeColor="White" Text="To Write Exam"></asp:Label>
        </center>
    </asp:Panel>
    <%-- </ContentTemplate>
    </asp:UpdatePanel>--%>
    <center>
        <%--<asp:UpdatePanel ID="UpdatePanel12" runat="server">
            <ContentTemplate>--%>
        <table style="margin: 0px; margin-bottom: 10px; margin-top: 10px;">
            <tr>
                <td align="center" colspan="5">
                    <div id="divMainContents" runat="server" style="display: table; margin: 0px; height: auto;
                        margin-bottom: 20px; margin-top: 10px; position: relative; width: auto; text-align: left;">
                        <asp:GridView ID="Showgrid" runat="server" Visible="false" HeaderStyle-ForeColor="Black"
                            HeaderStyle-BackColor="#0CA6CA" ShowHeader="false" Font-Names="Book Antiqua"
                            ShowHeaderWhenEmpty="true" OnRowDataBound="Showgrid_OnRowDataBound">
                            <%--   OnRowDataBound="Showgrid_OnRowDataBound"--%>
                            <HeaderStyle BackColor="#0CA6CA" Font-Bold="true" ForeColor="Black" Font-Size="Medium"
                                Font-Names="Book Antiqua" />
                            <FooterStyle BackColor="White" ForeColor="#333333" />
                            <PagerStyle BackColor="#336666" HorizontalAlign="Center" />
                            <RowStyle ForeColor="#333333" />
                            <SelectedRowStyle BackColor="#339966" Font-Bold="True" />
                        </asp:GridView>
                    </div>
                    <div>
                        <asp:Label ID="Label_error_Showgrid" runat="server" ForeColor="Red" Visible="False"
                            Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium" Style="margin: 0px;
                            margin-bottom: 10px; margin-top: 10px;"></asp:Label>
                    </div>
                </td>
            </tr>
            <tr style="margin: 0px; margin-bottom: 10px; margin-top: 10px;">
                <td align="center" colspan="5">
                    <table style="margin: 0px; margin-bottom: 10px; margin-top: 10px;">
                        <tr>
                            <td>
                                <asp:Label ID="lblrptname" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                    Font-Size="Medium" Text="Report Name"></asp:Label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtexcelname" runat="server" Height="20px" Width="180px" Style="font-family: 'Book Antiqua'"
                                    Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium" onkeypress="display()"></asp:TextBox>
                                <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender2" runat="server" TargetControlID="txtexcelname"
                                    FilterType="LowercaseLetters,UppercaseLetters,Numbers,Custom" ValidChars=",. ()[]{}"
                                    InvalidChars="/\">
                                </asp:FilteredTextBoxExtender>
                            </td>
                            <td>
                                <asp:Button ID="btnxl" runat="server" Text="Export Excel" Font-Bold="True" Font-Names="Book Antiqua"
                                    Font-Size="Medium" OnClick="btnxl_Click" />
                            </td>
                            <td>
                                <asp:Button ID="btnprintmaster" runat="server" Text="Print" OnClick="btnprintmaster_Click"
                                    Font-Names="Book Antiqua" Font-Size="Medium" Font-Bold="true" />
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
        </table>
        <%--</ContentTemplate>
        </asp:UpdatePanel>--%>
    </center>
    <center>
        <%-- <asp:UpdatePanel ID="UpdatePanel4" runat="server">
            <ContentTemplate>--%>
        <asp:Panel ID="PCondonation" runat="server" BackImageUrl="~/Menu/Top Band-2.jpg"
            Style="margin: 0px; position: relative; margin-bottom: 10px; margin-top: 10px;
            height: 20px; width: 95%">
            <center>
                <asp:Label ID="lblcondonation" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                    Font-Size="Medium" ForeColor="White" Text="To Apply Condonation"></asp:Label>
            </center>
        </asp:Panel>
        <%--  </ContentTemplate>
        </asp:UpdatePanel>--%>
    </center>
    <center>
        <%-- <asp:UpdatePanel ID="UpdatePanel5" runat="server">
            <ContentTemplate>--%>
        <table style="margin: 0px; margin-bottom: 10px; margin-top: 10px;">
            <tr>
                <td align="center" colspan="5">
                    <span style="padding-right: 100px;">
                        <asp:CheckBox ID="chkselecall" runat="server" Text="SelectAll" Visible="false" Style="margin-left: -520px;"
                            onchange="return SelLedgers1();" />
                    </span>
                    <div id="div1" runat="server" style="display: table; margin: 0px; height: auto; margin-bottom: 20px;
                        margin-top: 10px; position: relative; width: auto; text-align: left;">
                        <asp:GridView ID="Showgrid1" runat="server" Visible="false" HeaderStyle-ForeColor="Black"
                            HeaderStyle-BackColor="#0CA6CA" AutoGenerateColumns="true" Font-Names="Book Antiqua"
                            ShowHeaderWhenEmpty="true" OnRowDataBound="Showgrid1_OnRowDataBound">
                            <%--   OnRowDataBound="Showgrid1_OnRowDataBound"--%>
                            <Columns>
                                <asp:TemplateField HeaderText="SNo" HeaderStyle-BackColor="#0CA6CA" HeaderStyle-HorizontalAlign="center"
                                    HeaderStyle-Width="30px">
                                    <ItemTemplate>
                                        <center>
                                            <asp:Label ID="lbl_snum" runat="server" Visible="true" Text='<%#Eval("SNo") %>'>
                                            </asp:Label>
                                        </center>
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="center" />
                                </asp:TemplateField>
                                <asp:TemplateField>
                                    <ItemTemplate>
                                        <asp:CheckBox ID="selectchk" runat="server" />
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Center" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Edit" HeaderStyle-BackColor="#0CA6CA" HeaderStyle-HorizontalAlign="Center"
                                    HeaderStyle-Width="30px">
                                    <ItemTemplate>
                                        <asp:Button ID="editbtn" runat="server" Text="Edit" OnClick="btnedit_Click" />
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Center" />
                                </asp:TemplateField>
                            </Columns>
                            <HeaderStyle BackColor="#0CA6CA" Font-Bold="true" ForeColor="Black" Font-Size="Medium"
                                Font-Names="Book Antiqua" />
                            <FooterStyle BackColor="White" ForeColor="#333333" />
                            <PagerStyle BackColor="#336666" HorizontalAlign="Center" />
                            <RowStyle ForeColor="#333333" />
                            <SelectedRowStyle BackColor="#339966" Font-Bold="True" />
                        </asp:GridView>
                    </div>
                    <div>
                        <asp:Label ID="Label_error_Showgrid1" runat="server" ForeColor="Red" Visible="False"
                            Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium" Style="margin: 0px;
                            margin-bottom: 10px; margin-top: 10px;"></asp:Label>
                    </div>
                </td>
            </tr>
            <tr style="margin: 0px; margin-bottom: 10px; margin-top: 10px;">
                <td align="center" colspan="5">
                    <table style="margin: 0px; margin-bottom: 10px; margin-top: 10px;">
                        <tr>
                            <td>
                                <asp:Label ID="lblconrptname" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                    Font-Size="Medium" Text="Report Name"></asp:Label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtconexcel" runat="server" Height="20px" Width="180px" Style="font-family: 'Book Antiqua'"
                                    Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium" onkeypress="display()"></asp:TextBox>
                                <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" runat="server" TargetControlID="txtconexcel"
                                    FilterType="LowercaseLetters,UppercaseLetters,Numbers,Custom" ValidChars=",. ()[]{}"
                                    InvalidChars="/\">
                                </asp:FilteredTextBoxExtender>
                            </td>
                            <td>
                                <asp:Button ID="btnconxl" runat="server" Text="Export Excel" Font-Bold="True" Font-Names="Book Antiqua"
                                    Font-Size="Medium" OnClick="btnconxl_Click" />
                            </td>
                            <td>
                                <asp:Button ID="btnconprint" runat="server" Text="Print" OnClick="btnconprint_Click"
                                    Font-Names="Book Antiqua" Font-Size="Medium" Font-Bold="true" />
                            </td>
                            <td>
                                <asp:DropDownList ID="ddlCondonationReport" runat="server" Visible="false">
                                    <asp:ListItem Text="Condonation Report Format 1" Value="0"></asp:ListItem>
                                    <asp:ListItem Text="Condonation Report Format 2" Value="1"></asp:ListItem>
                                </asp:DropDownList>
                            </td>
                            <td>
                                <asp:Button ID="btnCondonationReport" runat="server" Text="Condonation Print" OnClick="btnCondonationReport_Click"
                                    Font-Names="Book Antiqua" Font-Size="Medium" Font-Bold="true" />
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr style="margin: 0px; margin-bottom: 10px; margin-top: 10px;">
                <td align="center" colspan="5">
                    <asp:Label ID="lblErrCondo" Visible="false" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                        Font-Size="Medium" Text="" ForeColor="Red" Style="margin-top: 10px; margin-bottom: 8px;"></asp:Label>
                </td>
            </tr>
        </table>
        <%--</ContentTemplate>
        </asp:UpdatePanel>--%>
    </center>
    <center>
        <%--<asp:UpdatePanel ID="UpdatePanel6" runat="server">
            <ContentTemplate>--%>
        <asp:Panel ID="PNotEligible" runat="server" BackImageUrl="~/Menu/Top Band-2.jpg"
            Style="margin: 0px; position: relative; margin-bottom: 10px; margin-top: 10px;
            height: 20px; width: 95%">
            <center>
                <asp:Label ID="lblnoteligble" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                    Font-Size="Medium" ForeColor="White" Text="Not Eligible"></asp:Label>
            </center>
        </asp:Panel>
        <%-- </ContentTemplate>
        </asp:UpdatePanel>--%>
    </center>
    <center>
        <%--<asp:UpdatePanel ID="UpdatePanel7" runat="server">
            <ContentTemplate>--%>
        <table style="margin: 0px; margin-bottom: 10px; margin-top: 10px;">
            <tr>
                <td align="center" colspan="5">
                    <div id="div2" runat="server" style="display: table; margin: 0px; height: auto; margin-bottom: 20px;
                        margin-top: 10px; position: relative; width: auto; text-align: left;">
                        <asp:GridView ID="Showgrid2" runat="server" Visible="false" HeaderStyle-ForeColor="Black"
                            OnRowDataBound="Showgrid2_onRowDataBound" OnDataBound="Showgrid2_DataBound" HeaderStyle-BackColor="#0CA6CA"
                            AutoGenerateColumns="true" Font-Names="Book Antiqua" ShowHeaderWhenEmpty="true">
                            <%--   OnRowDataBound="Showgrid1_OnRowDataBound"--%>
                            <Columns>
                                <asp:TemplateField HeaderText="SNo" HeaderStyle-BackColor="#0CA6CA" HeaderStyle-HorizontalAlign="center"
                                    HeaderStyle-Width="30px">
                                    <ItemTemplate>
                                        <center>
                                            <asp:Label ID="lbl_sno" runat="server" Visible="true" Text='<%#Eval("SNo") %>'>
                                            </asp:Label>
                                        </center>
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="center" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Eligible For Next Semester" HeaderStyle-BackColor="#0CA6CA"
                                    HeaderStyle-HorizontalAlign="Center">
                                    <ItemTemplate>
                                        <center>
                                            <asp:DropDownList ID="ddl_Eligible" runat="server" CssClass="textbox ddlheight3"
                                                Width="110px" Visible="true">
                                            </asp:DropDownList>
                                            <asp:Label ID="lbl_Eligible" runat="server" Visible="false" Text='<%#Eval("Eligible For Next Semester") %>'>
                                            </asp:Label>
                                        </center>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                            <HeaderStyle BackColor="#0CA6CA" Font-Bold="true" ForeColor="Black" Font-Size="Medium"
                                Font-Names="Book Antiqua" />
                            <FooterStyle BackColor="White" ForeColor="#333333" />
                            <PagerStyle BackColor="#336666" HorizontalAlign="Center" />
                            <RowStyle ForeColor="#333333" />
                            <SelectedRowStyle BackColor="#339966" Font-Bold="True" />
                        </asp:GridView>
                    </div>
                    <div>
                        <asp:Label ID="Label_error_Showgrid2" runat="server" ForeColor="Red" Visible="False"
                            Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium" Style="margin: 0px;
                            margin-bottom: 10px; margin-top: 10px;"></asp:Label>
                    </div>
                </td>
            </tr>
            <tr style="margin: 0px; margin-bottom: 10px; margin-top: 10px;">
                <td align="center" colspan="5">
                    <table style="margin: 0px; margin-bottom: 10px; margin-top: 10px;">
                        <tr>
                            <td>
                                <asp:Label ID="lblnotelexclname" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                    Font-Size="Medium" Text="Report Name"></asp:Label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtnoteliexcel" runat="server" Height="20px" Width="180px" Style="font-family: 'Book Antiqua'"
                                    Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium" onkeypress="display()"></asp:TextBox>
                                <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender4" runat="server" TargetControlID="txtnoteliexcel"
                                    FilterType="LowercaseLetters,UppercaseLetters,Numbers,Custom" ValidChars=",. ()[]{}"
                                    InvalidChars="/\">
                                </asp:FilteredTextBoxExtender>
                            </td>
                            <td>
                                <asp:Button ID="btnnoteliexcel" runat="server" Text="Export Excel" Font-Bold="True"
                                    Font-Names="Book Antiqua" Font-Size="Medium" OnClick="btnnoteliexcel_Click" />
                            </td>
                            <td>
                                <asp:Button ID="btnnoteliprint" runat="server" Text="Print" OnClick="btnnoteliprint_Click"
                                    Font-Names="Book Antiqua" Font-Size="Medium" Font-Bold="true" />
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr style="margin: 0px; margin-bottom: 10px; margin-top: 10px;">
                <td align="left" colspan="5">
                    <asp:Button ID="btnsave" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                        Font-Size="Medium" Text="Save" OnClick="btnsave_Click" />
                </td>
            </tr>
        </table>
        <%-- </ContentTemplate>
        </asp:UpdatePanel>--%>
    </center>
    <center>
        <%--  <asp:UpdatePanel ID="UpdatePanel8" runat="server">
            <ContentTemplate>--%>
        <div id="panelrollnopop" runat="server" visible="false" style="height: 100%; z-index: 2000;
            background-color: rgba(54, 25, 25, .2); position: absolute; top: 0; left: 0px;
            width: 100%;">
            <center>
                <asp:Panel ID="panelrollnopop1" runat="server" BorderColor="Black" BackColor="White"
                    Visible="true" BorderWidth="2px" Style="margin-left: auto; margin-right: auto;
                    margin-top: 150px; margin-bottom: auto; width: auto;">
                    <div class="PopupHeaderrstud2" id="Div3" style="text-align: center; font-family: MS Sans Serif;
                        font-size: Small; font-weight: bold;">
                        <caption style="top: 30px; border-style: solid; border-color: Black; position: absolute;
                            left: 200px">
                            Student Condonation Fee Settings
                            <br />
                            <br />
                        </caption>
                        <table style="height: 100px; width: 100%; margin: 0px; border: 0px; padding: 0px;">
                            <tr>
                                <td colspan="3">
                                    <div id="Div4" runat="server" visible="false" style="width: 600px; background-color: White;
                                        border-radius: 10px;">
                                        <asp:GridView ID="GridView1" Width="600px" runat="server" ShowFooter="false" AutoGenerateColumns="false"
                                            Font-Names="Book Antiqua" toGenerateColumns="false" ShowHeaderWhenEmpty="true"
                                            OnRowDataBound="GvUsersRowDataBound">
                                            <Columns>
                                                <asp:TemplateField HeaderText="S.No" HeaderStyle-BackColor="#0CA6CA" HeaderStyle-HorizontalAlign="Center"
                                                    HeaderStyle-Width="30px">
                                                    <ItemTemplate>
                                                        <%#Container.DataItemIndex+1 %>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="From %" HeaderStyle-BackColor="#0CA6CA" HeaderStyle-HorizontalAlign="Center"
                                                    HeaderStyle-Width="100px">
                                                    <ItemTemplate>
                                                        <center>
                                                            <asp:TextBox ID="txt_FromPer" runat="server" CssClass="textbox txtheight" Text='<%#Eval("FromPer") %>'
                                                                Height="15px" Width="100px" Style="text-align: right;"></asp:TextBox></center>
                                                        <asp:FilteredTextBoxExtender ID="filterextenderFromper" runat="server" TargetControlID="txt_FromPer"
                                                            FilterType="Numbers,Custom" ValidChars=".">
                                                        </asp:FilteredTextBoxExtender>
                                                        <asp:RangeValidator ID="percentageRangeValidator" runat="server" ControlToValidate="txt_FromPer"
                                                            Display="Dynamic" ErrorMessage="Please Enter From % / To % Must Be Between 0 and 100"
                                                            MaximumValue="100.00" MinimumValue="0.00" Type="Double" ForeColor="Red"></asp:RangeValidator>
                                                    </ItemTemplate>
                                                    <ItemStyle />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="To %" HeaderStyle-BackColor="#0CA6CA" HeaderStyle-HorizontalAlign="Center"
                                                    HeaderStyle-Width="100px">
                                                    <ItemTemplate>
                                                        <center>
                                                            <asp:TextBox ID="txt_ToPer" runat="server" CssClass="  textbox txtheight" Text='<%#Eval("ToPer") %>'
                                                                Height="15px" Width="100px" Style="text-align: right;"></asp:TextBox></center>
                                                        <asp:FilteredTextBoxExtender ID="filterextenderToper" runat="server" TargetControlID="txt_ToPer"
                                                            FilterType="Numbers,Custom" ValidChars=".">
                                                        </asp:FilteredTextBoxExtender>
                                                        <asp:RangeValidator ID="percentageRangeValidator1" runat="server" ControlToValidate="txt_ToPer"
                                                            Display="Dynamic" ErrorMessage="Please Enter From % / To % Must Be Between 0 and 100"
                                                            MaximumValue="100.00" MinimumValue="0.00" Type="Double" ForeColor="Red"></asp:RangeValidator>
                                                    </ItemTemplate>
                                                    <ItemStyle />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Day From" HeaderStyle-BackColor="#0CA6CA" HeaderStyle-HorizontalAlign="Center"
                                                    HeaderStyle-Width="100px">
                                                    <ItemTemplate>
                                                        <center>
                                                            <asp:TextBox ID="txt_DayFrom" runat="server" CssClass="textbox txtheight" Text='<%#Eval("DayFrom") %>'
                                                                Height="15px" Width="100px" Style="text-align: right;"></asp:TextBox></center>
                                                        <asp:FilteredTextBoxExtender ID="filterextenderDayFrom" runat="server" TargetControlID="txt_DayFrom"
                                                            FilterType="Numbers,Custom" ValidChars=".">
                                                        </asp:FilteredTextBoxExtender>
                                                    </ItemTemplate>
                                                    <ItemStyle />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Day To" HeaderStyle-BackColor="#0CA6CA" HeaderStyle-HorizontalAlign="Center"
                                                    HeaderStyle-Width="100px">
                                                    <ItemTemplate>
                                                        <center>
                                                            <asp:TextBox ID="txt_DayTo" runat="server" CssClass="  textbox txtheight" Text='<%#Eval("DayTo") %>'
                                                                Height="15px" Width="100px" Style="text-align: right;"></asp:TextBox></center>
                                                        <asp:FilteredTextBoxExtender ID="filterextenderDayTo" runat="server" TargetControlID="txt_DayTo"
                                                            FilterType="Numbers,Custom" ValidChars=".">
                                                        </asp:FilteredTextBoxExtender>
                                                    </ItemTemplate>
                                                    <ItemStyle />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Fine Amount" HeaderStyle-BackColor="#0CA6CA" HeaderStyle-HorizontalAlign="Center"
                                                    HeaderStyle-Width="100px">
                                                    <ItemTemplate>
                                                        <center>
                                                            <asp:TextBox ID="txt_FineAmount" runat="server" CssClass="  textbox txtheight" Text='<%#Eval("FineAmount") %>'
                                                                Height="15px" Width="100px" Style="text-align: right;"></asp:TextBox></center>
                                                        <asp:FilteredTextBoxExtender ID="filterextenderFineAmount" runat="server" TargetControlID="txt_FineAmount"
                                                            FilterType="Numbers,Custom">
                                                        </asp:FilteredTextBoxExtender>
                                                    </ItemTemplate>
                                                    <ItemStyle />
                                                </asp:TemplateField>
                                            </Columns>
                                            <HeaderStyle BackColor="#0CA6CA" ForeColor="Black" />
                                        </asp:GridView>
                                    </div>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="3">
                                    <table style="height: auto; width: auto;">
                                        <tr>
                                            <td colspan="4" align="right">
                                                <asp:RadioButtonList ID="rblCOndonationType" runat="server" RepeatDirection="Horizontal"
                                                    Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium" AutoPostBack="true"
                                                    OnSelectedIndexChanged="rblCOndonationType_OnSelectedIndexChanged">
                                                    <asp:ListItem Selected="True">With Fees</asp:ListItem>
                                                    <asp:ListItem>Without Fees</asp:ListItem>
                                                </asp:RadioButtonList>
                                            </td>
                                            <td colspan="3" align="left">
                                                <asp:RadioButtonList ID="rblDayOrPercerntage" runat="server" RepeatDirection="Horizontal"
                                                    Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium" AutoPostBack="true"
                                                    OnSelectedIndexChanged="rblDayOrPercerntage_OnSelectedIndexChanged">
                                                    <asp:ListItem Value="0" Selected="True">Percentage</asp:ListItem>
                                                    <asp:ListItem Value="1">Day</asp:ListItem>
                                                </asp:RadioButtonList>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:Label ID="lblpstream" runat="server" Text="Stream" Font-Bold="True" Font-Names="Book Antiqua"
                                                    Font-Size="Medium"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:DropDownList ID="ddlpstream" runat="server" OnSelectedIndexChanged="ddlpstream_SelectedIndexChanged"
                                                    AutoPostBack="True" Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium"
                                                    Height="25px" Width="85px">
                                                </asp:DropDownList>
                                            </td>
                                            <td>
                                                <asp:Label ID="lblpcourse" runat="server" Text="Education Level" Font-Bold="True"
                                                    Font-Names="Book Antiqua" Font-Size="Medium"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:DropDownList ID="ddlpcourse" runat="server" OnSelectedIndexChanged="ddlpcourse_SelectedIndexChanged"
                                                    AutoPostBack="True" Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium"
                                                    Height="25px" Width="69px">
                                                </asp:DropDownList>
                                            </td>
                                            <td colspan="5">
                                                <asp:Label ID="lblheader" runat="server" Text="Header" Font-Bold="True" Font-Names="Book Antiqua"
                                                    Font-Size="Medium"></asp:Label>
                                                <%-- </td>
                                                    <td>--%>
                                                <asp:DropDownList ID="ddlheader" runat="server" OnSelectedIndexChanged="ddlheader_SelectedIndexChanged"
                                                    AutoPostBack="True" Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium"
                                                    Width="200px" Height="25px">
                                                </asp:DropDownList>
                                                <%-- </td>
                                                    <td>--%>
                                                <asp:Label ID="lblledger" runat="server" Text="Ledger" Font-Bold="True" Font-Names="Book Antiqua"
                                                    Font-Size="Medium"></asp:Label>
                                                <%--</td>
                                                    <td>--%>
                                                <asp:DropDownList ID="ddlledger" runat="server" OnSelectedIndexChanged="ddlledger_SelectedIndexChanged"
                                                    AutoPostBack="True" Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium"
                                                    Width="200px" Height="25px">
                                                </asp:DropDownList>
                                                <%-- </td>
                                                    <td>--%>
                                                <asp:Button ID="btnpgo" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                                    Font-Size="Medium" Text="Go" OnClick="btnpgo_Click" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="7">
                                                <table>
                                                    <tr>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="3">
                                    <center>
                                        <center>
                                            <div id="DivAddcondonationfees" runat="server" visible="false" style="width: 600px;
                                                background-color: White; border-radius: 10px;">
                                                <asp:GridView ID="grdAddcondonationfees" Width="600px" runat="server" ShowFooter="false"
                                                    AutoGenerateColumns="false" Font-Names="Book Antiqua" toGenerateColumns="false"
                                                    ShowHeaderWhenEmpty="true" OnRowDataBound="GvUsersRowDataBound">
                                                    <Columns>
                                                        <asp:TemplateField HeaderText="S.No" HeaderStyle-BackColor="#0CA6CA" HeaderStyle-HorizontalAlign="Center"
                                                            HeaderStyle-Width="30px">
                                                            <ItemTemplate>
                                                                <%#Container.DataItemIndex+1 %>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="From %" HeaderStyle-BackColor="#0CA6CA" HeaderStyle-HorizontalAlign="Center"
                                                            HeaderStyle-Width="100px">
                                                            <ItemTemplate>
                                                                <center>
                                                                    <asp:TextBox ID="txt_FromPer" runat="server" CssClass="textbox txtheight" Text='<%#Eval("FromPer") %>'
                                                                        Height="15px" Width="100px" Style="text-align: right;"></asp:TextBox></center>
                                                                <asp:FilteredTextBoxExtender ID="filterextenderFromper" runat="server" TargetControlID="txt_FromPer"
                                                                    FilterType="Numbers,Custom" ValidChars=".">
                                                                </asp:FilteredTextBoxExtender>
                                                                <asp:RangeValidator ID="percentageRangeValidator" runat="server" ControlToValidate="txt_FromPer"
                                                                    Display="Dynamic" ErrorMessage="Please Enter From % / To % Must Be Between 0 and 100"
                                                                    MaximumValue="100.00" MinimumValue="0.00" Type="Double" ForeColor="Red"></asp:RangeValidator>
                                                            </ItemTemplate>
                                                            <ItemStyle />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="To %" HeaderStyle-BackColor="#0CA6CA" HeaderStyle-HorizontalAlign="Center"
                                                            HeaderStyle-Width="100px">
                                                            <ItemTemplate>
                                                                <center>
                                                                    <asp:TextBox ID="txt_ToPer" runat="server" CssClass="  textbox txtheight" Text='<%#Eval("ToPer") %>'
                                                                        Height="15px" Width="100px" Style="text-align: right;"></asp:TextBox></center>
                                                                <asp:FilteredTextBoxExtender ID="filterextenderToper" runat="server" TargetControlID="txt_ToPer"
                                                                    FilterType="Numbers,Custom" ValidChars=".">
                                                                </asp:FilteredTextBoxExtender>
                                                                <asp:RangeValidator ID="percentageRangeValidator1" runat="server" ControlToValidate="txt_ToPer"
                                                                    Display="Dynamic" ErrorMessage="Please Enter From % / To % Must Be Between 0 and 100"
                                                                    MaximumValue="100.00" MinimumValue="0.00" Type="Double" ForeColor="Red"></asp:RangeValidator>
                                                            </ItemTemplate>
                                                            <ItemStyle />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Day From" HeaderStyle-BackColor="#0CA6CA" HeaderStyle-HorizontalAlign="Center"
                                                            HeaderStyle-Width="100px">
                                                            <ItemTemplate>
                                                                <center>
                                                                    <asp:TextBox ID="txt_DayFrom" runat="server" CssClass="textbox txtheight" Text='<%#Eval("DayFrom") %>'
                                                                        Height="15px" Width="100px" Style="text-align: right;"></asp:TextBox></center>
                                                                <asp:FilteredTextBoxExtender ID="filterextenderDayFrom" runat="server" TargetControlID="txt_DayFrom"
                                                                    FilterType="Numbers,Custom" ValidChars=".">
                                                                </asp:FilteredTextBoxExtender>
                                                            </ItemTemplate>
                                                            <ItemStyle />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Day To" HeaderStyle-BackColor="#0CA6CA" HeaderStyle-HorizontalAlign="Center"
                                                            HeaderStyle-Width="100px">
                                                            <ItemTemplate>
                                                                <center>
                                                                    <asp:TextBox ID="txt_DayTo" runat="server" CssClass="  textbox txtheight" Text='<%#Eval("DayTo") %>'
                                                                        Height="15px" Width="100px" Style="text-align: right;"></asp:TextBox></center>
                                                                <asp:FilteredTextBoxExtender ID="filterextenderDayTo" runat="server" TargetControlID="txt_DayTo"
                                                                    FilterType="Numbers,Custom" ValidChars=".">
                                                                </asp:FilteredTextBoxExtender>
                                                            </ItemTemplate>
                                                            <ItemStyle />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Fine Amount" HeaderStyle-BackColor="#0CA6CA" HeaderStyle-HorizontalAlign="Center"
                                                            HeaderStyle-Width="100px">
                                                            <ItemTemplate>
                                                                <center>
                                                                    <asp:TextBox ID="txt_FineAmount" runat="server" CssClass="  textbox txtheight" Text='<%#Eval("FineAmount") %>'
                                                                        Height="15px" Width="100px" Style="text-align: right;"></asp:TextBox></center>
                                                                <asp:FilteredTextBoxExtender ID="filterextenderFineAmount" runat="server" TargetControlID="txt_FineAmount"
                                                                    FilterType="Numbers,Custom">
                                                                </asp:FilteredTextBoxExtender>
                                                            </ItemTemplate>
                                                            <ItemStyle />
                                                        </asp:TemplateField>
                                                    </Columns>
                                                    <HeaderStyle BackColor="#0CA6CA" ForeColor="Black" />
                                                </asp:GridView>
                                            </div>
                                        </center>
                                    </center>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="3">
                                    <asp:Button ID="btnaddrow" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                        Font-Size="Medium" Text="Add Row" OnClick="btnaddrow_Click" />
                                    <asp:Button ID="btnfeesave" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                        Font-Size="Medium" Text="Save" OnClick="bbtnfeesave_Click" />
                                    <asp:Button ID="btnfeedelete" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                        Font-Size="Medium" Text="Delete" OnClick="btnfeedelete_Click" />
                                    <asp:Button ID="btnexit" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                        Font-Size="Medium" Text="Exit" OnClick="btnexit_Click" />
                                </td>
                            </tr>
                            <tr>
                                <td colspan="3">
                                    <asp:Label ID="panelerrmsg" runat="server" Text="No Record(s) Found" ForeColor="Red"
                                        Visible="False" Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium"></asp:Label>
                                </td>
                            </tr>
                        </table>
                    </div>
                </asp:Panel>
            </center>
        </div>
        <%-- </ContentTemplate>
        </asp:UpdatePanel>--%>
    </center>
    <center>
        <asp:UpdatePanel ID="UpdatePanel9" runat="server">
            <ContentTemplate>
                <div id="divPopCond" runat="server" visible="false" style="height: 550em; z-index: 2000;
                    width: 100%; background-color: rgba(54, 25, 25, .2); position: absolute; top: 0%;
                    left: 0%;">
                    <center>
                        <div id="divPopUpCondenation" runat="server" class="table" style="background-color: White;
                            height: auto; width: 60%; border: 5px solid #0CA6CA; border-top: 25px solid #0CA6CA;
                            left: 18%; right: 18%; top: 35%; padding: 5px; position: fixed; border-radius: 10px;">
                            <center>
                                <table style="height: 100px; width: auto; padding: 5px;">
                                    <tr>
                                        <td>
                                            <asp:Label ID="Label3" runat="server" Text="Date" Font-Bold="True" Font-Size="Medium"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtCondonationDate" runat="server" placeholder="dd/MM/yyyy" Font-Names="Book Antiqua"
                                                Font-Size="Medium" Width="90px"></asp:TextBox>
                                            <asp:CalendarExtender ID="CalendarExtender4" Format="dd/MM/yyyy" TargetControlID="txtCondonationDate"
                                                runat="server">
                                            </asp:CalendarExtender>
                                        </td>
                                        <td>
                                            <asp:Label ID="lblChallanno" runat="server" Text="Challan No" Font-Bold="True" Font-Size="Medium"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtChallanAmount" runat="server" Font-Names="Book Antiqua" Font-Size="Medium"
                                                Width="120px" AutoPostBack="true"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Button ID="btnPopAlertClose" Font-Bold="true" Font-Size="Medium" Font-Names="Book Antiqua"
                                                CssClass="textbox textbox1" Style="height: auto; width: auto;" OnClick="btnPopAlertClose_Click"
                                                Text="Save" runat="server" />
                                        </td>
                                        <td>
                                            <asp:Button ID="btnCondExit" Font-Bold="true" Font-Size="Medium" Font-Names="Book Antiqua"
                                                CssClass="textbox textbox1" Style="height: auto; width: auto;" OnClick="btnCondExit_Click"
                                                Text="Close" runat="server" />
                                        </td>
                                    </tr>
                                </table>
                            </center>
                        </div>
                    </center>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
    </center>
    <center>
        <%--<asp:UpdatePanel ID="UpdatePanel10" runat="server">
            <ContentTemplate>--%>
        <div id="divPopUpAlert" runat="server" visible="false" style="height: 550em; z-index: 2000;
            width: 100%; background-color: rgba(54, 25, 25, .2); position: absolute; top: 0%;
            left: 0%;">
            <center>
                <div id="divPopUpAlertContent" runat="server" class="table" style="background-color: White;
                    height: 120px; width: 23%; border: 5px solid #0CA6CA; border-top: 25px solid #0CA6CA;
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
                                        <asp:Button ID="Button1" Font-Bold="true" Font-Size="Medium" Font-Names="Book Antiqua"
                                            CssClass="textbox textbox1" Style="height: auto; width: auto;" OnClick="btnPopUpAlertClose_Click"
                                            Text="Ok" runat="server" />
                                    </center>
                                </td>
                            </tr>
                        </table>
                    </center>
                    <asp:Label ID="lblxpos" runat="server" Visible="false"></asp:Label>
                    <asp:Label ID="lblypos" runat="server" Visible="false"></asp:Label>
                </div>
            </center>
        </div>
        <%-- </ContentTemplate>
        </asp:UpdatePanel>--%>
    </center>
    <%-- <Triggers>
            <asp:PostBackTrigger ControlID="btnxl" />
            <asp:PostBackTrigger ControlID="btnprintmaster" />
            <asp:PostBackTrigger ControlID="btnconxl" />
            <asp:PostBackTrigger ControlID="btnconprint" />
            <asp:PostBackTrigger ControlID="btnCondonationReport" />
            <asp:PostBackTrigger ControlID="btnnoteliexcel" />
            <asp:PostBackTrigger ControlID="btnnoteliprint" />
        </Triggers>--%>
    <NEW:NEWPrintMater runat="server" ID="Printcontrol" Visible="false" />
    <NEW:NEWPrintMater runat="server" ID="PRINTPDF1" Visible="false" />
    <NEW:NEWPrintMater runat="server" ID="PRINTPDF2" Visible="false" />
    <div style="height: 1px; width: 1px; overflow: auto;">
        <div id="contentDiv" runat="server" style="height: auto; width: 1344px;" visible="false">
        </div>
    </div>
    <%-- <center>
        <asp:UpdateProgress ID="UpdateProgress1" runat="server" AssociatedUpdatePanelID="UpGo">
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
    <center>
        <asp:UpdateProgress ID="UpdateProgress2" runat="server" AssociatedUpdatePanelID="UpdatePanel1">
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
    </center>--%>
</asp:Content>

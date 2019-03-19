<%@ Page Title="" Language="C#" MasterPageFile="~/AttendanceMOD/AttendanceSubSiteMaster.master"
    AutoEventWireup="true" CodeFile="Staff_Time_Table.aspx.cs" Inherits="Staff_Time_Table"
    EnableEventValidation="false" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <link href="~/Styles/css/Commoncss.css" rel="Stylesheet" type="text/css" />
    <script type="text/javascript" src="//ajax.googleapis.com/ajax/libs/jquery/1.11.1/jquery.min.js"></script>
    <style type="text/css">
        .Grid
        {
            border: 2px solid #999999;
            background-color: White;
            box-shadow: 0px 0px 8px #999999; /*F0F0F0*/
            border-radius: 10px;
            overflow: auto;
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
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <center>
        <span class="fontstyleheader" style="color: Green">Staff Time Table </span>
        <br />
        <br />
        <div class="maindivstyle">
            <br />
            <table class="maintablestyle">
                <tr>
                    <td>
                        College
                    </td>
                    <td>
                        <asp:DropDownList ID="ddlcollege" runat="server" Font-Bold="true" Font-Names="Book Antiqua"
                            Font-Size="Medium" CssClass="textbox1 ddlheight5" Width="200px" OnSelectedIndexChanged="ddlcollege_change"
                            AutoPostBack="true">
                        </asp:DropDownList>
                    </td>
                    <td>
                        Department
                    </td>
                    <td>
                        <asp:UpdatePanel ID="upddept" runat="server">
                            <ContentTemplate>
                                <asp:TextBox ID="txt_dept" runat="server" ReadOnly="true" CssClass="textbox txtheight2"
                                    Style="width: 135px; font-family: book antiqua; font-weight: bold; font-size: medium;">--Select--</asp:TextBox>
                                <asp:Panel ID="p1" runat="server" BorderColor="silver" CssClass="multxtpanel" Style="background: White;
                                    border-color: Gray; border-style: Solid; border-width: 2px; position: absolute;
                                    box-shadow: 0px 0px 4px #999999; border-radius: 5px; overflow: auto; height: 200px;">
                                    <asp:CheckBox ID="cb_dept" runat="server" Text="Select All" OnCheckedChanged="cb_dept_CheckedChange"
                                        AutoPostBack="true" />
                                    <asp:CheckBoxList ID="cbl_dept" runat="server" OnSelectedIndexChanged="cbl_dept_SelectedIndexChange"
                                        AutoPostBack="true">
                                    </asp:CheckBoxList>
                                </asp:Panel>
                                <asp:PopupControlExtender ID="PopupControlExtender1" runat="server" TargetControlID="txt_dept"
                                    PopupControlID="p1" Position="Bottom">
                                </asp:PopupControlExtender>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </td>
                    <td>
                        Designation
                    </td>
                    <td>
                        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                            <ContentTemplate>
                                <asp:TextBox ID="txtDesig" runat="server" ReadOnly="true" CssClass="textbox txtheight2"
                                    Style="width: 135px; font-family: book antiqua; font-weight: bold; font-size: medium;">--Select--</asp:TextBox>
                                <asp:Panel ID="Panel1" runat="server" BorderColor="silver" CssClass="multxtpanel"
                                    Style="background: White; border-color: Gray; border-style: Solid; border-width: 2px;
                                    position: absolute; box-shadow: 0px 0px 4px #999999; border-radius: 5px; overflow: auto;
                                    height: 200px;">
                                    <asp:CheckBox ID="cbDesig" runat="server" Text="Select All" OnCheckedChanged="cbDesig_CheckedChange"
                                        AutoPostBack="true" />
                                    <asp:CheckBoxList ID="cblDesig" runat="server" OnSelectedIndexChanged="cblDesig_SelectedIndexChange"
                                        AutoPostBack="true">
                                    </asp:CheckBoxList>
                                </asp:Panel>
                                <asp:PopupControlExtender ID="PopupControlExtender2" runat="server" TargetControlID="txtDesig"
                                    PopupControlID="Panel1" Position="Bottom">
                                </asp:PopupControlExtender>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </td>
                    <td>
                        Staff Type
                    </td>
                    <td>
                        <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                            <ContentTemplate>
                                <asp:TextBox ID="txtStfType" runat="server" ReadOnly="true" CssClass="textbox txtheight2"
                                    Style="width: 100px; font-family: book antiqua; font-weight: bold; font-size: medium;">--Select--</asp:TextBox>
                                <asp:Panel ID="Panel2" runat="server" BorderColor="silver" CssClass="multxtpanel"
                                    Style="background: White; border-color: Gray; border-style: Solid; border-width: 2px;
                                    position: absolute; box-shadow: 0px 0px 4px #999999; border-radius: 5px; overflow: auto;
                                    height: 200px;">
                                    <asp:CheckBox ID="cbStfType" runat="server" Text="Select All" OnCheckedChanged="cbStfType_CheckedChange"
                                        AutoPostBack="true" />
                                    <asp:CheckBoxList ID="cblStfType" runat="server" OnSelectedIndexChanged="cblStfType_SelectedIndexChange"
                                        AutoPostBack="true">
                                    </asp:CheckBoxList>
                                </asp:Panel>
                                <asp:PopupControlExtender ID="PopupControlExtender3" runat="server" TargetControlID="txtStfType"
                                    PopupControlID="Panel2" Position="Bottom">
                                </asp:PopupControlExtender>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </td>
                </tr>
                <tr>
                    <td>
                        Staff Name
                    </td>
                    <td>
                        <asp:DropDownList ID="ddlStfName" runat="server" Font-Bold="true" Font-Names="Book Antiqua"
                            Font-Size="Medium" CssClass="textbox1 ddlheight5" Width="200px">
                        </asp:DropDownList>
                    </td>
                    <td>
                        Search By
                    </td>
                    <td>
                        <asp:DropDownList ID="ddlSearchOption" runat="server" Font-Bold="true" Font-Size="Medium"
                            Font-Names="Book Antiqua" CssClass="textbox1 ddlheight5" Width="144px" OnSelectedIndexChanged="ddlSearchOption_Change"
                            AutoPostBack="true">
                            <asp:ListItem Selected="True" Text="Staff Code" Value="0"></asp:ListItem>
                            <asp:ListItem Text="Staff Name" Value="1"></asp:ListItem>
                        </asp:DropDownList>
                    </td>
                    <td id="tdStfCode" runat="server" visible="false">
                        Staff Code
                    </td>
                    <td id="tdStfCodeAuto" runat="server" visible="false">
                        <asp:TextBox ID="txt_scode" runat="server" OnTextChanged="txt_scode_Change" AutoPostBack="true"
                            MaxLength="10" CssClass="textbox txtheight2" Style="font-weight: bold; width: 135px;
                            font-family: book antiqua; font-size: medium;"></asp:TextBox>
                        <asp:AutoCompleteExtender ID="AutoCompleteExtender6" runat="server" DelimiterCharacters=""
                            Enabled="True" ServiceMethod="GetStaffCode" MinimumPrefixLength="0" CompletionInterval="100"
                            EnableCaching="false" CompletionSetCount="10" ServicePath="" TargetControlID="txt_scode"
                            CompletionListCssClass="autocomplete_completionListElement" CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem"
                            CompletionListItemCssClass="txtsearchpan">
                        </asp:AutoCompleteExtender>
                    </td>
                    <td id="tdStfName" runat="server" visible="false">
                        Staff Name
                    </td>
                    <td id="tdStfNameAuto" runat="server" visible="false">
                        <asp:TextBox ID="txt_sname" runat="server" OnTextChanged="txt_sname_Change" AutoPostBack="true"
                            MaxLength="10" CssClass="textbox txtheight2" Style="font-weight: bold; width: 135px;
                            font-family: book antiqua; font-size: medium;"></asp:TextBox>
                        <asp:AutoCompleteExtender ID="AutoCompleteExtender1" runat="server" DelimiterCharacters=""
                            Enabled="True" ServiceMethod="GetStaffName" MinimumPrefixLength="0" CompletionInterval="100"
                            EnableCaching="false" CompletionSetCount="10" ServicePath="" TargetControlID="txt_sname"
                            CompletionListCssClass="autocomplete_completionListElement" CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem"
                            CompletionListItemCssClass="txtsearchpan">
                        </asp:AutoCompleteExtender>
                    </td>
                </tr>
                <tr>
                    <td colspan="2">
                        <fieldset id="fldDates" runat="server" style="border-color: Black; border-radius: 5px;
                            width: auto;">
                            <asp:RadioButton ID="radSemWise" runat="server" Text="Semester Wise" Checked="true"
                                GroupName="SemDay" OnCheckedChanged="radSemWise_Change" AutoPostBack="true" />
                            &nbsp;&nbsp;&nbsp;&nbsp;
                            <asp:RadioButton ID="radDayWise" runat="server" Text="Day Wise" GroupName="SemDay"
                                OnCheckedChanged="radDayWise_Change" AutoPostBack="true" />
                        </fieldset>
                    </td>
                    <td id="tdlbFrm" runat="server" visible="false">
                        From Date
                    </td>
                    <td colspan="4">
                        <asp:TextBox ID="txtFrmDt" runat="server" Visible="false" CssClass="textbox txtheight2"
                            Style="width: 90px; height: 25px; font-family: book antiqua; font-weight: bold;
                            font-size: medium;"></asp:TextBox>
                        <asp:CalendarExtender ID="calFrmDt" runat="server" CssClass="cal_Theme1 ajax__calendar_active"
                            TargetControlID="txtFrmDt" Format="dd/MM/yyyy">
                        </asp:CalendarExtender>
                        <asp:Label ID="lblToDt" runat="server" Text="To Date" Visible="false"></asp:Label>
                        <asp:TextBox ID="txtToDt" runat="server" Visible="false" CssClass="textbox txtheight2"
                            Style="width: 90px; height: 25px; font-family: book antiqua; font-weight: bold;
                            font-size: medium;"></asp:TextBox>
                        <asp:CalendarExtender ID="calToDt" runat="server" CssClass="cal_Theme1 ajax__calendar_active"
                            TargetControlID="txtToDt" Format="dd/MM/yyyy">
                        </asp:CalendarExtender>
                        <asp:Button ID="btnGo" runat="server" Text="Go" Height="35px" Width="50px" OnClick="btnGo_Click"
                            Font-Bold="true" Font-Names="Book Antiqua" Font-Size="Medium" CssClass="textbox textbox1 btn2" />
                    </td>
                </tr>
            </table>
            <asp:UpdatePanel ID="updcolumn" runat="server">
                <ContentTemplate>
                    <div>
                        <br />
                        <center>
                            <asp:Panel ID="pheaderfilter" runat="server" CssClass="maintablestyle" Height="22px"
                                Width="920px" Style="margin-top: -0.1%; cursor: pointer;">
                                <asp:Label ID="Labelfilter" Text="Display Options" runat="server" Font-Size="Medium"
                                    Font-Bold="True" Font-Names="Book Antiqua" Style="margin-left: 0%;" />
                                <asp:Image ID="Imagefilter" runat="server" CssClass="cpimage" ImageAlign="Right" />
                            </asp:Panel>
                        </center>
                        <br />
                    </div>
                    <center>
                        <asp:Panel ID="pcolumnorder" runat="server" CssClass="maintablestyle" Width="920px">
                            <table>
                                <tr>
                                    <td>
                                        <asp:CheckBox ID="CheckBox_column" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                            Font-Size="Medium" Text="Select All" AutoPostBack="true" OnCheckedChanged="CheckBox_column_CheckedChanged" />
                                    </td>
                                    <td>
                                        <asp:LinkButton ID="lnk_columnorder" runat="server" Font-Size="X-Small" Height="16px"
                                            Style="font-family: 'Book Antiqua'; font-weight: 700; font-size: small; margin-left: -599px;"
                                            Width="111px" OnClick="LinkButtonsremove_Click">Remove  All</asp:LinkButton>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        &nbsp;
                                        <asp:TextBox ID="tborder" Visible="true" Text="SUBJECT CODE(1),ROOM NAME(2)" ReadOnly="true"
                                            Width="891px" TextMode="MultiLine" CssClass="style1" AutoPostBack="true" runat="server"
                                            Enabled="true">
                                        </asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:CheckBoxList ID="cblcolumnorder" runat="server" Height="43px" AutoPostBack="true"
                                            Width="920px" Style="font-family: 'Book Antiqua'; font-weight: 700; font-size: medium;"
                                            RepeatColumns="5" RepeatDirection="Horizontal" OnSelectedIndexChanged="cblcolumnorder_SelectedIndexChanged">
                                            <asp:ListItem Selected="True" Value="0">SUBJECT CODE</asp:ListItem>
                                            <asp:ListItem Value="1">SUBJECT NAME</asp:ListItem>
                                            <asp:ListItem Value="2">DEGREE</asp:ListItem>
                                            <asp:ListItem Value="3">BATCH</asp:ListItem>
                                            <asp:ListItem Value="4">SEMESTER</asp:ListItem>
                                            <asp:ListItem Value="5">SECTION</asp:ListItem>
                                            <asp:ListItem Selected="True" Value="6">ROOM NAME</asp:ListItem>
                                        </asp:CheckBoxList>
                                    </td>
                                </tr>
                            </table>
                        </asp:Panel>
                    </center>
                    <asp:CollapsiblePanelExtender ID="cpecolumnorder" runat="server" TargetControlID="pcolumnorder"
                        CollapseControlID="pheaderfilter" ExpandControlID="pheaderfilter" Collapsed="true"
                        TextLabelID="Labelfilter" CollapsedSize="0" ImageControlID="Imagefilter" CollapsedImage="../images/right.jpeg"
                        ExpandedImage="../images/down.jpeg">
                    </asp:CollapsiblePanelExtender>
                </ContentTemplate>
            </asp:UpdatePanel>
            <br />
            <asp:Label ID="lblMainErr" runat="server" Visible="false" Text="" Font-Bold="true"
                Font-Size="Medium" ForeColor="Red" Font-Names="Book Antiqua"></asp:Label>
            <br />
            <div id="printdiv" runat="server">
                <asp:GridView ID="grdStf_TT" runat="server" AutoGenerateColumns="True" Visible="false"
                    CssClass="Grid" GridLines="Both" HeaderStyle-BackColor="#0CA6CA" HeaderStyle-Font-Bold="true"
                    HeaderStyle-HorizontalAlign="Center" HeaderStyle-Font-Names="Book Antiqua" HeaderStyle-Font-Size="Medium"
                    OnRowDataBound="grdStf_TT_OnRowDataBound">
                </asp:GridView>
                <br />
                <asp:GridView ID="grdStfDet_TT" runat="server" AutoGenerateColumns="True" Visible="false"
                    CssClass="Grid" GridLines="Both" HeaderStyle-BackColor="#0CA6CA" HeaderStyle-Font-Bold="true"
                    HeaderStyle-HorizontalAlign="Left" HeaderStyle-Font-Names="Book Antiqua" HeaderStyle-Font-Size="Medium"
                    OnRowDataBound="grdStfDet_TT_OnRowDataBound">
                </asp:GridView>
                <br />
            </div>
            <button id="btnComPrint" runat="server" visible="false" onclick="return printTTOutput();"
                style="background-color: LightGreen; font-weight: bold; font-size: medium; font-family: Book Antiqua;">
                Print
            </button>
            <br />
            <br />
            <div id="alertpopwindow" runat="server" visible="false" style="height: 100%; z-index: 1000;
                width: 100%; background-color: rgba(54, 25, 25, .2); position: absolute; top: 0;
                left: 0px;">
                <center>
                    <div id="pnl2" runat="server" class="table" style="background-color: White; height: 120px;
                        width: 238px; border: 5px solid #0CA6CA; border-top: 25px solid #0CA6CA; margin-top: 200px;
                        border-radius: 10px;">
                        <center>
                            <table style="height: 100px; width: 100%">
                                <tr>
                                    <td align="center">
                                        <asp:Label ID="lblalerterr" runat="server" Text="" Style="color: Red;" Font-Bold="true"
                                            Font-Size="Medium"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <center>
                                            <asp:Button ID="btn_errorclose" CssClass="textbox textbox1 btn1" OnClick="btn_errorclose_Click"
                                                Text="OK" runat="server" />
                                        </center>
                                    </td>
                                </tr>
                            </table>
                        </center>
                    </div>
                </center>
            </div>
        </div>
    </center>
</asp:Content>

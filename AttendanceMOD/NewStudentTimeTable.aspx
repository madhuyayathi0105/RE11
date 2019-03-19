<%@ Page Title="" Language="C#" MasterPageFile="~/AttendanceMOD/AttendanceSubSiteMaster.master"
    AutoEventWireup="true" CodeFile="NewStudentTimeTable.aspx.cs" Inherits="NewStudentTimeTable" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Assembly="FarPoint.Web.Spread" Namespace="FarPoint.Web.Spread" TagPrefix="FarPoint" %>
<%@ Register Src="~/Usercontrols/PrintMaster.ascx" TagName="printmaster" TagPrefix="Insproplus" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <style type="text/css">
        .style1
        {
            width: 370px;
        }
    </style>
    <script type="text/javascript">
        var prm = Sys.WebForms.PageRequestManager.getInstance();
        prm.add_beginRequest(BeginRequestHandler);
        prm.add_endRequest(EndRequestHandler);
        function BeginRequestHandler(sender, args) {
            var popup = $find('<%= modalPopup.ClientID %>');
            if (popup != null) {
                popup.show();
            }
        }
        function EndRequestHandler(sender, args) {
            var popup = $find('<%= modalPopup.ClientID %>');
            if (popup != null) {
                popup.hide();
            }
        }
    </script>
    <style type="text/css">
        .floats
        {
            height: 26px;
        }
        .CenterPB
        {
            position: absolute;
            left: 50%;
            top: 50%;
            margin-top: -20px;
            margin-left: -20px;
            width: auto;
            height: auto;
        }
        .modalPopup
        {
            background-color: #ffffdd;
            border-width: 1px;
            -moz-border-radius: 5px;
            border-style: solid;
            border-color: Gray;
            min-width: 250px;
            max-width: 500px;
            min-height: 100px;
            max-height: 150px;
            top: 100px;
            left: 150px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <center>
        <asp:Label ID="Label5" runat="server" Font-Bold="True" CssClass="fontstyleheader"
            ForeColor="Green" Text="Semester Time Table" style="margin:0px; margin-bottom:10px; margin-top:15px;"></asp:Label>
    </center>
    <asp:UpdatePanel ID="udptimetable" runat="server">
        <ContentTemplate>
            <asp:UpdateProgress ID="UpdateProgress1" runat="server" DynamicLayout="true" AssociatedUpdatePanelID="udptimetable">
                <ProgressTemplate>
                    <div class='UpdateProgressBackground'>
                    </div>
                    <center>
                        <div class="UpdateProgressContent">
                            <img src="../images/progress2.gif" height="180px" width="180px" />
                        </div>
                    </center>
                </ProgressTemplate>
            </asp:UpdateProgress>
            <asp:ModalPopupExtender ID="modalPopup" runat="server" TargetControlID="UpdateProgress1"
                PopupControlID="UpdateProgress1">
            </asp:ModalPopupExtender>
            <asp:Panel ID="pnlmsgboxupdate" runat="server" CssClass="modalPopup" Style="display: none;
                height: 100; width: 300;">
                <table width="100%">
                    <tr class="topHandle">
                        <td colspan="2" align="left" runat="server" id="tdCaption">
                            <asp:Label ID="lblCaption" runat="server" Font-Bold="True" Text="Confirmation" Font-Names="Book Antiqua"
                                Font-Size="Large"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 60px" valign="middle" align="center">
                            <asp:Image ID="imgInfo" runat="server" ImageUrl="~/Info-48x48.png" />
                        </td>
                        <td valign="middle" align="left">
                            <asp:Label ID="Label1" Text="Do You want to Allow?" runat="server" Font-Bold="True"
                                Font-Names="Book Antiqua" Font-Size="Medium"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2" align="right">
                            <asp:Button ID="btnuupd" runat="server" Text="Make Combine Class" OnClick="btnupOk_Click"
                                Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium" />
                            <asp:Button ID="btnuCancel" runat="server" Text="Cancel" OnClick="btnupCancel_Click"
                                Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium" />
                        </td>
                    </tr>
                </table>
            </asp:Panel>
            <asp:ModalPopupExtender ID="mpemsgboxupdate" runat="server" TargetControlID="hfupdate"
                PopupControlID="pnlmsgboxupdate">
            </asp:ModalPopupExtender>
            <asp:HiddenField runat="server" ID="hfupdate" />
            <center>
                <table style="width: 900px; height: 70px; background-color: #0CA6CA; margin:0px; margin-bottom:10px; margin-top:15px;">
                    <tr>
                        <td>
                            <asp:Label ID="lblcolg" runat="server" Text="College " Font-Bold="True" Font-Names="Book Antiqua"
                                Font-Size="Medium"></asp:Label>
                        </td>
                        <td>
                            <asp:DropDownList ID="ddlcolg" runat="server" Width="100px" Font-Bold="True" Font-Names="Book Antiqua"
                                Font-Size="Medium" AutoPostBack="true" OnSelectedIndexChanged="collook_load">
                            </asp:DropDownList>
                        </td>
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
                    </tr>
                    <tr>
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
                        <td>
                            <asp:Label ID="lbltimetable" runat="server" Text="Time Table Name" Font-Bold="True"
                                Font-Names="Book Antiqua" Font-Size="Medium" Width="150"></asp:Label>
                        </td>
                        <td>
                            <asp:DropDownList ID="ddltimetable" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                Font-Size="Medium" Width="100px" AutoPostBack="true" OnSelectedIndexChanged="ddltimetable_SelectedIndexChanged">
                            </asp:DropDownList>
                        </td>
                        <td id="tdTime" runat="server">
                            <asp:TextBox ID="txttimetable" runat="server" Font-Bold="true" Font-Names="Book Antiqua"></asp:TextBox>
                            <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender6" runat="server" TargetControlID="txttimetable"
                                FilterType="LowercaseLetters,UppercaseLetters,Numbers,Custom" ValidChars=" -()_">
                            </asp:FilteredTextBoxExtender>
                        </td>
                        <td>
                            <asp:Label ID="lbldate" runat="server" Text="From Date" Font-Bold="True" Font-Names="Book Antiqua"
                                Font-Size="Medium"></asp:Label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtdate" runat="server" Font-Bold="true" Font-Names="Book Antiqua"
                                AutoPostBack="true" Width="80px" OnTextChanged="txtdate_TextChanged"></asp:TextBox>
                            <asp:CalendarExtender ID="CalToDate" runat="server" Format="dd/MM/yyyy" TargetControlID="txtdate">
                            </asp:CalendarExtender>
                        </td>
                        <td>
                            <asp:Button ID="btngo" runat="server" Text="Go" Font-Bold="True" Font-Names="Book Antiqua"
                                Font-Size="Medium" OnClick="btngo_Click" />
                        </td>
                    </tr>
                </table>
            </center>
            <center>
                <asp:Label ID="errmsg" ForeColor="Red" runat="server" Font-Bold="true" Font-Names="Book Antiqua"
                    Font-Size="Medium" style="margin:0px; margin-bottom:10px; margin-top:15px;"></asp:Label>
            </center>
            <center>
                <table style="margin:0px; margin-bottom:10px; margin-top:15px;">
                    <tr>
                        <td colspan="4">
                            <FarPoint:FpSpread ID="Fptimetable" runat="server" ActiveSheetViewIndex="0" BorderColor="Black"
                                BorderStyle="Solid" BorderWidth="1px" CommandBar-Font-Names="Arial" CssClass="cursorptr"
                                currentPageIndex="0" DesignString="&lt;?xml version=&quot;1.0&quot; encoding=&quot;utf-8&quot;?&gt;&lt;Spread /&gt;"
                                Height="212px" OnCellClick="Fptimetable_CellClick" OnPreRender="Fptimetable_SelectedIndexChanged"
                                ScrollBar3DLightColor="Red" ScrollBarArrowColor="Aqua" ScrollBarBaseColor="Goldenrod"
                                ScrollBarDarkShadowColor="#FF8080" ScrollBarFaceColor="#99FF66" scrollContent="true"
                                scrollContentColumns="" scrollContentMaxHeight="50" scrollContentTime="500" ShowHeaderSelection="false"
                                Style="margin: 0px; margin-top: 15px;">
                                <CommandBar BackColor="Control" ButtonFaceColor="Control" ButtonHighlightColor="ControlLightLight"
                                    ButtonShadowColor="ControlDark" Font-Bold="False" Font-Italic="False" Font-Overline="False"
                                    Font-Strikeout="False" Font-Underline="False" Visible="False">
                                    <Background BackgroundImageUrl="SPREADCLIENTPATH:/img/cbbg.gif" />
                                </CommandBar>
                                <Pager Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                    Font-Underline="False" />
                                <HierBar Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                    Font-Underline="False" />
                                <Sheets>
                                    <FarPoint:SheetView AllowPage="False" DesignString="&lt;?xml version=&quot;1.0&quot; encoding=&quot;utf-8&quot;?&gt;&lt;Sheet&gt;&lt;Data&gt;&lt;RowHeader class=&quot;FarPoint.Web.Spread.Model.DefaultSheetDataModel&quot; rows=&quot;3&quot; columns=&quot;1&quot;&gt;&lt;AutoCalculation&gt;True&lt;/AutoCalculation&gt;&lt;AutoGenerateColumns&gt;True&lt;/AutoGenerateColumns&gt;&lt;ReferenceStyle&gt;A1&lt;/ReferenceStyle&gt;&lt;Iteration&gt;False&lt;/Iteration&gt;&lt;MaximumIterations&gt;1&lt;/MaximumIterations&gt;&lt;MaximumChange&gt;0.001&lt;/MaximumChange&gt;&lt;/RowHeader&gt;&lt;ColumnHeader class=&quot;FarPoint.Web.Spread.Model.DefaultSheetDataModel&quot; rows=&quot;1&quot; columns=&quot;4&quot;&gt;&lt;AutoCalculation&gt;True&lt;/AutoCalculation&gt;&lt;AutoGenerateColumns&gt;True&lt;/AutoGenerateColumns&gt;&lt;ReferenceStyle&gt;A1&lt;/ReferenceStyle&gt;&lt;Iteration&gt;False&lt;/Iteration&gt;&lt;MaximumIterations&gt;1&lt;/MaximumIterations&gt;&lt;MaximumChange&gt;0.001&lt;/MaximumChange&gt;&lt;/ColumnHeader&gt;&lt;DataArea class=&quot;FarPoint.Web.Spread.Model.DefaultSheetDataModel&quot; rows=&quot;3&quot; columns=&quot;4&quot;&gt;&lt;AutoCalculation&gt;True&lt;/AutoCalculation&gt;&lt;AutoGenerateColumns&gt;True&lt;/AutoGenerateColumns&gt;&lt;ReferenceStyle&gt;A1&lt;/ReferenceStyle&gt;&lt;Iteration&gt;False&lt;/Iteration&gt;&lt;MaximumIterations&gt;1&lt;/MaximumIterations&gt;&lt;MaximumChange&gt;0.001&lt;/MaximumChange&gt;&lt;SheetName&gt;Sheet1&lt;/SheetName&gt;&lt;/DataArea&gt;&lt;SheetCorner class=&quot;FarPoint.Web.Spread.Model.DefaultSheetDataModel&quot; rows=&quot;1&quot; columns=&quot;1&quot;&gt;&lt;AutoCalculation&gt;True&lt;/AutoCalculation&gt;&lt;AutoGenerateColumns&gt;True&lt;/AutoGenerateColumns&gt;&lt;ReferenceStyle&gt;A1&lt;/ReferenceStyle&gt;&lt;Iteration&gt;False&lt;/Iteration&gt;&lt;MaximumIterations&gt;1&lt;/MaximumIterations&gt;&lt;MaximumChange&gt;0.001&lt;/MaximumChange&gt;&lt;/SheetCorner&gt;&lt;ColumnFooter class=&quot;FarPoint.Web.Spread.Model.DefaultSheetDataModel&quot; rows=&quot;1&quot; columns=&quot;4&quot;&gt;&lt;AutoCalculation&gt;True&lt;/AutoCalculation&gt;&lt;AutoGenerateColumns&gt;True&lt;/AutoGenerateColumns&gt;&lt;ReferenceStyle&gt;A1&lt;/ReferenceStyle&gt;&lt;Iteration&gt;False&lt;/Iteration&gt;&lt;MaximumIterations&gt;1&lt;/MaximumIterations&gt;&lt;MaximumChange&gt;0.001&lt;/MaximumChange&gt;&lt;/ColumnFooter&gt;&lt;/Data&gt;&lt;Presentation&gt;&lt;ActiveSkin class=&quot;FarPoint.Web.Spread.SheetSkin&quot;&gt;&lt;Name&gt;Default&lt;/Name&gt;&lt;BackColor&gt;Empty&lt;/BackColor&gt;&lt;CellBackColor&gt;Empty&lt;/CellBackColor&gt;&lt;CellForeColor&gt;Empty&lt;/CellForeColor&gt;&lt;CellSpacing&gt;0&lt;/CellSpacing&gt;&lt;GridLines&gt;Both&lt;/GridLines&gt;&lt;GridLineColor&gt;#d0d7e5&lt;/GridLineColor&gt;&lt;HeaderBackColor&gt;Empty&lt;/HeaderBackColor&gt;&lt;HeaderForeColor&gt;Empty&lt;/HeaderForeColor&gt;&lt;FlatColumnHeader&gt;False&lt;/FlatColumnHeader&gt;&lt;FooterBackColor&gt;Empty&lt;/FooterBackColor&gt;&lt;FooterForeColor&gt;Empty&lt;/FooterForeColor&gt;&lt;FlatColumnFooter&gt;False&lt;/FlatColumnFooter&gt;&lt;FlatRowHeader&gt;False&lt;/FlatRowHeader&gt;&lt;HeaderFontBold&gt;False&lt;/HeaderFontBold&gt;&lt;FooterFontBold&gt;False&lt;/FooterFontBold&gt;&lt;SelectionBackColor&gt;#eaecf5&lt;/SelectionBackColor&gt;&lt;SelectionForeColor&gt;Empty&lt;/SelectionForeColor&gt;&lt;EvenRowBackColor&gt;Empty&lt;/EvenRowBackColor&gt;&lt;OddRowBackColor&gt;Empty&lt;/OddRowBackColor&gt;&lt;ShowColumnHeader&gt;True&lt;/ShowColumnHeader&gt;&lt;ShowColumnFooter&gt;False&lt;/ShowColumnFooter&gt;&lt;ShowRowHeader&gt;True&lt;/ShowRowHeader&gt;&lt;ColumnHeaderBackground class=&quot;FarPoint.Web.Spread.Background&quot;&gt;&lt;BackgroundImageUrl&gt;SPREADCLIENTPATH:/img/chbg.gif&lt;/BackgroundImageUrl&gt;&lt;SelectedBackgroundImageUrl&gt;SPREADCLIENTPATH:/img/chm.png&lt;/SelectedBackgroundImageUrl&gt;&lt;/ColumnHeaderBackground&gt;&lt;SheetCornerBackground class=&quot;FarPoint.Web.Spread.Background&quot;&gt;&lt;BackgroundImageUrl&gt;SPREADCLIENTPATH:/img/chbg.gif&lt;/BackgroundImageUrl&gt;&lt;SelectedBackgroundImageUrl&gt;SPREADCLIENTPATH:/img/chm.png&lt;/SelectedBackgroundImageUrl&gt;&lt;/SheetCornerBackground&gt;&lt;HeaderGrayAreaColor&gt;#7999c2&lt;/HeaderGrayAreaColor&gt;&lt;/ActiveSkin&gt;&lt;AxisModels&gt;&lt;Column class=&quot;FarPoint.Web.Spread.Model.DefaultSheetAxisModel&quot; orientation=&quot;Horizontal&quot; count=&quot;4&quot;&gt;&lt;Items&gt;&lt;Item index=&quot;-1&quot;&gt;&lt;SortIndicator&gt;Ascending&lt;/SortIndicator&gt;&lt;/Item&gt;&lt;Item index=&quot;0&quot;&gt;&lt;Size&gt;55&lt;/Size&gt;&lt;/Item&gt;&lt;Item index=&quot;1&quot;&gt;&lt;Size&gt;111&lt;/Size&gt;&lt;/Item&gt;&lt;Item index=&quot;2&quot;&gt;&lt;Size&gt;81&lt;/Size&gt;&lt;/Item&gt;&lt;Item index=&quot;3&quot;&gt;&lt;Size&gt;79&lt;/Size&gt;&lt;/Item&gt;&lt;/Items&gt;&lt;/Column&gt;&lt;RowHeaderColumn class=&quot;FarPoint.Web.Spread.Model.DefaultSheetAxisModel&quot; defaultSize=&quot;40&quot; orientation=&quot;Horizontal&quot; count=&quot;1&quot;&gt;&lt;Items&gt;&lt;Item index=&quot;-1&quot;&gt;&lt;SortIndicator&gt;Ascending&lt;/SortIndicator&gt;&lt;Size&gt;40&lt;/Size&gt;&lt;/Item&gt;&lt;/Items&gt;&lt;/RowHeaderColumn&gt;&lt;ColumnHeaderRow class=&quot;FarPoint.Web.Spread.Model.DefaultSheetAxisModel&quot; defaultSize=&quot;22&quot; orientation=&quot;Vertical&quot; count=&quot;1&quot;&gt;&lt;Items&gt;&lt;Item index=&quot;-1&quot;&gt;&lt;Size&gt;22&lt;/Size&gt;&lt;/Item&gt;&lt;/Items&gt;&lt;/ColumnHeaderRow&gt;&lt;ColumnFooterRow class=&quot;FarPoint.Web.Spread.Model.DefaultSheetAxisModel&quot; defaultSize=&quot;22&quot; orientation=&quot;Vertical&quot; count=&quot;1&quot;&gt;&lt;Items&gt;&lt;Item index=&quot;-1&quot;&gt;&lt;Size&gt;22&lt;/Size&gt;&lt;/Item&gt;&lt;/Items&gt;&lt;/ColumnFooterRow&gt;&lt;/AxisModels&gt;&lt;StyleModels&gt;&lt;RowHeader class=&quot;FarPoint.Web.Spread.Model.DefaultSheetStyleModel&quot; Rows=&quot;3&quot; Columns=&quot;1&quot;&gt;&lt;AltRowCount&gt;2&lt;/AltRowCount&gt;&lt;DefaultStyle class=&quot;FarPoint.Web.Spread.NamedStyle&quot; Parent=&quot;RowHeaderDefault&quot; /&gt;&lt;ConditionalFormatCollections /&gt;&lt;/RowHeader&gt;&lt;ColumnHeader class=&quot;FarPoint.Web.Spread.Model.DefaultSheetStyleModel&quot; Rows=&quot;1&quot; Columns=&quot;4&quot;&gt;&lt;AltRowCount&gt;2&lt;/AltRowCount&gt;&lt;DefaultStyle class=&quot;FarPoint.Web.Spread.NamedStyle&quot; Parent=&quot;ColumnHeaderDefault&quot;&gt;&lt;Background class=&quot;FarPoint.Web.Spread.Background&quot;&gt;&lt;BackgroundImageUrl&gt;SPREADCLIENTPATH:/img/chbg.gif&lt;/BackgroundImageUrl&gt;&lt;SelectedBackgroundImageUrl&gt;SPREADCLIENTPATH:/img/chm.png&lt;/SelectedBackgroundImageUrl&gt;&lt;/Background&gt;&lt;/DefaultStyle&gt;&lt;ConditionalFormatCollections /&gt;&lt;/ColumnHeader&gt;&lt;DataArea class=&quot;FarPoint.Web.Spread.Model.DefaultSheetStyleModel&quot; Rows=&quot;3&quot; Columns=&quot;4&quot;&gt;&lt;AltRowCount&gt;2&lt;/AltRowCount&gt;&lt;DefaultStyle class=&quot;FarPoint.Web.Spread.NamedStyle&quot; Parent=&quot;DataAreaDefault&quot;&gt;&lt;Font&gt;&lt;Name&gt;Book Antiqua&lt;/Name&gt;&lt;Names&gt;&lt;Name&gt;Book Antiqua&lt;/Name&gt;&lt;/Names&gt;&lt;Size&gt;Medium&lt;/Size&gt;&lt;Bold&gt;False&lt;/Bold&gt;&lt;Italic&gt;False&lt;/Italic&gt;&lt;Overline&gt;False&lt;/Overline&gt;&lt;Strikeout&gt;False&lt;/Strikeout&gt;&lt;Underline&gt;False&lt;/Underline&gt;&lt;/Font&gt;&lt;GdiCharSet&gt;254&lt;/GdiCharSet&gt;&lt;ForeColor&gt;#0033cc&lt;/ForeColor&gt;&lt;HorizontalAlign&gt;Center&lt;/HorizontalAlign&gt;&lt;/DefaultStyle&gt;&lt;ConditionalFormatCollections /&gt;&lt;/DataArea&gt;&lt;SheetCorner class=&quot;FarPoint.Web.Spread.Model.DefaultSheetStyleModel&quot; Rows=&quot;1&quot; Columns=&quot;1&quot;&gt;&lt;AltRowCount&gt;2&lt;/AltRowCount&gt;&lt;DefaultStyle class=&quot;FarPoint.Web.Spread.NamedStyle&quot; Parent=&quot;CornerDefault&quot;&gt;&lt;Border class=&quot;FarPoint.Web.Spread.Border&quot; Size=&quot;1&quot; Style=&quot;Solid&quot;&gt;&lt;Bottom Color=&quot;ControlDark&quot; /&gt;&lt;Left Size=&quot;0&quot; /&gt;&lt;Right Color=&quot;ControlDark&quot; /&gt;&lt;Top Size=&quot;0&quot; /&gt;&lt;/Border&gt;&lt;Background class=&quot;FarPoint.Web.Spread.Background&quot;&gt;&lt;BackgroundImageUrl&gt;SPREADCLIENTPATH:/img/chbg.gif&lt;/BackgroundImageUrl&gt;&lt;SelectedBackgroundImageUrl&gt;SPREADCLIENTPATH:/img/chm.png&lt;/SelectedBackgroundImageUrl&gt;&lt;/Background&gt;&lt;/DefaultStyle&gt;&lt;ConditionalFormatCollections /&gt;&lt;/SheetCorner&gt;&lt;ColumnFooter class=&quot;FarPoint.Web.Spread.Model.DefaultSheetStyleModel&quot; Rows=&quot;1&quot; Columns=&quot;4&quot;&gt;&lt;AltRowCount&gt;2&lt;/AltRowCount&gt;&lt;DefaultStyle class=&quot;FarPoint.Web.Spread.NamedStyle&quot; Parent=&quot;ColumnFooterDefault&quot; /&gt;&lt;ConditionalFormatCollections /&gt;&lt;/ColumnFooter&gt;&lt;/StyleModels&gt;&lt;MessageRowStyle class=&quot;FarPoint.Web.Spread.Appearance&quot;&gt;&lt;BackColor&gt;LightYellow&lt;/BackColor&gt;&lt;ForeColor&gt;Red&lt;/ForeColor&gt;&lt;/MessageRowStyle&gt;&lt;SheetCornerStyle class=&quot;FarPoint.Web.Spread.NamedStyle&quot; Parent=&quot;CornerDefault&quot;&gt;&lt;Border class=&quot;FarPoint.Web.Spread.Border&quot; Size=&quot;1&quot; Style=&quot;Solid&quot;&gt;&lt;Bottom Color=&quot;ControlDark&quot; /&gt;&lt;Left Size=&quot;0&quot; /&gt;&lt;Right Color=&quot;ControlDark&quot; /&gt;&lt;Top Size=&quot;0&quot; /&gt;&lt;/Border&gt;&lt;Background class=&quot;FarPoint.Web.Spread.Background&quot;&gt;&lt;BackgroundImageUrl&gt;SPREADCLIENTPATH:/img/chbg.gif&lt;/BackgroundImageUrl&gt;&lt;SelectedBackgroundImageUrl&gt;SPREADCLIENTPATH:/img/chm.png&lt;/SelectedBackgroundImageUrl&gt;&lt;/Background&gt;&lt;/SheetCornerStyle&gt;&lt;AllowLoadOnDemand&gt;false&lt;/AllowLoadOnDemand&gt;&lt;LoadRowIncrement &gt;10&lt;/LoadRowIncrement &gt;&lt;LoadInitRowCount &gt;30&lt;/LoadInitRowCount &gt;&lt;AllowVirtualScrollPaging&gt;false&lt;/AllowVirtualScrollPaging&gt;&lt;TopRow&gt;0&lt;/TopRow&gt;&lt;PreviewRowStyle class=&quot;FarPoint.Web.Spread.PreviewRowInfo&quot; /&gt;&lt;/Presentation&gt;&lt;Settings&gt;&lt;Name&gt;Sheet1&lt;/Name&gt;&lt;Categories&gt;&lt;Appearance&gt;&lt;GridLineColor&gt;#d0d7e5&lt;/GridLineColor&gt;&lt;SelectionBackColor&gt;#eaecf5&lt;/SelectionBackColor&gt;&lt;SelectionBorder class=&quot;FarPoint.Web.Spread.Border&quot; /&gt;&lt;/Appearance&gt;&lt;Behavior&gt;&lt;EditTemplateColumnCount&gt;2&lt;/EditTemplateColumnCount&gt;&lt;ScrollingContentVisible&gt;True&lt;/ScrollingContentVisible&gt;&lt;PageSize&gt;100&lt;/PageSize&gt;&lt;AllowPage&gt;False&lt;/AllowPage&gt;&lt;GroupBarText&gt;Drag a column to group by that column.&lt;/GroupBarText&gt;&lt;/Behavior&gt;&lt;Layout&gt;&lt;RowHeaderColumnCount&gt;1&lt;/RowHeaderColumnCount&gt;&lt;ColumnHeaderRowCount&gt;1&lt;/ColumnHeaderRowCount&gt;&lt;/Layout&gt;&lt;/Categories&gt;&lt;ActiveRow&gt;0&lt;/ActiveRow&gt;&lt;ActiveColumn&gt;0&lt;/ActiveColumn&gt;&lt;ColumnHeaderRowCount&gt;1&lt;/ColumnHeaderRowCount&gt;&lt;ColumnFooterRowCount&gt;1&lt;/ColumnFooterRowCount&gt;&lt;PrintInfo&gt;&lt;Header /&gt;&lt;Footer /&gt;&lt;ZoomFactor&gt;0&lt;/ZoomFactor&gt;&lt;FirstPageNumber&gt;1&lt;/FirstPageNumber&gt;&lt;Orientation&gt;Auto&lt;/Orientation&gt;&lt;PrintType&gt;All&lt;/PrintType&gt;&lt;PageOrder&gt;Auto&lt;/PageOrder&gt;&lt;BestFitCols&gt;False&lt;/BestFitCols&gt;&lt;BestFitRows&gt;False&lt;/BestFitRows&gt;&lt;PageStart&gt;-1&lt;/PageStart&gt;&lt;PageEnd&gt;-1&lt;/PageEnd&gt;&lt;ColStart&gt;-1&lt;/ColStart&gt;&lt;ColEnd&gt;-1&lt;/ColEnd&gt;&lt;RowStart&gt;-1&lt;/RowStart&gt;&lt;RowEnd&gt;-1&lt;/RowEnd&gt;&lt;ShowBorder&gt;True&lt;/ShowBorder&gt;&lt;ShowGrid&gt;True&lt;/ShowGrid&gt;&lt;ShowColor&gt;True&lt;/ShowColor&gt;&lt;ShowColumnHeader&gt;Inherit&lt;/ShowColumnHeader&gt;&lt;ShowRowHeader&gt;Inherit&lt;/ShowRowHeader&gt;&lt;ShowColumnFooter&gt;Inherit&lt;/ShowColumnFooter&gt;&lt;ShowColumnFooterEachPage&gt;True&lt;/ShowColumnFooterEachPage&gt;&lt;ShowTitle&gt;True&lt;/ShowTitle&gt;&lt;ShowSubtitle&gt;True&lt;/ShowSubtitle&gt;&lt;UseMax&gt;True&lt;/UseMax&gt;&lt;UseSmartPrint&gt;False&lt;/UseSmartPrint&gt;&lt;Opacity&gt;255&lt;/Opacity&gt;&lt;PrintNotes&gt;None&lt;/PrintNotes&gt;&lt;Centering&gt;None&lt;/Centering&gt;&lt;RepeatColStart&gt;-1&lt;/RepeatColStart&gt;&lt;RepeatColEnd&gt;-1&lt;/RepeatColEnd&gt;&lt;RepeatRowStart&gt;-1&lt;/RepeatRowStart&gt;&lt;RepeatRowEnd&gt;-1&lt;/RepeatRowEnd&gt;&lt;SmartPrintPagesTall&gt;1&lt;/SmartPrintPagesTall&gt;&lt;SmartPrintPagesWide&gt;1&lt;/SmartPrintPagesWide&gt;&lt;HeaderHeight&gt;-1&lt;/HeaderHeight&gt;&lt;FooterHeight&gt;-1&lt;/FooterHeight&gt;&lt;/PrintInfo&gt;&lt;TitleInfo class=&quot;FarPoint.Web.Spread.TitleInfo&quot;&gt;&lt;Style class=&quot;FarPoint.Web.Spread.StyleInfo&quot;&gt;&lt;BackColor&gt;#e7eff7&lt;/BackColor&gt;&lt;HorizontalAlign&gt;Right&lt;/HorizontalAlign&gt;&lt;/Style&gt;&lt;Value type=&quot;System.String&quot; whitespace=&quot;&quot; /&gt;&lt;/TitleInfo&gt;&lt;LayoutTemplate class=&quot;FarPoint.Web.Spread.LayoutTemplate&quot;&gt;&lt;Layout&gt;&lt;ColumnCount&gt;4&lt;/ColumnCount&gt;&lt;RowCount&gt;1&lt;/RowCount&gt;&lt;/Layout&gt;&lt;Data&gt;&lt;LayoutData class=&quot;FarPoint.Web.Spread.Model.DefaultSheetDataModel&quot; rows=&quot;1&quot; columns=&quot;4&quot;&gt;&lt;AutoCalculation&gt;True&lt;/AutoCalculation&gt;&lt;AutoGenerateColumns&gt;True&lt;/AutoGenerateColumns&gt;&lt;ReferenceStyle&gt;A1&lt;/ReferenceStyle&gt;&lt;Iteration&gt;False&lt;/Iteration&gt;&lt;MaximumIterations&gt;1&lt;/MaximumIterations&gt;&lt;MaximumChange&gt;0.001&lt;/MaximumChange&gt;&lt;Cells&gt;&lt;Cell row=&quot;0&quot; column=&quot;0&quot;&gt;&lt;Data type=&quot;System.Int32&quot;&gt;0&lt;/Data&gt;&lt;/Cell&gt;&lt;Cell row=&quot;0&quot; column=&quot;1&quot;&gt;&lt;Data type=&quot;System.Int32&quot;&gt;1&lt;/Data&gt;&lt;/Cell&gt;&lt;Cell row=&quot;0&quot; column=&quot;2&quot;&gt;&lt;Data type=&quot;System.Int32&quot;&gt;2&lt;/Data&gt;&lt;/Cell&gt;&lt;Cell row=&quot;0&quot; column=&quot;3&quot;&gt;&lt;Data type=&quot;System.Int32&quot;&gt;3&lt;/Data&gt;&lt;/Cell&gt;&lt;/Cells&gt;&lt;/LayoutData&gt;&lt;/Data&gt;&lt;AxisModels&gt;&lt;LayoutColumn class=&quot;FarPoint.Web.Spread.Model.DefaultSheetAxisModel&quot; orientation=&quot;Horizontal&quot; count=&quot;4&quot;&gt;&lt;Items&gt;&lt;Item index=&quot;-1&quot;&gt;&lt;SortIndicator&gt;Ascending&lt;/SortIndicator&gt;&lt;/Item&gt;&lt;/Items&gt;&lt;/LayoutColumn&gt;&lt;LayoutRow class=&quot;FarPoint.Web.Spread.Model.DefaultSheetAxisModel&quot; orientation=&quot;Vertical&quot; count=&quot;1&quot;&gt;&lt;Items&gt;&lt;Item index=&quot;-1&quot; /&gt;&lt;/Items&gt;&lt;/LayoutRow&gt;&lt;/AxisModels&gt;&lt;/LayoutTemplate&gt;&lt;LayoutMode&gt;CellLayoutMode&lt;/LayoutMode&gt;&lt;CurrentPageIndex type=&quot;System.Int32&quot;&gt;0&lt;/CurrentPageIndex&gt;&lt;/Settings&gt;&lt;/Sheet&gt;"
                                        PageSize="100" SheetName="Sheet1">
                                    </FarPoint:SheetView>
                                </Sheets>
                                <TitleInfo BackColor="#E7EFF7" Font-Bold="False" Font-Italic="False" Font-Overline="False"
                                    Font-Size="X-Large" Font-Strikeout="False" Font-Underline="False" ForeColor=""
                                    HorizontalAlign="Center" Text="" VerticalAlign="NotSet">
                                </TitleInfo>
                            </FarPoint:FpSpread>
                        </td>
                    </tr>
                    <tr align="center" style="margin: 0px; margin-top: 15px;">
                        <td colspan="4">
                            <table>
                                <tr>
                                    <td>
                                        <asp:Button ID="btnsave" Text="Save" runat="server" OnClick="btnsave_Click" Font-Bold="true"
                                            Font-Names="Book Antiqua" />
                                    </td>
                                    <td>
                                        <asp:Button ID="btndelete" Text="Clear" runat="server" OnClick="btndelete_Click"
                                            Font-Bold="true" Font-Names="Book Antiqua" Visible="False" />
                                    </td>
                                    <td>
                                        <asp:Button ID="btnclassadvisor" runat="server" Text="Class Advisor" Font-Bold="True"
                                            Font-Names="Book Antiqua" Font-Size="Medium" OnClick="btnclassadvisor_Click"
                                            Visible="False" />
                                    </td>
                                    <td>
                                        <asp:CheckBox ID="chkSectionWise" runat="server" Text="All Section Staff" Visible="false"
                                            Checked="false" />
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                </table>
                <br />
                <table style="text-align: left;">
                    <tr>
                        <td>
                            <asp:Label ID="lblday" runat="server" Text="Day :" Font-Bold="true" Font-Names="Book Antiqua"></asp:Label>
                        </td>
                        <td>
                            <asp:Label ID="lblday1" runat="server" Font-Bold="true" Font-Names="Book Antiqua"></asp:Label>
                        </td>
                        <td>
                            <asp:Label ID="lbltimings" runat="server" Text="Timings" Font-Bold="true" Font-Names="Book Antiqua"></asp:Label>
                        </td>
                        <td>
                            <asp:Label ID="lblfromtime" runat="server" Font-Bold="true" Font-Names="Book Antiqua"></asp:Label>
                        </td>
                        <td>
                            <asp:Label ID="lbltotime" runat="server" Font-Bold="true" Font-Names="Book Antiqua"></asp:Label>
                        </td>
                    </tr>
                </table>
                <br />
                <FarPoint:FpSpread ID="fpdetails" runat="server" ActiveSheetViewIndex="0" BorderColor="Black"
                    BorderStyle="Solid" BorderWidth="1px" CommandBar-Font-Names="Arial" CssClass="cursorptr"
                    currentPageIndex="0" DesignString="&lt;?xml version=&quot;1.0&quot; encoding=&quot;utf-8&quot;?&gt;&lt;Spread /&gt;"
                    Height="212px" ScrollBar3DLightColor="Red" ScrollBarArrowColor="Aqua" ScrollBarBaseColor="Goldenrod"
                    ScrollBarDarkShadowColor="#FF8080" ScrollBarFaceColor="#99FF66" scrollContent="true"
                    scrollContentColumns="" scrollContentMaxHeight="50" scrollContentTime="500" Width="831px"
                    ShowHeaderSelection="false">
                    <CommandBar BackColor="Control" ButtonFaceColor="Control" ButtonHighlightColor="ControlLightLight"
                        ButtonShadowColor="ControlDark" Font-Bold="False" Font-Italic="False" Font-Overline="False"
                        Font-Strikeout="False" Font-Underline="False" Visible="False">
                        <Background BackgroundImageUrl="SPREADCLIENTPATH:/img/cbbg.gif" />
                    </CommandBar>
                    <Pager Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                        Font-Underline="False" />
                    <HierBar Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                        Font-Underline="False" />
                    <Sheets>
                        <FarPoint:SheetView AllowPage="False" DesignString="&lt;?xml version=&quot;1.0&quot; encoding=&quot;utf-8&quot;?&gt;&lt;Sheet&gt;&lt;Data&gt;&lt;RowHeader class=&quot;FarPoint.Web.Spread.Model.DefaultSheetDataModel&quot; rows=&quot;3&quot; columns=&quot;1&quot;&gt;&lt;AutoCalculation&gt;True&lt;/AutoCalculation&gt;&lt;AutoGenerateColumns&gt;True&lt;/AutoGenerateColumns&gt;&lt;ReferenceStyle&gt;A1&lt;/ReferenceStyle&gt;&lt;Iteration&gt;False&lt;/Iteration&gt;&lt;MaximumIterations&gt;1&lt;/MaximumIterations&gt;&lt;MaximumChange&gt;0.001&lt;/MaximumChange&gt;&lt;/RowHeader&gt;&lt;ColumnHeader class=&quot;FarPoint.Web.Spread.Model.DefaultSheetDataModel&quot; rows=&quot;1&quot; columns=&quot;4&quot;&gt;&lt;AutoCalculation&gt;True&lt;/AutoCalculation&gt;&lt;AutoGenerateColumns&gt;True&lt;/AutoGenerateColumns&gt;&lt;ReferenceStyle&gt;A1&lt;/ReferenceStyle&gt;&lt;Iteration&gt;False&lt;/Iteration&gt;&lt;MaximumIterations&gt;1&lt;/MaximumIterations&gt;&lt;MaximumChange&gt;0.001&lt;/MaximumChange&gt;&lt;/ColumnHeader&gt;&lt;DataArea class=&quot;FarPoint.Web.Spread.Model.DefaultSheetDataModel&quot; rows=&quot;3&quot; columns=&quot;4&quot;&gt;&lt;AutoCalculation&gt;True&lt;/AutoCalculation&gt;&lt;AutoGenerateColumns&gt;True&lt;/AutoGenerateColumns&gt;&lt;ReferenceStyle&gt;A1&lt;/ReferenceStyle&gt;&lt;Iteration&gt;False&lt;/Iteration&gt;&lt;MaximumIterations&gt;1&lt;/MaximumIterations&gt;&lt;MaximumChange&gt;0.001&lt;/MaximumChange&gt;&lt;SheetName&gt;Sheet1&lt;/SheetName&gt;&lt;/DataArea&gt;&lt;SheetCorner class=&quot;FarPoint.Web.Spread.Model.DefaultSheetDataModel&quot; rows=&quot;1&quot; columns=&quot;1&quot;&gt;&lt;AutoCalculation&gt;True&lt;/AutoCalculation&gt;&lt;AutoGenerateColumns&gt;True&lt;/AutoGenerateColumns&gt;&lt;ReferenceStyle&gt;A1&lt;/ReferenceStyle&gt;&lt;Iteration&gt;False&lt;/Iteration&gt;&lt;MaximumIterations&gt;1&lt;/MaximumIterations&gt;&lt;MaximumChange&gt;0.001&lt;/MaximumChange&gt;&lt;/SheetCorner&gt;&lt;ColumnFooter class=&quot;FarPoint.Web.Spread.Model.DefaultSheetDataModel&quot; rows=&quot;1&quot; columns=&quot;4&quot;&gt;&lt;AutoCalculation&gt;True&lt;/AutoCalculation&gt;&lt;AutoGenerateColumns&gt;True&lt;/AutoGenerateColumns&gt;&lt;ReferenceStyle&gt;A1&lt;/ReferenceStyle&gt;&lt;Iteration&gt;False&lt;/Iteration&gt;&lt;MaximumIterations&gt;1&lt;/MaximumIterations&gt;&lt;MaximumChange&gt;0.001&lt;/MaximumChange&gt;&lt;/ColumnFooter&gt;&lt;/Data&gt;&lt;Presentation&gt;&lt;ActiveSkin class=&quot;FarPoint.Web.Spread.SheetSkin&quot;&gt;&lt;Name&gt;Default&lt;/Name&gt;&lt;BackColor&gt;Empty&lt;/BackColor&gt;&lt;CellBackColor&gt;Empty&lt;/CellBackColor&gt;&lt;CellForeColor&gt;Empty&lt;/CellForeColor&gt;&lt;CellSpacing&gt;0&lt;/CellSpacing&gt;&lt;GridLines&gt;Both&lt;/GridLines&gt;&lt;GridLineColor&gt;#d0d7e5&lt;/GridLineColor&gt;&lt;HeaderBackColor&gt;Empty&lt;/HeaderBackColor&gt;&lt;HeaderForeColor&gt;Empty&lt;/HeaderForeColor&gt;&lt;FlatColumnHeader&gt;False&lt;/FlatColumnHeader&gt;&lt;FooterBackColor&gt;Empty&lt;/FooterBackColor&gt;&lt;FooterForeColor&gt;Empty&lt;/FooterForeColor&gt;&lt;FlatColumnFooter&gt;False&lt;/FlatColumnFooter&gt;&lt;FlatRowHeader&gt;False&lt;/FlatRowHeader&gt;&lt;HeaderFontBold&gt;False&lt;/HeaderFontBold&gt;&lt;FooterFontBold&gt;False&lt;/FooterFontBold&gt;&lt;SelectionBackColor&gt;#eaecf5&lt;/SelectionBackColor&gt;&lt;SelectionForeColor&gt;Empty&lt;/SelectionForeColor&gt;&lt;EvenRowBackColor&gt;Empty&lt;/EvenRowBackColor&gt;&lt;OddRowBackColor&gt;Empty&lt;/OddRowBackColor&gt;&lt;ShowColumnHeader&gt;True&lt;/ShowColumnHeader&gt;&lt;ShowColumnFooter&gt;False&lt;/ShowColumnFooter&gt;&lt;ShowRowHeader&gt;True&lt;/ShowRowHeader&gt;&lt;ColumnHeaderBackground class=&quot;FarPoint.Web.Spread.Background&quot;&gt;&lt;BackgroundImageUrl&gt;SPREADCLIENTPATH:/img/chbg.gif&lt;/BackgroundImageUrl&gt;&lt;SelectedBackgroundImageUrl&gt;SPREADCLIENTPATH:/img/chm.png&lt;/SelectedBackgroundImageUrl&gt;&lt;/ColumnHeaderBackground&gt;&lt;SheetCornerBackground class=&quot;FarPoint.Web.Spread.Background&quot;&gt;&lt;BackgroundImageUrl&gt;SPREADCLIENTPATH:/img/chbg.gif&lt;/BackgroundImageUrl&gt;&lt;SelectedBackgroundImageUrl&gt;SPREADCLIENTPATH:/img/chm.png&lt;/SelectedBackgroundImageUrl&gt;&lt;/SheetCornerBackground&gt;&lt;HeaderGrayAreaColor&gt;#7999c2&lt;/HeaderGrayAreaColor&gt;&lt;/ActiveSkin&gt;&lt;AxisModels&gt;&lt;Column class=&quot;FarPoint.Web.Spread.Model.DefaultSheetAxisModel&quot; orientation=&quot;Horizontal&quot; count=&quot;4&quot;&gt;&lt;Items&gt;&lt;Item index=&quot;-1&quot;&gt;&lt;SortIndicator&gt;Ascending&lt;/SortIndicator&gt;&lt;/Item&gt;&lt;Item index=&quot;0&quot;&gt;&lt;Size&gt;55&lt;/Size&gt;&lt;/Item&gt;&lt;Item index=&quot;1&quot;&gt;&lt;Size&gt;111&lt;/Size&gt;&lt;/Item&gt;&lt;Item index=&quot;2&quot;&gt;&lt;Size&gt;81&lt;/Size&gt;&lt;/Item&gt;&lt;Item index=&quot;3&quot;&gt;&lt;Size&gt;79&lt;/Size&gt;&lt;/Item&gt;&lt;/Items&gt;&lt;/Column&gt;&lt;RowHeaderColumn class=&quot;FarPoint.Web.Spread.Model.DefaultSheetAxisModel&quot; defaultSize=&quot;40&quot; orientation=&quot;Horizontal&quot; count=&quot;1&quot;&gt;&lt;Items&gt;&lt;Item index=&quot;-1&quot;&gt;&lt;SortIndicator&gt;Ascending&lt;/SortIndicator&gt;&lt;Size&gt;40&lt;/Size&gt;&lt;/Item&gt;&lt;/Items&gt;&lt;/RowHeaderColumn&gt;&lt;ColumnHeaderRow class=&quot;FarPoint.Web.Spread.Model.DefaultSheetAxisModel&quot; defaultSize=&quot;22&quot; orientation=&quot;Vertical&quot; count=&quot;1&quot;&gt;&lt;Items&gt;&lt;Item index=&quot;-1&quot;&gt;&lt;Size&gt;22&lt;/Size&gt;&lt;/Item&gt;&lt;/Items&gt;&lt;/ColumnHeaderRow&gt;&lt;ColumnFooterRow class=&quot;FarPoint.Web.Spread.Model.DefaultSheetAxisModel&quot; defaultSize=&quot;22&quot; orientation=&quot;Vertical&quot; count=&quot;1&quot;&gt;&lt;Items&gt;&lt;Item index=&quot;-1&quot;&gt;&lt;Size&gt;22&lt;/Size&gt;&lt;/Item&gt;&lt;/Items&gt;&lt;/ColumnFooterRow&gt;&lt;/AxisModels&gt;&lt;StyleModels&gt;&lt;RowHeader class=&quot;FarPoint.Web.Spread.Model.DefaultSheetStyleModel&quot; Rows=&quot;3&quot; Columns=&quot;1&quot;&gt;&lt;AltRowCount&gt;2&lt;/AltRowCount&gt;&lt;DefaultStyle class=&quot;FarPoint.Web.Spread.NamedStyle&quot; Parent=&quot;RowHeaderDefault&quot; /&gt;&lt;ConditionalFormatCollections /&gt;&lt;/RowHeader&gt;&lt;ColumnHeader class=&quot;FarPoint.Web.Spread.Model.DefaultSheetStyleModel&quot; Rows=&quot;1&quot; Columns=&quot;4&quot;&gt;&lt;AltRowCount&gt;2&lt;/AltRowCount&gt;&lt;DefaultStyle class=&quot;FarPoint.Web.Spread.NamedStyle&quot; Parent=&quot;ColumnHeaderDefault&quot;&gt;&lt;Background class=&quot;FarPoint.Web.Spread.Background&quot;&gt;&lt;BackgroundImageUrl&gt;SPREADCLIENTPATH:/img/chbg.gif&lt;/BackgroundImageUrl&gt;&lt;SelectedBackgroundImageUrl&gt;SPREADCLIENTPATH:/img/chm.png&lt;/SelectedBackgroundImageUrl&gt;&lt;/Background&gt;&lt;/DefaultStyle&gt;&lt;ConditionalFormatCollections /&gt;&lt;/ColumnHeader&gt;&lt;DataArea class=&quot;FarPoint.Web.Spread.Model.DefaultSheetStyleModel&quot; Rows=&quot;3&quot; Columns=&quot;4&quot;&gt;&lt;AltRowCount&gt;2&lt;/AltRowCount&gt;&lt;DefaultStyle class=&quot;FarPoint.Web.Spread.NamedStyle&quot; Parent=&quot;DataAreaDefault&quot;&gt;&lt;Font&gt;&lt;Name&gt;Book Antiqua&lt;/Name&gt;&lt;Names&gt;&lt;Name&gt;Book Antiqua&lt;/Name&gt;&lt;/Names&gt;&lt;Size&gt;Medium&lt;/Size&gt;&lt;Bold&gt;False&lt;/Bold&gt;&lt;Italic&gt;False&lt;/Italic&gt;&lt;Overline&gt;False&lt;/Overline&gt;&lt;Strikeout&gt;False&lt;/Strikeout&gt;&lt;Underline&gt;False&lt;/Underline&gt;&lt;/Font&gt;&lt;GdiCharSet&gt;254&lt;/GdiCharSet&gt;&lt;ForeColor&gt;#0033cc&lt;/ForeColor&gt;&lt;HorizontalAlign&gt;Center&lt;/HorizontalAlign&gt;&lt;/DefaultStyle&gt;&lt;ConditionalFormatCollections /&gt;&lt;/DataArea&gt;&lt;SheetCorner class=&quot;FarPoint.Web.Spread.Model.DefaultSheetStyleModel&quot; Rows=&quot;1&quot; Columns=&quot;1&quot;&gt;&lt;AltRowCount&gt;2&lt;/AltRowCount&gt;&lt;DefaultStyle class=&quot;FarPoint.Web.Spread.NamedStyle&quot; Parent=&quot;CornerDefault&quot;&gt;&lt;Border class=&quot;FarPoint.Web.Spread.Border&quot; Size=&quot;1&quot; Style=&quot;Solid&quot;&gt;&lt;Bottom Color=&quot;ControlDark&quot; /&gt;&lt;Left Size=&quot;0&quot; /&gt;&lt;Right Color=&quot;ControlDark&quot; /&gt;&lt;Top Size=&quot;0&quot; /&gt;&lt;/Border&gt;&lt;Background class=&quot;FarPoint.Web.Spread.Background&quot;&gt;&lt;BackgroundImageUrl&gt;SPREADCLIENTPATH:/img/chbg.gif&lt;/BackgroundImageUrl&gt;&lt;SelectedBackgroundImageUrl&gt;SPREADCLIENTPATH:/img/chm.png&lt;/SelectedBackgroundImageUrl&gt;&lt;/Background&gt;&lt;/DefaultStyle&gt;&lt;ConditionalFormatCollections /&gt;&lt;/SheetCorner&gt;&lt;ColumnFooter class=&quot;FarPoint.Web.Spread.Model.DefaultSheetStyleModel&quot; Rows=&quot;1&quot; Columns=&quot;4&quot;&gt;&lt;AltRowCount&gt;2&lt;/AltRowCount&gt;&lt;DefaultStyle class=&quot;FarPoint.Web.Spread.NamedStyle&quot; Parent=&quot;ColumnFooterDefault&quot; /&gt;&lt;ConditionalFormatCollections /&gt;&lt;/ColumnFooter&gt;&lt;/StyleModels&gt;&lt;MessageRowStyle class=&quot;FarPoint.Web.Spread.Appearance&quot;&gt;&lt;BackColor&gt;LightYellow&lt;/BackColor&gt;&lt;ForeColor&gt;Red&lt;/ForeColor&gt;&lt;/MessageRowStyle&gt;&lt;SheetCornerStyle class=&quot;FarPoint.Web.Spread.NamedStyle&quot; Parent=&quot;CornerDefault&quot;&gt;&lt;Border class=&quot;FarPoint.Web.Spread.Border&quot; Size=&quot;1&quot; Style=&quot;Solid&quot;&gt;&lt;Bottom Color=&quot;ControlDark&quot; /&gt;&lt;Left Size=&quot;0&quot; /&gt;&lt;Right Color=&quot;ControlDark&quot; /&gt;&lt;Top Size=&quot;0&quot; /&gt;&lt;/Border&gt;&lt;Background class=&quot;FarPoint.Web.Spread.Background&quot;&gt;&lt;BackgroundImageUrl&gt;SPREADCLIENTPATH:/img/chbg.gif&lt;/BackgroundImageUrl&gt;&lt;SelectedBackgroundImageUrl&gt;SPREADCLIENTPATH:/img/chm.png&lt;/SelectedBackgroundImageUrl&gt;&lt;/Background&gt;&lt;/SheetCornerStyle&gt;&lt;AllowLoadOnDemand&gt;false&lt;/AllowLoadOnDemand&gt;&lt;LoadRowIncrement &gt;10&lt;/LoadRowIncrement &gt;&lt;LoadInitRowCount &gt;30&lt;/LoadInitRowCount &gt;&lt;AllowVirtualScrollPaging&gt;false&lt;/AllowVirtualScrollPaging&gt;&lt;TopRow&gt;0&lt;/TopRow&gt;&lt;PreviewRowStyle class=&quot;FarPoint.Web.Spread.PreviewRowInfo&quot; /&gt;&lt;/Presentation&gt;&lt;Settings&gt;&lt;Name&gt;Sheet1&lt;/Name&gt;&lt;Categories&gt;&lt;Appearance&gt;&lt;GridLineColor&gt;#d0d7e5&lt;/GridLineColor&gt;&lt;SelectionBackColor&gt;#eaecf5&lt;/SelectionBackColor&gt;&lt;SelectionBorder class=&quot;FarPoint.Web.Spread.Border&quot; /&gt;&lt;/Appearance&gt;&lt;Behavior&gt;&lt;EditTemplateColumnCount&gt;2&lt;/EditTemplateColumnCount&gt;&lt;ScrollingContentVisible&gt;True&lt;/ScrollingContentVisible&gt;&lt;PageSize&gt;100&lt;/PageSize&gt;&lt;AllowPage&gt;False&lt;/AllowPage&gt;&lt;GroupBarText&gt;Drag a column to group by that column.&lt;/GroupBarText&gt;&lt;/Behavior&gt;&lt;Layout&gt;&lt;RowHeaderColumnCount&gt;1&lt;/RowHeaderColumnCount&gt;&lt;ColumnHeaderRowCount&gt;1&lt;/ColumnHeaderRowCount&gt;&lt;/Layout&gt;&lt;/Categories&gt;&lt;ActiveRow&gt;0&lt;/ActiveRow&gt;&lt;ActiveColumn&gt;0&lt;/ActiveColumn&gt;&lt;ColumnHeaderRowCount&gt;1&lt;/ColumnHeaderRowCount&gt;&lt;ColumnFooterRowCount&gt;1&lt;/ColumnFooterRowCount&gt;&lt;PrintInfo&gt;&lt;Header /&gt;&lt;Footer /&gt;&lt;ZoomFactor&gt;0&lt;/ZoomFactor&gt;&lt;FirstPageNumber&gt;1&lt;/FirstPageNumber&gt;&lt;Orientation&gt;Auto&lt;/Orientation&gt;&lt;PrintType&gt;All&lt;/PrintType&gt;&lt;PageOrder&gt;Auto&lt;/PageOrder&gt;&lt;BestFitCols&gt;False&lt;/BestFitCols&gt;&lt;BestFitRows&gt;False&lt;/BestFitRows&gt;&lt;PageStart&gt;-1&lt;/PageStart&gt;&lt;PageEnd&gt;-1&lt;/PageEnd&gt;&lt;ColStart&gt;-1&lt;/ColStart&gt;&lt;ColEnd&gt;-1&lt;/ColEnd&gt;&lt;RowStart&gt;-1&lt;/RowStart&gt;&lt;RowEnd&gt;-1&lt;/RowEnd&gt;&lt;ShowBorder&gt;True&lt;/ShowBorder&gt;&lt;ShowGrid&gt;True&lt;/ShowGrid&gt;&lt;ShowColor&gt;True&lt;/ShowColor&gt;&lt;ShowColumnHeader&gt;Inherit&lt;/ShowColumnHeader&gt;&lt;ShowRowHeader&gt;Inherit&lt;/ShowRowHeader&gt;&lt;ShowColumnFooter&gt;Inherit&lt;/ShowColumnFooter&gt;&lt;ShowColumnFooterEachPage&gt;True&lt;/ShowColumnFooterEachPage&gt;&lt;ShowTitle&gt;True&lt;/ShowTitle&gt;&lt;ShowSubtitle&gt;True&lt;/ShowSubtitle&gt;&lt;UseMax&gt;True&lt;/UseMax&gt;&lt;UseSmartPrint&gt;False&lt;/UseSmartPrint&gt;&lt;Opacity&gt;255&lt;/Opacity&gt;&lt;PrintNotes&gt;None&lt;/PrintNotes&gt;&lt;Centering&gt;None&lt;/Centering&gt;&lt;RepeatColStart&gt;-1&lt;/RepeatColStart&gt;&lt;RepeatColEnd&gt;-1&lt;/RepeatColEnd&gt;&lt;RepeatRowStart&gt;-1&lt;/RepeatRowStart&gt;&lt;RepeatRowEnd&gt;-1&lt;/RepeatRowEnd&gt;&lt;SmartPrintPagesTall&gt;1&lt;/SmartPrintPagesTall&gt;&lt;SmartPrintPagesWide&gt;1&lt;/SmartPrintPagesWide&gt;&lt;HeaderHeight&gt;-1&lt;/HeaderHeight&gt;&lt;FooterHeight&gt;-1&lt;/FooterHeight&gt;&lt;/PrintInfo&gt;&lt;TitleInfo class=&quot;FarPoint.Web.Spread.TitleInfo&quot;&gt;&lt;Style class=&quot;FarPoint.Web.Spread.StyleInfo&quot;&gt;&lt;BackColor&gt;#e7eff7&lt;/BackColor&gt;&lt;HorizontalAlign&gt;Right&lt;/HorizontalAlign&gt;&lt;/Style&gt;&lt;Value type=&quot;System.String&quot; whitespace=&quot;&quot; /&gt;&lt;/TitleInfo&gt;&lt;LayoutTemplate class=&quot;FarPoint.Web.Spread.LayoutTemplate&quot;&gt;&lt;Layout&gt;&lt;ColumnCount&gt;4&lt;/ColumnCount&gt;&lt;RowCount&gt;1&lt;/RowCount&gt;&lt;/Layout&gt;&lt;Data&gt;&lt;LayoutData class=&quot;FarPoint.Web.Spread.Model.DefaultSheetDataModel&quot; rows=&quot;1&quot; columns=&quot;4&quot;&gt;&lt;AutoCalculation&gt;True&lt;/AutoCalculation&gt;&lt;AutoGenerateColumns&gt;True&lt;/AutoGenerateColumns&gt;&lt;ReferenceStyle&gt;A1&lt;/ReferenceStyle&gt;&lt;Iteration&gt;False&lt;/Iteration&gt;&lt;MaximumIterations&gt;1&lt;/MaximumIterations&gt;&lt;MaximumChange&gt;0.001&lt;/MaximumChange&gt;&lt;Cells&gt;&lt;Cell row=&quot;0&quot; column=&quot;0&quot;&gt;&lt;Data type=&quot;System.Int32&quot;&gt;0&lt;/Data&gt;&lt;/Cell&gt;&lt;Cell row=&quot;0&quot; column=&quot;1&quot;&gt;&lt;Data type=&quot;System.Int32&quot;&gt;1&lt;/Data&gt;&lt;/Cell&gt;&lt;Cell row=&quot;0&quot; column=&quot;2&quot;&gt;&lt;Data type=&quot;System.Int32&quot;&gt;2&lt;/Data&gt;&lt;/Cell&gt;&lt;Cell row=&quot;0&quot; column=&quot;3&quot;&gt;&lt;Data type=&quot;System.Int32&quot;&gt;3&lt;/Data&gt;&lt;/Cell&gt;&lt;/Cells&gt;&lt;/LayoutData&gt;&lt;/Data&gt;&lt;AxisModels&gt;&lt;LayoutColumn class=&quot;FarPoint.Web.Spread.Model.DefaultSheetAxisModel&quot; orientation=&quot;Horizontal&quot; count=&quot;4&quot;&gt;&lt;Items&gt;&lt;Item index=&quot;-1&quot;&gt;&lt;SortIndicator&gt;Ascending&lt;/SortIndicator&gt;&lt;/Item&gt;&lt;/Items&gt;&lt;/LayoutColumn&gt;&lt;LayoutRow class=&quot;FarPoint.Web.Spread.Model.DefaultSheetAxisModel&quot; orientation=&quot;Vertical&quot; count=&quot;1&quot;&gt;&lt;Items&gt;&lt;Item index=&quot;-1&quot; /&gt;&lt;/Items&gt;&lt;/LayoutRow&gt;&lt;/AxisModels&gt;&lt;/LayoutTemplate&gt;&lt;LayoutMode&gt;CellLayoutMode&lt;/LayoutMode&gt;&lt;CurrentPageIndex type=&quot;System.Int32&quot;&gt;0&lt;/CurrentPageIndex&gt;&lt;/Settings&gt;&lt;/Sheet&gt;"
                            PageSize="100" SheetName="Sheet1">
                        </FarPoint:SheetView>
                    </Sheets>
                    <TitleInfo BackColor="#E7EFF7" Font-Bold="False" Font-Italic="False" Font-Overline="False"
                        Font-Size="X-Large" Font-Strikeout="False" Font-Underline="False" ForeColor=""
                        HorizontalAlign="Center" Text="" VerticalAlign="NotSet">
                    </TitleInfo>
                </FarPoint:FpSpread>
                <br />
                <br />
                <FarPoint:FpSpread ID="Fpclassadvisor" runat="server" ActiveSheetViewIndex="0" BorderColor="Black"
                    BorderStyle="Solid" BorderWidth="1px" CommandBar-Font-Names="Arial" CssClass="cursorptr"
                    currentPageIndex="0" DesignString="&lt;?xml version=&quot;1.0&quot; encoding=&quot;utf-8&quot;?&gt;&lt;Spread /&gt;"
                    Height="212px" ScrollBar3DLightColor="Red" ScrollBarArrowColor="Aqua" ScrollBarBaseColor="Goldenrod"
                    ScrollBarDarkShadowColor="#FF8080" ScrollBarFaceColor="#99FF66" scrollContent="true"
                    scrollContentColumns="" scrollContentMaxHeight="50" scrollContentTime="500" HorizontalScrollBarPolicy="Never"
                    VerticalScrollBarPolicy="Never" OnButtonCommand="Fpclassadvisor_ButtonCommand"
                    ShowHeaderSelection="false">
                    <CommandBar BackColor="Control" ButtonFaceColor="Control" ButtonHighlightColor="ControlLightLight"
                        ButtonShadowColor="ControlDark" Font-Bold="False" Font-Italic="False" Font-Overline="False"
                        Font-Strikeout="False" Font-Underline="False" Visible="False">
                        <Background BackgroundImageUrl="SPREADCLIENTPATH:/img/cbbg.gif" />
                    </CommandBar>
                    <Pager Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                        Font-Underline="False" />
                    <HierBar Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                        Font-Underline="False" />
                    <Sheets>
                        <FarPoint:SheetView AllowPage="False" DesignString="&lt;?xml version=&quot;1.0&quot; encoding=&quot;utf-8&quot;?&gt;&lt;Sheet&gt;&lt;Data&gt;&lt;RowHeader class=&quot;FarPoint.Web.Spread.Model.DefaultSheetDataModel&quot; rows=&quot;3&quot; columns=&quot;1&quot;&gt;&lt;AutoCalculation&gt;True&lt;/AutoCalculation&gt;&lt;AutoGenerateColumns&gt;True&lt;/AutoGenerateColumns&gt;&lt;ReferenceStyle&gt;A1&lt;/ReferenceStyle&gt;&lt;Iteration&gt;False&lt;/Iteration&gt;&lt;MaximumIterations&gt;1&lt;/MaximumIterations&gt;&lt;MaximumChange&gt;0.001&lt;/MaximumChange&gt;&lt;/RowHeader&gt;&lt;ColumnHeader class=&quot;FarPoint.Web.Spread.Model.DefaultSheetDataModel&quot; rows=&quot;1&quot; columns=&quot;4&quot;&gt;&lt;AutoCalculation&gt;True&lt;/AutoCalculation&gt;&lt;AutoGenerateColumns&gt;True&lt;/AutoGenerateColumns&gt;&lt;ReferenceStyle&gt;A1&lt;/ReferenceStyle&gt;&lt;Iteration&gt;False&lt;/Iteration&gt;&lt;MaximumIterations&gt;1&lt;/MaximumIterations&gt;&lt;MaximumChange&gt;0.001&lt;/MaximumChange&gt;&lt;/ColumnHeader&gt;&lt;DataArea class=&quot;FarPoint.Web.Spread.Model.DefaultSheetDataModel&quot; rows=&quot;3&quot; columns=&quot;4&quot;&gt;&lt;AutoCalculation&gt;True&lt;/AutoCalculation&gt;&lt;AutoGenerateColumns&gt;True&lt;/AutoGenerateColumns&gt;&lt;ReferenceStyle&gt;A1&lt;/ReferenceStyle&gt;&lt;Iteration&gt;False&lt;/Iteration&gt;&lt;MaximumIterations&gt;1&lt;/MaximumIterations&gt;&lt;MaximumChange&gt;0.001&lt;/MaximumChange&gt;&lt;SheetName&gt;Sheet1&lt;/SheetName&gt;&lt;/DataArea&gt;&lt;SheetCorner class=&quot;FarPoint.Web.Spread.Model.DefaultSheetDataModel&quot; rows=&quot;1&quot; columns=&quot;1&quot;&gt;&lt;AutoCalculation&gt;True&lt;/AutoCalculation&gt;&lt;AutoGenerateColumns&gt;True&lt;/AutoGenerateColumns&gt;&lt;ReferenceStyle&gt;A1&lt;/ReferenceStyle&gt;&lt;Iteration&gt;False&lt;/Iteration&gt;&lt;MaximumIterations&gt;1&lt;/MaximumIterations&gt;&lt;MaximumChange&gt;0.001&lt;/MaximumChange&gt;&lt;/SheetCorner&gt;&lt;ColumnFooter class=&quot;FarPoint.Web.Spread.Model.DefaultSheetDataModel&quot; rows=&quot;1&quot; columns=&quot;4&quot;&gt;&lt;AutoCalculation&gt;True&lt;/AutoCalculation&gt;&lt;AutoGenerateColumns&gt;True&lt;/AutoGenerateColumns&gt;&lt;ReferenceStyle&gt;A1&lt;/ReferenceStyle&gt;&lt;Iteration&gt;False&lt;/Iteration&gt;&lt;MaximumIterations&gt;1&lt;/MaximumIterations&gt;&lt;MaximumChange&gt;0.001&lt;/MaximumChange&gt;&lt;/ColumnFooter&gt;&lt;/Data&gt;&lt;Presentation&gt;&lt;ActiveSkin class=&quot;FarPoint.Web.Spread.SheetSkin&quot;&gt;&lt;Name&gt;Default&lt;/Name&gt;&lt;BackColor&gt;Empty&lt;/BackColor&gt;&lt;CellBackColor&gt;Empty&lt;/CellBackColor&gt;&lt;CellForeColor&gt;Empty&lt;/CellForeColor&gt;&lt;CellSpacing&gt;0&lt;/CellSpacing&gt;&lt;GridLines&gt;Both&lt;/GridLines&gt;&lt;GridLineColor&gt;#d0d7e5&lt;/GridLineColor&gt;&lt;HeaderBackColor&gt;Empty&lt;/HeaderBackColor&gt;&lt;HeaderForeColor&gt;Empty&lt;/HeaderForeColor&gt;&lt;FlatColumnHeader&gt;False&lt;/FlatColumnHeader&gt;&lt;FooterBackColor&gt;Empty&lt;/FooterBackColor&gt;&lt;FooterForeColor&gt;Empty&lt;/FooterForeColor&gt;&lt;FlatColumnFooter&gt;False&lt;/FlatColumnFooter&gt;&lt;FlatRowHeader&gt;False&lt;/FlatRowHeader&gt;&lt;HeaderFontBold&gt;False&lt;/HeaderFontBold&gt;&lt;FooterFontBold&gt;False&lt;/FooterFontBold&gt;&lt;SelectionBackColor&gt;#eaecf5&lt;/SelectionBackColor&gt;&lt;SelectionForeColor&gt;Empty&lt;/SelectionForeColor&gt;&lt;EvenRowBackColor&gt;Empty&lt;/EvenRowBackColor&gt;&lt;OddRowBackColor&gt;Empty&lt;/OddRowBackColor&gt;&lt;ShowColumnHeader&gt;True&lt;/ShowColumnHeader&gt;&lt;ShowColumnFooter&gt;False&lt;/ShowColumnFooter&gt;&lt;ShowRowHeader&gt;True&lt;/ShowRowHeader&gt;&lt;ColumnHeaderBackground class=&quot;FarPoint.Web.Spread.Background&quot;&gt;&lt;BackgroundImageUrl&gt;SPREADCLIENTPATH:/img/chbg.gif&lt;/BackgroundImageUrl&gt;&lt;SelectedBackgroundImageUrl&gt;SPREADCLIENTPATH:/img/chm.png&lt;/SelectedBackgroundImageUrl&gt;&lt;/ColumnHeaderBackground&gt;&lt;SheetCornerBackground class=&quot;FarPoint.Web.Spread.Background&quot;&gt;&lt;BackgroundImageUrl&gt;SPREADCLIENTPATH:/img/chbg.gif&lt;/BackgroundImageUrl&gt;&lt;SelectedBackgroundImageUrl&gt;SPREADCLIENTPATH:/img/chm.png&lt;/SelectedBackgroundImageUrl&gt;&lt;/SheetCornerBackground&gt;&lt;HeaderGrayAreaColor&gt;#7999c2&lt;/HeaderGrayAreaColor&gt;&lt;/ActiveSkin&gt;&lt;AxisModels&gt;&lt;Column class=&quot;FarPoint.Web.Spread.Model.DefaultSheetAxisModel&quot; orientation=&quot;Horizontal&quot; count=&quot;4&quot;&gt;&lt;Items&gt;&lt;Item index=&quot;-1&quot;&gt;&lt;SortIndicator&gt;Ascending&lt;/SortIndicator&gt;&lt;/Item&gt;&lt;Item index=&quot;0&quot;&gt;&lt;Size&gt;55&lt;/Size&gt;&lt;/Item&gt;&lt;Item index=&quot;1&quot;&gt;&lt;Size&gt;111&lt;/Size&gt;&lt;/Item&gt;&lt;Item index=&quot;2&quot;&gt;&lt;Size&gt;81&lt;/Size&gt;&lt;/Item&gt;&lt;Item index=&quot;3&quot;&gt;&lt;Size&gt;79&lt;/Size&gt;&lt;/Item&gt;&lt;/Items&gt;&lt;/Column&gt;&lt;RowHeaderColumn class=&quot;FarPoint.Web.Spread.Model.DefaultSheetAxisModel&quot; defaultSize=&quot;40&quot; orientation=&quot;Horizontal&quot; count=&quot;1&quot;&gt;&lt;Items&gt;&lt;Item index=&quot;-1&quot;&gt;&lt;SortIndicator&gt;Ascending&lt;/SortIndicator&gt;&lt;Size&gt;40&lt;/Size&gt;&lt;/Item&gt;&lt;/Items&gt;&lt;/RowHeaderColumn&gt;&lt;ColumnHeaderRow class=&quot;FarPoint.Web.Spread.Model.DefaultSheetAxisModel&quot; defaultSize=&quot;22&quot; orientation=&quot;Vertical&quot; count=&quot;1&quot;&gt;&lt;Items&gt;&lt;Item index=&quot;-1&quot;&gt;&lt;Size&gt;22&lt;/Size&gt;&lt;/Item&gt;&lt;/Items&gt;&lt;/ColumnHeaderRow&gt;&lt;ColumnFooterRow class=&quot;FarPoint.Web.Spread.Model.DefaultSheetAxisModel&quot; defaultSize=&quot;22&quot; orientation=&quot;Vertical&quot; count=&quot;1&quot;&gt;&lt;Items&gt;&lt;Item index=&quot;-1&quot;&gt;&lt;Size&gt;22&lt;/Size&gt;&lt;/Item&gt;&lt;/Items&gt;&lt;/ColumnFooterRow&gt;&lt;/AxisModels&gt;&lt;StyleModels&gt;&lt;RowHeader class=&quot;FarPoint.Web.Spread.Model.DefaultSheetStyleModel&quot; Rows=&quot;3&quot; Columns=&quot;1&quot;&gt;&lt;AltRowCount&gt;2&lt;/AltRowCount&gt;&lt;DefaultStyle class=&quot;FarPoint.Web.Spread.NamedStyle&quot; Parent=&quot;RowHeaderDefault&quot; /&gt;&lt;ConditionalFormatCollections /&gt;&lt;/RowHeader&gt;&lt;ColumnHeader class=&quot;FarPoint.Web.Spread.Model.DefaultSheetStyleModel&quot; Rows=&quot;1&quot; Columns=&quot;4&quot;&gt;&lt;AltRowCount&gt;2&lt;/AltRowCount&gt;&lt;DefaultStyle class=&quot;FarPoint.Web.Spread.NamedStyle&quot; Parent=&quot;ColumnHeaderDefault&quot;&gt;&lt;Background class=&quot;FarPoint.Web.Spread.Background&quot;&gt;&lt;BackgroundImageUrl&gt;SPREADCLIENTPATH:/img/chbg.gif&lt;/BackgroundImageUrl&gt;&lt;SelectedBackgroundImageUrl&gt;SPREADCLIENTPATH:/img/chm.png&lt;/SelectedBackgroundImageUrl&gt;&lt;/Background&gt;&lt;/DefaultStyle&gt;&lt;ConditionalFormatCollections /&gt;&lt;/ColumnHeader&gt;&lt;DataArea class=&quot;FarPoint.Web.Spread.Model.DefaultSheetStyleModel&quot; Rows=&quot;3&quot; Columns=&quot;4&quot;&gt;&lt;AltRowCount&gt;2&lt;/AltRowCount&gt;&lt;DefaultStyle class=&quot;FarPoint.Web.Spread.NamedStyle&quot; Parent=&quot;DataAreaDefault&quot;&gt;&lt;Font&gt;&lt;Name&gt;Book Antiqua&lt;/Name&gt;&lt;Names&gt;&lt;Name&gt;Book Antiqua&lt;/Name&gt;&lt;/Names&gt;&lt;Size&gt;Medium&lt;/Size&gt;&lt;Bold&gt;False&lt;/Bold&gt;&lt;Italic&gt;False&lt;/Italic&gt;&lt;Overline&gt;False&lt;/Overline&gt;&lt;Strikeout&gt;False&lt;/Strikeout&gt;&lt;Underline&gt;False&lt;/Underline&gt;&lt;/Font&gt;&lt;GdiCharSet&gt;254&lt;/GdiCharSet&gt;&lt;ForeColor&gt;#0033cc&lt;/ForeColor&gt;&lt;HorizontalAlign&gt;Center&lt;/HorizontalAlign&gt;&lt;/DefaultStyle&gt;&lt;ConditionalFormatCollections /&gt;&lt;/DataArea&gt;&lt;SheetCorner class=&quot;FarPoint.Web.Spread.Model.DefaultSheetStyleModel&quot; Rows=&quot;1&quot; Columns=&quot;1&quot;&gt;&lt;AltRowCount&gt;2&lt;/AltRowCount&gt;&lt;DefaultStyle class=&quot;FarPoint.Web.Spread.NamedStyle&quot; Parent=&quot;CornerDefault&quot;&gt;&lt;Border class=&quot;FarPoint.Web.Spread.Border&quot; Size=&quot;1&quot; Style=&quot;Solid&quot;&gt;&lt;Bottom Color=&quot;ControlDark&quot; /&gt;&lt;Left Size=&quot;0&quot; /&gt;&lt;Right Color=&quot;ControlDark&quot; /&gt;&lt;Top Size=&quot;0&quot; /&gt;&lt;/Border&gt;&lt;Background class=&quot;FarPoint.Web.Spread.Background&quot;&gt;&lt;BackgroundImageUrl&gt;SPREADCLIENTPATH:/img/chbg.gif&lt;/BackgroundImageUrl&gt;&lt;SelectedBackgroundImageUrl&gt;SPREADCLIENTPATH:/img/chm.png&lt;/SelectedBackgroundImageUrl&gt;&lt;/Background&gt;&lt;/DefaultStyle&gt;&lt;ConditionalFormatCollections /&gt;&lt;/SheetCorner&gt;&lt;ColumnFooter class=&quot;FarPoint.Web.Spread.Model.DefaultSheetStyleModel&quot; Rows=&quot;1&quot; Columns=&quot;4&quot;&gt;&lt;AltRowCount&gt;2&lt;/AltRowCount&gt;&lt;DefaultStyle class=&quot;FarPoint.Web.Spread.NamedStyle&quot; Parent=&quot;ColumnFooterDefault&quot; /&gt;&lt;ConditionalFormatCollections /&gt;&lt;/ColumnFooter&gt;&lt;/StyleModels&gt;&lt;MessageRowStyle class=&quot;FarPoint.Web.Spread.Appearance&quot;&gt;&lt;BackColor&gt;LightYellow&lt;/BackColor&gt;&lt;ForeColor&gt;Red&lt;/ForeColor&gt;&lt;/MessageRowStyle&gt;&lt;SheetCornerStyle class=&quot;FarPoint.Web.Spread.NamedStyle&quot; Parent=&quot;CornerDefault&quot;&gt;&lt;Border class=&quot;FarPoint.Web.Spread.Border&quot; Size=&quot;1&quot; Style=&quot;Solid&quot;&gt;&lt;Bottom Color=&quot;ControlDark&quot; /&gt;&lt;Left Size=&quot;0&quot; /&gt;&lt;Right Color=&quot;ControlDark&quot; /&gt;&lt;Top Size=&quot;0&quot; /&gt;&lt;/Border&gt;&lt;Background class=&quot;FarPoint.Web.Spread.Background&quot;&gt;&lt;BackgroundImageUrl&gt;SPREADCLIENTPATH:/img/chbg.gif&lt;/BackgroundImageUrl&gt;&lt;SelectedBackgroundImageUrl&gt;SPREADCLIENTPATH:/img/chm.png&lt;/SelectedBackgroundImageUrl&gt;&lt;/Background&gt;&lt;/SheetCornerStyle&gt;&lt;AllowLoadOnDemand&gt;false&lt;/AllowLoadOnDemand&gt;&lt;LoadRowIncrement &gt;10&lt;/LoadRowIncrement &gt;&lt;LoadInitRowCount &gt;30&lt;/LoadInitRowCount &gt;&lt;AllowVirtualScrollPaging&gt;false&lt;/AllowVirtualScrollPaging&gt;&lt;TopRow&gt;0&lt;/TopRow&gt;&lt;PreviewRowStyle class=&quot;FarPoint.Web.Spread.PreviewRowInfo&quot; /&gt;&lt;/Presentation&gt;&lt;Settings&gt;&lt;Name&gt;Sheet1&lt;/Name&gt;&lt;Categories&gt;&lt;Appearance&gt;&lt;GridLineColor&gt;#d0d7e5&lt;/GridLineColor&gt;&lt;SelectionBackColor&gt;#eaecf5&lt;/SelectionBackColor&gt;&lt;SelectionBorder class=&quot;FarPoint.Web.Spread.Border&quot; /&gt;&lt;/Appearance&gt;&lt;Behavior&gt;&lt;EditTemplateColumnCount&gt;2&lt;/EditTemplateColumnCount&gt;&lt;ScrollingContentVisible&gt;True&lt;/ScrollingContentVisible&gt;&lt;PageSize&gt;100&lt;/PageSize&gt;&lt;AllowPage&gt;False&lt;/AllowPage&gt;&lt;GroupBarText&gt;Drag a column to group by that column.&lt;/GroupBarText&gt;&lt;/Behavior&gt;&lt;Layout&gt;&lt;RowHeaderColumnCount&gt;1&lt;/RowHeaderColumnCount&gt;&lt;ColumnHeaderRowCount&gt;1&lt;/ColumnHeaderRowCount&gt;&lt;/Layout&gt;&lt;/Categories&gt;&lt;ActiveRow&gt;0&lt;/ActiveRow&gt;&lt;ActiveColumn&gt;0&lt;/ActiveColumn&gt;&lt;ColumnHeaderRowCount&gt;1&lt;/ColumnHeaderRowCount&gt;&lt;ColumnFooterRowCount&gt;1&lt;/ColumnFooterRowCount&gt;&lt;PrintInfo&gt;&lt;Header /&gt;&lt;Footer /&gt;&lt;ZoomFactor&gt;0&lt;/ZoomFactor&gt;&lt;FirstPageNumber&gt;1&lt;/FirstPageNumber&gt;&lt;Orientation&gt;Auto&lt;/Orientation&gt;&lt;PrintType&gt;All&lt;/PrintType&gt;&lt;PageOrder&gt;Auto&lt;/PageOrder&gt;&lt;BestFitCols&gt;False&lt;/BestFitCols&gt;&lt;BestFitRows&gt;False&lt;/BestFitRows&gt;&lt;PageStart&gt;-1&lt;/PageStart&gt;&lt;PageEnd&gt;-1&lt;/PageEnd&gt;&lt;ColStart&gt;-1&lt;/ColStart&gt;&lt;ColEnd&gt;-1&lt;/ColEnd&gt;&lt;RowStart&gt;-1&lt;/RowStart&gt;&lt;RowEnd&gt;-1&lt;/RowEnd&gt;&lt;ShowBorder&gt;True&lt;/ShowBorder&gt;&lt;ShowGrid&gt;True&lt;/ShowGrid&gt;&lt;ShowColor&gt;True&lt;/ShowColor&gt;&lt;ShowColumnHeader&gt;Inherit&lt;/ShowColumnHeader&gt;&lt;ShowRowHeader&gt;Inherit&lt;/ShowRowHeader&gt;&lt;ShowColumnFooter&gt;Inherit&lt;/ShowColumnFooter&gt;&lt;ShowColumnFooterEachPage&gt;True&lt;/ShowColumnFooterEachPage&gt;&lt;ShowTitle&gt;True&lt;/ShowTitle&gt;&lt;ShowSubtitle&gt;True&lt;/ShowSubtitle&gt;&lt;UseMax&gt;True&lt;/UseMax&gt;&lt;UseSmartPrint&gt;False&lt;/UseSmartPrint&gt;&lt;Opacity&gt;255&lt;/Opacity&gt;&lt;PrintNotes&gt;None&lt;/PrintNotes&gt;&lt;Centering&gt;None&lt;/Centering&gt;&lt;RepeatColStart&gt;-1&lt;/RepeatColStart&gt;&lt;RepeatColEnd&gt;-1&lt;/RepeatColEnd&gt;&lt;RepeatRowStart&gt;-1&lt;/RepeatRowStart&gt;&lt;RepeatRowEnd&gt;-1&lt;/RepeatRowEnd&gt;&lt;SmartPrintPagesTall&gt;1&lt;/SmartPrintPagesTall&gt;&lt;SmartPrintPagesWide&gt;1&lt;/SmartPrintPagesWide&gt;&lt;HeaderHeight&gt;-1&lt;/HeaderHeight&gt;&lt;FooterHeight&gt;-1&lt;/FooterHeight&gt;&lt;/PrintInfo&gt;&lt;TitleInfo class=&quot;FarPoint.Web.Spread.TitleInfo&quot;&gt;&lt;Style class=&quot;FarPoint.Web.Spread.StyleInfo&quot;&gt;&lt;BackColor&gt;#e7eff7&lt;/BackColor&gt;&lt;HorizontalAlign&gt;Right&lt;/HorizontalAlign&gt;&lt;/Style&gt;&lt;Value type=&quot;System.String&quot; whitespace=&quot;&quot; /&gt;&lt;/TitleInfo&gt;&lt;LayoutTemplate class=&quot;FarPoint.Web.Spread.LayoutTemplate&quot;&gt;&lt;Layout&gt;&lt;ColumnCount&gt;4&lt;/ColumnCount&gt;&lt;RowCount&gt;1&lt;/RowCount&gt;&lt;/Layout&gt;&lt;Data&gt;&lt;LayoutData class=&quot;FarPoint.Web.Spread.Model.DefaultSheetDataModel&quot; rows=&quot;1&quot; columns=&quot;4&quot;&gt;&lt;AutoCalculation&gt;True&lt;/AutoCalculation&gt;&lt;AutoGenerateColumns&gt;True&lt;/AutoGenerateColumns&gt;&lt;ReferenceStyle&gt;A1&lt;/ReferenceStyle&gt;&lt;Iteration&gt;False&lt;/Iteration&gt;&lt;MaximumIterations&gt;1&lt;/MaximumIterations&gt;&lt;MaximumChange&gt;0.001&lt;/MaximumChange&gt;&lt;Cells&gt;&lt;Cell row=&quot;0&quot; column=&quot;0&quot;&gt;&lt;Data type=&quot;System.Int32&quot;&gt;0&lt;/Data&gt;&lt;/Cell&gt;&lt;Cell row=&quot;0&quot; column=&quot;1&quot;&gt;&lt;Data type=&quot;System.Int32&quot;&gt;1&lt;/Data&gt;&lt;/Cell&gt;&lt;Cell row=&quot;0&quot; column=&quot;2&quot;&gt;&lt;Data type=&quot;System.Int32&quot;&gt;2&lt;/Data&gt;&lt;/Cell&gt;&lt;Cell row=&quot;0&quot; column=&quot;3&quot;&gt;&lt;Data type=&quot;System.Int32&quot;&gt;3&lt;/Data&gt;&lt;/Cell&gt;&lt;/Cells&gt;&lt;/LayoutData&gt;&lt;/Data&gt;&lt;AxisModels&gt;&lt;LayoutColumn class=&quot;FarPoint.Web.Spread.Model.DefaultSheetAxisModel&quot; orientation=&quot;Horizontal&quot; count=&quot;4&quot;&gt;&lt;Items&gt;&lt;Item index=&quot;-1&quot;&gt;&lt;SortIndicator&gt;Ascending&lt;/SortIndicator&gt;&lt;/Item&gt;&lt;/Items&gt;&lt;/LayoutColumn&gt;&lt;LayoutRow class=&quot;FarPoint.Web.Spread.Model.DefaultSheetAxisModel&quot; orientation=&quot;Vertical&quot; count=&quot;1&quot;&gt;&lt;Items&gt;&lt;Item index=&quot;-1&quot; /&gt;&lt;/Items&gt;&lt;/LayoutRow&gt;&lt;/AxisModels&gt;&lt;/LayoutTemplate&gt;&lt;LayoutMode&gt;CellLayoutMode&lt;/LayoutMode&gt;&lt;CurrentPageIndex type=&quot;System.Int32&quot;&gt;0&lt;/CurrentPageIndex&gt;&lt;/Settings&gt;&lt;/Sheet&gt;"
                            PageSize="100" SheetName="Sheet1">
                        </FarPoint:SheetView>
                    </Sheets>
                    <TitleInfo BackColor="#E7EFF7" Font-Bold="False" Font-Italic="False" Font-Overline="False"
                        Font-Size="X-Large" Font-Strikeout="False" Font-Underline="False" ForeColor=""
                        HorizontalAlign="Center" Text="" VerticalAlign="NotSet">
                    </TitleInfo>
                </FarPoint:FpSpread>
                <br />
                <asp:Panel ID="treepanel" runat="server" BorderStyle="Dotted" BorderColor="ActiveBorder"
                    Height="600px">
                    <table>
                        <tr>
                            <td style="height: 400px; margin: 0px;">
                                <asp:TreeView runat="server" ID="subjtree" BackColor="White" Height="300px" Width="300px"
                                    SelectedNodeStyle-ForeColor="Red" HoverNodeStyle-BackColor="LightBlue" AutoPostBack="true"
                                    OnSelectedNodeChanged="subjtree_SelectedNodeChanged" Font-Names="Book Antiqua"
                                    Font-Size="Small" ForeColor="Black" Style="overflow: scroll; border: 1px solid black;
                                    height: 300px; margin: 0px; margin-top: 10px; width: 300px;">
                                </asp:TreeView>
                            </td>
                            <td style="width: 500px; height: 400px; margin: 0px;">
                                <table>
                                    <tr style="width: auto; height: auto; margin: 0px; margin-bottom: 10px; margin-top: 8px;">
                                        <td>
                                            <asp:Label ID="lblmulstaff" runat="server" Text="For Mulitple Staff Selection Only"
                                                Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium">
                                            </asp:Label>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtmulstaff" runat="server" CssClass="Dropdown_Txt_Box" ReadOnly="true"
                                                Width="100px" Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium">---Select---</asp:TextBox>
                                            <asp:Panel ID="pmulstaff" runat="server" CssClass="multxtpanel" BackColor="White"
                                                BorderColor="Black" BorderStyle="Solid" BorderWidth="1px" ScrollBars="Vertical"
                                                Height="180px" Width="220px">
                                                <asp:CheckBox ID="chkmulstaff" runat="server" Font-Bold="True" OnCheckedChanged="chkmulstaff_ChekedChange"
                                                    Font-Names="Book Antiqua" Font-Size="Medium" Text="Select All" AutoPostBack="True" />
                                                <asp:CheckBoxList ID="chkmullsstaff" runat="server" Font-Size="Medium" AutoPostBack="True"
                                                    Font-Bold="True" Font-Names="Book Antiqua" OnSelectedIndexChanged="chkmullsstaff_SelectedIndexChanged">
                                                </asp:CheckBoxList>
                                            </asp:Panel>
                                            <asp:PopupControlExtender ID="PopupControlExtender2" runat="server" TargetControlID="txtmulstaff"
                                                PopupControlID="pmulstaff" Position="Bottom">
                                            </asp:PopupControlExtender>
                                        </td>
                                        <td>
                                            <asp:Button ID="btnmulstaff" runat="server" Text="Ok" OnClick="btnmulstaff_Click"
                                                Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium" />
                                        </td>
                                        <td>
                                            <asp:Button ID="btnCopySubject" runat="server" Text="Copy Subject" Visible="false"
                                                OnClick="btnCopySubject_Click" Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="4">
                                            <FarPoint:FpSpread ID="FpSpread1" runat="server" Width="500px" Height="222px" ActiveSheetViewIndex="0"
                                                currentPageIndex="0" OnButtonCommand="FpSpread1_ButtonCommand" ShowHeaderSelection="false">
                                                <CommandBar BackColor="Control" ButtonFaceColor="Control" ButtonHighlightColor="ControlLightLight"
                                                    ButtonShadowColor="ControlDark">
                                                    <Background BackgroundImageUrl="SPREADCLIENTPATH:/img/cbbg.gif"></Background>
                                                </CommandBar>
                                                <Pager Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                                    Font-Underline="False" />
                                                <HierBar Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                                    Font-Underline="False" />
                                                <Pager Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                                    Font-Underline="False"></Pager>
                                                <HierBar Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                                    Font-Underline="False"></HierBar>
                                                <Sheets>
                                                    <FarPoint:SheetView SheetName="Sheet1" AutoPostBack="true" GridLineColor="#DEDFDE"
                                                        SelectionBackColor="#CE5D5A" SelectionForeColor="White">
                                                    </FarPoint:SheetView>
                                                </Sheets>
                                                <TitleInfo BackColor="#E7EFF7" ForeColor="" HorizontalAlign="Center" VerticalAlign="NotSet"
                                                    Font-Size="X-Large" Font-Bold="False" Font-Italic="False" Font-Overline="False"
                                                    Font-Strikeout="False" Font-Underline="False">
                                                </TitleInfo>
                                            </FarPoint:FpSpread>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="4">
                                            <asp:GridView ID="gridSelTT" runat="server" HeaderStyle-BackColor="#0CA6CA" AutoGenerateColumns="false">
                                                <Columns>
                                                    <asp:TemplateField HeaderText="Day/Hour">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblDay" runat="server" Text='<%#Eval("Day") %>'></asp:Label>
                                                            <asp:Label ID="lblDayVal" runat="server" Text='<%#Eval("DayVal") %>' Visible="false"></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="1">
                                                        <ItemTemplate>
                                                            <asp:DropDownList ID="ddlH1" runat="server" CssClass="textbox ddlheight" Width="60px">
                                                                <asp:ListItem></asp:ListItem>
                                                                <asp:ListItem Value="1">Allot</asp:ListItem>
                                                            </asp:DropDownList>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="2">
                                                        <ItemTemplate>
                                                            <asp:DropDownList ID="ddlH2" runat="server" CssClass="textbox ddlheight" Width="60px">
                                                                <asp:ListItem></asp:ListItem>
                                                                <asp:ListItem Value="1">Allot</asp:ListItem>
                                                            </asp:DropDownList>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="3">
                                                        <ItemTemplate>
                                                            <asp:DropDownList ID="ddlH3" runat="server" CssClass="textbox ddlheight" Width="60px">
                                                                <asp:ListItem></asp:ListItem>
                                                                <asp:ListItem Value="1">Allot</asp:ListItem>
                                                            </asp:DropDownList>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="4">
                                                        <ItemTemplate>
                                                            <asp:DropDownList ID="ddlH4" runat="server" CssClass="textbox ddlheight" Width="60px">
                                                                <asp:ListItem></asp:ListItem>
                                                                <asp:ListItem Value="1">Allot</asp:ListItem>
                                                            </asp:DropDownList>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="5">
                                                        <ItemTemplate>
                                                            <asp:DropDownList ID="ddlH5" runat="server" CssClass="textbox ddlheight" Width="60px">
                                                                <asp:ListItem></asp:ListItem>
                                                                <asp:ListItem Value="1">Allot</asp:ListItem>
                                                            </asp:DropDownList>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="6">
                                                        <ItemTemplate>
                                                            <asp:DropDownList ID="ddlH6" runat="server" CssClass="textbox ddlheight" Width="60px">
                                                                <asp:ListItem></asp:ListItem>
                                                                <asp:ListItem Value="1">Allot</asp:ListItem>
                                                            </asp:DropDownList>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="7">
                                                        <ItemTemplate>
                                                            <asp:DropDownList ID="ddlH7" runat="server" CssClass="textbox ddlheight" Width="60px">
                                                                <asp:ListItem></asp:ListItem>
                                                                <asp:ListItem Value="1">Allot</asp:ListItem>
                                                            </asp:DropDownList>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="8">
                                                        <ItemTemplate>
                                                            <asp:DropDownList ID="ddlH8" runat="server" CssClass="textbox ddlheight" Width="60px">
                                                                <asp:ListItem></asp:ListItem>
                                                                <asp:ListItem Value="1">Allot</asp:ListItem>
                                                            </asp:DropDownList>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="9" Visible="false">
                                                        <ItemTemplate>
                                                            <asp:DropDownList ID="ddlH9" runat="server" CssClass="textbox ddlheight" Width="60px">
                                                                <asp:ListItem></asp:ListItem>
                                                                <asp:ListItem Value="1">Allot</asp:ListItem>
                                                            </asp:DropDownList>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="10" Visible="false">
                                                        <ItemTemplate>
                                                            <asp:DropDownList ID="ddlH10" runat="server" CssClass="textbox ddlheight" Width="60px">
                                                                <asp:ListItem></asp:ListItem>
                                                                <asp:ListItem Value="1">Allot</asp:ListItem>
                                                            </asp:DropDownList>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                </Columns>
                                            </asp:GridView>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td class="style1">
                            </td>
                            <td class="style12">
                                <asp:CheckBox ID="chkappend" runat="server" Text="Append to the schedule List" Font-Bold="True"
                                    Font-Names="Book Antiqua" Font-Size="Medium" ForeColor="blue" Visible="False" />
                                <asp:CheckBox ID="chk_multisubj" runat="server" OnCheckedChanged="chk_multisubj_CheckedChanged"
                                    Text="Multiple Staffs" Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium"
                                    ForeColor="blue" AutoPostBack="true" Visible="False" />
                                <asp:TextBox ID="txtmultisubj" runat="server" CssClass="Dropdown_Txt_Box" ReadOnly="true"
                                    Width="100px" Style="top: 1342px; left: 765px; position: absolute; font-family: 'Book Antiqua';"
                                    Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium"></asp:TextBox>
                                <asp:Panel ID="pnlmultisubj" runat="server" Height="400px" Width="300px" CssClass="MultipleSelectionDDL">
                                    <asp:CheckBoxList ID="chklistmultisubj" runat="server" CssClass="font" Font-Bold="True"
                                        OnSelectedIndexChanged="chklistmultisubj_selectedindetxchange" AutoPostBack="true"
                                        Font-Names="Book Antiqua" Font-Size="Medium">
                                    </asp:CheckBoxList>
                                </asp:Panel>
                                <asp:PopupControlExtender ID="PopupControlExtender1" runat="server" TargetControlID="txtmultisubj"
                                    PopupControlID="pnlmultisubj" Position="Bottom">
                                </asp:PopupControlExtender>
                                <asp:Button ID="btnOk" runat="server" Text="Ok" Font-Bold="True" Font-Names="Book Antiqua"
                                    Font-Size="Medium" Height="25px" Width="75px" BackColor="ControlLight" OnClick="btnOk_Click"
                                    Visible="False" />
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2" align="center">
                                <asp:Label ID="lblErrMsg" ForeColor="Red" runat="server" Font-Bold="true" Font-Names="Book Antiqua"
                                    Font-Size="Medium"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td class="style1">
                            </td>
                            <td>
                            </td>
                        </tr>
                        <tr>
                            <td class="style1">
                            </td>
                            <td>
                            </td>
                        </tr>
                    </table>
                </asp:Panel>
                <asp:Panel ID="panelstaff" runat="server" BorderColor="Black" BackColor="AliceBlue"
                    Visible="false" BorderWidth="2px" Style="left: 150px; top: 90px; position: absolute;"
                    Height="480px" Width="655px">
                    <div class="PopupHeaderrstud2" id="Div1" style="text-align: center; font-family: MS Sans Serif;
                        font-size: Small; font-weight: bold">
                        <br />
                        <caption style="top: 30px; border-style: solid; border-color: Black; position: absolute;
                            left: 200px">
                            Select Staff Incharge
                        </caption>
                        <br />
                        <br />
                        <table>
                            <tr>
                                <td>
                                    <asp:Label ID="lblcollege" runat="server" Text="College"></asp:Label>
                                </td>
                                <td>
                                    <%--sankar add--%>
                                    <asp:DropDownList ID="ddlcollege" runat="server" Width="150px" AutoPostBack="true"
                                        OnSelectedIndexChanged="ddlcollege_SelectedIndexChanged">
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="lblDepartment" runat="server" Text="Department"></asp:Label>
                                </td>
                                <td>
                                    <asp:DropDownList ID="ddldepratstaff" runat="server" Width="150px" AutoPostBack="true"
                                        OnSelectedIndexChanged="ddldepratstaff_SelectedIndexChanged">
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="lblsearchby" runat="server" Text="Staff By"></asp:Label>
                                </td>
                                <td>
                                    <asp:DropDownList ID="ddlstaff" runat="server" Width="150px" OnSelectedIndexChanged="ddlstaff_SelectedIndexChanged"
                                        AutoPostBack="true">
                                        <asp:ListItem Value="0">Staff Name</asp:ListItem>
                                        <asp:ListItem Value="1">Staff Code</asp:ListItem>
                                    </asp:DropDownList>
                                </td>
                                <td>
                                    <asp:TextBox ID="txt_search" runat="server" OnTextChanged="txt_search_TextChanged"
                                        AutoPostBack="True"></asp:TextBox>
                                </td>
                            </tr>
                        </table>
                        <div>
                            <FarPoint:FpSpread ID="fsstaff" runat="server" ActiveSheetViewIndex="0" Height="300"
                                Width="680" VerticalScrollBarPolicy="AsNeeded" BorderWidth="0.5" Visible="False"
                                ShowHeaderSelection="false">
                                <CommandBar BackColor="Control" ButtonType="PushButton">
                                    <Background BackgroundImageUrl="SPREADCLIENTPATH:/img/cbbg.gif" />
                                </CommandBar>
                                <Sheets>
                                    <FarPoint:SheetView SheetName="Sheet1">
                                    </FarPoint:SheetView>
                                </Sheets>
                            </FarPoint:FpSpread>
                            <fieldset style="width: 160px; height: 10px; position: absolute; top: 429px; left: 468px;">
                                <asp:Button runat="server" ID="btnstaffadd" Text="Ok" OnClick="btnstaffadd_Click"
                                    Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium" Width="75px" />
                                <asp:Button runat="server" ID="btnexitpop" Text="Exit" OnClick="exitpop_Click" Width="75px"
                                    Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium" />
                            </fieldset>
                        </div>
                    </div>
                </asp:Panel>
                <br />
                <table>
                    <tr>
                        <td style="width: 100px;">
                            <asp:Label ID="lblrptname" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                Font-Size="Medium" Text="Report Name"></asp:Label>
                        </td>
                        <td style="width: 180px;">
                            <asp:TextBox ID="txtexcelname" runat="server" Height="20px" Width="180px" Style="font-family: 'Book Antiqua'"
                                Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium"></asp:TextBox>
                            <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender2" runat="server" TargetControlID="txtexcelname"
                                FilterType="LowercaseLetters,UppercaseLetters,Numbers,Custom" ValidChars="!@$%^&*()_+|\}{][':;?><,./">
                            </asp:FilteredTextBoxExtender>
                        </td>
                        <td style="width: 115px;">
                            <asp:Button ID="btnexcel" runat="server" OnClick="btnexcel_Click" Text="Export Excel"
                                Font-Names="Book Antiqua" Font-Size="Medium" Font-Bold="true" />
                            <Insproplus:printmaster runat="server" ID="Printcontrol" Visible="false" />
                        </td>
                        <%--<td style="width:62px;">
                            <asp:Button ID="btnprint" runat="server" Text="Print" OnClick="btnprintmaster_Click" Font-Names="Book Antiqua" Font-Size="Medium"
                                Font-Bold="true" />
                            
                            </td>--%>
                        <td style="width: 60px;">
                            <asp:Button ID="btnPDF" runat="server" Text="Print" Font-Names="Book Antiqua" Font-Size="Medium"
                                Font-Bold="true" OnClick="btnPDF_Click" />
                        </td>
                    </tr>
                </table>
            </center>
        </ContentTemplate>
        <Triggers>
            <asp:PostBackTrigger ControlID="btnPDF" />
        </Triggers>
        <Triggers>
            <asp:PostBackTrigger ControlID="btnexcel" />
        </Triggers>
        <Triggers>
            <asp:AsyncPostBackTrigger ControlID="Fptimetable" />
        </Triggers>
    </asp:UpdatePanel>
</asp:Content>

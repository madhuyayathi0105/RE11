<%@ Page Title="AT11-Individual Subject Wise Attendance Report" Language="C#" MasterPageFile="~/AttendanceMOD/AttendanceSubSiteMaster.master"
    AutoEventWireup="true" CodeFile="singlesubject_wise_attendanceNew.aspx.cs" Inherits="singlesubject_wise_attendance" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Assembly="FarPoint.Web.Spread" Namespace="FarPoint.Web.Spread" TagPrefix="FarPoint" %>
<%@ Register Src="~/Usercontrols/PrintMaster.ascx" TagName="PRINTPDF" TagPrefix="Insproplus" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <%-- <style type="text/css">
        .style1
        {
            width: 109px;
        }
        .style2
        {
            width: 189px;
        }
        .style3
        {
            width: 419px;
        }
        .style4
        {
            width: 100px;
        }
        .style7
        {
            width: 84px;
        }
        .style9
        {
            width: 83px;
        }
        .style10
        {
            width: 301px;
        }
        .style11
        {
            width: 314px;
        }
        .txt
        {
        }
        .style12
        {
            width: 77px;
        }
        .style13
        {
            width: 326px;
        }
        .style14
        {
            width: 299px;
        }
        .style15
        {
            width: 297px;
        }
        .style16
        {
            width: 309px;
        }
    </style>--%>
    <script type="text/javascript">
        function display() {

            document.getElementById('MainContent_errlbl').innerHTML = "";

        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <center>
        <span class="fontstyleheader" style="color: Green; margin: 0px; margin-bottom: 10px;
            margin-top: 10px;">AT11-Individual Subject Wise Attendance Report</span>
    </center>
    <center>
        <div class="maintablestyle" style="width: 900px; margin: 0px; margin-bottom: 10px;
            margin-top: 10px; text-align: left;">
            <table cellpadding="0px" cellspacing="0px" style="height: 100%; width: auto; margin: 0px;
                margin-bottom: 10px; margin-top: 10px;">
                <tr>
                    <td class="style1">
                        <asp:Label runat="server" ID="lblbatch" Text="Batch" Font-Bold="True" Font-Names="Book Antiqua"
                            Font-Size="Medium"></asp:Label>
                        <asp:DropDownList ID="ddlbatch" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                            Font-Size="Medium" OnSelectedIndexChanged="ddlbatch_SelectedIndexChanged" Height="25px"
                            Width="60px" AutoPostBack="True">
                        </asp:DropDownList>
                    </td>
                    <td class="style2">
                        <asp:Label runat="server" ID="lbldegree" Text="Degree" Font-Bold="True" Font-Names="Book Antiqua"
                            Font-Size="Medium"></asp:Label>
                        <asp:DropDownList runat="server" ID="ddldegree" Height="24px" Width="88px" AutoPostBack="True"
                            Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium" OnSelectedIndexChanged="ddldegree_SelectedIndexChanged">
                        </asp:DropDownList>
                    </td>
                    <td class="style3">
                        <asp:Label runat="server" ID="lblbranch" Text="Branch" Font-Bold="True" Font-Names="Book Antiqua"
                            Font-Size="Medium"></asp:Label>
                        <asp:DropDownList runat="server" ID="ddlbranch" Font-Bold="True" Height="25px" Width="328px"
                            Font-Names="Book Antiqua" Font-Size="Medium" OnSelectedIndexChanged="ddlbranch_SelectedIndexChanged"
                            AutoPostBack="True">
                        </asp:DropDownList>
                    </td>
                    <td class="style4">
                        <asp:Label runat="server" ID="lblduration" Text="Sem" Font-Bold="True" Font-Names="Book Antiqua"
                            Font-Size="Medium"></asp:Label>
                        <asp:DropDownList runat="server" ID="ddlduration" Font-Bold="True" Font-Names="Book Antiqua"
                            Font-Size="Medium" AutoPostBack="True" Height="25px" Width="47px" OnSelectedIndexChanged="ddlduration_SelectedIndexChanged">
                        </asp:DropDownList>
                    </td>
                    <td class="style42">
                        <asp:Label runat="server" ID="lblsec" Text="Sec" Font-Bold="True" Font-Names="Book Antiqua"
                            Font-Size="Medium"></asp:Label>
                        <asp:DropDownList runat="server" ID="ddlsec" Font-Bold="True" Font-Names="Book Antiqua"
                            Font-Size="Medium" Height="25px" Width="49px" AutoPostBack="True" OnSelectedIndexChanged="ddlsec_SelectedIndexChanged">
                        </asp:DropDownList>
                    </td>
                </tr>
            </table>
            <table>
                <tr>
                    <td>
                        <asp:Label ID="lblsubject" runat="server" Text="Subject" Font-Bold="True" Font-Names="Book Antiqua"
                            Font-Size="Medium"></asp:Label>
                        <asp:DropDownList ID="ddlsubject" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                            Font-Size="Medium" Width="264px" OnSelectedIndexChanged="ddlsubject_SelectedIndexChanged"
                            AutoPostBack="True">
                        </asp:DropDownList>
                    </td>
                    <td class="style12">
                        <asp:Label ID="lblFromdate" runat="server" Text="From Date" Font-Bold="True" Font-Names="Book Antiqua"
                            Font-Size="Medium"></asp:Label>
                    </td>
                    <td class="style9">
                        <asp:TextBox ID="txtFromDate" CssClass="txt" runat="server" Height="20px" Width="75px"
                            Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium" OnTextChanged="txtFromDate_TextChanged"
                            AutoPostBack="True"></asp:TextBox>
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
                    <td class="style7">
                        <asp:TextBox ID="txtToDate" CssClass="txt" runat="server" Height="20px" Width="75px"
                            Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium" OnTextChanged="txtToDate_TextChanged"
                            AutoPostBack="True"></asp:TextBox>
                        <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" FilterType="Custom,Numbers"
                            ValidChars="/" runat="server" TargetControlID="txtToDate">
                        </asp:FilteredTextBoxExtender>
                        <asp:CalendarExtender ID="CalendarExtender1" TargetControlID="txtToDate" Format="d/MM/yyyy"
                            runat="server">
                        </asp:CalendarExtender>
                    </td>
                </tr>
            </table>
            <table>
                <tr>
                    <td>
                        <asp:Label ID="lbltest" runat="server" Text="Test Name" Font-Bold="True" Font-Names="Book Antiqua"
                            Font-Size="Medium">
                        </asp:Label>
                        <asp:DropDownList ID="ddltest" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                            Font-Size="Medium" Width="150px">
                        </asp:DropDownList>
                    </td>
                    <td>
                        <asp:CheckBox ID="chkonduty" runat="server" Text="Include On Duty Periods" Font-Bold="True"
                            Font-Names="Book Antiqua" Font-Size="Medium" />
                    </td>
                    <td>
                        <asp:CheckBox ID="chkondutyspit" runat="server" Text="On Duty Periods Details" Font-Bold="True"
                            OnCheckedChanged="chkondutyspit_CheckedChanged" Font-Names="Book Antiqua" Font-Size="Medium"
                            AutoPostBack="true" Width="200px" />
                    </td>
                    <td>
                        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                            <ContentTemplate>
                                <asp:TextBox ID="txtonduty" runat="server" CssClass="Dropdown_Txt_Box" ReadOnly="true"
                                    Width="110px" Style="font-family: 'Book Antiqua';" Font-Bold="True" Font-Names="Book Antiqua"
                                    Font-Size="Medium">---Select---</asp:TextBox>
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
                    <td>
                        <asp:Button ID="btnGo" runat="server" Text="Go" OnClick="btnGo_Click" Font-Bold="True"
                            Font-Names="Book Antiqua" Font-Size="Medium" Style="width: auto; height: auto;"
                            CssClass="textbox textbox1" />
                    </td>
                    <td>
                    </td>
                </tr>
            </table>
        </div>
        <table>
            <tr>
                <td>
                    <asp:Label ID="frmlbl" runat="server" Text="Select From Date" ForeColor="Red" Font-Bold="True"
                        Font-Names="Book Antiqua"></asp:Label>
                    <asp:Label ID="tolbl" runat="server" Text="Select To Date" ForeColor="Red" Font-Bold="True"
                        Font-Names="Book Antiqua"></asp:Label>
                    <asp:Label ID="tofromlbl" runat="server" Text="From date should not be greater than To date"
                        ForeColor="Red" Font-Bold="True" Font-Names="Book Antiqua"></asp:Label>
                    <asp:Label ID="norecordlbl" runat="server" ForeColor="Red"></asp:Label>
                </td>
            </tr>
        </table>
    </center>
    <center>
        <asp:Label ID="errlbl" runat="server" ForeColor="Red" Font-Bold="True" Font-Names="Book Antiqua"
            Font-Size="Medium" Height="20px"></asp:Label>
        <asp:Label ID="errmsg" runat="server" ForeColor="Red" Font-Bold="True" Font-Names="Book Antiqua"
            Font-Size="Medium" Height="16px"></asp:Label>
    </center>
    <center>
        <table>
            <tr>
                <td colspan="3" align="center">
                    <div>
                        <FarPoint:FpSpread ID="subject_spread" runat="server" BorderColor="Black" BorderStyle="Solid"
                            BorderWidth="0.5" ShowHeaderSelection="false">
                            <CommandBar BackColor="Control" ButtonFaceColor="Control" ButtonHighlightColor="ControlLightLight"
                                ButtonType="PushButton" ButtonShadowColor="ControlDark">
                            </CommandBar>
                            <Sheets>
                                <FarPoint:SheetView DesignString="&lt;?xml version=&quot;1.0&quot; encoding=&quot;utf-8&quot;?&gt;&lt;Sheet&gt;&lt;Data&gt;&lt;RowHeader class=&quot;FarPoint.Web.Spread.Model.DefaultSheetDataModel&quot; rows=&quot;3&quot; columns=&quot;1&quot;&gt;&lt;AutoCalculation&gt;True&lt;/AutoCalculation&gt;&lt;AutoGenerateColumns&gt;True&lt;/AutoGenerateColumns&gt;&lt;ReferenceStyle&gt;A1&lt;/ReferenceStyle&gt;&lt;Iteration&gt;False&lt;/Iteration&gt;&lt;MaximumIterations&gt;1&lt;/MaximumIterations&gt;&lt;MaximumChange&gt;0.001&lt;/MaximumChange&gt;&lt;/RowHeader&gt;&lt;ColumnHeader class=&quot;FarPoint.Web.Spread.Model.DefaultSheetDataModel&quot; rows=&quot;1&quot; columns=&quot;4&quot;&gt;&lt;AutoCalculation&gt;True&lt;/AutoCalculation&gt;&lt;AutoGenerateColumns&gt;True&lt;/AutoGenerateColumns&gt;&lt;ReferenceStyle&gt;A1&lt;/ReferenceStyle&gt;&lt;Iteration&gt;False&lt;/Iteration&gt;&lt;MaximumIterations&gt;1&lt;/MaximumIterations&gt;&lt;MaximumChange&gt;0.001&lt;/MaximumChange&gt;&lt;/ColumnHeader&gt;&lt;DataArea class=&quot;FarPoint.Web.Spread.Model.DefaultSheetDataModel&quot; rows=&quot;3&quot; columns=&quot;4&quot;&gt;&lt;AutoCalculation&gt;True&lt;/AutoCalculation&gt;&lt;AutoGenerateColumns&gt;True&lt;/AutoGenerateColumns&gt;&lt;ReferenceStyle&gt;A1&lt;/ReferenceStyle&gt;&lt;Iteration&gt;False&lt;/Iteration&gt;&lt;MaximumIterations&gt;1&lt;/MaximumIterations&gt;&lt;MaximumChange&gt;0.001&lt;/MaximumChange&gt;&lt;SheetName&gt;Sheet1&lt;/SheetName&gt;&lt;/DataArea&gt;&lt;SheetCorner class=&quot;FarPoint.Web.Spread.Model.DefaultSheetDataModel&quot; rows=&quot;1&quot; columns=&quot;1&quot;&gt;&lt;AutoCalculation&gt;True&lt;/AutoCalculation&gt;&lt;AutoGenerateColumns&gt;True&lt;/AutoGenerateColumns&gt;&lt;ReferenceStyle&gt;A1&lt;/ReferenceStyle&gt;&lt;Iteration&gt;False&lt;/Iteration&gt;&lt;MaximumIterations&gt;1&lt;/MaximumIterations&gt;&lt;MaximumChange&gt;0.001&lt;/MaximumChange&gt;&lt;/SheetCorner&gt;&lt;ColumnFooter class=&quot;FarPoint.Web.Spread.Model.DefaultSheetDataModel&quot; rows=&quot;1&quot; columns=&quot;4&quot;&gt;&lt;AutoCalculation&gt;True&lt;/AutoCalculation&gt;&lt;AutoGenerateColumns&gt;True&lt;/AutoGenerateColumns&gt;&lt;ReferenceStyle&gt;A1&lt;/ReferenceStyle&gt;&lt;Iteration&gt;False&lt;/Iteration&gt;&lt;MaximumIterations&gt;1&lt;/MaximumIterations&gt;&lt;MaximumChange&gt;0.001&lt;/MaximumChange&gt;&lt;/ColumnFooter&gt;&lt;/Data&gt;&lt;Presentation&gt;&lt;ActiveSkin class=&quot;FarPoint.Web.Spread.SheetSkin&quot;&gt;&lt;Name&gt;Default&lt;/Name&gt;&lt;BackColor&gt;Empty&lt;/BackColor&gt;&lt;CellBackColor&gt;Empty&lt;/CellBackColor&gt;&lt;CellForeColor&gt;Empty&lt;/CellForeColor&gt;&lt;CellSpacing&gt;0&lt;/CellSpacing&gt;&lt;GridLines&gt;Both&lt;/GridLines&gt;&lt;GridLineColor&gt;#d0d7e5&lt;/GridLineColor&gt;&lt;HeaderBackColor&gt;Empty&lt;/HeaderBackColor&gt;&lt;HeaderForeColor&gt;Empty&lt;/HeaderForeColor&gt;&lt;FlatColumnHeader&gt;False&lt;/FlatColumnHeader&gt;&lt;FooterBackColor&gt;Empty&lt;/FooterBackColor&gt;&lt;FooterForeColor&gt;Empty&lt;/FooterForeColor&gt;&lt;FlatColumnFooter&gt;False&lt;/FlatColumnFooter&gt;&lt;FlatRowHeader&gt;False&lt;/FlatRowHeader&gt;&lt;HeaderFontBold&gt;False&lt;/HeaderFontBold&gt;&lt;FooterFontBold&gt;False&lt;/FooterFontBold&gt;&lt;SelectionBackColor&gt;#eaecf5&lt;/SelectionBackColor&gt;&lt;SelectionForeColor&gt;Empty&lt;/SelectionForeColor&gt;&lt;EvenRowBackColor&gt;Empty&lt;/EvenRowBackColor&gt;&lt;OddRowBackColor&gt;Empty&lt;/OddRowBackColor&gt;&lt;ShowColumnHeader&gt;True&lt;/ShowColumnHeader&gt;&lt;ShowColumnFooter&gt;False&lt;/ShowColumnFooter&gt;&lt;ShowRowHeader&gt;True&lt;/ShowRowHeader&gt;&lt;ColumnHeaderBackground class=&quot;FarPoint.Web.Spread.Background&quot;&gt;&lt;BackgroundImageUrl&gt;SPREADCLIENTPATH:/img/chbg.gif&lt;/BackgroundImageUrl&gt;&lt;SelectedBackgroundImageUrl&gt;SPREADCLIENTPATH:/img/chm.png&lt;/SelectedBackgroundImageUrl&gt;&lt;/ColumnHeaderBackground&gt;&lt;SheetCornerBackground class=&quot;FarPoint.Web.Spread.Background&quot;&gt;&lt;BackgroundImageUrl&gt;SPREADCLIENTPATH:/img/chbg.gif&lt;/BackgroundImageUrl&gt;&lt;SelectedBackgroundImageUrl&gt;SPREADCLIENTPATH:/img/chm.png&lt;/SelectedBackgroundImageUrl&gt;&lt;/SheetCornerBackground&gt;&lt;HeaderGrayAreaColor&gt;#7999c2&lt;/HeaderGrayAreaColor&gt;&lt;/ActiveSkin&gt;&lt;AxisModels&gt;&lt;Column class=&quot;FarPoint.Web.Spread.Model.DefaultSheetAxisModel&quot; orientation=&quot;Horizontal&quot; count=&quot;4&quot;&gt;&lt;Items&gt;&lt;Item index=&quot;-1&quot;&gt;&lt;SortIndicator&gt;Ascending&lt;/SortIndicator&gt;&lt;/Item&gt;&lt;Item index=&quot;0&quot;&gt;&lt;Size&gt;55&lt;/Size&gt;&lt;/Item&gt;&lt;Item index=&quot;1&quot;&gt;&lt;Size&gt;111&lt;/Size&gt;&lt;/Item&gt;&lt;Item index=&quot;2&quot;&gt;&lt;Size&gt;81&lt;/Size&gt;&lt;/Item&gt;&lt;Item index=&quot;3&quot;&gt;&lt;Size&gt;79&lt;/Size&gt;&lt;/Item&gt;&lt;/Items&gt;&lt;/Column&gt;&lt;RowHeaderColumn class=&quot;FarPoint.Web.Spread.Model.DefaultSheetAxisModel&quot; defaultSize=&quot;40&quot; orientation=&quot;Horizontal&quot; count=&quot;1&quot;&gt;&lt;Items&gt;&lt;Item index=&quot;-1&quot;&gt;&lt;SortIndicator&gt;Ascending&lt;/SortIndicator&gt;&lt;Size&gt;40&lt;/Size&gt;&lt;/Item&gt;&lt;/Items&gt;&lt;/RowHeaderColumn&gt;&lt;ColumnHeaderRow class=&quot;FarPoint.Web.Spread.Model.DefaultSheetAxisModel&quot; defaultSize=&quot;22&quot; orientation=&quot;Vertical&quot; count=&quot;1&quot;&gt;&lt;Items&gt;&lt;Item index=&quot;-1&quot;&gt;&lt;Size&gt;22&lt;/Size&gt;&lt;/Item&gt;&lt;/Items&gt;&lt;/ColumnHeaderRow&gt;&lt;ColumnFooterRow class=&quot;FarPoint.Web.Spread.Model.DefaultSheetAxisModel&quot; defaultSize=&quot;22&quot; orientation=&quot;Vertical&quot; count=&quot;1&quot;&gt;&lt;Items&gt;&lt;Item index=&quot;-1&quot;&gt;&lt;Size&gt;22&lt;/Size&gt;&lt;/Item&gt;&lt;/Items&gt;&lt;/ColumnFooterRow&gt;&lt;/AxisModels&gt;&lt;StyleModels&gt;&lt;RowHeader class=&quot;FarPoint.Web.Spread.Model.DefaultSheetStyleModel&quot; Rows=&quot;3&quot; Columns=&quot;1&quot;&gt;&lt;AltRowCount&gt;2&lt;/AltRowCount&gt;&lt;DefaultStyle class=&quot;FarPoint.Web.Spread.NamedStyle&quot; Parent=&quot;RowHeaderDefault&quot; /&gt;&lt;ConditionalFormatCollections /&gt;&lt;/RowHeader&gt;&lt;ColumnHeader class=&quot;FarPoint.Web.Spread.Model.DefaultSheetStyleModel&quot; Rows=&quot;1&quot; Columns=&quot;4&quot;&gt;&lt;AltRowCount&gt;2&lt;/AltRowCount&gt;&lt;DefaultStyle class=&quot;FarPoint.Web.Spread.NamedStyle&quot; Parent=&quot;ColumnHeaderDefault&quot;&gt;&lt;Background class=&quot;FarPoint.Web.Spread.Background&quot;&gt;&lt;BackgroundImageUrl&gt;SPREADCLIENTPATH:/img/chbg.gif&lt;/BackgroundImageUrl&gt;&lt;SelectedBackgroundImageUrl&gt;SPREADCLIENTPATH:/img/chm.png&lt;/SelectedBackgroundImageUrl&gt;&lt;/Background&gt;&lt;/DefaultStyle&gt;&lt;ConditionalFormatCollections /&gt;&lt;/ColumnHeader&gt;&lt;DataArea class=&quot;FarPoint.Web.Spread.Model.DefaultSheetStyleModel&quot; Rows=&quot;3&quot; Columns=&quot;4&quot;&gt;&lt;AltRowCount&gt;2&lt;/AltRowCount&gt;&lt;DefaultStyle class=&quot;FarPoint.Web.Spread.NamedStyle&quot; Parent=&quot;DataAreaDefault&quot;&gt;&lt;Font&gt;&lt;Name&gt;Book Antiqua&lt;/Name&gt;&lt;Names&gt;&lt;Name&gt;Book Antiqua&lt;/Name&gt;&lt;/Names&gt;&lt;Size&gt;Medium&lt;/Size&gt;&lt;Bold&gt;False&lt;/Bold&gt;&lt;Italic&gt;False&lt;/Italic&gt;&lt;Overline&gt;False&lt;/Overline&gt;&lt;Strikeout&gt;False&lt;/Strikeout&gt;&lt;Underline&gt;False&lt;/Underline&gt;&lt;/Font&gt;&lt;GdiCharSet&gt;254&lt;/GdiCharSet&gt;&lt;ForeColor&gt;#0033cc&lt;/ForeColor&gt;&lt;HorizontalAlign&gt;Center&lt;/HorizontalAlign&gt;&lt;/DefaultStyle&gt;&lt;ConditionalFormatCollections /&gt;&lt;/DataArea&gt;&lt;SheetCorner class=&quot;FarPoint.Web.Spread.Model.DefaultSheetStyleModel&quot; Rows=&quot;1&quot; Columns=&quot;1&quot;&gt;&lt;AltRowCount&gt;2&lt;/AltRowCount&gt;&lt;DefaultStyle class=&quot;FarPoint.Web.Spread.NamedStyle&quot; Parent=&quot;CornerDefault&quot;&gt;&lt;Border class=&quot;FarPoint.Web.Spread.Border&quot; Size=&quot;1&quot; Style=&quot;Solid&quot;&gt;&lt;Bottom Color=&quot;ControlDark&quot; /&gt;&lt;Left Size=&quot;0&quot; /&gt;&lt;Right Color=&quot;ControlDark&quot; /&gt;&lt;Top Size=&quot;0&quot; /&gt;&lt;/Border&gt;&lt;Background class=&quot;FarPoint.Web.Spread.Background&quot;&gt;&lt;BackgroundImageUrl&gt;SPREADCLIENTPATH:/img/chbg.gif&lt;/BackgroundImageUrl&gt;&lt;SelectedBackgroundImageUrl&gt;SPREADCLIENTPATH:/img/chm.png&lt;/SelectedBackgroundImageUrl&gt;&lt;/Background&gt;&lt;/DefaultStyle&gt;&lt;ConditionalFormatCollections /&gt;&lt;/SheetCorner&gt;&lt;ColumnFooter class=&quot;FarPoint.Web.Spread.Model.DefaultSheetStyleModel&quot; Rows=&quot;1&quot; Columns=&quot;4&quot;&gt;&lt;AltRowCount&gt;2&lt;/AltRowCount&gt;&lt;DefaultStyle class=&quot;FarPoint.Web.Spread.NamedStyle&quot; Parent=&quot;ColumnFooterDefault&quot; /&gt;&lt;ConditionalFormatCollections /&gt;&lt;/ColumnFooter&gt;&lt;/StyleModels&gt;&lt;MessageRowStyle class=&quot;FarPoint.Web.Spread.Appearance&quot;&gt;&lt;BackColor&gt;LightYellow&lt;/BackColor&gt;&lt;ForeColor&gt;Red&lt;/ForeColor&gt;&lt;/MessageRowStyle&gt;&lt;SheetCornerStyle class=&quot;FarPoint.Web.Spread.NamedStyle&quot; Parent=&quot;CornerDefault&quot;&gt;&lt;Border class=&quot;FarPoint.Web.Spread.Border&quot; Size=&quot;1&quot; Style=&quot;Solid&quot;&gt;&lt;Bottom Color=&quot;ControlDark&quot; /&gt;&lt;Left Size=&quot;0&quot; /&gt;&lt;Right Color=&quot;ControlDark&quot; /&gt;&lt;Top Size=&quot;0&quot; /&gt;&lt;/Border&gt;&lt;Background class=&quot;FarPoint.Web.Spread.Background&quot;&gt;&lt;BackgroundImageUrl&gt;SPREADCLIENTPATH:/img/chbg.gif&lt;/BackgroundImageUrl&gt;&lt;SelectedBackgroundImageUrl&gt;SPREADCLIENTPATH:/img/chm.png&lt;/SelectedBackgroundImageUrl&gt;&lt;/Background&gt;&lt;/SheetCornerStyle&gt;&lt;AllowLoadOnDemand&gt;false&lt;/AllowLoadOnDemand&gt;&lt;LoadRowIncrement &gt;10&lt;/LoadRowIncrement &gt;&lt;LoadInitRowCount &gt;30&lt;/LoadInitRowCount &gt;&lt;AllowVirtualScrollPaging&gt;false&lt;/AllowVirtualScrollPaging&gt;&lt;TopRow&gt;0&lt;/TopRow&gt;&lt;PreviewRowStyle class=&quot;FarPoint.Web.Spread.PreviewRowInfo&quot; /&gt;&lt;/Presentation&gt;&lt;Settings&gt;&lt;Name&gt;Sheet1&lt;/Name&gt;&lt;Categories&gt;&lt;Appearance&gt;&lt;GridLineColor&gt;#d0d7e5&lt;/GridLineColor&gt;&lt;SelectionBackColor&gt;#eaecf5&lt;/SelectionBackColor&gt;&lt;SelectionBorder class=&quot;FarPoint.Web.Spread.Border&quot; /&gt;&lt;/Appearance&gt;&lt;Behavior&gt;&lt;EditTemplateColumnCount&gt;2&lt;/EditTemplateColumnCount&gt;&lt;ScrollingContentVisible&gt;True&lt;/ScrollingContentVisible&gt;&lt;PageSize&gt;100&lt;/PageSize&gt;&lt;AllowPage&gt;False&lt;/AllowPage&gt;&lt;GroupBarText&gt;Drag a column to group by that column.&lt;/GroupBarText&gt;&lt;/Behavior&gt;&lt;Layout&gt;&lt;RowHeaderColumnCount&gt;1&lt;/RowHeaderColumnCount&gt;&lt;ColumnHeaderRowCount&gt;1&lt;/ColumnHeaderRowCount&gt;&lt;/Layout&gt;&lt;/Categories&gt;&lt;ActiveRow&gt;0&lt;/ActiveRow&gt;&lt;ActiveColumn&gt;0&lt;/ActiveColumn&gt;&lt;ColumnHeaderRowCount&gt;1&lt;/ColumnHeaderRowCount&gt;&lt;ColumnFooterRowCount&gt;1&lt;/ColumnFooterRowCount&gt;&lt;PrintInfo&gt;&lt;Header /&gt;&lt;Footer /&gt;&lt;ZoomFactor&gt;0&lt;/ZoomFactor&gt;&lt;FirstPageNumber&gt;1&lt;/FirstPageNumber&gt;&lt;Orientation&gt;Auto&lt;/Orientation&gt;&lt;PrintType&gt;All&lt;/PrintType&gt;&lt;PageOrder&gt;Auto&lt;/PageOrder&gt;&lt;BestFitCols&gt;False&lt;/BestFitCols&gt;&lt;BestFitRows&gt;False&lt;/BestFitRows&gt;&lt;PageStart&gt;-1&lt;/PageStart&gt;&lt;PageEnd&gt;-1&lt;/PageEnd&gt;&lt;ColStart&gt;-1&lt;/ColStart&gt;&lt;ColEnd&gt;-1&lt;/ColEnd&gt;&lt;RowStart&gt;-1&lt;/RowStart&gt;&lt;RowEnd&gt;-1&lt;/RowEnd&gt;&lt;ShowBorder&gt;True&lt;/ShowBorder&gt;&lt;ShowGrid&gt;True&lt;/ShowGrid&gt;&lt;ShowColor&gt;True&lt;/ShowColor&gt;&lt;ShowColumnHeader&gt;Inherit&lt;/ShowColumnHeader&gt;&lt;ShowRowHeader&gt;Inherit&lt;/ShowRowHeader&gt;&lt;ShowColumnFooter&gt;Inherit&lt;/ShowColumnFooter&gt;&lt;ShowColumnFooterEachPage&gt;True&lt;/ShowColumnFooterEachPage&gt;&lt;ShowTitle&gt;True&lt;/ShowTitle&gt;&lt;ShowSubtitle&gt;True&lt;/ShowSubtitle&gt;&lt;UseMax&gt;True&lt;/UseMax&gt;&lt;UseSmartPrint&gt;False&lt;/UseSmartPrint&gt;&lt;Opacity&gt;255&lt;/Opacity&gt;&lt;PrintNotes&gt;None&lt;/PrintNotes&gt;&lt;Centering&gt;None&lt;/Centering&gt;&lt;RepeatColStart&gt;-1&lt;/RepeatColStart&gt;&lt;RepeatColEnd&gt;-1&lt;/RepeatColEnd&gt;&lt;RepeatRowStart&gt;-1&lt;/RepeatRowStart&gt;&lt;RepeatRowEnd&gt;-1&lt;/RepeatRowEnd&gt;&lt;SmartPrintPagesTall&gt;1&lt;/SmartPrintPagesTall&gt;&lt;SmartPrintPagesWide&gt;1&lt;/SmartPrintPagesWide&gt;&lt;HeaderHeight&gt;-1&lt;/HeaderHeight&gt;&lt;FooterHeight&gt;-1&lt;/FooterHeight&gt;&lt;/PrintInfo&gt;&lt;TitleInfo class=&quot;FarPoint.Web.Spread.TitleInfo&quot;&gt;&lt;Style class=&quot;FarPoint.Web.Spread.StyleInfo&quot;&gt;&lt;BackColor&gt;#e7eff7&lt;/BackColor&gt;&lt;HorizontalAlign&gt;Right&lt;/HorizontalAlign&gt;&lt;/Style&gt;&lt;Value type=&quot;System.String&quot; whitespace=&quot;&quot; /&gt;&lt;/TitleInfo&gt;&lt;LayoutTemplate class=&quot;FarPoint.Web.Spread.LayoutTemplate&quot;&gt;&lt;Layout&gt;&lt;ColumnCount&gt;4&lt;/ColumnCount&gt;&lt;RowCount&gt;1&lt;/RowCount&gt;&lt;/Layout&gt;&lt;Data&gt;&lt;LayoutData class=&quot;FarPoint.Web.Spread.Model.DefaultSheetDataModel&quot; rows=&quot;1&quot; columns=&quot;4&quot;&gt;&lt;AutoCalculation&gt;True&lt;/AutoCalculation&gt;&lt;AutoGenerateColumns&gt;True&lt;/AutoGenerateColumns&gt;&lt;ReferenceStyle&gt;A1&lt;/ReferenceStyle&gt;&lt;Iteration&gt;False&lt;/Iteration&gt;&lt;MaximumIterations&gt;1&lt;/MaximumIterations&gt;&lt;MaximumChange&gt;0.001&lt;/MaximumChange&gt;&lt;Cells&gt;&lt;Cell row=&quot;0&quot; column=&quot;0&quot;&gt;&lt;Data type=&quot;System.Int32&quot;&gt;0&lt;/Data&gt;&lt;/Cell&gt;&lt;Cell row=&quot;0&quot; column=&quot;1&quot;&gt;&lt;Data type=&quot;System.Int32&quot;&gt;1&lt;/Data&gt;&lt;/Cell&gt;&lt;Cell row=&quot;0&quot; column=&quot;2&quot;&gt;&lt;Data type=&quot;System.Int32&quot;&gt;2&lt;/Data&gt;&lt;/Cell&gt;&lt;Cell row=&quot;0&quot; column=&quot;3&quot;&gt;&lt;Data type=&quot;System.Int32&quot;&gt;3&lt;/Data&gt;&lt;/Cell&gt;&lt;/Cells&gt;&lt;/LayoutData&gt;&lt;/Data&gt;&lt;AxisModels&gt;&lt;LayoutColumn class=&quot;FarPoint.Web.Spread.Model.DefaultSheetAxisModel&quot; orientation=&quot;Horizontal&quot; count=&quot;4&quot;&gt;&lt;Items&gt;&lt;Item index=&quot;-1&quot;&gt;&lt;SortIndicator&gt;Ascending&lt;/SortIndicator&gt;&lt;/Item&gt;&lt;/Items&gt;&lt;/LayoutColumn&gt;&lt;LayoutRow class=&quot;FarPoint.Web.Spread.Model.DefaultSheetAxisModel&quot; orientation=&quot;Vertical&quot; count=&quot;1&quot;&gt;&lt;Items&gt;&lt;Item index=&quot;-1&quot; /&gt;&lt;/Items&gt;&lt;/LayoutRow&gt;&lt;/AxisModels&gt;&lt;/LayoutTemplate&gt;&lt;LayoutMode&gt;CellLayoutMode&lt;/LayoutMode&gt;&lt;CurrentPageIndex type=&quot;System.Int32&quot;&gt;0&lt;/CurrentPageIndex&gt;&lt;/Settings&gt;&lt;/Sheet&gt;"
                                    SheetName="Sheet1" GridLineColor="Black">
                                </FarPoint:SheetView>
                            </Sheets>
                        </FarPoint:FpSpread>
                    </div>
                </td>
            </tr>
            <tr>
                <td colspan="3" align="center">
                    <asp:Label ID="lblrptname" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                        Font-Size="Medium" Text="Report Name"></asp:Label>
                    <asp:TextBox ID="txtexcelname" runat="server" Height="20px" Width="180px" Style="font-family: 'Book Antiqua'"
                        Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium" onkeypress="display()"></asp:TextBox>
                    <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender4" runat="server" TargetControlID="txtexcelname"
                        FilterType="LowercaseLetters,UppercaseLetters,Numbers,Custom" ValidChars="!@$%^&*()_+|}{][':;?><,."
                        InvalidChars="/\">
                    </asp:FilteredTextBoxExtender>
                    <asp:Button ID="btnxl" Style="width: auto; height: auto;" CssClass="textbox textbox1"
                        runat="server" Text="Export Excel" Font-Bold="True" Font-Names="Book Antiqua"
                        Font-Size="Medium" OnClick="btnxl_Click" />
                    <asp:Button ID="btnprintmaster" Style="width: auto; height: auto;" CssClass="textbox textbox1"
                        runat="server" Text="Print" OnClick="btnprintmaster_Click" Font-Names="Book Antiqua"
                        Font-Size="Medium" Font-Bold="true" />
                    <Insproplus:PRINTPDF runat="server" ID="Printcontrol" Visible="false" />
                </td>
            </tr>
        </table>
    </center>
</asp:Content>

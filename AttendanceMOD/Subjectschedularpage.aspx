<%@ Page Title="" Language="C#" MasterPageFile="~/AttendanceMOD/AttendanceSubSiteMaster.master"
    AutoEventWireup="true" CodeFile="Subjectschedularpage.aspx.cs" Inherits="Subjectschedularpage" %>

<%@ Register Src="~/Usercontrols/Input_Events.ascx" TagName="subject" TagPrefix="UC" %>
<%@ Register Src="~/Usercontrols/PrintMaster.ascx" TagName="Print" TagPrefix="UC" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Assembly="FarPoint.Web.Spread" Namespace="FarPoint.Web.Spread" TagPrefix="FarPoint" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">
    <style type="text/css">
        .font
        {
            font-family: Book Antiqua;
            font-size: medium;
            font-weight: bold;
        }
        .topHandle
        {
            background-color: #97bae6;
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
    <script type="text/javascript">

        function AutoCompleteExtender1_OnClientPopulating(sender, args) {

            var details = document.getElementById('<%=lblValue.ClientID %>');
            sender.set_contextKey(details.innerHTML);
        }

    </script>
    <center>
        <asp:Label ID="Label2" CssClass="fontstyleheader" runat="server" Text="Staff Selector"
            ForeColor="Green" Visible="true"></asp:Label>
    </center>
    <br />
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <center>
        <asp:UpdatePanel ID="UDP1" runat="server">
            <ContentTemplate>
                <asp:TabContainer ID="TabContainer1" runat="server" Height="725" Width="1000" ActiveTabIndex="0"
                    OnActiveTabChanged="TabContainer1_ActiveTabChanged" AutoPostBack="true">
                    <asp:TabPanel ID="tabpanel1" Visible="false" runat="server" HeaderText="Subject Chooser"
                        TabIndex="1">
                        <ContentTemplate>
                            <asp:Panel ID="Panel1" runat="server">
                                <div>
                                    <center>
                                        <table style="width: 700px; height: 70px; background-color: #0CA6CA;">
                                            <tr>
                                                <td>
                                                    <UC:subject ID="usercontrol" runat="server"></UC:subject>
                                                </td>
                                            </tr>
                                        </table>
                                    </center>
                                    <center>
                                        <%-- <asp:UpdatePanel ID="udp1" runat="server"><ContentTemplate>--%>
                                        <table style="width: 700px; height: 70px; background-color: #0CA6CA;">
                                            <tr>
                                                <td>
                                                    <asp:RadioButton ID="rbsubname" Text="Subject Name" AutoPostBack="true" OnCheckedChanged="rbradio_CheckedChanged"
                                                        runat="server" GroupName="Subject" Font-Bold="True" Font-Names="Book Antiqua"
                                                        Font-Size="Medium" />
                                                </td>
                                                <td>
                                                    <asp:RadioButton ID="rbsubacr" Text="Subject Acronym" AutoPostBack="true" OnCheckedChanged="rbradio_CheckedChanged"
                                                        runat="server" GroupName="Subject" Font-Bold="True" Font-Names="Book Antiqua"
                                                        Font-Size="Medium" />
                                                </td>
                                                <td>
                                                    <asp:CheckBox ID="chkeleective" runat="server" Text="Show Only Elective" Font-Bold="True"
                                                        Font-Names="Book Antiqua" Font-Size="Medium" AutoPostBack="true" OnCheckedChanged="rbradio_CheckedChanged" />
                                                    <asp:Button ID="btnGo" Text="Go" runat="server" OnClick="btnGo_Click" Font-Bold="True"
                                                        Font-Names="Book Antiqua" Font-Size="Medium" />
                                                </td>
                                            </tr>
                                            <td>
                                                <asp:Label ID="Labelerror" runat="server" Text="Label" ForeColor="Red" Font-Bold="True"
                                                    Font-Names="Book Antiqua" Font-Size="Medium"></asp:Label>
                                            </td>
                                            <tr>
                                            </tr>
                                        </table>
                                        <%--                                         </ContentTemplate></asp:UpdatePanel>--%>
                                    </center>
                                </div>
                                <div>
                                    <%-- <asp:UpdatePanel ID="upd6" runat="server"><ContentTemplate>--%>
                                    <FarPoint:FpSpread ID="FpEntry" runat="server" Style="top: 360px; position: absolute;
                                        border-color: Black; border-style: solid; font-family: icon; border-width: 1px;
                                        height: 600px; width: 985px; position: absolute;" OnUpdateCommand="FpEntry_UpdateCommand"
                                        ActiveSheetViewIndex="0" BorderColor="Black" BorderStyle="Solid" BorderWidth="1px"
                                        ClientIDMode="AutoID" DesignString="&lt;?xml version=&quot;1.0&quot; encoding=&quot;utf-8&quot;?&gt;&lt;Spread /&gt;"
                                        ShowHeaderSelection="false">
                                        <CommandBar BackColor="Control" Visible="False">
                                            <Background BackgroundImageUrl="SPREADCLIENTPATH:/img/cbbg.gif" />
                                        </CommandBar>
                                        <Sheets>
                                            <FarPoint:SheetView SheetName="Sheet1" DesignString="&lt;?xml version=&quot;1.0&quot; encoding=&quot;utf-8&quot;?&gt;&lt;Sheet&gt;&lt;Data&gt;&lt;RowHeader class=&quot;FarPoint.Web.Spread.Model.DefaultSheetDataModel&quot; rows=&quot;3&quot; columns=&quot;1&quot;&gt;&lt;AutoCalculation&gt;True&lt;/AutoCalculation&gt;&lt;AutoGenerateColumns&gt;True&lt;/AutoGenerateColumns&gt;&lt;ReferenceStyle&gt;A1&lt;/ReferenceStyle&gt;&lt;Iteration&gt;False&lt;/Iteration&gt;&lt;MaximumIterations&gt;1&lt;/MaximumIterations&gt;&lt;MaximumChange&gt;0.001&lt;/MaximumChange&gt;&lt;/RowHeader&gt;&lt;ColumnHeader class=&quot;FarPoint.Web.Spread.Model.DefaultSheetDataModel&quot; rows=&quot;1&quot; columns=&quot;4&quot;&gt;&lt;AutoCalculation&gt;True&lt;/AutoCalculation&gt;&lt;AutoGenerateColumns&gt;True&lt;/AutoGenerateColumns&gt;&lt;ReferenceStyle&gt;A1&lt;/ReferenceStyle&gt;&lt;Iteration&gt;False&lt;/Iteration&gt;&lt;MaximumIterations&gt;1&lt;/MaximumIterations&gt;&lt;MaximumChange&gt;0.001&lt;/MaximumChange&gt;&lt;/ColumnHeader&gt;&lt;DataArea class=&quot;FarPoint.Web.Spread.Model.DefaultSheetDataModel&quot; rows=&quot;3&quot; columns=&quot;4&quot;&gt;&lt;AutoCalculation&gt;True&lt;/AutoCalculation&gt;&lt;AutoGenerateColumns&gt;True&lt;/AutoGenerateColumns&gt;&lt;ReferenceStyle&gt;A1&lt;/ReferenceStyle&gt;&lt;Iteration&gt;False&lt;/Iteration&gt;&lt;MaximumIterations&gt;1&lt;/MaximumIterations&gt;&lt;MaximumChange&gt;0.001&lt;/MaximumChange&gt;&lt;SheetName&gt;Sheet1&lt;/SheetName&gt;&lt;/DataArea&gt;&lt;SheetCorner class=&quot;FarPoint.Web.Spread.Model.DefaultSheetDataModel&quot; rows=&quot;1&quot; columns=&quot;1&quot;&gt;&lt;AutoCalculation&gt;True&lt;/AutoCalculation&gt;&lt;AutoGenerateColumns&gt;True&lt;/AutoGenerateColumns&gt;&lt;ReferenceStyle&gt;A1&lt;/ReferenceStyle&gt;&lt;Iteration&gt;False&lt;/Iteration&gt;&lt;MaximumIterations&gt;1&lt;/MaximumIterations&gt;&lt;MaximumChange&gt;0.001&lt;/MaximumChange&gt;&lt;/SheetCorner&gt;&lt;ColumnFooter class=&quot;FarPoint.Web.Spread.Model.DefaultSheetDataModel&quot; rows=&quot;1&quot; columns=&quot;4&quot;&gt;&lt;AutoCalculation&gt;True&lt;/AutoCalculation&gt;&lt;AutoGenerateColumns&gt;True&lt;/AutoGenerateColumns&gt;&lt;ReferenceStyle&gt;A1&lt;/ReferenceStyle&gt;&lt;Iteration&gt;False&lt;/Iteration&gt;&lt;MaximumIterations&gt;1&lt;/MaximumIterations&gt;&lt;MaximumChange&gt;0.001&lt;/MaximumChange&gt;&lt;/ColumnFooter&gt;&lt;/Data&gt;&lt;Presentation&gt;&lt;AxisModels&gt;&lt;Column class=&quot;FarPoint.Web.Spread.Model.DefaultSheetAxisModel&quot; orientation=&quot;Horizontal&quot; count=&quot;4&quot;&gt;&lt;Items&gt;&lt;Item index=&quot;-1&quot;&gt;&lt;SortIndicator&gt;Ascending&lt;/SortIndicator&gt;&lt;/Item&gt;&lt;/Items&gt;&lt;/Column&gt;&lt;RowHeaderColumn class=&quot;FarPoint.Web.Spread.Model.DefaultSheetAxisModel&quot; defaultSize=&quot;40&quot; orientation=&quot;Horizontal&quot; count=&quot;1&quot;&gt;&lt;Items&gt;&lt;Item index=&quot;-1&quot;&gt;&lt;SortIndicator&gt;Ascending&lt;/SortIndicator&gt;&lt;Size&gt;40&lt;/Size&gt;&lt;/Item&gt;&lt;/Items&gt;&lt;/RowHeaderColumn&gt;&lt;ColumnHeaderRow class=&quot;FarPoint.Web.Spread.Model.DefaultSheetAxisModel&quot; defaultSize=&quot;22&quot; orientation=&quot;Vertical&quot; count=&quot;1&quot;&gt;&lt;Items&gt;&lt;Item index=&quot;-1&quot;&gt;&lt;Size&gt;22&lt;/Size&gt;&lt;/Item&gt;&lt;/Items&gt;&lt;/ColumnHeaderRow&gt;&lt;ColumnFooterRow class=&quot;FarPoint.Web.Spread.Model.DefaultSheetAxisModel&quot; defaultSize=&quot;22&quot; orientation=&quot;Vertical&quot; count=&quot;1&quot;&gt;&lt;Items&gt;&lt;Item index=&quot;-1&quot;&gt;&lt;Size&gt;22&lt;/Size&gt;&lt;/Item&gt;&lt;/Items&gt;&lt;/ColumnFooterRow&gt;&lt;/AxisModels&gt;&lt;StyleModels&gt;&lt;RowHeader class=&quot;FarPoint.Web.Spread.Model.DefaultSheetStyleModel&quot; Rows=&quot;3&quot; Columns=&quot;1&quot;&gt;&lt;AltRowCount&gt;2&lt;/AltRowCount&gt;&lt;DefaultStyle class=&quot;FarPoint.Web.Spread.NamedStyle&quot; Parent=&quot;RowHeaderDefault&quot; /&gt;&lt;ConditionalFormatCollections /&gt;&lt;/RowHeader&gt;&lt;ColumnHeader class=&quot;FarPoint.Web.Spread.Model.DefaultSheetStyleModel&quot; Rows=&quot;1&quot; Columns=&quot;4&quot;&gt;&lt;AltRowCount&gt;2&lt;/AltRowCount&gt;&lt;DefaultStyle class=&quot;FarPoint.Web.Spread.NamedStyle&quot; Parent=&quot;ColumnHeaderDefault&quot;&gt;&lt;Background class=&quot;FarPoint.Web.Spread.Background&quot;&gt;&lt;BackgroundImageUrl&gt;SPREADCLIENTPATH:/img/chbg.gif&lt;/BackgroundImageUrl&gt;&lt;SelectedBackgroundImageUrl&gt;SPREADCLIENTPATH:/img/chm.png&lt;/SelectedBackgroundImageUrl&gt;&lt;/Background&gt;&lt;/DefaultStyle&gt;&lt;ConditionalFormatCollections /&gt;&lt;/ColumnHeader&gt;&lt;DataArea class=&quot;FarPoint.Web.Spread.Model.DefaultSheetStyleModel&quot; Rows=&quot;3&quot; Columns=&quot;4&quot;&gt;&lt;AltRowCount&gt;2&lt;/AltRowCount&gt;&lt;DefaultStyle class=&quot;FarPoint.Web.Spread.NamedStyle&quot; Parent=&quot;DataAreaDefault&quot; /&gt;&lt;ConditionalFormatCollections /&gt;&lt;/DataArea&gt;&lt;SheetCorner class=&quot;FarPoint.Web.Spread.Model.DefaultSheetStyleModel&quot; Rows=&quot;1&quot; Columns=&quot;1&quot;&gt;&lt;AltRowCount&gt;2&lt;/AltRowCount&gt;&lt;DefaultStyle class=&quot;FarPoint.Web.Spread.NamedStyle&quot; Parent=&quot;CornerDefault&quot;&gt;&lt;Background class=&quot;FarPoint.Web.Spread.Background&quot;&gt;&lt;BackgroundImageUrl&gt;SPREADCLIENTPATH:/img/chbg.gif&lt;/BackgroundImageUrl&gt;&lt;SelectedBackgroundImageUrl&gt;SPREADCLIENTPATH:/img/chm.png&lt;/SelectedBackgroundImageUrl&gt;&lt;/Background&gt;&lt;/DefaultStyle&gt;&lt;ConditionalFormatCollections /&gt;&lt;/SheetCorner&gt;&lt;ColumnFooter class=&quot;FarPoint.Web.Spread.Model.DefaultSheetStyleModel&quot; Rows=&quot;1&quot; Columns=&quot;4&quot;&gt;&lt;AltRowCount&gt;2&lt;/AltRowCount&gt;&lt;DefaultStyle class=&quot;FarPoint.Web.Spread.NamedStyle&quot; Parent=&quot;ColumnFooterDefault&quot; /&gt;&lt;ConditionalFormatCollections /&gt;&lt;/ColumnFooter&gt;&lt;/StyleModels&gt;&lt;MessageRowStyle class=&quot;FarPoint.Web.Spread.Appearance&quot;&gt;&lt;BackColor&gt;LightYellow&lt;/BackColor&gt;&lt;ForeColor&gt;Red&lt;/ForeColor&gt;&lt;/MessageRowStyle&gt;&lt;SheetCornerStyle class=&quot;FarPoint.Web.Spread.NamedStyle&quot; Parent=&quot;CornerDefault&quot;&gt;&lt;Background class=&quot;FarPoint.Web.Spread.Background&quot;&gt;&lt;BackgroundImageUrl&gt;SPREADCLIENTPATH:/img/chbg.gif&lt;/BackgroundImageUrl&gt;&lt;SelectedBackgroundImageUrl&gt;SPREADCLIENTPATH:/img/chm.png&lt;/SelectedBackgroundImageUrl&gt;&lt;/Background&gt;&lt;/SheetCornerStyle&gt;&lt;AllowLoadOnDemand&gt;false&lt;/AllowLoadOnDemand&gt;&lt;LoadRowIncrement &gt;10&lt;/LoadRowIncrement &gt;&lt;LoadInitRowCount &gt;30&lt;/LoadInitRowCount &gt;&lt;AllowVirtualScrollPaging&gt;false&lt;/AllowVirtualScrollPaging&gt;&lt;TopRow&gt;0&lt;/TopRow&gt;&lt;PreviewRowStyle class=&quot;FarPoint.Web.Spread.PreviewRowInfo&quot; /&gt;&lt;/Presentation&gt;&lt;Settings&gt;&lt;Name&gt;Sheet1&lt;/Name&gt;&lt;Categories&gt;&lt;Appearance&gt;&lt;GridLineColor&gt;#d0d7e5&lt;/GridLineColor&gt;&lt;SelectionBackColor&gt;#eaecf5&lt;/SelectionBackColor&gt;&lt;SelectionBorder class=&quot;FarPoint.Web.Spread.Border&quot; /&gt;&lt;/Appearance&gt;&lt;Behavior&gt;&lt;EditTemplateColumnCount&gt;2&lt;/EditTemplateColumnCount&gt;&lt;GroupBarText&gt;Drag a column to group by that column.&lt;/GroupBarText&gt;&lt;/Behavior&gt;&lt;Layout&gt;&lt;RowHeaderColumnCount&gt;1&lt;/RowHeaderColumnCount&gt;&lt;ColumnHeaderRowCount&gt;1&lt;/ColumnHeaderRowCount&gt;&lt;/Layout&gt;&lt;/Categories&gt;&lt;ColumnHeaderRowCount&gt;1&lt;/ColumnHeaderRowCount&gt;&lt;ColumnFooterRowCount&gt;1&lt;/ColumnFooterRowCount&gt;&lt;PrintInfo&gt;&lt;Header /&gt;&lt;Footer /&gt;&lt;ZoomFactor&gt;0&lt;/ZoomFactor&gt;&lt;FirstPageNumber&gt;1&lt;/FirstPageNumber&gt;&lt;Orientation&gt;Auto&lt;/Orientation&gt;&lt;PrintType&gt;All&lt;/PrintType&gt;&lt;PageOrder&gt;Auto&lt;/PageOrder&gt;&lt;BestFitCols&gt;False&lt;/BestFitCols&gt;&lt;BestFitRows&gt;False&lt;/BestFitRows&gt;&lt;PageStart&gt;-1&lt;/PageStart&gt;&lt;PageEnd&gt;-1&lt;/PageEnd&gt;&lt;ColStart&gt;-1&lt;/ColStart&gt;&lt;ColEnd&gt;-1&lt;/ColEnd&gt;&lt;RowStart&gt;-1&lt;/RowStart&gt;&lt;RowEnd&gt;-1&lt;/RowEnd&gt;&lt;ShowBorder&gt;True&lt;/ShowBorder&gt;&lt;ShowGrid&gt;True&lt;/ShowGrid&gt;&lt;ShowColor&gt;True&lt;/ShowColor&gt;&lt;ShowColumnHeader&gt;Inherit&lt;/ShowColumnHeader&gt;&lt;ShowRowHeader&gt;Inherit&lt;/ShowRowHeader&gt;&lt;ShowColumnFooter&gt;Inherit&lt;/ShowColumnFooter&gt;&lt;ShowColumnFooterEachPage&gt;True&lt;/ShowColumnFooterEachPage&gt;&lt;ShowTitle&gt;True&lt;/ShowTitle&gt;&lt;ShowSubtitle&gt;True&lt;/ShowSubtitle&gt;&lt;UseMax&gt;True&lt;/UseMax&gt;&lt;UseSmartPrint&gt;False&lt;/UseSmartPrint&gt;&lt;Opacity&gt;255&lt;/Opacity&gt;&lt;PrintNotes&gt;None&lt;/PrintNotes&gt;&lt;Centering&gt;None&lt;/Centering&gt;&lt;RepeatColStart&gt;-1&lt;/RepeatColStart&gt;&lt;RepeatColEnd&gt;-1&lt;/RepeatColEnd&gt;&lt;RepeatRowStart&gt;-1&lt;/RepeatRowStart&gt;&lt;RepeatRowEnd&gt;-1&lt;/RepeatRowEnd&gt;&lt;SmartPrintPagesTall&gt;1&lt;/SmartPrintPagesTall&gt;&lt;SmartPrintPagesWide&gt;1&lt;/SmartPrintPagesWide&gt;&lt;HeaderHeight&gt;-1&lt;/HeaderHeight&gt;&lt;FooterHeight&gt;-1&lt;/FooterHeight&gt;&lt;/PrintInfo&gt;&lt;TitleInfo class=&quot;FarPoint.Web.Spread.TitleInfo&quot;&gt;&lt;Style class=&quot;FarPoint.Web.Spread.StyleInfo&quot;&gt;&lt;BackColor&gt;#e7eff7&lt;/BackColor&gt;&lt;HorizontalAlign&gt;Right&lt;/HorizontalAlign&gt;&lt;/Style&gt;&lt;/TitleInfo&gt;&lt;LayoutTemplate class=&quot;FarPoint.Web.Spread.LayoutTemplate&quot;&gt;&lt;Layout&gt;&lt;ColumnCount&gt;4&lt;/ColumnCount&gt;&lt;RowCount&gt;1&lt;/RowCount&gt;&lt;/Layout&gt;&lt;Data&gt;&lt;LayoutData class=&quot;FarPoint.Web.Spread.Model.DefaultSheetDataModel&quot; rows=&quot;1&quot; columns=&quot;4&quot;&gt;&lt;AutoCalculation&gt;True&lt;/AutoCalculation&gt;&lt;AutoGenerateColumns&gt;True&lt;/AutoGenerateColumns&gt;&lt;ReferenceStyle&gt;A1&lt;/ReferenceStyle&gt;&lt;Iteration&gt;False&lt;/Iteration&gt;&lt;MaximumIterations&gt;1&lt;/MaximumIterations&gt;&lt;MaximumChange&gt;0.001&lt;/MaximumChange&gt;&lt;Cells&gt;&lt;Cell row=&quot;0&quot; column=&quot;0&quot;&gt;&lt;Data type=&quot;System.Int32&quot;&gt;0&lt;/Data&gt;&lt;/Cell&gt;&lt;Cell row=&quot;0&quot; column=&quot;1&quot;&gt;&lt;Data type=&quot;System.Int32&quot;&gt;1&lt;/Data&gt;&lt;/Cell&gt;&lt;Cell row=&quot;0&quot; column=&quot;2&quot;&gt;&lt;Data type=&quot;System.Int32&quot;&gt;2&lt;/Data&gt;&lt;/Cell&gt;&lt;Cell row=&quot;0&quot; column=&quot;3&quot;&gt;&lt;Data type=&quot;System.Int32&quot;&gt;3&lt;/Data&gt;&lt;/Cell&gt;&lt;/Cells&gt;&lt;/LayoutData&gt;&lt;/Data&gt;&lt;AxisModels&gt;&lt;LayoutColumn class=&quot;FarPoint.Web.Spread.Model.DefaultSheetAxisModel&quot; orientation=&quot;Horizontal&quot; count=&quot;4&quot;&gt;&lt;Items&gt;&lt;Item index=&quot;-1&quot;&gt;&lt;SortIndicator&gt;Ascending&lt;/SortIndicator&gt;&lt;/Item&gt;&lt;/Items&gt;&lt;/LayoutColumn&gt;&lt;LayoutRow class=&quot;FarPoint.Web.Spread.Model.DefaultSheetAxisModel&quot; orientation=&quot;Vertical&quot; count=&quot;1&quot;&gt;&lt;Items&gt;&lt;Item index=&quot;-1&quot; /&gt;&lt;/Items&gt;&lt;/LayoutRow&gt;&lt;/AxisModels&gt;&lt;/LayoutTemplate&gt;&lt;LayoutMode&gt;CellLayoutMode&lt;/LayoutMode&gt;&lt;CurrentPageIndex type=&quot;System.Int32&quot;&gt;0&lt;/CurrentPageIndex&gt;&lt;/Settings&gt;&lt;/Sheet&gt;"
                                                EditTemplateColumnCount="2" FrozenColumnCount="3" GridLineColor="#D0D7E5" GroupBarText="Drag a column to group by that column."
                                                SelectionBackColor="#EAECF5">
                                            </FarPoint:SheetView>
                                        </Sheets>
                                        <TitleInfo BackColor="#E7EFF7" Font-Size="X-Large" ForeColor="" HorizontalAlign="Center"
                                            VerticalAlign="NotSet">
                                        </TitleInfo>
                                    </FarPoint:FpSpread>
                                    <%--  </ContentTemplate></asp:UpdatePanel>--%>
                                </div>
                                <div>
                                    <center>
                                        <asp:Button ID="Button1" runat="server" Style="position: absolute; top: 965px; left: 420px;"
                                            Text="Save" OnClick="Savebtn_Click" Font-Bold="true" Font-Size="Medium" Font-Names="Book Antiqua" />
                                        <asp:Button ID="btnprint" runat="server" Style="position: absolute; top: 965px; margin-left: -73px;"
                                            Text="Print" OnClick="btnprint_Click" Font-Bold="true" Font-Size="Medium" Font-Names="Book Antiqua" />
                                    </center>
                                    <UC:Print ID="printcontrol" runat="server" />
                                </div>
                            </asp:Panel>
                        </ContentTemplate>
                    </asp:TabPanel>
                    <asp:TabPanel ID="TabPanel2" runat="server" HeaderText="Staff Selector" TabIndex="2"
                        Width="970px" Height="700px">
                        <ContentTemplate>
                            <div>
                                <table>
                                    <tr>
                                        <td style="height: 33px; width: 1015px;">
                                            <UC:subject ID="usercontrol1" runat="server"></UC:subject>
                                        </td>
                                        <td>
                                            <asp:UpdatePanel runat="server" ID="UpdatePanel3">
                                                <ContentTemplate>
                                                    <asp:Button ID="Button2" Text="Go" runat="server" OnClick="btnGo1_Click" Font-Bold="True"
                                                        Font-Names="Book Antiqua" Font-Size="Medium" Visible="true" Style="font-family: Book Antiqua;
                                                        font-size: medium; font-weight: bold; height: auto; width: auto;" />
                                                </ContentTemplate>
                                            </asp:UpdatePanel>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="2">
                                            <asp:Label ID="lblerror" runat="server" Text="Label" ForeColor="Red"></asp:Label>
                                        </td>
                                    </tr>
                                </table>
                            </div>
                            <div>
                                <table>
                                    <tr>
                                        <td style="height: 400px; margin: 0px;">
                                            <asp:TreeView runat="server" ID="subjtree" SelectedNodeStyle-ForeColor="Red" HoverNodeStyle-BackColor="LightBlue"
                                                AutoPostBack="true" OnSelectedNodeChanged="subjtree_SelectedNodeChanged" Font-Names="Book Antiqua"
                                                Font-Size="Small" ForeColor="Black" Style="color: Black; background-color: White;
                                                font-family: Book Antiqua; font-size: small; height: 358px; border-style: solid;
                                                border-width: 1px; overflow: scroll; width: 300px;">
                                            </asp:TreeView>
                                        </td>
                                        <td style="width: 700px; height: 400px; margin: 0px;">
                                            <table>
                                                <tr>
                                                    <td align="left">
                                                        <asp:Button ID="FindBtn" runat="server" Text="Select Staff" Font-Names="Book Antiqua"
                                                            Font-Bold="true" OnClick="FindBtn_Click" />
                                                        <asp:CheckBox ID="Chkalterotherdept" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                                            OnCheckedChanged="Chkalterotherdept_CheckedChanged" Font-Size="Medium" Text="Add Staff To Other Department"
                                                            AutoPostBack="True" />
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td align="justify">
                                                        <div style="height: 337px; width: 631px; overflow: auto;">
                                                            <asp:GridView runat="server" ID="gview" Visible="false" AutoGenerateColumns="false"
                                                                CssClass="grid-view" GridLines="Both">
                                                                <Columns>
                                                                    <asp:TemplateField HeaderText="S.No">
                                                                        <ItemTemplate>
                                                                            <center>
                                                                                <asp:Label ID="lblno" runat="server" Text='<%#Container.DisplayIndex+1 %>'></asp:Label>
                                                                            </center>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField>
                                                                        <HeaderTemplate>
                                                                            <center>
                                                                                <asp:Label ID="allchk" runat="server" Text="Select"></asp:Label></center>
                                                                        </HeaderTemplate>
                                                                        <ItemTemplate>
                                                                            <asp:CheckBox ID="selectchk" runat="server" />
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="Staff Code">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblcodee" runat="server" Text='<%#Eval("Staff_Code") %>'></asp:Label>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="Staff Name">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblnamee" runat="server" Text='<%#Eval("Staff_Name") %>'></asp:Label>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="Remove">
                                                                        <ItemTemplate>
                                                                            <center>
                                                                                <asp:Button ID="btn_remove" Text="Remove" OnClick="btn_remove" runat="server" /></center>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                </Columns>
                                                                <HeaderStyle BackColor="#0CA6CA" Font-Bold="true" ForeColor="Black" Font-Size="Medium" />
                                                                <FooterStyle BackColor="White" ForeColor="#333333" />
                                                                <PagerStyle BackColor="#336666" HorizontalAlign="Center" />
                                                                <RowStyle ForeColor="#333333" />
                                                                <SelectedRowStyle BackColor="#339966" Font-Bold="True" />
                                                            </asp:GridView>
                                                            <br />
                                                            <center>
                                                                <asp:Button ID="Save" runat="server" Text="Save" Font-Bold="true" Font-Size="Medium"
                                                                    Font-Names="Book Antiqua" OnClick="btnsave_Click" Width="75px" />
                                                            </center>
                                                        </div>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td align="center">
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                </table>
                            </div>
                        </ContentTemplate>
                    </asp:TabPanel>
                    <asp:TabPanel ID="TabPanel3" runat="server" HeaderText="Student's Staff Selector"
                        TabIndex="3" Width="970px" Height="700px">
                        <ContentTemplate>
                            <div>
                                <table>
                                    <tr>
                                        <td colspan="4">
                                            <UC:subject ID="usercontrol2" runat="server"></UC:subject>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <td colspan="2" style="background-color: lightblue; border-color: Black; border-width: 1px;
                                                border-style: solid; height: 30px;">
                                                <asp:RadioButton ID="rbstusubcode" Text="Subject Name" runat="server" GroupName="stusubject"
                                                    Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium" Enabled="true" />
                                                <asp:RadioButton ID="rbstusubacr" Text="Subject Acronym" runat="server" GroupName="stusubject"
                                                    Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium" Enabled="true"
                                                    AutoPostBack="true" OnCheckedChanged="StudentStaffchanged" />
                                            </td>
                                            <td colspan="2" style="background-color: lightblue; border-color: Black; border-width: 1px;
                                                border-style: solid;">
                                                <asp:UpdatePanel ID="updgo" runat="server">
                                                    <ContentTemplate>
                                                        <asp:RadioButton ID="rbstcode" runat="server" Text="Staff Code" GroupName="stfv"
                                                            Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium" Enabled="true"
                                                            AutoPostBack="true" OnCheckedChanged="StudentStaffchanged" />
                                                        <asp:RadioButton ID="rbstname" runat="server" Text="Staff Name" GroupName="stfv"
                                                            Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium" Enabled="true"
                                                            AutoPostBack="true" OnCheckedChanged="StudentStaffchanged" />
                                                        <asp:Button ID="btnstustafgo" Text="Go" runat="server" OnClick="btnstustafgo_Click"
                                                            Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium" Visible="true"
                                                            AutoPostBack="true" OnCheckedChanged="StudentStaffchanged" />
                                                        <asp:Button ID="btnSearchBy" Text="SearchBy" runat="server" OnClick="btnSearchBy_OnClick"
                                                            Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium" Visible="true" />
                                                    </ContentTemplate>
                                                </asp:UpdatePanel>
                                            </td>
                                        </td>
                                    </tr>
                                </table>
                                <asp:Label ID="lblstustaferr" runat="server" Text="" Font-Bold="True" Font-Names="Book Antiqua"
                                    Font-Size="Medium" ForeColor="Red" Style="margin: 0px; margin-bottom: 15px; margin-top: 15px;"></asp:Label>
                                <FarPoint:FpSpread ID="FpSpread2" runat="server" BorderWidth="2px" Height="50px"
                                    Width="900" OnUpdateCommand="FpSpread2_UpdateCommand" Style="margin: 0px; margin-bottom: 15px;
                                    margin-top: 15px;" ShowHeaderSelection="false">
                                    <CommandBar BackColor="Control" ButtonFaceColor="Control" Visible="false" ButtonHighlightColor="ControlLightLight"
                                        ButtonShadowColor="ControlDark">
                                    </CommandBar>
                                    <Sheets>
                                        <FarPoint:SheetView SheetName="Sheet1" DesignString="&lt;?xml version=&quot;1.0&quot; encoding=&quot;utf-8&quot;?&gt;&lt;Sheet&gt;&lt;Data&gt;&lt;RowHeader class=&quot;FarPoint.Web.Spread.Model.DefaultSheetDataModel&quot; rows=&quot;3&quot; columns=&quot;1&quot;&gt;&lt;AutoCalculation&gt;True&lt;/AutoCalculation&gt;&lt;AutoGenerateColumns&gt;True&lt;/AutoGenerateColumns&gt;&lt;ReferenceStyle&gt;A1&lt;/ReferenceStyle&gt;&lt;Iteration&gt;False&lt;/Iteration&gt;&lt;MaximumIterations&gt;1&lt;/MaximumIterations&gt;&lt;MaximumChange&gt;0.001&lt;/MaximumChange&gt;&lt;/RowHeader&gt;&lt;ColumnHeader class=&quot;FarPoint.Web.Spread.Model.DefaultSheetDataModel&quot; rows=&quot;1&quot; columns=&quot;4&quot;&gt;&lt;AutoCalculation&gt;True&lt;/AutoCalculation&gt;&lt;AutoGenerateColumns&gt;True&lt;/AutoGenerateColumns&gt;&lt;ReferenceStyle&gt;A1&lt;/ReferenceStyle&gt;&lt;Iteration&gt;False&lt;/Iteration&gt;&lt;MaximumIterations&gt;1&lt;/MaximumIterations&gt;&lt;MaximumChange&gt;0.001&lt;/MaximumChange&gt;&lt;/ColumnHeader&gt;&lt;DataArea class=&quot;FarPoint.Web.Spread.Model.DefaultSheetDataModel&quot; rows=&quot;3&quot; columns=&quot;4&quot;&gt;&lt;AutoCalculation&gt;True&lt;/AutoCalculation&gt;&lt;AutoGenerateColumns&gt;True&lt;/AutoGenerateColumns&gt;&lt;ReferenceStyle&gt;A1&lt;/ReferenceStyle&gt;&lt;Iteration&gt;False&lt;/Iteration&gt;&lt;MaximumIterations&gt;1&lt;/MaximumIterations&gt;&lt;MaximumChange&gt;0.001&lt;/MaximumChange&gt;&lt;SheetName&gt;Sheet1&lt;/SheetName&gt;&lt;/DataArea&gt;&lt;SheetCorner class=&quot;FarPoint.Web.Spread.Model.DefaultSheetDataModel&quot; rows=&quot;1&quot; columns=&quot;1&quot;&gt;&lt;AutoCalculation&gt;True&lt;/AutoCalculation&gt;&lt;AutoGenerateColumns&gt;True&lt;/AutoGenerateColumns&gt;&lt;ReferenceStyle&gt;A1&lt;/ReferenceStyle&gt;&lt;Iteration&gt;False&lt;/Iteration&gt;&lt;MaximumIterations&gt;1&lt;/MaximumIterations&gt;&lt;MaximumChange&gt;0.001&lt;/MaximumChange&gt;&lt;/SheetCorner&gt;&lt;ColumnFooter class=&quot;FarPoint.Web.Spread.Model.DefaultSheetDataModel&quot; rows=&quot;1&quot; columns=&quot;4&quot;&gt;&lt;AutoCalculation&gt;True&lt;/AutoCalculation&gt;&lt;AutoGenerateColumns&gt;True&lt;/AutoGenerateColumns&gt;&lt;ReferenceStyle&gt;A1&lt;/ReferenceStyle&gt;&lt;Iteration&gt;False&lt;/Iteration&gt;&lt;MaximumIterations&gt;1&lt;/MaximumIterations&gt;&lt;MaximumChange&gt;0.001&lt;/MaximumChange&gt;&lt;/ColumnFooter&gt;&lt;/Data&gt;&lt;Presentation&gt;&lt;ActiveSkin class=&quot;FarPoint.Web.Spread.SheetSkin&quot;&gt;&lt;Name&gt;Default&lt;/Name&gt;&lt;BackColor&gt;Empty&lt;/BackColor&gt;&lt;CellBackColor&gt;Empty&lt;/CellBackColor&gt;&lt;CellForeColor&gt;Empty&lt;/CellForeColor&gt;&lt;CellSpacing&gt;0&lt;/CellSpacing&gt;&lt;GridLines&gt;Both&lt;/GridLines&gt;&lt;GridLineColor&gt;#d0d7e5&lt;/GridLineColor&gt;&lt;HeaderBackColor&gt;Empty&lt;/HeaderBackColor&gt;&lt;HeaderForeColor&gt;Empty&lt;/HeaderForeColor&gt;&lt;FlatColumnHeader&gt;False&lt;/FlatColumnHeader&gt;&lt;FooterBackColor&gt;Empty&lt;/FooterBackColor&gt;&lt;FooterForeColor&gt;Empty&lt;/FooterForeColor&gt;&lt;FlatColumnFooter&gt;False&lt;/FlatColumnFooter&gt;&lt;FlatRowHeader&gt;False&lt;/FlatRowHeader&gt;&lt;HeaderFontBold&gt;False&lt;/HeaderFontBold&gt;&lt;FooterFontBold&gt;False&lt;/FooterFontBold&gt;&lt;SelectionBackColor&gt;#eaecf5&lt;/SelectionBackColor&gt;&lt;SelectionForeColor&gt;Empty&lt;/SelectionForeColor&gt;&lt;EvenRowBackColor&gt;Empty&lt;/EvenRowBackColor&gt;&lt;OddRowBackColor&gt;Empty&lt;/OddRowBackColor&gt;&lt;ShowColumnHeader&gt;True&lt;/ShowColumnHeader&gt;&lt;ShowColumnFooter&gt;False&lt;/ShowColumnFooter&gt;&lt;ShowRowHeader&gt;True&lt;/ShowRowHeader&gt;&lt;ColumnHeaderBackground class=&quot;FarPoint.Web.Spread.Background&quot;&gt;&lt;BackgroundImageUrl&gt;SPREADCLIENTPATH:/img/chbg.gif&lt;/BackgroundImageUrl&gt;&lt;SelectedBackgroundImageUrl&gt;SPREADCLIENTPATH:/img/chm.png&lt;/SelectedBackgroundImageUrl&gt;&lt;/ColumnHeaderBackground&gt;&lt;SheetCornerBackground class=&quot;FarPoint.Web.Spread.Background&quot;&gt;&lt;BackgroundImageUrl&gt;SPREADCLIENTPATH:/img/chbg.gif&lt;/BackgroundImageUrl&gt;&lt;SelectedBackgroundImageUrl&gt;SPREADCLIENTPATH:/img/chm.png&lt;/SelectedBackgroundImageUrl&gt;&lt;/SheetCornerBackground&gt;&lt;HeaderGrayAreaColor&gt;#7999c2&lt;/HeaderGrayAreaColor&gt;&lt;/ActiveSkin&gt;&lt;AxisModels&gt;&lt;Column class=&quot;FarPoint.Web.Spread.Model.DefaultSheetAxisModel&quot; orientation=&quot;Horizontal&quot; count=&quot;4&quot;&gt;&lt;Items&gt;&lt;Item index=&quot;-1&quot;&gt;&lt;SortIndicator&gt;Ascending&lt;/SortIndicator&gt;&lt;/Item&gt;&lt;Item index=&quot;0&quot;&gt;&lt;Size&gt;55&lt;/Size&gt;&lt;/Item&gt;&lt;Item index=&quot;1&quot;&gt;&lt;Size&gt;111&lt;/Size&gt;&lt;/Item&gt;&lt;Item index=&quot;2&quot;&gt;&lt;Size&gt;81&lt;/Size&gt;&lt;/Item&gt;&lt;Item index=&quot;3&quot;&gt;&lt;Size&gt;79&lt;/Size&gt;&lt;/Item&gt;&lt;/Items&gt;&lt;/Column&gt;&lt;RowHeaderColumn class=&quot;FarPoint.Web.Spread.Model.DefaultSheetAxisModel&quot; defaultSize=&quot;40&quot; orientation=&quot;Horizontal&quot; count=&quot;1&quot;&gt;&lt;Items&gt;&lt;Item index=&quot;-1&quot;&gt;&lt;SortIndicator&gt;Ascending&lt;/SortIndicator&gt;&lt;Size&gt;40&lt;/Size&gt;&lt;/Item&gt;&lt;/Items&gt;&lt;/RowHeaderColumn&gt;&lt;ColumnHeaderRow class=&quot;FarPoint.Web.Spread.Model.DefaultSheetAxisModel&quot; defaultSize=&quot;22&quot; orientation=&quot;Vertical&quot; count=&quot;1&quot;&gt;&lt;Items&gt;&lt;Item index=&quot;-1&quot;&gt;&lt;Size&gt;22&lt;/Size&gt;&lt;/Item&gt;&lt;/Items&gt;&lt;/ColumnHeaderRow&gt;&lt;ColumnFooterRow class=&quot;FarPoint.Web.Spread.Model.DefaultSheetAxisModel&quot; defaultSize=&quot;22&quot; orientation=&quot;Vertical&quot; count=&quot;1&quot;&gt;&lt;Items&gt;&lt;Item index=&quot;-1&quot;&gt;&lt;Size&gt;22&lt;/Size&gt;&lt;/Item&gt;&lt;/Items&gt;&lt;/ColumnFooterRow&gt;&lt;/AxisModels&gt;&lt;StyleModels&gt;&lt;RowHeader class=&quot;FarPoint.Web.Spread.Model.DefaultSheetStyleModel&quot; Rows=&quot;3&quot; Columns=&quot;1&quot;&gt;&lt;AltRowCount&gt;2&lt;/AltRowCount&gt;&lt;DefaultStyle class=&quot;FarPoint.Web.Spread.NamedStyle&quot; Parent=&quot;RowHeaderDefault&quot; /&gt;&lt;ConditionalFormatCollections /&gt;&lt;/RowHeader&gt;&lt;ColumnHeader class=&quot;FarPoint.Web.Spread.Model.DefaultSheetStyleModel&quot; Rows=&quot;1&quot; Columns=&quot;4&quot;&gt;&lt;AltRowCount&gt;2&lt;/AltRowCount&gt;&lt;DefaultStyle class=&quot;FarPoint.Web.Spread.NamedStyle&quot; Parent=&quot;ColumnHeaderDefault&quot;&gt;&lt;Background class=&quot;FarPoint.Web.Spread.Background&quot;&gt;&lt;BackgroundImageUrl&gt;SPREADCLIENTPATH:/img/chbg.gif&lt;/BackgroundImageUrl&gt;&lt;SelectedBackgroundImageUrl&gt;SPREADCLIENTPATH:/img/chm.png&lt;/SelectedBackgroundImageUrl&gt;&lt;/Background&gt;&lt;/DefaultStyle&gt;&lt;ConditionalFormatCollections /&gt;&lt;/ColumnHeader&gt;&lt;DataArea class=&quot;FarPoint.Web.Spread.Model.DefaultSheetStyleModel&quot; Rows=&quot;3&quot; Columns=&quot;4&quot;&gt;&lt;AltRowCount&gt;2&lt;/AltRowCount&gt;&lt;DefaultStyle class=&quot;FarPoint.Web.Spread.NamedStyle&quot; Parent=&quot;DataAreaDefault&quot;&gt;&lt;Font&gt;&lt;Name&gt;Book Antiqua&lt;/Name&gt;&lt;Names&gt;&lt;Name&gt;Book Antiqua&lt;/Name&gt;&lt;/Names&gt;&lt;Size&gt;Medium&lt;/Size&gt;&lt;Bold&gt;False&lt;/Bold&gt;&lt;Italic&gt;False&lt;/Italic&gt;&lt;Overline&gt;False&lt;/Overline&gt;&lt;Strikeout&gt;False&lt;/Strikeout&gt;&lt;Underline&gt;False&lt;/Underline&gt;&lt;/Font&gt;&lt;GdiCharSet&gt;254&lt;/GdiCharSet&gt;&lt;ForeColor&gt;#0033cc&lt;/ForeColor&gt;&lt;HorizontalAlign&gt;Center&lt;/HorizontalAlign&gt;&lt;/DefaultStyle&gt;&lt;ConditionalFormatCollections /&gt;&lt;/DataArea&gt;&lt;SheetCorner class=&quot;FarPoint.Web.Spread.Model.DefaultSheetStyleModel&quot; Rows=&quot;1&quot; Columns=&quot;1&quot;&gt;&lt;AltRowCount&gt;2&lt;/AltRowCount&gt;&lt;DefaultStyle class=&quot;FarPoint.Web.Spread.NamedStyle&quot; Parent=&quot;CornerDefault&quot;&gt;&lt;Border class=&quot;FarPoint.Web.Spread.Border&quot; Size=&quot;1&quot; Style=&quot;Solid&quot;&gt;&lt;Bottom Color=&quot;ControlDark&quot; /&gt;&lt;Left Size=&quot;0&quot; /&gt;&lt;Right Color=&quot;ControlDark&quot; /&gt;&lt;Top Size=&quot;0&quot; /&gt;&lt;/Border&gt;&lt;Background class=&quot;FarPoint.Web.Spread.Background&quot;&gt;&lt;BackgroundImageUrl&gt;SPREADCLIENTPATH:/img/chbg.gif&lt;/BackgroundImageUrl&gt;&lt;SelectedBackgroundImageUrl&gt;SPREADCLIENTPATH:/img/chm.png&lt;/SelectedBackgroundImageUrl&gt;&lt;/Background&gt;&lt;/DefaultStyle&gt;&lt;ConditionalFormatCollections /&gt;&lt;/SheetCorner&gt;&lt;ColumnFooter class=&quot;FarPoint.Web.Spread.Model.DefaultSheetStyleModel&quot; Rows=&quot;1&quot; Columns=&quot;4&quot;&gt;&lt;AltRowCount&gt;2&lt;/AltRowCount&gt;&lt;DefaultStyle class=&quot;FarPoint.Web.Spread.NamedStyle&quot; Parent=&quot;ColumnFooterDefault&quot; /&gt;&lt;ConditionalFormatCollections /&gt;&lt;/ColumnFooter&gt;&lt;/StyleModels&gt;&lt;MessageRowStyle class=&quot;FarPoint.Web.Spread.Appearance&quot;&gt;&lt;BackColor&gt;LightYellow&lt;/BackColor&gt;&lt;ForeColor&gt;Red&lt;/ForeColor&gt;&lt;/MessageRowStyle&gt;&lt;SheetCornerStyle class=&quot;FarPoint.Web.Spread.NamedStyle&quot; Parent=&quot;CornerDefault&quot;&gt;&lt;Border class=&quot;FarPoint.Web.Spread.Border&quot; Size=&quot;1&quot; Style=&quot;Solid&quot;&gt;&lt;Bottom Color=&quot;ControlDark&quot; /&gt;&lt;Left Size=&quot;0&quot; /&gt;&lt;Right Color=&quot;ControlDark&quot; /&gt;&lt;Top Size=&quot;0&quot; /&gt;&lt;/Border&gt;&lt;Background class=&quot;FarPoint.Web.Spread.Background&quot;&gt;&lt;BackgroundImageUrl&gt;SPREADCLIENTPATH:/img/chbg.gif&lt;/BackgroundImageUrl&gt;&lt;SelectedBackgroundImageUrl&gt;SPREADCLIENTPATH:/img/chm.png&lt;/SelectedBackgroundImageUrl&gt;&lt;/Background&gt;&lt;/SheetCornerStyle&gt;&lt;AllowLoadOnDemand&gt;false&lt;/AllowLoadOnDemand&gt;&lt;LoadRowIncrement &gt;10&lt;/LoadRowIncrement &gt;&lt;LoadInitRowCount &gt;30&lt;/LoadInitRowCount &gt;&lt;AllowVirtualScrollPaging&gt;false&lt;/AllowVirtualScrollPaging&gt;&lt;TopRow&gt;0&lt;/TopRow&gt;&lt;PreviewRowStyle class=&quot;FarPoint.Web.Spread.PreviewRowInfo&quot; /&gt;&lt;/Presentation&gt;&lt;Settings&gt;&lt;Name&gt;Sheet1&lt;/Name&gt;&lt;Categories&gt;&lt;Appearance&gt;&lt;GridLineColor&gt;#d0d7e5&lt;/GridLineColor&gt;&lt;SelectionBackColor&gt;#eaecf5&lt;/SelectionBackColor&gt;&lt;SelectionBorder class=&quot;FarPoint.Web.Spread.Border&quot; /&gt;&lt;/Appearance&gt;&lt;Behavior&gt;&lt;EditTemplateColumnCount&gt;2&lt;/EditTemplateColumnCount&gt;&lt;ScrollingContentVisible&gt;True&lt;/ScrollingContentVisible&gt;&lt;PageSize&gt;100&lt;/PageSize&gt;&lt;AllowPage&gt;False&lt;/AllowPage&gt;&lt;GroupBarText&gt;Drag a column to group by that column.&lt;/GroupBarText&gt;&lt;/Behavior&gt;&lt;Layout&gt;&lt;RowHeaderColumnCount&gt;1&lt;/RowHeaderColumnCount&gt;&lt;ColumnHeaderRowCount&gt;1&lt;/ColumnHeaderRowCount&gt;&lt;/Layout&gt;&lt;/Categories&gt;&lt;ActiveRow&gt;0&lt;/ActiveRow&gt;&lt;ActiveColumn&gt;0&lt;/ActiveColumn&gt;&lt;ColumnHeaderRowCount&gt;1&lt;/ColumnHeaderRowCount&gt;&lt;ColumnFooterRowCount&gt;1&lt;/ColumnFooterRowCount&gt;&lt;PrintInfo&gt;&lt;Header /&gt;&lt;Footer /&gt;&lt;ZoomFactor&gt;0&lt;/ZoomFactor&gt;&lt;FirstPageNumber&gt;1&lt;/FirstPageNumber&gt;&lt;Orientation&gt;Auto&lt;/Orientation&gt;&lt;PrintType&gt;All&lt;/PrintType&gt;&lt;PageOrder&gt;Auto&lt;/PageOrder&gt;&lt;BestFitCols&gt;False&lt;/BestFitCols&gt;&lt;BestFitRows&gt;False&lt;/BestFitRows&gt;&lt;PageStart&gt;-1&lt;/PageStart&gt;&lt;PageEnd&gt;-1&lt;/PageEnd&gt;&lt;ColStart&gt;-1&lt;/ColStart&gt;&lt;ColEnd&gt;-1&lt;/ColEnd&gt;&lt;RowStart&gt;-1&lt;/RowStart&gt;&lt;RowEnd&gt;-1&lt;/RowEnd&gt;&lt;ShowBorder&gt;True&lt;/ShowBorder&gt;&lt;ShowGrid&gt;True&lt;/ShowGrid&gt;&lt;ShowColor&gt;True&lt;/ShowColor&gt;&lt;ShowColumnHeader&gt;Inherit&lt;/ShowColumnHeader&gt;&lt;ShowRowHeader&gt;Inherit&lt;/ShowRowHeader&gt;&lt;ShowColumnFooter&gt;Inherit&lt;/ShowColumnFooter&gt;&lt;ShowColumnFooterEachPage&gt;True&lt;/ShowColumnFooterEachPage&gt;&lt;ShowTitle&gt;True&lt;/ShowTitle&gt;&lt;ShowSubtitle&gt;True&lt;/ShowSubtitle&gt;&lt;UseMax&gt;True&lt;/UseMax&gt;&lt;UseSmartPrint&gt;False&lt;/UseSmartPrint&gt;&lt;Opacity&gt;255&lt;/Opacity&gt;&lt;PrintNotes&gt;None&lt;/PrintNotes&gt;&lt;Centering&gt;None&lt;/Centering&gt;&lt;RepeatColStart&gt;-1&lt;/RepeatColStart&gt;&lt;RepeatColEnd&gt;-1&lt;/RepeatColEnd&gt;&lt;RepeatRowStart&gt;-1&lt;/RepeatRowStart&gt;&lt;RepeatRowEnd&gt;-1&lt;/RepeatRowEnd&gt;&lt;SmartPrintPagesTall&gt;1&lt;/SmartPrintPagesTall&gt;&lt;SmartPrintPagesWide&gt;1&lt;/SmartPrintPagesWide&gt;&lt;HeaderHeight&gt;-1&lt;/HeaderHeight&gt;&lt;FooterHeight&gt;-1&lt;/FooterHeight&gt;&lt;/PrintInfo&gt;&lt;TitleInfo class=&quot;FarPoint.Web.Spread.TitleInfo&quot;&gt;&lt;Style class=&quot;FarPoint.Web.Spread.StyleInfo&quot;&gt;&lt;BackColor&gt;#e7eff7&lt;/BackColor&gt;&lt;HorizontalAlign&gt;Right&lt;/HorizontalAlign&gt;&lt;/Style&gt;&lt;Value type=&quot;System.String&quot; whitespace=&quot;&quot; /&gt;&lt;/TitleInfo&gt;&lt;LayoutTemplate class=&quot;FarPoint.Web.Spread.LayoutTemplate&quot;&gt;&lt;Layout&gt;&lt;ColumnCount&gt;4&lt;/ColumnCount&gt;&lt;RowCount&gt;1&lt;/RowCount&gt;&lt;/Layout&gt;&lt;Data&gt;&lt;LayoutData class=&quot;FarPoint.Web.Spread.Model.DefaultSheetDataModel&quot; rows=&quot;1&quot; columns=&quot;4&quot;&gt;&lt;AutoCalculation&gt;True&lt;/AutoCalculation&gt;&lt;AutoGenerateColumns&gt;True&lt;/AutoGenerateColumns&gt;&lt;ReferenceStyle&gt;A1&lt;/ReferenceStyle&gt;&lt;Iteration&gt;False&lt;/Iteration&gt;&lt;MaximumIterations&gt;1&lt;/MaximumIterations&gt;&lt;MaximumChange&gt;0.001&lt;/MaximumChange&gt;&lt;Cells&gt;&lt;Cell row=&quot;0&quot; column=&quot;0&quot;&gt;&lt;Data type=&quot;System.Int32&quot;&gt;0&lt;/Data&gt;&lt;/Cell&gt;&lt;Cell row=&quot;0&quot; column=&quot;1&quot;&gt;&lt;Data type=&quot;System.Int32&quot;&gt;1&lt;/Data&gt;&lt;/Cell&gt;&lt;Cell row=&quot;0&quot; column=&quot;2&quot;&gt;&lt;Data type=&quot;System.Int32&quot;&gt;2&lt;/Data&gt;&lt;/Cell&gt;&lt;Cell row=&quot;0&quot; column=&quot;3&quot;&gt;&lt;Data type=&quot;System.Int32&quot;&gt;3&lt;/Data&gt;&lt;/Cell&gt;&lt;/Cells&gt;&lt;/LayoutData&gt;&lt;/Data&gt;&lt;AxisModels&gt;&lt;LayoutColumn class=&quot;FarPoint.Web.Spread.Model.DefaultSheetAxisModel&quot; orientation=&quot;Horizontal&quot; count=&quot;4&quot;&gt;&lt;Items&gt;&lt;Item index=&quot;-1&quot;&gt;&lt;SortIndicator&gt;Ascending&lt;/SortIndicator&gt;&lt;/Item&gt;&lt;/Items&gt;&lt;/LayoutColumn&gt;&lt;LayoutRow class=&quot;FarPoint.Web.Spread.Model.DefaultSheetAxisModel&quot; orientation=&quot;Vertical&quot; count=&quot;1&quot;&gt;&lt;Items&gt;&lt;Item index=&quot;-1&quot; /&gt;&lt;/Items&gt;&lt;/LayoutRow&gt;&lt;/AxisModels&gt;&lt;/LayoutTemplate&gt;&lt;LayoutMode&gt;CellLayoutMode&lt;/LayoutMode&gt;&lt;CurrentPageIndex type=&quot;System.Int32&quot;&gt;0&lt;/CurrentPageIndex&gt;&lt;/Settings&gt;&lt;/Sheet&gt;">
                                        </FarPoint:SheetView>
                                    </Sheets>
                                </FarPoint:FpSpread>
                                <asp:Button ID="btnstustaffsave" runat="server" Text="Save" Font-Bold="true" Font-Size="Medium"
                                    Font-Names="Book Antiqua" OnClick="btnstustaffsave_Click" Width="75px" />
                                <asp:Button ID="btnstustaffprint" runat="server" Text="Print" Font-Bold="true" Font-Size="Medium"
                                    Font-Names="Book Antiqua" OnClick="btnstustaffprint_Click" Width="75px" />
                                <UC:Print ID="ucstuprint" runat="server" />
                            </div>
                        </ContentTemplate>
                    </asp:TabPanel>
                </asp:TabContainer>
                <%--Style="left: 353px; top: 275px; position: absolute;"--%>
                <asp:Panel ID="panel3" runat="server" BorderColor="Black" BackColor="AliceBlue" Visible="false"
                    BorderWidth="2px" Style="left: 30%; top: 35%; right: 30%; position: absolute;
                    overflow: auto; z-index: 3;" Height="480px" Width="715px">
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
                                    <asp:DropDownList ID="ddlcollege" runat="server" Width="150px" OnSelectedIndexChanged="ddlcollege_SelectedIndexChanged"
                                        AutoPostBack="true">
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="lblDepartment" runat="server" Text="Department"></asp:Label>
                                </td>
                                <td>
                                    <asp:DropDownList ID="ddldepratstaff" runat="server" Width="150px" OnSelectedIndexChanged="ddldepratstaff_SelectedIndexChanged"
                                        AutoPostBack="true">
                                    </asp:DropDownList>
                                </td>
                                <td>
                                    <asp:Label ID="LblCategory" runat="server" Text="Staff Category"></asp:Label>
                                    <asp:TextBox ID="txt_Category" runat="server" CssClass="textbox textbox1" ReadOnly="true"
                                        Width="135px" Font-Bold="True" Visible="true">---Select---</asp:TextBox>
                                    <asp:Panel ID="panel_Category" runat="server" CssClass="multxtpanel" Height="250px"
                                        Width="355px" Style="text-align: left;">
                                        <asp:CheckBox ID="cb_Category" runat="server" OnCheckedChanged="cb_Category_CheckedChanged"
                                            Text="Select All" AutoPostBack="True" TextAlign="Right" Style="text-align: left;" />
                                        <asp:CheckBoxList ID="cbl_Category" runat="server" AutoPostBack="True" OnSelectedIndexChanged="cbl_Category_SelectedIndexChanged"
                                            TextAlign="Right">
                                        </asp:CheckBoxList>
                                    </asp:Panel>
                                    <asp:PopupControlExtender ID="PopupControlExtender3" runat="server" TargetControlID="txt_Category"
                                        PopupControlID="panel_Category" Position="Bottom">
                                    </asp:PopupControlExtender>
                                    <asp:Button ID="BtnCategory" runat="server" Text="Go" Font-Bold="true" Font-Size="Medium"
                                        Font-Names="Book Antiqua" OnClick="BtnCategory_Click" Width="53px" />
                                </td>
                                <%--<td>
                                    <asp:DropDownList ID="ddlCategory" runat="server" Width="150px" OnSelectedIndexChanged="ddlCategory_SelectedIndexChanged"
                                        AutoPostBack="true">
                                    </asp:DropDownList>
                                </td>--%>
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
                        <asp:UpdatePanel ID="upael3" runat="server">
                            <ContentTemplate>
                                <div id="div7" runat="server" style="overflow: auto; border: 1px solid Gray; width: 460px;
                                    height: 280px;">
                                    <asp:GridView runat="server" ID="gviewstaff" AutoGenerateColumns="false" Style="height: 300;
                                        width: 460px; overflow: auto;">
                                        <Columns>
                                            <asp:TemplateField HeaderText="S.No">
                                                <ItemTemplate>
                                                    <%#Container.DisplayIndex+1 %>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField>
                                                <HeaderTemplate>
                                                    <center>
                                                        <asp:Label ID="allchk" runat="server" Text="Select"></asp:Label></center>
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <asp:CheckBox ID="selectchk1" runat="server" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Staff Code">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblstaff" runat="server" Text='<%#Eval("Staff_Code") %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Staff Name">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblname" runat="server" Text='<%#Eval("Staff_Name") %>'></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Left" />
                                            </asp:TemplateField>
                                        </Columns>
                                        <HeaderStyle BackColor="#0CA6CA" Font-Bold="true" ForeColor="Black" Font-Size="Medium" />
                                        <FooterStyle BackColor="White" ForeColor="#333333" />
                                        <PagerStyle BackColor="#336666" HorizontalAlign="Center" />
                                        <RowStyle ForeColor="#333333" />
                                        <SelectedRowStyle BackColor="#339966" Font-Bold="True" />
                                    </asp:GridView>
                                    <fieldset style="position: absolute; left: 345px; visibility: visible; top: 426px;
                                        width: 140px; height: 2px;">
                                        <asp:Button runat="server" ID="btnstaffadd" AutoPostBack="True" Text="Ok" Font-Bold="true"
                                            OnClick="btnstaffadd_Click" Style="width: 75px; top: 2px; position: absolute;
                                            left: 2px;" />
                                        <asp:Button runat="server" ID="btnexit" AutoPostBack="True" Text="Exit" Font-Bold="true"
                                            OnClick="btnexit_Click" Style="width: 75px; top: 2px; position: absolute; left: 85px;" />
                                    </fieldset>
                                </div>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </div>
                </asp:Panel>
                <center>
                    <asp:Panel ID="panel2" runat="server" BorderColor="Black" BackColor="AliceBlue" Visible="false"
                        BorderWidth="2px" Style="left: 30%; top: 35%; right: 30%; position: absolute;
                        z-index: 3; height: auto; width: auto;">
                        <div id="div3" runat="server" class="PopupHeaderrstud2" visible="false" style="height: 550em;
                            z-index: 2000; width: auto; background-color: rgba(54, 25, 25, .2); top: 0%;
                            left: 0%; height: auto;">
                            <center>
                                <div id="div4" runat="server" class="PopupHeaderrstud2" style="background-color: White;
                                    height: auto; width: 464px; border: 5px solid #0CA6CA; border-top: 25px solid #0CA6CA;
                                    left: 28%; right: 39%; top: 27%; padding: 5px; border-radius: 10px;">
                                    <center>
                                        <div>
                                            <asp:Label ID="lblstu1" runat="server" Style="color: Green;" Text="Add Staff To another Deaprtment"
                                                CssClass="fontstyleheader"></asp:Label>
                                        </div>
                                    </center>
                                    <br />
                                    <center>
                                        <asp:GridView ID="gvatte" runat="server" ShowHeader="true" AutoGenerateColumns="False"
                                            OnRowDataBound="gvatte_OnDataBinding" Width="364px" CssClass="font">
                                            <Columns>
                                                <asp:TemplateField HeaderText="S.No" HeaderStyle-BackColor="#0CA6CA" HeaderStyle-HorizontalAlign="center"
                                                    HeaderStyle-Height="30px">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblsno" runat="server" Text="<%# Container.DisplayIndex+1 %>"></asp:Label>
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="Center" Width="50px" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Course Name" HeaderStyle-BackColor="#0CA6CA" HeaderStyle-HorizontalAlign="center">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblcourse" runat="server" Text='<%# Eval("course")  %>' Style="width: 10px;
                                                            text-align: center;" />
                                                        <%--   onclick="Check_Click1(this);"--%>
                                                        <asp:Label ID="lblsubj" Visible="false" runat="server" Text='<%# Eval("subject_no")%>'
                                                            Style="width: 100px; text-align: center;" />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Select" HeaderStyle-BackColor="#0CA6CA" HeaderStyle-HorizontalAlign="center">
                                                    <HeaderTemplate>
                                                        <asp:CheckBox ID="chkSelectAll" runat="server" AutoPostBack="true" OnCheckedChanged="SelectAll_Checked" />
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:CheckBox ID="chk11" runat="server" CssClass="mycheckbox" /><%--onclick="Check_Click(this);"--%>
                                                        <asp:Label ID="lbldeg" Visible="false" runat="server" Text='<%# Eval("degree_code")%>'
                                                            Style="width: 100px" />
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="Center" Width="100px" />
                                                </asp:TemplateField>
                                            </Columns>
                                        </asp:GridView>
                                    </center>
                                    <br />
                                    <center>
                                        <asp:Button ID="Btnok" runat="server" Text="Ok" OnClick="Btnok_Click" Font-Bold="True"
                                            Font-Names="Book Antiqua" Font-Size="Medium" />
                                        <asp:Button ID="Btncancle" runat="server" Text="Cancel" OnClick="Btncancle_Click"
                                            Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium" />
                                        <asp:Label ID="subjno" runat="server" Text="" Visible="false"></asp:Label>
                                    </center>
                                </div>
                            </center>
                        </div>
                    </asp:Panel>
            </ContentTemplate>
        </asp:UpdatePanel>
    </center>
    <%-- </ContentTemplate>--%>
    <%--<Triggers>
                <asp:PostBackTrigger ControlID="TabContainer1" />
            </Triggers>--%>
    <%--</asp:UpdatePanel>--%>
    </center>
    <asp:UpdatePanel ID="upd2" runat="server">
        <ContentTemplate>
            <asp:HiddenField ID="hfsave" runat="server" />
            <asp:ModalPopupExtender ID="mpesave" runat="server" TargetControlID="hfsave" PopupControlID="psave">
            </asp:ModalPopupExtender>
            <asp:Panel ID="psave" runat="server" CssClass="modalPopup" Style="display: none;
                height: 500; width: 500;" DefaultButton="btnsaveok">
                <table width="500">
                    <tr class="topHandle">
                        <td colspan="2" align="left" runat="server" id="td1">
                            <asp:Label ID="Label6" runat="server" Font-Bold="True" Text="Confirmation" Font-Names="Book Antiqua"
                                Font-Size="Large"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 60px" valign="middle" align="center">
                            <asp:Image ID="Image1" runat="server" ImageUrl="../images/Info-48x48.png" />
                        </td>
                        <td valign="middle" align="left">
                            <asp:Label ID="Label7" Text="Already allocate the batch for this class.You want to save this changes means, you should re-allocate the batches.Do you want to continue?"
                                runat="server" Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2" align="right">
                            <asp:Button ID="btnsaveok" runat="server" Text="Yes" OnClick="btnsaveok_Click" Font-Bold="True"
                                Font-Names="Book Antiqua" Font-Size="Medium" />
                            <asp:Button ID="btnsaveCancel" runat="server" Text="No" OnClick="btnsaveCancel_Click"
                                Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium" />
                        </td>
                    </tr>
                </table>
            </asp:Panel>
        </ContentTemplate>
        <%--<Triggers>
            <asp:PostBackTrigger ControlID="btnsaveok" />
        </Triggers>--%>
    </asp:UpdatePanel>
    <center>
        <asp:UpdatePanel ID="upd3" runat="server">
            <ContentTemplate>
                <div id="divPopSearchstudent" runat="server" visible="false" style="height: 550em;
                    z-index: 2000; width: 100%; background-color: rgba(54, 25, 25, .2); position: absolute;
                    top: 0%; left: 0%;">
                    <center>
                        <div id="divPopAlertContent" runat="server" class="table" style="background-color: White;
                            height: 120px; width: 50%; border: 5px solid #0CA6CA; border-top: 25px solid #0CA6CA;
                            left: 39%; right: 50%; top: 35%; padding: 5px; position: fixed; border-radius: 10px;">
                            <center>
                                <table style="height: 100px; width: 100%; padding: 5px;">
                                    <tr>
                                        <td>
                                            <table id="tblSearchStudent" runat="server" visible="true">
                                                <tr>
                                                    <td>
                                                        <asp:DropDownList ID="ddlSearchBy" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlSearchBy_SelectedIndexChanged"
                                                            Font-Bold="true">
                                                        </asp:DropDownList>
                                                        <asp:Label ID="lblValue" runat="server" Style="display: none;"></asp:Label>
                                                    </td>
                                                    <td>
                                                        <div id="divSearchStudent">
                                                            <asp:TextBox ID="txtRollNo" runat="server" Font-Names="Book Antiqua" Width="300px"
                                                                Font-Size="Medium" Visible="false"></asp:TextBox>
                                                            <asp:AutoCompleteExtender ID="AutoCompleteExtender1" runat="server" DelimiterCharacters=","
                                                                Enabled="True" ServiceMethod="GetRollNo" MinimumPrefixLength="0" CompletionInterval="100"
                                                                EnableCaching="false" CompletionSetCount="10" ServicePath="" TargetControlID="txtRollNo"
                                                                CompletionListCssClass="autocomplete_completionListElement" CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem"
                                                                CompletionListItemCssClass="txtsearchpan" OnClientPopulating="AutoCompleteExtender1_OnClientPopulating">
                                                            </asp:AutoCompleteExtender>
                                                            <asp:TextBox ID="txtRegNo" runat="server" Font-Names="Book Antiqua" Width="300px"
                                                                Font-Size="Medium" Visible="false"></asp:TextBox>
                                                            <asp:AutoCompleteExtender ID="AutoCompleteExtender2" runat="server" DelimiterCharacters=","
                                                                Enabled="True" ServiceMethod="GetRegNo" MinimumPrefixLength="0" CompletionInterval="100"
                                                                EnableCaching="false" CompletionSetCount="10" ServicePath="" TargetControlID="txtRegNo"
                                                                CompletionListCssClass="autocomplete_completionListElement" CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem"
                                                                CompletionListItemCssClass="txtsearchpan" OnClientPopulating="AutoCompleteExtender1_OnClientPopulating">
                                                            </asp:AutoCompleteExtender>
                                                            <asp:TextBox ID="txtAdmissionNo" runat="server" Font-Names="Book Antiqua" Width="300px"
                                                                Font-Size="Medium" Visible="false"></asp:TextBox>
                                                            <asp:AutoCompleteExtender ID="AutoCompleteExtender3" runat="server" DelimiterCharacters=","
                                                                Enabled="True" ServiceMethod="GetAdmitNo" MinimumPrefixLength="0" CompletionInterval="100"
                                                                EnableCaching="false" CompletionSetCount="10" ServicePath="" TargetControlID="txtAdmissionNo"
                                                                CompletionListCssClass="autocomplete_completionListElement" CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem"
                                                                CompletionListItemCssClass="txtsearchpan" OnClientPopulating="AutoCompleteExtender1_OnClientPopulating">
                                                            </asp:AutoCompleteExtender>
                                                        </div>
                                                    </td>
                                                    <td>
                                                        <asp:Button ID="btnSearchbyrollorreg" Font-Bold="true" Font-Size="Medium" Font-Names="Book Antiqua"
                                                            CssClass="textbox textbox1" Style="height: auto; width: auto;" OnClick="btnSearchbyrollorreg_Click"
                                                            Text="Search" runat="server" />
                                                    </td>
                                                    <td>
                                                        <asp:Button ID="btnsearchByClose" Font-Bold="true" Font-Size="Medium" Font-Names="Book Antiqua"
                                                            CssClass="textbox textbox1" Style="height: auto; width: auto;" OnClick="btnsearchByClose_Click"
                                                            Text="Close" runat="server" />
                                                    </td>
                                                </tr>
                                            </table>
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
        <asp:UpdatePanel ID="Upanel" runat="server">
            <ContentTemplate>
                <div id="div5" runat="server" visible="false" style="height: 550em; z-index: 2000;
                    width: 100%; background-color: rgba(54, 25, 25, .2); position: absolute; top: 0%;
                    left: 0%;">
                    <center>
                        <div id="div6" runat="server" class="table" style="background-color: White; height: 120px;
                            width: 23%; border: 5px solid #0CA6CA; border-top: 25px solid #0CA6CA; left: 39%;
                            right: 39%; top: 35%; padding: 5px; position: fixed; border-radius: 10px;">
                            <center>
                                <asp:Label ID="Label1" runat="server" Visible="false"></asp:Label>
                                <asp:Label ID="Label3" runat="server" Visible="false"></asp:Label>
                                <table style="height: 100px; width: 100%; padding: 5px;">
                                    <tr>
                                        <td align="center">
                                            <asp:Label ID="Label4" runat="server" Style="color: Red;" Font-Bold="true" Font-Size="Medium"
                                                Visible="true"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <center>
                                                <asp:UpdatePanel ID="upanel5" runat="server">
                                                    <ContentTemplate>
                                                        <asp:Button ID="Button3" Font-Bold="true" Font-Size="Medium" Font-Names="Book Antiqua"
                                                            CssClass="textbox textbox1" Style="height: auto; width: auto;" OnClick="btnPopAlertClose1_Click"
                                                            Text="Ok" runat="server" />
                                                        <asp:Button ID="Button4" Font-Bold="true" Font-Size="Medium" Font-Names="Book Antiqua"
                                                            CssClass="textbox textbox1" Style="height: auto; width: auto;" OnClick="btnCancel1_Click"
                                                            Text="Cancel" runat="server" />
                                                    </ContentTemplate>
                                                </asp:UpdatePanel>
                                            </center>
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
        <asp:UpdatePanel ID="Uupanel1" runat="server">
            <ContentTemplate>
                <div id="divPopAlert" runat="server" visible="false" style="height: 550em; z-index: 2000;
                    width: 100%; background-color: rgba(54, 25, 25, .2); position: absolute; top: 0%;
                    left: 0%;">
                    <center>
                        <div id="div2" runat="server" class="table" style="background-color: White; height: 120px;
                            width: 23%; border: 5px solid #0CA6CA; border-top: 25px solid #0CA6CA; left: 39%;
                            right: 39%; top: 35%; padding: 5px; position: fixed; border-radius: 10px;">
                            <center>
                                <asp:Label ID="lblSubNo" runat="server" Visible="false"></asp:Label>
                                <asp:Label ID="lblStaffCode" runat="server" Visible="false"></asp:Label>
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
                                                <asp:Button ID="btnCancel" Font-Bold="true" Font-Size="Medium" Font-Names="Book Antiqua"
                                                    CssClass="textbox textbox1" Style="height: auto; width: auto;" OnClick="btnCancel_Click"
                                                    Text="Cancel" runat="server" />
                                            </center>
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
        <div id="div8" runat="server" visible="false" style="height: 550em; z-index: 2000;
            width: 100%; background-color: rgba(54, 25, 25, .2); position: absolute; top: 0%;
            left: 0%;">
            <center>
                <div id="div9" runat="server" class="table" style="background-color: White; height: 120px;
                    width: 23%; border: 5px solid #0CA6CA; border-top: 25px solid #0CA6CA; left: 39%;
                    right: 39%; top: 35%; padding: 5px; position: fixed; border-radius: 10px;">
                    <center>
                        <table style="height: 100px; width: 100%; padding: 5px;">
                            <tr>
                                <td align="center">
                                    <asp:Label ID="Label5" runat="server" Style="color: Red;" Font-Bold="true" Font-Size="Medium"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <center>
                                        <asp:Button ID="btnpopupalert" Font-Bold="true" Font-Size="Medium" Font-Names="Book Antiqua"
                                            AutoPostBack="False" CssClass="textbox textbox1" Style="height: auto; width: auto;"
                                            OnClick="btnpopupalert_Click" Text="Ok" runat="server" />
                                    </center>
                                </td>
                            </tr>
                        </table>
                    </center>
                </div>
            </center>
        </div>
    </center>
    <center>
        <%--<asp:UpdateProgress ID="UpdateProgress3" runat="server" AssociatedUpdatePanelID="hi">
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
        <asp:ModalPopupExtender ID="ModalPopupExtender2" runat="server" TargetControlID="UpdateProgress3"
            PopupControlID="UpdateProgress3">
        </asp:ModalPopupExtender>--%>
    </center>
    <%--progressBar for Upgo--%>
    <%--<center>
        <asp:UpdateProgress ID="UpdateProgress14" runat="server" AssociatedUpdatePanelID="UpdatePanel3">
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
        <asp:ModalPopupExtender ID="ModalPopupExtender14" runat="server" TargetControlID="UpdateProgress14"
            PopupControlID="UpdateProgress14">
        </asp:ModalPopupExtender>
    </center>--%>
</asp:Content>

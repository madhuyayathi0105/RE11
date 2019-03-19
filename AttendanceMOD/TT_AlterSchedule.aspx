<%@ Page Title="" Language="C#" MasterPageFile="~/AttendanceMOD/AttendanceSubSiteMaster.master"
    AutoEventWireup="true" CodeFile="TT_AlterSchedule.aspx.cs" Inherits="ScheduleMOD_TT_AlterSchedule" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Assembly="FarPoint.Web.Spread" Namespace="FarPoint.Web.Spread" TagPrefix="FarPoint" %>
<%@ Register Src="~/Usercontrols/PrintMaster.ascx" TagName="PRINTPDF" TagPrefix="Insproplus" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <style type="text/css">
        .cursorptr
        {
            cursor: pointer;
        }
        .cursordflt
        {
            cursor: default;
        }
        .ModalPopupBG
        {
            background-color: #666699;
            filter: alpha(opacity=50);
            opacity: 0.7;
        }
        .HellowWorldPopup
        {
            min-width: 600px;
            min-height: 400px;
            background: white;
        }
        
        #clsbtn
        {
            height: 26px;
            width: 72px;
        }
    </style>
    <script type="text/javascript">
        function display() {
            document.getElementById('MainContent_norecordlbl').innerHTML = "";

        }
        function DisplayLoadingDiv() {
            document.getElementById("<%=divImageLoading.ClientID %>").style.display = "block";
        }
        function HideLoadingDiv() {
            document.getElementById("<%=divImageLoading.ClientID %>").style.display = "none";
        }

    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <center>
        <asp:Label ID="lblhead" runat="server" Text="Alternate Schedule Change" ForeColor="Green"
            CssClass="fontstyleheader" Style="margin: 0px; margin-bottom: 15px; margin-top: 15px;"></asp:Label>
    </center>
    <center>
        <table class="maintablestyle" style="width: 900px; margin: 0px; margin-bottom: 15px;
            margin-top: 15px; padding: 8px;" border="0">
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
                    <asp:DropDownList runat="server" ID="ddldegree" Height="25px" Width="67px" AutoPostBack="True"
                        OnSelectedIndexChanged="ddldegree_SelectedIndexChanged" Font-Bold="True" Font-Names="Book Antiqua"
                        Font-Size="Medium">
                    </asp:DropDownList>
                </td>
                <td>
                    <asp:Label runat="server" ID="lblbranch" Text="Branch" Font-Bold="True" Font-Names="Book Antiqua"
                        Font-Size="Medium"></asp:Label>
                </td>
                <td>
                    <asp:DropDownList runat="server" ID="ddlbranch" Font-Bold="True" Height="25px" Width="220px"
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
                        Font-Size="Medium" Height="25px" Width="46px" AutoPostBack="True" OnSelectedIndexChanged="ddlsec_SelectedIndexChanged">
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Label ID="lblFromdate" runat="server" Text="From Date" Font-Bold="True" Font-Names="Book Antiqua"
                        Font-Size="Medium"></asp:Label>
                </td>
                <td>
                    <asp:TextBox ID="txtFromDate" CssClass="txt textbox" runat="server" Width="75px"
                        OnTextChanged="txtFromDate_TextChanged" Font-Bold="True" Font-Names="Book Antiqua"
                        Font-Size="Medium"></asp:TextBox>
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
                    <asp:TextBox ID="txtToDate" CssClass="txt textbox" runat="server" Width="75px" Font-Bold="True"
                        Font-Names="Book Antiqua" Font-Size="Medium" OnTextChanged="txtToDate_TextChanged"></asp:TextBox>
                    <asp:FilteredTextBoxExtender ID="txtToDate_FilteredTextBoxExtender" runat="server"
                        TargetControlID="txtToDate" FilterType="Custom,Numbers" ValidChars="/">
                    </asp:FilteredTextBoxExtender>
                    <asp:CalendarExtender ID="caltodate" TargetControlID="txtToDate" Format="d/MM/yyyy"
                        runat="server">
                    </asp:CalendarExtender>
                </td>
                <td>
                    <asp:Label ID="lblnoofalter" runat="server" Text="No of Alter" Font-Bold="True" Font-Names="Book Antiqua"
                        Font-Size="Medium" />
                </td>
                <td>
                    <asp:TextBox ID="txtnoofalter" CssClass="txt textbox" runat="server" Width="45px"
                        Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium" MaxLength="1" Text="1"></asp:TextBox>
                    <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" runat="server" TargetControlID="txtnoofalter"
                        FilterType="Numbers">
                    </asp:FilteredTextBoxExtender>
                    <asp:Button ID="btnGo" runat="server" Text="Go" OnClick="btnGo_Click" Font-Bold="True"
                        Font-Names="Book Antiqua" Font-Size="Medium" CssClass="textbox btn1" />
                </td>
            </tr>
            <tr>
                <td colspan="10">
                    <asp:Label ID="frmlbl" runat="server" Text="Select From Date" ForeColor="Red" Font-Bold="True"
                        Font-Names="Book Antiqua"></asp:Label>
                    <asp:Label ID="tolbl" runat="server" Text="Select To Date" ForeColor="Red" Font-Bold="True"
                        Font-Names="Book Antiqua"></asp:Label>
                    <asp:Label ID="tofromlbl" runat="server" Text="From date should not be greater than To date"
                        ForeColor="Red" Font-Bold="True" Font-Names="Book Antiqua"></asp:Label>
                    <asp:Label ID="deglbl" runat="server" Text="Select degree" ForeColor="Red" Font-Bold="True"></asp:Label>
                    <asp:Label ID="branlbl" runat="server" Text="Select branch" ForeColor="Red" Font-Bold="True"></asp:Label>
                    <asp:Label ID="semlbl" runat="server" Text="Select semester" ForeColor="Red" Font-Bold="True"></asp:Label>
                    <asp:Label ID="seclbl" runat="server" Text="Select section" ForeColor="Red" Font-Bold="True"></asp:Label>
                </td>
            </tr>
        </table>
        <asp:UpdatePanel ID="UpdatePanel2" runat="server">
            <ContentTemplate>
                <asp:Panel ID="pnl" runat="server">
                    <div id="Div3" runat="server" visible="false" style="height: 200%; z-index: 1000;
                        width: 100%; background-color: rgba(54, 25, 25, .2); position: absolute; top: 0;
                        left: 0px;">
                        <center>
                            <div id="Div4" runat="server" class="table" style="background-color: White; height: 160px;
                                width: 530px; border: 5px solid #0CA6CA; border-top: 25px solid #0CA6CA; margin-top: 551px;
                                border-radius: 10px;">
                                <center>
                                    <table style="height: 100px; width: 100%">
                                        <tr>
                                            <td align="center">
                                                <asp:Label ID="Label12" runat="server" Text="" Style="color: Black;" Font-Bold="true"
                                                    Font-Size="Medium"></asp:Label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <center>
                                                    <asp:Button ID="btnOKsave" runat="server" Text="Ok" CssClass="btn1 textbox1 textbox "
                                                        Width="70px" OnClick="btnOKsave_Clik" />
                                                    <asp:Button ID="bt_closedalter" runat="server" Text="Cancel" CssClass="btn1 textbox1 textbox "
                                                        OnClientClick="return DisplayLoadingDiv();" OnClick="bt_closedalter_Clik" Width="80px" />
                                                </center>
                                            </td>
                                        </tr>
                                    </table>
                                </center>
                            </div>
                        </center>
                    </div>
                </asp:Panel>
            </ContentTemplate>
        </asp:UpdatePanel>
        <%-- Loading Image--%>
        <asp:UpdatePanel ID="UpdatePanel3" runat="server">
            <ContentTemplate>
                <asp:Panel ID="pnlLoadI" runat="server">
                    <center>
                        <div id="divImageLoading" runat="server" style="height: 300em; z-index: 100000; width: 100%;
                            background-color: rgba(54, 25, 25, .2); position: absolute; top: 30%; left: 0px;
                            display: none;">
                            <center>
                                <img src="../images/loader.gif" style="margin-top: 320px; height: 50px; border-radius: 10px;" />
                                <br />
                                <span style="font-family: Book Antiqua; font-size: Medium; font-weight: bold; color: Black;">
                                    Processing Please Wait...</span>
                            </center>
                        </div>
                    </center>
                </asp:Panel>
            </ContentTemplate>
        </asp:UpdatePanel>
        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
            <ContentTemplate>
                <asp:UpdateProgress ID="UpdateProgress1" runat="server" DynamicLayout="true" AssociatedUpdatePanelID="UpdatePanel1">
                    <ProgressTemplate>
                        <div class="CenterPB" style="height: 40px; width: 40px;">
                            <img src="../images/progress2.gif" height="180px" width="180px" />
                        </div>
                    </ProgressTemplate>
                </asp:UpdateProgress>
                <asp:ModalPopupExtender ID="modalPopup" runat="server" TargetControlID="UpdateProgress1"
                    PopupControlID="UpdateProgress1">
                </asp:ModalPopupExtender>
                <table>
                    <tr>
                        <td colspan="4" align="left">
                            <asp:Button ID="btn_remove" runat="server" Text="Remove" Font-Bold="True" Font-Names="Book Antiqua"
                                Font-Size="Medium" OnClick="btn_remove_Click" />
                        </td>
                    </tr>
                    <tr>
                        <td colspan="4" align="center">
                            <div style="margin-left: 150px;">
                                <FarPoint:FpSpread ID="SpdInfo" runat="server" BorderColor="Black" BorderStyle="Solid"
                                    BorderWidth="1px" Height="200" Width="400" OnPreRender="SpdInfo_SelectedIndexChanged"
                                    OnCellClick="SpdInfo_CellClick" CssClass="cursorptr">
                                    <CommandBar BackColor="Control" ButtonFaceColor="Control" ButtonHighlightColor="ControlLightLight"
                                        ButtonShadowColor="ControlDark">
                                    </CommandBar>
                                    <Sheets>
                                        <FarPoint:SheetView SheetName="Sheet1">
                                        </FarPoint:SheetView>
                                    </Sheets>
                                </FarPoint:FpSpread>
                            </div>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="4" align="left">
                            <asp:Label ID="lblcellerrmsg" runat="server" Font-Names="Book Antiqua" Font-Size="Medium"
                                Font-Bold="true" ForeColor="Red"></asp:Label>
                        </td>
                    </tr>
                </table>
                <table>
                    <tr>
                        <td>
                            <asp:Label ID="lblrptname" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                Font-Size="Medium" Text="Report Name"></asp:Label>
                            <asp:TextBox ID="txtexcelname" runat="server" Height="20px" onkeypress="display()"
                                Width="180px" Style="font-family: 'Book Antiqua'" Font-Bold="True" Font-Names="Book Antiqua"
                                Font-Size="Medium"></asp:TextBox>
                            <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender2" runat="server" TargetControlID="txtexcelname"
                                FilterType="LowercaseLetters,UppercaseLetters,Numbers,Custom" ValidChars="!@$%^&*()_+|\}{][':;?><,./">
                            </asp:FilteredTextBoxExtender>
                        </td>
                        <td>
                            <asp:Button ID="btnxl" runat="server" Text="Export Excel" Font-Bold="True" Font-Names="Book Antiqua"
                                Font-Size="Medium" OnClick="btnxl_Click" />
                        </td>
                        <td>
                            <asp:Button ID="btnprintmaster" runat="server" Text="Print" OnClick="btnprintmaster_Click"
                                Font-Names="Book Antiqua" Font-Size="Medium" Font-Bold="true" />
                            <Insproplus:PRINTPDF runat="server" ID="Printcontrol" Visible="false" />
                        </td>
                    </tr>
                </table>
                <%--End--%>
                <table>
                    <tr>
                        <td>
                            <asp:Label ID="norecordlbl" runat="server" Font-Bold="true" Font-Size="Medium" ForeColor="Red"></asp:Label>
                        </td>
                    </tr>
                    <asp:Panel ID="Panel3" runat="server">
                        <tr>
                            <td align="left">
                            </td>
                        </tr>
                        <tr>
                            <td class="style1">
                                <asp:Button ID="Button4" Visible="false" runat="server" Text="Free Staff List" Style="border-bottom-style: none;"
                                    Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium" />
                                <asp:ModalPopupExtender ID="ModalPopupExtender1" runat="server" TargetControlID="Button4"
                                    CancelControlID="Button1" PopupControlID="Panel1" PopupDragHandleControlID="PopupHeader"
                                    Drag="true" BackgroundCssClass="ModalPopupBG">
                                </asp:ModalPopupExtender>
                                <asp:Panel ID="Panel1" runat="server" BorderColor="Black" BorderStyle="Double" Style="display: none;
                                    height: 400; width: 700;">
                                    <div class="HellowWorldPopup">
                                        <div class="PopupHeader" id="Div2" style="text-align: center; color: Blue; font-family: Book Antiqua;
                                            font-size: xx-large; font-weight: bold">
                                            Free Staff List</div>
                                        <div class="PopupBody">
                                        </div>
                                        <div class="Controls">
                                            <center>
                                                <FarPoint:FpSpread ID="freestaff" runat="server" AllowSort="true" BorderColor="Black"
                                                    BorderStyle="Solid" BorderWidth="1px" CommandBar-Font-Names="Arial" DesignString="&lt;?xml version=&quot;1.0&quot; encoding=&quot;utf-8&quot;?&gt;&lt;Spread /&gt;"
                                                    Height="287px" Width="750" ScrollBar3DLightColor="Red" ScrollBarArrowColor="Aqua"
                                                    ScrollBarBaseColor="Goldenrod" ScrollBarDarkShadowColor="#FF8080" ScrollBarFaceColor="#99FF66"
                                                    scrollContent="true" scrollContentColumns="" scrollContentMaxHeight="50" scrollContentTime="500"
                                                    ActiveSheetViewIndex="0">
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
                                                        <FarPoint:SheetView AllowPage="False" DesignString="&lt;?xml version=&quot;1.0&quot; encoding=&quot;utf-8&quot;?&gt;&lt;Sheet&gt;&lt;Data&gt;&lt;RowHeader class=&quot;FarPoint.Web.Spread.Model.DefaultSheetDataModel&quot; rows=&quot;3&quot; columns=&quot;1&quot;&gt;&lt;AutoCalculation&gt;True&lt;/AutoCalculation&gt;&lt;AutoGenerateColumns&gt;True&lt;/AutoGenerateColumns&gt;&lt;ReferenceStyle&gt;A1&lt;/ReferenceStyle&gt;&lt;Iteration&gt;False&lt;/Iteration&gt;&lt;MaximumIterations&gt;1&lt;/MaximumIterations&gt;&lt;MaximumChange&gt;0.001&lt;/MaximumChange&gt;&lt;/RowHeader&gt;&lt;ColumnHeader class=&quot;FarPoint.Web.Spread.Model.DefaultSheetDataModel&quot; rows=&quot;1&quot; columns=&quot;4&quot;&gt;&lt;AutoCalculation&gt;True&lt;/AutoCalculation&gt;&lt;AutoGenerateColumns&gt;True&lt;/AutoGenerateColumns&gt;&lt;ReferenceStyle&gt;A1&lt;/ReferenceStyle&gt;&lt;Iteration&gt;False&lt;/Iteration&gt;&lt;MaximumIterations&gt;1&lt;/MaximumIterations&gt;&lt;MaximumChange&gt;0.001&lt;/MaximumChange&gt;&lt;/ColumnHeader&gt;&lt;DataArea class=&quot;FarPoint.Web.Spread.Model.DefaultSheetDataModel&quot; rows=&quot;3&quot; columns=&quot;4&quot;&gt;&lt;AutoCalculation&gt;True&lt;/AutoCalculation&gt;&lt;AutoGenerateColumns&gt;True&lt;/AutoGenerateColumns&gt;&lt;ReferenceStyle&gt;A1&lt;/ReferenceStyle&gt;&lt;Iteration&gt;False&lt;/Iteration&gt;&lt;MaximumIterations&gt;1&lt;/MaximumIterations&gt;&lt;MaximumChange&gt;0.001&lt;/MaximumChange&gt;&lt;SheetName&gt;Sheet1&lt;/SheetName&gt;&lt;/DataArea&gt;&lt;SheetCorner class=&quot;FarPoint.Web.Spread.Model.DefaultSheetDataModel&quot; rows=&quot;1&quot; columns=&quot;1&quot;&gt;&lt;AutoCalculation&gt;True&lt;/AutoCalculation&gt;&lt;AutoGenerateColumns&gt;True&lt;/AutoGenerateColumns&gt;&lt;ReferenceStyle&gt;A1&lt;/ReferenceStyle&gt;&lt;Iteration&gt;False&lt;/Iteration&gt;&lt;MaximumIterations&gt;1&lt;/MaximumIterations&gt;&lt;MaximumChange&gt;0.001&lt;/MaximumChange&gt;&lt;/SheetCorner&gt;&lt;ColumnFooter class=&quot;FarPoint.Web.Spread.Model.DefaultSheetDataModel&quot; rows=&quot;1&quot; columns=&quot;4&quot;&gt;&lt;AutoCalculation&gt;True&lt;/AutoCalculation&gt;&lt;AutoGenerateColumns&gt;True&lt;/AutoGenerateColumns&gt;&lt;ReferenceStyle&gt;A1&lt;/ReferenceStyle&gt;&lt;Iteration&gt;False&lt;/Iteration&gt;&lt;MaximumIterations&gt;1&lt;/MaximumIterations&gt;&lt;MaximumChange&gt;0.001&lt;/MaximumChange&gt;&lt;/ColumnFooter&gt;&lt;/Data&gt;&lt;Presentation&gt;&lt;ActiveSkin class=&quot;FarPoint.Web.Spread.SheetSkin&quot;&gt;&lt;Name&gt;Default&lt;/Name&gt;&lt;BackColor&gt;Empty&lt;/BackColor&gt;&lt;CellBackColor&gt;Empty&lt;/CellBackColor&gt;&lt;CellForeColor&gt;Empty&lt;/CellForeColor&gt;&lt;CellSpacing&gt;0&lt;/CellSpacing&gt;&lt;GridLines&gt;Both&lt;/GridLines&gt;&lt;GridLineColor&gt;#d0d7e5&lt;/GridLineColor&gt;&lt;HeaderBackColor&gt;Empty&lt;/HeaderBackColor&gt;&lt;HeaderForeColor&gt;Empty&lt;/HeaderForeColor&gt;&lt;FlatColumnHeader&gt;False&lt;/FlatColumnHeader&gt;&lt;FooterBackColor&gt;Empty&lt;/FooterBackColor&gt;&lt;FooterForeColor&gt;Empty&lt;/FooterForeColor&gt;&lt;FlatColumnFooter&gt;False&lt;/FlatColumnFooter&gt;&lt;FlatRowHeader&gt;False&lt;/FlatRowHeader&gt;&lt;HeaderFontBold&gt;False&lt;/HeaderFontBold&gt;&lt;FooterFontBold&gt;False&lt;/FooterFontBold&gt;&lt;SelectionBackColor&gt;#eaecf5&lt;/SelectionBackColor&gt;&lt;SelectionForeColor&gt;Empty&lt;/SelectionForeColor&gt;&lt;EvenRowBackColor&gt;Empty&lt;/EvenRowBackColor&gt;&lt;OddRowBackColor&gt;Empty&lt;/OddRowBackColor&gt;&lt;ShowColumnHeader&gt;True&lt;/ShowColumnHeader&gt;&lt;ShowColumnFooter&gt;False&lt;/ShowColumnFooter&gt;&lt;ShowRowHeader&gt;True&lt;/ShowRowHeader&gt;&lt;ColumnHeaderBackground class=&quot;FarPoint.Web.Spread.Background&quot;&gt;&lt;BackgroundImageUrl&gt;SPREADCLIENTPATH:/img/chbg.gif&lt;/BackgroundImageUrl&gt;&lt;SelectedBackgroundImageUrl&gt;SPREADCLIENTPATH:/img/chm.png&lt;/SelectedBackgroundImageUrl&gt;&lt;/ColumnHeaderBackground&gt;&lt;SheetCornerBackground class=&quot;FarPoint.Web.Spread.Background&quot;&gt;&lt;BackgroundImageUrl&gt;SPREADCLIENTPATH:/img/chbg.gif&lt;/BackgroundImageUrl&gt;&lt;SelectedBackgroundImageUrl&gt;SPREADCLIENTPATH:/img/chm.png&lt;/SelectedBackgroundImageUrl&gt;&lt;/SheetCornerBackground&gt;&lt;HeaderGrayAreaColor&gt;#7999c2&lt;/HeaderGrayAreaColor&gt;&lt;/ActiveSkin&gt;&lt;AxisModels&gt;&lt;Column class=&quot;FarPoint.Web.Spread.Model.DefaultSheetAxisModel&quot; orientation=&quot;Horizontal&quot; count=&quot;4&quot;&gt;&lt;Items&gt;&lt;Item index=&quot;-1&quot;&gt;&lt;SortIndicator&gt;Ascending&lt;/SortIndicator&gt;&lt;/Item&gt;&lt;Item index=&quot;0&quot;&gt;&lt;Size&gt;55&lt;/Size&gt;&lt;/Item&gt;&lt;Item index=&quot;1&quot;&gt;&lt;Size&gt;111&lt;/Size&gt;&lt;/Item&gt;&lt;Item index=&quot;2&quot;&gt;&lt;Size&gt;81&lt;/Size&gt;&lt;/Item&gt;&lt;Item index=&quot;3&quot;&gt;&lt;Size&gt;79&lt;/Size&gt;&lt;/Item&gt;&lt;/Items&gt;&lt;/Column&gt;&lt;RowHeaderColumn class=&quot;FarPoint.Web.Spread.Model.DefaultSheetAxisModel&quot; defaultSize=&quot;40&quot; orientation=&quot;Horizontal&quot; count=&quot;1&quot;&gt;&lt;Items&gt;&lt;Item index=&quot;-1&quot;&gt;&lt;SortIndicator&gt;Ascending&lt;/SortIndicator&gt;&lt;Size&gt;40&lt;/Size&gt;&lt;/Item&gt;&lt;/Items&gt;&lt;/RowHeaderColumn&gt;&lt;ColumnHeaderRow class=&quot;FarPoint.Web.Spread.Model.DefaultSheetAxisModel&quot; defaultSize=&quot;22&quot; orientation=&quot;Vertical&quot; count=&quot;1&quot;&gt;&lt;Items&gt;&lt;Item index=&quot;-1&quot;&gt;&lt;Size&gt;22&lt;/Size&gt;&lt;/Item&gt;&lt;/Items&gt;&lt;/ColumnHeaderRow&gt;&lt;ColumnFooterRow class=&quot;FarPoint.Web.Spread.Model.DefaultSheetAxisModel&quot; defaultSize=&quot;22&quot; orientation=&quot;Vertical&quot; count=&quot;1&quot;&gt;&lt;Items&gt;&lt;Item index=&quot;-1&quot;&gt;&lt;Size&gt;22&lt;/Size&gt;&lt;/Item&gt;&lt;/Items&gt;&lt;/ColumnFooterRow&gt;&lt;/AxisModels&gt;&lt;StyleModels&gt;&lt;RowHeader class=&quot;FarPoint.Web.Spread.Model.DefaultSheetStyleModel&quot; Rows=&quot;3&quot; Columns=&quot;1&quot;&gt;&lt;AltRowCount&gt;2&lt;/AltRowCount&gt;&lt;DefaultStyle class=&quot;FarPoint.Web.Spread.NamedStyle&quot; Parent=&quot;RowHeaderDefault&quot; /&gt;&lt;ConditionalFormatCollections /&gt;&lt;/RowHeader&gt;&lt;ColumnHeader class=&quot;FarPoint.Web.Spread.Model.DefaultSheetStyleModel&quot; Rows=&quot;1&quot; Columns=&quot;4&quot;&gt;&lt;AltRowCount&gt;2&lt;/AltRowCount&gt;&lt;DefaultStyle class=&quot;FarPoint.Web.Spread.NamedStyle&quot; Parent=&quot;ColumnHeaderDefault&quot;&gt;&lt;Background class=&quot;FarPoint.Web.Spread.Background&quot;&gt;&lt;BackgroundImageUrl&gt;SPREADCLIENTPATH:/img/chbg.gif&lt;/BackgroundImageUrl&gt;&lt;SelectedBackgroundImageUrl&gt;SPREADCLIENTPATH:/img/chm.png&lt;/SelectedBackgroundImageUrl&gt;&lt;/Background&gt;&lt;/DefaultStyle&gt;&lt;ConditionalFormatCollections /&gt;&lt;/ColumnHeader&gt;&lt;DataArea class=&quot;FarPoint.Web.Spread.Model.DefaultSheetStyleModel&quot; Rows=&quot;3&quot; Columns=&quot;4&quot;&gt;&lt;AltRowCount&gt;2&lt;/AltRowCount&gt;&lt;DefaultStyle class=&quot;FarPoint.Web.Spread.NamedStyle&quot; Parent=&quot;DataAreaDefault&quot; /&gt;&lt;ConditionalFormatCollections /&gt;&lt;/DataArea&gt;&lt;SheetCorner class=&quot;FarPoint.Web.Spread.Model.DefaultSheetStyleModel&quot; Rows=&quot;1&quot; Columns=&quot;1&quot;&gt;&lt;AltRowCount&gt;2&lt;/AltRowCount&gt;&lt;DefaultStyle class=&quot;FarPoint.Web.Spread.NamedStyle&quot; Parent=&quot;CornerDefault&quot;&gt;&lt;Border class=&quot;FarPoint.Web.Spread.Border&quot; Size=&quot;1&quot; Style=&quot;Solid&quot;&gt;&lt;Bottom Color=&quot;ControlDark&quot; /&gt;&lt;Left Size=&quot;0&quot; /&gt;&lt;Right Color=&quot;ControlDark&quot; /&gt;&lt;Top Size=&quot;0&quot; /&gt;&lt;/Border&gt;&lt;Background class=&quot;FarPoint.Web.Spread.Background&quot;&gt;&lt;BackgroundImageUrl&gt;SPREADCLIENTPATH:/img/chbg.gif&lt;/BackgroundImageUrl&gt;&lt;SelectedBackgroundImageUrl&gt;SPREADCLIENTPATH:/img/chm.png&lt;/SelectedBackgroundImageUrl&gt;&lt;/Background&gt;&lt;/DefaultStyle&gt;&lt;ConditionalFormatCollections /&gt;&lt;/SheetCorner&gt;&lt;ColumnFooter class=&quot;FarPoint.Web.Spread.Model.DefaultSheetStyleModel&quot; Rows=&quot;1&quot; Columns=&quot;4&quot;&gt;&lt;AltRowCount&gt;2&lt;/AltRowCount&gt;&lt;DefaultStyle class=&quot;FarPoint.Web.Spread.NamedStyle&quot; Parent=&quot;ColumnFooterDefault&quot; /&gt;&lt;ConditionalFormatCollections /&gt;&lt;/ColumnFooter&gt;&lt;/StyleModels&gt;&lt;MessageRowStyle class=&quot;FarPoint.Web.Spread.Appearance&quot;&gt;&lt;BackColor&gt;LightYellow&lt;/BackColor&gt;&lt;ForeColor&gt;Red&lt;/ForeColor&gt;&lt;/MessageRowStyle&gt;&lt;SheetCornerStyle class=&quot;FarPoint.Web.Spread.NamedStyle&quot; Parent=&quot;CornerDefault&quot;&gt;&lt;Border class=&quot;FarPoint.Web.Spread.Border&quot; Size=&quot;1&quot; Style=&quot;Solid&quot;&gt;&lt;Bottom Color=&quot;ControlDark&quot; /&gt;&lt;Left Size=&quot;0&quot; /&gt;&lt;Right Color=&quot;ControlDark&quot; /&gt;&lt;Top Size=&quot;0&quot; /&gt;&lt;/Border&gt;&lt;Background class=&quot;FarPoint.Web.Spread.Background&quot;&gt;&lt;BackgroundImageUrl&gt;SPREADCLIENTPATH:/img/chbg.gif&lt;/BackgroundImageUrl&gt;&lt;SelectedBackgroundImageUrl&gt;SPREADCLIENTPATH:/img/chm.png&lt;/SelectedBackgroundImageUrl&gt;&lt;/Background&gt;&lt;/SheetCornerStyle&gt;&lt;AllowLoadOnDemand&gt;false&lt;/AllowLoadOnDemand&gt;&lt;LoadRowIncrement &gt;10&lt;/LoadRowIncrement &gt;&lt;LoadInitRowCount &gt;30&lt;/LoadInitRowCount &gt;&lt;AllowVirtualScrollPaging&gt;false&lt;/AllowVirtualScrollPaging&gt;&lt;TopRow&gt;0&lt;/TopRow&gt;&lt;PreviewRowStyle class=&quot;FarPoint.Web.Spread.PreviewRowInfo&quot; /&gt;&lt;/Presentation&gt;&lt;Settings&gt;&lt;Name&gt;Sheet1&lt;/Name&gt;&lt;Categories&gt;&lt;Appearance&gt;&lt;GridLineColor&gt;#d0d7e5&lt;/GridLineColor&gt;&lt;SelectionBackColor&gt;#eaecf5&lt;/SelectionBackColor&gt;&lt;SelectionBorder class=&quot;FarPoint.Web.Spread.Border&quot; /&gt;&lt;/Appearance&gt;&lt;Behavior&gt;&lt;EditTemplateColumnCount&gt;2&lt;/EditTemplateColumnCount&gt;&lt;ScrollingContentVisible&gt;True&lt;/ScrollingContentVisible&gt;&lt;AllowSort&gt;True&lt;/AllowSort&gt;&lt;PageSize&gt;100&lt;/PageSize&gt;&lt;AllowPage&gt;False&lt;/AllowPage&gt;&lt;GroupBarText&gt;Drag a column to group by that column.&lt;/GroupBarText&gt;&lt;/Behavior&gt;&lt;Layout&gt;&lt;RowHeaderColumnCount&gt;1&lt;/RowHeaderColumnCount&gt;&lt;ColumnHeaderRowCount&gt;1&lt;/ColumnHeaderRowCount&gt;&lt;/Layout&gt;&lt;/Categories&gt;&lt;ActiveRow&gt;0&lt;/ActiveRow&gt;&lt;ActiveColumn&gt;0&lt;/ActiveColumn&gt;&lt;ColumnHeaderRowCount&gt;1&lt;/ColumnHeaderRowCount&gt;&lt;ColumnFooterRowCount&gt;1&lt;/ColumnFooterRowCount&gt;&lt;PrintInfo&gt;&lt;Header /&gt;&lt;Footer /&gt;&lt;ZoomFactor&gt;0&lt;/ZoomFactor&gt;&lt;FirstPageNumber&gt;1&lt;/FirstPageNumber&gt;&lt;Orientation&gt;Auto&lt;/Orientation&gt;&lt;PrintType&gt;All&lt;/PrintType&gt;&lt;PageOrder&gt;Auto&lt;/PageOrder&gt;&lt;BestFitCols&gt;False&lt;/BestFitCols&gt;&lt;BestFitRows&gt;False&lt;/BestFitRows&gt;&lt;PageStart&gt;-1&lt;/PageStart&gt;&lt;PageEnd&gt;-1&lt;/PageEnd&gt;&lt;ColStart&gt;-1&lt;/ColStart&gt;&lt;ColEnd&gt;-1&lt;/ColEnd&gt;&lt;RowStart&gt;-1&lt;/RowStart&gt;&lt;RowEnd&gt;-1&lt;/RowEnd&gt;&lt;ShowBorder&gt;True&lt;/ShowBorder&gt;&lt;ShowGrid&gt;True&lt;/ShowGrid&gt;&lt;ShowColor&gt;True&lt;/ShowColor&gt;&lt;ShowColumnHeader&gt;Inherit&lt;/ShowColumnHeader&gt;&lt;ShowRowHeader&gt;Inherit&lt;/ShowRowHeader&gt;&lt;ShowColumnFooter&gt;Inherit&lt;/ShowColumnFooter&gt;&lt;ShowColumnFooterEachPage&gt;True&lt;/ShowColumnFooterEachPage&gt;&lt;ShowTitle&gt;True&lt;/ShowTitle&gt;&lt;ShowSubtitle&gt;True&lt;/ShowSubtitle&gt;&lt;UseMax&gt;True&lt;/UseMax&gt;&lt;UseSmartPrint&gt;False&lt;/UseSmartPrint&gt;&lt;Opacity&gt;255&lt;/Opacity&gt;&lt;PrintNotes&gt;None&lt;/PrintNotes&gt;&lt;Centering&gt;None&lt;/Centering&gt;&lt;RepeatColStart&gt;-1&lt;/RepeatColStart&gt;&lt;RepeatColEnd&gt;-1&lt;/RepeatColEnd&gt;&lt;RepeatRowStart&gt;-1&lt;/RepeatRowStart&gt;&lt;RepeatRowEnd&gt;-1&lt;/RepeatRowEnd&gt;&lt;SmartPrintPagesTall&gt;1&lt;/SmartPrintPagesTall&gt;&lt;SmartPrintPagesWide&gt;1&lt;/SmartPrintPagesWide&gt;&lt;HeaderHeight&gt;-1&lt;/HeaderHeight&gt;&lt;FooterHeight&gt;-1&lt;/FooterHeight&gt;&lt;/PrintInfo&gt;&lt;TitleInfo class=&quot;FarPoint.Web.Spread.TitleInfo&quot;&gt;&lt;Style class=&quot;FarPoint.Web.Spread.StyleInfo&quot;&gt;&lt;BackColor&gt;#e7eff7&lt;/BackColor&gt;&lt;HorizontalAlign&gt;Right&lt;/HorizontalAlign&gt;&lt;/Style&gt;&lt;Value type=&quot;System.String&quot; whitespace=&quot;&quot; /&gt;&lt;/TitleInfo&gt;&lt;LayoutTemplate class=&quot;FarPoint.Web.Spread.LayoutTemplate&quot;&gt;&lt;Layout&gt;&lt;ColumnCount&gt;4&lt;/ColumnCount&gt;&lt;RowCount&gt;1&lt;/RowCount&gt;&lt;/Layout&gt;&lt;Data&gt;&lt;LayoutData class=&quot;FarPoint.Web.Spread.Model.DefaultSheetDataModel&quot; rows=&quot;1&quot; columns=&quot;4&quot;&gt;&lt;AutoCalculation&gt;True&lt;/AutoCalculation&gt;&lt;AutoGenerateColumns&gt;True&lt;/AutoGenerateColumns&gt;&lt;ReferenceStyle&gt;A1&lt;/ReferenceStyle&gt;&lt;Iteration&gt;False&lt;/Iteration&gt;&lt;MaximumIterations&gt;1&lt;/MaximumIterations&gt;&lt;MaximumChange&gt;0.001&lt;/MaximumChange&gt;&lt;Cells&gt;&lt;Cell row=&quot;0&quot; column=&quot;0&quot;&gt;&lt;Data type=&quot;System.Int32&quot;&gt;0&lt;/Data&gt;&lt;/Cell&gt;&lt;Cell row=&quot;0&quot; column=&quot;1&quot;&gt;&lt;Data type=&quot;System.Int32&quot;&gt;1&lt;/Data&gt;&lt;/Cell&gt;&lt;Cell row=&quot;0&quot; column=&quot;2&quot;&gt;&lt;Data type=&quot;System.Int32&quot;&gt;2&lt;/Data&gt;&lt;/Cell&gt;&lt;Cell row=&quot;0&quot; column=&quot;3&quot;&gt;&lt;Data type=&quot;System.Int32&quot;&gt;3&lt;/Data&gt;&lt;/Cell&gt;&lt;/Cells&gt;&lt;/LayoutData&gt;&lt;/Data&gt;&lt;AxisModels&gt;&lt;LayoutColumn class=&quot;FarPoint.Web.Spread.Model.DefaultSheetAxisModel&quot; orientation=&quot;Horizontal&quot; count=&quot;4&quot;&gt;&lt;Items&gt;&lt;Item index=&quot;-1&quot;&gt;&lt;SortIndicator&gt;Ascending&lt;/SortIndicator&gt;&lt;/Item&gt;&lt;/Items&gt;&lt;/LayoutColumn&gt;&lt;LayoutRow class=&quot;FarPoint.Web.Spread.Model.DefaultSheetAxisModel&quot; orientation=&quot;Vertical&quot; count=&quot;1&quot;&gt;&lt;Items&gt;&lt;Item index=&quot;-1&quot; /&gt;&lt;/Items&gt;&lt;/LayoutRow&gt;&lt;/AxisModels&gt;&lt;/LayoutTemplate&gt;&lt;LayoutMode&gt;CellLayoutMode&lt;/LayoutMode&gt;&lt;CurrentPageIndex type=&quot;System.Int32&quot;&gt;0&lt;/CurrentPageIndex&gt;&lt;/Settings&gt;&lt;/Sheet&gt;"
                                                            PageSize="100" SheetName="Sheet1" AllowSort="True">
                                                        </FarPoint:SheetView>
                                                    </Sheets>
                                                    <TitleInfo BackColor="#E7EFF7" Font-Bold="False" Font-Italic="False" Font-Overline="False"
                                                        Font-Size="X-Large" Font-Strikeout="False" Font-Underline="False" ForeColor=""
                                                        HorizontalAlign="Center" Text="" VerticalAlign="NotSet">
                                                    </TitleInfo>
                                                </FarPoint:FpSpread>
                                            </center>
                                            <br />
                                            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                            <asp:Button ID="Button1" runat="server" Text="Exit" />
                                        </div>
                                    </div>
                                </asp:Panel>
                                <%---------------------------------semester schedule------------------------------------------%>
                                <asp:Button ID="sem_schedule" runat="server" Text="As per day schedule" Style="border-bottom-style: none;"
                                    Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium" ToolTip="Select any cell from Alternate schedule column" />
                                <asp:ModalPopupExtender ID="ModalPopupExtender2" runat="server" TargetControlID="sem_schedule"
                                    CancelControlID="Button6" PopupControlID="Panel2" PopupDragHandleControlID="PopupHeader"
                                    Drag="true" BackgroundCssClass="ModalPopupBG">
                                </asp:ModalPopupExtender>
                                <asp:Panel ID="Panel2" runat="server" BorderColor="Black" BorderStyle="Double" Style="display: none;
                                    height: 250; width: 700;">
                                    <div class="HellowWorldPopup">
                                        <div class="PopupHeader" id="Div1" style="text-align: center; color: Blue; font-family: Book Antiqua;
                                            font-size: xx-large; font-weight: bold">
                                            Semester schedule</div>
                                        <div class="PopupBody">
                                        </div>
                                        <div class="Controls">
                                            <br />
                                            <br />
                                            <FarPoint:FpSpread ID="semspread" runat="server" Height="263px" Width="901px" OnCellClick="semspread_CellClick"
                                                ActiveSheetViewIndex="0" currentPageIndex="0" DesignString="&lt;?xml version=&quot;1.0&quot; encoding=&quot;utf-8&quot;?&gt;&lt;Spread /&gt;"
                                                EnableClientScript="False" BorderStyle="Double" BorderWidth="2px">
                                                <CommandBar BackColor="Control">
                                                    <Background BackgroundImageUrl="SPREADCLIENTPATH:/img/cbbg.gif" />
                                                </CommandBar>
                                                <Pager Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                                    Font-Underline="False" />
                                                <HierBar Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                                    Font-Underline="False" />
                                                <Sheets>
                                                    <FarPoint:SheetView SheetName="Sheet1" EditTemplateColumnCount="2" GridLineColor="#DEDFDE"
                                                        GroupBarText="Drag a column to group by that column." SelectionBackColor="#CE5D5A"
                                                        SelectionForeColor="White">
                                                    </FarPoint:SheetView>
                                                </Sheets>
                                                <TitleInfo BackColor="#E7EFF7" Font-Size="X-Large" ForeColor="" HorizontalAlign="Center"
                                                    VerticalAlign="NotSet" Font-Bold="False" Font-Italic="False" Font-Overline="False"
                                                    Font-Strikeout="False" Font-Underline="False">
                                                </TitleInfo>
                                            </FarPoint:FpSpread>
                                            <center>
                                                <asp:Label ID="semmsglbl" runat="server" Text="Click Alternate Schedule Column" ForeColor="Red"
                                                    Font-Size="Larger"></asp:Label>
                                            </center>
                                            <br />
                                            <br />
                                            <br />
                                            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                            <asp:Button ID="Button6" runat="server" Text="Exit" />
                                        </div>
                                    </div>
                                </asp:Panel>
                                <%------------------------------------------------------------------------------------%>
                                <br />
                                <br />
                            </td>
                        </tr>
                    </asp:Panel>
                    <tr>
                        <td>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Button ID="batchbtn" runat="server" Font-Bold="True" BorderStyle="None" Font-Names="Book Antiqua"
                                Font-Size="Medium" Text="Batch Allocation" BackColor="White" CssClass="cursorptr"
                                ForeColor="Blue" Font-Underline="true" OnClick="batchbtn_Click" />
                            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                            <asp:Button ID="btnsave" runat="server" Text="Save" Font-Bold="True" Font-Names="Book Antiqua"
                                Font-Size="Medium" Height="25px" Width="75px" OnClick="btnsave_Click" />
                        </td>
                    </tr>
                </table>
                <center>
                    <asp:Label ID="errmsg" runat="server" Font-Bold="true" Font-Size="Medium" ForeColor="Red"></asp:Label>
                </center>
                <asp:Panel ID="treepanel" runat="server" BorderStyle="Dotted" BorderColor="ActiveBorder">
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
                            <td style="height: 400px; width: 50px; margin: 0px;">
                            </td>
                            <td style="width: 500px; height: 400px; margin: 0px;" align="right">
                                <table>
                                    <tr>
                                        <td colspan="3">
                                            <asp:Label ID="lblmulstaff" runat="server" Text="For Mulitple Staff Selection Only"
                                                Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium">
                                            </asp:Label>
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
                                            <asp:Button ID="btnmulstaff" runat="server" Text="Ok" OnClick="btnmulstaff_Click"
                                                Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="3">
                                            <FarPoint:FpSpread ID="FpSpread1" runat="server" Height="222px" Width="565px" ActiveSheetViewIndex="0"
                                                currentPageIndex="0" OnButtonCommand="FpSpread1_ButtonCommand" OnPreRender="FpSpread1_SelectedIndexChanged"
                                                OnCellClick="FpSpread1_CellClick">
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
                                                        SelectionForeColor="White">
                                                    </FarPoint:SheetView>
                                                </Sheets>
                                                <TitleInfo BackColor="#E7EFF7" ForeColor="" HorizontalAlign="Center" VerticalAlign="NotSet"
                                                    Font-Size="X-Large" Font-Bold="False" Font-Italic="False" Font-Overline="False"
                                                    Font-Strikeout="False" Font-Underline="False">
                                                </TitleInfo>
                                            </FarPoint:FpSpread>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td>
                            </td>
                            <td colspan="2" align="center">
                                <asp:CheckBox ID="chkappend" runat="server" Text="Append to the schedule List" Font-Bold="True"
                                    Font-Names="Book Antiqua" Font-Size="Medium" ForeColor="blue" />
                                <asp:CheckBox ID="chk_multisubj" runat="server" OnCheckedChanged="chk_multisubj_CheckedChanged"
                                    Text="Multiple Staffs" Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium"
                                    ForeColor="blue" AutoPostBack="true" />
                                <asp:TextBox ID="txtmultisubj" runat="server" Height="21px" Width="99px" Font-Bold="True"
                                    Font-Names="Book Antiqua" Font-Size="Medium"></asp:TextBox>
                                <asp:Panel ID="pnlmultisubj" runat="server" Direction="LeftToRight" ScrollBars="Auto"
                                    BorderColor="Black" BorderStyle="Inset" Height="19px" Width="175px" Style="top: 1250px;
                                    left: 470px; position: absolute">
                                    <asp:CheckBoxList ID="chklistmultisubj" runat="server" CssClass="font" Font-Bold="True"
                                        OnSelectedIndexChanged="chklistmultisubj_selectedindetxchange" AutoPostBack="true"
                                        Font-Names="Book Antiqua" Font-Size="Medium">
                                    </asp:CheckBoxList>
                                </asp:Panel>
                                <asp:DropDownExtender ID="ddextmultisubj" runat="server" TargetControlID="txtmultisubj"
                                    DropDownControlID="pnlmultisubj" DynamicServicePath="" Enabled="true">
                                </asp:DropDownExtender>
                                <asp:Button ID="btnOk" runat="server" Text="Ok" Font-Bold="True" Font-Names="Book Antiqua"
                                    Font-Size="Medium" Height="25px" Width="75px" BackColor="ControlLight" OnClick="btnOk_Click" />
                            </td>
                        </tr>
                        <tr>
                            <td>
                            </td>
                            <td>
                            </td>
                        </tr>
                        <tr>
                            <td>
                            </td>
                            <td>
                            </td>
                        </tr>
                        <tr>
                            <td>
                            </td>
                            <td>
                            </td>
                        </tr>
                    </table>
                </asp:Panel>
            </ContentTemplate>
            <Triggers>
                <asp:PostBackTrigger ControlID="btnxl" />
            </Triggers>
        </asp:UpdatePanel>
    </center>
</asp:Content>

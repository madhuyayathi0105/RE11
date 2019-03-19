<%@ Page Title="" Language="C#" MasterPageFile="~/AttendanceMOD/AttendanceSubSiteMaster.master"
    AutoEventWireup="true" CodeFile="Day_Wise_Absentees_sms.aspx.cs" Inherits="Day_Wise_Absentees_sm" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Assembly="FarPoint.Web.Spread" Namespace="FarPoint.Web.Spread" TagPrefix="FarPoint" %>
<%@ Register Src="~/Usercontrols/PrintMaster.ascx" TagName="printmaster" TagPrefix="Insproplus" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">
    <script type="text/javascript">
        function display() {
            document.getElementById('MainContent_lbl_err').innerHTML = "";
        }
    </script>
    <style type="text/css">
        .head
        {
            background-color: Teal;
            font-family: Book Antiqua;
            font-size: medium;
            color: black;
            top: 165px;
            position: absolute;
            font-weight: bold;
            width: 980px;
            height: 25px;
            left: 15px;
        }
        .mainbatch
        {
            background-color: #3AAB97;
            width: 980px;
            position: absolute;
            height: 80px;
            top: 190px;
            left: 15px;
            font-family: Book Antiqua;
            font-size: medium;
            font-weight: bold;
            color: black;
        }
        .cpBody
        {
            background-color: #DCE4F9;
            font: normal 11px auto Verdana, Arial;
            border: 1px gray;
            padding-top: 7px;
            padding-left: 4px;
            padding-right: 4px;
            padding-bottom: 4px;
        }
    </style>
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <br />
    <center>
        <span class="fontstyleheader" style="color: Green;">AT-32 Day Wise Abseentees SMS</span></center>
    <br />
    <body>
        <asp:UpdatePanel ID="UpdatePanel2" runat="server">
            <ContentTemplate>
                <center>
                    <table class="maintablestyle">
                        <tr>
                            <td>
                                <asp:Label ID="Iblbatch" Font-Bold="true" Font-Size="Medium" ForeColor="Black" Font-Names="Book Antiqua"
                                    runat="server" Text="Batch"></asp:Label>
                            </td>
                            <td>
                                <div style="position: relative;">
                                    <asp:TextBox ID="txt_batch" CssClass="Dropdown_Txt_Box" Font-Size="Medium" Font-Names="Book Antiqua"
                                        Font-Bold="true" Width="100px" runat="server" ReadOnly="true">--Select--</asp:TextBox>
                                    <asp:Panel ID="pbatch" runat="server" Font-Names="Book Antiqua" Font-Bold="true"
                                        CssClass="multxtpanel" Width="114px" Font-Size="Medium" BackColor="White" ScrollBars="Auto"
                                        Style="font-family: 'Book Antiqua'">
                                        <asp:CheckBox ID="Chk_batch" Font-Bold="true" runat="server" Font-Size="Medium" Text="Select All"
                                            AutoPostBack="True" Font-Names="Book Antiqua" OnCheckedChanged="Chlk_batchchanged" />
                                        <asp:CheckBoxList ID="Chklst_batch" Font-Bold="true" Font-Size="Medium" runat="server"
                                            AutoPostBack="True" Font-Names="Book Antiqua" OnSelectedIndexChanged="Chlk_batchselected">
                                        </asp:CheckBoxList>
                                    </asp:Panel>
                                    <asp:PopupControlExtender ID="popupbatch" runat="server" TargetControlID="txt_batch"
                                        PopupControlID="pbatch" Position="Bottom">
                                    </asp:PopupControlExtender>
                                </div>
                            </td>
                            <td>
                                <asp:Label ID="Ibldegree" runat="server" Font-Names="Book Antiqua" Font-Bold="true"
                                    ForeColor="Black" Font-Size="Medium" Text="Degree"></asp:Label>
                            </td>
                            <td>
                                <div style="position: relative;">
                                    <asp:TextBox ID="txt_degree" CssClass="Dropdown_Txt_Box" Font-Names="Book Antiqua"
                                        Font-Bold="true" runat="server" ReadOnly="true" Width="100px">--Select--</asp:TextBox>
                                    <asp:Panel ID="pdegree" runat="server" CssClass="multxtpanel" Width="128px" Height="200px"
                                        Font-Bold="true" Font-Size="Medium" BackColor="White" ScrollBars="Auto" Style="font-family: 'Book Antiqua'">
                                        <asp:CheckBox ID="chk_degree" Font-Bold="true" runat="server" Font-Size="Medium"
                                            Text="Select All" AutoPostBack="True" Font-Names="Book Antiqua" OnCheckedChanged="checkDegree_CheckedChanged" />
                                        <asp:CheckBoxList ID="Chklst_degree" Font-Bold="true" Font-Size="Medium" runat="server"
                                            AutoPostBack="True" Font-Names="Book Antiqua" OnSelectedIndexChanged="cheklist_Degree_SelectedIndexChanged">
                                        </asp:CheckBoxList>
                                    </asp:Panel>
                                    <asp:PopupControlExtender ID="popupdegree" runat="server" TargetControlID="txt_degree"
                                        PopupControlID="pdegree" Position="Bottom">
                                    </asp:PopupControlExtender>
                                </div>
                            </td>
                            <td>
                                <asp:Label ID="Iblbranch" runat="server" Font-Names="Book Antiqua" Font-Bold="true"
                                    ForeColor="Black" Font-Size="Medium" Text="Branch"></asp:Label>
                            </td>
                            <td>
                                <div style="position: relative;">
                                    <asp:TextBox ID="txt_branch" CssClass="Dropdown_Txt_Box" Font-Bold="true" Font-Names="Book Antiqua"
                                        runat="server" ReadOnly="true" Width="100px">--Select--</asp:TextBox>
                                    <asp:Panel ID="Panel3" runat="server" CssClass="multxtpanel" Width="400px" Height="250px"
                                        BackColor="White" ScrollBars="Auto" Style="font-family: 'Book Antiqua'">
                                        <asp:CheckBox ID="chk_branch" runat="server" Font-Bold="true" Font-Size="Medium"
                                            Font-Names="Book Antiqua" Text="Select All" OnCheckedChanged="chk_branchchanged"
                                            AutoPostBack="True" />
                                        <asp:CheckBoxList ID="chklst_branch" Font-Names="Book Antiqua" Font-Bold="true" Font-Size="Medium"
                                            runat="server" OnSelectedIndexChanged="chklst_branchselected" AutoPostBack="True">
                                        </asp:CheckBoxList>
                                    </asp:Panel>
                                    <asp:PopupControlExtender ID="popupbranch" runat="server" TargetControlID="txt_branch"
                                        PopupControlID="Panel3" Position="Bottom">
                                    </asp:PopupControlExtender>
                                </div>
                            </td>
                            <td>
                                <asp:Label ID="lblfromdate" runat="server" Font-Names="Book Antiqua" Font-Bold="true"
                                    ForeColor="Black" Font-Size="Medium" Text="From"></asp:Label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtfromdate" runat="server" Font-Names="Book Antiqua" Font-Bold="true"
                                    Font-Size="Medium" Width="80px" AutoPostBack="true" OnTextChanged="txtfromdate_TextChanged" />
                                <asp:CalendarExtender ID="CalendarExtender1" TargetControlID="txtfromdate" runat="server"
                                    Format="dd/MM/yyyy">
                                </asp:CalendarExtender>
                            </td>
                            <td>
                                <asp:Label ID="lbltodate" runat="server" Text="To" font-name="Book Antiqua" Font-Size="Medium"
                                    Font-Bold="true" ForeColor="Black"></asp:Label>
                            </td>
                            <td>
                                <asp:TextBox ID="txttodate" runat="server" Font-Names="Book Antiqua" Font-Bold="true"
                                    Font-Size="Medium" Width="80px" AutoPostBack="true" OnTextChanged="txttodate_TextChanged" />
                                <asp:CalendarExtender ID="CalendarExtender2" TargetControlID="txttodate" runat="server"
                                    Format="dd/MM/yyyy">
                                </asp:CalendarExtender>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="lblfrom" runat="server" Text="Absent Days From" Font-Bold="True" Font-Names="Book Antiqua"
                                    Font-Size="Medium" ForeColor="Black"></asp:Label>
                            </td>
                            <td colspan="9">
                                <asp:TextBox ID="txtfromrange" runat="server" Enabled="false" Font-Bold="True" Font-Names="Book Antiqua"
                                    Font-Size="Medium" AutoPostBack="true" OnTextChanged="txtfromrange_TextChanged"
                                    MaxLength="5" Width="40px"></asp:TextBox>
                                <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" runat="server" TargetControlID="txtfromrange"
                                    FilterType="Numbers,Custom" ValidChars="." />
                                <asp:Label ID="Label1" runat="server" Text="From" Font-Bold="True" Font-Names="Book Antiqua"
                                    Font-Size="Medium" ForeColor="Black"></asp:Label>
                                <asp:TextBox ID="TextBox1" runat="server" Enabled="false" Font-Bold="True" Font-Names="Book Antiqua"
                                    Font-Size="Medium" MaxLength="5" Width="40px"></asp:TextBox>
                                <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender2" runat="server" TargetControlID="TextBox1"
                                    FilterType="Numbers,Custom" ValidChars="." />
                                <asp:Label ID="Label2" runat="server" Text="To" Font-Bold="True" Font-Names="Book Antiqua"
                                    Font-Size="Medium" ForeColor="Black"></asp:Label>
                                <asp:TextBox ID="TextBox2" runat="server" Enabled="false" Font-Bold="True" Font-Names="Book Antiqua"
                                    Font-Size="Medium" MaxLength="5" Width="40px"></asp:TextBox>
                                <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender3" runat="server" TargetControlID="TextBox2"
                                    FilterType="Numbers,Custom" ValidChars="." />
                                <asp:Button ID="btngo" runat="server" Font-Names="Book Antiqua" Text="Go" OnClick="btngo_Click"
                                    Font-Size="Medium" Font-Bold="true" />
                                <asp:RadioButton ID="rb1" runat="server" Text="Absent Count" Font-Bold="True" Font-Names="Book Antiqua"
                                    Font-Size="Medium" GroupName="rpt" AutoPostBack="true" OnCheckedChanged="rb1_checked" />
                                <asp:RadioButton ID="rb2" runat="server" Text="Percentage" Font-Bold="True"
                                    Font-Names="Book Antiqua" Font-Size="Medium" GroupName="rpt" AutoPostBack="true" OnCheckedChanged="rb2_checked" />
                            </td>
                        </tr>
                    </table>
                </center>
                <br />
                <center>
                    <asp:Panel ID="pheaderfilter" runat="server" CssClass="cpHeader" BackColor="#719DDB"
                        Width="900px">
                        <asp:Label ID="Labelfilter" Text="Column Order" runat="server" Font-Size="Medium"
                            Font-Bold="True" Font-Names="Book Antiqua" />
                        <asp:Image ID="Imagefilter" runat="server" CssClass="cpimage" ImageUrl="../images/right.jpeg"
                            ImageAlign="Right" />
                    </asp:Panel>
                    <asp:Panel ID="pbodyfilter" runat="server" CssClass="cpBody" Width="900px">
                        <asp:CheckBoxList ID="chklscolumn" runat="server" Font-Size="Medium" AutoPostBack="True"
                            OnSelectedIndexChanged="chklscolumn_SelectedIndexChanged" Font-Bold="True" RepeatColumns="5"
                            RepeatDirection="Horizontal" Font-Names="Book Antiqua">
                            <asp:ListItem Text="Degree Details"></asp:ListItem>
                            <asp:ListItem Text="Conducted Days"></asp:ListItem>
                            <asp:ListItem Text="Present Days"></asp:ListItem>
                            <asp:ListItem Text="Absent Days"></asp:ListItem>
                            <asp:ListItem Text="Attendance Precentage"></asp:ListItem>
                            <asp:ListItem Text="Conducted Periods"></asp:ListItem>
                            <asp:ListItem Text="Attend Periods"></asp:ListItem>
                            <asp:ListItem Text="Absent Periods"></asp:ListItem>
                            <asp:ListItem Text="Attendance Precentage"></asp:ListItem>
                            <asp:ListItem Text="Student Mobile"></asp:ListItem>
                            <asp:ListItem Text="Student Email"></asp:ListItem>
                            <asp:ListItem Text="Father Mobile"></asp:ListItem>
                            <asp:ListItem Text="Mother Mobile"></asp:ListItem>
                            <asp:ListItem Text="Fine Amount"></asp:ListItem>
                            <asp:ListItem Text="Select"></asp:ListItem>
                            <asp:ListItem Text="Remarks"></asp:ListItem>
                        </asp:CheckBoxList>
                    </asp:Panel>
                    <asp:CollapsiblePanelExtender ID="cpecolumnorder" runat="server" TargetControlID="pbodyfilter"
                        CollapseControlID="pheaderfilter" ExpandControlID="pheaderfilter" Collapsed="true"
                        TextLabelID="Labelfilter" CollapsedSize="0" ImageControlID="Imagefilter" CollapsedImage="../images/right.jpeg"
                        ExpandedImage="../images/down.jpeg">
                    </asp:CollapsiblePanelExtender>
                    <br />
                    <asp:Label ID="lbl_err" runat="server" Text="" ForeColor="Red" Font-Bold="true" Font-Size="Medium"
                        Font-Names="Book Antiqua"></asp:Label>
                    <br />
                </center>
                <FarPoint:FpSpread ID="FpSpread1" runat="server" Height="250px" Width="400px" ActiveSheetViewIndex="0"
                    currentPageIndex="0" DesignString="&lt;?xml version=&quot;1.0&quot; encoding=&quot;utf-8&quot;?&gt;&lt;Spread /&gt;"
                    EnableClientScript="False" CssClass="cursorptr" BorderColor="Black" BorderWidth="0.5"
                    OnUpdateCommand="FpSpread1_UpdateCommand" ShowHeaderSelection="false">
                    <Sheets>
                        <FarPoint:SheetView SheetName="Sheet1">
                        </FarPoint:SheetView>
                    </Sheets>
                </FarPoint:FpSpread>
                <br />
                <center>
                    <asp:Label ID="lblrptname" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                        Font-Size="Medium" Text="Report Name"></asp:Label>
                    <asp:TextBox ID="txtexcelname" runat="server" Height="20px" Width="180px" Style="font-family: 'Book Antiqua'"
                        Font-Bold="True" onkeypress="display()" Font-Names="Book Antiqua" Font-Size="Medium"></asp:TextBox>
                    <asp:FilteredTextBoxExtender ID="Filterspace" runat="server" TargetControlID="txtexcelname"
                        FilterType="LowercaseLetters,UppercaseLetters,Numbers,Custom" ValidChars="()}{][ ,/">
                    </asp:FilteredTextBoxExtender>
                    <asp:Button ID="btnxl" runat="server" Text="Export Excel" Font-Bold="True" Font-Names="Book Antiqua"
                        Font-Size="Medium" OnClick="btnxl_Click" />
                    <asp:Button ID="btnmasterprint" runat="server" Text="Print" Font-Bold="True" Font-Names="Book Antiqua"
                        Font-Size="Medium" OnClick="btnmasterprint_Click" />
                    <Insproplus:printmaster runat="server" ID="Printcontrol" Visible="false" />
                    <br />
                    <table>
                        <tr>
                            <td>
                                <asp:Label ID="lblmsg" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                    Font-Size="Medium" Text="Message"></asp:Label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtmsg" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                    Font-Size="Medium" MaxLength="400" TextMode="MultiLine" Width="585px" Height="200px"
                                    placeholder="Roll No=$ROLLNO$, Regno=$REGNO$, Name=$NAME$, BATCH=$BATCH$, DEGREE=$DEGREE$, DEPARTMENT=$DEPT$, SEMESTER=$SEM$, SECTION=$SEC$, FROM DATE=$FDATE$, TODATE=$TDATE$, CONDUCTED DAYS=$CDAY$, ABSENT DAYS=$ADAY$, DAY PRECENTAGE=$DAYPER$, PRESENT PERIODS=$PHOUR$, CONDUCTED PERIODS=$CHOUR$, ABSENT PERIODS=$AHOUR$, HOUR PRECENTAGE=$HOURPER, FINE AMOUNT=$FINE$"></asp:TextBox>
                            </td>
                            <td>
                                <asp:Button ID="btnmsg" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                    Font-Size="Medium" Text="Send" OnClick="btnmsg_Click" />
                        </tr>
                    </table>
                </center>
            </ContentTemplate>
            <Triggers>
                <asp:PostBackTrigger ControlID="btnmasterprint" />
                <asp:PostBackTrigger ControlID="btnxl" />
                <asp:PostBackTrigger ControlID="btngo" />
            </Triggers>
        </asp:UpdatePanel>
    </body>
</asp:Content>

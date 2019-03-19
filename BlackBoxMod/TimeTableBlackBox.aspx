<%@ Page Title="" Language="C#" MasterPageFile="~/BlackBoxMod/BlackBoxSubSiteMaster.master" AutoEventWireup="true"
    CodeFile="TimeTableBlackBox.aspx.cs" Inherits="TimeTableBlackBox" %>

<%@ Register Assembly="FarPoint.Web.Spread" Namespace="FarPoint.Web.Spread" TagPrefix="FarPoint" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Src="~/Usercontrols/PrintMaster.ascx" TagName="PRINTPDF" TagPrefix="Insproplus" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <link href="~/Styles/css/Commoncss.css" rel="stylesheet" type="text/css" />
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
            position: absolute;
            font-weight: bold;
            width: 950px;
            height: 25px;
            left: 15px;
        }
        .mainbatch
        {
            background-color: #3AAB97;
            width: 950px;
            position: absolute;
            height: 114px;
            top: 90px;
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
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <div>
        <br />
        <center>
            <asp:Label ID="lbl_head" runat="server" CssClass="fontstyleheader" ForeColor="Green"
                Text="Black Box 2"></asp:Label>
        </center>
        <br />
    </div>
    <body>
        <asp:UpdatePanel ID="UpdatePanel2" runat="server">
            <ContentTemplate>
                <center>
                    <div class="maintablestyle">
                        <table width="950px">
                            <tr>
                                <td>
                                    <asp:Label ID="Iblbatch" Font-Bold="true" Font-Size="Medium" ForeColor="white" Font-Names="Book Antiqua"
                                        runat="server" Text="Batch"></asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txt_batch" CssClass="Dropdown_Txt_Box" Font-Size="Medium" Font-Names="Book Antiqua"
                                        Font-Bold="true" Width="100px" runat="server" ReadOnly="true">--Select--</asp:TextBox>
                                    <asp:Panel ID="pbatch" runat="server" CssClass="multxtpanel" BackColor="White" BorderColor="Black"
                                        BorderStyle="Solid" Height="200" Width="205" ScrollBars="Auto" Style="">
                                        <asp:CheckBox ID="Chk_batch" Font-Bold="true" runat="server" Font-Size="Medium" Text="Select All"
                                            AutoPostBack="True" Font-Names="Book Antiqua" OnCheckedChanged="Chlk_batchchanged" />
                                        <asp:CheckBoxList ID="Chklst_batch" Font-Bold="true" Font-Size="Medium" runat="server"
                                            AutoPostBack="True" Font-Names="Book Antiqua" OnSelectedIndexChanged="Chlk_batchselected">
                                        </asp:CheckBoxList>
                                    </asp:Panel>
                                    <asp:PopupControlExtender ID="popupbatch" runat="server" TargetControlID="txt_batch"
                                        PopupControlID="pbatch" Position="Bottom">
                                    </asp:PopupControlExtender>
                                </td>
                                <td>
                                    <asp:Label ID="Ibldegree" runat="server" Font-Names="Book Antiqua" Font-Bold="true"
                                        ForeColor="white" Font-Size="Medium" Text="Degree"></asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txt_degree" CssClass="Dropdown_Txt_Box" Font-Names="Book Antiqua"
                                        Font-Bold="true" runat="server" ReadOnly="true" Width="100px">--Select--</asp:TextBox>
                                    <asp:Panel ID="pdegree" runat="server" CssClass="multxtpanel" BackColor="White" BorderColor="Black"
                                        BorderStyle="Solid" Height="200" Width="205" ScrollBars="Auto" Style="">
                                        <asp:CheckBox ID="chk_degree" Font-Bold="true" runat="server" Font-Size="Medium"
                                            Text="Select All" AutoPostBack="True" Font-Names="Book Antiqua" OnCheckedChanged="checkDegree_CheckedChanged" />
                                        <asp:CheckBoxList ID="Chklst_degree" Font-Bold="true" Font-Size="Medium" runat="server"
                                            AutoPostBack="True" Font-Names="Book Antiqua" OnSelectedIndexChanged="cheklist_Degree_SelectedIndexChanged">
                                        </asp:CheckBoxList>
                                    </asp:Panel>
                                    <asp:PopupControlExtender ID="popupdegree" runat="server" TargetControlID="txt_degree"
                                        PopupControlID="pdegree" Position="Bottom">
                                    </asp:PopupControlExtender>
                                </td>
                                <td>
                                    <asp:Label ID="Iblbranch" runat="server" Font-Names="Book Antiqua" Font-Bold="true"
                                        ForeColor="white" Font-Size="Medium" Text="Branch"></asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txt_branch" CssClass="Dropdown_Txt_Box" Font-Bold="true" Font-Names="Book Antiqua"
                                        runat="server" ReadOnly="true" Width="100px">--Select--</asp:TextBox>
                                    <asp:Panel ID="Panel3" runat="server" CssClass="multxtpanel" BackColor="White" BorderColor="Black"
                                        BorderStyle="Solid" Height="200" Width="205" ScrollBars="Auto" Style="">
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
                                </td>
                                <td>
                                    <asp:Label ID="lblsem" runat="server" Font-Names="Book Antiqua" Font-Bold="true"
                                        ForeColor="white" Font-Size="Medium" Text="Sem"></asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtsem" CssClass="Dropdown_Txt_Box" Font-Bold="true" Font-Names="Book Antiqua"
                                        runat="server" ReadOnly="true" Width="100px">--Select--</asp:TextBox>
                                    <asp:Panel ID="Psem" runat="server" CssClass="multxtpanel" BackColor="White" BorderColor="Black"
                                        BorderStyle="Solid" Height="200" Width="205" ScrollBars="Auto" Style="">
                                        <asp:CheckBox ID="chksem" runat="server" Font-Bold="true" Font-Size="Medium" Font-Names="Book Antiqua"
                                            Text="Select All" OnCheckedChanged="chksem_changed" AutoPostBack="True" />
                                        <asp:CheckBoxList ID="chklssem" Font-Names="Book Antiqua" Font-Bold="true" Font-Size="Medium"
                                            runat="server" OnSelectedIndexChanged="chklssem_selected" AutoPostBack="True">
                                        </asp:CheckBoxList>
                                    </asp:Panel>
                                    <asp:PopupControlExtender ID="PopupControlExtender1" runat="server" TargetControlID="txtsem"
                                        PopupControlID="Psem" Position="Bottom">
                                    </asp:PopupControlExtender>
                                </td>
                                <td>
                                    <asp:Label ID="lblsce" runat="server" Font-Names="Book Antiqua" Font-Bold="true"
                                        ForeColor="white" Font-Size="Medium" Text="Sec"></asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtsec" CssClass="Dropdown_Txt_Box" Font-Bold="true" Font-Names="Book Antiqua"
                                        runat="server" ReadOnly="true" Width="100px">--Select--</asp:TextBox>
                                    <asp:Panel ID="Psec" runat="server" CssClass="multxtpanel" BackColor="White" BorderColor="Black"
                                        BorderStyle="Solid" Height="200" Width="205" ScrollBars="Auto" Style="">
                                        <asp:CheckBox ID="chksec" runat="server" Font-Bold="true" Font-Size="Medium" Font-Names="Book Antiqua"
                                            Text="Select All" OnCheckedChanged="chksec_changed" AutoPostBack="True" />
                                        <asp:CheckBoxList ID="chklssec" Font-Names="Book Antiqua" Font-Bold="true" Font-Size="Medium"
                                            runat="server" OnSelectedIndexChanged="chklssec_selected" AutoPostBack="True">
                                        </asp:CheckBoxList>
                                    </asp:Panel>
                                    <asp:PopupControlExtender ID="PopupControlExtender2" runat="server" TargetControlID="txtsec"
                                        PopupControlID="Psec" Position="Bottom">
                                    </asp:PopupControlExtender>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="3">
                                    <asp:Label ID="Label1" runat="server" Font-Names="Book Antiqua" Font-Bold="true"
                                        ForeColor="white" Font-Size="Medium" Text="Time Table"></asp:Label>
                                    <asp:DropDownList ID="ddltimetable" runat="server" Font-Names="Book Antiqua" Font-Bold="true"
                                        Font-Size="Medium" AutoPostBack="true" OnSelectedIndexChanged="headrechnge">
                                        <asp:ListItem Text="All" Value="0"></asp:ListItem>
                                        <asp:ListItem Text="Generated" Value="1"></asp:ListItem>
                                        <asp:ListItem Text="Not Generated" Value="2"></asp:ListItem>
                                    </asp:DropDownList>
                                </td>
                                <td colspan="3">
                                    <asp:Label ID="Label2" runat="server" Font-Names="Book Antiqua" Font-Bold="true"
                                        ForeColor="white" Font-Size="Medium" Text="Batch Allocation"></asp:Label>
                                    <asp:DropDownList ID="ddlbatchallocation" runat="server" Font-Names="Book Antiqua"
                                        Font-Bold="true" Font-Size="Medium" AutoPostBack="true" OnSelectedIndexChanged="headrechnge">
                                        <asp:ListItem Text="All" Value="0"></asp:ListItem>
                                        <asp:ListItem Text="Alloted" Value="1"></asp:ListItem>
                                        <asp:ListItem Text="Not Alloted" Value="2"></asp:ListItem>
                                    </asp:DropDownList>
                                </td>
                                <td colspan="3">
                                    <asp:Label ID="lbllession" runat="server" Font-Names="Book Antiqua" Font-Bold="true"
                                        ForeColor="white" Font-Size="Medium" Text="Lesson Planner"></asp:Label>
                                    <asp:DropDownList ID="ddllession" runat="server" Font-Names="Book Antiqua" Font-Bold="true"
                                        Font-Size="Medium" AutoPostBack="true" OnSelectedIndexChanged="headrechnge">
                                        <asp:ListItem Text="All" Value="0"></asp:ListItem>
                                        <asp:ListItem Text="Planned" Value="1"></asp:ListItem>
                                        <asp:ListItem Text="Not  Planned" Value="2"></asp:ListItem>
                                    </asp:DropDownList>
                                </td>
                                <td>
                                    <asp:CheckBox ID="chkcurrent" runat="server" Font-Names="Book Antiqua" Font-Bold="true"
                                        ForeColor="white" Font-Size="Medium" AutoPostBack="true" OnCheckedChanged="headrechnge"
                                        Text="Current Only" />
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2">
                                    <asp:RadioButton ID="rbtimetable" runat="server" Font-Names="Book Antiqua" Font-Bold="true"
                                        ForeColor="white" Font-Size="Medium" AutoPostBack="true" OnCheckedChanged="headrechnge"
                                        GroupName="Report" Text="Time Table" />
                                </td>
                                <td colspan="2">
                                    <asp:RadioButton ID="rbbatch" runat="server" Font-Names="Book Antiqua" Font-Bold="true"
                                        ForeColor="white" Font-Size="Medium" AutoPostBack="true" OnCheckedChanged="headrechnge"
                                        GroupName="Report" Text="Batch Allocation" />
                                </td>
                                <td colspan="2">
                                    <asp:RadioButton ID="rblession" runat="server" Font-Names="Book Antiqua" Font-Bold="true"
                                        ForeColor="white" Font-Size="Medium" AutoPostBack="true" OnCheckedChanged="headrechnge"
                                        GroupName="Report" Text="Lesson Planner" />
                                </td>
                                <td>
                                    <asp:Button ID="btngo" runat="server" Font-Names="Book Antiqua" Text="Go" OnClick="btngo_Click"
                                        Font-Size="Medium" Font-Bold="true" />
                                </td>
                                <td>
                                    <%--  <asp:CheckBox ID="chktimetable" runat="server" Font-Names="Book Antiqua" Font-Bold="true"
                        ForeColor="white" Font-Size="Medium" AutoPostBack="true" OnCheckedChanged="headrechnge"
                        Style="position: absolute; left: 11px; top: 85px;" Text="Time Table" />
                    <asp:CheckBox ID="chkbatch" runat="server" Font-Names="Book Antiqua" Font-Bold="true"
                        ForeColor="white" Font-Size="Medium" AutoPostBack="true" OnCheckedChanged="headrechnge"
                        Style="position: absolute; left: 135px; top: 85px;" Text="Batch Allocation" />
                    <asp:CheckBox ID="chkles" runat="server" Font-Names="Book Antiqua" Font-Bold="true"
                        ForeColor="white" AutoPostBack="true" OnCheckedChanged="headrechnge" Font-Size="Medium"
                        Style="position: absolute; left: 278px; top: 85px;" Text="Lesson Planner" />--%>
                                </td>
                                <td>
                                </td>
                                <td>
                                </td>
                            </tr>
                        </table>
                    </div>
                    <br />
                    <asp:Panel ID="pheaderfilter" runat="server" CssClass="cpHeader" BackColor="#719DDB"
                        Width="959px" Style="margin-left: 15px">
                        <asp:Label ID="Labelfilter" Text="Column Order" runat="server" Font-Size="Medium"
                            Font-Bold="True" Font-Names="Book Antiqua" />
                        <asp:Image ID="Imagefilter" runat="server" CssClass="cpimage" ImageUrl="right.jpeg"
                            ImageAlign="Right" />
                    </asp:Panel>
                    <asp:Panel ID="pbodyfilter" runat="server" CssClass="cpBody" Width="952px">
                        <asp:CheckBoxList ID="chklscolumn" runat="server" Font-Size="Medium" AutoPostBack="True"
                            OnSelectedIndexChanged="headrechnge" Font-Bold="True" RepeatColumns="5" RepeatDirection="Horizontal"
                            Font-Names="Book Antiqua">
                            <asp:ListItem Text="S.No"></asp:ListItem>
                            <asp:ListItem Text="Batch Year"></asp:ListItem>
                            <asp:ListItem Text="Degree"></asp:ListItem>
                            <asp:ListItem Text="Branch"></asp:ListItem>
                            <asp:ListItem Text="Sem"></asp:ListItem>
                            <asp:ListItem Text="Section"></asp:ListItem>
                            <asp:ListItem Text="Time Table Name"></asp:ListItem>
                            <asp:ListItem Text="Start Date"></asp:ListItem>
                            <asp:ListItem Text="Batch Allocation Status"></asp:ListItem>
                            <asp:ListItem Text="No Of Batch"></asp:ListItem>
                            <asp:ListItem Text="Student Count"></asp:ListItem>
                            <asp:ListItem Text="Lesson Planner Status"></asp:ListItem>
                            <asp:ListItem Text="Subject Code"></asp:ListItem>
                            <asp:ListItem Text="Subject Name"></asp:ListItem>
                            <asp:ListItem Text="Staff Code"></asp:ListItem>
                            <asp:ListItem Text="Staff Name"></asp:ListItem>
                        </asp:CheckBoxList>
                    </asp:Panel>
                    <asp:CollapsiblePanelExtender ID="cpecolumnorder" runat="server" TargetControlID="pbodyfilter"
                        CollapseControlID="pheaderfilter" ExpandControlID="pheaderfilter" Collapsed="true"
                        TextLabelID="Labelfilter" CollapsedSize="0" ImageControlID="Imagefilter" CollapsedImage="right.jpeg"
                        ExpandedImage="down.jpeg">
                    </asp:CollapsiblePanelExtender>
                    <br />
                    <asp:Label ID="lbl_err" runat="server" Text="" ForeColor="Red" Font-Bold="true" Font-Size="Medium"
                        Font-Names="Book Antiqua"></asp:Label>
                    <FarPoint:FpSpread ID="FpSpread1" runat="server" BorderColor="Black" BorderStyle="Solid"
                        BorderWidth="1px" Visible="false" VerticalScrollBarPolicy="Never" HorizontalScrollBarPolicy="Never"
                        CssClass="stylefp" Style="margin-left: 15px">
                        <CommandBar BackColor="Control" ButtonFaceColor="Control" ButtonHighlightColor="ControlLightLight"
                            ButtonShadowColor="ControlDark">
                        </CommandBar>
                        <Sheets>
                            <FarPoint:SheetView SheetName="Sheet1">
                            </FarPoint:SheetView>
                        </Sheets>
                    </FarPoint:FpSpread>
                    <br />
                    <asp:Label ID="lblrptname" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                        Font-Size="Medium" Text="Report Name"></asp:Label>
                    <asp:TextBox ID="txtexcelname" runat="server" Height="20px" Width="180px" Style="font-family: 'Book Antiqua'"
                        Font-Bold="True" onkeypress="display()" Font-Names="Book Antiqua" Font-Size="Medium"></asp:TextBox>
                    <asp:FilteredTextBoxExtender ID="Filterspace" runat="server" TargetControlID="txtexcelname"
                        FilterType="LowercaseLetters,UppercaseLetters,Numbers,Custom" ValidChars="()}{][ .">
                    </asp:FilteredTextBoxExtender>
                    <asp:Button ID="btnxl" runat="server" Text="Export Excel" Font-Bold="True" Font-Names="Book Antiqua"
                        Font-Size="Medium" OnClick="btnxl_Click" />
                    <asp:Button ID="btnmasterprint" runat="server" Text="Print" Font-Bold="True" Font-Names="Book Antiqua"
                        Font-Size="Medium" OnClick="btnmasterprint_Click" />
                    <Insproplus:PRINTPDF runat="server" ID="Printcontrol" Visible="false" />
                </center>
            </ContentTemplate>
            <Triggers>
                <asp:PostBackTrigger ControlID="btnmasterprint" />
                <asp:PostBackTrigger ControlID="btnxl" />
                <asp:PostBackTrigger ControlID="btngo" />
            </Triggers>
        </asp:UpdatePanel>
</asp:Content>

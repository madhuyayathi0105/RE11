<%@ Page Title="" Language="C#" MasterPageFile="~/BlackBoxMod/BlackBoxSubSiteMaster.master"
    AutoEventWireup="true" CodeFile="blackbox3.aspx.cs" Inherits="blackbox3" MaintainScrollPositionOnPostback="true" %>

<%@ Register Assembly="FarPoint.Web.Spread" Namespace="FarPoint.Web.Spread" TagPrefix="FarPoint" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Src="~/Usercontrols/PrintMaster.ascx" TagName="PRINTPDF" TagPrefix="Insproplus" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <link href="~/Styles/css/Commoncss.css" rel="stylesheet" type="text/css" />
    <style type="text/css">
        .style1
        {
            width: 198px;
        }
        .style2
        {
            width: 235px;
        }
        .style3
        {
            width: 111px;
        }
    </style>
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
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <center>
        <asp:Label ID="Label4" runat="server" Text="Black Box" CssClass="fontstyleheader"
            Style="margin: 0px; margin-bottom: 10px; margin-top: 10px; position: relative;"
            ForeColor="Green"></asp:Label>
        <asp:Panel ID="pnl_filter" runat="server" Style="margin: 0px; margin-bottom: 10px;
            margin-top: 10px; position: relative;">
            <table class="maintablestyle">
                <tr>
                    <td>
                        <asp:Label ID="Label1" runat="server" Text="College" Font-Bold="True" Font-Names="Book Antiqua"
                            Font-Size="Medium"></asp:Label>
                    </td>
                    <td>
                        <asp:DropDownList ID="ddlcollege" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                            Width="181px" OnSelectedIndexChanged="ddlcollege_SelectedIndexChanged" Font-Size="Medium"
                            AutoPostBack="true">
                        </asp:DropDownList>
                    </td>
                    <td>
                        <asp:CheckBox ID="chkdegreewise" runat="server" Font-Bold="True" Text="DegreeWise"
                            AutoPostBack="True" OnCheckedChanged="chkdegreewise_CheckedChanged" />
                    </td>
                     <td>
                     <asp:RadioButton ID="rbAtt" runat="server" Visible="false" GroupName="CA" Text="Attendance" />
                      <asp:RadioButton ID="rbCAM" runat="server" Visible="false"  GroupName="CA" Text="CAM Details" />
                        <%--<asp:CheckBox ID="chkCAM" runat="server" Visible="false" Font-Bold="True" Text="CAM Details" />--%>
                    </td>
                    <td colspan="3">
                        <asp:RadioButtonList ID="rdiobtndetailornot" runat="server" RepeatDirection="Horizontal"
                            Font-Bold="true">
                            <asp:ListItem>Count</asp:ListItem>
                            <asp:ListItem>Detail</asp:ListItem>
                        </asp:RadioButtonList>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="deptlbl" runat="server" Text="Department" Font-Bold="True" Font-Names="Book Antiqua"
                            Font-Size="Medium" Style="font-family: 'Book Antiqua';"></asp:Label>
                    </td>
                    <td>
                        <div style="position: relative;">
                            <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                                <ContentTemplate>
                                    <asp:TextBox ID="txtbranch" runat="server" Height="20px" ReadOnly="true" Width="180px"
                                        Style="font-family: 'Book Antiqua';" Font-Bold="True" Font-Names="Book Antiqua"
                                        Font-Size="Medium">---Select---</asp:TextBox>
                                    <asp:Panel ID="pbranch" runat="server" CssClass="multxtpanel" Style="overflow-x: hidden;
                                        overflow-y: hidden;">
                                        <asp:CheckBox ID="chkbranch" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                            Font-Size="Medium" OnCheckedChanged="chkbranch_CheckedChanged" Text="Select All"
                                            AutoPostBack="True" />
                                        <asp:CheckBoxList ID="chklstbranch" runat="server" Font-Size="Medium" AutoPostBack="True"
                                            OnSelectedIndexChanged="chklstbranch_SelectedIndexChanged" Style="font-family: 'Book Antiqua'"
                                            Font-Bold="True" Font-Names="Book Antiqua">
                                        </asp:CheckBoxList>
                                    </asp:Panel>
                                    <asp:PopupControlExtender ID="PopupControlExtender1" runat="server" TargetControlID="txtbranch"
                                        PopupControlID="pbranch" Position="Bottom">
                                    </asp:PopupControlExtender>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </div>
                    </td>
                    <td>
                        <asp:Label ID="lbldesignation" runat="server" Text="Designation" Font-Bold="True"
                            ForeColor="Black" Font-Names="Book Antiqua" Font-Size="Medium" Width="100px"
                            Style="font-family: 'Book Antiqua';"></asp:Label>
                    </td>
                    <td>
                        <div style="position: relative;">
                            <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                                <ContentTemplate>
                                    <asp:TextBox ID="txtdesi" runat="server" Height="20px" ReadOnly="true" Style="font-family: 'Book Antiqua';"
                                        Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium">---Select---</asp:TextBox>
                                    <asp:Panel ID="Paneldesi" runat="server" CssClass="multxtpanel" Height="300px">
                                        <asp:CheckBox ID="chkdesi" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                            Font-Size="Medium" OnCheckedChanged="chkdesi_CheckedChanged" Text="Select All"
                                            AutoPostBack="True" />
                                        <asp:CheckBoxList ID="cbldesi" runat="server" Font-Size="Medium" AutoPostBack="True"
                                            OnSelectedIndexChanged="cbldesi_SelectedIndexChanged" Font-Bold="True" Font-Names="Book Antiqua">
                                        </asp:CheckBoxList>
                                    </asp:Panel>
                                    <asp:PopupControlExtender ID="PopupControlExtender2" runat="server" TargetControlID="txtdesi"
                                        PopupControlID="Paneldesi" Position="Bottom">
                                    </asp:PopupControlExtender>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </div>
                    </td>
                    <td class="style3">
                        <asp:Label ID="lblstaff" runat="server" Text="Staff Name" Font-Bold="True" Font-Names="Book Antiqua"
                            Font-Size="Medium" Style="font-family: 'Book Antiqua';"></asp:Label>
                    </td>
                    <td>
                        <div style="position: relative;">
                            <asp:UpdatePanel ID="UpdatePanel3" runat="server">
                                <ContentTemplate>
                                    <asp:TextBox ID="txtstaff" runat="server" Height="20px" ReadOnly="true" Style="font-family: 'Book Antiqua';"
                                        Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium">---Select---</asp:TextBox>
                                    <asp:Panel ID="pstaff" runat="server" CssClass="multxtpanel" Height="300px" Width="361px">
                                        <asp:CheckBox ID="chkstaff" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                            Font-Size="Medium" OnCheckedChanged="chkstaff_CheckedChanged" Text="Select All"
                                            AutoPostBack="True" />
                                        <asp:CheckBoxList ID="chklststaff" runat="server" Font-Size="Medium" AutoPostBack="True"
                                            OnSelectedIndexChanged="chklststaff_SelectedIndexChanged" Style="font-family: 'Book Antiqua'"
                                            Font-Bold="True" Font-Names="Book Antiqua" Height="58px">
                                        </asp:CheckBoxList>
                                    </asp:Panel>
                                    <asp:PopupControlExtender ID="PopupControlExtender3" runat="server" TargetControlID="txtstaff"
                                        PopupControlID="pstaff" Position="Bottom">
                                    </asp:PopupControlExtender>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </div>
                    </td>
                </tr>
                <tr>
                 <%-- Rajkumar--%>
                 <td>
                        <asp:Label ID="lblDegSubject" runat="server" Text="Subjects" Font-Bold="True"
                            Font-Names="Book Antiqua" Font-Size="Medium"></asp:Label>
                    </td>
                    <td>
                       <div style="position: relative;">
                            <asp:UpdatePanel ID="UpdatePanel5" runat="server">
                                <ContentTemplate>
                                    <asp:TextBox ID="txtDegSubject" runat="server" Height="20px" ReadOnly="true" Width="100px"
                                        Style="font-family: 'Book Antiqua';" Font-Bold="True" Font-Names="Book Antiqua"
                                        Font-Size="Medium">---Select---</asp:TextBox>
                                    &nbsp;&nbsp;&nbsp;
                                    <asp:Panel ID="Panel1" runat="server" CssClass="multxtpanel" Width="254px" Style="overflow-x: hidden;
                                        overflow-y: hidden;">
                                        <asp:CheckBox ID="chkDegSubject" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                            Font-Size="Medium" OnCheckedChanged="chkDegsubject_CheckedChanged" Text="Select All"
                                            AutoPostBack="True" />
                                        <asp:CheckBoxList ID="cblDegSubject" runat="server" Font-Size="Medium" AutoPostBack="True"
                                            OnSelectedIndexChanged="chklDegstsubject_SelectedIndexChanged" Style="font-family: 'Book Antiqua'"
                                            Font-Bold="True" Font-Names="Book Antiqua" Height="58px">
                                        </asp:CheckBoxList>
                                    </asp:Panel>
                                    <asp:PopupControlExtender ID="PopupControlExtender5" runat="server" TargetControlID="txtDegSubject"
                                        PopupControlID="Panel1" Position="Bottom">
                                    </asp:PopupControlExtender>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </div
                    </td>
                   <%-- Rajkumar--%>
                    <td>
                        <asp:Label ID="lblsubject" runat="server" Text="Hour" Width="61px" Font-Bold="True"
                            ForeColor="Black" Font-Names="Book Antiqua" Font-Size="Medium" Style="font-family: 'Book Antiqua';"></asp:Label>
                    </td>
                    <td>
                        <div style="position: relative;">
                            <asp:UpdatePanel ID="UpdatePanel4" runat="server">
                                <ContentTemplate>
                                    <asp:TextBox ID="txtsubject" runat="server" Height="20px" ReadOnly="true" Width="100px"
                                        Style="font-family: 'Book Antiqua';" Font-Bold="True" Font-Names="Book Antiqua"
                                        Font-Size="Medium">---Select---</asp:TextBox>
                                    &nbsp;&nbsp;&nbsp;
                                    <asp:Panel ID="psubject" runat="server" CssClass="multxtpanel" Style="overflow-x: hidden;
                                        overflow-y: hidden;">
                                        <asp:CheckBox ID="chksubject" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                            Font-Size="Medium" OnCheckedChanged="chksubject_CheckedChanged" Text="Select All"
                                            AutoPostBack="True" />
                                        <asp:CheckBoxList ID="chklstsubject" runat="server" Font-Size="Medium" AutoPostBack="True"
                                            OnSelectedIndexChanged="chklstsubject_SelectedIndexChanged" Style="font-family: 'Book Antiqua'"
                                            Font-Bold="True" Font-Names="Book Antiqua" Height="58px">
                                        </asp:CheckBoxList>
                                    </asp:Panel>
                                    <asp:PopupControlExtender ID="PopupControlExtender4" runat="server" TargetControlID="txtsubject"
                                        PopupControlID="psubject" Position="Bottom">
                                    </asp:PopupControlExtender>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </div>
                    </td>
                    <td>
                        <asp:Label ID="Labelfdate" runat="server" Text="From Date" Font-Bold="True" Font-Names="Book Antiqua"
                            Font-Size="Medium" Style="font-family: 'Book Antiqua';"></asp:Label>
                    </td>
                    <td>
                        <asp:TextBox ID="tbfdate" runat="server" OnTextChanged="tbfdate_TextChanged" Width="91px"
                            Style="font-family: 'Book Antiqua';" AutoPostBack="True" Font-Bold="True" Font-Names="Book Antiqua"
                            Font-Size="Medium"></asp:TextBox>
                        <asp:CalendarExtender ID="CalendarExtender1" TargetControlID="tbfdate" Format="d-MM-yyyy"
                            runat="server">
                        </asp:CalendarExtender>
                        <asp:RequiredFieldValidator Display="Dynamic" ID="RequiredFieldValidator1" runat="server"
                            ControlToValidate="tbfdate" ErrorMessage="Select From Date" ForeColor="Red" Width="110px"></asp:RequiredFieldValidator>
                    </td>
                    <td>
                        <asp:Label ID="Labeltodate" runat="server" Text="To Date" Font-Bold="True" Font-Names="Book Antiqua"
                            Font-Size="Medium" Style="font-family: 'Book Antiqua';"></asp:Label>
                    </td>
                    <td>
                        <asp:TextBox ID="tbtodate" runat="server" OnTextChanged="tbtodate_TextChanged" Width="80px"
                            AutoPostBack="True" Font-Bold="True" Style="font-family: 'Book Antiqua';" Font-Names="Book Antiqua"
                            Font-Size="Medium"></asp:TextBox>
                        <asp:CalendarExtender ID="CalendarExtender2" Format="d-MM-yyyy" TargetControlID="tbtodate"
                            runat="server">
                        </asp:CalendarExtender>
                        <asp:RequiredFieldValidator Display="Dynamic" ID="RequiredFieldValidator2" runat="server"
                            ControlToValidate="tbtodate" ErrorMessage="Select From Date" ForeColor="Red"
                            Width="110px"></asp:RequiredFieldValidator>
                    </td>
                    <td>
                        <asp:Button ID="Buttongo" runat="server" Text="Go" OnClick="Buttongo_Click" Style="width: 50px;
                            font-family: 'Book Antiqua';" Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium" />
                    </td>
                </tr>
            </table>
        </asp:Panel>
        <asp:Panel ID="pheaderfilter" runat="server" CssClass="cpHeader" BackColor="#719DDB"
            Width="959px" Style="margin: 0px; margin-bottom: 10px; margin-top: 10px; position: relative;">
            <asp:Label ID="Labelfilter" Text="Column Order" runat="server" Font-Size="Medium"
                Font-Bold="True" Font-Names="Book Antiqua" />
            <asp:Image ID="Imagefilter" runat="server" CssClass="cpimage" ImageUrl="right.jpeg"
                ImageAlign="Right" />
        </asp:Panel>
        <asp:Panel ID="pbodyfilter" runat="server" CssClass="cpBody" Width="952px">
            <center>
                <asp:CheckBoxList ID="chklscolumn" runat="server" Font-Size="Medium" AutoPostBack="True"
                    OnSelectedIndexChanged="chklscolumn_SelectedIndexChanged" Font-Bold="True" RepeatColumns="8"
                    RepeatDirection="Horizontal" Font-Names="Book Antiqua">
                    <asp:ListItem Text="Department"></asp:ListItem>
                    <asp:ListItem Text="Designation"></asp:ListItem>
                    <asp:ListItem Text="Category"></asp:ListItem>
                    <asp:ListItem Text="Staff type"></asp:ListItem>
                    <asp:ListItem Text="Staff Code"></asp:ListItem>
                    <asp:ListItem Text="Staff Name"></asp:ListItem>
                </asp:CheckBoxList>
            </center>
        </asp:Panel>
        <asp:CollapsiblePanelExtender ID="cpecolumnorder" runat="server" TargetControlID="pbodyfilter"
            CollapseControlID="pheaderfilter" ExpandControlID="pheaderfilter" Collapsed="true"
            TextLabelID="Labelfilter" CollapsedSize="0" ImageControlID="Imagefilter" CollapsedImage="right.jpeg"
            ExpandedImage="down.jpeg">
        </asp:CollapsiblePanelExtender>
    </center>
    <asp:Label ID="lblerror" runat="server" Text="lblerrormsg" ForeColor="Red" Font-Bold="true"
        Visible="false" Style="margin: 0px; margin-bottom: 10px; margin-top: 10px; position: relative;"></asp:Label>
    <asp:Label ID="Labelstaf" runat="server" ForeColor="Red" Text="There is no class for the staff between the given date"
        Visible="False" Font-Names="Book Antiqua" Font-Size="Medium" Style="margin: 0px;
        margin-bottom: 10px; margin-top: 10px; position: relative;"></asp:Label>
    <asp:Label ID="lbldatediff" runat="server" Font-Names="Book Antiqua" Font-Size="Medium"
        ForeColor="Red" Text="From Date Should not be greater than To Date" Visible="False"
        Style="margin: 0px; margin-bottom: 10px; margin-top: 10px; position: relative;"></asp:Label>
    <center>
        <div style="margin: 0px; margin-bottom: 10px; margin-top: 10px;">
            <FarPoint:FpSpread ID="FpSpread1" runat="server" Height="250px" Width="400px" BorderColor="Black"
                BorderWidth="0.5" Style="margin: 0px; margin-bottom: 10px; margin-top: 10px;
                position: relative;" CssClass="spreadborder" ShowHeaderSelection="false">
                <Sheets>
                    <FarPoint:SheetView SheetName="Sheet1">
                    </FarPoint:SheetView>
                </Sheets>
            </FarPoint:FpSpread>
        </div>
        <div style="margin: 0px; margin-bottom: 10px; margin-top: 10px;">
            <asp:Label ID="lblrptname" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                Font-Size="Medium" Text="Report Name"></asp:Label>
            <asp:TextBox ID="txtexcelname" runat="server" Height="20px" Width="180px" Style="font-family: 'Book Antiqua'"
                Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium"></asp:TextBox>
            <asp:Button ID="btnxl" runat="server" Text="Export Excel" Font-Bold="True" Font-Names="Book Antiqua"
                Font-Size="Medium" OnClick="btnxl_Click" />
            <asp:Button ID="btnprintmaster" runat="server" Text="Print" OnClick="btnprintmaster_Click"
                Font-Names="Book Antiqua" Font-Size="Medium" Font-Bold="true" />
            <Insproplus:PRINTPDF runat="server" ID="Printcontrol" Visible="false" />
        </div>
    </center>
</asp:Content>

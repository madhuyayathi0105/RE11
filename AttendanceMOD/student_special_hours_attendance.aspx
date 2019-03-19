<%@ Page Title="Student Special Hour Attendance" Language="C#" MasterPageFile="~/AttendanceMOD/AttendanceSubSiteMaster.master"
    AutoEventWireup="true" CodeFile="student_special_hours_attendance.aspx.cs" Inherits="student_special_hours_attendance" %>

<%@ Register Assembly="FarPoint.Web.Spread" Namespace="FarPoint.Web.Spread" TagPrefix="FarPoint" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Src="~/Usercontrols/PrintMaster.ascx" TagName="PRINTPDF" TagPrefix="Insproplus" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <style type="text/css">
        .floats
        {
            float: right;
        }
        .cpHeader
        {
            color: white;
            background-color: #719DDB;
            font-size: 12px;
            cursor: pointer;
            padding: 4px;
            font-style: normal;
            font-variant: normal;
            font-weight: bold;
            line-height: normal;
            font-family: "auto Trebuchet MS" , Verdana;
        }
        .cpBody
        {
            background-color: transparent;
            font: normal 11px auto Verdana, Arial;
            border: 1px gray;
            padding-top: 7px;
            padding-left: 4px;
            padding-right: 4px;
            padding-bottom: 4px;
        }
        .cpimage
        {
            float: right;
            vertical-align: middle;
            background-color: transparent;
        }
        .cur
        {
            cursor: pointer;
        }
        .cursorptr
        {
        }
        .style109
        {
        }
        .style110
        {
            width: 134px;
        }
        .txt
        {
        }
        .style111
        {
            width: 102px;
        }
        .style112
        {
            width: 429px;
        }
        .style113
        {
            width: 411px;
        }
        .style114
        {
            width: 558px;
        }
        .style115
        {
            width: 667px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <center>
        <span class="fontstyleheader" style="color: Green; margin: 0px; margin-bottom: 10px;
            margin-top: 10px;">Student Special Hour Attendance </span>
    </center>
    <asp:Panel ID="Panel1" runat="server">
        <center>
            <div class="maintablestyle" style="width: 900px; margin: 0px; margin-bottom: 10px;
                margin-top: 10px; text-align: left;">
                <table>
                    <tr>
                        <td>
                            <asp:Label ID="lblbatch" runat="server" Text="Batch" Style="font-family: 'Baskerville Old Face';
                                font-weight: 700;" Font-Names="Book Antiqua" Font-Size="Medium" Height="16px"></asp:Label>
                        </td>
                        <td class="style64">
                            <asp:DropDownList ID="ddlbatch" CssClass="cursorptr" runat="server" AutoPostBack="True"
                                OnSelectedIndexChanged="ddlbatch_SelectedIndexChanged" Font-Names="Book Antiqua"
                                Font-Size="Medium" Font-Bold="True" Height="25px">
                            </asp:DropDownList>
                        </td>
                        <td>
                            <asp:Label ID="lbldegree" runat="server" Text="Degree" Style="font-family: 'Baskerville Old Face';
                                font-weight: 700;" Font-Names="Book Antiqua" Font-Size="Medium" Height="16px"></asp:Label>
                        </td>
                        <td>
                            <asp:DropDownList ID="ddldegree" CssClass="cursorptr" runat="server" AutoPostBack="true"
                                OnSelectedIndexChanged="ddldegree_SelectedIndexChanged" Width="100px" Font-Names="Book Antiqua"
                                Font-Size="Medium" Font-Bold="True" Height="25px">
                            </asp:DropDownList>
                        </td>
                        <td>
                            <asp:Label ID="lblbranch" runat="server" Text="Branch" Style="font-family: 'Baskerville Old Face';
                                font-weight: 700;" Font-Names="Book Antiqua" Font-Size="Medium"></asp:Label>
                        </td>
                        <td>
                            <asp:DropDownList ID="ddlbranch" CssClass="cursorptr" runat="server" OnSelectedIndexChanged="ddlbranch_SelectedIndexChanged"
                                AutoPostBack="True" Height="25px" Width="191px" Font-Names="Book Antiqua" Font-Size="Medium"
                                Font-Bold="True">
                            </asp:DropDownList>
                        </td>
                        <td>
                            <asp:Label ID="lblsem" runat="server" Text="Sem" Style="font-family: 'Baskerville Old Face';
                                font-weight: 700;" Font-Names="Book Antiqua" Font-Size="Medium" Height="16px"></asp:Label>
                        </td>
                        <td>
                            <asp:DropDownList ID="ddlsem" CssClass="cursorptr" runat="server" OnSelectedIndexChanged="ddlsem_SelectedIndexChanged"
                                Width="80px" AutoPostBack="True" Height="25px" Font-Names="Book Antiqua" Font-Size="Medium"
                                Font-Bold="True">
                            </asp:DropDownList>
                        </td>
                        <td>
                            <asp:Label ID="lblsec" runat="server" Text="Sec" Style="font-family: 'Baskerville Old Face';
                                font-weight: 700;" Font-Names="Book Antiqua" Font-Size="Medium" Height="16px"></asp:Label>
                        </td>
                        <td>
                            <asp:DropDownList ID="ddlsec" CssClass="cursorptr" runat="server" AutoPostBack="True"
                                OnSelectedIndexChanged="ddlsec_SelectedIndexChanged" Height="25px" Width="81px"
                                Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium">
                            </asp:DropDownList>
                        </td>
                    </tr>
                </table>
                <table style="margin-left: 0px;">
                    <tr>
                        <td>
                            <asp:Label ID="lblfrom" runat="server" Text="From Date" Style="font-family: 'Baskerville Old Face';
                                font-weight: 700;" Width="80px" Font-Names="Book Antiqua" Font-Size="Medium"
                                Height="16px"></asp:Label>
                        </td>
                        <td class="style111">
                            <asp:TextBox ID="txtFromDate" CssClass="txt" runat="server" Height="19px" Width="90px"
                                Font-Names="Book Antiqua" Font-Size="Medium" OnTextChanged="txtFromDate_TextChanged"
                                Font-Bold="True" AutoPostBack="True"></asp:TextBox>
                        </td>
                        <asp:CalendarExtender ID="CalendarExtender2" TargetControlID="txtFromDate" runat="server"
                            Format="d-MM-yyyy">
                        </asp:CalendarExtender>
                        <td>
                            <asp:Label ID="lblTo" runat="server" Text="To Date" Style="font-family: 'Baskerville Old Face';
                                font-weight: 700;" Width="66px" Font-Names="Book Antiqua" Font-Size="Medium"
                                Height="16px"></asp:Label>
                        </td>
                        <td>
                            <asp:TextBox ID="TxtToDate" runat="server" Height="19px" Width="90px" Style="top: 281px;
                                left: 289px" Font-Names="Book Antiqua" Font-Size="Medium" OnTextChanged="TxtToDate_TextChanged"
                                Font-Bold="True" AutoPostBack="True"></asp:TextBox>
                            <asp:CalendarExtender ID="CalendarExtender1" TargetControlID="txtToDate" runat="server"
                                Format="d-MM-yyyy">
                            </asp:CalendarExtender>
                            <br />
                        </td>
                        <td colspan="3">
                            <asp:CheckBox ID="chkAllstudent" runat="server" AutoPostBack="true" Text="ShowAllStudent"/>
                        </td>
                        <td>
                            <asp:Button ID="Btngo" runat="server" OnClick="Btngo_Click" CssClass="cursorptr"
                                Style="font-weight: 700; top: 273px; left: 385px;" Text="GO" Width="56px" />
                        </td>
                       
                        
                        </tr>
                         <tr>
                        <td>
                            <asp:Label ID="lbl_subj_select" runat="server" Text="Select Subject" Font-Names="Book Antiqua"
                                Font-Bold="True" Font-Size="Medium"></asp:Label>
                        </td>
                        <td>
                            <asp:DropDownList ID="ddl_select_subj" runat="server" Font-Names="Book Antiqua" Font-Size="Medium"
                                Font-Bold="True" AutoPostBack="True" Height="22px" OnSelectedIndexChanged="ddl_select_subj_SelectedIndexChanged"
                                Width="172px">
                            </asp:DropDownList>
                        </td>
                        <td>
                            <asp:Label ID="lblSelectHour" runat="server" Text="Hour" Font-Names="Book Antiqua"
                                Font-Bold="True" Font-Size="Medium"></asp:Label>
                        </td>
                        <td>
                            <asp:DropDownList ID="ddl_select_hour" runat="server" Font-Names="Book Antiqua" Font-Size="Medium"
                                Font-Bold="True" AutoPostBack="True" Height="22px" OnSelectedIndexChanged="ddl_select_hour_SelectedIndexChanged"
                                Width="172px">
                            </asp:DropDownList>
                        </td>
                        <td>
                            &nbsp;
                        </td>
                        <td>
                            &nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2">
                            <asp:Label ID="lblfromdate" runat="server" Font-Bold="true" Font-Names="Book Antiqua"
                                Font-Size="Small" ForeColor="Red"></asp:Label>
                        </td>
                        <td colspan="2">
                            <asp:Label ID="lbltodate" runat="server" ForeColor="Red" Font-Names="Book Antiqua"
                                Font-Size="Small" Font-Bold="true"></asp:Label>
                        </td>
                        <td colspan="3">
                            <asp:Label ID="datelbl" runat="server" ForeColor="Red" Font-Names="Book Antiqua"
                                Font-Size="Small" Font-Bold="true"></asp:Label>
                        </td>
                    </tr>
                </table>
            </div>
        </center>
        <center>
            <table>
                <tr>
                    <td colspan="2">
                    </td>
                </tr>
                <tr>
                    <td colspan="2">
                    </td>
                </tr>
                <tr>
                    <td colspan="2">
                        <asp:Label ID="lblset" runat="server" Visible="False" Style="font-family: 'Baskerville Old Face';
                            font-weight: 700; height: auto; width: auto;" Font-Bold="False" Font-Size="Medium"
                            ForeColor="Red"></asp:Label>
                    </td>
                </tr>
            </table>
        </center>
    </asp:Panel>
    <asp:Panel ID="pHeaderatendence" Visible="false" runat="server" CssClass="cpHeader">
        <asp:Label ID="Labelatend" runat="server" Text="Mark Special Hour Attendance" BackColor="Transparent"
            BorderColor="Transparent" BorderWidth="0px" Height="16px" Font-Bold="True" Font-Names="Book Antiqua"
            Font-Size="Medium" />
        <asp:Image ID="ImageSel" runat="server" CssClass="cpimage" ImageUrl="../images/right.jpeg" />
    </asp:Panel>
    <asp:Panel ID="pBodyatendence" runat="server" CssClass="cpBody">
        <asp:Panel ID="Panelpage" Visible="false" runat="server" Height="28px">
            <asp:Button ID="Buttontotal" runat="server" Text="Button" Height="21px" Width="180px"
                BackColor="Transparent" BorderColor="Transparent" BorderWidth="0px" Font-Bold="True"
                Font-Names="Book Antiqua" Font-Size="Medium" />
            <asp:Label ID="Labelotherpage" runat="server" Text="No of records per page" Font-Bold="True"
                Font-Names="Book Antiqua" Font-Size="Medium"></asp:Label>
            <asp:DropDownList ID="DropDownListpage" runat="server" Height="16px" Width="59px"
                OnSelectedIndexChanged="DropDownListpage_SelectedIndexChanged" AutoPostBack="True"
                Font-Bold="True">
            </asp:DropDownList>
            <asp:TextBox ID="TextBoxother" runat="server" OnTextChanged="TextBoxother_TextChanged"
                Visible="false" AutoPostBack="True" Height="10px" Width="40px" Font-Bold="True"></asp:TextBox>
            <asp:Label ID="lblother" runat="server" Text="Select" Visible="False" ForeColor="Red"
                Style="font-weight: 400" Font-Names="Book Antiqua" Font-Size="Medium"></asp:Label>
            &nbsp;&nbsp;
            <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" TargetControlID="TextBoxother"
                FilterType="Numbers" runat="server">
            </asp:FilteredTextBoxExtender>
            <asp:Label ID="Labelpage" runat="server" Text="Page Search" Font-Bold="True" Font-Names="Book Antiqua"
                Font-Size="Medium"></asp:Label>
            <asp:TextBox ID="TextBoxpage" runat="server" OnTextChanged="TextBoxpage_TextChanged"
                AutoPostBack="True" Height="10px" Width="40px" Font-Bold="True"></asp:TextBox>
            <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender2" TargetControlID="TextBoxpage"
                FilterType="Numbers" runat="server">
            </asp:FilteredTextBoxExtender>
            <asp:Label ID="LabelE" runat="server" Font-Bold="False" Font-Names="Book Antiqua"
                Font-Size="Medium" ForeColor="Red" Height="17px" Width="247px"></asp:Label>
        </asp:Panel>
        <br />
        <center>
            <FarPoint:FpSpread ID="FpSpread2" runat="server" BorderWidth="2px" Height="50px"
                OnUpdateCommand="FpSpread2_UpdateCommand" ShowHeaderSelection="false">
                <CommandBar BackColor="Control" ButtonFaceColor="Control" Visible="false" ButtonHighlightColor="ControlLightLight"
                    ButtonShadowColor="ControlDark">
                </CommandBar>
                <Sheets>
                    <FarPoint:SheetView SheetName="Sheet1">
                    </FarPoint:SheetView>
                </Sheets>
            </FarPoint:FpSpread>
        </center>
        <br />
        <asp:Panel ID="Panel3" runat="server">
            <asp:Button ID="Buttonexit" runat="server" CssClass="floats" Visible="false" Text="Exit"
                OnClick="Buttonexit_Click" Font-Bold="true" />
            <asp:Button ID="Buttonsave" runat="server" CssClass="floats" Text="Save" OnClick="Buttonsave_Click"
                Font-Bold="true" />
            <asp:Button ID="Buttonupdate" runat="server" CssClass="floats" Text="Update" OnClick="Buttonupdate_Click"
                Font-Bold="true" />
            <asp:Button ID="Buttondeselect" runat="server" CssClass="floats" Text="De-Select All"
                OnClick="Buttondeselect_Click" Font-Bold="true" />
            <asp:Button ID="Buttonselectall" CssClass="floats" runat="server" Text="Select All"
                OnClick="Buttonselectall_Click" Font-Bold="true" />
            <asp:Button ID="btnprintmaster" CssClass="floats" runat="server" Text="Print" OnClick="btnprintmaster_Click"
                Font-Bold="true" />
            <Insproplus:PRINTPDF runat="server" ID="Printcontrol" Visible="false" />
            <br />
        </asp:Panel>
        <br />
        <asp:CollapsiblePanelExtender ID="cpeatend" runat="server" TargetControlID="pBodyatendence"
            CollapseControlID="pHeaderatendence" ExpandControlID="pHeaderatendence" Collapsed="true"
            TextLabelID="Labelatend" CollapsedSize="0" ImageControlID="Imagemark" CollapsedImage="../images/right.jpeg"
            ExpandedImage="../images/down.jpeg">
        </asp:CollapsiblePanelExtender>
    </asp:Panel>
    <center>
        <asp:Label ID="lblspecial" runat="server" Font-Bold="True" Font-Size="Medium" ForeColor="Red"
            Text="Special Class Can Not Taken For Particular Date" Visible="False"></asp:Label>
    </center>
</asp:Content>

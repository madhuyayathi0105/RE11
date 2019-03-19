<%@ Page Title="Special Day/Free Hour Entry" Language="C#" MasterPageFile="~/AttendanceMOD/AttendanceSubSiteMaster.master"
    AutoEventWireup="true" CodeFile="AttendanceSplDayFreeHrEntry.aspx.cs" Inherits="AttendanceSplDayFreeHrEntry"
    EnableEventValidation="false" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Assembly="FarPoint.Web.Spread" Namespace="FarPoint.Web.Spread" TagPrefix="FarPoint" %>
<%@ Register Src="~/Usercontrols/PrintMaster.ascx" TagName="printmaster" TagPrefix="Insproplus" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <link href="~/Styles/css/Commoncss.css" rel="Stylesheet" type="text/css" />
    <script src="//ajax.googleapis.com/ajax/libs/jquery/1.11.1/jquery.min.js"></script>
    <script type="text/javascript">
        function frelig() {
            document.getElementById('<%#btnreasonset.ClientID%>').style.display = 'block';
            document.getElementById('<%#btnreasonre.ClientID%>').style.display = 'block';
        }
        function display1() {
            document.getElementById('<%#lbl_norec1.ClientID %>').innerHTML = "";
        }
    </script>
    <style type="text/css">
        .tdLeft
        {
            width: 140px;
            padding: 3px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <center>
        <div>
            <span class="fontstyleheader" style="color: Green; margin: 0px; margin-bottom: 10px;
                margin-top: 10px; position: relative;">Special Day/Free Hour Entry</span>
        </div>
    </center>
    <center>
        <div id="divSearch" runat="server" visible="true" style="width: 100%; height: auto;
            margin: 0px; margin-bottom: 10px; margin-top: 10px; position: relative;">
            <div class="maintablestyle" id="tblsearch" runat="server" style="width: 100%; height: auto;">
                <table>
                    <tr>
                        <td colspan="2">
                            <asp:RadioButtonList ID="rblSplFree" runat="server" AutoPostBack="true" Font-Bold="true"
                                Font-Names="Book Antiqua" Font-Size="Medium" RepeatDirection="Horizontal" OnSelectedIndexChanged="rblSplFree_SelectedIndexChanged">
                                <asp:ListItem Text="Free Hour" Value="0" Selected="True">
                                </asp:ListItem>
                                <asp:ListItem Text="Special Day" Value="1"></asp:ListItem>
                            </asp:RadioButtonList>
                        </td>
                        <td>
                            <asp:CheckBox ID="cbViewOrNot" Text="View Report" runat="server" AutoPostBack="true"
                                Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium" OnCheckedChanged="cbViewOrNot_CheckedChanged" />
                        </td>
                    </tr>
                </table>
                <div style="float: left; width: 45%; margin: 0px;">
                    <table style="width=400px; margin: 0px;">
                        <tr>
                            <td class="tdLeft" colspan="2">
                                <asp:Label ID="lblCollege" runat="server" Text="College" Font-Bold="True" Style="font-family: 'Book Antiqua';"
                                    ForeColor="Black" Font-Size="Medium" Font-Names="Book Antiqua"></asp:Label>
                            </td>
                            <td colspan="2">
                                <asp:DropDownList ID="ddlCollege" runat="server" CssClass="dropdown" Style="font-family: 'Book Antiqua';"
                                    Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium" Width="200px" AutoPostBack="True"
                                    OnSelectedIndexChanged="ddlCollege_SelectedIndexChanged">
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLeft" colspan="2">
                                <asp:Label ID="lblDegree" runat="server" Text="Degree" Style="font-family: 'Book Antiqua';"
                                    Font-Bold="True" ForeColor="Black" Font-Names="Book Antiqua" Font-Size="Medium"></asp:Label>
                            </td>
                            <td colspan="2">
                                <div style="position: relative;">
                                    <asp:UpdatePanel ID="upnlDegree" runat="server">
                                        <ContentTemplate>
                                            <asp:TextBox ID="txtDegree" runat="server" Height="20px" ReadOnly="true" CssClass="dropdown"
                                                Width="193px" Style="font-family: 'Book Antiqua';" Font-Bold="True" Font-Names="Book Antiqua"
                                                Font-Size="Medium">---Select---</asp:TextBox>
                                            <asp:Panel ID="pnlDegree" runat="server" CssClass="multxtpanel" Width="150px" BackColor="White"
                                                BorderColor="Black" BorderStyle="Solid" BorderWidth="2px" Height="200px" Style="font-family: 'Book Antiqua'">
                                                <asp:CheckBox ID="cbDegree" runat="server" Width="100px" Font-Bold="True" Font-Names="Book Antiqua"
                                                    Font-Size="Medium" Text="Select All" AutoPostBack="True" OnCheckedChanged="cbDegree_CheckedChanged" />
                                                <asp:CheckBoxList ID="cblDegree" runat="server" Font-Size="Medium" AutoPostBack="True"
                                                    Width="100px" Height="58px" Font-Bold="True" Font-Names="Book Antiqua" OnSelectedIndexChanged="cblDegree_SelectedIndexChanged">
                                                </asp:CheckBoxList>
                                            </asp:Panel>
                                            <asp:PopupControlExtender ID="popubExtDegree" runat="server" TargetControlID="txtDegree"
                                                PopupControlID="pnlDegree" Position="Bottom">
                                            </asp:PopupControlExtender>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </div>
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLeft" colspan="2">
                                <asp:Label ID="lblsem" runat="server" Text="Semester" Font-Bold="True" ForeColor="Black"
                                    Font-Size="Medium" Font-Names="Book Antiqua">
                                </asp:Label>
                            </td>
                            <td colspan="2">
                                <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                                    <ContentTemplate>
                                        <asp:DropDownList ID="ddlsem" runat="server" Font-Bold="True" Font-Size="Medium"
                                            Font-Names="Book Antiqua" OnSelectedIndexChanged="ddlsem_SelectedIndexChanged"
                                            AutoPostBack="true" Width="200px">
                                        </asp:DropDownList>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLeft" colspan="2">
                                <asp:Label ID="lblFromDate" runat="server" Style="font-family: 'Book Antiqua';" Text="From Date"
                                    Font-Bold="true" Font-Names="Book Antiqua" Font-Size="Medium"></asp:Label>
                            </td>
                            <td colspan="2">
                                <asp:TextBox ID="txtFromDate" CssClass="dropdown" Style="font-family: 'Book Antiqua';"
                                    runat="server" Font-Bold="true" Width="193px" Font-Names="Book Antiqua" Font-Size="Medium"
                                    AutoPostBack="true" OnTextChanged="txtFromDate_TextChanged"></asp:TextBox>
                                <asp:CalendarExtender ID="calExtFDate" TargetControlID="txtFromDate" runat="server"
                                    Format="dd/MM/yyyy">
                                </asp:CalendarExtender>
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLeft">
                                <asp:Label ID="lblReason" runat="server" Style="font-family: 'Book Antiqua';" Text="Reason"
                                    Font-Bold="true" Font-Names="Book Antiqua" Font-Size="Medium"></asp:Label>
                            </td>
                            <td>
                                <asp:Button ID="btnreasonset" runat="server" Text="+" Font-Bold="True" Font-Names="Book Antiqua"
                                    Font-Size="Small" CssClass="textbox btn2" Width="25px" OnClick="btnreasonset_Click"
                                    Style="display: none; position: relative" />
                            </td>
                            <td style="margin: 0px; padding: 0px;">
                                <asp:DropDownList ID="ddlpurpose" runat="server" CssClass="font" AutoPostBack="true"
                                    Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium" OnSelectedIndexChanged="ddlpurpose_SelectedIndexChanged"
                                    Width="200px" onfocus="frelig()">
                                </asp:DropDownList>
                                <asp:CheckBox ID="cbreason" runat="server" Font-Bold="True" Visible="false" AutoPostBack="true"
                                    Font-Names="Book Antiqua" OnCheckedChanged="cbreason_CheckedChanged" Font-Size="Medium" />
                            </td>
                            <td>
                                <asp:Button ID="btnreasonre" runat="server" Text="-" OnClick="btnreasonre_Click"
                                    Font-Bold="True" CssClass="textbox btn2" Width="25px" Font-Names="Book Antiqua"
                                    Font-Size="Medium" Style="display: none; position: relative" />
                            </td>
                        </tr>
                    </table>
                </div>
                <div style="float: left; width: 50%; margin: 0px;">
                    <table style="width=100%; margin: 0px;">
                        <tr>
                            <td>
                                <asp:Label ID="lblBatch" runat="server" Text="Batch" Style="font-family: 'Book Antiqua';"
                                    Font-Bold="True" ForeColor="Black" Font-Size="Medium" Font-Names="Book Antiqua"></asp:Label>
                            </td>
                            <td colspan="2">
                                <asp:DropDownList ID="ddlbatch" runat="server" Font-Bold="True" Font-Size="Medium"
                                    Font-Names="Book Antiqua" OnSelectedIndexChanged="ddlbatch_SelectedIndexChanged"
                                    AutoPostBack="true" Width="152px">
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="lblBranch" runat="server" Style="font-family: 'Book Antiqua';" Text="Branch"
                                    Font-Bold="True" ForeColor="Black" Font-Names="Book Antiqua" Font-Size="Medium"></asp:Label>
                            </td>
                            <td colspan="2">
                                <div style="position: relative;">
                                    <asp:UpdatePanel ID="upnlBranch" runat="server">
                                        <ContentTemplate>
                                            <asp:TextBox ID="txtBranch" runat="server" Height="20px" CssClass="dropdown" ReadOnly="true"
                                                Width="146px" Style="font-family: 'Book Antiqua';" Font-Bold="True" Font-Names="Book Antiqua"
                                                Font-Size="Medium">---Select---</asp:TextBox>
                                            <asp:Panel ID="pnlBranch" runat="server" CssClass="multxtpanel" Height="250px" BackColor="White"
                                                BorderColor="Black" BorderStyle="Solid" BorderWidth="2px" Style="font-family: 'Book Antiqua'">
                                                <asp:CheckBox ID="cbBranch" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                                    Font-Size="Medium" Text="Select All" AutoPostBack="True" OnCheckedChanged="cbBranch_CheckedChanged" />
                                                <asp:CheckBoxList ID="cblBranch" runat="server" Font-Size="Medium" AutoPostBack="True"
                                                    Width="350px" Style="font-family: 'Book Antiqua'" Font-Bold="True" Font-Names="Book Antiqua"
                                                    Height="58px" OnSelectedIndexChanged="cblBranch_SelectedIndexChanged">
                                                </asp:CheckBoxList>
                                            </asp:Panel>
                                            <asp:PopupControlExtender ID="popubExtBranch" runat="server" TargetControlID="txtBranch"
                                                PopupControlID="pnlBranch" Position="Bottom">
                                            </asp:PopupControlExtender>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </div>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="lblSec" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                    Font-Size="Medium" ForeColor="White" Text="Section" Style="display: inline-block;
                                    color: Black; font-family: Book Antiqua; font-size: medium; font-weight: bold;
                                    width: 90px;">
                                </asp:Label>
                            </td>
                            <td colspan="2">
                                <div style="position: relative;">
                                    <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                                        <ContentTemplate>
                                            <asp:TextBox ID="txtsection" runat="server" Height="20px" ReadOnly="true" CssClass="dropdown"
                                                Width="146px" Style="font-family: 'Book Antiqua';" Font-Bold="True" Font-Names="Book Antiqua"
                                                Font-Size="Medium">---Select---</asp:TextBox>
                                            <asp:Panel ID="psection" runat="server" BackColor="White" BorderColor="Black" BorderStyle="Solid"
                                                BorderWidth="2px" Style="font-family: 'Book Antiqua'">
                                                <asp:CheckBox ID="chksection" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                                    Font-Size="Medium" Text="Select All" OnCheckedChanged="chksection_CheckedChanged"
                                                    AutoPostBack="True" />
                                                <asp:CheckBoxList ID="chklstsection" runat="server" Font-Size="Medium" AutoPostBack="True"
                                                    Style="font-family: 'Book Antiqua'" OnSelectedIndexChanged="chklstsection_SelectedIndexChanged"
                                                    Font-Bold="True" Font-Names="Book Antiqua">
                                                </asp:CheckBoxList>
                                            </asp:Panel>
                                            <asp:PopupControlExtender ID="PopupControlExtender5" runat="server" TargetControlID="txtsection"
                                                PopupControlID="psection" Position="Bottom">
                                            </asp:PopupControlExtender>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </div>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="lblToDate" runat="server" Style="font-family: 'Book Antiqua';" Text="To Date"
                                    Font-Bold="true" Font-Names="Book Antiqua" Font-Size="Medium"></asp:Label>
                                <asp:Label ID="lblPeriod" runat="server" Text="Period" Style="font-family: 'Book Antiqua';"
                                    Font-Bold="True" ForeColor="Black" Font-Names="Book Antiqua" Font-Size="Medium"></asp:Label>
                            </td>
                            <td style="margin: 0px; padding: 0px;">
                                <asp:TextBox ID="txtToDate" CssClass="dropdown" Style="font-family: 'Book Antiqua';"
                                    runat="server" Font-Bold="true" Width="124px" Font-Names="Book Antiqua" Font-Size="Medium"
                                    AutoPostBack="true" OnTextChanged="txtToDate_TextChanged"></asp:TextBox>
                                <asp:CalendarExtender ID="calExtTDate" TargetControlID="txtToDate" runat="server"
                                    Format="dd/MM/yyyy">
                                </asp:CalendarExtender>
                                <asp:UpdatePanel ID="upnlPeriod" runat="server">
                                    <ContentTemplate>
                                        <asp:TextBox ID="txtPeriod" runat="server" Height="20px" CssClass="dropdown" ReadOnly="true"
                                            Width="124px" Style="font-family: 'Book Antiqua';" Font-Bold="True" Font-Names="Book Antiqua"
                                            Font-Size="Medium">---Select---</asp:TextBox>
                                        <asp:Panel ID="pnlPeriod" runat="server" BackColor="White" BorderColor="Black" BorderStyle="Solid"
                                            BorderWidth="2px" Style="font-family: 'Book Antiqua'">
                                            <asp:CheckBox ID="cbPeriod" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                                Font-Size="Medium" Text="Select All" AutoPostBack="True" OnCheckedChanged="cbPeriod_CheckedChanged" />
                                            <asp:CheckBoxList ID="cblPeriod" runat="server" Font-Size="Medium" AutoPostBack="True"
                                                Style="font-family: 'Book Antiqua'" Font-Bold="True" Font-Names="Book Antiqua"
                                                Height="58px" OnSelectedIndexChanged="cblPeriod_SelectedIndexChanged">
                                            </asp:CheckBoxList>
                                        </asp:Panel>
                                        <asp:PopupControlExtender ID="popupExtPeriod" runat="server" TargetControlID="txtPeriod"
                                            PopupControlID="pnlPeriod" Position="Bottom">
                                        </asp:PopupControlExtender>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>
                            <td>
                                <asp:CheckBox ID="cbPeriods" runat="server" Font-Bold="True" Visible="false" AutoPostBack="true"
                                    Font-Names="Book Antiqua" OnCheckedChanged="cbPeriods_CheckedChanged" Font-Size="Medium"
                                    Style="margin: 0px; padding: 0px;" />
                                <%--Visible="false" --%>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="lblAttBasedon" runat="server" Style="font-family: 'Book Antiqua';"
                                    Text="Attendance Based On" Font-Bold="true" Font-Names="Book Antiqua" Font-Size="Medium"></asp:Label>
                            </td>
                            <td colspan="2">
                                <asp:DropDownList ID="ddlAttBaseon" runat="server" Font-Bold="True" Font-Size="Medium"
                                    Font-Names="Book Antiqua" OnSelectedIndexChanged="ddlAttBaseon_SelectedIndexChanged"
                                    AutoPostBack="true" Width="152px">
                                </asp:DropDownList>
                            </td>
                        </tr>
                    </table>
                </div>
                <center style="clear: both; float: none;">
                    <asp:Button ID="btnView" runat="server" Font-Bold="True" Font-Size="Medium" Font-Names="Book Antiqua"
                        Width="59px" CssClass="textbox btn2" Text="View" OnClick="btnView_Click" />
                    <asp:Button ID="btnSave" runat="server" Font-Bold="True" Font-Size="Medium" Font-Names="Book Antiqua"
                        Width="59px" CssClass="textbox btn2" Text="Save" OnClick="btnSave_Click" />
                </center>
            </div>
        </div>
    </center>
    <asp:Label ID="lblErrSearch" runat="server" Text="" ForeColor="Red" Visible="False"
        Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium" Style="margin: 0px;
        margin-bottom: 10px; margin-top: 10px; position: relative;"></asp:Label>
    <center>
        <div id="divViewSpread" runat="server" visible="false" style="margin: 0px; margin-bottom: 10px;
            margin-top: 28px; position: relative;">
            <FarPoint:FpSpread ID="FpViewSpread" AutoPostBack="false" Width="1000px" runat="server"
                Visible="true" BorderStyle="Solid" BorderWidth="0px" CssClass="spreadborder"
                ShowHeaderSelection="false" OnButtonCommand="FpViewSpread_ButtonCommand" Style="width: 100%;
                height: auto; margin: 0px; margin-bottom: 10px; margin-top: 10px;">
                <Sheets>
                    <FarPoint:SheetView SheetName="Sheet1">
                    </FarPoint:SheetView>
                </Sheets>
            </FarPoint:FpSpread>
            <center>
               
            </center>
        </div>
    </center>
    <center>
        <div id="rptprint1" runat="server" visible="false" style="margin: 0px; margin-bottom: 10px;
            margin-top: 10px;">
            <table>
                <tr>
                    <td colspan="5" align="center">
                        <asp:Label ID="lbl_norec1" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                            Font-Size="Medium" ForeColor="Red" Text="Please Enter Your Report Name" Visible="false"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="lblrptname1" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                            Font-Size="Medium" Text="Report Name"></asp:Label>
                    </td>
                    <td>
                        <asp:TextBox ID="txtexcelname1" runat="server" CssClass="textbox textbox1" Height="20px"
                            Width="180px" Style="font-family: 'Book Antiqua'" Font-Bold="True" Font-Names="Book Antiqua"
                            onkeypress="display1()" Font-Size="Medium"></asp:TextBox>
                        <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender9" runat="server" TargetControlID="txtexcelname1"
                            FilterType="LowercaseLetters,UppercaseLetters,Numbers,Custom" ValidChars="(),.[]_"
                            InvalidChars="/\">
                        </asp:FilteredTextBoxExtender>
                    </td>
                    <td>
                        <asp:Button ID="btnExcel1" runat="server" Font-Bold="true" Font-Names="Book Antiqua"
                            OnClick="btnExcel1_Click" Font-Size="Medium" Text="Export To Excel" Width="127px"
                            Height="35px" CssClass="textbox textbox1" />
                    </td>
                    <td>
                        <asp:Button ID="btnprintmaster1" runat="server" Text="Print" OnClick="btnprintmaster1_Click"
                            Font-Names="Book Antiqua" Font-Size="Medium" Font-Bold="true" Width="60px" Height="35px"
                            CssClass="textbox textbox1" />
                        <Insproplus:printmaster runat="server" ID="Printcontrol1" Visible="false" />
                    </td>
                    <td>
                        <asp:Button ID="btnDeleteFreeSpl" runat="server" Text="Delete" OnClick="btnDeleteFreeSpl_Click"
                            Font-Names="Book Antiqua" Font-Size="Medium" Font-Bold="true" Width="60px" Height="35px"
                            CssClass="textbox textbox1" Style="margin: 0px; margin-bottom: 10px; margin-top: 10px;" />
                    </td>
                </tr>
            </table>
        </div>
    </center>
    <center>
        <div id="popupdiv" runat="server" visible="false" style="height: 100%; z-index: 1000;
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
                                    <asp:Label ID="lblpoperr" runat="server" Text="" Style="color: Red;" Font-Bold="true"
                                        Font-Size="Medium" Font-Names="Book Antiqua"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <center>
                                        <asp:Button ID="btn_errorclose" runat="server" CssClass=" textbox btn1 comm" Font-Size="Medium"
                                            Font-Bold="True" Font-Names="Book Antiqua" Style="height: 28px; width: 65px;"
                                            OnClick="btn_errorclose_Click" Text="Ok" />
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
        <div id="divAddReason" runat="server" visible="false" style="height: 100%; z-index: 1000;
            width: 100%; background-color: rgba(54, 25, 25, .2); position: absolute; top: 0;
            left: 0px;">
            <center>
                <div id="pnlAdd" runat="server" class="table" style="background-color: White; height: 120px;
                    width: 650px; border: 5px solid #0CA6CA; border-top: 25px solid #0CA6CA; margin-top: 200px;
                    border-radius: 10px;">
                    <center>
                        <table style="height: 100px; width: 100%">
                            <tr>
                                <td>
                                    <asp:Label ID="lblAddReason" runat="server" Text="Add Reason" Style="color: Black;"
                                        Font-Bold="true" Font-Size="Medium" Font-Names="Book Antiqua"></asp:Label>
                                </td>
                                <td align="center">
                                    <asp:TextBox ID="txtAddReason" runat="server" MaxLength="50" CssClass="textbox textbox1"
                                        Text="" Style="color: Black;" Font-Bold="true" Font-Size="Medium" Font-Names="Book Antiqua"
                                        Width="430px" Height="20px"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2" align="center">
                                    <asp:Button ID="btnAddReason" runat="server" CssClass=" textbox btn1 comm" Font-Size="Medium"
                                        Font-Bold="True" Font-Names="Book Antiqua" Style="height: 28px; width: 65px;"
                                        OnClick="btnAddReason_Click" Text="Add" />
                                    <asp:Button ID="btnReasonExit" runat="server" CssClass=" textbox btn1 comm" Font-Size="Medium"
                                        Font-Bold="True" Font-Names="Book Antiqua" Style="height: 28px; width: 65px;"
                                        OnClick="btnReasonExit_Click" Text="Exit" />
                                </td>
                            </tr>
                        </table>
                    </center>
                </div>
            </center>
        </div>
    </center>
</asp:Content>

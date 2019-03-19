<%@ Page Title="" Language="C#" MasterPageFile="~/AttendanceMOD/AttendanceSubSiteMaster.master"
    AutoEventWireup="true" CodeFile="dailystudentattndreport.aspx.cs" Inherits="dailystudentattndreport"
    EnableEventValidation="false" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Assembly="FarPoint.Web.Spread" Namespace="FarPoint.Web.Spread" TagPrefix="FarPoint" %>
<%@ Register Src="~/Usercontrols/PrintMaster.ascx" TagName="printmaster" TagPrefix="Insproplus" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <link href="~/Styles/css/Registration.css" rel="stylesheet" type="text/css" />
    <script src="//ajax.googleapis.com/ajax/libs/jquery/1.11.1/jquery.min.js"></script>
    <link href="~/Styles/css/Commoncss.css" rel="Stylesheet" type="text/css" />
    <script type="text/javascript">
        function display() {

            document.getElementById('MainContent_lblnorec').innerHTML = "";

        }
        function display1() {
            document.getElementById('<%=lbl_norec1.ClientID %>').innerHTML = "";
        }

    </script>
    <style type="text/css">
        .style2
        {
            width: 729px;
        }
        .cursorptr
        {
            cursor: default;
        }
        .txt
        {
        }
        .style4
        {
            width: auto;
        }
        .style5
        {
            width: auto;
        }
        .style6
        {
            width: 119px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <center>
        <span class="fontstyleheader" style="color: Green; margin-top: 10px; margin-bottom: 10px;
            position: relative;">AT09-Individual Student Attendance Report</span>
    </center>
    <div>
        <center>
            <table class="maintablestyle" style="width: auto; height: auto; margin-top: 10px;
                margin-bottom: 10px; position: relative;">
                <tr>
                    <td>
                        <asp:Label ID="lblddl" runat="server" Text="Select Option" Font-Bold="True" Font-Names="Book Antiqua"
                            Font-Size="Medium"></asp:Label>
                    </td>
                    <td>
                        <asp:DropDownList ID="optionddl" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                            Font-Size="Medium" Width="147px" AutoPostBack="True" OnSelectedIndexChanged="optionddl_SelectedIndexChanged">
                            <asp:ListItem>Roll No.</asp:ListItem>
                            <asp:ListItem>Reg No.</asp:ListItem>
                            <asp:ListItem>Admission No.</asp:ListItem>
                        </asp:DropDownList>
                    </td>
                    <td>
                        <asp:Label ID="lblrollno" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                            Font-Size="Medium"></asp:Label>
                    </td>
                    <td>
                        <asp:TextBox ID="txtrollno" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                            Font-Size="Medium" OnTextChanged="txtrollno_TextChanged" AutoPostBack="True"></asp:TextBox>
                    </td>
                    <td>
                        <asp:Label ID="lblname1" runat="server" Text="Student Name: " Font-Bold="True" Font-Names="Book Antiqua"
                            Font-Size="Medium"></asp:Label>
                    </td>
                    <td>
                        <asp:Label ID="lblname2" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                            Font-Size="Medium" ForeColor="BlueViolet"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td colspan="1">
                        <table>
                            <tr>
                                <td>
                                    <asp:Label ID="lblSem" runat="server" Text="Sem" Font-Bold="True" Font-Names="Book Antiqua"
                                        Font-Size="Medium" AssociatedControlID=""></asp:Label>
                                </td>
                                <td>
                                    <asp:DropDownList ID="ddlSem" runat="server" Font-Bold="true" Font-Names="Book Antiqua"
                                        Font-Size="Medium" Width="50Px" AutoPostBack="true" CssClass="arrow" OnSelectedIndexChanged="ddlSem_SelectedIndexChanged">
                                    </asp:DropDownList>
                                </td>
                                <td>
                                </td>
                            </tr>
                        </table>
                    </td>
                    <td colspan="5">
                        <table>
                            <tr>
                                <td>
                                    <asp:Label ID="lblFromdate" runat="server" Text="From Date" Font-Bold="True" Font-Names="Book Antiqua"
                                        Font-Size="Medium"></asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtFromDate" CssClass="txt" runat="server" Height="21px" Width="75px"
                                        Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium" OnTextChanged="txtFromDate_TextChanged"
                                        AutoPostBack="True"></asp:TextBox>
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
                                    <asp:TextBox ID="txtToDate" CssClass="txt" runat="server" Height="21px" Width="75px"
                                        Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium" OnTextChanged="txtToDate_TextChanged"
                                        AutoPostBack="True"></asp:TextBox>
                                    <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" FilterType="Custom,Numbers"
                                        ValidChars="/" runat="server" TargetControlID="txtToDate">
                                    </asp:FilteredTextBoxExtender>
                                    <asp:CalendarExtender ID="CalendarExtender1" TargetControlID="txtToDate" Format="d/MM/yyyy"
                                        runat="server">
                                    </asp:CalendarExtender>
                                </td>
                                <td>
                                    <asp:Button ID="btnGo" runat="server" Text="Go" OnClick="btnGo_Click" Font-Bold="True"
                                        Font-Names="Book Antiqua" Font-Size="Medium" Height="26px" />
                                </td>
                                <td>
                                    <asp:Label ID="dateerrlbl" runat="server" ForeColor="Red" Font-Bold="True" Font-Names="Book Antiqua"
                                        Font-Size="Medium"></asp:Label>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
            </table>
            <div>
                <asp:Label ID="lbl_error" runat="server" Visible="false" ForeColor="Red" Style="margin: 0px;
                    margin-bottom: 10px; margin-top: 10px; position: relative;"></asp:Label>
            </div>
            <table style="margin: 0px; margin-bottom: 10px; margin-top: 10px; position: relative;">
                <tr>
                    <td class="style1" align="center">
                        <asp:Label ID="errlbl" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                            Font-Size="Medium" ForeColor="Red"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Panel ID="pageset_pnl" runat="server" BorderStyle="None" Width="1026px">
                            <asp:Label ID="Buttontotal" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                Font-Size="Medium"></asp:Label>
                            <asp:Label ID="ddlpagelbl" runat="server" Font-Bold="True" Text="     Records Per Page"
                                Font-Names="Book Antiqua" Font-Size="Medium"></asp:Label>
                            <asp:DropDownList ID="DropDownListpage" runat="server" Height="25px" Width="65px"
                                Font-Bold="True" Font-Size="Medium" Font-Names="Book Antiqua" OnSelectedIndexChanged="DropDownListpage_SelectedIndexChanged"
                                AutoPostBack="True">
                            </asp:DropDownList>
                            <asp:TextBox ID="pageddltxt" runat="server" Height="22px" Width="40px" Font-Bold="True"
                                Font-Size="Medium" Font-Names="Book Antiqua" OnTextChanged="pageddltxt_TextChanged"
                                AutoPostBack="True"></asp:TextBox>
                            <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender3" runat="server" FilterType="Numbers"
                                TargetControlID="pageddltxt">
                            </asp:FilteredTextBoxExtender>
                            <asp:Label ID="pgsearch_lbl" runat="server" Font-Bold="True" Text="Page Search" Width="95px"
                                Font-Names="Book Antiqua" Font-Size="Medium" Height="24px"></asp:Label>
                            <asp:TextBox ID="pagesearch_txt" runat="server" AutoPostBack="True" Font-Bold="True"
                                Font-Names="Book Antiqua" Font-Size="Medium" Height="22px" Width="40px" OnTextChanged="pagesearch_txt_TextChanged"></asp:TextBox>
                            <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender2" runat="server" TargetControlID="pagesearch_txt"
                                FilterType="Numbers">
                            </asp:FilteredTextBoxExtender>
                            <asp:CheckBox ID="viewattendall" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                Font-Size="Medium" OnCheckedChanged="viewattendall_CheckedChanged" Text="View Attendance Header"
                                AutoPostBack="True" Checked="true" />
                            <asp:Label ID="errmsg" runat="server" ForeColor="Red" Font-Bold="True" Font-Names="Book Antiqua"
                                Font-Size="Medium" Height="19px" Width="496px"></asp:Label>
                        </asp:Panel>
                    </td>
                </tr>
                <tr>
                    <td>
                        <div id="std_info" runat="server" visible="false" style="margin: 0px; margin-bottom: 10px;
                            margin-top: 10px;">
                            <fieldset style="border-radius: 6px; background-color: MediumSlateBlue; width: 960px;
                                height: auto;">
                                <table style="width: 935px;">
                                    <tr>
                                        <td>
                                            <asp:Label ID="lbl_name" runat="server" Text="Name" Font-Bold="True" Font-Names="Book Antiqua"
                                                Font-Size="Medium"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:Label ID="name" runat="server" Text="" ForeColor="blue" Font-Bold="True" Font-Names="Book Antiqua"
                                                Font-Size="Medium"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:Label ID="lbl_class" runat="server" Text="Class" Font-Bold="True" Font-Names="Book Antiqua"
                                                Font-Size="Medium"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:Label ID="clas" runat="server" Text="" ForeColor="blue" Font-Bold="True" Font-Names="Book Antiqua"
                                                Font-Size="Medium"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Label ID="lbl_fullday" runat="server" Text="Full Day Absent" Font-Bold="True"
                                                Font-Names="Book Antiqua" Font-Size="Medium"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:Label ID="fullday" runat="server" Text="" ForeColor="blue" Font-Bold="True"
                                                Font-Names="Book Antiqua" Font-Size="Medium"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:Label ID="lbl_halfday" runat="server" Text="Half Day Absent" Font-Bold="True"
                                                Font-Names="Book Antiqua" Font-Size="Medium"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:Label ID="halfday" runat="server" Text="" ForeColor="blue" Font-Bold="True"
                                                Font-Names="Book Antiqua" Font-Size="Medium"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Label ID="lbl_totdays" runat="server" Text="Total Days Absent" Font-Bold="True"
                                                Font-Names="Book Antiqua" Font-Size="Medium"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:Label ID="totdays" runat="server" Text="" ForeColor="blue" Font-Bold="True"
                                                Font-Names="Book Antiqua" Font-Size="Medium"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:Label ID="lbl_odapplied" runat="server" Text="OD Applied" Font-Bold="True" Font-Names="Book Antiqua"
                                                Font-Size="Medium"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:Label ID="odapplied" runat="server" Text="" ForeColor="blue" Font-Bold="True"
                                                Font-Names="Book Antiqua" Font-Size="Medium"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Label ID="lbl_leaveapplied" runat="server" Text="Leave Applied" Font-Bold="True"
                                                Font-Names="Book Antiqua" Font-Size="Medium"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:Label ID="leaveapplied" runat="server" Text="" ForeColor="blue" Font-Bold="True"
                                                Font-Names="Book Antiqua" Font-Size="Medium"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:Label ID="lbl_lastattndate" runat="server" Text="Last Attended Date" Font-Bold="True"
                                                Font-Names="Book Antiqua" Font-Size="Medium"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:Label ID="lastattndate" runat="server" Text="" ForeColor="blue" Font-Bold="True"
                                                Font-Names="Book Antiqua" Font-Size="Medium"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Label ID="lblHourPercentage" runat="server" Text="Hour Wise Percentage" Font-Bold="True"
                                                Font-Names="Book Antiqua" Font-Size="Medium"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:Label ID="lblHrsWisePercentage" runat="server" Text="" ForeColor="blue" Font-Bold="True"
                                                Font-Names="Book Antiqua" Font-Size="Medium"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:Label ID="lblDayPercentage" runat="server" Text="Day Wise Percentage" Font-Bold="True"
                                                Font-Names="Book Antiqua" Font-Size="Medium"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:Label ID="lblDaysWisePercentage" runat="server" Text="" ForeColor="blue" Font-Bold="True"
                                                Font-Names="Book Antiqua" Font-Size="Medium"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                    <td>
                                            <asp:Label ID="lbldiscontinu" runat="server" Text="Discontinue Date" Font-Bold="True" Visible="false"
                                                Font-Names="Book Antiqua" Font-Size="Medium"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:Label ID="lbdiscontinue" runat="server" Text="" ForeColor="blue" Font-Bold="True" Visible="false"
                                                Font-Names="Book Antiqua" Font-Size="Medium"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:Label ID="lblread" runat="server" Text="Readmission Date" Font-Bold="True" Visible="false"
                                                Font-Names="Book Antiqua" Font-Size="Medium"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:Label ID="lblreadmission" runat="server" Text="" ForeColor="blue" Font-Bold="True" Visible="false"
                                                Font-Names="Book Antiqua" Font-Size="Medium"></asp:Label>
                                        </td>
                                    </tr>
                                </table>
                            </fieldset>
                        </div>
                    </td>
                </tr>
                <tr>
                    <td align="center">
                        <div runat="server" id="divNote" visible="false">
                            <table style="width: auto; height: auto; margin: 0px; margin-bottom: 10px; margin-top: 10px;">
                                <tr>
                                    <td style="width: 10px; padding: 5px; background-color: #008000;">
                                    </td>
                                    <td style="width: auto; padding-left: 3px; padding-right: 3px;">
                                        -
                                    </td>
                                    <td style="width: auto; padding-left: 3px; padding-right: 10px;">
                                        P -Present
                                    </td>
                                    <td style="width: 10px; padding: 5px; background-color: #FF0000;">
                                    </td>
                                    <td style="width: auto; padding-left: 3px; padding-right: 3px;">
                                        -
                                    </td>
                                    <td style="width: auto; padding-left: 3px; padding-right: 10px;">
                                        A -Absent
                                    </td>
                                    <td style="width: 10px; padding: 5px; background-color: #800000;">
                                    </td>
                                    <td style="width: auto; padding-left: 3px; padding-right: 3px;">
                                        -
                                    </td>
                                    <td style="width: auto; padding-left: 3px; padding-right: 10px;">
                                        H -Holiday
                                    </td>
                                    <td style="width: 10px; padding: 5px; background-color: #0000FF;">
                                    </td>
                                    <td style="width: auto; padding-left: 3px; padding-right: 3px;">
                                        -
                                    </td>
                                    <td style="width: auto; padding-left: 3px; padding-right: 10px;">
                                        OD -Onduty
                                    </td>
                                    <td style="width: 10px; padding: 5px; background-color: #adff2f;">
                                    </td>
                                    <td style="width: auto; padding-left: 3px; padding-right: 3px;">
                                        -
                                    </td>
                                    <td style="width: auto; padding-left: 3px; padding-right: 10px;">
                                        SOD
                                    </td>
                                    <td style="width: 10px; padding: 5px; background-color: #E9967A;">
                                    </td>
                                    <td style="width: auto; padding-left: 3px; padding-right: 3px;">
                                        -
                                    </td>
                                    <td style="width: auto; padding-left: 3px; padding-right: 10px;">
                                        ML -Medical Leave
                                    </td>
                                </tr>
                                <tr>
                                    <td style="width: 10px; padding: 5px; background-color: #DAA520;">
                                    </td>
                                    <td style="width: auto; padding-left: 3px; padding-right: 3px;">
                                        -
                                    </td>
                                    <td style="width: auto; padding-left: 3px; padding-right: 10px;">
                                        NSS
                                    </td>
                                    <td style="width: 10px; padding: 5px; background-color: #0080ff;">
                                    </td>
                                    <td style="width: auto; padding-left: 3px; padding-right: 3px;">
                                        -
                                    </td>
                                    <td style="width: auto; padding-left: 3px; padding-right: 10px;">
                                        L -Leave
                                    </td>
                                    <td style="width: 10px; padding: 5px; background-color: #EE82EE;">
                                    </td>
                                    <td style="width: auto; padding-left: 3px; padding-right: 3px;">
                                        -
                                    </td>
                                    <td style="width: auto; padding-left: 3px; padding-right: 10px;">
                                        NCC
                                    </td>
                                    <td style="width: 10px; padding: 5px; background-color: #708090;">
                                    </td>
                                    <td style="width: auto; padding-left: 3px; padding-right: 3px;">
                                        -
                                    </td>
                                    <td style="width: auto; padding-left: 3px; padding-right: 10px;">
                                        HS
                                    </td>
                                    <td style="width: 10px; padding: 5px; background-color: #FFC0CB;">
                                    </td>
                                    <td style="width: auto; padding-left: 3px; padding-right: 3px;">
                                        -
                                    </td>
                                    <td style="width: auto; padding-left: 3px; padding-right: 10px;">
                                        PP
                                    </td>
                                    <td style="width: 10px; padding: 5px; background-color: #32cd32;">
                                    </td>
                                    <td style="width: auto; padding-left: 3px; padding-right: 3px;">
                                        -
                                    </td>
                                    <td style="width: auto; padding-left: 3px; padding-right: 10px;">
                                        SYOD
                                    </td>
                                </tr>
                                <tr>
                                    <td style="width: 10px; padding: 5px; background-color: #D2B48C;">
                                    </td>
                                    <td style="width: auto; padding-left: 3px; padding-right: 3px;">
                                        -
                                    </td>
                                    <td style="width: auto; padding-left: 3px; padding-right: 10px;">
                                        COD
                                    </td>
                                    <td style="width: 10px; padding: 5px; background-color: #f5deb3;">
                                    </td>
                                    <td style="width: auto; padding-left: 3px; padding-right: 3px;">
                                        -
                                    </td>
                                    <td style="width: auto; padding-left: 3px; padding-right: 10px;">
                                        OOD
                                    </td>
                                    <td style="width: 10px; padding: 5px; background-color: #8b4513;">
                                    </td>
                                    <td style="width: auto; padding-left: 3px; padding-right: 3px;">
                                        -
                                    </td>
                                    <td style="width: auto; padding-left: 3px; padding-right: 10px;">
                                        NJ -Not Join
                                    </td>
                                    <td style="width: 10px; padding: 5px; background-color: #000000;">
                                    </td>
                                    <td style="width: auto; padding-left: 3px; padding-right: 3px;">
                                        -
                                    </td>
                                    <td style="width: auto; padding-left: 3px; padding-right: 10px;">
                                        S
                                    </td>
                                    <td style="width: 10px; padding: 5px; background-color: #FFFF00;">
                                    </td>
                                    <td style="width: auto; padding-left: 3px; padding-right: 3px;">
                                        -
                                    </td>
                                    <td style="width: auto; padding-left: 3px; padding-right: 10px;">
                                        RAA
                                    </td>
                                    <td style="width: 10px; padding: 5px; background-color: #FF00FF;">
                                    </td>
                                    <td style="width: auto; padding-left: 3px; padding-right: 3px;">
                                        -
                                    </td>
                                    <td style="width: auto; padding-left: 3px; padding-right: 10px;">
                                        FH -Free Hour
                                    </td>
                                </tr>
                                <tr>
                                    <td style="width: 10px; padding: 5px; background-color: #432F5C;">
                                    </td>
                                    <td style="width: auto; padding-left: 3px; padding-right: 3px;">
                                        -
                                    </td>
                                    <td style="width: auto; padding-left: 3px; padding-right: 10px;">
                                        SH -Special Hour
                                    </td>
                                    <td style="width: 10px; padding: 5px; background-color: #7B68EE;">
                                    </td>
                                    <td style="width: auto; padding-left: 3px; padding-right: 3px;">
                                        -
                                    </td>
                                    <td style="width: auto; padding-left: 3px; padding-right: 10px;">
                                        NE -Not Enter
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="18">
                                        <fieldset style="width: auto; height: auto;">
                                            <legend style="font-weight:bold;">Attendance Status </legend>
                                            <table>
                                                <tr>
                                                    <td style="width: 10px; padding: 5px; background-color: #008000;">
                                                    </td>
                                                    <td style="width: auto; padding-left: 3px; padding-right: 3px;">
                                                        -
                                                    </td>
                                                    <td style="width: auto; padding-left: 3px; padding-right: 10px;">
                                                        FP -Full Present
                                                    </td>
                                                    <td style="width: 10px; padding: 5px; background-color: #FFC0CB;">
                                                    </td>
                                                    <td style="width: auto; padding-left: 3px; padding-right: 3px;">
                                                        -
                                                    </td>
                                                    <td style="width: auto; padding-left: 3px; padding-right: 10px;">
                                                        HP -Half Present
                                                    </td>
                                                    <td style="width: 10px; padding: 5px; background-color: #FF0000;">
                                                    </td>
                                                    <td style="width: auto; padding-left: 3px; padding-right: 3px;">
                                                        -
                                                    </td>
                                                    <td style="width: auto; padding-left: 3px; padding-right: 10px;">
                                                        FA -Full Absent
                                                    </td>
                                                    <td style="width: 10px; padding: 5px; background-color: #D2691E;">
                                                    </td>
                                                    <td style="width: auto; padding-left: 3px; padding-right: 3px;">
                                                        -
                                                    </td>
                                                    <td style="width: auto; padding-left: 3px; padding-right: 10px;">
                                                        HA -Half Absent
                                                    </td>
                                                </tr>
                                            </table>
                                        </fieldset>
                                    </td>
                                </tr>
                            </table>
                        </div>
                    </td>
                </tr>
                <tr>
                    <td>
                        <center style="width: 994px">
                            <FarPoint:FpSpread ID="attnd_report" runat="server" BorderColor="Black" BorderStyle="Solid"
                                BorderWidth="1px" Height="545px" Width="550" Visible="False" ActiveSheetViewIndex="0"
                                OnUpdateCommand="attnd_report_UpdateCommand" VerticalScrollBarPolicy="Never"
                                HorizontalScrollBarPolicy="Never" CommandBar-Visible="false" Style="margin: 0px;
                                margin-bottom: 10px; margin-top: 10px;" ShowHeaderSelection="false">
                                <CommandBar BackColor="Control" ShowPDFButton="true" ButtonType="PushButton" ButtonFaceColor="Control"
                                    ButtonHighlightColor="ControlLightLight" ButtonShadowColor="ControlDark">
                                    <Background BackgroundImageUrl="SPREADCLIENTPATH:/img/cbbg.gif"></Background>
                                </CommandBar>
                                <Sheets>
                                    <FarPoint:SheetView SheetName="Sheet1" AutoPostBack="True" DesignString="&lt;?xml version=&quot;1.0&quot; encoding=&quot;utf-8&quot;?&gt;&lt;Sheet&gt;&lt;Data&gt;&lt;RowHeader class=&quot;FarPoint.Web.Spread.Model.DefaultSheetDataModel&quot; rows=&quot;3&quot; columns=&quot;1&quot;&gt;&lt;AutoCalculation&gt;True&lt;/AutoCalculation&gt;&lt;AutoGenerateColumns&gt;True&lt;/AutoGenerateColumns&gt;&lt;ReferenceStyle&gt;A1&lt;/ReferenceStyle&gt;&lt;Iteration&gt;False&lt;/Iteration&gt;&lt;MaximumIterations&gt;1&lt;/MaximumIterations&gt;&lt;MaximumChange&gt;0.001&lt;/MaximumChange&gt;&lt;/RowHeader&gt;&lt;ColumnHeader class=&quot;FarPoint.Web.Spread.Model.DefaultSheetDataModel&quot; rows=&quot;1&quot; columns=&quot;4&quot;&gt;&lt;AutoCalculation&gt;True&lt;/AutoCalculation&gt;&lt;AutoGenerateColumns&gt;True&lt;/AutoGenerateColumns&gt;&lt;ReferenceStyle&gt;A1&lt;/ReferenceStyle&gt;&lt;Iteration&gt;False&lt;/Iteration&gt;&lt;MaximumIterations&gt;1&lt;/MaximumIterations&gt;&lt;MaximumChange&gt;0.001&lt;/MaximumChange&gt;&lt;/ColumnHeader&gt;&lt;DataArea class=&quot;FarPoint.Web.Spread.Model.DefaultSheetDataModel&quot; rows=&quot;3&quot; columns=&quot;4&quot;&gt;&lt;AutoCalculation&gt;True&lt;/AutoCalculation&gt;&lt;AutoGenerateColumns&gt;True&lt;/AutoGenerateColumns&gt;&lt;ReferenceStyle&gt;A1&lt;/ReferenceStyle&gt;&lt;Iteration&gt;False&lt;/Iteration&gt;&lt;MaximumIterations&gt;1&lt;/MaximumIterations&gt;&lt;MaximumChange&gt;0.001&lt;/MaximumChange&gt;&lt;SheetName&gt;Sheet1&lt;/SheetName&gt;&lt;/DataArea&gt;&lt;SheetCorner class=&quot;FarPoint.Web.Spread.Model.DefaultSheetDataModel&quot; rows=&quot;1&quot; columns=&quot;1&quot;&gt;&lt;AutoCalculation&gt;True&lt;/AutoCalculation&gt;&lt;AutoGenerateColumns&gt;True&lt;/AutoGenerateColumns&gt;&lt;ReferenceStyle&gt;A1&lt;/ReferenceStyle&gt;&lt;Iteration&gt;False&lt;/Iteration&gt;&lt;MaximumIterations&gt;1&lt;/MaximumIterations&gt;&lt;MaximumChange&gt;0.001&lt;/MaximumChange&gt;&lt;/SheetCorner&gt;&lt;ColumnFooter class=&quot;FarPoint.Web.Spread.Model.DefaultSheetDataModel&quot; rows=&quot;1&quot; columns=&quot;4&quot;&gt;&lt;AutoCalculation&gt;True&lt;/AutoCalculation&gt;&lt;AutoGenerateColumns&gt;True&lt;/AutoGenerateColumns&gt;&lt;ReferenceStyle&gt;A1&lt;/ReferenceStyle&gt;&lt;Iteration&gt;False&lt;/Iteration&gt;&lt;MaximumIterations&gt;1&lt;/MaximumIterations&gt;&lt;MaximumChange&gt;0.001&lt;/MaximumChange&gt;&lt;/ColumnFooter&gt;&lt;/Data&gt;&lt;Presentation&gt;&lt;AxisModels&gt;&lt;Column class=&quot;FarPoint.Web.Spread.Model.DefaultSheetAxisModel&quot; orientation=&quot;Horizontal&quot; count=&quot;4&quot;&gt;&lt;Items&gt;&lt;Item index=&quot;-1&quot;&gt;&lt;SortIndicator&gt;Ascending&lt;/SortIndicator&gt;&lt;/Item&gt;&lt;/Items&gt;&lt;/Column&gt;&lt;RowHeaderColumn class=&quot;FarPoint.Web.Spread.Model.DefaultSheetAxisModel&quot; defaultSize=&quot;40&quot; orientation=&quot;Horizontal&quot; count=&quot;1&quot;&gt;&lt;Items&gt;&lt;Item index=&quot;-1&quot;&gt;&lt;SortIndicator&gt;Ascending&lt;/SortIndicator&gt;&lt;Size&gt;40&lt;/Size&gt;&lt;/Item&gt;&lt;/Items&gt;&lt;/RowHeaderColumn&gt;&lt;ColumnHeaderRow class=&quot;FarPoint.Web.Spread.Model.DefaultSheetAxisModel&quot; defaultSize=&quot;22&quot; orientation=&quot;Vertical&quot; count=&quot;1&quot;&gt;&lt;Items&gt;&lt;Item index=&quot;-1&quot;&gt;&lt;Size&gt;22&lt;/Size&gt;&lt;/Item&gt;&lt;/Items&gt;&lt;/ColumnHeaderRow&gt;&lt;ColumnFooterRow class=&quot;FarPoint.Web.Spread.Model.DefaultSheetAxisModel&quot; defaultSize=&quot;22&quot; orientation=&quot;Vertical&quot; count=&quot;1&quot;&gt;&lt;Items&gt;&lt;Item index=&quot;-1&quot;&gt;&lt;Size&gt;22&lt;/Size&gt;&lt;/Item&gt;&lt;/Items&gt;&lt;/ColumnFooterRow&gt;&lt;/AxisModels&gt;&lt;StyleModels&gt;&lt;RowHeader class=&quot;FarPoint.Web.Spread.Model.DefaultSheetStyleModel&quot; Rows=&quot;3&quot; Columns=&quot;1&quot;&gt;&lt;AltRowCount&gt;2&lt;/AltRowCount&gt;&lt;DefaultStyle class=&quot;FarPoint.Web.Spread.NamedStyle&quot; Parent=&quot;RowHeaderDefault&quot; /&gt;&lt;ConditionalFormatCollections /&gt;&lt;/RowHeader&gt;&lt;ColumnHeader class=&quot;FarPoint.Web.Spread.Model.DefaultSheetStyleModel&quot; Rows=&quot;1&quot; Columns=&quot;4&quot;&gt;&lt;AltRowCount&gt;2&lt;/AltRowCount&gt;&lt;DefaultStyle class=&quot;FarPoint.Web.Spread.NamedStyle&quot; Parent=&quot;ColumnHeaderDefault&quot;&gt;&lt;Background class=&quot;FarPoint.Web.Spread.Background&quot;&gt;&lt;BackgroundImageUrl&gt;SPREADCLIENTPATH:/img/chbg.gif&lt;/BackgroundImageUrl&gt;&lt;SelectedBackgroundImageUrl&gt;SPREADCLIENTPATH:/img/chm.png&lt;/SelectedBackgroundImageUrl&gt;&lt;/Background&gt;&lt;/DefaultStyle&gt;&lt;ConditionalFormatCollections /&gt;&lt;/ColumnHeader&gt;&lt;DataArea class=&quot;FarPoint.Web.Spread.Model.DefaultSheetStyleModel&quot; Rows=&quot;3&quot; Columns=&quot;4&quot;&gt;&lt;AltRowCount&gt;2&lt;/AltRowCount&gt;&lt;DefaultStyle class=&quot;FarPoint.Web.Spread.NamedStyle&quot; Parent=&quot;DataAreaDefault&quot; /&gt;&lt;ConditionalFormatCollections /&gt;&lt;/DataArea&gt;&lt;SheetCorner class=&quot;FarPoint.Web.Spread.Model.DefaultSheetStyleModel&quot; Rows=&quot;1&quot; Columns=&quot;1&quot;&gt;&lt;AltRowCount&gt;2&lt;/AltRowCount&gt;&lt;DefaultStyle class=&quot;FarPoint.Web.Spread.NamedStyle&quot; Parent=&quot;CornerDefault&quot;&gt;&lt;Background class=&quot;FarPoint.Web.Spread.Background&quot;&gt;&lt;BackgroundImageUrl&gt;SPREADCLIENTPATH:/img/chbg.gif&lt;/BackgroundImageUrl&gt;&lt;SelectedBackgroundImageUrl&gt;SPREADCLIENTPATH:/img/chm.png&lt;/SelectedBackgroundImageUrl&gt;&lt;/Background&gt;&lt;/DefaultStyle&gt;&lt;ConditionalFormatCollections /&gt;&lt;/SheetCorner&gt;&lt;ColumnFooter class=&quot;FarPoint.Web.Spread.Model.DefaultSheetStyleModel&quot; Rows=&quot;1&quot; Columns=&quot;4&quot;&gt;&lt;AltRowCount&gt;2&lt;/AltRowCount&gt;&lt;DefaultStyle class=&quot;FarPoint.Web.Spread.NamedStyle&quot; Parent=&quot;ColumnFooterDefault&quot; /&gt;&lt;ConditionalFormatCollections /&gt;&lt;/ColumnFooter&gt;&lt;/StyleModels&gt;&lt;MessageRowStyle class=&quot;FarPoint.Web.Spread.Appearance&quot;&gt;&lt;BackColor&gt;LightYellow&lt;/BackColor&gt;&lt;ForeColor&gt;Red&lt;/ForeColor&gt;&lt;/MessageRowStyle&gt;&lt;SheetCornerStyle class=&quot;FarPoint.Web.Spread.NamedStyle&quot; Parent=&quot;CornerDefault&quot;&gt;&lt;Background class=&quot;FarPoint.Web.Spread.Background&quot;&gt;&lt;BackgroundImageUrl&gt;SPREADCLIENTPATH:/img/chbg.gif&lt;/BackgroundImageUrl&gt;&lt;SelectedBackgroundImageUrl&gt;SPREADCLIENTPATH:/img/chm.png&lt;/SelectedBackgroundImageUrl&gt;&lt;/Background&gt;&lt;/SheetCornerStyle&gt;&lt;AllowLoadOnDemand&gt;false&lt;/AllowLoadOnDemand&gt;&lt;LoadRowIncrement &gt;10&lt;/LoadRowIncrement &gt;&lt;LoadInitRowCount &gt;30&lt;/LoadInitRowCount &gt;&lt;AllowVirtualScrollPaging&gt;false&lt;/AllowVirtualScrollPaging&gt;&lt;TopRow&gt;0&lt;/TopRow&gt;&lt;PreviewRowStyle class=&quot;FarPoint.Web.Spread.PreviewRowInfo&quot; /&gt;&lt;/Presentation&gt;&lt;Settings&gt;&lt;Name&gt;Sheet1&lt;/Name&gt;&lt;Categories&gt;&lt;Appearance&gt;&lt;GridLineColor&gt;#d0d7e5&lt;/GridLineColor&gt;&lt;SelectionBackColor&gt;#eaecf5&lt;/SelectionBackColor&gt;&lt;SelectionBorder class=&quot;FarPoint.Web.Spread.Border&quot; /&gt;&lt;/Appearance&gt;&lt;Behavior&gt;&lt;EditTemplateColumnCount&gt;2&lt;/EditTemplateColumnCount&gt;&lt;AutoPostBack&gt;True&lt;/AutoPostBack&gt;&lt;GroupBarText&gt;Drag a column to group by that column.&lt;/GroupBarText&gt;&lt;/Behavior&gt;&lt;Layout&gt;&lt;RowHeaderColumnCount&gt;1&lt;/RowHeaderColumnCount&gt;&lt;ColumnHeaderRowCount&gt;1&lt;/ColumnHeaderRowCount&gt;&lt;/Layout&gt;&lt;/Categories&gt;&lt;ColumnHeaderRowCount&gt;1&lt;/ColumnHeaderRowCount&gt;&lt;ColumnFooterRowCount&gt;1&lt;/ColumnFooterRowCount&gt;&lt;PrintInfo&gt;&lt;Header /&gt;&lt;Footer /&gt;&lt;ZoomFactor&gt;0&lt;/ZoomFactor&gt;&lt;FirstPageNumber&gt;1&lt;/FirstPageNumber&gt;&lt;Orientation&gt;Auto&lt;/Orientation&gt;&lt;PrintType&gt;All&lt;/PrintType&gt;&lt;PageOrder&gt;Auto&lt;/PageOrder&gt;&lt;BestFitCols&gt;False&lt;/BestFitCols&gt;&lt;BestFitRows&gt;False&lt;/BestFitRows&gt;&lt;PageStart&gt;-1&lt;/PageStart&gt;&lt;PageEnd&gt;-1&lt;/PageEnd&gt;&lt;ColStart&gt;-1&lt;/ColStart&gt;&lt;ColEnd&gt;-1&lt;/ColEnd&gt;&lt;RowStart&gt;-1&lt;/RowStart&gt;&lt;RowEnd&gt;-1&lt;/RowEnd&gt;&lt;ShowBorder&gt;True&lt;/ShowBorder&gt;&lt;ShowGrid&gt;True&lt;/ShowGrid&gt;&lt;ShowColor&gt;True&lt;/ShowColor&gt;&lt;ShowColumnHeader&gt;Inherit&lt;/ShowColumnHeader&gt;&lt;ShowRowHeader&gt;Inherit&lt;/ShowRowHeader&gt;&lt;ShowColumnFooter&gt;Inherit&lt;/ShowColumnFooter&gt;&lt;ShowColumnFooterEachPage&gt;True&lt;/ShowColumnFooterEachPage&gt;&lt;ShowTitle&gt;True&lt;/ShowTitle&gt;&lt;ShowSubtitle&gt;True&lt;/ShowSubtitle&gt;&lt;UseMax&gt;True&lt;/UseMax&gt;&lt;UseSmartPrint&gt;False&lt;/UseSmartPrint&gt;&lt;Opacity&gt;255&lt;/Opacity&gt;&lt;PrintNotes&gt;None&lt;/PrintNotes&gt;&lt;Centering&gt;None&lt;/Centering&gt;&lt;RepeatColStart&gt;-1&lt;/RepeatColStart&gt;&lt;RepeatColEnd&gt;-1&lt;/RepeatColEnd&gt;&lt;RepeatRowStart&gt;-1&lt;/RepeatRowStart&gt;&lt;RepeatRowEnd&gt;-1&lt;/RepeatRowEnd&gt;&lt;SmartPrintPagesTall&gt;1&lt;/SmartPrintPagesTall&gt;&lt;SmartPrintPagesWide&gt;1&lt;/SmartPrintPagesWide&gt;&lt;HeaderHeight&gt;-1&lt;/HeaderHeight&gt;&lt;FooterHeight&gt;-1&lt;/FooterHeight&gt;&lt;/PrintInfo&gt;&lt;TitleInfo class=&quot;FarPoint.Web.Spread.TitleInfo&quot;&gt;&lt;Style class=&quot;FarPoint.Web.Spread.StyleInfo&quot;&gt;&lt;BackColor&gt;#e7eff7&lt;/BackColor&gt;&lt;HorizontalAlign&gt;Right&lt;/HorizontalAlign&gt;&lt;/Style&gt;&lt;/TitleInfo&gt;&lt;LayoutTemplate class=&quot;FarPoint.Web.Spread.LayoutTemplate&quot;&gt;&lt;Layout&gt;&lt;ColumnCount&gt;4&lt;/ColumnCount&gt;&lt;RowCount&gt;1&lt;/RowCount&gt;&lt;/Layout&gt;&lt;Data&gt;&lt;LayoutData class=&quot;FarPoint.Web.Spread.Model.DefaultSheetDataModel&quot; rows=&quot;1&quot; columns=&quot;4&quot;&gt;&lt;AutoCalculation&gt;True&lt;/AutoCalculation&gt;&lt;AutoGenerateColumns&gt;True&lt;/AutoGenerateColumns&gt;&lt;ReferenceStyle&gt;A1&lt;/ReferenceStyle&gt;&lt;Iteration&gt;False&lt;/Iteration&gt;&lt;MaximumIterations&gt;1&lt;/MaximumIterations&gt;&lt;MaximumChange&gt;0.001&lt;/MaximumChange&gt;&lt;Cells&gt;&lt;Cell row=&quot;0&quot; column=&quot;0&quot;&gt;&lt;Data type=&quot;System.Int32&quot;&gt;0&lt;/Data&gt;&lt;/Cell&gt;&lt;Cell row=&quot;0&quot; column=&quot;1&quot;&gt;&lt;Data type=&quot;System.Int32&quot;&gt;1&lt;/Data&gt;&lt;/Cell&gt;&lt;Cell row=&quot;0&quot; column=&quot;2&quot;&gt;&lt;Data type=&quot;System.Int32&quot;&gt;2&lt;/Data&gt;&lt;/Cell&gt;&lt;Cell row=&quot;0&quot; column=&quot;3&quot;&gt;&lt;Data type=&quot;System.Int32&quot;&gt;3&lt;/Data&gt;&lt;/Cell&gt;&lt;/Cells&gt;&lt;/LayoutData&gt;&lt;/Data&gt;&lt;AxisModels&gt;&lt;LayoutColumn class=&quot;FarPoint.Web.Spread.Model.DefaultSheetAxisModel&quot; orientation=&quot;Horizontal&quot; count=&quot;4&quot;&gt;&lt;Items&gt;&lt;Item index=&quot;-1&quot;&gt;&lt;SortIndicator&gt;Ascending&lt;/SortIndicator&gt;&lt;/Item&gt;&lt;/Items&gt;&lt;/LayoutColumn&gt;&lt;LayoutRow class=&quot;FarPoint.Web.Spread.Model.DefaultSheetAxisModel&quot; orientation=&quot;Vertical&quot; count=&quot;1&quot;&gt;&lt;Items&gt;&lt;Item index=&quot;-1&quot; /&gt;&lt;/Items&gt;&lt;/LayoutRow&gt;&lt;/AxisModels&gt;&lt;/LayoutTemplate&gt;&lt;LayoutMode&gt;CellLayoutMode&lt;/LayoutMode&gt;&lt;CurrentPageIndex type=&quot;System.Int32&quot;&gt;0&lt;/CurrentPageIndex&gt;&lt;/Settings&gt;&lt;/Sheet&gt;">
                                    </FarPoint:SheetView>
                                </Sheets>
                                <TitleInfo BackColor="#E7EFF7" ForeColor="" HorizontalAlign="Center" VerticalAlign="NotSet"
                                    Font-Size="X-Large">
                                </TitleInfo>
                            </FarPoint:FpSpread>
                        </center>
                    </td>
                </tr>
            </table>
            <table style="margin: 0px; margin-bottom: 10px; margin-top: 10px;">
                <tr>
                    <td class="style2" style="width: 900px;">
                        <center>
                            <div id="rptprint1" runat="server" visible="false">
                                <asp:Label ID="lbl_norec1" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                    Font-Size="Medium" ForeColor="Red" Text="Please Enter Your Report Name" Visible="false"></asp:Label>
                                <asp:Label ID="lblrptname1" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                    Font-Size="Medium" Text="Report Name"></asp:Label>
                                <asp:TextBox ID="txtexcelname1" runat="server" CssClass="textbox textbox1" Height="20px"
                                    Width="180px" Style="font-family: 'Book Antiqua'" Font-Bold="True" Font-Names="Book Antiqua"
                                    onkeypress="display1()" Font-Size="Medium"></asp:TextBox>
                                <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender9" runat="server" TargetControlID="txtexcelname1"
                                    FilterType="LowercaseLetters,UppercaseLetters,Numbers,Custom" ValidChars="(),.[]_"
                                    InvalidChars="/\">
                                </asp:FilteredTextBoxExtender>
                                <asp:Button ID="btnExcel1" runat="server" Font-Bold="true" Font-Names="Book Antiqua"
                                    OnClick="btnExcel1_Click" Font-Size="Medium" Text="Export To Excel" Width="127px"
                                    Height="35px" CssClass="textbox textbox1" />
                                <asp:Button ID="btnprintmaster1" runat="server" Text="Print" OnClick="btnprintmaster1_Click"
                                    Font-Names="Book Antiqua" Font-Size="Medium" Font-Bold="true" Width="60px" Height="35px"
                                    CssClass="textbox textbox1" />
                                <asp:Button ID="btnsave" runat="server" Text="Save" Font-Bold="True" Font-Names="Book Antiqua"
                                    Font-Size="Medium" Width="60px" Height="35px" CssClass="textbox textbox1" OnClick="btnsave_Click" />
                                <Insproplus:printmaster runat="server" ID="Printcontrol1" Visible="false" />
                            </div>
                        </center>
                    </td>
                </tr>
            </table>
        </center>
    </div>
     <center>
        <div id="divPopupAlert" runat="server" visible="false" style="height: 100em; z-index: 1000;
            width: 100%; background-color: rgba(54, 25, 25, .2); position: absolute; top: 0%;
            left: 0%; right: 0%;">
              <asp:ImageButton ID="imagebtnpop1" runat="server" Width="40px" Height="40px" ImageUrl="~/images/close.png"
                        Style="height: 25px; width: 23px; position: absolute; margin-top: 197px; margin-left: 118px;"
                        OnClick="btn_popclose_Click" />
            <center>
                <div id="divAlertContent" runat="server" class="table" style="background-color: White;
                    height: 120px; width: 276px; border: 5px solid #0CA6CA; border-top: 25px solid #0CA6CA;
                    margin-top: 200px; border-radius: 10px;">
                    <center>
                        <table style="height: 100px; width: 100%;">
                            <tr>
                                <td align="center">
                                    <asp:Label ID="lblAlertMsg" runat="server" Text="" Style="color: Red;" Font-Bold="true"
                                        Font-Size="Medium" Font-Names="Book Antiqua"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td align="center">
                                    <center>
                                        <asp:Button ID="btnoldrecord" runat="server" CssClass=" textbox btn2" Width="80px"
                                            OnClick="btnoldrecord_Click" Text="Old Record" />
                                            <asp:Button ID="btnnewrecord" runat="server" CssClass=" textbox btn2" Width="80px"
                                            OnClick="btnnewrecord_Click" Text="New Record" />
                                    </center>
                                </td>
                            </tr>
                        </table>
                    </center>
                </div>
            </center>
        </div>
    </center>
</asp:Content>

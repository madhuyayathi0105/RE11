<%@ Page Title="" Language="C#" MasterPageFile="~/AttendanceMOD/AttendanceSubSiteMaster.master"
    AutoEventWireup="true" CodeFile="newadmin.aspx.cs" Inherits="newadmin" EnableEventValidation="false" %>

<%--MaintainScrollPositionOnPostback="true"--%>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Assembly="FarPoint.Web.Spread" Namespace="FarPoint.Web.Spread" TagPrefix="FarPoint" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <link href="~/Styles/css/Registration.css" rel="stylesheet" type="text/css" />
    <script src="//ajax.googleapis.com/ajax/libs/jquery/1.11.1/jquery.min.js"></script>
    <link href="~/Styles/css/Commoncss.css" rel="Stylesheet" type="text/css" />
    <script type="text/javascript">
        var prm = Sys.WebForms.PageRequestManager.getInstance();
        prm.add_beginRequest(BeginRequestHandler);
        // Raised after an asynchronous postback is finished and control has been returned to the browser.
        prm.add_endRequest(EndRequestHandler);
        function BeginRequestHandler(sender, args) {
            //Shows the modal popup - the update progress
            var popup = $find('<%= modalPopup.ClientID %>');
            if (popup != null) {
                popup.show();
            }
        }
        function EndRequestHandler(sender, args) {
            //Hide the modal popup - the update progress
            var popup = $find('<%= modalPopup.ClientID %>');
            if (popup != null) {
                popup.hide();
            }
        }
    </script>
    <%--<script type="text/javascript">
            var xPos, yPos;
            var prm = Sys.WebForms.PageRequestManager.getInstance();
            prm.add_beginRequest(BeginRequestHandler);
            prm.add_endRequest(EndRequestHandler);
            function BeginRequestHandler(sender, args) {
                xPos = $get('scrollDiv').scrollLeft;
                yPos = $get('scrollDiv').scrollTop;
            }
            function EndRequestHandler(sender, args) {
                $get('scrollDiv').scrollLeft = xPos;
                $get('scrollDiv').scrollTop = yPos;
            }
        </script>--%>
    <style type="text/css" media="screen">
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
            top: 100px;
            left: 150px;
        }
    </style>
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
    <div style="margin: 0px; margin-bottom: 45px; position: relative; height: auto;">
        <center>
            <span class="fontstyleheader" style="color: Green; margin: 0px; margin-bottom: 10px;
                margin-top: 10px; position: relative;">Attendance</span>
        </center>
        <center>
            <asp:Panel ID="Panel1" runat="server">
                <div class="maintablestyle" style="width: 900px; margin: 0px; margin-bottom: 10px;
                    margin-top: 10px; position: relative; text-align: left; padding: 5px; height: auto;">
                    <table style="height: auto; width: auto;">
                        <tr>
                            <td>
                                <asp:Label ID="lblbatch" runat="server" Text="Batch" Style="font-family: 'Baskerville Old Face';
                                    font-weight: 700;" Font-Names="Book Antiqua" Font-Size="Medium" Height="16px"></asp:Label>
                            </td>
                            <td>
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
                    <table style="height: auto; width: auto;">
                        <tr>
                            <td>
                                <asp:Label ID="lblfrom" runat="server" Text="From Date" Style="font-family: 'Baskerville Old Face';
                                    font-weight: 700;" Font-Names="Book Antiqua" Font-Size="Medium"></asp:Label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtFromDate" CssClass="txt" runat="server" Width="90px" Font-Names="Book Antiqua"
                                    Font-Size="Medium" OnTextChanged="txtFromDate_TextChanged" Font-Bold="True" AutoPostBack="True"></asp:TextBox>
                                <asp:CalendarExtender ID="CalendarExtender2" TargetControlID="txtFromDate" runat="server"
                                    Format="d-MM-yyyy">
                                </asp:CalendarExtender>
                            </td>
                            <td>
                                <asp:Label ID="lblTo" runat="server" Text="To Date" Style="font-family: 'Baskerville Old Face';
                                    font-weight: 700;" Font-Names="Book Antiqua" Font-Size="Medium" Height="16px"></asp:Label>
                            </td>
                            <td>
                                <asp:TextBox ID="TxtToDate" runat="server" Width="90px" Font-Names="Book Antiqua"
                                    Font-Size="Medium" OnTextChanged="TxtToDate_TextChanged" Font-Bold="True" AutoPostBack="True"></asp:TextBox>
                                <asp:CalendarExtender ID="CalendarExtender1" TargetControlID="txtToDate" runat="server"
                                    Format="d-MM-yyyy">
                                </asp:CalendarExtender>
                            </td>
                            <td>
                                <asp:Button ID="Btngo" runat="server" OnClick="Btngo_Click" CssClass="cursorptr"
                                    Style="font-weight: 700; width: auto; height: auto;" Text="GO" />
                            </td>
                            <td>
                                <asp:CheckBox ID="cksubjectwise" runat="server" AutoPostBack="True" Enabled="False"
                                    OnCheckedChanged="cksubjectwise_CheckedChanged" Style="font-weight: 700" Text="Subjectwise"
                                    Visible="False" Font-Names="Book Antiqua" Font-Size="Medium" />
                            </td>
                            <td>
                                <asp:DropDownList ID="ddlsubject" runat="server" Visible="False" OnSelectedIndexChanged="ddlsubject_SelectedIndexChanged"
                                    AutoPostBack="True" Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium"
                                    Width="99px">
                                </asp:DropDownList>
                            </td>
                            <td>
                                <asp:CheckBox ID="ckmanual" runat="server" OnCheckedChanged="ckmanual_CheckedChanged"
                                    Style="font-weight: 700; width: auto; height: auto;" Text="Manual Schedule" AutoPostBack="True"
                                    Enabled="False" Visible="False" Font-Names="Book Antiqua" Font-Size="Medium" />
                            </td>
                            <td>
                                <asp:Button ID="btnok" runat="server" Text="OK" Visible="false" OnClick="btnok_Click"
                                    Style="font-weight: 700; width: auto; height: auto;" />
                                <asp:Button ID="btnsliplist" runat="server" Text="Slip List" Font-Names="Book Antiqua"
                                    Font-Size="Medium" Font-Bold="true" OnClick="btnsliplist_Click" Style="font-weight: 700;
                                    width: auto; height: auto;" />
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
                <center>
                    <asp:Panel ID="Panelhour" runat="server" Height="53px" BorderStyle="Solid" BorderWidth="1px"
                        Width="365px" Visible="False">
                        <center style="height: 21px; width: 351px">
                            <asp:Label ID="Labelhr" runat="server" Text="Select the Hour" Font-Bold="True" Font-Names="Book Antiqua"
                                Font-Size="Medium"></asp:Label>
                        </center>
                        <asp:CheckBoxList ID="ckhr2" runat="server" Width="358px" RepeatColumns="8" RepeatDirection="Horizontal"
                            OnSelectedIndexChanged="ckhr2_SelectedIndexChanged" Font-Bold="True" Font-Names="Book Antiqua"
                            Height="16px">
                        </asp:CheckBoxList>
                    </asp:Panel>
                </center>
                <table>
                    <tr>
                        <td>
                        </td>
                    </tr>
                    <tr>
                        <td>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label ID="lblset" runat="server" Visible="False" Style="font-family: 'Baskerville Old Face';
                                font-weight: 700; height: auto; width: auto; margin: 0px; margin-bottom: 10px;
                                margin-top: 10px; position: relative;" Font-Bold="False" Font-Size="Medium" ForeColor="Red"></asp:Label>
                        </td>
                        <td>
                        </td>
                    </tr>
                </table>
            </asp:Panel>
            <asp:Panel ID="Panel2" Visible="false" runat="server" Style="margin: 0px; margin-bottom: 10px;
                margin-top: 10px; position: relative; height: auto;">
                <asp:RadioButtonList ID="RadioButtonList1" runat="server" RepeatDirection="Horizontal"
                    Visible="false" Height="19px" Width="300px" AutoPostBack="True">
                    <asp:ListItem Value="1" Text="Rollno"></asp:ListItem>
                    <asp:ListItem Value="2" Text="Regno"></asp:ListItem>
                    <asp:ListItem Value="3" Text="Admission no"></asp:ListItem>
                </asp:RadioButtonList>
                <asp:RadioButtonList ID="option" RepeatDirection="Horizontal" runat="server" Height="19px"
                    Width="191px" Visible="False">
                    <asp:ListItem Value="1" Text="General"></asp:ListItem>
                    <asp:ListItem Value="2" Text="Individual"></asp:ListItem>
                </asp:RadioButtonList>
            </asp:Panel>
            <asp:Panel ID="Panelind" Visible="false" runat="server" Style="margin: 0px; margin-bottom: 10px;
                margin-top: 10px; position: relative; height: auto;">
                <table>
                    <tr>
                        <td>
                            <asp:Label ID="lblselected" runat="server" Style="font-family: 'Baskerville Old Face'"
                                Text="For the Selected Student" Width="182px" CssClass="style109" Font-Names="Book Antiqua"
                                Font-Size="Medium"></asp:Label>
                        </td>
                        <td class="style113">
                            <asp:DropDownList ID="ddlmark" runat="server" OnSelectedIndexChanged="ddlmark_SelectedIndexChanged"
                                AutoPostBack="true" CssClass="cursorptr" Font-Names="Book Antiqua" Font-Size="Medium"
                                Font-Bold="True" Height="25px">
                            </asp:DropDownList>
                            <asp:Label ID="lblmarkabs" runat="server" Text="Select" Visible="False" ForeColor="Red"
                                Style="font-weight: 400" Font-Names="Book Antiqua" Font-Size="Medium"></asp:Label>
                            <asp:Label ID="Label10" runat="server" Text="Should not be same as Rest of the students"
                                Visible="False" ForeColor="Red" Style="font-weight: 400" Font-Names="Book Antiqua"
                                Font-Size="Medium"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label ID="lblroll" runat="server" Style="font-family: 'Baskerville Old Face'"
                                Text="Enter The " Width="180px" CssClass="style109" Font-Names="Book Antiqua"
                                Font-Size="Medium"></asp:Label>
                        </td>
                        <td class="style113">
                            <br />
                            <br />
                            <br />
                            <asp:TextBox ID="txtregno" runat="server" Height="21px" Width="97px" CssClass="style109"
                                AutoPostBack="True" OnTextChanged="txtregno_TextChanged" Font-Names="Book Antiqua"
                                Font-Size="Medium"></asp:TextBox>
                            <asp:TextBox ID="txtrunning" runat="server" Height="21px" Width="335px" CssClass="style109"
                                AutoPostBack="True" OnTextChanged="txtrunning_TextChanged" Font-Names="Book Antiqua"
                                Font-Size="Medium"></asp:TextBox>
                            &nbsp;<asp:Label ID="lblstate" runat="server" ForeColor="#996633" Style="font-weight: 700"
                                Text="Static Part" Visible="false"></asp:Label>
                            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<asp:Label ID="lblrun" runat="server" ForeColor="#996633"
                                Style="font-weight: 700" Text="Running Part" Visible="false"></asp:Label>
                            &nbsp;&nbsp;&nbsp;
                            <asp:Label ID="lblrunerror" runat="server" ForeColor="Red" Text="Enter Running Part"
                                Visible="False" Font-Names="Book Antiqua" Font-Size="Medium"></asp:Label>
                            <br />
                            <asp:Label ID="lblinvalidreg" runat="server" ForeColor="Red" Visible="False" Font-Names="Book Antiqua"
                                Font-Size="Medium"></asp:Label>
                            <br />
                            &nbsp;<asp:Label ID="lblregno" runat="server" ForeColor="Red" Visible="False" Font-Names="Book Antiqua"
                                Font-Size="Medium"></asp:Label>
                        </td>
                        <td>
                            <asp:Button ID="btngoindividual" runat="server" CssClass="cursorptr" Height="29px"
                                OnClick="btngoindividual_Click" Text="GO" Width="59px" />
                        </td>
                        <td>
                            &nbsp;
                        </td>
                        <td class="style110">
                            <br />
                            <asp:TextBox ID="tbhourind" runat="server" Text="Select the hours" Height="25px"
                                Style="font-family: 'Baskerville Old Face'" Width="130px" Font-Names="Book Antiqua"
                                Font-Size="Medium" Font-Bold="True"></asp:TextBox>
                            <asp:DropDownExtender ID="DropDownExtender1" runat="server" DropDownControlID="Panel4"
                                DynamicServicePath="" Enabled="true" TargetControlID="tbhourind">
                            </asp:DropDownExtender>
                            <asp:Panel ID="Panel4" runat="server" Height="82px" ScrollBars="Auto" BorderStyle="Solid"
                                BorderWidth="1px" Width="129px">
                                <asp:CheckBoxList ID="Ckhour" runat="server" BorderWidth="2px" CssClass="style109"
                                    Width="120px" OnSelectedIndexChanged="Ckhour_SelectedIndexChanged" Font-Names="Book Antiqua"
                                    Font-Size="Medium" Height="56px">
                                </asp:CheckBoxList>
                            </asp:Panel>
                            <asp:Label ID="lblhrselect" runat="server" Text="Select the hour" Visible="False"
                                ForeColor="Red" Style="font-weight: 400" Font-Names="Book Antiqua" Font-Size="Medium"></asp:Label>
                        </td>
                        <td>
                            <asp:Label ID="lblindisave" runat="server" Text="Saved Successfully" Visible="False"
                                ForeColor="Red" Style="font-weight: 400" Font-Names="Book Antiqua" Font-Size="Medium"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label ID="lblrest" runat="server" Style="font-family: 'Baskerville Old Face'"
                                Text="For Rest of the Students" Width="181px" CssClass="style109" Font-Names="Book Antiqua"
                                Font-Size="Medium"></asp:Label>
                        </td>
                        <td class="style113">
                            <asp:DropDownList ID="ddlmarkothers" runat="server" OnSelectedIndexChanged="ddlmarkothers_SelectedIndexChanged"
                                AutoPostBack="true" CssClass="cursorptr" Font-Names="Book Antiqua" Font-Size="Medium">
                            </asp:DropDownList>
                            <asp:Label ID="Label7" runat="server" Text="Select" Visible="False" ForeColor="Red"
                                Style="font-weight: 400" Font-Names="Book Antiqua" Font-Size="Medium"></asp:Label>
                            <asp:Label ID="markdiff" runat="server" Text="Should not be same as Selected students"
                                Visible="False" ForeColor="Red" Style="font-weight: 400" Font-Names="Book Antiqua"
                                Font-Size="Medium"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td>
                        </td>
                    </tr>
                </table>
            </asp:Panel>
            <asp:Panel ID="pHeaderatendence" Visible="false" runat="server" CssClass="cpHeader">
                <asp:Label ID="Labelatend" runat="server" Text="Mark Attendance" BackColor="Transparent"
                    BorderColor="Transparent" BorderWidth="0px" Height="16px" Font-Bold="True" Font-Names="Book Antiqua"
                    Font-Size="Medium" />
                <asp:Image ID="ImageSel" runat="server" CssClass="cpimage" ImageUrl="right.jpeg" />
            </asp:Panel>
            <asp:UpdatePanel ID="UPD5" runat="server">
                <ContentTemplate>
                    <asp:UpdateProgress ID="UpdateProgress1" runat="server" DynamicLayout="true" AssociatedUpdatePanelID="UPD5">
                        <ProgressTemplate>
                            <div class="CenterPB" style="height: 40px; width: 40px;">
                                <img src="../images/progress2.gif" height="180px" width="180px" />
                            </div>
                        </ProgressTemplate>
                    </asp:UpdateProgress>
                    <asp:ModalPopupExtender ID="modalPopup" runat="server" TargetControlID="UpdateProgress1"
                        PopupControlID="UpdateProgress1">
                    </asp:ModalPopupExtender>
                    <asp:Panel ID="pBodyatendence" runat="server" CssClass="cpBody" Style="margin: 0px;
                        margin-bottom: 35px; margin-top: 10px; position: relative; height: auto;">
                        <asp:Panel ID="Panelpage" Visible="false" runat="server" Style="margin: 0px; margin-bottom: 10px;
                            margin-top: 10px; position: relative;">
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
                        <FarPoint:FpSpread ID="FpSpread2" runat="server" BorderWidth="2px" Height="50px"
                            OnUpdateCommand="FpSpread2_UpdateCommand" OnPreRender="FpSpread2_SelectedIndexChanged"
                            ShowHeaderSelection="false" Style="margin: 0px; margin-bottom: 10px; margin-top: 10px;
                            position: relative;">
                            <CommandBar BackColor="Control" ButtonFaceColor="Control" Visible="false" ButtonHighlightColor="ControlLightLight"
                                ButtonShadowColor="ControlDark">
                            </CommandBar>
                            <Sheets>
                                <FarPoint:SheetView SheetName="Sheet1">
                                </FarPoint:SheetView>
                            </Sheets>
                        </FarPoint:FpSpread>
                        <asp:Panel ID="Panel3" runat="server" Style="margin: 0px; margin-bottom: 50px; margin-top: 10px;
                            position: relative;">
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
                            <asp:Button ID="btncopy" CssClass="floats" runat="server" Text="Copy Attendance"
                                OnClick="btncopy_Click" Font-Bold="true" />
                        </asp:Panel>
                        <asp:CollapsiblePanelExtender ID="cpeatend" runat="server" TargetControlID="pBodyatendence"
                            CollapseControlID="pHeaderatendence" ExpandControlID="pHeaderatendence" Collapsed="true"
                            TextLabelID="Labelatend" CollapsedSize="0" ImageControlID="Imagemark" CollapsedImage="right.jpeg"
                            ExpandedImage="down.jpeg">
                        </asp:CollapsiblePanelExtender>
                    </asp:Panel>
                </ContentTemplate>
            </asp:UpdatePanel>
        </center>
    </div>
    <div style="margin: 0px; margin-bottom: 20px; margin-top: 20px; position: relative;">
        <center>
            <asp:Panel ID="pnl_sliplist" runat="server" BackColor="AliceBlue" ScrollBars="Vertical"
                Style="height: 400px; width: 750px; margin: 0px; margin-bottom: 10px; margin-top: 40px;
                position: relative;" BorderColor="Black" BorderStyle="Double">
                <table>
                    <tr>
                        <td colspan="2">
                            <center>
                                <asp:Label ID="headlbl_sl" runat="server" Text="Pending Slip List" Font-Bold="True"
                                    Font-Names="Book Antiqua" Font-Size="Medium"></asp:Label>
                            </center>
                        </td>
                    </tr>
                    <tr>
                        <td>
                        </td>
                    </tr>
                    <tr>
                        <td>
                        </td>
                    </tr>
                </table>
                <table>
                    <tr>
                        <td>
                            <center style="width: 665px">
                                <FarPoint:FpSpread ID="spread_sliplist" runat="server" BorderColor="Black" BorderStyle="Solid"
                                    BorderWidth="1px" Width="700" Style="margin: 0px; margin-bottom: 10px; margin-top: 10px;
                                    position: relative;" CommandBar-Visible="false" ShowHeaderSelection="false">
                                    <CommandBar BackColor="Control" ButtonFaceColor="Control" ButtonHighlightColor="ControlLightLight"
                                        ButtonShadowColor="ControlDark">
                                    </CommandBar>
                                    <Sheets>
                                        <FarPoint:SheetView SheetName="Sheet1" AutoPostBack="true" GridLineColor="Black">
                                        </FarPoint:SheetView>
                                    </Sheets>
                                </FarPoint:FpSpread></center>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label ID="lblnorecpending" Style="margin: 0px; margin-bottom: 10px; margin-top: 10px;
                                position: relative;" runat="server" Font-Bold="true" Font-Names="Book Antiqua"
                                Font-Size="Medium" Text="Attendance Completed" ForeColor="Red"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Button ID="exit_sliplist" runat="server" Text="Exit" Font-Bold="True" Font-Names="Book Antiqua"
                                Font-Size="Medium" OnClick="exit_sliplist_Click" Style="margin: 0px; margin-bottom: 10px;
                                margin-top: 10px; position: relative;" />
                        </td>
                    </tr>
                </table>
            </asp:Panel>
        </center>
    </div>
    <center>
        <asp:HiddenField ID="hf_save" runat="server" />
        <asp:ModalPopupExtender ID="mpemsgboxsave" runat="server" TargetControlID="hf_save"
            PopupControlID="pnlmsgboxsave">
        </asp:ModalPopupExtender>
        <asp:Panel ID="pnlmsgboxsave" runat="server" CssClass="modalPopup" Style="display: none;"
            DefaultButton="btnOk">
            <table width="500">
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
                        <asp:UpdatePanel ID="udp15" runat="server">
                            <ContentTemplate>
                                <asp:Label ID="lblMessage" Text="Do You Want to Save Attendance " runat="server"
                                    Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium"></asp:Label>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </td>
                </tr>
                <tr>
                    <td colspan="2" align="right">
                        <asp:Button ID="btnattOk" runat="server" Text="Yes" OnClick="btnattOk_Click" Font-Bold="True"
                            Font-Names="Book Antiqua" Font-Size="Medium" />
                        <asp:Button ID="btnattCancel" runat="server" Text="No" OnClick="btnattCancel_Click"
                            Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium" />
                    </td>
                </tr>
            </table>
        </asp:Panel>
    </center>
</asp:Content>

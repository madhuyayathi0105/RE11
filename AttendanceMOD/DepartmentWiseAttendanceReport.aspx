<%@ Page Title="" Language="C#" MasterPageFile="~/AttendanceMOD/AttendanceSubSiteMaster.master"
    AutoEventWireup="true" CodeFile="DepartmentWiseAttendanceReport.aspx.cs" Inherits="DepartmentWiseAttendanceReport"
    EnableEventValidation="false" %>

<%@ Register Src="~/Usercontrols/NewPrintMaster.ascx" TagName="NEWPrintMater" TagPrefix="NEW" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Src="~/Usercontrols/PrintMaster.ascx" TagName="printmaster" TagPrefix="Insproplus" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
<script type="text/javascript">
    function printTTOutput() {
        var panel = document.getElementById("<%=printdiv.ClientID %>");
        var printWindow = window.open('', '', 'height=816,width=980');
        printWindow.document.write('<html><head>');
        printWindow.document.write('</head><body >');
        printWindow.document.write(panel.innerHTML);
        printWindow.document.write('</body></html>');
        printWindow.document.close();
        setTimeout(function () {
            printWindow.print();
        }, 500);
        return false;
    }
    </script>
    <style tyle="text/css">
        .printclass
        {
            display: none;
        }
        .marginSet
        {
            margin: 0px;
            padding: 0px;
        }
        .headerDisp
        {
            font-size: 25px;
            font-weight: bold;
        }
        .headerDisp1
        {
            font-family: Book Antiqua;
            font-size: medium;
        }
        @media print
        {
            #printdiv
            {
                display: block;
            }
            .printclass
            {
                display: block;
                font-family: Book Antiqua;
            }
            .noprint
            {
                display: none;
            }
        }
        @media screen,print
        {
        
        }
        @page
        {
            size: A4;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">
    <!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
    <html xmlns="http://www.w3.org/1999/xhtml">
    <title></title>
    <link href="~/Styles/css/Registration.css" rel="stylesheet" type="text/css" />
    <script src="//ajax.googleapis.com/ajax/libs/jquery/1.11.1/jquery.min.js"></script>
    <link href="~/Styles/css/Commoncss.css" rel="Stylesheet" type="text/css" />
    <body>
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
                background-color: #0CA6CA;
                width: 980px;
                position: absolute;
                height: 80px;
                top: 140px;
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
        <center>
            <br />
            <span class="fontstyleheader" id="HeaderSapn" runat="server" style="color: Green;">Department
                & Period Wise Attendance Report</span>
            <br />
        </center>
        <asp:UpdatePanel ID="UpdatePanel2" runat="server">
            <ContentTemplate>
                <div class="mainbatch maintablestyle">
                    <asp:Label ID="Iblbatch" Font-Bold="true" Style="position: absolute; left: 15px;
                        top: 13px; height: 60px;" Font-Size="Medium" ForeColor="Black" Font-Names="Book Antiqua"
                        runat="server" Text="Batch"></asp:Label>
                    <asp:TextBox ID="txt_batch" CssClass="Dropdown_Txt_Box" Font-Names="Book Antiqua"
                        Font-Bold="true" Style="position: absolute; left: 60px; top: 11px; right: 250px;"
                        Width="100px" runat="server" ReadOnly="true">--Select--</asp:TextBox>
                    <asp:Panel ID="pbatch" runat="server" Font-Names="Book Antiqua" Font-Bold="true"
                        CssClass="multxtpanel" Width="114px" Font-Size="Medium">
                        <asp:CheckBox ID="Chk_batch" Font-Bold="true" runat="server" Font-Size="Medium" Text="Select All"
                            AutoPostBack="True" Font-Names="Book Antiqua" OnCheckedChanged="Chlk_batchchanged" />
                        <asp:CheckBoxList ID="Chklst_batch" Font-Bold="true" Font-Size="Medium" runat="server"
                            AutoPostBack="True" Font-Names="Book Antiqua" OnSelectedIndexChanged="Chlk_batchselected">
                        </asp:CheckBoxList>
                    </asp:Panel>
                    <asp:PopupControlExtender ID="popupbatch" runat="server" TargetControlID="txt_batch"
                        PopupControlID="pbatch" Position="Bottom">
                    </asp:PopupControlExtender>
                    <asp:Label ID="Ibldegree" runat="server" Font-Names="Book Antiqua" Font-Bold="true"
                        ForeColor="Black" Font-Size="Medium" Style="position: absolute; left: 185px;
                        top: 13px;"></asp:Label>
                    <asp:TextBox ID="txt_degree" CssClass="Dropdown_Txt_Box" Font-Names="Book Antiqua"
                        Font-Bold="true" Style="left: 266px; position: absolute; top: 11px;" runat="server"
                        ReadOnly="true" Width="100px">--Select--</asp:TextBox>
                    <asp:Panel ID="pdegree" runat="server" CssClass="multxtpanel" Width="128px" Height="200px"
                        Font-Bold="true" Font-Size="Medium">
                        <asp:CheckBox ID="chk_degree" Font-Bold="true" runat="server" Font-Size="Medium"
                            Text="Select All" AutoPostBack="True" Font-Names="Book Antiqua" OnCheckedChanged="checkDegree_CheckedChanged" />
                        <asp:CheckBoxList ID="Chklst_degree" Font-Bold="true" Font-Size="Medium" runat="server"
                            AutoPostBack="True" Font-Names="Book Antiqua" OnSelectedIndexChanged="cheklist_Degree_SelectedIndexChanged">
                        </asp:CheckBoxList>
                    </asp:Panel>
                    <asp:PopupControlExtender ID="popupdegree" runat="server" TargetControlID="txt_degree"
                        PopupControlID="pdegree" Position="Bottom">
                    </asp:PopupControlExtender>
                    <asp:Label ID="Iblbranch" runat="server" Font-Names="Book Antiqua" Font-Bold="true"
                        ForeColor="Black" Font-Size="Medium" Style="position: absolute; left: 387px;
                        top: 13px;"></asp:Label>
                    <asp:TextBox ID="txt_branch" CssClass="Dropdown_Txt_Box" Font-Bold="true" Font-Names="Book Antiqua"
                        Style="position: absolute; left: 469px; top: 11px;" runat="server" ReadOnly="true"
                        Width="100px">--Select--</asp:TextBox>
                    <asp:Panel ID="Panel3" runat="server" CssClass="multxtpanel" Width="400px" Height="250px">
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
                    <asp:Label ID="lblSec" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                        Font-Size="Medium" ForeColor="Black" Text="Sec" Style="width: 90px; left: 580px;
                        position: absolute; top: 13px;">
                    </asp:Label>
                    <asp:TextBox ID="txtsection" runat="server" CssClass="Dropdown_Txt_Box" Font-Bold="true"
                        Font-Names="Book Antiqua" Height="20px" ReadOnly="true" Width="100px" Style="top: 11px;
                        position: absolute; left: 615px;">---Select---</asp:TextBox>
                    <asp:Panel ID="psection" runat="server" Height="200px" CssClass="multxtpanel" Width="120px">
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
                    <asp:Label ID="lblfromdate" runat="server" Font-Names="Book Antiqua" Font-Bold="true"
                        ForeColor="Black" Font-Size="Medium" Style="position: absolute; left: 724px;
                        top: 13px;" Text="From"></asp:Label>
                    <asp:TextBox ID="txtfromdate" runat="server" Font-Names="Book Antiqua" Font-Bold="true"
                        Font-Size="Medium" Width="80px" AutoPostBack="true" OnTextChanged="txtfromdate_TextChanged"
                        Style="position: absolute; left: 773px; top: 11px;" />
                    <asp:CalendarExtender ID="CalendarExtender1" TargetControlID="txtfromdate" runat="server"
                        Format="dd/MM/yyyy">
                    </asp:CalendarExtender>
                    <asp:Label ID="lbltodate" runat="server" Text="To" font-name="Book Antiqua" Font-Size="Medium"
                        Style="position: absolute; left: 864px; top: 13px;" Width="100px" Font-Bold="true"
                        ForeColor="Black"></asp:Label>
                    <asp:TextBox ID="txttodate" runat="server" Font-Names="Book Antiqua" Font-Bold="true"
                        Font-Size="Medium" Width="80px" AutoPostBack="true" OnTextChanged="txttodate_TextChanged"
                        Style="position: absolute; left: 889px; top: 10px;" />
                    <asp:CalendarExtender ID="CalendarExtender2" TargetControlID="txttodate" runat="server"
                        Format="dd/MM/yyyy">
                    </asp:CalendarExtender>
                    <asp:Label ID="lblperiod" runat="server" Font-Names="Book Antiqua" Font-Bold="true"
                        ForeColor="Black" Font-Size="Medium" Style="position: absolute; left: 512px;
                        top: 47px;" Text="Period"></asp:Label>
                    <asp:TextBox ID="txtperiod" CssClass="Dropdown_Txt_Box" Font-Bold="true" Font-Names="Book Antiqua"
                        Style="position: absolute; left: 565px; top: 46px;" runat="server" ReadOnly="true"
                        Width="100px">--Select--</asp:TextBox>
                    <asp:Panel ID="Pperiod" runat="server" CssClass="multxtpanel" Width="120px">
                        <asp:CheckBox ID="chkperiod" runat="server" Font-Bold="true" Font-Size="Medium" Font-Names="Book Antiqua"
                            Text="Select All" OnCheckedChanged="chkperiod_CheckedChanged" AutoPostBack="True" />
                        <asp:CheckBoxList ID="chklsperiod" Font-Names="Book Antiqua" Font-Bold="true" Font-Size="Medium"
                            runat="server" OnSelectedIndexChanged="chklsperiod_SelectedIndexChanged" AutoPostBack="True">
                        </asp:CheckBoxList>
                    </asp:Panel>
                    <asp:PopupControlExtender ID="PopupControlExtender1" runat="server" TargetControlID="txtperiod"
                        PopupControlID="Pperiod" Position="Bottom">
                    </asp:PopupControlExtender>
                    <asp:RadioButton ID="rbdepartment" runat="server" Font-Names="Book Antiqua" Font-Bold="true"
                        ForeColor="Black" Font-Size="Medium" GroupName="Report" AutoPostBack="true" OnCheckedChanged="rbreport_CheckedChanged"
                        Style="top: 50px; left: 10px; position: absolute;" />
                    <asp:RadioButton ID="rbdate" runat="server" Text="Date Wise" Font-Names="Book Antiqua"
                        Font-Bold="true" ForeColor="Black" GroupName="Report" AutoPostBack="true" OnCheckedChanged="rbreport_CheckedChanged"
                        Font-Size="Medium" Style="top: 50px; left: 175px; position: absolute;" />
                    <asp:Label ID="lblattendance" runat="server" Font-Names="Book Antiqua" Font-Bold="true"
                        ForeColor="Black" Font-Size="Medium" Style="position: absolute; left: 312px;
                        top: 47px;" Text="Attendance"></asp:Label>
                    <asp:DropDownList ID="ddlattendance" runat="server" Font-Names="Book Antiqua" Font-Bold="true"
                        Font-Size="Medium" AutoPostBack="true" OnSelectedIndexChanged="chklscolumn_SelectedIndexChanged"
                        Style="position: absolute; left: 420px; top: 46px;">
                        <asp:ListItem Text="All"></asp:ListItem>
                        <asp:ListItem Text="Present"></asp:ListItem>
                        <asp:ListItem Text="Absent"></asp:ListItem>
                    </asp:DropDownList>
                    <asp:Button ID="btngo" runat="server" Font-Names="Book Antiqua" Text="Go" OnClick="btngo_Click"
                        Font-Size="Medium" Font-Bold="true" Style="top: 46px; left: 683px; position: absolute;" />
                </div>
                <br />
                <br />
                <br />
                <br />
                <br />
                <br />
                <center>
                    <asp:Label ID="lbl_err" runat="server" Text="" ForeColor="Red" Font-Bold="true" Font-Size="Medium"
                        Font-Names="Book Antiqua"></asp:Label>
                    <br />
                     <div id="printdiv" runat="server">
            <table class="printclass" style="width: 98%; height: auto; margin: 0px; padding: 0px;">
                <tr>
                    <td rowspan="5" style="width: 100px; margin: 0px; border: 0px;">
                        <asp:Image ID="imgLeftLogo2" runat="server" AlternateText="" ImageUrl="~/college/Left_Logo.jpeg"
                            Width="100px" Height="100px" />
                    </td>
                    <th class="marginSet" align="center" colspan="6">
                        <span id="spCollegeName" class="headerDisp" runat="server"></span>
                    </th>
                </tr>
                <tr>
                    <th class="marginSet" align="center" colspan="6">
                        <span id="spAddr" class="headerDisp1" runat="server"></span>
                    </th>
                </tr>
                <tr>
                    <th class="marginSet" align="center" colspan="6">
                        <span id="spReportName" class="headerDisp1" runat="server"></span>
                    </th>
                </tr>
                <tr>
                    <td class="marginSet" colspan="3" align="center">
                        <span id="spDegreeName" class="headerDisp1" runat="server"></span>
                    </td>
                    <td class="marginSet" colspan="3" align="right">
                        <span id="spSem" class="headerDisp1" runat="server"></span>
                    </td>
                </tr>
                <tr>
                    <td class="marginSet" colspan="3" align="left">
                        <span id="spProgremme" class="headerDisp1" runat="server"></span>
                    </td>
                    <td class="marginSet" colspan="3" align="right">
                        <span id="spSection" class="headerDisp1" runat="server"></span>
                    </td>
                </tr>
            </table>
     <center>
                                    <asp:GridView ID="Showgrid" runat="server" Visible="false" HeaderStyle-ForeColor="Black"
                    HeaderStyle-BackColor="#0CA6CA" >
                </asp:GridView>
                        </center>
            <table class="printclass" style="width: 98%; height: auto; margin-top: 100px; padding: 0px;">
                <tr>
                    <td>
                        
                    </td>
                    <td style="text-align: right">
                        
                    </td>
                </tr>
            </table>
        </div>
            
                    <br />
                    <asp:Chart ID="attedancechart" runat="server" Width="800px" Visible="true">
                        <Series>
                            <asp:Series Name="Series1" IsValueShownAsLabel="true" ChartArea="ChartArea1" ChartType="Column">
                            </asp:Series>
                        </Series>
                        <ChartAreas>
                            <asp:ChartArea Name="ChartArea1" BorderWidth="0">
                                <AxisY LineColor="White">
                                    <LabelStyle Font="Trebuchet MS, 8.25pt" />
                                    <MajorGrid LineColor="#e6e6e6" />
                                    <MinorGrid Enabled="false" LineColor="#e6e6e6" />
                                </AxisY>
                                <AxisX LineColor="White">
                                    <LabelStyle Font="Trebuchet MS, 8.25pt" />
                                    <MajorGrid LineColor="#e6e6e6" />
                                    <MinorGrid Enabled="false" LineColor="#e6e6e6" />
                                </AxisX>
                            </asp:ChartArea>
                        </ChartAreas>
                    </asp:Chart>
                    <br />
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
                     <NEW:NEWPrintMater runat="server" ID="Printcontrol" Visible="false" />

                    <button id="btnPrint" runat="server"  height="29px" width="62px" onclick="return printTTOutput();"
            style=" font-weight: bold; font-size: medium; font-family: Book Antiqua;">
            Direct Print
        </button>
                </center>
            </ContentTemplate>
            <Triggers>
                <asp:PostBackTrigger ControlID="btnmasterprint" />
                <asp:PostBackTrigger ControlID="btnxl" />
                <asp:PostBackTrigger ControlID="btngo" />
            </Triggers>
        </asp:UpdatePanel>

    </body>
    </html>
</asp:Content>

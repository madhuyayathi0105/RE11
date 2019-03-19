<%@ Page Title="" Language="C#" MasterPageFile="~/AttendanceMOD/AttendanceSubSiteMaster.master"
    AutoEventWireup="true" CodeFile="CondonationReport.aspx.cs" Inherits="AttendanceMOD_CondonationReport" %>

<%@ Register Src="~/Usercontrols/NewPrintMaster.ascx" TagName="NEWPrintMater" TagPrefix="NEW" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">

    <script type="text/javascript">
        function display() {
            document.getElementById('<%=lbl_norec.ClientID %>').innerHTML = "";
        }
    </script>
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
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <center>
        <span class="fontstyleheader" style="color: Green; margin: 0px; margin-bottom: 10px;
            margin-top: 10px;">Condonation Report</span>
    </center>
    <center>
        <table style="position: relative; margin: 0px; margin-bottom: 10px; margin-top: 10px;
            background-color: #0CA6CA; width: auto; border-radius: 10px;">
            <tr>
                <td>
                    Stream
                </td>
                <td>
                    <asp:DropDownList ID="ddlstream" runat="server" OnSelectedIndexChanged="ddlstream_SelectedIndexChanged"
                        AutoPostBack="True" Height="25px" Width="85px">
                    </asp:DropDownList>
                </td>
                <td>
                    Batch
                </td>
                <td>
                    <%--<asp:DropDownList ID="ddlbatch" runat="server" OnSelectedIndexChanged="ddlbatch_SelectedIndexChanged"
                        AutoPostBack="True" Height="25px" Width="69px">
                    </asp:DropDownList>--%>
                    <asp:UpdatePanel ID="UpdatePanel3" runat="server">
                        <ContentTemplate>
                            <asp:TextBox ID="txt_batchyear" Width=" 100px" runat="server" Font-Names="Book Antiqua"
                                CssClass="textbox  txtheight2" ReadOnly="true">-- Select --</asp:TextBox>
                            <asp:Panel ID="Panel3" runat="server" CssClass="multxtpanel" Height="200px" Width="150px">
                                <asp:CheckBox ID="cb_batchyear" runat="server" Text="Select All" AutoPostBack="True"
                                    OnCheckedChanged="cb_batchyear_CheckedChanged" />
                                <asp:CheckBoxList ID="cbl_batchyear" Font-Names="Book Antiqua" runat="server" AutoPostBack="True"
                                    OnSelectedIndexChanged="cbl_batchyear_SelectedIndexChanged">
                                </asp:CheckBoxList>
                            </asp:Panel>
                            <asp:PopupControlExtender ID="PopupControlExtender3" runat="server" TargetControlID="txt_batchyear"
                                PopupControlID="Panel3" Position="Bottom">
                            </asp:PopupControlExtender>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </td>
                <td>
                    Degree
                </td>
                <td>
                    <asp:UpdatePanel ID="UpnlDegree" runat="server">
                        <ContentTemplate>
                            <asp:TextBox ID="txtDegree" Width=" 100px" runat="server" Font-Names="Book Antiqua"
                                CssClass="textbox  txtheight2" ReadOnly="true">-- Select --</asp:TextBox>
                            <asp:Panel ID="pnlDegree" runat="server" CssClass="multxtpanel" Height="200px" Width="150px">
                                <asp:CheckBox ID="chkDegree" runat="server" Text="Select All" AutoPostBack="True"
                                    OnCheckedChanged="chkDegree_CheckedChanged" />
                                <asp:CheckBoxList ID="cblDegree" Font-Names="Book Antiqua" runat="server" AutoPostBack="True"
                                    OnSelectedIndexChanged="cblDegree_SelectedIndexChanged">
                                </asp:CheckBoxList>
                            </asp:Panel>
                            <asp:PopupControlExtender ID="popupExtDegree" runat="server" TargetControlID="txtDegree"
                                PopupControlID="pnlDegree" Position="Bottom">
                            </asp:PopupControlExtender>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </td>
                <td>
                    Branch
                </td>
                <td>
                    <asp:DropDownList ID="ddlbranch" runat="server" OnSelectedIndexChanged="ddlbranch_SelectedIndexChanged"
                        AutoPostBack="True" Height="25px" Width="271px" Visible="false">
                    </asp:DropDownList>
                    <asp:UpdatePanel ID="upnlBranch" runat="server">
                        <ContentTemplate>
                            <asp:TextBox ID="txtBranch" Width=" 100px" runat="server" Font-Names="Book Antiqua"
                                CssClass="textbox  txtheight2" ReadOnly="true">-- Select --</asp:TextBox>
                            <asp:Panel ID="pnlBranch" runat="server" CssClass="multxtpanel" Height="250px" Width="200px">
                                <asp:CheckBox ID="chkBranch" runat="server" Text="Select All" AutoPostBack="True"
                                    OnCheckedChanged="chkBranch_CheckedChanged" />
                                <asp:CheckBoxList ID="cblBranch" Font-Names="Book Antiqua" runat="server" AutoPostBack="True"
                                    OnSelectedIndexChanged="cblBranch_SelectedIndexChanged">
                                </asp:CheckBoxList>
                            </asp:Panel>
                            <asp:PopupControlExtender ID="popupExtBranch" runat="server" TargetControlID="txtBranch"
                                PopupControlID="pnlBranch" Position="Bottom">
                            </asp:PopupControlExtender>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </td>
                <td>
                    Semester
                </td>
                <td>
                    <%-- <asp:DropDownList ID="ddlsemester" runat="server" OnSelectedIndexChanged="ddlsemester_SelectedIndexChanged"
                        AutoPostBack="True" Height="25px" Width="41px">
                    </asp:DropDownList>--%>
                    <%--  --%>
                    <div>
                        <asp:UpdatePanel ID="upnlSem" runat="server">
                            <ContentTemplate>
                                <asp:TextBox ID="txtSem" Visible="true" Width="85px" runat="server" Font-Names="Book Antiqua"
                                    CssClass="textbox  txtheight2" ReadOnly="true">--Select--</asp:TextBox>
                                <asp:Panel ID="pnlSem" Visible="true" runat="server" CssClass="multxtpanel" Style="width: 260px;
                                    height: 230px; overflow: auto; margin: 0px; padding: 0px;">
                                    <asp:CheckBox ID="chkSem" Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium"
                                        runat="server" Text="Select All" AutoPostBack="True" Style="width: 100%; height: auto;
                                        margin: 0px; padding: 0px; border: 0px;" OnCheckedChanged="chkSem_CheckedChanged" />
                                    <asp:CheckBoxList ID="cblSem" Font-Bold="True" Font-Size="Medium" Font-Names="Book Antiqua"
                                        runat="server" AutoPostBack="True" Style="width: 100%; height: auto; margin: 0px;
                                        padding: 0px; border: 0px;" OnSelectedIndexChanged="cblSem_SelectedIndexChanged">
                                    </asp:CheckBoxList>
                                </asp:Panel>
                                <asp:PopupControlExtender ID="popExtSem" runat="server" TargetControlID="txtSem"
                                    PopupControlID="pnlSem" Position="Bottom">
                                </asp:PopupControlExtender>
                                <asp:DropDownList ID="ddlsemester" Visible="false" runat="server" CssClass="commonHeaderFont"
                                    OnSelectedIndexChanged="ddlsemester_SelectedIndexChanged" AutoPostBack="True"
                                    Width="40px">
                                </asp:DropDownList>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </div>
                </td>
                <td>
                    Section
                </td>
                <td>
                    <asp:UpdatePanel ID="upnlSec" runat="server">
                        <ContentTemplate>
                            <asp:TextBox ID="txtSec" Width="80px" runat="server" Font-Names="Book Antiqua" CssClass="textbox  txtheight2"
                                ReadOnly="true">-- Select --</asp:TextBox>
                            <asp:Panel ID="pnlSec" runat="server" CssClass="multxtpanel">
                                <asp:CheckBox ID="chkSec" runat="server" Text="Select All" AutoPostBack="True" OnCheckedChanged="chkSec_CheckedChanged" />
                                <asp:CheckBoxList ID="cblSec" Font-Names="Book Antiqua" runat="server" AutoPostBack="True"
                                    OnSelectedIndexChanged="cblSec_SelectedIndexChanged">
                                </asp:CheckBoxList>
                            </asp:Panel>
                            <asp:PopupControlExtender ID="popupExtSec" runat="server" TargetControlID="txtSec"
                                PopupControlID="pnlSec" Position="Bottom">
                            </asp:PopupControlExtender>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </td>
            </tr>
            <tr>
                <td>
                    Report Type
                </td>
                <td>
                    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                        <ContentTemplate>
                            <asp:TextBox ID="txt_reporttype" Width="80px" runat="server" Font-Names="Book Antiqua"
                                CssClass="textbox  txtheight2" ReadOnly="true">--Select--</asp:TextBox>
                            <asp:Panel ID="Panel1" runat="server" CssClass="multxtpanel">
                                <asp:CheckBox ID="cb_reporttype" runat="server" Text="Select All" AutoPostBack="True"
                                    OnCheckedChanged="cb_reporttype_CheckedChanged" />
                                <asp:CheckBoxList ID="cbl_reporttype" runat="server" AutoPostBack="True" OnSelectedIndexChanged="cbl_reporttype_SelectedIndexChanged">
                                    <asp:ListItem Value="1">Eligible</asp:ListItem>
                                    <asp:ListItem Value="2">Condonation</asp:ListItem>
                                    <asp:ListItem Value="3">Not Eligible</asp:ListItem>
                                </asp:CheckBoxList>
                            </asp:Panel>
                            <asp:PopupControlExtender ID="PopupControlExtender1" runat="server" TargetControlID="txt_reporttype"
                                PopupControlID="Panel1" Position="Bottom">
                            </asp:PopupControlExtender>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </td>
                <td>
                    Hostel Name
                </td>
                <td colspan="2">
                    <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                        <ContentTemplate>
                            <asp:TextBox ID="txt_hostelname" runat="server" Font-Names="Book Antiqua" CssClass="textbox  txtheight2"
                                ReadOnly="true">-- Select --</asp:TextBox>
                            <asp:Panel ID="Panel2" runat="server" CssClass="multxtpanel">
                                <asp:CheckBox ID="cb_hostelname" runat="server" Text="Select All" AutoPostBack="True"
                                    OnCheckedChanged="cb_hostelname_CheckedChanged" />
                                <asp:CheckBoxList ID="cbl_hostelname" Font-Names="Book Antiqua" runat="server" AutoPostBack="True"
                                    OnSelectedIndexChanged="cbl_hostelname_SelectedIndexChanged">
                                </asp:CheckBoxList>
                            </asp:Panel>
                            <asp:PopupControlExtender ID="PopupControlExtender2" runat="server" TargetControlID="txt_hostelname"
                                PopupControlID="Panel2" Position="Bottom">
                            </asp:PopupControlExtender>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </td>
                <td>
                    <asp:RadioButtonList ID="rblHrDaywise" runat="server" RepeatDirection="Horizontal"
                        Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium">
                        <asp:ListItem Selected="True">Day</asp:ListItem>
                        <asp:ListItem>Hour</asp:ListItem>
                    </asp:RadioButtonList>
                </td>
                <td>
                    <asp:Button ID="btngo" runat="server" Text="Go" OnClick="btngo_Click" CssClass="textbox1 btn1" />
                </td>
                <td>
                    <%--<asp:Label ID="lblfdate" runat="server" Text="From Date" Font-Bold="True" Font-Names="Book Antiqua"
                        Font-Size="Medium"></asp:Label>--%>
                    From Date
                    <asp:TextBox ID="txtfdate" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                        Font-Size="Medium" Width="80px" AutoPostBack="true" OnTextChanged="txtfdate_TextChanged"></asp:TextBox>
                    <asp:CalendarExtender ID="CalendarExtender1" Format="d/MM/yyyy" TargetControlID="txtfdate"
                        runat="server">
                    </asp:CalendarExtender>
                </td>
                <td>
                    <%-- <asp:Label ID="lbltodate" runat="server" Text="To Date" Font-Bold="True" Font-Names="Book Antiqua"
                        Font-Size="Medium"></asp:Label>--%>
                    To Date
                    <asp:TextBox ID="txttodate" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                        Font-Size="Medium" Width="80px" AutoPostBack="true" OnTextChanged="txttodate_TextChanged"></asp:TextBox>
                    <asp:CalendarExtender ID="CalendarExtender2" Format="d/MM/yyyy" TargetControlID="txttodate"
                        runat="server">
                    </asp:CalendarExtender>
                </td>
                <td>
                    Range From
                    <asp:TextBox runat="server" ID="txtfrom_range" Font-Names="Book Antiqua" Font-Size="Medium"
                        Width="20px" OnTextChanged="txtfrom_range_OnTextChanged" MaxLength="3"></asp:TextBox>
                </td>
                <td>
                    To
                    <asp:TextBox runat="server" ID="txtto_range" OnTextChanged="txttorange_OnTextChanged"
                        Font-Names="Book Antiqua" Font-Size="Medium" Width="25px" MaxLength="3"></asp:TextBox>
                </td>
            </tr>
        </table>
        <center>
            <asp:Label ID="errmsg" runat="server" ForeColor="Red" Visible="False"></asp:Label>
        </center>
        <br />


        <div id="divSpread" runat="server" visible="false">
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
                
                
                 <div>
                <asp:GridView ID="Showgrid" runat="server" Visible="false" HeaderStyle-ForeColor="Black"
                    HeaderStyle-BackColor="#0CA6CA" OnRowDataBound="Showgrid_OnRowDataBound">
                </asp:GridView>
                  </div>

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
           
            <center>
                <center>
                    <asp:Label ID="lbl_norec" Visible="False" runat="server" ForeColor="#FF3300" Text=""></asp:Label>
                </center>
                <asp:Label ID="lbl_reportname" runat="server" Text="Report Name"></asp:Label>
                <asp:TextBox ID="txt_excelname" runat="server" AutoPostBack="true" CssClass="textbox textbox1 txtheight5"
                    onkeypress="display()"></asp:TextBox>
                <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender16" runat="server" TargetControlID="txt_excelname"
                    FilterType="LowercaseLetters,UppercaseLetters,Numbers,Custom" ValidChars="!@$%^&()_+}{][';,.">
                </asp:FilteredTextBoxExtender>
                <asp:Button ID="btn_Excel" runat="server" Text="Export To Excel" Width="150px" CssClass="textbox btn2"
                    AutoPostBack="true" OnClick="btnExcel_Click" />
                <asp:Button ID="btn_printmaster" runat="server" Text="Print" CssClass="textbox btn2"
                    AutoPostBack="true" OnClick="btn_printmaster_Click" />
                <asp:Button ID="btn_CondonationEligibleSave" runat="server" Text="Save" CssClass="textbox btn2"
                    AutoPostBack="true" OnClick="btn_CondonationEligibleSave_Click" />
                <NEW:NEWPrintMater runat="server" ID="Printcontrol" Visible="false" />

                <button id="btnPrint" runat="server"  height="29px" width="62px" onclick="return printTTOutput();"
            style=" font-weight: bold; font-size: medium; font-family: Book Antiqua;">
            Direct Print
        </button>
            </center>
        </div>
    </center>
</asp:Content>

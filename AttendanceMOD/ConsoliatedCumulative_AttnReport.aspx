<%@ Page Title="" Language="C#" MasterPageFile="~/AttendanceMOD/AttendanceSubSiteMaster.master"
    AutoEventWireup="true" CodeFile="ConsoliatedCumulative_AttnReport.aspx.cs" Inherits="ConsoliatedCumulative_AttnReport" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Src="~/Usercontrols/NewPrintMaster.ascx" TagName="NEWPrintMater" TagPrefix="NEW" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">
    <html xmlns="http://www.w3.org/1999/xhtml">
    <head id="Head1">
        <title></title>
        <link href="~/Styles/css/Commoncss.css" rel="stylesheet" type="text/css" />
        <style type="text/css">
            .maindivstylesize
            {
                height: 620px;
                width: 1000px;
            }
            
            .style1
            {
                width: 122px;
            }
            
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
                #divMainContents
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
    </head>
    <body>
        <script type="text/javascript">
            function display() {

                document.getElementById('MainContent_errlbl').innerHTML = "";
                document.getElementById('MainContent_lbl_norec').innerHTML = "";
            }
            function PrintPanel() {

                var panel = document.getElementById("<%=divMainContents.ClientID %>");
                var printWindow = window.open('', '', 'height=842,width=1191');
                printWindow.document.write('<html');
                printWindow.document.write('<head>');
                printWindow.document.write('</head><body >');
                printWindow.document.write('<form>');
                printWindow.document.write(panel.innerHTML);
                printWindow.document.write(' </form>');
                printWindow.document.write('</body></html>');
                printWindow.document.close();
                setTimeout(function () {
                    printWindow.print();
                }, 500);
                return false;
            }
        </script>
        <form id="form1">
        <div>
            <asp:ScriptManager ID="ScriptManager1" runat="server">
            </asp:ScriptManager>
            <br />
            <center>
                <div>
                    <center>
                        <asp:Label ID="lblmenucost" runat="server" Style="font-size: large; color: #008000;"
                            Text="Consolidated Cumulative Attendance Report"></asp:Label>
                    </center>
                </div>
                <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                    <ContentTemplate>
                        <div class="maindivstyle maindivstylesize">
                            <br />
                            <center>
                                <table class="maintablestyle">
                                    <tr>
                                        <td>
                                            <asp:Label ID="lbl_collegename" Text="College" runat="server" CssClass="txtheight"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:DropDownList ID="ddl_college" runat="server" CssClass="textbox textbox1 ddlheight5"
                                                OnSelectedIndexChanged="ddlcollege_SelectedIndexChanged" AutoPostBack="true">
                                            </asp:DropDownList>
                                        </td>
                                        <td>
                                            <asp:Label ID="lbl_date" Text="Date" runat="server"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txt_date" runat="server" Width="80px" CssClass="textbox txtheight3"></asp:TextBox>
                                            <asp:CalendarExtender ID="caladmin" TargetControlID="txt_date" runat="server" CssClass="cal_Theme1 ajax__calendar_active"
                                                Format="d/MM/yyyy">
                                                <%--Format="dd/MM/yyyy" modified by Deepali on 9.4.18--%>
                                            </asp:CalendarExtender>
                                        </td>
                                        <td>
                                            <asp:CheckBox ID="chk_hour" runat="server" AutoPostBack="true" OnCheckedChanged="chk_hour_CheckedChanged" />
                                        </td>
                                        <td>
                                            <asp:Label ID="lbl_hour" Text="Hour" runat="server"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:UpdatePanel ID="Upp1" runat="server">
                                                <ContentTemplate>
                                                    <asp:TextBox ID="txt_hour" runat="server" CssClass="textbox txtheight1 textbox1"
                                                        ReadOnly="true" onfocus="return myFunction1(this)">--Select--</asp:TextBox>
                                                    <asp:Panel ID="p1" runat="server" BackColor="White" BorderColor="Black" BorderStyle="Solid"
                                                        BorderWidth="2px" CssClass="multxtpanel" Style="position: absolute;">
                                                        <asp:CheckBox ID="cb_hour" runat="server" Text="Select All" AutoPostBack="True" OnCheckedChanged="cb_hour_checkedchange" />
                                                        <asp:CheckBoxList ID="cbl_hour" runat="server" AutoPostBack="true" OnSelectedIndexChanged="cbl_hour_SelectedIndexChanged">
                                                        </asp:CheckBoxList>
                                                    </asp:Panel>
                                                    <asp:PopupControlExtender ID="PopupControlExtender1" runat="server" TargetControlID="txt_hour"
                                                        PopupControlID="p1" Position="Bottom">
                                                    </asp:PopupControlExtender>
                                                </ContentTemplate>
                                            </asp:UpdatePanel>
                                        </td>
                                        <td>
                                            <asp:UpdatePanel ID="UpdatePanel_go" runat="server">
                                                <ContentTemplate>
                                                    <asp:Button ID="btn_Go" Text="Go" runat="server" CssClass="textbox btn1" OnClick="btn_go_Click" />
                                                </ContentTemplate>
                                            </asp:UpdatePanel>
                                        </td>
                                    </tr>
                                </table>
                            </center>
                            <center>
                                <asp:Label ID="lblerr" Text="Please select the Hour" Style="color: Red;" runat="server"></asp:Label></center>
                            <br />
                            <center>
                                <div id="divMainContents" runat="server" visible="false" style="width: 850px; height: 400px;
                                    overflow: auto; border: 1px solid Gray; background-color: White;">
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
                                    <asp:GridView ID="Showgrid" runat="server" Visible="false" HeaderStyle-ForeColor="Black"
                                        HeaderStyle-BackColor="#0CA6CA" AutoGenerateColumns="true" ShowHeaderWhenEmpty="true"
                                        OnRowDataBound="Showgrid_OnRowDataBound">
                                    </asp:GridView>
                                </div>
                                <br />
                            </center>
                            <center>
                                <asp:Label ID="lbl_norec" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                    Font-Size="Medium" ForeColor="#FF3300" Text="" Visible="False">
                                </asp:Label></center>
                            <div id="div_report" runat="server" visible="false">
                                <center>
                                    <asp:Label ID="lbl_reportname" runat="server" Text="Report Name" Font-Bold="True"
                                        Font-Names="Book Antiqua" Font-Size="Medium"></asp:Label>
                                    <asp:TextBox ID="txt_excelname" runat="server" AutoPostBack="true" OnTextChanged="txtexcelname_TextChanged"
                                        CssClass="textbox textbox1 txtheight5" onkeypress="display()"></asp:TextBox>
                                    <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender56" runat="server" TargetControlID="txt_excelname"
                                        FilterType="LowercaseLetters,UppercaseLetters,Numbers,Custom" ValidChars="!@$%^&()_+}{][';,.">
                                    </asp:FilteredTextBoxExtender>
                                    <asp:Button ID="btn_Excel" runat="server" Text="Export To Excel" Width="150px" CssClass="textbox btn2"
                                        AutoPostBack="true" OnClick="btnExcel_Click" />
                                    <asp:Button ID="btn_printmaster" runat="server" Text="Print" CssClass="textbox btn2"
                                        AutoPostBack="true" OnClick="btn_printmaster_Click" />
                                    <NEW:NEWPrintMater runat="server" ID="Printcontrol" Visible="false" />
                                    <asp:Button ID="btnPrint" runat="server" Text="Direct Print" OnClientClick="return PrintPanel();"
                                        Font-Names="Book Antiqua" Font-Size="Medium" Font-Bold="true" Height="35px" CssClass="textbox textbox1" />
                                </center>
                            </div>
                        </div>
                    </ContentTemplate>
                    <Triggers>
                        <asp:PostBackTrigger ControlID="btn_Excel" />
                        <asp:PostBackTrigger ControlID="btn_printmaster" />
                        <asp:PostBackTrigger ControlID="btnPrint" />
                    </Triggers>
                </asp:UpdatePanel>
            </center>
        </div>
        <%--progressBar for go--%>
        <center>
            <asp:UpdateProgress ID="UpdateProgress1" runat="server" AssociatedUpdatePanelID="UpdatePanel_go">
                <ProgressTemplate>
                    <center>
                        <div style="height: 40px; width: 150px;">
                            <img src="../gv images/cloud_loading_256.gif" style="height: 150px;" />
                            <br />
                            <span style="font-family: Book Antiqua; font-size: medium; font-weight: bold; color: Black;">
                                Processing Please Wait...</span>
                        </div>
                    </center>
                </ProgressTemplate>
            </asp:UpdateProgress>
            <asp:ModalPopupExtender ID="ModalPopupExtender1" runat="server" TargetControlID="UpdateProgress1"
                PopupControlID="UpdateProgress1">
            </asp:ModalPopupExtender>
        </center>
        </form>
    </body>
    </html>
</asp:Content>

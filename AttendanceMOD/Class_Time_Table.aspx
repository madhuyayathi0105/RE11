<%@ Page Title="" Language="C#" MasterPageFile="~/AttendanceMOD/AttendanceSubSiteMaster.master"
    AutoEventWireup="true" CodeFile="Class_Time_Table.aspx.cs" Inherits="Class_Time_Table"
    EnableEventValidation="false" %>

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
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">
    <!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
    <html xmlns="http://www.w3.org/1999/xhtml">
    <title></title>
    <link href="~/Styles/css/Commoncss.css" rel="Stylesheet" type="text/css" />
    <script src="//ajax.googleapis.com/ajax/libs/jquery/1.11.1/jquery.min.js"></script>
    <body>
        <asp:ScriptManager ID="ScriptManager1" runat="server">
        </asp:ScriptManager>
        <center>
            <span class="fontstyleheader" style="color: Green">Class Time Table </span>
            <br />
            <br />
            <div class="maindivstyle" style="width: auto;">
                <br />
                <table class="maintablestyle">
                    <tr>
                        <td>
                            <asp:Label ID="lblColl" runat="server" Text="College"></asp:Label>
                        </td>
                        <td>
                            <asp:DropDownList ID="ddlcollege" runat="server" Font-Bold="true" Font-Names="Book Antiqua"
                                Font-Size="Medium" CssClass="textbox1 ddlheight5" Width="250px" OnSelectedIndexChanged="ddlcollege_change"
                                AutoPostBack="true">
                            </asp:DropDownList>
                        </td>
                        <td>
                            Batch Year
                        </td>
                        <td>
                            <asp:DropDownList ID="ddlBatch" runat="server" Font-Bold="true" Font-Names="Book Antiqua"
                                Font-Size="Medium" CssClass="textbox1 ddlheight1" OnSelectedIndexChanged="ddlBatch_Change"
                                AutoPostBack="true" Width="100px">
                            </asp:DropDownList>
                        </td>
                        <td>
                            <asp:Label ID="lblDeg" runat="server" Text="Degree"></asp:Label>
                        </td>
                        <td>
                            <asp:DropDownList ID="ddlDegree" runat="server" Font-Bold="true" Font-Names="Book Antiqua"
                                Font-Size="Medium" CssClass="textbox1 ddlheight3" OnSelectedIndexChanged="ddlDegree_Change"
                                AutoPostBack="true" Width="150px">
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label ID="lblBranch" runat="server" Text="Branch"></asp:Label>
                        </td>
                        <td>
                            <asp:DropDownList ID="ddlBranch" runat="server" Font-Bold="true" Font-Names="Book Antiqua"
                                Font-Size="Medium" CssClass="textbox1 ddlheight3" OnSelectedIndexChanged="ddlBranch_Change"
                                AutoPostBack="true" Width="250px">
                            </asp:DropDownList>
                        </td>
                        <td>
                            <asp:Label ID="lblSem" runat="server" Text="Semester"></asp:Label>
                        </td>
                        <td>
                            <asp:DropDownList ID="ddlSem" runat="server" Font-Bold="true" Font-Names="Book Antiqua"
                                Font-Size="Medium" CssClass="textbox1 ddlheight3" OnSelectedIndexChanged="ddlSem_Change"
                                AutoPostBack="true" Width="100px">
                            </asp:DropDownList>
                        </td>
                        <td>
                            Section
                        </td>
                        <td>
                            <asp:DropDownList ID="ddlSec" runat="server" Font-Bold="true" Font-Names="Book Antiqua"
                                Font-Size="Medium" CssClass="textbox1 ddlheight3" Width="100px">
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2">
                            <fieldset id="fldDates" runat="server" style="border-color: Black; border-radius: 5px;
                                width: auto;">
                                <asp:RadioButton ID="radSemWise" runat="server" Text="Semester Wise" Checked="true"
                                    GroupName="SemDay" OnCheckedChanged="radSemWise_Change" AutoPostBack="true" />
                                &nbsp;&nbsp;
                                <asp:RadioButton ID="radDayWise" runat="server" Text="Day Wise" GroupName="SemDay"
                                    OnCheckedChanged="radDayWise_Change" AutoPostBack="true" />
                            </fieldset>
                        </td>
                        <td id="tdlbFrm" runat="server" visible="false">
                            From Date
                        </td>
                        <td colspan="4">
                            <asp:TextBox ID="txtFrmDt" runat="server" Visible="false" CssClass="textbox txtheight2"
                                Style="width: 90px; height: 25px; font-family: book antiqua; font-weight: bold;
                                font-size: medium;"></asp:TextBox>
                            <asp:CalendarExtender ID="calFrmDt" runat="server" CssClass="cal_Theme1 ajax__calendar_active"
                                TargetControlID="txtFrmDt" Format="dd/MM/yyyy">
                            </asp:CalendarExtender>
                            <asp:Label ID="lblToDt" runat="server" Text="To Date" Visible="false"></asp:Label>
                            <asp:TextBox ID="txtToDt" runat="server" Visible="false" CssClass="textbox txtheight2"
                                Style="width: 90px; height: 25px; font-family: book antiqua; font-weight: bold;
                                font-size: medium;"></asp:TextBox>
                            <asp:CalendarExtender ID="calToDt" runat="server" CssClass="cal_Theme1 ajax__calendar_active"
                                TargetControlID="txtToDt" Format="dd/MM/yyyy">
                            </asp:CalendarExtender>
                            <asp:Button ID="btnGo" runat="server" Text="Go" Height="35px" Width="50px" OnClick="btnGo_Click"
                                Font-Bold="true" Font-Names="Book Antiqua" Font-Size="Medium" CssClass="textbox textbox1 btn2" />
                        </td>
                    </tr>
                </table>
                <asp:UpdatePanel ID="updcolumn" runat="server">
                    <ContentTemplate>
                        <div>
                            <br />
                            <center>
                                <asp:Panel ID="pheaderfilter" runat="server" CssClass="maintablestyle" Height="22px"
                                    Width="800px" Style="margin-top: -0.1%; cursor: pointer;">
                                    <asp:Label ID="Labelfilter" Text="Display Options" runat="server" Font-Size="Medium"
                                        Font-Bold="True" Font-Names="Book Antiqua" Style="margin-left: 0%;" />
                                    <asp:Image ID="Imagefilter" runat="server" CssClass="cpimage" ImageAlign="Right" />
                                </asp:Panel>
                            </center>
                            <br />
                        </div>
                        <center>
                            <asp:Panel ID="pcolumnorder" runat="server" CssClass="maintablestyle" Width="800px">
                                <table>
                                    <tr>
                                        <td>
                                            <asp:CheckBox ID="CheckBox_column" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                                Font-Size="Medium" Text="Select All" AutoPostBack="true" OnCheckedChanged="CheckBox_column_CheckedChanged" />
                                        </td>
                                        <td>
                                            <asp:LinkButton ID="lnk_columnorder" runat="server" Font-Size="X-Small" Height="16px"
                                                Style="font-family: 'Book Antiqua'; font-weight: 700; font-size: small; margin-left: -599px;"
                                                Width="111px" OnClick="LinkButtonsremove_Click">Remove  All</asp:LinkButton>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            &nbsp;
                                            <asp:TextBox ID="tborder" Visible="true" Text="SUBJECT CODE(1),STAFF CODE(2),ROOM NAME(3)"
                                                ReadOnly="true" Width="771px" TextMode="MultiLine" CssClass="style1" AutoPostBack="true"
                                                runat="server" Enabled="true">
                                            </asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:CheckBoxList ID="cblcolumnorder" runat="server" Height="43px" AutoPostBack="true"
                                                Width="800px" Style="font-family: 'Book Antiqua'; font-weight: 700; font-size: medium;"
                                                RepeatColumns="5" RepeatDirection="Horizontal" OnSelectedIndexChanged="cblcolumnorder_SelectedIndexChanged">
                                                <asp:ListItem Selected="True" Value="0">SUBJECT CODE</asp:ListItem>
                                                <asp:ListItem Value="1">SUBJECT NAME</asp:ListItem>
                                                <asp:ListItem Selected="True" Value="2">STAFF CODE</asp:ListItem>
                                                <asp:ListItem Value="3">STAFF NAME</asp:ListItem>
                                                <asp:ListItem Selected="True" Value="4">ROOM NAME</asp:ListItem>
                                            </asp:CheckBoxList>
                                            <asp:ListBox ID="lstcolorder" runat="server" Visible="false"></asp:ListBox>
                                        </td>
                                    </tr>
                                </table>
                            </asp:Panel>
                        </center>
                        <asp:CollapsiblePanelExtender ID="cpecolumnorder" runat="server" TargetControlID="pcolumnorder"
                            CollapseControlID="pheaderfilter" ExpandControlID="pheaderfilter" Collapsed="true"
                            TextLabelID="Labelfilter" CollapsedSize="0" ImageControlID="Imagefilter" CollapsedImage="../images/right.jpeg"
                            ExpandedImage="../images/down.jpeg">
                        </asp:CollapsiblePanelExtender>
                    </ContentTemplate>
                </asp:UpdatePanel>
                <br />
                <asp:Label ID="lblMainErr" runat="server" Visible="false" Text="" Font-Bold="true"
                    Font-Size="Medium" ForeColor="Red" Font-Names="Book Antiqua"></asp:Label>
                <br />
                <div id="printdiv" runat="server">
                    <asp:GridView ID="grdClass_TT" runat="server" AutoGenerateColumns="True" Visible="false"
                        GridLines="Both" HeaderStyle-BackColor="#0CA6CA" HeaderStyle-Font-Bold="true"
                        HeaderStyle-HorizontalAlign="Center" HeaderStyle-Font-Names="Book Antiqua" HeaderStyle-Font-Size="Medium"
                        OnRowDataBound="grdClass_TT_OnRowDataBound">
                    </asp:GridView>
                    <br />
                    <asp:GridView ID="grdClassDet_TT" runat="server" AutoGenerateColumns="True" Visible="false"
                        GridLines="Both" HeaderStyle-BackColor="#0CA6CA" HeaderStyle-Font-Bold="true"
                        HeaderStyle-HorizontalAlign="Left" HeaderStyle-Font-Names="Book Antiqua" HeaderStyle-Font-Size="Medium" OnRowDataBound="grdClassDet_TT_OnRowDataBound">
                    </asp:GridView>
                    <br />
                </div>
                <br />
                <button id="btnComPrint" runat="server" visible="false" onclick="return printTTOutput();" style="background-color: LightGreen;
                    font-weight: bold; font-size: medium; font-family: Book Antiqua;">
                    Print
                </button>
                <br />
                <div id="alertpopwindow" runat="server" visible="false" style="height: 100%; z-index: 1000;
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
                                            <asp:Label ID="lblalerterr" runat="server" Text="" Style="color: Red;" Font-Bold="true"
                                                Font-Size="Medium"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <center>
                                                <asp:Button ID="btn_errorclose" CssClass="textbox textbox1 btn1" OnClick="btn_errorclose_Click"
                                                    Text="OK" runat="server" />
                                            </center>
                                        </td>
                                    </tr>
                                </table>
                            </center>
                        </div>
                    </center>
                </div>
            </div>
        </center>
    </body>
    </html>
</asp:Content>

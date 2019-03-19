<%@ Page Title="Black Box 3 (Posted Hour Report)" Language="C#" AutoEventWireup="true"
    CodeFile="PostedHourReport.aspx.cs" MasterPageFile="~/BlackBoxMod/BlackBoxSubSiteMaster.master"
    Inherits="PostedHourReport" EnableEventValidation="false" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Assembly="FarPoint.Web.Spread" Namespace="FarPoint.Web.Spread" TagPrefix="FarPoint" %>
<%@ Register Src="~/Usercontrols/PrintMaster.ascx" TagName="printmaster" TagPrefix="Insproplus" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <title>Black Box 3 (Posted Hour Report)</title>
    <link href="~/Styles/css/Commoncss.css" rel="Stylesheet" type="text/css" />
    <script src="//ajax.googleapis.com/ajax/libs/jquery/1.11.1/jquery.min.js"></script>
    <script type="text/javascript">
        function display1() {
            document.getElementById('<%#lbl_norec1.ClientID %>').innerHTML = "";
        }
        function PrintPanel() {
            var panel = document.getElementById("<%#divPostHrSpread.ClientID %>");
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
    <style tyle="text/css">
        @media print
        {
            #divPostHrSpread
            {
                display: block;
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
        .gvHeader th
        {
            padding: 3px;
            background-color: #DDEECC;
            color: maroon;
            border: 1px solid #bbb;
        }
        .gvRow td
        {
            padding: 3px;
            background-color: #ffffff;
            border: 1px solid #bbb;
        }
        .gvAltRow td
        {
            padding: 3px;
            background-color: #f1f1f1;
            border: 1px solid #bbb;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <div id="divHead" class="noprint" runat="server">
        <center>
            <div>
                <span class="fontstyleheader" style="color: Green; margin: 0px; margin-bottom: 10px;
                    margin-top: 10px; position: relative;">Black Box 3 (Posted Hour Report)</span>
            </div>
        </center>
        <center>
            <div id="divSearch" runat="server" visible="true" style="width: 100%; height: auto;
                margin: 0px; margin-bottom: 10px; margin-top: 10px; position: relative;">
                <table class="maintablestyle" id="tblsearch" runat="server">
                    <tr>
                        <td>
                            <asp:Label ID="lblCollege" runat="server" Text="College" Font-Bold="True" Style="font-family: 'Book Antiqua';"
                                ForeColor="Black" Font-Size="Medium" Font-Names="Book Antiqua"></asp:Label>
                        </td>
                        <td>
                            <asp:DropDownList ID="ddlCollege" runat="server" CssClass="dropdown" Style="font-family: 'Book Antiqua';"
                                Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium" Width="150px" AutoPostBack="True"
                                OnSelectedIndexChanged="ddlCollege_SelectedIndexChanged">
                            </asp:DropDownList>
                        </td>
                        <td>
                            <asp:Label ID="lblBatch" runat="server" Text="Batch" Style="font-family: 'Book Antiqua';"
                                Font-Bold="True" ForeColor="Black" Font-Size="Medium" Font-Names="Book Antiqua"></asp:Label>
                        </td>
                        <td>
                            <div style="position: relative;">
                                <asp:UpdatePanel ID="upnlBatch" runat="server">
                                    <ContentTemplate>
                                        <asp:TextBox ID="txtBatch" runat="server" Height="20px" CssClass="dropdown" ReadOnly="true"
                                            Width="90px" Style="font-family: 'Book Antiqua'" Font-Bold="True" Font-Names="Book Antiqua"
                                            Font-Size="Medium">---Select---</asp:TextBox>
                                        <asp:Panel ID="pnlBatch" runat="server" BackColor="White" BorderColor="Black" BorderStyle="Solid"
                                            BorderWidth="2px" Style="font-family: 'Book Antiqua'; height: 250px; overflow: auto;">
                                            <asp:CheckBox ID="cbBatch" runat="server" Width="100px" Font-Bold="True" OnCheckedChanged="cbBatch_ChekedChange"
                                                Font-Names="Book Antiqua" Font-Size="Medium" Text="Select All" AutoPostBack="True" />
                                            <asp:CheckBoxList ID="cblBatch" runat="server" Font-Size="Medium" AutoPostBack="True"
                                                Width="100px" Height="58px" Font-Bold="True" Font-Names="Book Antiqua" OnSelectedIndexChanged="cblBatch_SelectedIndexChanged">
                                            </asp:CheckBoxList>
                                        </asp:Panel>
                                        <asp:PopupControlExtender ID="popupExtBatch" runat="server" TargetControlID="txtBatch"
                                            PopupControlID="pnlBatch" Position="Bottom">
                                        </asp:PopupControlExtender>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </div>
                        </td>
                        <td>
                            <asp:Label ID="lblDegree" runat="server" Text="Degree" Style="font-family: 'Book Antiqua';"
                                Font-Bold="True" ForeColor="Black" Font-Names="Book Antiqua" Font-Size="Medium"></asp:Label>
                        </td>
                        <td>
                            <div style="position: relative;">
                                <asp:UpdatePanel ID="upnlDegree" runat="server">
                                    <ContentTemplate>
                                        <asp:TextBox ID="txtDegree" runat="server" Height="20px" ReadOnly="true" CssClass="dropdown"
                                            Width="95px" Style="font-family: 'Book Antiqua';" Font-Bold="True" Font-Names="Book Antiqua"
                                            Font-Size="Medium">---Select---</asp:TextBox>
                                        <asp:Panel ID="pnlDegree" runat="server" BackColor="White" BorderColor="Black" BorderStyle="Solid"
                                            BorderWidth="2px" Style="font-family: 'Book Antiqua'; height: 250px; overflow: auto;">
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
                        <td>
                            <asp:Label ID="lblBranch" runat="server" Style="font-family: 'Book Antiqua';" Text="Branch"
                                Font-Bold="True" ForeColor="Black" Font-Names="Book Antiqua" Font-Size="Medium"></asp:Label>
                        </td>
                        <td>
                            <div style="position: relative;">
                                <asp:UpdatePanel ID="upnlBranch" runat="server">
                                    <ContentTemplate>
                                        <asp:TextBox ID="txtBranch" runat="server" Height="20px" CssClass="dropdown" ReadOnly="true"
                                            Width="120px" Style="font-family: 'Book Antiqua';" Font-Bold="True" Font-Names="Book Antiqua"
                                            Font-Size="Medium">---Select---</asp:TextBox>
                                        <asp:Panel ID="pnlBranch" runat="server" BackColor="White" BorderColor="Black" BorderStyle="Solid"
                                            BorderWidth="2px" Style="font-family: 'Book Antiqua'; height: 250px; overflow: auto;">
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
                        <td colspan="8">
                            <table>
                                <tr>
                                    <td id="tdLFDate" runat="server">
                                        <asp:Label ID="lblFromDate" runat="server" Style="font-family: 'Book Antiqua';" Text="From Date"
                                            Font-Bold="true" Font-Names="Book Antiqua" Font-Size="Medium"></asp:Label>
                                    </td>
                                    <td id="tdTFDate" runat="server">
                                        <asp:TextBox ID="txtFromDate" CssClass="dropdown" Style="font-family: 'Book Antiqua';"
                                            runat="server" Font-Bold="true" Width="100px" Font-Names="Book Antiqua" Font-Size="Medium"
                                            AutoPostBack="true" OnTextChanged="txtFromDate_TextChanged"></asp:TextBox>
                                        <asp:CalendarExtender ID="calExtFDate" TargetControlID="txtFromDate" runat="server"
                                            Format="dd/MM/yyyy">
                                        </asp:CalendarExtender>
                                    </td>
                                    <td id="tdLTDate" runat="server">
                                        <asp:Label ID="lblToDate" runat="server" Style="font-family: 'Book Antiqua';" Text="To Date"
                                            Font-Bold="true" Font-Names="Book Antiqua" Font-Size="Medium"></asp:Label>
                                    </td>
                                    <td id="tdTTDate" runat="server">
                                        <asp:TextBox ID="txtToDate" CssClass="dropdown" Style="font-family: 'Book Antiqua';"
                                            runat="server" Font-Bold="true" Width="100px" Font-Names="Book Antiqua" Font-Size="Medium"
                                            AutoPostBack="true" OnTextChanged="txtToDate_TextChanged"></asp:TextBox>
                                        <asp:CalendarExtender ID="calExtTDate" TargetControlID="txtToDate" runat="server"
                                            Format="dd/MM/yyyy">
                                        </asp:CalendarExtender>
                                    </td>
                                    <td>
                                        <asp:Label ID="lblPeriod" runat="server" Text="Period" Style="font-family: 'Book Antiqua';"
                                            Font-Bold="True" ForeColor="Black" Font-Names="Book Antiqua" Font-Size="Medium"></asp:Label>
                                    </td>
                                    <td>
                                        <div style="position: relative;">
                                            <asp:UpdatePanel ID="upnlPeriod" runat="server">
                                                <ContentTemplate>
                                                    <asp:TextBox ID="txtPeriod" runat="server" Height="20px" CssClass="dropdown" ReadOnly="true"
                                                        Width="120px" Style="font-family: 'Book Antiqua';" Font-Bold="True" Font-Names="Book Antiqua"
                                                        Font-Size="Medium">---Select---</asp:TextBox>
                                                    <asp:Panel ID="pnlPeriod" runat="server" BackColor="White" BorderColor="Black" BorderStyle="Solid"
                                                        BorderWidth="2px" Style="font-family: 'Book Antiqua';">
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
                                        </div>
                                    </td>
                                    <td>
                                        <asp:CheckBox ID="chkuser" runat="server" Text="Attendance Entry Rights User's" Font-Bold="True"
                                            Font-Size="Medium" Font-Names="Book Antiqua" />
                                    </td>
                                    <td>
                                        <asp:CheckBox ID="cb_notposted" CssClass="noprint" Text="Not Posted" runat="server"
                                            Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium" AutoPostBack="True"
                                            OnCheckedChanged="cb_notposted_CheckedChanged" />
                                    </td>
                                    <td>
                                        <asp:Button ID="btnGo" runat="server" Font-Bold="True" Font-Size="Medium" Font-Names="Book Antiqua"
                                            Style="width: 59px; height: auto;" CssClass="textbox" Text="Go" OnClick="btnGo_Click" />
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                </table>
            </div>
        </center>
        <asp:Label ID="lblErrSearch" runat="server" Text="" ForeColor="Red" Visible="False"
            Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium" Style="margin: 0px;
            margin-bottom: 10px; margin-top: 10px; position: relative;"></asp:Label>
    </div>
    <center>
        <div id="divPostHrSpread" runat="server" visible="false" style="margin: 0px; margin-bottom: 10px;
            margin-top: 10px;">
            <center>
                <div style="margin: 0px; margin-bottom: 10px; margin-top: 10px;">
                    <span style="color: White; padding: 5px; background-color: Green; font-family: Book Antiqua;
                        font-size: medium; font-weight: bold;">Posted</span> <span style="color: White; padding: 5px;
                            background-color: Red; font-family: Book Antiqua; font-size: medium; font-weight: bold;">
                            Not Posted</span> <span style="color: White; padding: 5px; background-color: Blue;
                                font-family: Book Antiqua; font-size: medium; font-weight: bold;">Free Hour</span>
                    <span style="color: White; padding: 5px; background-color: #FF1493; font-family: Book Antiqua;
                        font-size: medium; font-weight: bold;">Nil Absent</span> <span style="color: White;
                            padding: 5px; background-color: #800000; font-family: Book Antiqua; font-size: medium;
                            font-weight: bold;">Special Day</span> <span style="color: White; padding: 5px; background-color: Gray;
                                font-family: Book Antiqua; font-size: medium; font-weight: bold;">Holiday</span>
                    <span style="color: White; padding: 5px; background-color: #9370DB; font-family: Book Antiqua;
                        font-size: medium; font-weight: bold;">Not Applicable Hour</span>
                    <%-- <asp:CheckBox ID="cb_notposted" CssClass="noprint" Text="Not Posted" runat="server"
                        Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium" AutoPostBack="True"
                        OnCheckedChanged="cb_notposted_CheckedChanged" />--%><%--barath 27.02.17 changes--%>
                </div>
                <FarPoint:FpSpread ID="FpPostedHr" AutoPostBack="false" Width="900px" runat="server"
                    Visible="true" BorderStyle="Solid" BorderWidth="0px" CssClass="spreadborder"
                    ShowHeaderSelection="false" Style="width: 100%; height: auto; margin: 0px; margin-bottom: 10px;
                    margin-top: 10px;" OnPreRender="FpPostedHr_OnPreRender" OnCellClick="Cell1_Click">
                    <Sheets>
                        <FarPoint:SheetView SheetName="Sheet1">
                        </FarPoint:SheetView>
                    </Sheets>
                </FarPoint:FpSpread>
            </center>
        </div>
    </center>
    <center>
        <div style="margin: 0px; margin-bottom: 10px; margin-top: 10px;">
            <%--<asp:GridView ID="importgrid" runat="server" Visible="false" Width="99%" GridLines="None"
                HeaderStyle-CssClass="gvHeader" CssClass="gvRow" AlternatingRowStyle-CssClass="gvAltRow"
                AutoGenerateColumns="false">
                <Columns>
                    <asp:TemplateField>
                        <HeaderTemplate>
                            <tr class="gvHeader">
                                <th style="width: 0px">
                                </th>
                                <th colspan="1">
                                </th>
                                <th colspan="3">
                                    Presented Students
                                </th>
                                 <th colspan="1">
                                </th>
                                <th colspan="3">
                                    Absented Students
                                </th>
                            </tr>
                            <tr class="gvHeader">
                                <th>
                                </th>
                                <th>
                                    S.No
                                </th>
                                <th>
                                    Roll No
                                </th>
                                <th>
                                    Reg No
                                </th>
                                <th>
                                    Student Name
                                </th>
                                 <th>
                                    S.No
                                </th>
                                <th>
                                    Roll No
                                </th>
                                <th>
                                    Reg No
                                </th>
                                <th>
                                    Student Name
                                </th>
                            </tr>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <td style="width: 40px" align="center">
                                <%# Eval("SNO") %>
                            </td>
                            <td>
                                <%# Eval("RollNo")%>
                            </td>
                            <td>
                                <%# Eval("RegNo")%>
                            </td>
                            <td>
                                <%# Eval("StudentName")%>
                            </td>
                            <td style="width: 40px" align="center">
                                <%# Eval("S_NO") %>
                            </td>
                            <td>
                                <%# Eval("Roll_No")%>
                            </td>
                            <td>
                                <%# Eval("Reg_No")%>
                            </td>
                            <td>
                                <%# Eval("Student_Name")%>
                            </td>
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>--%>
            <FarPoint:FpSpread ID="FpSpread1" AutoPostBack="false" Visible="false" Width="950px"
                runat="server" BorderStyle="Solid" BorderWidth="0px" CssClass="spreadborder"
                ShowHeaderSelection="false" Style="width: 100%; height: auto;">
                <Sheets>
                    <FarPoint:SheetView SheetName="Sheet1">
                    </FarPoint:SheetView>
                </Sheets>
            </FarPoint:FpSpread>
        </div>
    </center>
    <center>
        <div id="rptprint1" class="noprint" runat="server" visible="false" style="margin: 0px;
            margin-bottom: 10px; margin-top: 10px;">
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
            <Insproplus:printmaster runat="server" ID="Printcontrol1" Visible="false" />
            <asp:Button ID="btnPrint" runat="server" Text="Direct Print" OnClientClick="return PrintPanel();"
                Font-Names="Book Antiqua" Font-Size="Medium" Font-Bold="true" Height="35px" CssClass="textbox textbox1" />
        </div>
    </center>
    <center>
        <div id="popupdiv" runat="server" visible="false" style="height: 150em; z-index: 1000;
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
</asp:Content>

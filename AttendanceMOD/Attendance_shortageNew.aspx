<%@ Page Title="" Language="C#" MasterPageFile="~/AttendanceMOD/AttendanceSubSiteMaster.master"
    AutoEventWireup="true" CodeFile="Attendance_shortageNew.aspx.cs" Inherits="Attendance_shortageNew" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Src="~/Usercontrols/NewPrintMaster.ascx" TagName="NEWPrintMater" TagPrefix="NEW" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <title>AT15 - Attendance Shortage Details - Regulation Report</title>
    <link href="~/Styles/css/Commoncss.css" rel="Stylesheet" type="text/css" />
    <script src="//ajax.googleapis.com/ajax/libs/jquery/1.11.1/jquery.min.js"></script>
    <style type="text/css">
        body
        {
            font-family: Book Antiqua;
            height: auto;
            background-color: #ffffff;
            color: Black;
        }
        .Chartdiv
        {
            background-color: #ffffff;
            margin: 0px;
            color: #000000;
            position: relative;
            font-family: Book Antiqua;
            height: auto;
            width: 100%;
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
    <script type="text/javascript">
        function display1() {
            document.getElementById('<%#lbl_norec1.ClientID %>').innerHTML = "";
        }
        function PrintPanel() {

            var panel = document.getElementById("<%#divMainContents.ClientID %>");
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
    <script src="Scripts/jquery-1.4.1.min.js" type="text/javascript"></script>
    <script type="text/javascript">
        $(document).ready(function () {
            $('#btnGo').click(function () {
                var mlconsider = $.trim($('#txtMinPresentML').val());
                $('#txtMinPresentML').val(mlconsider);
                if (mlconsider == "") {
                    alert('Please Enter Minimum Percentage For ML Consideration');
                }
                if ($.isNumeric(mlconsider)) {
                    alert('Minimum Percentage For ML Consideration Must Be Numeric');
                }
            });
        });
    </script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <center>
        <div>
            <span class="fontstyleheader" style="color: Green; margin: 0px; margin-bottom: 10px;
                margin-top: 10px; position: relative;">AT15 - Attendance Shortage Details - Regulation
                Report</span>
        </div>
    </center>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <center>
                <div id="divSearch" runat="server" visible="true" class="noprint" style="width: 100%;
                    height: auto; margin: 0px; margin-bottom: 20px; margin-top: 10px; position: relative;">
                    <table style="background-color: #0ca6ca; border: 1px solid #ccc; border-radius: 10px;
                        box-shadow: 0 0 8px #999999; height: auto; margin-left: 0px; margin-top: 8px;
                        padding: 1em; margin-left: 0px; width: 930px;">
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
                                <asp:Label ID="lbl_Batchyear" Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium"
                                    runat="server" Text="Batch"></asp:Label>
                            </td>
                            <td>
                                <asp:DropDownList ID="ddlbatch" runat="server" Font-Bold="True" Font-Size="Medium"
                                    Font-Names="Book Antiqua" OnSelectedIndexChanged="ddlbatch_SelectedIndexChanged"
                                    AutoPostBack="true" Width="100px">
                                </asp:DropDownList>
                            </td>
                            <td>
                                <asp:Label ID="lbldegree" runat="server" Text="Degree" Font-Bold="True" ForeColor="Black"
                                    Font-Size="Medium" Font-Names="Book Antiqua"></asp:Label>
                            </td>
                            <td>
                                <asp:DropDownList ID="ddldegree" runat="server" Font-Bold="True" Font-Size="Medium"
                                    Font-Names="Book Antiqua" OnSelectedIndexChanged="ddldegree_SelectedIndexChanged"
                                    AutoPostBack="true" Width="100px">
                                </asp:DropDownList>
                            </td>
                            <td>
                                <asp:Label ID="lblbranch" runat="server" Text="Branch" Font-Bold="True" ForeColor="Black"
                                    Font-Size="Medium" Font-Names="Book Antiqua" AutoPostBack="true"></asp:Label>
                            </td>
                            <td>
                                <asp:DropDownList ID="ddlbranch" runat="server" Font-Bold="True" Font-Size="Medium"
                                    Font-Names="Book Antiqua" OnSelectedIndexChanged="ddlbranch_SelectedIndexChanged"
                                    AutoPostBack="true" Width="150px">
                                </asp:DropDownList>
                            </td>
                            <td>
                                <asp:Label ID="lblsem" runat="server" Text="Sem" Font-Bold="True" ForeColor="Black"
                                    Font-Size="Medium" Font-Names="Book Antiqua"></asp:Label>
                            </td>
                            <td>
                                <asp:UpdatePanel ID="UpdatePanel_sem" runat="server">
                                    <ContentTemplate>
                                        <asp:DropDownList ID="ddlsem" runat="server" Font-Bold="True" Font-Size="Medium"
                                            Font-Names="Book Antiqua" OnSelectedIndexChanged="ddlsem_SelectedIndexChanged"
                                            AutoPostBack="true" Width="50px">
                                        </asp:DropDownList>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>
                            <td>
                                <asp:Label ID="lblsec" runat="server" Text="Sec" Font-Bold="True" ForeColor="Black"
                                    Font-Size="Medium" Font-Names="Book Antiqua"></asp:Label>
                            </td>
                            <td>
                                <asp:DropDownList ID="ddlsec" runat="server" Font-Bold="True" Font-Size="Medium"
                                    Font-Names="Book Antiqua" OnSelectedIndexChanged="ddlsec_SelectedIndexChanged"
                                    AutoPostBack="true" Width="50px">
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="12">
                                <table>
                                    <tr>
                                        <td>
                                            <asp:Label ID="lblFromDate" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                                Font-Size="Medium" Text="From Date"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtFromDate" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                                Font-Size="Medium" Width="75px" AutoPostBack="True" OnTextChanged="txtFromDate_TextChanged"></asp:TextBox>
                                            <asp:CalendarExtender ID="CalendarExtender1" TargetControlID="txtFromDate" Format="dd/MM/yyyy"
                                                runat="server">
                                            </asp:CalendarExtender>
                                        </td>
                                        <td>
                                            <asp:Label ID="lblToDate" runat="server" Text="To Date" Font-Bold="True" Font-Size="Medium"
                                                Font-Names="Book Antiqua"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtToDate" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                                Font-Size="Medium" Width="80px" AutoPostBack="True" OnTextChanged="txtToDate_TextChanged"></asp:TextBox>
                                            <asp:CalendarExtender ID="CalendarExtender2" TargetControlID="txtToDate" Format="dd/MM/yyyy"
                                                runat="server">
                                            </asp:CalendarExtender>
                                        </td>
                                        <td>
                                            <asp:Label ID="lblFromPer" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                                Font-Size="Medium" Text="%From"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtPerFrom" runat="server" AutoPostBack="true" Font-Bold="True"
                                                Font-Names="Book Antiqua" Font-Size="Medium" Height="23px" OnTextChanged="txtPerFrom_TextChanged"
                                                Width="40px"></asp:TextBox>
                                            <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" runat="server" FilterType="Numbers"
                                                TargetControlID="txtPerFrom" />
                                        </td>
                                        <td>
                                            <asp:Label ID="lblToPer" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                                Font-Size="Medium" Text="To"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtPerTo" runat="server" AutoPostBack="true" Font-Bold="True" Font-Names="Book Antiqua"
                                                Font-Size="Medium" Height="23px" OnTextChanged="txtPerTo_TextChanged" Width="40px"></asp:TextBox>
                                            <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender3" runat="server" FilterType="Numbers"
                                                TargetControlID="txtPerTo" />
                                        </td>
                                        <td>
                                            <asp:Label ID="lblMinPresentML" runat="server" Text="Minimum Percentage For ML Consideration"
                                                Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtMinPresentML" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                                Font-Size="Medium" Height="23px" Width="40px" Text="70"></asp:TextBox><b style="font-family: Book Antiqua;
                                                    font-size: medium; font-weight: bold;">%</b>
                                            <asp:FilteredTextBoxExtender ID="filtertxtMinPresentML" runat="server" FilterType="Numbers"
                                                TargetControlID="txtMinPresentML" />
                                        </td>
                                        <td>
                                            <asp:UpdatePanel ID="UpdatePanel_go" runat="server">
                                                <ContentTemplate>
                                                    <asp:Button ID="btnGo" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                                        Font-Size="Medium" OnClick="btnGo_Click" Text="Go" />
                                                </ContentTemplate>
                                            </asp:UpdatePanel>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                    </table>
                </div>
            </center>
            <asp:Label ID="lblErrSearch" runat="server" CssClass="noprint" Text="" ForeColor="Red"
                Visible="False" Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium"
                Style="margin: 0px; margin-bottom: 10px; margin-top: 10px; position: relative;"></asp:Label>
            <div id="divMainContents" runat="server" style="display: table; margin: 0px; height: auto;
                margin-bottom: 20px; margin-top: 10px; position: relative; width: auto; text-align: left;">
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
                    HeaderStyle-BackColor="#0CA6CA" ShowHeaderWhenEmpty="true" OnRowDataBound="Showgrid_OnRowDataBound">
                </asp:GridView>
            </div>
            <center>
                <div id="rptprint1" class="noprint" runat="server" visible="false" style="margin: 0px;
                    margin-bottom: 20px; margin-top: 15px; position: relative;">
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
                    <asp:Button ID="btnExcel" runat="server" Font-Bold="true" Font-Names="Book Antiqua"
                        OnClick="btnExcel_Click" Font-Size="Medium" Text="Export To Excel" Width="127px"
                        Height="35px" CssClass="textbox textbox1" />
                    <asp:Button ID="btnprintmaster" runat="server" Text="Print" OnClick="btnprintmaster_Click"
                        Font-Names="Book Antiqua" Font-Size="Medium" Font-Bold="true" Width="60px" Height="35px"
                        CssClass="textbox textbox1" />
                    <NEW:NEWPrintMater runat="server" ID="Printcontrol" Visible="false" />
                    <asp:Button ID="btnPrint" runat="server" Text="Direct Print" OnClientClick="return PrintPanel();"
                        Font-Names="Book Antiqua" Font-Size="Medium" Font-Bold="true" Height="35px" CssClass="textbox textbox1" />
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
                                            <asp:Label ID="lblpopuperr" runat="server" Text="" Style="color: Red;" Font-Bold="true"
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
                <div id="divPopAlert" runat="server" visible="false" style="height: 550em; z-index: 2000;
                    width: 100%; background-color: rgba(54, 25, 25, .2); position: absolute; top: 0%;
                    left: 0%;">
                    <center>
                        <div id="divPopAlertContent" runat="server" class="table" style="background-color: White;
                            height: auto; width: auto; border: 5px solid #0CA6CA; border-top: 25px solid #0CA6CA;
                            left: 39%; right: 39%; top: 35%; padding: 5px; position: fixed; border-radius: 10px;">
                            <center>
                                <table style="height: 100px; width: 100%; padding: 5px;">
                                    <tr>
                                        <td align="center">
                                            <asp:Label ID="lblAlertMsg" runat="server" Style="color: Red;" Font-Bold="true" Font-Size="Medium"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <center>
                                                <asp:Button ID="btnPopAlertClose" Font-Bold="true" Font-Size="Medium" Font-Names="Book Antiqua"
                                                    CssClass="textbox textbox1" Style="height: auto; width: auto;" OnClick="btnPopAlertClose_Click"
                                                    Text="Ok" runat="server" />
                                            </center>
                                        </td>
                                    </tr>
                                </table>
                            </center>
                        </div>
                    </center>
                </div>
            </center>
        </ContentTemplate>
        <Triggers>
            <asp:PostBackTrigger ControlID="btnExcel" />
            <asp:PostBackTrigger ControlID="btnprintmaster" />
            <asp:PostBackTrigger ControlID="btnPrint" />
            <asp:PostBackTrigger ControlID="btnGo" />
        </Triggers>
    </asp:UpdatePanel>
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
    <%--progressBar for Sem--%>
    <%--<center>
        <asp:UpdateProgress ID="UpdateProgress2" runat="server" AssociatedUpdatePanelID="UpdatePanel_sem">
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
        <asp:ModalPopupExtender ID="ModalPopupExtender2" runat="server" TargetControlID="UpdateProgress2"
            PopupControlID="UpdateProgress2">
        </asp:ModalPopupExtender>
    </center>--%>
</asp:Content>

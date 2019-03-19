<%@ Page Title="" Language="C#" MasterPageFile="~/MarkMod/CAMSubSiteMaster.master"
    AutoEventWireup="true" CodeFile="IndividualStudentTestWisePerformance.aspx.cs"
    Inherits="MarkMod_IndividualStudentTestWisePerformance" %>

<%@ Register Assembly="FarPoint.Web.Spread,  Version=5.0.3520.2008, Culture=neutral, PublicKeyToken=327c3516b1b18457"
    Namespace="FarPoint.Web.Spread" TagPrefix="FarPoint" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <link href="~/Styles/css/Commoncss.css" rel="Stylesheet" type="text/css" />
    <style type="text/css">
        .fontStyle
        {
            font-size: medium;
            font-weight: bolder;
            font-style: oblique;
            padding: 5px;
        }
        .fontStyle1
        {
            font-size: medium;
            font-style: oblique;
            padding: 3px;
            color: Blue;
        }
        .commonHeaderFont
        {
            font-size: medium;
            color: Black;
            font-family: 'Book Antiqua';
            font-weight: bold;
        }
    </style>
    <script type="text/javascript">
        function PrintDiv() {
            var panel = document.getElementById("<%=contentDiv.ClientID %>");
            var printWindow = window.open('', '', 'height=auto,width=1191');
            printWindow.document.write('<html');
            printWindow.document.write('<head> <style type="text/css"> p{ font-size: x-small;margin: 0px; padding: 0px; border: 0px;  } body{ margin:0px;}</style>');
            printWindow.document.write('</head><body>');
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
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <center>
        <div>
            <span id="spPageHeading" runat="server" class="fontstyleheader" style="color: Green;
                margin: 0px; margin-bottom: 10px; margin-top: 10px; position: relative;">Individual
                Student Academic Performance</span>
        </div>
        <div id="divSearch" runat="server" visible="true" class="maindivstyle" style="width: 100%;
            height: auto; margin: 0px; margin-bottom: 20px; margin-top: 10px; padding: 5px;
            position: relative;">
            <table class="maintablestyle" style="height: auto; margin-left: 0px; margin-top: 10px;
                margin-bottom: 10px; padding: 6px;">
                <tr>
                    <td>
                        <asp:Label ID="lblCollege" runat="server" Text="College" CssClass="commonHeaderFont"
                            AssociatedControlID="ddlCollege"></asp:Label>
                    </td>
                    <td>
                        <asp:DropDownList ID="ddlCollege" runat="server" CssClass="dropdown commonHeaderFont"
                            Width="200px" AutoPostBack="True" OnSelectedIndexChanged="ddlCollege_SelectedIndexChanged">
                        </asp:DropDownList>
                    </td>
                    <td>
                        <asp:Label ID="lblBatch" runat="server" Text="Batch" CssClass="commonHeaderFont"
                            AssociatedControlID="ddlBatch"></asp:Label>
                    </td>
                    <td>
                        <asp:DropDownList ID="ddlBatch" runat="server" CssClass="commonHeaderFont" OnSelectedIndexChanged="ddlBatch_SelectedIndexChanged"
                            AutoPostBack="True" Width="80px">
                        </asp:DropDownList>
                    </td>
                    <td>
                        <asp:Label ID="lblDegree" runat="server" CssClass="commonHeaderFont" Text="Degree"
                            AssociatedControlID="ddlDegree"></asp:Label>
                    </td>
                    <td>
                        <asp:DropDownList ID="ddlDegree" runat="server" CssClass="commonHeaderFont" OnSelectedIndexChanged="ddlDegree_SelectedIndexChanged"
                            AutoPostBack="True" Width="80px">
                        </asp:DropDownList>
                    </td>
                    <td>
                        <asp:Label ID="lblBranch" runat="server" CssClass="commonHeaderFont" Text="Branch"
                            AssociatedControlID="ddlBranch"></asp:Label>
                    </td>
                    <td>
                        <asp:DropDownList ID="ddlBranch" runat="server" CssClass="commonHeaderFont" OnSelectedIndexChanged="ddlBranch_SelectedIndexChanged"
                            AutoPostBack="True" Width="150px">
                        </asp:DropDownList>
                    </td>
                    <td>
                        <asp:Label ID="lblSem" runat="server" CssClass="commonHeaderFont" Text="Sem" AssociatedControlID="ddlSem"></asp:Label>
                    </td>
                    <td>
                        <asp:DropDownList ID="ddlSem" runat="server" CssClass="commonHeaderFont" OnSelectedIndexChanged="ddlSem_SelectedIndexChanged"
                            AutoPostBack="True" Width="40px">
                        </asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="lblSec" runat="server" Text="Section" CssClass="commonHeaderFont"
                            AssociatedControlID="ddlSec"></asp:Label>
                    </td>
                    <td>
                        <asp:DropDownList ID="ddlSec" runat="server" CssClass="commonHeaderFont" OnSelectedIndexChanged="ddlSec_SelectedIndexChanged"
                            AutoPostBack="True" Width="120px">
                        </asp:DropDownList>
                    </td>
                    <td>
                        <asp:Label ID="lblTest" runat="server" Text="Test" CssClass="commonHeaderFont" AssociatedControlID="ddlTest"></asp:Label>
                    </td>
                    <td>
                        <div style="position: relative;">
                            <asp:UpdatePanel ID="upnlTest" runat="server">
                                <ContentTemplate>
                                    <asp:TextBox ID="txtTest" Visible="false" Width="76px" runat="server" CssClass="textbox  txtheight2 commonHeaderFont"
                                        ReadOnly="true">-- Select --</asp:TextBox>
                                    <asp:Panel ID="pnlTest" Visible="false" runat="server" CssClass="multxtpanel" Height="200px"
                                        Width="280px">
                                        <asp:CheckBox ID="chkTest" CssClass="commonHeaderFont" runat="server" Text="Select All"
                                            AutoPostBack="True" OnCheckedChanged="chkTest_CheckedChanged" />
                                        <asp:CheckBoxList ID="cblTest" CssClass="commonHeaderFont" runat="server" AutoPostBack="True"
                                            OnSelectedIndexChanged="cblTest_SelectedIndexChanged">
                                        </asp:CheckBoxList>
                                    </asp:Panel>
                                    <asp:PopupControlExtender ID="popExtTest" runat="server" TargetControlID="txtTest"
                                        PopupControlID="pnlTest" Position="Bottom">
                                    </asp:PopupControlExtender>
                                    <asp:DropDownList ID="ddlTest" runat="server" Visible="true" CssClass="commonHeaderFont"
                                        Width="80px">
                                    </asp:DropDownList>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </div>
                    </td>
                    <td>
                        <asp:Button ID="btnGo" CssClass="textbox textbox1 commonHeaderFont" runat="server"
                            OnClick="btnGo_Click" Text="Go" Style="width: auto; height: auto;" />
                    </td>
                    <td>
                        <asp:Button ID="btnPrint" CssClass="textbox textbox1 commonHeaderFont" Visible="false"
                            runat="server" OnClick="btnPrint_Click" Text="Print" Style="width: auto; height: auto;" />
                    </td>
                    <td>
                        <asp:Label ID="lblconvertions" runat="server" Text="Convert" CssClass="commonHeaderFont"></asp:Label>
                    </td>
                    <td>
                        <asp:TextBox ID="txt_Convertion" Width="76px" runat="server" MaxLength="3" CssClass="textbox  txtheight2 commonHeaderFont"></asp:TextBox>
                        <asp:FilteredTextBoxExtender ID="filterConvert" runat="server" TargetControlID="txt_Convertion"
                            FilterType="Numbers">
                        </asp:FilteredTextBoxExtender>
                    </td>
                </tr>
            </table>
           
            <asp:Label ID="lblErrSearch" runat="server" Text="" ForeColor="Red" Font-Bold="True"
                Font-Names="Book Antiqua" Font-Size="Medium" Style="margin: 0px; margin-bottom: 15px;
                margin-top: 10px;"></asp:Label>
            <div id="ShowReport" runat="server" visible="false">
                <div>
                    <FarPoint:FpSpread ID="attnd_report" runat="server" ShowHeaderSelection="false" OnButtonCommand="attnd_report_Command_Click">
                        <Sheets>
                            <FarPoint:SheetView SheetName="Sheet1" AutoPostBack="True">
                            </FarPoint:SheetView>
                        </Sheets>
                    </FarPoint:FpSpread>
                </div>
            </div>
        </div>
    </center>
    <%-- Confirmation --%>
    <center>
        <div id="divConfirmBox" runat="server" visible="false" style="height: 550em; z-index: 1000;
            width: 100%; background-color: rgba(54, 25, 25, .2); position: absolute; top: 0;
            left: 0px;">
            <center>
                <div id="divConfirm" runat="server" class="table" style="background-color: White;
                    height: auto; width: 38%; border: 5px solid #0CA6CA; border-top: 25px solid #0CA6CA;
                    left: 30%; right: 30%; top: 40%; position: fixed; border-radius: 10px;">
                    <center>
                        <table style="height: auto; width: 100%; padding: 3px;">
                            <tr>
                                <td align="center">
                                    <asp:Label ID="lblConfirmMsg" runat="server" Text="Do You Want To Delete All Subject Remarks?"
                                        Style="color: Red;" Font-Bold="true" Font-Size="Medium"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <center>
                                        <asp:Button ID="btnYes" CssClass=" textbox btn1 textbox1" Style="height: 28px; width: 65px;"
                                            OnClick="btnYes_Click" Text="Yes" runat="server" />
                                        <asp:Button ID="btnNo" CssClass=" textbox btn1 textbox1" Style="height: 28px; width: 65px;"
                                            OnClick="btnNo_Click" Text="No" runat="server" />
                                    </center>
                                </td>
                            </tr>
                        </table>
                    </center>
                </div>
            </center>
        </div>
    </center>
    <%-- Alert Box --%>
    <center>
        <div id="divPopAlert" runat="server" visible="false" style="height: 550em; z-index: 2000;
            width: 100%; background-color: rgba(54, 25, 25, .2); position: absolute; top: 0%;
            left: 0%;">
            <center>
                <div id="divPopAlertContent" runat="server" class="table" style="background-color: White;
                    height: 120px; width: 23%; border: 5px solid #0CA6CA; border-top: 25px solid #0CA6CA;
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
    <div style="height: 1px; width: 1px; overflow: auto;">
        <div id="contentDiv" runat="server" style="height: auto; width: 1344px;" visible="false">
        </div>
    </div>
</asp:Content>

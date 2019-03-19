<%@ Page Title="" Language="C#" MasterPageFile="~/FeedBackMOD/FeedbackSubSiteMaster.master"
    AutoEventWireup="true" CodeFile="Feedbackreport_consolidation.aspx.cs" Inherits="FeedBackMOD_Feedbackreport_consolidation" %>

<%@ Register Src="~/Usercontrols/PrintMaster.ascx" TagName="printmaster" TagPrefix="Insproplus" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Assembly="FarPoint.Web.Spread" Namespace="FarPoint.Web.Spread" TagPrefix="FarPoint" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <link href="~/Styles/css/Commoncss.css" rel="Stylesheet" type="text/css" />
    <script type="text/javascript" src="//ajax.googleapis.com/ajax/libs/jquery/1.11.1/jquery.min.js"></script>
    <style type="text/css">
        .rdbstyle input[type=radio]
        {
            display: none;
        }
        .rdbstyle input[type=radio] + label
        {
            display: inline-block;
            margin: -2px;
            padding: 4px 12px;
            margin-bottom: 0;
            font-size: 14px;
            line-height: 20px;
            color: #993399;
            text-align: center;
            text-shadow: 0 1px 1px rgba(255,255,255,0.75);
            vertical-align: middle;
            cursor: pointer;
            background-color: #f5f5f5;
            background-image: -moz-linear-gradient(top,#fff,#e6e6e6);
            background-image: -webkit-gradient(linear,0 0,0 100%,from(#fff),to(#e6e6e6));
            background-image: -webkit-linear-gradient(top,#fff,#e6e6e6);
            background-image: -o-linear-gradient(top,#fff,#e6e6e6);
            background-image: linear-gradient(to bottom,#fff,#e6e6e6);
            background-repeat: repeat-x;
            border: 1px solid #ccc;
            border-color: #e6e6e6 #e6e6e6 #bfbfbf;
            border-color: rgba(0,0,0,0.1) rgba(0,0,0,0.1) rgba(0,0,0,0.25);
            border-bottom-color: #b3b3b3;
            filter: progid:DXImageTransform.Microsoft.gradient(startColorstr='#ffffffff',endColorstr='#ffe6e6e6',GradientType=0);
            filter: progid:DXImageTransform.Microsoft.gradient(enabled=false);
            -webkit-box-shadow: inset 0 1px 0 rgba(255,255,255,0.2),0 1px 2px rgba(0,0,0,0.05);
            -moz-box-shadow: inset 0 1px 0 rgba(255,255,255,0.2),0 1px 2px rgba(0,0,0,0.05);
            box-shadow: inset 0 1px 0 rgba(255,255,255,0.2),0 1px 2px rgba(0,0,0,0.05);
        }
        .rdbstyle input[type=radio]:checked + label
        {
            background-image: none;
            outline: 0;
            -webkit-box-shadow: inset 0 2px 4px rgba(0,0,0,0.15),0 1px 2px rgba(0,0,0,0.05);
            -moz-box-shadow: inset 0 2px 4px rgba(0,0,0,0.15),0 1px 2px rgba(0,0,0,0.05);
            box-shadow: inset 0 2px 4px rgba(0,0,0,0.15),0 1px 2px rgba(0,0,0,0.05);
            border-bottom-color: #b3b3b3;
            border-bottom-style: solid;
            border-bottom-color: #89D17C;
            border-bottom-width: medium;
        }
        .btnapprove1
        {
            background: transparent;
        }
        .btnapprove1:hover
        {
            background-color: Orange;
            color: White;
        }
        .lnk:hover
        {
            text-shadow: 0 1px 1px rgba(255,255,255,0.75);
            color: Green;
            font-weight: bold;
        }
    </style>
    <script type="text/javascript">
        function PrintPanel() {
            var panel = "";
            panel = document.getElementById("<%=chartdiv.ClientID %>");
            var printWindow = window.open('', '', 'height=auto,width=auto');
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
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <center>
        <div style="width: 1000px;">
            <center>
                <div>
                    <span class="fontstyleheader" style="color: Green">FeedBack Report </span>
                </div>
                <br />
            </center>
            <asp:RadioButtonList ID="rdbtype" runat="server" RepeatDirection="Horizontal" CellSpacing="4"
                BorderColor="#999999" Style="margin-left: -641px;" Font-Bold="True" CssClass="rdbstyle"
                OnSelectedIndexChanged="rdbtype_SelectedIndexChanged" AutoPostBack="true">
                <asp:ListItem Value="0">Faculty wise</asp:ListItem>
                <asp:ListItem Value="1">Department wise</asp:ListItem>
                <asp:ListItem Value="2">College wise</asp:ListItem>
            </asp:RadioButtonList>
            <center>
                <div class="maindivstyle" style="width: 1000px;">
                    <center>
                        <table runat="server">
                            <tr>
                                <td style="width: 110px;">
                                    College Name
                                </td>
                                <td colspan="2">
                                    <asp:UpdatePanel ID="UpdatePanel4" runat="server">
                                        <ContentTemplate>
                                            <asp:TextBox ID="txtclgnameformat6" ReadOnly="true" runat="server" CssClass="textbox  txtheight5">--Select--</asp:TextBox>
                                            <asp:Panel ID="Panel1" runat="server" CssClass="multxtpanel">
                                                <asp:CheckBox ID="cb_clgnameformat6" runat="server" Text="Select All" AutoPostBack="True"
                                                    OnCheckedChanged="cb_clgnameformat6_CheckedChanged" />
                                                <asp:CheckBoxList ID="cbl_clgnameformat6" runat="server" AutoPostBack="True" OnSelectedIndexChanged="cbl_clgnameformat6_SelectedIndexChanged">
                                                </asp:CheckBoxList>
                                            </asp:Panel>
                                            <asp:PopupControlExtender ID="PopupControlExtender2" runat="server" TargetControlID="txtclgnameformat6"
                                                PopupControlID="Panel1" Position="Bottom">
                                            </asp:PopupControlExtender>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                                <td>
                                    <asp:Label ID="lbl_formate6batch" runat="server" Text="Batch Year"></asp:Label>
                                </td>
                                <td>
                                    <asp:UpdatePanel ID="UpdatePanel121" runat="server">
                                        <ContentTemplate>
                                            <asp:TextBox ID="txt_formate6batch" ReadOnly="true" Width=" 90px" runat="server"
                                                CssClass="textbox  txtheight2">--Select--</asp:TextBox>
                                            <asp:Panel ID="Panel41" runat="server" CssClass="multxtpanel">
                                                <asp:CheckBox ID="cb_formate6batch" runat="server" Text="Select All" AutoPostBack="true"
                                                    OnCheckedChanged="cb_formate6batch_CheckedChanged" />
                                                <asp:CheckBoxList ID="cbl_formate6batch" runat="server" AutoPostBack="true" OnSelectedIndexChanged="cbl_formate6batch_SelectedIndexChanged">
                                                </asp:CheckBoxList>
                                            </asp:Panel>
                                            <asp:PopupControlExtender ID="PopupControlExtender71" runat="server" TargetControlID="txt_formate6batch"
                                                PopupControlID="Panel41" Position="Bottom">
                                            </asp:PopupControlExtender>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                                <td>
                                    Department
                                </td>
                                <td>
                                    <asp:DropDownList ID="ddlformate6_deptname" runat="server" OnSelectedIndexChanged="ddlformate6_deptname_selectedindex"
                                        AutoPostBack="true" CssClass="textbox1 ddlheight5">
                                    </asp:DropDownList>
                                    <%-- <asp:UpdatePanel ID="UpdatePanel9" runat="server">
                            <ContentTemplate>
                                <asp:TextBox ID="txtdeptnameformat6" ReadOnly="true" runat="server" CssClass="textbox  txtheight4">--Select--</asp:TextBox>
                                <asp:Panel ID="Panel2" runat="server" CssClass="multxtpanel" Height="250px">
                                    <asp:CheckBox ID="cb_deptnameformat6" runat="server" Text="Select All" AutoPostBack="True"
                                        OnCheckedChanged="cb_deptnameformat6_CheckedChanged" />
                                    <asp:CheckBoxList ID="cbl_deptnameformat6" runat="server" AutoPostBack="True" OnSelectedIndexChanged="cbl_deptnameformat6_SelectedIndexChanged">
                                    </asp:CheckBoxList>
                                </asp:Panel>
                                <asp:PopupControlExtender ID="PopupControlExtender3" runat="server" TargetControlID="txtdeptnameformat6"
                                    PopupControlID="Panel2" Position="Bottom">
                                </asp:PopupControlExtender>
                            </ContentTemplate>
                        </asp:UpdatePanel>--%>
                                </td>
                            </tr>
                            <tr>
                                <td style="width: 120px;">
                                    Feedback Name
                                </td>
                                <td colspan="2">
                                    <asp:UpdatePanel ID="UpdatePanel10" Visible="true" runat="server">
                                        <ContentTemplate>
                                            <asp:DropDownList ID="ddl_feedbackformate6" runat="server" Width="260px" Height="30px"
                                                CssClass=" textbox1 ddlheight5" AutoPostBack="true" OnSelectedIndexChanged="ddl_feedbackformate6_SelectedIndexChanged">
                                            </asp:DropDownList>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                                <td>
                                    Semester
                                </td>
                                <td>
                                    <asp:UpdatePanel ID="UpdatePanel13" runat="server">
                                        <ContentTemplate>
                                            <asp:TextBox ID="txt_formate6sem" ReadOnly="true" runat="server" Width="90px" CssClass="textbox  txtheight">--Select--</asp:TextBox>
                                            <asp:Panel ID="pformate6" runat="server" CssClass="multxtpanel" Height="250px">
                                                <asp:CheckBox ID="cb_formate6sem" runat="server" Text="Select All" AutoPostBack="True"
                                                    OnCheckedChanged="cb_formate6sem_CheckedChanged" />
                                                <asp:CheckBoxList ID="cbl_formate6sem" runat="server" AutoPostBack="True" OnSelectedIndexChanged="cbl_formate6sem_SelectedIndexChanged">
                                                </asp:CheckBoxList>
                                            </asp:Panel>
                                            <asp:PopupControlExtender ID="PopupControlExtender8" runat="server" TargetControlID="txt_formate6sem"
                                                PopupControlID="pformate6" Position="Bottom">
                                            </asp:PopupControlExtender>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                                <td>
                                    Staff Name
                                </td>
                                <td>
                                    <asp:UpdatePanel ID="UpdatePanel11" runat="server">
                                        <ContentTemplate>
                                            <asp:TextBox ID="txtstaffnameformat6" ReadOnly="true" runat="server" CssClass="textbox  txtheight5"
                                                Width="240px">--Select--</asp:TextBox>
                                            <asp:Panel ID="Panel3" runat="server" CssClass="multxtpanel" Height="200px" Width="200px">
                                                <asp:CheckBox ID="cb_staffnameformat6" runat="server" Text="Select All" AutoPostBack="True"
                                                    OnCheckedChanged="cb_staffnameformat6_CheckedChanged" />
                                                <asp:CheckBoxList ID="cbl_staffnameformat6" runat="server" AutoPostBack="True" OnSelectedIndexChanged="cbl_staffnameformat6_SelectedIndexChanged">
                                                </asp:CheckBoxList>
                                            </asp:Panel>
                                            <asp:PopupControlExtender ID="PopupControlExtender4" runat="server" TargetControlID="txtstaffnameformat6"
                                                PopupControlID="Panel3" Position="Bottom">
                                            </asp:PopupControlExtender>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    Subject Name
                                </td>
                                <td>
                                    <asp:UpdatePanel ID="UpdatePanel12" Visible="true" runat="server">
                                        <ContentTemplate>
                                            <asp:TextBox ID="txtsubjectnameformat6" ReadOnly="true" runat="server" CssClass="textbox  txtheight1"
                                                Width="162px">--Select--</asp:TextBox>
                                            <asp:Panel ID="Panel4" runat="server" CssClass="multxtpanel" Height="250px" Width="250px">
                                                <asp:CheckBox ID="cb_subjectnameformat6" runat="server" Text="Select All" AutoPostBack="True"
                                                    OnCheckedChanged="cb_subjectnameformat6_CheckedChanged" />
                                                <asp:CheckBoxList ID="cbl_subjectnameformat6" runat="server" AutoPostBack="True"
                                                    OnSelectedIndexChanged="cbl_subjectnameformat6_SelectedIndexChanged">
                                                </asp:CheckBoxList>
                                            </asp:Panel>
                                            <asp:PopupControlExtender ID="PopupControlExtender7" runat="server" TargetControlID="txtsubjectnameformat6"
                                                PopupControlID="Panel4" Position="Bottom">
                                            </asp:PopupControlExtender>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                                <td>
                                    <asp:Button ID="btn_go" runat="server" OnClick="btn_Go_Click" Text="Go" CssClass="textbox btn1" />
                                </td>
                                <td>
                                    <asp:CheckBox ID="cb_include" runat="server" Text="InClude Pie Chart" />
                                </td>
                            </tr>
                        </table>
                    </center>
                </div>
                <br />
                <asp:Label ID="lbl_error" runat="server" ForeColor="Red"></asp:Label>
                <br />
                <center>
                    <div>
                        <FarPoint:FpSpread ID="FpSpread1" runat="server" Visible="false" BorderStyle="Solid"
                            BorderWidth="0px" CssClass="spreadborder" ShowHeaderSelection="false">
                            <Sheets>
                                <FarPoint:SheetView SheetName="Sheet1">
                                </FarPoint:SheetView>
                            </Sheets>
                        </FarPoint:FpSpread>
                    </div>
                </center>
                <center>
                    <div id="rptprint1" runat="server" visible="false">
                        <br />
                        <asp:Label ID="lbl_norec1" runat="server" ForeColor="Red" Text="Please Enter Your Report Name"
                            Visible="false"></asp:Label>
                        <asp:Label ID="lblrptname1" runat="server" Text="Report Name"></asp:Label>
                        <asp:TextBox ID="txtexcelname1" runat="server" CssClass="textbox textbox1" Height="20px"
                            Width="180px" onkeypress="display1()"></asp:TextBox>
                        <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender9" runat="server" TargetControlID="txtexcelname1"
                            FilterType="LowercaseLetters,UppercaseLetters,Numbers,Custom" ValidChars="(),.[]_"
                            InvalidChars="/\">
                        </asp:FilteredTextBoxExtender>
                        <asp:Button ID="btnExcel1" runat="server" OnClick="btnExcel1_Click" Text="Export To Excel"
                            Width="127px" Height="31px" CssClass="textbox textbox1" />
                        <asp:Button ID="btnprintmaster1" runat="server" Text="Print" OnClick="btnprintmaster1_Click"
                            Width="60px" Height="31px" CssClass="textbox textbox1" />
                        <Insproplus:printmaster runat="server" ID="Printcontrol1" Visible="false" />
                    </div>
                    <br />
                </center>
                <div id="chartdiv" runat="server">
                    <asp:Chart ID="Chart1" runat="server" BorderlineWidth="0" Height="500px" Palette="None"
                        Width="900px" PaletteCustomColors="gold" Visible="false">
                        <Titles>
                            <asp:Title Docking="Top" Font="Microsoft Sans Serif, 12pt">
                            </asp:Title>
                        </Titles>
                        <Legends>
                            <asp:Legend Alignment="Center" Docking="Bottom" IsTextAutoFit="false" Name="Default"
                                LegendStyle="Row" MaximumAutoSize="100" />
                        </Legends>
                        <Series>
                            <asp:Series Name="Default" />
                        </Series>
                        <ChartAreas>
                            <asp:ChartArea Name="ChartArea1" BorderWidth="1" Area3DStyle-Enable3D="True" Area3DStyle-LightStyle="Realistic">
                            </asp:ChartArea>
                        </ChartAreas>
                    </asp:Chart>
                </div>
                <br />
                <asp:Button ID="btnprintimag" Text="Print" Visible="false" Height="30px" runat="server"
                    CssClass="btn1 textbox " OnClientClick="return PrintPanel();" />
            </center>
        </div>
    </center>
</asp:Content>

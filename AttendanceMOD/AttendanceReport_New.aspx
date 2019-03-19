<%@ Page Title="" Language="C#" MasterPageFile="~/AttendanceMOD/AttendanceSubSiteMaster.master"
    AutoEventWireup="true" CodeFile="AttendanceReport_New.aspx.cs" Inherits="AttendanceReport_New" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Src="~/Usercontrols/NewPrintMaster.ascx" TagName="NEWPrintMater" TagPrefix="NEW" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">
    <script type="text/javascript">
        function display() {
            document.getElementById('MainContent_lblexcelerror').innerHTML = "";
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
    <style type="text/css">
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
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <center>
        <div>
            <br />
            <span class="fontstyleheader" style="color: Green;">AT21-Attendance Report</span>
            <br />
        </div>
        <asp:UpdatePanel ID="UpdatePanel4" runat="server">
            <ContentTemplate>
                <div>
                    <table class="maintablestyle">
                        <tr>
                            <td>
                                <asp:Label ID="lblBatch" runat="server" Text="Batch" Font-Bold="True" Font-Names="Book Antiqua"
                                    Font-Size="Medium" Style=""></asp:Label>
                            </td>
                            <td>
                                <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                                    <ContentTemplate>
                                        <asp:TextBox ID="txt_batch" runat="server" CssClass="Dropdown_Txt_Box" ReadOnly="true"
                                            Width="120px" Height="20px" Font-Bold="True" Font-Names="Book Antiqua" Style=""
                                            Font-Size="Medium">---Select---</asp:TextBox>
                                        <asp:Panel ID="pbatch" runat="server" CssClass="multxtpanel">
                                            <asp:CheckBox ID="chk_batch" runat="server" Width="100px" Font-Bold="True" Font-Names="Book Antiqua"
                                                OnCheckedChanged="chk_batch_ChekedChanged" Font-Size="Medium" Text="Select All"
                                                AutoPostBack="True" />
                                            <asp:CheckBoxList ID="chklst_batch" runat="server" Font-Size="Medium" AutoPostBack="True"
                                                OnSelectedIndexChanged="chklst_batch_SelectedIndexChanged" Width="100px" Font-Bold="True"
                                                Font-Names="Book Antiqua">
                                            </asp:CheckBoxList>
                                        </asp:Panel>
                                        <asp:PopupControlExtender ID="pextendbatch" runat="server" TargetControlID="txt_batch"
                                            PopupControlID="pbatch" Position="Bottom">
                                        </asp:PopupControlExtender>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>
                            <td>
                                <asp:Label ID="lbldefree" runat="server" Text="Degree" Font-Bold="True" Font-Names="Book Antiqua"
                                    Font-Size="Medium" Style=""></asp:Label>
                            </td>
                            <td>
                                <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                                    <ContentTemplate>
                                        <asp:TextBox ID="txt_degree" runat="server" CssClass="Dropdown_Txt_Box" ReadOnly="true"
                                            Width="120px" Height="20px" Font-Bold="True" Font-Names="Book Antiqua" Style=""
                                            Font-Size="Medium">---Select---</asp:TextBox>
                                        <asp:Panel ID="pdegree" runat="server" Height="250px" CssClass="multxtpanel">
                                            <asp:CheckBox ID="chk_degree" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                                OnCheckedChanged="chk_degree_ChekedChanged" AutoPostBack="true" Font-Size="Medium"
                                                Text="Select All" />
                                            <asp:CheckBoxList ID="chklst_degree" runat="server" Font-Size="Medium" AutoPostBack="True"
                                                OnSelectedIndexChanged="chklst_degree_SelectedIndexChanged" Width="100px" Font-Bold="True"
                                                Font-Names="Book Antiqua">
                                            </asp:CheckBoxList>
                                        </asp:Panel>
                                        <asp:PopupControlExtender ID="pextenddegree" runat="server" TargetControlID="txt_degree"
                                            PopupControlID="pdegree" Position="Bottom">
                                        </asp:PopupControlExtender>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>
                            <td>
                                <asp:Label ID="lblbranch" runat="server" Text="Branch" Font-Bold="True" Font-Names="Book Antiqua"
                                    Font-Size="Medium" Style=""></asp:Label>
                            </td>
                            <td>
                                <asp:UpdatePanel ID="UpdatePanel3" runat="server">
                                    <ContentTemplate>
                                        <asp:TextBox ID="txt_branch" runat="server" CssClass="Dropdown_Txt_Box" ReadOnly="true"
                                            Width="120px" Height="20px" Font-Bold="True" Font-Names="Book Antiqua" Style=""
                                            Font-Size="Medium">---Select---</asp:TextBox>
                                        <asp:Panel ID="pbranch" runat="server" Height="350px" CssClass="multxtpanel">
                                            <asp:CheckBox ID="chk_branch" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                                OnCheckedChanged="chk_branch_ChekedChanged" Font-Size="Medium" Text="Select All"
                                                AutoPostBack="True" />
                                            <asp:CheckBoxList ID="chklst_branch" runat="server" Font-Size="Medium" AutoPostBack="True"
                                                OnSelectedIndexChanged="chklst_branch_SelectedIndexChanged" Font-Bold="True"
                                                Font-Names="Book Antiqua">
                                            </asp:CheckBoxList>
                                        </asp:Panel>
                                        <asp:PopupControlExtender ID="pextendbranch" runat="server" TargetControlID="txt_branch"
                                            PopupControlID="pbranch" Position="Bottom">
                                        </asp:PopupControlExtender>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>
                            <td>
                                <asp:RadioButton ID="rbtest" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                    Font-Size="Medium" Text="Test Wise" OnCheckedChanged="radiocheched" AutoPostBack="true"
                                    GroupName="Test" Style="" />
                                <asp:RadioButton ID="rboverall" runat="server" Width="100px" Font-Bold="True" Font-Names="Book Antiqua"
                                    Font-Size="Medium" Text="Over All" OnCheckedChanged="radiocheched" AutoPostBack="true"
                                    GroupName="Test" Style="" />
                            </td>
                        </tr>
                        <tr>
                            <td colspan="8">
                                <asp:Label ID="lblfrom" runat="server" Text="From" Font-Bold="True" Font-Names="Book Antiqua"
                                    Font-Size="Medium" Style=""></asp:Label>
                                <asp:TextBox ID="txtfrom" runat="server" Width="57px" Font-Bold="True" Font-Names="Book Antiqua"
                                    Font-Size="Medium" AutoPostBack="true" OnTextChanged="rangecheck" Height="20px"
                                    Style="" MaxLength="3"></asp:TextBox>
                                <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender2" runat="server" TargetControlID="txtfrom"
                                    FilterType="Numbers" />
                                <asp:Label ID="lblto" runat="server" Text="To" Font-Bold="True" Font-Names="Book Antiqua"
                                    Font-Size="Medium" Style=""></asp:Label>
                                <asp:TextBox ID="txtto" runat="server" Width="57px" Font-Bold="True" Font-Names="Book Antiqua"
                                    Font-Size="Medium" Height="20px" AutoPostBack="false" OnTextChanged="rangecheck"
                                    Style="" MaxLength="3"></asp:TextBox>
                                <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" runat="server" TargetControlID="txtto"
                                    FilterType="Numbers" />
                                <asp:Label ID="lblfdate" runat="server" Text="From Date" Style="margin-left: 85px;"
                                    Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium"></asp:Label>
                                <asp:TextBox ID="txtfdate" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                    Font-Size="Medium" AutoPostBack="false" OnTextChanged="datechange" Width="80px"
                                    Style=""></asp:TextBox>
                                <asp:CalendarExtender ID="CalendarExtender1" Format="d/MM/yyyy" TargetControlID="txtfdate"
                                    runat="server">
                                </asp:CalendarExtender>
                                <asp:Label ID="lbltdate" runat="server" Font-Bold="True" Text="To Date" Font-Names="Book Antiqua"
                                    Font-Size="Medium" Style=""></asp:Label>
                                <asp:TextBox ID="txttdate" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                    Font-Size="Medium" AutoPostBack="true" OnTextChanged="datechange" Width="80px"
                                    Style=""></asp:TextBox>
                                <asp:CalendarExtender ID="CalendarExtender2" Format="d/MM/yyyy" TargetControlID="txttdate"
                                    runat="server">
                                </asp:CalendarExtender>
                            </td>
                            <td>
                                <asp:UpdatePanel ID="UpdatePanel_go" runat="server">
                                    <ContentTemplate>
                                        <asp:Button ID="Button1" runat="server" Text="Go" Style="margin-left: -524px;" Font-Bold="True"
                                            Font-Names="Book Antiqua" Font-Size="Medium" OnClick="Button1_Click" />
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>
                        </tr>
                    </table>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
        <br />
        <asp:Label ID="lbl_error" runat="server" Text="" Font-Bold="True" Font-Names="Book Antiqua"
            Font-Size="Medium"></asp:Label>
        <br />
        <asp:UpdatePanel ID="UpdatePanel5" runat="server">
            <ContentTemplate>
                <div>
                    <asp:Label ID="lblnote" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                        Font-Size="Medium" ForeColor="Green" Text="Note : Attendance Percentage is Calculated from Semester Starting Date to Previous Date of Exam Date "></asp:Label>
                    <br />
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
                            HeaderStyle-BackColor="#0CA6CA" Font-Names="Book Antiqua" ShowHeaderWhenEmpty="true"
                            OnRowDataBound="Showgrid_OnRowDataBound">
                        </asp:GridView>
                    </div>
                    <br />
                    <asp:Label ID="lblexcelerror" runat="server" ForeColor="Red" Text="" Font-Bold="True"
                        Font-Names="Book Antiqua" Font-Size="Medium"></asp:Label>
                    <asp:Label ID="lblxl" runat="server" Text="Report Name" Font-Bold="True" Font-Names="Book Antiqua"
                        Font-Size="Medium"></asp:Label>
                    <asp:TextBox ID="txtxl" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                        Font-Size="Medium" onkeypress="display()" Style=""></asp:TextBox>
                    <asp:Button ID="btnxl" runat="server" Style="" Text="Export Excel" Font-Bold="True"
                        Font-Names="Book Antiqua" Font-Size="Medium" OnClick="btnxl_Click" />
                    <asp:Button ID="btnprintmaster" runat="server" Text="Print" Style="" OnClick="btnprintmaster_Click"
                        Font-Names="Book Antiqua" Font-Size="Medium" Font-Bold="true" />
                    <NEW:NEWPrintMater runat="server" ID="Printcontrol" Visible="false" />
                    <asp:Button ID="btnPrint" runat="server" Text="Direct Print" OnClientClick="return PrintPanel();"
                        Font-Names="Book Antiqua" Font-Size="Medium" Font-Bold="true" Height="35px" CssClass="textbox textbox1" />
                    <br />
                </div>
            </ContentTemplate>
            <Triggers>
                <asp:PostBackTrigger ControlID="btnxl" />
                <asp:PostBackTrigger ControlID="btnprintmaster" />
                <asp:PostBackTrigger ControlID="Button1" />
            </Triggers>
        </asp:UpdatePanel>
    </center>
    <%--progressBar for go--%>
    <%-- <center>
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
    </center>--%>
</asp:Content>

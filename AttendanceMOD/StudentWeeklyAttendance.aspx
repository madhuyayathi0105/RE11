<%@ Page Title="" Language="C#" MasterPageFile="~/AttendanceMOD/AttendanceSubSiteMaster.master"
    AutoEventWireup="true" CodeFile="StudentWeeklyAttendance.aspx.cs" Inherits="StudentWeeklyAttendance" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Src="~/Usercontrols/NewPrintMaster.ascx" TagName="NEWPrintMater" TagPrefix="NEW" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <link href="~/Styles/css/Commoncss.css" rel="Stylesheet" type="text/css" />
    <script type="text/javascript">
        function display() {
            document.getElementById('MainContent_Label1').innerHTML = "";
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
            width: 80px;
        }
        .style2
        {
            width: 120px;
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
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">
    <body>
        <asp:ScriptManager ID="ScriptManager1" runat="server">
        </asp:ScriptManager>
        <br />
        <center>
            <span class="fontstyleheader" style="color: Green;">AT19-Student Weekly Report</span>
        </center>
        <br />
        <asp:UpdatePanel ID="UpdatePanel5" runat="server">
            <ContentTemplate>
                <div>
                    <center>
                        <table class="maintablestyle">
                            <tr>
                                <td class="style1">
                                    <asp:Label ID="lblbach" runat="server" Text="Batch" Font-Bold="True" ForeColor="Black"
                                        Font-Size="Medium" Font-Names="Book Antiqua"></asp:Label>
                                </td>
                                <td class="style1">
                                    <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                                        <ContentTemplate>
                                            <asp:TextBox ID="txtbatch" runat="server" Height="20px" CssClass="Dropdown_Txt_Box"
                                                ReadOnly="true" Width="120px" Style="font-family: 'Book Antiqua';" Font-Bold="True"
                                                Font-Names="Book Antiqua" Font-Size="Medium">---Select---</asp:TextBox>
                                            <asp:Panel ID="pbatch" runat="server" CssClass="multxtpanel" Width="125px" Height="200px"
                                                Style="overflow-x: hidden; overflow-y: hidden;">
                                                <asp:CheckBox ID="chkbatch" runat="server" Width="100px" Font-Bold="True" OnCheckedChanged="chkbatch_ChekedChange"
                                                    Font-Names="Book Antiqua" Font-Size="Medium" Text="Select All" AutoPostBack="True" />
                                                <asp:CheckBoxList ID="chklsbatch" runat="server" Font-Size="Medium" AutoPostBack="True"
                                                    Width="100px" Height="58px" Font-Bold="True" Font-Names="Book Antiqua" OnSelectedIndexChanged="chklstbatch_SelectedIndexChanged">
                                                </asp:CheckBoxList>
                                            </asp:Panel>
                                            <asp:PopupControlExtender ID="pceSelections" runat="server" TargetControlID="txtbatch"
                                                PopupControlID="pbatch" Position="Bottom">
                                            </asp:PopupControlExtender>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                                <td class="style1">
                                    <asp:Label ID="lbldegree" runat="server" Text="Degree" Font-Bold="True" ForeColor="Black"
                                        Font-Names="Book Antiqua" Font-Size="Medium"></asp:Label>
                                </td>
                                <td class="style1">
                                    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                                        <ContentTemplate>
                                            <asp:TextBox ID="txtdegree" runat="server" Height="20px" ReadOnly="true" CssClass="Dropdown_Txt_Box"
                                                Width="120px" Style="font-family: 'Book Antiqua'" Font-Bold="True" Font-Names="Book Antiqua"
                                                Font-Size="Medium">---Select---</asp:TextBox>
                                            <asp:Panel ID="pdegree" runat="server" CssClass="multxtpanel" Width="125px" Height="250px">
                                                <asp:CheckBox ID="chkdegree" runat="server" Width="100px" Font-Bold="True" Font-Names="Book Antiqua"
                                                    Font-Size="Medium" Text="Select All" AutoPostBack="True" OnCheckedChanged="chkdegree_CheckedChanged" />
                                                <asp:CheckBoxList ID="chklstdegree" runat="server" Font-Size="Medium" AutoPostBack="True"
                                                    Width="100px" Height="58px" Font-Bold="True" Font-Names="Book Antiqua" OnSelectedIndexChanged="chklstdegree_SelectedIndexChanged">
                                                </asp:CheckBoxList>
                                            </asp:Panel>
                                            <asp:PopupControlExtender ID="PopupControlExtender1" runat="server" TargetControlID="txtdegree"
                                                PopupControlID="pdegree" Position="Bottom">
                                            </asp:PopupControlExtender>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                                <td class="style1">
                                    <asp:Label ID="lblbranch" runat="server" Text="Branch" Font-Bold="True" ForeColor="Black"
                                        Font-Names="Book Antiqua" Font-Size="Medium"></asp:Label>
                                </td>
                                <td class="style2">
                                    <asp:UpdatePanel ID="UpdatePanel3" runat="server">
                                        <ContentTemplate>
                                            <asp:TextBox ID="txtbranch" runat="server" Height="20px" CssClass="Dropdown_Txt_Box"
                                                ReadOnly="true" Width="120px" Style="font-family: 'Book Antiqua';" Font-Bold="True"
                                                Font-Names="Book Antiqua" Font-Size="Medium">---Select---</asp:TextBox>
                                            <asp:Panel ID="pbranch" runat="server" CssClass="multxtpanel" Width="250px" Height="250px">
                                                <asp:CheckBox ID="chkbranch" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                                    Font-Size="Medium" Text="Select All" AutoPostBack="True" OnCheckedChanged="chkbranch_CheckedChanged" />
                                                <asp:CheckBoxList ID="chklstbranch" runat="server" Font-Size="Medium" AutoPostBack="True"
                                                    Width="350px" Style="font-family: 'Book Antiqua'" Font-Bold="True" Font-Names="Book Antiqua"
                                                    Height="58px" OnSelectedIndexChanged="chklstbranch_SelectedIndexChanged">
                                                </asp:CheckBoxList>
                                            </asp:Panel>
                                            <asp:PopupControlExtender ID="PopupControlExtender2" runat="server" TargetControlID="txtbranch"
                                                PopupControlID="pbranch" Position="Bottom">
                                            </asp:PopupControlExtender>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                                <td>
                                    <asp:Label ID="lblsec" runat="server" Text="Section" Font-Bold="True" ForeColor="Black"
                                        Font-Names="Book Antiqua" Font-Size="Medium"></asp:Label>
                                </td>
                                <td class="style1">
                                    <asp:UpdatePanel ID="UpdatePanel4" runat="server">
                                        <ContentTemplate>
                                            <asp:TextBox ID="txtsec" runat="server" Height="20px" CssClass="Dropdown_Txt_Box"
                                                ReadOnly="true" Width="120px" Style="font-family: 'Book Antiqua';" Font-Bold="True"
                                                Font-Names="Book Antiqua" Font-Size="Medium">---Select---</asp:TextBox>
                                            <asp:Panel ID="psec" runat="server" CssClass="multxtpanel" Width="125px" Height="150px">
                                                <asp:CheckBox ID="chksec" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                                    Font-Size="Medium" Text="Select All" AutoPostBack="True" OnCheckedChanged="chksec_CheckedChanged" />
                                                <asp:CheckBoxList ID="chklssec" runat="server" Font-Size="Medium" AutoPostBack="True"
                                                    Style="font-family: 'Book Antiqua'" Font-Bold="True" Font-Names="Book Antiqua"
                                                    Height="58px" OnSelectedIndexChanged="chklstsec_SelectedIndexChanged">
                                                </asp:CheckBoxList>
                                            </asp:Panel>
                                            <asp:PopupControlExtender ID="PopupControlExtender3" runat="server" TargetControlID="txtsec"
                                                PopupControlID="psec" Position="Bottom">
                                            </asp:PopupControlExtender>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                            </tr>
                            <tr>
                                <td class="style1">
                                    <asp:Label ID="lblfrom" runat="server" Text="From Date" Font-Bold="true" Font-Names="Book Antiqua"
                                        Font-Size="Medium"></asp:Label>
                                </td>
                                <td class="style1">
                                    <asp:TextBox ID="txtfrom" runat="server" Font-Bold="true" Width="100px" Font-Names="Book Antiqua"
                                        Font-Size="Medium" AutoPostBack="true" OnTextChanged="txtfrom_TextChanged"></asp:TextBox>
                                    <asp:CalendarExtender ID="CalendarExtender2" TargetControlID="txtfrom" runat="server"
                                        Format="dd/MM/yyyy">
                                    </asp:CalendarExtender>
                                </td>
                                <td class="style1">
                                    <asp:Label ID="lblto" runat="server" Text="To Date" Font-Bold="true" Font-Names="Book Antiqua"
                                        Font-Size="Medium"></asp:Label>
                                </td>
                                <td class="style1">
                                    <asp:TextBox ID="txtto" AutoPostBack="true" runat="server" Font-Bold="true" Width="100px"
                                        Font-Names="Book Antiqua" Font-Size="Medium" OnTextChanged="txtto_TextChanged"></asp:TextBox>
                                    <asp:CalendarExtender ID="CalendarExtender1" TargetControlID="txtto" runat="server"
                                        Format="dd/MM/yyyy">
                                    </asp:CalendarExtender>
                                </td>
                                <td class="style1">
                                    <asp:CheckBox ID="chkdate" Width="100px" runat="server" Font-Bold="true" Font-Names="Book Antiqua"
                                        Font-Size="Medium" Text="Day/Date" Style="" />
                                </td>
                                <td class="style1">
                                    <asp:UpdatePanel ID="UpdatePanel_go" runat="server">
                                        <ContentTemplate>
                                            <asp:Button ID="btngo" runat="server" Text="Go" Font-Bold="true" Font-Names="Book Antiqua"
                                                Font-Size="Medium" Style="" OnClick="btngo_Click" />
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                            </tr>
                        </table>
                        <br />
                        <asp:Label ID="lblnorec" runat="server" Text="No Records Found" ForeColor="Red" Font-Bold="True"
                            Font-Names="Book Antiqua" Font-Size="Small"></asp:Label>
                        <asp:Label ID="errmsg" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                            Font-Size="Medium" ForeColor="Red" Width="676px"></asp:Label>
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
                        <asp:Label ID="lblexcelname" runat="server" Text="Report Name" Font-Bold="true" Font-Names="Book Antiqua"
                            Font-Size="Medium"></asp:Label>
                        <asp:TextBox ID="txtexcelname" runat="server" Font-Bold="true" Font-Names="Book Antiqua"
                            Font-Size="Medium"></asp:TextBox>
                        <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender2" runat="server" TargetControlID="txtexcelname"
                            FilterType="LowercaseLetters,UppercaseLetters,Numbers,Custom" ValidChars="!@$%^&*()_+|\}{][':;?><,./">
                        </asp:FilteredTextBoxExtender>
                        <asp:Button ID="btnxl" runat="server" Text="Export Excel" Font-Bold="True" Font-Names="Book Antiqua"
                            Font-Size="Medium" OnClick="btnxl_Click" />
                        <asp:Button ID="btnprintmaster" runat="server" Text="Print" OnClick="btnprintmaster_Click"
                            Font-Names="Book Antiqua" Font-Size="Medium" Font-Bold="true" />
                        <NEW:NEWPrintMater runat="server" ID="Printcontrol" Visible="false" />
                        <asp:Button ID="btnPrint" runat="server" Text="Direct Print" OnClientClick="return PrintPanel();"
                            Font-Names="Book Antiqua" Font-Size="Medium" Font-Bold="true" Height="35px" CssClass="textbox textbox1" />
                    </center>
                </div>
            </ContentTemplate>
            <Triggers>
                <asp:PostBackTrigger ControlID="btnxl" />
                <asp:PostBackTrigger ControlID="btnprintmaster" />
                <asp:PostBackTrigger ControlID="btnPrint" />
                <asp:PostBackTrigger ControlID="btngo" />
            </Triggers>
        </asp:UpdatePanel>
        <%--progressBar for Upbook_go--%>
        <%--  <center>
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
    </body>
</asp:Content>

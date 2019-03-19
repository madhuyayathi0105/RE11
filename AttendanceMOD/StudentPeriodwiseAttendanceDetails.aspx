<%@ Page Title="" Language="C#" MasterPageFile="~/AttendanceMOD/AttendanceSubSiteMaster.master"
    AutoEventWireup="true" CodeFile="StudentPeriodwiseAttendanceDetails.aspx.cs"
    Inherits="Student_Absenties_Report" EnableEventValidation="false" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Src="~/Usercontrols/NewPrintMaster.ascx" TagName="NEWPrintMater" TagPrefix="NEW" %>
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
        <style type="text/css">
            .style1
            {
                width: 80px;
            }
            .style2
            {
                width: 120px;
            }
        </style>
        <asp:UpdatePanel ID="UpdatePanel5" runat="server">
            <ContentTemplate>
        <asp:ScriptManager ID="ScriptManager1" runat="server">
        </asp:ScriptManager>
        <br />
        <center>
            <span class="fontstyleheader" style="color: Green;">Student Periodwise Absentees Report</span>
            <br />
            <br />
            <table class="maintablestyle" style="text-align: left;">
                <tr>
                    <td class="style1">
                        <asp:Label ID="lblbach" runat="server" Text="Batch" Font-Bold="True" ForeColor="Black"
                            Font-Size="Medium" Font-Names="Book Antiqua"></asp:Label>
                    </td>
                    <td class="style2">
                        <div style="position: relative;">
                            <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                                <ContentTemplate>
                                    <asp:TextBox ID="txtbatch" runat="server" Height="20px" CssClass="Dropdown_Txt_Box"
                                        ReadOnly="true" Width="120px" Style="font-family: 'Book Antiqua';" Font-Bold="True"
                                        Font-Names="Book Antiqua" Font-Size="Medium">---Select---</asp:TextBox>
                                    <asp:Panel ID="pbatch" runat="server" CssClass="multxtpanel" Width="125px">
                                        <asp:CheckBox ID="chkbatch" runat="server" Width="100px" Font-Bold="True" OnCheckedChanged="chkbatch_ChekedChange"
                                            Font-Names="Book Antiqua" Font-Size="Medium" Text="Select All" AutoPostBack="True" />
                                        <asp:CheckBoxList ID="chklsbatch" runat="server" Font-Size="Medium" AutoPostBack="True"
                                            Width="100px" Height="58px" Font-Bold="True" Font-Names="Book Antiqua" OnSelectedIndexChanged="chklstbatch_SelectedIndexChanged">
                                        </asp:CheckBoxList>
                                    </asp:Panel>
                                    <asp:PopupControlExtender ID="PopupControlExtender1" runat="server" TargetControlID="txtbatch"
                                        PopupControlID="pbatch" Position="Bottom">
                                    </asp:PopupControlExtender>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </div>
                    </td>
                    <td class="style1">
                        <asp:Label ID="lbldegree" runat="server" Text="Degree" Font-Bold="True" ForeColor="Black"
                            Font-Names="Book Antiqua" Font-Size="Medium"></asp:Label>
                    </td>
                    <td class="style2">
                        <div style="position: relative;">
                            <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                                <ContentTemplate>
                                    <asp:TextBox ID="txtdegree" runat="server" Height="20px" ReadOnly="true" CssClass="Dropdown_Txt_Box"
                                        Width="120px" Style="font-family: 'Book Antiqua'" Font-Bold="True" Font-Names="Book Antiqua"
                                        Font-Size="Medium">---Select---</asp:TextBox>
                                    <asp:Panel ID="pdegree" runat="server" CssClass="multxtpanel" Width="125px">
                                        <asp:CheckBox ID="chkdegree" runat="server" Width="100px" Font-Bold="True" Font-Names="Book Antiqua"
                                            Font-Size="Medium" Text="Select All" AutoPostBack="True" OnCheckedChanged="chkdegree_CheckedChanged" />
                                        <asp:CheckBoxList ID="chklstdegree" runat="server" Font-Size="Medium" AutoPostBack="True"
                                            Width="100px" Height="58px" Font-Bold="True" Font-Names="Book Antiqua" OnSelectedIndexChanged="chklstdegree_SelectedIndexChanged">
                                        </asp:CheckBoxList>
                                    </asp:Panel>
                                    <asp:PopupControlExtender ID="PopupControlExtender2" runat="server" TargetControlID="txtdegree"
                                        PopupControlID="pdegree" Position="Bottom">
                                    </asp:PopupControlExtender>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </div>
                    </td>
                    <td class="style1">
                        <asp:Label ID="lblbranch" runat="server" Text="Department" Font-Bold="True" ForeColor="Black"
                            Font-Names="Book Antiqua" Font-Size="Medium"></asp:Label>
                    </td>
                    <td class="style2">
                        <div style="position: relative;">
                            <asp:UpdatePanel ID="UpdatePanel3" runat="server">
                                <ContentTemplate>
                                    <asp:TextBox ID="txtbranch" runat="server" Height="20px" CssClass="Dropdown_Txt_Box"
                                        ReadOnly="true" Width="120px" Style="font-family: 'Book Antiqua';" Font-Bold="True"
                                        Font-Names="Book Antiqua" Font-Size="Medium">---Select---</asp:TextBox>
                                    <asp:Panel ID="pbranch" runat="server" CssClass="multxtpanel" Width="250px">
                                        <asp:CheckBox ID="chkbranch" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                            Font-Size="Medium" Text="Select All" AutoPostBack="True" OnCheckedChanged="chkbranch_CheckedChanged" />
                                        <asp:CheckBoxList ID="chklstbranch" runat="server" Font-Size="Medium" AutoPostBack="True"
                                            Width="350px" Style="font-family: 'Book Antiqua'" Font-Bold="True" Font-Names="Book Antiqua"
                                            Height="58px" OnSelectedIndexChanged="chklstbranch_SelectedIndexChanged">
                                        </asp:CheckBoxList>
                                    </asp:Panel>
                                    <asp:PopupControlExtender ID="PopupControlExtender3" runat="server" TargetControlID="txtbranch"
                                        PopupControlID="pbranch" Position="Bottom">
                                    </asp:PopupControlExtender>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </div>
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
                                <asp:Panel ID="psec" runat="server" CssClass="multxtpanel" Width="125px">
                                    <asp:CheckBox ID="chksec" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                        Font-Size="Medium" Text="Select All" AutoPostBack="True" OnCheckedChanged="chksec_CheckedChanged" />
                                    <asp:CheckBoxList ID="chklssec" runat="server" Font-Size="Medium" AutoPostBack="True"
                                        Style="font-family: 'Book Antiqua'" Font-Bold="True" Font-Names="Book Antiqua"
                                        Height="58px" OnSelectedIndexChanged="chklstsec_SelectedIndexChanged">
                                    </asp:CheckBoxList>
                                </asp:Panel>
                                <asp:PopupControlExtender ID="PopupControlExtender4" runat="server" TargetControlID="txtsec"
                                    PopupControlID="psec" Position="Bottom">
                                </asp:PopupControlExtender>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </td>
                </tr>
                <tr>
                    <td colspan="8">
                        <asp:Label ID="lbl_rtype" runat="server" Text="Report" Width="80px" Font-Bold="True"
                            ForeColor="Black" Font-Names="Book Antiqua" Font-Size="Medium"></asp:Label>
                        <asp:DropDownList runat="server" ID="ddl_rtype" Font-Bold="True" Height="25px" Width="225px"
                            Font-Names="Book Antiqua" Font-Size="Medium" AutoPostBack="true" OnSelectedIndexChanged="ddl_rtype_OnSelectedIndexChanged">
                            <asp:ListItem Value="0">Day Wise Absentees Report</asp:ListItem>
                            <asp:ListItem Value="1">Month Wise Absentees Report</asp:ListItem>
                        </asp:DropDownList>
                        <asp:Label ID="lblfrom" runat="server" Text="" Font-Bold="true" Font-Names="Book Antiqua"
                            Font-Size="Medium"></asp:Label>
                        <asp:TextBox ID="txtfrom" runat="server" Width="80px" Font-Bold="true" Font-Names="Book Antiqua"
                            Font-Size="Medium" AutoPostBack="true"></asp:TextBox>
                        <asp:CalendarExtender ID="CalendarExtender1" TargetControlID="txtfrom" runat="server"
                            Format="dd/MM/yyyy">
                        </asp:CalendarExtender>
                        <asp:Label ID="lblto" runat="server" Text="To Date" Font-Bold="true" Font-Names="Book Antiqua"
                            Font-Size="Medium" Style="position: relative"></asp:Label>
                        <asp:TextBox ID="txtto" runat="server" Width="80px" Font-Bold="true" Font-Names="Book Antiqua"
                            Font-Size="Medium" AutoPostBack="true" Style="position: relative"></asp:TextBox>
                        <asp:CalendarExtender ID="CalendarExtender2" TargetControlID="txtto" runat="server"
                            Format="dd/MM/yyyy">
                        </asp:CalendarExtender>

                        <asp:UpdatePanel ID="btngoUpdatePanel" runat="server">
                                <ContentTemplate>
                        <asp:Button ID="btngo" runat="server" Text="Go" Font-Bold="true" Font-Names="Book Antiqua"
                            Font-Size="Medium" OnClick="btngo_Click" Style="position: relative" />

                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </td>
                </tr>
            </table>
            <br />
            <asp:Label ID="lblnorec" runat="server" Visible="false" Text="" ForeColor="Red" Font-Bold="True"
                Font-Names="Book Antiqua" Font-Size="Medium"></asp:Label>
            <asp:Label ID="errmsg" runat="server" Visible="false" Font-Bold="True" Font-Names="Book Antiqua"
                Font-Size="Medium" ForeColor="Red"></asp:Label>
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
                            <asp:GridView ID="grdover" runat="server" Width="500px" BorderStyle="Double" Font-Bold="true"
                            Font-Names="Book Antiqua" Font-Size="Medium" GridLines="Both" CellPadding="4"
                            ShowFooter="false" ShowHeader="true">
                            <Columns>
                            </Columns>
                            <FooterStyle BackColor="White" ForeColor="#333333" />
                            <HeaderStyle BackColor="#0CA6CA" Font-Bold="True" ForeColor="Control" />
                            <PagerStyle BackColor="#336666"  HorizontalAlign="Center" />
                            <RowStyle BackColor="White" ForeColor="#333333" />
                            <SelectedRowStyle BackColor="#339966" Font-Bold="True"  />
                            <SortedAscendingCellStyle BackColor="#F7F7F7" />
                            <SortedAscendingHeaderStyle BackColor="#487575" />
                            <SortedDescendingCellStyle BackColor="#E5E5E5" />
                            <SortedDescendingHeaderStyle BackColor="#275353" />
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
            <div id="exceldiv" runat="server" visible="false">
                <asp:Label ID="lblexcelname" runat="server" Text="Report Name" Font-Bold="true" Font-Names="Book Antiqua"
                    Font-Size="Medium"></asp:Label>
                <asp:TextBox ID="txtexcelname" runat="server" Font-Bold="true" Font-Names="Book Antiqua"
                    Font-Size="Medium"></asp:TextBox>
                <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender2" runat="server" TargetControlID="txtexcelname"
                    FilterType="LowercaseLetters,UppercaseLetters,Numbers,Custom" ValidChars="!@$%^&*()_+|\}{][':;?><,./">
                </asp:FilteredTextBoxExtender>
                <asp:Button ID="btnxl" runat="server" Text="Export Excel" Font-Bold="True" Font-Names="Book Antiqua"
                    Font-Size="Medium" OnClick="btnxl_Click" Style="background-color: LightGrey;
                    border-radius: 7px;" />
                <asp:Button ID="btnprintmaster" runat="server" Text="Print" Style="background-color: LightGrey;
                    border-radius: 7px;" OnClick="btnprintmaster_Click" Font-Names="Book Antiqua"
                    Font-Size="Medium" Font-Bold="true" />
                 <NEW:NEWPrintMater runat="server" ID="Printcontrol" Visible="false" />

                <button id="btnPrint" runat="server"  height="29px" width="62px" onclick="return printTTOutput();"
            style=" font-weight: bold; font-size: medium; font-family: Book Antiqua;">
            Direct Print
        </button>
            </div>
            <br />
        </center>

        </ContentTemplate>
        <Triggers>
            <asp:PostBackTrigger ControlID="btnxl" />
        </Triggers>
    </asp:UpdatePanel>
     <center>
        <asp:UpdateProgress ID="UpdateProgress1" runat="server" AssociatedUpdatePanelID="btngoUpdatePanel">
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

    </body>
    </html>
</asp:Content>

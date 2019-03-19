<%@ Page Title="" Language="C#" MasterPageFile="~/AttendanceMOD/AttendanceSubSiteMaster.master"
    AutoEventWireup="true" CodeFile="AbsenteeRt.aspx.cs" Inherits="ReportClassLog" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Src="~/Usercontrols/NewPrintMaster.ascx" TagName="NEWPrintMater" TagPrefix="NEW" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">
    <!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN">
    <html>
    <style type="text/css">
        .dropdo
        {
        }
        #form1
        {
            height: 1778px;
        }
        
        .style41
        {
        }
        .style42
        {
            width: 317px;
        }
        .style44
        {
            width: 613px;
        }
        .style46
        {
            width: 111px;
        }
        .style52
        {
            width: 541px;
        }
        .style53
        {
            width: 88px;
        }
        .txt
        {
        }
        .style54
        {
            top: 198px;
        }
        .style55
        {
        }
        .style56
        {
            width: 106px;
        }
        
        .MultipleSelectionDDL
        {
            border: solid 1px #000000;
            height: 80px;
            width: 200px;
            overflow-y: scroll;
            background-color: #f0f8ff;
            font-size: 11px;
            font-family: Calibri, Arial, Helvetica;
            line-height: normal;
        }
    </style>
    <script type="text/javascript">
        function display() {
            document.getElementById('MainContent_norecordlbl').innerHTML = "";
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
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <body oncontextmenu="return false">
        <div>
            <asp:UpdatePanel ID="UpdatePanel4" runat="server">
                <ContentTemplate>
                    <br />
                    <center>
                        <span class="fontstyleheader" style="color: Green;">AT01-Hourwise/Daywise Absentees
                            Report</span>
                    </center>
                    <br />
                    <center>
                        <div class="maintablestyle" style="width: 800px;">
                            <table>
                                <tr>
                                    <td>
                                        <asp:Label runat="server" ID="lblbatch" Text="Batch" Font-Bold="True" Font-Names="Book Antiqua"
                                            Font-Size="Medium"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="ddlbatch" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                            Font-Size="Medium" OnSelectedIndexChanged="ddlbatch_SelectedIndexChanged" Height="25px"
                                            Width="60px" AutoPostBack="True">
                                        </asp:DropDownList>
                                    </td>
                                    <td>
                                        <asp:Label runat="server" ID="lbldegree" Text="Degree" Font-Bold="True" Font-Names="Book Antiqua"
                                            Font-Size="Medium"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:DropDownList runat="server" ID="ddldegree" Height="25px" Width="85px" AutoPostBack="True"
                                            OnSelectedIndexChanged="ddldegree_SelectedIndexChanged" Font-Bold="True" Font-Names="Book Antiqua"
                                            Font-Size="Medium">
                                        </asp:DropDownList>
                                    </td>
                                    <td>
                                        <asp:Label runat="server" ID="lblbranch" Text="Branch" Font-Bold="True" Font-Names="Book Antiqua"
                                            Font-Size="Medium"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:DropDownList runat="server" ID="ddlbranch" Font-Bold="True" Height="25px" Width="200px"
                                            Font-Names="Book Antiqua" Font-Size="Medium" OnSelectedIndexChanged="ddlbranch_SelectedIndexChanged"
                                            AutoPostBack="True">
                                        </asp:DropDownList>
                                    </td>
                                    <td>
                                        <asp:Label runat="server" ID="lblduration" Text="Sem" Font-Bold="True" Font-Names="Book Antiqua"
                                            Font-Size="Medium"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:DropDownList runat="server" ID="ddlduration" Font-Bold="True" Font-Names="Book Antiqua"
                                            Font-Size="Medium" AutoPostBack="True" Height="25px" Width="48px" OnSelectedIndexChanged="ddlduration_SelectedIndexChanged">
                                        </asp:DropDownList>
                                    </td>
                                    <td>
                                        <asp:Label runat="server" ID="lblsec" Text="Sec" Font-Bold="True" Font-Names="Book Antiqua"
                                            Font-Size="Medium"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:DropDownList runat="server" ID="ddlsec" Font-Bold="True" Font-Names="Book Antiqua"
                                            Font-Size="Medium" Height="25px" Width="47px" AutoPostBack="True" OnSelectedIndexChanged="ddlsec_SelectedIndexChanged">
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                            </table>
                            <table>
                                <tr>
                                    <td>
                                        <asp:Label ID="lblFromdate" runat="server" Text="From Date" Font-Bold="True" Font-Names="Book Antiqua"
                                            Width="80px" Font-Size="Medium"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtFromDate" CssClass="txt" runat="server" Height="26px" Width="74px"
                                            OnTextChanged="txtFromDate_TextChanged" Font-Bold="True" Font-Names="Book Antiqua"
                                            Font-Size="Medium" AutoPostBack="True"></asp:TextBox>
                                        <asp:FilteredTextBoxExtender ID="txtFromDate_FilteredTextBoxExtender" FilterType="Custom,Numbers"
                                            ValidChars="/" runat="server" TargetControlID="txtFromDate">
                                        </asp:FilteredTextBoxExtender>
                                        <asp:CalendarExtender ID="calfromdate" TargetControlID="txtFromDate" Format="d/MM/yyyy"
                                            runat="server">
                                        </asp:CalendarExtender>
                                    </td>
                                    <td>
                                        <asp:Label ID="lbltodate" runat="server" Text="To Date" Font-Bold="True" Font-Names="Book Antiqua"
                                            Width="80px" Font-Size="Medium"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtToDate" CssClass="txt" runat="server" Height="26px" Width="74px"
                                            Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium" OnTextChanged="txtToDate_TextChanged"
                                            AutoPostBack="True"></asp:TextBox>
                                        <asp:FilteredTextBoxExtender ID="txtToDate_FilteredTextBoxExtender" runat="server"
                                            TargetControlID="txtToDate" FilterType="Custom,Numbers" ValidChars="/">
                                        </asp:FilteredTextBoxExtender>
                                        <asp:CalendarExtender ID="caltodate" TargetControlID="txtToDate" Format="d/MM/yyyy"
                                            runat="server">
                                        </asp:CalendarExtender>
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="ddlformat" runat="server" Font-Names="Book Antiqua" AutoPostBack="true"
                                            Font-Size="Medium" Font-Bold="true" OnSelectedIndexChanged="onselected_SelectedIndexChanged">
                                            <asp:ListItem>Absentees</asp:ListItem>
                                            <asp:ListItem>General</asp:ListItem>
                                            <asp:ListItem>Hourwise Absentees</asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                    <td>
                                        <asp:CheckBoxList ID="optradio" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                            Font-Size="Medium">
                                            <asp:ListItem Value="day">Daywise List</asp:ListItem>
                                            <asp:ListItem Value="hour">Hourwise List</asp:ListItem>
                                        </asp:CheckBoxList>
                                        <div id="Multiple customers selected">
                                            <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                                                <ContentTemplate>
                                                    <asp:Label ID="Label2" runat="server" Text="Criteria" Font-Bold="True" Font-Names="Book Antiqua"
                                                        Font-Size="Medium">        </asp:Label>
                                                    <asp:TextBox ID="TextBox1" runat="server" Height="20px" ReadOnly="true" Width="120px"
                                                        Style="font-family: 'Book Antiqua'" Font-Bold="True" Font-Names="Book Antiqua"
                                                        Font-Size="Medium">---Select---</asp:TextBox>
                                                    <asp:Panel ID="pnlCustomers" runat="server" BackColor="White" BorderColor="Black"
                                                        BorderStyle="Solid" BorderWidth="2px" Height="250px" ScrollBars="Vertical" Width="250px">
                                                        <asp:CheckBox ID="SelectAll" runat="server" OnCheckedChanged="SelectAll_CheckedChanged"
                                                            Width="100px" Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium" Text="Select All"
                                                            AutoPostBack="True" />
                                                        <asp:CheckBoxList ID="ddlreport" runat="server" Font-Size="Medium" AutoPostBack="True"
                                                            Width="100px" Height="200px" Font-Bold="True" Font-Names="Book Antiqua" OnSelectedIndexChanged="ddlreport_SelectedIndexChanged">
                                                            <asp:ListItem >P</asp:ListItem>
                                                            <asp:ListItem >A</asp:ListItem>
                                                            <asp:ListItem >OD</asp:ListItem>
                                                            <asp:ListItem >ML</asp:ListItem>
                                                            <asp:ListItem >SOD</asp:ListItem>
                                                            <asp:ListItem >NSS</asp:ListItem>
                                                            <asp:ListItem >H</asp:ListItem>
                                                            <asp:ListItem >NJ</asp:ListItem>
                                                            <asp:ListItem >S</asp:ListItem>
                                                            <asp:ListItem >L</asp:ListItem>
                                                            <asp:ListItem >NCC</asp:ListItem>
                                                            <asp:ListItem >HS</asp:ListItem>
                                                            <asp:ListItem>PP</asp:ListItem>
                                                            <asp:ListItem >SYOD</asp:ListItem>
                                                            <asp:ListItem >COD</asp:ListItem>
                                                            <asp:ListItem >OOD</asp:ListItem>
                                                            <asp:ListItem >LA</asp:ListItem>
                                                        </asp:CheckBoxList>
                                                    </asp:Panel>
                                                    <asp:PopupControlExtender ID="pceSelections" runat="server" TargetControlID="TextBox1"
                                                        PopupControlID="pnlCustomers" Position="Bottom">
                                                    </asp:PopupControlExtender>
                                                </ContentTemplate>
                                            </asp:UpdatePanel>
                                        </div>
                                    </td>
                                    <td>
                                        <asp:Label ID="lblsubject" runat="server" Text="Subject" Font-Bold="True" Font-Names="Book Antiqua"
                                            Font-Size="Medium" Style="top: 152px; left: 10px; height: 21px; width: 31px"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:UpdatePanel ID="UpdatePanel_Department" runat="server">
                                            <ContentTemplate>
                                                <asp:TextBox ID="txt_subject" runat="server" ReadOnly="true" Font-Bold="True" Font-Names="Book Antiqua"
                                                    Font-Size="Medium" CssClass="Dropdown_Txt_Box">--Select--</asp:TextBox>
                                                <asp:Panel ID="panel_Department" runat="server" Height="300px" CssClass="MultipleSelectionDDL">
                                                    <asp:CheckBox ID="chktesr" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                                        Font-Size="Medium" Text="Select All" AutoPostBack="True" OnCheckedChanged="chksubject_checkedchanged" />
                                                    <asp:CheckBoxList ID="ddlsubject" runat="server" Font-Size="Medium" AutoPostBack="True"
                                                        Font-Bold="True" Font-Names="Book Antiqua" OnSelectedIndexChanged="ddlsubject_selectedchanged">
                                                    </asp:CheckBoxList>
                                                </asp:Panel>
                                                <asp:PopupControlExtender ID="PopupControlExtender2" runat="server" TargetControlID="txt_subject"
                                                    PopupControlID="panel_Department" Position="Bottom">
                                                </asp:PopupControlExtender>
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                    </td>
                                    <td>
                                        <asp:UpdatePanel ID="UpdatePanel_go" runat="server">
                                            <ContentTemplate>
                                                <asp:Button ID="btnGo" runat="server" Text="Go" OnClick="btnGo_Click" Width="41px"
                                                    Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium"></asp:Button>
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                    </td>
                                    <td>
                                        <asp:Label ID="lblpage" runat="server" Text="Page" Font-Names="Book Antiqua" Font-Size="Medium"
                                            Font-Bold="true"></asp:Label>
                                        <asp:DropDownList ID="ddlpage" runat="server" OnSelectedIndexChanged="ddlpage_SelectedIndexChanged"
                                            Font-Names="Book Antiqua" Font-Size="Medium" Font-Bold="true" AutoPostBack="True"
                                            Width="48px" Style="margin-bottom: 0px" Visible="false">
                                        </asp:DropDownList>
                                    </td>
                                    <td>
                                    </td>
                                    <td>
                                        <asp:Button ID="btnPrint" runat="server" Font-Bold="True" Text="Print Master Setting"
                                            Visible="False" Font-Names="Book Antiqua" Font-Size="Medium" CssClass="style41"
                                            OnClick="btnPrint_Click" Width="160px" />
                                    </td>
                                    <td>
                                    </td>
                                </tr>
                            </table>
                            <table>
                                <tr>
                                    <td>
                                        <asp:CheckBox ID="cbldisholy" runat="server" Text="Display Holiday" Font-Bold="true"
                                            Font-Names="Book Antiqua" Font-Size="Medium" />
                                        <asp:CheckBox ID="cbdispne" runat="server" Font-Bold="true" Font-Names="Book Antiqua"
                                            Font-Size="Medium" Text="Display NE(Unmarked Hours)" />
                                    </td>
                                    <td>
                                        <asp:RadioButtonList ID="rdiobtndetailornot" runat="server" RepeatDirection="Horizontal"
                                            Font-Bold="true" Font-Names="Book Antiqua" Font-Size="Medium" Width="150px">
                                            <asp:ListItem>Count</asp:ListItem>
                                            <asp:ListItem>Detail</asp:ListItem>
                                        </asp:RadioButtonList>
                                    </td>
                                    <td>
                                    </td>
                                    <td>
                                    </td>
                                    <td>
                                    </td>
                                    <td>
                                    </td>
                                </tr>
                            </table>
                        </div>
                    </center>
                    <br />
                    <center>
                        <asp:Label ID="norecordlbl" runat="server" Font-Bold="true" Font-Size="Medium" ForeColor="Red"></asp:Label>
                        <asp:Label ID="tofromlbl" runat="server" ForeColor="Red" Font-Bold="True" Font-Names="Book Antiqua"
                            Font-Size="Medium"></asp:Label>
                        <asp:Label ID="LabelE" runat="server" Visible="False" ForeColor="Red" Font-Bold="True"
                            Font-Names="Book Antiqua" Font-Size="Medium"></asp:Label>
                    </center>
                    <br />
                    <center>
                        <asp:Panel ID="pageset_pnl" runat="server" BorderStyle="None" Width="817px">
                            <asp:Label ID="Buttontotal" runat="server" Font-Bold="True" Visible="False" Font-Names="Book Antiqua"
                                Font-Size="Medium"></asp:Label>
                            <asp:Label ID="lblrecord" runat="server" Visible="False" Font-Bold="True" Text="     Records Per Page"
                                Font-Names="Book Antiqua" Font-Size="Medium"></asp:Label>
                            &nbsp;<asp:DropDownList ID="DropDownListpage" runat="server" AutoPostBack="True"
                                Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium" Height="24px" OnSelectedIndexChanged="DropDownListpage_SelectedIndexChanged"
                                Visible="False" Width="58px">
                            </asp:DropDownList>
                            &nbsp;<asp:TextBox ID="TextBoxother" runat="server" AutoPostBack="True" Font-Bold="True"
                                Font-Names="Book Antiqua" Font-Size="Medium" Height="16px" OnTextChanged="TextBoxother_TextChanged"
                                Visible="false" Width="34px"></asp:TextBox>
                            <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender3" runat="server" FilterType="Numbers"
                                TargetControlID="TextBoxother">
                            </asp:FilteredTextBoxExtender>
                            &nbsp;<asp:Label ID="lblpage_search" runat="server" Font-Bold="True" Text="Page Search"
                                Visible="False" Width="88px" Font-Names="Book Antiqua" Font-Size="Medium" Height="25px"></asp:Label>&nbsp;<asp:TextBox
                                    ID="TextBoxpage" runat="server" Visible="False" AutoPostBack="True" OnTextChanged="TextBoxpage_TextChanged"
                                    Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium" Height="17px" Width="34px"></asp:TextBox>
                            <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender9" runat="server" TargetControlID="TextBoxpage"
                                FilterType="Numbers" />
                            &nbsp;
                        </asp:Panel>
                    </center>
                    <center>
                        <asp:Label ID="lblnote" runat="server" Text="Note: AB-Absent  ,  NE-Not Entered  ,  HS-Hour Suspension  ,  SP-Special Hour"
                            Font-Names="Book Antiqua" ForeColor="Brown" Font-Bold="true" Visible="false"
                            Font-Size="Medium"></asp:Label>
                    </center>
                    <center>
                        <div id="divMainContents" runat="server" style="display: table; margin: 0px; height: auto;
                            margin-bottom: 20px; margin-top: 10px; position: relative; width: auto; text-align: left;">
                            <asp:GridView ID="GridView1" runat="server" Visible="false" HeaderStyle-ForeColor="Black"
                                HeaderStyle-BackColor="#0CA6CA" Font-Names="Book Antiqua" ShowHeaderWhenEmpty="true"
                                OnRowDataBound="gridview1_DataBound" BackColor="#F0F8FF">
                            </asp:GridView>
                            <asp:GridView ID="GridView2" runat="server" Visible="false" HeaderStyle-ForeColor="Black"
                                HeaderStyle-BackColor="#0CA6CA" Font-Names="Book Antiqua" ShowHeaderWhenEmpty="true"
                                OnRowDataBound="gridview2_DataBound" BackColor="#F0F8FF">
                            </asp:GridView>
                            <asp:GridView ID="GridView3" runat="server" Visible="false" HeaderStyle-ForeColor="Black"
                                HeaderStyle-BackColor="#0CA6CA" Font-Names="Book Antiqua" ShowHeaderWhenEmpty="true"
                                OnDataBound="gridview3_DataBound" OnRowDataBound="gridview3_DataBound" BackColor="#F0F8FF">
                            </asp:GridView>
                        </div>
                    </center>
                    <br />
                    <center>
                        <asp:Button ID="Button1" runat="server" Text="Print" OnClick="Button1_Click" Visible="False" />
                        <asp:Label ID="lblrptname" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                            Font-Size="Medium" Text="Report Name"></asp:Label>
                        <asp:TextBox ID="txtexcelname" runat="server" Height="20px" Width="180px" Style="font-family: 'Book Antiqua'"
                            Font-Bold="True" onkeypress="display()" Font-Names="Book Antiqua" Font-Size="Medium"></asp:TextBox>
                        <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" runat="server" TargetControlID="txtexcelname"
                            FilterType="LowercaseLetters,UppercaseLetters,Numbers,Custom" ValidChars="!@$%^&*()_+|\}{][':;?><,./">
                        </asp:FilteredTextBoxExtender>
                        <asp:Button ID="btnxl" runat="server" Text="Export Excel" Font-Bold="True" Font-Names="Book Antiqua"
                            Font-Size="Medium" OnClick="btnxl_Click" />
                        <asp:Button ID="btnprintmaster" runat="server" Text="Print" OnClick="btnprintmaster_Click"
                            Font-Names="Book Antiqua" Font-Size="Medium" Font-Bold="true" />
                        <NEW:NEWPrintMater runat="server" ID="Printcontrol" Visible="false" />
                        <asp:Button ID="btnditprint" runat="server" Text="Direct Print" OnClientClick="return PrintPanel();"
                            Font-Names="Book Antiqua" Font-Size="Medium" Visible="false" Font-Bold="true"
                            Height="35px" CssClass="textbox textbox1" />
                    </center>
                </ContentTemplate>
                <Triggers>
                    <asp:PostBackTrigger ControlID="btnxl" />
                    <asp:PostBackTrigger ControlID="btnprintmaster" />
                    <asp:PostBackTrigger ControlID="btnditprint" />
                </Triggers>
            </asp:UpdatePanel>
        </div>
        <%--progressBar for Go--%>
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
    </body>
    </html>
</asp:Content>

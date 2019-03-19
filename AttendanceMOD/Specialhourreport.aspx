<%@ Page Title="" Language="C#" MasterPageFile="~/AttendanceMOD/AttendanceSubSiteMaster.master"
    AutoEventWireup="true" CodeFile="Specialhourreport.aspx.cs" Inherits="Specialhourreport" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Src="~/Usercontrols/NewPrintMaster.ascx" TagName="NEWPrintMater" TagPrefix="NEW" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
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
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <center>
        <span class="fontstyleheader" style="color: Green;">AT18-Special Hour Report</span>
    </center>
    <asp:UpdatePanel ID="UpdatePanel3" runat="server">
        <ContentTemplate>
            <div>
                <center>
                    <table class="maintablestyle" style="width: 900px; top: 390px; text-align: left">
                        <tr>
                            <td class="style38">
                                <asp:Label ID="Label1" runat="server" Text="Batch " Font-Bold="True" Font-Names="Book Antiqua"
                                    Font-Size="Medium"></asp:Label>
                            </td>
                            <td class="style1">
                                <asp:DropDownList ID="ddlbatch" runat="server" OnSelectedIndexChanged="ddlbatch_SelectedIndexChanged"
                                    AutoPostBack="True" Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium"
                                    Height="25px" Width="80px">
                                </asp:DropDownList>
                            </td>
                            <td class="style41">
                                <asp:Label ID="Label2" runat="server" Text="Degree" Font-Bold="True" Font-Names="Book Antiqua"
                                    Font-Size="Medium"></asp:Label>
                            </td>
                            <td class="style21">
                                <asp:DropDownList ID="ddldegree" runat="server" OnSelectedIndexChanged="ddldegree_SelectedIndexChanged"
                                    AutoPostBack="True" Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium">
                                </asp:DropDownList>
                            </td>
                            <td class="style42">
                                <asp:Label ID="lblbranch" runat="server" Text="Branch" Font-Bold="True" Font-Names="Book Antiqua"
                                    Font-Size="Medium"></asp:Label>
                            </td>
                            <td class="style4">
                                <asp:DropDownList ID="ddlbranch" runat="server" OnSelectedIndexChanged="ddlbranch_SelectedIndexChanged"
                                    AutoPostBack="True" Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium"
                                    Height="25px" Width="150px">
                                </asp:DropDownList>
                            </td>
                            <td class="style6">
                                <asp:Label ID="lblsem" runat="server" Text="Semester" Font-Bold="True" Font-Names="Book Antiqua"
                                    Font-Size="Medium"></asp:Label>
                            </td>
                            <td class="style20">
                                <asp:UpdatePanel ID="UpdatePanel_sem" runat="server">
                                    <ContentTemplate>
                                        <asp:DropDownList ID="ddlsemester" runat="server" OnSelectedIndexChanged="ddlsemester_SelectedIndexChanged"
                                            AutoPostBack="True" Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium"
                                            Height="25px" Width="41px">
                                        </asp:DropDownList>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>
                            <td class="style8">
                                <asp:Label ID="lblsec" runat="server" Text="Sec" Font-Bold="True" Font-Names="Book Antiqua"
                                    Font-Size="Medium"></asp:Label>
                            </td>
                            <td class="style25">
                                <asp:DropDownList ID="ddlsection" runat="server" AutoPostBack="True" Font-Bold="True"
                                    Font-Names="Book Antiqua" Font-Size="Medium" Height="25px" Width="52px" OnSelectedIndexChanged="ddlsection_SelectedIndexChanged">
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="lblsubject" runat="server" Text="Subject" Font-Bold="True" Font-Names="Book Antiqua"
                                    Font-Size="Medium" Style="height: 15px; width: 46px;">
                                </asp:Label>
                            </td>
                            <td class="style1">
                                <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                                    <ContentTemplate>
                                        <asp:TextBox ID="txtsubject" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                            Font-Size="Medium" Text="--Select--" OnTextChanged="txtsubject_TextChanged" CssClass="Dropdown_Txt_Box"
                                            Style="height: 17px; width: 110px;"></asp:TextBox>
                                        <asp:Panel ID="pnlSubject" runat="server" CssClass="multxtpanel">
                                            <asp:CheckBox ID="chksubject" runat="server" AutoPostBack="True" Font-Bold="True"
                                                Font-Names="Book Antiqua" Font-Size="Medium" OnCheckedChanged="chksubject_CheckedChanged"
                                                Text="Select All" />
                                            <asp:CheckBoxList ID="ddlsubject" runat="server" OnSelectedIndexChanged="ddlsubject_SelectedIndexChanged"
                                                AutoPostBack="true" Font-Names="Book Antiqua" Font-Size="Medium">
                                            </asp:CheckBoxList>
                                        </asp:Panel>
                                        <asp:PopupControlExtender ID="pceSelections" runat="server" TargetControlID="txtsubject"
                                            PopupControlID="pnlSubject" Position="Bottom">
                                        </asp:PopupControlExtender>
                                        </td>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>
                            <td class="style4">
                                <asp:Label ID="lbldate" runat="server" Width="90px" Font-Bold="True" Font-Names="Book Antiqua"
                                    Font-Size="Medium" Text="From Date"></asp:Label>
                            </td>
                            <td class="style41">
                                <asp:TextBox ID="txtfromdate" runat="server" AutoPostBack="true" Width="80px"></asp:TextBox>
                                <asp:CalendarExtender ID="CalendarExtender1" TargetControlID="txtfromdate" runat="server"
                                    Format="dd/MM/yyyy" />
                            </td>
                            <td>
                                <asp:Label ID="lbltodat" runat="server" Width="75px" Font-Bold="True" Font-Names="Book Antiqua"
                                    Font-Size="Medium" Text="To Date"></asp:Label>
                            </td>
                            <td class="style42">
                                <asp:TextBox ID="txttodate" runat="server" AutoPostBack="true" Width="80px" OnTextChanged="txttodate_TextChanged"></asp:TextBox>
                                <asp:CalendarExtender ID="CalendarExtender2" TargetControlID="txttodate" runat="server"
                                    Format="dd/MM/yyyy" />
                            </td>
                            <td>
                                <asp:RadioButton ID="rbdetail" Text="Detailed" runat="server" Width="85px" Font-Bold="True"
                                    Font-Names="Book Antiqua" Font-Size="Medium" GroupName="sphour" />
                            </td>
                            <td>
                                <asp:RadioButton ID="rbcount" Text="Count" runat="server" Width="80px" Font-Bold="True"
                                    Font-Names="Book Antiqua" Font-Size="Medium" GroupName="sphour" />
                            </td>
                            <td class="style15">
                                <asp:UpdatePanel ID="UpdatePanel_go" runat="server">
                                    <ContentTemplate>
                                        <asp:Button ID="btngo" runat="server" Text="Go" OnClick="btngo_Click" Font-Bold="True"
                                            Font-Names="Book Antiqua" Font-Size="Medium" Width="70px" />
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>
                        </tr>
                    </table>
                    <br />
                    <asp:Label ID="norecordlbl" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                        Font-Size="Medium" ForeColor="Red" Width="676px" Text=""></asp:Label>
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
                        <asp:GridView ID="Showgrid" Style="height: auto; width: 800px;" runat="server" Visible="false"
                            HeaderStyle-ForeColor="Black" HeaderStyle-BackColor="#0CA6CA" AutoGenerateColumns="true"
                            ShowHeaderWhenEmpty="true" OnRowDataBound="Showgrid_OnRowDataBound">
                        </asp:GridView>
                    </div>
                    <br />
                    <asp:Label ID="lblrptname" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                        Font-Size="Medium" Text="Report Name"></asp:Label>
                    <asp:TextBox ID="txtexcelname" runat="server" Height="20px" Width="180px" Style="font-family: 'Book Antiqua'"
                        Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium" onkeypress="display()"></asp:TextBox>
                    <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender2" runat="server" TargetControlID="txtexcelname"
                        FilterType="LowercaseLetters,UppercaseLetters,Numbers,Custom" ValidChars="!@$%^&*()_+|}{][':;?><,."
                        InvalidChars="/\">
                    </asp:FilteredTextBoxExtender>
                    <asp:Button ID="btnexcel" runat="server" OnClick="btnexcel_Click" Text="Export Excel"
                        Font-Bold="true" Font-Size="Medium" Font-Names="Book Antiqua" />
                    <asp:Button ID="btnprintmaster" runat="server" Text="Print" OnClick="btnprintmaster_Click"
                        Font-Names="Book Antiqua" Font-Size="Medium" Font-Bold="true" />
                    <NEW:NEWPrintMater runat="server" ID="Printcontrol" Visible="false" />
                    <asp:Button ID="btnPrint" runat="server" Text="Direct Print" OnClientClick="return PrintPanel();"
                        Font-Names="Book Antiqua" Font-Size="Medium" Visible="false" Font-Bold="true"
                        Height="35px" CssClass="textbox textbox1" />
                </center>
            </div>
        </ContentTemplate>
        <Triggers>
            <asp:PostBackTrigger ControlID="btnexcel" />
            <asp:PostBackTrigger ControlID="btnprintmaster" />
        </Triggers>
    </asp:UpdatePanel>
    <%--progressBar for Upbook_go--%>
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
    <center>
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
    </center>
</asp:Content>

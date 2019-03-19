<%@ Page Title="" Language="C#" MasterPageFile="~/AttendanceMOD/AttendanceSubSiteMaster.master"
    AutoEventWireup="true" CodeFile="notes.aspx.cs" Inherits="notes" EnableEventValidation="false" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Src="~/Usercontrols/NewPrintMaster.ascx" TagName="NEWPrintMater" TagPrefix="NEW" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <style type="text/css">
        .style1
        {
            width: 96px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">
    <asp:ScriptManager ID="scriptvel" runat="server">
    </asp:ScriptManager>
    <body>
        <script type="text/javascript">
            function display() {
                document.getElementById('<%=lblnorec.ClientID %>').innerHTML = "";
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
        <br />
        <center>
            <span class="fontstyleheader" style="color: Green;">Subject Notes</span>
        </center>
        <br />
        <center>
            <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                <ContentTemplate>
                    <div style="width: 1072px">
                        <table class="maintablestyle" style="width: 1038px;">
                            <tr>
                                <td>
                                    <asp:Label ID="lblcollege" runat="server" Text="College" Font-Bold="True" Font-Names="Book Antiqua"
                                        Font-Size="Medium"></asp:Label>
                                </td>
                                <td>
                                    <asp:DropDownList ID="ddlcollege" runat="server" OnSelectedIndexChanged="ddlcollege_SelectedIndexChanged"
                                        AutoPostBack="True" Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium"
                                        Height="25px" Width="176px">
                                    </asp:DropDownList>
                                </td>
                                <td>
                                    <asp:Label ID="Label1" runat="server" Text="Batch" Font-Bold="True" Font-Names="Book Antiqua"
                                        Font-Size="Medium"></asp:Label>
                                </td>
                                <td>
                                    <asp:DropDownList ID="ddlbatch" runat="server" OnSelectedIndexChanged="ddlbatch_SelectedIndexChanged"
                                        AutoPostBack="True" Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium"
                                        Height="25px" Width="69px">
                                    </asp:DropDownList>
                                </td>
                                <td>
                                    <asp:Label ID="Label2" runat="server" Text="Degree" Font-Bold="True" Font-Names="Book Antiqua"
                                        Font-Size="Medium"></asp:Label>
                                </td>
                                <td>
                                    <asp:DropDownList ID="ddldegree" runat="server" OnSelectedIndexChanged="ddldegree_SelectedIndexChanged"
                                        AutoPostBack="True" Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium"
                                        Height="25px">
                                    </asp:DropDownList>
                                </td>
                                <td>
                                    <asp:Label ID="lblbranch" runat="server" Text="Branch" Font-Bold="True" Font-Names="Book Antiqua"
                                        Font-Size="Medium"></asp:Label>
                                </td>
                                <td>
                                    <asp:DropDownList ID="ddlbranch" runat="server" OnSelectedIndexChanged="ddlbranch_SelectedIndexChanged"
                                        AutoPostBack="True" Font-Bold="True" Font-Names="Book Antiqua" Width="212px"
                                        Font-Size="Medium">
                                    </asp:DropDownList>
                                </td>
                                <td>
                                    <asp:Label ID="lblsem" runat="server" Text="Sem" Font-Bold="True" Font-Names="Book Antiqua"
                                        Font-Size="Medium"></asp:Label>
                                </td>
                                <td>
                                    <asp:UpdatePanel ID="UpdatePanel_sem" runat="server">
                                        <ContentTemplate>
                                            <asp:DropDownList ID="ddlsemester" runat="server" OnSelectedIndexChanged="ddlsemester_SelectedIndexChanged"
                                                AutoPostBack="True" Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium"
                                                Height="25px" Width="41px">
                                            </asp:DropDownList>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                                <td>
                                    <asp:Label ID="lblsec" runat="server" Text="Sec" Font-Bold="True" Font-Names="Book Antiqua"
                                        Font-Size="Medium"></asp:Label>
                                </td>
                                <td>
                                    <asp:DropDownList ID="ddlsection" runat="server" AutoPostBack="True" Font-Bold="True"
                                        Font-Names="Book Antiqua" Font-Size="Medium" Height="25px" Width="52px" OnSelectedIndexChanged="ddlsection_SelectedIndexChanged">
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="lblsubject" runat="server" Text="Subject" Width="100px" Font-Bold="True"
                                        ForeColor="Black" Font-Names="Book Antiqua" Font-Size="Medium"></asp:Label>
                                </td>
                                <td style="margin: 0 0 0 150px;" class="style1">
                                    <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                                        <ContentTemplate>
                                            <asp:TextBox ID="txtsubject" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                                Font-Size="Medium" CssClass="Dropdown_Txt_Box" Style="font-family: 'Book Antiqua';
                                                height: 20px; width: 125px;">---Select---</asp:TextBox>
                                            <asp:Panel ID="psubject" runat="server" CssClass="multxtpanel" BackColor="White"
                                                BorderColor="Black" BorderStyle="Solid" ScrollBars="Auto" Style="font-family: 'Book Antiqua'">
                                                <asp:CheckBox ID="chksubject" runat="server" Font-Bold="True" OnCheckedChanged="chksubject_CheckedChanged"
                                                    Font-Names="Book Antiqua" Font-Size="Medium" Text="Select All" AutoPostBack="True" />
                                                <asp:CheckBoxList ID="chklstsubject" runat="server" Font-Size="Medium" AutoPostBack="True"
                                                    Font-Bold="True" Font-Names="Book Antiqua" OnSelectedIndexChanged="chklstsubject_SelectedIndexChanged">
                                                </asp:CheckBoxList>
                                            </asp:Panel>
                                            <asp:PopupControlExtender ID="pceSelections" runat="server" TargetControlID="txtsubject"
                                                PopupControlID="psubject" Position="Bottom">
                                            </asp:PopupControlExtender>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                                <td colspan="2">
                                    <asp:CheckBox ID="chkdate" runat="server" Text="DateWise" Font-Bold="True" AutoPostBack="true"
                                        OnCheckedChanged="chkdate_checkedchanged" Font-Names="Book Antiqua" Font-Size="Medium"
                                        Checked="true" />
                                </td>
                                <td colspan="5">
                                    &nbsp;<asp:Label ID="lblFromDate" runat="server" Text="From Date" Font-Bold="True"
                                        Font-Names="Book Antiqua" Font-Size="Medium">
                                    </asp:Label>
                                    <asp:TextBox ID="txtFromDate" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                        Font-Size="Medium" Width="75px"></asp:TextBox>
                                    <asp:CalendarExtender ID="CalendarExtender1" TargetControlID="txtFromDate" Format="d/MM/yyyy"
                                        runat="server">
                                    </asp:CalendarExtender>
                                    <asp:Label ID="lblToDate" runat="server" Text="To Date" Font-Bold="True" Font-Names="Book Antiqua"
                                        Font-Size="Medium">
                                    </asp:Label>
                                    <asp:TextBox ID="txtToDate" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                        Font-Size="Medium" Width="80px"></asp:TextBox>
                                    <asp:CalendarExtender ID="CalendarExtender2" TargetControlID="txtToDate" Format="d/MM/yyyy"
                                        runat="server">
                                    </asp:CalendarExtender>
                                </td>
                                <td>
                                    <asp:UpdatePanel ID="UpdatePanel_go" runat="server">
                                        <ContentTemplate>
                                            <asp:Button ID="btngo" runat="server" Text="Go" OnClick="btngo_Click" Font-Bold="True"
                                                Font-Names="Book Antiqua" Font-Size="Medium" />
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                            </tr>
                        </table>
                        <center>
                            <br />
                            <asp:Label ID="errmsg" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                Font-Size="Medium" ForeColor="Red"></asp:Label>
                            <br />
                            <div id="divMainContents" runat="server" style="display: table; margin: 0px; height: auto;
                                margin-bottom: 20px; margin-top: 10px; position: relative; width: auto; text-align: left;">
                                <asp:GridView ID="Showgrid" runat="server" Visible="false" HeaderStyle-ForeColor="Black"
                                    HeaderStyle-BackColor="#0CA6CA" AutoGenerateColumns="false" ShowHeaderWhenEmpty="true">
                                    <Columns>
                                        <asp:TemplateField HeaderText="S.No" HeaderStyle-BackColor="#0CA6CA" HeaderStyle-HorizontalAlign="center"
                                            HeaderStyle-Width="30px">
                                            <ItemTemplate>
                                                <center>
                                                    <asp:Label ID="lbl_sno" runat="server" Style="width: auto;" Text='<%#Eval("Sno") %>'></asp:Label>
                                                </center>
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Center" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Date" HeaderStyle-BackColor="#0CA6CA" HeaderStyle-HorizontalAlign="center"
                                            HeaderStyle-Width="100px">
                                            <ItemTemplate>
                                                <center>
                                                    <asp:Label ID="lbl_Date" runat="server" Style="width: auto;" Text='<%#Eval("date") %>'></asp:Label>
                                                </center>
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Center" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="View" HeaderStyle-BackColor="#0CA6CA" HeaderStyle-HorizontalAlign="center"
                                            HeaderStyle-Width="100px">
                                            <ItemTemplate>
                                                <center>
                                                    <asp:LinkButton ID="link_View" runat="server" Style="width: auto;" Visible="true"
                                                        Text='<%#Eval("view") %>' OnClick="link_View_Click"></asp:LinkButton>
                                                    <asp:Label ID="lbl_View" runat="server" Style="width: auto;" Text='<%#Eval("view") %>'
                                                        Visible="false"></asp:Label>
                                                    <asp:Label ID="lblfileid" runat="server" Text='<%#Eval("file") %>' Visible="false"></asp:Label>
                                                </center>
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Left" />
                                        </asp:TemplateField>
                                    </Columns>
                                </asp:GridView>
                            </div>
                            <br />
                            <asp:Label ID="lblnorec" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                Font-Size="Medium" ForeColor="#FF3300" Visible="False" CssClass="style50"></asp:Label>
                            <asp:Label ID="lblrptname" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                Font-Size="Medium" Text="Report Name"></asp:Label>
                            <asp:TextBox ID="txtexcelname" runat="server" onkeypress="display()" Height="20px"
                                Width="180px" Style="font-family: 'Book Antiqua'" Font-Bold="True" Font-Names="Book Antiqua"
                                Font-Size="Medium"></asp:TextBox>
                            <asp:Button ID="btnxl" runat="server" Text="Export Excel" Font-Bold="True" Font-Names="Book Antiqua"
                                Font-Size="Medium" OnClick="btnxl_Click" />
                            <asp:Button ID="btnprintmaster" runat="server" Text="Print" OnClick="btnprintmaster_Click"
                                Font-Names="Book Antiqua" Font-Size="Medium" Font-Bold="true" />
                            <NEW:NEWPrintMater runat="server" ID="Printcontrol" Visible="false" />
                            <asp:Button ID="btnPrint" runat="server" Text="Direct Print" OnClientClick="return PrintPanel();"
                                Font-Names="Book Antiqua" Font-Size="Medium" Font-Bold="true" Height="35px" CssClass="textbox textbox1" />
                            <br />
                        </center>
                    </div>
                </ContentTemplate>
                <Triggers>
                    <asp:PostBackTrigger ControlID="btnxl" />
                    <asp:PostBackTrigger ControlID="btnprintmaster" />
                    <asp:PostBackTrigger ControlID="Showgrid" />
                </Triggers>
            </asp:UpdatePanel>
        </center>
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
    </body>
</asp:Content>

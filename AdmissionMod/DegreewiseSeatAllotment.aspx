<%@ Page Title="" Language="C#" MasterPageFile="~/AdmissionMod/AttendanceSubSiteMaster.master"
    AutoEventWireup="true" CodeFile="DegreewiseSeatAllotment.aspx.cs" Inherits="DegreewiseSeatAllotment" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">
    <script type="text/javascript">
        function PrintGrid() {
            var panel = document.getElementById('printDiv');
            var college = document.getElementById("<%=ddlCollege.ClientID %>");
            college = college.options[college.selectedIndex].text;

            var batch = document.getElementById("<%=ddlbatch.ClientID %>").value;
            var edulevel = document.getElementById("<%=ddlEduLev.ClientID %>").value;



            var streamval = document.getElementById("<%=ddlStream.ClientID %>").value;
            var printWindow = window.open('', '', 'height=816,width=980');
            printWindow.document.write('<html><head>');
            printWindow.document.write('</head><body >');
            printWindow.document.write('<center><h2>');
            printWindow.document.write(college);
            printWindow.document.write('</h2>');
            // printWindow.document.write('<table style=\'font-size:14px; font-weight:bold;\' cellpadding=10><tr><td>Batch :</td><td>' + batch + '</td><td>Education Level :</td><td>' + edulevel + '</td><td>Course :</td><td></td><td>Stream :</td><td>' + streamval + '</td></tr></table>');

            printWindow.document.write(panel.innerHTML);
            printWindow.document.write('</center></body></html>');
            printWindow.document.close();
            setTimeout(function () {
                printWindow.print();
            }, 500);
            return false;
        }
    </script>
    <center>
        <div class="maindivstyle" style="width: 950px;">
            <center>
                <span class="fontstyleheader" style="color: Green;">Degree Wise Seat Allotment - Sheet
                    Matrix</span>
            </center>
            <asp:ScriptManager ID="scptMgrNew" runat="server">
            </asp:ScriptManager>
            <table class="maintablestyle">
                <tr>
                    <td>
                        <asp:Label ID="lblCollege" runat="server" Text="Institute"></asp:Label>
                    </td>
                    <td>
                        <asp:DropDownList ID="ddlCollege" runat="server" CssClass="textbox ddlheight5" AutoPostBack="true"
                            OnSelectedIndexChanged="ddlCollege_SelectedIndexChanged">
                        </asp:DropDownList>
                    </td>
                    <td>
                        <asp:Label ID="lblBatch" runat="server" Text="Batch"></asp:Label>
                    </td>
                    <td>
                        <asp:DropDownList ID="ddlbatch" runat="server" CssClass="textbox ddlheight" AutoPostBack="true"
                            OnSelectedIndexChanged="ddlbatch_SelectedIndexChanged">
                        </asp:DropDownList>
                    </td>
                    <td>
                        <asp:Label ID="lblEdulevel" runat="server" Text="Edu Level"></asp:Label>
                    </td>
                    <td>
                        <asp:DropDownList ID="ddlEduLev" runat="server" CssClass="textbox ddlheight" Width="50px"
                            AutoPostBack="true" OnSelectedIndexChanged="ddlEdulevel_SelectedIndexChanged">
                        </asp:DropDownList>
                    </td>
                    <td>
                        <asp:Label ID="lblCourse" runat="server" Text="Course"></asp:Label>
                    </td>
                    <td>
                        <asp:DropDownList ID="ddlcourse" runat="server" CssClass="textbox ddlheight" Visible="false"
                            AutoPostBack="true" OnSelectedIndexChanged="ddlcourse_OnSelectedIndexChanged">
                        </asp:DropDownList>
                        <asp:UpdatePanel ID="updSes" runat="server">
                            <contenttemplate>
                                <asp:TextBox ID="txtSession" runat="server" CssClass="textbox textbox1" ReadOnly="true"
                                    Width="80px"></asp:TextBox>
                                <asp:Panel ID="pnlSession" runat="server" CssClass="multxtpanel">
                                    <asp:CheckBox ID="cb_Session" runat="server" Text="Select All" AutoPostBack="True"
                                        OnCheckedChanged="cb_Session_OnCheckedChanged" />
                                    <asp:CheckBoxList ID="cbl_Session" runat="server" AutoPostBack="true" OnSelectedIndexChanged="cbl_Session_OnSelectedIndexChanged">
                                    </asp:CheckBoxList>
                                </asp:Panel>
                                <asp:PopupControlExtender ID="pceSession" runat="server" TargetControlID="txtSession"
                                    PopupControlID="pnlSession" Position="Bottom">
                                </asp:PopupControlExtender>
                            </contenttemplate>
                        </asp:UpdatePanel>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="lblStream" runat="server" Text="Stream"></asp:Label>
                    </td>
                    <td colspan="2">
                        <asp:DropDownList ID="ddlStream" runat="server" CssClass="textbox ddlheight2" AutoPostBack="true"
                            OnSelectedIndexChanged="ddlStream_OnSelectedIndexChanged">
                        </asp:DropDownList>
                        <asp:Button ID="btnBaseGo" runat="server" Text="Go" CssClass="textbox  btn2" Width="40px"
                            OnClick="btnBaseGo_OnClick" />
                        <asp:Button ID="btnDaySlotSave" runat="server" Text="Save" CssClass="textbox  btn2"
                            Width="60px" BackColor="#81C13F" ForeColor="White" OnClientClick="return OnSaveRankListCheck()"
                            Visible="false" OnClick="btnDaySlotSave_OnClick" />
                        <asp:Button ID="btnBasePrint" runat="server" Text="Print" CssClass="textbox  btn2"
                            Width="60px" BackColor="#EB7E8C" ForeColor="White" Visible="false" OnClientClick="return PrintGrid()" />
                    </td>
                </tr>
            </table>
            <br />
            <div id="printDiv">
                <asp:GridView ID="gridBranSeat" runat="server" AutoGenerateColumns="true" HeaderStyle-BackColor="#0CA6CA"
                    OnDataBound="gridBranSeat_DataBound" OnRowDataBound="gridBranSeat_OnRowDataBound"
                    Visible="false">
                    <%-- <Columns>
                    <asp:TemplateField HeaderText="S.No">
                        <ItemTemplate>
                            <asp:Label ID="lblSno" runat="server" Text='<%#Container.DisplayIndex+1 %>'></asp:Label>
                        </ItemTemplate>
                        <ItemStyle HorizontalAlign="Center" />
                    </asp:TemplateField>
                </Columns>--%>
                </asp:GridView>
            </div>
        </div>
    </center>
    <div id="imgdiv2" runat="server" visible="false" style="height: 300em; z-index: 1000;
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
                                <asp:Label ID="lbl_alert" runat="server" Text="" Style="color: Red;" Font-Bold="true"
                                    Font-Size="Medium"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <center>
                                    <asp:Button ID="btn_errorclose" CssClass=" textbox btn1 comm" Style="height: 28px;
                                        width: 65px;" OnClick="btn_errorclose_Click" Text="ok" runat="server" />
                                </center>
                            </td>
                        </tr>
                    </table>
                </center>
            </div>
        </center>
    </div>
</asp:Content>

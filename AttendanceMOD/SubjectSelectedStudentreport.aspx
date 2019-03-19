<%@ Page Title="" Language="C#" MasterPageFile="~/AttendanceMOD/AttendanceSubSiteMaster.master"
    AutoEventWireup="true" CodeFile="SubjectSelectedStudentreport.aspx.cs" Inherits="SubjectSelectedSTudentreport"
    EnableEventValidation="false" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Assembly="FarPoint.Web.Spread" Namespace="FarPoint.Web.Spread" TagPrefix="FarPoint" %>
<%@ Register Src="~/Usercontrols/PrintMaster.ascx" TagName="PRINTPDF" TagPrefix="Insproplus" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">
    <html>
    <body oncontextmenu="return false">
        <script type="text/javascript">
            function display() {
                document.getElementById('MainContent_errmsg').innerHTML = "";
            }
        </script>
        <asp:ScriptManager ID="ScriptManager1" runat="server">
        </asp:ScriptManager>
        <div>
            <center>
                <span class="fontstyleheader" style="color: Green;">Subject Selected By Student Report</span>
            </center>
            <br />
            <center>
                <table class="maintablestyle" style="border-bottom-style: solid; border-top-style: solid;
                    border-right-style: solid; border-left-style: solid; background-color: #0CA6CA;
                    border-width: 1px; text-align: left; text-align: left; width: auto; height: 69px;">
                    <tr>
                        <td>
                            <asp:Label ID="lblreport" runat="server" Text="Report" Font-Bold="True" Font-Names="Book Antiqua"
                                Font-Size="Medium"></asp:Label>
                        </td>
                        <td>
                            <asp:DropDownList ID="ddlreport" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                Font-Size="Medium" OnSelectedIndexChanged="ddlreport_SelectedIndexChanged" Height="25px"
                                AutoPostBack="True">
                                <asp:ListItem Text="Degree Wise"></asp:ListItem>
                                <asp:ListItem Text="Subject Wise"></asp:ListItem>
                                <asp:ListItem Text="Missing Student"></asp:ListItem>
                               <asp:ListItem Text="With staff"></asp:ListItem>
                            </asp:DropDownList>
                            <asp:CheckBox ID="chkelective" runat="server" Text="Elective Only" Font-Bold="True"
                                Font-Names="Book Antiqua" Font-Size="Medium" Width="118px" AutoPostBack="true"
                                OnCheckedChanged="chkelective_CheckedChanged" />
                        </td>
                        <td>
                            <asp:CheckBox ID="chkover" Text="Over All" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                Font-Size="Medium" Width="90px" AutoPostBack="true" OnCheckedChanged="chkover_CheckedChanged" />
                            <asp:Label runat="server" ID="lblbatch" Text="Batch" Font-Bold="True" Font-Names="Book Antiqua"
                                Font-Size="Medium"></asp:Label>
                        </td>
                        <td>
                            <asp:DropDownList ID="ddlbatch" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                Font-Size="Medium" OnSelectedIndexChanged="ddlbatch_SelectedIndexChanged" Height="25px"
                                AutoPostBack="True">
                            </asp:DropDownList>
                        </td>
                        <td>
                            <asp:Label runat="server" ID="lbldegree" Text="Degree" Font-Bold="True" Font-Names="Book Antiqua"
                                Font-Size="Medium"></asp:Label>
                        </td>
                        <td>
                            <asp:DropDownList runat="server" ID="ddldegree" Height="24px" AutoPostBack="True"
                                Font-Bold="True" Font-Names="Book Antiqua" Width="100px" Font-Size="Medium" OnSelectedIndexChanged="ddldegree_SelectedIndexChanged">
                            </asp:DropDownList>
                        </td>
                        <td>
                            <asp:Label runat="server" ID="lblbranch" Text="Branch" Font-Bold="True" Font-Names="Book Antiqua"
                                Font-Size="Medium"></asp:Label>
                        </td>
                        <td>
                            <asp:DropDownList runat="server" ID="ddlbranch" Font-Bold="True" Height="25px" Width="150px"
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
                                Font-Size="Medium" AutoPostBack="True" Height="25px" Width="40px" OnSelectedIndexChanged="ddlduration_SelectedIndexChanged">
                            </asp:DropDownList>
                        </td>
                        <td>
                            <asp:Label runat="server" ID="lblsec" Text="Sec" Font-Bold="True" Font-Names="Book Antiqua"
                                Font-Size="Medium"></asp:Label>
                        </td>
                        <td>
                            <asp:DropDownList runat="server" ID="ddlsec" Font-Bold="True" Font-Names="Book Antiqua"
                                Font-Size="Medium" AutoPostBack="True" Height="25px" Width="50px" OnSelectedIndexChanged="ddlsec_SelectedIndexChanged">
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="3">
                            <asp:RadioButton ID="rdb_subjectwise" Text="Subject" Font-Bold="True" Font-Names="Book Antiqua"
                                Font-Size="Medium" runat="server" GroupName="barath" AutoPostBack="true" OnCheckedChanged="rdb_subjectwise_selectedchange" />
                            <asp:RadioButton ID="rdb_subjecttypewise" Text="Subject Type" Font-Bold="True" Font-Names="Book Antiqua"
                                Font-Size="Medium" runat="server" AutoPostBack="true" GroupName="barath" OnCheckedChanged="rdb_subjecttypewise_selectedchange" />
                            <asp:Label ID="lblsubject" runat="server" Text="" Font-Bold="True" Font-Names="Book Antiqua"
                                Font-Size="Medium"></asp:Label><%--Subject--%>
                            <asp:DropDownList ID="ddlsubject" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                Font-Size="Medium" Width="180px" OnSelectedIndexChanged="ddlsubject_SelectedIndexChanged"
                                AutoPostBack="True">
                                <%--Style="top: 175px; left: 242px; position: absolute;"--%><%--28.02.17 barath alignment change--%>
                            </asp:DropDownList>
                            <asp:UpdatePanel ID="updsubject" runat="server" style="float: right;">
                                <ContentTemplate>
                                    <asp:TextBox ID="txtsubject" runat="server" CssClass="Dropdown_Txt_Box" ReadOnly="true"
                                        Width="154px" Style="margin-top: -20px;" Font-Bold="True" Font-Names="Book Antiqua"
                                        Font-Size="Medium">---Select---</asp:TextBox><%--top: 171px; left: 240px; position: absolute; --%>
                                    <asp:Panel ID="psubject" runat="server" Width="300px" CssClass="multxtpanel">
                                        <asp:CheckBox ID="chksubject" runat="server" Font-Bold="True" OnCheckedChanged="chksubject_ChekedChange"
                                            Font-Names="Book Antiqua" Font-Size="Medium" Text="Select All" AutoPostBack="True" />
                                        <asp:CheckBoxList ID="chklssubject" runat="server" Font-Size="Medium" AutoPostBack="True"
                                            Font-Bold="True" Font-Names="Book Antiqua" OnSelectedIndexChanged="chklssubject_SelectedIndexChanged">
                                        </asp:CheckBoxList>
                                    </asp:Panel>
                                    <asp:PopupControlExtender ID="PopupControlExtender1" runat="server" TargetControlID="txtsubject"
                                        PopupControlID="psubject" Position="Bottom">
                                    </asp:PopupControlExtender>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </td>
                        <%-- <td>
                        </td>--%>
                        <td>
                            <asp:Button ID="btnGo" runat="server" Text="Go" OnClick="btnGo_Click" Font-Bold="True"
                                Font-Names="Book Antiqua" Font-Size="Medium" />
                        </td>
                    </tr>
                </table>
                <br />
                <asp:Label ID="errmsg" runat="server" ForeColor="Red" Font-Bold="True" Font-Names="Book Antiqua"
                    Font-Size="Medium" Height="16px" Width="280px"></asp:Label>
                <br />
                <table>
                    <tr>
                        <td>
                            <asp:Button ID="btnav" runat="server" BackColor="LightPink" Width="20px" Enabled="false" />
                            <asp:Label ID="lblavailable" runat="server" Text="Available" Font-Bold="True" Font-Names="Book Antiqua"
                                Font-Size="Medium"></asp:Label>
                        </td>
                        <td>
                            <asp:Button ID="btnfil" runat="server" BackColor="Orange" Width="20px" Enabled="false" />
                            <asp:Label ID="lblfiled" runat="server" Text="Filled" Font-Bold="True" Font-Names="Book Antiqua"
                                Font-Size="Medium"></asp:Label>
                        </td>
                        <td>
                            <asp:Button ID="btnexc" runat="server" BackColor="LightSkyBlue" Width="20px" Enabled="false" />
                            <asp:Label ID="lblexceed" runat="server" Text="Exceed" Font-Bold="True" Font-Names="Book Antiqua"
                                Font-Size="Medium"></asp:Label>
                        </td>
                </table>
                <table>
                    <tr>
                        <%-- <td>
                            <asp:Label ID="lbldename" runat="server" Text="Department" Font-Bold="True" Font-Names="Book Antiqua"
                                Font-Size="Medium"></asp:Label>
                        </td>--%>
                        <td>
                            <asp:RadioButton ID="rbname" runat="server" GroupName="name" Font-Bold="True" Font-Names="Book Antiqua"
                                Font-Size="Medium" Text="Department Name" />
                        </td>
                        <td>
                            <asp:RadioButton ID="rbacr" runat="server" GroupName="name" Text="Acronym Name" Font-Bold="True"
                                Font-Names="Book Antiqua" Font-Size="Medium" />
                        </td>
                        <td>
                            <asp:CheckBox ID="chkattfile" runat="server" Text="Attendance Field" Font-Bold="True"
                                Font-Names="Book Antiqua" Font-Size="Medium" />
                        </td>
                    </tr>
                </table>
                <br />
                <br />
                <FarPoint:FpSpread ID="subject_spread" runat="server" BorderColor="Black" BorderStyle="Solid"
                    BorderWidth="0.5" Height="200" Width="400" OnCellClick="subject_spread_CellClick"
                    OnPreRender="subject_spread_Prerender" ShowHeaderSelection="false">
                    <CommandBar BackColor="Control" ButtonFaceColor="Control" ButtonHighlightColor="ControlLightLight"
                        ButtonType="PushButton" ButtonShadowColor="ControlDark">
                    </CommandBar>
                    <Sheets>
                        <FarPoint:SheetView SheetName="Sheet1" GridLineColor="Black">
                        </FarPoint:SheetView>
                    </Sheets>
                </FarPoint:FpSpread>
                <br />
                <table>
                    <tr>
                        <td>
                            <asp:Label ID="lblexcl" runat="server" Text="Report Name" Font-Names="Book Antiqua"
                                Font-Size="Medium" Font-Bold="True"></asp:Label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtexcel" runat="server" Font-Names="Book Antiqua" onkeypress="display()"
                                Font-Size="Medium" Font-Bold="True"></asp:TextBox>
                            <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender4" runat="server" TargetControlID="txtexcel"
                                FilterType="LowercaseLetters,UppercaseLetters,Numbers,Custom" ValidChars="!@$%^&()_+}{][';,.">
                            </asp:FilteredTextBoxExtender>
                        </td>
                        <td>
                            <asp:Button ID="btnxl" runat="server" Text="Export Excel" Font-Bold="True" Font-Names="Book Antiqua"
                                Font-Size="Medium" OnClick="btnxl_Click" />
                        </td>
                        <td>
                            <asp:Button ID="btnprintmaster" runat="server" Text="Print" OnClick="btnprintmaster_Click"
                                Font-Names="Book Antiqua" Font-Size="Medium" Font-Bold="true" />
                            <Insproplus:PRINTPDF runat="server" ID="Printcontrol" Visible="false" />
                        </td>
                    </tr>
                </table>
                <br />
                <asp:Label ID="lblnorec" runat="server" ForeColor="Red" Font-Bold="True" Font-Names="Book Antiqua"
                    Font-Size="Medium" Height="16px" Width="280px"></asp:Label>
                <br />
                <center>
                    <FarPoint:FpSpread ID="Fpstucount" runat="server" BorderColor="Black" BorderStyle="Solid"
                        BorderWidth="0.5" Height="200" Width="400">
                        <CommandBar BackColor="Control" ButtonFaceColor="Control" ButtonHighlightColor="ControlLightLight"
                            ButtonType="PushButton" ButtonShadowColor="ControlDark">
                        </CommandBar>
                        <Sheets>
                            <FarPoint:SheetView SheetName="Sheet1" GridLineColor="Black">
                            </FarPoint:SheetView>
                        </Sheets>
                    </FarPoint:FpSpread>
                </center>
                <br />
                <table>
                    <tr>
                        <td>
                            <asp:Label ID="lblxl" runat="server" Text="Report Name" Font-Names="Book Antiqua"
                                Font-Size="Medium" Font-Bold="True"></asp:Label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtxl" runat="server" Font-Names="Book Antiqua" onkeypress="display()"
                                Font-Size="Medium" Font-Bold="True"></asp:TextBox>
                            <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" runat="server" TargetControlID="txtxl"
                                FilterType="LowercaseLetters,UppercaseLetters,Numbers,Custom" ValidChars="!@$%^&()_+}{][';,.">
                            </asp:FilteredTextBoxExtender>
                        </td>
                        <td>
                            <asp:Button ID="btnxl1" runat="server" Text="Export Excel" Font-Bold="True" Font-Names="Book Antiqua"
                                Font-Size="Medium" OnClick="btnxl1_Click" />
                        </td>
                        <td>
                            <asp:Button ID="btnprint1" runat="server" Text="Print" OnClick="btnprint1_Click"
                                Font-Names="Book Antiqua" Font-Size="Medium" Font-Bold="true" />
                            <Insproplus:PRINTPDF runat="server" ID="PRINTPDF" Visible="false" />
                        </td>
                    </tr>
                </table>
            </center>
        </div>
    </body>
    </html>
</asp:Content>

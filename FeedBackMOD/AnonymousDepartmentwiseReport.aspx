<%@ Page Title="" Language="C#" MasterPageFile="~/FeedBackMOD/FeedbackSubSiteMaster.master"
    AutoEventWireup="true" CodeFile="AnonymousDepartmentwiseReport.aspx.cs" Inherits="AnonymousDepartmentwiseReport" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="Ajax" %>
<%@ Register Assembly="FarPoint.Web.Spread" Namespace="FarPoint.Web.Spread" TagPrefix="FarPoint" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <script type="text/javascript">
        function display(x) {
            x.style.borderColor = "#c4c4c4";
        }
        function display1() {
            document.getElementById('<%=lbl_norec1.ClientID %>').innerHTML = "";
        }
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
        <center>
            <span class="fontstyleheader" style="color: Green;">Anonymous Department Staff Report</span>
        </center>
        <br />
        <fieldset style="width: 230px; height: 20px; background-color: #ffccff; margin-left: -87px;
            margin-top: 10px; border-radius: 10px; border-color: #6699ee; overflow: auto;">
            <table>
                <tr>
                    <td>
                        <asp:RadioButton ID="rdbanonyomous" runat="server" Visible="true" AutoPostBack="true"
                            Text="Anonymous" OnCheckedChanged="rdbanonyomous_Click" />
                    </td>
                    <td>
                        <asp:RadioButton ID="rdbloginbased" runat="server" Visible="true" AutoPostBack="true"
                            Text="Login Based" OnCheckedChanged="rdbloginbased_Click" />
                    </td>
                </tr>
            </table>
        </fieldset>
        <br />
        <table class="maintablestyle">
            <tr>
                <td>
                    College Name
                </td>
                <td colspan="3">
                    <asp:UpdatePanel ID="UpdatePanel4" runat="server">
                        <ContentTemplate>
                            <asp:TextBox ID="txtclgname" ReadOnly="true" runat="server" CssClass="textbox  txtheight5">--Select--</asp:TextBox>
                            <asp:Panel ID="Panel1" runat="server" CssClass="multxtpanel">
                                <asp:CheckBox ID="cb_clgname" runat="server" Text="Select All" AutoPostBack="True"
                                    OnCheckedChanged="cb_clgname_CheckedChanged" />
                                <asp:CheckBoxList ID="cbl_clgname" runat="server" AutoPostBack="True" OnSelectedIndexChanged="cbl_clgname_SelectedIndexChanged">
                                </asp:CheckBoxList>
                            </asp:Panel>
                            <asp:PopupControlExtender ID="PopupControlExtender2" runat="server" TargetControlID="txtclgname"
                                PopupControlID="Panel1" Position="Bottom">
                            </asp:PopupControlExtender>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </td>
                <td>
                    Batch Year
                </td>
                <td>
                    <asp:UpdatePanel ID="UpdatePanel12" runat="server">
                        <ContentTemplate>
                            <asp:TextBox ID="txt_batch" ReadOnly="true" Width=" 90px" runat="server" CssClass="textbox  txtheight2">--Select--</asp:TextBox>
                            <asp:Panel ID="Panel4" runat="server" CssClass="multxtpanel">
                                <asp:CheckBox ID="cb_batch" runat="server" Text="Select All" AutoPostBack="true"
                                    OnCheckedChanged="cb_batch_CheckedChanged" />
                                <asp:CheckBoxList ID="cbl_batch" runat="server" AutoPostBack="true" OnSelectedIndexChanged="cbl_batch_SelectedIndexChanged">
                                </asp:CheckBoxList>
                            </asp:Panel>
                            <asp:PopupControlExtender ID="PopupControlExtender7" runat="server" TargetControlID="txt_batch"
                                PopupControlID="Panel4" Position="Bottom">
                            </asp:PopupControlExtender>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </td>
                <td>
                    <asp:Label ID="lbl_Degree" Width="50px" runat="server" Text="Degree" Visible="false"></asp:Label>
                </td>
                <td>
                    <asp:UpdatePanel ID="Updp_Degree" runat="server" Visible="false">
                        <ContentTemplate>
                            <asp:TextBox ID="txt_degree" Width=" 90px" runat="server" CssClass="textbox  txtheight2"
                                ReadOnly="true">--Select--</asp:TextBox>
                            <asp:Panel ID="Panel_Degree" runat="server" Height="200" CssClass="multxtpanel">
                                <asp:CheckBox ID="cb_degree" runat="server" Text="Select All" AutoPostBack="True"
                                    OnCheckedChanged="cb_degree_CheckedChanged" />
                                <asp:CheckBoxList ID="cbl_degree" runat="server" AutoPostBack="True" OnSelectedIndexChanged="cbl_degree_SelectedIndexChanged">
                                </asp:CheckBoxList>
                            </asp:Panel>
                            <asp:PopupControlExtender ID="PopupControlExtender9" runat="server" TargetControlID="txt_degree"
                                PopupControlID="Panel_Degree" Position="Bottom">
                            </asp:PopupControlExtender>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </td>
                <td>
                    Department
                </td>
                <td>
                    <%--  <asp:DropDownList ID="ddlformate6_deptname" runat="server" OnSelectedIndexChanged="ddlformate6_deptname_selectedindex"
                    AutoPostBack="true" CssClass="textbox1 ddlheight4">
                </asp:DropDownList>--%>
                    <asp:UpdatePanel ID="UpdatePanel9" runat="server">
                        <ContentTemplate>
                            <asp:TextBox ID="txtdeptname" ReadOnly="true" runat="server" CssClass="textbox  txtheight4">--Select--</asp:TextBox>
                            <asp:Panel ID="Panel2" runat="server" CssClass="multxtpanel" Height="250px">
                                <asp:CheckBox ID="cb_deptname" runat="server" Text="Select All" AutoPostBack="True"
                                    OnCheckedChanged="cb_deptname_CheckedChanged" />
                                <asp:CheckBoxList ID="cbl_deptname" runat="server" AutoPostBack="True" OnSelectedIndexChanged="cbl_deptname_SelectedIndexChanged">
                                </asp:CheckBoxList>
                            </asp:Panel>
                            <asp:PopupControlExtender ID="PopupControlExtender3" runat="server" TargetControlID="txtdeptname"
                                PopupControlID="Panel2" Position="Bottom">
                            </asp:PopupControlExtender>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </td>
            </tr>
            <tr>
                <td>
                    Semester
                </td>
                <td>
                    <asp:UpdatePanel ID="UpdatePanel13" runat="server">
                        <ContentTemplate>
                            <asp:TextBox ID="txt_sem" ReadOnly="true" runat="server" CssClass="textbox  txtheight">--Select--</asp:TextBox>
                            <asp:Panel ID="pformate6" runat="server" CssClass="multxtpanel" Height="250px">
                                <asp:CheckBox ID="cb_sem" runat="server" Text="Select All" AutoPostBack="True" OnCheckedChanged="cb_sem_CheckedChanged" />
                                <asp:CheckBoxList ID="cbl_sem" runat="server" AutoPostBack="True" OnSelectedIndexChanged="cbl_sem_SelectedIndexChanged">
                                </asp:CheckBoxList>
                            </asp:Panel>
                            <asp:PopupControlExtender ID="PopupControlExtender8" runat="server" TargetControlID="txt_sem"
                                PopupControlID="pformate6" Position="Bottom">
                            </asp:PopupControlExtender>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </td>
                <td>
                    Section
                </td>
                <td>
                    <asp:UpdatePanel ID="UpdatePanel25" runat="server">
                        <ContentTemplate>
                            <asp:TextBox ID="txt_sec" Width="90px" ReadOnly="true" runat="server" CssClass="textbox  txtheight2">--Select--</asp:TextBox>
                            <asp:Panel ID="Panel_Sec" runat="server" CssClass="multxtpanel">
                                <asp:CheckBox ID="cb_sec" runat="server" Text="Select All" AutoPostBack="True" OnCheckedChanged="cb_sec_CheckedChanged" />
                                <asp:CheckBoxList ID="cbl_sec" runat="server" AutoPostBack="True" OnSelectedIndexChanged="cbl_sec_SelectedIndexChanged">
                                </asp:CheckBoxList>
                            </asp:Panel>
                            <asp:PopupControlExtender ID="PopupControlExtender25" runat="server" TargetControlID="txt_sec"
                                PopupControlID="Panel_Sec" Position="Bottom">
                            </asp:PopupControlExtender>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </td>
                <td>
                    <asp:Label ID="lbl_staffName" Width="100px" runat="server" Visible="true" Text="Staff Name"></asp:Label>
                </td>
                <td colspan="3">
                    <asp:UpdatePanel ID="UpdatePanel11" runat="server">
                        <ContentTemplate>
                            <asp:TextBox ID="txtstaffname" ReadOnly="true" runat="server" CssClass="textbox  txtheight5">--Select--</asp:TextBox>
                            <asp:Panel ID="Panel3" runat="server" CssClass="multxtpanel" Height="200px" Width="250px">
                                <asp:CheckBox ID="cb_staffname" runat="server" Text="Select All" AutoPostBack="True"
                                    OnCheckedChanged="cb_staffname_CheckedChanged" />
                                <asp:CheckBoxList ID="cbl_staffname" runat="server" AutoPostBack="True" OnSelectedIndexChanged="cbl_staffname_SelectedIndexChanged">
                                </asp:CheckBoxList>
                            </asp:Panel>
                            <asp:PopupControlExtender ID="PopupControlExtender4" runat="server" TargetControlID="txtstaffname"
                                PopupControlID="Panel3" Position="Bottom">
                            </asp:PopupControlExtender>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </td>
                <td>
                    <asp:Label ID="lbl_subject" Width="100px" runat="server" Visible="false" Text="Subject Name"></asp:Label>
                    <asp:Label ID="lblstaff_subject" Width="100px" runat="server" Visible="false" Text="Subject"></asp:Label>
                </td>
                <td>
                    <asp:UpdatePanel ID="UpdatePanel5" Visible="false" runat="server">
                        <ContentTemplate>
                            <asp:TextBox ID="Txt_Subject" Width=" 93px" ReadOnly="true" runat="server" CssClass="textbox  txtheight2">--Select--</asp:TextBox>
                            <asp:Panel ID="Panel_Subject" runat="server" CssClass="multxtpanel" Height="200px"
                                Width="200px">
                                <asp:CheckBox ID="Cb_Subject" runat="server" Text="Select All" AutoPostBack="True"
                                    OnCheckedChanged="Cb_Subject_CheckedChanged" />
                                <asp:CheckBoxList ID="Cbl_Subject" runat="server" AutoPostBack="True" OnSelectedIndexChanged="Cbl_Subject_SelectedIndexChanged">
                                </asp:CheckBoxList>
                            </asp:Panel>
                            <asp:PopupControlExtender ID="PopupControlExtender5" runat="server" TargetControlID="Txt_Subject"
                                PopupControlID="Panel_Subject" Position="Bottom">
                            </asp:PopupControlExtender>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                    <%--   <asp:UpdatePanel ID="UpdatePanel2" Visible="false" runat="server">
                        <ContentTemplate>
                            <asp:TextBox ID="txtsubstaff" Width=" 158px" ReadOnly="true" runat="server" CssClass="textbox  txtheight2">--Select--</asp:TextBox>
                            <asp:Panel ID="panelstaffsub" runat="server" CssClass="multxtpanel" Height="200px"
                                Width="200px">
                                <asp:CheckBox ID="cbstaffsub" runat="server" Text="Select All" AutoPostBack="True"
                                    OnCheckedChanged="cbstaffsub_CheckedChanged" />
                                <asp:CheckBoxList ID="cblsubstaff" runat="server" AutoPostBack="True" OnSelectedIndexChanged="cblsubstaff_selectedindexchanged">
                                </asp:CheckBoxList>
                            </asp:Panel>
                            <asp:PopupControlExtender ID="PopupControlExtender6" runat="server" TargetControlID="txtsubstaff"
                                PopupControlID="panelstaffsub" Position="Bottom">
                            </asp:PopupControlExtender>
                        </ContentTemplate>
                    </asp:UpdatePanel>--%>
                </td>
            </tr>
            <tr>
                <td>
                    Feedback Name
                </td>
                <td colspan="3">
                    <asp:UpdatePanel ID="UpdatePanel10" Visible="true" runat="server">
                        <ContentTemplate>
                            <asp:DropDownList ID="ddl_feedback" runat="server" Visible="true" Width="260px" Height="30px"
                                CssClass=" textbox1 ddlheight5" AutoPostBack="true" OnSelectedIndexChanged="ddl_feedback_SelectedIndexChanged">
                            </asp:DropDownList>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                        <ContentTemplate>
                            <asp:TextBox ID="txtfeedbackmulti" ReadOnly="true" runat="server" CssClass="textbox  txtheight5">--Select--</asp:TextBox>
                            <asp:Panel ID="Panel5" runat="server" CssClass="multxtpanel" Height="200px" Width="250px">
                                <asp:CheckBox ID="cbfeedbackmulti" runat="server" Text="Select All" AutoPostBack="True"
                                    OnCheckedChanged="cbfeedbackmulti_CheckedChanged" />
                                <asp:CheckBoxList ID="cblfeedbackmulti" runat="server" AutoPostBack="True" OnSelectedIndexChanged="cblfeedbackmulti_SelectedIndexChanged">
                                </asp:CheckBoxList>
                            </asp:Panel>
                            <asp:PopupControlExtender ID="PopupControlExtender1" runat="server" TargetControlID="txtfeedbackmulti"
                                PopupControlID="Panel5" Position="Bottom">
                            </asp:PopupControlExtender>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </td>
                <%--<td>
                        Subject Name
                    </td>
                    <td>
                        <asp:UpdatePanel ID="UpdatePanel12" Visible="true" runat="server">
                            <ContentTemplate>
                                <asp:TextBox ID="txtsubjectnameformat6" ReadOnly="true" runat="server" CssClass="textbox  txtheight4">--Select--</asp:TextBox>
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
                    </td>--%>
                <td>
                    <asp:CheckBox ID="chkwithcomments" Visible="true" runat="server" Text="Report With Comments" />
                </td>
                <td>
                    <asp:RadioButton ID="rdb_deptwise" runat="server" AutoPostBack="true" Text="Department Wise"
                        OnCheckedChanged="rdb_deptwise_Click" />
                </td>
                <td>
                    <asp:RadioButton ID="rdb_classwise" runat="server" AutoPostBack="true" Text="Class Wise"
                        OnCheckedChanged="rdb_classwise_Click" />
                </td>
                <td>
                    <asp:CheckBox ID="cb_WithOutRoundOff" Visible="false" runat="server" Text="Without RoundOff" />
                </td>
                <td>
                    <asp:RadioButton ID="Rdbques" runat="server" Visible="false" AutoPostBack="true"
                        Text="Display Question" OnCheckedChanged="rdb_Rdbques_Click" />
                </td>
                <td>
                    <asp:RadioButton ID="Rdbquesacr" runat="server" Visible="false" AutoPostBack="true"
                        Text="Display Question Acronym" OnCheckedChanged="rdb_Rdbquesacr_Click" />
                </td>
                <td>
                    <%--  <asp:CheckBox ID="cbIndividual" Visible="true" runat="server" Text="Individual FeedBack"
                        AutoPostBack="true" Checked="false" OnCheckedChanged="cb_individual_checkedchange" />--%>
                    <asp:CheckBox ID="cbmul" runat="server" Text="Multiple Feedback" AutoPostBack="true"
                        Checked="false" Visible="false" OnCheckedChanged="cbmul_checkedchange" />
                    <asp:Button ID="btn_go" runat="server" Text="Go" CssClass="textbox btn1" OnClick="btn_go_Click" />
                </td>
            </tr>
        </table>
        <br />
        <asp:Label ID="lbl_error" runat="server" ForeColor="Red"></asp:Label>
        <div id="SpreadDiv" runat="server" visible="false">
            <FarPoint:FpSpread ID="FpSpread1" runat="server" BorderStyle="Solid" BorderWidth="0px"
                CssClass="spreadborder" OnPreRender="FpSpread1_PreRender" autopostback="true"
                OnCellClick="FpSpread1_CellClick" ShowHeaderSelection="false">
                <Sheets>
                    <FarPoint:SheetView SheetName="Sheet1">
                    </FarPoint:SheetView>
                </Sheets>
            </FarPoint:FpSpread>
            <div>
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
                <asp:Button ID="btnExcel1" runat="server" OnClick="btnExcel_Click" Text="Export To Excel"
                    Width="127px" Height="31px" CssClass="textbox textbox1" />
                <asp:Button ID="btnprintmaster1" runat="server" Text="Print" OnClick="btnprintmaster_Click"
                    Width="60px" Height="31px" CssClass="textbox textbox1" />
                <Insproplus:printmaster runat="server" ID="Printcontrol1" Visible="false" />
            </div>
        </div>
    </center>
    <div id="contentDiv" runat="server" style="height: auto; width: 1344px;" visible="false">
    </div>
</asp:Content>

<%@ Page Title="" Language="C#" MasterPageFile="~/AttendanceMOD/AttendanceSubSiteMaster.master"
    AutoEventWireup="true" CodeFile="StudentConduct.aspx.cs" Inherits="StudentConduct"
    MaintainScrollPositionOnPostback="true" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Assembly="FarPoint.Web.Spread" Namespace="FarPoint.Web.Spread" TagPrefix="FarPoint" %>
<%@ Register Src="~/Usercontrols/PrintMaster.ascx" TagName="printmaster" TagPrefix="Insproplus" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <script type="text/javascript">
        function frelig() {
            document.getElementById('<%=btnaddfraction.ClientID%>').style.display = 'block';
            document.getElementById('<%=btnfractiobremove.ClientID%>').style.display = 'block';
        }
    </script>
    <style type="text/css">
        .modalPopup
        {
            background-color: #ffffdd;
            border-width: 1px;
            -moz-border-radius: 5px;
            border-style: solid;
            border-color: Gray;
            min-width: 250px;
            max-width: 500px;
            min-height: 100px;
            max-height: 150px;
            top: 100px;
            left: 150px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <center>
        <span class="fontstyleheader" style="color: Green; margin: 0px; margin-bottom: 10px;
            margin-top: 10px; position: relative;">Students Remark Details</span>
        <div style="width: 100%; height: auto;">
            <%--position: relative; margin: 0px; margin-bottom: 10px;
            margin-top: 1px; position: relative;--%>
            <center>
                <asp:Panel ID="Panel5" runat="server" class="maintablestyle" Style="border-style: solid;
                    border-width: thin; border-color: Black; background: #0CA6CA; width: 893px; margin: 0px;
                    margin-bottom: 10px; margin-top: 10px; position: relative;">
                    <table>
                        <tr>
                            <td align="left">
                                <table>
                                    <tr>
                                        <td>
                                            <asp:Label ID="lblBatch1" runat="server" Text="Batch " Font-Bold="True" Font-Names="Book Antiqua"
                                                Font-Size="Medium"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:DropDownList ID="ddlbatch" runat="server" OnSelectedIndexChanged="ddlbatch_SelectedIndexChanged"
                                                AutoPostBack="True" Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium"
                                                Height="25px" Width="80px">
                                            </asp:DropDownList>
                                        </td>
                                        <td>
                                        </td>
                                        <td>
                                            <asp:Label ID="lblDegree1" runat="server" Text="Degree" Font-Bold="True" Font-Names="Book Antiqua"
                                                Font-Size="Medium"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:DropDownList ID="ddldegree" runat="server" OnSelectedIndexChanged="ddldegree_SelectedIndexChanged"
                                                AutoPostBack="True" Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium"
                                                Height="25px" Width="85px">
                                            </asp:DropDownList>
                                        </td>
                                        <td>
                                        </td>
                                        <td>
                                            <asp:Label ID="lblBranch1" runat="server" Text="Branch" Font-Bold="True" Font-Names="Book Antiqua"
                                                Font-Size="Medium"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:DropDownList ID="ddlbranch" runat="server" OnSelectedIndexChanged="ddlbranch_SelectedIndexChanged"
                                                AutoPostBack="True" Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium"
                                                Height="25px" Width="284px">
                                            </asp:DropDownList>
                                        </td>
                                        <td>
                                        </td>
                                        <td>
                                            <asp:Label ID="lblSem1" runat="server" Text="Semester" Font-Bold="True" Font-Names="Book Antiqua"
                                                Font-Size="Medium"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:DropDownList ID="ddlsemester" runat="server" OnSelectedIndexChanged="ddlsemester_SelectedIndexChanged"
                                                AutoPostBack="True" Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium"
                                                Height="25px" Width="41px">
                                            </asp:DropDownList>
                                        </td>
                                        <td>
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
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td align="left">
                                <table>
                                    <tr>
                                        <td>
                                            <%--<asp:UpdatePanel ID="Upanelchk" runat="server">
                                                <ContentTemplate>--%>
                                                    <asp:CheckBox ID="chkdate" runat="server" AutoPostBack="true" OnCheckedChanged="chkdatevisible" />
                                            <%--    </ContentTemplate>
                                                <Triggers><asp:PostBackTrigger ControlID="chkdate" /></Triggers>
                                            </asp:UpdatePanel>--%>
                                        </td>
                                        <td>
                                            <asp:Label ID="lbldate" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                                Font-Size="Medium" Text="From Date"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtfromdate" runat="server" AutoPostBack="true" Width="80px"></asp:TextBox>
                                            <asp:CalendarExtender ID="calendetextenfordatext" TargetControlID="txtfromdate" runat="server"
                                                Format="dd-MM-yyyy">
                                            </asp:CalendarExtender>
                                        </td>
                                        <td>
                                            <asp:Label ID="lbltodat" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                                Font-Size="Medium" Text="To Date"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txttodate" runat="server" AutoPostBack="true" Width="80px" OnTextChanged="txttodate_TextChanged"></asp:TextBox>
                                            <asp:CalendarExtender ID="CalendarExtender3" TargetControlID="txttodate" runat="server"
                                                Format="dd-MM-yyyy">
                                            </asp:CalendarExtender>
                                        </td>
                                        <td>
                                        </td>
                                        <td>
                                            <asp:Label ID="lblsturollno" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                                Font-Size="Medium" Text="Roll No"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtsturollno" runat="server" AutoPostBack="true" OnTextChanged="txtsturollno_TextChanged"
                                                Width="80px"></asp:TextBox>
                                        </td>
                                        <td>
                                            <asp:Button ID="btngo" runat="server" Text="Go" OnClick="btngo_Click" Font-Bold="True"
                                                Font-Names="Book Antiqua" Font-Size="Medium" />
                                        </td>
                                        <td>
                                            <asp:Button ID="btnadd" runat="server" Text="ADD" Font-Bold="True" Font-Names="Book Antiqua"
                                                Font-Size="Medium" OnClick="btnadd_Click" />
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                    </table>
                </asp:Panel>
            </center>
            <FarPoint:FpSpread ID="FpSpread1" runat="server" Height="300" Width="840" ActiveSheetViewIndex="0"
                currentPageIndex="0" DesignString="&lt;?xml version=&quot;1.0&quot; encoding=&quot;utf-8&quot;?&gt;&lt;Spread /&gt;"
                EnableClientScript="False" CssClass="cursorptr" BorderColor="Black" BorderStyle="Solid"
                OnCellClick="FpSpread1_CellClick" OnPreRender="FpSpread1_SelectedIndexChanged"
                BorderWidth="0.5" Visible="False" OnUpdateCommand="FpSpread1_UpdateCommand1"
                Style="margin: 0px; margin-bottom: 10px; margin-top: 10px; position: relative;">
                <CommandBar BackColor="Control" ButtonType="PushButton">
                    <Background BackgroundImageUrl="SPREADCLIENTPATH:/img/cbbg.gif" />
                </CommandBar>
                <Pager Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                    Font-Underline="False" />
                <HierBar Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                    Font-Underline="False" />
                <Sheets>
                    <FarPoint:SheetView SheetName="Sheet1" EditTemplateColumnCount="2" GridLineColor="Black"
                        GroupBarText="Drag a column to group by that column." SelectionBackColor="#CE5D5A"
                        SelectionForeColor="White">
                    </FarPoint:SheetView>
                </Sheets>
                <TitleInfo BackColor="#E7EFF7" Font-Size="X-Large" ForeColor="" HorizontalAlign="Center"
                    VerticalAlign="NotSet" Font-Bold="False" Font-Italic="False" Font-Overline="False"
                    Font-Strikeout="False" Font-Underline="False">
                </TitleInfo>
            </FarPoint:FpSpread>
            <table style="margin: 0px; margin-bottom: 10px; margin-top: 10px; position: relative;">
                <tr>
                    <td>
                        <asp:Button ID="btnexcel" runat="server" OnClick="btnexcel_Click" Text="Export Excel"
                            Font-Names="Book Antiqua" Font-Size="Medium" Font-Bold="true" />
                    </td>
                    <td>
                        <asp:Button ID="btnprint" runat="server" Text="Print" OnClick="btnprintmaster_Click"
                            Font-Names="Book Antiqua" Font-Size="Medium" Font-Bold="true" />
                    </td>
                </tr>
            </table>
            <Insproplus:printmaster runat="server" ID="Printcontrol" Visible="false" />
            <asp:Label ID="norecordlbl" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                Font-Size="Medium" ForeColor="Red" Width="676px" Text="" Style="margin: 0px;
                margin-bottom: 10px; margin-top: 1px; position: relative;"></asp:Label>
            <asp:UpdatePanel ID="upd1" runat="server">
                <ContentTemplate>
                    <asp:UpdateProgress ID="UpdateProgress1" runat="server" DynamicLayout="true" AssociatedUpdatePanelID="upd1">
                        <ProgressTemplate>
                            <div class="CenterPB" style="height: 40px; width: 40px;">
                                <img src="../images/progress2.gif" height="180px" width="180px" />
                            </div>
                        </ProgressTemplate>
                    </asp:UpdateProgress>
                    <asp:ModalPopupExtender ID="modalPopup" runat="server" TargetControlID="UpdateProgress1"
                        PopupControlID="UpdateProgress1">
                    </asp:ModalPopupExtender>
                    <%--  Width="690px" Height="670px" position: absolute;
                            left: 200px--%>
                    <asp:Panel ID="panelrollnopop" runat="server" BorderColor="Black" BackColor="White"
                        Visible="false" BorderWidth="2px" Style="margin-left: auto; margin-right: auto;
                        top: 20%; left: 20%; position: absolute; width: auto; height: auto; z-index: 1000;">
                        <div class="PopupHeaderrstud2" id="Div3" style="text-align: center; font-family: MS Sans Serif;
                            font-size: Small; font-weight: bold">
                            <center>
                                <span style="text-align: center; margin-bottom: 30px; margin-top: 30px; position: relative;
                                    font-size: large; color: Green;">Student's Remark Entry </span>
                            </center>
                            <table style="text-align: left; margin-top: 15px; margin-bottom: 10px; position: relative;">
                                <tr>
                                    <td>
                                        <asp:Label ID="lblbatch" runat="server" Text="Batch" Font-Bold="True" Font-Names="Book Antiqua"
                                            Font-Size="Medium"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="ddlbatchadd" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                            Font-Size="Medium" OnSelectedIndexChanged="ddlbatch_SelectedIndexXhanged" AutoPostBack="True"
                                            Width="80px">
                                        </asp:DropDownList>
                                    </td>
                                    <td>
                                        <asp:Label ID="lbldegree" runat="server" Text="Degree" Font-Bold="True" Font-Names="Book Antiqua"
                                            Font-Size="Medium"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="ddldegreeadd" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                            Font-Size="Medium" OnSelectedIndexChanged="ddldegreeadd_SelectedIndexXhanged"
                                            AutoPostBack="True" Width="80px">
                                        </asp:DropDownList>
                                    </td>
                                    <td>
                                        <asp:Label ID="lblbranchadd" runat="server" Text="Branch" Font-Bold="True" Font-Names="Book Antiqua"
                                            Font-Size="Medium"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="ddlbrachadd" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                            Font-Size="Medium" OnSelectedIndexChanged="ddlbrachadd_SelectedIndexXhanged"
                                            AutoPostBack="True" Width="200px">
                                        </asp:DropDownList>
                                    </td>
                                    <td>
                                        <asp:Label ID="lblSem" runat="server" Text="Sem" Font-Bold="True" Font-Names="Book Antiqua"
                                            Font-Size="Medium"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="ddlsemadd" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                            Font-Size="Medium" OnSelectedIndexChanged="ddlsem_SelectedIndexXhanged" AutoPostBack="True"
                                            Width="40px">
                                        </asp:DropDownList>
                                    </td>
                                    <td>
                                        <asp:Label ID="Label6" runat="server" Text="Sec" Font-Bold="True" Font-Names="Book Antiqua"
                                            Font-Size="Medium"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="ddlsecadd" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                            Font-Size="Medium" OnSelectedIndexChanged="ddlsec_SelectedIndexXhanged" AutoPostBack="True"
                                            Width="40px">
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="left" colspan="10">
                                        <FarPoint:FpSpread ID="sprdselectrollno" runat="server" BorderColor="Black" BorderStyle="Solid"
                                            BorderWidth="1px" Height="300" Width="680" HorizontalScrollBarPolicy="AsNeeded"
                                            VerticalScrollBarPolicy="Never" OnButtonCommand="sprdselectrollno_UpdateCommand"
                                            OnCellClick="sprdselectrollno_CellClick" OnPreRender="sprdselectrollno_SelectedIndexChanged">
                                            <CommandBar BackColor="Control" ButtonFaceColor="Control" ButtonHighlightColor="ControlLightLight"
                                                ButtonShadowColor="ControlDark">
                                            </CommandBar>
                                            <Sheets>
                                                <FarPoint:SheetView SheetName="Sheet1">
                                                </FarPoint:SheetView>
                                            </Sheets>
                                        </FarPoint:FpSpread>
                                    </td>
                                </tr>
                            </table>
                            <table style="text-align: left; margin: 0px; margin-top: 5px; margin-bottom: 5px;">
                                <tr>
                                    <td>
                                        <div id="divLeftRoll" runat="server">
                                            <asp:Label ID="lblrollno" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                                Font-Size="Medium" Text="Roll No"></asp:Label>
                                        </div>
                                        <div id="divLeftAdmit" runat="server">
                                            <asp:Label ID="lblAdmitNo" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                                Font-Size="Medium" Text="Admission No"></asp:Label>
                                        </div>
                                    </td>
                                    <td>
                                        <div id="divRightRoll" runat="server">
                                            <asp:TextBox ID="txtstdrollno" runat="server" AutoPostBack="true" Font-Bold="True"
                                                Font-Names="Book Antiqua" Font-Size="Medium" OnTextChanged="txtstdrollno_TextChanged"
                                                Width="135px"></asp:TextBox>
                                            <asp:Label ID="man3" Text="*" runat="server" ForeColor="Red"></asp:Label>
                                            <asp:Label ID="lblindex" Visible="false" runat="server" ForeColor="Red"></asp:Label>
                                        </div>
                                        <div id="divRightAdmit" runat="server">
                                            <asp:TextBox ID="txtAdmissionNo" runat="server" AutoPostBack="true" Font-Bold="True"
                                                Font-Names="Book Antiqua" Font-Size="Medium" OnTextChanged="txtAdmissionNo_TextChanged"
                                                Width="135px"></asp:TextBox>
                                            <asp:Label ID="lblAdmitMand" Text="*" runat="server" ForeColor="Red"></asp:Label>
                                        </div>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="lbldate1" runat="server" Text="Date" Font-Bold="True" Font-Names="Book Antiqua"
                                            Font-Size="Medium" Width="130px"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtdate1" runat="server" AutoPostBack="true" Font-Bold="True" Font-Names="Book Antiqua"
                                            Font-Size="Medium" OnTextChanged="txtdate1_TextChanged" Width="90px"></asp:TextBox>
                                        <asp:CalendarExtender ID="CalendarExtender1" TargetControlID="txtdate1" runat="server"
                                            Format="dd-MM-yyyy">
                                        </asp:CalendarExtender>
                                        <asp:Label ID="lblman1" Text="*" ForeColor="Red" runat="server"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:Label ID="lblfraction" runat="server" Text="InFraction/Remark" Font-Bold="True"
                                            Font-Names="Book Antiqua" Font-Size="Medium"></asp:Label>
                                    </td>
                                    <td colspan="3">
                                        <table>
                                            <tr>
                                                <td>
                                                    <asp:Button ID="btnaddfraction" runat="server" Text="+" Font-Bold="True" Font-Names="Book Antiqua"
                                                        Font-Size="Small" OnClick="btnaddfraction_Click" Style="display: none; width: auto;
                                                        height: auto;" />
                                                </td>
                                                <td>
                                                    <asp:DropDownList ID="ddlfraction" runat="server" AutoPostBack="true" Font-Bold="True"
                                                        Font-Names="Book Antiqua" Font-Size="Medium" OnSelectedIndexChanged="ddlfraction_SelectedIndexChanged"
                                                        Width="145px">
                                                    </asp:DropDownList>
                                                </td>
                                                <td>
                                                    <asp:Button ID="btnfractiobremove" runat="server" Text="-" OnClick="btnfractiobremove_Click"
                                                        Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium" Style="display: none;
                                                        width: auto; height: auto;" />
                                                </td>
                                                <td>
                                                    <asp:Label ID="Label3" Text="*" ForeColor="Red" runat="server"></asp:Label>
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="lblaction" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                            Font-Size="Medium" Text="Action " Width="130px"></asp:Label>
                                    </td>
                                    <td colspan="5">
                                        <table>
                                            <tr>
                                                <td>
                                                    <asp:CheckBox ID="chkdismissal" runat="server" AutoPostBack="true" Font-Bold="True"
                                                        Font-Names="Book Antiqua" Text="Dismissal" Font-Size="Medium" OnCheckedChanged="chkdismissal_CheckedChanged" />
                                                    <%--<asp:Label ID="lbldisimsion" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                            Font-Size="Medium" Text="Dismissal"></asp:Label>--%>
                                                </td>
                                                <td>
                                                    <asp:CheckBox ID="chkwarning" runat="server" AutoPostBack="true" Font-Bold="True"
                                                        Font-Names="Book Antiqua" Text="Warning" Font-Size="Medium" OnCheckedChanged="chkwarning_CheckedChanged" />
                                                    <%-- <asp:Label ID="lblwarning" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                            Font-Size="Medium" Text="Warning "></asp:Label>--%>
                                                </td>
                                                <td>
                                                    <asp:CheckBox ID="chksuspension" runat="server" AutoPostBack="true" OnCheckedChanged="chksuspension_CheckedChanged"
                                                        Font-Bold="True" Font-Names="Book Antiqua" Text="Suspension" Font-Size="Medium" />
                                                    <%--<asp:Label ID="lblsuspension" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                            Font-Size="Medium" ></asp:Label>--%>
                                                    <asp:CheckBox ID="chkfine" runat="server" AutoPostBack="true" OnCheckedChanged="chkfine_CheckedChanged"
                                                        Font-Bold="True" Text="Fine" Font-Names="Book Antiqua" Font-Size="Medium" />
                                                    <%--<asp:Label ID="lblfine1" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                            Font-Size="Medium" Text="Fine " ></asp:Label>--%>
                                                </td>
                                                <td>
                                                    <asp:CheckBox ID="chkfeeofroll" runat="server" AutoPostBack="true" OnCheckedChanged="chkfeeofroll_CheckedChanged"
                                                        Font-Bold="True" Font-Names="Book Antiqua" Text="Fee Off The Roll" Font-Size="Medium" />
                                                    <%-- <asp:Label ID="Label7" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                            Font-Size="Medium" Text="Fee Off The Roll "></asp:Label>--%>
                                                </td>
                                                <td>
                                                    <asp:CheckBox ID="chkremark" runat="server" Text="Remarks" Font-Bold="True" Font-Names="Book Antiqua"
                                                        Font-Size="Medium" />
                                                    <%-- <asp:Label ID="Label8" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                            Font-Size="Medium"></asp:Label>--%>
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                    </td>
                                    <td colspan="5">
                                        <asp:CheckBox ID="chkfeeonroll" runat="server" AutoPostBack="true" Font-Bold="True"
                                            Font-Names="Book Antiqua" Font-Size="Medium" Text="Fees On The Roll" Visible="false"
                                            OnCheckedChanged="chkfeeonroll_CheckedChanged" />
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="lblprof" runat="server" Text="Prof Incharge" Font-Bold="True" Font-Names="Book Antiqua"
                                            Font-Size="Medium"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtstaff" runat="server" ReadOnly="True" Width="100px" Font-Bold="True"
                                            Font-Names="Book Antiqua" Font-Size="Medium"></asp:TextBox>
                                        <asp:Button ID="btnstaff" runat="server" OnClick="btnstaff_Click" Font-Bold="True"
                                            Font-Names="Book Antiqua" Font-Size="Medium" Text="?" />
                                    </td>
                                    <td colspan="2">
                                        <table>
                                            <tr>
                                                <td>
                                                    <asp:Label ID="lblfine" runat="server" Text="Fine Amount" Font-Bold="True" Font-Names="Book Antiqua"
                                                        Font-Size="Medium"></asp:Label>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtfine" runat="server" Width="50px" MaxLength="6" Font-Bold="True"
                                                        Font-Names="Book Antiqua" Font-Size="Medium"></asp:TextBox>
                                                    <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" ControlToValidate="txtfine"
                                                        Display="Static" EnableClientScript="true" ErrorMessage="*" ForeColor="#FF3300"
                                                        ValidationExpression="(^[1-9]\d*$)" />
                                                    <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender2" runat="server" TargetControlID="txtfine"
                                                        FilterType="Numbers" />
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                    <td colspan="2">
                                        <div id="divFeeOnRollDate" runat="server" visible="false">
                                            <table>
                                                <tr>
                                                    <td>
                                                        <asp:Label ID="lblFeeOnRollDate" runat="server" Text="Fee On Roll Date" Font-Bold="True"
                                                            Font-Names="Book Antiqua" Font-Size="Medium"></asp:Label>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="txtFeeOnRollDate" runat="server" AutoPostBack="true" Font-Bold="True"
                                                            Font-Names="Book Antiqua" Font-Size="Medium" OnTextChanged="txtFeeOnRollDate_TextChanged"
                                                            Width="90px"></asp:TextBox>
                                                        <asp:CalendarExtender ID="calExtFeeOnRollDate" TargetControlID="txtFeeOnRollDate"
                                                            runat="server" Format="dd-MM-yyyy">
                                                        </asp:CalendarExtender>
                                                    </td>
                                                </tr>
                                            </table>
                                        </div>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="lblstartdate" runat="server" Text="Start Date" Font-Bold="True" Font-Names="Book Antiqua"
                                            Font-Size="Medium"></asp:Label>
                                    </td>
                                    <td colspan="3">
                                        <table>
                                            <tr>
                                                <td>
                                                    <asp:TextBox ID="txtstartdate" runat="server" Width="90px" AutoPostBack="true" OnTextChanged="txtstartdate_TextChanged"
                                                        Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium"></asp:TextBox>
                                                    <asp:CalendarExtender ID="CalendarExtender2" runat="server" Format="dd-MM-yyyy" TargetControlID="txtstartdate" />
                                                </td>
                                                <td>
                                                    <asp:Label ID="lblEndDate" runat="server" Text="End Date" Font-Bold="True" Font-Names="Book Antiqua"
                                                        Font-Size="Medium"></asp:Label>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtEndDate" runat="server" Width="90px" AutoPostBack="true" OnTextChanged="txtEndDate_TextChanged"
                                                        Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium"></asp:TextBox>
                                                    <asp:CalendarExtender ID="calExtEndDate" runat="server" Format="dd-MM-yyyy" TargetControlID="txtEndDate" />
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                    <td>
                                        <asp:Label ID="lbldays" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                            Font-Size="Medium" Text="Days"></asp:Label>
                                        <asp:TextBox ID="txtdays" Enabled="false" runat="server" Width="50px" AutoPostBack="true"
                                            OnTextChanged="txtdays_Changed" MaxLength="3" Font-Bold="True" Font-Names="Book Antiqua"
                                            Font-Size="Medium"></asp:TextBox>
                                        <asp:RegularExpressionValidator ID="RegularExpressionValidator2" runat="server" ControlToValidate="txtdays"
                                            Display="Static" EnableClientScript="true" ErrorMessage="*" ForeColor="#FF3300"
                                            ValidationExpression="\d+"></asp:RegularExpressionValidator>
                                        <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" runat="server" TargetControlID="txtdays"
                                            FilterType="Numbers" />
                                    </td>
                                    <td>
                                        <asp:Label ID="lblerrstaffcode" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                            Font-Size="Medium" ForeColor="Red" Text="" Width="150px" Visible="false"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="6">
                                        <asp:Label ID="lblErrMsg" runat="server" Text="" Font-Bold="true" ForeColor="Red"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="lblremark" runat="server" Font-Bold="True" Text="Remarks" Font-Names="Book Antiqua"
                                            Font-Size="Medium"></asp:Label>
                                    </td>
                                    <td colspan="5">
                                        <asp:TextBox ID="txtremarks" runat="server" Width="450px" TextMode="MultiLine" MaxLength="500"
                                            Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium" Style="resize: none;"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="6">
                                        <asp:Label ID="lblnorec" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                            Font-Size="Medium" ForeColor="Red" Text=" "></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="6">
                                        <asp:Label ID="errmsg" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                            Font-Size="Medium" ForeColor="Red"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="3">
                                    </td>
                                    <td align="right" colspan="3">
                                        <fieldset style="width: auto; height: auto;">
                                            <asp:Button ID="btnsave" runat="server" OnClick="btnsave_Click" Text="Save" Font-Bold="True"
                                                Font-Names="Book Antiqua" Font-Size="Medium" Width="70px" />
                                            <asp:Button ID="btndelete" runat="server" OnClick="btndelete_Click" Text="Delete"
                                                Width="70px" Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium" />
                                            <asp:Button ID="btnexit" runat="server" OnClick="btnexit_Click" Text="Exit" Width="70px"
                                                Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium" />
                                        </fieldset>
                                    </td>
                                </tr>
                            </table>
                        </div>
                    </asp:Panel>
                    <asp:Panel ID="pnlmsgboxdelete" runat="server" CssClass="modalPopup" Style="display: none;
                        height: 100; width: 300; z-index: 2000;" DefaultButton="btnOk">
                        <table width="100%">
                            <tr class="topHandle">
                                <td colspan="2" align="left" runat="server" id="tdCaption">
                                    <asp:Label ID="lblCaption" runat="server" Font-Bold="True" Text="Confirmation" Font-Names="Book Antiqua"
                                        Font-Size="Large"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td style="width: 60px" valign="middle" align="center">
                                    <asp:Image ID="imgInfo" runat="server" ImageUrl="~/Info-48x48.png" />
                                </td>
                                <td valign="middle" align="left">
                                    <asp:Label ID="lblMessage" Text="Do You want to Delete the Record?" runat="server"
                                        Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2" align="right">
                                    <asp:Button ID="btnOk" runat="server" Text="Yes" OnClick="btnOk_Click" Font-Bold="True"
                                        Font-Names="Book Antiqua" Font-Size="Medium" />
                                    <asp:Button ID="btnCancel" runat="server" Text="No" OnClick="btnCancel_Click" Font-Bold="True"
                                        Font-Names="Book Antiqua" Font-Size="Medium" />
                                </td>
                            </tr>
                        </table>
                    </asp:Panel>
                    <asp:HiddenField runat="server" ID="hfdelete" />
                    <asp:ModalPopupExtender ID="mpemsgboxdelete" runat="server" TargetControlID="hfdelete"
                        PopupControlID="pnlmsgboxdelete">
                    </asp:ModalPopupExtender>
                    <asp:Panel ID="panel3" runat="server" BorderColor="Black" BackColor="AliceBlue" Visible="false"
                        BorderWidth="2px" Style="left: 20%; top: 25%; right: 20%; z-index: 2000; position: absolute;
                        width: auto;" Height="580px">
                        <div class="PopupHeaderrstud2" id="Div1" style="text-align: center; font-family: MS Sans Serif;
                            font-size: Small; font-weight: bold; margin-bottom: 10px; margin-top: 10px; width: auto;
                            height: auto;">
                            <center>
                                <span style="top: 35px; text-align: center; bottom: 30px; position: relative; font-size: large;
                                    color: Green;">Select Staff Incharge </span>
                            </center>
                            <table style="text-align: left; margin-bottom: 10px; position: relative;">
                                <tr>
                                    <td>
                                        <asp:Label ID="lblcollege" runat="server" Text="College" Font-Bold="True" Font-Names="Book Antiqua"
                                            Font-Size="Medium"></asp:Label>
                                    </td>
                                    <td>
                                        <%--sankar add--%>
                                        <asp:DropDownList ID="ddlcollege" runat="server" Width="150px" AutoPostBack="true"
                                            OnSelectedIndexChanged="ddlcollege_SelectedIndexChanged" Font-Bold="True" Font-Names="Book Antiqua"
                                            Font-Size="Medium">
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="lblDepartment" runat="server" Text="Department" Font-Bold="True" Font-Names="Book Antiqua"
                                            Font-Size="Medium"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="ddldepratstaff" runat="server" Width="150px" OnSelectedIndexChanged="ddldepratstaff_SelectedIndexChanged"
                                            Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium" AutoPostBack="true">
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="lblsearchby" runat="server" Text="Staff By" Font-Bold="True" Font-Names="Book Antiqua"
                                            Font-Size="Medium"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="ddlstaff" runat="server" Width="150px" OnSelectedIndexChanged="ddlstaff_SelectedIndexChanged"
                                            Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium" AutoPostBack="true">
                                            <asp:ListItem Value="0">Staff Name</asp:ListItem>
                                            <asp:ListItem Value="1">Staff Code</asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txt_search" runat="server" OnTextChanged="txt_search_TextChanged"
                                            AutoPostBack="True" Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium"></asp:TextBox>
                                    </td>
                                </tr>
                            </table>
                            <table>
                                <tr>
                                    <td align="justify">
                                        <FarPoint:FpSpread ID="fsstaff" runat="server" ActiveSheetViewIndex="0" Height="300"
                                            Width="800" VerticalScrollBarPolicy="AsNeeded" BorderWidth="0.5" Visible="False"
                                            Style="margin: 0px; margin-top: 10px; margin-bottom: 10px; position: relative;">
                                            <CommandBar BackColor="Control" ButtonType="PushButton">
                                                <Background BackgroundImageUrl="SPREADCLIENTPATH:/img/cbbg.gif" />
                                            </CommandBar>
                                            <Sheets>
                                                <FarPoint:SheetView SheetName="Sheet1">
                                                </FarPoint:SheetView>
                                            </Sheets>
                                        </FarPoint:FpSpread>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="right">
                                        <fieldset style="width: 160px; height: auto; position: absolute; top: 520px; left: 480px;">
                                            <asp:Button runat="server" ID="btnstaffadd" Text="Ok" OnClick="btnstaffadd_Click"
                                                Width="75px" Font-Bold="True" />
                                            <asp:Button runat="server" ID="btnexitpop" Text="Exit" OnClick="exitpop_Click" Width="75px"
                                                Font-Bold="True" />
                                        </fieldset>
                                    </td>
                                </tr>
                            </table>
                        </div>
                    </asp:Panel>
                    <asp:Panel ID="panel4" runat="server" BorderColor="Black" BackColor="AliceBlue" Visible="false"
                        BorderWidth="2px" Style="left: 25%; right: 25%; top: 25%; position: absolute;
                        z-index: 2000; height: auto;" Width="690px">
                        <div class="panelinfraction" id="Div2" style="text-align: center; font-family: MS Sans Serif;
                            font-size: Small; font-weight: bold; margin: 0px; margin-bottom: 10px; margin-top: 10px;
                            padding: 2px; width: 100%;">
                            <center>
                                <span style="margin: 0px; margin-bottom: 20px; margin-top: 10px; position: relative;
                                    color: Green; font-size: large; text-align: center;">Infraction Type </span>
                            </center>
                            <table style="margin: 0px; margin-bottom: 10px; margin-top: 10px; position: relative;
                                width: 100%;">
                                <tr>
                                    <td align="right">
                                        <asp:Label ID="lblfractionnew" runat="server" Text="Infraction Type" Font-Bold="True"
                                            Font-Names="Book Antiqua" Font-Size="Medium"></asp:Label>
                                    </td>
                                    <td align="left">
                                        <asp:TextBox ID="txtfractionnew" runat="server" Width="600px" Height="30px" TextMode="MultiLine"
                                            Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium" Style="resize: none;"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2" align="center">
                                        <table>
                                            <tr>
                                                <td>
                                                    <asp:Button ID="btnfractionnew" runat="server" Text="Add" OnClick="btnfractionnew_Click"
                                                        Style="height: auto; width: auto" Font-Bold="True" Font-Names="Book Antiqua"
                                                        Font-Size="Medium" />
                                                </td>
                                                <td>
                                                    <asp:Button ID="btninfractionexit" runat="server" Text="Exit" Style="height: auto;
                                                        width: auto" OnClick="btninfractionexit_Click" Font-Bold="True" Font-Names="Book Antiqua"
                                                        Font-Size="Medium" />
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                            </table>
                        </div>
                    </asp:Panel>
                </ContentTemplate>
                <Triggers>
                    <asp:AsyncPostBackTrigger ControlID="sprdselectrollno" />
                </Triggers>
            </asp:UpdatePanel>
        </div>
    </center>
</asp:Content>

<%@ Page Title="" Language="C#" MasterPageFile="~/AttendanceMOD/AttendanceSubSiteMaster.master"
    AutoEventWireup="true" CodeFile="NewBatchschedular.aspx.cs" Inherits="NewBatchschedular"
    EnableEventValidation="false" %>

<%@ Register Src="~/Usercontrols/Input_Events.ascx" TagName="collegedeatils" TagPrefix="UC" %>
<%@ Register Src="~/Usercontrols/PrintMaster.ascx" TagName="printmaster" TagPrefix="Insproplus" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Assembly="FarPoint.Web.Spread" Namespace="FarPoint.Web.Spread" TagPrefix="FarPoint" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">
    <link href="~/Styles/css/Commoncss.css" rel="Stylesheet" type="text/css" />
    <script type="text/javascript">
        function display() {
            document.getElementById('<%=lblvalidation1.ClientID %>').innerHTML = "";
        }
    </script>
    <asp:UpdatePanel ID="UPD1" runat="server">
        <ContentTemplate>
            <asp:UpdateProgress ID="UpdateProgress1" runat="server" DynamicLayout="true" AssociatedUpdatePanelID="UPD1">
                <ProgressTemplate>
                    <div class="CenterPB" style="height: 40px; width: 40px;">
                        <img alt="" src="../images/progress2.gif" height="180px" width="180px" />
                    </div>
                </ProgressTemplate>
            </asp:UpdateProgress>
            <asp:ModalPopupExtender ID="modalPopup" runat="server" TargetControlID="UpdateProgress1"
                PopupControlID="UpdateProgress1">
            </asp:ModalPopupExtender>
            <asp:ScriptManager ID="ScriptManager1" runat="server">
            </asp:ScriptManager>
            <div>
                <center>
                    <span class="fontstyleheader" style="color: Green; margin: 0px; margin-bottom: 0px;
                        margin-top: 0px;">Batch Allocation</span>
                </center>
            </div>
            <center>
                <div>
                    <table style="width: 990px; margin: 0px; margin-bottom: 0px; margin-top: 0px; position: relative;
                        margin: 0px; margin-bottom: 0px; margin-top: 0px; background-color: lightblue;
                        border-color: Black; border-width: 1px; border-style: solid;">
                        <tr>
                            <td colspan="7">
                                <UC:collegedeatils ID="user_control" runat="server" />
                            </td>
                        </tr>
                        <tr style="">
                            <td>
                                <asp:Label ID="lblnobatch" runat="server" Text="No of Batches" Style="font-weight: bold;
                                    font-family: book antiqua; font-size: medium;"></asp:Label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtbatch" runat="server" OnTextChanged="txtbatch_TextChanged" Width="40"
                                    Font-Bold="true" Font-Names="Book Antiqua" Font-Size="Medium" AutoPostBack="true"
                                    MaxLength="2"></asp:TextBox>
                                <asp:FilteredTextBoxExtender ID="filt" runat="server" FilterType="Numbers" TargetControlID="txtbatch">
                                </asp:FilteredTextBoxExtender>
                            </td>
                            <td>
                                <asp:Label ID="lblbatches" runat="server" Text="LabBatch:" Font-Bold="true" Width="71"
                                    Style="font-weight: bold; font-family: book antiqua; font-size: medium;"></asp:Label>
                            </td>
                            <td>
                                <asp:DropDownList ID="ddlnobatches" runat="server" Width="84px" Font-Bold="true"
                                    AutoPostBack="true" Font-Names="Book Antiqua" Font-Size="Medium" OnSelectedIndexChanged="ddlnobatches_SelectedIndexChanged">
                                </asp:DropDownList>
                            </td>
                            <td>
                                <asp:Label ID="lblTimetable" runat="server" Text="TimeTable Name" Style="font-weight: bold;
                                    font-family: book antiqua; font-size: medium;"></asp:Label>
                            </td>
                            <td>
                                <asp:DropDownList ID="ddltimetable" runat="server" Font-Bold="true" OnSelectedIndexChanged="ddltimetable_SelectedIndexChanged">
                                </asp:DropDownList>
                            </td>
                            <td>
                                <asp:Button ID="btnGo" Text="Go" runat="server" OnClick="btnGo_Click" Style="font-weight: bold;
                                    font-family: book antiqua; font-size: medium;" Visible="true" />
                            </td>
                        </tr>
                    </table>
                    <%--<table style="width: 800px; margin: 0px; margin-bottom: 0px; margin-top: 0px; background-color: lightblue;
                        border-color: Black; border-width: 1px; border-style: solid;">
                        <tr>
                            <td>
                            </td>
                        </tr>
                    </table>--%>
                </div>
            </center>
            <asp:Label ID="lblerror" runat="server" Text="Label" ForeColor="Red" Font-Bold="true"
                Font-Size="Medium" Font-Names="Book Antiqua" Style="margin: 0px; margin-bottom: 0px;
                margin-top: 0px; position: relative;"></asp:Label>
            <center>
                <div style="margin: 0px; margin-bottom: 0px; margin-top: 0px; width: 1000px;">
                    <div style="width: 508px; float: left; height: auto;">
                        <fieldset id="Fieldset1" runat="server" style="height: auto; width: 100%;">
                            <legend style="font-weight: bold; font-family: book antiqua; font-size: medium;">Batch
                                Allocation </legend>
                            <FarPoint:FpSpread ID="batch_spread" runat="server" BorderColor="Black" BorderStyle="Solid"
                                BorderWidth="1px" Height="300" Width="500" ShowHeaderSelection="false" CommandBar-Visible="false"
                                OnUpdateCommand="batch_spread_UpdateCommand1">
                                <CommandBar BackColor="Control" ButtonFaceColor="Control" ButtonHighlightColor="ControlLightLight"
                                    ButtonShadowColor="ControlDark">
                                </CommandBar>
                                <Sheets>
                                    <FarPoint:SheetView SheetName="Sheet1">
                                    </FarPoint:SheetView>
                                </Sheets>
                            </FarPoint:FpSpread>
                            <fieldset id="Fieldset2" runat="server" style="height: auto; width: 305px; margin: 0px;
                                margin-top: 5px; margin-bottom: 5px;">
                                <table>
                                    <tr>
                                        <td>
                                            <asp:CheckBox ID="CheckBox1" runat="server" Text="Select" AutoPostBack="true" OnCheckedChanged="CheckBox1_CheckedChanged" />
                                            <asp:Label ID="lblselect" Visible="false" runat="server" Text="Select"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:Label ID="lblfrom" runat="server" Text="From"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="fromno" runat="server" Style="width: 53px;"></asp:TextBox>
                                            <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender2" runat="server" TargetControlID="fromno"
                                                FilterType="Numbers" />
                                        </td>
                                        <td>
                                            <asp:Label ID="lblto" runat="server" Text="To"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="tono" runat="server" Style="width: 53px;"></asp:TextBox>
                                            <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" runat="server" TargetControlID="tono"
                                                FilterType="Numbers" />
                                        </td>
                                        <td>
                                            <asp:Button ID="Button2" runat="server" Text="Go" OnClick="selectgo_Click" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="6" align="center">
                                            <asp:Button ID="Btnsave" runat="server" Text="Save" OnClick="Btnsave_Click" />
                                            <asp:Button ID="Btndelete" runat="server" Text="Delete" OnClick="Btndelete_Click" />
                                        </td>
                                    </tr>
                                </table>
                            </fieldset>
                            <div id="rptprint" runat="server" visible="false">
                                <table>
                                    <tr>
                                        <td colspan="5">
                                            <asp:Label ID="lblvalidation1" runat="server" ForeColor="Red" Text="Please Enter Your Report Name"
                                                Visible="false"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Label ID="lblrptname" runat="server" Text="Report Name"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtexcelname" runat="server" CssClass="textbox textbox1" Height="20px"
                                                Width="180px" onkeypress="display()"></asp:TextBox>
                                        </td>
                                        <td>
                                            <asp:Button ID="btnExcel" runat="server" OnClick="btnExcel_Click" Text="Export To Excel"
                                                CssClass="textbox btn2" Style="width: auto; height: auto;" />
                                        </td>
                                        <td>
                                            <asp:Button ID="btnprintmaster" runat="server" Text="Print" OnClick="btnprintmaster_Click"
                                                CssClass="textbox btn2" Style="width: auto; height: auto;" />
                                            <Insproplus:printmaster runat="server" ID="Printcontrol" Visible="false" />
                                        </td>
                                    </tr>
                                </table>
                            </div>
                        </fieldset>
                    </div>
                    <div style="float: right; width: 410px; height: auto;">
                        <fieldset id="Fieldset3" runat="server" style="height: auto; width: 100%;">
                            <legend style="font-weight: bold; font-family: book antiqua; font-size: medium;">Semester
                                Schedule Settings</legend>
                            <FarPoint:FpSpread ID="Batchallot_spread" runat="server" BorderColor="Black" BorderStyle="Solid"
                                BorderWidth="1px" Height="250" Width="400" CommandBar-Visible="false" OnCellClick="Batchallot_spread_CellClick"
                                OnPreRender="Batchallot_spread_SelectedIndexChanged" ShowHeaderSelection="false"
                                OnUpdateCommand="Batchallot_spread_UpdateCommand1">
                                <CommandBar BackColor="Control" ButtonFaceColor="Control" ButtonHighlightColor="ControlLightLight"
                                    ButtonShadowColor="ControlDark">
                                </CommandBar>
                                <Sheets>
                                    <FarPoint:SheetView SheetName="Sheet1">
                                    </FarPoint:SheetView>
                                </Sheets>
                            </FarPoint:FpSpread>
                            <table>
                                <tr>
                                    <td align="left">
                                        <asp:LinkButton ID="LinkButton1" runat="server" CausesValidation="False" Font-Bold="True"
                                            Font-Names="Book Antiqua" Font-Size="Small" ForeColor="blue" Style="width: 150px;
                                            text-align: left;" OnClick="LinkButton1_Click">To Add Multiple Batch</asp:LinkButton>
                                    </td>
                                    <td align="right">
                                        <fieldset id="Fieldset4" runat="server" style="height: auto; width: 50px; text-align: right;">
                                            <asp:Button ID="Button1" runat="server" Text="Save" OnClick="Batchallotsave_Click" />
                                        </fieldset>
                                    </td>
                                </tr>
                            </table>
                            <fieldset id="Fieldset5" runat="server" style="width: 116px; background-color: white;
                                height: 84px;">
                                <asp:CheckBoxList ID="Checkboxlistbatch" runat="server" Style="width: 92px; border-style: double;"
                                    OnSelectedIndexChanged="Checkboxlistbatch_SelectedIndexChanged">
                                </asp:CheckBoxList>
                                <asp:Button ID="Button3" runat="server" Text="Ok" OnClick="Button3_Click" />
                            </fieldset>
                            <fieldset id="Fieldset6" runat="server" style="height: auto; width: auto; text-align: left;">
                                <table>
                                    <tr>
                                        <td align="left">
                                            <asp:CheckBox ID="chkautoswitch" runat="server" Text="Automatic Batch Switch" Font-Bold="True"
                                                Font-Names="Book Antiqua" AutoPostBack="true" OnCheckedChanged="chkautoswitch_CheckedChanged"
                                                Font-Size="Small" Style="height: auto; width: auto;" />
                                        </td>
                                        <td align="right">
                                            <fieldset id="Fieldset7" runat="server" style="width: 150px; background-color: white;
                                                height: 84px;">
                                                <table>
                                                    <tr>
                                                        <td>
                                                            <asp:TextBox ID="txtautoswitch" runat="server" CssClass="Dropdown_Txt_Box" ReadOnly="true"
                                                                Width="150px" Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium">---Select---</asp:TextBox>
                                                            <asp:Panel ID="pautoswitch" runat="server" CssClass="multxtpanel" Height="150px"
                                                                Width="200px">
                                                                <asp:CheckBox ID="chkswitch" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                                                    Font-Size="Medium" Text="Select All" AutoPostBack="True" OnCheckedChanged="chkswitch_CheckedChanged" />
                                                                <asp:CheckBoxList ID="chklsautoswitch" runat="server" Font-Size="Medium" AutoPostBack="True"
                                                                    Font-Bold="True" Font-Names="Book Antiqua" OnSelectedIndexChanged="chklsautoswitch_SelectedIndexChanged">
                                                                </asp:CheckBoxList>
                                                            </asp:Panel>
                                                            <asp:PopupControlExtender ID="PopupControlExtender1" runat="server" TargetControlID="txtautoswitch"
                                                                PopupControlID="pautoswitch" Position="Bottom">
                                                            </asp:PopupControlExtender>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td align="center">
                                                            <asp:Button ID="btnautoswitch" runat="server" Text="Ok" OnClick="btnautoswitch_Click" />
                                                        </td>
                                                    </tr>
                                                </table>
                                            </fieldset>
                                        </td>
                                    </tr>
                                </table>
                            </fieldset>
                        </fieldset>
                    </div>
                </div>
            </center>
        </ContentTemplate>
        <Triggers>
            <asp:PostBackTrigger ControlID="btnExcel" />
        </Triggers>
    </asp:UpdatePanel>
</asp:Content>

<%@ Page Title="" Language="C#" MasterPageFile="~/AttendanceMOD/AttendanceSubSiteMaster.master"
    AutoEventWireup="true" CodeFile="Subject Room Allotement.aspx.cs" Inherits="AttendanceMOD_SubjectRoomAllotement" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Src="~/Usercontrols/PrintMaster.ascx" TagName="printmaster" TagPrefix="Insproplus" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <style type="text/css">
        .style1
        {
            width: 370px;
        }
    </style>
    <style type="text/css">
        .floats
        {
            height: 26px;
        }
        .CenterPB
        {
            position: absolute;
            left: 50%;
            top: 50%;
            margin-top: -20px;
            margin-left: -20px;
            width: auto;
            height: auto;
        }
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
        .modalPopup1
        {
            background-color: #ffffdd;
            border-width: 1px;
            -moz-border-radius: 5px;
            border-style: solid;
            border-color: Gray;
            min-width: 250px;
            max-width: 700px;
            min-height: 100px;
            max-height: 250px;
            overflow: scroll;
            top: 100px;
            left: 150px;
        }
    </style>
    <script language="javascript" type="text/javascript" src="../Scripts/jquery-1.4.1.js"></script>
    <style type="text/css">
        .GridDock
        {
        }
    </style>
    <script type="text/javascript">
        function Check_Click2() {

            document.getElementById('<%=imgdiv2.ClientID %>').style.display = "none";
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <center>
        <span class="fontstyleheader" style="color: Green">Subject Room Timetable </span>
        <br />
        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
            <ContentTemplate>
                <table style="width: 900px; height: 70px; background-color: #0CA6CA;">
                    <tr>
                        <td>
                            <asp:Label ID="lblcolg" runat="server" Text="College " Font-Bold="True" Font-Names="Book Antiqua"
                                Font-Size="Medium"></asp:Label>
                        </td>
                        <td>
                            <asp:DropDownList ID="ddlcolg" runat="server" Width="100px" Font-Bold="True" Font-Names="Book Antiqua"
                                Font-Size="Medium" AutoPostBack="true" OnSelectedIndexChanged="collook_load">
                            </asp:DropDownList>
                        </td>
                        <td>
                            <asp:Label ID="lblbatch" runat="server" Text="Batch " Font-Bold="True" Font-Names="Book Antiqua"
                                Font-Size="Medium"></asp:Label>
                        </td>
                        <td>
                            <asp:DropDownList ID="ddlbatch" runat="server" Width="80px" Font-Bold="True" Font-Names="Book Antiqua"
                                Font-Size="Medium" AutoPostBack="true" OnSelectedIndexChanged="ddlbatch_SelectedIndexChanged">
                            </asp:DropDownList>
                        </td>
                        <td>
                            <asp:Label ID="lbldegree" runat="server" Text="Degree " Font-Bold="True" Font-Names="Book Antiqua"
                                Font-Size="Medium"></asp:Label>
                        </td>
                        <td>
                            <asp:DropDownList ID="ddldegree" runat="server" Font-Bold="True" Width="80" Font-Names="Book Antiqua"
                                Font-Size="Medium" AutoPostBack="true" OnSelectedIndexChanged="ddldegree_SelectedIndexChanged">
                            </asp:DropDownList>
                        </td>
                        <td>
                            <asp:Label ID="lblbranch" runat="server" Text="Branch " Font-Bold="True" Font-Names="Book Antiqua"
                                Font-Size="Medium"></asp:Label>
                        </td>
                        <td id="tdbranch" runat="server" colspan="2">
                            <asp:DropDownList ID="ddlbranch" runat="server" Width="220px" Font-Bold="True" Font-Names="Book Antiqua"
                                Font-Size="Medium" AutoPostBack="true" OnSelectedIndexChanged="ddlbranch_SelectedIndexChanged">
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label ID="lblsem" runat="server" Text="Sem" Font-Bold="True" Font-Names="Book Antiqua"
                                Font-Size="Medium"></asp:Label>
                        </td>
                        <td>
                            <asp:DropDownList ID="ddlsem" runat="server" Width="50px" Font-Bold="True" Font-Names="Book Antiqua"
                                Font-Size="Medium" AutoPostBack="true" OnSelectedIndexChanged="ddlsem_SelectedIndexChanged">
                            </asp:DropDownList>
                        </td>
                        <td>
                            <asp:Label ID="lblsec" runat="server" Text="Sec " Font-Bold="True" Font-Names="Book Antiqua"
                                Font-Size="Medium"></asp:Label>
                        </td>
                        <td>
                            <asp:DropDownList ID="ddlsec" runat="server" Width="50px" Font-Bold="True" Font-Names="Book Antiqua"
                                Font-Size="Medium" AutoPostBack="true" OnSelectedIndexChanged="ddlsec_SelectedIndexChanged">
                            </asp:DropDownList>
                        </td>
                        <td>
                            <asp:UpdatePanel ID="UPGo" runat="server">
                                <contenttemplate>             
                            <asp:Button ID="btngo" runat="server" Text="Go" Font-Bold="True" Font-Names="Book Antiqua"
                                Font-Size="Medium" OnClick="btngo_Click" />
                                </contenttemplate>
                                </asp:UpdatePanel>
        </td>
        <td>
            <asp:RadioButton ID="subname" runat="server" Text="Subject Name" GroupName="Attendance"
                AutoPostBack="true" />
        </td>
        <td>
            <asp:RadioButton ID="subacr" runat="server" Text="Subject Acr" Checked="true" GroupName="Attendance"
                AutoPostBack="true" />
        </td>
        </tr> </table>
        <br />
        <br />
        <br />
        <div class="GridDock" id="divgrid">
            <asp:HiddenField ID="SelectedGridCellIndex" runat="server" Value="-1" />
            <asp:GridView ID="GridView2" runat="server" AutoGenerateColumns="false" Font-Names="Book Antiqua"
                HeaderStyle-BackColor="#0CA6CA" BackColor="White" OnSelectedIndexChanged="GridView1_SelectedIndexChanged">
                <%--OnRowCreated="gridview2_OnRowCreated"--%>
                <Columns>
                    <asp:TemplateField HeaderText="Day">
                        <ItemTemplate>
                            <asp:Label ID="lblDateDisp" runat="server" Text='<%#Eval("DateDisp") %>'></asp:Label>
                            <asp:Label ID="lblDayVal" runat="server" Text='<%#Eval("DateVal") %>' Visible="false"></asp:Label>
                            <asp:Label ID="dayacr" runat="server" Text='<%#Eval("dayacram") %>' Visible="false"></asp:Label>
                        </ItemTemplate>
                        <ItemStyle Width="100px" HorizontalAlign="Center" BackColor="#F8B7B3" />
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Subject Name" Visible="false">
                        <ItemTemplate>
                            <asp:Label ID="lnkPeriod_1" runat="server" Text='<%#Eval("P1Val") %>' ForeColor="Blue"
                                Font-Underline="false"></asp:Label>
                            <asp:Label ID="lblPeriod_1" runat="server" Text='<%#Eval("PVal1") %>' Visible="false"></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Staff Name" Visible="false">
                        <ItemTemplate>
                            <asp:Label ID="lnkStaff_1" runat="server" Text='<%#Eval("s1Val") %>' ForeColor="Blue"
                                Font-Underline="false"></asp:Label>
                            <asp:Label ID="lblStaff_1" runat="server" Text='<%#Eval("sVal1") %>' Visible="false"></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Room" Visible="false">
                        <ItemTemplate>
                            <asp:DropDownList ID="ddlr1" runat="server" Width="90px" CssClass="textbox ddlheight"
                                OnSelectedIndexChanged="ddlr1_SelectedIndexChanged" AutoPostBack="true">
                            </asp:DropDownList>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Subject Name" Visible="false">
                        <ItemTemplate>
                            <asp:Label ID="lnkPeriod_2" runat="server" Text='<%#Eval("P2Val") %>' ForeColor="Blue"
                                Font-Underline="false"></asp:Label>
                            <asp:Label ID="lblPeriod_2" runat="server" Text='<%#Eval("PVal2") %>' Visible="false"></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Staff Name" Visible="false">
                        <ItemTemplate>
                            <asp:Label ID="lnkStaff_2" runat="server" Text='<%#Eval("s2Val") %>' ForeColor="Blue"
                                Font-Underline="false"></asp:Label>
                            <asp:Label ID="lblStaff_2" runat="server" Text='<%#Eval("sVal2") %>' Visible="false"></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Room" Visible="false">
                        <ItemTemplate>
                            <asp:DropDownList ID="ddlr2" runat="server" CssClass="textbox ddlheight" Width="90px"
                                OnSelectedIndexChanged="ddlr2_SelectedIndexChanged" AutoPostBack="true">
                            </asp:DropDownList>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Subject Name" Visible="false">
                        <ItemTemplate>
                            <asp:Label ID="lnkPeriod_3" runat="server" Text='<%#Eval("P3Val") %>' ForeColor="Blue"
                                Font-Underline="false"></asp:Label>
                            <asp:Label ID="lblPeriod_3" runat="server" Text='<%#Eval("PVal3") %>' Visible="false"></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Staff Name" Visible="false">
                        <ItemTemplate>
                            <asp:Label ID="lnkStaff_3" runat="server" Text='<%#Eval("s3Val") %>' ForeColor="Blue"
                                Font-Underline="false"></asp:Label>
                            <asp:Label ID="lblStaff_3" runat="server" Text='<%#Eval("sVal3") %>' Visible="false"></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Room" Visible="false">
                        <ItemTemplate>
                            <asp:DropDownList ID="ddlr3" runat="server" CssClass="textbox ddlheight" Width="90px"
                                OnSelectedIndexChanged="ddlr3_SelectedIndexChanged" AutoPostBack="true">
                            </asp:DropDownList>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Subject Name" Visible="false">
                        <ItemTemplate>
                            <asp:Label ID="lnkPeriod_4" runat="server" Text='<%#Eval("P4Val") %>' ForeColor="Blue"
                                Font-Underline="false"></asp:Label>
                            <asp:Label ID="lblPeriod_4" runat="server" Text='<%#Eval("PVal4") %>' Visible="false"></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Staff Name" Visible="false">
                        <ItemTemplate>
                            <asp:Label ID="lnkStaff_4" runat="server" Text='<%#Eval("s4Val") %>' ForeColor="Blue"
                                Font-Underline="false"></asp:Label>
                            <asp:Label ID="lblStaff_4" runat="server" Text='<%#Eval("sVal4") %>' Visible="false"></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Room" Visible="false">
                        <ItemTemplate>
                            <asp:DropDownList ID="ddlr4" runat="server" CssClass="textbox ddlheight" Width="90px"
                                OnSelectedIndexChanged="ddlr4_SelectedIndexChanged" AutoPostBack="true">
                            </asp:DropDownList>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Subject Name" Visible="false">
                        <ItemTemplate>
                            <asp:Label ID="lnkPeriod_5" runat="server" Text='<%#Eval("P5Val") %>' ForeColor="Blue"
                                Font-Underline="false"></asp:Label>
                            <asp:Label ID="lblPeriod_5" runat="server" Text='<%#Eval("PVal5") %>' Visible="false"></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Staff Name" Visible="false">
                        <ItemTemplate>
                            <asp:Label ID="lnkStaff_5" runat="server" Text='<%#Eval("s5Val") %>' ForeColor="Blue"
                                Font-Underline="false"></asp:Label>
                            <asp:Label ID="lblStaff_5" runat="server" Text='<%#Eval("sVal5") %>' Visible="false"></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Room" Visible="false">
                        <ItemTemplate>
                            <asp:DropDownList ID="ddlr5" runat="server" CssClass="textbox ddlheight" Width="90px"
                                OnSelectedIndexChanged="ddlr5_SelectedIndexChanged" AutoPostBack="true">
                            </asp:DropDownList>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Subject Name" Visible="false">
                        <ItemTemplate>
                            <asp:Label ID="lnkPeriod_6" runat="server" Text='<%#Eval("P6Val") %>' ForeColor="Blue"
                                Font-Underline="false"></asp:Label>
                            <asp:Label ID="lblPeriod_6" runat="server" Text='<%#Eval("PVal6") %>' Visible="false"></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Staff Name" Visible="false">
                        <ItemTemplate>
                            <asp:Label ID="lnkStaff_6" runat="server" Text='<%#Eval("s6Val") %>' ForeColor="Blue"
                                Font-Underline="false"></asp:Label>
                            <asp:Label ID="lblStaff_6" runat="server" Text='<%#Eval("sVal6") %>' Visible="false"></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Room" Visible="false">
                        <ItemTemplate>
                            <asp:DropDownList ID="ddlr6" runat="server" CssClass="textbox ddlheight" Width="90px"
                                OnSelectedIndexChanged="ddlr6_SelectedIndexChanged" AutoPostBack="true">
                            </asp:DropDownList>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Subject Name" Visible="false">
                        <ItemTemplate>
                            <asp:Label ID="lnkPeriod_7" runat="server" Text='<%#Eval("P7Val") %>' ForeColor="Blue"
                                Font-Underline="false"></asp:Label>
                            <asp:Label ID="lblPeriod_7" runat="server" Text='<%#Eval("PVal7") %>' Visible="false"></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Staff Name" Visible="false">
                        <ItemTemplate>
                            <asp:Label ID="lnkStaff_7" runat="server" Text='<%#Eval("s7Val") %>' ForeColor="Blue"
                                Font-Underline="false"></asp:Label>
                            <asp:Label ID="lblStaff_7" runat="server" Text='<%#Eval("sVal7") %>' Visible="false"></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Room" Visible="false">
                        <ItemTemplate>
                            <asp:DropDownList ID="ddlr7" runat="server" CssClass="textbox ddlheight" Width="90px"
                                OnSelectedIndexChanged="ddlr7_SelectedIndexChanged" AutoPostBack="true">
                            </asp:DropDownList>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Subject Name" Visible="false">
                        <ItemTemplate>
                            <asp:Label ID="lnkPeriod_8" runat="server" Text='<%#Eval("P8Val") %>' ForeColor="Blue"
                                Font-Underline="false"></asp:Label>
                            <asp:Label ID="lblPeriod_8" runat="server" Text='<%#Eval("PVal8") %>' Visible="false"></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Staff Name" Visible="false">
                        <ItemTemplate>
                            <asp:Label ID="lnkStaff_8" runat="server" Text='<%#Eval("s8Val") %>' ForeColor="Blue"
                                Font-Underline="false"></asp:Label>
                            <asp:Label ID="lblStaff_8" runat="server" Text='<%#Eval("sVal8") %>' Visible="false"></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Room" Visible="false">
                        <ItemTemplate>
                            <asp:DropDownList ID="ddlr8" runat="server" CssClass="textbox ddlheight" Width="90px"
                                OnSelectedIndexChanged="ddlr8_SelectedIndexChanged" AutoPostBack="true">
                            </asp:DropDownList>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Subject Name" Visible="false">
                        <ItemTemplate>
                            <asp:Label ID="lnkPeriod_9" runat="server" Text='<%#Eval("P9Val") %>' ForeColor="Blue"
                                Font-Underline="false"></asp:Label>
                            <asp:Label ID="lblPeriod_9" runat="server" Text='<%#Eval("PVal9") %>' Visible="false"></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Staff Name" Visible="false">
                        <ItemTemplate>
                            <asp:Label ID="lnkStaff_9" runat="server" Text='<%#Eval("s9Val") %>' ForeColor="Blue"
                                Font-Underline="false"></asp:Label>
                            <asp:Label ID="lblStaff_9" runat="server" Text='<%#Eval("sVal9") %>' Visible="false"></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Room" Visible="false">
                        <ItemTemplate>
                            <asp:DropDownList ID="ddlr9" runat="server" CssClass="textbox ddlheight" Width="90px"
                                OnSelectedIndexChanged="ddlr9_SelectedIndexChanged" AutoPostBack="true">
                            </asp:DropDownList>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Subject Name" Visible="false">
                        <ItemTemplate>
                            <asp:Label ID="lnkPeriod_10" runat="server" Text='<%#Eval("P10Val") %>' ForeColor="Blue"
                                Font-Underline="false"></asp:Label>
                            <asp:Label ID="lblPeriod_10" runat="server" Text='<%#Eval("PVal10") %>' Visible="false"></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Staff Name" Visible="false">
                        <ItemTemplate>
                            <asp:Label ID="lnkStaff_10" runat="server" Text='<%#Eval("s10Val") %>' ForeColor="Blue"
                                Font-Underline="false"></asp:Label>
                            <asp:Label ID="lblStaff_10" runat="server" Text='<%#Eval("sVal10") %>' Visible="false"></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Room" Visible="false">
                        <ItemTemplate>
                            <asp:DropDownList ID="ddlr10" runat="server" CssClass="textbox ddlheight" Width="90px"
                                OnSelectedIndexChanged="ddlr10_SelectedIndexChanged" AutoPostBack="true">
                            </asp:DropDownList>
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
        </div>
        <br />
        <br />
        <center>
        
                            <asp:UpdatePanel ID="Upsave" runat="server">
                                <contenttemplate>   
            <asp:Button runat="server" ID="btnSave" Text="Save" OnClick="btnSave_Click" Visible="false"
                Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium" Width="75px" /></ContentTemplate></asp:UpdatePanel>
        </center>
        <div id="imgdiv2" runat="server" visible="false" style="height: 100%; z-index: 1000;
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
                                    <asp:Label ID="lbl_alerterror" runat="server" Text="" Style="color: Red;" Font-Bold="true"
                                        Font-Size="Medium"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <center>
                                        <asp:Button ID="btn_errorclose" CssClass=" textbox btn1 comm" Style="height: 28px;
                                            width: 65px;" OnClick="btn_errorclose_Click" Text="Yes" runat="server" />
                                        <asp:Button ID="Btncancle" CssClass=" textbox btn1 comm" Style="height: 28px; width: 65px;"
                                            OnClick="Btncancle_Click" Text="No" runat="server" />
                                    </center>
                                </td>
                            </tr>
                        </table>
                    </center>
                </div>
            </center>
        </div>
        </ContentTemplate>
        <%-- <Triggers>
                  <asp:AsyncPostBackTrigger ControlID="GridView2" EventName="gridview2_OnRowCreated" />
                </Triggers>--%>
                 <Triggers>
                  <asp:AsyncPostBackTrigger ControlID="GridView2"  />
                </Triggers>
        </asp:UpdatePanel>
    </center>
    <center>
         <center>
        <asp:UpdateProgress ID="UpdateProgress5" runat="server" AssociatedUpdatePanelID="UPGo">
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
        <asp:ModalPopupExtender ID="ModalPopupExtender5" runat="server" TargetControlID="UpdateProgress5"
            PopupControlID="UpdateProgress5">
        </asp:ModalPopupExtender>

        <asp:UpdateProgress ID="UpdateProgress1" runat="server" AssociatedUpdatePanelID="Upsave">
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
     
    </center>
</asp:Content>

<%@ Page Title="" Language="C#" MasterPageFile="~/AttendanceMOD/AttendanceSubSiteMaster.master"
    AutoEventWireup="true" CodeFile="Syllabus_Entry.aspx.cs" Inherits="Syllabus_Entry" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Src="~/Usercontrols/PrintMaster.ascx" TagName="printmaster" TagPrefix="Insproplus" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <script type="text/javascript">
        function addvalue() {
            document.getElementById('<%=btn_plus.ClientID%>').style.display = 'block';
            document.getElementById('<%=btn_minus.ClientID%>').style.display = 'block';

        }
    </script>
    <style type="text/css">
        .style1
        {
            width: 890px;
            background-color: Teal;
        }
        .style2
        {
            width: 818px;
        }
        .style3
        {
            width: 150px;
        }
        .cursorptr
        {
            cursor: pointer;
        }
    </style>
    <br />
    <center>
        <asp:Label ID="lblhead" runat="server" Text="Syllabus Entry" Font-Bold="True" Font-Names="Book Antiqua"
            Font-Size="Large" ForeColor="Green"></asp:Label></center>
    <br />
    <center>
        <asp:UpdatePanel ID="Upanel1" runat="server">
            <ContentTemplate>
                <table class="maintablestyle" style="width: 700px; height: 44px; background-color: #0CA6CA;">
                    <tr>
                        <td>
                            <asp:Label ID="lblbatch" runat="server" Text="Batch" Font-Bold="True" Font-Names="Book Antiqua"
                                Font-Size="Medium"></asp:Label>
                        </td>
                        <td>
                            <asp:DropDownList ID="ddlbatch" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                Font-Size="Medium" Height="21px" Width="69px" AutoPostBack="True" OnSelectedIndexChanged="ddlbatch_SelectedIndexChanged">
                            </asp:DropDownList>
                        </td>
                        <td>
                            <asp:Label ID="lbldegree" runat="server" Text="Degree" Font-Bold="True" Font-Names="Book Antiqua"
                                Font-Size="Medium"></asp:Label>
                        </td>
                        <td>
                            <asp:DropDownList ID="ddldegree" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                Font-Size="Medium" Height="21px" Width="187px" AutoPostBack="True" OnSelectedIndexChanged="ddldegree_SelectedIndexChanged">
                            </asp:DropDownList>
                        </td>
                        <td>
                            <asp:Label ID="lblbranch" runat="server" Text="Branch" Font-Bold="True" Font-Names="Book Antiqua"
                                Font-Size="Medium"></asp:Label>
                        </td>
                        <td>
                            <asp:DropDownList ID="ddlbranch" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                Font-Size="Medium" Height="21px" Width="187px" AutoPostBack="True" OnSelectedIndexChanged="ddlbranch_SelectedIndexChanged">
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label ID="lblsem" runat="server" Text="Sem" Font-Bold="True" Font-Names="Book Antiqua"
                                Font-Size="Medium"></asp:Label>
                        </td>
                        <td>
                            <asp:DropDownList ID="ddlsem" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                Font-Size="Medium" Height="21px" Width="55px" AutoPostBack="True" OnSelectedIndexChanged="ddlsem_SelectedIndexChanged">
                            </asp:DropDownList>
                        </td>
                        <td>
                            <asp:Label ID="lblsubject" runat="server" Text="Subject" Font-Bold="True" Font-Names="Book Antiqua"
                                Font-Size="Medium"></asp:Label>
                        </td>
                        <td>
                            <asp:DropDownList ID="ddlsubject" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                Font-Size="Medium" Height="21px" Width="187px" AutoPostBack="True" OnSelectedIndexChanged="ddlsubject_SelectedIndexChanged">
                            </asp:DropDownList>
                        </td>
                        <td colspan="2">
                            <asp:UpdatePanel ID="UPGo" runat="server">
                                <ContentTemplate>
                                    <asp:LinkButton ID="HpsyllabusCopy" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                        Font-Size="Medium" PostBackUrl="~/AttendanceMOD/syllabusCopy.aspx">Syllabus Copy</asp:LinkButton>
                                    <asp:Button ID="btngo" runat="server" Text="GO" Font-Bold="true" Font-Size="Medium"
                                        Font-Names="Book Antiqua" OnClick="GO_Click" />
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </td>
                    </tr>
                </table>
            </ContentTemplate>
        </asp:UpdatePanel>
    </center>
    <br />
    <center>
        <asp:UpdatePanel ID="UpdatePanel4" runat="server">
            <ContentTemplate>
                <asp:Panel ID="panel_content" runat="server" Visible="false" BorderStyle="Solid"
                    BorderWidth="1px">
                    <center>
                        <br />
                        <%--<asp:Panel ID="subject_details" runat="server">--%>
                        <asp:UpdatePanel ID="Upanel2" runat="server">
                            <ContentTemplate>
                                <table class="style2" style="border-bottom-style: solid; border-top-style: solid;
                                    border-width: 1px; border-right-style: solid; border-left-style: solid;">
                                    <tr>
                                        <td class="style3" style="border-bottom-style: solid; border-top-style: solid; border-width: 1px;
                                            border-left-style: solid;">
                                            <asp:Label ID="lbl_subjectcode" runat="server" Text="Subjct Code" Font-Names="Book Antiqua"
                                                Font-Size="Medium"></asp:Label>
                                        </td>
                                        <td class="style3" style="border-bottom-style: solid; border-top-style: solid; border-width: 1px;
                                            border-right-style: solid;">
                                            <asp:Label ID="lbl_subjectcode_display" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                                Font-Size="Medium" ForeColor="Black"></asp:Label>
                                        </td>
                                        <td class="style3" style="border-bottom-style: solid; border-top-style: solid; border-width: 1px;
                                            border-left-style: solid;">
                                            <asp:Label ID="lblmin_intmark" runat="server" Text="Min.Int.Marks" Font-Names="Book Antiqua"
                                                Font-Size="Medium"></asp:Label>
                                        </td>
                                        <td class="style3" style="border-bottom-style: solid; border-top-style: solid; border-width: 1px;
                                            border-right-style: solid;">
                                            <asp:Label ID="lbl_min_intmark_display" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                                Font-Size="Medium" ForeColor="Black"></asp:Label>
                                        </td>
                                        <td class="style3" style="border-bottom-style: solid; border-top-style: solid; border-width: 1px;
                                            border-left-style: solid;">
                                            <asp:Label ID="lblmaxintmark" runat="server" Text="Max.Int.Marks" Font-Names="Book Antiqua"
                                                Font-Size="Medium"></asp:Label>
                                        </td>
                                        <td class="style3" style="border-bottom-style: solid; border-top-style: solid; border-width: 1px;
                                            border-right-style: solid;">
                                            <asp:Label ID="lbl_max_intmark_display" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                                Font-Size="Medium" ForeColor="Black"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="style3" style="border-bottom-style: solid; border-top-style: solid; border-width: 1px;
                                            border-left-style: solid;">
                                            <asp:Label ID="lbl_min_ext_mark" runat="server" Text="Min.Ext.Marks" Font-Names="Book Antiqua"
                                                Font-Size="Medium"></asp:Label>
                                        </td>
                                        <td class="style3" style="border-bottom-style: solid; border-top-style: solid; border-width: 1px;
                                            border-right-style: solid;">
                                            <asp:Label ID="lbl_min_extmark_display" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                                Font-Size="Medium" ForeColor="Black"></asp:Label>
                                        </td>
                                        <td class="style3" style="border-bottom-style: solid; border-top-style: solid; border-width: 1px;
                                            border-left-style: solid;">
                                            <asp:Label ID="lbl_max_ext_mark" runat="server" Text="Max.Ext.Marks" Font-Names="Book Antiqua"
                                                Font-Size="Medium"></asp:Label>
                                        </td>
                                        <td class="style3" style="border-bottom-style: solid; border-top-style: solid; border-width: 1px;
                                            border-right-style: solid;">
                                            <asp:Label ID="lbl_max_extmark_display" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                                Font-Size="Medium" ForeColor="Black"></asp:Label>
                                        </td>
                                        <td class="style3" style="border-bottom-style: solid; border-top-style: solid; border-width: 1px;
                                            border-left-style: solid;">
                                            <asp:Label ID="lbl_total_min_mark" runat="server" Text="Total Marks(Min)" Font-Names="Book Antiqua"
                                                Font-Size="Medium"></asp:Label>
                                        </td>
                                        <td class="style3" style="border-bottom-style: solid; border-top-style: solid; border-width: 1px;
                                            border-right-style: solid;">
                                            <asp:Label ID="lbl_total_minmark_display" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                                Font-Size="Medium" ForeColor="Black"></asp:Label>
                                        </td>
                                    </tr>
                                </table>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                        <br />
                        <%------------------%>
                        <asp:UpdatePanel ID="UpdatePanel3" runat="server">
                            <ContentTemplate>
                                <table>
                                    <tr>
                                        <td align="right">
                                            <asp:Label ID="lblall" runat="server" Font-Bold="true" Text="Type" Font-Names="Book Antiqua"
                                                Font-Size="Medium"></asp:Label>
                                        </td>
                                        <td align="center">
                                        </td>
                                        <td align="center">
                                            <asp:DropDownList ID="ddl_all" runat="server" Width="100px" OnSelectedIndexChanged="ddl_all_SelectedIndexChanged"
                                                AutoPostBack="true">
                                                <asp:ListItem Value="AID" Text="Special Teaching"></asp:ListItem>
                                                <asp:ListItem Value="METH" Text="Methodology"></asp:ListItem>
                                                <asp:ListItem Value="INST" Text="Instructional Medium"></asp:ListItem>
                                            </asp:DropDownList>
                                        </td>
                                        <td align="center">
                                        </td>
                                        <td align="center">
                                            <asp:Button ID="btn_plus" runat="server" Text="+" Width="25px" Style="display: none;"
                                                OnClick="btn_plus_Click" />
                                        </td>
                                        <td align="center">
                                            <asp:DropDownList ID="ddl_selectValue" runat="server" Width="100px">
                                            </asp:DropDownList>
                                        </td>
                                        <td align="center">
                                            <asp:Button ID="btn_minus" runat="server" Text="-" Style="display: none;" Width="25px"
                                                OnClick="btn_minus_Click" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                        </td>
                                        <td>
                                        </td>
                                        <td>
                                        </td>
                                        <td>
                                        </td>
                                        <td align="center">
                                        </td>
                                        <td align="center">
                                            <asp:Panel ID="panel_addvalue" runat="server" Width="150px" Visible="false" BorderStyle="Solid"
                                                BorderWidth="1px" BackColor="#CCCCCC">
                                                <asp:TextBox ID="txt_enter_val" runat="server"></asp:TextBox>
                                                <asp:Button ID="btn_addvalue" runat="server" Text="Add" OnClick="btn_addvalue_Click" />
                                                <asp:Button ID="btnexit" runat="server" Text="Cancel" OnClick="btnexit_Click" />
                                            </asp:Panel>
                                        </td>
                                        <td align="center">
                                        </td>
                                    </tr>
                                </table>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                        <%------------------%>
                </asp:Panel>
            </ContentTemplate>
        </asp:UpdatePanel>
        <br />
        <asp:UpdatePanel ID="Upanel3" runat="server">
            <ContentTemplate>
                <table id="table_tree" runat="server" style="border-bottom-style: solid; border-top-style: solid;
                    border-width: 1px; border-right-style: solid; border-left-style: solid;">
                    <tr>
                        <td>
                            <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                                <ContentTemplate>
                                    <asp:Panel ID="panel_tree" runat="server" ScrollBars="Vertical" Style="width: 536px;
                                        background-color: White; height: 350px;">
                                        <asp:TreeView ID="TreeView1" runat="server" OnTreeNodeCheckChanged="TreeView1_TreeNodeCheckChanged"
                                            ViewStateMode="Enabled" HoverNodeStyle-BackColor="LightBlue" SelectedNodeStyle-ForeColor="Red"
                                            ShowLines="true" OnSelectedNodeChanged="TreeView1_SelectedNodeChanged" AutoPostBack="true">
                                        </asp:TreeView>
                                    </asp:Panel>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </td>
                    </tr>
                </table>
            </ContentTemplate>
        </asp:UpdatePanel>
        <br />
        <br />
        <asp:UpdatePanel ID="updatepanel_1" runat="server">
            <ContentTemplate>
                <div id="newrowdiv" runat="server">
                    <table>
                        <tr>
                            <td>
                                <asp:Label ID="lbl_add_row_type" runat="server" Text="Add Row Type in Grid" Font-Bold="true"
                                    Font-Size="Medium" Font-Names="Book Antiqua"></asp:Label>
                                <asp:DropDownList ID="ddl_addrow_type" runat="server" Font-Size="Medium" Font-Bold="true"
                                    Font-Names="Book Antiqua">
                                    <asp:ListItem Value="NewRow">New Row</asp:ListItem>
                                    <%--<asp:ListItem Value="Above">Above</asp:ListItem>
                                        <asp:ListItem Value="Below">Below</asp:ListItem>--%>
                                </asp:DropDownList>
                                <asp:Button ID="Btn_AddNewRow" runat="server" Text="Add" Font-Bold="true" Font-Size="Medium"
                                    Font-Names="Book Antiqua" OnClick="Btn_AddNewRow_Click" />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <table>
                                    <tr>
                                        <td>
                                            <asp:UpdatePanel ID="up1" runat="server">
                                                <ContentTemplate>
                                                    <asp:GridView runat="server" ID="gview" AutoGenerateColumns="false" OnRowDataBound="gview_OnDataBound"
                                                        BorderStyle="Double" CssClass="grid-view" GridLines="Both" Font-Names="Book Antique"
                                                        ShowFooter="false">
                                                        <Columns>
                                                            <asp:TemplateField HeaderText="S.No">
                                                                <ItemTemplate>
                                                                    <center>
                                                                        <asp:Label runat="server" ID="lblsno" Text='<%#Container.DataItemIndex+1 %>' /></center>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Topic" ControlStyle-Width="350">
                                                                <ItemTemplate>
                                                                    <asp:TextBox ID="txt_Topic" runat="server" CssClass="textbox txtheight" Text='<%#Eval("Topic") %>'
                                                                        Height="15px" Width="100px" Style="text-align: left; width: auto;"></asp:TextBox>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Top_tag" Visible="false">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lbltag" runat="server" CssClass="textbox txtheight" Text='<%#Eval("toptag") %>' />
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Description" ControlStyle-Width="250">
                                                                <ItemTemplate>
                                                                    <asp:TextBox ID="txt_Desc" runat="server" CssClass="textbox txtheight" Text='<%#Eval("Desc") %>'
                                                                        Height="15px" Width="100px" Style="text-align: left; width: auto;"></asp:TextBox>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Total No.of Hours" HeaderStyle-HorizontalAlign="center"
                                                                ControlStyle-Width="60">
                                                                <ItemTemplate>
                                                                    <asp:TextBox ID="txt_Hours" runat="server" CssClass="textbox txtheight" Text='<%#Eval("Hours") %>'
                                                                        Height="15px" Style="text-align: center; width: auto"></asp:TextBox>
                                                                    <asp:FilteredTextBoxExtender ID="filterextenderTotHours" runat="server" TargetControlID="txt_Hours"
                                                                        FilterType="Numbers" ValidChars=".">
                                                                    </asp:FilteredTextBoxExtender>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Special Teaching" ControlStyle-Width="90">
                                                                <ItemTemplate>
                                                                    <asp:DropDownList ID="ddlteach" runat="server" CssClass="textbox1 ddlheight1" Style="width: auto;">
                                                                    </asp:DropDownList>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Methodology" ControlStyle-Width="70">
                                                                <ItemTemplate>
                                                                    <asp:DropDownList ID="ddlmethod" runat="server" CssClass="textbox1 ddlheight1" Style="width: auto;">
                                                                    </asp:DropDownList>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Instructional Medium" ControlStyle-Width="170">
                                                                <ItemTemplate>
                                                                    <asp:DropDownList ID="ddlmedium" runat="server" CssClass="textbox1 ddlheight1">
                                                                    </asp:DropDownList>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Reference Book" ControlStyle-Width="150">
                                                                <ItemTemplate>
                                                                    <asp:TextBox ID="txt_Refbook" runat="server" CssClass="textbox txtheight" Text='<%#Eval("Refbook") %>'
                                                                        Height="15px" Width="100px" Style="text-align: left;"></asp:TextBox></center>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Course Outcome" ControlStyle-Width="170">
                                                                <ItemTemplate>
                                                                    <asp:DropDownList ID="ddlcourseout" runat="server" CssClass="textbox1 ddlheight1">
                                                                    </asp:DropDownList>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                        </Columns>
                                                        <HeaderStyle BackColor="#0CA6CA" Font-Bold="true" ForeColor="Black" Font-Size="Large" />
                                                        <FooterStyle BackColor="White" ForeColor="#333333" />
                                                        <PagerStyle BackColor="#336666" HorizontalAlign="Center" />
                                                        <RowStyle ForeColor="#333333" />
                                                        <SelectedRowStyle BackColor="#339966" Font-Bold="True" />
                                                    </asp:GridView>
                                                </ContentTemplate>
                                            </asp:UpdatePanel>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                                                <ContentTemplate>
                                                    <asp:Button ID="Btn_Save" runat="server" Text="Save" Style="float: left;" Font-Bold="true"
                                                        Font-Size="Medium" Font-Names="Book Antiqua" OnClick="Btn_Save_Click" />
                                                    <asp:Button ID="Btn_Cancel" runat="server" Text="Clear" Style="float: left;" Font-Bold="true"
                                                        Font-Size="Medium" Font-Names="Book Antiqua" OnClick="Btn_Cancel_Click" />
                                                    <asp:Button ID="Btn_delete" runat="server" Text="Delete" Style="float: left;" Font-Bold="true"
                                                        Font-Size="Medium" Font-Names="Book Antiqua" OnClick="Btn_Delete_Click" />
                                                    <asp:Label ID="lblsavevalidate" runat="server" ForeColor="Red" Font-Bold="true" Font-Names="Book Antiqua"
                                                        Font-Size="Medium"></asp:Label>
                                                </ContentTemplate>
                                            </asp:UpdatePanel>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                    </table>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
    </center>
    </asp:Panel> </center>
    <center>
        <asp:UpdateProgress ID="UpdateProgress1" runat="server" AssociatedUpdatePanelID="UPGo">
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
        <asp:ModalPopupExtender ID="ModalPopupExtender5" runat="server" TargetControlID="UpdateProgress1"
            PopupControlID="UpdateProgress1">
        </asp:ModalPopupExtender>
    </center>
</asp:Content>

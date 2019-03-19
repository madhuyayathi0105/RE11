<%@ Page Title="" Language="C#" MasterPageFile="~/AttendanceMOD/AttendanceSubSiteMaster.master"
    AutoEventWireup="true" CodeFile="syllabusCopy.aspx.cs" Inherits="syllabusCopy" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <style type="text/css">
        .ModalPopupBG
        {
            background-color: #666699;
            filter: alpha(opacity=50);
            opacity: 0.7;
        }
        .HellowWorldPopup
        {
            min-width: 100px;
            min-height: 50px;
            background: white;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <center>
        <br />
        <div>
            <asp:Label ID="Label2" runat="server" Text="Syllabus Copy" ForeColor="Green" CssClass="fontstyleheader"></asp:Label>
        </div>
    </center>
    <br />
    <br />
    <div style="top: 135px; position: absolute; border-bottom-style: solid; border-top-style: solid;
        border-left-style: solid; border-right-style: solid; border-width: 1px;">
        <table>
            <tr>
                <td style="border-bottom-style: solid; border-top-style: solid; border-left-style: solid;
                    background-color: lightblue; border-width: 1px;">
                    <asp:Label ID="lblbatch" runat="server" Text="Batch" Font-Bold="True" Font-Names="Book Antiqua"
                        Font-Size="Medium"></asp:Label>
                </td>
                <td style="border-bottom-style: solid; border-top-style: solid; background-color: #E6E6FA;
                    border-width: 1px; border-right-style: solid;">
                    <asp:DropDownList ID="ddlbatch" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                        Font-Size="Medium" Height="21px" Width="69px" AutoPostBack="True" OnSelectedIndexChanged="ddlbatch_SelectedIndexChanged">
                    </asp:DropDownList>
                </td>
                <td style="border-bottom-style: solid; border-top-style: solid; border-left-style: solid;
                    background-color: lightblue; border-width: 1px;">
                    <asp:Label ID="lbldegree" runat="server" Text="Degree" Font-Bold="True" Font-Names="Book Antiqua"
                        Font-Size="Medium"></asp:Label>
                </td>
                <td style="border-bottom-style: solid; border-top-style: solid; background-color: #E6E6FA;
                    width: 80px; border-width: 1px; border-right-style: solid;">
                    <asp:DropDownList ID="ddldegree" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                        Font-Size="Medium" Height="21px" Width="80px" AutoPostBack="True" OnSelectedIndexChanged="ddldegree_SelectedIndexChanged">
                    </asp:DropDownList>
                </td>
                <td style="border-bottom-style: solid; border-top-style: solid; border-left-style: solid;
                    background-color: lightblue; border-width: 1px;">
                    <asp:Label ID="lblbranch" runat="server" Text="Branch" Font-Bold="True" Font-Names="Book Antiqua"
                        Font-Size="Medium"></asp:Label>
                </td>
                <td style="border-bottom-style: solid; border-top-style: solid; background-color: #E6E6FA;
                    border-width: 1px; border-right-style: solid;">
                    <asp:DropDownList ID="ddlbranch" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                        Font-Size="Medium" Height="21px" Width="187px" AutoPostBack="True" OnSelectedIndexChanged="ddlbranch_SelectedIndexChanged">
                    </asp:DropDownList>
                </td>
            </tr>
        </table>
        <table>
            <tr>
                <td style="border-bottom-style: solid; border-top-style: solid; border-left-style: solid;
                    background-color: lightblue; border-width: 1px;">
                    <asp:Label ID="lblsem" runat="server" Text="Sem" Font-Bold="True" Font-Names="Book Antiqua"
                        Font-Size="Medium"></asp:Label>
                </td>
                <td style="border-bottom-style: solid; border-top-style: solid; background-color: #E6E6FA;
                    border-width: 1px; border-right-style: solid;">
                    <asp:DropDownList ID="ddlsem" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                        Font-Size="Medium" Height="21px" Width="55px" AutoPostBack="True" OnSelectedIndexChanged="ddlsem_SelectedIndexChanged">
                    </asp:DropDownList>
                </td>
                <td style="border-bottom-style: solid; border-top-style: solid; border-left-style: solid;
                    background-color: lightblue; border-width: 1px;">
                    <asp:Label ID="lblsubject" runat="server" Text="Subject" Font-Bold="True" Font-Names="Book Antiqua"
                        Font-Size="Medium"></asp:Label>
                </td>
                <td style="border-bottom-style: solid; border-top-style: solid; background-color: #E6E6FA;
                    border-width: 1px; border-right-style: solid;">
                    <asp:DropDownList ID="ddlsubject" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                        Font-Size="Medium" Height="21px" Width="187px" AutoPostBack="True" OnSelectedIndexChanged="ddlsubject_SelectedIndexChanged">
                    </asp:DropDownList>
                </td>
                <td>
                    <asp:Button ID="btngo" runat="server" Text="GO" Font-Bold="true" Font-Size="Medium"
                        Font-Names="Book Antiqua" OnClick="GO_Click" />
                </td>
            </tr>
        </table>
        <table style="border-bottom-style: solid; border-top-style: solid; border-width: 1px;
            border-right-style: solid; border-left-style: solid;">
            <tr>
                <td>
                    <asp:Label ID="lblmoveerror" runat="server" Font-Bold="true" Font-Names="Book Antiqua"
                        Font-Size="Medium" Visible="false" ForeColor="Red">
                    </asp:Label>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Panel ID="panel_tree" runat="server" ScrollBars="Vertical" Style="width: 536px;
                        background-color: White; height: 350px;">
                        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                            <ContentTemplate>
                                <asp:TreeView ID="TreeView1" runat="server" OnTreeNodeCheckChanged="TreeView1_TreeNodeCheckChanged"
                                    ViewStateMode="Enabled" HoverNodeStyle-BackColor="LightBlue" SelectedNodeStyle-ForeColor="Red"
                                    ShowLines="true" OnSelectedNodeChanged="TreeView1_SelectedNodeChanged" AutoPostBack="true">
                                </asp:TreeView>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </asp:Panel>
                </td>
            </tr>
        </table>
    </div>
    <div style="top: 400px; position: absolute; left: 570px;">
        <table>
            <tr>
                <td>
                    <asp:Button ID="btnmove" runat="server" Text="=>" Font-Bold="true" Font-Size="Medium"
                        Font-Names="Book Antiqua" OnClick="btnmove_click" />
                </td>
            </tr>
        </table>
    </div>
    <div style="top: 135px; position: absolute; left: 521px; border-bottom-style: solid;
        border-top-style: solid; border-left-style: solid; border-right-style: solid;
        border-width: 1px;">
        <table>
            <tr>
                <td style="border-bottom-style: solid; border-top-style: solid; border-left-style: solid;
                    background-color: lightblue; border-width: 1px;">
                    <asp:Label ID="lblToBatch" runat="server" Text="Batch" Font-Bold="True" ForeColor="Black"
                        Font-Size="Medium" Font-Names="Book Antiqua"></asp:Label>
                </td>
                <td style="border-bottom-style: solid; border-top-style: solid; background-color: #E6E6FA;
                    border-width: 1px; border-right-style: solid;">
                    <asp:DropDownList ID="toddlbatch" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                        Font-Size="Medium" Height="21px" Width="69px" AutoPostBack="True" OnSelectedIndexChanged="toddlbatch_SelectedIndexChanged">
                    </asp:DropDownList>
                </td>
                <td style="border-bottom-style: solid; border-top-style: solid; border-left-style: solid;
                    background-color: lightblue; border-width: 1px;">
                    <asp:Label ID="lbltodegree" runat="server" Text="Degree" Font-Bold="True" ForeColor="Black"
                        Font-Names="Book Antiqua" Font-Size="Medium"></asp:Label>
                </td>
                <td style="border-bottom-style: solid; border-top-style: solid; background-color: #E6E6FA;
                    border-width: 1px; border-right-style: solid;">
                    <asp:DropDownList ID="toddldegree" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                        Font-Size="Medium" Height="21px" Width="80px" AutoPostBack="True" OnSelectedIndexChanged="toddldegree_SelectedIndexChanged">
                    </asp:DropDownList>
                </td>
                <td style="border-bottom-style: solid; border-top-style: solid; border-left-style: solid;
                    background-color: lightblue; border-width: 1px;">
                    <asp:Label ID="lblTobranch" runat="server" Text="Branch" Font-Bold="True" ForeColor="Black"
                        Font-Names="Book Antiqua" Font-Size="Medium"></asp:Label>
                </td>
                <td style="border-bottom-style: solid; border-top-style: solid; background-color: #E6E6FA;
                    border-width: 1px; border-right-style: solid;">
                    <asp:DropDownList ID="toddlbranch" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                        Font-Size="Medium" Height="21px" Width="187px" AutoPostBack="True" OnSelectedIndexChanged="toddlbranch_SelectedIndexChanged">
                    </asp:DropDownList>
                </td>
            </tr>
        </table>
        <table>
            <tr>
                <td style="border-bottom-style: solid; border-top-style: solid; border-left-style: solid;
                    background-color: lightblue; border-width: 1px;">
                    <asp:Label ID="lbltosem" runat="server" Text="Sem" Font-Bold="True" ForeColor="Black"
                        Font-Names="Book Antiqua" Font-Size="Medium"></asp:Label>
                </td>
                <td style="border-bottom-style: solid; border-top-style: solid; background-color: #E6E6FA;
                    border-width: 1px; border-right-style: solid;">
                    <asp:DropDownList ID="toddlsem" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                        Font-Size="Medium" Height="21px" Width="55px" AutoPostBack="True" OnSelectedIndexChanged="toddlsem_SelectedIndexChanged">
                    </asp:DropDownList>
                </td>
                <td style="border-bottom-style: solid; border-top-style: solid; border-left-style: solid;
                    background-color: lightblue; border-width: 1px;">
                    <asp:Label ID="lbldescsubject" runat="server" Text="Subject" Font-Bold="True" ForeColor="Black"
                        Font-Names="Book Antiqua" Font-Size="Medium"></asp:Label>
                </td>
                <td style="border-bottom-style: solid; border-top-style: solid; background-color: #E6E6FA;
                    border-width: 1px; border-right-style: solid;">
                    <asp:DropDownList ID="toddlsubject" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                        Font-Size="Medium" Height="21px" Width="187px" AutoPostBack="True" OnSelectedIndexChanged="toddlsubject_SelectedIndexChanged">
                    </asp:DropDownList>
                </td>
            </tr>
        </table>
        <table style="border-bottom-style: solid; border-top-style: solid; border-width: 1px;
            border-right-style: solid; border-left-style: solid;">
            <tr>
                <td>
                    <asp:Label ID="lblerrormsg" runat="server" Font-Bold="true" Font-Names="Book Antiqua"
                        Font-Size="Medium" Visible="false" ForeColor="Red">
                    </asp:Label>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Panel ID="paneltodesc" runat="server" ScrollBars="Vertical" Style="width: 536px;
                        background-color: White; height: 350px;">
                        <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                            <ContentTemplate>
                                <asp:TreeView ID="TreeView2" runat="server" OnTreeNodeCheckChanged="TreeView2_TreeNodeCheckChanged"
                                    ViewStateMode="Enabled" HoverNodeStyle-BackColor="LightBlue" SelectedNodeStyle-ForeColor="Red"
                                    ShowLines="true" OnSelectedNodeChanged="TreeView2_SelectedNodeChanged" AutoPostBack="true">
                                </asp:TreeView>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </asp:Panel>
                </td>
            </tr>
        </table>
    </div>
    <div style="position: absolute; top: 640px; left: 568px;">
        <center>
            <asp:Button ID="btnsave" runat="server" Text="Save" OnClick="btnsave_click" Font-Bold="true"
                Visible="false" Font-Names="Book Antiqua" Font-Size="Medium" />
        </center>
    </div>
    <asp:ModalPopupExtender ID="ModalPopupExtender2" Drag="True" TargetControlID="hfphoto"
        PopupControlID="pnlVerifysave" runat="server" BackgroundCssClass="ModalPopupBG"
        DynamicServicePath="" Enabled="True">
    </asp:ModalPopupExtender>
    <asp:HiddenField ID="hfphoto" runat="server" />
    <asp:Panel ID="pnlVerifysave" runat="server" BorderColor="Black" BorderStyle="Double"
        Style="display: none; height: 200; width: 50;">
        <div class="HellowWorldPopup">
            <table width="100%">
                <tr>
                    <td>
                        <asp:Label ID="infoid" runat="server" Text="Subject Already Contains Syllabus.Do You Want to Delete it?"
                            Font-Bold="true" Font-Names="Book Antiqua" Font-Size="Medium"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td align="center">
                        <asp:Button ID="btnOk" runat="server" Text="Yes" OnClick="btnOk_Click" Font-Bold="True"
                            Font-Names="Book Antiqua" Font-Size="Medium" />
                        <asp:Button ID="btnCancel" runat="server" Text="No" OnClick="btnCancel_Click" Font-Bold="True"
                            Font-Names="Book Antiqua" Font-Size="Medium" />
                    </td>
                </tr>
            </table>
        </div>
    </asp:Panel>
</asp:Content>

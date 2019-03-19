<%@ Page Title="Hostel Master Settings" Language="C#" MasterPageFile="~/AdmissionMod/AttendanceSubSiteMaster.master"
    AutoEventWireup="true" CodeFile="HostelMasterSettings.aspx.cs" Inherits="OfficeMOD_HostelAllotmentToBatchaspx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <link href="../Styles/css/Commoncss.css" rel="Stylesheet" type="text/css" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <center>
        <span id="spHeader" class="fontstyleheader" style="margin: 0px; margin-bottom: 10px;
            margin-top: 10px; position: relative; color: Green; font-weight: bold;">Hostel Master
            Settings</span>
        <div class="maindivstyle" style="width: 950px; height: auto; margin: 0px; margin-top: 15px;
            margin-bottom: 15px; padding: 8px;">
            <table class="maintablestyle" style="margin: 0px; margin-top: 15px; margin-bottom: 15px;
                padding: 8px;">
                <tr>
                    <td>
                        <asp:Label ID="lblCollege" Style="font-family: 'Book Antiqua'; font-weight: bold;
                            font-size: medium;" runat="server" Text="Institution"></asp:Label>
                    </td>
                    <td>
                        <asp:DropDownList ID="ddlCollege" runat="server" CssClass="textbox1" Style="font-family: 'Book Antiqua';
                            font-weight: bold; font-size: medium;" Width="250px" AutoPostBack="True" OnSelectedIndexChanged="ddlCollege_SelectedIndexChanged">
                        </asp:DropDownList>
                    </td>
                    <td>
                        <asp:Label ID="lblBuilding" Style="font-family: 'Book Antiqua'; font-weight: bold;
                            font-size: medium;" runat="server" Text="Building Name"></asp:Label>
                    </td>
                    <td>
                        <asp:DropDownList ID="ddlBuildName" runat="server" CssClass="textbox1" Style="font-family: 'Book Antiqua';
                            font-weight: bold; font-size: medium; width: 95px;" AutoPostBack="True" OnSelectedIndexChanged="ddlBuildName_SelectedIndexChanged">
                        </asp:DropDownList>
                    </td>
                    <td>
                        <asp:Label ID="lblFloorName" Style="font-family: 'Book Antiqua'; font-weight: bold;
                            font-size: medium;" runat="server" Text="Floor Name"></asp:Label>
                    </td>
                    <td>
                        <asp:DropDownList ID="ddlFloorName" runat="server" CssClass="textbox1" Style="font-family: 'Book Antiqua';
                            font-weight: bold; font-size: medium; width: 95px;" AutoPostBack="True" OnSelectedIndexChanged="ddlFloorName_SelectedIndexChanged">
                        </asp:DropDownList>
                    </td>
                    <td>
                        <asp:Button ID="btnShowRooms" runat="server" Text="Show Rooms" Style="width: auto;
                            height: auto; font-family: 'Book Antiqua'; font-weight: bold; font-size: medium;"
                            CssClass="textbox btn2" OnClick="btnShowRooms_Click" />
                    </td>
                </tr>
            </table>
            <asp:Label ID="lblErrSearch" runat="server" Text="" ForeColor="Red" Visible="False"
                Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium" Style="margin: 0px;
                margin-bottom: 10px; margin-top: 10px; position: relative;"></asp:Label>
            <center>
                <div id="divMainContent" visible="false" runat="server" style="margin: 0px; margin-bottom: 20px;
                    margin-top: 20px;">
                    <div id="divSetBatch" runat="server" style="margin: 0px; margin-bottom: 10px; margin-top: 10px;">
                        <table>
                            <tr>
                                <td>
                                    <asp:Label ID="lblBatch" Style="font-family: 'Book Antiqua'; font-weight: bold; font-size: medium;"
                                        runat="server" Text="Batch Year"></asp:Label>
                                </td>
                                <td>
                                    <asp:DropDownList ID="ddlBatch" runat="server" CssClass="textbox1" Style="font-family: 'Book Antiqua';
                                        font-weight: bold; font-size: medium;" AutoPostBack="True" OnSelectedIndexChanged="ddlBatch_SelectedIndexChanged">
                                    </asp:DropDownList>
                                </td>
                                <td>
                                    <asp:Button ID="btnSet" CssClass="textbox textbox1" Visible="true" runat="server"
                                        Style="width: auto; height: auto; padding: 5px; font-family: 'Book Antiqua';
                                        font-weight: bold; font-size: medium;" Text="Allocate" OnClick="btnSet_Click" />
                                </td>
                                <td>
                                    <asp:CheckBox ID="chkSelectAllRooms" runat="server" Text="Select All" Checked="false"
                                        AutoPostBack="true" Style="width: auto; height: auto; padding: 3px; font-family: 'Book Antiqua';
                                        font-weight: bold;" OnCheckedChanged="chkSelectAllRooms_CheckedChanged" />
                                </td>
                            </tr>
                        </table>
                        <table>
                            <tr>
                                <td style="padding: 10px; margin: 0px; margin-right: 4px; background-color: Green;">
                                </td>
                                <td style="margin: 0px; padding: 2px; margin-left: 3px; margin-right: 3px;">
                                    Not Yet Allocate
                                </td>
                                <td style="padding: 10px; margin: 0px; margin-right: 4px; margin-left: 4px; background-color: Red">
                                </td>
                                <td style="margin: 0px; padding: 2px; margin-left: 3px; margin-right: 3px;">
                                    Allocated To Selected Year
                                </td>
                                <td style="padding: 10px; margin: 0px; margin-right: 4px; margin-left: 4px; background-color: Blue;">
                                </td>
                                <td style="margin: 0px; padding: 2px; margin-left: 3px; margin-right: 3px;">
                                    Allocated To Selected Other Year
                                </td>
                            </tr>
                        </table>
                    </div>
                    <div id="divRoomsDetails" runat="server" style="margin: 0px; margin-bottom: 10px;
                        margin-top: 10px;">
                        <table>
                            <tr>
                                <td colspan="3" align="center">
                                    <asp:DataList ID="dlRoomDetails" runat="server" Font-Size="Medium" RepeatColumns="5"
                                        Width="900px" ForeColor="#333333" OnItemDataBound="dlRoomDetails_ItemDataBound">
                                        <AlternatingItemStyle BackColor="White" />
                                        <FooterStyle BackColor="#1C5E55" Font-Bold="True" ForeColor="White" />
                                        <HeaderStyle BackColor="#1C5E55" Font-Bold="True" ForeColor="White" />
                                        <ItemStyle BackColor="#E3EAEB" BorderWidth="1px" Height="0px" />
                                        <ItemTemplate>
                                            <table>
                                                <tr>
                                                    <td>
                                                        <asp:CheckBox ID="chkRoomChecked" runat="server" Checked="false" />
                                                    </td>
                                                    <td>
                                                        <asp:Label ID="lblRoomName" ForeColor="Green" runat="server" Text='<%# Eval("Room_Name") %>'></asp:Label>
                                                        <asp:Label ID="lblRoomId" Visible="false" ForeColor="Green" runat="server" Text='<%# Eval("Roompk") %>'></asp:Label>
                                                        <asp:Label ID="lblCheckedBatch" ForeColor="Green" runat="server" Text='<%# Eval("batchYear") %>'
                                                            Visible="false"></asp:Label>
                                                    </td>
                                                </tr>
                                            </table>
                                        </ItemTemplate>
                                        <SelectedItemStyle BackColor="#C5BBAF" Font-Bold="True" ForeColor="#333333" />
                                    </asp:DataList>
                                </td>
                            </tr>
                            <%--<tr>
                            <td colspan="3" align="center">
                                <asp:Button ID="btnSave" CssClass="textbox textbox1" Visible="true" runat="server"
                                    Style="width: auto; height: auto; font-family: 'Book Antiqua'; font-weight: bold;"
                                    Text="Save" OnClick="btnSave_Click" />
                            </td>
                        </tr>--%>
                        </table>
                    </div>
                </div>
            </center>
        </div>
    </center>
    <center>
        <div id="divPopupAlert" runat="server" visible="false" style="height: 100em; z-index: 1000;
            width: 100%; background-color: rgba(54, 25, 25, .2); position: absolute; top: 0%;
            left: 0%; right: 0%;">
            <center>
                <div id="divAlertContent" runat="server" class="table" style="background-color: White;
                    height: 120px; width: 238px; border: 5px solid #0CA6CA; border-top: 25px solid #0CA6CA;
                    margin-top: 200px; border-radius: 10px;">
                    <center>
                        <table style="height: 100px; width: 100%;">
                            <tr>
                                <td align="center">
                                    <asp:Label ID="lblAlertMsg" runat="server" Text="" Style="color: Red;" Font-Bold="true"
                                        Font-Size="Medium" Font-Names="Book Antiqua"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td align="center">
                                    <center>
                                        <asp:Button ID="btnPopAlertClose" runat="server" CssClass=" textbox btn2 comm" Font-Size="Medium"
                                            Font-Bold="True" Font-Names="Book Antiqua" Style="height: auto; width: auto;"
                                            OnClick="btnPopAlertClose_Click" Text="Ok" />
                                    </center>
                                </td>
                            </tr>
                        </table>
                    </center>
                </div>
            </center>
        </div>
    </center>
</asp:Content>

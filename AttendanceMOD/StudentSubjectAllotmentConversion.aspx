<%@ Page Title="" Language="C#" MasterPageFile="~/AttendanceMOD/AttendanceSubSiteMaster.master"
    AutoEventWireup="true" CodeFile="StudentSubjectAllotmentConversion.aspx.cs" Inherits="StudentSubjectAllotmentConversion"
    EnableEventValidation="false" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Assembly="FarPoint.Web.Spread" Namespace="FarPoint.Web.Spread" TagPrefix="FarPoint" %>
<%@ Register Src="~/Usercontrols/PrintMaster.ascx" TagName="PRINTPDF" TagPrefix="Insproplus" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <link href="~/Styles/css/Registration.css" rel="stylesheet" type="text/css" />
    <script src="//ajax.googleapis.com/ajax/libs/jquery/1.11.1/jquery.min.js"></script>
    <link href="~/Styles/css/Commoncss.css" rel="Stylesheet" type="text/css" />
    <style type="text/css" media="screen">
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
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <center>
        <span class="fontstyleheader" style="color: Green; margin-bottom: 15px; margin-top: 15px;">
            Student Subject Allotment Conversion</span>
    </center>
    <center>
        <div class="maintablestyle" style="width: auto; margin-bottom: 15px; margin-top: 15px;">
            <table>
                <tr>
                    <table>
                        <tr>
                            <td>
                                <asp:Label ID="lblCollege" runat="server" Text="College" Font-Bold="true" Font-Names="Book Antiqua"
                                    Font-Size="Medium"></asp:Label>
                            </td>
                            <td>
                                <asp:DropDownList ID="ddlCollege" runat="server" Font-Names="Book Antiqua" Font-Size="Medium"
                                    Font-Bold="true" Width="190px" AutoPostBack="True" OnSelectedIndexChanged="ddlCollege_SelectedIndexChanged">
                                </asp:DropDownList>
                            </td>
                            <td>
                                <asp:Label runat="server" ID="lblbatch" Text="Batch" Font-Bold="True" Font-Names="Book Antiqua"
                                    Font-Size="Medium"></asp:Label>
                            </td>
                            <td>
                                <asp:DropDownList ID="ddlbatch" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                    Font-Size="Medium" OnSelectedIndexChanged="ddlbatch_SelectedIndexChanged" Height="25px"
                                    Width="60px" AutoPostBack="True">
                                </asp:DropDownList>
                            </td>
                            <td>
                                <asp:Label runat="server" ID="lbldegree" Text="Degree" Font-Bold="True" Font-Names="Book Antiqua"
                                    Font-Size="Medium"></asp:Label>
                            </td>
                            <td>
                                <asp:DropDownList runat="server" ID="ddldegree" Height="24px" Width="81px" AutoPostBack="True"
                                    Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium" OnSelectedIndexChanged="ddldegree_SelectedIndexChanged">
                                </asp:DropDownList>
                            </td>
                            <td>
                                <asp:Label runat="server" ID="lblbranch" Text="Branch" Font-Bold="True" Font-Names="Book Antiqua"
                                    Font-Size="Medium"></asp:Label>
                            </td>
                            <td>
                                <asp:DropDownList runat="server" ID="ddlbranch" Font-Bold="True" Height="25px" Width="168px"
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
                                    Font-Size="Medium" AutoPostBack="True" Height="25px" Width="47px" OnSelectedIndexChanged="ddlduration_SelectedIndexChanged">
                                </asp:DropDownList>
                            </td>
                            <td>
                                <asp:Label runat="server" ID="lblsec" Text="Sec" Font-Bold="True" Font-Names="Book Antiqua"
                                    Font-Size="Medium"></asp:Label>
                            </td>
                            <td>
                                <asp:DropDownList runat="server" ID="ddlsec" Font-Bold="True" Font-Names="Book Antiqua"
                                    Font-Size="Medium" Height="25px" Width="49px" AutoPostBack="True" OnSelectedIndexChanged="ddlsec_SelectedIndexChanged">
                                </asp:DropDownList>
                            </td>
                    </table>
                </tr>
                <%--  <table>--%>
                <tr>
                    <%-- </table>
            <table>--%>
                    <td>
                        <asp:Label runat="server" ID="lblSubType" Text="Subject Type" Font-Bold="True" Font-Names="Book Antiqua"
                            Font-Size="Medium"></asp:Label>
                    </td>
                    <td>
                        <asp:DropDownList runat="server" ID="ddlSubType" Font-Bold="True" Font-Names="Book Antiqua"
                            Font-Size="Medium" Height="25px" Width="122px" OnSelectedIndexChanged="ddlSubType_SelectedIndexChanged"
                            AutoPostBack="True">
                        </asp:DropDownList>
                    </td>
                    <td>
                        <asp:Label ID="lblsubject" runat="server" Text="Previous Sem Subject" Font-Bold="True"
                            Font-Names="Book Antiqua" Font-Size="Medium"></asp:Label>
                    </td>
                    <td>
                        <asp:DropDownList ID="ddlsubject" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                            Font-Size="Medium" Width="150px" OnSelectedIndexChanged="ddlsubject_SelectedIndexChanged"
                            AutoPostBack="True">
                        </asp:DropDownList>
                    </td>
                    <td>
                        <asp:Button ID="btnGo" runat="server" Text="Go" OnClick="btnGo_Click" Font-Bold="True"
                            Font-Names="Book Antiqua" Font-Size="Medium" Height="26px" />
                    </td>
                    <td>
                        <asp:Label ID="lblSubTypeNew" runat="server" Text="New Subject Type" Font-Bold="True"
                            Font-Names="Book Antiqua" Font-Size="Medium"></asp:Label>
                    </td>
                    <td>
                        <asp:DropDownList ID="ddlNewSubType" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                            Font-Size="Medium" Width="210px" OnSelectedIndexChanged="ddlNewSubType_SelectedIndexChanged"
                            AutoPostBack="True">
                        </asp:DropDownList>
                    </td>
                    <td>
                        <asp:Label ID="lblhours" runat="server" Text="New Subject" Font-Bold="True"
                            Font-Names="Book Antiqua" Font-Size="Medium"></asp:Label>
                    </td>
                    <td>
                        <asp:DropDownList ID="ddlNewSub" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                            Font-Size="Medium" Width="210px" OnSelectedIndexChanged="ddlmovingsubject_SelectedIndexChanged"
                            AutoPostBack="True">
                        </asp:DropDownList>
                    </td>
                    <td>
                        <asp:Button ID="btnmove" runat="server" Text="Move" OnClick="btnMove_Click" Font-Bold="True"
                            Font-Names="Book Antiqua" Font-Size="Medium" Height="26px" Width="62px" />
                    </td>
                </tr>
                <%--  </table>--%>
            </table>
        </div>
        <br />
        <asp:Label ID="errmsg" runat="server" ForeColor="Red" Font-Bold="True" Font-Names="Book Antiqua"
            Font-Size="Medium" Height="16px"></asp:Label>
    </center>
    <center>
        <asp:GridView ID="GridView1" runat="server" Style="margin-bottom: 15px; margin-top: 15px;
            width: auto;" Font-Names="Times New Roman" AutoGenerateColumns="false">
            <Columns>
                <asp:TemplateField HeaderText="S.No">
                    <ItemTemplate>
                        <%#Container.DataItemIndex+1 %>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:BoundField ItemStyle-Width="120px" DataField="roll_no" HeaderText="Roll No" />
                <asp:BoundField ItemStyle-Width="120px" DataField="reg_no" HeaderText="Reg No" />
                <asp:BoundField ItemStyle-Width="120px" DataField="Roll_Admit" HeaderText="Admission No" />
                <asp:BoundField ItemStyle-Width="190px" DataField="Stud_Name" HeaderText="Student Name" />
                <asp:BoundField ItemStyle-Width="110px" DataField="Stud_Type" HeaderText="Student Type" />
            </Columns>
            <HeaderStyle BackColor="#009999" ForeColor="White" />
        </asp:GridView>
    </center>
    <%-- Alert Box --%>
    <center>
        <div id="divPopAlert" runat="server" visible="false" style="height: 550em; z-index: 2000;
            width: 100%; background-color: rgba(54, 25, 25, .2); position: absolute; top: 0%;
            left: 0%;">
            <center>
                <div id="divPopAlertContent" runat="server" class="table" style="background-color: White;
                    height: 120px; width: 23%; border: 5px solid #0CA6CA; border-top: 25px solid #0CA6CA;
                    left: 39%; right: 39%; top: 35%; padding: 5px; position: fixed; border-radius: 10px;">
                    <center>
                        <table style="height: 100px; width: 100%; padding: 5px;">
                            <tr>
                                <td align="center">
                                    <asp:Label ID="lblAlertMsg" runat="server" Style="color: Red;" Font-Bold="true" Font-Size="Medium"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <center>
                                        <asp:Button ID="btnPopAlertClose" Font-Bold="true" Font-Size="Medium" Font-Names="Book Antiqua"
                                            CssClass="textbox textbox1" Style="height: auto; width: auto;" OnClick="btnPopAlertClose_Click"
                                            Text="Ok" runat="server" />
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

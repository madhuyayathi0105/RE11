<%@ Page Title="Special Hours Entry" Language="C#" MasterPageFile="~/AttendanceMOD/AttendanceSubSiteMaster.master"
    AutoEventWireup="true" CodeFile="StudentEntryForSpecialClass.aspx.cs" Inherits="StudentEntryForSpecialClass" %>

<%@ Register Assembly="FarPoint.Web.Spread" Namespace="FarPoint.Web.Spread" TagPrefix="FarPoint" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Src="~/Usercontrols/PrintMaster.ascx" TagName="PRINTPDF" TagPrefix="Insproplus" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <style type="text/css">
        .floats
        {
            float: right;
        }
        .cpHeader
        {
            color: white;
            background-color: #719DDB;
            font-size: 12px;
            cursor: pointer;
            padding: 4px;
            font-style: normal;
            font-variant: normal;
            font-weight: bold;
            line-height: normal;
            font-family: "auto Trebuchet MS" , Verdana;
        }
        .cpBody
        {
            background-color: transparent;
            font: normal 11px auto Verdana, Arial;
            border: 1px gray;
            padding-top: 7px;
            padding-left: 4px;
            padding-right: 4px;
            padding-bottom: 4px;
        }
        .cpimage
        {
            float: right;
            vertical-align: middle;
            background-color: transparent;
        }
        .cur
        {
            cursor: pointer;
        }
        .cursorptr
        {
        }
        .txt
        {
        }
        .style111
        {
            width: 102px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <center>
        <span class="fontstyleheader" style="color: Green; margin: 0px; margin-bottom: 10px;
            margin-top: 10px;">Special Hour Student Selection</span>
    </center>
    <asp:Panel ID="Panel1" runat="server">
        <center>
            <div>
                <table class="maintablestyle" style="width: auto; margin: 0px; margin-bottom: 10px;
                    margin-top: 10px; text-align: left;">
                    <tr>
                        <td>
                            <asp:Label ID="lblCollege" runat="server" Text="College" Font-Bold="true" Font-Names="Book Antiqua"
                                Font-Size="Medium"></asp:Label>
                        </td>
                        <td>
                            <asp:DropDownList ID="ddlCollege" runat="server" Font-Names="Book Antiqua" Font-Size="Medium"
                                Font-Bold="true" Width="250px" AutoPostBack="True" OnSelectedIndexChanged="ddlCollege_SelectedIndexChanged">
                            </asp:DropDownList>
                        </td>
                        <td>
                            <asp:Label ID="lblbatch" runat="server" Text="Batch" Style="font-family: 'Baskerville Old Face';
                                font-weight: 700;" Font-Names="Book Antiqua" Font-Size="Medium" Height="16px"></asp:Label>
                        </td>
                        <td>
                            <asp:DropDownList ID="ddlbatch" CssClass="cursorptr" runat="server" AutoPostBack="True"
                                OnSelectedIndexChanged="ddlbatch_SelectedIndexChanged" Font-Names="Book Antiqua"
                                Font-Size="Medium" Font-Bold="True" Height="25px">
                            </asp:DropDownList>
                        </td>
                        <td>
                            <asp:Label ID="lbldegree" runat="server" Text="Degree" Style="font-family: 'Baskerville Old Face';
                                font-weight: 700;" Font-Names="Book Antiqua" Font-Size="Medium" Height="16px"></asp:Label>
                        </td>
                        <td>
                            <asp:DropDownList ID="ddldegree" CssClass="cursorptr" runat="server" AutoPostBack="true"
                                OnSelectedIndexChanged="ddldegree_SelectedIndexChanged" Width="100px" Font-Names="Book Antiqua"
                                Font-Size="Medium" Font-Bold="True" Height="25px">
                            </asp:DropDownList>
                        </td>
                        <td>
                            <asp:Label ID="lblbranch" runat="server" Text="Branch" Style="font-family: 'Baskerville Old Face';
                                font-weight: 700;" Font-Names="Book Antiqua" Font-Size="Medium"></asp:Label>
                        </td>
                        <td>
                            <asp:DropDownList ID="ddlbranch" CssClass="cursorptr" runat="server" OnSelectedIndexChanged="ddlbranch_SelectedIndexChanged"
                                AutoPostBack="True" Height="25px" Width="191px" Font-Names="Book Antiqua" Font-Size="Medium"
                                Font-Bold="True">
                            </asp:DropDownList>
                        </td>
                        <td>
                            <asp:Label ID="lblsem" runat="server" Text="Sem" Style="font-family: 'Baskerville Old Face';
                                font-weight: 700;" Font-Names="Book Antiqua" Font-Size="Medium" Height="16px"></asp:Label>
                        </td>
                        <td>
                            <asp:DropDownList ID="ddlsem" CssClass="cursorptr" runat="server" OnSelectedIndexChanged="ddlsem_SelectedIndexChanged"
                                Width="80px" AutoPostBack="True" Height="25px" Font-Names="Book Antiqua" Font-Size="Medium"
                                Font-Bold="True">
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="10">
                            <table style="margin-left: 0px;">
                                <tr>
                                    <td>
                                        <asp:Label ID="lblsec" runat="server" Text="Sec" Style="font-family: 'Baskerville Old Face';
                                            font-weight: 700;" Font-Names="Book Antiqua" Font-Size="Medium" Height="16px"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="ddlsec" CssClass="cursorptr" runat="server" AutoPostBack="True"
                                            OnSelectedIndexChanged="ddlsec_SelectedIndexChanged" Height="25px" Width="81px"
                                            Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium">
                                        </asp:DropDownList>
                                    </td>
                                    <td>
                                        <asp:Label ID="lbl_subj_select" runat="server" Text="Subject" Font-Names="Book Antiqua"
                                            Font-Bold="True" Font-Size="Medium"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="ddl_select_subj" runat="server" Font-Names="Book Antiqua" Font-Size="Medium"
                                            Font-Bold="True" AutoPostBack="True" Height="22px" Width="172px" OnSelectedIndexChanged="ddl_select_subj_SelectedIndexChanged">
                                        </asp:DropDownList>
                                    </td>
                                    <td>
                                        <asp:Label ID="lblspecialhour" runat="server" Text="Date" Font-Names="Book Antiqua"
                                            Font-Bold="True" Font-Size="Medium"></asp:Label>
                                        <asp:DropDownList ID="ddlspecialhour" runat="server" Font-Names="Book Antiqua" Font-Size="Medium"
                                            Font-Bold="True" AutoPostBack="True" Height="22px" Width="120px" OnSelectedIndexChanged="ddlspecialhour_SelectedIndexChanged">
                                        </asp:DropDownList>
                                    </td>
                                    <td>
                                        <asp:Label ID="lblStaffList" runat="server" Text="Staff" Font-Names="Book Antiqua"
                                            Font-Bold="True" Font-Size="Medium"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="ddlStaffList" runat="server" Font-Names="Book Antiqua" Font-Size="Medium"
                                            Font-Bold="True" AutoPostBack="True" Width="152px" OnSelectedIndexChanged="ddlStaffList_SelectedIndexChanged">
                                        </asp:DropDownList>
                                    </td>
                                    <td>
                                        <asp:Label ID="lbltym" runat="server" Text="Hour" Font-Names="Book Antiqua" Font-Bold="True"
                                            Font-Size="Medium"></asp:Label>
                                        <asp:DropDownList ID="ddlspecialhourtiem" runat="server" Font-Names="Book Antiqua"
                                            Font-Size="Medium" Font-Bold="True" AutoPostBack="True" OnSelectedIndexChanged="ddlspecialhourtiem_SelectedIndexChanged"
                                            Height="22px" Width="120px">
                                        </asp:DropDownList>
                                    </td>
                                    <td>
                                        <asp:Button ID="Btngo" runat="server" OnClick="Btngo_Click" CssClass="cursorptr"
                                            Style="font-weight: 700; top: 273px; left: 385px;" Text="GO" Width="56px" Enabled="false" />
                                    </td>
                                    <td>
                                        &nbsp;
                                    </td>
                                    <td>
                                        &nbsp;
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2">
                                        <asp:Label ID="lblfromdate" runat="server" Font-Bold="true" Font-Names="Book Antiqua"
                                            Font-Size="Small" ForeColor="Red"></asp:Label>
                                    </td>
                                    <td colspan="2">
                                        <asp:Label ID="lbltodate" runat="server" ForeColor="Red" Font-Names="Book Antiqua"
                                            Font-Size="Small" Font-Bold="true"></asp:Label>
                                    </td>
                                    <td colspan="3">
                                        <asp:Label ID="datelbl" runat="server" ForeColor="Red" Font-Names="Book Antiqua"
                                            Font-Size="Small" Font-Bold="true"></asp:Label>
                                        &nbsp;&nbsp;&nbsp;
                                        <asp:Label ID="lblErrMsg" runat="server" Font-Bold="true" Font-Names="Book Antiqua"
                                            Font-Size="Small" ForeColor="Red"></asp:Label>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                </table>
            </div>
        </center>
        
        <center>
            <div id="divgrid" visible="false" runat="server">
                <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="false" Font-Names="Book Antiqua">
                    <Columns>
                        <asp:TemplateField HeaderText="S.No">
                            <ItemTemplate>
                                <asp:Label ID="lblSno" runat="server" Text='<%#Container.DataItemIndex+1 %>' Visible="true"></asp:Label>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="40px" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Select">
                            <HeaderTemplate>
                                <asp:CheckBox ID="chkSelectAll" runat="server" AutoPostBack="true" OnCheckedChanged="SelectAll_Checked" />
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:CheckBox ID="gridcb" runat="server" />
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="60px" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Roll No">
                            <ItemTemplate>
                                <asp:Label ID="lblgridrollno" runat="server" Text='<%# Bind("Roll_No") %>'></asp:Label>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="130px" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Reg No">
                            <ItemTemplate>
                                <asp:Label ID="lblgridregno" runat="server" Text='<%# Bind("Reg_No") %>'></asp:Label>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="130px" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Application Number" Visible="false">
                            <ItemTemplate>
                                <asp:Label ID="lblgridapplicationno" runat="server" Text='<%# Bind("App_No") %>'></asp:Label>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Admission Number">
                            <ItemTemplate>
                                <asp:Label ID="lblgridadmissionno" runat="server" Text='<%# Bind("Roll_Admit") %>'></asp:Label>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="130px" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Name">
                            <ItemTemplate>
                                <asp:Label ID="Lblgridname" runat="server" Text='<%# Bind("Stud_Name") %>'></asp:Label>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" Width="250px" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Batch Year" Visible="false">
                            <ItemTemplate>
                                <asp:Label ID="Lblgridbatchyear" runat="server" Text='<%# Bind("Batch_Year") %>'></asp:Label>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="80px" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Degree" Visible="false">
                            <ItemTemplate>
                                <asp:Label ID="Lblgriddegree" runat="server" Text='<%# Bind("degree_code") %>'></asp:Label>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="80px" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Section" Visible="false">
                            <ItemTemplate>
                                <asp:Label ID="Lblgridsection" runat="server" Text='<%# Bind("Sections") %>'></asp:Label>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="80px" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Semester" Visible="false">
                            <ItemTemplate>
                                <asp:Label ID="Lblgridsemester" runat="server" Text='<%# Bind("Current_Semester") %>'></asp:Label>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="100px" />
                        </asp:TemplateField>
                        <%-- <asp:TemplateField HeaderText="Serial No">
                            <ItemTemplate>
                                <asp:Label ID="lblgridserialno" runat="server" Text='<%# Bind("serialno") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>--%>
                        <asp:TemplateField HeaderText="Student Type">
                            <ItemTemplate>
                                <asp:Label ID="lblgridstudenttype" runat="server" Text='<%# Bind("Stud_Type") %>'></asp:Label>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="130px" />
                        </asp:TemplateField>
                    </Columns>
                    <FooterStyle BackColor="#CCCC99" />
                    <HeaderStyle BackColor="#6B696B" Font-Bold="True" ForeColor="White" />
                    <PagerStyle BackColor="#F7F7DE" ForeColor="Black" HorizontalAlign="Right" />
                    <RowStyle BackColor="#F7F7DE" />
                    <SelectedRowStyle BackColor="#CE5D5A" Font-Bold="True" ForeColor="White" />
                    <SortedAscendingCellStyle BackColor="#FBFBF2" />
                    <SortedAscendingHeaderStyle BackColor="#848384" />
                    <SortedDescendingCellStyle BackColor="#EAEAD3" />
                    <SortedDescendingHeaderStyle BackColor="#575357" />
                </asp:GridView>
                <br />
                <br />
                <center>
         <table>
                    <tr id="select_range" runat="server" visible="false">
                        <td>
                            <asp:Label ID="Label13" runat="server" Text="Range :" Style="margin-left: 16px;"></asp:Label>
                        </td>
                        <td>
                        </td>
                        <td>
                            <asp:Label ID="Label14" runat="server" Text="From"></asp:Label>
                            <asp:TextBox ID="txt_frange" CssClass="textbox textbox1 txtheight" runat="server"
                                MaxLength="4"></asp:TextBox>
                            <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender4" runat="server" TargetControlID="txt_frange"
                                FilterType="Numbers" ValidChars="/">
                            </asp:FilteredTextBoxExtender>
                        </td>
                        <td>
                            <asp:Label ID="Label15" runat="server" Text="To"></asp:Label>
                            <asp:TextBox ID="txt_trange" CssClass="textbox textbox1 txtheight" runat="server"
                                MaxLength="4"></asp:TextBox>
                            <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender2" runat="server" TargetControlID="txt_trange"
                                FilterType="Numbers" ValidChars="/">
                            </asp:FilteredTextBoxExtender>
                        </td>
                        <td>
                            <asp:Button ID="Btn_range" runat="server" Text="Select" OnClick="Btn_range_Click"
                                CssClass="textbox1 textbox btn2" />
                        </td>
                        <td>
                            <label id="lbl_pagecnt" runat="server" style="background-color: Green; margin-left: 2px;">
                            </label>
                        </td>
                        <td>
                            <label id="lbl_totrecord" runat="server" style="background-color: Green;">
                            </label>
                        </td>
                    </tr>
                </table></center>
                   <br />
                <br />
                <table>
                    <tr>
                        <td>
                            <asp:Label ID="lblset" runat="server" Visible="False" Style="font-family: 'Baskerville Old Face';
                                font-weight: 700; height: auto; width: auto;" Font-Bold="False" Font-Size="Medium"
                                ForeColor="Red"></asp:Label>
                        </td>
                        <td>
                            <asp:Button ID="btnsave" runat="server" OnClick="Btnsave_Click" CssClass="cursorptr"
                                Style="font-weight: 700; top: 273px; left: 385px;" Text="Save" Width="56px" />
                        </td>
                    </tr>
                </table>
            </div>
        </center>
    </asp:Panel>
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

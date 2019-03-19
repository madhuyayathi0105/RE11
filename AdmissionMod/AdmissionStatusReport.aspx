<%@ Page Title="" Language="C#" MasterPageFile="~/AdmissionMod/AttendanceSubSiteMaster.master"
    AutoEventWireup="true" CodeFile="AdmissionStatusReport.aspx.cs" Inherits="AdmissionMod_AdmissionStatusReport" %>

<%@ Register Assembly="FarPoint.Web.Spread,  Version=5.0.3520.2008, Culture=neutral, PublicKeyToken=327c3516b1b18457"
    Namespace="FarPoint.Web.Spread" TagPrefix="FarPoint" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <style type="text/css">
        .maindivstylesize
        {
            height: auto;
            width: 970px;
        }
        .lbl
        {
            text-align: center;
        }
        .container
        {
            width: 98%;
        }
        .col1
        {
            float: left;
            width: 49%;
        }
        .col2
        {
            float: right;
            width: 49%;
        }
        .newtextbox
        {
            border: 1px solid #c4c4c4;
            padding: 4px 4px 4px 4px;
            border-radius: 4px;
            -moz-border-radius: 4px;
            -webkit-border-radius: 4px;
            box-shadow: 0px 0px 8px #d9d9d9;
            -moz-box-shadow: 0px 0px 8px #d9d9d9;
            -webkit-box-shadow: 0px 0px 8px #d9d9d9;
        }
        
        .textboxshadow:hover
        {
            outline: none;
            border: 1px solid #BAFAB8;
            box-shadow: 0px 0px 8px #BAFAB8;
            -moz-box-shadow: 0px 0px 8px #BAFAB8;
            -webkit-box-shadow: 0px 0px 8px #BAFAB8;
        }
        .textboxchng
        {
            border: 1px solid #c4c4c4;
            padding: 4px 4px 4px 4px;
            border-radius: 4px;
            -moz-border-radius: 4px;
            -webkit-border-radius: 4px;
            box-shadow: 0px 0px 8px #d9d9d9;
            -moz-box-shadow: 0px 0px 8px #d9d9d9;
            -webkit-box-shadow: 0px 0px 8px #d9d9d9;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">
    <div>
        <asp:ScriptManager ID="ScriptManager1" runat="server">
        </asp:ScriptManager>
    </div>
    <center>
        <div>
            <span class="fontstyleheader" style="color: #008000; font-size: xx-large">Admission
                Status Report</span>
        </div>
        <br />
        <div class="maindivstyle maindivstylesize">
            <table class="maintablestyle" width="800px">
                <tr>
                    <td>
                        <asp:Label ID="lbl_clgname" Width="100px" runat="server" Text="Institution"></asp:Label>
                    </td>
                    <td>
                        <asp:DropDownList ID="ddlcollege" CssClass="ddlheight4 textbox1" runat="server" AutoPostBack="true"
                            OnSelectedIndexChanged="ddlcollege_SelectedIndexChanged">
                        </asp:DropDownList>
                    </td>
                    <td>
                        <asp:Label ID="lbl_graduation" Width="100px" runat="server" Text="Graduation"></asp:Label>
                    </td>
                    <td>
                        <asp:DropDownList ID="ddl_graduation" CssClass="ddlheight2  textbox1" runat="server"
                            AutoPostBack="true" OnSelectedIndexChanged="ddl_graduation_SelectedIndexChanged">
                        </asp:DropDownList>
                    </td>
                    <td>
                        <asp:Label ID="lbl_batch" Width="80px" runat="server" Text="Batch"></asp:Label>
                    </td>
                    <td>
                        <asp:DropDownList ID="ddl_batch" CssClass="ddlheight textbox1" runat="server" AutoPostBack="true"
                            OnSelectedIndexChanged="ddl_batch_SelectedIndexChanged">
                        </asp:DropDownList>
                    </td>
                    <td>
                        <asp:Label ID="lbl_degree" Text="Degree" runat="server"></asp:Label>
                    </td>
                    <td>
                        <asp:DropDownList ID="ddl_degree" runat="server" CssClass="ddlheight4 textbox1" AutoPostBack="true"
                            OnSelectedIndexChanged="ddl_degree_Selectedindexchange">
                        </asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="lbl_branch" Text="Branch" runat="server"></asp:Label>
                    </td>
                    <td>
                        <asp:UpdatePanel ID="UpdatePanel23" runat="server">
                            <contenttemplate>
                                                <asp:TextBox ID="txt_branch" runat="server" CssClass="textbox textbox1 txtheight3"
                                                    ReadOnly="true">-- Select--</asp:TextBox>
                                                <asp:Panel ID="p4" runat="server" CssClass="multxtpanel" Height="250px" Style="position: absolute;">
                                                    <asp:CheckBox ID="cb_branch" runat="server" Text="Select All" AutoPostBack="true"
                                                        OnCheckedChanged="cb_branch_checkedchange" />
                                                    <asp:CheckBoxList ID="cbl_branch" runat="server" AutoPostBack="true" OnSelectedIndexChanged="cbl_branch_SelectedIndexChanged">
                                                    </asp:CheckBoxList>
                                                </asp:Panel>
                                                <asp:PopupControlExtender ID="PopupControlExtender19" runat="server" TargetControlID="txt_branch"
                                                    PopupControlID="p4" Position="Bottom">
                                                </asp:PopupControlExtender>
                                            </contenttemplate>
                        </asp:UpdatePanel>
                    </td>
                    <td>
                        <asp:Label ID="lbl_org_sem" Text="Semester" runat="server"></asp:Label>
                    </td>
                    <td>
                        <asp:DropDownList ID="ddlsem" runat="server" CssClass="ddlheight textbox1">
                        </asp:DropDownList>
                    </td>
                    <td>
                        <asp:Label ID="lblFromDate" Style="font-family: 'Book Antiqua'; font-size: medium;"
                            runat="server" Text="From"></asp:Label>
                    </td>
                    <td>
                        <asp:TextBox ID="txtFromDate" CssClass="textbox textbox1" runat="server" Width="80px"></asp:TextBox>
                        <asp:CalendarExtender ID="calExtFromDate" runat="server" Format="dd/MM/yyyy" TargetControlID="txtFromDate">
                        </asp:CalendarExtender>
                    </td>
                    <td>
                        <asp:Label ID="lblToDate" Style="font-family: 'Book Antiqua'; font-size: medium;"
                            runat="server" Text="To"></asp:Label>
                    </td>
                    <td>
                        <asp:TextBox ID="txtToDate" runat="server" CssClass="textbox textbox1" Width="80px"></asp:TextBox>
                        <asp:CalendarExtender ID="calExtToDate" runat="server" Format="dd/MM/yyyy" TargetControlID="txtToDate">
                        </asp:CalendarExtender>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="lblStream" Style="font-family: 'Book Antiqua'; font-size: medium;"
                            runat="server" Text="Stream"></asp:Label>
                    </td>
                    <td>
                        <asp:DropDownList ID="ddlStream" runat="server" CssClass="textbox ddlheight1" Style="font-family: 'Book Antiqua';
                            font-size: medium; width: 90px;">
                        </asp:DropDownList>
                    </td>
                    <td>
                        <asp:Label ID="lblSession" Style="font-family: 'Book Antiqua'; font-size: medium;"
                            runat="server" Text="Session"></asp:Label>
                    </td>
                    <td>
                        <asp:DropDownList ID="ddlSession" runat="server" CssClass="textbox ddlheight3" Style="font-family: 'Book Antiqua';
                            font-size: medium;">
                        </asp:DropDownList>
                    </td>
                    <td colspan="4">
                        <asp:Label ID="lblReportType" Style="font-family: 'Book Antiqua'; font-size: medium;"
                            runat="server" Text="Report Type"></asp:Label>
                        <asp:DropDownList ID="ddlReportType" runat="server" CssClass="textbox ddlheight4"
                            Width="235px" Style="font-family: 'Book Antiqua'; font-size: medium;" AutoPostBack="True"
                            OnSelectedIndexChanged="ddlReportType_SelectedIndexChanged">
                            <asp:ListItem Text="Admitted"></asp:ListItem>
                            <%-- <asp:ListItem Text="Hostel registered"></asp:ListItem>
                            <asp:ListItem Text="Transport registered"></asp:ListItem>--%>
                        </asp:DropDownList>
                        <asp:Button ID="btnGo" CssClass="textbox btn" runat="server" Style="width: auto;
                            height: auto; font-family: 'Book Antiqua'; font-size: medium;" Text="Go" OnClick="btnGo_Click" />
                    </td>
                </tr>
            </table>
            <br />
            <div>
               
                <FarPoint:FpSpread ID="FpSpread1" runat="server" Visible="false" Width="950px" ActiveSheetViewIndex="0"
                    ShowHeaderSelection="false">
                    <Sheets>
                        <FarPoint:SheetView SheetName="Sheet1" AutoPostBack="true">
                        </FarPoint:SheetView>
                    </Sheets>
                </FarPoint:FpSpread>
            </div>
            <br />
            <div id="div_report" runat="server" visible="false">
                <center>
                    <asp:Label ID="lbl_reportname" runat="server" Text="Report Name" Font-Names="Book Antiqua"
                        Font-Size="Medium"></asp:Label>
                    <asp:TextBox ID="txt_excelname" runat="server" CssClass="textbox textbox1 txtheight5"
                        onkeypress="return ClearPrint1()"></asp:TextBox>
                    <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender56" runat="server" TargetControlID="txt_excelname"
                        FilterType="LowercaseLetters,UppercaseLetters,Numbers,Custom" ValidChars="!@$%^&()_+}{][';,.">
                    </asp:FilteredTextBoxExtender>
                    <asp:Button ID="btn_Excel" runat="server" Text="Export To Excel" Width="150px" CssClass="textbox textbox1 btn2"
                        AutoPostBack="true" Font-Names="Book Antiqua" OnClick="btnExcel_Click" />
                    <asp:Button ID="btn_printmaster" Font-Names="Book Antiqua" runat="server" Text="Print"
                        CssClass="textbox textbox1 btn2" AutoPostBack="true" OnClick="btn_printmaster_Click" />
                    <Insproplus:printmaster runat="server" ID="Printcontrol" Visible="false" />
                </center>
            </div>
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
                                                <asp:Button ID="btnPopAlertClose" runat="server" CssClass=" textbox btn2" Width="40px"
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
        </div>
    </center>
</asp:Content>

<%@ Page Title="Student Special Hour Entry" Language="C#" MasterPageFile="~/AttendanceMOD/AttendanceSubSiteMaster.master"
    AutoEventWireup="true" CodeFile="Student_special_Hour_Entry.aspx.cs" Inherits="Student_special_Hour_Entry" %>

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
            margin-top: 10px;">Special Hour Master Setting</span>
    </center>
    <asp:Panel ID="Panel1" runat="server">
        <center>
            <div class="maintablestyle" style="width: 900px; margin: 0px; margin-bottom: 10px;
                margin-top: 10px; text-align: left;">
                <table>
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
                        <td class="style64">
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
                    </tr>
                </table>
                <table style="margin-left: 0px;">
                    <tr>
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
                            <asp:Label ID="lblfrom" runat="server" Text="Special Hour Conducted On" Style="font-family: 'Baskerville Old Face';
                                font-weight: 700;" Width="230px" Font-Names="Book Antiqua" Font-Size="Medium"
                                Height="16px"></asp:Label>
                        </td>
                        <td class="style111">
                            <asp:TextBox ID="txtFromDate" CssClass="txt" runat="server" Height="19px" Width="90px"
                                Font-Names="Book Antiqua" Font-Size="Medium" OnTextChanged="txtFromDate_TextChanged"
                                Font-Bold="True" AutoPostBack="True"></asp:TextBox>
                        </td>
                        <asp:CalendarExtender ID="CalendarExtender2" TargetControlID="txtFromDate" runat="server"
                            Format="dd-MM-yyyy">
                        </asp:CalendarExtender>
                        <%-- <td>
                            <asp:Label ID="lblTo" runat="server" Text="To Date" Style="font-family: 'Baskerville Old Face';
                                font-weight: 700;" Width="66px" Font-Names="Book Antiqua" Font-Size="Medium"
                                Height="16px"></asp:Label>
                        </td>
                        <td>
                            <asp:TextBox ID="TxtToDate" runat="server" Height="19px" Width="90px" Style="top: 281px;
                                left: 289px" Font-Names="Book Antiqua" Font-Size="Medium" OnTextChanged="TxtToDate_TextChanged"
                                Font-Bold="True" AutoPostBack="True"></asp:TextBox>
                            <asp:CalendarExtender ID="CalendarExtender1" TargetControlID="txtToDate" runat="server"
                                Format="d-MM-yyyy">
                            </asp:CalendarExtender>
                            <br />
                        </td>--%>
                        <td>
                            <asp:Button ID="Btngo" runat="server" OnClick="Btngo_Click" CssClass="cursorptr"
                                Style="font-weight: 700; top: 273px; left: 385px;" Text="GO" Width="56px" />
                        </td>
                        <%--<td>
                            <asp:Label ID="lbl_subj_select" runat="server" Text="Select Subject" Font-Names="Book Antiqua"
                                Font-Bold="True" Font-Size="Medium"></asp:Label><asp:DropDownList ID="ddl_select_subj"
                                    runat="server" Font-Names="Book Antiqua" Font-Size="Medium" Font-Bold="True"
                                    AutoPostBack="True" Height="22px" OnSelectedIndexChanged="ddl_select_subj_SelectedIndexChanged"
                                    Width="172px">
                                </asp:DropDownList>
                        </td>--%>
                        <td>
                            &nbsp;
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
            </div>
        </center>
        <center>
            <div id="divMainContent" runat="server" visible="false">
                <table>
                    <tr>
                        <td colspan="2">
                            <asp:GridView ID="GridView1" runat="server" ShowFooter="True" AutoGenerateColumns="false"
                                OnRowDataBound="OnRowDataBound" Font-Names="Book Antiqua" OnSelectedIndexChanged="GridView1_SelectedIndexChanged"
                                OnRowCommand="GridView1_RowCommand" OnRowDeleting="GridView1_RowDeleting">
                                <Columns>
                                    <asp:TemplateField HeaderText="S.No">
                                        <ItemTemplate>
                                            <%#Container.DataItemIndex+1 %>
                                            <asp:Label ID="lblentryNo" runat="server" Text='<%# Eval("hrentry_no") %>' Visible="false"></asp:Label>
                                            <asp:Label ID="lblDetNo" runat="server" Text='<%# Eval("hrdet_no") %>' Visible="false"></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Subject Name">
                                        <ItemTemplate>
                                            <asp:Label ID="lblSubject" runat="server" Text='<%# Eval("subject_no") %>' Visible="false" />
                                            <asp:DropDownList ID="ddlSubject" runat="server" OnSelectedIndexChanged="ddlSubject_SelectedIndexChanged"
                                                AutoPostBack="true">
                                                <%-- OnTextChanged="ddlSubjectChanged" --%>
                                            </asp:DropDownList>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Staff Name">
                                        <ItemTemplate>
                                            <asp:Label ID="lblStaff" runat="server" Text='<%# Eval("staff_code") %>' Visible="false" />
                                            <asp:DropDownList ID="ddlStaff" runat="server" AutoPostBack="true">
                                            </asp:DropDownList>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Start Time">
                                        <ItemTemplate>
                                            <asp:TextBox ID="TextBox1" runat="server" Text='<%# Eval("start_time") %>' placeholder="HH:MM in 24 hrs format"></asp:TextBox>
                                            <asp:RegularExpressionValidator ID="starttime" runat="server" ErrorMessage="*" ForeColor="Red"
                                                ToolTip="Hours : Minutes" ControlToValidate="TextBox1" ValidationExpression="^([0-1]?[0-9]|2[0-4]):([0-5][0-9])(:[0-5][0-9])?$">
                                            </asp:RegularExpressionValidator>
                                            <asp:FilteredTextBoxExtender ID="startymextender" runat="server" ValidChars="1234567890:HH:MM" TargetControlID="TextBox1"   FilterType="Custom" ></asp:FilteredTextBoxExtender>
                                            <asp:RequiredFieldValidator ID="rqrdvalidator" runat="server" ControlToValidate="TextBox1"
                                                ErrorMessage="*" ForeColor="Red"></asp:RequiredFieldValidator>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="End Time">
                                        <ItemTemplate>

                                            <asp:TextBox ID="TextBox2" runat="server" Text='<%# Eval("end_time") %>' OnTextChanged="TextBox2_TextChanged"
                                                ToolTip="Hours : Minutes" AutoPostBack="true" placeholder="HH:MM in 24 hrs format"></asp:TextBox>
                                            <asp:RegularExpressionValidator ID="endtime" runat="server" ErrorMessage="*" ForeColor="Red"
                                                ControlToValidate="TextBox2" ValidationExpression="^([0-1]?[0-9]|2[0-4]):([0-5][0-9])(:[0-5][0-9])?$">
                                            </asp:RegularExpressionValidator>
                                            <asp:FilteredTextBoxExtender ID="endtymextender" runat="server" ValidChars="1234567890:HH:MM" TargetControlID="TextBox2" FilterMode="ValidChars" FilterType="Custom" ></asp:FilteredTextBoxExtender>
                                            <asp:RequiredFieldValidator ID="rqrdvalidator2" runat="server" ControlToValidate="TextBox2"
                                                ErrorMessage="*" ForeColor="Red"></asp:RequiredFieldValidator>
                                            <asp:Label ID="err" runat="server" Visible="false"></asp:Label>
                                            
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Topic Covered">
                                        <ItemTemplate>
                                            <asp:TextBox ID="TextBox3" runat="server" Text='<%# Eval("topic_no") %>'></asp:TextBox>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Delete">
                                        <ItemTemplate>
                                            <asp:LinkButton ID="lnkdelete" runat="server" CommandName="Delete">Delete</asp:LinkButton>
                                        </ItemTemplate>
                                        <FooterStyle HorizontalAlign="Right" />
                                        <FooterTemplate>
                                            <asp:Button ID="savesubentrymarks" runat="server" Text="Add New Row" OnClick="addnewrow" />
                                        </FooterTemplate>
                                    </asp:TemplateField>
                                    <%--<asp:TemplateField>
                                        <ItemTemplate>
                                            <%--<asp:CheckBox ID="chkRow" runat="server"  Visible="false"/>
                                        </ItemTemplate>
                                    </asp:TemplateField>--%>
                                </Columns>
                                <HeaderStyle BackColor="#009999" ForeColor="White" />
                            </asp:GridView>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2">
                            <asp:Label ID="lblset" runat="server" Visible="False" Style="font-family: 'Baskerville Old Face';
                                font-weight: 700; height: auto; width: auto;" Font-Bold="False" Font-Size="Medium"
                                ForeColor="Red"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2" align="center">
                            <asp:Button ID="btnSave" runat="server" CssClass="cursorptr" Style="font-weight: 700;"
                                Text="Save" Width="83px" OnClick="btnSave_Click" Height="32px" />
                            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                            <%--  <asp:Button ID="btndelete" runat="server" CssClass="cursorptr" Style="font-weight: 700;"
                                Text="Delete" Width="83px" OnClick="btndelete_Click" Height="32px" />--%>
                        </td>
                    </tr>
                </table>
            </div>
        </center>
    </asp:Panel>
    <center>
        <%--  <asp:Label ID="lblspecial" runat="server" Font-Bold="True" Font-Size="Medium" ForeColor="Red"
            Text="Special Class Can Not Taken For Particular Date" Visible="False"></asp:Label>--%>
    </center>
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

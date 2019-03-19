<%@ Page Title="" Language="C#" MasterPageFile="~/AttendanceMOD/AttendanceSubSiteMaster.master"
    AutoEventWireup="true" CodeFile="Foundationsubjectselection.aspx.cs" Inherits="Foundationsubjectselection" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">
    <!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
    <html xmlns="http://www.w3.org/1999/xhtml">
    <title></title>
    <link href="~/Styles/Style.css" rel="stylesheet" type="text/css" />
    <style type="text/css">
        .textbox
        {
            border: 1px solid #c4c4c4;
            height: 30px;
            width: 50px;
            font-size: 13px;
            text-transform: capitalize;
            padding: 4px 4px 4px 4px;
            border-radius: 4px;
            -moz-border-radius: 4px;
            -webkit-border-radius: 4px;
            box-shadow: 0px 0px 8px #d9d9d9;
            -moz-box-shadow: 0px 0px 8px #d9d9d9;
            -webkit-box-shadow: 0px 0px 8px #d9d9d9;
        }
        .textbox1:hover
        {
            outline: none;
            border: 1px solid #7bc1f7;
            box-shadow: 0px 0px 8px #7bc1f7;
            -moz-box-shadow: 0px 0px 8px #7bc1f7;
            -webkit-box-shadow: 0px 0px 8px #7bc1f7;
        }
    </style>
    <body>
        <asp:ScriptManager ID="ScriptManager1" runat="server">
        </asp:ScriptManager>
        <br />
        <center>
            <asp:Label ID="Label2" runat="server" Text="Foundation Subject Settings" Font-Bold="True"
                Font-Names="Book Antiqua" Font-Size="Large" ForeColor="Green"></asp:Label></center>
        <center>
            <br />
            <table class="maintablestyle" style="width: 700px; height: 40px;">
                <tr>
                    <td>
                        <asp:Label ID="Label3" runat="server" Text="College" Font-Bold="True" Font-Names="Book Antiqua"
                            Font-Size="Medium" />
                    </td>
                    <td>
                        <asp:DropDownList ID="ddlcollege" runat="server" Font-Names="Book Antiqua" Font-Size="medium"
                            Width="150px">
                        </asp:DropDownList>
                    </td>
                    <td>
                        <asp:Label ID="Label1" runat="server" Text="Batch" Font-Bold="True" Font-Names="Book Antiqua"
                            Font-Size="Medium" />
                    </td>
                    <td>
                        <asp:DropDownList ID="ddlbatch" runat="server" Font-Names="Book Antiqua" Font-Size="medium"
                            Width="100px" AutoPostBack="true" OnSelectedIndexChanged="ddlbatch_Change">
                        </asp:DropDownList>
                    </td>
                    <td>
                        <asp:Label ID="Label4" runat="server" Text="Degree" Font-Bold="True" Font-Names="Book Antiqua"
                            Font-Size="Medium" />
                    </td>
                    <td>
                        <asp:DropDownList ID="ddldegree" runat="server" Font-Names="Book Antiqua" Font-Size="medium"
                            Width="100px" AutoPostBack="true" OnSelectedIndexChanged="ddldegree_Change">
                        </asp:DropDownList>
                    </td>
                    <td>
                        <asp:Label ID="Label5" runat="server" Text="Department" Font-Bold="True" Font-Names="Book Antiqua"
                            Font-Size="Medium" />
                    </td>
                    <td>
                        <asp:DropDownList ID="ddldepartment" runat="server" Font-Names="Book Antiqua" Font-Size="medium"
                            Width="100px" AutoPostBack="true" OnSelectedIndexChanged="ddldepartment_Change">
                        </asp:DropDownList>
                    </td>
                    <td>
                        <asp:Label ID="Label6" runat="server" Text="Sem" Font-Bold="True" Font-Names="Book Antiqua"
                            Font-Size="Medium" />
                    </td>
                    <td>
                        <asp:DropDownList ID="ddlsem" runat="server" Font-Names="Book Antiqua" Font-Size="medium"
                            Width="50px">
                        </asp:DropDownList>
                    </td>
                    <td>
                        <asp:Button ID="btngo" runat="server" Text="GO" OnClick="btngo_click" />
                    </td>
                </tr>
            </table>
            <br />
            <div style="margin-left: 78px;">
                <center>
                    <div style="float: left;">
                        <table width="400px">
                            <tr>
                                <td>
                                    <asp:RadioButtonList ID="rdblist" runat="server" Visible="false" AutoPostBack="true"
                                        OnSelectedIndexChanged="rdblist_Change" RepeatDirection="Vertical">
                                    </asp:RadioButtonList>
                                </td>
                            </tr>
                        </table>
                    </div>
                    <div style="float: left;">
                        <table width="400px">
                            <tr>
                                <td>
                                    <asp:RadioButtonList ID="rdblist2" Visible="false" runat="server" RepeatDirection="Vertical">
                                    </asp:RadioButtonList>
                                </td>
                            </tr>
                        </table>
                    </div>
                </center>
            </div>
            <br />
            <br />
            <div style="width=800px; left: 858px; position: absolute; text-align: right;">
                <asp:Button ID="btnok" runat="server" Visible="false" CssClass="textbox textbox1"
                    Text="OK" OnClick="btn_ok" />
            </div>
            <br />
            <br />
            <div style="margin-left: 78px;">
                <center>
                    <div style="float: left;">
                        <table width="400px">
                            <tr>
                                <td>
                                    <asp:RadioButtonList ID="rdbsingsubject" runat="server" Visible="false" AutoPostBack="true"
                                        OnSelectedIndexChanged="rdbsingsubject_Change" RepeatDirection="Vertical">
                                    </asp:RadioButtonList>
                                </td>
                            </tr>
                        </table>
                    </div>
                    <div style="float: left;">
                        <table width="400px">
                            <tr>
                                <td>
                                    <asp:CheckBoxList ID="cbmultiplesubject" Visible="false" runat="server" RepeatDirection="Vertical">
                                    </asp:CheckBoxList>
                                </td>
                            </tr>
                        </table>
                    </div>
                </center>
            </div>
            <br />
            <br />
            <div style="width=800px; left: 858px; position: absolute; text-align: right;">
                <asp:CheckBox ID="cbdepanttamil" runat="server" Visible="false" Text="Depend on Tamil" />
                <br />
                <asp:Button ID="btnsave" runat="server" Text="Save" CssClass="textbox textbox1" Visible="false"
                    OnClick="btnsave_ok" />
            </div>
        </center>
    </body>
    </html>
</asp:Content>

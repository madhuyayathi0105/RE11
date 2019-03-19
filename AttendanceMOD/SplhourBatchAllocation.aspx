<%@ Page Title="" Language="C#" MasterPageFile="~/AttendanceMOD/AttendanceSubSiteMaster.master"
    AutoEventWireup="true" CodeFile="SplhourBatchAllocation.aspx.cs" Inherits="SplhourBatchAllocation" %>

<%@ Register Src="~/Usercontrols/Input_Events.ascx" TagName="collegedeatils" TagPrefix="UC" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Assembly="FarPoint.Web.Spread" Namespace="FarPoint.Web.Spread" TagPrefix="FarPoint" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <style type="text/css">
        .sty
        {
            font-size: medium;
            font-family: Book Antiqua;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <br />
    <center>
        <asp:Label ID="Label2" runat="server" Text="Special Hour Batch Allocation" Font-Bold="True"
            Font-Names="Book Antiqua" Font-Size="Large" ForeColor="Green"></asp:Label>
    </center>
    <br />
    <center>
        <table class="maintablestyle" style="width: 700px; height: 40px; background-color: #0CA6CA;">
            <tr>
                <td>
                    <asp:Label ID="Label31" runat="server" Text="College" Font-Bold="true" Font-Names="MS Sans Serif"
                        Font-Size="Medium"></asp:Label>
                </td>
                <td>
                    <asp:DropDownList ID="ddlcollege" runat="server" Font-Names="Book Antiqua" Font-Bold="true"
                        Font-Size="medium" Width="150px" OnSelectedIndexChanged="ddlcollege_change" AutoPostBack="true">
                    </asp:DropDownList>
                </td>
                <td>
                    <asp:Label ID="Label1" runat="server" Text="Batch" Font-Bold="true" Font-Names="Book Antiqua"
                        Font-Size="Medium"></asp:Label>
                </td>
                <td>
                    <asp:DropDownList ID="ddlbatch" runat="server" Font-Names="Book Antiqua" Font-Bold="true"
                        Font-Size="medium" Width="100px" AutoPostBack="true" OnSelectedIndexChanged="ddlbatch_Change">
                    </asp:DropDownList>
                </td>
                <td>
                    <asp:Label ID="Label3" runat="server" Text="Degree" Font-Bold="true" Font-Names="Book Antiqua"
                        Font-Size="Medium"></asp:Label>
                </td>
                <td>
                    <asp:DropDownList ID="ddldegree" runat="server" Font-Names="Book Antiqua" Font-Bold="true"
                        Font-Size="medium" Width="100px" AutoPostBack="true" OnSelectedIndexChanged="ddldegree_Change">
                    </asp:DropDownList>
                </td>
                <td>
                    <asp:Label ID="Label4" runat="server" Text="Department" Font-Bold="true" Font-Names="Book Antiqua"
                        Font-Size="Medium"></asp:Label>
                </td>
                <td>
                    <asp:DropDownList ID="ddldepartment" runat="server" Font-Names="Book Antiqua" Font-Bold="true"
                        Font-Size="medium" Width="100px" AutoPostBack="true" OnSelectedIndexChanged="ddldepartment_Change">
                    </asp:DropDownList>
                </td>
                <td>
                    <asp:Label ID="Label5" runat="server" Text="Sem" Font-Bold="true" Font-Names="Book Antiqua"
                        Font-Size="Medium"></asp:Label>
                </td>
                <td>
                    <asp:DropDownList ID="ddlsem" runat="server" Font-Names="Book Antiqua" Font-Bold="true"
                        Font-Size="medium" Width="50px" AutoPostBack="true" OnSelectedIndexChanged="ddlsem_Change">
                    </asp:DropDownList>
                </td>
                <td>
                    <asp:Label ID="Label6" runat="server" Text="Sec" Font-Bold="true" Font-Names="Book Antiqua"
                        Font-Size="Medium"></asp:Label>
                </td>
                <td>
                    <asp:DropDownList ID="ddlsection" runat="server" Font-Names="Book Antiqua" Font-Bold="true"
                        Font-Size="medium" Width="50px" AutoPostBack="true" OnSelectedIndexChanged="ddlsection_Change">
                    </asp:DropDownList>
                </td>
                <td>
                    <asp:Button ID="btngo" runat="server" Text="GO" Font-Bold="true" OnClick="btngo_click"
                        Width="70px" Height="30px" />
                </td>
            </tr>
        </table>
        <table id="subtable" runat="server" visible="false">
            <tr>
                <td>
                    <asp:Label ID="Label7" runat="server" Text="Date" Font-Bold="true" Font-Names="MS Sans Serif"
                        Font-Size="Medium" ForeColor="Green"></asp:Label>
                </td>
                <td>
                    <asp:DropDownList ID="ddlspecialdate" runat="server" Font-Bold="true" Font-Names="Book Antiqua"
                        Font-Size="medium" Width="150px" AutoPostBack="true" OnSelectedIndexChanged="ddlspecialdate_Change">
                    </asp:DropDownList>
                </td>
                <td>
                    <asp:Label ID="Label8" runat="server" Text="Subject" Font-Bold="true" Font-Names="MS Sans Serif"
                        Font-Size="Medium" ForeColor="Green"></asp:Label>
                </td>
                <td>
                    <asp:DropDownList ID="ddlsubject" runat="server" Font-Bold="true" Font-Names="Book Antiqua"
                        Font-Size="medium" Width="150px">
                    </asp:DropDownList>
                </td>
                <td>
                    <asp:Label ID="Label9" runat="server" Text="No of Batches" Font-Bold="true" Font-Names="MS Sans Serif"
                        Font-Size="Medium" ForeColor="Green"></asp:Label>
                </td>
                <td>
                    <asp:TextBox ID="txt_noofbatchs" runat="server" Font-Bold="true" Font-Names="Book Antiqua"
                        MaxLength="2" Font-Size="medium" Width="50px">
                    </asp:TextBox>
                    <asp:FilteredTextBoxExtender ID="fld" runat="server" FilterType="Numbers" TargetControlID="txt_noofbatchs">
                    </asp:FilteredTextBoxExtender>
                </td>
                <td>
                    <asp:Button ID="btnallocate" runat="server" Text="Go" Font-Bold="true" OnClick="btnallocate_click" />
                </td>
            </tr>
        </table>
        <br />
        <asp:Label ID="errorlable" runat="server" ForeColor="Red" Font-Bold="true" Font-Names="Book Antiqua"
            Font-Size="medium" Visible="false"></asp:Label>
        <br />
        <div id="mainvlaue" runat="server" visible="false">
            <div style="float: left;">
                <FarPoint:FpSpread ID="Fpspread" runat="server" CssClass="sty" BorderColor="Black"
                    BorderStyle="Solid" BorderWidth="1px" Height="300" Width="500" CommandBar-Visible="false"
                    ShowHeaderSelection="false">
                    <Sheets>
                        <FarPoint:SheetView SheetName="Sheet1">
                        </FarPoint:SheetView>
                    </Sheets>
                </FarPoint:FpSpread>
                <br />
                <br />
                <fieldset id="Fieldset2" runat="server" visible="false" style="width: 305px;">
                    <asp:CheckBox ID="CheckBox1" runat="server" AutoPostBack="true" OnCheckedChanged="CheckBox1_CheckedChanged" />
                    <asp:Label ID="lblselect" runat="server" Text="Select"></asp:Label>
                    <asp:Label ID="lblfrom" runat="server" Text="From"></asp:Label>
                    <asp:TextBox ID="fromno" runat="server" Width="50px" Enabled="false"></asp:TextBox>
                    <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender2" runat="server" TargetControlID="fromno"
                        FilterType="Numbers" />
                    <asp:Label ID="lblto" runat="server" Text="To"></asp:Label>
                    <asp:TextBox ID="tono" runat="server" Width="50px" Enabled="false"></asp:TextBox>
                    <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" runat="server" TargetControlID="tono"
                        FilterType="Numbers" />
                    <asp:Button ID="Button2" runat="server" Text="Go" Font-Bold="true" OnClick="selectgo_Click" />
                    <br />
                    <br />
                    <center>
                        <span style="font-family: Book Antiqua; font-size: medium;">LabBatch</span>
                        <asp:DropDownList ID="ddllabbatch" runat="server" Font-Names="Book Antiqua" Font-Size="medium"
                            Width="100px">
                        </asp:DropDownList>
                        <asp:Button ID="Btnsave" runat="server" Text="Save" Enabled="false" OnClick="Btnsave_Click" />
                        <asp:Button ID="Btndelete" runat="server" Text="Delete" Enabled="false" OnClick="Btndelete_Click" />
                    </center>
                </fieldset>
            </div>
            <div style="float: left; margin-left: 20px;">
                <FarPoint:FpSpread ID="Fpspread1" runat="server" CssClass="sty" BorderColor="Black"
                    BorderStyle="Solid" BorderWidth="1px" Height="300" Width="400" CommandBar-Visible="false"
                    ShowHeaderSelection="false">
                    <Sheets>
                        <FarPoint:SheetView SheetName="Sheet1">
                        </FarPoint:SheetView>
                    </Sheets>
                </FarPoint:FpSpread>
                <br />
                <br />
                <asp:LinkButton ID="lnkmultiple" runat="server" Visible="false" CausesValidation="False"
                    Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Small" ForeColor="blue"
                    OnClick="LinkButton1_Click">To Add Multiple Batch</asp:LinkButton>
                <div id="subdiv" runat="server" visible="false" style="border: 1px solid Red; width: 100px;
                    height: auto;">
                    <asp:CheckBoxList ID="cbbatchlist" runat="server">
                    </asp:CheckBoxList>
                    <br />
                    <center>
                        <asp:Button ID="btnsub" runat="server" Text="Ok" Font-Bold="true" OnClick="btnsub_Clcik" />
                    </center>
                </div>
                <center>
                    <asp:Button ID="btnbatchsave" runat="server" Visible="false" Text="Save" Font-Bold="true"
                        OnClick="Batchallotsave_Click" />
                </center>
            </div>
        </div>
    </center>
</asp:Content>

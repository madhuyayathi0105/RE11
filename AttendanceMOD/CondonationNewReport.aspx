<%@ Page Title="" Language="C#" MasterPageFile="~/AttendanceMOD/AttendanceSubSiteMaster.master"
    AutoEventWireup="true" CodeFile="CondonationNewReport.aspx.cs" Inherits="AttendanceMOD_CondonationNewReport" %>

<%@ Register Assembly="FarPoint.Web.Spread,  Version=5.0.3520.2008, Culture=neutral, PublicKeyToken=327c3516b1b18457"
    Namespace="FarPoint.Web.Spread" TagPrefix="FarPoint" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <center>
        <span class="fontstyleheader" style="color: Green; margin: 0px; margin-bottom: 10px;
            margin-top: 10px;">Student Condonation Report</span>
        <table style="margin: 0px; margin-top: 10px;">
            <tr>
                <td>
                   <%-- <asp:Label ID="lblRollNo" runat="server" Text="Roll No" Font-Bold="True" Font-Names="Book Antiqua"
                        Font-Size="Medium"></asp:Label>--%>

                    <asp:DropDownList ID="ddlrollType" runat="server">
                    <asp:ListItem Text="Roll No" Value="0"> </asp:ListItem>
                    <asp:ListItem Text="Reg No" Value="1"> </asp:ListItem>
                    </asp:DropDownList>

                </td>
                <td>
                    <asp:TextBox ID="txtRollNo" runat="server" Font-Names="Book Antiqua" Width="300px" Font-Size="Medium"></asp:TextBox>
                    <asp:AutoCompleteExtender ID="AutoCompleteExtender1" runat="server" DelimiterCharacters=","
                        Enabled="True" ServiceMethod="GetRollNo" MinimumPrefixLength="0" CompletionInterval="100"
                        EnableCaching="false" CompletionSetCount="10" ServicePath="" TargetControlID="txtRollNo"
                        CompletionListCssClass="autocomplete_completionListElement" CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem"
                        CompletionListItemCssClass="txtsearchpan">
                    </asp:AutoCompleteExtender>
                </td>
                <td>
                    <asp:Label ID="lblFormat" runat="server" Text="Format" Font-Bold="True" Font-Names="Book Antiqua"
                        Font-Size="Medium"></asp:Label>
                </td>
                <td>
                    <asp:DropDownList ID="ddlCondonationFormat" runat="server">
                        <asp:ListItem Text="Format 1" Value="0"></asp:ListItem>
                        <asp:ListItem Text="Format 2" Value="1">  </asp:ListItem>
                    </asp:DropDownList>
                </td>
                <td>
                    <asp:Button ID="btnGo" runat="server" Text="Go" Font-Bold="True" Font-Names="Book Antiqua"
                        Font-Size="Medium" OnClick="btnGo_OnClick" />
                </td>
            </tr>
        </table>
        <div id="divMainContent" runat="server" visible="false">
            <center>
                <table style="margin: 0px; margin-bottom: 10px; margin-top: 10px;">
                    <tr>
                       
                        <td colspan="2" align="center">
                            <FarPoint:FpSpread ID="FpSpreadCondonationList" runat="server" Height="250px" Width="400px"
                                CssClass="cursorptr" BorderColor="Black" BorderWidth="0.5" OnButtonCommand="FpSpreadCondonationList_BuutonCommand"
                                Visible="false">
                                <Sheets>
                                    <FarPoint:SheetView SheetName="Sheet1">
                                    </FarPoint:SheetView>
                                </Sheets>
                            </FarPoint:FpSpread>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2" align="center">
                            <asp:Button ID="btnPrint" runat="server" Text="Print" OnClick="btnPrint_OnClick" />
                        </td>
                    </tr>
                </table>
                <asp:Label ID="lblmsg" runat="server" Visible="false"></asp:Label>
            </center>
        </div>
    </center>
    <div id="divPopCond" runat="server" visible="false" style="height: 550em; z-index: 2000;
        width: 100%; background-color: rgba(54, 25, 25, .2); position: absolute; top: 0%;
        left: 0%;">
        <center>
            <div id="divPopUpCondenation" runat="server" class="table" style="background-color: White;
                height: auto; width: 60%; border: 5px solid #0CA6CA; border-top: 25px solid #0CA6CA;
                left: 18%; right: 18%; top: 35%; padding: 5px; position: fixed; border-radius: 10px;">
                <center>
                    <table style="height: 100px; width: auto; padding: 5px;">
                        <tr>
                            <td>
                                <asp:Label ID="Label3" runat="server" Text="Date" Font-Bold="True" Font-Size="Medium"></asp:Label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtCondonationDate" runat="server" placeholder="dd/MM/yyyy" Font-Names="Book Antiqua"
                                    Font-Size="Medium" Width="90px"></asp:TextBox>
                                <asp:CalendarExtender ID="CalendarExtender4" Format="dd/MM/yyyy" TargetControlID="txtCondonationDate"
                                    runat="server">
                                </asp:CalendarExtender>
                            </td>
                            <td>
                                <asp:Label ID="lblChallanno" runat="server" Text="Challan No" Font-Bold="True" Font-Size="Medium"></asp:Label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtChallanAmount" runat="server" Font-Names="Book Antiqua" Font-Size="Medium"
                                    Width="120px" AutoPostBack="true"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Button ID="btnPopAlertClose" Font-Bold="true" Font-Size="Medium" Font-Names="Book Antiqua"
                                    CssClass="textbox textbox1" Style="height: auto; width: auto;" OnClick="btnPopAlertClose_Click"
                                    Text="Save" runat="server" />
                            </td>
                            <td>
                                <asp:Button ID="btnCondExit" Font-Bold="true" Font-Size="Medium" Font-Names="Book Antiqua"
                                    CssClass="textbox textbox1" Style="height: auto; width: auto;" OnClick="btnCondExit_Click"
                                    Text="Close" runat="server" />
                            </td>
                        </tr>
                    </table>
                </center>
            </div>
        </center>
        <asp:Label ID="lblxpos" runat="server" Visible="false"></asp:Label>
        <asp:Label ID="lblypos" runat="server" Visible="false"></asp:Label>
    </div>
    <center>
        <div id="divPopUpAlert" runat="server" visible="false" style="height: 550em; z-index: 2000;
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
                                        <asp:Button ID="Button1" Font-Bold="true" Font-Size="Medium" Font-Names="Book Antiqua"
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

<%@ Page Title="" Language="C#" MasterPageFile="~/FeedBackMOD/FeedbackSubSiteMaster.master" AutoEventWireup="true"
    CodeFile="uniquecode_generation_Report.aspx.cs" Inherits="uniquecode_generation_Report" %>

<%@ Register Src="~/Usercontrols/PrintMaster.ascx" TagName="printmaster" TagPrefix="Insproplus" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Assembly="FarPoint.Web.Spread" Namespace="FarPoint.Web.Spread" TagPrefix="FarPoint" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <link href="~/Styles/css/Commoncss.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript">
        function display() {
            document.getElementById('<%=lblvalidation1.ClientID %>').innerHTML = "";
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <div>
        <br />
        <center>
            <div>
                <center>
                    <div>
                        <span class="fontstyleheader" style="color: Green">Unique Code Generation Report</span>
                    </div>
                </center>
                <br />
                <div class="maindivstyle maindivstylesize">
                    <br />
                    <center>
                        <div>
                            <table class="maintablestyle">
                                <tr>
                                    <td>
                                        <asp:Label ID="lbl_clgname" runat="server" Text="College Name"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="ddl_collegename" runat="server" CssClass="textbox1 ddlheight6">
                                        </asp:DropDownList>
                                    </td>
                                    <td>
                                        <asp:Label ID="lbl_feedbackname" runat="server" Text="Feedback Name"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="ddl_feedbackname" runat="server" CssClass="textbox1 ddlheight5">
                                        </asp:DropDownList>
                                    </td>
                                    <td>
                                        <asp:Button ID="btn_go" Text="Go" runat="server" CssClass="textbox btn1" OnClick="btn_go_Click" />
                                    </td>
                                </tr>
                            </table>
                            <br />
                            <center>
                                <div>
                                    <asp:Label ID="lbl_error" Visible="false" runat="server" ForeColor="red"></asp:Label>
                                </div>
                            </center>
                        </div>
                        <br />
                        <center>
                            <FarPoint:FpSpread ID="FpSpread1" runat="server" BorderColor="Black" BorderStyle="Solid"
                                BorderWidth="1px" Width="670px" Height="360px" CssClass="spreadborder">
                                <Sheets>
                                    <FarPoint:SheetView SheetName="Sheet1" SelectionBackColor="#0CA6CA">
                                    </FarPoint:SheetView>
                                </Sheets>
                            </FarPoint:FpSpread>
                        </center>
                        <br />
                        <div id="rptprint" runat="server" visible="false">
                            <asp:Label ID="lblvalidation1" runat="server" ForeColor="Red" Text="Please Enter Your Report Name"
                                Visible="false"></asp:Label>
                            <asp:Label ID="lblrptname" runat="server" Text="Report Name"></asp:Label>
                            <asp:TextBox ID="txtexcelname" CssClass="textbox textbox1" runat="server" Height="20px"
                                Width="180px" onkeypress="display()"></asp:TextBox>
                            <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender56" runat="server" TargetControlID="txtexcelname"
                                FilterType="LowercaseLetters,UppercaseLetters,Numbers,Custom" ValidChars="!@$%^&()_+}{][';,. ">
                            </asp:FilteredTextBoxExtender>
                            <asp:Button ID="btnExcel" runat="server" OnClick="btnExcel_Click" CssClass="textbox btn1"
                                Text="Export To Excel" Width="127px" />
                            <asp:Button ID="btnprintmaster" runat="server" Text="Print" OnClick="btnprintmaster_Click"
                                CssClass="textbox btn1" />
                            <Insproplus:printmaster runat="server" ID="Printcontrol" Visible="false" />
                        </div>
                        <br />
                    </center>
                </div>
            </div>
        </center>
        <center>
            <div id="alertpopwindow" runat="server" visible="false" style="height: 100%; z-index: 1000;
                width: 100%; background-color: rgba(54, 25, 25, .2); position: absolute; top: 0;
                left: 0px;">
                <center>
                    <div id="pnl2" runat="server" class="table" style="background-color: White; height: 120px;
                        width: 238px; border: 5px solid #0CA6CA; border-top: 25px solid #0CA6CA; margin-top: 200px;
                        border-radius: 10px;">
                        <center>
                            <br />
                            <table style="height: 100px; width: 100%">
                                <tr>
                                    <td align="center">
                                        <asp:Label ID="lblalerterr" runat="server" Text="" Style="color: Red;" Font-Bold="true"
                                            Font-Size="Medium"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <center>
                                            <asp:Button ID="btnerrclose" CssClass=" textbox btn1 comm" Style="height: 28px; width: 65px;"
                                                OnClick="btnerrclose_Click" Text="Ok" runat="server" />
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
</asp:Content>

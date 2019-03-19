<%@ Page Title="" Language="C#" MasterPageFile="~/FeedBackMOD/FeedbackSubSiteMaster.master"
    AutoEventWireup="true" CodeFile="Online_FeedBack.aspx.cs" Inherits="Online_FeedBack" %>

<%@ Register Src="~/Usercontrols/PrintMaster.ascx" TagName="printmaster" TagPrefix="Insproplus" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Assembly="FarPoint.Web.Spread" Namespace="FarPoint.Web.Spread" TagPrefix="FarPoint" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <link href="~/Styles/css/Registration.css" rel="stylesheet" type="text/css" />
    <link href="~/Styles/css/Commoncss.css" rel="Stylesheet" type="text/css" />
<%--    <script src="//ajax.googleapis.com/ajax/libs/jquery/1.11.1/jquery.min.js"></script>--%>
<script type="text/javascript" src="../Scripts/jquery-1.4.1.min.js"></script>
    <script type="text/javascript">
     {
    background-image: url("~/images/images.jpg");
    background-color: #cccccc;
}
        function check() {
            var id;
            var id1;
            var empty = "";

            if (document.getElementById("<%=txt_unic_fb.ClientID %>").value.trim() == "") {
                id = document.getElementById("<%=txt_unic_fb.ClientID %>");
                id.style.borderColor = 'Red';
                empty = "E";
            }
            if (empty.trim() != "") {
                return false;
            }
            else {
                return true;
            }
        }
        function display(x) {
            x.style.borderColor = "#c4c4c4";

        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <html xmlns="http://www.w3.org/1999/xhtml">
    <head id="Head1">
        <title></title>
        <link href="Styles/css/Registration.css" rel="stylesheet" type="text/css" />
        <link href="Styles/css/Commoncss.css" rel="Stylesheet" type="text/css" />
    </head>
    <body>
        <script type="text/javascript">
     {
    background-image: url("~/images/images.jpg");
    background-color: #cccccc;
}
        function check() {
            var id;
            var id1;
            var empty = "";
            if (document.getElementById("<%=txt_unic_fb.ClientID %>").value.trim() == "") {
                id = document.getElementById("<%=txt_unic_fb.ClientID %>");
                id.style.borderColor = 'Red';
                empty = "E";
            }
            if (empty.trim() != "") {
                return false;
            }
            else {
                return true;
            }
        }
        function display(x) {
            x.style.borderColor = "#c4c4c4";
        }  
        </script>
        <form id="form1">
        <center>
            <div>
                <span class="fontstyleheader" style="color: Green">Online FeedBack Test</span>
            </div>
        </center>
        <center>
            <%-- <div style="margin-left: 854px;">
                <asp:LinkButton ID="lb3" runat="server" Visible="true" Font-Bold="True" Font-Names="Book Antiqua"
                    Font-Size="Small" ForeColor="Blue" CausesValidation="False" OnClick="lb3_Click">Logout</asp:LinkButton>
            </div>--%>
            <br />
            <div id="questiondiv" runat="server" visible="false">
                <div>
                    <div>
                        <table style="width: 800px;">
                            <tr>
                                <td>
                                    <asp:Label ID="Label1" runat="server" CssClass="fontstyleheader" Text="Welcome" Font-Bold="true"
                                        ForeColor="Maroon"></asp:Label>
                                </td>
                                <td align="center">
                                    <asp:Label ID="lbl_Name" runat="server" Width="450px" CssClass="fontstyleheader"
                                        Text="" Font-Bold="true" ForeColor="Blue"></asp:Label>
                                </td>
                                <td>
                                    <asp:Button ID="Clickbtn" runat="server" CssClass="textbox btn2" Text="Take Test"
                                        OnClick="Clickbtn_Click" />
                                </td>
                                <td>
                                    <asp:Label ID="Label2" runat="server" Width="450px" Visible="false" CssClass="fontstyleheader"
                                        Text="" Font-Bold="true" ForeColor="Blue"></asp:Label>
                                </td>
                            </tr>
                            <tr runat="server" id="lockNote" visible="false">
                                <td colspan="3">
                                    <span style="color: red;">Note:Lavender color boxes not editable</span>
                                </td>
                            </tr>
                        </table>
                    </div>
                </div>
            </div>
            <div id="MainDivPart" runat="server" visible="false">
                <table>
                    <tr>
                        <td>
                            <asp:Label ID="lbl_alrt" runat="server" Width="450px" Text="" Font-Bold="true" ForeColor="Maroon"
                                Style="margin-left: -237px;"></asp:Label>
                        </td>
                        <td>
                        </td>
                        <td>
                            <asp:Label ID="App_no" runat="server" Visible="false" Width="200" Text=""></asp:Label>
                        </td>
                    </tr>
                </table>
                <center>
                    <div>
                        <FarPoint:FpSpread ID="FpSpread1" runat="server" Visible="true" BorderStyle="Solid"
                            BorderWidth="0px" ShowHeaderSelection="false" CssClass="spreadborder" OnButtonCommand="FpSpread1_OnButtonCommand">
                            <Sheets>
                                <FarPoint:SheetView SheetName="Sheet1">
                                </FarPoint:SheetView>
                            </Sheets>
                        </FarPoint:FpSpread>
                    </div>
                    <br />
                    <asp:Button ID="btn_save" runat="server" Visible="true" Width="85px" Height="34px"
                        CssClass="textbox textbox1" Text="Save" OnClick="btn_save_Click" /><br />
                </center>
            </div>
            <br />
            <br />
            <div id="div1" visible="True" runat="server" class="spreadborder" style="width: 451px;
                height: 250px; overflow: auto; background-color: #6495ED; border-radius: 10px;">
                <br />
                <br />
                <br />
                <br />
                <center>
                    <table style="line-height: 40px;">
                        <tr>
                            <td>
                                <asp:Label ID="lbl_unic_fb" runat="server" Text="Unicode in FeedBack"></asp:Label>
                            </td>
                            <td>
                                <asp:TextBox ID="txt_unic_fb" runat="server" Height="25px" onfocus=" return display(this)"
                                    Width="161px"></asp:TextBox>
                                <span style="color: Red;">*</span> <span style="font-weight: bold; font-size: larger;"
                                    id="msg1"></span>
                                <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" runat="server" TargetControlID="txt_unic_fb"
                                    FilterType="Numbers,UppercaseLetters,LowercaseLetters,Custom" ValidChars=" ">
                                </asp:FilteredTextBoxExtender>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2">
                                <center>
                                    <asp:Label ID="lbl_loginalrt" Visible="false" Font-Bold="true" ForeColor="red" runat="server"
                                        Text=""></asp:Label>
                                </center>
                            </td>
                        </tr>
                    </table>
                    <br />
                    <asp:Button ID="btn_Submit" OnClientClick="return check()" runat="server" CssClass="textbox btn2"
                        Text="Submit" Style="background: #2E8B57;" OnClick="btn_Submit_Click" />
                </center>
            </div>
        </center>
        <div id="imgdiv2" runat="server" visible="false" style="height: 100%; z-index: 1000;
            width: 100%; background-color: rgba(54, 25, 25, .2); position: absolute; top: 0;
            left: 0px;">
            <center>
                <div id="pnl2" runat="server" class="table" style="background-color: White; height: 120px;
                    width: 238px; border: 5px solid #0CA6CA; border-top: 25px solid #0CA6CA; margin-top: 200px;
                    border-radius: 10px;">
                    <center>
                        <table style="height: 100px; width: 100%">
                            <tr>
                                <td align="center">
                                    <asp:Label ID="lbl_alert1" runat="server" Text="" Style="color: Red;" Font-Bold="true"
                                        Font-Size="Medium"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <center>
                                        <asp:Button ID="btn_errorclose" CssClass=" textbox btn1 comm" Style="height: 28px;
                                            width: 65px;" OnClick="btn_errorclose_Click" Text="ok" runat="server" />
                                    </center>
                                </td>
                            </tr>
                        </table>
                    </center>
                </div>
            </center>
        </div>
        </center>
        </form>
    </body>
    </html>
</asp:Content>

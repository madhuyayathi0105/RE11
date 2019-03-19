<%@ Page Title="" Language="C#" MasterPageFile="~/FeedBackMOD/FeedbackSubSiteMaster.master" AutoEventWireup="true" CodeFile="Type_Master.aspx.cs" Inherits="Type_Master" %>


<%@ Register Src="~/Usercontrols/PrintMaster.ascx" TagName="printmaster" TagPrefix="Insproplus" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Assembly="FarPoint.Web.Spread" Namespace="FarPoint.Web.Spread" TagPrefix="FarPoint" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
<link href="~/Styles/css/Registration.css" rel="stylesheet" type="text/css" />
    <link href="~/Styles/css/Commoncss.css" rel="Stylesheet" type="text/css" />
  <%--  <script src="//ajax.googleapis.com/ajax/libs/jquery/1.11.1/jquery.min.js"></script>--%>
    <script type="text/javascript" src="../Scripts/jquery-1.4.1.min.js"></script>
    <style type="text/css">
        .nv
        {
            text-transform: uppercase;
        }
    </style>

    <script type="text/javascript">


        function check() {
            var id;

            if (document.getElementById("<%=txt_type2.ClientID %>").value.trim() == "") {
                id = document.getElementById("<%=txt_type2.ClientID %>");
                id.style.borderColor = 'Red';
                empty = "E";
            }
            var id1;

            if (document.getElementById("<%=txt_abtn.ClientID %>").value.trim() == "") {
                id1 = document.getElementById("<%=txt_abtn.ClientID %>");
                id1.style.borderColor = 'Red';
                empty = "E";
            }
            var id2;

            if (document.getElementById("<%=txt_point.ClientID %>").value.trim() == "") {
                id2 = document.getElementById("<%=txt_point.ClientID %>");
                id2.style.borderColor = 'Red';
                empty = "E";
            }
            if (empty != "") {
                return false;
            }
            else {

                return true;
            }
        }
        function display(x) {
            x.style.borderColor = "#c4c4c4";

        }
        function display1() {
            document.getElementById('<%=lbl_norec1.ClientID %>').innerHTML = "";
        }
        function get(txt1) {
            $.ajax({
                type: "POST",
                url: "Type_Master.aspx/CheckUserName",
                data: '{StoreName: "' + txt1 + '"}',
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: OnSuccess,
                failure: function (response) {
                    alert(response);
                }
            });
        }

        function OnSuccess(response) {
            var mesg = $("#msg1")[0];
            switch (response.d) {
                case "0":
                    mesg.style.color = "green";
                    mesg.innerHTML = "Type Name Does Not Exist";
                    break;
                case "1":
                    mesg.style.color = "red";
                    document.getElementById('<%=txt_type2.ClientID %>').value = "";
                    mesg.innerHTML = "Type Name Available";
                    break;
                case "2":
                    mesg.style.color = "red";
                    mesg.innerHTML = "Please Enter Type";
                    break;
                case "error":
                    mesg.style.color = "red";
                    mesg.innerHTML = "Error Occurred";
                    break;
            }
        }
           
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" Runat="Server">

    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
 
     <br />
    <center>
        <div>
            <center>
                <div>
                    <span class="fontstyleheader" style="color: Green">Type Master</span></div>
                     <br />
            </center>
            <center>
                <div class="maindivstyle">
                    <br />
                    <div>
                        <center>
                            <table class="maintablestyle" width="633px" height="40px">
                                <tr>
                                    <td>
                                        <asp:Label ID="lbl_college1" runat="server" Text="College"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:UpdatePanel ID="UpdatePanel6" runat="server">
                                            <ContentTemplate>
                                                <asp:TextBox ID="Txt_college1" Width=" 230px" ReadOnly="true" runat="server" CssClass="textbox  txtheight3">--Select--</asp:TextBox>
                                                <asp:Panel ID="Panel_college1" runat="server" CssClass="multxtpanel" Height="200px"
                                                    Width="200px">
                                                    <asp:CheckBox ID="Cb_college1" runat="server" Text="Select All" AutoPostBack="True"
                                                        OnCheckedChanged="Cb_college1_CheckedChanged" />
                                                    <asp:CheckBoxList ID="Cbl_college1" runat="server" AutoPostBack="True" OnSelectedIndexChanged="Cbl_college1_SelectedIndexChanged">
                                                    </asp:CheckBoxList>
                                                </asp:Panel>
                                                <asp:PopupControlExtender ID="PopupControlExtender6" runat="server" TargetControlID="Txt_college1"
                                                    PopupControlID="Panel_college1" Position="Bottom">
                                                </asp:PopupControlExtender>
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                    </td>
                                    <td>
                                        <asp:Label ID="lbl_Type1" runat="server" Text="Type"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:UpdatePanel ID="Updp_Type" runat="server">
                                            <ContentTemplate>
                                                <asp:TextBox ID="Txt_Type" Width=" 90px" runat="server" CssClass="textbox  txtheight2">--Select--</asp:TextBox>
                                                <asp:Panel ID="Panel_Type" runat="server" CssClass="multxtpanel" Height="200px" Width="100px">
                                                    <asp:CheckBox ID="cb_Type" runat="server" Text="Select All" AutoPostBack="True" OnCheckedChanged="cb_Type_CheckedChanged" />
                                                    <asp:CheckBoxList ID="cbl_Type" runat="server" AutoPostBack="True" OnSelectedIndexChanged="cbl_Type_SelectedIndexChanged">
                                                    </asp:CheckBoxList>
                                                </asp:Panel>
                                                <asp:PopupControlExtender ID="PopupControlExtender8" runat="server" TargetControlID="Txt_Type"
                                                    PopupControlID="Panel_Type" Position="Bottom">
                                                </asp:PopupControlExtender>
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                    </td>
                                    <td>
                                        <asp:Label ID="lbl_Search" Width="50px" runat="server" Text="Search"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txt_search" Width=" 141px" runat="server" CssClass="textbox  txtheight2"></asp:TextBox>
                                        <asp:AutoCompleteExtender ID="AutoCompleteExtender10" runat="server" DelimiterCharacters=""
                                            Enabled="True" ServiceMethod="Type" MinimumPrefixLength="0" CompletionInterval="100"
                                            EnableCaching="false" CompletionSetCount="10" ServicePath="" TargetControlID="txt_search"
                                            CompletionListCssClass="autocomplete_completionListElement" CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem"
                                            CompletionListItemCssClass="panelbackground">
                                        </asp:AutoCompleteExtender>
                                    </td>
                                    <td>
                                        <asp:Button ID="btn_Search" runat="server" CssClass="textbox btn2" Text="Search"
                                            OnClick="btnSearch_Click" />
                                    </td>
                                    <td>
                                        <asp:Button ID="btn_AddNew" runat="server" CssClass="textbox btn2" Text="Add New"
                                            OnClick="btn_AddNew_Click" />
                                    </td>
                                </tr>
                            </table>
                        </center>
                    </div>
                    <br />
                    <br />
                    <div id="div1" visible="false" runat="server" class="spreadborder" style="width: 750px;
                        height: 232px; overflow: auto; background-color: White; border-radius: 10px;">
                        <FarPoint:FpSpread ID="FpSpread1" runat="server" Visible="false" BorderStyle="Solid"
                            BorderWidth="0px" Width="750px" Height="500px" Style="overflow: auto; border: 0px solid #999999;
                            border-radius: 10px; background-color: White; box-shadow: 0px 0px 8px #999999;"
                            CssClass="spreadborder" ShowHeaderSelection="false" OnCellClick="FpSpread1_OnCellClick" OnPreRender="FpSpread1_Selectedindexchange">
                            <Sheets>
                                <FarPoint:SheetView SheetName="Sheet1">
                                </FarPoint:SheetView>
                            </Sheets>
                        </FarPoint:FpSpread>
                        <br />
                    </div>
                    <br />
                    <div id="rptprint1" runat="server" visible="false">
                        <asp:Label ID="lbl_norec1" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                            Font-Size="Medium" ForeColor="Red" Text="Please Enter Your Report Name" Visible="false"></asp:Label>
                        <asp:Label ID="lblrptname1" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                            Font-Size="Medium" Text="Report Name"></asp:Label>
                        <asp:TextBox ID="txtexcelname1" runat="server" CssClass="textbox textbox1" Height="20px"
                            Width="180px" Style="font-family: 'Book Antiqua'" Font-Bold="True" Font-Names="Book Antiqua"
                            onkeypress="display1()" Font-Size="Medium"></asp:TextBox>
                        <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender9" runat="server" TargetControlID="txtexcelname1"
                            FilterType="LowercaseLetters,UppercaseLetters,Numbers,Custom" ValidChars="(),.[]_"
                            InvalidChars="/\">
                        </asp:FilteredTextBoxExtender>
                        <asp:Button ID="btnExcel1" runat="server" Font-Bold="true" Font-Names="Book Antiqua"
                            OnClick="btnExcel1_Click" Font-Size="Medium" Text="Export To Excel" Width="127px"
                            Height="35px" CssClass="textbox textbox1" />
                        <asp:Button ID="btnprintmaster1" runat="server" Text="Print" OnClick="btnprintmaster1_Click"
                            Font-Names="Book Antiqua" Font-Size="Medium" Font-Bold="true" Width="60px" Height="35px"
                            CssClass="textbox textbox1" />
                        <Insproplus:printmaster runat="server" ID="Printcontrol1" Visible="false" />
                    </div>
                    <br />
                </div>
            </center>
            <div id="Addmark" runat="server" visible="false" style="height: 100%; z-index: 1000;
                width: 100%; background-color: rgba(54, 25, 25, .2); position: absolute; top: 0;
                left: 0px;">
                <asp:ImageButton ID="ImageButton2" runat="server" Width="40px" Height="40px" ImageUrl="~/images/close.png"
                    Style="height: 30px; width: 30px; position: absolute; margin-left: 265px; margin-top: 184px;"
                    OnClick="imagebtnpopclose1_Click" />
                <center>
                    <div id="panel_add" runat="server" visible="true" class="table" style="background-color: White;
                        height: auto; width: 560px; border: 5px solid #0CA6CA; border-top: 25px solid #0CA6CA;
                        margin-top: 200px; border-radius: 10px;">
                        <br />
                        <span class="fontstyleheader" style="color: Green">Type Master</span>
                        <br />
                        <table>
                            <br />
                            <tr>
                                <td>
                                    <asp:Label ID="lbl_college" runat="server" Text="College"></asp:Label>
                                </td>
                                <td>
                                    <asp:UpdatePanel ID="UpdatePanel20" runat="server">
                                        <ContentTemplate>
                                            <asp:TextBox ID="Txt_college" ReadOnly="true"  runat="server" Width="220px" CssClass="textbox  txtheight3">--Select--</asp:TextBox>
                                            <asp:Panel ID="Panel_college" runat="server" CssClass="multxtpanel" Height="200px"
                                                Width="200px">
                                                <asp:CheckBox ID="Cb_college" runat="server" Text="Select All" AutoPostBack="True"
                                                    OnCheckedChanged="Cb_college_CheckedChanged" />
                                                <asp:CheckBoxList ID="Cbl_college" runat="server" AutoPostBack="True" OnSelectedIndexChanged="Cbl_college_SelectedIndexChanged">
                                                </asp:CheckBoxList>
                                            </asp:Panel>
                                            <asp:PopupControlExtender ID="PopupControlExtender20" runat="server" TargetControlID="Txt_college"
                                                PopupControlID="Panel_college" Position="Bottom">
                                            </asp:PopupControlExtender>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="lbl_Type" runat="server" Text="Type"></asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txt_type2" runat="server" CssClass="textbox textbox1" Height="20px"
                                        Width="220px" Style="font-family: 'Book Antiqua'" Font-Bold="True" Font-Names="Book Antiqua"
                                        onfocus=" return display(this)" onblur="return get(this.value)" Font-Size="Medium"></asp:TextBox>
                                    <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender21" runat="server" TargetControlID="txt_type2"
                                        FilterType="UppercaseLetters,LowercaseLetters,numbers,Custom" ValidChars=" ">
                                    </asp:FilteredTextBoxExtender>
                                    <span style="color: Red;">*</span> <span style="font-weight: bold; font-size: larger;"
                                        id="msg1"></span>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="lbl_abtn" runat="server" Text="Abbreviation"></asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txt_abtn" runat="server" CssClass="nv textbox textbox1" Height="20px"
                                        Width="80px" Style="font-family: 'Book Antiqua'" Font-Bold="True" Font-Names="Book Antiqua"
                                        onfocus=" return display(this)" Font-Size="Medium"></asp:TextBox>
                                    <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" runat="server" TargetControlID="txt_abtn"
                                        FilterType="UppercaseLetters,LowercaseLetters,Custom" ValidChars=" ">
                                    </asp:FilteredTextBoxExtender>
                                    <span style="color: Red;">*</span>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="lbl_priority" runat="server" Text="Points"></asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txt_point" runat="server" MaxLength="1" text-transform="capitalize"
                                        CssClass="textbox textbox1" Height="20px" Width="24px" Style="font-family: 'Book Antiqua'"
                                        Font-Bold="True" Font-Names="Book Antiqua" onfocus=" return display(this)" Font-Size="Medium"></asp:TextBox>
                                    <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender2" runat="server" TargetControlID="txt_point"
                                        FilterType="Numbers,custom" ValidChars=".">
                                    </asp:FilteredTextBoxExtender>
                                    <span style="color: Red;">*</span>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                </td>
                                <td>
                                </td>
                            </tr>
                        </table>
                        <center>
                            <asp:Button ID="btn_Save" runat="server" Visible="true" Width="68px" Height="32px"
                                CssClass="textbox textbox1" Text="Save" Font-Bold="true" OnClientClick="return check()"
                                OnClick="btn_save_Click" />
                            <asp:Button ID="btndel" runat="server" Visible="false" Width="68px" Height="32px"
                                CssClass="textbox textbox1" Text="Delete" Font-Bold="true" OnClientClick="return check()"
                                OnClick="btndel_Click" />
                        </center>
                        <br />
                        <br />
                    </div>
                </center>
                <br />
                <br />
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
            </div>
            <div id="imgdiv3" runat="server" visible="false" style="height: 100%; z-index: 1000;
                width: 100%; background-color: rgba(54, 25, 25, .2); position: absolute; top: 0;
                left: 0px;">
                <center>
                    <div id="Div3" runat="server" class="table" style="background-color: White; height: 120px;
                        width: 238px; border: 5px solid #0CA6CA; border-top: 25px solid #0CA6CA; margin-top: 200px;
                        border-radius: 10px;">
                        <center>
                            <table style="height: 100px; width: 100%">
                                <tr>
                                    <td align="center">
                                        <asp:Label ID="lbl_alert" runat="server" Style="color: Red;" Font-Bold="true" Font-Size="Medium"></asp:Label>
                                        <asp:Label ID="Label1" runat="server" Visible="false" Font-Bold="true" ForeColor="Red"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <center>
                                            <asp:Button ID="btn_errorclose1" CssClass="textbox textbox1" Style="height: 28px;
                                                width: 65px;" OnClick="btn_errorclose1_Click" Text="Ok" runat="server" />
                                        </center>
                                    </td>
                                </tr>
                            </table>
                        </center>
                    </div>
                </center>
            </div>

             <div id="img4" runat="server" visible="false" style="height: 100%; z-index: 1000;
        width: 100%; background-color: rgba(54, 25, 25, .2); position: absolute; top: 0;
        left: 0px;">
        <center>
            <div id="panel_alert_warning" runat="server" class="table" style="background-color: White;
                height: 120px; width: 238px; border: 5px solid #0CA6CA; border-top: 25px solid #0CA6CA;
                margin-top: 200px; border-radius: 10px;">
                <center>
                    <table style="height: 100px; width: 100%">
                        <tr>
                            <td align="center">
                                <asp:Label ID="lbl_warning_alert" runat="server" Text="" Style="color: Red;" Font-Bold="true"
                                    Font-Size="Medium"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <center>
                                    <asp:Button ID="btn_warningmsg" CssClass="textbox textbox1" Style="height: 28px;
                                        width: 65px;" OnClick="btn_warningmsg_Click" Text="Yes" runat="server" />
                                         <asp:Button ID="btn_warning_exit" CssClass="textbox textbox1" Style="height: 28px;
                                        width: 65px;" OnClick="btn_warning_exit_Click" Text="No" runat="server" />
                            </td>
                        </tr>
                    </table>
                </center>
            </div>
        </center>
    </div>
   
</asp:Content>


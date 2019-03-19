<%@ Page Title="" Language="C#" MasterPageFile="~/FeedBackMOD/FeedbackSubSiteMaster.master"
    AutoEventWireup="true" CodeFile="Question_Master.aspx.cs" Inherits="Question_Master" %>

<%@ Register Src="~/Usercontrols/PrintMaster.ascx" TagName="printmaster" TagPrefix="Insproplus" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Assembly="FarPoint.Web.Spread" Namespace="FarPoint.Web.Spread" TagPrefix="FarPoint" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <link href="~/Styles/css/Registration.css" rel="stylesheet" type="text/css" />
    <link href="~/Styles/css/Commoncss.css" rel="Stylesheet" type="text/css" />
    <%--    <script src="//ajax.googleapis.com/ajax/libs/jquery/1.11.1/jquery.min.js"></script>--%>
    <script type="text/javascript" src="../Scripts/jquery-1.4.1.min.js"></script>
    <script type="text/javascript">


        function check() {
            var id;
            var id2;
            var empty = "";

            if (document.getElementById("<%=txt_qstn.ClientID %>").value.trim() == "" || document.getElementById("<%=txtacr.ClientID %>").value.trim() == "") {
                id = document.getElementById("<%=txt_qstn.ClientID %>");
                id2 = document.getElementById("<%=txtacr.ClientID %>");
                id.style.borderColor = 'Red';
                id2.style.borderColor = 'Red';
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
        function display1() {
            document.getElementById('<%=lbl_norec1.ClientID %>').innerHTML = "";
        }

        function get(txt1) {
            $.ajax({
                type: "POST",
                url: "Question_Master.aspx/CheckUserName",
                data: '{StoreName: "' + txt1 + '"}',
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: OnSuccess,
                failure: function (response) {
                    alert(response);
                }
            });

        }

        function get1(txt2) {

            $.ajax({
                type: "POST",
                url: "Question_Master.aspx/CheckUserName1",
                data: '{StoreName: "' + txt2 + '"}',
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: OnSuccess1,
                failure: function (response1) {
                    alert(response1);
                }
            });
        }

        function OnSuccess(response) {
            var mesg = $("#msg1")[0];
            switch (response.d) {
                case "0":
                    mesg.style.color = "green";
                    mesg.innerHTML = "Question Does Not exist";
                    break;
                case "1":
                    mesg.style.color = "red";
                    mesg.innerHTML = "Question Available";
                    document.getElementById('<%=txt_qstn.ClientID %>').value = "";
                    break;
                case "2":
                    mesg.style.color = "red";
                    mesg.innerHTML = "Please Enter  Question";
                    break;
                case "error":
                    mesg.style.color = "red";
                    mesg.innerHTML = "Error occurred";
                    break;
            }
        }

        function OnSuccess1(response1) {
            var mesg = $("#msg2")[0];
            switch (response1.d) {
                case "0":
                    mesg.style.color = "green";
                    mesg.innerHTML = "Question Acr Does Not exist";
                    break;
                case "1":
                    mesg.style.color = "red";
                    mesg.innerHTML = "Question Acr Available";
                    document.getElementById('<%=txtacr.ClientID %>').value = "";
                    break;
                case "2":
                    mesg.style.color = "red";
                    mesg.innerHTML = "Please Enter  Question Acr";
                    break;
                case "error":
                    mesg.style.color = "red";
                    mesg.innerHTML = "Error occurred";
                    break;
            }
        }

  
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <br />
    <div>
        <center>
            <center>
                <div>
                    <span class="fontstyleheader" style="color: Green">Question Master</span></div>
                <br />
            </center>
            <div class="maindivstyle">
                <br />
                <center>
                    <table class="maintablestyle" width="800px" height="40px">
                        <tr>
                            <td>
                                <asp:Label ID="lbl_college" runat="server" Text="College"></asp:Label>
                            </td>
                            <td>
                                <asp:UpdatePanel ID="UpdatePanel20" runat="server">
                                    <ContentTemplate>
                                        <asp:TextBox ID="Txt_college" Width=" 231px" ReadOnly="true" runat="server" CssClass="textbox  txtheight2">--Select--</asp:TextBox>
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
                            <td>
                                <asp:RadioButton ID="rb_Acad1" Width="103px" runat="server" GroupName="same2" Text="Academic"
                                    OnCheckedChanged="rb_Acad1_CheckedChanged" AutoPostBack="true" Checked="true">
                                </asp:RadioButton>
                            </td>
                            <td>
                                <asp:RadioButton ID="rb_Gend1" runat="server" Width="100px" GroupName="same2" Text="General"
                                    OnCheckedChanged="rb_Gend1_CheckedChanged" AutoPostBack="true"></asp:RadioButton>
                            </td>
                            <td>
                                <asp:RadioButton ID="rdb_obj" runat="server" Width="100px" AutoPostBack="true" Text="Objective"
                                    Checked="true" Visible="false" OnCheckedChanged="rdb_obj_checkedChange" />
                               
                            </td>
                            <td>
                             <asp:RadioButton ID="rdb_desc" runat="server" Width="112px" AutoPostBack="true" Text="Descriptive"
                                    Checked="false" Visible="false" OnCheckedChanged="rdb_desc_checkedChange" />
                            
                            </td>
                            <td>
                                <asp:Label ID="lbl_headersearch" runat="server" Text="Header"></asp:Label>
                            </td>
                            <td>
                                <asp:UpdatePanel ID="UpdatePanel3" runat="server">
                                    <ContentTemplate>
                                        <asp:TextBox ID="txt_headersearch" Width=" 150px" ReadOnly="true" runat="server"
                                            CssClass="textbox  txtheight2">--Select--</asp:TextBox>
                                        <asp:Panel ID="Panel_header1" runat="server" CssClass="multxtpanel" Height="200px"
                                            Width="200px">
                                            <asp:CheckBox ID="cb_header" runat="server" Text="Select All" AutoPostBack="True"
                                                OnCheckedChanged="cb_header_CheckedChanged" />
                                            <asp:CheckBoxList ID="cbl_header" runat="server" AutoPostBack="True" OnSelectedIndexChanged="cbl_header_SelectedIndexChanged">
                                            </asp:CheckBoxList>
                                        </asp:Panel>
                                        <asp:PopupControlExtender ID="PopupControlExtender3" runat="server" TargetControlID="txt_headersearch"
                                            PopupControlID="Panel_header1" Position="Bottom">
                                        </asp:PopupControlExtender>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>
                            <td>
                                <asp:Button ID="btn_search" runat="server" CssClass="textbox btn2" Text="Search"
                                    OnClick="btn_search_Click" />
                            </td>
                            <td>
                                <asp:Button ID="btn_Add1" runat="server" CssClass="textbox btn2" Text="Add New" OnClick="btnAdd1_Click" />
                            </td>
                        </tr>
                    </table>
                </center>
                <br />
                <center>
                    <div id="div1" visible="false" runat="server" class="spreadborder" style="width: 866px;
                        height: 330px; overflow: auto; background-color: White; border-radius: 10px;">
                        <FarPoint:FpSpread ID="FpSpread1" runat="server" Visible="false" BorderStyle="Solid"
                            BorderWidth="0px" Width="866px" Height="500px" Style="overflow: auto; border: 0px solid #999999;
                            border-radius: 10px; background-color: White; box-shadow: 0px 0px 8px #999999;"
                            CssClass="spreadborder" ShowHeaderSelection="false" OnCellClick="FpSpread1_OnCellClick"
                            OnPreRender="FpSpread1_Selectedindexchange">
                            <Sheets>
                                <FarPoint:SheetView SheetName="Sheet1">
                                </FarPoint:SheetView>
                            </Sheets>
                        </FarPoint:FpSpread>
                        <br />
                        <br />
                    </div>
                    <div id="rptprint1" runat="server" visible="false">
                        <br />
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
                </center>
                <br />
            </div>
    </div>
    <center>
        <div id="addnew" runat="server" visible="false" style="height: 50em; z-index: 1000;
            width: 100%; background-color: rgba(54, 25, 25, .2); position: absolute; top: 0;
            left: 0;">
            <asp:ImageButton ID="ImageButton2" runat="server" Width="40px" Height="40px" ImageUrl="~/images/close.png"
                Style="height: 30px; width: 30px; position: absolute; margin-top: 82px; margin-left: 355px;"
                OnClick="imagebtnpopclose1_Click" />
            <br />
            <br />
            <br />
            <br />
            <br />
            <div style="background-color: White; height: 571px; width: 738px; overflow: scroll;
                border: 5px solid #0CA6CA; border-top: 30px solid #0CA6CA; border-radius: 10px;">
                <center>
                    <br />
                    <%--  <asp:Label ID="lbl_Addquestion" runat="server" Style=" font-size: large; color: green"
                        Text="Add question"></asp:Label>--%>
                    <div>
                        <span class="fontstyleheader" style="color: Green;">Question Master</span></div>
                    <br />
                </center>
                <div>
                    <center>
                        <table>
                            <tr>
                                <td colspan="2">
                                    <asp:RadioButton ID="rb_Acad2" runat="server" GroupName="same1" AutoPostBack="true"
                                        OnCheckedChanged="rb_Acad2_CheckedChanged" Text="Academic" Checked="true"></asp:RadioButton>
                                    <asp:RadioButton ID="rb_Gend2" runat="server" GroupName="same1" AutoPostBack="true"
                                        OnCheckedChanged="rb_Gend2_CheckedChanged" Text="General"></asp:RadioButton>
                                    <asp:RadioButton ID="rdbobjective" runat="server" AutoPostBack="true" Text="Objective"
                                        Checked="true" Visible="false" OnCheckedChanged="rdbobjective_checkedChange" />
                                    <asp:RadioButton ID="rdbdescriptive" runat="server" AutoPostBack="true" Text="Descriptive"
                                        Checked="false" Visible="false" OnCheckedChanged="rdbdescriptive_checkedChange" />
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="lbl_college1" runat="server" Text="College"></asp:Label>
                                </td>
                                <td>
                                    <asp:UpdatePanel ID="UpdatePanel6" runat="server">
                                        <ContentTemplate>
                                            <asp:TextBox ID="Txt_college1" Width=" 231px" ReadOnly="true" runat="server" CssClass="textbox  txtheight2">--Select--</asp:TextBox>
                                            <asp:Panel ID="Panel_college1" runat="server" CssClass="multxtpanel" Height="100px"
                                                Width="300px">
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
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="lbl_header" runat="server" Text="Header"></asp:Label>
                                </td>
                                <td>
                                    <asp:Button ID="btn_plus" runat="server" Text="+" CssClass="textbox textbox1" Font-Bold="true"
                                        Font-Size="Medium" Font-Names="Book Antiqua" OnClick="btnplus_Click" Height="33px"
                                        Width="35px" />
                                    <asp:DropDownList ID="ddl_group" runat="server" Height="35px" onfocus=" return display(this)"
                                        CssClass="textbox textbox1 ddlstyle ddlheight4">
                                    </asp:DropDownList>
                                    <asp:Button ID="btn_minus" runat="server" Text="-" Font-Bold="true" Font-Size="Medium"
                                        Height="33px" Width="35px" Font-Names="Book Antiqua" CssClass="textbox textbox1"
                                        OnClick="btnminus_Click" />
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="lbl_qstn1" runat="server" Text="Question"></asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txt_qstn" runat="server" Height="25px" onfocus=" return display(this)"
                                        onblur="return get(this.value)" CssClass="textbox textbox1" Width="453px" Style="font-family: 'Book Antiqua'"
                                        Font-Bold="True" Font-Names="Book Antiqua"></asp:TextBox>
                                    <span style="color: Red;">*</span> <span style="font-weight: bold; font-size: larger;"
                                        id="msg1"></span>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="lblacr" runat="server" Text="Question Acronym"></asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtacr" runat="server" Height="25px" onfocus=" return display(this)"
                                        onblur="return get1(this.value)" CssClass="textbox textbox1" Width="250px" Style="font-family: 'Book Antiqua'"
                                        Font-Bold="True" Font-Names="Book Antiqua"></asp:TextBox>
                                    <span style="color: Red;">*</span> <span style="font-weight: bold; font-size: larger;"
                                        id="msg2"></span>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2">
                                    <asp:Label ID="lbl_tough" runat="server" Text="Toughness :"></asp:Label>&nbsp
                                    <asp:RadioButton ID="rb_easy" Width="88px" runat="server" GroupName="same" Text="Easy"
                                        OnCheckedChanged="rb_easy_CheckedChanged" AutoPostBack="false" Checked="true">
                                    </asp:RadioButton>
                                    <asp:RadioButton ID="rb_Medium" runat="server" Width="100px" GroupName="same" Text="Medium"
                                        OnCheckedChanged="rb_Medium_CheckedChanged" AutoPostBack="false"></asp:RadioButton>
                                    <asp:RadioButton ID="rdb_Difficult" Width="88px" runat="server" GroupName="same"
                                        Text="Difficult" OnCheckedChanged="rdb_Difficult_CheckedChanged" AutoPostBack="false">
                                    </asp:RadioButton>
                                    <asp:RadioButton ID="rdb_VeryDifficult" runat="server" Width="115px" GroupName="same"
                                        Text="VeryDifficult" OnCheckedChanged="rdb_VeryDifficult_CheckedChanged" AutoPostBack="false">
                                    </asp:RadioButton>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                </td>
                            </tr>
                        </table>
                    </center>
                    <center>
                        <div id="Div_Answer">
                            <table>
                                <tr>
                                    <td>
                                        <asp:Label ID="lbloption" runat="server" Text="Option"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:CheckBox ID="cb_option" runat="server" Text="Select All" AutoPostBack="True"
                                            fontsize="small" OnCheckedChanged="cb_option_CheckedChanged" />
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="4">
                                        <div style="border: thin groove #C0C0C0; overflow: auto;">
                                            <asp:CheckBoxList ID="cbl_option" runat="server" fontsize="small" RepeatColumns="5">
                                            </asp:CheckBoxList>
                                        </div>
                                    </td>
                                </tr>
                            </table>
                        </div>
                    </center>
                    <br />
                    <center>
                        <div>
                            <center>
                                <asp:Button ID="btn_saveqtion" runat="server" Visible="true" Width="68px" Height="32px"
                                    CssClass="textbox textbox1" Text="Save" OnClientClick="return check()" OnClick="btn_savequstion_Click" />
                                <asp:Button ID="btndel" runat="server" Visible="true" Width="68px" Height="32px"
                                    CssClass="textbox textbox1" Text="Delete" OnClientClick="return check()" OnClick="btndel_Click" />
                                <asp:Button ID="btn_exit" runat="server" Visible="true" Width="68px" Height="32px"
                                    CssClass="textbox textbox1" Text="Exit" OnClick="btn_exit_Click" /><br />
                            </center>
                        </div>
                    </center>
                </div>
            </div>
        </div>
    </center>
    <%---------end of popup--------%>
    <div id="imgdiv5" runat="server" visible="false" style="height: 100%; z-index: 1000;
        width: 100%; background-color: rgba(54, 25, 25, .2); position: absolute; top: 0;
        left: 0px;">
        <center>
            <div id="panel_header" runat="server" visible="false" class="table" style="background-color: White;
                height: auto; width: 435px; border: 5px solid #0CA6CA; border-top: 25px solid #0CA6CA;
                margin-top: 200px; border-radius: 10px;">
                <table>
                    <tr>
                        <td align="center">
                            <span class="fontstyleheader" style="color: Green">Header</span>
                        </td>
                    </tr>
                    <tr>
                        <td align="center">
                            <asp:TextBox ID="txt_header" runat="server" Height="25px" onfocus=" return display(this)"
                                Style="text-transform: capitalize;" CssClass="textbox textbox1" Width="380px"></asp:TextBox>
                        </td>
                    </tr>
                    <br />
                    <tr>
                        <td align="center">
                            <br />
                            <asp:Button ID="btn_addheader" runat="server" Visible="true" Width="58px" Height="32px"
                                CssClass="textbox textbox1" Text="Add" OnClientClick="return checkadd()" OnClick="btn_addheader_Click" />
                            <asp:Button ID="btn_exitheader" runat="server" Visible="true" Width="68px" Height="32px"
                                CssClass="textbox textbox1" Text="Exit" OnClick="btn_exitheader_Click" />
                            <br />
                            <br />
                        </td>
                    </tr>
                </table>
            </div>
        </center>
    </div>
    <%--************--%>
    <%--************--%>
    <div id="imgdiv2" runat="server" visible="false" style="height: 100%; z-index: 1000;
        width: 100%; background-color: rgba(54, 25, 25, .2); position: absolute; top: 0;
        left: 0px;">
        <center>
            <div id="panel_erroralert" runat="server" class="table" style="background-color: White;
                height: 120px; width: 238px; border: 5px solid #0CA6CA; border-top: 25px solid #0CA6CA;
                margin-top: 200px; border-radius: 10px;">
                <center>
                    <table style="height: 100px; width: 100%">
                        <tr>
                            <td align="center">
                                <asp:Label ID="lbl_erroralert" runat="server" Text="" Style="color: Red;" Font-Bold="true"
                                    Font-Size="Medium"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <center>
                                    <asp:Button ID="btn_erroralert" CssClass="textbox textbox1" Style="height: 28px;
                                        width: 65px;" OnClick="btnerrclose_Click" Text="Ok" runat="server" />
                                </center>
                            </td>
                        </tr>
                    </table>
                </center>
            </div>
        </center>
    </div>
    <div id="imgdiv3" runat="server" visible="false" style="height: 100%; z-index: 1000;
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
    <div id="imgdiv4" runat="server" visible="false" style="height: 100%; z-index: 1000;
        width: 100%; background-color: rgba(54, 25, 25, .2); position: absolute; top: 0;
        left: 0px;">
        <center>
            <div id="Div5" runat="server" class="table" style="background-color: White; height: 120px;
                width: 238px; border: 5px solid #0CA6CA; border-top: 25px solid #0CA6CA; margin-top: 200px;
                border-radius: 10px;">
                <center>
                    <table style="height: 100px; width: 100%">
                        <tr>
                            <td align="center">
                                <asp:Label ID="lbl_warningmsghed" runat="server" Text="" Style="color: Red;" Font-Bold="true"
                                    Font-Size="Medium"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <center>
                                    <asp:Button ID="btn_warningmsghed" CssClass="textbox textbox1" Style="height: 28px;
                                        width: 65px;" OnClick="btn_warningmsghed_Click" Text="Yes" runat="server" />
                                    <asp:Button ID="btn_warningmsghed_exit" CssClass="textbox textbox1" Style="height: 28px;
                                        width: 65px;" OnClick="btn_warningmsghed_exit_Click" Text="No" runat="server" />
                                </center>
                            </td>
                        </tr>
                    </table>
                </center>
            </div>
        </center>
    </div>
</asp:Content>

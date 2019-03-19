<%@ Page Title="" Language="C#" MasterPageFile="~/FeedBackMOD/FeedbackSubSiteMaster.master"
    AutoEventWireup="true" CodeFile="FeedBackquestion_type.aspx.cs" Inherits="FeedBackMOD_FeedBackquestion_type" %>

<%@ Register Src="~/Usercontrols/PrintMaster.ascx" TagName="printmaster" TagPrefix="Insproplus" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Assembly="FarPoint.Web.Spread" Namespace="FarPoint.Web.Spread" TagPrefix="FarPoint" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <link href="~/Styles/css/Commoncss.css" rel="Stylesheet" type="text/css" />
    <script language="javascript">
        function display1() {
            document.getElementById('<%=lbl_norec1.ClientID %>').innerHTML = "";
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <div>
        <center>
            <div>
                <div>
                    <asp:Label ID="Label1" runat="server" CssClass="fontstyleheader" ForeColor="Green"
                        Text="Question Type Matching"></asp:Label>
                </div>
                <br />
                <table class="maintablestyle ">
                    <tr>
                        <td>
                            <asp:Label ID="lbl_college" runat="server" Text="College"></asp:Label>
                        </td>
                        <td>
                            <asp:DropDownList ID="ddl_college1" runat="server" Height="35px" CssClass=" textbox1 ddlstyle ddlheight4">
                            </asp:DropDownList>
                        </td>
                        <td>
                            Feedback Name
                        </td>
                        <td>
                            <asp:DropDownList ID="ddl_feedback1" runat="server" Height="35px" CssClass=" textbox1 ddlstyle ddlheight4"
                                AutoPostBack="true" OnSelectedIndexChanged="ddl_feedback1_onselectedindexchanged">
                            </asp:DropDownList>
                        </td>
                        <td>
                            Header Name
                        </td>
                        <td>
                            <asp:UpdatePanel ID="UpdatePanel4" runat="server">
                                <ContentTemplate>
                                    <asp:TextBox ID="txt_header1" Width=" 100px" ReadOnly="true" runat="server" CssClass="textbox  txtheight2">--Select--</asp:TextBox>
                                    <asp:Panel ID="Panel3" runat="server" CssClass="multxtpanel" Height="200px" Width="200px">
                                        <asp:CheckBox ID="cb_header1" runat="server" Text="Select All" AutoPostBack="True"
                                            OnCheckedChanged="cb_header1_CheckedChanged" />
                                        <asp:CheckBoxList ID="cbl_header1" runat="server" AutoPostBack="True" OnSelectedIndexChanged="cbl_header1_SelectedIndexChanged">
                                        </asp:CheckBoxList>
                                    </asp:Panel>
                                    <asp:PopupControlExtender ID="PopupControlExtender4" runat="server" TargetControlID="txt_header1"
                                        PopupControlID="Panel3" Position="Bottom">
                                    </asp:PopupControlExtender>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </td>
                        <td>
                            Question
                        </td>
                        <td>
                            <asp:UpdatePanel ID="UpdatePanel5" runat="server">
                                <ContentTemplate>
                                    <asp:TextBox ID="txt_question1" Width=" 150px" ReadOnly="true" runat="server" CssClass="textbox  txtheight2">--Select--</asp:TextBox>
                                    <asp:Panel ID="Panel4" runat="server" CssClass="multxtpanel" Height="200px" Width="200px">
                                        <asp:CheckBox ID="cb_question1" runat="server" Text="Select All" AutoPostBack="True"
                                            OnCheckedChanged="cb_question1_CheckedChanged" />
                                        <asp:CheckBoxList ID="cbl_question1" runat="server" AutoPostBack="True" OnSelectedIndexChanged="cbl_question1_SelectedIndexChanged">
                                        </asp:CheckBoxList>
                                    </asp:Panel>
                                    <asp:PopupControlExtender ID="PopupControlExtender5" runat="server" TargetControlID="txt_question1"
                                        PopupControlID="Panel4" Position="Bottom">
                                    </asp:PopupControlExtender>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            Option
                        </td>
                        <td>
                            <asp:UpdatePanel ID="UpdatePanel6" runat="server">
                                <ContentTemplate>
                                    <asp:TextBox ID="txt_option1" Width=" 100px" ReadOnly="true" runat="server" CssClass="textbox  txtheight2">--Select--</asp:TextBox>
                                    <asp:Panel ID="Panel5" runat="server" CssClass="multxtpanel" Height="200px" Width="200px">
                                        <asp:CheckBox ID="cb_option1" runat="server" Text="Select All" AutoPostBack="True"
                                            OnCheckedChanged="cb_option1_CheckedChanged" />
                                        <asp:CheckBoxList ID="cbl_option1" runat="server" AutoPostBack="True" OnSelectedIndexChanged="cbl_option1_SelectedIndexChanged">
                                        </asp:CheckBoxList>
                                    </asp:Panel>
                                    <asp:PopupControlExtender ID="PopupControlExtender6" runat="server" TargetControlID="txt_option1"
                                        PopupControlID="Panel5" Position="Bottom">
                                    </asp:PopupControlExtender>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </td>
                        <td>
                            <asp:Button ID="btn_go" runat="server" CssClass="textbox btn1" Text="Go" OnClick="btn_go_Click" />
                            <asp:Button ID="btn_addnew" runat="server" CssClass="textbox btn2" Text="Add New"
                                OnClick="btn_addnew_Click" />
                        </td>
                    </tr>
                </table>
                <br />
                <asp:Label ID="lbl_error" runat="server" ForeColor="Red" Visible="false"></asp:Label>
                <br />
                <center>
                    <div>
                        <FarPoint:FpSpread ID="FpSpread1" runat="server" Visible="false" BorderStyle="Solid"
                            BorderWidth="0px" Width="866px" Height="500px" Style="overflow: auto; border: 0px solid #999999;
                            border-radius: 10px; background-color: White; box-shadow: 0px 0px 8px #999999;"
                            CssClass="spreadborder" ShowHeaderSelection="false">
                            <Sheets>
                                <FarPoint:SheetView SheetName="Sheet1">
                                </FarPoint:SheetView>
                            </Sheets>
                        </FarPoint:FpSpread>
                        <br />
                        <br />
                        <div id="rptprint1" runat="server" visible="false">
                            <br />
                            <asp:Label ID="lbl_norec1" runat="server" ForeColor="Red" Text="Please Enter Your Report Name"
                                Visible="false"></asp:Label>
                            <asp:Label ID="lblrptname1" runat="server" Text="Report Name"></asp:Label>
                            <asp:TextBox ID="txtexcelname1" runat="server" CssClass="textbox textbox1" Height="20px"
                                Width="180px" onkeypress="display1()"></asp:TextBox>
                            <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender9" runat="server" TargetControlID="txtexcelname1"
                                FilterType="LowercaseLetters,UppercaseLetters,Numbers,Custom" ValidChars="(),.[]_"
                                InvalidChars="/\">
                            </asp:FilteredTextBoxExtender>
                            <asp:Button ID="btnExcel1" runat="server" OnClick="btnExcel1_Click" Text="Export To Excel"
                                Width="127px" Height="30px" CssClass="textbox textbox1" />
                            <asp:Button ID="btnprintmaster1" runat="server" Text="Print" OnClick="btnprintmaster1_Click"
                                Width="60px" Height="30px" CssClass="textbox textbox1" />
                            <Insproplus:printmaster runat="server" ID="Printcontrol1" Visible="false" />
                        </div>
                    </div>
                </center>
                <br />
                <br />
            </div>
            <div>
                <center>
                    <div id="addnew" runat="server" visible="false" style="height: 50em; z-index: 1000;
                        width: 100%; background-color: rgba(54, 25, 25, .2); position: absolute; top: 0;
                        left: 0;">
                        <asp:ImageButton ID="ImageButton2" runat="server" Width="40px" Height="40px" ImageUrl="~/images/close.png"
                            Style="height: 30px; width: 30px; position: absolute; margin-top: 81px; margin-left: 439px;"
                            OnClick="imagebtnpopclose1_Click" />
                        <br />
                        <br />
                        <br />
                        <br />
                        <div style="background-color: White; height: auto; width: 912px; border: 5px solid #0CA6CA;
                            border-top: 30px solid #0CA6CA; border-radius: 10px;">
                            <center>
                                <br />
                                <div>
                                    <span class="fontstyleheader" style="color: Green;">Question Type Matching</span></div>
                                <br />
                            </center>
                            <table>
                                <%-- <tr>
                   <td colspan="2">
                        <asp:RadioButton ID="rb_Acad" runat="server" GroupName="same1" AutoPostBack="true"
                            OnCheckedChanged="rb_Acad_CheckedChanged" Text="Academic" Checked="true"></asp:RadioButton>
                        <asp:RadioButton ID="rb_Gend" runat="server" GroupName="same1" AutoPostBack="true"
                            OnCheckedChanged="rb_Gend_CheckedChanged" Text="General"></asp:RadioButton>
                    </td>
                </tr>--%>
                                <tr>
                                    <td>
                                        <asp:Label ID="lbl_college1" runat="server" Text="College"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="ddl_college" runat="server" Height="35px" CssClass=" textbox1 ddlstyle ddlheight4">
                                        </asp:DropDownList>
                                    </td>
                                    <td>
                                        Feedback Name
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="ddl_feedback" runat="server" Height="35px" CssClass=" textbox1 ddlstyle ddlheight4"
                                            AutoPostBack="true" OnSelectedIndexChanged="ddl_feedback_onselectedindexchanged">
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        Header Name
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="ddl_header" runat="server" Height="35px" CssClass=" textbox1 ddlstyle ddlheight4"
                                            AutoPostBack="true" OnSelectedIndexChanged="ddl_header_selectedindexChanged">
                                        </asp:DropDownList>
                                        <%--<asp:UpdatePanel ID="UpdatePanel3" runat="server">
                                            <ContentTemplate>
                                                <asp:TextBox ID="txt_header" Width=" 150px" ReadOnly="true" runat="server" CssClass="textbox  txtheight2">--Select--</asp:TextBox>
                                                <asp:Panel ID="Panel_header1" runat="server" CssClass="multxtpanel" Height="200px"
                                                    Width="200px">
                                                    <asp:CheckBox ID="cb_header" runat="server" Text="Select All" AutoPostBack="True"
                                                        OnCheckedChanged="cb_header_CheckedChanged" />
                                                    <asp:CheckBoxList ID="cbl_header" runat="server" AutoPostBack="True" OnSelectedIndexChanged="cbl_header_SelectedIndexChanged">
                                                    </asp:CheckBoxList>
                                                </asp:Panel>
                                                <asp:PopupControlExtender ID="PopupControlExtender3" runat="server" TargetControlID="txt_header"
                                                    PopupControlID="Panel_header1" Position="Bottom">
                                                </asp:PopupControlExtender>
                                            </ContentTemplate>
                                        </asp:UpdatePanel>--%>
                                    </td>
                                    <td>
                                        Question
                                    </td>
                                    <td>
                                        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                                            <ContentTemplate>
                                                <asp:TextBox ID="txt_question" Width=" 150px" ReadOnly="true" runat="server" CssClass="textbox  txtheight2">--Select--</asp:TextBox>
                                                <asp:Panel ID="Panel1" runat="server" CssClass="multxtpanel" Height="200px" Width="200px">
                                                    <asp:CheckBox ID="cb_question" runat="server" Text="Select All" AutoPostBack="True"
                                                        OnCheckedChanged="cb_question_CheckedChanged" />
                                                    <asp:CheckBoxList ID="cbl_question" runat="server" AutoPostBack="True" OnSelectedIndexChanged="cbl_question_SelectedIndexChanged">
                                                    </asp:CheckBoxList>
                                                </asp:Panel>
                                                <asp:PopupControlExtender ID="PopupControlExtender1" runat="server" TargetControlID="txt_question"
                                                    PopupControlID="Panel1" Position="Bottom">
                                                </asp:PopupControlExtender>
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        Option
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <%--<asp:UpdatePanel ID="UpdatePanel2" runat="server">
                                            <ContentTemplate>
                                               <asp:TextBox ID="txt_option" Width=" 150px" ReadOnly="true" runat="server" CssClass="textbox  txtheight2">--Select--</asp:TextBox>
                                                <asp:Panel ID="Panel2" runat="server" CssClass="multxtpanel" Height="200px" Width="200px">--%>
                                        <asp:CheckBox ID="cb_option" runat="server" Text="Select All" AutoPostBack="True"
                                            OnCheckedChanged="cb_option_CheckedChanged" />
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="4">
                                        <div style="border: thin groove #C0C0C0; overflow: auto;">
                                            <asp:CheckBoxList ID="cbl_option" runat="server" RepeatColumns="5">
                                            </asp:CheckBoxList>
                                        </div>
                                        <%-- </asp:Panel>
                                                <asp:PopupControlExtender ID="PopupControlExtender2" runat="server" TargetControlID="txt_option"
                                                    PopupControlID="Panel2" Position="Bottom">
                                                </asp:PopupControlExtender>
                                            </ContentTemplate>
                                        </asp:UpdatePanel>--%>
                                    </td>
                                </tr>
                            </table>
                            <br />
                            <br />
                            <center>
                                <div>
                                    <asp:Button ID="btn_save" runat="server" CssClass="textbox btn2" Text="Save" OnClick="btn_Save_Click" />
                                    <br />
                                    <br />
                                    <asp:Label ID="lbl_error1" runat="server" ForeColor="Red" Visible="false"></asp:Label>
                                </div>
                            </center>
                        </div>
                    </div>
                </center>
            </div>
        </center>
    </div>
</asp:Content>

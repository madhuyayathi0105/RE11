<%@ Page Title="" Language="C#" MasterPageFile="~/FeedBackMOD/FeedbackSubSiteMaster.master"
    AutoEventWireup="true" CodeFile="FeedBack_Master.aspx.cs" Inherits="FeedBack_Master" %>

<%@ Register Src="~/Usercontrols/PrintMaster.ascx" TagName="printmaster" TagPrefix="Insproplus" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Assembly="FarPoint.Web.Spread" Namespace="FarPoint.Web.Spread" TagPrefix="FarPoint" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <link href="~/Styles/css/Registration.css" rel="stylesheet" type="text/css" />
    <link href="~/Styles/css/Commoncss.css" rel="Stylesheet" type="text/css" />
    <style type="text/css">
        .nv
        {
            text-transform: uppercase;
        }
    </style>
    <script type="text/javascript">

        function check() {
            var id = "";

            id = document.getElementById("<%=txt_FBName.ClientID %>").value;
            if (id.trim() == "") {
                id = document.getElementById("<%=txt_FBName.ClientID %>");
                id.style.borderColor = 'Red';
                empty = "E";
            }
            var chkidcomman = document.getElementById("<%=chk_includecommon.ClientID %>");
            var chkidrandom = document.getElementById("<%=chk_random.ClientID %>");

            if (chkidcomman.checked == true && chkidrandom.checked == false) {

                id = document.getElementById("<%=txt_fb_acr.ClientID %>").value;

                if (id.trim() == "") {
                    id = document.getElementById("<%=txt_fb_acr.ClientID %>");
                    id.style.borderColor = 'Red';
                    empty = "E";
                }
                id = document.getElementById("<%=txt_running_series.ClientID %>").value;

                if (id.trim() == "") {
                    id = document.getElementById("<%=txt_running_series.ClientID %>");
                    id.style.borderColor = 'Red';
                    empty = "E";
                }
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
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <br />
    <div>
        <center>
            <div>
                <span class="fontstyleheader" style="color: Green">FeedBack Master</span></div>
        </center>
    </div>
    <br />
    <div class="maindivstyle" width="610px">
        <br />
        <center>
            <center>
                <asp:RadioButton ID="rb_Acad1" Width="95px" runat="server" GroupName="same" Text="Academic"
                    OnCheckedChanged="rb_Acad1_CheckedChanged" AutoPostBack="true" Checked="true">
                </asp:RadioButton>
                <asp:RadioButton ID="rb_Gend1" runat="server" Width="100px" GroupName="same" Text="General"
                    OnCheckedChanged="rb_Gend1_CheckedChanged" AutoPostBack="true"></asp:RadioButton>
            </center>
            <br />
            <div id="Acd" runat="server" visible="true">
                <table class="maintablestyle" width="840px" height="40px">
                    <tr>
                        <td>
                            <asp:Label ID="lbl_college1" runat="server" Text="College"></asp:Label>
                        </td>
                        <td>
                            <asp:UpdatePanel ID="UpdatePanel6" runat="server">
                                <ContentTemplate>
                                    <asp:TextBox ID="Txt_college1" Width=" 90px" ReadOnly="true" runat="server" CssClass="textbox  txtheight2">--Select--</asp:TextBox>
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
                            <asp:Label ID="lbl_Batchyear1" runat="server" Text="Batch Year"></asp:Label>
                        </td>
                        <td>
                            <asp:UpdatePanel ID="Updp_Batchyear1" runat="server">
                                <ContentTemplate>
                                    <asp:TextBox ID="txt_batch1" ReadOnly="true" Width=" 90px" runat="server" CssClass="textbox  txtheight2">--Select--</asp:TextBox>
                                    <asp:Panel ID="Panel_Batchyear1" runat="server" CssClass="multxtpanel" Height="200px"
                                        Width="200px">
                                        <asp:CheckBox ID="cb_batch1" runat="server" Text="Select All" AutoPostBack="True"
                                            OnCheckedChanged="cb_batch1_CheckedChanged" />
                                        <asp:CheckBoxList ID="cbl_batch1" runat="server" AutoPostBack="True" OnSelectedIndexChanged="cbl_batch1_SelectedIndexChanged">
                                        </asp:CheckBoxList>
                                    </asp:Panel>
                                    <asp:PopupControlExtender ID="PopupControlExtender7" runat="server" TargetControlID="txt_batch1"
                                        PopupControlID="Panel_Batchyear1" Position="Bottom">
                                    </asp:PopupControlExtender>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </td>
                        <td>
                            <asp:Label ID="lbl_Degree1" Width="50px" runat="server" Text="Degree"></asp:Label>
                        </td>
                        <td>
                            <asp:UpdatePanel ID="Updp_Degree1" runat="server">
                                <ContentTemplate>
                                    <asp:TextBox ID="txt_degree1" Width=" 90px" runat="server" CssClass="textbox  txtheight2"
                                        ReadOnly="true">--Select--</asp:TextBox>
                                    <asp:Panel ID="Panel_Degree1" runat="server" CssClass="multxtpanel" Height="200px"
                                        Width="200px">
                                        <asp:CheckBox ID="cb_degree1" runat="server" Text="Select All" AutoPostBack="True"
                                            OnCheckedChanged="cb_degree1_CheckedChanged" />
                                        <asp:CheckBoxList ID="cbl_degree1" runat="server" AutoPostBack="True" OnSelectedIndexChanged="cbl_degree1_SelectedIndexChanged">
                                        </asp:CheckBoxList>
                                    </asp:Panel>
                                    <asp:PopupControlExtender ID="PopupControlExtender22" runat="server" TargetControlID="txt_degree1"
                                        PopupControlID="Panel_Degree1" Position="Bottom">
                                    </asp:PopupControlExtender>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </td>
                        <td>
                            <asp:Label ID="lbl_dpt1" runat="server" Width="75px" Text="Department"></asp:Label>
                        </td>
                        <td>
                            <asp:UpdatePanel ID="UpdatePanel7" runat="server">
                                <ContentTemplate>
                                    <asp:TextBox ID="txt_branch1" Width=" 128px" ReadOnly="true" runat="server" CssClass="textbox  txtheight2">--Select--</asp:TextBox>
                                    <asp:Panel ID="Panel_dpt1" runat="server" CssClass="multxtpanel" Height="200px" Width="200px">
                                        <asp:CheckBox ID="cb_branch1" runat="server" Text="Select All" AutoPostBack="True"
                                            OnCheckedChanged="cb_branch1_CheckedChanged" />
                                        <asp:CheckBoxList ID="cbl_branch1" runat="server" AutoPostBack="True" OnSelectedIndexChanged="cbl_branch1_SelectedIndexChanged">
                                        </asp:CheckBoxList>
                                    </asp:Panel>
                                    <asp:PopupControlExtender ID="PopupControlExtender10" runat="server" TargetControlID="txt_branch1"
                                        PopupControlID="Panel_dpt1" Position="Bottom">
                                    </asp:PopupControlExtender>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label ID="lbl_sem1" runat="server" Text="Sem"></asp:Label>
                        </td>
                        <td>
                            <asp:UpdatePanel ID="UpdatePanel8" runat="server">
                                <ContentTemplate>
                                    <asp:TextBox ID="txt_sem1" Width=" 90px" ReadOnly="true" runat="server" CssClass="textbox  txtheight2">--Select--</asp:TextBox>
                                    <asp:Panel ID="Panel_Sem1" runat="server" CssClass="multxtpanel" Height="200px" Width="200px">
                                        <asp:CheckBox ID="cb_sem1" runat="server" Text="Select All" AutoPostBack="True" OnCheckedChanged="cb_sem1_CheckedChanged" />
                                        <asp:CheckBoxList ID="cbl_sem1" runat="server" AutoPostBack="True" OnSelectedIndexChanged="cbl_sem1_SelectedIndexChanged">
                                        </asp:CheckBoxList>
                                    </asp:Panel>
                                    <asp:PopupControlExtender ID="PopupControlExtender11" runat="server" TargetControlID="txt_sem1"
                                        PopupControlID="Panel_Sem1" Position="Bottom">
                                    </asp:PopupControlExtender>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </td>
                        <td>
                            <asp:Label ID="lbl_Sec1" runat="server" Text="Sec"></asp:Label>
                        </td>
                        <td>
                            <asp:UpdatePanel ID="UpdatePanel9" runat="server">
                                <ContentTemplate>
                                    <asp:TextBox ID="txt_sec1" Width=" 90px" ReadOnly="true" runat="server" CssClass="textbox  txtheight2">--Select--</asp:TextBox>
                                    <asp:Panel ID="Panel_Sec1" runat="server" CssClass="multxtpanel" Height="200px" Width="200px">
                                        <asp:CheckBox ID="cb_sec1" runat="server" Text="Select All" AutoPostBack="True" OnCheckedChanged="cb_sec1_CheckedChanged" />
                                        <asp:CheckBoxList ID="cbl_sec1" runat="server" AutoPostBack="True" OnSelectedIndexChanged="cbl_sec1_SelectedIndexChanged">
                                        </asp:CheckBoxList>
                                    </asp:Panel>
                                    <asp:PopupControlExtender ID="PopupControlExtender12" runat="server" TargetControlID="txt_sec1"
                                        PopupControlID="Panel_Sec1" Position="Bottom">
                                    </asp:PopupControlExtender>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </td>
                        <td>
                            <asp:Label ID="lbl_FbName1" runat="server" Text="FeedBack Name"></asp:Label>
                        </td>
                        <td>
                            <asp:UpdatePanel ID="UpdatePanel5" runat="server">
                                <ContentTemplate>
                                    <asp:TextBox ID="Txt_FbName1" Width=" 114px" ReadOnly="true" runat="server" CssClass="textbox  txtheight2">--Select--</asp:TextBox>
                                    <asp:Panel ID="Panel_FbName1" runat="server" CssClass="multxtpanel" Height="200px"
                                        Width="200px">
                                        <asp:CheckBox ID="Cb_FbName1" runat="server" Text="Select All" AutoPostBack="True"
                                            OnCheckedChanged="Cb_FbName1_CheckedChanged" />
                                        <asp:CheckBoxList ID="Cbl_FbName1" runat="server" AutoPostBack="True" OnSelectedIndexChanged="Cbl_FbName1_SelectedIndexChanged">
                                        </asp:CheckBoxList>
                                    </asp:Panel>
                                    <asp:PopupControlExtender ID="PopupControlExtender5" runat="server" TargetControlID="Txt_FbName1"
                                        PopupControlID="Panel_FbName1" Position="Bottom">
                                    </asp:PopupControlExtender>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </td>
                        <td>
                            <asp:Button ID="btn_Search" runat="server" CssClass="textbox btn2" Text="Search"
                                OnClick="btn_Search_Click" Visible="true" />
                        </td>
                        <td>
                            <asp:Button ID="btn_Add" runat="server" CssClass="textbox btn2" Text="Add New" OnClick="btn_Add_Click" />
                        </td>
                    </tr>
                </table>
            </div>
            <br />
            <FarPoint:FpSpread ID="FpSpread1" runat="server" Visible="true" BorderStyle="Solid"
                BorderWidth="0px" Width="1206px" Height="500px" Style="overflow: auto; border: 0px solid #999999;
                border-radius: 10px; background-color: White; box-shadow: 0px 0px 8px #999999;"
                CssClass="spreadborder" ShowHeaderSelection="false" OnCellClick="FpSpread1_OnCellClick"
                OnPreRender="FpSpread1_OnButtonCommand">
                <Sheets>
                    <FarPoint:SheetView SheetName="Sheet1">
                    </FarPoint:SheetView>
                </Sheets>
            </FarPoint:FpSpread>
            <br />
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
        </center>
    </div>
    <div id="Add_FeedBack" runat="server" visible="false" style="height: 100%; z-index: 1000;
        width: 100%; background-color: rgba(54, 25, 25, .2); position: absolute; top: 0;
        left: 0px;">
        <center>
            <asp:ImageButton ID="ImageButton2" runat="server" Width="792px" Height="40px" ImageUrl="~/images/close.png"
                Style="height: 30px; width: 30px; position: absolute; margin-top: -12px; margin-left: 439px;"
                OnClick="imagebtnpopclose1_Click" />
            <div id="panel_add" runat="server" visible="true" class="table" style="background-color: White;
                height: auto; width: 920px; border: 5px solid #0CA6CA; border-top: 25px solid #0CA6CA;
                margin-top: 27px; border-radius: 10px;">
                <br />
                <div>
                    <span class="fontstyleheader" style="color: Green">FeedBack Master</span>
                </div>
                <br />
                <center>
                    <asp:RadioButton ID="rb_Acad" Width="95px" runat="server" GroupName="same1" Text="Academic"
                        OnCheckedChanged="rb_Acad_CheckedChanged" AutoPostBack="true" Checked="true">
                    </asp:RadioButton>
                    <asp:RadioButton ID="rb_Gend" runat="server" Width="100px" GroupName="same1" Text="General"
                        OnCheckedChanged="rb_Gend_CheckedChanged" AutoPostBack="true"></asp:RadioButton>
                    <asp:RadioButton ID="rb_induvgual" Visible="false" runat="server" Width="100px" GroupName="updat"
                        Text="Individual " OnCheckedChanged="rb_induvgual_CheckedChanged" AutoPostBack="true"
                        Checked="true"></asp:RadioButton>
                    <asp:RadioButton ID="rb_common" runat="server" Visible="false" Width="100px" GroupName="updat"
                        Text="Common" OnCheckedChanged="rb_common_CheckedChanged" AutoPostBack="true">
                    </asp:RadioButton>
                    <div id="Acad" runat="server" visible="true">
                        <table class="maintablestyle" width="752px" height="40px">
                            <tr>
                                <td>
                                    <asp:Label ID="lbl_college" runat="server" Text="College"></asp:Label>
                                </td>
                                <td>
                                    <asp:UpdatePanel ID="UpdatePanel10" Visible="true" runat="server">
                                        <ContentTemplate>
                                            <asp:DropDownList ID="ddl_college" runat="server" Width="196px" Height="30px" CssClass="textbox textbox1 ddlheight3"
                                                AutoPostBack="true" OnSelectedIndexChanged="ddl_college_SelectedIndexChanged">
                                            </asp:DropDownList>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                                <td>
                                    <asp:Label ID="lbl_Batchyear" runat="server" Text="Batch Year"></asp:Label>
                                </td>
                                <td>
                                    <asp:UpdatePanel ID="Updp_Batchyear" runat="server">
                                        <ContentTemplate>
                                            <asp:TextBox ID="txt_batch" ReadOnly="true" Width=" 90px" runat="server" CssClass="textbox  txtheight2">--Select--</asp:TextBox>
                                            <asp:Panel ID="Panel_Batchyear" runat="server" CssClass="multxtpanel" Height="200px"
                                                Width="200px">
                                                <asp:CheckBox ID="cb_batch" runat="server" Text="Select All" AutoPostBack="True"
                                                    OnCheckedChanged="cb_batch_CheckedChanged" />
                                                <asp:CheckBoxList ID="cbl_batch" runat="server" AutoPostBack="True" OnSelectedIndexChanged="cbl_batch_SelectedIndexChanged">
                                                </asp:CheckBoxList>
                                            </asp:Panel>
                                            <asp:PopupControlExtender ID="PopupControlExtender8" runat="server" TargetControlID="txt_batch"
                                                PopupControlID="Panel_Batchyear" Position="Bottom">
                                            </asp:PopupControlExtender>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                                <td>
                                    <asp:Label ID="lbl_Degree" Width="50px" runat="server" Text="Degree"></asp:Label>
                                </td>
                                <td>
                                    <asp:UpdatePanel ID="Updp_Degree" runat="server">
                                        <ContentTemplate>
                                            <asp:TextBox ID="txt_degree" Width=" 90px" runat="server" CssClass="textbox  txtheight2"
                                                ReadOnly="true">--Select--</asp:TextBox>
                                            <asp:Panel ID="Panel_Degree" runat="server" CssClass="multxtpanel" Height="200px"
                                                Width="200px">
                                                <asp:CheckBox ID="cb_degree" runat="server" Text="Select All" AutoPostBack="True"
                                                    OnCheckedChanged="cb_degree_CheckedChanged" />
                                                <asp:CheckBoxList ID="cbl_degree" runat="server" AutoPostBack="True" OnSelectedIndexChanged="cbl_degree_SelectedIndexChanged">
                                                </asp:CheckBoxList>
                                            </asp:Panel>
                                            <asp:PopupControlExtender ID="PopupControlExtender9" runat="server" TargetControlID="txt_degree"
                                                PopupControlID="Panel_Degree" Position="Bottom">
                                            </asp:PopupControlExtender>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="lbl_dpt" runat="server" Width="75px" Text="Department"></asp:Label>
                                </td>
                                <td>
                                    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                                        <ContentTemplate>
                                            <asp:TextBox ID="txt_branch" Width=" 128px" ReadOnly="true" runat="server" CssClass="textbox  txtheight2">--Select--</asp:TextBox>
                                            <asp:Panel ID="Panel_dpt" runat="server" CssClass="multxtpanel" Height="200px" Width="200px">
                                                <asp:CheckBox ID="cb_branch" runat="server" Text="Select All" AutoPostBack="True"
                                                    OnCheckedChanged="cb_branch_CheckedChanged" />
                                                <asp:CheckBoxList ID="cbl_branch" runat="server" AutoPostBack="True" OnSelectedIndexChanged="cbl_branch_SelectedIndexChanged">
                                                </asp:CheckBoxList>
                                            </asp:Panel>
                                            <asp:PopupControlExtender ID="PopupControlExtender1" runat="server" TargetControlID="txt_branch"
                                                PopupControlID="Panel_dpt" Position="Bottom">
                                            </asp:PopupControlExtender>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                                <td>
                                    <asp:Label ID="lbl_sem" runat="server" Text="Sem"></asp:Label>
                                </td>
                                <td>
                                    <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                                        <ContentTemplate>
                                            <asp:TextBox ID="txt_sem" Width=" 90px" ReadOnly="true" runat="server" CssClass="textbox  txtheight2">--Select--</asp:TextBox>
                                            <asp:Panel ID="Panel_Sem" runat="server" CssClass="multxtpanel" Height="200px" Width="200px">
                                                <asp:CheckBox ID="cb_sem" runat="server" Text="Select All" AutoPostBack="True" OnCheckedChanged="cb_sem_CheckedChanged" />
                                                <asp:CheckBoxList ID="cbl_sem" runat="server" AutoPostBack="True" OnSelectedIndexChanged="cbl_sem_SelectedIndexChanged">
                                                </asp:CheckBoxList>
                                            </asp:Panel>
                                            <asp:PopupControlExtender ID="PopupControlExtender2" runat="server" TargetControlID="txt_sem"
                                                PopupControlID="Panel_Sem" Position="Bottom">
                                            </asp:PopupControlExtender>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                                <td>
                                    <asp:Label ID="lbl_Sec" runat="server" Text="Sec"></asp:Label>
                                </td>
                                <td>
                                    <asp:UpdatePanel ID="UpdatePanel3" runat="server">
                                        <ContentTemplate>
                                            <asp:TextBox ID="txt_sec" Width=" 90px" ReadOnly="true" runat="server" CssClass="textbox  txtheight2">--Select--</asp:TextBox>
                                            <asp:Panel ID="Panel_Sec" runat="server" CssClass="multxtpanel" Height="200px" Width="200px">
                                                <asp:CheckBox ID="cb_sec" runat="server" Text="Select All" AutoPostBack="True" OnCheckedChanged="cb_sec_CheckedChanged" />
                                                <asp:CheckBoxList ID="cbl_sec" runat="server" AutoPostBack="True" OnSelectedIndexChanged="cbl_sec_SelectedIndexChanged">
                                                </asp:CheckBoxList>
                                            </asp:Panel>
                                            <asp:PopupControlExtender ID="PopupControlExtender3" runat="server" TargetControlID="txt_sec"
                                                PopupControlID="Panel_Sec" Position="Bottom">
                                            </asp:PopupControlExtender>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                            </tr>
                        </table>
                    </div>
                </center>
                <div id="Gendral" runat="server" visible="true">
                    <br />
                    <table visible="false" style="width: 600px; margin-left: 50px;">
                        <tr>
                            <td>
                                <asp:Label ID="lbl_FBName" runat="server" Text="FeedBack Name"></asp:Label>
                            </td>
                            <td>
                                <asp:TextBox ID="txt_FBName" runat="server" onfocus=" return display(this)" CssClass="textbox textbox1"
                                    Height="20px" Width="358px" Style="font-family: 'Book Antiqua'" Font-Bold="True"
                                    Font-Names="Book Antiqua" Font-Size="Medium"></asp:TextBox>
                                <span style="color: Red;">*</span>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="lbl_fromdate" runat="server" Width="75px" Text="Start Date"></asp:Label>
                            </td>
                            <td>
                                <asp:TextBox ID="txt_fromdate" Width=" 90px" runat="server" CssClass="textbox textbox1"
                                    AutoPostBack="true" OnTextChanged="txt_fromdate_TextChanged"></asp:TextBox>
                                <asp:CalendarExtender ID="CalendarExtender1" TargetControlID="txt_fromdate" runat="server"
                                    Format="dd/MM/yyyy" CssClass="cal_Theme1 ajax__calendar_active">
                                </asp:CalendarExtender>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="lbl_Enddate" runat="server" Text="End Date"></asp:Label>
                            </td>
                            <td>
                                <asp:TextBox ID="txt_Enddate" runat="server" CssClass="textbox textbox1" AutoPostBack="true"
                                    OnTextChanged="txt_Enddate_TextChanged" Width=" 90px"></asp:TextBox>
                                <asp:CalendarExtender ID="CalendarExtender2" TargetControlID="txt_Enddate" runat="server"
                                    Format="dd/MM/yyyy" CssClass="cal_Theme1 ajax__calendar_active">
                                </asp:CalendarExtender>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:CheckBox ID="cb_IsGeneral" runat="server" Text="Is General" AutoPostBack="True"
                                    OnCheckedChanged="cb_IsGeneral_CheckedChanged" />
                            </td>
                            <td>
                                <asp:CheckBox ID="cb_Typeindiviual" runat="server" Text="Type Individual" />
                                <asp:CheckBox ID="cb_Subjectwise" runat="server" Text="Subjectwise" />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="lbl_Subject_Type" runat="server" Text="Subject Type"></asp:Label>
                            </td>
                            <td>
                                <asp:UpdatePanel ID="UpdatePanel4" runat="server">
                                    <ContentTemplate>
                                        <asp:TextBox ID="Txt_Subjecttype" Width=" 125px" ReadOnly="true" runat="server" CssClass="textbox  textbox1">--Select--</asp:TextBox>
                                        <asp:Panel ID="Panel_Subjecttype" runat="server" CssClass="multxtpanel" Height="200px"
                                            Width="200px">
                                            <asp:CheckBox ID="Cb_Subjecttype" runat="server" Text="Select All" AutoPostBack="True"
                                                OnCheckedChanged="Cb_Subjecttype_CheckedChanged" />
                                            <asp:CheckBoxList ID="Cbl_Subjecttype" runat="server" AutoPostBack="True" OnSelectedIndexChanged="Cbl_Subjecttype_SelectedIndexChanged">
                                            </asp:CheckBoxList>
                                        </asp:Panel>
                                        <asp:PopupControlExtender ID="PopupControlExtender4" runat="server" TargetControlID="Txt_Subjecttype"
                                            PopupControlID="Panel_Subjecttype" Position="Bottom">
                                        </asp:PopupControlExtender>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:CheckBox ID="cb_optional" runat="server" Text="Optional Subject Type" AutoPostBack="True"
                                    OnCheckedChanged="cb_optional_CheckedChanged" />
                            </td>
                            <td>
                                <%--<asp:Label ID="lbl_Subject_Type1" runat="server" Text="Subject Type"></asp:Label>--%>
                                <asp:UpdatePanel ID="UpdatePanel11" runat="server">
                                    <ContentTemplate>
                                        <asp:TextBox ID="Txt_Subjecttype1" Enabled="false" Width=" 125px" ReadOnly="true"
                                            runat="server" CssClass="textbox  textbox1">--Select--</asp:TextBox>
                                        <asp:Panel ID="Panel_Subjecttype1" runat="server" CssClass="multxtpanel" Height="200px"
                                            Width="200px">
                                            <asp:CheckBox ID="Cb_Subjecttype1" runat="server" Text="Select All" AutoPostBack="True"
                                                OnCheckedChanged="Cb_Subjecttype1_CheckedChanged" />
                                            <asp:CheckBoxList ID="Cbl_Subjecttype1" runat="server" AutoPostBack="True" OnSelectedIndexChanged="Cbl_Subjecttype1_SelectedIndexChanged">
                                            </asp:CheckBoxList>
                                        </asp:Panel>
                                        <asp:PopupControlExtender ID="PopupControlExtender13" runat="server" TargetControlID="Txt_Subjecttype1"
                                            PopupControlID="Panel_Subjecttype1" Position="Bottom">
                                        </asp:PopupControlExtender>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>
                        </tr>
                    </table>
                    <br />
                    <table>
                        <tr>
                            <td>
                            </td>
                        </tr>
                    </table>
                    <table id="Table1" runat="server" style="width: 600px; margin-left: 50px;">
                        <tr>
                            <td>
                                <asp:CheckBox ID="chk_includecommon" runat="server" AutoPostBack="true" Text=" Include generated anonymous feed back "
                                    OnCheckedChanged="chk_includecommon_CheckedChanged" />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="lbl_totl_strength" runat="server" Width="173px" Text="Total Strength"></asp:Label>
                                <asp:TextBox ID="txt_total_strength" runat="server" Enabled="false" CssClass="textbox textbox1"
                                    Height="20px" Width="119px" Style="font-family: 'Book Antiqua'" Font-Bold="True"
                                    Font-Names="Book Antiqua" Font-Size="Medium"></asp:TextBox>
                                <%--    Added by Saranyadevi 19.2.2018--%>
                                <asp:CheckBox ID="chk_random" runat="server" AutoPostBack="true" Visible="false"
                                    Text="Generate Random Number" OnCheckedChanged="chk_random_CheckedChanged" />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="lbl_fb_acr" runat="server" Text="Start Feed Back Acronames"></asp:Label>
                                <asp:TextBox ID="txt_fb_acr" runat="server" CssClass="nv textbox textbox1" Height="20px"
                                    Width="80px" Style="font-family: 'Book Antiqua'" onfocus=" return display(this)"
                                    Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium"></asp:TextBox>
                                <%--  <asp:TextBox ID="txt_fb_acr" runat="server" onfocus=" return display(this)" CssClass="textbox textbox1"
                                    Height="20px" Width="119px" Style="font-family: 'Book Antiqua'" Font-Bold="True"
                                    Font-Names="Book Antiqua" Font-Size="Medium"></asp:TextBox>--%>
                                <%--  onfocus=" return display(this)", onfocus=" return display(this)"--%>
                                <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" runat="server" TargetControlID="txt_fb_acr"
                                    FilterType="UppercaseLetters,LowercaseLetters,Custom,numbers" ValidChars=" ">
                                </asp:FilteredTextBoxExtender>
                                <asp:Label ID="lbl_run_sries" runat="server" Text="Running Series"></asp:Label>
                                <asp:TextBox ID="txt_running_series" runat="server" CssClass="textbox textbox1" Height="20px"
                                    Width="37px" Style="font-family: 'Book Antiqua'" Font-Bold="True" Font-Names="Book Antiqua"
                                    Font-Size="Medium" onfocus=" return display(this)"></asp:TextBox>
                                <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender2" runat="server" TargetControlID="txt_running_series"
                                    FilterType="Numbers,custom" ValidChars=".">
                                </asp:FilteredTextBoxExtender>
                            </td>
                        </tr>
                    </table>
                    <center>
                        <br />
                        <asp:Button ID="btn_Save" runat="server" Visible="true" Width="68px" Height="32px"
                            Text="Save" CssClass="textbox textbox1" OnClientClick="return check()" OnClick="btn_save_Click" />
                        <asp:Button ID="btndel" runat="server" Visible="true" Width="68px" Height="32px"
                            CssClass="textbox textbox1" Text="Delete" OnClientClick="return check()" OnClick="btndel_Click" />
                        <asp:Button ID="btn_exit" runat="server" Visible="true" Width="68px" Height="32px"
                            Text="Exit" CssClass="textbox textbox1" OnClick="btn_exit_Click" />
                        <asp:Label ID="lbl_firstpk" runat="server" Visible="false" Text=""></asp:Label>
                    </center>
                    <br />
                    <center>
                        <Insproplus:printmaster runat="server" ID="Printcontrol" Visible="false" />
                    </center>
                </div>
                <br />
                <br />
            </div>
        </center>
        <br />
        <br />
    </div>
    <div id="imgdiv2" runat="server" visible="false" style="height: 100%; z-index: 1000;
        width: 100%; background-color: rgba(54, 25, 25, .2); position: absolute; top: 0;
        left: 0px;">
        <asp:ImageButton ID="img_close" runat="server" Width="392px" Height="30px" ImageUrl="~/images/close.png"
            Style="height: 30px; width: 30px; position: absolute; margin-top: 184px; margin-left: 614px;"
            OnClick="img_close_Click" />
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
                                    <asp:Button ID="btn_errorclose" Visible="true" CssClass=" textbox btn1 comm" Style="height: 28px;
                                        width: 65px;" OnClick="btn_errorclose_Click" Text="ok" runat="server" />
                                    <asp:Button ID="btn_creatxl" Visible="false" CssClass=" textbox btn1 comm" Style="height: 28px;
                                        width: 65px;" OnClick="btn_creatxl_Click" Text="ok" runat="server" />
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
</asp:Content>

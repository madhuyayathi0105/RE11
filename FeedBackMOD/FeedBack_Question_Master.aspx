<%@ Page Title="" Language="C#" MasterPageFile="~/FeedBackMOD/FeedbackSubSiteMaster.master"
    AutoEventWireup="true" CodeFile="FeedBack_Question_Master.aspx.cs" Inherits="FeedBack_Question_Master" %>

<%@ Register Src="~/Usercontrols/PrintMaster.ascx" TagName="printmaster" TagPrefix="Insproplus" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Assembly="FarPoint.Web.Spread" Namespace="FarPoint.Web.Spread" TagPrefix="FarPoint" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <link href="~/Styles/css/Registration.css" rel="stylesheet" type="text/css" />
    <link href="~/Styles/css/Commoncss.css" rel="Stylesheet" type="text/css" />
    <script type="text/javascript">
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
            <center>
                <div>
                    <span class="fontstyleheader" style="color: Green">FeedBack Question Master</span></div>
            </center>
            <br />
            <div class="maindivstyle">
                <center>
                    <asp:RadioButton ID="rb_Acad" Width="100px" runat="server" GroupName="type" Text="Academic"
                        OnCheckedChanged="rb_Acad_CheckedChanged" AutoPostBack="true" Checked="true">
                    </asp:RadioButton>
                    <asp:RadioButton ID="rb_Gend" runat="server" Width="100px" GroupName="type" Text="General"
                        OnCheckedChanged="rb_Gend_CheckedChanged" AutoPostBack="true"></asp:RadioButton>
                </center>
                <br />
                <div id="Acad" runat="server" visible="true">
                    <table class="maintablestyle" width="819px" height="40px">
                        <tr>
                            <td>
                                <asp:Label ID="lbl_college" runat="server" Text="College"></asp:Label>
                            </td>
                            <td>
                                <asp:UpdatePanel ID="UpdatePanel20" runat="server">
                                    <ContentTemplate>
                                        <asp:TextBox ID="Txt_college" Width=" 90px" ReadOnly="true" runat="server" CssClass="textbox  txtheight2">--Select--</asp:TextBox>
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
                                <asp:Label ID="lbl_Batchyear" runat="server" Text="Batch Year" Width=" 90px"></asp:Label>
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
                                        <asp:PopupControlExtender ID="PopupControlExtender21" runat="server" TargetControlID="txt_batch"
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
                            <td>
                                <asp:Label ID="lbl_dpt" runat="server" Width="75px" Text="Department"></asp:Label>
                            </td>
                            <td>
                                <asp:UpdatePanel ID="UpdatePanel23" runat="server">
                                    <ContentTemplate>
                                        <asp:TextBox ID="txt_branch" Width=" 128px" ReadOnly="true" runat="server" CssClass="textbox  txtheight2">--Select--</asp:TextBox>
                                        <asp:Panel ID="Panel_dpt" runat="server" CssClass="multxtpanel" Height="200px" Width="200px">
                                            <asp:CheckBox ID="cb_branch" runat="server" Text="Select All" AutoPostBack="True"
                                                OnCheckedChanged="cb_branch_CheckedChanged" />
                                            <asp:CheckBoxList ID="cbl_branch" runat="server" AutoPostBack="True" OnSelectedIndexChanged="cbl_branch_SelectedIndexChanged">
                                            </asp:CheckBoxList>
                                        </asp:Panel>
                                        <asp:PopupControlExtender ID="PopupControlExtender23" runat="server" TargetControlID="txt_branch"
                                            PopupControlID="Panel_dpt" Position="Bottom">
                                        </asp:PopupControlExtender>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="lbl_sem" runat="server" Text="Sem"></asp:Label>
                            </td>
                            <td>
                                <asp:UpdatePanel ID="UpdatePanel24" runat="server">
                                    <ContentTemplate>
                                        <asp:TextBox ID="txt_sem" Width=" 90px" ReadOnly="true" runat="server" CssClass="textbox  txtheight2">--Select--</asp:TextBox>
                                        <asp:Panel ID="Panel_Sem" runat="server" CssClass="multxtpanel" Height="200px" Width="200px">
                                            <asp:CheckBox ID="cb_sem" runat="server" Text="Select All" AutoPostBack="True" OnCheckedChanged="cb_sem_CheckedChanged" />
                                            <asp:CheckBoxList ID="cbl_sem" runat="server" AutoPostBack="True" OnSelectedIndexChanged="cbl_sem_SelectedIndexChanged">
                                            </asp:CheckBoxList>
                                        </asp:Panel>
                                        <asp:PopupControlExtender ID="PopupControlExtender24" runat="server" TargetControlID="txt_sem"
                                            PopupControlID="Panel_Sem" Position="Bottom">
                                        </asp:PopupControlExtender>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>
                            <td>
                                <asp:Label ID="lbl_Sec" runat="server" Text="Sec"></asp:Label>
                            </td>
                            <td>
                                <asp:UpdatePanel ID="UpdatePanel25" runat="server">
                                    <ContentTemplate>
                                        <asp:TextBox ID="txt_sec" Width=" 90px" ReadOnly="true" runat="server" CssClass="textbox  txtheight2">--Select--</asp:TextBox>
                                        <asp:Panel ID="Panel_Sec" runat="server" CssClass="multxtpanel" Height="200px" Width="200px">
                                            <asp:CheckBox ID="cb_sec" runat="server" Text="Select All" AutoPostBack="True" OnCheckedChanged="cb_sec_CheckedChanged" />
                                            <asp:CheckBoxList ID="cbl_sec" runat="server" AutoPostBack="True" OnSelectedIndexChanged="cbl_sec_SelectedIndexChanged">
                                            </asp:CheckBoxList>
                                        </asp:Panel>
                                        <asp:PopupControlExtender ID="PopupControlExtender25" runat="server" TargetControlID="txt_sec"
                                            PopupControlID="Panel_Sec" Position="Bottom">
                                        </asp:PopupControlExtender>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>
                            <td>
                                <asp:Label ID="lbl_FbName1" runat="server" Text="FeedBack Name" Width=" 120px"></asp:Label>
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
                                <td>
                                    <asp:Button ID="btn_search1" runat="server" CssClass="textbox btn2" Text="Search"
                                        OnClick="btn_Search1_Click" />
                                </td>
                                <td>
                                    <asp:Button ID="btn_Add" runat="server" CssClass="textbox btn2" Text="Add New" OnClick="btnAdd_Click" />
                                </td>
                        </tr>
                    </table>
                </div>
                <br />
                <FarPoint:FpSpread ID="FpSpread1" runat="server" Visible="false" BorderStyle="Solid"
                    BorderWidth="0px" Width="623px" Height="350px" Style="overflow: auto; border: 0px solid #999999;
                    border-radius: 10px; background-color: White; box-shadow: 0px 0px 8px #999999;"
                    CssClass="spreadborder" ShowHeaderSelection="false" OnButtonCommand="FpSpread1_OnButtonCommand">
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
                    <asp:Button ID="btndel" runat="server" Visible="true" Width="68px" Height="32px"
                        CssClass="textbox textbox1" Text="Delete" Font-Bold="true" OnClientClick="return check()"
                        OnClick="btndel_Click" />
                </div>
                <br />
            </div>
        </center>
    </div>
    <center>
        <div id="addnew" runat="server" visible="false" style="height: 50em; z-index: 1000;
            width: 100%; background-color: rgba(54, 25, 25, .2); position: absolute; top: 0;
            left: 0;">
            <asp:ImageButton ID="ImageButton2" runat="server" Width="40px" Height="40px" ImageUrl="~/images/close.png"
                Style="height: 30px; width: 30px; position: absolute; margin-top: 102px; margin-left: 453px;"
                OnClick="imagebtnpopclose1_Click" />
            <br />
            <br />
            <br />
            <br />
            <br />
            <div style="background-color: White; height: 520px; width: 930px; border: 5px solid #0CA6CA;
                border-top: 30px solid #0CA6CA; border-radius: 10px;">
                <center>
                    <center>
                        <span class="fontstyleheader" style="color: Green">FeedBack Question Master</span>
                        <br />
                        <br />
                    </center>
                    <center>
                        <asp:RadioButton ID="rb_Acad1" Width="95px" runat="server" GroupName="same2" Text="Academic"
                            OnCheckedChanged="rb_Acad1_CheckedChanged" AutoPostBack="true" Checked="true">
                        </asp:RadioButton>
                        <asp:RadioButton ID="rb_Gend1" runat="server" Width="100px" GroupName="same2" Text="General"
                            OnCheckedChanged="rb_Gend1_CheckedChanged" AutoPostBack="true"></asp:RadioButton>
                        <asp:RadioButton ID="rdbobjective" runat="server" AutoPostBack="true" Text="Objective"
                            Checked="true" Visible="false" OnCheckedChanged="rdbobjective_checkedChange" />
                        <asp:RadioButton ID="rdbdescriptive" runat="server" AutoPostBack="true" Text="Descriptive"
                            Checked="false" Visible="false" OnCheckedChanged="rdbdescriptive_checkedChange" />
                    </center>
                    <br />
                    <div id="Acad1" runat="server" visible="true">
                        <table class="maintablestyle" width="689px" height="40px">
                            <tr>
                                <td>
                                    <asp:Label ID="lbl_college1" runat="server" Text="College"></asp:Label>
                                </td>
                                <td>
                                    <asp:UpdatePanel ID="UpdatePanel10" Visible="true" runat="server">
                                        <ContentTemplate>
                                            <asp:DropDownList ID="ddl_college" runat="server" Width="146px" Height="30px" CssClass="textbox1 ddlheight3"
                                                AutoPostBack="true" OnSelectedIndexChanged="ddl_college_SelectedIndexChanged">
                                            </asp:DropDownList>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                                <td>
                                    <asp:Label ID="lbl_Batchyear1" Width=" 90px" runat="server" Text="Batch Year"></asp:Label>
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
                                            <asp:PopupControlExtender ID="PopupControlExtender8" runat="server" TargetControlID="txt_batch1"
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
                                    <asp:Label ID="lbl_dpt1" runat="server" Width="80px" Text="Department"></asp:Label>
                                </td>
                                <td>
                                    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                                        <ContentTemplate>
                                            <asp:TextBox ID="txt_branch1" Width=" 128px" ReadOnly="true" runat="server" CssClass="textbox  txtheight2">--Select--</asp:TextBox>
                                            <asp:Panel ID="Panel_dpt1" runat="server" CssClass="multxtpanel" Height="200px" Width="200px">
                                                <asp:CheckBox ID="cb_branch1" runat="server" Text="Select All" AutoPostBack="True"
                                                    OnCheckedChanged="cb_branch1_CheckedChanged" />
                                                <asp:CheckBoxList ID="cbl_branch1" runat="server" AutoPostBack="True" OnSelectedIndexChanged="cbl_branch1_SelectedIndexChanged">
                                                </asp:CheckBoxList>
                                            </asp:Panel>
                                            <asp:PopupControlExtender ID="PopupControlExtender1" runat="server" TargetControlID="txt_branch1"
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
                                    <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                                        <ContentTemplate>
                                            <asp:TextBox ID="txt_sem1" Width=" 90px" ReadOnly="true" runat="server" CssClass="textbox  txtheight2">--Select--</asp:TextBox>
                                            <asp:Panel ID="Panel_Sem1" runat="server" CssClass="multxtpanel" Height="200px" Width="200px">
                                                <asp:CheckBox ID="cb_sem1" runat="server" Text="Select All" AutoPostBack="True" OnCheckedChanged="cb_sem1_CheckedChanged" />
                                                <asp:CheckBoxList ID="cbl_sem1" runat="server" AutoPostBack="True" OnSelectedIndexChanged="cbl_sem1_SelectedIndexChanged">
                                                </asp:CheckBoxList>
                                            </asp:Panel>
                                            <asp:PopupControlExtender ID="PopupControlExtender2" runat="server" TargetControlID="txt_sem1"
                                                PopupControlID="Panel_Sem1" Position="Bottom">
                                            </asp:PopupControlExtender>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                                <td>
                                    <asp:Label ID="lbl_Sec1" runat="server" Text="Sec"></asp:Label>
                                </td>
                                <td>
                                    <asp:UpdatePanel ID="UpdatePanel3" runat="server">
                                        <ContentTemplate>
                                            <asp:TextBox ID="txt_sec1" Width=" 90px" ReadOnly="true" runat="server" CssClass="textbox  txtheight2">--Select--</asp:TextBox>
                                            <asp:Panel ID="Panel_Sec1" runat="server" CssClass="multxtpanel" Height="200px" Width="200px">
                                                <asp:CheckBox ID="cb_sec1" runat="server" Text="Select All" AutoPostBack="True" OnCheckedChanged="cb_sec1_CheckedChanged" />
                                                <asp:CheckBoxList ID="cbl_sec1" runat="server" AutoPostBack="True" OnSelectedIndexChanged="cbl_sec1_SelectedIndexChanged">
                                                </asp:CheckBoxList>
                                            </asp:Panel>
                                            <asp:PopupControlExtender ID="PopupControlExtender3" runat="server" TargetControlID="txt_sec1"
                                                PopupControlID="Panel_Sec1" Position="Bottom">
                                            </asp:PopupControlExtender>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                                <td>
                                    <asp:Label ID="lbl_FbName2" runat="server" Width=" 114px" Text="FeedBack Name"></asp:Label>
                                </td>
                                <td>
                                    <%-- <asp:TextBox ID="txt_Fbname2" Width=" 114px"  runat="server" CssClass="textbox  txtheight2"></asp:TextBox>
                                    --%>
                                    <asp:UpdatePanel ID="UpdatePanel7" runat="server">
                                        <ContentTemplate>
                                            <asp:TextBox ID="txt_Fbname2" Width=" 114px" ReadOnly="true" runat="server" CssClass="textbox  txtheight2">--Select--</asp:TextBox>
                                            <asp:Panel ID="Panel_FbName3" runat="server" CssClass="multxtpanel" Height="200px"
                                                Width="200px">
                                                <asp:CheckBox ID="Cb_FbName2" runat="server" Text="Select All" AutoPostBack="True"
                                                    OnCheckedChanged="Cb_FbName2_CheckedChanged" />
                                                <asp:CheckBoxList ID="Cbl_FbName2" runat="server" AutoPostBack="True" OnSelectedIndexChanged="Cbl_FbName2_SelectedIndexChanged">
                                                </asp:CheckBoxList>
                                            </asp:Panel>
                                            <asp:PopupControlExtender ID="PopupControlExtender7" runat="server" TargetControlID="txt_Fbname2"
                                                PopupControlID="Panel_FbName3" Position="Bottom">
                                            </asp:PopupControlExtender>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                                <td>
                                    <asp:Label ID="lbl_Subject_Type" runat="server" Text="Subject"></asp:Label>
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
                                <td>
                                    <asp:Button ID="btn_search" runat="server" CssClass="textbox btn1" Text="Go" OnClick="btnsearch_Click" />
                                </td>
                            </tr>
                        </table>
                    </div>
                    <br />
                    <%-- <div id="div2" visible="true" runat="server" class="spreadborder" style="width: 635px;
                    height: 232px; overflow: auto; background-color: White; border-radius: 10px;">
                    --%>
                    <FarPoint:FpSpread ID="FpSpread2" runat="server" Visible="true" BorderStyle="Solid"
                        BorderWidth="0px" Width="635px" Height="243px" Style="overflow: auto; border: 0px solid #999999;
                        border-radius: 10px; background-color: White; box-shadow: 0px 0px 8px #999999;"
                        CssClass="spreadborder" ShowHeaderSelection="false" OnButtonCommand="FpSpread2_OnButtonCommand">
                        <Sheets>
                            <FarPoint:SheetView SheetName="Sheet1">
                            </FarPoint:SheetView>
                        </Sheets>
                    </FarPoint:FpSpread>
                    <br />
                    <%-- </div>--%>
                    <br />
                    <asp:Button ID="btn_save" runat="server" Visible="true" Width="68px" Height="32px"
                        CssClass="textbox textbox1" Text="Save" OnClientClick="return check()" OnClick="btn_savequstion_Click" />
                    <asp:Button ID="btn_exit" runat="server" Visible="true" Width="68px" Height="32px"
                        CssClass="textbox textbox1" Text="Exit" OnClick="btn_exit_Click" /><br />
                    <br />
                </center>
            </div>
            <br />
        </div>
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
                                            width: 65px;" OnClick="btn_warningmsg_Click" Text="YES" runat="server" />
                                        <asp:Button ID="btn_warning_exit" CssClass="textbox textbox1" Style="height: 28px;
                                            width: 65px;" OnClick="btn_warning_exit_Click" Text="NO" runat="server" />
                                </td>
                            </tr>
                        </table>
                    </center>
                </div>
            </center>
        </div>
    </center>
</asp:Content>

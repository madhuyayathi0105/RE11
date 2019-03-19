<%@ Page Title="" Language="C#" MasterPageFile="~/FeedBackMOD/FeedbackSubSiteMaster.master"
    AutoEventWireup="true" CodeFile="Feedback_report.aspx.cs" Inherits="Feedback_report" %>

<%@ Register Src="~/Usercontrols/PrintMaster.ascx" TagName="printmaster" TagPrefix="Insproplus" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Assembly="FarPoint.Web.Spread" Namespace="FarPoint.Web.Spread" TagPrefix="FarPoint" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <link href="~/Styles/css/Commoncss.css" rel="Stylesheet" type="text/css" />
    <%-- <script src="//ajax.googleapis.com/ajax/libs/jquery/1.11.1/jquery.min.js"></script>--%>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">
    <body>
        <script type="text/javascript">
            function myFunction(x) {
                x.style.borderColor = "#c4c4c4";
            }
            function PrintPanel() {

                var panel = document.getElementById("<%=chartdiv.ClientID %>");
                var printWindow = window.open('', '', 'height=600px,width=1200px');
                printWindow.document.write('<html');
                printWindow.document.write('<head>');
                printWindow.document.write('</head><body style="transform: rotate(90deg);margin: 5px; padding: 5px;">');
                printWindow.document.write('<form>');
                printWindow.document.write(panel.innerHTML);
                printWindow.document.write(' </form>');
                printWindow.document.write('</body></html>');

                printWindow.document.close();
                setTimeout(function () {
                    printWindow.print();
                }, 500);
                return false;
            }           
        </script>
        <asp:ScriptManager ID="ScriptManager1" runat="server">
        </asp:ScriptManager>
        <div style="width: auto;">
            <br />
            <center>
                <div>
                    <span class="fontstyleheader" style="color: Green">FeedBack Report </span>
                </div>
                <br />
            </center>
            <div class="maindivstyle" width="610px">
                <center>
                    <fieldset style="width: 212px; height: 10px; background-color: #ffccff; margin-left: -744px;
                        margin-top: 10px; border-radius: 10px; border-color: #6699ee; overflow: auto;">
                        <table style="margin-top: -7px;">
                            <tr>
                                <td>
                                    <asp:RadioButton ID="rb_Acad" Width="92px" runat="server" GroupName="same" Text="Academic"
                                        OnCheckedChanged="rb_Acad1_CheckedChanged" AutoPostBack="true" Checked="true">
                                    </asp:RadioButton>
                                    <asp:RadioButton ID="rb_Gend" runat="server" Width="100px" GroupName="same" Text="General"
                                        OnCheckedChanged="rb_Gend1_CheckedChanged" AutoPostBack="true"></asp:RadioButton>
                                </td>
                            </tr>
                        </table>
                    </fieldset>
                    <fieldset id="acd_comman_login" runat="server" visible="true" style="width: 250px;
                        height: 10px; background-color: #44E8B7; margin-left: -216px; margin-top: -30px;
                        border-radius: 10px; overflow: auto;">
                        <table style="margin-top: -5px;">
                            <tr>
                                <td>
                                    <asp:RadioButton ID="rb_login" Width="110px" Visible="true" runat="server" GroupName="login"
                                        Text="Login Based" OnCheckedChanged="rb_login_CheckedChanged" AutoPostBack="true"
                                        Checked="true"></asp:RadioButton>
                                </td>
                                <td>
                                    <asp:RadioButton ID="rb_anonymous" runat="server" Visible="true" Width="130px" GroupName="login"
                                        Text="Anonymous" OnCheckedChanged="rb_anonymous_CheckedChanged" AutoPostBack="true">
                                    </asp:RadioButton>
                                </td>
                            </tr>
                        </table>
                    </fieldset>
                </center>
                <div id="Acad" runat="server" class="maintablestyle" width="971px" height="40px"
                    style="margin-top: 15px;">
                    <table id="anonymousfilter1" runat="server">
                        <tr>
                            <td>
                                <asp:Label ID="lbl_college" runat="server" Text="College"></asp:Label>
                            </td>
                            <td>
                                <asp:UpdatePanel ID="UpdatePanel20" runat="server">
                                    <ContentTemplate>
                                        <asp:TextBox ID="Txt_college" Width=" 90px" ReadOnly="true" runat="server" CssClass="textbox  txtheight2">--Select--</asp:TextBox>
                                        <asp:Panel ID="Panel_college" runat="server" CssClass="multxtpanel">
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
                                <asp:Label ID="lbl_Batchyear" runat="server" Text="Batch Year"></asp:Label>
                            </td>
                            <td>
                                <asp:UpdatePanel ID="Updp_Batchyear" runat="server">
                                    <ContentTemplate>
                                        <asp:TextBox ID="txt_batch" ReadOnly="true" Width=" 90px" runat="server" CssClass="textbox  txtheight2">--Select--</asp:TextBox>
                                        <asp:Panel ID="Panel_Batchyear" runat="server" CssClass="multxtpanel">
                                            <asp:CheckBox ID="cb_batch" runat="server" Text="Select All" AutoPostBack="true"
                                                OnCheckedChanged="cb_batch_CheckedChanged" />
                                            <asp:CheckBoxList ID="cbl_batch" runat="server" AutoPostBack="true" OnSelectedIndexChanged="cbl_batch_SelectedIndexChanged">
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
                                        <asp:Panel ID="Panel_Degree" runat="server" CssClass="multxtpanel">
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
                                        <asp:TextBox ID="txt_branch" Width=" 91px" ReadOnly="true" runat="server" CssClass="textbox  txtheight2">--Select--</asp:TextBox>
                                        <asp:Panel ID="Panel_dpt" runat="server" CssClass="multxtpanel" Height="350px">
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
                            <td>
                                <asp:Label ID="lbl_sem" runat="server" Text="Semester"></asp:Label>
                            </td>
                            <td>
                                <asp:UpdatePanel ID="UpdatePanel24" runat="server">
                                    <ContentTemplate>
                                        <asp:TextBox ID="txt_sem" Width="85px" ReadOnly="true" runat="server" CssClass="textbox  txtheight2">--Select--</asp:TextBox>
                                        <asp:Panel ID="Panel_Sem" runat="server" CssClass="multxtpanel">
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
                        </tr>
                    </table>
                    <table id="anonymousfilter2" runat="server">
                        <tr>
                            <td>
                                <asp:Label ID="lbl_Sec" runat="server" Text="Section"></asp:Label>
                            </td>
                            <td>
                                <asp:UpdatePanel ID="UpdatePanel25" runat="server">
                                    <ContentTemplate>
                                        <asp:TextBox ID="txt_sec" Width=" 90px" ReadOnly="true" runat="server" CssClass="textbox  txtheight2">--Select--</asp:TextBox>
                                        <asp:Panel ID="Panel_Sec" runat="server" CssClass="multxtpanel">
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
                                <asp:Label ID="fb_name" Width="115px" runat="server" Text="Feedback Name"></asp:Label>
                            </td>
                            <td>
                                <asp:UpdatePanel ID="UpdatePanel1" Visible="true" runat="server">
                                    <ContentTemplate>
                                        <asp:DropDownList ID="ddl_Feedbackname" runat="server" Height="30px" CssClass=" textbox1 ddlheight3"
                                            AutoPostBack="true" Font-Names="Book Antiqua" Font-Size="Medium" OnSelectedIndexChanged="ddl_Feedbackname_SelectedIndexChanged">
                                        </asp:DropDownList>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>
                            <td>
                                <asp:Label ID="lbl_subject" Width="100px" runat="server" Visible="true" Text="Subject Name"></asp:Label>
                            </td>
                            <td>
                                <asp:UpdatePanel ID="UpdatePanel5" Visible="true" runat="server">
                                    <ContentTemplate>
                                        <asp:TextBox ID="Txt_Subject" Width=" 93px" ReadOnly="true" runat="server" CssClass="textbox  txtheight2">--Select--</asp:TextBox>
                                        <asp:Panel ID="Panel_Subject" runat="server" CssClass="multxtpanel" Height="200px"
                                            Width="200px">
                                            <asp:CheckBox ID="Cb_Subject" runat="server" Text="Select All" AutoPostBack="True"
                                                OnCheckedChanged="Cb_Subject_CheckedChanged" />
                                            <asp:CheckBoxList ID="Cbl_Subject" runat="server" AutoPostBack="True" OnSelectedIndexChanged="Cbl_Subject_SelectedIndexChanged">
                                            </asp:CheckBoxList>
                                        </asp:Panel>
                                        <asp:PopupControlExtender ID="PopupControlExtender5" runat="server" TargetControlID="Txt_Subject"
                                            PopupControlID="Panel_Subject" Position="Bottom">
                                        </asp:PopupControlExtender>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>
                            <td>
                                <asp:Label ID="lbl_staffname" runat="server" Width="80px" Text="Staff Name"></asp:Label>
                            </td>
                            <td>
                                <asp:UpdatePanel ID="UpdatePanel8" runat="server">
                                    <ContentTemplate>
                                        <asp:TextBox ID="txt_staffname" Width=" 98px" ReadOnly="true" runat="server" CssClass="textbox  txtheight2">--Select--</asp:TextBox>
                                        <asp:Panel ID="Panel_staffname" runat="server" CssClass="multxtpanel" Height="200px"
                                            Width="200px">
                                            <asp:CheckBox ID="cb_staffname" runat="server" Text="Select All" AutoPostBack="True"
                                                OnCheckedChanged="cb_staffname_CheckedChanged" />
                                            <asp:CheckBoxList ID="cbl_staffname" runat="server" AutoPostBack="True" OnSelectedIndexChanged="cbl_staffname_SelectedIndexChanged">
                                            </asp:CheckBoxList>
                                        </asp:Panel>
                                        <asp:PopupControlExtender ID="PopupControlExtender11" runat="server" TargetControlID="txt_staffname"
                                            PopupControlID="Panel_staffname" Position="Bottom">
                                        </asp:PopupControlExtender>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>
                            <td>
                                <asp:UpdatePanel ID="UpdatePanel3" Visible="false" runat="server">
                                    <ContentTemplate>
                                        <asp:DropDownList ID="ddl_staffname" runat="server" Height="30px" CssClass="textbox textbox1 ddlheight3"
                                            AutoPostBack="true" Font-Names="Book Antiqua" Font-Size="Medium" OnSelectedIndexChanged="ddl_staffname_SelectedIndexChanged">
                                        </asp:DropDownList>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>
                            <td>
                                <asp:Label ID="lbl_stfsubject" Width="100px" runat="server" Visible="true" Text="Subject Name"></asp:Label>
                            </td>
                            <td>
                                <asp:UpdatePanel ID="UpdatePanel17" Visible="true" runat="server">
                                    <ContentTemplate>
                                        <asp:TextBox ID="Txt_stfSubject" Width=" 80px" ReadOnly="true" runat="server" CssClass="textbox  txtheight2">--Select--</asp:TextBox>
                                        <asp:Panel ID="Panel_stfSubject" runat="server" CssClass="multxtpanel" Height="200px"
                                            Width="200px">
                                            <asp:CheckBox ID="Cb_StfSubject" runat="server" Text="Select All" AutoPostBack="True"
                                                OnCheckedChanged="Cb_StfSubject_CheckedChanged" />
                                            <asp:CheckBoxList ID="Cbl_StfSubject" runat="server" AutoPostBack="True" OnSelectedIndexChanged="Cbl_StfSubject_SelectedIndexChanged">
                                            </asp:CheckBoxList>
                                        </asp:Panel>
                                        <asp:PopupControlExtender ID="PopupControlExtender17" runat="server" TargetControlID="Txt_stfSubject"
                                            PopupControlID="Panel_stfSubject" Position="Bottom">
                                        </asp:PopupControlExtender>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>
                            <td>
                                <asp:Label ID="lbl_question" runat="server" Text="Questions"></asp:Label>
                            </td>
                            <td>
                                <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                                    <ContentTemplate>
                                        <asp:TextBox ID="txt_question" Width=" 88px" ReadOnly="true" runat="server" CssClass="textbox  txtheight2">--Select--</asp:TextBox>
                                        <asp:Panel ID="Panel_question" runat="server" CssClass="multxtpanel" Height="200px"
                                            Width="200px">
                                            <asp:CheckBox ID="cb_question" runat="server" Text="Select All" AutoPostBack="True"
                                                OnCheckedChanged="cb_question_CheckedChanged" />
                                            <asp:CheckBoxList ID="cbl_question" runat="server" AutoPostBack="True" OnSelectedIndexChanged="cbl_question_SelectedIndexChanged">
                                            </asp:CheckBoxList>
                                        </asp:Panel>
                                        <asp:PopupControlExtender ID="PopupControlExtender1" runat="server" TargetControlID="txt_question"
                                            PopupControlID="Panel_question" Position="Bottom">
                                        </asp:PopupControlExtender>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="lbl_Critera" runat="server" Text="Criteria"></asp:Label>
                            </td>
                            <td>
                                <asp:DropDownList ID="ddl_criter" runat="server" CssClass="textbox1 ddlheight1">
                                    <asp:ListItem Value="0">Select</asp:ListItem>
                                    <asp:ListItem Value="1">Average</asp:ListItem>
                                </asp:DropDownList>
                            </td>
                            <td colspan="5">
                                <asp:Label ID="lbl_range" runat="server" Text="Range"></asp:Label>
                                <asp:TextBox ID="txtfrom_range" CssClass="textbox txtheight" runat="server"></asp:TextBox>
                                <asp:FilteredTextBoxExtender ID="filter" runat="server" TargetControlID="txtfrom_range"
                                    FilterType="Numbers,custom" ValidChars=".">
                                </asp:FilteredTextBoxExtender>
                                <asp:Label ID="Label1" runat="server" Text="To"></asp:Label>
                                <asp:TextBox ID="txtto_range" CssClass="textbox txtheight" runat="server"></asp:TextBox>
                                <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" runat="server" TargetControlID="txtto_range"
                                    FilterType="Numbers,custom" ValidChars=".">
                                </asp:FilteredTextBoxExtender>
                                <asp:CheckBox ID="cb_avgcolumn" Visible="false" runat="server" Text="InClude Average" />
                                &nbsp;&nbsp;&nbsp;<asp:Label ID="lbl_header" Text="   Show Header Type" runat="server"
                                    Visible="false"></asp:Label>
                                <asp:RadioButton ID="rdb_form4staffwise" Visible="false" runat="server" Text="Staff Wise"
                                    GroupName="form4type" />
                                <asp:RadioButton ID="rdb_form4questwise" Visible="false" runat="server" Text="Question Wise"
                                    GroupName="form4type" />
                            </td>
                            <td>
                                <asp:DropDownList ID="ddl_headershow" runat="server" CssClass="textbox1 ddlheight2"
                                    Visible="false">
                                    <asp:ListItem Value="0">Subject Name</asp:ListItem>
                                    <asp:ListItem Value="1">Subject Code</asp:ListItem>
                                    <asp:ListItem Value="2">Subject Code & Subject Name</asp:ListItem>
                                </asp:DropDownList>
                            </td>
                        </tr>
                    </table>
                    <table id="anonymousfilter3" runat="server">
                        <tr>
                            <td>
                                College Name
                            </td>
                            <td>
                                <asp:UpdatePanel ID="UpdatePanel4" runat="server">
                                    <ContentTemplate>
                                        <asp:TextBox ID="txtclgnameformat6" ReadOnly="true" runat="server" CssClass="textbox  txtheight4">--Select--</asp:TextBox>
                                        <asp:Panel ID="Panel1" runat="server" CssClass="multxtpanel">
                                            <asp:CheckBox ID="cb_clgnameformat6" runat="server" Text="Select All" AutoPostBack="True"
                                                OnCheckedChanged="cb_clgnameformat6_CheckedChanged" />
                                            <asp:CheckBoxList ID="cbl_clgnameformat6" runat="server" AutoPostBack="True" OnSelectedIndexChanged="cbl_clgnameformat6_SelectedIndexChanged">
                                            </asp:CheckBoxList>
                                        </asp:Panel>
                                        <asp:PopupControlExtender ID="PopupControlExtender2" runat="server" TargetControlID="txtclgnameformat6"
                                            PopupControlID="Panel1" Position="Bottom">
                                        </asp:PopupControlExtender>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>
                            <td>
                                <asp:Label ID="lbl_formate6batch" runat="server" Text="Batch Year"></asp:Label>
                            </td>
                            <td>
                                <asp:UpdatePanel ID="UpdatePanel12" runat="server">
                                    <ContentTemplate>
                                        <asp:TextBox ID="txt_formate6batch" ReadOnly="true" Width=" 90px" runat="server"
                                            CssClass="textbox  txtheight2">--Select--</asp:TextBox>
                                        <asp:Panel ID="Panel4" runat="server" CssClass="multxtpanel">
                                            <asp:CheckBox ID="cb_formate6batch" runat="server" Text="Select All" AutoPostBack="true"
                                                OnCheckedChanged="cb_formate6batch_CheckedChanged" />
                                            <asp:CheckBoxList ID="cbl_formate6batch" runat="server" AutoPostBack="true" OnSelectedIndexChanged="cbl_formate6batch_SelectedIndexChanged">
                                            </asp:CheckBoxList>
                                        </asp:Panel>
                                        <asp:PopupControlExtender ID="PopupControlExtender7" runat="server" TargetControlID="txt_formate6batch"
                                            PopupControlID="Panel4" Position="Bottom">
                                        </asp:PopupControlExtender>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>
                            <td>
                                Department
                            </td>
                            <td>
                                <asp:UpdatePanel ID="UpdatePanel9" runat="server">
                                    <ContentTemplate>
                                        <asp:DropDownList ID="ddlformate6_deptname" runat="server" OnSelectedIndexChanged="ddlformate6_deptname_selectedindex"
                                            AutoPostBack="true" CssClass="textbox1 ddlheight4">
                                        </asp:DropDownList>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                                <%-- <asp:UpdatePanel ID="UpdatePanel9" runat="server">
                            <ContentTemplate>
                                <asp:TextBox ID="txtdeptnameformat6" ReadOnly="true" runat="server" CssClass="textbox  txtheight4">--Select--</asp:TextBox>
                                <asp:Panel ID="Panel2" runat="server" CssClass="multxtpanel" Height="250px">
                                    <asp:CheckBox ID="cb_deptnameformat6" runat="server" Text="Select All" AutoPostBack="True"
                                        OnCheckedChanged="cb_deptnameformat6_CheckedChanged" />
                                    <asp:CheckBoxList ID="cbl_deptnameformat6" runat="server" AutoPostBack="True" OnSelectedIndexChanged="cbl_deptnameformat6_SelectedIndexChanged">
                                    </asp:CheckBoxList>
                                </asp:Panel>
                                <asp:PopupControlExtender ID="PopupControlExtender3" runat="server" TargetControlID="txtdeptnameformat6"
                                    PopupControlID="Panel2" Position="Bottom">
                                </asp:PopupControlExtender>
                            </ContentTemplate>
                        </asp:UpdatePanel>--%>
                            </td>
                            <td>
                                Semester
                            </td>
                            <td>
                                <asp:UpdatePanel ID="UpdatePanel13" runat="server">
                                    <ContentTemplate>
                                        <asp:TextBox ID="txt_formate6sem" ReadOnly="true" runat="server" CssClass="textbox  txtheight">--Select--</asp:TextBox>
                                        <asp:Panel ID="pformate6" runat="server" CssClass="multxtpanel" Height="250px">
                                            <asp:CheckBox ID="cb_formate6sem" runat="server" Text="Select All" AutoPostBack="True"
                                                OnCheckedChanged="cb_formate6sem_CheckedChanged" />
                                            <asp:CheckBoxList ID="cbl_formate6sem" runat="server" AutoPostBack="True" OnSelectedIndexChanged="cbl_formate6sem_SelectedIndexChanged">
                                            </asp:CheckBoxList>
                                        </asp:Panel>
                                        <asp:PopupControlExtender ID="PopupControlExtender8" runat="server" TargetControlID="txt_formate6sem"
                                            PopupControlID="pformate6" Position="Bottom">
                                        </asp:PopupControlExtender>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>
                            <%--   <%--Section added by saranya 0n 08/09/2018 for deparmentwise--%>
                            <td>
                                <asp:Label ID="LblSec" runat="server" Visible="false" Text="Section"></asp:Label>
                            </td>
                            <td>
                                <asp:UpdatePanel ID="UpSec" runat="server" Visible="false">
                                    <ContentTemplate>
                                        <asp:TextBox ID="txt_section" Width="90px" ReadOnly="true" runat="server" CssClass="textbox  txtheight2">--Select--</asp:TextBox>
                                        <asp:Panel ID="PnlsecDeptWise" runat="server" CssClass="multxtpanel">
                                            <asp:CheckBox ID="cb_secDeptWise" runat="server" Text="Select All" AutoPostBack="True"
                                                OnCheckedChanged="cb_secDeptWise_CheckedChanged" />
                                            <asp:CheckBoxList ID="cbl_secDeptWise" runat="server" AutoPostBack="True" OnSelectedIndexChanged="cbl_secDeptWise_SelectedIndexChanged">
                                            </asp:CheckBoxList>
                                        </asp:Panel>
                                        <asp:PopupControlExtender ID="PopupControlExtender3" runat="server" TargetControlID="txt_section"
                                            PopupControlID="PnlsecDeptWise" Position="Bottom">
                                        </asp:PopupControlExtender>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>
                            <%-------------------------------------------------------------------%>
                        </tr>
                        <tr>
                            <td>
                                Staff Name
                            </td>
                            <td>
                                <asp:UpdatePanel ID="UpdatePanel11" runat="server">
                                    <ContentTemplate>
                                        <asp:TextBox ID="txtstaffnameformat6" ReadOnly="true" runat="server" CssClass="textbox  txtheight4">--Select--</asp:TextBox>
                                        <asp:Panel ID="Panel3" runat="server" CssClass="multxtpanel" Height="200px" Width="200px">
                                            <asp:CheckBox ID="cb_staffnameformat6" runat="server" Text="Select All" AutoPostBack="True"
                                                OnCheckedChanged="cb_staffnameformat6_CheckedChanged" />
                                            <asp:CheckBoxList ID="cbl_staffnameformat6" runat="server" AutoPostBack="True" OnSelectedIndexChanged="cbl_staffnameformat6_SelectedIndexChanged">
                                            </asp:CheckBoxList>
                                        </asp:Panel>
                                        <asp:PopupControlExtender ID="PopupControlExtender4" runat="server" TargetControlID="txtstaffnameformat6"
                                            PopupControlID="Panel3" Position="Bottom">
                                        </asp:PopupControlExtender>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>
                            <td>
                                Feedback Name
                            </td>
                            <td colspan="3">
                                <asp:UpdatePanel ID="UpdatePanel10" Visible="true" runat="server">
                                    <ContentTemplate>
                                        <asp:DropDownList ID="ddl_feedbackformate6" runat="server" Width="260px" Height="30px"
                                            CssClass=" textbox1 ddlheight5" AutoPostBack="true" Font-Names="Book Antiqua"
                                            Font-Size="Medium" OnSelectedIndexChanged="ddl_feedbackformate6_SelectedIndexChanged">
                                        </asp:DropDownList>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>
                            <td>
                                <asp:CheckBox ID="cbBarChart" runat="server" Text="Barchart" Checked="true" />
                            </td>
                            <td>
                                <asp:Label ID="Label4" runat="server" Visible="false" Text="Subject Name"></asp:Label>
                            </td>
                            <td>
                                <asp:UpdatePanel ID="UpsubName" Visible="false" runat="server">
                                    <ContentTemplate>
                                        <asp:TextBox ID="txtsubjectnameformat6" ReadOnly="true" runat="server" CssClass="textbox  txtheight4">--Select--</asp:TextBox>
                                        <asp:Panel ID="PnlSubjectName" runat="server" CssClass="multxtpanel" Height="250px"
                                            Width="250px">
                                            <asp:CheckBox ID="cb_subjectnameformat6" runat="server" Text="Select All" AutoPostBack="True"
                                                OnCheckedChanged="cb_subjectnameformat6_CheckedChanged" />
                                            <asp:CheckBoxList ID="cbl_subjectnameformat6" runat="server" AutoPostBack="True"
                                                OnSelectedIndexChanged="cbl_subjectnameformat6_SelectedIndexChanged">
                                            </asp:CheckBoxList>
                                        </asp:Panel>
                                        <asp:PopupControlExtender ID="PopupControlExtenderSubjectName" runat="server" TargetControlID="txtsubjectnameformat6"
                                            PopupControlID="PnlSubjectName" Position="Bottom">
                                        </asp:PopupControlExtender>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>
                        </tr>
                    </table>
                    <table>
                        <tr>
                            <td colspan="9">
                                <table id="Acd" visible="true" runat="server">
                                    <%--delsi--%>
                                    <tr>
                                        <td>
                                            <asp:Label ID="lblreport_formate" runat="server" Text="Report Formate"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:DropDownList ID="ddl_Loginbasec" runat="server" CssClass="textbox1 ddlheight2"
                                                OnSelectedIndexChanged="ddl_SelectLogin_Changed" AutoPostBack="true" Visible="true">
                                                <asp:ListItem Value="1">Staff Wise Report</asp:ListItem>
                                                <asp:ListItem Value="2">Detailed Subject Wise Cumulative Report</asp:ListItem>
                                                <asp:ListItem Value="3">Student Count Report</asp:ListItem>
                                                <asp:ListItem Value="4">Staff Percentage Chart</asp:ListItem>
                                                <asp:ListItem Value="5">Questionwise Performance Chart</asp:ListItem>
                                                <asp:ListItem Value="6">Questionwise Average Chart</asp:ListItem>
                                                <asp:ListItem Value="7">Individual students wise Report</asp:ListItem>
                                            </asp:DropDownList>
                                        </td>
                                        <td>
                                            <asp:DropDownList ID="ddl_Anonyomous" runat="server" CssClass="textbox1 ddlheight2"
                                                OnSelectedIndexChanged="ddl_SelectAnontomous_Changed" AutoPostBack="true" Visible="true">
                                                <asp:ListItem Value="1">Questionwise Total Points</asp:ListItem>
                                                <asp:ListItem Value="2">Subject Wise / Questionwise Staff Average</asp:ListItem>
                                                <asp:ListItem Value="3">Student Count Report</asp:ListItem>
                                                <asp:ListItem Value="4">Staff Percentage Chart</asp:ListItem>
                                                <asp:ListItem Value="5">Questionwise Performance Chart</asp:ListItem>
                                                <%-- <asp:ListItem Value="6">Staff Evaluation Report</asp:ListItem>--%>
                                                <asp:ListItem Value="6">Department Wise Feedback</asp:ListItem>
                                                <asp:ListItem Value="7">Staff Wise Question Percentage Report</asp:ListItem>
                                            </asp:DropDownList>
                                        </td>
                                        <td style="margin-top: 1px;">
                                            <fieldset id="ind_login" runat="server" visible="false" style="height: 17px; width: 680px;
                                                background-color: #ffccff; margin-left: -6px; border-radius: 10px; border-color: #6699ee;
                                                margin-top: 10px;">
                                                <asp:RadioButton ID="rb_farmate1" Width="90px" runat="server" GroupName="format"
                                                    Text="Format1" OnCheckedChanged="rb_farmate1_CheckedChanged" AutoPostBack="true"
                                                    Checked="true"></asp:RadioButton>
                                                <asp:RadioButton ID="rb_farmate2" runat="server" Width="90px" GroupName="format"
                                                    Text="Format2" OnCheckedChanged="rb_farmate2_CheckedChanged" AutoPostBack="true">
                                                </asp:RadioButton>
                                                <asp:RadioButton ID="rb_farmate3" runat="server" Width="90px" GroupName="format"
                                                    Text="Format3" OnCheckedChanged="rb_farmate3_CheckedChanged" AutoPostBack="true">
                                                </asp:RadioButton>
                                                <asp:RadioButton ID="rb_farmate4" runat="server" Width="90px" GroupName="format"
                                                    Text="Format4" OnCheckedChanged="rb_farmate4_CheckedChanged" AutoPostBack="true">
                                                </asp:RadioButton>
                                                <asp:RadioButton ID="rb_farmate5" runat="server" OnCheckedChanged="rb_farmate5_CheckedChanged"
                                                    Width="90px" GroupName="format" Text="Format5" AutoPostBack="true"></asp:RadioButton>
                                                <asp:RadioButton ID="rb_farmate6" runat="server" OnCheckedChanged="rb_farmate6_CheckedChanged"
                                                    Width="90px" GroupName="format" Text="Format6" AutoPostBack="true"></asp:RadioButton>
                                                <asp:RadioButton ID="rb_farmate7" runat="server" OnCheckedChanged="rb_farmate7_CheckedChanged"
                                                    Width="90px" GroupName="format" Text="Format7" AutoPostBack="true"></asp:RadioButton>
                                            </fieldset>
                                        </td>
                                        <td>
                                            <fieldset id="form_2" runat="server" visible="false" style="height: 15px; background-color: InactiveCaption;
                                                border-radius: 10px; border-color: #F0F0F0; overflow: auto; margin-top: 6px;
                                                width: 309px;">
                                                <asp:RadioButton ID="rb_subject" runat="server" Width="80px" GroupName="format2"
                                                    Text="Subject" OnCheckedChanged="rb_subject_CheckedChanged" Checked="true" AutoPostBack="true">
                                                </asp:RadioButton>
                                                <asp:RadioButton ID="rb_type" runat="server" Width="80px" GroupName="format2" Text="Type"
                                                    OnCheckedChanged="rb_type_CheckedChanged" AutoPostBack="true"></asp:RadioButton>
                                                <asp:CheckBox ID="cb_total" runat="server" Text="Total" AutoPostBack="True" />
                                                <asp:CheckBox ID="cb_avg" runat="server" Text="Average" AutoPostBack="True" />
                                            </fieldset>
                                            <fieldset id="chart_selct" runat="server" visible="false" style="overflow: auto;
                                                height: 15px; background-color: InactiveCaption; margin-left: 0px; border-radius: 10px;
                                                border-color: #ffccff; margin-top: 6px;">
                                                <asp:RadioButton ID="rb_linchart" Visible="false" runat="server" Width="94px" GroupName="aa"
                                                    Text="LineChart" Checked="true" AutoPostBack="false"></asp:RadioButton>
                                                <asp:RadioButton ID="rb_barchart" Visible="false" runat="server" Width="85px" GroupName="aa"
                                                    Text="BarChart" AutoPostBack="false"></asp:RadioButton>
                                            </fieldset>
                                        </td>
                                        <td>
                                            <asp:Button ID="btn_go" Text="Go" runat="server" Visible="true" OnClick="btn_go_Click"
                                                Style="font-weight: bold; margin-left: 0px; font-family: book antiqua; font-size: medium;
                                                background-color: #6699ee; border-radius: 6px; margin-bottom: -12px;" />
                                        </td>
                                    </tr>
                                </table>
                                <table id="gend" visible="false" runat="server">
                                    <tr>
                                        <td>
                                            <fieldset style="height: 19px; background-color: #ffccff; margin-left: 0px; border-radius: 10px;
                                                border-color: #6699ee; overflow: auto; margin-top: 10px;">
                                                <asp:RadioButton ID="rb_gen_farmate1" Width="80px" runat="server" GroupName="gndformat"
                                                    Text="Format1" OnCheckedChanged="rb_gndfarmate1_CheckedChanged" AutoPostBack="false"
                                                    Checked="true"></asp:RadioButton>
                                                <asp:RadioButton ID="rb_gen_farmate2" Visible="true" runat="server" Width="80px"
                                                    GroupName="gndformat" Text="Format2" OnCheckedChanged="rb_gndfarmate2_CheckedChanged"
                                                    AutoPostBack="false" />
                                                <asp:Button ID="btn_gogen" Text="Go" runat="server" OnClick="btn_go1_Click" Style="font-weight: bold;
                                                    margin-left: 0px; font-family: book antiqua; font-size: medium; background-color: #6699ee;
                                                    border-radius: 6px; margin-bottom: -12px;" />
                                            </fieldset>
                                        </td>
                                    </tr>
                                </table>
                                <table id="acd_anonyms" visible="false" runat="server">
                                    <%--delsi--%>
                                    <tr>
                                        <td>
                                            <fieldset style="height: 23px; width: auto; background-color: #ffccff; margin-left: 0px;
                                                border-radius: 10px; border-color: #6699ee; overflow: auto; margin-top: 10px;">
                                                <table>
                                                    <tr>
                                                        <td colspan="2">
                                                            <asp:RadioButton ID="rb_anonyms_farmate1" Width="90px" runat="server" GroupName="anonformat"
                                                                Text="Format1" OnCheckedChanged="rb_anonyms_farmate1_CheckedChanged" AutoPostBack="true"
                                                                Checked="true"></asp:RadioButton>
                                                            <asp:RadioButton ID="rb_anonyms_farmate2" runat="server" Width="90px" GroupName="anonformat"
                                                                Text="Format2" OnCheckedChanged="rb_anonyms_farmate2_CheckedChanged" AutoPostBack="true" />
                                                            <asp:RadioButton ID="rb_anonyms_farmate3" runat="server" Width="90px" GroupName="anonformat"
                                                                Text="Format3" OnCheckedChanged="rb_anonyms_farmate3_CheckedChanged" AutoPostBack="true" />
                                                            <asp:RadioButton ID="rb_anonyms_farmate4" runat="server" Width="90px" GroupName="anonformat"
                                                                Text="Format4" OnCheckedChanged="rb_anonyms_farmate4_CheckedChanged" AutoPostBack="true" />
                                                            <asp:RadioButton ID="rb_anonyms_farmate5" runat="server" Width="90px" GroupName="anonformat"
                                                                Text="Format5" OnCheckedChanged="rb_anonyms_farmate5_CheckedChanged" AutoPostBack="true" />
                                                            <asp:RadioButton ID="rb_anonyms_farmate6" runat="server" Width="90px" GroupName="anonformat"
                                                                Text="Format6" OnCheckedChanged="rb_anonyms_farmate6_CheckedChanged" AutoPostBack="true" />
                                                            <asp:RadioButton ID="rb_anonyms_farmate7" runat="server" Width="90px" GroupName="anonformat"
                                                                Text="Format7" OnCheckedChanged="rb_anonyms_farmate7_CheckedChanged" AutoPostBack="true" />
                                                        </td>
                                                        <%--<td>
                                                            <div id="anonym_form1" runat="server" visible="false" style="overflow: auto; height: 23px;
                                                                background-color: InactiveCaption; margin-right: 0px; border-radius: 10px; border-color: #ffccff;
                                                                margin-top: 0px;">
                                                                <asp:RadioButton ID="rb_anonymsubject" Visible="true" runat="server" Width="100px"
                                                                    GroupName="anonym1" Text="Subject" Checked="true" AutoPostBack="true" OnCheckedChanged="rb_anonymsubject_CheckedChanged">
                                                                </asp:RadioButton>
                                                                <asp:RadioButton ID="rb_anonymcummulativ" Visible="true" runat="server" Width="   105px"
                                                                    GroupName="anonym1" Text="Cumulative" AutoPostBack="true" OnCheckedChanged="rb_anonymcummulativ_CheckedChanged">
                                                                </asp:RadioButton>
                                                                <asp:CheckBox ID="cb_total1" runat="server" Text="Total" Checked="true" AutoPostBack="false" />
                                                                <asp:CheckBox ID="cb_avg1" runat="server" Text="Average" Checked="true" AutoPostBack="false" />
                                                            </div>
                                                            <div id="anoynosformate4" runat="server" visible="false" style="overflow: auto; height: 23px;
                                                                background-color: InactiveCaption; margin-right: 0px; border-radius: 10px; border-color: #ffccff;
                                                                margin-top: 0px;">
                                                                <asp:RadioButton ID="rdb_line" runat="server" Width="100px" GroupName="f4" Text="LineChart"
                                                                    Checked="true" AutoPostBack="false"></asp:RadioButton>
                                                                <asp:RadioButton ID="rdb_bar" runat="server" Width="   99px" GroupName="f4" Text="BarChart"
                                                                    AutoPostBack="false"></asp:RadioButton>
                                                            </div>
                                                        </td>
                                                        <td>
                                                            <asp:Button ID="btn_goanonymous" Text="Go" runat="server" OnClick="btn_goanonymous_Click"
                                                                Style="font-weight: bold; margin-left: 0px; font-family: book antiqua; font-size: medium;
                                                                background-color: #6699ee; border-radius: 6px; margin-bottom: 0px;" />
                                                        </td>--%>
                                                    </tr>
                                                </table>
                                            </fieldset>
                                            <%-- <asp:Button ID="btn_gogen" runat="server" CssClass="textbox btn2" Text="Go" OnClick="btn_go1_Click" />
                                            --%>
                                        </td>
                                    </tr>
                                </table>
                                <%--<table id="Table1" visible="false" runat="server">
                                    <tr>
                                        <td>
                                            <fieldset style="height: 19px; background-color: #ffccff; margin-left: 0px; border-radius: 10px;
                                                border-color: #6699ee; overflow: auto; margin-top: 10px;">
                                                <asp:RadioButton ID="RadioButton1" Width="80px" runat="server" GroupName="gndformat"
                                                    Text="Format1" OnCheckedChanged="rb_gndfarmate1_CheckedChanged" AutoPostBack="false"
                                                    Checked="true"></asp:RadioButton>
                                                <asp:RadioButton ID="RadioButton2" runat="server" Width="80px" GroupName="gndformat"
                                                    Text="Format2" OnCheckedChanged="rb_gndfarmate2_CheckedChanged" AutoPostBack="false" />
                                                <asp:Button ID="Button1" Text="Go" runat="server" OnClick="btn_go1_Click" Style="font-weight: bold;
                                                    margin-left: 0px; font-family: book antiqua; font-size: medium; background-color: #6699ee;
                                                    border-radius: 6px; margin-bottom: -12px;" />
                                            </fieldset>

                                        </td>
                                    </tr>
                                </table>--%>
                            </td>
                            <td>
                                <table>
                                    <tr>
                                        <td>
                                            <div id="anonym_form1" runat="server" visible="false" style="overflow: auto; height: 23px;
                                                background-color: InactiveCaption; margin-right: 0px; border-radius: 10px; border-color: #ffccff;
                                                margin-top: 0px;">
                                                <asp:RadioButton ID="rb_anonymsubject" Visible="true" runat="server" Width="100px"
                                                    GroupName="anonym1" Text="Subject" Checked="true" AutoPostBack="true" OnCheckedChanged="rb_anonymsubject_CheckedChanged">
                                                </asp:RadioButton>
                                                <asp:RadioButton ID="rb_anonymcummulativ" Visible="true" runat="server" Width="   105px"
                                                    GroupName="anonym1" Text="Cumulative" AutoPostBack="true" OnCheckedChanged="rb_anonymcummulativ_CheckedChanged">
                                                </asp:RadioButton>
                                                <asp:CheckBox ID="cb_total1" runat="server" Text="Total" Checked="true" AutoPostBack="false" />
                                                <asp:CheckBox ID="cb_avg1" runat="server" Text="Average" Checked="true" AutoPostBack="false" />
                                            </div>
                                            <div id="anoynosformate4" runat="server" visible="false" style="overflow: auto; height: 23px;
                                                background-color: InactiveCaption; margin-right: 0px; border-radius: 10px; border-color: #ffccff;
                                                margin-top: 0px;">
                                                <asp:RadioButton ID="rdb_line" runat="server" Width="100px" GroupName="f4" Text="LineChart"
                                                    Checked="true" AutoPostBack="false"></asp:RadioButton>
                                                <asp:RadioButton ID="rdb_bar" runat="server" Width="   99px" GroupName="f4" Text="BarChart"
                                                    AutoPostBack="false"></asp:RadioButton>
                                            </div>
                                        </td>
                                        <td>
                                            <asp:Button ID="btn_goanonymous" Text="Go" runat="server" OnClick="btn_goanonymous_Click"
                                                Style="font-weight: bold; margin-left: 0px; font-family: book antiqua; font-size: medium;
                                                background-color: #6699ee; border-radius: 6px; margin-bottom: 0px;" />
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                    </table>
                </div>
                <div id="staff" runat="server" visible="false">
                    <table class="maintablestyle" width="971px" height="40px">
                        <tr>
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
                            <td>
                                <asp:Label ID="lbl_Designation" Width="50px" runat="server" Text="Designation"></asp:Label>
                            </td>
                            <td>
                                <asp:UpdatePanel ID="Updp_Designation" runat="server">
                                    <ContentTemplate>
                                        <asp:TextBox ID="txt_Designation" Width=" 90px" runat="server" CssClass="textbox  txtheight2"
                                            ReadOnly="true">--Select--</asp:TextBox>
                                        <asp:Panel ID="Panel_Designation" runat="server" CssClass="multxtpanel" Height="200px"
                                            Width="200px">
                                            <asp:CheckBox ID="cb_Designation" runat="server" Text="Select All" AutoPostBack="True"
                                                OnCheckedChanged="cb_Designation_CheckedChanged" />
                                            <asp:CheckBoxList ID="cbl_Designation" runat="server" AutoPostBack="True" OnSelectedIndexChanged="cbl_Designation_SelectedIndexChanged">
                                            </asp:CheckBoxList>
                                        </asp:Panel>
                                        <asp:PopupControlExtender ID="PopupControlExtender22" runat="server" TargetControlID="txt_Designation"
                                            PopupControlID="Panel_Designation" Position="Bottom">
                                        </asp:PopupControlExtender>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>
                            <td>
                                <asp:Label ID="lbl_staff_type" runat="server" Text="Category_Name"></asp:Label>
                            </td>
                            <td>
                                <asp:UpdatePanel ID="UpdatePanel6" runat="server">
                                    <ContentTemplate>
                                        <asp:TextBox ID="Txt_stafftype" Width=" 125px" ReadOnly="true" runat="server" CssClass="textbox  txtheight2">--Select--</asp:TextBox>
                                        <asp:Panel ID="Panel_stafftype" runat="server" CssClass="multxtpanel" Height="200px"
                                            Width="200px">
                                            <asp:CheckBox ID="Cb_stafftype" runat="server" Text="Select All" AutoPostBack="True"
                                                OnCheckedChanged="Cb_stafftype_CheckedChanged" />
                                            <asp:CheckBoxList ID="Cbl_stafftype" runat="server" AutoPostBack="True" OnSelectedIndexChanged="Cbl_stafftype_SelectedIndexChanged">
                                            </asp:CheckBoxList>
                                        </asp:Panel>
                                        <asp:PopupControlExtender ID="PopupControlExtender6" runat="server" TargetControlID="Txt_stafftype"
                                            PopupControlID="Panel_stafftype" Position="Bottom">
                                        </asp:PopupControlExtender>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>
                            <td>
                                <asp:Button ID="btn_go_gend" runat="server" CssClass="textbox btn2" Text="Go" OnClick="btn_go_gend_Click" />
                            </td>
                        </tr>
                    </table>
                </div>
                <div id="search" runat="server" visible="false">
                    <center>
                        <table>
                            <tr>
                                <td colspan="2">
                                    <asp:Label ID="lbl_points" runat="server" Text="Points"></asp:Label>
                                    <asp:TextBox ID="Txt_point_from" Width=" 90px" runat="server" CssClass="textbox  txtheight2"></asp:TextBox>
                                    <asp:TextBox ID="Txt_point_to" Width=" 90px" runat="server" CssClass="textbox  txtheight2"></asp:TextBox>
                                    <asp:Button ID="btn_search" Visible="false" runat="server" CssClass="textbox btn2"
                                        Text="Search" OnClick="btn_search_Click" />
                                </td>
                                <td colspan="2">
                                    <asp:RadioButton ID="rb_column_order" Width="133px" runat="server" GroupName="order"
                                        Text="Column_Order" OnCheckedChanged="rb_Acad1_CheckedChanged" AutoPostBack="false"
                                        Checked="true"></asp:RadioButton>
                                    <asp:RadioButton ID="rb_order" runat="server" Width="100px" GroupName="order" Text="Order"
                                        OnCheckedChanged="rb_Gend1_CheckedChanged" AutoPostBack="false"></asp:RadioButton>
                                </td>
                                <td colspan="2">
                                    <asp:RadioButton ID="rb_cumulative" Width="110px" runat="server" GroupName="search"
                                        Text="Cumulative" OnCheckedChanged="rb_cumulative_CheckedChanged" AutoPostBack="true"
                                        Checked="true"></asp:RadioButton>
                                    <asp:RadioButton ID="rb_indiv" runat="server" Width="100px" GroupName="search" Text="Indivdgual"
                                        OnCheckedChanged="rb_indiv_CheckedChanged" AutoPostBack="true"></asp:RadioButton>
                                    <asp:TextBox ID="txt_indivdgual" Visible="false" Width=" 160px" runat="server" CssClass="textbox  txtheight2"></asp:TextBox>
                                    <asp:Button ID="btn_ind_search" Visible="false" runat="server" CssClass="textbox btn2"
                                        Text="Search" OnClick="btn_ind_search_Click" />
                                </td>
                            </tr>
                        </table>
                    </center>
                </div>
                <br />
                <div>
                    <center>
                        <asp:Label ID="lbl_headig" runat="server" Style="width: auto;" Font-Bold="true" ForeColor="Indigo"
                            Font-Size="X-Large" Text=""></asp:Label>
                    </center>
                </div>
                <br />
                <center>
                    <div>
                        <FarPoint:FpSpread ID="Fpspread6" runat="server" Visible="false" AutoPostBack="true"
                            BorderWidth="0px" Style="overflow: auto; height: 400px; border: 0px solid #999999;
                            border-radius: 10px; background-color: White; box-shadow: 0px 0px 8px #999999;">
                            <Sheets>
                                <FarPoint:SheetView SheetName="Sheet1" SelectionBackColor="#0CA6CA">
                                </FarPoint:SheetView>
                            </Sheets>
                        </FarPoint:FpSpread>
                    </div>
                </center>
                <center>
                    <div id="rptprint1" runat="server" visible="false">
                        <br />
                        <asp:Label ID="lbl_norec1" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                            Font-Size="Medium" ForeColor="Red" Text="Please Enter Your Report Name" Visible="false"></asp:Label>
                        <asp:Label ID="lblrptname1" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                            Font-Size="Medium" Text="Report Name"></asp:Label>
                        <%--  --%>
                        <asp:TextBox ID="txtexcelname1" runat="server" CssClass="textbox textbox1" Height="20px"
                            Width="180px" onkeypress="display1()" Style="font-family: 'Book Antiqua'" Font-Bold="True"
                            Font-Names="Book Antiqua" Font-Size="Medium"></asp:TextBox>
                        <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender9" runat="server" TargetControlID="txtexcelname1"
                            FilterType="LowercaseLetters,UppercaseLetters,Numbers,Custom" ValidChars="(),.[]_"
                            InvalidChars="/\">
                        </asp:FilteredTextBoxExtender>
                        <asp:Button ID="btnExcel1" runat="server" Font-Bold="true" Font-Names="Book Antiqua"
                            OnClick="btnExcel1_Click" Font-Size="Medium" Text="Export To Excel" Width="127px"
                            Height="31px" CssClass="textbox textbox1" />
                        <asp:Button ID="btnprintmaster1" runat="server" Text="Print" OnClick="btnprintmaster1_Click"
                            Font-Names="Book Antiqua" Font-Size="Medium" Font-Bold="true" Width="60px" Height="31px"
                            CssClass="textbox textbox1" />
                        <asp:Button ID="btn_printperticulaterstaff" runat="server" Text="Print Particular Staff"
                            OnClick="btn_printperticulaterstaff_click" Visible="false" Font-Names="Book Antiqua"
                            Font-Size="Medium" Font-Bold="true" Width="175px" Height="31px" CssClass="textbox textbox1" />
                        <Insproplus:printmaster runat="server" ID="Printcontrol1" Visible="false" />
                    </div>
                    <br />
                </center>
                <center>
                    <%--<div id="div1" visible="false" runat="server" class="spreadborder" style="width: 817px;
                height: 330px; overflow: auto; background-color: White; border-radius: 10px;">--%>
                    <FarPoint:FpSpread ID="FpSpread1" Width="900px" runat="server" Visible="false" BorderStyle="Solid"
                        BorderWidth="0px" CssClass="spreadborder" OnCellClick="FpSpread1_OnCellClick"
                        ShowHeaderSelection="false" OnPreRender="FpSpread1_Selectedindexchange" OnUpdateCommand="FpSpread1_Command">
                        <Sheets>
                            <FarPoint:SheetView SheetName="Sheet1">
                            </FarPoint:SheetView>
                        </Sheets>
                    </FarPoint:FpSpread>
                    <FarPoint:FpSpread ID="FpSpread2" Width="971px" runat="server" Visible="false" BorderStyle="Solid"
                        BorderWidth="0px" CssClass="spreadborder" OnCellClick="FpSpread2_OnCellClick"
                        ShowHeaderSelection="false" OnPreRender="FpSpread2_Selectedindexchange">
                        <Sheets>
                            <FarPoint:SheetView SheetName="Sheet1">
                            </FarPoint:SheetView>
                        </Sheets>
                    </FarPoint:FpSpread>
                    <FarPoint:FpSpread ID="FpSpread3" Width="971px" runat="server" Visible="false" BorderStyle="Solid"
                        BorderWidth="0px" CssClass="spreadborder" OnCellClick="FpSpread3_OnCellClick"
                        OnPreRender="FpSpread3_Selectedindexchange">
                        <Sheets>
                            <FarPoint:SheetView SheetName="Sheet1">
                            </FarPoint:SheetView>
                        </Sheets>
                    </FarPoint:FpSpread>
                    <div id="stud_div" runat="server" visible="false" width="900px" style="height: auto;">
                        <FarPoint:FpSpread ID="FpSpread4" runat="server" Visible="false" BorderStyle="Solid"
                            BorderWidth="0px" CssClass="spreadborder">
                            <Sheets>
                                <FarPoint:SheetView SheetName="Sheet1">
                                </FarPoint:SheetView>
                            </Sheets>
                        </FarPoint:FpSpread>
                    </div>
                    <%--</div>--%>
                    <br />
                    <br />
                    <div id="chartdiv" runat="server">
                        <asp:GridView ID="chart_staff_chart" runat="server" Visible="false" Font-Names="Book Antiqua"
                            Font-Size="Medium">
                        </asp:GridView>
                        <asp:Chart ID="staff_chart" runat="server" Height="700px" Font-Names="Book Antiqua"
                            Font-Size="Medium" Visible="false">
                            <%--Width="800px"--%>
                            <Series>
                            </Series>
                            <Legends>
                                <asp:Legend Title="Staff Chart" Docking="Bottom" ShadowOffset="2" Font="Book Antiqua">
                                </asp:Legend>
                            </Legends>
                            <Titles>
                                <asp:Title Docking="Top" Text="STAFF PERCENTAGE" Font="Microsoft Sans Serif, 12pt">
                                </asp:Title>
                                <asp:Title Docking="Bottom" Font="Book Antiqua" Text="Question">
                                </asp:Title>
                                <asp:Title Docking="Left" Font="Book Antiqua" Text="Points">
                                </asp:Title>
                            </Titles>
                            <ChartAreas>
                                <asp:ChartArea Name="ChartArea1" BorderWidth="0">
                                    <AxisY LineColor="White">
                                        <LabelStyle Font="Trebuchet MS, 8.25pt" />
                                        <MajorGrid LineColor="#e6e6e6" />
                                        <MinorGrid Enabled="false" LineColor="#e6e6e6" />
                                    </AxisY>
                                    <AxisX LineColor="White">
                                        <LabelStyle Font="Trebuchet MS, 8.25pt" />
                                        <MajorGrid LineColor="#e6e6e6" />
                                        <MinorGrid Enabled="false" LineColor="#e6e6e6" />
                                    </AxisX>
                                    <%-- <Area3DStyle Enable3D="true" WallWidth="10"/>--%>
                                </asp:ChartArea>
                            </ChartAreas>
                        </asp:Chart>
                        <asp:Chart ID="question_chart" runat="server" Height="1000px" Visible="false" Font-Names="Book Antiqua"
                            EnableViewState="true" Font-Size="Medium">
                            <Series>
                            </Series>
                            <Legends>
                                <asp:Legend Title="Question wise Charts" ShadowOffset="2" Docking="Bottom" Font="Book Antiqua">
                                </asp:Legend>
                            </Legends>
                            <Titles>
                                <asp:Title Docking="Top" Text="Questions Wise Chart" Font="Microsoft Sans Serif, 12pt">
                                </asp:Title>
                                <asp:Title Docking="Bottom" Font="Book Antiqua" Text="Staff Name">
                                </asp:Title>
                                <asp:Title Docking="Left" Font="Book Antiqua" Text="Question">
                                </asp:Title>
                            </Titles>
                            <ChartAreas>
                                <asp:ChartArea Name="ChartArea2" BorderWidth="0">
                                    <AxisY LineColor="White">
                                        <LabelStyle Font="Trebuchet MS, 8.25pt" />
                                        <MajorGrid LineColor="#e6e6e6" />
                                        <MinorGrid Enabled="false" LineColor="#e6e6e6" />
                                    </AxisY>
                                    <AxisX LineColor="White">
                                        <LabelStyle Font="Trebuchet MS, 8.25pt" />
                                        <MajorGrid LineColor="#e6e6e6" />
                                        <MinorGrid Enabled="false" LineColor="#e6e6e6" />
                                    </AxisX>
                                </asp:ChartArea>
                            </ChartAreas>
                        </asp:Chart>
                        <asp:Chart ID="Total_points" runat="server" Height="500px" Visible="false" Font-Names="Book Antiqua"
                            EnableViewState="true" Font-Size="Medium">
                            <%--Width="800px"--%>
                            <Series>
                            </Series>
                            <Legends>
                            </Legends>
                            <Titles>
                                <asp:Title Docking="Top" Text="Questions Wise Average for (5%)" Font="Microsoft Sans Serif, 12pt">
                                </asp:Title>
                                <asp:Title Docking="Bottom" Font="Book Antiqua, 14pt" Text="question">
                                </asp:Title>
                                <asp:Title Docking="Left" Font="Book Antiqua, 12pt" Text=" Total Points">
                                </asp:Title>
                            </Titles>
                            <ChartAreas>
                                <asp:ChartArea Name="ChartArea3" BorderWidth="0">
                                    <AxisY LineColor="White">
                                        <LabelStyle Font="Trebuchet MS, 8.25pt" />
                                        <MajorGrid LineColor="#e6e6e6" />
                                        <MinorGrid Enabled="false" LineColor="#e6e6e6" />
                                    </AxisY>
                                    <AxisX LineColor="White">
                                        <LabelStyle Font="Trebuchet MS, 8.25pt" />
                                        <MajorGrid LineColor="#e6e6e6" />
                                        <MinorGrid Enabled="false" LineColor="#e6e6e6" />
                                    </AxisX>
                                </asp:ChartArea>
                            </ChartAreas>
                        </asp:Chart>
                    </div>
                </center>
                <br />
                <center>
                    <div id="chartprint" runat="server" visible="false">
                        <asp:Button ID="btnprintimag" Text="Chart Print To PDF" Height="30px" runat="server"
                            CssClass="btn1 textbox txtheight2" OnClientClick="return PrintPanel();" /><%--OnClick="btnExportPDF_Click"--%>
                    </div>
                </center>
                <center>
                    <div id="popupquestiondet" runat="server" visible="false" class="popupstyle popupheight1">
                        <asp:ImageButton ID="ImageButton1" runat="server" Width="40px" Height="40px" ImageUrl="~/images/close.png"
                            Style="height: 30px; width: 30px; position: absolute; margin-top: 27px; margin-left: 470px;"
                            OnClick="imagebtnpopclose1_Click" />
                        <br />
                        <br />
                        <div style="background-color: White; height: 500px; width: 965px; border: 5px solid #0CA6CA;
                            border-top: 30px solid #0CA6CA; border-radius: 10px;">
                            <br />
                            <br />
                            <div>
                                <asp:Label ID="lbl_errormsg1" Visible="false" runat="server" Text="" ForeColor="Red"></asp:Label>
                            </div>
                            <div>
                                <FarPoint:FpSpread ID="Fpspread5" runat="server" Visible="false" AutoPostBack="true"
                                    BorderWidth="0px" Style="overflow: auto; height: 400px; border: 0px solid #999999;
                                    border-radius: 10px; background-color: White; box-shadow: 0px 0px 8px #999999;">
                                    <Sheets>
                                        <FarPoint:SheetView SheetName="Sheet1" SelectionBackColor="#0CA6CA">
                                        </FarPoint:SheetView>
                                    </Sheets>
                                </FarPoint:FpSpread>
                            </div>
                            <center>
                                <div id="reportdivquestiondet" runat="server" visible="false">
                                    <br />
                                    <asp:Label ID="Label2" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                        Font-Size="Medium" ForeColor="Red" Text="Please Enter Your Report Name" Visible="false"></asp:Label>
                                    <asp:Label ID="Label3" runat="server" Text="Report Name"></asp:Label>
                                    <asp:TextBox ID="txt_questionreport" runat="server" CssClass="textbox textbox1" Height="20px"
                                        Width="180px" onkeypress="display1()"></asp:TextBox>
                                    <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender2" runat="server" TargetControlID="txt_questionreport"
                                        FilterType="LowercaseLetters,UppercaseLetters,Numbers,Custom" ValidChars="(),.[]_"
                                        InvalidChars="/\">
                                    </asp:FilteredTextBoxExtender>
                                    <asp:Button ID="btn_quest" runat="server" OnClick="btn_quest_Click" Text="Export To Excel"
                                        Width="127px" Height="31px" CssClass="textbox textbox1" />
                                    <asp:Button ID="btn_questprint" runat="server" Text="Print" OnClick="btn_questprint_Click"
                                        Width="60px" Height="31px" CssClass="textbox textbox1" />
                                    <Insproplus:printmaster runat="server" ID="Printmaster1" Visible="false" />
                                </div>
                                <br />
                            </center>
                        </div>
                    </div>
                </center>
                <center>
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
            </div>
        </div>
    </body>
</asp:Content>

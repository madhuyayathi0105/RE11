<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/FeedBackMOD/FeedbackSubSiteMaster.master"
    CodeFile="generaldescriptivefeedbackreport.aspx.cs" Inherits="generaldescriptivefeedbackreport" %>

<%@ Register Src="~/Usercontrols/NewPrintMaster.ascx" TagName="NEWPrintMater" TagPrefix="NEW" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <link href="~/Styles/css/Commoncss.css" rel="Stylesheet" type="text/css" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">
    <!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
    <html xmlns="http://www.w3.org/1999/xhtml">
    <title></title>
    <script src="//ajax.googleapis.com/ajax/libs/jquery/1.11.1/jquery.min.js"></script>
    <link href="~/Styles/css/Commoncss.css" rel="stylesheet" type="text/css" />
    <body>
        <script type="text/javascript">
            function display() {

                document.getElementById('MainContent_errlbl').innerHTML = "";
                document.getElementById('MainContent_lbl_norec').innerHTML = "";
            }
            function PrintPanel() {

                var panel = document.getElementById("<%=divMainContents.ClientID %>");
                var printWindow = window.open('', '', 'height=842,width=1191');
                printWindow.document.write('<html');
                printWindow.document.write('<head>');
                printWindow.document.write('</head><body >');
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
        <br />
        <center>
            <div style="width: auto;">
                <center>
                    <div>
                        <span class="fontstyleheader" style="color: Green">General Descriptive Feedback Report</span>
                    </div>
                    <br />
                </center>
                <asp:UpdatePanel ID="UpdatePanel8" runat="server">
                    <ContentTemplate>
                        <div class="maindivstyle">
                            <center>
                                <table class="maintablestyle">
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
                                                    <asp:Panel ID="Panel_Batchyear" runat="server" Height="200" CssClass="multxtpanel">
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
                                                    <asp:Panel ID="Panel_Degree" runat="server" Height="200" CssClass="multxtpanel">
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
                                        <td colspan="2">
                                            <asp:UpdatePanel ID="UpdatePanel1" Visible="true" runat="server">
                                                <ContentTemplate>
                                                    <asp:DropDownList ID="ddl_Feedbackname" runat="server" Height="30px" CssClass=" textbox1 ddlheight4"
                                                        AutoPostBack="true" OnSelectedIndexChanged="ddl_Feedbackname_SelectedIndexChanged">
                                                    </asp:DropDownList>
                                                </ContentTemplate>
                                            </asp:UpdatePanel>
                                        </td>
                                        <td>
                                            <asp:UpdatePanel ID="UpdatePanel2" Visible="true" runat="server">
                                                <ContentTemplate>
                                                    <asp:Button ID="btn_go" runat="server" CssClass="textbox btn1" Text="Go" OnClick="btn_go_Click" />
                                                </ContentTemplate>
                                            </asp:UpdatePanel>
                                        </td>
                                    </tr>
                                </table>
                                <br />
                                <br />
                                <div id="divMainContents" runat="server" style="display: table; margin: 0px; height: auto;
                                    margin-bottom: 20px; margin-top: 10px; position: relative; width: auto; text-align: left;">
                                    <asp:GridView ID="Showgrid" Style="height: auto; width: auto;" runat="server" Visible="false"
                                        HeaderStyle-ForeColor="Black" HeaderStyle-BackColor="#0CA6CA" AutoGenerateColumns="true"
                                        ShowHeaderWhenEmpty="true" OnRowDataBound="Showgrid_OnRowDataBound">
                                    </asp:GridView>
                                </div>
                                <asp:Label ID="lbl_error" runat="server" ForeColor="Red"></asp:Label>
                                <br />
                            </center>
                        </div>
                        </br>
                        <asp:Label ID="lbl_norec" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                            Font-Size="Medium" ForeColor="#FF3300" Text="" Visible="False">
                        </asp:Label></center>
                        <div id="div_report" runat="server" visible="false">
                            <center>
                                <asp:Label ID="lbl_reportname" runat="server" Text="Report Name" Font-Bold="True"
                                    Font-Names="Book Antiqua" Font-Size="Medium"></asp:Label>
                                <asp:TextBox ID="txt_excelname" runat="server" AutoPostBack="true" OnTextChanged="txtexcelname_TextChanged"
                                    CssClass="textbox textbox1 txtheight5" onkeypress="display()"></asp:TextBox>
                                <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender56" runat="server" TargetControlID="txt_excelname"
                                    FilterType="LowercaseLetters,UppercaseLetters,Numbers,Custom" ValidChars="!@$%^&()_+}{][';,.">
                                </asp:FilteredTextBoxExtender>
                                <asp:Button ID="btn_Excel" runat="server" Text="Export To Excel" Width="150px" CssClass="textbox btn2"
                                    AutoPostBack="true" OnClick="btnExcel_Click" />
                                <asp:Button ID="btn_printmaster" runat="server" Text="Print" CssClass="textbox btn2"
                                    AutoPostBack="true" OnClick="btn_printmaster_Click" />
                                <NEW:NEWPrintMater runat="server" ID="Printcontrol" Visible="false" />
                                <asp:Button ID="btnPrint" runat="server" Text="Direct Print" OnClientClick="return PrintPanel();"
                                    Font-Names="Book Antiqua" Position="absolute" Font-Size="Medium" Font-Bold="true" Height="35px" CssClass="textbox textbox1" />
                            </center>
                        </div>
                    </ContentTemplate>
                    <Triggers>
                        <asp:PostBackTrigger ControlID="btn_Excel" />
                        <asp:PostBackTrigger ControlID="btn_printmaster" />
                        <asp:PostBackTrigger ControlID="btnPrint" />
                    </Triggers>
                </asp:UpdatePanel>
            </div>
        </center>
        <center>
            <asp:UpdateProgress ID="UpdateProgress1" runat="server" AssociatedUpdatePanelID="UpdatePanel2">
                <ProgressTemplate>
                    <center>
                        <div style="height: 40px; width: 150px;">
                            <img src="../gv images/cloud_loading_256.gif" style="height: 150px;" />
                            <br />
                            <span style="font-family: Book Antiqua; font-size: medium; font-weight: bold; color: Black;">
                                Processing Please Wait...</span>
                        </div>
                    </center>
                </ProgressTemplate>
            </asp:UpdateProgress>
            <asp:ModalPopupExtender ID="ModalPopupExtender1" runat="server" TargetControlID="UpdateProgress1"
                PopupControlID="UpdateProgress1">
            </asp:ModalPopupExtender>
        </center>
    </body>
    </html>
</asp:Content>

﻿<%@ Page Title="Staff Key List" Language="C#" MasterPageFile="~/AttendanceMOD/AttendanceSubSiteMaster.master"
    AutoEventWireup="true" CodeFile="StaffKeyListDetails.aspx.cs" Inherits="AttendanceMOD_StaffKeyListDetails" %>

<%@ Register Assembly="FarPoint.Web.Spread,  Version=5.0.3520.2008, Culture=neutral, PublicKeyToken=327c3516b1b18457"
    Namespace="FarPoint.Web.Spread" TagPrefix="FarPoint" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <link href="~/Styles/css/Commoncss.css" rel="Stylesheet" type="text/css" />
    <script src="//ajax.googleapis.com/ajax/libs/jquery/1.11.1/jquery.min.js"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <center>
        <div>
            
            <span class="fontstyleheader" style="color: Green; margin: 0px; margin-bottom: 10px;
                margin-top: 10px; position: relative;">Staff Key List</span>
        </div>
        <div id="divSearch" runat="server" visible="true" class="maindivstyle" style="width: 100%;
            height: auto; margin: 0px; margin-bottom: 20px; margin-top: 10px; padding: 5px;
            position: relative;">
            <div id="divStaffHeader" runat="server" visible="false">
                <table style="height: auto; margin-left: 0px; margin-top: 3px; padding: 5px;">
                    <tr>
                        <td>
                            <asp:Label ID="lblSubject" runat="server" Text="Subject" Font-Bold="True" Style="font-family: 'Book Antiqua';"
                                ForeColor="Black" Font-Size="Medium" Font-Names="Book Antiqua"></asp:Label>
                        </td>
                        <td>
                            <%-- <asp:DropDownList ID="ddlSubject" runat="server" CssClass="dropdown" Style="font-family: 'Book Antiqua';"
                                Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium" Width="250px" AutoPostBack="True"
                                OnSelectedIndexChanged="ddlSubject_SelectedIndexChanged">
                            </asp:DropDownList>--%>
                            <div style="position: relative;">
                                <asp:UpdatePanel ID="upnlSubjectSF" runat="server">
                                    <ContentTemplate>
                                        <asp:TextBox ID="txtStaffSubject" Width="120px" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                            Font-Size="Medium" CssClass="textbox  txtheight2" ReadOnly="true">--Select--</asp:TextBox>
                                        <asp:Panel ID="pnlStaffSubject" runat="server" CssClass="multxtpanel" Style="width: 270px;
                                            height: 250px; overflow: auto; margin: 0px; padding: 0px;">
                                            <asp:CheckBox ID="chkStaffSubject" Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium"
                                                runat="server" Text="Select All" AutoPostBack="True" Style="width: 100%; height: auto;
                                                margin: 0px; padding: 0px; border: 0px;" OnCheckedChanged="chkStaffSubject_CheckedChanged" />
                                            <asp:CheckBoxList ID="cblStaffSubject" Font-Bold="True" Font-Size="Medium" Font-Names="Book Antiqua"
                                                runat="server" AutoPostBack="True" Style="width: 100%; height: auto; margin: 0px;
                                                padding: 0px; border: 0px;" OnSelectedIndexChanged="cblStaffSubject_SelectedIndexChanged">
                                            </asp:CheckBoxList>
                                        </asp:Panel>
                                        <asp:PopupControlExtender ID="popExtStaffSub" runat="server" TargetControlID="txtStaffSubject"
                                            PopupControlID="pnlStaffSubject" Position="Bottom">
                                        </asp:PopupControlExtender>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </div>
                        </td>
                        <td>
                            <asp:Button ID="btnGo" CssClass="textbox textbox1" runat="server" Font-Bold="True"
                                Font-Size="Medium" Font-Names="Book Antiqua" Style="width: auto; height: auto;"
                                Text="Go" OnClick="btnGo_Click" />
                        </td>
                    </tr>
                </table>
            </div>
            <div id="divOther" runat="server" visible="false">
                <table class="maintablestyle" style="height: auto; margin-left: 0px; margin-top: 5px;
                    margin-bottom: 5px; padding: 5px;">
                    <tr>
                        <td>
                            <asp:Label ID="lblCollege" runat="server" Text="College" Font-Bold="True" Style="font-family: 'Book Antiqua';"
                                ForeColor="Black" Font-Size="Medium" Font-Names="Book Antiqua"></asp:Label>
                        </td>
                        <td>
                            <div style="position: relative;">
                                <asp:UpdatePanel ID="upnlCollege" runat="server">
                                    <ContentTemplate>
                                        <asp:TextBox ID="txtCollege" Width="120px" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                            Font-Size="Medium" CssClass="textbox  txtheight2" ReadOnly="true">--Select--</asp:TextBox>
                                        <asp:Panel ID="pnlCollege" runat="server" CssClass="multxtpanel" Style="width: 330px;
                                            height: auto; overflow: auto; margin: 0px; padding: 0px;">
                                            <asp:CheckBox ID="chkCollege" Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium"
                                                runat="server" Text="Select All" AutoPostBack="True" Style="width: 100%; height: auto;
                                                margin: 0px; padding: 0px; border: 0px;" OnCheckedChanged="chkCollege_CheckedChanged" />
                                            <asp:CheckBoxList ID="cblCollege" Font-Bold="True" Font-Size="Medium" Font-Names="Book Antiqua"
                                                runat="server" AutoPostBack="True" Style="width: 100%; height: auto; margin: 0px;
                                                padding: 0px; border: 0px;" OnSelectedIndexChanged="cblCollege_SelectedIndexChanged">
                                            </asp:CheckBoxList>
                                        </asp:Panel>
                                        <asp:PopupControlExtender ID="popExtCollege" runat="server" TargetControlID="txtCollege"
                                            PopupControlID="pnlCollege" Position="Bottom">
                                        </asp:PopupControlExtender>
                                        <%--<asp:DropDownList ID="ddlCollege" runat="server" CssClass="dropdown" Style="font-family: 'Book Antiqua';"
                                            Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium" Width="200px" AutoPostBack="True"
                                            OnSelectedIndexChanged="ddlCollege_SelectedIndexChanged">
                                        </asp:DropDownList>--%>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </div>
                        </td>
                        <td>
                            <asp:Label ID="lblBatch" runat="server" Text="Batch" Font-Bold="True" Style="font-family: 'Book Antiqua';"
                                ForeColor="Black" Font-Size="Medium" Font-Names="Book Antiqua"></asp:Label>
                        </td>
                        <td>
                            <div style="position: relative;">
                                <asp:UpdatePanel ID="upnlBatch" runat="server">
                                    <ContentTemplate>
                                        <asp:TextBox ID="txtBatch" Width="60px" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                            Font-Size="Medium" CssClass="textbox  txtheight2" ReadOnly="true">--Select--</asp:TextBox>
                                        <asp:Panel ID="pnlBatch" runat="server" CssClass="multxtpanel" Style="width: 120px;
                                            height: auto; overflow: auto; margin: 0px; padding: 0px;">
                                            <asp:CheckBox ID="chkBatch" Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium"
                                                runat="server" Text="Select All" AutoPostBack="True" Style="width: 100%; height: auto;
                                                margin: 0px; padding: 0px; border: 0px;" OnCheckedChanged="chkBatch_CheckedChanged" />
                                            <asp:CheckBoxList ID="cblBatch" Font-Bold="True" Font-Size="Medium" Font-Names="Book Antiqua"
                                                runat="server" AutoPostBack="True" Style="width: 100%; height: auto; margin: 0px;
                                                padding: 0px; border: 0px;" OnSelectedIndexChanged="cblBatch_SelectedIndexChanged">
                                            </asp:CheckBoxList>
                                        </asp:Panel>
                                        <asp:PopupControlExtender ID="popExtBatch" runat="server" TargetControlID="txtBatch"
                                            PopupControlID="pnlBatch" Position="Bottom">
                                        </asp:PopupControlExtender>
                                        <%-- <asp:DropDownList ID="ddlBatch" runat="server" CssClass="dropdown" Style="font-family: 'Book Antiqua';"
                                            Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium" Width="60px" AutoPostBack="True"
                                            OnSelectedIndexChanged="ddlBatch_SelectedIndexChanged">
                                        </asp:DropDownList>--%>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </div>
                        </td>
                        <td>
                            <asp:Label ID="lblDegree" runat="server" Text="Degree" Font-Bold="true" Font-Names="Book Antiqua"
                                Font-Size="Medium"></asp:Label>
                        </td>
                        <td>
                            <div style="position: relative;">
                                <asp:UpdatePanel ID="upnlDegree" runat="server">
                                    <ContentTemplate>
                                        <asp:TextBox ID="txtDegree" Width="70px" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                            Font-Size="Medium" CssClass="textbox  txtheight2" ReadOnly="true">--Select--</asp:TextBox>
                                        <asp:Panel ID="pnlDegree" runat="server" CssClass="multxtpanel" Style="width: 150px;
                                            margin: 0px; padding: 0px; height: 220px; overflow: auto;">
                                            <asp:CheckBox ID="chkDegree" Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium"
                                                runat="server" Text="Select All" AutoPostBack="True" Style="width: 100%; height: auto;
                                                margin: 0px; padding: 0px; border: 0px;" OnCheckedChanged="chkDegree_CheckedChanged" />
                                            <asp:CheckBoxList ID="cblDegree" Font-Bold="True" Font-Size="Medium" Font-Names="Book Antiqua"
                                                runat="server" AutoPostBack="True" Style="width: 100%; height: auto; margin: 0px;
                                                padding: 0px; border: 0px;" OnSelectedIndexChanged="cblDegree_SelectedIndexChanged">
                                            </asp:CheckBoxList>
                                        </asp:Panel>
                                        <asp:PopupControlExtender ID="popExtDegree" runat="server" TargetControlID="txtDegree"
                                            PopupControlID="pnlDegree" Position="Bottom">
                                        </asp:PopupControlExtender>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </div>
                        </td>
                        <td>
                            <asp:Label ID="lblBranch" runat="server" Text="Branch" Font-Bold="true" Font-Names="Book Antiqua"
                                Font-Size="Medium"></asp:Label>
                        </td>
                        <td>
                            <div style="position: relative;">
                                <asp:UpdatePanel ID="upnlBranch" runat="server">
                                    <ContentTemplate>
                                        <asp:TextBox ID="txtBranch" Width="80px" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                            Font-Size="Medium" CssClass="textbox  txtheight2" ReadOnly="true">--Select--</asp:TextBox>
                                        <asp:Panel ID="pnlBranch" runat="server" CssClass="multxtpanel" Style="width: 190px;
                                            height: 220px; overflow: auto; margin: 0px; padding: 0px;">
                                            <asp:CheckBox ID="chkBranch" Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium"
                                                runat="server" Text="Select All" AutoPostBack="True" Style="width: 100%; height: auto;
                                                margin: 0px; padding: 0px; border: 0px;" OnCheckedChanged="chkBranch_CheckedChanged" />
                                            <asp:CheckBoxList ID="cblBranch" Font-Bold="True" Font-Size="Medium" Font-Names="Book Antiqua"
                                                runat="server" AutoPostBack="True" Style="width: 100%; height: auto; margin: 0px;
                                                padding: 0px; border: 0px;" OnSelectedIndexChanged="cblBranch_SelectedIndexChanged">
                                            </asp:CheckBoxList>
                                        </asp:Panel>
                                        <asp:PopupControlExtender ID="popExtBranch" runat="server" TargetControlID="txtBranch"
                                            PopupControlID="pnlBranch" Position="Bottom">
                                        </asp:PopupControlExtender>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </div>
                        </td>
                        <td>
                            <asp:Label ID="lblSem" runat="server" Text="Semester" Font-Bold="True" Style="font-family: 'Book Antiqua';"
                                ForeColor="Black" Font-Size="Medium" Font-Names="Book Antiqua"></asp:Label>
                        </td>
                        <td>
                            <asp:UpdatePanel ID="upnlSem" runat="server">
                                <ContentTemplate>
                                    <asp:TextBox ID="txtSem" Width="70px" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                        Font-Size="Medium" CssClass="textbox  txtheight2" ReadOnly="true">--Select--</asp:TextBox>
                                    <asp:Panel ID="pnlSem" runat="server" CssClass="multxtpanel" Style="width: 110px;
                                        height: auto; overflow: auto; margin: 0px; padding: 0px;">
                                        <asp:CheckBox ID="chkSem" Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium"
                                            runat="server" Text="Select All" AutoPostBack="True" Style="width: 100%; height: auto;
                                            margin: 0px; padding: 0px; border: 0px;" OnCheckedChanged="chkSem_CheckedChanged" />
                                        <asp:CheckBoxList ID="cblSem" Font-Bold="True" Font-Size="Medium" Font-Names="Book Antiqua"
                                            runat="server" AutoPostBack="True" Style="width: 100%; height: auto; margin: 0px;
                                            padding: 0px; border: 0px;" OnSelectedIndexChanged="cblSem_SelectedIndexChanged">
                                        </asp:CheckBoxList>
                                    </asp:Panel>
                                    <asp:PopupControlExtender ID="popExtSem" runat="server" TargetControlID="txtSem"
                                        PopupControlID="pnlSem" Position="Bottom">
                                    </asp:PopupControlExtender>
                                    <%--<asp:DropDownList ID="ddlSem" runat="server" CssClass="dropdown" Style="font-family: 'Book Antiqua';"
                                        Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium" Width="50px" AutoPostBack="True"
                                        OnSelectedIndexChanged="ddlSem_SelectedIndexChanged">
                                    </asp:DropDownList>--%>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label ID="lblSec" runat="server" Text="Section" Font-Bold="True" Style="font-family: 'Book Antiqua';"
                                ForeColor="Black" Font-Size="Medium" Font-Names="Book Antiqua"></asp:Label>
                        </td>
                        <td>
                            <div style="position: relative;">
                                <asp:UpdatePanel ID="upnlSec" runat="server">
                                    <ContentTemplate>
                                        <asp:TextBox ID="txtSec" Width="65px" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                            Font-Size="Medium" CssClass="textbox  txtheight2" ReadOnly="true">--Select--</asp:TextBox>
                                        <asp:Panel ID="pnlSec" runat="server" CssClass="multxtpanel" Style="width: 120px;
                                            height: 100px; overflow: auto; margin: 0px; padding: 0px;">
                                            <asp:CheckBox ID="chkSec" Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium"
                                                runat="server" Text="Select All" AutoPostBack="True" Style="width: 100%; height: auto;
                                                margin: 0px; padding: 0px; border: 0px;" OnCheckedChanged="chkSec_CheckedChanged" />
                                            <asp:CheckBoxList ID="cblSec" Font-Bold="True" Font-Size="Medium" Font-Names="Book Antiqua"
                                                runat="server" AutoPostBack="True" Style="width: 100%; height: auto; margin: 0px;
                                                padding: 0px; border: 0px;" OnSelectedIndexChanged="cblSec_SelectedIndexChanged">
                                            </asp:CheckBoxList>
                                        </asp:Panel>
                                        <asp:PopupControlExtender ID="popExtSec" runat="server" TargetControlID="txtSec"
                                            PopupControlID="pnlSec" Position="Bottom">
                                        </asp:PopupControlExtender>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </div>
                        </td>
                        <td>
                            <asp:Label ID="lblStaff" runat="server" Text="Staff" Font-Bold="True" Style="font-family: 'Book Antiqua';"
                                ForeColor="Black" Font-Size="Medium" Font-Names="Book Antiqua"></asp:Label>
                        </td>
                        <td>
                            <div style="position: relative;">
                                <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                                    <ContentTemplate>
                                        <asp:DropDownList ID="ddlStaff" runat="server" CssClass="dropdown" Style="font-family: 'Book Antiqua';"
                                            Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium" Width="100px" AutoPostBack="True"
                                            OnSelectedIndexChanged="ddlStaff_SelectedIndexChanged">
                                        </asp:DropDownList>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </div>
                        </td>
                        <td>
                            <asp:Label ID="lblSubjectAD" runat="server" Text="Subject" Font-Bold="True" Style="font-family: 'Book Antiqua';"
                                ForeColor="Black" Font-Size="Medium" Font-Names="Book Antiqua"></asp:Label>
                        </td>
                        <td>
                            <div style="position: relative;">
                                <asp:UpdatePanel ID="upnlSubject" runat="server">
                                    <ContentTemplate>
                                        <asp:TextBox ID="txtSubject" Width="120px" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                            Font-Size="Medium" CssClass="textbox  txtheight2" ReadOnly="true">--Select--</asp:TextBox>
                                        <asp:Panel ID="pnlSubject" runat="server" CssClass="multxtpanel" Style="width: 270px;
                                            height: 250px; overflow: auto; margin: 0px; padding: 0px;">
                                            <asp:CheckBox ID="chkSubject" Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium"
                                                runat="server" Text="Select All" AutoPostBack="True" Style="width: 100%; height: auto;
                                                margin: 0px; padding: 0px; border: 0px;" OnCheckedChanged="chkSubject_CheckedChanged" />
                                            <asp:CheckBoxList ID="cblSubject" Font-Bold="True" Font-Size="Medium" Font-Names="Book Antiqua"
                                                runat="server" AutoPostBack="True" Style="width: 100%; height: auto; margin: 0px;
                                                padding: 0px; border: 0px;" OnSelectedIndexChanged="cblSubject_SelectedIndexChanged">
                                            </asp:CheckBoxList>
                                        </asp:Panel>
                                        <asp:PopupControlExtender ID="popExtSubject" runat="server" TargetControlID="txtSubject"
                                            PopupControlID="pnlSubject" Position="Bottom">
                                        </asp:PopupControlExtender>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </div>
                        </td>
                        <td>
                            <asp:Button ID="btnAdminGo" CssClass="textbox textbox1" runat="server" Font-Bold="True"
                                Font-Size="Medium" Font-Names="Book Antiqua" Style="width: auto; height: auto;"
                                Text="Go" OnClick="btnAdminGo_Click" />
                        </td>
                    </tr>
                </table>
            </div>
            <asp:Label ID="lblErrSearch" runat="server" Text="" ForeColor="Red" Visible="False"
                Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium" Style="margin: 0px;
                margin-bottom: 15px; margin-top: 10px;"></asp:Label>
            <center>
                <div id="divMainContents" runat="server" visible="false" style="margin: 0px; margin-bottom: 5px;
                    margin-top: 10px;">
                    <center>
                    <asp:Label ID="lblStaffNameDisp" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                            Font-Size="Medium" ForeColor="Red" Text="" Visible="false"></asp:Label>
                        <div id="divPrintContent" runat="server" style="margin: 0px; margin-top: 20px;">
                            <table>
                                <tr>
                                    <td colspan="4">
                                        <asp:Label ID="lblExcelError" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                            Font-Size="Medium" ForeColor="Red" Text="Please Enter Your Report Name" Visible="false"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="lblReportName" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                            Font-Size="Medium" Text="Report Name"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtExcelFileName" runat="server" CssClass="textbox textbox1" Height="20px"
                                            Width="180px" Style="font-family: 'Book Antiqua'" Font-Bold="True" Font-Names="Book Antiqua"
                                            onkeypress="display1()" Font-Size="Medium"></asp:TextBox>
                                        <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender9" runat="server" TargetControlID="txtExcelFileName"
                                            FilterType="LowercaseLetters,UppercaseLetters,Numbers,Custom" ValidChars="(),.[]_"
                                            InvalidChars="/\">
                                        </asp:FilteredTextBoxExtender>
                                    </td>
                                    <td>
                                        <asp:Button ID="btnExportExcel" runat="server" Font-Bold="true" Font-Names="Book Antiqua"
                                            OnClick="btnExportExcel_Click" Font-Size="Medium" Style="width: auto; height: auto;"
                                            Text="Export To Excel" CssClass="textbox textbox1" />
                                    </td>
                                    <td>
                                        <asp:Button ID="btnPrintPDF" runat="server" Text="Print" OnClick="btnPrintPDF_Click"
                                            Font-Names="Book Antiqua" Font-Size="Medium" Font-Bold="true" Style="width: auto;
                                            height: auto;" CssClass="textbox textbox1" />
                                        <Insproplus:printmaster runat="server" ID="printMaster1" Visible="false" />
                                    </td>
                                </tr>
                            </table>
                        </div>
                      
                        <FarPoint:FpSpread ID="FpStudentList" autopostback="false" runat="server" Visible="true"
                            BorderStyle="Solid" BorderWidth="0px" CssClass="spreadborder" ShowHeaderSelection="false"
                            Style="width: 100%; height: auto; margin: 0px; margin-bottom: 15px; margin-top: 10px;
                            padding: 0px;">
                            <Sheets>
                                <FarPoint:SheetView DesignString="&lt;?xml version=&quot;1.0&quot; encoding=&quot;utf-8&quot;?&gt;&lt;Sheet&gt;&lt;Data&gt;&lt;RowHeader class=&quot;FarPoint.Web.Spread.Model.DefaultSheetDataModel&quot; rows=&quot;3&quot; columns=&quot;1&quot;&gt;&lt;AutoCalculation&gt;True&lt;/AutoCalculation&gt;&lt;AutoGenerateColumns&gt;True&lt;/AutoGenerateColumns&gt;&lt;ReferenceStyle&gt;A1&lt;/ReferenceStyle&gt;&lt;Iteration&gt;False&lt;/Iteration&gt;&lt;MaximumIterations&gt;1&lt;/MaximumIterations&gt;&lt;MaximumChange&gt;0.001&lt;/MaximumChange&gt;&lt;/RowHeader&gt;&lt;ColumnHeader class=&quot;FarPoint.Web.Spread.Model.DefaultSheetDataModel&quot; rows=&quot;1&quot; columns=&quot;4&quot;&gt;&lt;AutoCalculation&gt;True&lt;/AutoCalculation&gt;&lt;AutoGenerateColumns&gt;True&lt;/AutoGenerateColumns&gt;&lt;ReferenceStyle&gt;A1&lt;/ReferenceStyle&gt;&lt;Iteration&gt;False&lt;/Iteration&gt;&lt;MaximumIterations&gt;1&lt;/MaximumIterations&gt;&lt;MaximumChange&gt;0.001&lt;/MaximumChange&gt;&lt;/ColumnHeader&gt;&lt;DataArea class=&quot;FarPoint.Web.Spread.Model.DefaultSheetDataModel&quot; rows=&quot;3&quot; columns=&quot;4&quot;&gt;&lt;AutoCalculation&gt;True&lt;/AutoCalculation&gt;&lt;AutoGenerateColumns&gt;True&lt;/AutoGenerateColumns&gt;&lt;ReferenceStyle&gt;A1&lt;/ReferenceStyle&gt;&lt;Iteration&gt;False&lt;/Iteration&gt;&lt;MaximumIterations&gt;1&lt;/MaximumIterations&gt;&lt;MaximumChange&gt;0.001&lt;/MaximumChange&gt;&lt;SheetName&gt;Sheet1&lt;/SheetName&gt;&lt;/DataArea&gt;&lt;SheetCorner class=&quot;FarPoint.Web.Spread.Model.DefaultSheetDataModel&quot; rows=&quot;1&quot; columns=&quot;1&quot;&gt;&lt;AutoCalculation&gt;True&lt;/AutoCalculation&gt;&lt;AutoGenerateColumns&gt;True&lt;/AutoGenerateColumns&gt;&lt;ReferenceStyle&gt;A1&lt;/ReferenceStyle&gt;&lt;Iteration&gt;False&lt;/Iteration&gt;&lt;MaximumIterations&gt;1&lt;/MaximumIterations&gt;&lt;MaximumChange&gt;0.001&lt;/MaximumChange&gt;&lt;/SheetCorner&gt;&lt;ColumnFooter class=&quot;FarPoint.Web.Spread.Model.DefaultSheetDataModel&quot; rows=&quot;1&quot; columns=&quot;4&quot;&gt;&lt;AutoCalculation&gt;True&lt;/AutoCalculation&gt;&lt;AutoGenerateColumns&gt;True&lt;/AutoGenerateColumns&gt;&lt;ReferenceStyle&gt;A1&lt;/ReferenceStyle&gt;&lt;Iteration&gt;False&lt;/Iteration&gt;&lt;MaximumIterations&gt;1&lt;/MaximumIterations&gt;&lt;MaximumChange&gt;0.001&lt;/MaximumChange&gt;&lt;/ColumnFooter&gt;&lt;/Data&gt;&lt;Presentation&gt;&lt;ActiveSkin class=&quot;FarPoint.Web.Spread.SheetSkin&quot;&gt;&lt;Name&gt;Default&lt;/Name&gt;&lt;BackColor&gt;Empty&lt;/BackColor&gt;&lt;CellBackColor&gt;Empty&lt;/CellBackColor&gt;&lt;CellForeColor&gt;Empty&lt;/CellForeColor&gt;&lt;CellSpacing&gt;0&lt;/CellSpacing&gt;&lt;GridLines&gt;Both&lt;/GridLines&gt;&lt;GridLineColor&gt;#d0d7e5&lt;/GridLineColor&gt;&lt;HeaderBackColor&gt;Empty&lt;/HeaderBackColor&gt;&lt;HeaderForeColor&gt;Empty&lt;/HeaderForeColor&gt;&lt;FlatColumnHeader&gt;False&lt;/FlatColumnHeader&gt;&lt;FooterBackColor&gt;Empty&lt;/FooterBackColor&gt;&lt;FooterForeColor&gt;Empty&lt;/FooterForeColor&gt;&lt;FlatColumnFooter&gt;False&lt;/FlatColumnFooter&gt;&lt;FlatRowHeader&gt;False&lt;/FlatRowHeader&gt;&lt;HeaderFontBold&gt;False&lt;/HeaderFontBold&gt;&lt;FooterFontBold&gt;False&lt;/FooterFontBold&gt;&lt;SelectionBackColor&gt;#eaecf5&lt;/SelectionBackColor&gt;&lt;SelectionForeColor&gt;Empty&lt;/SelectionForeColor&gt;&lt;EvenRowBackColor&gt;Empty&lt;/EvenRowBackColor&gt;&lt;OddRowBackColor&gt;Empty&lt;/OddRowBackColor&gt;&lt;ShowColumnHeader&gt;True&lt;/ShowColumnHeader&gt;&lt;ShowColumnFooter&gt;False&lt;/ShowColumnFooter&gt;&lt;ShowRowHeader&gt;True&lt;/ShowRowHeader&gt;&lt;ColumnHeaderBackground class=&quot;FarPoint.Web.Spread.Background&quot;&gt;&lt;BackgroundImageUrl&gt;SPREADCLIENTPATH:/img/chbg.gif&lt;/BackgroundImageUrl&gt;&lt;SelectedBackgroundImageUrl&gt;SPREADCLIENTPATH:/img/chm.png&lt;/SelectedBackgroundImageUrl&gt;&lt;/ColumnHeaderBackground&gt;&lt;SheetCornerBackground class=&quot;FarPoint.Web.Spread.Background&quot;&gt;&lt;BackgroundImageUrl&gt;SPREADCLIENTPATH:/img/chbg.gif&lt;/BackgroundImageUrl&gt;&lt;SelectedBackgroundImageUrl&gt;SPREADCLIENTPATH:/img/chm.png&lt;/SelectedBackgroundImageUrl&gt;&lt;/SheetCornerBackground&gt;&lt;HeaderGrayAreaColor&gt;#7999c2&lt;/HeaderGrayAreaColor&gt;&lt;/ActiveSkin&gt;&lt;AxisModels&gt;&lt;Column class=&quot;FarPoint.Web.Spread.Model.DefaultSheetAxisModel&quot; orientation=&quot;Horizontal&quot; count=&quot;4&quot;&gt;&lt;Items&gt;&lt;Item index=&quot;-1&quot;&gt;&lt;SortIndicator&gt;Ascending&lt;/SortIndicator&gt;&lt;/Item&gt;&lt;Item index=&quot;0&quot;&gt;&lt;Size&gt;55&lt;/Size&gt;&lt;/Item&gt;&lt;Item index=&quot;1&quot;&gt;&lt;Size&gt;111&lt;/Size&gt;&lt;/Item&gt;&lt;Item index=&quot;2&quot;&gt;&lt;Size&gt;81&lt;/Size&gt;&lt;/Item&gt;&lt;Item index=&quot;3&quot;&gt;&lt;Size&gt;79&lt;/Size&gt;&lt;/Item&gt;&lt;/Items&gt;&lt;/Column&gt;&lt;RowHeaderColumn class=&quot;FarPoint.Web.Spread.Model.DefaultSheetAxisModel&quot; defaultSize=&quot;40&quot; orientation=&quot;Horizontal&quot; count=&quot;1&quot;&gt;&lt;Items&gt;&lt;Item index=&quot;-1&quot;&gt;&lt;SortIndicator&gt;Ascending&lt;/SortIndicator&gt;&lt;Size&gt;40&lt;/Size&gt;&lt;/Item&gt;&lt;/Items&gt;&lt;/RowHeaderColumn&gt;&lt;ColumnHeaderRow class=&quot;FarPoint.Web.Spread.Model.DefaultSheetAxisModel&quot; defaultSize=&quot;22&quot; orientation=&quot;Vertical&quot; count=&quot;1&quot;&gt;&lt;Items&gt;&lt;Item index=&quot;-1&quot;&gt;&lt;Size&gt;22&lt;/Size&gt;&lt;/Item&gt;&lt;/Items&gt;&lt;/ColumnHeaderRow&gt;&lt;ColumnFooterRow class=&quot;FarPoint.Web.Spread.Model.DefaultSheetAxisModel&quot; defaultSize=&quot;22&quot; orientation=&quot;Vertical&quot; count=&quot;1&quot;&gt;&lt;Items&gt;&lt;Item index=&quot;-1&quot;&gt;&lt;Size&gt;22&lt;/Size&gt;&lt;/Item&gt;&lt;/Items&gt;&lt;/ColumnFooterRow&gt;&lt;/AxisModels&gt;&lt;StyleModels&gt;&lt;RowHeader class=&quot;FarPoint.Web.Spread.Model.DefaultSheetStyleModel&quot; Rows=&quot;3&quot; Columns=&quot;1&quot;&gt;&lt;AltRowCount&gt;2&lt;/AltRowCount&gt;&lt;DefaultStyle class=&quot;FarPoint.Web.Spread.NamedStyle&quot; Parent=&quot;RowHeaderDefault&quot; /&gt;&lt;ConditionalFormatCollections /&gt;&lt;/RowHeader&gt;&lt;ColumnHeader class=&quot;FarPoint.Web.Spread.Model.DefaultSheetStyleModel&quot; Rows=&quot;1&quot; Columns=&quot;4&quot;&gt;&lt;AltRowCount&gt;2&lt;/AltRowCount&gt;&lt;DefaultStyle class=&quot;FarPoint.Web.Spread.NamedStyle&quot; Parent=&quot;ColumnHeaderDefault&quot;&gt;&lt;Background class=&quot;FarPoint.Web.Spread.Background&quot;&gt;&lt;BackgroundImageUrl&gt;SPREADCLIENTPATH:/img/chbg.gif&lt;/BackgroundImageUrl&gt;&lt;SelectedBackgroundImageUrl&gt;SPREADCLIENTPATH:/img/chm.png&lt;/SelectedBackgroundImageUrl&gt;&lt;/Background&gt;&lt;/DefaultStyle&gt;&lt;ConditionalFormatCollections /&gt;&lt;/ColumnHeader&gt;&lt;DataArea class=&quot;FarPoint.Web.Spread.Model.DefaultSheetStyleModel&quot; Rows=&quot;3&quot; Columns=&quot;4&quot;&gt;&lt;AltRowCount&gt;2&lt;/AltRowCount&gt;&lt;DefaultStyle class=&quot;FarPoint.Web.Spread.NamedStyle&quot; Parent=&quot;DataAreaDefault&quot;&gt;&lt;Font&gt;&lt;Name&gt;Book Antiqua&lt;/Name&gt;&lt;Names&gt;&lt;Name&gt;Book Antiqua&lt;/Name&gt;&lt;/Names&gt;&lt;Size&gt;Medium&lt;/Size&gt;&lt;Bold&gt;False&lt;/Bold&gt;&lt;Italic&gt;False&lt;/Italic&gt;&lt;Overline&gt;False&lt;/Overline&gt;&lt;Strikeout&gt;False&lt;/Strikeout&gt;&lt;Underline&gt;False&lt;/Underline&gt;&lt;/Font&gt;&lt;GdiCharSet&gt;254&lt;/GdiCharSet&gt;&lt;ForeColor&gt;#0033cc&lt;/ForeColor&gt;&lt;HorizontalAlign&gt;Center&lt;/HorizontalAlign&gt;&lt;/DefaultStyle&gt;&lt;ConditionalFormatCollections /&gt;&lt;/DataArea&gt;&lt;SheetCorner class=&quot;FarPoint.Web.Spread.Model.DefaultSheetStyleModel&quot; Rows=&quot;1&quot; Columns=&quot;1&quot;&gt;&lt;AltRowCount&gt;2&lt;/AltRowCount&gt;&lt;DefaultStyle class=&quot;FarPoint.Web.Spread.NamedStyle&quot; Parent=&quot;CornerDefault&quot;&gt;&lt;Border class=&quot;FarPoint.Web.Spread.Border&quot; Size=&quot;1&quot; Style=&quot;Solid&quot;&gt;&lt;Bottom Color=&quot;ControlDark&quot; /&gt;&lt;Left Size=&quot;0&quot; /&gt;&lt;Right Color=&quot;ControlDark&quot; /&gt;&lt;Top Size=&quot;0&quot; /&gt;&lt;/Border&gt;&lt;Background class=&quot;FarPoint.Web.Spread.Background&quot;&gt;&lt;BackgroundImageUrl&gt;SPREADCLIENTPATH:/img/chbg.gif&lt;/BackgroundImageUrl&gt;&lt;SelectedBackgroundImageUrl&gt;SPREADCLIENTPATH:/img/chm.png&lt;/SelectedBackgroundImageUrl&gt;&lt;/Background&gt;&lt;/DefaultStyle&gt;&lt;ConditionalFormatCollections /&gt;&lt;/SheetCorner&gt;&lt;ColumnFooter class=&quot;FarPoint.Web.Spread.Model.DefaultSheetStyleModel&quot; Rows=&quot;1&quot; Columns=&quot;4&quot;&gt;&lt;AltRowCount&gt;2&lt;/AltRowCount&gt;&lt;DefaultStyle class=&quot;FarPoint.Web.Spread.NamedStyle&quot; Parent=&quot;ColumnFooterDefault&quot; /&gt;&lt;ConditionalFormatCollections /&gt;&lt;/ColumnFooter&gt;&lt;/StyleModels&gt;&lt;MessageRowStyle class=&quot;FarPoint.Web.Spread.Appearance&quot;&gt;&lt;BackColor&gt;LightYellow&lt;/BackColor&gt;&lt;ForeColor&gt;Red&lt;/ForeColor&gt;&lt;/MessageRowStyle&gt;&lt;SheetCornerStyle class=&quot;FarPoint.Web.Spread.NamedStyle&quot; Parent=&quot;CornerDefault&quot;&gt;&lt;Border class=&quot;FarPoint.Web.Spread.Border&quot; Size=&quot;1&quot; Style=&quot;Solid&quot;&gt;&lt;Bottom Color=&quot;ControlDark&quot; /&gt;&lt;Left Size=&quot;0&quot; /&gt;&lt;Right Color=&quot;ControlDark&quot; /&gt;&lt;Top Size=&quot;0&quot; /&gt;&lt;/Border&gt;&lt;Background class=&quot;FarPoint.Web.Spread.Background&quot;&gt;&lt;BackgroundImageUrl&gt;SPREADCLIENTPATH:/img/chbg.gif&lt;/BackgroundImageUrl&gt;&lt;SelectedBackgroundImageUrl&gt;SPREADCLIENTPATH:/img/chm.png&lt;/SelectedBackgroundImageUrl&gt;&lt;/Background&gt;&lt;/SheetCornerStyle&gt;&lt;AllowLoadOnDemand&gt;false&lt;/AllowLoadOnDemand&gt;&lt;LoadRowIncrement &gt;10&lt;/LoadRowIncrement &gt;&lt;LoadInitRowCount &gt;30&lt;/LoadInitRowCount &gt;&lt;AllowVirtualScrollPaging&gt;false&lt;/AllowVirtualScrollPaging&gt;&lt;TopRow&gt;0&lt;/TopRow&gt;&lt;PreviewRowStyle class=&quot;FarPoint.Web.Spread.PreviewRowInfo&quot; /&gt;&lt;/Presentation&gt;&lt;Settings&gt;&lt;Name&gt;Sheet1&lt;/Name&gt;&lt;Categories&gt;&lt;Appearance&gt;&lt;GridLineColor&gt;#d0d7e5&lt;/GridLineColor&gt;&lt;SelectionBackColor&gt;#eaecf5&lt;/SelectionBackColor&gt;&lt;SelectionBorder class=&quot;FarPoint.Web.Spread.Border&quot; /&gt;&lt;/Appearance&gt;&lt;Behavior&gt;&lt;EditTemplateColumnCount&gt;2&lt;/EditTemplateColumnCount&gt;&lt;ScrollingContentVisible&gt;True&lt;/ScrollingContentVisible&gt;&lt;PageSize&gt;100&lt;/PageSize&gt;&lt;AllowPage&gt;False&lt;/AllowPage&gt;&lt;GroupBarText&gt;Drag a column to group by that column.&lt;/GroupBarText&gt;&lt;/Behavior&gt;&lt;Layout&gt;&lt;RowHeaderColumnCount&gt;1&lt;/RowHeaderColumnCount&gt;&lt;ColumnHeaderRowCount&gt;1&lt;/ColumnHeaderRowCount&gt;&lt;/Layout&gt;&lt;/Categories&gt;&lt;ActiveRow&gt;0&lt;/ActiveRow&gt;&lt;ActiveColumn&gt;0&lt;/ActiveColumn&gt;&lt;ColumnHeaderRowCount&gt;1&lt;/ColumnHeaderRowCount&gt;&lt;ColumnFooterRowCount&gt;1&lt;/ColumnFooterRowCount&gt;&lt;PrintInfo&gt;&lt;Header /&gt;&lt;Footer /&gt;&lt;ZoomFactor&gt;0&lt;/ZoomFactor&gt;&lt;FirstPageNumber&gt;1&lt;/FirstPageNumber&gt;&lt;Orientation&gt;Auto&lt;/Orientation&gt;&lt;PrintType&gt;All&lt;/PrintType&gt;&lt;PageOrder&gt;Auto&lt;/PageOrder&gt;&lt;BestFitCols&gt;False&lt;/BestFitCols&gt;&lt;BestFitRows&gt;False&lt;/BestFitRows&gt;&lt;PageStart&gt;-1&lt;/PageStart&gt;&lt;PageEnd&gt;-1&lt;/PageEnd&gt;&lt;ColStart&gt;-1&lt;/ColStart&gt;&lt;ColEnd&gt;-1&lt;/ColEnd&gt;&lt;RowStart&gt;-1&lt;/RowStart&gt;&lt;RowEnd&gt;-1&lt;/RowEnd&gt;&lt;ShowBorder&gt;True&lt;/ShowBorder&gt;&lt;ShowGrid&gt;True&lt;/ShowGrid&gt;&lt;ShowColor&gt;True&lt;/ShowColor&gt;&lt;ShowColumnHeader&gt;Inherit&lt;/ShowColumnHeader&gt;&lt;ShowRowHeader&gt;Inherit&lt;/ShowRowHeader&gt;&lt;ShowColumnFooter&gt;Inherit&lt;/ShowColumnFooter&gt;&lt;ShowColumnFooterEachPage&gt;True&lt;/ShowColumnFooterEachPage&gt;&lt;ShowTitle&gt;True&lt;/ShowTitle&gt;&lt;ShowSubtitle&gt;True&lt;/ShowSubtitle&gt;&lt;UseMax&gt;True&lt;/UseMax&gt;&lt;UseSmartPrint&gt;False&lt;/UseSmartPrint&gt;&lt;Opacity&gt;255&lt;/Opacity&gt;&lt;PrintNotes&gt;None&lt;/PrintNotes&gt;&lt;Centering&gt;None&lt;/Centering&gt;&lt;RepeatColStart&gt;-1&lt;/RepeatColStart&gt;&lt;RepeatColEnd&gt;-1&lt;/RepeatColEnd&gt;&lt;RepeatRowStart&gt;-1&lt;/RepeatRowStart&gt;&lt;RepeatRowEnd&gt;-1&lt;/RepeatRowEnd&gt;&lt;SmartPrintPagesTall&gt;1&lt;/SmartPrintPagesTall&gt;&lt;SmartPrintPagesWide&gt;1&lt;/SmartPrintPagesWide&gt;&lt;HeaderHeight&gt;-1&lt;/HeaderHeight&gt;&lt;FooterHeight&gt;-1&lt;/FooterHeight&gt;&lt;/PrintInfo&gt;&lt;TitleInfo class=&quot;FarPoint.Web.Spread.TitleInfo&quot;&gt;&lt;Style class=&quot;FarPoint.Web.Spread.StyleInfo&quot;&gt;&lt;BackColor&gt;#e7eff7&lt;/BackColor&gt;&lt;HorizontalAlign&gt;Right&lt;/HorizontalAlign&gt;&lt;/Style&gt;&lt;Value type=&quot;System.String&quot; whitespace=&quot;&quot; /&gt;&lt;/TitleInfo&gt;&lt;LayoutTemplate class=&quot;FarPoint.Web.Spread.LayoutTemplate&quot;&gt;&lt;Layout&gt;&lt;ColumnCount&gt;4&lt;/ColumnCount&gt;&lt;RowCount&gt;1&lt;/RowCount&gt;&lt;/Layout&gt;&lt;Data&gt;&lt;LayoutData class=&quot;FarPoint.Web.Spread.Model.DefaultSheetDataModel&quot; rows=&quot;1&quot; columns=&quot;4&quot;&gt;&lt;AutoCalculation&gt;True&lt;/AutoCalculation&gt;&lt;AutoGenerateColumns&gt;True&lt;/AutoGenerateColumns&gt;&lt;ReferenceStyle&gt;A1&lt;/ReferenceStyle&gt;&lt;Iteration&gt;False&lt;/Iteration&gt;&lt;MaximumIterations&gt;1&lt;/MaximumIterations&gt;&lt;MaximumChange&gt;0.001&lt;/MaximumChange&gt;&lt;Cells&gt;&lt;Cell row=&quot;0&quot; column=&quot;0&quot;&gt;&lt;Data type=&quot;System.Int32&quot;&gt;0&lt;/Data&gt;&lt;/Cell&gt;&lt;Cell row=&quot;0&quot; column=&quot;1&quot;&gt;&lt;Data type=&quot;System.Int32&quot;&gt;1&lt;/Data&gt;&lt;/Cell&gt;&lt;Cell row=&quot;0&quot; column=&quot;2&quot;&gt;&lt;Data type=&quot;System.Int32&quot;&gt;2&lt;/Data&gt;&lt;/Cell&gt;&lt;Cell row=&quot;0&quot; column=&quot;3&quot;&gt;&lt;Data type=&quot;System.Int32&quot;&gt;3&lt;/Data&gt;&lt;/Cell&gt;&lt;/Cells&gt;&lt;/LayoutData&gt;&lt;/Data&gt;&lt;AxisModels&gt;&lt;LayoutColumn class=&quot;FarPoint.Web.Spread.Model.DefaultSheetAxisModel&quot; orientation=&quot;Horizontal&quot; count=&quot;4&quot;&gt;&lt;Items&gt;&lt;Item index=&quot;-1&quot;&gt;&lt;SortIndicator&gt;Ascending&lt;/SortIndicator&gt;&lt;/Item&gt;&lt;/Items&gt;&lt;/LayoutColumn&gt;&lt;LayoutRow class=&quot;FarPoint.Web.Spread.Model.DefaultSheetAxisModel&quot; orientation=&quot;Vertical&quot; count=&quot;1&quot;&gt;&lt;Items&gt;&lt;Item index=&quot;-1&quot; /&gt;&lt;/Items&gt;&lt;/LayoutRow&gt;&lt;/AxisModels&gt;&lt;/LayoutTemplate&gt;&lt;LayoutMode&gt;CellLayoutMode&lt;/LayoutMode&gt;&lt;CurrentPageIndex type=&quot;System.Int32&quot;&gt;0&lt;/CurrentPageIndex&gt;&lt;/Settings&gt;&lt;/Sheet&gt;"
                                    AllowPage="False" PageSize="100" SheetName="Sheet1">
                                </FarPoint:SheetView>
                            </Sheets>
                        </FarPoint:FpSpread>
                    </center>
                </div>
            </center>
            <center>
                <div id="divPopAlert" runat="server" visible="false" style="height: 400em; z-index: 2000;
                    width: 100%; background-color: rgba(54, 25, 25, .2); position: absolute; top: 0%;
                    left: 0%;">
                    <center>
                        <div id="divPopAlertContent" runat="server" class="table" style="background-color: White;
                            height: 120px; width: 238px; border: 5px solid #0CA6CA; border-top: 25px solid #0CA6CA;
                            margin-top: 200px; border-radius: 10px;">
                            <center>
                                <table style="height: 100px; width: 100%">
                                    <tr>
                                        <td align="center">
                                            <asp:Label ID="lblAlertMsg" runat="server" Style="color: Red;" Font-Bold="true" Font-Size="Medium"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <center>
                                                <asp:Button ID="btnPopAlertClose" Font-Bold="true" Font-Size="Medium" Font-Names="Book Antiqua"
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
        </div>
    </center>
</asp:Content>

<%@ Page Title="" Language="C#" MasterPageFile="~/AttendanceMOD/AttendanceSubSiteMaster.master"
    AutoEventWireup="true" CodeFile="AllStudentAttendance1.aspx.cs" Inherits="AllStudentAttendance1"
    EnableEventValidation="false" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Assembly="FarPoint.Web.Spread" Namespace="FarPoint.Web.Spread" TagPrefix="FarPoint" %>
<%@ Register Src="~/Usercontrols/PrintMaster.ascx" TagName="printmaster" TagPrefix="Insproplus" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <script src="//ajax.googleapis.com/ajax/libs/jquery/1.11.1/jquery.min.js"></script>
    <link href="~/Styles/css/Commoncss.css" rel="Stylesheet" type="text/css" />
    <style type="text/css">
        .style1
        {
            width: 80px;
        }
        .style2
        {
            width: 120px;
        }
        .dropdown
        {
            font: 12px/0.8 Arial;
            border: solid 1px #6FA602;
            border-radius: 4px;
            -moz-border-radius: 4px;
            -webkit-border-radius: 4px;
            cursor: pointer;
            width: auto;
        }
        .modal_popup_background_color
        {
            background-color: #666699;
            filter: alpha(opacity=100);
            opacity: 0.7;
        }
    </style>
    <script type="text/javascript">
        function get(txt1, lbl1batch, lbl1degree, lbl1semester, lbl1section, lblentry) {
            var batch = lbl1batch;
            var degree = lbl1degree;
            var sem = lbl1semester;
            var sect = lbl1section;
            var sst = document.getElementById(txt1).value;
            var enrty = lblentry;
            $.ajax({
                type: "POST",
                url: "AllStudentAttendance1.aspx/CheckUserName",
                data: '{rollno: "' + sst + '",batch:"' + batch + '",degree:"' + degree + '",sem:"' + sem + '",sec:"' + sect + '",entryby:"' + enrty + '"}',
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
                    mesg.style.color = "red";
                    mesg.innerHTML = "Admission No Not Exist";
                    break;
                case "1":
                    mesg.style.color = "green";
                    mesg.innerHTML = "Available";
                    break;
                case "2":
                    mesg.style.color = "red";
                    mesg.innerHTML = "Please Enter Admission No";
                    break;
                case "error":
                    mesg.style.color = "red";
                    mesg.innerHTML = "Error occurred";
                    break;
            }
        }

        function restrictspecial(e, key, obj) {
            var keynum;
            if (window.event) // IE
            {
                keynum = e.keyCode;
            }
            else if (e.which) // Netscape/Firefox/Opera
            {
                keynum = e.which;
            }
            switch (key) {

                case "STRING":
                    var keyboardchars = /[\x00\x08]/;
                    var validchars = new RegExp("[ A-Za-z]");
                    break;

                case "NUMERIC1TO9":
                    var keyboardchars = /[\x00\x08]/;
                    var validchars = new RegExp("[1-9]");
                    break;

                case "EMAIL":
                    var keyboardchars = /[\x00\x08]/;
                    var validchars = new RegExp("[A-Za-z0-9,]");
                    break;
            }
            var keychar = String.fromCharCode(keynum);
            if (!validchars.test(keychar) && !keyboardchars.test(keychar)) {
                return false
            } else {
                return keychar.toUpperCase();
            }
        }

        function setHeight(txtdesc) {
            txtdesc.style.height = txtdesc.scrollHeight + "px";
        }

    </script>
    <script type="text/javascript" language="javascript">
        function CheckAllEmp(Checkbox) {
            var GridVwHeaderChckbox = document.getElementById("<%=gvdatass.ClientID %>");
            for (i = 1; i < GridVwHeaderChckbox.rows.length; i++) {
                GridVwHeaderChckbox.rows[i].cells[4].getElementsByTagName("INPUT")[0].checked = Checkbox.checked;
            }
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <center>
        <span class="fontstyleheader" style="color: Green; margin: 0px; margin-bottom: 10px;
            margin-top: 10px; position: relative;">All Student Attendance</span></center>
    <center>
        <table class="maintablestyle" runat="server" style="margin: 0px; margin-bottom: 10px;
            margin-top: 10px; position: relative;">
            <tr>
                <td>
                    <asp:Label ID="Label1" runat="server" Text="College" Font-Bold="True" Style="font-family: 'Book Antiqua';"
                        ForeColor="Black" Font-Size="Medium" Font-Names="Book Antiqua"></asp:Label>
                </td>
                <td>
                    <asp:DropDownList ID="ddlcollege" runat="server" CssClass="textbox1 ddlheight1" Font-Bold="true"
                        Font-Names="Book Antiqua" Font-Size="Medium" AutoPostBack="true" OnSelectedIndexChanged="ddlcollege_OnSelectedIndexChanged">
                    </asp:DropDownList>
                </td>
                <td>
                    <asp:Label ID="lblbach" runat="server" Text="Batch" Style="font-family: 'Book Antiqua';"
                        Font-Bold="True" ForeColor="Black" Font-Size="Medium" Font-Names="Book Antiqua"></asp:Label>
                </td>
                <td>
                    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                        <ContentTemplate>
                            <asp:TextBox ID="txtbatch" runat="server" Height="20px" CssClass="textbox textbox1"
                                ReadOnly="true" Width="120px" Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium">---Select---</asp:TextBox>
                            <asp:Panel ID="pbatch" runat="server" CssClass="multxtpanel" BackColor="White" BorderColor="Black"
                                BorderStyle="Solid" BorderWidth="2px" Style="font-family: 'Book Antiqua'">
                                <asp:CheckBox ID="chkbatch" runat="server" Width="100px" Font-Bold="True" OnCheckedChanged="chkbatch_ChekedChange"
                                    Font-Names="Book Antiqua" Font-Size="Medium" Text="Select All" AutoPostBack="True" />
                                <asp:CheckBoxList ID="chklsbatch" runat="server" Font-Size="Medium" AutoPostBack="True"
                                    Font-Bold="True" Font-Names="Book Antiqua" OnSelectedIndexChanged="chklstbatch_SelectedIndexChanged">
                                </asp:CheckBoxList>
                            </asp:Panel>
                            <asp:PopupControlExtender ID="pceSelections" runat="server" TargetControlID="txtbatch"
                                PopupControlID="pbatch" Position="Bottom">
                            </asp:PopupControlExtender>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </td>
                <td>
                    <asp:Label ID="lbldegree" runat="server" Text="Degree" Font-Bold="True" ForeColor="Black"
                        Font-Names="Book Antiqua" Font-Size="Medium"></asp:Label>
                </td>
                <td>
                    <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                        <ContentTemplate>
                            <asp:TextBox ID="txtdegree" runat="server" Height="20px" ReadOnly="true" CssClass="textbox textbox1"
                                Width="120px" Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium">---Select---</asp:TextBox>
                            <asp:Panel ID="pdegree" runat="server" BackColor="White" CssClass="multxtpanel" Height="250px"
                                Width="200px" BorderColor="Black" BorderStyle="Solid" BorderWidth="2px">
                                <asp:CheckBox ID="chkdegree" runat="server" Width="100px" Font-Bold="True" Font-Names="Book Antiqua"
                                    Font-Size="Medium" Text="Select All" AutoPostBack="True" OnCheckedChanged="chkdegree_CheckedChanged" />
                                <asp:CheckBoxList ID="chklstdegree" runat="server" Font-Size="Medium" AutoPostBack="True"
                                    Font-Bold="True" Font-Names="Book Antiqua" OnSelectedIndexChanged="chklstdegree_SelectedIndexChanged">
                                </asp:CheckBoxList>
                            </asp:Panel>
                            <asp:PopupControlExtender ID="PopupControlExtender1" runat="server" TargetControlID="txtdegree"
                                PopupControlID="pdegree" Position="Bottom">
                            </asp:PopupControlExtender>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </td>
                <td>
                    <asp:Label ID="lblbranch" runat="server" Text="Branch" Font-Bold="True" ForeColor="Black"
                        Font-Names="Book Antiqua" Font-Size="Medium"></asp:Label>
                </td>
                <td>
                    <asp:UpdatePanel ID="UpdatePanel3" runat="server">
                        <ContentTemplate>
                            <asp:TextBox ID="txtbranch" runat="server" Height="20px" CssClass="textbox textbox1"
                                ReadOnly="true" Width="120px" Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium">---Select---</asp:TextBox>
                            <asp:Panel ID="pbranch" runat="server" CssClass="multxtpanel" Height="250px" BackColor="White"
                                BorderColor="Black" BorderStyle="Solid" BorderWidth="2px">
                                <asp:CheckBox ID="chkbranch" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                    Font-Size="Medium" Text="Select All" AutoPostBack="True" OnCheckedChanged="chkbranch_CheckedChanged" />
                                <asp:CheckBoxList ID="chklstbranch" runat="server" Font-Size="Medium" AutoPostBack="True"
                                    Font-Bold="True" Font-Names="Book Antiqua" OnSelectedIndexChanged="chklstbranch_SelectedIndexChanged">
                                </asp:CheckBoxList>
                            </asp:Panel>
                            <asp:PopupControlExtender ID="PopupControlExtender2" runat="server" TargetControlID="txtbranch"
                                PopupControlID="pbranch" Position="Bottom">
                            </asp:PopupControlExtender>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Label ID="lblsec" runat="server" Text="Hour" Font-Bold="True" ForeColor="Black"
                        Font-Names="Book Antiqua" Font-Size="Medium"></asp:Label>
                </td>
                <td>
                    <asp:UpdatePanel ID="UpdatePanel4" runat="server">
                        <ContentTemplate>
                            <asp:TextBox ID="txtsec" runat="server" Height="20px" CssClass="textbox textbox1"
                                ReadOnly="true" Width="120px" Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium">---Select---</asp:TextBox>
                            <asp:Panel ID="psec" runat="server" BackColor="White" CssClass="multxtpanel" Height="150px"
                                BorderColor="Black" BorderStyle="Solid" BorderWidth="2px">
                                <asp:CheckBox ID="chksec" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                    Font-Size="Medium" Text="Select All" AutoPostBack="True" OnCheckedChanged="chksec_CheckedChanged" />
                                <asp:CheckBoxList ID="chklssec" runat="server" Font-Size="Medium" AutoPostBack="True"
                                    Font-Bold="True" Font-Names="Book Antiqua" OnSelectedIndexChanged="chklstsec_SelectedIndexChanged">
                                </asp:CheckBoxList>
                            </asp:Panel>
                            <asp:PopupControlExtender ID="PopupControlExtender3" runat="server" TargetControlID="txtsec"
                                PopupControlID="psec" Position="Bottom">
                            </asp:PopupControlExtender>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </td>
                <td>
                    <asp:Label ID="lblfrom1" runat="server" Text="Date" Font-Bold="true" Font-Names="Book Antiqua"
                        Font-Size="Medium"></asp:Label>
                </td>
                <td>
                    <asp:TextBox ID="txtfrom" CssClass="textbox textbox1" runat="server" Font-Bold="true"
                        Width="100px" Font-Names="Book Antiqua" Font-Size="Medium" AutoPostBack="true"
                        OnTextChanged="txtfrom_TextChanged"></asp:TextBox>
                    <asp:CalendarExtender ID="CalendarExtender2" CssClass="cal_Theme1 ajax__calendar_active"
                        TargetControlID="txtfrom" runat="server" Format="dd/MM/yyyy">
                    </asp:CalendarExtender>
                </td>
                <td>
                    <asp:Label ID="Label4" runat="server" Text="Entry By" Font-Bold="true" Font-Names="Book Antiqua"
                        Font-Size="Medium"></asp:Label>
                </td>
                <td>
                    <asp:DropDownList ID="ddlentry" runat="server" AutoPostBack="true" OnSelectedIndexChanged="txtfrom_TextChanged"
                        CssClass="textbox textbox1 ddlheight3" Font-Bold="true" Font-Names="Book Antiqua"
                        Font-Size="Medium">
                        <asp:ListItem Text="Admission No" Value="0"></asp:ListItem>
                        <asp:ListItem Text="Roll No" Value="1"></asp:ListItem>
                        <asp:ListItem Text="Reg No" Value="2"></asp:ListItem>
                    </asp:DropDownList>
                </td>
                <td>
                    <asp:Button ID="btngo" runat="server" Height="30px" CssClass="textbox1 btn2" Text="Go"
                        Font-Bold="true" Font-Names="Book Antiqua" Font-Size="Medium" OnClick="btngo_Click" />
                </td>
            </tr>
        </table>
    </center>
    <center>
        <asp:Label ID="errmsg" runat="server" Visible="false" Font-Bold="true" Font-Names="Book Antiqua"
            Font-Size="Medium" ForeColor="Red" Style="margin: 0px; margin-bottom: 10px; margin-top: 10px;
            position: relative;">
        </asp:Label>
        <table id="Panel3" runat="server" style="margin-left: 0px; width: 976px; margin: 0px;
            margin-bottom: 10px; margin-top: 10px; position: relative;">
            <tr>
                <td>
                    <div id="divscrll1" visible="false" runat="server" style="border-style: none; border-color: inherit;
                        border-width: 2px; height: 425px; width: 973px; overflow: scroll;">
                        <asp:GridView Visible="False" ID="gvuserodrlist" runat="server" AutoGenerateColumns="False"
                            OnRowCommand="grid_view_employee_RowCommad" OnRowDataBound="GridView1_RowDataBound"
                            CellPadding="1" ForeColor="#333333" GridLines="None" Width="559px">
                            <AlternatingRowStyle BackColor="White" />
                            <Columns>
                                <asp:TemplateField HeaderText="S.No" HeaderStyle-Width="70px" ItemStyle-Width="70px"
                                    ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Left" ItemStyle-Font-Bold="true"
                                    ControlStyle-Font-Size="Medium" HeaderStyle-Font-Size="Medium">
                                    <ItemTemplate>
                                        <asp:Label ID="lblSRNO" Width="40px" runat="server" Text='<%#Container.DataItemIndex+1 %>'></asp:Label>
                                    </ItemTemplate>
                                    <ControlStyle Font-Size="Medium"></ControlStyle>
                                    <HeaderStyle HorizontalAlign="Left" Font-Size="Medium" Width="70px"></HeaderStyle>
                                    <ItemStyle HorizontalAlign="Center" Font-Bold="True" Width="70px"></ItemStyle>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Batch" HeaderStyle-Width="80px" ItemStyle-Width="80px"
                                    ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Left" ControlStyle-Font-Size="Medium"
                                    HeaderStyle-Font-Bold="true" HeaderStyle-Font-Size="Medium">
                                    <ItemTemplate>
                                        <asp:Label ID="lblbatch_Year" Width="40px" runat="server" Text='<%#Eval("batch_Year")%>' />
                                    </ItemTemplate>
                                    <ControlStyle Font-Size="Medium"></ControlStyle>
                                    <HeaderStyle HorizontalAlign="Left" Font-Bold="True" Font-Size="Medium" Width="80px">
                                    </HeaderStyle>
                                    <ItemStyle HorizontalAlign="Center" Width="80px"></ItemStyle>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Degree" HeaderStyle-Width="110px" ItemStyle-Width="100px"
                                    ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" ControlStyle-Font-Size="Medium"
                                    HeaderStyle-Font-Bold="true" HeaderStyle-Font-Size="Medium">
                                    <ItemTemplate>
                                        <asp:Label ID="lblCourse_Name" Width="80px" runat="server" Text='<%#Eval("Course_Name")%>' />
                                        <asp:Label ID="lblCourse_id" Visible="false" runat="server" Text='<%#Eval("degree_code")%>' />
                                    </ItemTemplate>
                                    <ControlStyle Font-Size="Medium"></ControlStyle>
                                    <HeaderStyle HorizontalAlign="Center" Font-Bold="True" Font-Size="Medium" Width="100px">
                                    </HeaderStyle>
                                    <ItemStyle HorizontalAlign="Center" Width="110px"></ItemStyle>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Department" HeaderStyle-Width="150px" ItemStyle-Width="150px"
                                    ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left" ControlStyle-Font-Size="Medium"
                                    HeaderStyle-Font-Bold="true" HeaderStyle-Font-Size="Medium">
                                    <ItemTemplate>
                                        <asp:Label ID="lblDept_Name" Width="180px" runat="server" Text='<%#Eval("Dept_Name")%>'></asp:Label>
                                    </ItemTemplate>
                                    <ControlStyle Font-Size="Medium"></ControlStyle>
                                    <HeaderStyle HorizontalAlign="Left" Font-Bold="True" Font-Size="Medium" Width="150px">
                                    </HeaderStyle>
                                    <ItemStyle HorizontalAlign="Left" Width="150px"></ItemStyle>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Sem" HeaderStyle-Width="80px" ItemStyle-HorizontalAlign="Center"
                                    HeaderStyle-HorizontalAlign="Center" ControlStyle-Font-Size="Medium" HeaderStyle-Font-Bold="true"
                                    HeaderStyle-Font-Size="Medium">
                                    <ItemTemplate>
                                        <asp:Label ID="lblcurrent_semester" Width="38px" runat="server" Text='<%#Eval("current_semester")%>'></asp:Label>
                                    </ItemTemplate>
                                    <ControlStyle Font-Size="Medium"></ControlStyle>
                                    <HeaderStyle HorizontalAlign="Center" Font-Bold="True" Font-Size="Medium" Width="70px">
                                    </HeaderStyle>
                                    <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Sec" HeaderStyle-Width="80px" ItemStyle-HorizontalAlign="Center"
                                    HeaderStyle-HorizontalAlign="Center" ControlStyle-Font-Size="Medium" HeaderStyle-Font-Bold="true"
                                    HeaderStyle-Font-Size="Medium">
                                    <ItemTemplate>
                                        <asp:Label ID="lblsections" Width="38px" runat="server" Text='<%#Eval("sections")%>'></asp:Label>
                                    </ItemTemplate>
                                    <ControlStyle Font-Size="Medium"></ControlStyle>
                                    <HeaderStyle HorizontalAlign="Center" Font-Bold="True" Font-Size="Medium" Width="70px">
                                    </HeaderStyle>
                                    <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Absentees-Roll No" HeaderStyle-Width="400px" ItemStyle-Width="400px"
                                    ControlStyle-Font-Size="Medium" HeaderStyle-Font-Bold="true" HeaderStyle-Font-Size="Medium">
                                    <ItemTemplate>
                                        <asp:TextBox ID="txtabbsent" runat="server" Width="385px" AutoPostBack="true" TextMode="MultiLine"
                                            Height="33px" onkeypress="return restrictspecial(event,'EMAIL',this);" onkeydown="setHeight(this);"></asp:TextBox>
                                    </ItemTemplate>
                                    <ControlStyle Font-Size="Medium"></ControlStyle>
                                    <HeaderStyle Font-Bold="True" Font-Size="Medium" Width="400px"></HeaderStyle>
                                    <ItemStyle Width="400px"></ItemStyle>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Select" HeaderStyle-Width="100px" ItemStyle-HorizontalAlign="Center"
                                    ControlStyle-Font-Size="Medium" HeaderStyle-Font-Bold="true" HeaderStyle-Font-Size="Medium">
                                    <ItemTemplate>
                                        <asp:LinkButton ID="LinkButton1" runat="server" CommandName="Select" Font-Overline="false"
                                            CommandArgument='<%# Container.DataItemIndex %>' Text="Select"></asp:LinkButton>
                                    </ItemTemplate>
                                    <ControlStyle Font-Size="Medium"></ControlStyle>
                                    <HeaderStyle Font-Bold="True" Font-Size="Medium" Width="80px"></HeaderStyle>
                                    <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="All Present" HeaderStyle-Width="400px" ItemStyle-Width="400px"
                                    ControlStyle-Font-Size="Medium" HeaderStyle-Font-Bold="true" HeaderStyle-Font-Size="Medium">
                                    <ItemTemplate>
                                        <asp:CheckBox ID="presentall" runat="server" Width="100px" />
                                        <%--<div id="sd" style=" width:100px; height:10px; background-color:Red;"></div>--%>
                                    </ItemTemplate>
                                    <ControlStyle Font-Size="Medium"></ControlStyle>
                                    <HeaderStyle Font-Bold="True" Font-Size="Medium" Width="400px"></HeaderStyle>
                                    <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                </asp:TemplateField>
                            </Columns>
                            <EditRowStyle BackColor="#7C6F57" />
                            <FooterStyle BackColor="#1C5E55" Font-Bold="True" ForeColor="White" />
                            <HeaderStyle BackColor="#0CA6CA" Font-Bold="True" ForeColor="White" />
                            <PagerStyle BackColor="#666666" ForeColor="White" HorizontalAlign="Center" />
                            <RowStyle BackColor="#E3EAEB" />
                            <SelectedRowStyle BackColor="#C5BBAF" Font-Bold="True" ForeColor="#333333" />
                            <SortedAscendingCellStyle BackColor="#F8FAFA" />
                            <SortedAscendingHeaderStyle BackColor="#246B61" />
                            <SortedDescendingCellStyle BackColor="#D4DFE1" />
                            <SortedDescendingHeaderStyle BackColor="#15524A" />
                        </asp:GridView>
                    </div>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:CheckBox ID="chkselectall" runat="server" Font-Bold="True" Text="All Present"
                        AutoPostBack="true" OnCheckedChanged="chkselectall_Change" Style="position: absolute;
                        left: 533px; padding-top: 5px;" Font-Names="Book Antiqua" Visible="false" Font-Size="Medium" />
                    <asp:CheckBox ID="chksms" runat="server" Font-Bold="True" Text="SMS" Style="position: absolute;
                        left: 645px; padding-top: 5px;" Font-Names="Book Antiqua" Visible="false" Font-Size="Medium" />
                    <asp:CheckBox ID="chkvoice" runat="server" Font-Bold="True" Text="Voice Call" Style="position: absolute;
                        left: 720px; padding-top: 5px;" Font-Names="Book Antiqua" Visible="false" Font-Size="Medium" />
                    <asp:Button ID="btnxl" runat="server" Text="Save" Font-Bold="True" Width="85px" Style="position: absolute;
                        left: 845px;" Font-Names="Book Antiqua" Visible="false" Font-Size="Medium" OnClick="btnxl_Click" />
                    <asp:Button ID="btnprintmaster" runat="server" Text="Cancel" Width="85px" Style="position: absolute;
                        left: 865px;" Font-Names="Book Antiqua" Font-Size="Medium" Font-Bold="true" Visible="false" />
                </td>
            </tr>
        </table>
    </center>
    <div>
        <asp:ModalPopupExtender runat="server" ID="mdl_full_employee_details" TargetControlID="Button1"
            BackgroundCssClass="modal_popup_background_color" PopupControlID="pnl_employee_details">
        </asp:ModalPopupExtender>
        <asp:Button ID="Button1" runat="server" Text="Button" Style="display: none;" />
        <asp:Panel ID="pnl_employee_details" runat="server" Visible="false" BackColor="#A0BFA9"
            Style="border-style: none; border-color: inherit; border-width: 1px; height: 610px;
            width: 958px;" BorderColor="Blue">
            <table style="background-color: #1C5E55; width: 958px; padding-left: 7px; font-size: medium;
                color: #FFFFFF; font-weight: bold;">
                <tr>
                    <td align="center" colspan="4">
                        <asp:Label ID="Label2" runat="server" Text="Student List" Font-Bold="true" Font-Size="Large"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td align="center">
                        <asp:Label ID="lbldatch" Font-Bold="true" Font-Size="Medium" runat="server" Text="Batch  : "></asp:Label>
                        <asp:Label ID="lblbaatch" Font-Bold="true" Font-Size="Medium" runat="server"></asp:Label>
                    </td>
                    <td align="center">
                        &nbsp;&nbsp;
                        <asp:Label ID="lbldegh" Font-Bold="true" Font-Size="Medium" runat="server" Text="Degree  : "></asp:Label>
                        <asp:Label ID="lbldegh1" Font-Bold="true" Font-Size="Medium" runat="server"></asp:Label>
                    </td>
                    <td align="center">
                        &nbsp;&nbsp;
                        <asp:Label ID="bldept" Font-Bold="true" Font-Size="Medium" runat="server" Text="Department  : "></asp:Label>
                        <asp:Label ID="bldept1" Font-Bold="true" Font-Size="Medium" runat="server" Text="Label"></asp:Label>
                    </td>
                    <td align="center">
                        &nbsp;&nbsp;
                        <asp:Label ID="lblseee" Font-Bold="true" Font-Size="Medium" runat="server" Text="Sem & Sec  : "></asp:Label>
                        <asp:Label ID="lblseeemsec" runat="server"></asp:Label>
                    </td>
                </tr>
                <tr align="center">
                    <td colspan="2">
                        <asp:Label ID="lblerrros" Font-Bold="true" Style="top: 53px; left: 368px; position: absolute;
                            font-family: 'Book Antiqua';" Font-Size="Medium" ForeColor="Red" Visible="false"
                            runat="server"></asp:Label>
                    </td>
                </tr>
            </table>
            <br />
            <div id="divscrll" runat="server" style="border-style: none; border-color: inherit;
                border-width: 1px; height: 507px; width: 955px; overflow: auto;">
                <div style="margin-left: 10px;">
                    <asp:DataList ID="gvdatass" runat="server" Font-Names="Book Antiqua" Font-Size="Small"
                        RepeatColumns="3" Width="900px" ForeColor="#333333">
                        <AlternatingItemStyle BackColor="White" />
                        <FooterStyle BackColor="#1C5E55" Font-Bold="True" ForeColor="White" />
                        <HeaderStyle BackColor="#1C5E55" Font-Bold="True" ForeColor="White" />
                        <ItemStyle BackColor="#E3EAEB" BorderWidth="1px" Height="0px" />
                        <ItemTemplate>
                            <table>
                                <tr>
                                    <td>
                                        <asp:CheckBox ID="chkup3" runat="server" />
                                    </td>
                                    <td>
                                        <asp:Label ID="Label3" ForeColor="Red" runat="server" Text='<%# Eval("SSno") %>'></asp:Label>
                                    </td>
                                    <td>
                                        <asp:Label ID="lblRoll_No" ForeColor="Red" runat="server" Text='<%# Eval("Roll_No") %>'></asp:Label>
                                        <asp:Label ID="lblRoll_Noonly" ForeColor="Red" Visible="false" runat="server" Text='<%# Eval("Roll_Noonly") %>'></asp:Label>
                                        <asp:Label ID="lblreg_noonly" ForeColor="Red" Visible="false" runat="server" Text='<%# Eval("Reg_Noonly") %>'></asp:Label>
                                        <%--  <asp:Label ID="lblroll_admit" ForeColor="Red" visible="false" runat="server" Text='<%# Eval("Roll_Admit") %>'></asp:Label>--%>
                                    </td>
                                    <td>
                                        <asp:Label ID="lblprice1" ForeColor="BlueViolet" Width="156px" runat="server" Text='<%# Eval("Stud_Name") %>'></asp:Label>
                                    </td>
                                </tr>
                            </table>
                        </ItemTemplate>
                        <SelectedItemStyle BackColor="#C5BBAF" Font-Bold="True" ForeColor="#333333" />
                    </asp:DataList></div>
                <br />
            </div>
            <div align="center">
                <asp:Button ID="btnok" CssClass="dropdown" Height="25px" runat="server" Width="65px"
                    Text="Ok" OnClick="Button2_Click" />
                <asp:Button ID="btnexits" CssClass="dropdown" Height="25px" runat="server" Width="65px"
                    Text="Exit" OnClick="Button3_Click" />
            </div>
        </asp:Panel>
        <div style="margin-left: 0px; top: 262px; left: 600px; position: absolute;">
            <table>
                <tr>
                    <td>
                        <span style="font-weight: bold; font-size: larger;" id="msg1"></span>
                    </td>
                </tr>
            </table>
        </div>
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

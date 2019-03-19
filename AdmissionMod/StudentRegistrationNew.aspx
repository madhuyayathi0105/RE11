<%@ Page Title="" Language="C#" MasterPageFile="~/AdmissionMod/AttendanceSubSiteMaster.master"
    AutoEventWireup="true" CodeFile="StudentRegistrationNew.aspx.cs" Inherits="AdmissionMod_StudentRegistrationNew" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <style type="text/css">
        .rdbstyle input[type=radio]
        {
            display: none;
        }
        .rdbstyle input[type=radio] + label
        {
            display: inline-block;
            margin: -2px;
            padding: 4px 12px;
            margin-bottom: 0;
            font-size: 14px;
            line-height: 20px;
            color: #993399;
            text-align: center;
            text-shadow: 0 1px 1px rgba(255,255,255,0.75);
            vertical-align: middle;
            cursor: pointer;
            background-color: #f5f5f5;
            background-image: -moz-linear-gradient(top,#fff,#e6e6e6);
            background-image: -webkit-gradient(linear,0 0,0 100%,from(#fff),to(#e6e6e6));
            background-image: -webkit-linear-gradient(top,#fff,#e6e6e6);
            background-image: -o-linear-gradient(top,#fff,#e6e6e6);
            background-image: linear-gradient(to bottom,#fff,#e6e6e6);
            background-repeat: repeat-x;
            border: 1px solid #ccc;
            border-color: #e6e6e6 #e6e6e6 #bfbfbf;
            border-color: rgba(0,0,0,0.1) rgba(0,0,0,0.1) rgba(0,0,0,0.25);
            border-bottom-color: #b3b3b3;
            filter: progid:DXImageTransform.Microsoft.gradient(startColorstr='#ffffffff',endColorstr='#ffe6e6e6',GradientType=0);
            filter: progid:DXImageTransform.Microsoft.gradient(enabled=false);
            -webkit-box-shadow: inset 0 1px 0 rgba(255,255,255,0.2),0 1px 2px rgba(0,0,0,0.05);
            -moz-box-shadow: inset 0 1px 0 rgba(255,255,255,0.2),0 1px 2px rgba(0,0,0,0.05);
            box-shadow: inset 0 1px 0 rgba(255,255,255,0.2),0 1px 2px rgba(0,0,0,0.05);
        }
        .rdbstyle input[type=radio]:checked + label
        {
            background-image: none;
            outline: 0;
            -webkit-box-shadow: inset 0 2px 4px rgba(0,0,0,0.15),0 1px 2px rgba(0,0,0,0.05);
            -moz-box-shadow: inset 0 2px 4px rgba(0,0,0,0.15),0 1px 2px rgba(0,0,0,0.05);
            box-shadow: inset 0 2px 4px rgba(0,0,0,0.15),0 1px 2px rgba(0,0,0,0.05);
            border-bottom-color: #b3b3b3;
            border-bottom-style: solid;
            border-bottom-color: #89D17C;
            border-bottom-width: medium;
        }
    </style>
    <script type="text/javascript">
        function date() {
            var dateObj = new Date();
            var week = dateObj.format("hh:mm:ss tt");
            document.getElementById('<%=lbltime.ClientID %>').innerHTML = week.toString();
            setTimeout('date()', 500);
            return "";
        }
        function enterkeyvoid(e) {
            if (e.keycode == 13 || e.which == 13) {
                // void enter key
                return false;
            }
        }
        function verifyEntry() {
            var txtbx = document.getElementById("<%=txt_applicationno.ClientID %>");
            if (txtbx.value == "" || txtbx.vale == "0") {
                txtbx.style.borderColor = "#ff0000";
                return false;
            } else {
                txtbx.style.borderColor = "#c4c4c4";
            }
        }
        function displayNormal(txtid) {
            txtid.style.borderColor = "#c4c4c4";
        }
        function applicationno_OnClientPopulating(sender, args) {
            var college = document.getElementById("<%=ddlCollege.ClientID%>").value;
            var batch = document.getElementById("<%=ddlbatch.ClientID%>").value;
            var edulevel = document.getElementById("<%=ddlEduLev.ClientID%>").value;
            var courseid = document.getElementById("<%=ddlcourse.ClientID%>").value;
            //var tapselect = $('input:checkbox').value($("rdbtype"));
            //var tapselect = $('#<%=rdbtype.ClientID %>').index($("rdbtype"));
            var tapselect = 0;
            var checkBoxList = document.getElementById('<%= rdbtype.ClientID %>');
            var checkboxes = checkBoxList.getElementsByTagName('input');
            for (var loop = 0; loop < checkboxes.length; loop++) {
                if (checkboxes[loop].checked)
                    tapselect = loop;
            }
            var filtervalues = college + '$' + batch + '$' + edulevel + '$' + courseid + '$' + tapselect;
            sender.set_contextKey(filtervalues);
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">
    <center>
        <center>
            <span class="fontstyleheader" style="color: Green;">Counselling Student Registration</span>
        </center>
        <asp:ScriptManager ID="scptMgrNew" runat="server">
        </asp:ScriptManager>
        <div style="margin-top: 0px; width: 950px;">
            <asp:RadioButtonList ID="rdbtype" runat="server" OnSelectedIndexChanged="rdbtype_SelectedIndexChanged"
                AutoPostBack="true" RepeatDirection="Horizontal" CellSpacing="4" BorderColor="#999999"
                Font-Bold="True" CssClass="rdbstyle" Style="float: left;">
                <asp:ListItem Value="0" Selected="True">Registration</asp:ListItem>
                <%--<asp:ListItem Value="1">Verification</asp:ListItem>--%>
            </asp:RadioButtonList>
        </div>
        <br />
        <br />
        <div class="maindivstyle" style="width: 950px; height: 498px;">
            <table class="maintablestyle">
                <tr>
                    <td>
                        <asp:Label ID="lblCollege" runat="server" Text="Institute"></asp:Label>
                    </td>
                    <td>
                        <asp:DropDownList ID="ddlCollege" runat="server" CssClass="textbox1 ddlheight6">
                        </asp:DropDownList>
                    </td>
                    <td>
                        <asp:Label ID="lblBatch" runat="server" Text="Batch"></asp:Label>
                    </td>
                    <td>
                        <asp:DropDownList ID="ddlbatch" runat="server" CssClass="textbox1 ddlheight">
                        </asp:DropDownList>
                    </td>
                    <td>
                        <asp:Label ID="lblEdulevel" runat="server" Width="85px" Text="Edu Level"></asp:Label>
                    </td>
                    <td>
                        <asp:DropDownList ID="ddlEduLev" runat="server" CssClass="textbox1 ddlheight" AutoPostBack="true"
                            OnSelectedIndexChanged="ddlEduLev_selectedindexchanged">
                        </asp:DropDownList>
                    </td>
                    <td>
                        <asp:Label ID="lblCourse" runat="server" Text="Course"></asp:Label>
                    </td>
                    <td>
                        <asp:DropDownList ID="ddlcourse" runat="server" CssClass="textbox1 ddlheight3">
                        </asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <td>
                        Date
                    </td>
                    <td colspan="2">
                        <asp:TextBox ID="txt_date" runat="server" CssClass="textbox textbox1 txtheight1"></asp:TextBox>
                        Time:
                        <asp:Label ID="lbltime" runat="server" ForeColor="#FFFFFF" Width="200px"><script>                                                                                                     document.write(date())</script></asp:Label>
                    </td>
                    <td colspan="5">
                        <asp:CheckBox ID="cbinclude" runat="server" Text="Include Date and Session" />
                        Session
                        <asp:DropDownList ID="ddlsession" runat="server" CssClass="textbox1 ddlheight3">
                        </asp:DropDownList>
                    </td>
                </tr>
            </table>
            <br />
            <div id="registration_div" runat="server">
                <table>
                    <tr>
                        <td>
                            <span id="id_span" runat="server">Application Number</span>
                        </td>
                        <td>
                            <asp:TextBox ID="txt_applicationno" placeholder="Application Number" runat="server"
                                CssClass="textbox textbox1 txtheight3" TabIndex="1" onkeyup="return displayNormal(this);"
                                onkeypress="return enterkeyvoid(event)"></asp:TextBox>
                            <asp:AutoCompleteExtender ID="applicationno" runat="server" DelimiterCharacters=""
                                Enabled="True" ServiceMethod="Getname" MinimumPrefixLength="0" CompletionInterval="100"
                                EnableCaching="false" CompletionSetCount="10" ServicePath="" TargetControlID="txt_applicationno"
                                CompletionListCssClass="autocomplete_completionListElement" CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem"
                                CompletionListItemCssClass="txtsearchpan" OnClientPopulating="applicationno_OnClientPopulating">
                            </asp:AutoCompleteExtender>
                            <asp:FilteredTextBoxExtender ID="applicationnoExtender" runat="server" TargetControlID="txt_applicationno"
                                FilterType="UppercaseLetters,LowercaseLetters,Numbers,custom" ValidChars=" .">
                            </asp:FilteredTextBoxExtender>
                        </td>
                        <td>
                            <asp:Button ID="btn_register" runat="server" CssClass="textbox btn2" Text="Register"
                                OnClick="btn_register_click" OnClientClick="return verifyEntry()" TabIndex="2" />
                        </td>
                    </tr>
                </table>
                <br />
                <div id="verification_div" runat="server" style="width: 900px;" visible="false">
                    <div id="certificate_grid_div" runat="server" width="450px" style="float: right;">
                        <center>
                            <asp:GridView ID="certificate_grid" runat="server" CaptionAlign="Top" HorizontalAlign="Justify"
                                CellPadding="4" ForeColor="#333333" GridLines="Vertical" Width="400px">
                                <RowStyle BackColor="#E3EAEB" />
                                <Columns>
                                    <asp:TemplateField HeaderText="S.No">
                                        <ItemTemplate>
                                            <asp:Label ID="lbl_sno" runat="server" Text="<%# Container.DisplayIndex+1 %>"></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Center" Width="50px" />
                                    </asp:TemplateField>
                                </Columns>
                                <Columns>
                                    <asp:TemplateField HeaderText="Select">
                                        <ItemTemplate>
                                            <asp:CheckBox ID="cb_select" runat="server" />
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Center" Width="50px" />
                                    </asp:TemplateField>
                                </Columns>
                                <FooterStyle BackColor="#1C5E55" Font-Bold="True" ForeColor="White" />
                                <PagerStyle BackColor="#666666" ForeColor="White" HorizontalAlign="Center" />
                                <SelectedRowStyle BackColor="#C5BBAF" Font-Bold="True" ForeColor="#333333" />
                                <HeaderStyle BackColor="#0ca6ca" Font-Bold="True" ForeColor="White" />
                                <EditRowStyle BackColor="#7C6F57" />
                                <AlternatingRowStyle BackColor="White" />
                            </asp:GridView>
                            <asp:CheckBox ID="cb_selectall" runat="server" Style="float: left;" OnCheckedChanged="cb_selectall_click"
                                AutoPostBack="true" Text="Select All" />
                            <br />
                            <asp:Button ID="btn_shortlist" runat="server" CssClass="textbox btn2" Text="Verified"
                                Visible="false" OnClick="btn_shortlist_click" TabIndex="3" />
                        </center>
                    </div>
                    <div id="Student_details_div" runat="server" width="450px" style="float: left;">
                        <div id="personaldet_div" runat="server">
                            <table width="450px">
                                <tr>
                                    <td colspan="2" style="background-color: #0ca6ca; color: White;">
                                        Personal Details
                                    </td>
                                </tr>
                                <tr>
                                    <td style="width: 150px;">
                                        Application Number
                                    </td>
                                    <td>
                                        <span runat="server" id="applicantno_span"></span>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        Applicant Name
                                    </td>
                                    <td>
                                        <span runat="server" id="applicantname_span"></span>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        Date of Birth
                                    </td>
                                    <td>
                                        <span runat="server" id="dob_span"></span>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        Gender
                                    </td>
                                    <td>
                                        <span runat="server" id="gender_span"></span>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        Mobile Number
                                    </td>
                                    <td>
                                        <span runat="server" id="studmobileno_span"></span>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        Father Name
                                    </td>
                                    <td>
                                        <span runat="server" id="fathername_span"></span>
                                    </td>
                                </tr>
                            </table>
                        </div>
                        <div id="acadamicdet_div" runat="server">
                            <table width="450px">
                                <tr>
                                    <td colspan="2" style="background-color: #0ca6ca; color: White;">
                                        Acadamic Details
                                    </td>
                                </tr>
                                <tr>
                                    <td style="width: 150px;">
                                        JEE Mark
                                    </td>
                                    <td>
                                        <span runat="server" id="jeemark_span"></span>
                                    </td>
                                </tr>
                                <tr style="display: none;">
                                    <td>
                                        JEE State Rank
                                    </td>
                                    <td>
                                        <span runat="server" id="jeestaterank"></span>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        HSC Mark
                                    </td>
                                    <td>
                                        <span runat="server" id="hscmark_span"></span>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        Board
                                    </td>
                                    <td>
                                        <span runat="server" id="board"></span>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        Year of Passing
                                    </td>
                                    <td>
                                        <span runat="server" id="yearofpassing_span"></span>
                                    </td>
                                </tr>
                            </table>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </center>
    <div id="alert_pop" runat="server" visible="false" style="height: 300em; z-index: 1000;
        width: 100%; background-color: rgba(54, 25, 25, .2); position: absolute; top: 0;
        left: 0px;">
        <center>
            <div id="pnl2" runat="server" class="table" style="background-color: White; height: 120px;
                width: 238px; border: 5px solid #0CA6CA; border-top: 25px solid #0CA6CA; margin-top: 300px;
                border-radius: 10px;">
                <center>
                    <table style="height: 100px; width: 100%">
                        <tr>
                            <td align="center">
                                <asp:Label ID="lbl_alert" runat="server" Text="" Style="color: Red;" Font-Bold="true"
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
</asp:Content>

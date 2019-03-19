<%@ Page Title="" Language="C#" MasterPageFile="~/AdmissionMod/AttendanceSubSiteMaster.master"
    AutoEventWireup="true" CodeFile="CBSCRegistration.aspx.cs" Inherits="AdmissionMod_CBSCRegistration" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <script type="text/javascript">
        function verifyEntry() {
            var txtbx = document.getElementById("<%=txt_applicationno.ClientID %>");
            var txtpwd = document.getElementById("<%=txt_Password.ClientID %>");
            if (txtbx.value == "" || txtbx.vale == "0") {
                txtbx.style.borderColor = "#ff0000";
                return false;
            } else {
                txtbx.style.borderColor = "#B2B2B2";
            }
        }
        function displayNormal(txtid) {
            txtid.style.borderColor = "#B2B2B2";
        }

        function verifyMobile() {
            var txtbx = document.getElementById("<%=txt_Mobile.ClientID %>");
            var val = txtbx.value;
            if (val == "" || val.length != 10) {
                txtbx.style.borderColor = "#ff0000";
                return false;
            }
        }

        function date() {
            var dateObj = new Date();
            var week = dateObj.format("hh:mm:ss tt");
            document.getElementById('<%=lbltime.ClientID %>').innerHTML = week.toString();
            setTimeout('date()', 500);
            return "";
        }
        
    </script>
    <style type="text/css">
        .modalPopup
        {
            background: rgba(54, 25, 25, .2);
        }
        .ponts
        {
            cursor: pointer;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <center>
        <div id="popupEntryChk" runat="server" style="height: auto; z-index: -100000; width: 100%;
            background-color: rgba(255, 255, 255, 1); position: absolute; top: 100; left: 0;">
            <asp:Image ID="image3" runat="server" Visible="false" ImageUrl="~/Handler6.ashx"
                Style="height: 100px; width: 100%;" />
            <br />
            <br />
            <br />
            <center>
                <div style="width: 560px; height: auto; border: 2px solid #68AA; border-radius: 10px;
                    border-top-width: 10px;">
                    <br />
                    <span id="Span1" style="color: #336699; font-weight: bold; font-size: large;" runat="server">
                        CBCS Registration</span>
                    <br />
                    <br />
                    <table cellpadding="5">
                        <tr>
                            <td>
                                Register Number
                                <br />
                            </td>
                            <td>
                                <asp:TextBox ID="txt_applicationno" onkeyup="return displayNormal(this);" Height="25px"
                                    CssClass="textbox textbox1" runat="server" Width="140px" MaxLength="20"></asp:TextBox>
                                <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender9" runat="server" TargetControlID="txt_applicationno"
                                    FilterType="UppercaseLetters,LowercaseLetters,Numbers" ValidChars=" ">
                                </asp:FilteredTextBoxExtender>
                                <span style="color: Red;">*</span>
                            </td>
                        </tr>
                        <tr style="display: none;">
                            <td>
                                Password
                            </td>
                            <td>
                                <%-- <asp:DropDownList ID="ddldate" CssClass="textbox3 textbox1" runat="server" onblur="blurFunction(this)"
                                    onfocus="myFunction(this)" Style="width: 58px;">
                                </asp:DropDownList>
                                <asp:DropDownList ID="ddlmonth" CssClass="textbox3 textbox1" runat="server" onblur="blurFunction(this)"
                                    onfocus="myFunction(this)" Style="width: 68px;">
                                    <asp:ListItem Value="1">JAN</asp:ListItem>
                                    <asp:ListItem Value="2">FEB</asp:ListItem>
                                    <asp:ListItem Value="3">MAR</asp:ListItem>
                                    <asp:ListItem Value="4">APR</asp:ListItem>
                                    <asp:ListItem Value="5">MAY</asp:ListItem>
                                    <asp:ListItem Value="6">JUN</asp:ListItem>
                                    <asp:ListItem Value="7">JUL</asp:ListItem>
                                    <asp:ListItem Value="8">AUG</asp:ListItem>
                                    <asp:ListItem Value="9">SEP</asp:ListItem>
                                    <asp:ListItem Value="10">OCT</asp:ListItem>
                                    <asp:ListItem Value="11">NOV</asp:ListItem>
                                    <asp:ListItem Value="12">DEC</asp:ListItem>
                                </asp:DropDownList>
                                <asp:DropDownList ID="ddlyear" CssClass="textbox3 textbox1" runat="server" onblur="blurFunction(this)"
                                    onchange="return agecal(this)" onfocus="myFunction(this)" Style="width: 60px;">
                                </asp:DropDownList>--%>
                                <asp:TextBox ID="txt_Password" placeholder="dd/MM/yyyy" Visible="false" onkeyup="return displayNormal(this);"
                                    CssClass="textbox textbox1" Height="25px" runat="server" Width="100px" MaxLength="10"></asp:TextBox>
                                <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" runat="server" TargetControlID="txt_Password"
                                    FilterType="Custom,Numbers" ValidChars="/">
                                </asp:FilteredTextBoxExtender>
                                <span style="color: Red;">*</span>
                            </td>
                        </tr>
                        <tr>
                            <td>
                            </td>
                            <td align="left">
                                <asp:Button ID="Button1" runat="server" Text="Submit" OnClick="btn_submit_OnClick"
                                    CssClass="textbox textbox1 type" OnClientClick="return verifyEntry()" BackColor="Brown"
                                    ForeColor="White" Width="70px" Height="30px" />
                            </td>
                        </tr>
                        <div id="submitdiv" runat="server" visible="false">
                            <tr>
                                <td>
                                    Mobile No
                                </td>
                                <td>
                                    <asp:TextBox ID="txt_Mobile" onkeyup="return displayNormal(this);" Height="25px"
                                        CssClass="textbox textbox1" runat="server" Width="140px" MaxLength="10"></asp:TextBox>
                                    <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender2" runat="server" TargetControlID="txt_Mobile"
                                        FilterType="Numbers" ValidChars="">
                                    </asp:FilteredTextBoxExtender>
                                    <span style="color: Red;">*</span>
                                </td>
                                <td>
                                    <asp:Button ID="btnOtp" runat="server" Text="Send OTP" CssClass="textbox textbox1 type"
                                        BackColor="Brown" OnClick="btnOtp_Click" OnClientClick="return verifyMobile()"
                                        ForeColor="White" Width="100px" Height="30px" />
                                </td>
                            </tr>
                            <tr>
                                <td colspan="3">
                                    <span id="Span2" style="color: #1A1F7F;" runat="server">Note: All the information regarding
                                        CBCS registration will be sent this Number.</span>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    Enter OTP
                                </td>
                                <td>
                                    <asp:TextBox ID="txt_OTP" onkeyup="return displayNormal(this);" Height="25px" CssClass="textbox textbox1"
                                        runat="server" Width="60px" placeholder="xxxxxxx" MaxLength="7"></asp:TextBox>
                                    <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender3" runat="server" TargetControlID="txt_OTP"
                                        FilterType="Numbers" ValidChars="">
                                    </asp:FilteredTextBoxExtender>
                                    <span style="color: Red;">*</span>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                </td>
                                <td align="left">
                                    <asp:Button ID="btnConfirm" runat="server" Text="Confirm" OnClick="btnConfirm_OnClick"
                                        CssClass="textbox textbox1 type" BackColor="Brown" ForeColor="White" Width="70px"
                                        Height="30px" />
                                </td>
                            </tr>
                        </div>
                        <tr>
                            <td colspan="3">
                                <span id="errorspan" style="color: Red;" runat="server"></span>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2" style="font-size: large;">
                                <asp:UpdatePanel ID="upd" runat="server">
                                    <contenttemplate>
                                        <asp:Timer ID="tmr" runat="server" Interval="1000" OnTick="tmr_Click">
                                        </asp:Timer>
                                        <asp:Label ID="txt_date" runat="server" ForeColor="Black"></asp:Label>
                                        <asp:Label ID="lbltime" runat="server" ForeColor="Black" Width="200px"></asp:Label>
                                    </contenttemplate>
                                </asp:UpdatePanel>
                            </td>
                        </tr>
                    </table>
                </div>
            </center>
        </div>
        <div id="ttSelectionDiv" runat="server" style='width: 950px; margin: 0px; margin-top: 12px;
            background-color: #F0F0F0;' visible="false">
            <asp:Image ID="imgBanner" runat="server" Visible="false" ImageUrl="~/Handler6.ashx"
                Style="height: 100px; width: 100%;" />
            <br />
            <br />
            <div style="width: 100%; height: 25px; background-color: #0CA6CA; border-radius: 5px;
                font-weight: bold; font-size: larger; text-align: left;">
                <span style="padding-left: 50px;">Student Details </span><span style="padding-left: 750px;
                    position: absolute;">
                    <asp:ImageButton ID="imgLogout" runat="server" Width="30px" Height="30px" ImageUrl="~/images/close.png"
                        OnClick="imgLogout_OnClick" /></span>
            </div>
            <table style="width: 100%; font-size: medium; text-align: left; text-indent: 50px;">
                <tr>
                    <td>
                    </td>
                    <td>
                    </td>
                </tr>
                <tr>
                    <td style="width: 200px; font-weight: bold;">
                        Student Name
                    </td>
                    <td colspan="3">
                        :
                        <asp:Label ID="lblStudName" runat="server"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td style="width: 200px; font-weight: bold;">
                        Registration No
                    </td>
                    <td>
                        :
                        <asp:Label ID="lblAppFormNo" runat="server"></asp:Label>
                        <asp:Label ID="lblAppNo" runat="server" Visible="false"></asp:Label>
                    </td>
                    <td style="font-weight: bold;">
                        Branch
                    </td>
                    <td>
                        :
                        <asp:Label ID="lblBranchDisp" runat="server"></asp:Label>
                        <asp:Label ID="lblBranch" runat="server" Visible="false"></asp:Label>
                        <asp:Label ID="lblsection" runat="server" Visible="false"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td style="width: 200px; font-weight: bold;">
                        Batch<span style="padding-left: 50px;"></span>
                    </td>
                    <td>
                        :
                        <asp:Label ID="lblBatch" runat="server"></asp:Label>
                        <asp:Label ID="lblCollegeCode" runat="server" Visible="false"></asp:Label>
                    </td>
                    <td style="font-weight: bold;">
                        Semester
                    </td>
                    <td>
                        :
                        <asp:Label ID="lblSem" runat="server"></asp:Label>
                    </td>
                </tr>
            </table>
            <div style="width: 100%; height: 25px; background-color: #0CA6CA; border-radius: 5px;
                font-weight: bold; font-size: larger; text-align: left;">
                <span style="padding-left: 50px;">Time Tables </span><span style="padding-left: 600px;">
                    <a href=" http://sastra.edu/cbcs/2017junindex.php " target="_blank" style="color: White;">
                        View Time Table</a> </span>
                <br />
            </div>
            <br />
            <asp:UpdatePanel ID="updPnlTmr" runat="server">
                <contenttemplate>
                    <asp:Timer ID="tmrTTStat" runat="server" Interval="20000" OnTick="tmrTTStat_OnTick">
                    </asp:Timer>
                    <div style="height: auto;">
                        <center>
                            <asp:CheckBoxList ID="cblcolumnorder" runat="server" Height="43px" AutoPostBack="true"
                                Width="800px" Style="font-family: 'Book Antiqua'; font-weight: 700; font-size: medium;"
                                RepeatColumns="5" RepeatDirection="Horizontal" Visible="false">
                                <asp:ListItem Selected="True" Value="0">SUBJECT CODE</asp:ListItem>
                                <asp:ListItem Value="1">SUBJECT NAME</asp:ListItem>
                                <asp:ListItem Selected="True" Value="2">STAFF CODE</asp:ListItem>
                                <asp:ListItem Value="3">STAFF NAME</asp:ListItem>
                                <asp:ListItem Selected="True" Value="4">ROOM NAME</asp:ListItem>
                            </asp:CheckBoxList>
                            <%--OnSelectedIndexChanged="cblcolumnorder_SelectedIndexChanged"--%>
                            <div>
                                <%--style="width:950px; overflow:auto;"--%>
                                <asp:GridView ID="gridFnl" runat="server" AutoGenerateColumns="False" Visible="false"
                                    GridLines="Both" HeaderStyle-BackColor="#0CA6CA" HeaderStyle-Font-Bold="true"
                                    HeaderStyle-HorizontalAlign="Center" HeaderStyle-Font-Names="Book Antiqua" HeaderStyle-Font-Size="Medium"
                                    OnRowDataBound="gridFnl_OnRowDataBound" Style="font-size: medium;">
                                    <Columns>
                                        <asp:TemplateField HeaderText="S.No">
                                            <ItemTemplate>
                                                <asp:Label ID="lblSNo" runat="server" Text='<%#Container.DisplayIndex+1 %>'></asp:Label>
                                                <%--<asp:Label ID="lblAppNo" runat="server" Text='<%#Eval("AppNo") %>'></asp:Label>--%>
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Center" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Name">
                                            <ItemTemplate>
                                                <asp:Label ID="lblTTName" runat="server" Text='<%#Eval("TTName") %>'></asp:Label>
                                                <asp:Label ID="lblTTPk" runat="server" Text='<%#Eval("TTPk") %>' Visible="false"></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Section">
                                            <ItemTemplate>
                                                <asp:Label ID="lblTTSec" runat="server" Text='<%#Eval("TTSec") %>'></asp:Label>
                                                <asp:Label ID="lblbatch" runat="server" Visible="false" Text='<%#Eval("batch") %>'></asp:Label>
                                                <asp:Label ID="lbldegree" runat="server" Visible="false" Text='<%#Eval("degree") %>'></asp:Label>
                                                <asp:Label ID="lblsem" runat="server" Visible="false" Text='<%#Eval("sem") %>'></asp:Label>
                                                <asp:Label ID="lblcollege" runat="server" Visible="false" Text='<%#Eval("collegecode") %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Center" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Remaining">
                                            <ItemTemplate>
                                                <asp:Label ID="lblTTMaxRem" runat="server" Text='<%#Eval("TTMaxRem") %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Center" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Time Tables" Visible="false">
                                            <ItemTemplate>
                                                <asp:GridView ID="grdClass_TT" runat="server" AutoGenerateColumns="True" GridLines="Both"
                                                    HeaderStyle-BackColor="#0CA6CA" HeaderStyle-Font-Bold="true" HeaderStyle-HorizontalAlign="Center"
                                                    HeaderStyle-Font-Names="Book Antiqua" HeaderStyle-Font-Size="Medium" OnRowDataBound="grdClass_TT_OnRowDataBound"
                                                    Style="font-size: small;">
                                                </asp:GridView>
                                                <br />
                                                <asp:GridView ID="grdClassDet_TT" runat="server" AutoGenerateColumns="True" GridLines="Both"
                                                    HeaderStyle-BackColor="#0CA6CA" HeaderStyle-Font-Bold="true" HeaderStyle-HorizontalAlign="Left"
                                                    HeaderStyle-Font-Names="Book Antiqua" HeaderStyle-Font-Size="Medium" OnRowDataBound="grdClassDet_TT_OnRowDataBound"
                                                    Style="font-size: small;">
                                                </asp:GridView>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="View" Visible="false">
                                            <ItemTemplate>
                                                <asp:Button ID="btnViewTT" runat="server" Text="View" CssClass="textbox btn" Width="50px"
                                                    Height="30px" OnClick="btnViewTT_OnClick" BackColor="#F865A5" />
                                                <%-- <asp:CheckBox ID="chkSel" runat="server" AutoPostBack="true" OnCheckedChanged="chkSel_OnCheckedChanged" />--%>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Select">
                                            <ItemTemplate>
                                                <asp:Button ID="btnSaveTT" runat="server" Text="Select" CssClass="textbox btn" Width="50px"
                                                    Height="30px" OnClick="btnSaveTT_OnClick" BackColor="#76D7C4" />
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Center" />
                                        </asp:TemplateField>
                                    </Columns>
                                </asp:GridView>
                            </div>
                        </center>
                    </div>
                    <br />
                    <div id="divElective" runat="server" visible="false">
                        <div style="width: 100%; height: 25px; background-color: #0CA6CA; border-radius: 5px;
                            font-weight: bold; font-size: larger; text-align: left;">
                            <span style="padding-left: 50px;">Elective Selection</span>
                            <br />
                        </div>
                        <br />
                        <div>
                            <asp:GridView ID="gridElective" runat="server" AutoGenerateColumns="False" Visible="false"
                                GridLines="Both" HeaderStyle-BackColor="#0CA6CA" HeaderStyle-Font-Bold="true"
                                HeaderStyle-HorizontalAlign="Center" HeaderStyle-Font-Names="Book Antiqua" HeaderStyle-Font-Size="Medium"
                                OnDataBound="gridElective_OnDataBound">
                                <Columns>
                                    <asp:TemplateField HeaderText="S.No">
                                        <ItemTemplate>
                                            <asp:Label ID="lblSNo" runat="server" Text='<%#Container.DisplayIndex+1 %>'></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Center" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Select">
                                        <ItemTemplate>
                                            <asp:CheckBox ID="chkSel" runat="server" />
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Center" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Subject Type" Visible="false">
                                        <ItemTemplate>
                                            <asp:Label ID="lblSubTypeNo" runat="server" Text='<%#Eval("subType_no") %>' Visible="false"></asp:Label>
                                            <asp:Label ID="lblPP" runat="server" Text='<%#Eval("pp") %>' Visible="false"></asp:Label>
                                            <asp:Label ID="lblSubTypeName" runat="server" Text='<%#Eval("subject_type") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Subject Code">
                                        <ItemTemplate>
                                            <asp:Label ID="lblSubNo" runat="server" Text='<%#Eval("TT_subno") %>' Visible="false"></asp:Label>
                                            <asp:Label ID="lblSubCode" runat="server" Text='<%#Eval("subject_code") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Subject Name">
                                        <ItemTemplate>
                                            <asp:Label ID="lblSubName" runat="server" Text='<%#Eval("subject_name") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Staff Code">
                                        <ItemTemplate>
                                            <asp:Label ID="lblStaffCode" runat="server" Text='<%#Eval("TT_staffcode") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Staff Name">
                                        <ItemTemplate>
                                            <asp:Label ID="lblStaffName" runat="server" Text='<%#Eval("staff_name") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Remaining">
                                        <ItemTemplate>
                                            <asp:Label ID="lblRemain" runat="server" Text='<%#Eval("StudCount") %>'></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Center" />
                                    </asp:TemplateField>
                                </Columns>
                            </asp:GridView>
                            <br />
                            <asp:Button ID="btnSave" runat="server" Text="Save Elective" CssClass="textbox btn2"
                                Width="120px" Height="30px" Visible="false" OnClick="btnSaveElective_Click" BackColor="#76D7C4" />
                        </div>
                    </div>
                    <center>
                        <div id="divViewTT" runat="server" style="height: 350em; z-index: 1000; width: 100%;
                            background-color: rgba(54, 25, 25, .2); position: absolute; top: 0; left: 0px;"
                            visible="false">
                            <asp:ImageButton ID="imgViewTT" runat="server" Width="40px" Height="40px" OnClick="imgViewTT_OnClick"
                                ImageUrl="~/image/close.png" Style="height: 30px; width: 30px; position: absolute;
                                margin-top: 25px; margin-left: 430px;" />
                            <center>
                                <div id="Div2" runat="server" class="table" style="background-color: White; height: auto;
                                    width: 950px; border: 5px solid #0CA6CA; border-top: 25px solid #0CA6CA; margin-top: 20px;
                                    border-radius: 10px;">
                                    <center>
                                        <br />
                                        <span style="font-size: larger; color: Green; font-weight: bold;">Time Table</span>
                                        <div style="width: 950px; overflow: auto; height: 450px;">
                                            <asp:GridView ID="grdClass_TT" runat="server" AutoGenerateColumns="True" GridLines="Both"
                                                HeaderStyle-BackColor="#0CA6CA" HeaderStyle-Font-Bold="true" HeaderStyle-HorizontalAlign="Center"
                                                HeaderStyle-Font-Names="Book Antiqua" HeaderStyle-Font-Size="Medium" OnRowDataBound="grdClass_TT_OnRowDataBound"
                                                Style="font-size: small;">
                                            </asp:GridView>
                                        </div>
                                        <br />
                                        <asp:GridView ID="grdClassDet_TT" runat="server" AutoGenerateColumns="True" GridLines="Both"
                                            HeaderStyle-BackColor="#0CA6CA" HeaderStyle-Font-Bold="true" HeaderStyle-HorizontalAlign="Left"
                                            HeaderStyle-Font-Names="Book Antiqua" HeaderStyle-Font-Size="Medium" OnRowDataBound="grdClassDet_TT_OnRowDataBound"
                                            Style="font-size: small;">
                                        </asp:GridView>
                                    </center>
                                </div>
                            </center>
                        </div>
                    </center>
                </contenttemplate>
            </asp:UpdatePanel>
        </div>
    </center>
</asp:Content>

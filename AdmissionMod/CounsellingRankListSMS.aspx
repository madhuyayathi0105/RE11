<%@ Page Title="" Language="C#" MasterPageFile="~/AdmissionMod/AttendanceSubSiteMaster.master"
    AutoEventWireup="true" CodeFile="CounsellingRankListSMS.aspx.cs" Inherits="AdmissionMod_CounsellingRankListSMS" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">
    <script type="text/javascript">
        function checkNextVal(id) {
            var row = id.parentNode.parentNode;
            var indx = row.rowIndex - 1;
            var indxVal = parseInt(indx);

            var id = document.getElementById("<%=gridDaySlots.ClientID %>");
            var len = id.rows.length;

            for (var sl = (indxVal + 1); sl < len; sl++) {
                var fromCnt = document.getElementById('MainContent_gridDaySlots_txtFrom_' + sl.toString());
                var toCnt = document.getElementById('MainContent_gridDaySlots_txtTo_' + sl.toString());
                fromCnt.value = "";
                toCnt.value = "";
            }

        }

        function checkPrevTextVal(id) {
            var row = id.parentNode.parentNode;
            var indx = row.rowIndex - 1;
            var indxVal = parseInt(indx);

            if (id.value != "") {
                var idVal = parseInt(id.value);
                if (indxVal > 0) {
                    var prevTxt = document.getElementById('MainContent_gridDaySlots_txtTo_' + (indxVal - 1).toString()).value;
                    if (prevTxt != "") {
                        var prevVal = parseInt(prevTxt);
                        if (idVal != (prevVal + 1)) {
                            id.value = (prevVal + 1).toString();
                        }
                    } else {
                        id.value = "";
                        document.getElementById('MainContent_gridDaySlots_txtTo_' + (indxVal).toString()).value = "";
                    }

                }
            } else {
                document.getElementById('MainContent_gridDaySlots_txtTo_' + (indxVal).toString()).value = "";
            }

            var id = document.getElementById("<%=gridDaySlots.ClientID %>");
            var len = id.rows.length;

            for (var sl = (indxVal + 1); sl < len; sl++) {
                var fromCnt = document.getElementById('MainContent_gridDaySlots_txtFrom_' + sl.toString());
                var toCnt = document.getElementById('MainContent_gridDaySlots_txtTo_' + sl.toString());
                fromCnt.value = "";
                toCnt.value = "";
            }
        }

        function OnSaveRankListCheck() {
            var id = document.getElementById("<%=gridDaySlots.ClientID %>");
            var len = id.rows.length;

            var indx = 0;
            for (var sl = 1; sl < len; sl++, indx++) {
                var fromCnt = document.getElementById('MainContent_gridDaySlots_txtFrom_' + indx.toString());
                var toCnt = document.getElementById('MainContent_gridDaySlots_txtTo_' + indx.toString());

                if (fromCnt.value.trim() == "" && toCnt.value.trim() == "") {
                }
                else if (fromCnt.value.trim() == "" || toCnt.value.trim() == "") {
                    alert("Please check inputs");
                    return false;
                } else {
                    var fromVal = parseInt(fromCnt.value);
                    var toVal = parseInt(toCnt.value);
                    if (toVal < fromVal) {
                        alert("Please check inputs");
                        return false;
                    }
                }
            }
            return true;
        }

        function OnSaveDaySlotCheck() {

            var id = document.getElementById("<%=gridDaySlots.ClientID %>");
            var len = id.rows.length;

            var indx = 0;
            for (var sl = 1; sl < len; sl++, indx++) {
                var ischecked = document.getElementById('MainContent_gridDaySlots_chkSel_' + indx.toString()).checked;

                if (ischecked == true) {
                    return true;
                }
            }
            alert("Please select any slot");
            return false;
        }

        var checkedId = false;
        function OnGridHeaderSelected() {
            var id = document.getElementById("<%=gridDaySlots.ClientID %>");
            var len = id.rows.length;
            var i = 0;
            var checkedId = id.rows[0].getElementsByTagName("input")[0].checked;
            for (var ak = 1; ak < len; ak++) {
                if (id.rows[ak].getElementsByTagName("input")[i].type == "checkbox") {
                    if (checkedId == true) {
                        id.rows[ak].getElementsByTagName("input")[i].checked = true;
                    } else {
                        id.rows[ak].getElementsByTagName("input")[i].checked = false;
                    }
                }
            }
        }
    </script>
    <center>
        <div class="maindivstyle" style="width: 950px;">
            <center>
                <span class="fontstyleheader" style="color: Green;">Counselling Student's SMS Alert
                </span>
            </center>
            <asp:ScriptManager ID="scptMgrNew" runat="server">
            </asp:ScriptManager>
            <table class="maintablestyle">
                <tr>
                    <td>
                        <asp:Label ID="lblCollege" runat="server" Text="Institute"></asp:Label>
                    </td>
                    <td>
                        <asp:DropDownList ID="ddlCollege" runat="server" CssClass="textbox ddlheight3" AutoPostBack="true"
                            OnSelectedIndexChanged="ddlCollege_SelectedIndexChanged">
                        </asp:DropDownList>
                    </td>
                    <td>
                        <asp:Label ID="lblBatch" runat="server" Text="Batch"></asp:Label>
                    </td>
                    <td>
                        <asp:DropDownList ID="ddlbatch" runat="server" CssClass="textbox ddlheight" AutoPostBack="true"
                            OnSelectedIndexChanged="ddlbatch_SelectedIndexChanged">
                        </asp:DropDownList>
                    </td>
                    <td>
                        <asp:Label ID="lblEdulevel" runat="server" Text="Edu Level"></asp:Label>
                    </td>
                    <td>
                        <asp:DropDownList ID="ddlEduLev" runat="server" CssClass="textbox ddlheight" AutoPostBack="true"
                            OnSelectedIndexChanged="ddlEdulevel_SelectedIndexChanged">
                        </asp:DropDownList>
                    </td>
                    <td>
                        <asp:Label ID="lblCourse" runat="server" Text="Course"></asp:Label>
                    </td>
                    <td>
                        <asp:DropDownList ID="ddlcourse" runat="server" CssClass="textbox ddlheight" AutoPostBack="true"
                            OnSelectedIndexChanged="ddlcourse_OnSelectedIndexChanged">
                        </asp:DropDownList>
                    </td>
                    <td>
                        <asp:Label ID="lblStream" runat="server" Text="Stream"></asp:Label>
                    </td>
                    <td>
                        <asp:DropDownList ID="ddlStream" runat="server" CssClass="textbox ddlheight2" AutoPostBack="true"
                            OnSelectedIndexChanged="ddlStream_OnSelectedIndexChanged">
                        </asp:DropDownList>
                    </td>
                    <td>
                        <asp:Button ID="btnBaseGo" runat="server" Text="Go" CssClass="textbox  btn2" Width="40px"
                            OnClick="btnBaseGo_Click" />
                    </td>
                    <td>
                        <asp:Button ID="btnsmssend" runat="server" Text="Send SMS" CssClass="textbox  btn2"
                            Width="100px" OnClick="btnsmssend_Click" />
                    </td>
                </tr>
            </table>
            <br />
            <asp:GridView ID="gridDaySlots" runat="server" AutoGenerateColumns="false" HeaderStyle-BackColor="#0CA6CA"
                Width="500px" Visible="false">
                <Columns>
                    <asp:TemplateField HeaderText="S.No">
                        <ItemTemplate>
                            <asp:Label ID="lblSno" runat="server" Text='<%#Container.DisplayIndex+1 %>'></asp:Label>
                        </ItemTemplate>
                        <ItemStyle HorizontalAlign="Center" Width="60px" />
                    </asp:TemplateField>
                    <asp:TemplateField>
                        <HeaderTemplate>
                            <asp:CheckBox ID="cb_selectHead" runat="server" onchange="return OnGridHeaderSelected()">
                            </asp:CheckBox>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <asp:CheckBox ID="chkSel" runat="server" />
                        </ItemTemplate>
                        <ItemStyle HorizontalAlign="Center" Width="50px" />
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Date">
                        <ItemTemplate>
                            <asp:HiddenField ID="hdnDate" runat="server" Value='<%#Eval("SlotDate") %>' />
                            <asp:Label ID="lblDate" runat="server" Text='<%#Eval("date") %>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Slot Time">
                        <ItemTemplate>
                            <asp:HiddenField ID="hdnSlotPk" runat="server" Value='<%#Eval("SlotTime") %>' />
                            <asp:Label ID="lblSlotVal" runat="server" Text='<%#Eval("SlotTime") %>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Category">
                        <ItemTemplate>
                            <asp:Label ID="lblcriteria" runat="server" Text='<%#Eval("criteriavalue") %>'></asp:Label>
                            <asp:HiddenField ID="hdnCategory" runat="server" Value='<%#Eval("criteriaCode") %>' />
                        </ItemTemplate>
                        <ItemStyle HorizontalAlign="Center" />
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Count">
                        <ItemTemplate>
                            <asp:Label ID="lblcount" runat="server" Text='<%#Eval("count") %>'></asp:Label>
                        </ItemTemplate>
                        <ItemStyle HorizontalAlign="Center" />
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
        </div>
    </center>
    <div id="imgdiv2" runat="server" visible="false" style="height: 300em; z-index: 1000;
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

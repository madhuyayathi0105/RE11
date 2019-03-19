<%@ Page Title="" Language="C#" MasterPageFile="~/AttendanceMOD/AttendanceSubSiteMaster.master"
    AutoEventWireup="true" CodeFile="AutoTimeTableGeneration.aspx.cs" Inherits="AttendanceMOD_AutoTimeTableGeneration" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">
    <script type="text/javascript">
        function printTTOutput() {
            var panel = document.getElementById("<%=printDivFnl.ClientID %>");
            var printWindow = window.open('', '', 'height=816,width=980');
            printWindow.document.write('<html><head>');
            printWindow.document.write('</head><body >');
            printWindow.document.write(panel.innerHTML);
            printWindow.document.write('</body></html>');
            printWindow.document.close();
            setTimeout(function () {
                printWindow.print();
            }, 500);
            return false;
        }

        var checkedId = false;
        function OnGridHeaderSelected() {
            var id = document.getElementById("<%=gridDetails.ClientID %>");
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
        function OnGenerateSelectCheck() {
            var id = document.getElementById("<%=gridDetails.ClientID %>");
            var len = id.rows.length;
            var i = 0;
            for (var ak = 1; ak < len; ak++) {
                if (id.rows[ak].getElementsByTagName("input")[i].type == "checkbox") {
                    var checkedId = id.rows[ak].getElementsByTagName("input")[i].checked;
                    if (checkedId == true) {
                        return true;
                    }
                }
            }
            alert('Please Select Atleast One Branch');
            return false;
        }

        function CheckforSelectedTT() {
            var id = document.getElementById("<%=ddlSelectedTimeTable.ClientID %>");

            if (id.value == "") {
                return true;
            }
            else {
                return true;
            }
        }
    </script>
    <center>
        <div class="maindivstyle" style="width: 950px;">
            <center>
                <span class="fontstyleheader" style="color: Green;">Time Table Generation</span>
            </center>
            <asp:ScriptManager ID="scptMgrNew" runat="server">
            </asp:ScriptManager>
            <table class="maintablestyle" style="text-align: right;">
                <tr>
                    <td>
                        <asp:Label ID="lblCollege" runat="server" Text="Institute"></asp:Label>
                    </td>
                    <td>
                        <asp:DropDownList ID="ddlCollege" runat="server" CssClass="textbox ddlheight5" AutoPostBack="true"
                            OnSelectedIndexChanged="ddlCollege_IndexChanged">
                        </asp:DropDownList>
                    </td>
                    <td>
                        <asp:Label ID="lblBatch" runat="server" Text="Batch"></asp:Label>
                    </td>
                    <td>
                        <asp:UpdatePanel ID="updBatch" runat="server">
                            <ContentTemplate>
                                <asp:TextBox ID="txtBatch" runat="server" CssClass="textbox textbox1" Width="80px"></asp:TextBox>
                                <asp:Panel ID="pnlBatch" runat="server" CssClass="multxtpanel">
                                    <asp:CheckBox ID="cbBatch" runat="server" Text="Select All" AutoPostBack="true" OnCheckedChanged="chkBatch_OnCheckedChanged" />
                                    <asp:CheckBoxList ID="cblBatch" runat="server" AutoPostBack="true" OnSelectedIndexChanged="cblBatch_OnSelectedIndexChanged">
                                    </asp:CheckBoxList>
                                </asp:Panel>
                                <asp:PopupControlExtender ID="pceBatch" runat="server" TargetControlID="txtBatch"
                                    PopupControlID="pnlBatch" Position="Bottom">
                                </asp:PopupControlExtender>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </td>
                    <td>
                        <asp:Label ID="lblEdulevel" runat="server" Text="Education Level"></asp:Label>
                    </td>
                    <td>
                        <asp:DropDownList ID="ddlEduLev" runat="server" CssClass="textbox ddlheight">
                        </asp:DropDownList>
                    </td>
                    <td>
                        <asp:Label ID="lblCriteria" runat="server" Text="Option"></asp:Label>
                    </td>
                    <td>
                        <asp:DropDownList ID="ddlCriteria" runat="server" CssClass="textbox ddlheight" AutoPostBack="True"
                            OnSelectedIndexChanged="ddlCriteria_NewOnIndexChanged">
                        </asp:DropDownList>
                    </td>
                    <td>
                        <asp:Button ID="btnGo" runat="server" CssClass="textbox btn" Text="Go" OnClick="btnGo_Click" />
                        <asp:Button ID="btnGenOption" runat="server" CssClass="textbox btn" Width="100px"
                            Visible="false" OnClientClick="return OnGenerateSelectCheck() " Text="Show Option"
                            OnClick="generateOptions" />
                    </td>
                </tr>
                <tr>
                    <td colspan="6">
                        <asp:CheckBox ID="chkMinElect" runat="server" Text="Minimum Elective" />
                        <asp:CheckBox ID="chkShowPart" runat="server" Text="Show Partial" />
                    </td>
                    <td>
                        <asp:Button ID="btnStaffAllot" runat="server" Visible="false" CssClass="textbox btn"
                            Text="Allot Staff" OnClick="btnStaffAllot_Click" Width="80px" />
                    </td>
                    <td>
                        <asp:Button ID="btnGenerate" runat="server" CssClass="textbox btn" Width="80px" Text="Generate"
                            OnClick="btnGenerate_Click" Visible="false" OnClientClick="return OnGenerateSelectCheck() " />
                    </td>
                    <td>
                        <asp:Button ID="btnClear" runat="server" CssClass="textbox btn" Text="Clear" OnClick="btnClear_Click"
                            Width="60px" />
                        <asp:Button ID="btnImport" runat="server" CssClass="textbox btn" Text="Import" OnClick="btnImport_Click"
                            Width="80px" />
                    </td>
                </tr>
            </table>
            <br />
            <center>
                <div>
                    <asp:Label ID="lblRecNotFound" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                        Font-Size="Medium" ForeColor="Red" Text="No Records Found" Visible="false"></asp:Label>
                    <asp:GridView ID="gridDetails" runat="server" AutoGenerateColumns="false" GridLines="Both"
                        Visible="false" OnRowDataBound="gridDetails_OnRowDataBound" OnDataBound="gridDetails_DataBound">
                        <Columns>
                            <asp:TemplateField HeaderText="S.No" HeaderStyle-BackColor="#0CA6CA" HeaderStyle-HorizontalAlign="Center">
                                <ItemTemplate>
                                    <asp:Label ID="lbl_serial" runat="server" Text='<%#Container.DisplayIndex+1 %>'></asp:Label>
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="center" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Batch" HeaderStyle-BackColor="#0CA6CA" HeaderStyle-HorizontalAlign="Center">
                                <ItemTemplate>
                                    <asp:Label ID="lbl_Batch" runat="server" Text='<%#Eval("Batch") %>'></asp:Label>
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="center" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Degree" HeaderStyle-BackColor="#0CA6CA" HeaderStyle-HorizontalAlign="Center">
                                <ItemTemplate>
                                    <asp:Label ID="lbl_Degree" runat="server" Text='<%#Eval("Degree") %>'></asp:Label>
                                    <asp:Label ID="lbl_DegreeCode" runat="server" Text='<%#Eval("DegreeCode") %>' Visible="false"></asp:Label>
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Left" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Branch" HeaderStyle-BackColor="#0CA6CA" HeaderStyle-HorizontalAlign="Center">
                                <ItemTemplate>
                                    <asp:Label ID="lbl_Branch" runat="server" Text='<%#Eval("Branch") %>'></asp:Label>
                                    <asp:Label ID="lbl_BranchCode" runat="server" Text='<%#Eval("BranchCode") %>' Visible="false"></asp:Label>
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Left" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderStyle-BackColor="#0CA6CA" HeaderStyle-HorizontalAlign="Center">
                                <%--  <HeaderTemplate>
                                    <asp:CheckBox ID="cb_selectHead" runat="server" onchange="return OnGridHeaderSelected()">
                                    </asp:CheckBox>
                                </HeaderTemplate>--%>
                                <ItemTemplate>
                                    <asp:CheckBox ID="cb_select" runat="server" AutoPostBack="true" OnCheckedChanged="cb_select_CheckedChanged">
                                    </asp:CheckBox>
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="center" />
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                    <asp:GridView ID="gridUserOptions" runat="server" AutoGenerateColumns="false" GridLines="Both"
                        Visible="false" OnRowDataBound="gridUserOptions_OnRowDataBound" OnDataBound="gridUserOptions_DataBound">
                        <Columns>
                            <asp:TemplateField HeaderText="S.No" HeaderStyle-BackColor="#0CA6CA" HeaderStyle-HorizontalAlign="Center">
                                <ItemTemplate>
                                    <asp:Label ID="lbl_serial" runat="server" Text='<%#Container.DisplayIndex+1 %>'></asp:Label>
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="center" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Type" HeaderStyle-BackColor="#0CA6CA" HeaderStyle-HorizontalAlign="Center">
                                <ItemTemplate>
                                    <asp:Label ID="lblSubType" runat="server" Text='<%#Eval("subject_type")%>'></asp:Label>
                                    <asp:Label ID="lblSubTypeNo" runat="server" Text='<%#Eval("subType_no")%>' Visible="false"></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Subject" HeaderStyle-BackColor="#0CA6CA" HeaderStyle-HorizontalAlign="Center">
                                <ItemTemplate>
                                    <asp:Label ID="lblSubName" runat="server" Text='<%#Eval("subject_name")%>'></asp:Label>
                                    <asp:Label ID="lblSubCode" runat="server" Text='<%#Eval("subject_code")%>'></asp:Label>
                                    <asp:Label ID="lblSubNo" runat="server" Text='<%#Eval("subject_no")%>' Visible="false"></asp:Label>
                                    <asp:Label ID="lblIsLab" runat="server" Text='<%#Eval("Lab")%>' Visible="false"></asp:Label>
                                    <asp:Label ID="lblIsElective" runat="server" Text='<%#Eval("ElectivePap")%>' Visible="false"></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Faculty" HeaderStyle-BackColor="#0CA6CA" HeaderStyle-HorizontalAlign="Center">
                                <ItemTemplate>
                                    <asp:DropDownList ID="ddlFaculty" runat="server" CssClass="textbox ddlheight1" Width="120px"
                                        AutoPostBack="true" OnSelectedIndexChanged="ddlFaculty_OnSelectedIndexChanged">
                                    </asp:DropDownList>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Monday" HeaderStyle-BackColor="#0CA6CA" HeaderStyle-HorizontalAlign="Center">
                                <ItemTemplate>
                                    <asp:DropDownList ID="ddlMondayHour" runat="server" CssClass="textbox ddlheight1"
                                        Width="50px">
                                    </asp:DropDownList>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Tuesday" HeaderStyle-BackColor="#0CA6CA" HeaderStyle-HorizontalAlign="Center">
                                <ItemTemplate>
                                    <asp:DropDownList ID="ddlTuesdayHour" runat="server" CssClass="textbox ddlheight1"
                                        Width="50px">
                                    </asp:DropDownList>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Wednesday" HeaderStyle-BackColor="#0CA6CA" HeaderStyle-HorizontalAlign="Center">
                                <ItemTemplate>
                                    <asp:DropDownList ID="ddlWednesdayHour" runat="server" CssClass="textbox ddlheight1"
                                        Width="50px">
                                    </asp:DropDownList>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Thursday" HeaderStyle-BackColor="#0CA6CA" HeaderStyle-HorizontalAlign="Center">
                                <ItemTemplate>
                                    <asp:DropDownList ID="ddlThursdayHour" runat="server" CssClass="textbox ddlheight1"
                                        Width="50px">
                                    </asp:DropDownList>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Friday" HeaderStyle-BackColor="#0CA6CA" HeaderStyle-HorizontalAlign="Center">
                                <ItemTemplate>
                                    <asp:DropDownList ID="ddlFridayHour" runat="server" CssClass="textbox ddlheight1"
                                        Width="50px">
                                    </asp:DropDownList>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                </div>
            </center>
        </div>
    </center>
    <center>
        <div id="divAllotStaff" runat="server" visible="false" class="popupstyle popupheight1 "
            style="height: 300em;">
            <asp:ImageButton ID="imgBtn" runat="server" OnClick="closedivAllotStaff" Width="40px"
                Height="40px" ImageUrl="~/images/close.png" Style="height: 30px; width: 30px;
                position: absolute; margin-top: 35px; margin-left: 380px;" />
            <br />
            <br />
            <center>
                <div style="width: 800px; height: 500px; overflow: auto; background-color: White;
                    border: 1px solid #0CA6CA; border-top: 10px solid #0CA6CA; border-radius: 10px;">
                    <center>
                        <span class="fontstyleheader" style="color: Green;">Staff Manual Allotment</span>
                    </center>
                    <table>
                        <tr>
                            <td>
                                Staff
                            </td>
                            <td>
                                <asp:DropDownList ID="ddlStaff" runat="server" CssClass=" textbox ddlheight2">
                                </asp:DropDownList>
                            </td>
                            <td>
                                Batch
                            </td>
                            <td>
                                <asp:DropDownList ID="ddlBatchStaffAllot" runat="server" CssClass=" textbox ddlheight">
                                </asp:DropDownList>
                            </td>
                            <td>
                                Degree
                            </td>
                            <td>
                                <asp:DropDownList ID="ddlDegreeStaffAllot" runat="server" CssClass=" textbox ddlheight">
                                </asp:DropDownList>
                            </td>
                            <td>
                                Branch
                            </td>
                            <td>
                                <asp:DropDownList ID="ddlBranchStaffAllot" runat="server" CssClass=" textbox ddlheight2">
                                </asp:DropDownList>
                            </td>
                            <td>
                                Sem
                            </td>
                            <td>
                                <asp:DropDownList ID="ddlSemStaffAllot" runat="server" CssClass=" textbox ddlheight"
                                    Width="40px">
                                </asp:DropDownList>
                            </td>
                        </tr>
                    </table>
                </div>
            </center>
        </div>
    </center>
    <center>
        <div id="divImport" runat="server" visible="false" class="popupstyle popupheight1 "
            style="height: 300em;">
            <asp:ImageButton ID="imgbtnImport" runat="server" OnClick="closeTTImport" Width="40px"
                Height="40px" ImageUrl="~/images/close.png" Style="height: 30px; width: 30px;
                position: absolute; margin-top: 40px; margin-left: 230px;" />
            <br />
            <br />
            <center>
                <div style="width: 500px; height: 250px; overflow: auto; background-color: White;
                    border: 1px solid #0CA6CA; border-top: 10px solid #0CA6CA; border-radius: 10px;">
                    <center>
                        <center>
                            <span class="fontstyleheader" style="color: Green;">Import</span>
                        </center>
                        <br />
                        <table>
                            <tr>
                                <td colspan="3">
                                    <asp:RadioButtonList ID="rblImportType" runat="server" RepeatDirection="Horizontal">
                                        <asp:ListItem Selected="True" Value="0">Staff Allotment</asp:ListItem>
                                        <asp:ListItem Value="1">Time Table Criteria</asp:ListItem>
                                    </asp:RadioButtonList>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    Browse
                                </td>
                                <td>
                                    <asp:FileUpload ID="fuImport" runat="server" ToolTip="Choose Excel File Only" CssClass="textbox ddlheight"
                                        Height="30px" Width="200px" />
                                </td>
                                <td>
                                    <asp:Button ID="btnImportExcel" runat="server" Text="Import Excel" CssClass="textbox btn"
                                        Width="120px" OnClick="btnImportExcel_Click" />
                                </td>
                            </tr>
                        </table>
                    </center>
                </div>
            </center>
        </div>
    </center>
    <center>
        <div id="divTimeTableOutput" runat="server" visible="false" class="popupstyle popupheight1 "
            style="height: 100%;">
            <button onclick="return printTTOutput();" style="position: absolute; margin-top: 35px;
                margin-left: 400px; border-radius: 35px;">
                <img src="../images/PrintImg.ico" width="25px" height="25px" />
            </button>
            <%--  <asp:ImageButton ID="imgPrintTimeTable" runat="server" OnClientClick="printTTOutput()" Width="40px"
                Height="40px" ImageUrl="~/images/PrintImg.ico" Style="height: 30px; width: 30px;
                position: absolute; margin-top: 35px; margin-left: 420px;" />--%>
            <asp:ImageButton ID="imgCloseTTOutput" runat="server" OnClick="closeTTOutput" Width="40px"
                Height="40px" ImageUrl="~/images/close.png" Style="height: 30px; width: 30px;
                position: absolute; margin-top: 35px; margin-left: 460px;" />
            <br />
            <br />
            <center>
                <div id="printDiv" runat="server" style="width: 950px; height: 600px; overflow: auto;
                    background-color: White; border: 1px solid #0CA6CA; border-top: 10px solid #0CA6CA;
                    border-radius: 10px;">
                    <center>
                        <span class="fontstyleheader" style="color: Green;">Time Table</span>
                    </center>
                    <br />
                    <center>
                        <table id="tblHeaderNextTT" runat="server" visible="false">
                            <tr>
                                <td>
                                    Next Option
                                </td>
                                <td>
                                    <asp:DropDownList ID="ddlCriteriaReduced" runat="server" CssClass="textbox ddlheight">
                                    </asp:DropDownList>
                                </td>
                                <td>
                                    Selected Time Table
                                </td>
                                <td>
                                    <asp:DropDownList ID="ddlSelectedTimeTable" runat="server" CssClass="textbox ddlheight5">
                                    </asp:DropDownList>
                                </td>
                                <td>
                                    <asp:Button ID="btnGenerateNext" runat="server" CssClass="textbox btn" Width="80px"
                                        Text="Generate" OnClick="btnGenerateNext_Click" OnClientClick="return CheckforSelectedTT();" />
                                    <asp:Button ID="btnShowSavedTables" runat="server" CssClass="textbox btn" Width="120px"
                                        Text="Show Saved" OnClick="btnShowSavedTables_Click" />
                                    <asp:Button ID="btnRoomTables" runat="server" CssClass="textbox btn" Width="120px"
                                        Text="Show Room" OnClick="btnRoomTables_Click" />
                                </td>
                            </tr>
                            <tr>
                                <td colspan="5">
                                    Staff
                                    <asp:DropDownList ID="ddlStaffTT" runat="server" CssClass=" textbox ddlheight2" Width="250px">
                                    </asp:DropDownList>
                                    <asp:Button ID="btnStaffTimeTable" runat="server" CssClass="textbox btn" Width="160px"
                                        Text="Show Time Table" OnClick="btnStaffTimeTable_Click" />
                                    <asp:CheckBox ID="chkShowPart2" runat="server" Text="Show Partial" />
                                </td>
                            </tr>
                        </table>
                        <div runat="server" id="printDivFnl" style="height: 510px; overflow: auto;">
                            <asp:PlaceHolder ID="phTimeTable" runat="server"></asp:PlaceHolder>
                        </div>
                    </center>
                </div>
            </center>
        </div>
    </center>
    <center>
        <div id="popuperr" runat="server" visible="false" style="height: 100%; z-index: 2000;
            width: 100%; background-color: rgba(54, 25, 25, .2); position: absolute; top: 0;
            left: 0px; height: 100em;">
            <center>
                <div id="pnl2" runat="server" class="table" style="background-color: White; height: 120px;
                    width: 238px; border: 5px solid #0CA6CA; border-top: 25px solid #0CA6CA; margin-top: 200px;
                    border-radius: 10px;">
                    <center>
                        <br />
                        <table style="height: 100px; width: 100%">
                            <tr>
                                <td align="center">
                                    <asp:Label ID="lbl_popuperr" Visible="true" runat="server" Text="" Style="color: Red;"
                                        Font-Bold="true" Font-Size="Medium"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <center>
                                        <asp:Button ID="btnPopErrclose" CssClass=" textbox btn1 comm" Style="height: 28px;
                                            width: 65px;" OnClick="btnPopErrclose_Click" Text="OK" runat="server" />
                                    </center>
                                </td>
                            </tr>
                        </table>
                    </center>
                </div>
            </center>
        </div>
    </center>
    <center>
        <div id="divNotInserted" runat="server" visible="false" style="height: 100%; z-index: 1000;
            width: 100%; background-color: rgba(54, 25, 25, .2); position: absolute; top: 0;
            left: 0px; height: 100em;">
            <asp:ImageButton ID="imgbtnNotInserted" runat="server" Width="40px" Height="40px"
                ImageUrl="~/images/close.png" Style="height: 30px; width: 30px; position: absolute;
                margin-top: 61px; margin-left: 405px;" OnClick="imgbtnNotInserted_Click" />
            <center>
                <div id="divCannotInsert" runat="server" style="background-color: White; height: 400px;
                    width: 840px; border: 5px solid #0CA6CA; border-top: 5px solid #0CA6CA; margin-top: 72px;
                    border-radius: 10px;">
                    <asp:Label ID="lbl_upload_suc" runat="server" Visible="false" ForeColor="Blue"></asp:Label>
                    <br />
                    <asp:Label ID="lbl_cannotsave" Visible="true" runat="server" Style="color: Red;"
                        Font-Bold="true" Font-Size="Medium"></asp:Label>
                    <div style="height: 345px; width: 700px; overflow: auto;">
                        <asp:TextBox ID="txtNotInserted" TextMode="MultiLine" runat="server" Style="height: 334px;
                            overflow: auto;" Visible="true" Width="650px" ForeColor="Blue" ReadOnly="true"></asp:TextBox>
                    </div>
                </div>
            </center>
        </div>
    </center>
</asp:Content>

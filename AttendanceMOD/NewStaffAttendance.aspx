<%@ Page Title="" Language="C#" MasterPageFile="~/AttendanceMOD/AttendanceSubSiteMaster.master"
    AutoEventWireup="true" CodeFile="NewStaffAttendance.aspx.cs" Inherits="AttendanceMOD_NewStaffAttendance" %>

<%@ Register Assembly="FarPoint.Web.Spread,  Version=5.0.3520.2008, Culture=neutral, PublicKeyToken=327c3516b1b18457"
    Namespace="FarPoint.Web.Spread" TagPrefix="FarPoint" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">
    <asp:ScriptManager ID="scptMgr" runat="server">
    </asp:ScriptManager>
    <center>
        <span class="fontstyleheader" style="color: Green;">Attendance Entry</span>
        <div class="maindivstyle">
            <table class="maintablestyle">
                <tr>
                    <td>
                        From
                    </td>
                    <td>
                        <asp:TextBox ID="txtFromDate" runat="server" CssClass="textbox textbox1" Width="80px"
                            AutoPostBack="true" OnTextChanged="CheckDate"></asp:TextBox>
                        <asp:CalendarExtender ID="ceFromDate" runat="server" TargetControlID="txtFromDate"
                            Format="dd/MM/yyyy">
                        </asp:CalendarExtender>
                    </td>
                    <td>
                        To
                    </td>
                    <td>
                        <asp:TextBox ID="txtToDate" runat="server" CssClass="textbox textbox1" Width="80px"
                            AutoPostBack="true" OnTextChanged="CheckDate"></asp:TextBox>
                        <asp:CalendarExtender ID="ceToDate" runat="server" TargetControlID="txtToDate" Format="dd/MM/yyyy">
                        </asp:CalendarExtender>
                    </td>
                    <td>
                        <asp:Button ID="btnGo" runat="server" Text="Go" OnClick="btnGo_OnClick" CssClass=" textbox btn1" />
                    </td>
                </tr>
            </table>
            <br />
            <div id="divTimeTable" runat="server" visible="false">
                <asp:GridView ID="gridTimeTable" runat="server" AutoGenerateColumns="false" HeaderStyle-BackColor="#0CA6CA"
                    BackColor="White" OnDataBound="gridTimeTable_OnDataBound">
                    <Columns>
                        <asp:TemplateField HeaderText="Date">
                            <ItemTemplate>
                                <asp:Label ID="lblDateDisp" runat="server" Text='<%#Eval("DateDisp") %>'></asp:Label>
                                <asp:Label ID="lblDate" runat="server" Text='<%#Eval("DateVal") %>' Visible="false"></asp:Label>
                                <asp:Label ID="lblDayVal" runat="server" Text='<%#Eval("DayVal") %>' Visible="false"></asp:Label>
                                <asp:Label ID="lblElect" runat="server" Text='<%#Eval("Elective") %>' Visible="false"></asp:Label>
                                <asp:Label ID="lblLab" runat="server" Text='<%#Eval("Lab") %>' Visible="false"></asp:Label>
                            </ItemTemplate>
                            <ItemStyle Width="100px" HorizontalAlign="Center" BackColor="#F8B7B3" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Period 1">
                            <ItemTemplate>
                                <asp:LinkButton ID="lnkPeriod_1" runat="server" Text='<%#Eval("P1ValDisp") %>' ForeColor="Blue"
                                    OnClick="lnkAttMark"></asp:LinkButton>
                                <asp:Label ID="lblPeriod_1" runat="server" Text='<%#Eval("P1Val") %>' Visible="false"></asp:Label>
                                <asp:Label ID="lblTT_1" runat="server" Text='<%#Eval("TT_1") %>' Visible="false"></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Period 2">
                            <ItemTemplate>
                                <asp:LinkButton ID="lnkPeriod_2" runat="server" Text='<%#Eval("P2ValDisp") %>' ForeColor="Blue"
                                    OnClick="lnkAttMark"></asp:LinkButton>
                                <asp:Label ID="lblPeriod_2" runat="server" Text='<%#Eval("P2Val") %>' Visible="false"></asp:Label>
                                <asp:Label ID="lblTT_2" runat="server" Text='<%#Eval("TT_2") %>' Visible="false"></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Period 3">
                            <ItemTemplate>
                                <asp:LinkButton ID="lnkPeriod_3" runat="server" Text='<%#Eval("P3ValDisp") %>' ForeColor="Blue"
                                    OnClick="lnkAttMark"></asp:LinkButton>
                                <asp:Label ID="lblPeriod_3" runat="server" Text='<%#Eval("P3Val") %>' Visible="false"></asp:Label>
                                <asp:Label ID="lblTT_3" runat="server" Text='<%#Eval("TT_3") %>' Visible="false"></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Period 4">
                            <ItemTemplate>
                                <asp:LinkButton ID="lnkPeriod_4" runat="server" Text='<%#Eval("P4ValDisp") %>' ForeColor="Blue"
                                    OnClick="lnkAttMark"></asp:LinkButton>
                                <asp:Label ID="lblPeriod_4" runat="server" Text='<%#Eval("P4Val") %>' Visible="false"></asp:Label>
                                <asp:Label ID="lblTT_4" runat="server" Text='<%#Eval("TT_4") %>' Visible="false"></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Period 5">
                            <ItemTemplate>
                                <asp:LinkButton ID="lnkPeriod_5" runat="server" Text='<%#Eval("P5ValDisp") %>' ForeColor="Blue"
                                    OnClick="lnkAttMark"></asp:LinkButton>
                                <asp:Label ID="lblPeriod_5" runat="server" Text='<%#Eval("P5Val") %>' Visible="false"></asp:Label>
                                <asp:Label ID="lblTT_5" runat="server" Text='<%#Eval("TT_5") %>' Visible="false"></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Period 6">
                            <ItemTemplate>
                                <asp:LinkButton ID="lnkPeriod_6" runat="server" Text='<%#Eval("P6ValDisp") %>' ForeColor="Blue"
                                    OnClick="lnkAttMark"></asp:LinkButton>
                                <asp:Label ID="lblPeriod_6" runat="server" Text='<%#Eval("P6Val") %>' Visible="false"></asp:Label>
                                <asp:Label ID="lblTT_6" runat="server" Text='<%#Eval("TT_6") %>' Visible="false"></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Period 7">
                            <ItemTemplate>
                                <asp:LinkButton ID="lnkPeriod_7" runat="server" Text='<%#Eval("P7ValDisp") %>' ForeColor="Blue"
                                    OnClick="lnkAttMark"></asp:LinkButton>
                                <asp:Label ID="lblPeriod_7" runat="server" Text='<%#Eval("P7Val") %>' Visible="false"></asp:Label>
                                <asp:Label ID="lblTT_7" runat="server" Text='<%#Eval("TT_7") %>' Visible="false"></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Period 8">
                            <ItemTemplate>
                                <asp:LinkButton ID="lnkPeriod_8" runat="server" Text='<%#Eval("P8ValDisp") %>' ForeColor="Blue"
                                    OnClick="lnkAttMark"></asp:LinkButton>
                                <asp:Label ID="lblPeriod_8" runat="server" Text='<%#Eval("P8Val") %>' Visible="false"></asp:Label>
                                <asp:Label ID="lblTT_8" runat="server" Text='<%#Eval("TT_8") %>' Visible="false"></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Period 9">
                            <ItemTemplate>
                                <asp:LinkButton ID="lnkPeriod_9" runat="server" Text='<%#Eval("P9ValDisp") %>' ForeColor="Blue"
                                    OnClick="lnkAttMark"></asp:LinkButton>
                                <asp:Label ID="lblPeriod_9" runat="server" Text='<%#Eval("P9Val") %>' Visible="false"></asp:Label>
                                <asp:Label ID="lblTT_9" runat="server" Text='<%#Eval("TT_9") %>' Visible="false"></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Period 10">
                            <ItemTemplate>
                                <asp:LinkButton ID="lnkPeriod_10" runat="server" Text='<%#Eval("P10ValDisp") %>'
                                    ForeColor="Blue" OnClick="lnkAttMark"></asp:LinkButton>
                                <asp:Label ID="lblPeriod_10" runat="server" Text='<%#Eval("P10Val") %>' Visible="false"></asp:Label>
                                <asp:Label ID="lblTT_10" runat="server" Text='<%#Eval("TT_10") %>' Visible="false"></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
            </div>
            <br />
            <center>
                <div id="markDiv" runat="server" style="width: 950px; height: auto; overflow: auto;
                    background-color: White; border: 1px solid #0CA6CA; border-top: 10px solid #0CA6CA;
                    border-radius: 10px;" visible="false">
                    <asp:ImageButton ID="imgCloseAttMark" runat="server" OnClick="closeAttMark" Width="40px"
                        Height="40px" ImageUrl="~/images/close.png" Style="height: 30px; width: 30px;
                        position: absolute; margin-top: -10px; margin-left: 450px;" />
                    <center>
                        <span class="fontstyleheader" style="color: Green;">Mark Attendance</span>
                    </center>
                    <table class="maintablestyle" width="900px" style="font-weight: bold;">
                        <tr>
                            <td style="display: none;">
                                Batch
                            </td>
                            <td style="display: none;">
                                :
                                <asp:Label ID="lblBatch" runat="server">
                                </asp:Label>
                            </td>
                            <td style="display: none;">
                                Course
                            </td>
                            <td style="display: none;">
                                :
                                <asp:Label ID="lblCourseDisp" runat="server">
                                </asp:Label>
                                <asp:Label ID="lblDegCode" runat="server" Visible="false">
                                </asp:Label>
                            </td>
                            <td>
                                Subject
                            </td>
                            <td>
                                :
                                <asp:Label ID="lblSubname" runat="server">
                                </asp:Label>
                                <asp:Label ID="lblSubno" runat="server" Visible="false">
                                </asp:Label>
                                <asp:Label ID="lblSubCode" runat="server" Visible="false">
                                </asp:Label>
                            </td>
                            <td>
                                Date
                            </td>
                            <td>
                                :
                                <asp:Label ID="lblDate" runat="server">
                                </asp:Label>
                                <asp:Label ID="lblDayFK" runat="server" Visible="false">
                                </asp:Label>
                                <asp:Label ID="lblHourFk" runat="server" Visible="false">
                                </asp:Label>
                                <br />
                            </td>
                        </tr>
                        <tr>
                            <td style="display: none;">
                                Semester
                            </td>
                            <td style="display: none;">
                                :
                                <asp:Label ID="lblSem" runat="server">
                                </asp:Label>
                            </td>
                            <td style="display: none;">
                                Section
                            </td>
                            <td style="display: none;">
                                :
                                <asp:Label ID="lblSec" runat="server">
                                </asp:Label>
                            </td>
                            <%-- <td>
                                Date
                            </td>
                            <td>
                                :
                                <asp:Label ID="lblDate" runat="server">
                                </asp:Label>
                                <asp:Label ID="lblDayFK" runat="server" Visible="false">
                                </asp:Label>
                                <asp:Label ID="lblHourFk" runat="server" Visible="false">
                                </asp:Label>
                            </td>--%>
                        </tr>
                    </table>
                    <table>
                        <tr>
                            <td>
                                <div id="divMultiSubj" runat="server" visible="false" style="float: left;">
                                    <asp:Label ID="lblSub" runat="server" Text="Subject" Font-Bold="True" Font-Names="Book Antiqua"
                                        Font-Size="Medium"></asp:Label>
                                    <asp:DropDownList ID="ddlMultiSub" runat="server" CssClass="textbox ddlheight5" AutoPostBack="true"
                                        OnSelectedIndexChanged="ddlMultiSub_OnChanged">
                                    </asp:DropDownList>
                                </div>
                                <div style="float: left;">
                                    <asp:CheckBox ID="chkAbsEntry" runat="server" Text="Absentees Entry" AutoPostBack="true"
                                        OnCheckedChanged="chkAbsEntry_OnCheckChanged" Style="float: left; padding-top: 5px;"
                                        Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium" />
                                    <asp:UpdatePanel ID="updAbsReason" runat="server" style="float: left;">
                                        <ContentTemplate>
                                            <table>
                                                <tr>
                                                    <td>
                                                    </td>
                                                    <td>
                                                        <asp:Label ID="lblreason" runat="server" Text="Reason" Font-Bold="True" Font-Names="Book Antiqua"
                                                            Font-Size="Medium"></asp:Label>
                                                    </td>
                                                    <td>
                                                        <asp:Button ID="btnaddreason" runat="server" Text="+" Font-Bold="True" Font-Names="Book Antiqua"
                                                            Font-Size="Small" OnClick="btnaddreason_Click" Style="display: none;" />
                                                    </td>
                                                    <td>
                                                        <script type="text/javascript">
                                                            function reason() {
                                                                document.getElementById('<%=btnaddreason.ClientID%>').style.display = 'block';
                                                                document.getElementById('<%=btnremovereason.ClientID%>').style.display = 'block';
                                                            }
                                                        </script>
                                                        <asp:DropDownList ID="ddlreason" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                                            Font-Size="Medium" Height="25px" Width="150px" onfocus="reason()">
                                                        </asp:DropDownList>
                                                    </td>
                                                    <td>
                                                        <asp:Button ID="btnremovereason" runat="server" Text="-" OnClick="btnremovereason_Click"
                                                            Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium" Style="display: none;" />
                                                    </td>
                                                </tr>
                                            </table>
                                            <asp:Panel ID="panel1" runat="server" BorderColor="Black" BackColor="AliceBlue" Visible="false"
                                                BorderWidth="2px" Height="125px" Width="690px">
                                                <div class="panelinfraction" id="Div2" style="text-align: center; font-family: MS Sans Serif;
                                                    font-size: Small; font-weight: bold">
                                                    <br />
                                                    <caption style="top: 30px; border-style: solid; border-color: Black; position: absolute;
                                                        left: 200px">
                                                        Add Reason
                                                    </caption>
                                                    <br />
                                                    <br />
                                                    <table>
                                                        <tr>
                                                            <td>
                                                                <asp:Label ID="lblatreason" runat="server" Text="Add Reason" Font-Bold="True" Font-Names="Book Antiqua"
                                                                    Font-Size="Medium"></asp:Label>
                                                            </td>
                                                            <td>
                                                                <asp:TextBox ID="txtreason" runat="server" Width="600px" Height="30px" TextMode="MultiLine"
                                                                    Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium"></asp:TextBox>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                            </td>
                                                            <td>
                                                                <asp:Button ID="btnreasonnew" runat="server" Text="Add" OnClick="btnreasonnew_Click"
                                                                    Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium" />
                                                                <asp:Button ID="btnreasonexit" runat="server" Text="Exit" OnClick="btnreasonexit_Click"
                                                                    Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium" />
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </div>
                                            </asp:Panel>
                                            <fieldset id="fieldat" runat="server" style="width: 300px; height: 430px" visible="false">
                                                <table style="text-align: right;">
                                                    <tr>
                                                        <td>
                                                            <asp:Label ID="lblatdate" runat="server" Text="Date :" Font-Bold="True" Font-Names="Book Antiqua"
                                                                Font-Size="Medium"></asp:Label>
                                                        </td>
                                                        <td>
                                                            <asp:Label ID="lblcurdate" runat="server" Text="" Font-Bold="True" Font-Names="Book Antiqua"
                                                                Font-Size="Medium"></asp:Label>
                                                        </td>
                                                        <td>
                                                            <asp:Label ID="lblhour" Text="Hour(s) :" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                                                Font-Size="Medium"></asp:Label>
                                                        </td>
                                                        <td>
                                                            <asp:Label ID="lblhrvalue" Text="" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                                                Font-Size="Medium"></asp:Label>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            <asp:Label ID="lblattend" runat="server" Text="Selected Students :" Font-Bold="True"
                                                                Font-Names="Book Antiqua" Font-Size="Medium"></asp:Label>
                                                        </td>
                                                        <td>
                                                            <asp:DropDownList ID="ddlattend" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                                                Font-Size="Medium">
                                                            </asp:DropDownList>
                                                        </td>
                                                    </tr>
                                                </table>
                                                <table>
                                                    <tr>
                                                        <td>
                                                            <asp:Button ID="btnaddrow" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                                                Font-Size="Medium" Text="Add Row" OnClick="btnaddrow_Click" />
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            
                                                            <FarPoint:FpSpread ID="fpattendanceentry" runat="server" BorderColor="Black" BorderStyle="Solid"
                                                                BorderWidth="1px" Height="100" Width="600" Enabled="False" ShowHeaderSelection="false">
                                                                <CommandBar BackColor="White" ButtonFaceColor="Control" ButtonHighlightColor="ControlLightLight"
                                                                    ButtonShadowColor="ControlDark" Visible="true">
                                                                </CommandBar>
                                                                <Sheets>
                                                                    <FarPoint:SheetView SheetName="Sheet1">
                                                                    </FarPoint:SheetView>
                                                                </Sheets>
                                                            </FarPoint:FpSpread>
                                                        </td>
                                                    </tr>
                                                </table>
                                                <table>
                                                    <tr>
                                                        <td>
                                                            <asp:Label ID="lblreststudent" runat="server" Text="For The Rest Of Students" Font-Bold="True"
                                                                Font-Names="Book Antiqua" Font-Size="Medium"></asp:Label>
                                                        </td>
                                                        <td>
                                                            <asp:DropDownList ID="ddlreststudent" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                                                Font-Size="Medium">
                                                            </asp:DropDownList>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            <asp:Label ID="lblerrmsg" runat="server" Font-Bold="true" ForeColor="Red" CssClass="floats"
                                                                Font-Names="Book Antiqua" Font-Size="Medium"></asp:Label>
                                                        </td>
                                                    </tr>
                                                </table>
                                                <table>
                                                    <tr>
                                                        <td>
                                                            <asp:Button ID="btnaddattendance" runat="server" Text="Save" OnClick="btnaddattendance_Click"
                                                                Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium" Style="left: 550px;
                                                                position: absolute;" />
                                                </table>
                                                </td></tr>
                                            </fieldset>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </div>
                            </td>
                        </tr>
                    </table>
                    <div id="divAttMark" runat="server">
                        <br />
                        <script type="text/javascript">

                            function Check_Click(objRef, rowIndex, colIndex, tot) {
                                //Get the Row based on checkbox
                                var row = objRef.parentNode.parentNode;
                                if (objRef.checked) {
                                    //If checked change color to Aqua
                                    row.style.backgroundColor = "green";
                                }
                                else {
                                    //If not checked change back to original color
                                    {
                                        row.style.backgroundColor = "red";
                                    }
                                }
                                //Get the reference of GridView
                                var GridView = row.parentNode;
                                //Get all input elements in Gridview
                                var inputList = GridView.getElementsByTagName("input");
                                for (var i = 0; i < inputList.length; i++) {
                                    //The First element is the Header Checkbox
                                    var headerCheckBox = inputList[0];
                                    //Based on all or none checkboxes
                                    //are checked check/uncheck Header Checkbox
                                    var checked = true;
                                    if (inputList[i] != null && inputList[i].type == "checkbox" && inputList[i] != headerCheckBox) {
                                        if (!inputList[i].checked) {
                                            checked = false;
                                            break;
                                        }
                                    }
                                }

                                var fl = 0;
                                var id = document.getElementById("<%=gridMarkAttnd.ClientID %>");
                                var len = id.rows.length;
                                var ak = rowIndex;
                                colIndex -= tot;
                                for (var i = 0; i < id.rows[rowIndex].cells.length; i++) {
                                    if (id.rows[ak].getElementsByTagName("input")[i] != null && id.rows[ak].getElementsByTagName("input")[i].type == "checkbox") {
                                        if (id.rows[ak].getElementsByTagName("input")[i].checked == false) {
                                            id.rows[ak].getElementsByTagName("input")[i].checked = false;
                                            var row1 = id.rows[ak].getElementsByTagName("input")[i].parentNode.parentNode;
                                            if (row1.style.backgroundColor != "DarkViolet") {
                                                row1.style.backgroundColor = "red";
                                            }
                                        }
                                        else {
                                            if (colIndex == i) {
                                                id.rows[ak].getElementsByTagName("input")[i].checked = true;
                                                var row1 = id.rows[ak].getElementsByTagName("input")[i].parentNode.parentNode;
                                                if (row1.style.backgroundColor != "DarkViolet") {
                                                    row1.style.backgroundColor = "green";
                                                }
                                            } else {
                                                id.rows[ak].getElementsByTagName("input")[i].checked = false;
                                                var row1 = id.rows[ak].getElementsByTagName("input")[i].parentNode.parentNode;
                                                if (row1.style.backgroundColor != "DarkViolet") {
                                                    row1.style.backgroundColor = "red";
                                                }
                                            }
                                        }
                                    }
                                }


                                var id22 = document.getElementById("<%=gridMarkAttnd.ClientID %>");
                                var newlen = id22.rows.length;
                                var lbl = "";
                                var startCol = document.getElementById('<%=lblPresentCol.ClientID%>');
                                var endCol = document.getElementById('<%=lblReasonCol.ClientID%>');
                                var stCol = 8;
                                var edCol = id22.rows[0].cells.length;
                                if (typeof startCol !== 'undefined' && startCol != null && startCol.innerHTML != "")
                                    stCol = parseInt(startCol.innerHTML);
                                if (typeof endCol !== 'undefined' && endCol != null && endCol.innerHTML != "")
                                    edCol = parseInt(endCol.innerHTML);
                                var start = 9;
                                var presentCount = 0;
                                for (var col = stCol; col <= id22.rows[0].cells.length; col++) {
                                    var count = 0;
                                    var dispval = document.getElementById('MainContent_gridMarkAttnd_col' + start);
                                    if (dispval != null) {
                                        //lbl += "No of "+dispval.value + ": ";
                                        lbl += "No of " + retFullAttType(dispval.value.toString()) + " : ";
                                        for (var row = 0; row < newlen; row++) {
                                            var newid = document.getElementById('MainContent_gridMarkAttnd_chk_' + start + '_' + row);
                                            if (newid != null && newid.checked == true) {
                                                count++;
                                            }
                                        }
                                        lbl += count + "<br/>";
                                        presentCount = count;
                                    }
                                    start++;
                                }
                                var presentOnly = document.getElementById('<%=lblPresentOnly.ClientID%>');
                                var onlypres = 0;
                                if (typeof presentOnly !== 'undefined' && presentOnly != null && presentOnly.innerHTML != "")
                                    onlypres = parseInt(presentOnly.innerHTML);
                                if (onlypres == 1) {
                                    lbl = "No of Present : " + presentCount + "<br/>No of Absent : " + (newlen - 1 - presentCount);
                                }
                                document.getElementById("<%=lblOpAbsent.ClientID %>").innerHTML = lbl;
                            }

                            function retFullAttType(attVal) {

                                switch (attVal.toUpperCase()) {
                                    case "P":
                                        attVal = "Present";
                                        break;
                                    case "A":
                                        attVal = "Absent";
                                        break;
                                    case "OD":
                                        attVal = "On Duty";
                                        break;
                                    default:
                                        attVal = attVal.toUpperCase();
                                        break;
                                }
                                return attVal;
                            }

                            var check = false;
                            function checkvalueHeader(colIndex, tot) {
                                var fl = 0;
                                var id = document.getElementById("<%=gridMarkAttnd.ClientID %>");
                                var len = id.rows.length;
                                var i = colIndex;
                                if (check == false) {
                                    check = true;
                                } else {
                                    check = false;
                                }
                                for (var ak = 1; ak < len; ak++) {
                                    if (id.rows[ak].getElementsByTagName("input")[i] != null && id.rows[ak].getElementsByTagName("input")[i].type == "checkbox") {
                                        if (check == true) {
                                            if (id.rows[ak].getElementsByTagName("input")[i].disabled == false) {
                                                id.rows[ak].getElementsByTagName("input")[i].checked = false;
                                                var row1 = id.rows[ak].getElementsByTagName("input")[i].parentNode.parentNode;
                                                if (row1.style.backgroundColor != "DarkViolet") {
                                                    row1.style.backgroundColor = "red";
                                                }
                                            }
                                        }
                                        else {
                                            if (id.rows[ak].getElementsByTagName("input")[i].disabled == false) {
                                                id.rows[ak].getElementsByTagName("input")[i].checked = true;
                                                var row1 = id.rows[ak].getElementsByTagName("input")[i].parentNode.parentNode;
                                                if (row1.style.backgroundColor != "DarkViolet") {
                                                    row1.style.backgroundColor = "green";
                                                }
                                            }
                                        }
                                    }
                                }
                                for (var ak = 1; ak < len; ak++) {
                                    for (var k = tot; k <= id.rows[1].cells.length; k++) {
                                        if ((k - tot) != colIndex) {
                                            if (id.rows[ak].getElementsByTagName("input")[(k - tot)] != null && id.rows[ak].getElementsByTagName("input")[(k - tot)].type == "checkbox") {
                                                if (id.rows[ak].getElementsByTagName("input")[(k - tot)].disabled == false) {
                                                    id.rows[ak].getElementsByTagName("input")[(k - tot)].checked = false;
                                                    var row1 = id.rows[ak].getElementsByTagName("input")[(k - tot)].parentNode.parentNode;
                                                    if (row1.style.backgroundColor != "DarkViolet") {
                                                        row1.style.backgroundColor = "red";
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                                //////=================starty
                                var id22 = document.getElementById("<%=gridMarkAttnd.ClientID %>");
                                var newlen = id22.rows.length;
                                var lbl = "";
                                var startCol = document.getElementById('<%=lblPresentCol.ClientID%>');
                                var endCol = document.getElementById('<%=lblReasonCol.ClientID%>');
                                var stCol = 8;
                                var edCol = id22.rows[0].cells.length;
                                if (typeof startCol !== 'undefined' && startCol != null && startCol.innerHTML != "")
                                    stCol = parseInt(startCol.innerHTML);
                                if (typeof endCol !== 'undefined' && endCol != null && endCol.innerHTML != "")
                                    edCol = parseInt(endCol.innerHTML);
                                var start = 9;
                                var presentCount = 0;
                                for (var col = stCol; col <= id22.rows[0].cells.length; col++) {
                                    var count = 0;
                                    var dispval = document.getElementById('MainContent_gridMarkAttnd_col' + start);
                                    if (dispval != null) {
                                        //lbl += dispval.value + "-";
                                        lbl += "No of " + retFullAttType(dispval.value.toString()) + " : ";
                                        for (var row = 0; row < newlen; row++) {
                                            var newid = document.getElementById('MainContent_gridMarkAttnd_chk_' + start + '_' + row);
                                            if (newid != null && newid.checked == true) {
                                                count++;
                                            }
                                        }
                                        lbl += count + "<br/>";
                                        presentCount = count;
                                    }
                                    start++;
                                }

                                var presentOnly = document.getElementById('<%=lblPresentOnly.ClientID%>');
                                var onlypres = 0;
                                if (typeof presentOnly !== 'undefined' && presentOnly != null && presentOnly.innerHTML != "")
                                    onlypres = parseInt(presentOnly.innerHTML);
                                if (onlypres == 1) {
                                    lbl = "No of Present : " + presentCount + "<br/>No of Absent : " + (newlen - 1 - presentCount);
                                }
                                document.getElementById("<%=lblOpAbsent.ClientID %>").innerHTML = lbl;
                                if (document.getElementById('MainContent_gridMarkAttnd_col' + colIndex) != null)
                                    document.getElementById('MainContent_gridMarkAttnd_col' + colIndex).onfocus = function () { this.blur(); }
                            }
                        </script>
                        <asp:Label ID="lblPresentCol" Text="9" runat="server" Style="display: none;" />
                        <asp:Label ID="lblReasonCol" Text="0" runat="server" Style="display: none;" />
                        <asp:Label ID="lblResonColEx" Text="0" runat="server" Style="display: none;" />
                        <asp:Label ID="lblPresentOnly" Text="0" runat="server" Style="display: none;" />
                        <asp:GridView ID="gridMarkAttnd" runat="server" AutoGenerateColumns="true" HeaderStyle-BackColor="#0CA6CA"
                            OnRowDataBound="gridMarkAttnd_OnRowDataBound" OnDataBound="gridMarkAttnd_OnDataBound">
                            <Columns>
                                <asp:TemplateField HeaderText="S.No">
                                    <ItemTemplate>
                                        <asp:Label ID="lblSNo" runat="server" Text='<%#Container.DisplayIndex+1 %>'></asp:Label>
                                        <asp:Label ID="lblAppNo" runat="server" Text='<%#Eval("app_no") %>' Visible="false"></asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Center" />
                                </asp:TemplateField>
                            </Columns>
                        </asp:GridView>
                        <br />
                        <div style="text-align: right; padding-right: 50px;">
                            <asp:Label ID="lblOpAbsent" runat="server" ForeColor="Red" Font-Bold="true" CssClass="floats"
                                Font-Names="Book Antiqua" Font-Size="Medium"></asp:Label>
                        </div>
                        <br />
                        <asp:Button ID="btnMarkAttSave" runat="server" CssClass="textbox btn" Width="60px"
                            Text="Save" OnClick="btnMarkAttSave_Click" BackColor="#63E7B1" UseSubmitBehavior="false" />
                        <asp:Button ID="btnMarkAttUpdate" runat="server" CssClass="textbox btn" Width="60px"
                            Text="Update" OnClick="btnMarkAttUpdate_Click" BackColor="#F8B7B3" UseSubmitBehavior="false" />
                    </div>
                    <br />
                </div>
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
                        <table style="height: 100px; width: 100%; padding:3px;">
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
</asp:Content>

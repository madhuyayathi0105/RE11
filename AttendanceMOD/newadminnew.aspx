<%@ Page Title="Attendance" Language="C#" MasterPageFile="~/AttendanceMOD/AttendanceSubSiteMaster.master"
    AutoEventWireup="true" CodeFile="newadminnew.aspx.cs" Inherits="newadminnew"
    EnableEventValidation="false" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <%--<script src="//ajax.googleapis.com/ajax/libs/jquery/1.11.1/jquery.min.js"></script>--%>
    <script type="text/javascript" src="../Scripts/jquery-1.4.1.min.js"></script>
    <script type="text/javascript">        //src="http://ajax.googleapis.com/ajax/libs/jquery/1.8.3/jquery.min.js"></script>
    <script type="text/javascript">        // src="http://code.jquery.com/jquery-2.1.1.min.js">
        //
        var prm = Sys.WebForms.PageRequestManager.getInstance();
        //Raised before processing of an asynchronous postback starts and the postback request is sent to the server.
        prm.add_beginRequest(BeginRequestHandler);
        // Raised after an asynchronous postback is finished and control has been returned to the browser.
        prm.add_endRequest(EndRequestHandler);
        function BeginRequestHandler(sender, args) {
            //Shows the modal popup - the update progress
            var popup = $find('<%= modalPopup.ClientID %>');
            if (popup != null) {
                popup.show();
            }
        }

        function EndRequestHandler(sender, args) {
            //Hide the modal popup - the update progress
            var popup = $find('<%= modalPopup.ClientID %>');
            if (popup != null) {
                popup.hide();
            }
        }
    </script>
    <script type="text/javascript">
        function Check_Click(objRef) {
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
                if (inputList[i].type == "checkbox" && inputList[i] != headerCheckBox) {
                    if (!inputList[i].checked) {
                        checked = false;
                        break;
                    }
                }
            }
        }
    </script>
    <script type="text/javascript">
        function onMouseOver(rowIndex) {
            var gv = document.getElementById('<%= gvatte.ClientID %>');
            var rowElement = gv.rows[rowIndex];
            rowElement.style.backgroundColor = "#FBBF0F";
        }
        function onMouseOut(rowIndex) {
            var gv = document.getElementById('<%= gvatte.ClientID %>');
            var rowElement = gv.rows[rowIndex];
            rowElement.style.backgroundColor = "#fff";
        }
        function checkvalue(rowIndex) {
            var fl = 0;
            var id = document.getElementById("<%=gvatte.ClientID %>");
            var len = id.rows.length;
            var ak = rowIndex;
            for (var i = 0; i < id.rows[rowIndex].cells.length; i++) {
                if (id.rows[ak].getElementsByTagName("input")[i].type == "checkbox" && id.rows[ak].getElementsByTagName("input")[i].disabled == false) {
                    if (id.rows[ak].getElementsByTagName("input")[i].checked == true) {
                        id.rows[ak].getElementsByTagName("input")[i].checked = false;
                        var row1 = id.rows[ak].getElementsByTagName("input")[i].parentNode.parentNode;
                        if (row1.style.backgroundColor != "DarkViolet") {
                            row1.style.backgroundColor = "red";
                        }
                    }
                    else {
                        id.rows[ak].getElementsByTagName("input")[i].checked = true;
                        var row1 = id.rows[ak].getElementsByTagName("input")[i].parentNode.parentNode;
                        if (row1.style.backgroundColor != "DarkViolet") {
                            row1.style.backgroundColor = "green";
                        }
                    }
                }
            }
        }

        //        function calculatePresentAbsent() {
        //            var presentCount = 0;
        //            var absentCount = 0;
        //            var id = document.getElementById("<%=gvatte.ClientID %>");
        //            var idchk = "chk";
        //            var idcount = "p";
        //            var gridViewControls = id.getElementsByTagName("input");
        //            var rowsCount = id.rows.length - 2;
        //            if (rowsCount > 0) {
        //                var cellCount = id.rows[0].cells.length;    
        //                for (var col = 5; col < cellCount; col++) {
        //                    presentCount = 0;
        //                    absentCount = 0;
        //                    for (var row = 0; row < rowsCount; row++) {
        //                        if (id.rows[row].getElementsByTagName("input")[col].type == "checkbox") {
        //                            alert('Present' + presentCount);
        //                            if (id.rows[row].getElementsByTagName("input")[col].checked == true) {
        //                                presentCount++;
        //                            }
        //                            else {
        //                                absentCount++;
        //                            }
        //                        }
        //                        
        //                    }
        //                    id.rows[rowsCount - 2].cells[col].valueOf(presentCount);
        //                    id.rows[rowsCount - 1].cells[col].valueOf(absentCount); 
        //                }
        //            }
        //        }

        function checkvaluecolumn(rowIndex) {
            var fl = 0;
            var id = document.getElementById("<%=gvatte.ClientID %>");
            var len = id.rows.length;
            var ak = rowIndex;
            var idchk = "chk" + rowIndex;
            var idcount = "p" + rowIndex;
            var len = id.rows.length;
            var gridViewControls = id.getElementsByTagName("input");
            var len = id.rows.length;
            for (var i = 0; i < gridViewControls.length; i++) {
                if (gridViewControls[i].name.indexOf(idchk) > 4 && gridViewControls[i].disabled == false) {
                    if (gridViewControls[i].checked == false) {
                        gridViewControls[i].checked = true;
                        var row1 = gridViewControls[i].parentNode.parentNode;

                        if (row1.style.backgroundColor != "DarkViolet") {
                            row1.style.backgroundColor = "green";
                        }
                        fl++;
                    }
                    else {
                        var row1 = gridViewControls[i].parentNode.parentNode;

                        if (row1.style.backgroundColor != "DarkViolet") {
                            row1.style.backgroundColor = "red";
                        }
                        gridViewControls[i].checked = false;
                        fl++;
                    }
                }
            }
        }
    </script>
    <script>
        function SyncTableColumns() {
            var grid = document.getElementById("<%= gvatte.ClientID %>");
            var table = document.getElementById("<%= GVhead.ClientID %>");
            for (var i = 0; i < grid.rows[0].cells.length; i++) {
                table.rows[0].cells[i].style.width = (parseInt(grid.rows[0].cells[i].offsetWidth)) + 'px';
            }
        }
    </script>
    <style type="text/css">
        .mycheckbox input[type="checkbox"]
        {
            cursor: pointer;
            appearance: none;
            background: #34495E;
            border-radius: 1px;
            box-sizing: border-box;
            box-sizing: content-box;
            border-width: 0;
            transition: all .3s linear;
            width: 100px;
            height: 50px;
        }
        .cb_style
        {
            height: 20px;
            width: 82px;
        }
        .RowStyle
        {
            height: 50px;
        }
        .AlternateRowStyle
        {
            height: 50px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <asp:UpdatePanel ID="UpdatePanel2" runat="server">
        <ContentTemplate>
            <asp:UpdateProgress ID="UpdateProgress123" runat="server" DynamicLayout="true" AssociatedUpdatePanelID="UpdatePanel2">
                <ProgressTemplate>
                    <div style="height: 1000px; width: 100%; position: fixed; top: 0; left: 0px; background-color: White;">
                        <center>
                            <img src="../gv images/cloud_loading_256.gif" style="margin-top: 100px; height: 150px;" />
                            <br />
                            <span style="font-family: Book Antiqua; font-size: medium; color: Gray;">Processing
                                Please Wait...</span>
                        </center>
                    </div>
                </ProgressTemplate>
            </asp:UpdateProgress>
            <asp:ModalPopupExtender ID="modalPopup" runat="server" TargetControlID="UpdateProgress123"
                PopupControlID="UpdateProgress123">
            </asp:ModalPopupExtender>
            <div>
                <center>
                    <span class="fontstyleheader" style="color: Green; margin: 0px; margin-top: 10px;
                        margin-bottom: 10px; position: relative;">Attendance</span>
                    <asp:Panel ID="Panel1" runat="server" Style="margin: 0px; margin-top: 10px; margin-bottom: 10px;
                        position: relative; width: 100%; height: auto;">
                        <table class="maintablestyle" style="background-color: #0CA6CA; width: auto; height: auto;">
                            <tr>
                                <td align="left">
                                    <table>
                                        <tr>
                                            <td>
                                                <asp:RadioButton ID="rbcommon" runat="server" Font-Names="Book Antiqua" Font-Size="Medium"
                                                    Font-Bold="true" Text="Commom" GroupName="AttEntry" AutoPostBack="true" OnCheckedChanged="Radio_CheckedChanged" />
                                            </td>
                                            <td>
                                                <asp:RadioButton ID="rbelective" runat="server" Font-Names="Book Antiqua" Font-Size="Medium"
                                                    Font-Bold="true" Text="Subject Wise" GroupName="AttEntry" AutoPostBack="true"
                                                    OnCheckedChanged="Radio_CheckedChanged" />
                                            </td>
                                            <td>
                                                <asp:RadioButton ID="rbincludeonduty" runat="server" Font-Names="Book Antiqua" Font-Size="Medium"
                                                    Font-Bold="true" Text="Include On Duty Student's" GroupName="Onduty" AutoPostBack="true"
                                                    OnCheckedChanged="ddlsem_SelectedIndexChanged" />
                                            </td>
                                            <td>
                                                <asp:RadioButton ID="rbexcludeonduty" runat="server" Font-Names="Book Antiqua" Font-Size="Medium"
                                                    Font-Bold="true" Text="Exclude On Duty Student's" GroupName="Onduty" AutoPostBack="true"
                                                    OnCheckedChanged="ddlsem_SelectedIndexChanged" />
                                            </td>
                                            <td>
                                                <asp:CheckBox ID="chkIncludeRedoStudent" Visible="true" Checked="false" runat="server" Text="Include Redo Student" />
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr>
                                <td align="left">
                                    <table style="height: auto; width: auto;">
                                        <tr>
                                            <td>
                                                <asp:Label ID="lblstraem" runat="server" Text="Stream" Font-Names="Book Antiqua"
                                                    Font-Size="Medium" Font-Bold="true"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:DropDownList ID="ddlstream" CssClass="cursorptr" runat="server" AutoPostBack="True"
                                                    Font-Names="Book Antiqua" OnSelectedIndexChanged="ddlstream_SelectedIndexChanged"
                                                    Font-Size="Medium" Font-Bold="True">
                                                </asp:DropDownList>
                                            </td>
                                            <td>
                                                <asp:Label ID="lblbatch" runat="server" Text="Batch Year" Font-Names="Book Antiqua"
                                                    Font-Size="Medium" Font-Bold="true"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:DropDownList ID="ddlbatch" CssClass="cursorptr" runat="server" AutoPostBack="True"
                                                    OnSelectedIndexChanged="ddlbatch_SelectedIndexChanged" Font-Names="Book Antiqua"
                                                    Font-Size="Medium" Font-Bold="True">
                                                </asp:DropDownList>
                                            </td>
                                            <td>
                                                <asp:Label ID="lblcourse" runat="server" Text="Course" Font-Names="Book Antiqua"
                                                    Font-Size="Medium" Font-Bold="true"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:DropDownList ID="ddlcourse" CssClass="cursorptr" runat="server" AutoPostBack="True"
                                                    OnSelectedIndexChanged="ddlcourse_SelectedIndexChanged" Font-Names="Book Antiqua"
                                                    Font-Size="Medium" Font-Bold="True">
                                                </asp:DropDownList>
                                            </td>
                                            <td>
                                                <asp:Label ID="lbldegree" runat="server" Text="Degree" Font-Names="Book Antiqua"
                                                    Font-Size="Medium" Font-Bold="true"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:DropDownList ID="ddldegree" CssClass="cursorptr" runat="server" AutoPostBack="true"
                                                    OnSelectedIndexChanged="ddldegree_SelectedIndexChanged" Width="100px" Font-Names="Book Antiqua"
                                                    Font-Size="Medium" Font-Bold="True">
                                                </asp:DropDownList>
                                            </td>
                                            <td>
                                                <asp:Label ID="lblbranch" runat="server" Text="Branch" Font-Names="Book Antiqua"
                                                    Font-Size="Medium" Font-Bold="true"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:DropDownList ID="ddlbranch" CssClass="cursorptr" runat="server" OnSelectedIndexChanged="ddlbranch_SelectedIndexChanged"
                                                    AutoPostBack="True" Width="189px" Font-Names="Book Antiqua" Font-Size="Medium"
                                                    Font-Bold="True">
                                                </asp:DropDownList>
                                            </td>
                                            <td>
                                                <asp:Label ID="lblsem" runat="server" Text="Sem" Font-Names="Book Antiqua" Font-Size="Medium"
                                                    Font-Bold="true"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:DropDownList ID="ddlsem" CssClass="cursorptr" runat="server" OnSelectedIndexChanged="ddlsem_SelectedIndexChanged"
                                                    Width="70px" AutoPostBack="True" Font-Names="Book Antiqua" Font-Size="Medium"
                                                    Font-Bold="True">
                                                </asp:DropDownList>
                                            </td>
                                            <td>
                                                <asp:Label ID="lblsec" runat="server" Text="Sec" Font-Names="Book Antiqua" Font-Size="Medium"
                                                    Font-Bold="true"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:DropDownList ID="ddlsec" CssClass="cursorptr" runat="server" AutoPostBack="True"
                                                    OnSelectedIndexChanged="ddlsec_SelectedIndexChanged" Width="60px" Font-Bold="True"
                                                    Font-Names="Book Antiqua" Font-Size="Medium">
                                                </asp:DropDownList>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr>
                                <td align="left">
                                    <table>
                                        <tr>
                                            <td>
                                                <asp:Label ID="lblsubtype" runat="server" Text="Subject Type" Font-Names="Book Antiqua"
                                                    Font-Size="Medium" Font-Bold="true"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:DropDownList ID="ddlsubtype" CssClass="cursorptr" runat="server" OnSelectedIndexChanged="ddlsubtype_SelectedIndexChanged"
                                                    AutoPostBack="True" Font-Names="Book Antiqua" Font-Size="Medium" Font-Bold="True">
                                                </asp:DropDownList>
                                            </td>
                                            <td>
                                                <asp:Label ID="lblsubjcet" runat="server" Text="Subject" Font-Names="Book Antiqua"
                                                    Font-Size="Medium" Font-Bold="true"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:DropDownList ID="ddlsubject" CssClass="cursorptr" runat="server" OnSelectedIndexChanged="ddlsubject_SelectedIndexChanged"
                                                    AutoPostBack="True" Font-Names="Book Antiqua" Font-Size="Medium" Font-Bold="True"
                                                    Width="150px">
                                                </asp:DropDownList>
                                            </td>
                                            <td>
                                                <asp:Label ID="lblstubatch" runat="server" Text="Student Batch" Font-Names="Book Antiqua"
                                                    Font-Size="Medium" Font-Bold="true"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:DropDownList ID="ddlstubatch" CssClass="cursorptr" runat="server" OnSelectedIndexChanged="ddlstubatch_SelectedIndexChanged"
                                                    AutoPostBack="True" Font-Names="Book Antiqua" Font-Size="Medium" Font-Bold="True"
                                                    Width="50px">
                                                </asp:DropDownList>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr>
                                <td align="left">
                                    <table>
                                        <tr>
                                            <td>
                                                <asp:Label ID="lblfrom" runat="server" Text="Date" Font-Names="Book Antiqua" Font-Size="Medium"
                                                    Font-Bold="true"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtFromDate" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                                    OnTextChanged="txtFromDate_TextChanged" Font-Size="Medium" AutoPostBack="true"
                                                    Width="75px"></asp:TextBox>
                                                <asp:CalendarExtender ID="CalendarExtender2" TargetControlID="txtFromDate" Format="dd-MM-yyyy"
                                                    runat="server">
                                                </asp:CalendarExtender>
                                            </td>
                                            <td>
                                                <asp:Label ID="lblhour" runat="server" Text="Hours" Font-Names="Book Antiqua" Font-Size="Medium"
                                                    Font-Bold="true"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:UpdatePanel ID="UpdatePanel4" runat="server">
                                                    <ContentTemplate>
                                                        <asp:TextBox ID="txthour" runat="server" ReadOnly="true" Width="100px" Font-Bold="True"
                                                            Font-Names="Book Antiqua" Font-Size="Medium">---Select---</asp:TextBox>
                                                        <asp:Panel ID="phour" runat="server" CssClass="multxtpanel" Height="200px">
                                                            <asp:CheckBox ID="chkhour" runat="server" Font-Bold="True" OnCheckedChanged="chkhour_ChekedChange"
                                                                Font-Names="Book Antiqua" Font-Size="Medium" Text="Select All" AutoPostBack="True" />
                                                            <asp:CheckBoxList ID="chklshour" runat="server" Font-Size="Medium" AutoPostBack="True"
                                                                Font-Bold="True" Font-Names="Book Antiqua" OnSelectedIndexChanged="chklshour_SelectedIndexChanged">
                                                            </asp:CheckBoxList>
                                                        </asp:Panel>
                                                        <asp:PopupControlExtender ID="PopupControlExtender2" runat="server" TargetControlID="txthour"
                                                            PopupControlID="phour" Position="Bottom">
                                                        </asp:PopupControlExtender>
                                                    </ContentTemplate>
                                                </asp:UpdatePanel>
                                            </td>
                                            <td>
                                                <asp:Button ID="Btngo" runat="server" OnClick="Btngo_Click" CssClass="cursorptr"
                                                    Style="font-weight: 700; width: auto; height: auto;" Text="Go" />
                                            </td>
                                            <td>
                                                <asp:Label ID="lblpre" runat="server" Text="Present" Style="font-weight: 700;" Font-Names="Book Antiqua"
                                                    Font-Size="Medium"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:ImageButton ID="imgbtnpresent" runat="server" OnClick="imgbtnpresentclick" Visible="true"
                                                    ImageUrl="~/gv images/Tick.png" />
                                            </td>
                                            <td>
                                                <asp:Label ID="lblabst" runat="server" Text="Absent" Style="font-weight: 700;" Font-Names="Book Antiqua"
                                                    Font-Size="Medium"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:ImageButton ID="imgbtnabst" runat="server" OnClick="imgbtnabstclick" Visible="true"
                                                    ImageUrl="~/gv images/Cross.png" />
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr>
                                <td align="left">
                                    <table>
                                        <tr>
                                            <td>
                                                <asp:Label ID="lblcopyfrom" runat="server" Text="Copy From" Style="font-weight: 700;"
                                                    Font-Names="Book Antiqua" Font-Size="Medium"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:DropDownList ID="ddlcopyfrom" CssClass="cursorptr" runat="server" Height="25px"
                                                    Font-Names="Book Antiqua" Font-Size="Medium" Font-Bold="True">
                                                </asp:DropDownList>
                                            </td>
                                            <td>
                                                <asp:Label ID="Label2" runat="server" Text="Copy To" Style="font-weight: 700;" Font-Names="Book Antiqua"
                                                    Font-Size="Medium"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:UpdatePanel ID="UpdatePanel3" runat="server">
                                                    <ContentTemplate>
                                                        <asp:TextBox ID="txtcopyto" runat="server" CssClass="Dropdown_Txt_Box" ReadOnly="true"
                                                            Width="100px" Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium">---Select---</asp:TextBox>
                                                        <asp:Panel ID="pcopyto" runat="server" CssClass="multxtpanel" Height="200px">
                                                            <asp:CheckBox ID="chkcopyto" runat="server" Font-Bold="True" OnCheckedChanged="chkcopyto_ChekedChange"
                                                                Font-Names="Book Antiqua" Font-Size="Medium" Text="Select All" AutoPostBack="True" />
                                                            <asp:CheckBoxList ID="chklscopyto" runat="server" Font-Size="Medium" AutoPostBack="True"
                                                                Font-Bold="True" Font-Names="Book Antiqua" OnSelectedIndexChanged="chklscopyto_SelectedIndexChanged">
                                                            </asp:CheckBoxList>
                                                        </asp:Panel>
                                                        <asp:PopupControlExtender ID="PopupControlExtender1" runat="server" TargetControlID="txtcopyto"
                                                            PopupControlID="pcopyto" Position="Bottom">
                                                        </asp:PopupControlExtender>
                                                    </ContentTemplate>
                                                </asp:UpdatePanel>
                                            </td>
                                            <td>
                                                <asp:Button ID="btncopy" runat="server" CssClass="cursorptr" Font-Bold="true" Text="Copy"
                                                    OnClick="btncopy_Click" Style="width: auto; height: auto;" />
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                        </table>
                    </asp:Panel>
                </center>
                <br />
                <center>
                    <asp:Label ID="lblset" runat="server" Visible="False" Style="font-family: 'Baskerville Old Face';
                        font-weight: 700; height: auto; width: auto;" Font-Bold="False" Font-Size="Medium"
                        ForeColor="Red"></asp:Label>
                </center>
            </div>
            <div>
                <center>
                    <div style="width: 1000px;">
                        <div id="styledDiv">
                            <div id="float">
                                <asp:GridView ID="GVhead" CssClass="gridview font" runat="server" ShowHeader="false"
                                    Font-Size="Small" OnRowCommand="GVhead_OnRowCommand" OnRowDataBound="GVhead_RowDataBound">
                                    <RowStyle CssClass="RowStyle" />
                                    <AlternatingRowStyle CssClass="AlternateRowStyle" />
                                </asp:GridView>
                            </div>
                        </div>
                        <div>
                            <asp:GridView ID="gvatte" runat="server" ShowHeader="false" AutoGenerateColumns="False"
                                OnSelectedIndexChanged="gvatte_SelectedIndexChanege" OnDataBound="OnDataBound"
                                CssClass="font" OnRowDataBound="OnRowDataBound" OnRowCommand="gv_OnRowCommand">
                                <Columns>
                                    <asp:TemplateField HeaderText="S.No">
                                        <ItemTemplate>
                                            <asp:Label ID="lblsno" runat="server" Text="<%# Container.DisplayIndex+1 %>"></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Center" Width="50px" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Roll No">
                                        <ItemTemplate>
                                            <asp:Label ID="lblroll_no" runat="server" Text='<%# Eval("Roll_no") %>'></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Center" Width="100px" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Reg No">
                                        <ItemTemplate>
                                            <asp:Label ID="lblReg_no" runat="server" Text='<%# Eval("Reg_no") %>'></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Center" Width="100px" />
                                    </asp:TemplateField>
                                     <asp:TemplateField HeaderText="Admission No">
                                        <ItemTemplate>
                                            <asp:Label ID="lblAdmitNo" runat="server" Text='<%# Eval("Roll_Admit") %>'></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Center" Width="100px" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Name">
                                        <ItemTemplate>
                                            <asp:Label ID="lblstud_name" runat="server" Text='<%# Eval("stud_name") %>'></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Left" Width="350px" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Type">
                                        <ItemTemplate>
                                            <asp:Label ID="lblstud_type" runat="server" ForeColor="Brown" Text='<%# Eval("stud_type") %>'></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" Width="150px" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="1">
                                        <ItemTemplate>
                                            <asp:CheckBox ID="chk6" runat="server" Width="100px" onclick="Check_Click(this);"
                                                CssClass="mycheckbox" />
                                            <asp:Label ID="p6" runat="server" Text="a" Visible="false"></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="100px" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="2">
                                        <ItemTemplate>
                                            <asp:CheckBox ID="chk7" runat="server" onclick="Check_Click(this);" CssClass="mycheckbox" />
                                            <asp:Label ID="p7" runat="server" Text="a" Visible="false"></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="100px" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="3">
                                        <ItemTemplate>
                                            <asp:CheckBox ID="chk8" runat="server" onclick="Check_Click(this);" CssClass="mycheckbox" />
                                            <asp:Label ID="p8" runat="server" Text="a" Visible="false"></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="100px" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="4">
                                        <ItemTemplate>
                                            <asp:CheckBox ID="chk9" runat="server" onclick="Check_Click(this);" CssClass="mycheckbox" />
                                            <asp:Label ID="p9" runat="server" Text="a" Visible="false"></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="100px" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="5">
                                        <ItemTemplate>
                                            <asp:CheckBox ID="chk10" runat="server" onclick="Check_Click(this);" CssClass="mycheckbox" />
                                            <asp:Label ID="p10" runat="server" Text="a" Visible="false"></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="100px" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="6">
                                        <ItemTemplate>
                                            <asp:CheckBox ID="chk11" runat="server" onclick="Check_Click(this);" CssClass="mycheckbox" />
                                            <asp:Label ID="p11" runat="server" Text="a" Visible="false"></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="100px" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="7">
                                        <ItemTemplate>
                                            <asp:CheckBox ID="chk12" runat="server" onclick="Check_Click(this);" CssClass="mycheckbox" />
                                            <asp:Label ID="p12" runat="server" Text="a" Visible="false"></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="100px" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="8">
                                        <ItemTemplate>
                                            <asp:CheckBox ID="chk13" runat="server" onclick="Check_Click(this);" CssClass="mycheckbox" />
                                            <asp:Label ID="p13" runat="server" Text="a" Visible="false"></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="100px" />
                                    </asp:TemplateField>

                                       <asp:TemplateField HeaderText="9">
                                        <ItemTemplate>
                                            <asp:CheckBox ID="chk14" runat="server" onclick="Check_Click(this);" CssClass="mycheckbox" />
                                            <asp:Label ID="p14" runat="server" Text="a" Visible="false"></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="100px" />
                                    </asp:TemplateField>
                                </Columns>
                            </asp:GridView>
                        </div>
                        <br />
                        <asp:Button ID="btngvsave" runat="server" Text="Save" OnClick="btngvsave_click" CssClass=" textbox btn"
                            Style="width: auto; height: auto;" />
                        <asp:Button ID="btnprint" runat="server" Text="Print" OnClick="btnprint_click" CssClass=" textbox btn"
                            Style="width: auto; height: auto;" />
                    </div>
                </center>
            </div>
            <style>
                .gvvisible
                {
                    visibility: hidden;
                }
                .font
                {
                    font-family: Book Antiqua;
                    font-size: medium;
                    font-weight: bold;
                }
                .gridview td
                {
                    background-color: rgb(32,100,165);
                    color: white;
                    font-family: Book Antiqua;
                    font-size: medium;
                    font-weight: bold;
                    text-align: center;
                }
                .fixedPos
                {
                    position: fixed;
                    top: 0px;
                }
                .btn
                {
                    background: #3498db;
                    background-image: -webkit-linear-gradient(top, #3498db, #2980b9);
                    background-image: -moz-linear-gradient(top, #3498db, #2980b9);
                    background-image: -ms-linear-gradient(top, #3498db, #2980b9);
                    background-image: -o-linear-gradient(top, #3498db, #2980b9);
                    background-image: linear-gradient(to bottom, #3498db, #2980b9);
                    -webkit-border-radius: 10;
                    -moz-border-radius: 10;
                    border-radius: 10px;
                    border: 0px solid white;
                    color: #ffffff;
                    font-size: 25px;
                    padding: 10px 20px 10px 20px;
                    text-decoration: none;
                    font-family: Book Antiqua;
                    font-size: medium;
                    font-weight: bold;
                }
                
                .btn:hover
                {
                    background: #3cb0fd;
                    background-image: -webkit-linear-gradient(top, #3cb0fd, #3498db);
                    background-image: -moz-linear-gradient(top, #3cb0fd, #3498db);
                    background-image: -ms-linear-gradient(top, #3cb0fd, #3498db);
                    background-image: -o-linear-gradient(top, #3cb0fd, #3498db);
                    background-image: linear-gradient(to bottom, #3cb0fd, #3498db);
                    text-decoration: none;
                    cursor: pointer;
                }
            </style>
            <script type="text/javascript">
                $(window).scroll(function () {
                    var styledDiv = $('#float'),
            targetScroll = $('#styledDiv').position().top,
            currentScroll = $('html').scrollTop() || $('body').scrollTop();

                    styledDiv.toggleClass('fixedPos', currentScroll >= targetScroll);
                });
            </script>
        </ContentTemplate>
        <Triggers>
            <asp:PostBackTrigger ControlID="btnprint" />
        </Triggers>
    </asp:UpdatePanel>
</asp:Content>

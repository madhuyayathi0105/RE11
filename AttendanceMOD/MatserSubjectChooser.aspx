<%@ Page Title="" Language="C#" MasterPageFile="~/AttendanceMOD/AttendanceSubSiteMaster.master"
    AutoEventWireup="true" CodeFile="MatserSubjectChooser.aspx.cs" Inherits="MatserSubjectChooser" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Src="~/Usercontrols/PrintMaster.ascx" TagName="PRINTPDF" TagPrefix="Insproplus" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <style type="text/css">
        .font
        {
            font-family: Book Antiqua;
            font-size: medium;
            font-weight: bold;
        }
        .topHandle
        {
            background-color: #97bae6;
        }
        .modalPopup
        {
            background-color: #ffffdd;
            border-width: 1px;
            -moz-border-radius: 5px;
            border-style: solid;
            border-color: Gray;
            min-width: 250px;
            max-width: 500px;
            min-height: 100px;
            max-height: 150px;
            top: 100px;
            left: 150px;
        }
    </style>
    <script type="text/javascript">
        function SelectAll(id) {
            var grid = document.getElementById("<%=GridView1.ClientID %>");
            var chk = document.getElementById(id);
            var array = id.split("_");
            var position = array[2].toString();
            if (chk.checked) {
                if (grid.rows.length > 0) {
                    for (var jk = 2; jk < grid.rows.length; jk++) {
                        var chkall = document.getElementById('MainContent_GridView1_' + position + '_' + jk.toString());
                        chkall.checked = true;
                    }
                }
            }
            else {
                if (grid.rows.length > 0) {
                    for (var ji = 2; ji < grid.rows.length; ji++) {
                        var chekall = document.getElementById('MainContent_GridView1_' + position + '_' + ji.toString());
                        chekall.checked = false;
                    }
                }
            }
        }
    </script>
    <script type="text/javascript" language="javascript">
        function HeaderCheckBoxClick(checkbox) {
            var gridview = document.getElementById("GridView1");
            for (var i = 1; i < gridview.rows.length; i++) {
                gridview.rows[i].cell[4].getElementsByTagName("INPUT")[4].checked = checkbox.checked;
            }
        }
    </script>
    <script type="text/javascript">
        $(document).ready(function () {
            var table = $('#GridView1').DataTable({
                scrollY: "300px",
                scrollX: true,
                scrollCollapse: true,
                paging: false,
                fixedColumns: {
                    leftColumns: 1,
                    rightColumns: 1
                }
            });
        });
    </script>
    <script type="text/javascript">

        function AutoCompleteExtender1_OnClientPopulating(sender, args) {
            var degree = document.getElementById("<%=ddldegree.ClientID%>").value;
            var batch = document.getElementById("<%=ddlbatch.ClientID%>").value;
            var branch = document.getElementById("<%=ddlbranch.ClientID%>").value;

            var Section = 'all';

            if (document.getElementById("<%=ddlsection.ClientID%>").hasChildNodes()) {
                Section = document.getElementById("<%=ddlsection.ClientID%>").value;
            }

            var semester = document.getElementById("<%=ddlsemester.ClientID%>").value;
            var details = degree + '-' + batch + '-' + branch + '-' + Section + '-' + semester;
            sender.set_contextKey(details);
        }
    </script>
    <script type="text/javascript" src="http://ajax.googleapis.com/ajax/libs/jquery/1.9.0/jquery.min.js"></script>
    <script type="text/javascript" src="http://ajax.googleapis.com/ajax/libs/jqueryui/1.9.1/jquery-ui.min.js"></script>
    <script src="<%= ResolveUrl("~/GridviewScroll/gridviewScroll.min.js") %>" type="text/javascript"></script>
    <script src="~/GridviewScroll/jquery.min.js" type="text/javascript"></script>
    <link href="~/GridviewScroll/gridviewScroll.css" rel="stylesheet" type="text/css" />
    <script src="~/GridviewScroll/gridviewScroll.js" type="text/javascript"></script>
    <script src="~/GridviewScroll/gridviewScroll.min.js" type="text/javascript"></script>
    <style type="text/css">
        body
        {
            font-family: Arial;
            font-size: 10pt;
        }
    </style>
    <script type="text/javascript">
        $(document).ready(function () {

            gridviewScroll();
        });
        function gridviewScroll() {
            //            alert("hello");
            //            $('#ctl00_MainContent_GridView1').gridviewScroll({
           
           var ms = document.getElementById("<%=hid.ClientID %>").value;
            $('#<%=GridView1.ClientID%>').gridviewScroll({
                width: 980,
                height: 500,
                freezesize: ms,
               
            });


        }
    </script>
    <script type="text/javascript">
    .panellayout { z-index: 1000; border: solid; border-width: 1px; border-color: gray;
    position: absolute; left: 1px; } .GridViewContainer { position: relative; overflow:
    auto; ) /* to freeze column cells and its respecitve header*/ .FrozenCell { background-color:
    #F0F8FF; position: relative; cursor: default; left: expression(document.getElementById("GridViewContainer").scrollLeft-2);
    z-index: 30; } /* for freezing column header*/ .FrozenHeader { position: relative;
    cursor: default; left: expression(document.getElementById("GridViewContainer").scrollLeft-2);
    z-index: 20; }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <center>
        <br />
        <asp:Label ID="Label5" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
            Font-Size="Large" ForeColor="Green" Text="Master Subject Allotment"></asp:Label>
    </center>
    <br />
    <center>
        <center>
            <div id="mainpaneldiv" runat="server">
                <table style="width: 700px; height: 70px; background-color: #0CA6CA;">
                    <tr>
                        <td>
                            <asp:Label ID="lblstream" runat="server" Text="Stream" Font-Bold="True" Font-Names="Book Antiqua"
                                Font-Size="Medium"></asp:Label>
                        </td>
                        <td>
                            <asp:DropDownList ID="ddlstream" runat="server" OnSelectedIndexChanged="ddlstream_SelectedIndexChanged"
                                AutoPostBack="True" Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium"
                                Height="25px" Width="85px">
                            </asp:DropDownList>
                        </td>
                        <td>
                            <asp:Label ID="Label1" runat="server" Text="Batch" Font-Bold="True" Font-Names="Book Antiqua"
                                Font-Size="Medium"></asp:Label>
                        </td>
                        <td>
                            <asp:DropDownList ID="ddlbatch" runat="server" OnSelectedIndexChanged="ddlbatch_SelectedIndexChanged"
                                AutoPostBack="True" Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium"
                                Height="25px" Width="69px">
                            </asp:DropDownList>
                        </td>
                        <td>
                            <asp:Label ID="Label2" runat="server" Text="Degree" Font-Bold="True" Font-Names="Book Antiqua"
                                Font-Size="Medium"></asp:Label>
                        </td>
                        <td>
                            <asp:DropDownList ID="ddldegree" runat="server" OnSelectedIndexChanged="ddldegree_SelectedIndexChanged"
                                AutoPostBack="True" Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium"
                                Height="25px">
                            </asp:DropDownList>
                        </td>
                        <td>
                            <asp:Label ID="lblbranch" runat="server" Text="Branch" Font-Bold="True" Font-Names="Book Antiqua"
                                Font-Size="Medium"></asp:Label>
                        </td>
                        <td>
                            <asp:DropDownList ID="ddlbranch" runat="server" OnSelectedIndexChanged="ddlbranch_SelectedIndexChanged"
                                AutoPostBack="True" Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium"
                                Height="25px" Width="170px">
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label ID="lblsem" runat="server" Text="Sem" Font-Bold="True" Font-Names="Book Antiqua"
                                Font-Size="Medium"></asp:Label>
                        </td>
                        <td>
                            <asp:DropDownList ID="ddlsemester" runat="server" OnSelectedIndexChanged="ddlsemester_SelectedIndexChanged"
                                AutoPostBack="True" Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium"
                                Height="25px" Width="41px">
                            </asp:DropDownList>
                        </td>
                        <td>
                            <asp:Label ID="lblsec" runat="server" Text="Sec" Font-Bold="True" Font-Names="Book Antiqua"
                                Font-Size="Medium"></asp:Label>
                        </td>
                        <td>
                            <asp:DropDownList ID="ddlsection" runat="server" AutoPostBack="True" Font-Bold="True"
                                Font-Names="Book Antiqua" Font-Size="Medium" Height="25px" Width="52px" OnSelectedIndexChanged="ddlsection_SelectedIndexChanged">
                            </asp:DropDownList>
                        </td>
                        <td>
                            <asp:Label ID="lblSuType" runat="server" Text="Type" Font-Bold="True" Font-Names="Book Antiqua"
                                Font-Size="Medium">
                            </asp:Label>
                        </td>
                        <td>
                            <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                                <ContentTemplate>
                                    <asp:TextBox ID="txtSubType" runat="server" CssClass="textbox txtheight2" ReadOnly="true"
                                        Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium">--Select--</asp:TextBox>
                                    <asp:Panel ID="Panel3" runat="server" CssClass="multxtpanel" Width="350px" Height="180px"
                                        Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium">
                                        <asp:CheckBox ID="CheckBox1" runat="server" Text="Select All" AutoPostBack="true"
                                            OnCheckedChanged="CheckBox1_checkedchange" />
                                        <asp:CheckBoxList ID="CheckBoxList1" runat="server" AutoPostBack="true" OnSelectedIndexChanged="CheckBoxList1_SelectedIndexChanged">
                                        </asp:CheckBoxList>
                                    </asp:Panel>
                                    <asp:PopupControlExtender ID="PopupControlExtender4" runat="server" TargetControlID="txtSubType"
                                        PopupControlID="Panel3" Position="Bottom">
                                    </asp:PopupControlExtender>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </td>
                        <td>
                            <asp:Label ID="lblSubject" runat="server" Text="Subject" Font-Bold="True" Font-Names="Book Antiqua"
                                Font-Size="Medium"></asp:Label>
                        </td>
                        <td>
                            <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                                <ContentTemplate>
                                    <asp:TextBox ID="txtSubject" runat="server" CssClass="textbox txtheight2" ReadOnly="true"
                                        Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium">--Select--</asp:TextBox>
                                    <asp:Panel ID="Panel2" runat="server" CssClass="multxtpanel" Width="350px" Height="180px"
                                        Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium">
                                        <asp:CheckBox ID="cbSubjet" runat="server" Text="Select All" AutoPostBack="true"
                                            OnCheckedChanged="cbSubjet_checkedchange" />
                                        <asp:CheckBoxList ID="cblSubject" runat="server" AutoPostBack="true" OnSelectedIndexChanged="cblSubject_SelectedIndexChanged">
                                        </asp:CheckBoxList>
                                    </asp:Panel>
                                    <asp:PopupControlExtender ID="PopupControlExtender3" runat="server" TargetControlID="txtSubject"
                                        PopupControlID="Panel2" Position="Bottom">
                                    </asp:PopupControlExtender>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label ID="lblsearchby" runat="server" Text="Search By" Font-Bold="True" Font-Names="Book Antiqua"
                                Font-Size="Medium"></asp:Label>
                        </td>
                        <td>
                            <asp:DropDownList ID="ddlSearchBy" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlSearchBy_SelectedIndexChanged"
                                Font-Bold="true">
                            </asp:DropDownList>
                        </td>
                        <td colspan="3">
                            <div id="divSearchStudent">
                                <asp:TextBox ID="txtRollNo" runat="server" Font-Names="Book Antiqua" Width="200px"
                                    Font-Size="Medium" Visible="false" onkeydown="return (event.keyCode!=13);"></asp:TextBox>
                                <asp:AutoCompleteExtender ID="AutoCompleteExtender1" runat="server" DelimiterCharacters=","
                                    Enabled="True" ServiceMethod="GetRollNo" MinimumPrefixLength="0" CompletionInterval="100"
                                    EnableCaching="false" CompletionSetCount="10" ServicePath="" TargetControlID="txtRollNo"
                                    CompletionListCssClass="autocomplete_completionListElement" CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem"
                                    CompletionListItemCssClass="txtsearchpan" OnClientPopulating="AutoCompleteExtender1_OnClientPopulating">
                                </asp:AutoCompleteExtender>
                                <asp:TextBox ID="txtRegNo" runat="server" Font-Names="Book Antiqua" Width="200px"
                                    Font-Size="Medium" Visible="false" onkeydown="return (event.keyCode!=13);"></asp:TextBox>
                                <asp:AutoCompleteExtender ID="AutoCompleteExtender2" runat="server" DelimiterCharacters=","
                                    Enabled="True" ServiceMethod="GetRegNo" MinimumPrefixLength="0" CompletionInterval="100"
                                    EnableCaching="false" CompletionSetCount="10" ServicePath="" TargetControlID="txtRegNo"
                                    CompletionListCssClass="autocomplete_completionListElement" CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem"
                                    CompletionListItemCssClass="txtsearchpan" OnClientPopulating="AutoCompleteExtender1_OnClientPopulating">
                                </asp:AutoCompleteExtender>
                                <asp:TextBox ID="txtAdmissionNo" runat="server" Font-Names="Book Antiqua" Width="200px"
                                    Font-Size="Medium" Visible="false" onkeydown="return (event.keyCode!=13);"></asp:TextBox>
                                <asp:AutoCompleteExtender ID="AutoCompleteExtender3" runat="server" DelimiterCharacters=","
                                    Enabled="True" ServiceMethod="GetAdmitNo" MinimumPrefixLength="0" CompletionInterval="100"
                                    EnableCaching="false" CompletionSetCount="10" ServicePath="" TargetControlID="txtAdmissionNo"
                                    CompletionListCssClass="autocomplete_completionListElement" CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem"
                                    CompletionListItemCssClass="txtsearchpan" OnClientPopulating="AutoCompleteExtender1_OnClientPopulating">
                                </asp:AutoCompleteExtender>
                            </div>
                        </td>
                        <td>
                            <script type="text/javascript" language="javascript">
                                Sys.Application.add_load(gridviewScroll);
                            </script>
                            <asp:Button ID="btngo" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                Font-Size="Medium" Text="Go" OnClick="btngo_Click" />
                        </td>
                    </tr>
                    <tr>
                        <td colspan="8">
                            <table>
                                <tr>
                                    <td>
                                        <asp:Label ID="lblsubfilter" runat="server" Text="Subject Filter" Font-Bold="True"
                                            Font-Names="Book Antiqua" Font-Size="Medium" Width="100px"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:CheckBox ID="chkbatch" runat="server" Text="Batch Wise" Font-Bold="True" Font-Names="Book Antiqua"
                                            Font-Size="Medium" AutoPostBack="true" OnCheckedChanged="subwisefiler" Width="105px" />
                                        <asp:CheckBox ID="chksem" runat="server" Text="Sem Wise" Font-Bold="True" Font-Names="Book Antiqua"
                                            Font-Size="Medium" AutoPostBack="true" OnCheckedChanged="subwisefiler" Width="105px" />
                                    </td>
                                    <td>
                                        <asp:Label ID="lblsubtype" runat="server" Text="Subject Type" Font-Bold="True" Font-Names="Book Antiqua"
                                            Font-Size="Medium" Width="100px"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="ddlsubtype" runat="server" AutoPostBack="True" Font-Bold="True"
                                            Font-Names="Book Antiqua" Font-Size="Medium" Height="25px" Width="200px" OnSelectedIndexChanged="ddlsubtype_SelectedIndexChanged">
                                        </asp:DropDownList>
                                    </td>
                                    <td>
                                        <asp:Label ID="Label4" runat="server" Text="Subject" Font-Bold="True" Font-Names="Book Antiqua"
                                            Font-Size="Medium"> </asp:Label>
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="ddlsubject" runat="server" AutoPostBack="True" Font-Bold="True"
                                            Font-Names="Book Antiqua" Font-Size="Medium" Height="25px" Width="200px">
                                        </asp:DropDownList>
                                    </td>
                                    <td>
                                        <asp:Button ID="btnaddsub" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                            Font-Size="Medium" Text="Add" OnClick="btnaddsub_Click" />
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                </table>
            </div>
        </center>
        <br />
        <br />
        <center>
            <div style="height: 500px; width: 1000px; left: 0px">
                <asp:Label ID="errmsg" runat="server" Text="No Record(s) Found" ForeColor="Red" Visible="False"
                    Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium"></asp:Label>
                <asp:Panel ID="Panel1" runat="server" CssClass="panellayout" Style="top: 91px; height: 482px;
                    width: 1000px; left: 0px">
                    <div id="GridViewContainer" class="GridViewContainer" style="height: 482px; width: 1000px;
                        left: 0px">
                        <asp:GridView ID="GridView1" runat="server" Style="position: relative; clip: rect(auto auto auto auto);
                            left: 1px; width: 1000px; top: 3px;" Font-Names="Times New Roman" AutoGenerateColumns="true"
                            ShowHeader="false" OnRowDataBound="gridview1_OnRowDataBound" CssClass="FrozenCell">
                            <Columns>
                            </Columns>
                            <SelectedRowStyle BackColor="#339966" Font-Bold="True" />
                        </asp:GridView>
                        <asp:CheckBox ID="chkexammrk" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                            Font-Size="Medium" Text="EFFECT IN COE MARK" ForeColor="Blue" /><br />
                        <asp:Button ID="btnsave" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                            Font-Size="Medium" Text="Save" OnClick="btnsave_Click" />
                    </div>
                </asp:Panel>
            </div>
        </center>
        <br />
        <br />
        <center>
            <input type="hidden" runat="server" id="hid" />
            <div id="divSave" runat="server" style="height: 150px; width: 1000px; left: 0px;
                margin-top: 100px;">
                <asp:HiddenField ID="hfsave" runat="server" />
                <asp:ModalPopupExtender ID="mpesave" runat="server" TargetControlID="hfsave" PopupControlID="psave">
                </asp:ModalPopupExtender>
                <asp:Panel ID="psave" runat="server" CssClass="modalPopup" Style="display: none;
                    height: 500; width: 500;" DefaultButton="btnsaveok">
                    <table width="500">
                        <tr class="topHandle">
                            <td colspan="2" align="left" runat="server" id="td1">
                                <asp:Label ID="Label6" runat="server" Font-Bold="True" Text="Confirmation" Font-Names="Book Antiqua"
                                    Font-Size="Large"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 60px" valign="middle" align="center">
                                <asp:Image ID="Image1" runat="server" ImageUrl="~/Info-48x48.png" />
                            </td>
                            <td valign="middle" align="left">
                                <asp:Label ID="Label7" Text="Already allocate the batch for this class.You want to save this changes means, you should re-allocate the batches.Do you want to continue?"
                                    runat="server" Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2" align="right">
                                <asp:Button ID="btnsaveok" runat="server" Text="Yes" OnClick="btnsaveok_Click" Font-Bold="True"
                                    Font-Names="Book Antiqua" Font-Size="Medium" />
                                <asp:Button ID="btnsaveCancel" runat="server" Text="No" OnClick="btnsaveCancel_Click"
                                    Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium" />
                            </td>
                        </tr>
                    </table>
                </asp:Panel>
            </div>
        </center>
        <center>
            <div id="divPopAlert" runat="server" visible="false" style="height: 550em; z-index: 2000;
                width: 100%; background-color: rgba(54, 25, 25, .2); position: absolute; top: 0%;
                left: 0%;">
                <center>
                    <div id="divPopAlertContent" runat="server" class="table" style="background-color: White;
                        height: 120px; width: 23%; border: 5px solid #0CA6CA; border-top: 25px solid #0CA6CA;
                        left: 39%; right: 39%; top: 35%; padding: 5px; position: fixed; border-radius: 10px;">
                        <center>
                            <table style="height: 100px; width: 100%; padding: 5px;">
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
    </center>
    <%--<center>
        <asp:UpdateProgress ID="UpdateProgress1" runat="server" AssociatedUpdatePanelID="UpGoAdd">
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
    <center>
        <asp:UpdateProgress ID="UpdateProgress2" runat="server" AssociatedUpdatePanelID="upsave">
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
        <asp:ModalPopupExtender ID="ModalPopupExtender2" runat="server" TargetControlID="UpdateProgress2"
            PopupControlID="UpdateProgress2">
        </asp:ModalPopupExtender>
    </center>
    <center>
        <asp:UpdateProgress ID="UpdateProgress3" runat="server" AssociatedUpdatePanelID="upadd">
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
        <asp:ModalPopupExtender ID="ModalPopupExtender3" runat="server" TargetControlID="UpdateProgress3"
            PopupControlID="UpdateProgress3">
        </asp:ModalPopupExtender>
    </center>--%>
</asp:Content>

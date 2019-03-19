<%@ Page Title="" Language="C#" MasterPageFile="~/AttendanceMOD/AttendanceSubSiteMaster.master"
    AutoEventWireup="true" CodeFile="SchoolCompartmentwiseReport.aspx.cs" Inherits="SchoolCompartmentwiseReport"
    EnableEventValidation="false" %>

<%@ Register Src="~/Usercontrols/PrintMaster.ascx" TagName="printmaster" TagPrefix="Insproplus" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Assembly="FarPoint.Web.Spread" Namespace="FarPoint.Web.Spread" TagPrefix="FarPoint" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">
    <!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
    <html xmlns="http://www.w3.org/1999/xhtml">
    <title></title>
    <link href="~/Styles/css/Registration.css" rel="stylesheet" type="text/css" />
    <link href="~/Styles/css/Commoncss.css" rel="Stylesheet" type="text/css" />
    <body>
        <style type="text/css">
            .printclass
            {
                display: none;
            }
            .grid-view
            {
                padding: 0;
                margin: 0;
                border: 1px solid #333;
                font-family: "Verdana, Arial, Helvetica, sans-serif, Trebuchet MS";
                font-size: 0.9em;
            }
            
            .grid-view tr.header
            {
                color: white;
                background-color: #0CA6CA;
                height: 30px;
                vertical-align: middle;
                text-align: center;
                font-weight: bold;
                font-size: 20px;
            }
            
            .grid-view tr.normal
            {
                color: black;
                background-color: #FDC64E;
                height: 25px;
                vertical-align: middle;
                text-align: center;
            }
            
            .grid-view tr.alternate
            {
                color: black;
                background-color: #D59200;
                height: 25px;
                vertical-align: middle;
                text-align: center;
            }
            
            .grid-view tr.normal:hover, .grid-view tr.alternate:hover
            {
                background-color: white;
                color: black;
                font-weight: bold;
            }
            
            .grid_view_lnk_button
            {
                color: Black;
                text-decoration: none;
                font-size: large;
            }
            .lbl
            {
                font-family: Book Antiqua;
                font-size: 30px;
                font-weight: bold;
                color: Green;
                text-align: center;
                font-style: italic;
            }
            .hdtxt
            {
                font-family: Book Antiqua;
                font-size: large;
                font-weight: bold;
            }
            .FixedHeader
            {
                position: absolute;
                font-weight: bold;
            }
        </style>
        <script type="text/javascript">
            function PrintPanel() {
                var panel = document.getElementById("<%=pnlContents.ClientID %>");
                var printWindow = window.open('', '', 'height=842,width=1191');
                printWindow.document.write('<html');
                printWindow.document.write('<head><title>SchoolCompartmentWise</title>');
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
        <asp:ScriptManager ID="ScriptManager" runat="server">
        </asp:ScriptManager>
        <div>
            <center>
                <div>
                    <span class="fontstyleheader" style="color: Green;">School Compartmentwise Report</span></div>
            </center>
        </div>
        <br />
        <div>
            <center>
                <div id="maindiv" runat="server" class="maindivstyle" style="width: 1000px; height: auto">
                    <br />
                    <table class="maintablestyle" >
                        <tr>
                            <td>
                                <asp:Label ID="lblclg" runat="server" Text="College"></asp:Label>
                            </td>
                            <td>
                                <asp:DropDownList ID="ddlcollege" runat="server" CssClass="textbox ddlstyle ddlheight3"
                                    OnSelectedIndexChanged="ddlcollege_SelectedIndexChanged" AutoPostBack="true"
                                    Width="199px">
                                </asp:DropDownList>
                            </td>
                            <td>
                               <asp:Label ID="lbl_batch" runat="server" Text="Batch"></asp:Label>
                           </td>
                           <td>
                                <asp:UpdatePanel ID="UP_batch" runat="server">
                                    <ContentTemplate>
                                        <asp:TextBox ID="txt_batch" runat="server" Style="height: 20px; width: 100px;" ReadOnly="true">--Select--</asp:TextBox>
                                        <asp:Panel ID="panel_batch" runat="server" CssClass="multxtpanel" Style="width: 150px;
                                            height: 200px;">
                                            <asp:CheckBox ID="cb_batch" runat="server" Width="100px" Text="Select All" AutoPostBack="True"
                                                OnCheckedChanged="cb_batch_OnCheckedChanged" />
                                            <asp:CheckBoxList ID="cbl_batch" runat="server" AutoPostBack="True" OnSelectedIndexChanged="cbl_batch_OnSelectedIndexChanged">
                                            </asp:CheckBoxList>
                                        </asp:Panel>
                                        <asp:PopupControlExtender ID="pce_batch" runat="server" TargetControlID="txt_batch"
                                            PopupControlID="panel_batch" Position="Bottom">
                                        </asp:PopupControlExtender>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>
                            <td>
                                <asp:Label ID="lbldeg" runat="server" Text="Degree"></asp:Label>
                            </td>
                            <td>
                                <asp:UpdatePanel ID="UP_degree" runat="server">
                                    <ContentTemplate>
                                        <asp:TextBox ID="txt_degree" runat="server" Style="height: 20px; width: 100px;" ReadOnly="true">--Select--</asp:TextBox>
                                        <asp:Panel ID="panel_degree" runat="server" CssClass="multxtpanel" Style="width: 150px;
                                            height: 200px;">
                                            <asp:CheckBox ID="cb_degree" runat="server" Width="100px" Text="Select All" AutoPostBack="True"
                                                OnCheckedChanged="cb_degree_OnCheckedChanged" />
                                            <asp:CheckBoxList ID="cbl_degree" runat="server" AutoPostBack="True" OnSelectedIndexChanged="cbl_degree_OnSelectedIndexChanged">
                                            </asp:CheckBoxList>
                                        </asp:Panel>
                                        <asp:PopupControlExtender ID="pce_degree" runat="server" TargetControlID="txt_degree"
                                            PopupControlID="panel_degree" Position="Bottom">
                                        </asp:PopupControlExtender>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>
                            <td>
                                <asp:Label ID="lbldept" runat="server" Text="Department"></asp:Label>
                            </td>
                            <td>
                                <asp:UpdatePanel ID="Up_dept" runat="server">
                                    <ContentTemplate>
                                        <asp:TextBox ID="txt_dept" runat="server" Style="height: 20px; width: 100px;" ReadOnly="true">--Select--</asp:TextBox>
                                        <asp:Panel ID="panel_dept" runat="server" CssClass="multxtpanel" Style="width: 250px;
                                            height: 300px;">
                                            <asp:CheckBox ID="cb_dept" runat="server" Width="100px" Text="Select All" AutoPostBack="True"
                                                OnCheckedChanged="cb_dept_OnCheckedChanged" />
                                            <asp:CheckBoxList ID="cbl_dept" runat="server" AutoPostBack="True" OnSelectedIndexChanged="cbl_dept_OnSelectedIndexChanged">
                                            </asp:CheckBoxList>
                                        </asp:Panel>
                                        <asp:PopupControlExtender ID="pce_dept" runat="server" TargetControlID="txt_dept"
                                            PopupControlID="panel_dept" Position="Bottom">
                                        </asp:PopupControlExtender>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="lblleve" runat="server" Text="Leave Category"></asp:Label>
                            </td>
                            <td>
                                <asp:UpdatePanel ID="UpdatePanel5" runat="server">
                                    <ContentTemplate>
                                        <asp:TextBox ID="txtleve" runat="server" Style="height: 20px; width: 150px;" ReadOnly="true">--Select--</asp:TextBox>
                                        <asp:Panel ID="pnlleve" runat="server" CssClass="multxtpanel" Style="width: 160px;
                                            height: 210px;">
                                            <asp:CheckBox ID="cbleve" runat="server" Width="150px" Text="Select All" AutoPostBack="True"
                                                OnCheckedChanged="cbleve_OnCheckedChanged" />
                                            <asp:CheckBoxList ID="cblleve" runat="server" AutoPostBack="True" OnSelectedIndexChanged="cblleve_OnSelectedIndexChanged">
                                            </asp:CheckBoxList>
                                        </asp:Panel>
                                        <asp:PopupControlExtender ID="PopupControlExtender6" runat="server" TargetControlID="txtleve"
                                            PopupControlID="pnlleve" Position="Bottom">
                                        </asp:PopupControlExtender>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>
                            <td colspan="3">
                                <asp:Label ID="lbldate" runat="server" Text="Date"></asp:Label>
                                <asp:TextBox ID="txt_fromdate" runat="server" Style="height: 20px; width: 75px;"></asp:TextBox>
                                <asp:CalendarExtender ID="CalendarExtender1" TargetControlID="txt_fromdate" runat="server"
                                    Format="dd/MM/yyyy" CssClass="cal_Theme1 ajax__calendar_active">
                                </asp:CalendarExtender>
                                <asp:CheckBox ID="cbperct" runat="server" Checked="true" Text="Percentage(%)" />
                            </td>
                            <td>
                                <asp:Button ID="btngo" runat="server" CssClass="textbox btn2" Width="56px" Text="Go"
                                    OnClick="btngo_Click" />
                            </td>
                        </tr>
                    </table>
                    <br />
                    <asp:Panel ID="pnlContents" runat="server" Visible="false">
                        <style type="text/css" media="print">
                            @page
                            {
                                size: A3 portrait;
                                margin: 0.5cm;
                            }
                            @media print
                            {
                                .printclass
                                {
                                    display: table;
                                }
                                thead
                                {
                                    display: table-header-group;
                                }
                                tfoot
                                {
                                    display: table-footer-group;
                                }
                                #header
                                {
                                    position: fixed;
                                    top: 0px;
                                    left: 0px;
                                }
                                #footer
                                {
                                    position: fixed;
                                    bottom: 0px;
                                    left: 0px;
                                }
                                #printable
                                {
                                    position: relative;
                                    bottom: 30px;
                                    height: 300;
                                }
                            
                            }
                            @media screen
                            {
                                thead
                                {
                                    display: block;
                                }
                                tfoot
                                {
                                    display: block;
                                }
                            }
                        </style>
                        <div id="printable">
                            <table>
                                <thead>
                                    <tr>
                                        <th>
                                            <div style="margin: 0px; border: 0px;">
                                                <table class="printclass" style="width: 100%; font-weight: bold; font-family: Book Antiqua;
                                                    font-size: medium; margin: 0px; margin-top: 20px;">
                                                    <tr>
                                                        <td rowspan="6" style="width: 80px; margin: 0px; border: 0px;">
                                                            <asp:Image ID="imgLeftLogo" runat="server" AlternateText="" ImageUrl="~/college/Left_Logo.jpeg"
                                                                Width="80px" Height="100px" Style="margin: 0px; border: 0px;" />
                                                        </td>
                                                        <td align="center">
                                                            <span id="spCollege" runat="server" style="font-size: 18px;"></span>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td align="center">
                                                            <span id="spAffBy" runat="server" style="font-size: 15px;"></span>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td align="center">
                                                            <span id="spController" runat="server" style="font-size: 15px;"></span>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td align="center">
                                                            <span id="spSeating" runat="server" style="font-size: 15px;"></span>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td align="center" colspan="2">
                                                            <span id="spDateSession" runat="server" style="font-size: 14px; display:none;"></span>                                                           
                                                            <span id="sprptnamedt" runat="server" style="font-size: 14px;"></span>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td align="right" colspan="2">
                                                            Date: <span id="spdate" runat="server" style="font-size: 14px;"></span>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </div>
                                        </th>
                                    </tr>
                                    <tr>
                                        <td colspan="2" style="display: none;">
                                            <center>
                                                <div>
                                                    <asp:Table ID="tblFormat2" runat="server" Style="width: 1417px; border-color: Black;
                                                        text-align: center; border-bottom: 0px solid black; font-weight: bold; font-size: medium;
                                                        border-style: solid; border-width: 1px;">
                                                        <asp:TableRow ID="tblRow1" runat="server">
                                                            <asp:TableCell ID="tblcellsno" runat="server" Text="S.No" Width="30px"></asp:TableCell>
                                                            <asp:TableCell ID="tblcellInvName" runat="server" Text="Invigilator Name" Width="69px"></asp:TableCell>
                                                            <asp:TableCell ID="tblcellHallNo" runat="server" Text="Hall No" Width="65px"></asp:TableCell>
                                                            <asp:TableCell ID="tcInvSign" runat="server" Text="Initials of the Invigilator" Width="65px"></asp:TableCell>
                                                            <asp:TableCell ID="TableCell4" runat="server" Text="Degree/Branch" Width="105px"></asp:TableCell>
                                                            <asp:TableCell ID="TableCell6" runat="server" Text="Subject Code" Width="80px"></asp:TableCell>
                                                            <asp:TableCell ID="TableCell7" runat="server" Text="Reg. No of the Candidate" Width="380px"></asp:TableCell>
                                                            <asp:TableCell ID="TableCell8" runat="server" Text="Total No of Student" Width="70px"></asp:TableCell>
                                                            <asp:TableCell ID="tcBooletNo" runat="server" Text="Answer Booklet Numbers" Width="40px"></asp:TableCell>
                                                            <asp:TableCell ID="tcHallSuperend" runat="server" Text="Signature <br/>of the<br/> Hall <br/>Superintendents"
                                                                Width="40px"></asp:TableCell>
                                                            <asp:TableCell ID="TableCell11" runat="server" Text="Present" Width="55px"></asp:TableCell>
                                                            <asp:TableCell ID="TableCell12" runat="server" Text="Absent" Width="55px"></asp:TableCell>
                                                            <asp:TableCell ID="TableCell13" runat="server" Text="Initials<br/> of the<br/> Invigilator"
                                                                Width="65px"></asp:TableCell>
                                                        </asp:TableRow>
                                                    </asp:Table>
                                                </div>
                                            </center>
                                        </td>
                                    </tr>
                                </thead>
                                <tbody>
                                    <tr>
                                        <td colspan="2" align="center">
                                            <asp:Label ID="lblmark" runat="server" Text="Not Entered" Visible="false"></asp:Label>
                                            <asp:GridView ID="gdattrpt" runat="server" AutoGenerateColumns="true" GridLines="Both"
                                                CssClass="grid-view" BackColor="WhiteSmoke" Style="width: auto;" OnRowDataBound="gdattrpt_OnRowDataBound"
                                                OnRowCreated="gdattrpt_OnRowCreated" OnDataBound="gdattrpt_OnDataBound">
                                            </asp:GridView>
                                        </td>
                                    </tr>
                                </tbody>
                            </table>
                        </div>
                          <center><asp:Label ID="lblCount" runat="server"></asp:Label></center>
                    </asp:Panel>
                    <br />
                    <br />
                  
                    <asp:Button ID="btnExport" runat="server" Style="font-family: Book Antiqua; font-weight: bold;"
                        Text="Export To PDF" Visible="false" OnClientClick=" return PrintPanel()" />
                </div>
            </center>
        </div>
    </body>
    </html>
</asp:Content>

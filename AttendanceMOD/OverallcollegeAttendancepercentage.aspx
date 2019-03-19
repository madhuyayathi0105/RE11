<%@ Page Title="" Language="C#" MasterPageFile="~/AttendanceMOD/AttendanceSubSiteMaster.master" AutoEventWireup="true" CodeFile="OverallcollegeAttendancepercentage.aspx.cs" Inherits="OverallcollegeAttendancepercentage" %>


<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Assembly="FarPoint.Web.Spread" Namespace="FarPoint.Web.Spread" TagPrefix="FarPoint" %>
<%@ Register Src="~/Usercontrols/PrintMaster.ascx" TagName="PRINTPDF" TagPrefix="Insproplus" %>


<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
<style type="text/css">
        .fontbold
        {
            font-family: Book Antiqua;
            font-size: medium;
            font-weight: bold;
        }
        .fontnormal
        {
            font-family: Book Antiqua;
            font-size: medium;
        }
        .printclass
        {
            display: none;
        }
    </style>
    <style type="text/css">
        .fontStyle
        {
            font-size: medium;
            font-weight: bolder;
            font-style: oblique;
            padding: 5px;
        }
        .fontStyle1
        {
            font-size: medium;
            font-style: oblique;
            padding: 3px;
            color: Blue;
        }
        .commonHeaderFont
        {
            font-size: medium;
            color: Black;
            font-family: 'Book Antiqua';
            font-weight: bold;
        }
    </style>
    <script type="text/javascript">
        function PrintPanel1() {
            var panel = document.getElementById("<%=pnlContent1.ClientID %>");
            var printWindow = window.open('', '', 'height=842,width=1191');

            printWindow.document.write('<html');
            printWindow.document.write('<head><title>DespatchOfAnswerPackets</title>');
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
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" Runat="Server">
<asp:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server">
    </asp:ToolkitScriptManager>
    <center>
        <span class="fontstyleheader" style="color: Green; margin: 0px; margin-bottom: 10px;
            margin-top: 10px; position: relative;">Attendance Presentage </span>
    </center>
     <br />
     <div class="maindivstyle maindivstylesize">
     <br />
     <center>
        <table class="maintablestyle" style="margin: 0px; margin-bottom: 10px; margin-top: 10px;
            position: relative;">
            <tr>
                
               <td>
                        <asp:Label ID="lblCollege" runat="server" Text="College" CssClass="commonHeaderFont"
                            AssociatedControlID="txtCollege"></asp:Label>
                            </td>
                            <td>
                        <div style="position: relative;">
                            <asp:UpdatePanel ID="upnlCollege" runat="server">
                                <ContentTemplate>
                                    <asp:TextBox ID="txtCollege" Visible="true" Width="104px" runat="server" CssClass="textbox  txtheight2 commonHeaderFont"
                                        ReadOnly="true">-- Select --</asp:TextBox>
                                    <asp:Panel ID="pnlCollege" Visible="true" runat="server" CssClass="multxtpanel" Height="200px"
                                        Width="280px">
                                        <asp:CheckBox ID="chkCollege" CssClass="commonHeaderFont" runat="server" Text="Select All"
                                            AutoPostBack="True" OnCheckedChanged="chkCollege_CheckedChanged" />
                                        <asp:CheckBoxList ID="cblCollege" CssClass="commonHeaderFont" runat="server" AutoPostBack="True"
                                            OnSelectedIndexChanged="cblCollege_SelectedIndexChanged">
                                        </asp:CheckBoxList>
                                    </asp:Panel>
                                    <asp:PopupControlExtender ID="popupExtCollege" runat="server" TargetControlID="txtCollege"
                                        PopupControlID="pnlCollege" Position="Bottom">
                                    </asp:PopupControlExtender>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </div>
                    </td>
                    <td>
                        <asp:Label ID="lblfromdate" runat="server" Text="From Date" CssClass="commonHeaderFont"
                            AssociatedControlID="txtfromDate"></asp:Label>
                    </td>
                     <td>
                      <asp:TextBox ID="txtfromDate" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                         OnTextChanged="txtfromDate_TextChanged" Font-Size="Medium" AutoPostBack="true"
                         Width="75px"></asp:TextBox>
                      <asp:CalendarExtender ID="CalendarExtender2" TargetControlID="txtfromDate" Format="dd-MM-yyyy"
                              runat="server">
                       </asp:CalendarExtender>
                       </td>
                        <td>
                           <asp:Label ID="Lbltodate" runat="server" Text="From Date" CssClass="commonHeaderFont"
                              AssociatedControlID="txttoDate"></asp:Label>
                      </td>
                      <td>
                         <asp:TextBox ID="txttoDate" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                              OnTextChanged="txttoDate_TextChanged" Font-Size="Medium" AutoPostBack="true"
                              Width="75px"></asp:TextBox>
                      <asp:CalendarExtender ID="CalendarExtender1" TargetControlID="txttoDate" Format="dd-MM-yyyy"
                              runat="server">
                       </asp:CalendarExtender>
                       </td>
                       <td colspan="2" align="right">
                        <asp:Button ID="btnattOk" runat="server" Text="Go" OnClick="btngo_Click" Font-Bold="True"
                            Font-Names="Book Antiqua" Font-Size="Medium" />
                            </td>
                       </tr>
                       </table>
                       </center>
                       <center>
                           <asp:Label ID="lblnorec" runat="server" Text="No Record(s) Found" ForeColor="Red"
                    Visible="False" Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium"></asp:Label>
                       </center>
                       <br />
                       
                        <div id="divFormat1" runat="server" visible="true">
                     <center>
                       <asp:Panel ID="pnlContent1" runat="server" Visible="false">
                       <center>
                        <table>
                    <tr>
                        <td>
                            
                               <%-- <FarPoint:FpSpread ID="Fpspread" runat="server" BorderColor="Black" BorderStyle="Solid"
                                    BorderWidth="1px" Width="700" Style="margin: 0px; margin-bottom: 10px; margin-top: 10px;
                                    position: relative;"   Visible="true" CommandBar-Visible="false" ShowHeaderSelection="false">
                                    <CommandBar BackColor="Control" ButtonFaceColor="Control" ButtonHighlightColor="ControlLightLight"
                                        ButtonShadowColor="ControlDark">
                                    </CommandBar>
                                    <Sheets>
                                        <FarPoint:SheetView SheetName="Sheet" AutoPostBack="true" GridLineColor="Black">
                                        </FarPoint:SheetView>
                                    </Sheets>
                                </FarPoint:FpSpread>--%>
                                   <FarPoint:FpSpread ID="Fpspread" runat="server" Visible="true" BorderWidth="4px" Style="margin-left:2px;"                                  BorderStyle="Solid" BorderColor="Black"  CssClass="spreadborder" ActiveSheetViewIndex="0" >
                    <Sheets>
                        <FarPoint:SheetView  PageSize="100" SheetName="Sheet" SelectionBackColor="#0CA6CA">
                        </FarPoint:SheetView>
                    </Sheets>
                </FarPoint:FpSpread>
                               
                        </td>
                    </tr>
                    </table>
                    </center>
                    <br />

                   <%-- <center>
            <asp:Button ID="btn_directprint" runat="server" CssClass="fontbold" Width="100px"
                Text="Direct Print"  OnClientClick="return PrintPanel1();"  />
        </center>--%>

        </asp:Panel>
        </center>
        
                    </div>
                    <center>
                     <asp:Label ID="lblrptname" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                    Font-Size="Medium" Text="Report Name"></asp:Label>
                <asp:TextBox ID="txtexcelname" runat="server" Height="20px" Width="180px" Style="font-family: 'Book Antiqua'"
                    Font-Bold="True" onkeypress="display()" Font-Names="Book Antiqua" Font-Size="Medium"></asp:TextBox>
                <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" runat="server" TargetControlID="txtexcelname"
                    FilterType="LowercaseLetters,UppercaseLetters,Numbers,Custom" ValidChars="!@$%^&*()_+|\}{][':;?><,./">
                </asp:FilteredTextBoxExtender>
                <asp:Button ID="btnxl" runat="server" Text="Export Excel" Font-Bold="True" Font-Names="Book Antiqua"
                    Font-Size="Medium" OnClick="btnxl_Click" />
                <asp:Button ID="btnprintmaster" runat="server" Text="Print" OnClick="btnprintmaster_Click"
                    Font-Names="Book Antiqua" Font-Size="Medium" Font-Bold="true" />
                <Insproplus:PRINTPDF runat="server" ID="Printcontrol" Visible="false" />
                </center>
                    </div>

</asp:Content>


<%@ Page Title="" Language="C#" MasterPageFile="~/AttendanceMOD/AttendanceSubSiteMaster.master"
    AutoEventWireup="true" CodeFile="dailystaffreport.aspx.cs" Inherits="CITreport"
    EnableEventValidation="false" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Src="~/Usercontrols/NewPrintMaster.ascx" TagName="NEWPrintMater" TagPrefix="NEW" %>
<%@ Register Src="~/Usercontrols/PrintMaster.ascx" TagName="printmaster" TagPrefix="Insproplus" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <script type="text/javascript">
        window.scrollTo = function (x, y) {
            return true;
        }
    </script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">
    <body>
     <asp:UpdatePanel ID="UpdatePanel3" runat="server">
            <ContentTemplate>
       
        <asp:Label ID="Msg" runat="server"></asp:Label>
        <asp:ScriptManager ID="ScriptManager1" runat="server">
        </asp:ScriptManager>
        <center>
            <br />
            <span class="fontstyleheader" style="color: Green;">AT24-Daywise Staff Task Performance
                Report</span>
            <br />
            <br />
            <table class="maintablestyle">
                <tr>
                    <td>
                        <asp:Label ID="lbldept" runat="server" Text="Department" Font-Bold="True" Font-Names="Book Antiqua"
                            Font-Size="Medium" Style=""></asp:Label>
                    </td>
                    <td>
                        <asp:UpdatePanel ID="UpdatePanel5" runat="server">
                            <ContentTemplate>
                                <asp:TextBox ID="txtdept" runat="server" CssClass="Dropdown_Txt_Box" ReadOnly="true"
                                    Width="140px" Height="20px" Font-Bold="True" Font-Names="Book Antiqua" Style=""
                                    Font-Size="Medium">---Select---</asp:TextBox>
                                <asp:Panel ID="pdept" runat="server" CssClass="multxtpanel" Height="349px" Width="461px"
                                    BackColor="White" BorderColor="Black" BorderStyle="Solid" ScrollBars="Auto" Style="font-family: 'Book Antiqua'">
                                    <asp:CheckBox ID="chkdept" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                        Font-Size="Medium" Text="Select All" AutoPostBack="true" OnCheckedChanged="chkdept_CheckedChanged" />
                                    <asp:CheckBoxList ID="chklstdept" runat="server" Font-Size="Medium" Font-Bold="True"
                                        Font-Names="Book Antiqua" AutoPostBack="true" OnSelectedIndexChanged="chklstdept_SelectedIndexChanged">
                                    </asp:CheckBoxList>
                                </asp:Panel>
                                <asp:PopupControlExtender ID="PopupControlExtender2" runat="server" TargetControlID="txtdept"
                                    PopupControlID="pdept" Position="Bottom">
                                </asp:PopupControlExtender>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </td>
                    <td>
                        <asp:Label runat="server" ID="lblstaffcode" Text="Staff Code" Font-Bold="True" Font-Names="Book Antiqua"
                            Font-Size="Medium" Style="font-family: Book Antiqua; font-size: medium; font-weight: bold;"></asp:Label>
                    </td>
                    <td>
                        <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                            <ContentTemplate>
                                <asp:TextBox ID="txtstaff" runat="server" CssClass="Dropdown_Txt_Box" ReadOnly="true"
                                    Width="100px" Style="font-size: medium; font-weight: bold; font-family: 'Book Antiqua';
                                    width: 100px; height: 20px;">---Select---</asp:TextBox>
                                <asp:Panel ID="pstaff" runat="server" Height="213px" Width="130px" CssClass="multxtpanel"
                                    BackColor="White" BorderColor="Black" BorderStyle="Solid" ScrollBars="Auto" Style="font-family: 'Book Antiqua'">
                                    <asp:CheckBox ID="chkstaff" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                        Font-Size="Medium" OnCheckedChanged="chkstaff_CheckedChanged" Text="Select All"
                                        AutoPostBack="True" />
                                    <asp:CheckBoxList ID="chklststaff" runat="server" Style="font-family: Book Antiqua;
                                        font-size: medium; font-weight: bold; height: 37px; text-align: left;" Font-Size="Medium"
                                        AutoPostBack="True" OnSelectedIndexChanged="chklststaff_SelectedIndexChanged"
                                        Height="200px" Font-Bold="True" Font-Names="Book Antiqua">
                                    </asp:CheckBoxList>
                                </asp:Panel>
                                <asp:PopupControlExtender ID="PopupControlExtender3" runat="server" TargetControlID="txtstaff"
                                    PopupControlID="pstaff" Position="Bottom">
                                </asp:PopupControlExtender>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </td>
                    <td>
                        <asp:Label runat="server" ID="lbldate" Text="Date" Font-Bold="True" Font-Names="Book Antiqua"
                            Font-Size="Medium" Style="font-family: Book Antiqua; font-size: medium; font-weight: bold;"></asp:Label>
                        <asp:TextBox ID="txtdate" runat="server" Font-Bold="true" AutoPostBack="true" Width="80px"
                            ForeColor="Black" Font-Size="Medium" Font-Names="Book Antiqua" Style=""></asp:TextBox>
                        <asp:CalendarExtender ID="CalendarExtender2" TargetControlID="txtdate" runat="server"
                            Format="dd/MM/yyyy">
                        </asp:CalendarExtender>
                        <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender3" runat="server" TargetControlID="txtdate"
                            FilterType="Numbers,Custom" ValidChars="/">
                        </asp:FilteredTextBoxExtender>
                    </td>
                    <td>
                        <asp:Button ID="btnGo" runat="server" OnClick="btnGo_Click" Text="Go" Style="font-family: Book Antiqua;
                            font-size: medium; height: 27px; width: 36px; font-weight: 700;" />
                    </td>
                </tr>
            </table>
            <br />
            <asp:Label ID="lblerr" runat="server" ForeColor="Red" Font-Bold="True" Font-Names="Book Antiqua"
                Font-Size="Medium" Visible="false"></asp:Label>
            <br />
    

            <center>
           
                       

                              <div ID="divgrid" runat="server" visible="false" style ="height:400px; width:593px; overflow:auto; border: solid 1px black;">
                              <asp:GridView ID="Showgrid" runat="server" Visible="false" HeaderStyle-ForeColor="Black"
                    HeaderStyle-BackColor="#0CA6CA" BorderColor="Black" >
                
                            <Columns>
                            <asp:TemplateField HeaderText="Select" HeaderStyle-BackColor="#0CA6CA" HeaderStyle-HorizontalAlign="center"
                            HeaderStyle-Width="30px">
                            <ItemTemplate>
                                <center>

                                     <asp:CheckBox ID="chkselectall" runat="server" Width="30px" AutoPostBack="true" OnCheckedChanged="chkselectall_CheckedChanged"></asp:CheckBox>
                                    <asp:CheckBox ID="lbl_cb" runat="server" Width="30px" ></asp:CheckBox>
                                </center>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="right" />
                        </asp:TemplateField>
                            </Columns>
                           
                            
                        </asp:GridView>

                         </div>


                    
                </center>
                <br />
            <asp:Button ID="btngenerate" runat="server" Text="Generate" Font-Bold="True" Font-Names="Book Antiqua"
                Font-Size="Medium" OnClick="btngenerate_click" />
            <br />
        </center>

         </ContentTemplate>
        <Triggers>
            <asp:PostBackTrigger ControlID="btnGo" />
            <asp:PostBackTrigger ControlID="btngenerate" />
            
        </Triggers>
    </asp:UpdatePanel>
    </body>
</asp:Content>

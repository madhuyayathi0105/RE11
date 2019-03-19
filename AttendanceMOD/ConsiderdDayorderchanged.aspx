<%@ Page Title="" Language="C#" MasterPageFile="~/AttendanceMOD/AttendanceSubSiteMaster.master"
    AutoEventWireup="true" CodeFile="ConsiderdDayorderchanged.aspx.cs" Inherits="ConsiderdDayorderchanged" %>

<%@ Register Assembly="FarPoint.Web.Spread" Namespace="FarPoint.Web.Spread" TagPrefix="FarPoint" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Src="~/Usercontrols/PrintMaster.ascx" TagName="PRINTPDF" TagPrefix="Insproplus" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <script type="text/javascript">
        function frelig() {

            document.getElementById('<%=btnreasonadd.ClientID%>').style.display = 'block';
            document.getElementById('<%=btnreasondelete.ClientID%>').style.display = 'block';
        }
    </script>
    <script type="text/javascript">
        function PrintDiv() {
            var panel = document.getElementById("<%=contentDiv.ClientID %>");
            var printWindow = window.open('', '', 'height=auto,width=685');
            printWindow.document.write('<html');
            printWindow.document.write('<head> <style type="text/css"> p{ font-size: x-small;margin: 0px; padding: 0px; border: 0px;  } body{ margin:0px;}</style>');
            printWindow.document.write('</head><body>');
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
    <script type="text/javascript">
        function display() {
            document.getElementById('MainContent_lblnorec').innerHTML = "";
        }
    </script>
    <style type="text/css">
        ody, input
        {
            font-family: Tahoma;
            font-size: 10px;
        }
        
        .modalBackground
        {
            background-color: Gray;
            filter: alpha(opacity=80);
            opacity: 0.8;
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
        
        .topHandle
        {
            background-color: #97bae6;
        }
        .floatr
        {
            border-style: solid;
            border-color: Black;
            border-width: thin;
            float: right;
        }
        
        
        .tabl
        {
            empty-cells: show;
            border-style: solid;
            border-color: Gray;
            border-width: thin;
            caption-side: top;
            font-family: MS Sans Serif;
            font-size: Small;
            font-style: normal;
            font-weight: normal;
        }
        .tablfont
        {
            empty-cells: show;
            border-style: solid;
            border-color: Gray;
            border-width: thin;
            caption-side: top;
            font-family: MS Sans Serif;
            font-size: Small;
            font-style: normal;
            font-weight: bold;
        }
        .tabl3
        {
            empty-cells: hide;
            border-style: solid;
            border-color: Black;
            border-width: thin;
            caption-side: top;
            font-family: MS Sans Serif;
            font-size: Small;
            font-style: normal;
            font-weight: bold;
            text-align: left;
        }
        .tabl5
        {
            border-style: solid;
            border-color: Black;
            border-width: thin;
            text-align: left;
        }
        .tabl1
        {
            empty-cells: show;
            caption-side: top;
            font-family: MS Sans Serif;
            font-size: Small;
            font-style: normal;
            font-weight: bold;
        }
        .linkbtn
        {
            border-color: White;
            border-style: none;
            background-color: transparent;
            cursor: pointer;
        }
        .HeaderSelectedCSS
        {
            color: white;
            background-color: #719DDB;
            font-weight: bold;
            font-size: medium; /* font-style:italic;  */
        }
        .HellowWorldPopup
        {
            min-width: 600px;
            min-height: 400px;
            background: white;
        }
        .ModalPopupBG
        {
            background-color: #666699;
            filter: alpha(opacity=50);
            opacity: 0.7;
        }
        
        
        .font
        {
            font-size: medium;
            font-family: Book Antiqua;
            font-weight: bold;
        }
        .HeaderCSS
        {
            color: white;
            background-color: #719DDB;
            font-size: small;
            font-weight: bold;
            height: 10px;
        }
        .cpBody
        {
            background-color: #DCE4F9;
        }
        .accordion
        {
            width: 300px;
        }
        
        .accordionHeader
        {
            border: 1px solid #2F4F4F;
            color: white;
            background-color: #2E4d7B;
            font-family: Arial, Sans-Serif;
            font-size: 12px;
            font-weight: bold;
            padding: 5px;
            margin-top: 5px;
            cursor: pointer;
        }
        
        .accordionHeaderSelected
        {
            border: 1px solid #2F4F4F;
            color: white;
            background-color: #5078B3;
            font-family: Arial, Sans-Serif;
            font-size: 12px;
            font-weight: bold;
            padding: 5px;
            margin-top: 5px;
            cursor: pointer;
        }
        
        .accordionContent
        {
            background-color: White;
            border: 1px dashed #2F4F4F;
            border-top: none;
            padding: 5px;
            padding-top: 10px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">
    <asp:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server">
    </asp:ToolkitScriptManager>
    <center>
        <asp:Label ID="Label31" runat="server" Text="Consider Day Order" Font-Bold="true"
            Font-Names="Book Antiqua" Font-Size="Large" ForeColor="Green"></asp:Label>
        <asp:UpdatePanel ID="UpdatePanel4" runat="server">
            <ContentTemplate>
                <asp:Accordion ID="Accordion1" CssClass="accordion" HeaderCssClass="accordionHeader"
                    HeaderSelectedCssClass="accordionHeaderSelected" ContentCssClass="accordionContent"
                    runat="server" Width="985px" Height="500px" BorderColor="White" Style="height: 500px;
                    margin: 0px; margin-bottom: 10px; margin-top: 20px; position: relative;">
                    <Panes>
                        <asp:AccordionPane ID="AccordionPane1" runat="server" Style="padding: 20px; margin: 20px;
                            height: 100%;">
                            <Header>
                                View
                            </Header>
                            <Content>
                                <div style="width: auto; height: auto;">
                                    <center>
                                        <table style="width: 700px; height: 70px; background-color: #0CA6CA; margin: 0px;
                                            margin-top: 0px; margin-bottom: 30px; position: relative;">
                                            <tr>
                                            <td >
                                                        <asp:Label ID="Label5" runat="server" Text="College" CssClass="font"></asp:Label>
                                                    </td>
                                                    <td >
                                                        <asp:UpdatePanel ID="UpdatePanel13" runat="server">
                                                            <ContentTemplate>
                                                                <div style="position: relative;">
                                                                     <asp:DropDownList ID="ddlCollege" AutoPostBack="true" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                            Font-Size="Medium" Width="182px" OnSelectedIndexChanged="ddlCollege_SelectedIndexChanged"></asp:DropDownList></div>
                                                            </ContentTemplate>
                                                        </asp:UpdatePanel>
                                                    </td>
                                                <td>
                                                    <asp:Label ID="lblbatch" runat="server" Text="Batch" CssClass="font"></asp:Label>
                                                </td>
                                                <td>
                                                    <div style="position: relative;">
                                                        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                                                            <ContentTemplate>
                                                                <asp:TextBox ID="txtbatch" runat="server" CssClass="Dropdown_Txt_Box" ReadOnly="true"
                                                                    Width="100px" Style="font-family: 'Book Antiqua';" Font-Bold="True" Font-Names="Book Antiqua"
                                                                    Font-Size="Medium">---Select---</asp:TextBox>
                                                                <asp:Panel ID="pbatch" runat="server" CssClass="multxtpanel" BackColor="White" BorderColor="Black"
                                                                    BorderStyle="Solid" BorderWidth="1px" ScrollBars="Vertical" Height="150px" Width="125px">
                                                                    <asp:CheckBox ID="chkbatch" runat="server" Font-Bold="True" OnCheckedChanged="chkbatch_ChekedChange"
                                                                        Font-Names="Book Antiqua" Font-Size="Medium" Text="Select All" AutoPostBack="True" />
                                                                    <asp:CheckBoxList ID="chklsbatch" runat="server" Font-Size="Medium" AutoPostBack="True"
                                                                        Font-Bold="True" Font-Names="Book Antiqua" OnSelectedIndexChanged="chklsbatch_SelectedIndexChanged">
                                                                    </asp:CheckBoxList>
                                                                </asp:Panel>
                                                                <asp:PopupControlExtender ID="PopupControlExtender1" runat="server" TargetControlID="txtbatch"
                                                                    PopupControlID="pbatch" Position="Bottom">
                                                                </asp:PopupControlExtender>
                                                            </ContentTemplate>
                                                        </asp:UpdatePanel>
                                                    </div>
                                                </td>
                                                <td>
                                                    <asp:Label ID="lbldegree" runat="server" Text="Degree" CssClass="font"></asp:Label>
                                                </td>
                                                <td>
                                                    <div style="position: relative;">
                                                        <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                                                            <ContentTemplate>
                                                                <asp:TextBox ID="txtdegree" runat="server" CssClass="Dropdown_Txt_Box" ReadOnly="true"
                                                                    Width="100px" Style="font-family: 'Book Antiqua';" Font-Bold="True" Font-Names="Book Antiqua"
                                                                    Font-Size="Medium">---Select---</asp:TextBox>
                                                                <asp:Panel ID="pdegree" runat="server" CssClass="multxtpanel" BackColor="White" BorderColor="Black"
                                                                    BorderStyle="Solid" BorderWidth="1px" ScrollBars="Vertical" Height="150px" Width="150px">
                                                                    <asp:CheckBox ID="chkdegree" runat="server" Font-Bold="True" OnCheckedChanged="chkdegree_ChekedChange"
                                                                        Font-Names="Book Antiqua" Font-Size="Medium" Text="Select All" AutoPostBack="True" />
                                                                    <asp:CheckBoxList ID="chklsdegree" runat="server" Font-Size="Medium" AutoPostBack="True"
                                                                        Font-Bold="True" Font-Names="Book Antiqua" OnSelectedIndexChanged="chklsdegree_SelectedIndexChanged">
                                                                    </asp:CheckBoxList>
                                                                </asp:Panel>
                                                                <asp:PopupControlExtender ID="PopupControlExtender2" runat="server" TargetControlID="txtdegree"
                                                                    PopupControlID="pdegree" Position="Bottom">
                                                                </asp:PopupControlExtender>
                                                            </ContentTemplate>
                                                        </asp:UpdatePanel>
                                                    </div>
                                                </td>
                                                <td>
                                                    <asp:Label ID="lblbranch" runat="server" Text="Branch" CssClass="font"></asp:Label>
                                                </td>
                                                <td>
                                                    <div style="position: relative;">
                                                        <asp:UpdatePanel ID="UpdatePanel3" runat="server">
                                                            <ContentTemplate>
                                                                <asp:TextBox ID="txtbranch" runat="server" CssClass="Dropdown_Txt_Box" ReadOnly="true"
                                                                    Width="100px" Style="font-family: 'Book Antiqua';" Font-Bold="True" Font-Names="Book Antiqua"
                                                                    Font-Size="Medium">---Select---</asp:TextBox>
                                                                <asp:Panel ID="pbranch" runat="server" CssClass="multxtpanel" BackColor="White" BorderColor="Black"
                                                                    BorderStyle="Solid" BorderWidth="1px" ScrollBars="Vertical" Height="250px" Width="250px">
                                                                    <asp:CheckBox ID="chkbranch" runat="server" Font-Bold="True" OnCheckedChanged="chkbranch_ChekedChange"
                                                                        Font-Names="Book Antiqua" Font-Size="Medium" Text="Select All" AutoPostBack="True" />
                                                                    <asp:CheckBoxList ID="chklsbranch" runat="server" Font-Size="Medium" AutoPostBack="True"
                                                                        Font-Bold="True" Font-Names="Book Antiqua" OnSelectedIndexChanged="chklsbranch_SelectedIndexChanged">
                                                                    </asp:CheckBoxList>
                                                                </asp:Panel>
                                                                <asp:PopupControlExtender ID="PopupControlExtender3" runat="server" TargetControlID="txtbranch"
                                                                    PopupControlID="pbranch" Position="Bottom">
                                                                </asp:PopupControlExtender>
                                                            </ContentTemplate>
                                                        </asp:UpdatePanel>
                                                    </div>
                                                </td>
                                                  </tr>
                                            <tr>
                                                <td>
                                                    <asp:Label ID="lblreason" runat="server" Text="Reason" CssClass="font"></asp:Label>
                                                </td>
                                                <td>
                                                    <div style="position: relative;">
                                                        <asp:UpdatePanel ID="UpdatePanel7" runat="server">
                                                            <ContentTemplate>
                                                                <asp:TextBox ID="txtreason" runat="server" CssClass="Dropdown_Txt_Box" ReadOnly="true"
                                                                    Width="100px" Style="font-family: 'Book Antiqua';" Font-Bold="True" Font-Names="Book Antiqua"
                                                                    Font-Size="Medium">---Select---</asp:TextBox>
                                                                <asp:Panel ID="preason" runat="server" CssClass="multxtpanel" BackColor="White" BorderColor="Black"
                                                                    BorderStyle="Solid" BorderWidth="1px" ScrollBars="Vertical" Height="150px" Width="200px">
                                                                    <asp:CheckBox ID="chkreason" runat="server" Font-Bold="True" OnCheckedChanged="chkreason_ChekedChange"
                                                                        Font-Names="Book Antiqua" Font-Size="Medium" Text="Select All" AutoPostBack="True" />
                                                                    <asp:CheckBoxList ID="chklsreason" runat="server" Font-Size="Medium" AutoPostBack="True"
                                                                        Font-Bold="True" Font-Names="Book Antiqua" OnSelectedIndexChanged="chklsreason_SelectedIndexChanged">
                                                                    </asp:CheckBoxList>
                                                                </asp:Panel>
                                                                <asp:PopupControlExtender ID="PopupControlExtender7" runat="server" TargetControlID="txtreason"
                                                                    PopupControlID="preason" Position="Bottom">
                                                                </asp:PopupControlExtender>
                                                            </ContentTemplate>
                                                        </asp:UpdatePanel>
                                                    </div>
                                                </td>
                                          
                                                <td>
                                                    <asp:CheckBox ID="chkdate" runat="server" AutoPostBack="true" OnCheckedChanged="chkdate_CheckedChanged" />
                                                </td>
                                                <td>
                                                    <asp:Label ID="lblfromdate" runat="server" Text="From Date" CssClass="font"></asp:Label>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtfromdate" runat="server" CssClass="font" Width="98px" AutoPostBack="true"
                                                        OnTextChanged="txtfromdate_TextChanged"></asp:TextBox>
                                                    <asp:CalendarExtender ID="CalendarExtender1" runat="server" Format="dd/MM/yyyy" TargetControlID="txtfromdate">
                                                    </asp:CalendarExtender>
                                                </td>
                                                <td>
                                                    <asp:Label ID="lbltodate" runat="server" Text="To Date" CssClass="font"></asp:Label>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txttodate" runat="server" CssClass="font" Width="98px" AutoPostBack="true"
                                                        OnTextChanged="txttodate_TextChanged"></asp:TextBox>
                                                    <asp:CalendarExtender ID="CalendarExtender2" runat="server" Format="dd/MM/yyyy" TargetControlID="txttodate">
                                                    </asp:CalendarExtender>
                                                </td>
                                                <td>
                                                    <asp:Button ID="btngo" runat="server" CssClass="font" Text="Go" OnClick="btngo_Click" />
                                                </td>
                                            </tr>
                                        </table>
                                    </center>
                                    <center>
                                        <asp:Label ID="lblnorec" runat="server" Visible="false" ForeColor="Red" CssClass="font"
                                            Style="margin: 0px; margin-top: 10px; margin-bottom: 30px; position: relative;"></asp:Label>
                                    </center>
                                    <center>
                                        <div>
                                            <FarPoint:FpSpread ID="FpSpread1" runat="server" Height="250px" Width="400px" ActiveSheetViewIndex="0"
                                                OnCellClick="FpSpread1_CellClick" OnPreRender="FpSpread1_SelectedIndexChanged"
                                                currentPageIndex="0" DesignString="&lt;?xml version=&quot;1.0&quot; encoding=&quot;utf-8&quot;?&gt;&lt;Spread /&gt;"
                                                EnableClientScript="False" CssClass="cursorptr" BorderColor="Black" BorderWidth="0.5"
                                                Style="margin: 0px; margin-top: 20px; margin-bottom: 20px; position: relative;">
                                                <CommandBar BackColor="Control" ButtonType="PushButton">
                                                    <Background BackgroundImageUrl="SPREADCLIENTPATH:/img/cbbg.gif" />
                                                </CommandBar>
                                                <Pager Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                                    Font-Underline="False" />
                                                <HierBar Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                                    Font-Underline="False" />
                                                <Sheets>
                                                    <FarPoint:SheetView SheetName="Sheet1" EditTemplateColumnCount="2" GridLineColor="Black"
                                                        GroupBarText="Drag a column to group by that column." SelectionBackColor="#CE5D5A"
                                                        SelectionForeColor="White">
                                                    </FarPoint:SheetView>
                                                </Sheets>
                                                <TitleInfo BackColor="#E7EFF7" Font-Size="X-Large" ForeColor="" HorizontalAlign="Center"
                                                    VerticalAlign="NotSet" Font-Bold="False" Font-Italic="False" Font-Overline="False"
                                                    Font-Strikeout="False" Font-Underline="False">
                                                </TitleInfo>
                                            </FarPoint:FpSpread>
                                        </div>
                                    </center>
                                    <center>
                                        <div style="margin: 0px; width: auto; height: auto; margin-top: 10px; margin-bottom: 30px;
                                            position: relative; text-align: center;">
                                            <asp:Label ID="lblrptname" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                                Font-Size="Medium" Text="Report Name"></asp:Label>
                                            <asp:TextBox ID="txtexcelname" runat="server" Height="20px" Width="180px" onkeypress="display()"
                                                Style="font-family: 'Book Antiqua'" Font-Bold="True" Font-Names="Book Antiqua"
                                                Font-Size="Medium"></asp:TextBox>
                                            <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender2" runat="server" TargetControlID="txtexcelname"
                                                FilterType="LowercaseLetters,UppercaseLetters,Numbers,Custom" ValidChars="!@$%^&()_+}{][';,.">
                                            </asp:FilteredTextBoxExtender>
                                            <asp:Button ID="btnxl" runat="server" Text="Export Excel" Font-Bold="True" Font-Names="Book Antiqua"
                                                Font-Size="Medium" OnClick="btnxl_Click" />
                                            <asp:Button ID="btnprintmaster" runat="server" Text="Print" OnClick="btnprintmaster_Click"
                                                Font-Names="Book Antiqua" Font-Size="Medium" Font-Bold="true" />
                                            <Insproplus:PRINTPDF runat="server" ID="Printcontrol" Visible="false" />
                                        </div>
                                    </center>
                                </div>
                            </Content>
                        </asp:AccordionPane>
                        <asp:AccordionPane ID="AccordionPane2" runat="server">
                            <Header>
                                Entry
                            </Header>
                            <Content>
                                <div style="margin: 0px; width: auto; height: 335px; margin-top: 20px; margin-bottom: 30px;
                                    position: relative; text-align: center;">
                                    <center>
                                        <div>
                                            <table   style="margin: 0px; margin-top: 10px; margin-bottom: 10px; position: relative;
                                                padding: 5px; width: auto; height: auto;">
                                                <tr>
                                                 <td style="padding-right: 5px;">
                                                        <asp:Label ID="Label4" runat="server" Text="College" CssClass="font"></asp:Label>
                                                    </td>
                                                    <td style="padding-right: 15px; padding-left: 5px;">
                                                        <asp:UpdatePanel ID="UpdatePanel12" runat="server">
                                                            <ContentTemplate>
                                                                <div style="position: relative;">
                                                                    <asp:DropDownList ID="ddlcoll" runat="server" Font-Bold="True" Font-Names="Book Antiqua" AutoPostBack="true"
                            Font-Size="Medium" Width="182px" OnSelectedIndexChanged="ddlcoll_SelectedIndexChanged"></asp:DropDownList>
                                                                </div>
                                                            </ContentTemplate>
                                                        </asp:UpdatePanel>
                                                    </td>
                                                    <td style="padding-right: 5px;">
                                                        <asp:Label ID="Label1" runat="server" Text="Batch" CssClass="font"></asp:Label>
                                                    </td>
                                                    <td style="padding-right: 15px; padding-left: 5px;">
                                                        <asp:UpdatePanel ID="UpdatePanel8" runat="server">
                                                            <ContentTemplate>
                                                                <div style="position: relative;">
                                                                    <asp:TextBox ID="txtbatchadd" runat="server" CssClass="Dropdown_Txt_Box" ReadOnly="true"
                                                                        Width="100px" Style="font-family: 'Book Antiqua';" Font-Bold="True" Font-Names="Book Antiqua"
                                                                        Font-Size="Medium">---Select---</asp:TextBox>
                                                                    <asp:Panel ID="pbatchadd" runat="server" CssClass="multxtpanel" Height="250px" Style="width: 100%;">
                                                                        <asp:CheckBox ID="chkbatchadd" runat="server" Font-Bold="True" OnCheckedChanged="chkbatchadd_ChekedChange"
                                                                            Font-Names="Book Antiqua" Font-Size="Medium" Text="Select All" AutoPostBack="True" />
                                                                        <asp:CheckBoxList ID="chklsbatchadd" Style="width: auto;" runat="server" Font-Size="Medium"
                                                                            AutoPostBack="True" Font-Bold="True" Font-Names="Book Antiqua" OnSelectedIndexChanged="chklsbatchadd_SelectedIndexChanged">
                                                                        </asp:CheckBoxList>
                                                                    </asp:Panel>
                                                                    <asp:PopupControlExtender ID="PopupControlExtender4" runat="server" TargetControlID="txtbatchadd"
                                                                        PopupControlID="pbatchadd" Position="Bottom">
                                                                    </asp:PopupControlExtender>
                                                                </div>
                                                            </ContentTemplate>
                                                        </asp:UpdatePanel>
                                                    </td>
                                                       </tr>
                                                        <tr>
                                                    <td  padding-right: 5px;">
                                                        <asp:Label ID="Label2" runat="server" Text="Degree" CssClass="font"></asp:Label>
                                                    </td>
                                                    <td  style="padding-left: 5px;">
                                                        <div style="position: relative;">
                                                            <asp:UpdatePanel ID="UpdatePanel5" runat="server">
                                                                <ContentTemplate>
                                                                    <asp:TextBox ID="txtdegreeadd" runat="server" CssClass="Dropdown_Txt_Box" ReadOnly="true"
                                                                        Width="100px" Style="font-family: 'Book Antiqua';" Font-Bold="True" Font-Names="Book Antiqua"
                                                                        Font-Size="Medium">---Select---</asp:TextBox>
                                                                    <asp:Panel ID="pdegreeadd" runat="server" CssClass="multxtpanel" Height="250px" Style="width: 100%;
                                                                        margin: 0px;">
                                                                        <asp:CheckBox ID="chkdegreeadd" runat="server" Font-Bold="True" OnCheckedChanged="chkdegreeadd_ChekedChange"
                                                                            Font-Names="Book Antiqua" Font-Size="Medium" Text="Select All" AutoPostBack="True" />
                                                                        <asp:CheckBoxList ID="chklsdegreeadd" Style="width: auto; margin: 0px;" runat="server"
                                                                            Font-Size="Medium" AutoPostBack="True" Font-Bold="True" Font-Names="Book Antiqua"
                                                                            OnSelectedIndexChanged="chklsdegreeadd_SelectedIndexChanged">
                                                                        </asp:CheckBoxList>
                                                                    </asp:Panel>
                                                                    <asp:PopupControlExtender ID="PopupControlExtender5" runat="server" TargetControlID="txtdegreeadd"
                                                                        PopupControlID="pdegreeadd" Position="Bottom">
                                                                    </asp:PopupControlExtender>
                                                                </ContentTemplate>
                                                            </asp:UpdatePanel>
                                                        </div>
                                                    </td>
                                             
                                               
                                                    <td style="padding-right: 5px;">
                                                        <asp:Label ID="Label3" runat="server" Text="Branch" CssClass="font"></asp:Label>
                                                    </td>
                                                    <td style="padding-right: 15px; padding-left: 5px;">
                                                        <div style="position: relative;">
                                                            <asp:UpdatePanel ID="UpdatePanel6" runat="server">
                                                                <ContentTemplate>
                                                                    <asp:TextBox ID="txtbranchadd" runat="server" CssClass="Dropdown_Txt_Box" ReadOnly="true"
                                                                        Width="100px" Style="font-family: 'Book Antiqua';" Font-Bold="True" Font-Names="Book Antiqua"
                                                                        Font-Size="Medium">---Select---</asp:TextBox>
                                                                    <asp:Panel ID="pbranchadd" runat="server" CssClass="multxtpanel" Height="250px" Style="width: -moz-max-content;">
                                                                        <asp:CheckBox ID="chkbranchadd" runat="server" Font-Bold="True" OnCheckedChanged="chkbranchadd_ChekedChange"
                                                                            Font-Names="Book Antiqua" Font-Size="Medium" Text="Select All" AutoPostBack="True" />
                                                                        <asp:CheckBoxList ID="chklsbranchadd" Style="width: auto;" runat="server" Font-Size="Medium"
                                                                            AutoPostBack="True" Font-Bold="True" Font-Names="Book Antiqua" OnSelectedIndexChanged="chklsbranchadd_SelectedIndexChanged">
                                                                        </asp:CheckBoxList>
                                                                    </asp:Panel>
                                                                    <asp:PopupControlExtender ID="PopupControlExtender6" runat="server" TargetControlID="txtbranchadd"
                                                                        PopupControlID="pbranchadd" Position="Bottom">
                                                                    </asp:PopupControlExtender>
                                                                </ContentTemplate>
                                                            </asp:UpdatePanel>
                                                        </div>
                                                    </td>
                                                    <td style="padding-left: 15px; padding-right: 5px;">
                                                        <asp:Label ID="lblreasonadd" runat="server" Text="Reason" CssClass="font"></asp:Label>
                                                    </td>
                                                    <td  style="padding-right: 2px; padding-left: 2px;">
                                                        <asp:Button ID="btnreasonadd" runat="server" Text="+" CssClass="font" Height="21px"
                                                            OnClick="btnreasonadd_Click" Style="display: none; height: auto; width: auto;" />
                                                    </td>
                                                    <td style="padding-right: 2px; padding-left: 2px;">
                                                        <asp:DropDownList ID="ddlreason" runat="server" Width="150px" CssClass="font">
                                                        </asp:DropDownList>
                                                    </td>
                                                    <td style="padding-left: 2px;">
                                                        <asp:Button ID="btnreasondelete" runat="server" Text="-" OnClick="btnreasondelete_Click"
                                                            CssClass="font" Style="display: none; height: auto; width: auto;" />
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td style="padding-right: 5px;">
                                                        <asp:Label ID="lblfromdateadd" runat="server" Text="From Date" CssClass="font"></asp:Label>
                                                    </td>
                                                    <td style="padding-right: 15px; padding-left: 5px;">
                                                        <asp:TextBox ID="txtfromdateadd" runat="server" CssClass="font" Width="98px" AutoPostBack="true"
                                                            OnTextChanged="txtfromdateadd_TextChanged"></asp:TextBox>
                                                        <asp:CalendarExtender ID="CalendarExtender3" runat="server" Format="dd/MM/yyyy" TargetControlID="txtfromdateadd">
                                                        </asp:CalendarExtender>
                                                    </td>
                                                    <td style="padding-left: 15px; padding-right: 5px;">
                                                        <asp:Label ID="lbltodateadd" runat="server" Text="To Date" CssClass="font"></asp:Label>
                                                    </td>
                                                    <td colspan="3" style="padding-left: 5px;">
                                                        <asp:TextBox ID="txttodateadd" runat="server" CssClass="font" Width="98px" AutoPostBack="true"
                                                            OnTextChanged="txttodateadd_TextChanged"></asp:TextBox>
                                                        <asp:CalendarExtender ID="CalendarExtender4" runat="server" Format="dd/MM/yyyy" TargetControlID="txttodateadd">
                                                        </asp:CalendarExtender>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td colspan="2" style="padding-right: 15px; padding-left: 5px; margin: 0px; margin-top: 10px;
                                                        margin-bottom: 10px; padding-bottom: 5px; padding-top: 5px;">
                                                        <asp:CheckBox ID="chkConsiderDayOrder" Text="Consider Alternate Day Order" Checked="false"
                                                            runat="server" AutoPostBack="true" OnCheckedChanged="chkConsiderDayOrder_CheckedChanged"
                                                            Style="font-weight: bold;" />
                                                    </td>
                                                    <td id="alterdayy" runat="server" style="padding-left: 15px; margin: 0px; margin-top: 10px;
                                                        margin-bottom: 10px; padding-bottom: 5px; padding-top: 5px;">
                                                        <div id="divAlternateDayOrder" runat="server" visible="false">
                                                            <table style="">
                                                                <tr>
                                                                    <td style="font-weight: bold;">
                                                                        <span>Alternate Day Order</span>
                                                                    </td>
                                                                    <td>
                                                                        <asp:DropDownList ID="ddlAlternateDayOrder" runat="server" Style="font-weight: bold;
                                                                            width: auto;">
                                                                            <asp:ListItem Selected="True" Text="Select" Value="0"></asp:ListItem>
                                                                            <asp:ListItem Selected="False" Text="Day 1" Value="1"></asp:ListItem>
                                                                            <asp:ListItem Selected="False" Text="Day 2" Value="2"></asp:ListItem>
                                                                            <asp:ListItem Selected="False" Text="Day 3" Value="3"></asp:ListItem>
                                                                            <asp:ListItem Selected="False" Text="Day 4" Value="4"></asp:ListItem>
                                                                            <asp:ListItem Selected="False" Text="Day 5" Value="5"></asp:ListItem>
                                                                            <asp:ListItem Selected="False" Text="Day 6" Value="6"></asp:ListItem>
                                                                        </asp:DropDownList>
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </div>
                                                    </td>
                                                    <td colspan="3">
                                                        <asp:CheckBox ID="Chkasperday" Text="As Perday Schedule" Checked="false" Visible="false" runat="server"
                                                            AutoPostBack="true" Style="font-weight: bold;" />
                                                    </td>
                                                </tr>
                                                <tr>
                                                <td colspan="5" style="padding-left: 15px; margin: 0px; margin-top: 10px;
                                                        margin-bottom: 10px; padding-bottom: 5px; padding-top: 5px;">
                                                 <fieldset style="width: 504px; height: 48px;">
                                                  <legend style="font-size: larger; font-weight: bold">Day Order Settings For NextDay</legend>
                                                 <asp:RadioButton ID="rdbasperday" runat="server" Text="As Perday Schedule" 
                                                    GroupName="orderchange" AutoPostBack="true" />
                                                <asp:RadioButton ID="rdbskipday" runat="server" Text="Skipday order Change" GroupName="orderchange"
                                                    AutoPostBack="true" />
                                                <asp:RadioButton ID="rdbnextorder" runat="server" Text="Next Dayorder" GroupName="orderchange" 
                                                    AutoPostBack="true" Checked="true" />
                                              
                                            </fieldset>
                                            </td>
                                                </tr>
                                                <tr>
                                                    <td colspan="2">
                                                        <asp:CheckBox ID="Chkincludeattendance" Text="Include Period in Attendance Report"
                                                            Checked="false" runat="server" AutoPostBack="true" Visible="false" Style="font-weight: bold;" />
                                                    </td>
                                                </tr>
                                                <tr style="margin-top: 10px; margin-bottom: 10px; padding-bottom: 5px; padding-top: 5px;">
                                                    <td colspan="5" align="center" style="margin-top: 10px; margin-bottom: 10px; padding-bottom: 5px;
                                                        padding-top: 5px;">
                                                        <asp:Button ID="btnsave" runat="server" CssClass="font" Text="Save" OnClick="btnsave_Click" />
                                                        <asp:Button ID="Btndelete" runat="server" CssClass="font" Text="Delete" OnClick="Btndelete_Click"
                                                            Visible="false" />
                                                        <asp:Button ID="btnclear" runat="server" CssClass="font" Text="Clear" OnClick="btnclear_Click"
                                                            Visible="false" />
                                                        <asp:Button ID="btnHelp" Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Large"
                                                            runat="server" OnClick="btnHelp_Click" Text="Note" Style="width: 59px; height: auto;" />
                                                    </td>
                                                </tr>
                                                <tr style="margin-top: 10px; margin-bottom: 10px; padding-bottom: 5px; padding-top: 5px;">
                                                    <td colspan="6" style="margin-top: 10px; margin-bottom: 10px; padding-bottom: 5px;
                                                        padding-top: 5px;">
                                                        <asp:Label ID="lblerrmsg" runat="server" CssClass="font" ForeColor="Red" Style="margin-top: 10px;
                                                            margin-bottom: 10px; position: relative; padding: 3px;"></asp:Label>
                                                    </td>
                                                </tr>
                                                <tr style="margin-top: 10px; margin-bottom: 10px; padding-bottom: 5px; padding-top: 5px;">
                                                    <%--<td colspan="8" style="margin-top: 10px; margin-bottom: 10px; padding-bottom: 5px; padding-top: 5px;">--%>
                                                    <td colspan="11" style="padding-right: 25px;">
                                                        <asp:Label ID="lblnote" runat="server" Text="" CssClass="font" Width="517px" Visible="false"></asp:Label>
                                                    </td>
                                                </tr>
                                            </table>
                                        </div>
                                    </center>
                                </div>
                            </Content>
                        </asp:AccordionPane>
                    </Panes>
                </asp:Accordion>
            </ContentTemplate>
        </asp:UpdatePanel>
    </center>
    <center>
        <asp:UpdatePanel ID="UpdatePanel9" runat="server">
            <ContentTemplate>
                <div id="divPopAlert" runat="server" visible="false" style="height: 400em; z-index: 2000;
                    width: 100%; background-color: rgba(54, 25, 25, .2); position: absolute; top: 0;
                    left: 0px;">
                    <center>
                        <div id="divPopAlertContent" runat="server" class="table" style="background-color: White;
                            height: 120px; width: 238px; border: 5px solid #0CA6CA; border-top: 25px solid #0CA6CA;
                            margin-top: 200px; border-radius: 10px;">
                            <center>
                                <table style="height: 100px; width: 100%">
                                    <tr>
                                        <td align="center">
                                            <asp:Label ID="lblAlertMsg" runat="server" Style="color: Red;" Font-Bold="true" Font-Size="Medium"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <center>
                                                <asp:Button ID="btnPopAlertClose" CssClass="textbox textbox1" Style="height: 28px;
                                                    width: 65px;" OnClick="btnPopAlertClose_Click" Text="Ok" runat="server" />
                                            </center>
                                        </td>
                                    </tr>
                                </table>
                            </center>
                        </div>
                    </center>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
    </center>
    <center>
        <asp:UpdatePanel ID="UpdatePanel10" runat="server">
            <ContentTemplate>
                <div id="divPopErr" runat="server" visible="false" style="height: 400em; z-index: 2000;
                    width: 100%; background-color: rgba(54, 25, 25, .2); position: absolute; top: 0;
                    left: 0px;">
                    <center>
                        <div id="pnlPopErrContent" runat="server" class="table" style="background-color: White;
                            height: 120px; width: 238px; border: 5px solid #0CA6CA; border-top: 25px solid #0CA6CA;
                            margin-top: 200px; border-radius: 10px;">
                            <center>
                                <table style="height: 100px; width: 100%">
                                    <tr>
                                        <td align="center">
                                            <asp:Label ID="lblPopErr" runat="server" Text="" Style="color: Red;" Font-Bold="true"
                                                Font-Size="Medium"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <center>
                                                <asp:Button ID="btnPopErrClose" CssClass=" textbox btn1 comm" Style="height: 28px;
                                                    width: 65px;" OnClick="btnPopErrClose_Click" Text="Ok" runat="server" />
                                            </center>
                                        </td>
                                    </tr>
                                </table>
                            </center>
                        </div>
                    </center>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
    </center>
    <center>
        <asp:UpdatePanel ID="UpdatePanel11" runat="server">
            <ContentTemplate>
                <asp:Panel ID="panelreason" runat="server" BorderColor="Black" BackColor="AliceBlue"
                    Visible="false" BorderWidth="2px" Style="left: 20%; top: 40%; right: 20%; height: auto;
                    position: absolute; width: 55%; z-index: 1000;">
                    <div class="panelinfraction" id="Div2" style="text-align: center; font-family: MS Sans Serif;
                        font-size: Small; font-weight: bold; margin: 6px; padding: 5px; height: auto;
                        width: auto;">
                        <span style="text-align: center; margin-bottom: 10px; margin-top: 10px; font-size: large;
                            font-weight: bold; line-height: 3px; padding: 3px;">Reason Entry </span>
                        <center>
                            <table style="margin: 0px; padding: 5px; margin-bottom: 10px; margin-top: 10px; width: 100%;
                                height: auto;">
                                <tr>
                                    <td style="margin: 0px; padding: 0px; width: auto; text-align: right; font-size: medium;
                                        padding-right: 10px;">
                                        <asp:Label ID="lblreaon" runat="server" Text="Reason" Font-Bold="True" Font-Names="Book Antiqua"
                                            Style="margin: 0px; width: auto; padding: 0px;"></asp:Label>
                                    </td>
                                    <td style="margin: 0px; padding: 0px; width: auto; text-align: left; font-size: medium;">
                                        <asp:TextBox ID="textreason" runat="server" Height="28px" TextMode="MultiLine" Font-Bold="True"
                                            Font-Names="Book Antiqua" Font-Size="Medium" MaxLength="100" Style="resize: none;
                                            width: 98%;"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2" align="center">
                                        <asp:Button ID="btnreasonsave" runat="server" Text="Add" OnClick="btnreasonsave_Click"
                                            Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium" />
                                        <asp:Button ID="btnreasonexit" runat="server" Text="Exit" OnClick="btnreasonexit_Click"
                                            Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium" />
                                    </td>
                                </tr>
                            </table>
                        </center>
                        <asp:Label ID="lblreasonerr" runat="server" CssClass="font" ForeColor="Red" Style="margin-bottom: 10px;
                            margin-top: 10px;"></asp:Label>
                    </div>
                </asp:Panel>
            </ContentTemplate>
        </asp:UpdatePanel>
    </center>
    <div style="height: 1px; width: 1px; overflow: auto;">
        <div id="contentDiv" runat="server" style="height: auto; width: 900px;" visible="false">
        </div>
    </div>
</asp:Content>

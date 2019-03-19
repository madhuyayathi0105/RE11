<%@ Page Title="" Language="C#" MasterPageFile="~/AttendanceMOD/AttendanceSubSiteMaster.master"
    AutoEventWireup="true" CodeFile="ElectiveSubjectCountReport.aspx.cs" Inherits="AttendanceMOD_ElectiveSubjectCountReport" %>

<%@ Register Assembly="FarPoint.Web.Spread,  Version=5.0.3520.2008, Culture=neutral, PublicKeyToken=327c3516b1b18457"
    Namespace="FarPoint.Web.Spread" TagPrefix="FarPoint" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <link href="../Styles/css/Commoncss.css" rel="Stylesheet" type="text/css" />
    <script type="text/javascript">
        function PrintGrid() {
            var panel = document.getElementById("<%=divMainContent.ClientID %>");
            var college = document.getElementById("<%=ddlCollege.ClientID %>");
            college = college.options[college.selectedIndex].text;

            var batch = document.getElementById("<%=ddlBatch.ClientID %>").value;
            var edulevel = document.getElementById("<%=ddlEduLevel.ClientID %>").value;

            var course = document.getElementById("<%=ddlCourse.ClientID %>");
            course = course.options[course.selectedIndex].text;

            var printWindow = window.open('', '', 'height=816,width=980');
            printWindow.document.write('<html><head>');
            printWindow.document.write('</head><body >');
            printWindow.document.write('<center><h2>');
            printWindow.document.write(college);
            printWindow.document.write('</h2>');
            printWindow.document.write('<table style=\'font-size:14px; font-weight:bold;\' cellpadding=10><tr><td>Batch :</td><td>' + batch + '</td><td>Education Level :</td><td>' + edulevel + '</td><td>Course :</td><td>' + course + '</td></tr></table>');
            printWindow.document.write(panel.innerHTML);
            printWindow.document.write('</center></body></html>');
            printWindow.document.close();
            setTimeout(function () {
                printWindow.print();
            }, 500);
            return false;
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <center>
       
        <span id="spHeader" class="fontstyleheader" style="margin: 0px; margin-bottom: 10px;
            margin-top: 10px; position: relative; color: Green; font-weight: bold;">CBCS Report</span>
        <div class="maindivstyle" style="width: 950px; height: auto; margin: 0px; margin-top: 15px;
            margin-bottom: 15px; padding: 8px;">
            <table class="maintablestyle" style="margin: 0px; margin-top: 15px; margin-bottom: 15px;
                padding: 8px;">
                <tr>
                    <td>
                        <asp:Label ID="lblCollege" Style="font-family: 'Book Antiqua'; font-weight: bold;
                            font-size: medium;" runat="server" Text="Institution"></asp:Label>
                    </td>
                    <td>
                        <asp:DropDownList ID="ddlCollege" runat="server" CssClass="textbox1" Style="font-family: 'Book Antiqua';
                            font-weight: bold; font-size: medium;" Width="250px" AutoPostBack="True" OnSelectedIndexChanged="ddlCollege_SelectedIndexChanged">
                        </asp:DropDownList>
                    </td>
                    <td>
                        <asp:Label ID="lblBatch" Style="font-family: 'Book Antiqua'; font-weight: bold; font-size: medium;"
                            runat="server" Text="Batch"></asp:Label>
                    </td>
                    <td>
                        <asp:DropDownList ID="ddlBatch" runat="server" CssClass="textbox1" Style="font-family: 'Book Antiqua';
                            font-weight: bold; font-size: medium; width: 70px;" AutoPostBack="True" OnSelectedIndexChanged="ddlBatch_SelectedIndexChanged">
                        </asp:DropDownList>
                    </td>
                    <td>
                        <asp:Label ID="lblEduLevel" Style="font-family: 'Book Antiqua'; font-weight: bold;
                            font-size: medium;" runat="server" Text="Graduate"></asp:Label>
                    </td>
                    <td>
                        <asp:DropDownList ID="ddlEduLevel" runat="server" CssClass="textbox1" Style="font-family: 'Book Antiqua';
                            font-weight: bold; font-size: medium; width: 90px;" AutoPostBack="True" OnSelectedIndexChanged="ddlEduLevel_SelectedIndexChanged">
                        </asp:DropDownList>
                    </td>
                    <td>
                        <asp:Label ID="lblCourse" Style="font-family: 'Book Antiqua'; font-weight: bold;
                            font-size: medium;" runat="server" Text="Course"></asp:Label>
                    </td>
                    <td>
                        <asp:DropDownList ID="ddlCourse" runat="server" CssClass="textbox1" Style="font-family: 'Book Antiqua';
                            font-weight: bold; font-size: medium; width: 100px;" AutoPostBack="True" OnSelectedIndexChanged="ddlCourse_SelectedIndexChanged">
                        </asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <td colspan="10">
                        <table>
                            <tr>
                                <td>
                                    <asp:Label ID="lblBranch" Style="font-family: 'Book Antiqua'; font-weight: bold;
                                        font-size: medium;" runat="server" Text="Branch"></asp:Label>
                                </td>
                                <td>
                                 <asp:DropDownList ID="ddlBranch" runat="server" CssClass="textbox1" Style="font-family: 'Book Antiqua';
                                        font-weight: bold; font-size: medium; width: 150px;" AutoPostBack="True" OnSelectedIndexChanged="ddlBranch_SelectedIndexChanged">
                                    </asp:DropDownList>
                                   <%-- <div style="position: relative;">
                                        <asp:UpdatePanel ID="upnlBranch" runat="server">
                                            <contenttemplate>
                                                <asp:TextBox ID="txtBranch" Visible="true" Width="85px" runat="server" Font-Bold="True"
                                                    Font-Names="Book Antiqua" Font-Size="Medium" CssClass="textbox  txtheight2" ReadOnly="true">--Select--</asp:TextBox>
                                                <asp:Panel ID="pnlBranch" Visible="true" runat="server" CssClass="multxtpanel" Style="width: 260px;
                                                    height: 230px; overflow: auto; margin: 0px; padding: 0px;">
                                                    <asp:CheckBox ID="chkBranch" Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium"
                                                        runat="server" Text="Select All" AutoPostBack="True" Style="width: 100%; height: auto;
                                                        margin: 0px; padding: 0px; border: 0px;" OnCheckedChanged="chkBranch_CheckedChanged" />
                                                    <asp:CheckBoxList ID="cblBranch" Font-Bold="True" Font-Size="Medium" Font-Names="Book Antiqua"
                                                        runat="server" AutoPostBack="True" Style="width: 100%; height: auto; margin: 0px;
                                                        padding: 0px; border: 0px;" OnSelectedIndexChanged="cblBranch_SelectedIndexChanged">
                                                    </asp:CheckBoxList>
                                                </asp:Panel>
                                                <asp:PopupControlExtender ID="popExtBranch" runat="server" TargetControlID="txtBranch"
                                                    PopupControlID="pnlBranch" Position="Bottom">
                                                </asp:PopupControlExtender>
                                                <asp:DropDownList ID="ddlBranch" Visible="false" Width="150px" runat="server" Font-Bold="True"
                                                    Font-Names="Book Antiqua" Font-Size="Medium" CssClass="arrow" AutoPostBack="true"
                                                    OnSelectedIndexChanged="ddlBranch_SelectedIndexChanged">
                                                </asp:DropDownList>
                                            </contenttemplate>
                                        </asp:UpdatePanel>
                                    </div>--%>
                                </td>
                                <td>
                                    <asp:Label ID="lblSem" Style="font-family: 'Book Antiqua'; font-weight: bold; font-size: medium;"
                                        runat="server" Text="Sem"></asp:Label>
                                </td>
                                <td>
                                    <asp:DropDownList ID="ddlSem" runat="server" CssClass="textbox1" Style="font-family: 'Book Antiqua';
                                        font-weight: bold; font-size: medium; width: 55px;" AutoPostBack="True" OnSelectedIndexChanged="ddlSem_SelectedIndexChanged">
                                    </asp:DropDownList>
                                </td>
                                <td>
                                    <asp:DropDownList ID="ddlreportType" runat="server" CssClass="textbox1" Style="font-family: 'Book Antiqua';
                                        font-weight: bold; font-size: medium; width: 150px;">
                                        <asp:ListItem>Count Report</asp:ListItem>
                                        <asp:ListItem>Registered Student Report</asp:ListItem>
                                        <asp:ListItem>UnRegistered Student Report</asp:ListItem>
                                         <asp:ListItem>Incomplete Criteria</asp:ListItem>
                                          <%-- <asp:ListItem>Incomplete Student Report</asp:ListItem>--%>
                                    </asp:DropDownList>
                                </td>
                                <td>
                                    <asp:Button ID="btnGo" CssClass="textbox textbox1" runat="server" Style="width: auto;
                                        height: auto; font-family: 'Book Antiqua'; font-weight: bold; font-size: medium;"
                                        Text="Go" OnClick="btnGo_Click" />
                                </td>
                                <td>                   
                    <asp:CheckBox ID="chksubstaff" Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium"
                        Visible="true" runat="server" AutoPostBack="True" Text="Subject/Staff" OnCheckedChanged="chkSubstaff_CheckedChanged" />
                    <asp:DropDownList ID="ddlSubStaff" runat="server" CssClass="textbox1 ddlheight1" Font-Bold="true"
                        Font-Names="Book Antiqua" Font-Size="Medium" AutoPostBack="true" OnSelectedIndexChanged="ddlSubStaff_OnSelectedIndexChanged">
                        <asp:ListItem>--Select--</asp:ListItem>
                        <asp:ListItem>Subject Wise</asp:ListItem>
                        <asp:ListItem>Staff Wise</asp:ListItem>
                    </asp:DropDownList>
                </td>
                 <td>
                          
                               
                                <asp:TextBox ID="txt_searchbystaf" runat="server" AutoPostBack="true" OnTextChanged="txt_searchbystaf_OnTextChanged"
                                    CssClass="textbox txtheight2" Style="font-weight: bold; width: 150px; font-family: book antiqua;
                                    font-size: medium; margin-left: 0px;"></asp:TextBox>
                                <asp:AutoCompleteExtender ID="AutoCompleteExtender6" runat="server" DelimiterCharacters=""
                                    Enabled="True" ServiceMethod="GetStaffCode" MinimumPrefixLength="0" CompletionInterval="100"
                                    EnableCaching="false" CompletionSetCount="10" ServicePath="" TargetControlID="txt_searchbystaf"
                                    CompletionListCssClass="autocomplete_completionListElement" CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem"
                                    CompletionListItemCssClass="txtsearchpan">
                                </asp:AutoCompleteExtender>
                                 <asp:AutoCompleteExtender ID="AutoCompleteExtender1" runat="server" DelimiterCharacters=""
                                    Enabled="True" ServiceMethod="GetSubCode" MinimumPrefixLength="0" CompletionInterval="100"
                                    EnableCaching="false" CompletionSetCount="10" ServicePath="" TargetControlID="txt_searchbystaf"
                                    CompletionListCssClass="autocomplete_completionListElement" CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem"
                                    CompletionListItemCssClass="txtsearchpan">
                                </asp:AutoCompleteExtender>
                         
                        </td>




                                <td>
                                    <asp:Button ID="btnPrint" runat="server" Style="width: auto; height: auto; font-family: 'Book Antiqua';
                                        font-weight: bold; font-size: medium;" Text="Print" CssClass="textbox  btn2"
                                        BackColor="#EB7E8C" ForeColor="White" Visible="false" OnClientClick="return PrintGrid()" />
                                </td>
                                <td>
                                    <asp:Button ID="btnSave" runat="server" Style="width: auto; height: auto; font-family: 'Book Antiqua';
                                        font-weight: bold; font-size: medium;" Text="Save" CssClass="textbox  btn2" BackColor="#EB7E8C"
                                        ForeColor="White" Visible="false" OnClick="btnSave_Click" />
                                </td>
                                <td>
                                    <asp:CheckBox ID="cbSectionWise" runat="server" Visible="false" Text="Include Section Wise Count" />
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
            </table>
            <asp:Label ID="lblErrSearch" runat="server" Text="" ForeColor="Red" Visible="False"
                Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium" Style="margin: 0px;
                margin-bottom: 10px; margin-top: 10px; position: relative;"></asp:Label>
            <center>
                <div id="divMainContent" visible="false" runat="server" style="margin: 0px; margin-bottom: 20px;
                    margin-top: 20px;">
                    <asp:GridView ID="gvSectionWiseCount" runat="server" AutoGenerateColumns="false">
                        <Columns>
                            <asp:TemplateField HeaderText="S.No">
                                <ItemTemplate>
                                    <asp:Label ID="lblSno" runat="server" Text="<%# Container.DisplayIndex+1 %>"></asp:Label>
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="30px" />
                            </asp:TemplateField>
                        </Columns>
                        <Columns>
                            <asp:TemplateField HeaderText="Batch Year">
                                <ItemTemplate>
                                    <asp:Label ID="lblDegreeName" runat="server" Visible="true" Text='<%#Eval("batchyear") %>'></asp:Label>
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="200px" />
                            </asp:TemplateField>
                        </Columns>
                        <Columns>
                            <asp:TemplateField HeaderText="Degree">
                                <ItemTemplate>
                                    <asp:Label ID="lbldegree" runat="server" Visible="true" Style="font-weight: bold;"
                                        ForeColor="#003366" Text='<%#Eval("degreename") %>'></asp:Label>
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" Width="130px" />
                            </asp:TemplateField>
                        </Columns>
                        <Columns>
                            <asp:TemplateField HeaderText="Branch">
                                <ItemTemplate>
                                    <asp:Label ID="lblbaranch" runat="server" Visible="true" Style="font-weight: bold;"
                                        ForeColor="#660033" Text='<%#Eval("deptName") %>'></asp:Label>
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" Width="130px" />
                            </asp:TemplateField>
                        </Columns>
                        <Columns>
                            <asp:TemplateField HeaderText="Semester">
                                <ItemTemplate>
                                    <asp:Label ID="lblsemester" runat="server" Visible="true" Style="font-weight: bold;"
                                        ForeColor="#660033" Text='<%#Eval("semester") %>'></asp:Label>
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="40px" />
                            </asp:TemplateField>
                        </Columns>
                        <Columns>
                            <asp:TemplateField HeaderText="Eligible">
                                <ItemTemplate>
                                    <asp:Label ID="lblEligible" runat="server" Visible="true" Style="font-weight: bold;"
                                        ForeColor="#660033" Text='<%#Eval("MaxStudent") %>'></asp:Label>
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="130px" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Registered">
                                <ItemTemplate>
                                    <asp:Label ID="lblregistered" runat="server" Visible="true" Style="font-weight: bold;"
                                        ForeColor="#660033" Text='<%#Eval("SelectedStudent") %>'></asp:Label>
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="130px" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Un Registered">
                                <ItemTemplate>
                                    <asp:Label ID="lblunregistered" runat="server" Visible="true" Style="font-weight: bold;"
                                        ForeColor="#660033" Text='<%#Eval("UnSelectedStudent") %>'></asp:Label>
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="130px" />
                            </asp:TemplateField>
                       <%--     <asp:TemplateField HeaderText="Incomplete">
                                <ItemTemplate>
                                    <asp:Label ID="lblincomplete" runat="server" Visible="true" Style="font-weight: bold;"
                                        ForeColor="#660033" Text='<%#Eval("PartialySelectedStudent") %>'></asp:Label>
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="130px" />
                            </asp:TemplateField>--%>
                        </Columns>
                    </asp:GridView>
                </div>
                <div id="DivSubMain" runat="server" visible="false">
                    <FarPoint:FpSpread ID="FpSpread1" runat="server" Visible="false" HorizontalScrollBarPolicy="Never"
                        VerticalScrollBarPolicy="Never" Width="700px" ShowHeaderSelection="false">
                        <Sheets>
                            <FarPoint:SheetView AutoPostBack="true" SheetName="Sheet1" Visible="true">
                            </FarPoint:SheetView>
                        </Sheets>
                    </FarPoint:FpSpread>
                    <div>
                        <asp:Label ID="lblrptname" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                            Font-Size="Medium" Text="Report Name"></asp:Label>
                        <asp:TextBox ID="txtexcelname" runat="server" Height="20px" Width="180px" Style="font-family: 'Book Antiqua'"
                            Font-Bold="True" onkeypress="display()" Font-Names="Book Antiqua" Font-Size="Medium"></asp:TextBox>
                        <asp:FilteredTextBoxExtender ID="Filterspace" runat="server" TargetControlID="txtexcelname"
                            FilterType="LowercaseLetters,UppercaseLetters,Numbers,Custom" ValidChars="()}{][ ,/">
                        </asp:FilteredTextBoxExtender>
                        <asp:Button ID="btnxl" runat="server" Text="Export Excel" Font-Bold="True" Font-Names="Book Antiqua"
                            Font-Size="Medium" OnClick="btnxl_Click" />
                        <asp:Button ID="btnmasterprint" runat="server" Text="Print" Font-Bold="True" Font-Names="Book Antiqua"
                            Font-Size="Medium" OnClick="btnmasterprint_Click" />
                        <Insproplus:printmaster runat="server" ID="Printcontrol" Visible="false" />
                    </div>
                </div>
            </center>
        </div>
    </center>
    <center>
        <div id="divPopupAlert" runat="server" visible="false" style="height: 100em; z-index: 1000;
            width: 100%; background-color: rgba(54, 25, 25, .2); position: absolute; top: 0%;
            left: 0%; right: 0%;">
            <center>
                <div id="divAlertContent" runat="server" class="table" style="background-color: White;
                    height: 120px; width: 238px; border: 5px solid #0CA6CA; border-top: 25px solid #0CA6CA;
                    margin-top: 200px; border-radius: 10px;">
                    <center>
                        <table style="height: 100px; width: 100%;">
                            <tr>
                                <td align="center">
                                    <asp:Label ID="lblAlertMsg" runat="server" Text="" Style="color: Red;" Font-Bold="true"
                                        Font-Size="Medium" Font-Names="Book Antiqua"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td align="center">
                                    <center>
                                        <asp:Button ID="btnPopAlertClose" runat="server" CssClass=" textbox btn2" Width="40px"
                                            OnClick="btnPopAlertClose_Click" Text="Ok" />
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

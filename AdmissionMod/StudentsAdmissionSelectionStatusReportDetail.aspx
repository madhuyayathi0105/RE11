<%@ Page Title="Admission Selection Status Report" Language="C#" MasterPageFile="~/AdmissionMod/AttendanceSubSiteMaster.master"
    AutoEventWireup="true" CodeFile="StudentsAdmissionSelectionStatusReportDetail.aspx.cs"
    Inherits="StudentsAdmissionSelectionStatusReportDetail" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <link href="../Styles/css/Commoncss.css" rel="Stylesheet" type="text/css" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">
    <script type="text/javascript">
        function PrintGrid() {
            var panel = document.getElementById('printDiv');
            var college = document.getElementById("<%=ddlCollege.ClientID %>");
            college = college.options[college.selectedIndex].text;

            var batch = document.getElementById("<%=ddlBatch.ClientID %>").value;
            var edulevel = document.getElementById("<%=ddlEduLevel.ClientID %>").value;

            var course = document.getElementById("<%=ddlReportType.ClientID %>");
            course = course.options[course.selectedIndex].text;

            var streamval = document.getElementById("<%=ddlStream.ClientID %>");
            streamval = streamval.options[streamval.selectedIndex].text;

            var date = document.getElementById("<%=txtFromDate.ClientID %>").value;

            var sessionvalue = document.getElementById("<%=ddlSession.ClientID %>");
            sessionvalue = sessionvalue.options[sessionvalue.selectedIndex].text;
            if (sessionvalue != "All") {
                sessionvalue += " Hours";
            }

            var printWindow = window.open('', '', 'height=816,width=980');
            printWindow.document.write('<html><head>');
            printWindow.document.write('</head><body >');
            printWindow.document.write('<center><h2>');
            printWindow.document.write(college);
            printWindow.document.write('</h2>');
            printWindow.document.write('<table style=\'font-size:14px; font-weight:bold;\' cellpadding=10><tr><td></td><td>B.Tech. / M.Tech. (5-year Integ.) ' + course + ' List for 2017-18  ' + (streamval + ' - ' + date + '  ' + sessionvalue) + ' </td></tr></table>');

            printWindow.document.write(panel.innerHTML);
            printWindow.document.write('</center></body></html>');
            printWindow.document.close();
            setTimeout(function () {
                printWindow.print();
            }, 500);
            return false;
        }
    </script>
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <center>
        <span id="spHeader" class="fontstyleheader" style="margin: 0px; margin-bottom: 10px;
            margin-top: 10px; position: relative; color: Green; font-weight: bold;">Detailed
            Admission Status Report</span>
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
                        <asp:DropDownList ID="ddlCollege" runat="server" CssClass="textbox ddlheight" Style="font-family: 'Book Antiqua';
                            font-weight: bold; font-size: medium;" Width="250px" AutoPostBack="True" OnSelectedIndexChanged="ddlCollege_SelectedIndexChanged">
                        </asp:DropDownList>
                    </td>
                    <td>
                        <asp:Label ID="lblBatch" Style="font-family: 'Book Antiqua'; font-weight: bold; font-size: medium;"
                            runat="server" Text="Batch"></asp:Label>
                    </td>
                    <td>
                        <asp:DropDownList ID="ddlBatch" runat="server" CssClass="textbox ddlheight" Style="font-family: 'Book Antiqua';
                            font-weight: bold; font-size: medium; width: 70px;" AutoPostBack="True" OnSelectedIndexChanged="ddlBatch_SelectedIndexChanged">
                        </asp:DropDownList>
                    </td>
                    <td>
                        <asp:Label ID="lblEduLevel" Style="font-family: 'Book Antiqua'; font-weight: bold;
                            font-size: medium;" runat="server" Text="Graduate"></asp:Label>
                    </td>
                    <td>
                        <asp:DropDownList ID="ddlEduLevel" runat="server" CssClass="textbox ddlheight" Style="font-family: 'Book Antiqua';
                            font-weight: bold; font-size: medium; width: 90px;" AutoPostBack="True" OnSelectedIndexChanged="ddlEduLevel_SelectedIndexChanged">
                        </asp:DropDownList>
                    </td>
                    <td>
                        <asp:Label ID="lblCourse" Style="font-family: 'Book Antiqua'; font-weight: bold;
                            font-size: medium;" runat="server" Text="Course"></asp:Label>
                    </td>
                    <td>
                        <asp:DropDownList ID="ddlCourse" runat="server" CssClass="textbox ddlheight" Style="font-family: 'Book Antiqua';
                            font-weight: bold; font-size: medium; width: 100px;" AutoPostBack="True" OnSelectedIndexChanged="ddlCourse_SelectedIndexChanged">
                        </asp:DropDownList>
                    </td>
                    <td>
                        <asp:Label ID="lblStream" Style="font-family: 'Book Antiqua'; font-weight: bold;
                            font-size: medium;" runat="server" Text="Stream"></asp:Label>
                    </td>
                    <td>
                        <asp:DropDownList ID="ddlStream" runat="server" CssClass="textbox ddlheight1" Style="font-family: 'Book Antiqua';
                            font-weight: bold; font-size: medium; width: 90px;" AutoPostBack="True" OnSelectedIndexChanged="ddlStream_SelectedIndexChanged">
                        </asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <td colspan="12">
                        <table>
                            <tr>
                                <td>
                                    <asp:Label ID="lblReportType" Style="font-family: 'Book Antiqua'; font-weight: bold;
                                        font-size: medium;" runat="server" Text="Report Type"></asp:Label>
                                </td>
                                <td>
                                    <asp:DropDownList ID="ddlReportType" runat="server" CssClass="textbox ddlheight4"
                                        Width="235px" Style="font-family: 'Book Antiqua'; font-weight: bold; font-size: medium;"
                                        AutoPostBack="True" OnSelectedIndexChanged="ddlReportType_SelectedIndexChanged">
                                        <asp:ListItem Text="Counselling called"></asp:ListItem>
                                        <asp:ListItem Text="Registered"></asp:ListItem>
                                        <asp:ListItem Text="Not Registered"></asp:ListItem>
                                        <asp:ListItem Text="Verified"></asp:ListItem>
                                        <asp:ListItem Text="Not Verified"></asp:ListItem>
                                        <asp:ListItem Text="Admitted"></asp:ListItem>
                                        <asp:ListItem Text="Admitted Count"></asp:ListItem>
                                        <asp:ListItem Text="Admitted Count With Session"></asp:ListItem>
                                        <asp:ListItem Text="Hostel registered"></asp:ListItem>
                                        <asp:ListItem Text="Transport registered"></asp:ListItem>
                                    </asp:DropDownList>
                                </td>
                                <td>
                                    <asp:Label ID="lblFromDate" Style="font-family: 'Book Antiqua'; font-weight: bold;
                                        font-size: medium;" runat="server" Text="From"></asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtFromDate" CssClass="textbox textbox1" runat="server" Font-Bold="true"
                                        Width="80px" OnTextChanged="txtDate_TextChanged" AutoPostBack="true"></asp:TextBox>
                                    <asp:CalendarExtender ID="calExtFromDate" runat="server" Format="dd/MM/yyyy" TargetControlID="txtFromDate">
                                    </asp:CalendarExtender>
                                </td>
                                <td>
                                    <asp:Label ID="lblToDate" Style="font-family: 'Book Antiqua'; font-weight: bold;
                                        font-size: medium;" runat="server" Text="To"></asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtToDate" runat="server" CssClass="textbox textbox1" Font-Bold="true"
                                        Width="80px" OnTextChanged="txtDate_TextChanged" AutoPostBack="true"></asp:TextBox>
                                    <asp:CalendarExtender ID="calExtToDate" runat="server" Format="dd/MM/yyyy" TargetControlID="txtToDate">
                                    </asp:CalendarExtender>
                                </td>
                                <td>
                                    <asp:Label ID="lblSession" Style="font-family: 'Book Antiqua'; font-weight: bold;
                                        font-size: medium;" runat="server" Text="Session"></asp:Label>
                                </td>
                                <td>
                                    <asp:DropDownList ID="ddlSession" runat="server" CssClass="textbox ddlheight3" Style="font-family: 'Book Antiqua';
                                        font-weight: bold; font-size: medium;" AutoPostBack="True" OnSelectedIndexChanged="ddlSession_SelectedIndexChanged">
                                    </asp:DropDownList>
                                </td>
                                <td>
                                    <asp:Button ID="btnGo" CssClass="textbox btn" runat="server" Style="width: auto;
                                        height: auto; font-family: 'Book Antiqua'; font-weight: bold; font-size: medium;"
                                        Text="Go" OnClick="btnGo_Click" />
                                </td>
                                <td>
                                    <asp:Button ID="btnBasePrint" runat="server" Text="Print" CssClass="textbox  btn2"
                                        Width="60px" BackColor="#EB7E8C" ForeColor="White" Visible="false" OnClientClick="return PrintGrid()" />
                                </td>
                            </tr>
                            <tr id="Showdiv" runat="server" visible="false">
                                <td>
                                    Gender
                                </td>
                                <td>
                                    <asp:DropDownList ID="ddlgender" runat="server" CssClass="textbox ddlheight3" Visible="false"
                                        Style="font-family: 'Book Antiqua'; font-weight: bold; font-size: medium;">
                                    </asp:DropDownList>
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
                <div id="printDiv">
                    <asp:GridView ID="gridReport" runat="server" AutoGenerateColumns="true" HeaderStyle-BackColor="#5A71A6"
                        Style="font-family: Arial Narrow; font-size: large;" HeaderStyle-ForeColor="White"
                        OnRowDataBound="gridReport_OnRowDataBound" OnDataBound="gridReport_OnDataBound"
                        Visible="false">
                        <Columns>
                            <asp:TemplateField HeaderText="S.No">
                                <ItemTemplate>
                                    <asp:Label ID="lblSno" runat="server" Text='<%#Container.DisplayIndex+1 %>'></asp:Label>
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
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

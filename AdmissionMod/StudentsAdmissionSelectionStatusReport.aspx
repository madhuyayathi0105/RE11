<%@ Page Title="Admission Selection Status Report" Language="C#" MasterPageFile="~/AdmissionMod/AttendanceSubSiteMaster.master"
    AutoEventWireup="true" CodeFile="StudentsAdmissionSelectionStatusReport.aspx.cs"
    Inherits="AdmissionMod_StudentsAdmissionSelectionStatusReport" %>

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

            var streamval = document.getElementById("<%=ddlStream.ClientID %>");
            streamval = streamval.options[streamval.selectedIndex].text;
            var streamName='Stream :';
            if (streamval.toLowerCase() == 'all')
            {
                streamval = '';
                streamName='';
                }
            var printWindow = window.open('', '', 'height=816,width=980');
            printWindow.document.write('<html><head>');
            printWindow.document.write('</head><body >');
            printWindow.document.write('<center><h2>');
            printWindow.document.write(college);
            printWindow.document.write('</h2>');
            printWindow.document.write('<table style=\'font-size:14px; font-weight:bold;\' cellpadding=10><tr><td>Batch :</td><td>' + batch + '</td><td>Education Level :</td><td>' + edulevel + '</td><td>Course :</td><td>' + course + '</td><td>' + streamName + '</td><td>' + streamval + '</td></tr></table>');
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
            margin-top: 10px; position: relative; color: Green; font-weight: bold;">Admission
            Status Report</span>
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
                    <td>
                        <asp:Label ID="lblStream" Style="font-family: 'Book Antiqua'; font-weight: bold;
                            font-size: medium;" runat="server" Text="Stream"></asp:Label>
                    </td>
                    <td>
                        <asp:DropDownList ID="ddlStream" runat="server" CssClass="textbox1" Style="font-family: 'Book Antiqua';
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
                                    <asp:DropDownList ID="ddlReportType" runat="server" CssClass="textbox1" Style="font-family: 'Book Antiqua';
                                        font-weight: bold; font-size: medium;" AutoPostBack="True" OnSelectedIndexChanged="ddlReportType_SelectedIndexChanged">
                                        <asp:ListItem Selected="False" Text="Date Wise" Value="0" style="display: none;"></asp:ListItem>
                                        <asp:ListItem Selected="True" Text="Date Wise" Value="2"></asp:ListItem>
                                        <asp:ListItem Selected="False" Text="Session Wise" Value="1"></asp:ListItem>
                                    </asp:DropDownList>
                                </td>
                                <td>
                                    <asp:Label ID="lblFromDate" Style="font-family: 'Book Antiqua'; font-weight: bold;
                                        font-size: medium;" runat="server" Text="From Date"></asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtFromDate" runat="server" AutoPostBack="true" Font-Bold="true"
                                        Width="80px" OnTextChanged="txtFromDate_TextChanged"></asp:TextBox>
                                    <asp:CalendarExtender ID="calExtFromDate" runat="server" Format="dd/MM/yyyy" TargetControlID="txtFromDate">
                                    </asp:CalendarExtender>
                                </td>
                                <td>
                                    <asp:Label ID="lblToDate" Style="font-family: 'Book Antiqua'; font-weight: bold;
                                        font-size: medium;" runat="server" Text="To Date"></asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtToDate" runat="server" AutoPostBack="true" Font-Bold="true" Width="80px"
                                        OnTextChanged="txtToDate_TextChanged"></asp:TextBox>
                                    <asp:CalendarExtender ID="calExtToDate" runat="server" Format="dd/MM/yyyy" TargetControlID="txtToDate">
                                    </asp:CalendarExtender>
                                </td>
                                <td colspan="2">
                                    <div id="divSession" runat="server" visible="false">
                                        <table>
                                            <tr>
                                                <td>
                                                    <asp:Label ID="lblSession" Style="font-family: 'Book Antiqua'; font-weight: bold;
                                                        font-size: medium;" runat="server" Text="Session"></asp:Label>
                                                </td>
                                                <td>
                                                    <asp:DropDownList ID="ddlSession" runat="server" CssClass="textbox1" Style="font-family: 'Book Antiqua';
                                                        font-weight: bold; font-size: medium;" AutoPostBack="True" OnSelectedIndexChanged="ddlSession_SelectedIndexChanged">
                                                    </asp:DropDownList>
                                                </td>
                                            </tr>
                                        </table>
                                    </div>
                                </td>
                                <td>
                                    <asp:Button ID="btnGo" CssClass="textbox textbox1" runat="server" Style="width: auto;
                                        height: auto; font-family: 'Book Antiqua'; font-weight: bold; font-size: medium;"
                                        Text="Go" OnClick="btnGo_Click" />
                                </td>
                                <td>
                                    <asp:Button ID="btnPrint" runat="server" Style="width: auto; height: auto; font-family: 'Book Antiqua';
                                        font-weight: bold; font-size: medium;" Text="Print" CssClass="textbox  btn2"
                                        BackColor="#EB7E8C" ForeColor="White" Visible="false" OnClientClick="return PrintGrid()" />
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
                    <table style="margin: 0px; margin-bottom: 6px; display: none;">
                        <tr>
                            <td style="padding: 10px; margin: 0px; margin-right: 4px; background-color: #003366;">
                            </td>
                            <td style="margin: 0px; padding: 2px; margin-left: 3px; margin-right: 3px;">
                                Total Seats
                            </td>
                            <td style="padding: 10px; margin: 0px; margin-right: 4px; margin-left: 4px; background-color: #660033">
                            </td>
                            <td style="margin: 0px; padding: 2px; margin-left: 3px; margin-right: 3px;">
                                Called
                            </td>
                            <td style="padding: 10px; margin: 0px; margin-right: 4px; margin-left: 4px; background-color: #9933FF;">
                            </td>
                            <td style="margin: 0px; padding: 2px; margin-left: 3px; margin-right: 3px;">
                                Registered
                            </td>
                            <td style="padding: 10px; margin: 0px; margin-right: 4px; margin-left: 4px; background-color: #993366;">
                            </td>
                            <td style="margin: 0px; padding: 2px; margin-left: 3px; margin-right: 3px;">
                                Verified
                            </td>
                            <td style="padding: 10px; margin: 0px; margin-right: 4px; margin-left: 4px; background-color: Green;">
                            </td>
                            <td style="margin: 0px; padding: 2px; margin-left: 3px; margin-right: 3px;">
                                Admited
                            </td>
                            <td style="padding: 10px; margin: 0px; margin-right: 4px; margin-left: 4px; background-color: #006666;">
                            </td>
                            <td style="margin: 0px; padding: 2px; margin-left: 3px; margin-right: 3px;">
                                Hostel
                            </td>
                            <td style="padding: 10px; margin: 0px; margin-right: 4px; margin-left: 4px; background-color: #CC9900;">
                            </td>
                            <td style="margin: 0px; padding: 2px; margin-left: 3px; margin-right: 3px;">
                                Transport
                            </td>
                        </tr>
                    </table>
                    <asp:GridView ID="gvAdmissionStatus" runat="server" CaptionAlign="Top" HorizontalAlign="Justify"
                        CellPadding="4" ForeColor="#333333" AutoGenerateColumns="false" GridLines="Vertical"
                        Style="width: 100%; height: auto;" OnDataBound="gvAdmissionStatus_DataBound">
                        <RowStyle BackColor="#E3EAEB" />
                        <Columns>
                            <asp:TemplateField HeaderText="S.No">
                                <ItemTemplate>
                                    <asp:Label ID="lblSno" runat="server" Text="<%# Container.DisplayIndex+1 %>"></asp:Label>
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="30px" />
                            </asp:TemplateField>
                        </Columns>
                        <Columns>
                            <asp:TemplateField HeaderText="Date">
                                <ItemTemplate>
                                    <asp:Label ID="lblDate" runat="server" Text='<%#Eval("EnrollDate") %>'></asp:Label>
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="80px" />
                            </asp:TemplateField>
                        </Columns>
                        <Columns>
                            <asp:TemplateField HeaderText="Session">
                                <ItemTemplate>
                                    <asp:Label ID="lblgvSession" runat="server" Text='<%#Eval("enrollSession") %>'></asp:Label>
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="80px" />
                            </asp:TemplateField>
                        </Columns>
                        <Columns>
                            <asp:TemplateField HeaderText="Course Name" Visible="false">
                                <ItemTemplate>
                                    <asp:Label ID="lblCourseName" runat="server" Visible="true" Text='<%#Eval("courseName") %>'></asp:Label>
                                    <asp:Label ID="lblCourseId" runat="server" Visible="false" Text='<%#Eval("courseId") %>'></asp:Label>
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" Width="60px" />
                            </asp:TemplateField>
                        </Columns>
                        <Columns>
                            <asp:TemplateField HeaderText="Degree Name">
                                <ItemTemplate>
                                    <asp:Label ID="lblDegreeName" runat="server" Visible="true" Text='<%#Eval("DegreeName") %>'></asp:Label>
                                    <asp:Label ID="lblDegreeCode" runat="server" Visible="false" Text='<%#Eval("DegreeCode") %>'></asp:Label>
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" Width="200px" />
                            </asp:TemplateField>
                        </Columns>
                        <Columns>
                            <asp:TemplateField HeaderText="Total Seats">
                                <ItemTemplate>
                                    <asp:Label ID="lblTotSeats" runat="server" Visible="true" Style="font-weight: bold;"
                                        ForeColor="#003366" Text='<%#Eval("totSeats") %>'></asp:Label>
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Right" VerticalAlign="Middle" Width="50px" />
                            </asp:TemplateField>
                        </Columns>
                        <Columns>
                            <asp:TemplateField HeaderText="Called">
                                <ItemTemplate>
                                    <asp:Label ID="lblCalled" runat="server" Visible="true" Style="font-weight: bold;"
                                        ForeColor="#660033" Text='<%#Eval("calledStudent") %>'></asp:Label>
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Right" VerticalAlign="Middle" Width="50px" />
                            </asp:TemplateField>
                        </Columns>
                        <Columns>
                            <asp:TemplateField HeaderText="Registered">
                                <ItemTemplate>
                                    <asp:Label ID="lblRegistered" runat="server" Visible="true" Style="font-weight: bold;"
                                        ForeColor="#9933FF" Text='<%#Eval("registered") %>'></asp:Label>
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Right" VerticalAlign="Middle" Width="50px" />
                            </asp:TemplateField>
                        </Columns>
                        <Columns>
                            <asp:TemplateField HeaderText="Verified">
                                <ItemTemplate>
                                    <asp:Label ID="lblVerified" runat="server" Visible="true" Style="font-weight: bold;"
                                        ForeColor="#993366" Text='<%#Eval("verified") %>'></asp:Label>
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Right" VerticalAlign="Middle" Width="50px" />
                            </asp:TemplateField>
                        </Columns>
                        <Columns>
                            <asp:TemplateField HeaderText="Admited">
                                <ItemTemplate>
                                    <asp:Label ID="lblAdmited" runat="server" ForeColor="Green" Style="font-weight: bold;"
                                        Visible="true" Text='<%#Eval("admited") %>'></asp:Label>
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Right" VerticalAlign="Middle" Width="50px" />
                            </asp:TemplateField>
                        </Columns>
                        <Columns>
                            <asp:TemplateField HeaderText="Hostel">
                                <ItemTemplate>
                                    <asp:Label ID="lblHostel" runat="server" Visible="true" Style="font-weight: bold;"
                                        ForeColor="#006666" Text='<%#Eval("hostel") %>'></asp:Label>
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Right" VerticalAlign="Middle" Width="50px" />
                            </asp:TemplateField>
                        </Columns>
                        <Columns>
                            <asp:TemplateField HeaderText="Transport">
                                <ItemTemplate>
                                    <asp:Label ID="lblTransport" runat="server" Visible="true" Style="font-weight: bold;"
                                        ForeColor="#CC9900" Text='<%#Eval("transport") %>'></asp:Label>
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Right" VerticalAlign="Middle" Width="50px" />
                            </asp:TemplateField>
                        </Columns>
                        <FooterStyle BackColor="#1C5E55" Font-Bold="True" ForeColor="White" />
                        <PagerStyle BackColor="#666666" ForeColor="White" HorizontalAlign="Center" />
                        <SelectedRowStyle BackColor="#C5BBAF" Font-Bold="True" ForeColor="#333333" />
                        <HeaderStyle BackColor="#0ca6ca" Font-Bold="True" ForeColor="White" />
                        <EditRowStyle BackColor="#7C6F57" />
                        <AlternatingRowStyle BackColor="White" />
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
                                        <asp:Button ID="btnPopAlertClose" runat="server" CssClass=" textbox btn2 comm" Font-Size="Medium"
                                            Font-Bold="True" Font-Names="Book Antiqua" Style="height: auto; width: auto;"
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

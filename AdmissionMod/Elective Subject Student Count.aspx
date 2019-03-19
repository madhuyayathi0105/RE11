<%@ Page Title="Staff Wise Elective Subject Student Count Master Settings" Language="C#"
    MasterPageFile="~/AdmissionMod/AttendanceSubSiteMaster.master" AutoEventWireup="true"
    CodeFile="Elective Subject Student Count.aspx.cs" Inherits="AdmissionMod_Elective_Subject_Student_Count" %>

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
    <script type="text/javascript">
        function clickCells(rowIndex) {
            var StudentCount = document.getElementById(gvStudentCount[rowIndex]);
            var StudentCountLbl = document.getElementById(gvStudentCount_lbl[rowIndex]);
            if (typeof StudentCount !== 'undefined' && StudentCount != null) {
                if (StudentCountLbl.style.display == 'block') {
                    StudentCount.style.display = 'block';
                    StudentCountLbl.style.display = 'none';
                    StudentCount.focus();
                }
                else {
                    StudentCount.style.display = 'none';
                    StudentCountLbl.style.display = 'block';
                }
                StudentCount.focus();
            }
        }
        function focusOut(rowIndex) {
            var StudentCount = document.getElementById(gvStudentCount[rowIndex]);
            var StudentCountLbl = document.getElementById(gvStudentCount_lbl[rowIndex]);
            if (typeof StudentCount !== 'undefined' && StudentCount != null) {
                StudentCountLbl.innerHTML = StudentCount.value;
                if (StudentCount.value != '') {
                    if (StudentCount.style.display == 'block') {
                        StudentCount.style.display = 'none';
                        StudentCountLbl.style.display = 'block';
                    }
                    else {
                        StudentCount.style.display = 'block';
                        StudentCountLbl.style.display = 'none';
                    }
                }
                StudentCount.style.display = 'none';
                StudentCountLbl.style.display = 'block';
            }
        }
        function textBoxClick(rowIndex) {
            var StudentCount = document.getElementById(gvStudentCount[rowIndex]);
            var StudentCountLbl = document.getElementById(gvStudentCount_lbl[rowIndex]);
            if (typeof StudentCount !== 'undefined' && StudentCount != null && StudentCount.value != '') {
                StudentCountLbl.innerHTML = StudentCount.value;
                StudentCountLbl.style.display = 'none';
                StudentCount.style.display = 'block';
                StudentCount.focus();
            }
        }

        function textBoxClicknEW(rowIndex) {
            var StudentCount = document.getElementById(gvStudentCount[rowIndex]);
            var StudentCountLbl = document.getElementById(gvStudentCount_lbl[rowIndex]);
            if (typeof StudentCount !== 'undefined' && StudentCount != null && StudentCount.value != '') {
                StudentCountLbl.innerHTML = StudentCount.value;
                StudentCountLbl.style.display = 'none';
                StudentCount.style.display = 'block';
                StudentCount.focus();
            }
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <center>
        <span id="spHeader" class="fontstyleheader" style="margin: 0px; margin-bottom: 10px;
            margin-top: 10px; position: relative; color: Green; font-weight: bold;">Staff Wise
            Elective Subject Student Count Settings</span>
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
                                    <div style="position: relative;">
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
                                    </div>
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
                                    <asp:Button ID="btnGo" CssClass="textbox textbox1" runat="server" Style="width: auto;
                                        height: auto; font-family: 'Book Antiqua'; font-weight: bold; font-size: medium;"
                                        Text="Go" OnClick="btnGo_Click" />
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
                                    <asp:CheckBox ID="cbSectionWise" runat="server" Text="Include Section Wise Count" />
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
                    <asp:GridView ID="gvSectionWiseCount" runat="server" CaptionAlign="Top" HorizontalAlign="Justify"
                        CellPadding="4" ForeColor="#333333" BorderStyle="Solid" AutoGenerateColumns="false"
                        GridLines="Both" Style="width: 100%; height: auto;" OnPreRender="gvSectionWiseCount_PreRender"
                        OnDataBound="gvSectionWiseCount_DataBound">
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
                            <asp:TemplateField HeaderText="Degree Name">
                                <ItemTemplate>
                                    <asp:Label ID="lblDegreeName" runat="server" Visible="true" Text='<%#Eval("DegreeName") %>'></asp:Label>
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" Width="200px" />
                            </asp:TemplateField>
                        </Columns>
                        <Columns>
                            <asp:TemplateField HeaderText="Subject Detail">
                                <ItemTemplate>
                                    <asp:Label ID="lblSubjectDet" runat="server" Visible="true" Style="font-weight: bold;"
                                        ForeColor="#003366" Text='<%#Eval("subjectDetails") %>'></asp:Label>
                                    <asp:Label ID="lblSubjectNo" runat="server" Style="display: none;" Text='<%#Eval("subject_no") %>'></asp:Label>
                                    <asp:Label ID="lblSubjectCode" runat="server" Style="font-weight: bold; display: none;"
                                        Text='<%#Eval("subject_code") %>'></asp:Label>
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" Width="130px" />
                            </asp:TemplateField>
                        </Columns>
                        <Columns>
                            <asp:TemplateField HeaderText="Staff Detail">
                                <ItemTemplate>
                                    <asp:Label ID="lblStaffName" runat="server" Visible="true" Style="font-weight: bold;"
                                        ForeColor="#660033" Text='<%#Eval("staffDetails") %>'></asp:Label>
                                    <asp:Label ID="lblStaffApplId" runat="server" Style="font-weight: bold; display: none;"
                                        Text='<%#Eval("appl_id") %>'></asp:Label>
                                    <asp:Label ID="lblStaffCode" runat="server" Style="font-weight: bold; display: none;"
                                        Text='<%#Eval("staff_code") %>'></asp:Label>
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" Width="130px" />
                            </asp:TemplateField>
                        </Columns>
                        <Columns>
                            <asp:TemplateField HeaderText="Student Count">
                                <ItemTemplate>
                                    <asp:TextBox ID="txtStudentCount" runat="server" Visible="true" Style="font-weight: bold;"
                                        Width="50px" Text='<%#Eval("studentCount") %>'></asp:TextBox>
                                    <asp:FilteredTextBoxExtender runat="server" ID="filterStudentCount" FilterType="Numbers"
                                        TargetControlID="txtStudentCount">
                                    </asp:FilteredTextBoxExtender>
                                    <asp:Label ID="lblDegreeCode" runat="server" Style="display: none;" Text='<%#Eval("Degree_Code") %>'></asp:Label>
                                    <asp:Label ID="lblStudentCount" runat="server" Style="font-weight: bold; display: none;"
                                        Text='<%#Eval("studentCount") %>'></asp:Label>
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="40px" />
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
                <div id="divmaincontentSectionwise" visible="false" runat="server" style="margin: 0px;
                    margin-bottom: 20px; margin-top: 20px;">
                    <asp:GridView ID="DivgvSectionWiseCount" runat="server" CaptionAlign="Top" HorizontalAlign="Justify"
                        CellPadding="4" ForeColor="#333333" BorderStyle="Solid" AutoGenerateColumns="false"
                        GridLines="Both" Style="width: 100%; height: auto;" OnDataBound="gvSection_DataBound">
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
                            <asp:TemplateField HeaderText="Degree Name">
                                <ItemTemplate>
                                    <asp:Label ID="lblDegreeName" runat="server" Visible="true" Text='<%#Eval("DegreeName") %>'></asp:Label>
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" Width="200px" />
                            </asp:TemplateField>
                        </Columns>
                        <Columns>
                            <asp:TemplateField HeaderText="Subject Detail">
                                <ItemTemplate>
                                    <asp:Label ID="lblSubjectDet" runat="server" Visible="true" Style="font-weight: bold;"
                                        ForeColor="#003366" Text='<%#Eval("subjectDetails") %>'></asp:Label>
                                    <asp:Label ID="lblSubjectNo" runat="server" Style="display: none;" Text='<%#Eval("subject_no") %>'></asp:Label>
                                    <asp:Label ID="lblSubjectCode" runat="server" Style="font-weight: bold; display: none;"
                                        Text='<%#Eval("subject_code") %>'></asp:Label>
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" Width="130px" />
                            </asp:TemplateField>
                        </Columns>
                        <Columns>
                            <asp:TemplateField HeaderText="Staff Detail">
                                <ItemTemplate>
                                    <asp:Label ID="lblStaffName" runat="server" Visible="true" Style="font-weight: bold;"
                                        ForeColor="#660033" Text='<%#Eval("staffDetails") %>'></asp:Label>
                                    <asp:Label ID="lblStaffApplId" runat="server" Style="font-weight: bold; display: none;"
                                        Text='<%#Eval("appl_id") %>'></asp:Label>
                                    <asp:Label ID="lblStaffCode" runat="server" Style="font-weight: bold; display: none;"
                                        Text='<%#Eval("staff_code") %>'></asp:Label>
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" Width="130px" />
                            </asp:TemplateField>
                        </Columns>
                        <Columns>
                            <asp:TemplateField HeaderText="Section">
                                <ItemTemplate>
                                    <asp:Label ID="lblSection" runat="server" Visible="true" Style="font-weight: bold;"
                                        ForeColor="#660033" Text='<%#Eval("Section") %>'></asp:Label>
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="60px" />
                            </asp:TemplateField>
                        </Columns>
                        <Columns>
                            <asp:TemplateField HeaderText="Student Count">
                                <ItemTemplate>
                                    <asp:TextBox ID="txtStudentCount" runat="server" Visible="true" Style="font-weight: bold;
                                        text-align: center;" Width="50px" Text='<%#Eval("studentCount") %>'></asp:TextBox>
                                    <asp:FilteredTextBoxExtender runat="server" ID="filterStudentCount" FilterType="Numbers"
                                        TargetControlID="txtStudentCount">
                                    </asp:FilteredTextBoxExtender>
                                    <asp:Label ID="lblDegreeCode" runat="server" Style="display: none;" Text='<%#Eval("Degree_Code") %>'></asp:Label>
                                    <asp:Label ID="lblStudentCount" runat="server" Style="font-weight: bold; display: none;"
                                        Text='<%#Eval("studentCount") %>'></asp:Label>
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="40px" />
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

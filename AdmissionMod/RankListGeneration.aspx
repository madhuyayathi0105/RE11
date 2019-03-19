<%@ Page Title="" Language="C#" MasterPageFile="~/AdmissionMod/AttendanceSubSiteMaster.master"
    AutoEventWireup="true" CodeFile="RankListGeneration.aspx.cs" Inherits="AdmissionMod_RankListGeneration" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">
    <script type="text/javascript">
        function PrintGrid() {
            var panel = document.getElementById('<%=ShwoDiv.ClientID %>');
            var college = document.getElementById("<%=ddlCollege.ClientID %>");
            college = college.options[college.selectedIndex].text;

            var batch = document.getElementById("<%=ddlbatch.ClientID %>").value;
            var edulevel = document.getElementById("<%=ddlEduLev.ClientID %>").value;

            var course = document.getElementById("<%=ddlcourse.ClientID %>");
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

        function myFunction() {
            var r = confirm("Do you want generate rank list ?.");
            if (r == true) {
                return true;
            } else {
                return false;
            }
        }
    </script>
    <center>
        <div class="maindivstyle" style="width: 950px;">
            <center>
                <span class="fontstyleheader" style="color: Green;">Rank List Generation</span>
            </center>
            <asp:ScriptManager ID="scptMgrNew" runat="server">
            </asp:ScriptManager>
            <table class="maintablestyle">
                <tr>
                    <td>
                        <asp:Label ID="lblCollege" runat="server" Text="Institute"></asp:Label>
                    </td>
                    <td>
                        <asp:DropDownList ID="ddlCollege" runat="server" CssClass="textbox ddlheight5">
                        </asp:DropDownList>
                    </td>
                    <td>
                        <asp:Label ID="lblBatch" runat="server" Text="Batch"></asp:Label>
                    </td>
                    <td>
                        <asp:DropDownList ID="ddlbatch" runat="server" CssClass="textbox ddlheight">
                        </asp:DropDownList>
                    </td>
                    <td>
                        <asp:Label ID="lblEdulevel" runat="server" Text="Edu Level"></asp:Label>
                    </td>
                    <td>
                        <asp:DropDownList ID="ddlEduLev" runat="server" CssClass="textbox ddlheight">
                        </asp:DropDownList>
                    </td>
                    <td>
                        <asp:Label ID="lblCourse" runat="server" Text="Course"></asp:Label>
                    </td>
                    <td>
                        <asp:DropDownList ID="ddlcourse" runat="server" CssClass="textbox ddlheight">
                        </asp:DropDownList>
                    </td>
                    <td>
                        <asp:Label ID="lblStream" runat="server" Text="Stream"></asp:Label>
                    </td>
                    <td>
                        <asp:DropDownList ID="ddlStream" runat="server" CssClass="textbox ddlheight2">
                        </asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <td colspan="5">
                        <asp:Button ID="btnShowDetails" runat="server" CssClass="textbox btn" Width="100px"
                            Text="Show Details" OnClick="btnShowDetails_Click" />
                        <asp:Button ID="btnGenOption" runat="server" CssClass="textbox btn" Width="100px"
                            Text="Generate" OnClick="btnGenOption_Click" OnClientClick="return myFunction()" />
                        <asp:Button ID="btnGenerateSettings" runat="server" CssClass="textbox btn" Width="150px"
                            Text="Rank Category Settings" OnClick="btnGenerateSettings_Click" />
                        <asp:Button ID="btnBordWiseMaximumMark" runat="server" CssClass="textbox btn" Width="174px"
                            Text="Board Wise Topper Mark" OnClick="btnBordWiseMaximumMark_Click" />
                        <asp:Button ID="btnBasePrint" runat="server" Text="Print" CssClass="textbox  btn2"
                            Width="60px" Visible="false" OnClientClick="return PrintGrid()" />
                    </td>
                    <td>
                        <asp:Label ID="lbltest" runat="server"></asp:Label>
                    </td>
                </tr>
            </table>
            <br />
            <div id="ShwoDiv" runat="server">
            </div>
        </div>
    </center>
    <center>
        <div id="divImport" runat="server" visible="false" class="popupstyle popupheight1 "
            style="height: 300em;">
            <asp:ImageButton ID="imgbtnImport" runat="server" OnClick="closeTTImport" Width="40px"
                Height="40px" ImageUrl="~/images/close.png" Style="height: 30px; width: 30px;
                position: absolute; margin-top: 37px; margin-left: 380px;" />
            <br />
            <br />
            <center>
                <div style="width: 800px; height: 500px; overflow: auto; background-color: White;
                    border: 1px solid #0CA6CA; border-top: 25px solid #0CA6CA; border-radius: 10px;">
                    <center>
                        <center>
                            <span class="fontstyleheader" style="color: Green;">Settings</span>
                        </center>
                        <br />
                        <table>
                            <tr>
                                <td>
                                    <asp:Button ID="btn_addtype" runat="server" Text="+" CssClass="textbox textbox1 btn1"
                                        OnClick="btn_addtype_OnClick" />
                                </td>
                                <td>
                                    <asp:DropDownList ID="ddl_coltypeadd" runat="server" CssClass="textbox textbox1 ddlheight4">
                                    </asp:DropDownList>
                                </td>
                                <td>
                                    <asp:Button ID="btn_deltype" runat="server" Text="-" CssClass="textbox textbox1 btn1"
                                        OnClick="btn_deltype_OnClick" />
                                </td>
                                <td>
                                    <asp:Label ID="lblDistrict" runat="server" Text="HSC District"></asp:Label>
                                    <fieldset>
                                        <asp:Panel ID="Panel4" runat="server" ScrollBars="Auto" Style="height: 109px; width: 150px;">
                                            <asp:CheckBoxList ID="MultipleSelection" runat="server">
                                            </asp:CheckBoxList>
                                        </asp:Panel>
                                    </fieldset>
                                </td>
                                <td>
                                    <asp:Label ID="Label1" runat="server" Text="SSLC District"></asp:Label>
                                    <fieldset>
                                        <asp:Panel ID="Panel1" runat="server" ScrollBars="Auto" Style="height: 109px; width: 150;">
                                            <asp:CheckBoxList ID="MultipleSelectionSSLC" runat="server">
                                            </asp:CheckBoxList>
                                        </asp:Panel>
                                    </fieldset>
                                </td>
                                <td>
                                    <asp:Button ID="btnImportExcel" runat="server" Text="Add Criteria" CssClass="textbox btn"
                                        Width="120px" OnClick="btnImportExcel_Click" />
                                </td>
                            </tr>
                        </table>
                        <br />
                        <br />
                        <asp:GridView ID="CriteriaGrid" runat="server" HeaderStyle-BackColor="#0CA6CA">
                        </asp:GridView>
                    </center>
                </div>
            </center>
        </div>
    </center>
    <div id="imgdiv33" runat="server" visible="false" style="height: 100%; z-index: 1000;
        width: 100%; background-color: rgba(54, 25, 25, .2); position: absolute; top: 0;
        left: 0px;">
        <center>
            <div id="panel_description11" runat="server" visible="false" class="table" style="background-color: White;
                height: 120px; width: 467px; border: 5px solid #0CA6CA; border-top: 25px solid #0CA6CA;
                margin-top: 200px; border-radius: 10px;">
                <table>
                    <tr>
                        <td align="center">
                            <asp:Label ID="lbl_description111" runat="server" Text="Criteria Name" Font-Bold="true"
                                Font-Size="Medium"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td align="center">
                            <asp:TextBox ID="txt_description11" runat="server" Width="400px" Style="font-family: 'Book Antiqua';
                                margin-left: 13px" Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td align="center">
                            <asp:Button ID="btn_adddesc1" runat="server" Text="Add" Font-Bold="True" Font-Names="Book Antiqua"
                                Font-Size="Medium" CssClass="textbox btn1" OnClick="btndescpopadd_Click" />
                            <asp:Button ID="btn_exitdesc1" runat="server" Text="Exit" Font-Bold="True" Font-Names="Book Antiqua"
                                Font-Size="Medium" CssClass="textbox btn1" OnClick="btndescpopexit_Click" />
                        </td>
                    </tr>
                </table>
            </div>
        </center>
    </div>
    <div id="imgdiv2" runat="server" visible="false" style="height: 300em; z-index: 1000;
        width: 100%; background-color: rgba(54, 25, 25, .2); position: absolute; top: 0;
        left: 0px;">
        <center>
            <div id="pnl2" runat="server" class="table" style="background-color: White; height: 120px;
                width: 238px; border: 5px solid #0CA6CA; border-top: 25px solid #0CA6CA; margin-top: 400px;
                border-radius: 10px;">
                <center>
                    <table style="height: 100px; width: 100%">
                        <tr>
                            <td align="center">
                                <asp:Label ID="lbl_alert" runat="server" Text="" Style="color: Red;" Font-Bold="true"
                                    Font-Size="Medium"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <center>
                                    <asp:Button ID="btn_errorclose" CssClass=" textbox btn1 comm" Style="height: 28px;
                                        width: 65px;" OnClick="btn_errorclose_Click" Text="ok" runat="server" />
                                </center>
                            </td>
                        </tr>
                    </table>
                </center>
            </div>
        </center>
    </div>
    <center>
        <div id="DivBoradWiseSetMark" runat="server" visible="false" class="popupstyle popupheight1 "
            style="height: 300em;">
            <asp:ImageButton ID="imageButtonNew" runat="server" OnClick="imageButtonNew_Clcik"
                Width="40px" Height="40px" ImageUrl="~/images/close.png" Style="height: 30px;
                width: 30px; position: absolute; margin-top: 10px; margin-left: 450px;" />
            <br />
            <center>
                <div style="width: 950px; height: 700px; overflow: auto; background-color: White;
                    border: 1px solid #0CA6CA; border-top: 25px solid #0CA6CA; border-radius: 10px;">
                    <center>
                        <center>
                            <span class="fontstyleheader" style="color: Green;">Board Wise Topper Mark Settings</span>
                        </center>
                        <br />
                        <asp:GridView ID="GridBoardWiseMaxMark" runat="server" HeaderStyle-BackColor="#0CA6CA"
                            AutoGenerateColumns="false">
                            <Columns>
                                <asp:TemplateField HeaderText="S.No" HeaderStyle-HorizontalAlign="Center">
                                    <ItemTemplate>
                                        <asp:Label ID="lblsno" runat="server" Text="<%# Container.DisplayIndex+1 %>"></asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Center" Width="50px" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Board" HeaderStyle-HorizontalAlign="Center">
                                    <ItemTemplate>
                                        <asp:Label ID="lblboardName" runat="server" Text='<%# Eval("TextVal") %>'></asp:Label>
                                        <asp:Label ID="lblboardCode" runat="server" Visible="false" Text='<%# Eval("board") %>'></asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Left" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Topper Mark" HeaderStyle-HorizontalAlign="Center">
                                    <ItemTemplate>
                                        <asp:TextBox ID="txt_MaxTopper" runat="server" MaxLength="4" Width="50px" Style="text-align: center;"></asp:TextBox>
                                        <asp:FilteredTextBoxExtender ID="TextBoxExtender" runat="server" TargetControlID="txt_MaxTopper"
                                            FilterType="Numbers">
                                        </asp:FilteredTextBoxExtender>
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Center" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Maximum Mark" HeaderStyle-HorizontalAlign="Center">
                                    <ItemTemplate>
                                        <asp:TextBox ID="txt_MaxMark" runat="server" MaxLength="4" Width="50px" Style="text-align: center;"></asp:TextBox>
                                        <asp:FilteredTextBoxExtender ID="TextBoxExtender1" runat="server" TargetControlID="txt_MaxMark"
                                            FilterType="Numbers">
                                        </asp:FilteredTextBoxExtender>
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Center" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Maths Mark" HeaderStyle-HorizontalAlign="Center">
                                    <ItemTemplate>
                                        <asp:TextBox ID="txt_MathsMax" runat="server" MaxLength="4" Width="50px" Style="text-align: center;"></asp:TextBox>
                                        <asp:FilteredTextBoxExtender ID="TextBoxExtender2" runat="server" TargetControlID="txt_MaxMark"
                                            FilterType="Numbers">
                                        </asp:FilteredTextBoxExtender>
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Center" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Physics Mark" HeaderStyle-HorizontalAlign="Center">
                                    <ItemTemplate>
                                        <asp:TextBox ID="txt_Physics" runat="server" MaxLength="4" Width="50px" Style="text-align: center;"></asp:TextBox>
                                        <asp:FilteredTextBoxExtender ID="TextBoxExtender3" runat="server" TargetControlID="txt_Physics"
                                            FilterType="Numbers">
                                        </asp:FilteredTextBoxExtender>
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Center" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Chemistry Mark" HeaderStyle-HorizontalAlign="Center">
                                    <ItemTemplate>
                                        <asp:TextBox ID="txt_Chemistry" runat="server" MaxLength="4" Width="50px" Style="text-align: center;"></asp:TextBox>
                                        <asp:FilteredTextBoxExtender ID="TextBoxExtender4" runat="server" TargetControlID="txt_Chemistry"
                                            FilterType="Numbers">
                                        </asp:FilteredTextBoxExtender>
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Center" />
                                </asp:TemplateField>
                            </Columns>
                        </asp:GridView>
                        <br />
                        <asp:Button ID="btnMaxMarkSave" runat="server" CssClass="textbox btn" Width="100px"
                            Text="Save" OnClick="btnMaxMarkSave_Click" />
                    </center>
                </div>
            </center>
        </div>
    </center>
</asp:Content>

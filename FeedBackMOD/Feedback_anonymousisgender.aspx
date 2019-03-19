<%@ Page Title="" Language="C#" MasterPageFile="~/FeedBackMOD/FeedbackSubSiteMaster.master"
    AutoEventWireup="true" CodeFile="Feedback_anonymousisgender.aspx.cs" Inherits="FeedBackMod_Feedback_anonymousisgender" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">
    <body>
        <script type="text/javascript">
            function myFunction(x) {
                x.style.borderColor = "#c4c4c4";
            }
            function PrintPanel() {
                var subjectwise = document.getElementById("<%=rb_Subjectswise.ClientID %>");
                var panel = "";
                if (subjectwise.checked) {
                    panel = document.getElementById("<%=chart_div.ClientID %>");
                } else { panel = document.getElementById("<%=chart_div1.ClientID %>"); }
                var printWindow = window.open('', '', 'height=auto,width=auto');
                printWindow.document.write('<html');
                printWindow.document.write('<head>');
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
        <asp:ScriptManager ID="ScriptManager1" runat="server">
        </asp:ScriptManager>
        <center>
            <div>
                <span class="fontstyleheader" style="color: Green">FeedBack Report</span>
            </div>
            <br />
        </center>
        <div class="maindivstyle">
            <center>
                <table class="maintablestyle">
                    <tr>
                        <td>
                            <asp:Label ID="lbl_college" runat="server" Text="College"></asp:Label>
                        </td>
                        <td>
                            <asp:UpdatePanel ID="UpdatePanel20" runat="server">
                                <ContentTemplate>
                                    <asp:TextBox ID="Txt_college" Width=" 90px" ReadOnly="true" runat="server" CssClass="textbox  txtheight2">--Select--</asp:TextBox>
                                    <asp:Panel ID="Panel_college" runat="server" CssClass="multxtpanel">
                                        <asp:CheckBox ID="Cb_college" runat="server" Text="Select All" AutoPostBack="True"
                                            OnCheckedChanged="Cb_college_CheckedChanged" />
                                        <asp:CheckBoxList ID="Cbl_college" runat="server" AutoPostBack="True" OnSelectedIndexChanged="Cbl_college_SelectedIndexChanged">
                                        </asp:CheckBoxList>
                                    </asp:Panel>
                                    <asp:PopupControlExtender ID="PopupControlExtender20" runat="server" TargetControlID="Txt_college"
                                        PopupControlID="Panel_college" Position="Bottom">
                                    </asp:PopupControlExtender>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </td>
                        <td>
                            <asp:Label ID="lbl_Batchyear" runat="server" Text="Batch Year"></asp:Label>
                        </td>
                        <td>
                            <asp:UpdatePanel ID="Updp_Batchyear" runat="server">
                                <ContentTemplate>
                                    <asp:TextBox ID="txt_batch" ReadOnly="true" Width=" 90px" runat="server" CssClass="textbox  txtheight2">--Select--</asp:TextBox>
                                    <asp:Panel ID="Panel_Batchyear" runat="server" Height="200" CssClass="multxtpanel">
                                        <asp:CheckBox ID="cb_batch" runat="server" Text="Select All" AutoPostBack="true"
                                            OnCheckedChanged="cb_batch_CheckedChanged" />
                                        <asp:CheckBoxList ID="cbl_batch" runat="server" AutoPostBack="true" OnSelectedIndexChanged="cbl_batch_SelectedIndexChanged">
                                        </asp:CheckBoxList>
                                    </asp:Panel>
                                    <asp:PopupControlExtender ID="PopupControlExtender21" runat="server" TargetControlID="txt_batch"
                                        PopupControlID="Panel_Batchyear" Position="Bottom">
                                    </asp:PopupControlExtender>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </td>
                        <td>
                            <asp:Label ID="lbl_Degree" Width="50px" runat="server" Text="Degree"></asp:Label>
                        </td>
                        <td>
                            <asp:UpdatePanel ID="Updp_Degree" runat="server">
                                <ContentTemplate>
                                    <asp:TextBox ID="txt_degree" Width=" 90px" runat="server" CssClass="textbox  txtheight2"
                                        ReadOnly="true">--Select--</asp:TextBox>
                                    <asp:Panel ID="Panel_Degree" runat="server" Height="200" CssClass="multxtpanel">
                                        <asp:CheckBox ID="cb_degree" runat="server" Text="Select All" AutoPostBack="True"
                                            OnCheckedChanged="cb_degree_CheckedChanged" />
                                        <asp:CheckBoxList ID="cbl_degree" runat="server" AutoPostBack="True" OnSelectedIndexChanged="cbl_degree_SelectedIndexChanged">
                                        </asp:CheckBoxList>
                                    </asp:Panel>
                                    <asp:PopupControlExtender ID="PopupControlExtender9" runat="server" TargetControlID="txt_degree"
                                        PopupControlID="Panel_Degree" Position="Bottom">
                                    </asp:PopupControlExtender>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </td>
                        <td>
                            <asp:Label ID="lbl_dpt" runat="server" Width="75px" Text="Department"></asp:Label>
                        </td>
                        <td>
                            <asp:UpdatePanel ID="UpdatePanel23" runat="server">
                                <ContentTemplate>
                                    <asp:TextBox ID="txt_branch" Width=" 91px" ReadOnly="true" runat="server" CssClass="textbox  txtheight2">--Select--</asp:TextBox>
                                    <asp:Panel ID="Panel_dpt" runat="server" CssClass="multxtpanel" Height="350px">
                                        <asp:CheckBox ID="cb_branch" runat="server" Text="Select All" AutoPostBack="True"
                                            OnCheckedChanged="cb_branch_CheckedChanged" />
                                        <asp:CheckBoxList ID="cbl_branch" runat="server" AutoPostBack="True" OnSelectedIndexChanged="cbl_branch_SelectedIndexChanged">
                                        </asp:CheckBoxList>
                                    </asp:Panel>
                                    <asp:PopupControlExtender ID="PopupControlExtender23" runat="server" TargetControlID="txt_branch"
                                        PopupControlID="Panel_dpt" Position="Bottom">
                                    </asp:PopupControlExtender>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </td>
                        <td>
                            <asp:Label ID="lbl_sem" runat="server" Text="Semester"></asp:Label>
                        </td>
                        <td>
                            <asp:UpdatePanel ID="UpdatePanel24" runat="server">
                                <ContentTemplate>
                                    <asp:TextBox ID="txt_sem" Width="85px" ReadOnly="true" runat="server" CssClass="textbox  txtheight2">--Select--</asp:TextBox>
                                    <asp:Panel ID="Panel_Sem" runat="server" CssClass="multxtpanel">
                                        <asp:CheckBox ID="cb_sem" runat="server" Text="Select All" AutoPostBack="True" OnCheckedChanged="cb_sem_CheckedChanged" />
                                        <asp:CheckBoxList ID="cbl_sem" runat="server" AutoPostBack="True" OnSelectedIndexChanged="cbl_sem_SelectedIndexChanged">
                                        </asp:CheckBoxList>
                                    </asp:Panel>
                                    <asp:PopupControlExtender ID="PopupControlExtender24" runat="server" TargetControlID="txt_sem"
                                        PopupControlID="Panel_Sem" Position="Bottom">
                                    </asp:PopupControlExtender>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label ID="lbl_Sec" runat="server" Text="Section"></asp:Label>
                        </td>
                        <td>
                            <asp:UpdatePanel ID="UpdatePanel25" runat="server">
                                <ContentTemplate>
                                    <asp:TextBox ID="txt_sec" Width=" 90px" ReadOnly="true" runat="server" CssClass="textbox  txtheight2">--Select--</asp:TextBox>
                                    <asp:Panel ID="Panel_Sec" runat="server" CssClass="multxtpanel">
                                        <asp:CheckBox ID="cb_sec" runat="server" Text="Select All" AutoPostBack="True" OnCheckedChanged="cb_sec_CheckedChanged" />
                                        <asp:CheckBoxList ID="cbl_sec" runat="server" AutoPostBack="True" OnSelectedIndexChanged="cbl_sec_SelectedIndexChanged">
                                        </asp:CheckBoxList>
                                    </asp:Panel>
                                    <asp:PopupControlExtender ID="PopupControlExtender25" runat="server" TargetControlID="txt_sec"
                                        PopupControlID="Panel_Sec" Position="Bottom">
                                    </asp:PopupControlExtender>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </td>
                        <td>
                            <asp:Label ID="fb_name" Width="115px" runat="server" Text="Feedback Name"></asp:Label>
                        </td>
                        <td colspan="2">
                            <asp:UpdatePanel ID="UpdatePanel1" Visible="true" runat="server">
                                <ContentTemplate>
                                    <asp:DropDownList ID="ddl_Feedbackname" runat="server" Height="30px" CssClass=" textbox1 ddlheight4"
                                        AutoPostBack="true" OnSelectedIndexChanged="ddl_Feedbackname_SelectedIndexChanged">
                                    </asp:DropDownList>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </td>
                        <td>
                            <asp:Label ID="lbl_subject" Width="100px" runat="server" Visible="true" Text="Subject Name"></asp:Label>
                        </td>
                        <td>
                            <asp:UpdatePanel ID="UpdatePanel5" Visible="true" runat="server">
                                <ContentTemplate>
                                    <asp:TextBox ID="Txt_Subject" Width=" 93px" ReadOnly="true" runat="server" CssClass="textbox  txtheight2">--Select--</asp:TextBox>
                                    <asp:Panel ID="Panel_Subject" runat="server" CssClass="multxtpanel" Height="200px"
                                        Width="200px">
                                        <asp:CheckBox ID="Cb_Subject" runat="server" Text="Select All" AutoPostBack="True"
                                            OnCheckedChanged="Cb_Subject_CheckedChanged" />
                                        <asp:CheckBoxList ID="Cbl_Subject" runat="server" AutoPostBack="True" OnSelectedIndexChanged="Cbl_Subject_SelectedIndexChanged">
                                        </asp:CheckBoxList>
                                    </asp:Panel>
                                    <asp:PopupControlExtender ID="PopupControlExtender5" runat="server" TargetControlID="Txt_Subject"
                                        PopupControlID="Panel_Subject" Position="Bottom">
                                    </asp:PopupControlExtender>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </td>
                        <td colspan="3">
                            <asp:CheckBox ID="cb_include" runat="server" Text="InClude Chart" />
                            <asp:DropDownList ID="ddl_charttype" runat="server" Height="30px" CssClass=" textbox1 ddlheight1">
                                <asp:ListItem Value="0">Bar Chart</asp:ListItem>
                                <asp:ListItem Value="1">Line Chart</asp:ListItem>
                            </asp:DropDownList>
                            <asp:Button ID="btn_go" runat="server" CssClass="textbox btn1" Text="Go" OnClick="btn_go_Click" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:RadioButton ID="rb_Subjectswise" runat="server" GroupName="login" Width="125px"
                                Text="Subject Wise" OnCheckedChanged="rb_Subjectswise_CheckedChanged" AutoPostBack="true"
                                Checked="true"></asp:RadioButton>
                        </td>
                        <td colspan="2">
                            <asp:RadioButton ID="rb_Subjectheaderswise" runat="server" Visible="true" GroupName="login"
                                Text="Subject Header wise" OnCheckedChanged="rb_Subjectheaderswise_CheckedChanged"
                                AutoPostBack="true"></asp:RadioButton>
                        </td>
                    </tr>
                </table>
                <br />
                <asp:Label ID="lbl_error" runat="server" ForeColor="Red"></asp:Label>
                <br />
                <FarPoint:FpSpread ID="FpSpread1" Width="900px" Visible="false" runat="server" BorderStyle="Solid"
                    BorderWidth="0px" CssClass="spreadborder" ShowHeaderSelection="false">
                    <Sheets>
                        <FarPoint:SheetView SheetName="Sheet1">
                        </FarPoint:SheetView>
                    </Sheets>
                </FarPoint:FpSpread>
                <br />
                <div id="chart_div" runat="server">
                    <asp:Chart ID="subjectwise_chart" runat="server" Height="500px" Visible="false" Font-Names="Book Antiqua"
                        Font-Size="Medium">
                        <Series>
                        </Series>
                        <Legends>
                            <asp:Legend Title="Subject wise Charts" ShadowOffset="3" Docking="Bottom" Font="Book Antiqua">
                            </asp:Legend>
                        </Legends>
                        <Titles>
                            <asp:Title Docking="Top" Text="Subject Wise Chart" Font="Microsoft Sans Serif, 12pt">
                            </asp:Title>
                            <asp:Title Docking="Bottom" Font="Book Antiqua" Text="Subject Code and Subject Name">
                            </asp:Title>
                            <asp:Title Docking="Left" Font="Book Antiqua" Text="Point Percentage">
                            </asp:Title>
                        </Titles>
                        <ChartAreas>
                            <asp:ChartArea Name="ChartArea2" BorderWidth="0">
                                <AxisY LineColor="White">
                                    <LabelStyle Font="Trebuchet MS, 8.25pt" />
                                    <MajorGrid LineColor="#e6e6e6" />
                                    <MinorGrid Enabled="false" LineColor="#e6e6e6" />
                                </AxisY>
                                <AxisX LineColor="White">
                                    <LabelStyle Font="Trebuchet MS, 8.25pt" />
                                    <MajorGrid LineColor="#e6e6e6" />
                                    <MinorGrid Enabled="false" LineColor="#e6e6e6" />
                                </AxisX>
                            </asp:ChartArea>
                        </ChartAreas>
                    </asp:Chart>
                </div>
                <div id="chart_div1" runat="server">
                    <asp:Chart ID="subjectheaderwise_chart" runat="server" Height="500px" Visible="false"
                        Font-Names="Book Antiqua" Font-Size="Medium">
                        <Series>
                        </Series>
                        <Legends>
                            <asp:Legend Title="Subject wise Chart" ShadowOffset="3" Docking="Bottom" Font="Book Antiqua">
                            </asp:Legend>
                        </Legends>
                        <Titles>
                            <asp:Title Docking="Top" Text="Subject Header Wise Chart" Font="Microsoft Sans Serif, 12pt">
                            </asp:Title>
                            <asp:Title Docking="Bottom" Font="Book Antiqua" Text="Subject Code and Subject Name">
                            </asp:Title>
                            <asp:Title Docking="Left" Font="Book Antiqua" Text="Point Percentage">
                            </asp:Title>
                        </Titles>
                        <ChartAreas>
                            <asp:ChartArea Name="ChartArea3" BorderWidth="0">
                                <AxisY LineColor="White">
                                    <LabelStyle Font="Trebuchet MS, 8.25pt" />
                                    <MajorGrid LineColor="#e6e6e6" />
                                    <MinorGrid Enabled="false" LineColor="#e6e6e6" />
                                </AxisY>
                                <AxisX LineColor="White">
                                    <LabelStyle Font="Trebuchet MS, 8.25pt" />
                                    <MajorGrid LineColor="#e6e6e6" />
                                    <MinorGrid Enabled="false" LineColor="#e6e6e6" />
                                </AxisX>
                            </asp:ChartArea>
                        </ChartAreas>
                    </asp:Chart>
                </div>
                <center>
                    <div id="rptprint1" runat="server" visible="false">
                        <br />
                        <asp:Label ID="lbl_norec1" runat="server" ForeColor="Red" Text="Please Enter Your Report Name"
                            Visible="false"></asp:Label>
                        <asp:Label ID="lblrptname1" runat="server" Text="Report Name"></asp:Label>
                        <asp:TextBox ID="txtexcelname1" runat="server" CssClass="textbox textbox1" Height="20px"
                            Width="180px" onkeypress="display1()"></asp:TextBox>
                        <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender9" runat="server" TargetControlID="txtexcelname1"
                            FilterType="LowercaseLetters,UppercaseLetters,Numbers,Custom" ValidChars="(),.[]_"
                            InvalidChars="/\">
                        </asp:FilteredTextBoxExtender>
                        <asp:Button ID="btnExcel1" runat="server" OnClick="btnExcel1_Click" Text="Export To Excel"
                            Width="127px" Height="31px" CssClass="textbox textbox1" />
                        <asp:Button ID="btnprintmaster1" runat="server" Text="Print" OnClick="btnprintmaster1_Click"
                            Width="60px" Height="31px" CssClass="textbox textbox1" />
                        <Insproplus:printmaster runat="server" ID="Printcontrol1" Visible="false" />
                        <asp:Button ID="btnprintimag" Text="Chart Print To PDF" Height="30px" runat="server"
                            CssClass="btn1 textbox txtheight2" OnClientClick="return PrintPanel();" />
                    </div>
                    <br />
                </center>
            </center>
            <center>
                <div id="imgdiv2" runat="server" visible="false" style="height: 100%; z-index: 1000;
                    width: 100%; background-color: rgba(54, 25, 25, .2); position: absolute; top: 0;
                    left: 0px;">
                    <center>
                        <div id="pnl2" runat="server" class="table" style="background-color: White; height: 120px;
                            width: 238px; border: 5px solid #0CA6CA; border-top: 25px solid #0CA6CA; margin-top: 200px;
                            border-radius: 10px;">
                            <center>
                                <table style="height: 100px; width: 100%">
                                    <tr>
                                        <td align="center">
                                            <asp:Label ID="lbl_alert1" runat="server" Text="" Style="color: Red;" Font-Bold="true"
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
            </center>
        </div>
    </body>
</asp:Content>

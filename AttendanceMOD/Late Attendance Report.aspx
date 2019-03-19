<%@ Page Title="" Language="C#" MasterPageFile="~/AttendanceMOD/AttendanceSubSiteMaster.master" AutoEventWireup="true" CodeFile="Late Attendance Report.aspx.cs" Inherits="LateAttendanceReport" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" Runat="Server">
  <!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
    <html xmlns="http://www.w3.org/1999/xhtml">
    <head id="Head1">
        <title></title>
        <link href="~/Styles/css/Commoncss.css" rel="stylesheet" type="text/css" />
        <style type="text/css">
            .maindivstylesize
            {
                height: 500px;
                width: 1000px;
            }
        </style>
    </head>
    <body>
      <form id="form1">
        <asp:ScriptManager ID="ScriptManager1" runat="server">
        </asp:ScriptManager>
        <center>
            <span style="color: #008000" class="fontstyleheader">Late Entry Report</span>
            <div style="margin-right: 853px; margin-top: 0px;">
                <asp:Label ID="lbltime" runat="server" ForeColor="#336699" Width="280px"><script>                                                                                             document.write(date())</script></asp:Label>

                </div>
                  <br />
        </center>
        <center>
            <div class="maindivstyle maindivstylesize">
                <br />
                <center>
                   <table class="maintablestyle" width="800px">
                         <tr>
                    <td colspan="10">
                        <fieldset style="width: 700px; height: 35px;">
                            <asp:RadioButton ID="rdbhostel" runat="server" Text="Cumlative" 
                                GroupName="Attendance" AutoPostBack="true" Checked="true" OnCheckedChanged="rdbmess_CheckedChange"  />
                            <asp:RadioButton ID="rdbmess" runat="server" Text="Count Wise" GroupName="Attendance"
                                OnCheckedChanged="rdbmess_CheckedChange" AutoPostBack="true" />
                            
                        </fieldset>
                    </td>
                </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="lbl_clgname" Width="100px" runat="server" Text="College"></asp:Label>
                                </td>
                                <td>
                                    <asp:DropDownList ID="ddlcollege" CssClass="ddlheight4 textbox textbox1" runat="server"
                                        AutoPostBack="true" OnSelectedIndexChanged="ddlcollege_SelectedIndexChanged">
                                    </asp:DropDownList>
                                </td>
                               
                                <td>
                                    <asp:Label ID="lbl_graduation" Width="100px" runat="server" Text="Graduation"></asp:Label>
                                </td>
                                <td>
                                    <asp:UpdatePanel ID="Upp1" runat="server">
                                        <ContentTemplate>
                                            <asp:TextBox ID="txt_graduation" runat="server" CssClass="textbox txtheight3 textbox1"
                                                ReadOnly="true">--Select--</asp:TextBox>
                                            <asp:Panel ID="p1" runat="server" BackColor="White" BorderColor="Black" BorderStyle="Solid"
                                                BorderWidth="2px" CssClass="multxtpanel" Style="width: 130px; height: 130px;">
                                                <asp:CheckBox ID="cb_graduation" runat="server" Text="Select All" AutoPostBack="True"
                                                    OnCheckedChanged="cb_graduation_checkedchange" />
                                                <asp:CheckBoxList ID="cbl_graduation" runat="server" AutoPostBack="true" OnSelectedIndexChanged="cbl_graduation_SelectedIndexChanged">
                                                </asp:CheckBoxList>
                                            </asp:Panel>
                                            <asp:PopupControlExtender ID="PopupControlExtender1" runat="server" TargetControlID="txt_graduation"
                                                PopupControlID="p1" Position="Bottom">
                                            </asp:PopupControlExtender>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                                <td>
                                    <asp:Label ID="lbl_batch" Width="102px" runat="server" Text="Batch"></asp:Label>
                                </td>
                                <td>
                                    <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                                        <ContentTemplate>
                                            <asp:TextBox ID="txt_batch" runat="server" Width="70px" CssClass="textbox txtheight1 textbox1"
                                                ReadOnly="true">--Select--</asp:TextBox>
                                            <asp:Panel ID="Panel3" runat="server" BackColor="White" BorderColor="Black" BorderStyle="Solid"
                                                BorderWidth="2px" CssClass="multxtpanel" Style="width: 120px; height: 150px;">
                                                <asp:CheckBox ID="cb_batch" runat="server" Text="Select All" AutoPostBack="True"
                                                    OnCheckedChanged="cb_batch_checkedchange" />
                                                <asp:CheckBoxList ID="cbl_batch" runat="server" AutoPostBack="true" OnSelectedIndexChanged="cbl_batch_SelectedIndexChanged">
                                                </asp:CheckBoxList>
                                            </asp:Panel>
                                            <asp:PopupControlExtender ID="PopupControlExtender3" runat="server" TargetControlID="txt_batch"
                                                PopupControlID="Panel3" Position="Bottom">
                                            </asp:PopupControlExtender>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="lbl_degree" Text="Degree" runat="server"></asp:Label>
                                </td>
                                <td>
                                 <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                                        <ContentTemplate>
                                   
                                            <asp:TextBox ID="txt_degree" runat="server" CssClass="textbox  textbox1 txtheight3"
                                                ReadOnly="true">-- Select--</asp:TextBox>
                                            <asp:Panel ID="p3" runat="server" BackColor="White" BorderColor="Black"
                                                BorderStyle="Solid" BorderWidth="2px" CssClass="multxtpanel" Height="250px" Width="120px"
                                                Style="position: absolute;">
                                                <asp:CheckBox ID="cb_degree" runat="server" Text="Select All" AutoPostBack="true"
                                                    OnCheckedChanged="cb_degree_checkedchange" />
                                                <asp:CheckBoxList ID="cbl_degree" runat="server" AutoPostBack="true" OnSelectedIndexChanged="cbl_degree_SelectedIndexChanged">
                                                </asp:CheckBoxList>
                                            </asp:Panel>
                                            <asp:PopupControlExtender ID="PopupControlExtender18" runat="server" TargetControlID="txt_degree"
                                                PopupControlID="p3" Position="Bottom">
                                            </asp:PopupControlExtender></ContentTemplate></asp:UpdatePanel>
                                        
                                </td>
                                <td>
                                    <asp:Label ID="lbl_branch" Text="Branch" runat="server"></asp:Label>
                                </td>
                                <td>
                                    <asp:UpdatePanel ID="UpdatePanel23" runat="server">
                                        <ContentTemplate>
                                            <asp:TextBox ID="txt_branch" runat="server" CssClass="textbox textbox1 txtheight3"
                                                ReadOnly="true">-- Select--</asp:TextBox>
                                            <asp:Panel ID="p4" runat="server" BackColor="White" BorderColor="Black" BorderStyle="Solid"
                                                BorderWidth="2px" CssClass="multxtpanel" Height="250px" Style="position: absolute;">
                                                <asp:CheckBox ID="cb_branch" runat="server" Text="Select All" AutoPostBack="true"
                                                    OnCheckedChanged="cb_branch_checkedchange" />
                                                <asp:CheckBoxList ID="cbl_branch" runat="server" AutoPostBack="true" OnSelectedIndexChanged="cbl_branch_SelectedIndexChanged">
                                                </asp:CheckBoxList>
                                            </asp:Panel>
                                            <asp:PopupControlExtender ID="PopupControlExtender19" runat="server" TargetControlID="txt_branch"
                                                PopupControlID="p4" Position="Bottom">
                                            </asp:PopupControlExtender>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                               <%-- <td>
                                    <asp:Label ID="lbl_org_sem" Text="Semester" runat="server"></asp:Label>
                                </td>
                                <td>
                                    <asp:UpdatePanel ID="UpdatePanel24" runat="server">
                                        <ContentTemplate>
                                            <asp:TextBox ID="txt_sem" runat="server" CssClass="textbox textbox1 txtheight3" ReadOnly="true">-- Select--</asp:TextBox>
                                            <asp:Panel ID="Panel11" runat="server" BackColor="White" BorderColor="Black" BorderStyle="Solid"
                                                BorderWidth="2px" CssClass="multxtpanel" Height="250px" Style="position: absolute;">
                                                <asp:CheckBox ID="cb_sem" runat="server" Text="Select All" AutoPostBack="true" OnCheckedChanged="cb_sem_checkedchange" />
                                                <asp:CheckBoxList ID="cbl_sem" runat="server" AutoPostBack="true" OnSelectedIndexChanged="cbl_sem_SelectedIndexChanged">
                                                </asp:CheckBoxList>
                                            </asp:Panel>
                                            <asp:PopupControlExtender ID="PopupControlExtender20" runat="server" TargetControlID="txt_sem"
                                                PopupControlID="Panel11" Position="Bottom">
                                            </asp:PopupControlExtender>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>--%>
                                <td>
                                    <asp:Label ID="lbl_sec" Text="Section" runat="server"></asp:Label>
                                </td>
                                <td>
                                    <asp:UpdatePanel ID="UpdatePanel9" runat="server">
                                        <ContentTemplate>
                                            <asp:TextBox ID="txt_sec" runat="server" Width="70px" CssClass="textbox textbox1 txtheight"
                                                ReadOnly="true">-- Select--</asp:TextBox>
                                            <asp:Panel ID="Panel8" runat="server" BackColor="White" BorderColor="Black" BorderStyle="Solid"
                                                BorderWidth="2px" CssClass="multxtpanel" Height="250px" Style="position: absolute;">
                                                <asp:CheckBox ID="cb_sec" runat="server" Text="Select All" AutoPostBack="true" OnCheckedChanged="cb_sec_checkedchange" />
                                                <asp:CheckBoxList ID="cbl_sec" runat="server" AutoPostBack="true" OnSelectedIndexChanged="cbl_sec_SelectedIndexChanged">
                                                </asp:CheckBoxList>
                                            </asp:Panel>
                                            <asp:PopupControlExtender ID="PopupControlExtender9" runat="server" TargetControlID="txt_sec"
                                                PopupControlID="Panel8" Position="Bottom">
                                            </asp:PopupControlExtender>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                            </tr>
                            <tr>
                               
                                <td>
                                   
                                    <asp:Label ID="lbl_fromdate" runat="server" Text="From Date"></asp:Label>
                                </td>
                                <td>
                                  <asp:UpdatePanel ID="UpdatePanel3" runat="server">
                                        <ContentTemplate>
                                            <asp:TextBox ID="txt_fromdate"  runat="server" CssClass="newtextbox txtheight textbox2"></asp:TextBox>
                                            <asp:CalendarExtender ID="CalendarExtender10" TargetControlID="txt_fromdate" runat="server"
                                                CssClass="cal_Theme1 ajax__calendar_active" Format="dd/MM/yyyy">
                                            </asp:CalendarExtender> </ContentTemplate></asp:UpdatePanel>
                                        
                                </td>
                                <td>
                                    <asp:Label ID="lbl_todate" runat="server" Text="To Date"></asp:Label>
                                </td>
                                <td>
                                    <asp:UpdatePanel ID="UpdatePanel4" runat="server">
                                        <ContentTemplate>
                                            <asp:TextBox ID="txt_todate"  runat="server" CssClass="newtextbox txtheight textbox2"></asp:TextBox>
                                            <asp:CalendarExtender ID="CalendarExtender1" TargetControlID="txt_todate" runat="server"
                                                CssClass="cal_Theme1 ajax__calendar_active" Format="dd/MM/yyyy">
                                            </asp:CalendarExtender>
                                        </ContentTemplate></asp:UpdatePanel>
                                </td>
                                <td>
                                    <asp:Button ID="btndetailgo" Visible="true" runat="server" Font-Bold="true" Font-Names="Book Antiqua"
                                        Text="Go" CssClass="textbox btn1 textbox1" OnClick="btndetailgo_Click" />
                                </td>
                                
                            </tr>
                            <tr>
                                
                                <td>
                                   
                                    <asp:Label ID="Label1" runat="server" Text="From Range" Visible="false"></asp:Label>
                                </td>
                                <td>
                                  <asp:UpdatePanel ID="UpdatePanel5" runat="server">
                                        <ContentTemplate>
                                            <asp:TextBox ID="TextBox1"  runat="server" CssClass="newtextbox txtheight textbox2"  Visible="false"></asp:TextBox>
                                          </ContentTemplate></asp:UpdatePanel>
                                        
                                </td>
   <td>
                                   
                                    <asp:Label ID="Label2" runat="server" Text="To Range"  Visible="false"></asp:Label>
                                </td>
                                <td>
                                  <asp:UpdatePanel ID="UpdatePanel6" runat="server">
                                        <ContentTemplate>
                                            <asp:TextBox ID="TextBox2"  runat="server" CssClass="newtextbox txtheight textbox2"  Visible="false"></asp:TextBox>
                                          </ContentTemplate></asp:UpdatePanel>
                                        
                                </td>

                            </tr>
                            
                            <%-- End By Saranyadevi 24.2.2018--%>
                        </table>
                    <br />
                                    
                               
                                  <center>
                                <FarPoint:FpSpread ID="Fpspread1" runat="server" Visible="false" BorderWidth="5px"
                                    BorderStyle="Groove" BorderColor="#0CA6CA" ActiveSheetViewIndex="0" Style="margin-left: -5px" OnCellClick="FpSpread1_CellClick" OnPreRender="FpSpread1_SelectedIndexChanged"
                                   ><%-- OnCellClick="FpSpread1_CellClick" OnPreRender="FpSpread1_SelectedIndexChanged"--%>
                                    <Sheets>
                                        <FarPoint:SheetView SheetName="Sheet1">
                                        </FarPoint:SheetView>
                                    </Sheets>
                                </FarPoint:FpSpread>
                          
                       
                    </center>
                    <center>
                      <div id="rptprint1" runat="server" visible="false">
                        <br />
                        <asp:Label ID="lbl_norec1" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                            Font-Size="Medium" ForeColor="Red" Text="Please Enter Your Report Name" Visible="false"></asp:Label>
                        <asp:Label ID="lblrptname1" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                            Font-Size="Medium" Text="Report Name"></asp:Label>
                        <%--  --%>
                        <asp:TextBox ID="txtexcelname1" runat="server" CssClass="textbox textbox1" Height="20px"
                            Width="180px" onkeypress="display1()" Style="font-family: 'Book Antiqua'" Font-Bold="True"
                            Font-Names="Book Antiqua" Font-Size="Medium"></asp:TextBox>
                        <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender9" runat="server" TargetControlID="txtexcelname1"
                            FilterType="LowercaseLetters,UppercaseLetters,Numbers,Custom" ValidChars="(),.[]_"
                            InvalidChars="/\">
                        </asp:FilteredTextBoxExtender>
                        <asp:Button ID="btnExcel1" runat="server" Font-Bold="true" Font-Names="Book Antiqua"
                            OnClick="btnExcel1_Click" Font-Size="Medium" Text="Export To Excel" Width="127px"
                            Height="31px" CssClass="textbox textbox1" />
                        <asp:Button ID="btnprintmaster1" runat="server" Text="Print" OnClick="btnprintmaster1_Click"
                            Font-Names="Book Antiqua" Font-Size="Medium" Font-Bold="true" Width="60px" Height="31px"
                            CssClass="textbox textbox1" />
                        
                        <Insproplus:printmaster runat="server" ID="Printcontrol1" Visible="false" />
                    </div>
</center>

                    <br />
                     <center>
                           <asp:Panel ID="pheaderfilter" runat="server" CssClass="maintablestyle" Height="22px"
                                Width="970px" Style="margin-top: -0.1%;">
                                <%--&nbsp;Filter your Search here&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;--%>
                                <asp:Label ID="Labelfilter" Text="Column Order" runat="server" Font-Size="Medium"
                                    Font-Bold="True" Font-Names="Book Antiqua" Style="margin-left: 0%;" />
                                <asp:Image ID="Imagefilter" runat="server" CssClass="cpimage" ImageUrl="right.jpeg"
                                    ImageAlign="Right" />
                            </asp:Panel>
                        </center>
                 <br />
                    <center>
                      <asp:CollapsiblePanelExtender ID="cpecolumnorder" runat="server" TargetControlID="pcolumnorder"
                        CollapseControlID="pheaderfilter" ExpandControlID="pheaderfilter" Collapsed="true"
                        TextLabelID="Labelfilter" CollapsedSize="0" ImageControlID="Imagefilter" CollapsedImage="right.jpeg"
                        ExpandedImage="down.jpeg">
                    </asp:CollapsiblePanelExtender>
                        <asp:Panel ID="pcolumnorder" runat="server" CssClass="maintablestyle" Width="970px">
                            <%--style="margin-left:74px;"                     --%>
                            <table>
                                <tr>
                                    <td>
                                        <asp:CheckBox ID="CheckBox_column" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                            Font-Size="Medium" Text="Select All" AutoPostBack="true" OnCheckedChanged="CheckBox_column_CheckedChanged" />
                                    </td>
                                    <td>
                                        <asp:LinkButton ID="lnk_columnorder" runat="server" Font-Size="X-Small" Height="16px"
                                            Style="font-family: 'Book Antiqua'; font-weight: 700; font-size: small; margin-left: -512px;"
                                            Visible="false" Width="111px" OnClick="LinkButtonsremove_Click">Remove  All</asp:LinkButton>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        &nbsp;
                                        <asp:TextBox ID="tborder" Visible="false" Width="907px" TextMode="MultiLine" CssClass="style1"
                                            AutoPostBack="true" runat="server" Enabled="false">
                                        </asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:CheckBoxList ID="cblcolumnorder" runat="server" Height="43px" AutoPostBack="true"
                                            Width="932px" Style="font-family: 'Book Antiqua'; font-weight: 700; font-size: medium;"
                                            RepeatColumns="5" RepeatDirection="Horizontal" OnSelectedIndexChanged="cblcolumnorder_SelectedIndexChanged">
                                            <asp:ListItem Selected="True" Value="Roll_No">Roll No</asp:ListItem>
                                             <asp:ListItem Selected="True" Value="reg_no">Reg No</asp:ListItem>
                                            <asp:ListItem Selected="True" Value="Stud_Name">Student Name</asp:ListItem>
                                          
                                            <asp:ListItem Value="Batch_Year">Batch Year</asp:ListItem>
                                            <asp:ListItem Value="Course_Name">Degree</asp:ListItem>
                                               <asp:ListItem Value="Dept_Name">Department</asp:ListItem>
                                          
                                           
                                            
                                        </asp:CheckBoxList>
                                    </td>
                                </tr>
                            </table>
                        </asp:Panel>
                        
                    </center>
                  
                    </center>
                    <br/>
                     <center>

                                <FarPoint:FpSpread ID="Fpspread2" runat="server" Visible="false" BorderWidth="5px"
                                    BorderStyle="Groove" BorderColor="#0CA6CA" ActiveSheetViewIndex="0" Style="margin-left: -5px" 
                                   ><%-- OnCellClick="FpSpread1_CellClick" OnPreRender="FpSpread1_SelectedIndexChanged"--%>
                                    <Sheets>
                                        <FarPoint:SheetView SheetName="Sheet1">
                                        </FarPoint:SheetView>
                                    </Sheets>
                                </FarPoint:FpSpread>
                          
                       
                    </center>

                          <center>
                      <div id="Div1" runat="server" visible="false">
                        <br />
                        <asp:Label ID="Label3" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                            Font-Size="Medium" ForeColor="Red" Text="Please Enter Your Report Name" Visible="false"></asp:Label>
                        <asp:Label ID="Label4" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                            Font-Size="Medium" Text="Report Name"></asp:Label>
                        <%--  --%>
                        <asp:TextBox ID="TextBox3" runat="server" CssClass="textbox textbox1" Height="20px"
                            Width="180px" onkeypress="display1()" Style="font-family: 'Book Antiqua'" Font-Bold="True"
                            Font-Names="Book Antiqua" Font-Size="Medium"></asp:TextBox>
                        <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" runat="server" TargetControlID="txtexcelname1"
                            FilterType="LowercaseLetters,UppercaseLetters,Numbers,Custom" ValidChars="(),.[]_"
                            InvalidChars="/\">
                        </asp:FilteredTextBoxExtender>
                        <asp:Button ID="Button1" runat="server" Font-Bold="true" Font-Names="Book Antiqua"
                            OnClick="btnExcel2_Click" Font-Size="Medium" Text="Export To Excel" Width="127px"
                            Height="31px" CssClass="textbox textbox1" />
                        <asp:Button ID="Button2" runat="server" Text="Print" OnClick="btnprintmaster2_Click"
                            Font-Names="Book Antiqua" Font-Size="Medium" Font-Bold="true" Width="60px" Height="31px"
                            CssClass="textbox textbox1" />
                        
                        <Insproplus:printmaster runat="server" ID="Printmaster1" Visible="false" />
                    </div>
</center>
                </center>
                <br />
                <center>
                   </div>
                    <br />
                   
                </center>
                </form>
                </body>
                </html>
</asp:Content>


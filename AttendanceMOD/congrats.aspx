<%@ Page Title="" Language="C#" MasterPageFile="~/AttendanceMOD/AttendanceSubSiteMaster.master"
    AutoEventWireup="true" CodeFile="congrats.aspx.cs" Inherits="congrats" EnableEventValidation="false" %>
    <%@ Register Src="~/Usercontrols/NewPrintMaster.ascx" TagName="NEWPrintMater" TagPrefix="NEW" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Src="~/Usercontrols/PrintMaster.ascx" TagName="printmaster" TagPrefix="Insproplus" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">
    <!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
    <html xmlns="http://www.w3.org/1999/xhtml">
    <title></title>
    <link href="~/Styles/css/Registration.css" rel="stylesheet" type="text/css" />
    <script src="//ajax.googleapis.com/ajax/libs/jquery/1.11.1/jquery.min.js"></script>
    <link href="~/Styles/css/Commoncss.css" rel="Stylesheet" type="text/css" />
    <body>
        <style type="text/css">
            .style1
            {
                width: 1000px;
            }
            .style3
            {
                width: 90px;
            }
            .txt
            {
            }            
            #gview
            {
                padding: 0;
                margin: 0;
                border: 1px solid #333;
                font-family: Arial;
                
            }
        </style>
        <asp:ScriptManager ID="ScriptManager1" runat="server">
        </asp:ScriptManager>
        <div>
            <br />
            <center>
                <span class="fontstyleheader" style="color: Green;">AT10-Congratulation Report</span>
            </center>
        </div>
        <div>
            <script type="text/javascript">
                function display() {
                    document.getElementById('MainContent_lblnorec').innerHTML = "";
                }
            </script>
            <br />
            <div class="maintablestyle" runat="server" style="width: 1000px;">
                <table class="style1">
                    <tr>
                        <td style="width: 111px;">
                            <asp:Label runat="server" ID="lblbatch" Text="Batch" Font-Bold="True" Font-Names="Book Antiqua"
                                Font-Size="Medium"></asp:Label>
                            <asp:DropDownList ID="ddlbatch" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                Font-Size="Medium" OnSelectedIndexChanged="ddlbatch_SelectedIndexChanged" Height="25px"
                                Width="61px" AutoPostBack="True">
                            </asp:DropDownList>
                        </td>
                        <td style="width: 194px;">
                            <asp:Label runat="server" ID="lbldegree" Text="Degree" Font-Bold="True" Font-Names="Book Antiqua"
                                Font-Size="Medium"></asp:Label>
                            <asp:DropDownList runat="server" ID="ddldegree" Height="25px" Width="90px" AutoPostBack="True"
                                OnSelectedIndexChanged="ddldegree_SelectedIndexChanged" Font-Bold="True" Font-Names="Book Antiqua"
                                Font-Size="Medium">
                            </asp:DropDownList>
                        </td>
                        <td style="width: 338px;">
                            <asp:Label runat="server" ID="lblbranch" Text="Branch" Font-Bold="True" Font-Names="Book Antiqua"
                                Font-Size="Medium"></asp:Label>
                            <asp:DropDownList runat="server" ID="ddlbranch" Font-Bold="True" Height="25px" Width="260px"
                                Font-Names="Book Antiqua" Font-Size="Medium" OnSelectedIndexChanged="ddlbranch_SelectedIndexChanged"
                                AutoPostBack="True">
                            </asp:DropDownList>
                        </td>
                        <td class="style3">
                            <asp:Label runat="server" ID="lblduration" Text="Sem" Font-Bold="True" Font-Names="Book Antiqua"
                                Font-Size="Medium"></asp:Label>
                            <asp:DropDownList runat="server" ID="ddlduration" Font-Bold="True" Font-Names="Book Antiqua"
                                Font-Size="Medium" AutoPostBack="True" Height="25px" Width="40px" OnSelectedIndexChanged="ddlduration_SelectedIndexChanged">
                            </asp:DropDownList>
                        </td>
                        <td>
                            <asp:Label runat="server" ID="lblsec" Text="Sec" Font-Bold="True" Font-Names="Book Antiqua"
                                Font-Size="Medium"></asp:Label>
                            <asp:DropDownList runat="server" ID="ddlsec" Font-Bold="True" Font-Names="Book Antiqua"
                                Font-Size="Medium" Height="25px" Width="40px" AutoPostBack="True" OnSelectedIndexChanged="ddlsec_SelectedIndexChanged">
                            </asp:DropDownList>
                        </td>
                        <td>
                        </td>
                        <td>
                        </td>
                    </tr>
                </table>
                <table>
                    <tr>
                        <td>
                            <asp:Label ID="lblFromdate" runat="server" Text="From Date" Font-Bold="True" Font-Names="Book Antiqua"
                                Font-Size="Medium"></asp:Label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtFromDate" CssClass="txt" runat="server" Height="20px" Width="75px"
                                OnTextChanged="txtFromDate_TextChanged" Font-Bold="True" Font-Names="Book Antiqua"
                                Font-Size="Medium" AutoPostBack="True"></asp:TextBox>
                            <asp:FilteredTextBoxExtender ID="txtFromDate_FilteredTextBoxExtender" FilterType="Custom,Numbers"
                                ValidChars="/" runat="server" TargetControlID="txtFromDate">
                            </asp:FilteredTextBoxExtender>
                            <asp:CalendarExtender ID="calfromdate" TargetControlID="txtFromDate" Format="d/MM/yyyy"
                                runat="server">
                            </asp:CalendarExtender>
                        </td>
                        <td>
                            <asp:Label ID="lbltodate" runat="server" Text="To Date" Font-Bold="True" Font-Names="Book Antiqua"
                                Font-Size="Medium"></asp:Label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtToDate" CssClass="txt" runat="server" Height="20px" Width="75px"
                                Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium" OnTextChanged="txtToDate_TextChanged"
                                AutoPostBack="True"></asp:TextBox>
                            <asp:FilteredTextBoxExtender ID="txtToDate_FilteredTextBoxExtender" runat="server"
                                TargetControlID="txtToDate" FilterType="Custom,Numbers" ValidChars="/">
                            </asp:FilteredTextBoxExtender>
                            <asp:CalendarExtender ID="caltodate" TargetControlID="txtToDate" Format="d/MM/yyyy"
                                runat="server">
                            </asp:CalendarExtender>
                        </td>
                        <td>
                            &nbsp;<asp:Button ID="btnGo" runat="server" Text="Go" Style="font-weight: 700" OnClick="btnGo_Click"
                                Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium" Height="27px" Width="36px" />
                        </td>
                        <td>
                            <asp:Button ID="btnPrint" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                Font-Size="Medium" OnClick="btnPrint_Click" Text="Print Master Setting" Visible="False"
                                Width="160px" />
                            <asp:Label ID="lblpages" runat="server" Font-Bold="true" Font-Names="Book Antiqua"
                                Font-Size="Medium" Text="Page"></asp:Label>
                            <asp:DropDownList ID="ddlpage" runat="server" AutoPostBack="True" Font-Bold="true"
                                Font-Names="Book Antiqua" Font-Size="Medium" Height="21px" OnSelectedIndexChanged="ddlpage_SelectedIndexChanged"
                                Width="47px">
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2">
                            <asp:Label ID="frmlbl" runat="server" Text="Select From Date" ForeColor="Red" Font-Bold="True"
                                Font-Names="Book Antiqua"></asp:Label>
                        </td>
                        <td colspan="2">
                            <asp:Label ID="tolbl" runat="server" Text="Select To Date" ForeColor="Red" Font-Bold="True"
                                Font-Names="Book Antiqua"></asp:Label>
                        </td>
                        <td colspan="2">
                            <asp:Label ID="tofromlbl" runat="server" Text="From date should not be greater than To date"
                                ForeColor="Red" Font-Bold="True" Font-Names="Book Antiqua"></asp:Label>
                        </td>
                    </tr>
                </table>
            </div>
            <br />
            <center>
                <asp:Label ID="lblnorec" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                    Font-Size="Medium" ForeColor="#FF3300" Text="No Record(s) Found" Visible="False"></asp:Label>
            </center>
            <br />
            <asp:Panel ID="pnl_pageset" runat="server">
                <asp:Label ID="Buttontotal" runat="server" Font-Bold="True" Font-Size="Medium" Font-Names="Book Antiqua"></asp:Label>&nbsp;&nbsp;
                <asp:Label ID="lblrecord" runat="server" Visible="false" Font-Bold="True" Text="Records Per Page"
                    Font-Names="Book Antiqua" Font-Size="Medium"></asp:Label>&nbsp;&nbsp;
                <asp:DropDownList ID="DropDownListpage" runat="server" AutoPostBack="True" OnSelectedIndexChanged="DropDownListpage_SelectedIndexChanged"
                    Font-Bold="True" Visible="False" Font-Names="Book Antiqua" Font-Size="Medium"
                    Height="24px" Width="58px">
                </asp:DropDownList>
                &nbsp;&nbsp;
                <asp:TextBox ID="TextBoxother" Visible="false" runat="server" Height="16px" Width="34px"
                    AutoPostBack="True" OnTextChanged="TextBoxother_TextChanged" Font-Bold="True"
                    Font-Names="Book Antiqua" Font-Size="Medium"></asp:TextBox>
                <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender4" runat="server" TargetControlID="TextBoxother"
                    FilterType="Numbers" />
                &nbsp;&nbsp;
                <asp:Label ID="lblpage_search" runat="server" Font-Bold="True" Text="Page Search"
                    Width="96px" Font-Names="Book Antiqua" Font-Size="Medium"></asp:Label>&nbsp;&nbsp;
                <asp:TextBox ID="TextBoxpage" runat="server" AutoPostBack="True" OnTextChanged="TextBoxpage_TextChanged"
                    Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium" Height="16px" Width="34px"></asp:TextBox>
                <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender2" runat="server" TargetControlID="TextBoxpage"
                    FilterType="Numbers" />
                &nbsp;&nbsp;
                <asp:Label ID="LabelE" runat="server" Visible="False" ForeColor="Red" Font-Bold="True"
                    Font-Names="Book Antiqua" Font-Size="Medium"></asp:Label>
            </asp:Panel>
            <center>
            
             <asp:GridView ID="gview" runat="server" BorderStyle="Double" AutoGenerateColumns="true"
            Font-Names="Book Antiqua" Font-Size="Medium" GridLines="Both" ShowFooter="false" ShowHeader="false">
            <Columns>
                <%--<asp:TemplateField HeaderText="S.No">
                    <ItemTemplate>
                    <center>
                        <asp:Label ID="lblSno" runat="server" Text='<%# Eval("sno") %>'></asp:Label>
                    </center>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Roll No">
                    <ItemTemplate>
                      <asp:Label ID="lblrol" runat="server" Text='<%# Eval("Roll No") %>'  />
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Reg No">
                    <ItemTemplate>
                      <asp:Label ID="lblreg" runat="server" Text='<%# Eval("Reg No") %>'  />
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Admission No">
                    <ItemTemplate>
                        <asp:Label ID="lblroladmt" runat="server" Text='<%# Eval("Admission No") %>' />                  
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Name of the Student">
                    <ItemTemplate>
                        <asp:Label ID="lblstunme" runat="server" Text='<%# Eval("Name of the Student") %>' />                 
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Student Type">
                    <ItemTemplate>
                        <asp:Label ID="lblstutype" runat="server" Text='<%# Eval("Student Type") %>' />                      
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Cond Hrs">
                    <ItemTemplate>
                    <center>
                        <asp:Label ID="lblchrs" runat="server"  Text='<%# Eval("conHr") %>' />                        
                    </center>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Atten Hrs">
                    <ItemTemplate>
                        <center>
                        <asp:Label ID="lblahrs" runat="server" Text='<%# Eval("AttHr") %>' />
                        </center>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Atten %">
                    <ItemTemplate>
                        <center>
                        <asp:Label ID="lblatnper" runat="server" Text='<%# Eval("AttnPer") %>'/>
                        </center>
                    </ItemTemplate>
                </asp:TemplateField>--%>
            </Columns>
            <FooterStyle BackColor="White" ForeColor="#333333" />
            <HeaderStyle BackColor="#0CA6CA" Font-Bold="True" ForeColor="Black" />
            <PagerStyle BackColor="#336666"  HorizontalAlign="Center" />
            <RowStyle  ForeColor="#333333" />
            <SelectedRowStyle BackColor="#339966" Font-Bold="True" />
            </asp:GridView>

               
            </center>
            <br />
            <center>
                <asp:Label ID="lblrptname" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                    Font-Size="Medium" Text="Report Name"></asp:Label>
                <asp:TextBox ID="txtexcelname" runat="server" Height="20px" Width="180px" Style="font-family: 'Book Antiqua'"
                    Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium"></asp:TextBox>
                <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" runat="server" TargetControlID="txtexcelname"
                    FilterType="LowercaseLetters,UppercaseLetters,Numbers,Custom" ValidChars="!@$%^&*()_+|\}{][':;?><,./">
                </asp:FilteredTextBoxExtender>
                <asp:Button ID="btnxl" runat="server" Text="Export Excel" Font-Bold="True" Font-Names="Book Antiqua"
                    Font-Size="Medium" OnClick="btnxl_Click" />
                <asp:Button ID="btnprintmaster" runat="server" Text="Print" Style="position: absolute;
                    left: 720px;" OnClick="btnprintmaster_Click" Font-Names="Book Antiqua" Font-Size="Medium"
                    Font-Bold="true" />
                     <NEW:NEWPrintMater runat="server" ID="NEWPrintMater1" Visible="false" />
                <Insproplus:printmaster runat="server" ID="Printcontrol" Visible="false" />
            </center>
          
        </div>
    </body>
    </html>
</asp:Content>

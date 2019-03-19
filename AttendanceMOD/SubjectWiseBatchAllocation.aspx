<%@ Page Title="" Language="C#" MasterPageFile="~/AttendanceMOD/AttendanceSubSiteMaster.master"
    AutoEventWireup="true" CodeFile="SubjectWiseBatchAllocation.aspx.cs" Inherits="SubjectWiseBatchAllocation" %>

<%@ Register Assembly="FarPoint.Web.Spread" Namespace="FarPoint.Web.Spread" TagPrefix="FarPoint" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">
    <center>
        <asp:Label ID="Label1" CssClass="fontstyleheader" runat="server" Text="Subject Wise Batch Allocation"
            Font-Bold="True" ForeColor="Green"></asp:Label>
    </center>
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <br />
    <center>
        <asp:UpdatePanel ID="Upanel1" runat="server">
            <ContentTemplate>
                <table class="maintablestyle" style="width: 750px; height: auto; padding: 5px; background-color: #0CA6CA;">
                    <tr>
                        <td>
                            <asp:Label ID="lblstraem" runat="server" Text="Stream" Font-Names="Book Antiqua"
                                Font-Size="Medium" Font-Bold="true"></asp:Label>
                        </td>
                        <td>
                            <asp:DropDownList ID="ddlstream" CssClass="cursorptr" runat="server" AutoPostBack="True"
                                Font-Names="Book Antiqua" OnSelectedIndexChanged="ddlstream_SelectedIndexChanged"
                                Font-Size="Medium" Font-Bold="True" Height="25px">
                            </asp:DropDownList>
                        </td>
                        <td>
                            <asp:Label ID="lblbatch" runat="server" Text="Batch" Font-Names="Book Antiqua" Font-Size="Medium"
                                Font-Bold="true"></asp:Label>
                        </td>
                        <td>
                            <asp:DropDownList ID="ddlbatch" CssClass="cursorptr" runat="server" AutoPostBack="True"
                                OnSelectedIndexChanged="ddlbatch_SelectedIndexChanged" Font-Names="Book Antiqua"
                                Font-Size="Medium" Font-Bold="True">
                            </asp:DropDownList>
                        </td>
                        <td>
                            <asp:Label ID="lblcourse" runat="server" Text="Course" Font-Names="Book Antiqua"
                                Font-Size="Medium" Font-Bold="true"></asp:Label>
                        </td>
                        <td>
                            <asp:DropDownList ID="ddlcourse" CssClass="cursorptr" runat="server" AutoPostBack="True"
                                OnSelectedIndexChanged="ddlcourse_SelectedIndexChanged" Font-Names="Book Antiqua"
                                Font-Size="Medium" Font-Bold="True">
                            </asp:DropDownList>
                        </td>
                        <td>
                            <asp:Label ID="lbldegree" runat="server" Text="Degree" Font-Names="Book Antiqua"
                                Font-Size="Medium" Font-Bold="true"></asp:Label>
                        </td>
                        <td>
                            <div style="position: relative;">
                                <asp:UpdatePanel ID="updegree" runat="server">
                                    <ContentTemplate>
                                        <asp:TextBox ID="txtdegree" runat="server" ReadOnly="true" Font-Names="Book Antiqua"
                                            Font-Size="Medium" Font-Bold="true" CssClass="textbox  txtheight2">-- Select --</asp:TextBox>
                                        <asp:Panel ID="pdegree" runat="server" CssClass="multxtpanel" Height="100px" Width="110px">
                                            <asp:CheckBox ID="chkdegree" Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium"
                                                runat="server" Text="Select All" AutoPostBack="True" OnCheckedChanged="chkdegree_CheckedChanged" />
                                            <asp:CheckBoxList ID="cbldegree" runat="server" Font-Size="Medium" Font-Bold="True"
                                                Font-Names="Book Antiqua" AutoPostBack="true" OnSelectedIndexChanged="chklstdegree_SelectedIndexChanged">
                                            </asp:CheckBoxList>
                                        </asp:Panel>
                                        <asp:PopupControlExtender ID="popExtdegree" runat="server" TargetControlID="txtdegree"
                                            PopupControlID="pdegree" Position="Bottom">
                                        </asp:PopupControlExtender>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </div>
                        </td>
                        <td>
                            <asp:Label ID="lblbranch" runat="server" Text="Branch" Font-Names="Book Antiqua"
                                Font-Size="Medium" Font-Bold="true"></asp:Label>
                        </td>
                        <td>
                            <div style="position: relative;">
                                <asp:UpdatePanel ID="upbranch" runat="server">
                                    <ContentTemplate>
                                        <asp:TextBox ID="txtbranch" runat="server" ReadOnly="true" Font-Names="Book Antiqua"
                                            Font-Size="Medium" Font-Bold="true" CssClass="textbox  txtheight2">-- Select --</asp:TextBox>
                                        <asp:Panel ID="pbranch" runat="server" CssClass="multxtpanel" Height="100px" Width="200px">
                                            <asp:CheckBox ID="chkbranch" Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium"
                                                runat="server" Text="Select All" AutoPostBack="True" OnCheckedChanged="chkbranch_CheckedChanged" />
                                            <asp:CheckBoxList ID="cblbranch" runat="server" Font-Size="Medium" Font-Bold="True"
                                                Font-Names="Book Antiqua" AutoPostBack="true" OnSelectedIndexChanged="chklstbranch_SelectedIndexChanged">
                                            </asp:CheckBoxList>
                                        </asp:Panel>
                                        <asp:PopupControlExtender ID="popExtbranch" runat="server" TargetControlID="txtbranch"
                                            PopupControlID="pbranch" Position="Bottom">
                                        </asp:PopupControlExtender>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </div>
                        </td>
                        <td>
                            <asp:Label ID="lblsem" runat="server" Text="Sem" Font-Names="Book Antiqua" Font-Size="Medium"
                                Font-Bold="true"></asp:Label>
                        </td>
                        <td>
                            <asp:DropDownList ID="ddlsem" CssClass="cursorptr" runat="server" OnSelectedIndexChanged="ddlsem_SelectedIndexChanged"
                                AutoPostBack="True" Font-Names="Book Antiqua" Font-Size="Medium" Font-Bold="True">
                            </asp:DropDownList>
                        </td>
                        <td>
                            <asp:Label ID="lblsec" runat="server" Text="Sec" Font-Names="Book Antiqua" Font-Size="Medium"
                                Font-Bold="true"></asp:Label>
                        </td>
                        <td>
                            <div style="position: relative;">
                                <asp:UpdatePanel ID="upsec" runat="server">
                                    <ContentTemplate>
                                        <asp:TextBox ID="txtsec" runat="server" ReadOnly="true" Font-Names="Book Antiqua"
                                            Font-Size="Medium" Font-Bold="true" CssClass="textbox  txtheight2">-- Select --</asp:TextBox>
                                        <asp:Panel ID="psec" runat="server" CssClass="multxtpanel" Height="100px" Width="110px">
                                            <asp:CheckBox ID="chksec" Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium"
                                                runat="server" Text="Select All" AutoPostBack="True" OnCheckedChanged="chksec_CheckedChanged" />
                                            <asp:CheckBoxList ID="cblsec" runat="server" Font-Size="Medium" Font-Bold="True"
                                                Font-Names="Book Antiqua" AutoPostBack="true" OnSelectedIndexChanged="chklstsec_SelectedIndexChanged">
                                            </asp:CheckBoxList>
                                        </asp:Panel>
                                        <asp:PopupControlExtender ID="popExtsec" runat="server" TargetControlID="txtsec"
                                            PopupControlID="psec" Position="Bottom">
                                        </asp:PopupControlExtender>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </div>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label ID="lblsubtype" runat="server" Text="Subject Type" Font-Names="Book Antiqua"
                                Font-Size="Medium" Font-Bold="true"></asp:Label>
                        </td>
                        <td>
                            <asp:DropDownList ID="ddlsubtype" CssClass="cursorptr" runat="server" OnSelectedIndexChanged="ddlsubtype_SelectedIndexChanged"
                                AutoPostBack="True" Font-Names="Book Antiqua" Font-Size="Medium" Font-Bold="True"
                                Width="95px">
                            </asp:DropDownList>
                        </td>
                        <td colspan="10">
                            <table>
                                <tr>
                                    <td>
                                        <asp:Label ID="lblsubjcet" runat="server" Text="Subject" Font-Names="Book Antiqua"
                                            Font-Size="Medium" Font-Bold="true"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="ddlsubject" CssClass="cursorptr" runat="server" OnSelectedIndexChanged="ddlsubject_SelectedIndexChanged"
                                            AutoPostBack="True" Font-Names="Book Antiqua" Font-Size="Medium" Font-Bold="True"
                                            Width="100px">
                                        </asp:DropDownList>
                                    </td>
                                    <td>
                                        <asp:UpdatePanel ID="UPGo" runat="server">
                                            <ContentTemplate>
                                                <asp:Button ID="Btngo" runat="server" OnClick="Btngo_Click" Font-Names="Book Antiqua"
                                                    Font-Size="Medium" Font-Bold="True" Text="Go" />
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                    </td>
                                    <td>
                                        <asp:Label ID="lblnoofbatch" runat="server" Text="No Of Batch" Font-Names="Book Antiqua"
                                            Font-Size="Medium" Font-Bold="true"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtnoofbatch" runat="server" Font-Bold="true" Font-Names="Book Antiqua"
                                            Font-Size="Medium" Width="50px" MaxLength="3" AutoPostBack="true" OnTextChanged="txtnoofbatch_TextChanged"></asp:TextBox>
                                        <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender3" runat="server" TargetControlID="txtnoofbatch"
                                            FilterType="Numbers" />
                                    </td>
                                    <td>
                                        <asp:Label ID="lblstubatch" runat="server" Text="Student Batch" Font-Names="Book Antiqua"
                                            Font-Size="Medium" Font-Bold="true"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="ddlnobatch" CssClass="cursorptr" runat="server" AutoPostBack="True"
                                            Font-Names="Book Antiqua" OnSelectedIndexChanged="ddlnobatch_SelectedIndexChanged"
                                            Font-Size="Medium" Font-Bold="True">
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                </table>
            </ContentTemplate>
              <Triggers>
                        <asp:PostBackTrigger ControlID="Btngo" />
                    </Triggers>
        </asp:UpdatePanel>
    </center>
    <br />
    <asp:UpdatePanel ID="upd" runat="server">
        <ContentTemplate>
            <center>
                <asp:Label ID="errmsg" runat="server" ForeColor="Red" Font-Bold="true" Font-Names="Book Antiqua"
                    Font-Size="Medium"></asp:Label>
                <br />
                <asp:GridView ID="gview" runat="server" AutoGenerateColumns="false" OnRowDataBound="gview_OnRowDataBound">
                    <Columns>
                        <asp:TemplateField HeaderText="S.No">
                            <ItemTemplate>
                                <center>
                                    <asp:Label ID="lblsno" runat="server" Text='<%#Eval("SNo") %>' />
                                </center>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Staff Code">
                            <ItemTemplate>
                                <asp:Label ID="lblstaffcode" runat="server" Text='<%#Eval("Staff_Code") %>' />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Staff Name">
                            <ItemTemplate>
                                <asp:Label ID="lblstaffnme" runat="server" Text='<%#Eval("Staff_Name") %>' />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Batch" HeaderStyle-Width="50">
                            <ItemTemplate>
                                <asp:DropDownList ID="lblddlbatch" runat="server">
                                </asp:DropDownList>
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                    <HeaderStyle BackColor="#0CA6CA" Font-Bold="true" ForeColor="Black" Font-Size="Medium" />
                    <FooterStyle BackColor="White" ForeColor="#333333" />
                    <PagerStyle BackColor="#336666" HorizontalAlign="Center" />
                    <RowStyle ForeColor="#333333" />
                    <SelectedRowStyle BackColor="#339966" Font-Bold="True" />
                </asp:GridView>
                <asp:Button ID="btnsatff" runat="server" Text="Save" OnClick="btnsatff_Click" Font-Bold="true"
                    Font-Names="Book Antiqua" Font-Size="Medium" />
                <center>
                    <div id="divMainContents" runat="server" style="display: table; margin: 0px; height: auto;
                        margin-bottom: 20px; margin-top: 10px; position: relative; width: auto; text-align: left;">
                        <asp:GridView ID="gview1" Style="height: auto;" runat="server" Visible="false" HeaderStyle-ForeColor="Black"
                            HeaderStyle-BackColor="#0CA6CA" AutoGenerateColumns="false">
                            <Columns>
                                <asp:TemplateField HeaderText="S.No" HeaderStyle-BackColor="#0CA6CA" HeaderStyle-HorizontalAlign="center"
                                    HeaderStyle-Width="30px">
                                    <ItemTemplate>
                                        <center>
                                            <asp:Label ID="lbl_sno" runat="server" Style="width: auto;" Text='<%#Eval("Sno") %>'></asp:Label>
                                        </center>
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Center" />
                                    <ItemStyle HorizontalAlign="left" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Degree Details" HeaderStyle-BackColor="#0CA6CA" HeaderStyle-HorizontalAlign="center"
                                    HeaderStyle-Width="100px">
                                    <ItemTemplate>
                                        <asp:Label ID="lbl_degreedetails" runat="server" Style="width: auto; text-align: right;"
                                            Text='<%#Eval("degreedetails") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Roll No" HeaderStyle-BackColor="#0CA6CA" HeaderStyle-HorizontalAlign="center"
                                    HeaderStyle-Width="100px">
                                    <ItemTemplate>
                                        <asp:Label ID="lbl_Roll" runat="server" Style="width: auto;" Text='<%#Eval("roll") %>'></asp:Label>
                                        <asp:Label ID="lbl_tagroll" runat="server" Visible="false" Style="width: auto;" Text='<%#Eval("tagroll") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Reg No" HeaderStyle-BackColor="#0CA6CA" HeaderStyle-HorizontalAlign="center"
                                    HeaderStyle-Width="100px">
                                    <ItemTemplate>
                                        <asp:Label ID="lbl_Reg" runat="server" Style="width: auto;" Text='<%#Eval("Reg") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Student Type" HeaderStyle-BackColor="#0CA6CA" HeaderStyle-HorizontalAlign="center"
                                    HeaderStyle-Width="100px">
                                    <ItemTemplate>
                                        <asp:Label ID="lbl_stdtyp" runat="server" Style="width: auto;" Text='<%#Eval("stdtype") %>'></asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Center" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Student Name" HeaderStyle-BackColor="#0CA6CA" HeaderStyle-HorizontalAlign="center"
                                    HeaderStyle-Width="100px">
                                    <ItemTemplate>
                                        <asp:Label ID="lbl_stdname" runat="server" Style="width: auto;" Text='<%#Eval("stdname") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Select">
                                    <ItemTemplate>
                                        <asp:CheckBox ID="selectchk" runat="server" />
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Center" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Batch" HeaderStyle-BackColor="#0CA6CA" HeaderStyle-HorizontalAlign="center"
                                    HeaderStyle-Width="100px">
                                    <ItemTemplate>
                                        <center>
                                            <asp:Label ID="lbl_batch" runat="server" Style="width: auto;" Text='<%#Eval("Batch") %>'></asp:Label>
                                        </center>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                        </asp:GridView>
                    </div>
                </center>
        </ContentTemplate>
    </asp:UpdatePanel>
    <br />
    <center>
        <asp:UpdatePanel ID="Upanel2" runat="server">
            <ContentTemplate>
                <table>
                    <tr>
                        <td>
                            <asp:CheckBox ID="CheckBox1" runat="server" AutoPostBack="true" OnCheckedChanged="CheckBox1_CheckedChanged"
                                Font-Bold="true" Font-Names="Book Antiqua" Text="Select" Font-Size="Medium" />
                        </td>
                        <td>
                            <asp:Label ID="lblfrom" runat="server" Text="From" Font-Bold="true" Font-Names="Book Antiqua"
                                Font-Size="Medium"></asp:Label>
                        </td>
                        <td>
                            <asp:TextBox ID="fromno" runat="server" Font-Bold="true" Font-Names="Book Antiqua"
                                Font-Size="Medium"></asp:TextBox>
                        </td>
                        <td>
                            <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender2" runat="server" TargetControlID="fromno"
                                FilterType="Numbers" />
                            <asp:Label ID="lblto" runat="server" Text="To" Font-Bold="true" Font-Names="Book Antiqua"
                                Font-Size="Medium"></asp:Label>
                        </td>
                        <td>
                            <asp:TextBox ID="tono" runat="server" Font-Bold="true" Font-Names="Book Antiqua"
                                Font-Size="Medium"></asp:TextBox>
                            <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" runat="server" TargetControlID="tono"
                                FilterType="Numbers" />
                        </td>
                        <td>
                            <asp:Button ID="Button2" runat="server" Font-Bold="true" Font-Names="Book Antiqua"
                                Font-Size="Medium" Text="Go" OnClick="selectgo_Click" />
                        </td>
                        <td>
                            <asp:UpdatePanel ID="save2" runat="server">
                                <ContentTemplate>
                                    <asp:Button ID="Btnsave" runat="server" Text="Save" OnClick="Btnsave_Click" Font-Bold="true"
                                        Font-Names="Book Antiqua" Font-Size="Medium" />
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </td>
                        <td>
                            <asp:UpdatePanel ID="delete" runat="server">
                                <ContentTemplate>
                                    <asp:Button ID="Btndelete" runat="server" Text="Delete" OnClick="Btndelete_Click"
                                        Font-Bold="true" Font-Names="Book Antiqua" Font-Size="Medium" />
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </td>
                    </tr>
                    <tr>
                    </tr>
                </table>
            </ContentTemplate>
        </asp:UpdatePanel>
    </center>
    <center>
        <asp:UpdateProgress ID="UpdateProgress1" runat="server" AssociatedUpdatePanelID="UPGo">
            <ProgressTemplate>
                <center>
                    <div style="height: 40px; width: 150px;">
                        <img src="../gv images/cloud_loading_256.gif" style="height: 150px;" />
                        <br />
                        <span style="font-family: Book Antiqua; font-size: medium; font-weight: bold; color: Black;">
                            Processing Please Wait...</span>
                    </div>
                </center>
            </ProgressTemplate>
        </asp:UpdateProgress>
        <asp:ModalPopupExtender ID="ModalPopupExtender1" runat="server" TargetControlID="UpdateProgress1"
            PopupControlID="UpdateProgress1">
        </asp:ModalPopupExtender>
        <asp:UpdateProgress ID="UpdateProgress2" runat="server" AssociatedUpdatePanelID="save2">
            <ProgressTemplate>
                <center>
                    <div style="height: 40px; width: 150px;">
                        <img src="../gv images/cloud_loading_256.gif" style="height: 150px;" />
                        <br />
                        <span style="font-family: Book Antiqua; font-size: medium; font-weight: bold; color: Black;">
                            Processing Please Wait...</span>
                    </div>
                </center>
            </ProgressTemplate>
        </asp:UpdateProgress>
        <asp:ModalPopupExtender ID="ModalPopupExtender2" runat="server" TargetControlID="UpdateProgress2"
            PopupControlID="UpdateProgress2">
        </asp:ModalPopupExtender>
        <asp:UpdateProgress ID="UpdateProgress3" runat="server" AssociatedUpdatePanelID="delete">
            <ProgressTemplate>
                <center>
                    <div style="height: 40px; width: 150px;">
                        <img src="../gv images/cloud_loading_256.gif" style="height: 150px;" />
                        <br />
                        <span style="font-family: Book Antiqua; font-size: medium; font-weight: bold; color: Black;">
                            Processing Please Wait...</span>
                    </div>
                </center>
            </ProgressTemplate>
        </asp:UpdateProgress>
        <asp:ModalPopupExtender ID="ModalPopupExtender3" runat="server" TargetControlID="UpdateProgress3"
            PopupControlID="UpdateProgress3">
        </asp:ModalPopupExtender>
    </center>
</asp:Content>

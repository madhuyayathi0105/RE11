<%@ Page Title="" Language="C#" MasterPageFile="~/AdmissionMod/AttendanceSubSiteMaster.master"
    AutoEventWireup="true" CodeFile="Admission_chart.aspx.cs" Inherits="AdmissionMod_Admission_chart" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">
    <center>
        <center>
            <span class="fontstyleheader" style="color: Green;">Admission Chart</span>
        </center>
        <asp:ScriptManager ID="scptMgrNew" runat="server">
        </asp:ScriptManager>
        <br />
        <div class="maindivstyle" style="width: 1000px; height: 498px;">
            <table class="maintablestyle">
                <tr>
                    <td>
                        <asp:Label ID="lblCollege" runat="server" Text="Institute"></asp:Label>
                    </td>
                    <td>
                        <asp:DropDownList ID="ddlCollege" runat="server" CssClass="textbox1 ddlheight6">
                        </asp:DropDownList>
                    </td>
                    <td>
                        <asp:Label ID="lblBatch" runat="server" Text="Batch"></asp:Label>
                    </td>
                    <td>
                        <asp:DropDownList ID="ddlbatch" runat="server" CssClass="textbox1 ddlheight" Width="90px"
                            AutoPostBack="true" OnSelectedIndexChanged="ddlbatch_selectedindexchanged">
                        </asp:DropDownList>
                    </td>
                    <td>
                        <asp:Label ID="lblEdulevel" runat="server" Width="85px" Text="Edu Level"></asp:Label>
                    </td>
                    <td>
                        <asp:DropDownList ID="ddlEduLev" runat="server" CssClass="textbox1 ddlheight" AutoPostBack="true"
                            OnSelectedIndexChanged="ddlEduLev_selectedindexchanged">
                        </asp:DropDownList>
                    </td>
                    <td>
                        <asp:Label ID="lblCourse" runat="server" Text="Course"></asp:Label>
                    </td>
                    <td>
                        <asp:DropDownList ID="ddlcourse" runat="server" CssClass="textbox1 ddlheight3" AutoPostBack="true"
                            OnSelectedIndexChanged="ddlcourse_selectedindexchanged">
                        </asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <td>
                        Stream
                    </td>
                    <td>
                        <asp:DropDownList ID="ddl_stream" runat="server" CssClass="textbox1 ddlheight3">
                        </asp:DropDownList>
                        Session
                        <asp:DropDownList ID="ddl_session" runat="server" CssClass="textbox1 ddlheight3"
                            Width="154px">
                        </asp:DropDownList>
                    </td>
                    <td>
                        <asp:Label ID="Label1" Width="77px" runat="server" Text="From Date"></asp:Label>
                    </td>
                    <td>
                        <asp:TextBox ID="txt_fromdate" runat="server" CssClass="textbox textbox1 txtheight"></asp:TextBox>
                        <asp:CalendarExtender ID="CalendarExtender1" TargetControlID="txt_fromdate" runat="server"
                            Format="dd/MM/yyyy">
                        </asp:CalendarExtender>
                    </td>
                    <td>
                        To Date
                    </td>
                    <td>
                        <asp:TextBox ID="txt_todate" runat="server" CssClass="textbox textbox1 txtheight"></asp:TextBox>
                        <asp:CalendarExtender ID="CalendarExtender2" TargetControlID="txt_todate" runat="server"
                            Format="dd/MM/yyyy">
                        </asp:CalendarExtender>
                    </td>
                    <td>
                        <asp:Button ID="btn_go" runat="server" OnClick="btn_go_Click" Text="Go" CssClass="textbox btn1 " />
                    </td>
                </tr>
            </table>
            <br />
            <div id="chart_div1" runat="server">
                <asp:Chart ID="admission_chart" runat="server" Height="600px" Visible="false" Font-Names="Book Antiqua"
                    Font-Size="Medium">
                    <Series>
                    </Series>
                    <Legends>
                        <asp:Legend Title="Admission Chart" ShadowOffset="3" Docking="Bottom" Font="Book Antiqua">
                        </asp:Legend>
                    </Legends>
                    <Titles>
                        <asp:Title Docking="Top" Text="Admission Chart" Font="Microsoft Sans Serif, 12pt">
                        </asp:Title>
                        <asp:Title Docking="Bottom" Font="Book Antiqua" Text="Quota wise">
                        </asp:Title>
                        <asp:Title Docking="Left" Font="Book Antiqua" Text="Point ">
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
        </div>
        <div id="alert_pop" runat="server" visible="false" style="height: 300em; z-index: 1000;
            width: 100%; background-color: rgba(54, 25, 25, .2); position: absolute; top: 0;
            left: 0px;">
            <center>
                <div id="pnl2" runat="server" class="table" style="background-color: White; height: 120px;
                    width: 238px; border: 5px solid #0CA6CA; border-top: 25px solid #0CA6CA; margin-top: 300px;
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
    </center>
</asp:Content>

<%@ Page Title="" Language="C#" MasterPageFile="~/AttendanceMOD/AttendanceSubSiteMaster.master" AutoEventWireup="true" CodeFile="Grievance Mail.aspx.cs" Inherits="AttendanceMOD_Grievance_Mail" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">
    <center>
    <center>
        <div>
            <span style="color: Green;" class="fontstyleheader">Grievance Mail</span>
            <br />
            <br />
        </div>
    </center>
        <div style="width: 980px;">
           
                    <div id="mailsen" runat="server">
                        <table id="mailsend" runat="server" >
                    <tr>
                                <td>
                                    <asp:Label ID="lblsub" Text="Subject" runat="server" Font-Bold="true" Style="display: inline-block;
                                        color: Black; font-family: Book Antiqua; font-size: medium; font-weight: bold;
                                        width: 90px; text-align: center"></asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtsub" runat="server" Style="display: inline-block; color: Black;
                                        border-color: Black; text-align: left; border-width: thin; font-family: Book Antiqua;
                                        font-size: medium; font-weight: bold; width: 500px;"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="lblbody" Text="Body" runat="server" Font-Bold="true" Style="display: inline-block;
                                        color: Black; font-family: Book Antiqua; font-size: medium; font-weight: bold;
                                        width: 90px; text-align: center;"></asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtbody" TextMode="MultiLine" runat="server" MaxLength="4000" Style="display: inline-block;
                                        border-color: Black; border-width: thin; color: Black; font-family: Book Antiqua;
                                        font-size: medium; font-weight: bold; width: 500px; height: 300px;"></asp:TextBox>
                                </td>
                            </tr>
                          
                    </table>
                    <center>
                       <asp:Button ID="btnsend" OnClick="btnsend_Click" BackColor="#FFD800"  Text="Mail Send" runat="server" Style="font-family: Book Antiqua;
                font-size: medium; font-weight: bold;" />
                    </center></div></div></center>

</asp:Content>


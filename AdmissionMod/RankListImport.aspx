<%@ Page Title="" Language="C#" MasterPageFile="~/AdmissionMod/AttendanceSubSiteMaster.master"
    AutoEventWireup="true" CodeFile="RankListImport.aspx.cs" Inherits="AdmissionMod_RankListImport" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">
    <center>
        <div>
            <span>Rank List Import</span>
        </div>
        <br />
        <div>
            <table>
                <tr>
                    <td>
                        <asp:FileUpload ID="fileUpload" runat="server" />
                    </td>
                    <td>
                        <asp:Button ID="btnUpload" runat="server" Text="Upload" OnClick="btnUpload_Click" />
                    </td>
                </tr>
            </table>
        </div>
    </center>
</asp:Content>

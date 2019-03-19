<%@ Page Language="C#" MasterPageFile="~/Backup/Backup.master" AutoEventWireup="true" CodeFile="DatabaseBackup.aspx.cs"
    Inherits="DatabaseBackup" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <link href="~/Styles/css/Commoncss.css" rel="stylesheet" type="text/css" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">
    <!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
    <html xmlns="http://www.w3.org/1999/xhtml">
    <title></title>
    <body>
        <div>
            <center>
                <div>
                    <span class="fontstyleheader" style="color: Green;">Database Backup</span></div>
            </center>
        </div>
        <div class="maindivstyle" style="width: auto; height: auto;">
            <br />
            <center>
                <div id="tblhdr" runat="server">
                    <table class="maintablestyle" id="maintbl" runat="server">
                        <tr>
                            <td>
                                <asp:Label ID="lbl_location" runat="server" Style="top: 10px; left: 6px;" Text="Location"></asp:Label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtlocation" runat="server" placeholder="Enter Path to Save" CssClass="textbox textbox1 txtheight4"></asp:TextBox>
                            </td>
                            <td>
                                <asp:Button ID="btnbackup" runat="server" Text="BackUp" CssClass="textbox textbox1 btn2"
                                    OnClick="btnbackup_Click" />
                            </td>
                            <td>
                                <asp:FileUpload ID="FileUpload1" Height="25px" runat="server" Visible="false" CssClass="textbox"
                                    ForeColor="White" />
                            </td>
                            <td>
                                <asp:Button ID="btnrestore" runat="server" Text="Restore" Visible="false" CssClass="textbox textbox1 btn2"
                                    OnClick="btnrestore_Click" />
                            </td>                           
                            
                        </tr>
                    </table>
                </div>
                <asp:Label ID="ldlexample" runat="server" Style="top: 10px; left: 6px;" Text="Example: C:\"></asp:Label>
            </center>
            <br />            
            <br />
            <asp:Label ID="lbl_error" runat="server" ForeColor="Red" Visible="false"></asp:Label>
        </div>
    </body>
    </html>
</asp:Content>

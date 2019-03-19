<%@ Page Title="" Language="C#" MasterPageFile="~/FeedBackMOD/FeedbackSubSiteMaster.master" AutoEventWireup="true"
    CodeFile="Feedbackhome.aspx.cs" Inherits="Feedbackhome" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">
    <style type="text/css">
        /* body
        {
            background-image: url('images/money.jpg');
            background-repeat: no-repeat;
            background-attachment: fixed;
        }
        img
        {
            opacity: 0.5;
            filter: alpha(opacity=50); 
        }*/
        .grid-view
        {
            padding: 0;
            margin: 0;
            border: 1px solid #333;
            font-family: "Verdana, Arial, Helvetica, sans-serif, Trebuchet MS";
            font-size: 0.9em;
        }
        
        .grid-view tr.header
        {
            color: white;
            background-color: #0CA6CA;
            height: 30px;
            vertical-align: middle;
            text-align: center;
            font-weight: bold;
            font-size: 20px;
        }
        
        .grid-view tr.normal
        {
            color: black;
            background-color: #FDC64E;
            height: 25px;
            vertical-align: middle;
            text-align: center;
        }
        
        .grid-view tr.alternate
        {
            color: black;
            background-color: #D59200;
            height: 25px;
            vertical-align: middle;
            text-align: center;
        }
        
        .grid-view tr.normal:hover, .grid-view tr.alternate:hover
        {
            background-color: white;
            color: black;
            font-weight: bold;
        }
        
        .grid_view_lnk_button
        {
            color: Black;
            text-decoration: none;
            font-size: large;
        }
        .lbl
        {
            font-family: Book Antiqua;
            font-size: 30px;
            font-weight: bold;
            color: Green;
            text-align: center;
            font-style: italic;
        }
        .hdtxt
        {
            font-family: Book Antiqua;
            font-size: large;
            font-weight: bold;
        }
        .FixedHeader
        {
            position: absolute;
            font-weight: bold;
        }
    </style>
    <center>
        <div>
            <table>
                <tr>
                    <td>
                    </td>
                    <td>
                        <center>
                            <asp:Label ID="Label1" CssClass="lbl" runat="server" Text="FeedBack"></asp:Label>
                        </center>
                    </td>
                    <td>
                    </td>
                </tr>
                <tr>
                    <td>
                    </td>
                    <td>
                        <asp:GridView ID="gridMenu" runat="server" AutoGenerateColumns="False" GridLines="Both"
                            OnRowDataBound="gridMenu_OnRowDataBound" OnDataBound="gridMenu_OnDataBound" CssClass="grid-view"
                            BackColor="White" Width="800px">
                            <Columns>
                                <asp:TemplateField HeaderText="S.No">
                                    <ItemTemplate>
                                        <center>
                                            <asp:Label ID="lblSno" runat="server" CssClass="grid_view_lnk_button" Text='<%#Container.DisplayIndex+1 %>'></asp:Label>
                                        </center>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Header Name">
                                    <ItemTemplate>
                                        <center>
                                            <asp:Label ID="lblHdrName" runat="server" CssClass="grid_view_lnk_button" Text='<%#Eval("HeaderName") %>'></asp:Label>
                                        </center>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="ID">
                                    <ItemTemplate>
                                        <center>
                                            <asp:Label ID="lblReportId" runat="server" CssClass="grid_view_lnk_button" Text='<%#Eval("ReportId") %>'></asp:Label>
                                        </center>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Menu">
                                    <ItemTemplate>
                                        <asp:LinkButton ID="lbPagelink" runat="server" CssClass="grid_view_lnk_button" Font-Underline="false"
                                            Text='<%#Eval("ReportName") %>' PostBackUrl='<%#Eval("PageName") %>'></asp:LinkButton>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Help" Visible="false">
                                    <ItemTemplate>
                                        <center>
                                            <asp:LinkButton ID="lbHelplink" runat="server" CssClass="grid_view_lnk_button" Text="Help"
                                                Font-Underline="false" PostBackUrl='<%#Eval("HelpURL") %>'></asp:LinkButton>
                                        </center>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                        </asp:GridView>
                    </td>
                    <td>
                    </td>
                </tr>
            </table>
        </div>
    </center>
</asp:Content>

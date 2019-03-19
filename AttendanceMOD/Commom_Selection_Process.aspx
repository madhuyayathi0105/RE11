<%@ Page Title="" Language="C#" MasterPageFile="~/studentmod/StudentSubSiteMaster.master" AutoEventWireup="true"
    CodeFile="Commom_Selection_Process.aspx.cs" Inherits="Commom_Selection_Process" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Assembly="FarPoint.Web.Spread" Namespace="FarPoint.Web.Spread" TagPrefix="FarPoint" %>
<%@ Register Src="~/Usercontrols/PrintMaster.ascx" TagName="printmaster" TagPrefix="Insproplus" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <title></title>
    <link href="../Styles/css/Registration.css" rel="stylesheet" type="text/css" />
    <link href="../Styles/css/Commoncss.css" rel="Stylesheet" type="text/css" />
    <style type="text/css">
        .modal_popup_background_color
        {
            background: rgba(0, 0, 0, 0.6);
            filter: alpha(opacity=70);
            opacity: 0.7;
        }
        .search
        {
            background-image: url('images/sat.gif');
            background-repeat: no-repeat;
            width: 40px;
            height: 40px;
            border: 0;
        }
        .sakthi
        {
            width: 140px;
            height: 130px;
            background-color: Black;
        }
        
        .font11
        {
            font-family: Book Antiqua;
            font-size: medium;
            font-weight: bold;
        }
        .modalPopup
        {
            background-color: #ffffdd;
            border-width: 1px;
            -moz-border-radius: 5px;
            border-style: solid;
            border-color: Gray;
            min-width: 300px;
            max-width: 1600px;
            min-height: 10px;
            max-height: 200px;
            top: 100px;
            left: 150px;
            width: 1200px;
        }
        .modalBackground
        {
            background-color: Gray;
            filter: alpha(opacity=80);
            opacity: 0.8;
        }
        ody, input
        {
            font-family: Tahoma;
            font-size: 11px;
        }
        
        .modalBackground
        {
            background-color: Gray;
            filter: alpha(opacity=80);
        }
        
        .accordion
        {
            width: 300px;
        }
        
        .accordionHeader
        {
            border: 1px solid #2F4F4F;
            color: white;
            background-color: #2E4d7B;
            font-family: Arial, Sans-Serif;
            font-size: 12px;
            font-weight: bold;
            padding: 5px;
            margin-top: 5px;
            cursor: pointer;
            height: 15px;
        }
        
        .accordionHeaderSelected
        {
            border: 1px solid #2F4F4F;
            color: white;
            background-color: #5078B3;
            font-family: Arial, Sans-Serif;
            font-size: 12px;
            font-weight: bold;
            padding: 5px;
            margin-top: 5px;
            cursor: pointer;
            height: 15px;
        }
        
        .accordionContent
        {
            background-color: White;
            border: 1px dashed #2F4F4F;
            border-top: none;
            padding: 5px;
            padding-top: 10px;
        }
        
        .autocomplete_highlightedListItem
        {
            background-color: #ffff99;
            color: black;
            padding: 1px;
            width: 241px;
        }
        .autocomplete_completionListElement
        {
            margin: 0px !important;
            background-color: White;
            color: windowtext;
            border: buttonshadow;
            border-width: 0px;
            border-style: solid;
            cursor: 'default';
            height: 100px;
            font-family: Book Antiqua;
            font-size: small;
            text-align: left;
            list-style-type: none;
            padding-left: 1px;
            width: 430px;
            overflow: auto;
            overflow-x: hidden;
        }
    </style>
    <style type="text/css">
        .ajax__myTab
        {
            text-align: center;
        }
        .ajax__myTab .ajax__tab_header
        {
            font-family: Book Antiqua;
            text-align: initial;
            font-size: 16px;
            font-weight: bold;
            color: White;
            border-left: solid 1px #666666;
            border-bottom: thin 1px #666666;
        }
        .ajax__myTab .ajax__tab_outer
        {
            border: 1px solid black;
            width: 220px;
            height: 35px;
            border-top: 3px solid transparent;
        }
        .ajax__myTab .ajax__tab_inner
        {
            padding-left: 4px;
            background-color: indigo;
            width: 275px;
            height: 35px;
        }
        
        .ajax__myTab .ajax__tab_tab
        {
            height: 22px;
            padding: 4px;
            margin: 0;
            text-align: center;
        }
        .ajax__myTab .ajax__tab_hover .ajax__tab_outer
        {
            border-top: 3px solid #00527D;
        }
        .ajax__myTab .ajax__tab_hover .ajax__tab_inner
        {
            background-color: #A1C344;
            color: White;
        }
        .ajax__myTab .ajax__tab_hover .ajax__tab_tab
        {
            background-color: #A1C344;
            cursor: pointer;
            color: White;
        }
        .ajax__myTab .ajax__tab_active .ajax__tab_outer
        {
            border-top: 2px solid white;
            border-bottom: transparent;
            color: #B0E0E6;
        }
        .ajax__myTab .ajax__tab_active .ajax__tab_inner
        {
            background-color: #F36200;
            border-bottom: transparent;
        }
        .ajax__myTab .ajax__tab_active .ajax__tab_tab
        {
            background-color: #F36200;
            cursor: inherit;
            width: 160px;
        }
        .ajax__myTab .ajax__tab_body
        {
            border: 1.5px solid #F36200;
            padding: 6px;
            background-color: #EFEBEF;
        }
        .ajax__myTab .ajax__tab_disabled
        {
            color: #F1F1F1;
        }
        .btnapprove1
        {
            background: transparent;
        }
        .btnapprove1:hover
        {
            background-color: Orange;
            color: White;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">
    <body>
        <script type="text/javascript">
            //Added by Idhris 05-05-2016
            function checktotalamount() {
                var tbl = document.getElementById("<%=gridAdmLedge.ClientID %>");
                var gridViewControls = tbl.getElementsByTagName("input");

                for (var i = 0; i < (gridViewControls.length); i++) {
                    var lblTot = document.getElementById('gridAdmLedge_lblAdmFeeTotal_' + i.toString());
                    var txtAllot = document.getElementById('gridAdmLedge_txtAdmFeeAllot_' + i.toString());
                    var txtDedu = document.getElementById('gridAdmLedge_txtAdmDeduc_' + i.toString());

                    if (txtAllot.value.length == 0) {
                        txtAllot.value = "0";
                    }
                    if (txtDedu.value.length == 0) {
                        txtDedu.value = "0";
                    }
                    var totval = parseFloat(txtAllot.value);
                    var dedval = parseFloat(txtDedu.value);

                    if (dedval > totval) {
                        txtDedu.value = "0";
                        dedval = 0;
                    }

                    lblTot.innerHTML = (totval - dedval).toString();
                }
            }
            function checktotalamount1() {
                var allot = document.getElementById('<%=lblaltamt.ClientID %>');
                var cons = document.getElementById('<%=lblconsamt.ClientID %>');
                var tot = document.getElementById('<%=lbltotamt.ClientID %>');
                var totAllot = 0;
                var totCon = 0;
                var totTot = 0;

                var tbl = document.getElementById("<%=gridFeeDet.ClientID %>");
                var gridViewControls = tbl.getElementsByTagName("input");
                for (var i = 0; i < (gridViewControls.length); i++) {
                    var lblTot = document.getElementById('gridFeeDet_lblAdmFeeTotal_' + i.toString());
                    var txtAllot = document.getElementById('gridFeeDet_txtAdmFeeAllot_' + i.toString());
                    var txtDedu = document.getElementById('gridFeeDet_txtAdmDeduc_' + i.toString());

                    if (txtAllot.value.length == 0) {
                        txtAllot.value = "0";
                    }
                    if (txtDedu.value.length == 0) {
                        txtDedu.value = "0";
                    }
                    var totval = parseFloat(txtAllot.value);
                    var dedval = parseFloat(txtDedu.value);
                    if (dedval > totval) {
                        txtDedu.value = "0";
                        dedval = 0;
                    }
                    lblTot.innerHTML = (totval - dedval).toString();

                    totAllot += totval;
                    totCon += dedval;
                    totTot += (totval - dedval);
                    allot.innerHTML = totAllot.toString();
                    cons.innerHTML = totCon.toString();
                    tot.innerHTML = totTot.toString();
                }


            }

            function InterviewOk(txtid) {
                var txtven = document.getElementById("<%=txtVenue.ClientID %>");
                var txtAmt = document.getElementById("<%=txtddAmount.ClientID %>");
                var ok = false;
                if (txtven.value.trim().length > 0 && txtAmt.value.length > 0) {
                    ok = true;
                } else {
                    alert("Please Fill All Values");
                }
                return ok;
            }
            function PrintDiv() {
                var panel = document.getElementById("<%=contentDiv.ClientID %>");
                var printWindow = window.open('', '', 'height=900,width=1000');
                printWindow.document.write('<html><head>');
                printWindow.document.write('<style> .classRegular { font-family:Arial; font-size:9px; } .classBold10 { font-family:Arial; font-size:11px; font-weight:bold;} .classBold12 { font-family:Arial; font-size:13px; font-weight:bold;} .classBold { font-family:Arial; font-size:9px; font-weight:bold;} </style>');
                printWindow.document.write('</head><body >');
                printWindow.document.write(panel.innerHTML);
                printWindow.document.write('</body></html>');
                printWindow.document.close();
                setTimeout(function () {
                    //<div id="footer" style="background-color:White;"></div>
                    // <div id="header" style="background-color:White;"></div>
                    //                document.getElementById('header').style.display = 'none';
                    //                document.getElementById('footer').style.display = 'none';
                    printWindow.print();
                }, 500);
                return false;
            }

            //Ended by Idhris
            function flg() {

                document.getElementById('<%=btnadd.ClientID%>').style.display = 'block';
                document.getElementById('<%=btnminus.ClientID%>').style.display = 'block';
            }
            function flg1() {

                document.getElementById('<%=Button13.ClientID%>').style.display = 'block';
                document.getElementById('<%=Button14.ClientID%>').style.display = 'block';
            }

            function hideclick() {
                document.getElementById('<%=panel4.ClientID%>').style.display = 'none';
                return false;
            }

            function showpopup() {
                document.getElementById("<%=txt_keyword.ClientID %>").value = "";
                document.getElementById('<%=paneltemp.ClientID%>').style.display = 'block';
                return false;
            }

            function showfeepopup() {
                showunfeepopup
                document.getElementById("<%=txt_feeconfirm.ClientID %>").value = "";
                document.getElementById('<%=panelfeeconfirm.ClientID%>').style.display = 'block';
                return false;
            }

            function showunfeepopup() {
                document.getElementById("<%=txt_unpaidkeyword.ClientID %>").value = "";
                document.getElementById('<%=panelunpaid.ClientID%>').style.display = 'block';
                return false;
            }

            function verifystatus() {
                var agevalue = document.getElementById("<%=txt_keyword.ClientID %>").value;
                if (agevalue == "jcpnm") {
                    document.getElementById('<%=paneltemp.ClientID%>').style.display = 'none';
                    return true;
                }
                else {
                    document.getElementById('<%=paneltemp.ClientID%>').style.display = 'none';
                    alert('Enter correct Key Word');
                    return false;
                }
            }

            function verifyfeeconfirm() {
                var agevalue = document.getElementById("<%=txt_feeconfirm.ClientID %>").value;
                if (agevalue == "aaaa") {
                    document.getElementById('<%=panelfeeconfirm.ClientID%>').style.display = 'none';
                    return true;
                }
                else {
                    document.getElementById('<%=panelfeeconfirm.ClientID%>').style.display = 'none';
                    alert('Enter correct Key Word');
                    return false;
                }
            }

            function verifyfeeunconfirm() {
                var agevalue = document.getElementById("<%=txt_unpaidkeyword.ClientID %>").value;
                if (agevalue == "aaaa") {
                    document.getElementById('<%=panelunpaid.ClientID%>').style.display = 'none';
                    return true;
                }
                else {
                    document.getElementById('<%=panelunpaid.ClientID%>').style.display = 'none';
                    alert('Enter correct Key Word');
                    return false;
                }
            }
            function minpop() {

            }        
        </script>
        <center>
            <asp:Panel ID="paneltemp" runat="server" Style="display: none; height: 100em; z-index: 1000;
                width: 100%; position: absolute; top: 0; left: 0;">
                <center>
                    <div style="background-color: #FFFFFF; height: 200px; margin-top: 180px; width: 300px;">
                        <br />
                        <br />
                        <span id="Span1" runat="server">Enter your key word</span>&nbsp;
                        <asp:TextBox ID="txt_keyword" runat="server" TextMode="Password"></asp:TextBox>
                        <br />
                        <br />
                        <br />
                        <br />
                        <asp:Button ID="Button4" runat="server" Style="background-color: rgb(0, 128, 128);
                            border: 0px; color: White;" Text="Okay" OnClientClick="return verifystatus()"
                            OnClick="btn_confirm_clcik" />
                        <%-- <button style="background-color: rgb(0, 128, 128); border: 0px; color: White;">
                    Okay</button>--%>
                    </div>
                </center>
            </asp:Panel>
            <asp:Panel ID="panelfeeconfirm" runat="server" Style="display: none; height: 100em;
                z-index: 1000; width: 100%; position: absolute; top: 0; left: 0;">
                <center>
                    <div style="background-color: #FFFFFF; height: 200px; margin-top: 180px; width: 300px;">
                        <br />
                        <br />
                        <span id="Span2" runat="server">Enter your key word</span>&nbsp;
                        <asp:TextBox ID="txt_feeconfirm" runat="server" TextMode="Password"></asp:TextBox>
                        <br />
                        <br />
                        <br />
                        <br />
                        <asp:Button ID="btnfeeconfirm" runat="server" Style="background-color: rgb(0, 128, 128);
                            border: 0px; color: White;" Text="Okay" OnClientClick="return verifyfeeconfirm()"
                            OnClick="btnfeeconfirm_clcik" />
                        <%-- <button style="background-color: rgb(0, 128, 128); border: 0px; color: White;">
                    Okay</button>--%>
                    </div>
                </center>
            </asp:Panel>
            <asp:Panel ID="panelunpaid" runat="server" Style="display: none; height: 100em; z-index: 1000;
                width: 100%; position: absolute; top: 0; left: 0;">
                <center>
                    <div style="background-color: #FFFFFF; height: 200px; margin-top: 180px; width: 300px;">
                        <br />
                        <br />
                        <span id="Span3" runat="server">Enter your key word</span>&nbsp;
                        <asp:TextBox ID="txt_unpaidkeyword" runat="server" TextMode="Password"></asp:TextBox>
                        <br />
                        <br />
                        <br />
                        <br />
                        <asp:Button ID="btnunpaidconfirm" runat="server" Style="background-color: rgb(0, 128, 128);
                            border: 0px; color: White;" Text="Okay" OnClientClick="return verifyfeeunconfirm()"
                            OnClick="btnunpaidconfirm_clcik" />
                        <%-- <button style="background-color: rgb(0, 128, 128); border: 0px; color: White;">
                    Okay</button>--%>
                    </div>
                </center>
            </asp:Panel>
            <div style="width: 100%; margin-left: 0px; margin-top: 0px; position: absolute;">
                <asp:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server">
                </asp:ToolkitScriptManager>
                <asp:Panel ID="Panel23" runat="server" Visible="false" CssClass="modalPopup" Style="height: 113px;
                    left: 236px; top: 310px; width: 500px; position: absolute;">
                    <table width="500">
                        <tr class="topHandle" style="background-color: lightblue;">
                            <td colspan="2" align="left" runat="server" id="td1">
                                <asp:Label ID="Label28" runat="server" Font-Bold="True" Text="Confirmation" Font-Names="Book Antiqua"
                                    Font-Size="Large"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 60px" valign="middle" align="center">
                                <asp:Image ID="Image1" runat="server" ImageUrl="~/Info-48x48.png" />
                            </td>
                            <td valign="middle" align="left">
                                <%--<asp:UpdatePanel ID="UpdatePanel1" runat="server">
                        <ContentTemplate>--%>
                                <asp:Label ID="Label29" runat="server" Text="" Font-Bold="True" Font-Names="Book Antiqua"
                                    Font-Size="Medium"></asp:Label>
                                <%-- </ContentTemplate>
                    </asp:UpdatePanel>--%>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2" align="right">
                                <asp:Button ID="btnsavek" runat="server" Text="Yes" OnClick="bttnsavekclick" Font-Bold="True"
                                    Font-Names="Book Antiqua" Font-Size="Medium" Style="left: 398px; position: absolute;
                                    top: 81px;" />
                                <asp:Button ID="btnsaveexit" runat="server" Text="No" OnClick="bttnsaveexitclick"
                                    Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium" Style="left: 449px;
                                    position: absolute; top: 81px;" />
                            </td>
                        </tr>
                    </table>
                </asp:Panel>
                <asp:Panel ID="Panel5" runat="server" CssClass="cpBody" Width="980px" Height="763px"
                    Visible="true" Style="border-radius: 6px; background-color: darkcyan;">
                    <asp:Panel ID="Panel1" runat="server" Style="background-color: darkcyan; width: 980px;
                        height: 30px; border-radius: 6px;">
                        <table style="width: 900px;">
                            <tr>
                                <td style="text-align:center;">
                                    <asp:Label ID="Label1" runat="server" Text="Selection Process-Admission" Font-Bold="True"
                                        Font-Names="Book Antiqua" Font-Size="large" ForeColor="white"></asp:Label>
                                </td>
                               <%-- <td style="text-align: right;">
                                    <asp:LinkButton ID="LinkButton2" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                        Font-Size="Small" ForeColor="white" PostBackUrl="~/Student.aspx" CausesValidation="False">Back</asp:LinkButton>
                                    &nbsp;
                                    <asp:LinkButton ID="Home_btn" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                        Font-Size="Small" ForeColor="white" PostBackUrl="~/Default_login.aspx" CausesValidation="False">Home</asp:LinkButton>
                                    &nbsp;
                                    <asp:LinkButton ID="Logout_btn" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                        Font-Size="Small" ForeColor="white" CausesValidation="False" OnClick="Logout_btn_Click">Logout</asp:LinkButton>
                                </td>--%>
                            </tr>
                        </table>
                    </asp:Panel>
                    <br />
                    <asp:TabContainer ID="TabContainer1" runat="server" Visible="true" Height="650px"
                        CssClass="ajax__myTab" BackColor="Lavender" Width="950" ActiveTabIndex="0" OnActiveTabChanged="TabContainer1_ActiveTabChanged"
                        AutoPostBack="true">
                        <asp:TabPanel ID="tabpanel1" runat="server" HeaderText="Applied" Font-Names="Book Antiqua"
                            CssClass="ajax__myTab1" Font-Size="Medium" Visible="true" TabIndex="1">
                            <ContentTemplate>
                                <table>
                                    <tr>
                                        <td>
                                            <br />
                                            <br />
                                            <br />
                                            <br />
                                            <br />
                                            <br />
                                            <br />
                                            <br />
                                            <FarPoint:FpSpread ID="FpSpread3" runat="server" BorderColor="Black" BorderStyle="Solid"
                                                BorderWidth="1px" AutoPostBack="true" OnButtonCommand="FpSpread3_command" Style="top: 260px;"
                                                Visible="false" HierBar-ShowParentRow="False">
                                                <Sheets>
                                                    <FarPoint:SheetView SheetName="Sheet1" GridLineColor="Black">
                                                    </FarPoint:SheetView>
                                                </Sheets>
                                            </FarPoint:FpSpread>
                                        </td>
                                    </tr>
                                </table>
                            </ContentTemplate>
                        </asp:TabPanel>
                        <asp:TabPanel ID="tabpanel2" runat="server" HeaderText="ShortList" Font-Names="Book Antiqua"
                            Font-Size="Medium" Visible="false" TabIndex="2">
                            <ContentTemplate>
                                <table>
                                    <tr>
                                        <td>
                                            <br />
                                            <br />
                                            <br />
                                            <br />
                                            <br />
                                            <br />
                                            <br />
                                            <br />
                                            <FarPoint:FpSpread ID="FpSpread1" runat="server" BorderColor="Black" BorderStyle="Solid"
                                                BorderWidth="1px" AutoPostBack="true" OnButtonCommand="FpSpread1_command" Style="top: 260px;"
                                                Visible="false" HierBar-ShowParentRow="False">
                                                <Sheets>
                                                    <FarPoint:SheetView SheetName="Sheet1" GridLineColor="Black">
                                                    </FarPoint:SheetView>
                                                </Sheets>
                                            </FarPoint:FpSpread>
                                        </td>
                                    </tr>
                                </table>
                            </ContentTemplate>
                        </asp:TabPanel>
                        <asp:TabPanel ID="tabpanel3" runat="server" HeaderText="Waiting for-Admitted" Font-Names="Book Antiqua"
                            Font-Size="Medium" Visible="false" TabIndex="3">
                            <ContentTemplate>
                                <table>
                                    <tr>
                                        <td>
                                            <br />
                                            <br />
                                            <br />
                                            <br />
                                            <br />
                                            <br />
                                            <br />
                                            <br />
                                            <FarPoint:FpSpread ID="FpSpread2" runat="server" BorderColor="Black" BorderStyle="Solid"
                                                BorderWidth="1px" AutoPostBack="true" OnButtonCommand="FpSpread2_command" Style="top: 260px;"
                                                Visible="false">
                                                <Sheets>
                                                    <FarPoint:SheetView SheetName="Sheet1" GridLineColor="Black" SelectionBackColor="Lavender">
                                                    </FarPoint:SheetView>
                                                </Sheets>
                                            </FarPoint:FpSpread>
                                        </td>
                                    </tr>
                                </table>
                            </ContentTemplate>
                        </asp:TabPanel>
                        <asp:TabPanel ID="tabpanel4" runat="server" HeaderText="EnRollment" Font-Names="Book Antiqua"
                            Font-Size="Medium" Visible="false" TabIndex="4">
                            <ContentTemplate>
                                <table>
                                    <tr>
                                        <td>
                                            <br />
                                            <br />
                                            <br />
                                            <br />
                                            <br />
                                            <br />
                                            <br />
                                            <br />
                                            <FarPoint:FpSpread ID="FpSpread4" runat="server" BorderColor="Black" BorderStyle="Solid"
                                                BorderWidth="1px" AutoPostBack="true" Style="top: 260px;" Visible="false" OnButtonCommand="FpSpread4_command">
                                                <Sheets>
                                                    <FarPoint:SheetView SheetName="Sheet1" GridLineColor="Black" SelectionBackColor="Lavender">
                                                    </FarPoint:SheetView>
                                                </Sheets>
                                            </FarPoint:FpSpread>
                                        </td>
                                    </tr>
                                </table>
                            </ContentTemplate>
                        </asp:TabPanel>
                    </asp:TabContainer>
                    <div style="top: 89px; position: absolute; text-align: left;">
                        <table id="firsttable" runat="server" style="border-bottom-style: solid; border-top-style: solid;
                            border-left-style: solid; border-right-style: solid; background-color: #e3e3ef;
                            border-width: 0.2px; border-color: indigo; border-radius: 5px; left: 16px; top: 0px;
                            position: absolute; width: 950px;">
                            <tr>
                                <td>
                                    <asp:Label ID="lbl_college" runat="server" Text="College" Font-Bold="True" Font-Names="Book Antiqua"
                                        Font-Size="Medium" Width="55px"></asp:Label>
                                </td>
                                <td>
                                    <asp:DropDownList ID="ddl_collegename" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                        Font-Size="Medium" Width="250px" AutoPostBack="True" OnSelectedIndexChanged="ddl_collegename_SelectedIndexchange">
                                    </asp:DropDownList>
                                </td>
                                <td>
                                    <asp:Label ID="Label2" runat="server" Text="Batch :" Font-Bold="True" Font-Names="Book Antiqua"
                                        Font-Size="Medium" Width="55px"></asp:Label>
                                </td>
                                <td>
                                    <%--<asp:TextBox ID="txtbatch" runat="server" ReadOnly="true" Width="91px" Font-Bold="True"
                            Font-Size="Medium" Font-Names="Book Antiqua"></asp:TextBox>--%>
                                    <%--<asp:Label ID="txtbatch" runat="server" Text="" Font-Bold="True" Font-Names="Book Antiqua"
                                    Font-Size="Medium" Width="40px" ForeColor="Brown"></asp:Label>--%>
                                    <asp:DropDownList ID="ddl_batch" Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium"
                                        Height="27px" runat="server" CssClass=" textbox1 txtheight" AutoPostBack="true"
                                        OnSelectedIndexChanged="ddl_batch_SelectedIndexChanged">
                                    </asp:DropDownList>
                                </td>
                                <td>
                                    <asp:Label ID="Label65" Visible="false" runat="server" Text="Type" Font-Bold="True"
                                        Font-Names="Book Antiqua" Font-Size="Medium"></asp:Label>
                                    <asp:DropDownList ID="ddltype" Visible="false" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                        Font-Size="Medium" Width="110px" OnSelectedIndexChanged="ddltype_select" AutoPostBack="True">
                                        <asp:ListItem Value="0">--Select--</asp:ListItem>
                                    </asp:DropDownList>
                                </td>
                                <td>
                                    <asp:Label ID="Label4" runat="server" Text="Edu Level" Font-Bold="True" Font-Names="Book Antiqua"
                                        Font-Size="Medium"></asp:Label>
                                    <asp:DropDownList ID="ddledu" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                        Font-Size="Medium" Width="70px" AutoPostBack="True" OnSelectedIndexChanged="ddledu_SelectedIndexchange">
                                        <asp:ListItem Value="0">--Select--</asp:ListItem>
                                    </asp:DropDownList>
                                </td>
                                <td>
                                    <asp:Label ID="Label3" runat="server" Text="Degree" Font-Bold="True" Font-Names="Book Antiqua"
                                        Font-Size="Medium" Width="100px"></asp:Label>
                                    <asp:DropDownList ID="ddldegree" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                        Font-Size="Medium" Width="78px" AutoPostBack="True" OnSelectedIndexChanged="ddldegreeselected">
                                        <asp:ListItem Value="0">--Select--</asp:ListItem>
                                    </asp:DropDownList>
                                </td>
                            </tr>
                        </table>
                        <table style="border-bottom-style: solid; border-top-style: solid; border-left-style: solid;
                            border-right-style: solid; background-color: #e3e3ef; border-width: 0.2px; border-color: indigo;
                            border-top-width: 1px; border-radius: 5px; left: 16px; top: 33px; position: absolute;
                            width: 948px;">
                            <tr>
                                <td>
                                    <asp:Label ID="Label5" runat="server" Text="Dept" Font-Bold="True" Font-Names="Book Antiqua"
                                        Font-Size="Medium"></asp:Label>
                                    <asp:DropDownList ID="ddldept" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                        Font-Size="Medium" Width="160px" AutoPostBack="true" OnSelectedIndexChanged="ddldept_Change">
                                        <asp:ListItem Value="0">--Select--</asp:ListItem>
                                    </asp:DropDownList>
                                </td>
                                <td>
                                    <asp:Label ID="Label59" runat="server" Text="From Date" Font-Bold="True" Font-Names="Book Antiqua"
                                        Font-Size="Medium"></asp:Label>
                                    <asp:TextBox ID="TextBox2" runat="server" Width="94px" Font-Bold="True" Font-Names="Book Antiqua"
                                        Font-Size="Medium"></asp:TextBox>
                                    <asp:CalendarExtender ID="CalendarExtender1" runat="server" Format="dd/MM/yyyy" TargetControlID="TextBox2" />
                                    <asp:TextBox ID="txt_curdate" runat="server" Visible="false" Width="94px" Font-Bold="True"
                                        Font-Names="Book Antiqua" Font-Size="Medium"></asp:TextBox>
                                </td>
                                <td>
                                    <asp:Label ID="Label60" runat="server" Text="To Date" Font-Bold="True" Font-Names="Book Antiqua"
                                        Font-Size="Medium"></asp:Label>
                                    <asp:TextBox ID="TextBox3" runat="server" Width="90px" Font-Bold="True" Font-Names="Book Antiqua"
                                        Font-Size="Medium"></asp:TextBox>
                                    <asp:CalendarExtender ID="CalendarExtender2" runat="server" Format="dd/MM/yyyy" TargetControlID="TextBox3" />
                                </td>
                                <td>
                                    <asp:Label ID="lbl_religion" Text="Religion" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                        Font-Size="Medium"></asp:Label>
                                </td>
                                <td>
                                    <asp:UpdatePanel ID="UpdatePanel7" runat="server">
                                        <ContentTemplate>
                                            <asp:TextBox ID="txt_religion" runat="server" CssClass="textbox textbox1 txtheight"
                                                ReadOnly="true" Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium"
                                                Enabled="true">-- Select--</asp:TextBox>
                                            <asp:Panel ID="Panel7" runat="server" BackColor="White" BorderColor="Black" BorderStyle="Solid"
                                                BorderWidth="2px" CssClass="multxtpanel" Height="250px" Width="150px" Style="position: absolute;">
                                                <asp:CheckBox ID="cb_religion" Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium"
                                                    runat="server" Text="Select All" AutoPostBack="true" OnCheckedChanged="cb_religion_checkedchange" />
                                                <asp:CheckBoxList ID="cbl_religion" Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium"
                                                    runat="server" AutoPostBack="true" OnSelectedIndexChanged="cbl_religion_SelectedIndexChanged">
                                                </asp:CheckBoxList>
                                            </asp:Panel>
                                            <asp:PopupControlExtender ID="PopupControlExtender7" runat="server" TargetControlID="txt_religion"
                                                PopupControlID="Panel7" Position="Bottom">
                                            </asp:PopupControlExtender>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                                <td>
                                    <asp:Label ID="lbl_comm" Text="Community" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                        Font-Size="Medium"></asp:Label>
                                </td>
                                <td>
                                    <asp:UpdatePanel ID="UpdatePanel8" runat="server">
                                        <ContentTemplate>
                                            <asp:TextBox ID="txt_comm" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                                Font-Size="Medium" CssClass="textbox textbox1 txtheight" ReadOnly="true" Enabled="true">-- Select--</asp:TextBox>
                                            <asp:Panel ID="Panel8" runat="server" BackColor="White" BorderColor="Black" BorderStyle="Solid"
                                                BorderWidth="2px" CssClass="multxtpanel" Width="140px" Height="250px" Style="position: absolute;">
                                                <asp:CheckBox ID="cb_comm" Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium"
                                                    runat="server" Text="Select All" AutoPostBack="true" OnCheckedChanged="cb_comm_checkedchange" />
                                                <asp:CheckBoxList ID="cbl_comm" Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium"
                                                    runat="server" AutoPostBack="true" OnSelectedIndexChanged="cbl_comm_SelectedIndexChanged">
                                                </asp:CheckBoxList>
                                            </asp:Panel>
                                            <asp:PopupControlExtender ID="PopupControlExtender8" runat="server" TargetControlID="txt_comm"
                                                PopupControlID="Panel8" Position="Bottom">
                                            </asp:PopupControlExtender>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="lbl_org_sem" Text="Semester" Font-Bold="True" runat="server" Font-Names="Book Antiqua"
                                        Font-Size="Medium"></asp:Label>
                                    <%-- </td>
                            <td>--%>
                                    <asp:DropDownList ID="ddl_sem" Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium"
                                        CssClass="ddlheight textbox textbox1" runat="server">
                                    </asp:DropDownList>
                                </td>
                                <td>
                                    <asp:Label ID="Label15" Text="Row Count Size" Font-Bold="True" runat="server" Font-Names="Book Antiqua"
                                        Font-Size="Medium"></asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtrowcount" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                        Font-Size="Medium" CssClass="textbox textbox1 txtheight" Width="50px"></asp:TextBox>
                                </td>
                                <td>
                                    <asp:Label ID="Label8" Text="Board" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                        Font-Size="Medium"></asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtBoardUniv" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                        Font-Size="Medium" CssClass="textbox textbox1 txtheight" ReadOnly="true" Enabled="true">Board</asp:TextBox>
                                    <asp:Panel ID="pnlBoardUniv" runat="server" BackColor="White" BorderColor="Black"
                                        BorderStyle="Solid" BorderWidth="2px" CssClass="multxtpanel" Width="140px" Height="250px"
                                        Style="position: absolute;">
                                        <asp:CheckBox ID="cb_BoardUniv" Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium"
                                            runat="server" Text="Select All" AutoPostBack="true" OnCheckedChanged="cb_BoardUniv_checkedchange" />
                                        <asp:CheckBoxList ID="cbl_BoardUniv" Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium"
                                            runat="server" AutoPostBack="true" OnSelectedIndexChanged="cbl_BoardUniv_SelectedIndexChanged">
                                        </asp:CheckBoxList>
                                    </asp:Panel>
                                    <asp:PopupControlExtender ID="pceBoardUniv" runat="server" TargetControlID="txtBoardUniv"
                                        PopupControlID="pnlBoardUniv" Position="Bottom">
                                    </asp:PopupControlExtender>
                                </td>
                                <td>
                                    <asp:Label ID="Label9" Text="Attempt" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                        Font-Size="Medium"></asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtAttempt" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                        Font-Size="Medium" CssClass="textbox textbox1 txtheight" ReadOnly="true" Enabled="true">Attempt</asp:TextBox>
                                    <asp:Panel ID="pnlAttempt" runat="server" BackColor="White" BorderColor="Black" BorderStyle="Solid"
                                        BorderWidth="2px" CssClass="multxtpanel" Width="140px" Height="250px" Style="position: absolute;">
                                        <asp:CheckBox ID="cbAttempt" Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium"
                                            runat="server" Text="Select All" AutoPostBack="true" OnCheckedChanged="cbAttempt_checkedchange" />
                                        <asp:CheckBoxList ID="cblAttempt" Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium"
                                            runat="server" AutoPostBack="true" OnSelectedIndexChanged="cblAttempt_SelectedIndexChanged">
                                        </asp:CheckBoxList>
                                    </asp:Panel>
                                    <asp:PopupControlExtender ID="pceAttempt" runat="server" TargetControlID="txtAttempt"
                                        PopupControlID="pnlAttempt" Position="Bottom">
                                    </asp:PopupControlExtender>
                                </td>
                            </tr>
                        </table>
                        <table id="Table1" runat="server" style="border-bottom-style: solid; border-top-style: solid;
                            border-left-style: solid; border-right-style: solid; background-color: #e3e3ef;
                            border-width: 0.2px; border-color: indigo; border-radius: 5px; left: 16px; top: 107px;
                            position: absolute; width: 950px;">
                            <tr>
                                <td>
                                    <asp:Label ID="lbl_searchstudname" runat="server" Text="Name" Font-Bold="True" Font-Names="Book Antiqua"
                                        Font-Size="Medium"></asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txt_searchstudname" Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium"
                                        runat="server" CssClass="textbox textbox1 txtheight2" Width="135px" OnTextChanged="txt_searchstudname_TextChanged"></asp:TextBox>
                                    <asp:AutoCompleteExtender ID="acext_name" runat="server" DelimiterCharacters="" Enabled="True"
                                        ServiceMethod="Getname" MinimumPrefixLength="0" CompletionInterval="100" EnableCaching="false"
                                        CompletionSetCount="10" ServicePath="" TargetControlID="txt_searchstudname" CompletionListCssClass="autocomplete_completionListElement"
                                        CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem" CompletionListItemCssClass="panelbackground">
                                    </asp:AutoCompleteExtender>
                                    <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender3" runat="server" TargetControlID="txt_searchstudname"
                                        FilterType="UppercaseLetters,LowercaseLetters,custom" ValidChars=" .-">
                                    </asp:FilteredTextBoxExtender>
                                </td>
                                <td>
                                    <asp:Label ID="lbl_searchappno" runat="server" Text="Application No" Font-Bold="True"
                                        Font-Names="Book Antiqua" Font-Size="Medium"></asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txt_searchappno" Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium"
                                        runat="server" CssClass="textbox textbox1 txtheight" Width="100px" OnTextChanged="txt_searchappno_TextChanged"></asp:TextBox>
                                    <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender02" runat="server" TargetControlID="txt_searchappno"
                                        FilterType="UppercaseLetters,LowercaseLetters,custom,numbers" ValidChars=" ">
                                    </asp:FilteredTextBoxExtender>
                                    <asp:AutoCompleteExtender ID="AutoCompleteExtender2" runat="server" DelimiterCharacters=""
                                        Enabled="True" ServiceMethod="getappfrom" MinimumPrefixLength="0" CompletionInterval="100"
                                        EnableCaching="false" CompletionSetCount="10" ServicePath="" TargetControlID="txt_searchappno"
                                        CompletionListCssClass="autocomplete_completionListElement" CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem"
                                        CompletionListItemCssClass="panelbackground">
                                    </asp:AutoCompleteExtender>
                                </td>
                                <td>
                                    <asp:TextBox ID="txt_searchmobno" Placeholder="Mobile Number" Font-Bold="True" Font-Names="Book Antiqua"
                                        Font-Size="Medium" runat="server" CssClass="textbox textbox1 txtheight2" Width="125px"
                                        MaxLength="13" OnTextChanged="txt_searchmobno_TextChanged"></asp:TextBox>
                                    <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender2" runat="server" TargetControlID="txt_searchmobno"
                                        FilterType="numbers,custom" ValidChars=" +">
                                    </asp:FilteredTextBoxExtender>
                                    <asp:AutoCompleteExtender ID="AutoCompleteExtender1" runat="server" DelimiterCharacters=""
                                        Enabled="True" ServiceMethod="getmob" MinimumPrefixLength="0" CompletionInterval="100"
                                        EnableCaching="false" CompletionSetCount="10" ServicePath="" TargetControlID="txt_searchmobno"
                                        CompletionListCssClass="autocomplete_completionListElement" CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem"
                                        CompletionListItemCssClass="panelbackground">
                                    </asp:AutoCompleteExtender>
                                </td>
                                <td>
                                    <asp:Label ID="Label61" runat="server" Visible="false" Text="Show On" Font-Bold="True"
                                        Font-Names="Book Antiqua" Font-Size="Medium"></asp:Label>
                                    <asp:DropDownList ID="DropDownList2" Visible="false" runat="server" Font-Bold="True"
                                        Font-Names="Book Antiqua" Font-Size="Medium" Width="90px" AutoPostBack="True"
                                        OnSelectedIndexChanged="DropDownList2_OnSelectedIndexChanged">
                                        <asp:ListItem Value="1">Admitted</asp:ListItem>
                                        <asp:ListItem Value="2">Left</asp:ListItem>
                                        <asp:ListItem Value="3">Fee Paid</asp:ListItem>
                                    </asp:DropDownList>
                                </td>
                                <td>
                                    <asp:LinkButton ID="linksetting" Visible="true" runat="server" Text="Settings" Font-Bold="True"
                                        Font-Names="Book Antiqua" Font-Size="Medium" PostBackUrl="~/studentmod/Add_Settings.aspx"
                                        Width="84px"></asp:LinkButton>
                                </td>
                                <td>
                                    <span>
                                        <asp:CheckBox ID="cbAltCourse" runat="server" Text="Alternate Course" Font-Bold="true"
                                            AutoPostBack="true" OnCheckedChanged="cbAltCourse_CheckedChanged" /></span>
                                </td>
                                <td>
                                    <asp:Button ID="Button1" CssClass="btnapprove1" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                        Font-Size="Medium" Text="Go" Width="44px" Style="border: 1px solid indigo;" OnClick="Button1_Click" />
                                </td>
                            </tr>
                        </table>
                        <table style="left: 16px; top: 140px; position: absolute; width: 950px; clear: both;">
                            <tr>
                                <td style="width: 130px;">
                                    <asp:Label ID="lblmg" runat="server" Text="" Visible="false" Font-Bold="True" Font-Names="Book Antiqua"
                                        Font-Size="Medium" ForeColor="Red"></asp:Label>
                                    <asp:Label ID="lblmg1" runat="server" Text="" Visible="false" Font-Bold="True" Font-Names="Book Antiqua"
                                        Font-Size="Medium" ForeColor="Red"></asp:Label>
                                    <asp:Label ID="lblmg2" runat="server" Text="" Visible="false" Font-Bold="True" Font-Names="Book Antiqua"
                                        Font-Size="Medium" ForeColor="Red"></asp:Label>
                                    <asp:Label ID="lblmg3" runat="server" Text="" Visible="false" Font-Bold="True" Font-Names="Book Antiqua"
                                        Font-Size="Medium" ForeColor="Red"></asp:Label>
                                    <asp:Label ID="lbltotalcount" runat="server" Text="" Visible="false" Font-Bold="True"
                                        Font-Names="Book Antiqua" Font-Size="Medium"></asp:Label>
                                    <asp:Label ID="lbltotalcount1" runat="server" Text="" Visible="false" Font-Bold="True"
                                        Font-Names="Book Antiqua" Font-Size="Medium"></asp:Label>
                                    <asp:Label ID="lbltotalcount2" runat="server" Text="" Visible="false" Font-Bold="True"
                                        Font-Names="Book Antiqua" Font-Size="Medium"></asp:Label>
                                    <asp:Label ID="lbltotalcount3" runat="server" Text="" Visible="false" Font-Bold="True"
                                        Font-Names="Book Antiqua" Font-Size="Medium"></asp:Label>
                                </td>
                                <td style="width: 200px;">
                                    <asp:Label ID="lbltotalfeepaid" runat="server" Text="" Visible="false" Font-Bold="True"
                                        Font-Names="Book Antiqua" Font-Size="Medium"></asp:Label>
                                    &nbsp;&nbsp;
                                    <asp:Label ID="lbltotalfeepaid_value" runat="server" Text="" Visible="false" Font-Bold="True"
                                        Font-Names="Book Antiqua" Font-Size="Medium"></asp:Label>
                                </td>
                                <td style="width: 130px;">
                                </td>
                                <td style="width: 194px;">
                                    <asp:Label ID="Label66" runat="server" Visible="false" Font-Bold="True" Font-Names="Book Antiqua"
                                        Font-Size="Medium"></asp:Label>
                                    <asp:Label ID="Label68" runat="server" Text="" Visible="false" Font-Bold="True" Font-Names="Book Antiqua"
                                        ForeColor="Brown" Font-Size="Medium"></asp:Label>
                                </td>
                            </tr>
                        </table>
                    </div>
                    <div style="margin-top: -500px; margin-left: -45px; position: absolute; width: 950px;">
                    </div>
                    <div style="margin-top: -693px; margin-left: 400px; position: absolute; text-align: right;">
                        <table>
                            <tr>
                                <td>
                                    <asp:Label ID="lblnew" runat="server" Text=" " Visible="false" Font-Bold="True" Font-Names="Book Antiqua"
                                        Font-Size="Small" ForeColor="black" Style="border-radius: 5px; background-color: papayawhip;"
                                        Width="52px"></asp:Label>
                                </td>
                                <td style="width: 200px;">
                                    <asp:Label ID="lblnew1" runat="server" Visible="false" Text=" " Font-Bold="True"
                                        Font-Names="Book Antiqua" Font-Size="Small" ForeColor="black" Style="border-radius: 5px;
                                        background-color: papayawhip;" Width="52px"></asp:Label>
                                </td>
                                <td style="width: 200px;">
                                    <asp:Label ID="lblnew2" runat="server" Text=" " Font-Bold="True" Visible="false"
                                        Font-Names="Book Antiqua" Font-Size="Small" ForeColor="black" Style="border-radius: 5px;
                                        background-color: papayawhip;" Width="52px"></asp:Label>
                                </td>
                            </tr>
                        </table>
                    </div>
                    <div style="margin-top: -33px; margin-left: 0px; position: absolute; width: 980px;">
                        <table style="width: 980px;">
                            <tr>
                                <td>
                                    <asp:CheckBox ID="ckbx" runat="server" Text="Select All" Font-Bold="True" Font-Names="Book Antiqua"
                                        Font-Size="Medium" Visible="false" AutoPostBack="true" OnCheckedChanged="Checkselect_CheckedChanged" />
                                </td>
                                <td style="width: 970px; text-align: center;">
                                    <asp:Button ID="btn_pdf" CssClass="btnapprove1" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                        Font-Size="Medium" Text="Pdf" Style="border: 2px solid orange;" Width="60px"
                                        Visible="true" OnClick="btn_pdf_Click" />
                                    <asp:Button ID="btnSendSmsOpenPop" CssClass="btnapprove1" runat="server" Font-Bold="True"
                                        Font-Names="Book Antiqua" Font-Size="Medium" Text="Send SMS" Style="border: 2px solid orange;"
                                        Width="90px" Visible="false" OnClick="btnSendSmsOpenPop_Click" />
                                    <asp:Button ID="btnapprove" CssClass="btnapprove1" runat="server" Font-Bold="True"
                                        Font-Names="Book Antiqua" Font-Size="Medium" Text="Move To Shortlist" Style="border: 2px solid orange;"
                                        Width="150px" Visible="false" OnClick="btnappclick" />
                                    <asp:Button ID="Button2" CssClass="btnapprove1" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                        Font-Size="Medium" Text="Move To Recommend" Style="border: 2px solid orange;"
                                        Width="180px" Visible="false" OnClick="btnappclick1" />
                                    <asp:Button ID="Button3" CssClass="btnapprove1" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                        Font-Size="Medium" Text="Move To Admit" Style="border: 2px solid orange;" Width="135px"
                                        Visible="false" OnClick="btnappclick2" />
                                    <asp:Button ID="btnleft" CssClass="btnapprove1" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                        Font-Size="Medium" Text="Left" Style="border: 2px solid orange;" Width="100px"
                                        Visible="false" OnClick="btnleft_click" />
                                    <asp:Button ID="btnadmitprint" CssClass="btnapprove1" runat="server" Font-Bold="True"
                                        Font-Names="Book Antiqua" Font-Size="Medium" Text="Print" Style="border: 2px solid orange;"
                                        Width="100px" Visible="false" OnClick="btnadmitprint_click" />
                                    <asp:Button ID="btnadmitcard" CssClass="btnapprove1" runat="server" Font-Bold="True"
                                        Font-Names="Book Antiqua" Font-Size="Medium" Text="Generate Admit Card" Style="border: 2px solid orange;"
                                        Width="175px" Visible="false" OnClientClick="return showpopup()" />
                                    <asp:Button ID="btnunpaid" CssClass="btnapprove1" runat="server" Font-Bold="True"
                                        Font-Names="Book Antiqua" Font-Size="Medium" Text="Un Paid" Style="border: 2px solid orange;"
                                        Width="127px" Visible="false" OnClientClick="return showunfeepopup()" />
                                    <asp:Button ID="btn_ch_gen" CssClass="btnapprove1" runat="server" Font-Bold="True"
                                        Font-Names="Book Antiqua" Font-Size="Medium" Text="Challan Generate" Style="border: 2px solid orange;"
                                        Width="140px" Visible="false" OnClick="btn_challanPrintClick" />
                                    <asp:Button ID="btnconform" CssClass="btnapprove1" runat="server" Font-Bold="True"
                                        Font-Names="Book Antiqua" Font-Size="Medium" Text="Fee Confirm" Style="border: 2px solid orange;"
                                        Width="104px" Visible="false" OnClick="btnconform_onclick" />
                                    <asp:Button ID="btnconformrecpt" CssClass="btnapprove1" runat="server" Font-Bold="True"
                                        Font-Names="Book Antiqua" Font-Size="Medium" Text="Fee Confirm" Style="border: 2px solid orange;"
                                        Width="104px" Visible="false" OnClick="btnconformrecpt_onclick" />
                                    <asp:Button ID="btn_calltr" CssClass="btnapprove1" runat="server" Font-Bold="True"
                                        Font-Names="Book Antiqua" Font-Size="Medium" Text="Call Letter" Style="border: 2px solid orange;"
                                        Width="89px" Visible="false" OnClick="btn_calltr_click2" />
                                    <asp:Button ID="Button9" CssClass="btnapprove1" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                        Font-Size="Medium" Text="Reject" Style="border: 2px solid orange;" Width="64px"
                                        Visible="false" OnClick="btnappreject" />
                                    <%--added by sudhagar--%>
                                    <asp:Button ID="buttnleft" CssClass="btnapprove1" runat="server" Font-Bold="True"
                                        Font-Names="Book Antiqua" Font-Size="Medium" Text="Left" Style="border: 2px solid orange;"
                                        Width="64px" Visible="false" OnClick="buttnleft_Click" />
                                    <asp:Label ID="lbl_reportname" runat="server" Text="Report Name" Font-Bold="True"
                                        Font-Names="Book Antiqua" Visible="false" Font-Size="Medium"></asp:Label>
                                    <asp:TextBox ID="txt_excelname" runat="server" CssClass="textbox textbox1 txtheight4"
                                        onkeypress="display()"></asp:TextBox>
                                    <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender56" runat="server" TargetControlID="txt_excelname"
                                        FilterType="LowercaseLetters,UppercaseLetters,Numbers,Custom" ValidChars="!@$%^&()_+}{][';,.">
                                    </asp:FilteredTextBoxExtender>
                                    <asp:Button ID="btn_excel" CssClass="btnapprove1" runat="server" Font-Bold="True"
                                        Font-Names="Book Antiqua" Font-Size="Medium" Text="Export To Excel" Style="border: 2px solid orange;"
                                        Width="127px" Visible="false" OnClick="btn_excel_click" />
                                    <asp:Button ID="btn_print" CssClass="btnapprove1" runat="server" Font-Bold="True"
                                        Font-Names="Book Antiqua" Font-Size="Medium" Text="Print" Style="border: 2px solid orange;"
                                        Width="54px" Visible="false" OnClick="btn_print_click" />
                                </td>
                            </tr>
                        </table>
                    </div>
                </asp:Panel>
                <%--<asp:ModalPopupExtender runat="server" ID="mdl_full_employee_details" TargetControlID="Button4"
        BackgroundCssClass="modal_popup_background_color" PopupControlID="panel6">
    </asp:ModalPopupExtender>
    <asp:Button ID="Button4" runat="server" Text="Button" Style="display: none;" />--%>
                <asp:Panel ID="panel4" runat="server" Style="background: none repeat scroll 0 0 rgba(0, 0, 0, 0.83);
                    border-color: inherit; border-style: none; border-width: 1px; height: 101%; left: -8px;
                    position: absolute; top: -9px; width: 101%; display: none;" BorderColor="Blue">
                    <asp:Panel ID="panel6" runat="server" Visible="true" BackColor="mediumaquamarine"
                        Style="border-style: none; border-color: inherit; border-width: 1px; height: 590px;
                        width: 985px; left: 7px; top: 67px; position: absolute;" BorderColor="Blue">
                        <br />
                        <div class="panel6" id="Div1" style="text-align: center; font-family: Book Antiqua;
                            font-size: medium; font-weight: bold">
                            <caption style="top: 20px; border-style: solid; border-color: Black; position: absolute;
                                left: 200px">
                                <asp:Label ID="Label17" runat="server" Text="Student Details" Font-Bold="true" Font-Size="Large"
                                    Font-Names="Book Antiqua"></asp:Label>
                            </caption>
                            <asp:Button ID="Button5" runat="server" Text="X" ForeColor="Black" OnClientClick="return hideclick()"
                                Style="top: 0px; left: 942px; position: absolute; height: 26px; border-width: 0;
                                background-color: mediumaquamarine; width: 25px;" Font-Bold="True" Font-Names="Microsoft Sans Serif"
                                Font-Size="Medium" />
                            <br />
                            <br />
                            <asp:Panel ID="panel3" runat="server" Visible="true" BackColor="Lavender" Height="500px"
                                ScrollBars="Vertical" Style="border-style: none; border-color: inherit; border-width: 1px;
                                width: 949px; left: 21px; position: absolute;" BorderColor="Blue">
                                <table align="right">
                                    <tr align="right">
                                        <td align="right">
                                            <asp:Button ID="Button6" Visible="false" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                                Font-Size="Medium" Text="Print" Width="50px" OnClick="Button6_Clcik" />
                                        </td>
                                    </tr>
                                </table>
                                <br />
                                <br />
                                <center>
                                    <%-- <asp:UpdatePanel ID="UpdatePaneltd" runat="server">
                                    <ContentTemplate>--%>
                                    <asp:CheckBox ID="cbpersonal" runat="server" Text="Personal Details" AutoPostBack="true"
                                        OnCheckedChanged="cbpersonal_Changed" Visible="false" Style="margin-left: -464px;" />
                                    <%-- </ContentTemplate>
                                </asp:UpdatePanel>--%>
                                    <div id="coursedetails" runat="server" visible="false">
                                        <table>
                                            <tr>
                                                <td colspan="2" style="text-align: center; color: White; background-color: brown;">
                                                    <span style="font-weight: bold; font-size: large;">Course Information</span>
                                                </td>
                                            </tr>
                                            <tr style="display: none;">
                                                <td>
                                                    <span>Stream</span>
                                                </td>
                                                <td>
                                                    <span id="college_span" runat="server"></span>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <span>Graduation</span>
                                                </td>
                                                <td>
                                                    <span id="degree_Span" runat="server"></span>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <span>Degree</span>
                                                </td>
                                                <td>
                                                    <span id="graduation_span" runat="server"></span>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <span>Subject / Course</span>
                                                </td>
                                                <td>
                                                    <span id="course_span" runat="server"></span>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td colspan="2" style="text-align: right;">
                                                </td>
                                            </tr>
                                            <tr>
                                                <td colspan="2" style="text-align: center; color: White; background-color: brown;">
                                                    <span style="font-weight: bold; font-size: large;">Personal Information</span>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <span>Applicant Name</span>
                                                </td>
                                                <td>
                                                    <span id="applicantname_span" runat="server"></span>
                                                </td>
                                            </tr>
                                            <tr style="display: none;">
                                                <td>
                                                    <span>Last Name</span>
                                                </td>
                                                <td>
                                                    <span id="lastname_span" runat="server"></span>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <span>Date of birth</span>
                                                </td>
                                                <td>
                                                    <span id="dob_span" runat="server"></span>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <span>Gender</span>
                                                </td>
                                                <td>
                                                    <span id="gender_span" runat="server"></span>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <span>Parent Name</span>
                                                </td>
                                                <td>
                                                    <span id="parent_span" runat="server"></span>
                                                </td>
                                            </tr>
                                            <tr style="display: none;">
                                                <td>
                                                    <span>RelationShip</span>
                                                </td>
                                                <td>
                                                    <span id="relationship_span" runat="server"></span>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <span>Occupation</span>
                                                </td>
                                                <td>
                                                    <span id="occupation_span" runat="server"></span>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <span>Mother Tongue</span>
                                                </td>
                                                <td>
                                                    <span id="mothertongue_span" runat="server"></span>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <span>Religion</span>
                                                </td>
                                                <td>
                                                    <span id="religion_span" runat="server"></span>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <span>Nationality</span>
                                                </td>
                                                <td>
                                                    <span id="nationality_span" runat="server"></span>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <span>Community</span>
                                                </td>
                                                <td>
                                                    <span id="commuity_span" runat="server"></span>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <span>Caste</span>
                                                </td>
                                                <td>
                                                    <span id="Caste_span" runat="server"></span>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <span>Are You of Tamil Origin From Andaman and Nicobar Islands ?</span>
                                                </td>
                                                <td>
                                                    <span id="tamilorigin_span" runat="server"></span>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <span>Are You a Child of an ex-serviceman of Tamil Nadu origin ?</span>
                                                </td>
                                                <td>
                                                    <span id="Ex_service_span" runat="server"></span>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <span>Are You Differently abled</span>
                                                </td>
                                                <td>
                                                    <span id="Differentlyable_Span" runat="server"></span>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <span>Are you first genaration learner ?</span>
                                                </td>
                                                <td>
                                                    <span id="first_generation_Span" runat="server"></span>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <span>Is Residence on Campus Required ? </span>
                                                </td>
                                                <td>
                                                    <span id="residancerequired_span" runat="server"></span>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <span>Distinction in Sports </span>
                                                </td>
                                                <td>
                                                    <span id="sport_span" runat="server"></span>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <span>Extra Curricular Activites/Co-Curricular Activites </span>
                                                </td>
                                                <td>
                                                    <span id="Co_Curricular_span" runat="server"></span>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <span>Are you NCC cadet?</span>
                                                </td>
                                                <td>
                                                    <span id="ncccadetspan" runat="server"></span>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td colspan="2" style="text-align: center; color: White; background-color: brown;">
                                                    <span style="font-weight: bold; font-size: large;">Communication Address</span>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <span>Address Line1</span>
                                                </td>
                                                <td>
                                                    <span id="caddressline1_span" runat="server"></span>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <span>Address Line2</span>
                                                </td>
                                                <td>
                                                    <span id="Addressline2_span" runat="server"></span>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <span>Address Line3</span>
                                                </td>
                                                <td>
                                                    <span id="Addressline3_span" runat="server"></span>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <span>City</span>
                                                </td>
                                                <td>
                                                    <span id="city_span" runat="server"></span>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <span>State</span>
                                                </td>
                                                <td>
                                                    <span id="state_span" runat="server"></span>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <span>Country</span>
                                                </td>
                                                <td>
                                                    <span id="Country_span" runat="server"></span>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <span>PIN Code</span>
                                                </td>
                                                <td>
                                                    <span id="Postelcode_Span" runat="server"></span>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <span>Mobile Number</span>
                                                </td>
                                                <td>
                                                    <span id="Mobilenumber_Span" runat="server"></span>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <span>Alternate Mobile No</span>
                                                </td>
                                                <td>
                                                    <span id="Alternatephone_span" runat="server"></span>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <span>Email ID</span>
                                                </td>
                                                <td>
                                                    <span id="emailid_span" runat="server"></span>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <span>Phone Number with (Landline) STD/ISD code</span>
                                                </td>
                                                <td>
                                                    <span id="std_ist_span" runat="server"></span>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td colspan="2" style="text-align: center; color: White; background-color: brown;">
                                                    <span style="font-weight: bold; font-size: large;">Permanent Address</span>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <span>Address Line1</span>
                                                </td>
                                                <td>
                                                    <span id="paddressline1_span" runat="server"></span>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <span>Address Line2</span>
                                                </td>
                                                <td>
                                                    <span id="paddressline2_span" runat="server"></span>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <span>Address Line3</span>
                                                </td>
                                                <td>
                                                    <span id="paddressline3_span" runat="server"></span>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <span>City</span>
                                                </td>
                                                <td>
                                                    <span id="pcity_span" runat="server"></span>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <span>State</span>
                                                </td>
                                                <td>
                                                    <span id="pstate_span" runat="server"></span>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <span>Country</span>
                                                </td>
                                                <td>
                                                    <span id="pcountry_span" runat="server"></span>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <span>PIN Code</span>
                                                </td>
                                                <td>
                                                    <span id="ppostelcode_span" runat="server"></span>
                                                </td>
                                            </tr>
                                            <tr style="display: none;">
                                                <td>
                                                    <span>Mobile Number</span>
                                                </td>
                                                <td>
                                                    <span id="pmobilenumber_span" runat="server"></span>
                                                </td>
                                            </tr>
                                            <tr style="display: none;">
                                                <td>
                                                    <span>Alternate Mobile No</span>
                                                </td>
                                                <td>
                                                    <span id="palternatephone_span" runat="server"></span>
                                                </td>
                                            </tr>
                                            <tr style="display: none;">
                                                <td>
                                                    <span>Email ID</span>
                                                </td>
                                                <td>
                                                    <span id="peamilid_span" runat="server"></span>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <span>Phone Number with (Landline) STD/ISD code</span>
                                                </td>
                                                <td>
                                                    <span id="pstdisd_span" runat="server"></span>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td colspan="2" style="text-align: right;">
                                                </td>
                                            </tr>
                                        </table>
                                    </div>
                                    <div id="Academicinfo" runat="server" visible="false">
                                        <div id="ugdiv_verification" runat="server" visible="false">
                                            <table>
                                                <tr>
                                                    <td colspan="2" style="text-align: center; color: White; background-color: brown;">
                                                        <span style="font-weight: bold; font-size: large;">Academic Information</span>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <span>Qualifying Examination Pass</span>
                                                    </td>
                                                    <td>
                                                        <span id="qualifyingexam_span" runat="server"></span>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <span>Name of School</span>
                                                    </td>
                                                    <td>
                                                        <span id="Nameofschool_span" runat="server"></span>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <span>Location of School</span>
                                                    </td>
                                                    <td>
                                                        <span id="locationofschool_Span" runat="server"></span>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <span>Medium of Study of Qualifying Examination</span>
                                                    </td>
                                                    <td>
                                                        <span id="mediumofstudy_span" runat="server"></span>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <span>Qualifying Board & State</span>
                                                    </td>
                                                    <td>
                                                        <span id="qualifyingboard_span" runat="server"></span>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <span>Are you Vocational stream</span>
                                                    </td>
                                                    <td>
                                                        <span id="Vocationalspan" runat="server"></span>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <span>Marks/Grade</span>
                                                    </td>
                                                    <td>
                                                        <span id="marksgrade_span" runat="server"></span>
                                                    </td>
                                                </tr>
                                            </table>
                                            <br />
                                            <asp:GridView ID="VerificationGridug" runat="server">
                                            </asp:GridView>
                                            <br />
                                        </div>
                                        <div id="pgdiv_verification" runat="server" visible="false">
                                            <table style="width: 600px;">
                                                <tr>
                                                    <td colspan="2" style="text-align: center; color: White; background-color: brown;">
                                                        <span style="font-weight: bold; font-size: large;">Academic Information</span>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <span>Qualifying Examination Passed</span>
                                                    </td>
                                                    <td>
                                                        <span id="ugqualifyingexam_span" runat="server"></span>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <span>Name of the College</span>
                                                    </td>
                                                    <td>
                                                        <span id="nameofcollege_Sapn" runat="server"></span>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <span>Location of the College</span>
                                                    </td>
                                                    <td>
                                                        <span id="locationofcollege_sapn" runat="server"></span>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <span>Mention Major</span>
                                                    </td>
                                                    <td>
                                                        <span id="major_span" runat="server"></span>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <span>Type of Major</span>
                                                    </td>
                                                    <td>
                                                        <span id="typeofmajor_span" runat="server"></span>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <span>Type of Semester</span>
                                                    </td>
                                                    <td>
                                                        <span id="typeofsemester_span" runat="server"></span>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <span>Medium of Study at UG level</span>
                                                    </td>
                                                    <td>
                                                        <span id="mediumofstudy_spanug" runat="server"></span>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <span>Marks/Grade</span>
                                                    </td>
                                                    <td>
                                                        <span id="marksorgradeug_span" runat="server"></span>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <span>Registration No as Mentioned on your Mark Sheet </span>
                                                    </td>
                                                    <td>
                                                        <span id="reg_no_span" runat="server"></span>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <span id="tnspan" runat="server" visible="false">TANCET Mark / Year of Pass </span>
                                                    </td>
                                                    <td>
                                                        <span id="Tancetspan" runat="server" visible="false"></span>
                                                    </td>
                                                </tr>
                                            </table>
                                            <br />
                                            <asp:GridView ID="Verificationgridpg" runat="server">
                                            </asp:GridView>
                                            <br />
                                        </div>
                                        <div id="ugtotaldiv" runat="server" visible="false">
                                            <table style="width: 700px;">
                                                <tr>
                                                    <td>
                                                        <span>Total Marks Obtained</span>
                                                    </td>
                                                    <td>
                                                        <span id="total_marks_secured" runat="server"></span>
                                                    </td>
                                                    <td>
                                                        <span>Maximum Marks</span>
                                                    </td>
                                                    <td>
                                                        <span id="maximum_marks" runat="server"></span>
                                                    </td>
                                                    <td>
                                                        <span>Percentage</span>
                                                    </td>
                                                    <td>
                                                        <span id="percentage_span" runat="server"></span>
                                                    </td>
                                                </tr>
                                            </table>
                                        </div>
                                        <div id="pgtotaldiv" runat="server" visible="false">
                                            <table style="width: 890px;">
                                                <tr>
                                                    <td>
                                                        <span>Total percentage of marks in all subjects (Language/major/Allied/Ancillary/Elective
                                                            inclusive ofTheory and Practical</span>
                                                    </td>
                                                    <td>
                                                        <span id="percentagemajorspan" runat="server"></span>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <span>Total % of Marks in Major subjects alone (Including theory & Practicals)</span>
                                                    </td>
                                                    <td>
                                                        <span id="majorsubjectspan" runat="server"></span>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <span>Total percentage marks in major/Allied/Ancillary subjects alone inclusive of Theory
                                                            and Practicals</span>
                                                    </td>
                                                    <td>
                                                        <span id="alliedmajorspan" runat="server"></span>
                                                    </td>
                                                </tr>
                                            </table>
                                        </div>
                                        <br />
                                        <br />
                                    </div>
                                    <div id="AddtionalInformationDiv" runat="server" visible="false">
                                        <center>
                                            <table>
                                                <tr>
                                                    <td colspan="6" style="text-align: center; color: White; background-color: brown;">
                                                        <span style="font-weight: bold; font-size: large;">Admission Information</span>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        Admission No
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="txt_AdmissionNo" runat="server" CssClass="  textbox txtheight" Width="120px"></asp:TextBox>
                                                    </td>
                                                    <td>
                                                        Admission Date
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="txt_AdmissionDate" runat="server" CssClass="  textbox txtheight"
                                                            Width="120px"></asp:TextBox>
                                                        <asp:CalendarExtender ID="CalendarExtender5" runat="server" Format="dd/MM/yyyy" TargetControlID="txt_AdmissionDate" />
                                                    </td>
                                                    <td rowspan="2">
                                                        <asp:Image ID="StudentImage" runat="server" Height="100px" Width="100px" />
                                                    </td>
                                                    <td>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <asp:CheckBox ID="cbCounselling" runat="server" AutoPostBack="true" OnCheckedChanged="cbCounselling_CheckedChange"
                                                            Checked="false" />
                                                        Counselling No
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="txtCounsellingNo" runat="server" CssClass="  textbox txtheight"
                                                            Width="120px" MaxLength="20" Enabled="false"></asp:TextBox>
                                                        <asp:FilteredTextBoxExtender ID="feCouns" runat="server" TargetControlID="txtCounsellingNo"
                                                            FilterType="UppercaseLetters,LowercaseLetters,Custom,Numbers" ValidChars=" ">
                                                        </asp:FilteredTextBoxExtender>
                                                    </td>
                                                    <td>
                                                        Counselling Date
                                                    </td>
                                                    <td>
                                                        <div style="position: relative;">
                                                            <asp:TextBox ID="txtCounsellingDt" runat="server" CssClass="  textbox txtheight"
                                                                Width="120px" Enabled="false"></asp:TextBox>
                                                            <asp:CalendarExtender ID="CalendarExtender6" runat="server" Format="dd/MM/yyyy" TargetControlID="txtCounsellingDt"
                                                                PopupPosition="BottomRight" />
                                                        </div>
                                                    </td>
                                                    <td>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        Roll No &nbsp;&nbsp;&nbsp;&nbsp;
                                                        <asp:CheckBox ID="cbSame" runat="server" AutoPostBack="true" OnCheckedChanged="cbSame_CheckedChange" />
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="txt_rollno" runat="server" CssClass="  textbox txtheight" Width="120px"></asp:TextBox>
                                                    </td>
                                                    <td>
                                                        Student Type
                                                    </td>
                                                    <td>
                                                        <asp:DropDownList ID="ddlAdmissionStudType" runat="server" CssClass="ddlheight ddlheight1"
                                                            AutoPostBack="true" OnSelectedIndexChanged="ddlAdmissionStudType_IndexChange"
                                                            Width="120px">
                                                            <asp:ListItem>Day Scholar</asp:ListItem>
                                                            <asp:ListItem>Hostler</asp:ListItem>
                                                        </asp:DropDownList>
                                                    </td>
                                                    <td>
                                                        <asp:LinkButton ID="linkbtn" runat="server" Text="Add Student Photo" OnClick="Link_Photo"></asp:LinkButton>
                                                    </td>
                                                    <td>
                                                        <asp:Button ID="btnUpdateInformation" runat="server" OnClick="btnUpdateInformation_Click"
                                                            Text="Update" />
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td colspan="5">
                                                        <asp:DropDownList ID="rbldayScTrans" runat="server" RepeatDirection="Horizontal"
                                                            Style="float: left;" CssClass="ddlheight ddlheight4" AutoPostBack="true" OnSelectedIndexChanged="rbldayScTrans_IndexChange"
                                                            Visible="false">
                                                            <asp:ListItem Value="0">Own Transport</asp:ListItem>
                                                            <asp:ListItem Value="1">Institution Transport</asp:ListItem>
                                                        </asp:DropDownList>
                                                        <asp:Label ID="lblBoardPnt" runat="server" Text="Boarding" Style="float: left; padding-top: 5px;
                                                            padding-left: 7px;" Visible="false"></asp:Label>
                                                        <asp:TextBox ID="txtBoardPnt" runat="server" Style="float: left;" CssClass="textbox txtheight5"
                                                            MaxLength="50" Visible="false"></asp:TextBox>
                                                        <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender4" runat="server" TargetControlID="txtBoardPnt"
                                                            FilterType="UppercaseLetters,LowercaseLetters,Custom" ValidChars=" ">
                                                        </asp:FilteredTextBoxExtender>
                                                        <asp:CheckBox runat="server" ID="cb_IncDayscMess" Text="Include Day Scholar Mess Fees"
                                                            Checked="false" />
                                                        <asp:AutoCompleteExtender ID="AutoCompleteExtender3" runat="server" DelimiterCharacters=""
                                                            Enabled="True" ServiceMethod="getboard" MinimumPrefixLength="0" CompletionInterval="100"
                                                            EnableCaching="false" CompletionSetCount="10" ServicePath="" TargetControlID="txtBoardPnt"
                                                            CompletionListCssClass="autocomplete_completionListElement" CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem"
                                                            CompletionListItemCssClass="panelbackground">
                                                        </asp:AutoCompleteExtender>
                                                        <asp:Label ID="lblHosHostel" runat="server" Text="Hostel" Style="float: left; padding-top: 5px;
                                                            padding-left: 7px;" Visible="false"></asp:Label>
                                                        <asp:DropDownList ID="ddlHosHostel" runat="server" Style="float: left;" CssClass="ddlheight ddlheight2"
                                                            Visible="false" AutoPostBack="True" OnSelectedIndexChanged="ddlHosHostel_IndexChange"
                                                            Width="120px">
                                                        </asp:DropDownList>
                                                        <asp:Label ID="lblHosRoom" runat="server" Text="Room" Style="float: left; padding-top: 5px;
                                                            padding-left: 7px;" Visible="false"></asp:Label>
                                                        <asp:DropDownList ID="ddlHosRoom" runat="server" Style="float: left;" CssClass="ddlheight ddlheight2"
                                                            Visible="false" Width="120px">
                                                        </asp:DropDownList>
                                                    </td>
                                                    <td>
                                                    </td>
                                                    <td>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        Mode
                                                    </td>
                                                    <td colspan="2">
                                                        <asp:DropDownList ID="rblModeDet" runat="server" RepeatDirection="Horizontal">
                                                            <asp:ListItem Value="1" Selected="True">Regular</asp:ListItem>
                                                            <asp:ListItem Value="2">Transfer</asp:ListItem>
                                                            <asp:ListItem Value="3">Lateral</asp:ListItem>
                                                        </asp:DropDownList>
                                                    </td>
                                                    <td>
                                                        Batch :
                                                        <asp:Label ID="lblBatchDet" runat="server"></asp:Label>
                                                    </td>
                                                    <td>
                                                        Current Semester :
                                                        <asp:Label ID="lblCurSemDet" runat="server"></asp:Label>
                                                    </td>
                                                    <td>
                                                        <asp:LinkButton ID="lbRcpt" runat="server" PostBackUrl="~/ChallanReceipt.aspx" Text="Go To Receipt"
                                                            Width="200px"></asp:LinkButton>
                                                    </td>
                                                </tr>
                                            </table>
                                            <div id="photo_div" runat="server" visible="false" class="popupstyle popupheight1"
                                                style="position: fixed; width: 100%; z-index: 1000; height: 100%;">
                                                <center>
                                                    <div id="Div18" runat="server" class="table" style="background-color: White; height: 300px;
                                                        width: 500px; border: 5px solid #0CA6CA; border-top: 25px solid #0CA6CA; margin-top: 110px;
                                                        border-radius: 10px;">
                                                        <br />
                                                        <center>
                                                            <asp:Label ID="Label14" runat="server" Text="Add Student Photo" Style="color: Red;"
                                                                Font-Bold="true" Font-Size="large"></asp:Label>
                                                        </center>
                                                        <center>
                                                            <br />
                                                            <br />
                                                            <table style="margin-left: 75px;">
                                                                <tr>
                                                                    <td>
                                                                        <asp:FileUpload ID="fileuploadbrowse" Style="font-weight: bold; font-family: book antiqua;
                                                                            font-size: medium; background-color: #6699ee; border-radius: 6px;" runat="server" />
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td>
                                                                        <asp:Button ID="btn_photoupload" Text="UpLoad" runat="server" OnClick="btn_photoupload_OnClick"
                                                                            Style="font-weight: bold; font-family: book antiqua; font-size: medium; background-color: #6699ee;
                                                                            border-radius: 6px;" />
                                                                    </td>
                                                                    <td colspan="2">
                                                                        <asp:Button ID="btn_uploadclose" Text="Close" runat="server" OnClick="btn_uploadclose_OnClick"
                                                                            Style="font-weight: bold; font-family: book antiqua; font-size: medium; background-color: #6699ee;
                                                                            border-radius: 6px; margin-left: 0px;" />
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </center>
                                                    </div>
                                                </center>
                                            </div>
                                        </center>
                                    </div>
                                    <br />
                                    <br />
                                    <center>
                                        <%-- <span style="font-size: 18px; font-weight: bold; color: Green;"><u>Student Fee Update</u><span
                                        style="padding-left: 200px;">--%>
                                        <asp:Label ID="lblstudmsg" runat="server" Visible="false" Text=" Student Fee Update"
                                            Style="font-size: 18px; font-weight: bold; color: Green; text-decoration: underline;"></asp:Label>
                                        <asp:Button ID="btnFeeUpdate" runat="server" Text="Update" OnClick="btnFeeUpdate_Click"
                                            Style="height: 25px; width: 70px; border: 1px solid green; border-radius: 5px;
                                            text-decoration: none; font-size: 14px; font-weight: bold;" />
                                        <asp:CheckBox ID="ShowAllCb" runat="server" Visible="false" AutoPostBack="true" OnCheckedChanged="ShowAllCb_CheckedChange"
                                            Text="Show All Ledger" />
                                        <%--</span></span>--%>
                                    </center>
                                    <br />
                                    <div id="divstuddt" runat="server" visible="false" style="width: 490px; height: 450px;
                                        overflow: auto;">
                                        <asp:Label ID="lblAppnoFee" runat="server" Text="" Visible="false"></asp:Label>
                                        <asp:GridView ID="gridFeeDet" runat="server" Width="460px" AutoGenerateColumns="false"
                                            GridLines="Both" OnRowDataBound="gridFeeDet_OnRowDataBound">
                                            <Columns>
                                                <asp:TemplateField HeaderText="S.No" HeaderStyle-BackColor="#0CA6CA" HeaderStyle-HorizontalAlign="Center"
                                                    HeaderStyle-Width="60px">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblAdmFeeSno" runat="server" Text='<%#Container.DisplayIndex+1 %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="Center" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Ledger" HeaderStyle-BackColor="#0CA6CA" HeaderStyle-HorizontalAlign="Center">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblAdmLedger" runat="server" Text='<%#Eval("AdmLedger") %>'></asp:Label>
                                                        <asp:Label ID="lblAdmLedgerId" runat="server" Text='<%#Eval("AdmLedgerId") %>' Visible="false"></asp:Label>
                                                        <asp:Label ID="lblAdmHeaderId" runat="server" Text='<%#Eval("AdmHeaderId") %>' Visible="false"></asp:Label>
                                                        <asp:Label ID="lblAdmPaymode" runat="server" Text='<%#Eval("AdmPaymode") %>' Visible="false"></asp:Label>
                                                        <asp:Label ID="lblAdmDedRes" runat="server" Text='<%#Eval("AdmDedRes") %>' Visible="false"></asp:Label>
                                                        <asp:Label ID="lblAdmFine" runat="server" Text='<%#Eval("AdmFine") %>' Visible="false"></asp:Label>
                                                        <asp:Label ID="lblAdmRefund" runat="server" Text='<%#Eval("AdmRefund") %>' Visible="false"></asp:Label>
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="Left" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Alloted" HeaderStyle-BackColor="#0CA6CA" HeaderStyle-HorizontalAlign="Center"
                                                    HeaderStyle-Width="80px">
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="txtAdmFeeAllot" runat="server" CssClass="  textbox txtheight" Width="80px"
                                                            Text='<%#Eval("FeeAlloted") %>' Height="15px" Style="text-align: right;" onchange="return checktotalamount1();"></asp:TextBox><asp:FilteredTextBoxExtender
                                                                ID="FilteredtxtAdmledge1Amt" runat="server" TargetControlID="txtAdmFeeAllot"
                                                                FilterType="Numbers,custom" ValidChars=".">
                                                            </asp:FilteredTextBoxExtender>
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="Right" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Concession" HeaderStyle-BackColor="#0CA6CA" HeaderStyle-HorizontalAlign="Center"
                                                    HeaderStyle-Width="80px">
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="txtAdmDeduc" runat="server" CssClass="  textbox txtheight" Width="80px"
                                                            Text='<%#Eval("Deduction") %>' Height="15px" Style="text-align: right;" onchange="return checktotalamount1();"></asp:TextBox>
                                                        <asp:FilteredTextBoxExtender ID="FilteredtxtAdmledge2Amt" runat="server" TargetControlID="txtAdmDeduc"
                                                            FilterType="Numbers,custom" ValidChars=".">
                                                        </asp:FilteredTextBoxExtender>
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="Right" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Total" HeaderStyle-BackColor="#0CA6CA" HeaderStyle-HorizontalAlign="Center"
                                                    HeaderStyle-Width="60px">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblAdmFeeTotal" runat="server" Text='<%#Eval("TotalAmt") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="Right" />
                                                </asp:TemplateField>
                                            </Columns>
                                        </asp:GridView>
                                        <br />
                                        <table id="trtotal" runat="server" visible="false">
                                            <tr>
                                                <td>
                                                    <asp:Label ID="lblallot" runat="server" Text="Total Allot"></asp:Label>
                                                </td>
                                                <td>
                                                    <asp:Label ID="lblaltamt" runat="server"></asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <asp:Label ID="lblconse" runat="server" Text="Total Consession"></asp:Label>
                                                </td>
                                                <td>
                                                    <asp:Label ID="lblconsamt" runat="server"></asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <asp:Label ID="lbltot" runat="server" Text="Total Amount"></asp:Label>
                                                </td>
                                                <td>
                                                    <asp:Label ID="lbltotamt" runat="server"></asp:Label>
                                                </td>
                                            </tr>
                                        </table>
                                    </div>
                                </center>
                                <br />
                                <br />
                                <br />
                            </asp:Panel>
                        </div>
                    </asp:Panel>
                </asp:Panel>
                <asp:Panel ID="panel2" runat="server" Visible="false" Style="background: none repeat scroll 0 0 rgba(0, 0, 0, 0.83);
                    border-color: inherit; border-style: none; border-width: 1px; height: 101%; left: -8px;
                    position: absolute; top: -9px; width: 101%;" BorderColor="Blue">
                    <center>
                        <asp:Panel ID="panel9" runat="server" Visible="false" BackColor="mediumaquamarine"
                            Style="border-style: none; border-color: inherit; border-width: 1px; height: 376px;
                            width: 928px; left: 47px; top: 74px; position: absolute;" BorderColor="Blue">
                            <div class="panel6" id="Div3" style="text-align: center; font-family: Book Antiqua;
                                font-size: medium; font-weight: bold">
                                <asp:Button ID="Button10" runat="server" Text="X" ForeColor="Black" OnClick="btnreason_click"
                                    Style="top: 0px; left: 897px; position: absolute; height: 26px; border-width: 0;
                                    background-color: mediumaquamarine; width: 25px;" Font-Bold="True" Font-Names="Microsoft Sans Serif"
                                    Font-Size="Medium" />
                                <table>
                                    <tr>
                                        <td>
                                            <asp:Label ID="Label7" runat="server" Text="Reason For Admission" Font-Bold="True"
                                                Font-Names="Book Antiqua" Font-Size="Medium" Style="top: 80px; position: absolute;
                                                left: 62px;"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:Button ID="btnadd" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                                Font-Size="Medium" Text="+" Width="29px" Style="top: 77px; position: absolute;
                                                left: 225px; height: 27px; display: none;" OnClick="btnadd_Click" />
                                            <asp:DropDownList ID="ddlreason" runat="server" Style="top: 78px; position: absolute;
                                                left: 254px;" Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium" Width="616px">
                                            </asp:DropDownList>
                                            <asp:Button ID="btnminus" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                                Font-Size="Medium" Text="-" Width="29px" Style="top: 77px; position: absolute;
                                                left: 870px; height: 27px; display: none;" OnClick="btnminus_Click" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Button ID="Button11" runat="server" Text="Admit" OnClick="btnadmit_Click" Style="top: 198px;
                                                left: 397px; position: absolute; height: 31px; width: 88px" Font-Bold="True"
                                                Font-Names="Book Antiqua" Font-Size="Medium" margin-left="6px" />
                                        </td>
                                    </tr>
                                </table>
                            </div>
                            <asp:Panel ID="Panel10" runat="server" Visible="false" Style="top: 104px; border-color: Black;
                                background-color: lightyellow; border-style: solid; border-width: 0.5px; left: 9px;
                                position: absolute; width: 912px; height: 75px;">
                                <asp:TextBox ID="txtadd" runat="server" Width="886px" Style="font-family: 'Book Antiqua';
                                    top: 14px; position: absolute; margin-left: 9px" Font-Bold="True" Font-Names="Book Antiqua"
                                    Font-Size="Medium"></asp:TextBox>
                                <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender5" runat="server" TargetControlID="txtadd"
                                    FilterType="LowercaseLetters, UppercaseLetters,Custom" ValidChars="  " />
                                <asp:Button ID="btnadd1" runat="server" Text="Add" OnClick="btnadd1_Click" Style="top: 46px;
                                    left: 330px; position: absolute; height: 26px; width: 88px" Font-Bold="True"
                                    Font-Names="Book Antiqua" Font-Size="Medium" margin-left="6px" />
                                <asp:Button ID="btnexit1" runat="server" Text="Exit" OnClick="btnexit1_Click" Style="top: 46px;
                                    left: 419px; position: absolute; height: 26px; width: 88px" Font-Bold="True"
                                    Font-Names="Book Antiqua" Font-Size="Medium" />
                            </asp:Panel>
                        </asp:Panel>
                        <asp:Panel ID="panel11" runat="server" Visible="false" BackColor="mediumaquamarine"
                            Style="border-style: none; border-color: inherit; border-width: 1px; height: 376px;
                            width: 928px; margin-top: 74px;" BorderColor="Blue">
                            <div class="panel6" id="Div4" style="text-align: center; font-family: Book Antiqua;
                                font-size: medium; font-weight: bold">
                                <asp:Button ID="Button12" runat="server" Text="X" ForeColor="Black" OnClick="btnrjt_click"
                                    Style="margin-left: 897px; height: 26px; border-width: 0; background-color: mediumaquamarine;
                                    width: 25px;" Font-Bold="True" Font-Names="Microsoft Sans Serif" Font-Size="Medium" />
                                <table>
                                    <tr>
                                        <td>
                                            <asp:Label ID="Label62" runat="server" Text="Reason For Reject" Font-Bold="True"
                                                Font-Names="Book Antiqua" Font-Size="Medium" Style=""></asp:Label>
                                        </td>
                                        <td>
                                            <asp:Button ID="Button13" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                                Font-Size="Medium" Text="+" Width="29px" Style="height: 27px; display: none;"
                                                OnClick="btnaddrejt_Click" />
                                        </td>
                                        <td>
                                            <asp:DropDownList ID="btnrejectreason" runat="server" Style="" Font-Bold="True" Font-Names="Book Antiqua"
                                                Font-Size="Medium" Width="616px">
                                            </asp:DropDownList>
                                        </td>
                                        <td>
                                            <asp:Button ID="Button14" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                                Font-Size="Medium" Text="-" Width="29px" Style="height: 27px; display: none;"
                                                OnClick="btnminusrejt_Click" />
                                        </td>
                                    </tr>
                                </table>
                                <table>
                                    <tr>
                                        <td>
                                            <asp:Button ID="Button15" runat="server" Text="Reject" OnClick="btnrct_Click" Style="margin-left: 397px;
                                                height: 31px; width: 88px" Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium" />
                                        </td>
                                    </tr>
                                </table>
                            </div>
                            <asp:Panel ID="Panel12" runat="server" Visible="false" Style="border-color: Black;
                                background-color: lightyellow; border-style: solid; border-width: 0.5px; width: 912px;
                                height: 75px;">
                                <asp:TextBox ID="TextBox1" runat="server" Width="886px" Style="font-family: 'Book Antiqua';
                                    margin-top: 14px; margin-left: 10px;" Font-Bold="True" Font-Names="Book Antiqua"
                                    Font-Size="Medium"></asp:TextBox>
                                <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" runat="server" TargetControlID="txtadd"
                                    FilterType="LowercaseLetters, UppercaseLetters,Custom" ValidChars="  " />
                                <table>
                                    <tr>
                                        <td>
                                            <asp:Button ID="Button16" runat="server" Text="Add" OnClick="btnadd1ret_Click" Style="height: 26px;
                                                width: 88px" Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium" margin-left="6px" />
                                            <asp:Button ID="Button17" runat="server" Text="Exit" OnClick="btnexit1rejt_Click"
                                                Style="height: 26px; width: 88px" Font-Bold="True" Font-Names="Book Antiqua"
                                                Font-Size="Medium" />
                                        </td>
                                    </tr>
                                </table>
                            </asp:Panel>
                        </asp:Panel>
                    </center>
                </asp:Panel>
                <asp:Panel ID="panelpopup" runat="server" Visible="false" Style="background: none repeat scroll 0 0 rgba(0, 0, 0, 0.83);
                    border-color: inherit; border-style: none; border-width: 1px; height: 101%; left: -8px;
                    position: absolute; top: -9px; width: 101%;" BorderColor="Blue">
                    <center>
                        <asp:Panel ID="panelsubpopup" runat="server" BackColor="White" Style="border-style: none;
                            border-color: inherit; border-width: 1px; height: 600px; width: 928px; margin-top: 74px;"
                            BorderColor="Blue">
                            <asp:Button ID="btncancelreport" runat="server" Text="X" ForeColor="Black" OnClick="btncancelreport_click"
                                Style="margin-left: 903px; height: 26px; border-width: 0; background-color: mediumaquamarine;
                                width: 25px;" Font-Bold="True" Font-Names="Microsoft Sans Serif" Font-Size="Medium" />
                            <br />
                            <span id="reportnamesapn" runat="server" visible="false" style="font-size: larger;
                                font-family: Book Antiqua; color: Green;"></span>
                            <br />
                            <FarPoint:FpSpread ID="FpReport" runat="server" BorderColor="Black" BorderStyle="Solid"
                                BorderWidth="1px" HorizontalScrollBarPolicy="Always" VerticalScrollBarPolicy="Always">
                                <Sheets>
                                    <FarPoint:SheetView SheetName="Sheet1" GridLineColor="Black">
                                    </FarPoint:SheetView>
                                </Sheets>
                            </FarPoint:FpSpread>
                            <br />
                            <asp:Label ID="lblreporterror" runat="server" ForeColor="Red" Visible="false"></asp:Label>
                            <asp:Button ID="btn_printreport" runat="server" Text="Print" Font-Bold="True" Visible="false"
                                Font-Names="Book Antiqua" Font-Size="Medium" CssClass="btnapprove1" OnClick="btn_printreport_Click" />
                        </asp:Panel>
                    </center>
                </asp:Panel>
            </div>
        </center>
        <div id="imgdiv2" runat="server" visible="false" style="height: 300em; z-index: 1000;
            width: 100%; background-color: rgba(54, 25, 25, .2); position: absolute; top: 0;
            left: 0px;">
            <center>
                <div id="pnl2" runat="server" class="panel6" style="background-color: white; height: 650px;
                    width: 500px; margin-top: 30px; border: 1px solid #008B8B; border-top: 25px solid #008B8B;
                    border-radius: 10px;">
                    <center>
                        <table style="height: 150px;">
                            <tr align="center">
                                <td>
                                    <asp:Label ID="lbl_alert" runat="server" Text="Seat Type" Font-Bold="true" Font-Size="Large"></asp:Label>
                                    <asp:DropDownList ID="ddl_seattype" runat="server" CssClass="ddlheight3 textbox textbox1"
                                        AutoPostBack="true" OnSelectedIndexChanged="ddl_seattype_IndexChange">
                                    </asp:DropDownList>
                                </td>
                                <td align="center">
                                    <asp:Button ID="btn_errorclose" CssClass=" textbox btn1 comm" Style="height: 28px;
                                        width: 65px;" OnClick="btn_errorclose_Click" Text="ok" runat="server" Font-Bold="true" />
                                    <asp:Button ID="btn_popclose" CssClass=" textbox btn1 comm" Style="height: 28px;
                                        width: 65px;" OnClick="btn_popclose_Click" Text="Cancel" runat="server" Font-Bold="true" />
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2" id="tdconces" runat="server" visible="false">
                                    <table>
                                        <tr>
                                            <td>
                                                <asp:Label ID="lblcons" runat="server" Text="Concession" Font-Bold="true" Font-Size="Large"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:DropDownList ID="ddlconces" runat="server" CssClass="ddlheight3 textbox textbox1">
                                                </asp:DropDownList>
                                            </td>
                                            <td>
                                                <asp:RadioButtonList ID="rbtype" runat="server" RepeatDirection="Horizontal">
                                                    <asp:ListItem Text="Amount" Value="0" Selected="True"></asp:ListItem>
                                                    <asp:ListItem Text="Percentage" Value="1"></asp:ListItem>
                                                </asp:RadioButtonList>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                                <%--AutoPostBack="true" OnSelectedIndexChanged="ddl_seattype_IndexChange"--%>
                            </tr>
                            <tr>
                                <td colspan="2">
                                    <center>
                                        <asp:Label ID="txt_gridAdmLedgeTot" runat="server" Visible="false" ForeColor="Red"
                                            Font-Bold="true" Font-Size="Medium"></asp:Label>
                                    </center>
                                    <div style="width: 490px; height: 450px; overflow: auto;">
                                        <asp:GridView ID="gridAdmLedge" runat="server" Width="460px" AutoGenerateColumns="false"
                                            GridLines="Both" OnRowDataBound="gridAdmLedge_OnRowDataBound">
                                            <Columns>
                                                <asp:TemplateField HeaderText="Ledger" HeaderStyle-BackColor="#0CA6CA" HeaderStyle-HorizontalAlign="Center">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblAdmLedger" runat="server" Text='<%#Eval("AdmLedger") %>'></asp:Label>
                                                        <asp:Label ID="lblAdmLedgerId" runat="server" Text='<%#Eval("AdmLedgerId") %>' Visible="false"></asp:Label>
                                                        <asp:Label ID="lblAdmHeaderId" runat="server" Text='<%#Eval("AdmHeaderId") %>' Visible="false"></asp:Label>
                                                        <asp:Label ID="lblAdmPaymode" runat="server" Text='<%#Eval("AdmPaymode") %>' Visible="false"></asp:Label>
                                                        <asp:Label ID="lblAdmDedRes" runat="server" Text='<%#Eval("AdmDedRes") %>' Visible="false"></asp:Label>
                                                        <asp:Label ID="lblAdmFine" runat="server" Text='<%#Eval("AdmFine") %>' Visible="false"></asp:Label>
                                                        <asp:Label ID="lblAdmRefund" runat="server" Text='<%#Eval("AdmRefund") %>' Visible="false"></asp:Label>
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="Left" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Alloted" HeaderStyle-BackColor="#0CA6CA" HeaderStyle-HorizontalAlign="Center"
                                                    HeaderStyle-Width="80px">
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="txtAdmFeeAllot" runat="server" CssClass="  textbox txtheight" Width="80px"
                                                            Text='<%#Eval("FeeAlloted") %>' Height="15px" Style="text-align: right;" onchange="return checktotalamount();"></asp:TextBox><asp:FilteredTextBoxExtender
                                                                ID="FilteredtxtAdmledge1Amt" runat="server" TargetControlID="txtAdmFeeAllot"
                                                                FilterType="Numbers,custom" ValidChars=".">
                                                            </asp:FilteredTextBoxExtender>
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="Right" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Concession" HeaderStyle-BackColor="#0CA6CA" HeaderStyle-HorizontalAlign="Center"
                                                    HeaderStyle-Width="80px">
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="txtAdmDeduc" runat="server" CssClass="  textbox txtheight" Width="80px"
                                                            Text='<%#Eval("Deduction") %>' Height="15px" Style="text-align: right;" onchange="return checktotalamount();"></asp:TextBox>
                                                        <asp:FilteredTextBoxExtender ID="FilteredtxtAdmledge2Amt" runat="server" TargetControlID="txtAdmDeduc"
                                                            FilterType="Numbers,custom" ValidChars=".">
                                                        </asp:FilteredTextBoxExtender>
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="Right" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Total" HeaderStyle-BackColor="#0CA6CA" HeaderStyle-HorizontalAlign="Center"
                                                    HeaderStyle-Width="60px">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblAdmFeeTotal" runat="server" Text='<%#Eval("TotalAmt") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="Right" />
                                                </asp:TemplateField>
                                            </Columns>
                                        </asp:GridView>
                                    </div>
                                </td>
                            </tr>
                            <tr style="display: none;">
                                <td>
                                    <asp:CheckBox ID="cbAdmLedgeFee" runat="server" Text="Use Ledger Amount" AutoPostBack="true"
                                        OnCheckedChanged="cbAdmLedgeFee_Change" Checked="false" />
                                </td>
                                <td>
                                    <asp:Button ID="btnfeesave" runat="server" Text="Save" Width="44px" Style="border: 1px solid indigo;"
                                        OnClick="btnfeesave_Click" />
                                </td>
                            </tr>
                            <tr style="display: none;">
                                <td colspan="2">
                                    Ledger1
                                    <asp:DropDownList ID="ddlAdmLedge1" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlAdmLedge1_IndexChanged"
                                        CssClass="ddlheight3 textbox textbox1" Width="120px">
                                    </asp:DropDownList>
                                    Amount
                                    <asp:TextBox ID="txtAdmledge1Amt" runat="server" CssClass="txtheight textbox textbox1"
                                        Width="70px"></asp:TextBox>
                                    <asp:FilteredTextBoxExtender ID="FilteredtxtAdmledge1Amt" runat="server" TargetControlID="txtAdmledge1Amt"
                                        FilterType="Numbers,custom" ValidChars=".">
                                    </asp:FilteredTextBoxExtender>
                                </td>
                            </tr>
                            <tr style="display: none;">
                                <td colspan="2">
                                    Ledger2
                                    <asp:DropDownList ID="ddlAdmLedge2" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlAdmLedge2_IndexChanged"
                                        CssClass="ddlheight3 textbox textbox1" Width="120px">
                                    </asp:DropDownList>
                                    Amount
                                    <asp:TextBox ID="txtAdmledge2Amt" runat="server" CssClass="txtheight textbox textbox1"
                                        Width="70px"></asp:TextBox>
                                    <asp:FilteredTextBoxExtender ID="FilteredtxtAdmledge2Amt" runat="server" TargetControlID="txtAdmledge2Amt"
                                        FilterType="Numbers,custom" ValidChars=".">
                                    </asp:FilteredTextBoxExtender>
                                </td>
                            </tr>
                        </table>
                    </center>
                </div>
            </center>
        </div>
        <div id="popSendSms" runat="server" style="height: 100em; z-index: 1000; width: 100%;
            position: absolute; top: 0; left: 0;" visible="false">
            <center>
                <div style="background-color: #FFFFFF; height: 300px; margin-top: 180px; width: 500px;">
                    <br />
                    <table>
                        <tr>
                            <td style="color: Green; text-align: center; font-size: 20px; font-weight: bold;">
                                Send SMS
                            </td>
                        </tr>
                        <tr>
                            <td>
                                Type your message here :
                                <br />
                                <asp:TextBox ID="txt_SmsMsgPop" runat="server" Width="400px" TextMode="MultiLine"
                                    Rows="10" Placeholder="New Message"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Button ID="btnSendSmsPop" runat="server" Style="border: 1px solid orange; border-radius: 5px;
                                    width: 150px; height: 30px;" Text="Send SMS" OnClick="btnSendSmsPop_click" />
                                <asp:Button ID="btnClosePop" runat="server" Style="border: 1px solid orange; border-radius: 5px;
                                    width: 60px; height: 30px;" Text="Exit" OnClick="btnClosePop_click" />
                            </td>
                        </tr>
                    </table>
                </div>
            </center>
        </div>
        <div id="poperrjs" runat="server" style="height: 100em; z-index: 10000; width: 100%;
            position: absolute; top: 0; left: 0;">
            <center>
                <div style="background-color: #FFFFFF; height: 200px; margin-top: 180px; width: 300px;
                    border: 1px solid rgb(0, 128, 128); border-radius: 5px; border-top-width: 5px;">
                    <br />
                    <br />
                    <span id="errorspan" runat="server"></span>
                    <br />
                    <br />
                    <br />
                    <br />
                    <asp:Button ID="popupbtn" runat="server" Style="background-color: rgb(0, 128, 128);
                        border: 0px; color: White;" Text="Okay" OnClick="btnpopup_clcik" />
                    <%-- <button style="background-color: rgb(0, 128, 128); border: 0px; color: White;">
                    Okay</button>--%>
                </div>
            </center>
        </div>
        <div id="Div2" runat="server" visible="false" style="height: 100em; z-index: 1000;
            width: 100%; position: absolute; top: 0; left: 0;">
            <center>
                <div style="background-color: #FFFFFF; height: 260px; margin-top: 180px; width: 350px;">
                    <br />
                    <table>
                        <tr>
                            <td>
                                <asp:Label ID="Label10" runat="server" Text="Date of Preparation" Font-Bold="True"
                                    Font-Names="Book Antiqua" Font-Size="Medium"></asp:Label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtPrepDate" runat="server" Width="90px" Font-Bold="True" Font-Names="Book Antiqua"
                                    Font-Size="Medium"></asp:TextBox>
                                <asp:CalendarExtender ID="CalendarExtender4" runat="server" Format="dd/MM/yyyy" TargetControlID="txtPrepDate" />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="Label6" runat="server" Text="Interview Date" Font-Bold="True" Font-Names="Book Antiqua"
                                    Font-Size="Medium"></asp:Label>
                            </td>
                            <td>
                                <asp:TextBox ID="Txt_callDate" runat="server" Width="90px" Font-Bold="True" Font-Names="Book Antiqua"
                                    Font-Size="Medium"></asp:TextBox>
                                <asp:CalendarExtender ID="CalendarExtender3" runat="server" Format="dd/MM/yyyy" TargetControlID="Txt_callDate" />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="Label13" runat="server" Text="Interview Time" Font-Bold="True" Font-Names="Book Antiqua"
                                    Font-Size="Medium"></asp:Label>
                            </td>
                            <td>
                                <asp:DropDownList ID="ddlIntHr" runat="server" Width="40px">
                                    <asp:ListItem Value="1" Selected="True">01</asp:ListItem>
                                    <asp:ListItem Value="2">02</asp:ListItem>
                                    <asp:ListItem Value="3">03</asp:ListItem>
                                    <asp:ListItem Value="4">04</asp:ListItem>
                                    <asp:ListItem Value="5">05</asp:ListItem>
                                    <asp:ListItem Value="6">06</asp:ListItem>
                                    <asp:ListItem Value="7">07</asp:ListItem>
                                    <asp:ListItem Value="8">08</asp:ListItem>
                                    <asp:ListItem Value="9">09</asp:ListItem>
                                    <asp:ListItem Value="10">10</asp:ListItem>
                                    <asp:ListItem Value="11">11</asp:ListItem>
                                    <asp:ListItem Value="12">12</asp:ListItem>
                                </asp:DropDownList>
                                <asp:DropDownList ID="ddlIntMin" runat="server" Width="40px">
                                </asp:DropDownList>
                                <asp:DropDownList ID="ddlIntMed" runat="server" Width="40px">
                                    <asp:ListItem Value="AM" Selected="true">AM</asp:ListItem>
                                    <asp:ListItem Value="PM">PM</asp:ListItem>
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="Label11" runat="server" Text="Venue" Font-Bold="True" Font-Names="Book Antiqua"
                                    Font-Size="Medium"></asp:Label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtVenue" runat="server" Width="120px" Font-Bold="True" Font-Names="Book Antiqua"
                                    Font-Size="Medium"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="Label12" runat="server" Text="DD Amount" Font-Bold="True" Font-Names="Book Antiqua"
                                    Font-Size="Medium"></asp:Label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtddAmount" runat="server" Width="120px" Font-Bold="True" Font-Names="Book Antiqua"
                                    Font-Size="Medium" MaxLength="10"></asp:TextBox>
                                <asp:FilteredTextBoxExtender ID="ftTxt" runat="server" FilterType="Numbers,Custom"
                                    ValidChars="." TargetControlID="txtddAmount">
                                </asp:FilteredTextBoxExtender>
                            </td>
                        </tr>
                    </table>
                    <br />
                    <asp:Button ID="Button7" runat="server" Style="background-color: rgb(0, 128, 128);
                        border: 0px; color: White;" Text="Okay" OnClick="Okay_clcik" OnClientClick="return InterviewOk(this);" />
                    <asp:Button ID="Button8" runat="server" Style="background-color: rgb(0, 128, 128);
                        border: 0px; color: White;" Text="Cancel" OnClick="Cancel_clcik" />
                    <%-- <button style="background-color: rgb(0, 128, 128); border: 0px; color: White;">
                    Okay</button>--%>
                </div>
            </center>
        </div>
        <Insproplus:printmaster runat="server" ID="Printcontrol" Visible="false" />
        <%-- New Print div--%>
        <div style="height: 1px; width: 1px; overflow: auto;">
            <div id="contentDiv" runat="server" style="height: 710px; width: 1344px;" visible="false">
            </div>
        </div>
    </body>
    </html>
</asp:Content>

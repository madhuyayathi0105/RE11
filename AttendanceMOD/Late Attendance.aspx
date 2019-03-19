<%@ Page Title="" Language="C#" MasterPageFile="~/AttendanceMOD/AttendanceSubSiteMaster.master" AutoEventWireup="true" CodeFile="Late Attendance.aspx.cs" Inherits="LateAttendance" %>

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
     <script type="text/javascript">
            function date() {
                var dateObj = new Date();
                var week = dateObj.format("dddd/MMMM/yyyy h:mm:ss tt");
                document.getElementById('<%=lbltime.ClientID %>').innerHTML = week.toString();
                setTimeout('date()', 500);
                return "";
            }
            function enterkeyvoid(e) {
                if (e.keycode == 13 || e.which == 13) {
                    // void enter key
                    return false;
                }
            }
            function getsmartrollno(txt1) {
                var dne = txt1.value;
                $.ajax({
                    type: "POST",
                    url: "Late Attendance.aspx/studroll",
                    data: '{Smart_No: "' + dne + '"}',
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success: function (responsesm) {
                        bindss(responsesm.d);
                    },
                    failure: function (responsesm) {
                        alert(responsesm);
                    }
                });

            }
            function getsmartno(txt1) {
                var dne = txt1.value;
                var smartcardno_len = dne.length;
                if (smartcardno_len >= 10) {
                    $.ajax({
                        type: "POST",
                        url: "Late Attendance.aspx/studsmartno",
                        data: '{Smart_No: "' + dne + '"}',
                        contentType: "application/json; charset=utf-8",
                        dataType: "json",
                        success: function (responsesm) {
                            bindss(responsesm.d);
                        },
                        failure: function (responsesm) {
                            alert(responsesm);
                        }
                    });

                }
            }

            function bindss(Employees) {
                    var statusmsg1 = Employees[5].toString();
                    var rno = Employees[0].toString();
                  
                    if (statusmsg1 == "1") {
                        document.getElementById('<%=txt_roll_no.ClientID %>').value = rno;
                        document.getElementById('<%=txt_studentname.ClientID %>').value = Employees[1].toString();
                        document.getElementById('<%=Txtreg.ClientID %>').value = Employees[7].toString();
                        document.getElementById('<%=txt_studenttype.ClientID %>').value = Employees[2].toString();
                        document.getElementById('<%=txt_degree.ClientID %>').value = Employees[3].toString();
                        document.getElementById('<%=lbl_studimage.ClientID %>').src = Employees[4].toString();
                        //Mark msg
                        var attn_count = Employees[8].toString();
                        document.getElementById('<%=lblcon.ClientID %>').innerHTML = attn_count;
                        document.getElementById('<%=lblcon.ClientID %>').style.color = "Lawngreen";
                        var mesg = document.getElementById('<%=lbl_att.ClientID %>');
                        mesg.style.color = "green";
                        mesg.innerHTML = "Late Entered";
                        //Lable text
                        var type = document.getElementById("<%=ddlrollno.ClientID%>").value;
                      
                        if (type == "0") {
                            document.getElementById('<%=txt_smartno.ClientID %>').value = "";
                            document.getElementById('<%=txt_smartno.ClientID %>').focus();
                        }
                        else {
                            document.getElementById('<%=txt_rollno.ClientID %>').value = "";
                            document.getElementById('<%=txt_rollno.ClientID %>').focus();
                           
                        }
                        document.getElementById('<%=txt_rollno.ClientID %>').value = "";
                        var textbox = document.getElementById('<%=txt_rollno.ClientID %>');
                        textbox.focus();
                        textbox.scrollIntoView();
                    } else {
                        document.getElementById('<%=txt_roll_no.ClientID %>').value = rno;
                        document.getElementById('<%=txt_studentname.ClientID %>').value = Employees[1].toString();
                        document.getElementById('<%=txt_studenttype.ClientID %>').value = Employees[2].toString();
                        document.getElementById('<%=txt_degree.ClientID %>').value = Employees[3].toString();
                        document.getElementById('<%=lbl_studimage.ClientID %>').src = Employees[4].toString();
                        document.getElementById('<%=Txtreg.ClientID %>').value = Employees[7].toString();
                        var attn_count = Employees[8].toString();
                        document.getElementById('<%=lblcon.ClientID %>').innerHTML = attn_count;
                        document.getElementById('<%=lblcon.ClientID %>').style.color = "Lawngreen";
                        var mesg = document.getElementById('<%=lbl_att.ClientID %>');
                        var statusmsg1 = Employees[5].toString();
                        if (statusmsg1 == "0") {
                            mesg.style.color = "red";
                            mesg.innerHTML = "Please Check Student Details";

                            var type = document.getElementById("<%=ddlrollno.ClientID%>").value;
                            alert(type);
                            if (type == "0") {
                                document.getElementById('<%=txt_smartno.ClientID %>').value = "";
                                var textbox = document.getElementById('<%=txt_smartno.ClientID %>');
                                textbox.focus();
                                textbox.scrollIntoView();
                            }
                            else {
                                document.getElementById('<%=txt_rollno.ClientID %>').value = "";
                                var textbox = document.getElementById('<%=txt_rollno.ClientID %>');
                                textbox.focus();
                                textbox.scrollIntoView();
                            }
                            alert('Please Check Student Details');
                        }
                        if (statusmsg1 == "2") {
                            mesg.style.color = "brown";
                            mesg.innerHTML = "Late AlreadyMarked";
                            var type = document.getElementById("<%=ddlrollno.ClientID%>").value;
                            if (type == "0") {
                                document.getElementById('<%=txt_smartno.ClientID %>').value = "";
                                var textbox = document.getElementById('<%=txt_smartno.ClientID %>');
                                textbox.focus();
                                textbox.scrollIntoView();
                            }
                            else {
                                document.getElementById('<%=txt_rollno.ClientID %>').value = "";
                                var textbox = document.getElementById('<%=txt_rollno.ClientID %>');
                                textbox.focus();
                                textbox.scrollIntoView();
                            }
                            //alert('Attendance AlreadyMarked');
                        }
                    }
                    
                   
                }
                
            
               </script>
    <form id="form1">
        <asp:ScriptManager ID="ScriptManager1" runat="server">
        </asp:ScriptManager>
        <center>
            <span style="color: #008000" class="fontstyleheader">Late Entry</span>
            <div style="margin-right: 853px; margin-top: 0px;">
                <asp:Label ID="lbltime" runat="server" ForeColor="#336699" Width="280px"><script>                                                                                             document.write(date())</script></asp:Label>

                </div>
                  <br />
        </center>
        <center>
            <div class="maindivstyle maindivstylesize">
                <br />
                <center>
                    <table class="maintablestyle">
                        <tr>
                            <td>
                                <asp:Label ID="lbl_College" Text="College Name" runat="server"></asp:Label>
                            </td>
                            <td>
                             <asp:UpdatePanel ID="updPnlTmr" runat="server">
                <ContentTemplate>
                                <asp:DropDownList ID="ddl_College" runat="server" Visible="true" AutoPostBack="True"
                                    CssClass="textbox1 ddlheight4" >
                                </asp:DropDownList><%--OnSelectedIndexChanged="ddl_College_SelectedIndexChanged"--%>
                                </ContentTemplate></asp:UpdatePanel>
                            </td>
                             <td>
                             <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                <ContentTemplate>
                                <asp:DropDownList ID="ddlrollno" runat="server" CssClass="textbox1 ddlheight"
                                    Width="100px" AutoPostBack="true" OnSelectedIndexChanged="ddlrollno_selectedindexchanged">
                                    <asp:ListItem Value="0">Smart Card</asp:ListItem>
                                    <asp:ListItem Value="1">Roll No</asp:ListItem>
                                    <asp:ListItem Value="2">Reg No</asp:ListItem>
                                    <asp:ListItem Value="3">Student Name</asp:ListItem>
                               
                                 
                                </asp:DropDownList></ContentTemplate></asp:UpdatePanel>
                            </td>
                               <td>
                               <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                <ContentTemplate>
                                <asp:TextBox ID="txt_smartno" Visible="true" runat="server" CssClass="textbox textbox1"
                                    Width="110px" onkeyup="return getsmartno(this)" TextMode="Password" onkeypress="return enterkeyvoid(event)"
                                    placeholder="Smartcard No"></asp:TextBox><%--OnTextChanged="rollNo_Oncheckedchange" AutoPostBack="true" onkeyup--%>
                                <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" runat="server" TargetControlID="txt_rollno"
                                    FilterType="UppercaseLetters,LowercaseLetters,Numbers,custom" ValidChars=" .">
                                </asp:FilteredTextBoxExtender>
                                <asp:TextBox ID="txt_rollno" runat="server" CssClass="textbox textbox1" Width="110px" Visible="false"
                                    onchange="return getsmartrollno(this)" ></asp:TextBox><%--OnTextChanged="rollNo_Oncheckedchange" AutoPostBack="true" onkeyup--%>
                                <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender2" runat="server" TargetControlID="txt_rollno"
                                        FilterType="UppercaseLetters,LowercaseLetters,numbers,custom" ValidChars=" -,.">
                                    </asp:FilteredTextBoxExtender>
                                    <asp:AutoCompleteExtender ID="autocomplete_rollno" runat="server" DelimiterCharacters=""
                                        Enabled="True" ServiceMethod="Getrno" MinimumPrefixLength="0" CompletionInterval="100"
                                        EnableCaching="false" CompletionSetCount="10" ServicePath="" TargetControlID="txt_rollno"
                                        CompletionListCssClass="autocomplete_completionListElement" CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem"
                                        CompletionListItemCssClass="panelbackground">
                                    </asp:AutoCompleteExtender></ContentTemplate></asp:UpdatePanel>
                                </td>
                                 </tr>
                    </table>
                    <br />
                                 <center>
                        <div>
                            <asp:Label ID="lbl_error" runat="server" Visible="false" ForeColor="Red"></asp:Label>
                        </div>
                    </center>
                </center>
                <br />
                <center>
                    <asp:Panel ID="Panel1" runat="server" BorderStyle="Solid" Style="height: auto; width: 766px;"
                        CssClass="maindivstyle">
                        <%--BorderColor="#993333"--%>
                        <br />
                        <table>
                            <tr>
                                <td>
                                    Name
                                </td>
                                <td>
                                    <asp:TextBox ID="txt_studentname" runat="server" CssClass="textbox  textbox1 txtheight5"></asp:TextBox>
                                </td>
                                <td rowspan="4">
                                    <asp:Image ID="lbl_studimage" runat="server" Width="130px" Height="120px" />
                                </td>
                                <td></td>
                                 <td rowspan="4">
                                     <asp:Label ID="lblcon" runat="server" Style="color: Red;" Font-Size="135px" Font-Bold="true"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                           
                                <td >
                                    <asp:Label ID="lbl_roll" runat="server">Roll No</asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txt_roll_no" runat="server" CssClass="textbox  textbox1 txtheight3"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                              <td >
                                    <asp:Label ID="labreg" runat="server">Reg No</asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox ID="Txtreg" runat="server" CssClass="textbox  textbox1 txtheight3"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="lbl_type" runat="server">Student Type</asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txt_studenttype" runat="server" CssClass="textbox  textbox1 txtheight3"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="lbl_desig" runat="server"> Degree</asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txt_degree" runat="server" CssClass="textbox  textbox1 txtheight5"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                   
                                </td>
                                <td>
                                    <asp:Label ID="lbl_att" runat="server" Style="color: Red;" Font-Size="Large" Font-Bold="true"></asp:Label>
                                </td>
                            </tr>
                        </table>
                        <br />
                    </asp:Panel>
                </center>
                </form>
                </body>
                </html>
</asp:Content>


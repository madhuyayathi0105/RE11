<%@ Page Title="" Language="C#" MasterPageFile="~/AdmissionMod/AttendanceSubSiteMaster.master"
    AutoEventWireup="true" CodeFile="DateAndTimeSlotSettings.aspx.cs" Inherits="DateAndTimeSlotSettings" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">
    <script type="text/javascript">

        function OnSaveDaySlotCheck() {

            var id = document.getElementById("<%=gridDaySlots.ClientID %>");
            var len = id.rows.length;
            var indx = 0;
            for (var sl = 1; sl < len; sl++, indx++) {
                var ischecked = document.getElementById('MainContent_gridDaySlots_chkSel_' + indx.toString()).checked;
                if (ischecked == true) {
                    return true;
                }
            }
            alert("Please select any slot");
            return false;
        }

        $(document).ready(function () {
            $('#<%=btnDaySlotDelete.ClientID %>').click(function () {
                var gridID = document.getElementById("<%=gridDaySlots.ClientID %>");
                var gridLen = gridID.rows.length - 1;
                var gridIndex = 0;
                var totChecked = 0;
                for (var gdrow = 0; gdrow < gridLen; gdrow++, gridIndex++) {
                    var isChecked = document.getElementById('MainContent_gridDaySlots_chkSel_' + gridIndex.toString()).checked;
                    if (isChecked == true)
                        totChecked++;
                }
                if (totChecked == 0) {
                    alert("Please Select Any slots!");
                    return false;
                }
            });
        });

        function checkDate() {
            var fromDate = "";
            var toDate = "";
            var date = ""
            var date1 = ""
            var month = "";
            var month1 = "";
            var year = "";
            var year1 = "";
            var empty = "";
            fromDate = document.getElementById('<%=txt_fromSlotSet.ClientID%>').value;
            toDate = document.getElementById('<%=txt_toSlotSet.ClientID%>').value;

            date = fromDate.substring(0, 2);
            month = fromDate.substring(3, 5);
            year = fromDate.substring(6, 10);

            date1 = toDate.substring(0, 2);
            month1 = toDate.substring(3, 5);
            year1 = toDate.substring(6, 10);
            var today = new Date();
            //  var currentDate = today.getDate() + '/' + (today.getMonth() + 1) + '/' + today.getFullYear();
            var today = new Date();
            var dd = today.getDate();
            var mm = today.getMonth() + 1;
            var yyyy = today.getFullYear();
            if (dd < 10) { dd = '0' + dd }
            if (mm < 10) { mm = '0' + mm }
            var today = dd + '/' + mm + '/' + yyyy;

            if (year == year1) {
                if (month == month1) {
                    if (date == date1) {
                        empty = "";
                    }
                    else if (date < date1) {
                        empty = "";
                    }
                    else {
                        empty = "e";
                    }
                }
                else if (month < month1) {
                    empty = "";
                }
                else if (month > month1) {
                    empty = "e";
                }
            }
            else if (year < year1) {
                empty = "";
            }
            else if (year > year1) {
                empty = "e";
            }
            if (empty != "") {
                document.getElementById('<%=txt_fromSlotSet.ClientID%>').value = today;
                document.getElementById('<%=txt_toSlotSet.ClientID%>').value = today;
                alert("From Date Should Not Exceed To Date");
                return false;
            }
        }

        var checkedId = false;
        function OnGridHeaderSelected() {
            var id = document.getElementById("<%=gridDaySlots.ClientID %>");
            var len = id.rows.length;
            var i = 0;
            var checkedId = id.rows[0].getElementsByTagName("input")[0].checked;
            for (var ak = 1; ak < len; ak++) {
                if (id.rows[ak].getElementsByTagName("input")[i].type == "checkbox") {
                    if (checkedId == true) {
                        id.rows[ak].getElementsByTagName("input")[i].checked = true;
                    } else {
                        id.rows[ak].getElementsByTagName("input")[i].checked = false;
                    }
                }
            }
        }

        function OnSaveSlotCheck() {

            var id = document.getElementById("<%=gridSlots.ClientID %>");
            var len = id.rows.length;

            var indx = 0;
            for (var sl = 1; sl < len; sl++, indx++) {
                var fromhrs = document.getElementById('MainContent_gridSlots_ddlSlotFromHrs_' + indx.toString()).value;
                var frommins = document.getElementById('MainContent_gridSlots_ddlSlotFromMin_' + indx.toString()).value;
                var tohrs = document.getElementById('MainContent_gridSlots_ddlSlotToHrs_' + indx.toString()).value;
                var tomins = document.getElementById('MainContent_gridSlots_ddlSlotToMin_' + indx.toString()).value;

                var fromtime = parseInt((fromhrs + frommins));
                var totime = parseInt((tohrs + tomins));
                if (totime <= fromtime) {
                    alert("Please check From and To Time");
                    return false;
                }

            }
            return true;
        }

        function checkNoofslots() {
            var id = document.getElementById("<%=txtNoOfSlot.ClientID %>");
            if (id.value.trim() != "" && id.value.trim() != "0" && id.value.trim() != "00") {
                return true;
            }

            alert('Please enter number of slots');
            return false;
        }
    </script>
    <center>
        <div class="maindivstyle" style="width: 950px;">
            <center>
                <span class="fontstyleheader" style="color: Green;">Date and Time Slot Settings</span>
            </center>
            <asp:ScriptManager ID="scptMgrNew" runat="server">
            </asp:ScriptManager>
            <table class="maintablestyle">
                <tr>
                    <td>
                        <asp:Label ID="lblCollege" runat="server" Text="Institute"></asp:Label>
                    </td>
                    <td>
                        <asp:DropDownList ID="ddlCollege" runat="server" CssClass="textbox ddlheight5" AutoPostBack="true"
                            OnSelectedIndexChanged="ddlCollege_SelectedIndexChanged">
                        </asp:DropDownList>
                    </td>
                    <td>
                        <asp:Label ID="lblBatch" runat="server" Text="Batch"></asp:Label>
                    </td>
                    <td>
                        <asp:DropDownList ID="ddlbatch" runat="server" CssClass="textbox ddlheight" AutoPostBack="true"
                            OnSelectedIndexChanged="ddlbatch_SelectedIndexChanged">
                        </asp:DropDownList>
                    </td>
                    <td>
                        <asp:Label ID="lblEdulevel" runat="server" Text="Edu Level"></asp:Label>
                    </td>
                    <td>
                        <asp:DropDownList ID="ddlEduLev" runat="server" CssClass="textbox ddlheight" AutoPostBack="true"
                            OnSelectedIndexChanged="ddlEdulevel_SelectedIndexChanged">
                        </asp:DropDownList>
                    </td>
                    <td>
                        <asp:Label ID="lblCourse" runat="server" Text="Course"></asp:Label>
                    </td>
                    <td>
                        <asp:DropDownList ID="ddlcourse" runat="server" CssClass="textbox ddlheight" AutoPostBack="true"
                            OnSelectedIndexChanged="ddlcourse_OnSelectedIndexChanged">
                        </asp:DropDownList>
                    </td>
                    <td>
                        <asp:Button ID="btnAddSlot" runat="server" CssClass="textbox btn" Width="100px" Text="Slot Settings"
                            OnClick="btnAddSlot_Click" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="lblFromSlotSet" runat="server" Text="From"></asp:Label>
                    </td>
                    <td>
                        <asp:TextBox ID="txt_fromSlotSet" runat="server" CssClass="textbox txtheight" Height="15px"
                            onchange="return checkDate()"></asp:TextBox>
                        <%--OnTextChanged="checkDate" AutoPostBack="true"--%>
                        <asp:CalendarExtender ID="CalendarExtender1" TargetControlID="txt_fromSlotSet" runat="server"
                            CssClass="cal_Theme1 ajax__calendar_active" Format="dd/MM/yyyy">
                        </asp:CalendarExtender>
                        <asp:Label ID="lbl_toSlotSet" runat="server" Text="To"></asp:Label>
                        <asp:TextBox ID="txt_toSlotSet" runat="server" CssClass="textbox txtheight" Height="15px"
                            onchange="return checkDate()"></asp:TextBox>
                        <%--OnTextChanged="checkDate" AutoPostBack="true"--%>
                        <asp:CalendarExtender ID="CalendarExtender2" TargetControlID="txt_toSlotSet" runat="server"
                            CssClass="cal_Theme1 ajax__calendar_active" Format="dd/MM/yyyy">
                        </asp:CalendarExtender>
                    </td>
                    <td>
                        Slot
                    </td>
                    <td>
                        <asp:UpdatePanel ID="updSlot" runat="server">
                            <ContentTemplate>
                                <asp:TextBox ID="txt_Slot" runat="server" CssClass="textbox txtheight2" ReadOnly="true"
                                    placeholder="Slot"></asp:TextBox>
                                <asp:Panel ID="panel_slot" runat="server" CssClass="multxtpanel">
                                    <asp:CheckBox ID="cb_slot" runat="server" Width="100px" Text="Select All" AutoPostBack="True"
                                        OnCheckedChanged="cb_slot_CheckedChanged" />
                                    <asp:CheckBoxList ID="cbl_slot" runat="server" AutoPostBack="True" OnSelectedIndexChanged="cbl_slot_SelectedIndexChanged">
                                    </asp:CheckBoxList>
                                </asp:Panel>
                                <asp:PopupControlExtender ID="popupce_slot" runat="server" TargetControlID="txt_Slot"
                                    PopupControlID="panel_slot" Position="Bottom">
                                </asp:PopupControlExtender>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </td>
                    <td>
                        <asp:Button ID="btnBaseGo" runat="server" Text="Go" CssClass="textbox  btn2" Width="40px"
                            OnClick="btnBaseGo_OnClick" />
                    </td>
                    <td>
                        <asp:Button ID="btnDaySlotSave" runat="server" Text="Save" CssClass="textbox  btn2"
                            Width="60px" BackColor="#81C13F" ForeColor="White" OnClientClick="return OnSaveDaySlotCheck()"
                            Visible="false" OnClick="btnDaySlotSave_OnClick" />
                    </td>
                    <td>
                        <asp:Button ID="btnDaySlotShow" runat="server" Text="Show" CssClass="textbox  btn2"
                            Width="60px" BackColor="#adff2f" ForeColor="Black" Visible="true" OnClick="btnDaySlotShow_OnClick" />
                        <%--OnClientClick="return OnSaveDaySlotCheck()"--%>
                    </td>
                    <td>
                        <asp:Button ID="btnDaySlotDelete" runat="server" Text="Delete" CssClass="textbox  btn2"
                            Width="60px" BackColor="#ff0000" ForeColor="White" Visible="false" OnClick="btnDaySlotDelete_OnClick" />
                        <%--OnClientClick="return OnSaveDaySlotCheck()"--%>
                    </td>
                </tr>
            </table>
            <br />
            <asp:GridView ID="gridDaySlots" runat="server" AutoGenerateColumns="false" HeaderStyle-BackColor="#0CA6CA"
                OnDataBound="gridDaySlots_DataBound" OnRowDataBound="gridDaySlots_OnRowDataBound"
                Width="500px" Visible="false">
                <Columns>
                    <asp:TemplateField HeaderText="S.No">
                        <ItemTemplate>
                            <asp:Label ID="lblSno" runat="server" Text='<%#Container.DisplayIndex+1 %>'></asp:Label>
                        </ItemTemplate>
                        <ItemStyle HorizontalAlign="Center" Width="60px" />
                    </asp:TemplateField>
                    <asp:TemplateField>
                        <HeaderTemplate>
                            <asp:CheckBox ID="cb_selectHead" runat="server" onchange="return OnGridHeaderSelected()">
                            </asp:CheckBox>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <asp:CheckBox ID="chkSel" runat="server" />
                        </ItemTemplate>
                        <ItemStyle HorizontalAlign="Center" Width="50px" />
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Date">
                        <ItemTemplate>
                            <asp:Label ID="lblDate" runat="server" Text='<%#Eval("Date") %>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Slot">
                        <ItemTemplate>
                            <asp:Label ID="lblSlotVal" runat="server" Text='<%#Eval("Slot") %>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
        </div>
    </center>
    <center>
        <div id="divSlotSet" runat="server" visible="false" class="popupstyle popupheight1 "
            style="height: 300em;">
            <asp:ImageButton ID="imgbtnImport" runat="server" OnClick="closeSlotSet" Width="40px"
                Height="40px" ImageUrl="~/images/close.png" Style="height: 30px; width: 30px;
                position: absolute; margin-top: 20px; margin-left: 420px;" />
            <br />
            <center>
                <div style="width: 900px; height: 450px; overflow: auto; background-color: White;
                    border: 1px solid #0CA6CA; border-top: 10px solid #0CA6CA; border-radius: 10px;">
                    <center>
                        <center>
                            <span class="fontstyleheader" style="color: Green;">Slot Settings</span>
                        </center>
                        <br />
                        <table class="maintablestyle">
                            <tr>
                                <td>
                                    <asp:Label ID="lblNoOfSlot" runat="server" Text="Number of Slots to allot"></asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtNoOfSlot" runat="server" CssClass="textbox textbox1" Width="50px"
                                        MaxLength="2"></asp:TextBox>
                                    <asp:FilteredTextBoxExtender ID="fteNoSLot" runat="server" TargetControlID="txtNoOfSlot"
                                        FilterType="Numbers">
                                    </asp:FilteredTextBoxExtender>
                                </td>
                                <td>
                                    <asp:Button ID="btnNoOfSlot" runat="server" Text="GO" CssClass="textbox btn1" Width="40px"
                                        OnClientClick="return checkNoofslots()" OnClick="btnNoOfSlot_Click" />
                                </td>
                                <td>
                                    <asp:Button ID="btnSaveSlot" runat="server" Text="Save Slot" BackColor="#81C13F"
                                        ForeColor="White" CssClass="textbox btn1" Width="100px" Visible="false" OnClientClick="return OnSaveSlotCheck()"
                                        OnClick="btnSaveSlot_Click" />
                                </td>
                            </tr>
                        </table>
                        <br />
                        <div style="height: 300px; width: 880px; overflow: auto;">
                            <asp:GridView ID="gridSlots" runat="server" AutoGenerateColumns="false" HeaderStyle-BackColor="#0CA6CA"
                                Visible="false">
                                <Columns>
                                    <asp:TemplateField HeaderText="S.No">
                                        <ItemTemplate>
                                            <asp:Label ID="lblSno" runat="server" Text='<%#Container.DisplayIndex+1 %>'></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Center" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="From">
                                        <ItemTemplate>
                                            <asp:DropDownList ID="ddlSlotFromHrs" runat="server" CssClass="textbox ddlheight"
                                                Width="50px">
                                                <asp:ListItem>00</asp:ListItem>
                                                <asp:ListItem>01</asp:ListItem>
                                                <asp:ListItem>02</asp:ListItem>
                                                <asp:ListItem>03</asp:ListItem>
                                                <asp:ListItem>04</asp:ListItem>
                                                <asp:ListItem>05</asp:ListItem>
                                                <asp:ListItem>06</asp:ListItem>
                                                <asp:ListItem>07</asp:ListItem>
                                                <asp:ListItem>08</asp:ListItem>
                                                <asp:ListItem>09</asp:ListItem>
                                                <asp:ListItem>10</asp:ListItem>
                                                <asp:ListItem>11</asp:ListItem>
                                                <asp:ListItem>12</asp:ListItem>
                                                <asp:ListItem>13</asp:ListItem>
                                                <asp:ListItem>14</asp:ListItem>
                                                <asp:ListItem>15</asp:ListItem>
                                                <asp:ListItem>16</asp:ListItem>
                                                <asp:ListItem>17</asp:ListItem>
                                                <asp:ListItem>18</asp:ListItem>
                                                <asp:ListItem>19</asp:ListItem>
                                                <asp:ListItem>20</asp:ListItem>
                                                <asp:ListItem>21</asp:ListItem>
                                                <asp:ListItem>22</asp:ListItem>
                                                <asp:ListItem>23</asp:ListItem>
                                            </asp:DropDownList>
                                            <asp:DropDownList ID="ddlSlotFromMin" runat="server" CssClass="textbox ddlheight"
                                                Width="50px">
                                                <asp:ListItem>00</asp:ListItem>
                                                <asp:ListItem>01</asp:ListItem>
                                                <asp:ListItem>02</asp:ListItem>
                                                <asp:ListItem>03</asp:ListItem>
                                                <asp:ListItem>04</asp:ListItem>
                                                <asp:ListItem>05</asp:ListItem>
                                                <asp:ListItem>06</asp:ListItem>
                                                <asp:ListItem>07</asp:ListItem>
                                                <asp:ListItem>08</asp:ListItem>
                                                <asp:ListItem>09</asp:ListItem>
                                                <asp:ListItem>10</asp:ListItem>
                                                <asp:ListItem>11</asp:ListItem>
                                                <asp:ListItem>12</asp:ListItem>
                                                <asp:ListItem>13</asp:ListItem>
                                                <asp:ListItem>14</asp:ListItem>
                                                <asp:ListItem>15</asp:ListItem>
                                                <asp:ListItem>16</asp:ListItem>
                                                <asp:ListItem>17</asp:ListItem>
                                                <asp:ListItem>18</asp:ListItem>
                                                <asp:ListItem>19</asp:ListItem>
                                                <asp:ListItem>20</asp:ListItem>
                                                <asp:ListItem>21</asp:ListItem>
                                                <asp:ListItem>22</asp:ListItem>
                                                <asp:ListItem>23</asp:ListItem>
                                                <asp:ListItem>24</asp:ListItem>
                                                <asp:ListItem>25</asp:ListItem>
                                                <asp:ListItem>26</asp:ListItem>
                                                <asp:ListItem>27</asp:ListItem>
                                                <asp:ListItem>28</asp:ListItem>
                                                <asp:ListItem>29</asp:ListItem>
                                                <asp:ListItem>30</asp:ListItem>
                                                <asp:ListItem>31</asp:ListItem>
                                                <asp:ListItem>32</asp:ListItem>
                                                <asp:ListItem>33</asp:ListItem>
                                                <asp:ListItem>34</asp:ListItem>
                                                <asp:ListItem>35</asp:ListItem>
                                                <asp:ListItem>36</asp:ListItem>
                                                <asp:ListItem>37</asp:ListItem>
                                                <asp:ListItem>38</asp:ListItem>
                                                <asp:ListItem>39</asp:ListItem>
                                                <asp:ListItem>40</asp:ListItem>
                                                <asp:ListItem>41</asp:ListItem>
                                                <asp:ListItem>42</asp:ListItem>
                                                <asp:ListItem>43</asp:ListItem>
                                                <asp:ListItem>44</asp:ListItem>
                                                <asp:ListItem>45</asp:ListItem>
                                                <asp:ListItem>46</asp:ListItem>
                                                <asp:ListItem>47</asp:ListItem>
                                                <asp:ListItem>48</asp:ListItem>
                                                <asp:ListItem>49</asp:ListItem>
                                                <asp:ListItem>50</asp:ListItem>
                                                <asp:ListItem>51</asp:ListItem>
                                                <asp:ListItem>52</asp:ListItem>
                                                <asp:ListItem>53</asp:ListItem>
                                                <asp:ListItem>54</asp:ListItem>
                                                <asp:ListItem>55</asp:ListItem>
                                                <asp:ListItem>56</asp:ListItem>
                                                <asp:ListItem>57</asp:ListItem>
                                                <asp:ListItem>58</asp:ListItem>
                                                <asp:ListItem>59</asp:ListItem>
                                            </asp:DropDownList>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Center" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="To">
                                        <ItemTemplate>
                                            <asp:DropDownList ID="ddlSlotToHrs" runat="server" CssClass="textbox ddlheight" Width="50px">
                                                <asp:ListItem>00</asp:ListItem>
                                                <asp:ListItem>01</asp:ListItem>
                                                <asp:ListItem>02</asp:ListItem>
                                                <asp:ListItem>03</asp:ListItem>
                                                <asp:ListItem>04</asp:ListItem>
                                                <asp:ListItem>05</asp:ListItem>
                                                <asp:ListItem>06</asp:ListItem>
                                                <asp:ListItem>07</asp:ListItem>
                                                <asp:ListItem>08</asp:ListItem>
                                                <asp:ListItem>09</asp:ListItem>
                                                <asp:ListItem>10</asp:ListItem>
                                                <asp:ListItem>11</asp:ListItem>
                                                <asp:ListItem>12</asp:ListItem>
                                                <asp:ListItem>13</asp:ListItem>
                                                <asp:ListItem>14</asp:ListItem>
                                                <asp:ListItem>15</asp:ListItem>
                                                <asp:ListItem>16</asp:ListItem>
                                                <asp:ListItem>17</asp:ListItem>
                                                <asp:ListItem>18</asp:ListItem>
                                                <asp:ListItem>19</asp:ListItem>
                                                <asp:ListItem>20</asp:ListItem>
                                                <asp:ListItem>21</asp:ListItem>
                                                <asp:ListItem>22</asp:ListItem>
                                                <asp:ListItem>23</asp:ListItem>
                                            </asp:DropDownList>
                                            <asp:DropDownList ID="ddlSlotToMin" runat="server" CssClass="textbox ddlheight" Width="50px">
                                                <asp:ListItem>00</asp:ListItem>
                                                <asp:ListItem>01</asp:ListItem>
                                                <asp:ListItem>02</asp:ListItem>
                                                <asp:ListItem>03</asp:ListItem>
                                                <asp:ListItem>04</asp:ListItem>
                                                <asp:ListItem>05</asp:ListItem>
                                                <asp:ListItem>06</asp:ListItem>
                                                <asp:ListItem>07</asp:ListItem>
                                                <asp:ListItem>08</asp:ListItem>
                                                <asp:ListItem>09</asp:ListItem>
                                                <asp:ListItem>10</asp:ListItem>
                                                <asp:ListItem>11</asp:ListItem>
                                                <asp:ListItem>12</asp:ListItem>
                                                <asp:ListItem>13</asp:ListItem>
                                                <asp:ListItem>14</asp:ListItem>
                                                <asp:ListItem>15</asp:ListItem>
                                                <asp:ListItem>16</asp:ListItem>
                                                <asp:ListItem>17</asp:ListItem>
                                                <asp:ListItem>18</asp:ListItem>
                                                <asp:ListItem>19</asp:ListItem>
                                                <asp:ListItem>20</asp:ListItem>
                                                <asp:ListItem>21</asp:ListItem>
                                                <asp:ListItem>22</asp:ListItem>
                                                <asp:ListItem>23</asp:ListItem>
                                                <asp:ListItem>24</asp:ListItem>
                                                <asp:ListItem>25</asp:ListItem>
                                                <asp:ListItem>26</asp:ListItem>
                                                <asp:ListItem>27</asp:ListItem>
                                                <asp:ListItem>28</asp:ListItem>
                                                <asp:ListItem>29</asp:ListItem>
                                                <asp:ListItem>30</asp:ListItem>
                                                <asp:ListItem>31</asp:ListItem>
                                                <asp:ListItem>32</asp:ListItem>
                                                <asp:ListItem>33</asp:ListItem>
                                                <asp:ListItem>34</asp:ListItem>
                                                <asp:ListItem>35</asp:ListItem>
                                                <asp:ListItem>36</asp:ListItem>
                                                <asp:ListItem>37</asp:ListItem>
                                                <asp:ListItem>38</asp:ListItem>
                                                <asp:ListItem>39</asp:ListItem>
                                                <asp:ListItem>40</asp:ListItem>
                                                <asp:ListItem>41</asp:ListItem>
                                                <asp:ListItem>42</asp:ListItem>
                                                <asp:ListItem>43</asp:ListItem>
                                                <asp:ListItem>44</asp:ListItem>
                                                <asp:ListItem>45</asp:ListItem>
                                                <asp:ListItem>46</asp:ListItem>
                                                <asp:ListItem>47</asp:ListItem>
                                                <asp:ListItem>48</asp:ListItem>
                                                <asp:ListItem>49</asp:ListItem>
                                                <asp:ListItem>50</asp:ListItem>
                                                <asp:ListItem>51</asp:ListItem>
                                                <asp:ListItem>52</asp:ListItem>
                                                <asp:ListItem>53</asp:ListItem>
                                                <asp:ListItem>54</asp:ListItem>
                                                <asp:ListItem>55</asp:ListItem>
                                                <asp:ListItem>56</asp:ListItem>
                                                <asp:ListItem>57</asp:ListItem>
                                                <asp:ListItem>58</asp:ListItem>
                                                <asp:ListItem>59</asp:ListItem>
                                            </asp:DropDownList>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Center" />
                                    </asp:TemplateField>
                                </Columns>
                            </asp:GridView>
                        </div>
                    </center>
                </div>
            </center>
        </div>
    </center>
    <div id="imgdiv2" runat="server" visible="false" style="height: 300em; z-index: 1000;
        width: 100%; background-color: rgba(54, 25, 25, .2); position: absolute; top: 0;
        left: 0px;">
        <center>
            <div id="pnl2" runat="server" class="table" style="background-color: White; height: 120px;
                width: 238px; border: 5px solid #0CA6CA; border-top: 25px solid #0CA6CA; margin-top: 200px;
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
</asp:Content>

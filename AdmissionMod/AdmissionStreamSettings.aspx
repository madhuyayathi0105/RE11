<%@ Page Title="" Language="C#" MasterPageFile="~/AdmissionMod/AttendanceSubSiteMaster.master"
    AutoEventWireup="true" CodeFile="AdmissionStreamSettings.aspx.cs" Inherits="AdmissionStreamSettings" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">
    <center>
        <div class="maindivstyle" style="width: 950px;">
            <center>
                <span class="fontstyleheader" style="color: Green;">Admission Stream Settings</span>
            </center>
            <asp:ScriptManager ID="scptMgrNew" runat="server">
            </asp:ScriptManager>
            <table style="background: white; border-radius: 5px; border-color: Red; border-style: dashed;
                border-width: 1px;">
                <tr>
                    <td>
                        <asp:Label ID="lblCollege" runat="server" Text="Institute"></asp:Label>
                    </td>
                    <td colspan="5">
                        <asp:DropDownList ID="ddlCollege" runat="server" CssClass="textbox ddlheight5" Width="400px"
                            AutoPostBack="true" OnSelectedIndexChanged="ddlCollege_SelectedIndexChanged">
                        </asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="lblBatch" runat="server" Text="Batch"></asp:Label>
                    </td>
                    <td>
                        <asp:DropDownList ID="ddlbatch" runat="server" CssClass="textbox ddlheight2" AutoPostBack="true"
                            OnSelectedIndexChanged="ddlbatch_SelectedIndexChanged">
                        </asp:DropDownList>
                    </td>
                    <td>
                        <asp:Label ID="lblEdulevel" runat="server" Text="Edu Level"></asp:Label>
                    </td>
                    <td>
                        <asp:DropDownList ID="ddlEduLev" runat="server" CssClass="textbox ddlheight2" AutoPostBack="true"
                            OnSelectedIndexChanged="ddlEdulevel_SelectedIndexChanged">
                        </asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="lblCourse" runat="server" Text="Course"></asp:Label>
                    </td>
                    <td>
                        <%-- <asp:DropDownList ID="ddlcourse" runat="server" CssClass="textbox ddlheight2" AutoPostBack="true"
                            OnSelectedIndexChanged="ddlcourse_OnSelectedIndexChanged">
                        </asp:DropDownList>--%>
                        <asp:UpdatePanel ID="updSes" runat="server">
                            <contenttemplate>
                                <asp:TextBox ID="txtSession" runat="server" CssClass="textbox textbox1" ReadOnly="true"
                                    Width="80px"></asp:TextBox>
                                <asp:Panel ID="pnlSession" runat="server" CssClass="multxtpanel">
                                    <asp:CheckBox ID="cb_Session" runat="server" Text="Select All" AutoPostBack="True"
                                        OnCheckedChanged="cb_Session_OnCheckedChanged" />
                                    <asp:CheckBoxList ID="cbl_Session" runat="server" AutoPostBack="true" OnSelectedIndexChanged="cbl_Session_OnSelectedIndexChanged">
                                    </asp:CheckBoxList>
                                </asp:Panel>
                                <asp:PopupControlExtender ID="pceSession" runat="server" TargetControlID="txtSession"
                                    PopupControlID="pnlSession" Position="Bottom">
                                </asp:PopupControlExtender>
                            </contenttemplate>
                        </asp:UpdatePanel>
                    </td>
                    <td>
                        <asp:Label ID="lblStream" runat="server" Text="Stream"></asp:Label>
                    </td>
                    <td>
                        <asp:DropDownList ID="ddlStream" runat="server" CssClass="textbox ddlheight2">
                        </asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="lblCategory" runat="server" Text="Category"></asp:Label>
                    </td>
                    <td>
                        <asp:DropDownList ID="ddlCategory" runat="server" CssClass="textbox ddlheight2">
                        </asp:DropDownList>
                    </td>
                    <td colspan="2">
                        Room Detail Include Batch Year
                        <asp:CheckBox ID="cb_includebatch" runat="server" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="lblRegistrarSign" runat="server" Text="Registrar Sign"></asp:Label>
                    </td>
                    <td colspan="3">
                        <asp:FileUpload ID="fuRegSign" runat="server" CssClass="textbox btn" Width="200px" />
                        <asp:Button ID="btnRegSignUpload" runat="server" CssClass="textbox btn" Width="60px"
                            Text="Upload" OnClick="btnRegSignUpload_OnClick" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="lblCommDate" runat="server" Text="Commence Date"></asp:Label>
                    </td>
                    <td>
                        <asp:TextBox ID="txtCommDate" runat="server" CssClass=" textbox textbox1" Width="80px"></asp:TextBox>
                        <asp:CalendarExtender ID="ceCommDate" runat="server" TargetControlID="txtCommDate"
                            Format="dd/MM/yyyy">
                        </asp:CalendarExtender>
                    </td>
                    <td>
                        <asp:Label ID="lblCommTime" runat="server" Text="Commence Time"></asp:Label>
                    </td>
                    <td>
                        <asp:DropDownList ID="ddlComHrs" runat="server" CssClass="textbox ddlheight" Width="50px">
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
                        </asp:DropDownList>
                        <asp:DropDownList ID="ddlCommMin" runat="server" CssClass="textbox ddlheight" Width="50px">
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
                        <asp:DropDownList ID="ddlCommAmPm" runat="server" CssClass="textbox ddlheight" Width="50px">
                            <asp:ListItem>AM</asp:ListItem>
                            <asp:ListItem>PM</asp:ListItem>
                        </asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:CheckBox ID="cb_hostelfees" runat="server" Text="Only Allot Hostel Fees" />
                    </td>
                    <td colspan="2">
                        <asp:CheckBox ID="cb_transportfees" runat="server" Text="Only Allot Transport Fees" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:CheckBox ID="cb_ListRegister" runat="server" Text="Show List of Register Students" />
                    </td>
                </tr>
                <tr>
                    <td colspan="4">
                        <center>
                            <br />
                            <asp:Button ID="btnDaySlotSave" runat="server" Text="Save" CssClass="textbox  btn2"
                                Width="60px" BackColor="#81C13F" ForeColor="White" OnClientClick="return OnSaveRankListCheck()"
                                OnClick="btnDaySlotSave_OnClick" />
                        </center>
                    </td>
                </tr>
                <tr>
                    <td colspan="6">
                        <fieldset style="height: 140px; width: 300px; float: left;">
                            <legend>Hostel Admission Form Fee</legend>
                            <table>
                                <tr>
                                    <td colspan="2">
                                        <asp:CheckBox ID="cb_hosteladmissionformfee" runat="server" Text="Hostel Admission Form Fee" />
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="Label45" runat="server" Text="Header"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="ddl_admissionH" runat="server" CssClass="textbox textbox1 ddlheight5"
                                            AutoPostBack="true" OnSelectedIndexChanged="ddl_admissionH_SelectedIndexChanged">
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="Label46" runat="server" Text="Ledger"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="ddl_admissionL" runat="server" CssClass="textbox textbox1 ddlheight5">
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="Label47" runat="server" Text="Amount"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txt_admissionfee" runat="server" placeholder="0.00" CssClass="textbox txtheight"
                                            Style="text-align: right; width: 80px; height: 15px;" BackColor="#EFF8D5" MaxLength="15">
                                        </asp:TextBox>
                                        <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender20" runat="server" FilterType="Numbers,Custom"
                                            ValidChars="." TargetControlID="txt_admissionfee">
                                        </asp:FilteredTextBoxExtender>
                                        <asp:Button ID="btn_hostelfeesave" runat="server" OnClick="btn_hostelfeesave_click"
                                            Height="26px" Width="60px" Text="Add" />
                                    </td>
                                </tr>
                            </table>
                        </fieldset>
                        <fieldset style="height: 140px; width: 300px; float: left;">
                            <legend>Add Details</legend>

                        </fieldset>
                    </td>
                </tr>
            </table>
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

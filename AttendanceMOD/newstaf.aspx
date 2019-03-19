<%@ Page Title="" Language="C#" MasterPageFile="~/AttendanceMOD/AttendanceSubSiteMaster.master"
    AutoEventWireup="true" CodeFile="newstaf.aspx.cs" Inherits="newstaf" EnableEventValidation="false" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <script type="text/javascript">
        function reason() {
            document.getElementById('<%=btnaddreason.ClientID%>').style.display = 'block';
            document.getElementById('<%=btnremovereason.ClientID%>').style.display = 'block';
        }

        function buildDropDown(obj) {
            var status = obj.options[obj.selectedIndex].value;
            var row = obj.parentNode.parentNode;
            //alert(status); 
            var rowIndex = row.rowIndex;
            //alert(status);
            var grid = document.getElementById('<%=GridView1.ClientID%>');
            //alert(grid);
            var ddl = document.getElementById('MainContent_GridView1_ddlSelectAll');
            var subid = ddl.options[ddl.selectedIndex].innerHTML;

            //alert(subid);
            for (var i = 0; i < grid.rows.length - 1; i++) {
                var ddl3 = document.getElementById('MainContent_GridView1_ddlLeavetype_' + i.toString());
                if (ddl3.disabled == false) {
                    for (value = 0; value <= ddl3.options.length - 1; value++) {
                        //alert(subid);
                        if (ddl3.options[value].text.trim() == subid.trim()) {
                            ddl3.options[value].selected = subid;
                        }
                    }
                }

            }
        }

        function buildDropDown2(obj) {
            var status = obj.options[obj.selectedIndex].value;
            var row = obj.parentNode.parentNode;
            //alert(status); 
            var rowIndex = row.rowIndex;
            //alert(status);
            var grid = document.getElementById('<%=GridView1.ClientID%>');
            //alert(grid);
            var ddl = document.getElementById('MainContent_GridView1_ddlSelectAll2');
            var subid = ddl.options[ddl.selectedIndex].innerHTML;

            //alert(subid);
            for (var i = 0; i < grid.rows.length - 1; i++) {
                var ddl3 = document.getElementById('MainContent_GridView1_ddlLeavetype2_' + i.toString());
                if (ddl3.disabled == false) {
                    for (value = 0; value <= ddl3.options.length - 1; value++) {
                        //alert(subid);
                        if (ddl3.options[value].text.trim() == subid.trim()) {
                            ddl3.options[value].selected = subid;
                        }
                    }
                }
            }
        }
        function SelectDropDown() {
            var subid = "P";
            //alert(subid);
            var grid = document.getElementById('<%=GridView1.ClientID%>');
            for (var i = 0; i < grid.rows.length - 1; i++) {
                var ddl3 = document.getElementById('MainContent_GridView1_ddlLeavetype_' + i.toString());
                if (ddl3.disabled == false) {
                    for (value = 0; value <= ddl3.options.length - 1; value++) {
                        //alert(subid);
                        if (ddl3.options[value].text == subid) {
                            ddl3.options[value].selected = subid;
                        }
                    }
                }
            }
        }

        function buildDropDown3(obj) {
            var status = obj.options[obj.selectedIndex].value;
            var row = obj.parentNode.parentNode;
            //alert(status); 
            var rowIndex = row.rowIndex;
            //alert(status);
            var grid = document.getElementById('<%=GridView1.ClientID%>');
            //alert(grid);
            var ddl = document.getElementById('MainContent_GridView1_ddlSelectAll3');
            var subid = ddl.options[ddl.selectedIndex].innerHTML;

            //alert(subid);
            for (var i = 0; i < grid.rows.length - 1; i++) {
                var ddl3 = document.getElementById('MainContent_GridView1_ddlLeavetype3_' + i.toString());
                if (ddl3.disabled == false) {
                    for (value = 0; value <= ddl3.options.length - 1; value++) {
                        //alert(subid);
                        if (ddl3.options[value].text.trim() == subid.trim()) {
                            ddl3.options[value].selected = subid;
                        }
                    }
                }

            }
        }
        function buildDropDown4(obj) {
            var status = obj.options[obj.selectedIndex].value;
            var row = obj.parentNode.parentNode;
            //alert(status); 
            var rowIndex = row.rowIndex;
            //alert(status);
            var grid = document.getElementById('<%=GridView1.ClientID%>');
            //alert(grid);
            var ddl = document.getElementById('MainContent_GridView1_ddlSelectAll4');
            var subid = ddl.options[ddl.selectedIndex].innerHTML;

            //alert(subid);
            for (var i = 0; i < grid.rows.length - 1; i++) {
                var ddl3 = document.getElementById('MainContent_GridView1_ddlLeavetype4_' + i.toString());
                if (ddl3.disabled == false) {
                    for (value = 0; value <= ddl3.options.length - 1; value++) {
                        //alert(subid);
                        if (ddl3.options[value].text.trim() == subid.trim()) {
                            ddl3.options[value].selected = subid;
                        }
                    }
                }

            }
        }

        function buildDropDown5(obj) {
            var status = obj.options[obj.selectedIndex].value;
            var row = obj.parentNode.parentNode;
            //alert(status); 
            var rowIndex = row.rowIndex;
            //alert(status);
            var grid = document.getElementById('<%=GridView1.ClientID%>');
            //alert(grid);
            var ddl = document.getElementById('MainContent_GridView1_ddlSelectAll5');
            var subid = ddl.options[ddl.selectedIndex].innerHTML;

            //alert(subid);
            for (var i = 0; i < grid.rows.length - 1; i++) {
                var ddl3 = document.getElementById('MainContent_GridView1_ddlLeavetype5_' + i.toString());
                if (ddl3.disabled == false) {
                    for (value = 0; value <= ddl3.options.length - 1; value++) {
                        //alert(subid);
                        if (ddl3.options[value].text.trim() == subid.trim()) {
                            ddl3.options[value].selected = subid;
                        }
                    }
                }

            }
        }

        function buildDropDown6(obj) {
            var status = obj.options[obj.selectedIndex].value;
            var row = obj.parentNode.parentNode;
            //alert(status); 
            var rowIndex = row.rowIndex;
            //alert(status);
            var grid = document.getElementById('<%=GridView1.ClientID%>');
            //alert(grid);
            var ddl = document.getElementById('MainContent_GridView1_ddlSelectAll6');
            var subid = ddl.options[ddl.selectedIndex].innerHTML;

            //alert(subid);
            for (var i = 0; i < grid.rows.length - 1; i++) {
                var ddl3 = document.getElementById('MainContent_GridView1_ddlLeavetype6_' + i.toString());
                if (ddl3.disabled == false) {
                    for (value = 0; value <= ddl3.options.length - 1; value++) {
                        //alert(subid);
                        if (ddl3.options[value].text.trim() == subid.trim()) {
                            ddl3.options[value].selected = subid;
                        }
                    }
                }

            }
        }

        function buildDropDown7(obj) {
            var status = obj.options[obj.selectedIndex].value;
            var row = obj.parentNode.parentNode;
            //alert(status); 
            var rowIndex = row.rowIndex;
            //alert(status);
            var grid = document.getElementById('<%=GridView1.ClientID%>');
            //alert(grid);
            var ddl = document.getElementById('MainContent_GridView1_ddlSelectAll7');
            var subid = ddl.options[ddl.selectedIndex].innerHTML;

            //alert(subid);
            for (var i = 0; i < grid.rows.length - 1; i++) {
                var ddl3 = document.getElementById('MainContent_GridView1_ddlLeavetype7_' + i.toString());
                if (ddl3.disabled == false) {
                    for (value = 0; value <= ddl3.options.length - 1; value++) {
                        //alert(subid);
                        if (ddl3.options[value].text.trim() == subid.trim()) {
                            ddl3.options[value].selected = subid;
                        }
                    }
                }

            }
        }

        function buildDropDown8(obj) {
            var status = obj.options[obj.selectedIndex].value;
            var row = obj.parentNode.parentNode;
            //alert(status); 
            var rowIndex = row.rowIndex;
            //alert(status);
            var grid = document.getElementById('<%=GridView1.ClientID%>');
            //alert(grid);
            var ddl = document.getElementById('MainContent_GridView1_ddlSelectAll8');
            var subid = ddl.options[ddl.selectedIndex].innerHTML;

            //alert(subid);
            for (var i = 0; i < grid.rows.length - 1; i++) {
                var ddl3 = document.getElementById('MainContent_GridView1_ddlLeavetype8_' + i.toString());
                if (ddl3.disabled == false) {
                    for (value = 0; value <= ddl3.options.length - 1; value++) {
                        //alert(subid);
                        if (ddl3.options[value].text.trim() == subid.trim()) {
                            ddl3.options[value].selected = subid;
                        }
                    }
                }
            }
        }

        function buildDropDown9(obj) {
            var status = obj.options[obj.selectedIndex].value;
            var row = obj.parentNode.parentNode;
            //alert(status); 
            var rowIndex = row.rowIndex;
            //alert(status);
            var grid = document.getElementById('<%=GridView1.ClientID%>');
            //alert(grid);
            var ddl = document.getElementById('MainContent_GridView1_ddlSelectAll9');
            var subid = ddl.options[ddl.selectedIndex].innerHTML;

            //alert(subid);
            for (var i = 0; i < grid.rows.length - 1; i++) {
                var ddl3 = document.getElementById('MainContent_GridView1_ddlLeavetype9_' + i.toString());
                if (ddl3.disabled == false) {
                    for (value = 0; value <= ddl3.options.length - 1; value++) {
                        //alert(subid);
                        if (ddl3.options[value].text.trim() == subid.trim()) {
                            ddl3.options[value].selected = subid;
                        }
                    }
                }
            }
        }
        function buildDropDown10(obj) {
            var status = obj.options[obj.selectedIndex].value;
            var row = obj.parentNode.parentNode;
            //alert(status); 
            var rowIndex = row.rowIndex;
            //alert(status);
            var grid = document.getElementById('<%=GridView1.ClientID%>');
            //alert(grid);
            var ddl = document.getElementById('MainContent_GridView1_ddlSelectAll10');
            var subid = ddl.options[ddl.selectedIndex].innerHTML;

            //alert(subid);
            for (var i = 0; i < grid.rows.length - 1; i++) {
                var ddl3 = document.getElementById('MainContent_GridView1_ddlLeavetype10_' + i.toString());
                if (ddl3.disabled == false) {
                    for (value = 0; value <= ddl3.options.length - 1; value++) {
                        //alert(subid);
                        if (ddl3.options[value].text.trim() == subid.trim()) {
                            ddl3.options[value].selected = subid;
                        }
                    }
                }

            }
        }
        function SelectDropDown() {
            var subid1 = "P";
            //alert(subid);
            var grid = document.getElementById('<%=GridView1.ClientID%>');
            for (var i = 0; i < grid.rows.length - 1; i++) {
                var ddl5 = document.getElementById('MainContent_GridView1_ddlLeavetype_' + i.toString());
                var ddl2 = document.getElementById('MainContent_GridView1_ddlLeavetype2_' + i.toString());
                var ddl3 = document.getElementById('MainContent_GridView1_ddlLeavetype3_' + i.toString());
                var ddl4 = document.getElementById('MainContent_GridView1_ddlLeavetype4_' + i.toString());
                var ddl1 = document.getElementById('MainContent_GridView1_ddlLeavetype5_' + i.toString());
                var ddl6 = document.getElementById('MainContent_GridView1_ddlLeavetype6_' + i.toString());
                var ddl7 = document.getElementById('MainContent_GridView1_ddlLeavetype7_' + i.toString());
                var ddl8 = document.getElementById('MainContent_GridView1_ddlLeavetype8_' + i.toString());
                var ddl9 = document.getElementById('MainContent_GridView1_ddlLeavetype9_' + i.toString());
                var ddl10 = document.getElementById('MainContent_GridView1_ddlLeavetype10_' + i.toString());
                if (ddl5 != null && ddl5.disabled == false) {
                    for (value = 0; value <= ddl5.options.length - 1; value++) {
                        //alert(subid1);
                        if (ddl5.options[value].text == subid1) {
                            ddl5.options[value].selected = true;
                        }
                    }
                }
                if (ddl2 != null && ddl2.disabled == false) {
                    for (value = 0; value <= ddl2.options.length - 1; value++) {
                        //alert(subid1);
                        if (ddl2.options[value].text == subid1) {
                            ddl2.options[value].selected = true;
                        }
                    }
                }
                if (ddl3 != null && ddl3.disabled == false) {
                    for (value = 0; value <= ddl3.options.length - 1; value++) {
                        //alert(subid1);
                        if (ddl3.options[value].text == subid1) {
                            ddl3.options[value].selected = true;
                        }
                    }
                }
                if (ddl4 != null && ddl4.disabled == false) {
                    for (value = 0; value <= ddl4.options.length - 1; value++) {
                        //alert(subid1);
                        if (ddl4.options[value].text == subid1) {
                            ddl4.options[value].selected = true;
                        }
                    }
                }
                if (ddl1 != null && ddl1.disabled == false) {
                    for (value = 0; value <= ddl1.options.length - 1; value++) {
                        //alert(subid1);
                        if (ddl1.options[value].text == subid1) {
                            ddl1.options[value].selected = true;
                        }
                    }
                }
                if (ddl6 != null && ddl6.disabled == false) {
                    for (value = 0; value <= ddl6.options.length - 1; value++) {
                        //alert(subid1);
                        if (ddl6.options[value].text == subid1) {
                            ddl6.options[value].selected = true;
                        }
                    }
                }

                if (ddl7 != null && ddl7.disabled == false) {
                    for (value = 0; value <= ddl7.options.length - 1; value++) {
                        //alert(subid1);
                        if (ddl7.options[value].text == subid1) {
                            ddl7.options[value].selected = true;
                        }
                    }
                }
                if (ddl8 != null && ddl8.disabled == false) {
                    for (value = 0; value <= ddl8.options.length - 1; value++) {
                        //alert(subid1);
                        if (ddl8.options[value].text == subid1) {
                            ddl8.options[value].selected = true;
                        }
                    }
                }
                if (ddl9 != null && ddl9.disabled == false) {
                    for (value = 0; value <= ddl9.options.length - 1; value++) {
                        //alert(subid1);
                        if (ddl9.options[value].text == subid1) {
                            ddl9.options[value].selected = true;
                        }
                    }
                }
                if (ddl10 != null && ddl10.disabled == false) {
                    for (value = 0; value <= ddl10.options.length - 1; value++) {
                        //alert(subid1);
                        if (ddl10.options[value].text == subid1) {
                            ddl10.options[value].selected = true;
                        }
                    }
                }

            }
        }
        function DeSelectDropDown() {
            var subid1 = " ";
            //alert(subid);
            var grid = document.getElementById('<%=GridView1.ClientID%>');
            for (var i = 0; i < grid.rows.length - 1; i++) {

                var ddl5 = document.getElementById('MainContent_GridView1_ddlLeavetype_' + i.toString());
                var ddl2 = document.getElementById('MainContent_GridView1_ddlLeavetype2_' + i.toString());
                var ddl3 = document.getElementById('MainContent_GridView1_ddlLeavetype3_' + i.toString());
                var ddl4 = document.getElementById('MainContent_GridView1_ddlLeavetype4_' + i.toString());
                var ddl1 = document.getElementById('MainContent_GridView1_ddlLeavetype5_' + i.toString());
                var ddl6 = document.getElementById('MainContent_GridView1_ddlLeavetype6_' + i.toString());
                var ddl7 = document.getElementById('MainContent_GridView1_ddlLeavetype7_' + i.toString());
                var ddl8 = document.getElementById('MainContent_GridView1_ddlLeavetype8_' + i.toString());
                var ddl9 = document.getElementById('MainContent_GridView1_ddlLeavetype9_' + i.toString());
                var ddl10 = document.getElementById('MainContent_GridView1_ddlLeavetype10_' + i.toString());
                if (ddl5 != null) {
                    for (value = 0; value <= ddl5.options.length - 1; value++) {
                        //alert(subid1);
                        if (ddl5.options[value].text.trim() == subid1.trim()) {
                            ddl5.options[value].selected = true;
                        }
                    }
                }
                if (ddl2 != null) {
                    for (value = 0; value <= ddl2.options.length - 1; value++) {
                        //alert(subid1);
                        if (ddl2.options[value].text.trim() == subid1.trim()) {
                            ddl2.options[value].selected = true;
                        }
                    }
                }
                if (ddl3 != null) {
                    for (value = 0; value <= ddl3.options.length - 1; value++) {
                        //alert(subid1);
                        if (ddl3.options[value].text.trim() == subid1.trim()) {
                            ddl3.options[value].selected = true;
                        }
                    }
                }
                if (ddl4 != null) {
                    for (value = 0; value <= ddl4.options.length - 1; value++) {
                        //alert(subid1);
                        if (ddl4.options[value].text.trim() == subid1.trim()) {
                            ddl4.options[value].selected = true;
                        }
                    }
                }
                if (ddl1 != null) {
                    for (value = 0; value <= ddl1.options.length - 1; value++) {
                        //alert(subid1);
                        if (ddl1.options[value].text.trim() == subid1.trim()) {
                            ddl1.options[value].selected = true;
                        }
                    }
                }
                if (ddl6 != null) {
                    for (value = 0; value <= ddl6.options.length - 1; value++) {
                        //alert(subid1);
                        if (ddl6.options[value].text.trim() == subid1.trim()) {
                            ddl6.options[value].selected = true;
                        }
                    }
                }

                if (ddl7 != null) {
                    for (value = 0; value <= ddl7.options.length - 1; value++) {
                        //alert(subid1);
                        if (ddl7.options[value].text.trim() == subid1.trim()) {
                            ddl7.options[value].selected = true;
                        }
                    }
                }
                if (ddl8 != null) {
                    for (value = 0; value <= ddl8.options.length - 1; value++) {
                        //alert(subid1);
                        if (ddl8.options[value].text.trim() == subid1.trim()) {
                            ddl8.options[value].selected = true;
                        }
                    }
                }
                if (ddl9 != null) {
                    for (value = 0; value <= ddl9.options.length - 1; value++) {
                        //alert(subid1);
                        if (ddl9.options[value].text.trim() == subid1.trim()) {
                            ddl9.options[value].selected = true;
                        }
                    }
                }
                if (ddl10 != null) {
                    for (value = 0; value <= ddl10.options.length - 1; value++) {
                        //alert(subid1);
                        if (ddl10.options[value].text.trim() == subid1.trim()) {
                            ddl10.options[value].selected = true;
                        }
                    }
                }

            }
        }
        function executeAfter(row) {
            var rowval = row;
            alert(rowval);
        }

        function selectedExcludedoption(selectBox) {
            var rowIndex1 = $(selectBox).closest('tr').index();
            var rowIndex = rowIndex1 - 1;
            //var colInde = $(selectBox).closest('td').index();
            //alert(colInde);
            var ddl4 = document.getElementById('MainContent_GridView1_ddlSelect_' + rowIndex.toString());
            //alert(ddl4);
            var subid1 = ddl4.options[ddl4.selectedIndex].innerHTML.trim();
            //alert(subid1);

            var ddl5 = document.getElementById('MainContent_GridView1_ddlLeavetype_' + rowIndex.toString());
            var ddl2 = document.getElementById('MainContent_GridView1_ddlLeavetype2_' + rowIndex.toString());
            var ddl3 = document.getElementById('MainContent_GridView1_ddlLeavetype3_' + rowIndex.toString());
            var ddl4 = document.getElementById('MainContent_GridView1_ddlLeavetype4_' + rowIndex.toString());
            var ddl1 = document.getElementById('MainContent_GridView1_ddlLeavetype5_' + rowIndex.toString());
            var ddl6 = document.getElementById('MainContent_GridView1_ddlLeavetype6_' + rowIndex.toString());
            var ddl7 = document.getElementById('MainContent_GridView1_ddlLeavetype7_' + rowIndex.toString());
            var ddl8 = document.getElementById('MainContent_GridView1_ddlLeavetype8_' + rowIndex.toString());
            var ddl9 = document.getElementById('MainContent_GridView1_ddlLeavetype9_' + rowIndex.toString());
            var ddl10 = document.getElementById('MainContent_GridView1_ddlLeavetype10_' + rowIndex.toString());
            if (ddl5 != null && ddl5.disabled == false) {
                for (value = 0; value <= ddl5.options.length - 1; value++) {
                    //alert(subid1);
                    if (ddl5.options[value].text == subid1) {
                        ddl5.options[value].selected = true;
                    }
                }
            }
            if (ddl2 != null && ddl2.disabled == false) {
                for (value = 0; value <= ddl2.options.length - 1; value++) {
                    //alert(subid1);
                    if (ddl2.options[value].text == subid1) {
                        ddl2.options[value].selected = true;
                    }
                }
            }
            if (ddl3 != null && ddl3.disabled == false) {
                for (value = 0; value <= ddl3.options.length - 1; value++) {
                    //alert(subid1);
                    if (ddl3.options[value].text == subid1) {
                        ddl3.options[value].selected = true;
                    }
                }
            }
            if (ddl4 != null && ddl4.disabled == false) {
                for (value = 0; value <= ddl4.options.length - 1; value++) {
                    //alert(subid1);
                    if (ddl4.options[value].text == subid1) {
                        ddl4.options[value].selected = true;
                    }
                }
            }
            if (ddl1 != null && ddl1.disabled == false) {
                for (value = 0; value <= ddl1.options.length - 1; value++) {
                    //alert(subid1);
                    if (ddl1.options[value].text == subid1) {
                        ddl1.options[value].selected = true;
                    }
                }
            }
            if (ddl6 != null && ddl6.disabled == false) {
                for (value = 0; value <= ddl6.options.length - 1; value++) {
                    //alert(subid1);
                    if (ddl6.options[value].text == subid1) {
                        ddl6.options[value].selected = true;
                    }
                }
            }

            if (ddl7 != null && ddl7.disabled == false) {
                for (value = 0; value <= ddl7.options.length - 1; value++) {
                    //alert(subid1);
                    if (ddl7.options[value].text == subid1) {
                        ddl7.options[value].selected = true;
                    }
                }
            }
            if (ddl8 != null && ddl8.disabled == false) {
                for (value = 0; value <= ddl8.options.length - 1; value++) {
                    //alert(subid1);
                    if (ddl8.options[value].text == subid1) {
                        ddl8.options[value].selected = true;
                    }
                }
            }
            if (ddl9 != null && ddl9.disabled == false) {
                for (value = 0; value <= ddl9.options.length - 1; value++) {
                    //alert(subid1);
                    if (ddl9.options[value].text == subid1) {
                        ddl9.options[value].selected = true;
                    }
                }
            }
            if (ddl10 != null && ddl10.disabled == false) {
                for (value = 0; value <= ddl10.options.length - 1; value++) {
                    //alert(subid1);
                    if (ddl10.options[value].text == subid1) {
                        ddl10.options[value].selected = true;
                    }
                }
            }
        }
    </script>
    <style type="text/css">
        .GridHeaderFixed
        {
            background-color: #DCDCDC;
            font-weight: bold;
            border: none;
            color: Black;
            vertical-align: bottom;
            position: relative;
            top: -1px;
        }
    </style>
    <style type="text/css">
        .floats
        {
            float: right;
        }
        .cpHeader
        {
            color: white;
            background-color: #719DDB;
            font-size: 12px;
            cursor: pointer;
            padding: 4px;
            font-style: normal;
            font-variant: normal;
            font-weight: bold;
            line-height: normal;
            font-family: "auto Trebuchet MS" , Verdana;
        }
        .cpBody
        {
            background-color: transparent;
            font: normal 11px auto Verdana, Arial;
            border: 1px gray;
            padding-top: 7px;
            padding-left: 4px;
            padding-right: 4px;
            padding-bottom: 4px;
        }
        .cpimage
        {
            float: right;
            vertical-align: middle;
            background-color: transparent;
        }
        .cur
        {
            cursor: pointer;
        }
        .label
        {
            font-family: Book Antiqua;
            font-size: 15px;
            font-weight: bold;
        }
    </style>
    <script language="javascript" type="text/javascript">

        function postBackByObject() {
            var o = window.event.srcElement;
            if (o.tagName == "INPUT" && o.type == "checkbox")
                var o = window.event.srcElement;
            {
                __doPostBack("", "");
            }
        }
        function callme() {
            var i = val.lastIndexOf("\\");
            return val.substring(i + 1);
        }
        function callme(oFile) {
            document.getElementById("TextBox1").value = oFile.value;
        }
    </script>
    <style type="text/css" media="screen">
        .floats
        {
            height: 26px;
        }
        .CenterPB
        {
            position: absolute;
            left: 50%;
            top: 50%;
            margin-top: -20px;
            margin-left: -20px;
            width: auto;
            height: auto;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <center>
        <asp:Label ID="Label1" runat="server" Text="Attendance" class="fontstyleheader" Font-Bold="True"
            Font-Names="Book Antiqua" ForeColor="Green" Style="margin: 0px; margin-bottom: 10px;
            position: relative;"></asp:Label>
    </center>
    <asp:UpdatePanel ID="updattendance" runat="server">
        <ContentTemplate>
            <asp:UpdateProgress ID="UpdateProgress1" runat="server" DynamicLayout="true" AssociatedUpdatePanelID="updattendance">
                <ProgressTemplate>
                    <div class="CenterPB" style="height: 40px; width: 40px;">
                        <img src="../images/progress2.gif" height="180px" width="180px" />
                    </div>
                </ProgressTemplate>
            </asp:UpdateProgress>
            <asp:ModalPopupExtender ID="modalPopup" runat="server" TargetControlID="UpdateProgress1"
                PopupControlID="UpdateProgress1">
            </asp:ModalPopupExtender>
            <center>
                <table class="maintablestyle" style="margin: 0px; margin-top: 10px; margin-bottom: 10px;">
                    <tr>
                        <td>
                            <asp:Label ID="scodelbl" runat="server" Text="Staff Code" Font-Bold="True" Font-Names="Book Antiqua"
                                Font-Size="Medium"></asp:Label>
                        </td>
                        <td>
                            <asp:DropDownList ID="scodetxt" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                Font-Size="Medium" Height="26px" AutoPostBack="True" OnSelectedIndexChanged="scodetxt_SelectedIndexChanged">
                            </asp:DropDownList>
                        </td>
                        <td>
                            <asp:Label ID="lblstaffname" runat="server" Text="Staff Name" Font-Bold="True" Font-Names="Book Antiqua"
                                Font-Size="Medium"></asp:Label>
                        </td>
                        <td>
                            <asp:DropDownList ID="ddlstaffname" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                Font-Size="Medium" Height="26px" AutoPostBack="True" OnSelectedIndexChanged="ddlstaffname_SelectedIndexChanged">
                            </asp:DropDownList>
                        </td>
                        <td>
                            <asp:Label ID="Labelfdate" runat="server" Text="From" Font-Bold="True" Font-Names="Book Antiqua"
                                Font-Size="Medium"></asp:Label>
                        </td>
                        <td>
                            <asp:TextBox ID="tbfdate" runat="server" OnTextChanged="tbfdate_TextChanged" Height="19px"
                                Width="91px" AutoPostBack="True" Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium"></asp:TextBox>
                        </td>
                        <asp:CalendarExtender ID="CalendarExtender1" TargetControlID="tbfdate" Format="d-MM-yyyy"
                            runat="server">
                        </asp:CalendarExtender>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" Display="Dynamic" runat="server"
                            ControlToValidate="tbfdate" ErrorMessage="Select From Date" ForeColor="Red" Width="110px"></asp:RequiredFieldValidator>
                        <td>
                            <asp:Label ID="Labeltodate" runat="server" Text="To" Font-Bold="True" Font-Names="Book Antiqua"
                                Font-Size="Medium"></asp:Label>
                        </td>
                        <td>
                            <asp:TextBox ID="tbtodate" runat="server" OnTextChanged="tbtodate_TextChanged" Height="18px"
                                Width="80px" AutoPostBack="True" Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium"></asp:TextBox>
                        </td>
                        <asp:CalendarExtender ID="CalendarExtender2" Format="d-MM-yyyy" TargetControlID="tbtodate"
                            runat="server">
                        </asp:CalendarExtender>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator2" Display="Dynamic" runat="server"
                            ControlToValidate="tbtodate" ErrorMessage="Select From Date" ForeColor="Red"
                            Width="110px"></asp:RequiredFieldValidator>
                        <td>
                            <asp:Button ID="Buttongo" runat="server" Text="Go" OnClick="Buttongo_Click" Font-Bold="True"
                                Font-Names="Book Antiqua" Font-Size="Medium" />
                        </td>
                        <td>
                            <asp:CheckBox ID="ck_append" runat="server" Font-Names="Book Antiqua" Font-Size="Medium"
                                Font-Bold="true" Text="Append Periods" />
                            <asp:CheckBox ID="CheckBox1" runat="server" Font-Names="Book Antiqua" Font-Size="Medium"
                                Font-Bold="true" Text="With Cond Hour" />
                            <asp:CheckBox ID="chkis_studavailable" runat="server" Visible="false" Text="" />
                        </td>
                        <td>
                            <asp:Button ID="btnsliplist" runat="server" Text="Slip List" Font-Names="Book Antiqua"
                                Font-Size="Medium" Font-Bold="true" OnClick="btnsliplist_Click" />
                        </td>
                    </tr>
                </table>
                <table style="margin: 0px; margin-bottom: 10px; position: relative;">
                    <tr>
                        <td>
                            <asp:Label ID="snamelbl1" runat="server" Text="Staff Name:" Font-Bold="True" Font-Names="Book Antiqua"
                                Font-Size="Medium"></asp:Label>
                        </td>
                        <td>
                            <asp:Label ID="snamelbl" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                Font-Size="Small" ForeColor="Green"></asp:Label>
                        </td>
                    </tr>
                </table>
            </center>
            <asp:Label ID="Labelstaf" runat="server" ForeColor="Red" Width="1200px" Text="There is no class for the staff between the given date"
                Visible="False" Font-Names="Book Antiqua" Font-Size="Medium" Style="margin: 0px;
                margin-top: 20px; margin-bottom: 15px; position: relative;"></asp:Label>
            <div id="divTimeTable" runat="server" visible="false">
                <asp:GridView ID="gridTimeTable" runat="server" AutoGenerateColumns="false" Font-Names="Book Antiqua"
                    HeaderStyle-BackColor="#0CA6CA" BackColor="White">
                    <%-- OnDataBound="gridTimeTable_OnDataBound"--%>
                    <Columns>
                        <asp:TemplateField HeaderText="Date">
                            <ItemTemplate>
                                <asp:Label ID="lblDateDisp" runat="server" Text='<%#Eval("DateDisp") %>'></asp:Label>
                                <asp:Label ID="lblDate" runat="server" Text='<%#Eval("DateVal") %>' Visible="false"></asp:Label>
                                <asp:Label ID="lblDayVal" runat="server" Text='<%#Eval("DayOrder") %>' Visible="false"></asp:Label>
                            </ItemTemplate>
                            <ItemStyle Width="100px" HorizontalAlign="Center" BackColor="#F8B7B3" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Period 1">
                            <ItemTemplate>
                                <asp:LinkButton ID="lnkPeriod_1" runat="server" Text='<%#Eval("P1ValDisp") %>' ForeColor="Blue"
                                    OnClick="lnkAttMark" Font-Underline="false"></asp:LinkButton>
                                <asp:Label ID="lblPeriod_1" runat="server" Text='<%#Eval("P1Val") %>' Visible="false"></asp:Label>
                                <asp:Label ID="lblTT_1" runat="server" Text='<%#Eval("TT_1") %>' Visible="false"></asp:Label>
                            </ItemTemplate>
                            <ItemStyle Width="200px" HorizontalAlign="Left" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Period 2">
                            <ItemTemplate>
                                <asp:LinkButton ID="lnkPeriod_2" runat="server" Text='<%#Eval("P2ValDisp") %>' ForeColor="Blue"
                                    OnClick="lnkAttMark" Font-Underline="false"></asp:LinkButton>
                                <asp:Label ID="lblPeriod_2" runat="server" Text='<%#Eval("P2Val") %>' Visible="false"></asp:Label>
                                <asp:Label ID="lblTT_2" runat="server" Text='<%#Eval("TT_2") %>' Visible="false"></asp:Label>
                            </ItemTemplate>
                            <ItemStyle Width="200px" HorizontalAlign="Left" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Period 3">
                            <ItemTemplate>
                                <asp:LinkButton ID="lnkPeriod_3" runat="server" Text='<%#Eval("P3ValDisp") %>' ForeColor="Blue"
                                    OnClick="lnkAttMark" Font-Underline="false"></asp:LinkButton>
                                <asp:Label ID="lblPeriod_3" runat="server" Text='<%#Eval("P3Val") %>' Visible="false"></asp:Label>
                                <asp:Label ID="lblTT_3" runat="server" Text='<%#Eval("TT_3") %>' Visible="false"></asp:Label>
                            </ItemTemplate>
                            <ItemStyle Width="200px" HorizontalAlign="Left" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Period 4">
                            <ItemTemplate>
                                <asp:LinkButton ID="lnkPeriod_4" runat="server" Text='<%#Eval("P4ValDisp") %>' ForeColor="Blue"
                                    OnClick="lnkAttMark" Font-Underline="false"></asp:LinkButton>
                                <asp:Label ID="lblPeriod_4" runat="server" Text='<%#Eval("P4Val") %>' Visible="false"></asp:Label>
                                <asp:Label ID="lblTT_4" runat="server" Text='<%#Eval("TT_4") %>' Visible="false"></asp:Label>
                            </ItemTemplate>
                            <ItemStyle Width="200px" HorizontalAlign="Left" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Period 5">
                            <ItemTemplate>
                                <asp:LinkButton ID="lnkPeriod_5" runat="server" Text='<%#Eval("P5ValDisp") %>' ForeColor="Blue"
                                    OnClick="lnkAttMark" Font-Underline="false"></asp:LinkButton>
                                <asp:Label ID="lblPeriod_5" runat="server" Text='<%#Eval("P5Val") %>' Visible="false"></asp:Label>
                                <asp:Label ID="lblTT_5" runat="server" Text='<%#Eval("TT_5") %>' Visible="false"></asp:Label>
                            </ItemTemplate>
                            <ItemStyle Width="200px" HorizontalAlign="Left" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Period 6">
                            <ItemTemplate>
                                <asp:LinkButton ID="lnkPeriod_6" runat="server" Text='<%#Eval("P6ValDisp") %>' ForeColor="Blue"
                                    OnClick="lnkAttMark" Font-Underline="false"></asp:LinkButton>
                                <asp:Label ID="lblPeriod_6" runat="server" Text='<%#Eval("P6Val") %>' Visible="false"></asp:Label>
                                <asp:Label ID="lblTT_6" runat="server" Text='<%#Eval("TT_6") %>' Visible="false"></asp:Label>
                            </ItemTemplate>
                            <ItemStyle Width="200px" HorizontalAlign="Left" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Period 7">
                            <ItemTemplate>
                                <asp:LinkButton ID="lnkPeriod_7" runat="server" Text='<%#Eval("P7ValDisp") %>' ForeColor="Blue"
                                    OnClick="lnkAttMark" Font-Underline="false"></asp:LinkButton>
                                <asp:Label ID="lblPeriod_7" runat="server" Text='<%#Eval("P7Val") %>' Visible="false"></asp:Label>
                                <asp:Label ID="lblTT_7" runat="server" Text='<%#Eval("TT_7") %>' Visible="false"></asp:Label>
                            </ItemTemplate>
                            <ItemStyle Width="200px" HorizontalAlign="Left" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Period 8">
                            <ItemTemplate>
                                <asp:LinkButton ID="lnkPeriod_8" runat="server" Text='<%#Eval("P8ValDisp") %>' ForeColor="Blue"
                                    OnClick="lnkAttMark" Font-Underline="false"></asp:LinkButton>
                                <asp:Label ID="lblPeriod_8" runat="server" Text='<%#Eval("P8Val") %>' Visible="false"></asp:Label>
                                <asp:Label ID="lblTT_8" runat="server" Text='<%#Eval("TT_8") %>' Visible="false"></asp:Label>
                            </ItemTemplate>
                            <ItemStyle Width="200px" HorizontalAlign="Left" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Period 9">
                            <ItemTemplate>
                                <asp:LinkButton ID="lnkPeriod_9" runat="server" Text='<%#Eval("P9ValDisp") %>' ForeColor="Blue"
                                    OnClick="lnkAttMark" Font-Underline="false"></asp:LinkButton>
                                <asp:Label ID="lblPeriod_9" runat="server" Text='<%#Eval("P9Val") %>' Visible="false"></asp:Label>
                                <asp:Label ID="lblTT_9" runat="server" Text='<%#Eval("TT_9") %>' Visible="false"></asp:Label>
                            </ItemTemplate>
                            <ItemStyle Width="200px" HorizontalAlign="Left" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Period 10">
                            <ItemTemplate>
                                <asp:LinkButton ID="lnkPeriod_10" runat="server" Text='<%#Eval("P10ValDisp") %>'
                                    ForeColor="Blue" OnClick="lnkAttMark" Font-Underline="false"></asp:LinkButton>
                                <asp:Label ID="lblPeriod_10" runat="server" Text='<%#Eval("P10Val") %>' Visible="false"></asp:Label>
                                <asp:Label ID="lblTT_10" runat="server" Text='<%#Eval("TT_10") %>' Visible="false"></asp:Label>
                            </ItemTemplate>
                            <ItemStyle Width="200px" HorizontalAlign="Left" />
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
            </div>
            <asp:Label ID="lbl_alert" runat="server" Font-Names="Book Antiqua" Font-Size="Medium"
                ForeColor="Red" Text="You cannot edit this day/Hour attendance due to security reasons.Contact Inspro Plus Administrator"
                Visible="False"></asp:Label>
            <br />
            <table>
                <tr>
                    <td>
                        <asp:Label ID="lblmanysubject" runat="server" Text="Select Class For Below Details"
                            Font-Size="Medium" Width="225px" Font-Bold="true" Font-Names="Book Antiqua"></asp:Label>
                    </td>
                    <td>
                        <asp:DropDownList ID="ddlselectmanysub" runat="server" AutoPostBack="true" Font-Size="Medium"
                            Font-Names="Book Antiqua" Height="23px" OnSelectedIndexChanged="ddlselectmanysub_SelectedIndexChanged"
                            Width="166px">
                        </asp:DropDownList>
                    </td>
                </tr>
            </table>
            <asp:Panel ID="Panel2" Visible="false" runat="server">
                <asp:RadioButtonList ID="RadioButtonList1" runat="server" RepeatDirection="Horizontal"
                    Visible="false" Height="19px" Width="300px" AutoPostBack="True" OnSelectedIndexChanged="RadioButtonList1_SelectedIndexChanged">
                    <asp:ListItem Value="1" Text="Rollno"></asp:ListItem>
                    <asp:ListItem Value="2" Text="Regno"></asp:ListItem>
                    <asp:ListItem Value="3" Text="Admission no"></asp:ListItem>
                </asp:RadioButtonList>
                <asp:RadioButtonList ID="option" RepeatDirection="Horizontal" runat="server" Height="19px"
                    Width="191px" Visible="False">
                    <asp:ListItem Value="1" Text="General"></asp:ListItem>
                    <asp:ListItem Value="2" Text="Individual"></asp:ListItem>
                </asp:RadioButtonList>
            </asp:Panel>
            <asp:Panel ID="Panel4" Visible="false" runat="server">
                <table>
                    <tr>
                        <td>
                            <asp:Label ID="lblselected" runat="server" Style="font-family: 'Baskerville Old Face'"
                                Text="For the Selected Student" Width="182px" CssClass="style109"></asp:Label>
                        </td>
                        <td class="style113">
                            <asp:DropDownList ID="ddlmark" runat="server" OnSelectedIndexChanged="ddlmark_SelectedIndexChanged"
                                AutoPostBack="true" CssClass="cursorptr">
                            </asp:DropDownList>
                            <asp:Label ID="lblmarkabs" runat="server" Text="Select" Visible="false" ForeColor="Red"
                                Style="font-weight: 400"></asp:Label><asp:Label ID="Label10" runat="server" Text="Should not be same as Rest of the students"
                                    Visible="false" ForeColor="Red" Style="font-weight: 400"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label ID="lblreg" runat="server" Style="font-family: 'Baskerville Old Face';
                                font-weight: 700;" Text="Enter The Roll No" Width="180px" Visible="false"></asp:Label>
                            <asp:Label ID="lblroll" runat="server" Style="font-family: 'Baskerville Old Face'"
                                Text="Enter The Reg No" Visible="false" Width="180px" CssClass="style109"></asp:Label>
                            <asp:Label ID="lblad" runat="server" Visible="false" Style="font-family: 'Baskerville Old Face'"
                                Text="Enter Admission No" Width="180px" CssClass="style109"></asp:Label>
                        </td>
                        <td class="style113">
                            <br />
                            <br />
                            <br />
                            <asp:TextBox ID="txtregno" runat="server" Height="21px" Width="97px" CssClass="style109"
                                onKeyPress="return alpha(event)" AutoPostBack="True" OnTextChanged="txtregno_TextChanged"></asp:TextBox>
                            <asp:TextBox ID="txtrunning" runat="server" Height="21px" onKeyPress="return alpha1(event)"
                                Visible="false" Width="335px" CssClass="style109" AutoPostBack="True" OnTextChanged="txtrunning_TextChanged"></asp:TextBox>
                            &nbsp;<asp:Label ID="lblstate" runat="server" ForeColor="#996633" Style="font-weight: 700"
                                Text="Static Part" Visible="false"></asp:Label>
                            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<asp:Label ID="lblrun" runat="server" ForeColor="#996633"
                                Style="font-weight: 700" Text="Running Part" Visible="false"></asp:Label>
                            &nbsp;&nbsp;&nbsp;
                            <asp:Label ID="lblrunerror" runat="server" ForeColor="Red" Text="Enter Running Part"
                                Visible="False"></asp:Label>
                            <br />
                            <asp:Label ID="lblinvalidreg" runat="server" ForeColor="Red" Visible="false"></asp:Label>
                            <br />
                            &nbsp;<asp:Label ID="lblregno" runat="server" ForeColor="Red" Visible="false"></asp:Label>
                        </td>
                        <td>
                            <asp:Button ID="btngoindividual" runat="server" CssClass="cursorptr" Height="29px"
                                OnClick="btngoindividual_Click" Text="GO" Width="59px" />
                        </td>
                        <td>
                            &nbsp;
                        </td>
                        <td class="style110">
                            <br />
                            <br />
                            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                        </td>
                        <td>
                            <asp:Label ID="lblindisave" runat="server" Text="Saved Successfully" Visible="false"
                                ForeColor="Red" Style="font-weight: 400"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label ID="lblrest" runat="server" Style="font-family: 'Baskerville Old Face'"
                                Text="For Rest of the Students" Width="181px" CssClass="style109"></asp:Label>
                        </td>
                        <td class="style113">
                            <asp:DropDownList ID="ddlmarkothers" runat="server" OnSelectedIndexChanged="ddlmarkothers_SelectedIndexChanged"
                                AutoPostBack="true" CssClass="cursorptr">
                            </asp:DropDownList>
                            <asp:Label ID="markdiff" runat="server" Text="Should not be same as Selected students"
                                Visible="false" ForeColor="Red" Style="font-weight: 400"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td>
                        </td>
                    </tr>
                </table>
            </asp:Panel>
            <asp:Panel ID="pHeaderatendence" Visible="false" runat="server" CssClass="cpHeader"
                Height="19px" Width="953px">
                <asp:Label ID="Labelatend" runat="server" Text="Mark Attendance" Font-Size="Medium"
                    Font-Names="Book Antiqua" />
                <asp:Image ID="ImageSel" runat="server" CssClass="cpimage" ImageUrl="../images/right.jpeg" />
            </asp:Panel>
            <asp:Panel ID="pBodyatendence" runat="server" CssClass="cpBody">
                <asp:Label ID="lbldayorder" runat="server" Text="day" Font-Bold="True" Font-Names="Book Antiqua"
                    Font-Size="Large" ForeColor="#3333CC" Visible="False"></asp:Label>
                <table>
                    <tr>
                        <td>
                            <asp:RadioButton ID="rbgraphics" runat="server" Text="Graphical Display" Font-Bold="True"
                                Font-Names="Book Antiqua" Font-Size="Medium" GroupName="attendance" AutoPostBack="true"
                                OnCheckedChanged="rbgraphics_checkchange" />
                        </td>
                        <td>
                            <asp:RadioButton ID="rbappenses" runat="server" Text="Absent Entry" Font-Bold="True"
                                Font-Names="Book Antiqua" Font-Size="Medium" GroupName="attendance" AutoPostBack="true"
                                OnCheckedChanged="rbappenses_checkchange" />
                        </td>
                        <td>
                            <table>
                                <tr>
                                    <td>
                                        <asp:Label ID="lblreason" runat="server" Text="Reason" Font-Bold="True" Font-Names="Book Antiqua"
                                            Font-Size="Medium"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:Button ID="btnaddreason" runat="server" Text="+" Font-Bold="True" Font-Names="Book Antiqua"
                                            Font-Size="Small" OnClick="btnaddreason_Click" Style="display: none;" />
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="ddlreason" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                            Font-Size="Medium" Height="25px" Width="150px">
                                        </asp:DropDownList>
                                    </td>
                                    <td>
                                        <asp:Button ID="btnremovereason" runat="server" Text="-" OnClick="btnremovereason_Click"
                                            Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium" Style="display: none;" />
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                </table>
                <br />
                <asp:Panel ID="panel1" runat="server" BorderColor="Black" BackColor="AliceBlue" Visible="false"
                    BorderWidth="2px" Height="125px" Width="690px">
                    <div class="panelinfraction" id="Div2" style="text-align: center; font-family: MS Sans Serif;
                        font-size: Small; font-weight: bold">
                        <br />
                        <caption style="top: 30px; border-style: solid; border-color: Black; position: absolute;
                            left: 200px">
                            Add Reason
                        </caption>
                        <br />
                        <br />
                        <table>
                            <tr>
                                <td>
                                    <asp:Label ID="lblatreason" runat="server" Text="Add Reason" Font-Bold="True" Font-Names="Book Antiqua"
                                        Font-Size="Medium"></asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtreason" runat="server" Width="600px" Height="30px" TextMode="MultiLine"
                                        Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                </td>
                                <td>
                                    <asp:Button ID="btnreasonnew" runat="server" Text="Add" OnClick="btnreasonnew_Click"
                                        Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium" />
                                    <asp:Button ID="btnreasonexit" runat="server" Text="Exit" OnClick="btnreasonexit_Click"
                                        Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium" />
                                </td>
                            </tr>
                        </table>
                    </div>
                </asp:Panel>
                <br />
                <div style="margin-left: 0px">
                    <table>
                        <tr>
                            <td>
                                <asp:GridView ID="GridView1" runat="server" Style="margin-bottom: 15px; margin-top: 15px;
                                    width: auto; font-size: 14px" Font-Names="Times New Roman" AutoGenerateColumns="false"
                                    OnRowDataBound="OnRowDataBound" OnDataBound="GridView1_OnDataBound" ShowFooter="true">
                                    <HeaderStyle BackColor="#009999" ForeColor="White" />
                                    <AlternatingRowStyle Height="20px" />
                                    <%-- OnPreRender="GridView1_PreRender"--%>
                                    <Columns>
                                        <asp:TemplateField HeaderText="S.No">
                                            <ItemTemplate>
                                                <%#Container.DataItemIndex+1 %>
                                                <asp:Label ID="lbldegCode" runat="server" Text='<%# Eval("Degree_code") %>' Visible="false" />
                                                <asp:Label ID="lblBatch" runat="server" Text='<%# Eval("Batch_Year") %>' Visible="false" />
                                                <asp:Label ID="lblCurSems" runat="server" Text='<%# Eval("current_semester") %>'
                                                    Visible="false" />
                                                <asp:Label ID="Label6" runat="server" Text='<%# Eval("Sections") %>' Visible="false" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Select" HeaderStyle-HorizontalAlign="Left">
                                            <ItemTemplate>
                                                <asp:DropDownList ID="ddlSelect" runat="server" onchange="selectedExcludedoption(this)">
                                                </asp:DropDownList>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Roll No">
                                            <ItemTemplate>
                                                <asp:Label ID="lblrollNo" runat="server" Text='<%# Eval("roll_no") %>' />
                                                <asp:Label ID="lblDis" runat="server" Text='<%# Eval("delflag") %>' Visible="false" />
                                                <asp:Label ID="lblDebar" runat="server" Text='<%# Eval("exam_flag ") %>' Visible="false" />
                                                <asp:Label ID="lblAppNo" runat="server" Visible="false" Text='<%# Eval("App_no") %>' />
                                                <asp:Label ID="lblCollCode" runat="server" Visible="false" Text='<%# Eval("College_code") %>' />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Reg No">
                                            <ItemTemplate>
                                                <asp:Label ID="lblRegNo" runat="server" Text='<%# Eval("Reg_no") %>' />
                                                <asp:Label ID="lblDate" runat="server" Visible="false" />
                                                <asp:Label ID="lblHR" runat="server" Visible="false" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Admission No">
                                            <ItemTemplate>
                                                <asp:Label ID="lblAdmNo" runat="server" Text='<%# Eval("roll_admit") %>' />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Student Name">
                                            <ItemTemplate>
                                                <asp:Label ID="lblStuName" runat="server" Text='<%# Eval("stud_name") %>' />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Student Type">
                                            <ItemTemplate>
                                                <asp:Label ID="lblStuType" runat="server" Text='<%# Eval("stud_type") %>' />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Con ( Attnd )">
                                            <ItemTemplate>
                                                <asp:Label ID="lblStaff" runat="server" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Period 1">
                                            <HeaderTemplate>
                                                <asp:Label ID="lbl1" runat="server" Text="Period 1" />
                                                <br />
                                                <asp:DropDownList ID="ddlSelectAll" runat="server" onchange="buildDropDown(this);">
                                                </asp:DropDownList>
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <asp:DropDownList ID="ddlLeavetype" runat="server">
                                                </asp:DropDownList>
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Center" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Reason">
                                            <ItemTemplate>
                                                <asp:DropDownList ID="ddlReson" runat="server">
                                                </asp:DropDownList>
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Center" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Period 2">
                                            <HeaderTemplate>
                                                <asp:Label ID="lbl2" runat="server" Text="Period 2" />
                                                <br />
                                                <asp:DropDownList ID="ddlSelectAll2" runat="server" onchange="buildDropDown2(this);">
                                                </asp:DropDownList>
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <asp:DropDownList ID="ddlLeavetype2" runat="server">
                                                </asp:DropDownList>
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Center" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Reason">
                                            <ItemTemplate>
                                                <asp:DropDownList ID="ddlReson2" runat="server">
                                                </asp:DropDownList>
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Center" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Period 3">
                                            <HeaderTemplate>
                                                <asp:Label ID="lbl3" runat="server" Text="Period 3" />
                                                <br />
                                                <asp:DropDownList ID="ddlSelectAll3" runat="server" onchange="buildDropDown3(this);">
                                                </asp:DropDownList>
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <asp:DropDownList ID="ddlLeavetype3" runat="server">
                                                </asp:DropDownList>
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Center" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Reason">
                                            <ItemTemplate>
                                                <asp:DropDownList ID="ddlReson3" runat="server">
                                                </asp:DropDownList>
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Center" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Period 4">
                                            <HeaderTemplate>
                                                <asp:Label ID="lbl4" runat="server" Text="Period 4" />
                                                <br />
                                                <asp:DropDownList ID="ddlSelectAll4" runat="server" onchange="buildDropDown4(this);">
                                                </asp:DropDownList>
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <asp:DropDownList ID="ddlLeavetype4" runat="server">
                                                </asp:DropDownList>
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Center" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Reason">
                                            <ItemTemplate>
                                                <asp:DropDownList ID="ddlReson4" runat="server">
                                                </asp:DropDownList>
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Center" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Period 5">
                                            <HeaderTemplate>
                                                <asp:Label ID="lbl5" runat="server" Text="Period 5" />
                                                <br />
                                                <asp:DropDownList ID="ddlSelectAll5" runat="server" onchange="buildDropDown5(this);">
                                                </asp:DropDownList>
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <asp:DropDownList ID="ddlLeavetype5" runat="server">
                                                </asp:DropDownList>
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Center" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Reason">
                                            <ItemTemplate>
                                                <asp:DropDownList ID="ddlReson5" runat="server">
                                                </asp:DropDownList>
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Center" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Period 6">
                                            <HeaderTemplate>
                                                <asp:Label ID="lbl6" runat="server" Text="Period 6" />
                                                <br />
                                                <asp:DropDownList ID="ddlSelectAll6" runat="server" onchange="buildDropDown6(this);">
                                                </asp:DropDownList>
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <asp:DropDownList ID="ddlLeavetype6" runat="server">
                                                </asp:DropDownList>
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Center" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Reason">
                                            <ItemTemplate>
                                                <asp:DropDownList ID="ddlReson6" runat="server">
                                                </asp:DropDownList>
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Center" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Period 7">
                                            <HeaderTemplate>
                                                <asp:Label ID="lbl7" runat="server" Text="Period 7" />
                                                <br />
                                                <asp:DropDownList ID="ddlSelectAll7" runat="server" onchange="buildDropDown7(this);">
                                                </asp:DropDownList>
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <asp:DropDownList ID="ddlLeavetype7" runat="server">
                                                </asp:DropDownList>
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Center" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Reason">
                                            <ItemTemplate>
                                                <asp:DropDownList ID="ddlReson7" runat="server">
                                                </asp:DropDownList>
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Center" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Period 8">
                                            <HeaderTemplate>
                                                <asp:Label ID="lbl8" runat="server" Text="Period 8" />
                                                <br />
                                                <asp:DropDownList ID="ddlSelectAll8" runat="server" onchange="buildDropDown8(this);">
                                                </asp:DropDownList>
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <asp:DropDownList ID="ddlLeavetype8" runat="server">
                                                </asp:DropDownList>
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Center" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Reason">
                                            <ItemTemplate>
                                                <asp:DropDownList ID="ddlReson8" runat="server">
                                                </asp:DropDownList>
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Center" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Period 9">
                                            <HeaderTemplate>
                                                <asp:Label ID="lbl9" runat="server" Text="Period 9" />
                                                <br />
                                                <asp:DropDownList ID="ddlSelectAll9" runat="server" onchange="buildDropDown9(this);">
                                                </asp:DropDownList>
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <asp:DropDownList ID="ddlLeavetype9" runat="server">
                                                </asp:DropDownList>
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Center" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Reason">
                                            <ItemTemplate>
                                                <asp:DropDownList ID="ddlReson9" runat="server">
                                                </asp:DropDownList>
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Center" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Period 10">
                                            <HeaderTemplate>
                                                <asp:Label ID="lbl10" runat="server" Text="Period 10" />
                                                <br />
                                                <asp:DropDownList ID="ddlSelectAll10" runat="server" onchange="buildDropDown10(this);">
                                                </asp:DropDownList>
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <asp:DropDownList ID="ddlLeavetype10" runat="server">
                                                </asp:DropDownList>
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Center" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Reason">
                                            <ItemTemplate>
                                                <asp:DropDownList ID="ddlReson10" runat="server">
                                                </asp:DropDownList>
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Center" />
                                        </asp:TemplateField>
                                    </Columns>
                                    <FooterStyle Font-Bold="True" ForeColor="Black" />
                                    <%--  <HeaderStyle CssClass="GridHeaderFixed" />--%>
                                </asp:GridView>
                            </td>
                        </tr>
                    </table>
                </div>
                <br />
                <br />
                <div id="divatt" runat="server" style="margin-left: 450px">
                    <table id="tableat" style="text-align: center">
                        <tr>
                            <%--@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@PRABHA On 13/6/12@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@--%>
                            <%--@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@--%>
                            <td>
                                <asp:CheckBox ID="check_attendance" runat="server" Text="Copy Attendance" Font-Size="small"
                                    Font-Names="Book Antiqua" Font-Bold="true" />
                            </td>
                            <td>
                                <asp:Button ID="Buttonselectall" Visible="false" CssClass="floats" Font-Bold="true"
                                    runat="server" Text="Select All" OnClientClick="SelectDropDown()" />
                                <%--OnClick="Buttonselectall_Click"--%>
                            </td>
                            <td>
                                <asp:Button ID="Buttondeselect" Visible="false" Font-Bold="true" runat="server" CssClass="floats"
                                    Text="De-Select All" OnClientClick="DeSelectDropDown()" />
                            </td>
                            <td>
                                <asp:Button ID="Buttonsave" Visible="false" Font-Bold="true" runat="server" CssClass="floats"
                                    Text="Save" OnClick="Buttonsave_Click" />
                            </td>
                            <td>
                                <%--<asp:Button ID="Buttonupdate" Visible="false" Font-Bold="true" runat="server" CssClass="floats"
                                    Text="Update" OnClick="Buttonupdate_Click" />--%>
                            </td>
                            <td>
                                <asp:Button ID="Buttonexit" runat="server" Font-Bold="true" Visible="false" CssClass="floats"
                                    Text="Exit" OnClick="Buttonexit_Click" />
                            </td>
                        </tr>
                    </table>
                </div>
                <div id="Slipentry" runat="server" visible="false">
                    <fieldset id="fieldat" runat="server" style="width: 500px; height: 430px">
                        <table style="text-align: right;">
                            <tr>
                                <td>
                                    <asp:Label ID="lblatdate" runat="server" Text="Date :" Font-Bold="True" Font-Names="Book Antiqua"
                                        Font-Size="Medium"></asp:Label>
                                </td>
                                <td>
                                    <asp:Label ID="lblcurdate" runat="server" Text="" Font-Bold="True" Font-Names="Book Antiqua"
                                        Font-Size="Medium"></asp:Label>
                                </td>
                                <td>
                                    <asp:Label ID="lblhour" Text="Hour(s) :" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                        Font-Size="Medium"></asp:Label>
                                </td>
                                <td>
                                    <asp:Label ID="lblhrvalue" Text="" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                        Font-Size="Medium"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="lblattend" runat="server" Text="Selected Students :" Font-Bold="True"
                                        Font-Names="Book Antiqua" Font-Size="Medium"></asp:Label>
                                </td>
                                <td>
                                    <asp:DropDownList ID="ddlattend" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                        Font-Size="Medium">
                                    </asp:DropDownList>
                                </td>
                            </tr>
                        </table>
                        <table>
                            <%--<tr>
                            <td>
                                <asp:Button ID="btnaddrow" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                    Font-Size="Medium" Text="Add Row" OnClick="btnaddrow_Click" />
                            </td>
                        </tr>--%>
                            <tr>
                                <td>
                                    <asp:GridView ID="Gridview2" runat="server" ShowFooter="true" AutoGenerateColumns="false">
                                        <Columns>
                                            <asp:TemplateField HeaderText="Roll Prefix">
                                                <ItemTemplate>
                                                    <asp:TextBox ID="TextBox1" runat="server" Text='<%# Eval("Column1") %>'></asp:TextBox>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Roll No of the Student">
                                                <ItemTemplate>
                                                    <asp:TextBox ID="TextBox2" runat="server" Text='<%# Eval("Column2") %>'></asp:TextBox>
                                                </ItemTemplate>
                                                <FooterStyle HorizontalAlign="Right" />
                                                <FooterTemplate>
                                                    <asp:Button ID="ButtonAdd" runat="server" Text="Add New Row" OnClick="addnewrow" />
                                                </FooterTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                    </asp:GridView>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="lblreststudent" runat="server" Text="For The Rest Of Students" Font-Bold="True"
                                        Font-Names="Book Antiqua" Font-Size="Medium"></asp:Label>
                                </td>
                                <td>
                                    <asp:DropDownList ID="ddlreststudent" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                        Font-Size="Medium">
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="lblerrmsg" runat="server" Font-Bold="true" ForeColor="Red" CssClass="floats"
                                        Font-Names="Book Antiqua" Font-Size="Medium"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Button ID="btnaddattendance" runat="server" Text="Save" OnClick="btnaddattendance_Click"
                                        Font-Bold="True" Font-Names="Book Antiqua" Font-Size="Medium" />
                                </td>
                            </tr>
                        </table>
                    </fieldset>
                </div>
                <asp:CollapsiblePanelExtender ID="cpeatend" runat="server" TargetControlID="pBodyatendence"
                    CollapseControlID="pHeaderatendence" ExpandControlID="pHeaderatendence" Collapsed="true"
                    TextLabelID="Labelatend" CollapsedSize="0" ImageControlID="ImageSel" CollapsedImage="../images/right.jpeg"
                    ExpandedImage="../images/down.jpeg">
                </asp:CollapsiblePanelExtender>
            </asp:Panel>
            <br />
            <asp:Panel ID="pHeaderlesson" Visible="false" runat="server" CssClass="cpHeader"
                Height="19px" Width="953px">
                <asp:Label ID="Labellesson" runat="server" Text="Daily Entry" Font-Size="Medium"
                    Font-Names="Book Antiqua" />
                <asp:Image ID="Image1" runat="server" CssClass="cpimage" ImageUrl="../images/right.jpeg" />
            </asp:Panel>
            <asp:Panel ID="pBodylesson" runat="server" CssClass="cpBody">
                <asp:Label ID="Labellvalid" Visible="False" runat="server" Text="Label" Font-Names="Book Antiqua"
                    Font-Size="Medium" ForeColor="Red"></asp:Label>
                <table>
                    <tr>
                        <td>
                            <asp:Panel ID="Panelcomplete" Visible="false" runat="server" Height="338px" Width="312px"
                                ScrollBars="Auto" BorderWidth="1px">
                                <center>
                                    <asp:Label ID="Labelc" runat="server" Text="Topics Completed" Font-Bold="True" Font-Names="Book Antiqua"
                                        Font-Size="Medium"></asp:Label>
                                </center>
                                <asp:TreeView ID="tvcomplete" ForeColor="Blue" runat="server" BorderWidth="0px" SelectedNodeStyle-ForeColor="Red"
                                    ShowCheckBoxes="Leaf" Font-Names="Book Antiqua" Font-Size="Medium">
                                    <HoverNodeStyle ForeColor="Black" />
                                    <Nodes>
                                        <asp:TreeNode Expanded="True" SelectAction="Expand"></asp:TreeNode>
                                    </Nodes>
                                </asp:TreeView>
                            </asp:Panel>
                        </td>
                        <td>
                            <asp:Panel ID="Panelyet" Visible="false" runat="server" Height="335px" Width="312px"
                                ScrollBars="Auto" BorderWidth="1px">
                                <center>
                                    <asp:Label ID="Label2" runat="server" Text="Topics Yet To Complete" Font-Bold="True"
                                        Font-Names="Book Antiqua" Font-Size="Medium"></asp:Label>
                                </center>
                                <asp:TreeView ID="tvyet" runat="server" ShowCheckBoxes="Leaf" BorderWidth="0px" OnTreeNodeCheckChanged="OnTreeNodeCheckChanged"
                                    Font-Names="Book Antiqua" Font-Size="Medium" OnTreeNodeExpanded="OnTreeNodeCheckChanged"
                                    OnSelectedNodeChanged="tvyet_SelectedNodeChanged1">
                                </asp:TreeView>
                            </asp:Panel>
                        </td>
                        <td>
                            <asp:Panel ID="Plessionalter" Visible="true" runat="server" Height="335px" Width="300px"
                                ScrollBars="Auto" BorderWidth="1px">
                                <center>
                                    <asp:Label ID="Label5" runat="server" Text="Previous Days Lession Plan Topics" Font-Bold="True"
                                        Font-Names="Book Antiqua" Font-Size="Medium"></asp:Label>
                                </center>
                                <asp:TreeView ID="tvalterlession" runat="server" ShowCheckBoxes="Leaf" BorderWidth="0px"
                                    OnTreeNodeCheckChanged="OnTreeNodeCheckChanged" Font-Names="Book Antiqua" Font-Size="Medium"
                                    OnTreeNodeExpanded="OnTreeNodeCheckChanged" OnSelectedNodeChanged="tvyet_SelectedNodeChanged1">
                                </asp:TreeView>
                            </asp:Panel>
                        </td>
                    </tr>
                </table>
                <br />
                <table width="850px">
                    <tr>
                        <td>
                            <asp:Label ID="Label8" runat="server" Text="Other Topics" Font-Bold="True" Font-Names="Book Antiqua"
                                Font-Size="Medium"></asp:Label>
                            <asp:Button ID="Btnadd" runat="server" Visible="true" Text="+" Font-Bold="true" OnClick="Btnadd_Click"
                                Height="31px" />
                            <asp:DropDownList ID="ddlother" runat="server" Font-Bold="True" Font-Names="Book Antiqua"
                                Width="500px" Font-Size="Medium" OnSelectedIndexChanged="ddlother_selected" AutoPostBack="true"
                                Height="36px">
                            </asp:DropDownList>
                            <asp:Button ID="Btndel" runat="server" Visible="true" Text="-" Font-Bold="true" OnClick="Btndel_Click"
                                Height="31px" Width="33px" />
                            <asp:Button ID="Button4" Font-Bold="true" Font-Size="Medium" Font-Names="Book Antiqua"
                                CssClass="textbox textbox1" Style="height: auto; width: auto;" OnClick="btnsaves"
                                Text="Save" runat="server" Visible="false" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Button ID="btndailyentrydelete" runat="server" Visible="true" Text="Delete"
                                Font-Bold="true" OnClick="btndailyentrydelete_Click" />
                            <asp:Button ID="Buttonexitlesson" runat="server" Visible="false" CssClass="floats"
                                Text="Exit" Font-Bold="true" OnClick="Buttonexitlesson_Click" />
                            <asp:Button ID="Buttonsavelesson" Visible="false" runat="server" CssClass="floats"
                                Font-Bold="true" Text="Save" OnClick="Buttonsavelesson_Click" />
                            <asp:CheckBox ID="chkalterlession" runat="server" AutoPostBack="true" Font-Names="Book Antiqua"
                                Font-Size="Medium" Text="Previous Days Lession Plan Topics" Font-Bold="true"
                                OnCheckedChanged="chkalterlession_CheckedChanged" />
                        </td>
                    </tr>
                </table>
                <asp:CollapsiblePanelExtender ID="cpelesson" runat="server" TargetControlID="pBodylesson"
                    CollapseControlID="pHeaderlesson" ExpandControlID="pHeaderlesson" Collapsed="true"
                    TextLabelID="Labellesson" CollapsedSize="0" ImageControlID="Image1" CollapsedImage="../images/right.jpeg"
                    ExpandedImage="../images/down.jpeg">
                </asp:CollapsiblePanelExtender>
            </asp:Panel>
            <br />
            <asp:Panel ID="headerpanelnotes" Visible="false" runat="server" CssClass="cpHeader"
                Height="19px" Width="953px">
                <asp:Label ID="lblnotes" runat="server" Text="Notes" Font-Size="Medium" Font-Names="Book Antiqua" />
                <asp:Image ID="Image2" runat="server" CssClass="cpimage" ImageUrl="../images/right.jpeg" />
            </asp:Panel>
            <asp:Panel ID="pBodynotes" runat="server" CssClass="cpBody">
                <table>
                    <tr>
                        <td>
                            <asp:Button ID="btnSave" runat="server" Font-Bold="true" Text="Add Notes" CssClass="floats"
                                Font-Size="Medium" Font-Names="Book Antiqua" OnClick="btnSave_Click" />
                        </td>
                        <td>
                            <asp:Button ID="btnnotesdelete" runat="server" Font-Bold="true" Text="Delete" CssClass="floats"
                                Font-Size="Medium" Font-Names="Book Antiqua" Visible="false" />
                        </td>
                        <td>
                            <asp:Label ID="lblerror" runat="server" Text="" Visible="false" ForeColor="Red" CssClass="floats"></asp:Label>
                        </td>
                    </tr>
                </table>
                <asp:Panel runat="server" ID="fpspread3panel">
                    <table>
                        <tr>
                            <td>
                                <asp:GridView ID="GridView3" runat="server" Style="margin-bottom: 15px; margin-top: 15px;
                                    width: auto; font-size: 14px" Font-Names="Times New Roman" AutoGenerateColumns="false">
                                    <HeaderStyle BackColor="#009999" ForeColor="White" />
                                    <%-- OnRowDataBound="GridView3_OnRowDataBound" OnDataBound="GridView3_OnDataBound"--%>
                                    <AlternatingRowStyle Height="20px" />
                                    <%-- OnPreRender="GridView1_PreRender"--%>
                                    <Columns>
                                        <asp:TemplateField HeaderText="S.No">
                                            <ItemTemplate>
                                                <%#Container.DataItemIndex+1 %>
                                                <asp:Label ID="lblBatchYear" runat="server" Visible="false" Text='<%# Eval("Batch") %>' />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Date" HeaderStyle-HorizontalAlign="Left">
                                            <ItemTemplate>
                                                <asp:Label ID="lblDate" runat="server" Text='<%# Eval("date1") %>' />
                                                <asp:Label ID="lblDegCode" runat="server" Visible="false" Text='<%# Eval("degCode") %>' />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Subject">
                                            <ItemTemplate>
                                                <asp:Label ID="lblSubject" runat="server" Text='<%# Eval("subName") %>' />
                                                <asp:Label ID="lblSem" runat="server" Visible="false" Text='<%# Eval("sem") %>' />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Topic">
                                            <ItemTemplate>
                                                <asp:Label ID="lblTopic" runat="server" Text='<%# Eval("topic") %>' />
                                                <asp:Label ID="lblSubjectNo" runat="server" Visible="false" Text='<%# Eval("subNo") %>' />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Download">
                                            <ItemTemplate>
                                                <asp:LinkButton ID="lnkDownload" runat="server" Text='<%#Eval("path") %>' ForeColor="Blue"
                                                    OnClick="lnkDownload_click" Font-Underline="false"></asp:LinkButton>
                                                <asp:Label ID="lblPath" runat="server" Text='<%# Eval("path") %>' Visible="false" />
                                                <asp:Label ID="lblPathTag" runat="server" Visible="false" Text='<%# Eval("pathtag") %>' />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Delete">
                                            <ItemTemplate>
                                                <asp:LinkButton ID="lnkDelete" runat="server" Text='<%#Eval("path") %>' ForeColor="Blue"
                                                    OnClick="lnkDelete_click" Font-Underline="false"></asp:LinkButton>
                                                <asp:Label ID="lblPathdel" runat="server" Text='<%# Eval("path") %>' Visible="false" />
                                                <asp:Label ID="lblPathTagdel" runat="server" Visible="false" Text='<%# Eval("pathtag") %>' />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                </asp:GridView>
                            </td>
                        </tr>
                    </table>
                </asp:Panel>
            </asp:Panel>
            <asp:CollapsiblePanelExtender ID="cpenotes" runat="server" TargetControlID="pBodynotes"
                CollapseControlID="headerpanelnotes" ExpandControlID="headerpanelnotes" Collapsed="true"
                TextLabelID="lblnotes" CollapsedSize="0" ImageControlID="Image2" CollapsedImage="../images/right.jpeg"
                ExpandedImage="../images/down.jpeg">
            </asp:CollapsiblePanelExtender>
            <br />
            <asp:Panel ID="headerADDQuestion" Visible="false" runat="server" CssClass="cpHeader"
                Height="19px" Width="953px">
                <asp:Label ID="lblADDquestion" runat="server" Text="Add Question" Font-Size="Medium"
                    Font-Names="Book Antiqua" />
                <asp:Image ID="Image3" runat="server" CssClass="cpimage" ImageUrl="../images/right.jpeg" />
            </asp:Panel>
            <asp:Panel ID="pBodyaddquestion" runat="server" CssClass="cpBody">
                <table>
                    <tr>
                        <td>
                            <asp:Label ID="lblunits" runat="server" Text="Units" Font-Bold="True" Font-Names="Book Antiqua"
                                Font-Size="Medium"></asp:Label>
                        </td>
                        <td>
                            <asp:DropDownList ID="ddlunits" runat="server" Width="100px" Font-Bold="True" Font-Names="Book Antiqua"
                                Font-Size="Medium">
                            </asp:DropDownList>
                        </td>
                        <td>
                            <asp:Label ID="lblquestion1" runat="server" Text="Question" Font-Bold="True" Font-Names="Book Antiqua"
                                Font-Size="Medium"></asp:Label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtquestion1" runat="server" Font-Bold="True" Width="400px" Font-Names="Book Antiqua"
                                Font-Size="Medium"></asp:TextBox>
                        </td>
                        <td>
                            <asp:Label ID="lblMarks" runat="server" Text="Marks" Font-Bold="True" Font-Names="Book Antiqua"
                                Font-Size="Medium"></asp:Label>
                        </td>
                        <td>
                            <asp:DropDownList ID="ddlgivemarks" runat="server" Font-Bold="true" Font-Names="Book Antiqua"
                                Font-Size="Medium" Width="40px">
                                <asp:ListItem>1</asp:ListItem>
                                <asp:ListItem>2</asp:ListItem>
                                <asp:ListItem>3</asp:ListItem>
                                <asp:ListItem>4</asp:ListItem>
                                <asp:ListItem>5</asp:ListItem>
                                <asp:ListItem>6</asp:ListItem>
                                <asp:ListItem>7</asp:ListItem>
                                <asp:ListItem>8</asp:ListItem>
                                <asp:ListItem>9</asp:ListItem>
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
                            </asp:DropDownList>
                        </td>
                        <td>
                            <asp:Button ID="btnaddquestion" runat="server" Text="Save" Font-Bold="True" Font-Names="Book Antiqua"
                                Font-Size="Medium" OnClick="btnaddquestion_Click" />
                        </td>
                        <td>
                            <asp:Button ID="btnupdatequetion" runat="server" Visible="false" Text="Update" Font-Bold="True"
                                Font-Names="Book Antiqua" Font-Size="Medium" OnClick="btnupdatequetion_Click" />
                        </td>
                        <td>
                            <asp:Button ID="btndeleteatndqtn" runat="server" Visible="false" Text="Delete" Font-Bold="True"
                                Font-Names="Book Antiqua" Font-Size="Medium" />
                        </td>
                        <td>
                            <asp:Label ID="lblerrorquestionadd_att" runat="server" Text="" ForeColor="Red" Font-Bold="true"
                                Font-Size="Medium" Font-Names="Book Antiqua"></asp:Label>
                        </td>
                    </tr>
                </table>
                <asp:Panel runat="server" ID="spreadttqtnadd">
                    <table>
                        <tr>
                            <td>
                                <asp:GridView ID="GridView6" runat="server" Style="margin-bottom: 15px; margin-top: 15px;
                                    width: auto; font-size: 14px" Font-Names="Times New Roman" AutoGenerateColumns="false">
                                    <HeaderStyle BackColor="#009999" ForeColor="White" />
                                    <%-- OnRowDataBound="GridView3_OnRowDataBound" OnDataBound="GridView3_OnDataBound"--%>
                                    <AlternatingRowStyle Height="20px" />
                                    <%-- OnPreRender="GridView1_PreRender"--%>
                                    <Columns>
                                        <asp:TemplateField HeaderText="S.No" HeaderStyle-HorizontalAlign="Left">
                                            <ItemTemplate>
                                                <asp:Label ID="lblSno" runat="server" Text='<%# Eval("Sno") %>' />
                                                <asp:Label ID="lblBatch" runat="server" Text='<%# Eval("bacth") %>' Visible="false" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Date">
                                            <ItemTemplate>
                                                <asp:Label ID="lblDate" runat="server" Text='<%# Eval("Date1") %>' />
                                                <asp:Label ID="lblDeg" runat="server" Text='<%# Eval("degree") %>' Visible="false" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Subject">
                                            <ItemTemplate>
                                                <asp:TextBox ID="lblSubject" runat="server" Text='<%# Eval("subject") %>'></asp:TextBox>
                                                <asp:Label ID="lblSuNo" runat="server" Text='<%# Eval("subNo") %>' Visible="false" />
                                                <asp:Label ID="lblSem" runat="server" Text='<%# Eval("sem") %>' Visible="false" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Units">
                                            <ItemTemplate>
                                                <asp:TextBox ID="lblUnit" runat="server" Text='<%# Eval("unit") %>'></asp:TextBox>
                                                <asp:Label ID="lblUnitNo" runat="server" Text='<%# Eval("unitNo") %>' Visible="false" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Question">
                                            <ItemTemplate>
                                                <asp:TextBox ID="lblQen" runat="server" Text='<%# Eval("qen") %>'></asp:TextBox>
                                                <asp:Label ID="lblQno" runat="server" Text='<%# Eval("qno") %>' Visible="false" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Marks">
                                            <ItemTemplate>
                                                <asp:TextBox ID="lblMark" runat="server" Text='<%# Eval("mark") %>'></asp:TextBox>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Delete">
                                            <ItemTemplate>
                                                <asp:LinkButton ID="lnkGDelete" runat="server" Text="Delete" ForeColor="Blue" OnClick="lnkGDelete_click"
                                                    Font-Underline="false"></asp:LinkButton>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                </asp:GridView>
                            </td>
                        </tr>
                    </table>
                </asp:Panel>
            </asp:Panel>
            <asp:CollapsiblePanelExtender ID="CollapsiblePanelExtender1" runat="server" TargetControlID="pBodyaddquestion"
                CollapseControlID="headerADDQuestion" ExpandControlID="headerADDQuestion" Collapsed="true"
                TextLabelID="lblADDquestion" CollapsedSize="0" ImageControlID="Image3" CollapsedImage="../images/right.jpeg"
                ExpandedImage="../images/down.jpeg">
            </asp:CollapsiblePanelExtender>
            <br />
            <asp:Panel ID="headerquestionaddition" Visible="false" runat="server" CssClass="cpHeader"
                Height="19px" Width="953px">
                <asp:Label ID="lblquestionaddition" runat="server" Text="Objective Type" Font-Size="Medium"
                    Font-Names="Book Antiqua" />
                <asp:Image ID="Image4" runat="server" CssClass="cpimage" ImageUrl="../images/right.jpeg" />
                &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp;&nbsp; &nbsp; &nbsp;&nbsp; &nbsp; &nbsp;&nbsp;
                &nbsp; &nbsp;
                <asp:RadioButton ID="RadioSubject" runat="server" GroupName="Question" AutoPostBack="true"
                    Text="Subject" CssClass="label" OnCheckedChanged="RadioSubject_CheckedChanged"
                    Checked="true" />
                <asp:RadioButton ID="RadioGeneral" runat="server" GroupName="Question" AutoPostBack="true"
                    Text="General" CssClass="label" OnCheckedChanged="RadioGeneral_CheckedChanged" />
            </asp:Panel>
            <asp:Panel ID="pBodyquestionaddition" runat="server" CssClass="cpBody" BorderColor="Gray"
                BackImageUrl="~/StudentImage/Box.jpg" BorderWidth="2px" Height="300px" Width="1000px">
                <asp:Panel ID="paneltoaddquestion" runat="server">
                    <table>
                        <tr>
                            <td colspan="2">
                                <asp:Label ID="lblnoofanswers" runat="server" Text="No of Answers" CssClass="label"></asp:Label>
                                <asp:DropDownList ID="ddlnoofanswers" runat="server" CssClass="font" AutoPostBack="true "
                                    Width="40px" OnSelectedIndexChanged="ddlnoofanswers_SelectedIndexchanged">
                                    <asp:ListItem Value="A" Text=""></asp:ListItem>
                                    <asp:ListItem Value="B">2</asp:ListItem>
                                    <asp:ListItem Value="C">3</asp:ListItem>
                                    <asp:ListItem Value="D">4</asp:ListItem>
                                    <asp:ListItem Value="E">5</asp:ListItem>
                                    <asp:ListItem Value="F">6</asp:ListItem>
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="lblqtnName" runat="server" CssClass="label" Text="Question Name"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2">
                                <asp:TextBox ID="txtqtnname" runat="server" CssClass="font" Width="900px"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="lblAnswers" runat="server" CssClass="label" Text="Answers"></asp:Label>
                                &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp;&nbsp; &nbsp; &nbsp;&nbsp; &nbsp; &nbsp;&nbsp;
                                &nbsp; &nbsp;
                                <asp:Label ID="lblcorrectans" runat="server" CssClass="label" Text="Check Correct Answers(any one)"></asp:Label>
                                <asp:Label ID="lblcompulsarymark" runat="server" ForeColor="Red" CssClass="label"
                                    Text="*"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:GridView ID="GridView4" runat="server" Style="margin-bottom: 15px; margin-top: 15px;
                                    width: auto; font-size: 14px" Font-Names="Times New Roman" AutoGenerateColumns="false">
                                    <HeaderStyle BackColor="#009999" ForeColor="White" />
                                    <%-- OnRowDataBound="GridView3_OnRowDataBound" OnDataBound="GridView3_OnDataBound"--%>
                                    <AlternatingRowStyle Height="20px" />
                                    <%-- OnPreRender="GridView1_PreRender"--%>
                                    <Columns>
                                        <asp:TemplateField HeaderText="Choice" HeaderStyle-HorizontalAlign="Left">
                                            <ItemTemplate>
                                                <asp:Label ID="lbloption" runat="server" Text='<%# Eval("option") %>' />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Answer">
                                            <ItemTemplate>
                                                <asp:CheckBox ID="Option" runat="server" Checked='<%# Eval("ischecked").ToString().Equals("1") %>' />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Option">
                                            <ItemTemplate>
                                                <asp:TextBox ID="opttxt" runat="server" Text='<%# Eval("Val") %>'></asp:TextBox>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                </asp:GridView>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2">
                                <asp:Label ID="lbltoughness" runat="server" Text="Toughness" CssClass="label"></asp:Label>
                                &nbsp;&nbsp;&nbsp;&nbsp;
                                <asp:RadioButton ID="radiotough1" runat="server" GroupName="tough" CssClass="label"
                                    Text="Easy" Font-Bold="True" Checked="true" />
                                &nbsp;
                                <asp:RadioButton ID="radiotough2" runat="server" GroupName="tough" CssClass="label"
                                    Text="Medium" Font-Bold="True" />
                                &nbsp;
                                <asp:RadioButton ID="radiotough3" runat="server" GroupName="tough" CssClass="label"
                                    Text="Difficult" Font-Bold="True" />
                                &nbsp;
                                <asp:RadioButton ID="radiotough4" runat="server" GroupName="tough" CssClass="label"
                                    Text="Very Difficult" Font-Bold="True" />
                            </td>
                        </tr>
                    </table>
                </asp:Panel>
                <asp:Panel ID="panelsavecommands" runat="server">
                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                    <table id="Table3" runat="server" align="center">
                        <tr>
                            <td>
                                <asp:Button ID="btnqtnsave" runat="server" Text="Save" Font-Bold="true" Font-Size="Medium"
                                    Font-Names="Book Antiqua" OnClick="btnqtnsave_Click" />
                            </td>
                            <td>
                                <asp:Button ID="btnqtnupdate" runat="server" Text="Update" Font-Bold="true" Font-Size="Medium"
                                    Font-Names="Book Antiqua" OnClick="btnqtnupdate_Click" Visible="false" />
                            </td>
                            <td>
                                <asp:Button ID="btnNew" runat="server" Text="New" Font-Bold="true" Font-Size="Medium"
                                    Font-Names="Book Antiqua" OnClick="btnNew_Click" />
                            </td>
                            <td>
                                <asp:Button ID="btnqtndelete" runat="server" Text="Delete" Font-Bold="true" Font-Size="Medium"
                                    Font-Names="Book Antiqua" OnClick="btnqtndelete_Click" />
                            </td>
                            <td>
                                <asp:Label ID="lblnorec" runat="server" Text="No records Found" Visible="false" CssClass="label"
                                    ForeColor="Red"></asp:Label>
                            </td>
                        </tr>
                    </table>
                    <asp:Panel ID="sprdviewdatapanel" runat="server">
                        <table>
                            <tr>
                                <td>
                                    <asp:GridView ID="GridView5" runat="server" Style="margin-bottom: 15px; margin-top: 15px;
                                        width: auto; font-size: 14px" Font-Names="Times New Roman" AutoGenerateColumns="false">
                                        <HeaderStyle BackColor="#009999" ForeColor="White" />
                                        <%-- OnRowDataBound="GridView3_OnRowDataBound" OnDataBound="GridView3_OnDataBound"--%>
                                        <AlternatingRowStyle Height="20px" />
                                        <%-- OnPreRender="GridView1_PreRender"--%>
                                        <Columns>
                                            <asp:TemplateField HeaderText="Q No " HeaderStyle-HorizontalAlign="Left">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblQNo" runat="server" Text='<%# Eval("qno") %>' />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Questions">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblQun" runat="server" Text='<%# Eval("qun") %>' />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Answers">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblAns" runat="server" Text='<%# Eval("ans") %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Answers">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblCans" runat="server" Text='<%# Eval("cAns") %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                    </asp:GridView>
                                </td>
                            </tr>
                        </table>
                    </asp:Panel>
                </asp:Panel>
                <
            </asp:Panel>
            </asp:Panel>
            <asp:CollapsiblePanelExtender ID="CollapsiblePanelExtender2" runat="server" TargetControlID="pBodyquestionaddition"
                CollapseControlID="headerquestionaddition" ExpandControlID="headerquestionaddition"
                Collapsed="true" TextLabelID="lblquestionaddition" CollapsedSize="0" ImageControlID="Image4"
                CollapsedImage="../images/right.jpeg" ExpandedImage="../images/down.jpeg">
            </asp:CollapsiblePanelExtender>
            <div>
                <asp:Panel ID="pnl_sliplist" runat="server" BackColor="AliceBlue" Height="200" Width="670"
                    Style="top: 492px; left: 190px; position: absolute; height: 200px; width: 600"
                    BorderColor="Black" BorderStyle="Double">
                    <table>
                        <tr>
                            <td class="style4" colspan="2">
                                <center>
                                    <asp:Label ID="headlbl_sl" runat="server" Text="Pending Slip List" Font-Bold="True"
                                        Font-Names="Book Antiqua" Font-Size="Medium"></asp:Label>
                                </center>
                            </td>
                        </tr>
                        <tr>
                            <td class="style4">
                            </td>
                        </tr>
                        <tr>
                            <td class="style4">
                            </td>
                        </tr>
                    </table>
                    <table>
                        <tr>
                            <td class="style2">
                                <center>
                                    <%--   <FarPoint:FpSpread ID="spread_sliplist" runat="server" BorderColor="Black" BorderStyle="Solid"
                                BorderWidth="1px" Height="200" Width="600" CommandBar-Visible="false" ShowHeaderSelection="false">
                                <CommandBar BackColor="Control" ButtonFaceColor="Control" ButtonHighlightColor="ControlLightLight"
                                    ButtonShadowColor="ControlDark">
                                </CommandBar>
                                <Sheets>
                                    <FarPoint:SheetView SheetName="Sheet1" AutoPostBack="true" GridLineColor="Black">
                                    </FarPoint:SheetView>
                                </Sheets>
                            </FarPoint:FpSpread>--%>
                                    <%-- <asp:GridView ID="GridView2" runat="server" Style="margin-bottom: 15px; margin-top: 15px;
                                        width: auto; font-size: 14px" Font-Names="Times New Roman" AutoGenerateColumns="false">
                                        <HeaderStyle BackColor="#009999" ForeColor="White" />
                                        <AlternatingRowStyle Height="20px" />
                                        <Columns>
                                            <asp:TemplateField HeaderText="S.No " HeaderStyle-HorizontalAlign="Left">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblSNo" runat="server" Text='<%# Eval("sno") %>' />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Date">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblDate" runat="server" Text='<%# Eval("Date") %>' />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Hour">
                                                <ItemTemplate>
                                                    <asp:TextBox ID="lblHour" runat="server" Text='<%# Eval("Hour") %>'></asp:TextBox>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Staff Name">
                                                <ItemTemplate>
                                                    <asp:TextBox ID="lblStaffName" runat="server" Text='<%# Eval("StaffName") %>'></asp:TextBox>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                             <asp:TemplateField HeaderText="Degree">
                                                <ItemTemplate>
                                                    <asp:TextBox ID="lblDegree" runat="server" Text='<%# Eval("Degree") %>'></asp:TextBox>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                    </asp:GridView>--%>
                                </center>
                            </td>
                        </tr>
                        <tr>
                            <td class="style2">
                            </td>
                            <td>
                                <asp:Button ID="exit_sliplist" runat="server" Text="Exit" Font-Bold="True" Font-Names="Book Antiqua"
                                    Font-Size="Medium" OnClick="exit_sliplist_Click" />
                            </td>
                        </tr>
                    </table>
                </asp:Panel>
            </div>
            <%-- Confirmation --%>
            <center>
                <div id="divConfirmBox" runat="server" visible="false" style="height: 550em; z-index: 1000;
                    width: 100%; background-color: rgba(54, 25, 25, .2); position: absolute; top: 0;
                    left: 0px;">
                    <center>
                        <div id="divConfirm" runat="server" class="table" style="background-color: White;
                            height: auto; width: 38%; border: 5px solid #0CA6CA; border-top: 25px solid #0CA6CA;
                            left: 30%; right: 30%; top: 40%; position: fixed; border-radius: 10px;">
                            <center>
                                <table style="height: auto; width: 100%; padding: 3px;">
                                    <tr>
                                        <td align="center">
                                            <asp:Label ID="lblConfirmMsg" runat="server" Text="Do You Want To Delete All Subject Remarks?"
                                                Style="color: Red;" Font-Bold="true" Font-Size="Medium"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <center>
                                                <asp:Button ID="btnYes" CssClass=" textbox btn1 textbox1" Style="height: 28px; width: 65px;"
                                                    OnClick="btnYes_Click" Text="Yes" runat="server" />
                                                <asp:Button ID="btnNo" CssClass=" textbox btn1 textbox1" Style="height: 28px; width: 65px;"
                                                    OnClick="btnNo_Click" Text="No" runat="server" />
                                            </center>
                                        </td>
                                    </tr>
                                </table>
                            </center>
                        </div>
                    </center>
                </div>
            </center>
            <%-- Alert Box --%>
            <center>
                <div id="divPopAlert" runat="server" visible="false" style="height: 550em; z-index: 2000;
                    width: 100%; background-color: rgba(54, 25, 25, .2); position: absolute; top: 0%;
                    left: 0%;">
                    <center>
                        <div id="divPopAlertContent" runat="server" class="table" style="background-color: White;
                            height: 120px; width: 23%; border: 5px solid #0CA6CA; border-top: 25px solid #0CA6CA;
                            left: 39%; right: 39%; top: 35%; padding: 5px; position: fixed; border-radius: 10px;">
                            <center>
                                <table style="height: 100px; width: 100%; padding: 5px;">
                                    <tr>
                                        <td align="center">
                                            <asp:Label ID="lblAlertMsg" runat="server" Style="color: Red;" Font-Bold="true" Font-Size="Medium"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <center>
                                                <asp:Button ID="btnPopAlertClose" Font-Bold="true" Font-Size="Medium" Font-Names="Book Antiqua"
                                                    CssClass="textbox textbox1" Style="height: auto; width: auto;" OnClick="btnPopAlertClose_Click"
                                                    Text="Ok" runat="server" />
                                            </center>
                                        </td>
                                    </tr>
                                </table>
                            </center>
                        </div>
                    </center>
                </div>
            </center>
            <center>
                <div id="div5" runat="server" visible="false" style="height: 550em; z-index: 2000;
                    width: 100%; background-color: rgba(54, 25, 25, .2); position: absolute; top: 0%;
                    left: 0%;">
                    <center>
                        <div id="div6" runat="server" class="table" style="background-color: White; height: 269px;
                            width: 40%; border: 5px solid #0CA6CA; border-top: 25px solid #0CA6CA; left: 39%;
                            right: 39%; top: 35%; padding: 5px; position: fixed; border-radius: 10px;">
                            <center>
                                <table style="height: 100px; width: 100%; padding: 5px;">
                                    <tr>
                                        <asp:Label ID="Label9" runat="server" Style="color: Red;" Font-Bold="true" Font-Size="Medium"
                                            Text="Topic Add"></asp:Label>
                                    </tr>
                                    <tr>
                                    </tr>
                                    <tr>
                                        <td align="center">
                                            <asp:TextBox ID="TextBox3" runat="server" Width="400px" AutoPostBack="true" TextMode="MultiLine"
                                                Height="150px"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                    </tr>
                                    <tr>
                                        <td>
                                            <center>
                                                <asp:Button ID="Button2" Font-Bold="true" Font-Size="Medium" Font-Names="Book Antiqua"
                                                    CssClass="textbox textbox1" Style="height: auto; width: auto;" OnClick="btnadd"
                                                    Text="Save" runat="server" />
                                                <asp:Button ID="Button3" Font-Bold="true" Font-Size="Medium" Font-Names="Book Antiqua"
                                                    CssClass="textbox textbox1" Style="height: auto; width: auto;" OnClick="btnnexit"
                                                    Text="Exit" runat="server" />
                                            </center>
                                        </td>
                                    </tr>
                                </table>
                            </center>
                        </div>
                    </center>
                </div>
            </center>
            <center>
                <div id="div1" runat="server" visible="false" style="height: 550em; z-index: 2000;
                    width: 100%; background-color: rgba(54, 25, 25, .2); position: absolute; top: 0%;
                    left: 0%;">
                    <center>
                        <div id="div4" runat="server" class="table" style="background-color: White; height: 120px;
                            width: 23%; border: 5px solid #0CA6CA; border-top: 25px solid #0CA6CA; left: 39%;
                            right: 39%; top: 35%; padding: 5px; position: fixed; border-radius: 10px;">
                            <center>
                                <table style="height: 100px; width: 100%; padding: 5px;">
                                    <tr>
                                        <td align="center">
                                            <asp:Label ID="Label7" runat="server" Style="color: Red;" Font-Bold="true" Font-Size="Medium"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <center>
                                                <asp:Button ID="Button1" Font-Bold="true" Font-Size="Medium" Font-Names="Book Antiqua"
                                                    CssClass="textbox textbox1" Style="height: auto; width: auto;" OnClick="btnPopAlertClose_Click"
                                                    Text="Ok" runat="server" />
                                            </center>
                                        </td>
                                    </tr>
                                </table>
                            </center>
                        </div>
                    </center>
                </div>
            </center>
            </center>
            <%--</ContentTemplate>
   <Triggers>
            <asp:PostBackTrigger ControlID="btnsave" />
            <asp:PostBackTrigger ControlID="btnnotesdelete" />
            <asp:PostBackTrigger ControlID="tbtodate" />
            <asp:PostBackTrigger ControlID="tbfdate" />
            <asp:PostBackTrigger ControlID="Buttongo" />
        </Triggers>
    </asp:UpdatePanel>--%>
            <asp:Panel ID="pnotesuploadadd" runat="server" BorderColor="Black" BackColor="AliceBlue"
                BorderWidth="2px" Style="left: 150px; top: 350px; position: absolute;" Height="200px"
                Width="691px">
                <div class="PopupHeaderrstud2" id="Div3" style="text-align: center; font-family: Book Antiqua;
                    font-size: medium; font-weight: bold">
                    <br />
                    <caption style="top: 30px; border-style: solid; border-color: Black; position: absolute;
                        left: 200px">
                        Notes Upload
                    </caption>
                    <br />
                    <br />
                    <table style="text-align: left">
                        <tr>
                            <td>
                                <asp:Label ID="Label4" runat="server" Text="Select Class For Below Details" Font-Size="Medium"
                                    Width="225px" Font-Bold="true" Font-Names="Book Antiqua"></asp:Label>
                            </td>
                            <td>
                                <asp:DropDownList ID="ddlclassnotes" runat="server" AutoPostBack="true" Font-Size="Medium"
                                    Font-Names="Book Antiqua" Height="23px" OnSelectedIndexChanged="ddlselectmanysub_SelectedIndexChanged"
                                    Width="166px">
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:FileUpload ID="fileupload" runat="server" onchange="callme(this)" />
                            </td>
                            <td>
                                <asp:Button ID="btnaddnotes" runat="server" Font-Bold="true" Text="Save" CssClass="floats"
                                    Font-Size="Medium" Font-Names="Book Antiqua" OnClick="btnaddnotes_Click" />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Button ID="btnclosenotes" runat="server" Font-Bold="true" Text="Exit" CssClass="floats"
                                    Font-Size="Medium" Font-Names="Book Antiqua" OnClick="btnclosenotes_Click" />
                            </td>
                        </tr>
                    </table>
                    <table>
                        <tr>
                            <td>
                                <asp:Label ID="Label3" runat="server" Text=" " CssClass="font" ForeColor="Red"></asp:Label>
                            </td>
                        </tr>
                    </table>
                </div>
            </asp:Panel>
            <br />
            <asp:Panel ID="headerpanelhomework" Visible="true" runat="server" CssClass="cpHeader"
                Height="19px" Width="953px">
                <asp:Label ID="lblhomework" runat="server" Text="Home Work" Font-Size="Medium" Font-Names="Book Antiqua" />
                <asp:Image ID="Image5" runat="server" CssClass="cpimage" ImageUrl="../images/right.jpeg" />
            </asp:Panel>
            <asp:Panel ID="pBodyhomework" runat="server" CssClass="cpBody">
                <%--class="maindivstyle"--%>
                <div id="Divv2" runat="server" style="text-align: center; font-family: MS Sans Serif;
                    font-size: Small; font-weight: bold" visible="true">
                    <div>
                        <%--<asp:Button ID="btnaddhme" runat="server" Font-Bold="true" Text="Add Home Work" Font-Size="Medium"
                            Font-Names="Book Antiqua" />--%><%--OnClick="btnaddhme_Click" --%>
                        <table id="Tablenote" runat="server" visible="false">
                            <tr>
                                <td>
                                    <asp:Label ID="lblsubject" Text="Subject" runat="server" Font-Bold="true" Visible="true"
                                        Style="display: inline-block; color: Black; font-family: Book Antiqua; font-size: medium;
                                        font-weight: bold; width: 90px"></asp:Label>
                                </td>
                                <td>
                                    <asp:Label ID="lblsubtext" runat="server" Visible="true" Font-Bold="true" Style="display: inline-block;
                                        color: Black; font-family: Book Antiqua; font-size: medium; font-weight: bold;
                                        width: auto"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="lblheading" Text="Heading" runat="server" Font-Bold="true" Visible="true"
                                        Style="display: inline-block; color: Black; font-family: Book Antiqua; font-size: medium;
                                        font-weight: bold; width: 90px"></asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtheading" runat="server" AutoPostBack="true" border-color="black"
                                        Style="display: inline-block; color: Black; border-width: thin; border-color: Black;
                                        font-family: Book Antiqua; font-size: medium; font-weight: bold; width: 500px;"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="lblhomewrk" Text="HomeWork" runat="server" Font-Bold="true" Style="display: inline-block;
                                        color: Black; font-family: Book Antiqua; font-size: medium; font-weight: bold;
                                        width: 90px;"></asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txthomework" TextMode="MultiLine" runat="server" MaxLength="4000"
                                        Style="display: inline-block; color: Black; font-family: Book Antiqua; font-size: medium;
                                        border-width: thin; border-color: Black; font-weight: bold; width: 500px; height: 75px;"></asp:TextBox>
                                </td>
                                <td>
                                    <asp:Label ID="lblfile" Text="Photos" runat="server" Font-Bold="true" Style="font-family: Book Antiqua;
                                        font-size: medium; font-weight: bold; margin-left: 0px; width: 90px" text-align="left"></asp:Label>
                                    <asp:FileUpload ID="fudfile" runat="server" />
                                    <asp:Label ID="lblshowpic" runat="server" Visible="false" />
                                    <asp:LinkButton ID="lnkdelpic" runat="server" Visible="false" OnClick="lnlremovepic"
                                        ForeColor="Blue" Font-Underline="true">Remove</asp:LinkButton>
                                    <br />
                                    <asp:Label ID="lblattachements" Text="Attachements" runat="server" Font-Bold="true"
                                        Style="font-family: Book Antiqua; font-size: medium; font-weight: bold; width: 90px;
                                        text-align: left" />
                                    <asp:FileUpload ID="fudattachemntss" runat="server" />
                                    <asp:Label ID="lblshowdoc" runat="server" Visible="false" />
                                    <asp:LinkButton ID="lnkdeldoc" runat="server" Visible="false" OnClick="lnlremovedoc"
                                        ForeColor="Blue" Font-Underline="true">Remove</asp:LinkButton>
                                </td>
                            </tr>
                            <tr>
                            </tr>
                            <tr>
                            </tr>
                            <tr>
                            </tr>
                            <tr>
                                <td colspan="2">
                                    <asp:Button ID="btnsavewrk" OnClick="btnsavewrk_Click" Text="Save" runat="server"
                                        Style="font-family: Book Antiqua; font-size: medium; font-weight: bold;" />
                                    <asp:Button ID="btndeletewrk" OnClick="btndeletewrk_Click" Text="Delete" runat="server"
                                        Style="font-family: Book Antiqua; font-size: medium; font-weight: bold;" />
                                    <asp:Label ID="lbldel" Visible="false" runat="server" />
                                </td>
                            </tr>
                        </table>
                    </div>
                    </center>
                </div>
                <%--GridHomeWork--%>
                <asp:UpdatePanel ID="updHM" runat="server">
                    <ContentTemplate>
                        <asp:Panel runat="server" ID="Panel3">
                            <asp:Label ID="lbldate" runat="server" Visible="false" />
                            <table id="Tablegview" runat="server">
                                <tr>
                                    <td>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:HiddenField ID="SelectedGridCellIndex" runat="server" Value="-1" />
                                        <asp:GridView ID="gviewhomewrk" runat="server" OnRowCreated="gviewhme_onRowCreated"
                                            OnSelectedIndexChanged="gviewhme_selectedindexchange" Style="margin-bottom: 15px;
                                            margin-top: 15px; width: auto; font-size: 14px" Font-Names="Times New Roman"
                                            AutoGenerateColumns="false">
                                            <HeaderStyle BackColor="#009999" ForeColor="White" />
                                            <%-- OnRowDataBound="GridView3_OnRowDataBound" OnDataBound="GridView3_OnDataBound"--%>
                                            <AlternatingRowStyle Height="20px" />
                                            <%-- OnPreRender="GridView1_PreRender"--%>
                                            <Columns>
                                                <asp:TemplateField HeaderText="S.No">
                                                    <ItemTemplate>
                                                        <center>
                                                            <asp:Label ID="lblsno" runat="server" Text='<%# Eval("sno") %>' /></center>
                                                        <asp:Label ID="lbluniq" runat="server" Visible="false" Text='<%#Eval("uniqid") %>' />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Date" HeaderStyle-HorizontalAlign="Left">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lbldate" runat="server" Text='<%# Eval("Date1") %>' />
                                                        <%--<asp:Label ID="lblDate" runat="server" Text='<%# Eval("date1") %>' />
                                                <asp:Label ID="lblDegCode" runat="server" Visible="false" Text='<%# Eval("degCode") %>' />--%>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Subject">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblsubject" runat="server" Text='<%# Eval("Subject1") %>' />
                                                        <asp:Label ID="lblsubno" runat="server" Visible="false" Text='<%# Eval("Subjectno") %>' />
                                                        <%--<asp:Label ID="lblSubject" runat="server" Text='<%# Eval("subName") %>' />
                                                <asp:Label ID="lblSem" runat="server" Visible="false" Text='<%# Eval("sem") %>' />--%>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Heading">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblhead" runat="server" Text='<%# Eval("Heading1") %>' />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Topic">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lbltopic" runat="server" Text='<%# Eval("Topic1") %>' />
                                                        <%--<asp:Label ID="lblTopic" runat="server" Text='<%# Eval("topic") %>' />
                                                <asp:Label ID="lblSubjectNo" runat="server" Visible="false" Text='<%# Eval("subNo") %>' />--%>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Photo">
                                                    <ItemTemplate>
                                                        <asp:LinkButton ID="lnkDownloadpic" runat="server" Text='<%#Eval("Photo1") %>' ForeColor="Blue"
                                                            OnClick="lnkDownloadpic_click" Font-Underline="false"></asp:LinkButton>
                                                        <%--<asp:LinkButton ID="lnkDownload" runat="server" Text='<%#Eval("path") %>' ForeColor="Blue"
                                                    OnClick="lnkDownload_click" Font-Underline="false"></asp:LinkButton>
                                                <asp:Label ID="lblPath" runat="server" Text='<%# Eval("path") %>' Visible="false" />
                                                <asp:Label ID="lblPathTag" runat="server" Visible="false" Text='<%# Eval("pathtag") %>' />--%>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Document">
                                                    <ItemTemplate>
                                                        <asp:LinkButton ID="lnkDownloadfile" runat="server" Text='<%#Eval("Attachment1") %>'
                                                            ForeColor="Blue" OnClick="lnkDownloadfile_click" Font-Underline="false"></asp:LinkButton>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                            </Columns>
                                        </asp:GridView>
                                    </td>
                                </tr>
                            </table>
                        </asp:Panel>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </asp:Panel>
            <asp:CollapsiblePanelExtender ID="cpehomework" runat="server" TargetControlID="pBodyhomework"
                CollapseControlID="headerpanelhomework" ExpandControlID="headerpanelhomework"
                Collapsed="true" TextLabelID="lblhomework" CollapsedSize="0" ImageControlID="Image5"
                CollapsedImage="../images/right.jpeg" ExpandedImage="../images/down.jpeg">
            </asp:CollapsiblePanelExtender>
        </ContentTemplate>
        <Triggers>
            <asp:PostBackTrigger ControlID="btnsave" />
            <asp:PostBackTrigger ControlID="btnnotesdelete" />
            <asp:PostBackTrigger ControlID="tbtodate" />
            <asp:PostBackTrigger ControlID="tbfdate" />
            <asp:PostBackTrigger ControlID="Buttongo" />
            <asp:PostBackTrigger ControlID="btnsavewrk" />
            <asp:PostBackTrigger ControlID="btnaddnotes" />
            <asp:PostBackTrigger ControlID="gviewhomewrk" />
            <asp:PostBackTrigger ControlID="txtheading" />
        </Triggers>
    </asp:UpdatePanel>
</asp:Content>

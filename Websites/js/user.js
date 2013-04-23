var count = 0;
function OpenAdd(type) {
    if (type == "dept") {
        $('#navbar_add_menu').css("display", ($('#navbar_add_menu').css("display") == "inline") ? "none" : "inline");
        $('#navbar_addacc_menu').css("display", "none");
        $('#navbar_addeacc_menu').css("display", "none");
    }
    else if (type == "acc") {
        if ($('#navbar_addacc_menu').css("display") == "none") {
            OpenAddAcc();
            $('#navbar_addacc_menu').css("display", "inline");
        }
        else {
            $('#navbar_addacc_menu').css("display", "none");
        }

        $('#navbar_add_menu').css("display", "none");
        $('#navbar_addeacc_menu').css("display", "none");
    }
    else if (type == "exist") {
        if ($('#navbar_addeacc_menu').css("display") == "none") {
            LoadDeptList();
            $('#navbar_addeacc_menu').css("display", "inline");
        }
        else {
            $('#navbar_addeacc_menu').css("display", "none");
        }
        $('#navbar_add_menu').css("display", "none");
        $('#navbar_addacc_menu').css("display", "none");
    }
}
function OnUpdate(type) {
    if (type == "add") {
        if (trim($("#txtDept").val()) == "") {
            accountManagement.utils.showAlertMessage("errfunc", "Please input Department name.", 1);
        }
        else {
            document.fAddDept.submit();
        }
    }
    else if (type == "acc") {
        if ($("#txtUName").val() == "") {
            accountManagement.utils.showAlertMessage("errfunc", "Please input User Name.", 1);
            return;
        }
        if ($("#txtPwd").val() == "") {
            accountManagement.utils.showAlertMessage("errfunc", "Please input Password.", 1);
            return;
        }
        if ($("#selGroup").val() == "0") {
            accountManagement.utils.showAlertMessage("errfunc", "Please choose Department.", 1);
            return;
        }
        document.fAddAcc.submit();
    }
    else if (type == "exist") {
        if ($("#txtEUName").val() == "") {
            accountManagement.utils.showAlertMessage("errfunc", "Please input User Name.", 1);
            $("#txtEUName").focus();
            return;
        }
        if ($("#selEGroup").val() == "0") {
            accountManagement.utils.showAlertMessage("errfunc", "Please choose Department.", 1);
            $("#selEGroup").focus();
            return;
        }
        document.fAddExistAcc.submit();
    }
    else {
        document.fEditDept.submit();
    }
}
function OnCancel(type) {
    if (type == "add") {
        //$('#navbar_add_menu').css("display", "none");
    }
    else if (type == "acc") {
        //$('#navbar_addacc_menu').css("display", "none");
    }
    else if (type == "exist") {
        //$('#navbar_addeacc_menu').css("display", "none");
    ClosePopup();
    }
    $("#err").html("");
}
function LoadDeptInfo() {
    try { parent.BlockUI(); } catch (e) { }
    $.ajax({
        type: "POST",
        url: "GetDepartments2.ashx?t=all",
        async: true,
        data: null,
        success: function (rs) {
            parent.jQuery.unblockUI();
            BindDeptData(rs);
        }
        , error: function () {
            parent.jQuery.unblockUI();
        }
    });
}
function BindDeptData(rs) {
    accountManagement.viewAllDepartments.renderTemplate(JSON.parse(rs), document.getElementById("htmlGroupList"));
}

function DelDept(id) {
    var strC = confirm("Are you sure you want to delete this Department?");
    if (strC) {
        var f = self.frames["frDelete"];
        if (f) {
            try { parent.BlockUI(); } catch (e) { }
            f.location.href = "UpdateDept.ashx?type=del&groupId=" + id;
        }
    }
}

function SelAccount(id, name) {
    tId = id;
    alert_error('', 'err');
    alert_error('', 'errfunc');
    try { parent.BlockUI(); } catch (e) { }
    var args = 't=mem&groupId=' + id + '&dname=' + name;
    if (id == '' && name == '') {
        var search = obj("txtSearch") != null ? obj("txtSearch").value : "All";
        args += "&txtSearch=" + search;
        var alpha = obj("listAlpha") != null ? obj("listAlpha").value : "All";
        args += "&listAlpha=" + alpha;
    }
    OnCancel('acc');
    OnCancel('exist');
    OnCancel('add');
    try { parent.BlockUI(); } catch (e) { }
    $.ajax({
        type: "POST",
        url: "GetDepartments.ashx",
        async: true,
        data: args,
        success: BindGroupPol
    });
}
function SelAccount2(id, name) {
    tId = id;
    try { parent.BlockUI(); } catch (e) { }
    var args = 't=mem&groupId=' + id + '&dname=' + name;
    if (id == '' && name == '') {
        var search = obj("txtSearch") != null ? obj("txtSearch").value : "All";
        args += "&txtSearch=" + search;
        var alpha = obj("listAlpha") != null ? obj("listAlpha").value : "All";
        args += "&listAlpha=" + alpha;
    }
    $.ajax({
        type: "POST",
        url: "GetDepartments2.ashx",
        async: true,
        data: args,
        success: function (rs) {
            parent.jQuery.unblockUI();
            BindGroupPol2(rs);
        }
        , error: function () {
            parent.jQuery.unblockUI();
        }
    });
}

function OnCheckRead(funcId, subc) {

    var idH = "chkH";
    var idR = "chkR";
    var chkRead = document.getElementById(idR + funcId);
    if (!chkRead.checked) {
        document.getElementById(idH + funcId).checked = chkRead.checked;
    }
    if (subc.indexOf("S") == -1) {

        var arr = subc.split(';');
        for (var i = 0; i < arr.length; i++) {
            document.getElementById(idR + arr[i].toString()).checked = chkRead.checked;
            if (!chkRead.checked) {
                document.getElementById(idH + arr[i].toString()).checked = chkRead.checked;
            }
        }
    }
    else {
        var isok = chkRead.checked;
        if (subc.indexOf("_") > -1) {
            var arr = subc.split('_');

            var _arr = arr[1].split(';');

            for (var i = 0; i < _arr.length; i++) {
                if (document.getElementById(idR + _arr[i]).checked) {
                    isok = true;
                }
            }
            document.getElementById(idR + arr[0].substring(2, arr[0].length)).checked = isok
            if (!isok) document.getElementById(idH + arr[0].substring(2, arr[0].length)).checked = isok
        }
        else {
            if (isok == false) {
                document.getElementById(idH + funcId).checked = isok
            }
        }
    }
}
function OnCheckHost(funcId, subc) {
    var idH = "chkH";
    var idR = "chkR";

    var chkRead = document.getElementById(idH + funcId);
    if (chkRead.checked) {
        document.getElementById(idR + funcId).checked = true;
    }

    if (subc.indexOf("S") == -1) {
        var arr = subc.split(';');
        for (var i = 0; i < arr.length; i++) {
            document.getElementById(idH + arr[i].toString()).checked = chkRead.checked;
            if (chkRead.checked) {
                document.getElementById(idR + arr[i].toString()).checked = chkRead.checked;
            }
        }
    }
    else {

        var isok = chkRead.checked;
        if (subc.indexOf("_") > -1) {
            var arr = subc.split('_');

            var _arr = arr[1].split(';');

            for (var i = 0; i < _arr.length; i++) {
                if (document.getElementById(idH + _arr[i]).checked) {
                    isok = true;
                }
            }
            document.getElementById(idH + arr[0].substring(2, arr[0].length)).checked = isok
            if (isok) {
                document.getElementById(idH + arr[0].substring(2, arr[0].length)).checked = isok
                if (arr[0].indexOf("-") > -1)
                    document.getElementById(idR + arr[0].split('-')[1]).checked = isok;
            }


        }
        else {
            if (isok == true) {
                document.getElementById(idR + funcId).checked = isok
            }
        }
    }
}
var tId = "";
function OpenEditDept(id) {
    tId = id;
    LoadEditGroupInfo2(id);
}
function OpenEditDept2(id) {
    tId = id;
    LoadEditGroupInfo2(id);
}
function LoadEditGroupInfo(id) {
    var url = "UserControl/EditGroup.aspx?groupId=" + id;
    showPopup("Edit Group", url, 0, 0, 360, 180, false, 2000, function () { LoadDeptInfo(); });
}

function LoadEditGroupInfo2(id) {
    var url = "EditGroup.aspx?groupId=" + id;
    OpenPopup(url, "Edit Group", 360, 180, true, true, window, 2000, function () { LoadDeptInfo(); }, null);
}

function GetCheck() {
    if (typeof parent.BlockUI != "undefined") parent.BlockUI();
    document.fUpdatePolicy.submit();
}

function OpenUpdateMult(dept) {
    var elements = document.fAdminList.elements;
    var hasCheck = false;
    var strCustid = "";
    var strUser = "";
    for (var i = 0; i < elements.length; i++) {
        if (elements[i].type == "checkbox" && elements[i].name == "chkPolicy") {
            if (elements[i].checked) {
                var arr = elements[i].id.split("_");
                strCustid += arr[0] + ",";
                strUser += arr[1] + ", ";
                hasCheck = true;
            }
        }
    }
    if (hasCheck) {
        if (strUser.length >= 2) strUser = strUser.substr(0, strUser.length - 1);
        LoadMultUpdate(strCustid, strUser, dept);
    }
    else {
        showAlertMessage("errfunc", "Please choose at least one account.", 1);
    }
}
function LoadMultUpdate(arrCustId, arrUser, dept) {
    try { parent.BlockUI(); } catch (e) { }
    $.ajax({
        type: "POST",
        url: "GetDepartments.ashx",
        async: true,
        data: 't=mult&custId=' + arrCustId + '&user=' + arrUser + '&dept=' + dept,
        success: BindGroupPol
    });
}

function CheckAll(chk) {
    alert_error('', 'errfunc');
    var elements = document.fAdminList.elements;
    chk.checked = !chk.checked;
    for (var i = 0; i < elements.length; i++) {
        if (elements[i].type == "checkbox" && elements[i].name == "chkPolicy") {
            elements[i].checked = chk.checked;
        }
    }

}
function OpenAddAcc() {

    $.ajax({
        type: "POST",
        url: "GetUpdateInfo.ashx",
        async: true,
        data: null,
        success: BindAddAcc
    });
}
function BindAddAcc(rs) {
    var data = rs.split('|');
    var selObj = document.getElementById("selGroup");
    if (data.length > 0) {
        selObj.options.length = 0;
        for (var i = 0; i < data.length; i++) {
            if (data[i] != "") {
                var item = data[i].split(',');
                selObj.options.add(new Option(item[1], item[0]));
            }
        }
    }
}
function OnCheck() {
    if ($('#errfunc').val() != "") {
        alert_error('', 'errfunc');
    }
}
function OpenEditAcc(custId, user, dept) {
    var url = "EditAccount.aspx?custId=" + custId + "&dept=" + dept;
    OpenPopup(url, "Edit Account", 420, 240, true, true, window, 2000, function () { SelAccount(tId, dept); }, null);
}
function doReloadAccList(id, dept) {

    var url = self.location.href;
    var index = url.indexOf('?');
    if (index >= 0) {
        url = url.substr(0, index);
    }
    url += "?re=1&gId=" + id + "&dept=" + dept;

    window.location.replace(url);
    //LoadDeptInfo()
    //SelAccount('', '');
    parent.jQuery.unblockUI();

}


function updateStatus(custId, groupId, dept) {
    try {
        var obj = document.getElementById(String(custId) + '_closed');
        obj.checked = !obj.checked;
        var strClosed = obj.checked;

        var _URL = 'UpdateAccount.ashx?type=status&groupId=' + groupId + '&custid=' + custId + '&active=' + strClosed + '&dept=' + dept;
        var f = self.frames['frUpd'];
        if (typeof f != 'undefined' && f) f.location.replace(_URL);
    }
    catch (ex) {
        accountManagement.utils.showAlertMessage("errfunc", ex, 1);
    }
}

function CancelUpd() {
    accountManagement.utils.hideTab6();
}

function OnEnter(e) {
    var code = (e && e.which) ? e.which : e.keyCode;
    if (code == 13) {
        obj("btnAddDept").click();
        return;
    }
    return;
}
function LoadDeptList() {
    $.ajax({
        type: "POST",
        url: "GetUpdateInfo.ashx",
        async: true,
        data: null,
        success: BindDeptList
    });
}
function BindDeptList(rs) {
    var data = rs.split('|');

    var selObj = document.getElementById("selEGroup");
    if (data.length > 0) {

        selObj.options.length = 0;
        for (var i = 0; i < data.length; i++) {
            if (data[i] != "") {
                var item = data[i].split(',');
                selObj.options.add(new Option(item[1], item[0]));
            }
        }
    }
}

function On_KeyPress(e) {
    var code = (e && e.which) ? e.which : e.keyCode;
    if (code == 13) {
        obj("btnGo").click();
        return false;
    }
    return true;
}
function on_Blur() {
    if (obj('txtSearch').value.length == 0)
        obj('txtSearch').value = 'All';
}

function On_Focus() {
    if (obj('txtSearch').value == 'All')
        obj('txtSearch').value = '';
}

function BindGroupPol(rs) {

    document.getElementById("htmlGroupPolicy").innerHTML = '';
    document.getElementById("htmlGroupPolicy").innerHTML = rs;
    Custom.init(); //add styled checkbox
    parent.jQuery.unblockUI();
    $('#tblallaccountlist').find("tr:eq(1)").attr("class", "first");
    $('#tblallaccountlist').tableScroll({ height: 700 });
    $('#tblallaccountlist').find("tr:eq(0) td:nth-child(7)").width(150);
    $('#tblallaccountlist').find("tr:eq(0) td:nth-child(8)").width(79);
    $('#tblallaccountlist').find("tr:eq(0) td:nth-child(9)").width(90);
    $('#tblallaccountlist').find("tr:eq(0) td:nth-child(10)").width(90 - getScrollbarWidth());
    $('#tblallaccountlist').find("tr:nth-child(odd)").css("background-color", "white");
}

function BindGroupPol2(rs) {
    rs = JSON.parse(rs);
    $('#tabs').tabs("select", 4);
    accountManagement.viewAllAccount.renderTemplate(rs, $("#divGroupPolicy").get(0));
}

function getScrollbarWidth() {
    var scrollbarWidth;
    var div = $('<div style="width:50px;height:50px;overflow:hidden;position:absolute;top:-200px;left:-200px;"><div style="height:100px;"></div></div>');
    $('body').append(div);
    var w1 = $('div', div).innerWidth();
    div.css('overflow-y', 'auto');
    var w2 = $('div', div).innerWidth();
    $(div).remove();
    scrollbarWidth = (w1 - w2);
    return scrollbarWidth;
}

var groupId = "";
var groupName = "";
var userName = "";
var custId = "";
//--Get Group Policy--//
function SelGroupPolicy(id) {
    groupId = id;
    try { parent.BlockUI(); } catch (e) { }
    $.ajax({
        type: "POST",
        url: "UserControlService.svc/GetGroupPolicyList",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        data: '{"groupId":' + id + '}',
        success: BindGroupPolicy
    });
}
function SelGroupPolicy2(id) {
    try { parent.BlockUI(); } catch (e) { }
    $.ajax({
        type: "POST",
        url: "UserControlService.svc/GetGroupPolicyList",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        data: '{"groupId":' + id + '}',
        success: function (rs) {
            if (typeof parent.jQuery != "undefined" && typeof parent.jQuery.unblockUI != "undefined")
                parent.jQuery.unblockUI();
            BindGroupPolicy2(id, rs);
        }
        , error: function (e) {
            if (typeof parent.jQuery != "undefined" && typeof parent.jQuery.unblockUI != "undefined")
                parent.jQuery.unblockUI();
            accountManagement.utils.showAlertMessage("errfunc", e, 1);
        }
    });
}
//Get Account Policy
function SelAccountPolicy(id, dept, gId, user) {
    groupName = dept;
    groupId = gId;
    userName = user;
    custId = id;
    try { parent.BlockUI(); } catch (e) { }
    $.ajax({
        type: "POST",
        url: "UserControlService.svc/GetUserPolicyList",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        data: '{"userId":' + id + '}',
        success: function (rs) {
            try { parent.jQuery.unblockUI(); } catch (e) { }
            BindAccountPolicy2(id, dept, gId, user, rs);
        }
        , error: function (e) {
            try { parent.jQuery.unblockUI(); } catch (e) { }
            accountManagement.utils.showAlertMessage("errfunc", e, 1);
        }
    });
}

function BindAccountPolicy2(id, dept, gId, user, rs) {
    rs.groupId = gId;
    rs.groupName = dept;
    rs.userName = user;
    rs.custId = id;
    accountManagement.utils.hideTab6();
    accountManagement.accountPolicy.renderTemplate(rs, $("#divAccountPolicy").get(0));
    title = "";
    WriteDataAccountPolicy(rs, 0);
    $('#tbodyAccountPolicyData').append(title);
    $("#arrFuncId").val(strFunc.substring(1, strFunc.length));
    accountManagement.utils.showTab7();
}

function BindAccountPolicy(rs) {
    $('#htmlGroupPolicy').html("");
    title = "";
    title += '<span style="display: block; width: 710px;">'
    title += '<b>Department:&nbsp;</b>';
    title += '<span class="canchange"  style="font-weight:bold;cursor:pointer" onclick="SelAccount(' + groupId + ',\'' + groupName + '\');">' + groupName + '</span>';
    title += '<b>&nbsp;>>&nbsp;Account:&nbsp;</b>' + userName;
    title += '</span>';
    title += '<form target="frUpd" action="UpdatePolicy.ashx" method="post" name="fUpdatePolicy">';
    title += '<input type="hidden" name="arrFuncId" id="arrFuncId" value=""/>';
    title += '<input type="hidden" name="groupId" id="groupId" value="' + groupId + '"/>';
    title += '<input type="hidden" name="custId" id="custId" value="' + custId + '"/>';
    title += '<input type="hidden" name="user" id="user" value="' + userName + '"/>';
    title += '<input type="hidden" name="dept" id="dept" value="' + groupName + '"/>';
    title += '<br><table cellspacing="1" style="width: 600px;" id="tblcontent" class="tbllayout">';
    title += '<tr class="title">';
    title += '<td style="width:300px; white-space: nowrap;" rowspan="2" >Function</td>';
    title += '<td colspan="2" >Individual Permission</td>';
    title += '<td colspan="2" >Group Permission</td>';
    title += '</tr>';
    title += '<tr class="title">';
    title += '<td style="width:100px;">Read</td>';
    title += '<td style="width:100px;">Host</td>';
    title += '<td style="width:100px;">Read</td>';
    title += '<td style="width:100px;">Host</td>';
    title += '</tr>';
    WriteDataAccountPolicy(rs, 0);
    title += '<tr><td class="c" colspan="5"><input type="button" class="btn" value="Update" id="btnUpdate" name="btnUpdate" onclick="GetCheck();"><input type="button" class="btn" value="Cancel" id="btnCancel" name="btnCancel" onclick="CancelUpd();"></td></tr>';
    title += '</table>';
    title += '</form>';

    $('#htmlGroupPolicy').append(title);
    $("#arrFuncId").val(strFunc.substring(1, strFunc.length));

    //Custom.init(); //add styled checkbox


    //add event for rows data only, ignore title row
    $("#tblcontent tr:not(:.title)").mouseover(function () {
        rowover(this)
    });
    $("#tblcontent tr:not(:.title)").mouseout(function ()
    { rowout() });

    try { parent.jQuery.unblockUI(); } catch (e) { }
}

function WriteDataAccountPolicy(listFunction, level) {

    for (var i = 0; i < listFunction.length; i++) {
        strFunc += "," + listFunction[i].ID;

        title += "<tr>";

        title += "<td class='l indent" + level + "' style='text-align:left'>";
        if (level == 0) {
            title += "<div style='width:301px'>";
            title += listFunction[i].FuncDesc;
            title += "</div>";
        }
        else{
            title += "<span>";
            title += listFunction[i].FuncDesc;
            title += "</span>";
        }
        title += "</td>";
        var isRead = "";
        var isHost = "";
        var isGroupRead = "";
        var isGroupHost = "";
        if (listFunction[i].IsHost == true) { isHost = "checked"; }
        if (listFunction[i].IsRead == true) { isRead = "checked"; }
        if (listFunction[i].IsGroupHost == true) { isGroupHost = "checked"; }
        if (listFunction[i].IsGroupRead == true) { isGroupRead = "checked"; }
        title += "<td class='c'><div style='width:100px'><input type='checkbox' class='chkR' parentId='" + listFunction[i].ParentId + "' id='chkR" + listFunction[i].ID + "' name='chkR" + listFunction[i].ID + "' " + isRead + " onclick='chkCheckRead(this);'></div></td>";
        title += "<td class='c'><div style='width:100px'><input type='checkbox' class='chkH' parentId='" + listFunction[i].ParentId + "' id='chkH" + listFunction[i].ID + "' name='chkH" + listFunction[i].ID + "' " + isHost + " onclick='chkCheckHost(this);'></div></td>";
        title += "<td class='c'><div style='width:100px'><input type='checkbox' " + isGroupRead + " disabled></div></td>";
        title += "<td class='c'><div style='width:82px'><input type='checkbox' " + isGroupHost + " disabled></div></td>";
        title += "</tr>";

        //$("#tblcontent").append(strRow);
        WriteDataAccountPolicy(listFunction[i].SubFuncs, level + 1);
    }


}

var title = "";
var strFunc = "";
function BindGroupPolicy(rs) {

    $('#htmlGroupPolicy').html("");
    title = "";
    title += '<span style="display: block; width: 510px;">'
    title += '<b>Department:</b>';
    title += '<span class="canchange"  style="font-weight:bold;cursor:pointer" onclick="SelAccount(' + groupId + ',\'' + rs[0].GroupName + '\');">' + rs[0].GroupName + '</span>';
    title += '</span>';
    title += '<form target="frUpd" action="UpdatePolicy.ashx" method="post" name="fUpdatePolicy">';
    title += '<input type="hidden" name="arrFuncId" id="arrFuncId" value=""/>';
    title += '<input type="hidden" name="groupId" id="groupId" value="' + groupId + '"/>';
    title += '<input type="hidden" name="dept" id="dept" value="' + rs[0].GroupName + '"/>';
    title += '<br><table cellspacing="1" style="width: 400px;" id="tblcontent" class="tbllayout">';
    title += '<tr class="title">';
    title += '<td style="width:300px; white-space: nowrap;" rowspan="2" >Function</td>';
    title += '<td colspan="2" >Group Permission</td>';
    title += '</tr>';
    title += '<tr class="title">';
    title += '<td style="width:100px;">Read</td>';
    title += '<td style="width:100px;">Host</td>';
    title += '</tr>';
    WriteData(rs, 0);
    title += '<tr><td class="c" colspan="3"><input type="button" class="btn" value="Update" id="btnUpdate" name="btnUpdate" onclick="GetCheck();"><input type="button" class="btn" value="Cancel" id="btnCancel" name="btnCancel" onclick="CancelUpd();"></td></tr>';
    title += '</table>';
    title += '</form>';

    $('#htmlGroupPolicy').append(title);
    $("#arrFuncId").val(strFunc.substring(1, strFunc.length));

    //Custom.init(); //add styled checkbox


    //add event for rows data only, ignore title row
    $("#tblcontent tr:not(:.title)").mouseover(function () {
        rowover(this)
    });
    $("#tblcontent tr:not(:.title)").mouseout(function ()
    { rowout() });

    try { parent.jQuery.unblockUI(); } catch (e) { }
}

function BindGroupPolicy2(groupId, rs) {
    accountManagement.utils.hideTab7();
    rs.groupId = groupId;
    rs.groupName = rs[0].GroupName;
    accountManagement.rolePolicy.renderTemplate(rs, $("#divRolePolicy").get(0));
    title = "";
    WriteData(rs, 0);
    $("#tbodyRolePolicyData").empty().append(title);
    $("#arrFuncId").val(strFunc.substring(1, strFunc.length));
    //Custom.init(); //add styled checkbox
    accountManagement.utils.showTab6();
}

function WriteData(listFunction, level) {

    for (var i = 0; i < listFunction.length; i++) {
        strFunc += "," + listFunction[i].ID;

        title += "<tr>";
        title += "<td style='text-align:left;padding:1px' class='indent" + level + "'>";
        if (level == 0) {
            title += "<div style='width:301px;overflow:hidden'>";
            title += listFunction[i].FuncDesc;
            title += "</div>";
        }
        else {
            title += "<span style='width:300px;overflow:hidden'>";
            title += listFunction[i].FuncDesc;
            title += "</span>";
        }
        title += "</td>";
        var isRead = "";
        var isHost = "";
        if (listFunction[i].IsGroupHost == true) { isHost = "checked"; }
        if (listFunction[i].IsGroupRead == true) { isRead = "checked"; }
        title += "<td class='c'><div style='width:100px;overflow:hidden'><input type='checkbox' class='chkR' parentId='" + listFunction[i].ParentId + "' id='chkR" + listFunction[i].ID + "' name='chkR" + listFunction[i].ID + "' " + isRead + " onclick='javascript:chkCheckRead(this);'></div></td>";
        title += "<td class='c'><div style='width:92px;overflow:hidden'><input type='checkbox' class='chkH' parentId='" + listFunction[i].ParentId + "' id='chkH" + listFunction[i].ID + "' name='chkH" + listFunction[i].ID + "' " + isHost + " onclick='javascript:chkCheckHost(this);'></div></td>";

        title += "</tr>";
        //$("#tblcontent").append(strRow);
        WriteData(listFunction[i].SubFuncs, level + 1);
    }
}
//--Get Group Policy--//
function chkCheckHost(obj) {
    var chkHost = $(obj);
    var id = chkHost.attr("id").replace("chkH", "");
    checkHostByParent(id, chkHost.attr("checked"));
    checkHostParentById(chkHost.attr("parentId"), chkHost.attr("checked"));

}

function chkCheckRead(obj) {
    var chkRead = $(obj);
    var id = chkRead.attr("id").replace("chkR", "");
    checkReadByParent(id, chkRead.attr("checked"));
    checkReadParentById(chkRead.attr("parentId"), chkRead.attr("checked"));
    if (!chkRead.attr("checked")) {
        chkCheckHost(document.getElementById("chkH" + id));
    }
}

//Checked or uncheck subfunction 
function checkHostByParent(parentId, value) {
    var objs = $('.chkH');
    if (value) { $("#chkR" + parentId).attr("checked", value); }
    objs.each(function () {
        if ($(this).attr("parentId") == parentId) {
            $(this).attr("checked", value);
            //check read when host is checked
            if (value) {
                $("#" + $(this).attr("id").replace("chkH", "chkR")).attr("checked", value);
            }
            var sid = $(this).attr("id").replace("chkH", "");
            checkHostByParent(sid, $(this).attr("checked"));
        }
    });
}

function checkHostParentById(parentId, value) {
    if (parentId == 0) return;
    var objs = $('.chkH');
    if (value == true) {
        objs.each(function () {
            if ($(this).attr("id").replace("chkH", "") == parentId) {
                $(this).attr("checked", value);
                $("#" + $(this).attr("id").replace("chkH", "chkR")).attr("checked", value);
                parentId = $(this).attr("parentId");
                checkHostParentById(parentId, $(this).attr("checked"));
            }
        });
    }
    else {
        var ischeck = false;
        objs.each(function () {
            if ($(this).attr("parentId") == parentId) {
                if ($(this).attr("checked")) {
                    ischeck = $(this).attr("checked");
                }
            }
        });
        if (ischeck == false) {
            $("#chkH" + parentId).attr("checked", false);
            checkHostParentById($("#chkH" + parentId).attr("parentId"), false);
        }
    }
}

//Checked or uncheck subfunction 
function checkReadByParent(parentId, value) {
    var objs = $('.chkR');
    if (!value) { $("#chkH" + parentId).attr("checked", value); }
    objs.each(function () {
        if ($(this).attr("parentId") == parentId) {
            $(this).attr("checked", value);
            //check read when host is checked
            if (!value) {
                $("#" + $(this).attr("id").replace("chkR", "chkH")).attr("checked", value);
            }
            var sid = $(this).attr("id").replace("chkR", "");
            checkReadByParent(sid, $(this).attr("checked"));
        }
    });
}

function checkReadParentById(parentId, value) {
    if (parentId == 0) return;
    var objs = $('.chkR');
    if (value == true) {
        objs.each(function () {
            if ($(this).attr("id").replace("chkR", "") == parentId) {
                $(this).attr("checked", value);
                parentId = $(this).attr("parentId");
                checkReadParentById(parentId, $(this).attr("checked"));
            }
        });
    }
    else {

        var ischeck = false;
        objs.each(function () {
            if ($(this).attr("parentId") == parentId) {
                if ($(this).attr("checked")) {
                    ischeck = $(this).attr("checked");
                }
            }
        });
        if (ischeck == false) {

            $("#chkR" + parentId).attr("checked", false);
            $("#chkH" + parentId).attr("checked", false);

            //Recursive
            checkReadParentById($("#chkH" + parentId).attr("parentId"), false);
        }
    }
}
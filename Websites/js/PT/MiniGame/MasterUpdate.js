
$(document).ready(function() {
    getMasterPT();
});

//-----------Load data-----------------//
function getMasterPT() {
    if (parent.BlockUI) parent.BlockUI();
    else if (parent.parent.BlockUI) parent.parent.BlockUI();
    var username = gup("username");
    var param = '{"username":"' + username + '"}';
    
    $.ajax({
        type: "POST",
        url: "../../Services/WCF/PT/MiniGame/ServiceMiniGame.svc/GetMasterPTSetting",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        data: param,
        success: function(rs) {
            var data = null;
            data = $.evalJSON(rs);
            if (data != null) {
               
                $(".bg_table").removeClass("hidden");
                var MasterInfo = $("#MasterInfo");
                MasterInfo.setTemplateURL("../../App_Templates/Customer/PT/MiniGame/Info.htm");
                MasterInfo.setParam('Title', "Master: " + username);
                MasterInfo.processTemplate(data[0]);
                AssignCommission();

                //Assing MiniGame PT
                var MasterMiniGame = $("#MasterMiniGame");
                MasterMiniGame.setTemplateURL("../../App_Templates/Customer/PT/MiniGame/MasterUpdateMiniGamePT.htm");
                MasterMiniGame.processTemplate(data[0]);
                AssignPT();
            }
            else {
                $("#err").html('<div class="alertboxerr"><div>There is no information available.</div></div>');
                $(".bg_table").addClass("hidden");
            }
             if (parent.jQuery.unblockUI) parent.jQuery.unblockUI();
             else if (parent.parent.jQuery.unblockUI) {
                parent.parent.jQuery.unblockUI();}
        }
    });
}
//-----------End Load data-------------//

//----------Event------------------------//
function updateMaster() {
    if (parent.BlockUI) parent.BlockUI();
    else if (parent.parent.BlockUI) parent.parent.BlockUI();
    var custId = gup("custId");
    var spt = $("#listSPre252").val();
    var isauto = $("#chkAutoMiniGame").attr("checked");
    var sautopt = $("#listSAutoPre252").val();
    var mpt = $("#listMMax252").val();
    var minpt = $("#listMMin252").val();
    var comm = $("#listGroupA").val();
    
    var param = '{"custid":' + custId + ',"spt":' + spt + ',"isauto":' + isauto + ',"sautopt":' + sautopt + ',"mpt":' + mpt + ',"minpt":' + minpt + ',"comm":' + comm + ' }';
    $.ajax({
        type: "POST",
        url: "../../Services/WCF/PT/MiniGame/ServiceMiniGame.svc/UpdateMaster",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        data: param,
        success: function(rs) {
            if (rs == "True") {
                showAlertMessage("err", "Update Master successfully", 0);
                getMasterPT();
            }
            else {
                showAlertMessage("err", rs, 1);
            }
            if (parent.jQuery.unblockUI) parent.jQuery.unblockUI();
            else if (parent.parent.jQuery.unblockUI) {//Used on case open from superlist page
                parent.parent.jQuery.unblockUI();
                setTimeout("parent.ibcWnd.close()", 2000);
            }
        }
    });
}

function ReducePT(id, step) {
    var obj = $("#" + id);
    var curr = obj.val();
    var min = obj.attr("min");
    if (!obj.attr("disabled")) {
        if (obj.val() > min) {
            var newvalue = roundNumber((curr * 100 - step) / 100, 2);
            var max = obj.attr("max");
            if (newvalue < min) newvalue = curr;

            if ($("#" + id + " option").size() <= 1) {
                CreateOption(obj, newvalue, max, min, 0.01, 2);
            }
            else {
                $("#" + id + " option[value='" + newvalue + "']").attr("selected", "selected");
            }

            if (id == 'listSPre252') On_ChangeSuper();
            if (id == 'listMMax252') On_ChangeMaster();
        }
    }
}

function IncreasePT(id, step) {
    var obj = $("#" + id);
    if (!obj.attr("disabled")) {
        var curr = obj.val();
        var min = obj.attr("min");
        var max = obj.attr("max");
        if (obj.val() < max) {
            var newvalue = roundNumber((curr * 100 + step) / 100, 2);
            if (newvalue > max) newvalue = curr;
            if ($("#" + id + " option").size() <= 1) {
                CreateOption(obj, newvalue, max, min, 0.01, 2);
            }
            else {
                $("#" + id + " option[value='" + newvalue + "']").attr("selected", "selected");
            }
            if (id == 'listSPre252') On_ChangeSuper();
        }
    }
}

function On_ChangeSuper() {
    var SPrelist = $("#listSPre252");
    var MMaxlist = $("#listMMax252");
    var MMinlist = $("#listMMin252");
    var SPrevalue = SPrelist.val();
    var SMinUpdline = SPrelist.attr("minupline");
    var Max_MMaxvalue = roundNumber(SPrelist.attr("max") - SPrevalue, 2);
    var MMaxvalue = MMaxlist.val();
    MMaxvalue = (MMaxvalue >= Max_MMaxvalue) ? Max_MMaxvalue : MMaxvalue;
    MMaxlist.attr("max", Max_MMaxvalue);
    if (SPrevalue < SMinUpdline) {
        MMaxlist.attr("min", roundNumber(SMinUpdline - SPrevalue,2));
        MMinlist.attr("min", roundNumber(SMinUpdline - SPrevalue,2));
    }
    else {
        MMaxlist.attr("min", 0);
        MMinlist.attr("min", 0);
    }
    CreateOption(MMaxlist, MMaxvalue, Max_MMaxvalue, MMaxlist.attr("min"), 0.01, 2);
    On_ChangeMaster();
}

function On_ChangeSuperAuto()
{
    var SAutoPrelist = $("#listSAutoPre252");
    var MMaxlist = $("#listMMax252");
    var MMinlist = $("#listMMin252");
    var SAutoPrevalue = SAutoPrelist.val();
    var MMaxvalue = MMaxlist.val();
    var SMinUpdline = SAutoPrelist.attr("minupline");
    if ((Number(SAutoPrevalue) + Number(MMaxvalue)) < SMinUpdline) {
        MMaxvalue = roundNumber(SMinUpdline - SAutoPrevalue, 2);
    }
    if (SAutoPrevalue < SMinUpdline) {
        MMaxlist.attr("min", roundNumber(SMinUpdline - SAutoPrevalue,2));
        MMinlist.attr("min", roundNumber(SMinUpdline - SAutoPrevalue,2));
    }
    else {
        MMaxlist.attr("min", 0);
        MMinlist.attr("min", 0);
    }
    //alert(MMaxvalue + "," + MMaxlist.attr("max") + "," + MMaxlist.attr("min"));
    CreateOption(MMaxlist, MMaxvalue, MMaxlist.attr("max"), MMaxlist.attr("min"), 0.01, 2);
    On_ChangeMaster();
}

function On_ChangeMaster() {
    var MMaxlist = $("#listMMax252");
    var MMinlist = $("#listMMin252");
    var MMinvalue = MMinlist.val();
    var MMaxvalue = MMaxlist.val();
    if (MMaxvalue < MMinvalue) MMinvalue = MMaxvalue;
    MMinlist.attr("max", MMaxvalue);
    CreateOption(MMinlist, MMinvalue, MMaxvalue, MMinlist.attr("min"), 0.01, 2);
}

function CheckIsAuto(obj) {
    var listSPre252 = $('#listSPre252');
    var listSAutoPre252 = $('#listSAutoPre252');
    var listMMax252 = $('#listMMax252');
    if (obj.checked == true) {
        listSPre252.attr("disabled", "disabled");
        listSAutoPre252.attr("disabled", "");
        listMMax252.attr("max", listSAutoPre252.attr("max"));
        On_ChangeSuperAuto();
    }
    else {
        listSAutoPre252.attr("disabled", "disabled");
        listSPre252.attr("disabled", "");
        listMMax252.attr("max", roundNumber(listSPre252.attr("max") - listSPre252.val(), 2));
        On_ChangeSuper();
    }
    On_ChangeMaster();
}

function changeMinValue(isAuto, SMinUpline, SPreValue, SAutoPreValue) {
    var value=SPreValue;
    if (isAuto) {
        value = SAutoPreValue;
    }
    if (value < SMinUpline) return roundNumber(SMinUpline - value, 2);
    else return 0;
}
//----------End Event--------------------//


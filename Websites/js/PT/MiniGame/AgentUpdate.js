
$(document).ready(function() {
    getAgentPT();
});

//-----------Load data-----------------//
function getAgentPT() {
    if (parent.BlockUI) parent.BlockUI();
    else if (parent.parent.BlockUI) parent.parent.BlockUI();
    var username = gup("username");
    var param = '{"username":"' + username + '"}';
    $.ajax({
        type: "POST",
        url: "../../Services/WCF/PT/MiniGame/ServiceMiniGame.svc/GetAgentPTSetting",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        data: param,
        success: function(rs) {
            var data = null;
            data = $.evalJSON(rs);

            if (data != null) {
               
                $(".bg_table").removeClass("hidden");
                var AgentInfo = $("#AgentInfo");
                AgentInfo.setTemplateURL("../../App_Templates/Customer/PT/MiniGame/Info.htm");
                AgentInfo.setParam('Title', "Agent: " + username);
                AgentInfo.processTemplate(data[0]);
                AssignCommission();

                //Assing MiniGame PT
                var AgentMiniGame = $("#AgentMiniGame");
                AgentMiniGame.setTemplateURL("../../App_Templates/Customer/PT/MiniGame/AgentUpdateMiniGamePT.htm");
                AgentMiniGame.processTemplate(data[0]);
                AssignPT();
            }
            else {
                $("#err").html('<div class="alertboxerr"><div>There is no information available.</div></div>');
                $(".bg_table").addClass("hidden");
            }
            if (parent.jQuery.unblockUI) parent.jQuery.unblockUI();
            else if (parent.parent.jQuery.unblockUI) {
                parent.parent.jQuery.unblockUI();
            }
        }
    });
}
//-----------End Load data-------------//

//----------Event------------------------//
function updateAgent() {
    if (parent.BlockUI) parent.BlockUI();
    else if (parent.parent.BlockUI) parent.parent.BlockUI();
    var custId = gup("custId");
    var mpt = $("#listMPre252").val();
    var apt = $("#listAMax252").val();
    var minpt = $("#listAMin252").val();
    var comm = $("#listGroupA").val();

    var param = '{"custid":' + custId + ',"mpt":' + mpt + ',"apt":' + apt + ',"minpt":' + minpt + ',"comm":' + comm + ' }';
    $.ajax({
        type: "POST",
        url: "../../Services/WCF/PT/MiniGame/ServiceMiniGame.svc/UpdateAgent",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        data: param,
        success: function(rs) {
            if (rs == "True") {
                showAlertMessage("err", "Update Agent successfully", 0);
                getAgentPT();
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

function On_ChangeMaster() {
    var MPrelist = $("#listMPre252");
    var AMaxlist = $("#listAMax252");
    var AMinlist = $("#listAMin252");
    var MPrevalue = MPrelist.val();
    var AMaxvalue;

    var Max_AMaxvalue = AMaxvalue = roundNumber(MPrelist.attr("max") - MPrevalue, 2);
    var MMinUpline = MPrelist.attr("minupline");
    if (MPrevalue < MMinUpline) {
        AMaxlist.attr("min", roundNumber(MMinUpline - MPrevalue, 2));
        AMinlist.attr("min", roundNumber(MMinUpline - MPrevalue, 2));
    }
    else {
        AMaxlist.attr("min", 0);
        AMinlist.attr("min", 0);
    }
    AMaxlist.attr("max", Max_AMaxvalue);
    if (Max_AMaxvalue < AMaxvalue) AMaxvalue = Max_AMaxvalue;
    
    CreateOption(AMaxlist, AMaxvalue, Max_AMaxvalue, AMaxlist.attr("min"), 0.01, 2);
    On_ChangeAgent();
}

function On_ChangeAgent() {
    var AMaxlist = $("#listAMax252");
    var AMinlist = $("#listAMin252");
    var AMinvalue = AMinlist.val();
    var AMaxvalue = AMaxlist.val();
    var Max_AMinvalue = AMinlist.attr("max");

    Max_AMinvalue = AMaxvalue;

    if (AMaxvalue < AMinvalue) AMinvalue = AMaxvalue;
    AMinlist.attr("max", Max_AMinvalue);
    CreateOption(AMinlist, AMinvalue, Max_AMinvalue, AMinlist.attr("min"), 0.01, 2);
}

function ReducePT(id, step) {
    var obj = $("#" + id);
    var curr = obj.val();
    var min = obj.attr("min");
  
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
    }
    if (id == 'listMPre252') On_ChangeMaster();
    if (id == 'listAMax252') On_ChangeAgent();
}

function IncreasePT(id, step) {
    var obj = $("#" + id);
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
        if (id == 'listMPre252') On_ChangeMaster();
        if (id == 'listAMax252') On_ChangeAgent();
    }

}

//----------End Event--------------------//

function changeMinValue(MMinUpline, MPreValue) {
    if (MPreValue < MMinUpline) {
        return roundNumber(MMinUpline - MPreValue, 2);
    }
    else {
        return 0;
    };
}
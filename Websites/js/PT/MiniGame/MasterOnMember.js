
$(document).ready(function() {
    
    getMasterOnMemberPT();
});

//-----------Load data-----------------//
function getMasterOnMemberPT() {
    try{parent.BlockUI();}catch(e){}
    var username = gup("username");
    var param = '{"username":"' + username + '"}';
    $.ajax({
        type: "POST",
        url: "../../Services/WCF/PT/MiniGame/ServiceMiniGame.svc/GetMasterOnMemberPTSetting",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        data: param,
        success: function(rs) {
            var data = null;
            data = $.evalJSON(rs);


            //Assign Super Info
            if (data != null) {
                if (data[0].IBCCustID == null) {
                    $("#err").html('<div class="alertboxerr"><div>' + rs.replace(/\"/g, "") + '</div></div>');
                    $(".bg_table").addClass("hidden");
                }
                else {
                    $(".bg_table").removeClass("hidden");
                    var MasterInfo = $("#MasterInfo");
                    MasterInfo.setTemplateURL("../../App_Templates/Customer/PT/MiniGame/InfoMember.htm");
                    MasterInfo.setParam('Title', "Member: " + username);
                    MasterInfo.processTemplate(data[0]);

                    //Assing MiniGame PT
                    var MasterMiniGame = $("#MasterMiniGame");
                    MasterMiniGame.setTemplateURL("../../App_Templates/Customer/PT/MiniGame/MasterOnMemberMiniGamePT.htm");
                    MasterMiniGame.processTemplate(data[0]);
                    AssignPT();
                }
            }
            else {
                $("#err").html('<div class="alertboxerr"><div>There is no information available.</div></div>');
                $(".bg_table").addClass("hidden");
            }
            try{parent.jQuery.unblockUI();}catch(e){}
        }
    });
}
//----------End load data----------------//
//----------Event------------------------//
function updateMasterOnMember() {
    try{parent.BlockUI();}catch(e){}
    var custId = gup("custid");
    var mpt = $("#listMPT252").val();
    var param = '{"custid":' + custId + ',"mpt":' + mpt + ' }';
    $.ajax({
        type: "POST",
        url: "../../Services/WCF/PT/MiniGame/ServiceMiniGame.svc/UpdateMasterOnMember",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        data: param,
        success: function(rs) {
            if (rs == "True") {
                showAlertMessage("err", "Update Master On Member successfully", 0);
                getMasterOnMemberPT();
            }
            else {
                showAlertMessage("err", rs, 1);
            }
            try{parent.jQuery.unblockUI();}catch(e){}
        }
    });
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
    }
}
//-------------End Event-------------------//




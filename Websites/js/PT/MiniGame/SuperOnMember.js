
$(document).ready(function() {
    
    getSuperOnMemberPT();
});

//-----------Load data-----------------//
function getSuperOnMemberPT() {
    try{parent.BlockUI();}catch(e){}
    var username = gup("username");
    var param = '{"username":"' + username + '"}';
    $.ajax({
        type: "POST",
        url: "../../Services/WCF/PT/MiniGame/ServiceMiniGame.svc/GetSuperOnMemberPTSetting",
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
                    var SuperInfo = $("#SuperInfo");
                    SuperInfo.setTemplateURL("../../App_Templates/Customer/PT/MiniGame/InfoMember.htm");
                    SuperInfo.setParam('Title', "Member: " + username);
                    SuperInfo.processTemplate(data[0]);

                    //Assing MiniGame PT
                    var SuperMiniGame = $("#SuperMiniGame");
                    SuperMiniGame.setTemplateURL("../../App_Templates/Customer/PT/MiniGame/SuperOnMemberMiniGamePT.htm");
                    SuperMiniGame.processTemplate(data[0]);
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
function updateSuperOnMember() {
    try{parent.BlockUI();}catch(e){}
    var custId = gup("custid");
    var spt = $("#listSPT252").val();
    var isauto = $("#chkAutoMiniGame").attr("checked");
    var sautopt = $("#listSAutoPT252").val();

    var param = '{"custid":' + custId + ',"spt":' + spt + ',"isautopt":' + isauto + ',"sautopt":' + sautopt + ' }';
    $.ajax({
        type: "POST",
        url: "../../Services/WCF/PT/MiniGame/ServiceMiniGame.svc/UpdateSuperOnMember",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        data: param,
        success: function(rs) {
            if (rs == "True") {
                showAlertMessage("err", "Update Super On Member successfully", 0);
                getSuperOnMemberPT();
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
function CheckIsAuto(obj) {
    if (obj.checked == true) {
        $("#listSPT252").attr("disabled", "disabled");
        $("#chkR1SPre252").attr("disabled", "disabled");
        $("#chkI1SPre252").attr("disabled", "disabled");
        $("#chkR10SPre252").attr("disabled", "disabled");
        $("#chkI10SPre252").attr("disabled", "disabled");

        $("#listSAutoPT252").attr("disabled", "");
        $("#chkR1SAutoPre252").attr("disabled", "");
        $("#chkI1SAutoPre252").attr("disabled", "");
        $("#chkR10SAutoPre252").attr("disabled", "");
        $("#chkI10SAutoPre252").attr("disabled", "");
    }
    else {
        $("#listSAutoPT252").attr("disabled", "disabled");
        $("#chkR1SAutoPre252").attr("disabled", "disabled");
        $("#chkI1SAutoPre252").attr("disabled", "disabled");
        $("#chkR10SAutoPre252").attr("disabled", "disabled");
        $("#chkI10SAutoPre252").attr("disabled", "disabled");

        $("#listSPT252").attr("disabled", "");
        $("#chkR1SPre252").attr("disabled", "");
        $("#chkI1SPre252").attr("disabled", "");
        $("#chkR10SPre252").attr("disabled", "");
        $("#chkI10SPre252").attr("disabled", "");
    }
}
//-------------End Event-------------------//




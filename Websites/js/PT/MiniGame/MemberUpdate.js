$(document).ready(function() {
    
    getMembertPT();
});

//-----------Load data-----------------//
function getMembertPT() {

    if (parent.BlockUI) parent.BlockUI();
    else if (parent.parent.BlockUI) parent.parent.BlockUI();
    var username = gup("username");
    var param = '{"username":"' + username + '"}';
    $.ajax({
        type: "POST",
        url: "../../Services/WCF/PT/MiniGame/ServiceMiniGame.svc/GetMemberPTSetting",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        data: param,
        success: function(rs) {
            var data = null;
            data = $.evalJSON(rs);
            if (data != null) {
                if (data[0].IBCCustID == null) {
                    $("#err").html('<div class="alertboxerr"><div>' + rs.replace(/\"/g,"") + '</div></div>');
                    $(".bg_table").addClass("hidden");
                }
                else {
                    $(".bg_table").removeClass("hidden");
                    var MemberInfo = $("#MemberInfo");
                    MemberInfo.setTemplateURL("../../App_Templates/Customer/PT/MiniGame/Info.htm");
                    MemberInfo.setParam('Title', "Member: " + username);
                    MemberInfo.processTemplate(data[0]);
                    AssignCommission();

                    //Assing MiniGame PT
                    var MemberMiniGame = $("#MemberMiniGame");
                    MemberMiniGame.setTemplateURL("../../App_Templates/Customer/PT/MiniGame/MemberUpdateMiniGamePT.htm");
                    MemberMiniGame.processTemplate(data[0]);
                    AssignPT();
                }
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

function updateMember() {
    if (parent.BlockUI) parent.BlockUI();
    else if (parent.parent.BlockUI) parent.parent.BlockUI();
    var custId = gup("custId");
    var apt = $("#listAPre252").val();
    var comm = $("#listGroupA").val();

    var param = '{"custid":' + custId + ',"apt":' + apt + ',"comm":' + comm + ' }';
    $.ajax({
        type: "POST",
        url: "../../Services/WCF/PT/MiniGame/ServiceMiniGame.svc/UpdateMember",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        data: param,
        success: function(rs) {
            if (rs == "True") {
                showAlertMessage("err", "Update Member successfully", 0);
                getMembertPT();
            }
            else {
                showAlertMessage("err", rs, 1);
            }
            if (parent.jQuery.unblockUI) parent.jQuery.unblockUI()();
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
//----------End Event--------------------//




$(document).ready(function() {
   
    getSuperPT();
});

//-----------Load data-----------------//
function getSuperPT(isReload) {
    if (parent.BlockUI) parent.BlockUI();
    else if (parent.parent.BlockUI) parent.parent.BlockUI();
    var username = gup("username");
    var param = '{"username":"' + username + '"}';
    $.ajax({
        type: "POST",
        url: "../../Services/WCF/PT/MiniGame/ServiceMiniGame.svc/GetSuperPTSetting",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        data: param,
        success: function(rs) {
            var data = null;
            data = $.evalJSON(rs);


            //Assign Super Info
            if (data != null) {
                $(".bg_table").removeClass("hidden");
                var SuperInfo = $("#SuperInfo");
                SuperInfo.setTemplateURL("../../App_Templates/Customer/PT/MiniGame/Info.htm");
                SuperInfo.setParam('Title', "Super: " + username);
                SuperInfo.processTemplate(data[0]);
                AssignCommission();

                //Assing MiniGame PT
                var SuperMiniGame = $("#SuperMiniGame");
                SuperMiniGame.setTemplateURL("../../App_Templates/Customer/PT/MiniGame/SuperUpdateMiniGamePT.htm");
                SuperMiniGame.processTemplate(data[0]);
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
//----------End load data----------------//
//----------Event------------------------//
function updateSuper() {
    if (parent.BlockUI) parent.BlockUI();
    else if (parent.parent.BlockUI) parent.parent.BlockUI();
    var custId = gup("custId");
    var spt = $("#listSMax252").val();
    var minpt = $("#listSMin252").val();
    var comm = $("#listGroupA").val();

    var param = '{"custid":' + custId + ',"spt":' + spt + ',"minpt":' + minpt + ',"comm":' + comm + ' }';
    $.ajax({
        type: "POST",
        url: "../../Services/WCF/PT/MiniGame/ServiceMiniGame.svc/UpdateSuper",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        data: param,
        success: function(rs) {
            if (rs == "True") {
                showAlertMessage("err", "Update Super successfully", 0);
                getSuperPT();
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

function On_ChangeSuper(obj) {
    var SMaxlist = $(obj);
    var SMinlist = $("#listSMin252");
    var SMinvalue = SMinlist.val();
    var Max_SMinvalue = SMinlist.attr("max");
    var SMaxvalue = SMaxlist.val();
    Max_SMinvalue = SMaxvalue;

    if (SMaxvalue < SMinvalue) SMinvalue = SMaxvalue;

    SMinlist.attr("max", Max_SMinvalue);
    CreateOption(SMinlist, SMinvalue, Max_SMinvalue, 0, 0.01, 2);
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

        if (id == "listSMax252") {
            On_ChangeSuper(document.getElementById(id));
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
        if (id == "listSMax252") {
            On_ChangeSuper(document.getElementById(id));
        }
    }
}

//-------------End Event-------------------//




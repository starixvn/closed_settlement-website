var refund = {};

refund.init = function () {

};

refund.submit = function () {
    $("#msg").hide();
    if (!confirm("Are you sure you want to Refund this game?")) return;
    var MatchID = $("#hiddenMatchID").val();
    var AdminID = $("#hiddenAdminID").val();
    var AdminName = $("#hiddenAdminName").val();
    refund.blockUI();
    $.ajax({
        type: "POST",
        url: "service/SettleLiveCasinoService.svc/Refund",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        data: '{"MatchID":' + MatchID + ', "AdminID": ' + AdminID + ', "AdminName": "' + AdminName + '", "ShowDate":"' + $("#hiddenShowDate").val() + '"}',
        success: function (rs) {
            refund.unBlock();
            if (rs != "") {
                $("#msg").css("border-color", "red").css("color", "red").html(rs).show();
            }
            else {
                $("#msg").css("border-color", "green").css("color", "green").html("Refund is successfully!").show();
                var timerId = setTimeout(function () {
                    clearTimeout(timerId);
                    if (parent.parent.settlelc && parent.parent.settlelc.getData) {
                        parent.parent.settlelc.getData();
                    }
                }, 2000);
            }
        }
    });
};

refund.closePopup = function () {
    if (typeof parent.ClosePopup != "undefined") {
        parent.ClosePopup();
    }
};

refund.blockUI = function () {
    try { parent.parent.parent.BlockUI(); } catch (e) { try { parent.parent.BlockUI(); } catch (e) { parent.BlockUI(); } }
};

refund.unBlock = function () {
    try { parent.parent.parent.$.unblockUI(); } catch (e) { try { parent.parent.$.unblockUI(); } catch (e) { parent.BlockUI(); } }
};

$(document).ready(function () {
    refund.init();
});
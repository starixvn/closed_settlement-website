var recalc = {};
recalc.PartyBaccratId = 1101;
recalc.PartyRoutleId = 1102;
recalc.LiveBaccratId = 1103;
recalc.LiveRoutleId = 1104;

recalc.init = function () {
    var bettype = $('#hiddenBetType').val();
    if (bettype == recalc.PartyBaccratId || bettype == recalc.LiveBaccratId)
        recalc.baccaratInit();
    else
        recalc.routleInit();
};

recalc.baccaratInit = function () {
    $('#tbRoutle').addClass('hidden');
    $('#tbBaccarat').removeClass('hidden');

    $('#tbBaccarat').find("input[type='text']").keydown(function (e) {
        var el = e.target;
        var key = e.charCode || e.keyCode || 0;
        if (key == 9 || key == 8 || key == 46) return true;
        if (key >= 35 && key <= 40) return true;
        if ($(el).val().length == 0) {
            if (key != 83 && key != 72 && key != 67 && key != 68) return false;
        }
        else if ($(el).val().length == 1) {
            var currentPosition = $(this).caret().start;
            if (currentPosition == 0) {
                if ($(el).val() == "S" || $(el).val() == "H" || $(el).val() == "C" || $(el).val() == "D") return false;
                if (key != 83 && key != 72 && key != 67 && key != 68) return false;
            }
            else {
                if (key != 65 && key != 74 && key != 75 && key != 81 && (key < 48 || key > 57) && (key < 96 || key > 105)) return false;
            }
        }
    });

    $("#btnSubmit").click(function () {
        recalc.submit();
    });
}

recalc.routleInit = function () {
    $('#tbBaccarat').addClass('hidden');
    $('#tbRoutle').removeClass('hidden');
    $('#tbRoutle').find("input[type='text']").keydown(function (e) {
        var el = e.target;
        var key = e.charCode || e.keyCode || 0;

        if (key == 9 || key == 8 || key == 46) return true;
        if ((key >= 48 && key <= 57) | (key >= 96 && key <= 105)) {
            return true; 
        }
        return false;
    });
    $("#btnRoutleCalc").click(function () {
        recalc.submit();
    });
};

recalc.submit = function () {
    var bettype = $('#hiddenBetType').val();

    if (bettype == recalc.PartyBaccratId || bettype == recalc.LiveBaccratId)
        recalc.baccaratCalc();
    else
        recalc.routleCalc();

};
recalc.baccaratCalc = function () {
    $("#msg").hide();

    if ($("#tbx1").val().length != 2 || $("#tbx2").val().length != 2 || $("#tbx3").val().length != 2 || $("#tbx4").val().length != 2) {
        $("#msg").html("Cards is invalid").show();
        return;
    }

    if ($("#tbx5").val().length == 1 || $("#tbx6").val().length == 1) {
        $("#msg").html("Cards is invalid").show();
        return;
    }

    if (!confirm("Are you sure you want to Re-Calculate this game?")) return;

    var bankerCard = $("#tbx2").val() + "," + $("#tbx4").val() + ($("#tbx6").val() == "" ? "" : "," + $("#tbx6").val());
    var playerCard = $("#tbx1").val() + "," + $("#tbx3").val() + ($("#tbx5").val() == "" ? "" : "," + $("#tbx5").val());
    recalc.blockUI();
    var IBCBetTypeID = $("#hiddenBetType").val();
    $.ajax({
        type: "POST",
        url: "service/SettleLiveCasinoService.svc/ReCalc",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        data: '{"MatchId":' + $("#hiddenMatchID").val() + ',"IBCBetTypeID":' + IBCBetTypeID + ' , "BankerCard": "' + bankerCard.toUpperCase()
        + '", "PlayerCard": "' + playerCard.toUpperCase() + '", "AdminID": ' + $("#hiddenAdminID").val() + ', "AdminName": "' + $("#hiddenAdminName").val()
        + '", "ShowDate": "' + $("#hiddenShowDate").val() + '"}',
        success: function (rs) {
            recalc.unBlock();
            if (rs != "") {
                $("#msg").css("border-color", "red").css("color", "red").html(rs).show();
            }
            else {
                $("#msg").css("border-color", "green").css("color", "green").html("ReSettlement is successfully!").show();
                var timerId = setTimeout(function () {
                    clearTimeout(timerId);
                    if (parent.parent.settlelc && parent.parent.settlelc.getData) {
                        parent.parent.settlelc.getData();
                    }
                }, 2000);
            }
        },
        error: function () {
            recalc.unBlock();
        }
    });
};
recalc.routleCalc = function () {
    $("#msg").hide();
    var cardResult = $("#txtRoutleFinalScore").val();

    if (($("#txtRoutleFinalScore").val().length > 2) | cardResult > 36 | ($("#txtRoutleFinalScore").val().length == 0) | isNaN(cardResult)) {
        $("#msg").html("Ball Number is invalid").show();
        return;
    }

    if (!confirm("Are you sure you want to Re-Calculate this game?")) return;

    var bankerCard = 0;
    var playerCard = cardResult;
    recalc.blockUI();
    var IBCBetTypeID = $("#hiddenBetType").val();
    $.ajax({
        type: "POST",
        url: "service/SettleLiveCasinoService.svc/ReCalc",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        data: '{"MatchId":' + $("#hiddenMatchID").val() + ',"IBCBetTypeID":' + IBCBetTypeID + ' , "BankerCard": "' + bankerCard
        + '", "PlayerCard": "' + playerCard + '", "AdminID": ' + $("#hiddenAdminID").val() + ', "AdminName": "' + $("#hiddenAdminName").val()
        + '", "ShowDate": "' + $("#hiddenShowDate").val() + '"}',
        success: function (rs) {
            recalc.unBlock();
            if (rs != "") {
                $("#msg").css("border-color", "red").css("color", "red").html(rs).show();
            }
            else {
                $("#msg").css("border-color", "green").css("color", "green").html("ReSettlement is successfully!").show();
                var timerId = setTimeout(function () {
                    clearTimeout(timerId);
                    if (parent.parent.settlelc && parent.parent.settlelc.getData) {
                        parent.parent.settlelc.getData();
                    }
                }, 2000);
            }
        },
        error: function () {
            recalc.unBlock();
        }
    });
};

recalc.closePopup = function () {
    if (typeof parent.ClosePopup != "undefined") {
        parent.ClosePopup();
    }
};

recalc.blockUI = function () {

    try { parent.parent.parent.BlockUI(); } catch (e) { try { parent.parent.BlockUI(); } catch (e) { parent.BlockUI(); } }

};

recalc.unBlock = function () {
    try { parent.parent.parent.$.unblockUI(); } catch (e) { try { parent.parent.$.unblockUI(); } catch (e) { parent.BlockUI(); } }
};

$(document).ready(function () {
    recalc.init();
});

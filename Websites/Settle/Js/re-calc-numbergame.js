var recalc = {
};

$(document).ready(function () {
    recalc.init();
});

recalc.init = function () {
    $("#popup_recalc_numbergame").find("td[title]").html("NUMBER GAME NO " + matchcode + " - " + kickoffdate);

    var cbxSetNaSeq1 = document.getElementById("cbxSetNaSeq1");
    cbxSetNaSeq1.checked = false;
    cbxSetNaSeq1.disabled = false;

    var cbxSetNaSeq2 = document.getElementById("cbxSetNaSeq2");
    cbxSetNaSeq2.checked = false;
    cbxSetNaSeq2.disabled = false;

    var cbxSetNaSeq3 = document.getElementById("cbxSetNaSeq3");
    cbxSetNaSeq3.checked = false;
    cbxSetNaSeq3.disabled = false;

    var getBall = function (seq) {
        var number = 0;
        for (var i = 0; i < balls.length; ++i) {
            if (balls[i].Seq == seq) {
                number = balls[i].Number;
                break;
            }
        }
        return number == 0 ? "" : number;
    };

    var tbxResultReCal1 = document.getElementById("tbxResultReCal1");
    $(tbxResultReCal1).val(getBall(1)).show().parent().find("span").hide();

    var tbxResultReCal2 = document.getElementById("tbxResultReCal2");
    $(tbxResultReCal2).val(getBall(2)).show().parent().find("span").hide();

    var tbxResultReCal3 = document.getElementById("tbxResultReCal3");
    $(tbxResultReCal3).val(getBall(3)).show().parent().find("span").hide();

    $("#popup_recalc_numbergame div[msg]").hide();

    $(tbxResultReCal1).ForceDecimalOnly();
    $(tbxResultReCal2).ForceDecimalOnly();
    $(tbxResultReCal3).ForceDecimalOnly();

    $("#btnSubmit").click(function () {
        recalc.reCalcNumberGame();
    });
};

recalc.blockUI = function() {
    if (parent.parent.BlockUI) {
        try { parent.parent.BlockUI(); } catch (e) { }
    }
    else {
        try { parent.BlockUI(); } catch (e) { }
    }
};

recalc.unBlock = function () {
    
    try { parent.parent.jQuery.unblockUI(); } catch (e) { try { parent.jQuery.unblockUI(); } catch (e) { try { jQuery.unblockUI(); } catch (e) { } } }
};

recalc.reCalcNumberGame = function () {
    $("#popup_recalc_numbergame div[msg]").hide();

    var cbxSetNaSeq1 = document.getElementById("cbxSetNaSeq1");
    var cbxSetNaSeq2 = document.getElementById("cbxSetNaSeq2");
    var cbxSetNaSeq3 = document.getElementById("cbxSetNaSeq3");

    var tbxResultReCal1 = document.getElementById("tbxResultReCal1");
    var tbxResultReCal2 = document.getElementById("tbxResultReCal2");
    var tbxResultReCal3 = document.getElementById("tbxResultReCal3");

    var firstball_value = 0;
    if (!cbxSetNaSeq1.checked && $(tbxResultReCal1).val() != '') {
        firstball_value = parseInt($(tbxResultReCal1).val());
    }

    var secondball_value = 0;
    if (!cbxSetNaSeq2.checked && $(tbxResultReCal2).val() != '') {
        secondball_value = parseInt($(tbxResultReCal2).val());
    }

    var thirdball_value = 0;
    if (!cbxSetNaSeq3.checked && $(tbxResultReCal3).val() != '') {
        thirdball_value = parseInt($(tbxResultReCal3).val());
    }

    if (firstball_value < 0 || firstball_value > 75) {
        $("#popup_recalc_numbergame div[msg]").html("First ball value is invalid!").show();
        return;
    }

    if (secondball_value < 0 || secondball_value > 75) {
        $("#popup_recalc_numbergame div[msg]").html("Second ball value is invalid!").show();
        return;
    }

    if (thirdball_value < 0 || thirdball_value > 75) {
        $("#popup_recalc_numbergame div[msg]").html("Third ball value is invalid!").show();
        return;
    }

    recalc.blockUI();

    $.ajax({
        type: 'POST',
        url: 'Service/SettlementService.svc/ReCalcNumberGame',
        contentType: "application/json; charset=utf-8",
        async: false,
        data: '{\"matchid\": ' + matchid
        + ', \"firstBallvalue\": ' + firstball_value
        + ', \"secondBallValue\": ' + secondball_value
        + ', \"thirdBallValue\": ' + thirdball_value + '}',
        success: function (rs) {
            if (rs && rs.length > 0) {
                alert(rs);
            }
            else {                
                if (parent.addTimerId_ReCalNumberGame) {
                    var ballNo = '';
                    if (firstball_value < 10)
                        firstball_value = '0' + firstball_value;
                    if (secondball_value < 10)
                        secondball_value = '0' + secondball_value;
                    if (thirdball_value < 10)
                        thirdball_value = '0' + thirdball_value;
                    ballNo = '' + firstball_value + secondball_value + thirdball_value;
                    if (firstball_value == 0)
                        ballNo = '000000';
                    else if (secondball_value == 0) {
                        ballNo = '' + firstball_value + '0000'
                    }

                    $.ajax({
                        type: 'POST',
                        url: 'Service/SettlementService.svc/UpdBallNoByMatchID',
                        contentType: "application/json; charset=utf-8",
                        async: false,
                        data: '{\"matchid\": ' + matchid
                    + ', \"ballNo\": ' + ballNo + '}',
                        success: function () {

                        }
                    });

                    parent.addTimerId_ReCalNumberGame();
                }
            }

            recalc.unBlock();
            if (parent.ClosePopup) parent.ClosePopup();
        },
        error: function () {
            recalc.unBlock();
        }
    });
};

function setNASequence1() {
    var cbxSetNaSeq1 = document.getElementById("cbxSetNaSeq1");
    var cbxSetNaSeq2 = document.getElementById("cbxSetNaSeq2");
    var cbxSetNaSeq3 = document.getElementById("cbxSetNaSeq3");

    var tbxResultReCal1 = document.getElementById("tbxResultReCal1");
    var tbxResultReCal2 = document.getElementById("tbxResultReCal2");
    var tbxResultReCal3 = document.getElementById("tbxResultReCal3");

    if (cbxSetNaSeq1.checked) {
        $(tbxResultReCal1).hide().parent().find("span").show();

        cbxSetNaSeq2.checked = true;
        cbxSetNaSeq2.disabled = true;
        $(tbxResultReCal2).hide().parent().find("span").show();

        cbxSetNaSeq3.checked = true;
        cbxSetNaSeq3.disabled = true;
        $(tbxResultReCal3).hide().parent().find("span").show();
    }
    else {
        $(tbxResultReCal1).show().parent().find("span").hide();

        cbxSetNaSeq2.disabled = false;
    }
};

function setNASequence2() {
    var cbxSetNaSeq2 = document.getElementById("cbxSetNaSeq2");
    var cbxSetNaSeq3 = document.getElementById("cbxSetNaSeq3");

    var tbxResultReCal2 = document.getElementById("tbxResultReCal2");
    var tbxResultReCal3 = document.getElementById("tbxResultReCal3");

    if (cbxSetNaSeq2.checked) {
        $(tbxResultReCal2).hide().parent().find("span").show();

        cbxSetNaSeq3.checked = true;
        cbxSetNaSeq3.disabled = true;
        $(tbxResultReCal3).hide().parent().find("span").show();
    }
    else {
        $(tbxResultReCal2).show().parent().find("span").hide();

        cbxSetNaSeq3.disabled = false;
    }
};

function setNASequence3 () {
    var cbxSetNaSeq1 = document.getElementById("cbxSetNaSeq1");
    var cbxSetNaSeq2 = document.getElementById("cbxSetNaSeq2");
    var cbxSetNaSeq3 = document.getElementById("cbxSetNaSeq3");

    var tbxResultReCal3 = document.getElementById("tbxResultReCal3");

    if (cbxSetNaSeq3.checked) {
        $(tbxResultReCal3).hide().parent().find("span").show();
    }
    else {
        $(tbxResultReCal3).show().parent().find("span").hide();
    }
};
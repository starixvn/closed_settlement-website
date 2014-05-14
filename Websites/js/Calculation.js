var gHasUpdated = false;
var isInputScoreDirectly = false;
function DoExit() {
    parent.CloseDiv(gHasUpdated);
}
function FromKP(e) {
    if (!e) e = window.event;
    var key = (e.keyCode) ? e.keyCode : e.which;
    if (key == 27) { DoExit(); return false; }
}

function OnKP(e, val) {
    if (!e) e = window.event;
    var key = (e.keyCode) ? e.keyCode : e.which;

    if (key == 13) {
        var _type = $('btnSubmit').value;
        if (_type == 'Calculate ALL') { // bill soccer only
            var _qstr = parent.location.search.substring(1);
            var _ext = '';
            if (_qstr == 'm=htoveronly') _ext = 'HT';
            if (_qstr == 'm=overonly') _ext = 'FT';
            DoCalculate('bill', _ext);
            return false;
        }
        if (_type == 'BILL') DoCalculate('bill');
        if (_type == 'KEY SCORE') DoCalculate('keyscore');
        return false;
    }
    if (key == 27) { DoExit(); return false; }
    if ((key < 33) || (key == 37) || (key == 39) || (key == 46)) return true;
    if (/[^0-9]/.test(String.fromCharCode(key))) return false;
    return true;
}

function OnKP_HR(e, val) {
    if (!e) e = window.event;
    var key = (e.keyCode) ? e.keyCode : e.which;


    if (key == 27) { DoExit(); return false; }
    if ((key < 33) || (key == 37) || (key == 39) || (key == 46)) return true;
    //if (/^$/.test(val.value) && /0/.test(String.fromCharCode(key))) return false;
    if (/[^0-9]/.test(String.fromCharCode(key))) return false;
    return true;
}



function OnKUSoccer() {
    try {
        $('errMsg').style.display = 'none';
        var _homeH = $('homeH').value.trim();
        var _homeF = $('homeF').value.trim();
        var _awayH = $('awayH').value.trim();
        var _awayF = $('awayF').value.trim();
        if (_homeH == '0') _homeH = '';
        if (_homeF == '0') _homeF = '';
        if (_awayH == '0') _awayH = '';
        if (_awayF == '0') _awayF = '';

        if (_homeH == '' && _homeF == '' && _awayH == '' && _awayF == '') {
            $('chkFgHome').checked = false;
            $('chkLgHome').checked = false;
            $('chkFgAway').checked = false;
            $('chkLgAway').checked = false;
            $('chkFgHome').disabled = 'disable';
            $('chkLgHome').disabled = 'disable';
            $('chkFgAway').disabled = 'disable';
            $('chkLgAway').disabled = 'disable';
        }
        else {
            $('chkFgHome').disabled = '';
            $('chkLgHome').disabled = '';
            $('chkFgAway').disabled = '';
            $('chkLgAway').disabled = '';
        }
    }
    catch (ex) { }
}

//-- Basketball, Football
function OnKUBasketball(e) {
    $('errMsg').style.display = 'none';

    var totalHH = 0, totalHA = 0;
    var totalFH = 0, totalFA = 0;
    var i = 1;
    for (var i = 1; i < 5; i++) {
        var _scurH = $('txt' + i + 'qHome').value.trim();
        var _scurA = $('txt' + i + 'qAway').value.trim();
        if (_scurH == '') _scurH = '0';
        if (_scurA == '') _scurA = '0';
        var currH = (/[^0-9]/.test(_scurH)) ? 0 : parseInt(_scurH, 10);
        var currA = (/[^0-9]/.test(_scurA)) ? 0 : parseInt(_scurA, 10);
        if ($('txt' + i + 'qHome').value.trim() != '0') $('txt' + i + 'qHome').value = (currH == 0) ? '' : currH.toString();
        if ($('txt' + i + 'qAway').value.trim() != '0') $('txt' + i + 'qAway').value = (currA == 0) ? '' : currA.toString();
        totalFH += currH;
        totalFA += currA;
        if (i == 2) {
            totalHH = totalFH;
            totalHA = totalFA;
        }
    }
    totalFH += ($('txtOTHome').value == '') ? 0 : parseInt($('txtOTHome').value, 10);
    totalFA += ($('txtOTAway').value == '') ? 0 : parseInt($('txtOTAway').value, 10);

    $('homeH').value = totalHH.toString();
    $('awayH').value = totalHA.toString();
    $('homeF').value = totalFH.toString();
    $('awayF').value = totalFA.toString();

    if ($('IsHTOnly').value == '1') {
        $('homeF').value = '';
        $('awayF').value = '';
    }


}

//-- Hockey
function OnKUHockey(e) {
    $('errMsg').style.display = 'none';

    var totalFH = 0, totalFA = 0;
    var i = 1;
    for (var i = 1; i < 4; i++) {
        var _scurH = $('txt' + i + 'pHome').value.trim();
        var _scurA = $('txt' + i + 'pAway').value.trim();
        if (_scurH == '') _scurH = '0';
        if (_scurA == '') _scurA = '0';
        var currH = (/[^0-9]/.test(_scurH)) ? 0 : parseInt(_scurH, 10);
        var currA = (/[^0-9]/.test(_scurA)) ? 0 : parseInt(_scurA, 10);
        if ($('txt' + i + 'pHome').value.trim() != '0') $('txt' + i + 'pHome').value = (currH == 0) ? '' : currH.toString();
        if ($('txt' + i + 'pAway').value.trim() != '0') $('txt' + i + 'pAway').value = (currA == 0) ? '' : currA.toString();
        totalFH += currH;
        totalFA += currA;
    }
    totalFH += ($('txtOTHome').value == '') ? 0 : parseInt($('txtOTHome').value, 10);
    totalFA += ($('txtOTAway').value == '') ? 0 : parseInt($('txtOTAway').value, 10);
    $('homeF').value = totalFH.toString();
    $('awayF').value = totalFA.toString();
}

//-- Tennis
function OnKUTennis(e) {
    $('errMsg').style.display = 'none';

    var totalGH = 0, totalGA = 0;
    var totalFH = 0, totalFA = 0;
    var i = 1;
    for (var i = 1; i < 6; i++) {
        var _scurH = $('txt' + i + 'sHome').value.trim();
        var _scurA = $('txt' + i + 'sAway').value.trim();
        if (_scurH == '') _scurH = '0';
        if (_scurA == '') _scurA = '0';
        var currH = (/[^0-9]/.test(_scurH)) ? 0 : parseInt(_scurH, 10);
        var currA = (/[^0-9]/.test(_scurA)) ? 0 : parseInt(_scurA, 10);
        if ($('txt' + i + 'sHome').value.trim() != '0') $('txt' + i + 'sHome').value = (currH == 0) ? '' : currH.toString();
        if ($('txt' + i + 'sAway').value.trim() != '0') $('txt' + i + 'sAway').value = (currA == 0) ? '' : currA.toString();
        totalGH += currH;
        totalGA += currA;
        if (currH > currA) {
            totalFH++;
        }
        else if (currH < currA) {
            totalFA++;
        }
    }
    $('txtTGHome').value = totalGH.toString();
    $('txtTGAway').value = totalGA.toString();
    $('txtFSHome').value = totalFH.toString();
    $('txtFSAway').value = totalFA.toString();
}

//-- Badminton, Volleyball
function OnKUBadminton(e) {
    $('errMsg').style.display = 'none';

    var totalGH = 0, totalGA = 0;
    var totalFH = 0, totalFA = 0;
    var i = 1;
    for (var i = 1; i < 6; i++) {
        var _scurH = $('txt' + i + 'sHome').value.trim();
        var _scurA = $('txt' + i + 'sAway').value.trim();
        if (_scurH == '') _scurH = '0';
        if (_scurA == '') _scurA = '0';
        var currH = (/[^0-9]/.test(_scurH)) ? 0 : parseInt(_scurH, 10);
        var currA = (/[^0-9]/.test(_scurA)) ? 0 : parseInt(_scurA, 10);
        if ($('txt' + i + 'sHome').value.trim() != '0') $('txt' + i + 'sHome').value = (currH == 0) ? '' : currH.toString();
        if ($('txt' + i + 'sAway').value.trim() != '0') $('txt' + i + 'sAway').value = (currA == 0) ? '' : currA.toString();
        totalGH += currH;
        totalGA += currA;
        if (currH > currA) {
            totalFH++;
        }
        else if (currH < currA) {
            totalFA++;
        }
    }
    $('txtTPHome').value = totalGH.toString();
    $('txtTPAway').value = totalGA.toString();
    $('txtTSHome').value = totalFH.toString();
    $('txtTSAway').value = totalFA.toString();
}

function OnKUBaseBall(e) {
    $('errMsg').style.display = 'none';

    var totalFH = 0, totalFA = 0, totalHTH = 0, totalHTA = 0;
    for (var i = 1; i < 10; i++) {
        var _scurH = $('txt' + i + 'Home').value.trim();
        var _scurA = $('txt' + i + 'Away').value.trim();
        if (_scurH == '') _scurH = '0';
        if (_scurA == '') _scurA = '0';
        var currH = (/[^0-9]/.test(_scurH)) ? 0 : parseInt(_scurH, 10);
        var currA = (/[^0-9]/.test(_scurA)) ? 0 : parseInt(_scurA, 10);
        if ($('txt' + i + 'Home').value.trim() != '0') $('txt' + i + 'Home').value = (currH == 0) ? '' : currH.toString();
        if ($('txt' + i + 'Away').value.trim() != '0') $('txt' + i + 'Away').value = (currA == 0) ? '' : currA.toString();
        if (i < 6) {
            totalHTH += currH;
            totalHTA += currA;
        }
        totalFH += currH;
        totalFA += currA;
    }
    totalFH += ($('txtEIHome').value == '') ? 0 : parseInt($('txtEIHome').value, 10);
    totalFA += ($('txtEIAway').value == '') ? 0 : parseInt($('txtEIAway').value, 10);
    $('homeH').value = totalHTH.toString();
    $('awayH').value = totalHTA.toString();
    $('homeF').value = totalFH.toString();
    $('awayF').value = totalFA.toString();
}

function RefundRunningBetDaq(matchid, matchcode) {
    try { parent.parent.BlockUI(); } catch (e) { parent.BlockUI(); }
    parent.jQuery.ajax({
        type: "POST",
        url: "Service/SettlementService.svc/RefundRunningBetDaqTickets",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        data: '{"Matchid":' + matchid + '}',
        success: function (rs) {
            try { parent.parent.jQuery.unblockUI(); } catch (e) { try { parent.jQuery.unblockUI(); } catch (e) { jQuery.unblockUI(); } }

            if (rs != "" && rs != null && typeof rs != "undefined") {
                jQuery('#errmsg').html(rs).show();
            }
            else {
                parent.ClosePopup();
                try { parent.frames['centerFr'].GetSettleMatch(); } catch (e) { parent.GetSettleMatch(); }
            }
        }
    });
}

function RefundRunningBetFair(matchid, matchcode) {
    try { parent.parent.BlockUI(); } catch (e) { parent.BlockUI(); }
    parent.jQuery.ajax({
        type: "POST",
        url: "Service/SettlementService.svc/RefundRunningBetFairTickets",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        data: '{"Matchid":' + matchid + '}',
        success: function (rs) {
            try { parent.parent.jQuery.unblockUI(); } catch (e) { try { parent.jQuery.unblockUI(); } catch (e) { jQuery.unblockUI(); } }

            if (rs != "" && rs != null && typeof rs != "undefined") {
                jQuery('#errmsg').html(rs).show();
            }
            else {
                parent.ClosePopup();
                try { parent.frames['centerFr'].GetSettleMatch(); } catch (e) { parent.GetSettleMatch(); }
            }
        }
    });
}

function DoCalculate(type, ext) {
    DoCalculate_PreventWaitingList(type, ext);
}

function DoCalculate_PreventWaitingList(type, ext) {
    if (parent.settlementManagement) {
        var calcStatusConstants = parent.settlementManagement.enums.CalcStatusConstants;
        var matchId = $('sParams').value.split('|')[0];
        jQuery.ajax({
            type: "POST",
            url: "../Settlement_CalculateMP/GetCalcMPStatus",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: JSON.stringify(null),
            beforeSend: function () {
            },
            success: function (rs) {
                var matchCalculateStatus = parent.settlementManagement.getMatchCalculateStatus(rs, matchId);
                if (matchCalculateStatus != null &&
                    matchCalculateStatus.Status != calcStatusConstants.Completed) {
                    alert("This match is in calculate queue (status {0})".replace(/\{0\}/g,
                            matchCalculateStatus.StatusName));
                } else {
                    DoCalculate_AfterCheckWaitingList(type, ext);
                }
            },
            error: function (event, jqXHR, ajaxSettings) {
            },
            complete: function () {
            }
        });
    } else {
        DoCalculate_AfterCheckWaitingList(type, ext);
    }
}

var matchValidator = {
    showError: function (errorMessage) {
        $('errMsg').style.display = '';
        $('errMsg').innerHTML = errorMessage;
        matchValidator.unblockUI();
    },
    unblockUI: function () {
        try { parent.parent.jQuery.unblockUI(); } catch (e) { }
    },
    isCorrectGoalRecordFT: function (homeScore, awayScore, goalRecord) {
        return matchValidator.isCorrectGoalRecord(homeScore, awayScore, goalRecord);
    },
    isCorrectGoalRecordHT: function (homeScore, awayScore, goalRecord) {
        var htScore = goalRecord.split(';')[0];
        return matchValidator.isCorrectGoalRecord(homeScore, awayScore, htScore);
    },
    isCorrectGoalRecord: function (homeScore, awayScore, goalRecord) {
        var isCorrectTotalScore = (homeScore + awayScore == goalRecord.length);
        var goalRecordHomeScore = ReplaceAll(ReplaceAll(goalRecord, 'a', ''), 'A', '').length;
        return isCorrectTotalScore &&
          goalRecordHomeScore == homeScore;
    }
};

function DoCalculate_AfterCheckWaitingList(type, ext) {
    var errorMessage = 'Goal record is not consistent with the score, please correct Goal record before calculating.';
    $('errMsg').style.display = 'none';
    //MatchID|SportID
    var sportID = parseInt($('sParams').value.split('|')[1], 10);
    //only for bill, soccer and FT or ALL
    if (sportID == 1 && (type == 'calculate' || type == 'bill')) {
        var goalrecord = $('GR').value;
        var htScore = goalrecord.split(';')[0];
        goalrecord = ReplaceAll(goalrecord, ';', '');

        var homeF = ($('homeF').value.length == 0) ? 0 : parseInt($('homeF').value);
        var awayF = ($('awayF').value.length == 0) ? 0 : parseInt($('awayF').value);
        var homeH = ($('homeH').value.length == 0) ? 0 : parseInt($('homeH').value);
        var awayH = ($('awayH').value.length == 0) ? 0 : parseInt($('awayH').value);

        if (type == 'calculate') {
            if (homeH > homeF || awayH > awayF) {
                homeF = homeH;
                awayF = awayH;
            }
        }

        var IsExistsNG = $('txtIsExistsNG').value == 'True';
        var IsLive = $('txtIsLive').value == 'True';
        var IsHedge = $('txtMatchCode').value.toLowerCase().indexOf('hedge') >= 0;
        if (!IsHedge &&
            (type == 'calculate' || type == 'bill') &&
            ($('sHasFGLG').value == '1' || IsExistsNG || $('sHas1HFGLG').value == '1' || $('sHas1HLG').value == '1')) {
            if (IsLive && ext != 'HT') {
                if (!matchValidator.isCorrectGoalRecordFT(homeF, awayF, goalrecord)) {
                    matchValidator.showError(errorMessage);
                    return;
                }
            }
            else {
                if (ext == 'HT') {
                    if (!matchValidator.isCorrectGoalRecordHT(homeH, awayH, htScore)) {
                        matchValidator.showError(errorMessage);
                        return;
                    }
                }
                else {
                    //Case H=0 A=0 -> GR = Empty
                    if (homeF + awayF == 0 && goalrecord.length > 0 && homeH + awayH != goalrecord.length) {
                        matchValidator.showError(errorMessage);
                        return false;
                    }
                    //Case H=0 A>0 --> GR = A only
                    if (homeF == 0 && awayF > 0 && (goalrecord.toLowerCase().indexOf("h") >= 0 || goalrecord.toLowerCase().indexOf("a") < 0)) {
                        matchValidator.showError(errorMessage);
                        return false;
                    }
                    //Case H>=1 A=0 -->GR = H only

                    if (homeF >= 1 && awayF == 0 && (goalrecord.toLowerCase().indexOf("a") >= 0 || goalrecord.toLowerCase().indexOf("h") < 0)) {
                        matchValidator.showError(errorMessage);
                        return false;
                    }

                    if (homeF == 1 && awayF == 1 && (goalrecord.toLowerCase().indexOf("a") < 0 || goalrecord.toLowerCase().indexOf("h") < 0 ||
                        goalrecord.toLowerCase().substring(0, 1) == goalrecord.toLowerCase().substring(goalrecord.length - 1, goalrecord.length))) {
                        matchValidator.showError(errorMessage);
                        return false;
                    }
                    //Case H=1 A>1 -->GR = Exist A
                    if (homeF == 1 && awayF > 1 && goalrecord.toLowerCase().indexOf("a") < 0) {
                        matchValidator.showError(errorMessage);
                        return false;
                    }
                    //Case H>1 A=1 -->GR = Exist H
                    if (homeF > 1 && awayF == 1 && goalrecord.toLowerCase().indexOf("h") < 0) {
                        matchValidator.showError(errorMessage);
                        return false;
                    }
                    if (homeF + awayF > 0 && goalrecord.length <= 0) {
                        matchValidator.showError(errorMessage);
                        return false;
                    }

                    if (((homeF + awayF) != goalrecord.length) || (ReplaceAll(ReplaceAll(goalrecord, 'a', ''), 'A', '').length != homeF)) {
                        matchValidator.showError(errorMessage);
                        return false;
                    }
                }
            }
        }
    }

    switch (type) {
        case 'bill':
            if (!isInputScoreDirectly && !CheckInputValue(sportID)) {
                try { parent.parent.jQuery.unblockUI(); } catch (e) { try { parent.jQuery.unblockUI(); } catch (e) { jQuery.unblockUI(); } }
                return;
            }
            if (typeof (ext) != 'undefined' && ext != '') {
                if (ext == 'HT') {
                    var s = '';

                    if ($('IsAlreadyRefundHT').value.toLowerCase() == 'true') {
                        s = 'This match is already refunded HT. All refund will be rollback if you calculate HT!\n';
                    }
                    s += 'Please double check the score!\nYou will not be able to cancel the FIRST HALF CALCULATION of this match!';

                    if (!confirm("WARNING!\n\n" + s)) {
                        try { parent.parent.jQuery.unblockUI(); } catch (e) { try { parent.jQuery.unblockUI(); } catch (e) { jQuery.unblockUI(); } }
                        return;
                    }
                    if (!confirm("Check again!\nAre you sure you want to CALCULATE FIRST HALF for this match?")) {
                        try { parent.parent.jQuery.unblockUI(); } catch (e) { }
                        return;
                    }
                    type = 'billHT';
                }
                else if (ext == 'FT') {
                    if (!confirm("WARNING!\n\nPlease double check the score!\nYou will not be able to cancel the FULL TIME CALCULATION of this match!")) {
                        try { parent.parent.jQuery.unblockUI(); } catch (e) { try { parent.jQuery.unblockUI(); } catch (e) { jQuery.unblockUI(); } }
                        return;
                    }
                    if (!confirm("Check again!\nAre you sure you want to CALCULATE only FULL TIME for this match?")) {
                        try { parent.parent.jQuery.unblockUI(); } catch (e) { try { parent.jQuery.unblockUI(); } catch (e) { jQuery.unblockUI(); } }
                        return;

                    }
                    type = 'billFT';
                }
            }
            else {
                if (!confirm("WARNING!\n\nPlease double check the score and confirm this match is OVER!\nYou will not be able to cancel the CALCULATION and REUSE this match!")) {
                    try { parent.parent.jQuery.unblockUI(); } catch (e) { try { parent.jQuery.unblockUI(); } catch (e) { jQuery.unblockUI(); } }
                    return;
                }
                if (!confirm("Check again!\nAre you sure you want to CALCULATE this match and it is OVER?")) {
                    try { parent.parent.jQuery.unblockUI(); } catch (e) { try { parent.jQuery.unblockUI(); } catch (e) { jQuery.unblockUI(); } }
                    return;
                }
            }

            break;
        case 'calculate':
            if (!confirm("Are you sure you want to CALCULATE First Goal and No Goal?")) {
                try { parent.parent.jQuery.unblockUI(); } catch (e) { try { parent.jQuery.unblockUI(); } catch (e) { jQuery.unblockUI(); } }
                return;
            }
            break;
        case 'keyscore':
            if (!isInputScoreDirectly && !CheckInputValue(sportID)) {
                try { parent.parent.jQuery.unblockUI(); } catch (e) { try { parent.jQuery.unblockUI(); } catch (e) { jQuery.unblockUI(); } }
                return;
            }
            break;
        case 'unsettle':
            if (!confirm("WARNING!\n\nPlease double check and confirm this match really needs to be UNSETTLED!")) {
                try { parent.parent.jQuery.unblockUI(); } catch (e) { try { parent.jQuery.unblockUI(); } catch (e) { jQuery.unblockUI(); } }
                return;
            }

            if (!confirm("Check again!\nAre you sure you want to UNSETTLE this match?")) {
                try { parent.parent.jQuery.unblockUI(); } catch (e) { try { parent.jQuery.unblockUI(); } catch (e) { jQuery.unblockUI(); } }
                return;
            }
            break;
        case 'refund':
            scoreConfirm(type);
            return;
        case 'refundfulltime':
            scoreConfirm(type);
            return;
        case 'refundhalftime':
            scoreConfirm(type);
            return;

        case 'refundhtfglg':
            if (!confirm("Are you sure you want to REFUND HT FGLG this match?")) {
                try { parent.parent.jQuery.unblockUI(); } catch (e) { try { parent.jQuery.unblockUI(); } catch (e) { jQuery.unblockUI(); } }
                return;
            }
            break;
        case 'refundftfglg':
            if (!confirm("Are you sure you want to REFUND FT FGLG this match?")) {
                try { parent.parent.jQuery.unblockUI(); } catch (e) { try { parent.jQuery.unblockUI(); } catch (e) { jQuery.unblockUI(); } }
                return;
            }
            break;
        case 'refundallfglg':
            if (!confirm("Are you sure you want to REFUND All FGLG this match?")) {
                try { parent.parent.jQuery.unblockUI(); } catch (e) { try { parent.jQuery.unblockUI(); } catch (e) { jQuery.unblockUI(); } }
                return;
            }
            break;
        case 'refundhtlastgoal':
            if (!confirm("Are you sure you want to REFUND HT Last Goal this match?")) {
                try { parent.parent.jQuery.unblockUI(); } catch (e) { try { parent.jQuery.unblockUI(); } catch (e) { jQuery.unblockUI(); } }
                return;
            }
            break;
        case 'refundftlastgoal':
            if (!confirm("Are you sure you want to REFUND FT Last Goal this match?")) {
                try { parent.parent.jQuery.unblockUI(); } catch (e) { try { parent.jQuery.unblockUI(); } catch (e) { jQuery.unblockUI(); } }
                return;
            }
            break;
        case 'refundalllastgoal':
            if (!confirm("Are you sure you want to REFUND All Last Goal this match?")) {
                try { parent.parent.jQuery.unblockUI(); } catch (e) { try { parent.jQuery.unblockUI(); } catch (e) { jQuery.unblockUI(); } }
                return;
            }
            break;
    }

    DoCalculate_Submit(type);
}
function DoCalculate_Submit(type) {
    $('sAction').value = type;
    try { parent.parent.BlockUI(); } catch (e) { try { parent.BlockUI(); } catch (e) { BlockUI(); } }
    var myajax = new Ajax.Request("Calculation.ashx", {
        asynchronous: true,
        method: 'post',
        parameters: Form.serialize('frmBill'),
        onComplete: ResultCalculate
    });
};

function DoCalculateHR() {
    $('errMsg').style.display = 'none';
    $('infoMsg').style.display = 'none';

    var checkplacing1_4 = 0;
    for (var i = 1; i < 5; i++) {

        if (document.getElementById('sHName_' + i) != null && document.getElementById('sHName_' + i)[document.getElementById('sHName_' + i).selectedIndex].value == "-") {
            checkplacing1_4 = 1;
        }
        if (i < 4 && document.getElementById('txtPlace_' + i) != null && trim(document.getElementById('txtPlace_' + i).value, ' ') == "") {
            checkplacing1_4 = 1;
        }
    }
    if (trim(document.getElementById('txtWin_1').value, ' ') == "") {
        checkplacing1_4 = 1;
    }
    if (checkplacing1_4 == 1) {
        $('errMsg').style.display = '';
    }
    else {
        $('infoMsg').style.display = '';
    }
}

function ResultCalculate(rs) {
    var IsSuccess = 0;
    var delay = 0;

    $('errMsg').style.display = 'none';
    var objRs = jQuery.parseJSON(rs.responseText);

    if ($('sAction').value == 'unsettle') {
        var myajax = new Ajax.Request("ReplicateData.ashx", {
            asynchronous: true,
            method: 'post',
            parameters: Form.serialize('frmBill') + "&successMatchIds=" + objRs.successMatchIds,
            onComplete: null
        });
    }

    if (objRs.responseText == 'success') {
        gHasUpdated = true;
        IsSuccess = 1;
    }
    else if (objRs.responseText == 'success-refund') {
        alert('Refund All Successful!');
        gHasUpdated = true;
        IsSuccess = 1;
    }
    else if (objRs.responseText == 'success-refundfulltime') {
        alert('Refund FT Successful!');
        gHasUpdated = true;
        IsSuccess = 1;
    }
    else if (objRs.responseText == 'FGLGsuccess') {
        alert('In case you have changed the result of First Goal/Last Goal here, please re-calculate the MULTIPLE also to make sure all the tickets are update correctly.');
        gHasUpdated = true;
        IsSuccess = 0;
    }
    else if (objRs.responseText == 'soccerSuccess') {
        $('infoMsg').style.display = '';
        //$('infoMsg').innerHTML = 'SUCCESS!';
        IsSuccess = 1; // that mean no need close the window settlement, 
    }
    else if (objRs.responseText == 'success-refundalllastgoal') {
        alert('Refund All Last Goal Successful!');
        gHasUpdated = true;
        IsSuccess = 1;
    }
    else if (objRs.responseText == 'Access Denied') {
        $('infoMsg').innerHTML = 'ACCESS IS DENIED!';
        $('infoMsg').style.display = '';
        IsSuccess = 0; // that mean no need close the window settlement, 
    }
    else if (objRs.responseText.split('|')[0] == 'no') {
        $('errmsg').innerHTML = '<br/>' + objRs.responseText.split('|')[1];
        $('errmsg').style.display = '';
        IsSuccess = 0;
    }
    else if (objRs.responseText.split('|')[0] == 'delay') {
        IsSuccess = 0;
        delay = parseInt(objRs.responseText.split('|')[1]);
    }
    else {
        $('_sContent').innerHTML = objRs.responseText;
        IsSuccess = 1;
    }

    if (IsSuccess == 1) {
        var hasBetdaq = objRs.isExcludeBetdaq || objRs.isExcludeBetfair;

        if (!hasBetdaq) {
            parent.jQuery("#dialog-modal").dialog('close');
            window.setTimeout(function () {
                try { parent.frames['centerFr'].GetSettleMatch() } catch (e) { parent.GetSettleMatch() }
            }, 2000);
        }
        else {
            try { parent.frames['centerFr'].GetSettleMatch() } catch (e) { parent.GetSettleMatch() }

            setTimeout(function () {
                var timeout = 2000;
                try { parent.parent.jQuery.unblockUI(); } catch (e) { try { parent.jQuery.unblockUI(); } catch (e) { jQuery.unblockUI(); } }

                if (objRs.isExcludeBetdaq && !objRs.isExcludeBetfair) {
                    parent.jQuery("#dialog-modal").dialog({ height: 150 });
                    timeout = 10000;
                    $('_sContent').innerHTML =
                        '<div style="color: #ff0000; margin-left: 20px"><font color="Blue">Refunded successfully.</font><br/><br/>' +
                        'Please move Betdaq tickets to a new match before ' +
                        objRs.kickoffBetdaq + '<br/>' +
                        'Otherwise they will bring harm to the system</div>';
                }
                else if (!objRs.isExcludeBetdaq && objRs.isExcludeBetfair) {
                    parent.jQuery("#dialog-modal").dialog({ height: 150 });
                    timeout = 10000;
                    $('_sContent').innerHTML =
                        '<div style="color: #ff0000; margin-left: 20px"><font color="Blue">Refunded successfully.</font><br/><br/>' +
                        'Please move Betfair tickets to a new match before ' +
                        objRs.kickoffBetfair + '<br/>' +
                        'Otherwise they will bring harm to the system</div>';
                }
                else if (objRs.isExcludeBetdaq && objRs.isExcludeBetfair) {
                    parent.jQuery("#dialog-modal").dialog({ height: 150 });
                    timeout = 10000;
                    $('_sContent').innerHTML =
                        '<div style="color: #ff0000; margin-left: 20px"><font color="Blue">Refunded successfully.</font><br/><br/>' +
                        'Please move Betdaq and Betfair tickets to a new match before ' +
                        objRs.kickoffBetdaq + '<br/>' +
                        'Otherwise they will bring harm to the system</div>';
                }

                setTimeout(function () {
                    parent.jQuery("#dialog-modal").dialog('close');
                }, timeout);
            }, 1000);
        }
    }
    else {
        try { parent.parent.jQuery.unblockUI(); } catch (e) { try { parent.jQuery.unblockUI(); } catch (e) { jQuery.unblockUI(); } }
        if (delay > 0) {
            SettleDelay(delay);
            //parent.parent.SettleDelay(delay);
        }
    }

}


/*Settlement delay*/
var totaltime = 0;
var t;

function timedCount() {

    if (totaltime > 0) {
        document.getElementById("lblduration").innerHTML = totaltime;
        totaltime--;
        t = setTimeout("timedCount()", 1000);
    }
    else {
        clearTimeout(t);
        overlay("overlaydiv");
        $("errmsg").style.display = 'none';
    }
}

function SettleDelay(duration) {
    totaltime = duration;
    $("errmsg").style.display = 'inline';
    overlay("overlaydiv");
    timedCount();

}
function overlay(id) {
    var el = $(id);
    if (el.style.display == 'none') {
        el.style.display = 'inline';
    }
    else {
        el.style.display = 'none';
    }
}


function CheckInputValue(sportID) {
    switch (sportID) {
        case 1: // soccer
            return true;
        case 2: // basketball
        case 3: // football
        case 26: // rugby    
        case 32: // netball        
            return IsValidScore(['txt1qHome', 'txt2qHome', 'txt3qHome', 'txt4qHome', 'txt1qAway', 'txt2qAway', 'txt3qAway', 'txt4qAway']);
        case 4: // ice hockey
        case 28: // field hockey
            return IsValidScore(['txt1pHome', 'txt2pHome', 'txt3pHome', 'txt1pAway', 'txt2pAway', 'txt3pAway']);
        case 5: // tennis
            return IsValidScore(['txt1sHome', 'txt2sHome', 'txt3sHome', 'txt4sHome', 'txt5sHome', 'txt1sAway', 'txt2sAway', 'txt3sAway', 'txt4sAway', 'txt5sAway']);
        case 10: // golf
        case 11: // Motorsports
        case 7: // billiard
        case 154:
            return true; //return IsValidScore(['homeF','awayF']);  
        case 8: // baseball
            return IsValidScore(['txt1Home', 'txt2Home', 'txt3Home', 'txt4Home', 'txt5Home', 'txt6Home', 'txt7Home', 'txt8Home', 'txt9Home', 'txt1Away', 'txt2Away', 'txt3Away', 'txt4Away', 'txt5Away', 'txt6Away', 'txt7Away', 'txt8Away', 'txt9Away']);
        case 9: // badminton
        case 6: // volleyball
            return IsValidScore(['txt1sHome', 'txt2sHome', 'txt3sHome', 'txt4sHome', 'txt5sHome', 'txt1sAway', 'txt2sAway', 'txt3sAway', 'txt4sAway', 'txt5sAway']);
        default: return true; //return IsValidScore(['homeH','awayH','homeF','awayF']);                        
    }
    return false;
}

function IsValidScore(arrItem) {
    var len = arrItem.length;
    if (len < 1) return true;
    for (var i = 0; i < len; i++) {
        if (!($(arrItem[i]).value).isNullOrEmpty()) return true;
    }
    $('errMsg').style.display = '';
    return false;
}

function IsValidSoccer() {
    if ($('chkFgHome') != null && $('chkFgHome').disabled == '') {
        try {
            var fg = Form.getInputs('frmBill', 'radio', 'chkFg').find(function (radio) { return radio.checked; }).value;
            var lg = Form.getInputs('frmBill', 'radio', 'chkLg').find(function (radio) { return radio.checked; }).value;
        }
        catch (ex) {
            $('errMsg').innerHTML = 'Please input First Goal / Last Goal!';
            $('errMsg').style.display = '';
            return false;
        }
    }
    return true;
}

window.onload = function () {


    if (!$('sParams')) return;
    //MatchID|SportID
    var sportID = $('sParams').value.split('|')[1];

    switch (parseInt(sportID, 10)) {
        case 1:
        case 2:
        case 3:
        case 9:
        case 32:
            if (parent.$('divMatch') && parent.$('frMatch')) {
                parent.$('divMatch').className = 'divMatch65';
                parent.$('frMatch').className = 'frMatch65';
            }
            break;
        default:
            if (parent.$('divMatch') && parent.$('frMatch')) {
                parent.$('divMatch').className = 'divMatch200';
                parent.$('frMatch').className = 'frMatch200';
            }
            break;
    }

    var IsRefund = $('IsRefund');
    if (IsRefund && IsRefund.value == '1') {

        if (sportID != 9) {
            if (parent.$('divMatch') && parent.$('frMatch')) {
                parent.$('divMatch').className = 'divMatch200';
                parent.$('frMatch').className = 'frMatch200';
            }
        }
        return;
    }

    switch (parseInt(sportID, 10)) {
        case 1: // soccer   
            if ($('homeH') != null) $('homeH').focus();
            else if ($('chkFgHome').disabled == '') $('chkFgHome').focus();
            break;
        case 2: // basketball            
        case 3: // football   
        case 32: // netball   
            $('txt1qHome').focus();
            break;
        case 4: // hockey
            $('txt1pHome').focus();
            break;
        case 5: // tennis
            $('txt1sHome').focus();
            break;
        case 10: // golf
        case 11: // Motorsports
        case 7: // billiard
        case 25: // Dart
        case 33: //     Cycling
        case 34: //     Fencing
        case 35: //     Judo
        case 36: //     Modern Pentathlon
        case 37: //     Rowing
        case 38: //     Sailing
        case 39: //     Shooting
        case 40: //     Taekwondo
        case 41: //     Triathlon
        case 42: //     Wrestling
        case 154:
            $('homeF').focus();
            break;
        case 8: // baseball
            $('txt1Home').focus();
            break;
        case 9: // badminton
        case 6: // volleyball
            $('txt1sHome').focus();
            break;
        default:
            $('homeH').focus();
            break;
    }
}

function ChangeTypeSoccer(calType) {
    //MatchID|SportID
    var mid = $('sParams').value.split('|')[0];
    var sid = $('sParams').value.split('|')[1];
    this.location.replace('Calculation.aspx?type=b&p=' + mid + '|' + sid + '&calType=' + calType);
}


function MoveBetdaqTickets(matchid, matchcode) {
    var height = 400;
    //parent.ibcWnd.open('SelectMatchMoveBet.aspx?matchid=' + matchid + '&matchcode=' + matchcode, 'Move Betdaq Tickets', 350, height, 270, 170, 'class1', 'class2');

    var url = 'SelectMatchMoveBet.aspx?matchid=' + matchid + '&matchcode=' + matchcode + '&isbetfair=0&sportType=' + $('sParams').value.split('|')[1] + '&v=' + new Date().toGMTString();
    var title = 'Move Betdaq Tickets';
    var width = 350;

    parent.OpenNextPopup('movebetdaq', url, title, width, height, true, false, window, 1000, null, null);

}

function MoveBetfairTickets(matchid, matchcode) {
    var height = 400;

    var url = 'SelectMatchMoveBet.aspx?matchid=' + matchid + '&matchcode=' + matchcode + '&isbetfair=1&sportType=' + $('sParams').value.split('|')[1] + '&v=' + new Date().toGMTString();
    var title = 'Move Betfair Tickets';
    var width = 350;

    parent.OpenNextPopup('movebetfair', url, title, width, height, true, false, window, 1000, null, null);
}
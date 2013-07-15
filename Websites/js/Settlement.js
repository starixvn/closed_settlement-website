var gSubmitParams = '';
var isRefresh = false;
var scrollTop = 0;
var isUpdateList = false;
function ClosePopupSettlement() {
    parent.dhtmlwindow.close(ctrWnd);
}

function ChangeSport() {
    var isCal = (jQuery('#m').val() == 'CALC');
    if (isCal) {

        try { parent.BlockUI(); } catch (e) { BlockUI(); }
        jQuery('#sContent').html('');
        var sportType = jQuery('#ddlSports').val();
        var myajax = new Ajax.Request("ChangeSport.ashx", {
            asynchronous: true,
            method: 'post',
            parameters: 'fdate=' + jQuery('#fdate').val() + '&tdate=' + jQuery('#tdate').val() + '&sportID=' + sportType,
            onComplete: BindLeagues
        });
    }
}

function GetDivClassSuffix(sportType) {
    switch (sportType) {
        case '8':
            return '78';
        case '1':
        case '2':
        case '3':
        case '5':
        case '6':
        case '9':
            return '65';
        default: return '58';
    }
}
function BindLeagues(rs) {
    jQuery('#selLeague').html(rs.responseText);
    try {
        parent.jQuery.unblockUI();
    } catch (e) { jQuery.unblockUI(); }
}

function GetSettleMatch() {
    try { parent.BlockUI(); } catch (e) { BlockUI(); }
    if (jQuery('#ddlLeagues')) {
        _leagueid = parseInt(jQuery('#ddlLeagues').val());
    }
    var isCal = (jQuery('#m').val() == 'CALC') ? 1 : 0;
    var params = 'mode=' + jQuery('#m').val();
    params += '&fdate=' + jQuery('#fdate').val();
    params += '&sportID=' + jQuery('#ddlSports').val();
    if (isCal == 1) {
        params += '&tdate=' + jQuery('#tdate').val();
        params += '&leagueID=' + jQuery('#ddlLeagues').val();
        params += '&isUncal=' + ((jQuery('#chkUnCal').attr('checked')) ? 1 : 0);
        params += '&isUncalMul=' + ((jQuery('#chkUnCalMultiple').attr('checked')) ? 1 : 0);
    }
    gSubmitParams = params;

    isRefresh = false;
    var myajax = new Ajax.Request("GetSettleMatch.ashx", {
        asynchronous: true,
        method: 'post',
        parameters: params, //isCalculate | fdate | tdate | sportID | leagueID | isUncal | isUncalMul        
        onComplete: BindSettleMatch
    });
}

function BindSettleMatch(rs) {
    jQuery('#sContent').html(rs.responseText);
    //FinishLoading();
    jQuery("#divcontainer").removeClass("allhidden");

    initMultipleCalculate();
    settlementManagement.getCalcMPStatus();
    settlementManagement.updateDomAfterGetStatus();
    try { parent.jQuery.unblockUI(); } catch (e) { jQuery.unblockUI(); }
}

function Refresh() {
    //BeginLoading();
    try { parent.BlockUI(); } catch (e) { BlockUI(); }
    //    isRefresh = true;
    //    if (isRefresh) {
    //        var container = settlementManagement.cachedData.tables.report.find(".container");
    //        scrollTop = container.scrollTop();
    //    }

    var myajax = new Ajax.Request("GetSettleMatch.ashx", {
        asynchronous: true,
        method: 'post',
        parameters: gSubmitParams, //isCalculate | fdate | tdate | sportID | leagueID | isUncal | isUncalMul        
        onComplete: BindSettleMatch
    });
}

function CloseDiv(hasUpdated) {
    if (hasUpdated) Refresh();
    jQuery('#divMatch').hide();
    self.frames['frMatch'].jQuery('#_sContent').html('');
    this.focus();
}

function DoCal(type, params) {
    settlementManagement.doCal(type, params);
}

function DoCalHR(type, params) {
    var url = 'CalculationHR.aspx?p=' + params + '&type=' + type;

    ShowPopup(url, 'Settlement', 700, 340, 50, 200);
}

function DoCalHR1(obj) {
    var Placing;
    var SCR = 0;
    var TF = 0;
    var Win = 0;
    var Place = 0
    Placing = $(obj.id.replace(/_BtnKey/, "")).options[$(obj.id.replace(/_BtnKey/, "")).selectedIndex].value;
    Win = $(obj.id.replace(/_BtnKey/, "_Win")).value;
    Place = $(obj.id.replace(/_BtnKey/, "_Place")).value;
    SCR = $(obj.id.replace(/_BtnKey/, "_SCR")).checked ? 1 : 0;
    TF = $(obj.id.replace(/_BtnKey/, "_TF")).checked ? 1 : 0;
    alert("Placing: " + Placing + ", SCR: " + SCR + ", TF: " + TF + ", Win: " + Win + ", Place:" + Place);
}

function DoCalFGLG(params) {
    var height = 280;
    var url = 'Calculation.aspx?type=b&p=' + params + '&calType=fglg';
    ShowPopup(url, 'Settlement', 600, height, 50, 200);
}

function DoCalSoccer(matchID, sportID) {
    var url = 'Calculation.aspx?type=b&p=' + matchID + '|' + sportID + '&calType=main';
    var height = 400;
    ShowPopup(url, 'Settlement', 600, height, 50, 200);
}

function DoCal2_PreventWaitingList(type, ext) {
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
                    DoCal2_AfterCheckWaitingList(type, ext);
                }
            },
            error: function (event, jqXHR, ajaxSettings) {
            },
            complete: function () {
            }
        });
    } else {
        DoCal2_AfterCheckWaitingList(type, ext);
    }
}

function DoCal2_AfterCheckWaitingList(type, params) {
    var sConfirm = '';
    var matchId = params.split("|")[0];

    switch (type) {
        case 'm':
            sConfirm = 'Are you sure you want to CALCULATE MULTIPLE This match?';
            break;
        case 'r':
            sConfirm = 'Are you sure you want to REFUND MULTIPLE This match?';
            break;

        case 'rht':
            sConfirm = 'Are you sure you want to REFUND MULTIPLE HT This match?';
            break;

        case 'rft':
            sConfirm = 'Are you sure you want to REFUND MULTIPLE FT This match?';
            break;
        default:
            sConfirm = 'Invalid type, Please try again!!!';
            alert(sConfirm);
            return;
    }
    if (parseInt(params.split("|")[1]) != 0) {
        if (!confirm(sConfirm)) return;
    }
    //BeginLoading();
    if (parent.parent.BlockUI) {
        try { parent.parent.BlockUI(); } catch (e) { }
    }
    else {
        try { parent.BlockUI(); } catch (e) { BlockUI(); }
    }

    if (type == "m") {
        settlementManagement.doCalcMP(type, params);
    }
    else {
        var myajax = new Ajax.Request("Multiple.ashx", {
            asynchronous: true,
            method: 'post',
            parameters: 'p=' + params + '&type=' + type,
            onComplete: function (rs) {
                RefundResult(rs, type, matchId);
            }
        });
    }
};

function DoCal2(type, params) {
    DoCal2_PreventWaitingList(type, params);
}

function MultipleResult(rs, type, matchId) {
    //FinishLoading();
    if (parent.parent.jQuery && parent.parent.jQuery.unblockUI) {
        parent.parent.jQuery.unblockUI();
    }
    else {
        try { parent.jQuery.unblockUI(); } catch (e) { jQuery.unblockUI(); }
    }

    jQuery("#chk_all").attr("checked", false);
    jQuery(".chkmultiple").attr("checked", false);
    settlementManagement.updateCalculateMatchQueue(calcStatusList, false);
    settlementManagement.updateDomAfterGetStatus();

    jQuery("#msgResult").html(rs.responseText);
    var result = rs.responseText;
    if (result == 'Calculate Multiple Successfully!' || result == 'Refund Multiple Successfully!' || result == 'Refund Multiple FT Successfully!') {
        Refresh();
    }
    else {
        jQuery("#dialog-result").data('result', rs.responseText).dialog("open");
    }

    try {
        apollo.updateConfirmedAction_Parlay(matchId, type);
    }
    catch (e) {
    }

    if (jQuery('#divMatch')) jQuery('#divMatch').hide();
    parent.ClosePopup();
}
function RefundResult(rs, type, matchId) {
    //FinishLoading();

    try { parent.parent.jQuery.unblockUI(); } catch (e) { }
    var result = rs.responseText;

    try {
        parent.apollo.updateConfirmedAction_Parlay(matchId, type);
    }
    catch (e) {
    }

    if (jQuery('#divMatch')) jQuery('#divMatch').hide();

    if (result == 'Refund Multiple Successfully!' || result == 'Refund Multiple FT Successfully!' || result == 'Refund Multiple HT Successfully!') {
        try { parent.frames['centerFr'].Refresh(); } catch (e) { parent.Refresh(); }
    } else {
        alert(result);
    }

    try { parent.jQuery.unblockUI(); } catch (e) { jQuery.unblockUI(); }
    parent.ClosePopup();
}

function GetMatchSettleLog() {
    jQuery('#spMsg').html('');
    if (jQuery('#txtMatchCode').val().length == 0) {
        showAlertMessage("spMsg", "Please input a match code", 1);
        jQuery('#txtMatchCode').focus();
        return;
    }
    if (parent.parent.BlockUI) {
        try { parent.parent.BlockUI(); } catch (e) { }
    }
    else {
        try { parent.BlockUI(); } catch (e) { BlockUI(); }
    }

    var param = 'mode=3&matchcode=' + jQuery('#txtMatchCode').val().replace('&', '^');
    param += '&fdate=' + jQuery('#fromdate').val();
    param += '&tdate=' + jQuery('#todate').val();
    param += '&t=' + jQuery('#t').val();
    jQuery.ajax({
        type: 'POST',
        url: '../Settle/ProcessData.ashx',
        async: true,
        data: param,
        success: function (rs) {
            jQuery('#sData').html(rs);
            InitFixedHeaderLog(800);
            if (parent.parent.jQuery && parent.parent.jQuery.unblockUI) {
                parent.parent.jQuery.unblockUI();
            }
            else {
                try { parent.jQuery.unblockUI(); } catch (e) { jQuery.unblockUI(); }
            }
        }
    });

}

function InitFixedHeaderLog(widthmainpage) {

    if (jQuery('#trFooter').length == 0) return;

    jQuery('#trFooter').show();
    var topHeader = jQuery('#trHeader').position().top;
    var topFooter = jQuery('#trFooter').position().top;
    jQuery('#trFooter').hide();

    var innerHeight = window.innerHeight;
    var delta = 0;
    if (!innerHeight) {
        innerHeight = document.documentElement.clientHeight;
        delta = 0;
    }

    var RealHeight = jQuery(window).height() - topHeader - 10;

    if ((topFooter - topHeader) > jQuery(window).height()) {
        jQuery('#table').Scrollable(RealHeight, widthmainpage);
    }
    else {
        jQuery('#table .rspace').removeClass('rspace');
    }

}

function ShowSettlementLog(matchcode, fdate, tdate) {
    var now = new Date();
    var dateFormat = (now.getMonth() + 1) + '/' + now.getDate() + '/' + now.getFullYear();
    fdate = fdate ? fdate : dateFormat;
    tdate = tdate ? tdate : dateFormat;

    matchcode = matchcode.replace('&', '^');
    var title = 'SETTLEMENT LOG';
    var url = 'GetSettleLog.aspx?matchcode=' + matchcode + '&fdate=' + fdate + '&tdate=' + tdate;
    var width = 950;
    var height = 600;
    OpenPopup(url, title, width, height, true, true, window, 2000, null, null);
}

function CloseDiv() {
    try { ibcWnd.close(); } catch (e) { }
}

function DisplayMatchInfoSettleLog(matchid) {
    var matchinfo = document.getElementById(matchid);
    matchinfo.className = (matchinfo.className.length == 0) ? 'hide' : '';
}

function refundNumberGame(matchId) {
    numgamerefund.confirm(matchId, 'ht');
    return;

    if (!confirm('Are you sure you want to Refund this match?')) return;
    var url = 'CalculationNumberGame.ashx?action=refund' + '&matchid=' + matchId;
    try { parent.BlockUI(); } catch (e) { BlockUI(); }
    var myajax = new Ajax.Request(url, {
        asynchronous: true,
        method: 'post',
        parameters: '',
        onComplete: refundNumberGameResult
    });
}

function refundNumberGameFt(matchId) {
    numgamerefund.confirm(matchId, 'ft');
    return;

    if (!confirm('Are you sure you want to Refund this match?')) return;
    var url = 'CalculationNumberGame.ashx?action=refundft' + '&matchid=' + matchId;
    try { parent.BlockUI(); } catch (e) { BlockUI(); }
    var myajax = new Ajax.Request(url, {
        asynchronous: true,
        method: 'post',
        parameters: '',
        onComplete: refundNumberGameResult
    });
}

function loadNumberGameResult() {
    try { parent.jQuery.unblockUI(); } catch (e) { jQuery.unblockUI(); }
    GetSettleMatch();
}

function refundNumberGameResult(rs) {
    //FinishLoading();
    if (rs.split('|')[0] == 'no') {
        alert(rs.split('|')[1]);
    }
    numgamerefund.closeNumbergamePopup();
    setTimeout('loadNumberGameResult()', 1000);
}
///
/// Number game functons
///
var numgamerefund = {
    curMatchId: 0,
    message: { title: "Number Game - Refund{type} confirmation",
        errorMatch: "Number Game: Match invalid.",
        errorMessage: "Number Game: Refund message invalid."
    },
    confirm: function (matchId, type) {
        if (parseInt(matchId) > 0) {
            this.curMatchId = parseInt(matchId);
            var title = "", width = 520, height = 330;
            if (type.toUpperCase() == 'FT') {
                title = this.message.title.replace(/{type}/g, " " + type.toUpperCase());
            } else {
                title = this.message.title.replace(/{type}/g, "");
            }

            var template = jQuery("#numbergame_templaterf").html();
            template = template.replace(/{matchid}/gi, matchId);
            template = template.replace(/{type}/gi, type);
            // global popup 
            //var left = (jQuery('body').width() / 2) - (width / 2);
            //ctrWnd = dhtmlwindow.open('numbergame_box', 'inline', template, title, 'width=' + width + 'px,height=' + height + 'px,left=' + left + 'px,top=150px,resize=0,scrolling=1');

            //------------------------------
            jQuery("#dialog-numbergame").html(template).dialog({
                width: width,
                height: height,
                autoOpen: false,
                modal: true,
                title: title,
                resizable: false
            });
            jQuery("#dialog-numbergame").dialog("open")
            //------------------------------

            jQuery("div.drag-contentarea .message").hide();
        }
        else {
            alert(this.message.errorMatch);
        }
        return false;
    },
    isCheckSend: function (chkElm) {
        var isSend = null, isChecked;
        if (typeof chkElm == 'undefined') {
            var chkElm = jQuery("input#chkissend" + this.curMatchId);
            isChecked = jQuery(chkElm).attr("checked");
            isSend = !isChecked;
            jQuery(chkElm).attr("checked", isSend);
        }
        else {
            isSend = jQuery(chkElm).attr("checked");
        }
        if (isSend) {
            jQuery("div.message." + this.curMatchId).show();
            jQuery("div.drag-contentarea").css({ height: '300px' });

        } else {
            jQuery("div.message." + this.curMatchId).hide();
            jQuery("textarea." + this.curMatchId).val('');
            jQuery("div.drag-contentarea").css({ height: '100px' });
        }
        return false;
    },
    refundSubmit: function (matchId, type) {
        var validSend = this.validSend();
        if (validSend > 0) {
            type = (type == 'ft') ? 'refundft' : 'refund';
            var url = 'CalculationNumberGame.ashx';
            var params = { action: type, matchid: matchId, isMsg: 0 };
            if (validSend == 2) {
                params = jQuery.extend(params, this.getMessage());
            }
            try { parent.BlockUI(); } catch (e) { }
            jQuery.post(url, params, function (resp) {
                refundNumberGameResult(resp);
            });
        } else {
            alert(this.message.errorMessage);
        }
    },
    validSend: function () {
        var validSend = 0, isSend = jQuery("input#chkissend" + this.curMatchId).attr("checked");
        if (isSend) {
            var elmMessage = jQuery("textarea.ereacontent." + this.curMatchId);
            for (var i = 0, len = elmMessage.length; i < len; i++) {
                var areaText = jQuery(elmMessage[i]).val();
                if (jQuery.trim(areaText) != '') {
                    validSend = 2; break;
                }
            }
        } else {
            validSend = 1; // NOT MESSAGE
        }
        return validSend;
    },
    getMessage: function () {
        var message = { isMsg: 1, msgen: '', msgch: '', msgcs: '' };
        var elmMessage = jQuery("textarea.ereacontent." + this.curMatchId);
        for (var i = 0, len = elmMessage.length; i < len; i++) {
            var areaText = jQuery(elmMessage[i]).val();
            if (jQuery.trim(areaText) != '') {
                switch (i) {
                    case 0:
                        message.msgen = jQuery.trim(areaText);
                        break;
                    case 1:
                        message.msgch = jQuery.trim(areaText);
                        break;
                    case 2:
                        message.msgcs = jQuery.trim(areaText);
                        break;

                    default:
                        break;
                }
            }
        }
        return message;
    },
    closeNumbergamePopup: function () {
        jQuery("#dialog-numbergame").dialog("close")
    }
};

function showSettlementPopup(url, title, width, height) {
    OpenPopup(url, title, width, height, false, false, window, 1000, null, null);
}


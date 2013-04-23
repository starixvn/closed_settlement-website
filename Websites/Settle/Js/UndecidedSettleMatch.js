

var undecided = {
    userId: 0,
    SetUnDecidedSettleMatch: function (matchId, matchCode, isUndecided) {

        var msg = '';

        if (isUndecided == 0) {
            msg = 'Are you sure you want to mark this match ' + matchCode + ' as UNDECIDED?';
        }
        else {
            msg = 'Are you sure you want to unmark this match ' + matchCode + ' as UNDECIDED?';
        }

        if (confirm(msg)) {

            try { PTBlockUI(); } catch (e) { }

            var param = '{"matchId":' + matchId + ',"isUndecided":' + (isUndecided == 0 ? 1 : 0) + ',"userId":' + undecided.userId + '}';

            jQuery.ajax({
                type: "POST",
                url: "Service/SettlementService.svc/SetMatchIsUndecided",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                data: param,
                success: function (rs) {
                    GetSettleMatch();
                    try { PTUnBlockUI(); } catch (e) { }
                }
            });
        }
    }
};

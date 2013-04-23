var _matchid = 0;
var _matchcode = 0;
function RenderMatches(matchid, matchcode, sysPath) {
    var contentwraper = $("#divMatches");
    var params = "matchid=" + matchid;
    _matchid = matchid;
    _matchcode = matchcode;
    $.ajax({
        type: "POST",
        url: "MoveBet/BetdaqGetMatches.ashx",
        dataType: "text",
        data: params,
        success: function (data) {
            data = $.evalJSON(data);
            var Error = $.evalJSON(data.Error);
            if (Error.ErrorCode != 0) {
                contentwraper.html(Error.ErrorMessage);
            }
            else {
                var Matches = $.evalJSON(data.Matches)

                contentwraper.setTemplateURL("../" + sysPath + "/Settle/SelectMatchMoveBet.htm");
                contentwraper.processTemplate(Matches);
            }
        }
    });
}

function MoveBet() {

    

    var contentwraper = $("#divMatches");
    var newMatchid = $("#frmMatch input:radio:checked").val();
    var newMatchCode = $("#frmMatch input:radio:checked").attr('matchcode');

    if (!newMatchid) {
        alert('Please choose one new match code!');
        return;
    }
    
    
    var params = "matchid=" + _matchid + "&newmatchid=" + newMatchid;
    params += "&matchcode=" + _matchcode + "&newmatchcode=" + newMatchCode;
    $.ajax({
        type: "POST",
        url: "MoveBet/BetdaqMoveTicket.ashx",
        dataType: "text",
        data: params,
        success: function (data) {
            var data = $.evalJSON(data);
            var result = $.evalJSON(data.Result);

            contentwraper.html(result.ErrorMessage);
            if (result.ErrorCode == 0) {
                setTimeout("CloseWnd()", 3000);
            }
        }
    });
    

}

function CloseWnd() {
    if (typeof (parent.ibcWnd) != 'undefined')
        parent.ibcWnd.close();
    if (typeof (parent.DoCal) != 'undefined')
        parent.DoCal('r', _matchid + '|1');

}

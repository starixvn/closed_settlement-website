/*--End global variable declaretion--*/
$(document).ready(function () {
    resultbetlist.getNumberGameResult();
});


var resultbetlist = {
    matchId: "",
    title: "",
    getNumberGameResult: function () {
        resultbetlist.matchId = matchId;
        if (matchId != null) {
            var param = { matchId: resultbetlist.matchId };
            try { parent.BlockUI(); } catch (e) { }
            $.ajax({
                type: "POST",
                url: "../Report/Service/LiveCasinoReportService.svc/GetBetlistResultLiveCasino",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                data: JSON.stringify(param),
                success: function (rs) {
                    resultbetlist.title = bettype + " " + rs[0].showtime + " - Result";
                    $("#gridtitle").html(resultbetlist.title);
                    var arr = [];
                    if (rs != null && rs.length > 0) {
                        arr.push(resultbetlist.renderLeagueRow(rs[0]));
                        var numberitems = rs.length;
                        for (var i = 0; i < numberitems; i++) {
                            arr.push(resultbetlist.renderMainRow(rs[i]));
                        }
                        
                        $("#tblLiveCasino").empty().html(arr.join(''));

                    }
                    else {
                        $("#tblLiveCasino").empty().html('<tr><td colspan="10"><div class="c" style="width:715px">There is no information available.</div></td></tr>');
                        $("#tblFooter").empty();
                    }

                    $("#LiveCasinoDetails").removeClass("hidden").reRenderScroller(715, 342, true);

                },
                complete: function () {
                    try { parent.jQuery.unblockUI(); } catch (e) { }
                }
            });
        }
    },
    renderLeagueRow: function (row) {
        var finalscore = "";
        var html = "";
        if (bettype == "Baccarat")
            finalscore = "P - B";
        html = '<tr>\
                <td colspan="5" style="background-color:#B4B4B4"><span style="margin-left:143px; font-weight:bold">' + league + '</span><span style="margin-left:127px; font-weight:bold;">' + finalscore + '</span></div></td>\
            </tr>';
        return html;
    },
    renderMainRow: function (row) {
        var html = '';
        var gametype = bettype + ' ' + row.GameType;
        var FinalScore = row.FinalHomeScore + ' - ' + row.FinalAwayScore;
        html = '<tr>\
                <td><div class="col140 c ">' + row.KickoffTime + '</div></td>\
                <td><div class="col200 l ">' + row.GameType + '</div></td>\
                <td><div class="col100 c">' + FinalScore + '</div></td>\
                <td><div class="col180 c">' + row.Results + '</div></td>\
                <td><div class="col80 c">' + row.Status + '</div></td>\
            </tr>';
        return html;
    }
}



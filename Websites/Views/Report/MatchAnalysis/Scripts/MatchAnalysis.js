

var MatchAnalysisManager = {
    matchLeagueList: null,
    eventDate: null,
    leagueIndex: -1,
    matchIndex: -1,
    previousMatchId: -1,
    showMatchSelection: function () {
        $("#filterByPopup").hide();
        filterManagement.isFilterDropdownOpen = false;
        $("#betTypesPopup").hide();
        filterManagement.isBetTypesDropdownOpen = false;
        $("#mchReportDetailsDiv").hide();
        MatchAnalysisManager.initMatchSelection();
    },
    showMatchReportDetails: function (eventDate, leagueIndex, matchIndex) {
        $("#filterByPopup").hide();
        filterManagement.isFilterDropdownOpen = false;
        $("#betTypesPopup").hide();
        filterManagement.isBetTypesDropdownOpen = false;
        MatchAnalysisManager.initMatchMatchReportDetails(eventDate, leagueIndex, matchIndex);
    },
    backMatchSelection: function (eventDate, leagueIndex, matchIndex) {
        if (!isAllowRead) {
            return;
        }
        try { parent.BlockUI(); } catch (e) { }
        $("#matchAnalysis").addClass('hidden');
        $("#mchReportDetailsDiv").hide();
        $("#matchSelectionDiv").show();
        try { parent.jQuery.unblockUI(); } catch (e) { }
    },
    initMatchSelection: function (matchReportDetails) {
        try { parent.BlockUI(); } catch (e) { }
        MatchSelectionManager.setupCalendar('txtfromdate', today);
        $("#txtfromdate").datepicker('hide');
        MatchSelectionManager.getLeagueList();
    },
    initMatchMatchReportDetails: function (eventDate, leagueIndex, matchIndex) {
        try { parent.BlockUI(); } catch (e) { }
        MatchAnalysisManager.eventDate = eventDate;
        MatchAnalysisManager.leagueIndex = leagueIndex;
        MatchAnalysisManager.matchIndex = matchIndex;
        var matchDetail = MatchAnalysisManager.matchLeagueList[leagueIndex].Matches[matchIndex];
        //        $("#matchReportDetailsHeader").html("[" + eventDate + "] "
        //                + MatchAnalysisManager.matchLeagueList[leagueIndex].Name + ": "
        //                + MatchAnalysisManager.getMatchDetail(matchDetail.HomeTeamName, " vs ", matchDetail.AwayTeamName,
        //                   matchDetail.SportTypeId, matchDetail.ShowTime, matchDetail.Name));

        $("#hEventDate").html(eventDate);
        $("#hLeague").html(MatchAnalysisManager.matchLeagueList[leagueIndex].Name);
        $("#hMatch").html(MatchAnalysisManager.getMatchDetail(matchDetail.HomeTeamName, '<font color="#428AA0">'
                         + " vs " + '</font>', matchDetail.AwayTeamName,
                           matchDetail.SportTypeId, matchDetail.ShowTime, matchDetail.Name));
        filterManagement.initFilterPopup();
        filterManagement.initFilterDropdown();
        filterManagement.initBetTypesDropdown();
    },
    removeSpaceIE9: function (html) {
        var expr = new RegExp('>[ \t\r\n\v\f]*<', 'g');
        html = html.replace(expr, '><');
        return html;
    },

    getMatchDetail: function (homeTeamName, vsDisplay, awayTeamName, sportType, showTime, matchCode) {
        var matchDetail = "";
        switch (sportType) {
            case numberGameSportId:
                matchDetail = numberGamePrefix + " " + matchCode;
                break;
            case liveCasinoSportId:
                matchDetail = liveCasinoPrefix + " " + showTime;
                break;
            default:
                if (homeTeamName == "" && awayTeamName == "") {
                    matchDetail = "";
                }
                else if (homeTeamName == "") {
                    matchDetail = awayTeamName;
                }
                else if (awayTeamName == "") {
                    matchDetail = homeTeamName;
                }
                else {
                    matchDetail = homeTeamName + vsDisplay + awayTeamName;
                }
                break;
        }
        return matchDetail;
    }
}

var MatchReportManager = {
    isAllowViewReport: false,
    height: 450,
    width: 927,
    width1: 844,
    isMemberReportType: false,
    params: {
        reportType: 0,
        matchId: '',
        status: '',
        liveStatus: '',
        betTypes: ''
    },
    viewMatchReport: function () {
        if (!isAllowRead) {
            return;
        }
        var betTypes = filterManagement.getSelectedBetType();
        MatchReportManager.allowViewReport();
        if (betTypes == "null" || betTypes == "") {
            showAlertMessage("InformationMssg_MatchRprt", "There is no data.", 1);
            $("#InformationMssg_MatchRprt").show();
            $("#matchAnalysis").addClass('hidden');
        }
        else {
            $("#InformationMssg_MatchRprt").hide();
            $("#InformationMssg_MatchRprt").empty();
            MatchReportManager.getData();
        }
    },
    exportToExcel: function () {
        if (!isAllowRead) {
            return;
        }
        MatchReportManager.setParams();
        var url = 'MatchSelection?IsExport=1&EventDate=' + MatchAnalysisManager.eventDate
                            + '&MatchId=' + MatchReportManager.params.matchId
                            + '&MatchCode=' + MatchReportManager.params.matchCode
                            + '&Liveindicator=' + MatchReportManager.params.liveStatus
                            + '&Bettypes=' + MatchReportManager.params.betTypes
                            + '&MoveBetsDay=' + moveBetDay
                            + '&BalanceUpDay=' + balanceUpDay
                            + '&StatusId=' + MatchReportManager.params.statusId
                            + '&ReportType=' + MatchReportManager.params.reportType
                            + '&LeagueId=' + MatchReportManager.params.LeagueId
                            + '&HomeId=' + MatchReportManager.params.HomeId
                            + '&AwayId=' + MatchReportManager.params.AwayId;
        if (MatchReportManager.params.SportTypeId == numberGameSportId
                || MatchReportManager.params.SportTypeId == liveCasinoSportId) {
            url = url + '&SportTypeId=' + MatchReportManager.params.SportTypeId
                      + '&ShowTime=' + MatchReportManager.params.ShowTime;
        }
        $('#frm').attr('src', url);
    },
    allowViewReport: function () {
        if (filterManagement.getSelectedBetType() != "") {
            $("#btnViewReport").removeAttr("disabled");
            MatchReportManager.isAllowViewReport = true;
        }
        else {
            $("#btnViewReport").attr("disabled", "disabled");
            MatchReportManager.isAllowViewReport = false;
        }
    },
    setParams: function () {
        var matchDetail = MatchAnalysisManager.matchLeagueList[MatchAnalysisManager.leagueIndex].Matches[MatchAnalysisManager.matchIndex];
        MatchReportManager.params.matchId = matchDetail.Id;
        MatchReportManager.params.matchCode = matchDetail.Name;
        MatchReportManager.params.reportType = filterManagement.getSelectedReportBy();
        MatchReportManager.params.statusId = filterManagement.getSelectedStatusBy();
        MatchReportManager.params.liveStatus = filterManagement.getSelectedLiveStatusBy();
        MatchReportManager.params.betTypes = filterManagement.getSelectedBetType();
        MatchReportManager.params.LeagueId = MatchAnalysisManager.matchLeagueList[MatchAnalysisManager.leagueIndex].Id;
        MatchReportManager.params.HomeId = matchDetail.HomeId;
        MatchReportManager.params.AwayId = matchDetail.AwayId;
        MatchReportManager.params.SportTypeId = matchDetail.SportTypeId;
        MatchReportManager.params.ShowTime = matchDetail.ShowTime;

    },
    initGrid: function () {
        MatchReportManager.me = $("#matchAnalysis");
        MatchReportManager.th = $("#tblHeader");
        MatchReportManager.tb = $("#tblContent tbody");
        MatchReportManager.tf = $("#tblFooter");
    },
    getData: function () {
        MatchReportManager.setParams();
        MatchReportManager.initGrid();
        ///Ajax here
        if (MatchReportManager.params.matchId != '') {
            MatchReportManager.setTitle();
            var data = {
                param: {
                    EventDate: MatchAnalysisManager.eventDate,
                    MoveBetsDay: moveBetDay,
                    BalanceUpDay: balanceUpDay,
                    MatchId: MatchReportManager.params.matchId,
                    StatusId: MatchReportManager.params.statusId,
                    Liveindicator: MatchReportManager.params.liveStatus,
                    ReportType: MatchReportManager.params.reportType,
                    Bettypes: MatchReportManager.params.betTypes
                }
            };
            try { parent.BlockUI(); } catch (e) { };
            $("#matchAnalysis").addClass('hidden');
            $.ajax({
                type: "POST",
                url: window.rootUrl + "MatchAnalysis/SelMatchAnalysisReport",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                data: JSON.stringify(data),
                success: function (rs) {
                    if (MatchReportManager.params.reportType != '3') {
                        MatchReportManager.isMemberReportType = false;
                        $("#colCurrency").addClass('hidden');
                    }
                    else {
                        $("#colCurrency").removeClass('hidden');
                        MatchReportManager.isMemberReportType = true;
                    }
                    MatchReportManager.renderData(rs);
                    try { parent.jQuery.unblockUI(); } catch (e) { }
                },
                error: function () {
                },
                complete: function () {
                    $("#matchAnalysis").removeClass('hidden');
                    try { parent.jQuery.unblockUI(); } catch (e) { }
                }
            });

        }
    },
    setTitle: function () {
        var reportType = filterManagement.getSelectedReportBy();
        var title = '';
        switch (reportType) {
            case '0':
                title = 'Currency';
                break;
            case '1':
                title = 'Bet type';
                break;
            case '2':
                title = 'Super';
                break;
            case '3':
                switch (filterManagement.getSelectedStatusBy()) {
                    case '112':
                        title = 'Top 100 (WON) Member';
                        break;
                    case '113':
                        title = 'Top 100 (LOSE) Member';
                        break;
                    default:
                        title = 'Report';
                        break;
                }
                break;
            default:
                title = 'Report';
                break;
        }
        $('#ttlreportType').html(title);
    },
    renderData: function (_data) {
        var len = _data.Items.length;
        var arr = [];
        if (len > 0) {
            for (var i = 0; i < len; i++) {
                arr.push(MatchReportManager.rendRow(_data.Items[i]));
            }
            $("#reportFooter").show();
            MatchReportManager.tb.html(arr.join(''));
            MatchReportManager.tf.html(MatchReportManager.rendRow(_data.Total));
            MatchReportManager.createScroll();
        }
        else {
            $("#reportFooter").hide();
            MatchReportManager.renderNoData();
        }
    },
    renderNoData: function () {
        MatchReportManager.tb.html('<tr><td><div style="width:' + (!MatchReportManager.isMemberReportType ? (MatchReportManager.width1 - 3) : (MatchReportManager.width - 3)) + 'px;" colspan=' + (MatchReportManager.toalColumn - 1) + ' class="b c">There is no information available.</div></td></tr>');
        MatchReportManager.me.removeClass('hidden').reRenderScroller(!MatchReportManager.isMemberReportType ? MatchReportManager.width1 : MatchReportManager.width, MatchReportManager.height, true);
        MatchReportManager.tf.addClass("hidden");
    },
    createScroll: function () {
        MatchReportManager.tf.removeClass("hidden");
        MatchReportManager.me.removeClass('hidden').reRenderScroller(!MatchReportManager.isMemberReportType ? MatchReportManager.width1 : MatchReportManager.width, MatchReportManager.height, true);
    },
    rendRow: function (row) {
        var betCount = row.BetCount;
        if (betCount.indexOf(".00") != -1) {
            betCount = betCount.replace(".00", "");
        }
        var html = '<tr>\
            <td>\
                <div class="col100 c">' + row.ItemName + '</div>\
            </td>\
            <td>\
                <div class="col120 r">' + row.MemberTO + '</div>\
            </td>\
            <td>\
                <div class="col100 r">' + row.MemberWL + '</div>\
            </td>\
            <td>\
                <div class="col100 r">' + betCount + '</div>\
            </td>\
            <td>\
                <div class="col100 r">' + row.HouseTO + '</div>\
            </td>\
            <td>\
                <div class="col100 r ' + (parseFloat(ReplaceAll(row.HouseWL, ',', '')) < 0 ? "red" : "") + '">' + row.HouseWL + '</div>\
            </td>\
            <td>\
                <div class="col100 r">' + row.SMATO + '</div>\
            </td>\
            <td>\
                <div class="col100 r">' + row.SMAWL + '</div>\
            </td>\
            <td ' + (!MatchReportManager.isMemberReportType ? 'class="hidden"' : '') + '>\
                <div class="col80 c">' + row.Currency + '</div>\
            </td>\
        </tr>';
        html = MatchAnalysisManager.removeSpaceIE9(html);
        return html;
    }
};

var filterManagement = {
    filterDropdownIsShow: false,
    betTypeDropdownIsShow: false,
    reportBy: "Currency",
    reportByDiv: null,
    statusBy: "All",
    statusByDiv: null,
    liveStatusBy: "All",
    liveStatusByDiv: null,
    isAllStatus: false,
    isWinStatus: false,
    isLoseStatus: false,
    isDrawStatus: false,
    isFirstLoad: true,
    isFilterDropdownOpen: false,
    isBetTypesDropdownOpen: false,
    initFilterDropdown: function () {
        if (filterManagement.reportByDiv != undefined) {
            filterManagement.reportByDiv.removeClass("divCheckbox-select");
        }
        if (filterManagement.liveStatusByDiv != undefined) {
            filterManagement.liveStatusByDiv.removeClass("divCheckbox-select");
        }

        filterManagement.reportByDiv = $("#Currency");
        filterManagement.reportBy = "Currency";
        filterManagement.showWinDrawStatus();
        filterManagement.liveStatusByDiv = $("#LiveStatusAll");
        filterManagement.liveStatusBy = "LiveStatusAll";

        filterManagement.reportByDiv.addClass("divCheckbox-select");
        filterManagement.liveStatusByDiv.addClass("divCheckbox-select");

        filterManagement.allStatusCheck();
        if (filterManagement.isFirstLoad) {
            $("#ddlFilterBy").click(filterManagement.clickFilterDropdown);
            $("#ddlFilterBy").focusout(filterManagement.hideFilterDropdown);
        }
    },
    clickFilterDropdown: function () {
        if (filterManagement.isFilterDropdownOpen == false) {
            $("#betTypesPopup").hide();
            filterManagement.isBetTypesDropdownOpen = false;
            $("#filterByPopup").show();
            filterManagement.isFilterDropdownOpen = true;
        }
        else {
            $("#filterByPopup").hide();
            filterManagement.isFilterDropdownOpen = false;
        }
    },
    hideFilterDropdown: function () {
        filterManagement.isFilterDropdownOpen = false;
    },
    initBetTypesDropdown: function () {
        $("#InformationMssg_MatchRprt").empty();
        var data = {
            param: {
                EventDate: MatchAnalysisManager.eventDate,
                MoveBetsDay: moveBetDay,
                BalanceUpDay: balanceUpDay,
                MatchId: MatchAnalysisManager.matchLeagueList[MatchAnalysisManager.leagueIndex].Matches[MatchAnalysisManager.matchIndex].Id
            }
        };
        $.ajax({
            type: "POST",
            url: window.rootUrl + "MatchAnalysis/SelBettypeByMatchId",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: JSON.stringify(data),
            success: function (rs) {
                if (rs && rs.length > 0) {
                    var arr = [];
                    var orderIndex = 1;
                    var tempstring = "";
                    for (var i = 0; i < rs.length; i++) {
                        var betType = rs[i];
                        if (i % 2 == 0) {
                            tempstring = '<tr><td style="width: 150px;"><input type="checkbox" checked="checked" class="betTypeCkb" onclick="filterManagement.betTypeCheck(this)" value="' + betType.Id + '" id="BetType' + betType.Id + '" />';
                            tempstring = tempstring + '<label for="BetType' + betType.Id + '">' + betType.Name + '</label>' + '</td>';
                        }
                        else {
                            tempstring = tempstring + '<td style="width: 150px;"><input type="checkbox" checked="checked" class="betTypeCkb" onclick="filterManagement.betTypeCheck(this)" value="' + betType.Id + '" id="BetType' + betType.Id + '" />';
                            tempstring = tempstring + '<label for="BetType' + betType.Id + '">' + betType.Name + '</label>' + '</td></tr>';
                            arr.push(tempstring);
                            tempstring = "";
                        }
                    }
                    if (rs.length % 2 == 1) {
                        tempstring = MatchAnalysisManager.removeSpaceIE9(tempstring);
                        arr.push(tempstring);
                    }
                    $('#tblBetTypeDetail').empty().append(arr.join(''));
                    $("#cbAllBetType").attr('checked', 'checked');
                    $("#trBettype").show();
                    $("#trNoBettype").hide();
                    $("#InformationMssg_MatchRprt").hide();
                    $("#InformationMssg_MatchRprt").empty();
                }
                else {
                    $("#trBettype").hide();
                    $("#trNoBettype").show();
                    $('#tblBetTypeDetail').empty();
                    showAlertMessage("InformationMssg_MatchRprt", "No bettype found.", 1);
                    $("#InformationMssg_MatchRprt").show();
                }
            },
            error: function () {
                showAlertMessage("InformationMssg_MatchRprt", "There is no data.", 1);
                $("#InformationMssg_MatchRprt").show();
            },
            complete: function () {
                if (filterManagement.isFirstLoad) {
                    $("#ddlBetType").click(filterManagement.clickBetTypeDropdown);
                    $("#ddlBetType").focusout(filterManagement.hideBetTypesDropdown);
                    filterManagement.isFirstLoad = false;
                }
                $("#cbAllBetType").click(filterManagement.checkAllBetType);
                $("#cbAllBetType").click(filterManagement.selectBetType);
                $("#matchSelectionDiv").hide();
                MatchReportManager.allowViewReport();
                $("#mchReportDetailsDiv").show();
                try { parent.jQuery.unblockUI(); } catch (e) { }
            }
        });
    },
    clickBetTypeDropdown: function () {
        if (filterManagement.isBetTypesDropdownOpen == false) {
            $("#filterByPopup").hide();
            filterManagement.isFilterDropdownOpen = false;
            $("#betTypesPopup").show();
            filterManagement.isBetTypesDropdownOpen = true;
        }
        else {
            $("#betTypesPopup").hide();
            filterManagement.isBetTypesDropdownOpen = false;
        }
    },
    hideBetTypesDropdown: function () {
        filterManagement.isBetTypesDropdownOpen = false;
    },
    changReportBySelection: function (selectDiv) {
        if (filterManagement.reportByDiv != undefined) {
            filterManagement.reportByDiv.removeClass("divCheckbox-select");
        }

        $(selectDiv).addClass("divCheckbox-select");
        filterManagement.reportByDiv = $(selectDiv);
        var lastSelectedReportBy = filterManagement.reportBy;
        filterManagement.reportBy = $(selectDiv).attr("id");
        if (filterManagement.reportBy == "Member") {
            filterManagement.winStatusCheck();
            filterManagement.hideWinDrawStatus();
        } else {
            filterManagement.allStatusCheck();
            filterManagement.showWinDrawStatus();
        }
        filterManagement.liveStatusBy = "LiveStatusAll";
        filterManagement.changLiveStatusBySelection($("#LiveStatusAll"));
    },
    allStatusCheck: function () {
        if (filterManagement.reportBy == "Member") {
            return;
        }
        if (filterManagement.isWinStatus) {
            $("#StatusWin").removeClass("divCheckbox-select");
            filterManagement.isWinStatus = false;
        }
        if (filterManagement.isLoseStatus) {
            $("#StatusLose").removeClass("divCheckbox-select");
            filterManagement.isLoseStatus = false;
        }
        if (filterManagement.isDrawStatus) {
            $("#StatusDraw").removeClass("divCheckbox-select");
            filterManagement.isDrawStatus = false;
        }
        $("#StatusAll").addClass("divCheckbox-select");
        filterManagement.isAllStatus = true;
    },
    winStatusCheck: function () {
        if (filterManagement.isAllStatus) {
            $("#StatusAll").removeClass("divCheckbox-select");
            filterManagement.isAllStatus = false;
        }
        $("#StatusWin").addClass("divCheckbox-select");
        filterManagement.isWinStatus = true;
        if (filterManagement.isLoseStatus) {
            $("#StatusLose").removeClass("divCheckbox-select");
            filterManagement.isLoseStatus = false;
        }
        if (filterManagement.isDrawStatus) {
            $("#StatusDraw").removeClass("divCheckbox-select");
            filterManagement.isDrawStatus = false;
        }
    },
    loseStatusCheck: function () {
        if (filterManagement.isAllStatus) {
            $("#StatusAll").removeClass("divCheckbox-select");
            filterManagement.isAllStatus = false;
        }
        if (filterManagement.isWinStatus) {
            $("#StatusWin").removeClass("divCheckbox-select");
            filterManagement.isWinStatus = false;
        }
        $("#StatusLose").addClass("divCheckbox-select");
        filterManagement.isLoseStatus = true;
        if (filterManagement.isDrawStatus) {
            $("#StatusDraw").removeClass("divCheckbox-select");
            filterManagement.isDrawStatus = false;
        }
    },
    drawStatusCheck: function () {
        if (filterManagement.reportBy == "Member") {
            return;
        }
        if (filterManagement.isAllStatus) {
            $("#StatusAll").removeClass("divCheckbox-select");
            filterManagement.isAllStatus = false;
        }
        if (filterManagement.isWinStatus) {
            $("#StatusWin").removeClass("divCheckbox-select");
            filterManagement.isWinStatus = false;
        }
        if (filterManagement.isLoseStatus) {
            $("#StatusLose").removeClass("divCheckbox-select");
            filterManagement.isLoseStatus = false;
        }

        $("#StatusDraw").addClass("divCheckbox-select");
        filterManagement.isDrawStatus = true;
    },
    changStatusBySelection: function (selectDiv) {
        var selectId = $(selectDiv).attr("id");
        if ((filterManagement.reportBy == "Member" && (selectId == "StatusWin" || selectId == "StatusLose")) ||
            filterManagement.reportBy != "Member") {
            if (filterManagement.statusByDiv != undefined) {
                filterManagement.statusByDiv.removeClass("divCheckbox-select");
            }
            $(selectDiv).addClass("divCheckbox-select");
            filterManagement.statusByDiv = $(selectDiv);
        }
    },
    changLiveStatusBySelection: function (selectDiv) {
        if (filterManagement.liveStatusByDiv != undefined) {
            filterManagement.liveStatusByDiv.removeClass("divCheckbox-select");
        }
        $(selectDiv).addClass("divCheckbox-select");
        filterManagement.liveStatusByDiv = $(selectDiv);
        filterManagement.liveStatusBy = $(selectDiv).attr("id");
    },
    renderBetTypeRow: function (betType, i) {
    },
    checkAllBetType: function () {
        if ($("#cbAllBetType").attr('checked') == 'checked') {
            $(".betTypeCkb").attr('checked', 'checked');
        }
        else {
            $(".betTypeCkb").removeAttr('checked');
        }
    },
    betTypeCheck: function (checkbox) {
        var betTypeCkbList = $(".betTypeCkb");
        var numberofSelected = 0;
        for (var i = 1; i < betTypeCkbList.length; i++) {
            if ($(betTypeCkbList[i]).attr('checked') == 'checked') {
                numberofSelected++;
            }
        }
        if (numberofSelected + 1 == betTypeCkbList.length) {
            $("#cbAllBetType").attr('checked', 'checked');
        }
        else {
            $("#cbAllBetType").removeAttr('checked');
        }
    },
    getSelectedReportBy: function () {
        var reportByParam = "";
        switch (filterManagement.reportBy) {
            case "Currency":
                reportByParam = "0";
                break;
            case "BetType":
                reportByParam = "1";
                break;
            case "Super":
                reportByParam = "2";
                break;
            case "Member":
                reportByParam = "3";
                break;
            case "Country":
                reportByParam = "4";
                break;
        }
        return reportByParam;
    },
    getSelectedStatusBy: function () {
        var selectedStatusByParam = "";
        if (filterManagement.isAllStatus) {
            selectedStatusByParam = "0";
            return;
        }
        if (filterManagement.isWinStatus) {
            selectedStatusByParam = "112";
        }
        if (filterManagement.isLoseStatus) {
            selectedStatusByParam = "113";
        }
        if (filterManagement.isDrawStatus) {
            selectedStatusByParam = "111";
        }
        return selectedStatusByParam;
    },
    getSelectedLiveStatusBy: function () {
        var selectedLiveStatusByParam = "-1";
        switch (filterManagement.liveStatusBy) {
            case "LiveStatusAll":
                selectedLiveStatusByParam = "-1";
                break;
            case "LiveStatusLive":
                selectedLiveStatusByParam = "1";
                break;
            case "LiveStatusNoneLive":
                selectedLiveStatusByParam = "0";
                break;
        }
        return selectedLiveStatusByParam;
    },
    getSelectedBetType: function () {
        var betTypeCkbList = $(".betTypeCkb");
        var prefixLength = "BetType".length;
        var result = "";
        for (var i = 1; i < betTypeCkbList.length; i++) {
            if ($(betTypeCkbList[i]).attr('checked') == 'checked') {
                result = result + $(betTypeCkbList[i]).attr('id').substring(prefixLength) + ",";
            }
        }
        result = result.replace(/,*$/, "");
        if (result == "") {
            result = "null";
        }
        return result;
    },
    hideWinDrawStatus: function () {
        $("#StatusAll").hide();
        $("#StatusDraw").hide();
    },
    showWinDrawStatus: function () {
        $("#StatusAll").show();
        $("#StatusDraw").show();
    },
    initFilterPopup: function () {
        var offsetTop = 106;
        var textLength = $("#hLeague").html().length;
        var maxTextLength = 63;
        var additionTextHeight = 6;
        var topAddition = ((textLength - textLength % maxTextLength) / maxTextLength) * additionTextHeight;
        if (topAddition != 0) {
            offsetTop = offsetTop + topAddition;
            $("#filterByPopup").css({ position: 'absolute',
                top: offsetTop + 'px'
            });
            $("#betTypesPopup").css({ position: 'absolute',
                top: offsetTop + 'px'
            });
            return;
        }

        textLength = $("#hMatch").html().length;
        maxTextLength = 80;
        topAddition = ((textLength - textLength % maxTextLength) / maxTextLength) * additionTextHeight;
        if (topAddition != 0) {
            offsetTop = offsetTop + topAddition;
            $("#filterByPopup").css({ position: 'absolute',
                top: offsetTop + 'px'
            });
            $("#betTypesPopup").css({ position: 'absolute',
                top: offsetTop + 'px'
            });
            return;
        }

        $("#filterByPopup").css({ position: 'absolute',
            top: offsetTop + 'px'
        });
        $("#betTypesPopup").css({ position: 'absolute',
            top: offsetTop + 'px'
        });
    }
};







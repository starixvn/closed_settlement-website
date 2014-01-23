
var settlementManagement = {
    kickoffdate: null,
    enums: {
        CalcStatusConstants: {
            Completed: 1,
            Waiting: 2,
            Calculating: 3
        },
        SettleType: {
            Calculate: "",
            HTOverOnly: "HTOVER",
            OverOnly: "OVER"
        },
        CalculateType: {
            Billing: 'b',
            Refund: 'r',
            KeyScore: 'k'
        }
    },
    cachedData: {
        selectedMatchInfo: null,
        domElement: {},
        testCounter: 0,
        tables: {
            sContent: null,
            report: null,
            report_Content: null
        },

        inCalculateQueueMatches: []
    },
    cachedTables: function () {
        settlementManagement.cachedData.tables.sContent = jQuery("#sContent");
        settlementManagement.cachedData.tables.report = settlementManagement.cachedData.tables.sContent.find(".settleMatchLog");
        settlementManagement.cachedData.tables.report_Content = settlementManagement.cachedData.tables.report.find("#settleMatchLog_Content");
    },
    cacheDOMElementBeforCalcMP_ByMatchId: function (matchInfo) {
        var checkboxCalcMP = jQuery('input[id^="chk_' + matchInfo + '_"]');
        if (checkboxCalcMP == undefined || checkboxCalcMP.length == 0) {
            checkboxCalcMP = jQuery('.colCalcMP_' + matchInfo);
        }

        if (checkboxCalcMP && checkboxCalcMP.length) {
            var matchId = "match" + matchInfo;
            var colCalculation = checkboxCalcMP.parent().parent().find(".colCalculation").parent();
            var colRefund = checkboxCalcMP.parent().parent().find(".colRefund").parent();
            var colCalcMP = checkboxCalcMP.parent();
            settlementManagement.cachedData.domElement[matchId] = {};
            settlementManagement.cachedData.domElement[matchId].colCalculation = colCalculation.html();
            settlementManagement.cachedData.domElement[matchId].colRefund = colRefund.html();
            settlementManagement.cachedData.domElement[matchId].colCalcMP = colCalcMP.html();
        }
    },
    cacheDOMElementBeforCalcMP: function () {
        var paramsForSelecting = settlementManagement.cachedData.selectedMatchInfo;
        if (paramsForSelecting == null) {
            return;
        }

        var matchCodes = paramsForSelecting.split("|")[0].split(",");
        var infoLength = matchCodes.length;
        for (var i = 0; i < infoLength; i++) {
            var matchInfo = matchCodes[i];
            settlementManagement.cacheDOMElementBeforCalcMP_ByMatchId(matchInfo);
        }
    },
    getCalcMPStatus: function () {
        jQuery.ajax({
            type: "POST",
            url: "../Settlement_CalculateMP/GetCalcMPStatus",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: JSON.stringify(null),
            beforeSend: function () {
            },
            success: function (rs) {
                settlementManagement.informCalcMPStatus(rs);
            },
            error: function (event, jqXHR, ajaxSettings) {
            },
            complete: function () {
            }
        });
    },
    informCalcMPStatus: function (rs) {
        if (rs && rs.ChildInfo) {
            settlementManagement.updateCalculateMatchQueue(rs.ChildInfo, true);
        }

        settlementManagement.updateDomAfterGetStatus();
    },
    updateDomAfterGetStatus: function () {
        var calcStatusConstants = settlementManagement.enums.CalcStatusConstants;
        var lastQueueLength = settlementManagement.cachedData.inCalculateQueueMatches.length;
        var updatedQueue = [];
        var completedMatchs = [];
        for (var i = 0; i < lastQueueLength; i++) {
            var calcInfo = settlementManagement.cachedData.inCalculateQueueMatches[i];
            switch (calcInfo.Status) {
                case calcStatusConstants.Waiting:
                    var match_row = jQuery(".match_row_" + calcInfo.Id);
                    if (match_row && match_row.length) {
                        match_row.find(".colCalculation:not(.colCalculation_Disabled)").addClass("hidden");
                        match_row.find(".colRefund:not(.colRefund_Disabled)").addClass("hidden");
                        match_row.find(".calmultiple:not(.calmultiple_Disabled)").addClass("hidden");
                        match_row.find(".colCalculation_Disabled").removeClass("hidden");
                        match_row.find(".colRefund_Disabled").removeClass("hidden");
                        match_row.find(".calmultiple_Disabled").html(calcInfo.StatusName)
                                                                   .removeClass("calcStatus_Calculating")
                                                                   .addClass("calcStatus_Waiting")
                                                                   .removeClass("hidden");

                        var calmultipleParent = match_row.find(".calmultiple").parent();
                        //                        if (calcInfo.PreClass == undefined) {
                        //                            calcInfo.PreClass = calmultipleParent.attr("class");
                        //                        }
                        calmultipleParent.removeClass("bgRef").removeClass("bgBill");
                    }

                    updatedQueue.push(calcInfo);
                    break;
                case calcStatusConstants.Calculating:
                    var match_row = jQuery(".match_row_" + calcInfo.Id);
                    if (match_row && match_row.length) {
                        match_row.find(".colCalculation:not(.colCalculation_Disabled)").addClass("hidden");
                        match_row.find(".colRefund:not(.colRefund_Disabled)").addClass("hidden");
                        match_row.find(".calmultiple:not(.calmultiple_Disabled)").addClass("hidden");
                        match_row.find(".colCalculation_Disabled").removeClass("hidden");
                        match_row.find(".colRefund_Disabled").removeClass("hidden");
                        match_row.find(".calmultiple_Disabled").html(calcInfo.StatusName)
                                                                   .removeClass("calcStatus_Waiting")
                                                                   .addClass("calcStatus_Calculating")
                                                                   .removeClass("hidden");

                        var calmultipleParent = match_row.find(".calmultiple").parent();
                        calmultipleParent.removeClass("bgRef").removeClass("bgBill");
                        //                        if (calcInfo.PreClass != undefined) {
                        //                            calmultipleParent.removeClass("bgRef").removeClass("bgBill").addClass(calcInfo.PreClass);
                        //                        }
                    }

                    updatedQueue.push(calcInfo);
                    break;
                case calcStatusConstants.Completed:
                    completedMatchs.push(calcInfo.Id);
                    break;
            }
        }

        settlementManagement.updateCompletedMatches(completedMatchs);
        settlementManagement.cachedData.inCalculateQueueMatches = updatedQueue;
    },
    updateCompletedMatches: function (completedMatchs) {
        if (completedMatchs == null || completedMatchs.length == 0) {
            return;
        }

        jQuery.ajax({
            type: "POST",
            url: "../Settlement_CalculateMP/GetCompletedCalcMPStatus",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: JSON.stringify({
                MatchCodes: completedMatchs.join(",")
            }),
            beforeSend: function () {
            },
            success: function (rs) {
                if (rs && rs.ChildInfo.length) {
                    var resultLength = rs.ChildInfo.length;
                    for (var i = 0; i < resultLength; i++) {
                        var calcInfo = rs.ChildInfo[i];
                        var match_row = jQuery(".match_row_" + calcInfo.Id);
                        if (match_row && match_row.length) {
                            match_row.find(".colCalculation_Disabled").addClass("hidden");
                            match_row.find(".colRefund_Disabled").addClass("hidden");
                            match_row.find(".calmultiple_Disabled").html('').addClass("hidden");
                            var calmultipleCheckBox = match_row.find(".calmultiple:not(.calmultiple_Disabled)");
                            var subCheckAll = calmultipleCheckBox.attr("id").split("_")[3];
                            calmultipleCheckBox.attr("checked", false).removeClass("hidden");
                            jQuery("#chk_all").attr("checked", false);
                            jQuery("#chk_" + subCheckAll).attr("checked", false);

                            var calmultipleParent = match_row.find(".calmultiple").parent();
                            if (calcInfo.IsMultipleCalculated) {
                                calmultipleParent.removeClass("bgRef").addClass("bgBill");
                            } else if (calcInfo.IsRefunded) {
                                calmultipleParent.removeClass("bgBill").addClass("bgRef");
                            }

                            match_row.find(".colCalculation:not(.colCalculation_Disabled)").removeClass("hidden");
                            match_row.find(".colRefund:not(.colRefund_Disabled)").removeClass("hidden");
                        }
                    }
                }
            },
            error: function (event, jqXHR, ajaxSettings) {
            },
            complete: function () {
            }
        });
    },
    isInCalcMPQueue: function (matchId) {
        var queueLength = settlementManagement.cachedData.inCalculateQueueMatches.length;
        var isExist = false;
        for (var i = 0; i < queueLength; i++) {
            if (matchId == settlementManagement.cachedData.inCalculateQueueMatches[i].Id) {
                isExist = true;
                break;
            }
        }

        return isExist;
    },
    updateCalculateMatchQueue: function (matchQueue, isDeleteComplete) {
        var queueLength = matchQueue.length;
        var isExist = false;
        var lastQueueLength = settlementManagement.cachedData.inCalculateQueueMatches.length;
        for (var i = 0; i < queueLength; i++) {
            isExist = false;
            for (var j = 0; j < lastQueueLength; j++) {
                if (matchQueue[i].Id == settlementManagement.cachedData.inCalculateQueueMatches[j].Id) {
                    settlementManagement.cachedData.inCalculateQueueMatches[j] = matchQueue[i];
                    isExist = true;
                    break;
                }
            }

            if (!isExist) {
                settlementManagement.cachedData.inCalculateQueueMatches.push(matchQueue[i]);
                lastQueueLength++;
            }
        }

        if (isDeleteComplete) {
            queueLength = matchQueue.length;
            lastQueueLength = settlementManagement.cachedData.inCalculateQueueMatches.length;
            var calcStatusConstants = settlementManagement.enums.CalcStatusConstants;
            var tempArray = [];
            for (var j = 0; j < lastQueueLength; j++) {
                isExist = false;
                for (var i = 0; i < queueLength; i++) {
                    if (matchQueue[i].Id == settlementManagement.cachedData.inCalculateQueueMatches[j].Id) {
                        lastQueueLength[j] = matchQueue[i];
                        isExist = true;
                        break;
                    }
                }

                if (!isExist) {
                    settlementManagement.cachedData.inCalculateQueueMatches[j].Status = calcStatusConstants.Completed;
                }
            }
        }
    },
    removeMatchRequest_CalcMP: function (matchId) {
        var updatedParams = settlementManagement.cachedData.selectedMatchInfo;
        var matchString = "," + matchId + ",";
        if (updatedParams.indexOf(matchString) != -1) {
            updatedParams = updatedParams.replace(matchString, "");
        }
        else {
            matchString = matchId + ",";
            if (updatedParams.indexOf(matchString) == 0) {
                updatedParams = updatedParams.replace(matchString, "");
            }
            else {
                matchString = "," + matchId + "|";
                if (updatedParams.indexOf(matchString) != -1) {
                    updatedParams = updatedParams.replace(matchString, "|");
                }
            }
        }

        var requestMatchIds = updatedParams.split("|")[0];
        if (requestMatchIds == null || requestMatchIds == "") {
            updatedParams = null;
        }

        return updatedParams;
    },
    doCalcMP: function (type, params) {
        //        settlementManagement.cachedData.selectedMatchInfo = params;

        var matchId = params.split("|")[0];
        var myajax = new Ajax.Request("Multiple.ashx", {
            asynchronous: true,
            method: 'post',
            parameters: 'p=' + params + '&type=' + type,
            onComplete: function (rs) {
                MultipleResult(rs, type, matchId);
            }
        });
    },
    makeFreezeGrid: function () {
        settlementManagement.cachedTables();
        if (settlementManagement.cachedData.tables.report &&
            settlementManagement.cachedData.tables.report.length > 0) {
            var gridWidth = 1250;
            var settleType = jQuery('#m').val();
            var settleTypeEnum = settlementManagement.enums.SettleType;
            switch (settleType) {
                case settleTypeEnum.HTOverOnly:
                    var colBetCount = settlementManagement.cachedData.tables.report.find(".colBetCount");
                    if (colBetCount == null || colBetCount.length == 0) {
                        gridWidth = gridWidth - 45;
                    }

                    gridWidth = gridWidth - (45 + 65); // hide (colCalmultiple + colConfirmRT)
                    break;
                case settleTypeEnum.OverOnly:
                    var colBetCount = settlementManagement.cachedData.tables.report.find(".colBetCount");
                    if (colBetCount == null || colBetCount.length == 0) {
                        gridWidth = gridWidth - 45;
                    }

                    gridWidth = gridWidth - 45; // hide colCalmultiple
                    break;
            }

            var colTitle_NoInfo = settlementManagement.cachedData.tables.report.find(".colTitle_NoInfo");
            if (colTitle_NoInfo && colTitle_NoInfo.length > 0) {
                colTitle_NoInfo.css("width", gridWidth);
            }

            var gridOptions = {
                isShowPager: false,
                tblContentId: 'settleMatchLog_Content',
                manualCopyHeader: true,
                fullHeight: true,
                fullWidth: true,
                gridWidth: gridWidth,
                //            columns: [120, 80, 80, 100, 100, 100, 100, 100, 100, 100, 120],
                gridHeight: 100,
                freezeWidth: 0,
                freezeHeight: 200,
                numberFreezeCol: 0
            };

            settlementManagement.cachedData.tables.report.removeClass("hidden").gridScroller(gridOptions);


            if (isRefresh) {
                var container = settlementManagement.cachedData.tables.report.find(".container");
                container.attr("scrollTop", scrollTop);
            }
        }
    },
    getMatchCalculateStatus: function (calcMPStatus, matchId) {
        var matchCalculateStatus = null;
        if (calcMPStatus && calcMPStatus.ChildInfo.length > 0) {
            var infoLength = calcMPStatus.ChildInfo.length;
            for (var i = 0; i < infoLength; i++) {
                var calcInfo = calcMPStatus.ChildInfo[i];
                if (calcInfo.Id == matchId) {
                    matchCalculateStatus = calcInfo;
                    break;
                }
            }
        }

        return matchCalculateStatus;
    },
    doCal: function (type, params, kickoffdate) {
        this.kickoffdate = !kickoffdate ? this.kickoffdate : kickoffdate;

        var calcStatusConstants = settlementManagement.enums.CalcStatusConstants;
        var matchId = params.split('|')[1];
        jQuery.ajax({
            type: "POST",
            url: "../Settlement_CalculateMP/GetCalcMPStatus",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: JSON.stringify(null),
            beforeSend: function () {
            },
            success: function (rs) {
                var matchCalculateStatus = settlementManagement.getMatchCalculateStatus(rs, matchId);
                if (matchCalculateStatus != null &&
                    matchCalculateStatus.Status != calcStatusConstants.Completed) {
                    alert("This match is in calculate queue (status {0})".replace(/\{0\}/g,
                             matchCalculateStatus.StatusName));
                } else {
                    settlementManagement.showSettlementPopup(type, params, matchId, settlementManagement.kickoffdate);
                }
            },
            error: function (event, jqXHR, ajaxSettings) {
            },
            complete: function () {
            }
        });
    },
    showSettlementPopup: function (type, params, sportId, kickoffdate) {
        kickoffdate = kickoffdate || new Date().toGMTString();
        var height = 330;
        var width = 620;
        switch (sportId) {
            case 1:
                height = 330;
                break;
            case 8:
                width = 750;
                break;
        }

        if (type != 'b') {
            height = 450;
        }

        var isIE7 = navigator.appVersion.indexOf('MSIE 7.0') > 0;
        if (isIE7) {
            height = height + 50;
        }

        var url = 'Calculation.aspx?p=' + params + '&type=' + type + '&kickoffdate=' + jQuery('<div/>').text(kickoffdate.replace('<br>', ' ').replace('<br />', ' ').replace('<br >', ' ').replace('</span>', ' ')).html();
        var title = 'Settlement';
        var calculateTypeEnum = settlementManagement.enums.CalculateType;
        switch (type) {
            case calculateTypeEnum.Refund:
                title = 'Refund';
                height += 80;
                break;
            case calculateTypeEnum.Billing:
                height += 180;
                break;
            case calculateTypeEnum.KeyScore:
                title = 'Key Score';
                break;
        }

        OpenPopup(url, title, width, height, false, false, window, 1000, null, null);
    },
    doCalMultiple: function () {
    }
};
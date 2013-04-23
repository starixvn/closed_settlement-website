



var FinancialReportManager = {
    reportDate: null,
    sportTypeId: null,
    bettype: null,
    path: "Finance",
    backToMainMenuText: "Back to Main Menu",
    title: "",
    noneLiveReportTitle: "",
    liveReportTitle: "",
    reportDetailTitle: "",
    reportDetailPath: "",
    initGrid: function (sportId, wldate) {
        FinancialReportManager.sportTypeId = sportId;
        FinancialReportManager.reportDate = wldate;

        $("#btnReport").unbind("click");
        $("#btnExport").unbind("click");
        $("#btnReport").click(function () {
            var reportDate = $("#txtfromdate").val();
            FinancialReportManager.viewReport(reportDate);
        });
        $("#btnExport").click(function () {
            FinancialReportManager.exportToExcel();
        });

        $('#spMoreElement').empty();
        var strtoday = '<input type="button" onclick="FinancialReportManager.viewReportByDate(1)" id="btnToday" name="Today" class="btn"/>';
        var stryesterday = '<input type="button" onclick="FinancialReportManager.viewReportByDate(2)" id="btnYesterday" name="Yesterday" class="btn"/>';
        $('#spMoreElement').append(strtoday);
        $('#spMoreElement').append(stryesterday);
        $('#btnToday').val(today);
        $('#btnYesterday').val(yesterday);

        var fullpath = "<b><a onclick=\"WL2hShowHideDivManager.showMainReportFromFIR('" + sportId +
                         "','" + wldate + "')\"> Main </a> > " + FinancialReportManager.path + "</b>";
        $("#trPath").html(fullpath);
        $("#backToMainMenu_FIR").html(FinancialReportManager.backToMainMenuText);
        $("#backToMainMenu_FIR").unbind("click");
        $("#backToMainMenu_FIR").click(function () {
            WL2hShowHideDivManager.showMainReportFromFIR(sportId, wldate);
        });

        $("#btnExport_FinDetail").unbind("click");
        $("#btnExport_FinDetail").click(function () {
            FinancialReportManager.exportReportDetail();
        });

        FinancialReportManager.initReportTable();
        $("#dailyWLReportDiv").removeClass("hidden");
        try { parent.jQuery.unblockUI(); } catch (e) { }
        FinancialReportManager.viewReport(wldate);
    },
    initReportTable: function () {
        FinancialReportManager.reportView = $("#financeReport");

        FinancialReportManager.nonLiveFIR = $("#nonLiveFinanceReport");
        FinancialReportManager.nonLiveFIR_TB = $("#tblContent_NlFin tbody");
        FinancialReportManager.nonLiveFIR_TF = $("#tblFooter_NlFin");

        FinancialReportManager.liveFIR = $("#liveFinanceReport");
        FinancialReportManager.liveFIR_TB = $("#tblContent_LFin tbody");
        FinancialReportManager.liveFIR_TF = $("#tblFooter_LFin");

        FinancialReportManager.rightDetailReport = $("#rightDetails_FIR");
        FinancialReportManager.reportDetailFIR = $("#financeReportDetails");
        FinancialReportManager.reportDetailFIR_TB = $("#tblContent_RDtFin tbody");
        FinancialReportManager.reportDetailFIR_TF = $("#tblFooter_RDtFin");
    },
    viewReport: function (wldate) {
        FinancialReportManager.getLivetypeSportDetail(wldate);
    },
    viewReportByDate: function (date) {
        switch (date) {
            case 1:
                $("#txtfromdate").datepicker("setDate", new Date($('#btnToday').val()));
                FinancialReportManager.getLivetypeSportDetail($('#btnToday').val());
                break;
            case 2:
                $("#txtfromdate").datepicker("setDate", new Date($('#btnYesterday').val()));
                FinancialReportManager.getLivetypeSportDetail($('#btnYesterday').val());
                break;
            default:
                break;
        }
    },
    getLivetypeSportDetail: function (wldate) {
        if (!isAllowReadFinanceReport) {
            return;
        }
        FinancialReportManager.reportDate = wldate;
        var data = {
            param:
            {
                ReportDate: wldate
            }
        };

        try { parent.BlockUI(); } catch (e) { }
        DailyWinLoss2hManager.disableReportButton();
        $.ajax({
            type: "POST",
            url: window.rootUrl + "DailyWinLoss2h/SelFinanceReport",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: JSON.stringify(data),
            success: function (rs) {
                if ($("#InformationMssg").html() != "") {
                    $("#InformationMssg").hide();
                    $("#InformationMssg").empty();
                }
                FinancialReportManager.nonLiveFIR.addClass('hidden');
                FinancialReportManager.reportView.removeClass('hidden');

                FinancialReportManager.renderDailyWLReport(rs, wldate);
            },
            error: function () {
                FinancialReportManager.noneLiveReportTitle = 'FINACE "NON - LIVE" PRODUCT WIN/LOSS REPORT';
                $("#nL_FIRHeader").html(FinancialReportManager.noneLiveReportTitle);
                FinancialReportManager.nonLiveFIR.removeClass('hidden');
                FinancialReportManager.renderNoData_NoneLiveReport();
                FinancialReportManager.liveReportTitle = 'FINANCE "LIVE" PRODUCT WIN/LOSS REPORT';
                $("#l_FIRHeader").html(FinancialReportManager.liveReportTitle);
                FinancialReportManager.liveFIR.removeClass('hidden');
                FinancialReportManager.renderNoData_LiveReport();
            },
            complete: function () {
                DailyWinLoss2hManager.enableReportButton();
                try { parent.jQuery.unblockUI(); } catch (e) { }
            }
        });
    },
    renderDailyWLReport: function (dailyWLResult, wldate) {
        if (dailyWLResult) {
            $("#gridSplitter_FIR").addClass("hidden");
            FinancialReportManager.rightDetailReport.addClass("hidden");
            isBARMActive = dailyWLResult.NoneLiveReport.IsBARMActive.toString();
            FinancialReportManager.renderNoneLiveReportDWL2h(dailyWLResult.NoneLiveReport, wldate);
            FinancialReportManager.renderLiveReportDWL2h(dailyWLResult.LiveReport, wldate);
        }
    },
    renderNoneLiveReportDWL2h: function (noneLiveReport, wldate) {
        FinancialReportManager.noneLiveReportTitle = 'FINACE "NON - LIVE" PRODUCT WIN/LOSS REPORT';
        $("#nL_FIRHeader").html(FinancialReportManager.noneLiveReportTitle);
        var items = noneLiveReport.ReportDetails;
        FinancialReportManager.nonLiveFIR.removeClass('hidden');
        if (items != null && items.length > 0) {
            var arrBody = [];
            var arrFooter = [];
            var numberItems = items.length;
            var rowOrder = 0;
            for (var i = 0; i < numberItems; i++) {
                arrBody.push(FinancialReportManager.renderMainRow_LeftReport(items[i], (rowOrder % 2 == 0 ? "evenrow" : "")));
                rowOrder++;
            }
            arrBody.push(FinancialReportManager.renderEmptyRow_LeftReport());
            arrFooter.push(FinancialReportManager.renderTotalRow_LeftReport(noneLiveReport.ReportTotal.NormalTotal));

            var allIBCTotal = noneLiveReport.ReportTotal.EightRMTotal.WinLoss;
            if (isBARMActive == 'true') {
                allIBCTotal += noneLiveReport.ReportTotal.BARMTotal.WinLoss;
            }

            if (allIBCTotal != 0) {
                arrFooter.push(FinancialReportManager.render8RMTotalRow_LeftReport(allIBCTotal));
            }

            var bodyHtml = arrBody.join('');
            bodyHtml = StringFormatter.removeSpaceIE9(bodyHtml);
            FinancialReportManager.nonLiveFIR_TB.html(bodyHtml);

            var footerHtml = arrFooter.join('');
            footerHtml = StringFormatter.removeSpaceIE9(footerHtml);
            FinancialReportManager.nonLiveFIR_TF.html(footerHtml);
            FinancialReportManager.nonLiveFIR_TF.removeClass('hidden');
            FinancialReportManager.nonLiveFIR.removeClass('hidden').reRenderScroller(531, 500, true);
        }
        else {
            FinancialReportManager.renderNoData_NoneLiveReport();
        }
    },
    renderLiveReportDWL2h: function (liveReport, wldate) {
        FinancialReportManager.liveReportTitle = 'FINANCE "LIVE" PRODUCT WIN/LOSS REPORT';
        $("#l_FIRHeader").html(FinancialReportManager.liveReportTitle);
        var items = liveReport.ReportDetails;
        FinancialReportManager.liveFIR.removeClass('hidden');
        if (items != null && items.length > 0) {
            var arrBody = [];
            var arrFooter = [];
            var numberItems = items.length;
            var rowOrder = 0;
            for (var i = 0; i < numberItems; i++) {
                arrBody.push(FinancialReportManager.renderMainRow_LeftReport(items[i], (rowOrder % 2 == 0 ? "evenrow" : "")));
                rowOrder++;
            }
            arrBody.push(FinancialReportManager.renderEmptyRow_LeftReport());
            arrFooter.push(FinancialReportManager.renderTotalRow_LeftReport(liveReport.Total.Total));

            var allIBCTotal = liveReport.ReportTotal.EightRMTotal.WinLoss;
            if (isBARMActive == 'true') {
                allIBCTotal += liveReport.ReportTotal.BARMTotal.WinLoss;
            }

            if (allIBCTotal != 0) {
                arrFooter.push(SoccerReportManager.render8RMTotalRow_LeftReport(allIBCTotal));
            }

            var bodyHtml = arrBody.join('');
            bodyHtml = StringFormatter.removeSpaceIE9(bodyHtml);
            FinancialReportManager.liveFIR_TB.html(bodyHtml);

            var footerHtml = arrFooter.join('');
            footerHtml = StringFormatter.removeSpaceIE9(footerHtml);
            FinancialReportManager.liveFIR_TF.html(footerHtml);
            FinancialReportManager.liveFIR_TF.removeClass('hidden');
            FinancialReportManager.liveFIR.removeClass('hidden').reRenderScroller(531, 500, true);
        }
        else {
            FinancialReportManager.renderNoData_LiveReport();
        }
    },
    renderNoData_NoneLiveReport: function () {
        FinancialReportManager.nonLiveFIR_TB.html('<tr id="noinfo"><td id="tdNoInfomation" colspan="7" class="b c" style="width: 528px;" >There is no information available.</td></tr>');
        FinancialReportManager.nonLiveFIR_TF.addClass('hidden');
        FinancialReportManager.nonLiveFIR.removeClass('hidden').reRenderScroller(531, 500, true);
    },
    renderNoData_LiveReport: function () {
        FinancialReportManager.liveFIR_TB.html('<tr id="noinfo"><td id="tdNoInfomation" colspan="7" class="b c" style="width: 528px;" >There is no information available.</td></tr>');
        FinancialReportManager.liveFIR_TF.addClass('hidden');
        FinancialReportManager.liveFIR.removeClass('hidden').reRenderScroller(531, 500, true);
    },
    renderMainRow_LeftReport: function (row, cssClass) {
        var html = '';
        html = '<tr class="' + cssClass + '">\
                <td><div class="l b col100" style="">' + FinancialReportManager.getReportLink(row) + '</div></td>\
                <td><div class="r col80 ' + (row.WinLoss.toFixed(2) > 0 ? 'red' : '') + '" style="">' + StringFormatter.formatNumber(row.WinLoss) + '</div></td>\
                <td><div class="r longtext-wrapper" style="width: 70px;">' + StringFormatter.formatNumber(row.Position) + '</div></td>\
                <td><div class="r longtext-wrapper" style="width: 50px;">' + StringFormatter.formatNumber(row.Commission) + '</div></td>\
                <td><div class="r col80 ' + (row.LineTotal.toFixed(2) > 0 ? 'red' : '') + '" style="">' + StringFormatter.formatNumber(row.LineTotal) + '</div></td>\
                <td><div class="r col80" style="">' + StringFormatter.formatNumber(row.Turnover) + '</div></td>\
                <td><div class="r longtext-wrapper ' + (row.Turnover.toFixed(2) != 0 && row.Margin.toFixed(2) > 0 ? 'red' : '') + '" style="width: 50px;">' + (row.Turnover != 0 ? StringFormatter.formatNumber(row.Margin) + '%' : 'N/A') + '</div></td>\
            </tr>';
        return html;
    },
    renderTotalRow_LeftReport: function (row) {
        var html = '';
        html = '<tr>\
                <td><div class="r b col100" style="width: 100px;">All Total</div></td>\
                <td><div class="r col80 ' + (row.WinLoss.toFixed(2) > 0 ? 'red' : '') + '" style="font-weight: normal;">' + StringFormatter.formatNumber(row.WinLoss) + '</div></td>\
                <td><div class="r longtext-wrapper" style="font-weight: normal; width: 70px;">' + StringFormatter.formatNumber(row.Position) + '</div></td>\
                <td><div class="r longtext-wrapper" style="width: 50px; font-weight: normal;">' + StringFormatter.formatNumber(row.Commission) + '</div></td>\
                <td style="background-color:#B5C4D0"><div class="r b col80 ' + (row.LineTotal.toFixed(2) > 0 ? 'red' : '') + '" style="">' + StringFormatter.formatNumber(row.LineTotal) + '</div></td>\
                <td><div class="r b col80" style="">' + StringFormatter.formatNumber(row.Turnover) + '</div></td>\
                <td><div class="r longtext-wrapper ' + (row.Turnover.toFixed(2) != 0 && row.Margin.toFixed(2) > 0 ? 'red' : '') + '" style="width: 50px; font-weight: normal;">' + (row.Turnover != 0 ? StringFormatter.formatNumber(row.Margin) + '%' : 'N/A') + '</div></td>\
            </tr>';
        return html;
    },
    render8RMTotalRow_LeftReport: function (ibcValue) {
        var html = '<tr>\
                        <td><div class="r b col100" style="">IBC(8RM' +
                                        (isBARMActive == 'true' ? ' + ' + barmStr : '') + ') Total:</div></td>\
                        <td><div class="r b col80 ' + (ibcValue.toFixed(2) > 0 ? 'red' : '') + '" style="">' + StringFormatter.formatNumber(ibcValue) + '</div></td>\
                        <td colspan ="5" class ="c" style="height:18px;"></td>\
                    </tr>';
        return html;
    },
    renderEmptyRow_LeftReport: function () {
        var html = '<tr>\
                        <td colspan ="7" class ="c" style="height:18px;"></td>\
                    </tr>';
        return html;
    },
    getReportLink: function (normalWLItem) {
        var evt = "FinancialReportManager.openReportDetails('" + normalWLItem.Id + "','" +
                                                        normalWLItem.Name + "','" +
                                                        normalWLItem.Hedge + "','" +
                                                        FinancialReportManager.sportTypeId + "','" +
                                                        FinancialReportManager.reportDate + "','" +
                                                        normalWLItem.IsLive + "'); return;";
        var reportLink = '<b>\
                             <a onclick="' + evt + '">' + normalWLItem.AliasName + '</a>\
                          </b>';

        switch (normalWLItem.Id) {
            case 71:
                reportLink = "Casino";
                break;
            case 72:
                reportLink = "P2P";
                break;
            case 73:
                reportLink = "IBCbet Bingo";
                break;
        }
        return reportLink;
    },
    openReportDetails: function (bettypeId, bettypeName, hedge, sporttypeId, reportDate, isLive) {
        if (!isAllowReadFinanceReport) {
            return;
        }
        return;

        var isHedge = (hedge.indexOf("(hedge)") != -1);
        var data = {
            param:
            {
                ReportDate: reportDate,
                SportType: FinancialReportManager.sportTypeId,
                LiveIndicator: isLive,
                BetType: bettypeId,
                IsHedge: isHedge,
                IsLive: (isLive == 'true' ? 1 : 0)
            }
        };
        try { parent.BlockUI(); } catch (e) { }
        DailyWinLoss2hManager.disableReportButton();
        $.ajax({
            type: "POST",
            url: window.rootUrl + "DailyWinLoss2h/SelDailyWLDirectDetails",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: JSON.stringify(data),
            success: function (rs) {
                if ($("#InformationMssg").html() != "") {
                    $("#InformationMssg").hide();
                    $("#InformationMssg").empty();
                }
                FinancialReportManager.reportDetailFIR.addClass('hidden');
                $("#gridSplitter_FIR").removeClass('hidden');
                FinancialReportManager.rightDetailReport.removeClass("hidden");
                FinancialReportManager.renderReportDetailsDWL2h(rs, reportDate, bettypeId, bettypeName, isHedge, isLive);
            },
            error: function () {
                FinancialReportManager.reportDetailTitle = 'FINANCE EVENT WIN/LOSS REPORT';
                $("#RDt_FIRHeader").html(FinancialReportManager.reportDetailTitle);
                FinancialReportManager.reportDetailPath = "Main>Finance>";
                $("#RDt_FIRHeader_Path").html(FinancialReportManager.reportDetailPath);
                FinancialReportManager.liveFIR.removeClass('hidden');
                FinancialReportManager.renderNoData_ReportDetails();
            },
            complete: function () {
                DailyWinLoss2hManager.enableReportButton();
                try { parent.jQuery.unblockUI(); } catch (e) { }
            }
        });
    },
    renderReportDetailsDWL2h: function (reportDetail, wldate, bettypeId, bettypeName, isHedge, isLive) {
        isBARMActive = reportDetail.IsBARMActive.toString();
        FinancialReportManager.reportDetailTitle = 'FINANCE EVENT WIN/LOSS REPORT';
        $("#RDt_FIRHeader").html(FinancialReportManager.reportDetailTitle);
        FinancialReportManager.reportDetailPath = "Main>Finance>" + bettypeName;
        //FinancialReportManager.reportDetailPath += (isHedge ? "(headge)" : "");
        $("#RDt_FIRHeader_Path").html(FinancialReportManager.reportDetailPath);
        var items = reportDetail.ReportDetails;
        FinancialReportManager.liveFIR.removeClass('hidden');
        if (items != null && items.length > 0) {
            var arrBody = [];
            var arrFooter = [];
            var numberItems = items.length;
            var rowOrder = 0;
            var odd_even;
            for (var i = 0; i < numberItems; i++) {
                arrBody.push(FinancialReportManager.renderDailyWLRowSportDetails(items[i].NormalItems[j], odd_even));
            }
            arrFooter.push(FinancialReportManager.renderTotalRow_RightReport(reportDetail.ReportTotal.NormalTotal, isLive, bettypeName, isHedge));

            var allIBCTotal = reportDetail.ReportTotal.EightRMTotal.WinLoss;
            if (isBARMActive == 'true') {
                allIBCTotal += reportDetail.ReportTotal.BARMTotal.WinLoss;
            }

            if (allIBCTotal != 0) {
                arrFooter.push(FinancialReportManager.render8RMTotalRow_RightReport(allIBCTotal));
            }

            var bodyHtml = arrBody.join('');
            bodyHtml = StringFormatter.removeSpaceIE9(bodyHtml);
            FinancialReportManager.reportDetailFIR_TB.html(bodyHtml);

            var footerHtml = arrFooter.join('');
            footerHtml = StringFormatter.removeSpaceIE9(footerHtml);
            FinancialReportManager.reportDetailFIR_TF.html(footerHtml);
            FinancialReportManager.reportDetailFIR_TF.removeClass('hidden');
            FinancialReportManager.reportDetailFIR.removeClass('hidden').reRenderScroller(681, 500, true);

        }
        else {
            FinancialReportManager.renderNoData_ReportDetails();
        }
    },
    renderDailyWLRowSportDetails: function (dwlRow, index) {
        var html = "";
        switch (dwlRow.RowTypeDWL2h.Id) {
            case 0:
                html = SoccerReportManager.renderMainRow_RightReport(dwlRow, index);
                break;
            case 3:
                html = SoccerReportManager.renderBettypeTotalRow_RightReport(dwlRow);
                break;
            case 9:
                html = SoccerReportManager.emptyRowHtml_RightReport(dwlRow);
                break;
            case 10:
                html = SoccerReportManager.renderLeagueHeadLine(dwlRow);
                break;
        }
        return html;
    },
    renderLeagueHeadLine: function (dwlRow) {
        var html = '';
        html = '<tr>\
                    <td style="width: 90px;">\
                        <div></div></td>\
                    <td colspan = "9" style="width: 570px;"><div class="l b longtext-wrapper">' + dwlRow.Name + '</div></td>\
                </tr>';
        return html;
    },
    renderMainRow_RightReport: function (row, index) {
        var html = '';
        var eventDetail = "";
        if (FinancialReportManager.bettype == 10 || FinancialReportManager.sportTypeId == 201) {
            eventDetail = '<td colspan = "3"><div class="c longtext-wrapper" style="width: 167px;">' + row.MatchInfo.HomeTeam.Name + (row.MatchInfo.IsNeutral ? " (N)" : "") + '</div></td>';
        }
        else {
            eventDetail = '<td><div class="r longtext-wrapper" style="width: 70px;">' + row.MatchInfo.HomeTeam.Name + (row.MatchInfo.IsNeutral ? " (N)" : "") + '</div></td>\
                        <td class="c" style="width: 31px;">-vs-</td>\
                        <td><div class="l longtext-wrapper" style="width: 70px;">' + row.MatchInfo.AwayTeam.Name + '</div></td>';
        }

        html = '<tr class="' + (row.RowIndex % 2 == 0 ? "evenrow" : "") + '">\
                    <td style="word-wrap: break-word;"><div class="c" style="width: 90px;">' + row.MatchInfo.Name + '</div></td>'
                   + eventDetail + '<td><div class="r col80 ' + (row.WinLoss.toFixed(2) > 0 ? 'red' : '') + '" style="">' + StringFormatter.formatNumber(row.WinLoss) + '</div></td>\
                    <td><div class="r longtext-wrapper" style="width: 70px;">' + StringFormatter.formatNumber(row.Position) + '</div></td>\
                    <td><div class="r longtext-wrapper" style="width: 50px;">' + StringFormatter.formatNumber(row.Commission) + '</div></td>\
                    <td><div class="r col80 ' + (row.LineTotal.toFixed(2) > 0 ? 'red' : '') + '" style="">' + StringFormatter.formatNumber(row.LineTotal) + '</div></td>\
                    <td><div class="r col80" style="">' + StringFormatter.formatNumber(row.Turnover) + '</div></td>\
                    <td><div class="r longtext-wrapper ' + (row.Turnover.toFixed(2) != 0 && row.Margin.toFixed(2) > 0 ? 'red' : '') + '" style="width: 40px;">' + (row.Turnover != 0 ? StringFormatter.formatNumber(row.Margin) + '%' : 'N/A') + '</div></td></tr>';
        return html;
    },
    renderBettypeTotalRow_RightReport: function (row) {
        var html = '';
        html = '<tr>\
                <td class="tdtitle" bgcolor="#888888" colspan = "4" ><div class="r b" style="width: 260px;"></div></td>\
                <td><div class="r col80 ' + (row.WinLoss.toFixed(2) > 0 ? 'red' : '') + '" style="font-weight: normal;">' + StringFormatter.formatNumber(row.WinLoss) + '</div></td>\
                <td><div class="r longtext-wrapper" style="font-weight: normal; width: 70px;">' + StringFormatter.formatNumber(row.Position) + '</div></td>\
                <td><div class="r longtext-wrapper" style="width: 50px; font-weight: normal;">' + StringFormatter.formatNumber(row.Commission) + '</div></td>\
                <td style=""><div class="r col80 ' + (row.LineTotal.toFixed(2) > 0 ? 'red' : '') + '" style="">' + StringFormatter.formatNumber(row.LineTotal) + '</div></td>\
                <td><div class="r col80" style="">' + StringFormatter.formatNumber(row.Turnover) + '</div></td>\
                <td><div class="r longtext-wrapper ' + (row.Turnover.toFixed(2) != 0 && row.Margin.toFixed(2) > 0 ? 'red' : '') + '" style="width: 40px; font-weight: normal;">' + (row.Turnover != 0 ? StringFormatter.formatNumber(row.Margin) + '%' : 'N/A') + '</div></td>\
            </tr>';
        return html;
    },
    renderTotalRow_RightReport: function (row, isLive, bettypeName, isHedge) {
        var html = '';
        var totalTitle = (isLive == "true" ? "<font color='red'>Live!</font>" : "") + " " +
                        bettypeName + (isHedge ? " (Hegde)" : "") + " Total:";
        html = '<tr>\
                <td colspan = "4"><div class="r b longtext-wrapper" style="width: 260px;">' + totalTitle + '</div></td>\
                <td><div class="r col80 ' + (row.WinLoss.toFixed(2) > 0 ? 'red' : '') + '" style="font-weight: normal;">' + StringFormatter.formatNumber(row.WinLoss) + '</div></td>\
                <td><div class="r longtext-wrapper" style="font-weight: normal; width: 70px;">' + StringFormatter.formatNumber(row.Position) + '</div></td>\
                <td><div class="r longtext-wrapper" style="width: 50px; font-weight: normal;">' + StringFormatter.formatNumber(row.Commission) + '</div></td>\
                <td style=""><div class="r col80 ' + (row.LineTotal.toFixed(2) > 0 ? 'red' : '') + '" style="font-weight: normal;">' + StringFormatter.formatNumber(row.LineTotal) + '</div></td>\
                <td><div class="r col80" style="font-weight: normal;">' + StringFormatter.formatNumber(row.Turnover) + '</div></td>\
                <td><div class="r longtext-wrapper ' + (row.Turnover.toFixed(2) != 0 && row.Margin.toFixed(2) > 0 ? 'red' : '') + '" style="width: 40px; font-weight: normal;">' + (row.Turnover != 0 ? StringFormatter.formatNumber(row.Margin) + '%' : 'N/A') + '</div></td>\
            </tr>';
        return html;
    },
    render8RMTotalRow_RightReport: function (ibcValue) {
        var html = '<tr>\
                        <td colspan = "4"><div class="r b" style="width: 260px;">IBC(8RM' +
                                                   (isBARMActive == 'true' ? ' + ' + barmStr : '')
                                                   + ') Total:</div></td>\
                        <td><div class="r b col80 ' + (ibcValue.toFixed(2) > 0 ? 'red' : '') + '" style="">' + StringFormatter.formatNumber(ibcValue) + '</div></td>\
                        <td colspan ="5" class ="c" style="height:18px; width: 534px;"></td>\
                    </tr>';
        return html;
    },
    renderNoData_ReportDetails: function () {
        FinancialReportManager.reportDetailFIR_TB.html('<tr id="noinfo"><td id="tdNoInfomation" colspan="10" class="b c" style="width: 678px;" >There is no information available.</td></tr>');
        FinancialReportManager.reportDetailFIR_TF.addClass('hidden');
        FinancialReportManager.reportDetailFIR.removeClass('hidden').reRenderScroller(681, 500, true);
    },
    exportToExcel: function () {
        if (!isAllowReadFinanceReport) {
            return;
        }
    },
    exportReportDetail: function () {
        if (!isAllowReadFinanceReport) {
            return;
        }
    }
};

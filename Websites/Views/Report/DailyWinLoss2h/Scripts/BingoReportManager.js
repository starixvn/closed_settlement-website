

var BingoReportManager = {
    reportDate: null,
    sportTypeId: null,
    sportTypeName: null,
    bettype: null,
    bettypename: null,
    path: "Bingo",
    backToMainMenuText: "Back to Main Menu",
    isFirstLoad: true,
    title: "",
    noneLiveReportTitle: "",
    liveReportTitle: "",
    reportDetailTitle: "",
    reportDetailPath: "",
    initGrid: function (sportId, wldate) {
        BingoReportManager.sportTypeId = sportId;
        BingoReportManager.reportDate = wldate;

        $("#btnReport").unbind("click");
        $("#btnExport").unbind("click");
        $("#btnReport").click(function () {
            var reportDate = $("#txtfromdate").val();
            BingoReportManager.viewReport(reportDate);
        });
        $("#btnExport").click(function () {
            BingoReportManager.exportToExcel();
        });

        $('#spMoreElement').empty();
        var strtoday = '<input type="button" onclick="BingoReportManager.viewReportByDate(1)" id="btnToday" name="Today" class="btn"/>';
        var stryesterday = '<input type="button" onclick="BingoReportManager.viewReportByDate(2)" id="btnYesterday" name="Yesterday" class="btn"/>';
        $('#spMoreElement').append(strtoday);
        $('#spMoreElement').append(stryesterday);
        $('#btnToday').val(today);
        $('#btnYesterday').val(yesterday);

        var fullpath = "<b><a onclick=\"WL2hShowHideDivManager.showMainReportFromBGR('" + sportId +
                         "','" + wldate + "')\"> Main </a> > " + BingoReportManager.path + "</b>";
        $("#trPath").html(fullpath);

        $("#backToMainMenu_BGR").html(BingoReportManager.backToMainMenuText);
        $("#backToMainMenu_BGR").unbind("click");
        $("#backToMainMenu_BGR").click(function () {
            WL2hShowHideDivManager.showMainReportFromBGR(sportId, wldate);
        });

        $("#btnExport_BgoDetail").unbind("click");
        $("#btnExport_BgoDetail").click(function () {
            BingoReportManager.exportReportDetail();
        });

        BingoReportManager.initReportTable();
        $("#dailyWLReportDiv").removeClass("hidden");
        try { parent.jQuery.unblockUI(); } catch (e) { }
        BingoReportManager.viewReport(wldate);
    },
    initReportTable: function () {
        BingoReportManager.reportView = $("#bingoReport");

        BingoReportManager.nonLiveBGR = $("#nonLiveBingoReport");
        BingoReportManager.nonLiveBGR_TB = $("#tblContent_NlBgo");

        BingoReportManager.rightDetailReport = $("#rightDetails_BGR");
        BingoReportManager.reportDetailBGR = $("#bingoReportDetails");
        BingoReportManager.reportDetailBGR_TB = $("#tblContent_RDtBgo tbody");
        BingoReportManager.reportDetailBGR_TF = $("#tblFooter_RDtBgo");
    },
    viewReport: function (wldate) {
        BingoReportManager.getLivetypeSportDetail(wldate);
    },
    viewReportByDate: function (date) {
        switch (date) {
            case 1:
                $("#txtfromdate").datepicker("setDate", new Date($('#btnToday').val()));
                BingoReportManager.getLivetypeSportDetail($('#btnToday').val());
                break;
            case 2:
                $("#txtfromdate").datepicker("setDate", new Date($('#btnYesterday').val()));
                BingoReportManager.getLivetypeSportDetail($('#btnYesterday').val());
                break;
            default:
                break;
        }
    },
    getLivetypeSportDetail: function (wldate) {
        if (!isAllowReadBingoReport) {
            return;
        }
        BingoReportManager.reportDate = wldate;
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
            url: window.rootUrl + "DailyWinLoss2h/SelBingoReport",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: JSON.stringify(data),
            success: function (rs) {
                if ($("#InformationMssg").html() != "") {
                    $("#InformationMssg").hide();
                    $("#InformationMssg").empty();
                }
                BingoReportManager.nonLiveBGR.addClass('hidden');
                BingoReportManager.reportView.removeClass('hidden');

                BingoReportManager.renderDailyWLReport(rs, wldate);
            },
            error: function () {
                BingoReportManager.noneLiveReportTitle = 'BINGO "NON - LIVE" PRODUCT WIN/LOSS REPORT';
                $("#nL_BgoHeader").html(BingoReportManager.noneLiveReportTitle);
                BingoReportManager.renderNoData_NoneLiveReport();

            },
            complete: function () {
                DailyWinLoss2hManager.enableReportButton();
                try { parent.jQuery.unblockUI(); } catch (e) { }
            }
        });
    },
    renderDailyWLReport: function (dailyWLResult, wldate) {
        if (dailyWLResult) {
            $("#gridSplitter_BGR").addClass('hidden');
            BingoReportManager.rightDetailReport.addClass("hidden");
            BingoReportManager.renderNoneLiveReportDWL2h(dailyWLResult.NoneLiveReport, wldate);
        }
    },
    renderNoneLiveReportDWL2h: function (noneLiveReport, wldate) {
        BingoReportManager.noneLiveReportTitle = 'BINGO "NON - LIVE" PRODUCT WIN/LOSS REPORT';
        $("#nL_BgoHeader").html(BingoReportManager.noneLiveReportTitle);
        var items = noneLiveReport.ReportDetails;
        BingoReportManager.nonLiveBGR.removeClass('hidden');
        if (items != null && items.length > 0) {
            var arrBody = [];
            var arrFooter = [];
            var numberItems = items.length;
            var rowOrder = 0;
            for (var i = 0; i < numberItems; i++) {
                arrBody.push(BingoReportManager.renderMainRow_LeftReport(items[i], (rowOrder % 2 == 0 ? "evenrow" : "")));
                rowOrder++;
            }
            arrBody.push(BingoReportManager.renderEmptyRow_LeftReport());
            arrFooter.push(BingoReportManager.renderTotalRow_LeftReport(noneLiveReport.ReportTotal.NormalTotal));

            var bodyHtml = arrBody.join('');
            bodyHtml = StringFormatter.removeSpaceIE9(bodyHtml);
            BingoReportManager.nonLiveBGR_TB.find("tr:not(.header)").remove();
            BingoReportManager.nonLiveBGR_TB.append(bodyHtml);

            var footerHtml = arrFooter.join('');
            footerHtml = StringFormatter.removeSpaceIE9(footerHtml);
            BingoReportManager.nonLiveBGR_TB.append(footerHtml);
            BingoReportManager.nonLiveBGR.removeClass('hidden');
        }
        else {
            BingoReportManager.renderNoData_NoneLiveReport();
        }
    },
    renderMainRow_LeftReport: function (row, cssClass) {
        var html = '';
        html = '<tr class="' + cssClass + '">\
                <td><div class="l b col160" style="">' + BingoReportManager.getReportLink(row) + '</div></td>\
                <td><div class="r col100 ' + (row.WinLoss.toFixed(2) > 0 ? 'red' : '') + '" style="">' + StringFormatter.formatNumber(row.WinLoss) + '</div></td>\
                <td><div class="r col90" style="">' + StringFormatter.formatNumber(row.Position) + '</div></td>\
                <td><div class="r col80" style="">' + StringFormatter.formatNumber(row.Commission) + '</div></td>\
                <td><div class="r col100 ' + (row.LineTotal.toFixed(2) > 0 ? 'red' : '') + '" style="">' + StringFormatter.formatNumber(row.LineTotal) + '</div></td>\
                <td><div class="r col100" style="">' + StringFormatter.formatNumber(row.Turnover) + '</div></td>\
                <td><div class="r col80 ' + (row.Turnover.toFixed(2) != 0 && row.Margin.toFixed(2) > 0 ? 'red' : '') + '" style="">' + (row.Turnover.toFixed(2) != 0 ? StringFormatter.formatNumber(row.Margin) + '%' : 'N/A') + '</div></td>\
            </tr>';
        return html;
    },
    renderTotalRow_LeftReport: function (row) {
        var html = '';
        html = '<tr>\
                <td><div class="r b col160" style="">All Total:</div></td>\
                <td><div class="r col100 ' + (row.WinLoss.toFixed(2) > 0 ? 'red' : '') + '" style="font-weight: normal;">' + StringFormatter.formatNumber(row.WinLoss) + '</div></td>\
                <td><div class="r col90" style="font-weight: normal;">' + StringFormatter.formatNumber(row.Position) + '</div></td>\
                <td><div class="r col80" style="font-weight: normal;">' + StringFormatter.formatNumber(row.Commission) + '</div></td>\
                <td style="background-color:#B5C4D0"><div class="r b col100 ' + (row.LineTotal.toFixed(2) > 0 ? 'red' : '') + '" style="">' + StringFormatter.formatNumber(row.LineTotal) + '</div></td>\
                <td><div class="r b col100" style="">' + StringFormatter.formatNumber(row.Turnover) + '</div></td>\
                <td><div class="r col80 ' + (row.Turnover.toFixed(2) != 0 && row.Margin.toFixed(2) > 0 ? 'red' : '') + '" style="font-weight: normal;">' + (row.Turnover.toFixed(2) != 0 ? StringFormatter.formatNumber(row.Margin) + '%' : 'N/A') + '</div></td>\
            </tr>';
        return html;
    },
    renderEmptyRow_LeftReport: function () {
        var html = '<tr>\
                        <td colspan ="7" class ="c" style="height:18px;"></td>\
                    </tr>';
        return html;
    },
    renderNoData_NoneLiveReport: function () {
        BingoReportManager.nonLiveBGR_TB.find("tr:not(.header)").remove();
        BingoReportManager.nonLiveBGR_TB.append('<tr id="noinfo"><td id="tdNoInfomation" colspan="7" class="b c" style="" >There is no information available.</td></tr>');
        BingoReportManager.nonLiveBGR.removeClass('hidden');
    },
    getReportLink: function (normalWLItem) {
        var evt = "BingoReportManager.openReportDetails('" + normalWLItem.Id + "','" +
                                                        normalWLItem.Name + "','" +
                                                        BingoReportManager.sportTypeId + "','" +
                                                        BingoReportManager.reportDate + "','" +
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
            default:
                reportLink = normalWLItem.AliasName;
                break;
        }
        return reportLink;
    },
    openReportDetails: function (bettypeId, bettypeName, sporttypeId, reportDate, isLive) {
        if (!isAllowReadBingoReport) {
            return;
        }
        return;

        var data = {
            param:
            {
                ReportDate: reportDate,
                SportType: BingoReportManager.sportTypeId,
                LiveIndicator: isLive,
                BetType: bettypeId,
                IsHedge: 0,
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
                BingoReportManager.reportDetailBGR.addClass('hidden');
                $("#gridSplitterCSR").removeClass("hidden");
                BingoReportManager.rightDetailReport.removeClass("hidden");
                BingoReportManager.renderReportDetailsDWL2h(rs, reportDate, bettypeId, bettypeName, isLive);
            },
            error: function () {
                BingoReportManager.reportDetailTitle = 'BINGO WIN/LOSS REPORT';
                $("#RDt_BGRHeader").html(BingoReportManager.liveReportTitle);
                BingoReportManager.reportDetailPath = "Main>Bingo>";
                $("#RDt_BGRHeader_Path").html(BingoReportManager.reportDetailPath);
                BingoReportManager.renderNoData_ReportDetails();
            },
            complete: function () {
                DailyWinLoss2hManager.enableReportButton();
                try { parent.jQuery.unblockUI(); } catch (e) { }
            }
        });
    },
    renderReportDetailsDWL2h: function (reportDetail, wldate, bettypeId, bettypeName, isLive) {
        BingoReportManager.reportDetailTitle = 'BINGO WIN/LOSS REPORT';
        $("#RDt_BGRHeader").html(BingoReportManager.liveReportTitle);
        BingoReportManager.reportDetailPath = "Main>Bingo>" + bettypeName;
        $("#RDt_BGRHeader_Path").html(BingoReportManager.reportDetailPath);
        var items = reportDetail.ReportDetails;
        BingoReportManager.liveBGR.removeClass('hidden');
        if (items != null && items.length > 0) {
            var arrBody = [];
            var arrFooter = [];
            var numberItems = items.length;
            for (var i = 0; i < numberItems; i++) {
                arrBody.push(BingoReportManager.renderDailyWLRowSportDetails(items[i], i));
            }
            arrFooter.push(BingoReportManager.renderTotalRow_RightReport(reportDetail.ReportTotal.NormalTotal, isLive, bettypeName));

            var bodyHtml = arrBody.join('');
            bodyHtml = StringFormatter.removeSpaceIE9(bodyHtml);
            BingoReportManager.reportDetailBGR_TB.html(bodyHtml);

            var footerHtml = arrFooter.join('');
            footerHtml = StringFormatter.removeSpaceIE9(footerHtml);
            BingoReportManager.reportDetailBGR_TF.html(footerHtml);
            BingoReportManager.reportDetailBGR_TF.removeClass('hidden');
            BingoReportManager.reportDetailBGR.removeClass('hidden').reRenderScroller(624, 500, true);

        }
        else {
            BingoReportManager.renderNoData_ReportDetails();
        }
    },
    renderDailyWLRowSportDetails: function (dwlRow, index) {
        var html = "";
        switch (dwlRow.RowTypeDWL2h.Id) {
            case 0:
                html = BingoReportManager.renderMainRow_RightReport(dwlRow, index);
                break;
            case 3:
                html = BingoReportManager.renderBettypeTotalRow_RightReport(dwlRow);
                break;
            case 9:
                html = BingoReportManager.emptyRowHtml_RightReport(dwlRow);
                break;
            case 10:
                html = BingoReportManager.renderLeagueHeadLine(dwlRow);
                break;
        }
        return html;
    },
    renderLeagueHeadLine: function (dwlRow) {
        var html = '';
        html = '<tr>\
                    <td style="width: 60px;">\
                        <div></div></td>\
                    <td colspan = "9" style=""><div class="l b longtext-wrapper">' + dwlRow.Name + '</div></td>\
                </tr>';
        return html;
    },
    renderMainRow_RightReport: function (row, index) {
        var html = '';

        if (BingoReportManager.bettype == 10 || BingoReportManager.sportTypeId == 201) {
            eventDetail = '<td colspan = "3"><div class="c longtext-wrapper" style="width: 167px;">' + "" + '</div></td>';
        }
        else {
            eventDetail = '<td><div class="r longtext-wrapper" style="width: 70px;">' + "" + '</div></td>\
                        <td class="c" style="width: 31px;">-vs-</td>\
                        <td><div class="l longtext-wrapper" style="width: 70px;">' + "" + '</div></td>';
        }

        html = '<tr class="' + (row.RowIndex % 2 == 0 ? "evenrow" : "") + '">\
                    <td style="word-wrap: break-word;"><div class="c" style="width: 60px;">' + row.MatchInfo.Name + '</div></td>\
                    <td><div class="r longtext-wrapper" style="width: 31px;">' + "" + '</div></td>\
                    <td class="c" style="width: 32px;">-vs-</td>\
                    <td><div class="l longtext-wrapper" style="width: 31px;">' + row.MatchInfo.AwayTeam.Name + '</div></td>\
                    <td><div class="r col80 ' + (row.WinLoss.toFixed(2) > 0 ? 'red' : '') + '" style="">' + StringFormatter.formatNumber(row.WinLoss) + '</div></td>\
                    <td><div class="r col80" style="">' + StringFormatter.formatNumber(row.Position) + '</div></td>\
                    <td><div class="r col80" style="">' + StringFormatter.formatNumber(row.Commission) + '</div></td>\
                    <td><div class="r col80 ' + (row.LineTotal.toFixed(2) > 0 ? 'red' : '') + '" style="">' + StringFormatter.formatNumber(row.LineTotal) + '</div></td>\
                    <td><div class="r" style="width: 80px;">' + StringFormatter.formatNumber(row.Turnover) + '</div></td>\
                    <td><div class="r col80 ' + (row.Turnover.toFixed(2) != 0 && row.Margin.toFixed(2) > 0 ? 'red' : '') + '" style="">' + (row.Turnover.toFixed(2) != 0 ? StringFormatter.formatNumber(row.Margin) + '%' : 'N/A') + '</div></td>\
                </tr>';
        return html;
    },
    renderBettypeTotalRow_RightReport: function (row) {
        var html = '';
        html = '<tr>\
                <td class="tdtitle" bgcolor="#888888" colspan = "4" ><div class="r b" style="width: 150px;"></div></td>\
                <td><div class="r col80 ' + (row.WinLoss.toFixed(2) > 0 ? 'red' : '') + '" style="font-weight: normal;">' + StringFormatter.formatNumber(row.WinLoss) + '</div></td>\
                <td><div class="r col80" style="font-weight: normal;">' + StringFormatter.formatNumber(row.Position) + '</div></td>\
                <td><div class="r col80" style="font-weight: normal;">' + StringFormatter.formatNumber(row.Commission) + '</div></td>\
                <td style=""><div class="r col80 ' + (row.LineTotal.toFixed(2) > 0 ? 'red' : '') + '" style="">' + StringFormatter.formatNumber(row.LineTotal) + '</div></td>\
                <td><div class="r col80" style="">' + StringFormatter.formatNumber(row.Turnover) + '</div></td>\
                <td><div class="r col80 ' + (row.Margin.toFixed(2) > 0 ? 'red' : '') + '" style="font-weight: normal;">' + (row.Turnover.toFixed(2) != 0 ? StringFormatter.formatNumber(row.Margin) + '%' : 'N/A') + '</div></td>\
            </tr>';
        return html;
    },
    renderTotalRow_RightReport: function (row, isLive, bettypeName) {
        var html = '';
        var totalTitle = (isLive == "true" ? "<font color='red'>Live!</font>" : "") + " " +
                        bettypeName + " Total:";
        html = '<tr>\
                <td colspan = "4"><div class="r b longtext-wrapper" style="width: 153px;">' + totalTitle + '</div></td>\
                <td><div class="r col80 ' + (row.WinLoss.toFixed(2) > 0 ? 'red' : '') + '" style="font-weight: normal;">' + StringFormatter.formatNumber(row.WinLoss) + '</div></td>\
                <td><div class="r col80" style="font-weight: normal;">' + StringFormatter.formatNumber(row.Position) + '</div></td>\
                <td><div class="r col80" style="font-weight: normal;">' + StringFormatter.formatNumber(row.Commission) + '</div></td>\
                <td style=""><div class="r col80 ' + (row.LineTotal.toFixed(2) > 0 ? 'red' : '') + '" style="font-weight: normal;">' + StringFormatter.formatNumber(row.LineTotal) + '</div></td>\
                <td><div class="r col80" style="font-weight: normal;">' + StringFormatter.formatNumber(row.Turnover) + '</div></td>\
                <td><div class="r col80 ' + (row.Turnover.toFixed(2) != 0 && row.Margin.toFixed(2) > 0 ? 'red' : '') + '" style="font-weight: normal;">' + (row.Turnover.toFixed(2) != 0 ? StringFormatter.formatNumber(row.Margin) + '%' : 'N/A') + '</div></td>\
            </tr>';
        return html;
    },
    renderNoData_ReportDetails: function () {
        BingoReportManager.reportDetailBGR_TB.html('<tr id="noinfo"><td id="tdNoInfomation" colspan="10" class="b c" style="width: 621px;" >There is no information available.</td></tr>');
        BingoReportManager.reportDetailBGR_TF.addClass('hidden');
        BingoReportManager.reportDetailBGR.removeClass('hidden');
    },
    exportToExcel: function () {
        if (!isAllowReadBingoReport) {
            return;
        }
        var url = 'DailyWinLoss2h?ExportType=BingoMain' +
                    '&ReportDate=' + BingoReportManager.reportDate;
        $('#frm').attr('src', url);
    },
    exportReportDetail: function () {
        if (!isAllowReadBingoReport) {
            return;
        }
    }
};
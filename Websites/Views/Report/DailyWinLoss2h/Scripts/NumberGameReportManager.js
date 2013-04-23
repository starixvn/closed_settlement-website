


var NumberGameReportManager = {
    reportDate: null,
    sportTypeId: null,
    sportTypeName: null,
    bettype: null,
    bettypename: null,
    path: "Number Game",
    backToMainMenuText: "Back to Main Menu",
    isFirstLoad: true,
    title: "",
    noneLiveReportTitle: "",
    liveReportTitle: "",
    reportDetailTitle: "",
    reportDetailPath: "",
    reportDateForDetail: null,
    isLive: null,
    IsHedge: null,
    initGrid: function (sportId, wldate) {
        NumberGameReportManager.sportTypeId = sportId;
        NumberGameReportManager.reportDate = wldate;

        $("#btnReport").unbind("click");
        $("#btnExport").unbind("click");
        $("#btnReport").click(function () {
            var reportDate = $("#txtfromdate").val();
            NumberGameReportManager.viewReport(reportDate);
        });
        $("#btnExport").click(function () {
            NumberGameReportManager.exportToExcel();
        });

        $('#spMoreElement').empty();
        var strtoday = '<input type="button" onclick="NumberGameReportManager.viewReportByDate(1)" id="btnToday" name="Today" class="btn"/>';
        var stryesterday = '<input type="button" onclick="NumberGameReportManager.viewReportByDate(2)" id="btnYesterday" name="Yesterday" class="btn"/>';
        $('#spMoreElement').append(strtoday);
        $('#spMoreElement').append(stryesterday);
        $('#btnToday').val(today);
        $('#btnYesterday').val(yesterday);

        var fullpath = "<b><a onclick=\"WL2hShowHideDivManager.showMainReportFromNGR('" + sportId +
                         "','" + wldate + "')\"> Main </a> > " + NumberGameReportManager.path + "</b>";
        $("#trPath").html(fullpath);

        $("#backToMainMenu_NGR").html(NumberGameReportManager.backToMainMenuText);
        $("#backToMainMenu_NGR").unbind("click");
        $("#backToMainMenu_NGR").click(function () {
            WL2hShowHideDivManager.showMainReportFromNGR(sportId, wldate);
        });

        $("#btnExport_NgmDetail").unbind("click");
        $("#btnExport_NgmDetail").click(function () {
            NumberGameReportManager.exportReportDetail();
        });

        NumberGameReportManager.initReportTable();
        $("#dailyWLReportDiv").removeClass("hidden");
        try { parent.jQuery.unblockUI(); } catch (e) { }
        NumberGameReportManager.viewReport(wldate);
    },
    initReportTable: function () {
        NumberGameReportManager.reportView = $("#numberGameReport");

        NumberGameReportManager.nonLiveNGR = $("#nonLiveNumberGameReport");
        NumberGameReportManager.nonLiveNGR_TB = $("#tblContent_NlNgm");

        NumberGameReportManager.liveNGR = $("#liveNumberGameReport");
        NumberGameReportManager.liveNGR_TB = $("#tblContent_LNgm");

        NumberGameReportManager.rightDetailReport = $("#rightDetails_NGR");
        //        NumberGameReportManager.rightDetailReport.css("width", $(window).width() - 605 + "px");

        NumberGameReportManager.reportDetailNGR = $("#numberGameReportDetails");
        NumberGameReportManager.reportDetailNGR_TB = $("#tblContent_RDtNgm tbody");
        NumberGameReportManager.reportDetailNGR_TF = $("#tblFooter_RDtNgm");
    },
    viewReport: function (wldate) {
        NumberGameReportManager.getLivetypeSportDetail(wldate);
    },
    viewReportByDate: function (date) {
        switch (date) {
            case 1:
                $("#txtfromdate").datepicker("setDate", new Date($('#btnToday').val()));
                NumberGameReportManager.getLivetypeSportDetail($('#btnToday').val());
                break;
            case 2:
                $("#txtfromdate").datepicker("setDate", new Date($('#btnYesterday').val()));
                NumberGameReportManager.getLivetypeSportDetail($('#btnYesterday').val());
                break;
            default:
                break;
        }
    },
    getLivetypeSportDetail: function (wldate) {
        if (!isAllowReadNumberGameReport) {
            return;
        }
        NumberGameReportManager.reportDate = wldate;
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
            url: window.rootUrl + "DailyWinLoss2h/DailyWLDirect_NumberGame",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: JSON.stringify(data),
            success: function (rs) {
                if ($("#InformationMssg").html() != "") {
                    $("#InformationMssg").hide();
                    $("#InformationMssg").empty();
                }
                NumberGameReportManager.nonLiveNGR.addClass('hidden');
                NumberGameReportManager.reportView.removeClass('hidden');

                NumberGameReportManager.renderDailyWLReport(rs, wldate);
            },
            error: function () {
                NumberGameReportManager.noneLiveReportTitle = 'NUMBER GAME "NON - LIVE" PRODUCT WIN/LOSS REPORT';
                $("#nL_NgmHeader").html(NumberGameReportManager.noneLiveReportTitle);
                NumberGameReportManager.nonLiveNGR.removeClass('hidden');
                NumberGameReportManager.renderNoData_NoneLiveReport();
                NumberGameReportManager.liveReportTitle = 'NUMBER GAME "LIVE" PRODUCT WIN/LOSS REPORT';
                $("#l_NgmHeader").html(NumberGameReportManager.liveReportTitle);
                NumberGameReportManager.liveNGR.removeClass('hidden');
                NumberGameReportManager.renderNoData_LiveReport();
            },
            complete: function () {
                $("#numberGameReport").css("width", "100%");
                DailyWinLoss2hManager.enableReportButton();
                try { parent.jQuery.unblockUI(); } catch (e) { }
            }
        });
    },
    renderDailyWLReport: function (dailyWLResult, wldate) {
        if (dailyWLResult) {
            $("#gridSplitter_NGR").addClass('hidden');
            NumberGameReportManager.rightDetailReport.addClass("hidden");
            NumberGameReportManager.renderNoneLiveReportDWL2h(dailyWLResult.NoneLiveReport, wldate);
            NumberGameReportManager.renderLiveReportDWL2h(dailyWLResult.LiveReport, wldate);
        }
    },
    renderNoneLiveReportDWL2h: function (noneLiveReport, wldate) {
        NumberGameReportManager.noneLiveReportTitle = 'NUMBER GAME "NON - LIVE" PRODUCT WIN/LOSS REPORT';
        $("#nL_NgmHeader").html(NumberGameReportManager.noneLiveReportTitle);
        var items = noneLiveReport.ReportDetails;
        NumberGameReportManager.nonLiveNGR.removeClass('hidden');
        if (items != null && items.length > 0) {
            var arrBody = [];
            var arrFooter = [];
            var numberItems = items.length;
            var rowOrder = 0;
            for (var i = 0; i < numberItems; i++) {
                arrBody.push(NumberGameReportManager.renderMainRow_LeftReport(items[i], (rowOrder % 2 == 0 ? "evenrow" : "")));
                rowOrder++;
            }
            arrBody.push(NumberGameReportManager.renderEmptyRow_LeftReport());
            arrFooter.push(NumberGameReportManager.renderTotalRow_LeftReport(noneLiveReport.ReportTotal.NormalTotal));

            var bodyHtml = arrBody.join('');
            bodyHtml = StringFormatter.removeSpaceIE9(bodyHtml);
            NumberGameReportManager.nonLiveNGR_TB.find("tr:not(.header)").remove();
            NumberGameReportManager.nonLiveNGR_TB.append(bodyHtml);

            var footerHtml = arrFooter.join('');
            footerHtml = StringFormatter.removeSpaceIE9(footerHtml);
            NumberGameReportManager.nonLiveNGR_TB.append(footerHtml);
            NumberGameReportManager.nonLiveNGR.removeClass('hidden');
        }
        else {
            NumberGameReportManager.renderNoData_NoneLiveReport();
        }
    },
    renderLiveReportDWL2h: function (liveReport, wldate) {
        NumberGameReportManager.liveReportTitle = 'NUMBER GAME "LIVE" PRODUCT WIN/LOSS REPORT';
        $("#l_NgmHeader").html(NumberGameReportManager.liveReportTitle);
        var items = liveReport.ReportDetails;
        NumberGameReportManager.liveNGR.removeClass('hidden');
        if (items != null && items.length > 0) {
            var arrBody = [];
            var arrFooter = [];
            var numberItems = items.length;
            var rowOrder = 0;
            for (var i = 0; i < numberItems; i++) {
                arrBody.push(NumberGameReportManager.renderMainRow_LeftReport(items[i], (rowOrder % 2 == 0 ? "evenrow" : "")));
                rowOrder++;
            }
            arrBody.push(NumberGameReportManager.renderEmptyRow_LeftReport());
            arrFooter.push(NumberGameReportManager.renderTotalRow_LeftReport(liveReport.ReportTotal.NormalTotal));

            var bodyHtml = arrBody.join('');
            bodyHtml = StringFormatter.removeSpaceIE9(bodyHtml);
            NumberGameReportManager.liveNGR_TB.find("tr:not(.header)").remove();
            NumberGameReportManager.liveNGR_TB.append(bodyHtml);

            var footerHtml = arrFooter.join('');
            footerHtml = StringFormatter.removeSpaceIE9(footerHtml);
            NumberGameReportManager.liveNGR_TB.append(footerHtml);
            NumberGameReportManager.liveNGR.removeClass('hidden');
        }
        else {
            NumberGameReportManager.renderNoData_LiveReport();
        }
    },
    renderNoData_NoneLiveReport: function () {
        NumberGameReportManager.nonLiveNGR_TB.find("tr:not(.header)").remove();
        NumberGameReportManager.nonLiveNGR_TB.append('<tr id="noinfo"><td id="tdNoInfomation" colspan="7" class="b c" style="" >There is no information available.</td></tr>');
        NumberGameReportManager.nonLiveNGR.removeClass('hidden');
    },
    renderNoData_LiveReport: function () {
        NumberGameReportManager.liveNGR_TB.find("tr:not(.header)").remove();
        NumberGameReportManager.liveNGR_TB.append('<tr id="noinfo"><td id="tdNoInfomation" colspan="7" class="b c" style="" >There is no information available.</td></tr>');
        NumberGameReportManager.liveNGR.removeClass('hidden');
    },
    renderMainRow_LeftReport: function (row, cssClass) {
        var html = '';
        html = '<tr class="' + cssClass + '">\
                <td class="l"><span class="b" style="">' + NumberGameReportManager.getReportLink(row) + '</span></td>\
                <td class="r"><span class="' + (row.WinLoss.toFixed(2) > 0 ? 'red' : '') + '" style="">' + StringFormatter.formatNumber(row.WinLoss) + '</span></td>\
                <td class="r"><span class="" style="">' + StringFormatter.formatNumber(row.Position) + '</span></td>\
                <td class="r"><span class="" style="">' + StringFormatter.formatNumber(row.Commission) + '</span></td>\
                <td class="r"><span class="' + (row.LineTotal.toFixed(2) > 0 ? 'red' : '') + '" style="">' + StringFormatter.formatNumber(row.LineTotal) + '</span></td>\
                <td class="r"><span class="" style="">' + StringFormatter.formatNumber(row.Turnover) + '</span></td>\
                <td class="r"><span class="' + (row.Turnover.toFixed(2) != 0 && row.Margin.toFixed(2) > 0 ? 'red' : '') + '" style="">' + (row.Turnover.toFixed(2) != 0 ? StringFormatter.formatNumber(row.Margin) + '%' : 'N/A') + '</span></td>\
            </tr>';
        return html;
    },
    renderTotalRow_LeftReport: function (row) {
        var html = '';
        html = '<tr>\
                <td class="r"><span class="b" style="">' + row.Name + '</span></td>\
                <td class="r"><span class="' + (row.WinLoss.toFixed(2) > 0 ? 'red' : '') + '" style="font-weight: normal;">' + StringFormatter.formatNumber(row.WinLoss) + '</span></td>\
                <td class="r"><span class="" style="font-weight: normal;">' + StringFormatter.formatNumber(row.Position) + '</span></td>\
                <td class="r"><span class="" style="font-weight: normal;">' + StringFormatter.formatNumber(row.Commission) + '</span></td>\
                <td class="r" style="background-color:#B5C4D0"><span class="b ' + (row.LineTotal.toFixed(2) > 0 ? 'red' : '') + '" style="">' + StringFormatter.formatNumber(row.LineTotal) + '</span></td>\
                <td class="r"><span class="b" style="">' + StringFormatter.formatNumber(row.Turnover) + '</span></td>\
                <td class="r"><span class="' + (row.Turnover.toFixed(2) != 0 && row.Margin.toFixed(2) > 0 ? 'red' : '') + '" style="font-weight: normal;">' + (row.Turnover.toFixed(2) != 0 ? StringFormatter.formatNumber(row.Margin) + '%' : 'N/A') + '</span></td>\
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
        var evt = "NumberGameReportManager.openReportDetails('" + normalWLItem.Id + "','" +
                                                        normalWLItem.Name + "','" +
                                                        NumberGameReportManager.sportTypeId + "','" +
                                                        NumberGameReportManager.reportDate + "','" +
                                                        normalWLItem.IsLive + "','" +
                                                        normalWLItem.Hedge + "'); return;";
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
    openReportDetails: function (bettypeId, bettypeName, sporttypeId, reportDate, isLive, hedge) {
        if (!isAllowReadNumberGameReport) {
            return;
        }
        var isHedge = (hedge && hedge.indexOf("(Hedge)") != -1);
        var data = {
            param:
            {
                ReportDate: reportDate,
                SportType: NumberGameReportManager.sportTypeId,
                LiveIndicator: isLive,
                BetType: bettypeId,
                IsHedge: isHedge,
                IsLive: (isLive == 'true' ? 1 : 0)
            }
        };
        try { parent.BlockUI(); } catch (e) { }
        DailyWinLoss2hManager.disableReportButton();
        NumberGameReportManager.reportDateForDetail = reportDate;
        NumberGameReportManager.bettype = bettypeId;
        NumberGameReportManager.bettypename = bettypeName;
        NumberGameReportManager.isLive = (isLive == 'true' ? 1 : 0);
        NumberGameReportManager.isHedge = isHedge;

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
                NumberGameReportManager.reportDetailNGR.addClass('hidden');
                $("#gridSplitter_NGR").removeClass('hidden');
                NumberGameReportManager.rightDetailReport.removeClass("hidden");
                NumberGameReportManager.renderReportDetailsDWL2h(rs, reportDate, bettypeId, bettypeName, isLive, isHedge);
            },
            error: function () {
                NumberGameReportManager.reportDetailTitle = 'NUMBER GAME WIN/LOSS REPORT';
                NumberGameReportManager.reportDetailPath = "Main>NumberGame>";
                $("#RDt_NGRHeader_Path").html(NumberGameReportManager.reportDetailPath);
                NumberGameReportManager.liveNGR.removeClass('hidden');
                NumberGameReportManager.renderNoData_ReportDetails();
            },
            complete: function () {
                $("#numberGameReport").css("width", "1500px");
                var totalWidth = $("#numberGameReportTable").width() + 10;
                $("#numberGameReport").css("width", totalWidth + "px");
                DailyWinLoss2hManager.enableReportButton();
                try { parent.jQuery.unblockUI(); } catch (e) { }
            }
        });
    },
    renderReportDetailsDWL2h: function (reportDetail, wldate, bettypeId, bettypeName, isLive, isHedge) {
        NumberGameReportManager.reportDetailTitle = 'NUMBER GAME WIN/LOSS REPORT';
        NumberGameReportManager.reportDetailPath = "Main>NumberGame>" + bettypeName;
        $("#RDt_NGRHeader_Path").html(NumberGameReportManager.reportDetailPath);
        NumberGameReportManager.liveNGR.removeClass('hidden');
        if (reportDetail != null && reportDetail.ReportDetails.length > 0) {
            var arrBody = [];
            var arrFooter = [];
            var items = reportDetail.ReportDetails;
            var numberItems = items.length;
            for (var i = 0; i < numberItems; i++) {
                arrBody.push(NumberGameReportManager.renderDailyWLRowSportDetails(items[i], i));
            }
            arrFooter.push(NumberGameReportManager.renderTotalRow_RightReport(reportDetail.ReportTotal.NormalTotal, isLive, bettypeName, isHedge));

            var bodyHtml = arrBody.join('');
            bodyHtml = StringFormatter.removeSpaceIE9(bodyHtml);
            NumberGameReportManager.reportDetailNGR_TB.find("tr:not(.header)").remove();
            NumberGameReportManager.reportDetailNGR_TB.append(bodyHtml);

            var footerHtml = arrFooter.join('');
            footerHtml = StringFormatter.removeSpaceIE9(footerHtml);
            NumberGameReportManager.reportDetailNGR_TB.append(footerHtml);
            NumberGameReportManager.reportDetailNGR.removeClass('hidden');

        }
        else {
            NumberGameReportManager.renderNoData_ReportDetails();
        }
    },
    renderDailyWLRowSportDetails: function (dwlRow, index) {
        var html = "";
        switch (dwlRow.RowTypeDWL2h.Id) {
            case 0:
                html = NumberGameReportManager.renderMainRow_RightReport(dwlRow, index);
                break;
            case 3:
                html = NumberGameReportManager.renderBettypeTotalRow_RightReport(dwlRow);
                break;
            case 9:
                html = NumberGameReportManager.emptyRowHtml_RightReport(dwlRow);
                break;
            case 10:
                html = NumberGameReportManager.renderLeagueHeadLine(dwlRow);
                break;
        }
        return html;
    },
    emptyRowHtml_RightReport: function (dwlRow) {
        var html = '<tr>\
                        <td colspan ="7" class ="c" style="height:18px;"></td>\
                    </tr>';
        return html;
    },
    renderLeagueHeadLine: function (dwlRow) {
        var html = '';
        html = '<tr>\
                    <td style="">\
                        <div></div></td>\
                    <td colspan = "6" class="l" style=""><span class="b longtext-wrapper">' + dwlRow.Name + '</span></td>\
                </tr>';
        return html;
    },
    renderMainRow_RightReport: function (row, index) {
        var html = '';
        html = '<tr class="' + (row.RowIndex % 2 == 0 ? "evenrow" : "") + '">\
                    <td style="word-wrap: break-word;"><span class="c" style="">' + row.MatchInfo.Name + '</span></td>\
                    <td class="r"><span class="' + (row.WinLoss.toFixed(2) > 0 ? 'red' : '') + '" style="">' + StringFormatter.formatNumber(row.WinLoss) + '</span></td>\
                    <td class="r"><span class="" style="">' + StringFormatter.formatNumber(row.Position) + '</span></td>\
                    <td class="r"><span class="" style="">' + StringFormatter.formatNumber(row.Commission) + '</span></td>\
                    <td class="r"><span class="' + (row.LineTotal.toFixed(2) > 0 ? 'red' : '') + '" style="">' + StringFormatter.formatNumber(row.LineTotal) + '</span></td>\
                    <td class="r"><span class="" style="">' + StringFormatter.formatNumber(row.Turnover) + '</span></td>\
                    <td class="r"><span class="' + (row.Turnover.toFixed(2) != 0 && row.Margin.toFixed(2) > 0 ? 'red' : '') + '" style="">' + (row.Turnover.toFixed(2) != 0 ? StringFormatter.formatNumber(row.Margin) + '%' : 'N/A') + '</span></td>\
                </tr>';
        return html;
    },
    renderBettypeTotalRow_RightReport: function (row) {
        var html = '';
        html = '<tr>\
                <td class="tdtitle r" bgcolor="#888888" colspan = "1" ><span class="b" style=""></span></td>\
                <td class="r"><span class="' + (row.WinLoss.toFixed(2) > 0 ? 'red' : '') + '" style="font-weight: normal;">' + StringFormatter.formatNumber(row.WinLoss) + '</span></td>\
                <td class="r"><span class="" style="font-weight: normal;">' + StringFormatter.formatNumber(row.Position) + '</span></td>\
                <td class="r"><span class="" style="font-weight: normal;">' + StringFormatter.formatNumber(row.Commission) + '</span></td>\
                <td class="r"><span class="' + (row.LineTotal.toFixed(2) > 0 ? 'red' : '') + '" style="">' + StringFormatter.formatNumber(row.LineTotal) + '</span></td>\
                <td class="r"><span class="" style="">' + StringFormatter.formatNumber(row.Turnover) + '</span></td>\
                <td class="r"><span class="' + (row.Turnover.toFixed(2) != 0 && row.Margin.toFixed(2) > 0 ? 'red' : '') + '" style="font-weight: normal;">' + (row.Turnover.toFixed(2) != 0 ? StringFormatter.formatNumber(row.Margin) + '%' : 'N/A') + '</span></td>\
            </tr>';
        return html;
    },
    renderTotalRow_RightReport: function (row, isLive, bettypeName, isHedge) {
        var html = '';
        var totalTitle = (isLive == "true" ? "<font color='red'>Live!</font>" : "") + " " +
                        bettypeName + (isHedge ? " (Hedge) " : "") + " Total:";
        html = '<tr>\
                <td class="r" colspan = "1"><span class="b longtext-wrapper" style="">' + totalTitle + '</span></td>\
                <td class="r"><span class="' + (row.WinLoss.toFixed(2) > 0 ? 'red' : '') + '" style="font-weight: normal;">' + StringFormatter.formatNumber(row.WinLoss) + '</span></td>\
                <td class="r"><span class="" style="font-weight: normal;">' + StringFormatter.formatNumber(row.Position) + '</span></td>\
                <td class="r"><span class="" style="font-weight: normal;">' + StringFormatter.formatNumber(row.Commission) + '</span></td>\
                <td class="r"><span class="' + (row.LineTotal.toFixed(2) > 0 ? 'red' : '') + '" style="font-weight: normal;">' + StringFormatter.formatNumber(row.LineTotal) + '</span></td>\
                <td class="r"><span class="" style="font-weight: normal;">' + StringFormatter.formatNumber(row.Turnover) + '</span></td>\
                <td class="r"><span class="' + (row.Turnover.toFixed(2) != 0 && row.Margin.toFixed(2) > 0 ? 'red' : '') + '" style="font-weight: normal;">' + (row.Turnover.toFixed(2) != 0 ? StringFormatter.formatNumber(row.Margin) + '%' : 'N/A') + '</span></td>\
            </tr>';
        return html;
    },
    renderNoData_ReportDetails: function () {
        NumberGameReportManager.reportDetailNGR_TB.find("tr:not(.header)").remove();
        NumberGameReportManager.reportDetailNGR_TB.append('<tr id="noinfo"><td id="tdNoInfomation" colspan="10" class="b c" style="" >There is no information available.</td></tr>');
        NumberGameReportManager.reportDetailNGR.removeClass('hidden');
    },
    exportToExcel: function () {
        if (!isAllowReadNumberGameReport) {
            return;
        }
        var url = 'DailyWinLoss2h?ExportType=NumberGameMain' + '&ReportDate=' +
                    NumberGameReportManager.reportDate;
        $('#frm').attr('src', url);
    },
    exportReportDetail: function () {
        if (!isAllowReadNumberGameReport) {
            return;
        }
        var url = 'DailyWinLoss2h?ExportType=NumberGameDetails' +
                    '&ReportDate=' + NumberGameReportManager.reportDateForDetail +
                    '&SportType=' + NumberGameReportManager.sportTypeId +
                    '&BetType=' + NumberGameReportManager.bettype +
                    '&BetTypeName=' + NumberGameReportManager.bettypename +
                    '&IsLive=' + NumberGameReportManager.isLive +
                    '&IsHedge=' + NumberGameReportManager.isHedge;
        $('#frm').attr('src', url);
    }
};

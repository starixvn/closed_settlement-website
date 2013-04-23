


var CasinoReportManager = {
    reportDate: null,
    sportTypeId: null,
    sportTypeName: null,
    bettype: null,
    bettypename: null,
    path: "Casino",
    backToMainMenuText: "Back to Main Menu",
    isFirstLoad: true,
    title: "",
    noneLiveReportTitle: "",
    liveReportTitle: "",
    reportDetailTitle: "",
    reportDetailPath: "",
    initGrid: function (sportId, wldate) {
        CasinoReportManager.sportTypeId = sportId;
        CasinoReportManager.reportDate = wldate;

        $("#btnReport").unbind("click");
        $("#btnExport").unbind("click");
        $("#btnReport").click(function () {
            var reportDate = $("#txtfromdate").val();
            CasinoReportManager.viewReport(reportDate);
        });
        $("#btnExport").click(function () {
            CasinoReportManager.exportToExcel();
        });

        $('#spMoreElement').empty();
        var strtoday = '<input type="button" onclick="CasinoReportManager.viewReportByDate(1)" id="btnToday" name="Today" class="btn"/>';
        var stryesterday = '<input type="button" onclick="CasinoReportManager.viewReportByDate(2)" id="btnYesterday" name="Yesterday" class="btn"/>';
        $('#spMoreElement').append(strtoday);
        $('#spMoreElement').append(stryesterday);
        $('#btnToday').val(today);
        $('#btnYesterday').val(yesterday);

        var fullpath = "<b><a onclick=\"WL2hShowHideDivManager.showMainReportFromCSR('" + sportId +
                         "','" + wldate + "')\"> Main </a> > " + CasinoReportManager.path + "</b>";
        $("#trPath").html(fullpath);

        $("#backToMainMenu_CSR").html(CasinoReportManager.backToMainMenuText);
        $("#backToMainMenu_CSR").unbind("click");
        $("#backToMainMenu_CSR").click(function () {
            WL2hShowHideDivManager.showMainReportFromCSR(sportId, wldate);
        });

        $("#btnExport_NgmDetail").unbind("click");
        $("#btnExport_NgmDetail").click(function () {
            CasinoReportManager.exportReportDetail();
        });

        CasinoReportManager.initReportTable();
        $("#dailyWLReportDiv").removeClass("hidden");
        try { parent.jQuery.unblockUI(); } catch (e) { }
        CasinoReportManager.viewReport(wldate);
    },
    initReportTable: function () {
        CasinoReportManager.reportView = $("#casinoReport");

        CasinoReportManager.nonLiveCSR = $("#nonLive_CasinoReport");
        CasinoReportManager.nonLiveCSR_TB = $("#tblContent_NlCsn");

        CasinoReportManager.rightDetailReport = $("#rightDetails_CSR");
        CasinoReportManager.reportDetailCSR = $("#casinoReportDetails");
        CasinoReportManager.reportDetailCSR_TB = $("#tblContent_RDtCsn tbody");
        CasinoReportManager.reportDetailCSR_TF = $("#tblFooter_RDtCsn");
    },
    viewReport: function (wldate) {
        CasinoReportManager.getLivetypeSportDetail(wldate);
    },
    viewReportByDate: function (date) {
        switch (date) {
            case 1:
                $("#txtfromdate").datepicker("setDate", new Date($('#btnToday').val()));
                CasinoReportManager.getLivetypeSportDetail($('#btnToday').val());
                break;
            case 2:
                $("#txtfromdate").datepicker("setDate", new Date($('#btnYesterday').val()));
                CasinoReportManager.getLivetypeSportDetail($('#btnYesterday').val());
                break;
            default:
                break;
        }
    },
    getLivetypeSportDetail: function (wldate) {
        if (!isAllowReadCasinoReport) {
            return;
        }
        CasinoReportManager.reportDate = wldate;
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
            url: window.rootUrl + "DailyWinLoss2h/SelCasinoReport",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: JSON.stringify(data),
            success: function (rs) {
                if ($("#InformationMssg").html() != "") {
                    $("#InformationMssg").hide();
                    $("#InformationMssg").empty();
                }
                CasinoReportManager.renderDailyWLReport(rs, wldate);
            },
            error: function () {
                CasinoReportManager.noneLiveReportTitle = 'CASINO "NON - LIVE" PRODUCT WIN/LOSS REPORT';
                $("#nL_CsnHeader").html(CasinoReportManager.noneLiveReportTitle);
                CasinoReportManager.nonLiveCSR.removeClass('hidden');
                CasinoReportManager.renderNoData_NoneLiveReport();
            },
            complete: function () {
                DailyWinLoss2hManager.enableReportButton();
                try { parent.jQuery.unblockUI(); } catch (e) { }
            }
        });
    },
    renderDailyWLReport: function (dailyWLResult, wldate) {
        if (dailyWLResult) {
            CasinoReportManager.nonLiveCSR.addClass('hidden');
            $("#gridSplitterCSR").addClass('hidden');
            CasinoReportManager.reportView.removeClass('hidden');
            CasinoReportManager.renderNoneLiveReportDWL2h(dailyWLResult.NoneLiveReport, wldate);
        }
    },
    renderNoneLiveReportDWL2h: function (noneLiveReport, wldate) {
        CasinoReportManager.noneLiveReportTitle = 'CASINO "NON - LIVE" PRODUCT WIN/LOSS REPORT';
        $("#nL_CsnHeader").html(CasinoReportManager.noneLiveReportTitle);
        var items = noneLiveReport.ReportDetails;
        CasinoReportManager.nonLiveCSR.removeClass('hidden');
        if (items != null && items.length > 0) {
            var arrBody = [];
            var arrFooter = [];
            var numberItems = items.length;
            var rowOrder = 0;
            for (var i = 0; i < numberItems; i++) {
                arrBody.push(CasinoReportManager.renderMainRow_LeftReport(items[i], (rowOrder % 2 == 0 ? "evenrow" : "")));
                rowOrder++;
            }
            arrBody.push(CasinoReportManager.renderEmptyRow_LeftReport());
            arrFooter.push(CasinoReportManager.renderTotalRow_LeftReport(noneLiveReport.ReportTotal.NormalTotal));

            var bodyHtml = arrBody.join('');
            bodyHtml = StringFormatter.removeSpaceIE9(bodyHtml);
            CasinoReportManager.nonLiveCSR_TB.find("tr:not(.header)").remove();
            CasinoReportManager.nonLiveCSR_TB.append(bodyHtml);

            var footerHtml = arrFooter.join('');
            footerHtml = StringFormatter.removeSpaceIE9(footerHtml);
            CasinoReportManager.nonLiveCSR_TB.append(footerHtml);
            CasinoReportManager.nonLiveCSR.removeClass('hidden');

        }
        else {
            CasinoReportManager.renderNoData_NoneLiveReport();
        }

    },
    renderMainRow_LeftReport: function (row, cssClass) {
        var html = '';
        html = '<tr class="' + cssClass + '">\
                <td><div class="l b col160" style="">' + CasinoReportManager.getReportLink(row) + '</div></td>\
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
                <td><div class="r col80 ' + (row.Margin.toFixed(2) > 0 ? 'red' : '') + '" style="font-weight: normal;">' + (row.Turnover.toFixed(2) != 0 ? StringFormatter.formatNumber(row.Margin) + '%' : 'N/A') + '</div></td>\
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
        CasinoReportManager.nonLiveCSR_TB.find("tr:not(.header)").remove();
        CasinoReportManager.nonLiveCSR_TB.append('<tr id="noinfo"><td id="tdNoInfomation" colspan="7" class="b c" style="" >There is no information available.</td></tr>');
        CasinoReportManager.nonLiveCSR.removeClass('hidden');
    },
    getReportLink: function (normalWLItem) {
        var evt = "CasinoReportManager.openReportDetails('" + normalWLItem.Id + "','" +
                                                        normalWLItem.Name + "','" +
                                                        CasinoReportManager.sportTypeId + "','" +
                                                        CasinoReportManager.reportDate + "','" +
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
        if (!isAllowReadCasinoReport) {
            return;
        }
        return;
        var data = {
            param:
            {
                ReportDate: reportDate,
                SportType: CasinoReportManager.sportTypeId,
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
                CasinoReportManager.reportDetailCSR.addClass('hidden');
                $("#gridSplitterCSR").removeClass("hidden");
                CasinoReportManager.rightDetailReport.removeClass("hidden");
                CasinoReportManager.renderReportDetailsDWL2h(rs, reportDate, bettypeId, bettypeName, isLive);
            },
            error: function () {
                CasinoReportManager.reportDetailTitle = 'CASINO WIN/LOSS REPORT';
                $("#RDt_NGRHeader").html(CasinoReportManager.liveReportTitle);
                CasinoReportManager.reportDetailPath = "Main>Casino>";
                $("#RDt_NGRHeader_Path").html(CasinoReportManager.reportDetailPath);
                CasinoReportManager.liveCSR.removeClass('hidden');
                CasinoReportManager.renderNoData_ReportDetails();
            },
            complete: function () {
                DailyWinLoss2hManager.enableReportButton();
                try { parent.jQuery.unblockUI(); } catch (e) { }
            }
        });
    },
    renderReportDetailsDWL2h: function (reportDetail, wldate, bettypeId, bettypeName, isLive) {
        CasinoReportManager.reportDetailTitle = 'CASINO WIN/LOSS REPORT';
        $("#RDt_NGRHeader").html(CasinoReportManager.liveReportTitle);
        CasinoReportManager.reportDetailPath = "Main>Casino>" + bettypeName;
        $("#RDt_NGRHeader_Path").html(CasinoReportManager.reportDetailPath);
        var items = reportDetail.ReportDetails;
        CasinoReportManager.liveCSR.removeClass('hidden');
        if (items != null && items.length > 0) {
            var arrBody = [];
            var arrFooter = [];
            var numberItems = items.length;
            for (var i = 0; i < numberItems; i++) {
                arrBody.push(CasinoReportManager.renderDailyWLRowSportDetails(items[i], i));
            }
            arrFooter.push(CasinoReportManager.renderTotalRow_RightReport(reportDetail.ReportTotal.NormalTotal, isLive, bettypeName));

            var bodyHtml = arrBody.join('');
            bodyHtml = StringFormatter.removeSpaceIE9(bodyHtml);
            CasinoReportManager.reportDetailCSR_TB.html(bodyHtml);

            var footerHtml = arrFooter.join('');
            footerHtml = StringFormatter.removeSpaceIE9(footerHtml);
            CasinoReportManager.reportDetailCSR_TF.html(footerHtml);
            CasinoReportManager.reportDetailCSR_TF.removeClass('hidden');
            CasinoReportManager.reportDetailCSR.removeClass('hidden').reRenderScroller(624, 500, true);

        }
        else {
            CasinoReportManager.renderNoData_ReportDetails();
        }
    },
    renderDailyWLRowSportDetails: function (dwlRow, index) {
        var html = "";
        switch (dwlRow.RowTypeDWL2h.Id) {
            case 0:
                html = CasinoReportManager.renderMainRow_RightReport(dwlRow, index);
                break;
            case 3:
                html = CasinoReportManager.renderBettypeTotalRow_RightReport(dwlRow);
                break;
            case 9:
                html = CasinoReportManager.emptyRowHtml_RightReport(dwlRow);
                break;
            case 10:
                html = CasinoReportManager.renderLeagueHeadLine(dwlRow);
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
    renderMainRow_RightReport: function (row, cssClass) {
        var html = '';
        var eventDetail = "";

        if (CasinoReportManager.bettype == 10 || CasinoReportManager.sportTypeId == 201) {
            eventDetail = '<td colspan = "3"><div class="c longtext-wrapper" style="width: 167px;">' + "" + '</div></td>';
        }
        else {
            eventDetail = '<td><div class="r longtext-wrapper" style="width: 70px;">' + "" + '</div></td>\
                        <td class="c" style="width: 31px;">-vs-</td>\
                        <td><div class="l longtext-wrapper" style="width: 70px;">' + "" + '</div></td>';
        }

        html = '<tr class="' + cssClass + '">\
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
                <td><div class="r col80 ' + (row.Turnover.toFixed(2) != 0 && row.Margin.toFixed(2) > 0 ? 'red' : '') + '" style="font-weight: normal;">' + (row.Turnover.toFixed(2) != 0 ? StringFormatter.formatNumber(row.Margin) + '%' : 'N/A') + '</div></td>\
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
                <td><div class="r col80 ' + (row.Turnover.toFixed(2) != 0 && row.Margin.toFixed(2) > 0 ? 'red' : '') + '" style="width: 50px; font-weight: normal;">' + (row.Turnover.toFixed(2) != 0 ? StringFormatter.formatNumber(row.Margin) + '%' : 'N/A') + '</div></td>\
            </tr>';
        return html;
    },
    renderNoData_ReportDetails: function () {
        CasinoReportManager.reportDetailCSR_TB.html('<tr id="noinfo"><td id="tdNoInfomation" colspan="10" class="b c" style="width: 621px;" >There is no information available.</td></tr>');
        CasinoReportManager.reportDetailCSR_TF.addClass('hidden');
        CasinoReportManager.reportDetailCSR.removeClass('hidden').reRenderScroller(624, 500, true);
    },
    exportToExcel: function () {
        if (!isAllowReadCasinoReport) {
            return;
        }
        var url = 'DailyWinLoss2h?ExportType=CasinoMain' +
                    '&ReportDate=' + CasinoReportManager.reportDate;
        $('#frm').attr('src', url);
    },
    exportReportDetail: function () {
        if (!isAllowReadCasinoReport) {
            return;
        }
    }
};
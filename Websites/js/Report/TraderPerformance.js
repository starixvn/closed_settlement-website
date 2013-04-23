$(document).ready(function () {
    permissionManagement.viewReport();
});

var permissionManagement = {
    permissionType: "None",
    isSoccer: false,
    getPermissionType: function () {
        if (hasTraderSoccerPermission == 1) {
            if (hasTraderOtherSportsPermission == 1) {
                permissionManagement.permissionType = "All";
            }
            else {
                permissionManagement.permissionType = "Soccer";
            }
        } else {
            if (hasTraderOtherSportsPermission == 1) {
                permissionManagement.permissionType = "OtherSports";
            }
            else {
                permissionManagement.permissionType = "None";
            }
        }
    },
    viewReport: function () {
        permissionManagement.getPermissionType();
        permissionManagement.hideLink();
        traderPerformanceManager.initGrid();

        var isSoccerObj = $('#IsSoccer');
        var soccer = $('#soccer');
        var other = $('#other');

        switch (permissionManagement.permissionType) {
            case "All":
                permissionManagement.isSoccer = 1;
                permissionManagement.switchView(1);
                break;
            case "Soccer":
                permissionManagement.isSoccer = 1;
                soccer.removeClass('lnk');
                soccer.addClass('blueText');              
                traderPerformanceManager.viewReport();
                break;
            case "OtherSports":
                permissionManagement.isSoccer = 0;
                other.removeClass('lnk');
                other.addClass('blueText');
                traderPerformanceManager.viewReport();
                break;
            case "None":
                soccer.removeClass('lnk');
                other.removeClass('lnk');
                break;
        }
    },
    hideLink: function () {
        var soccer = $('#soccer');
        var permissionSeperator = $("#permissionSeperator");
        var other = $('#other');

        switch (permissionManagement.permissionType) {
            case "All":
                soccer.removeClass("hidden");
                permissionSeperator.removeClass("hidden");
                other.removeClass("hidden");
                break;
            case "Soccer":
                soccer.removeClass("hidden");
                permissionSeperator.addClass("hidden");
                other.addClass("hidden");
                break;
            case "OtherSports":
                soccer.addClass("hidden");
                permissionSeperator.addClass("hidden");
                other.removeClass("hidden");
                break;
            case "None":
                break;
        }
    },
    switchView: function (viewId) {
        var soccer = $('#soccer');
        var other = $('#other');
        switch (viewId) {
            case 0:
                soccer.addClass('lnk');
                soccer.removeClass('blueText');
                soccer.bind('click', function (e) {
                    permissionManagement.switchView(1);
                });

                other.unbind('click');
                other.removeClass('lnk');
                other.removeClass('blueText');
                permissionManagement.isSoccer = 0;
                traderPerformanceManager.viewReport();
                break;
            case 1:
                other.addClass('lnk');
                other.removeClass('blueText');
                other.bind('click', function (e) {
                    permissionManagement.switchView(0);
                });
                soccer.unbind('click');
                soccer.removeClass('lnk');
                soccer.removeClass('blueText');
                permissionManagement.isSoccer = 1;
                traderPerformanceManager.viewReport();
                break;
        }

    }
};

var traderPerformanceManager = {
    param: {
        FromDate: "",
        ToDate: "",
        IsSooccer: 0

    },
    setParam: function () {
        traderPerformanceManager.param.FromDate = $("#fdate").val();
        traderPerformanceManager.param.ToDate = $("#tdate").val();
        traderPerformanceManager.param.IsSooccer = permissionManagement.isSoccer;
    },
    initGrid: function () {
        traderPerformanceManager.initReportElement();
    },
    initReportElement: function () {
        traderPerformanceManager.gridtitleSummary = $("#gridtitleSummary");
        traderPerformanceManager.traderPerformanceReport = $("#traderPerformanceReport");
        traderPerformanceManager.traderPerformanceReportContainer = $("#traderPerformanceReport_Container");
    },
    viewReport: function () {
        try { parent.BlockUI(); } catch (e) { }

        var reportGrid = traderPerformanceManager.traderPerformanceReport;
        traderPerformanceManager.setParam();
        reportGrid.addClass("hidden");
        reportGrid.resetGrid("traderPerformanceReport_Container");
        traderPerformanceManager.gridtitleSummary.html("Trader Performance - From Date: " +
                            traderPerformanceManager.param.FromDate + " To Date: " +
                            traderPerformanceManager.param.ToDate);
        $.ajax({
            type: "POST",
            url: "../../Services/WCF/Report/TraderPerformanceService.svc/GetTraderPerformance",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: '{"fromDate":"' + traderPerformanceManager.param.FromDate + '","toDate":"' +
                        traderPerformanceManager.param.ToDate + '","isSoccer":"' +
                        traderPerformanceManager.param.IsSooccer + '"}',
            success: function (rs) {
                traderPerformanceManager.traderPerformanceReport.find(".col_header_content thead").empty().append(traderPerformanceManager.displayTraderPerformanceReportHeader(traderPerformanceManager.param.IsSooccer));
                $("#traderPerformanceReport_Container tbody").empty().append(traderPerformanceManager.displayTraderPerformanceReport(rs));
            },
            error: function () {
                traderPerformanceManager.traderPerformanceReport.find(".col_header_content thead").empty().append(traderPerformanceManager.displayTraderPerformanceReportHeader(traderPerformanceManager.param.IsSooccer));
                $("#traderPerformanceReport_Container tbody").empty().append(traderPerformanceManager.displayTraderPerformanceReportNoDetail());
            },
            complete: function () {
                reportGrid.removeClass("hidden");
                var gridOpts = {
                    tblContentId: 'traderPerformanceReport_Container',
                    manualCopyHeader: true,
                    fullWidth: true,
                    fullHeight: true,
                    gridHeight: 200,
                    gridWidth: 710,
                    freezeWidth: 180,
                    freezeHeight: 56,
                    numberFreezeCol: 3
                };
                traderPerformanceManager.traderPerformanceReport.gridScroller(gridOpts);
                try { parent.jQuery.unblockUI(); } catch (e) { }
            }
        });
    },
    displayTraderPerformanceReport: function (rs) {
        var bodyHtml = "";
        if (rs.rows != null && rs.rows.length > 0) {
            var frozenHtml = "";
            var traderPerformanceDetail = null;
            var totalRow = rs.rows.length;
            for (var i = 0; i < totalRow; i++) {
                traderPerformanceDetail = rs.rows[i];
                bodyHtml += traderPerformanceManager.renderEachRowScroll(traderPerformanceDetail, traderPerformanceManager.param.IsSooccer, i);
                frozenHtml += traderPerformanceManager.renderEachRowFrozen(i, traderPerformanceDetail);
            }
            $("#traderPerformanceReport_RowHeader tbody").empty().append(frozenHtml);
            traderPerformanceManager.displayTraderPerformanceFooter(rs.footers[0], traderPerformanceManager.param.IsSooccer);
        }
        else {
            bodyHtml = traderPerformanceManager.displayTraderPerformanceReportNoDetail();
        }
        return bodyHtml;
    },
    displayTraderPerformanceFooter: function (footer, isSooccer) {
        traderPerformanceManager.traderPerformanceReport.find(".row_footer_content thead").empty().append(traderPerformanceManager.renderTraderPerformanceReportFooterFrozen(footer));
        traderPerformanceManager.traderPerformanceReport.find(".col_footer_content thead").empty().append(traderPerformanceManager.renderTraderPerformanceReportFooterScroll(footer, isSooccer));
    },
    displayTraderPerformanceReportNoDetail: function (isSoccer) {
        var colspan = 76;
        if (isSoccer == 0) {
            colspan = 46;
        }
        var html = "";
        frozenHtml = '<tr><td style="height:17px;" colspan="3">&nbsp</td></tr>';
        $("#traderPerformanceReport_RowHeader tbody").empty().append(frozenHtml);
        html = '<tr><td style="height:17px;" colspan="3">&nbsp</td><td class="l b" style="height:17px;" colspan="' + colspan + '">There is no information available.</td></tr>';

        traderPerformanceManager.traderPerformanceReport.find(".row_footer_content thead").empty();
        traderPerformanceManager.traderPerformanceReport.find(".col_footer_content thead").empty();
 
        return html;
    },
    renderEachRowFrozen: function (i, n) {
        var row = "";
        row += '<tr class="' + ((i + 1) % 2 == 0 ? 'evenrow' : 'oddrow') + '">';
        row += '<td><div class="col30 c">' + (i + 1) + '</div></td>';
        row += '<td><div class="col100 c">' + n.UserName + '</div></td>';
        row += '<td><div class="col40 c">' + n.MatchNo + '</div></td>';
        row += '</tr>';
        return row;
    },
    renderEachRowScroll: function (n, isSoccer, i) {
        var row = "";
        row += '<tr class="' + ((i + 1) % 2 == 0 ? 'evenrow' : 'oddrow') + '">';

        row += '<td><div class="col30 c">' + (i + 1) + '</div></td>';
        row += '<td><div class="col100 c">' + n.UserName + '</div></td>';
        row += '<td><div class="col40 c">' + n.MatchNo + '</div></td>';

        row += '<td class="a3"><div class="col80 r">&nbsp;' + (parseInt(n.WinLost1st1x2) < 0 ? '<font style="color:#f11">(' + n.WinLost1st1x2 + ')</font>' : (n.WinLost1st1x2 == 0 ? '' : n.WinLost1st1x2)) + '</div></td>';
        row += '<td class="a3"><div class="col80 r">&nbsp;' + (parseInt(n.WinLost1x2) < 0 ? '<font style="color:#f11">(' + n.WinLost1x2 + ')</font>' : (n.WinLost1x2 == 0 ? '' : n.WinLost1x2)) + '</div></td>';
        row += '<td class="a3"><div class="col80 r">&nbsp;' + (parseInt(n.WinLost1stHdp) < 0 ? '<font style="color:#f11">(' + n.WinLost1stHdp + ')</font>' : (n.WinLost1stHdp == 0 ? '' : n.WinLost1stHdp)) + '</div></td>';
        row += '<td class="a3"><div class="col80 r">&nbsp;' + (parseInt(n.WinLostHdp) < 0 ? '<font style="color:#f11">(' + n.WinLostHdp + ')</font>' : (n.WinLostHdp == 0 ? '' : n.WinLostHdp)) + '</div></td>';
        row += '<td class="a3"><div class="col80 r">&nbsp;' + (parseInt(n.WinLost1stOu) < 0 ? '<font style="color:#f11">(' + n.WinLost1stOu + ')</font>' : (n.WinLost1stOu == 0 ? '' : n.WinLost1stOu)) + '</div></td>';
        row += '<td class="a3"><div class="col80 r">&nbsp;' + (parseInt(n.WinLostOu) < 0 ? '<font style="color:#f11">(' + n.WinLostOu + ')</font>' : (n.WinLostOu == 0 ? '' : n.WinLostOu)) + '</div></td>';
        if (isSoccer == 1) {
            row += '<td class="a3"><div class="col80 r">&nbsp;' + (parseInt(n.WinLost1stNoOfCorners) < 0 ? '<font style="color:#f11">(' + n.WinLost1stNoOfCorners + ')</font>' : (n.WinLost1stNoOfCorners == 0 ? '' : n.WinLost1stNoOfCorners)) + '</div></td>';
            row += '<td class="a3"><div class="col80 r">&nbsp;' + (parseInt(n.WinLostNoOfCorners) < 0 ? '<font style="color:#f11">(' + n.WinLostNoOfCorners + ')</font>' : (n.WinLostNoOfCorners == 0 ? '' : n.WinLostNoOfCorners)) + '</div></td>';
            row += '<td class="a3"><div class="col80 r">&nbsp;' + (parseInt(n.WinLost1stNoOfYellowCards) < 0 ? '<font style="color:#f11">(' + n.WinLost1stNoOfYellowCards + ')</font>' : (n.WinLost1stNoOfYellowCards == 0 ? '' : n.WinLost1stNoOfYellowCards)) + '</div></td>';
            row += '<td class="a3"><div class="col80 r">&nbsp;' + (parseInt(n.WinLostNoOfYellowCards) < 0 ? '<font style="color:#f11">(' + n.WinLostNoOfYellowCards + ')</font>' : (n.WinLostNoOfYellowCards == 0 ? '' : n.WinLostNoOfYellowCards)) + '</div></td>';
            row += '<td class="a3"><div class="col80 r">&nbsp;' + (parseInt(n.WinLost15minOu) < 0 ? '<font style="color:#f11">(' + n.WinLost15minOu + ')</font>' : (n.WinLost15minOu == 0 ? '' : n.WinLost15minOu)) + '</div></td>';
            row += '<td class="a3"><div class="col80 r">&nbsp;' + (parseInt(n.WinLostNextCorner) < 0 ? '<font style="color:#f11">(' + n.WinLostNextCorner + ')</font>' : (n.WinLostNextCorner == 0 ? '' : n.WinLostNextCorner)) + '</div></td>';
        }
        row += '<td class="a3"><div class="col80 r">&nbsp;' + (parseInt(n.WinLost1stOe) < 0 ? '<font style="color:#f11">(' + n.WinLost1stOe + ')</font>' : (n.WinLost1stOe == 0 ? '' : n.WinLost1stOe)) + '</div></td>';
        row += '<td class="a3"><div class="col80 r">&nbsp;' + (parseInt(n.WinLostOe) < 0 ? '<font style="color:#f11">(' + n.WinLostOe + ')</font>' : (n.WinLostOe == 0 ? '' : n.WinLostOe)) + '</div></td>';
        row += '<td class="a3"><div class="col80 r">&nbsp;' + (parseInt(n.WinLostOverall) < 0 ? '<font style="color:#f11">(' + n.WinLostOverall + ')</font>' : (n.WinLostOverall == 0 ? '' : n.WinLostOverall)) + '</div></td>';

        row += '<td class="a2"><div class="col80 r">&nbsp;' + (parseInt(n.TurnOver1st1x2) < 0 ? '<font style="color:#f11">(' + n.TurnOver1st1x2 + ')</font>' : (n.TurnOver1st1x2 == 0 ? '' : n.TurnOver1st1x2)) + '</div></td>';
        row += '<td class="a2"><div class="col80 r">&nbsp;' + (parseInt(n.TurnOver1x2) < 0 ? '<font style="color:#f11">(' + n.TurnOver1x2 + ')</font>' : (n.TurnOver1x2 == 0 ? '' : n.TurnOver1x2)) + '</div></td>';
        row += '<td class="a2"><div class="col80 r">&nbsp;' + (parseInt(n.TurnOver1stHdp) < 0 ? '<font style="color:#f11">(' + n.TurnOver1stHdp + ')</font>' : (n.TurnOver1stHdp == 0 ? '' : n.TurnOver1stHdp)) + '</div></td>';
        row += '<td class="a2"><div class="col80 r">&nbsp;' + (parseInt(n.TurnOverHdp) < 0 ? '<font style="color:#f11">(' + n.TurnOverHdp + ')</font>' : (n.TurnOverHdp == 0 ? '' : n.TurnOverHdp)) + '</div></td>';
        row += '<td class="a2"><div class="col80 r">&nbsp;' + (parseInt(n.TurnOver1stOu) < 0 ? '<font style="color:#f11">(' + n.TurnOver1stOu + ')</font>' : (n.TurnOver1stOu == 0 ? '' : n.TurnOver1stOu)) + '</div></td>';
        row += '<td class="a2"><div class="col80 r">&nbsp;' + (parseInt(n.TurnOverOu) < 0 ? '<font style="color:#f11">(' + n.TurnOverOu + ')</font>' : (n.TurnOverOu == 0 ? '' : n.TurnOverOu)) + '</div></td>';
        if (isSoccer == 1) {
            row += '<td class="a2"><div class="col80 r">&nbsp;' + (parseInt(n.TurnOver1stNoOfCorners) < 0 ? '<font style="color:#f11">(' + n.TurnOver1stNoOfCorners + ')</font>' : (n.TurnOver1stNoOfCorners == 0 ? '' : n.TurnOver1stNoOfCorners)) + '</div></td>';
            row += '<td class="a2"><div class="col80 r">&nbsp;' + (parseInt(n.TurnOverNoOfCorners) < 0 ? '<font style="color:#f11">(' + n.TurnOverNoOfCorners + ')</font>' : (n.TurnOverNoOfCorners == 0 ? '' : n.TurnOverNoOfCorners)) + '</div></td>';
            row += '<td class="a2"><div class="col80 r">&nbsp;' + (parseInt(n.TurnOver1stNoOfYellowCards) < 0 ? '<font style="color:#f11">(' + n.TurnOver1stNoOfYellowCards + ')</font>' : (n.TurnOver1stNoOfYellowCards == 0 ? '' : n.TurnOver1stNoOfYellowCards)) + '</div></td>';
            row += '<td class="a2"><div class="col80 r">&nbsp;' + (parseInt(n.TurnOverNoOfYellowCards) < 0 ? '<font style="color:#f11">(' + n.TurnOverNoOfYellowCards + ')</font>' : (n.TurnOverNoOfYellowCards == 0 ? '' : n.TurnOverNoOfYellowCards)) + '</div></td>';
            row += '<td class="a2"><div class="col80 r">&nbsp;' + (parseInt(n.TurnOver15minOu) < 0 ? '<font style="color:#f11">(' + n.TurnOver15minOu + ')</font>' : (n.TurnOver15minOu == 0 ? '' : n.TurnOver15minOu)) + '</div></td>';
            row += '<td class="a2"><div class="col80 r">&nbsp;' + (parseInt(n.TurnOverNextCorner) < 0 ? '<font style="color:#f11">(' + n.TurnOverNextCorner + ')</font>' : (n.TurnOverNextCorner == 0 ? '' : n.TurnOverNextCorner)) + '</div></td>';
        }
        row += '<td class="a2"><div class="col80 r">&nbsp;' + (parseInt(n.TurnOver1stOe) < 0 ? '<font style="color:#f11">(' + n.TurnOver1stOe + ')</font>' : (n.TurnOver1stOe == 0 ? '' : n.TurnOver1stOe)) + '</div></td>';
        row += '<td class="a2"><div class="col80 r">&nbsp;' + (parseInt(n.TurnOverOe) < 0 ? '<font style="color:#f11">(' + n.TurnOverOe + ')</font>' : (n.TurnOverOe == 0 ? '' : n.TurnOverOe)) + '</div></td>';
        row += '<td class="a2"><div class="col80 r">&nbsp;' + (parseInt(n.TurnOverOverall) < 0 ? '<font style="color:#f11">(' + n.TurnOverOverall + ')</font>' : (n.TurnOverOverall == 0 ? '' : n.TurnOverOverall)) + '</div></td>';

        row += '<td class="a6"><div class="col80 r">&nbsp;' + (parseInt(n.LiveBetCount1st1x2) < 0 ? '<font style="color:#f11">(' + n.LiveBetCount1st1x2 + ')</font>' : (n.LiveBetCount1st1x2 == 0 ? '' : n.LiveBetCount1st1x2)) + '</div></td>';
        row += '<td class="a6"><div class="col80 r">&nbsp;' + (parseInt(n.LiveBetCount1x2) < 0 ? '<font style="color:#f11">(' + n.LiveBetCount1x2 + ')</font>' : (n.LiveBetCount1x2 == 0 ? '' : n.LiveBetCount1x2)) + '</div></td>';
        row += '<td class="a6"><div class="col80 r">&nbsp;' + (parseInt(n.LiveBetCount1stHdp) < 0 ? '<font style="color:#f11">(' + n.LiveBetCount1stHdp + ')</font>' : (n.LiveBetCount1stHdp == 0 ? '' : n.LiveBetCount1stHdp)) + '</div></td>';
        row += '<td class="a6"><div class="col80 r">&nbsp;' + (parseInt(n.LiveBetCountHdp) < 0 ? '<font style="color:#f11">(' + n.LiveBetCountHdp + ')</font>' : (n.LiveBetCountHdp == 0 ? '' : n.LiveBetCountHdp)) + '</div></td>';
        row += '<td class="a6"><div class="col80 r">&nbsp;' + (parseInt(n.LiveBetCount1stOu) < 0 ? '<font style="color:#f11">(' + n.LiveBetCount1stOu + ')</font>' : (n.LiveBetCount1stOu == 0 ? '' : n.LiveBetCount1stOu)) + '</div></td>';
        row += '<td class="a6"><div class="col80 r">&nbsp;' + (parseInt(n.LiveBetCountOu) < 0 ? '<font style="color:#f11">(' + n.LiveBetCountOu + ')</font>' : (n.LiveBetCountOu == 0 ? '' : n.LiveBetCountOu)) + '</div></td>';
        if (isSoccer == 1) {
            row += '<td class="a6"><div class="col80 r">&nbsp;' + (parseInt(n.LiveBetCount1stNoOfCorners) < 0 ? '<font style="color:#f11">(' + n.LiveBetCount1stNoOfCorners + ')</font>' : (n.LiveBetCount1stNoOfCorners == 0 ? '' : n.LiveBetCount1stNoOfCorners)) + '</div></td>';
            row += '<td class="a6"><div class="col80 r">&nbsp;' + (parseInt(n.LiveBetCountNoOfCorners) < 0 ? '<font style="color:#f11">(' + n.LiveBetCountNoOfCorners + ')</font>' : (n.LiveBetCountNoOfCorners == 0 ? '' : n.LiveBetCountNoOfCorners)) + '</div></td>';
            row += '<td class="a6"><div class="col80 r">&nbsp;' + (parseInt(n.LiveBetCount1stNoOfYellowCards) < 0 ? '<font style="color:#f11">(' + n.LiveBetCount1stNoOfYellowCards + ')</font>' : (n.LiveBetCount1stNoOfYellowCards == 0 ? '' : n.LiveBetCount1stNoOfYellowCards)) + '</div></td>';
            row += '<td class="a6"><div class="col80 r">&nbsp;' + (parseInt(n.LiveBetCountNoOfYellowCards) < 0 ? '<font style="color:#f11">(' + n.LiveBetCountNoOfYellowCards + ')</font>' : (n.LiveBetCountNoOfYellowCards == 0 ? '' : n.LiveBetCountNoOfYellowCards)) + '</div></td>';
            row += '<td class="a6"><div class="col80 r">&nbsp;' + (parseInt(n.LiveBetCount15minOu) < 0 ? '<font style="color:#f11">(' + n.LiveBetCount15minOu + ')</font>' : (n.LiveBetCount15minOu == 0 ? '' : n.LiveBetCount15minOu)) + '</div></td>';
            row += '<td class="a6"><div class="col80 r">&nbsp;' + (parseInt(n.LiveBetCountNextCorner) < 0 ? '<font style="color:#f11">(' + n.LiveBetCountNextCorner + ')</font>' : (n.LiveBetCountNextCorner == 0 ? '' : n.LiveBetCountNextCorner)) + '</div></td>';
        }
        row += '<td class="a6"><div class="col80 r">&nbsp;' + (parseInt(n.LiveBetCount1stOe) < 0 ? '<font style="color:#f11">(' + n.LiveBetCount1stOe + ')</font>' : (n.LiveBetCount1stOe == 0 ? '' : n.LiveBetCount1stOe)) + '</div></td>';
        row += '<td class="a6"><div class="col80 r">&nbsp;' + (parseInt(n.LiveBetCountOe) < 0 ? '<font style="color:#f11">(' + n.LiveBetCountOe + ')</font>' : (n.LiveBetCountOe == 0 ? '' : n.LiveBetCountOe)) + '</div></td>';
        row += '<td class="a6"><div class="col80 r">&nbsp;' + (parseInt(n.LiveBetCountOverall) < 0 ? '<font style="color:#f11">(' + n.LiveBetCountOverall + ')</font>' : (n.LiveBetCountOverall == 0 ? '' : n.LiveBetCountOverall)) + '</div></td>';

        row += '<td class="a5"><div class="col80 r">&nbsp;' + (parseInt(n.Margin1st1x2) < 0 ? '<font style="color:#f11">(' + n.Margin1st1x2 + '%)</font>' : (n.Margin1st1x2 == 0 ? '' : n.Margin1st1x2 + '%')) + '</div></td>';
        row += '<td class="a5"><div class="col80 r">&nbsp;' + (parseInt(n.Margin1x2) < 0 ? '<font style="color:#f11">(' + n.Margin1x2 + '%)</font>' : (n.Margin1x2 == 0 ? '' : n.Margin1x2 + '%')) + '</div></td>';
        row += '<td class="a5"><div class="col80 r">&nbsp;' + (parseInt(n.Margin1stHdp) < 0 ? '<font style="color:#f11">(' + n.Margin1stHdp + ')</font>' : (n.Margin1stHdp == 0 ? '' : n.Margin1stHdp + '%')) + '</div></td>';
        row += '<td class="a5"><div class="col80 r">&nbsp;' + (parseInt(n.MarginHdp) < 0 ? '<font style="color:#f11">(' + n.MarginHdp + '%)</font>' : (n.MarginHdp == 0 ? '' : n.MarginHdp + '%')) + '</div></td>';
        row += '<td class="a5"><div class="col80 r">&nbsp;' + (parseInt(n.Margin1stOu) < 0 ? '<font style="color:#f11">(' + n.Margin1stOu + '%)</font>' : (n.Margin1stOu == 0 ? '' : n.Margin1stOu + '%')) + '</div></td>';
        row += '<td class="a5"><div class="col80 r">&nbsp;' + (parseInt(n.MarginOu) < 0 ? '<font style="color:#f11">(' + n.MarginOu + '%)</font>' : (n.MarginOu == 0 ? '' : n.MarginOu + '%')) + '</div></td>';
        if (isSoccer == 1) {
            row += '<td class="a5"><div class="col80 r">&nbsp;' + (parseInt(n.Margin1stNoOfCorners) < 0 ? '<font style="color:#f11">(' + n.Margin1stNoOfCorners + '%)</font>' : (n.Margin1stNoOfCorners == 0 ? '' : n.Margin1stNoOfCorners + '%')) + '</div></td>';
            row += '<td class="a5"><div class="col80 r">&nbsp;' + (parseInt(n.MarginNoOfCorners) < 0 ? '<font style="color:#f11">(' + n.MarginNoOfCorners + '%)</font>' : (n.MarginNoOfCorners == 0 ? '' : n.MarginNoOfCorners + '%')) + '</div></td>';
            row += '<td class="a5"><div class="col80 r">&nbsp;' + (parseInt(n.Margin1stNoOfYellowCards) < 0 ? '<font style="color:#f11">(' + n.Margin1stNoOfYellowCards + '%)</font>' : (n.Margin1stNoOfYellowCards == 0 ? '' : n.Margin1stNoOfYellowCards + '%')) + '</div></td>';
            row += '<td class="a5"><div class="col80 r">&nbsp;' + (parseInt(n.MarginNoOfYellowCards) < 0 ? '<font style="color:#f11">(' + n.MarginNoOfYellowCards + '%)</font>' : (n.MarginNoOfYellowCards == 0 ? '' : n.MarginNoOfYellowCards + '%')) + '</div></td>';
            row += '<td class="a5"><div class="col80 r">&nbsp;' + (parseInt(n.Margin15minOu) < 0 ? '<font style="color:#f11">(' + n.Margin15minOu + '%)</font>' : (n.Margin15minOu == 0 ? '' : n.Margin15minOu + '%')) + '</div></td>';
            row += '<td class="a5"><div class="col80 r">&nbsp;' + (parseInt(n.MarginNextCorner) < 0 ? '<font style="color:#f11">(' + n.MarginNextCorner + '%)</font>' : (n.MarginNextCorner == 0 ? '' : n.MarginNextCorner + '%')) + '</div></td>';
        }
        row += '<td class="a5"><div class="col80 r">&nbsp;' + (parseInt(n.Margin1stOe) < 0 ? '<font style="color:#f11">(' + n.Margin1stOe + '%)</font>' : (n.Margin1stOe == 0 ? '' : n.Margin1stOe + '%')) + '</div></td>';
        row += '<td class="a5"><div class="col80 r">&nbsp;' + (parseInt(n.MarginOe) < 0 ? '<font style="color:#f11">(' + n.MarginOe + '%)</font>' : (n.MarginOe == 0 ? '' : n.MarginOe + '%')) + '</div></td>';
        row += '<td class="a5"><div class="col80 r">&nbsp;' + (parseInt(n.MarginOverall) < 0 ? '<font style="color:#f11">(' + n.MarginOverall + '%)</font>' : (n.MarginOverall == 0 ? '' : n.MarginOverall + '%')) + '</div></td>';


        row += '<td class="a4"><div class="col80 r">&nbsp;' + (parseInt(n.Comm1st1x2) < 0 ? '<font style="color:#f11">(' + n.Comm1st1x2 + ')</font>' : (n.Comm1st1x2 == 0 ? '' : n.Comm1st1x2)) + '</div></td>';
        row += '<td class="a4"><div class="col80 r">&nbsp;' + (parseInt(n.Comm1x2) < 0 ? '<font style="color:#f11">(' + n.Comm1x2 + ')</font>' : (n.Comm1x2 == 0 ? '' : n.Comm1x2)) + '</div></td>';
        row += '<td class="a4"><div class="col80 r">&nbsp;' + (parseInt(n.Comm1stHdp) < 0 ? '<font style="color:#f11">(' + n.Comm1stHdp + ')</font>' : (n.Comm1stHdp == 0 ? '' : n.Comm1stHdp)) + '</div></td>';
        row += '<td class="a4"><div class="col80 r">&nbsp;' + (parseInt(n.CommHdp) < 0 ? '<font style="color:#f11">(' + n.CommHdp + ')</font>' : (n.CommHdp == 0 ? '' : n.CommHdp)) + '</div></td>';
        row += '<td class="a4"><div class="col80 r">&nbsp;' + (parseInt(n.Comm1stOu) < 0 ? '<font style="color:#f11">(' + n.Comm1stOu + ')</font>' : (n.Comm1stOu == 0 ? '' : n.Comm1stOu)) + '</div></td>';
        row += '<td class="a4"><div class="col80 r">&nbsp;' + (parseInt(n.CommOu) < 0 ? '<font style="color:#f11">(' + n.CommOu + ')</font>' : (n.CommOu == 0 ? '' : n.CommOu)) + '</div></td>';
        if (isSoccer == 1) {
            row += '<td class="a4"><div class="col80 r">&nbsp;' + (parseInt(n.Comm1stNoOfCorners) < 0 ? '<font style="color:#f11">(' + n.Comm1stNoOfCorners + ')</font>' : (n.Comm1stNoOfCorners == 0 ? '' : n.Comm1stNoOfCorners)) + '</div></td>';
            row += '<td class="a4"><div class="col80 r">&nbsp;' + (parseInt(n.CommNoOfCorners) < 0 ? '<font style="color:#f11">(' + n.CommNoOfCorners + ')</font>' : (n.CommNoOfCorners == 0 ? '' : n.CommNoOfCorners)) + '</div></td>';
            row += '<td class="a4"><div class="col80 r">&nbsp;' + (parseInt(n.Comm1stNoOfYellowCards) < 0 ? '<font style="color:#f11">(' + n.Comm1stNoOfYellowCards + ')</font>' : (n.Comm1stNoOfYellowCards == 0 ? '' : n.Comm1stNoOfYellowCards)) + '</div></td>';
            row += '<td class="a4"><div class="col80 r">&nbsp;' + (parseInt(n.CommNoOfYellowCards) < 0 ? '<font style="color:#f11">(' + n.CommNoOfYellowCards + ')</font>' : (n.CommNoOfYellowCards == 0 ? '' : n.CommNoOfYellowCards)) + '</div></td>';
            row += '<td class="a4"><div class="col80 r">&nbsp;' + (parseInt(n.Comm15minOu) < 0 ? '<font style="color:#f11">(' + n.Comm15minOu + ')</font>' : (n.Comm15minOu == 0 ? '' : n.Comm15minOu)) + '</div></td>';
            row += '<td class="a4"><div class="col80 r">&nbsp;' + (parseInt(n.CommNextCorner) < 0 ? '<font style="color:#f11">(' + n.CommNextCorner + ')</font>' : (n.CommNextCorner == 0 ? '' : n.CommNextCorner)) + '</div></td>';
        }
        row += '<td class="a4"><div class="col80 r">&nbsp;' + (parseInt(n.Comm1stOe) < 0 ? '<font style="color:#f11">(' + n.Comm1stOe + ')</font>' : (n.Comm1stOe == 0 ? '' : n.Comm1stOe)) + '</div></td>';
        row += '<td class="a4"><div class="col80 r">&nbsp;' + (parseInt(n.CommOe) < 0 ? '<font style="color:#f11">(' + n.CommOe + ')</font>' : (n.CommOe == 0 ? '' : n.CommOe)) + '</div></td>';
        row += '<td class="a4"><div class="col80 r">&nbsp;' + (parseInt(n.CommOverall) < 0 ? '<font style="color:#f11">(' + n.CommOverall + ')</font>' : (n.CommOverall == 0 ? '' : n.CommOverall)) + '</div></td>';

        row += '<td><div class="col100 c">' + n.UserName + '</div></td>';
        row += '</tr>';

        return row;
    },
    displayTraderPerformanceReportHeader: function (isSoccer) {
        var colspanOfEachType = 15;
        if (isSoccer == 0) {
            colspanOfEachType = 9;
        }

        var headerHtml = "";
        //FirstRow
        headerHtml += '<tr>';
        headerHtml += '<th rowspan="2"><div class="col30 c" style="height: 61px;">&nbsp</div></th>';
        headerHtml += '<th rowspan="2"><div class="col100 c" style="height: 61px;">&nbsp</div></th>';
        headerHtml += '<th rowspan="2"><div class="col40" style="height: 61px;">&nbsp</div></th>';
        headerHtml += '<th colspan="' + colspanOfEachType + '"><div class="colHeaderSoccer c table-cell" style="height: 17px;">Winlost</div></th>';
        headerHtml += '<th colspan="' + colspanOfEachType + '"><div class="colHeaderSoccer c table-cell" style="height: 17px;">Turnover</div></th>';
        headerHtml += '<th colspan="' + colspanOfEachType + '"><div class="colHeaderSoccer c table-cell" style="height: 17px;">Live BetCount</div></th>';
        headerHtml += '<th colspan="' + colspanOfEachType + '"><div class="colHeaderSoccer c table-cell" style="height: 17px;">Margin %</div></th>';
        headerHtml += '<th colspan="' + colspanOfEachType + '"><div class="colHeaderSoccer c table-cell" style="height: 17px;">Commission</div></th>';
        headerHtml += '<th rowspan="2" align="center"><div class="col100 c" style="">Trader</div></th>';
        headerHtml += '</tr>';

        //SecondRow
        headerHtml += '<tr>';
        //Winlost
        headerHtml += '<th><div class="col80 c" style="">1st 1x2</div></th>';
        headerHtml += '<th><div class="col80 c" style="">1x2</div></th>';
        headerHtml += '<th><div class="col80 c" style="">1st HDP</div></th>';
        headerHtml += '<th><div class="col80 c" style="">HDP</div></th>';
        headerHtml += '<th><div class="col80 c" style="">1st O/U</div></th>';
        headerHtml += '<th><div class="col80 c" style="">O/U</div></th>';
        if (isSoccer == 1) {
            headerHtml += '<th><div class="col80 c" style="">1st No. of Corner</div></th>';
            headerHtml += '<th><div class="col80 c" style="">No. of Corner</div></th>';
            headerHtml += '<th><div class="col80 c" style="">1st No. of Yellow Card</div></th>';
            headerHtml += '<th><div class="col80 c" style=""> No. of Yellow Card</div></th>';
            headerHtml += '<th><div class="col80 c" style="">15mins O/U</div></th>';
            headerHtml += '<th><div class="col80 c" style="">Next Corner</div></th>';
        }
        headerHtml += '<th><div class="col80 c" style="">1st O/E</div></th>';
        headerHtml += '<th><div class="col80 c" style="">O/E</div></th>';
        headerHtml += '<th><div class="col80 c" style="">Overall</div></th>';
        //Turnover
        headerHtml += '<th><div class="col80 c" style="">1st 1x2</div></th>';
        headerHtml += '<th><div class="col80 c" style="">1x2</div></th>';
        headerHtml += '<th><div class="col80 c" style="">1st HDP</div></th>';
        headerHtml += '<th><div class="col80 c" style="">HDP</div></th>';
        headerHtml += '<th><div class="col80 c" style="">1st O/U</div></th>';
        headerHtml += '<th><div class="col80 c" style="">O/U</div></th>';
        if (isSoccer == 1) {
            headerHtml += '<th><div class="col80 c" style="">1st No. of Corner</div></th>';
            headerHtml += '<th><div class="col80 c" style="">No. of Corner</div></th>';
            headerHtml += '<th><div class="col80 c" style="">1st No. of Yellow Card</div></th>';
            headerHtml += '<th><div class="col80 c" style=""> No. of Yellow Card</div></th>';
            headerHtml += '<th><div class="col80 c" style="">15mins O/U</div></th>';
            headerHtml += '<th><div class="col80 c" style="">Next Corner</div></th>';
        }
        headerHtml += '<th><div class="col80 c" style="">1st O/E</div></th>';
        headerHtml += '<th><div class="col80 c" style="">O/E</div></th>';
        headerHtml += '<th><div class="col80 c" style=";">Overall</div></th>';
        //Live BetCount
        headerHtml += '<th><div class="col80 c" style="">1st 1x2</div></th>';
        headerHtml += '<th><div class="col80 c" style="">1x2</div></th>';
        headerHtml += '<th><div class="col80 c" style="">1st HDP</div></th>';
        headerHtml += '<th><div class="col80 c" style="">HDP</div></th>';
        headerHtml += '<th><div class="col80 c" style="">1st O/U</div></th>';
        headerHtml += '<th><div class="col80 c" style="">O/U</div></th>';
        if (isSoccer == 1) {
            headerHtml += '<th><div class="col80 c" style="">1st No. of Corner</div></th>';
            headerHtml += '<th><div class="col80 c" style="">No. of Corner</div></th>';
            headerHtml += '<th><div class="col80 c" style="">1st No. of Yellow Card</div></th>';
            headerHtml += '<th><div class="col80 c" style=""> No. of Yellow Card</div></th>';
            headerHtml += '<th><div class="col80 c" style="">15mins O/U</div></th>';
            headerHtml += '<th><div class="col80 c" style="">Next Corner</div></th>';
        }
        headerHtml += '<th><div class="col80 c" style="">1st O/E</div></th>';
        headerHtml += '<th><div class="col80 c" style="">O/E</div></th>';
        headerHtml += '<th><div class="col80 c" style=";">Overall</div></th>';
        //Margin %
        headerHtml += '<th><div class="col80 c" style="">1st 1x2</div></th>';
        headerHtml += '<th><div class="col80 c" style="">1x2</div></th>';
        headerHtml += '<th><div class="col80 c" style="">1st HDP</div></th>';
        headerHtml += '<th><div class="col80 c" style="">HDP</div></th>';
        headerHtml += '<th><div class="col80 c" style="">1st O/U</div></th>';
        headerHtml += '<th><div class="col80 c" style="">O/U</div></th>';
        if (isSoccer == 1) {
            headerHtml += '<th><div class="col80 c" style="">1st No. of Corner</div></th>';
            headerHtml += '<th><div class="col80 c" style="">No. of Corner</div></th>';
            headerHtml += '<th><div class="col80 c" style="">1st No. of Yellow Card</div></th>';
            headerHtml += '<th><div class="col80 c" style=""> No. of Yellow Card</div></th>';
            headerHtml += '<th><div class="col80 c" style="">15mins O/U</div></th>';
            headerHtml += '<th><div class="col80 c" style="">Next Corner</div></th>';
        }
        headerHtml += '<th><div class="col80 c" style="">1st O/E</div></th>';
        headerHtml += '<th><div class="col80 c" style="">O/E</div></th>';
        headerHtml += '<th><div class="col80 c" style=";">Overall</div></th>';
        //Commission
        headerHtml += '<th><div class="col80 c" style="">1st 1x2</div></th>';
        headerHtml += '<th><div class="col80 c" style="">1x2</div></th>';
        headerHtml += '<th><div class="col80 c" style="">1st HDP</div></th>';
        headerHtml += '<th><div class="col80 c" style="">HDP</div></th>';
        headerHtml += '<th><div class="col80 c" style="">1st O/U</div></th>';
        headerHtml += '<th><div class="col80 c" style="">O/U</div></th>';
        if (isSoccer == 1) {
            headerHtml += '<th><div class="col80 c" style="">1st No. of Corner</div></th>';
            headerHtml += '<th><div class="col80 c" style="">No. of Corner</div></th>';
            headerHtml += '<th><div class="col80 c" style="">1st No. of Yellow Card</div></th>';
            headerHtml += '<th><div class="col80 c" style=""> No. of Yellow Card</div></th>';
            headerHtml += '<th><div class="col80 c" style="">15mins O/U</div></th>';
            headerHtml += '<th><div class="col80 c" style="">Next Corner</div></th>';
        }
        headerHtml += '<th><div class="col80 c" style="">1st O/E</div></th>';
        headerHtml += '<th><div class="col80 c" style="">O/E</div></th>';
        headerHtml += '<th><div class="col80 c" style=";">Overall</div></th>';

        headerHtml += '</tr>';
        return headerHtml;
    },
    renderTraderPerformanceReportFooterFrozen: function (n, isSoccer) {
        var row = "";
        row += '<tr>';
        row += '<th style="border-bottom: none 0px;"><div class="col30 c">&nbsp;</div></th>';
        row += '<th style="border-bottom: none 0px;"><div class="col100 c">Total</div></th>';
        row += '<th style="border-bottom: none 0px;"><div class="col40 c">' + n.MatchNo + '</div></th>';
        row += '</tr>';

        return row;
    },
    renderTraderPerformanceReportFooterScroll: function (n, isSoccer) {
        var row = "";
        row += '<tr>';

        row += '<th style="border-bottom: none 0px;"><div class="col30 c">&nbsp;</div></td>';
        row += '<th style="border-bottom: none 0px;"><div class="col100 c">Total</div></td>';
        row += '<th style="border-bottom: none 0px;"><div class="col40 c">' + n.MatchNo + '</div></td>';

        row += '<th><div class="col80 r">&nbsp;' + (parseInt(n.WinLost1st1x2) < 0 ? '<font style="color:#f11">(' + n.WinLost1st1x2 + ')</font>' : (n.WinLost1st1x2 == 0 ? '' : n.WinLost1st1x2)) + '</div></th>';
        row += '<th><div class="col80 r">&nbsp;' + (parseInt(n.WinLost1x2) < 0 ? '<font style="color:#f11">(' + n.WinLost1x2 + ')</font>' : (n.WinLost1x2 == 0 ? '' : n.WinLost1x2)) + '</div></th>';
        row += '<th><div class="col80 r">&nbsp;' + (parseInt(n.WinLost1stHdp) < 0 ? '<font style="color:#f11">(' + n.WinLost1stHdp + ')</font>' : (n.WinLost1stHdp == 0 ? '' : n.WinLost1stHdp)) + '</div></th>';
        row += '<th><div class="col80 r">&nbsp;' + (parseInt(n.WinLostHdp) < 0 ? '<font style="color:#f11">(' + n.WinLostHdp + ')</font>' : (n.WinLostHdp == 0 ? '' : n.WinLostHdp)) + '</div></th>';
        row += '<th><div class="col80 r">&nbsp;' + (parseInt(n.WinLost1stOu) < 0 ? '<font style="color:#f11">(' + n.WinLost1stOu + ')</font>' : (n.WinLost1stOu == 0 ? '' : n.WinLost1stOu)) + '</div></th>';
        row += '<th><div class="col80 r">&nbsp;' + (parseInt(n.WinLostOu) < 0 ? '<font style="color:#f11">(' + n.WinLostOu + ')</font>' : (n.WinLostOu == 0 ? '' : n.WinLostOu)) + '</div></th>';
        if (isSoccer == 1) {
            row += '<th><div class="col80 r">&nbsp;' + (parseInt(n.WinLost1stNoOfCorners) < 0 ? '<font style="color:#f11">(' + n.WinLost1stNoOfCorners + ')</font>' : (n.WinLost1stNoOfCorners == 0 ? '' : n.WinLost1stNoOfCorners)) + '</div></th>';
            row += '<th><div class="col80 r">&nbsp;' + (parseInt(n.WinLostNoOfCorners) < 0 ? '<font style="color:#f11">(' + n.WinLostNoOfCorners + ')</font>' : (n.WinLostNoOfCorners == 0 ? '' : n.WinLostNoOfCorners)) + '</div></th>';
            row += '<th><div class="col80 r">&nbsp;' + (parseInt(n.WinLost1stNoOfYellowCards) < 0 ? '<font style="color:#f11">(' + n.WinLost1stNoOfYellowCards + ')</font>' : (n.WinLost1stNoOfYellowCards == 0 ? '' : n.WinLost1stNoOfYellowCards)) + '</div></th>';
            row += '<th><div class="col80 r">&nbsp;' + (parseInt(n.WinLostNoOfYellowCards) < 0 ? '<font style="color:#f11">(' + n.WinLostNoOfYellowCards + ')</font>' : (n.WinLostNoOfYellowCards == 0 ? '' : n.WinLostNoOfYellowCards)) + '</div></th>';
            row += '<th><div class="col80 r">&nbsp;' + (parseInt(n.WinLost15minOu) < 0 ? '<font style="color:#f11">(' + n.WinLost15minOu + ')</font>' : (n.WinLost15minOu == 0 ? '' : n.WinLost15minOu)) + '</div></th>';
            row += '<th><div class="col80 r">&nbsp;' + (parseInt(n.WinLostNextCorner) < 0 ? '<font style="color:#f11">(' + n.WinLostNextCorner + ')</font>' : (n.WinLostNextCorner == 0 ? '' : n.WinLostNextCorner)) + '</div></th>';
        }
        row += '<th><div class="col80 r">&nbsp;' + (parseInt(n.WinLost1stOe) < 0 ? '<font style="color:#f11">(' + n.WinLost1stOe + ')</font>' : (n.WinLost1stOe == 0 ? '' : n.WinLost1stOe)) + '</div></th>';
        row += '<th><div class="col80 r">&nbsp;' + (parseInt(n.WinLostOe) < 0 ? '<font style="color:#f11">(' + n.WinLostOe + ')</font>' : (n.WinLostOe == 0 ? '' : n.WinLostOe)) + '</div></th>';
        row += '<th><div class="col80 r">&nbsp;' + (parseInt(n.WinLostOverall) < 0 ? '<font style="color:#f11">(' + n.WinLostOverall + ')</font>' : (n.WinLostOverall == 0 ? '' : n.WinLostOverall)) + '</div></th>';

        row += '<th><div class="col80 r">&nbsp;' + (parseInt(n.TurnOver1st1x2) < 0 ? '<font style="color:#f11">(' + n.TurnOver1st1x2 + ')</font>' : (n.TurnOver1st1x2 == 0 ? '' : n.TurnOver1st1x2)) + '</div></th>';
        row += '<th><div class="col80 r">&nbsp;' + (parseInt(n.TurnOver1x2) < 0 ? '<font style="color:#f11">(' + n.TurnOver1x2 + ')</font>' : (n.TurnOver1x2 == 0 ? '' : n.TurnOver1x2)) + '</div></td>';
        row += '<th><div class="col80 r">&nbsp;' + (parseInt(n.TurnOver1stHdp) < 0 ? '<font style="color:#f11">(' + n.TurnOver1stHdp + ')</font>' : (n.TurnOver1stHdp == 0 ? '' : n.TurnOver1stHdp)) + '</div></th>';
        row += '<th><div class="col80 r">&nbsp;' + (parseInt(n.TurnOverHdp) < 0 ? '<font style="color:#f11">(' + n.TurnOverHdp + ')</font>' : (n.TurnOverHdp == 0 ? '' : n.TurnOverHdp)) + '</div></th>';
        row += '<th><div class="col80 r">&nbsp;' + (parseInt(n.TurnOver1stOu) < 0 ? '<font style="color:#f11">(' + n.TurnOver1stOu + ')</font>' : (n.TurnOver1stOu == 0 ? '' : n.TurnOver1stOu)) + '</div></th>';
        row += '<th><div class="col80 r">&nbsp;' + (parseInt(n.TurnOverOu) < 0 ? '<font style="color:#f11">(' + n.TurnOverOu + ')</font>' : (n.TurnOverOu == 0 ? '' : n.TurnOverOu)) + '</div></th>';
        if (isSoccer == 1) {
            row += '<th><div class="col80 r">&nbsp;' + (parseInt(n.TurnOver1stNoOfCorners) < 0 ? '<font style="color:#f11">(' + n.TurnOver1stNoOfCorners + ')</font>' : (n.TurnOver1stNoOfCorners == 0 ? '' : n.TurnOver1stNoOfCorners)) + '</div></th>';
            row += '<th><div class="col80 r">&nbsp;' + (parseInt(n.TurnOverNoOfCorners) < 0 ? '<font style="color:#f11">(' + n.TurnOverNoOfCorners + ')</font>' : (n.TurnOverNoOfCorners == 0 ? '' : n.TurnOverNoOfCorners)) + '</div></th>';
            row += '<th><div class="col80 r">&nbsp;' + (parseInt(n.TurnOver1stNoOfYellowCards) < 0 ? '<font style="color:#f11">(' + n.TurnOver1stNoOfYellowCards + ')</font>' : (n.TurnOver1stNoOfYellowCards == 0 ? '' : n.TurnOver1stNoOfYellowCards)) + '</div></th>';
            row += '<th><div class="col80 r">&nbsp;' + (parseInt(n.TurnOverNoOfYellowCards) < 0 ? '<font style="color:#f11">(' + n.TurnOverNoOfYellowCards + ')</font>' : (n.TurnOverNoOfYellowCards == 0 ? '' : n.TurnOverNoOfYellowCards)) + '</div></th>';
            row += '<th><div class="col80 r">&nbsp;' + (parseInt(n.TurnOver15minOu) < 0 ? '<font style="color:#f11">(' + n.TurnOver15minOu + ')</font>' : (n.TurnOver15minOu == 0 ? '' : n.TurnOver15minOu)) + '</div></th>';
            row += '<th><div class="col80 r">&nbsp;' + (parseInt(n.TurnOverNextCorner) < 0 ? '<font style="color:#f11">(' + n.TurnOverNextCorner + ')</font>' : (n.TurnOverNextCorner == 0 ? '' : n.TurnOverNextCorner)) + '</div></th>';
        }
        row += '<th><div class="col80 r">&nbsp;' + (parseInt(n.TurnOver1stOe) < 0 ? '<font style="color:#f11">(' + n.TurnOver1stOe + ')</font>' : (n.TurnOver1stOe == 0 ? '' : n.TurnOver1stOe)) + '</div></th>';
        row += '<th><div class="col80 r">&nbsp;' + (parseInt(n.TurnOverOe) < 0 ? '<font style="color:#f11">(' + n.TurnOverOe + ')</font>' : (n.TurnOverOe == 0 ? '' : n.TurnOverOe)) + '</div></th>';
        row += '<th><div class="col80 r">&nbsp;' + (parseInt(n.TurnOverOverall) < 0 ? '<font style="color:#f11">(' + n.TurnOverOverall + ')</font>' : (n.TurnOverOverall == 0 ? '' : n.TurnOverOverall)) + '</div></th>';

        row += '<th><div class="col80 r">&nbsp;' + (parseInt(n.LiveBetCount1st1x2) < 0 ? '<font style="color:#f11">(' + n.LiveBetCount1st1x2 + ')</font>' : (n.LiveBetCount1st1x2 == 0 ? '' : n.LiveBetCount1st1x2)) + '</div></th>';
        row += '<th><div class="col80 r">&nbsp;' + (parseInt(n.LiveBetCount1x2) < 0 ? '<font style="color:#f11">(' + n.LiveBetCount1x2 + ')</font>' : (n.LiveBetCount1x2 == 0 ? '' : n.LiveBetCount1x2)) + '</div></th>';
        row += '<th><div class="col80 r">&nbsp;' + (parseInt(n.LiveBetCount1stHdp) < 0 ? '<font style="color:#f11">(' + n.LiveBetCount1stHdp + ')</font>' : (n.LiveBetCount1stHdp == 0 ? '' : n.LiveBetCount1stHdp)) + '</div></th>';
        row += '<th><div class="col80 r">&nbsp;' + (parseInt(n.LiveBetCountHdp) < 0 ? '<font style="color:#f11">(' + n.LiveBetCountHdp + ')</font>' : (n.LiveBetCountHdp == 0 ? '' : n.LiveBetCountHdp)) + '</div></th>';
        row += '<th><div class="col80 r">&nbsp;' + (parseInt(n.LiveBetCount1stOu) < 0 ? '<font style="color:#f11">(' + n.LiveBetCount1stOu + ')</font>' : (n.LiveBetCount1stOu == 0 ? '' : n.LiveBetCount1stOu)) + '</div></th>';
        row += '<th><div class="col80 r">&nbsp;' + (parseInt(n.LiveBetCountOu) < 0 ? '<font style="color:#f11">(' + n.LiveBetCountOu + ')</font>' : (n.LiveBetCountOu == 0 ? '' : n.LiveBetCountOu)) + '</div></th>';
        if (isSoccer == 1) {
            row += '<th><div class="col80 r">&nbsp;' + (parseInt(n.LiveBetCount1stNoOfCorners) < 0 ? '<font style="color:#f11">(' + n.LiveBetCount1stNoOfCorners + ')</font>' : (n.LiveBetCount1stNoOfCorners == 0 ? '' : n.LiveBetCount1stNoOfCorners)) + '</div></th>';
            row += '<th><div class="col80 r">&nbsp;' + (parseInt(n.LiveBetCountNoOfCorners) < 0 ? '<font style="color:#f11">(' + n.LiveBetCountNoOfCorners + ')</font>' : (n.LiveBetCountNoOfCorners == 0 ? '' : n.LiveBetCountNoOfCorners)) + '</div></th>';
            row += '<th><div class="col80 r">&nbsp;' + (parseInt(n.LiveBetCount1stNoOfYellowCards) < 0 ? '<font style="color:#f11">(' + n.LiveBetCount1stNoOfYellowCards + ')</font>' : (n.LiveBetCount1stNoOfYellowCards == 0 ? '' : n.LiveBetCount1stNoOfYellowCards)) + '</div></th>';
            row += '<th><div class="col80 r">&nbsp;' + (parseInt(n.LiveBetCountNoOfYellowCards) < 0 ? '<font style="color:#f11">(' + n.LiveBetCountNoOfYellowCards + ')</font>' : (n.LiveBetCountNoOfYellowCards == 0 ? '' : n.LiveBetCountNoOfYellowCards)) + '</div></th>';
            row += '<th><div class="col80 r">&nbsp;' + (parseInt(n.LiveBetCount15minOu) < 0 ? '<font style="color:#f11">(' + n.LiveBetCount15minOu + ')</font>' : (n.LiveBetCount15minOu == 0 ? '' : n.LiveBetCount15minOu)) + '</div></th>';
            row += '<th><div class="col80 r">&nbsp;' + (parseInt(n.LiveBetCountNextCorner) < 0 ? '<font style="color:#f11">(' + n.LiveBetCountNextCorner + ')</font>' : (n.LiveBetCountNextCorner == 0 ? '' : n.LiveBetCountNextCorner)) + '</div></th>';
        }
        row += '<th><div class="col80 r">&nbsp;' + (parseInt(n.LiveBetCount1stOe) < 0 ? '<font style="color:#f11">(' + n.LiveBetCount1stOe + ')</font>' : (n.LiveBetCount1stOe == 0 ? '' : n.LiveBetCount1stOe)) + '</div></th>';
        row += '<th><div class="col80 r">&nbsp;' + (parseInt(n.LiveBetCountOe) < 0 ? '<font style="color:#f11">(' + n.LiveBetCountOe + ')</font>' : (n.LiveBetCountOe == 0 ? '' : n.LiveBetCountOe)) + '</div></th>';
        row += '<th><div class="col80 r">&nbsp;' + (parseInt(n.LiveBetCountOverall) < 0 ? '<font style="color:#f11">(' + n.LiveBetCountOverall + ')</font>' : (n.LiveBetCountOverall == 0 ? '' : n.LiveBetCountOverall)) + '</div></th>';

        row += '<th><div class="col80 r">&nbsp;' + (parseInt(n.Margin1st1x2) < 0 ? '<font style="color:#f11">(' + n.Margin1st1x2 + '%)</font>' : (n.Margin1st1x2 == 0 ? '' : n.Margin1st1x2 + '%')) + '</div></th>';
        row += '<th><div class="col80 r">&nbsp;' + (parseInt(n.Margin1x2) < 0 ? '<font style="color:#f11">(' + n.Margin1x2 + '%)</font>' : (n.Margin1x2 == 0 ? '' : n.Margin1x2 + '%')) + '</div></th>';
        row += '<th><div class="col80 r">&nbsp;' + (parseInt(n.Margin1stHdp) < 0 ? '<font style="color:#f11">(' + n.Margin1stHdp + ')</font>' : (n.Margin1stHdp == 0 ? '' : n.Margin1stHdp + '%')) + '</div></th>';
        row += '<th><div class="col80 r">&nbsp;' + (parseInt(n.MarginHdp) < 0 ? '<font style="color:#f11">(' + n.MarginHdp + '%)</font>' : (n.MarginHdp == 0 ? '' : n.MarginHdp + '%')) + '</div></th>';
        row += '<th><div class="col80 r">&nbsp;' + (parseInt(n.Margin1stOu) < 0 ? '<font style="color:#f11">(' + n.Margin1stOu + '%)</font>' : (n.Margin1stOu == 0 ? '' : n.Margin1stOu + '%')) + '</div></th>';
        row += '<th><div class="col80 r">&nbsp;' + (parseInt(n.MarginOu) < 0 ? '<font style="color:#f11">(' + n.MarginOu + '%)</font>' : (n.MarginOu == 0 ? '' : n.MarginOu + '%')) + '</div></th>';
        if (isSoccer == 1) {
            row += '<th><div class="col80 r">&nbsp;' + (parseInt(n.Margin1stNoOfCorners) < 0 ? '<font style="color:#f11">(' + n.Margin1stNoOfCorners + '%)</font>' : (n.Margin1stNoOfCorners == 0 ? '' : n.Margin1stNoOfCorners + '%')) + '</div></th>';
            row += '<th><div class="col80 r">&nbsp;' + (parseInt(n.MarginNoOfCorners) < 0 ? '<font style="color:#f11">(' + n.MarginNoOfCorners + '%)</font>' : (n.MarginNoOfCorners == 0 ? '' : n.MarginNoOfCorners + '%')) + '</div></th>';
            row += '<th><div class="col80 r">&nbsp;' + (parseInt(n.Margin1stNoOfYellowCards) < 0 ? '<font style="color:#f11">(' + n.Margin1stNoOfYellowCards + '%)</font>' : (n.Margin1stNoOfYellowCards == 0 ? '' : n.Margin1stNoOfYellowCards + '%')) + '</div></th>';
            row += '<th><div class="col80 r">&nbsp;' + (parseInt(n.MarginNoOfYellowCards) < 0 ? '<font style="color:#f11">(' + n.MarginNoOfYellowCards + '%)</font>' : (n.MarginNoOfYellowCards == 0 ? '' : n.MarginNoOfYellowCards + '%')) + '</div></th>';
            row += '<th><div class="col80 r">&nbsp;' + (parseInt(n.Margin15minOu) < 0 ? '<font style="color:#f11">(' + n.Margin15minOu + '%)</font>' : (n.Margin15minOu == 0 ? '' : n.Margin15minOu + '%')) + '</div></th>';
            row += '<th><div class="col80 r">&nbsp;' + (parseInt(n.MarginNextCorner) < 0 ? '<font style="color:#f11">(' + n.MarginNextCorner + '%)</font>' : (n.MarginNextCorner == 0 ? '' : n.MarginNextCorner + '%')) + '</div></th>';
        }
        row += '<th><div class="col80 r">&nbsp;' + (parseInt(n.Margin1stOe) < 0 ? '<font style="color:#f11">(' + n.Margin1stOe + '%)</font>' : (n.Margin1stOe == 0 ? '' : n.Margin1stOe + '%')) + '</div></th>';
        row += '<th><div class="col80 r">&nbsp;' + (parseInt(n.MarginOe) < 0 ? '<font style="color:#f11">(' + n.MarginOe + '%)</font>' : (n.MarginOe == 0 ? '' : n.MarginOe + '%')) + '</div></th>';
        row += '<th><div class="col80 r">&nbsp;' + (parseInt(n.MarginOverall) < 0 ? '<font style="color:#f11">(' + n.MarginOverall + '%)</font>' : (n.MarginOverall == 0 ? '' : n.MarginOverall + '%')) + '</div></th>';


        row += '<th><div class="col80 r">&nbsp;' + (parseInt(n.Comm1st1x2) < 0 ? '<font style="color:#f11">(' + n.Comm1st1x2 + ')</font>' : (n.Comm1st1x2 == 0 ? '' : n.Comm1st1x2)) + '</div></th>';
        row += '<th><div class="col80 r">&nbsp;' + (parseInt(n.Comm1x2) < 0 ? '<font style="color:#f11">(' + n.Comm1x2 + ')</font>' : (n.Comm1x2 == 0 ? '' : n.Comm1x2)) + '</div></th>';
        row += '<th><div class="col80 r">&nbsp;' + (parseInt(n.Comm1stHdp) < 0 ? '<font style="color:#f11">(' + n.Comm1stHdp + ')</font>' : (n.Comm1stHdp == 0 ? '' : n.Comm1stHdp)) + '</div></th>';
        row += '<th><div class="col80 r">&nbsp;' + (parseInt(n.CommHdp) < 0 ? '<font style="color:#f11">(' + n.CommHdp + ')</font>' : (n.CommHdp == 0 ? '' : n.CommHdp)) + '</div></th>';
        row += '<th><div class="col80 r">&nbsp;' + (parseInt(n.Comm1stOu) < 0 ? '<font style="color:#f11">(' + n.Comm1stOu + ')</font>' : (n.Comm1stOu == 0 ? '' : n.Comm1stOu)) + '</div></th>';
        row += '<th><div class="col80 r">&nbsp;' + (parseInt(n.CommOu) < 0 ? '<font style="color:#f11">(' + n.CommOu + ')</font>' : (n.CommOu == 0 ? '' : n.CommOu)) + '</div></th>';
        if (isSoccer == 1) {
            row += '<th><div class="col80 r">&nbsp;' + (parseInt(n.Comm1stNoOfCorners) < 0 ? '<font style="color:#f11">(' + n.Comm1stNoOfCorners + ')</font>' : (n.Comm1stNoOfCorners == 0 ? '' : n.Comm1stNoOfCorners)) + '</div></th>';
            row += '<th><div class="col80 r">&nbsp;' + (parseInt(n.CommNoOfCorners) < 0 ? '<font style="color:#f11">(' + n.CommNoOfCorners + ')</font>' : (n.CommNoOfCorners == 0 ? '' : n.CommNoOfCorners)) + '</div></th>';
            row += '<th><div class="col80 r">&nbsp;' + (parseInt(n.Comm1stNoOfYellowCards) < 0 ? '<font style="color:#f11">(' + n.Comm1stNoOfYellowCards + ')</font>' : (n.Comm1stNoOfYellowCards == 0 ? '' : n.Comm1stNoOfYellowCards)) + '</div></th>';
            row += '<th><div class="col80 r">&nbsp;' + (parseInt(n.CommNoOfYellowCards) < 0 ? '<font style="color:#f11">(' + n.CommNoOfYellowCards + ')</font>' : (n.CommNoOfYellowCards == 0 ? '' : n.CommNoOfYellowCards)) + '</div></th>';
            row += '<th><div class="col80 r">&nbsp;' + (parseInt(n.Comm15minOu) < 0 ? '<font style="color:#f11">(' + n.Comm15minOu + ')</font>' : (n.Comm15minOu == 0 ? '' : n.Comm15minOu)) + '</div></th>';
            row += '<th><div class="col80 r">&nbsp;' + (parseInt(n.CommNextCorner) < 0 ? '<font style="color:#f11">(' + n.CommNextCorner + ')</font>' : (n.CommNextCorner == 0 ? '' : n.CommNextCorner)) + '</div></th>';
        }
        row += '<th><div class="col80 r">&nbsp;' + (parseInt(n.Comm1stOe) < 0 ? '<font style="color:#f11">(' + n.Comm1stOe + ')</font>' : (n.Comm1stOe == 0 ? '' : n.Comm1stOe)) + '</div></th>';
        row += '<th><div class="col80 r">&nbsp;' + (parseInt(n.CommOe) < 0 ? '<font style="color:#f11">(' + n.CommOe + ')</font>' : (n.CommOe == 0 ? '' : n.CommOe)) + '</div></th>';
        row += '<th><div class="col80 r">&nbsp;' + (parseInt(n.CommOverall) < 0 ? '<font style="color:#f11">(' + n.CommOverall + ')</font>' : (n.CommOverall == 0 ? '' : n.CommOverall)) + '</div></th>';

        row += '<th><div class="col100 c">' + n.UserName + '</div></th>';

        row += '</tr>';

        return row;
    },
    loadTraderDetail: function () {
        var isSoccer = permissionManagement.isSoccer;
        var fromDate = $("#fdate").val();
        var toDate = $("#tdate").val();

        //var url = '../Report/TraderPerformance/TraderPerformanceDetail.aspx?fdate=' + fromDate + '&tdate=' + toDate + '&IsSoccer=' + isSoccer;
        //parent.gotoURL('centerFr', url);
        window.open('TraderPerformanceDetail.aspx?fdate=' + fromDate + '&tdate=' + toDate + '&IsSoccer=' + isSoccer, 'TraderPerformanceDetail');
    },
    export2Excel1: function () {
        var isSoccer = permissionManagement.isSoccer;
        var fromDate = $("#fdate").val();
        var toDate = $("#tdate").val();

        try { parent.BlockUI(); } catch (e) { }
        $.ajax({
            type: "POST",
            url: "../../Services/WCF/Report/TraderPerformanceService.svc/TraderPerformanceExport2Excel",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: '{"fromDate":"' + fromDate + '","toDate":"' + toDate + '","isSoccer":"' + isSoccer + '"}',
            success: function () {

            }
        });
        try { parent.jQuery.unblockUI(); } catch (e) { }
    },
    export2Excel: function () {
        var isSoccer = permissionManagement.isSoccer;
        var fromDate = $("#fdate").val();
        var toDate = $("#tdate").val();
        var params = 'type=export&fdate=' + fromDate + '&tdate=' + toDate + '&issoccer=' + isSoccer;
        $('#frm').attr('src', 'TraderPerformance.aspx?' + params);

    }
}
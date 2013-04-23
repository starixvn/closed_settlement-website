



var OtherSportsReportManager = {
    reportDate: null,
    sportTypeId: null,
    sportTypeName: null,
    bettype: null,
    bettypename: null,
    path: "Other Sports",
    backToMainMenuText: "Back to Main Menu",
    isFirstLoad: true,
    title: "",
    noneLiveReportTitle: "",
    liveReportTitle: "",
    reportDetailTitle: "",
    reportDetailPath: "",
    sportTypeIdLive: null,
    sportTypeNameLive: null,
    sportTypeIdNoneLive: null,
    sportTypeNameNoneLive: null,
    initGrid: function (sportId, wldate) {
        OtherSportsReportManager.sportTypeId = sportId;
        OtherSportsReportManager.reportDate = wldate;
        $("#btnReport").unbind("click");
        $("#btnExport").unbind("click");
        $("#btnReport").click(function () {
            var reportDate = $("#txtfromdate").val();
            OtherSportsReportManager.viewReport(reportDate);
        });
        $("#btnExport").click(function () {
            OtherSportsReportManager.exportToExcel("1");
        });
        $('#spMoreElement').empty();
        var strtoday = '<input type="button" id="btnToday" name="Today" class="btn"/>';
        var stryesterday = '<input type="button" id="btnYesterday" name="Yesterday" class="btn"/>';
        $('#spMoreElement').append(strtoday);
        $('#spMoreElement').append(stryesterday);
        $('#btnToday').val(today);
        $('#btnYesterday').val(yesterday);
        $("#btnToday").click(function () {
            OtherSportsReportManager.viewReportByDate(1);
        });
        $("#btnYesterday").click(function () {
            OtherSportsReportManager.viewReportByDate(2);
        });

        var fullpath = "<b><a onclick=\"WL2hShowHideDivManager.showMainReportFromOSR('" + sportId +
                         "','" + wldate + "')\"> Main </a> > " + OtherSportsReportManager.path + "</b>";

        $("#trPath").html(fullpath);
        $("#backToMainMenu_OSR").html(OtherSportsReportManager.backToMainMenuText);
        $("#backToMainMenu_OSR").unbind("click");
        $("#backToMainMenu_OSR").click(function () {
            WL2hShowHideDivManager.showMainReportFromOSR(sportId, wldate);
        });

        $("#dailyWLReportDiv").removeClass("hidden");

        try { parent.jQuery.unblockUI(); } catch (e) { }
        OtherSportsReportManager.viewReport(wldate);
    },
    viewReport: function (wldate) {
        OtherSportsReportManager.getcontent(wldate);
    },
    viewReportByDate: function (date) {
        switch (date) {
            case 1:
                $("#txtfromdate").datepicker("setDate", new Date($('#btnToday').val()));
                OtherSportsReportManager.getcontent($('#btnToday').val());
                break;
            case 2:
                $("#txtfromdate").datepicker("setDate", new Date($('#btnYesterday').val()));
                OtherSportsReportManager.getcontent($('#btnYesterday').val());
                break;
            default:
                break;
        }
    },
    getcontent: function (wldate) {
        if (!isAllowReadOtherSportsReport) {
            return;
        }

        OtherSportsReportManager.reportDate = wldate;
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
            url: window.rootUrl + "DailyWinLoss2hOtherSports/SelOtherSportsReport",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: JSON.stringify(data),
            success: function (rs) {
                if ($("#InformationMssg").html() != "") {
                    $("#InformationMssg").hide();
                    $("#InformationMssg").empty();
                }

                OtherSportsReportManager.renderDailyWLReport(rs, wldate);
                OtherSportsReportManager.showMainReport(true);
                $("#leftPaneOSR").removeClass("hidden");
                $("#otherSportsReport").removeClass("hidden");
            },
            error: function () {
                $("#nonliveDailyWLOtherSports").find("tr:not(.header)").remove();
                $("#nonliveDailyWLOtherSports").append('<tr><td colspan="7" class="b c">There is no information available.</td></tr>');
                OtherSportsReportManager.showNonLiveReport(true);
                $("#liveDailyWLOtherSports").find("tr:not(.header)").remove();
                $("#liveDailyWLOtherSports").append('<tr><td colspan="7" class="b c">There is no information available.</td></tr>');
                OtherSportsReportManager.showLiveReport(true);
            },
            complete: function () {
                DailyWinLoss2hManager.enableReportButton();
                try { parent.jQuery.unblockUI(); } catch (e) { }
            }
        });
    },
    renderDailyWLReport: function (dailyWLResult, wldate) {
        if (dailyWLResult) {
            OtherSportsReportManager.showOtherSportsBettypeReport(false);
            isBARMActive = dailyWLResult.NoneLiveReport.IsBARMActive.toString();
            OtherSportsReportManager.renderNoneLiveReportDWL2h(dailyWLResult.NoneLiveReport, wldate);
            OtherSportsReportManager.renderLiveReportDWL2h(dailyWLResult.LiveReport, wldate);
        }
    },
    renderNoneLiveReportDWL2h: function (noneLiveReport, wldate) {
        if (noneLiveReport && noneLiveReport.ReportDetails != null && noneLiveReport.ReportDetails.length > 0) {
            var arrBody = [];
            var items = noneLiveReport.ReportDetails;
            var totalSportReport = items.length;

            for (var i = 0; i < totalSportReport; i++) {
                arrBody.push(OtherSportsReportManager.renderDailyWLRow(items[i], (i % 2 == 0 ? "evenrow" : "")));
            }
            arrBody.push('<tr><td colspan="7" style="height: 18px;"></td></tr>');

            arrBody.push(OtherSportsReportManager.renderDailyWLTotal(noneLiveReport.ReportTotal.NormalTotal));

            var allIBCTotal = noneLiveReport.ReportTotal.EightRMTotal.WinLoss;
            if (isBARMActive == 'true') {
                allIBCTotal += noneLiveReport.ReportTotal.BARMTotal.WinLoss;
            }
            if (allIBCTotal != 0) {
                arrBody.push(OtherSportsReportManager.renderIBCTotalRow_LeftReport(allIBCTotal));
            }

            var bodyHtml = arrBody.join('');
            bodyHtml = StringFormatter.removeSpaceIE9(bodyHtml);

            $("#nonliveDailyWLOtherSports").find("tr:not(.header)").remove();
            $("#nonliveDailyWLOtherSports").append(bodyHtml);

        }
        else {
            $("#nonliveDailyWLOtherSports").find("tr:not(.header)").remove();
            $("#nonliveDailyWLOtherSports").append('<tr><td colspan="7" class="b c">There is no information available.</td></tr>');
        }
        OtherSportsReportManager.showNonLiveReport(true);
    },
    renderLiveReportDWL2h: function (liveReport, wldate) {
        if (liveReport && liveReport.ReportDetails != null && liveReport.ReportDetails.length > 0) {
            var arrBody = [];
            var items = liveReport.ReportDetails;
            var totalSportReport = items.length;

            for (var i = 0; i < totalSportReport; i++) {
                arrBody.push(OtherSportsReportManager.renderDailyWLRow(items[i], (i % 2 == 0 ? "evenrow" : "")));
            }
            arrBody.push('<tr><td colspan="7" style="height: 18px;"></td></tr>');

            arrBody.push(OtherSportsReportManager.renderDailyWLTotal(liveReport.ReportTotal.NormalTotal));
            var allIBCTotal = liveReport.ReportTotal.EightRMTotal.WinLoss;
            if (isBARMActive == 'true') {
                allIBCTotal += liveReport.ReportTotal.BARMTotal.WinLoss;
            }
            if (allIBCTotal != 0) {
                arrBody.push(OtherSportsReportManager.renderIBCTotalRow_LeftReport(allIBCTotal));
            }

            var bodyHtml = arrBody.join('');
            bodyHtml = StringFormatter.removeSpaceIE9(bodyHtml);

            $("#liveDailyWLOtherSports").find("tr:not(.header)").remove();
            $("#liveDailyWLOtherSports").append(bodyHtml);

        }
        else {
            $("#liveDailyWLOtherSports").find("tr:not(.header)").remove();
            $("#liveDailyWLOtherSports").append('<tr><td colspan="7" class="b c">There is no information available.</td></tr>');
        }
        OtherSportsReportManager.showLiveReport(true);

    },
    renderDailyWLRow: function (row, cssClass) {
        var html = '';
        html = '<tr class="' + cssClass + '">\
                        <td class="l"><span class="blue b" onclick="OtherSportsReportManager.getDailyWLBySport(\'' + row.Id + '\',\'' + row.Name + '\',\'' + row.IsLive + '\');">' + row.Name + '</span></td>\
                        <td class="r"><span class="' + (row.WinLoss.toFixed(2) <= 0 ? '' : 'red') + '">' + StringFormatter.formatNumber(row.WinLoss) + '</span></td>\
                        <td class="r"><span class="' + '' + '" style="">' + StringFormatter.formatNumber(row.Position) + '</span></td>\
                        <td class="r"><span class="' + '' + '" style="">' + StringFormatter.formatNumber(row.Commission) + '</span></td>\
                        <td class="r"><span class="' + (row.LineTotal.toFixed(2) <= 0 ? '' : 'red') + '">' + StringFormatter.formatNumber(row.LineTotal) + '</span></td>\
                        <td class="r"><span class="' + '' + '">' + StringFormatter.formatNumber(row.Turnover) + '</span></td>\
                        <td class="r"><span class="' + (row.Turnover.toFixed(2) != 0 && row.Margin.toFixed(2) <= 0 ? '' : 'red') + '" style="">' + (row.Turnover.toFixed(2) != 0 ? StringFormatter.formatNumber(row.Margin) + "%" : "N/A") + '</span></td>\
                    </tr>';
        return html;
    },
    renderDailyWLTotal: function (row) {
        var html = '';
        html = '<tr class="">\
                        <td class="r b"><span>' + row.Name + '</span></td>\
                        <td class="r"><span class="' + (row.WinLoss.toFixed(2) <= 0 ? '' : 'red') + '">' + StringFormatter.formatNumber(row.WinLoss) + '</span></td>\
                        <td class="r"><span class="' + '' + '" style="">' + StringFormatter.formatNumber(row.Position) + '</span></td>\
                        <td class="r"><span class="' + '' + '" style="">' + StringFormatter.formatNumber(row.Commission) + '</span></td>\
                        <td class="tdtitle r b" bgcolor="#dddddd"><span class="' + (row.LineTotal.toFixed(2) <= 0 ? '' : 'red') + '">' + StringFormatter.formatNumber(row.LineTotal) + '</span></td>\
                        <td class="r b"><span class="' + '' + '">' + StringFormatter.formatNumber(row.Turnover) + '</span></td>\
                        <td class="r"><span class="' + ((row.Turnover.toFixed(2) == 0 || row.Margin.toFixed(2) < 0) ? '' : 'red') + '" style="">' + (row.Turnover.toFixed(2) != 0 ? StringFormatter.formatNumber(row.Margin) + "%" : "N/A") + '</span></td>\
                    </tr>';
        return html;
    },
    renderIBCTotalRow_LeftReport: function (allIBCTotal) {
        var html = '';
        html = '<tr class="">\
                        <td class="r b"><span>' + 'IBC(8RM' +
                                        (isBARMActive == 'true' ? ' + ' + barmStr : '') + ') Total:' + '</span></td>\
                        <td class="r b"><span class="' + (allIBCTotal < 0 ? '' : 'red') + '">' + StringFormatter.formatNumber(allIBCTotal) + '</span></td>\
                        <td colspan="5"></td>\
                </tr>';
        return html;
    },
    getDailyWLBySport: function (sporttype, sportname, isLive, reportDate) {
        if (!isAllowReadOtherSportsReport) {
            return;
        }
        OtherSportsReportManager.sportTypeIdBySport = sporttype;
        if (reportDate == null || reportDate == undefined) {
            OtherSportsReportManager.reportDateBySport = OtherSportsReportManager.reportDate;
        }
        else {
            OtherSportsReportManager.reportDateBySport = reportDate;
        }

        var data = {
            param:
            {
                ReportDate: OtherSportsReportManager.reportDateBySport,
                SportType: sporttype
            }
        };
        try { parent.BlockUI(); } catch (e) { }
        DailyWinLoss2hManager.disableReportButton();
        OtherSportsReportManager.showOtherSportsBettypeReport(false);
        OtherSportsReportManager.sportTypeIdNoneLive = sporttype;
        OtherSportsReportManager.sportTypeNameNoneLive = sportname;
        $.ajax({
            type: "POST",
            url: window.rootUrl + "DailyWinLoss2hOtherSports/SelOtherSportsReportBySport",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: JSON.stringify(data),
            success: function (rs) {
                if ($("#InformationMssg").html() != "") {
                    $("#InformationMssg").hide();
                    $("#InformationMssg").empty();
                }
                OtherSportsReportManager.renderDailyWLReportBySport(rs, sporttype, sportname);
            },
            error: function () {
                $("#nonliveDailyWLOtherSportsBySport").find("tr:not(.header)").remove();
                $("#nonliveDailyWLOtherSportsBySport").append('<tr><td colspan="7" class="b c">There is no information available.</td></tr>');
                $("#liveDailyWLOtherSportsBySport").find("tr:not(.header)").remove();
                $("#liveDailyWLOtherSportsBySport").append('<tr><td colspan="7" class="b c">There is no information available.</td></tr>');

            },
            complete: function () {
                DailyWinLoss2hManager.enableReportButton();
                try { parent.jQuery.unblockUI(); } catch (e) { }
            }
        });
    },
    getDailyWLBySportLive: function (sporttype, sportname) {
    },
    getDailyWLBySportNoneLive: function (sporttype, sportname) {
    },
    renderDailyWLReportBySport: function (dailyWLResult, sporttype, sportname) {
        if (dailyWLResult) {

            $("#btnReport").unbind("click");
            $("#btnExport").unbind("click");
            $("#btnToday").unbind("click");
            $("#btnYesterday").unbind("click");

            $("#btnReport").click(function () {
                var reportDate = $("#txtfromdate").val();
                OtherSportsReportManager.getDailyWLBySport(sporttype, sportname, null, reportDate);
            });
            $("#btnExport").click(function () {
                OtherSportsReportManager.exportToExcel("7");
            });
            $("#btnToday").click(function () {
                OtherSportsReportManager.getDailyWLBySport(sporttype, sportname, null, new Date($('#btnToday').val()));
            });
            $("#btnYesterday").click(function () {
                OtherSportsReportManager.getDailyWLBySport(sporttype, sportname, null, new Date($('#btnYesterday').val()));
            });

            var fullpath = "<b><a onclick=\"WL2hShowHideDivManager.showMainReportFromOSR('" + OtherSportsReportManager.sportTypeId +
                         "','" + OtherSportsReportManager.reportDate + "')\"> Main </a> > " +
                         "<a onclick=\"OtherSportsReportManager.showNonLiveReport(true);\"> " + OtherSportsReportManager.path + " </a> > " + sportname + "</b>";

            $("#trPath").html(fullpath);


            OtherSportsReportManager.sportTypeIdNoneLive = sporttype;
            OtherSportsReportManager.sportTypeIdLive = sporttype;

            OtherSportsReportManager.sportTypeNameNoneLive = sportname;
            OtherSportsReportManager.sportTypeNameLive = sportname;

            isBARMActive = dailyWLResult.NoneLiveReport.IsBARMActive.toString();
            OtherSportsReportManager.renderNoneLiveReportBySportDWL2h(dailyWLResult.NoneLiveReport, sporttype, sportname);
            OtherSportsReportManager.renderLiveReportBySportDWL2h(dailyWLResult.LiveReport, sporttype, sportname);

            OtherSportsReportManager.showNonLiveReport(false);
        }
    },
    renderNoneLiveReportBySportDWL2h: function (noneLiveReport, sporttype, sportname) {
        OtherSportsReportManager.noneLiveReportTitle = "OTHER SPORTS|" + sportname.toUpperCase() + ' "NON - LIVE" PRODUCT WIN\/LOSS REPORT';
        $("#dailyWLOtherSports_NonLiveTitleBySport").html(OtherSportsReportManager.noneLiveReportTitle);
        if (noneLiveReport != null && noneLiveReport.ReportDetails &&
            noneLiveReport.ReportDetails.length > 0) {

            var arrBody = [];
            var items = noneLiveReport.ReportDetails;
            var totalSportReport = items.length;
            for (var i = 0; i < totalSportReport; i++) {
                arrBody.push(OtherSportsReportManager.renderDailyWLRowBySport(items[i], (i % 2 == 0 ? "evenrow" : "")));
            }
            arrBody.push('<tr><td colspan="7" style="height: 18px;"></td></tr>');
            arrBody.push(OtherSportsReportManager.renderDailyWLTotalBySport(noneLiveReport.ReportTotal.NormalTotal));

            var allIBCTotal = noneLiveReport.ReportTotal.EightRMTotal.WinLoss;
            if (isBARMActive == 'true') {
                allIBCTotal += noneLiveReport.ReportTotal.BARMTotal.WinLoss;
            }
            if (allIBCTotal) {
                arrBody.push(OtherSportsReportManager.renderIBCTotalRowBySport_LeftReport(allIBCTotal));
            }

            var bodyHtml = arrBody.join('');
            bodyHtml = StringFormatter.removeSpaceIE9(bodyHtml);

            $("#nonliveDailyWLOtherSportsBySport").find("tr:not(.header)").remove();
            $("#nonliveDailyWLOtherSportsBySport").append(bodyHtml);

        }
        else {
            $("#nonliveDailyWLOtherSportsBySport").find("tr:not(.header)").remove();
            $("#nonliveDailyWLOtherSportsBySport").append('<tr><td colspan="7" class="b c">There is no information available.</td></tr>');
        }
    },
    renderLiveReportBySportDWL2h: function (liveReport, sporttype, sportname) {
        OtherSportsReportManager.liveReportTitle = "OTHER SPORTS|" + sportname.toUpperCase() + ' "LIVE" PRODUCT WIN\/LOSS REPORT';
        $("#dailyWLOtherSports_LiveTitleBySport").html(OtherSportsReportManager.liveReportTitle);
        if (liveReport != null && liveReport.ReportDetails &&
            liveReport.ReportDetails.length > 0) {

            var arrBody = [];
            var items = liveReport.ReportDetails;
            var totalSportReport = items.length;
            for (var i = 0; i < totalSportReport; i++) {
                arrBody.push(OtherSportsReportManager.renderDailyWLRowBySport(items[i], (i % 2 == 0 ? "evenrow" : "")));
            }
            arrBody.push('<tr><td colspan="7" style="height: 18px;"></td></tr>');
            arrBody.push(OtherSportsReportManager.renderDailyWLTotalBySport(liveReport.ReportTotal.NormalTotal));

            var allIBCTotal = liveReport.ReportTotal.EightRMTotal.WinLoss;
            if (isBARMActive == 'true') {
                allIBCTotal += liveReport.ReportTotal.BARMTotal.WinLoss;
            }
            if (allIBCTotal != 0) {
                arrBody.push(OtherSportsReportManager.renderIBCTotalRowBySport_LeftReport(allIBCTotal));
            }

            var bodyHtml = arrBody.join('');
            bodyHtml = StringFormatter.removeSpaceIE9(bodyHtml);

            $("#liveDailyWLOtherSportsBySport").find("tr:not(.header)").remove();
            $("#liveDailyWLOtherSportsBySport").append(bodyHtml);

        }
        else {
            $("#liveDailyWLOtherSportsBySport").find("tr:not(.header)").remove();
            $("#liveDailyWLOtherSportsBySport").append('<tr><td colspan="7" class="b c">There is no information available.</td></tr>');
        }
    },
    renderDailyWLRowBySport: function (row, cssClass) {
        var html = '';
        html = '<tr class="' + cssClass + '">\
                        <td class="l"><span class="blue b" onclick="OtherSportsReportManager.getDailyWLBySportBettype(\'' + row.Id + '\',\'' + row.Name + '\',\'' + row.IsLive + '\',\'' + row.Hedge + '\');">' + row.AliasName + '</span></td>\
                        <td class="r"><span class="' + (row.WinLoss.toFixed(2) <= 0 ? '' : 'red') + '">' + StringFormatter.formatNumber(row.WinLoss) + '</span></td>\
                        <td class="r"><span class="' + '' + '" style="">' + StringFormatter.formatNumber(row.Position) + '</span></td>\
                        <td class="r"><span class="' + '' + '" style="">' + StringFormatter.formatNumber(row.Commission) + '</span></td>\
                        <td class="r"><span class="' + (row.LineTotal.toFixed(2) <= 0 ? '' : 'red') + '">' + StringFormatter.formatNumber(row.LineTotal) + '</span></td>\
                        <td class="r"><span class="' + '' + '">' + StringFormatter.formatNumber(row.Turnover) + '</span></td>\
                        <td class="r"><span class="' + (row.Turnover.toFixed(2) == 0 || row.Margin.toFixed(2) <= 0 ? '' : 'red') + '" style="">' + (row.Turnover.toFixed(2) != 0 ? StringFormatter.formatNumber(row.Margin) + "%" : "N/A") + '</span></td>\
                    </tr>';
        return html;
    },
    renderDailyWLTotalBySport: function (row) {
        var html = '';
        html = '<tr class="">\
                        <td class="r b"><span>' + "All Total:" + '</span></td>\
                        <td class="r"><span class="' + (row.WinLoss.toFixed(2) <= 0 ? '' : 'red') + '">' + StringFormatter.formatNumber(row.WinLoss) + '</span></td>\
                        <td class="r"><span class="' + '' + '" style="">' + StringFormatter.formatNumber(row.Position) + '</span></td>\
                        <td class="r"><span class="' + '' + '" style="">' + StringFormatter.formatNumber(row.Commission) + '</span></td>\
                        <td class="tdtitle r b" bgcolor="#dddddd"><span class="' + (row.LineTotal.toFixed(2) <= 0 ? '' : 'red') + '">' + StringFormatter.formatNumber(row.LineTotal) + '</span></td>\
                        <td class="r b"><span class="' + '' + '">' + StringFormatter.formatNumber(row.Turnover) + '</span></td>\
                        <td class="r"><span class="' + ((row.Turnover.toFixed(2) == 0 || row.Margin.toFixed(2) <= 0) ? '' : 'red') + '" style="">' + (row.Turnover.toFixed(2) != 0 ? StringFormatter.formatNumber(row.Margin) + "%" : "N/A") + '</span></td>\
                    </tr>';
        return html;
    },
    renderIBCTotalRowBySport_LeftReport: function (allIBCTotal) {
        var html = '';
        html = '<tr class="">\
                        <td class="r b"><span>' + 'IBC(8RM' +
                                                   (isBARMActive == 'true' ? ' + ' + barmStr : '')
                                                   + ') Total:' + '</span></td>\
                        <td class="r b"><span class="' + (allIBCTotal.toFixed(2) < 0 ? '' : 'red') + '">' + StringFormatter.formatNumber(allIBCTotal) + '</span></td>\
                        <td colspan="5"></td>\
                </tr>';
        return html;
    },
    getDailyWLBySportBettype: function (bettype, bettypename, isLive, hedge) {
        if (!isAllowReadOtherSportsReport) {
            return;
        }
        var sporttype = "";
        var sporttypeName = "";
        if (isLive == 'true') {
            sporttype = OtherSportsReportManager.sportTypeIdLive;
            sporttypeName = OtherSportsReportManager.sportTypeNameLive;
        }
        else {
            sporttype = OtherSportsReportManager.sportTypeIdNoneLive;
            sporttypeName = OtherSportsReportManager.sportTypeNameNoneLive;
        }

        var isHedge = (hedge && hedge.indexOf("(hedge)") != -1);

        var data = {
            param:
            {
                ReportDate: OtherSportsReportManager.reportDateBySport,
                SportType: sporttype,
                BetType: bettype,
                IsLive: (isLive == 'true' ? 1 : 0),
                IsHedge: (isHedge ? true : false)
            }
        };

        try { parent.BlockUI(); } catch (e) { }
        DailyWinLoss2hManager.disableReportButton();
        OtherSportsReportManager.bettype = bettype;
        OtherSportsReportManager.bettypeName = bettypename;
        OtherSportsReportManager.isLive = (isLive == 'true' ? 1 : 0);
        OtherSportsReportManager.isHedge = isHedge;

        $.ajax({
            type: "POST",
            url: window.rootUrl + "DailyWinLoss2hOtherSports/SelDailyWLDirectDetails",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: JSON.stringify(data),
            success: function (rs) {
                if ($("#InformationMssg").html() != "") {
                    $("#InformationMssg").hide();
                    $("#InformationMssg").empty();
                }
                OtherSportsReportManager.renderDailyWLReportSportBettype(rs, sporttypeName, bettypename, isLive, isHedge);

            },
            error: function () {
                $("#tblContent_RDtOsp").find("tr:not(.header)").remove();
                $("#tblContent_RDtOsp").append('<tr><td colspan="10" class="b c"><span style="">There is no information available.</span></td></tr>');
                OtherSportsReportManager.showOtherSportsBettypeReport(true);
                $("#gridbettypeOSR").removeClass('hidden');
            },
            complete: function () {
                DailyWinLoss2hManager.enableReportButton();
                try { parent.jQuery.unblockUI(); } catch (e) { }
            }
        });
    },
    renderDailyWLReportSportBettype: function (dailyWLResult, sporttypeName, bettypename, isLive, isHedge) {
        if (dailyWLResult) {
            isBARMActive = dailyWLResult.IsBARMActive.toString();
            var detailReportTitle = "Main>Other Sports|" + sporttypeName + ">" + bettypename;
            $("#gridtitlebettype_OSR").html(detailReportTitle);
            if (dailyWLResult.ReportDetails != null && dailyWLResult.ReportDetails.length > 0) {
                var items = dailyWLResult.ReportDetails;
                var arrBody = [];
                var arrFooter = [];
                var numberItems = items.length;

                for (var i = 0; i < numberItems; i++) {
                    arrBody.push(OtherSportsReportManager.renderDailyWLRowSportDetails(items[i], i));
                }

                arrFooter.push(OtherSportsReportManager.renderTotalRow_RightReport(dailyWLResult.ReportTotal.NormalTotal, isLive, bettypename, isHedge));

                var allIBCTotal = dailyWLResult.ReportTotal.EightRMTotal.WinLoss;
                if (isBARMActive == 'true') {
                    allIBCTotal += dailyWLResult.ReportTotal.BARMTotal.WinLoss;
                }

                if (allIBCTotal != 0) {
                    arrFooter.push(OtherSportsReportManager.renderIBCTotalRow_RightReport(allIBCTotal));
                }

                var bodyHtml = arrBody.join('');
                bodyHtml = StringFormatter.removeSpaceIE9(bodyHtml);
                $("#tblContent_RDtOsp").find("tr:not(.header)").remove();
                $("#tblContent_RDtOsp").append(bodyHtml);

                var footerHtml = arrFooter.join('');
                footerHtml = StringFormatter.removeSpaceIE9(footerHtml);
                $("#tblContent_RDtOsp").append(footerHtml);

            }
            else {
                $("#tblContent_RDtOsp").find("tr:not(.header)").remove();
                $("#tblContent_RDtOsp").append('<tr><td colspan="10" class="b c"><span style="">There is no information available.</span></td></tr>');
            }

            OtherSportsReportManager.showOtherSportsBettypeReport(true);
            $("#gridbettypeOSR").removeClass('hidden');
        }
    },
    renderDailyWLRowSportDetails: function (dwlRow, index) {
        var html = "";
        switch (dwlRow.RowTypeDWL2h.Id) {
            case 0:
                html = OtherSportsReportManager.renderMainRow_RightReport(dwlRow, index);
                break;
            case 3:
                html = OtherSportsReportManager.renderBettypeTotalRow_RightReport(dwlRow);
                break;
            case 9:
                html = OtherSportsReportManager.emptyRowHtml_RightReport(dwlRow);
                break;
            case 10:
                html = OtherSportsReportManager.renderLeagueHeadLine(dwlRow);
                break;
        }
        return html;
    },
    renderLeagueHeadLine: function (dwlRow) {
        var html = '';
        html = '<tr>\
                    <td>\
                        <span style=""></span></td>\
                    <td colspan = "9" class="l" style=""><span class="b longtext-wrapper">' + dwlRow.Name + '</span></td>\
                </tr>';
        return html;
    },
    renderMainRow_RightReport: function (row, index) {
        var html = '';
        var eventDetail = "";
        if (OtherSportsReportManager.bettype == 10 || OtherSportsReportManager.sportTypeId == 201) {
            eventDetail = '<td colspan = "3" style="max-width: 300px;"><span class="c longtext-wrapper" style="">' + row.MatchInfo.HomeTeam.Name + (row.MatchInfo.IsNeutral ? " (N)" : "") + '</span></td>';
        }
        else {
            eventDetail = '<td class="r" style="max-width: 300px;"><span class="longtext-wrapper" style="">' + row.MatchInfo.HomeTeam.Name + (row.MatchInfo.IsNeutral ? " (N)" : "") + '</span></td>\
                        <td class="c" style="">-vs-</td>\
                        <td class="l" style="max-width: 300px;"><span class="longtext-wrapper" style="">' + row.MatchInfo.AwayTeam.Name + '</span></td>';
        }

        html = '<tr class="' + (row.RowIndex % 2 == 0 ? "evenrow" : "") + '">\
                    <td style="word-wrap: break-word;"><span class="c" style="">' + row.MatchInfo.Name + '</span></td>'
                   + eventDetail + '<td class="r"><span class="' + (row.WinLoss.toFixed(2) > 0 ? 'red' : '') + '" style="">' + StringFormatter.formatNumber(row.WinLoss) + '</span></td>\
                    <td class="r"><span class="" style="">' + StringFormatter.formatNumber(row.Position) + '</span></td>\
                    <td class="r"><span class="" style="">' + StringFormatter.formatNumber(row.Commission) + '</span></td>\
                    <td class="r"><span class="' + (row.LineTotal.toFixed(2) > 0 ? 'red' : '') + '" style="">' + StringFormatter.formatNumber(row.LineTotal) + '</span></td>\
                    <td class="r"><span class="" style="">' + StringFormatter.formatNumber(row.Turnover) + '</span></td>\
                    <td class="r"><span class=" ' + (row.Turnover.toFixed(2) != 0 && row.Margin.toFixed(2) > 0 ? 'red' : '') + '" style="">' + (row.Turnover.toFixed(2) != 0 ? StringFormatter.formatNumber(row.Margin) + '%' : 'N/A') + '</span></td>\
                </tr>';
        return html;
    },
    renderBettypeTotalRow_RightReport: function (row) {
        var html = '';
        html = '<tr>\
                <td class="r tdtitle" bgcolor="#888888" colspan = "4"><span class="b" style=""></span></td>\
                <td class="r"><span class="' + (row.WinLoss.toFixed(2) > 0 ? 'red' : '') + '" style="font-weight: normal;">' + StringFormatter.formatNumber(row.WinLoss) + '</span></td>\
                <td class="r"><span class="" style="font-weight: normal;">' + StringFormatter.formatNumber(row.Position) + '</span></td>\
                <td class="r"><span class="" style="font-weight: normal;">' + StringFormatter.formatNumber(row.Commission) + '</span></td>\
                <td class="r" style=""><span class="' + (row.LineTotal.toFixed(2) > 0 ? 'red' : '') + '" style="">' + StringFormatter.formatNumber(row.LineTotal) + '</span></td>\
                <td class="r"><span class="" style="">' + StringFormatter.formatNumber(row.Turnover) + '</span></td>\
                <td class="r"><span class="' + (row.Turnover.toFixed(2) != 0 && row.Margin.toFixed(2) > 0 ? 'red' : '') + '" style="font-weight: normal;">' + (row.Turnover.toFixed(2) != 0 ? StringFormatter.formatNumber(row.Margin) + '%' : 'N/A') + '</span></td>\
            </tr>';
        return html;
    },
    renderTotalRow_RightReport: function (row, isLive, bettypeName, isHedge) {
        var html = '';
        var totalTitle = (isLive == "true" ? "<font color='red'>Live!</font>" : "") + " " +
                        bettypeName + (isHedge ? " (Hegde)" : "") + " Total:";
        html = '<tr>\
                <td class="r" colspan = "4"><div class="b longtext-wrapper" style="">' + totalTitle + '</span></td>\
                <td class="r"><span class="' + (row.WinLoss.toFixed(2) > 0 ? 'red' : '') + '" style="font-weight: normal;">' + StringFormatter.formatNumber(row.WinLoss) + '</span></td>\
                <td class="r"><span class="" style="font-weight: normal;">' + StringFormatter.formatNumber(row.Position) + '</span></td>\
                <td class="r"><span class="" style="font-weight: normal;">' + StringFormatter.formatNumber(row.Commission) + '</span></td>\
                <td class="r" style=""><span class="' + (row.LineTotal.toFixed(2) > 0 ? 'red' : '') + '" style="font-weight: normal;">' + StringFormatter.formatNumber(row.LineTotal) + '</span></td>\
                <td class="r"><span class="" style="font-weight: normal;">' + StringFormatter.formatNumber(row.Turnover) + '</span></td>\
                <td class="r"><span class="' + (row.Turnover.toFixed(2) != 0 && row.Margin.toFixed(2) > 0 ? 'red' : '') + '" style="font-weight: normal;">' + (row.Turnover.toFixed(2) != 0 ? StringFormatter.formatNumber(row.Margin) + '%' : 'N/A') + '</span></td>\
            </tr>';
        return html;
    },
    renderIBCTotalRow_RightReport: function (ibcValue) {
        var html = '<tr>\
                        <td class="r" colspan = "4"><span class="b" style="">IBC(8RM' +
                                                   (isBARMActive == 'true' ? ' + ' + barmStr : '')
                                                   + ') Total:</span></td>\
                        <td class="r"><span class="b ' + (ibcValue.toFixed(2) > 0 ? 'red' : '') + '" style="">' + StringFormatter.formatNumber(ibcValue) + '</span></td>\
                        <td colspan ="5" class ="c" style="height:18px;"></td>\
                    </tr>';
        return html;
    },
    showNonLiveReport: function (isshow) {
        if (isshow) {
            $("#liveDailyWLOtherSportsBySport").addClass("hidden");
            $("#nonliveDailyWLOtherSportsBySport").addClass("hidden");

            $("#liveDailyWLOtherSports").removeClass("hidden");
            $("#nonliveDailyWLOtherSports").removeClass("hidden");

            OtherSportsReportManager.changeAction2Sporttype(isshow);
        }
        else {
            $("#liveDailyWLOtherSports").addClass("hidden");
            $("#nonliveDailyWLOtherSports").addClass("hidden");

            $("#liveDailyWLOtherSportsBySport").removeClass("hidden");
            $("#nonliveDailyWLOtherSportsBySport").removeClass("hidden");
        }
        OtherSportsReportManager.showOtherSportsBettypeReport(false);
    },
    showLiveReport: function (isshow) {
        if (isshow) {
            $("#liveDailyWLOtherSportsBySport").addClass("hidden");
            $("#nonliveDailyWLOtherSportsBySport").addClass("hidden");

            $("#liveDailyWLOtherSports").removeClass("hidden");
            $("#nonliveDailyWLOtherSports").removeClass("hidden");

            OtherSportsReportManager.changeAction2Sporttype(isshow);
        }
        else {
            $("#liveDailyWLOtherSports").addClass("hidden");
            $("#nonliveDailyWLOtherSports").addClass("hidden");

            $("#liveDailyWLOtherSportsBySport").removeClass("hidden");
            $("#nonliveDailyWLOtherSportsBySport").removeClass("hidden");
        }
        OtherSportsReportManager.showOtherSportsBettypeReport(false);
    },
    changeAction2Sporttype: function (isChange) {
        if (isChange) {
            $("#txtfromdate").datepicker("setDate", OtherSportsReportManager.reportDate);

            var fullpath = "<b><a onclick=\"WL2hShowHideDivManager.showMainReportFromOSR('" + OtherSportsReportManager.sportTypeId +
                         "','" + OtherSportsReportManager.reportDate + "')\"> Main </a> > " + OtherSportsReportManager.path + "</b>";

            $("#trPath").html(fullpath);

            $("#btnReport").unbind("click");
            $("#btnExport").unbind("click");
            $("#btnToday").unbind("click");
            $("#btnYesterday").unbind("click");

            $("#btnReport").click(function () {
                var reportDate = $("#txtfromdate").val();
                OtherSportsReportManager.viewReport(reportDate);
            });
            $("#btnExport").click(function () {
                OtherSportsReportManager.exportToExcel("1");
            });
            $("#btnToday").click(function () {
                OtherSportsReportManager.viewReportByDate(1);
            });
            $("#btnYesterday").click(function () {
                OtherSportsReportManager.viewReportByDate(2);
            });
        }
    },
    showMainReport: function (isshow) {
        if (isshow) {
            OtherSportsReportManager.showOtherSportsBettypeReport(false);
        }
    },
    showOtherSportsBettypeReport: function (isshow) {
        if (isshow) {
            $("#gridSplitter_OSR").removeClass('hidden');
            $("#gridbettypeOSR").removeClass("hidden");
            $("#otherSportsReport").css("width", "1500px");
            var totalWidth = $("#otherSportsReportTable").width() + 10;
            $("#otherSportsReport").css("width", totalWidth + "px");
        }
        else {
            $("#otherSportsReport").css("width", "100%");
            $("#gridSplitter_OSR").addClass('hidden');
            $("#gridbettypeOSR").addClass("hidden");
        }
    },
    hideAllReport: function () {
        $("#otherSportsReport").addClass("hidden");
        OtherSportsReportManager.showOtherSportsBettypeReport(false);
        $("#liveDailyWLOtherSportsBySport").addClass("hidden");
        $("#nonliveDailyWLOtherSportsBySport").addClass("hidden");

        $("#liveDailyWLOtherSports").addClass("hidden");
        $("#nonliveDailyWLOtherSports").addClass("hidden");
    },
    backToMain: function () {
        WL2hShowHideDivManager.showMainReportFromOSR(OtherSportsReportManager.sportTypeId, OtherSportsReportManager.reportDate);
    },
    exportToExcel: function (reportType) {
        if (!isAllowReadOtherSportsReport) {
            return;
        }
        var url = "DailyWinLoss2hOtherSports?";
        switch (reportType) {
            case "1": // OtherSportsMain
                url += "&ExportType=OtherSportsMain";
                url += "&ReportDate=" + OtherSportsReportManager.reportDate;
                break;
            case "2": // OtherSportsMain_NoneLive
                url += "&ExportType=OtherSportsMain_NoneLive";
                url += "&ReportDate=" + OtherSportsReportManager.reportDate;
                break;
            case "3": // OtherSportsMain_Live
                url += "&ExportType=OtherSportsMain_Live";
                url += "&ReportDate=" + OtherSportsReportManager.reportDate;
                break;
            case "4": // OtherSportBySport_NoneLive
                url += "&ExportType=OtherSportsBySport_NoneLive";
                url += "&ReportDate=" + OtherSportsReportManager.reportDateBySport;
                url += "&SportType=" + OtherSportsReportManager.sportTypeIdBySport;
                break;
            case "5": // OtherSportBySport_Live
                url += "&ExportType=OtherSportsBySport_Live";
                url += "&ReportDate=" + OtherSportsReportManager.reportDateBySport;
                url += "&SportType=" + OtherSportsReportManager.sportTypeIdBySport;
                break;
            case "6": // OtherSportsBySportBettypeDetails
                url += "&ExportType=OtherSportsBySportBettypeDetails";
                url += "&ReportDate=" + OtherSportsReportManager.reportDateBySport;
                if (OtherSportsReportManager.isLive) {
                    url += "&SportType=" + OtherSportsReportManager.sportTypeIdBySport;
                }
                else {
                    url += "&SportType=" + OtherSportsReportManager.sportTypeIdBySport;
                }

                url += "&BetType=" + OtherSportsReportManager.bettype;
                url += '&BetTypeName=' + OtherSportsReportManager.bettypeName;
                url += '&IsLive=' + OtherSportsReportManager.isLive;
                url += '&IsHedge=' + OtherSportsReportManager.isHedge;
                break;
            case "7": // OtherSportBySport
                url += "&ExportType=OtherSportsBySport";
                url += "&ReportDate=" + OtherSportsReportManager.reportDateBySport;
                url += "&SportType=" + OtherSportsReportManager.sportTypeIdBySport;
                break;
        }
        $('#frm').attr('src', url);
    }

};
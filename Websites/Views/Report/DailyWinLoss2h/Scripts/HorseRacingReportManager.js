

var HorseRacingReportManager = {
    reportDate: null,
    sportTypeId: null,
    sportTypeName: null,
    bettype: null,
    bettypename: null,
    path: "Racing",
    backToMainMenuText: "Back to Main Menu",
    isFirstLoad: true,
    initGrid: function (sportId, wldate, bettype) {
        HorseRacingReportManager.sportTypeId = sportId;
        HorseRacingReportManager.reportDate = wldate;
        HorseRacingReportManager.bettype = bettype;

        $("#btnReport").unbind("click");
        $("#btnExport").unbind("click");
        $("#btnReport").click(function () {
            var reportDate = $("#txtfromdate").val();
            HorseRacingReportManager.viewReport(reportDate);
        });
        $("#btnExport").click(function () {
            HorseRacingReportManager.exportToExcel("1");
        });
        $('#spMoreElement').empty();
        var strtoday = '<input type="button" onclick="HorseRacingReportManager.viewReportByDate(1)" id="btnToday" name="Today" class="btn"/>';
        var stryesterday = '<input type="button" onclick="HorseRacingReportManager.viewReportByDate(2)" id="btnYesterday" name="Yesterday" class="btn"/>';
        $('#spMoreElement').append(strtoday);
        $('#spMoreElement').append(stryesterday);
        $('#btnToday').val(today);
        $('#btnYesterday').val(yesterday);
        var fullpath = "<b><a onclick=\"WL2hShowHideDivManager.showMainReportFromHRR('" + sportId +
                         "','" + wldate + "')\"> Main </a> > " + HorseRacingReportManager.path + "</b>";
        $("#trPath").html(fullpath);
        $("#backToMainMenu_HRR").html(HorseRacingReportManager.backToMainMenuText);
        $("#backToMainMenu_HRR").unbind("click");
        $("#backToMainMenu_HRR").click(function () {
            WL2hShowHideDivManager.showMainReportFromHRR(sportId, wldate);
        });

        $("#dailyWLReportDiv").removeClass("hidden");
        HorseRacingReportManager.initReportTable();

        try { parent.jQuery.unblockUI(); } catch (e) { }
        HorseRacingReportManager.viewReport(wldate);
        //        HorseRacingReportManager.initGridSplitter();
    },
    initReportTable: function () {


    },
    viewReport: function (wldate) {
        HorseRacingReportManager.getcontent(wldate);
    },
    viewReportByDate: function (date) {
        switch (date) {
            case 1:
                $("#txtfromdate").datepicker("setDate", new Date($('#btnToday').val()));
                HorseRacingReportManager.getcontent($('#btnToday').val());
                break;
            case 2:
                $("#txtfromdate").datepicker("setDate", new Date($('#btnYesterday').val()));
                HorseRacingReportManager.getcontent($('#btnYesterday').val());
                break;
            default:
                break;
        }
    },
    getcontent: function (wldate) {
        if (!isAllowReadHorseRacingReport) {
            return;
        }
        HorseRacingReportManager.reportDate = wldate;
        var data = {
            param:
            {
                ReportDate: wldate
            }
        };
        try { parent.BlockUI(); } catch (e) { }
        $("#horseRacingReport").removeClass("hidden");
        DailyWinLoss2hManager.disableReportButton();
        $.ajax({
            type: "POST",
            url: window.rootUrl + "DailyWinLoss2h/DailyWLDirectRacing",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: JSON.stringify(data),
            success: function (rs) {
                if ($("#InformationMssg").html() != "") {
                    $("#InformationMssg").hide();
                    $("#InformationMssg").empty();
                }
                HorseRacingReportManager.renderDailyWLReport(rs, wldate);
                HorseRacingReportManager.showMainReport(true);
            },
            error: function () {
                $("#dailyWLRacing").find("tr:not(.header)").remove();
                $("#dailyWLRacing").append('<tr><td colspan="7" class="b c">There is no information available.</td></tr>');
            },
            complete: function () {
                DailyWinLoss2hManager.enableReportButton();
                try { parent.jQuery.unblockUI(); } catch (e) { }
            }
        });
    },
    renderDailyWLReport: function (dailyWLResult, wldate) {
        if (dailyWLResult && dailyWLResult.ReportDetails && dailyWLResult.ReportDetails.length > 0) {
            var arrBody = [];
            var arrFooter = [];
            var items = dailyWLResult.ReportDetails;
            var total = items.length;
            for (var i = 0; i < total; i++) {
                arrBody.push(HorseRacingReportManager.renderDailyWLRow(items[i]));
            }
            arrBody.push(HorseRacingReportManager.renderDailyWLTotal(dailyWLResult.ReportTotal.NormalTotal));

            var bodyHtml = arrBody.join('');
            bodyHtml = StringFormatter.removeSpaceIE9(bodyHtml);

            $("#dailyWLRacing").find("tr:not(.header)").remove();
            $("#dailyWLRacing").append(bodyHtml);
        }
        else {
            $("#dailyWLRacing").find("tr:not(.header)").remove();
            $("#dailyWLRacing").append('<tr><td colspan="7" class="b c">There is no information available.</td></tr>');
        }
    },
    renderDailyWLRow: function (dwlRow) {
        var html = "";
        switch (dwlRow.RowTypeDWL2h.Id) {
            case 0:
                html = HorseRacingReportManager.normalRowHtml(dwlRow);
                break;
            case 9:
                html = HorseRacingReportManager.emptyRowHtml(dwlRow);
                break;
        }
        return html;
    },
    normalRowHtml: function (row) {
        var html = '';
        html = '<tr>\
                        <td class="l"><span class="blue b" onclick="HorseRacingReportManager.getDailyWLBySport(\'' + row.Id + '\',\'' + row.Name + '\');">' + row.Name + '</span></td>\
                        <td class="r"><span class="' + (row.WinLoss.toFixed(2) <= 0 ? '' : 'red') + '">' + StringFormatter.formatNumber(row.WinLoss) + '</span></td>\
                        <td class="r"><span class="' + (row.Position.toFixed(2) <= 0 ? '' : 'red') + '">' + StringFormatter.formatNumber(row.Position) + '</span></td>\
                        <td class="r"><span class="' + (row.Commission.toFixed(2) <= 0 ? '' : 'red') + '">' + StringFormatter.formatNumber(row.Commission) + '</span></td>\
                        <td class="r"><span class="' + (row.LineTotal.toFixed(2) <= 0 ? '' : 'red') + '">' + StringFormatter.formatNumber(row.LineTotal) + '</span></td>\
                        <td class="r"><span class="' + (row.Turnover.toFixed(2) <= 0 ? '' : 'red') + '">' + StringFormatter.formatNumber(row.Turnover) + '</span></td>\
                        <td class="r"><span class="' + ((row.Turnover.toFixed(2) == 0 || row.Margin.toFixed(2) <= 0) ? '' : 'red') + '">' + (row.Turnover.toFixed(2) != 0 ? StringFormatter.formatNumber(row.Margin) + "%" : "N/A") + '</span></td>\
                    </tr>';
        return html;
    },
    emptyRowHtml: function (dwlRow, wldate) {
        var html = '<tr>\
                        <td colspan ="7" class ="c" style="height:18px;"></td>\
                    </tr>';
        return html;
    },
    renderDailyWLTotal: function (row) {
        var html = '';
        html = '<tr class="">\
                        <td class="r b"><span>' + "All Total:" + '</span></td>\
                        <td class="r"><span class="' + (row.WinLoss.toFixed(2) <= 0 ? '' : 'red') + '">' + StringFormatter.formatNumber(row.WinLoss) + '</span></td>\
                        <td class="r"><span class="' + (row.Position.toFixed(2) <= 0 ? '' : 'red') + '">' + StringFormatter.formatNumber(row.Position) + '</span></td>\
                        <td class="r"><span class="' + (row.Commission.toFixed(2) <= 0 ? '' : 'red') + '">' + StringFormatter.formatNumber(row.Commission) + '</span></td>\
                        <td class="linetotal r b"><span class="' + (row.LineTotal.toFixed(2) <= 0 ? '' : 'red') + '">' + StringFormatter.formatNumber(row.LineTotal) + '</span></td>\
                        <td class="r b"><span class="' + (row.Turnover.toFixed(2) <= 0 ? '' : 'red') + '">' + StringFormatter.formatNumber(row.Turnover) + '<span></td>\
                        <td class="r"><span class="' + ((row.Turnover.toFixed(2) == 0 || row.Margin.toFixed(2) <= 0) ? '' : 'red') + '">' + (row.Turnover.toFixed(2) != 0 ? StringFormatter.formatNumber(row.Margin) + "%" : "N/A") + '</span></td>\
                    </tr>';
        return html;
    },
    createScroll: function () {
    },
    renderNoData: function () {
        $("#dailyWLRacing").find("tr:not(.header)").remove();
        $("#dailyWLRacing").html('<tr id="noinfo"><td id="tdNoInfomation" colspan="7" class="b c" style="" >There is no information available.</td></tr>');
    },
    getDailyWLBySport: function (sporttype, sportname) {
        if (!isAllowReadHorseRacingReport) {
            return;
        }
        var data = {
            param:
            {
                ReportDate: HorseRacingReportManager.reportDate,
                SportType: sporttype
            }
        };
        try { parent.BlockUI(); } catch (e) { }
        DailyWinLoss2hManager.disableReportButton();
        HorseRacingReportManager.showRacingBettypeReport(false);
        HorseRacingReportManager.sportTypeId = sporttype;
        HorseRacingReportManager.sportTypeName = sportname;
        $.ajax({
            type: "POST",
            url: window.rootUrl + "DailyWinLoss2h/DailyWLDirectRacingBySport",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: JSON.stringify(data),
            success: function (rs) {
                if ($("#InformationMssg").html() != "") {
                    $("#InformationMssg").hide();
                    $("#InformationMssg").empty();
                }
                HorseRacingReportManager.title = "Daily WinLoss Racing|" + sportname;
                $("#dailyWLBySportTitle").html(HorseRacingReportManager.title);

                HorseRacingReportManager.renderDailyWLReportBySport(rs, sporttype);
                HorseRacingReportManager.showMainReport(false);
            },
            error: function () {
                $("#dailyWLBySport").find("tr:not(.header)").remove();
                $("#dailyWLBySport").append('<tr><td colspan="7" class="b c">There is no information available.</td></tr>');
            },
            complete: function () {
                DailyWinLoss2hManager.enableReportButton();
                try { parent.jQuery.unblockUI(); } catch (e) { }
            }
        });
    },
    renderDailyWLReportBySport: function (dailyWLResult, sporttype) {
        if (dailyWLResult && dailyWLResult.ReportDetails && dailyWLResult.ReportDetails.length > 0) {
            var arrBody = [];
            var arrFooter = [];
            var items = dailyWLResult.ReportDetails;
            var total = items.length;
            for (var i = 0; i < total; i++) {
                arrBody.push(HorseRacingReportManager.renderDailyWLRowBySport(items[i], i));
            }
            arrBody.push(HorseRacingReportManager.renderDailyWLTotal(dailyWLResult.ReportTotal.NormalTotal));

            var bodyHtml = arrBody.join('');
            bodyHtml = StringFormatter.removeSpaceIE9(bodyHtml);

            $("#dailyWLBySport").find("tr:not(.header)").remove();
            $("#dailyWLBySport").append(bodyHtml);
        }
        else {
            $("#dailyWLBySport").find("tr:not(.header)").remove();
            $("#dailyWLBySport").append('<tr><td colspan="7" class="b c">There is no information available.</td></tr>');
        }
    },
    renderDailyWLRowBySport: function (dwlRow, index) {
        var html = "";
        switch (dwlRow.RowTypeDWL2h.Id) {
            case 0:
                html = HorseRacingReportManager.normalRowHtmlBySport(dwlRow, index);
                break;
            case 9:
                html = HorseRacingReportManager.emptyRowHtml(dwlRow);
                break;
        }
        return html;
    },
    normalRowHtmlBySport: function (row, index) {
        var html = '';
        html = '<tr class="' + (index % 2 == 0 ? 'evenrow' : '') + '">\
                        <td class="l"><span class="blue b" onclick="HorseRacingReportManager.getDailyWLBySportBettype(\'' + row.Id + '\',\'' + row.Name + '\',\'' + HorseRacingReportManager.sportTypeId + '\');">' + row.Name + '</span></td>\
                        <td class="r"><span class="' + (row.WinLoss.toFixed(2) <= 0 ? '' : 'red') + '">' + StringFormatter.formatNumber(row.WinLoss) + '</span></td>\
                        <td class="r"><span class="' + (row.Position.toFixed(2) <= 0 ? '' : 'red') + '">' + StringFormatter.formatNumber(row.Position) + '</span></td>\
                        <td class="r"><span class="' + (row.Commission.toFixed(2) <= 0 ? '' : 'red') + '">' + StringFormatter.formatNumber(row.Commission) + '</span></td>\
                        <td class="r"><span class="' + (row.LineTotal.toFixed(2) <= 0 ? '' : 'red') + '">' + StringFormatter.formatNumber(row.LineTotal) + '</span></td>\
                        <td class="r"><span class="' + (row.Turnover.toFixed(2) <= 0 ? '' : 'red') + '">' + StringFormatter.formatNumber(row.Turnover) + '</span></td>\
                        <td class="r"><span class="' + ((row.Turnover.toFixed(2) == 0 || row.Margin.toFixed(2) <= 0) ? '' : 'red') + '">' + (row.Turnover.toFixed(2) != 0 ? StringFormatter.formatNumber(row.Margin) + "%" : "N/A") + '</span></td>\
                    </tr>';
        return html;
    },
    getDailyWLBySportBettype: function (bettype, bettypename, sporttype) {
        if (!isAllowReadHorseRacingReport) {
            return;
        }
        var data = {
            param:
            {
                ReportDate: HorseRacingReportManager.reportDate,
                SportType: sporttype,
                BetType: bettype
            }
        };
        try { parent.BlockUI(); } catch (e) { }
        DailyWinLoss2hManager.disableReportButton();
        HorseRacingReportManager.bettype = bettype;
        HorseRacingReportManager.bettypeName = bettypename;
        $.ajax({
            type: "POST",
            url: window.rootUrl + "DailyWinLoss2h/DailyWLDirectRacingBySportDetails",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: JSON.stringify(data),
            success: function (rs) {
                if ($("#InformationMssg").html() != "") {
                    $("#InformationMssg").hide();
                    $("#InformationMssg").empty();
                }

                $("#gridtitlebettype").html(HorseRacingReportManager.title + ">" + bettypename);
                HorseRacingReportManager.renderDailyWLReportSportBettype(rs);
            },
            error: function () {
                $("#tblbodyscrollbettype").find("tr:not(.header)").remove();
                $("#tblbodyscrollbettype").append('<tr><td colspan="7" class="b c"><span style="">There is no information available.</span></td></tr>');
                HorseRacingReportManager.showRacingBettypeReport(true);
                $("#gridbettype").removeClass('hidden');
            },
            complete: function () {
                DailyWinLoss2hManager.enableReportButton();
                try { parent.jQuery.unblockUI(); } catch (e) { }
            }
        });
    },
    renderDailyWLReportSportBettype: function (dailyWLResult) {
        if (dailyWLResult && dailyWLResult.ReportDetails && dailyWLResult.ReportDetails.length > 0) {
            var arrBody = [];
            var arrFooter = [];
            var items = dailyWLResult.ReportDetails;
            var itemTotal = items.length;
            for (var i = 0; i < itemTotal; i++) {
                arrBody.push(HorseRacingReportManager.renderDailyWLRowSportDetails(items[i], i));
            }
            arrFooter.push(HorseRacingReportManager.renderDailyWLBettypeGrandTotal(dailyWLResult.ReportTotal.NormalTotal));
            var bodyHtml = arrBody.join('');
            bodyHtml = StringFormatter.removeSpaceIE9(bodyHtml);

            var footerHtml = arrFooter.join('');
            footerHtml = StringFormatter.removeSpaceIE9(footerHtml);

            $("#tblbodyscrollbettype").find("tr:not(.header)").remove();
            $("#tblbodyscrollbettype").append(bodyHtml);
            $("#tblbodyscrollbettype").append(footerHtml);
        }
        else {
            $("#gridbettype").find("tr:not(.header)").remove();
            $("#tblbodyscrollbettype").append('<tr><td colspan="7" class="b c"><div style="">There is no information available.</div></td></tr>');
        }
        HorseRacingReportManager.showRacingBettypeReport(true);
        $("#gridbettype").removeClass("hidden");
    },
    renderDailyWLRowSportDetails: function (dwlRow, index) {
        var html = "";
        switch (dwlRow.RowTypeDWL2h.Id) {
            case 0:
                html = HorseRacingReportManager.normalRowHtmlSportDetails(dwlRow, index);
                break;
            case 3:
                html = HorseRacingReportManager.sportTotalRowHtmlSportDetails(dwlRow);
                break;
            case 9:
                html = HorseRacingReportManager.emptyRowHtml(dwlRow);
                break;
            case 10:
                html = HorseRacingReportManager.leagueHeadLineHtml(dwlRow);
                break;
        }
        return html;
    },
    leagueHeadLineHtml: function (row) {
        var html = '';
        html = '<tr><td class="l" colspan="7"><span class="b longtext-wrapper" style="">' +
                        row.Name + '</span></td></tr>';

        return html;
    },
    normalRowHtmlSportDetails: function (row, index) {
        var html = '';
        html = '<tr class="' + (row.RowIndex % 2 == 0 ? 'evenrow' : '') + '">\
                        <td class="b c"><span>' + row.Name + '<span></td>\
                        <td class="r"><span class="' + (row.WinLoss.toFixed(2) <= 0 ? '' : 'red') + '">' + StringFormatter.formatNumber(row.WinLoss) + '</span></td>\
                        <td class="r"><span class="' + (row.Position.toFixed(2) <= 0 ? '' : 'red') + '">' + StringFormatter.formatNumber(row.Position) + '</span></td>\
                        <td class="r"><span class="' + (row.Commission.toFixed(2) <= 0 ? '' : 'red') + '">' + StringFormatter.formatNumber(row.Commission) + '</span></td>\
                        <td class="r"><span class="' + (row.LineTotal.toFixed(2) <= 0 ? '' : 'red') + '">' + StringFormatter.formatNumber(row.LineTotal) + '</span></td>\
                        <td class="r"><span class="' + (row.Turnover.toFixed(2) <= 0 ? '' : 'red') + '">' + StringFormatter.formatNumber(row.Turnover) + '</span></td>\
                        <td class="r"><span class="' + ((row.Turnover.toFixed(2) == 0 || row.Margin.toFixed(2) <= 0) ? '' : 'red') + '">' + (row.Turnover.toFixed(2) != 0 ? StringFormatter.formatNumber(row.Margin) + "%" : "N/A") + '</span></td>\
                    </tr>';
        return html;
    },
    sportTotalRowHtmlSportDetails: function (row) {
        var html = '';
        html = '<tr class="">\
                        <td class="r b"><span>' + row.Name + '</span></td>\
                        <td class="r"><span class="' + (row.WinLoss.toFixed(2) <= 0 ? '' : 'red') + '">' + StringFormatter.formatNumber(row.WinLoss) + '</span></td>\
                        <td class="r"><span class="' + (row.Position.toFixed(2) <= 0 ? '' : 'red') + '">' + StringFormatter.formatNumber(row.Position) + '</span></td>\
                        <td class="r"><span class="' + (row.Commission.toFixed(2) <= 0 ? '' : 'red') + '">' + StringFormatter.formatNumber(row.Commission) + '</span></td>\
                        <td class="linetotal r b"><span class="' + (row.LineTotal.toFixed(2) <= 0 ? '' : 'red') + '">' + StringFormatter.formatNumber(row.LineTotal) + '</span></td>\
                        <td class="r b"><span class="' + (row.Turnover.toFixed(2) <= 0 ? '' : 'red') + '">' + StringFormatter.formatNumber(row.Turnover) + '</span></td>\
                        <td class="r"><span class="' + ((row.Turnover.toFixed(2) == 0 || row.Margin.toFixed(2) <= 0) ? '' : 'red') + '">' + (row.Turnover.toFixed(2) != 0 ? StringFormatter.formatNumber(row.Margin) + "%" : "N/A") + '</span></td>\
                    </tr>';
        return html;
    },
    renderDailyWLBettypeGrandTotal: function (row) {
        var html = '';
        html = '<tr class="title">\
                        <td class="r b"><span class="">Grand Total:</span></td>\
                        <td class="r"><span class="' + (row.WinLoss.toFixed(2) <= 0 ? '' : 'red') + '">' + StringFormatter.formatNumber(row.WinLoss) + '</span></td>\
                        <td class="r"><span class="' + (row.Position.toFixed(2) <= 0 ? '' : 'red') + '">' + StringFormatter.formatNumber(row.Position) + '</span></td>\
                        <td class="r"><span class="' + (row.Commission.toFixed(2) <= 0 ? '' : 'red') + '">' + StringFormatter.formatNumber(row.Commission) + '</span></td>\
                        <td class="linetotal r b"><span class="' + (row.LineTotal.toFixed(2) <= 0 ? '' : 'red') + '">' + StringFormatter.formatNumber(row.LineTotal) + '</span></td>\
                        <td class="r b"><span class="' + (row.Turnover.toFixed(2) <= 0 ? '' : 'red') + '">' + StringFormatter.formatNumber(row.Turnover) + '<span></td>\
                        <td class="r"><span class="' + ((row.Turnover.toFixed(2) == 0 || row.Margin.toFixed(2) <= 0) ? '' : 'red') + '">' + (row.Turnover.toFixed(2) != 0 ? StringFormatter.formatNumber(row.Margin) + "%" : "N/A") + '</span></td>\
                    </tr>'
        return html;
    },
    showMainReport: function (isshow) {
        var dailyWLBySport = $("#dailyWLBySport");
        var dailyWLRacing = $("#dailyWLRacing");
        if (isshow) {
            dailyWLRacing.removeClass("hidden");
            dailyWLBySport.addClass("hidden");
            HorseRacingReportManager.showRacingBettypeReport(false);
        }
        else {
            dailyWLBySport.removeClass("hidden");
            dailyWLRacing.addClass("hidden");
        }
        //        $("#leftPane").css("width", "620px").show();
        //        HorseRacingReportManager.resize_spliter();
    },
    showRacingBettypeReport: function (isshow) {
        if (isshow) {
            $("#gridSplitter_HRR").removeClass('hidden');
            $("#gridbettype").removeClass("hidden");
            $("#horseRacingReport").css("width", "1500px");
            var totalWidth = $("#horseRacingReportTable").width() + 10;
            $("#horseRacingReport").css("width", totalWidth + "px");
        }
        else {
            $("#gridSplitter_HRR").addClass("hidden");
            $("#gridbettype").addClass("hidden");
            $("#horseRacingReport").css("width", "100%");
        }
    },
    exportToExcel: function (reportType) {
        if (!isAllowReadHorseRacingReport) {
            return;
        }
        var url = "DailyWinLoss2h?";
        switch (reportType) {
            case "1": // HorseRacingMain
                url += "&ExportType=HorseRacingMain";
                url += "&ReportDate=" + HorseRacingReportManager.reportDate;
                break;
            case "2": // HorseRacingSporttype
                url += "&ExportType=HorseRacingSporttype";
                url += "&ReportDate=" + HorseRacingReportManager.reportDate;
                break;
            case "3": // HorseRacingSportBettype
                url += "&ExportType=HorseRacingSportBettype";
                url += "&ReportDate=" + HorseRacingReportManager.reportDate;
                url += "&SportType=" + HorseRacingReportManager.sportTypeId;
                break;
            case "4": // HorseRacingSportBettypeDetails
                url += "&ExportType=HorseRacingSportBettypeDetails";
                url += "&ReportDate=" + HorseRacingReportManager.reportDate;
                url += "&SportType=" + HorseRacingReportManager.sportTypeId;
                url += "&BetType=" + HorseRacingReportManager.bettype;
                break;
        }
        $('#frm').attr('src', url);
    },
    initGridSplitter: function () {
        if (HorseRacingReportManager.isFirstLoad) {
            HorseRacingReportManager.resize_spliter();
            $("#splitterContainer").splitter({ minAsize: 10, splitVertical: true, A: $('#leftPane'), B: $('#rightPane'), closeableto: 0 });
            $(".splitbarV").addClass("hidden");
            $(window).resize(function () {
                HorseRacingReportManager.resize_spliter();
            });
            HorseRacingReportManager.isFirstLoad = false;
        }
    },
    resize_spliter: function () {
        if ($("#leftPane").width() != 0) {
            $("#leftPane").css("width", "620px").show();
        }
        $("#horseRacingReport").css("width", $(window).width() + "px");
        var splittercontainerheight = $(window).height() - 70;
        $("#splitterContainer").css("height", splittercontainerheight + "px");
        var rightPaneWidth = $(window).width() - $("#leftPane").width() - $(".splitbarV").width() - 5;
        $("#rightPane").css("width", rightPaneWidth + "px");

    },
    backToMain: function () {
        WL2hShowHideDivManager.showMainReportFromHRR(HorseRacingReportManager.sportTypeId, HorseRacingReportManager.reportDate);
    }
}
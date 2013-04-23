

var LiveCasinoReportManager = {
    reportDate: null,
    sportTypeId: null,
    bettype: null,
    path: "Live Casino",
    backToMainMenuText: "Back to Main Menu",
    title: "",
    initGrid: function (sportId, wldate, bettype) {
        LiveCasinoReportManager.sportTypeId = sportId;
        LiveCasinoReportManager.reportDate = wldate;
        LiveCasinoReportManager.bettype = bettype;

        $("#btnReport").unbind("click");
        $("#btnExport").unbind("click");

        $("#btnReport").click(function () {
            var reportDate = $("#txtfromdate").val();
            LiveCasinoReportManager.viewReport(reportDate);
        });

        $("#btnExport").click(function () {
            LiveCasinoReportManager.exportToExcel();
        });

        $('#spMoreElement').empty();
        var strtoday = '<input type="button" onclick="LiveCasinoReportManager.viewReportByDate(1)" id="btnToday" name="Today" class="btn"/>';
        var stryesterday = '<input type="button" onclick="LiveCasinoReportManager.viewReportByDate(2)" id="btnYesterday" name="Yesterday" class="btn"/>';
        $('#spMoreElement').append(strtoday);
        $('#spMoreElement').append(stryesterday);
        $('#btnToday').val(today);
        $('#btnYesterday').val(yesterday);
        var fullpath = "<b><a onclick=\"WL2hShowHideDivManager.showMainReportFromLCR('" + sportId +
                         "','" + wldate + "')\"> Main </a> > " + LiveCasinoReportManager.path + "</b>";
        $("#trPath").html(fullpath);
        $("#backToMainMenu_LCR").html(LiveCasinoReportManager.backToMainMenuText);
        $("#backToMainMenu_LCR").unbind("click");
        $("#backToMainMenu_LCR").click(function () {
            WL2hShowHideDivManager.showMainReportFromLCR(sportId, wldate);
        });

        LiveCasinoReportManager.me = $("#liveCasinoReport");
        LiveCasinoReportManager.tb = $("#tblContent_LCR");

        $("#dailyWLReportDiv").removeClass("hidden");
        try { parent.jQuery.unblockUI(); } catch (e) { }
        LiveCasinoReportManager.viewReport(wldate);
    },
    viewReport: function (wldate) {
        LiveCasinoReportManager.getcontent(wldate, MixParlayReportManager.bettype);
    },
    viewReportByDate: function (date) {
        switch (date) {
            case 1:
                $("#txtfromdate").datepicker("setDate", new Date($('#btnToday').val()));
                LiveCasinoReportManager.getcontent($('#btnToday').val(),
                        MixParlayReportManager.bettype);
                break;
            case 2:
                $("#txtfromdate").datepicker("setDate", new Date($('#btnYesterday').val()));
                LiveCasinoReportManager.getcontent($('#btnYesterday').val(),
                        MixParlayReportManager.bettype);
                break;
            default:
                break;
        }
    },
    getcontent: function (wldate, bettype) {
        if (!isAllowReadLiveCasinoReport) {
            return;
        }

        LiveCasinoReportManager.reportDate = wldate;
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
            url: window.rootUrl + "DailyWinLoss2h/SelLiveCasinoReport",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: JSON.stringify(data),
            success: function (rs) {
                if ($("#InformationMssg").html() != "") {
                    $("#InformationMssg").hide();
                    $("#InformationMssg").empty();
                }
                LiveCasinoReportManager.me.addClass('hidden');
                LiveCasinoReportManager.renderDailyWLReport(rs, wldate);
            },
            error: function () {
                LiveCasinoReportManager.title = "LIVE CASINO PRODUCT WIN/LOSS REPORT";
                $("#liveCasinoReportHeader").html(LiveCasinoReportManager.title);
                LiveCasinoReportManager.renderNoData();
            },
            complete: function () {
                DailyWinLoss2hManager.enableReportButton();
                try { parent.jQuery.unblockUI(); } catch (e) { }
            }
        });
    },
    renderDailyWLReport: function (dailyWLResult, wldate) {
        LiveCasinoReportManager.title = "LIVE CASINO PRODUCT WIN/LOSS REPORT";
        $("#liveCasinoReportHeader").html(LiveCasinoReportManager.title);
        var items = dailyWLResult.ReportDetails;
        if (items != null && items.length > 0) {
            var arrBody = [];
            var arrFooter = [];
            var numberItems = items.length;
            for (var i = 0; i < numberItems; i++) {
                arrBody.push(LiveCasinoReportManager.renderMainRow(items[i], (i % 2 == 0 ? "bgeven" : "")));
            }
            arrBody.push(LiveCasinoReportManager.renderEmptyRow());
            arrFooter.push(LiveCasinoReportManager.renderTotalRow(dailyWLResult.ReportTotal.NormalTotal));

            var bodyHtml = arrBody.join('');
            bodyHtml = StringFormatter.removeSpaceIE9(bodyHtml);
            LiveCasinoReportManager.tb.find("tr:not(.header)").remove();
            LiveCasinoReportManager.tb.append(bodyHtml);

            var footerHtml = arrFooter.join('');
            footerHtml = StringFormatter.removeSpaceIE9(footerHtml);
            LiveCasinoReportManager.tb.append(footerHtml);
            LiveCasinoReportManager.createScroll();
        }
        else {
            LiveCasinoReportManager.renderNoData();
        }
    },
    renderMainRow: function (row, cssClass) {
        var html = '';
        html = '<tr class="' + cssClass + '">\
                <td><div class="col120 l b">' + row.Name + '</div></td>\
                <td><div class="col100 r ' + (row.WinLoss.toFixed(2) > 0 ? 'red' : '') + '">' + StringFormatter.formatNumber(row.WinLoss) + '</div></td>\
                <td><div class="col90 r">' + StringFormatter.formatNumber(row.Position) + '</div></td>\
                <td><div class="col80 r">' + StringFormatter.formatNumber(row.Commission) + '</div></td>\
                <td><div class="col100 r ' + (row.LineTotal.toFixed(2) > 0 ? 'red' : '') + '">' + StringFormatter.formatNumber(row.LineTotal) + '</div></td>\
                <td><div class="col100 r">' + StringFormatter.formatNumber(row.Turnover) + '</div></td>\
                <td><div class="col80 r ' + (row.Turnover.toFixed(2) != 0 && row.Margin.toFixed(2) > 0 ? 'red' : '') + '">' + (row.Turnover.toFixed(2) != 0 ? StringFormatter.formatNumber(row.Margin) + '%' : 'N/A') + '</div></td>\
            </tr>';
        return html;
    },
    renderTotalRow: function (row) {
        var html = '';
        html = '<tr>\
                <td><div class="col120 r b">' + row.Name + '</div></td>\
                <td><div class="col100 r b ' + (row.WinLoss.toFixed(2) > 0 ? 'red' : '') + '">' + StringFormatter.formatNumber(row.WinLoss) + '</div></td>\
                <td><div class="col90 r b">' + StringFormatter.formatNumber(row.Position) + '</div></td>\
                <td><div class="col80 r b">' + StringFormatter.formatNumber(row.Commission) + '</div></td>\
                <td style="background-color:#B5C4D0"><div class="col100 r b ' + (row.LineTotal.toFixed(2) > 0 ? 'red' : '') + '">' + StringFormatter.formatNumber(row.LineTotal) + '</div></td>\
                <td><div class="col100 r b">' + StringFormatter.formatNumber(row.Turnover) + '</div></td>\
                <td><div class="col80 r b ' + (row.Turnover.toFixed(2) != 0 && row.Margin.toFixed(2) > 0 ? 'red' : '') + '">' + (row.Turnover.toFixed(2) != 0 ? StringFormatter.formatNumber(row.Margin) + '%' : 'N/A') + '</div></td>\
            </tr>';
        return html;
    },
    createScroll: function () {
        LiveCasinoReportManager.me.removeClass('hidden');
    },
    renderNoData: function () {
        LiveCasinoReportManager.tb.find("tr:not(.header)").remove();
        LiveCasinoReportManager.tb.append('<tr id="noinfo"><td id="tdNoInfomation" colspan="7" class="b c" style="" >There is no information available.</td></tr>');
        LiveCasinoReportManager.me.removeClass('hidden');
    },
    renderEmptyRow: function () {
        var html = '<tr>\
                        <td colspan ="7" class ="c" style="height:18px;"></td>\
                    </tr>';
        return html;
    },
    exportToExcel: function () {
        if (!isAllowReadLiveCasinoReport) {
            return;
        }
        var url = 'DailyWinLoss2h?ExportType=LiveCasino&BetType=' + LiveCasinoReportManager.bettype + '&ReportDate=' +
                    LiveCasinoReportManager.reportDate;
        $('#frm').attr('src', url);
    }
}






var MixParlayReportManager = {
    reportDate: null,
    sportTypeId: null,
    bettype: null,
    path: "Mix Parlay",
    backToMainMenuText: "Back to Main Menu",
    title: "",
    initGrid: function (sportId, wldate, bettype) {
        MixParlayReportManager.sportTypeId = sportId;
        MixParlayReportManager.reportDate = wldate;
        MixParlayReportManager.bettype = bettype;

        $("#btnReport").unbind("click");
        $("#btnExport").unbind("click");

        $("#btnReport").click(function () {
            var reportDate = $("#txtfromdate").val();
            MixParlayReportManager.viewReport(reportDate);
        });

        $("#btnExport").click(function () {
            MixParlayReportManager.exportToExcel();
        });
        $('#spMoreElement').empty();
        var strtoday = '<input type="button" onclick="MixParlayReportManager.viewReportByDate(1)" id="btnToday" name="Today" class="btn"/>';
        var stryesterday = '<input type="button" onclick="MixParlayReportManager.viewReportByDate(2)" id="btnYesterday" name="Yesterday" class="btn"/>';
        $('#spMoreElement').append(strtoday);
        $('#spMoreElement').append(stryesterday);
        $('#btnToday').val(today);
        $('#btnYesterday').val(yesterday);
        var fullpath = "<b><a onclick=\"WL2hShowHideDivManager.showMainReportFromMPR('" + sportId +
                         "','" + wldate + "')\"> Main </a> > " + MixParlayReportManager.path + "</b>";
        $("#trPath").html(fullpath);
        $("#backToMainMenu_MPR").html(MixParlayReportManager.backToMainMenuText);
        $("#backToMainMenu_MPR").unbind("click");
        $("#backToMainMenu_MPR").click(function () {
            WL2hShowHideDivManager.showMainReportFromMPR(sportId, wldate);
        });

        MixParlayReportManager.me = $("#mixParlayReport");
        MixParlayReportManager.tb = $("#tblContent_MPR");

        $("#dailyWLReportDiv").removeClass("hidden");
        try { parent.jQuery.unblockUI(); } catch (e) { }
        MixParlayReportManager.viewReport(wldate);
    },
    viewReport: function (wldate) {
        MixParlayReportManager.getcontent(wldate, MixParlayReportManager.bettype);
    },
    viewReportByDate: function (date) {
        switch (date) {
            case 1:
                $("#txtfromdate").datepicker("setDate", new Date($('#btnToday').val()));
                MixParlayReportManager.getcontent($('#btnToday').val(),
                        MixParlayReportManager.bettype);
                break;
            case 2:
                $("#txtfromdate").datepicker("setDate", new Date($('#btnYesterday').val()));
                MixParlayReportManager.getcontent($('#btnYesterday').val(),
                        MixParlayReportManager.bettype);
                break;
            default:
                break;
        }
    },
    getcontent: function (wldate, bettype) {
        if (!isAllowReadMixParlayReport) {
            return;
        }
        var reportDate = new Date(wldate);
        var settingDate = new Date(dailyWLMP_WebSettings);
        if (settingDate <= reportDate) {
            WL2hShowHideDivManager.showAdvanceMixParlayReportFromMPR(MixParlayReportManager.sportTypeId, wldate);
            return;
        }
        MixParlayReportManager.reportDate = wldate;
        var data = {
            param:
            {
                ReportDate: wldate,
                BetType: bettype
            }
        };
        try { parent.BlockUI(); } catch (e) { }
        DailyWinLoss2hManager.disableReportButton();
        $.ajax({
            type: "POST",
            url: window.rootUrl + "DailyWinLoss2h/DailyWLDirect_MixParlay",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: JSON.stringify(data),
            success: function (rs) {
                if ($("#InformationMssg").html() != "") {
                    $("#InformationMssg").hide();
                    $("#InformationMssg").empty();
                }
                MixParlayReportManager.me.addClass('hidden');
                MixParlayReportManager.renderDailyWLReport(rs, wldate);
            },
            error: function () {
                MixParlayReportManager.title = 'MIX PARLAY PRODUCT WIN/LOSS REPORT';
                $("#mixParlayReportHeader").html(MixParlayReportManager.title);
                MixParlayReportManager.renderNoData();
            },
            complete: function () {
                DailyWinLoss2hManager.enableReportButton();
                try { parent.jQuery.unblockUI(); } catch (e) { }
            }
        });
    },
    createScroll: function () {
        MixParlayReportManager.me.removeClass('hidden');
    },
    renderDailyWLReport: function (dailyWLResult, wldate) {
        var reportDate = new Date(wldate);
        MixParlayReportManager.title = 'MIX PARLAY PRODUCT WIN/LOSS REPORT';
        $("#mixParlayReportHeader").html(MixParlayReportManager.title);

        if (dailyWLResult && dailyWLResult.ReportDetails && dailyWLResult.ReportDetails.length > 0) {
            var arrBody = [];
            var arrFooter = [];
            var items = dailyWLResult.ReportDetails;
            for (var i = 0; i < items.length; i++) {
                arrBody.push(MixParlayReportManager.renderDailyWLRow(items[i], i, wldate));
            }
            arrFooter.push(MixParlayReportManager.renderReportFooter(dailyWLResult));

            var bodyHtml = arrBody.join('');
            bodyHtml = StringFormatter.removeSpaceIE9(bodyHtml);
            MixParlayReportManager.tb.find("tr:not(.header)").remove();
            MixParlayReportManager.tb.append(bodyHtml);

            var footerHtml = arrFooter.join('');
            footerHtml = StringFormatter.removeSpaceIE9(footerHtml);
            MixParlayReportManager.tb.append(footerHtml);
            MixParlayReportManager.createScroll();
        }
        else {
            MixParlayReportManager.renderNoData();
        }
    },
    renderDailyWLRow: function (dwlRow, index, wldate) {
        var html = "";
        switch (dwlRow.RowTypeDWL2h.Id) {
            case 0:
                html = MixParlayReportManager.renderNormalWLRow(dwlRow, index);
                break;
            case 3:
                html = MixParlayReportManager.sportTotalRowHtml(dwlRow, wldate);
                break;
            case 9:
                html = MixParlayReportManager.emptyRowHtml(dwlRow);
                break;
        }
        return html;
    },
    renderNormalWLRow: function (dwlRow, index) {
        var html = "";
        if (dwlRow) {
            html = '<tr class="' + (index % 2 == 0 ? "evenrow" : "") + '" >\
                        <td>\
                            <div class="col160 b l">' + dwlRow.Name + '</div>\
                        </td>\
                        <td>\
                            <div class="col100 r">' + StringFormatter.getDisplayColorOfValue(dwlRow.WinLoss) + '</div>\
                        </td>\
                        <td>\
                            <div class="col90 r">' + StringFormatter.formatNumber(dwlRow.Position) + '</div>\
                        </td>\
                         <td>\
                            <div class="col80 r">' + StringFormatter.formatNumber(dwlRow.Commission) + '</div>\
                        </td>\
                         <td>\
                            <div class="col100 r">' + StringFormatter.getDisplayColorOfValue(dwlRow.LineTotal) + '</div>\
                        </td>\
                         <td>\
                            <div class="col100 r">' + StringFormatter.formatNumber(dwlRow.Turnover) + '</div>\
                        </td>\
                         <td>\
                            <div class="col80 r ' + (dwlRow.Turnover.toFixed(2) != 0 && dwlRow.Margin.toFixed(2) > 0 ? 'red' : '') + '" style="">' +
                             (dwlRow.Turnover.toFixed(2) != 0 ? StringFormatter.formatNumber(dwlRow.Margin) + '%' : 'N/A') + '</div>\
                        </td>\
                    </tr>';
        }
        return html;
    },
    sportTotalRowHtml: function (dwlRow, wldate) {
        var html = "";
        if (dwlRow) {
            html = '<tr class="total2" style="font-weight:bold; background-color:#d0e8fb;">\
                        <td>\
                            <div class="col160 b l">' + dwlRow.Name + '</div>\
                        </td>\
                        <td>\
                            <div class="col100 r">' + StringFormatter.getDisplayColorOfValue(dwlRow.WinLoss) + '</div>\
                        </td>\
                        <td>\
                            <div class="col90 r">' + StringFormatter.formatNumber(dwlRow.Position) + '</div>\
                        </td>\
                         <td>\
                            <div class="col80 r">' + StringFormatter.formatNumber(dwlRow.Commission) + '</div>\
                        </td>\
                         <td>\
                            <div class="col100 r">' + StringFormatter.getDisplayColorOfValue(dwlRow.LineTotal) + '</div>\
                        </td>\
                         <td>\
                            <div class="col100 r">' + StringFormatter.formatNumber(dwlRow.Turnover) + '</div>\
                        </td>\
                         <td>\
                            <div class="col80 r ' + (dwlRow.Turnover.toFixed(2) != 0 && dwlRow.Margin.toFixed(2) > 0 ? 'red' : '') + '" style="">' +
                              (dwlRow.Turnover.toFixed(2) != 0 ? StringFormatter.formatNumber(dwlRow.Margin) + '%' : 'N/A') + '</div>\
                        </td>\
                    </tr>';
        }
        return html;
    },
    renderReportFooter: function (dailyWLReport) {
        var arr = [];
        arr.push(MixParlayReportManager.renderAllTotalRF(dailyWLReport.ReportTotal.NormalTotal));
        return arr.join('');
    },
    renderAllTotalRF: function (total) {
        var html;
        if (total) {
            lineTotalColor = "#B5C4D0";
            html = '<tr>\
                        <td>\
                            <div class="col160 r b">All Total:</div>\
                        </td>\
                        <td>\
                            <div class="col100 r" style="font-weight: normal;">' + StringFormatter.getDisplayColorOfValue(total.WinLoss) + '</div>\
                        </td>\
                        <td>\
                            <div class="col90 r" style="font-weight: normal;">' + StringFormatter.formatNumber(total.Position) + '</div>\
                        </td>\
                         <td>\
                            <div class="col80 r" style="font-weight: normal;">' + StringFormatter.formatNumber(total.Commission) + '</div>\
                        </td>\
                         <td style="background-color: ' + lineTotalColor + '">\
                            <div class="col100 r">' + StringFormatter.getDisplayColorOfValue(total.LineTotal) + '</div>\
                        </td>\
                         <td>\
                            <div class="col100 r" style="">' + StringFormatter.formatNumber(total.Turnover) + '</div>\
                        </td>\
                         <td>\
                            <div class="col80 r ' + (total.Turnover.toFixed(2) != 0 && total.Margin.toFixed(2) > 0 ? 'red' : '') + '" style="font-weight: normal;">' +
                            (total.Turnover.toFixed(2) != 0 ? StringFormatter.formatNumber(total.Margin) + '%' : 'N/A') + '</div>\
                        </td>\
                    </tr>';
        }
        return html;
    },
    getSportLink: function (normalWLItem, wldate) {
        var sportLink = "";
        switch (normalWLItem.Sporttype.Id) {
            case 1:
                sportLink = "<b>Soccer Sport Total</b>";
                break;
            case 98:
                sportLink = "<b>Multi-Sport Total</b>";
                break;
            default:
                sportLink = "<b>Others Sport Total</b>";
                break;
        }
        return sportLink;
    },
    renderNoData: function () {
        MixParlayReportManager.tb.find("tr:not(.header)").remove();
        MixParlayReportManager.tb.append('<tr id="noinfo"><td id="tdNoInfomation" colspan="7" class="b c" style="" >There is no information available.</td></tr>');
        MixParlayReportManager.me.removeClass('hidden');
    },
    emptyRowHtml: function (dwlRow) {
        var html = '<tr>\
                        <td colspan ="7" class ="c" style="height:18px;"></td>\
                    </tr>';
        return html;
    },
    exportToExcel: function () {
        if (!isAllowRead2HOReport) {
            return;
        }
        var url = 'DailyWinLoss2h?ExportType=NormalMixParlay&BetType=' + MixParlayReportManager.bettype + '&ReportDate=' +
                    MixParlayReportManager.reportDate;
        $('#frm').attr('src', url);
    }
}
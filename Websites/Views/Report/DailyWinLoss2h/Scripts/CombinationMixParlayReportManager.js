



var CombinationMixParlayReportManager = {
    reportDate: null,
    sportTypeId: null,
    bettype: null,
    path: "System Parlay",
    backToMainMenuText: "Back to Main Menu",
    initGrid: function (sportId, wldate, bettype) {
        CombinationMixParlayReportManager.sportTypeId = sportId;
        CombinationMixParlayReportManager.reportDate = wldate;
        CombinationMixParlayReportManager.bettype = bettype;

        $("#btnReport").unbind("click");
        $("#btnExport").unbind("click");

        $("#btnReport").click(function () {
            var reportDate = $("#txtfromdate").val();
            CombinationMixParlayReportManager.viewReport(reportDate);
        });

        $("#btnExport").click(function () {
            CombinationMixParlayReportManager.exportToExcel();
        });
        $('#spMoreElement').empty();
        var strtoday = '<input type="button" onclick="CombinationMixParlayReportManager.viewReportByDate(1)" id="btnToday" name="Today" class="btn"/>';
        var stryesterday = '<input type="button" onclick="CombinationMixParlayReportManager.viewReportByDate(2)" id="btnYesterday" name="Yesterday" class="btn"/>';
        $('#spMoreElement').append(strtoday);
        $('#spMoreElement').append(stryesterday);
        $('#btnToday').val(today);
        $('#btnYesterday').val(yesterday);
        var fullpath = "<b><a onclick=\"WL2hShowHideDivManager.showMainReportFromCMPR('" + sportId +
                         "','" + wldate + "')\"> Main </a> > " + CombinationMixParlayReportManager.path + "</b>";
        $("#trPath").html(fullpath);
        $("#backToMainMenu_CMPR").html(CombinationMixParlayReportManager.backToMainMenuText);
        $("#backToMainMenu_CMPR").unbind("click");
        $("#backToMainMenu_CMPR").click(function () {
            WL2hShowHideDivManager.showMainReportFromCMPR(sportId, wldate);
        });

        CombinationMixParlayReportManager.me = $("#combinationMixParlayReport");
        CombinationMixParlayReportManager.tb = $("#tblContent_Cmpr");

        $("#dailyWLReportDiv").removeClass("hidden");
        try { parent.jQuery.unblockUI(); } catch (e) { }
        CombinationMixParlayReportManager.viewReport(wldate);
    },
    viewReport: function (wldate) {
        CombinationMixParlayReportManager.getcontent(wldate, CombinationMixParlayReportManager.bettype);
    },
    viewReportByDate: function (date) {
        switch (date) {
            case 1:
                $("#txtfromdate").datepicker("setDate", new Date($('#btnToday').val()));
                CombinationMixParlayReportManager.getcontent($('#btnToday').val(),
                        CombinationMixParlayReportManager.bettype);
                break;
            case 2:
                $("#txtfromdate").datepicker("setDate", new Date($('#btnYesterday').val()));
                CombinationMixParlayReportManager.getcontent($('#btnYesterday').val(),
                        CombinationMixParlayReportManager.bettype);
                break;
            default:
                break;
        }
    },
    getcontent: function (wldate, bettype) {
        if (!isAllowRead2HOReport) {
            return;
        }
        CombinationMixParlayReportManager.reportDate = wldate;
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
                CombinationMixParlayReportManager.me.addClass('hidden');
                CombinationMixParlayReportManager.renderDailyWLReport(rs, wldate);
            },
            error: function () {
                CombinationMixParlayReportManager.renderNoData();
            },
            complete: function () {
                DailyWinLoss2hManager.enableReportButton();
                try { parent.jQuery.unblockUI(); } catch (e) { }
            }
        });
    },
    createScroll: function () {
        CombinationMixParlayReportManager.me.removeClass('hidden');
    },
    renderDailyWLReport: function (dailyWLResult, wldate) {
        if (dailyWLResult && dailyWLResult.ReportDetails && dailyWLResult.ReportDetails.length > 0) {
            var arrBody = [];
            var arrFooter = [];
            var items = dailyWLResult.ReportDetails;
            for (var i = 0; i < items.length; i++) {
                arrBody.push(CombinationMixParlayReportManager.renderDailyWLRow(items[i], wldate));
            }
            arrFooter.push(CombinationMixParlayReportManager.renderReportFooter(dailyWLResult));

            var bodyHtml = arrBody.join('');
            bodyHtml = StringFormatter.removeSpaceIE9(bodyHtml);
            CombinationMixParlayReportManager.tb.find("tr:not(.header)").remove();
            CombinationMixParlayReportManager.tb.append(bodyHtml);

            var footerHtml = arrFooter.join('');
            footerHtml = StringFormatter.removeSpaceIE9(footerHtml);
            CombinationMixParlayReportManager.tb.append(footerHtml);

            CombinationMixParlayReportManager.createScroll();
        }
        else {
            CombinationMixParlayReportManager.renderNoData();
        }
    },
    renderDailyWLRow: function (dwlRow, wldate) {
        var html = "";
        switch (dwlRow.RowTypeDWL2h.Id) {
            case 0:
                html = CombinationMixParlayReportManager.normalRowHtml(dwlRow, wldate);
                break;
            case 3:
                html = CombinationMixParlayReportManager.sportTotalRowHtml(dwlRow, wldate);
                break;
            case 9:
                html = CombinationMixParlayReportManager.emptyRowHtml(dwlRow, wldate);
                break;
        }
        return html;
    },
    normalRowHtml: function (dwlRow, wldate) {
        var html = "";
        if (dwlRow) {
            html = '<tr class="" >\
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
    emptyRowHtml: function (dwlRow, wldate) {
        var html = '<tr>\
                        <td colspan ="7" class ="c" style="height:18px;"></td>\
                    </tr>';
        return html;
    },
    renderReportFooter: function (dailyWLReport) {
        var arr = [];
        arr.push(CombinationMixParlayReportManager.renderAllTotalRF(dailyWLReport.ReportTotal.NormalTotal));
        return arr.join('');
    },
    renderAllTotalRF: function (total) {
        var html;
        if (total) {
            //            lineTotalColor = "#FFFFFF";
            //            lineTotalColor = "#8ac9db";
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
    renderNoData: function () {
        CombinationMixParlayReportManager.tb.find("tr:not(.header)").remove();
        CombinationMixParlayReportManager.tb.append('<tr id="noinfo"><td id="tdNoInfomation" colspan="7" class="b c" style="width:766px;" >There is no information available.</td></tr>');
        CombinationMixParlayReportManager.me.removeClass('hidden');
    },
    exportToExcel: function () {
        if (!isAllowRead2HOReport) {
            return;
        }
        var url = 'DailyWinLoss2h?ExportType=CombinationMixParlayMain&BetType=' + CombinationMixParlayReportManager.bettype + '&ReportDate=' +
                    CombinationMixParlayReportManager.reportDate;
        $('#frm').attr('src', url);

    }
}


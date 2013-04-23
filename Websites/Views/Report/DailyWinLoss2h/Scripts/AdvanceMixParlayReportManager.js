

var AdvanceMixParlayReportManager = {
    reportDate: null,
    sportTypeId: null,
    bettype: null,
    path: "Mix Parlay",
    backToMainMenuText: "Back to Main Menu",
    title: "",
    initGrid: function (sportId, wldate, bettype) {
        AdvanceMixParlayReportManager.sportTypeId = sportId;
        AdvanceMixParlayReportManager.reportDate = wldate;
        AdvanceMixParlayReportManager.bettype = bettype;

        $("#btnReport").unbind("click");
        $("#btnExport").unbind("click");
        $("#btnReport").click(function () {
            var reportDate = $("#txtfromdate").val();
            AdvanceMixParlayReportManager.viewReport(reportDate);
        });
        $("#btnExport").click(function () {
            AdvanceMixParlayReportManager.exportToExcel();
        });

        $('#spMoreElement').empty();
        var strtoday = '<input type="button" onclick="AdvanceMixParlayReportManager.viewReportByDate(1)" id="btnToday" name="Today" class="btn"/>';
        var stryesterday = '<input type="button" onclick="AdvanceMixParlayReportManager.viewReportByDate(2)" id="btnYesterday" name="Yesterday" class="btn"/>';
        $('#spMoreElement').append(strtoday);
        $('#spMoreElement').append(stryesterday);
        $('#btnToday').val(today);
        $('#btnYesterday').val(yesterday);

        var fullpath = "<b><a onclick=\"WL2hShowHideDivManager.showMainReportFromAMPR('" + sportId +
                         "','" + wldate + "')\"> Main </a> > " + AdvanceMixParlayReportManager.path + "</b>";
        $("#trPath").html(fullpath);
        $("#backToMainMenu_AMPR").html(AdvanceMixParlayReportManager.backToMainMenuText);
        $("#backToMainMenu_AMPR").unbind("click");
        $("#backToMainMenu_AMPR").click(function () {
            WL2hShowHideDivManager.showMainReportFromAMPR(sportId, wldate);
        });

        AdvanceMixParlayReportManager.me = $("#advanceMixParlayReport");
        AdvanceMixParlayReportManager.tb = $("#tblContent_AMPR");

        $("#dailyWLReportDiv").removeClass("hidden");
        try { parent.jQuery.unblockUI(); } catch (e) { }
        AdvanceMixParlayReportManager.viewReport(wldate);
    },
    viewReport: function (wldate) {
        AdvanceMixParlayReportManager.getcontent(wldate);
    },
    viewReportByDate: function (date) {
        switch (date) {
            case 1:
                $("#txtfromdate").datepicker("setDate", new Date($('#btnToday').val()));
                AdvanceMixParlayReportManager.getcontent($('#btnToday').val());
                break;
            case 2:
                $("#txtfromdate").datepicker("setDate", new Date($('#btnYesterday').val()));
                AdvanceMixParlayReportManager.getcontent($('#btnYesterday').val());
                break;
            default:
                break;
        }
    },
    getcontent: function (wldate) {
        if (!isAllowReadMixParlayReport) {
            return;
        }
        var reportDate = new Date(wldate);
        var settingDate = new Date(dailyWLMP_WebSettings);
        if (reportDate < settingDate) {
            WL2hShowHideDivManager.showMixParlayReportFromAMPR(AdvanceMixParlayReportManager.sportTypeId, wldate);
            return;
        }
        AdvanceMixParlayReportManager.reportDate = wldate;
        var data = {
            param:
            {
                ReportDate: wldate,
                BetType: AdvanceMixParlayReportManager.bettype
            }
        };
        try { parent.BlockUI(); } catch (e) { }
        DailyWinLoss2hManager.disableReportButton();
        $.ajax({
            type: "POST",
            url: window.rootUrl + "DailyWinLoss2h/SelAdvanceMixParlayReport",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: JSON.stringify(data),
            success: function (rs) {
                if ($("#InformationMssg").html() != "") {
                    $("#InformationMssg").hide();
                    $("#InformationMssg").empty();
                }
                AdvanceMixParlayReportManager.me.addClass('hidden');
                AdvanceMixParlayReportManager.renderDailyWLReport(rs, wldate);
            },
            error: function () {
                AdvanceMixParlayReportManager.renderNoData();
            },
            complete: function () {
                DailyWinLoss2hManager.enableReportButton();
                try { parent.jQuery.unblockUI(); } catch (e) { }
            }
        });
    },
    createScroll: function () {
        AdvanceMixParlayReportManager.me.removeClass('hidden');
    },
    renderDailyWLReport: function (dailyWLResult, wldate) {
        var reportDate = new Date(wldate);
        AdvanceMixParlayReportManager.title = 'MIX PARLAY PRODUCT WIN/LOSS REPORT FOR ' + StringFormatter.get_MMDDYYYY_String(reportDate);
        $("#advanceMixParlayReportHeader").html(AdvanceMixParlayReportManager.title);
        if (dailyWLResult != null & (dailyWLResult.NoneLiveReport.ReportDetails.length > 0 ||
                                        dailyWLResult.LiveReport.ReportDetails.length > 0 ||
                                        dailyWLResult.MixLiveAndNoneLiveReport.ReportDetails.length > 0)) {
            var arrBody = [];
            var arrFooter = [];
            arrBody.push(AdvanceMixParlayReportManager.renderRow(dailyWLResult.LiveReport));
            arrBody.push(AdvanceMixParlayReportManager.renderRow(dailyWLResult.NoneLiveReport));
            arrBody.push(AdvanceMixParlayReportManager.renderRow(dailyWLResult.MixLiveAndNoneLiveReport));
            arrFooter.push(AdvanceMixParlayReportManager.renderGrandTotal(dailyWLResult.ReportTotal.NormalTotal));

            var bodyHtml = arrBody.join('');
            bodyHtml = StringFormatter.removeSpaceIE9(bodyHtml);
            AdvanceMixParlayReportManager.tb.find("tr:not(.header)").remove();
            AdvanceMixParlayReportManager.tb.append(bodyHtml);

            var footerHtml = arrFooter.join('');
            AdvanceMixParlayReportManager.tb.append(footerHtml);
            AdvanceMixParlayReportManager.createScroll();
        }
        else {
            AdvanceMixParlayReportManager.renderNoData();
        }
    },
    renderRow: function (row) {
        var html = '';
        var reportDetails = row.ReportDetails;
        var totalItems = reportDetails.length;
        if (totalItems == 0) {
            return html;
        }
        html = '<tr>';
        var css = '';
        switch (row.Livetype.Name) {
            case 'Live':
                css = "live";
                break;
            case 'Non Live':
                css = "nonlive";
                break;
            default:
                css = "mix";
                break;
        }
        html += '<td rowspan="' + totalItems + '" class="' + css + '"><div class="col120 c b">' + row.Livetype.Name + '</div></td>'

        var sportHtml = "";
        var sportRowSpan = 0;
        var numSport = 0;
        for (var i = 0; i < totalItems; i++) {
            var detail = reportDetails[i];
            switch (detail.RowTypeDWL2h.Id) {
                case 0:
                    if (sportRowSpan > 0) {
                        sportHtml += '<tr>';
                    }
                    sportHtml += '<td><div class="col120 l">' + detail.AliasName + '</div></td>\
                                    <td><div class="col100 r ' + (detail.WinLoss.toFixed(2) > 0 ? 'red' : '0') + '">' + StringFormatter.formatNumber(detail.WinLoss) + '</div></td>\
                                    <td><div class="col90 r">' + StringFormatter.formatNumber(detail.Position) + '</div></td>\
                                    <td><div class="col80 r">' + StringFormatter.formatNumber(detail.Commission) + '</div></td>\
                                    <td><div class="col100 r ' + (detail.LineTotal.toFixed(2) > 0 ? 'red' : '0') + '">' + StringFormatter.formatNumber(detail.LineTotal) + '</div></td>\
                                    <td><div class="col100 r">' + StringFormatter.formatNumber(detail.Turnover) + '</div></td>\
                                    <td><div class="col80 r ' + (detail.Turnover.toFixed(2) != 0 && detail.Margin.toFixed(2) > 0 ? 'red' : '0') + '">' + (detail.Turnover.toFixed(2) != 0 ? (StringFormatter.formatNumber(detail.Margin) + '%') : 'N/A') + '</div></td>';

                    sportHtml += '</tr>';
                    sportRowSpan++;
                    break;
                case 3:
                    sportHtml += '<tr>\
                                    <td><div class="col120 c" style="height: 18px;"></div></td>\
                                    <td style="background-color:#C4BD97"><div class="col120 c b" >Sub Total</div></td>\
                                    <td style="background-color:#C4BD97"><div class="col100 r b ' + (detail.WinLoss.toFixed(2) > 0 ? 'red' : '0') + '" >' + StringFormatter.formatNumber(detail.WinLoss) + '</div></td>\
                                    <td style="background-color:#C4BD97"><div class="col90 r b">' + StringFormatter.formatNumber(detail.Position) + '</div></td>\
                                    <td style="background-color:#C4BD97"><div class="col80 r b">' + StringFormatter.formatNumber(detail.Commission) + '</div></td>\
                                    <td style="background-color:#C4BD97"><div class="col100 r b ' + (detail.LineTotal.toFixed(2) > 0 ? 'red' : '0') + '">' + StringFormatter.formatNumber(detail.LineTotal) + '</div></td>\
                                    <td style="background-color:#C4BD97"><div class="col100 r b">' + StringFormatter.formatNumber(detail.Turnover) + '</div></td>\
                                    <td style="background-color:#C4BD97"><div class="col80 r b ' + (detail.Turnover.toFixed(2) != 0 && detail.Margin.toFixed(2) > 0 ? 'red' : '0') + '">' + (detail.Turnover.toFixed(2) != 0 ? (StringFormatter.formatNumber(detail.Margin) + '%') : 'N/A') + '</div></td>\
                                </tr>';

                    if (numSport > 0) {
                        html += '<tr>';
                    }
                    html += '<td rowspan="' + sportRowSpan + '"><div class="col120 c b">' + detail.Name + '</div></td>';
                    html += sportHtml;
                    sportHtml = "";
                    sportRowSpan = 0;
                    numSport++;
                    break;
            }
        }

        var normalTotal = row.ReportTotal.NormalTotal;
        html += '<tr>\
                        <td><div class="col120 c" style="height: 18px;"></div></td>\
                        <td style="background-color:#215967; height: 18px;"><div class="col120 c" ></div></td>\
                        <td style="background-color:#215967"><div class="col120 c b" style="color: White;">Total</div></td>\
                        <td style="background-color:#215967"><div class="col100 r b ' + (normalTotal.WinLoss.toFixed(2) > 0 ? 'red' : 'whiteText') + '">' + StringFormatter.formatNumber(normalTotal.WinLoss) + '</div></td>\
                        <td style="background-color:#215967"><div class="col90 r b" style="color: White;">' + StringFormatter.formatNumber(normalTotal.Position) + '</div></td>\
                        <td style="background-color:#215967"><div class="col80 r b" style="color: White;">' + StringFormatter.formatNumber(normalTotal.Commission) + '</div></td>\
                        <td style="background-color:#215967"><div class="col100 r b ' + (normalTotal.LineTotal.toFixed(2) > 0 ? 'red' : 'whiteText') + '">' + StringFormatter.formatNumber(normalTotal.LineTotal) + '</div></td>\
                        <td style="background-color:#215967"><div class="col100 r b" style="color: White;">' + StringFormatter.formatNumber(normalTotal.Turnover) + '</div></td>\
                        <td style="background-color:#215967"><div class="col80 r b ' + (normalTotal.Turnover.toFixed(2) != 0 && normalTotal.Margin.toFixed(2) > 0 ? 'red' : 'whiteText') + '">' + (normalTotal.Turnover.toFixed(2) != 0 ? (StringFormatter.formatNumber(normalTotal.Margin) + '%') : 'N/A') + '</div></td>\
                    </tr>';
        return html;
    },
    renderGrandTotal: function (row) {
        var html = '';
        html = '<tr>\
                        <td colspan="3" style="background-color:#D8E4BC"><div class="c b" style="width:366px">Grand Total</div></td>\
                        <td style="background-color:#D8E4BC"><div class="col100 r b ' + (row.WinLoss.toFixed(2) > 0 ? 'red' : '0') + '">' + StringFormatter.formatNumber(row.WinLoss) + '</div></td>\
                        <td style="background-color:#D8E4BC"><div class="col90 r b">' + StringFormatter.formatNumber(row.Position) + '</div></td>\
                        <td style="background-color:#D8E4BC"><div class="col80 r b">' + StringFormatter.formatNumber(row.Commission) + '</div></td>\
                        <td style="background-color:#B5C4D0"><div class="col100 r b ' + (row.LineTotal.toFixed(2) > 0 ? 'red' : '0') + '">' + StringFormatter.formatNumber(row.LineTotal) + '</div></td>\
                        <td style="background-color:#D8E4BC"><div class="col100 r b ">' + StringFormatter.formatNumber(row.Turnover) + '</div></td>\
                        <td style="background-color:#D8E4BC"><div class="col80 r b ' + (row.Turnover.toFixed(2) != 0 && row.Margin.toFixed(2) > 0 ? 'red' : '0') + '">' + (row.LineTotal.toFixed(2) != 0 ? (StringFormatter.formatNumber(row.Margin) + '%') : 'N/A') + '</div></td>\
                    </tr>';
        return html;
    },
    renderNoData: function () {
        AdvanceMixParlayReportManager.tb.find("tr:not(.header)").remove();
        AdvanceMixParlayReportManager.tb.append('<tr id="noinfo"><td id="tdNoInfomation" colspan="9" class="b c" style="" >There is no information available.</td></tr>');
        AdvanceMixParlayReportManager.me.removeClass('hidden');
    },
    renderEmptyRow: function () {
        var html = '<tr>\
                        <td colspan ="9" class ="c" style="height:18px;"></td>\
                    </tr>';
        return html;
    },
    exportToExcel: function () {
        if (!isAllowReadMixParlayReport) {
            return;
        }
        var url = 'DailyWinLoss2h?ExportType=AdvanceMixParlay&ReportDate=' +
                    AdvanceMixParlayReportManager.reportDate +
                    '&Bettype=' + AdvanceMixParlayReportManager.bettype;
        $('#frm').attr('src', url);
    }
}
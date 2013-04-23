

var WL2hShowHideDivManager = {
    showOtherSportsReport: function (sportId, wldate) {
        if (isAllowReadOtherSportsReport) {
            try { parent.BlockUI(); } catch (e) { }
            $("#dailyWLReportDiv").addClass("hidden");
            WL2hShowHideDivManager.hideShareElement();
            $("#dailyWinLossReport").addClass("hidden");

            OtherSportsReportManager.initGrid(sportId, wldate);

        } else {
            showAlertMessage("InformationMssg", "Permission is denied.", 1);
            $("#InformationMssg").show();
        }
    },
    showMainReportFromOSR: function (sportId, wldate) {
        if (isAllowRead) {
            try { parent.BlockUI(); } catch (e) { }
            $("#dailyWLReportDiv").addClass("hidden");
            WL2hShowHideDivManager.hideShareElement();
            OtherSportsReportManager.hideAllReport();
            DailyWinLoss2hManager.reloadGrid(sportId, wldate);

        } else {
            showAlertMessage("InformationMssg", "Permission is denied.", 1);
            $("#InformationMssg").show();
        }
    },
    hideShareElement: function () {
        $("#InformationMssg").hide();
        $("#InformationMssg").empty();
    }
}
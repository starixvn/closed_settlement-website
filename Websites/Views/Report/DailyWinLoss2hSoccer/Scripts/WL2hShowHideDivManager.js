

var WL2hShowHideDivManager = {    
    showSoccerReport: function (sportId, wldate) {
        if (isAllowReadSoccerReport) {
            try { parent.BlockUI(); } catch (e) { }
            $("#dailyWLReportDiv").addClass("hidden");
            WL2hShowHideDivManager.hideShareElement();
            $("#dailyWinLossReport").addClass("hidden");

            SoccerReportManager.initGrid(sportId, wldate);

        } else {
            showAlertMessage("InformationMssg", "Permission is denied.", 1);
            $("#InformationMssg").show();
        }
    },
    showMainReportFromSCR: function (sportId, wldate) {
        if (isAllowRead) {
            try { parent.BlockUI(); } catch (e) { }
            $("#dailyWLReportDiv").addClass("hidden");
            WL2hShowHideDivManager.hideShareElement();

            $("#gridSplitter_SCR").addClass('hidden');
            SoccerReportManager.rightDetailReport.addClass("hidden");
            $("#soccerReport").addClass("hidden");
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
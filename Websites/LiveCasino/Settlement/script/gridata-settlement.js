var maxGridHeigh = 500;

var settlelc = {
    width: 965,
    height: maxGridHeigh
};

settlelc.init = function () {
    if ($.browser.msie) {
        settlelc.height = maxGridHeigh;
    }
    $("#divMainGrid").removeClass("hidden").reRenderScroller(settlelc.width, settlelc.height, true);
};

settlelc.reCalc = function (matchid, leagueid, bettype, showDate) {
    var width = $(window).width() - 10;
    if (bettype == 1102)
        width = 760;
    var left = (jQuery('body').width() / 2) - (width / 2);
    ShowPopupIframe("LiveCasinoReCalc.aspx?matchid=" + matchid + "&lid=" + leagueid + "&bettype=" + bettype + "&showdate=" + showDate, "ReSettlement", width, 130, left, 200);
};

settlelc.refund = function (matchid, leagueid, bettype, showDate) {
    var width = 640;
    var left = (jQuery('body').width() / 2) - (width / 2);
    ShowPopupIframe("LiveCasinoRefund.aspx?matchid=" + matchid + "&lid=" + leagueid + "&bettype=" + bettype + "&showdate=" + showDate, "Refund", width, 150, left, 200);
};

settlelc.showLog = function(matchcode) {
    var now = new Date();
    var dateFormat = (now.getMonth()+1)+'/'+ now.getDate() + '/' + now.getFullYear();
    var fdate = $('#fdate') ? $('#fdate').val() : dateFormat;
    var tdate = $('#tdate') ? $('#tdate').val() : dateFormat;

	matchcode = matchcode.replace('&', '^');
	var title = 'SETTLEMENT LOG';
	var url = '../../Settle/GetSettleLog.aspx?matchcode=' + matchcode + '&fdate=' + fdate + '&tdate=' + tdate;
    var width = 950;
    var height = 600;
    parent.OpenPopup(url, title, width, height, true, true, window, 2000, null, null);
};

$(document).ready(function () {
    settlelc.init();
});
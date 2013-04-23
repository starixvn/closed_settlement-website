function getPramJs(name) {
    var q = document.location.search;
    var i = q.indexOf(name + '=');

    if (i == -1) {
        return false;
    }

    var r = q.substr(i + name.length + 1, q.length - i - name.length - 1);

    i = r.indexOf('&');

    if (i != -1) {
        r = r.substr(0, i);
    }

    return r.replace(/\+/g, ' ');
}

function getRealTop(el) {
    try {
        if (!$(el)) return 0;
        var yPos = document.getElementById(el).offsetTop;
        var tempEl = document.getElementById(el).offsetParent;
        while (tempEl != null) {
            yPos += tempEl.offsetTop;
            tempEl = tempEl.offsetParent;
        }
    }
    catch (e)
                { }

    return yPos;
}

function PopUpWindow(url, width, height) {
    var top = (screen.height - height) / 2;
    var left = (screen.width - width) / 2;
    var newwindow = window.open(url, "EditSuper", "height=" + height + ",width=" + width + ",top=" + top + ",left=" + left + ",scrollbars=1");
    if (newwindow) newwindow.focus();
}

var widthmainpage = 100;
var betlistpage = '';
var mode = 0
var bettypename = ''

//This function only apply for Fixed Odds and Tote HR
function ViewReport() {
    
    if (parent.BlockUI)
    {
        try{parent.BlockUI();}catch(e){}
    }
    else
    {
        if (opener.BlockUI)
        {
             opener.BlockUI();
        }
    }
   
    var movebetsday = new Date($('#movebetsday').val());
    var fdate = $('#fdate').val();
    if (movebetsday >= new Date(fdate))
        $('#rdRep').attr('checked', true);

    var db = ($('#rdMain').attr('checked')) ? 'main' : 'rep';
    if (getPramJs('db')) db = getPramJs('db');

    

    var param = '';
    param += 'mode=' + mode;
    param += '&db=' + db;
    param += '&wldate=' + fdate;
    param += '&bettype=' + bettypename;
    param += '&type=' + getPramJs('type');

    $('#btnReport').attr('disabled', true);
    $('#btnExport').attr('disabled', true);
    
    $.ajax(
                 {
                     type: "POST",
                     url: "GetData.ashx",
                     data: param,
                     success: GetJSONResult

                 }
             );
}

function GetJSONResult(resContent) {
    var reportInfo = $.evalJSON(resContent);
    var sData = reportInfo.sData;
    var sErrMsg = reportInfo.sErrMsg;

    $("#sContent").html(sData);
    $("#sErrMsg").html(sErrMsg);

    InitFixedHeader();
    

    $('#btnReport').attr('disabled', false);
    $('#btnExport').attr('disabled', false);

   

    if (parent.jQuery.unblockUI)
    {
         parent.jQuery.unblockUI();
    }
    else
    {
         if (opener.jQuery.unblockUI)
         {
            opener.jQuery.unblockUI();
         }
    }
   
}

function InitFixedHeader() {


    var topHeader = getRealTop('trHeader');
    var topFooter = getRealTop('trFooter');
    var innerHeight = window.innerHeight;
    var delta = 20;

    if (!innerHeight) {
        innerHeight = document.documentElement.clientHeight;
        delta = 40;
    }

    if (($('#rdMain').length) > 0)
        delta = delta - 30;

    var RealHeight = innerHeight - topHeader + delta;

    if (topFooter > innerHeight) {
        jQuery('#table').Scrollable(RealHeight, widthmainpage);
    }
    else {
        $('#table .rspace').removeClass('rspace');
    }
    // alert('topHeader: ' + topHeader + '\ntopFooter: ' + topFooter + '\nwindow.innerHeight: ' + innerHeight);
}

//leagueid|raceNo|currencyID|currencyName|wldate|db
function ViewBetList(leagueid, raceNo, currencyID, currencyName, wldate, db) {
    var url = betlistpage + '?leagueid=' + leagueid;
    url += '&raceNo=' + raceNo;
    url += '&currencyID=' + currencyID;
    url += '&currencyName=' + currencyName;
    url += '&wldate=' + wldate;
    url += '&db=' + db;

    var w = 1200;
    var h = 800;
    PopUpWindow(url, w, h);
}

function ExportToExcel(mode, bettype) {
    var wldate = $('#fdate').val();
    var movebetsday = new Date($('#movebetsday').val());
    if (movebetsday >= new Date(wldate))
        $('#rdRep').attr('checked', true);
    var db = ($('#rdMain').attr('checked')) ? 'main' : 'rep';

    var url = '?wldate=' + wldate;
    url += '&bettype=' + bettypename;
    url += '&db=' + db;
    url += '&IsExport=1';

    window.location.href = url;
}


//for betlist
function InitFixedHeaderBL() {
    var topHeader = getRealTop('trHeader');
    var topFooter = getRealTop('trFooter');
    var innerHeight = window.innerHeight;
    var delta = 0;
    var widthmainpage = 1100;

    if (!innerHeight) {
        innerHeight = document.documentElement.clientHeight;
        delta = 0;
    }

    var RealHeight = innerHeight - topHeader + delta;

    if (topFooter > innerHeight) {
        jQuery('#table').Scrollable(RealHeight, widthmainpage);
    }
    else {
        $('#table .rspace').removeClass('rspace');
    }
}


//for league wl - sport
function InitFixedHeaderLeagueWL(widthmainpage) {
    var topHeader = getRealTop('trHeader');
    var topFooter = getRealTop('trFooter');
    var innerHeight = window.innerHeight;
    var delta = 0;
    if (!innerHeight) {
        innerHeight = document.documentElement.clientHeight;
        delta = 0;
    }

    var RealHeight = innerHeight - topHeader + delta;

    if (topFooter > innerHeight) {
        jQuery('#table').Scrollable(RealHeight, widthmainpage);
    }
    else {
        $('#table .rspace').removeClass('rspace');
    }
}


function ViewReportLeagueWLbySport(mode) {
    try{parent.BlockUI();}catch(e){}
    
    var movebetsday = new Date($('#movebetsday').val());
    var fdate = $('#fdate').val();
    if (movebetsday >= new Date(fdate))
        $('#rdRep').attr('checked', true);

    var db = ($('#rdMain').attr('checked')) ? 'main' : 'rep';
    if (getPramJs('db')) db = getPramJs('db');

    

    var param = '';
    param += 'mode=' + mode;
    param += '&db=' + db;
    param += '&wldate=' + fdate;

    $('#btnReport').attr('disabled', true);
    $('#btnExport').attr('disabled', true);
    
    $.ajax(
                 {
                     type: "POST",
                     url: "GetDataLeagueWL.ashx",
                     data: param,
                     success: GetJSONResultLeagueWLbySport

                 }
             );
}




function GetJSONResultLeagueWLbySport(resContent) {
    var reportInfo = $.evalJSON(resContent);

    var sData = reportInfo.sData;

    $("#sContent").html(sData);

    InitFixedHeaderLeagueWL(600);
    

    $('#btnReport').attr('disabled', false);
    $('#btnExport').attr('disabled', false);
    
    parent.jQuery.unblockUI();
}

function LeagueWLBettype(sporttype, leagueid) {
    //alert('sporttype: ' + sporttype + '\nleagueid: ' + leagueid);

}
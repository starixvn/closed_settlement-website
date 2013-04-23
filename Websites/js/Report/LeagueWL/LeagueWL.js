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

function GetData(param) {
    
    var movebetsday = new Date($('#movebetsday').val());
    var fdate = $('#fdate').val();
    if (movebetsday >= new Date(fdate))
        $('#rdRep').attr('checked', true);

    var db = ($('#rdMain').attr('checked')) ? 'main' : 'rep';
    if (getPramJs('db')) db = getPramJs('db');

    

    
    param += '&db=' + db;
    param += '&wldate=' + fdate;

    $('#btnReport').attr('disabled', true);
    $('#btnExport').attr('disabled', true);

    $.ajax(
                 {
                     type: "POST",
                     url: "GetDataLeagueWL.ashx",
                     data: param,
                     success: ShowData

                 }
             );
}

function ShowData(resContent) {
    var reportInfo = $.evalJSON(resContent);

    var sData = reportInfo.sData;

    $("#sContent").html(sData);
    
    InitFixedHeaderLeagueWL(800);
    
    

    $('#btnReport').attr('disabled', false);
    $('#btnExport').attr('disabled', false);
    try{parent.jQuery.unblockUI();}catch(e){}
}



function GetLeagueWLSport(param) {
    try{parent.BlockUI();}catch(e){}
    var path = "<span class='rpt-bold'>Main</span> ";
    $('#spPath').html(path);
    $('#sporttype').val('');
    $('#leagueid').val('');
    $('#type').val(1);
    $('#sContentSub').html('');
    $('tdLeague').attr('disabled', true);
    GetData(param);
    $('#tdLine').attr('bgcolor', '#E6E9F0');
}

function GetLeagueWLBettype(sporttype, leagueid, name) {

    try{parent.BlockUI();}catch(e){}    
    $('#sport_league_name').val(name);
    var path = "<span class='hand rpt-link rpt-bold' onclick=\" GetLeagueWLSport('mode=1&isback=1', 1); \">Main</span> > <b>" + name + "</b>";
    $('#spPath').html(path);
    $('#sporttype').val(sporttype);
    $('#leagueid').val(leagueid);
    $('#type').val(2);
    $('#sContentSub').html('');
    
    var param = 'mode=2';
    param += '&sporttype=' + sporttype;
    param += '&leagueid=' + leagueid;
    $('tdBettype').attr('disabled', 'disabled');
    
    GetData(param);
}

function GetLeagueWLDetails(leagueid, sporttype, bettype, liveindicator, IsHedge, db, wldate) {
    try{parent.BlockUI();}catch(e){}
    var param = '';
    param = 'mode=3' ;
    param += '&leagueid=' + leagueid;
    param += '&sporttype=' + sporttype;
    param += '&bettype=' + bettype;
    param += '&liveindicator=' + liveindicator;
    param += '&IsHedge=' + IsHedge;
    param += '&db=' + db;
    param += '&wldate=' + wldate;

    $('#btnReport').attr('disabled', true);
    $('#btnExport').attr('disabled', true);

    $.ajax(
                 {
                     type: "POST",
                     url: "GetDataLeagueWL.ashx",
                     data: param,
                     success: ShowLeagueWLDatails

                 }
             );
}


function ShowLeagueWLDatails(resContent) {

    var reportInfo = $.evalJSON(resContent);

    var sData = reportInfo.sData;

    $('#tdLine').attr('bgcolor', 'green');
    $("#sContentSub").html(sData);

    InitFixedHeaderLeagueWL(600);
    

    $('#btnReport').attr('disabled', false);
    $('#btnExport').attr('disabled', false);
    try{parent.jQuery.unblockUI();}catch(e){}

}

function ExportLeagueWLDetailsToExcel() {
    $('#type').val('3');
    $('#IsExport').val('1');
    $('#form').submit();
}
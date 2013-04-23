//Check if browser is IE
function isIE() {
    return (navigator.appName.indexOf("Internet Explorer") > -1)
}

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


function CheckDecNumber(e) {
    var code = "";
    if (e.which)
        code = e.which;
    else if (e.keyCode)
        code = e.keyCode;
    //alert(code);

    if (code == 69 || code == 101 || code == 45 || code == 37 || code == 39 || code == 36 || code == 35) //e or E or "-"
        return true;

    if ((code < 48 || code > 57) && code != 8 && code != 13 && code != 0 && code != 46 && code != 9) {
        return false;
    }
    if (code == 13) {
        //   $('btnsubmit').click();
    }
    return true;
}




function ViewReport() {
    var IsLeague = $('input[id=rdByLeague]').is(':checked');
    if (IsLeague) { //League
        SelTopPunterSummaryByLeagues();
    }
    else //Other
    {
        SelTopPunterSummary();
    }
    $('#dvTable').show();
}

function SelTopPunterSummary() {
    try { parent.BlockUI(); } catch (e) { }
    var fdate, tdate;
    var username = $('#txtUserName').val();
    var searchtype = $("input[name=rdSearch]:checked").val();
    if (searchtype == '1') username = '';
    $('#orderby').val($('#ddlOrderby').val());
    $('#ordertype').val($('#ddlOrderType').val());

    var obj = GetDay();
    fdate = obj.fdate;
    tdate = obj.tdate;

    $('#type').val('SelTopPunterSummary');

    var params = 'type=SelTopPunterSummary';
    params += '&username=' + username;
    params += '&fromdate=' + fdate;
    params += '&todate=' + tdate;
    params += '&toprow=' + $('#txtTopRow').val();
    params += '&orderby=' + $('#ddlOrderby').val();
    params += '&ordertype=' + $('#ddlOrderType').val();
    $.ajax({
        type: 'POST',
        url: 'GetDataPunter.ashx',
        async: true,
        data: params,
        success: function (rs) {
            var rpt = $.evalJSON(rs);

            $("#sData").html(rpt.data);
            $('#FullPath').val(rpt.fullpath);
            $('#SelTopPunterSummarycachepath').val(rpt.fullpath);
            $('#SelTopPunterSummaryContent').val($('#sData').html());
            InitFixedHeader(600);
            try { parent.jQuery.unblockUI(); } catch (e) { }
        }
    });
}

function SelTopPunterSummaryExportOnly() {
    try { parent.BlockUI(); } catch (e) { }
    var fdate, tdate;
    var username = $('#txtUserName').val();
    var searchtype = $("input[name=rdSearch]:checked").val();
    if (searchtype == '1') username = '';
    $('#orderby').val($('#ddlOrderby').val());
    $('#ordertype').val($('#ddlOrderType').val());

    var obj = GetDay();
    var fdate, tdate;
    fdate = obj.fdate;
    tdate = obj.tdate;

    var type = $('#type').val();
    var custid = $('#custid').val();
    var leagueid = '';
    switch (type) {
        case 'SelTopPunterByCustId':
        case 'SelTopPunterBySporttype':
        case 'SelTopPunterByBettype':
            username = $('#username').val();
            break;
        case 'SelIPByCustomer':
        case 'SelBetCountByCustomer':
            username = getPramJs('username');
            custid = getPramJs('custid');
            fdate = getPramJs('fromdate');
            tdate = getPramJs('todate');
            leagueid = getPramJs('leagueid');
            break;
    }

    var params = '?type=' + type;
    params += '&username=' + username;
    params += '&custid=' + custid;
    params += '&fromdate=' + fdate;
    params += '&todate=' + tdate;
    params += '&sporttype=' + $('#sporttype').val();
    params += '&bettype=' + $('#bettype').val();
    params += '&toprow=' + $('#txtTopRow').val();
    params += '&orderby=' + $('#ddlOrderby').val();
    params += '&ordertype=' + $('#ddlOrderType').val();
    params += '&leagueid=' + leagueid;

    $("#frm").attr('src', 'PunterAnalysis.aspx' + params);
    try { parent.jQuery.unblockUI(); } catch (e) { }

}


function SelTopPunterSummaryByLeagues() {

    try { parent.BlockUI(); } catch (e) { }
    $('#orderby').val($('#ddlOrderby').val());
    $('#ordertype').val($('#ddlOrderType').val());
    $('#sportid').val($('#ddlSports').val());
    $('#leagueid').val($('#ddlLeagues').val());

    var obj = GetDay();
    var fdate, tdate;
    fdate = obj.fdate;
    tdate = obj.tdate;
    $('#type').val('SelTopPunterSummaryByLeagues');
    var params = 'type=SelTopPunterSummaryByLeagues';

    params += '&leagueid=' + $('#ddlLeagues').val();
    params += '&leaguename=' + $("#ddlLeagues option:selected").text();
    params += '&fromdate=' + fdate;
    params += '&todate=' + tdate;
    params += '&toprow=' + $('#txtTopRow').val();
    params += '&orderby=' + $('#ddlOrderby').val();
    params += '&ordertype=' + $('#ddlOrderType').val();

    $.ajax({
        type: 'POST',
        url: 'GetDataPunter.ashx',
        async: true,
        data: params,
        success: function (rs) {
            var rpt = $.evalJSON(rs);
            $("#sData").html(rpt.data);
            $('#FullPath').val(rpt.fullpath);
            $('#SelTopPunterSummaryByLeaguePath').val(rpt.fullpath);
            $('#SelTopPunterSummaryByLeagueContent').val($('#sData').html());
            InitFixedHeader(600);
            parent.jQuery.unblockUI();
        }
    });
}

function SelTopPunterSummaryByLeaguesExportOnly() {

    try { parent.BlockUI(); } catch (e) { }
    $('#orderby').val($('#ddlOrderby').val());
    $('#ordertype').val($('#ddlOrderType').val());
    $('#sportid').val($('#ddlSports').val());
    $('#leagueid').val($('#ddlLeagues').val());
    $('#leaguename').val($("#ddlLeagues option:selected").text());

    var obj = GetDay();
    var fdate, tdate;
    fdate = obj.fdate;
    tdate = obj.tdate;


    var type = $('#type').val();

    var params = '?type=' + type;

    params += '&leagueid=' + $('#ddlLeagues').val();
    params += '&leaguename=' + $("#ddlLeagues option:selected").text();
    params += '&fromdate=' + fdate;
    params += '&todate=' + tdate;
    params += '&toprow=' + $('#txtTopRow').val();
    params += '&orderby=' + $('#ddlOrderby').val();
    params += '&ordertype=' + $('#ddlOrderType').val();


    $("#frm").attr('src', 'PunterAnalysis.aspx' + params);
    try { parent.jQuery.unblockUI(); } catch (e) { }
}

function GetDay() {
    var fdate, tdate;
    var obj = new Object();
    var periodtype = $("input[name=rd]:checked").val();
    switch (periodtype) {
        case 'month':
            fdate = $('#ddlMonth').val() + '/01/' + $('#ddlYear').val();
            tdate = $('#ddlMonth').val() + '/' + GetMaxDayByMonth($('#ddlMonth').val()) + '/' + $('#ddlYear').val();
            $('#fMonth').val($('#ddlMonth').val());
            $('#fYear').val($('#ddlYear').val());
            break;
        case 'annal':
            fdate = '01/01/' + $('#ddlYear').val();
            tdate = '12/31/' + $('#ddlYear').val();
            $('#fYear').val($('#ddlYear').val());
            break;
        case 'longrange':
            fdate = $('#ddfMonth').val() + '/01/' + $('#ddfYear').val();
            tdate = $('#ddtMonth').val() + '/' + GetMaxDayByMonth($('#ddtMonth').val()) + '/' + $('#ddtYear').val();
            $('#fMonth').val($('#ddfMonth').val());
            $('#fYear').val($('#ddfYear').val());
            $('#tMonth').val($('#ddtMonth').val());
            $('#tYear').val($('#ddtYear').val());
            break;
    }

    obj.fdate = fdate;
    obj.tdate = tdate;

    return obj;
}

function SetEventForCalendar() {
    var periodtype = $("input[name=rd]:checked").val();
    switch (periodtype) {
        case 'month':
            $('#ddlMonth').live('change', LoadLeagues);
            $('#ddlYear').live('change', LoadLeagues);
            break;
        case 'annal':
            $('#ddlYear').live('change', LoadLeagues);
            break;
        case 'longrange':
            $('#ddfMonth').live('change', LoadLeagues);
            $('#ddfYear').live('change', LoadLeagues);
            $('#ddtMonth').live('change', LoadLeagues);
            $('#ddtYear').live('change', LoadLeagues);
            break;
    }
}

function GetMaxDayByMonth(Month) {
    var day = '';
    switch (parseInt(Month)) {
        case 1:
        case 3:
        case 5:
        case 7:
        case 8:
        case 10:
        case 12:
            day = 31;
            break;
        case 2:
            day = 28;
            break;
        case 4:
        case 6:
        case 9:
        case 11:
            day = 30;
            break;
    }
    return day;
}

window.onload = LoadFromDefault;

function LoadFromDefault() {
    DisplayPeriodType();
    SearchBy();

    if ($('#sData').html().length == 0)
        $('#dvTable').hide();
    else
        $('#dvTable').show();
}
function LoadForm(searchby, periodtype) {
    DisplayPeriodType(periodtype);
    SearchBy(searchby);
}

function DisplayPeriodType(back) {

    if ($('#rdMonthly').length == 0) return;

    try { parent.BlockUI(); } catch (e) { }
    SetEventForCalendar();
    var periodtype = $("input[name=rd]:checked").val();
    if (typeof (back) != 'undefined') periodtype = back;
    switch (periodtype) {
        case 'month':
            $('#rdMonthly').attr('checked', true);
            break;
        case 'annal':
            $('#rdAnnual').attr('checked', true);
            break;
        case 'longrange':
            $('#rdLongRange').attr('checked', true);
            break;
    }

    var param = 'type=' + periodtype;
    param += '&fMonth=' + $('#fMonth').val();
    param += '&fYear=' + $('#fYear').val();
    param += '&tMonth=' + $('#tMonth').val();
    param += '&tYear=' + $('#tYear').val();

    $.ajax({
        type: 'POST',
        url: 'GetDataPunter.ashx',
        async: true,
        data: param,
        success: function (rs) {
            var rpt = $.evalJSON(rs);
            $('#dvCarlendarType').html(rpt.data);
            parent.jQuery.unblockUI();
        }
    });
}

function OrderBy() {
    var column = $('#ddlOrderby').val();
    if (column == '2') {
        var ddlOrderType = document.getElementById('ddlOrderType');
        ddlOrderType.value = '0';
        $('#ddlOrderType').hide();
    }
    else {
        $('#ddlOrderType').show();
    }
}

function SearchBy(back) {
    var s = $("input[name=rdSearch]:checked").val();
    if (typeof (back) != 'undefined') s = back;
    if (s == '0') {//0: User
        $('#trToprows').hide();
        $('#trOrderby').hide();
        $('#dvLeague').hide();
        $('#trUserName').show();
        $('#rdAllUser').attr('checked', false);
        $('#rdByUser').attr('checked', true);
        $('#rdByLeague').attr('checked', false);
        $('#sData').html($('#SelTopPunterSummaryContent').val());
        $('#FullPath').val($('#SelTopPunterSummarycachepath').val());
    }
    else if (s == '2') {//2: League
        $('#trToprows').show();
        $('#trOrderby').show();
        $('#trUserName').hide();
        $('#dvLeague').show();
        $('#rdAllUser').attr('checked', false);
        $('#rdByUser').attr('checked', false);
        $('#rdByLeague').attr('checked', true);
        $('#sData').html($('#SelTopPunterSummaryByLeagueContent').val());
        $('#FullPath').val($('#SelTopPunterSummaryByLeaguePath').val());
        if ($('#spSports').html() == '') {
            LoadSports();
        }
    }
    else {//1: All Users
        $('#trToprows').show();
        $('#trOrderby').show();
        $('#dvLeague').hide();
        $('#trUserName').hide();
        $('#rdAllUser').attr('checked', true);
        $('#rdByUser').attr('checked', false);
        $('#rdByLeague').attr('checked', false);
        $('#sData').html($('#SelTopPunterSummaryContent').val());
        $('#FullPath').val($('#SelTopPunterSummarycachepath').val());
    }
    if ($('#sData').html().length == 0)
        $('#dvTable').hide();
    else
        $('#dvTable').show();

    SetEventForCalendar();
    InitFixedHeader(600);
}

function LoadSports() {
    try { parent.BlockUI(); } catch (e) { }
    var obj = GetDay();
    var fdate, tdate;
    fdate = obj.fdate;
    tdate = obj.tdate;

    var params = '';
    params = 'type=SelSports';
    params += '&fromdate=' + fdate;
    params += '&todate=' + tdate;

    $.ajax({
        type: 'POST',
        url: 'GetDataPunter.ashx',
        async: true,
        data: params,
        success: function (rs) {
            var rpt = $.evalJSON(rs);
            $("#spSports").html(rpt.sports);
            $("#spLeagues").html(rpt.leagues);

            if ($('#sData').html().length == 0)
                $('#dvTable').hide();
            else
                $('#dvTable').show();
            parent.jQuery.unblockUI();
        }
    });
}

function LoadLeagues() {

    var IsBind = $('input[id=rdByLeague]').is(':checked');
    if (!IsBind) return;

    try { parent.BlockUI(); } catch (e) { }
    var obj = GetDay();
    var fdate, tdate;
    fdate = obj.fdate;
    tdate = obj.tdate;

    var params = '';
    params = 'type=SelLeagues';
    params += '&fromdate=' + fdate;
    params += '&todate=' + tdate;
    params += '&sporttype=' + $('#ddlSports').val();

    $.ajax({
        type: 'POST',
        url: 'GetDataPunter.ashx',
        async: true,
        data: params,
        success: function (rs) {
            $("#spLeagues").html(rs);
            parent.jQuery.unblockUI();
        }
    });
}


function ExportToExcel() {

    var IsLeague = $('input[id=rdByLeague]').is(':checked');
    if (IsLeague) { //League        
        SelTopPunterSummaryByLeaguesExportOnly();
    }
    else //Other
    {
        SelTopPunterSummaryExportOnly();
    }
}

function InitFixedHeader(widthmainpage, name) {

//    if (typeof (name) == 'undefined')
//        name = '';


//    if ($('#trFooter' + name).length == 0) return;

//    var ishide = $('#trFooter' + name).is(':visible');

//    if (!ishide) $('#trFooter' + name).show();

//    var topHeader = $('#trHeader' + name).position().top;
//    var topFooter = $('#trFooter' + name).position().top;

//    if (!ishide) $('#trFooter' + name).hide();

//    var innerHeight = window.innerHeight;
//    var delta = 0;
//    if (!innerHeight) {
//        innerHeight = document.documentElement.clientHeight;
//        delta = 0;
//    }

//    var RealHeight = $(window).height() - topHeader;

    //if ((topFooter - topHeader) > $(window).height()) {

        //jQuery('#table' + name).Scrollable(RealHeight, widthmainpage);
    //}
    //else {
    //    $('#table' + name + ' .rspace').removeClass('rspace');
    //}
}

function SelTopPunterByCustID(CustId, UserName, OrderBy, OrderType, LeagueId) {
    try { parent.BlockUI(); } catch (e) { }
    var fdate, tdate;
    var periodtype = $("input[name=rd]:checked").val();
    var searchby = $("input[name=rdSearch]:checked").val();
    $('#searchby').val(searchby);
    $('#periodtype').val(periodtype);
    $('#custid').val(CustId);
    $('#username').val(UserName);

    var obj = GetDay();
    var fdate, tdate;
    fdate = obj.fdate;
    tdate = obj.tdate;



    var params = '';

    if (LeagueId.length > 0) {
        $('#type').val('SelTopPunterByLeagueCustId');
        params = 'type=SelTopPunterByLeagueCustId';
        $('#SelTopPunterSummaryByLeagueContent').val($("#sData").html());
        $('#SelTopPunterSummaryByLeaguePath').val($('#FullPath').val());
    }
    else {
        $('#type').val('SelTopPunterByCustId');
        params = 'type=SelTopPunterByCustId';
        $('#SelTopPunterSummaryContent').val($("#sData").html());
        $('#SelTopPunterSummarycachepath').val($('#FullPath').val());
    }
    params += '&custid=' + CustId;
    params += '&username=' + UserName;
    params += '&fromdate=' + fdate;
    params += '&todate=' + tdate;
    params += '&orderby=' + OrderBy;
    params += '&ordertype=' + OrderType;
    params += '&leagueid=' + LeagueId;
    params += '&sporttype=' + $('#ddlSports').val();

    $.ajax({
        type: 'POST',
        url: 'GetDataPunter.ashx',
        async: true,
        data: params,
        success: function (rs) {
            var rpt = $.evalJSON(rs);
            $('#SelTopPunterSummaryHeader').val($('#dvMainHeader').html());
            $('#dvMainHeader').html(rpt.header);
            $("#sData").html(rpt.data);
            $('#FullPath').val(rpt.fullpath);
            $('#SelTopPunterByCustIDcachpath').val(rpt.fullpath);

            InitFixedHeader(400);
            parent.jQuery.unblockUI();
        }
    });
}

function BackToMain() {
    try { parent.BlockUI(); } catch (e) { }
    $("#sData").html($('#SelTopPunterSummaryContent').val());
    $('#dvMainHeader').html($('#SelTopPunterSummaryHeader').val());
    $('#FullPath').val($('#SelTopPunterSummarycachepath').val());
    LoadForm($('#searchby').val(), $('#periodtype').val());

    switch ($('#periodtype').val()) {
        case 'month':
            $('#ddlMonth').val($('#fMonth').val());
            $('#ddlYear').val($('#fYear').val());
            break;
        case 'annal':
            $('#ddlYear').val($('#fYear').val());
            break;
        case 'longrange':
            $('#ddfMonth').val($('#fMonth').val());
            $('#ddfYear').val($('#fYear').val());
            $('#ddtMonth').val($('#tMonth').val());
            $('#ddtYear').val($('#tYear').val());
            break;
    }


    parent.jQuery.unblockUI();

    var orderby = $('#orderby').val();
    var ordertype = $('#ordertype').val();
    var sportid = $('#sportid').val();
    var leagueid = $('#leagueid').val();

    var ddlOrderby = document.getElementById('ddlOrderby');
    ddlOrderby.value = orderby;

    var ddlOrderType = document.getElementById('ddlOrderType');
    ddlOrderType.value = ordertype;

    if ($('#ddlSports').length > 0) {
        var ddlSports = document.getElementById('ddlSports');
        ddlSports.value = sportid;

        var ddlLeagues = document.getElementById('ddlLeagues');
        ddlLeagues.value = leagueid;
    }


    if (isIE()) InitFixedHeader(600);
}


function SelTopPunterBySporttype(CustId, UserName, Sporttype, fdate, tdate, OrderBy, OrderType) {
    try { parent.BlockUI(); } catch (e) { }

    $('#SelTopPunterByCustIDContentHeader').val($('#dvMainHeader').html());
    $('#SelTopPunterByCustIDContent').val($('#sData').html());
    $('#SelTopPunterByCustIDcachpath').val($('#FullPath').val());
    $('#custid').val(CustId);
    $('#username').val(UserName);
    $('#sporttype').val(Sporttype);
    $('#type').val('SelTopPunterBySporttype');

    var params = 'type=SelTopPunterBySporttype';
    params += '&custid=' + CustId;
    params += '&username=' + UserName;
    params += '&sporttype=' + Sporttype;
    params += '&fromdate=' + fdate;
    params += '&todate=' + tdate;
    params += '&orderby=' + OrderBy;
    params += '&ordertype=' + OrderType;

    $.ajax({
        type: 'POST',
        url: 'GetDataPunter.ashx',
        async: true,
        data: params,
        success: function (rs) {
            var rpt = $.evalJSON(rs);
            $('#header').val($('#dvMainHeader').html());
            $('#dvMainHeader').html(rpt.header);
            $("#sData").html(rpt.data);
            $('#FullPath').val(rpt.fullpath);

            InitFixedHeader(500);
            parent.jQuery.unblockUI();
        }
    });
}

function BackToSelTopPunterByCustID() {
    try { parent.BlockUI(); } catch (e) { }
    $("#sData").html($('#SelTopPunterByCustIDContent').val());
    $('#dvMainHeader').html($('#SelTopPunterByCustIDContentHeader').val());
    $('#FullPath').val($('#SelTopPunterByCustIDcachpath').val());

    parent.jQuery.unblockUI();
    return;
}

function SelTopPunterByBettype(CustId, UserName, Sporttype, Bettype, fdate, tdate, OrderBy, OrderType) {
    try { parent.BlockUI(); } catch (e) { }

    $('#SelTopPunterBySporttypeHeader').val($('#dvMainHeader').html());
    $('#SelTopPunterBySporttypeContent').val($('#sData').html());
    $('#SelTopPunterBySporttypecatchpath').val($('#FullPath').val());
    $('#custid').val(CustId);
    $('#username').val(UserName);
    $('#sporttype').val(Sporttype);
    $('#bettype').val(Bettype);
    $('#type').val('SelTopPunterByBettype');

    var params = 'type=SelTopPunterByBettype';
    params += '&custid=' + CustId;
    params += '&username=' + UserName;
    params += '&sporttype=' + Sporttype;
    params += '&bettype=' + Bettype;
    params += '&fromdate=' + fdate;
    params += '&todate=' + tdate;
    params += '&orderby=' + OrderBy;
    params += '&ordertype=' + OrderType;

    $.ajax({
        type: 'POST',
        url: 'GetDataPunter.ashx',
        async: true,
        data: params,
        success: function (rs) {
            var rpt = $.evalJSON(rs);
            $('#header').val($('#dvMainHeader').html());
            $('#dvMainHeader').html(rpt.header);
            $("#sData").html(rpt.data);
            $('#FullPath').val(rpt.fullpath);

            InitFixedHeader(500);
            parent.jQuery.unblockUI();
        }
    });
}

function BackToSelTopPunterByBettype() {
    try { parent.BlockUI(); } catch (e) { }
    $("#sData").html($('#SelTopPunterBySporttypeContent').val());
    $('#dvMainHeader').html($('#SelTopPunterBySporttypeHeader').val());
    $('#FullPath').val($('#SelTopPunterBySporttypecatchpath').val());

    parent.jQuery.unblockUI();
    return;
}

function SelIPByCustomer(CustId, UserName, fdate, tdate, leagueid) {
    try { parent.BlockUI(); } catch (e) { }

    $('#custid').val(CustId);
    $('#username').val(UserName);
    var params = 'type=SelIPByCustomer';
    params += '&custid=' + CustId;
    params += '&username=' + UserName;
    params += '&fromdate=' + fdate;
    params += '&todate=' + tdate;
    params += '&leagueid=' + leagueid;

    var url = 'PopupIpBetCount.aspx?' + params;
    ibcWnd.open(url, 'PUNTER ANALYSIS', 60, 200, 440, 200);
    parent.jQuery.unblockUI();

}

function SelBetCountByCustomer(CustId, UserName, fdate, tdate, leagueid) {
    try { parent.BlockUI(); } catch (e) { }

    $('#custid').val(CustId);
    $('#username').val(UserName);
    $('#leagueid').val(leagueid);
    var params = 'type=SelBetCountByCustomer';
    params += '&custid=' + CustId;
    params += '&username=' + UserName;
    params += '&fromdate=' + fdate;
    params += '&todate=' + tdate;
    params += '&leagueid=' + leagueid;

    var url = 'PopupIpBetCount.aspx?' + params;
    ibcWnd.open(url, 'PUNTER ANALYSIS', 60, 200, 540, 200);
    parent.jQuery.unblockUI();
    return;
}



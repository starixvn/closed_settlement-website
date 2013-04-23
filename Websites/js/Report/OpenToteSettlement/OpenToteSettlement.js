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

function InitFixedHeader(widthmainpage) {


    if ($('#trFooter').length == 0) return;

    var ishide = document.getElementById('trFooter').style.display.length > 0;

    if (ishide) document.getElementById('trFooter').style.display = '';

    var topHeader = $('#trHeader').position().top;
    var topFooter = $('#trFooter').position().top;

    if (ishide) document.getElementById('trFooter').style.display = 'none';

    var innerHeight = window.innerHeight;
    var delta = 0;
    if (!innerHeight) {
        innerHeight = document.documentElement.clientHeight;
        delta = 0;
    }

    var RealHeight = $(window).height() - topHeader-30 ;

    if ((topFooter - topHeader) > $(window).height()) {

        jQuery('#table').Scrollable(RealHeight, widthmainpage);
    }
    else {
        $('#table .rspace').removeClass('rspace');
    }
}

function WinlostHR_OpenTote() {
    var db = 'rep';
    if ($('input[id=rdMain]').is(':checked')) {
        if (document.getElementById('trGetData').style.display != 'none')
            db = 'main';
    }
        

    $('#db').val(db);

    var IsTest = getPramJs('IsTest');
    var params = '';
    params += 'type=WinlostHR_OpenTote';
    params += '&winlostdate=' + $('#fdate').val();
    params += '&db=' + db;
    params += '&IsTest=' + IsTest;


    try{parent.BlockUI();}catch(e){}
    $.ajax({
        type: 'POST',
        url: 'OpenToteSettlement.ashx',
        async: true,
        data: params,
        success: function(rs) {
            $("#sContent").html(rs);
            InitFixedHeader(2300);
            parent.jQuery.unblockUI();
        }
    });
}

function ViewReport() {
    WinlostHR_OpenTote();
}

function WinlostHR_OpenTote_Details(winlostdate, partnertype, poolcurrency, db) {
    //alert('winlostdate: ' + winlostdate + '\npartnertype: ' + partnertype + '\npoolcurrency: ' + poolcurrency + '\ndb: ' + db);
    $('#WinlostHR_OpenToteTitle').val($('#spTitle').html());
    $('#WinlostHR_OpenToteHeader').val($('#sHeader').html());
    $('#WinlostHR_OpenToteContent').val($('#sContent').html());
    $('#winlostdate').val(winlostdate);
    
    $('#spTitle').html('Racing Daily Report For Racing ' + partnertype);

    var IsTest = getPramJs('IsTest');

    var evt = "ExportToExcelDetail('" + winlostdate + "','" + partnertype + "','" + poolcurrency + "','" + db + "','" + IsTest + "')";
    $('#sHeader').html('<b> <span class="rpt-link" onclick="BackToMain();">Main</span></b> > ' + partnertype + '   <img onclick="' + evt + '" class="hand" src="../../App_Themes/Images/excel.gif" />');

    var params = '';
    params += 'type=WinlostHR_OpenTote_Details';
    params += '&winlostdate=' + winlostdate;
    params += '&partnertype=' + partnertype;
    params += '&poolcurrency=' + poolcurrency;
    params += '&db=' + db;
    params += '&IsTest=' + IsTest;

    

    $('#db').val(db);

    try{parent.BlockUI();}catch(e){}
    $.ajax({
        type: 'POST',
        url: 'OpenToteSettlement.ashx',
        async: true,
        data: params,
        success: function(rs) {
            $("#sContent").html(rs);
            InitFixedHeader(2300);
            parent.jQuery.unblockUI();
        }
    });
}

function BackToMain() {
    try{parent.BlockUI();}catch(e){}
    var params = '';
    params += 'type=LoadHeader';
    params += '&winlostdate=' + $('#winlostdate').val();
    params += '&db=' + $('#db').val();

    $.ajax({
        type: 'POST',
        url: 'OpenToteSettlement.ashx',
        async: true,
        data: params,
        success: function(rs) {
            $("#sHeader").html(rs);
            $('#spTitle').html($('#WinlostHR_OpenToteTitle').val());
            $('#sContent').html($('#WinlostHR_OpenToteContent').val());

            if ($('#db').val() == 'rep') SetGetData('rdRep');
            else SetGetData('rdMain')
            SetEvtGetdate();
            parent.jQuery.unblockUI();
        }
    });
}


function PopUpWindow(url, width, height) {
    var top = (screen.height - height) / 2;
    var left = (screen.width - width) / 2;
    var newwindow = window.open(url, "BeList", "height=" + height + ",width=" + width + ",top=" + top + ",left=" + left + ",scrollbars=1");
    if (newwindow) newwindow.focus();
}

function ViewBetList(meetingid, raceNo, currencyID, winlostdate, db, IsTest) {
    try{parent.BlockUI();}catch(e){}  
    
    var params = '';
    params += 'meetingid=' + meetingid;
    params += '&raceNo=' + raceNo;
    params += '&currencyid=' + currencyID;
    params += '&winlostdate=' + winlostdate;
    params += '&db=' + db;
    params += '&IsTest=' + IsTest;
    
    var url = 'OpenToteBetList.aspx?' + params;
    //ibcWnd.open(url, 'Racing OpenTote Detail Bet List', 60, 200, 1120, 300);
    PopUpWindow(url, 868, 600);
    
    parent.jQuery.unblockUI();
}

function ExportToExcel() {
    try{parent.BlockUI();}catch(e){}

    var IsTest = getPramJs('IsTest');
    
    var params = 'type=ExportToExcel_OpenTote';
    params += '&winlostdate=' + $('#fdate').val();
    params += '&db=' + $('#db').val();
    params += '&IsTest=' + IsTest;

    $("#frm").attr('src', '../ExportExcelFile.aspx?' + params);
    parent.jQuery.unblockUI();
}

function ExportToExcelDetail(winlostdate, partnertype, poolcurrency, db, IsTest) {
    try{parent.BlockUI();}catch(e){}
    var params = 'type=ExportToExcel_OpenToteDetail';
    params += '&winlostdate=' + winlostdate;
    params += '&partnertype=' + partnertype;
    params += '&poolcurrency=' + poolcurrency;
    params += '&db=' + db;
    params += '&IsTest=' + IsTest;

    $("#frm").attr('src', '../ExportExcelFile.aspx?' + params);
    parent.jQuery.unblockUI();
}

function Comparision(winlostdate, ibcTO, ibcDividendWon) {
    //alert(winlostdate + '\n' + ibcTO + '\n' + ibcDividendWon);
    var url = 'Comparision.aspx?winlostdate=' + winlostdate;
    url += '&ibcTO=' + ibcTO;
    url += '&ibcDividendWon=' + ibcDividendWon;
    var title = 'OpenTote Comparision';
    var wnd = ibcWnd.open(url, title, 60, 50, 480, 200);
}
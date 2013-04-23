function BlockUI() {
    jQuery.blockUI({
        message: $('img#displayBox'),
        fadeIn: 0,
        fadeOut: 0,
        css: {
            top: ($(window).height() - 32) / 2 + 'px',
            left: ($(window).width() - 32) / 2 + 'px',
            width: '32px',
            border: 'none'
        }
    });
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


function InitFixedHeader(widthmainpage) {


    if ($('#trFooter').length == 0) return;

    var ishide = $('#trFooter').is(':visible');

    //if (ishide) $('#trFooter').show();

    var topHeader = $('#trHeader').position().top;
    var topFooter = $('#trFooter').position().top;

    //if (ishide) $('#trFooter').hide();

    var innerHeight = window.innerHeight;
    var delta = 0;
    if (!innerHeight) {
        innerHeight = document.documentElement.clientHeight;
        delta = 0;
    }

    var RealHeight = $(window).height() - topHeader ;

    if ((topFooter - topHeader) > $(window).height()) {

        jQuery('#table').Scrollable(RealHeight, widthmainpage);
    }
    else {
        $('#table .rspace').removeClass('rspace');
    }
}


function ViewReport(IsSendMail) {

    if (IsSendMail == 1) {
        if (!confirm('Are you sure you want to send email this report?')) return;
    }
    try{parent.BlockUI();}catch(e){}
    $("#spMsg").html('');
    IsSendMail = (!IsSendMail) ? '0' : IsSendMail;
    var param = '';
    param += 'fdate=' + $('#fdate').val();
    param += '&tdate=' + $('#tdate').val();
    param += '&type=' + $('#type').val();
    param += '&IsSendMail=' + IsSendMail;
    $.ajax(
                 {
                     type: "POST",
                     url: "HRGroupDaily.ashx",
                     data: param,
                     success: function(rs) {
                         var rpt = $.evalJSON(rs);
                         $("#spMsg").html(rpt.msg);
                         if (IsSendMail == 0) {
                             $("#sContent").html(rpt.data);
                             var IsToteOnly = getPramJs('type') == 'tote';
                             var width = 1180;
                             if (IsToteOnly)
                                 width = 1100;
                             InitFixedHeader(width);
                         }
                         parent.jQuery.unblockUI();
                     }
                 }
             );
}

function GetJSONResult(rs) {
    
    
}

function ExportToExcel() {
    try{parent.BlockUI();}catch(e){}
    var param = '';
    param += 'fdate=' + $('#fdate').val();
    param += '&tdate=' + $('#tdate').val();
    

    var IsToteOnly = getPramJs('type') == 'tote';
    if (IsToteOnly) {
        param += '&type=ExportHRGroupDaily_ToteOnly';
    }
    else {
        param += '&type=ExportHRGroupDaily';
    }

    $("#frm").attr('src', '../ExportExcelFile.aspx?' + param);
    
    parent.jQuery.unblockUI();
}


window.onload = function() {
    var tdToDate = document.getElementById('tdToDate');
    tdToDate.style.width = '50px';
}
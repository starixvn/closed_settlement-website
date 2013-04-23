


function ViewReport() {

    $('#msg').html('');

    var ischecked = $('input[name=chk]').is(':checked')

    if (!ischecked) {
        $('#msg').html('Please select a statistic type.');
        return;
    }   
    
    
    try{parent.BlockUI();}catch(e){}
    var fMonth = parseInt($('#ddfMonth').val());
    var fYear = parseInt($('#ddfYear').val());
    var tMonth = parseInt($('#ddtMonth').val());
    var tYear = parseInt($('#ddtYear').val());
    var Statistic = 0;
    var username = $('#txtUserName').val();

    var chkWL = $('input[id=chkWL]').is(':checked')
    var chkTO = $('input[id=chkTO]').is(':checked')
    var chkMar = $('input[id=chkMar]').is(':checked')
    if (chkWL) Statistic += 4;
    if (chkTO) Statistic += 2;
    if (chkMar) Statistic += 1;

    var params = 'type=DirectCustomerReport';
    params += '&fMonth=' + fMonth;
    params += '&fYear=' + fYear;
    params += '&tMonth=' + tMonth;
    params += '&tYear=' + tYear;
    params += '&Statistic=' + Statistic;
    params += '&username=' + username;

    $.ajax({
        type: 'POST',
        url: 'GetDataAgency.ashx',
        async: true,
        data: params,
        success: function(rs) {
            var rpt = $.evalJSON(rs);
            $("#sData").html(rpt.data);
            $('#FullPath').val(rpt.fullpath);
            var TotalColumn = parseInt(rpt.TotalColumn) * 100 + 80;
            $('#dvTable').show();
            InitFixedHeader(TotalColumn);
            try { parent.jQuery.unblockUI(); } catch (e) { }
        }
    });

}


function ExportExcelOnly() {
    $('#msg').html('');

    var ischecked = $('input[name=chk]').is(':checked')

    if (!ischecked) {
        $('#msg').html('Please select a statistic type.');
        return;
    }


    try { parent.BlockUI(); } catch (e) { }
    var fMonth = parseInt($('#ddfMonth').val());
    var fYear = parseInt($('#ddfYear').val());
    var tMonth = parseInt($('#ddtMonth').val());
    var tYear = parseInt($('#ddtYear').val());
    var Statistic = 0;
    var username = $('#txtUserName').val();

    var chkWL = $('input[id=chkWL]').is(':checked')
    var chkTO = $('input[id=chkTO]').is(':checked')
    var chkMar = $('input[id=chkMar]').is(':checked')
    if (chkWL) Statistic += 4;
    if (chkTO) Statistic += 2;
    if (chkMar) Statistic += 1;

    var params = 'type=DirectCustomerReportExportOnly';
    params += '&fMonth=' + fMonth;
    params += '&fYear=' + fYear;
    params += '&tMonth=' + tMonth;
    params += '&tYear=' + tYear;
    params += '&Statistic=' + Statistic;
    params += '&username=' + username;

    $.ajax({
        type: 'POST',
        url: 'GetDataAgency.ashx',
        async: true,
        data: params,
        success: function(rs) {
            var rpt = $.evalJSON(rs);
            $('#FullPath').val(rpt.fullpath);
            $("#frm").attr('src', '../../Others/GetExcelFile.aspx?path=' + rpt.fullpath);            
            jQuery.unblockUI();
        }
    });
}

function ExportToExcel() {

    $('#msg').html('');

    var ischecked = $('input[name=chk]').is(':checked')

    if (!ischecked) {
        $('#msg').html('Please select a statistic type.');
        return;
    }


    try { parent.BlockUI(); } catch (e) { }
    var fMonth = parseInt($('#ddfMonth').val());
    var fYear = parseInt($('#ddfYear').val());
    var tMonth = parseInt($('#ddtMonth').val());
    var tYear = parseInt($('#ddtYear').val());
    var Statistic = 0;
    var username = $('#txtUserName').val();

    var chkWL = $('input[id=chkWL]').is(':checked')
    var chkTO = $('input[id=chkTO]').is(':checked')
    var chkMar = $('input[id=chkMar]').is(':checked')
    if (chkWL) Statistic += 4;
    if (chkTO) Statistic += 2;
    if (chkMar) Statistic += 1;

    var params = '?type=DirectCustomerReport';
    params += '&fMonth=' + fMonth;
    params += '&fYear=' + fYear;
    params += '&tMonth=' + tMonth;
    params += '&tYear=' + tYear;
    params += '&Statistic=' + Statistic;
    params += '&username=' + username;

    if ($("#sData").html().length == 0) {
        params += '&IsExportOnly=1';
    }

    $('#frm').attr('src', 'AgencyAnalysis.aspx' + params);

    try { parent.jQuery.unblockUI(); } catch (e) { }
   
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

    var RealHeight = $(window).height() - topHeader - 30;

    if ((topFooter - topHeader) > $(window).height()) {
        
        jQuery('#table').Scrollable(RealHeight, widthmainpage);
    }
    else {
        $('#table .rspace').removeClass('rspace');
    }
}

function ChangeYear(Year) {
    var SelectYear = parseInt(Year.value);
    var EndYear = SelectYear + 1;
    var date = new Date();
    var CurrYear = date.getFullYear(date);

    if (EndYear > CurrYear)
        EndYear = CurrYear;

    var strOPtion = '';
    for (i = SelectYear; i <= EndYear; i++) {
        strOPtion += "<option value='" + i + "'>" + i + "</option>";
    }
    $('#ddtYear').children().remove().end().append(strOPtion);
}


function GenerateChartByCusId(custid, username) {
    try { parent.BlockUI(); } catch (e) { }

    var fMonth = parseInt($('#ddfMonth').val());
    var fYear = parseInt($('#ddfYear').val());
    var tMonth = parseInt($('#ddtMonth').val());
    var tYear = parseInt($('#ddtYear').val());
    
    var params = '?type=ChartByCustId';
    params += '&custid=' + custid;
    params += '&username=' + username;
    params += '&fMonth=' + fMonth;
    params += '&fYear=' + fYear;
    params += '&tMonth=' + tMonth;
    params += '&tYear=' + tYear;


    $("#frm").attr('src', 'AgencyAnalysis.aspx' + params);
    try { parent.jQuery.unblockUI(); } catch (e) { }
}

function CheckOnStatistic() {
    $('#msg').html('');

    var ischecked = $('input[name=chk]').is(':checked')

    if (!ischecked) {
        $('#msg').html('Please select a statistic type.');
        return;
    }   
}
function InitFixedHeader(widthmainpage, tblID, idFooter, idHeader) {


    if ($('#' + idFooter).length == 0) return;

    var ishide = document.getElementById(idFooter).style.display.length > 0;

    if (ishide) document.getElementById(idFooter).style.display = '';

    var topHeader = $('#' + idHeader).position().top;
    var topFooter = $('#' + idFooter).position().top;

    if (ishide) document.getElementById(idFooter).style.display = 'none';

    var innerHeight = window.innerHeight;
    var delta = 0;
    if (!innerHeight) {
        innerHeight = document.documentElement.clientHeight;
        delta = 0;
    }

    var RealHeight = $(window).height() - topHeader - 30;

    if ((topFooter - topHeader) > $(window).height()) {

        jQuery('#' + tblID).Scrollable(RealHeight, widthmainpage);
    }
    else {
        $('#' + tblID + ' .rspace').removeClass('rspace');
    }
}
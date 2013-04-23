

function InitFixedHeader() {

    var widthmainpage = 700;
    if ($('#trFooter').length == 0) return;

    var topHeader = $('#trHeader').position().top;
    var topFooter = $('#trFooter').position().top;
    

    var innerHeight = window.innerHeight;
    var delta = 0;
    if (!innerHeight) {
        innerHeight = document.documentElement.clientHeight;
        delta = 0;
    }

    var RealHeight = $(window).height() - topHeader - 50;

    if ((topFooter - topHeader) > $(window).height()) {
        jQuery('#table').Scrollable(RealHeight, widthmainpage);
    }
    else {
        $('#table .rspace').removeClass('rspace');
    }
}

function GetNonePTMemSummary(pageindex) {

    parent.BlockUI();

    var username = $('#txtUserName').val();
    var param = 'username=' + username;
    param += '&pageindex=' + pageindex;

    

    $.ajax({
        type: 'POST',
        url: 'NonePTMemSummary.ashx',
        async: true,
        data: param,
        success: function(rs) {
            $("#sContent").html(rs);
            InitFixedHeader();


            //bind event for pager
            $(".pagenav_ibtn").click(function() {
                if (isNumberNotNull("txtpage")) {
                    pageindex = $("#txtpage").val();
                    $('#currpage').val(pageindex);
                    GetNonePTMemSummary(pageindex);
                    return false;
                }
            });

            $(".pagerlink").click(function() {
                var hash = this.href;
                hash = hash.replace(/^.*#/, '');
                $('#currpage').val(hash);
                GetNonePTMemSummary(hash);
                return false;
            });

            parent.jQuery.unblockUI();
        }
    });
}

function Export2Excel() {
    var username = $('#txtUserName').val();
    var pageindex = $('#currpage').val();
    
    var param = 'username=' + username;
    param += '&pageindex=' + pageindex;
    param += '&type=NonePTMemSummary';
    $("#frm").attr('src', '../ExportExcelFile.aspx?' + param);
}
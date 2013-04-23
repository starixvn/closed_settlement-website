
function IsValidFileExtension() {
    var filename = $('#txtfile').val();
    $('#FileName').val(filename);
    var isOK = false;
    $('#tdToDate').attr('width', 50);
    if (filename != "") {
        var ext = filename.substring(filename.lastIndexOf('.') + 1, filename.length).toLowerCase();
        isOK = (ext == "xls");
        if (!isOK) {
            $('#dvCalendar').hide();
            $('#msgError').show();
            $('#msgError').html("<span class='error-message'>Sorry, You chose invalid file extension. Please choose an excel file.</span>");
        }
        else {
            $('#msgError').html('');
            isOK = true;
        }
        $('#content').html('');
        $('#btnSubmit').attr('disabled', true);
    }
    return isOK;
}

window.onload = function() {
    $('#btnViewReport').attr('disabled', true);
    $('#btnExport').attr('disabled', true);
}

function UploadFile() {
    if (IsValidFileExtension()) {
        try{parent.BlockUI();}catch(e){}
        $.ajaxFileUpload
                (
                    {
                        url: '../Handlers/ExportStatement.ashx',
                        secureuri: false,
                        fileElementId: 'txtfile',
                        dataType: 'html',
                        complete: function() { try{parent.jQuery.unblockUI();}catch(e){} }, // Disable image pending.
                        success: GetInfoJSON

                    }
                )
    }

}


function GetInfoJSON(s) {
    var arr = s.split('|');
    var sData = arr[0];
    var sUsersListValid = arr[1];
    var success = arr[2];

    var dvCalendar = document.getElementById('dvCalendar');
    dvCalendar.style.display = 'none';
    $('#msgError').hide();
    $('#content').html(sData);
    $('#userslistvalid').val(sUsersListValid);
    $('#bolDelDataTmp').val(1);

    $('#btnViewReport').attr('disabled', false);
    $('#btnExport').attr('disabled', false);
    if (success == '1') {
        dvCalendar.style.display = '';
        $('#tdToDate').attr('width', 50);
        InitFixedHeaderAccList(250);
    }
}


window.onunload = DelDataTmp;
function DelDataTmp() {   
    if ($('#bolDelDataTmp').val() == 1) {
        $.ajax({
            type: 'POST',
            url: '../Handlers/ExportStatement.ashx',
            async: true,
            data: 'bolUnLoadPage=1',
            success: function(rs) {           
            }
        });
    }
}


function InitFixedHeaderAccList(widthmainpage) {
    //var widthmainpage = 200;
    if ($('#trFooter').length == 0) return;

    $('#trFooter').show();
    document.getElementById('trFooter').style.display = '';
    var topHeader = $('#trHeader').position().top;
    var topFooter = $('#trFooter').position().top;
    $('#trFooter').hide();

    var innerHeight = window.innerHeight;
    var delta = 0;
    if (!innerHeight) {
        innerHeight = document.documentElement.clientHeight;
        delta = 0;
    }
    var isIE = $.browser.msie;
    var RealHeight = $(window).height() - topHeader - 30;
    if ((topFooter - topHeader) > $(window).height()) {
        if (isIE) $('#table .rspace').removeClass('rspace');
        jQuery('#table').Scrollable(RealHeight, widthmainpage);
    }
    else {
        $('#table .rspace').removeClass('rspace');
    }

}

function ViewReport() {
    try{parent.BlockUI();}catch(e){}
    var param = 'mode=1';
    param += '&fdate=' + $('#fdate').val();
    param += '&tdate=' + $('#tdate').val();
    param += '&userslistvalid=' + $('#userslistvalid').val();

    $.ajax({
        type: 'POST',
        url: '../Handlers/ExportStatement.ashx',
        async: true,
        data: param,
        success: function(rs) {
            document.getElementById('content').innerHTML = rs;
            try{parent.jQuery.unblockUI();}catch(e){}
            InitFixedHeaderAccList(850);
        }
    });
}

function ExportToExcel() {
    //var path = $('#FullPath').val();
    //$("#frm").attr('src', 'GetExcelFile.aspx?path=' + path);
    var param = 'type=ExportStatement';
    param += '&fdate=' + $('#fdate').val();
    param += '&tdate=' + $('#tdate').val();
    param += '&userslistvalid=' + $('#userslistvalid').val();
    //../Report/ExportExcelFile.aspx

    try{parent.BlockUI();}catch(e){}
    $("#frm").attr('src', '../Report/ExportExcelFile.aspx?' + param);
    try{parent.jQuery.unblockUI();}catch(e){}
}
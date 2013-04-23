var defaultSearch = "SpName or Action name";
function Blur(obj) {
    if ($.trim(obj.value) == "")
        obj.value = defaultSearch;
}
function Focus(obj) {
    if ($.trim(obj.value) == defaultSearch) {
        obj.value = '';
    }
}
function BlockUI() {
    $.blockUI({
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
function GetLogActionContent() {
    var urlLink;
    var txtFilter = $('#txtFilter').attr("value");
    var lang = $('#dlLanguage').attr("value");
    var actionTypeID = $('#ddlActionType').attr("value");
    if ($.trim(txtFilter) == defaultSearch) {
        txtFilter = "";
    }
    BlockUI();
    urlLink = 'LogAction.ashx?type=LogAction&filter=' + encodeURIComponent(txtFilter) + '&lang=' + lang + '&actiontype=' + actionTypeID;
    GeneralAjaxCall(urlLink);
}
function GetLogActionTypeContent() {
    var urlLink;
    BlockUI();
    urlLink = 'LogAction.ashx?type=LogActionType';
    GeneralAjaxCall(urlLink);
}

function GeneralAjaxCall(urlLink) {
    $.ajax({
        url: urlLink + '&nocache=' + new Date().getTime(),
        dataType: 'html',
        success: function(data) {
            $('#sContent').html(data);
            $.unblockUI();
        }
    });
}

function PreSubmit(e) {
    e = (e) ? e : window.event;
    var key = (e.keyCode) ? e.keyCode : e.which;
    if (key == 13)
        $("#btnSubmit").click();
}
function EditLogAction(ActionID) {
    
    var url = 'LogActionUpdate.aspx?ActionID=' + ActionID;
    ShowPopup(url, 'Edit Log Action', 650, 280, 50, 200);
}
function EditLogActionType(ActionTypeID) {
    var url = 'LogActionTypeUpdate.aspx?ActionTypeID=' + ActionTypeID;
    ShowPopup(url, 'Edit Log Action Type', 650, 280, 50, 200);
}
function UpdateAction() {
    $('#divErr').hide();
    var txtEN = $('#txtEN').attr("value");
    if ($.trim(txtEN) == '') {
        alert("English Name cannot be left blank!");
        $('#txtEN').focus();
        return;
    }
    
    var txtSPName = $('#txtSPName').attr("value");
    if ($.trim(txtSPName) == '') {
        alert("SP Name cannot be left blank!");
        $('#txtSPName').focus();
        return;
    }
    
    var actionID = $('#txtActionID').attr("value");
    var txtCH = $('#txtCH').attr("value");
    var txtCS = $('#txtCS').attr("value");
    var txtJP = $('#txtJP').attr("value");
    var txtTH = $('#txtTH').attr("value");
    var txtKO = $('#txtKO').attr("value");
    var txtIT = $('#txtIT').attr("value");
    var isInternal = ($('#rdYes').attr("checked") == false) ? 0 : 1;
    var actionType = $('#ddlActionType').attr("value");

    var urlLink = 'LogAction.ashx?type=LogactionUpdate&actionid=' + actionID;
    urlLink = urlLink + '&txtEN=' + encodeURIComponent(txtEN);
    urlLink = urlLink + '&txtCH=' + encodeURIComponent(txtCH);
    urlLink = urlLink + '&txtCS=' + encodeURIComponent(txtCS);
    urlLink = urlLink + '&txtJP=' + encodeURIComponent(txtJP);
    urlLink = urlLink + '&txtTH=' + encodeURIComponent(txtTH);
    urlLink = urlLink + '&txtKO=' + encodeURIComponent(txtKO);
    urlLink = urlLink + '&txtIT=' + encodeURIComponent(txtIT);
    urlLink = urlLink + '&internal=' + isInternal;
    urlLink = urlLink + '&actionType=' + actionType;
    urlLink = urlLink + '&txtSPName=' + encodeURIComponent(txtSPName);

    $.ajax({
        url: urlLink + '&nocache=' + new Date().getTime(),
        dataType: 'html',
        success: function(data) {
            $.unblockUI();

            var arr = data.split('|');
            if (arr.length > 1 && arr[0] == 'err') {
                //alert(arr[1]);
                $('#divErr').html(arr[1]);
                $('#divErr').show();
            }
            else {
                ClosePopup();
                GetLogActionContent();
            }
        }
    });

}
function MenuGoTo(menu) {
    if (menu == 'Action') {
        document.location.href = 'LogAction.aspx';
    }
    else {
        document.location.href = 'LogActionType.aspx';
    }
}

function OnColor(obj, e, index) {
    if (e == 0)
        obj.className = "mouseMove";
    else
        obj.className = ((index == 0) ? "oddrow" : "evenrow");
}
function UpdateActionType() {
    $('#divErr').hide();
    var txtEN = $('#txtEN').attr("value");
    if ($.trim(txtEN) == '') {
        alert("English Name cannot be left blank!");
        $('#txtEN').focus();
        return;
    }



    var actionTypeID = $('#txtActionTypeID').attr("value");
    var txtCH = $('#txtCH').attr("value");
    var txtCS = $('#txtCS').attr("value");
    var txtJP = $('#txtJP').attr("value");
    var txtTH = $('#txtTH').attr("value");
    var txtKO = $('#txtKO').attr("value");
    var txtIT = $('#txtIT').attr("value");


    var urlLink = 'LogAction.ashx?type=LogactionTypeUpdate&actiontypeid=' + actionTypeID;
    urlLink = urlLink + '&txtEN=' + encodeURIComponent(txtEN);
    urlLink = urlLink + '&txtCH=' + encodeURIComponent(txtCH);
    urlLink = urlLink + '&txtCS=' + encodeURIComponent(txtCS);
    urlLink = urlLink + '&txtJP=' + encodeURIComponent(txtJP);
    urlLink = urlLink + '&txtTH=' + encodeURIComponent(txtTH);
    urlLink = urlLink + '&txtKO=' + encodeURIComponent(txtKO);
    urlLink = urlLink + '&txtIT=' + encodeURIComponent(txtIT);

    $.ajax({
        url: urlLink + '&nocache=' + new Date().getTime(),
        dataType: 'html',
        success: function(data) {
            $.unblockUI();
            var arr = data.split('|');
            if (arr.length > 1 && arr[0] == 'err') {
                //alert(arr[1]);
                $('#divErr').html(arr[1]);
                $('#divErr').show();
            }
            else {
                ClosePopup();
                GetLogActionTypeContent();
            }
        }
    });
}
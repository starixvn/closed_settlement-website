
/* Lewin
get query string value */
function getQuerystring(key, default_) {
    if (default_ == null) default_ = "";
    key = key.replace(/[\[]/, "\\\[").replace(/[\]]/, "\\\]");
    var regex = new RegExp("[\\?&]" + key + "=([^&#]*)");
    var qs = regex.exec(window.location.href);
    if (qs == null)
        return default_;
    else
        return qs[1];

}
$(document).ready(function() {

    $('#txtUserName').attr('value', getQuerystring('custname'));
    $('#RequiredUserName').hide();
    $('#Search').bind('click', null, DoSearch);

    //bind event keypress, trigger when user enter on username
    $('#txtUserName').bind('keypress', function(e) {
        var key = e.charCode ? e.charCode : e.keyCode ? e.keyCode : 0;
        var target = e.target.tagName.toLowerCase();
        if (key === 13 && target === 'input') {
            e.preventDefault();
            DoSearch();
        }
    });


    /*Focus tabs*/
    var tabId = getQuerystring('tabid');
    /* focus on customer setting by default*/
    if (tabId.length == 0)
        tabId = 'CustomerSetting';
    tabId = "#" + tabId;
    $(tabId).addClass('current');

    /*show/hide by logactiontype*/
    if (tabId.toLowerCase() == '#customersetting') {

        $('#ddlLogActionType').show();
        $(ddlActionType).val(GetActionType());
        //auto postback
        //$(ddlActionType).change(DoSearch);
    }
    else {
        $('#ddlLogActionType').hide();
    }

    /*show/hide Link to get old data (before 1-1-2009)*/
    if (tabId.toLowerCase() == '#login') {
        $('#loginMsg').show();
        $('#lnkOld').hide();
    }
    else {
        $('#lnkOld').show();
        $('#loginMsg').hide();
    }


    Calendar.setup({
        animation: false,
        showTime: false,
        min: 20090101,
        trigger: "imgCalFrom",
        inputField: "txtFromDate",
        dateFormat: "%m/%d/%Y",
        onSelect: function() {
            this.hide();
        }
    });

    Calendar.setup({
        animation: false,
        showTime: false,
        min: 20090101,
        trigger: "imgCalTo",
        inputField: "txtToDate",
        dateFormat: "%m/%d/%Y",
        onSelect: function() {

            this.hide();
        }
    });



    /*
    Calendar.setup({
    inputField: "txtFromDate",
    ifFormat: "%m/%d/%Y",
    showsTime: false,
    button: "imgCalFrom",
    singleClick: true,
    showOthers: true,
    step: 1
    });

    Calendar.setup({
    inputField: "txtToDate",
    ifFormat: "%m/%d/%Y",
    showsTime: false,
    button: "imgCalTo",
    singleClick: true,
    showOthers: true,
    step: 1
    }
    );
    */
});


function DoSearch() {
    
    var tabId = getQuerystring('tabid');
    var custName = $('#txtUserName').attr("value");
    if ($.trim(custName).length > 0) {
        var fromDate = $('#txtFromDate').attr("value");
        var toDate = $('#txtToDate').attr("value");

        var queryString = "?tabid=" + tabId + "&custname=" + $.trim(custName) + "&fromdate=" + fromDate + "&todate=" + toDate + "&actiontype=" + $(ddlActionType).val();
        //ibc.DelayReloadPage();
        try{parent.BlockUI();}catch(e){}
        //search on specific page
        location = GetPageByTabId(tabId) + queryString;
    }
    else {

        //show error warning
        $('#RequiredUserName').show();
        $('#txtUserName').focus();
        $('#txtUserName').attr('value', '');
        $('#displayBox').hide();
        return false; //do nothing
    }
}

function GetActionType() {
    if (getQuerystring('actiontype') == '') {
        return $(ddlActionType).val();
    }

    return getQuerystring('actiontype');
}

function EnterUserEvent() {
    var e = window.event;
    var key = e.charCode ? e.charCode : e.keyCode ? e.keyCode : 0;
    var target = e.target.tagName.toLowerCase();
    if (key === 13 && target === 'input') {
        //e.preventDefault();
        DoSearch();
    }
}

//return page name by tabid
function GetPageByTabId(tabid) {
    var url = "LogCustomerSetting.aspx";

    switch (tabid) {
        case "Status": url = "LogStatus.aspx";
            break;
        case "Credit": url = "LogCredit.aspx";
            break;
        case "Login": url = "LogLogin.aspx";
            break;
        default: url = "LogCustomerSetting.aspx";
    }

    return url;
}

//create url and combine with query string
function GetUrlByTabId(tabid, pageindex) {

    var url = GetPageByTabId(tabid);
    var queryString = "?tabid=" + tabid + "&custname=" + getQuerystring("custname") + "&fromdate=" + getQuerystring("fromdate") + "&todate=" + getQuerystring("todate") + "&pageindex=" + (pageindex - 1).toString() + "&actiontype=" + $(ddlActionType).val(); ;
    return url + queryString;
}


function NavigatePage(page, url, site) {

    //var tabid = getQuerystring("tabid");
    //var queryString = "?tabid=" + tabid + "&custname=" + getQuerystring("custname") + "&fromdate=" + getQuerystring("fromdate") + "&todate=" + getQuerystring("todate") + "&pageindex=" + (page - 1).toString();

    //reload page
    try{parent.BlockUI();}catch(e){}
    var tabid = getQuerystring("tabid");
    //ibc.DelayReloadPage();
    window.location = GetUrlByTabId(tabid, page);
}

//redirect to menu item
function MenuGoTo(tabid) {
    try{parent.BlockUI();}catch(e){}
    var custName = $('#txtUserName').attr("value");
    var fromDate = $('#txtFromDate').attr("value");
    var toDate = $('#txtToDate').attr("value");
    var queryString = "?tabid=" + tabid + "&custname=" + $.trim(custName) + "&fromdate=" + fromDate + "&todate=" + toDate;
    location = GetPageByTabId(tabid) + queryString;
}

function GotoPageNext(url, site) {

    page = $('#txtpage').attr("value");

    if (page == null || page == '') {
        $('#txtpage').focus();
        return false;
    }

    var tabid = getQuerystring("tabid");
    //var queryString = "?tabid=" + tabid + "&custname=" + getQuerystring("custname") + "&fromdate=" + getQuerystring("fromdate") + "&todate=" + getQuerystring("todate") + "&pageindex=" + (page - 1).toString();
    //ibc.DelayReloadPage();
    window.location = GetUrlByTabId(tabid, page);
}
function OldForm() {
    var tabId = getQuerystring('tabid');
    var custName = $('#txtUserName').attr("value");
    if (tabId.length == 0)
        tabId = 'CustomerSetting';

    if (tabId.toLowerCase() == "credit") {
        if ($.trim(custName) == '')
            document.location.href = 'CreditLog.aspx';
        else
            document.location.href = 'CreditLog.aspx?txtUsername=' + custName + '&hdSearch=Search';
    }
    else {
        if ($.trim(custName) == '')
            document.location.href = 'Logs.aspx';
        else
            document.location.href = 'Logs.aspx?txtUsername=' + custName + '&hdSearch=Search';
    }
}    


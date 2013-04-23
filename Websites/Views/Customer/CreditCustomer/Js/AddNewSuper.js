/// <reference path="../../../../js/jQuery/jquery.js" />
/// <reference path="../../../../js/jQuery/json2.js" />
/// <reference path="../../../../js/PT/CommonPT.js" />
//load function
$(function () {
    ans.init();
});

var customer;
var context;
var ptGrid = null;
var bettingLimitGrid = null;
var isInsterTwice = false;

var ans = {
    ptGrid: null
};
//init
ans.init = function () {
    ans.initContext();
};
//biding for knockout
ans.initForKnockout = function () {
    var CustomerModel = function (customer) {
        this.saveCustomer = function () {
            //console.log(JSON.stringify(ko.toJS(customer), null, '\t'));
        };
    };
    ko.applyBindings(new CustomerModel(customer));
};
//get context of form
ans.initContext = function () {
    $.ajax({
        type: "POST",
        url: "GetContextObject",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        data: "",
        success: function (stringContext) {
            context = JSON.parse(stringContext);
            ans.initCustomer();
        }
        , error: function (e) {
            showAlertMessage("err", "Init Context Error", 1);
        }
    });
};
//get default super value
ans.initCustomer = function () {
    $.ajax({
        type: "POST",
        url: "GetCustomerObject",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        data: "",
        success: function (stringCustomer) {
            customer = JSON.parse(stringCustomer);
            ans.initPtGrid();
            ans.initForKnockout();
            ans.initCommission();
        }
        , error: function (e) {
            showAlertMessage("err", "Init Customer Error", 1);
        }
    });
};
//init PT Object Grid
ans.initPtGrid = function () {
    Fanex.PTEngine.CURRENT_CONTEXT = context;
    ptGrid = new Fanex.PTEngine.PTGrid(customer.ptSetting);
    var divPtGrid = document.getElementById('divPtGrid');
    ptGrid.Render(divPtGrid);

    bettingLimitGrid = new Fanex.PTEngine.BettingLimit(customer.bettingLimit);
    var bettingLimitContainer = document.getElementById('divBettingLimit');
    bettingLimitContainer.innerHTML = "";
    bettingLimitGrid.Render(bettingLimitContainer);
};
//render Commission combobox
ans.initCommission = function () {
    var commissionSetting = customer.commissionSettings[0];
    Fanex.PTEngine.PT_SetSelect(document.getElementById('listGroupA'), 0, commissionSetting.groupA, Fanex.PTEngine.COMMISSION_STEP, commissionSetting.groupA);
    Fanex.PTEngine.PT_SetSelect(document.getElementById('listGroupB'), 0, commissionSetting.groupB, Fanex.PTEngine.COMMISSION_STEP, commissionSetting.groupB);
    Fanex.PTEngine.PT_SetSelect(document.getElementById('listGroupC'), 0, commissionSetting.groupC, Fanex.PTEngine.COMMISSION_STEP, commissionSetting.groupC);
    Fanex.PTEngine.PT_SetSelect(document.getElementById('listGroupD'), 0, commissionSetting.groupD, Fanex.PTEngine.COMMISSION_STEP, commissionSetting.groupD);
    Fanex.PTEngine.PT_SetSelect(document.getElementById('listOther'), 0, commissionSetting.discountCS, Fanex.PTEngine.COMMISSION_STEP, commissionSetting.discountCS);
    Fanex.PTEngine.PT_SetSelect(document.getElementById('listGroup1x2'), 0, commissionSetting.discount1x2, Fanex.PTEngine.COMMISSION_STEP, commissionSetting.discount1x2);
    Fanex.PTEngine.PT_SetSelect(document.getElementById('listGameComm'), 0, commissionSetting.discountNumber, Fanex.PTEngine.COMMISSION_STEP, 0);
};
// check valid user input
ans.checkValidInput = function () {
    //    if (!ValidateUserName(customer.userName)) {
    //        showAlertMessage("err", "Invalid Username", 1);
    //        obj("txtUsername").focus();
    //        return false;
    //    }
    if (CheckConstrainForUsernameAndPassword() == false) {
        return false;
    }
    if ($('#listSite').val() == "Select") {
        AlertValidationMessage("err", alertSite, 1, 'listSite');
        return false;
    }
    else {
        customer.siteName = $('#listSite').val();
    }
    if ($('#listCurrency').val() == "Select") {
        AlertValidationMessage("err", alertCurrency, 1, 'listCurrency');
        return false;
    }
    else {
        customer.currencyId = $('#listCurrency').val();
    }
    if (isEmpty('txtCredit') || isString('txtCredit')) {
        AlertValidationMessage("err", alertIncorrectCredit, 1, 'txtCredit');
        return false;
    }
    else if (parseFloat($('#txtCredit').val(), true) > 99999999999999) {
        AlertValidationMessage("err", "Super Max Credit must be equal or less than 99,999,999,999,999", 1, 'txtCredit');
        return false;
    }
    else {
        customer.credit = parseFloat($('#txtCredit').val(), true);
    }
    //get commision
    var commissionSetting = customer.commissionSettings[0];
    commissionSetting.groupA = $('#listGroupA').val();
    commissionSetting.groupB = $('#listGroupB').val();
    commissionSetting.groupA = $('#listGroupA').val();
    commissionSetting.groupC = $('#listGroupC').val();
    commissionSetting.groupD = $('#listGroupD').val();
    commissionSetting.discount1x2 = $('#listGroup1x2').val();
    commissionSetting.discountCS = $('#listOther').val();
    commissionSetting.discountNumber = $('#listGameComm').val();
    //get pt setting
    customer.ptSetting = ptGrid.Save();

    return true;
};
// do add new super
ans.addNewSuper = function () {
    if (ans.checkValidInput()) {
        //get betting limit
        if (bettingLimitGrid.Validate()) {
            customer.bettingLimit = bettingLimitGrid.Save();
        }
        else {
            return;
        }

        try { parent.parent.BlockUI(); } catch (e) { }
        $.ajax({
            type: "POST",
            url: "DoAddNewSuper",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: '{"stringCustomer":' + JSON.stringify(JSON.stringify(customer)) + ', "currentContext":' + JSON.stringify(context) + '}',
            success: function (result) {
                $('#divShowInfo').show();
                if (typeof result != 'undefined' && result != null) {
                    if (result.ErrorCode > 0) {
                        showAlertMessage("err", result.ErrorMessage, 1);
                    }
                    else {
                        showAlertMessage("err", "Inserted Super Successfully!", 0);
                        //redirect to update page and clear timeout
                        var redirectString = result.ErrorMessage;
                        var timeOutId = setTimeout(function () {
                            parent.parent.$("#centerFr").attr("src", redirectString);
                            clearTimeout(timeOutId);
                        }, 2000);
                    }
                }
                else {
                    showAlertMessage("err", "Insert Super Unsuccessfully!", 1);
                }
                try { parent.parent.jQuery.unblockUI(); } catch (e) { }
            }
            , error: function (e) {
                $('#divShowInfo').show();
                if (e.status == '404' || e.statusText == 'Not Found') {
                    if (isInsterTwice == false) {
                        ans.addNewSuper();
                        isInsterTwice = true;
                    }
                    else {
                        showAlertMessage("err", "Request Error", 1);
                    }
                }
                else {
                    showAlertMessage("err", "Request Error", 1);
                }
                try { parent.parent.jQuery.unblockUI(); } catch (e) { }
            }
        });
    }
    else {
        $('#divShowInfo').show();
    }
};
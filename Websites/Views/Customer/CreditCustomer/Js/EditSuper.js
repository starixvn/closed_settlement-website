/// <reference path="../../../../js/jQuery/jquery.js" />
/// <reference path="../../../../js/jQuery/json2.js" />
/// <reference path="../../../../js/PT/CommonPT.js" />
//load function
$(function () {
    eds.init();
});

var customer;
var context;
var ptGrid = null;
var bettingLimitGrid = null;
var isInsterTwice = false;

var eds = {
    ptGrid: null
};
//init
eds.init = function () {
    eds.initContext();
};
//biding for knockout
eds.initForKnockout = function () {
    var CustomerModel = function (customer) {
        this.saveCustomer = function () {
            //console.log(JSON.stringify(ko.toJS(customer), null, '\t'));
        };
    };
    ko.applyBindings(new CustomerModel(customer));
};
//get context of form
eds.initContext = function () {
    $.ajax({
        type: "POST",
        url: "GetContextObject",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        data: '{"superId":' + JSON.stringify(superId) + '}',
        success: function (stringContext) {
            context = JSON.parse(stringContext);
            eds.initCustomer();
        }
        , error: function (e) {
            showAlertMessage("err", "Init Context Error", 1);
        }
    });
};
//get default super value
eds.initCustomer = function () {
    $.ajax({
        type: "POST",
        url: "GetCustomerObject",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        data: '{"requestContext":' + JSON.stringify(context) + '}',
        success: function (stringCustomer) {
            customer = JSON.parse(stringCustomer);
            eds.initPtGrid();
            eds.initForKnockout();
            eds.initCommission();
            //set credit min
            $('#txtCreditMin').text(''.FormatNumber(customer.minCredit));
            $('#txtCredit').val(''.FormatNumber(customer.credit));
        }
        , error: function (e) {
            showAlertMessage("err", "Init Customer Error", 1);
        }
    });
};
//init PT Object Grid
eds.initPtGrid = function () {
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
eds.initCommission = function () {
    var commissionSetting = customer.commissionSettings[0];
    Fanex.PTEngine.PT_SetSelect(document.getElementById('listGroupA'), 0, Math.max(commissionSetting.groupAMax, commissionSetting.groupA), Fanex.PTEngine.COMMISSION_STEP, commissionSetting.groupA);
    Fanex.PTEngine.PT_SetSelect(document.getElementById('listGroupB'), 0, Math.max(commissionSetting.groupBMax, commissionSetting.groupB), Fanex.PTEngine.COMMISSION_STEP, commissionSetting.groupB);
    Fanex.PTEngine.PT_SetSelect(document.getElementById('listGroupC'), 0, Math.max(commissionSetting.groupCMax, commissionSetting.groupC), Fanex.PTEngine.COMMISSION_STEP, commissionSetting.groupC);
    Fanex.PTEngine.PT_SetSelect(document.getElementById('listGroupD'), 0, Math.max(commissionSetting.groupDMax, commissionSetting.groupD), Fanex.PTEngine.COMMISSION_STEP, commissionSetting.groupD);
    Fanex.PTEngine.PT_SetSelect(document.getElementById('listOther'), 0, Math.max(commissionSetting.discountCSMax, commissionSetting.discountCS), Fanex.PTEngine.COMMISSION_STEP, commissionSetting.discountCS);
    Fanex.PTEngine.PT_SetSelect(document.getElementById('listGroup1x2'), 0, Math.max(commissionSetting.discount1x2Max, commissionSetting.discount1x2), Fanex.PTEngine.COMMISSION_STEP, commissionSetting.discount1x2);
    Fanex.PTEngine.PT_SetSelect(document.getElementById('listGameComm'), 0, Math.max(commissionSetting.discountNumberMax, commissionSetting.discountNumber), Fanex.PTEngine.COMMISSION_STEP, commissionSetting.discountNumber);
};
// check valid user input
eds.checkValidInput = function () {
    if (isEmpty('txtCredit') || isString('txtCredit')) {
        AlertValidationMessage("err", alertIncorrectCredit, 1, 'txtCredit');
        return false;
    }
    else if (parseFloat($('#txtCredit').val(), true) < customer.minCredit) {
        AlertValidationMessage("err", "Super Max Credit must be equal or greater than " + $('#txtCreditMin').text(), 1, 'txtCredit');
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
eds.editSuper = function () {
    if (eds.checkValidInput()) {
        //get betting limit
        if (bettingLimitGrid.Validate()) {
            customer.bettingLimit = bettingLimitGrid.Save();
        }
        else {
            return;
        }

        try { parent.parent.BlockUI(); } catch (e) { }
        try { parent.BlockUI(); } catch (e) { }
        $.ajax({
            type: "POST",
            url: "DoEditSuper",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: '{"stringCustomer":' + JSON.stringify(JSON.stringify(customer)) + ', "currentContext":' + JSON.stringify(context) + '}',
            success: function (result) {
                $('#divShowInfo').show();
                if (typeof result != 'undefined' && result != null) {
                    if (result.ErrorCode > 0) {
                        alert_error(result.ErrorMessage, 'err');
                        parent.scroll(0, 0);
                        window.scroll(0, 0);
                    }
                    else {
                        showAlertMessage("err", "Updated Super Successfully!", 0);
                        //redirect to current page
                        setTimeout(function () { location.href = location.href; }, 3000);
                    }
                }
                else {
                    showAlertMessage("err", "Update Super Unsuccessfully!", 1);
                }
                try { parent.parent.jQuery.unblockUI(); } catch (e) { }
                try { parent.jQuery.unblockUI(); } catch (e) { }
            }
            , error: function (e) {
                $('#divShowInfo').show();
                if (e.status == '404' || e.statusText == 'Not Found') {
                    if (isInsterTwice == false) {
                        eds.editSuper();
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
                try { parent.jQuery.unblockUI(); } catch (e) { }
            }
        });
    }
    else {
        $('#divShowInfo').show();
    }
};
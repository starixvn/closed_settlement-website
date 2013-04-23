/// <reference path="../../../../js/jQuery/jquery.js" />
/// <reference path="../../../../js/jQuery/json2.js" />
/// <reference path="../../../../js/PT/CommonPT.js" />
/// <reference path="../../../../js/common.js" />

//load function
$(function () {
    edm.init();
});

var customer;
var context;
var ptGrid = null;
var bettingLimitGrid = null;
var myCustomizeValue = null;
var isInsterTwice = false;

var edm = {
    ptGrid: null
};
//init
edm.init = function () {
    edm.initContext();
};
//biding for knockout
edm.initForKnockout = function () {
    var CustomerModel = function (customer) {
        this.saveCustomer = function () {
            //console.log(JSON.stringify(ko.toJS(customer), null, '\t'));
        };
    };
    ko.applyBindings(new CustomerModel(customer));
};
//get context of form
edm.initContext = function () {
    $.ajax({
        type: "POST",
        url: "GetContextObject",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        data: '{"agentId":' + JSON.stringify(agentId) + ',"memberId":' +  JSON.stringify(memberId) + '}',
        success: function (stringContext) {
            context = JSON.parse(stringContext);
            edm.initCustomer();
        }
        , error: function (e) {
            showAlertMessage("err", "Init Context Error", 1);
        }
    });
};
//get default super value
edm.initCustomer = function () {
    $.ajax({
        type: "POST",
        url: "GetCustomerObject",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        data: '{"requestContext":' + JSON.stringify(context) + '}',
        success: function (stringCustomer) {
            customer = JSON.parse(stringCustomer);
            edm.initPtGrid();
            edm.initForKnockout();
            edm.initCommission();
            $('#txtCreditMax').text(''.FormatNumber(customer.maxCredit));
            $('#lblGroup').text(customer.group);
        }
        , error: function (e) {
            showAlertMessage("err", "Init Customer Error", 1);
        }
    });
};
//init PT Object Grid
edm.initPtGrid = function () {
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
edm.initCommission = function () {

    var commissionSetting = customer.commissionSettings[0];
    $('#txtHDPOUOE').val(commissionSetting.discount);
    Fanex.PTEngine.PT_SetSelect(document.getElementById('listHDPOUOE'), 0, commissionSetting.discount, Fanex.PTEngine.COMMISSION_STEP, commissionSetting.playerDiscount);
    Fanex.PTEngine.PT_SetSelect(document.getElementById('listOtherBettypes'), 0, commissionSetting.discountCS, Fanex.PTEngine.COMMISSION_STEP, commissionSetting.playerDiscountCs);
    Fanex.PTEngine.PT_SetSelect(document.getElementById('list1X2'), 0, commissionSetting.discount1x2, Fanex.PTEngine.COMMISSION_STEP, commissionSetting.playerDiscount1x2);
    Fanex.PTEngine.PT_SetSelect(document.getElementById('listGameComm'), 0, commissionSetting.discountNumber, Fanex.PTEngine.COMMISSION_STEP, commissionSetting.playerDiscountNumber);
};
//render Transfer Option
// check valid user input
edm.checkValidInput = function () {
    if (isEmpty('txtCredit') || isString('txtCredit')) {
        AlertValidationMessage("err", "Invalid Member Credit", 1, 'txtCredit');
        return false;
    } else if (parseFloat($('#txtCredit').val(), true) > parseFloat($('#txtCreditMax').text(), true)) {
        AlertValidationMessage("err", "Member Max Credit must be between 0 and " + $('#txtCreditMax').text(), 1, 'txtCredit');
        return false;
    }
    else {
        customer.credit = $('#txtCredit').val();
    }
    //get commision
    var commissionSetting = customer.commissionSettings[0];
    commissionSetting.playerDiscount = $('#listHDPOUOE').val();
    commissionSetting.playerDiscount1x2 = $('#list1X2').val();
    commissionSetting.playerDiscountCs = $('#listOtherBettypes').val();
    commissionSetting.playerDiscountNumber = $('#listGameComm').val();
    // get user group
    //customer.group = $('#ddlUGruop').val();
    //get pt setting
    customer.ptSetting = ptGrid.Save();
    return true;
};
// do add new agent
edm.editMember = function () {
    if (edm.checkValidInput()) {
        //get betting limit
        if (bettingLimitGrid.Validate()) {
            customer.bettingLimit = bettingLimitGrid.Save();
        }
        else {
            return;
        }
        try { parent.parent.BlockUI(); } catch (e) {  }
        try { parent.BlockUI(); } catch (e) { }
        $.ajax({
            type: "POST",
            url: "DoEditMember",
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
                        showAlertMessage("err", "Edited Member Successfully!", 0);
                        //redirect to current page
                        setTimeout(function () { location.href = location.href; }, 3000);
                    }
                }
                else {
                    showAlertMessage("err", "Edit Member Unsuccessfully!", 1);
                }
                try { parent.parent.jQuery.unblockUI(); } catch (e) { }
                try { parent.jQuery.unblockUI(); } catch (e) { }
            }
            , error: function (e) {
                $('#divShowInfo').show();
                if (e.status == '404' || e.statusText == 'Not Found') {
                    if (isInsterTwice == false) {
                        edm.editMember();
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
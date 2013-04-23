/// <reference path="../../../../js/jQuery/jquery.js" />
/// <reference path="../../../../js/jQuery/json2.js" />
/// <reference path="../../../../js/PT/CommonPT.js" />
/// <reference path="../../../../js/common.js" />

//load function
$(function () {
    anm.init();
    //init transfer option
    $('#rdDaily').trigger('click');
    selectTranferTime();
});

var customer;
var context;
var ptGrid = null;
var bettingLimitGrid = null;
var myCustomizeValue = null;
var isShowTransferOption = true;
var isInsterTwice = false;

var anm = {
    ptGrid: null
};
//init
anm.init = function () {
    anm.initContext();
};
//biding for knockout
anm.initForKnockout = function () {
    var CustomerModel = function (customer) {
        this.saveCustomer = function () {
            //console.log(JSON.stringify(ko.toJS(customer), null, '\t'));
        };
    };
    ko.applyBindings(new CustomerModel(customer));
};
//get context of form
anm.initContext = function () {
    $.ajax({
        type: "POST",
        url: "GetContextObject",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        data: '{"superId":' + JSON.stringify(superId) + '}',
        success: function (stringContext) {
            context = JSON.parse(stringContext);
            anm.initCustomer();
        }
        , error: function (e) {
            showAlertMessage("err", "Init Context Error", 1);
        }
    });
};
//get default super value
anm.initCustomer = function () {
    $.ajax({
        type: "POST",
        url: "GetCustomerObject",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        data: '{"requestContext":' + JSON.stringify(context) + '}',
        success: function (stringCustomer) {
            customer = JSON.parse(stringCustomer);
            anm.initPtGrid();
            anm.initForKnockout();
            anm.initCommission();
            // Default is allow auto pt
            $('#chbAllowAutoPT').prop('checked', true);
            //format max credit
            $('#txtCreditMax').text(''.FormatNumber(customer.maxCredit));
            //show transfer option or not
            if (CheckShowTransferOption(customer.transferOption) == false) {
                $('#tbTransferOption').hide();
                isShowTransferOption = false;
            }
        }
        , error: function (e) {
            showAlertMessage("err", "Init Customer Error", 1);
        }
    });
};
//init PT Object Grid
anm.initPtGrid = function () {
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
anm.initCommission = function () {
    var commissionSetting = customer.commissionSettings[0];
    Fanex.PTEngine.PT_SetSelect(document.getElementById('listGroupA'), 0, Math.max(commissionSetting.groupAMax, commissionSetting.groupA), Fanex.PTEngine.COMMISSION_STEP, commissionSetting.groupA);
    Fanex.PTEngine.PT_SetSelect(document.getElementById('listGroupB'), 0, Math.max(commissionSetting.groupBMax, commissionSetting.groupB), Fanex.PTEngine.COMMISSION_STEP, commissionSetting.groupB);
    Fanex.PTEngine.PT_SetSelect(document.getElementById('listGroupC'), 0, Math.max(commissionSetting.groupCMax, commissionSetting.groupC), Fanex.PTEngine.COMMISSION_STEP, commissionSetting.groupC);
    Fanex.PTEngine.PT_SetSelect(document.getElementById('listGroupD'), 0, Math.max(commissionSetting.groupDMax, commissionSetting.groupD), Fanex.PTEngine.COMMISSION_STEP, commissionSetting.groupD);
    Fanex.PTEngine.PT_SetSelect(document.getElementById('listOther'), 0, Math.max(commissionSetting.discountCSMax, commissionSetting.discountCS), Fanex.PTEngine.COMMISSION_STEP, commissionSetting.discountCS);
    Fanex.PTEngine.PT_SetSelect(document.getElementById('listGroup1x2'), 0, Math.max(commissionSetting.discount1x2Max, commissionSetting.discount1x2), Fanex.PTEngine.COMMISSION_STEP, commissionSetting.discount1x2);
    Fanex.PTEngine.PT_SetSelect(document.getElementById('listGameComm'), 0, Math.max(commissionSetting.discountNumberMax, commissionSetting.discountNumber), Fanex.PTEngine.COMMISSION_STEP, commissionSetting.discountNumber);
};
//render Transfer Option
// check valid user input
anm.checkValidInput = function () {
    //    if (!ValidateUserName(customer.userName)) {
    //        showAlertMessage("err", "Invalid Username", 1);
    //        obj("txtUsername").focus();
    //        return false;
    //    }
    if (CheckConstrainForUsernameAndPassword() == false) {
        return false;
    }
    if (isEmpty('txtCredit') || isString('txtCredit')) {
        AlertValidationMessage("err", alertIncorrectCredit, 1, 'txtCredit');
        return false;
    }
    else if (parseFloat($('#txtCredit').val(), true) > customer.maxCredit) {
        AlertValidationMessage("err", "Master Max Credit must be between 0 and " + $('#txtCreditMax').text(), 1, 'txtCredit');
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

    // Save Allow Auto Pt
    customer.statusSetting.disableAutoMPT = $('#chbAllowAutoPT').prop('checked');

    //get pt setting
    customer.ptSetting = ptGrid.Save();
    //Set value to transfer option
    if (CheckTime()) {
        if (isShowTransferOption == true) {
            customer.transferOption = GetTransferOption();
        }
    }
    else {
        return false;
    }
    return true;
};
// do add new super
anm.addNewMaster = function () {
    if (anm.checkValidInput()) {
        //get betting limit
        if (bettingLimitGrid.Validate()) {
            customer.bettingLimit = bettingLimitGrid.Save();
        }
        else {
            return;
        }
        try { parent.parent.BlockUI(); } catch (e) { parent.BlockUI(); }

        $.ajax({
            type: "POST",
            url: "DoAddNewMaster",
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
                        showAlertMessage("err", "Inserted Master Successfully!", 0);
                        //redirect to update page and clear timeout
                        var redirectString = result.ErrorMessage;
                        var timeOutId = setTimeout(function () {
                            try {
                                parent.parent.$("#centerFr").attr("src", redirectString);
                            }
                            catch (e) {
                                parent.$("#centerFr").attr("src", redirectString);
                            }
                            clearTimeout(timeOutId);
                        }, 2000);
                    }
                }
                else {
                    showAlertMessage("err", "Insert Master Unsuccessfully!", 1);
                }
                try { parent.parent.jQuery.unblockUI(); } catch (e) { }
                try { parent.jQuery.unblockUI(); } catch (e) { }
            }
            , error: function (e) {
                $('#divShowInfo').show();
                if (e.status == '404' || e.statusText == 'Not Found') {
                    if (isInsterTwice == false) {
                        anm.addNewMaster();
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
/// <reference path="../../../../js/jQuery/jquery.js" />
/// <reference path="../../../../js/jQuery/json2.js" />
/// <reference path="../../../../js/PT/CommonPT.js" />
/// <reference path="../../../../js/common.js" />

//load function
$(function () {
    anmb.init();
});

var customer;
var context;
var ptGrid = null;
var bettingLimitGrid = null;
var myCustomizeValue = null;
var isInsterTwice = false;

var anmb = {
    ptGrid: null
};
//init
anmb.init = function () {
    anmb.initContext();
};
//biding for knockout
anmb.initForKnockout = function () {
    var CustomerModel = function (customer) {
        this.saveCustomer = function () {
            //console.log(JSON.stringify(ko.toJS(customer), null, '\t'));
        };
    };
    ko.applyBindings(new CustomerModel(customer));
};
//get context of form
anmb.initContext = function () {
    $.ajax({
        type: "POST",
        url: "GetContextObject",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        data: '{"agentId":' + JSON.stringify(agentId) + '}',
        success: function (stringContext) {
            context = JSON.parse(stringContext);
            anmb.initCustomer();
        }
        , error: function (e) {
            showAlertMessage("err", "Init Context Error", 1);
        }
    });
};
//get default super value
anmb.initCustomer = function () {
    $.ajax({
        type: "POST",
        url: "GetCustomerObject",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        data: '{"requestContext":' + JSON.stringify(context) + '}',
        success: function (stringCustomer) {
            customer = JSON.parse(stringCustomer);
            anmb.initPtGrid();
            anmb.initForKnockout();
            anmb.initCommission();
            $('#txtCreditMax').text(''.FormatNumber(customer.maxCredit));
        }
        , error: function (e) {
            showAlertMessage("err", "Init Customer Error", 1);
        }
    });
};
//init PT Object Grid
anmb.initPtGrid = function () {
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
anmb.initCommission = function () {
    var commissionSetting = customer.commissionSettings[0];
    Fanex.PTEngine.PT_SetSelect(document.getElementById('listHDPOUOE'), 0, commissionSetting.groupA, Fanex.PTEngine.COMMISSION_STEP, commissionSetting.groupA);
    Fanex.PTEngine.PT_SetSelect(document.getElementById('listOtherBettypes'), 0, commissionSetting.discountCS, Fanex.PTEngine.COMMISSION_STEP, commissionSetting.discountCS);
    Fanex.PTEngine.PT_SetSelect(document.getElementById('list1X2'), 0, commissionSetting.discount1x2, Fanex.PTEngine.COMMISSION_STEP, commissionSetting.discount1x2);
    Fanex.PTEngine.PT_SetSelect(document.getElementById('listGameComm'), 0, commissionSetting.discountNumber, Fanex.PTEngine.COMMISSION_STEP, commissionSetting.discountNumber);
    $('#ddlUGruop').change(function () {
        var $this = $(this);
        var lastCharacter = $this.val()[$this.val().length - 1];
        if (lastCharacter == "a") {
            Fanex.PTEngine.PT_SetSelect(document.getElementById('listHDPOUOE'), 0, commissionSetting.groupA, Fanex.PTEngine.COMMISSION_STEP, commissionSetting.groupA);
            $('#txtHDPOUOE').val(commissionSetting.groupA);
        }
        else if (lastCharacter == "b") {
            Fanex.PTEngine.PT_SetSelect(document.getElementById('listHDPOUOE'), 0, commissionSetting.groupB, Fanex.PTEngine.COMMISSION_STEP, commissionSetting.groupB);
            $('#txtHDPOUOE').val(commissionSetting.groupB);
        }
        else if (lastCharacter == "c") {
            Fanex.PTEngine.PT_SetSelect(document.getElementById('listHDPOUOE'), 0, commissionSetting.groupC, Fanex.PTEngine.COMMISSION_STEP, commissionSetting.groupC);
            $('#txtHDPOUOE').val(commissionSetting.groupC);
        }
        else if (lastCharacter == "d") {
            Fanex.PTEngine.PT_SetSelect(document.getElementById('listHDPOUOE'), 0, commissionSetting.groupD, Fanex.PTEngine.COMMISSION_STEP, commissionSetting.groupD);
            $('#txtHDPOUOE').val(commissionSetting.groupD);
        }
    });
};
//render Transfer Option
// check valid user input
anmb.checkValidInput = function () {
    //    if (!ValidateUserName(customer.userName)) {
    //        showAlertMessage("err", "Invalid Username", 1);
    //        obj("txtUsername").focus();
    //        return false;
    //    }

    if (CheckConstrainForUsernameAndPassword() == false) {
        return false;
    }

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
    customer.group = $('#ddlUGruop').val();
    //get pt setting
    customer.ptSetting = ptGrid.Save();
    return true;
};
// do add new agent
anmb.addNewMember = function () {
    if (anmb.checkValidInput()) {
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
            url: "DoAddNewMember",
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
                        showAlertMessage("err", "Inserted Member Successfully!", 0);
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
                    showAlertMessage("err", "Insert Member Unsuccessfully!", 1);
                }
                try { parent.parent.jQuery.unblockUI(); } catch (e) { }
                try { parent.jQuery.unblockUI(); } catch (e) { }
            }
            , error: function (e) {
                $('#divShowInfo').show();
                if (e.status == '404' || e.statusText == 'Not Found') {
                    if (isInsterTwice == false) {
                        anmb.addNewMember();
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
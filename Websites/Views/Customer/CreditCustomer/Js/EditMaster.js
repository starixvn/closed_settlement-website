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
var isShowTransferOption = true;
var isInsterTwice = false;
var isUpdateAllMembers = false;
var newPositionTakingValue = 0;

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
        data: '{"superId":' + JSON.stringify(superId) + ', "masterId": ' + JSON.stringify(masterId) + '}',
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
            storeOldPTSetting = customer.ptSetting.clone();
            //format max credit
            $('#txtCreditMax').text(''.FormatNumber(customer.maxCredit));
            $('#txtCreditMin').text(''.FormatNumber(customer.minCredit));
            $('#txtCredit').val(''.FormatNumber(customer.credit));
        }
        , error: function (e) {
            showAlertMessage("err", "Init Customer Error", 1);
        }
    });
};
//init PT Object Grid
edm.initPtGrid = function () {
    Fanex.PTEngine.CURRENT_CONTEXT = context;
    Fanex.PTEngine.CURRENT_CONTEXT.ia3 = !customer.statusSetting.disableAutoMPT;
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
edm.checkValidInput = function () {
    if (isEmpty('txtCredit') || isString('txtCredit')) {
        AlertValidationMessage("err", alertIncorrectCredit, 1, 'txtCredit');
        return false;
    }
    else if (parseFloat($('#txtCredit').val(), true) > customer.maxCredit) {
        AlertValidationMessage("err", "Master Max Credit must be between 0 and " + $('#txtCreditMax').text(), 1, 'txtCredit');
        return false;
    }
    else if (parseFloat($('#txtCredit').val(), true) < customer.minCredit) {
        AlertValidationMessage("err", "Master Max Credit must be equal or greater than " + $('#txtCreditMin').text(), 1, 'txtCredit');
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

    return true;
};
// handel change auto pt
var isCheckChangeAuto = false;
var isShowPopUpSetPTMember = false;
var storeOldPTSetting = null;

// We need check have any PT value change from auto to none auto. And Set new value to Member
edm.CheckChangeAuto = function (ptSettingOld, ptSettingNew) {
    var sportTypes = ptSettingOld.sportTypes;
    for (var sportTypeIndex = 0; sportTypeIndex < sportTypes.length; sportTypeIndex++) {
        var sportTypesItem = sportTypes[sportTypeIndex]
        var betTypeGroupSets = sportTypesItem.betTypeGroupSets;

        for (var groupSetIndex = 0; groupSetIndex < betTypeGroupSets.length; groupSetIndex++) {
            var betTypeGroupSetsItem = betTypeGroupSets[groupSetIndex];
            var betTypeGroups = betTypeGroupSetsItem.betTypeGroups;

            for (var groupIndex = 0; groupIndex < betTypeGroups.length; groupIndex++) {
                var betTypeGroupItem = betTypeGroups[groupIndex];
                var betTypes = betTypeGroupItem.betTypes;

                for (var betTypeIndex = 0; betTypeIndex < betTypes.length; betTypeIndex++) {
                    betTypeItem = betTypes[betTypeIndex];
                    var isAutoOld = betTypeItem.pt.is3;
                    var isAutoNew = ptSettingNew.sportTypes[sportTypeIndex].betTypeGroupSets[groupSetIndex].betTypeGroups[groupIndex].betTypes[betTypeIndex].pt.is3;

                    if (isAutoOld && !isAutoNew) {
                        return true;
                    }
                }
            }
        }
    }
    return false;
};

// Get selected and m4 value of Soccer HdpD Master Preset
edm.MemberMaximumPT = function () {
    var ptSetting = customer.ptSetting;
    var s4 = ptSetting.sportTypes[0].betTypeGroupSets[0].betTypeGroups[0].betTypes[0].pt.s4;
    var m3 = ptSetting.sportTypes[0].betTypeGroupSets[0].betTypeGroups[0].betTypes[0].pt.m3;
    return Fanex.Round(s4 - m3);
};

// Confirm new value for all member before transfer to server
edm.SetNewValueToMember = function (isUpdateAllMember) {
    isShowPopUpSetPTMember = true;
    newPositionTakingValue = $('#selectedMemberPositionTaking').val();
    isUpdateAllMembers = isUpdateAllMember;
    //        var customerModel = new CustomerModel(customer);
    //        customerModel.saveCustomer();
    Popups.Close();
    isShowPopUpSetPTMember = false;
    edm.SendAjaxEditMaster();
};

function ConfirmYes() {
    edm.SetNewValueToMember(true);
}

function ConfirmNo() {
    edm.SetNewValueToMember(false);
}

edm.handleAutoChangeForMember = function () {
    if (context.targetCustId > 0) {
        if (!isShowPopUpSetPTMember) {
            //get pt setting
            customer.ptSetting = ptGrid.Save();
            isCheckChangeAuto = edm.CheckChangeAuto(storeOldPTSetting, customer.ptSetting);
        }
        if (isCheckChangeAuto && !isShowPopUpSetPTMember) {
            Popups.Confirm(edm.MemberMaximumPT(), Language.CONFIRM_MASTER_NOT_TO_TAKE_AUTO_POSITION_TAKING);
            return;
        }
        else if (isCheckChangeAuto == false) {
            edm.SendAjaxEditMaster();
        }
        else if (isCheckChangeAuto && isShowPopUpSetPTMember) {
            Popups.Close();
        }
    }
};
edm.SendAjaxEditMaster = function () {
    try { parent.parent.BlockUI(); } catch (e) { }
    try { parent.BlockUI(); } catch (e) { }
    $.ajax({
        type: "POST",
        url: "DoEditMaster",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        data: '{"stringCustomer":' + JSON.stringify(JSON.stringify(customer)) + ', "currentContext":' + JSON.stringify(context) + ', "isUpdateAllMembers":' + isUpdateAllMembers + ', "newPositionTakingValue":' + newPositionTakingValue + '}',
        success: function (result) {
            $('#divShowInfo').show();
            try { parent.jQuery.unblockUI(); } catch (e) { }
            if (typeof result != 'undefined' && result != null) {
                if (result.ErrorCode > 0) {
                    alert_error(result.ErrorMessage, 'err');
                    parent.scroll(0, 0);
                    window.scroll(0, 0);
                }
                else {
                    showAlertMessage("err", "Updated Master Successfully!", 0);
                    //redirect to current page
                    setTimeout(function () { location.href = location.href; }, 3000);
                }
            }
            else {
                showAlertMessage("err", "Update Master Unsuccessfully!", 1);
            }
            //try { parent.parent.jQuery.unblockUI(); } catch (e) { }
        }
            , error: function (e) {
                $('#divShowInfo').show();
                if (e.status == '404' || e.statusText == 'Not Found') {
                    if (isInsterTwice == false) {
                        edm.SendAjaxEditMaster();
                        isInsterTwice = true;
                    }
                    else {
                        showAlertMessage("err", "Request Error", 1);
                    }
                }
                else {
                    showAlertMessage("err", "Request Error", 1);
                }
                //try { parent.parent.jQuery.unblockUI(); } catch (e) { }
                try { parent.jQuery.unblockUI(); } catch (e) { }
            }
    });
};
// do add new super
edm.editMaster = function () {
    if (edm.checkValidInput()) {
        //get betting limit
        if (bettingLimitGrid.Validate()) {
            customer.bettingLimit = bettingLimitGrid.Save();
        }
        else {
            return;
        }
        edm.handleAutoChangeForMember();
    }
    else {
        $('#divShowInfo').show();
    }
};
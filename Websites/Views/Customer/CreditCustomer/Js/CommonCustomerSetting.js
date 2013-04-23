function GetTransferOption() {
    var str = "00000000";
    var transferConditionId = document.getElementsByName("transferCondition");

    if (transferConditionId != 'undefined' && transferConditionId.length > 0) {
        if (transferConditionId[1].checked) {
            str = "";
            var arrWeekly = new Array("chkSun", "chkMon", "chkTue", "chkWed", "chkThurs", "chkFri", "chkSat");
            for (var i = 0; i < arrWeekly.length; i++) {
                if (document.getElementById(arrWeekly[i]).checked) {
                    str += "1";
                }
                else {
                    str += "0";
                }
            }
            str += "1";
        }
    }
    return str;
};
//this function is used to check case editing transfer time
//When Weekly radio is selected and don't have any checkbox of days is checked
function CheckTimeForTransferOption() {
    var chkCount = 0;
    var isSelect = false;
    if (obj("rdWeekly").checked) {
        var elements = obj("alldays").getElementsByTagName("input");
        for (var i = 1; i < elements.length; i++) {
            if (elements[i].checked) {
                isSelect = true;
                chkCount++;
            }
        }
        if (!isSelect) {
            $('#divShowInfo').show();
            showAlertMessage("err", lblChooseOneDay, 1);
            scroll(0, 0);
            return false;
        }
    }
    return true;
}

//this function is used to check case editing transfer time
//When Weekly radio is selected and all checkbox of 7 days can not be checked
function ChooseDateTransfer() {
    var chkCount = 0;
    if (obj("rdWeekly").checked) {
        var elements = obj("alldays").getElementsByTagName("input");
        if (obj("err") != null && obj("err").innerHTML != "") {
            obj("err").innerHTML = "";
        }
        for (var i = 1; i < elements.length; i++) {
            if (elements[i].checked) {
                isSelect = true;
                chkCount++;
                if (chkCount == 7) {
                    $('#divShowInfo').show();
                    showAlertMessage("err", lblChooseDaily, 2);
                    return false;
                }
            }
        }
    }
    return true;
};

function ValidateUserName(userName) {
    var validateExpr = /^[A-Za-z0-9_-]{0,40}$/;
    return validateExpr.test(userName);
};

function CheckShowTransferOption(transferOption) {
    if (transferOption == "00000000") {
        return true;
    }
    else {
        return false;
    }
};

// Success=0, Error=1, Warning=2
function AlertValidationMessage(containerid, msg, type, controlValidatedId) {
    switch (type) {
        case 0:
            document.getElementById(containerid).innerHTML = "<div class='alertboxscc'><div>" + msg + "</div></div>";
            break;
        case 1:
            document.getElementById(containerid).innerHTML = "<div class='alertboxerr'><div>" + msg + "</div></div>";
            break;
        case 2:
            document.getElementById(containerid).innerHTML = "<div class='alertboxwrn'><div>" + msg + "</div></div>"; ;
            break;
        default:
            break
    }
    Validators.animate(document.getElementById(controlValidatedId));
    document.getElementById(controlValidatedId).focus();
    window.scroll(0, 0);
    parent.scroll(0, 0);
}

// Check Constrain for user name and password
function CheckConstrainForUsernameAndPassword() {
    if (isEmpty("txtUsername")) {
        AlertValidationMessage("err", alertUserName, 1, 'txtUsername');
        return false;
    }
    if ($('#txtUsername').val().length > 30) {
        AlertValidationMessage("err", "Sorry, Username cannot exceed 30 characters.", 1, 'txtUsername');
        return false;
    }
    if (isEmpty("txtPwd")) {
        AlertValidationMessage("err", "Sorry, password cannot be empty!!!", 1, 'txtPwd');
        return false;
    }
    if ($('#txtPwd').val().length > 30) {
        $('#txtPwd').val("");
        AlertValidationMessage("err", "Sorry, Password cannot exceed 30 characters.", 1, 'txtPwd');
        return false;
    }
    // Save Customer Info
    customer.firstName = $('#txtFirstname').val();
    customer.lastName = $('#txtLastname').val();
    customer.phone = $('#txtPhone').val();
    customer.mobilePhone = $('#txtMobile').val();
    customer.fax = $('#txtFax').val();
    return true;
}
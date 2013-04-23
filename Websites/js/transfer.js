$(document).ready(function () {
    var transfer = {
        ui: {}
        , utils: {}
        , business: {}
    };

    transfer.init = function () {
        this.utils.init();
        this.ui.init();
    };

    transfer.business.showExRateFromUser = function () {
        transfer.ui.hideMessage();
        if ($("#tbxFromUser").val() == '') {
            $("#spanCurrencyFromUser").text('');
            $("#spanExRateFromUser").text('');
            $("#trSystemTransExRate").hide();
            transfer.business.rowFromUser = null;
            return;
        }
        var param = { "username": $("#tbxFromUser").val() };
        $("#divAjaxLoaderImageFromUser").show();
        $.ajax({
            type: "POST",
            url: "Services/WCF/Customer/ServiceCustomerInfo.svc/SelectCustomerExRate",
            data: $.toJSON(param),
            contentType: "application/json; charset=utf-8",
            success: function (rs) {
                $("#divAjaxLoaderImageFromUser").hide();
                if (rs == null || typeof rs == "undefined" || typeof rs.username == "undefined") {
                    $("#spanCurrencyFromUser").text('');
                    $("#spanExRateFromUser").text('');
                    $("#trSystemTransExRate").hide();
                    transfer.ui.showError("User does not exist");
                    transfer.business.rowFromUser = null;
                    return;
                }
                transfer.ui.hideMessage();
                transfer.business.rowFromUser = rs;
                $("#spanCurrencyFromUser").text(rs.currency);
                $("#spanExRateFromUser").text(rs.actualrate);
                transfer.business.showSystemExRate();
            }
            , error: function (e) {
                $("#divAjaxLoaderImageFromUser").hide();
            }
        });
    };

    transfer.business.showExRateToUser = function () {
        transfer.ui.hideMessage();
        if ($("#tbxToUser").val() == '') {
            $("#spanCurrencyToUser").text('');
            $("#spanExRateToUser").text('');
            $("#trSystemTransExRate").hide();
            transfer.business.rowToUser = null;
            return;
        }
        $("#divAjaxLoaderImageToUser").show();
        var param = { "username": $("#tbxToUser").val() };
        $.ajax({
            type: "POST",
            url: "Services/WCF/Customer/ServiceCustomerInfo.svc/SelectCustomerExRate",
            data: $.toJSON(param),
            contentType: "application/json; charset=utf-8",
            success: function (rs) {
                $("#divAjaxLoaderImageToUser").hide();
                if (rs == null || typeof rs == "undefined" || typeof rs.username == "undefined") {
                    $("#spanCurrencyToUser").text('');
                    $("#spanExRateToUser").text('');
                    $("#trSystemTransExRate").hide();
                    transfer.ui.showError("User does not exist");
                    transfer.business.rowToUser = null;
                    return;
                }
                transfer.ui.hideMessage();
                transfer.business.rowToUser = rs;
                $("#spanCurrencyToUser").text(rs.currency);
                $("#spanExRateToUser").text(rs.actualrate);
                transfer.business.showSystemExRate();
            }
            , error: function (e) {
                $("#divAjaxLoaderImageToUser").hide();
            }
        });
    };

    transfer.business.showSystemExRate = function () {
        if (this.rowFromUser != null && this.rowToUser != null) {
            $("#spanPerCurrencySystem").text(this.rowFromUser.currency + "/" + this.rowToUser.currency);
            if (this.rowToUser.actualrate != 0) {
                var systemExRate = this.rowFromUser.actualrate / this.rowToUser.actualrate;
                systemExRate = Math.round(systemExRate * 10000) / 10000;
                $("#spanExRateSystem").text(systemExRate);
            }
            $("#spanPerCurrencyTrans").text(this.rowFromUser.currency + "/" + this.rowToUser.currency);
            $("#trSystemTransExRate").show();
        }
    };

    transfer.business.checkTransferAmount = function () {
        if ($("#tbxFromAmount").val() != "" && $("#tbxToAmount").val() != "") {
            var fromAmount = parseFloat(transfer.utils.removeAllExistCharacter($("#tbxFromAmount").val(), ","));
            var toAmount = parseFloat(transfer.utils.removeAllExistCharacter($("#tbxToAmount").val(), ","));

            if (isNaN(fromAmount) || fromAmount == 0) {
                transfer.ui.showError("From amount is invalid.");
                return false;
            }

            if (isNaN(toAmount) || toAmount == 0) {
                transfer.ui.showError("To amount is invalid.");
                return false;
            }

            if ((fromAmount < 0 && toAmount > 0) || (fromAmount > 0 && toAmount < 0)) {
                return false;
            }
        }
        return true;
    };

    transfer.business.checkTransferAmountOnTransfer = function () {
        if (transfer.business.rowFromUser == null || transfer.business.rowToUser == null) {
            return false;
        }
        if ($("#tbxFromAmount").val() != "" && $("#tbxToAmount").val() != "") {
            var fromAmount = parseFloat(transfer.utils.removeAllExistCharacter($("#tbxFromAmount").val(), ","));
            var toAmount = parseFloat(transfer.utils.removeAllExistCharacter($("#tbxToAmount").val(), ","));

            if (isNaN(fromAmount) || fromAmount == 0) {
                transfer.ui.showError("From amount is invalid.");
                return false;
            }

            if (isNaN(toAmount) || toAmount == 0) {
                transfer.ui.showError("To amount is invalid.");
                return false;
            }

            if ((fromAmount < 0 && toAmount > 0) || (fromAmount > 0 && toAmount < 0)) {
                transfer.ui.showError("From amount or To amount is invalid");
                return false;
            }
        }
        else if ($("#tbxFromAmount").val() == "") {
            transfer.ui.showError("From amount is invalid.");
            return false;
        }
        else if ($("#tbxToAmount").val() == "") {
            transfer.ui.showError("To amount is invalid.");
            return false;
        }
        return true;
    };

    transfer.business.showTransExRate = function () {
        if (transfer.business.checkTransferAmount() == false) {
            return;
        }
        else {
            transfer.ui.hideMessage();
            var fromAmount = parseFloat(transfer.utils.removeAllExistCharacter($("#tbxFromAmount").val(), ","));
            var toAmount = parseFloat(transfer.utils.removeAllExistCharacter($("#tbxToAmount").val(), ","));
            if (!isNaN(fromAmount) && fromAmount != 0 && !isNaN(toAmount) && toAmount != 0) {
                var transExRate = toAmount / fromAmount;
                transExRate = Math.round(transExRate * 10000) / 10000;
                $("#spanExRateTrans").text(transExRate);
            }
        }
    };

    transfer.ui.showError = function (message) {
        $("#divMessage").attr("class", "alertboxerr").html("<div style='padding-top:0px; padding-bottom:0px'>" + message + "</div>").show();
    };

    transfer.ui.showSuccessMessage = function (message) {
        $("#divMessage").attr("class", "alertboxscc").html("<div style='padding-top:0px; padding-bottom:0px'>" + message + "</div>").show();
    };

    transfer.ui.hideMessage = function () {
        $("#divMessage").hide();
    };

    transfer.ui.getAllForexAccount = function () {
        $.ajax({
            type: "POST",
            url: "Services/WCF/Customer/ServiceCustomerInfo.svc/SelectAllForexAccount",
            data: null,
            contentType: "application/json; charset=utf-8",
            success: function (rs) {
                if (typeof rs != "undefined" && rs != null && rs.length > 0) {
                    var forexAccounts = rs[0].username.split(",");
                    for (var i = 0; i < forexAccounts.length; ++i) {
                        $("#selectForexAccount").append("<option value='" + forexAccounts[i] + "'>" + forexAccounts[i] + "</option>");
                    }
                    $("#selectForexAccount").find("option").each(function (index, element) {
                        if ($(element).val() == "DIFF10") {
                            $(element).attr("selected", "selected");
                        }
                    });
                }
            }
            , error: function (e) {
            }
        });
    };

    transfer.ui.init = function () {
        //left_tab1_1
        $("#left_tab1_1").click(function () {
            $(this).parents("ul:first").find("li[class='current']").attr("class", "");
            $(this).attr("class", "current");

            $("#content_tab1").show();
            $("#content_tab2").hide();
            transfer.ui.hideMessage();
        });

        //left_tab1_2
        $("#left_tab1_2").click(function () {
            $(this).parents("ul:first").find("li[class='current']").attr("class", "");
            $(this).attr("class", "current");

            $("#content_tab2").show();
            $("#content_tab1").hide();
            transfer.ui.hideMessage();
            if (typeof this.isTheFirst == "undefined") {
                transfer.business.showExRateFromUser();
                this.isTheFirst = false;
            }
        });

        //tbxFromUser
        $("#tbxFromUser").val($("#hiddenSearchedName").val());
        $("#tbxFromUser").keyup(function (e) {
            if (typeof this.timeoutID != "undefined") {
                clearTimeout(this.timeoutID);
            }
            if (e.keyCode == 13) {
                transfer.business.showExRateFromUser();
            }
            else {
                this.timeoutID = setTimeout(
                    $.proxy(function () {
                        clearTimeout(this.timeoutID);
                        transfer.business.showExRateFromUser();
                    }, this), 1000);
            }

        });

        //tbxToUser
        $("#tbxToUser").keyup(function (e) {
            if (typeof this.timeoutID != "undefined") {
                clearTimeout(this.timeoutID);
            }
            if (e.keyCode == 13) {
                transfer.business.showExRateToUser();
            }
            else {
                this.timeoutID = setTimeout(
                    $.proxy(function () {
                        transfer.business.showExRateToUser();
                        clearTimeout(this.timeoutID);
                    }, this)
                , 1000);
            }
        });

        //selectForexAccount
        this.getAllForexAccount();

        //tbxFromAmount
        $("#tbxFromAmount").ForceDecimalOnly().ForceCurrencyOnly();
        $("#tbxFromAmount").keyup(function (e) {
            if (typeof this.timeoutID != "undefined") {
                clearTimeout(this.timeoutID);
            }

            if (e.keyCode == 13) {
                transfer.business.showTransExRate();
            }
            else {
                this.timeoutID = setTimeout(
                    $.proxy(function () {
                        transfer.business.showTransExRate();
                        clearTimeout(this.timeoutID);
                    }, this)
                , 1000);
            }
        });

        //tbxToAmount
        $("#tbxToAmount").ForceDecimalOnly().ForceCurrencyOnly();
        $("#tbxToAmount").keyup(function (e) {
            if (typeof this.timeoutID != "undefined") {
                clearTimeout(this.timeoutID);
            }
            if (e.keyCode == 13) {
                transfer.business.showTransExRate();
            }
            else {
                this.timeoutID = setTimeout(transfer.business.showTransExRate, 1000);
            }
        });

        //---------------------------
        //"Transfer Yesterday's Transactions tab"
        //exrate
        //$("#exrate").ForceDecimalOnly().ForceCurrencyOnly();

        //exrate
        //$("#txtamount").ForceDecimalOnly().ForceCurrencyOnly();
        //---------------------------

        //btnTransfer
        $("#btnTransfer").click(function () {
            var fromAmount = parseFloat(transfer.utils.removeAllExistCharacter($("#tbxFromAmount").val(), ","));
            var toAmount = parseFloat(transfer.utils.removeAllExistCharacter($("#tbxToAmount").val(), ","));

            if (transfer.business.rowFromUser == null || typeof transfer.business.rowFromUser == "undefined") {
                transfer.ui.showError("From user is invalid.");
                return;
            }
            else if (isNaN(fromAmount) || fromAmount == 0) {
                transfer.ui.showError("From amount is invalid.");
                return false;
            }
            else if (transfer.business.rowToUser == null || typeof transfer.business.rowToUser == "undefined") {
                transfer.ui.showError("To user is invalid.");
                return;
            }
            else if (isNaN(toAmount) || toAmount == 0) {
                transfer.ui.showError("To amount is invalid.");
                return false;
            }
            else if (transfer.business.rowToUser.actualrate == 0) {
                transfer.ui.showError("System Ex.Rate is invalid.");
                return;
            }
            else if (transfer.business.checkTransferAmountOnTransfer() == false) {
                return;
            }
            else if (transfer.business.rowFromUser.currency == transfer.business.rowToUser.currency) {
                transfer.ui.showError("You can only do cross-currency transfer between 2 accounts that have different currencies'");
                return;
            }
            else {
                transfer.ui.hideMessage();
                transfer.business.save();
            }
        });
    };

    transfer.business.save = function () {
        BlockUI();
        var param = {
            "fromCustID": transfer.business.rowFromUser.custid,
            "toCustID": transfer.business.rowToUser.custid,
            "fromAmount": parseFloat(transfer.utils.removeAllExistCharacter($("#tbxFromAmount").val(), ",")),
            "toAmount": parseFloat(transfer.utils.removeAllExistCharacter($("#tbxToAmount").val(), ",")),
            "fromExRate": transfer.business.rowFromUser.actualrate,
            "toExRate": transfer.business.rowToUser.actualrate,
            "forexUsername": $("#selectForexAccount").val(),
            "remark": $("#tbxDescription").val(),
            "custid": $("#hiddenLoginID").val()
        };
        $.ajax({
            type: "POST",
            url: "Services/WCF/Customer/ServiceCustomerInfo.svc/CustomerCrossTransferInsert",
            data: $.toJSON(param),
            contentType: "application/json; charset=utf-8",
            success: function (rs) {
                if (rs != null && typeof rs != "undefined") {
                    if (rs.ErrorCode == 0) {
                        transfer.ui.showSuccessMessage("Transfer successfully.");
                    }
                    else {
                        transfer.ui.showError(rs.ErrorMessage);
                    }
                }
                $.unblockUI();
            }
            , error: function (e) {
                $.unblockUI();
            }
        });
    };

    transfer.utils.init = function () {
        //(2, '.', ',')
        Number.prototype.formatMoney = function (c, d, t) {
            var n = this, c = isNaN(c = Math.abs(c)) ? 2 : c, d = d == undefined ? "," : d, t = t == undefined ? "." : t, s = n < 0 ? "-" : "", i = parseInt(n = Math.abs(+n || 0).toFixed(c)) + "", j = (j = i.length) > 3 ? j % 3 : 0;
            return s + (j ? i.substr(0, j) + t : "") + i.substr(j).replace(/(\d{3})(?=\d)/g, "$1" + t) + (c ? d + Math.abs(n - i).toFixed(c).slice(2) : "");
        };

        jQuery.fn.ForceDecimalOnly = function () {
            return this.each(function () {
                $(this).keydown(function (e) {
                    //at keydown event, new-value is not available on textbox, still old-value
                    //at keydown event, prevent "not allow keys" is a main action
                    //not format at this event
                    //-------------------------------------------
                    //check dot(.), just for currency
                    var key = e.charCode || e.keyCode || 0;
                    if (key == 110) {//dot(.)
                        if ($(this).val() == "" || $(this).val().indexOf(".") >= 0) {
                            return false;
                        }
                    }
                    //-------------------------------------------
                    var isAccept =
                     (
                        (e.ctrlKey && e.keyCode == 67) || //accept ctrl + c(copy)
                        (e.ctrlKey && e.keyCode == 86) || //accept ctrl + v(paste)
                        key == 16 || //shift
                        key == 17 || //ctrl
                        key == 8 || //backspace
                        key == 36 || //home
                        key == 35 || //end
                        key == 110 ||  //.
                        key == 9 || //tab
                        key == 46 || //delete
                        key == 109 || //-
                        (key >= 37 && key <= 40) || //left , right , up , down (arrow)
                        (key >= 48 && key <= 57) || //0-9
                        (key >= 96 && key <= 105)); //numpad 0-9
                    return isAccept;
                });
            });
        };

        jQuery.fn.ForceCurrencyOnly = function () {
            return this.each(function () {
                $(this).keyup(function (e) {
                    //if at keydown event is stopped, keyup event has still be fired
                    //at keyup event, new-value is available on textbox
                    //at keyup event, format is a main action , except some follow cases
                    //--------------------------------------
                    //do not format for these cases
                    var key = e.charCode || e.keyCode || 0;
                    if (key == 36 //home
                        || key == 17 //ctrl
                        || key == 16 //shift
                        || key == 35 //end
                        || key == 37 //left arrow
                        || key == 38 //up arrow
                        || key == 39//right arrow
                        || key == 40//down arrow
                        || key == 109//-
                        ) {
                        return true;
                    }
                    //--------------------------------------

                    var value = $(this).val();
                    var backupValue = $(this).val();

                    if (value.indexOf(".") == value.length - 1 || parseFloat(value) == 0) {
                        return true;
                    }

                    if (value.indexOf(".") >= 0) {
                        var arr = value.split('.');
                        if (parseInt(arr[1]) == 0) {
                            return true;
                        }
                    }

                    while (value.indexOf(",") >= 0) {
                        value = transfer.utils.removeAllExistCharacter(value, ","); //value.replace(",", "");
                    }
                    var strFormated = parseFloat(value).formatMoney(4, ".", ",");
                    while ((strFormated.substr(strFormated.length - 1, 1) == "0" && strFormated.indexOf(".") >= 0) || strFormated.substr(strFormated.length - 1, 1) == ".") {
                        strFormated = strFormated.substr(0, strFormated.length - 1);
                    }
                    //backup current caret position
                    var currentPosition = $(this).caret().start;

                    //set formated value
                    $(this).val(strFormated);

                    //recover current caret position
                    var numberIncreasedChars = strFormated.length - backupValue.length;
                    if (numberIncreasedChars >= 0) {
                        transfer.utils.setCaretPosition(this, currentPosition + numberIncreasedChars);
                    }
                    else {
                        transfer.utils.setCaretPosition(this, currentPosition);
                    }
                });
            });
        };
    };

    transfer.utils.removeAllExistCharacter = function (value, character) {
        while (value.indexOf(character) >= 0) {
            value = value.replace(character, "");
        }
        return value;
    };

    transfer.utils.setCaretPosition = function (ctrl, pos) {
        if (ctrl.setSelectionRange) {
            ctrl.focus();
            ctrl.setSelectionRange(pos, pos);
        }
        else if (ctrl.createTextRange) {
            var range = ctrl.createTextRange();
            range.collapse(true);
            range.moveEnd('character', pos);
            range.moveStart('character', pos);
            range.select();
        }
    };

    transfer.init();
});

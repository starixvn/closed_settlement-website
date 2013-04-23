var transfer = {
    ForexAccDefault: "DIFF10"
};

transfer.init = function () {
    transfer.tab.init();
    transfer.crossCurrencyTransfer.init();
    transfer.crossCurrencyTransfer2.init();
};

transfer.tab = {
    init: function () {
        //left_tab1_1
        $("#left_tab1_1").click(function () {
            $(this).parents("ul:first").find("li[class='current']").attr("class", "");
            $(this).attr("class", "current");

            $("#content_tab1").show();
            $("#content_tab2").hide();
            $("#content_tab3").hide();
            transfer.utils.hideMessage();
        });

        //left_tab1_2
        $("#left_tab1_2").click(function () {
            $(this).parents("ul:first").find("li[class='current']").attr("class", "");
            $(this).attr("class", "current");

            $("#content_tab2").show();
            $("#content_tab1").hide();
            $("#content_tab3").hide();
            transfer.utils.hideMessage();
            if (typeof this.isTheFirst == "undefined") {
                transfer.crossCurrencyTransfer.showExRateFromUser();
                this.isTheFirst = false;
            }
        });
        //left_tab1_3
        $("#left_tab1_3").click(function () {
            $(this).parents("ul:first").find("li[class='current']").attr("class", "");
            $(this).attr("class", "current");

            $("#content_tab3").show();
            $("#content_tab1").hide();
            $("#content_tab2").hide();
            transfer.utils.hideMessage();
            if (typeof this.isTheFirst == "undefined") {
                transfer.crossCurrencyTransfer2.showExRateFromUser();
                this.isTheFirst = false;
            }
        });
    }
};

transfer.crossCurrencyTransfer = {
    showExRateFromUser: function () {
        transfer.utils.hideMessage();
        $("#divAjaxLoaderImageFromUser").show();

        if ($("#tbxFromUser").val() == '') {
            $("#spanCurrencyFromUser").text('');
            $("#spanExRateFromUser").text('');
            $("#trSystemTransExRate").hide();
            this.rowFromUser = null;
            $("#divAjaxLoaderImageFromUser").hide();
            return;
        }
        var param = { username: $("#tbxFromUser").val() };
        $.get("SelectCustomerExRate", param, $.proxy(function (rs) {
            if (rs == null || typeof rs == "undefined" || typeof rs.username == "undefined") {
                $("#spanCurrencyFromUser").text('');
                $("#spanExRateFromUser").text('');
                $("#trSystemTransExRate").hide();
                transfer.utils.showError("User does not exist");
                this.rowFromUser = null;
            }
            else {
                this.rowFromUser = rs;
                $("#spanCurrencyFromUser").text(rs.currency);
                $("#spanExRateFromUser").text(rs.actualrate);
                this.showSystemExRate();
            }
            $("#divAjaxLoaderImageFromUser").hide();
        }, this));
    },
    showExRateToUser: function () {
        transfer.utils.hideMessage();
        $("#divAjaxLoaderImageToUser").show();

        if ($("#tbxToUser").val() == '') {
            $("#spanCurrencyToUser").text('');
            $("#spanExRateToUser").text('');
            $("#trSystemTransExRate").hide();
            this.rowToUser = null;
            $("#divAjaxLoaderImageToUser").hide();
            return;
        }
        var param = { username: $("#tbxToUser").val() };
        $.get("SelectCustomerExRate", param, $.proxy(function (rs) {
            if (rs == null || typeof rs == "undefined" || typeof rs.username == "undefined") {
                $("#spanCurrencyToUser").text('');
                $("#spanExRateToUser").text('');
                $("#trSystemTransExRate").hide();
                transfer.utils.showError("User does not exist");
                this.rowToUser = null;
            }
            else {
                this.rowToUser = rs;
                $("#spanCurrencyToUser").text(rs.currency);
                $("#spanExRateToUser").text(rs.actualrate);
                this.showSystemExRate();
            }
            $("#divAjaxLoaderImageToUser").hide();
        }, this));
    },
    showSystemExRate: function () {
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
    },
    checkTransferAmount: function () {
        if ($("#tbxFromAmount").val() != "" && $("#tbxToAmount").val() != "") {
            var fromAmount = parseFloat(transfer.utils.removeAllExistCharacter($("#tbxFromAmount").val(), ","));
            var toAmount = parseFloat(transfer.utils.removeAllExistCharacter($("#tbxToAmount").val(), ","));

            if (isNaN(fromAmount) || fromAmount == 0) {
                transfer.utils.showError("From amount is invalid.");
                return false;
            }

            if (isNaN(toAmount) || toAmount == 0) {
                transfer.utils.showError("To amount is invalid.");
                return false;
            }

            if ((fromAmount < 0 && toAmount > 0) || (fromAmount > 0 && toAmount < 0)) {
                return false;
            }
        }
        return true;
    },
    checkTransferAmountOnTransfer: function () {
        if (transfer.crossCurrencyTransfer.rowFromUser == null || transfer.crossCurrencyTransfer.rowToUser == null) {
            return false;
        }
        if ($("#tbxFromAmount").val() != "" && $("#tbxToAmount").val() != "") {
            var fromAmount = parseFloat(transfer.utils.removeAllExistCharacter($("#tbxFromAmount").val(), ","));
            var toAmount = parseFloat(transfer.utils.removeAllExistCharacter($("#tbxToAmount").val(), ","));

            if (isNaN(fromAmount) || fromAmount == 0) {
                transfer.utils.showError("From amount is invalid.");
                return false;
            }

            if (isNaN(toAmount) || toAmount == 0) {
                transfer.utils.showError("To amount is invalid.");
                return false;
            }

            if ((fromAmount < 0 && toAmount > 0) || (fromAmount > 0 && toAmount < 0)) {
                transfer.utils.showError("From amount or To amount is invalid");
                return false;
            }
        }
        else if ($("#tbxFromAmount").val() == "") {
            transfer.utils.showError("From amount is invalid.");
            return false;
        }
        else if ($("#tbxToAmount").val() == "") {
            transfer.utils.showError("To amount is invalid.");
            return false;
        }
        return true;
    },
    showTransExRate: function () {
        if (transfer.crossCurrencyTransfer.checkTransferAmount() == false) {
            return;
        }
        else {
            transfer.utils.hideMessage();
            var fromAmount = parseFloat(transfer.utils.removeAllExistCharacter($("#tbxFromAmount").val(), ","));
            var toAmount = parseFloat(transfer.utils.removeAllExistCharacter($("#tbxToAmount").val(), ","));
            if (!isNaN(fromAmount) && fromAmount != 0 && !isNaN(toAmount) && toAmount != 0) {
                var transExRate = toAmount / fromAmount;
                transExRate = Math.round(transExRate * 10000) / 10000;
                $("#spanExRateTrans").text(transExRate);
            }
        }
    },
    save: function () {
        transfer.utils.blockUI();
        var param = {
            FromCustID: transfer.crossCurrencyTransfer.rowFromUser.custid,
            ToCustID: transfer.crossCurrencyTransfer.rowToUser.custid,
            FromAmount: parseFloat($("#tbxFromAmount").getNumberValue()),
            ToAmount: parseFloat($("#tbxToAmount").getNumberValue()),
            FromExRate: transfer.crossCurrencyTransfer.rowFromUser.actualrate,
            ToExRate: transfer.crossCurrencyTransfer.rowToUser.actualrate,
            ForexUsername: $("#selectForexAccount").val(),
            Remark: ($("#tbxDescription").val() == "" ? " " : $("#tbxDescription").val())
        };
        $.post("CrossCurrencyTransferInsert", param, function (data) {
            if (parseInt(data.ErrorCode) != 0) {
                transfer.utils.showError(data.ErrorMessage);
            }
            else {
                transfer.utils.showSuccessMessage("Transfer successfully.");
            }
            transfer.utils.unBlock();
        });
    },
    getAllForexAccount: function () {
        $.post("SelectAllForexAccount", null, function (rs) {
            if (typeof rs != "undefined" && rs != null && rs.length > 0) {
                var forexAccounts = rs[0].username.split(",");
                for (var i = 0; i < forexAccounts.length; ++i) {
                    $("#selectForexAccount").append("<option value='" + forexAccounts[i] + "'>" + forexAccounts[i] + "</option>");
                }
                if ($("#hiddenForexAccount").val() != "") {
                    $("#selectForexAccount").val($("#hiddenForexAccount").val());
                }
                else {
                    $("#selectForexAccount").val(transfer.ForexAccDefault);
                }
            }
        });
    },
    init: function () {
        //tbxFromUser
        $("#tbxFromUser").keyup(function (e) {
            if (typeof this.timeoutID != "undefined") {
                clearTimeout(this.timeoutID);
            }
            if (e.keyCode == 13) {
                transfer.crossCurrencyTransfer.showExRateFromUser();
            }
            else {
                this.timeoutID = setTimeout(
                        $.proxy(function () {
                            clearTimeout(this.timeoutID);
                            transfer.crossCurrencyTransfer.showExRateFromUser();
                        }, this), 1000);
            }

        });

        //tbxToUser
        $("#tbxToUser").keyup(function (e) {
            if (typeof this.timeoutID != "undefined") {
                clearTimeout(this.timeoutID);
            }
            if (e.keyCode == 13) {
                transfer.crossCurrencyTransfer.showExRateToUser();
            }
            else {
                this.timeoutID = setTimeout(
                        $.proxy(function () {
                            transfer.crossCurrencyTransfer.showExRateToUser();
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
                transfer.crossCurrencyTransfer.showTransExRate();
            }
            else {
                this.timeoutID = setTimeout(
                        $.proxy(function () {
                            transfer.crossCurrencyTransfer.showTransExRate();
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
                transfer.crossCurrencyTransfer.showTransExRate();
            }
            else {
                this.timeoutID = setTimeout(transfer.crossCurrencyTransfer.showTransExRate, 1000);
            }
        });

        //btnTransfer
        $("#btnTransfer").click(function () {
            var fromAmount = parseFloat(transfer.utils.removeAllExistCharacter($("#tbxFromAmount").val(), ","));
            var toAmount = parseFloat(transfer.utils.removeAllExistCharacter($("#tbxToAmount").val(), ","));

            if (transfer.crossCurrencyTransfer.rowFromUser == null || typeof transfer.crossCurrencyTransfer.rowFromUser == "undefined") {
                transfer.utils.showError("From user is invalid.");
                return;
            }
            else if (isNaN(fromAmount) || fromAmount == 0) {
                transfer.utils.showError("From amount is invalid.");
                return false;
            }
            else if (transfer.crossCurrencyTransfer.rowToUser == null || typeof transfer.crossCurrencyTransfer.rowToUser == "undefined") {
                transfer.utils.showError("To user is invalid.");
                return;
            }
            else if (isNaN(toAmount) || toAmount == 0) {
                transfer.utils.showError("To amount is invalid.");
                return false;
            }
            else if (transfer.crossCurrencyTransfer.rowToUser.actualrate == 0) {
                transfer.utils.showError("System Ex.Rate is invalid.");
                return;
            }
            else if (transfer.crossCurrencyTransfer.checkTransferAmountOnTransfer() == false) {
                return;
            }
            else if ($("#tbxDescription").val().indexOf(">") >= 0 || $("#tbxDescription").val().indexOf("<") >= 0) {
                transfer.utils.showError("Remark is invalid.");
                return;
            }
            else if (transfer.crossCurrencyTransfer.rowFromUser.currency == transfer.crossCurrencyTransfer.rowToUser.currency) {
                transfer.utils.showError("You can only do cross-currency transfer between 2 accounts that have different currencies");
                return;
            }
            else {
                transfer.utils.hideMessage();
                transfer.crossCurrencyTransfer.save();
            }
        });
    }
};

transfer.crossCurrencyTransfer2 = {
    showExRateFromUser: function () {
        transfer.utils.hideMessage();
        $("#divAjaxLoaderImageFromUser2").show();

        if ($("#tbxFromUser2").val() == '') {
            $("#spanCurrencyFromUser2").text('');
            $("#spanExRateFromUser2").text('');
            $("#trSystemTransExRate2").hide();
            this.rowFromUser = null;
            $("#divAjaxLoaderImageFromUser2").hide();
            return;
        }
        var param = { username: $("#tbxFromUser2").val() };
        $.get("SelectCustomerExRate", param, $.proxy(function (rs) {
            if (rs == null || typeof rs == "undefined" || typeof rs.username == "undefined") {
                $("#spanCurrencyFromUser2").text('');
                $("#spanExRateFromUser2").text('');
                $("#trSystemTransExRate2").hide();
                transfer.utils.showError("User does not exist");
                this.rowFromUser = null;
            }
            else {
                this.rowFromUser = rs;
                $("#spanCurrencyFromUser2").text(rs.currency);
                $("#spanExRateFromUser2").text(rs.actualrate);

                if (nexcelUtils.trimStartEnd(rs.currency) == "MYR" || nexcelUtils.trimStartEnd(rs.currency) == "RM") {
                    $("#selectCurrencyCodeFromUser2").removeAttr("disabled");
                    $("#tbxFromAmountConvert2").removeAttr("disabled").css("background-color", "");
                    $("#imgCalculateLeft").css("cursor", "pointer").css("background-position", "0px 0px");
                }
                else {
                    $("#selectCurrencyCodeFromUser2").attr("disabled", "disabled");
                    $("#tbxFromAmountConvert2").attr("disabled", "disabled").css("background-color", "#DDDDDD");
                    $("#imgCalculateLeft").css("cursor", "").css("background-position", "19px 0px");
                }

                this.showSystemExRate();
            }
            $("#divAjaxLoaderImageFromUser2").hide();
        }, this));
    },
    showExRateToUser: function () {
        transfer.utils.hideMessage();
        $("#divAjaxLoaderImageToUser2").show();

        if ($("#tbxToUser2").val() == '') {
            $("#spanCurrencyToUser2").text('');
            $("#spanExRateToUser2").text('');
            $("#trSystemTransExRate2").hide();
            this.rowToUser = null;
            $("#divAjaxLoaderImageToUser2").hide();
            return;
        }
        var param = { username: $("#tbxToUser2").val() };
        $.get("SelectCustomerExRate", param, $.proxy(function (rs) {
            if (rs == null || typeof rs == "undefined" || typeof rs.username == "undefined") {
                $("#spanCurrencyToUser2").text('');
                $("#spanExRateToUser2").text('');
                $("#trSystemTransExRate2").hide();
                transfer.utils.showError("User does not exist");
                this.rowToUser = null;
            }
            else {
                this.rowToUser = rs;
                $("#spanCurrencyToUser2").text(rs.currency);
                $("#spanExRateToUser2").text(rs.actualrate);

                if (nexcelUtils.trimStartEnd(rs.currency) == "MYR" || nexcelUtils.trimStartEnd(rs.currency) == "RM") {
                    $("#selectCurrencyCodeToUser2").removeAttr("disabled");
                    $("#tbxToAmountConvert2").removeAttr("disabled").css("background-color", "");
                    $("#imgCalculateRight").css("cursor", "pointer").css("background-position", "0px 0px");
                }
                else {
                    $("#selectCurrencyCodeToUser2").attr("disabled", "disabled");
                    $("#tbxToAmountConvert2").attr("disabled", "disabled").css("background-color", "#DDDDDD");
                    $("#imgCalculateRight").css("cursor", "").css("background-position", "19px 0px");
                }
                this.showSystemExRate();
            }
            $("#divAjaxLoaderImageToUser2").hide();
        }, this));
    },
    showSystemExRate: function () {
        if (this.rowFromUser != null && this.rowToUser != null) {
            $("#spanPerCurrencySystem2").text(this.rowFromUser.currency + "/" + this.rowToUser.currency);
            if (this.rowToUser.actualrate != 0) {
                var systemExRate = this.rowFromUser.actualrate / this.rowToUser.actualrate;
                systemExRate = Math.round(systemExRate * 10000) / 10000;
                $("#spanExRateSystem2").text(systemExRate);
            }
            $("#spanPerCurrencyTrans2").text(this.rowFromUser.currency + "/" + this.rowToUser.currency);
            $("#trSystemTransExRate2").show();
        }
    },
    checkTransferAmount: function () {
        if ($("#tbxFromAmount2").val() != "" && $("#tbxToAmount2").val() != "") {
            var fromAmount = parseFloat(transfer.utils.removeAllExistCharacter($("#tbxFromAmount2").val(), ","));
            var toAmount = parseFloat(transfer.utils.removeAllExistCharacter($("#tbxToAmount2").val(), ","));

            /*
            if (isNaN(fromAmount) || fromAmount == 0) {
            transfer.utils.showError("From amount is invalid.");
            return false;
            }

            if (isNaN(toAmount) || toAmount == 0) {
            transfer.utils.showError("To amount is invalid.");
            return false;
            }
            */

            if ((fromAmount < 0 && toAmount > 0) || (fromAmount > 0 && toAmount < 0)) {
                return false;
            }
        }
        return true;
    },
    checkTransferAmountOnTransfer: function () {
        if (transfer.crossCurrencyTransfer2.rowFromUser == null || transfer.crossCurrencyTransfer2.rowToUser == null) {
            return false;
        }
        if ($("#tbxFromAmount2").val() != "" && $("#tbxToAmount2").val() != "") {
            var fromAmount = parseFloat(transfer.utils.removeAllExistCharacter($("#tbxFromAmount2").val(), ","));
            var toAmount = parseFloat(transfer.utils.removeAllExistCharacter($("#tbxToAmount2").val(), ","));

            if (isNaN(fromAmount) || fromAmount == 0) {
                transfer.utils.showError("From amount is invalid.");
                return false;
            }

            if (isNaN(toAmount) || toAmount == 0) {
                transfer.utils.showError("To amount is invalid.");
                return false;
            }

            if ((fromAmount < 0 && toAmount > 0) || (fromAmount > 0 && toAmount < 0)) {
                transfer.utils.showError("From amount or To amount is invalid");
                return false;
            }
        }
        else if ($("#tbxFromAmount2").val() == "") {
            transfer.utils.showError("From amount is invalid.");
            return false;
        }
        else if ($("#tbxToAmount2").val() == "") {
            transfer.utils.showError("To amount is invalid.");
            return false;
        }
        return true;
    },
    showTransExRate: function () {
        if (transfer.crossCurrencyTransfer2.checkTransferAmount() == false) {
            return;
        }
        else {
            transfer.utils.hideMessage();
            var fromAmount = parseFloat(transfer.utils.removeAllExistCharacter($("#tbxFromAmount2").val(), ","));
            var toAmount = parseFloat(transfer.utils.removeAllExistCharacter($("#tbxToAmount2").val(), ","));
            if (!isNaN(fromAmount) && fromAmount != 0 && !isNaN(toAmount) && toAmount != 0) {
                var transExRate = toAmount / fromAmount;
                transExRate = Math.round(transExRate * 10000) / 10000;
                $("#spanExRateTrans2").text(transExRate);
            }
        }
    },
    save: function () {
        transfer.utils.blockUI();
        var param = {
            FromCustID: transfer.crossCurrencyTransfer2.rowFromUser.custid,
            ToCustID: transfer.crossCurrencyTransfer2.rowToUser.custid,
            FromAmount: parseFloat($("#tbxFromAmount2").getNumberValue()),
            ToAmount: parseFloat($("#tbxToAmount2").getNumberValue()),
            FromExRate: transfer.crossCurrencyTransfer2.rowFromUser.actualrate,
            ToExRate: transfer.crossCurrencyTransfer2.rowToUser.actualrate,
            ForexUsername: $("#selectForexAccount2").val(),
            Remark: ($("#tbxDescription2").val() == "" ? " " : $("#tbxDescription2").val())
        };
        $.post("CrossCurrencyTransferInsert", param, function (data) {
            if (parseInt(data.ErrorCode) != 0) {
                transfer.utils.showError(data.ErrorMessage);
            }
            else {
                transfer.utils.showSuccessMessage("Transfer successfully.");
            }
            transfer.utils.unBlock();
        });
    },
    getAllForexAccount: function () {
        $.post("SelectAllForexAccount", null, function (rs) {
            if (typeof rs != "undefined" && rs != null && rs.length > 0) {
                var forexAccounts = rs[0].username.split(",");
                for (var i = 0; i < forexAccounts.length; ++i) {
                    $("#selectForexAccount2").append("<option value='" + forexAccounts[i] + "'>" + forexAccounts[i] + "</option>");
                }
                if ($("#hiddenForexAccount2").val() != "") {
                    $("#selectForexAccount2").val($("#hiddenForexAccount").val());
                }
                else {
                    $("#selectForexAccount2").val(transfer.ForexAccDefault);
                }
            }
        });
    },
    init: function () {
        //tbxFromUser2
        $("#tbxFromUser2").keyup(function (e) {
            if (typeof this.timeoutID != "undefined") {
                clearTimeout(this.timeoutID);
            }
            if (e.keyCode == 13) {
                transfer.crossCurrencyTransfer2.showExRateFromUser();
            }
            else {
                this.timeoutID = setTimeout(
                        $.proxy(function () {
                            clearTimeout(this.timeoutID);
                            transfer.crossCurrencyTransfer2.showExRateFromUser();
                        }, this), 1000);
            }

        });

        //tbxToUser2
        $("#tbxToUser2").keyup(function (e) {
            if (typeof this.timeoutID != "undefined") {
                clearTimeout(this.timeoutID);
            }
            if (e.keyCode == 13) {
                transfer.crossCurrencyTransfer2.showExRateToUser();
            }
            else {
                this.timeoutID = setTimeout(
                        $.proxy(function () {
                            transfer.crossCurrencyTransfer2.showExRateToUser();
                            clearTimeout(this.timeoutID);
                        }, this)
                    , 1000);
            }
        });

        //selectForexAccount
        this.getAllForexAccount();

        //tbxFromAmount2
        $("#tbxFromAmount2").ForceDecimalOnly().ForceCurrencyOnly();
        $("#tbxFromAmount2").keyup(function (e) {
            if (typeof this.timeoutID != "undefined") {
                clearTimeout(this.timeoutID);
            }

            if (e.keyCode == 13) {
                transfer.crossCurrencyTransfer2.showTransExRate();
            }
            else {
                this.timeoutID = setTimeout(
                        $.proxy(function () {
                            transfer.crossCurrencyTransfer2.showTransExRate();
                            clearTimeout(this.timeoutID);
                        }, this)
                    , 1000);
            }
        });

        //tbxToAmount2
        $("#tbxToAmount2").ForceDecimalOnly().ForceCurrencyOnly();
        $("#tbxToAmount2").keyup(function (e) {
            if (typeof this.timeoutID != "undefined") {
                clearTimeout(this.timeoutID);
            }
            if (e.keyCode == 13) {
                transfer.crossCurrencyTransfer2.showTransExRate();
            }
            else {
                this.timeoutID = setTimeout(transfer.crossCurrencyTransfer2.showTransExRate, 1000);
            }
        });

        //btnTransfer2
        $("#btnTransfer2").click(function () {
            var fromAmount = parseFloat(transfer.utils.removeAllExistCharacter($("#tbxFromAmount2").val(), ","));
            var toAmount = parseFloat(transfer.utils.removeAllExistCharacter($("#tbxToAmount2").val(), ","));

            if (transfer.crossCurrencyTransfer2.rowFromUser == null || typeof transfer.crossCurrencyTransfer2.rowFromUser == "undefined") {
                transfer.utils.showError("From user is invalid.");
                return;
            }
            else if (isNaN(fromAmount) || fromAmount == 0) {
                transfer.utils.showError("From amount is invalid.");
                return false;
            }
            else if (transfer.crossCurrencyTransfer2.rowToUser == null || typeof transfer.crossCurrencyTransfer2.rowToUser == "undefined") {
                transfer.utils.showError("To user is invalid.");
                return;
            }
            else if (isNaN(toAmount) || toAmount == 0) {
                transfer.utils.showError("To amount is invalid.");
                return false;
            }
            else if (transfer.crossCurrencyTransfer2.rowToUser.actualrate == 0) {
                transfer.utils.showError("System Ex.Rate is invalid.");
                return;
            }
            else if (transfer.crossCurrencyTransfer2.checkTransferAmountOnTransfer() == false) {
                return;
            }
            else if ($("#tbxDescription2").val().indexOf(">") >= 0 || $("#tbxDescription2").val().indexOf("<") >= 0) {
                transfer.utils.showError("Remark is invalid.");
                return;
            }
            else if (transfer.crossCurrencyTransfer2.rowFromUser.currency == transfer.crossCurrencyTransfer2.rowToUser.currency) {
                transfer.utils.showError("You can only do cross-currency transfer between 2 accounts that have different currencies");
                return;
            }
            else {
                transfer.utils.hideMessage();
                transfer.crossCurrencyTransfer2.save();
            }
        });

        //selectCurrencyCodeFromUser2
        $("#selectCurrencyCodeFromUser2").change(function () {
            if ($(this).val() == "") {
                $("#spanExRateFromUser2Left").html("");
                $("#tbxFromAmount2").val("");
            }
            else {
                $("#spanExRateFromUser2Left").html(transfer.crossCurrencyTransfer2.getExchangeRate($(this).val()));
            }
        });

        //selectCurrencyCodeToUser2
        $("#selectCurrencyCodeToUser2").change(function () {
            if ($(this).val() == "") {
                $("#spanExRateToUser2Right").html("");
                $("#tbxToAmount2").val("");
            }
            else {
                $("#spanExRateToUser2Right").html(transfer.crossCurrencyTransfer2.getExchangeRate($(this).val()));
            }
        });

        //tbxFromAmountConvert2
        $("#tbxFromAmountConvert2").ForceDecimalOnly().ForceCurrencyOnly();

        //tbxToAmountConvert2
        $("#tbxToAmountConvert2").ForceDecimalOnly().ForceCurrencyOnly();

        //imgCalculateLeft
        $("#imgCalculateLeft").click(function () {
            if (transfer.crossCurrencyTransfer2.rowFromUser == null || typeof (transfer.crossCurrencyTransfer2.rowFromUser) == "undefined") return;
            if (nexcelUtils.trimStartEnd(transfer.crossCurrencyTransfer2.rowFromUser.currency) != "MYR" && nexcelUtils.trimStartEnd(transfer.crossCurrencyTransfer2.rowFromUser.currency) != "RM")
                return;
            transfer.crossCurrencyTransfer2.convertLeft();
        });

        //imgCalculateRight
        $("#imgCalculateRight").click(function () {
            if (transfer.crossCurrencyTransfer2.rowToUser == null || typeof (transfer.crossCurrencyTransfer2.rowToUser) == "undefined") return;
            if (nexcelUtils.trimStartEnd(transfer.crossCurrencyTransfer2.rowToUser.currency) != "MYR" && nexcelUtils.trimStartEnd(transfer.crossCurrencyTransfer2.rowToUser.currency) != "RM")
                return;
            transfer.crossCurrencyTransfer2.convertRight();
        });
    },
    getExchangeRate: function (code) {
        for (var i = 0; i < window.arrExchangeRate.length; ++i) {
            if (nexcelUtils.trimStartEnd(window.arrExchangeRate[i].Code) == nexcelUtils.trimStartEnd(code))
                return window.arrExchangeRate[i].ActualRate;
        }
        return 0;
    },
    convertLeft: function () {
        var rate = transfer.crossCurrencyTransfer2.getExchangeRate($("#selectCurrencyCodeFromUser2").val());
        var tbxVal = 0;
        if ($("#tbxFromAmountConvert2").getNumberValue() != "") {
            tbxVal = parseFloat($("#tbxFromAmountConvert2").getNumberValue());
        }
        var val = rate * tbxVal;
        $("#tbxFromAmount2").val(val.formatMoneyFromVal());
        transfer.crossCurrencyTransfer2.showTransExRate();
    },
    convertRight: function () {
        var rate = transfer.crossCurrencyTransfer2.getExchangeRate($("#selectCurrencyCodeToUser2").val());
        var tbxVal = 0;
        if ($("#tbxToAmountConvert2").getNumberValue() != "") {
            tbxVal = parseFloat($("#tbxToAmountConvert2").getNumberValue());
        }
        var val = rate * tbxVal;
        $("#tbxToAmount2").val(val.formatMoneyFromVal());
        transfer.crossCurrencyTransfer2.showTransExRate();
    }
};

transfer.utils = {
    removeAllExistCharacter: function (value, character) {
        while (value.indexOf(character) >= 0) {
            value = value.replace(character, "");
        }
        return value;
    },
    blockUI: function () {
        if (parent.parent.BlockUI) {
            try { parent.parent.BlockUI(); } catch (e) { }
        }
        else {
            try { parent.BlockUI(); } catch (e) { }
        }
    },
    unBlock: function () {
        if (parent.parent.$ && parent.parent.$.unblockUI) {
            try { parent.parent.$.unblockUI(); } catch (e) { }
        }
        else {
            try { parent.$.unblockUI(); } catch (e) { }
        }
    },
    showError: function (message) {
        showAlertMessage("errMsg", message, 1);
        $("#errMsg").show();
    },
    showSuccessMessage: function (message) {
        showAlertMessage("errMsg", message, 0);
        $("#errMsg").show();
    },
    hideMessage: function () {
        $("#errMsg").hide();
    }
};

$(document).ready(function () {
    transfer.init();
});
    

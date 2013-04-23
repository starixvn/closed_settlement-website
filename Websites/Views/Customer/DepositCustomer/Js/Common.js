var common = {
    moneyMax: 912337203685477.5807,
    errMsg: ""
};

common.init = function () {
    $("#tblTabSBNG").click(function () {
        $("#divSBNGContainer").slideToggle("fast", function () {
            if (!$("#divSBNGContainer").is(":visible")) {
                $("#tblTabSBNG").find("td[arrow]").removeClass("md2").addClass("md3");
            }
            else {
                $("#tblTabSBNG").find("td[arrow]").removeClass("md3").addClass("md2");
            }
        });
    });
    $("#tblTabRacing").click(function () {
        $("#divRacingContainer").slideToggle("fast", function () {
            if (!$("#divRacingContainer").is(":visible")) {
                $("#tblTabRacing").find("td[arrow]").removeClass("md2").addClass("md3");
            }
            else {
                $("#tblTabRacing").find("td[arrow]").removeClass("md3").addClass("md2");
            }
        });
    });

    this.arrInputsReaminSportMinBet_SB = $("#divSBNGContainer").find("input[type='text'][remainsport][minbet]");
    this.arrInputsReaminSportMaxBet_SB = $("#divSBNGContainer").find("input[type='text'][remainsport][maxbet]");
    this.arrInputsReaminSportMaxperMatch_SB = $("#divSBNGContainer").find("input[type='text'][remainsport][maxpermatch]");

    this.arrInputCopyToMinBet_Racing = $("#divRacingContainer").find("input[type='text'][copyto][minbet]");
    this.arrInputCopyToMaxBet_Racing = $("#divRacingContainer").find("input[type='text'][copyto][maxbet]");
    this.arrInputCopyToMaxperMatch_Racing = $("#divRacingContainer").find("input[type='text'][copyto][maxpermatch]");

    this.arrSelectCopyToComm_Racing = $("#divRacingContainer").find("select[copyto][comm]");
    this.arrSelectCopyToWinplacept_Racing = $("#divRacingContainer").find("select[copyto][winplacept]");
    this.arrSelectCopyToHouseautopt_Racing = $("#divRacingContainer").find("select[copyto][houseautopt]");
    this.arrCheckBoxEnable_Racing = $("#divRacingContainer").find("input[type='checkbox'][copyto]");


    $("#divSBNGContainer").find("input[type='text']").ForceDecimalOnly().ForceCurrencyOnly();
    $("#divSBNGContainer").find("input[type='text'][soccer][minbet]").keyup(function () {
        if ($("#cbxCopyAllSportsBook").is(":checked")) {
            common.arrInputsReaminSportMinBet_SB.val($(this).val());
        }
    });
    $("#divSBNGContainer").find("input[type='text'][soccer][maxbet]").keyup(function () {
        if ($("#cbxCopyAllSportsBook").is(":checked")) {
            common.arrInputsReaminSportMaxBet_SB.val($(this).val());
        }
    });
    $("#divSBNGContainer").find("input[type='text'][soccer][maxpermatch]").keyup(function () {
        if ($("#cbxCopyAllSportsBook").is(":checked")) {
            common.arrInputsReaminSportMaxperMatch_SB.val($(this).val());
        }
    });

    $("#divRacingContainer").find("input[type='text']").ForceDecimalOnly().ForceCurrencyOnly();
    $("#divRacingContainer").find("input[type='text'][copyfrom][minbet]").keyup(function () {
        if ($("#cbxCopyRacing").is(":checked")) {
            common.arrInputCopyToMinBet_Racing.val($(this).val());
        }
    });
    $("#divRacingContainer").find("input[type='text'][copyfrom][maxbet]").keyup(function () {
        if ($("#cbxCopyRacing").is(":checked")) {
            common.arrInputCopyToMaxBet_Racing.val($(this).val());
        }
    });
    $("#divRacingContainer").find("input[type='text'][copyfrom][maxpermatch]").keyup(function () {
        if ($("#cbxCopyRacing").is(":checked")) {
            common.arrInputCopyToMaxperMatch_Racing.val($(this).val());
        }
    });
    $("#divRacingContainer").find("select[copyfrom][comm]").change(function () {
        if ($("#cbxCopyRacing").is(":checked")) {
            common.arrSelectCopyToComm_Racing.val($(this).val());
        }
    });
    $("#divRacingContainer").find("select[copyfrom][winplacept]").change(function () {
        if ($("#cbxCopyRacing").is(":checked")) {
            common.arrSelectCopyToWinplacept_Racing.val($(this).val());
        }
    });
    $("#divRacingContainer").find("select[copyfrom][houseautopt]").change(function () {
        if ($("#cbxCopyRacing").is(":checked")) {
            common.arrSelectCopyToHouseautopt_Racing.val($(this).val());
        }
    });
    $("#divRacingContainer").find("input[type='checkbox'][copyfrom]").change(function () {
        if ($("#cbxCopyRacing").is(":checked")) {
            var checked = this.checked;
            common.arrCheckBoxEnable_Racing.each(function () {
                this.checked = checked;
            });
        }
    });

    //Sportbook
    var pt = 0.9;
    var val;
    while (pt > 0) {
        val = parseFloat(pt.toFixed(2));
        $("#selSBPT").append("<option value='" + val + "'>" + val + "</option>");
        $("#selNGPT").append("<option value='" + val + "'>" + val + "</option>");
        pt = pt - 0.01;
    }
    $("#selSBPT").append("<option selected='selected' value='0'>0</option>");
    $("#selNGPT").append("<option selected='selected' value='0'>0</option>");

    //Racing
    //init comm
    pt = 0.04;
    while (pt > 0) {
        val = parseFloat(pt.toFixed(3));
        $("#divRacingContainer").find("select[comm]").append("<option value='" + val + "'>" + val + "</option>");
        pt = pt - 0.005;
    }
    $("#divRacingContainer").find("select[comm]").append("<option selected='selected' value='0'>0</option>");

    //init Win/Place PT
    pt = 0.5;
    while (pt > 0.1) {
        val = parseFloat(pt.toFixed(3));
        $("#divRacingContainer").find("select[winplacept]").append("<option value='" + val + "'>" + val + "</option>");
        pt = pt - 0.1;
    }
    $("#divRacingContainer").find("select[winplacept]").append("<option selected='selected' value='0'>0</option>");

    //init House Auto PT
    pt = 0.7;
    while (pt > 0.1) {
        val = parseFloat(pt.toFixed(3));
        $("#divRacingContainer").find("select[houseautopt]").append("<option value='" + val + "'>" + val + "</option>");
        pt = pt - 0.1;
    }
    $("#divRacingContainer").find("select[houseautopt]").append("<option selected='selected' value='0'>0</option>");

    $("#cbxCopyAllSportsBook").change(function () {
        if ($(this).is(":checked")) {
            $("#divSBNGContainer").find("input[remainsport]").attr("disabled", "disabled");
            var soccerMinBet = $("#divSBNGContainer").find("input[soccer][minbet]").val();
            var soccerMaxBet = $("#divSBNGContainer").find("input[soccer][maxbet]").val();
            var soccerMaxPerMatch = $("#divSBNGContainer").find("input[soccer][maxpermatch]").val();
            $("#divSBNGContainer").find("input[remainsport][minbet]").val(soccerMinBet);
            $("#divSBNGContainer").find("input[remainsport][maxbet]").val(soccerMaxBet);
            $("#divSBNGContainer").find("input[remainsport][maxpermatch]").val(soccerMaxPerMatch);
        }
        else {
            $("#divSBNGContainer").find("input[remainsport]").removeAttr("disabled");
        }
    });

    $("#cbxCopyRacing").change(function () {
        if ($(this).is(":checked")) {
            $("#divRacingContainer").find("input[copyto]").attr("disabled", "disabled");
            $("#divRacingContainer").find("select[copyto]").attr("disabled", "disabled");
            var minbet = $("#divRacingContainer").find("input[minbet]").val();
            var maxbet = $("#divRacingContainer").find("input[maxbet]").val();
            var maxpermatch = $("#divRacingContainer").find("input[maxpermatch]").val();
            var comm = $("#divRacingContainer").find("select[comm]").val();
            var winplacept = $("#divRacingContainer").find("select[winplacept]").val();
            var houseautopt = $("#divRacingContainer").find("select[houseautopt]").val();
            $("#divRacingContainer").find("input[copyto][minbet]").val(minbet);
            $("#divRacingContainer").find("input[copyto][maxbet]").val(maxbet);
            $("#divRacingContainer").find("input[copyto][maxpermatch]").val(maxpermatch);
            $("#divRacingContainer").find("select[copyto][comm]").val(comm);
            $("#divRacingContainer").find("select[copyto][winplacept]").val(winplacept);
            $("#divRacingContainer").find("select[copyto][houseautopt]").val(houseautopt);
            $("#divRacingContainer").find("input[copyto][type='checkbox']").each(function () {
                var arr = $("#divRacingContainer").find("input[copyfrom][type='checkbox']");
                if (arr.length > 0) {
                    this.checked = arr.get(0).checked;
                }
            });
        }
        else {
            $("#divRacingContainer").find("input[copyto]").removeAttr("disabled");
            $("#divRacingContainer").find("select[copyto]").removeAttr("disabled");
        }
    });
};

common.resetValues = function () {
    $("#tbxSuperName").val("");
    $("#tbxMasterName").val("");
    $("#tbxAgentName").val("");
    $("#tbxUserNameAddNew").val("");

    $("#divSBNGContainer").find("input[type='text']").val("0");
    $("#divSBNGContainer").find("select").find("option[value='0']").attr("selected", "selected");

    $("#divRacingContainer").find("input[type='text']").val("0");
    $("#divRacingContainer").find("select").find("option[value='0']").attr("selected", "selected");

    $(document).find("input[type='checkbox']").each(function () {
        this.checked = false;
        $(this).removeAttr("disabled");
    });
    $(document).find("input[type='text']").removeAttr("disabled");
    $(document).find("select").removeAttr("disabled");
};

common.initConstraint = function () {
    $("#divSBNGContainer").find("input[type='text']").each(function () {
        if ($(this).attr("minbet") != null && typeof $(this).attr("minbet") != "undefined" && this.uplineVal > 0) {
            $(this).parent().find("span").html(">=" + parseFloat(this.uplineVal).formatMoneyFromVal());
        }
        else if ($(this).attr("maxbet") != null && typeof $(this).attr("maxbet") != "undefined" && this.uplineVal > 0) {
            $(this).parent().find("span").html("<=" + parseFloat(this.uplineVal).formatMoneyFromVal());
        }
        else if ($(this).attr("maxpermatch") != null && typeof $(this).attr("maxpermatch") != "undefined" && this.uplineVal > 0) {
            $(this).parent().find("span").html("<=" + parseFloat(this.uplineVal).formatMoneyFromVal());
        }
        else if ($(this).attr("maxperball") != null && typeof $(this).attr("maxperball") != "undefined" && this.uplineVal > 0) {
            $(this).parent().find("span").html("<=" + parseFloat(this.uplineVal).formatMoneyFromVal());
        }
    });
    $("#divRacingContainer").find("input[type='text']").each(function () {
        if ($(this).attr("minbet") != null && typeof $(this).attr("minbet") != "undefined" && this.uplineVal > 0) {
            $(this).parent().find("span").html(">=" + parseFloat(this.uplineVal).formatMoneyFromVal());
        }
        else if ($(this).attr("maxbet") != null && typeof $(this).attr("maxbet") != "undefined" && this.uplineVal > 0) {
            $(this).parent().find("span").html("<=" + parseFloat(this.uplineVal).formatMoneyFromVal());
        }
        else if ($(this).attr("maxpermatch") != null && typeof $(this).attr("maxpermatch") != "undefined" && this.uplineVal > 0) {
            $(this).parent().find("span").html("<=" + parseFloat(this.uplineVal).formatMoneyFromVal());
        }
    });
};

common.isNumber = function(n) {
    return !isNaN(parseFloat(n)) && isFinite(n);
};

common.hideDivMsg = function () {
    $("#divErr").hide();
};

common.showErr = function (text) {
    showAlertMessage("divErr", text, 1);
    $("#divErr").show().find("div:first");
};

common.showSuccess = function (text) {
    showAlertMessage("divErr", text, 0);
    $("#divErr").show().find("div:first");
};

common.blockUI = function () {
    if (parent.parent.BlockUI) {
        try { parent.parent.BlockUI(); } catch (e) { }
    }
    else {
        try { parent.BlockUI(); } catch (e) { }
    }
};

common.unBlock = function () {
    if (parent.parent.$ && parent.parent.$.unblockUI) {
        try { parent.parent.$.unblockUI(); } catch (e) { }
    }
    else {
        try { parent.$.unblockUI(); } catch (e) { }
    }
};

common.isEmpty = function (val) {
    if (typeof (val) == "undefined" || val == null) return false;
    return val.replace(/ /g, '') == "";
};

common.validateUserName = function (str) {
    if (str.indexOf("<") >= 0) return false;
    if (str.indexOf(">") >= 0) return false;
    return true;
};

common.verifyDataContrain = function () {
    this.errMsg = "";
    var valid = true;
    var job = function (obj) {
        valid = false; $(obj).css("border-color", "red");
    };
    var setMsg1 = function (val) {
        common.errMsg = "Min Bet must be less than " + val + " (Max Bet)";
    };
    var setMsg2 = function (val) {
        common.errMsg = "Min Bet must be greater than or equal " + val;
    };
    var setMsg3 = function (val) {
        common.errMsg = "Max Bet must be less than or equal " + val;
    };
    var setMsg4 = function (val) {
        common.errMsg = "Max Per Match must be less than or equal " + val;
    };
    var setMsg5 = function (val) {
        common.errMsg = "Max Per Ball must be less than or equal " + val;
    };
    var setMsg6 = function (val) {
        common.errMsg = "Max Bet must be less than " + val + " (Max Per Match)";
    };
    var setMsg7 = function (val) {
        common.errMsg = "Please input value!";
    };
    var setMsg8 = function (val) {
        common.errMsg = "Value is not a number!";
    };
    var setMsg9 = function (val) {
        common.errMsg = "Value can not be nagative!";
    };
    var setMsg10 = function (val) {
        common.errMsg = "Value is so larger!";
    };
    var setMsg11 = function (val) {
        common.errMsg = "Min Bet must be less than " + val + " (Max Per Match)";
    };
    $("#divSBNGContainer").find("input[type='text']").css("border-color", "").each(function () {
        var currentVal = parseFloat($(this).getNumberValue());
        if (valid) {
            if (common.isEmpty($(this).val())) {
                job(this);
                setMsg7();
                return;
            }
            if (!common.isNumber($(this).getNumberValue())) {
                job(this);
                setMsg8();
                return;
            }
            if (currentVal < 0) {
                job(this);
                setMsg9();
                return;
            }
            if (currentVal > common.moneyMax) {
                job(this);
                setMsg10();
                return;
            }

            var minBet, maxBet, maxPerMatch;
            if ($(this).attr("minbet") != null && typeof $(this).attr("minbet") != "undefined") {
                maxBet = parseFloat($(this).parents("tr:first").find("input[maxbet]").getNumberValue());
                if (isNaN(maxBet)) return;
                maxPerMatch = parseFloat($(this).parents("tr:first").find("input[maxpermatch]").getNumberValue());
                if (isNaN(maxPerMatch)) return;
                if (currentVal > maxBet) {
                    job(this);
                    setMsg1(maxBet);
                    return;
                }
                if (currentVal > maxPerMatch) {
                    job(this);
                    setMsg11(maxPerMatch);
                    return;
                }
            }
            else if ($(this).attr("maxbet") != null && typeof $(this).attr("maxbet") != "undefined") {
                maxPerMatch = parseFloat($(this).parents("tr:first").find("input[maxpermatch]").getNumberValue());
                if (isNaN(maxPerMatch)) return;
                if (currentVal > maxPerMatch) {
                    job(this); setMsg6(maxPerMatch); return;
                }
            }

            if (typeof this.uplineVal == "undefined") {
                return;
            }

            var uplineMinBet, uplineMaxBet, uplineMaxPermatch;
            uplineMinBet = $(this).parents("tr:first").find("input[minbet]").get(0).uplineVal;
            uplineMaxBet = $(this).parents("tr:first").find("input[maxbet]").get(0).uplineVal;
            uplineMaxPermatch = $(this).parents("tr:first").find("input[maxpermatch]").get(0).uplineVal;
            if (uplineMinBet == 0 && uplineMaxBet == 0 && uplineMaxPermatch == 0) return; //valid case

            minBet = parseFloat($(this).parents("tr:first").find("input[minbet]").getNumberValue());
            if (isNaN(minBet)) minBet = 0;
            maxBet = parseFloat($(this).parents("tr:first").find("input[maxbet]").getNumberValue());
            if (isNaN(maxBet)) maxBet = 0;
            maxPerMatch = parseFloat($(this).parents("tr:first").find("input[maxpermatch]").getNumberValue());
            if (isNaN(maxPerMatch)) maxPerMatch = 0;
            if (minBet == 0 && maxBet == 0 && maxPerMatch == 0) return; //valid case

            if ($(this).attr("minbet") != null && typeof $(this).attr("minbet") != "undefined") {
                if (currentVal < this.uplineVal) { job(this); setMsg2(this.uplineVal); return; }
            }
            else if ($(this).attr("maxbet") != null && typeof $(this).attr("maxbet") != "undefined") {
                if (currentVal > this.uplineVal) { job(this); setMsg3(this.uplineVal); return; }
            }
            else if ($(this).attr("maxpermatch") != null && typeof $(this).attr("maxpermatch") != "undefined") {
                if (currentVal > this.uplineVal) { job(this); setMsg4(this.uplineVal); return; }
            }
            else if ($(this).attr("maxperball") != null && typeof $(this).attr("maxperball") != "undefined") {
                if (currentVal > this.uplineVal) { job(this); setMsg5(this.uplineVal); return; }
            }
        }
    });
    $("#divRacingContainer").find("input[type='text']").css("border-color", "").each(function () {
        var currentVal = parseFloat($(this).getNumberValue());
        if (valid) {
            if (common.isEmpty($(this).val()) || !common.isNumber($(this).getNumberValue()) || currentVal < 0 || currentVal > common.moneyMax) {
                job(this);
                return;
            }

            var minBet, maxBet, maxPerMatch;
            if ($(this).attr("minbet") != null && typeof $(this).attr("minbet") != "undefined") {
                maxBet = parseFloat($(this).parents("tr:first").find("input[maxbet]").getNumberValue());
                if (isNaN(maxBet)) return;
                maxPerMatch = parseFloat($(this).parents("tr:first").find("input[maxpermatch]").getNumberValue());
                if (isNaN(maxPerMatch)) return;
                if (currentVal > maxBet) {
                    job(this);
                    setMsg1(maxBet);
                    return;
                }
                if (currentVal > maxPerMatch) {
                    job(this);
                    setMsg11(maxPerMatch);
                    return;
                }
            }
            else if ($(this).attr("maxbet") != null && typeof $(this).attr("maxbet") != "undefined") {
                maxPerMatch = parseFloat($(this).parents("tr:first").find("input[maxpermatch]").getNumberValue());
                if (isNaN(maxPerMatch)) return;
                if (currentVal > maxPerMatch) {
                    job(this);
                    setMsg6(maxPerMatch);
                    return;
                }
            }

            if (typeof this.uplineVal == "undefined") {
                return;
            }

            var uplineMinBet, uplineMaxBet, uplineMaxPermatch;
            uplineMinBet = $(this).parents("tr:first").find("input[minbet]").get(0).uplineVal;
            uplineMaxBet = $(this).parents("tr:first").find("input[maxbet]").get(0).uplineVal;
            uplineMaxPermatch = $(this).parents("tr:first").find("input[maxpermatch]").get(0).uplineVal;
            if (uplineMinBet == 0 && uplineMaxBet == 0 && uplineMaxPermatch == 0) return; //valid case

            minBet = parseFloat($(this).parents("tr:first").find("input[minbet]").getNumberValue());
            if (isNaN(minBet)) minBet = 0;
            maxBet = parseFloat($(this).parents("tr:first").find("input[maxbet]").getNumberValue());
            if (isNaN(maxBet)) maxBet = 0;
            maxPerMatch = parseFloat($(this).parents("tr:first").find("input[maxpermatch]").getNumberValue());
            if (isNaN(maxPerMatch)) maxPerMatch = 0;
            if (minBet == 0 && maxBet == 0 && maxPerMatch == 0) return; //valid case

            if ($(this).attr("minbet") != null && typeof $(this).attr("minbet") != "undefined") {
                if (currentVal < this.uplineVal) { job(this); setMsg2(this.uplineVal); return; }
            }
            else if ($(this).attr("maxbet") != null && typeof $(this).attr("maxbet") != "undefined") {
                if (currentVal > this.uplineVal) { job(this); setMsg3(this.uplineVal); return; }
            }
            else if ($(this).attr("maxpermatch") != null && typeof $(this).attr("maxpermatch") != "undefined") {
                if (currentVal > this.uplineVal) { job(this); setMsg4(this.uplineVal); return; }
            }
        }
    });
    return valid;
};

common.reloadPage = function (url) {
    var timerId = setTimeout(function () {
        parent.gotoURL('centerFr', url);
        clearTimeout(timerId);
    }, 2000);
};

$(document).ready(function () {
    common.init();
});

function scoreConfirm_Cancel(type, sport) {
    jQuery("#errMsg_ScoreConfirm").hide();
    if (jQuery("#cbxInputDirectly").length) {
        jQuery("#cbxInputDirectly").get(0).checked = false;
    }
    if (sport == "soccer" || sport == "Common") {
        jQuery("#tbxScoreConfirm_homeHScore").val(jQuery("#tbxScoreConfirm_homeHScore").attr("beginvalue"));
        jQuery("#tbxScoreConfirm_awayHScore").val(jQuery("#tbxScoreConfirm_awayHScore").attr("beginvalue"));
        jQuery("#tbxScoreConfirm_homeFScore").val(jQuery("#tbxScoreConfirm_homeFScore").attr("beginvalue"));
        jQuery("#tbxScoreConfirm_awayFScore").val(jQuery("#tbxScoreConfirm_awayFScore").attr("beginvalue"));
        switch (type) {
            case 'refundhalftime':
                jQuery("#tbxScoreConfirm_homeHScore").removeAttr("disabled").focus();
                jQuery("#tbxScoreConfirm_awayHScore").removeAttr("disabled");
                break;
            case 'refundfulltime':
                jQuery("#tbxScoreConfirm_homeFScore").removeAttr("disabled").focus();
                jQuery("#tbxScoreConfirm_awayFScore").removeAttr("disabled");
                break;
            case 'refund':
                jQuery("#tbxScoreConfirm_homeHScore").removeAttr("disabled").focus();
                jQuery("#tbxScoreConfirm_awayHScore").removeAttr("disabled");
                jQuery("#tbxScoreConfirm_homeFScore").removeAttr("disabled");
                jQuery("#tbxScoreConfirm_awayFScore").removeAttr("disabled");
                break;
        }
    }
    else if (sport == "BasketballFootbal") {
        jQuery("#tbxScoreConfirm_homeHScore").val(jQuery("#tbxScoreConfirm_homeHScore").attr("beginvalue")).attr("disabled", "disabled");
        jQuery("#tbxScoreConfirm_awayHScore").val(jQuery("#tbxScoreConfirm_awayHScore").attr("beginvalue")).attr("disabled", "disabled");
        jQuery("#tbxScoreConfirm_homeFScore").val(jQuery("#tbxScoreConfirm_homeFScore").attr("beginvalue")).attr("disabled", "disabled");
        jQuery("#tbxScoreConfirm_awayFScore").val(jQuery("#tbxScoreConfirm_awayFScore").attr("beginvalue")).attr("disabled", "disabled");
        for (var i = 1; i <= 4; ++i) {
            jQuery("#value" + i + "qHome").removeAttr("disabled").val(jQuery("#value" + i + "qHome").attr("beginvalue"));
            jQuery("#value" + i + "qAway").removeAttr("disabled").val(jQuery("#value" + i + "qAway").attr("beginvalue"));
        }
        jQuery("#value1qHome").focus();
        jQuery("#valueOTHome").removeAttr("disabled").val(jQuery("#valueOTHome").attr("beginvalue"));
        jQuery("#valueOTAway").removeAttr("disabled").val(jQuery("#valueOTAway").attr("beginvalue"));
    }
    else if (sport == "Hockey") {
        jQuery("#tbxScoreConfirm_homeFScore").val(jQuery("#tbxScoreConfirm_homeFScore").attr("beginvalue")).attr("disabled", "disabled");
        jQuery("#tbxScoreConfirm_awayFScore").val(jQuery("#tbxScoreConfirm_awayFScore").attr("beginvalue")).attr("disabled", "disabled");
        for (var i = 1; i <= 3; ++i) {
            jQuery("#value" + i + "qHome").removeAttr("disabled").val(jQuery("#value" + i + "qHome").attr("beginvalue"));
            jQuery("#value" + i + "qAway").removeAttr("disabled").val(jQuery("#value" + i + "qAway").attr("beginvalue"));
        }
        jQuery("#value1qHome").focus();
        jQuery("#valueOTHome").removeAttr("disabled").val(jQuery("#valueOTHome").attr("beginvalue"));
        jQuery("#valueOTAway").removeAttr("disabled").val(jQuery("#valueOTAway").attr("beginvalue"));
    }
    else if (sport == "Tennis" || sport == "Badminton") {
        for (var i = 1; i <= 5; ++i) {
            jQuery("#value" + i + "qHome").removeAttr("disabled").val(jQuery("#value" + i + "qHome").attr("beginvalue"));
            jQuery("#value" + i + "qAway").removeAttr("disabled").val(jQuery("#value" + i + "qAway").attr("beginvalue"));
        }
        jQuery("#value1qHome").focus();
        jQuery("#tbxScoreConfirm_homeFScore").val(jQuery("#tbxScoreConfirm_homeFScore").attr("beginvalue")).attr("disabled", "disabled");
        jQuery("#tbxScoreConfirm_awayFScore").val(jQuery("#tbxScoreConfirm_awayFScore").attr("beginvalue")).attr("disabled", "disabled");
        jQuery("#valueFSHome").val(jQuery("#valueFSHome").attr("beginvalue"));
        jQuery("#valueFSAway").val(jQuery("#valueFSAway").attr("beginvalue"));
    }
    else if (sport == "Billiard") {
        jQuery("#tbxScoreConfirm_homeFScore").val(jQuery("#tbxScoreConfirm_homeFScore").attr("beginvalue")).removeAttr("disabled").focus();
        jQuery("#tbxScoreConfirm_awayFScore").val(jQuery("#tbxScoreConfirm_awayFScore").attr("beginvalue")).removeAttr("disabled");
    }
    else if (sport == "Baseball") {
        jQuery("#tbxScoreConfirm_homeHScore").val(jQuery("#tbxScoreConfirm_homeHScore").attr("beginvalue")).attr("disabled", "disabled");
        jQuery("#tbxScoreConfirm_awayHScore").val(jQuery("#tbxScoreConfirm_awayHScore").attr("beginvalue")).attr("disabled", "disabled");
        jQuery("#tbxScoreConfirm_homeFScore").val(jQuery("#tbxScoreConfirm_homeFScore").attr("beginvalue")).attr("disabled", "disabled");
        jQuery("#tbxScoreConfirm_awayFScore").val(jQuery("#tbxScoreConfirm_awayFScore").attr("beginvalue")).attr("disabled", "disabled");
        for (var i = 1; i <= 9; ++i) {
            jQuery("#value" + i + "qHome").removeAttr("disabled").val(jQuery("#value" + i + "qHome").attr("beginvalue"));
            jQuery("#value" + i + "qAway").removeAttr("disabled").val(jQuery("#value" + i + "qAway").attr("beginvalue"));
        }
        jQuery("#value1qHome").focus();
        jQuery("#valueEIHome").removeAttr("disabled").val(jQuery("#valueEIHome").attr("beginvalue"));
        jQuery("#valueEIAway").removeAttr("disabled").val(jQuery("#valueEIAway").attr("beginvalue"));
    }
};

function scoreConfirm_VerifyScore() {
    var arrTextBoxs = jQuery("#divScoreConfirm").find("input[type='text']");
    var emptyCounter = 0;
    arrTextBoxs.each(function () {
        if (jQuery(this).val() == "") emptyCounter++;
    })
    if (emptyCounter == arrTextBoxs.length) return false;
    return true;
};

function scoreConfirm_Proceed(type, sport) {
    if (!scoreConfirm_VerifyScore()) {
        jQuery("#errMsg_ScoreConfirm").show();
        return false;
    }
    jQuery("#errMsg_ScoreConfirm").hide();
    jQuery("#hiddenScoreConfirm_homeHScore").val(jQuery("#tbxScoreConfirm_homeHScore").val());
    jQuery("#hiddenScoreConfirm_awayHScore").val(jQuery("#tbxScoreConfirm_awayHScore").val());
    jQuery("#hiddenScoreConfirm_homeFScore").val(jQuery("#tbxScoreConfirm_homeFScore").val());
    jQuery("#hiddenScoreConfirm_awayFScore").val(jQuery("#tbxScoreConfirm_awayFScore").val());

    var proceed_BasketballFootbal = function () {
        //this section commented: verify empty, if not valid -> do not submit
        /*var haveOneNotEmpty = false;
        for (var i = 1; i <= 4; ++i) {
        if (jQuery("#value" + i + "qHome").val() != "" && jQuery("#value" + i + "qHome").val() != null) { haveOneNotEmpty = true; break; }
        if (jQuery("#value" + i + "qAway").val() != "" && jQuery("#value" + i + "qAway").val() != null) { haveOneNotEmpty = true; break; }
        }
        if (jQuery("#valueOTHome").val() != "" && jQuery("#valueOTHome").val() != null) haveOneNotEmpty = true;
        if (jQuery("#valueOTAway").val() != "" && jQuery("#valueOTAway").val() != null) haveOneNotEmpty = true;
        if (jQuery("#tbxScoreConfirm_homeHScore").val() != "" && jQuery("#tbxScoreConfirm_homeHScore").val() != null) haveOneNotEmpty = true;
        if (jQuery("#tbxScoreConfirm_awayHScore").val() != "" && jQuery("#tbxScoreConfirm_awayHScore").val() != null) haveOneNotEmpty = true;
        if (jQuery("#tbxScoreConfirm_homeFScore").val() != "" && jQuery("#tbxScoreConfirm_homeFScore").val() != null) haveOneNotEmpty = true;
        if (jQuery("#tbxScoreConfirm_awayFScore").val() != "" && jQuery("#tbxScoreConfirm_awayFScore").val() != null) haveOneNotEmpty = true;
        if (!haveOneNotEmpty) {
        return false;
        }*/
        if (!jQuery("#cbxInputDirectly").is(":checked")) {
            var _1q = "", _2q = "", _3q = "", _4q = "", _ot = "";
            if (jQuery("#value1qHome").val() == "") _1q = "0-";
            else _1q = jQuery("#value1qHome").val() + "-";
            if (jQuery("#value1qAway").val() == "") _1q += "0";
            else _1q += jQuery("#value1qAway").val();


            if (jQuery("#value2qHome").val() == "") _2q = "0-";
            else _2q = jQuery("#value2qHome").val() + "-";
            if (jQuery("#value2qAway").val() == "") _2q += "0";
            else _2q += jQuery("#value2qAway").val();

            if (jQuery("#value3qHome").val() == "") _3q = "0-";
            else _3q = jQuery("#value3qHome").val() + "-";
            if (jQuery("#value3qAway").val() == "") _3q += "0";
            else _3q += jQuery("#value3qAway").val();

            if (jQuery("#value4qHome").val() == "" || jQuery("#value4qHome").val() == null) _4q = "0-";
            else _4q = jQuery("#value4qHome").val() + "-";
            if (jQuery("#value4qAway").val() == "" || jQuery("#value4qAway").val() == null) _4q += "0";
            else _4q += jQuery("#value4qAway").val();

            if (jQuery("#valueOTHome").val() == "") _ot = "0-";
            else _ot = jQuery("#valueOTHome").val() + "-";
            if (jQuery("#valueOTAway").val() == "") _ot += "0";
            else _ot += jQuery("#valueOTAway").val();

            var betstatus = _1q + "," + _2q + "," + _3q;
            if (sport == "BasketballFootbal") betstatus += "," + _4q;
            if (_ot != "") betstatus += "," + _ot;
            jQuery("#hiddenBetStatus_ConfirmScore").val(betstatus);
        }
        else {
            jQuery("#hiddenBetStatus_ConfirmScore").val("");
        }
    };

    var proceed_Tennis = function () {
        if (!jQuery("#cbxInputDirectly").is(":checked")) {
            var _1q = "", _2q = "", _3q = "", _4q = "", _5q = "";
            if (jQuery("#value1qHome").val() == "") _1q = "0-";
            else _1q = jQuery("#value1qHome").val() + "-";
            if (jQuery("#value1qAway").val() == "") _1q += "0";
            else _1q += jQuery("#value1qAway").val();


            if (jQuery("#value2qHome").val() == "") _2q = "0-";
            else _2q = jQuery("#value2qHome").val() + "-";
            if (jQuery("#value2qAway").val() == "") _2q += "0";
            else _2q += jQuery("#value2qAway").val();

            if (jQuery("#value3qHome").val() == "") _3q = "0-";
            else _3q = jQuery("#value3qHome").val() + "-";
            if (jQuery("#value3qAway").val() == "") _3q += "0";
            else _3q += jQuery("#value3qAway").val();

            if (jQuery("#value4qHome").val() == "") _4q = "0-";
            else _4q = jQuery("#value4qHome").val() + "-";
            if (jQuery("#value4qAway").val() == "") _4q += "0";
            else _4q += jQuery("#value4qAway").val();

            if (jQuery("#value5qHome").val() == "") _5q = "0-";
            else _5q = jQuery("#value5qHome").val() + "-";
            if (jQuery("#value5qAway").val() == "") _5q += "0";
            else _5q += jQuery("#value5qAway").val();

            var betstatus = _1q + "," + _2q + "," + _3q + "," + _4q + "," + _5q;
            jQuery("#hiddenBetStatus_ConfirmScore").val(betstatus);
        }
        else {
            jQuery("#hiddenBetStatus_ConfirmScore").val("");
        }
    };
    var proceed_Baseball = function () {
        if (!jQuery("#cbxInputDirectly").is(":checked")) {
            var _1q = "", _2q = "", _3q = "", _4q = "", _5q = "", _6q = "", _7q = "", _8q = "", _9q = "", _ei = "";
            if (jQuery("#value1qHome").val() == "") _1q = "0-";
            else _1q = jQuery("#value1qHome").val() + "-";
            if (jQuery("#value1qAway").val() == "") _1q += "0";
            else _1q += jQuery("#value1qAway").val();


            if (jQuery("#value2qHome").val() == "") _2q = "0-";
            else _2q = jQuery("#value2qHome").val() + "-";
            if (jQuery("#value2qAway").val() == "") _2q += "0";
            else _2q += jQuery("#value2qAway").val();

            if (jQuery("#value3qHome").val() == "") _3q = "0-";
            else _3q = jQuery("#value3qHome").val() + "-";
            if (jQuery("#value3qAway").val() == "") _3q += "0";
            else _3q += jQuery("#value3qAway").val();

            if (jQuery("#value4qHome").val() == "" || jQuery("#value4qHome").val() == null) _4q = "0-";
            else _4q = jQuery("#value4qHome").val() + "-";
            if (jQuery("#value4qAway").val() == "" || jQuery("#value4qAway").val() == null) _4q += "0";
            else _4q += jQuery("#value4qAway").val();

            if (jQuery("#value5qHome").val() == "" || jQuery("#value5qHome").val() == null) _5q = "0-";
            else _5q = jQuery("#value5qHome").val() + "-";
            if (jQuery("#value5qAway").val() == "" || jQuery("#value5qAway").val() == null) _5q += "0";
            else _5q += jQuery("#value5qAway").val();

            if (jQuery("#value6qHome").val() == "" || jQuery("#value6qHome").val() == null) _6q = "0-";
            else _6q = jQuery("#value6qHome").val() + "-";
            if (jQuery("#value6qAway").val() == "" || jQuery("#value6qAway").val() == null) _6q += "0";
            else _6q += jQuery("#value6qAway").val();

            if (jQuery("#value7qHome").val() == "" || jQuery("#value7qHome").val() == null) _7q = "0-";
            else _7q = jQuery("#value7qHome").val() + "-";
            if (jQuery("#value7qAway").val() == "" || jQuery("#value7qAway").val() == null) _7q += "0";
            else _7q += jQuery("#value7qAway").val();

            if (jQuery("#value8qHome").val() == "" || jQuery("#value8qHome").val() == null) _8q = "0-";
            else _8q = jQuery("#value8qHome").val() + "-";
            if (jQuery("#value8qAway").val() == "" || jQuery("#value8qAway").val() == null) _8q += "0";
            else _8q += jQuery("#value8qAway").val();

            if (jQuery("#value9qHome").val() == "" || jQuery("#value9qHome").val() == null) _9q = "0-";
            else _9q = jQuery("#value9qHome").val() + "-";
            if (jQuery("#value9qAway").val() == "" || jQuery("#value9qAway").val() == null) _9q += "0";
            else _9q += jQuery("#value9qAway").val();

            if (jQuery("#valueEIHome").val() == "") _ei = "0-";
            else _ei = jQuery("#valueEIHome").val() + "-";
            if (jQuery("#valueEIAway").val() == "") _ei += "0";
            else _ei += jQuery("#valueEIAway").val();

            var betstatus = _1q + "," + _2q + "," + _3q + "," + _4q + "," + _5q + "," + _6q + "," + _7q + "," + _8q + "," + _9q;
            if (_ei != "") betstatus += "," + _ei;
            jQuery("#hiddenBetStatus_ConfirmScore").val(betstatus);
        }
        else {
            jQuery("#hiddenBetStatus_ConfirmScore").val("");
        }
    };
    if (sport == "BasketballFootbal" || sport == "Hockey") {
        proceed_BasketballFootbal();
    }
    else if (sport == "Tennis" || sport == "Badminton") {
        proceed_Tennis();
    }
    else if (sport == "Baseball") {
        proceed_Baseball();
    }
    return true;
};

var scoreConfirm_Inited = false;
function scoreConfirm(type) {
    BasketballFootbal_RefundType = type;
    jQuery("#errMsg_ScoreConfirm").hide();
    var doAsNormal = function () {
        switch (type) {
            case 'refund':
                if (!confirm("Are you sure you want to REFUND/CANCEL\nThis function is only in use when the match cancel?")) {
                    try { parent.parent.jQuery.unblockUI(); } catch (e) { }
                    return;
                }
                break;
            case 'refundfulltime':
                if (!confirm("Are you sure you want to REFUND FULLTIME\nThis function is only in use when the match cancel?")) {
                    try { parent.parent.jQuery.unblockUI(); } catch (e) { }
                    return;
                }
                break;
            case 'refundhalftime':
                if (!confirm("Are you sure you want to REFUND HALFTIME\n this match?")) {
                    try { parent.parent.jQuery.unblockUI(); } catch (e) { }
                    return;
                }
                break;
        }
        jQuery("#divScoreConfirm").dialog("close");
        DoCalculate_Submit(type);
    };

    if (jQuery("#chkIsSettleDeterminedTickets").is(":checked")) {
        var sport = jQuery("#hiddenScoreConfirm_Sport").val();
        var _width = 400;

        var reset_Soccer = function () {
            _width = 400;
            jQuery("#tbxScoreConfirm_homeHScore").val(jQuery("#tbxScoreConfirm_homeHScore").attr("beginvalue")).attr("disabled", "disabled");
            jQuery("#tbxScoreConfirm_awayHScore").val(jQuery("#tbxScoreConfirm_awayHScore").attr("beginvalue")).attr("disabled", "disabled");
            jQuery("#tbxScoreConfirm_homeFScore").val(jQuery("#tbxScoreConfirm_homeFScore").attr("beginvalue")).attr("disabled", "disabled");
            jQuery("#tbxScoreConfirm_awayFScore").val(jQuery("#tbxScoreConfirm_awayFScore").attr("beginvalue")).attr("disabled", "disabled");
        };

        var reset_BasketballFootbal = function () {
            _width = 600;
            for (var i = 1; i <= 4; ++i) {
                jQuery("#value" + i + "qHome").attr("disabled", "disabled").val(jQuery("#value" + i + "qHome").attr("beginvalue"));
                jQuery("#value" + i + "qAway").attr("disabled", "disabled").val(jQuery("#value" + i + "qAway").attr("beginvalue"));
            }
            jQuery("#valueOTHome").attr("disabled", "disabled").val(jQuery("#valueOTHome").attr("beginvalue"));
            jQuery("#valueOTAway").attr("disabled", "disabled").val(jQuery("#valueOTAway").attr("beginvalue"));

            jQuery("#tbxScoreConfirm_homeHScore").attr("disabled", "disabled").val(jQuery("#tbxScoreConfirm_homeHScore").attr("beginvalue"));
            jQuery("#tbxScoreConfirm_awayHScore").attr("disabled", "disabled").val(jQuery("#tbxScoreConfirm_awayHScore").attr("beginvalue"));
            jQuery("#tbxScoreConfirm_homeFScore").attr("disabled", "disabled").val(jQuery("#tbxScoreConfirm_homeFScore").attr("beginvalue"));
            jQuery("#tbxScoreConfirm_awayFScore").attr("disabled", "disabled").val(jQuery("#tbxScoreConfirm_awayFScore").attr("beginvalue"));
        };

        var reset_Hockey = function () {
            _width = 600;
            for (var i = 1; i <= 3; ++i) {
                jQuery("#value" + i + "qHome").attr("disabled", "disabled").val(jQuery("#value" + i + "qHome").attr("beginvalue"));
                jQuery("#value" + i + "qAway").attr("disabled", "disabled").val(jQuery("#value" + i + "qAway").attr("beginvalue"));
            }
            jQuery("#valueOTHome").attr("disabled", "disabled").val(jQuery("#valueOTHome").attr("beginvalue"));
            jQuery("#valueOTAway").attr("disabled", "disabled").val(jQuery("#valueOTAway").attr("beginvalue"));

            jQuery("#tbxScoreConfirm_homeFScore").attr("disabled", "disabled").val(jQuery("#tbxScoreConfirm_homeFScore").attr("beginvalue"));
            jQuery("#tbxScoreConfirm_awayFScore").attr("disabled", "disabled").val(jQuery("#tbxScoreConfirm_awayFScore").attr("beginvalue"));
        };

        var reset_Tennis = function () {
            _width = 600;
            for (var i = 1; i <= 5; ++i) {
                jQuery("#value" + i + "qHome").attr("disabled", "disabled").val(jQuery("#value" + i + "qHome").attr("beginvalue"));
                jQuery("#value" + i + "qAway").attr("disabled", "disabled").val(jQuery("#value" + i + "qAway").attr("beginvalue"));
            }
            jQuery("#tbxScoreConfirm_homeFScore").val(jQuery("#tbxScoreConfirm_homeFScore").attr("beginvalue")).attr("disabled", "disabled");
            jQuery("#tbxScoreConfirm_awayFScore").val(jQuery("#tbxScoreConfirm_awayFScore").attr("beginvalue")).attr("disabled", "disabled");
            jQuery("#valueFSHome").val(jQuery("#valueFSHome").attr("beginvalue"));
            jQuery("#valueFSAway").val(jQuery("#valueFSAway").attr("beginvalue"));
        };

        var reset_Billiard = function () {
            jQuery("#tbxScoreConfirm_homeFScore").val(jQuery("#tbxScoreConfirm_homeFScore").attr("beginvalue")).attr("disabled", "disabled");
            jQuery("#tbxScoreConfirm_awayFScore").val(jQuery("#tbxScoreConfirm_awayFScore").attr("beginvalue")).attr("disabled", "disabled");
        };

        if (jQuery("#cbxInputDirectly").length) {
            jQuery("#cbxInputDirectly").get(0).checked = false;
        }

        var reset_Baseball = function () {
            _width = 600;
            jQuery("#tbxScoreConfirm_homeHScore").val(jQuery("#tbxScoreConfirm_homeHScore").attr("beginvalue")).attr("disabled", "disabled");
            jQuery("#tbxScoreConfirm_awayHScore").val(jQuery("#tbxScoreConfirm_awayHScore").attr("beginvalue")).attr("disabled", "disabled");
            jQuery("#tbxScoreConfirm_homeFScore").val(jQuery("#tbxScoreConfirm_homeFScore").attr("beginvalue")).attr("disabled", "disabled");
            jQuery("#tbxScoreConfirm_awayFScore").val(jQuery("#tbxScoreConfirm_awayFScore").attr("beginvalue")).attr("disabled", "disabled");
            for (var i = 1; i <= 9; ++i) {
                jQuery("#value" + i + "qHome").val(jQuery("#value" + i + "qHome").attr("beginvalue")).attr("disabled", "disabled");
                jQuery("#value" + i + "qAway").val(jQuery("#value" + i + "qAway").attr("beginvalue")).attr("disabled", "disabled");
            }
            jQuery("#value1qHome").focus();
            jQuery("#valueEIHome").val(jQuery("#valueEIHome").attr("beginvalue")).attr("disabled", "disabled");
            jQuery("#valueEIAway").val(jQuery("#valueEIAway").attr("beginvalue")).attr("disabled", "disabled");
        };

        switch (sport) {
            case "soccer":
            case "Common":
                reset_Soccer();
                break;
            case "BasketballFootbal":
                reset_BasketballFootbal();
                break;
            case "Hockey":
                reset_Hockey();
                break;
            case "Tennis":
            case "Badminton":
                reset_Tennis();
                break;
            case "Billiard":
                reset_Billiard();
                break;
            case "Baseball":
                reset_Baseball();
                break;
        }

        jQuery("#divScoreConfirm").dialog({ title: "Score Confirmation", modal: true, width: _width, resizable: false });

        jQuery("#btnScoreConfirmProceed").unbind("click").click(function () {
            if (scoreConfirm_Proceed(type, sport)) {
                doAsNormal();
            }
        });

        jQuery("#btnScoreConfirmCancel").unbind("click").click(function () {
            scoreConfirm_Cancel(type, sport);
        });

        if (!scoreConfirm_Inited) {
            var init_Soccer = function () {
                jQuery("#tbxScoreConfirm_homeHScore").ForceDecimalOnly();
                jQuery("#tbxScoreConfirm_awayHScore").ForceDecimalOnly();
                jQuery("#tbxScoreConfirm_homeFScore").ForceDecimalOnly();
                jQuery("#tbxScoreConfirm_awayFScore").ForceDecimalOnly();
            };
            var init_BasketballFootbal = function () {
                var changeScoreHome = function (e) {
                    var htScore = 0;
                    if (jQuery("#value1qHome").val() != "") htScore = parseInt(jQuery("#value1qHome").val());
                    if (jQuery("#value2qHome").val() != "") htScore += parseInt(jQuery("#value2qHome").val());

                    var ftScore = 0;
                    if (jQuery("#value1qHome").val() != "") ftScore = parseInt(jQuery("#value1qHome").val());
                    if (jQuery("#value2qHome").val() != "") ftScore += parseInt(jQuery("#value2qHome").val());
                    if (jQuery("#value3qHome").val() != "") ftScore += parseInt(jQuery("#value3qHome").val());
                    if (jQuery("#value4qHome").val() != "") ftScore += parseInt(jQuery("#value4qHome").val());
                    if (jQuery("#valueOTHome").val() != "") ftScore += parseInt(jQuery("#valueOTHome").val());

                    if (jQuery("#value1qHome").val() == "" && jQuery("#value2qHome").val() == "") {
                        jQuery("#tbxScoreConfirm_homeHScore").val("");
                    }
                    else {
                        jQuery("#tbxScoreConfirm_homeHScore").val(htScore);
                    }

                    if (jQuery("#value1qHome").val() == "" && jQuery("#value2qHome").val() == "" && jQuery("#value3qHome").val() == ""
                        && jQuery("#value4qHome").val() == "" && jQuery("#valueOTHome").val() == "") {
                        jQuery("#tbxScoreConfirm_homeFScore").val("");
                    }
                    else {
                        jQuery("#tbxScoreConfirm_homeFScore").val(ftScore);
                    }
                };
                var changeScoreAway = function (e) {
                    var htScore = 0;
                    if (jQuery("#value1qAway").val() != "") htScore = parseInt(jQuery("#value1qAway").val());
                    if (jQuery("#value2qAway").val() != "") htScore += parseInt(jQuery("#value2qAway").val());

                    var ftScore = 0;
                    if (jQuery("#value1qAway").val() != "") ftScore = parseInt(jQuery("#value1qAway").val());
                    if (jQuery("#value2qAway").val() != "") ftScore += parseInt(jQuery("#value2qAway").val());
                    if (jQuery("#value3qAway").val() != "") ftScore += parseInt(jQuery("#value3qAway").val());
                    if (jQuery("#value4qAway").val() != "") ftScore += parseInt(jQuery("#value4qAway").val());
                    if (jQuery("#valueOTAway").val() != "") ftScore += parseInt(jQuery("#valueOTAway").val());

                    if (jQuery("#value1qAway").val() == "" && jQuery("#value2qAway").val() == "") {
                        jQuery("#tbxScoreConfirm_awayHScore").val("");
                    }
                    else {
                        jQuery("#tbxScoreConfirm_awayHScore").val(htScore);
                    }

                    if (jQuery("#value1qAway").val() == "" && jQuery("#value2qAway").val() == "" && jQuery("#value3qAway").val() == ""
                        && jQuery("#value4qAway").val() == "" && jQuery("#valueOTAway").val() == "") {
                        jQuery("#tbxScoreConfirm_awayFScore").val("");
                    }
                    else {
                        jQuery("#tbxScoreConfirm_awayFScore").val(ftScore);
                    }
                };
                jQuery("#tbxScoreConfirm_homeHScore").ForceDecimalOnly();
                jQuery("#tbxScoreConfirm_awayHScore").ForceDecimalOnly();
                jQuery("#tbxScoreConfirm_homeFScore").ForceDecimalOnly();
                jQuery("#tbxScoreConfirm_awayFScore").ForceDecimalOnly();
                for (var i = 1; i <= 4; ++i) {
                    jQuery("#value" + i + "qHome").ForceDecimalOnly().keyup(function (e) {
                        changeScoreHome(e);
                    });
                    jQuery("#value" + i + "qAway").ForceDecimalOnly().keyup(function (e) {
                        changeScoreAway(e);
                    });
                }
                jQuery("#valueOTHome").ForceDecimalOnly().keyup(function (e) {
                    changeScoreHome(e);
                });
                jQuery("#valueOTAway").ForceDecimalOnly().keyup(function (e) {
                    changeScoreAway(e);
                });
            };
            var init_Hockey = function () {
                var changeScoreHome = function (e) {
                    var ftScore = 0;
                    if (jQuery("#value1qHome").val() != "") ftScore = parseInt(jQuery("#value1qHome").val());
                    if (jQuery("#value2qHome").val() != "") ftScore += parseInt(jQuery("#value2qHome").val());
                    if (jQuery("#value3qHome").val() != "") ftScore += parseInt(jQuery("#value3qHome").val());
                    if (jQuery("#valueOTHome").val() != "") ftScore += parseInt(jQuery("#valueOTHome").val());

                    if (jQuery("#value1qHome").val() == "" && jQuery("#value2qHome").val() == "" && jQuery("#value3qHome").val() == ""
                        && jQuery("#valueOTHome").val() == "") {
                        jQuery("#tbxScoreConfirm_homeFScore").val("");
                    }
                    else {
                        jQuery("#tbxScoreConfirm_homeFScore").val(ftScore);
                    }
                };
                var changeScoreAway = function (e) {
                    var ftScore = 0;
                    if (jQuery("#value1qAway").val() != "") ftScore = parseInt(jQuery("#value1qAway").val());
                    if (jQuery("#value2qAway").val() != "") ftScore += parseInt(jQuery("#value2qAway").val());
                    if (jQuery("#value3qAway").val() != "") ftScore += parseInt(jQuery("#value3qAway").val());
                    if (jQuery("#valueOTAway").val() != "") ftScore += parseInt(jQuery("#valueOTAway").val());

                    if (jQuery("#value1qAway").val() == "" && jQuery("#value2qAway").val() == "" && jQuery("#value3qAway").val() == ""
                        && jQuery("#valueOTAway").val() == "") {
                        jQuery("#tbxScoreConfirm_awayFScore").val("");
                    }
                    else {
                        jQuery("#tbxScoreConfirm_awayFScore").val(ftScore);
                    }
                };
                jQuery("#tbxScoreConfirm_homeFScore").ForceDecimalOnly();
                jQuery("#tbxScoreConfirm_awayFScore").ForceDecimalOnly();
                for (var i = 1; i <= 3; ++i) {
                    jQuery("#value" + i + "qHome").ForceDecimalOnly().keyup(function (e) {
                        changeScoreHome(e);
                    });
                    jQuery("#value" + i + "qAway").ForceDecimalOnly().keyup(function (e) {
                        changeScoreAway(e);
                    });
                }
                jQuery("#valueOTHome").ForceDecimalOnly().keyup(function (e) {
                    changeScoreHome(e);
                });
                jQuery("#valueOTAway").ForceDecimalOnly().keyup(function (e) {
                    changeScoreAway(e);
                });
            };
            var init_Tennis = function () {
                var changeScoreHome = function (e) {
                    //Total Games
                    var ftScore = 0;
                    if (jQuery("#value1qHome").val() != "" && jQuery("#value1qHome").val() != null) ftScore = parseInt(jQuery("#value1qHome").val());
                    if (jQuery("#value2qHome").val() != "" && jQuery("#value2qHome").val() != null) ftScore += parseInt(jQuery("#value2qHome").val());
                    if (jQuery("#value3qHome").val() != "" && jQuery("#value3qHome").val() != null) ftScore += parseInt(jQuery("#value3qHome").val());
                    if (jQuery("#value4qHome").val() != "" && jQuery("#value4qHome").val() != null) ftScore += parseInt(jQuery("#value4qHome").val());
                    if (jQuery("#value5qHome").val() != "" && jQuery("#value5qHome").val() != null) ftScore += parseInt(jQuery("#value5qHome").val());

                    if (jQuery("#value1qHome").val() == "" && jQuery("#value2qHome").val() == ""
                        && jQuery("#value3qHome").val() == "" && jQuery("#value4qHome").val() == "" && jQuery("#value5qHome").val() == "") {
                        jQuery("#valueFSHome").val("");
                    }
                    else {
                        jQuery("#valueFSHome").val(ftScore);
                    }
                };
                var changeScoreAway = function (e) {
                    //Total Games
                    var ftScore = 0;
                    if (jQuery("#value1qAway").val() != "" && jQuery("#value1qAway").val() != null) ftScore = parseInt(jQuery("#value1qAway").val());
                    if (jQuery("#value2qAway").val() != "" && jQuery("#value2qAway").val() != null) ftScore += parseInt(jQuery("#value2qAway").val());
                    if (jQuery("#value3qAway").val() != "" && jQuery("#value3qAway").val() != null) ftScore += parseInt(jQuery("#value3qAway").val());
                    if (jQuery("#value4qAway").val() != "" && jQuery("#value4qAway").val() != null) ftScore += parseInt(jQuery("#value4qAway").val());
                    if (jQuery("#value5qAway").val() != "" && jQuery("#value5qAway").val() != null) ftScore += parseInt(jQuery("#value5qAway").val());

                    if (jQuery("#value1qAway").val() == "" && jQuery("#value2qAway").val() == ""
                        && jQuery("#value3qAway").val() == "" && jQuery("#value4qAway").val() == "" && jQuery("#value5qAway").val() == "") {
                        jQuery("#valueFSAway").val("");
                    }
                    else {
                        jQuery("#valueFSAway").val(ftScore);
                    }
                };
                var updateTotalSet = function () {
                    //Total Set Home
                    var totalSet = 0;
                    if (
                    (jQuery("#value1qAway").val() == "" || parseInt(jQuery("#value1qHome").val()) > parseInt(jQuery("#value1qAway").val()))
                    && jQuery("#value1qHome").val() != "") totalSet += 1;
                    if (
                    (jQuery("#value2qAway").val() == "" || parseInt(jQuery("#value2qHome").val()) > parseInt(jQuery("#value2qAway").val()))
                    && jQuery("#value2qHome").val() != "") totalSet += 1;
                    if (
                    (jQuery("#value3qAway").val() == "" || parseInt(jQuery("#value3qHome").val()) > parseInt(jQuery("#value3qAway").val()))
                    && jQuery("#value3qHome").val() != "") totalSet += 1;
                    if (
                    (jQuery("#value4qAway").val() == "" || parseInt(jQuery("#value4qHome").val()) > parseInt(jQuery("#value4qAway").val()))
                    && jQuery("#value4qHome").val() != "") totalSet += 1;
                    if (
                    (jQuery("#value5qAway").val() == "" || parseInt(jQuery("#value5qHome").val()) > parseInt(jQuery("#value5qAway").val()))
                    && jQuery("#value5qHome").val() != "") totalSet += 1;

                    if (jQuery("#value1qHome").val() == "" && jQuery("#value2qHome").val() == ""
                        && jQuery("#value3qHome").val() == "" && jQuery("#value4qHome").val() == "" && jQuery("#value5qHome").val() == "") {
                        jQuery("#tbxScoreConfirm_homeFScore").val("");
                    }
                    else {
                        jQuery("#tbxScoreConfirm_homeFScore").val(totalSet);
                    }

                    //Total Set Away
                    var totalSet = 0;
                    if (
                    (jQuery("#value1qHome").val() == "" || parseInt(jQuery("#value1qAway").val()) > parseInt(jQuery("#value1qHome").val()))
                    && jQuery("#value1qAway").val() != "") totalSet += 1;
                    if (
                    (jQuery("#value2qHome").val() == "" || parseInt(jQuery("#value2qAway").val()) > parseInt(jQuery("#value2qHome").val()))
                    && jQuery("#value2qAway").val() != "") totalSet += 1;
                    if (
                    (jQuery("#value3qHome").val() == "" || parseInt(jQuery("#value3qAway").val()) > parseInt(jQuery("#value3qHome").val()))
                    && jQuery("#value3qAway").val() != "") totalSet += 1;
                    if (
                    (jQuery("#value4qHome").val() == "" || parseInt(jQuery("#value4qAway").val()) > parseInt(jQuery("#value4qHome").val()))
                    && jQuery("#value4qAway").val() != "") totalSet += 1;
                    if (
                    (jQuery("#value5qHome").val() == "" || parseInt(jQuery("#value5qAway").val()) > parseInt(jQuery("#value5qHome").val()))
                    && jQuery("#value5qAway").val() != "") totalSet += 1;

                    if (jQuery("#value1qAway").val() == "" && jQuery("#value2qAway").val() == ""
                        && jQuery("#value3qAway").val() == "" && jQuery("#value4qAway").val() == "" && jQuery("#value5qAway").val() == "") {
                        jQuery("#tbxScoreConfirm_awayFScore").val("");
                    }
                    else {
                        jQuery("#tbxScoreConfirm_awayFScore").val(totalSet);
                    }
                };
                jQuery("#valueFSHome").ForceDecimalOnly();
                jQuery("#valueFSAway").ForceDecimalOnly();
                for (var i = 1; i <= 5; ++i) {
                    jQuery("#value" + i + "qHome").ForceDecimalOnly().keyup(function (e) {
                        changeScoreHome(e);
                        updateTotalSet();
                    });
                    jQuery("#value" + i + "qAway").ForceDecimalOnly().keyup(function (e) {
                        changeScoreAway(e);
                        updateTotalSet();
                    });
                }
            };
            var init_Billiard = function () {
                jQuery("#tbxScoreConfirm_homeFScore").ForceDecimalOnly();
                jQuery("#tbxScoreConfirm_awayFScore").ForceDecimalOnly();
            };
            var init_Baseball = function () {
                var changeScoreHome = function (e) {
                    var htScore = 0;
                    if (jQuery("#value1qHome").val() != "") htScore = parseInt(jQuery("#value1qHome").val());
                    if (jQuery("#value2qHome").val() != "") htScore += parseInt(jQuery("#value2qHome").val());
                    if (jQuery("#value3qHome").val() != "") htScore += parseInt(jQuery("#value3qHome").val());
                    if (jQuery("#value4qHome").val() != "") htScore += parseInt(jQuery("#value4qHome").val());
                    if (jQuery("#value5qHome").val() != "") htScore += parseInt(jQuery("#value5qHome").val());

                    var ftScore = 0;
                    if (jQuery("#value1qHome").val() != "") ftScore = parseInt(jQuery("#value1qHome").val());
                    if (jQuery("#value2qHome").val() != "") ftScore += parseInt(jQuery("#value2qHome").val());
                    if (jQuery("#value3qHome").val() != "") ftScore += parseInt(jQuery("#value3qHome").val());
                    if (jQuery("#value4qHome").val() != "") ftScore += parseInt(jQuery("#value4qHome").val());
                    if (jQuery("#value5qHome").val() != "") ftScore += parseInt(jQuery("#value5qHome").val());
                    if (jQuery("#value6qHome").val() != "") ftScore += parseInt(jQuery("#value6qHome").val());
                    if (jQuery("#value7qHome").val() != "") ftScore += parseInt(jQuery("#value7qHome").val());
                    if (jQuery("#value8qHome").val() != "") ftScore += parseInt(jQuery("#value8qHome").val());
                    if (jQuery("#value9qHome").val() != "") ftScore += parseInt(jQuery("#value9qHome").val());
                    if (jQuery("#valueEIHome").val() != "") ftScore += parseInt(jQuery("#valueEIHome").val());

                    if (jQuery("#value1qHome").val() == "" && jQuery("#value2qHome").val() == "" && jQuery("#value3qHome").val() == ""
                    && jQuery("#value4qHome").val() == "" && jQuery("#value5qHome").val() == "") {
                        jQuery("#tbxScoreConfirm_homeHScore").val("");
                    }
                    else {
                        jQuery("#tbxScoreConfirm_homeHScore").val(htScore);
                    }

                    if (jQuery("#value1qHome").val() == "" && jQuery("#value2qHome").val() == ""
                        && jQuery("#value3qHome").val() == "" && jQuery("#valueOTHome").val() == ""
                        && jQuery("#value4qHome").val() == "" && jQuery("#value5qHome").val() == ""
                        && jQuery("#value6qHome").val() == "" && jQuery("#value7qHome").val() == ""
                        && jQuery("#value8qHome").val() == "" && jQuery("#value9qHome").val() == ""
                        && jQuery("#valueEIHome").val() == "") {
                        jQuery("#tbxScoreConfirm_homeFScore").val("");
                    }
                    else {
                        jQuery("#tbxScoreConfirm_homeFScore").val(ftScore);
                    }
                };
                var changeScoreAway = function (e) {
                    var htScore = 0;
                    if (jQuery("#value1qAway").val() != "") htScore = parseInt(jQuery("#value1qAway").val());
                    if (jQuery("#value2qAway").val() != "") htScore += parseInt(jQuery("#value2qAway").val());
                    if (jQuery("#value3qAway").val() != "") htScore += parseInt(jQuery("#value3qAway").val());
                    if (jQuery("#value4qAway").val() != "") htScore += parseInt(jQuery("#value4qAway").val());
                    if (jQuery("#value5qAway").val() != "") htScore += parseInt(jQuery("#value5qAway").val());

                    var ftScore = 0;
                    if (jQuery("#value1qAway").val() != "") ftScore = parseInt(jQuery("#value1qAway").val());
                    if (jQuery("#value2qAway").val() != "") ftScore += parseInt(jQuery("#value2qAway").val());
                    if (jQuery("#value3qAway").val() != "") ftScore += parseInt(jQuery("#value3qAway").val());
                    if (jQuery("#value4qAway").val() != "") ftScore += parseInt(jQuery("#value4qAway").val());
                    if (jQuery("#value5qAway").val() != "") ftScore += parseInt(jQuery("#value5qAway").val());
                    if (jQuery("#value6qAway").val() != "") ftScore += parseInt(jQuery("#value6qAway").val());
                    if (jQuery("#value7qAway").val() != "") ftScore += parseInt(jQuery("#value7qAway").val());
                    if (jQuery("#value8qAway").val() != "") ftScore += parseInt(jQuery("#value8qAway").val());
                    if (jQuery("#value9qAway").val() != "") ftScore += parseInt(jQuery("#value9qAway").val());
                    if (jQuery("#valueEIAway").val() != "") ftScore += parseInt(jQuery("#valueEIAway").val());

                    if (jQuery("#value1qAway").val() == "" && jQuery("#value2qAway").val() == ""
                    && jQuery("#value3qAway").val() == "" && jQuery("#value4qAway").val() == ""
                    && jQuery("#value5qAway").val() == "") {
                        jQuery("#tbxScoreConfirm_awayHScore").val("");
                    }
                    else {
                        jQuery("#tbxScoreConfirm_awayHScore").val(htScore);
                    }

                    if (jQuery("#value1qAway").val() == "" && jQuery("#value2qAway").val() == ""
                        && jQuery("#value3qAway").val() == "" && jQuery("#valueOTAway").val() == ""
                        && jQuery("#value4qAway").val() == "" && jQuery("#value5qAway").val() == ""
                        && jQuery("#value6qAway").val() == "" && jQuery("#value7qAway").val() == ""
                        && jQuery("#value8qAway").val() == "" && jQuery("#value9qAway").val() == ""
                        && jQuery("#valueEIAway").val() == "") {
                        jQuery("#tbxScoreConfirm_awayFScore").val("");
                    }
                    else {
                        jQuery("#tbxScoreConfirm_awayFScore").val(ftScore);
                    }
                };
                jQuery("#tbxScoreConfirm_homeHScore").ForceDecimalOnly();
                jQuery("#tbxScoreConfirm_awayHScore").ForceDecimalOnly();
                jQuery("#tbxScoreConfirm_homeFScore").ForceDecimalOnly();
                jQuery("#tbxScoreConfirm_awayFScore").ForceDecimalOnly();
                for (var i = 1; i <= 9; ++i) {
                    jQuery("#value" + i + "qHome").ForceDecimalOnly().keyup(function (e) {
                        changeScoreHome(e);
                    });
                    jQuery("#value" + i + "qAway").ForceDecimalOnly().keyup(function (e) {
                        changeScoreAway(e);
                    });
                }
                jQuery("#valueEIHome").ForceDecimalOnly().keyup(function (e) {
                    changeScoreHome(e);
                });
                jQuery("#valueEIAway").ForceDecimalOnly().keyup(function (e) {
                    changeScoreAway(e);
                });
            };
            switch (sport) {
                case "soccer":
                case "Common":
                    init_Soccer();
                    break;
                case "BasketballFootbal":
                    init_BasketballFootbal();
                    break;
                case "Hockey":
                    init_Hockey();
                    break;
                case "Tennis":
                case "Badminton":
                    init_Tennis();
                    break;
                case "Billiard":
                    init_Billiard();
                    break;
                case "Baseball":
                    init_Baseball();
                    break;
            }
            scoreConfirm_Inited = true;
        }
    }
    else {
        doAsNormal();
    }
};

var BasketballFootbal_RefundType = "";
function BasketballFootbal_InputDirectly_Event(e) {
    jQuery("#errMsg_ScoreConfirm").hide();

    var target = e.target || e.srcElement;
    if (target.checked) {
        for (var i = 1; i <= 4; ++i) {
            jQuery("#value" + i + "qHome").attr("disabled", "disabled").val("");
            jQuery("#value" + i + "qAway").attr("disabled", "disabled").val("");
        }
        jQuery("#valueOTHome").attr("disabled", "disabled").val("");
        jQuery("#valueOTAway").attr("disabled", "disabled").val("");
        jQuery("#tbxScoreConfirm_homeHScore").attr("disabled", "disabled").val("");
        jQuery("#tbxScoreConfirm_awayHScore").attr("disabled", "disabled").val("");
        jQuery("#tbxScoreConfirm_homeFScore").attr("disabled", "disabled").val("");
        jQuery("#tbxScoreConfirm_awayFScore").attr("disabled", "disabled").val("");
        switch (BasketballFootbal_RefundType) {
            case 'refundhalftime':
                jQuery("#tbxScoreConfirm_homeHScore").removeAttr("disabled").focus();
                jQuery("#tbxScoreConfirm_awayHScore").removeAttr("disabled");
                break;
            case 'refundfulltime':
                jQuery("#tbxScoreConfirm_homeFScore").removeAttr("disabled").focus();
                jQuery("#tbxScoreConfirm_awayFScore").removeAttr("disabled");
                break;
            case 'refund':
                jQuery("#tbxScoreConfirm_homeHScore").removeAttr("disabled").focus();
                jQuery("#tbxScoreConfirm_awayHScore").removeAttr("disabled");
                jQuery("#tbxScoreConfirm_homeFScore").removeAttr("disabled");
                jQuery("#tbxScoreConfirm_awayFScore").removeAttr("disabled");
                break;
        }
    }
    else {
        for (var i = 1; i <= 4; ++i) {
            jQuery("#value" + i + "qHome").removeAttr("disabled").val("");
            jQuery("#value" + i + "qAway").removeAttr("disabled").val("");
        }
        jQuery("#value1qHome").focus();
        jQuery("#valueOTHome").removeAttr("disabled").val("");
        jQuery("#valueOTAway").removeAttr("disabled").val("");
        jQuery("#tbxScoreConfirm_homeHScore").attr("disabled", "disabled").val("");
        jQuery("#tbxScoreConfirm_awayHScore").attr("disabled", "disabled").val("");
        jQuery("#tbxScoreConfirm_homeFScore").attr("disabled", "disabled").val("");
        jQuery("#tbxScoreConfirm_awayFScore").attr("disabled", "disabled").val("");
    }
}

function Tennis_InputDirectly_Event(e) {
    jQuery("#errMsg_ScoreConfirm").hide();

    jQuery("#valueFSHome").val("");
    jQuery("#valueFSAway").val("");

    var target = e.target || e.srcElement;
    if (target.checked) {
        for (var i = 1; i <= 5; ++i) {
            jQuery("#value" + i + "qHome").attr("disabled", "disabled").val("");
            jQuery("#value" + i + "qAway").attr("disabled", "disabled").val("");
        }
        jQuery("#tbxScoreConfirm_homeFScore").removeAttr("disabled").val("").focus();
        jQuery("#tbxScoreConfirm_awayFScore").removeAttr("disabled").val("");
    }
    else {
        for (var i = 1; i <= 5; ++i) {
            jQuery("#value" + i + "qHome").removeAttr("disabled").val("");
            jQuery("#value" + i + "qAway").removeAttr("disabled").val("");
        }
        jQuery("#value1qHome").focus();
        jQuery("#tbxScoreConfirm_homeFScore").attr("disabled", "disabled").val("");
        jQuery("#tbxScoreConfirm_awayFScore").attr("disabled", "disabled").val("");
    }
}


function Baseball_InputDirectly_Event(e) {
    jQuery("#errMsg_ScoreConfirm").hide();

    jQuery("#valueEIHome").val("");
    jQuery("#valueEIAway").val("");

    var target = e.target || e.srcElement;
    if (target.checked) {
        for (var i = 1; i <= 9; ++i) {
            jQuery("#value" + i + "qHome").attr("disabled", "disabled").val("");
            jQuery("#value" + i + "qAway").attr("disabled", "disabled").val("");
        }
        jQuery("#valueEIHome").attr("disabled", "disabled");
        jQuery("#valueEIAway").attr("disabled", "disabled");
        switch (BasketballFootbal_RefundType) {
            case 'refundhalftime':
                jQuery("#tbxScoreConfirm_homeHScore").removeAttr("disabled").focus();
                jQuery("#tbxScoreConfirm_awayHScore").removeAttr("disabled");
                break;
            case 'refundfulltime':
                jQuery("#tbxScoreConfirm_homeFScore").removeAttr("disabled").focus();
                jQuery("#tbxScoreConfirm_awayFScore").removeAttr("disabled");
                break;
            case 'refund':
                jQuery("#tbxScoreConfirm_homeHScore").removeAttr("disabled").focus();
                jQuery("#tbxScoreConfirm_awayHScore").removeAttr("disabled");
                jQuery("#tbxScoreConfirm_homeFScore").removeAttr("disabled");
                jQuery("#tbxScoreConfirm_awayFScore").removeAttr("disabled");
                break;
        }
    }
    else {
        for (var i = 1; i <= 9; ++i) {
            jQuery("#value" + i + "qHome").removeAttr("disabled").val("");
            jQuery("#value" + i + "qAway").removeAttr("disabled").val("");
        }
        jQuery("#valueEIHome").removeAttr("disabled");
        jQuery("#valueEIAway").removeAttr("disabled");
        jQuery("#value1qHome").focus();
        jQuery("#tbxScoreConfirm_homeHScore").attr("disabled", "disabled").val("");
        jQuery("#tbxScoreConfirm_awayHScore").attr("disabled", "disabled").val("");
        jQuery("#tbxScoreConfirm_homeFScore").attr("disabled", "disabled").val("");
        jQuery("#tbxScoreConfirm_awayFScore").attr("disabled", "disabled").val("");
    }
}
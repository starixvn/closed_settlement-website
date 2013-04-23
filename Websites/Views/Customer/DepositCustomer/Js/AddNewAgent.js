var addNewAgent = {
};

addNewAgent.init = function () {
    this.roleId = parseInt($("#hiddenRoleID").val());
    this.fillData();
    $("#tblAddNewAgent").click(function () {
        addNewAgent.submit();
    });

    if (this.roleId == 3 || this.roleId == 2 || this.roleId == 1) {
        common.initConstraint();
    }
};

addNewAgent.fillSportbookSettingAndPT = function (dataJson) {
    for (var i = 0; i < window.sports.length; ++i) {
        var id = window.sports[i].Id;

        //set value for textbox bet setting
        var minBet = parseFloat(dataJson["minbetv" + id]);
        var maxBet = parseFloat(dataJson["maxbetv" + id]);
        var maxPerMatch = parseFloat(dataJson["maxpermatchv" + id]);

        if (minBet == 0) {
            if (this.roleId == 2) {
                minBet = parseFloat(dataJson["minbetv" + id + "_min"]);
            }
            else {
                minBet = parseFloat(dataJson["minbetv" + id + "_max"]);
            }
        }

        if (maxBet == 0) {
            if (this.roleId == 2) {
                maxBet = parseFloat(dataJson["maxbetv" + id + "_max"]);
            }
            else {
                maxBet = parseFloat(dataJson["maxbetv" + id + "_min"]);
            }
        }

        if (maxPerMatch == 0) {
            maxPerMatch = parseFloat(dataJson["maxpermatchv" + id + "_max"]);
        }

        $("#" + "minbetv" + id).val(minBet.formatMoneyFromVal());
        $("#" + "maxbetv" + id).val(maxBet.formatMoneyFromVal());
        $("#" + "maxpermatchv" + id).val(maxPerMatch.formatMoneyFromVal());

        //set upline value
        if (this.roleId == 3) {
            $("#" + "minbetv" + id).get(0).uplineVal = parseFloat(dataJson["minbetv" + id]);
            $("#" + "maxbetv" + id).get(0).uplineVal = parseFloat(dataJson["maxbetv" + id]);
            $("#" + "maxpermatchv" + id).get(0).uplineVal = parseFloat(dataJson["maxpermatchv" + id]);
        }
        else {
            if (this.roleId == 2) {
                $("#" + "minbetv" + id).get(0).uplineVal = parseFloat(dataJson["minbetv" + id + "_min"]);
            }
            else {
                $("#" + "minbetv" + id).get(0).uplineVal = parseFloat(dataJson["minbetv" + id + "_max"]);
            }

            if (this.roleId == 2) {
                $("#" + "maxbetv" + id).get(0).uplineVal = parseFloat(dataJson["maxbetv" + id + "_max"]);
            }
            else {
                $("#" + "maxbetv" + id).get(0).uplineVal = parseFloat(dataJson["maxbetv" + id + "_min"]);
            }

            $("#" + "maxpermatchv" + id).get(0).uplineVal = parseFloat(dataJson["maxpermatchv" + id + "_max"]);
        }

        if (minBet < $("#" + "minbetv" + id).get(0).uplineVal) {
            $("#" + "minbetv" + id).val($("#" + "minbetv" + id).get(0).uplineVal.formatMoneyFromVal());
        }
        if (maxBet > $("#" + "maxbetv" + id).get(0).uplineVal) {
            $("#" + "maxbetv" + id).val($("#" + "maxbetv" + id).get(0).uplineVal.formatMoneyFromVal());
        }
        if (maxPerMatch > $("#" + "maxpermatchv" + id).get(0).uplineVal) {
            $("#" + "maxpermatchv" + id).val($("#" + "maxpermatchv" + id).get(0).uplineVal.formatMoneyFromVal());
        }
    }
    //sportsbook PT
    var uplineValMax;
    if (this.roleId == 3) {
        uplineValMax = parseFloat(dataJson["v1spositiontaking_1"]);
        if (isNaN(uplineValMax)) uplineValMax = 0;
        $("#selSBPT").find("option").each(function () {
            if (parseFloat($(this).val()) > uplineValMax) $(this).remove();
        });
        $("#selSBPT").removeAttr("selected").find("option[value='" + parseFloat(dataJson["v1spositiontaking_1"]).formatMoneyFromVal() + "']").attr("selected", "selected");
    }
    else if (this.roleId == 2) {
        uplineValMax = parseFloat(dataJson["v1mpositiontaking_1"]);
        if (isNaN(uplineValMax)) uplineValMax = 0;
        $("#selSBPT").find("option").each(function () {
            if (parseFloat($(this).val()) > uplineValMax) $(this).remove();
        });
        $("#selSBPT").removeAttr("selected").find("option[value='" + parseFloat(dataJson["v1mpositiontaking_1"]).formatMoneyFromVal() + "']").attr("selected", "selected");
    }
    else {
        uplineValMax = parseFloat(dataJson["v1apositiontaking_1"]);
        if (isNaN(uplineValMax)) uplineValMax = 0;
        $("#selSBPT").find("option").each(function () {
            if (parseFloat($(this).val()) > uplineValMax) $(this).remove();
        });
        $("#selSBPT").removeAttr("selected").find("option[value='" + parseFloat(dataJson["v1apositiontaking_1"]).formatMoneyFromVal() + "']").attr("selected", "selected");
    }
};

addNewAgent.fillNumberGameSettingAndPT = function (dataJson) {
    //set value for textbox bet setting
    var minBet = parseFloat(dataJson["minbetv161"]);
    var maxBet = parseFloat(dataJson["maxbetv161"]);
    var maxPerMatch = parseFloat(dataJson["maxpermatchv161"]);
    var maxPerBall = parseFloat(dataJson["maxperballv161"]);

    if (minBet == 0) {
        if (this.roleId == 2) {
            minBet = parseFloat(dataJson["minbetv161_min"]);
        }
        else {
            minBet = parseFloat(dataJson["minbetv161_max"]);
        }
    }

    if (maxBet == 0) {
        if (this.roleId == 2) {
            maxBet = parseFloat(dataJson["maxbetv161_max"]);
        }
        else {
            maxBet = parseFloat(dataJson["maxbetv161_min"]);
        }
    }

    if (maxPerMatch == 0) {
        maxPerMatch = parseFloat(dataJson["maxpermatchv161_max"]);
    }

    if (maxPerBall == 0) {
        maxPerBall = parseFloat(dataJson["maxperballv161_max"]);
    }

    $("#minbetv161").val(minBet.formatMoneyFromVal());
    $("#maxbetv161").val(maxBet.formatMoneyFromVal());
    $("#maxpermatchv161").val(maxPerMatch.formatMoneyFromVal());
    $("#maxperballv161").val(maxPerBall.formatMoneyFromVal());

    //set upline value
    if (this.roleId == 3) {
        $("#minbetv161").get(0).uplineVal = parseFloat(dataJson["minbetv161"]);
        $("#maxbetv161").get(0).uplineVal = parseFloat(dataJson["maxbetv161"]);
        $("#maxpermatchv161").get(0).uplineVal = parseFloat(dataJson["maxpermatchv161"]);
        $("#maxperballv161").get(0).uplineVal = parseFloat(dataJson["maxperballv161"]);
    }
    else {
        if (this.roleId == 2) {
            $("#minbetv161").get(0).uplineVal = parseFloat(dataJson["minbetv161_min"]);
        }
        else {
            $("#minbetv161").get(0).uplineVal = parseFloat(dataJson["minbetv161_max"]);
        }

        if (this.roleId == 2) {
            $("#maxbetv161").get(0).uplineVal = parseFloat(dataJson["maxbetv161_max"]);
        }
        else {
            $("#maxbetv161").get(0).uplineVal = parseFloat(dataJson["maxbetv161_min"]);
        }

        $("#maxpermatchv161").get(0).uplineVal = parseFloat(dataJson["maxpermatchv161_max"]);

        $("#maxperballv161").get(0).uplineVal = parseFloat(dataJson["maxperballv161_max"]);
    }

    if (minBet < $("#minbetv161").get(0).uplineVal) {
        $("#minbetv161").val($("#minbetv161").get(0).uplineVal.formatMoneyFromVal());
    }

    if (maxBet > $("#maxbetv161").get(0).uplineVal) {
        $("#maxbetv161").val($("#maxbetv161").get(0).uplineVal.formatMoneyFromVal());
    }

    if (maxPerMatch > $("#maxpermatchv161").get(0).uplineVal) {
        $("#maxpermatchv161").val($("#maxpermatchv161").get(0).uplineVal.formatMoneyFromVal());
    }

    if (maxPerBall > $("#maxperballv161").get(0).uplineVal) {
        $("#maxperballv161").val($("#maxperballv161").get(0).uplineVal.formatMoneyFromVal());
    }

    //number game PT
    var uplineValMax;
    if (this.roleId == 3) {
        uplineValMax = parseFloat(dataJson["v161spositiontaking_81"]);
        if (isNaN(uplineValMax)) uplineValMax = 0;
        $("#selNGPT").find("option").each(function () {
            if (parseFloat($(this).val()) > uplineValMax) $(this).remove();
        });
        $("#selNGPT").removeAttr("selected").find("option[value='" + parseFloat(dataJson["v161spositiontaking_81"]).formatMoneyFromVal() + "']").attr("selected", "selected");
    }
    else if (this.roleId == 2) {
        uplineValMax = parseFloat(dataJson["v161mpositiontaking_81"]);
        if (isNaN(uplineValMax)) uplineValMax = 0;
        $("#selNGPT").find("option").each(function () {
            if (parseFloat($(this).val()) > uplineValMax) $(this).remove();
        });
        $("#selNGPT").removeAttr("selected").find("option[value='" + parseFloat(dataJson["v161mpositiontaking_81"]).formatMoneyFromVal() + "']").attr("selected", "selected");
    }
    else {
        uplineValMax = parseFloat(dataJson["v161apositiontaking_81"]);
        if (isNaN(uplineValMax)) uplineValMax = 0;
        $("#selNGPT").find("option").each(function () {
            if (parseFloat($(this).val()) > uplineValMax) $(this).remove();
        });
        $("#selNGPT").removeAttr("selected").find("option[value='" + parseFloat(dataJson["v161apositiontaking_81"]).formatMoneyFromVal() + "']").attr("selected", "selected");
    }
};

addNewAgent.fillRacingSettingAndPT = function (dataJson) {
    for (var j = 0; j < window.arrRacing.length; ++j) {
        var racing = window.arrRacing[j];
        for (var k = 0; k < racing.totes.length; ++k) {
            var tote = { Id: racing.totes[k].Id };
            //bet setting
            var minBet = parseFloat(dataJson["v" + racing.Id + "minbet_" + tote.Id]);
            var maxBet = parseFloat(dataJson["v" + racing.Id + "maxbet_" + tote.Id]);
            var maxPerMatch = parseFloat(dataJson["v" + racing.Id + "maxpermatch_" + tote.Id]);

            if (minBet == 0) {
                if (this.roleId == 2) {
                    minBet = parseFloat(dataJson["v" + racing.Id + "minbet_" + tote.Id + "_min"]);
                }
                else {
                    minBet = parseFloat(dataJson["v" + racing.Id + "minbet_" + tote.Id + "_max"]);
                }
            }

            if (maxBet == 0) {
                if (this.roleId == 2) {
                    maxBet = parseFloat(dataJson["v" + racing.Id + "maxbet_" + tote.Id + "_max"]);
                }
                else {
                    maxBet = parseFloat(dataJson["v" + racing.Id + "maxbet_" + tote.Id + "_min"]);
                }
            }

            if (maxPerMatch == 0) {
                maxPerMatch = parseFloat(dataJson["v" + racing.Id + "maxpermatch_" + tote.Id + "_max"]);
            }

            $("#v" + racing.Id + "minbet_" + tote.Id).val(minBet.formatMoneyFromVal());
            $("#v" + racing.Id + "maxbet_" + tote.Id).val(maxBet.formatMoneyFromVal());
            $("#v" + racing.Id + "maxpermatch_" + tote.Id).val(maxPerMatch.formatMoneyFromVal());

            //set upline
            if (this.roleId == 3) {
                $("#v" + racing.Id + "minbet_" + tote.Id).get(0).uplineVal = parseFloat(dataJson["v" + racing.Id + "minbet_" + tote.Id]);
                $("#v" + racing.Id + "maxbet_" + tote.Id).get(0).uplineVal = parseFloat(dataJson["v" + racing.Id + "maxbet_" + tote.Id]);
                $("#v" + racing.Id + "maxpermatch_" + tote.Id).get(0).uplineVal = parseFloat(dataJson["v" + racing.Id + "maxpermatch_" + tote.Id]);
            }
            else {
                if (this.roleId == 2) {
                    $("#v" + racing.Id + "minbet_" + tote.Id).get(0).uplineVal = parseFloat(dataJson["v" + racing.Id + "minbet_" + tote.Id + "_min"]);
                }
                else {
                    $("#v" + racing.Id + "minbet_" + tote.Id).get(0).uplineVal = parseFloat(dataJson["v" + racing.Id + "minbet_" + tote.Id + "_max"]);
                }

                if (this.roleId == 2) {
                    $("#v" + racing.Id + "maxbet_" + tote.Id).get(0).uplineVal = parseFloat(dataJson["v" + racing.Id + "maxbet_" + tote.Id + "_max"]);
                }
                else {
                    $("#v" + racing.Id + "maxbet_" + tote.Id).get(0).uplineVal = parseFloat(dataJson["v" + racing.Id + "maxbet_" + tote.Id + "_min"]);
                }

                $("#v" + racing.Id + "maxpermatch_" + tote.Id).get(0).uplineVal = parseFloat(dataJson["v" + racing.Id + "maxpermatch_" + tote.Id + "_max"]);
            }

            if (minBet < $("#v" + racing.Id + "minbet_" + tote.Id).get(0).uplineVal) {
                $("#v" + racing.Id + "minbet_" + tote.Id).val($("#v" + racing.Id + "minbet_" + tote.Id).get(0).uplineVal.formatMoneyFromVal());
            }

            if (maxBet > $("#v" + racing.Id + "maxbet_" + tote.Id).get(0).uplineVal) {
                $("#v" + racing.Id + "maxbet_" + tote.Id).val($("#v" + racing.Id + "maxbet_" + tote.Id).get(0).uplineVal.formatMoneyFromVal());
            }

            if (maxPerMatch > $("#v" + racing.Id + "maxpermatch_" + tote.Id).get(0).uplineVal) {
                $("#v" + racing.Id + "maxpermatch_" + tote.Id).val($("#v" + racing.Id + "maxpermatch_" + tote.Id).get(0).uplineVal.formatMoneyFromVal());
            }

            //commision
            var uplineValMax;
            uplineValMax = parseFloat(dataJson["v" + racing.Id + "discount_" + tote.Id]);
            if (isNaN(uplineValMax)) uplineValMax = 0;
            $("#v" + racing.Id + "discount_" + tote.Id).find("option").each(function () {
                if (parseFloat($(this).val()) > uplineValMax) $(this).remove();
            });
            $("#v" + racing.Id + "discount_" + tote.Id).removeAttr("selected").find("option[value='" + parseFloat(dataJson["v" + racing.Id + "discount_" + tote.Id]).formatMoneyFromVal() + "']").attr("selected", "selected");

            //win place pt
            if (this.roleId == 3) {
                uplineValMax = parseFloat(dataJson["v" + racing.Id + "spositiontaking_" + tote.Id]);
                if (isNaN(uplineValMax)) uplineValMax = 0;
                $("#v" + racing.Id + "spositiontaking_" + tote.Id).find("option").each(function () {
                    if (parseFloat($(this).val()) > uplineValMax) $(this).remove();
                });
                $("#v" + racing.Id + "spositiontaking_" + tote.Id).removeAttr("selected").find("option[value='" + parseFloat(dataJson["v" + racing.Id + "spositiontaking_" + tote.Id]).formatMoneyFromVal() + "']").attr("selected", "selected");
            }
            else if (this.roleId == 2) {
                uplineValMax = parseFloat(dataJson["v" + racing.Id + "mpositiontaking_" + tote.Id]);
                if (isNaN(uplineValMax)) uplineValMax = 0;
                $("#v" + racing.Id + "mpositiontaking_" + tote.Id).find("option").each(function () {
                    if (parseFloat($(this).val()) > uplineValMax) $(this).remove();
                });
                $("#v" + racing.Id + "mpositiontaking_" + tote.Id).removeAttr("selected").find("option[value='" + parseFloat(dataJson["v" + racing.Id + "mpositiontaking_" + tote.Id]).formatMoneyFromVal() + "']").attr("selected", "selected");
            }
            else {
                uplineValMax = parseFloat(dataJson["v" + racing.Id + "apositiontaking_" + tote.Id]);
                if (isNaN(uplineValMax)) uplineValMax = 0;
                $("#v" + racing.Id + "apositiontaking_" + tote.Id).find("option").each(function () {
                    if (parseFloat($(this).val()) > uplineValMax) $(this).remove();
                });
                $("#v" + racing.Id + "apositiontaking_" + tote.Id).removeAttr("selected").find("option[value='" + parseFloat(dataJson["v" + racing.Id + "apositiontaking_" + tote.Id]).formatMoneyFromVal() + "']").attr("selected", "selected");
            }

            //disable checkbox if upline is not enable
            if (dataJson["disable" + racing.Id + "_" + tote.Id] == "True") {
                $("#" + "disable" + racing.Id + "_" + tote.Id).attr("disabled", "disabled");
            }
            else {
                $("#" + "disable" + racing.Id + "_" + tote.Id).attr("checked", "checked");
            }
        }
    }
};

addNewAgent.fillData = function () {
    this.fillSportbookSettingAndPT(window.dataJson);
    this.fillNumberGameSettingAndPT(window.dataJson);
    this.fillRacingSettingAndPT(window.dataJson);
};

addNewAgent.assignSportbookSettingAndPT = function (params) {
    for (var i = 0; i < window.sports.length; ++i) {
        var bettypes = window.sports[i].Bettype;
        var id = window.sports[i].Id;
        params.push({
            Name: "minbetv" + id
        , Value: $("#" + "minbetv" + id).getNumberValue()
        });

        params.push({
            Name: "maxbetv" + id
        , Value: $("#" + "maxbetv" + id).getNumberValue()
        });

        params.push({
            Name: "maxpermatchv" + id
        , Value: $("#" + "maxpermatchv" + id).getNumberValue()
        });

        if (id.toString().toLowerCase() == "99mp") {
            continue;
        }
        //sportbook position taking
        if (bettypes.length > 0) {
            for (var l = 0; l < bettypes.length; l++) {
                params.push({
                    Name: "v" + id + "spositiontaking_" + bettypes[l]
                    , Value: $("#selSBPT").val()
                });
                params.push({
                    Name: "v" + id + "mpositiontaking_" + bettypes[l]
                    , Value: $("#selSBPT").val()
                });
                params.push({
                    Name: "v" + id + "apositiontaking_" + bettypes[l]
                    , Value: $("#selSBPT").val()
                });
                if (id == "1") {
                    params.push({
                        Name: "v" + id + "slivepositiontaking_" + bettypes[l]
                        , Value: $("#selSBPT").val()
                    });
                    params.push({
                        Name: "v" + id + "mlivepositiontaking_" + bettypes[l]
                        , Value: $("#selSBPT").val()
                    });
                    params.push({
                        Name: "v" + id + "alivepositiontaking_" + bettypes[l]
                        , Value: $("#selSBPT").val()
                    });
                }
            }
        }
    }
};

addNewAgent.assignNumberGameSetting = function (params) {
    params.push({
        Name: "minbetv161"
        , Value: $("#minbetv161").getNumberValue()
    });
    params.push({
        Name: "maxbetv161"
        , Value: $("#maxbetv161").getNumberValue()
    });
    params.push({
        Name: "maxpermatchv161"
        , Value: $("#maxpermatchv161").getNumberValue()
    });
    params.push({
        Name: "maxperballv161"
        , Value: $("#maxperballv161").getNumberValue()
    });
};

addNewAgent.assignNumberGamePT = function (params) {
    var id = 161;
    var arr = ["s", "m", "a"];
    for (var i = 0; i < arr.length; i++) {
        var type = arr[i];

        var bettypes = window.arrNumberGameBettypes;
        
        for (var j = 0; j < bettypes.length; j++) {
            params.push({
                Name: "v" + id + type + "positiontaking_" + bettypes[j]
                , Value: $("#selNGPT").val()
            });
        }

        for (var k = 0; k < bettypes.length; k++) {
            params.push({
                Name: "v" + id + type + "livepositiontaking_" + bettypes[k]
                , Value: $("#selNGPT").val()
            });
        }
    }
};

addNewAgent.assignRacingBetSettingComPT = function (params) {
    for (var j = 0; j < window.arrRacing.length; ++j) {
        var racing = window.arrRacing[j];
        for (var k = 0; k < racing.totes.length; ++k) {
            var tote = { Id: racing.totes[k].Id, Bettype: racing.totes[k].Bettype };

            //bet setting
            params.push({
                Name: "v" + racing.Id + "minbet_" + tote.Id
                    , Value: $("#v" + racing.Id + "minbet_" + tote.Id).getNumberValue()
            });
            params.push({
                Name: "v" + racing.Id + "maxbet_" + tote.Id
                    , Value: $("#v" + racing.Id + "maxbet_" + tote.Id).getNumberValue()
            });
            params.push({
                Name: "v" + racing.Id + "maxpermatch_" + tote.Id
                    , Value: $("#v" + racing.Id + "maxpermatch_" + tote.Id).getNumberValue()
            });
            
            for (var i = 0; i < tote.Bettype.length; i++) {
                //commision
                params.push({
                    Name: "v" + racing.Id + (this.roleId == 1 ? "player" : "") + "discount_" + tote.Bettype[i]
                    , Value: $("#v" + racing.Id + "discount_" + tote.Id).val()
                });

                //win place pt
                if (this.roleId == 3) {
                    params.push({
                        Name: "v" + racing.Id + "mpositiontaking_" + tote.Bettype[i]
                    , Value: $("#v" + racing.Id + "spositiontaking_" + tote.Id).val()
                    });
                }
                else if (this.roleId == 2) {
                    params.push({
                        Name: "v" + racing.Id + "apositiontaking_" + tote.Bettype[i]
                        , Value: $("#v" + racing.Id + "mpositiontaking_" + tote.Id).val()
                    });
                }
                else {
                    params.push({
                        Name: "v" + racing.Id + "apositiontaking_" + tote.Bettype[i]
                        , Value: $("#v" + racing.Id + "apositiontaking_" + tote.Id).val()
                    });
                }
            }
            
            //enable
            params.push({
                Name: "disable" + racing.Id + "_" + tote.Id
                , Value: !$("#" + "disable" + racing.Id + "_" + tote.Id).is(":checked")
            });
        }
    }
};

addNewAgent.getPostParam = function () {
    var params = [];
    params.push({
        Name: "creator"
        , Value: $("#hiddenAccID").val()
    });

    //sportbook bet setting, postion taking
    this.assignSportbookSettingAndPT(params);

    //numbergame bet setting
    this.assignNumberGameSetting(params);

    //numbergame position taking
    this.assignNumberGamePT(params);

    //racing bet setting, comm & PT setting
    this.assignRacingBetSettingComPT(params);

    var postParam = {
        MasterName: $("#tbxUserNameAddNew").val()
        , AgentName: $("#tbxUserNameAddNew").val()
        , MemberName: $("#tbxUserNameAddNew").val()
        , PostParams: params
    };

    if (this.roleId == 3) {
        postParam.SuperId = $("#hiddenCustID").val();
    }
    else if (this.roleId == 2) {
        postParam.MasterId = $("#hiddenCustID").val();
    }
    else {
        postParam.AgentId = $("#hiddenCustID").val();
    }
    
    return {
        postParam: postParam
    };
};

addNewAgent.verifyData = function () {
    if (common.isEmpty($("#tbxUserNameAddNew").val())) {
        common.showErr("Please input Username!");
        return false;
    }

    if (!common.validateUserName($("#tbxUserNameAddNew").val())) {
        common.showErr("User Name is invalid!");
        return false;
    }

    return true;
};

addNewAgent.submit = function () {
    common.hideDivMsg();

    if (!this.verifyData()) return;

    switch (this.roleId) {
        case 3:
            {
                if (!common.verifyDataContrain()) {
                    if (common.errMsg != "") {
                        common.showErr(common.errMsg);
                    }
                    else common.showErr("Data is invalid!");
                    return;
                }
                common.blockUI();
                $.ajax({
                    url: 'AddNewMasterSumitHandler',
                    type: 'POST',
                    dataType: 'json',
                    data: $.toJSON(this.getPostParam()),
                    contentType: 'application/json; charset=utf-8',
                    success: function (data) {
                        if (parseInt(data.ErrorCode) != 0) {
                            common.showErr(data.ErrorMessage);
                        }
                        else {
                            common.showSuccess("Add new master " + $("#tbxUserNameAddNew").val() + " successfully!");
                            $("#tbxUserNameAddNew").val("");
                            //common.resetValues();
                        }
                        common.unBlock();
                    }
                });
            }
            break;
        case 2:
            {
                if (!common.verifyDataContrain()) {
                    if (common.errMsg != "") {
                        common.showErr(common.errMsg);
                    }
                    else common.showErr("Data is invalid!");
                    return;
                }
                common.blockUI();
                $.ajax({
                    url: 'AddNewAgentSumitHandler',
                    type: 'POST',
                    dataType: 'json',
                    data: $.toJSON(this.getPostParam()),
                    contentType: 'application/json; charset=utf-8',
                    success: function (data) {
                        if (parseInt(data.ErrorCode) != 0) {
                            common.showErr(data.ErrorMessage);
                        }
                        else {
                            common.showSuccess("Add new agent " + $("#tbxUserNameAddNew").val() + " successfully!");
                            $("#tbxUserNameAddNew").val("");
                            //common.resetValues();
                        }
                        common.unBlock();
                    }
                });
            }
            break;
        case 1:
            {
                if (!common.verifyDataContrain()) {
                    if (common.errMsg != "") {
                        common.showErr(common.errMsg);
                    }
                    else common.showErr("Data is invalid!");
                    return;
                }
                common.blockUI();
                $.ajax({
                    url: 'AddNewMemberSumitHandler',
                    type: 'POST',
                    dataType: 'json',
                    data: $.toJSON(this.getPostParam()),
                    contentType: 'application/json; charset=utf-8',
                    success: function (data) {
                        if (parseInt(data.ErrorCode) != 0) {
                            common.showErr(data.ErrorMessage);
                        }
                        else {
                            common.showSuccess("Add new member " + $("#tbxUserNameAddNew").val() + " successfully!");
                            $("#tbxUserNameAddNew").val("");
                            //common.resetValues();
                        }
                        common.unBlock();
                    }
                });
            }
            break;
    }
};

$(document).ready(function () {
    addNewAgent.init();
});
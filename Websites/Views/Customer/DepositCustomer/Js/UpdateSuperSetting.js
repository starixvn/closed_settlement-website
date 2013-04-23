var updSuperSetting = {
};

updSuperSetting.init = function () {
    this.roleId = parseInt($("#hiddenRoleID").val());
    this.fillData();
    $("#tblUpdate").click(function () {
        updSuperSetting.submit();
    });

    if (this.roleId == 3 || this.roleId == 2 || this.roleId == 1) {
        common.initConstraint();
    }
};

updSuperSetting.fillSportbookSettingAndPT = function (dataJson) {
    for (var i = 0; i < window.sports.length; ++i) {
        var id = window.sports[i].Id;
        $("#" + "minbetv" + id).val(parseFloat(dataJson["minbetv" + id]).formatMoneyFromVal());
        $("#" + "maxbetv" + id).val(parseFloat(dataJson["maxbetv" + id]).formatMoneyFromVal());
        $("#" + "maxpermatchv" + id).val(parseFloat(dataJson["maxpermatchv" + id]).formatMoneyFromVal());

        if (this.roleId == 3 || this.roleId == 2 || this.roleId == 1) {
            if (this.roleId == 3) {
                $("#" + "minbetv" + id).get(0).uplineVal = parseFloat(dataJson["minbetv" + id + "_min"]);
                $("#" + "maxbetv" + id).get(0).uplineVal = parseFloat(dataJson["maxbetv" + id + "_max"]);
                $("#" + "maxpermatchv" + id).get(0).uplineVal = parseFloat(dataJson["maxpermatchv" + id + "_max"]);
            }
            else {
                $("#" + "minbetv" + id).get(0).uplineVal = parseFloat(dataJson["minbetv" + id + "_max"]);
                $("#" + "maxbetv" + id).get(0).uplineVal = parseFloat(dataJson["maxbetv" + id + "_min"]);
                $("#" + "maxpermatchv" + id).get(0).uplineVal = parseFloat(dataJson["maxpermatchv" + id + "_max"]);
            }
        }
    }
    //sportsbook PT
    var uplineValMax, uplineValMin;
    if (this.roleId == 4) {
        $("#selSBPT").removeAttr("selected").find("option[value='" + parseFloat(dataJson["v1spositiontaking_1"]).formatMoneyFromVal() + "']").attr("selected", "selected");
    }
    else if (this.roleId == 3) {
        uplineValMax = parseFloat(dataJson["v1mpositiontaking_1_max"]);
        if (isNaN(uplineValMax)) uplineValMax = 0;
        uplineValMin = parseFloat(dataJson["v1mpositiontaking_1_min"]);
        if (isNaN(uplineValMin)) uplineValMin = 0;
        $("#selSBPT").find("option").each(function () {
            if (parseFloat($(this).val()) > uplineValMax) $(this).remove();
            if (parseFloat($(this).val()) < uplineValMin) $(this).remove();
        });
        $("#selSBPT").removeAttr("selected").find("option[value='" + parseFloat(dataJson["v1mpositiontaking_1"]).formatMoneyFromVal() + "']").attr("selected", "selected");
    }
    else {
        uplineValMax = parseFloat(dataJson["v1apositiontaking_1_max"]);
        if (isNaN(uplineValMax)) uplineValMax = 0;
        $("#selSBPT").find("option").each(function () {
            if (parseFloat($(this).val()) > uplineValMax) $(this).remove();
        });
        $("#selSBPT").removeAttr("selected").find("option[value='" + parseFloat(dataJson["v1apositiontaking_1"]).formatMoneyFromVal() + "']").attr("selected", "selected");
    }
};

updSuperSetting.fillNumberGameSettingAndPT = function (dataJson) {
    $("#minbetv161").val(parseFloat(dataJson["minbetv161"]).formatMoneyFromVal());
    $("#maxbetv161").val(parseFloat(dataJson["maxbetv161"]).formatMoneyFromVal());
    $("#maxpermatchv161").val(parseFloat(dataJson["maxpermatchv161"]).formatMoneyFromVal());
    $("#maxperballv161").val(parseFloat(dataJson["maxperballv161"]).formatMoneyFromVal());
    
    if (this.roleId == 3 || this.roleId == 2 || this.roleId == 1) {
        if (this.roleId == 3) {
            $("#minbetv161").get(0).uplineVal = parseFloat(dataJson["minbetv161_min"]);
            $("#maxbetv161").get(0).uplineVal = parseFloat(dataJson["maxbetv161_max"]);
            $("#maxpermatchv161").get(0).uplineVal = parseFloat(dataJson["maxpermatchv161_max"]);
            $("#maxperballv161").get(0).uplineVal = parseFloat(dataJson["maxperballv161_max"]);
        }
        else {
            $("#minbetv161").get(0).uplineVal = parseFloat(dataJson["minbetv161_max"]);
            $("#maxbetv161").get(0).uplineVal = parseFloat(dataJson["maxbetv161_min"]);
            $("#maxpermatchv161").get(0).uplineVal = parseFloat(dataJson["maxpermatchv161_max"]);
            $("#maxperballv161").get(0).uplineVal = parseFloat(dataJson["maxperballv161_max"]);
        }
    }

    //number game PT
    var uplineValMax, uplineValMin;
    if (this.roleId == 4) {
        $("#selNGPT").removeAttr("selected").find("option[value='" + parseFloat(dataJson["v161spositiontaking_81"]).formatMoneyFromVal() + "']").attr("selected", "selected");
    }
    else if (this.roleId == 3) {
        uplineValMax = parseFloat(dataJson["v161mpositiontaking_81_max"]);
        if (isNaN(uplineValMax)) uplineValMax = 0;
        uplineValMin = parseFloat(dataJson["v161mpositiontaking_81_min"]);
        if (isNaN(uplineValMin)) uplineValMin = 0;
        $("#selNGPT").find("option").each(function () {
            if (parseFloat($(this).val()) > uplineValMax) $(this).remove();
            if (parseFloat($(this).val()) < uplineValMin) $(this).remove();
        });
        $("#selNGPT").removeAttr("selected").find("option[value='" + parseFloat(dataJson["v161mpositiontaking_81"]).formatMoneyFromVal() + "']").attr("selected", "selected");
    }
    else {
        uplineValMax = parseFloat(dataJson["v161apositiontaking_81_max"]);
        if (isNaN(uplineValMax)) uplineValMax = 0;
        $("#selNGPT").find("option").each(function () {
            if (parseFloat($(this).val()) > uplineValMax) $(this).remove();
        });
        $("#selNGPT").removeAttr("selected").find("option[value='" + parseFloat(dataJson["v161apositiontaking_81"]).formatMoneyFromVal() + "']").attr("selected", "selected");
    }
};

updSuperSetting.fillRacingSettingAndPT = function (dataJson) {
    for (var j = 0; j < window.arrRacing.length; ++j) {
        var racing = window.arrRacing[j];
        for (var k = 0; k < racing.totes.length; ++k) {
            var tote = { Id: racing.totes[k].Id };
            //bet setting
            $("#v" + racing.Id + "minbet_" + tote.Id).val(parseFloat(dataJson["v" + racing.Id + "minbet_" + tote.Id]).formatMoneyFromVal());
            if (this.roleId == 3) {
                $("#v" + racing.Id + "minbet_" + tote.Id).get(0).uplineVal = parseFloat(dataJson["v" + racing.Id + "minbet_" + tote.Id + "_min"]);
            }
            else if (this.roleId == 2 || this.roleId == 1) {
                $("#v" + racing.Id + "minbet_" + tote.Id).get(0).uplineVal = parseFloat(dataJson["v" + racing.Id + "minbet_" + tote.Id + "_max"]);
            }

            $("#v" + racing.Id + "maxbet_" + tote.Id).val(parseFloat(dataJson["v" + racing.Id + "maxbet_" + tote.Id]).formatMoneyFromVal());
            if (this.roleId == 3) {
                $("#v" + racing.Id + "maxbet_" + tote.Id).get(0).uplineVal = parseFloat(dataJson["v" + racing.Id + "maxbet_" + tote.Id + "_max"]);
            }
            else if (this.roleId == 2 || this.roleId == 1) {
                $("#v" + racing.Id + "maxbet_" + tote.Id).get(0).uplineVal = parseFloat(dataJson["v" + racing.Id + "maxbet_" + tote.Id + "_min"]);
            }

            $("#v" + racing.Id + "maxpermatch_" + tote.Id).val(parseFloat(dataJson["v" + racing.Id + "maxpermatch_" + tote.Id]).formatMoneyFromVal());
            if (this.roleId != 4) {
                $("#v" + racing.Id + "maxpermatch_" + tote.Id).get(0).uplineVal = parseFloat(dataJson["v" + racing.Id + "maxpermatch_" + tote.Id + "_max"]);
            }

            var uplineValMax, uplineValMin;

            //commision
            if (this.roleId == 3 || this.roleId == 2 || this.roleId == 1) {
                uplineValMax = parseFloat(dataJson["v" + racing.Id + "discount_" + tote.Id + "_max"]);
                if (isNaN(uplineValMax)) uplineValMax = 0;
                uplineValMin = parseFloat(dataJson["v" + racing.Id + "discount_" + tote.Id + "_min"]);
                if (isNaN(uplineValMin)) uplineValMin = 0;
                $("#v" + racing.Id + "discount_" + tote.Id).find("option").each(function () {
                    if (parseFloat($(this).val()) > uplineValMax) $(this).remove();
                    if (parseFloat($(this).val()) < uplineValMin) $(this).remove();
                });    
            }

            $("#v" + racing.Id + "discount_" + tote.Id).removeAttr("selected").find("option[value='" + parseFloat(dataJson["v" + racing.Id + "discount_" + tote.Id]).formatMoneyFromVal() + "']").attr("selected", "selected");

            //win place pt
            if (this.roleId == 4) {
                $("#v" + racing.Id + "spositiontaking_" + tote.Id).removeAttr("selected").find("option[value='" + parseFloat(dataJson["v" + racing.Id + "spositiontaking_" + tote.Id]).formatMoneyFromVal() + "']").attr("selected", "selected");
            }
            else if (this.roleId == 3) {
                uplineValMax = parseFloat(dataJson["v" + racing.Id + "mpositiontaking_" + tote.Id + "_max"]);
                if (isNaN(uplineValMax)) uplineValMax = 0;
                uplineValMin = parseFloat(dataJson["v" + racing.Id + "mpositiontaking_" + tote.Id + "_min"]);
                if (isNaN(uplineValMin)) uplineValMin = 0;
                $("#v" + racing.Id + "mpositiontaking_" + tote.Id).find("option").each(function () {
                    if (parseFloat($(this).val()) > uplineValMax) $(this).remove();
                    if (parseFloat($(this).val()) < uplineValMin) $(this).remove();
                });
                $("#v" + racing.Id + "mpositiontaking_" + tote.Id).removeAttr("selected").find("option[value='" + parseFloat(dataJson["v" + racing.Id + "mpositiontaking_" + tote.Id]).formatMoneyFromVal() + "']").attr("selected", "selected");
            }
            else {
                uplineValMax = parseFloat(dataJson["v" + racing.Id + "apositiontaking_" + tote.Id + "_max"]);
                if (isNaN(uplineValMax)) uplineValMax = 0;
                $("#v" + racing.Id + "apositiontaking_" + tote.Id).find("option").each(function () {
                    if (parseFloat($(this).val()) > uplineValMax) $(this).remove();
                });
                $("#v" + racing.Id + "apositiontaking_" + tote.Id).removeAttr("selected").find("option[value='" + parseFloat(dataJson["v" + racing.Id + "apositiontaking_" + tote.Id]).formatMoneyFromVal() + "']").attr("selected", "selected");
            }

            if (this.roleId == 4) {
                //house auto pt
                $("#v" + racing.Id + "ibcpositiontaking_" + tote.Id).removeAttr("selected").find("option[value='" + parseFloat(dataJson["v" + racing.Id + "ibcpositiontaking_" + tote.Id]).formatMoneyFromVal() + "']").attr("selected", "selected");
            }
        }
    }
};

updSuperSetting.fillData = function () {
    this.fillSportbookSettingAndPT(window.dataJson);
    this.fillNumberGameSettingAndPT(window.dataJson);
    this.fillRacingSettingAndPT(window.dataJson);
};

updSuperSetting.assignSportbookSettingAndPT = function (params) {
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

updSuperSetting.assignNumberGameSetting = function (params) {
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

updSuperSetting.assignNumberGamePT = function (params) {
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

updSuperSetting.assignRacingBetSettingComPT = function (params) {
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
                    Name: "v" + racing.Id + "discount_" + tote.Bettype[i]
                    , Value: $("#v" + racing.Id + "discount_" + tote.Id).val()
                });

                //win place pt
                if (this.roleId == 4) {
                    params.push({
                        Name: "v" + racing.Id + "spositiontaking_" + tote.Bettype[i]
                        , Value: $("#v" + racing.Id + "spositiontaking_" + tote.Id).val()
                    });
                    params.push({
                        Name: "v" + racing.Id + "mpositiontaking_" + tote.Bettype[i]
                        , Value: $("#v" + racing.Id + "spositiontaking_" + tote.Id).val()
                    });
                    params.push({
                        Name: "v" + racing.Id + "apositiontaking_" + tote.Bettype[i]
                        , Value: $("#v" + racing.Id + "spositiontaking_" + tote.Id).val()
                    });
                }
                else if (this.roleId == 3) {
                    params.push({
                        Name: "v" + racing.Id + "mpositiontaking_" + tote.Bettype[i]
                        , Value: $("#v" + racing.Id + "mpositiontaking_" + tote.Id).val()
                    });
                }
                else {
                    params.push({
                        Name: "v" + racing.Id + "apositiontaking_" + tote.Bettype[i]
                        , Value: $("#v" + racing.Id + "apositiontaking_" + tote.Id).val()
                    });
                }

                if (this.roleId == 4) {
                    //house auto pt
                    params.push({
                        Name: "v" + racing.Id + "ibcpositiontaking_" + tote.Bettype[i]
                        , Value: $("#v" + racing.Id + "ibcpositiontaking_" + tote.Id).val()
                    });
                }
            }
        }
    }
};

updSuperSetting.getPostParam = function () {
    var params = [];
    params.push({
        Name: "userid"
        , Value: $("#hiddenAccID").val()
    });
    params.push({
        Name: "custid"
        , Value: $("#hiddenCustID").val()
    });

    //sportbook bet setting, postion taking
    this.assignSportbookSettingAndPT(params);

    //numbergame bet setting
    this.assignNumberGameSetting(params);

    //numbergame position taking
    this.assignNumberGamePT(params);

    //racing bet setting, comm & PT setting
    this.assignRacingBetSettingComPT(params);

    return {
        postParam: {
            PostParams: params
        }
    };
};

updSuperSetting.submit = function () {
    common.hideDivMsg();
    switch (this.roleId) {
        case 4:
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
                    url: 'UpdateSuperSettingSumitHandler',
                    type: 'POST',
                    dataType: 'json',
                    data: $.toJSON(this.getPostParam()),
                    contentType: 'application/json; charset=utf-8',
                    success: function (data) {
                        if (parseInt(data.ErrorCode) != 0) {
                            common.showErr(data.ErrorMessage);
                        }
                        else {
                            common.showSuccess("Update successfully!");
                            common.reloadPage("DepositCustomer/EditSettingSuper");
                        }
                        common.unBlock();
                    }
                });
            }
            break;
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
                    url: 'UpdateMasterSettingSumitHandler',
                    type: 'POST',
                    dataType: 'json',
                    data: $.toJSON(this.getPostParam()),
                    contentType: 'application/json; charset=utf-8',
                    success: function (data) {
                        if (parseInt(data.ErrorCode) != 0) {
                            common.showErr(data.ErrorMessage);
                        }
                        else {
                            common.showSuccess("Update successfully!");
                            common.reloadPage("DepositCustomer/EditSettingAgent");
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
                    url: 'UpdateAgentSettingSumitHandler',
                    type: 'POST',
                    dataType: 'json',
                    data: $.toJSON(this.getPostParam()),
                    contentType: 'application/json; charset=utf-8',
                    success: function (data) {
                        if (parseInt(data.ErrorCode) != 0) {
                            common.showErr(data.ErrorMessage);
                        }
                        else {
                            common.showSuccess("Update successfully!");
                            common.reloadPage("DepositCustomer/EditSettingAgent");
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
                    url: 'UpdateMemberSettingSumitHandler',
                    type: 'POST',
                    dataType: 'json',
                    data: $.toJSON(this.getPostParam()),
                    contentType: 'application/json; charset=utf-8',
                    success: function (data) {
                        if (parseInt(data.ErrorCode) != 0) {
                            common.showErr(data.ErrorMessage);
                        }
                        else {
                            common.showSuccess("Update successfully!");
                            common.reloadPage("DepositCustomer/EditSettingAgent");
                        }
                        common.unBlock();
                    }
                });
            }
            break;
    }
};

$(document).ready(function () {
    updSuperSetting.init();
});
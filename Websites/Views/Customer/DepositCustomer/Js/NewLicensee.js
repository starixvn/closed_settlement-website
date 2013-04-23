var newLic = {};

newLic.init = function () {
    $("#tblAdd").click(function () {
        newLic.submit();
    });
};

newLic.verifyData = function () {
    if (common.isEmpty($("#tbxSuperName").val())) {
        common.showErr("Please input Super's Username!");
        return false;
    }

    if (!common.validateUserName($("#tbxSuperName").val())) {
        common.showErr("Super's Username is invalid!");
        return false;
    }

    if (!common.isEmpty($("#tbxAgentName").val()) && common.isEmpty($("#tbxMasterName").val())) {
        common.showErr("Please input Master's Username!");
        return false;
    }

    if (!common.isEmpty($("#tbxMasterName").val())) {
        if (!common.validateUserName($("#tbxMasterName").val())) {
            common.showErr("Master's Username is invalid!");
            return false;
        }
    }

    if (!common.isEmpty($("#tbxAgentName").val())) {
        if (!common.validateUserName($("#tbxAgentName").val())) {
            common.showErr("Agent's Username is invalid!");
            return false;
        }
    }

    return true;
};

newLic.assignSportbookSettingAndPT = function (params) {
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

newLic.assignNumberGameSetting = function (params) {
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

newLic.assignNumberGamePT = function (params) {
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

newLic.assignRacingBetSettingComPT = function (params) {
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

                //house auto pt
                params.push({
                    Name: "v" + racing.Id + "ibcpositiontaking_" + tote.Bettype[i]
                    , Value: $("#v" + racing.Id + "ibcpositiontaking_" + tote.Id).val()
                });
            }

            //enable
            params.push({
                Name: "disable" + racing.Id + "_" + tote.Id
                , Value: !$("#" + "disable" + racing.Id + "_" + tote.Id).is(":checked")
            });
        }
    }
};

newLic.getPostParam = function () {
    var params = [];
    params.push({
        Name: "creator"
        , Value: $("#hiddenAccID").val()
    });
    params.push({
        Name: "site"
        , Value: $("#selSites").find("option[value='" + $("#selSites").val() + "']").html().replace(/ /g, '')
    });
    params.push({
        Name: "currency"
        , Value: $("#selCurrency").val()
    });

    //sportbook bet setting, postion taking
    this.assignSportbookSettingAndPT(params);

    //numbergame bet setting
    this.assignNumberGameSetting(params);

    //numbergame position taking
    this.assignNumberGamePT(params);

    //racing bet setting, comm & PT setting
    this.assignRacingBetSettingComPT(params);

    //enable number game
    params.push({
        Name: "DisableNumber"
        , Value: !$("#cbxEnableNumberGame").is(":checked")
    });
    
    return {
        postParam: {
            SuperName: $("#tbxSuperName").val()
            , MasterName: $("#tbxMasterName").val()
            , AgentName: $("#tbxAgentName").val()
            , PostParams: params
        }
    };
};

newLic.submit = function () {
    common.hideDivMsg();

    if (!this.verifyData()) return;

    if (!common.verifyDataContrain()) {
        if (common.errMsg != "") {
            common.showErr(common.errMsg);
        }
        else common.showErr("Data is invalid!");
        return;
    }

    common.blockUI();

    $.ajax({
        url: 'NewLicenseeSubmitHandler',
        type: 'POST',
        dataType: 'json',
        data: $.toJSON(this.getPostParam()),
        contentType: 'application/json; charset=utf-8',
        success: function (data) {
            if (parseInt(data.ErrorCode) != 0) {
                common.showErr(data.ErrorMessage);
            }
            else {
                var msg = "Add new super: " + $("#tbxSuperName").val();
                if ($("#tbxMasterName").val() != "") {
                    msg += ", master: " + $("#tbxMasterName").val();
                }

                if ($("#tbxAgentName").val() != "") {
                    msg += ", agent: " + $("#tbxAgentName").val();
                }

                msg += " successfully!";

                common.showSuccess(msg);
                common.resetValues();
            }
            common.unBlock();
        }
    });
};

$(document).ready(function () {
    newLic.init();
});
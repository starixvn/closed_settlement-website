var gridHeight = $(window).height() - 150;

$(function () {
    $('#addNewFollowCustomer').dialog({
        title: "Add New Follow Customer",
        autoOpen: false,
        height: 140,
        width: 370,
        modal: true
    });

});

function showPopup() {
    $('#errMsgDig').hide();
    $('#txtUserName').val('');
    $('#txtRate').val('');
    $('#addNewFollowCustomer').removeClass("hidden").dialog("open");
}

function isPositiveDouble(rate) {
    var validChars = '0123456789.';
    for (var i = 0; i < rate.length; i++) {
        if (validChars.indexOf(rate.charAt(i)) == -1)
            return false;
    }
    return true;
}

var autoFollowBet = {
    width: 400,
    height: 500,
    status: 0,
    buildGrid: function (model) {
        var arr = [];
        if (model.length > 0) {
            for (var i = 0; i < model.length; i++) {
                if (i % 2 != 0)
                    arr.push(autoFollowBet.renderRow(i, 'bgeven', model[i].Username, model[i].Rate, model[i].EnableBA));
                else
                    arr.push(autoFollowBet.renderRow(i, '', model[i].Username, model[i].Rate, model[i].EnableBA));
            }
            $("#tobdyMainGrid").html(arr.join(''));
            $("#divMainGrid").removeClass("hidden").reRenderScroller(435, gridHeight, true);
        }
        else {
            $("#tobdyMainGrid").html('<tr><td colspan="4" style="border-right:0px none"><div class="c b" style="width:369px">There is no information available</div></td></tr>');
            $("#divMainGrid").removeClass("hidden").reRenderScroller(433, gridHeight, true);
        }
        try { parent.jQuery.unblockUI(); } catch (e) { }
    },

    GetAutoFollowedBet: function () {
        autoFollowBet.status = $('#dlStatus').val();
        var data = { Status: $('#dlStatus').val() };
        try { parent.BlockUI(); } catch (e) { }
        $.ajax({
            type: "POST",
            url: "GetAutoFollowedBet",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: JSON.stringify(data),
            success: function (model) {
                var arr = [];
                if (model.length > 0) {
                    for (var i = 0; i < model.length; i++) {
                        if (i % 2 != 0)
                            arr.push(autoFollowBet.renderRow(i, 'bgeven', model[i].Username, model[i].Rate, model[i].EnableBA));
                        else
                            arr.push(autoFollowBet.renderRow(i, '', model[i].Username, model[i].Rate, model[i].EnableBA));
                    }
                    $("#tobdyMainGrid").html(arr.join(''));
                    $("#divMainGrid").removeClass("hidden").reRenderScroller(435, gridHeight, true);
                }
                else {
                    $("#tobdyMainGrid").html('<tr><td colspan="4" style="border-right:0px none"><div class="c b" style="width:369px">There is no information available</div></td></tr>');
                    $("#divMainGrid").removeClass("hidden").reRenderScroller(432, gridHeight, true);
                }
                try { parent.jQuery.unblockUI(); } catch (e) { }
            },
            complete: function () {
                try { parent.jQuery.unblockUI(); } catch (e) { }
            }

        });
    },
    renderRow: function (i, css, Username, Rate, EnableBA) {
        var idlbRate = 'lbrate' + i;
        var imgEdit = 'imgEdit' + i;
        var imgSave = 'imgSave' + i;
        var imgUndo = 'imgUndo' + i;
        var Save = 'Save';
        var Cancel = 'Cancel'
        var strrow = '';
        strrow += '<tr class="' + css + '">';
        strrow += '<td><div class="col120 c">' + Username + '</div></td>';
        strrow += '<td id="td' + idlbRate + '"><div id="' + idlbRate + '" class="col120 c">' + Rate + '</div></td>';
        if (EnableBA)
            strrow += '<td><div class="col35 c checkBA"></div></td>';
        else
            strrow += '<td><div class="col60 c"></div></td>';
        strrow += '<td>';
        strrow += '<div class="col60 c">';
        strrow += '<img id="' + imgEdit + '" src= "' + window.rootUrl + 'App_Themes/Images/icn_edit.png"   onclick="autoFollowBet.beginEdit(event, \'' + imgEdit + '\', \'' + imgSave + '\', \'' + imgUndo + '\', \'' + idlbRate + '\', \'' + Username + '\',\'' + Rate + '\')" style="cursor:pointer;" title="Edit"  />';
        strrow += '<img id="' + imgSave + '" src="' + window.rootUrl + 'App_Themes/Images/Save.png" onclick="autoFollowBet.updFollowBetCustomer( event, \'' + imgEdit + '\', \'' + imgSave + '\', \'' + imgUndo + '\',\'' + Username + '\', \'' + idlbRate + '\', \'' + Rate + '\')" style="cursor:pointer;display:none" title="Save" />';
        strrow += '<img id="' + imgUndo + '" src="' + window.rootUrl + 'App_Themes/Images/undo.png" onclick="autoFollowBet.undoEdit( event, \'' + idlbRate + '\', \'' + Rate + '\')" style="cursor:pointer;display:none" title="Cancel" />';
        strrow += '<td>';
        strrow += '<div class="col60 c">';
        strrow += '<img src="' + window.rootUrl + 'App_Themes/Images/icn_del.png" style="cursor:pointer;" onclick="autoFollowBet.delAutoFollowedBet( \'' + Username + '\')" title="Delete" /></div></td>';
        return strrow;

    },
    beginEdit: function (e, idImgEdit, idImgSave, idImgUndo, id, Username, Rate) {

        var iconEdit = $('#' + idImgEdit);
        var iconSave = $('#' + idImgSave);
        var iconUndo = $('#' + idImgUndo);
        iconEdit.hide();
        iconSave.show()
        iconUndo.show();
        var rate = $('#' + id).html();
        var lbRate = $('#td' + id);

        var tbxrate = 'tbx' + id;
        lbRate.html('<input  id="' + tbxrate + '" style="width:115px" maxlength="5" type="text" onkeyup="autoFollowBet.onKeyEnterUdp(event ,\'' + idImgEdit + '\', \'' + idImgSave + '\', \'' + idImgUndo + '\',\'' + Username + '\', \'' + id + '\', \'' + Rate + '\');" class="txt" editor="true"/>');
        $('#' + tbxrate).val(rate);
    },

    updFollowBetCustomer: function (e, idImgEdit, idImgSave, idImgUndo, username, idlbRate, Rate) {
        try { parent.BlockUI(); } catch (e) { }

        var iconEdit = $('#' + idImgEdit);
        var iconSave = $('#' + idImgSave);
        var iconUndo = $('#' + idImgUndo);
        var rate = $('#tbx' + idlbRate).val();


        if (isPositiveDouble(rate) && parseFloat(rate) > 0) {
            var data = '{"username":"' + username + '","rate":"' + rate + '"}';
            $.ajax({
                type: "POST",
                url: "UpdAutoFollowedBet",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                async: false,
                data: data,
                success: function (rs) {
                    if (parseInt(rs.ErrorCode) != 0) {
                        showAlertMessage("msgError", rs.ErrorMessage, 1);
                        $("#msgError").show().find("div:first").width(390);
                        var lbRate = $('#td' + idlbRate);
                        lbRate.html('<div id="' + idlbRate + '" class="col120 c">' + Rate + '</div>');
                        iconEdit.show();
                        iconSave.hide()
                        iconUndo.hide();
                    }
                    else {
                        showAlertMessage("msgError", "Updated Successfully!", 0);
                        $("#msgError").show().find("div:first").width(390);
                        var data = { Status: autoFollowBet.status };
                        $.ajax({
                            type: "POST",
                            url: "GetAutoFollowedBet",
                            contentType: "application/json; charset=utf-8",
                            dataType: "json",
                            data: JSON.stringify(data),
                            success: function (rs) {
                                autoFollowBet.buildGrid(rs);
                            }
                        });
                    }
                    try { parent.jQuery.unblockUI(); } catch (e) { }

                }
            });

        }
        else {
            showAlertMessage("msgError", "Invalid Rate . Please input positive double value!", 1);
            $("#msgError").show().find("div:first").width(390);
            var lbRate = $('#td' + idlbRate);
            lbRate.html('<div id="' + idlbRate + '" class="col120 c">' + Rate + '</div>');
            var target = e.target || e.srcElement;
            var tr = $(target).parents("tr:first");
            iconEdit.show();
            iconSave.hide()
            iconUndo.hide();
            try { parent.jQuery.unblockUI(); } catch (e) { }

        }
        var t = setTimeout(function () {
            $("#msgError").empty();
        }, 4000);
    },
    undoEdit: function (e, idRate, Rate) {
        var target = e.target || e.srcElement;
        var tr = $(target).parents("tr:first");
        var iconEdit = $(target).parent().find("img[title='Edit']");
        var iconSave = $(target).parent().find("img[title='Save']");
        var iconUndo = $(target).parent().find("img[title='Cancel']");
        iconEdit.show();
        iconSave.hide();
        iconUndo.hide();
        var lbRate = $('#td' + idRate);
        lbRate.html('<div id="' + idRate + '" class="col120 c">' + Rate + '</div>');

    },

    delAutoFollowedBet: function (username) {
        try { parent.BlockUI(); } catch (e) { }
        var data = '{"username":"' + username + '"}';
        if (confirm("Are you sure you want to delete followed customer?")) {
            $.ajax({
                type: "POST",
                url: "DelAutoFollowedBet",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                data: data,
                success: function (rs) {
                    if (parseInt(rs.ErrorCode) != 0) {
                        showAlertMessage("msgError", rs.ErrorMessage, 1);
                        $("#msgError").show().find("div:first").width(390);
                    }
                    else {
                        showAlertMessage("msgError", "Deleted Successfully!", 0);
                        $("#msgError").show().find("div:first").width(390);
                        var data = { Status: autoFollowBet.status };
                        $.ajax({
                            type: "POST",
                            url: "GetAutoFollowedBet",
                            data: JSON.stringify(data),
                            contentType: "application/json; charset=utf-8",
                            dataType: "json",
                            success: function (rs) {
                                autoFollowBet.buildGrid(rs);
                            }
                        });
                    }

                    var t = setTimeout(function () {
                        $("#msgError").empty();
                    }, 4000);
                }
            });
        }
        try { parent.jQuery.unblockUI(); } catch (e) { }
    },

    addNewFollewCustomer: function () {
        var username = $("#txtUserName").val();
        var rate = $("#txtRate").val();
        if (username == '') {
            showAlertMessage("errMsgDig", "Please input Username!", 1);
            $("#errMsgDig").show().find("div:first").width(350);
        }
        else if (rate == '') {
            showAlertMessage("errMsgDig", "Please input Rate!", 1);
            $("#errMsgDig").show().find("div:first").width(350);
        }
        else if (!isPositiveDouble(rate) || parseFloat(rate) <= 0) {
            showAlertMessage("errMsgDig", "Invalid Rate . Please input positive double value!", 1);
            $("#errMsgDig").show().find("div:first").width(350);
            return;
        }
        else {

            var data = '{"username":"' + username + '","rate":"' + rate + '"}';
            $.ajax({
                type: "POST",
                url: "InsAutoFollowedBet",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                async: false,
                data: data,
                success: function (rs) {
                    if (parseInt(rs.ErrorCode) != 0) {
                        showAlertMessage("errMsgDig", rs.ErrorMessage, 1);
                        $("#errMsgDig").show().find("div:first").width(350);
                    }
                    else {

                        showAlertMessage("errMsgDig", "Inserted Successfully!", 0);
                        $("#errMsgDig").show().find("div:first").width(350);
                        var data = { Status: autoFollowBet.status };
                        $.ajax({
                            type: "POST",
                            url: "GetAutoFollowedBet",
                            contentType: "application/json; charset=utf-8",
                            dataType: "json",
                            data: JSON.stringify(data),
                            success: function (rs) {
                                autoFollowBet.buildGrid(rs);
                            }
                        });

                    }
                    try { parent.jQuery.unblockUI(); } catch (e) { }

                }
            });
        }
        var t = setTimeout(function () {
            $("#errMsgDig").empty();
        }, 4000);


    },

    onKeyEnterUdp: function (e, idImgEdit, idImgSave, idImgUndo, username, idlbRate, Rate) {
        if (e.keyCode == 13) {
            autoFollowBet.updFollowBetCustomer(e, idImgEdit, idImgSave, idImgUndo, username, idlbRate, Rate);
        }
    },

    onKeyEnterAdd: function (e) {
        if (e.keyCode == 13) {
            autoFollowBet.addNewFollewCustomer();
        }
    }
};
//$(document).ready(function () {
//    autoFollowBet.buildGrid(autoFollowBet.model);
//});
function NumberGameCopyPT(type) {
    var IsCopyAll = false;
    switch (type) {
        case 'Deadball':
            IsCopyAll = $('#chkCopy1stOUD').is(':checked')
            $('.copyfromD').attr('disabled', IsCopyAll);
            CheckStatusAutoPTDeadball(IsCopyAll);
            break;
        case 'Live':
            IsCopyAll = $('#chkCopyNBOUL').is(':checked')
            $('.copyfromL').attr('disabled', IsCopyAll);
            CheckStatusAutoPTLive(IsCopyAll);
            break;
    }
}


function disable161DDL(chkCopy) {
    var IsCopyAll = chkCopy.checked;
    $('.copyfromD').attr('disabled', IsCopyAll);
    $('.copyfromL').attr('disabled', IsCopyAll);
    $('.copyAll').attr('disabled', IsCopyAll);
    $("input[name*='161']").attr('disabled', IsCopyAll);

    var remainControls = ['chkAuto1stOUD', 'chkAutoNBOUL'];
    for (i = 0; i < remainControls.length; i++) {
        if ($( '#' + remainControls[i]).length > 0)
            $('#' + remainControls[i]).attr('disabled', IsCopyAll);
    }

    //auto pt at master level
    if (!IsCopyAll) {
        NumberGameCopyPT('Deadball');
        NumberGameCopyPT('Live');

        var chks = ['chkAuto1stOUD', 'chkAutoNBOUL'];
        var ddlSuperPreset = ['listS1stOUD', 'listSNBOUL'];
        var ddlSutoSuperPreset = ['autoS1stOUD', 'autoSNBOUL'];
        for (i = 0; i < chks.length; i++) {
            CheckAutoPT(chks[i], ddlSuperPreset[i], ddlSutoSuperPreset[i]);
        }
    }
}

function CheckAutoPT(chk, presetDDL, autopresetDDL) {
    var IsAuto = chk.checked;

    if (IsAuto == undefined) {
        IsAuto = $('#' + chk).is(':checked');
    }
    
    $('#' + presetDDL).attr('disabled', IsAuto);
    $('#' + autopresetDDL).attr('disabled', !IsAuto);
}

function CheckStatusAutoPTDeadball(IsCopyAll) {
    var chks = ['chkAuto1stOED', 'chkAuto1stWarriorD', 'chkAuto1stWaFTD', 'chkAuto1stOEFTD'];
    var ddlSuperPreset = ['listS1stOED', 'listS1stWarriorD', 'listS1stWaFTD', 'listS1stOEFTD'];
    var ddlSutoSuperPreset = ['autoS1stOED', 'autoS1stWarriorD', 'autoS1stWaFTD', 'autoS1stOEFTD'];
    for (i = 0; i < chks.length; i++) {
        if ($('#' + chks[i]).length > 0) {
            $('#' + ddlSuperPreset[i]).attr('disabled', $('#' + chks[i]).is(':checked') || IsCopyAll );
            $('#' + ddlSutoSuperPreset[i]).attr('disabled', !$('#' + chks[i]).is(':checked') || IsCopyAll);
            
            if(!IsCopyAll)
            {
                CheckAutoPreset(obj(chks[i]), 'chkAuto1stOED');
            } 
        }
    }
}

function CheckStatusAutoPTLive(IsCopyAll) {
    var chks = ['chkAutoNBOEL', 'chkAutoHLL', 'chkAutoNCL', 'chkAutoNGL'];
    var ddlSuperPreset = ['listSNBOEL', 'listSHLL', 'listSNCL', 'listSNGL'];
    var ddlSutoSuperPreset = ['autoSNBOEL', 'autoSHLL', 'autoSNCL', 'autoSNGL'];
    for (i = 0; i < chks.length; i++) {        
        if ($('#' + chks[i]).length > 0) {
            $('#' + ddlSuperPreset[i]).attr('disabled', $('#' + chks[i]).is(':checked') || IsCopyAll);
            $('#' + ddlSutoSuperPreset[i]).attr('disabled', !$('#' + chks[i]).is(':checked') || IsCopyAll);
            
            if(!IsCopyAll)
            {
                CheckAutoPreset(obj(chks[i]), 'chkCopyNBOUL');
            }
        }
        
    }
}

function CheckAutoPresetOthersNB(chkIsAuto, autoDDL, chkCopy)
{
    var IsAuto = chkIsAuto.checked;
    $('#' + autoDDL).attr('disabled', !IsAuto);
    $('#' + chkCopy).attr('disabled', !IsAuto);
    
    if(!IsAuto)
        $('#' + chkCopy).attr('checked', false);
        
    NumberGameCopyPT('Deadball');
    NumberGameCopyPT('Live');
}

function CheckMaxPerBall161() {
    
    if (!obj('txtMinbet161')) return true;

    $('#errMsgball161').html('');
    
    if ($('#txtMaxPerBall').val() == '') {
        showAlertMessage("errMsgball161", "Incorrect max per ball", 1);
        $('#txtMaxPerBall').focus();
        return false;
    }
    var RoleId = parseInt($('#RoleId').val());
    var Maxbet161 = 0, MaxPerMatch161 = 0, MaxPerBall161 = 0, hdMaxPerBall161 = 0, actualMaxPerBall161 = 0;
    var hdMinPerBall161 = 0, actualMinPerBall161 = 0;

    hdMaxPerBall161 = parseFloat($('#hdMaxPerBall').val().replace(/,/g, ''));
    hdMinPerBall161 = parseFloat($('#hdMinPerBall').val().replace(/,/g, ''));
    MaxPerBall161 = parseFloat($('#txtMaxPerBall').val().replace(/,/g, ''));

    if ($('#chkCopyAll').is(':checked')) {
        Maxbet161 = parseFloat($('#txtMaxbet1').val().replace(/,/g, ''));
        MaxPerMatch161 = parseFloat($('#txtMaxPerMatch1').val().replace(/,/g, ''));
    }
    else if ($('#chkCopyBasket').is(':checked')) {
        Maxbet161 = parseFloat($('#txtMaxbet2').val().replace(/,/g, ''));
        MaxPerMatch161 = parseFloat($('#txtMaxPerMatch2').val().replace(/,/g, ''));
    }
    else {
        Maxbet161 = parseFloat($('#txtMaxbet161').val().replace(/,/g, ''));
        MaxPerMatch161 = parseFloat($('#txtMaxPerMatch161').val().replace(/,/g, ''));
    }

    if (Maxbet161 == 0 && MaxPerMatch161 == 0 && MaxPerBall161 == 0) return true;

//    if (hdMaxPerBall161 == 0)
//        hdMaxPerBall161 = MaxPerMatch161;
    if(hdMaxPerBall161 == 0)
        return true;

    if (MaxPerBall161 > hdMaxPerBall161) 
    {
        showAlertMessage("errMsgball161", "Max per ball must be less than or equal to " + hdMaxPerBall161, 1);
        $('#txtMaxPerBall').focus();
        return false;
    }

    return true;
}

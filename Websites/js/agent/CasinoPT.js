//Game Kind definition
var LiveGame = 8;
var RnG = 9;
var P2P = 10;
var LiveGameName = "Live Game";
var RnGName = "RnG Game";
var P2PName = "P2P";

var arrGameKindName = []
var arrLiveGameType = []; //Add new game type to this array

//RnG
var arrRnGGameType = [];
var arrRngGGameName = ["RNG Baccarat", "Casino War", "BlackJack", "Pontoon", "Three Card Poker", "Casino Hold 'Em", "Pai Gow", "Roulette", "Mini Roulette", "Roulette Pro", "Sic Bo", "Jacks or Better", "Bonus Bear", "A Night Out"];
//P2P
var arrP2PGameType = [];
var arrGameKindID = [];

var arrGameKind = [];
//List of all game type
var groupGameType = 0;
var strChkGroupSetting = 'ChkGroupSetting';
var strChkAuto = 'ChkAuto';
var strChkSAutoPT = 'ChkSAuto';

var SPT = 'SPre', strSPT = "Super Preset PT"; ;
var SMinPT = 'SMin', strSMinPT = "Super Min PT";
var SMaxPT = 'SMax', strSMinPT = "Super Max PT";
var SAutoPT = 'SAuto', strSAutoPT = "Super Auto PT";
var MPT = "MPre", strMPT = "Master Preset PT";
var MMaxPT = "MMax", strMMaxPT = "Master Max PT";
var MMinPT = "MMin", strMMinPT = "Master Min PT";
var APT = "APre", strAPT = "Agent Preset PT";
var AMaxPT = "AMax", strAMaxPT = "Agent Max PT";
var AMinPT = "AMin", strAMinPT = "Agent Min PT";

var Comm = "Comm", strComm = "Commission";
var SComm = "SComm", strSComm = "Super Commision";
var MComm = "MComm", strMComm = "Master Commision";
var AComm = "AComm", strAComm = "Agent Commision";
var arrPT = [SPT, SMinPT, SAutoPT, MMaxPT, MMinPT, AMaxPT, AMinPT, Comm, SComm, MComm, AComm, MPT, APT];
var arrPTName = [strSPT, strSMinPT, strSAutoPT, strMMaxPT, strMMinPT, strAMaxPT, strAMinPT, strComm, strSComm, strMComm, strAComm, strMPT, strAPT];

var LM1 = 'LM1';
var LM2 = 'LM2';
var LM3 = 'LM3';
var LM = 'LM';
var GroupSettingType = 0;
var sepChar = '_';
var err = "error";
var errMaxValue = "*";
var errGroupSettingValue = "Group value is larger than max value of game type";
var TXT_PREFIX = "txt";
//
//Hashtable game type
var hGameType = new Hashtable();
var hPTType = new Hashtable();
hPTType = GenHashPTType();
//
var hRowSpan = new Hashtable();

//define button
var BtnInc_1 = "BtnInc_1";
var BtnDec_10 = "BtnDec_10";
var BtnDec_1 = "BtnDec_1";
var BtnInc_10 = "BtnInc_10";

var BTN_INC1_CLASS = "btn-inc1";
var BTN_INC10_CLASS = "btn-inc10";
var BTN_DEC1_CLASS = "btn-dec1";
var BTN_DEC10_CLASS = "btn-dec10";

var BTN_INC1_DISABLE_CLASS = "btn-inc1-disable";
var BTN_INC10_DISABLE_CLASS = "btn-inc10-disable";
var BTN_DEC1_DISABLE_CLASS = "btn-dec1-disable";
var BTN_DEC10_DISABLE_CLASS = "btn-dec10-disable";

//define hidden
var HIDDEN_RoldID = "hidRoleId";
var HIDDEN_PT = "hidPT";
var HIDDEN_PT_BY_GAMETYPE = "hidPTByGameType";
var CLASS_BTN_DISABLED = "";
var CLASS_BTN_ENABLED = "btn_hand";
///////////////////////////

//Attribute
var attrDisabled = "disabled";
var attrSelected = "selected";
/////////////////////////////

///prefix
var PREFIX_OPTIONS = "opt";

//Const value
var errNo = 0;
var CONST_PT_INT = 1000;
var CONST_PT_INT10 = 10000;
var CONST_PT_MAX = 0.9;
var s4 = 0.9, s3 = 0.9, s1 = 0.9, m3 = 0.9, m2 = 0.9, m1 = 0.9, a2 = 0.9, a1 = 0.9;
var u3 = 0.9, n4 = 0, n3 = 0, n2 = 0, l4 = 0.9, l3 = 0.9, l2 = 0.9;
var s4Max = 0.9, s3Max = 0.9, s1Max = 0.9, m3Max = 0.9, m2Max = 0.9, m1Max = 0.9, a2Max = 0.9, a1Max = 0.9;
var u3Max = 0.9, n4Max = 0, n3Max = 0, n2Max = 0, l4Max = 0.9, l3Max = 0.9, l2Max = 0.9;
var m3MaxAuto = 0.9, n3MaxAuto = 0.9;
var interval_PT = 100;
var interval_Comm = 25;
var str_m3MaxAuto = "m3MaxAuto", str_n3MaxAuto = "n3MaxAuto";

var SEP_PT = "|";
var SEP_PT_VALUE = ":";

function GenHashPTType() {
    var h = new Hashtable();
    for (var i = 0; i < arrPT.length; i++) {
        h.put(arrPT[i], arrPTName[i]);
    }
    return h;
}


function GetGameTypeName(key) {
    return hGameType.get(key);
}

function GetPTTypeName(key) {
    return hPTType.get(key);
}
//level 4: super, 3: master, 2: agent, 1: member
function CheckGroupSetting(chkGroupSetting, level) {
    level = (level == null) ? 4 : level;
    var gameKind = chkGroupSetting.id.split("_")[0];
    var isGroupSetting = chkGroupSetting.checked;
    obj(HIDDEN_PT_BY_GAMETYPE).value = (isGroupSetting) ? "False" : "True";
    var arrGameType = GetGameTypeList(parseInt(gameKind));
    
    switch (level) {
        case 1: //member
            break;
        case 2: //agent
            break;
        case 3: //master
            GroupSettingMaster(gameKind, isGroupSetting, arrGameType);
            break;
        default: //super
            GroupSettingSuper(isGroupSetting);
            break;
    }
    //check auto pt

}

//Group setting for super
function GroupSettingSuper(isGroupSetting) {
    for (var i = 0; i < arrGameKindID.length; i++) {
        var gameKind = arrGameKindID[i];
        $("select[id^='" + gameKind + "']").not("select[id^='" + gameKind + sepChar + groupGameType + "']").attr('disabled', isGroupSetting);
        //ChangeGroupButtonPTStatusByChkGroupSetting(gameKind, !isGroupSetting);
        if(isGroupSetting == true)
        {
           $("[id^='tr_']").css("display", "none");
           $("[id^='td_{gametypeid}']").css("display", "");
           $("[id^='td_" + gameKind +"']").attr("rowspan", hRowSpan.get("rowspan" + sepChar + gameKind + sepChar + "usegroup"));

        }
        else
        {
            $("[id^='tr_']").css("display", "");
            $("[id^='td_{gametypeid}']").css("display", "none");
            $("[id^='td_" + gameKind +"']").attr("rowspan", hRowSpan.get("rowspan" + sepChar + gameKind));

        }
    }

}

//Group setting master
function GroupSettingMaster(gameKind, isGroupSetting, arrGameType, isParentGroupSetting) {
    isParentGroupSetting = (isParentGroupSetting == null) ? false : isParentGroupSetting;
    if (isParentGroupSetting == false) {
        var useGroupAutoPT = (!isGroupSetting || $("input[type=checkbox][id^=" + gameKind + sepChar + groupGameType + sepChar + strChkAutoSPT + "]").attr('checked'));
        $("select[id^='" + gameKind + sepChar + groupGameType + sepChar + SPT + "']").attr('disabled', (!isGroupSetting || useGroupAutoPT));
        $("select[id^='" + gameKind + sepChar + groupGameType + sepChar + SAutoPT + "']").attr('disabled', (!isGroupSetting || !useGroupAutoPT));
    }

}

function ChangePT(ctrBtn, value) {
    var arrControlInfo = ctrBtn.id.split(sepChar);
    var gameKind = arrControlInfo[0];
    var gameType = arrControlInfo[1];
    var ptType = arrControlInfo[2];
    var hidRole = obj(HIDDEN_RoldID);
    var level = (hidRole == null) ? 4 : parseInt(hidRole.value);
    var selectID = gameKind + sepChar + groupGameType + sepChar + ptType;
    var objSelect = obj(selectID);
    if (objSelect != null) {
        var selectedIndex = objSelect.selectedIndex;
        var groupValue = parseFloat(objSelect.options[selectedIndex].value);
        groupValue = (parseInt(groupValue * CONST_PT_INT) + (value * CONST_PT_INT)) / CONST_PT_INT;
        if (groupValue >= 0 && groupValue <= CONST_PT_MAX) {
            objGroupSelect = obj(selectID + sepChar + PREFIX_OPTIONS + sepChar + groupValue);
            if (objGroupSelect != null) {
                objGroupSelect.selected = true;
                OnGroupSettingChange(objSelect, level);
            }
        }
    }
}

//Group setting select change
function OnGroupSettingChange(ctrSelect, level) {
    var arrControlInfo = ctrSelect.id.split(sepChar);
    var gameKind = arrControlInfo[0];
    var gameType = arrControlInfo[1];
    var ptType = arrControlInfo[2];
    var useGroupSetting = false;
    if (obj(gameKind + sepChar + strChkGroupSetting) != null)
        useGroupSetting = obj(gameKind + sepChar + strChkGroupSetting).checked;
    var arrGameType = GetGameTypeList(parseInt(gameKind));
    var ctrSelect = obj(ctrSelect.id);
    var groupValue = ctrSelect.options[ctrSelect.selectedIndex].value;
    //change group value
    ChangePresetPT(obj(gameKind + sepChar + groupGameType + sepChar + ptType));

    for (var i = 0; i < arrGameType.length; i++) {
        var objError = obj(gameKind + sepChar + arrGameType[i] + sepChar + ptType + sepChar + err);
        var objSelectedOption = obj(gameKind + sepChar + arrGameType[i] + sepChar + ptType + sepChar + PREFIX_OPTIONS + sepChar + groupValue);
        if(objError) objError.innerHTML = "";
        if (objSelectedOption == null) {
            if (!useGroupSetting) {
                if (objError) 
                    objError.innerHTML = errMaxValue;
            }
            errNo++;
        }
        else { //existed a same value with group value
            errNo--;
            objSelectedOption.selected = true;
            var objSelect = obj(gameKind + sepChar + arrGameType[i] + sepChar + ptType);
            ChangePresetPT(objSelect);
        }
    }
    if (errNo > 0) {
        if (!useGroupSetting)
        //obj("lblError").innerHTML = errGroupSettingValue;
            showAlertMessage("lblError", errGroupSettingValue, 1);
    }


}

//Check auto
function CheckGroupAuto(ctrChkGroupAuto, useGroupSetting) {
    var arrControlInfo = ctrChkGroupAuto.id.split(sepChar);
    var gameKind = arrControlInfo[0];
    var gameType = arrControlInfo[1];
    var ptType = arrControlInfo[2];
    var useGroupAuto = obj(gameKind + sepChar + groupGameType + sepChar + strChkSAutoPT).checked;
    useGroupSetting = (useGroupSetting == null) ? false : useGroupSetting;

    var arrGameType = GetGameTypeList(parseInt(gameKind));
    CheckAuto(gameKind + sepChar + groupGameType + sepChar + strChkSAutoPT, useGroupSetting);
    ChangeGroupButtonPTStatus(gameKind, gameType, SPT, useGroupAuto);
    ChangeGroupButtonPTStatus(gameKind, gameType, SAutoPT, !useGroupAuto);
    //if (!useGroupSetting) {
        for (var i = 0; i < arrGameType.length; i++) {
            var chkAutoID = gameKind + sepChar + arrGameType[i] + sepChar + strChkSAutoPT;
            var objChkAuto = obj(chkAutoID);
            if (objChkAuto != null) {
                objChkAuto.checked = useGroupAuto;
                CheckAuto(chkAutoID, useGroupSetting);
            }
        } 
    //}
}

function CheckAuto(chkAutoControlID, useGroupSetting) {
    if (typeof (chkAutoControlID.id) == "undefined")
        chkAutoControlID = chkAutoControlID;
    else
        chkAutoControlID = chkAutoControlID.id;
    var arrControlInfo = chkAutoControlID.split(sepChar);
    var gameKind = arrControlInfo[0];
    var gameType = arrControlInfo[1];
    var autoType = arrControlInfo[2];
    useGroupSetting = (useGroupSetting == null) ? false : useGroupSetting;
    var useAuto = obj(gameKind + sepChar + gameType + sepChar + strChkSAutoPT).checked;
    if (gameType != groupGameType) {
        obj(gameKind + sepChar + gameType + sepChar + SPT).disabled = (useAuto || useGroupSetting);
        obj(gameKind + sepChar + gameType + sepChar + SAutoPT).disabled = (!useAuto || useGroupSetting);
    }
    else {
        obj(gameKind + sepChar + gameType + sepChar + SPT).disabled = useAuto;
        obj(gameKind + sepChar + gameType + sepChar + SAutoPT).disabled = !useAuto;
    }
    var level = 3;
    if (autoType == strChkSAutoPT) {
        level = 3
    }
    else {
        level = 2;
    }

    if (level == 3) { //super auto on master
        if (useAuto) {
            m3MaxAuto = GetPTValue(gameKind + sepChar + gameType + sepChar + MPT + sepChar + str_m3MaxAuto);
            n3MaxAuto = m3MaxAuto;
            //alert(n3MaxAuto);
            var objMPT = obj(gameKind + sepChar + gameType + sepChar + MPT);
            var objMMinPT = obj(gameKind + sepChar + gameType + sepChar + MMinPT);
            if (objMPT != null) {
                //ClearSelectOptions(objMMaxPT);
                var currentValue = objMPT.options[objMPT.selectedIndex].value;
                SelectOptionsBinding(objMPT, m3MaxAuto, currentValue, interval_PT);
            }
            if (objMMinPT != null) {
                var currentMMinPT = objMMinPT.options[objMMinPT.selectedIndex].value;
                currentMMinPT = Math.min(currentMMinPT, n3MaxAuto);
                SelectOptionsBinding(objMMinPT, n3MaxAuto, currentMMinPT, interval_PT);
            }
        }
        else {
            var objSPT = obj(gameKind + sepChar + gameType + sepChar + SPT);
            if (objSPT != null) {
                ChangePresetPT(objSPT);
            }
        }
    }

}

//change group button status follow the auto pt check box
function ChangeGroupButtonPTStatus(gameKind, gameType, ptType, disabled) {
    var btnInc1 = gameKind + sepChar + gameType + sepChar + ptType + sepChar + BtnInc_1;
    var btnInc10 = gameKind + sepChar + gameType + sepChar + ptType + sepChar + BtnInc_10;
    var btnDec1 = gameKind + sepChar + gameType + sepChar + ptType + sepChar + BtnDec_1;
    var btnDec10 = gameKind + sepChar + gameType + sepChar + ptType + sepChar + BtnDec_10;

    var Btn = "Btn";
    var groupBtnID = gameKind + sepChar + gameType + sepChar + ptType + sepChar + Btn;

    $("input[type=button][id^=" + groupBtnID + "]").attr(attrDisabled, disabled);

    $("#" + btnInc1).removeClass((disabled) ? CLASS_BTN_ENABLED : CLASS_BTN_DISABLED);
    $("#" + btnInc10).removeClass((disabled) ? CLASS_BTN_ENABLED : CLASS_BTN_DISABLED);
    $("#" + btnDec1).removeClass((disabled) ? CLASS_BTN_ENABLED : CLASS_BTN_DISABLED);
    $("#" + btnDec10).removeClass((disabled) ? CLASS_BTN_ENABLED : CLASS_BTN_DISABLED);

    $("#" + btnInc1).addClass((disabled) ? CLASS_BTN_DISABLED : CLASS_BTN_ENABLED);
    $("#" + btnInc10).addClass((disabled) ? CLASS_BTN_DISABLED : CLASS_BTN_ENABLED);
    $("#" + btnDec1).addClass((disabled) ? CLASS_BTN_DISABLED : CLASS_BTN_ENABLED);
    $("#" + btnDec10).addClass((disabled) ? CLASS_BTN_DISABLED : CLASS_BTN_ENABLED);
}

//change preset pt event
function ChangePresetPT(objSelect) {
    var arrInfo = objSelect.id.split(sepChar);
    var gameKind = arrInfo[0];
    var gameType = arrInfo[1];
    var presetPTType = arrInfo[2];
    var PTType = "";
    var minPTType = "";
    var level = 3;
    var objSMinPT = obj(gameKind + sepChar + gameType + sepChar + SMinPT);
    var objAMinPT = obj(gameKind + sepChar + gameType + sepChar + AMinPT);
    var objMMinPT = obj(gameKind + sepChar + gameType + sepChar + MMinPT);
    if (objSMinPT != null) {
        level = 4;
        PTType = SPT;
        minPTType = SMinPT;
    }
    else if (objMMinPT != null) {
        PTType = MPT;
        minPTType = MMinPT;
        level = 3;
    }
    else if (objAMinPT != null) {
        PTType = APT;
        minPTType = AMinPT;
        level = 2;
    }
    //alert(level);
    var presetPTId = gameKind + sepChar + gameType + sepChar + PTType;
    var minPTId = gameKind + sepChar + gameType + sepChar + minPTType;
    var objPresetPT = obj(presetPTId);
    var objMinPT = obj(minPTId);

    if (level == 4) { //edit super
        if (presetPTType == SPT) { //change super max pt
            objErrSPT = obj(gameKind + sepChar + gameType + sepChar + SPT + sepChar + err);
            objErrSMinPT = obj(gameKind + sepChar + gameType + sepChar + SMinPT + sepChar + err);
            if (objErrSPT != null)
                objErrSPT.innerHTML = "";
            if (objErrSMinPT != null)
                objErrSMinPT.innerHTML = "";
            s4 = objSelect.options[objSelect.selectedIndex].value * CONST_PT_INT / CONST_PT_INT;
            n4Max = s4;
            n4 = objSMinPT.options[objSMinPT.selectedIndex].value * CONST_PT_INT / CONST_PT_INT;
            n4 = (n4 > n4Max) ? 0 : n4;
            SelectOptionsBinding(objSMinPT, n4Max, n4, interval_PT);
        }
    }
    else if (level == 3) { //super on master
        if (presetPTType == SPT) { //change super pt
            objErrMPT = obj(gameKind + sepChar + gameType + sepChar + MPT + sepChar + err);
            objErrMMinPT = obj(gameKind + sepChar + gameType + sepChar + MMinPT + sepChar + err);
            if (objErrMPT != null)
                objErrMPT.innerHTML = "";
            if (objErrMMinPT != null)
                objErrMMinPT.innerHTML = "";
            s4 = parseFloat(objSelect.options[0].value);
            s3 = parseFloat(objSelect.options[objSelect.selectedIndex].value);
            m3Max = (parseInt(s4 * CONST_PT_INT) - parseInt(s3 * CONST_PT_INT)) / CONST_PT_INT;
            n3Max = m3Max;
            n3 = 0;
            
            if (objPresetPT == null)
                return;
            var objChkAuto = obj(gameKind + sepChar + gameType + sepChar + strChkSAutoPT);
            var isAuto = (objChkAuto == null) ? false : objChkAuto.checked;
            if (isAuto)
                return; //Master does not change value when use auto
            var currentPresetPT = objPresetPT.options[objPresetPT.selectedIndex].value;
            var currentMinPT = objMinPT.options[objMinPT.selectedIndex].value;

            currentPresetPT = Math.min(currentPresetPT, m3Max);
            currentMinPT = Math.min(currentMinPT, n3Max);
            currentPresetPT = (currentPresetPT == 0) ? m3Max : currentPresetPT;
            SelectOptionsBinding(objPresetPT, m3Max, currentPresetPT, interval_PT)
            SelectOptionsBinding(objMinPT, n3Max, currentMinPT, interval_PT);
        }
        else if (presetPTType == MPT) //change master pt
        {
            objErrMPT = obj(gameKind + sepChar + gameType + sepChar + MPT + sepChar + err);
            objErrMMinPT = obj(gameKind + sepChar + gameType + sepChar + MMinPT + sepChar + err);
            if (objErrMPT != null)
                objErrMPT.innerHTML = "";
            if (objErrMMinPT != null)
                objErrMMinPT.innerHTML = "";
            m3 = parseFloat(objSelect.options[objSelect.selectedIndex].value);
            n3Max = m3;
            if (objMMinPT != null) {
                n3 = parseFloat(objMMinPT.options[objMMinPT.selectedIndex].value);
                n3 = (n3 > n3Max) ? 0 : n3;
                SelectOptionsBinding(objMinPT, n3Max, n3, interval_PT);
            }
        }
    }
    else if (level == 2) { //master on agent
        if (presetPTType == MPT) //change master pt
        {
            objErrAPT = obj(gameKind + sepChar + gameType + sepChar + APT + sepChar + err);
            objErrMMinPT = obj(gameKind + sepChar + gameType + sepChar + MMinPT + sepChar + err);
            if (objErrAPT != null)
                objErrAPT.innerHTML = "";
            if (objErrMMinPT != null)
                objErrMMinPT.innerHTML = "";
            m3 = parseFloat(objSelect.options[0].value);
            m2 = parseFloat(objSelect.options[objSelect.selectedIndex].value);
            a2Max = (parseInt(m3 * CONST_PT_INT) - parseInt(m2 * CONST_PT_INT)) / CONST_PT_INT;
            n2Max = a2Max;
            n2 = 0;
            if (objPresetPT == null)
                return;
            var currentPresetPT = objPresetPT.options[objPresetPT.selectedIndex].value;
            var currentMinPT = objMinPT.options[objMinPT.selectedIndex].value;

            //currentPresetPT = Math.min(currentPresetPT, a2Max);
            currentMinPT = Math.min(currentMinPT, n2Max);
            //currentPresetPT = (currentPresetPT == 0) ? m3Max : currentPresetPT;
            SelectOptionsBinding(objPresetPT, a2Max, a2Max, interval_PT)
            SelectOptionsBinding(objMinPT, n2Max, currentMinPT, interval_PT);
        }
        else if (presetPTType == APT) //change agent pt
        {
            objErrAPT = obj(gameKind + sepChar + gameType + sepChar + APT + sepChar + err);
            objErrMMinPT = obj(gameKind + sepChar + gameType + sepChar + MMinPT + sepChar + err);
            if (objErrAPT != null)
                objErrAPT.innerHTML = "";
            if (objErrMMinPT != null)
                objErrMMinPT.innerHTML = "";
            a2 = parseFloat(objSelect.options[objSelect.selectedIndex].value);
            n2Max = a2;
            n2 = parseFloat(objAMinPT.options[objAMinPT.selectedIndex].value);
            n2 = (n2 > n2Max) ? 0 : n2;
            SelectOptionsBinding(objMinPT, n2Max, n2, interval_PT); //binding agent min pt
        }
    }
}

//change group button status follow the group setting check box
function ChangeGroupButtonPTStatusByChkGroupSetting(gameKind, disabled) {
    var groupBtnID = gameKind + sepChar + groupGameType + sepChar;
    $("input[type=button][id^=" + groupBtnID + "]").attr(attrDisabled, disabled);
    $("input[type=button][id^=" + groupBtnID + "]").removeClass((disabled) ? CLASS_BTN_ENABLED : CLASS_BTN_DISABLED);
    $("input[type=button][id^=" + groupBtnID + "]").addClass((disabled) ? CLASS_BTN_DISABLED : CLASS_BTN_ENABLED);
}

function ClearSelectOptions(objSelect) {
    if (objSelect != null) {
        for (var i = objSelect.options.length - 1; i >= 0; i--) {
            objSelect.remove(i);
        }
    }
}

function SelectOptionsBinding(objSelect, maxValue, currentValue, interval) {
    if (objSelect != null) {

        ClearSelectOptions(objSelect);

        var groupValue = Math.round(maxValue * CONST_PT_INT10);
        if (currentValue > maxValue) {
            currentValue = maxValue;
        }
        while (groupValue >= 0) {
            var selValue = groupValue / CONST_PT_INT10;

            var id = objSelect.id + sepChar + PREFIX_OPTIONS + sepChar + selValue;
            if (currentValue == selValue) {
                objSelect.options.add(CreateOption(selValue, id, true));
            }
            else {

                objSelect.options.add(CreateOption(selValue, id, false));
            }
            groupValue = groupValue - interval;

        }
    }
}


function CreateOption(value, id, selected) {
    var objOption = document.createElement("OPTION");
    objOption.text = value;
    objOption.value = value;
    objOption.id = id;
    if (selected == true)
        objOption.selected = true;
    return objOption;
}

//Change max pt handle
function ChangeMaxPT(objSelect)
{
}

function GetPTValue(ptKey) {
    try {
        var objHidPTData = obj(HIDDEN_PT);
        if (objHidPTData != null) {
            var ptData = objHidPTData.value;
            var arrPTData = ptData.split(SEP_PT);

            for (var i = 0; i < arrPTData.length; i++) {
                if (arrPTData[i] != "" && arrPTData[i] != null) {
                    var arrData = arrPTData[i].split(SEP_PT_VALUE);
                    if (arrData[0] == ptKey)
                        return parseFloat(arrData[1]);
                }
            }
        }
        return 0;
    }
    catch (error) {
        alert("error GetPT");
    }
}

var ID_CHECKBOX_DISABLEDCASINO_OLDVALUE = -1;
function MarkChangeStatus()
{
    var cbxDisabled = obj(ID_CHECKBOX_DISABLEDCASINO);
    var cbxDisabledDL = obj(ID_CHECKBOX_DISABLEDCASINO_DL);
    var hidUpdDownline = obj(strHidUpdDownline);
    var spnDownlineOPT = obj(ID_SPAN_DOWNLINE_OPT);
    var disabledCS = (cbxDisabledDL==null);

    if(hidUpdDownline)
        hidUpdDownline.value = "-1"; // Mark as changed
    
    if(cbxDisabled.checked)
    {
        if(hidUpdDownline)
            hidUpdDownline.value = "-1"; // Mark as changed
        if(spnDownlineOPT)
                spnDownlineOPT.style.visibility = (!disabledCS)?'visible':'hidden';
    }
    else
    {
        if(hidUpdDownline)
            hidUpdDownline.value = "-1"; // Mark as changed
        if(spnDownlineOPT)
            spnDownlineOPT.style.visibility = (disabledCS)?'visible':'hidden';
    }
}

function CheckConfirmUpdDownline()
{
    if(obj(strHidUpdDownline) && obj(ID_CHECKBOX_DISABLEDCASINO_DL))
    obj(strHidUpdDownline).value = obj(ID_CHECKBOX_DISABLEDCASINO_DL).checked;
    BtnOKClick();
}

function GetGameTypeList(gameKindId) {
    var arrGameType = [];
    
    for (var i = 0; i < arrGameKindID.length; i++) {

        if (arrGameKindID[i] == gameKindId) {
            arrGameType = arrGameKind[i];
            break;
        }
    }
    return arrGameType;
}

function obj(id) {
    return document.getElementById(id);
}

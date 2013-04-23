var arrSport = [201];
var arrBettype201 = [];
var sporttype_FI = 201;
var strChkAuto = 'isauto';
var strChkSAutoPT = 'ChkSAutoPT';
var strChkCopyFrom = "chkCopy";
var SPT = 'spositiontaking', strSPT = "Super Preset PT";
var ISUPLINEDISABLED = "uplinedisablefi", ISDISABLED = "disablefinancial", ISDOWNLINEDISABLED = "downlinedisablefi";
var SMinPT = '', strSMinPT = "Super Min PT";
var SMaxPT = 'SMaxPT', strSMinPT = "Super Max PT";
var SAutoPT = 'SAutoPT', strSAutoPT = "Super Auto PT";
var MPT = "mpositiontaking", strMPT = "Master Preset PT";
var MMaxPT = "MMaxPT", strMMaxPT = "Master Max PT";
var MMinPT = "MMinPT", strMMinPT = "Master Min PT";
var APT = "apositiontaking", strAPT = "Agent Preset PT";
var AMaxPT = "AMaxPT", strAMaxPT = "Agent Max PT";
var AMinPT = "AMinPT", strAMinPT = "Agent Min PT";
var MINPT = "min";
var MAXPT = "max";

var Auto = "auto";
var strIsAuto = "isauto";

var MSG_UpdatedSuccessful = "Updated Successfully";

var arrPTType = [];

var PREFIX_OPTIONS = "opt";

var PREFIX_SPORTTYPE = "v";

// Attribute
var attrDisabled = "disabled";
var attrSelected = "selected";
// ///////////////////////////
var uplineid = 0;
var custid = 0;
var roleid = 0;
var insert = false;

var BtnInc_1 = "BtnInc_1";
var BtnDec_10 = "BtnDec_10";
var BtnDec_1 = "BtnDec_1";
var BtnInc_10 = "BtnInc_10";
var CLASS_BTN_DISABLED = "";
var CLASS_BTN_ENABLED = "btn_hand";

// Const value
var errNo = 0;
var CONST_PT_INT = 1000;
var CONST_PT_INT10 = 10000;
var CONST_PT_MAX = 0.5;
var s4 = 0.9, s3 = 0.9, s1 = 0.9, m3 = 0.9, m2 = 0.9, m1 = 0.9, a2 = 0.9, a1 = 0.9;
var u3 = 0.9, n4 = 0, n3 = 0, n2 = 0, l4 = 0.9, l3 = 0.9, l2 = 0.9;
var s4Max = 0.9, s3Max = 0.9, s1Max = 0.9, m3Max = 0.9, m2Max = 0.9, m1Max = 0.9, a2Max = 0.9, a1Max = 0.9;
var u3Max = 0.9, n4Max = 0, n3Max = 0, n2Max = 0, l4Max = 0.9, l3Max = 0.9, l2Max = 0.9;

var delimiter = "_";
var delimiter_response = "|";
var interval_PT = 100;
// 0.01
var interval_Comm = 25;
var hPTData = new Hashtable();

var newspositiontaking = 0;
var updateallmembers = 0;
var str_newspositiontaking = "newspositiontaking";
var str_updateallmembers = "updateallmembers";
var memberPTMax = 0;
var msPTMax = 0;
var mmPTMax = 0;
var str_MmPTMax = "mmPTMax";
var str_MsPTMax = "msPTMax";
var bettypeMemberPTMax = 3;
var BETTYPE_COPYFROM = 3;
var isUseAuto = 0;
var isOpenPopUp = 0;
var disableMinPTFI = false;
var isAutoMaster = false;

var STR_AGENT = "Agent";
var STR_MASTER = "Master";
// check super auto
function CheckAuto(objChkAuto) {
    if (objChkAuto == null)
        return;
    if (objChkAuto.id == null || objChkAuto.id == "")
        alert("Auto checkbox got an empty id");
    var arrInfo = objChkAuto.id.split(delimiter);
    var sporttype = arrInfo[0];
    var bettype = arrInfo[1];
    var ptType = arrInfo[2];
    var isAuto = objChkAuto.checked;
    // alert(hPTData.get(objChkAuto.id));
    if (!objChkAuto.checked && hPTData.get(objChkAuto.id) == 1) {
        isOpenPopUp = 1;
    }
    if (roleid == 1) // super and master auto on member
    {
        var objPT = obj(sporttype + delimiter + bettype + delimiter + ptType);
        var objPTAuto = obj(sporttype + delimiter + bettype + delimiter
				+ ptType + delimiter + Auto);
        if (objPT == null || objPTAuto == null)
            return;
        if (objPT.nodeName.toString() != "LABEL")
            objPT.disabled = isAuto;
        objPTAuto.disabled = !isAuto;
        if (!isAuto) {
            var maxSAuto = objPTAuto.options[0].value;
            BindSelectOptions(objPTAuto, maxSAuto, 0, 0, interval_PT);
        }

        ChangeGroupButtonPTStatus(objPT.id, isAuto);
        ChangeGroupButtonPTStatus(objPTAuto.id, !isAuto);

        // check master (agent) auto
        var uplineType = (isAutoMaster == true) ? STR_AGENT : STR_MASTER;
        var isAutoUpline = hPTData.get(sporttype + delimiter + bettype
				+ delimiter + strIsAuto + delimiter + uplineType)

        if (isAutoUpline == true) {
            if (bettype == BETTYPE_COPYFROM) {
                var objCopy = obj(sporttype + delimiter + strChkCopyFrom
						+ delimiter + bettype);
                if (objCopy) {
                    objCopy.disabled = !objChkAuto.checked;
                    if (!objChkAuto.checked) {
                        objCopy.checked = false;
                        ChkCopyFrom(objCopy);
                    }
                }

            }
        }

    } else if (roleid == 2) // Master Auto On Agent
    {
        // master
        var objMPT = obj(sporttype + delimiter + bettype + delimiter + ptType);
        var objPTAuto = obj(sporttype + delimiter + bettype + delimiter
				+ ptType + delimiter + Auto);
        var objAPT = obj(sporttype + delimiter + bettype + delimiter + APT);
        if (objMPT == null || objPTAuto == null)
            return;
        objMPT.disabled = isAuto;
        objPTAuto.disabled = !isAuto;
        ChangeGroupButtonPTStatus(objMPT.id, isAuto);
        ChangeGroupButtonPTStatus(objPTAuto.id, !isAuto);

        if (isAuto) {
            var objAPT = obj(sporttype + delimiter + bettype + delimiter + APT);
            if (objMPT == null)
                return;
            var aptCurrent = objAPT.options[objAPT.selectedIndex].value;
            var aptMin = hPTData.get(sporttype + delimiter + bettype
					+ delimiter + APT + delimiter + MINPT);
            var aptMax = hPTData.get(sporttype + delimiter + bettype
					+ delimiter + APT + delimiter + MAXPT);
            aptCurrent = Math.min(aptMax, aptCurrent);
            aptMin = Math.min(aptMin, aptCurrent);
            BindSelectOptions(objAPT, aptMax, aptMin, aptCurrent, interval_PT);

            // var maxSPT = objSPT.options[0].value;
            // BindSelectOptions(objSPT, maxSPT, 0, 0, interval_PT);
        } else {
            var mptMax = hPTData.get(sporttype + delimiter + bettype
					+ delimiter + MPT + delimiter + MAXPT);
            var mptCurrent = objMPT.options[objMPT.selectedIndex].value;
            var objAPT = obj(sporttype + delimiter + bettype + delimiter + APT);
            if (objAPT == null)
                return;
            var aptCurrent = objAPT.options[objAPT.selectedIndex].value;
            var aptMin = hPTData.get(sporttype + delimiter + bettype
					+ delimiter + APT + delimiter + MINPT);
            var aptMax = hPTData.get(sporttype + delimiter + bettype
					+ delimiter + APT + delimiter + MAXPT);
            aptMax = Math.min(mptMax - mptCurrent, aptMax);
            aptCurrent = Math.min(aptMax, aptCurrent);
            // Math.min(mptMax, mptCurrent);
            aptMin = Math.min(aptMin, aptCurrent);

            BindSelectOptions(objAPT, aptMax, aptMin, aptCurrent, interval_PT);
        }
    } else if (roleid == 3) {
        // master
        var objSPT = obj(sporttype + delimiter + bettype + delimiter + ptType);
        var objPTAuto = obj(sporttype + delimiter + bettype + delimiter
				+ ptType + delimiter + Auto);
        var objMPT = obj(sporttype + delimiter + bettype + delimiter + MPT);
        if (objSPT == null || objPTAuto == null)
            return;
        objSPT.disabled = isAuto;
        objPTAuto.disabled = !isAuto;
        ChangeGroupButtonPTStatus(objSPT.id, isAuto);
        ChangeGroupButtonPTStatus(objPTAuto.id, !isAuto);

        if (isAuto) {
            var objMPT = obj(sporttype + delimiter + bettype + delimiter + MPT);
            if (objMPT == null)
                return;
            var mptCurrent = objMPT.options[objMPT.selectedIndex].value;
            var mptMin = hPTData.get(sporttype + delimiter + bettype
					+ delimiter + MPT + delimiter + MINPT);
            var mptMax = hPTData.get(sporttype + delimiter + bettype
					+ delimiter + MPT + delimiter + MAXPT);
            mptCurrent = Math.min(mptMax, mptCurrent);
            mptMin = Math.min(mptMin, mptCurrent);
            BindSelectOptions(objMPT, mptMax, mptMin, mptCurrent, interval_PT);

            // var maxSPT = objSPT.options[0].value;
            // BindSelectOptions(objSPT, maxSPT, 0, 0, interval_PT);
        } else {
            var sptMax = hPTData.get(sporttype + delimiter + bettype
					+ delimiter + SPT + delimiter + MAXPT);
            var sptCurrent = objSPT.options[objSPT.selectedIndex].value;
            var objMPT = obj(sporttype + delimiter + bettype + delimiter + MPT);
            if (objMPT == null)
                return;
            var mptCurrent = objMPT.options[objMPT.selectedIndex].value;
            var mptMin = hPTData.get(sporttype + delimiter + bettype
					+ delimiter + MPT + delimiter + MINPT);
            var mptMax = hPTData.get(sporttype + delimiter + bettype
					+ delimiter + MPT + delimiter + MAXPT);
            mptMax = Math.min(sptMax - sptCurrent, mptMax);
            mptCurrent = Math.min(mptMax, mptCurrent);
            // Math.min(mptMax, mptCurrent);
            mptMin = Math.min(mptMin, mptCurrent);

            BindSelectOptions(objMPT, mptMax, mptMin, mptCurrent, interval_PT);
            // reset auto pt value to 0
            // var maxSAuto = objPTAuto.options[0].value;
            // BindSelectOptions(objPTAuto, maxSAuto, 0, 0, interval_PT);
        }
    }
}

function ChkCopyFrom(objChkCopy) {
    if (objChkCopy == null) {
        alert("Null object");
        return;
    }
    var id = objChkCopy.id;
    if (id == null || id == "") {
        alert("Checkbox control wasn't received an id");
    }

    var arrInfo = id.split(delimiter);
    if (arrInfo < 1) {
        alert("Id is wrong format");
        return;
    }
    var copyFromBettype = arrInfo[2];
    var sporttype = arrInfo[0];
    for (var i = 0; i < arrBettype201.length; i++) {
        if (arrBettype201[i] != copyFromBettype) {
            // $("[id^='" + sporttype + delimiter + arrBettype201[i] +
            // "']").attr(attrDisabled, (objChkCopy.checked));
            for (var j = 0; j < arrPTType.length; j++) {
                // obj(sporttype + );
                var objControl = obj(sporttype + delimiter + arrBettype201[i]
						+ delimiter + arrPTType[j]);
                if (objControl == null)
                    continue;
                objControl.disabled = objChkCopy.checked;
                if (arrPTType[j] == MINPT) {
                    var objMinPT = obj(sporttype + delimiter + arrBettype201[i]
							+ delimiter + MINPT);
                    if (objMinPT != null) {
                        objMinPT.disabled = objChkCopy.checked
								|| disableMinPTFI
                    }
                }
                if (!objChkCopy.checked) {
                    if (arrPTType[j].indexOf(SPT + delimiter + strChkAuto) != -1) // super
                    // auto
                    {
                        // super auto
                        var objChkAuto = obj(sporttype + delimiter
								+ arrBettype201[i] + delimiter + SPT
								+ delimiter + strIsAuto);
                        if (objChkAuto == null)
                            continue;
                        CheckAuto(objChkAuto);

                    } else if (arrPTType[j].indexOf(MPT + delimiter
							+ strChkAuto) != -1) // master
                    // auto
                    {
                        var objChkAuto = obj(sporttype + delimiter
								+ arrBettype201[i] + delimiter + MPT
								+ delimiter + strIsAuto);
                        if (objChkAuto == null)
                            continue;
                        CheckAuto(objChkAuto);

                    }
                }

            }
        }
    }
}

function OnChangePT(objSelectPT) {
    if (objSelectPT == null)
        return;
    var id = objSelectPT.id;
    if (id == null || id == "") {
        alert("Select control was received an id");
        return;
    }
    var arrInfo = id.split(delimiter);
    if (arrInfo.length < 1)
        return;
    var ptType = arrInfo[2];
    if (arrInfo.length == 4)
        ptType += delimiter + arrInfo[3];
    var bettype = arrInfo[1];
    var sporttype = arrInfo[0];
    if (isNaN(bettype))
        return;

    if (ptType == MAXPT) // max PT
    {
        var maxPTCurrent = objSelectPT.options[objSelectPT.selectedIndex].value;
        var objMinPT = obj(sporttype + delimiter + bettype + delimiter + MINPT);
        if (objMinPT == null)
            return;
        var minPTCurrent = objMinPT.options[objMinPT.selectedIndex].value;
        var minPTMax = hPTData.get(sporttype + delimiter + bettype + delimiter
				+ MINPT + delimiter + MAXPT)
        minPTMax = Math.min(maxPTCurrent, minPTMax);
        minPTCurrent = Math.min(minPTMax, minPTCurrent);
        BindSelectOptions(objMinPT, minPTMax, 0, minPTCurrent, interval_PT);
    } else if (ptType == MINPT) {
        var minPTCurrent = objSelectPT.options[objSelectPT.selectedIndex].value;
        var objMaxPT = obj(sporttype + delimiter + bettype + delimiter + MAXPT);
        if (objMaxPT == null)
            return;
        var maxPTCurrent = objMaxPT.options[objMaxPT.selectedIndex].value;
        var maxPTMax = objMaxPT.options[0].value;
        var maxPTMin = minPTCurrent;
        // maxPTMin = Math.max(maxPTMin, minPTCurrent);
        BindSelectOptions(objMaxPT, maxPTMax, maxPTMin, maxPTCurrent,
				interval_PT);
    } else {
        if (roleid == 4) // super
        {
            if (ptType == SPT) {
                var sptCurrent = objSelectPT.options[objSelectPT.selectedIndex].value;
                var objMPT = obj(sporttype + delimiter + bettype + delimiter
						+ MPT);
                var objAPT = obj(sporttype + delimiter + bettype + delimiter
						+ APT);
                if (objMPT != null)
                    BindSelectOptions(objMPT, sptCurrent, 0, sptCurrent,
							interval_PT);
                if (objAPT != null)
                    BindSelectOptions(objAPT, sptCurrent, 0, sptCurrent,
							interval_PT);

            } else if (ptType == MPT) {
                var mptCurrent = objSelectPT.options[objSelectPT.selectedIndex].value;
                var objAPT = obj(sporttype + delimiter + bettype + delimiter
						+ APT);
                if (objAPT != null)
                    BindSelectOptions(objAPT, mptCurrent, 0, mptCurrent,
							interval_PT);

            }
        } else if (roleid == 3) {
            // master
            if (ptType == SPT) {
                var sptMax = hPTData.get(sporttype + delimiter + bettype
						+ delimiter + SPT + delimiter + MAXPT);
                var sptCurrent = objSelectPT.options[objSelectPT.selectedIndex].value;
                var objMPT = obj(sporttype + delimiter + bettype + delimiter
						+ MPT);
                if (objMPT == null)
                    return;
                var mptCurrent = objMPT.options[objMPT.selectedIndex].value;
                var mptMin = hPTData.get(sporttype + delimiter + bettype
						+ delimiter + MPT + delimiter + MINPT);
                var mptMax = hPTData.get(sporttype + delimiter + bettype
						+ delimiter + MPT + delimiter + MAXPT);
                mptMax = Math.min(sptMax - sptCurrent, mptMax);
                mptCurrent = mptMax;
                // Math.min(mptMax, mptCurrent);
                mptMin = Math.min(mptMin, mptCurrent);
                BindSelectOptions(objMPT, mptMax, mptMin, mptCurrent,
						interval_PT);
            }
        } else if (roleid == 2) {
            // agent
            if (ptType == MPT) {
                var mptMax = hPTData.get(sporttype + delimiter + bettype
						+ delimiter + MPT + delimiter + MAXPT);
                var mptCurrent = objSelectPT.options[objSelectPT.selectedIndex].value;
                var objAPT = obj(sporttype + delimiter + bettype + delimiter
						+ APT);
                if (objAPT == null)
                    return;
                var aptCurrent = objAPT.options[objAPT.selectedIndex].value;
                var aptMin = hPTData.get(sporttype + delimiter + bettype
						+ delimiter + APT + delimiter + MINPT);
                var aptMax = hPTData.get(sporttype + delimiter + bettype
						+ delimiter + APT + delimiter + MAXPT);
                aptMax = Math.min(mptMax - mptCurrent, aptMax);
                aptCurrent = aptMax;
                // Math.min(mptMax, mptCurrent);
                aptMin = Math.min(aptMin, aptCurrent);
                BindSelectOptions(objAPT, aptMax, aptMin, aptCurrent,
						interval_PT);
            }
        } else if (roleid == 1) {

        }
    }

}

// Click button to change pt
function ChangePT(objButton, value) {
    if (objButton == null)
        return;
    var id = objButton.id;
    if (id == null || id == "")
        return;
    var arrInfo = id.split("_Btn");
    var objSelectPT = obj(arrInfo[0]);
    // 201_2_spositiontaking
    if (objSelectPT == null)
        return;
    var currentPTValue = objSelectPT.options[objSelectPT.selectedIndex].value;
    currentPTValue = (Math.round(currentPTValue * CONST_PT_INT) + Math
			.round(value * CONST_PT_INT))
			/ CONST_PT_INT;
    if ((currentPTValue >= 0) && (currentPTValue <= CONST_PT_MAX)) {
        var objOption = obj(arrInfo[0] + delimiter + PREFIX_OPTIONS + delimiter
				+ currentPTValue);
        if (objOption != null) {
            objOption.selected = true;
            OnChangePT(objSelectPT);
        }
    }

}
// Clear all select options
function ClearSelectOptions(objSelect) {
    if (objSelect != null) {
        for (var i = objSelect.options.length - 1; i >= 0; i--) {
            objSelect.remove(i);
        }
    }
}

function BindSelectOptions(objSelect, maxValue, minValue, currentValue,
		interval) {
    if (objSelect != null) {

        if (minValue == null)
            minValue = 0;
        ClearSelectOptions(objSelect);
        minValue = Math.min(maxValue, minValue);
        currentValue = Math.max(currentValue, 0);
        maxValue = Math.max(maxValue, 0);
        minValue = Math.max(minValue, 0);
        currentValue = Math.max(minValue, currentValue);
        var min = Math.round(minValue * CONST_PT_INT10);
        var groupValue = Math.round(maxValue * CONST_PT_INT10);
        if (currentValue > maxValue) {
            currentValue = maxValue;
        }
        while (groupValue >= min) {
            var selValue = groupValue / CONST_PT_INT10;

            var id = objSelect.id + delimiter + PREFIX_OPTIONS + delimiter
					+ selValue;
            if (currentValue == selValue) {
                objSelect.options.add(CreateOption(selValue, id, true));
            } else {

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

function GenPostData(sporttype) {
    if (sporttype == null)
        sporttype = 201;
    var postData = "";
    var objChkCopy = $("[id^='" + sporttype + delimiter + strChkCopyFrom
			+ delimiter + "']");
    var idChkCopy = objChkCopy.attr('id');
    var isCopyFrom = false;
    var copyFromBettype = 0;
    if (objChkCopy != null && idChkCopy != null && idChkCopy != "") {
        isCopyFrom = objChkCopy.attr('checked');
        copyFromBettype = idChkCopy.split(delimiter)[2];
    }

    for (var i = 0; i < arrBettype201.length; i++) {
        for (var j = 0; j < arrPTType.length; j++) {
            var idSelectPT = "";
            if (isCopyFrom) {
                idSelectPT = sporttype + delimiter + copyFromBettype
						+ delimiter + arrPTType[j];
            } else {
                idSelectPT = sporttype + delimiter + arrBettype201[i]
						+ delimiter + arrPTType[j];
            }
            var objSelectPT = obj(idSelectPT);
            if (objSelectPT != null) {
                var prefix = (sporttype > 1)
						? PREFIX_SPORTTYPE + sporttype
						: "";
                var arrPTInfo = arrPTType[j].split(delimiter);
                if (arrPTInfo.length < 2) {
                    postData += "&"
							+ prefix
							+ arrPTType[j]
							+ delimiter
							+ arrBettype201[i]
							+ "="
							+ objSelectPT.options[objSelectPT.selectedIndex].value;
                } else {
                    // isauto, auto_v201spostiontaking_2
                    if (arrPTInfo[1] == strChkAuto) {
                        postData += "&" + arrPTInfo[1] + delimiter + prefix
								+ arrPTInfo[0] + delimiter + arrBettype201[i]
								+ "=" + objSelectPT.checked;
                    } else if (objSelectPT.type == "select-one") {
                        postData += "&"
								+ arrPTInfo[1]
								+ delimiter
								+ prefix
								+ arrPTInfo[0]
								+ delimiter
								+ arrBettype201[i]
								+ "="
								+ objSelectPT.options[objSelectPT.selectedIndex].value;
                    }
                }
            }
        }

    }

    // cust info
    postData += "&custid=" + custid;
    postData += "&roleid=" + roleid;

    // commission
    var objCommission = obj("201_commission");
    if (objCommission != null) {
        postData += "&v201discount="
				+ objCommission.options[objCommission.selectedIndex].value;
    }
    var minbet201 = obj("txtMinbet201");
    var maxbet201 = obj("txtMaxbet201");
    var maxpermatch201 = obj("txtMaxPerMatch201");
    if (minbet201)
        postData += "&minbetv201=" + minbet201.value;
    if (maxbet201)
        postData += "&maxbetv201=" + maxbet201.value;
    if (maxpermatch201)
        postData += "&maxpermatchv201=" + maxpermatch201.value;
    var disablefinance;
    disablefinance = !obj("chkDisabledFI").checked;

    var disablefinancedownline;
    if (obj("chkDisabledFIDL") != null) {
        disablefinancedownline = !obj("chkDisabledFIDL").checked;
        postData += "&" + ISDOWNLINEDISABLED + "=" + disablefinancedownline;
    }

    postData += "&" + ISDISABLED + "=" + disablefinance;

    postData += "&" + str_newspositiontaking + "=" + newspositiontaking;
    postData += "&" + str_updateallmembers + "=" + updateallmembers;

    return (postData);
}

function obj(id) {
    return document.getElementById(id);
}

function UpdatePT() {
    alert_error('', 'errMsg');
    alert_error('', 'errMsg201');
    if (!CheckLimitMinMaxBet201(201)) {
        return false;
    } else {

        if (isOpenPopUp == 1) {
            if (roleid == 3) {
                msPTMax = hPTData.get(str_MsPTMax);
                var objMPT = obj(sporttype_FI + delimiter + bettypeMemberPTMax
						+ delimiter + MPT);
                var mptValue = (objMPT == null)
						? 0
						: objMPT.options[objMPT.selectedIndex].value;
                memberPTMax = msPTMax - mptValue;
            } else if (roleid == 2) { // auto master on agent
                mmPTMax = hPTData.get(str_MmPTMax);
                var objAPT = obj(sporttype_FI + delimiter + bettypeMemberPTMax
						+ delimiter + APT);
                var aptValue = (objAPT == null)
						? 0
						: objAPT.options[objAPT.selectedIndex].value;
                memberPTMax = mmPTMax - aptValue;
            }
            PTBlockUI();

            confirmPopup = window
					.open("ConfirmEditMasterFI.aspx?roleid=" + roleid, "ConfirmEdit",
							"top=250,left=400,width=350,height=160,resizable,scrollbars=no,dependent");
            return false;

        } else {
            PTBlockUI();
        }
        var PTData = GenPostData();
        PTData = PTData.substring(1);
        $.ajax({
            type: "POST",
            url: "../../Handlers/UpdateFI.ashx",
            async: true,
            data: PTData,
            success: GetUpdateResult
        });
    }
}

function GenMinMaxPTPostData(sporttype) {
    if (sporttype == null)
        sporttype = 201;
    var postData = "";
    var objChkCopy = $("[id^='" + sporttype + delimiter + strChkCopyFrom
			+ delimiter + "']");
    var idChkCopy = objChkCopy.attr('id');
    var isCopyFrom = false;
    var copyFromBettype = 0;
    if (objChkCopy != null && idChkCopy != null && idChkCopy != "") {
        isCopyFrom = objChkCopy.attr('checked');
        copyFromBettype = idChkCopy.split(delimiter)[2];
    }

    for (var i = 0; i < arrBettype201.length; i++) {
        for (var j = 0; j < arrPTType.length; j++) {
            var idSelectPT = "";
            if (isCopyFrom) {
                idSelectPT = sporttype + delimiter + copyFromBettype
						+ delimiter + arrPTType[j];
            } else {
                idSelectPT = sporttype + delimiter + arrBettype201[i]
						+ delimiter + arrPTType[j];
            }
            var objSelectPT = obj(idSelectPT);
            if (objSelectPT != null) {
                var prefix = (sporttype > 1)
						? PREFIX_SPORTTYPE + sporttype
						: "";
                var arrPTInfo = arrPTType[j].split(delimiter);

                if (objSelectPT.type == "select-one") {
                    postData += "&"
							+ prefix
							+ arrPTType[j]
							+ delimiter
							+ arrBettype201[i]
							+ "="
							+ objSelectPT.options[objSelectPT.selectedIndex].value;
                }

            }
        }

    }

    // cust info
    postData += "&custid=" + custid;
    // commission
    return (postData);
}

function GenPTMasterOnMemberPostData(sporttype) {
    if (sporttype == null)
        sporttype = 201;
    var postData = "";
    var objChkCopy = $("[id^='" + sporttype + delimiter + strChkCopyFrom
			+ delimiter + "']");
    var idChkCopy = objChkCopy.attr('id');
    var isCopyFrom = false;
    var copyFromBettype = 0;
    if (objChkCopy != null && idChkCopy != null && idChkCopy != "") {
        isCopyFrom = objChkCopy.attr('checked');
        copyFromBettype = idChkCopy.split(delimiter)[2];
    }


    for (var i = 0; i < arrBettype201.length; i++) {
        for (var j = 0; j < arrPTType.length; j++) {
            var idSelectPT = "";
            if (isCopyFrom) {
                idSelectPT = sporttype + delimiter + copyFromBettype
						+ delimiter + arrPTType[j];
            } else {
                idSelectPT = sporttype + delimiter + arrBettype201[i]
						+ delimiter + arrPTType[j];
            }
            var objSelectPT = obj(idSelectPT);
            if (objSelectPT != null) {
                var prefix = (sporttype > 1)
						? PREFIX_SPORTTYPE + sporttype
						: "";
                var arrPTInfo = arrPTType[j].split(delimiter);
                if (arrPTInfo[1] == strChkAuto) {
                    postData += "&" + arrPTInfo[1] + delimiter + prefix
							+ arrPTInfo[0] + delimiter + arrBettype201[i] + "="
							+ objSelectPT.checked;
                }
                if (objSelectPT.type == "select-one") {
                    postData += "&"
							+ prefix
							+ arrPTType[j]
							+ delimiter
							+ arrBettype201[i]
							+ "="
							+ objSelectPT.options[objSelectPT.selectedIndex].value;
                } else if (objSelectPT.nodeName.toString() == "LABEL") {
                    postData += "&" + prefix + arrPTType[j] + delimiter
							+ arrBettype201[i] + "=" + objSelectPT.innerHTML;
                }

            }
        }

    }

    // cust info
    postData += "&custid=" + custid;
    // commission
    return (postData);

}

function GenPTSuperOnMemberPostData(sporttype) {
    if (sporttype == null)
        sporttype = 201;
    var postData = "";
    var objChkCopy = $("[id^='" + sporttype + delimiter + strChkCopyFrom
			+ delimiter + "']");
    var idChkCopy = objChkCopy.attr('id');
    var isCopyFrom = false;
    var copyFromBettype = 0;
    if (objChkCopy != null && idChkCopy != null && idChkCopy != "") {
        isCopyFrom = objChkCopy.attr('checked');
        copyFromBettype = idChkCopy.split(delimiter)[2];
    }

    for (var i = 0; i < arrBettype201.length; i++) {
        for (var j = 0; j < arrPTType.length; j++) {
            var idSelectPT = "";
            if (isCopyFrom) {
                idSelectPT = sporttype + delimiter + copyFromBettype
						+ delimiter + arrPTType[j];
            } else {
                idSelectPT = sporttype + delimiter + arrBettype201[i]
						+ delimiter + arrPTType[j];
            }
            var objSelectPT = obj(idSelectPT);

            if (objSelectPT != null) {
                var prefix = (sporttype > 1)
						? PREFIX_SPORTTYPE + sporttype
						: "";
                var arrPTInfo = arrPTType[j].split(delimiter);

                if (arrPTInfo[1] == strChkAuto) {
                    postData += "&" + arrPTInfo[1] + delimiter + prefix
							+ arrPTInfo[0] + delimiter + arrBettype201[i] + "="
							+ objSelectPT.checked;
                }
                if (objSelectPT.type == "select-one") {
                    postData += "&"
							+ prefix
							+ arrPTType[j]
							+ delimiter
							+ arrBettype201[i]
							+ "="
							+ objSelectPT.options[objSelectPT.selectedIndex].value;
                }
                else if (objSelectPT.nodeName.toString() == "LABEL") {
                    postData += "&" + prefix + arrPTType[j] + delimiter
							+ arrBettype201[i] + "=" + objSelectPT.innerHTML;
                }

            }
        }

    }

    // cust info
    postData += "&custid=" + custid;
    // commission
    return (postData);
}

function UpdateMinMaxPTFI() {
    alert_error('', 'errMsg');

    var PTData = GenMinMaxPTPostData();
    PTData = PTData.substring(1);
    // jquery post string do not include first char '&'

    $.ajax({
        type: "POST",
        url: "../../Handlers/UpdateMinMaxPTFI.ashx",
        async: true,
        data: PTData,
        success: GetUpdateMinMaxResult
    });
    PTBlockUI();
}

function UpdatePTFIMasterOnMember() {
    alert_error('', 'errMsg');

    var PTData = GenPTMasterOnMemberPostData();
    PTData = PTData.substring(1);
    // jquery post string do not include first char '&'

    $.ajax({
        type: "POST",
        url: "../../Handlers/UpdatePTFIMasterOnMember.ashx",
        async: true,
        data: PTData,
        success: GetUpdateMasterOnMemberResult
    });
    PTBlockUI();
}

function UpdatePTFISuperOnMember() {
    alert_error('', 'errMsg');
    var PTData = GenPTSuperOnMemberPostData();
    PTData = PTData.substring(1);
    // jquery post string do not include first char '&'
    $.ajax({
        type: "POST",
        url: "../../Handlers/UpdatePTFISuperOnMember.ashx",
        async: true,
        data: PTData,
        success: GetUpdateSuperOnMemberResult
    });
    PTBlockUI();
}

function GetUpdateSuperOnMemberResult(rs) {


    var url = "EditSuperOnMemberFI.aspx?"
    var result = $.evalJSON(rs);
    // DelayReloadPage('updatefinancept.aspx', 2000)
    if (result.errorCode == "0") {
        url += "custid=" + custid + "&roleid=" + roleid + "&uplineid="
				+ uplineid + "&insert=" + insert + "&errorcode=" + 0;
        DelayReloadPage(url, 2000);
    } else {
        PTUnBlockUI();
        alert_error(result.errorMsg, 'errMsg');
    }
}

function GetUpdateMasterOnMemberResult(rs) {

    var url = "EditMasterOnMemberFI.aspx?"
    // DelayReloadPage('updatefinancept.aspx', 2000)
    var result = $.evalJSON(rs);
    if (result.errorCode == "0") {
        url += "custid=" + custid + "&roleid=" + roleid + "&uplineid="
				+ uplineid + "&insert=" + insert + "&errorcode=" + 0;
        DelayReloadPage(url, 2000);
    } else {
        PTUnBlockUI();
        alert_error(result.errorMsg, 'errMsg');
    }
}

function GetUpdateMinMaxResult(rs) {
    var url = "MinMaxFinancePT.aspx?";
    var result = $.evalJSON(rs);
    if (result.errorCode == "0") {
        url += "custid=" + custid + "&roleid=" + roleid + "&uplineid="
				+ uplineid + "&insert=" + insert + "&errorcode=" + 0;
        DelayReloadPage(url, 2000, "", "../../");
    } else {
        PTUnBlockUI();
        alert_error(result.errorMsg, 'errMsg');
    }
}

function GetUpdateResult(rs) {
    var result = $.evalJSON(rs);
    if (result.errorCode == "0") {
        alert_error(result.errorMsg, 'errMsg');
        afterUpdate(true, 2000);
    } else {
        PTUnBlockUI();
        alert_error(result.errorMsg, 'errMsg');
        if (result.errorCode == "500")//Check no permission
        {
            afterUpdate(false, 2000);
        }
    }
}

function CheckLimitMinMaxBet201(sporttypeId) {
    try {
        var minbetId = "txtMinbet" + sporttypeId;
        var maxbetId = "txtMaxbet" + sporttypeId;
        var maxpermatchId = "txtMaxPerMatch" + sporttypeId;
        var lmMinbetId = "txtMinbet_Min" + sporttypeId;
        var lmMaxbetId = "txtMaxbet_Max" + sporttypeId;
        var maxpermatchMinId = "txtMaxPerMatch_Min" + sporttypeId;
        var maxpermatchMaxId = "txtMaxPerMatch_Max" + sporttypeId;
        if (isEmpty(minbetId) || isString(minbetId)) {
            //alert_error1("*" + alertMinbet, "errMsg" + sporttypeId);
            showAlertMessage("errMsg" + sporttypeId, alertMinbet, 1);
            obj(minbetId).focus();
            return false;
        }

        if (isNumeric(minbetId) != 0 && isNumericSpan(lmMinbetId) != 0
				&& isNumeric(minbetId) < isNumericSpan(lmMinbetId)) {
            //			alert_error1("*" + alertminbetC + " " + isNumericSpan(lmMinbetId),
            //					"errMsg" + sporttypeId);
            showAlertMessage("errMsg" + sporttypeId, alertminbetC + " " + isNumericSpan(lmMinbetId), 1);
            obj(minbetId).focus();
            return false;
        }

        if (isEmpty(maxbetId) || isString(maxbetId)) {
            //alert_error1("*" + alertMaxbet, "errMsg" + sporttypeId);
            showAlertMessage("errMsg" + sporttypeId, alertMaxbet, 1);
            obj(maxbetId).focus();
            return false;
        }
        if (isNumeric(maxbetId) != 0 && isNumericSpan(lmMaxbetId) != 0
				&& isNumeric(maxbetId) > isNumericSpan(lmMaxbetId)) {
            //			alert_error1("*" + alertMaxbetC + " " + isNumericSpan(lmMaxbetId),
            //					"errMsg" + sporttypeId);
            showAlertMessage("errMsg" + sporttypeId, alertMaxbetC + " " + isNumericSpan(lmMaxbetId), 1);
            obj(maxbetId).focus();
            return false;
        }

        if (isNumeric(minbetId) > isNumeric(maxbetId)) {
            //alert_error1("*" + alertCompare3, "errMsg" + sporttypeId);
            showAlertMessage("errMsg" + sporttypeId, alertCompare3, 1);
            obj(minbetId).focus();
            return false;
        }

        if (isEmpty(maxpermatchId) || isString(maxpermatchId)) {
            //alert_error1("*" + alertMaxpermatch, "errMsg" + sporttypeId);
            showAlertMessage("errMsg" + sporttypeId, alertMaxpermatch, 1);
            obj(maxpermatchId).focus();
            return false;
        }

        if (isNumeric(maxbetId) > isNumeric(maxpermatchId)) {
            //alert_error1("*" + alertCompare, "errMsg" + sporttypeId);
            showAlertMessage("errMsg" + sporttypeId, alertCompare, 1);
            obj(maxbetId).focus();
            return false;
        }

        if (isNumeric(maxpermatchId) != 0) {
            if (isNumeric(maxpermatchId) < isNumericSpan(maxpermatchMinId)
					&& (isNumericSpan(maxpermatchMinId) != 0)) {
                var str = alertMaxpermatchC.replace("{1}",
						isNumericSpan(maxpermatchMinId));
                str = str.replace("{2}", isNumericSpan(maxpermatchMaxId));
                //				alert_error1("*" + str, "errMsg" + sporttypeId);
                showAlertMessage("errMsg" + sporttypeId, str, 1);
                obj(maxpermatchId).focus();
                return false;
            } else if (isNumeric(maxpermatchId) > isNumericSpan(maxpermatchMaxId)
					&& (isNumericSpan(maxpermatchMaxId) != 0)) {
                var str = alertMaxpermatchC.replace("{1}",
						isNumericSpan(maxpermatchMaxId));
                str = str.replace("{2}", isNumericSpan(maxpermatchMaxId));
                //alert_error1("*" + str, "errMsg" + sporttypeId);
                showAlertMessage("errMsg" + sporttypeId, str, 1);
                obj(maxpermatchId).focus();
                return false;
            }
        }
    } catch (ex) {
        return false;
    }
    return true;
}

function alert_error1(errmsg, errHolderId) {
    var intValue;
    document.getElementById(errHolderId).innerHTML = errmsg;
    intValue = window
			.setTimeout("alert_error1('','" + errHolderId + "')", 6000);
}

// change group button follow auto checkbox
function ChangeGroupButtonPTStatus(selectId, disabled) {
    $("input[type=button][id^=" + selectId + "]").attr(attrDisabled, disabled);
    $("input[type=button][id^='" + selectId + "']").removeClass((disabled)
			? CLASS_BTN_ENABLED
			: CLASS_BTN_DISABLED);
    $("input[type=button][id^='" + selectId + "']").addClass((disabled)
			? CLASS_BTN_DISABLED
			: CLASS_BTN_ENABLED);
}




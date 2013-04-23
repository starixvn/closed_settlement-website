var arrSport = [151];
var arrBettype151 = [];
var arrBettype151_tote = [];
var arrBettype151_fixed = [];
var arrBettype = [arrBettype151_tote, arrBettype151_fixed];
var sporttype_HR = 151;
var strChkAuto = 'isauto';
var strChkSAutoPT = 'ChkSAutoPT';
var strChkCopyFrom = "chkCopy";
var strChkCopyAll = "chkCopyAll";
var IBCPT = "IBCPositiontaking", SPT = 'spositiontaking', strSPT = "Super Preset PT";
var ISUPLINEDISABLED = "uplinedisablehr", ISDISABLED = "disablehorseracing", ISDOWNLINEDISABLED = "DisableDownlinesHR";
var ISUPLINEDISABLEDOpenTote = "uplinedisableopentote", ISDISABLEDOpenTote = "disableopentote", ISDOWNLINEDISABLEDOpenTote = "disabledownlinesopentote";

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
var CopyPreset = "iscopypreset";

var STR_AGENT = "Agent";
var STR_MASTER = "Master";
var Auto = "auto";
var strIsAuto = "isauto";

var MSG_UpdatedSuccessful = "Updated Successfully";

var arrPTType = [];
var arrCopyPreset = [31, 41];

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
var interval_PT = 1000;
// 0.01
var interval_Comm = 25;
var hPTData = new Hashtable();

var newspositiontaking = 0;
var updateallmembers = 0;
var str_newspositiontaking = "newspositiontaking";
var str_updateallmembers = "updateallmembers";
var memberPTMax = 0;
var msPTMax = 0;
var str_MsPTMax = "msPTMax";
var mmPTMax = 0;
var str_MmPTMax = "mmPTMax";
var bettypeMemberPTMax = 31;
var isUseAuto = 0;
var isOpenPopUp = 0;
var disableMinPTHR = false;
var isAutoMaster = false;
var FIXED_BETTYPE_COPYFROM = 41;
var BETTYPE_COPY_ALL = 31;
// check super auto
function CheckAutoForCopyBettype(objChkAuto) {
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
    if (roleid == 1) // super and master on member
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

        //check master (agent) auto
        var uplineType = (isAutoMaster == true) ? STR_AGENT : STR_MASTER;
        var isAutoUpline = hPTData.get(sporttype + delimiter + bettype + delimiter + strIsAuto + delimiter + uplineType)
        if (isAutoUpline == true) {
            if (IsCopyFromBettype(bettype) == true) {
                if (bettype == BETTYPE_COPY_ALL) {
                    var objCopyAll = obj(sporttype + delimiter + strChkCopyAll + delimiter + bettype)
                    if (objCopyAll) {
                        objCopyAll.disabled = !objChkAuto.checked;
                        if (!objChkAuto.checked) {
                            objCopyAll.checked = false;
                            ChkCopyAll(objCopyAll);
                        }
                    }
                }
                var objCopy = obj(sporttype + delimiter + strChkCopyFrom + delimiter + bettype);
                if (objCopy) {
                    objCopy.disabled = !objChkAuto.checked;
                    if (!objChkAuto.checked) {
                        objCopy.checked = false;
                        ChkCopyFrom(objCopy);
                    }
                }
            }
        }
    } else if (roleid == 2) // agent
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
            if (objAPT == null)
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
            aptCurrent = Math.min(aptCurrent, aptMax);
            // Math.min(mptMax, mptCurrent);
            aptMin = Math.min(aptMin, aptCurrent);

            BindSelectOptions(objAPT, aptMax, aptMin, aptCurrent, interval_PT);
            // reset auto pt value to 0
            // var maxSAuto = objPTAuto.options[0].value;
            // BindSelectOptions(objPTAuto, maxSAuto, 0, 0, interval_PT);
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
            mptCurrent = Math.min(mptCurrent, mptMax);
            // Math.min(mptMax, mptCurrent);
            mptMin = Math.min(mptMin, mptCurrent);

            BindSelectOptions(objMPT, mptMax, mptMin, mptCurrent, interval_PT);
            // reset auto pt value to 0
            // var maxSAuto = objPTAuto.options[0].value;
            // BindSelectOptions(objPTAuto, maxSAuto, 0, 0, interval_PT);
        }
    }
}


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
    if (roleid == 1) // super and master on member
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

        //check master (agent) auto
        var uplineType = (isAutoMaster == true) ? STR_AGENT : STR_MASTER;
        var isAutoUpline = hPTData.get(sporttype + delimiter + bettype + delimiter + strIsAuto + delimiter + uplineType)
        if (isAutoUpline == true) {
            if (IsCopyFromBettype(bettype) == true) {
                if (bettype == BETTYPE_COPY_ALL) {
                    var objCopyAll = obj(sporttype + delimiter + strChkCopyAll + delimiter + bettype)
                    if (objCopyAll)
                        objCopyAll.disabled = !objChkAuto.checked;
                }
                var objCopy = obj(sporttype + delimiter + strChkCopyFrom + delimiter + bettype);
                if (objCopy)
                    objCopy.disabled = !objChkAuto.checked;
            }
        }
    } else if (roleid == 2) // agent
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
            if (objAPT == null)
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
            aptCurrent = Math.min(aptCurrent, aptMax);
            // Math.min(mptMax, mptCurrent);
            aptMin = Math.min(aptMin, aptCurrent);

            BindSelectOptions(objAPT, aptMax, aptMin, aptCurrent, interval_PT);
            // reset auto pt value to 0
            // var maxSAuto = objPTAuto.options[0].value;
            // BindSelectOptions(objPTAuto, maxSAuto, 0, 0, interval_PT);
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
            mptCurrent = Math.min(mptCurrent, mptMax);
            // Math.min(mptMax, mptCurrent);
            mptMin = Math.min(mptMin, mptCurrent);

            BindSelectOptions(objMPT, mptMax, mptMin, mptCurrent, interval_PT);
            // reset auto pt value to 0
            // var maxSAuto = objPTAuto.options[0].value;
            // BindSelectOptions(objPTAuto, maxSAuto, 0, 0, interval_PT);
        }
    }
}

function ChkCopyAll(objChkCopy) {
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
    var objChkCopyFrom = null;

    if (obj("txtMinbet151_41") != null) {
        obj("txtMinbet151_41").disabled = objChkCopy.checked
    }
    if (obj("txtMaxbet151_41") != null) {
        obj("txtMaxbet151_41").disabled = objChkCopy.checked
    }
    if (obj("txtMaxPerMatch151_41") != null) {
        obj("txtMaxPerMatch151_41").disabled = objChkCopy.checked
    }

    for (var k = 0; k < arrBettype.length; k++) {

        arrBettype151 = arrBettype[k];
        var totalBettype = 0;
        for (var i = 0; i < arrBettype151.length; i++) {
            totalBettype++;
            if (IsCopyFromBettype(arrBettype151[i])) {

                objChkCopyFrom = obj(sporttype + delimiter + strChkCopyFrom
						+ delimiter + arrBettype151[i]);
                if (objChkCopyFrom != null) {
                    objChkCopyFrom.disabled = objChkCopy.checked;
                }

            }
            if (arrBettype151[i] != BETTYPE_COPY_ALL) {

                var objCopyPreset = obj(arrBettype151[i] + delimiter
						+ CopyPreset)
                if (objCopyPreset != null) {
                    objCopyPreset.disabled = objChkCopy.checked;
                }
                for (var j = 0; j < arrPTType.length; j++) {
                    // obj(sporttype + );
                    var objControl = obj(sporttype + delimiter
							+ arrBettype151[i] + delimiter + arrPTType[j]);
                    if (objControl == null)
                        continue;
                    objControl.disabled = objChkCopy.checked;
                    ChangeGroupButtonPTStatus(objControl.id, objChkCopy.checked);
                    if (arrPTType[j] == MINPT) {
                        var objMinPT = obj(sporttype + delimiter
								+ arrBettype151[i] + delimiter + MINPT);
                        if (objMinPT != null) {
                            objMinPT.disabled = objChkCopy.checked
									|| disableMinPTHR
                        }
                    }
                    if (!objChkCopy.checked) {
                        if (arrPTType[j].indexOf(SPT + delimiter + strChkAuto) != -1) {
                            // super auto

                            var objChkAuto = obj(sporttype + delimiter
									+ arrBettype151[i] + delimiter + SPT
									+ delimiter + strIsAuto);
                            if (objChkAuto == null)
                                continue;
                            CheckAuto(objChkAuto);
                        } else if (arrPTType[j].indexOf(MPT + delimiter
								+ strChkAuto) != -1) {
                            // super auto

                            var objChkAuto = obj(sporttype + delimiter
									+ arrBettype151[i] + delimiter + MPT
									+ delimiter + strIsAuto);
                            if (objChkAuto == null)
                                continue;
                            CheckAuto(objChkAuto);
                        }
                    }
                }
            }

            if (totalBettype == arrBettype151.length) {
                if (objChkCopyFrom != null) {
                    if (!objChkCopy.checked) {
                        ChkCopyFrom(objChkCopyFrom);
                    }
                }
            }
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
    if (IsHRToteBettype(copyFromBettype))
        arrBettype151 = arrBettype151_tote;
    else
        arrBettype151 = arrBettype151_fixed;
    var sporttype = arrInfo[0];
    for (var i = 0; i < arrBettype151.length; i++) {
        if (arrBettype151[i] != copyFromBettype) {
            // $("[id^='" + sporttype + delimiter + arrBettype151[i] +
            // "']").attr(attrDisabled, (objChkCopy.checked));
            for (var j = 0; j < arrPTType.length; j++) {
                // obj(sporttype + );
                var objControl = obj(sporttype + delimiter + arrBettype151[i]
						+ delimiter + arrPTType[j]);
                if (objControl == null)
                    continue;
                objControl.disabled = objChkCopy.checked;
                if (arrPTType[j] == MINPT) {
                    var objMinPT = obj(sporttype + delimiter + arrBettype151[i]
							+ delimiter + MINPT);
                    if (objMinPT != null) {
                        objMinPT.disabled = objChkCopy.checked
								|| disableMinPTHR
                    }
                }
                if (!objChkCopy.checked) {
                    if (arrPTType[j].indexOf(SPT + delimiter + strChkAuto) != -1) { // super
                        // auto
                        // on
                        // master
                        // super auto

                        var objChkAuto = obj(sporttype + delimiter
								+ arrBettype151[i] + delimiter + SPT
								+ delimiter + strIsAuto);
                        if (objChkAuto == null)
                            continue;
                        CheckAuto(objChkAuto);

                    } else if (arrPTType[j].indexOf(MPT + delimiter
							+ strChkAuto) != -1) {// master
                        // auto
                        // on
                        // agent
                        // super auto

                        var objChkAuto = obj(sporttype + delimiter
								+ arrBettype151[i] + delimiter + MPT
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
    // 151_2_spositiontaking
    if (objSelectPT == null)
        return;
    var maxValue = objSelectPT.options[0].value;
    var currentPTValue = objSelectPT.options[objSelectPT.selectedIndex].value;
    currentPTValue = (Math.round(currentPTValue * CONST_PT_INT) + Math
			.round(value * CONST_PT_INT))
			/ CONST_PT_INT;
    if ((currentPTValue >= 0) && (currentPTValue <= maxValue)) {
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
        sporttype = 151;
    var postData = "";
    // arrBettype = [arrBettype151_tote, arrBettype151_fixed];
    var isCopyAll = false;
    var objCopyAll = obj(sporttype + delimiter + strChkCopyAll + delimiter
			+ BETTYPE_COPY_ALL);
    if (objCopyAll != null)
        isCopyAll = objCopyAll.checked;

    for (var k = 0; k < arrBettype.length; k++) {
        arrBettype151 = arrBettype[k];
        var isCopyFrom = false;

        var copyFromBettype = 0;
        var objChkCopy = GetCopyFromObject(sporttype, arrBettype151);
        var idChkCopy = "";
        if (objChkCopy != null) {
            idChkCopy = objChkCopy.id;
            isCopyFrom = objChkCopy.checked;
            copyFromBettype = idChkCopy.split(delimiter)[2];
        }

        for (var i = 0; i < arrBettype151.length; i++) {
            for (var j = 0; j < arrPTType.length; j++) {
                var idSelectPT = "";
                if (isCopyAll) {
                    idSelectPT = sporttype + delimiter + BETTYPE_COPY_ALL
							+ delimiter + arrPTType[j];
                } else {
                    if (isCopyFrom) {
                        idSelectPT = sporttype + delimiter + copyFromBettype
								+ delimiter + arrPTType[j];
                    } else {
                        idSelectPT = sporttype + delimiter + arrBettype151[i]
								+ delimiter + arrPTType[j];
                    }
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
								+ arrBettype151[i]
								+ "="
								+ objSelectPT.options[objSelectPT.selectedIndex].value;
                    } else {
                        // isauto, auto_v151spostiontaking_2
                        if (arrPTInfo[1] == strChkAuto) {
                            postData += "&" + arrPTInfo[1] + delimiter + prefix
									+ arrPTInfo[0] + delimiter
									+ arrBettype151[i] + "="
									+ objSelectPT.checked;
                        } else if (objSelectPT.type == "select-one") {
                            postData += "&"
									+ arrPTInfo[1]
									+ delimiter
									+ prefix
									+ arrPTInfo[0]
									+ delimiter
									+ arrBettype151[i]
									+ "="
									+ objSelectPT.options[objSelectPT.selectedIndex].value;
                        }
                    }
                }
            }

        }
    }

    // cust info
    postData += "&custid=" + custid;
    postData += "&roleid=" + roleid;

    for (var b = 0; b < arrCopyPreset.length; b++) {
        var copyPre = false;
        if (isCopyAll) {
            copyPre = (obj(BETTYPE_COPY_ALL + delimiter + CopyPreset) == null)
					? false
					: $('#' + BETTYPE_COPY_ALL + delimiter + CopyPreset)
							.attr("checked")
            postData += "&" + CopyPreset + delimiter + arrCopyPreset[b] + "="
					+ copyPre;
        } else {
            copyPre = (obj(arrCopyPreset[b] + delimiter + CopyPreset) == null)
					? false
					: $('#' + arrCopyPreset[b] + delimiter + CopyPreset)
							.attr("checked")
            postData += "&" + CopyPreset + delimiter + arrCopyPreset[b] + "="
					+ copyPre;
        }

    }

    // commission
    var objCommissionTote = obj("151_31_commission");
    if (objCommissionTote != null) {
        postData += "&v151discount_31="
				+ objCommissionTote.options[objCommissionTote.selectedIndex].value;
    }

    var objCommissionFixed = obj("151_41_commission");
    if (objCommissionFixed != null) {
        postData += "&v151discount_41="
				+ objCommissionFixed.options[objCommissionFixed.selectedIndex].value;
    }

    var minbet151_31 = obj("txtMinbet151_31");
    var maxbet151_31 = obj("txtMaxbet151_31");
    var maxpermatch151_31 = obj("txtMaxPerMatch151_31");
    if (minbet151_31)
        postData += "&minbetv151_31=" + minbet151_31.value;
    if (maxbet151_31)
        postData += "&maxbetv151_31=" + maxbet151_31.value;
    if (maxpermatch151_31)
        postData += "&maxpermatchv151_31=" + maxpermatch151_31.value;

    var minbet151_41 = obj("txtMinbet151_41");
    var maxbet151_41 = obj("txtMaxbet151_41");
    var maxpermatch151_41 = obj("txtMaxPerMatch151_41");
    if (minbet151_41) {
        if (isCopyAll && minbet151_31) {
            postData += "&minbetv151_41=" + minbet151_31.value;
        }
        else {
            postData += "&minbetv151_41=" + minbet151_41.value;
        }
    }
    if (maxbet151_41) {
        if (isCopyAll && maxbet151_31) {
            postData += "&maxbetv151_41=" + maxbet151_31.value;
        }
        else {
            postData += "&maxbetv151_41=" + maxbet151_41.value;
        }
    }
    if (maxpermatch151_41) {
        if (isCopyAll && maxpermatch151_31) {
            postData += "&maxpermatchv151_41=" + maxpermatch151_31.value;
        }
        else {
            postData += "&maxpermatchv151_41=" + maxpermatch151_41.value;
        }
    }

    if (obj("chkDisabledHR") != null) {
        var disablehr;
        disablehr = !obj("chkDisabledHR").checked;
        postData += "&" + ISDISABLED + "=" + disablehr;
    }

    var disablehrdownline;
    if (obj("chkDisabledHRDL") != null) {
        disablehrdownline = !obj("chkDisabledHRDL").checked;
        postData += "&" + ISDOWNLINEDISABLED + "=" + disablehrdownline;
    }


    var disableopentote;
    disableopentote = !obj("chkDisabledOpenTote").checked;
    postData += "&" + ISDISABLEDOpenTote + "=" + disableopentote;

    var disableopentotedownline;
    if (obj("chkDisabledOpenToteDL") != null) {
        disableopentotedownline = !obj("chkDisabledOpenToteDL").checked;
        postData += "&" + ISDOWNLINEDISABLEDOpenTote + "=" + disableopentotedownline;
    }



    postData += "&" + str_newspositiontaking + "=" + newspositiontaking;
    postData += "&" + str_updateallmembers + "=" + updateallmembers;
    postData += "&isdisablesa=" + obj("isDisabledSA").value;
    return (postData);
}

function obj(id) {
    return document.getElementById(id);
}

function UpdatePT() {

    alert_error('', 'errMsg');
    $("#errMsg151tote").html("");
    $("#errMsg151fixed").html("");
    if (!CheckLimitMinMaxBet151(151, 31) || !CheckLimitMinMaxBet151(151, 41)) {
        return false;
    } else {
        if (obj("isDisabledSA").value == "1") bettypeMemberPTMax = 41;
        if (isOpenPopUp == 1) {
            if (roleid == 3) {
                msPTMax = hPTData.get(str_MsPTMax);
              
                var objMPT = obj(sporttype_HR + delimiter + bettypeMemberPTMax
						+ delimiter + MPT);
                var mptValue = (objMPT == null)
						? 0
						: objMPT.options[objMPT.selectedIndex].value;
               
                memberPTMax = msPTMax - mptValue;

            }
            if (roleid == 2) { //auto master on agent
                mmPTMax = hPTData.get(str_MmPTMax);
                var objAPT = obj(sporttype_HR + delimiter + bettypeMemberPTMax
						+ delimiter + APT);
                var aptValue = (objAPT == null)
						? 0
						: objAPT.options[objAPT.selectedIndex].value;
                memberPTMax = mmPTMax - aptValue;
            }
            PTBlockUI();

            confirmPopup = window
					.open("ConfirmEditMasterHR.aspx?roleid=" + roleid, "ConfirmEdit",
							"top=250,left=400,width=350,height=160,resizable,scrollbars=no,dependent");
            return false;

        } else {

            var PTData = GenPostData();
            PTData = PTData.substring(1);
            $.ajax({
                type: "POST",
                url: "../../../Handlers/UpdateHR.ashx",
                async: true,
                data: PTData,
                success: GetUpdateResult
            });
            PTBlockUI();
        }
    }
}

function GenMinMaxPTPostData(sporttype) {
    if (sporttype == null)
        sporttype = 151;
    var postData = "";

    var isCopyAll = false;
    var objCopyAll = obj(sporttype + delimiter + strChkCopyAll + delimiter
			+ BETTYPE_COPY_ALL);
    if (objCopyAll != null)
        isCopyAll = objCopyAll.checked;

    for (var k = 0; k < arrBettype.length; k++) {
        arrBettype151 = arrBettype[k];
        var isCopyFrom = false;

        var copyFromBettype = 0;
        var objChkCopy = GetCopyFromObject(sporttype, arrBettype151);
        var idChkCopy = "";
        if (objChkCopy != null) {
            idChkCopy = objChkCopy.id;
            isCopyFrom = objChkCopy.checked;
            copyFromBettype = idChkCopy.split(delimiter)[2];
        }

        for (var i = 0; i < arrBettype151.length; i++) {
            for (var j = 0; j < arrPTType.length; j++) {
                var idSelectPT = "";
                if (isCopyAll) {
                    idSelectPT = sporttype + delimiter + BETTYPE_COPY_ALL
							+ delimiter + arrPTType[j];
                } else {
                    if (isCopyFrom) {
                        idSelectPT = sporttype + delimiter + copyFromBettype
								+ delimiter + arrPTType[j];
                    } else {
                        idSelectPT = sporttype + delimiter + arrBettype151[i]
								+ delimiter + arrPTType[j];
                    }
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
								+ arrBettype151[i]
								+ "="
								+ objSelectPT.options[objSelectPT.selectedIndex].value;
                    }

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
        sporttype = 151;
    var postData = "";
    var isCopyAll = false;
    var objCopyAll = obj(sporttype + delimiter + strChkCopyAll + delimiter
			+ BETTYPE_COPY_ALL);
    if (objCopyAll != null)
        isCopyAll = objCopyAll.checked;

    for (var k = 0; k < arrBettype.length; k++) {
        arrBettype151 = arrBettype[k];
        var isCopyFrom = false;

        var copyFromBettype = 0;
        var objChkCopy = GetCopyFromObject(sporttype, arrBettype151);
        var idChkCopy = "";
        if (objChkCopy != null) {
            idChkCopy = objChkCopy.id;
            isCopyFrom = objChkCopy.checked;
            copyFromBettype = idChkCopy.split(delimiter)[2];
        }

        for (var i = 0; i < arrBettype151.length; i++) {
            for (var j = 0; j < arrPTType.length; j++) {
                var idSelectPT = "";
                if (isCopyAll) {
                    idSelectPT = sporttype + delimiter + BETTYPE_COPY_ALL
							+ delimiter + arrPTType[j];
                } else {
                    if (isCopyFrom) {
                        idSelectPT = sporttype + delimiter + copyFromBettype
								+ delimiter + arrPTType[j];
                    } else {
                        idSelectPT = sporttype + delimiter + arrBettype151[i]
								+ delimiter + arrPTType[j];
                    }
                }

                var objSelectPT = obj(idSelectPT);
                if (objSelectPT != null) {
                    var prefix = (sporttype > 1)
							? PREFIX_SPORTTYPE + sporttype
							: "";
                    var arrPTInfo = arrPTType[j].split(delimiter);
                    if (arrPTInfo[1] == strChkAuto) {
                        postData += "&" + arrPTInfo[1] + delimiter + prefix
								+ arrPTInfo[0] + delimiter + arrBettype151[i]
								+ "=" + objSelectPT.checked;
                    }
                    if (objSelectPT.type == "select-one") {
                        postData += "&"
								+ prefix
								+ arrPTType[j]
								+ delimiter
								+ arrBettype151[i]
								+ "="
								+ objSelectPT.options[objSelectPT.selectedIndex].value;
                    }
                    else if (objSelectPT.nodeName.toString() == "LABEL") {
                        postData += "&"
								+ prefix
								+ arrPTType[j]
								+ delimiter
								+ arrBettype151[i]
								+ "="
								+ objSelectPT.innerHTML;
                    }

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
        sporttype = 151;
    var postData = "";
    var isCopyAll = false;
    var objCopyAll = obj(sporttype + delimiter + strChkCopyAll + delimiter
			+ BETTYPE_COPY_ALL);
    if (objCopyAll != null)
        isCopyAll = objCopyAll.checked;

    for (var k = 0; k < arrBettype.length; k++) {
        arrBettype151 = arrBettype[k];
        var isCopyFrom = false;

        var copyFromBettype = 0;
        var objChkCopy = GetCopyFromObject(sporttype, arrBettype151);
        var idChkCopy = "";
        if (objChkCopy != null) {
            idChkCopy = objChkCopy.id;
            isCopyFrom = objChkCopy.checked;
            copyFromBettype = idChkCopy.split(delimiter)[2];
        }

        for (var i = 0; i < arrBettype151.length; i++) {
            for (var j = 0; j < arrPTType.length; j++) {
                var idSelectPT = "";
                if (isCopyAll) {
                    idSelectPT = sporttype + delimiter + BETTYPE_COPY_ALL
							+ delimiter + arrPTType[j];
                } else {
                    if (isCopyFrom) {
                        idSelectPT = sporttype + delimiter + copyFromBettype
								+ delimiter + arrPTType[j];
                    } else {
                        idSelectPT = sporttype + delimiter + arrBettype151[i]
								+ delimiter + arrPTType[j];
                    }
                }
                var objSelectPT = obj(idSelectPT);

                if (objSelectPT != null) {
                    var prefix = (sporttype > 1)
							? PREFIX_SPORTTYPE + sporttype
							: "";
                    var arrPTInfo = arrPTType[j].split(delimiter);

                    if (arrPTInfo[1] == strChkAuto) {
                        postData += "&" + arrPTInfo[1] + delimiter + prefix
								+ arrPTInfo[0] + delimiter + arrBettype151[i]
								+ "=" + objSelectPT.checked;
                    }
                    if (objSelectPT.type == "select-one") {
                        postData += "&"
								+ prefix
								+ arrPTType[j]
								+ delimiter
								+ arrBettype151[i]
								+ "="
								+ objSelectPT.options[objSelectPT.selectedIndex].value;
                    }
                    else if (objSelectPT.nodeName.toString() == "LABEL") {
                        postData += "&"
								+ prefix
								+ arrPTType[j]
								+ delimiter
								+ arrBettype151[i]
								+ "="
								+ objSelectPT.innerHTML;
                    }

                }
                else {
                    var prefix = (sporttype > 1)
							? PREFIX_SPORTTYPE + sporttype
							: "";
                    var arrPTInfo = arrPTType[j].split(delimiter);

                    postData += "&"
							+ prefix
							+ arrPTType[j]
							+ delimiter
							+ arrBettype151[i]
							+ "="
							+ 0;

                }
            }

        }
    }

    // cust info
    postData += "&custid=" + custid;
    // commission

    return (postData);
}

function UpdateMinMaxPTHR() {
    alert_error('', 'err');

    var PTData = GenMinMaxPTPostData();
    PTData = PTData.substring(1);
    // jquery post string do not include first char '&'
    $.ajax({
        type: "POST",
        url: "../../../Handlers/UpdateMinMaxPTHR.ashx",
        async: true,
        data: PTData,
        success: GetUpdateMinMaxResult
    });
    PTBlockUI();
}

function UpdatePTHRMasterOnMember() {
    alert_error('', 'err');
    var PTData = GenPTMasterOnMemberPostData();
    PTData = PTData.substring(1);
    // jquery post string do not include first char '&'
    $.ajax({
        type: "POST",
        url: "../../../Handlers/UpdatePTHRMasterOnMember.ashx",
        async: true,
        data: PTData,
        success: GetUpdateMasterOnMemberResult
    });
    PTBlockUI();
}

function UpdatePTHRSuperOnMember() {
    alert_error('', 'err');
    var PTData = GenPTSuperOnMemberPostData();
    PTData = PTData.substring(1);
    // jquery post string do not include first char '&'
    $.ajax({
        type: "POST",
        url: "../../../Handlers/UpdatePTHRSuperOnMember.ashx",
        async: true,
        data: PTData,
        success: GetUpdateSuperOnMemberResult
    });
    PTBlockUI();
}

function GetUpdateSuperOnMemberResult(rs) {

    var url = "EditSuperOnMemberHR.aspx?"
    var result = $.evalJSON(rs);
    // DelayReloadPage('updatefinancept.aspx', 2000)
    if (result.errorCode == "0") {
        url += "custid=" + custid + "&roleid=" + roleid + "&uplineid="
				+ uplineid + "&insert=" + insert + "&errorcode=" + 0;
        DelayReloadPage1(url, 2000, "", "../../../");
    } else {
        alert_error(result.errorMsg, 'err');
    }
    PTUnBlockUI();
}

function GetUpdateMasterOnMemberResult(rs) {

    var url = "EditMasterOnMemberHR.aspx?"
    var result = $.evalJSON(rs);
    // DelayReloadPage('updatefinancept.aspx', 2000)

    if (result.errorCode == "0") {
        url += "custid=" + custid + "&roleid=" + roleid + "&uplineid="
				+ uplineid + "&insert=" + insert + "&errorcode=" + 0;
        DelayReloadPage1(url, 2000, "", "../../../");
    } else {
        alert_error(result.errorMsg, 'err');
    }
    PTUnBlockUI();

}

function GetUpdateMinMaxResult(rs) {
    var url = "EditMinMaxPTHR.aspx?"
    var result = $.evalJSON(rs);
    if (result.errorCode == "0") {
        url += "custid=" + custid + "&roleid=" + roleid + "&uplineid="
				+ uplineid + "&insert=" + insert + "&errorcode=" + 0;

        DelayReloadPage1(url, 2000, "", "../../../");

    } else {
        alert_error(result.errorMsg, 'errMsg');
    }
    PTUnBlockUI();
}

function GetUpdateResult(rs) {
    window.scroll(0, 0);
    parent.scroll(0, 0);
    var result = $.evalJSON(rs);
    if (result.errorCode == "0") {
        alert_error(result.errorMsg, 'errMsg');
        afterUpdate(true, 2000);
    } else {

        alert_error(result.errorMsg, 'errMsg');
        if (result.errorCode == "500") {
            afterUpdate(false, 2000);
        }
        PTUnBlockUI();
    }
}


function CheckLimitMinMaxBet151(sporttypeId, bettype) {
    try {
        var errID = "errMsg" + sporttypeId + (bettype == 31 ? "tote" : "fixed");
        var lmMinbetId = "txtMinbet_Min" + sporttypeId + "_" + bettype;
        var lmMaxbetId = "txtMaxbet_Max" + sporttypeId + "_" + bettype;
        var maxpermatchMinId = "txtMaxPerMatch_Min" + sporttypeId + "_" + bettype;
        var maxpermatchMaxId = "txtMaxPerMatch_Max" + sporttypeId + "_" + bettype;

        if (obj("151_chkCopyAll_31") != null && obj("151_chkCopyAll_31").checked) {
            bettype = 31;
        }
        var minbetId = "txtMinbet" + sporttypeId + "_" + bettype;
        var maxbetId = "txtMaxbet" + sporttypeId + "_" + bettype;
        var maxpermatchId = "txtMaxPerMatch" + sporttypeId + "_" + bettype;


        if (obj(minbetId) == null)
            return true;


        if (isEmpty(minbetId) || isString(minbetId)) {
            //alert_error1("*" + alertMinbet, errID);
            showAlertMessage(errID, alertMinbet, 1);
            obj(minbetId).focus();
            return false;
        }

        if (roleid == 1) {
            if (isNumeric(minbetId) == 0 && isNumeric(maxbetId) == 0 && isNumeric(maxpermatchId) == 0)
            { return true; }
            else {
                if (isNumeric(minbetId) < isNumericSpan(lmMinbetId)) {
                    showAlertMessage(errID, alertminbetC + " " + isNumericSpan(lmMinbetId), 1);
                    obj(minbetId).focus();
                    return false;
                }
                if (isNumeric(maxbetId) > isNumericSpan(lmMaxbetId)) {
                    showAlertMessage(errID, alertMaxbetC + " " + isNumericSpan(lmMaxbetId), 1);
                    obj(maxbetId).focus();
                    return false;
                } 
            }
        }

        if (isNumeric(minbetId) != 0 && isNumericSpan(lmMinbetId) != 0
				&& isNumeric(minbetId) < isNumericSpan(lmMinbetId)) {
            //alert_error1(alertminbetC + " " + isNumericSpan(lmMinbetId), errID);
            showAlertMessage(errID, alertminbetC + " " + isNumericSpan(lmMinbetId), 1);
            obj(minbetId).focus();
            return false;
        }

        if (isEmpty(maxbetId) || isString(maxbetId)) {
            //alert_error1(alertMaxbet, errID);
            showAlertMessage(errID, alertMaxbet, 1);
            obj(maxbetId).focus();
            return false;
        }
        if (isNumeric(maxbetId) != 0 && isNumericSpan(lmMaxbetId) != 0
				&& isNumeric(maxbetId) > isNumericSpan(lmMaxbetId)) {
            //alert_error1(alertMaxbetC + " " + isNumericSpan(lmMaxbetId), errID);
            showAlertMessage(errID, alertMaxbetC + " " + isNumericSpan(lmMaxbetId), 1);
            obj(maxbetId).focus();
            return false;
        }

        if (isNumeric(minbetId) > isNumeric(maxbetId)) {

            //alert_error1(alertCompare3, errID);
            showAlertMessage(errID, alertCompare3, 1);
            obj(minbetId).focus();
            return false;
        }

        
        if (isEmpty(maxpermatchId) || isString(maxpermatchId)) {
            //alert_error1(alertMaxpermatch, errID);
            showAlertMessage(errID, alertMaxpermatch, 1);
            obj(maxpermatchId).focus();
            return false;
        }

        if (isNumeric(maxbetId) > isNumeric(maxpermatchId)) {
            //alert_error1(alertCompare, errID);
            showAlertMessage(errID, alertCompare, 1);
            obj(maxbetId).focus();
            return false;
        }

        if (isNumeric(maxpermatchId) != 0) {
            if (isNumeric(maxpermatchId) < isNumericSpan(maxpermatchMinId)
					&& (isNumericSpan(maxpermatchMinId) != 0)) {
                var str = alertMaxpermatchC.replace("{1}",
						isNumericSpan(maxpermatchMinId));
                str = str.replace("{2}", isNumericSpan(maxpermatchMaxId));
                //alert_error1(str, errID);
                showAlertMessage(errID, str, 1);
                obj(maxpermatchId).focus();
                return false;
            } else if (isNumeric(maxpermatchId) > isNumericSpan(maxpermatchMaxId)
					&& (isNumericSpan(maxpermatchMaxId) != 0)) {
                var str = alertMaxpermatchC.replace("{1}",
						isNumericSpan(maxpermatchMaxId));
                str = str.replace("{2}", isNumericSpan(maxpermatchMaxId));
                //alert_error1(str, errID);
                showAlertMessage(errID, str, 1);
                obj(maxpermatchId).focus();
                return false;
            }

        }
    } catch (ex) {
        return true;
    }
    return true;
}

function alert_error1(errmsg, errHolderId) {
    var intValue;
    showAlertMessage(errHolderId, errmsg, 1);
    intValue = window
			.setTimeout("alert_error('','" + errHolderId + "')", 6000);
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

function IsHRToteBettype(bettype) {
    if (bettype == 31 || bettype == 32 || bettype == 33)
        return true;
    return false;
}

function GetCopyFromObject(sport, arrBet) {
    var objCopyFrom = null;
    for (var i = 0; i < arrBet.length; i++) {
        // alert(sport + delimiter + strChkCopyFrom + delimiter + arrBet[i]);
        objCopyFrom = obj(sport + delimiter + strChkCopyFrom + delimiter
				+ arrBet[i]);
        if (objCopyFrom != null)
            return objCopyFrom;
    }
}

function IsCopyFromBettype(bettype) {
    if (bettype == 31 || bettype == 41)
        return true;
    return false;
}

function MenuItemClick(divControl) {
    var tdid = divControl.replace(/div/, "td");
    if ($("#" + divControl).is(":hidden")) {
        $("#" + divControl).show();
        $("#" + tdid).removeClass("md2");
        $("#" + tdid).addClass("md3");
    } else {

        $("#" + divControl).hide();
        $("#" + tdid).removeClass("md3");
        $("#" + tdid).addClass("md2");
    }
}

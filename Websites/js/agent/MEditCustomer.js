/*******************************************************************************
 * this script is used in: Maccount/MEditCustomer.aspx Maccount/AddNewAgent.aspx /
 ******************************************************************************/
// declare array that contain id of Master Preset Position dropdown to use for
// CheckAutoPreset function
var arrDdl1 = ["listS1HdpD", "listS1OUD", "listS11stHdpD", "listS11stOUD",
		"listS1OE", "listS11x2", "listS1CS", "listS1TG", "listS1MP",
		"listS1OR", "listS1HdpL", "listS1OUL", "listS11stHdpL", "listS11stOUL"];
var arrDdl2 = ["listS2Hdp", "listS2OU", "listS2OE", "listS2MP", "listS2ML"];// basketball
var arrDdl3 = ["listS3Hdp", "listS3OU", "listS3OE", "listS3ML"];// football
var arrDdl5 = ["listS5Hdp", "listS5OU", "listS5OE", "listS5ML"];// tennis
var arrDdl8 = ["listS8Hdp", "listS8OU", "listS8ML"];// baseball
var arrDdl10 = ["listS10Hdp", "listS10OU", "listS10OE", "listS10ML"];// golf
var arrDdl11 = ["listS11Hdp", "listS11ML"];// motosports
var arrDdl99 = ["listS99Hdp", "listS99OU", "listS99OE", "listS99ML"];// othersports

// declare array that contains id of
var arr1Super1 = ["listS1OUD", "listS11stHdpD", "listS11stOUD", "listS1OE"];
var arr2Super1 = ["listS1CS", "listS1TG", "listS1MP", "listS1OR"];
var arr3Super1 = ["listS1OUL", "listS11stHdpL", "listS11stOUL"];

var arr1Master1 = ["listM1OUD", "listM11stHdpD", "listM11stOUD", "listM1OE"];
var arr2Master1 = ["listM1CS", "listM1TG", "listM1MP", "listM1OR"];
var arr3Master1 = ["listM1OUL", "listM11stHdpL", "listM11stOUL"];

var arrSuper2 = ["listS2OU", "listS2OE", "listS2MP", "listS2ML"];// basketball
var arrSuper3 = ["listS3OU", "listS3OE", "listS3ML"];// football
var arrSuper5 = ["listS5OU", "listS5OE", "listS5ML"];// tennis
var arrSuper8 = ["listS8OU", "listS8ML"];// baseball
var arrSuper10 = ["listS10OU", "listS10OE", "listS10ML"];// golf
var arrSuper11 = ["listS11ML"];// motosports
var arrSuper201 = ["listS201OU", "listS201OE"]; // financial
var arr2Super201 = ["listS201OE"]; // financial
var arrSuper99 = ["listS99OU", "listS99OE", "listS99ML"];// othersports

var arrMaster2 = ["listM2OU", "listM2OE", "listM2MP", "listM2ML"];// basketball
var arrMaster3 = ["listM3OU", "listM3OE", "listM3ML"];// football
var arrMaster5 = ["listM5OU", "listM5OE", "listM5ML"];// tennis
var arrMaster8 = ["listM8OU", "listM8ML"];// baseball
var arrMaster10 = ["listM10OU", "listM10OE", "listM10ML"];// golf
var arrMaster11 = ["listM11ML"];// motosports
var arrMaster201 = ["listM201OU", "listM201OE"]; // financial
var arr2Master201 = ["listM201OE"]; // financial
var arrMaster99 = ["listM99OU", "listM99OE", "listM99ML"];// othersports
var flag = false;
var gisCopyAll = false;
var FI_Sporttype = "201";
function BtnOnClick() {
    ClearAllMsg();
	var isEdit = (obj("hidEdit") != null ? true : false);
	var arrAcc;
	var arrPAcc;
	var arrMin;
	if (!flag) {
		if (!CheckInfoValue(isEdit))
			return false;
		if (!isEdit) {
			if (obj('rdWeekly') != null && !CheckTime())
				return false;
		} else {
		}

		if (!CheckMaxPerBall161()) return false;
		// check info for Soccer
		if (!CheckLimitMinMaxBet(1))
			return false;
		// -----end----
		var isMult = (obj("isMult") == null || obj("isMult").value == "1");
		if (isMult || !obj("chkCopyAll").checked) {
			// check info for Basketball
			if (!CheckLimitMinMaxBet(2))
				return false;
			// -----end----
			if (isMult || !obj("chkCopyBasket").checked) {
				// check info for Football
				if (!CheckLimitMinMaxBet(3))
					return false;
				// -----end----
				// check info for Tennis
				if (!CheckLimitMinMaxBet(5))
					return false;
				// -----end----
				// check info for Baseball
				if (!CheckLimitMinMaxBet(8))
					return false;
				// -----end----
				// check info for Golf
				if (!CheckLimitMinMaxBet(10))
					return false;
				// -----end----
				// check info for MotoSports
				if (!CheckLimitMinMaxBet(11))
				    return false;
				if (obj('txtMinbet161')) {
				    if (!CheckLimitMinMaxBet(161))
				        return false;
				}
				// -----end----
				//check info for mixed sports
				if (!CheckLimitMinMaxBet('99_9'))
				    return false;
				// -----end----
				// check info for MotoSports
				if (obj("txtMinbet201") || obj("txtMaxbet201")
						|| obj("txtMaxPerMatch201")) {
					if (!CheckLimitMinMaxBet(201))
						return false;
				}
				// -----end----
				// check info for Other Sports
				if (!CheckLimitMinMaxBet(99))
					return false;					
				// -----end----
				//check info for mixed sports
				if (!CheckLimitMinMaxBet('99_9'))
					return false;					
				// -----end----
				if (obj('chkdisableHR')) {
					if (!CheckLimitMinMaxBet(151))
						return false;
				}
			}
		}
		
		if (obj("isAllowUpd").value == "1") {
			if (!isEdit) {
				flag = true;
				PTBlockUI();
				document.fUpdate.submit();
			} else {
				if (CheckChangeAutoUpdAgent()) {
					// member maximum position taking
					obj("maxMemberPT").value = parseFloat(obj('maxM1HdpD').value)
							- parseFloat(obj("listM1HdpD").value);
					PTBlockUI();
					confirmPopup = window
							.open("ConfirmEditAgent.aspx", "ConfirmEdit",
									"top=250,left=400,width=350,height=140,resizable,scrollbars=no,dependent");
					return false;
				} else {
					obj("memberPT").value = 0;
					obj("isUpdateMember").value = 0;
					PTBlockUI();
					document.fUpdate.submit();
				}
			}
		}
		return true;
	}
}
function CheckChangeAutoUpdAgent() {
	var elements = obj("divUpdPT1").getElementsByTagName("input");
	// this script for Multiple Edit and allow update PT
	var isMult = false;
	if (obj("isMult")) {
		isMult = (obj("isMult").value == "1" ? true : false);
	}
	if (isMult) {
		var sportId = ["1", "2", "3", "5", "8", "10", "11", "201", "99", "161"];
		var isUpd;
		for (var i = 0; i < sportId.length; i++) {
			if (obj("chkUpdPT" + sportId[i]).checked) {
				isUpd = true;
				break;
			}
		}
		if (isUpd)
			return true;
	}
	// ----------------------------
	// this script to check for Soccer
	var isNewAuto;
	for (var i = 0; i < elements.length; i++) {
		var id = elements[i].id;

		if (id == "chkCopy1HdpDeadball" || id == "chkCopy11x2"
				|| id == "chkCopy1HdpLive") {
			continue;
		}
		var isAutoId = id.replace("chkAuto", "isAuto");
		var isAuto = (obj(isAutoId)) ? obj(isAutoId).value == "1" : false;
		if (isAuto && !elements[i].checked) {
			return true;
		}
	}

	
	// ----------------------------
	// this script to check for All Sporttype (except Soccer)
	sportId = ["2", "3", "5", "8", "10", "11", "201", "99", "161"];
	for (var j = 0; j < sportId.length; j++) {
		if (obj("divUpdPT" + sportId[j])) {
			elements = obj("divUpdPT" + sportId[j])
					.getElementsByTagName("input");
			for (var i = 0; i < elements.length; i++) {
				var isAutoId = elements[i].id.replace("chkAuto", "isAuto");
				var isAuto = (obj(isAutoId)) ? obj(isAutoId).value == "1" : false;
				// this script for Copy from Handicap is checked
				// if(elements[i].id=="chkCopy" + sportId[j] + "Hdp") continue;
				if (obj("chkCopy" + sportId[j] + "Hdp")
						&& obj("chkCopy" + sportId[j] + "Hdp").checked == true) {
					isNewAuto = elements[1].checked;
				} else if ((!obj("chkCopy" + sportId[j] + "Hdp"))
						|| (obj("chkCopy" + sportId[j] + "Hdp") && obj("chkCopy"
								+ sportId[j] + "Hdp").checked == false)) {

					isNewAuto = elements[i].checked;
				}

				if (isAuto && !isNewAuto) {
					return true;
				}
			}
		}
	}
	// ----------------------------
	return false;
}

function CheckAutoPreset(chkAutoId, chkCopy) {

	var isChk = false;
	if (chkAutoId) {
		isChk = chkAutoId.checked;
	}
	var isCopy = (chkCopy != null ? obj(chkCopy).checked : false);
	
	var objPreset;
	var objAuto;
	var objMaster = "";
	if (chkAutoId) {
		objMaster = chkAutoId.id.replace("chkAuto", "listM");
	}
	if (isChk) {
		if (chkAutoId) {
			objPreset = chkAutoId.id.replace("chkAuto", "listS");
			objAuto = chkAutoId.id.replace("chkAuto", "autoS");
			chkAutoId.disabled = (isCopy ? isCopy : !isChk);
		}
	} else {
		if (chkAutoId) {
			objPreset = chkAutoId.id.replace("chkAuto", "listS");
			objAuto = chkAutoId.id.replace("chkAuto", "autoS");
			chkAutoId.disabled = (isCopy ? isCopy : isChk);
		}
	}
	if (obj("chkCopyAll") != null) {
		if (!obj("chkCopyAll").checked && !isCopy) // this script for Copy All
													// PT checkbox is checked
		{
			if (obj(objMaster)) {
				obj(objMaster).disabled = false;
			}
		}
	}
	if (obj(objPreset)) {
		obj(objPreset).disabled = (isCopy ? isCopy : isChk);
	}
	if (obj(objAuto)) {
		obj(objAuto).disabled = (isCopy ? isCopy : !isChk);
	}
	// ******** may be this function is make IE slow *************
	if (!gisCopyAll) {
		PresetValuePT(chkAutoId);
	}
	// ***********************************************************
}
function PresetValuePT(chkAutoId) {
	var isAuto = chkAutoId.checked;
	var masterId = chkAutoId.id.replace("chkAuto", "listM");
	var maxMasterId = chkAutoId.id.replace("chkAuto", "maxA");
	var curMaster = obj(masterId).value;
	var minMaster = obj(masterId).options[obj(masterId).length - 1].value;

	minMaster = parseInt(round(minMaster * 100));
	var maxMaster = obj(maxMasterId).value;
	maxMaster = parseInt(round(maxMaster * 100));
	if (!isAuto) {
		var superId = chkAutoId.id.replace("chkAuto", "listS");
		var maxMSId = chkAutoId.id.replace("chkAuto", "maxM");
		var curSuper = parseFloat(obj(superId).value);
		var maxMS = parseFloat(obj(maxMSId).value);
		var msVal = maxMS - curSuper;
		msVal = parseInt(round(msVal * 100));
		maxMaster = (maxMaster > msVal ? msVal : maxMaster);
		var tmp = parseFloat(curSuper) + parseFloat(curMaster);
		if (tmp > maxMS)
			curMaster = round((maxMS - curSuper));
	}
	obj(masterId).length = 0;
	var isRange = true;
	if ((curMaster * 100) > maxMaster) {
		isRange = false;
		obj(masterId).options.add(new Option(curMaster, curMaster));
	}
	while (maxMaster >= minMaster) {
		var val = maxMaster / 100;
		obj(masterId).options.add(new Option(val, val));
		maxMaster -= 1;
	}

	if (isRange) {
		obj(masterId).value = curMaster;
	}
}
function ActiveSuperPreset(isCopyBasket) {
   
	var arrId1 = obj("divUpdPT1").getElementsByTagName("input");
	var isUpdPT1 = (obj("chkUpdPT1") != null ? obj("chkUpdPT1").checked : true);
	var isMult = (obj("isMult") != null ? obj("isMult").value == "1" : false);
	var chkCopyId;
	// this case for Enable or disable control of Soccer

	if (!isCopyBasket) {
		if (isUpdPT1) {
           
			for (var i = 0; i < arrId1.length; i++) {
                
				if (arrId1[i].id.indexOf("chkCopy1") > -1
						|| arrId1[i].id.indexOf("chkUpd") > -1)
					continue;
				if(arrId1[i].id=="btnReduce1" || arrId1[i].id=="btnIncrease1" || arrId1[i].id=="btnReduce10" || arrId1[i].id=="btnIncrease10" ) continue;	
				switch (arrId1[i].id) {

					case "chkAuto1HdpD" : {

						gisCopyAll = true;
						CheckAutoPreset(arrId1[i]);
						chkCopyId = "chkCopy1HdpDeadball";
						break;
					}
					case "chkAuto11x2" : {
						gisCopyAll = true;
						CheckAutoPreset(arrId1[i]);
						chkCopyId = "chkCopy11x2";
						break;
					}
					case "chkAuto1HdpL" : {
						gisCopyAll = true;
						CheckAutoPreset(arrId1[i]);
						chkCopyId = "chkCopy1HdpLive";
						break;
					}
					default : {

						gisCopyAll = true;
						// CheckAutoPreset(arrId1[i]);
						CheckAutoPreset(arrId1[i], chkCopyId);
						break;
					}
				}
				obj("chkCopy1HdpDeadball").disabled = false;
				obj("chkCopy11x2").disabled = false;
				obj("chkCopy1HdpLive").disabled = false;
			}
		}
		gisCopyAll = false;
	}
	
	// this case for Enable or disable control of all Sporttypes (except Soccer)
	var sportId = ["2", "3", "5", "8", "10", "11", "201", "99", "151"];
	
	for (var i = 0; i < sportId.length; i++) {
		if (sportId[i] != "151") {
			// this script to check PostionTaking checkbox is check or not
			isUpdPT1 = (obj("chkUpdPT" + sportId[i]) != null ? obj("chkUpdPT"
					+ sportId[i]).checked : true);
			if (!isUpdPT1)
				continue;
			
			// --------------------------
			var pre = "Hdp";
			if (i == 6)
				pre = "OU";

			if (obj("divUpdPT" + sportId[i])) {
				var arrId2 = obj("divUpdPT" + sportId[i])
						.getElementsByTagName("input");

				for (var j = 0; j < arrId2.length; j++) {
					if (arrId2[j].id.indexOf("chkCopy") > -1
							|| arrId2[j].id.indexOf("chkUpd") > -1)
						continue;
					if(arrId2[j].id=="btnReduce1" || arrId2[j].id=="btnIncrease1" || arrId2[j].id=="btnReduce10" || arrId2[j].id=="btnIncrease10" ) continue;		
					if (arrId2[j].id == "chkAuto" + sportId[i] + pre) {
						gisCopyAll = true;
						CheckAutoPreset(arrId2[j]);
					} else if (arrId2[j].id == "chkAuto" + sportId[i] + "OE"
							&& sportId[i] == FI_Sporttype) // Finance, chkAuto
															// for Odd/Even
					{
						gisCopyAll = true;
						CheckAutoPreset(arrId2[j], "chkCopy" + sportId[i]
										+ "OU");
					} else {
						gisCopyAll = true;
						CheckAutoPreset(arrId2[j], "chkCopy" + sportId[i]
										+ "Hdp");
					}
					if (obj("chkCopy" + sportId[i] + "OU")) { // finance, copy
																// value from
																// OVER/UNDER
						obj("chkCopy" + sportId[i] + "OU").disabled = false;
					} else if (obj("chkCopy" + sportId[i] + "Hdp")) {
						obj("chkCopy" + sportId[i] + "Hdp").disabled = false;
					}
				}
			}
		}
	}
	var isAutoMPT = obj("isAutoMPT") ? ParseToBoolean(obj("isAutoMPT").value) : false;
	if(isAutoMPT == false)
	{
		if (obj("divUpdPT1")) {
			var arrId2 = obj("divUpdPT1").getElementsByTagName("select");
		
			for (var j = 0; j < arrId2.length; j++) {
				arrId2[j].disabled = obj("chkCopyAll").checked;
			}
			CheckCopyPT(obj("chkCopy1HdpDeadball"),arr1Super1,arr1Master1,'chkAuto1HdpD')
			CheckCopyPT(obj("chkCopy11x2"),arr2Super1,arr2Master1,'chkAuto11x2');
			CheckCopyPT(obj("chkCopy1HdpLive"),arr3Super1,arr3Master1,'chkAuto1HdpL')
			if(obj("chkAuto1HdpD"))
				CheckAutoPreset(obj("chkAuto1HdpD"));
			if(obj("chkAuto11x2"))
				CheckAutoPreset(obj("chkAuto11x2"));
			if(obj("chkAuto1HdpL"))
				CheckAutoPreset(obj("chkAuto1HdpL"));
		}
	}
	
	gisCopyAll = false;
}

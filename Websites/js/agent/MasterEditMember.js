// declare array that contain id of
var arr1Super1 = ["listM1OUD", "listM11stHdpD", "listM11stOUD", "listM1OE"];
var arr2Super1 = ["listM1CS", "listM1TG", "listM1MP", "listM1OR"];
var arr3Super1 = ["listM1OUL", "listM11stHdpL", "listM11stOUL"];

var arrSuper2 = ["listM2OU", "listM2OE", "listM2MP", "listM2ML"];
// basketball
var arrSuper3 = ["listM3OU", "listM3OE", "listM3ML"];
// football
var arrSuper5 = ["listM5OU", "listM5OE", "listM5ML"];
// tennis
var arrSuper8 = ["listM8OU", "listM8ML"];
// baseball
var arrSuper10 = ["listM10OU", "listM10OE", "listM10ML"];
// golf
var arrSuper11 = ["listM11ML"];
// motosports
var arr2Super201 = ["listM201OE"];
// financial
var arrSuper201 = ["listM201OU", "listM201OE"];
// financial
var arrSuper99 = ["listM99OU", "listM99OE", "listM99ML"]; 
// othersports

// this function is used to enable or disable controls when Copy All PT checkbox
// is checked
// when Copy All PT checkbox is checked
function CopyAllPT(chkCopyAll) {
    //disable161DDL(chkCopyAll);
	if (obj("chkCopyBasket")) {
		obj("chkCopyBasket").disabled = chkCopyAll.checked;
	}
	var sportId = ["1", "2", "3", "5", "8", "10", "11", "99"];
	var arrSuper = [null, arrSuper2, arrSuper3, arrSuper5, arrSuper8,
			arrSuper10, arrSuper11, arrSuper99];
	var isMaster = (obj("roleId") != null);
	if (!chkCopyAll.checked) // this case for Copy All PT checkbox is
	// unchecked (add & edit)
	{
		for (var i = 0; i < sportId.length; i++) {
			if (i == 0) {

				if (obj("chkCopy1HdpDeadball")) {
					obj("chkCopy1HdpDeadball").disabled = false;
					CheckCopyPT(obj("chkCopy1HdpDeadball"), arr1Super1);
				}
				if (obj("chkCopy11x2")) {
					obj("chkCopy11x2").disabled = false;
					CheckCopyPT(obj("chkCopy11x2"), arr2Super1);
				}
				if (obj("chkCopy1HdpLive")) {
					obj("chkCopy1HdpLive").disabled = false;
					CheckCopyPT(obj("chkCopy1HdpLive"), arr3Super1);
				}

				if (!isMaster) {
					if (obj("chkAuto11x2")) {
						obj("chkAuto11x2").disabled = false;
						CheckAutoPreset(obj("chkAuto11x2"));
					}
					if (obj("chkAuto1HdpL")) {
						obj("chkAuto1HdpL").disabled = false;
						CheckAutoPreset(obj("chkAuto1HdpL"));
					}
				} else {
					if (obj("chkAuto11x2"))
						obj("chkAuto11x2").disabled = false;
					if (obj("chkAuto1HdpL"))
						obj("chkAuto1HdpL").disabled = false;
					CheckAutoPreset(obj("chkAuto11x2"));
					CheckAutoPreset(obj("chkAuto1HdpL"));
					CheckAutoPreset(obj("chkAuto1HdpD"));
					if (obj("listM11x2")) {
						obj("listM11x2").disabled = false;
					}
					if (obj("listM1HdpL")) {
						obj("listM1HdpL").disabled = false;
					}
				}
				continue;
			}
			var pre = "Hdp";
			
			var chkAutoId = "chkAuto" + sportId[i] + pre;
			// elements=obj("divUpdPT" +
			// sportId[i]).getElementsByTagName("input");

			if (obj("chkCopy" + sportId[i] + "OU")) { // phuong.nguyen, new
				// bet type
				obj("chkCopy" + sportId[i] + "OU").disabled = false;
				CheckCopyPT(obj("chkCopy" + sportId[i] + "OU"), arrSuper[i]);
			} else if (obj("chkCopy" + sportId[i] + "Hdp")) {
				obj("chkCopy" + sportId[i] + "Hdp").disabled = false;
				CheckCopyPT(obj("chkCopy" + sportId[i] + "Hdp"), arrSuper[i]);
			} else {
				CheckCopyPT(obj("chkCopyAll"), arrSuper[i]);
			}
			if (obj(chkAutoId)) {
				obj(chkAutoId).disabled = false;
				CheckAutoPreset(obj(chkAutoId));
			}
		}
	} else // this case for Copy All PT checkbox is checked (add & edit)
	{
		for (var i = 0; i < sportId.length; i++) {
			var elements = '';
			if (obj("divUpdPT" + sportId[i])) {
				if (obj("divUpdPT" + sportId[i])) {
					elements = obj("divUpdPT" + sportId[i])
							.getElementsByTagName("select");
					for (var j = 0; j < elements.length; j++) {
						var id = elements[j].id;
						if (id == "autoM1HdpD" || id == "listM1HdpD")
							continue;
						elements[j].disabled = chkCopyAll.checked;
					}
				}
			}
			if (!isMaster) {
				if (obj("divUpdPT" + sportId[i])) {
					elements = obj("divUpdPT" + sportId[i])
							.getElementsByTagName("input");
					if (obj("divUpdPT" + sportId[i])) {
						for (var j = 0; j < elements.length; j++) {
							var id = elements[j].id;
							if (id == "chkAuto1HdpD")
								continue;
							elements[j].disabled = chkCopyAll.checked;
						}
					}
				}
			} else {
				if (obj("divUpdPT" + sportId[i])) {
					elements = obj("divUpdPT" + sportId[i])
							.getElementsByTagName("input");
					if (obj("divUpdPT" + sportId[i])) {
						for (var j = 0; j < elements.length; j++) {
							var id = elements[j].id;
							if (id == "chkAuto1HdpD")
								continue;
							elements[j].disabled = chkCopyAll.checked;
						}
					}
				}
				if (i == 0) {
					if (obj("chkCopy1HdpDeadball")) {
						obj("chkCopy1HdpDeadball").disabled = chkCopyAll.checked;
					}
					if (obj("chkCopy11x2")) {
						obj("chkCopy11x2").disabled = chkCopyAll.checked;
					}
					if (obj("chkCopy1HdpLive")) {
						obj("chkCopy1HdpLive").disabled = chkCopyAll.checked;
					}
					continue;
				}
				if (obj("chkCopy" + sportId[i] + "OU")) { // phuong.nguyen,
					// new bet type for
					// FI
					obj("chkCopy" + sportId[i] + "OU").disabled = chkCopyAll.checked;
				} else if (obj("chkCopy" + sportId[i] + "Hdp")) {
					obj("chkCopy" + sportId[i] + "Hdp").disabled = chkCopyAll.checked;
				}
			}
		}
    }
    $(".btn-dec1").removeAttr("disabled");
    $(".btn-inc1").removeAttr("disabled");
    $(".btn-dec10").removeAttr("disabled");
    $(".btn-inc10").removeAttr("disabled"); 
}

// this function is used to disabled controls of all Sporttype except Soccer
// when Copy All BS and PT from Basketball
function CopyBasketPT(chkCopy) {
    //disable161DDL(chkCopy);
	var sportId = ["2", "3", "5", "8", "10", "11", "99"];
	var arrSuper = [arrSuper2, arrSuper3, arrSuper5, arrSuper8, arrSuper10,
			arrSuper11, arrSuper99];
	if (obj("chkCopyAll")) {
		var objIsAutoAgentSoccerHdp = obj("chkAuto1HdpD");
		var objHiddenAutoAgentSoccerHdp = obj(STR_ISAUTO + BETTYPE_HDP
				+ Delimiter + STR_AGENT);
		var isAutoHdpAgentSoccer = false;
		var hidAutoAgentSoccerHdp = false;
		if (objHiddenAutoAgentSoccerHdp)
			hidAutoAgentSoccerHdp = ParseToBoolean(objHiddenAutoAgentSoccerHdp.value);
		if (objIsAutoAgentSoccerHdp && hidAutoAgentSoccerHdp == true)
			isAutoHdpAgentSoccer = objIsAutoAgentSoccerHdp.checked;
		if (!chkCopy.checked)
			obj("chkCopyAll").disabled = !(isAutoHdpAgentSoccer == true)
					&& (hidAutoAgentSoccerHdp == true);
		else
			obj("chkCopyAll").disabled = true;
		// obj("chkCopyAll").disabled = chkCopy.checked;
	}
	var isMaster = (obj("roleId") != null);
	if (!chkCopy.checked) {
		for (var i = 0; i < sportId.length; i++) {
			var pre = "Hdp";
			// if (i == 6) pre = "OU";

			var chkAutoId = "chkAuto" + sportId[i] + pre;
			if (obj("divUpdPT" + sportId[i])) {
				elements = obj("divUpdPT" + sportId[i])
						.getElementsByTagName("input");
			}

			if (obj("chkCopy" + sportId[i] + "OU")) {
				// phuong.nguyen, new bettype
				obj("chkCopy" + sportId[i] + "OU").disabled = false;
				CheckCopyPT(obj("chkCopy" + sportId[i] + "OU"), arrSuper[i]);
			} else if (obj("chkCopy" + sportId[i] + "Hdp")) {
				obj("chkCopy" + sportId[i] + "Hdp").disabled = false;
				CheckCopyPT(obj("chkCopy" + sportId[i] + "Hdp"), arrSuper[i]);
			} else {
				CheckCopyPT(obj("chkCopyBasket"), arrSuper[i]);
			}

			if (obj(chkAutoId)) {
				obj(chkAutoId).disabled = false;
				CheckAutoPreset(obj(chkAutoId));
			}

		}
	} else// this case for Copy All Basketball is checked
	{
		obj("panelBasket").style.display = "";
		var isAuto = false;

		for (var i = 0; i < sportId.length; i++) {
			var elements = '';
			if (obj("divUpdPT" + sportId[i])) {
				if (obj("divUpdPT" + sportId[i])) {
					elements = obj("divUpdPT" + sportId[i])
							.getElementsByTagName("select");
					for (var j = 0; j < elements.length; j++) {
						var id = elements[j].id;
						if (id == "listM2Hdp" || id == "autoM2Hdp")
							continue;
						elements[j].disabled = chkCopy.checked;
					}
				}
			}
			if (!isMaster) {
				if (obj("divUpdPT" + sportId[i])) {
					elements = obj("divUpdPT" + sportId[i])
							.getElementsByTagName("input");
					for (var j = 0; j < elements.length; j++) {
						var id = elements[j].id;
						if (id == "chkAuto2Hdp")
							continue;
						elements[j].disabled = chkCopy.checked;
					}
				}
			} else {

				if (obj("divUpdPT" + sportId[i])) {
					elements = obj("divUpdPT" + sportId[i])
							.getElementsByTagName("input");
					for (var j = 0; j < elements.length; j++) {
						var id = elements[j].id;
						if (id == "chkAuto2Hdp")
							continue;
						elements[j].disabled = chkCopy.checked;
					}
				}
				if (obj("chkCopy" + sportId[i] + "OU")) {
					// phuong.nguyen, new bet type
					obj("chkCopy" + sportId[i] + "OU").disabled = chkCopy.checked;
				} else if (obj("chkCopy" + sportId[i] + "Hdp")) {
					obj("chkCopy" + sportId[i] + "Hdp").disabled = chkCopy.checked;
				}

			}
		}
    }
    $(".btn-dec1").removeAttr("disabled");
    $(".btn-inc1").removeAttr("disabled");
    $(".btn-dec10").removeAttr("disabled");
    $(".btn-inc10").removeAttr("disabled"); 
}

function CheckCopyPT(chkCopy, arrMaster) {
	if (chkCopy == null)
		return;
	for (var i = 0; i < arrMaster.length; i++) {
		var autoId = arrMaster[i].replace("listM", "autoM");
		var chkId = arrMaster[i].replace("listM", "chkAuto");
		var objPreset = obj(arrMaster[i]);
		var objAuto = obj(autoId);
		var objChkAuto = obj(chkId);
		if (obj("roleId") != null) // this case for Master edit Member
		{
			if (chkCopy.checked) {
				if (objPreset)
					objPreset.disabled = true;

				if (objChkAuto)
					objChkAuto.disabled = true;

				if (objAuto)
					objAuto.disabled = true;

			} else {
				if (objChkAuto) {
					objChkAuto.disabled = false;
					CheckAutoPreset(objChkAuto);
				} else {
					if (objPreset)
						objPreset.disabled = false;
				}

			}

		} else // this case for Super edit Member
		{
			if (obj(chkId)) {
				obj(chkId).disabled = chkCopy.checked;
				if (chkCopy.checked) {
					obj(arrMaster[i]).disabled = chkCopy.checked;
					obj(autoId).disabled = chkCopy.checked;
				} else {
					obj(arrMaster[i]).disabled = obj(chkId).checked;
					obj(autoId).disabled = !obj(chkId).checked;
				}
			}
		}
	}
}
var CHK_AUTO = "chkAuto";
var BETTYPE_HDPD = "HdpD";
var BETTYPE_HdpDeadball = "HdpDeadball";
var BETTYPE_HdpLive = "HdpLive";
var BETTYPE_HDPL = "HdpL";
var BETTYPE_1x2 = "1x2";
var BETTYPE_HDP = "Hdp";
var BETTYPE_OUD = "stOUD";
var BETTYPE_NBOUL = "NBOUL";
var STR_ISAUTO = "isAuto";
var STR_AGENT = "Agent";
var Delimiter = "_";
var CHK_COPYALL = "chkCopyAll";
var CHK_CopyBasket = "chkCopyBasket";
var STR_CHKCOPY = "chkCopy";

function CheckAutoPresetSoccer(objChk) { // check soccer HDP, 1x2, HDP live
	if (objChk == null)
		return;
	var sId = objChk.id.replace("chkAuto", "listM");
	var autoId = objChk.id.replace("chkAuto", "autoM");
	var objPreset = obj(sId);
	var objAuto = obj(autoId);
	if (objPreset)
		objPreset.disabled = objChk.checked;
	if (objAuto)
		objAuto.disabled = !objChk.checked;

	// copy handle
	var sporttype = parseInt(GetSporttypeFromChkAuto(objChk.id));
	var bettype = objChk.id.replace(CHK_AUTO + sporttype, "");

	if (bettype == BETTYPE_HDPD) { // handicap dead ball
		var objIsAutoHdpAgent = obj(STR_ISAUTO + BETTYPE_HDP + Delimiter
				+ STR_AGENT);
		var isAutoHdpAgent = false;
		if (objIsAutoHdpAgent)
			isAutoHdpAgent = ParseToBoolean(objIsAutoHdpAgent.value);
		if (isAutoHdpAgent == true) {
			var objChkCopyAll = obj(CHK_COPYALL);
			var objChkCopyBasket = obj("chkCopyBasket");
			var isCopyFromBasket = false;
			if (objChkCopyBasket) {
				isCopyFromBasket = objChkCopyBasket.checked;
			}
			if (isCopyFromBasket == false) {
				if (objChkCopyAll) {
					if (!objChk.checked) {
						objChkCopyAll.checked = false;
						CopyAllPT(objChkCopyAll);
					}

					objChkCopyAll.disabled = !objChk.checked;
				}
			}

			var objChkCopyHdpD = obj(STR_CHKCOPY + sporttype
					+ BETTYPE_HdpDeadball);
			if (objChkCopyHdpD) {
				if (!objChk.checked) {
					objChkCopyHdpD.checked = false;
					CheckCopyPT(objChkCopyHdpD, arr1Super1);
				}
				objChkCopyHdpD.disabled = !objChk.checked;

			}

		}
	} else if (bettype == BETTYPE_HDPL) { // handicap live ball
		var objIsAutoHdpAgent = obj(STR_ISAUTO + BETTYPE_HDPL + Delimiter
				+ STR_AGENT);
		var isAutoHdpAgent = false;

		if (objIsAutoHdpAgent)
			isAutoHdpAgent = ParseToBoolean(objIsAutoHdpAgent.value);

		if (isAutoHdpAgent == true) {
			var objChkCopyHdpL = obj(STR_CHKCOPY + sporttype + BETTYPE_HdpLive);

			if (objChkCopyHdpL) {
				objChkCopyHdpL.disabled = !objChk.checked;
				if (!objChk.checked) {

					objChkCopyHdpL.checked = false;
					CheckCopyPT(objChkCopyHdpL, arr3Super1);
				}
			}
		}
	} else if (bettype == BETTYPE_1x2) { // 1x2
		var objIsAuto1x2Agent = obj(STR_ISAUTO + BETTYPE_1x2 + Delimiter
				+ STR_AGENT);
		var isAuto1x2Agent = false;
		if (objIsAuto1x2Agent)
			isAuto1x2Agent = ParseToBoolean(objIsAuto1x2Agent.value);

		if (isAuto1x2Agent == true) {

			var objChkCopy1x2 = obj(STR_CHKCOPY + sporttype + BETTYPE_1x2);
			if (objChkCopy1x2) {
				if (!objChk.checked) {

					objChkCopy1x2.checked = false;
					CheckCopyPT(objChkCopy1x2, arr2Super1);
				}
				objChkCopy1x2.disabled = !objChk.checked;

			}
		}
	}

}

function CheckAutoPresetOthers(objChk) { // check auto for basket ball and
	// other sport HDP
	if (objChk == null)
		return;
	var sId = objChk.id.replace("chkAuto", "listM");
	var autoId = objChk.id.replace("chkAuto", "autoM");
	var objPreset = obj(sId);
	var objAuto = obj(autoId);
	if (objPreset)
		objPreset.disabled = objChk.checked;
	if (objAuto)
		objAuto.disabled = !objChk.checked;

	// copy handle
	var sporttype = parseInt(GetSporttypeFromChkAuto(objChk.id));
	var bettype = objChk.id.replace(CHK_AUTO + sporttype, "");
	
    if(bettype=="chkAutoNBOUL")
        bettype = objChk.id.replace(CHK_AUTO, "");    
    
	if (bettype == BETTYPE_HDP) { // handicap dead ball
		var objIsAutoHdpAgent = obj(STR_ISAUTO + Delimiter + "V" + sporttype
				+ BETTYPE_HDP + Delimiter + STR_AGENT);
				
		var isAutoHdpAgent = false;

		if (objIsAutoHdpAgent)
			isAutoHdpAgent = ParseToBoolean(objIsAutoHdpAgent.value);
		if (isAutoHdpAgent == true) {
			if (sporttype == 2) {
				var objChkCopyBasket = obj(CHK_CopyBasket);
				if (objChkCopyBasket) {
					objChkCopyBasket.disabled = !objChk.checked;
					if (!objChk.checked) {
						objChkCopyBasket.checked = false;
						CopyBasketPT(objChkCopyBasket);
					}
				}
			}

			var objChkCopyHdp = obj(STR_CHKCOPY + sporttype + BETTYPE_HDP);
			if (objChkCopyHdp) {
				objChkCopyHdp.disabled = !objChk.checked;
				if (!objChk.checked) {
					objChkCopyHdp.checked = false;
					eval("var arrData =" + "arrSuper" + sporttype + ";");
					// alert(arrData.length);
					CheckCopyPT(objChkCopyHdp, arrData);
				}
			}
		}
	}
	else if (bettype == BETTYPE_OUD) { // 1st Last O/U
	
		var objIsAutoHdpAgent = obj(STR_ISAUTO + Delimiter + "V1611" + BETTYPE_OUD + Delimiter + STR_AGENT);		
		var isAutoHdpAgent = false;

		if (objIsAutoHdpAgent)
			isAutoHdpAgent = ParseToBoolean(objIsAutoHdpAgent.value);
		if (isAutoHdpAgent == true) {
			
			var objChkCopyHdp = obj(STR_CHKCOPY + sporttype + BETTYPE_OUD);
			if (objChkCopyHdp) {
				objChkCopyHdp.disabled = !objChk.checked;
				if (!objChk.checked) {
					objChkCopyHdp.checked = false;
					NumberGameCopyPT('Deadball');
				}
			}
		}
	}
    else if (bettype == BETTYPE_NBOUL) { // Next Over/Under
		var objIsAutoHdpAgent = obj(STR_ISAUTO + Delimiter + "V161" + BETTYPE_NBOUL + Delimiter + STR_AGENT);		
		var isAutoHdpAgent = false;

		if (objIsAutoHdpAgent)
			isAutoHdpAgent = ParseToBoolean(objIsAutoHdpAgent.value);
		if (isAutoHdpAgent == true) {
			
			var objChkCopyHdp = obj(STR_CHKCOPY +  BETTYPE_NBOUL);
			
			if (objChkCopyHdp) {
				objChkCopyHdp.disabled = !objChk.checked;
				if (!objChk.checked) {
					objChkCopyHdp.checked = false;
					NumberGameCopyPT('Live');
				}
			}
		}
	}

}

function CheckAutoPreset(objChk) {
	if (objChk == null)
		return;
	var sId = objChk.id.replace("chkAuto", "listM");
	var autoId = objChk.id.replace("chkAuto", "autoM");
	var objPreset = obj(sId);
	var objAuto = obj(autoId);
	if (objPreset)
		objPreset.disabled = objChk.checked;
	if (objAuto)
		objAuto.disabled = !objChk.checked;

	// copy handle
	var sporttype = parseInt(GetSporttypeFromChkAuto(objChk.id));
	var bettype = objChk.id.replace(CHK_AUTO + sporttype, "");

	if (sporttype == 1) // soccer
	{
		if (bettype == BETTYPE_HDPD) { // handicap dead ball
			var objIsAutoHdpAgent = obj(STR_ISAUTO + BETTYPE_HDP + Delimiter
					+ STR_AGENT);
			var isAutoHdpAgent = false;
			if (objIsAutoHdpAgent)
				isAutoHdpAgent = ParseToBoolean(objIsAutoHdpAgent.value);
			if (isAutoHdpAgent == true) {
				var objChkCopyAll = obj(CHK_COPYALL);
				if (objChkCopyAll) {
					objChkCopyAll.disabled = !objChk.checked;
				}

				var objChkCopyHdpD = obj(STR_CHKCOPY + sporttype
						+ BETTYPE_HdpDeadball);
				if (objChkCopyHdpD) {
					objChkCopyHdpD.disabled = !objChk.checked;
				}

			}
		} else if (bettype == BETTYPE_HDPL) { // handicap live ball
			var objIsAutoHdpAgent = obj(STR_ISAUTO + BETTYPE_HDPL + Delimiter
					+ STR_AGENT);
			var isAutoHdpAgent = false;

			if (objIsAutoHdpAgent)
				isAutoHdpAgent = ParseToBoolean(objIsAutoHdpAgent.value);

			if (isAutoHdpAgent == true) {
				var objChkCopyHdpL = obj(STR_CHKCOPY + sporttype
						+ BETTYPE_HdpLive);

				if (objChkCopyHdpL) {
					objChkCopyHdpL.disabled = !objChk.checked;

				}
			}
		} else if (bettype == BETTYPE_1x2) { // 1x2
			var objIsAuto1x2Agent = obj(STR_ISAUTO + BETTYPE_1x2 + Delimiter
					+ STR_AGENT);
			var isAuto1x2Agent = false;
			if (objIsAuto1x2Agent)
				isAuto1x2Agent = ParseToBoolean(objIsAuto1x2Agent.value);

			if (isAuto1x2Agent == true) {

				var objChkCopy1x2 = obj(STR_CHKCOPY + sporttype + BETTYPE_1x2);
				if (objChkCopy1x2) {
					objChkCopy1x2.disabled = !objChk.checked;
				}
			}
		}
	} else // other sport
	{
		if (bettype == BETTYPE_HDP) { // handicap dead ball
			var objIsAutoHdpAgent = obj(STR_ISAUTO + Delimiter + "V"
					+ sporttype + BETTYPE_HDP + Delimiter + STR_AGENT);
			var isAutoHdpAgent = false;

			if (objIsAutoHdpAgent)
				isAutoHdpAgent = ParseToBoolean(objIsAutoHdpAgent.value);
			if (isAutoHdpAgent == true) {
				if (sporttype == 2) {
					var objChkCopyBasket = obj(CHK_CopyBasket);
					if (objChkCopyBasket) {
						objChkCopyBasket.disabled = !objChk.checked;
					}
				}

				var objChkCopyHdp = obj(STR_CHKCOPY + sporttype + BETTYPE_HDP);
				if (objChkCopyHdp) {
					objChkCopyHdp.disabled = !objChk.checked;
				}
			}
		}

	}

}

function GetSporttypeFromChkAuto(chkAutoId) {
	if (chkAutoId == null || chkAutoId == "")
		return 0;
	chkAutoId = chkAutoId.replace("chkAuto", "");
	chkAutoId = chkAutoId.replace("1x2", "");
	var sporttype = "";
	for (var i = 0; i < chkAutoId.length; i++) {
		var charI = chkAutoId.substring(i, i + 1);
		if (!isNaN(charI)) {
			sporttype += charI;
		} else
			break;
	}
	return sporttype;
}

function IncreasePT(ddl, step) {
	var max = obj(ddl).options[0].value;
	Increase(ddl, step, max);
}
function ReducePT(ddl, step) {
	var min = 0;
	min = obj(ddl).options[obj(ddl).length - 1].value;
	Reduce(ddl, step, min);
}

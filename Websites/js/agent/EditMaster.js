/**** THIS FILE IS ONLY USED IN ****/
//		Super/AddNewMaster.aspx
//		Super/EditMaster.aspx
//		Maccount/MEditCustomer.aspx
/*******************************************************/

//global variables:
var gisCopyAll=false; 

//declare array that contains id of
var arr1Super1=["listS1OUD","listS11stHdpD","listS11stOUD","listS1OE"];
var arr2Super1=["listS1CS","listS1TG","listS1MP","listS1OR"];
var arr3Super1=["listS1OUL","listS11stHdpL","listS11stOUL"];

var arr1Master1=["listM1OUD","listM11stHdpD","listM11stOUD","listM1OE"];
var arr2Master1=["listM1CS","listM1TG","listM1MP","listM1OR"];
var arr3Master1=["listM1OUL","listM11stHdpL","listM11stOUL"];

var arrSuper2=["listS2OU","listS2OE","listS2MP","listS2ML"];// basketball
var arrSuper3=["listS3OU","listS3OE","listS3ML"];// football
var arrSuper5=["listS5OU","listS5OE","listS5ML"];// tennis
var arrSuper8=["listS8OU","listS8ML"];// baseball
var arrSuper10=["listS10OU","listS10OE","listS10ML"];// golf
var arrSuper11=["listS11ML"];// motosports
var arrSuper201 = ["listS201OU", "listS201OE"]; // financial
var arr2Super201 = ["listS201OE"]; // financial
var arrSuper99=["listS99OU","listS99OE","listS99ML"];// othersports

var arrMaster2=["listM2OU","listM2OE","listM2MP","listM2ML"];// basketball
var arrMaster3=["listM3OU","listM3OE","listM3ML"];// football
var arrMaster5=["listM5OU","listM5OE","listM5ML"];// tennis
var arrMaster8=["listM8OU","listM8ML"];// baseball
var arrMaster10=["listM10OU","listM10OE","listM10ML"];// golf
var arrMaster11=["listM11ML"];// motosports
var arrMaster201 = ["listM201OU", "listM201OE"]; // financial
var arr2Master201 = ["listM201OE"]; // financial
var arrMaster99=["listM99OU","listM99OE","listM99ML"];// othersports

var FI_SportType = "201";

//this function is used to enable or disable controls in Multiple edit case
function AllowUpdate(chkObj,isBet)
{			
	var divId=chkObj.id;
	divId=divId.replace("chk","div");
	var isMaster=(obj("roleId")!=null?obj("roleId").value=="3":false);
	var element;	
	if(divId=="divUpdStatus") 
	{
		obj("chkClosed").disabled=!chkObj.checked;
		return;
	}	
	if (!chkObj.checked || chkObj.id=="chkUpdComm")
	{						
		element=obj(divId).getElementsByTagName("input");								
		for(var i=1;i<element.length;i++)
		{			
			element[i].disabled=!chkObj.checked;
		}	
		if(!isBet)
		{
			element=obj(divId).getElementsByTagName("select");					
			for(var i=0;i<element.length;i++)
			{				
				element[i].disabled=!chkObj.checked;
			}
		}	
	}	
	else
	{	//this case for Bet Setting checkbox is checked or Position Taking checkbox is checked (Super case)
		if(isBet || !isMaster)
		{	
			element=obj(divId).getElementsByTagName("input");	
			for(var i=0;i<element.length;i++)
			{			
				element[i].disabled=!chkObj.checked;					
				if(!isBet && ! isMaster)
				{
					var id=element[i].id;
					if(id.indexOf("chkCopy")>-1) continue;
					var masterId=id.replace("chkAuto","listM");
					var superId=id.replace("chkAuto","listS");
					var autoId=id.replace("chkAuto","autoS");	
					obj(masterId).disabled=!chkObj.checked;					
					obj(superId).disabled=obj(id).checked;				
					obj(autoId).disabled=!obj(id).checked;				
				}				
			}	
		}// end case
		if(isBet) return;
		if(chkObj.id=="chkUpdPT1")
		{
			if(isMaster)
			{
				obj("listM1HdpD").disabled=!chkObj.checked;
				obj("listS1HdpD").disabled=!chkObj.checked;
				obj("listM11x2").disabled=!chkObj.checked;
				obj("listS11x2").disabled=!chkObj.checked;
				obj("listM1HdpL").disabled=!chkObj.checked;
				obj("listS1HdpL").disabled=!chkObj.checked;				
				obj("chkCopy1HdpDeadball").disabled=!chkObj.checked;
				obj("chkCopy11x2").disabled=!chkObj.checked;
				obj("chkCopy1HdpLive").disabled=!chkObj.checked;
			}
			CheckCopyPT(obj("chkCopy1HdpDeadball"),arr1Super1,arr1Master1);
			CheckCopyPT(obj("chkCopy11x2"),arr2Super1,arr2Master1);
			CheckCopyPT(obj("chkCopy1HdpLive"),arr3Super1,arr3Master1);
			return;
		}		
		var sId="";
		var arrSuper;
		var arrMaster;			
		switch(chkObj.id)
		{
			case "chkUpdPT2":
			{				
				sId="chkCopy2Hdp";
				arrSuper=arrSuper2;
				arrMaster=arrMaster2;
				if(isMaster)
				{
					obj("listM2Hdp").disabled=!chkObj.checked;
					obj("listS2Hdp").disabled=!chkObj.checked;				
					obj("chkCopy2Hdp").disabled=!chkObj.checked;
				}
				break;
			}
			case "chkUpdPT3":
			{
				sId="chkCopy3Hdp";
				arrSuper=arrSuper3;
				arrMaster=arrMaster3;
				if(isMaster)
				{
					obj("listM3Hdp").disabled=!chkObj.checked;
					obj("listS3Hdp").disabled=!chkObj.checked;				
					obj("chkCopy3Hdp").disabled=!chkObj.checked;
				}
				break;
			}
			case "chkUpdPT5":
			{
				sId="chkCopy5Hdp";
				arrSuper=arrSuper5;
				arrMaster=arrMaster5;
				if(isMaster)
				{
					obj("listM5Hdp").disabled=!chkObj.checked;
					obj("listS5Hdp").disabled=!chkObj.checked;				
					obj("chkCopy5Hdp").disabled=!chkObj.checked;
				}
				break;
			}
			case "chkUpdPT8":
			{
				sId="chkCopy8Hdp";
				arrSuper=arrSuper8;
				arrMaster=arrMaster8;
				if(isMaster)
				{
					obj("listM8Hdp").disabled=!chkObj.checked;
					obj("listS8Hdp").disabled=!chkObj.checked;				
					obj("chkCopy8Hdp").disabled=!chkObj.checked;
				}
				break;
			}
			case "chkUpdPT10":
			{				
				sId="chkCopy10Hdp";
				arrSuper=arrSuper10;
				arrMaster=arrMaster10;
				if(isMaster)
				{
					obj("listM10Hdp").disabled=!chkObj.checked;
					obj("listS10Hdp").disabled=!chkObj.checked;				
					obj("chkCopy10Hdp").disabled=!chkObj.checked;
				}
				break;
			}
			case "chkUpdPT11":
			{
				sId="chkCopy11Hdp";
				arrSuper=arrSuper11;
				arrMaster=arrMaster11;
				if(isMaster)
				{
					obj("listM11Hdp").disabled=!chkObj.checked;
					obj("listS11Hdp").disabled=!chkObj.checked;				
					obj("chkCopy11Hdp").disabled=!chkObj.checked;
				}
				break;
			}
			case "chkUpdPT201":
			{
				sId="chkCopy201Hdp";
				arrSuper=arrSuper201;
				arrMaster=arrMaster201;
				if(isMaster)
				{
					obj("listM201OU").disabled=!chkObj.checked;					
				}
				break;
			}
			case "chkUpdPT99":
			{
				sId="chkCopy99Hdp";
				arrSuper=arrSuper99;
				arrMaster=arrMaster99;
				if(isMaster)
				{
					obj("listM99Hdp").disabled=!chkObj.checked;
					obj("listS99Hdp").disabled=!chkObj.checked;				
					obj("chkCopy99Hdp").disabled=!chkObj.checked;
				}
				break;
			}
		}		
		CheckCopyPT(obj(sId),arrSuper,arrMaster);
	}		
}

function BtnOnClick() {    
	var arrAcc;
	var arrPAcc;
	var arrMin;
    ClearAllMsg();
    if (!CheckInfoValue(true)) return false;

    if (!CheckMaxPerBall161()) return false;
    
    //check info for Soccer
    if(!CheckLimitMinMaxBet(1)) return false;
    //-----end----	
    if(obj("isMult").value=="1" || !obj("chkCopyAll").checked)
    {
	    //check info for Basketball
	    if(!CheckLimitMinMaxBet(2)) return false;
	    //-----end----
	    if(obj("isMult").value=="1" || !obj("chkCopyBasket").checked)
	    {
		    //check info for Football
		    if(!CheckLimitMinMaxBet(3)) return false;
		    //-----end----		
		    //check info for Tennis
		    if(!CheckLimitMinMaxBet(5)) return false;
		    //-----end----
		    //check info for Baseball
		    if(!CheckLimitMinMaxBet(8)) return false;
		    //-----end----
		    //check info for Golf
		    if(!CheckLimitMinMaxBet(10)) return false;
		    //-----end----
		    //check info for MotoSports
		    if (!CheckLimitMinMaxBet(11)) return false;

		    if (obj('txtMinbet161')) {
		        if (!CheckLimitMinMaxBet(161)) return false;
		    }
		    //-----end----
		    //check info for mixed sport
		    if (!CheckLimitMinMaxBet('99_9')) return false;
		    //-----end----
		    //check info for Financial
		    if(obj("txtMinbet201") || obj("txtMaxbet201") || obj("txtMaxPerMatch201"))
		    {
		        if(!CheckLimitMinMaxBet(201)) return false;
		    }
		    //-----end----
		    //check info for Other Sports
		    if(!CheckLimitMinMaxBet(99)) return false;
		    
		    //-----end----	
		    //  if(!CheckLimitMinMaxBet(151)) return false;		
	    }
	}
    
    if(CheckChangeAuto())
    {
	    //member maximum position taking	
	    obj("maxMemberPT").value=parseFloat(obj('maxMS1HdpD').value)-parseFloat(obj("listM1HdpD").value);                	   			
		//DisableControls();
	    PTBlockUI();   
		confirmPopup=window.open("ConfirmEditMaster.aspx","ConfirmEdit","top=250,left=400,width=350,height=140,resizable,scrollbars=no,dependent");                                                                				  
	    return false;
    }
    else {
      	obj("memberPT").value=0;
      	obj("isUpdateMember").value = 0;
      	flag=false;
      	PTBlockUI();
		document.form1.submit();    		
    }	  	
	return true;
}

//this function is Override that is used for Edit case.
var count=0;
function CheckAutoPreset(chkAutoId, chkCopy)
{	
    var isChk=false;
	if(chkAutoId)
	{
	    isChk=chkAutoId.checked;				
	}
	var isCopy=(chkCopy!=null?obj(chkCopy).checked:false);
	/*var isCopy;

	if (chkCopy != null) {
	    if (obj(chkCopy) != null)
	        isCopy = obj(chkCopy).checked;
	    else
	        alert("null");
	}
	else isCopy = false;*/
	var objPreset;
	var objAuto;
	var objMaster='';
	if(chkAutoId)
	{
	    objMaster=chkAutoId.id.replace("chkAuto","listM");	
    }	
	if (isChk)
	{
	    if(chkAutoId)
	    {
		    objPreset=chkAutoId.id.replace("chkAuto","listS");
		    objAuto=chkAutoId.id.replace("chkAuto","autoS");															
		    chkAutoId.disabled=(isCopy?isCopy:!isChk);	
		}			
	}
	else
	{
	    if(chkAutoId)
	    {
		    objPreset=chkAutoId.id.replace("chkAuto","listS");
		    objAuto=chkAutoId.id.replace("chkAuto","autoS");					
		    chkAutoId.disabled=(isCopy?isCopy:isChk);					
		}
	}
	if(obj("chkCopyAll")!=null)
	{
		if (!obj("chkCopyAll").checked && !isCopy)	//this script for Copy All PT checkbox is checked
		{	
		    if(obj(objMaster))
		    {			
			    obj(objMaster).disabled=false;			
			}
		}	
	}
	if(obj(objPreset))
	{
	    obj(objPreset).disabled=(isCopy?isCopy:isChk);	
	}
	if(obj(objAuto))
	{
	    obj(objAuto).disabled=(isCopy?isCopy:!isChk); 
	}
	//******** may be this function is make IE slow *************		
	if(!gisCopyAll)
	{		
		PresetValuePT(chkAutoId);			
	}
	//***********************************************************
}
//this function is used for Copy All PT checkbox is unchecked
function ActiveSuperPreset(isCopyBasket)
{
	var arrId1=obj("divUpdPT1").getElementsByTagName("input");	
	var isUpdPT1=(obj("chkUpdPT1")!=null? obj("chkUpdPT1").checked:true);
	var isMult=(obj("isMult")!=null?obj("isMult").value=="1":false);					
	var chkCopyId;	
	//this case for Enable or disable control of Soccer		
	if(!isCopyBasket)
	{
		if(isUpdPT1)		
		{
			for(var i=0;i<arrId1.length;i++)
			{
			   
				if(arrId1[i].id.indexOf("chkCopy1")>-1 || arrId1[i].id.indexOf("chkUpd")>-1) continue;
				if(arrId1[i].id=="btnReduce1" || arrId1[i].id=="btnIncrease1" || arrId1[i].id=="btnReduce10" || arrId1[i].id=="btnIncrease10" ) 
				{
				    continue;	
				}
			
				switch(arrId1[i].id)
				{
					case "chkAuto1HdpD":
					{
						gisCopyAll=true;
						CheckAutoPreset(arrId1[i]);
						chkCopyId="chkCopy1HdpDeadball";
						break;
					}
					case "chkAuto11x2":
					{
						gisCopyAll=true;
						CheckAutoPreset(arrId1[i]);
						chkCopyId="chkCopy11x2";
						break;
					}
					case "chkAuto1HdpL":
					{
						gisCopyAll=true;
						CheckAutoPreset(arrId1[i]);
						chkCopyId="chkCopy1HdpLive";
						break;
					}
					default:
					{
						gisCopyAll=true;
						CheckAutoPreset(arrId1[i],chkCopyId);
						break;
					}
				}										
				obj("chkCopy1HdpDeadball").disabled=false;
				obj("chkCopy11x2").disabled=false;
				obj("chkCopy1HdpLive").disabled=false;
			}
		}
		gisCopyAll=false;
	}
	//this case for Enable or disable control of all Sporttypes (except Soccer)
	var sportId=["2","3","5","8","10","11","201","99"];				 
	for(var i=0;i<sportId.length;i++)
	{
		//this script to check PostionTaking checkbox is check or not
		isUpdPT1=(obj("chkUpdPT"+ sportId[i])!=null? obj("chkUpdPT"+ sportId[i]).checked:true);
		if(!isUpdPT1) continue;
		
		//--------------------------
		var pre="Hdp";
		if(i==6) pre="OU";		
		if(obj("divUpdPT"+ sportId[i]))
		{		    
		   var arrId2=obj("divUpdPT"+ sportId[i]).getElementsByTagName("input");				
		    for(var j=0;j<arrId2.length;j++)
		    {		
		       
			    if(arrId2[j].id.indexOf("chkCopy")>-1 || arrId2[j].id.indexOf("chkUpd")>-1) continue;	
			    if(arrId2[j].id=="btnReduce1" || arrId2[j].id=="btnIncrease1" || arrId2[j].id=="btnReduce10" || arrId2[j].id=="btnIncrease10" ) 
			    {
			        continue;			
			    }
			  
			    if(arrId2[j].id=="chkAuto"+ sportId[i] +pre)
			    {			
				    gisCopyAll=true;	
				    CheckAutoPreset(arrId2[j]);
				}
				else if (arrId2[j].id == "chkAuto" + sportId[i] + "OE" && sportId[i] == FI_SportType) //Phuong.Nguyen, Finance, check Auto OE
			    {
				    gisCopyAll=true;
				    CheckAutoPreset(arrId2[j],"chkCopy"+ sportId[i] +"OU");
				}
			    else
			    {
				    gisCopyAll=true;
				    CheckAutoPreset(arrId2[j],"chkCopy"+ sportId[i] +"Hdp");
				}
				if (obj("chkCopy" + sportId[i] + "OU")) { //Phuong.Nguyen, Finance, copy value from OVER/UNDER
				    obj("chkCopy" + sportId[i] + "OU").disabled = false;
				}
			    else if(obj("chkCopy"+ sportId[i] +"Hdp"))
			    {
			        obj("chkCopy"+ sportId[i] +"Hdp").disabled=false;
			    }
		    }
		}
	}
	gisCopyAll=false;	
	
}

//this function is used to Set value for dropdown:
//when Auto checkbox is checked
//			value Master Preset PT dropdown display from min Master value to max Master value 
//else
//			value Master Preset PT dropdown display from min Master value to min(max Master value, maxMS-current Super) 			
function PresetValuePT(chkAutoId)
{
	var isAuto=chkAutoId.checked;
	var masterId=chkAutoId.id.replace("chkAuto","listM");				
	var maxMasterId=chkAutoId.id.replace("chkAuto","maxM");
	var curMaster=obj(masterId).value;									
	var minMaster=obj(masterId).options[obj(masterId).length-1].value;		
			
	minMaster=parseInt(round(minMaster*100));				
	
	var maxMaster=obj(maxMasterId).value;
	maxMaster=parseInt(round(maxMaster*100));					
	if (!isAuto)
	{
		var superId=chkAutoId.id.replace("chkAuto","listS");					
		var maxMSId=chkAutoId.id.replace("chkAuto","maxMS");
		var curSuper=parseFloat(obj(superId).value);
		var maxMS=parseFloat(obj(maxMSId).value);
		var msVal=maxMS-curSuper;
		msVal=parseInt(round(msVal*100));
		maxMaster=(maxMaster>msVal?msVal:maxMaster);
		var tmp=parseFloat(curSuper) + parseFloat(curMaster);		
		if(tmp > maxMS) curMaster=round((maxMS -curSuper));
	}
	obj(masterId).length=0;	
	var isRange = true;	
	if ((curMaster*100) >maxMaster)
	{
		isRange=false;
		obj(masterId).options.add(new Option(curMaster,curMaster));
	}	
	while(maxMaster >= minMaster)
	{
		var val=maxMaster/100;
		obj(masterId).options.add(new Option(val,val));
		maxMaster -=1;
	}
		
	if (isRange)
	{		
		obj(masterId).value=curMaster;
	}	
}
//this function is used to check
//have any PT value change from auto to none auto (alert)
function CheckChangeAuto(){
	var elements=obj("divUpdPT1").getElementsByTagName("input");
	//this script for Multiple Edit and allow update PT				
	var isMult=(obj("isMult").value=="1"?true:false);
	if(isMult)
	{
		var sportId=["1","2","3","5","8","10","11","201","99"];
		var isUpd;
		for(var i=0;i<sportId.length;i++)
		{
			if(obj("chkUpdPT" + sportId[i]).checked)
			{
				isUpd=true;
				break;
			}
		}
		if(isUpd) return true;
    }
    
	
	//----------------------------
	//this script to check for Soccer
	var isNewAuto;
	for(var i=0;i<elements.length;i++)
	{	    
		var id=elements[i].id;
		
		if(id=="chkCopy1HdpDeadball" || id=="chkCopy11x2" || id=="chkCopy1HdpLive")
		{			
			continue;
		}
		var isAutoId = id.replace("chkAuto", "isAuto");	
		var isAuto= (obj(isAutoId)) ? obj(isAutoId).value=="1" : false;
		if(isAuto && !elements[i].checked)
		{
			return true;
		}
}
//------Number Game----------
    var elementsNG = obj("divUpdPT161").getElementsByTagName("input");
    for (var i = 0; i < elementsNG.length; i++) {
        var id = elementsNG[i].id;

        if (id == "chkCopy1stOUD" || id == "chkCopyNBOUL") {
            continue;
        }
        var isAutoId = id.replace("chkAuto", "isAuto");
        var isAuto = (obj(isAutoId)) ? obj(isAutoId).value == "1" : false;
        if (isAuto && !elementsNG[i].checked) {
            return true;
        }
    }
    
	//----------------------------				
	//this script to check for All Sporttype (except Soccer)
	sportId = ["2", "3", "5", "8", "10", "11", "201", "99"];
	
	for(var j=0;j<sportId.length;j++)
	{
	    if(obj("divUpdPT" + sportId[j]))
	    {
		    elements=obj("divUpdPT" + sportId[j]).getElementsByTagName("input");		
		    for(var i=0;i<elements.length;i++)
		    {
			    var isAutoId=elements[i].id.replace("chkAuto","isAuto");
			    var isAuto = (obj(isAutoId)) ? obj(isAutoId).value == "1" : false;
			    //this script for Copy from Handicap is checked		
			    //if(elements[i].id=="chkCopy" + sportId[j] + "Hdp") continue;
    		
			    if (obj("chkCopy" + sportId[j] + "Hdp") && obj("chkCopy" + sportId[j] + "Hdp").checked==true) 
			    {				
				    isNewAuto=elements[1].checked;					
			    }
			    else if ((!obj("chkCopy" + sportId[j] + "Hdp")) || (obj("chkCopy" + sportId[j] + "Hdp") && obj("chkCopy" + sportId[j] + "Hdp").checked==false))
			    {
    			  
				    isNewAuto=elements[i].checked;
				}
			    
    			
			    if(isAuto && !isNewAuto)
			    {
				    return true;
			    }
		    }
		}					
	}	
	//----------------------------			
	return false;
}
//this function to check min PT value 
//
function CheckMinPTValue()
{
	var elements;
	var isCopyAll=obj("chkCopyAll").checked;
	//this script to check for Soccer
	elements=obj("divUpdPT1").getElementsByTagName("input");
	for(var i=0;i<elements.length;i++)
	{
		var chkId=elements[i].id;
		if(chkId.indexOf("chkCopy1")>-1) continue;		
	}
}

function WaitingReload(msg, isupdate) {
   
    PTUnBlockUI();
    var url = self.location.href.replace(/#/gi, "");
    if (url.indexOf("&msg") == -1) url = url + '&errorcode=0' + '&msg=' + msg;
    self.location.href = url;
//    if (opener) {
//       close();
//    }
}

function showEnrichMsg(msg) {
    $('#err').html(msg);
}

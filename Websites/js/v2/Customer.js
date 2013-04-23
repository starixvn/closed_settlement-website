//this function just only input is number
function keyUp(e, object)
{
    if (object.value =='') return;
    
    var stack = object;
	var curr =stack.value;
	var numstr ='0123456789';
	var flag =false;		
	var hasdec =false;
	var arrnum ='';		
	for (var i =0; i <curr.length; i++) 
	{
		var currLetter =curr.substring(i,i+1);			
		if (currLetter ==".") 
		{
			if (hasdec ==true) continue;
			hasdec =true;
		}	
		else if (numstr.indexOf(currLetter) ==-1) 
		{					
			continue;				
		}							
		arrnum +=currLetter;
		flag =true;			
	}
	stack.value = arrnum;	
}


//this function is used to visible or invisible sporttype area when menu sporttype is click
//panelId: id of <tr> tag that contains sporttype info
function OpenPanelEdit(panelId)
{				
	if(document.getElementById(panelId).style.display=="none")
	{
		document.getElementById(panelId).style.display="";
	}
	else
	{					
		document.getElementById(panelId).style.display="none";
	}				
}

//this function to check value input of min bet, max bet, max per match
//min bet <=maxbet <= max per match
//sporttypeId: is of sporttype 
function CheckLimitMinMaxBet(sporttypeId)			
{			
	try
	{
	    obj('errMsg').innerHTML = '';
	    
		var minbetId="txtMinbet" + sporttypeId;
		var maxbetId="txtMaxbet"+ sporttypeId;
		var maxpermatchId="txtMaxPerMatch" + sporttypeId;	
		var lmMinbetId="txtMinbet_Min"+sporttypeId;
		var lmMaxbetId="txtMaxbet_Max"+sporttypeId;
		var maxpermatchMinId="txtMaxPerMatch_Min"+sporttypeId;
		var maxpermatchMaxId="txtMaxPerMatch_Max"+sporttypeId;	
		
		/*if(sporttypeId==1)				
		{
			alert(obj(lmMinbetId) + " " + obj(lmMaxbetId)+ " " + obj(maxpermatchMinId) +" " + obj(maxpermatchMaxId));
			alert("minbet="+isNumeric(minbetId)+ "maxbet="+isNumeric(maxbetId)+"maxpermatch="+isNumeric(maxpermatchId));
		}*/
		
		if(isEmpty(minbetId) || isString(minbetId))
		{						
			//ShowError(alertMinbet,"error" + sporttypeId)
			showAlertMessage("error" + sporttypeId,alertMinbet,1)
			try 
			{		
			    resetFocusMinMaxBet(sporttypeId);
			    obj(minbetId).focus();
			    return false;
			}
			catch(ex)
			{
			    resetFocusMinMaxBet(sporttypeId);
			    obj(minbetId).focus();
			    return false;						
			}
		}
		else
		{
		    ShowError("","error" + sporttypeId)
		}
			
		if(isNumeric(minbetId)!=0 && isNumericSpan(lmMinbetId)!=0 && isNumeric(minbetId)<isNumericSpan(lmMinbetId))
		{			
			//ShowError(alertminbetC + " " + isNumericSpan(lmMinbetId),"error" + sporttypeId);
			showAlertMessage("error" + sporttypeId,alertminbetC + " " + isNumericSpan(lmMinbetId),1)
			
			try
			{
			    resetFocusMinMaxBet(sporttypeId);
			    obj(minbetId).focus();
			    return false;
			}
			catch(ex)
			{
			    resetFocusMinMaxBet(sporttypeId);
			    obj(minbetId).focus();
			    return false;						
			}			
		}	
		else
		{
		    ShowError("","error" + sporttypeId)
		}				
		
		if(isEmpty(maxbetId) || isString(maxbetId))
		{
			//ShowError(alertMaxbet,"error" + sporttypeId);
			showAlertMessage("error" + sporttypeId, alertMaxbet, 1);
			try
			{
			    resetFocusMinMaxBet(sporttypeId);
			    obj(maxbetId).focus();
			    return false;
			}
			catch(ex)
			{
			    resetFocusMinMaxBet(sporttypeId);
			    obj(maxbetId).focus();
			    return false;						
			}
		}
		else
		{
		    ShowError("","error" + sporttypeId)
		}
		
		if(isNumeric(maxbetId)!=0 && isNumericSpan(lmMaxbetId)!=0 && isNumeric(maxbetId)>isNumericSpan(lmMaxbetId))
		{
			//ShowError(alertMaxbetC + " " + isNumericSpan(lmMaxbetId),"error" + sporttypeId);
			showAlertMessage("error" + sporttypeId, alertMaxbet,1);
			try
			{
			    resetFocusMinMaxBet(sporttypeId);
			    obj(maxbetId).focus();
			    return false;
			}
			catch(ex)
			{
			    resetFocusMinMaxBet(sporttypeId);
			    obj(maxbetId).focus();
			    return false;						
			}
		}
		else
		{
		    ShowError("","error" + sporttypeId)
		}
		
		if(isNumeric(minbetId)>isNumeric(maxbetId))
		{
			//ShowError(,"error" + sporttypeId);
			showAlertMessage("error" + sporttypeId, alertCompare3,1);
			try
			{
			    resetFocusMinMaxBet(sporttypeId);
			    obj(minbetId).focus();
			    return false;
			}
			catch(ex)
			{
			    resetFocusMinMaxBet(sporttypeId);
			    obj(minbetId).focus();
			    return false;
			}
		}
		else
		{
		    ShowError("","error" + sporttypeId)
		}
							
		if(isEmpty(maxpermatchId) || isString(maxpermatchId))
		{
			//ShowError(alertMaxpermatch,"error" + sporttypeId);
			showAlertMessage("error" + sporttypeId, alertMaxpermatch,1);
			try
			{
			    resetFocusMinMaxBet(sporttypeId);
			    obj(maxpermatchId).focus();
			    return false;
			}
			catch(ex)
			{
			    resetFocusMinMaxBet(sporttypeId);
			    resetFocusMinMaxBet(sporttypeId);
			    obj(maxpermatchId).focus();
			    return false;
			}
		}	
		else
		{
		    ShowError("","error" + sporttypeId)
		}
					
		if(isNumeric(maxbetId)>isNumeric(maxpermatchId))
		{
			//alert('Maxbet: ' + isNumeric(maxbetId) + '\nMaxpermatch: ' + isNumeric(maxpermatchId));
			//ShowError(alertCompare,"error" + sporttypeId);
			showAlertMessage("error" + sporttypeId, alertCompare,1);
			try
		    {
		        resetFocusMinMaxBet(sporttypeId);
			    obj(maxbetId).focus();
			    return false;
			}
			catch(ex)
			{
			    resetFocusMinMaxBet(sporttypeId);    
			    obj(maxbetId).focus();
			    return false;
			}
		}
		else
		{
		    ShowError("","error" + sporttypeId)
		}
		
		if(isNumeric(maxpermatchId)!=0 && (isNumericSpan(maxpermatchMinId)!=0 || isNumericSpan(maxpermatchMaxId)))
		{
			if(isNumeric(maxpermatchId)< isNumericSpan(maxpermatchMinId))
			{
				var str=alertMaxpermatchC.replace("{1}",isNumericSpan(maxpermatchMinId));
				str=str.replace("{2}",isNumericSpan(maxpermatchMaxId));
				//ShowError(str,"error" + sporttypeId);
				showAlertMessage("error" + sporttypeId, str,1);
				try
				{
				    resetFocusMinMaxBet(sporttypeId);
				    obj(maxpermatchId).focus();
				    return false;
				}
				catch(ex)
				{
				    resetFocusMinMaxBet(sporttypeId);
				    obj(maxpermatchId).focus();
				    return false;
				}
			}	
			else if(isNumeric(maxpermatchId)> isNumericSpan(maxpermatchMaxId))		
			{
			    var str=alertMaxpermatchC.replace("{1}",isNumericSpan(maxpermatchMaxId));
				str=str.replace("{2}",isNumericSpan(maxpermatchMaxId));
				//ShowError(str,"error" + sporttypeId);
				showAlertMessage("error" + sporttypeId, str,1);
				try
				{
				    resetFocusMinMaxBet(sporttypeId);
				    obj(maxpermatchId).focus();
				    return false;
				}
				catch(ex)
				{
				    resetFocusMinMaxBet(sporttypeId);
				    obj(maxpermatchId).focus();
				    return false;
				}
			}			
		}
		else
		{
		    ShowError("","error" + sporttypeId)
		}		
	}
	catch(ex)
	{
		alert("Error here");
		return false;
	}
	return true;
}

function resetFocusMinMaxBet(sporttypeId)
{
    if(sporttypeId == 1 && obj('panelSoccer').style.display == 'none') OpenPanelEdit('panelSoccer');
    if(sporttypeId == 2 && obj('panelBasket').style.display == 'none') OpenPanelEdit('panelBasket');
    if(sporttypeId == 3 && obj('panelFootball').style.display == 'none') OpenPanelEdit('panelFootball');
    if(sporttypeId == 5 && obj('panelTennis').style.display == 'none') OpenPanelEdit('panelTennis');
    if(sporttypeId == 8 && obj('panelBaseball').style.display == 'none') OpenPanelEdit('panelBaseball');
    if(sporttypeId == 10 && obj('panelGolf').style.display == 'none') OpenPanelEdit('panelGolf');
    if(sporttypeId == 11 && obj('panelMoto').style.display == 'none') OpenPanelEdit('panelMoto');
    if(sporttypeId == 12 && obj('panelFinancial').style.display == 'none') OpenPanelEdit('panelFinancial');
    if(sporttypeId == 99 && obj('panelOtherSports').style.display == 'none') OpenPanelEdit('panelOtherSports');
}

function ChangeValuePT(ddlAgent,currentMaster,maxMaster,maxAgent,minAgent)
{    
	var valueAgent=parseFloat(round(maxMaster-currentMaster));
	var maxA=parseFloat(round(maxAgent));	
	if(valueAgent > maxA)
	{
		valueAgent=maxA;                
	}            
	var minAgent2=0;
	if(minAgent !=null)
	{
		minAgent2=parseInt(minAgent*100);
	}
	obj(ddlAgent).options.length=0;
	valueAgent=parseInt(valueAgent*100);
	while(valueAgent>=minAgent2)
	{
		var val=parseFloat(valueAgent/100);
		obj(ddlAgent).options.add(new Option(val,val));
		valueAgent=valueAgent-1;
	}       
}

//this function is used to increase value in PT dropdown when increase button is clicked
//ddl: id of PT dropdown that want to reduce
//step: gap that is increase in each click times (it's 1 or 10)
//when pt value of account is changed it's not effect to upline
//when pt value of upline is changed, PT value of current account is changed following
function IncreasePT(ddl,step)
{
	var max=obj(ddl).options[0].value;              
	if(ddl.indexOf("listM")>-1)
	{               
		Increase(ddl,step,max);
	} 
	else if(ddl.indexOf("listSP")>-1)
	{
		Increase(ddl,step,max);
	}   
	else if(ddl.indexOf("listS")>-1)
	{	
		if(Increase(ddl,step,max)){ 
			var ddlMaster=ddl.replace("listS","listM");                
			var hidMMax=ddl.replace("listS","maxM");
			var hidMsMax=ddl.replace("listS","maxMS");
			var hidMinM=ddl.replace("listS","minM");	                    
			var currentSuper=obj(ddl).value;
			//ChangeValueAgentPT(ddlMaster,currentSuper,obj(hidMsMax).value,obj(hidMMax).value,obj(hidMinM).value);
			ChangeValuePT(ddlMaster,currentSuper,obj(hidMsMax).value,obj(hidMMax).value);
		}
	}
	            
}

//this function is used to reduce value in PT dropdown when reduce button is clicked
//ddl: id of PT dropdown that want to reduce
//step: gap that is reduce in each click times (it's 1 or 10)
//when pt value of account is changed it's not effect to upline
//when pt value of upline is changed, PT value of current account is changed following
function ReducePT(ddl,step)
{
	var min=0;            
	if(ddl.indexOf("listSP")>-1) 
	{
		Reduce(ddl,step,min);
	}
	else if(ddl.indexOf("listM")>-1)
	{
		min=obj(ddl).options[obj(ddl).length-1].value;					
		Reduce(ddl,step,min);
	}
	else if(ddl.indexOf("listS")>-1)
	{
		var ddlMaster=ddl.replace("listS","listM");                
		var hidMMax=ddl.replace("listS","maxM");
		var hidMsMax=ddl.replace("listS","maxMS");
		var hidMinM=ddl.replace("listS","minM");
		var maxMaster=obj(ddl).options[0].value;
		if(Reduce(ddl,step,min)){                    
			currentSuper=obj(ddl).value;
			//ChangeValueAgentPT(ddlMaster,currentSuper,obj(hidMsMax).value,obj(hidMMax).value,obj(hidMinM).value);
			ChangeValuePT(ddlMaster,currentSuper,obj(hidMsMax).value,obj(hidMMax).value);
		}
	}           
}       


// this function is used to increase value of dropdown when increase button is clicked
//ddl: id of Positiontaking dropdown 
//step: value of each step (1 or 10)
// max : max value which dropdown can increase to			
function Increase(ddl,step,max)
{
	var objDDL=obj(ddl);
	var maxValue=Math.round(max*100);
	var currSelect=Math.round(parseFloat(objDDL.value)*100);
	var nextValue=currSelect+step;                        
	if(nextValue<=maxValue)
	{
		objDDL.value=nextValue/100.0;
		return true;
	}
	return false;
}

// this function is used to Reduce value of dropdown when button is clicked
//ddl: id of Positiontaking dropdown 
//step: value of each step (1 or 10)
// min : min value which dropdown can reduce to
function Reduce(ddl,step,min)
{
	var objDDL=obj(ddl);
	var minValue=Math.round(min*100);
	var currSelect=Math.round(parseFloat(objDDL.value)*100);
	var nextValue=currSelect-step;                        
	if(nextValue>=minValue)
	{
		objDDL.value=nextValue/100.0;
		return true;
	}
	return false;
}  

function scrollTop()
{
    scroll(0,0);
}
function CheckInfoValue(isEdit)
{
	var isMult=(obj("isMult")!=null?obj("isMult").value:"");	
	if (isMult=="1") return true; 	
	if(!isEdit)
	{	
	    if(isEmpty("txtUsername"))
		{
			ShowError(alertUserName,"err");
			obj("txtUsername").focus();
			scrollTop();
			return false;
		}	
		if(isEmpty("txtPwd"))
		{
			ShowError(alertPasswordEmpty,"err");
			obj("txtPwd").focus();
			scrollTop();
			return false;
		}						
						
		/*if(!validPass(6,obj("txtPwd").value))
		{						
			ShowError(lblPasswordRequire,"err");
			obj("txtPwd").focus();
			return false;
		}*/
	}	
	if(isEmpty("txtMaxCredit") || isString("txtMaxCredit"))
	{
		ShowError(alertIncorrectCredit,"err");
		obj("txtMaxCredit").focus();
		scrollTop();
		return false;
	}
	var credit = parseFloat(replaceAll(obj('txtMaxCredit').value, ',', ''));
	var mincredit = parseFloat(replaceAll(obj('spCreditMin').innerHTML, ',', ''));
	if(credit < mincredit)
	{
		ShowError(alertMinLessCredit + ' ' + mincredit,"err");
		obj("txtMaxCredit").focus();
		scrollTop();
		return false;
	}
	return true;				
}

//this function is used to:
//check selected PT value of current account update & his upline can not less than min value
//arrDDL: array contains id of PT dropdowns for current account (Ex: Master)
//arrPDDL: array contains id of PT dropdowns for upline (Ex: Super)
//arrMin: array contains id of hidden fields that save min value
//errHolder: id of Label that display error according sportstype.
function CheckMinPTValue(arrDDL,arrPDDL,arrMin,errHolder)
{		
	for(var i=0;i<arrDDL.length;i++)
	{
		//alert("master:"+obj(arrDDL[i])+" super:"+obj(arrPDDL[i])+" min:"+obj(arrMin[i]));
		var totalPT= isNumeric(arrDDL[i]) + isNumeric(arrPDDL[i]);
		var minVal=isNumeric(arrMin[i]);
		if(totalPT<minVal)
		{
			var msg=errMsgMinPT + minVal;
			ShowError(msg,errHolder);
			obj(arrDDL[i]).focus();
			return false;
		}
	}
	return true;
}

//this function is used to enable or disable controls when Copy All PT checkbox is checked
//when Copy All PT checkbox is checked
function CopyAllPT(chkCopyAll)
{	
	obj("chkCopyBasket").disabled=chkCopyAll.checked;
	var isMult=(obj("isMult")==null? false:(obj("isMult").value=="1"));	
	if (isMult) return;
	var isEdit=(obj("hidEdit")==null?false:(obj("hidEdit").value=="1"));
	var sportId=["1","2","3","5","8","10","11","99"];
	if(!chkCopyAll.checked) //this case for Copy All PT checkbox is unchecked (add & edit)
	{
		obj("chkCopyBasket").disabled=chkCopyAll.checked;
		obj("listM1HdpD").disabled=(obj("chkUpdPT1")==null? false:!obj("chkUpdPT1").checked);// chkCopyAll.checked;
		obj("listM11x2").disabled=(obj("chkUpdPT1")==null? false:!obj("chkUpdPT1").checked);// chkCopyAll.checked;
		obj("listM1HdpL").disabled=(obj("chkUpdPT1")==null? false:!obj("chkUpdPT1").checked);// chkCopyAll.checked;
		obj("listM2Hdp").disabled=(obj("chkUpdPT2")==null? false:!obj("chkUpdPT2").checked);// chkCopyAll.checked;
		obj("listM3Hdp").disabled=(obj("chkUpdPT3")==null? false:!obj("chkUpdPT3").checked);// chkCopyAll.checked;
		obj("listM5Hdp").disabled=(obj("chkUpdPT5")==null? false:!obj("chkUpdPT5").checked);// chkCopyAll.checked;
		obj("listM8Hdp").disabled=(obj("chkUpdPT8")==null? false:!obj("chkUpdPT8").checked);// chkCopyAll.checked;
		obj("listM10Hdp").disabled=(obj("chkUpdPT10")==null? false:!obj("chkUpdPT10").checked);// chkCopyAll.checked;
		obj("listM11Hdp").disabled=(obj("chkUpdPT11")==null? false:!obj("chkUpdPT11").checked);// chkCopyAll.checked;
		obj("listM99Hdp").disabled=(obj("chkUpdPT99")==null? false:!obj("chkUpdPT99").checked);// chkCopyAll.checked;
		for(var i=0;i<sportId.length;i++)
		{
			if(i==0) continue;
			elements=obj("divUpdInfo" + sportId[i]).getElementsByTagName("input");
			for(var j=0;j<elements.length;j++)
			{				
				elements[j].disabled=chkCopyAll.checked;
			}
		}
		if (!isEdit) //this script for Adding case
		{
			CheckAutoPreset(obj("chkAuto1"),arrDdl1);					
			CheckAutoPreset(obj("chkAuto2"),arrDdl2);
			CheckAutoPreset(obj("chkAuto3"),arrDdl3);
			CheckAutoPreset(obj("chkAuto5"),arrDdl5);
			CheckAutoPreset(obj("chkAuto8"),arrDdl8);
			CheckAutoPreset(obj("chkAuto10"),arrDdl10);
			CheckAutoPreset(obj("chkAuto11"),arrDdl11);
			CheckAutoPreset(obj("chkAuto99"),arrDdl99);
			var arr1=["chkAuto1","chkAuto2","chkAuto3","chkAuto5","chkAuto8","chkAuto10","chkAuto11","chkAuto99","chkCopy1HdpDeadball"];
			var arr2=["chkCopy11x2","chkCopy1HdpLive","chkCopy2Hdp","chkCopy3Hdp","chkCopy5Hdp","chkCopy8Hdp","chkCopy10Hdp","chkCopy11Hdp","chkCopy99Hdp"];
			for(var i=0;i<9;i++)
			{
				obj(arr1[i]).disabled=chkCopyAll.checked;
				obj(arr2[i]).disabled=chkCopyAll.checked;
			}
		}	
		else	
		{
			ActiveSuperPreset();
			if(isMult)  //this script for Multiple Edit case
			{				
				for(var i=0;i<sportId.length;i++)
				{
					var chkBetId="chkUpdInfo" + sportId[i];
					var chkPTId="chkUpdPT" + sportId[i];
					obj(chkBetId).disabled=false;
					obj(chkPTId).disabled=false;
				}
				obj("chkAuto1HdpD").disabled=(obj("chkUpdPT1")==null? false:!obj("chkUpdPT1").checked);
				obj("autoS1HdpD").disabled=(obj("chkUpdPT1").checked?!obj("chkAuto1HdpD").checked:true);				
				obj("listS1HdpD").disabled=(obj("chkUpdPT1").checked?obj("chkAuto1HdpD").checked:true);	
			}
		}
	}
	else //this case for Copy All PT checkbox is checked (add & edit)
	{		
		var isAuto=false;				
		for(var i=0;i<sportId.length;i++)
		{
			var elements=obj("divUpdPT" + sportId[i]).getElementsByTagName("select");
			for(var j=0;j<elements.length;j++)
			{
				var id=elements[j].id;
				if(id=="listM1HdpD" || id=="listS1HdpD" || id=="autoS1HdpD") continue;
				elements[j].disabled=chkCopyAll.checked;
			}
			elements=obj("divUpdPT" + sportId[i]).getElementsByTagName("input");
			for(var j=0;j<elements.length;j++)
			{
				var id=elements[j].id;
				if(id=="chkAuto1HdpD" || id=="chkUpdPT" + sportId[i]) continue;
				elements[j].disabled=chkCopyAll.checked;
			}
			if(i==0) continue;
			elements=obj("divUpdInfo" + sportId[i]).getElementsByTagName("input");
			for(var j=0;j<elements.length;j++)
			{				
				elements[j].disabled=chkCopyAll.checked;
			}
		}		
	}				
}

//this function is used to disbale or enable dropdown positon according Copy Group
function CheckCopyPT(chkCopy,arrSuper,arrMaster,chkAutoId)
{				
	var isEdit=(obj("hidEdit")!=null?obj("hidEdit").value:"");
	var isCopy=chkCopy.checked;
	var isAuto=(chkAutoId!=null?obj(chkAutoId).checked:false);
	for(var i=0;i<arrMaster.length;i++)
	{
		obj(arrMaster[i]).disabled=chkCopy.checked;			
		var id=arrSuper[i].replace("listS","autoS");
		if (!isEdit)
		{
			if(!isAuto)
			{
				obj(arrSuper[i]).disabled=chkCopy.checked;
			}
		}
		else
		{		
			var chkId=arrSuper[i].replace("listS","chkAuto");	
			CheckAutoPreset(obj(chkId),chkCopy.id);			
		}
	}
}		

function GetArrSoccer(chkCopyId)
{
	var arrAcc;
	var arrPAcc;
	var arrMin;
	//this case for Copy All PT checkbox is checked
	if(obj("chkCopyAll").checked)
	{
		switch(chkCopyId)
		{
			case "chkCopy1HdpDeadball":
			{
				arrMin=["minMS1HdpD","minMS1OUD","minMS11stHdpD","minMS11stOUD","minMS1OE"];
				arrAcc=["listM1HdpD","listM1HdpD","listM1HdpD","listM1HdpD","listM1HdpD"];
				if(!obj("chkAuto1").checked)
				{
					arrPAcc=["listS1HdpD","listS1HdpD","listS1HdpD","listS1HdpD","listS1HdpD"];
				}
				else
				{
					arrPAcc=["autoS1HdpD","autoS1HdpD","autoS1HdpD","autoS1HdpD","autoS1HdpD"];
				}
				break;
			}
			case "chkCopy11x2":
			{
				arrAcc=["listM1HdpD","listM1HdpD","listM1HdpD","listM1HdpD","listM1HdpD"];
				arrMin=["minMS11x2","minMS1CS","minMS1TG","minMS1MP","minMS1OR"];
				if(!obj("chkAuto1").checked)
				{
					arrPAcc=["listS1HdpD","listS1HdpD","listS1HdpD","listS1HdpD","listS1HdpD"];
				}
				else
				{
					arrPAcc=["autoS1HdpD","autoS1HdpD","autoS1HdpD","autoS1HdpD","autoS1HdpD"];
				}
				break;							
			}
			case "chkCopy1HdpLive":
			{
				arrAcc=["listM1HdpD","listM1HdpD","listM1HdpD","listM1HdpD"];
				arrMin=["minMS1HdpL","minMS1OUL","minMS11stHdpL","minMS11stOUL"];
				if(!obj("chkAuto1").checked)
				{
					arrPAcc=["listS1HdpD","listS1HdpD","listS1HdpD","listS1HdpD"];
				}
				else
				{
					arrPAcc=["autoS1HdpD","autoS1HdpD","autoS1HdpD","autoS1HdpD"];
				}
				break;							
			}
		}
		this.arr1=arrAcc;
		this.arr2=arrPAcc;
		this.arr3=arrMin			
		return [this.arr1,this.arr2,this.arr3];
	}//-------------end case ---------------------
	switch(chkCopyId)
	{
		case "chkCopy1HdpDeadball":
		{
			if(obj("chkCopy1HdpDeadball").checked)
			{
				arrAcc=["listM1HdpD","listM1HdpD","listM1HdpD","listM1HdpD","listM1HdpD"];					
				arrMin=["minMS1HdpD","minMS1OUD","minMS11stHdpD","minMS11stOUD","minMS1OE"];
				if(!obj("chkAuto1").checked)
				{
					arrPAcc=["listS1HdpD","listS1HdpD","listS1HdpD","listS1HdpD","listS1HdpD"];
				}
				else
				{
					arrPAcc=["autoS1HdpD","autoS1HdpD","autoS1HdpD","autoS1HdpD","autoS1HdpD"];
				}
			}
			else
			{
				arrAcc=["listM1HdpD","listM1OUD","listM11stHdpD","listM11stOUD","listM1OE"];					
				arrMin=["minMS1HdpD","minMS1OUD","minMS11stHdpD","minMS11stOUD","minMS1OE"];
				if(!obj("chkAuto1").checked)
				{
					arrPAcc=["listS1HdpD","listS1OUD","listS11stHdpD","listS11stOUD","listS1OE"];
				}
				else
				{
					arrPAcc=["autoS1HdpD","autoS1OUD","autoS11stHdpD","autoS11stOUD","autoS1OE"];
				}
			}
			break;
		}
		case "chkCopy11x2":
		{
			if(obj("chkCopy11x2").checked)
			{
				arrAcc=["listM11x2","listM11x2","listM11x2","listM11x2","listM11x2"];					
				arrMin=["minMS11x2","minMS1CS","minMS1TG","minMS1MP","minMS1OR"];
				if(!obj("chkAuto1").checked)
				{
					arrPAcc=["listS11x2","listS11x2","listS11x2","listS11x2","listS11x2"];
				}
				else
				{
					arrPAcc=["autoS11x2","autoS11x2","autoS11x2","autoS11x2","autoS11x2"];
				}
			}
			else
			{
				arrAcc=["listM11x2","listM1CS","listM1TG","listM1MP","listM1OR"];					
				arrMin=["minMS11x2","minMS1CS","minMS1TG","minMS1MP","minMS1OR"];
				if(!obj("chkAuto1").checked)
				{
					arrPAcc=["listS11x2","listS1CS","listS1TG","listS1MP","listS1OR"];
				}
				else
				{
					arrPAcc=["autoS11x2","autoS1CS","autoS1TG","autoS1MP","autoS1OR"];
				}
			}
			break;
		}
		case "chkCopy1HdpLive":
		{
			if(obj("chkCopy1HdpLive").checked)
			{
				arrAcc=["listM1HdpL","listM1HdpL","listM1HdpL","listM1HdpL"];					
				arrMin=["minMS1HdpL","minMS1OUL","minMS11stHdpL","minMS11stOUL"];
				if(!obj("chkAuto1").checked)
				{
					arrPAcc=["listS1HdpL","listS1HdpL","listS1HdpL","listS1HdpL"];
				}
				else
				{
					arrPAcc=["autoS1HdpL","autoS1HdpL","autoS1HdpL","autoS1HdpL"];
				}
			}
			else
			{
				arrAcc=["listM1HdpL","listM1OUL","listM11stHdpL","listM11stOUL"];					
				arrMin=["minMS1HdpL","minMS1OUL","minMS11stHdpL","minMS11stOUL"];
				if(!obj("chkAuto1").checked)
				{
					arrPAcc=["listS1HdpL","listS1OUL","listS11stHdpL","listS11stOUL"];
				}
				else
				{
					arrPAcc=["autoS1HdpL","autoS1OUL","autoS11stHdpL","autoS11stOUL"];
				}
			}
			break;
		}
	}
	this.arr1=arrAcc;
	this.arr2=arrPAcc;
	this.arr3=arrMin			
	return [this.arr1,this.arr2,this.arr3];
}

//this function to get arrays that will use in checking Min Positon
//use for basketball, football, tennis, golf, other sports 
function GetArr4(sporttypeId)
{
	var arrAcc;
	var arrPAcc;
	var arrMin;
	arrMin=["minMS"+ sporttypeId +"Hdp","minMS"+ sporttypeId +"OU","minMS"+ sporttypeId +"OE","minMS"+ sporttypeId +"ML"];
	//this case is used in Copy All PT checkbox is checked
	if(obj("chkCopyAll").checked)
	{
		arrAcc=["listM1HdpD","listM1HdpD","listM1HdpD","listM1HdpD"];	
		if(!obj("chkAuto1").checked)				
		{
			arrPAcc=["listS1HdpD","listS1HdpD","listS1HdpD","listS1HdpD"];
		}
		else
		{
			arrPAcc=["autoS1HdpD","autoS1HdpD","autoS1HdpD","autoS1HdpD"];
		}
		this.arr1=arrAcc;
		this.arr2=arrPAcc;
		this.arr3=arrMin			
		return [this.arr1,this.arr2,this.arr3];
	}//------- end case -----------
	if(obj("chkCopy"+ sporttypeId +"Hdp").checked)
	{
		arrAcc=["listM"+ sporttypeId +"Hdp","listM"+ sporttypeId +"Hdp","listM"+ sporttypeId +"Hdp","listM"+ sporttypeId +"Hdp"];					
		if(!obj("chkAuto"+ sporttypeId).checked)
		{
			arrPAcc=["listS"+ sporttypeId +"Hdp","listS"+ sporttypeId +"Hdp","listS"+ sporttypeId +"Hdp","listS"+ sporttypeId +"Hdp"];
		}
		else
		{
			arrPAcc=["autoS"+ sporttypeId +"Hdp","autoS"+ sporttypeId +"Hdp","autoS"+ sporttypeId +"Hdp","autoS"+ sporttypeId +"Hdp"];
		}
	}
	else
	{
		arrAcc=["listM"+ sporttypeId +"Hdp","listM"+ sporttypeId +"OU","listM"+ sporttypeId +"OE","listM"+ sporttypeId +"ML"];					
		if(!obj("chkAuto"+ sporttypeId ).checked)
		{
			arrPAcc=["listS"+ sporttypeId +"Hdp","listS"+ sporttypeId +"OU","listS"+ sporttypeId +"OE","listS"+ sporttypeId +"ML"];
		}
		else
		{
			arrPAcc=["autoS"+ sporttypeId +"Hdp","autoS"+ sporttypeId +"OU","autoS"+ sporttypeId +"OE","autoS"+ sporttypeId +"ML"];
		}
	}	
	this.arr1=arrAcc;
	this.arr2=arrPAcc;
	this.arr3=arrMin			
	return [this.arr1,this.arr2,this.arr3];
}

function GetArrBaseball()
{
	var arrAcc;
	var arrPAcc;
	var arrMin;
	arrMin=["minMS8Hdp","minMS8OU","minMS8ML"];
	if(obj("chkCopyAll").checked)		
	{		
		arrAcc=["listM1HdpD","listM1HdpD","listM1HdpD"];
		if(!obj("chkAuto1").checked)
		{
			arrPAcc=["listS1HdpD","listS1HdpD","listS1HdpD"];
		}
		else
		{
			arrPAcc=["autoS1HdpD","autoS1HdpD","autoS1HdpD"];
		}
		this.arr1=arrAcc;
		this.arr2=arrPAcc;
		this.arr3=arrMin			
		return [this.arr1,this.arr2,this.arr3];
	}
	
	if(obj("chkCopy8Hdp").checked)
	{
		arrAcc=["listM8Hdp","listM8Hdp","listM8Hdp"];			
		if(!obj("chkAuto8").checked)
		{
			arrPAcc=["listS8Hdp","listS8Hdp","listS8Hdp"];
		}
		else
		{
			arrPAcc=["autoS8Hdp","autoS8Hdp","autoS8Hdp"];
		}
	}
	else
	{
		arrAcc=["listM8Hdp","listM8OU","listM8ML"];			
		if(!obj("chkAuto8").checked)
		{
			arrPAcc=["listS8Hdp","listS8OU","listS8ML"];
		}
		else
		{
			arrPAcc=["autoS8Hdp","autoS8OU","autoS8ML"];
		}
	}	
	this.arr1=arrAcc;
	this.arr2=arrPAcc;
	this.arr3=arrMin			
	return [this.arr1,this.arr2,this.arr3];
}

function GetArrMoto()
{
	var arrAcc;
	var arrPAcc;
	var arrMin;
	arrMin=["minMS11Hdp","minMS11ML"];
	if(obj("chkCopyAll").checked)		
	{		
		arrAcc=["listM1HdpD","listM1HdpD"];
		if(!obj("chkAuto1").checked)
		{
			arrPAcc=["listS1HdpD","listS1HdpD"];
		}
		else
		{
			arrPAcc=["autoS1HdpD","autoS1HdpD"];
		}
		this.arr1=arrAcc;
		this.arr2=arrPAcc;
		this.arr3=arrMin			
		return [this.arr1,this.arr2,this.arr3];
	}	
	if(obj("chkCopy11Hdp").checked)
	{
		arrAcc=["listM11Hdp","listM11Hdp"];			
		if(!obj("chkAuto11").checked)
		{
			arrPAcc=["listS11Hdp","listS11Hdp"];
		}
		else
		{
			arrPAcc=["autoS11Hdp","autoS11Hdp"];
		}
	}
	else
	{
		arrAcc=["listM11Hdp","listM11ML"];			
		if(!obj("chkAuto11").checked)
		{
			arrPAcc=["listS11Hdp","listS11ML"];
		}
		else
		{
			arrPAcc=["autoS11Hdp","autoS11ML"];
		}
	}
	this.arr1=arrAcc;
	this.arr2=arrPAcc;
	this.arr3=arrMin			
	return [this.arr1,this.arr2,this.arr3];		
}
//this function is used to disabled controls of all Sporttype except Soccer 
//when Copy All BS and PT from Basketball
function CopyBasketPT(chkCopy)
{
	var isEdit=(obj("hidEdit")!=null?true:false);
	var isMult=(obj("isMult")!=null?obj("isMult").value=="1":false)
	if (isMult) return;
	var isCopyBas=chkCopy.checked;
	var sportId=["2","3","5","8","10","11","99"];	
	obj("chkCopyAll").disabled=chkCopy.checked;
	if(chkCopy.checked) 
	{
		obj("panelBasket").style.display="";		
	}
	if(isEdit && !isCopyBas)
	{
		ActiveSuperPreset(true);
	}
	for(var i=0;i<sportId.length;i++)
	{
		var elements;	
		if(isEdit)//	
		{
			elements=obj("divUpdPT"+sportId[i]).getElementsByTagName("input");
			{
				for(var j=0;j<elements.length;j++)
				{
					var id=elements[j].id;
					if(id=="chkAuto2Hdp" || id=="chkUpdPT"+sportId[i]) continue;
					elements[j].disabled=chkCopy.checked;			
				}					
			}
		}		
		if(isCopyBas || !isEdit)
		{
			elements=obj("divUpdPT"+sportId[i]).getElementsByTagName("select");
			obj("chkCopy"+ sportId[i]+"Hdp").disabled=chkCopy.checked;
			for(var j=0;j<elements.length;j++)
			{
				var id=elements[j].id;
				if(id=="listM2Hdp" || id=="listS2Hdp") continue;
				elements[j].disabled=chkCopy.checked;			
			}
		}
		if(i==0) continue;
		elements=obj("divUpdInfo" + sportId[i]).getElementsByTagName("input");
		for(var j=0;j<elements.length;j++)
		{
			elements[j].disabled=chkCopy.checked;
		}
	}
	if(isEdit)
	{
		obj("listS2Hdp").disabled=obj("chkAuto2Hdp").checked;
		obj("autoS2Hdp").disabled=!obj("chkAuto2Hdp").checked;
	}
}

//this function is used to change value of current account when PT dropdown value of upline is changed
//uplineDDLId: id of PT dropdown that is clicked
// ex current account is Master
//    upline is Super
function SelectOnChange(uplineDDLId)			
{		
	if(uplineDDLId.indexOf("listS")>-1)	
	{	
		var accDDL=uplineDDLId.replace("listS","listM");  				
		var maxMS;					
		var maxMaster;	
		var isMaster=(obj("roleId")!=null?obj("roleId").value=="3":false);		
		if (isMaster)//this for Master Edit
		{
			maxMS=obj(uplineDDLId).options[0].value;	
			maxMaster=obj(uplineDDLId.replace("listS","maxA")).value
		}
		else //this for Super Edit
		{
			maxMS=uplineDDLId.replace("listS","maxMS");		
			maxMS=obj(maxMS).value;
			maxMaster=obj(uplineDDLId.replace("listS","maxM")).value;			
		}
		var masterVal=parseFloat(maxMS) - parseFloat(obj(uplineDDLId).value);					
		//alert(parseFloat(obj(maxMS).value) + " " +parseFloat(obj(uplineDDLId).value) + " " +masterVal);
		masterVal=parseInt(round(masterVal*100));								
		if(masterVal >parseInt(maxMaster*100))
		{
			masterVal=parseInt(round(maxMaster*100));
		}
		//for Editing
		var minMaster=0;
		minMaster=obj(accDDL).options[obj(accDDL).length-1].value
		minMaster=parseInt(round(minMaster * 100));
		//					
		obj(accDDL).options.length=0;					
		while(masterVal >= minMaster)
		{
			var val=masterVal/100;
			obj(accDDL).options.add(new Option(val,val));
			masterVal -=1;
		}	
	}			
}
////////////Hanh commented ///////////////
//this function is used display error message that according sporttype is contained in span			
/*function ShowError(errMsg,placeHolder)
{
	var arrPlaceHolder=["err","error1","error2","error3","error5","error8","error10","error11", "error12","error99"];
	var isEdit=(obj("hidEdit")!=null?true:false);
	for(var i=0;i<arrPlaceHolder.length;i++)
	{
		if(placeHolder==arrPlaceHolder[i])
		{
			alert_error(errMsg,placeHolder);
		}
		else
		{						
			obj(arrPlaceHolder[i]).innerHTML="";
		}
	}
	if(isEdit)
	{
		obj("btnSubmit").style.color="#ffffff";
		obj("btnSubmit").disabled=false;
	}
	else
	{
		obj("btnAdd").style.color="#ffffff";
		obj("btnAdd").disabled=false;
	}
}*/
/////////////////////////////////////////
//check Numeric input field by id , return value
function isNumericSpan(id)
{
    return parseFloat(ReplaceAll(obj(id).innerHTML,',',''));
}

//Find all occurrence of regexp in str and replace it with newstr
function ReplaceAll(str,regexp,newstr)
{	
	return str.replace(eval('/' +regexp +'/g'), newstr);
}
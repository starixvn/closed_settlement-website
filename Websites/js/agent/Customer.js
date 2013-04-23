//this function is called in onload event of body tag
// to get Custid string & Username string from MasterList page for the case Multiple Edit 
//assign Username string to span username
function GetCustID()
{				
	var strCustid= parent.document.getElementById("arrayCustID");// hidden field from MasterList page
	var objCustid=document.getElementById("saveCustid");
	if(strCustid)
	{
		if (strCustid.value !="")
		{
			objCustid.value=strCustid.value;
		}
	}
	var strUsername=parent.document.getElementById("arrayUserName")	;// hidden field from MasterList page
	var objUsername=document.getElementById("saveUsername");
	if (strUsername)
	{
		if (strUsername.value != "")
		{						
			var t = strUsername.value;						
			while(t.indexOf("-") != -1)
			{
				t = t.replace("-",", ");
			}
			objUsername.value=t.substring(0,t.length-2);			
			if (document.getElementById("username"))
			{
				document.getElementById("username").innerHTML=objUsername.value;
			}
		}
	}							   
}

//this function is used to visible or invisible sporttype area when menu sporttype is click
//panelId: id of <tr> tag that contains sporttype info
function OpenPanelEdit(panelId)
{				
	if(obj(panelId).className.indexOf("hidden") != -1)
       {
            obj(panelId).className = obj(panelId).className.replace("hidden","show"); 
       }
       else if(obj(panelId).className.indexOf("show") != -1)
       {
            obj(panelId).className = obj(panelId).className.replace("show","hidden"); 
       }      
       else
       {
            obj(panelId).className = obj(panelId).className + " hidden";
       }				
}

//this function to check value input of min bet, max bet, max per match
//min bet <=maxbet <= max per match
//sporttypeId: is of sporttype 
function CheckLimitMinMaxBet(sporttypeId)			
{
    try {
        
        //showAlertMessage("error" + sporttypeId, 'testing', 1);

		var minbetId="txtMinbet" + sporttypeId;
		var maxbetId="txtMaxbet"+sporttypeId;
		var maxpermatchId="txtMaxPerMatch" + sporttypeId;	
		var lmMinbetId="txtMinbet_Min"+sporttypeId;
		var lmMaxbetId="txtMaxbet_Max"+sporttypeId;
		var maxpermatchMinId="txtMaxPerMatch_Min"+sporttypeId;
		var maxpermatchMaxId="txtMaxPerMatch_Max"+sporttypeId;			
		if(isEmpty(minbetId) || isString(minbetId))
		{						
			showAlertMessage("error" + sporttypeId , alertMinbetc,1)
			obj(minbetId).focus();
			return false;
        }
        
							
		if(isNumeric(minbetId)!=0 && isNumericSpan(lmMinbetId)!=0 && isNumeric(minbetId)<isNumericSpan(lmMinbetId))
		{			
			showAlertMessage("error" + sporttypeId, alertminbetC + " " + isNumericSpan(lmMinbetId),1 );
			obj(minbetId).focus();
			return false;
		}					
		
		if(isEmpty(maxbetId) || isString(maxbetId))
		{
			showAlertMessage("error" + sporttypeId, alertMaxbet,1);
			obj(maxbetId).focus();
			return false;

        }
        
		
		if(isNumeric(maxbetId)!=0 && isNumericSpan(lmMaxbetId)!=0 && isNumeric(maxbetId)>isNumericSpan(lmMaxbetId))
		{
			showAlertMessage("error" + sporttypeId, alertMaxbetC + " " + isNumericSpan(lmMaxbetId), 1);
			obj(maxbetId).focus();
			return false;
		}

		if (isNumeric(minbetId) > isNumeric(maxbetId)) {
			showAlertMessage("error" + sporttypeId, alertCompare3, 1);
			obj(minbetId).focus();
			return false;
		}
		
		if(isEmpty(maxpermatchId) || isString(maxpermatchId))
		{
			showAlertMessage("error" + sporttypeId, alertMaxpermatch, 1);
			obj(maxpermatchId).focus();
			return false;						
		}				
		if(isNumeric(maxbetId)>isNumeric(maxpermatchId)) {
			showAlertMessage("error" + sporttypeId, alertCompare, 1);
			obj(maxbetId).focus();
			return false;
		}
		
		if(isNumeric(maxpermatchId)!=0)
		{
			if(isNumeric(maxpermatchId)< isNumericSpan(maxpermatchMinId) && (isNumericSpan(maxpermatchMinId)!=0)) {
				var str=alertMaxpermatchC.replace("{1}",isNumericSpan(maxpermatchMinId));
				str=str.replace("{2}",isNumericSpan(maxpermatchMaxId));
				showAlertMessage("error" + sporttypeId, str, 1);
				obj(maxpermatchId).focus();
				return false;
			}
			else if (isNumeric(maxpermatchId)> isNumericSpan(maxpermatchMaxId) && (isNumericSpan(maxpermatchMaxId)!=0)) {
			    var str=alertMaxpermatchC.replace("{1}",isNumericSpan(maxpermatchMaxId));
				str=str.replace("{2}",isNumericSpan(maxpermatchMaxId));
				showAlertMessage("error" + sporttypeId, str, 1);
				obj(maxpermatchId).focus();
				return false;
			}
		}					
	}
	catch(ex)
	{
		//alert("Error here");
		return false;
	}
	return true;
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
	var isRange=false;	
	while(valueAgent>=minAgent2)
	{
		isRange=true;
		var val=parseFloat(valueAgent/100);
		obj(ddlAgent).options.add(new Option(val,val));
		valueAgent=valueAgent-1;
	}     
	if(!isRange) obj(ddlAgent).options.add(new Option(minAgent2/100,minAgent2/100));  
}

//this function is used to increase value in PT dropdown when increase button is clicked
//ddl: id of PT dropdown that want to reduce
//step: gap that is increase in each click times (it's 1 or 10)
//when pt value of account is changed it's not effect to upline
//when pt value of upline is changed, PT value of current account is changed following
function IncreasePT(ddl,step)
{
	var max=obj(ddl).options[0].value;   
	var roleId=(obj("roleId")!=null?obj("roleId").value:"");           
	if(ddl.indexOf("listM")>-1)
	{               
		Increase(ddl,step,max);
	} 
	else if(ddl.indexOf("autoS")>-1)
	{
		Increase(ddl,step,max);
	}   
	else if(ddl.indexOf("listS")>-1)
	{	
		if(Increase(ddl,step,max))
		{ 
			var ddlMaster=ddl.replace("listS","listM");                
			var hidMMax=ddl.replace("listS","maxM");
			var hidMsMax=ddl.replace("listS","maxMS");
			var hidMinM=ddl.replace("listS","minM");		
			switch(roleId)                    
			{
				case "3"://this case for Master
				{
					hidMMax=ddl.replace("listS","maxM"); //for current
					hidMsMax=ddl.replace("listS","maxMS"); // for upline							
					hidMinM=obj(ddlMaster).options[obj(ddlMaster).length-1].value;
					hidMsMax=obj(hidMsMax).value;
					hidMMax=obj(hidMMax).value;						
					break;
				}
                default:
                    {
                        hidMMax = ddl.replace("listS", "maxA"); //for current
                        hidMsMax = ddl.replace("listS", "maxM"); // for upline
                        hidMinM = ddl.replace("listS", "listM");
                        hidMinM = obj(hidMinM).options[obj(hidMinM).length - 1].value;
                        hidMsMax = obj(hidMsMax).value;
                        hidMMax = obj(hidMMax).value;
                        break;
                    }
			}
			var currentSuper=obj(ddl).value;			
			ChangeValuePT(ddlMaster,currentSuper,hidMsMax,hidMMax,hidMinM);
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
	var roleId=(obj("roleId")!=null?obj("roleId").value:"");
	if(ddl.indexOf("autoS")>-1) 
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
		var hidMMax;
		var hidMsMax;
		var hidMinM;		
		var maxMaster=obj(ddl).options[0].value;
		switch(roleId)
		{
			case "3":
			{
				hidMMax = ddl.replace("listS", "maxM"); // for current
				hidMsMax = ddl.replace("listS", "maxMS"); // for upline						
				hidMinM=obj(ddlMaster).options[obj(ddlMaster).length-1].value;
				hidMsMax=obj(hidMsMax).value;
				hidMMax=obj(hidMMax).value;
				break;
			}
            default:
                {
                    hidMMax = ddl.replace("listS", "maxA"); // for current
                    hidMsMax = ddl.replace("listS", "maxM"); // for upline
                    hidMinM = ddl.replace("listS", "listM");
                    hidMinM = obj(hidMinM).options[obj(hidMinM).length - 1].value;
                    hidMsMax = obj(hidMsMax).value;
                    hidMMax = obj(hidMMax).value;
                    break;
                }
		}
		if(Reduce(ddl,step,min)){                    
			currentSuper=obj(ddl).value;			
			ChangeValuePT(ddlMaster,currentSuper,hidMsMax,hidMMax,hidMinM);
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
	/*if(nextValue<=maxValue)
	{
		objDDL.value=nextValue/100.0;
		return true;
	}*/
	for(var i=0;i<objDDL.length;i++) 
	{
		if(objDDL[i].value==(nextValue/100.0))
		{
			objDDL[i].selected=true;
			return true;
		}		
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
	/*if(nextValue>=minValue)
	{		
		objDDL.value=nextValue/100.0;
		return true;	
	}	*/
	for(var i=0;i<objDDL.length;i++) 
	{		
		if(objDDL[i].value == (nextValue/100.0))
		{
			objDDL[i].selected=true;
			return true;
		}		
	}
	return false;
}  
//this function is used to check Password and Max Credit 
//in Editing case just check Max Credit
function CheckInfoValue(isEdit)
{
	var isMult=(obj("isMult")!=null?obj("isMult").value:"");	
	if (isMult=="1") return true; 	
	if(!isEdit)
	{		
	    if(isEmpty("txtUsername"))
		{
		    //ShowError(alertUserName, "err");
		    showAlertMessage("err", alertUserName, 1);
			obj("txtUsername").focus();
			return false;
		}
		if(isEmpty("txtPwd"))
		{
		    //ShowError(alertPasswordEmpty, "err");
		    showAlertMessage("err", alertPasswordEmpty, 1);
			obj("txtPwd").focus();
			return false;
		}						
		/*				
		if(!checkpass(obj("txtPwd")))
		{						
			ShowError(lblPasswordRequire,"err");
			obj("txtPwd").focus();
			return false;
		}*/
	}	
	if(isEmpty("txtCredit") || isString("txtCredit")) {

	    //ShowError(alertIncorrectCredit, "err");
	    showAlertMessage("err", alertIncorrectCredit, 1);
		obj("txtCredit").focus();
		return false;
	}	
	if((isNumeric("txtCredit")<isNumericSpan("txtCreditMin")) || (isNumeric("txtCredit")>isNumericSpan("txtCreditMax")))
	{		
		var valMin=isNumericSpan("txtCreditMin") ? isNumericSpan("txtCreditMin"):0;
		var str=alertCreditInvalid.replace("{1}", formatNumber2(valMin));
		str=str.replace("{2}", formatNumber2(isNumericSpan("txtCreditMax")));
		//ShowError(str,"err");
		showAlertMessage("err", str, 1);
		obj("txtCredit").focus()
		return false;
	}
	
	
	if( (!obj('txtMaxCredit')) ? false : isNumeric("txtMaxCredit")>0 && ( ((!obj('lblMinCredit')) ? false : (isNumeric("txtMaxCredit")<isNumericSpan("lblMinCredit"))) || ((!obj('txtMaxCredit')) ? false : (isNumeric("txtMaxCredit")>isNumericSpan("lblMaxCredit"))) ))
	{	
	    
		var valMin =  ((!obj('lblMinCredit')) ? 0 : (isNumericSpan("lblMinCredit") ? isNumericSpan("lblMinCredit"):0)) ;
		var str=alertMemberMaxCreditInvalid.replace("{1}", formatNumber2(valMin));
		str=str.replace("{2}", formatNumber2(isNumericSpan("lblMaxCredit")));
		//ShowError(str,"err");
		showAlertMessage("err", str, 1);
		obj("txtMaxCredit").focus()
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
		var totalPT= isNumeric(arrDDL[i]) + isNumeric(arrPDDL[i]);
		var minVal=isNumeric(arrMin[i]);
		if(totalPT<minVal)
		{
			var msg=errMsgMinPT + minVal;
			//ShowError(msg, errHolder);
			showAlertMessage(errHolder, str, 1);
			obj(arrDDL[i]).focus();
			return false;
		}
	}
	return true;
}

//this function is used to enable or disable controls when Copy All PT checkbox is checked (for Super)
//when Copy All Bet Setting & PT checkbox is checked
function CopyAllPT(chkCopyAll)
{	
    
	obj("chkCopyBasket").disabled=chkCopyAll.checked;
	var isMult=(obj("isMult")==null? false:(obj("isMult").value=="1"));	
	if (isMult) return;
	var isEdit=(obj("hidEdit")==null?false:(obj("hidEdit").value=="1"));
	var sportId = ["1", "2", "3", "5", "8", "10", "11", "201", "99", "151"];

	//disable161DDL(chkCopyAll);
	
	if(!chkCopyAll.checked) //this case for Copy All PT checkbox is unchecked (add & edit)
	{		
		obj("listM1HdpD").disabled=false;
		obj("listM11x2").disabled=false;
		obj("listM1HdpL").disabled=false;
		obj("listM2Hdp").disabled=false;
		obj("listM3Hdp").disabled=false;
		obj("listM5Hdp").disabled=false;
		obj("listM8Hdp").disabled=false;
		obj("listM10Hdp").disabled=false;
		obj("listM11Hdp").disabled=false;
		if(obj("listM201OU"))
		{
		    obj("listM201OU").disabled = false;
		    		    
		}
		
		obj("listM99Hdp").disabled=false;
		//this script to enable Min bet, Max Bet, Max Per Match textbox in Copy from Soccer case
	
		for(var i=0;i<sportId.length;i++)
		{
			//if(i==0) continue;
			if(obj("divUpdInfo" + sportId[i]))
			{
			    elements=obj("divUpdInfo" + sportId[i]).getElementsByTagName("input");
			    for(var j=0;j<elements.length;j++)
			    {	
				    elements[j].disabled=false;
			    }
			}
		}
        ActiveSuperPreset(); 
        
	}
	else //this case for Copy All PT checkbox is checked (add & edit)
	{		
		var isAuto=false;				
		for(var i=0;i<sportId.length;i++)
		{
		    if(sportId[i] != "151")
		    {
		        var elements="";
		        if(obj("divUpdPT" + sportId[i]))
		        {
			        elements=obj("divUpdPT" + sportId[i]).getElementsByTagName("select");
			        for(var j=0;j<elements.length;j++)
			        {
				        var id=elements[j].id;
				        if(id=="listM1HdpD" || id=="listS1HdpD" || id=="autoS1HdpD") continue;
				        elements[j].disabled=chkCopyAll.checked;
			        }
			    }
			    if(obj("divUpdPT" + sportId[i]))
			    {
			        elements=obj("divUpdPT" + sportId[i]).getElementsByTagName("input");
			        for(var j=0;j<elements.length;j++)
			        {
				        var id=elements[j].id;
				        if(id=="chkAuto1HdpD" || id=="chkUpdPT" + sportId[i] || id=="btnReduce1" || id=="btnIncrease1" || id=="btnReduce10" || id=="btnIncrease10" ) continue;
				        elements[j].disabled=chkCopyAll.checked;
			        }
			    }
			}
			if(i==0) continue;
			if(obj("divUpdInfo" + sportId[i]))
			{
			    elements=obj("divUpdInfo" + sportId[i]).getElementsByTagName("input");
			    for(var j=0;j<elements.length;j++)
			    {				
				    elements[j].disabled=chkCopyAll.checked;
			    }
			}
		}		
	}				
}

//this function is used to disbale or enable dropdown positon according Copy from Handicap/1x2/OddEven
function CheckCopyPT(chkCopy,arrSuper,arrMaster,chkAutoId)
{				
	var isEdit=(obj("hidEdit")!=null?obj("hidEdit").value=="1":false);
	var roleId=(obj("roleId")!=null ? obj("roleId").value:"");
	var isCopy=chkCopy.checked;
	var isAuto=(obj(chkAutoId) != null) ? obj(chkAutoId).checked : false;
	for(var i=0;i<arrMaster.length;i++)
	{
	    if(obj(arrMaster[i]))
	    {
		    obj(arrMaster[i]).disabled=chkCopy.checked;			
		}
		var id=arrSuper[i].replace("listS","autoS");
		var chkId=arrSuper[i].replace("listS","chkAuto");	
		gisCopyAll=true;
		if(obj(chkId))
		{
			CheckAutoPreset(obj(chkId),chkCopy.id);
		}
		else
		{
			if(obj(arrSuper[i]))
				obj(arrSuper[i]).disabled = isCopy;
		}
		gisCopyAll=false;
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

function GetArrFinancial()
{
	var arrAcc;
	var arrPAcc;
	var arrMin;
	arrMin=["minMS201OU"];
	if(obj("chkCopyAll").checked)		
	{		
		arrAcc=["listM1HdpD"];
		if(!obj("chkAuto1").checked)
		{
			arrPAcc=["listS1HdpD"];
		}
		else
		{
			arrPAcc=["autoS1HdpD"];
		}
		this.arr1=arrAcc;
		this.arr2=arrPAcc;
		this.arr3=arrMin			
		return [this.arr1,this.arr2,this.arr3];
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
//this function is used to disabled controls of all Sporttype except Soccer (for Super)
//when Copy All Bet Setting and PT from Basketball
function CopyBasketPT(chkCopy)
{ 
	var isEdit=(obj("hidEdit")!=null?true:false);
	var isMult=(obj("isMult")!=null?obj("isMult").value=="1":false)
	if (isMult) return;
	var isCopyBas=chkCopy.checked;
	var sportId = ["2", "3", "5", "8", "10", "11", "201", "99", "151"];
	//disable161DDL(chkCopy);
	obj("chkCopyAll").disabled=chkCopy.checked;
	if(chkCopy.checked) 
	{
		obj("panelBasket").style.display="";		
	}
	if (chkCopy.checked)
	{
		if(isEdit && !isCopyBas)
		{
			ActiveSuperPreset(true);
		}
		for(var i=0;i<sportId.length;i++)
		{
			var elements;	
			if(sportId[i]!="151")
			{
			    if(obj("divUpdPT"+sportId[i]))
			    {
			        if(obj("divUpdPT"+sportId[i]))
			        {
			            elements=obj("divUpdPT"+sportId[i]).getElementsByTagName("input");
			            {
				            for(var j=0;j<elements.length;j++)
				            {
					            var id=elements[j].id;
					            if(id=="chkAuto2Hdp" || id=="chkUpdPT"+sportId[i]) continue;
					            if(id=="btnReduce1" || id=="btnIncrease1" || id=="btnReduce10" || id=="btnIncrease10" ) 
			                    {
			                        continue;			
			                    }
					            elements[j].disabled=chkCopy.checked;			
				            }					
			            }
			        }
			    }		
			    if(isCopyBas)
			    {
			        if(obj("divUpdPT"+sportId[i]))
			        {
				        elements=obj("divUpdPT"+sportId[i]).getElementsByTagName("select");
				        if(obj("chkCopy"+ sportId[i]+"Hdp"))
				        {
    				        obj("chkCopy"+ sportId[i]+"Hdp").disabled=chkCopy.checked;
    				    }
    				    if (obj("chkCopy" + sportId[i] + "OU")) {
    				        obj("chkCopy" + sportId[i] + "OU").disabled = chkCopy.checked;
    				    }
				        for(var j=0;j<elements.length;j++)
				        {
					        var id=elements[j].id;
					        if(id=="listM2Hdp" || id=="listS2Hdp") continue;
					         if(id=="btnReduce1" || id=="btnIncrease1" || id=="btnReduce10" || id=="btnIncrease10" ) 
			                    {
			                        continue;			
			                    }
					        elements[j].disabled=chkCopy.checked;			
				        }
				    }
			    }
			}
			if(i==0) continue;	
			if(obj("divUpdInfo" + sportId[i]))
			{		
			    if(obj("divUpdInfo" + sportId[i]))
			    {
			        elements=obj("divUpdInfo" + sportId[i]).getElementsByTagName("input");
			        for(var j=0;j<elements.length;j++)
			        {
				        elements[j].disabled=chkCopy.checked;
			        }
			    }
			}
		}
		obj("listS2Hdp").disabled=obj("chkAuto2Hdp").checked;
		obj("autoS2Hdp").disabled=!obj("chkAuto2Hdp").checked;
	}
	else// this case for Copy All Basketball is unchecked 
	{
		var arrSuper=[arrSuper2,arrSuper3,arrSuper5,arrSuper8,arrSuper10,arrSuper11,arr2Super201,arrSuper99];
		var arrMaster=[arrMaster2,arrMaster3,arrMaster5,arrMaster8,arrMaster10,arrMaster11,arr2Master201,arrMaster99];	
		
		for(var i=0;i<sportId.length;i++)		
		{
		   
		    if(sportId[i]!="151")
		    {
			    var pre="Hdp";
			    if (i == 6) pre = "OU";
			    var chkObj = obj("chkCopy" + sportId[i] + "Hdp");
			    var chkObjOU = obj("chkCopy" + sportId[i] + "OU");		
			    if(chkObj)
			    {
			        chkObj.disabled=chkCopy.checked;
			    }
			    if (chkObjOU) {
			        chkObjOU.disabled = chkCopy.checked;
			    }		    
			    if(obj("listM" + sportId[i]+ pre))
			    {
			        obj("listM" + sportId[i]+ pre).disabled=chkCopy.checked;
			    }
			    
			    if(obj("chkAuto" + sportId[i] + pre))
			    {
			        var chkAuto=obj("chkAuto" + sportId[i] + pre);
			        chkAuto.disabled=chkCopy.checked;
			    }
			    if(obj("listS" + sportId[i]+pre))
			    {
			        obj("listS" + sportId[i]+pre).disabled=chkAuto.checked;
			    }
			    if(obj("autoS" + sportId[i]+pre))
			    {
			        obj("autoS" + sportId[i]+pre).disabled=!chkAuto.checked;
			    }

			    if (chkObjOU) {
			        CheckCopyPT(chkObjOU, arrSuper[i], arrMaster[i]);
			    }
			    
			    else if(chkObj)	
			    {
			        CheckCopyPT(chkObj,arrSuper[i],arrMaster[i]);			    
			    }
			    else
			    {
			        CheckCopyPT(obj("chkCopyBasket"),arrSuper[i],arrMaster[i]);			    
			    }
			}
			if(obj("divUpdInfo" + sportId[i]))
			{			
			   var elements=obj("divUpdInfo" + sportId[i]).getElementsByTagName("input");
			   for(var j=0;j<elements.length;j++)			
			   {
			   
			       elements[j].disabled=chkCopy.checked;
			   }
			}		
		
		}
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
		var isMaster = (obj("roleId") != null ? obj("roleId").value == "3" : false);
		if (obj("roleId2").value == "3")//this for Master Edit
		{
		    maxMS = uplineDDLId.replace("listS", "maxMS"); // for upline
		    maxMS = obj(maxMS).value;
		    maxMaster = obj(uplineDDLId.replace("listS", "maxM")).value; // for current level
		}
		else if (obj("roleId2").value == "2") {//edit agent
		    maxMS = uplineDDLId.replace("listS", "maxM"); // for upline
		    maxMS = obj(maxMS).value;
		    maxMaster = obj(uplineDDLId.replace("listS", "maxA")).value; // for current level
		}
		else //this for Super Edit
		{
			maxMS=uplineDDLId.replace("listS","maxM"); // for upline
			maxMS=obj(maxMS).value;			
			maxMaster=obj(uplineDDLId.replace("listS","maxA")).value; // for current level			
		}
		var masterVal=parseFloat(maxMS) - parseFloat(obj(uplineDDLId).value);	
		
		masterVal=parseInt(round(masterVal*100));	
		
		if(masterVal >parseInt(maxMaster*100))
		{
		    					
			masterVal=parseInt(round(maxMaster*100));
		}
		//for Editing
		var minMaster=0;
		minMaster=obj(accDDL).options[obj(accDDL).length-1].value;
			
		minMaster=parseInt(round(minMaster * 100));
		
		//					
		obj(accDDL).options.length=0;		
		var isRang=false;				
		while(masterVal >= minMaster)
		{		
			isRang=true;
			var val=masterVal/100;
			obj(accDDL).options.add(new Option(val,val));
			masterVal -=1;
		}	
		if (!isRang) obj(accDDL).options.add(new Option(minMaster/100,minMaster/100));
	}			
}

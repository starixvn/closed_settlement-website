// JScript File
////////////**** this file is used in Customer/v2/AddNewMember.aspx
//									  Customer/MasterAgentList.aspx
//									  Customer/AgentList.aspx
//									  Customer/MemberList.aspx
//									  Customer/OperatorList.aspx
//									Customer/SuperAgentList.aspx
//									Customer/v2/AddNewSuper.aspx
//									Customer/v2/EditCustomerNew.aspx
//									Customer/v2/EditMinMaxPT.aspx

 function CopyPositionTaking(objChk,arrId,arrId2) //************ is overrided in v2/EditMinMaxPT.aspx and v2/AddNewSuper.aspx
{              
    try {
        var len =arrId.length;
        for(var i=0;i<len;i++)
        {
            var o =obj(arrId[i]);
            if (typeof o !='undefined' && o) o.disabled =objChk.checked;
        }
        var arr2=eval(arrId2);
        
        for(var i=0;i<arr2.length/2;i++)
        {
            var chkSp =obj(arr2[i]);
            var listSp =obj(arr2[i+arr2.length/2]);
            var listS=obj(arrId[i])
            var listM=obj(arrId[i+arr2.length/2])
            chkSp.disabled =objChk.checked;
            listSp.disabled =(objChk.checked || !chkSp.checked);
            if(!objChk.checked)
            {
                listM.disabled=chkSp.checked;
            }            
        }   
        
    }
    catch (ex) {
    }
}

//this function to check Username, Pwd, Max Credit
function CheckInputInfo() // *************it's not used in new pages of PT 
{
    var isEdit=(obj("custId") !=null && obj("custId").value>0);
    if(!isEdit){
        if(isEmpty("txtUsername")) {
            //alert_error("Invalid Username.", "errMsg");
            showAlertMessage("errMsg", "Invalid Username.", 1);
            obj("txtUsername").focus(); 
            scrollTop();
            return false;
        }
        if(isEmpty("txtPwd"))
        {
            //alert_error("Please input password.","errMsg");
            showAlertMessage("errMsg", "Please input password.", 1);
            obj("txtPwd").focus(); 
            scrollTop();
            return false;
        }
        else
        {
            var str = obj("txtPwd").value;
            numbers = new String("0123456789");		
            var strError="Password must have at least 6 characters,including at least 2 numbers and at least 2 letters. (e.g:pass22)";				
            if(str.length < 6)
            {
                //alert_error(strError, "errMsg");
                showAlertMessage("errMsg", strError, 1);
	            return false;
            }
            var i = 0;
            var nCount = 0;            
            while(i < str.length)
            {								
	            if(numbers.indexOf(str.substring(i,i+1)) >= 0)
	            {							
		            nCount ++;
	            }
	            i++;
            }			            
            if(nCount > (str.length -2) || nCount < 2)
            {
                //alert_error(strError, "errMsg");
                showAlertMessage("errMsg", strError, 1);
	            scrollTop();
	            return false;
            }
        }
    }
    
    if(isEmpty("txtMaxCredit") || isString("txtMaxCredit") || isNumeric("txtMaxCredit")<0)
    {
        //alert_error("Invalid Super Max Credit.", "errMsg");
        showAlertMessage("errMsg", "Invalid Super Max Credit.", 1);
        obj("txtMaxCredit").focus();
        scrollTop();
        return false;
    }   
    return true;
}

//this function to check Min Bet, Max Bet, Max Per Match 
// isCheckLimit = true if need check limit of Min Bet, Max Bet, Max Per Match 
// isCheckLimit = false (not check limit in Super case)
function CheckMinMaxBet(isCheckLimit) // *************it's not used in new pages of PT 
{
    if(isEmpty("txtMinBet") || isString("txtMinBet") || isNumeric("txtMinBet")<0)
    {
        //alert_error("Invalid Min Bet.", "errMsg");
        showAlertMessage("errMsg", "Invalid Min Bet.", 1);
        obj("txtMinBet").focus();
        return false;
    }
    if(isEmpty("txtMaxBet") || isString("txtMaxBet") || isNumeric("txtMaxBet")<0)
    {
        showAlertMessage("errMsg","Invalid Max Bet.",1 );
        obj("txtMaxBet").focus();
        return false;
    }    
    if(isEmpty("txtMaxPerMatch") || isString("txtMaxPerMatch") || isNumeric("txtMaxPerMatch")<0)
    {
        //alert_error("Invalid Max Per Match.", "errMsg");
        showAlertMessage("errMsg", "Invalid Max Per Match.", 1);
        obj("txtMaxPerMatch").focus();
        return false;
    }
    if(isNumeric("txtMinBet")!=0 && isNumeric("txtMaxBet")!=0)
    {
        if(isNumeric("txtMinBet") > isNumeric("txtMaxBet"))
        {                    
            //alert_error("Min Bet must be less than Max Bet(Soccer).","errMsg");
            showAlertMessage("errMsg", "Min Bet must be less than Max Bet(Soccer).", 1);
            obj("txtMinBet").focus();
            return false;                
        }
    }
    if(isNumeric("txtMaxBet")!=0 && isNumeric("txtMaxPerMatch")!=0)
    {
        if(isNumeric("txtMaxBet")>isNumeric("txtMaxPerMatch"))
        {
            //alert_error("Max Bet must be less than Max Per Match(Soccer).", "errMsg");
            showAlertMessage("errMsg", "Max Bet must be less than Max Per Match(Soccer).", 1);
            obj("txtMaxBet").focus();
            return false;                
        }
    }
     
    if(isNumeric("txtMinBet")!=0 && isNumeric("txtMaxPerMatch")!=0)
    {
        if(isNumeric("txtMinBet") > isNumeric("txtMaxPerMatch"))
        {                    
            //alert_error("Min Bet must be less than Max Per Match(Soccer).","errMsg");
            showAlertMessage("errMsg", "Min Bet must be less than Max Per Match(Soccer).", 1);
            obj("txtMinBet").focus();
            return false;                
        }
    }
               
    if(isEmpty("txtMinBetb") || isString("txtMinBetb") || isNumeric("txtMinBetb")<0)
    {
        //alert_error("Invalid Min Bet.", "errMsg");
        showAlertMessage("errMsg", "Invalid Min Bet.", 1);
        obj("txtMinBetb").focus();
        return false;
    }
    if(isEmpty("txtMaxBetb") || isString("txtMaxBetb") || isNumeric("txtMaxBetb")<0)
    {
        //alert_error("Invalid Max Bet.", "errMsg");
        showAlertMessage("errMsg", "Invalid Max Bet.", 1);
        obj("txtMaxBetb").focus();
        return false;
    }
    if(isEmpty("txtMaxPerMatchb") || isString("txtMaxPerMatchb") || isNumeric("txtMaxPerMatchb")<0)
    {        
        //alert_error("Invalid Max Per Match.","errMsg");
        showAlertMessage("errMsg", "Invalid Max Per Match.", 1);
        obj("txtMaxPerMatchb").focus();
        return false;
    }
    if(isNumeric("txtMinBetb")!=0 && isNumeric("txtMaxBetb")!=0){
        if(isNumeric("txtMinBetb")>isNumeric("txtMaxBetb"))
        {
            //alert_error("Min Bet must be less than Max Bet(Basket ball).", "errMsg");
            showAlertMessage("errMsg", "Min Bet must be less than Max Bet(Basket ball).", 1);
            obj("txtMinBetb").focus();
            return false;                
        }
    }
    if(isNumeric("txtMaxPerMatchb")!=0 && isNumeric("txtMaxBetb")!=0){
        if(isNumeric("txtMaxBetb")>isNumeric("txtMaxPerMatchb"))
        {
            //alert_error("Max Bet must be less than Max Per Match(Basket ball).", "errMsg");
            showAlertMessage("errMsg", "Max Bet must be less than Max Per Match(Basket ball).", 1);
            obj("txtMaxBetb").focus();
            return false;                
        }
    }
    if(isNumeric("txtMaxPerMatchb")!=0 && isNumeric("txtMinBetb")!=0){
        if(isNumeric("txtMinBetb")>isNumeric("txtMaxPerMatchb")) {
            showAlertMessage("errMsg", "Min Bet must be less than Max Bet(Basket ball).", 1);
            //alert_error("Min Bet must be less than Max Bet(Basket ball).","errMsg");
            obj("txtMinBetb").focus();
            return false;                
        }
    }
    
    if(isCheckLimit)
    {
        if(isNumeric("txtMinBet") != 0 && isNumericSpan("txtMinBetMax")!=0)
        {
            if(isNumeric("txtMinBet")< isNumericSpan("txtMinBetMax"))
            {
                //alert_error('Min Bet (soccer) must be more than or equal to '+ isNumericSpan("txtMinBetMax"),'errMsg');
                showAlertMessage("errMsg", "Min Bet (soccer) must be more than or equal to " + isNumericSpan("txtMinBetMax"), 1);
                obj("txtMinBet").focus();
                return false;
            }
        }        
        if(isNumeric("txtMaxBet") != 0 && isNumericSpan("txtMaxBetMin")!=0)
        {
            if(isNumeric("txtMaxBet")> isNumericSpan("txtMaxBetMin"))
            {
                //alert_error('Max Bet (soccer) must be less than or equal to '+ isNumericSpan("txtMaxBetMin"),'errMsg');
                showAlertMessage("errMsg", "Max Bet (soccer) must be less than or equal to " + isNumericSpan("txtMaxBetMin"), 1);
                obj("txtMaxBet").focus();
                return false;
            }
        }        
        if((isNumeric("txtMaxPerMatch") != 0 && isNumericSpan("txtMinPerMax")!=0) || ((isNumeric("txtMaxPerMatch") != 0 && isNumericSpan("txtMaxPerMin")!=0)) )
        {
            if(isNumeric("txtMaxPerMatch")> isNumericSpan("txtMinPerMax") || (isNumeric("txtMaxPerMatch")< isNumericSpan("txtMaxPerMin")))
            {
                //alert_error('Max Per Match (soccer) must be between '+ isNumericSpan("txtMaxPerMin") +' and '+ isNumericSpan("txtMinPerMax"),'errMsg');
                showAlertMessage("errMsg", "Max Per Match (soccer) must be between " + isNumericSpan("txtMaxPerMin") + " and " + isNumericSpan("txtMinPerMax"), 1);
                obj("txtMaxPerMatch").focus();
                return false;
            }
        }        
        
        if(isNumeric("txtMinBetb") != 0 && isNumericSpan("txtMinBetMaxb")!=0)
        {
            if(isNumeric("txtMinBetb")< isNumericSpan("txtMinBetMaxb"))
            {
                //alert_error('Min Bet (basketball) must be more than or equal to '+ isNumericSpan("txtMinBetMaxb"),'errMsg');
                showAlertMessage("errMsg", 'Min Bet (basketball) must be more than or equal to ' + isNumericSpan("txtMinBetMaxb"), 1);
                obj("txtMinBetb").focus();
                return false;
            }
        }        
        if(isNumeric("txtMaxBetb") != 0 && isNumericSpan("txtMaxBetMinb")!=0)
        {
            if(isNumeric("txtMaxBetb")> isNumericSpan("txtMaxBetMinb"))
            {
                //alert_error('Max Bet (basketball) must be less than or equal to '+ isNumericSpan("txtMaxBetMinb"),'errMsg');
                showAlertMessage("errMsg", 'Max Bet (basketball) must be less than or equal to ' + isNumericSpan("txtMaxBetMinb"), 1);
                obj("txtMaxBetb").focus();
                return false;
            }
        }
        
        if((isNumeric("txtMaxPerMatchb") != 0 && isNumericSpan("txtMinPerMaxb")!=0) || ((isNumeric("txtMaxPerMatchb") != 0 && isNumericSpan("txtMaxPerMinb")!=0)) )
        {
            if((isNumeric("txtMaxPerMatchb") > isNumericSpan("txtMinPerMaxb")) || ((isNumeric("txtMaxPerMatchb") < isNumericSpan("txtMaxPerMinb"))))
            {
                //alert_error('Max Per Match (basketball) must be between '+ isNumericSpan("txtMaxPerMinb") +' and '+ isNumericSpan("txtMinPerMaxb"),'errMsg');
                showAlertMessage("errMsg", 'Max Per Match (basketball) must be between ' + isNumericSpan("txtMaxPerMinb") + ' and ' + isNumericSpan("txtMinPerMaxb"), 1);
                obj("txtMaxPerMatchb").focus();
                return false;
            }
        }
    }
    obj('errMsg').innerHTML="";
    return true;
}

//this function to check value is numeric with Span control
function isNumericSpan(id)  // ********* it's same in js/common.js
{
    return parseFloat(ReplaceAll(obj(id).innerHTML,',',''));
}

// this function is used to increase value of dropdown when button is clicked
//ddl: id of Positiontaking dropdown 
//step: value of each step (1 or 10)
// max : max value which dropdown can increase to
function Increase(ddl,step,max)  // **************same in js/common.js  and overrided in js/agent/Customer.js
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
function Reduce(ddl,step,min)  // **************same in js/common.js  and overrided in js/agent/Customer.js
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
//this function is used to check min value of Position taking in Add New Master and Add New Agent
//in case Add New Master: value selected of Super Position + Master Position >= min 
//in case Add New Agent: value selected of Master Position + Agent Position >= min 
//isAuto=true: is used in Add New Master & Super Auto Preset checkbox is checked 
//      when Super Auto Preset checkbox is checked , value selected of Master Position + auto Value (is saved in hidden field)>=min
function CheckMinPT(isAuto)   //************************* not used now
{
    var arrPTIdDeadball;
    var arrMasterPTIdDeadball;
    var arrHidDeadball;
    var arrPTId;
    var arrMasterPTId;
    var arrHid;
    var arrPTIdLive;
    var arrMasterPTIdLive;
    var arrHidLive;
    var arrPTIdBs;
    var arrMasterPTIdBs;
    var arrHidBs;      
    var isEdit=(obj("custId") !=null && obj("custId").value>0);          
    var roleId=obj("roleId").value;
    if(obj("chkCopyHdpD").checked)// when Copy From Handicap checkbox of Deadball is checked
    {        
        if(isAuto)
        {            
            arrMasterPTIdDeadball=["hidHdpD"];
        }
        else
        {
            var chkAuto=(roleId==3)?obj("chkSPHdpD").checked:false;
            if(isEdit && chkAuto)
                arrMasterPTIdDeadball=["listSPHdpD"]; 
            else
                arrMasterPTIdDeadball=["listMHdpD"]; 
        }          
        arrPTIdDeadball=["listSHdpD"];       
        arrHidDeadball=["hidMinHdpD"];        
        if(!CheckValidMinPT(arrPTIdDeadball,arrMasterPTIdDeadball,arrHidDeadball))
        {
            return false;
        }
    }
    else  // when Copy From Handicap checkbox of Deadball isn't checked
    {
        if(isAuto)
        {            
            arrMasterPTIdDeadball=["hidHdpD","hidOUD","hid1stHdpD","hid1stOUD","hidOE"];
        }
        else
        {
            arrMasterPTIdDeadball=["listMHdpD","listMOUD","listM1stHdpD","listM1stOUD","listMOE"];
            if(isEdit && roleId==3){
                for(var i=0;i<arrMasterPTIdDeadball.length;++i)
                {
                    if(obj(arrMasterPTIdDeadball[i].replace("listM","chkSP")).checked)
                        arrMasterPTIdDeadball[i]=arrMasterPTIdDeadball[i].replace("listM","listSP");
                }
            }
        }               
        arrPTIdDeadball=["listSHdpD","listSOUD","listS1stHdpD","listS1stOUD","listSOE"];
        arrHidDeadball=["hidMinHdpD","hidMinOUD","hidMin1stHdpD","hidMin1stOUD","hidMinOE"];                    
        if(!CheckValidMinPT(arrPTIdDeadball,arrMasterPTIdDeadball,arrHidDeadball))
        {
            return false;
        }
    }
    if(obj("chkCopy1x2").checked)  // when Copy From 1x2 checkbox is checked
    {
        if(isAuto)
        {            
            arrMasterPTId=["hid1x2"];  
        }
        else
        {
            var chkAuto=(roleId==3)?obj("chkSP1x2").checked:false;
            if(isEdit && chkAuto)
                arrMasterPTId=["listSP1x2"];  
            else
                arrMasterPTId=["listM1x2"];  
        }
        arrPTId=["listS1x2"];              
        arrHid=["hidMin1x2"];
        if(!CheckValidMinPT(arrPTId,arrMasterPTId,arrHid))
        {
            return false;
        }
    }
    else  // when Copy From 1x2 checkbox isn't checked
    {
        if(isAuto)
        {            
            arrMasterPTId=["hid1x2","hidCS","hidTG","hidMP","hidOutright"];
        }
        else
        {            
            arrMasterPTId=["listM1x2","listMCS","listMTG","listMMP","listMOutright"];
            if(isEdit && roleId==3)
            {
                for(var i=0;i<arrMasterPTId.length;++i)
                {
                    if(obj(arrMasterPTId[i].replace("listM","chkSP")).checked)
                        arrMasterPTId[i]=arrMasterPTId[i].replace("listM","listSP");
                }
            }
        }
        arrPTId=["listS1x2","listSCS","listSTG","listSMP","listSOutright"];
        arrHid=["hidMin1x2","hidMinCS","hidMinTG","hidMinMP","hidMinOutright"];
        if(!CheckValidMinPT(arrPTId,arrMasterPTId,arrHid))
        {
            return false;
        }
    }
    if(obj("chkCopyHdpL").checked)  // when Copy from Handicap checkbox of Live is checked
    {
        if(isAuto)
        {            
            arrMasterPTIdLive=["hidHdpL"];
        }
        else
        {   
            var chkAuto=(roleId==3)?obj("chkSPHdpL").checked:false;         
            if(iEdit && chkAuto)
                arrMasterPTIdLive=["listSPHdpL"];
            else
                arrMasterPTIdLive=["listMHdpL"];
        }
        arrPTIdLive=["listSHdpL"];
        arrHidLive=["hidMinHdpL"];
        if(!CheckValidMinPT(arrPTIdLive,arrMasterPTIdLive,arrHidLive))
        {
            return false;
        }
    }
    else  // when Copy from Handicap checkbox of Live isn't checked
    {
        if(isAuto)
        {            
            arrMasterPTIdLive=["hidHdpL","hidOUL","hid1stHdpL","hid1stOUL"];
        }
        else
        {
            arrMasterPTIdLive=["listMHdpL","listMOUL","listM1stHdpL","listM1stOUL"];
            if(isEdit && roleId==3)
            {
                for(var i=0;i<arrMasterPTIdLive.length;++i)
                {
                    if(obj(arrMasterPTIdLive[i].replace("listM","chkSP")).checked)
                        arrMasterPTIdLive[i]=arrMasterPTIdLive[i].replace("listM","listSP");
                }
            }
        }
        arrPTIdLive=["listSHdpL","listSOUL","listS1stHdpL","listS1stOUL"];
        arrHidLive=["hidMinHdpL","hidMinOUL","hidMin1stHdpL","hidMin1stOUL"];
        if(!CheckValidMinPT(arrPTIdLive,arrMasterPTIdLive,arrHidLive))
        {
            return false;
        }
    }
    if(obj("chkCopyHdpB").checked)  // when Copy from Handicap checkbox of Basketball is checked
    {
        if(isAuto)
        {            
            arrMasterPTIdBs=["hidHdpB"];
        }
        else
        {     
            var chkAuto=(roleId==3)?obj("chkSPHdpB").checked:false;         
            if(iEdit && chkAuto)
                arrMasterPTIdBs=["listSPHdpB"];
            else
                arrMasterPTIdBs=["listMHdpB"];
        }
        arrPTIdBs=["listSHdpB"];
        arrHidBs=["hidMinHdpB"];
        if(!CheckValidMinPT(arrPTIdBs,arrMasterPTIdBs,arrHidBs))
        {
            return false;
        }
    }
    else   // when Copy from Handicap checkbox of Basketball isn't checked
    {
        if(isAuto)
        {            
            arrMasterPTIdBs=["hidHdpB","hidOUB","hidOEB"];
        }
        else
        {
            arrMasterPTIdBs=["listMHdpB","listMOUB","listMOEB"];
            if(isEdit && roleId==3)
            {
                for(var i=0;i<arrMasterPTIdBs.length;++i)
                {
                    if(obj(arrMasterPTIdBs[i].replace("listM","chkSP")).checked)
                        arrMasterPTIdBs[i]=arrMasterPTIdBs[i].replace("listM","listSP");
                }
            }
        }
        arrPTIdBs=["listSHdpB","listSOUB","listSOEB"];        
        arrHidBs=["hidMinHdpB","hidMinOUB","hidMinOEB"];
        if(!CheckValidMinPT(arrPTIdBs,arrMasterPTIdBs,arrHidBs))
        {
            return false;
        }
    }
    return true;            
}
//this function to check value min Position
//arrMasterId: id of dropdown position taking
//arrAgentId :id of dropdown position taking
//arrMinId : id of Hidden fields to save min value
function CheckValidMinPT(arrMasterId,arrAgentId,arrMinId)   //************************* not used now
{
    var len=arrMasterId.length;
    for(var i=0;i<len;i++)
    {
        var selectedMaster=parseFloat(obj(arrMasterId[i]).value);
        var selectedAgent=parseFloat(obj(arrAgentId[i]).value);
        var selTotal=round(selectedMaster+selectedAgent);
        var minValue=round(obj(arrMinId[i]).value);
        if(selTotal<minValue)
        {
            alert_error(strInvalidMin+minValue,'errMsg');
            obj(arrAgentId[i]).focus();
            return false;
        }              
    }
    return true; 
}
//this function is used to get max value 
//currentMaster : current value is selected 
//currentMaster: max value in dropdown
//maxAgent : max value is saved in hidden fields
function GetMaxValue(currentMaster,maxMaster,maxAgent)  //***************** not used in new pages of PT
{
    var max=0;           
    var max_Master=round(maxMaster)-round(currentMaster);
    var maxA=round(maxAgent);	   	    
    if(max_Master>maxA)
    {
        max=maxA;
    }
    else
    {
        max=max_Master;
    }	   
    return max;		        
} 

//this function to change value of Position taking dropdown when value of Preset Position Taking dropdown is changed

function ChangeValueAgentPT(ddlAgent,currentMaster,maxMaster,maxAgent,minAgent)//***************** not used in new pages of PT
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
/////////////////////***********************************////////////////////////////////////////

function urlImg()
{
    var url = '../../App_Themes/Default/Images/';
    return url;
}

function scrollTop()
{
    scroll(0,0);
}

//this function is used to Edit Max Exposure for Super, Master, Agent.
function OpenEditMaxEx(id,uname,exval,type)
{
    var url = "Customer/EditMaxExposure.aspx?custId=" + id + "&uname=" + uname + "&exval=" + exval + "&type=" + type;
    var fnNameReload = '';
    var pageIndex = 1;
    if (location.href.indexOf("#") > -1) {
        pageIndex = location.href.split("#")[1];
    }
    switch (type) {
        case 'Super':
            fnNameReload = 'GenSuperListContentByPager(\'' + pageIndex + '\');';
            break;
        case 'Master':
            fnNameReload = 'GenMasterListContentByPager(\'' + pageIndex + '\');';
            break;
        case 'Agent':
            fnNameReload = 'GenAgentListContentByPager(\'' + pageIndex + '\');';
            break;
        case 'Member':
            fnNameReload = 'GenMemberListContentByPager(\'' + pageIndex + '\');';
            break;
    }
    showPopup('Edit Max Exposure', url, 0, 0, 270, 150, false, 2000, function() { eval(fnNameReload) });  
}
function OpenEditVirtualExRate(id,uname,type)
{
    var url = "Customer/EditVirtualExRate.aspx?custId=" + id + "&uname=" + uname + "&type=" + type;
    var fnNameReload = '';
    var pageIndex = 1;
    if (location.href.indexOf("#") > -1) {
        pageIndex = location.href.split("#")[1];
    }

    switch(type)
    {
        case 'Super':
            fnNameReload = 'GenSuperListContentByPager(\'' + pageIndex + '\');';
            break;
        case 'Master':
            fnNameReload = 'GenMasterListContentByPager(\'' + pageIndex + '\');';
            break;
        case 'Agent':
            fnNameReload = 'GenAgentListContentByPager(\'' + pageIndex + '\');';
            break;
        case 'Member':
            fnNameReload = 'GenMemberListContentByPager(\'' + pageIndex + '\');';
            break;
    }
  
    showPopup('Edit VirtualRate', url, 0, 0, 300, 180, false, 2000, function() { eval(fnNameReload) });
   
}

//this function is used to Edit Information for Customer
function OpenEditCustInfo(id,type)
{
   var url="Customer/EditCustomerInfo.aspx?custId="+id + "&type="+type;
   var pageIndex="1";
   if (location.href.indexOf("#")>-1)
   {
    pageIndex=location.href.split("#")[1];
   }
   showPopup("Edit Customer", url, "", "" , 600, 360, false, 2000, function (){ callAjax(getParams(pageIndex));});
}

//this function is used to Min Max Position Taking
function OpenEditMinMaxPT(id,type)
{    
    var url="Customer/v2/EditMinMaxPT.aspx?custId="+id + "&type="+type; 
    showPopup("Edit Min/max PT", url ,"","",970,700, false, 2000, null)
}

//open edit Horseracing window
function OpenEditHR(id, roleid) {

    var type = "";
    switch (roleid) {
        case 4:
            type = "SuperUpdate";
            break;
        case 3:
            type = "MasterUpdate";
            break;
        case 2:
            type = "AgentUpdate";
            break;
        default:
            type = "MemberUpdate";
            break;
    }
    var url = "Customer/pt/racing/" + type + ".aspx?custId=" + id;
    showPopup("Update Racing Setting", url, "", "", 950, 700, false, 2000, null);
}

//open edit Casino window
function OpenEditCS(id) {
    var url = "Customer/PositionTakingCS.aspx?set=2&custId=" + id;
    //showPopup("Update Casino Setting", url, "", "" , 900, 520);
    showPopup("Update Casino Setting", url ,"","",900,520, false, 2000, null);
}

//open edit Finance window
function OpenEditFI(id, uplineid, roleid) {
    uplineid = (uplineid == null || uplineid == "") ? "0" : uplineid;
    roleid = (roleid == null || roleid == "") ? "4" : roleid;
    uplineid = uplineid.replace(",", "");
    var url = "Customer/v2/UpdateFinancePT.aspx?type=1&custid=" + id + "&uplineid=" + uplineid + "&roleid=" + roleid;
    showPopup("Update Finance Setting", url ,"","",760,520, false, 2000, null);
}


//open edit Bingo window
function OpenEditBI(id, username, roleid) {
    var url="Customer/PT/Bingo/";
    if (roleid==1) url+="MemberUpdate.aspx";
    else if (roleid==2) url+="AgentUpdate.aspx";
    else if (roleid == 3) url += "MasterUpdate.aspx";
    else url += "SuperUpdate.aspx";
    url += "?username=" + username + "&custId=" + id;
     showPopup("Update Bingo Setting", url ,"","",900,520, false, 2000, null);
}

function PopUpWindow(url, width, height) {
    var top = (screen.height - height) / 2;
    var left = (screen.width - width) / 2;
    var newwindow = window.open(url, "EditSuper", "height=" + height + ",width=" + width + ",top=" + top + ",left=" + left + ",scrollbars=1");
    if (newwindow) newwindow.focus();
    //fadf
}
//this function is used to edit password in Operator, Super Agent, Master Agent, Agent List.
function OpenChangePwd(id,uname,type) {

    var url="../ChangePassword/ChangePassword.aspx?custId="+id + "&uname="+uname+"&type=" + type;
    //showPopup("Change Password", url, 0, 0, 360, 180, false, 2000, null);
    PopUpWindow(url, 360, 180);

}

//used for add/edit master check min master + super
//not used temporarily
function On_ChangeSuper(superListObj) //--------------- is used in Customer/v2/AddNewSuper.aspx
{
    return;
    if(obj("roleId").value!="3")
        return
        
    var masVal=superListObj.value;
    var minMS=obj(superListObj.id.replace("listS","hidMinMS")).value;
    var superList1=obj(superListObj.id.replace("listS","listM"));
    var super1Val=superList1.value;
    var maxSuper1=superList1.options[0].value;
    var minSuper=parseFloat(minMS)-parseFloat(masVal);
    var minSuper1=parseFloat(superList1.options[superList1.options.length-1].value);
    //alert(minSuper1+"-"+minMS+"-"+masVal);
    if (minSuper1 != minSuper)
    {
        if(minSuper < 0)
            minSuper=0;
        FillPtDll(superList1,maxSuper1,minSuper,super1Val,true);
    }
    var isEdit=(obj("custId") !=null && obj("custId").value>0);
    if(isEdit){
        var superList2=obj(superListObj.id.replace("listS","listSP"));
        var maxSuper2=superList2.options[0].value;
        var super2Val=superList2.value;
        var minSuper2=parseFloat(superList2.options[superList2.options.length-1].value);
        if (minSuper != minSuper2)
        {
            FillPtDll(superList2,maxSuper2,minSuper,super2Val,true);
        }
    }
}

function FillPtDll(dllObj,maxVal,minVal,selVal,lastPrefer)  //--------------- is used in On_ChangeSuper() function above:
{
    //alert(maxVal+"-"+minVal+"-"+selVal);
    
    if(maxVal<minVal){
        dllObj.options.length=1;
        return;
    }
    dllObj.options.length=0;
    var maxVal2=parseInt(maxVal*100);
    minVal=parseInt(minVal*100);
    selVal=parseFloat(selVal*100);
    var selIndex=0;
    while(maxVal2>=minVal){
        if(selVal < maxVal2 && selVal > minVal)
            selIndex++;
        var val=parseFloat(maxVal2/100);
        var opt=new Option(val,val);
        dllObj.options[dllObj.options.length]=opt;
        maxVal2--;
    }
    if(lastPrefer && selVal<minVal)
        selIndex=dllObj.options.length-1;
    dllObj.selectedIndex=selIndex;
}

//this function is used to visible or invisible sporttype area when menu sporttype is click
//panelId: id of <tr> tag that contains sporttype info
function OpenPanelEdit(panelId)  // **************** is defined in js/agent/Customer.js
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
        //alert(getElementsByClassName("md2", obj(panelId)).innerText);	
        try{
            $("#" + panelId + " md2").removeClass("md2").addClass("md3");
        }catch(Error){};
}

function createCheck(id){   //--------------------- is used in checkpass() function belove
	t = document.createElement("img");
	t.setAttribute("id", id);
	t.width = 18;t.height = 18;t.border=0;
	t.src = urlImg() + "x5.gif";
	return t;
}

function checkpass(o)   //--------------------- is used to check password now
{	
	var isValid=false;
	var pass = o.value;				
	var t = obj("check_pass");
	var td = obj("td_pass");	
	if (pass.length < 6)
	{					
		if (!t){
			t = createCheck("check_pass");
			td.appendChild(t);						
		}
		else{
			t.src = urlImg() +"x5.gif";						
		}
		return false;
	}
	else 
	{
		isValid=check(pass);
		//Hanh commented to not display image when check password.		
		if (isValid) 
		{	
			if (!t)
			{
				t = createCheck("check_pass");
				td.appendChild(t);						
			}
			//else{t.src = "../Image/x5.gif";}			
			else{t.src = urlImg() + "t1.gif";}			
		}		
	}
	return isValid;
}

function check(str){		//--------------------- is used in checkpass() function above		
	numbers = new String("0123456789");	
	var i = 0;
	var nCount = 0;
	while(i < str.length){								
		(numbers.indexOf(str.substring(i,i+1)) >= 0) ? nCount ++ : false;					
		i++;
	}					
	return ((nCount > (str.length - 2)) || nCount < 2) ? false : true;				
}

//this function is used to check Password and Max Credit 
//in Editing case just check Max Credit
function CheckInfoValue(isEdit)  //--------------------- this function is overrided in js/agent/Customer.js
{ 
	var isMult=(obj("isMult")!=null?obj("isMult").value:"");	
	if (isMult=="1") return true; 	
	if(!isEdit)
	{	
	    if(isEmpty("txtUsername"))
		{
		    showAlertMessage("err", alertUserName,1);
			obj("txtUsername").focus();
			return false;
		}	
	
		if(isEmpty("txtPwd"))
		{
			ShowError(alertPasswordEmpty,"err");
			obj("txtPwd").focus();
			return false;
		}

		if (obj("listCurrency")) {
		    
		    if (isEmpty("listCurrency")) {
		        ShowError(alertCurrency, "err");
		        obj("listCurrency").focus();
		        return false;
		    }
		}

		var site = $("#listSite");		
        if (site) {

            if (site.val() == "" || site.val() == "Select") {

		        ShowError(alertSite, "err");
		        obj("listSite").focus();

		        return false;
		    }
		}
					
		/*if(!checkpass(obj("txtPwd")))
		{						
			ShowError(lblPasswordRequire,"err");
			obj("txtPwd").focus();
			return false;
		}*/
    }
    
    if ($('#hisexternal').val() != '1') {
        if (isEmpty("txtCredit") || isString("txtCredit")) {
            showAlertMessage("err", alertIncorrectCredit, 1);
            obj("txtCredit").focus();
            return false;
        }
    }
    var valMin = isNumericSpan("txtCreditMin") ? isNumericSpan("txtCreditMin") : 0;
    
	if(obj('roleId')!=null && obj('roleId').value=='4') // for super
	{
	    if ($('#hisexternal').val() != '1') {
	        if (isNumeric("txtCredit") < isNumericSpan("txtCreditMin")) {
	            showAlertMessage("err", alertMinLessCredit + ' ' + valMin, 1);
	            obj("txtMaxCredit").focus();
	        }
	    }
	}
	else // for member
	{
	    if((isNumeric("txtCredit")<isNumericSpan("txtCreditMin")) || (isNumeric("txtCredit")>isNumericSpan("txtCreditMax")))
	    {		
		    var str=alertCreditInvalid.replace("{1}",valMin);
		    str=str.replace("{2}", formatNumber2(isNumericSpan("txtCreditMax")));
		    ShowError(str,"err");
		    obj("txtCredit").focus()
		    return false;
	    }
	}
	return true;				
}

//this function to check value input of min bet, max bet, max per match
//min bet <=maxbet <= max per match
//sporttypeId: is of sporttype
var roleId = 0;
function CheckLimitMinMaxBet(sporttypeId)	//****************** same in js/agent/Customer.js		
{
    try {        
		var minbetId="txtMinbet" + sporttypeId;
		var maxbetId="txtMaxbet"+sporttypeId;
		var maxpermatchId="txtMaxPerMatch" + sporttypeId;	
		var lmMinbetId="txtMinbet_Min"+sporttypeId;
		var lmMaxbetId="txtMaxbet_Max"+sporttypeId;
		var maxpermatchMinId="txtMaxPerMatch_Min"+sporttypeId;
		var maxpermatchMaxId="txtMaxPerMatch_Max"+sporttypeId;			
		if(isEmpty(minbetId) || isString(minbetId))
		{
		    showAlertMessage("error" + sporttypeId, alertMinbet,1 )
			obj(minbetId).focus();
			return false;						
		}

        if (roleId == 1 && isNumeric(minbetId) >=0 && isNumeric(maxbetId) >=0) {
            if (isNumeric(minbetId) == 0 && isNumeric(maxbetId) == 0 && isNumeric(maxpermatchId) == 0)
            {return true; }
            else {
                var alertMsg = "";
                if (isNumeric(minbetId) < isNumericSpan(lmMinbetId))
                {
                    try {                        
                        alertMsg = alertminbetC + " " + isNumericSpan(lmMinbetId);
                    }
                    catch (ex) {
                    }
                    showAlertMessage("error" + sporttypeId, alertMsg, 1);
                    obj(minbetId).focus();
                    return false;
                }
                if (isNumeric(maxbetId) < isNumeric(minbetId)) {
                    try {

                        alertMsg = alertCompare3;
                    }
                    catch (ex) {
                    }
                    showAlertMessage("error" + sporttypeId, alertMsg, 1);
                    obj(maxbetId).focus();
                    return false;
                } 
                if (isNumeric(maxbetId) == 0) {
                    try {
                        
                        alertMsg =  alertMaxbetC + " " + isNumericSpan(lmMaxbetId);
                    }
                    catch (ex) {
                    }
                    showAlertMessage("error" + sporttypeId, alertMsg, 1);
                    obj(maxbetId).focus();
                    return false;
                } 
            }
        }
		//phuong.nguyen
		//Check if input value (min, max, max per match) are negative
		//start
		if(isNumeric(minbetId) < 0)
		{
		    var alertMsg = "";
		    try
		    {
		        var alertMsg = alertMinbetNegative;
		    }
		    catch(ex)
		    {
		    //Min Bet must be larger than zero
		        alertMsg = "Min Bet must be greater than zero";
		    }

		    showAlertMessage("error" + sporttypeId ,alertMsg, 1);
			obj(minbetId).focus();
			return false;
		}
		
		if(isNumeric(maxbetId) < 0)
		{
		    var alertMsg = "";
		    try
		    {
		        var alertMsg = alertMaxbetNegative;
		    }
		    catch(ex)
		    {
		        alertMsg = "Max Bet must be greater than zero";
		    }
		    showAlertMessage("error" + sporttypeId, alertMsg, 1);
			obj(maxbetId).focus();
			return false;

        }

        
		if(isNumeric(maxpermatchId) < 0)
		{
		    var alertMsg = "";
		    try
		    {
		        var alertMsg = alertMaxperMatchNegative;
		    }
		    catch(ex)
		    {
		        alertMsg = "Max Per Match must be greater than zero";
		    }
		    showAlertMessage("error" + sporttypeId, alertMsg, 1);
			obj(maxpermatchId).focus();
			return false;
		}
		//end	
					
		if(isNumeric(minbetId)!=0 && isNumericSpan(lmMinbetId)!=0 && isNumeric(minbetId)<isNumericSpan(lmMinbetId))
		{
		    showAlertMessage("error" + sporttypeId, alertminbetC + " " + isNumericSpan(lmMinbetId), 1);			
			obj(minbetId).focus();
			return false;
		}					
		
		if(isEmpty(maxbetId) || isString(maxbetId))
		{
		    showAlertMessage("error" + sporttypeId, alertMaxbet, 1);
			obj(maxbetId).focus();
			return false;						
		}
		if(isNumeric(maxbetId)!=0 && isNumericSpan(lmMaxbetId)!=0 && isNumeric(maxbetId)>isNumericSpan(lmMaxbetId))
		{
		    showAlertMessage("error" + sporttypeId, alertMaxbetC + " " + isNumericSpan(lmMaxbetId), 1);
			obj(maxbetId).focus();
			return false;
		}
		
		if(isNumeric(minbetId)>isNumeric(maxbetId))
		{
		    showAlertMessage("error" + sporttypeId, alertCompare3, 1);
			obj(minbetId).focus();
			return false;
		}
							
		if(isEmpty(maxpermatchId) || isString(maxpermatchId))
		{
		    showAlertMessage( "error" + sporttypeId, alertMaxpermatch,1);
			obj(maxpermatchId).focus();
			return false;						
		}				
		if(isNumeric(maxbetId)>isNumeric(maxpermatchId))
		{
		    showAlertMessage("error" + sporttypeId, alertCompare, 1 );
			obj(maxbetId).focus();
			return false;
		}		
		if(isNumeric(maxpermatchId)!=0 && isNumericSpan(maxpermatchMaxId)!=0)
		{	        
		    if( isNumeric(maxpermatchId) > isNumericSpan(maxpermatchMaxId) )
		    {
			    var str="";
			    str=alertMaxpermatchC.replace("{1}",isNumericSpan(maxpermatchMaxId));
			    showAlertMessage("error" + sporttypeId, str, 1);
			    obj(maxpermatchId).focus();
			    return false;
		    }					
		}					
	}
	catch(ex)
	{		
		return false;
	}
	return true;
}

//agent --> acc
function updateCommission(form) //---------------- is used in v2/AddNewMember.aspx
{
	
	var ug = document.getElementById('ddlUGruop');

	if ((ug.options[ug.selectedIndex].text == "a") || (ug.options[ug.selectedIndex].text == "hka") || (ug.options[ug.selectedIndex].text == "indoa") || (ug.options[ug.selectedIndex].text == "dea"))
	{
		form.txtDiscount.value = valGroupa;		
		setValueCommission(form, valGroupa);
		changeCommHDP(parseFloat(valGroupa));
		
	}
if ((ug.options[ug.selectedIndex].text == "b") || (ug.options[ug.selectedIndex].text == "hkb") || (ug.options[ug.selectedIndex].text == "indob") || (ug.options[ug.selectedIndex].text == "deb"))
	{
        form.txtDiscount.value = valGroupb;
        setValueCommission(form, valGroupb);
        changeCommHDP(parseFloat(valGroupb));
    }
    if ((ug.options[ug.selectedIndex].text == "c") || (ug.options[ug.selectedIndex].text == "hkc") || (ug.options[ug.selectedIndex].text == "indoc") || (ug.options[ug.selectedIndex].text == "dec"))
	{
		form.txtDiscount.value = valGroupc;
		setValueCommission(form, valGroupc);
		changeCommHDP(parseFloat(valGroupc));
	}
if ((ug.options[ug.selectedIndex].text == "d") || (ug.options[ug.selectedIndex].text == "hkd") || (ug.options[ug.selectedIndex].text == "indod") || (ug.options[ug.selectedIndex].text == "ded"))
	{
	    form.txtDiscount.value = valGroupd;
		changeCommHDP(parseFloat(valGroupd));
	}
}

//agent-->acc
function setValueCommission(form, vcom)    //---------------- is used in updateCommission() function above
{
	if(form.hidden_discount) form.hidden_discount.value = vcom;		
}

//agent -->acc
function changeCommHDP(value){		//---------------- is used in updateCommission() function above		
	var select = obj("ddrCommissionH_OU_OE");
	clearOptions(select);					
	
	value = value * 10000 ; 
	
	while (value > 0){		
		var format = value / 10000;		
		
		var op = new Option(format, format);						
		select.options[select.options.length] = op;	
		value = value - 5;					
	}
	if (value <= 0){
		var op = new Option("0", "0");						
		select.options[select.options.length] = op;	
	}
	//select.options.selectedIndex = select.options.length - 1;
	select.options.selectedIndex = 0;
}

//agent -->acc
function clearOptions(select){		//---------------- is used in changeCommHDP() function above		
	for (i = select.options.length - 1; i >=0 ; i--){
		select.options[i] = null;
	}	
}

function OpenEditTransferTime(custid, isAgent)
{
    var url = 'Customer/v2/EditTransferTime.aspx?custid=' + custid + '&rid=' + ((isAgent == 1) ? '2' : '');
    showPopup('Edit Transfer Condition', url, 0, 0, 430, 200, false, 2000, null);
}

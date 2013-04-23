function obj(id){
    return document.getElementById(id);
}
function showErrMsg(msg, type) {
    if (typeof type == 'undefined') type = 0;
    obj('diverrmsg').style.display = IE ? 'block' : 'table';
    obj('spmsgerr').innerHTML = msg;
    obj('spmsgerr').className = (type == 1) ? 'msgscc' : 'msgerr';
    var cname = (type == 1) ? "succ" : "emsg";
    obj('1_1').className = cname + "_1_1";
    obj('1_2').className = cname + "_1_2";
    obj('1_3').className = cname + "_1_3";
    obj('2_1').className = cname + "_2_1";
    obj('2_2').className = cname + "_2_2";
    obj('3_1').className = cname + "_3_1";
    obj('3_2').className = cname + "_3_2";
    obj('3_3').className = cname + "_3_3";

    ResetBtnState();
    HideLoadingDiv();
    window.scrollTo(0, 0);
    if (window.parent && window.parent != window) window.parent.scrollTo(0, 0);
}

function CancelCS(confirmMsg) {

    if(confirmMsg && parent.ibcWnd == null)
    {
        var bOK = confirm(confirmMsg);

        if(!bOK)
        {
            return;
        }
    }

    if (parent.ibcWnd)
        parent.ibcWnd.close();
    else {
        parent.location.href = 'DownlineList.aspx';
    }
}

function ResetMaxTransfer(id) {
    var arrayCustID = document.getElementById('arrayCustID');

    if(arrayCustID && arrayCustID.value != '') 
    {
        id = arrayCustID.value.replace('^', ',');
        while(id.indexOf('^') != -1)
        {
            id = id.replace('^', ',');
        }
        id = id.substr(0, id.length - 1);
    }

    $.ajax(
     {
         type: "POST",
         url: "../Handlers/ResetMaxTransfer.ashx",
         async: true,
         data: "custid=" + id,
         success: function(res) {
             if (res != 'Failed') {

                 document.getElementById('lblAccumulatedWL').innerHTML = document.getElementById('hidTransferCredit').value;
                 ShowMessage(res);
             }
             else {
                 ShowMessage(res);
             }
         }
     }
     );
} 

function CopyMultData()
{
    if(obj('bIsMult').value == '1')
    {
        obj('arrayCustID').value = parent.document.getElementById('arrayCustID').value;
        var strUserName = parent.document.getElementById('arrayUserName').value;
        obj('arrayStatus').value = parent.document.getElementById('arrayStatus').value;
        
        obj('arrayUserName').value = strUserName;
        
        var index = strUserName.indexOf('^');
        while(index != -1){
            strUserName = strUserName.replace('^',', ');
            index = strUserName.indexOf('^');
        }
        
        strUserName = strUserName.substr(0, strUserName.length - 2);
        
        obj('lstusername').innerHTML = strUserName;
    }
}

function InitCasinoSchema()
{
    // parse variable
    
    var hidCSData =  document.getElementById('hidCSData');
    if(hidCSData) hidCSData = hidCSData.value;
    else return;    
    var A = hidCSData.split(/;/g); // Split each game kind
    var c = A.length;
    for(var i=0; i<c; i++) {
        var ai = A[i];
        var i1 = ai.indexOf('[');
        if(i1 != -1) {
            var gameKind = new Object();
            gameKind.gameKindId = ai.substr(0, i1);
            gameKind.gameTypes = ai.substr(i1 + 1, ai.length - i1 - 2).split(/-/g); // Split each game type
            
            var A1 = gameKind.gameTypes;
            var c1 = A1.length;
            for(var j=0; j<c1; j++) {
                var a1j = A1[j];
                var j1 = a1j.indexOf('(');
                if(j1 != -1) {
                    var gameType = new Object();
                    gameType.gameTypeId = a1j.substr(0, j1);
                    gameType.bettingLimits = a1j.substr(j1 + 1, a1j.length - j1 - 2).split(/, /g); // Split each betting limit
                    
                    A1[j] = gameType;
                    a1j = null; // free memory
                    
                    // apply checking betting limit
                    //ApplyCheckingBettingLimit(gameKind.gameKindId + '_' + gameType.gameTypeId + '_ChkStake_', gameType.bettingLimits, '');
                }
                else {
                    A1[j] = null;
                }
            }
            
            A[i] = gameKind;
            ai = null; // free memory
        }
        else {
            A[i] = null;
        }
    }
    
    // Store schema
    parent._CasinoSchema = A;
}

function ApplyCheckingBettingLimit(prefix, arrId, subfix)
{
    if(typeof arrId != 'object') return;
    var c = arrId.length;
    for(var i=0; i<c; i++) {
        var el = document.getElementById(prefix + arrId[i] + subfix);
        
        if(!el) break;
        
        // Check for first checkbox and disable it
        if(i==0) {
            el.checked = true;
            el.className = 'chkDisabled';
            
            el.onclick = function() {
                this.checked = true;
            }
        }
        else {

            if(!el.parentNode.getAttribute('arrId')) {
                el.parentNode.setAttribute('arrId', arrId);
                el.parentNode.setAttribute('prefix', prefix);
                el.parentNode.setAttribute('subfix', subfix);
            }

            el.onclick = function() {
                arrId = this.parentNode.getAttribute('arrId');
                if(typeof arrId == 'string') arrId = arrId.split(/,/g);
                prefix = this.parentNode.getAttribute('prefix');
                subfix = this.parentNode.getAttribute('subfix');
                var c1 = arrId.length;
                var num1 = 0; var chk1;
                for(var j=0; j<c1; j++) {
                    var chk = document.getElementById(prefix + arrId[j] + subfix);
                    if(chk.checked) {chk1 = chk; num1++;}
                }
                if(num1 == 1) {
                    chk1.checked = true;
                }
                else if(num1 == 0) {
                    this.checked = true;
                    alert(bettingLimitMsg);
                }
            }
        }
    }
}

IBC.addEvent(window, 'load', InitCasinoSchema);

function numberOnly(ev)
{

    // 48-57, 96-105
    if((ev.keyCode < 48 || (ev.keyCode > 57 && ev.keyCode < 96) || ev.keyCode > 105) && ev.keyCode != 8 && ev.keyCode != 46 && ev.keyCode != 13){
        if (window.event) { // IE
		    window.event.cancelBubble = true;
		    window.event.returnValue = false;
	    } else {
		    ev.preventDefault();
		    ev.stopPropagation();
	    }
	    return false;
    }
    return true;
}
function IncreasePT(ddl, step) {
    var max = 0.9;
    if (obj(ddl) || obj(ddl) != 'undefined') {
        max = obj(ddl).options[0].value;
    }
    Increase(ddl, step, max);

    var f = ddl.substring(5, 4);
    if (f == 'S') On_ChangeSuper(ddl);
    else if (f == 'M') On_ChangeMaster(ddl);
}

function ReducePT(ddl, step) {
    var min = obj(ddl).options[obj(ddl).length - 1].value;
    Reduce(ddl, step, min);

    var f = ddl.substring(5, 4);
    if (f == 'S') On_ChangeSuper(ddl);
    else if (f == 'M') On_ChangeMaster(ddl);
}

//------------- v2 -----------------//
function CopyPositionTaking(chkCopy) {
    switch (chkCopy.id) {
        case 'chkCopyHdpD1': disable1DDL1(chkCopy); break;
        case 'chkCopy1x21': disable1DDL2(chkCopy); break;
        case 'chkCopyHdpL1': disable1DDL3(chkCopy); break;
        case 'chkCopyHdpB2': disable2DDL(chkCopy); break;
        case 'chkCopyHdpB3': disable3DDL(chkCopy); break;
        case 'chkCopyHdpB5': disable5DDL(chkCopy); break;
        case 'chkCopyHdpB8': disable8DDL(chkCopy); break;
        case 'chkCopyHdpB10': disable10DDL(chkCopy); break;
        case 'chkCopyHdpB11': disable11DDL(chkCopy); break;
        //                case 'chkCopyOEB12': disable12DDL(chkCopy); break;  
        case 'chkCopyHdpB99': disable99DDL(chkCopy); break;
        case 'chkCopyOU201': disable201DDL(chkCopy); break;
    }
}

function getObj(id) {
    return document.getElementById(id);
}

//this function was used copy PT from Handicap
//chkCopy: is checkbox id Copy all Bet Setting and Position Taking from Soccer Handicap or Copy all Bet Setting and Position Taking from Basketball Handicap
//arr: array ids of ddl PT want to disable
//chk: id's checkbox copy all from handicap
function CheckCopyPT(chkCopy, arr, chk) {

    var chkcopy = 0;
    if (chk) chkcopy = getObj(chk);
    for (var i = 0; i < arr.length; i++) {
        if (getObj(arr[i])) {
            getObj(arr[i]).disabled = (chkCopy.checked) ? true : ((chkcopy.checked) ? true : false);
        }
    }
}

var arr1S1 = ['listSOUD1', 'listS1stHdpD1', 'listS1stOUD1', 'listSOE1'];
var arr1M1 = ['listMOUD1', 'listM1stHdpD1', 'listM1stOUD1', 'listMOE1'];
var arr1A1 = ['listAOUD1', 'listA1stHdpD1', 'listA1stOUD1', 'listAOE1'];

var arr1S2 = ['listSCS1', 'listSTG1', 'listSMP1', 'listSOutright1'];
var arr1M2 = ['listMCS1', 'listMTG1', 'listMMP1', 'listMOutright1'];
var arr1A2 = ['listACS1', 'listATG1', 'listAMP1', 'listAOutright1'];

var arr1S3 = ['listSOUL1', 'listS1stHdpL1', 'listS1stOUL1'];
var arr1M3 = ['listMOUL1', 'listM1stHdpL1', 'listM1stOUL1'];
var arr1A3 = ['listAOUL1', 'listA1stHdpL1', 'listA1stOUL1'];

var arr2S = ['listSOUB2', 'listSOEB2', 'listSMPB2', 'listSMLB2'];
var arr2M = ['listMOUB2', 'listMOEB2', 'listMMPB2', 'listMMLB2'];
var arr2A = ['listAOUB2', 'listAOEB2', 'listAMPB2', 'listAMLB2'];

var arr3S = ['listSOUB3', 'listSOEB3', 'listSMLB3'];
var arr3M = ['listMOUB3', 'listMOEB3', 'listMMLB3'];
var arr3A = ['listAOUB3', 'listAOEB3', 'listAMLB3'];

var arr5S = ['listSOUB5', 'listSOEB5', 'listSMLB5'];
var arr5M = ['listMOUB5', 'listMOEB5', 'listMMLB5'];
var arr5A = ['listAOUB5', 'listAOEB5', 'listAMLB5'];

var arr8S = ['listSOUB8', 'listSMLB8'];
var arr8M = ['listMOUB8', 'listMMLB8'];
var arr8A = ['listAOUB8', 'listAMLB8'];

var arr10S = ['listSOUB10', 'listSOEB10', 'listSMLB10'];
var arr10M = ['listMOUB10', 'listMOEB10', 'listMMLB10'];
var arr10A = ['listAOUB10', 'listAOEB10', 'listAMLB10'];

var arr11S = ['listSMLB11'];
var arr11M = ['listMMLB11'];
var arr11A = ['listAMLB11'];

var arr201S = ['listSOUB201', 'listSOEB201'];
var arr201M = ['listMOUB201', 'listMOEB201'];
var arr201A = ['listAOUB201', 'listAOEB201'];

var arr201S2 = ['listSOEB201'];
var arr201M2 = ['listMOEB201'];
var arr201A2 = ['listAOEB201'];

var arr99S = ['listSOUB99', 'listSOEB99', 'listSMLB99'];
var arr99M = ['listMOUB99', 'listMOEB99', 'listMMLB99'];
var arr99A = ['listAOUB99', 'listAOEB99', 'listAMLB99'];

var arrCopyFromSoccer = ['listSHdpB2', 'listMHdpB2', 'listAHdpB2', 'listS1x21', 'listM1x21', 'listA1x21', 'listSHdpL1', 'listMHdpL1', 'listAHdpL1', 'txtMinbet2', 'txtMaxbet2', 'txtMaxPerMatch2', 'chkCopyHdpD1', 'chkCopy1x21', 'chkCopyHdpL1', 'chkCopyBasket'];
var arrCopyFromBasketball = ['listSHdpB3', 'listMHdpB3', 'listAHdpB3', 'listSHdpB5', 'listMHdpB5', 'listAHdpB5', 'listSHdpB8', 'listMHdpB8', 'listAHdpB8', 'listSHdpB10', 'listMHdpB10', 'listAHdpB10', 'listSHdpB11', 'listMHdpB11', 'listAHdpB11', 'listSHdpB99', 'listMHdpB99', 'listAHdpB99', 'txtMinbet3', 'txtMaxbet3', 'txtMaxPerMatch3', 'txtMinbet5', 'txtMaxbet5', 'txtMaxPerMatch5', 'txtMinbet8', 'txtMaxbet8', 'txtMaxPerMatch8', 'txtMinbet10', 'txtMaxbet10', 'txtMaxPerMatch10', 'txtMinbet11', 'txtMaxbet11', 'txtMaxPerMatch11', 'txtMinbet99', 'txtMaxbet99', 'txtMaxPerMatch99', 'chkCopyOU201', 'chkCopyHdpB2', 'chkCopyHdpB3', 'chkCopyHdpB5', 'chkCopyHdpB8', 'chkCopyHdpB10', 'chkCopyHdpB11', 'chkCopyHdpB99', 'listSOUB201', 'listMOUB201', 'listAOUB201', 'txtMinbet201', 'txtMaxbet201', 'txtMaxPerMatch201', 'txtMinbet151', 'txtMaxbet151', 'txtMaxPerMatch151'];
var arrChk = ['chkCopyAll'];

var arrHdpLiveId = new Array("listSOUL", "listS1stHdpL", "listS1stOUL", "listMOUL", "listM1stHdpL", "listM1stOUL", "listAOUL", "listA1stHdpL", "listA1stOUL");

function disable1DDL1(chkCopy, chkCopyHdp) {
    CheckCopyPT(chkCopy, arr1S1, chkCopyHdp);
    CheckCopyPT(chkCopy, arr1M1, chkCopyHdp);
    CheckCopyPT(chkCopy, arr1A1, chkCopyHdp);
}

function disable1DDL2(chkCopy, chkCopyHdp) {
    CheckCopyPT(chkCopy, arr1S2, chkCopyHdp);
    CheckCopyPT(chkCopy, arr1M2, chkCopyHdp);
    CheckCopyPT(chkCopy, arr1A2, chkCopyHdp);
}

function disable1DDL3(chkCopy, chkCopyHdp) {
    CheckCopyPT(chkCopy, arr1S3, chkCopyHdp);
    CheckCopyPT(chkCopy, arr1M3, chkCopyHdp);
    CheckCopyPT(chkCopy, arr1A3, chkCopyHdp);
}

function disable2DDL(chkCopy, chkCopyHdp) {
    CheckCopyPT(chkCopy, arr2S, chkCopyHdp);
    CheckCopyPT(chkCopy, arr2M, chkCopyHdp);
    CheckCopyPT(chkCopy, arr2A, chkCopyHdp);
}

function disable3DDL(chkCopy, chkCopyHdp) {
    CheckCopyPT(chkCopy, arr3S, chkCopyHdp);
    CheckCopyPT(chkCopy, arr3M, chkCopyHdp);
    CheckCopyPT(chkCopy, arr3A, chkCopyHdp);
}

function disable5DDL(chkCopy, chkCopyHdp) {
    CheckCopyPT(chkCopy, arr5S, chkCopyHdp);
    CheckCopyPT(chkCopy, arr5M, chkCopyHdp);
    CheckCopyPT(chkCopy, arr5A, chkCopyHdp);
}

function disable8DDL(chkCopy, chkCopyHdp) {
    CheckCopyPT(chkCopy, arr8S, chkCopyHdp);
    CheckCopyPT(chkCopy, arr8M, chkCopyHdp);
    CheckCopyPT(chkCopy, arr8A, chkCopyHdp);
}

function disable10DDL(chkCopy, chkCopyHdp) {
    CheckCopyPT(chkCopy, arr10S, chkCopyHdp);
    CheckCopyPT(chkCopy, arr10M, chkCopyHdp);
    CheckCopyPT(chkCopy, arr10A, chkCopyHdp);
}

function disable11DDL(chkCopy, chkCopyHdp) {
    CheckCopyPT(chkCopy, arr11S, chkCopyHdp);
    CheckCopyPT(chkCopy, arr11M, chkCopyHdp);
    CheckCopyPT(chkCopy, arr11A, chkCopyHdp);
}

function disable201DDL(chkCopy, chkCopyHdp) {

    CheckCopyPT(chkCopy, arr201S2, chkCopyHdp);
    CheckCopyPT(chkCopy, arr201M2, chkCopyHdp);
    CheckCopyPT(chkCopy, arr201A2, chkCopyHdp);
}

function disable99DDL(chkCopy, chkCopyHdp) {
    CheckCopyPT(chkCopy, arr99S, chkCopyHdp);
    CheckCopyPT(chkCopy, arr99M, chkCopyHdp);
    CheckCopyPT(chkCopy, arr99A, chkCopyHdp);
}

function CopyAllPT(chkCopy) {
    disable1DDL1(chkCopy, 'chkCopyHdpD1');
    disable1DDL2(chkCopy, 'chkCopy1x21');
    disable1DDL3(chkCopy, 'chkCopyHdpL1');
    disable2DDL(chkCopy, 'chkCopyHdpB2');
    disable3DDL(chkCopy, 'chkCopyHdpB3');
    disable5DDL(chkCopy, 'chkCopyHdpB5');
    disable8DDL(chkCopy, 'chkCopyHdpB8');
    disable10DDL(chkCopy, 'chkCopyHdpB10');
    disable11DDL(chkCopy, 'chkCopyHdpB11');
    disable201DDL(chkCopy, 'chkCopyOU201');
    disable99DDL(chkCopy, 'chkCopyHdpB99');
    CheckCopyPT(chkCopy, arrCopyFromSoccer);
    CheckCopyPT(chkCopy, arrCopyFromBasketball);
    //disable161DDL(chkCopy);
}

function CopyBasketPT(chkCopy) {
    disable2DDL(chkCopy, 'chkCopyHdpB2');
    disable3DDL(chkCopy, 'chkCopyHdpB3');
    disable5DDL(chkCopy, 'chkCopyHdpB5');
    disable8DDL(chkCopy, 'chkCopyHdpB8');
    disable10DDL(chkCopy, 'chkCopyHdpB10');
    disable11DDL(chkCopy, 'chkCopyHdpB11');
    disable201DDL(chkCopy, 'chkCopyOU201');
    disable99DDL(chkCopy, 'chkCopyHdpB99');
    CheckCopyPT(chkCopy, arrCopyFromBasketball);
    CheckCopyPT(chkCopy, arrChk);
    //disable161DDL(chkCopy);
}

function BtnOnClick() {
    ClearAllMsg();
    var isEdit = getObj('type').value;
    if (isEdit == 'edit') isEdit = true;
    else isEdit = false;
    if (!CheckInfoValue(isEdit)) return false;

    if (!CheckMaxPerBall161()) return false;
    
    if (!CheckLimitMinMaxBet(1)) return false;
    if (!obj("chkCopyAll").checked) {
        if (!CheckLimitMinMaxBet(2)) return false;
        if (!obj("chkCopyBasket").checked) {
            if (!CheckLimitMinMaxBet(3)) return false;
            if (!CheckLimitMinMaxBet(5)) return false;

            if (!CheckLimitMinMaxBet(8)) return false;
            if (!CheckLimitMinMaxBet(10)) return false;
            if (!CheckLimitMinMaxBet(11)) return false;

            if (getObj('txtMinbet161')) {                
                if (!CheckLimitMinMaxBet(161)) return false;
            }
                        
            if (!CheckLimitMinMaxBet('99_9')) return false;
            if (obj("txtMinbet201") || obj("txtMaxbet201") || obj("txtMaxPerMatch201")) {
                if (!CheckLimitMinMaxBet(201)) return false;
            }
            if (!CheckLimitMinMaxBet(99)) return false;
            if (isEdit == false) {
                if (obj("txtMinbet151") || obj("txtMaxbet151") || obj("txtMaxPerMatch151")) {
                    if (!CheckLimitMinMaxBet(151)) return false;
                }
            }
        }
    }
    PTBlockUI();
    document.fInsertNew.submit();
    return true;
}
function ShowErrorInsert(msg) {
    getObj('err').innerHTML = msg;
}

function On_ChangeSuper(chkS) {
    //reset value for master PT base on super PT
    var Sid = (chkS.id) ? chkS.id : chkS;

    var f = Sid.substring(0, 4);
    var l = Sid.substring(5, Sid.length);
    var Mid = f + "M" + l;

    var Sv = (chkS.value) ? chkS.value : getObj(chkS).value;
    var objM = getObj(Mid);
    objM.options.length = 0;
    var maxM = parseFloat(Sv * 100).toFixed(0);
    while (parseFloat(maxM).toFixed(0) >= 0) {
        var val = parseFloat(maxM / 100);
        var opt = new Option(val, val);
        objM.options[objM.options.length] = opt;
        maxM--;
    }
    //reset value for agent PT base on super PT
    var Aid = f + "A" + l;
    var objA = getObj(Aid);
    objA.options.length = 0;
    var maxA = parseFloat(Sv * 100).toFixed(0);

    while (parseFloat(maxA).toFixed(0) >= 0) {
        var val = parseFloat(maxA / 100);
        var opt = new Option(val, val);
        objA.options[objA.options.length] = opt;
        maxA--;
    }
}
function On_ChangeMaster(chkM) {
    //reset value for agent PT base on master PT
    var Mid = (chkM.id) ? chkM.id : chkM;
    var f = Mid.substring(0, 4);
    var l = Mid.substring(5, Mid.length);
    var Aid = f + "A" + l;

    var Mv = (chkM.value) ? chkM.value : getObj(chkM).value;
    var objA = getObj(Aid);
    objA.options.length = 0;

    var maxA = parseFloat(Mv * 100).toFixed(0);
    while (parseFloat(maxA).toFixed(0) >= 0) {
        var val = parseFloat(maxA / 100);
        var opt = new Option(val, val);
        objA.options[objA.options.length] = opt;
        maxA--; 
    }
}

function SetFirstLoadCurr() {
    if ($('#hdCurr').val() == '0') {
        $('#hdCurr').val($('#listCurrency').val().split('_')[0]);
        $('#hdAllowCurr').val($('#listCurrency').val().split('_')[1]);
    }
}

function obj(id) {
    return document.getElementById(id);
}

function showMP(refno, listType) {
    var spEvent = obj('spEvent_' + refno);
    if (spEvent.className == 'hide') {
        if (spEvent.innerHTML == "") {
            BlockUI();
            $.ajax({
                url: '../Handlers/GenMixParlay.ashx?refno=' + refno + '&listType=' + listType + '&type=1',
                dataType: 'html',
                success: function(data) {
                    spEvent.innerHTML = data;
                    spEvent.className = '';
                    $.unblockUI();
                }
                , error: function() {
                    $.unblockUI();
                }
            });

        }
        else {
            spEvent.className = '';
        }
    }
    else {
        spEvent.className = 'hide';
    }
}

function showCMP(event, refno, listType) {
    var spEvent = obj('spEvent_' + refno);
    var target = event.target ? event.target : event.srcElement;
    $(target).parents("td:first").find("div").each(function(index, element) {
        if ($(element).attr("id").indexOf("spEvent") >= 0) {
            spEvent = element;
        }
    });
    if (spEvent.className == 'hide') {
        if (spEvent.innerHTML == "") {
            BlockUI();
            $.ajax({
                url: '../Handlers/GenMixParlay.ashx?refno=' + refno + '&listType=' + listType + '&type=1',
                dataType: 'html',
                success: function(data) {
                    spEvent.innerHTML = data;
                    spEvent.className = '';

                    $(event.target).parents("tr:first").find("td").each(function(index, element) {
                        element.style.verticalAlign = "top";
                    });

                    $.unblockUI();
                }
                , error: function() {
                    $.unblockUI();
                }
            });

        }
        else {
            spEvent.className = '';
            $(event.target).parents("tr:first").find("td").each(function(index, element) {
                element.style.verticalAlign = "top";
            });
        }
    }
    else {
        spEvent.className = 'hide';
        $(event.target).parents("tr:first").find("td").each(function(index, element) {
            element.style.verticalAlign = "middle";
        });
    }
}

function showDetailsCMP(refno, title, transid, betid) {
    //OpenPopup(url, title, width, height, modal, resizable, currWindow, duration, fnRecall, fnOnClose) {
    OpenPopup("../BetList/CombinationMPDetail.aspx?refno=" + refno + "&transid=" + transid + "&betid=" + betid, title, 720, 500, true, false, window, 0, null, null);
}

function cmpMouseOutDetailButton(event) {
    var target = event.target ? event.target : event.srcElement;
    $(target).parents("table:first").find("div").get(0).style.backgroundPosition = "0px 0px";
}

function cmpMouseOverDetailButton(event) {
    var target = event.target ? event.target : event.srcElement;
    $(target).parents("table:first").find("div").get(0).style.backgroundPosition = "0px -14px";
}

function confirm_delete(betid, betfrom, matchid, bettype) {
    var NewWindow = window.open('DeleteBet.aspx?betid=' + betid + '&betfrom=' + betfrom + '&matchid=' + matchid + '&bettype=' + bettype, 'DeleteBet', 'toolbar=no,location=no,status=no,menubar=no,resizable=yes,scrollbars=yes,width=720,height=450,left=400');
    if (NewWindow) NewWindow.focus();
}
function OpenByIP(custid, fdate, ip, type) {
    //window.location= 'BetList.aspx?custid='+custid+'&fdate='+fdate+'&ip='+ip+'&bltype=3&type='+type;
    try { parent.BlockUI(); } catch (e) { }
    var urlLink = '../Handlers/GenBetList.ashx?custid=' + custid + '&requesttype=4' + '&fdate=' + fdate + '&ip=' + ip + '&type=' + type;
    GeneralAjaxCall(urlLink);
}

function ViewMixParlayResult(refno) {
    var width = 750;
    OpenPopup('../Report/ViewMixParlayResult.aspx?refno=' + refno, 'Mix Parlay', width, 500, true, false, window, 0, null, null);

//    if (parent.ibcWnd) {
//        parent.ibcWnd.open('Report/ViewMixParlayResult.aspx?refno=' + refno, 'Mix Parlay', 100, 100, width, 400, 'class1', 'class2')        
//    }
//    else {
//        ibcWnd.open('../Report/ViewMixParlayResult.aspx?refno=' + refno, 'Mix Parlay', 100, 100, width, 400, 'class1', 'class2')
//    }
}

function ViewCombinationMixParlayResult(refno) {
    var width = 700;
    OpenPopup('../Report/ViewMixParlayResult.aspx?refno=' + refno, 'System Parlay', width, 500, true, false, window, 0, null, null);

//    if (parent.ibcWnd) {
//        parent.ibcWnd.open('Report/ViewMixParlayResult.aspx?refno=' + refno, 'System Parlay', 100, 100, width, 400, 'class1', 'class2')        
//    }
//    else {
//        ibcWnd.open('../Report/ViewMixParlayResult.aspx?refno=' + refno, 'System Parlay', 100, 100, width, 400, 'class1', 'class2')
//    }
}

function ViewResult(matchid, bettype) {
    var width = 750;

    OpenPopup('../Report/ViewResult.aspx?matchid=' + matchid + (bettype ? '&bettype=' + bettype : ''), '', width, 500, true, false, window, 0, null, null);
//    
//    if (parent.ibcWnd) {
//        parent.ibcWnd.open('Report/ViewResult.aspx?matchid=' + matchid + (bettype ? '&bettype=' + bettype : ''), '', 50, 100, width, 400, 'class1', 'class2');
//    }
//    else
//        ibcWnd.open(url, '', 50, 100, width, 400, 'class1', 'class2');
    
}
function ViewResultHR(url) {
       
    OpenPopup(url, '', 300, 100, true, false, window, 0, null, null);
    //    if (parent.ibcWnd)
    //        parent.ibcWnd.open(url, '', 300, 100, 300, 250, 'class1', 'class2');
    //    else
    //        ibcWnd.open(url, '', 300, 100, 300, 250, 'class1', 'class2');
}
function ViewResultNumberGame(url) {
    //ibcWnd.open(url, '', 100, 100, 710, 200, 'class1', 'class2');

    OpenPopup(url, '', 750, 200, true, false, window, 0, null, null);
}
function ViewResultLiveCasino(url) {
    //ibcWnd.open(url, '', 100, 100, 740, 200, 'class1', 'class2');
    OpenPopup(url, '', 760, 200, true, false, window, 0, null, null);
}
function viewFinanceResult(transid) {
    var width = 420;
    if (parent.ibcWnd)
        parent.ibcWnd.open('Report/ViewFinanceResult.aspx?transid=' + transid, '', 50, 100, width, 200, 'class1', 'class2');
    else
        ibcWnd.open('../Report/ViewFinanceResult.aspx?transid=' + transid, '', 50, 100, width, 200, 'class1', 'class2');
}
function ViewMainReportByBettype(fdate, tdate, bettype) {
    var urlLink = '../Handlers/GenBetList.ashx?fdate=' + fdate + '&tdate=' + tdate + '&bettype=' + bettype + '&requesttype=1';
    GeneralAjaxCall(urlLink);
}
function ViewMainReportByBettype2(fdate, tdate, bettype, page) {
    var urlLink = '../Handlers/GenBetList.ashx?fdate=' + fdate + '&tdate=' + tdate + '&bettype=' + bettype + '&requesttype=1' + '&page=' + page;
    GeneralAjaxCall(urlLink);
}
function ViewMainReportByTransID(transid) {
    var urlLink = '../Handlers/GenBetList.ashx?transid=' + transid + '&requesttype=2';
    GeneralAjaxCall(urlLink);
}
function ViewMainReportByTransID(transid, page) {
    var urlLink = '../Handlers/GenBetList.ashx?transid=' + transid + '&requesttype=2' + '&page=' + page;
    GeneralAjaxCall(urlLink);
}
function ViewMainReport() {
    try { parent.BlockUI(); } catch (e) { }
    var requesttype = 1;
    var rdTransID = obj('rdTransID').checked;
    var rdBettype = obj('rdBetType').checked;
    if (rdTransID == true) {
        requesttype = 2;
        var transid = obj('txtTransID').value;
        if (transid != "") {
            if (isNaN(transid) == true) {
                alert('Ref No should be a number!');
                obj('txtTransID').focus();
                try { parent.jQuery.unblockUI(); } catch (e) { }
            }
            else

                ViewMainReportByTransID(transid);
        }
        else {
            alert('Please input a Ref No!');
            obj('txtTransID').focus();
            try { parent.jQuery.unblockUI(); } catch (e) { }
        }
    }
    else {
        requesttype = 1;
        var bettype = obj('dlBettype').value;
        var fdate = obj('fdate').value;
        var tdate = obj('tdate').value;
        ViewMainReportByBettype(fdate, tdate, bettype);
        obj('txtTransID').value = '';
    }
}

function ViewMainReport2(page) {
    try { parent.BlockUI(); } catch (e) { }

    var requesttype = 1;
    var rdTransID = obj('rdTransID').checked;
    var rdBettype = obj('rdBetType').checked;

    if (rdTransID == true) {
        requesttype = 2;
        var transid = obj('txtTransID').value;
        if (transid != "") {
            if (isNaN(transid) == true) {
                alert('Ref No should be a number!');
                obj('txtTransID').focus();
                try { parent.jQuery.unblockUI(); } catch (e) { }
            }
            else
                ViewMainReportByTransID(transid, page);
        }
        else {
            alert('Please input a Ref No!');
            obj('txtTransID').focus();
            try { parent.jQuery.unblockUI(); } catch (e) { }
        }
    }
    else {
        requesttype = 1;
        var bettype = obj('dlBettype').value;
        var fdate = obj('fdate').value;
        var tdate = obj('tdate').value;
        ViewMainReportByBettype2(fdate, tdate, bettype, page);
        obj('txtTransID').value = '';
    }
}

function DoReset() {
    obj('rdBetType').checked = true;
    obj('txtTransID').value = '';
    obj('dlBettype').selectedIndex = 0;
    reset_date();
    BL_ChangeOption();
    ChangeDateTime();
}

function PreSubmit(e) {
    e = (e) ? e : window.event;
    var key = (e.keyCode) ? e.keyCode : e.which;
    if (key == 13)
        obj("btnSubmit").click();
}

String.prototype.trimLeft = function() {
    return this.replace(/^\s*/, "");
}

String.prototype.trimRight = function() {
    return this.replace(/\s*$/, "");
}

String.prototype.trim = function() {
    return this.trimRight().trimLeft();
}
function ViewBLIPReport() {
    try { parent.BlockUI(); } catch (e) { }
    var ip = obj('ip').value;
    if (ip.trim() == "") {
        showAlertMessage('errMsg', 'Please input IP!', 1);
        obj('ip').focus();
        try { parent.jQuery.unblockUI(); } catch (e) { }
    }
    else {
        $('#errMsg').html('');
        var urlLink = '../Handlers/GenBetList.ashx?ip=' + ip + '&requesttype=3' + '&fdate=' + obj('fdate').value;
        GeneralAjaxCall(urlLink);
    }
}
function ViewBLByStatement(fdate) {
    try { parent.BlockUI(); } catch (e) { }
    var userName = gup("custname");
    var transTypeId = gup("transtypeid");

    var urlLink = '../Handlers/GenBetList.ashx?requesttype=5' + '&fdate=' + fdate + "&username=" + userName + "&transtypeid=" + transTypeId;
    GeneralAjaxCall(urlLink);
}
function GeneralAjaxCall(urlLink) {
    $.ajax({
        url: urlLink + '&nocache=' + new Date().getTime(),
        dataType: 'html',
        success: function(data) {
            obj('sContent').innerHTML = data;

            $(".pagenav_ibtn").click(function() {
                if (isNumberNotNull("txtpage")) {
                    var pageIndex = $("#txtpage").val();
                    GotoPage(pageIndex);
                }
            });

            $("#divcontainer").removeClass("allhidden");
            //check heigh of table
            var tbl = $("#btable");
            if (tbl.height() < 560) {
                //$("#btable").width("905px");
                //$("#htable").width("905px");
                //$(".ui-jqgrid-sdiv").css("margin-top", "-1px");

                //                $("#tdSTitle").width("685px");
                //                $("#tdSWL").width("105px");
                //                $("#tdSEnd").width("170px");

                //                $("#tdIpTitle").width("724px");
                //                $("#tdIpTotalWin").width("100px");
                //                $("#tdIpEnd").width("165px");
                //$("#tfoot").width("905px");
            }
            //-------

            try { parent.jQuery.unblockUI(); } catch (e) { }
        }
    });
}

function GotoPage1() {
    var txtPage = obj('txtpage');
    if (txtPage) {
        GotoPage(txtPage.value);
    }
}

function GotoPage(num) {
    ViewMainReport2(num);
}


function BL_ChangeOption() {
    var rdTransID = obj('rdTransID').checked;
    var rdBettype = obj('rdBetType').checked;
    if (rdTransID == true) {
        obj('dlBettype').disabled = true;
        obj('spanFromDate').disabled = true;
        obj('spanToDate').disabled = true;
        obj('fdate_trigger').disabled = true;
        obj('tdate_trigger').disabled = true;
        obj('txtTransID').disabled = false;
    }
    else {
        obj('dlBettype').disabled = false;
        obj('spanFromDate').disabled = false;
        obj('spanToDate').disabled = false;
        obj('fdate_trigger').disabled = false;
        obj('tdate_trigger').disabled = false;
        obj('txtTransID').disabled = true;
    }
}
function ChangeDateTime() {
    var fdate = obj('fdate').value;
    var tdate = obj('tdate').value;
    var urlLink = '../Handlers/GenBetList.ashx?requesttype=6' + '&fdate=' + fdate + '&tdate=' + tdate;
    $.ajax({
        url: urlLink + '&nocache=' + new Date().getTime(),
        dataType: 'html',
        success: function(data) {
            if (data != "") {
                obj('spanBettype').innerHTML = "<select id='dlBettype'><option value='0'>All</option>" + data + "</select>";
                obj('bettypeArea').style.display = 'inline';
            }
            else
                obj('bettypeArea').style.display = 'none';
        }
    });
}
function reset_date() {
    var dayNoReset = 7;
    var fdateId = 'fdate';
    var tdateId = 'tdate';
    var td = new Date();
    var fd = new Date(td.getTime() - dayNoReset * 24 * 60 * 60 * 1000);
    document.getElementById(fdateId).value = (fd.getMonth() + 1) + "/" + fd.getDate() + "/" + fd.getFullYear();
    document.getElementById(tdateId).value = (td.getMonth() + 1) + "/" + td.getDate() + "/" + td.getFullYear();
}

function openHRresult(url, title, width, heigh) {
    var theWidth = window.innerWidth ? window.innerWidth : document.body.offsetWidth;
    ibcWnd.open(url, title, theWidth / 2 - 270, 50, width, heigh)
}

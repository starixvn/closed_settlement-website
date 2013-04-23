(function($) {
    var cache = [];
    // Arguments are image paths relative to the current page.
    $.preLoadImages = function() {
        var args_len = arguments.length;
        for (var i = args_len; i--; ) {
            var cacheImage = document.createElement('img');
            cacheImage.src = arguments[i];
            cache.push(cacheImage);
        }
    }
})(jQuery)

function GetCustId() {
    return obj('custid').value;
}
function ClearUsername() {
    obj('username').value = "";
    window.location.reload(true);
}
function LoginAgent(agentName) {
    document.AgLoginForm.custName.value = agentName;
    document.AgLoginForm.submit(); 
}
function ChangeGivenCredit(ischanging) {
    try {
    
        if (typeof ischanging != 'undefined' && ischanging) {
            var spanGC = document.getElementById('spanGivenCredit');
            var txtGC = document.getElementById('txtGivenCredit');
            txtGC.value = replaceAll(replaceAll(spanGC.innerHTML," ",""), ",", "");
            spanGC.style.display = 'none';
            txtGC.style.display = ''
            txtGC.focus();
        }
        else {
            document.getElementById('spanGivenCredit').style.display = '';
            document.getElementById('txtGivenCredit').style.display = 'none';
        }
    }
    catch (e) {
    }
}


function checkKeyPress(e) {
    try {
        if (typeof e == 'undefined') e = window.event;
        var key = (typeof e.keyCode == 'undefined') ? e.which : e.keyCode;
        if (key == 27) {
            
            ChangeGivenCredit();
            return false;
        }
        if (key == 13) {
            var givencredit = document.getElementById('txtGivenCredit').value;
            if (!givencredit.isNumeric()) {
                alert('Invalid value!');
                return false;
            }
            var fr = self.frames['frmUpdate'];
            if (typeof fr != 'undefined' && fr) {
                fr.location.href = 'Handlers/UpdateGivenCredit.ashx?amount=' + givencredit + '&custid=' + GetCustId();
               
                return true;
            }
        }
    } catch (ex) {
        alert(ex);
    }
}

function changeStatus(type, hasDownLine) {
    try {
        var isUpdateDownline = false; //update HR status for all downline?

        switch (type) {
            case "suspended":
                yes = ($('#suspended').attr('class').indexOf("chkuncheck") > -1);
                if (typeof hasDownLine != 'undefined' && hasDownLine) { 
                    if (yes) {
                        if (!confirm("Are you sure you want to change Suspended status for this customer? \n Warning, all members under this agent can't place bet!"))
                            return;
                    }
                    else {
                        if (!confirm("Are you sure you want to change Suspended status for this customer? All downlines will be un-suspended as well"))
                            return;
                    }
                }
                else {
                    if (yes) {
                        if (!confirm("Are you sure you want to change Suspended status for this customer? Warning, this member can't place bet!"))
                            return;
                    }
                    else {
                        if (!confirm("Are you sure you want to change Suspended status for this customer?"))
                            return;
                    }
                }
                break;
            case "Outright":
                yes = ($('#disabledOutright').attr('class').indexOf("chkchecked") > -1);
                if (typeof hasDownLine !='undefined' && hasDownLine && yes) {
                    if (!confirm("Are you sure you want to change " + type +" status for this customer? \n Warning, all members under this agent can't place Outright bet!")) 
                        return;
                }
                else
                    if (!confirm("Are you sure you want to change " + type +" status for this customer?")) 
                        return;
                break;
            case "special":
                if (!confirm("Are you sure you want to change Special status for this customer?"))
                    return;
                yes = ($('#special').attr('class').indexOf("chkuncheck") > -1);
                break;
            case "dangerous":
                if (!confirm("Are you sure you want to change " + type + " status for this customer?")) return;
                yes = (document.getElementById('dangerous').innerHTML == "No");
                break;
            case "autompt":
                if (!confirm("Are you sure you want to change Auto MPT status for this customer?"))
                    return;
                yes = ($('#AllowAutoPT').attr('class').indexOf("chkuncheck") > -1);    
                //yes = (document.getElementById('AllowAutoPT').innerHTML == "No");
                break;
            case "closed":
                yes = ($('#ClosedStatus').attr('class').indexOf("chkuncheck") > -1);

                var isallowupdatecredit = "0";
                isallowupdatecredit = document.getElementById("isallowupdatecredit").value;

                if (yes && isallowupdatecredit == "1") {
                    ShowClosedPop(document.getElementById("ClosedStatus"));
                    return false;
                }
                else {
                    if (!confirm("Are you sure you want to change Closed status for this customer?")) return;
                    if (typeof hasDownLine != 'undefined' && hasDownLine && yes) {
                        if (!confirm('Check again!!!! Are you sure you want to close this customer and all downline?'))
                            return;
                    }
                }
                break;
            case "SelectedBetting":
                yes = ($('#SelectedBetting').attr('class').indexOf("chkuncheck") > -1);
                if (!confirm("Are you sure you want to change Selected Betting status for this customer?")) return;
                if ( yes == false && hasDownLine) {
                    if (confirm("WARNING!!! \n\n Are you sure you want to update Selected Betting on this customer and all downlines?")) isUpdateDownline=true;
                    else
                    isUpdateDownline=false;
                    }
                break;
            case "AutoSuspend":
                //yes = (document.getElementById('AutoSuspend').innerHTML == "No");
                yes = ($('#AutoSuspend').attr('class').indexOf("chkuncheck") > -1);
                if (hasDownLine) {
                    if (!confirm("WARNING!!! \n\n Are you sure you want to update auto suspend on this customer and all downlines?")) return;
                    isUpdateDownline = true;
                }
                else {
                    if (!confirm("WARNING!!! \n\n Are you sure you want to update AutoSuspend on this customer?")) return;
                    isUpdateDownline = false;
                }
                break;
            case "Internal":
                if (!confirm("Are you sure you want to change " + type + " status for this customer?")) return;
                yes = ($('#internalUser').attr('class').indexOf("chkuncheck") > -1);
                break; 
            default: return;
        }
        var _URL = 'Handlers/UpdateCustomerStatus.ashx?isupddownline=' + isUpdateDownline + '&type=' + type + '&status=';
        _URL += String(yes);
        _URL += '&custid=' + GetCustId();
        var fr = self.frames['frmUpdate'];
        if (typeof fr != 'undefined' && fr) fr.location.href = _URL;
    }
    catch (ex) {
        alert(ex);
    }
} 
function updateLimitPerIP(limitperid, isUpdateDownline) {
    var _URL = 'Handlers/UpdateCustomerStatus.ashx?isupddownline=' + isUpdateDownline + '&type=limitperip&status=true';
    _URL +="&limitperid="+ limitperid;
    _URL += "&custid=" + GetCustId();
    var fr = self.frames['frmUpdate'];
    if (typeof fr != 'undefined' && fr) fr.location.href = _URL;
}

function updateCloseStatus(type, hasDownLine) {
    if (hasDownLine) {
        if (!confirm("Are you sure you want to change Close status for this customer? Warning, all downlines will be closed as well!"))
            return;
    }
    else {
        if (!confirm("Are you sure you want to change Close status for this customer?"))
            return;
    }
    var yes = false;
    var isUpdateDownline = false;
    var isClearCredit = false;
    isClearCredit = document.getElementById('chkClearCredit').checked;
    yes = ($('#ClosedStatus').attr('class').indexOf("chkuncheck") > -1);
    var _URL = 'Handlers/UpdateCustomerStatus.ashx?isupddownline=' + isUpdateDownline + '&type=' + type + '&status=';
    _URL += String(yes);
    _URL += "&isClearCredit=" + String(isClearCredit);
    _URL += "&custid=" + GetCustId();
    var fr = self.frames['frmUpdate'];
    if (typeof fr != 'undefined' && fr) fr.location.href = _URL;
}
//showPopup(title, url, left, top, width, height, isreload, duration, fnRecall)
function openInfoPanel(url) {
    showPopup("Edit Customer", url, "", "", 600, 360, true, 2000,null);
}
function openMinMaxPtPanel(url) {
    if (url == "") return;
    window.open(url, "EditCust", "height=700,width=980,top=200,left=200,scrollbars=1");
}

function OpenEditVirtualExRate(id, uname, type) {
    var url = "Customer/EditVirtualExRate.aspx?custId=" + id + "&uname=" + uname + "&type=" + type + "&iscustinfo=yes";
    showPopup('Edit VirtualRate', url, 0, 0, 300, 180, true, 2000, null);
    //window.open(url, "ExitMaxEx", "height=160,width=300,top=200,left=200");
}

function OpenCloseMenuDangerous() {
    $('#divLevel').css('display', ($('#divLevel').css('display') == 'inline') ? 'none' : 'inline');
}
function ChangeDangerousLevel(typename, level, name, color) {
    if (isDigitOnly(level) == false) {
        return false;
    }
    
    if (parseInt(level) < 0 || parseInt(level) > 10) {
        return false;
    }

    var curLevel = document.getElementById('currentLevel' + typename.replace(/ /, "")).value;
    if (curLevel == level || !confirm("Are you sure you want to change " + typename + " dangerous level to '" + name + "' for this customer?")) {
        if (currlevelbox != "") {
            $("#" + currlevelbox).css('display','none');
        }
        return;
    }
    if (currlevelbox != "") {
        $("#" + currlevelbox).css('display', 'none');
    }
    var _URL = 'Handlers/UpdateCustomerStatus.ashx?type=dangerous&status=false&level=' + level + '&name=' + typename.replace(/ /, "") + "&custid=" + GetCustId();
    var fr = self.frames['frmUpdate'];
    if (typeof fr != 'undefined' && fr) fr.location.href = _URL;
}

function replaceAll(text, strA, strB) {
    while (text.indexOf(strA) != -1) {
        text = text.replace(strA, strB);
    }
    return text;
}

/* New version */
// function change dangerous for agent
function ChangeDangerousLevelAgent(typename,type, level) {
    if (isDigitOnly(level) == false) {
        return false;
    }
    
    if (parseInt(level) < 0 || parseInt(level) > 10) {
        return false;
    }

    var curLevel = document.getElementById('currentLevel' + typename.replace(/ /, "")).value;
    if (curLevel == level) {
        if (currlevelbox != "") {
            $("#" + currlevelbox).css('display', 'none');
        }
    }
    else {
        $('#dangerousAgentpopup' + type + "_" + level).toggle();
        overlay("dangerousAgentoverlay");
    }
    
}

function left_Tab1_Click(item) {
    $("#left_tab1 .current").removeClass("current");
    $("#" + item.id).addClass("current");
    $('#box_04_contents').each(function() {
        $(this).children().css("display", "none");
    });

    if (item.id.indexOf("_1") >= 0) {
        $("#box_04_content_1").css("display", "block");
        $('#pane2').jScrollPane({ showArrows: true, scrollbarWidth: 15, arrowSize: 16 });
        $(".pagerbox").css("display", "none");
    }
    else {
        $("#box_04_content_2").css("display", "block");
        try{
            $('#ctrDownlines').jScrollPane({ showArrows: true, scrollbarWidth: 15, arrowSize: 16 });
        }
        catch (error){}
        if (totalPage > 1) {
            $(".pagerbox").css("display", "block");
        }
    }
}

function left_Tab2_Click(item) {
    $("#left_tab2 .current").removeClass("current");
    $("#" + item.id).addClass("current");
   
    for (var i=1;i<=4;i++)
    {
         $("#box_05_content_" + i).addClass("hidden");
    }
   
    if (item.id.indexOf("_1") >= 0) {
        $("#box_05_content_1").removeClass("hidden");
     
        
    }
    else if (item.id.indexOf("_2") >= 0) {
        $("#box_05_content_2").removeClass("hidden");
     
    }
    else if (item.id.indexOf("_3") >= 0) {
        $("#box_05_content_3").removeClass("hidden");
     
    }
    else {
        $("#box_05_content_4").removeClass("hidden");
      
        $('#pane21').jScrollPane({ showArrows: true, scrollbarWidth: 15, arrowSize: 16 });
    }
}

function detail_tab_Click(item) {
    $("#detail_tab .left_tab_on").removeClass("left_tab_on");
    $("#" + item.id).addClass("left_tab_on");

    $('#detail_tab_contents').each(function() {
        $(this).children('div').css("display", "none");
    });

    if (item.id.indexOf("_1") >= 0) {
        $("#detail_tab_content_1").css("display", "block");
    }
    else if (item.id.indexOf("_2") >= 0) {
        $("#detail_tab_content_2").css("display", "block");
    }
    else {
        $("#detail_tab_content_3").css("display", "block");
    }
}

function detail_tabnew_Click(item) {
    $("#de_left .current").removeClass("current");
    $("#" + item.id).addClass("current");

    $('#detail_tab_contents').each(function() {
        $(this).children('div').addClass("hidden");
    });

    if (item.id.indexOf("_1") >= 0) {
        $("#detail_tab_content_1").removeClass("hidden");//.css("display", "block");
    }
    else if (item.id.indexOf("_2") >= 0) {
    $("#detail_tab_content_2").removeClass("hidden"); ;
    }
    else {
        $("#detail_tab_content_3").removeClass("hidden"); ;
    }
}

function menudetailclick(item) {
    $("#de_left .current").removeClass("current");
    $("#" + item.id).addClass("current");
}
    
function StatementClick() {
    parent.ParentStatementClick();
}

//document.myFunc = function() { alert('!') } //is called from parent

function DoTransfer(span, value, isYesterday) {
    if (span.className.indexOf('canchange')==-1) return;
    //parent.BlockUI();
    var yesterdayTransfer = (isYesterday) ? '&isYesterday=1' : '';
    var url = 'Transfer.aspx?amount=' + value + yesterdayTransfer;
    //submitForm(url);
    parent.gotoURL('centerFr', url);
    parent.resetMenu();
}
function OpenViewPT(username) {
    var url = 'Customer/PT/ViewPT/ViewPT.aspx?username=' + username;
    parent.gotoURL('centerFr', url);
    parent.resetMenu();
}

function collapseInfo() {
    var curr = $("#btnupdownCustInfo");
    var obj = $("#custInfoDetal");
    if (obj.hasClass("hidden")) {
        obj.removeClass("hidden");
        Set_Cookie("coUserInfo", "show", 7, "/", "", "");
        curr.attr("title", "Hide Detail");
        curr.html('<img src="App_Themes/Images/icon_down.gif" class="imgDownUp" />');
    }
    else {
        obj.addClass("hidden");
        Set_Cookie("coUserInfo", "hidden", 7, "/", "", "");
        curr.html('<img src="App_Themes/Images/icon_up.gif" class="imgDownUp" />');
        curr.attr("title", "Show Detail");
    }
}

function changeDisableDangerAgent(type, level) {
    if ($('#disabledDangerchk'+ type + "_" + level).attr('checked')) {

        $('#disabledangerouschkDL' + type + "_" + level).attr('disabled', false);
        $('#UpdateDangerous' + type + "_" + level).attr("disabled", false);
    }
    else {
        $('#disabledangerouschkDL' + type + "_" + level).attr('disabled', true);
        $('#UpdateDangerous' + type + "_" + level).attr("disabled", true);
    }
}

function isDigitOnly(s) {
    // Check if string is a whole number(digits only).
    var isWhole_re = /^\s*\d+\s*$/;
    return String(s).search(isWhole_re) != -1;
}

function UpdateDangerousCustomer(typename, type, level) {
    var custId = GetCustId();
    var updateDownline = "0";
    if ($('#disabledangerouschkDL'+ type + "_" + level).attr('checked')) {
        updateDownline = "1";
    }
    if (level == "None") level = "0";
    var param = { "custId": custId, "updateDownline": updateDownline, "dangerType": type, "dangerLevel": level };
    var resultDialog = false;
    if (updateDownline == "0") {
        resultDialog = confirm("Are you sure you want to change " + typename + " dangerous level to '" + level + "' for this customer?");
    }
    else {
        resultDialog = confirm("Are you sure you want to change " + typename + " dangerous level to '" + level + "' for this customer and all downlines?");
    }
    if (resultDialog) {
        if (PTBlockUI) PTBlockUI();
        $.ajax({
            type: "POST",
            url: "Services/WCF/Customer/ServiceCustomerInfo.svc/UpdateDangerousCustomerSer",
            data: $.toJSON(param),
            contentType: "application/json; charset=utf-8",
            success: function (rs) {
                var resetLevelLabel = function (span, level) {
                    if (level == "0")
                        $(span).html("None");
                    else
                        $(span).html(level);
                    switch (level) {
                        case "0":
                            {
                                $(span).parent()[0].style.backgroundColor = "White";
                                break;
                            }
                        case "1":
                            {
                                $(span).parent()[0].style.backgroundColor = "Pink";
                                break;
                            }
                        case "2":
                            {
                                $(span).parent()[0].style.backgroundColor = "yellow";
                                break;
                            }
                        case "3":
                            {
                                $(span).parent()[0].style.backgroundColor = "orange";
                                break;
                            }
                        case "4":
                            {
                                $(span).parent()[0].style.backgroundColor = "green";
                                break;
                            }
                        case "5":
                            {
                                $(span).parent()[0].style.backgroundColor = "Red";
                                break;
                            }
                        case "6":
                            {
                                $(span).parent()[0].style.backgroundColor = "Purple";
                                break;
                            }
                        case "7":
                            {
                                $(span).parent()[0].style.backgroundColor = "Brown";
                                break;
                            }
                        case "8":
                            {
                                $(span).parent()[0].style.backgroundColor = "Gray";
                                break;
                            }
                        case "9":
                            {
                                $(span).parent()[0].style.backgroundColor = "Lime";
                                break;
                            }
                        case "10":
                            {
                                $(span).parent()[0].style.backgroundColor = "Aqua";
                                break;
                            }
                    }
                }
                if (typeof rs.ErrorCode != "undefined" && rs.ErrorCode == 600 && rs.ErrorMessage != "") {
                    $.msg({ content: rs.ErrorMessage, timeOut: 5000, fadeIn: 500, fadeOut: 200 });
                }
                else if (rs && rs.ErrorCode == 0 && rs.ErrorMessage == "") {
                    switch (typename) {
                        case "Original":
                            {
                                resetLevelLabel(document.getElementById("spanDangerousOriginal"), level);
                                break;
                            }
                        case "ABT":
                            {
                                resetLevelLabel(document.getElementById("spanDangerousABT"), level);
                                break;
                            }
                        case "Other Sports":
                            {
                                resetLevelLabel(document.getElementById("spanDangerousOtherSports"), level);
                                break;
                            }
                    }
                }

                ClosePopUpDanger("dangerousAgentpopup" + type + "_" + level);
                if (currlevelbox != "") {
                    $("#" + currlevelbox).css('display', 'none');
                }
                if (PTUnBlockUI) PTUnBlockUI();
            }
            , error: function (e) {
                if (PTUnBlockUI) PTUnBlockUI();
            }
        });
    }
    else 
    {
        ClosePopUpDanger("dangerousAgentpopup" + type + "_" + level);
        if (currlevelbox != "") {
            $("#" + currlevelbox).css('display', 'none');
        }
    }   
}


function ShowAllBettypes() {
    if ($("#dvBettypes").html().length > 0) {
        if ($('#chkBettypes').is(':checked')) {
            $("#dvBettypes").show();
        }
        else {
            $("#dvBettypes").hide();
        }
        return;
    }    


    parent.BlockUI();
    $.ajax({
        type: "POST",
        url: "../../Services/WCF/Report/NonePT/NonePTLastestBet.svc/getAllBettypes",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function(rs) {
            var rsvalue = $.evalJSON(rs);
            $("#dvBettypes").html(rsvalue.bettypes);
            parent.jQuery.unblockUI();
        }
    });
}

function on_changeBetype(obj) {
    var chkBettype = document.getElementsByName('chkBettype');
    var countChecked = 0;
    for (i = 0; i < chkBettype.length; i++) {
        if (chkBettype[i].checked)
            countChecked++;
    }
    $('#chkAllBettype').attr('checked', (chkBettype.length == countChecked));
}

function CheckAllBettype(obj) {
    var chkBettype = document.getElementsByName('chkBettype');
    for (i = 0; i < chkBettype.length; i++) {
        chkBettype[i].checked = obj.checked;
    }    
}

function GetCurrency() {
    if ($('#dvCurrency').html().length == 0) {
      
        parent.BlockUI();
        $.ajax({
            type: "POST",
            url: "../../Services/WCF/Report/NonePT/NonePTLastestBet.svc/getAllCurrencies",                  
            contentType: "application/json; charset=utf-8",
            dataType: "json",            
            success: function(rs) {
                var rsvalue = $.evalJSON(rs);
                $("#dvCurrency").html(rsvalue.currency);
                parent.jQuery.unblockUI();
            }
        });
    }
}




function getParams(pageindex) {
    var username = $('#txtUsername').val();
    var currency = $('#ddlCurrency').val();

    var bettype = '';

    var param = 'type=getData';
    var isBettype = $('input[id=chkBettypes]').is(':checked');

    if (isBettype) {

        var chkBettypes = document.getElementsByName('chkBettype');
        for (i = 0; i < chkBettypes.length; i++) {
            if (chkBettypes[i].checked) {
                bettype += chkBettypes[i].value.split('^')[0] + '^';
            }
        }
    }


    var fromstake = $('#txtFromStake').val();
    var tostake = $('#txtToStake').val();

    if (fromstake.length == 0) fromstake = '0';
    if (tostake.length == 0) tostake = '0';

    var pagesize = $('#ddlPageSize').val();

    var strParam = '';
    var arrKey = ['username', 'currency', 'bettype', 'fromstake', 'tostake', 'pageindex', 'pagesize'];
    var arrValue = [username, currency, bettype, fromstake, tostake, pageindex, pagesize];

    for (i = 0; i < arrKey.length; i++) {
        strParam += '"' + arrKey[i] + '":"' + arrValue[i] + '"';
        if (i < arrKey.length - 1)
            strParam += ',';
    }
    return strParam;
}

var timeIdRemainTime = 0;
function GetData(pageindex) {
    $('#errMsg').hide();
    if (!pageindex) pageindex = 1;
    var strParam = getParams(pageindex);
    callAjax(strParam);

    if (timeIdRemainTime == 0)
    timeIdRemainTime = setTimeout('CalcRemainTime(' + pageindex + ')', 1000);
}

function callAjax(postdata) {
    try { parent.BlockUI(); } catch (e) { }
    $.ajax({
        type: "POST",
        url: "../../Services/WCF/Report/NonePT/NonePTLastestBet.svc/getNonePTLastestBet",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        data: "{" + postdata + "}",
        success: BindData
    });
}

function BindData(rs) {
    var rsvalue = $.evalJSON(rs);
    $("#sContent").html(rsvalue.data);

    if ($("#sContent").html().length == 0)
        $('#dvMainTable').hide();
    else
        $('#dvMainTable').show();

    var topHeader = $('#trHeader').position().top;
    var topFooter = $('#dvPaging').position().top;

    var innerHeight = window.innerHeight;
    if (!innerHeight) {
        innerHeight = document.documentElement.clientHeight;
    }

    var RealHeight = $(window).height() - topHeader - 210;
    

    if ((topFooter - topHeader) > $(window).height()) {
        $('#bdiv').height(RealHeight + 'px');
    }
    else {
        $('#bdiv').height('auto');
    }

    //bind event for pager
    $(".pagenav_ibtn").click(function() {
        if (isNumberNotNull("txtpage")) {
            var pageIndex = $("#txtpage").val();
            if (parseInt(pageIndex) <= parseInt(rsvalue.Totalpages)) {
                var hash = this.href + pageIndex;
                hash = hash.replace(/^.*#/, '');
                GetData(hash);
            }
            return false;
        }
        else {
            return false;
        }
    });

    $(".pagerlink").click(function() {
        var hash = this.href;
        hash = hash.replace(/^.*#/, '');
        GetData(hash);
        return false;
    });

    parent.jQuery.unblockUI();

}

function showMP(refno, listType) {
    parent.BlockUI();
    var spEvent = obj('spEvent_' + refno);
    if (spEvent.className == 'hide') {
        if (spEvent.innerHTML == "") {
            $.ajax({
                url: '../../Handlers/GenMixParlay.ashx?refno=' + refno + '&listType=' + listType + '&type=1',
                dataType: 'html',
                success: function(data) {
                    spEvent.innerHTML = data;
                    spEvent.className = '';
                    parent.jQuery.unblockUI();
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

    parent.jQuery.unblockUI();
}

function CalcRemainTime(pageindex) {
    if (!$('#IsPause').is(':checked')) {
        clearTimeout(timeIdRemainTime);
        timeIdRemainTime = 0;
    }
    
    var second = $('#spRemainTime').html().replace('(', '').replace(')', '');
    if (isNaN(second)) {
        second = parseInt($('#ddlReloadTime').val());
        if (second < 10) second = 10;
    }

    var t = parseInt(second) - 1;

    if (t <= 0) {
        if (!pageindex) pageindex = 0;
         GetData(pageindex);
        $('#spRemainTime').html('('+ $('#ddlReloadTime').val() + ')');
    }
    else
        $('#spRemainTime').html('(' + t + ')');

    if (timeIdRemainTime == 0) {
        timeIdRemainTime = setTimeout('CalcRemainTime()', 1000);
    }
}
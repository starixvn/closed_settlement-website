var maxGridHeight = $(window).height() - 100;
var resultpage = {
    width: 981,
    height: maxGridHeight,
    arrQueue: [],
    arrRow: []
};

resultpage.init = function () {
    $("#fdate").datepicker({
        changeMonth: true,
        changeYear: true,
        today: new Date($("#hiddenServerDate").val()),
        onSelect: function (selectedDate) {
            resultpage.getLeagues();
        }
    });


    $("#fdate").datepicker("setDate", new Date($("#hiddenServerDate").val()));

    resultpage.fillCombox(leagues);

    $("#btnSubmit").click(function () {
        resultpage.getData();
    });
};

resultpage.blockUI = function () {
    if (parent.parent.BlockUI) {
        try { parent.parent.BlockUI(); } catch (e) { }
    }
    else {
        try { parent.BlockUI(); } catch (e) { }
    }
};

resultpage.unBlock = function () {
    if (parent.parent.$ && parent.parent.$.unblockUI) {
        try { parent.parent.$.unblockUI(); } catch (e) { }
    }
    else {
        try { parent.$.unblockUI(); } catch (e) { }
    }
};

resultpage.getLeagues = function () {
    resultpage.blockUI();
    $.ajax({
        type: "POST",
        url: "service/SettleLiveCasinoService.svc/GetLeagues",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        data: '{"fdate":"' + $("#fdate").val() + '", "tdate": "' + $("#fdate").val() + '"}',
        success: function (rs) {
            resultpage.fillCombox(rs);
            resultpage.unBlock();            
            $('#divStatus').html(resultpage.renderStatus(false));        
        }
    });
};

resultpage.fillCombox = function (rs) {
    if (typeof rs == "undefined" || rs == null) return;
    $("#selLeagues").empty();
    $("#selLeagues").append("<option value='' selected='selected'>*..::¤ Show All Leagues¤::..*</option>");
    for (var i = 0; i < rs.length; ++i) {
        $("#selLeagues").append("<option value='" + rs[i].LeagueId + "'>" + rs[i].LeagueName + "</option>");
    }
};

resultpage.getLeagueIds = function () {
    if ($("#selLeagues").val() != "") {
        return $("#selLeagues").val();
    }
    var ids = [];
    $("#selLeagues").find("option").each(function () {
        ids.push($(this).val());
    });
    return ids.join(",");
};

resultpage.getData = function () {
    var fdate = $("#fdate").val();
    var tdate = $("#fdate").val();
    $("#tdtitle1").html("EVENT FROM " + fdate + " 12:00:00 AM TO " + tdate + " 12:00:00 AM");
        
    resultpage.blockUI();
    $.ajax({
        type: "POST",
        url: "service/SettleLiveCasinoService.svc/GetMatches",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        data: '{"fdate":"' + $("#fdate").val() + '", "tdate": "' + $("#fdate").val() + '", "LeagueIds": "' + resultpage.getLeagueIds() + '", "statusId": ' + $('#ddlStatus').val() + '}',
        success: function (rs) {
            resultpage.buildGrid(rs);
        }
    });
};

resultpage.buildGrid = function (rs) {
    $("#divContainer").show();
    if ($("#selLeagues").find("option").length == 1 || rs.length <= 0) {
        $("#tobdyMainGrid").empty().append("<tr><td style='width:" + (resultpage.width - 5) + "px;text-align:center'>There is no information</td></tr>");
        $("#divMainGrid").removeClass("hidden").reRenderScroller(resultpage.width, resultpage.height, true);
        resultpage.unBlock();
    }
    else {
        $("#tobdyMainGrid").empty();
        resultpage.arrQueue = [];
        resultpage.arrRow = [];
        var queue = new Queue();
        resultpage.arrQueue.push(queue);
        for (var i = 0; i < rs.length; ++i) {
            queue.receive(rs[i]);
        }
        queue.process(function (obj) {
            resultpage.renderLeague(obj);
        }, 1);
        var timerId = setInterval(function () {
            if (!resultpage.isQueueFinishAll()) return;
            clearInterval(timerId);
            while (resultpage.arrRow.length > 0) {
                var item = resultpage.arrRow.pop();
                $("#tobdyMainGrid").append(item.rows);
            }
            $("#divMainGrid").removeClass("hidden").reRenderScroller(resultpage.width, resultpage.height, true);
            resultpage.unBlock();
        }, 10);
    }
};

resultpage.isQueueFinishAll = function () {
    if (typeof this.arrQueue == "undefined") return true;
    for (var i = 0; i < this.arrQueue.length; ++i) {
        if (!this.arrQueue[i].finish) return false;
    }
    return true;
};

resultpage.renderLeague = function (model) {
    var arr = [];
    var obj = { LeagueName: model.LeagueName };
    if (obj.LeagueName.toLowerCase().indexOf("baccarat") >= 0) {
        obj.FinalScore = "(P - B)";
    }
    else {
        obj.FinalScore = "";
    }
    arr.push($("#stLeague").tmpl([obj]).get(0));
    for (var k = 0; k < model.Matches.length; ++k) {
        arr.push($("#stMatch").tmpl([model.Matches[k]]).get(0));
    }
    resultpage.arrRow.push({ rows: arr });
};

resultpage.renderStatus = function (isAll) {
    var html = [];
    html.push('<select id="ddlStatus">');
    if (isAll) {
        html.push('<option value="0">All</option>');
    }
    html.push('<option value="1">Running</option>');
    html.push('<option selected="selected" value="2">Completed</option>');
    html.push('<option value="3">Refund</option>');
    html.push('<option value="5">Closed</option>');
    html.push('</select>');
    return html.join('');
}

resultpage.leagueOnChange = function () {
    var selected = $('#selLeagues').val();    
    if (selected == '') {
        $('#divStatus').html(resultpage.renderStatus(false));       
    }
    else {
        $('#divStatus').html(resultpage.renderStatus(true));        
    }


}; 


$(document).ready(function () {
    resultpage.init();
});

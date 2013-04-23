var settlelc = {
    width: 1225,
    height: 680,
    arrQueue: []
};

settlelc.init = function () {
    var dates = $("#fdate, #tdate").datepicker({
        changeMonth: true,
        changeYear: true,
        today: new Date($("#hiddenServerDate").val()),
        onSelect: function (selectedDate) {
            var option = this.id == "fdate" ? "minDate" : "maxDate",
				instance = $(this).data("datepicker"),
				date = $.datepicker.parseDate(
					instance.settings.dateFormat ||
					$.datepicker._defaults.dateFormat,
					selectedDate, instance.settings);
            dates.not(this).datepicker("option", option, date);
            settlelc.getLeagues();
        }
    });

    $("#fdate").datepicker("setDate", new Date($("#hiddenServerDatePrevious").val()));
    $("#tdate").datepicker("setDate", new Date($("#hiddenServerDate").val()));

    settlelc.fillCombox('selLeagues', leagues);

    $("#btnSubmit").click(function () {
        settlelc.getData();
    });

    $("#iframeData").load(function () {
        settlelc.unBlock();
    });
    if ($.browser.msie) {
        $("#iframeData").height(700);
    }
};

settlelc.blockUI = function () {
    if (parent.parent.BlockUI) {
        try { parent.parent.BlockUI(); } catch (e) { }
    }
    else {
        try { parent.BlockUI(); } catch (e) { }
    }
};

settlelc.unBlock = function () {
    if (parent.parent.$ && parent.parent.$.unblockUI) {
        try { parent.parent.$.unblockUI(); } catch (e) { }
    }
    else {
        try { parent.$.unblockUI(); } catch (e) { }
    }
};

settlelc.getLeagues = function () {
    settlelc.blockUI();
    $.ajax({
        type: "POST",
        url: "service/SettleLiveCasinoService.svc/GetLeagues",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        data: '{"fdate":"' + $("#fdate").val() + '", "tdate": "' + $("#tdate").val() + '"}',
        success: function (rs) {
            settlelc.fillCombox('selLeagues', rs);
            settlelc.unBlock();
        }
    });
};

settlelc.fillCombox = function (id, rs) {
    if (typeof rs == "undefined" || rs == null) return;
    $("#" + id).empty();    
    for (var i = 0; i < rs.length; ++i) {
        $("#" + id).append("<option value='" + rs[i].LeagueId + "'>" + rs[i].LeagueName + "</option>");
    }
};

settlelc.getLeagueIds = function () {
    if ($("#selLeagues").val() != "") {
        return $("#selLeagues").val();
    }
    var ids = [];
    $("#selLeagues").find("option").each(function () {
        ids.push($(this).val());
    });
    return ids.join(",");
};

settlelc.getStatusId = function () {
    if ($("#selStatus").val() != "") {
        return $("#selStatus").val();
    }
    return 0;
}

settlelc.getData = function () {
    settlelc.blockUI();
    var param = "fdate=" + $("#fdate").val() + "&tdate=" + $("#tdate").val() + "&LeagueIds=" + settlelc.getLeagueIds() + '&status=' + settlelc.getStatusId();
    $("#iframeData").attr("src", "GridDataSettlment.aspx?" + param);
};

settlelc.buildGrid = function (rs) {
    $("#divContainer").show();

    //rendertype: queue
    if ($("#selLeagues").find("option").length == 1) {
        $("#tobdyMainGrid").empty().append("<tr><td><div style='text-align:center;width:" + (settlelc.width - 264) + "px'>There is no information!</div></td></tr>");
        $("#divMainGrid").removeClass("hidden").reRenderScroller(settlelc.width, settlelc.height, true);
        settlelc.unBlock();
        return;
    }

    $("#tobdyMainGrid").empty();
    settlelc.arrQueue = [];
    var queue = new Queue();
    settlelc.arrQueue.push(queue);
    if (rs.length > 1) {
        settlelc.renderLeague(rs.pop());
    }
    for (var i = 0; i < rs.length; ++i) {
        queue.receive(rs[i]);
    }
    queue.process(function (obj) {
        settlelc.renderLeague(obj);
    }, 1000);
    var timerId = setInterval(function () {
        if (!settlelc.isQueueFinishAll()) return;
        clearInterval(timerId);
        $("#divMainGrid").removeClass("hidden").reRenderScroller(settlelc.width, settlelc.height, true);
    }, 10);
    /*

    /*
    //-------------------------------------------------------------------------------------------
    //rendertype: server render
    if (rs == "") {
    $("#tobdyMainGrid").empty().append("<tr><td><div style='text-align:center;width:" + (settlelc.width - 4) + "px'>There is no information!</div></td></tr>");
    $("#divMainGrid").removeClass("hidden").reRenderScroller(settlelc.width, settlelc.height, true);
    settlelc.unBlock();
    return;
    }
    $("#tobdyMainGrid").empty().append(rs);
    $("#divMainGrid").removeClass("hidden").reRenderScroller(settlelc.width, settlelc.height, true);
    settlelc.unBlock();
    */
};

settlelc.isQueueFinishAll = function () {
    if (typeof this.arrQueue == "undefined") return true;
    for (var i = 0; i < this.arrQueue.length; ++i) {
        if (!this.arrQueue[i].finish) return false;
    }
    return true;
};

settlelc.renderLeague = function (model) {
    var arr = [];
    var obj = { LeagueName: model.LeagueName };
    if (obj.LeagueName.toLowerCase().indexOf("baccarat") >= 0) {
        obj.FinalScore = "(P - B)";
    }
    else {
        obj.FinalScore = "";
    }
    if (model.Matches != null) {
        arr.push($("#stLeague").tmpl([obj]).get(0));

        var queue = new Queue();
        settlelc.arrQueue.push(queue);

        for (var k = 0; k < model.Matches.length; ++k) {
            queue.receive(model.Matches[k]);
        }

        queue.process(function (obj) {
            arr.push($("#stMatch").tmpl([obj]).get(0));
        }, 1);
        var timerId = setInterval(function () {
            if (queue.finish) {
                clearInterval(timerId);
                $("#tobdyMainGrid").append(arr);
                $("#divMainGrid").removeClass("hidden").reRenderScroller(settlelc.width, settlelc.height, true);
                settlelc.unBlock();
            }
        }, 10);
    }
};

$(document).ready(function () {
    settlelc.init();
});
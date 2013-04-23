
$(function () {

    var puntergrid = $("#puntergridSummary");
    puntergrid.scroller();

    var puntergridByCustId = $("#puntergridByCustId");
    puntergridByCustId.scroller();

    var puntergridBySporttype = $("#puntergridBySporttype");
    puntergridBySporttype.scroller();

    var puntergridByBettype = $("#puntergridByBettype");
    puntergridByBettype.scroller();

    punter.setSearchBy(0);
    punter.setPeriodType(0);
    punter.loadSports();

    punter.SportSelected = punter.getSelectedSport();


});

$(document).delegate('html', 'click', function (e, isInsideCall) {
    var $target = $(e.target);
    if (!$target.closest('div.ui-multiselect-options').length && !$target.parent().hasClass('ui-multiselect')) {

        //------------ Sport by League
        var rs = $('div.ui-multiselect-options:last');
        if (rs.css('display') == 'block') {
            punter.SportSelected = punter.getSelectedSport();
            if (punter.SportSelected.length == 0) {
                showAlertMessage("msg", "Please choose at least one sport.", 1);
                $('#ddlLeagues').empty();
                $('#btnReport2').attr('disabled', true);
            }
            else {
                $('#msg').empty();
                punter.loadLeagueBySport();
                $('#btnReport2').attr('disabled', false);
            }
        }

        //------------ Sport By User
        rs = $('div.ui-multiselect-options:first');
        if (rs.css('display') == 'block') {
            punter.SportSelected = punter.getSelectedSport();
            if (punter.SportSelected.length == 0) {
                showAlertMessage("msg", "Please choose at least one sport.", 1);
                $('#btnReport1').attr('disabled', true);
            }
            else {
                $('#msg').empty();
                $('#btnReport1').attr('disabled', false);
            }
        }
    }
});

var Helper = {
    format: function (num) {
        var n = num;

        var IsMargin = num.indexOf('%') > 0;

        n = num.replace('%', '');
        if (parseFloat(n) < 0) {
            n = num.replace('-', '');
            return '<span style="color:red">(' + n + ')</span>'
        }

        return num;

    }
};

var punter = {
    SearchBy: 0, // 0:User, 1:League
    PeriodType: 0, // 0:Monthly, 1:Annual, 2:Long Range
    arrMonths: ['January', 'February', 'March', 'April', 'May', 'June', 'July', 'August', 'September', 'October', 'November', 'December'],
    beginYear: 2007,
    SportSelected: '',
    param: {
        FromDate: '',
        ToDate: '',
        SportId: '',
        BettypeId: '',
        UserName: '',
        TopRow: '',
        OrderBy: '', //@Orderby: 1 winlost, 2 turnover, 3 margin 
        OrderType: '', //@OrderType: 0 Ascending, 1 Descending
        LeagueId: '', //0:All
        CustId: '',
        liveindicator: '',
        PeriodType: ''
    },
    model: [],

    endYear: function () {
        var d = new Date();
        return d.getFullYear();
    },
    genMonth: function (isCurrent) {
        var d = new Date();
        var month = '';
        for (var i = 0; i < punter.arrMonths.length; i++) {
            if (i == d.getMonth() - (isCurrent ? 0 : 1)) {
                month += '<option selected="selected" value=' + (i + 1) + '>' + punter.arrMonths[i] + '</option>';
            }
            else {
                month += '<option value=' + (i + 1) + '>' + punter.arrMonths[i] + '</option>';
            }
        }
        return month;
    },

    genYear: function () {
        var d = new Date();
        var year = '';
        for (var i = punter.beginYear; i <= punter.endYear(); i++) {
            if (i == d.getFullYear()) {
                year += '<option selected="selected" value=' + i + '>' + i + '</option>';
            }
            else {
                year += '<option value=' + i + '>' + i + '</option>';
            }
        }

        return year;
    },

    setSearchBy: function (type) {
        punter.SearchBy = type;

        $('#dvSearchBy input[type="radio"]').removeAttr("checked");
        $('#msg').empty();
        switch (type) {
            case 0: // Search by User
                $('#dvUserName').show();
                $('#dvSportByLeague').hide();
                $('#rdUser').prop('checked', true);
                $('#spReport1').show();
                $('#spReport2').hide();
                $('#dvSport').show();
                break;
            case 1: //Search By League
                $('#dvUserName').hide();
                $('#dvSportByLeague').show();
                $('#rdLeague').prop('checked', true);
                $('#spReport1').hide();
                $('#spReport2').show();
                $('#dvSport').hide();
                punter.loadLeagueBySport();
                break;
        }
    },

    setPeriodType: function (type) {
        punter.PeriodType = type;

        $('#dvPeriodType input[type="radio"]').removeAttr("checked");

        switch (type) {
            case 0:
                $('#spPeriodType').html('<span>MONTHLY From</span><span style="padding-left:13px"><select onchange="punter.loadLeagueBySport();" id="ddlMonth">' + punter.genMonth(false) + '</select></span>\
                <span>Year</span><span><select onchange="punter.loadLeagueBySport();" id="ddlYear">' + punter.genYear() + '</select></span>');
                $('#rdMonthly').attr('checked', true);
                break;
            case 1:
                $('#spPeriodType').html('<span>ANNUAL For Year</span><span style="padding-left:3px"><select onchange="punter.loadLeagueBySport();" id="ddlYear">' + punter.genYear() + '</select></span></span>');
                $('#rdAnnual').attr('checked', true);
                break;
            case 2:
                $('#spPeriodType').html('<span>From</span><span style="padding-left:58px">\
                <select onchange="punter.loadLeagueBySport();" id="ddlfMonth">' + punter.genMonth(false) + '</select></span>\
                <span><select onchange="punter.loadLeagueBySport();" id="ddlfYear">' + punter.genYear() + '</select></span>\
                <span style="padding-left:20px">To</span>\
                <select onchange="punter.loadLeagueBySport();" id="ddltMonth">' + punter.genMonth(true) + '</select></span>\
                <span><select onchange="punter.loadLeagueBySport();" id="ddltYear">' + punter.genYear() + '</select></span>');
                $('#rdLongRange').attr('checked', true);
                break;
        }
    },

    getSelectedSport: function () {
        var id = 'ddlSportLeague';
        switch (punter.SearchBy) {
            case 0:
                id = 'ddlSportUser';
                break;
            case 1:
                id = 'ddlSportLeague';
                break;
        }
        var selectedList = $('#' + id).multiSelect('getCheckedItems');
        var result = "";
        for (var i = 0; i < selectedList.length; i++) {
            result = result + selectedList[i] + ",";
        }
        result = result.replace(/,*$/, "");
        return result;
    },

    daysInMonth: function (month, year) {
        return new Date(year, month, 0).getDate();
    },

    setDefaultTopRows: function () {
        if ($('#txtTopRows').val() == '' || isNaN($('#txtTopRows').val())) {
            $('#txtTopRows').val('10');
        }
    },

    setParam: function () {
        var fromdate = '';
        var todate = '';
        var sportId = punter.getSelectedSport();

        var month, year;

        switch (punter.PeriodType) {
            case 0:
                month = $('#ddlMonth').val();
                year = $('#ddlYear').val();
                fromdate = month + '/01/' + year;
                todate = month + '/' + punter.daysInMonth(month, year) + '/' + year;
                break;
            case 1:
                year = $('#ddlYear').val();
                fromdate = '01' + '/01/' + year;
                todate = '31' + '/12/' + year;
                break;
            case 2:
                fromdate = '01' + '/' + $('#ddlfMonth').val() + '/' + $('#ddlfYear').val();
                todate = punter.daysInMonth($('#ddltMonth').val(), $('#ddltYear').val()) + '/' + $('#ddltMonth').val() + '/' + $('#ddltYear').val();
                break;
        }
        punter.param.FromDate = fromdate;
        punter.param.ToDate = todate;
        punter.param.SportId = sportId;
        punter.param.UserName = $('#txtUserName').val() == 'All' ? '' : $('#txtUserName').val();
        punter.param.OrderBy = $('#ddlOrderby').val();
        punter.param.OrderType = $('#ddlOrderType').val();
        punter.param.TopRow = $('#txtTopRows').val();
        punter.param.LeagueId = $('#ddlLeagues').val();
        punter.param.OrderByName = $('#ddlOrderby option:selected').text();
        punter.param.OrderTypeName = $('#ddlOrderType option:selected').text();
        punter.param.SportName = PunterAnalysisExportExcel.getSelectedSportsName();
        punter.param.LeagueName = $('#ddlLeagues option:selected').text();
    },

    onclickUserName: function () {
        var username = $('#txtUserName').val();
        if (username == 'All') {
            $('#txtUserName').val('');
        }
    },

    onblurUserName: function () {
        var username = $('#txtUserName').val();
        if (username.length == 0) {
            $('#txtUserName').val('All');
        }
    },

    loadSports: function () {
        $.ajax({
            type: "POST",
            url: window.rootUrl + "PunterAnalysis/SelSports",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: '',
            success: function (rs) {
                $('#ddlSports').empty();
                if (rs.length > 0) {
                    punter.renderSports(rs, 'ddlSportLeague');
                    punter.renderSports(rs, 'ddlSportUser');
                }
                else {
                    $('#ddlSportLeague').append('<select><option value="-0">None</option></select>');
                    $('#ddlSportUser').append('<select><option value="-0">None</option></select>');
                }
            }
        });
    },

    loadLeagueBySport: function () {
        if (punter.SearchBy == 0)
            return;

        $('#msg').empty();

        punter.SportSelected = punter.getSelectedSport();
        if (punter.SportSelected.length == 0) {
            showAlertMessage("msg", "Please choose at least one sport.", 1);
            $('#ddlLeagues').empty();
            return;
        }

        punter.setParam();
        var p = {
            param: punter.param
        };

        $.ajax({
            type: "POST",
            url: window.rootUrl + "PunterAnalysis/SelLeagueBySportId",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: JSON.stringify(p),
            success: function (rs) {
                $('#ddlLeagues').empty();
                $('#ddlLeagues').append('<option value="0">All</option>');
                for (var i = 0; i < rs.length; i++) {
                    $('#ddlLeagues').append('<option value="' + rs[i].Id + '">' + rs[i].Name + '</option>');
                }
            }
        });
    },

    renderSports: function (data, id) {
        var tempData = [];
        for (var i = 0; i < data.length; i++) {
            tempData.push({
                Id: "#" + id + data[i].Id.toString(),
                Value: data[i].Id,
                Text: data[i].Name,
                Checked: true,
                Enable: true,
                CssClass: "",
                Items: []
            });
        }
        $("#" + id).multiSelect(
        {
            minWidth: 140,
            maxHeight: 450,
            shadow: true,
            showHeader: true,
            isCheckAll: true,
            showFooter: false,
            listData: tempData
        });
    },

    generateReport: function () {

        try { parent.BlockUI(); } catch (e) { }

        punter.setParam();
        PunterAnalysisExportExcel.saveParamForExportExcel(false);

        var param = {
            param: punter.param
        };

        $('#tblbodyfrozenSummary').empty();
        $('#tblbodyscrollSummary').empty();
        $('#tblfooterfrozenSummary').empty();
        $('#tblfooterscrollSummary').empty();
        $('#msg').empty();

        $("#PunterByCustIdreport").addClass("hidden");
        $("#PunterBySporttypereport").addClass("hidden");

        var action = 'PunterAnalysis/SelTopPunterSummary';
        switch (punter.SearchBy) {
            case 0:
                action = 'PunterAnalysis/SelTopPunterSummary';
                break;
            case 1:
                action = 'PunterAnalysis/SelTopPunterSummaryByLeague';
                break;
        }

        $.ajax({
            type: "POST",
            url: window.rootUrl + action,
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: JSON.stringify(param),
            success: function (rs) {
                if (rs.Punters.length > 0) {
                    $("#puntergridSummary").removeClass("hidden");
                    $('#tblbodyfrozenSummary').append(punter.genBodyFrozenSummary(rs));
                    $('#tblbodyscrollSummary').append(punter.genBodyScrollSummary(rs));
                    $('#tblfooterfrozenSummary').append(punter.genFooterFrozen(rs));
                    $('#tblfooterscrollSummary').append(punter.genFooterScrollSummary(rs));

                    $("#puntergridSummary").reRenderScroller(518, 550, false); //width:500+6*1+6*2|height:562px

                }
                else {
                    $("#puntergridSummary").addClass("hidden");
                    showAlertMessage("msg", "There is no data.", 1);
                }
            },
            complete: function () {
                try { parent.jQuery.unblockUI(); } catch (e) { }
            }
        });
    },

    genBodyFrozenSummary: function (data) {
        var html = [];
        for (var i = 0; i < data.Punters.length; i++) {
            html.push('<tr><td><div class="c col100 link" onclick="punter.callSelTopPunterByCustID(' + data.Punters[i].Id, ',\'' + data.Punters[i].Name + '\'' + ')">' + data.Punters[i].Name + '</div></td></tr>');
        }
        return html.join('');
    },

    genBodyScrollSummary: function (data) {
        var html = [];
        for (var i = 0; i < data.Punters.length; i++) {
            html.push('<tr><td><div class="r col100">' + Helper.format(data.Punters[i].Winlost) + '</div></td>');
            html.push('<td><div class="r col100">' + data.Punters[i].Turnover + '</div></td>');
            html.push('<td><div class="r col100">' + Helper.format(data.Punters[i].Margin) + '</div></td>');
            html.push('<td><div class="r col100">' + data.Punters[i].Betcount + '</div></td>');
            html.push('<td><div class="c col100">view by ip</div></td></tr>');
        }
        return html.join('');
    },

    genFooterFrozen: function (data) {
        var html = [];
        if (data.Total != null) {
            html.push('<tr><td><div class="r col100">' + data.Total.Name + '</div></td></tr>');
        }

        return html.join('');
    },

    genFooterScrollSummary: function (data) {
        var html = [];
        if (data.Total != null) {
            html.push('<tr><td><div class="r col100">' + Helper.format(data.Total.Winlost) + '</td>');
            html.push('<td><div class="r col100">' + data.Total.Turnover + '</td>');
            html.push('<td><div class="r col100">' + Helper.format(data.Total.Margin) + '</td>');
            html.push('<td><div class="r col100">' + data.Total.Betcount + '</td>');
            html.push('<td><div class="r col100"></td>');
        }

        return html.join('');
    },

    renderEmptyRow: function (col) {
        var html = "";
        html = '<tr>\
                <td colspan="' + col + '">\
                    <div>&nbsp;</div>\
                </td>\
            </tr>';
        return html;
    },

    callSelTopPunterByCustID: function (custId, userName) {
        punter.param.CustId = custId;
        punter.param.CustName = userName;
        $('#spUserName').html(userName);
        $("#puntergridSummary").addClass("hidden");
        punterByCustId.SelTopPunterByCustId();
    }
};


var punterByCustId = {
    SelTopPunterByCustId: function () {
        try { parent.BlockUI(); } catch (e) { }

        punter.setParam();
        PunterAnalysisExportExcel.saveParamForExportExcel(false);

        var param = {
            param: punter.param
        };

        $('#tblbodyfrozenByCustId').empty();
        $('#tblbodyscrollByCustId').empty();
        $('#tblfooterfrozenByCustId').empty();
        $('#tblfooterscrollByCustId').empty();
        $('#msg').empty();

        var action = 'PunterAnalysis/SelTopPunterByCustID';

        switch (punter.SearchBy) {
            case 0: //User
                action = 'PunterAnalysis/SelTopPunterByCustID';
                $('#spFrozenTitleHeader').html('Sport Type');
                break;
            case 1: //League
                action = 'PunterAnalysis/SelTopPunterByLeagueCustID';
                $('#spFrozenTitleHeader').html('Bet Type');
                break;
        }

        $.ajax({
            type: "POST",
            url: window.rootUrl + action,
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: JSON.stringify(param),
            success: function (rs) {
                if (rs.Punters.length > 0) {
                    $("#PunterByCustIdreport").removeClass("hidden");
                    $('#tblbodyfrozenByCustId').append(punterByCustId.genBodyFrozenSummary(rs));
                    $('#tblbodyscrollByCustId').append(punterByCustId.genBodyScrollSummary(rs));
                    $('#tblfooterfrozenByCustId').append(punterByCustId.genFooterFrozen(rs));
                    $('#tblfooterscrollByCustId').append(punterByCustId.genFooterScrollSummary(rs));

                    $("#puntergridByCustId").reRenderScroller(418, 550, false); //width:500+6*1+6*2|height:562px
                }
                else {

                }
            },
            complete: function () {
                try { parent.jQuery.unblockUI(); } catch (e) { }
            }
        });
    },

    genBodyFrozenSummary: function (data) {
        var html = [];
        for (var i = 0; i < data.Punters.length; i++) {
            html.push('<tr><td><div class="c col100 link" onclick="punterByCustId.callSelTopPunterBySporttype(' + data.Punters[i].Id, ',\'' + data.Punters[i].Name + '\'' + ')">' + data.Punters[i].Name + '</div></td></tr>');
        }
        return html.join('');
    },

    genBodyScrollSummary: function (data) {
        var html = [];
        for (var i = 0; i < data.Punters.length; i++) {
            html.push('<tr><td><div class="r col100">' + Helper.format(data.Punters[i].Winlost) + '</div></td>');
            html.push('<td><div class="r col100">' + data.Punters[i].Turnover + '</div></td>');
            html.push('<td><div class="r col100">' + Helper.format(data.Punters[i].Margin) + '</div></td>');
            html.push('<td><div class="r col100">' + data.Punters[i].Betcount + '</div></td></tr>');
        }
        return html.join('');
    },

    genFooterFrozen: function (data) {
        var html = [];
        if (data.Total != null) {
            html.push('<tr><td><div class="r col100">' + data.Total.Name + '</div></td></tr>');
        }

        return html.join('');
    },

    genFooterScrollSummary: function (data) {
        var html = [];
        if (data.Total != null) {
            html.push('<tr><td><div class="r col100">' + Helper.format(data.Total.Winlost) + '</td>');
            html.push('<td><div class="r col100">' + data.Total.Turnover + '</td>');
            html.push('<td><div class="r col100">' + Helper.format(data.Total.Margin) + '</td>');
            html.push('<td><div class="r col100">' + data.Total.Betcount + '</td></tr>');
        }

        return html.join('');
    },

    callBackSelTopPunterSummary: function () {
        $("#puntergridSummary").removeClass("hidden");
        $("#PunterByCustIdreport").addClass("hidden");
        $("#puntergridBySporttype").addClass("hidden");
        $('#PunterBySporttypereport').addClass("hidden");
        $('#PunterByBettypereport').addClass("hidden");

    },

    callSelTopPunterBySporttype: function (sportId, sportName) {

        $("#PunterByCustIdreport").addClass("hidden");

        var userName = $('#spUserName').html();
        var title = '<span class="link" onclick="punterByCustId.callBackSelTopPunterSummary()">Main</span> >';
        title += '<span class="link" onclick="punterBySporttype.callBackSelTopPunterByCustId()">' + userName + '</span> > <span>' + sportName + '</span>';
        $('#spUserName_Sport').html(title);

        PunterAnalysisExportExcel.SportTypeId = sportId;
        PunterAnalysisExportExcel.SportTypeName = sportName;
        punterBySporttype.SelTopPunterBySporttype(sportId);
    }
};

var punterBySporttype = {
    SelTopPunterBySporttype: function (sportId) {
        try { parent.BlockUI(); } catch (e) { }

        punter.setParam();
        PunterAnalysisExportExcel.saveParamForExportExcel(true);
        punter.param.SportId = sportId;

        var param = {
            param: punter.param
        };

        $("#puntergridBySporttype").removeClass("hidden");

        $('#tblbodyfrozenBySporttype').empty();
        $('#tblbodyscrollBySporttype').empty();
        $('#tblfooterfrozenBySporttype').empty();
        $('#tblfooterscrollBySporttype').empty();
        $('#msg').empty();

        var action = 'PunterAnalysis/SelTopPunterBySporttype';

        $.ajax({
            type: "POST",
            url: window.rootUrl + action,
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: JSON.stringify(param),
            success: function (rs) {
                if (rs.Punters.length > 0) {
                    $("#PunterBySporttypereport").removeClass("hidden");
                    $('#tblbodyfrozenBySporttype').append(punterBySporttype.genBodyFrozenSummary(rs));
                    $('#tblbodyscrollBySporttype').append(punterBySporttype.genBodyScrollSummary(rs));
                    $('#tblfooterfrozenBySporttype').append(punterBySporttype.genFooterFrozen(rs));
                    $('#tblfooterscrollBySporttype').append(punterBySporttype.genFooterScrollSummary(rs));

                    $("#puntergridBySporttype").reRenderScroller(784, 550, false); //width:500+6*1+6*2|height:562px
                }
                else {

                }
            },
            complete: function () {
                try { parent.jQuery.unblockUI(); } catch (e) { }
            }
        });
    },

    genBodyFrozenSummary: function (rs) {
        var html = [];
        var data = rs.Punters.length > 0 ? rs.Punters[0] : [];

        for (var i = 0; i < data.Punters.length; i++) {
            html.push('<tr><td><div class="c col100" >' + data.Punters[i].Name + '</div></td></tr>');
        }
        return html.join('');
    },

    genBodyScrollSummary: function (rs) {
        var html = [];

        var cnt = rs.Punters[0].Punters.length;
        for (var i = 0; i < cnt; i++) {//Rows
            html.push('<tr>');

            for (var j = 0; j < rs.Punters.length; j++) {//Tables
                var data = rs.Punters[j].Punters[i];

                html.push('<td><div class="r col100 link" onclick="punterBySporttype.callSelTopPunterByBettype(' + rs.Punters[j].Name + "," + data.Id + "," + data.SportType.Id + ')">' + (data.HasData == 1 ? Helper.format(data.Winlost) : '') + '</div></td>');
                html.push('<td><div class="r col100">' + (data.HasData == 1 ? data.Turnover : '') + '</div></td>');
                html.push('<td><div class="r col100">' + (data.HasData == 1 ? Helper.format(data.Margin) : '') + '</div></td>');
                html.push('<td><div class="r col100 link">' + (data.HasData == 1 ? data.Betcount : '') + '</div></td>');
            }

            html.push('</tr>');
        }

        return html.join('');
    },

    genFooterFrozen: function (rs) {
        var html = [];

        var cnt = rs.Punters[0].Punters.length;
        if (cnt > 0) {
            html.push('<tr><td><div class="r col100">Total</div></td></tr>');
        }

        return html.join('');
    },

    genFooterScrollSummary: function (rs) {
        var html = [];


        html.push('<tr>');

        for (var j = 0; j < rs.Punters.length; j++) {//Tables
            var data = rs.Punters[j].Total;

            html.push('<td><div class="r col100">' + Helper.format(data.Winlost) + '</div></td>');
            html.push('<td><div class="r col100">' + data.Turnover + '</div></td>');
            html.push('<td><div class="r col100">' + Helper.format(data.Margin) + '</div></td>');
            html.push('<td><div class="r col100">' + data.Betcount + '</div></td>');
        }

        html.push('</tr>');


        return html.join('');
    },

    callBackSelTopPunterByCustId: function () {
        $("#PunterByCustIdreport").removeClass("hidden");
        $("#PunterBySporttypereport").addClass("hidden");
        $('#PunterByBettypereport').addClass("hidden");
    },

    callSelTopPunterByBettype: function (liveindicator, bettype, sporttype) {
        $('#spUserName_Bettype').html($('#spUserName_Sport').html());
        punterByBettype.SelTopPunterByBettype(liveindicator, bettype, sporttype);

        $('#PunterBySporttypereport').addClass("hidden");
        $('#PunterByBettypereport').removeClass("hidden");
    }
};

var punterByBettype = {
    SelTopPunterByBettype: function (liveindicator, bettype, sporttype) {

        punter.setParam();

        var param = {
            param: punter.param
        };

        punter.param.liveindicator = liveindicator;
        punter.param.BettypeId = bettype;
        punter.param.SportId = sporttype;

        try { parent.BlockUI(); } catch (e) { }

        var action = 'PunterAnalysis/SelTopPunterByBettype';

        $('#tblheaderfrozenByBettype').empty();
        $('#tblbodyfrozenByBettype').empty();
        $('#tblbodyscrollByBettype').empty();
        $('#tblfooterfrozenByBettype').empty();
        $('#tblfooterscrollByBettype').empty();
        $('#msg').empty();

        $.ajax({
            type: "POST",
            url: window.rootUrl + action,
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: JSON.stringify(param),
            success: function (rs) {
                if (rs.Punters.length > 0) {
                    var cnt = rs.Punters.length;            
                    $("#PunterByBettypereport").removeClass("hidden");
                    $('#tblheaderfrozenByBettype').append(punterByBettype.genHeaderFrozenSummary(rs));

                    $('#tblbodyfrozenByBettype').append(punterByBettype.genBodyFrozenSummary(rs));
                    $('#tblbodyscrollByBettype').append(punterByBettype.genBodyScrollSummary(rs));
                    $('#tblfooterfrozenByBettype').append(punterByBettype.genFooterFrozen(rs));
                    $('#tblfooterscrollByBettype').append(punterByBettype.genFooterScrollSummary(rs));

                    $("#puntergridByBettype").reRenderScroller(784, 550, false); //width:500+6*1+6*2|height:562px
                }
                else {

                }
            },
            complete: function () {
                try { parent.jQuery.unblockUI(); } catch (e) { }
            }
        });
    },

    genHeaderFrozenSummary: function (data) {
        var html = [];
        var cnt = data.Punters.length;

        //row1
        html.push("<tr>");
        for (var i = 0; i < cnt; i++) {
            var name = data.Punters[i].Name.split('|');
            var year = name[0];
            var month = name[1];

            html.push("<td class='c' colspan='4'>" + (i == 0 ? "Year " + year : punter.arrMonths[i - 1]) + "</rd>");
        }
        html.push("</tr>");

        //row2
        html.push("<tr>");
        for (var i = 0; i < cnt; i++) {
            html.push("<td><div class='col100 c'>Winlost</div></td>");
            html.push("<td><div class='col100 c'>Turnover</div></td>");
            html.push("<td><div class='col100 c'>Margin</div></td>");
            html.push("<td><div class='col100 c'>Bet Cnt</div></td>");
        }
        html.push("</tr>");

        return html.join('');
    },

    genBodyFrozenSummary: function (rs) {
        var html = [];
        var data = rs.Punters.length > 0 ? rs.Punters[0] : [];

        for (var i = 0; i < data.Punters.length; i++) {
            html.push('<tr><td><div class="c col260" >' + data.Punters[i].Name + '</div></td></tr>');
        }
        return html.join('');
    },

    genBodyScrollSummary: function (rs) {
        var html = [];

        var cnt = rs.Punters[0].Punters.length;
        for (var i = 0; i < cnt; i++) {//Rows
            html.push('<tr>');

            for (var j = 0; j < rs.Punters.length; j++) {//Tables
                var data = rs.Punters[j].Punters[i];

                html.push('<td><div class="r col100 link">' + (data.HasData == 1 ? Helper.format(data.Winlost) : '') + '</div></td>');
                html.push('<td><div class="r col100">' + (data.HasData == 1 ? data.Turnover : '') + '</div></td>');
                html.push('<td><div class="r col100">' + (data.HasData == 1 ? Helper.format(data.Margin) : '') + '</div></td>');
                html.push('<td><div class="r col100 link">' + (data.HasData == 1 ? data.Betcount : '') + '</div></td>');
            }

            html.push('</tr>');
        }

        return html.join('');
    },

    genFooterFrozen: function (rs) {
        var html = [];

        var cnt = rs.Punters[0].Punters.length;
        if (cnt > 0) {
            html.push('<tr><td><div class="r col260">Total</div></td></tr>');
        }

        return html.join('');
    },

    genFooterScrollSummary: function (rs) {
        var html = [];


        html.push('<tr>');

        for (var j = 0; j < rs.Punters.length; j++) {//Tables
            var data = rs.Punters[j].Total;

            html.push('<td><div class="r col100">' + Helper.format(data.Winlost) + '</div></td>');
            html.push('<td><div class="r col100">' + data.Turnover + '</div></td>');
            html.push('<td><div class="r col100">' + Helper.format(data.Margin) + '</div></td>');
            html.push('<td><div class="r col100">' + data.Betcount + '</div></td>');
        }

        html.push('</tr>');


        return html.join('');
    },
};

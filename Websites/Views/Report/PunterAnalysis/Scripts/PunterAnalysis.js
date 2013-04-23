
$(document).ready(function () {
    var puntergrid = $("#puntergridSummary");

    var puntergridByCustId = $("#puntergridByCustId");

    var puntergridBySporttype = $("#puntergridBySporttype");

    var puntergridByBettype = $("#puntergridByBettype");

    punter.setSearchBy(0);
    punter.setPeriodType(0);
    punter.loadSports();

    punter.SportSelected = punter.getSelectedSport();

});




$(document).delegate('html', 'click', function (e, isInsideCall) {
    var $target = $(e.target);
    if (!$target.closest('div.ui-multiselect-options').length && !$target.parent().hasClass('ui-multiselect')) {

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

var cache = {
    SelTopPunterSummary: [],
    SelTopPunterByCustID: [],
    SelTopPunterBySporttype: []
};

var punter = {
    SearchBy: 0, // 0:User, 1:League
    PeriodType: 0, // 0:Monthly, 1:Annual, 2:Long Range
    arrMonths: ['January', 'February', 'March', 'April', 'May', 'June', 'July', 'August', 'September', 'October', 'November', 'December'],
    beginYear: 2007,
    SportSelected: '',
    ReportType: '',
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
        LiveIndicator: '',
        PeriodType: '',
        SearchBy: ''
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

        $('#msg').empty();
        switch (type) {
            case 0: // Search by User
                $('#dvUserName').show();
                $('#dvSportByLeague').hide();
                $('#spReport1').show();
                $('#spReport2').hide();
                $('#dvSport').show();
                break;
            case 1: //Search By League
                $('#dvUserName').hide();
                $('#dvSportByLeague').show();
                $('#spReport1').hide();
                $('#spReport2').show();
                $('#dvSport').hide();
                punter.loadLeagueBySport();
                break;
        }
    },

    setPeriodType: function (type) {
        punter.PeriodType = type;
        punter.param.PeriodType = type;

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
        var id = 'ddlSportUser';

        var selectedList = $('#' + id).multiSelect('getCheckedItems');
        var result = "";

        // result += $('#ddlSportUser1').is(':checked') ? '1,' : '';

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
        var sportId = '';

        switch (punter.SearchBy) {
            case 0: //User
                sportId = punter.getSelectedSport();
                sportName = PunterAnalysisExportExcel.getSelectedSportsName(punter.SearchBy);
                punter.param.UserName = $('#txtUserName').val() == 'All' ? '' : $('#txtUserName').val();
                break;
            case 1: // League
                sportId = $('#ddlSportLeague').val();
                sportName = $('#ddlSportLeague').find('option:selected').text();
                punter.param.UserName = '';
                break;
        }

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
                todate = '12' + '/31/' + year;
                break;
            case 2:
                fromdate = $('#ddlfMonth').val() + '/01' + '/' + $('#ddlfYear').val();
                todate = $('#ddltMonth').val() + '/' + punter.daysInMonth($('#ddltMonth').val(), $('#ddltYear').val()) + '/' + $('#ddltYear').val();
                break;
        }
        punter.param.FromDate = fromdate;
        punter.param.ToDate = todate;
        punter.param.SportId = sportId;
        punter.param.OrderBy = $('#ddlOrderby').val();
        punter.param.OrderType = $('#ddlOrderType').val();
        punter.param.TopRow = $('#txtTopRows').val();
        punter.param.LeagueId = $('#ddlLeagues').val();
        punter.param.OrderByName = $('#ddlOrderby option:selected').text();
        punter.param.OrderTypeName = $('#ddlOrderType option:selected').text();
        punter.param.SportName = sportName;
        punter.param.LeagueName = $('#ddlLeagues option:selected').text();
        punter.param.SearchBy = punter.SearchBy;
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
            url: window.rootUrl + "PunterAnalysis/SelSportsGroup",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: '',
            success: function (rs) {
                if (rs.GroupName.length > 0) {
                    punter.renderSportSingleSelected(rs);
                    punter.renderSportMultiSelected(rs.GroupName, 'ddlSportUser');
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

        punter.param.SportId = $('#ddlSportLeague').val();

        if (punter.param.SportId.length == 0) {
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
                if (rs.length > 0) {
                    for (var i = 0; i < rs.length; i++) {
                        $('#ddlLeagues').append('<option value="' + rs[i].Id + '">' + rs[i].Name + '</option>');
                    }
                }
                else {
                    $('#ddlLeagues').append('<option value="-100000">There is no information available.</option>');
                }
            }
        });
    },

    renderSportMultiSelected: function (data, id) {
        var tempData = [];
        for (var i = 0; i < data.length; i++) {
            var tempLeagues = [];
            for (var j = 0; j < data[i].GroupSports.length; j++) {
                tempLeagues.push({
                    Id: "#" + id + "Detail" + data[i].GroupSports[j].Id.toString(),
                    Value: data[i].GroupSports[j].Id,
                    Text: data[i].GroupSports[j].Name,
                    Checked: true,
                    Enable: true,
                    CssClass: ""
                });
            }
            tempData.push({
                Id: id + data[i].Id.toString(),
                Value: data[i].Id,
                Text: data[i].Name,
                Checked: true,
                Enable: true,
                CssClass: "",
                Items: tempLeagues
            });
        }

        $("#" + id).multiSelect(
        {
            minWidth: 190,
            maxHeight: 450,
            shadow: true,
            showHeader: true,
            isCheckAll: true,
            showFooter: false,
            checkAllText: 'All',
            listData: tempData,
            onCheck: function () {
                if (this.value != 1 && this.value != 999 && this.value != 'on' &&
                        this.checked == true) {
                    $('#ddlSportUser999').attr('checked', true);
                }
            }
        });
    },

    renderSportSingleSelected: function (data) {
        var html = [];
        html.push('<select id="ddlSportLeague" onchange="punter.loadLeagueBySport();">');

        if (data.GroupName.length > 1) {
            html.push('<option value=' + data.GroupName[0].Id + '>' + data.GroupName[0].Name + '</option>');
        }

        for (var i = 0; i < data.GroupName[1].GroupSports.length; i++) {
            html.push('<option value=' + data.GroupName[1].GroupSports[i].Id + '>' + data.GroupName[1].GroupSports[i].Name + '</option>');
        }
        html.push('</select>');
        $('#spSportLeague').html(html.join(''));
    },



    renderReport: function (rs) {
        var mainreport = $("#mainreport");

        mainreport.removeClass("hidden");
        mainreport.resetGrid("grid1");
        $("#grid1 tbody").empty().append(punter.genBodyScrollSummary(rs));
        mainreport.find(".row_footer_content thead").empty().append('<tr><th><div style="height:30px; width:100px;" class="r table-cell">' + rs.Total.Name + '</div></th></tr>');
        mainreport.find(".col_footer_content thead").empty().append(punter.genFooterScrollSummary(rs.Total));

        if (punter.SearchBy == 1) {
            $(".IPHeader").addClass("hidden");
        }
        else {
            $(".IPHeader").removeClass("hidden");
        }
        var grid1Width = 630;
        if (punter.SearchBy == 1) {
            grid1Width = 525;
        }

        var gridOpts = {
            tblContentId: 'grid1',
            manualCopyHeader: false,
            fullWidth: true,
            fullHeight: true,
            gridHeight: 136,
            gridWidth: grid1Width,
            freezeWidth: 100,
            numberFreezeCol: 1
        };
        mainreport.gridScroller(gridOpts);

    },

    generateReport: function () {
        try { parent.BlockUI(); } catch (e) { }
        punter.setParam();

        if (punter.param.UserName != '')
            punter.param.CustId = 0;

        PunterAnalysisExportExcel.saveParamForExportExcel(false);
        PunterAnalysisExportExcel.BetCountType = 0;

        var param = {
            param: punter.param
        };


        $('#msg').empty();

        $("#PunterByCustIdreport").addClass("hidden");
        $("#PunterBySporttypereport").addClass("hidden");
        $('#PunterByBettypereport').addClass("hidden");
        $('#PunterSelIPByCustomerreport').addClass("hidden");
        $('#PunterSelBetCountByCustomerreport').addClass("hidden");
        $('#SummaryLongRange').addClass("hidden");


        $('#gridtitleSummary').html("Main - Punter Analysis");

        var action = 'PunterAnalysis/SelTopPunterSummary';
        switch (punter.SearchBy) {
            case 0: //User
                if (punter.param.UserName == '') {
                    action = 'PunterAnalysis/SelTopPunterSummary';
                }
                else {
                    action = 'PunterAnalysis/SelTopPunterByCustID';
                    PunterAnalysisExportExcel.CustName = punter.param.UserName;
                    punter.param.CustName = punter.param.UserName;
                    $('#gridtitleByCustId').html("Main - Punter Analysis");
                }
                break;
            case 1: //League
                punter.param.UserName = '';
                punter.param.CustName = '';
                PunterAnalysisExportExcel.UserName = "";
                PunterAnalysisExportExcel.CustName = "";
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
                if (punter.param.UserName == '') {
                    cache.SelTopPunterSummary = rs;
                    punter.renderReport(rs);
                }
                else {
                    cache.SelTopPunterByCustID = rs;
                    punterByCustId.renderReport(rs);
                }
            },
            complete: function () {
                try { parent.jQuery.unblockUI(); } catch (e) { }
            }
        });
    },

    genBodyScrollSummary: function (data) {
        var html = [];
        var cnt = data.Punters.length;
        if (cnt > 0) {
            for (var i = 0; i < cnt; i++) {
                html.push('<tr style="height: 17px;"><td style="height: 17px;"><div class="l link" onclick="punter.callSelTopPunterByCustID(' + data.Punters[i].Id, ',\'' + data.Punters[i].Name + '\'' + ')">' + data.Punters[i].Name + '</div></td>')
                html.push('<td><div class="r">' + Helper.format(data.Punters[i].Winlost) + '</div></td>');
                html.push('<td><div class="r">' + data.Punters[i].Turnover + '</div></td>');
                html.push('<td><div class="r">' + Helper.format(data.Punters[i].Margin) + '</div></td>');
                //SelBetCountByCustomer: function (userName, custId, type, IsDetail, bettype, bettypeName, liveindicator, sportId, sportName)
                html.push('<td><div class="r link" onclick="punterSelBetCountByCustomer.SelBetCountByCustomer(\'' + data.Punters[i].Name + '\',' + data.Punters[i].Id + ',\'SelTopPunterSummary\',' + false + ',' + null + ',' + null + ',' + null + ',' + data.Punters[i].SportType.Id + ',\'' + data.Punters[i].SportType.Name + '\')">' + data.Punters[i].Betcount + '</div></td>');
                if (punter.SearchBy == 0) {
                    html.push('<td><div class="c link" onclick="punterSelIPByCustomer.SelTopPunterByCustId(\'' + data.Punters[i].Name + '\',' + data.Punters[i].Id + ')">view by ip</div></td></tr>');
                }
            }
        }
        else {
            html.push('<tr><td class="c" style="height:17px;">&nbsp;</td><td class="c" style="height:17px;" colspan="5">There is no information available.</td></tr>');
        }
        return html.join('');
    },

    genFooterScrollSummary: function (total) {
        var html = [];
        if (total != null) {
            html.push('<tr><th><div style="width:100px;height:30px;" class="table-cell">&nbsp;</div></th>');
            html.push('<th><div style="width:100px;height:30px;" class="r table-cell">' + (total.HasData != 0 ? Helper.format(total.Winlost) : '') + '</div></th>');
            html.push('<th><div style="width:100px;height:30px;" class="r table-cell">' + (total.HasData != 0 ? total.Turnover : '') + '</div></th>');
            html.push('<th><div style="width:100px;height:30px;" class="r table-cell">' + (total.HasData != 0 ? Helper.format(total.Margin) : '') + '</div></th>');
            html.push('<th><div style="width:100px;height:30px;" class="r table-cell">' + (total.HasData != 0 ? total.Betcount : '') + '</div></th>');
            if (punter.SearchBy == 0) {
                html.push('<th><div style="width:100px;height:30px;" class="r table-cell" >&nbsp;</div></th></tr>');
            }
        }
        return html.join('');
    },

    renderEmptyRow: function (col, width) {
        var html = "";
        html += '<tr>';
        html += '<td class="c" colspan="' + col + '">';
        html += '<div style="width:' + width + 'px">There is no information available.</div>';
        html += '</td>';
        html += '</tr>';
        return html;
    },

    callSelTopPunterByCustID: function (custId, userName) {
        punter.param.CustId = custId;
        punter.param.CustName = userName;
        punterByCustId.SelTopPunterByCustId();
        $('#SummaryLongRange').addClass('hidden');


        var title = '<span id="spMain" class="link" onclick="punterByCustId.callBackSelTopPunterSummary();">Main</span><span> > </span>';
        title += '<span id="spUserName">' + userName + '</span>';
        $('#gridtitleByCustId').html(title);
    }

};


var punterByCustId = {

    renderReport: function (rs) {
        var mainreport = $("#mainreport");
        mainreport.addClass("hidden");
        var PunterByCustIdreport = $("#PunterByCustIdreport");
        PunterByCustIdreport.resetGrid("grid2");

        $("#grid2 tbody").empty().append(punterByCustId.genBodyScrollSummary(rs));
        PunterByCustIdreport.find(".row_footer_content thead").empty().append('<tr><th><div style="height:30px; width:100px;" class="r table-cell">' + rs.Total.Name + '</div></th></tr>');
        PunterByCustIdreport.find(".col_footer_content thead").empty().append(punterByCustId.genFooterScrollSummary(rs.Total));
        var gridOpts = {
            tblContentId: 'grid2',
            manualCopyHeader: false,
            fullWidth: true,
            fullHeight: true,
            gridHeight: 136,
            freezeWidth: 100,
            numberFreezeCol: 1
        };

        PunterByCustIdreport.removeClass("hidden");
        PunterByCustIdreport.gridScroller(gridOpts);

    },

    SelTopPunterByCustId: function () {
        try { parent.BlockUI(); } catch (e) { }

        punter.setParam();
        PunterAnalysisExportExcel.saveParamForExportExcel(false);
        PunterAnalysisExportExcel.BetCountType = 1;

        var param = {
            param: punter.param
        };

        $('#msg').empty();

        var action = 'PunterAnalysis/SelTopPunterByCustID';

        switch (punter.SearchBy) {
            case 0: //User
                action = 'PunterAnalysis/SelTopPunterByCustID';
                break;
            case 1: //League
                action = 'PunterAnalysis/SelTopPunterBySporttypeSum';
                break;
        }

        $.ajax({
            type: "POST",
            url: window.rootUrl + action,
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: JSON.stringify(param),
            success: function (rs) {
                cache.SelTopPunterByCustID = rs;
                punterByCustId.renderReport(rs);
            },
            complete: function () {
                try { parent.jQuery.unblockUI(); } catch (e) { }
            }
        });
    },

    genBodyScrollSummary: function (data) {
        var html = [];
        var cnt = data.Punters.length;
        var userName = punter.param.CustName;
        var custId = 0;
        if (cnt > 0) {
            for (var i = 0; i < cnt; i++) {
                if (data.Punters[i].Id == 100) {
                    html.push('<tr style="height: 17px;"><td style="height: 17px;"><div class="l">' + data.Punters[i].Name + '</div></td>');
                } else {
                    html.push('<tr style="height: 17px;"><td style="height: 17px;"><div class="l link" onclick="punterByCustId.callSelTopPunterBySporttype(' + data.Punters[i].Id, ',\'' + data.Punters[i].Name + '\',' + data.Punters[i].CustId + ')">' + data.Punters[i].Name + '</div></td>');
                }
                html.push('<td><div class="r">' + Helper.format(data.Punters[i].Winlost) + '</div></td>');
                html.push('<td><div class="r">' + data.Punters[i].Turnover + '</div></td>');
                html.push('<td><div class="r">' + Helper.format(data.Punters[i].Margin) + '</div></td>');
                if (data.Punters[i].Id == 100) {
                    html.push('<td><div class="r">' + data.Punters[i].Betcount + '</div></td>');

                }
                else {
                    //SelBetCountByCustomer: function (userName, custId, type, IsDetail, bettype, bettypeName, liveindicator, sportId)
                    html.push('<td><div class="r link" onclick="punterSelBetCountByCustomer.SelBetCountByCustomer(\'' + userName + '\',' + data.Punters[i].CustId + ',\'SelTopPunterByCustId\',' + false + ',' + null + ',' + null + ',' + null + ',' + data.Punters[i].Id + ',\'' + data.Punters[i].Name + '\')">' + data.Punters[i].Betcount + '</div></td>');
                }
            }
        }
        else {
            html.push('<tr><td class="c" style="height:17px;">&nbsp;</td><td class="c" style="height:17px;" colspan="4">There is no information available.</td></tr>');
        }
        return html.join('');
    },

    genFooterScrollSummary: function (total) {
        var html = [];
        if (total != null) {
            html.push('<tr><th ><div style="width:100px;height:30px;" class="table-cell">&nbsp;</div></th>');
            html.push('<th ><div style="width:100px;height:30px" class="r table-cell">' + Helper.format(total.Winlost) + '</th>');
            html.push('<th><div style="width:100px;height:30px" class="r table-cell">' + total.Turnover + '</th>');
            html.push('<th><div style="width:100px;height:30px" class="r table-cell">' + Helper.format(total.Margin) + '</th>');
            html.push('<th><div style="width:100px;height:30px" class="r table-cell">' + total.Betcount + '</th></tr>');
        }
        return html.join('');
    },

    callBackSelTopPunterSummary: function () {
        $("#mainreport").removeClass("hidden");
        $("#PunterByCustIdreport").addClass("hidden");
        $('#PunterBySporttypereport').addClass("hidden");
        $('#PunterByBettypereport').addClass("hidden");
        $('#PunterSelIPByCustomerreport').addClass("hidden");
        $('#PunterSelBetCountByCustomerreport').addClass("hidden");
        $('#SummaryLongRange').addClass("hidden");

        PunterAnalysisExportExcel.BetCountType = 0;


        var rs = '';
        if (punter.param.UserName == '') {
            rs = cache.SelTopPunterSummary;
            punter.renderReport(rs);
        }
        else {
            rs = cache.SelTopPunterByCustID;
            punterByCustId.renderReport(rs);
        }
    },

    callSelTopPunterBySporttype: function (sportId, sportName, custId) {

        $("#PunterByCustIdreport").addClass("hidden");

        var userName = $('#spUserName').html();
        var title = '';
        if (punter.param.UserName != '') {
            title = '<span id="spMain" class="link" onclick="punterBySporttype.callBackSelTopPunterByCustId()">Main</span> >';
            title += '<span id="spSportName_custId">' + sportName + '</span>';
        }
        else {
            title = '<span id="spMain" class="link" onclick="punterByCustId.callBackSelTopPunterSummary()">Main</span> >';
            title += '<span id="spUserName" class="link" onclick="punterBySporttype.callBackSelTopPunterByCustId()">' + userName + '</span> > ';
            title += '<span id="spSportName_custId">' + sportName + '</span>';
        }
        $('#gridtitleBySporttype').html(title);

        PunterAnalysisExportExcel.SportTypeId = sportId;
        PunterAnalysisExportExcel.SportTypeName = sportName;

        PunterAnalysisExportExcel.SportIdBetCount = sportId;
        PunterAnalysisExportExcel.SportNameBetCount = sportName;

        punterBySporttype.SelTopPunterBySporttype(sportId, custId);
    }
};

var punterBySporttype = {

    renderReport: function (rs) {
        var PunterByCustIdreport = $("#PunterByCustIdreport");
        PunterByCustIdreport.addClass("hidden");

        var PunterBySporttypereport = $("#PunterBySporttypereport");
        PunterBySporttypereport.resetGrid("grid3");

        $("#grid3 tbody").empty().append(punterBySporttype.genBodyScrollSummary(rs));
        $("#grid3rowheader tbody").empty().append(punterBySporttype.gen1stColBodyScroll(rs));
        PunterBySporttypereport.find(".row_footer_content thead").empty().append('<tr><th ><div style="height:30px; width:100px;" class="r table-cell">' + rs.Total.Name + '</div></th></tr>');
        PunterBySporttypereport.find(".col_footer_content thead").empty().append(punterBySporttype.genFooterScrollSummary(rs.Total));
        var gridOpts = {
            tblContentId: 'grid3',
            manualCopyHeader: true,
            fullWidth: true,
            fullHeight: true,
            gridHeight: 136,
            freezeWidth: 100,
            numberFreezeCol: 1
        };

        PunterBySporttypereport.removeClass("hidden");
        PunterBySporttypereport.gridScroller(gridOpts);


    },

    SelTopPunterBySporttype: function (sportId, custId) {
        try { parent.BlockUI(); } catch (e) { }

        punter.setParam();
        PunterAnalysisExportExcel.saveParamForExportExcel(true);
        PunterAnalysisExportExcel.BetCountType = 2;
        punter.param.SportId = sportId;
        punter.param.CustId = custId;

        var param = {
            param: punter.param
        };

        $("#puntergridBySporttype").removeClass("hidden");

        $('#msg').empty();

        var action = 'PunterAnalysis/SelTopPunterBySporttype';

        $.ajax({
            type: "POST",
            url: window.rootUrl + action,
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: JSON.stringify(param),
            success: function (rs) {
                cache.SelTopPunterBySporttype = rs;
                punterBySporttype.renderReport(rs);
            },
            complete: function () {
                try { parent.jQuery.unblockUI(); } catch (e) { }
            }
        });
    },

    genBodyScrollSummary: function (rs) {
        var html = [];

        var userName = punter.param.CustName;
        var custId = punter.param.CustId;
        var cnt = rs.Punters.length;
        if (cnt > 0) {
            for (var i = 0; i < cnt; i++) {//Rows
                html.push('<tr>');
                html.push('<td ><div style="height:17px; " class="r" >' + rs.Punters[i].Name + '</div></td>');
                for (var j = 0; j < rs.Punters[i].Columns.length; j++) {//Tables
                    var data = rs.Punters[i].Columns[j];
                    var onlick = "onclick=\"punterBySporttype.callSelTopPunterByBettype(" + j + "," + rs.Punters[i].Id + "," + rs.Punters[i].SportType.Id + ",'" + rs.Punters[i].Name + "');\"";
                    html.push('<td><div class="r link" ' + onlick + '>' + (data.HasData == 1 ? Helper.format(data.Winlost) : '') + '</div></td>');
                    html.push('<td><div class="r ">' + (data.HasData == 1 ? data.Turnover : '') + '</div></td>');
                    html.push('<td><div class="r">' + (data.HasData == 1 ? Helper.format(data.Margin) : '') + '</div></td>');
                    //SelBetCountByCustomer: function (userName, custId, type, IsDetail, bettype, bettypeName, liveindicator, sportId)
                    html.push('<td><div class="r link" onclick="punterSelBetCountByCustomer.SelBetCountByCustomer(\'' + userName + '\',' + custId + ',\'SelTopPunterBySporttype\',' + true + ',' + rs.Punters[i].Id + ',\'' + rs.Punters[i].Name + '\',' + j + ',' + punter.param.SportId + ')">' + (data.HasData == 1 ? data.Betcount : '') + '</div></td>');
                }

                html.push('</tr>');
            }
        }
        else {
            html.push('<tr><td class="c" style="height:17px;" colspan="13">There is no information available.</td></tr>');
        }
        return html.join('');
    },

    gen1stColBodyScroll: function (rs) {
        var html = [];
        var cnt = rs.Punters.length;

        if (cnt > 0) {
            for (var i = 0; i < cnt; i++) {//Rows
                html.push('<tr>');
                html.push('<td ><div style="width:100px;height:17px;" class="l" >' + rs.Punters[i].Name + '</div></td>');
                html.push('</tr>');
            }
        }
        else {
            html.push('<tr>');
            html.push('<td ><div style="width:100px;height:17px;" class="c" >&nbsp;</div></td>');
            html.push('</tr>');
        }
        return html.join('');
    },

    genFooterScrollSummary: function (total) {
        var html = [];

        html.push('<tr>');
        html.push('<th ><div style="width:100px; height:30px;" class="c table-cell" >' + total.Name + '</div></th>');
        for (var j = 0; j < total.Columns.length; j++) {//Tables
            var data = total.Columns[j];
            html.push('<th><div class="r table-cell" style="width:100px; height:30px;">' + (data.HasData == 1 ? Helper.format(data.Winlost) : '') + '</div></th>');
            html.push('<th><div class="r table-cell" style="width:100px; height:30px;">' + (data.HasData == 1 ? data.Turnover : '') + '</div></th>');
            html.push('<th><div class="r table-cell" style="width:100px; height:30px;">' + (data.HasData == 1 ? Helper.format(data.Margin) : '') + '</div></th>');
            html.push('<th><div class="r table-cell" style="width:100px; height:30px;">' + (data.HasData == 1 ? data.Betcount : '') + '</div></th>');
        }

        html.push('</tr>');

        return html.join('');
    },

    callBackSelTopPunterByCustId: function () {
        $('#PunterBySporttypereport').addClass("hidden");
        $('#PunterByBettypereport').addClass("hidden");
        $('#SummaryLongRange').addClass("hidden");

        $('#PunterSelBetCountByCustomerreport').addClass("hidden");
        PunterAnalysisExportExcel.BetCountType = 1;
        var rs = cache.SelTopPunterByCustID;
        punterByCustId.renderReport(rs);
    },

    callSelTopPunterByBettype: function (liveindicator, bettype, sporttype, bettypeName) {
        var arr = [];

        var spMain = '<span id="spMain" class="link" onclick="punterByCustId.callBackSelTopPunterSummary()">Main</span> <span> > </span>';
        arr.push(spMain);

        if (punter.param.UserName.length == 0) {
            var spUserName = '<span id="spUserName" class="link" onclick="punterBySporttype.callBackSelTopPunterByCustId()">' + $('#spUserName').html() + '</span> <span> > </span>';
            arr.push(spUserName);
        }

        var spSportName = '<span id="spSportName_sporttype" class="link" onclick="punterByBettype.callBackSelTopPunterBySporttype()">' + $('#spSportName_custId').html() + '</span> <span> > </span>';
        arr.push(spSportName);

        var title = '';
        switch (liveindicator) {
            case 0:
                title = 'None Live - ';
                break;
            case 1:
                title = 'Live - ';
                break;
            case 2:
                title = 'None Live & Live - ';
                break;
        }
        title += bettypeName;
        var spLiveIndicator = '<span id="spLiveindicator">' + title + '</span>';
        PunterAnalysisExportExcel.BettypeNameByBettype = bettypeName;
        arr.push(spLiveIndicator);

        $('#gridtitleByBettype').html(arr.join(''));

        punterByBettype.SelTopPunterByBettype(liveindicator, bettype, sporttype);

        $('#PunterBySporttypereport').addClass("hidden");

    }
};

var punterByBettype = {

    renderReport: function (rs, liveindicator, bettype, sporttype) {
        var PunterByBettypereport = $("#PunterByBettypereport");
        PunterByBettypereport.resetGrid("grid4");
        PunterByBettypereport.find(".col_header_content thead").empty().append(punterByBettype.genHeaderFrozenSummary(rs, liveindicator, bettype, sporttype));
        $("#grid4 tbody").empty().append(punterByBettype.genBodyScrollSummary(rs));
        $("#grid4rowheader tbody").empty().append(punterByBettype.gen1stColBodyScroll(rs));

        PunterByBettypereport.find(".row_footer_content thead").empty().append('<tr><th><div style="height:30px; width:300px;" class="table-cell r">' + rs.Total.Name + '</div></th></tr>');
        PunterByBettypereport.find(".col_footer_content thead").empty().append(punterByBettype.genFooterScrollSummary(rs.Total));

        $('#SummaryLongRange').removeClass("hidden");

        if (punter.param.PeriodType == 2) {
            $('#SummaryLongRange').empty().append(punterByBettype.genSummaryLongRange(rs));
        }
        else {
            $('#SummaryLongRange').empty();
        }

        var gridOpts = {
            tblContentId: 'grid4',
            manualCopyHeader: true,
            fullWidth: true,
            fullHeight: true,
            gridHeight: 136 + $('#SummaryLongRange').height() + 6,
            freezeWidth: 300,
            numberFreezeCol: 1
        };

        PunterByBettypereport.removeClass("hidden");
        var PunterBySporttypereport = $("#PunterBySporttypereport");

        PunterBySporttypereport.addClass("hidden");
        PunterByBettypereport.gridScroller(gridOpts);

    },

    SelTopPunterByBettype: function (liveindicator, bettype, sporttype, isViewAnnualFromLongRange, year) {

        punter.setParam();
        PunterAnalysisExportExcel.saveParamForExportExcel(true);
        PunterAnalysisExportExcel.LiveIndicator = liveindicator;
        PunterAnalysisExportExcel.BettypeIdByBettype = bettype;
        PunterAnalysisExportExcel.SportIdByBettype = sporttype;
        PunterAnalysisExportExcel.SportTypeNameByBettype = PunterAnalysisExportExcel.SportTypeName;

        punter.param.LiveIndicator = liveindicator;
        punter.param.BettypeId = bettype;
        punter.param.SportId = sporttype;

        var oldPeriodType = punter.param.PeriodType;
        var oldFromDate = punter.param.FromDate;
        var oldToDate = punter.param.ToDate;
        if (isViewAnnualFromLongRange) {

            var _oldFromYear = parseInt(oldFromDate.split('/')[2]);
            var _oldToYear = parseInt(oldToDate.split('/')[2]);
            //mm/dd/yyyy

            if (_oldToYear - _oldFromYear == 0) {                
                punter.param.FromDate = $('#ddlfMonth').val() + '/01/' + year;
                punter.param.ToDate = $('#ddltMonth').val() + '/' + punter.daysInMonth($('#ddltMonth').val(), year) + '/' + year;
            }
            else if (_oldFromYear == year) {
                punter.param.FromDate = $('#ddlfMonth').val() + '/01/' + year;
                punter.param.ToDate = '12' + '/31/' + year;
            }
            else if (_oldToYear == year) {
                punter.param.FromDate = '01' + '/01/' + year;
                punter.param.ToDate = $('#ddltMonth').val() + '/' + punter.daysInMonth($('#ddltMonth').val(), year) + '/' + year;
            }
            else {
                punter.param.FromDate = '01' + '/01/' + year;
                punter.param.ToDate = '12' + '/31/' + year;
            }


            

            punter.param.PeriodType = 1;


        }

        PunterAnalysisExportExcel.PeriodTypeByBettype = punter.param.PeriodType;
        PunterAnalysisExportExcel.FromDateByBettype = punter.param.FromDate;
        PunterAnalysisExportExcel.ToDateByBettype = punter.param.ToDate;

        var param = {
            param: punter.param
        };

        try { parent.BlockUI(); } catch (e) { }

        var action = 'PunterAnalysis/SelTopPunterByBettype';


        $('#msg').empty();

        $.ajax({
            type: "POST",
            url: window.rootUrl + action,
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: JSON.stringify(param),
            success: function (rs) {

                punterByBettype.renderReport(rs, liveindicator, bettype, sporttype);

                punter.param.FromDate = oldFromDate;
                punter.param.ToDate = oldToDate;
                punter.param.PeriodType = oldPeriodType;
            },
            complete: function () {
                try { parent.jQuery.unblockUI(); } catch (e) { }
            }
        });
    },

    genHeaderFrozenSummary: function (data, liveindicator, bettype, sporttype) {
        var html = [];
        var cnt = data.Punters.length > 0 ? data.Punters[0].Columns.length : 0;
        var title = '';

        //row1
        html.push("<tr>");
        html.push('<th rowspan="2"><div style="height:17px;width: 300px;">&nbsp;</div></th>');

        if (cnt == 0) {
            html.push('<th colspan="4" ><div style="height:17px; width: 415px;" class="table-cell c">&nbsp;</div></th>');
            html.push("<tr><th><div style='height:17px;width:100px;'>Winlost</div></th>");
            html.push("<th><div style='height:17px;width:100px;'>Turnover</div></th>");
            html.push("<th><div style='height:17px;width:100px;'>Margin</div></th>");
            html.push("<th><div style='height:17px;width:100px;'>Bet Cnt</div></th></tr>");
        }

        for (var i = 0; i < cnt; i++) {
            var type = data.Punters[0].Columns[i].Type;
            var year = type.split('|')[0];
            var month = type.split('|')[1];

            switch (punter.param.PeriodType) {
                case 0: //Monthly                                       
                    html.push('<th colspan="4" ><div style="height:17px; width: 415px;" class="table-cell c">' + punter.arrMonths[month - 1] + ' - ' + year + '</div></th>');
                    break;
                case 1: //Annual                                         
                    if (i == 0) {
                        html.push('<th colspan="4"><div style="height:17px; width: 415px;" class="table-cell c">Year ' + year + '</div></th>');
                    }
                    else {
                        html.push('<th colspan="4"><div style="height:17px; width: 415px;" class="table-cell c">' + punter.arrMonths[month - 1] + '</div></th>');
                    }
                    break;
                case 2: //Long Range      
                    if (i == cnt - 1) {
                        html.push('<th colspan="4"><div style="height:17px; width: 415px;" class="table-cell c">Grand Total</div></th>');
                    }
                    else {
                        var onlick = "punterByBettype.SelTopPunterByBettype(" + liveindicator + "," + bettype + "," + sporttype + "," + true + "," + year + ")";
                        html.push('<th colspan="4" ><div style="height:17px; width: 415px;" class="c table-cell link" onclick="' + onlick + '" >Year ' + year + '</div></th>');
                    }
                    break;
            }
        }
        html.push("</tr>");

        //row2
        html.push("<tr>");
        for (var i = 0; i < cnt; i++) {
            html.push("<th><div style='height:17px;width:100px;'>Winlost</div></th>");
            html.push("<th><div style='height:17px;width:100px;'>Turnover</div></th>");
            html.push("<th><div style='height:17px;width:100px;'>Margin</div></th>");
            html.push("<th><div style='height:17px;width:100px;'>Bet Cnt</div></th>");
        }
        html.push("</tr>");

        return html.join('');
    },

    gen1stColBodyScroll: function (rs) {
        var html = [];
        var cnt = rs.Punters.length;
        if (cnt > 0) {
            for (var i = 0; i < cnt; i++) {//Rows
                html.push('<tr>');
                var name = rs.Punters[i].Name;
                var maxLengthPerRow = 58;
                var height = parseInt(name.length / maxLengthPerRow);
                if (parseInt(name.length % maxLengthPerRow) != 0)
                    height += 1;

                height = height * 15;
                if (height == 15 || height == 0) {
                    height = 17
                }

                html.push('<td ><div style="width:300px;height:' + height + 'px;" class="l" >' + name + '</div></td>');
                html.push('</tr>');
            }
        }
        else {
            html.push('<tr>');
            html.push('<td ><div style="width:300px;height:17px;" class="c" >&nbsp;</div></td>');
            html.push('</tr>');
        }
        return html.join('');
    },

    genBodyScrollSummary: function (rs) {
        var html = [];

        var cnt = rs.Punters.length;
        if (cnt > 0) {
            for (var i = 0; i < cnt; i++) {//Rows
                html.push('<tr>');

                var name = rs.Punters[i].Name;
                var maxLengthPerRow = 58;
                var height = parseInt(name.length / maxLengthPerRow);
                if (parseInt(name.length % maxLengthPerRow) != 0)
                    height += 1;

                height = height * 15;
                if (height == 15 || height == 0) {
                    height = 17
                }

                html.push('<td><div style="width:300px; height:' + height + 'px; " class="c" >' + rs.Punters[i].Name + '</div></td>');
                for (var j = 0; j < rs.Punters[i].Columns.length; j++) {//Tables
                    var data = rs.Punters[i].Columns[j];
                    html.push('<td><div class="r">' + (data.HasData == 1 ? Helper.format(data.Winlost) : '') + '</div></td>');
                    html.push('<td><div class="r">' + (data.HasData == 1 ? data.Turnover : '') + '</div></td>');
                    html.push('<td><div class="r">' + (data.HasData == 1 ? Helper.format(data.Margin) : '') + '</div></td>');
                    html.push('<td><div class="r">' + (data.HasData == 1 ? data.Betcount : '') + '</div></td>');
                }

                html.push('</tr>');
            }
        }
        else {
            html.push('<tr><td class="c" style="height:17px;">&nbsp;</td><td class="c" style="height:17px;" colspan="4">There is no information available.</td></tr>');
        }
        return html.join('');
    },

    genFooterScrollSummary: function (total) {
        var html = [];

        html.push('<tr>');
        html.push('<th><div style="width:300px; height:30px;" class="c table-cell" >' + total.Name + '</div></th>');
        for (var j = 0; j < total.Columns.length; j++) {//Tables
            var data = total.Columns[j];
            html.push('<th><div class="r table-cell" style="width:100px; height:30px;">' + (data.HasData == 1 ? Helper.format(data.Winlost) : '') + '</div></th>');
            html.push('<th><div class="r table-cell" style="width:100px; height:30px;">' + (data.HasData == 1 ? data.Turnover : '') + '</div></th>');
            html.push('<th><div class="r table-cell" style="width:100px; height:30px;">' + (data.HasData == 1 ? Helper.format(data.Margin) : '') + '</div></th>');
            html.push('<th><div class="r table-cell" style="width:100px; height:30px;">' + (data.HasData == 1 ? data.Betcount : '') + '</div></th>');
        }

        html.push('</tr>');

        return html.join('');
    },

    genSummaryLongRange: function (rs) {
        var html = [];
        if (rs.GrandTotal == null || rs.GrandTotal.length == 0)
            return html.join('');

        html.push('<table class="tblSum" style="margin-bottom:6px;">');
        html.push('<tr><td class="title" colspan="5">' + $('#spLiveindicator').html() + '</tr>');
        html.push('<tr><td class="c title" style="width:102px">Year</td>');
        html.push('<td class="c title" style="width:102px">Winlost</td>');
        html.push('<td class="c title" style="width:102px">Turnover</td>');
        html.push('<td class="c title" style="width:102px">Margin</td>');
        html.push('<td class="c title" style="width:102px">Bet Count</td></tr>');

        for (var i = 0; i < rs.GrandTotal.length; i++) {
            var data = rs.GrandTotal[i];
            html.push('<tr><td class="c">' + data.Id + '</td>');
            html.push('<td class="r">' + Helper.format(data.Winlost) + '</td>');
            html.push('<td class="r">' + data.Turnover + '</td>');
            html.push('<td class="r">' + Helper.format(data.Margin) + '</td>');
            html.push('<td class="r">' + data.Betcount + '</td></tr>');
        }

        html.push('</table>');
        return html.join('');
    },

    callBackSelTopPunterBySporttype: function () {
        $("#PunterBySporttypereport").removeClass("hidden");
        $('#PunterByBettypereport').addClass("hidden");
        $('#SummaryLongRange').addClass("hidden");
        $('#PunterSelBetCountByCustomerreport').addClass("hidden");
    }
};

var punterSelIPByCustomer = {

    renderReport: function (rs) {
        var mainreport = $("#mainreport");
        mainreport.addClass("hidden");

        var PunterSelIPByCustomerreport = $("#PunterSelIPByCustomerreport");
        PunterSelIPByCustomerreport.resetGrid("grid5");

        $("#grid5 tbody").empty().append(punterSelIPByCustomer.genBodyScrollSummary(rs));
        PunterSelIPByCustomerreport.find(".row_footer_content thead").empty().append('<tr><th><div style="height:30px; width:100px;" class="c table-cell">&nbsp;</div></th></tr>');
        var gridOpts = {
            tblContentId: 'grid5',
            manualCopyHeader: false,
            fullWidth: true,
            fullHeight: true,
            gridHeight: 136,
            freezeWidth: 100,
            numberFreezeCol: 1
        };

        PunterSelIPByCustomerreport.removeClass("hidden");
        PunterSelIPByCustomerreport.gridScroller(gridOpts);

    },

    SelTopPunterByCustId: function (userName, custId) {
        try { parent.BlockUI(); } catch (e) { }

        var oldCustName = punter.param.CustName;
        var oldCustId = punter.param.CustId;

        punter.setParam();
        punter.param.CustName = userName;
        punter.param.CustId = custId;

        //hard code for testing
        //        punter.param.FromDate = '01/01/2012';
        //        punter.param.ToDate = '31/12/2012';
        //        punter.param.CustId = 28104;

        PunterAnalysisExportExcel.FromDateCustIP = punter.param.FromDate;
        PunterAnalysisExportExcel.ToDateCustIP = punter.param.ToDate;
        PunterAnalysisExportExcel.CustIdCustIP = punter.param.CustId;
        PunterAnalysisExportExcel.CustNameCustIP = userName;
        PunterAnalysisExportExcel.PeriodTypeByBettype = punter.PeriodType;

        var param = {
            param: punter.param
        };

        $('#msg').empty();

        // set title
        var title = '<span onclick="punterByCustId.callBackSelTopPunterSummary();" class="link" id="spMain">Main</span><span> > </span>';
        title += '<span>Punter ' + userName + ' by IP from ' + punter.param.FromDate + ' to ' + punter.param.ToDate + '</span>';
        $('#gridtitleSelIPByCustomer').html(title);

        var action = 'PunterAnalysis/SelIPByCustomer';

        $.ajax({
            type: "POST",
            url: window.rootUrl + action,
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: JSON.stringify(param),
            success: function (rs) {
                punterSelIPByCustomer.renderReport(rs);
                punter.param.CustName = oldCustName;
                punter.param.CustId = oldCustId;
            },
            complete: function () {
                try { parent.jQuery.unblockUI(); } catch (e) { }
            }
        });
    },

    genBodyScrollSummary: function (data) {
        var html = [];
        var custId = punter.param.CustId;
        var cnt = data.length;
        if (cnt > 0) {
            for (var i = 0; i < cnt; i++) {
                html.push('<tr style="height: 17px;"><td style="height: 17px;" class="c"><div>' + data[i].Month + '</div></td>');
                html.push('<td><div class="c">' + data[i].Year + '</div></td>');
                html.push('<td><div class="c">' + data[i].IP + '</div></td>');
                html.push('<td><div class="c">' + data[i].Country + '</div></td></tr>');
            }
        }
        else {
            html.push('<tr><td class="c" style="height:17px;">&nbsp;</td><td class="c" style="height:17px;" colspan="3">There is no information available.</td></tr>');
        }
        return html.join('');
    }



};

var punterSelBetCountByCustomer = {
    renderReport: function (rs) {
        var mainreport = $("#mainreport");
        mainreport.addClass("hidden");
        var PunterSelBetCountByCustomerreport = $("#PunterSelBetCountByCustomerreport");
        PunterSelBetCountByCustomerreport.resetGrid("grid6");

        $("#grid6 tbody").empty().append(punterSelBetCountByCustomer.genBodyScrollSummary(rs));
        PunterSelBetCountByCustomerreport.find(".row_footer_content thead").empty().append('<tr><th><div style="height:30px; width:100px;" class="r table-cell">Total</div></th></tr>');
        PunterSelBetCountByCustomerreport.find(".col_footer_content thead").empty().append(punterSelBetCountByCustomer.genFooterScrollSummary(rs.Total));
        var gridOpts = {
            tblContentId: 'grid6',
            manualCopyHeader: false,
            fullWidth: true,
            fullHeight: true,
            gridHeight: 136,
            freezeWidth: 100,
            numberFreezeCol: 1
        };

        PunterSelBetCountByCustomerreport.removeClass("hidden");
        PunterSelBetCountByCustomerreport.gridScroller(gridOpts);

    },

    setTitlebyReportType: function (userName, type, bettypeName, liveindicator) {
        var title = '';
        var titleLive = '';
        switch (liveindicator) {
            case 0:
                titleLive = 'None Live';
                break;
            case 1:
                titleLive = 'Live';
                break;
            case 2:
                titleLive = 'None Live & Live';
                break;
        }

        switch (type) {
            case 'SelTopPunterSummary':
                title = '<span onclick="punterByCustId.callBackSelTopPunterSummary();" class="link" id="spMain">Main</span><span> > </span>';
                title += '<span>Punter ' + userName + ' by bet count</span>';
                break;
            case 'SelTopPunterByCustId':
                $('#PunterByCustIdreport').addClass("hidden");
                title = '<span onclick="punterByCustId.callBackSelTopPunterSummary();" class="link" id="spMain">Main</span><span> > </span>';
                title += '<span class="link" onclick="punterBySporttype.callBackSelTopPunterByCustId()">' + userName + '</span><span> > </span>';
                title += '<span>Punter ' + userName + ' by bet count</span>';
                break;
            case 'SelTopPunterBySporttype':
                $('#PunterBySporttypereport').addClass("hidden");
                title = '<span onclick="punterByCustId.callBackSelTopPunterSummary();" class="link" id="spMain">Main</span><span> > </span>';
                title += '<span class="link" onclick="punterBySporttype.callBackSelTopPunterByCustId()">' + userName + '</span><span> > </span>';
                title += '<span class="link" onclick="punterByBettype.callBackSelTopPunterBySporttype()">' + $('#spSportName_custId').html() + '</span><span> > </span>';
                title += '<span>' + bettypeName + ' - ' + titleLive + '</span>';
                break;
        }
        return title;
    },

    SelBetCountByCustomer: function (userName, custId, type, IsDetail, bettype, bettypeName, liveindicator, sportId, sportName) {
        try { parent.BlockUI(); } catch (e) { }

        var oldCustName = punter.param.CustName;
        var oldCustId = punter.param.CustId;
        var oldLeagueId = punter.param.LeagueId;

        punter.setParam();
        punter.param.CustName = userName;
        punter.param.CustId = custId;
        punter.param.LiveIndicator = liveindicator;
        //punter.param.SportId = sportId;
        if (punter.SearchBy == 0) //Usr
            punter.param.LeagueId = 0;

        PunterAnalysisExportExcel.FromDateCustBetCount = punter.param.FromDate;
        PunterAnalysisExportExcel.ToDateCustBetCount = punter.param.ToDate;
        PunterAnalysisExportExcel.CustIdCustBetCount = custId;
        PunterAnalysisExportExcel.CustNameCustBetCount = userName;
        PunterAnalysisExportExcel.PeriodTypeByBetCount = punter.PeriodType;
        PunterAnalysisExportExcel.LeagueId = punter.param.LeagueId;
        PunterAnalysisExportExcel.LeagueName = punter.param.LeagueName;
        PunterAnalysisExportExcel.LiveIndicatorBetCount = liveindicator;
        if (PunterAnalysisExportExcel.LeagueId == 0) {
            PunterAnalysisExportExcel.LeagueName = "All";
        }

        if (sportId != null && sportId != undefined) {
            punter.param.SportId = sportId;
            PunterAnalysisExportExcel.SportIdBetCount = sportId;
            if (sportName != null && sportName != undefined) {
                punter.param.SportName = sportName;
                PunterAnalysisExportExcel.SportNameBetCount = sportName;
            }
        }
        PunterAnalysisExportExcel.BettypeIdBetCount = bettype;
        PunterAnalysisExportExcel.BettypeNameBetCount = bettypeName;


        var param = {
            param: punter.param
        };

        $('#msg').empty();

        // set title        
        $('#gridtitleSelBetCountByCustomer').html(punterSelBetCountByCustomer.setTitlebyReportType(userName, type, bettypeName, liveindicator));

        var action = 'PunterAnalysis/SelBetCountByCustomer';
        if (IsDetail) {
            action = 'PunterAnalysis/SelBetCountByCustomer_Detail';
            punter.param.BettypeId = bettype;
        }

        $.ajax({
            type: "POST",
            url: window.rootUrl + action,
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: JSON.stringify(param),
            success: function (rs) {
                punterSelBetCountByCustomer.renderReport(rs);
                punter.param.CustName = oldCustName;
                punter.param.CustId = oldCustId;
                punter.param.LeagueId = oldLeagueId;
            },
            complete: function () {
                try { parent.jQuery.unblockUI(); } catch (e) { }
            }
        });
    },

    genBodyScrollSummary: function (data) {
        var html = [];
        var custId = punter.param.CustId;
        var cnt = data.Punters.length;
        if (cnt > 0) {
            for (var i = 0; i < cnt; i++) {
                html.push('<tr style="height: 17px;"><td style="height: 17px;"><div class="c">' + data.Punters[i].Date + '</div></td>');
                html.push('<td><div class="r">' + data.Punters[i].BetCount + '</div></td>');
                html.push('<td><div class="r">' + data.Punters[i].WinCount + '</div></td>');
                html.push('<td><div class="r">' + data.Punters[i].DrawCount + '</div></td>');
                html.push('<td><div class="r">' + Helper.format(data.Punters[i].Winlost) + '</div></td></tr>');
            }
        }
        else {
            html.push('<tr><td class="c" style="height:17px;">&nbsp;</td><td class="c" style="height:17px;" colspan="4">There is no information available.</td></tr>');
        }
        return html.join('');
    },

    genFooterScrollSummary: function (total) {
        var html = [];

        html.push('<tr>');
        html.push('<th><div style="width:100px; height:30px;" class="r table-cell" >Total</div></th>');
        html.push('<th><div style="width:100px; height:30px;" class="r table-cell" >' + (total.HasData == 1 ? total.BetCount : '') + '</div></th>');
        html.push('<th><div style="width:100px; height:30px;" class="r table-cell" >' + (total.HasData == 1 ? total.WinCount : '') + '</div></th>');
        html.push('<th><div style="width:100px; height:30px;" class="r table-cell" >' + (total.HasData == 1 ? total.DrawCount : '') + '</div></th>');
        html.push('<th><div style="width:100px; height:30px;" class="r table-cell" >' + (total.HasData == 1 ? Helper.format(total.Winlost) : '') + '</div></th>');
        html.push('</tr>');

        return html.join('');
    }
};

var grid = {
    getGridWidth: function (width, totalCol)
    {
        return width + totalCol * 3;
    },
    getObj: function ()
    {
        return $("#gridLiveCasino");
    },
    getBody: function ()
    {
        return grid.getObj().find("#tblBodyLiveCasino");
    },
    getFooter: function ()
    {
        return grid.getObj().find("#footerLiveCasino");
    },
    initScroller: function ()
    {
        $("#gridLiveCasino").scroller();

        var gWidth;
        var totalCol;

        totalCol = 4; gWidth = 380;

        LiveCasinoWinLoss.params.totalItems = _TotalItems;

        LiveCasinoWinLoss.initPager(LiveCasinoWinLoss.params.totalItems, LiveCasinoWinLoss.params.PageSize, LiveCasinoWinLoss.params.PageIndex - 1);
        grid.getObj().removeClass("hidden").reRenderScroller(grid.getGridWidth(gWidth, totalCol), 400, false);
        $('.maincontent_box').css({ width: $('#gridLiveCasino').width() - 3 });

        if ($.browser.msie)
            $('#gridLiveCasino .header-frozen .tbl-header-frozen td').css('height', $('#gridLiveCasino .header-scroll').height() - 2);
        else
        {
            $('#gridLiveCasino .header-frozen .tbl-header-frozen td').css('height', $('#gridLiveCasino .header-scroll').height());
            if ($('#gridLiveCasino').height() <= 172)
            {
                $('#gridLiveCasino .header-scroll').css('width', $('#gridLiveCasino .body-scroll').width());
                $('#gridLiveCasino .footer-scroll').css('width', $('#gridLiveCasino .body-scroll').width());
            }
        }

    }
};

var LiveCasinoWinLoss = {
    params: {
        FromDate: '',
        ToDate: '',
        MoveBetDay: '',
        UserName: '',
        ExcludeTest: false,
        SiteID: 0,
        GameType: 1101,
        GameTable: '',
        Currency: 0,
        totalItems: 0,
        ReportBy: 3,
        PageSize: 50,
        PageIndex: 1,
        FromCache: false,
        isAgency: 0,
        Sort: { SortType: '', SortCol: '', IsDecimal: false, IsDate: false }
    },
    filterInfo: [],
    defaultDate: '',
    specialReport: 0,
    specificPeriodIndex: 9,
    Super: '',
    Master: '',
    Agent: '',
    getReportData: function (fromCache, isAgency, isParent, IdParent)
    {
        LiveCasinoWinLoss.getUplineAgency();
        LiveCasinoWinLoss.setParams(isAgency, isParent, IdParent);
        LiveCasinoWinLoss.params.FromCache = fromCache;
        if (!fromCache)
            LiveCasinoWinLoss.params.PageIndex = 1;
        try { parent.BlockUI(); } catch (e) { }
        $.ajax({
            type: "POST",
            url: "../LiveCasinoWinLoss/GetReport",
            contentType: "application/json; charset=utf-8",
            data: JSON.stringify({ dbparams: LiveCasinoWinLoss.params }),
            success: function (result)
            {
                $('#Report').html(result);
                grid.initScroller();

                //                if (fromCache)
                LiveCasinoWinLoss.addSortingIconAfterPaging();
            },
            error: function ()
            {
                showAlertMessage("err", "There is no data.", 1);
            },
            complete: function ()
            {
                try { parent.jQuery.unblockUI(); } catch (e) { }
            }
        });
    },
    getReportDataSpecialDate: function (id)
    {
        LiveCasinoWinLoss.specialReport = id;
        LiveCasinoWinLoss.getReportData(false,0, 0);
    },
    changeDateOption: function ()
    {
        if ($('#txtfromdate').val() != LiveCasinoWinLoss.defaultDate | $('#txttodate').val() != LiveCasinoWinLoss.defaultDate)
        {
            $("#TimePeriodID").attr('selectedIndex', LiveCasinoWinLoss.specificPeriodIndex);
        }
    },
    showhideSearchCondition: function (id)
    {
        if ($(id).hasClass('upCondition'))
        {
            $(id).removeClass('upCondition');
            $(id).addClass('downCondition');
            $('.maincontent_box').addClass('hidden');
        }
        else
        {
            $(id).removeClass('downCondition');
            $(id).addClass('upCondition');
            $('.maincontent_box').removeClass('hidden');
        }
    },
    setParams: function (isAgency, isParent, IdParent)
    {
        LiveCasinoWinLoss.setDateFilter();

        if (isAgency == 0)
        {
            LiveCasinoWinLoss.params.UserName = $('#txtUsername').val().toLowerCase() == 'all' ? '' : $('#txtUsername').val();
        }
        else
        {

            if (isParent == 0)
            {
                LiveCasinoWinLoss.params.UserName = $('#' + IdParent).text();
            }
            else
            {
                if (_RoleId == 3)
                {
                    LiveCasinoWinLoss.params.UserName = LiveCasinoWinLoss.Super;
                }
                else if (_RoleId == 2)
                {
                    LiveCasinoWinLoss.params.UserName = LiveCasinoWinLoss.Master;
                }
                else
                {
                    LiveCasinoWinLoss.params.UserName = LiveCasinoWinLoss.Agent;
                }
            }
        }
        LiveCasinoWinLoss.params.UserName = trim(LiveCasinoWinLoss.params.UserName);
        LiveCasinoWinLoss.params.ExcludeTest = $('#chkExcludeTestAcc').attr('checked');
        LiveCasinoWinLoss.params.SiteID = $('#CompanyID').val();
        LiveCasinoWinLoss.params.GameType = $('#GameTypeID').val();
        LiveCasinoWinLoss.params.GameTable = $('#GameTableID').val();
        LiveCasinoWinLoss.params.Currency = $('#CurrencyID').val();
        LiveCasinoWinLoss.params.ReportBy = $('input[name=reportby]:checked').val();
        LiveCasinoWinLoss.params.isAgency = isAgency;
    },
    setDateFilter: function ()
    {
        var selectValue = $('#TimePeriodID').val();

        var dt = new Date(_today);
        var fDate = new Date(_today);
        var tDate = new Date(_today);
        if (LiveCasinoWinLoss.specialReport == 1)
        {
            fDate = new Date(_today);
            tDate = new Date(_today);
        }
        else if (LiveCasinoWinLoss.specialReport == 2)
        {

            fDate = new Date($('#btnYesterday').val());
            tDate = new Date($('#btnYesterday').val());
        }
        else
        {
            switch (parseInt(selectValue))
            {
                case 1:
                    fDate = dt;
                    tDate = dt;
                    break;
                case 2:
                    fDate = new Date(_yesterday);
                    tDate = new Date(_yesterday);
                    break;
                case 3:
                    var dateRange = gameStatisticsHelper.getDateRangeOfWeek();
                    fDate = dateRange[0];
                    tDate = dateRange[1];
                    break;
                case 4:
                    var dateRange = gameStatisticsHelper.getDateRangeOfLastWeek();
                    fDate = dateRange[0];
                    tDate = dateRange[1];
                    break;
                case 5:
                    var dateRange = gameStatisticsHelper.getDateRangeOfCurrentMonth();
                    fDate = dateRange[0];
                    tDate = dateRange[1];
                    break;
                case 6:
                    var dateRange = gameStatisticsHelper.getDateRangeOfMonths(1);
                    fDate = dateRange[0];
                    tDate = dateRange[1];
                    break;
                case 7:
                    fDate = new Date(dt.getFullYear(), dt.getMonth() - 1, 1);
                    tDate = dt;
                    break;
                case 8:

                    var dateRange = gameStatisticsHelper.getDateRangeOfMonths(3);
                    fDate = dateRange[0];
                    tDate = dateRange[1];

                    break;
                case 9:
                    var dateRange = gameStatisticsHelper.getDateRangeOfMonths(6);
                    fDate = dateRange[0];
                    tDate = dateRange[1];
                    break;
                case 0:
                    fDate = new Date($('#txtfromdate').val());
                    tDate = new Date($('#txttodate').val());
                    break;
                default:
            }
        }

        LiveCasinoWinLoss.params.FromDate = (fDate.getMonth() + 1) + '/' + fDate.getDate() + '/' + fDate.getFullYear();
        LiveCasinoWinLoss.params.ToDate = (tDate.getMonth() + 1) + '/' + tDate.getDate() + '/' + tDate.getFullYear();
    },
    initPager: function (total_items, items_per_page, current_page)
    {
        pagerOpts.callback = LiveCasinoWinLoss.pageSelectCallback;
        pagerOpts.items_per_page = items_per_page;
        pagerOpts.current_page = current_page;
        $("#pagination").pagination(total_items, pagerOpts);
    },
    pageSelectCallback: function (page_index, _items_per_page, jq)
    {
        LiveCasinoWinLoss.params.totalItems = _TotalItems;
        var max_page = Math.ceil(LiveCasinoWinLoss.params.totalItems / _items_per_page);
        page_index = page_index < max_page ? page_index : max_page - 1;
        LiveCasinoWinLoss.params.PageIndex = page_index + 1;
        LiveCasinoWinLoss.params.PageSize = _items_per_page;
        LiveCasinoWinLoss.getReportData(true, 0);

        return false;
    },
    sorting: function (obj, sortCol)
    {

        if (LiveCasinoWinLoss.params.Sort.SortType == '')
        {
            LiveCasinoWinLoss.params.Sort.SortType = 'asc';
        }

        if (LiveCasinoWinLoss.params.Sort.SortType == 'desc')
        {

            LiveCasinoWinLoss.params.Sort.SortType = 'asc';
        }
        else
        {

            LiveCasinoWinLoss.params.Sort.SortType = 'desc';
        }

        LiveCasinoWinLoss.params.Sort.SortCol = sortCol;

        if (sortCol == 'Company' | sortCol == 'SuperTotal' | sortCol == 'MasterTotal' | sortCol == 'MemberTotal' | sortCol == 'AgentTotal' | sortCol == 'MemberTotal' | sortCol == 'RollingTO')
            LiveCasinoWinLoss.params.Sort.IsDecimal = true;
        else
            LiveCasinoWinLoss.params.Sort.IsDecimal = false;

        LiveCasinoWinLoss.renderDataSorted(obj);
    },
    addSortingIconAfterRender: function (obj)
    {

        //--replace sort icon
        //        if (LiveCasinoWinLoss.params.Sort.SortType == '') {
        //            LiveCasinoWinLoss.params.Sort.SortType = 'asc';
        //        }

        var id = $(obj).attr('id');
        if (LiveCasinoWinLoss.params.Sort.SortType == 'asc')
        {

            $('#' + id).addClass("asc");
            $('#' + id).removeClass("desc");
        }
        else
        {
            $('#' + id).addClass("desc");
            $('#' + id).removeClass("asc");
        }
        //-------
    },
    addSortingIconAfterPaging: function ()
    {
        var sorttype = '';
        var sortcol = '';
        if (LiveCasinoWinLoss.params.Sort.SortCol == 'ReportType')
            sortcol = 'divReportType';
        else if (LiveCasinoWinLoss.params.Sort.SortCol == 'MemberTotal')
            sortcol = 'divMemberTol';
        else if (LiveCasinoWinLoss.params.Sort.SortCol == 'AgentToltal')
            sortcol = 'divAgentTol';
        else if (LiveCasinoWinLoss.params.Sort.SortCol == 'MasterTotal')
            sortcol = 'divMasterTol';
        else if (LiveCasinoWinLoss.params.Sort.SortCol == 'SuperTotal')
            sortcol = 'divSuperTol';
        else
            sortcol = 'div' + LiveCasinoWinLoss.params.Sort.SortCol;

        if (LiveCasinoWinLoss.params.Sort.SortType == '')
            return;
        else
            sorttype = LiveCasinoWinLoss.params.Sort.SortType;

        $('#' + sortcol).addClass(sorttype);
    },
    renderDataSorted: function (obj)
    {
        LiveCasinoWinLoss.params.FromCache = true;

        //try { parent.BlockUI(); } catch (e) { }
        $.ajax({
            type: "POST",
            url: "../LiveCasinoWinLoss/GetReport",
            contentType: "application/json; charset=utf-8",
            data: JSON.stringify({ dbparams: LiveCasinoWinLoss.params }),
            success: function (result)
            {
                $('#Report').html(result);
                grid.initScroller();
                LiveCasinoWinLoss.addSortingIconAfterRender(obj);
            },
            error: function ()
            {
                showAlertMessage("err", "There is no data.", 1);
            },
            complete: function ()
            {
                try { parent.jQuery.unblockUI(); } catch (e) { }
            }
        });


    },
    setDefaultFilter: function ()
    {
        //        $('#gridgamestatistic').addClass('hidden');
        LiveCasinoWinLoss.params.Sort.SortType = '';
        LiveCasinoWinLoss.params.Sort.SortCol = '';
        LiveCasinoWinLoss.params.PageIndex = 1;
    },
    exportExcel: function (fromCache)
    {
        if (!fromCache)
        {
            LiveCasinoWinLoss.specialReport = 0;
            LiveCasinoWinLoss.setParams(0);
        }

        var p = LiveCasinoWinLoss.params;
        var url = window.rootUrl + 'LiveCasinoWinLoss?IsExport=true&fdate=' + p.FromDate + '&tdate=' + p.ToDate +
            '&username=' + p.UserName + '&site=' + p.SiteID + '&gType=' + p.GameType + '&gTable=' + p.GameTable +
            '&currency=' + p.Currency + '&isExcludeTest=' + p.ExcludeTest + '&reporttype=' + p.ReportBy +
            '&fromcache=' + fromCache;
        $('#frm').attr('src', url);
    },
    getUplineAgency: function ()
    {
        if (_RoleId == 4)
        {
            LiveCasinoWinLoss.Super = _Username;
            LiveCasinoWinLoss.Master = '';
            LiveCasinoWinLoss.Agent = '';
        }
        else if (_RoleId == 3)
        {
            LiveCasinoWinLoss.Master = _Username;
            LiveCasinoWinLoss.Agent = '';
        }
        else if (_RoleId == 2)
        {
            LiveCasinoWinLoss.Agent = _Username;
        }
    }
};

var pagerOpts = {
    callback: null,
    start_per_page: 10,
    step_per_page: 10,
    max_items_per_page: 200,
    items_per_page: 50,
    current_page: 1
};

function trim(str)
{
    return str.replace(/(^\s*)|(\s*$)/g, "");
};

jQuery('.numbersOnly').keyup(function ()
{
    this.value = this.value.replace(/[^0-9\.]/g, '');
});

var gameStatisticsHelper = {
    getDateRangeOfWeek: function () {
        var _date = new Date(_today);
        var _lastDate = new Date(_today);

        var day = _date.getDay(),
        diff = _date.getDate() - day + (day == 0 ? -6 : 1); // adjust when day is sunday
        var startDate = new Date(_date.setDate(diff));
        startDate = _date.getMonth() < _lastDate.getMonth() ? new Date(_lastDate.getFullYear(), _lastDate.getMonth(), 1) : startDate;

        return [startDate, _lastDate];
    },
    getDateRangeOfLastWeek: function () {
        var lastWeek = new Date(_today);
        // set the date to last week's Sunday
        lastWeek.setDate(lastWeek.getDate() - lastWeek.getDay() - 7);
        var startDate = gameStatisticsHelper.getShortDateString(lastWeek);
        // set the date to last week's Saturday
        lastWeek.setDate(lastWeek.getDate() + 6);
        var endDate = gameStatisticsHelper.getShortDateString(lastWeek);

        return [new Date(startDate), new Date(endDate)];
    },
    getDateRangeOfCurrentMonth: function () {
        var today = new Date(_today);
        var startday = new Date(_today);
        startday.setDate(1);
        startday.setMonth(today.getMonth());
        var endday = new Date(today.setMonth(today.getMonth()));
        return [startday, endday];
    },
    getDateRangeOfMonths: function (totalMonths) {
        var cDate = new Date(_today);
        var currentMonth = cDate.getMonth();
        var currentYear = cDate.getFullYear();
        var lastYear = cDate.getFullYear() - 1;

        var startDate = new Date();
        var endDate = new Date();
        if (totalMonths - (currentMonth - 1) > 0) {

            var month = 12 - (totalMonths - (currentMonth));
            startDate = new Date(lastYear, month, 1);
            endDate = new Date(currentYear, (cDate.getMonth() - 1), (gameStatisticsHelper.getTotalDayOfMonth((currentMonth), currentYear)));
        }
        else if (totalMonths - (currentMonth - 1) == 0) {
            startDate = new Date(lastYear, 12, 1);
            endDate = new Date(lastYear, 12, 31);
        }
        else {
            var month = 12 - (totalMonths - (currentMonth));
            startDate = new Date(lastYear, month, 1);
            endDate = new Date(currentYear, (cDate.getMonth() - 1), (gameStatisticsHelper.getTotalDayOfMonth((currentMonth), currentYear)));
        }

        return [startDate, endDate];
    },
    getCurrentWeekNumber: function () {
        var currentDate = new Date(_today);
        var firstDay = new Date(currentDate.getFullYear(), currentDate.getMonth(), 1).getDay();
        return Math.ceil((currentDate.getDate() + firstDay) / 7);
    },
    getTotalDayOfMonth: function (month, year) {
        var m = [31, 28, 31, 30, 31, 30, 31, 31, 30, 31, 30, 31];
        if (month != 2) return m[month - 1];
        if (year % 4 != 0) return m[1];
        if (year % 100 == 0 && year % 400 != 0) return m[1];
        return m[1] + 1;
    },
    renderCalendarFromTo: function (ids) {
        var defaults = {
            onSelect: function (selectedDate) {
                var option = this.id == "txtfromdate" ? "minDate" : "maxDate",
                     instance = $(this).data("datepicker"),
                     date = $.datepicker.parseDate(
                         instance.settings.dateFormat ||
                             $.datepicker._defaults.dateFormat,
                         selectedDate, instance.settings);
                dates.not(this).datepicker("option", option, date);
                
                LiveCasinoWinLoss.changeDateOption();
            }
        };

        var dates = $(ids).datepicker(defaults);
        var dt = new Date(_today);
        var fdate = $(ids.split(',')[0]);
        var tdate = $(ids.split(',')[1]);
        fdate.datepicker("setDate", dt);
        tdate.datepicker("setDate", dt);
    },
    on_Blur: function () {
        var inputValue = 'All';
        var oldName = '';
        var newName = $('#txtUsername').val();

        if (oldName != '' && newName != '') {
            if (newName != oldName);
            inputValue = newName;
        }
        else if (oldName != '' && newName == '') {
            inputValue = oldName;
        }
        else if (newName != '')
            inputValue = newName;
        else
            inputValue = 'All';

        $('#txtUsername').val(inputValue);
    },
    on_Focus: function () {
        if ($('#txtUsername').val().length > 0)
            $('#txtUsername').val('');
    },
    getShortDateString: function (oDate) {
        // month is 0 based (0 - 11)
        var strDate = gameStatisticsHelper.getDateStr(oDate.getMonth() + 1) + "/";
        strDate += gameStatisticsHelper.getDateStr(oDate.getDate()) + "/";
        strDate += gameStatisticsHelper.getDateStr(oDate.getFullYear());
        return strDate;
    },
    getDateStr: function (d) {
        if (!d) {
            return '';
        }
        if (d < 10) {
            return '0' + d.toString();
        }
        return d.toString();
    }
};

head.ready(function () {
    $('#GameTypeID').live('change', function () {
        var param = 'value = ' + $('#GameTypeID option:selected').val();
        $.ajax({
            type: "POST",
            url: window.rootUrl + "LiveCasinoWinLoss/GameTable",
            data: { value: $('#GameTypeID option:selected').val() },
            success: function (result) {
                $('#GameTable').html(result);
            }
        });
    });

    gameStatisticsHelper.renderCalendarFromTo("#txtfromdate, #txttodate");

    var dt = new Date(_today);
    var yDt = new Date(_yesterday);

    var month = ((dt.getMonth() + 1) > 9 ? "" : "0") + (dt.getMonth() + 1);
    var yMonth = ((yDt.getMonth() + 1) > 9 ? "" : "0") + (yDt.getMonth() + 1);
    var year = dt.getFullYear();
    var yYear = yDt.getFullYear();

    var currentDate = month + '/' + ((dt.getDate() > 9 ? "" : "0") + dt.getDate()) + '/' + year;
    var yesterday = yMonth + '/' + (((yDt.getDate()) > 9 ? "" : "0")) + (yDt.getDate()) + '/' + yYear;

    LiveCasinoWinLoss.defaultDate = currentDate;
    $('#btnToday').val(currentDate);
    $('#btnYesterday').val(yesterday);
    //----------auto complete
    var autosearch;
    var isonauto = false;
    var txtusername = $('#txtUsername');
    autosearch = txtusername.autocomplete({ serviceUrl: '../Handlers/QueryUserName.ashx', minChars: 2 });
    $('.autocomplete').mouseover(function () {
        isonauto = true;
    }).mouseout(function () {
        isonauto = false;
    });
    txtusername.blur(function () {
        if (!isonauto) {
            $('#Autocomplete_1').css('display', 'none');
        }
        else {
            txtusername.focus();
        }

    });
    //-----------
   
});
//-

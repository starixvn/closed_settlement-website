var OANDAExRate = {
    fdate: '',
    tdate: '',
    LoadDataOANDAExRate: function () {
        var arr = [];
        var endRow = false;
        OANDAExRate.fdate = $('#txtfromdate').val();
        OANDAExRate.tdate = $('#txttodate').val();
        var data = { fdate: $('#txtfromdate').val(), tdate: $('#txttodate').val() };
        try { parent.parent.BlockUI(); } catch (e) { }
        $.ajax({
            type: "POST",
            url: "GetExchangeDailyOanda",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: JSON.stringify(data),
            success: function (rs) {
                if (rs.length > 0) {
                    for (var i = 0; i < rs.length; i++) {
                        if (i == rs.length - 1)
                            endRow = true;
                        arr.push(OANDAExRate.renderRow(rs[i], endRow));
                    }
                    $("#tblOANDAExRate").html(arr.join(''));
                    $("#OANDAExRate").removeClass("hidden").reRenderScroller(552, 550, true);
                }
                else {
                    $("#tblOANDAExRate").html('<tr><td colspan="4" style="border-right:0px none"><div class="c b" style="width:552px">There is no information available</div></td></tr>');
                    $("#OANDAExRate").removeClass("hidden").reRenderScroller(549, 550, true);
                }
                try { parent.jQuery.unblockUI(); } catch (e) { }
            }
        });

    },

    renderRow: function (rows, endRow) {
        var strrow = '';
        var items = rows.ExchangeDailyList;
        strrow += '<tr>';
        if (endRow)
            strrow += '<td rowspan="' + items.length + '" style="border-bottom:0px none"><div class="col120 c">' + rows.ExchangeDate + '</div></td>';
        else
            strrow += '<td rowspan="' + items.length + '"><div class="col120 c">' + rows.ExchangeDate + '</div></td>';
        for (var i = 0; i < items.length; i++) {
            var listBaseCurr = items[i].BaseCurrencies;
            if (i == 0) {
                strrow += '<td><div class="col140 c">' + items[i].CurrencyCode + '</div></td>';
                for (var j = 0; j < listBaseCurr.length; j++) {
                    strrow += '<td><div class="col140 r">' + listBaseCurr[j].Value + '</div></td>';
                }
                strrow += '</tr>';
            }
            else {
                strrow += '<tr>';
                strrow += '<td><div class="col140 c">' + items[i].CurrencyCode + '</div></td>';
                for (var j = 0; j < listBaseCurr.length; j++) {
                    strrow += '<td><div class="col140 r">' + listBaseCurr[j].Value + '</div></td>';
                }
                strrow += '</tr>';
            }

        }
        return strrow;

    },

    exportExcelOANDAExRate: function () {

        var url = 'OandaExRate?IsExport=1&fdate=' + OANDAExRate.fdate + '&tdate=' + OANDAExRate.tdate;
        $('#frm').attr('src', url);
    },


    renderCalendar: function (ids, opts) {
        var defaults = {
            changeMonth: true,
            changeYear: true,
            showOtherMonths: true,
            selectOtherMonths: true
        };
        var new_opts = $.extend(true, defaults, opts);
        var dates = $("#" + ids).datepicker(new_opts);
        $("#" + ids).datepicker("setDate", new Date(today));
    },


    setupCalendar: function (id, toDate) {
        //alert(toDate);
        var limitDate = {
          
            maxDate: toDate
        };
        OANDAExRate.renderCalendar(id, limitDate);
        $('#' + id).click(function () {
            $('#' + id).datepicker("show");
        });

    }

};
$(document).ready(function () {
    OANDAExRate.setupCalendar("txtfromdate", today);
    OANDAExRate.setupCalendar("txttodate", today);
}); 
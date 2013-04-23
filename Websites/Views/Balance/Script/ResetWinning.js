var ResetWinning = {
    date: '',
    data: 0,
    getContentResetWinning: function () {
        try { parent.BlockUI(); } catch (e) { }
        var arr = [];
        ResetWinning.date = $('#txtfromdate').val();
        ResetWinning.data = $("input[name='RdData']:checked").val();

        var bgClass = '';
        var data = '{"date":"' + ResetWinning.date + '","data":' + ResetWinning.data + '}';
        $.ajax({
            type: "POST",
            url: "GetDataReportResetWinning",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: data,
            success: function (rs) {
                var records = rs.Items;
                for (var i = 0; i < records.length; i++) {
                    var colorVariance = '';
                    if (i % 2 != 0) {
                        bgClass = 'bgRow';
                    }
                    else
                        bgClass = '';
                    if (parseFloat(records[i].Variance) != 0 && records[i].Variance != "N/A") {
                        bgClass = 'bgSpecial';
                        colorVariance = 'colorVariance';
                    }
                    if (i == records.length - 1)
                        bgClass = 'bgOthers';
                    arr.push(ResetWinning.renderRows(records[i], bgClass, colorVariance));
                }
                arr.push('<tr style="background-color:#C3D79B"><td><div class="col140 l">Total</div></td><td colspan="3"><div class="r">' + rs.Total + '</div></td></tr>')
                $("#tblresetwinning").html(arr.join(''));
                $("#Resetwinning").removeClass("hidden").reRenderScroller(512, 350, true);
                try { parent.jQuery.unblockUI(); } catch (e) { }
            }
        });

    },
    renderRows: function (row, cssClass, colorVariance) {
        var strrow = "";
        strrow += '<tr class="' + cssClass + '">';
        strrow += '<td><div class="col140 l">' + row.Products + '</div></td>';
        strrow += '<td><div class="col120 r">' + row.Winloss + '</div></td>';
        strrow += '<td><div class="col120 r">' + row.Reset + '</div></td>';
        strrow += '<td><div class="col120 r ' + colorVariance + '">' + row.Variance + '</div></td>';
        strrow += '</tr>';
        return strrow;
    },

    renderCalendar: function (ids, opts, chosenDate) {
        var defaults = {
            changeMonth: true,
            changeYear: true,
            showOtherMonths: true,
            selectOtherMonths: true
        };
        var new_opts = $.extend(true, defaults, opts);
        var dates = $("#" + ids).datepicker(new_opts);
        
        //chosenDate = chosenDate.getDay() - 1;
        
        $("#" + ids).datepicker("setDate", chosenDate);
    },


    setupCalendar: function (id, fromDate, toDate, chosenDate) {

        var limitDate = {
            minDate: fromDate,
            maxDate: toDate
        };
        ResetWinning.renderCalendar(id, limitDate, chosenDate);
        $('#' + id).click(function () {
            $('#' + id).datepicker("show");
        });
        ResetWinning.isfirstCalendarSetUp = false;
    }

};


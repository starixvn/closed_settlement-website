var gridHeight = $(window).height() - 150;

var BACustomer = {
    pageIndex: 1,
    pageSize: 100,
    totalItems: 0,
    width: 516,
    height: 590,
    getLicensee: function (model) {
        var arr = '';
        for (var i = 0; i < model.length; i++) {
            var obj = model[i];
            arr += '<option value="' + obj.LicenseeID + '">' + obj.LicenseeName + '</option>'
        }
        $('#dlLicensee').append(arr);

    },

    getBACustomerList: function (fromCache) {
        var LicenseeId = $('#dlLicensee').val();
        var Status = $('#dlStatus').val();
        var username = $('#txtusername').val();
        if (fromCache == 0)
            BACustomer.pageIndex = 1;
        var data = { LicenseeId: $('#dlLicensee').val(), Status: $('#dlStatus').val(), Username: $('#txtusername').val(), pageSize: BACustomer.pageSize, pageIndex: BACustomer.pageIndex, fromCache: fromCache };
        try { parent.BlockUI(); } catch (e) { }
        if (Status == 1) {
            $('#cbxIsFol').attr("checked", true);
            $('#cbxIsFol').attr("disabled", true);
            $('#btnFollowedBet').attr("disabled", true);
        }
        else {
            $('#cbxIsFol').attr("checked", false);
            $('#cbxIsFol').attr("disabled", false);
        }
        $.ajax({
            type: "POST",
            url: "GetBACustomerList",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            async: false,
            data: JSON.stringify(data),
            success: function (rs) {
                var arr = [];
                var j = 0;
                if (rs != null) {
                    if (rs.FollowedCus.length > 0) {
                        var countitems = rs.FollowedCus.length;
                        for (var i = 0; i < rs.FollowedCus.length; i++) {
                            var Item = rs.FollowedCus[i];
                            if (Item.IsFollowed)
                                j++;
                            if (i % 2 != 0)
                                arr.push(BACustomer.renderRows(Item, 'bgeven'));
                            else
                                arr.push(BACustomer.renderRows(Item, ''));
                        }
                        $("#tblBACustomer").empty().html(arr.join(''));
                        BACustomer.totalItems = rs.TotalCustomer;
                        BACustomer.initPager(BACustomer.totalItems, BACustomer.pageSize, BACustomer.pageIndex - 1);
                        if (j == countitems) {
                            $('#cbxIsFol').attr("checked", true);
                            $('#cbxIsFol').attr("disabled", true);
                            $('#btnFollowedBet').attr("disabled", true);
                        }
                        else {
                            $('#cbxIsFol').attr("checked", false);
                            $('#cbxIsFol').attr("disabled", false);
                        }
                    }
                    else {
                        $("#pagination").html('&nbsp;');
                        $("#tblBACustomer").html('<tr><td colspan="9"><div class="c" style="width:526px;">There is no information available.</div></td></tr>');
                        $('#cbxIsFol').attr("checked", false);
                        $('#cbxIsFol').attr("disabled", true);
                    }

                    $('#BACustomer').removeClass("hidden").reRenderScroller(529, gridHeight, true);
                }
                else {
                    $("#pagination").html('&nbsp;');
                    $("#tblBACustomer").html('<tr><td colspan="9"><div class="c" style="width:526px;">There is no information available.</div></td></tr>');
                    $('#BACustomer').removeClass("hidden").reRenderScroller(529, gridHeight, true);
                    $('#cbxIsFol').attr("checked", false);
                    $('#cbxIsFol').attr("disabled", true);
                }
            },
            complete: function () {
                try { parent.jQuery.unblockUI(); } catch (e) { }
            }

        });
    },

    initPager: function (total_item, items_per_page, current_page) {
        var opt = {
            start_per_page: 50,
            step_per_page: 50,
            max_items_per_page: 500,
            item_per_page_arr: [100, 200, 300, 400, 500],
            callback: BACustomer.pageSelectCallback
        };
        opt["items_per_page"] = items_per_page;
        opt["current_page"] = current_page;
        $("#pagination").pagination(total_item, opt);
    },
    pageSelectCallback: function (page_index, _items_per_page, jq) {
        var max_page = Math.ceil(BACustomer.totalItems / _items_per_page);
        page_index = page_index < max_page ? page_index : max_page - 1;
        BACustomer.pageIndex = page_index + 1;
        BACustomer.pageSize = _items_per_page;
        BACustomer.getBACustomerList(1);
        return false;
    },

    renderRows: function (row, cssClass) {
        var html = '';
        html = '<tr class="' + cssClass + '">';
        html += '<td><div class="col200 c">' + row.Username + '</div></td>';
        if (row.IsFollowed)
            html += '<td><div class="col160 c"><input type="checkbox" disabled="disabled" checked="checked" class="cbx_dis" value="' + row.CustId + '"  /></div></td>';
        else
            html += '<td><div class="col160 c"><input type="checkbox" class="cbx" value="' + row.CustId + '" onclick="checkenableBtnFollowedBet()" /></div></td>'
        html += '<td><div class="col160 c">' + row.Licensee + '</div></td>';

        return html;

    },

    addMultiBACustomers: function () {
        var strCustId = '';
        $("input.cbx").each(function () {
            if ($(this).attr("checked") && !$(this).attr("disabled"))
                strCustId += $(this).val() + ",";
        });
        var str = strCustId.substr(0, strCustId.length - 1);
        var rate = $("#txtRate").val();
        if (rate == '') {
            showAlertMessage("errMsgDig", "Please input Rate!", 1);
            $("#errMsgDig").show().find("div:first").width(350);
        }
        else if (!isPositiveDouble(rate) || parseFloat(rate) <= 0 || rate == ".") {
            showAlertMessage("errMsgDig", "Invalid Rate . Please input positive double value!", 1);
            $("#errMsgDig").show().find("div:first").width(350);
            return;
        }
        else if (parseFloat(rate) < 0.5 || parseFloat(rate) > 3) {
            showAlertMessage("errMsgDig", "Invalid Rate! Rate must be greater than or equal to 0.5 and less than or equal to 3.", 1);
            $("#errMsgDig").show().find("div:first").width(350);
            return;
        }
        else {
            var data = { strCustId: str, Rate: $('#txtRate').val() };
            $.ajax({
                type: "POST",
                url: "InsMultiBACustomer",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                async: false,
                data: JSON.stringify(data),
                success: function (rs) {
                    if (parseInt(rs.ErrorCode) != 0) {
                        showAlertMessage("errMsgDig", rs.ErrorMessage, 1);
                        $("#errMsgDig").show().find("div:first").width(350);
                    }
                    else {

                        showAlertMessage("errMsgDig", rs.ErrorMessage, 0);
                        $("#errMsgDig").show().find("div:first").width(350);
                        BACustomer.getBACustomerList(0);
                        $('#btnFollowedBet').attr("disabled", true);

                    }
                    try { parent.jQuery.unblockUI(); } catch (e) { }
                }
            });
        }
    }

};
$(function () {
    $('#addMultiBACustomers').dialog({
        title: "Add Rate for BA Customers",
        autoOpen: false,
        height: 140,
        width: 370,
        modal: true
    });

});

function changeStatusCbx() {
    if ($('#cbxIsFol').attr("checked")) {
        $("input.cbx").each(function () {
            if (!$(this).attr("disabled"))
                $(this).attr("checked", true);

        });
        $('#btnFollowedBet').attr("disabled", false);
    }
    else {
        $("input.cbx").each(function () {
            if (!$(this).attr("disabled"))
                $(this).attr("checked", false);

        });
        $('#btnFollowedBet').attr("disabled", true);
    }
}

function checkenableBtnFollowedBet() {
    var count = $("input.cbx").length;
    var i = 0;
    $("input.cbx").each(function () {
        if ($(this).attr("checked"))
            i++;
    });
    if (i >= 1 && i < parseInt(count)) {
        $('#btnFollowedBet').attr("disabled", false);
        $('#cbxIsFol').attr("checked", false);
        return false;
    }
    else if (i < 1) {
        $('#btnFollowedBet').attr("disabled", true);
        $('#cbxIsFol').attr("checked", false);
        return false;
    }
    else if (i == parseInt(count)) {
        $('#btnFollowedBet').attr("disabled", false);
        $('#cbxIsFol').attr("checked", true);
        return false;
    }
   
}

function isPositiveDouble(rate) {
    var validChars = '0123456789.';
    for (var i = 0; i < rate.length; i++) {
        if (validChars.indexOf(rate.charAt(i)) == -1)
            return false;
    }
    return true;
}

function showPopup() {
    $('#errMsgDig').hide();
    $('#txtRate').val('');
    $('#addMultiBACustomers').removeClass("hidden").dialog("open");
}

function changeDlStatus() {
    if (parseInt($('#dlStatus').val()) == 1) {
        $('#btnFollowedBet').attr("disabled", true);
        //$('#cbxIsFol').attr("disabled", true);
        //$('#cbxIsFol').attr("checked", true);
    }
    else {
        $('#cbxIsFol').attr("disabled", false);
       
    }
}

$(document).ready(function () {
    BACustomer.getLicensee(BACustomer.model);
});
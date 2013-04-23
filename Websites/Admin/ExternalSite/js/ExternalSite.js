var _pageIndex = 1;
var _pageSize = 50;
var total_item = 0;
var noject = 0;
var fromcahe = false;
var objCust = [];
var groupname;
var username;
var isResfresh = false;


$(function () {
    $('#addNewSuper').dialog({
        title:"Add Existing Super",
        autoOpen: false,
        height: 100,
        width: 350,
        modal: true
    });

});
function changeGroup() {
    fromcahe = false;
   
}
function trim(s) {
    var l = 0; var r = s.length - 1;
    while (l < s.length && s[l] == ' ')
    { l++; }
    while (r > l && s[r] == ' ')
    { r -= 1; }
    return s.substring(l, r + 1);
}
function onKeyEnter(e) {
    if (e.keyCode == 13) {
        isResfresh = false;
        ExterSite.getCustInfo();
    }
}

function showPopup() {
    $('#errMsg').hide();
    $('#txtUserName').val('');
    $('#addNewSuper').removeClass("hidden").dialog("open");
}
function hidePopup() {
    $('#addNewSuper').removeClass("hidden").dialog("close");
}
function renderGroup() {
    $.ajax({
        type: "POST",
        url: "../../Admin/ExternalSite/Service/ExternalSiteService.svc/GetGroupName",
        data: "",
        contentType: "application/json; charset=utf-8",
        success: function (rs) {
            var objSelect = $('#groupName');
            for (var i = 0; i < rs.length; i++) {
                objSelect.append("<option value='" + rs[i] + "'>" + rs[i] + "</option>");
            try { parent.jQuery.unblockUI(); } catch (e) { }
            }

        }
    });
}

function GetReportPager() {
    fromcahe = true;
    try { parent.BlockUI(); } catch (e) { }
    var arr = [];
    var Path = "";
    
    var param = { "groupName": groupname, "pageIndex": _pageIndex, "pageSize": _pageSize, "fromcache": fromcahe, "username": username }
    $.ajax({
        type: "POST",
        url: "../../Admin/ExternalSite/Service/ExternalSiteService.svc/PagerCust",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        data: $.toJSON(param),
        success: function (rs) {
            objCust = rs;
            if (rs.rows.length > 0) {
                var tbl = $("#tblexternalsite");
                tbl.empty();
                var cssClassEven = "";
                $.each(rs.rows, function (i, row) {
                    cssClassEven = i % 2 == 0 ? "" : "evenrow";
                    arr.push(RenderRow(i, row, cssClassEven));
                });

                tbl.html(arr.join(''));
                $("#gridExAccControl").reRenderScroller(783, 460, true);
                try { parent.jQuery.unblockUI(); } catch (e) { }
            }
        }
    });
}

function initPager(total_item, items_per_page, current_page) {
    var opt = { callback: pageselectCallback };
    // Collect options from the text fields - the fields are named like their option counterparts
    opt["items_per_page"] = items_per_page;
    opt["current_page"] = current_page;
    
    
    $("#pagination").pagination(total_item, opt);
}


function pageselectCallback(page_index, _items_per_page, jq) {

    var max_page = Math.ceil(total_item / _items_per_page);
    page_index = page_index < max_page ? page_index : max_page - 1;

    _pageIndex = page_index + 1;
    _pageSize = _items_per_page;

    GetReportPager();
    initPager(total_item, _items_per_page, page_index);

    return false;
}

var ExterSite = {
    arr: [],
    getCustInfo: function () {
        try { parent.BlockUI(); } catch (e) { }

        if (isResfresh == false) {
            groupname = $('#groupName').val();
            username = trim($('#txt_search').val());

        }
        if (flag == true)
            _pageIndex = 1;

        var param = { "groupName": groupname, "pageIndex": _pageIndex, "pageSize": _pageSize, "fromcache": fromcahe, "username": username };

        $('#search_user').show();
        $('#trMsg').hide();
        $('#nodata').hide();
        $('#pagination').show();

        $.ajax({
            type: "POST",
            url: "../../Admin/ExternalSite/Service/ExternalSiteService.svc/PagerCust",
            data: $.toJSON(param),
            contentType: "application/json; charset=utf-8",
            success: function (rs) {

                objCust = rs;

                var gridExAccControl = $("#gridExAccControl");
                total_item = rs.total;
                ExterSite.arr = [];
                var total = rs.rows.length;
                if (total > 0) {
                    var tbl = $("#tblexternalsite");

                    var cssClassEven = "";

                    $.each(rs.rows, function (i, row) {
                        cssClassEven = i % 2 == 0 ? "" : "evenrow";
                        ExterSite.arr.push(RenderRow(i, row, cssClassEven));
                    });
                    tbl.empty().html(ExterSite.arr.join(''));
                    if (flag == true)
                        initPager(total_item, _pageSize, 0);
                    else {
                        initPager(total_item, _pageSize, _pageIndex - 1);
                        flag = true;
                    }
                    gridExAccControl.show();
                    gridExAccControl.removeClass("hidden").reRenderScroller(783, 460, true);

                    try { parent.jQuery.unblockUI(); } catch (e) { }

                }
                else {
                    $('#nodata').show();
                    showAlertMessage("nodata", "Username does not belong to "+groupname+" group.", 1);
                    gridExAccControl.hide();
                    try { parent.jQuery.unblockUI(); } catch (e) { }
                }
            }
        });
    },
    addCustomer: function () {
        var username = $('#txtUserName').val();
        var groupName = $('#groupName').val();
        var param = { "username": username, "groupName": groupName };
        $('#errMsg').hide();
        $('#nodata').hide();
        $('#trMsg').hide();

        $.ajax({
            type: "POST",
            url: "../../Admin/ExternalSite/Service/ExternalSiteService.svc/AddCustomer",
            data: $.toJSON(param),
            contentType: "application/json; charset=utf-8",
            success: function (rs) {
                if (rs.ErrorCode == 0) {
                    $('#errMsg').show();
                    fromcahe = false;
                    showAlertMessage('errMsg', 'Added Successfully!', 0);
                    setTimeout('hidePopup();', 2000);
                    setTimeout('ExterSite.getCustInfo();', 3000);

                }
                else {
                    $('#errMsg').show();
                    showAlertMessage('errMsg', rs.ErrorMessage, 1);
                }
            }
        });

    },
    deleteCustomer: function (custId) {
        flag = false;
        var param = { "custId": custId };
        $('#nodata').hide();
        $('#trMsg').hide();
        if (!confirm('Are you sure you want to remove this super?'))
            return;
        $.ajax({
            type: "POST",
            url: "../../Admin/ExternalSite/Service/ExternalSiteService.svc/DeleteCustomer",
            data: $.toJSON(param),
            contentType: "application/json; charset=utf-8",
            success: function (rs) {
                if (rs.ErrorCode == 0) {
                    $('#trMsg').show();
                    showAlertMessage("dvMsg", "Removed Successfully", 0);
                    var param = { "groupname": groupname, "username": username };
                    $.ajax({
                        type: "POST",
                        url: "../../Admin/ExternalSite/Service/ExternalSiteService.svc/GetInfoCusts",
                        data: $.toJSON(param),
                        contentType: "application/json; charset=utf-8",
                        success: function (rs) {
                            if (rs.length <= _pageSize)
                                _pageIndex = 1;
                        }
                    });
                    setTimeout('ExterSite.getCustInfo();', 2000);
                }
                else {
                    $('#trMsg').show();
                    showAlertMessage("dvMsg", rs.ErrorMessage, 1);
                }

            }
        });

    },
    SubmitData: function() {
        isResfresh = false;
        ExterSite.getCustInfo();
    },
    refreshPage: function () {
        isResfresh = true;
        ExterSite.getCustInfo();
    },
    searchCustomer: function () {
        var sear_result;
        _pageIndex = 1;
        $('#nodata').hide();
        $('#trMsg').hide();
        var groupname = $('#groupName').val();
        var username = $('#txt_search').val();
        var param = { "username": username, "groupname": groupname, "pageIndex": _pageIndex, "pageSize": _pageSize };
        try { parent.BlockUI(); } catch (e) { }
        if (username == null || username == "") {
            $('#trMsg').show();
            showAlertMessage("dvMsg", "Please input username!", 1);
            try { parent.jQuery.unblockUI(); } catch (e) { }
        }
        else {
            $.ajax({
                type: "POST",
                url: "../../Admin/ExternalSite/Service/ExternalSiteService.svc/SearchCust",
                data: $.toJSON(param),
                contentType: "application/json; charset=utf-8",
                success: function (rs) {
                    objCust = rs;

                    var gridExAccControl = $("#gridExAccControl");
                    total_item = rs.total;
                    ExterSite.arr = [];
                    var total = rs.rows.length;
                    if (total_item > 0) {
                        var tbl = $("#tblexternalsite");
                        $('#pagination').show();
                        var cssClassEven = "";

                        $.each(rs.rows, function (i, row) {
                            cssClassEven = i % 2 == 0 ? "" : "evenrow";
                            ExterSite.arr.push(RenderRow(i, row, cssClassEven));
                        });
                        tbl.empty().html(ExterSite.arr.join(''));

                        initPager(total_item, _pageSize, _pageIndex - 1);
                        gridExAccControl.show();
                        gridExAccControl.removeClass("hidden").reRenderScroller(783, 460, true);

                        try { parent.jQuery.unblockUI(); } catch (e) { }
                    }
                    else {
                        $('#nodata').show();
                        showAlertMessage("nodata", "No customer has this username.", 1);
                        gridExAccControl.hide();
                        try { parent.jQuery.unblockUI(); } catch (e) { }

                    }
                }

            });
        }


    }
}

function RenderRow(index, row, cssClassEven) {
    no = _pageSize * (_pageIndex - 1) + index + 1;
    var strrow = "";
    strrow = "<tr class="+cssClassEven+">";
    strrow += "<td><div class='w50 c'>";
    strrow += no;
    strrow += "</div></td>";
    strrow += "<td><div class='w180 c'>";
    strrow += row.UserName;
    strrow += "</div></td>";
    strrow += "<td><div class='w180 c'>";
    strrow += row.FirstName;
    strrow += "</div></td>";
    strrow += "<td><div class='w180 c'>";
    strrow += row.LastName;
    strrow += "</div></td>";
    strrow += "<td><div class='w120 c'>";
    strrow += row.LastLoginIp;
    strrow += "</div></td>";
    strrow += "<td><div class='w55 c'>";
    strrow += "<img class='hand' src='../../App_Themes/Images/icn_del.png' title='Remove Super' onclick='ExterSite.deleteCustomer(" + row.CustId + ")' />";
    strrow += "</div></td>";
    strrow += "</tr>";
   
    return strrow;
}

    

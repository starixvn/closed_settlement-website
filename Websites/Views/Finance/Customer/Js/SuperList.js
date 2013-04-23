var superList = {
    init: function () {
        this.pageIndex = 0;
        this.pageSize = 100;
        this.fillCurrencyList();
        if ($.browser.msie) {
            $("#txtSearch").focus(function () {
                var val = $(this).val().replace(/ /g, '');
                if (val == '' || val.toLowerCase() == 'all') {
                    $(this).val('');
                }
            }).blur(function () {
                var val = $(this).val().replace(/ /g, '');
                if (val == "") {
                    $(this).val('All');
                }
            }).val("All");
        }
        $("#btnGo").click(function () {
            superList.getData();
        });

        var roleId = $("#hiddenRoleId").val();
        switch (roleId) {
            case "4":
                $("#title").html("Super Finance List");
                break;
            case "3":
                $("#title").html("Master Finance List");
                break;
            case "2":
                $("#title").html("Agent Finance List");
                break;
            case "1":
                $("#title").html("Member Finance List");
                break;
        }

        this.getData();
    },
    fillCurrencyList: function () {
        $("#listCurrency").append('<option selected="selected" value="">All</option>');
        $(currencyList).each(function () {
            $("#listCurrency").append('<option value="' + this.Id + '">' + this.Name + '</option>');
        });
    },
    getPostParam: function () {
        var params = [];
        var roleId = $("#hiddenRoleId").val();
        if (roleId == "4") {
            var keyWord = _utils.trimStartEnd($("#txtSearch").val());
            if (keyWord != "" && keyWord.toLowerCase() != "all") {
                params.push({
                    Name: "keyword",
                    Value: keyWord
                });
            }
            if ($("#listAlpha").val() != "All") {
                params.push({
                    Name: "firstchar",
                    Value: $("#listAlpha").val()
                });
            }
            if ($("#listCurrency").val() != "") {
                params.push({
                    Name: "currency",
                    Value: $("#listCurrency").val()
                });
            }
            if ($("#listType").val() != "") {
                params.push({
                    Name: "closed",
                    Value: $("#listType").val() == 0 ? false : true
                });
            }
        }
        else {
            var paramName = "";
            if (roleId == "3") {
                paramName = "srecommend";
            }
            else if (roleId == "2") {
                paramName = "mrecommend";
            }
            else if (roleId == "1") {
                paramName = "recommend";
            }
            params.push({
                Name: paramName,
                Value: $("#hiddenUplineId").val()
            });
        }
        params.push({
            Name: "PageIndex",
            Value: superList.pageIndex + 1
        });
        params.push({
            Name: "PageSize",
            Value: superList.pageSize
        });

        return {
            postParams: params,
            roleId: roleId
        };
    },
    getData: function () {
        _utils.blockUI();
        $.ajax({
            url: 'GetCustomerList',
            type: 'POST',
            dataType: 'json',
            data: $.toJSON(superList.getPostParam()),
            contentType: 'application/json; charset=utf-8',
            success: function (result) {
                customerList = result;
                superList.renderGrid()
                _utils.unBlock();
            }
        });
    },
    renderGrid: function () {
        var customers = customerList.Customers;
        var arr = [];
        if (customers.length == 0) {
            arr = $("#scriptTmpl_NoInfo").tmpl([{ noinfo: true}]);
            $("#tbodyCustList").empty().append(arr);
            $("#footer").hide();
        }
        else {
            arr = $("#scriptTmpl").tmpl(customers);
            $("#tbodyCustList").empty().append(arr);
            $("#footer").show();

            //Menu edit
            //this.bindEventMenuEdit();

            this.initPager(customerList.Total, this.pageSize, this.pageIndex);
        }
    },
    bindEventMenuEdit: function () {
        var hideMenu = function () {
            if (typeof superList.currentMenu != "undefined") {
                superList.currentMenu.hide();
            }
        };
        $(document).click(function () {
            hideMenu();
        });
        $("#tbodyCustList").find("span[menuedit]").click(function (e) {
            hideMenu();
            superList.currentMenu = $($(this).attr("menuid")).css("margin-top", "-2px").css("margin-left", "100px").show();
            e.stopPropagation();
        });
    },
    initPager: function (totalItem, itemsPerPage, currentPage) {
        var opt = {
            callback: $.proxy(function () {
                this.pageSelectCallback(arguments[0], arguments[1], arguments[2]);
            }, this),
            item_per_page_arr: [100, 200, 300, 400, 500],
            items_per_page: itemsPerPage,
            current_page: currentPage
        };
        if (totalItem > 0)
            $("#pagination").pagination(totalItem, opt);
        else
            $("#pagination").hide();
    },
    pageSelectCallback: function (pageIndex, itemsPerPage) {
        var maxPage = Math.ceil(customerList.Total / itemsPerPage);
        pageIndex = pageIndex < maxPage ? pageIndex : maxPage - 1;

        this.pageSize = parseInt(itemsPerPage);
        this.pageIndex = pageIndex;
        this.getData();
        return false;
    },
    openEditSuper: function (id, uname) {
        var url = "Customer/v2/AddNewSuper.aspx?custid=" + id + "&uname=" + uname;
        showPopup("Update Super Setting", url, "", "", 980, 650, false, 2000, function () {
            superList.getData();
        });
    },
    openEditMaster: function (custid, uname) {
        var superid = $("#hiddenUplineId").val();
        var url = "Customer/v2/EditMaster.aspx?custid=" + custid + "&superId=" + superid + "&uname=" + uname;
        showPopup("Edit Master Setting", url, "", "", 1000, 650, false, 2000, function () {
            superList.getData();
        });
    },
    openEditAgent: function (custid, uname) {
        var masterId = $("#hiddenUplineId").val();
        var url = "Customer/v2/EditAgent.aspx?custId=" + custid + "&masterId=" + masterId + "&uname=" + uname;
        showPopup("Update Agent Setting", url, "", "", 980, 650, false, 2000, function () {
            superList.getData();
        });
    },
    openEditMember: function (custid, uname) {
        var agentId = $("#hiddenUplineId").val();
        var url = "Customer/v2/EditCustomerNew.aspx?custId=" + custid + "&m=0&agentId=" + agentId + "&uname=" + uname;
        showPopup("Update Member Setting", url, "", "", 980, 650, false, 2000, function () {
            superList.getData();
        });
    },
    openEditCustInfo: function (id, type) {
        var url = "Customer/EditCustomerInfo.aspx?custId=" + id + "&type=" + type;
        showPopup("Edit Customer", url, "", "", 600, 360, false, 2000, function () {
            superList.getData();
        });
    }
};
$(document).ready(function () {
    superList.init();
});
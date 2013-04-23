var userLog = {
    init: function () {
        var dates = $("#tbxFromDate, #tbxToDate").datepicker({
            changeMonth: true,
            changeYear: true,
            today: new Date($("#hiddenServerDate").val()),
            minDate: "-18m",
            onSelect: function (selectedDate) {
                var option = this.id == "tbxFromDate" ? "minDate" : "maxDate",
				instance = $(this).data("datepicker"),
				date = $.datepicker.parseDate(
					instance.settings.dateFormat ||
					$.datepicker._defaults.dateFormat,
					selectedDate, instance.settings);
                dates.not(this).datepicker("option", option, date);
            }
        });

        var today = new Date($("#hiddenCurrentDate").val());
        var tomorow = new Date($("#hiddenCurrentDate").val());
        tomorow.setDate(tomorow.getDate() + 1);
        $("#tbxFromDate").datepicker("setDate", today);
        $("#tbxToDate").datepicker("setDate", tomorow);

        //btnSearch
        $("#btnSearch").click($.proxy(function () {
            closePopup();
            this.search();
        }, this));

        //txtFilterBy
        $("#txtFilterBy").keyup($.proxy(function (e) {
            if (e.keyCode == 13) {
                closePopup();
                this.search();
            }
        }, this));
    },
    search: function () {
        if (accManager.utils.isEmpty($("#txtFilterBy").val())) {
            if ($("#rdoGroup").is(":checked")) {
                accManager.utils.showErr("Please input group name!");
            }
            else {
                accManager.utils.showErr(" Please input admin name!");
            }
            $("#txtFilterBy").focus();
            return;
        }
        closePopup();
        accManager.utils.hideMsg();
        userLog.mainGrid.pageIndex = 0;
        userLog.mainGrid.getData();
    }
};

userLog.mainGrid = {
    width: 1018,
    height: 650,
    pageIndex: 0,
    pageSize: 100,
    getData: function () {
        var param = {
            fromDate: $("#tbxFromDate").val(),
            toDate: $("#tbxToDate").val(),
            type: ($("#rdoGroup").is(":checked") ? 0 : 1),
            userName: accManager.utils.removeSpecifiedCharacter(accManager.utils.trimStartEnd($("#txtFilterBy").val())),
            pageIndex: this.pageIndex,
            pageSize: this.pageSize
        };
        userLog.utils.blockUI();
        $.post("UserControlLogSelect", param, $.proxy(function (data) {
            this.data = data;
            this.data.Logs = this.data.Logs;
            this.render(data);
            userLog.utils.unBlock();
        }, this));
    },
    render: function (data) {
        if (this.data.RowsTotal > 0) {
            $("#tbodyUserLogMainGrid").empty();
            for (var i = 0; i < this.data.Logs.length; i++) {
                $("#scriptTmpUserLogMainGrid").tmpl([this.data.Logs[i]]).appendTo($("#tbodyUserLogMainGrid"));
            }
            $("#diveUserLogMainGrid").find("div[paging]").show();
            $("#diveUserLogMainGrid").show();
            this.initPager(this.getCurrentVMLength(), this.pageSize, this.pageIndex);
            var timerId = setTimeout(function () {
                $("#tbodyUserLogMainGrid").find("tr").mouseover(function () {
                    $(this).find("td").css("background-color", "#FFDDFE");
                }).mouseout(function () {
                    $(this).find("td").css("background-color", "");
                });
                clearTimeout(timerId);
            }, 200);
        }
        else {
            if (data.Message != "") {
                accManager.utils.showErr(data.Message);
                $("#diveUserLogMainGrid").hide();
            }
            else {
                $("#scriptTmpUserLogMainGrid_NoInfo").tmpl([{ noinfo: true}]).appendTo($("#tbodyUserLogMainGrid").empty());
                $("#diveUserLogMainGrid").show();
                $("#diveUserLogMainGrid").find("div[paging]").hide();
            }
        }
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
            $("#pagination1").pagination(totalItem, opt);
        else
            $("#pagination1").hide();
    },
    getCurrentVMLength: function () {
        return this.data.RowsTotal;
    },
    pageSelectCallback: function (pageIndex, itemsPerPage) {
        var maxPage = Math.ceil(this.getCurrentVMLength() / itemsPerPage);
        pageIndex = pageIndex < maxPage ? pageIndex : maxPage - 1;

        this.pageSize = parseInt(itemsPerPage);
        this.pageIndex = pageIndex;
        this.getData();
        return false;
    },
    showDetail: function (logId, actionId, param) {
        closePopup();
        switch (actionId) {
            case 1708:
                this.showLogUpdateGroupInfo(logId, param);
                break;
            case 1709:
            case 1700:
                this.showLogUpdateGroupPolicies(logId, param);
                break;
            case 1704:
                this.showLogUpdateAccountInfo(logId, param);
                break;
            case 1703:
                this.showLogUpdateAdminPolicies(logId, param);
                break;
            case 1701:
                this.showLogNewAccountInfo(logId, param);
                break;
        };
    },
    showLogNewAccountInfo: function (logId, param) {
        var row = this.getRow(logId);
        $("#divUserLogNewAccountInfo").find("div[groupname]").html(row.NewAccountInfo.ToGroup);
        $("#divUserLogNewAccountInfo").find("div[firstname]").html(row.NewAccountInfo.FirstName);
        $("#divUserLogNewAccountInfo").find("div[lastname]").html(row.NewAccountInfo.LastName);
        showPopup({
            content: $("#divUserLogNewAccountInfo"),
            title: "Individual Information : " + param,
            width: 400,
            height: 190,
            multiple: false, dialog: true
        });
    },
    showLogUpdateGroupInfo: function (logId, param) {
        var row = this.getRow(logId);
        $("#divUserLogUpdateGroupInfo").find("div[groupnameold]").html(row.GroupInfo.GroupNameOld);
        $("#divUserLogUpdateGroupInfo").find("div[groupnamenew]").html(row.GroupInfo.GroupNameNew);
        $("#divUserLogUpdateGroupInfo").find("div[descriptionold]").html(row.GroupInfo.DescriptionOld);
        $("#divUserLogUpdateGroupInfo").find("div[descriptionnew]").html(row.GroupInfo.DescriptionNew);
        showPopup({
            content: $("#divUserLogUpdateGroupInfo"),
            title: "Group Information : " + param,
            width: 839,
            height: 180,
            multiple: false, dialog: true
        });
    },
    showLogUpdateAccountInfo: function (logId, param) {
        var row = this.getRow(logId);
        $("#divUserLogUpdateAccountInfo").find("div[firstnameold]").html(row.AccountInfo.FirstNameOld);
        $("#divUserLogUpdateAccountInfo").find("div[firstnamenew]").html(row.AccountInfo.FirstNameNew);
        $("#divUserLogUpdateAccountInfo").find("div[lastnameold]").html(row.AccountInfo.LastNameOld);
        $("#divUserLogUpdateAccountInfo").find("div[lastnamenew]").html(row.AccountInfo.LastNameNew);
        showPopup({
            content: $("#divUserLogUpdateAccountInfo"),
            title: "Individual Information : " + param,
            width: 839,
            height: 180,
            multiple: false, dialog: true
        });
    },
    showLogUpdateGroupPolicies: function (logId, param) {
        var row = this.getRow(logId);
        userLog.groupPolicy.show(row, param);
    },
    showLogUpdateAdminPolicies: function (logId, param) {
        var row = this.getRow(logId);
        userLog.adminPolicy.show(row, param);
    },
    getRow: function (logId) {
        for (var i = 0; i < this.data.Logs.length; ++i) {
            if (this.data.Logs[i].LogId == logId) return this.data.Logs[i];
        }
        return null;
    },
    showMembers: function (logId, param) {
        closePopup();
        var width = 184, height = 300;
        var row = this.getRow(logId);
        if (row.GroupPolicy.Members.length > 0) {
            $("#tbodyUserLogGroupMembersEffected").empty();
            for (var i = 0; i < row.GroupPolicy.Members.length; i++) {
                $("#scriptTmpUserLogGroupMembersEffected").tmpl([row.GroupPolicy.Members[i]]).appendTo($("#tbodyUserLogGroupMembersEffected"));
            }
        }
        else {
            $("#scriptTmpUserLogMainGrid_NoInfo").tmpl([{ noinfo: true}]).appendTo($("#tbodyUserLogGroupMembersEffected").empty());
        }
        var dialogHeight = 350;
        var gridHeight = (row.GroupPolicy.Members.length * 50);
        if (gridHeight < dialogHeight) dialogHeight = gridHeight;
        if (dialogHeight < 200) dialogHeight = 200;
        showPopup({
            content: $("#divUserLogMembersEffected"),
            title: "Group Member : " + param,
            width: 250,
            height: dialogHeight,
            multiple: false, dialog: true
        });
        $("#divUserLogMembersEffected").show().reRenderScroller(width, height, true);

        var dialogHeight = $("#divUserLogMembersEffected").height() + 100;

        closePopup();
        showPopup({
            content: $("#divUserLogMembersEffected"),
            title: "Group Member : " + param,
            width: 250,
            height: dialogHeight,
            multiple: false, dialog: true
        });
    }
};

userLog.groupPolicy = {
    buildTableTree: function (table, t, l) {
        var level = 1;
        if (typeof l != "undefined") level = l + 1;
        if (typeof table == "undefined") return [];
        var tableTmp = [];
        if (typeof t != "undefined") tableTmp = t;
        for (var i = 0; i < table.length; ++i) {
            tableTmp.push({
                FuncDesc: table[i].FuncDesc,
                FuncName: table[i].FuncName,
                GroupName: table[i].GroupName,
                ID: table[i].ID,
                IsGroupHost: table[i].IsGroupHost,
                IsGroupRead: table[i].IsGroupRead,
                ParentId: table[i].ParentId,
                level: level,
                TotalSub: table[i].TotalSub
            });
            if (table[i].TotalSub == 0) continue;
            this.buildTableTree(table[i].SubFuncs, tableTmp, level);
        }
        return tableTmp;
    },
    setValue: function (row, arrTableTree) {
        var setVal = function (values, type) {
            var arr = values.split(";");
            for (var i = 0; i < arr.length; ++i) {
                var keyValue = arr[i].split(",");
                var funcId = keyValue[0];
                var value = keyValue[1];
                for (var j = 0; j < arrTableTree.length; ++j) {
                    if (arrTableTree[j].ID.toString() == funcId) {
                        if (value == "3") {
                            arrTableTree[j]["IsGroupRead" + type] = true;
                            arrTableTree[j]["IsGroupHost" + type] = true;
                        }
                        else if (value == "0") {
                            arrTableTree[j]["IsGroupRead" + type] = false;
                            arrTableTree[j]["IsGroupHost" + type] = false;
                        }
                        else {
                            arrTableTree[j]["IsGroupRead" + type] = true;
                            arrTableTree[j]["IsGroupHost" + type] = false;
                        }
                        break;
                    }
                }
            }
        };
        setVal(row.GroupPolicy.PoliciesOld, "Old");
        setVal(row.GroupPolicy.PoliciesNew, "New");
    },
    remove: function (row, json, tableTree) {
        var arr = row.GroupPolicy.PoliciesOld.split(";");
        var funcIds = [];
        var arrDoNotRemove = [];
        for (var i = 0; i < arr.length; ++i) {
            var t = arr[i].split(",");
            funcIds.push(t[0]);
            arrDoNotRemove.push(t[0]);
        }
        var append = function (_json, funcId) {
            if (_json == null) _json = json;
            for (var k = 0; k < _json.length; ++k) {
                if (_json[k].ID.toString() == funcId) {
                    if (!userLog.groupPolicy.isExisted(arrDoNotRemove, funcId)) {
                        arrDoNotRemove.push(funcId);
                    }
                    if (_json[k].ParentId == 0) return;
                    append(null, _json[k].ParentId.toString());
                }
                else {
                    if (_json[k].TotalSub == 0) continue;
                    append(_json[k].SubFuncs, funcId);
                }
            }
        };
        for (var j = 0; j < funcIds.length; ++j) {
            append(json, funcIds[j]);
        }
        var isExisted = function (id) {
            for (var p = 0; p < arrDoNotRemove.length; ++p) {
                if (arrDoNotRemove[p] == id) return true;
            }
            return false;
        };
        for (var h = 0; h < tableTree.length; ++h) {
            if (!isExisted(tableTree[h].ID)) {
                tableTree[h].hide = true;
            }
            else {
                tableTree[h].hide = false;
            }
        }
        return arrDoNotRemove.length;
    },
    isExisted: function (arr, id) {
        for (var i = 0; i < arr.length; ++i) {
            if (arr[i] == id) return true;
        }
        return false;
    },
    show: function (row, param) {
        this.row = row;
        var selectId = row.AccId;

        //any groupid, do not important, just need function tree list.
        if (window.viewModel.Departments.length > 0) {
            selectId = window.viewModel.Departments[0].Id;
        }

        if (row.ActionId == 1709) {
            if (row.TypeId == 1) {
                param = row.GroupPolicy.GroupName;
            }
        }

        if (row.ActionId == 1700) {
            var tableHeader = $("#divUserLogGroupPolicy").find("table[class='tbl-header-scroll']");
            tableHeader.find("div[before1]").hide();
            tableHeader.find("div[before2]").show();
            tableHeader.find("div[before2][from]").find("span").html(row.GroupPolicy.GroupNameOld);
            tableHeader.find("div[before2][to]").find("span").html(row.GroupPolicy.GroupNameNew);
        }
        else {
            var tableHeader = $("#divUserLogGroupPolicy").find("table[class='tbl-header-scroll']");
            tableHeader.find("div[before1]").show();
            tableHeader.find("div[before2]").hide();
        }
        if (row.GroupPolicy.PoliciesOld == "") {
            closePopup();
            showPopup({
                content: $("#divUserLogGroupPolicy"),
                title: "Group Permissions : " + param,
                width: 840,
                height: 300,
                multiple: false, dialog: true
            });
            $("#scriptTmpUserLogGroupPolicy_NoInfo").tmpl([{ noinfo: true}]).appendTo($("#tbodyUserLogDeptPolicy").empty());
            $("#divUserLogGroupPolicy").show().reRenderScroller(775, 543, true);
            return;
        }
        userLog.utils.blockUI();
        $.post("GetGroupPolicyList", { groupId: selectId }, function (json) {
            var arr = userLog.groupPolicy.buildTableTree(json);
            userLog.groupPolicy.setValue(row, arr);
            var rowCount = userLog.groupPolicy.remove(row, json, arr);
            $("#scriptTemplateUserLogDeptPolicy").tmpl(arr).appendTo($("#tbodyUserLogDeptPolicy").empty());
            $("#tbodyUserLogDeptPolicy").find("tr[class='hidden']").remove();
            $("#tbodyUserLogDeptPolicy").find("tr").each(function () {
                var hiddenLevel = $(this).find("input[level]");
                var hTotalSub = $(this).find("input[TotalSub]");
                var spanIcon = $(this).find("span[icon]");
                var spanFuncName = $(this).find("span[funcname]");
                if (parseInt($(hTotalSub).val()) != 0) {
                    $(spanFuncName).css("font-weight", "bold");
                }
                accManager.dept.buildLevelPadding(spanIcon, $(hiddenLevel).val());
            });
            closePopup();
            showPopup({
                content: $("#divUserLogGroupPolicy"),
                title: "Group Permissions : " + param,
                width: 840,
                height: 400,
                multiple: false, dialog: true
            });

            $("#divUserLogGroupPolicy").show().reRenderScroller(775, 400, true);

            var dialogHeight = $("#divUserLogGroupPolicy").height() + 100;

            closePopup();
            showPopup({
                content: $("#divUserLogGroupPolicy"),
                title: "Group Permissions : " + param,
                width: 840,
                height: dialogHeight,
                multiple: false, dialog: true
            });

            $("#tbodyUserLogDeptPolicy").find("tr").each(function () {
                $(this).mouseover(function () {
                    $(this).addClass("mouseon");
                })
                .mouseout(function () {
                    $(this).removeClass("mouseon");
                });
            });
            //init expand , collapse tree
            $("#tbodyUserLogDeptPolicy").find("tr span[icon]").click(function () {
                var tr = $(this).parents("tr:first");
                var level = parseInt($(tr).attr("level"));
                if ($(this).hasClass("tree-expanded")) {
                    $(this).removeClass("tree-expanded").addClass("tree-expand2");
                    while (
                    parseInt($(tr).next("tr").attr("level")) > level
                    ) {
                        tr = $(tr).next("tr");
                        $(tr).fadeOut(500, function () {
                            $("#divUserLogGroupPolicy").reRenderScroller(775, 543, true);
                        });
                    }
                }
                else {
                    $(this).removeClass("tree-expand2").addClass("tree-expanded");
                    while (
                    parseInt($(tr).next("tr").attr("level")) > level
                    ) {
                        tr = $(tr).next("tr");
                        var spanIcon = $(tr).find("span[icon]");
                        if ($(spanIcon).hasClass("tree-expand2")) {
                            $(spanIcon).removeClass("tree-expand2").addClass("tree-expanded");
                        }
                        $(tr).show();
                    }
                    $("#divUserLogGroupPolicy").reRenderScroller(775, 543, true);
                }
            });
            userLog.utils.unBlock();
        });
    }
};

userLog.adminPolicy = {
    buildTableTree: function (table, t, l) {
        var level = 1;
        if (typeof l != "undefined") level = l + 1;
        if (typeof table == "undefined") return [];
        var tableTmp = [];
        if (typeof t != "undefined") tableTmp = t;
        for (var i = 0; i < table.length; ++i) {
            tableTmp.push({
                FuncDesc: table[i].FuncDesc,
                FuncName: table[i].FuncName,
                GroupName: table[i].GroupName,
                ID: table[i].ID,
                IsGroupHost: table[i].IsGroupHost,
                IsGroupRead: table[i].IsGroupRead,
                ParentId: table[i].ParentId,
                level: level,
                TotalSub: table[i].TotalSub
            });
            if (table[i].TotalSub == 0) continue;
            this.buildTableTree(table[i].SubFuncs, tableTmp, level);
        }
        return tableTmp;
    },
    setValue: function (row, arrTableTree) {
        var setVal = function (values, type) {
            var arr = values.split(";");
            for (var i = 0; i < arr.length; ++i) {
                var keyValue = arr[i].split(",");
                var funcId = keyValue[0];
                var value = keyValue[1];
                for (var j = 0; j < arrTableTree.length; ++j) {
                    if (arrTableTree[j].ID.toString() == funcId) {
                        if (value == "3") {
                            arrTableTree[j]["IsRead" + type] = true;
                            arrTableTree[j]["IsHost" + type] = true;
                        }
                        else if (value == "0") {
                            arrTableTree[j]["IsRead" + type] = false;
                            arrTableTree[j]["IsHost" + type] = false;
                        }
                        else {
                            arrTableTree[j]["IsRead" + type] = true;
                            arrTableTree[j]["IsHost" + type] = false;
                        }
                        break;
                    }
                }
            }
        };
        setVal(row.AdminPolicy.PoliciesOld, "Old");
        setVal(row.AdminPolicy.PoliciesNew, "New");
    },
    remove: function (row, json, tableTree) {
        var arr = row.AdminPolicy.PoliciesOld.split(";");
        var funcIds = [];
        var arrDoNotRemove = [];
        for (var i = 0; i < arr.length; ++i) {
            var t = arr[i].split(",");
            funcIds.push(t[0]);
            arrDoNotRemove.push(t[0]);
        }
        var append = function (_json, funcId) {
            if (_json == null) _json = json;
            for (var k = 0; k < _json.length; ++k) {
                if (_json[k].ID.toString() == funcId) {
                    if (!userLog.groupPolicy.isExisted(arrDoNotRemove, funcId)) {
                        arrDoNotRemove.push(funcId);
                    }
                    if (_json[k].ParentId == 0) return;
                    append(null, _json[k].ParentId.toString());
                }
                else {
                    if (_json[k].TotalSub == 0) continue;
                    append(_json[k].SubFuncs, funcId);
                }
            }
        };
        for (var j = 0; j < funcIds.length; ++j) {
            append(json, funcIds[j]);
        }
        var isExisted = function (id) {
            for (var p = 0; p < arrDoNotRemove.length; ++p) {
                if (arrDoNotRemove[p] == id) return true;
            }
            return false;
        };
        for (var h = 0; h < tableTree.length; ++h) {
            if (!isExisted(tableTree[h].ID)) {
                tableTree[h].hide = true;
            }
            else {
                tableTree[h].hide = false;
            }
        }
        return arrDoNotRemove.length;
    },
    isExisted: function (arr, id) {
        for (var i = 0; i < arr.length; ++i) {
            if (arr[i] == id) return true;
        }
        return false;
    },
    show: function (row, param) {
        this.row = row;
        var selectId = row.AccId;
        if (row.AdminPolicy.PoliciesOld == "") {
            closePopup();
            showPopup({
                content: $("#divUserLogAdminPolicy"),
                title: "Admin Individual Permissions : " + param,
                width: 840,
                height: 300,
                multiple: false, dialog: true
            });
            $("#scriptTmpUserLogAdminPolicy_NoInfo").tmpl([{ noinfo: true}]).appendTo($("#tbodyUserLogAdminPolicy").empty());
            $("#divUserLogAdminPolicy").show().reRenderScroller(775, 543, true);
            return;
        }
        userLog.utils.blockUI();
        $.post("GetUserPolicyList", { userId: selectId }, function (json) {
            var arr = userLog.adminPolicy.buildTableTree(json);
            userLog.adminPolicy.setValue(row, arr);
            var rowCount = userLog.adminPolicy.remove(row, json, arr);
            $("#scriptTemplateUserLogAdminPolicy").tmpl(arr).appendTo($("#tbodyUserLogAdminPolicy").empty());
            $("#tbodyUserLogAdminPolicy").find("tr[class='hidden']").remove();
            $("#tbodyUserLogAdminPolicy").find("tr").each(function () {
                var hiddenLevel = $(this).find("input[level]");
                var hTotalSub = $(this).find("input[TotalSub]");
                var spanIcon = $(this).find("span[icon]");
                var spanFuncName = $(this).find("span[funcname]");
                if (parseInt($(hTotalSub).val()) != 0) {
                    $(spanFuncName).css("font-weight", "bold");
                }
                accManager.dept.buildLevelPadding(spanIcon, $(hiddenLevel).val());
            });

            closePopup();
            showPopup({
                content: $("#divUserLogAdminPolicy"),
                title: "Admin Individual Permissions : " + param,
                width: 840,
                height: 400,
                multiple: false, dialog: true
            });

            $("#divUserLogAdminPolicy").show().reRenderScroller(775, 400, true);

            var dialogHeight = $("#divUserLogAdminPolicy").height() + 100;

            closePopup();
            showPopup({
                content: $("#divUserLogAdminPolicy"),
                title: "Admin Individual Permissions : " + param,
                width: 840,
                height: dialogHeight,
                multiple: false, dialog: true
            });

            $("#tbodyUserLogAdminPolicy").find("tr").each(function () {
                $(this).mouseover(function () {
                    $(this).addClass("mouseon");
                })
                .mouseout(function () {
                    $(this).removeClass("mouseon");
                });
            });
            //init expand , collapse tree
            $("#tbodyUserLogAdminPolicy").find("tr span[icon]").click(function () {
                var tr = $(this).parents("tr:first");
                var level = parseInt($(tr).attr("level"));
                if ($(this).hasClass("tree-expanded")) {
                    $(this).removeClass("tree-expanded").addClass("tree-expand2");
                    while (
                    parseInt($(tr).next("tr").attr("level")) > level
                    ) {
                        tr = $(tr).next("tr");
                        $(tr).fadeOut(500, function () {
                            $("#divUserLogAdminPolicy").reRenderScroller(775, 543, true);
                        });
                    }
                }
                else {
                    $(this).removeClass("tree-expand2").addClass("tree-expanded");
                    while (
                    parseInt($(tr).next("tr").attr("level")) > level
                    ) {
                        tr = $(tr).next("tr");
                        var spanIcon = $(tr).find("span[icon]");
                        if ($(spanIcon).hasClass("tree-expand2")) {
                            $(spanIcon).removeClass("tree-expand2").addClass("tree-expanded");
                        }
                        $(tr).show();
                    }
                    $("#divUserLogAdminPolicy").reRenderScroller(775, 543, true);
                }
            });
            userLog.utils.unBlock();
        });
    }
};

userLog.utils = {
    blockUI: function () {
        if (parent.parent.BlockUI) {
            try { parent.parent.BlockUI(); } catch (e) { }
        }
        else {
            try { parent.BlockUI(); } catch (e) { }
        }
    },
    unBlock: function () {
        if (parent.parent.$ && parent.parent.$.unblockUI) {
            try { parent.parent.$.unblockUI(); } catch (e) { }
        }
        else {
            try { parent.$.unblockUI(); } catch (e) { }
        }
    }
};

$(document).ready(function () {
    userLog.init();
});
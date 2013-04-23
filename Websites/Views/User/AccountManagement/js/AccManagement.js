var accManager = {
};

accManager.init = function () {
    this.dept.init();
    this.tabs.init();
    this.accounts.init();
    this.others.init();
};

accManager.tabs = {
    init: function () {
        for (var i = 1; i <= 6; i++) {
            $("#left_tab1_" + i).click(function () {
                for (var j = 1; j <= 6; j++) {
                    $("#left_tab1_" + j).removeAttr("class");
                }
                $(this).attr("class", "current");
                for (var k = 1; k <= 6; k++) {
                    $("#left_tab1_" + k + "_content").hide();
                }
                $("#" + $(this).attr("id") + "_content").show();
                accManager.utils.hideMsg();
            });
        }
        $("#left_tab1_1").click(function () {
            $("#tbxSearcDept").val("");
            window.accManager.dept.renderGrid(window.viewModel.Departments);
            $("#divDeptFilter").show();
            $("#divAccFilter").hide();
            $("#divMainHeader").height(63);
        });
        $("#left_tab1_2").click(function () {
            $("#divDeptFilter").hide();
            $("#divAccFilter").hide();
            $("#divMainHeader").height(30);
        });
        $("#left_tab1_3").click(function () {
            accManager.accounts.fillDeptComboBoxs();
            $("#divDeptFilter").hide();
            $("#divAccFilter").hide();
            $("#divMainHeader").height(30);
        });
        $("#left_tab1_4").click(function () {
            accManager.accounts.fillDeptComboBoxs();
            $("#divDeptFilter").hide();
            $("#divAccFilter").hide();
            $("#divMainHeader").height(30);
        });
        $("#left_tab1_5").click(function () {
            $("#divHeaderUserName").removeClass("asc").removeClass("desc");
            $("#txtSearchAcc").val("");
            $("#listAlpha").val("All");
            $("#divMainHeader").height(61);
            $("#divDeptFilter").hide();
            $("#divAccPath").hide();
            $("#divAccFilter").show();
            window.accManager.accounts.deptId = 0;
            window.accManager.accounts.vmFiltering = null;
            window.accManager.accounts.pageIndex = 0;
            window.accManager.accounts.renderGrid(window.accManager.accounts.concatVM4Paging());
        });
        $("#left_tab1_6").click(function () {
            $("#divDeptFilter").hide();
            $("#divAccFilter").hide();
            $("#divMainHeader").height(30);
        });
    }
};

accManager.accounts = {
    width: 1190,
    height: 750,
    pageIndex: 0,
    pageSize: 100,
    deptId: 0,
    AllFuncIds: [],
    getValueOfFuncId: function (arrPolicies, funcId) {
        for (var i = 0; i < arrPolicies.length; ++i) {
            var dic = arrPolicies[i].split(",");
            if (dic[0] == funcId.toString()) {
                return dic[1];
            }
        }
        return "";
    },
    getChangedData: function () {
        var arrBeforeEdit = this.policiesDataBeforeEdit.split(";");
        var arrEdited = this.getCurrentPoliciesData().split(";");

        var arrOldPoliciesData = [];
        var arrPoliciesEdited = [];

        for (var i = 0; i < this.AllFuncIds.length; ++i) {
            var valueBeforeEdit = this.getValueOfFuncId(arrBeforeEdit, this.AllFuncIds[i]);
            var valueEdited = this.getValueOfFuncId(arrEdited, this.AllFuncIds[i]);
            if (valueBeforeEdit != valueEdited) {
                arrOldPoliciesData.push(this.AllFuncIds[i] + "," + valueBeforeEdit);
                arrPoliciesEdited.push(this.AllFuncIds[i] + "," + valueEdited);
            }
        }

        var result = {
            policiesDataBeforeEdit: arrOldPoliciesData.join(";"),
            policiesDataEdited: arrPoliciesEdited.join(";")
        };

        var arrOld = result.policiesDataBeforeEdit.split(";");
        var arrNew = result.policiesDataEdited.split(";");

        for (var m = 0; m < arrOldPoliciesData.length; ++m) {
            var keyValues1 = arrOldPoliciesData[m].split(",");
            this.appendParentAtOld(arrOld, keyValues1[0]);
        }

        for (var n = 0; n < arrPoliciesEdited.length; ++n) {
            var keyValues2 = arrPoliciesEdited[n].split(",");
            this.appendParentAtNew(arrNew, keyValues2[0]);
        }

        result = {
            policiesDataBeforeEdit: arrOld.join(";"),
            policiesDataEdited: arrNew.join(";")
        };

        return result;
    },
    appendParentAtOld: function (arr, funcId) {
        var node = this.findNodeInFunctionTrees(this.functionTrees, funcId);
        if (node == null || node.ParentId == 0) return;
        var parentNode = this.findNodeInFunctionTrees(this.functionTrees, node.ParentId.toString());
        var isExisted = false;
        for (var i = 0; i < arr.length; ++i) {
            var keyValues = arr[i].split(",");
            if (keyValues[0] == parentNode.ID.toString()) isExisted = true;
        }
        if (!isExisted) {
            var arrBeforeEdit = this.policiesDataBeforeEdit.split(";");
            for (var j = 0; j < arrBeforeEdit.length; ++j) {
                var keyValues2 = arrBeforeEdit[j].split(",");
                if (keyValues2[0] == parentNode.ID.toString()) {
                    arr.push(arrBeforeEdit[j]);
                    break;
                }
            }
        }
        this.appendParentAtOld(arr, parentNode.ID.toString());
    },
    appendParentAtNew: function (arr, funcId) {
        var node = this.findNodeInFunctionTrees(this.functionTrees, funcId);
        if (node == null || node.ParentId == 0) return;
        var parentNode = this.findNodeInFunctionTrees(this.functionTrees, node.ParentId.toString());
        var isExisted = false;
        for (var i = 0; i < arr.length; ++i) {
            var keyValues = arr[i].split(",");
            if (keyValues[0] == parentNode.ID.toString()) isExisted = true;
        }
        if (!isExisted) {
            var arrEdited = this.getCurrentPoliciesData().split(";");
            for (var j = 0; j < arrEdited.length; ++j) {
                var keyValues2 = arrEdited[j].split(",");
                if (keyValues2[0] == parentNode.ID.toString()) {
                    arr.push(arrEdited[j]);
                    break;
                }
            }
        }
        this.appendParentAtNew(arr, parentNode.ID.toString());
    },
    findNodeInFunctionTrees: function (trees, funcId) {
        for (var j = 0; j < trees.length; ++j) {
            if (funcId == trees[j].ID.toString()) {
                return trees[j];
            }
            else {
                if (trees[j].TotalSub == 0) continue;
                var node = this.findNodeInFunctionTrees(trees[j].SubFuncs, funcId);
                if (node != null) return node;
            }
        }
        return null;
    },
    getCurrentPoliciesData: function () {
        this.FuncIds = [];
        $("#tbodyUserPol").find("tr").each(function () {
            var cbxRead = $(this).find("input[cbxRead]");
            var cbxHost = $(this).find("input[cbxHost]");
            var funcId = $(this).find("input[funcid]").val();
            if (!cbxRead.get(0).checked && !cbxHost.get(0).checked) return;
            if (cbxRead.get(0).checked && cbxHost.get(0).checked) {
                accManager.accounts.FuncIds.push(funcId + "," + 3);
            }
            else if (cbxRead.get(0).checked) {
                accManager.accounts.FuncIds.push(funcId + "," + 1);
            }
            else if (cbxHost.get(0).checked) {
                accManager.accounts.FuncIds.push(funcId + "," + 2);
            }
        });
        for (var i = 0; i < this.AllFuncIds.length; ++i) {
            if (!this.isExisted(this.AllFuncIds[i])) {
                this.FuncIds.push(this.AllFuncIds[i] + "," + 0);
            }
        }
        return this.FuncIds.join(";");
    },
    isExisted: function (id) {
        for (var i = 0; i < this.FuncIds.length; ++i) {
            if (parseInt(this.FuncIds[i].split(',')[0]) == id) return true;
        }
        return false;
    },
    policy: function (custId, deptName, deptId, userName) {
        accManager.utils.blockUI();
        accManager.accounts.AllFuncIds = [];
        $.post("GetUserPolicyList", { userId: custId }, $.proxy(function (json) {
            accManager.accounts.functionTrees = json;
            closePopup();
            showPopup
                ({
                    content: $("#userPolicyGrid"),
                    title: "Account: " + userName,
                    width: 839,
                    height: 462,
                    buttons: [{
                        "text": "Update",
                        callback: function (popuid) {
                            accManager.accounts.updateUserPolicy(custId, popuid);
                        }
                    }], multiple: false, dialog: true,
                    onclose: function () {
                        if (accManager.utils.getMessageType() == "alertboxerr") {
                            accManager.utils.hideMsg();
                        }
                    }
                });
            this.bindDataPolicy(json);
            accManager.accounts.policiesDataBeforeEdit = accManager.accounts.getCurrentPoliciesData();
            accManager.utils.unBlock();
        }, this));
    },
    updateUserPolicy: function (custId, popuid) {
        var data = this.getChangedData();
        if (data.policiesDataEdited == "") {
            accManager.utils.showErr("There is no change!");
            return;
        }
        var param = {
            memberPolicies: data.policiesDataEdited,
            oldMemberPolicies: data.policiesDataBeforeEdit,
            custId: custId
        };
        accManager.utils.blockUI();
        $.post("UpdateAdminPolicy", param, function (data) {
            if (parseInt(data.ErrorCode) != 0) {
                accManager.utils.showErr(data.ErrorMessage);
            }
            else {
                accManager.utils.showSuccess("Update successfully!");
                accManager.accounts.policiesDataBeforeEdit = accManager.accounts.getCurrentPoliciesData();
            }
            accManager.utils.unBlock();
            closePopup(popuid);
        });
    },
    bindDataPolicy: function (table) {
        var width = 775;
        var height = 343;
        $("#scriptTempalteUserPolicy").tmpl(this.buildTableTree(table)).appendTo($("#tbodyUserPol").empty());
        $("#tbodyUserPol").find("tr").each(function () {
            var hiddenLevel = $(this).find("input[level]");
            var hTotalSub = $(this).find("input[TotalSub]");
            var spanIcon = $(this).find("span[icon]");
            var spanFuncName = $(this).find("span[funcname]");
            if (parseInt($(hTotalSub).val()) != 0) {
                $(spanFuncName).css("font-weight", "bold");
            }
            accManager.accounts.buildLevelPadding(spanIcon, $(hiddenLevel).val());
        });
        $("#userPolicyGrid").removeClass("hidden").reRenderScroller(width, height, true);
        this.initGridUserPolEvent();
    },
    haveOneCheckedInSameLevelAndParent: function (cbx, tr) {
        var trsSame = $("#tbodyUserPol").find("tr[parentID='" + $(tr).attr("parentID") + "']");
        var isHaveOneChecked = false;
        $(trsSame).each(function () {
            if ($(cbx).attr("cbxRead") == "true") {
                if ($(this).find("input[cbxRead]").get(0).checked == true) {
                    isHaveOneChecked = true;
                }
            }
            else {
                if ($(this).find("input[cbxHost]").get(0).checked == true) {
                    isHaveOneChecked = true;
                }
            }
        });
        return isHaveOneChecked;
    },
    initGridUserPolEvent: function () {
        var width = 775;
        var height = 343;
        $("#tbodyUserPol").find("tr").each(function () {
            $(this).mouseover(function () {
                $(this).addClass("mouseon");
            })
            .mouseout(function () {
                $(this).removeClass("mouseon");
            });
        });

        //init expand , collapse tree
        $("#tbodyUserPol").find("tr span[icon]").click(function () {
            var tr = $(this).parents("tr:first");
            var level = parseInt($(tr).attr("level"));
            if ($(this).hasClass("tree-expanded")) {
                $(this).removeClass("tree-expanded").addClass("tree-expand2");
                while (
                parseInt($(tr).next("tr").attr("level")) > level
                ) {
                    tr = $(tr).next("tr");
                    $(tr).fadeOut(500, function () {
                        $("#policyGrid").reRenderScroller(width, height, true);
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
                $("#policyGrid").reRenderScroller(width, height, true);
            }
        });

        //init handler for checkbox's click event
        $("#tbodyUserPol").find("input[type='checkbox']").each(function () {
            $(this).change(function () {
                var cbx = this;
                var childFuncTr;
                var parentFuncTr;
                var tr;
                if (this.checked) {
                    tr = $(this).parents("tr:first");
                    if ($(this).attr("cbxHost") == "true") {
                        $(tr).find("input[cbxRead]").get(0).checked = this.checked;
                    }
                    //all parents will be checked
                    if ($(tr).attr("parentID") != "0") {
                        parentFuncTr = $("#tbodyUserPol").find("tr[_id='" + $(tr).attr("parentID") + "']");
                        if ($(this).attr("cbxRead") == "true") {
                            $(parentFuncTr).find("input[cbxRead]").get(0).checked = true;
                        }
                        else {
                            $(parentFuncTr).find("input[cbxHost]").get(0).checked = true;
                            $(parentFuncTr).find("input[cbxRead]").get(0).checked = true;
                        }
                        parentFuncTr = $("#tbodyUserPol").find("tr[_id='" + $(parentFuncTr).attr("parentID") + "']");
                        while (parentFuncTr.length > 0) {
                            if ($(this).attr("cbxRead") == "true") {
                                $(parentFuncTr).find("input[cbxRead]").get(0).checked = true;
                            }
                            else {
                                $(parentFuncTr).find("input[cbxHost]").get(0).checked = true;
                                $(parentFuncTr).find("input[cbxRead]").get(0).checked = true;
                            }
                            parentFuncTr = $("#tbodyUserPol").find("tr[_id='" + $(parentFuncTr).attr("parentID") + "']");
                        }
                    }
                    //all childs will be checked
                    if ($(tr).attr("TotalSub") != "0") {
                        childFuncTr = $("#tbodyUserPol").find("tr[parentID='" + $(tr).attr("_id") + "']");
                        $(childFuncTr).each(function () {
                            if ($(cbx).attr("cbxRead") == "true") {
                                $(this).find("input[cbxRead]").get(0).checked = true;
                            }
                            else {
                                $(this).find("input[cbxHost]").get(0).checked = true;
                                $(this).find("input[cbxRead]").get(0).checked = true;
                            }
                        });
                        $(childFuncTr).each(function () {
                            childFuncTr = $("#tbodyUserPol").find("tr[parentID='" + $(this).attr("_id") + "']");
                            while (childFuncTr.length > 0) {
                                $(childFuncTr).each(function () {
                                    if ($(cbx).attr("cbxRead") == "true") {
                                        $(this).find("input[cbxRead]").get(0).checked = true;
                                    }
                                    else {
                                        $(this).find("input[cbxHost]").get(0).checked = true;
                                        $(this).find("input[cbxRead]").get(0).checked = true;
                                    }
                                });
                                childFuncTr = $("#tbodyUserPol").find("tr[parentID='" + $(childFuncTr).attr("_id") + "']");
                            }
                        });
                    }
                }
                else {
                    tr = $(this).parents("tr:first"); //all parents will be un-check

                    //------------------------------------------------
                    //section 1
                    //find all tr which have same level and same parent
                    if (!accManager.accounts.haveOneCheckedInSameLevelAndParent(cbx, tr)) {
                        if ($(tr).attr("parentID") != "0") {
                            parentFuncTr = $("#tbodyUserPol").find("tr[_id='" + $(tr).attr("parentID") + "']");
                            if ($(this).attr("cbxRead") == "true") {
                                $(parentFuncTr).find("input[cbxRead]").get(0).checked = false;
                            }
                            else {
                                $(parentFuncTr).find("input[cbxHost]").get(0).checked = false;
                            }
                            if (!accManager.accounts.haveOneCheckedInSameLevelAndParent(cbx, parentFuncTr)) {
                                parentFuncTr = $("#tbodyUserPol").find("tr[_id='" + $(parentFuncTr).attr("parentID") + "']");
                                while (parentFuncTr.length > 0) {
                                    if ($(this).attr("cbxRead") == "true") {
                                        $(parentFuncTr).find("input[cbxRead]").get(0).checked = false;
                                    }
                                    else {
                                        $(parentFuncTr).find("input[cbxHost]").get(0).checked = false;
                                    }
                                    parentFuncTr = $("#tbodyUserPol").find("tr[_id='" + $(parentFuncTr).attr("parentID") + "']");
                                    if (parentFuncTr.length < 0 || accManager.accounts.haveOneCheckedInSameLevelAndParent(cbx, parentFuncTr)) {
                                        break;
                                    }
                                }
                            }
                        }
                    }
                    //--end
                    //------------------------------------------------

                    //------------------------------------------------
                    //section 2
                    //if not read, then must not host
                    accManager.accounts.uncheckedHost($(tr).find("input[cbxHost]"));
                    //------------------------------------------------

                    //all childs will be checked
                    if ($(tr).attr("TotalSub") != "0") {
                        childFuncTr = $("#tbodyUserPol").find("tr[parentID='" + $(tr).attr("_id") + "']");
                        $(childFuncTr).each(function () {
                            if ($(cbx).attr("cbxRead") == "true") {
                                $(this).find("input[cbxRead]").get(0).checked = false;
                            }
                            else {
                                $(this).find("input[cbxHost]").get(0).checked = false;
                            }
                        });
                        $(childFuncTr).each(function () {
                            childFuncTr = $("#tbodyUserPol").find("tr[parentID='" + $(this).attr("_id") + "']");
                            while (childFuncTr.length > 0) {
                                $(childFuncTr).each(function () {
                                    if ($(cbx).attr("cbxRead") == "true") {
                                        $(this).find("input[cbxRead]").get(0).checked = false;
                                    }
                                    else {
                                        $(this).find("input[cbxHost]").get(0).checked = false;
                                    }
                                });
                                childFuncTr = $("#tbodyUserPol").find("tr[parentID='" + $(childFuncTr).attr("_id") + "']");
                            }
                        });
                    }
                }
            });
        });
    },
    uncheckedHost: function (cbx) {
        $(cbx).get(0).checked = false;
        var tr = $(cbx).parents("tr:first");
        //all parents will be un-check

        //------------------------------------------------
        //section 1
        //find all tr which have same level and same parent
        if (!accManager.accounts.haveOneCheckedInSameLevelAndParent(cbx, tr)) {
            if ($(tr).attr("parentID") != "0") {
                var parentFuncTr = $("#tbodyUserPol").find("tr[_id='" + $(tr).attr("parentID") + "']");
                if ($(cbx).attr("cbxRead") == "true") {
                    $(parentFuncTr).find("input[cbxRead]").get(0).checked = false;
                }
                else {
                    $(parentFuncTr).find("input[cbxHost]").get(0).checked = false;
                }
                if (!accManager.accounts.haveOneCheckedInSameLevelAndParent(cbx, parentFuncTr)) {
                    parentFuncTr = $("#tbodyUserPol").find("tr[_id='" + $(parentFuncTr).attr("parentID") + "']");
                    while (parentFuncTr.length > 0) {
                        if ($(cbx).attr("cbxRead") == "true") {
                            $(parentFuncTr).find("input[cbxRead]").get(0).checked = false;
                        }
                        else {
                            $(parentFuncTr).find("input[cbxHost]").get(0).checked = false;
                        }
                        parentFuncTr = $("#tbodyUserPol").find("tr[_id='" + $(parentFuncTr).attr("parentID") + "']");
                        if (parentFuncTr.length < 0 || accManager.accounts.haveOneCheckedInSameLevelAndParent(cbx, parentFuncTr)) {
                            break;
                        }
                    }
                }
            }
        }
        //--end
        //------------------------------------------------

        //all childs will be checked
        if ($(tr).attr("TotalSub") != "0") {
            var childFuncTr = $("#tbodyUserPol").find("tr[parentID='" + $(tr).attr("_id") + "']");
            $(childFuncTr).each(function () {
                if ($(cbx).attr("cbxRead") == "true") {
                    $(this).find("input[cbxRead]").get(0).checked = false;
                }
                else {
                    $(this).find("input[cbxHost]").get(0).checked = false;
                }
            });
            $(childFuncTr).each(function () {
                childFuncTr = $("#tbodyUserPol").find("tr[parentID='" + $(this).attr("_id") + "']");
                while (childFuncTr.length > 0) {
                    $(childFuncTr).each(function () {
                        if ($(cbx).attr("cbxRead") == "true") {
                            $(this).find("input[cbxRead]").get(0).checked = false;
                        }
                        else {
                            $(this).find("input[cbxHost]").get(0).checked = false;
                        }
                    });
                    childFuncTr = $("#tbodyUserPol").find("tr[parentID='" + $(childFuncTr).attr("_id") + "']");
                }
            });
        }
    },
    buildTableTree: function (table, t, l) {
        var level = 1;
        if (typeof l != "undefined") level = l + 1;
        if (typeof table == "undefined") return [];
        var tableTmp = [];
        if (typeof t != "undefined") tableTmp = t;
        for (var i = 0; i < table.length; ++i) {
            this.AllFuncIds.push(table[i].ID);
            tableTmp.push({
                FuncDesc: table[i].FuncDesc,
                FuncName: table[i].FuncName,
                GroupName: table[i].GroupName,
                ID: table[i].ID,
                IsGroupHost: table[i].IsGroupHost,
                IsGroupRead: table[i].IsGroupRead,
                IsRead: table[i].IsRead,
                IsHost: table[i].IsHost,
                ParentId: table[i].ParentId,
                level: level,
                TotalSub: table[i].TotalSub
            });
            if (table[i].TotalSub == 0) continue;
            this.buildTableTree(table[i].SubFuncs, tableTmp, level);
        }
        return tableTmp;
    },
    buildLevelPadding: function (span, level) {
        level = parseInt(level);
        var marginLeft = 10;
        switch (level) {
            case 2:
                $(span).css("margin-left", marginLeft + "px");
                break;
            case 3:
                $(span).css("margin-left", marginLeft + 12 + "px");
                break;
            case 4:
                $(span).css("margin-left", marginLeft + 24 + "px");
                break;
        }
    },
    addAcc: function () {
        var userName = $("#tbxUserNameNewAcc").val();
        var firstName = $("#tbxFirstNameNewAcc").val();
        var lastName = $("#tbxLastNameNewAcc").val();
        var password = $("#tbxPassNewAcc").val();
        var deptId = $("#selDeptNewAcc").val();

        if (accManager.utils.isEmpty(userName)) {
            accManager.utils.showErr("Please input Username!");
            return;
        }

        if (!accManager.utils.validateStringInput(userName)) {
            accManager.utils.showErr("User name is invalid!");
            return;
        }

        if (!accManager.utils.validateStringInput2(firstName)) {
            accManager.utils.showErr("First name is invalid!");
            return;
        }

        if (!accManager.utils.validateStringInput2(lastName)) {
            accManager.utils.showErr("Last name is invalid!");
            return;
        }

        if (accManager.utils.isEmpty(password)) {
            accManager.utils.showErr("Please input password!");
            return;
        }

        if (!accManager.utils.validateStringInput(password)) {
            accManager.utils.showErr("Password is invalid!");
            return;
        }

        if (deptId == "0") {
            accManager.utils.showErr("Please choice department!");
            return;
        }

        var param = {
            userName: userName,
            firstName: accManager.utils.encodeSpecifiedCharacter(firstName),
            lastName: accManager.utils.encodeSpecifiedCharacter(lastName),
            groupId: deptId,
            password: password
        };

        accManager.utils.blockUI();
        $.post("AddAccount", param,
            $.proxy(function (data) {
                if (parseInt(data.ErrorCode) != 0) {
                    accManager.utils.showErr(data.ErrorMessage);
                    accManager.utils.unBlock();
                }
                else {
                    accManager.utils.showSuccess("Add new account (" + userName + ") successfully!");
                    $("#tbxUserNameNewAcc").val("");
                    $("#tbxFirstNameNewAcc").val("");
                    $("#tbxLastNameNewAcc").val("");
                    $("#tbxPassNewAcc").val("");
                    setTimeout(function () {
                        accManager.accounts.addSuccessCallBack();
                        accManager.utils.unBlock();
                    }, 800);
                }
            }, this));
    },
    addExistedAcc: function () {
        var custName = $("#tbxUsernameExisted").val();
        var deptId = $("#selDeptExistedAcc").val();

        if (accManager.utils.isEmpty(custName)) {
            accManager.utils.showErr("Please input Username!");
            return;
        }

        if (deptId == "0") {
            accManager.utils.showErr("Please choice department!");
            return;
        }

        var param = {
            custName: custName,
            groupId: deptId
        };

        accManager.utils.blockUI();
        $.post("AddExistedAccount", param,
            $.proxy(function (data) {
                if (parseInt(data.ErrorCode) != 0) {
                    accManager.utils.showErr(data.ErrorMessage);
                    accManager.utils.unBlock();
                }
                else {
                    accManager.utils.showSuccess("Add existed account (" + custName + ") successfully!");
                    $("#tbxUsernameExisted").val("");
                    setTimeout(function () {
                        accManager.accounts.addSuccessCallBack();
                        accManager.utils.unBlock();
                    }, 800);
                }
            }, this));
    },
    addSuccessCallBack: function () {
        $.post("GetDepts", function (json) {
            viewModel.Departments = json;
        });
        $.post("GetAllAccount", $.proxy(function (json) {
            viewModel.Accounts = json;
            if (typeof this.vmFiltering != "undefined" && this.vmFiltering != null) {
                if (this.deptId != 0) {
                    var arr = jlinq.from(window.viewModel.Accounts).equals("DeptId", this.deptId).select();
                    this.fillVmFiltering(arr);
                } else {
                    this.fillVmFiltering(window.viewModel.Accounts);
                }
            }
        }, this));
    },
    filterByDepartment: function (deptId) {
        $("#txtSearchAcc").val("");
        $("#listAlpha").val("All");
        $("#divMainHeader").height(61);
        $("#left_tab1_1").removeAttr("class");
        $("#left_tab1_1_content").hide();
        $("#left_tab1_5").attr("class", "current");
        $("#left_tab1_5_content").show();
        $("#divDeptFilter").hide();
        $("#divAccPath").hide();
        $("#divAccFilter").show();
        this.deptId = deptId;
        var arrDept = jlinq.from(window.viewModel.Departments).equals("Id", this.deptId).select();
        var deptName = "";
        if (arrDept.length > 0) deptName = arrDept[0].Name;
        var arr = jlinq.from(window.viewModel.Accounts).equals("DeptId", this.deptId.toString()).select();

        $("#divAccPath").html("<span><label>Department >> </label> <span style='color:#0066CC'>" + deptName + "</span></span>").show();
        this.fillVmFiltering(arr);
        this.pageIndex = 0;
        this.renderGrid(this.concatVM4Paging());

    },
    init: function () {
        $("#txtSearchAcc").blur(function () {
            if ($(this).val() == "") $(this).val("All");
        }).focus(function () {
            if ($(this).val() == "All") $(this).val("");
        }).keyup(function (e) {
            if (e.keyCode == 13) {
                accManager.accounts.search();
            }
        });
        $("#btnSearcAcc").click(function () {
            accManager.accounts.search();
        });
        this.fillDeptComboBoxs();
    },
    fillDeptComboBoxs: function () {
        var arr = [];
        arr.push("<option value='0'>[Select]</option>");
        for (var i = 0; i < window.viewModel.Departments.length; i++) {
            arr.push("<option value='" + window.viewModel.Departments[i].Id + "'>" + window.viewModel.Departments[i].Name + "</option>");
        }
        $("#selDeptNewAcc").empty().append(arr.join(""));
        $("#selDeptExistedAcc").empty().append(arr.join(""));
    },
    createDeptDropDownBox: function () {
        var arr = [];
        for (var i = 0; i < window.viewModel.Departments.length; i++) {
            arr.push("<option value='" + window.viewModel.Departments[i].Id + "'>" + window.viewModel.Departments[i].Name + "</option>");
        }
        return "<select editor='true' style='width:80px'>" + arr.join("") + "</select>";
    },
    sortByUserName: function (e) {
        var target = e.target || e.srcElement;
        var tag;
        if (!target.sortOrder) {
            tag = "UserName";
            target.sortOrder = true;
            $(target).removeClass("asc").removeClass("desc").addClass("asc");
        }
        else {
            tag = "-UserName";
            target.sortOrder = false;
            $(target).removeClass("desc").removeClass("asc").addClass("desc");
        }
        if (typeof this.vmFiltering == "undefined" || this.vmFiltering == null) {
            jlinq.from(window.viewModel.Accounts).sort(tag);
        }
        else {
            jlinq.from(this.vmFiltering).sort(tag);
        }
        this.renderGrid(this.concatVM4Paging());
    },
    fillVmFiltering: function (arrAcc) {
        var keyWord = accManager.utils.removeSpecifiedCharacter(accManager.utils.trimStartEnd($("#txtSearchAcc").val()));
        var firstLetter = $("#listAlpha").val().replace(/ /g, "");
        var arr, i;
        if (this.deptId == 0) {
            if (keyWord == "All" || keyWord == "") {
                if (firstLetter == "All") {
                    this.vmFiltering = null;
                    return;
                }
            }
        }
        else {
            if (keyWord == "All" || keyWord == "") {
                if (firstLetter == "All") {
                    this.vmFiltering = arrAcc;
                    return;
                }
            }
        }

        if (keyWord == "All" || keyWord == "") {
            if (firstLetter == "All") {
                this.vmFiltering = null;
            }
            else if (firstLetter == "0-9") {
                arr = [];
                for (i = 0; i < 10; ++i) {
                    arr.concat(jlinq.from(arrAcc).starts("UserName", i.toString()).select());
                }
                this.vmFiltering = arr;
            }
            else {
                this.vmFiltering = jlinq.from(arrAcc).starts("UserName", firstLetter).select();
            }
        }
        else {
            if (firstLetter == "All") {
                this.vmFiltering = jlinq.from(arrAcc).contains("UserName", keyWord).select();
            }
            else if (firstLetter == "0-9") {
                arr = [];
                for (i = 0; i < 10; ++i) {
                    arr.concat(jlinq.from(arrAcc).starts("UserName", i.toString()).select());
                }
                this.vmFiltering = jlinq.from(arr).contains("UserName", keyWord).select();
            }
            else {
                arr = jlinq.from(arrAcc).starts("UserName", firstLetter).select();
                this.vmFiltering = jlinq.from(arr).contains("UserName", keyWord).select();
            }
        }
    },
    search: function () {
        $("#divHeaderUserName").removeClass("asc").removeClass("desc");
        this.fillVmFiltering(window.viewModel.Accounts);
        this.pageIndex = 0;
        this.renderGrid(this.concatVM4Paging());
        $("#divAccPath").hide();
    },
    renderGrid: function (arr) {
        if (arr.length > 0) {
            $("#tbodyViewAllAcc").empty();
            for (var i = 0; i < arr.length; i++) {
                $("#scriptTmpAcc").tmpl([arr[i]]).appendTo($("#tbodyViewAllAcc"));
            }
            $("#divViewAllAcc").find("div[paging]").show();
            $("#divViewAllAcc").removeClass("hidden");
            this.initPager(this.getCurrentVMLength(), this.pageSize, this.pageIndex);
            var timerId = setTimeout(function () {
                $("#tbodyViewAllAcc").find("tr").mouseover(function () {
                    $(this).find("td").css("background-color", "#FFDDFE");
                }).mouseout(function () {
                    $(this).find("td").css("background-color", "");
                });
                clearTimeout(timerId);
            }, 200);
        }
        else {
            $("#scriptTmpAcc_NoInfo").tmpl([{ noinfo: true}]).appendTo($("#tbodyViewAllAcc").empty());
            $("#divViewAllAcc").removeClass("hidden");
            $("#divViewAllAcc").find("div[paging]").hide();
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
            $("#pagination").pagination(totalItem, opt);
        else
            $("#pagination").hide();
    },
    pageSelectCallback: function (pageIndex, itemsPerPage) {
        var maxPage = Math.ceil(this.getCurrentVMLength() / itemsPerPage);
        pageIndex = pageIndex < maxPage ? pageIndex : maxPage - 1;

        this.pageSize = parseInt(itemsPerPage);
        this.pageIndex = pageIndex;
        this.renderGrid(this.concatVM4Paging());
        return false;
    },
    getCurrentVMLength: function () {
        if (typeof this.vmFiltering == "undefined" || this.vmFiltering == null)
            return window.viewModel.Accounts.length;
        else return this.vmFiltering.length;
    },
    concatVM4Paging: function () {
        var vm;
        if (typeof this.vmFiltering == "undefined" || this.vmFiltering == null)
            vm = window.viewModel.Accounts;
        else vm = this.vmFiltering;

        if (vm.length <= this.pageSize) {
            return vm;
        }
        var vmTmp = [];

        var start = this.pageIndex * this.pageSize;
        var end;

        if (vm.length < (start + this.pageSize)) {
            end = vm.length;
        }
        else {
            end = start + this.pageSize;
        }

        for (var i = start; i < end; ++i) {
            vmTmp.push(vm[i]);
        }
        return vmTmp;
    },
    editClick: function (e, id) {
        var target = e.target || e.srcElement;

        if ($(target).parents("tr:first").find("input[editor]").length > 0) return;

        var colFirstName = $(target).parents("tr:first").find("div[firstname]").hide();
        var colLastName = $(target).parents("tr:first").find("div[lastname]").hide();
        var colDept = $(target).parents("tr:first").find("div[dept]").hide();

        colFirstName.parent().append("<input type='text' style='width:196px' editor='true' firstname='true' />").find("input[editor]").val(accManager.utils.decodeSpecifiedCharacter(accManager.utils.trimStartEnd(colFirstName.html())));
        colLastName.parent().append("<input type='text' style='width:196px' editor='true' lastname='true' />").find("input[editor]").val(accManager.utils.decodeSpecifiedCharacter(accManager.utils.trimStartEnd(colLastName.html())));

        var arr = jlinq.from(window.viewModel.Accounts).equals("Id", id).select();
        if (arr.length > 0) {
            colDept.parent().append(this.createDeptDropDownBox()).find("select").val(arr[0].DeptId);
        }

        $(target).parents("tr:first").find("img[alt='Edit']").hide();
        $(target).parents("tr:first").find("img[alt='Save']").show();
        $(target).parents("tr:first").find("img[alt='Cancel']").show();
    },
    saveClick: function (e, id) {
        var target = e.target || e.srcElement;
        this.saveEdit($(target).parents("tr:first"), id);
    },
    cancelClick: function (e) {
        var target = e.target || e.srcElement;

        $(target).parents("tr:first").find("input[editor]").remove();
        $(target).parents("tr:first").find("select[editor]").remove();
        $(target).parents("tr:first").find("div[firstname]").show();
        $(target).parents("tr:first").find("div[lastname]").show();
        $(target).parents("tr:first").find("div[dept]").show();

        $(target).parents("tr:first").find("img[alt='Edit']").show();
        $(target).parents("tr:first").find("img[alt='Save']").hide();
        $(target).parents("tr:first").find("img[alt='Cancel']").hide();
    },
    saveEdit: function (tr, id) {
        var firstName = tr.find("input[editor][firstname]").val();
        var lastName = tr.find("input[editor][lastname]").val();

        if (!accManager.utils.validateStringInput2(firstName)) {
            accManager.utils.showErr("First name is invalid!");
            return;
        }

        if (!accManager.utils.validateStringInput2(lastName)) {
            accManager.utils.showErr("Last name is invalid!");
            return;
        }

        var params = [];
        params.push({
            "Name": "custid",
            "Value": id
        });
        params.push({
            "Name": "groupid",
            "Value": tr.find("select[editor]").val()
        });
        params.push({
            "Name": "firstname",
            "Value": accManager.utils.encodeSpecifiedCharacter(firstName)
        });
        params.push({
            "Name": "lastname",
            "Value": accManager.utils.encodeSpecifiedCharacter(lastName)
        });
        accManager.utils.blockUI();
        $.ajax({
            url: 'UpdateAccount',
            type: 'POST',
            dataType: 'json',
            data: $.toJSON({ postParams: params }),
            contentType: 'application/json; charset=utf-8',
            success: $.proxy(function (data) {
                if (parseInt(data.ErrorCode) != 0) {
                    accManager.utils.showErr(data.ErrorMessage);
                    accManager.utils.unBlock();
                } else {
                    this.successCallBack();
                }
            }, this)
        });
    },
    changeStatus: function (id, e) {
        var target = e.target || e.srcElement;
        var param = {
            accId: id,
            closed: !target.checked
        };
        accManager.utils.blockUI();
        $.post("ChangeAccountStatus", param,
            $.proxy(function (data) {
                if (parseInt(data.ErrorCode) != 0) {
                    accManager.utils.showErr(data.ErrorMessage);
                    accManager.utils.unBlock();
                }
                else {
                    this.successCallBack();
                }
            }, this));
    },
    successCallBack: function () {
        var timerId = setTimeout($.proxy(function () {
            $.post("GetAllAccount", $.proxy(function (json) {
                window.viewModel.Accounts = json;
                if (typeof this.vmFiltering != "undefined" && this.vmFiltering != null) {
                    if (this.deptId != 0) {
                        var arr = jlinq.from(window.viewModel.Accounts).equals("DeptId", this.deptId).select();
                        this.fillVmFiltering(arr);
                    } else {
                        this.fillVmFiltering(window.viewModel.Accounts);
                    }
                }
                this.renderGrid(this.concatVM4Paging());
                accManager.utils.unBlock();
                accManager.utils.showSuccess("Update successfully!");
                $("#divHeaderUserName").removeClass("asc").removeClass("desc");
            }, this));
            $.post("GetDepts", function (json) {
                window.viewModel.Departments = json;
            });
            clearTimeout(timerId);
        }, this), 1500);
    }
};

accManager.dept = {
    width: 718,
    height: 608,
    FuncIds: [],
    AllFuncIds: [],
    haveOneCheckedInSameLevelAndParent: function (cbx, tr) {
        var trsSame = $("#tbodyDeptPolicy").find("tr[parentID='" + $(tr).attr("parentID") + "']");
        var isHaveOneChecked = false;
        $(trsSame).each(function () {
            if ($(cbx).attr("cbxRead") == "true") {
                if ($(this).find("input[cbxRead]").get(0).checked == true) {
                    isHaveOneChecked = true;
                }
            }
            else {
                if ($(this).find("input[cbxHost]").get(0).checked == true) {
                    isHaveOneChecked = true;
                }
            }
        });
        return isHaveOneChecked;
    },
    uncheckedHost: function (cbx) {
        $(cbx).get(0).checked = false;
        var tr = $(cbx).parents("tr:first");
        //all parents will be un-check

        //------------------------------------------------
        //section 1
        //find all tr which have same level and same parent

        if (!this.haveOneCheckedInSameLevelAndParent(cbx, tr)) {
            if ($(tr).attr("parentID") != "0") {
                var parentFuncTr = $("#tbodyDeptPolicy").find("tr[_id='" + $(tr).attr("parentID") + "']");
                if ($(cbx).attr("cbxRead") == "true") {
                    $(parentFuncTr).find("input[cbxRead]").get(0).checked = false;
                }
                else {
                    $(parentFuncTr).find("input[cbxHost]").get(0).checked = false;
                }
                if (!this.haveOneCheckedInSameLevelAndParent(cbx, parentFuncTr)) {
                    parentFuncTr = $("#tbodyDeptPolicy").find("tr[_id='" + $(parentFuncTr).attr("parentID") + "']");
                    while (parentFuncTr.length > 0) {
                        if ($(cbx).attr("cbxRead") == "true") {
                            $(parentFuncTr).find("input[cbxRead]").get(0).checked = false;
                        }
                        else {
                            $(parentFuncTr).find("input[cbxHost]").get(0).checked = false;
                        }
                        parentFuncTr = $("#tbodyDeptPolicy").find("tr[_id='" + $(parentFuncTr).attr("parentID") + "']");
                        if (parentFuncTr.length < 0 || this.haveOneCheckedInSameLevelAndParent(cbx, parentFuncTr)) {
                            break;
                        }
                    }
                }
            }
        }
        //--end
        //------------------------------------------------

        //all childs will be checked
        if ($(tr).attr("TotalSub") != "0") {
            var childFuncTr = $("#tbodyDeptPolicy").find("tr[parentID='" + $(tr).attr("_id") + "']");
            $(childFuncTr).each(function () {
                if ($(cbx).attr("cbxRead") == "true") {
                    $(this).find("input[cbxRead]").get(0).checked = false;
                }
                else {
                    $(this).find("input[cbxHost]").get(0).checked = false;
                }
            });
            $(childFuncTr).each(function () {
                childFuncTr = $("#tbodyDeptPolicy").find("tr[parentID='" + $(this).attr("_id") + "']");
                while (childFuncTr.length > 0) {
                    $(childFuncTr).each(function () {
                        if ($(cbx).attr("cbxRead") == "true") {
                            $(this).find("input[cbxRead]").get(0).checked = false;
                        }
                        else {
                            $(this).find("input[cbxHost]").get(0).checked = false;
                        }
                    });
                    childFuncTr = $("#tbodyDeptPolicy").find("tr[parentID='" + $(childFuncTr).attr("_id") + "']");
                }
            });
        }
    },
    buildLevelPadding: function (span, level) {
        level = parseInt(level);
        var marginLeft = 10;
        switch (level) {
            case 2:
                $(span).css("margin-left", marginLeft + "px");
                break;
            case 3:
                $(span).css("margin-left", marginLeft + 12 + "px");
                break;
            case 4:
                $(span).css("margin-left", marginLeft + 24 + "px");
                break;
        }
    },
    buildTableTree: function (table, t, l) {
        var level = 1;
        if (typeof l != "undefined") level = l + 1;
        if (typeof table == "undefined") return [];
        var tableTmp = [];
        if (typeof t != "undefined") tableTmp = t;
        for (var i = 0; i < table.length; ++i) {
            this.AllFuncIds.push(table[i].ID);
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
    policy: function (groupId, groupName) {
        accManager.utils.blockUI();
        accManager.dept.AllFuncIds = [];
        $.post("GetGroupPolicyList", { groupId: groupId }, function (json) {
            accManager.dept.functionTrees = json;
            $("#scriptTemplateDeptPolicy").tmpl(accManager.dept.buildTableTree(json)).appendTo($("#tbodyDeptPolicy").empty());
            $("#tbodyDeptPolicy").find("tr").each(function () {
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
            showPopup
            ({
                content: $("#deptPolicyGrid"),
                title: "Department: " + groupName,
                width: 635,
                height: 462,
                buttons: [{
                    "text": "Update",
                    callback: function (popuid) {
                        accManager.dept.updateGroupPolicy(groupId, popuid);
                    }
                }],
                multiple: false, dialog: true,
                onclose: function () {
                    if (accManager.utils.getMessageType() == "alertboxerr") {
                        accManager.utils.hideMsg();
                    }
                }
            });

            $("#deptPolicyGrid").removeClass("hidden").reRenderScroller(569, 343, true);

            accManager.dept.policiesDataBeforeEdit = accManager.dept.getCurrentPoliciesData();

            $("#tbodyDeptPolicy").find("tr").each(function () {
                $(this).mouseover(function () {
                    $(this).addClass("mouseon");
                })
                .mouseout(function () {
                    $(this).removeClass("mouseon");
                });
            });
            //init expand , collapse tree
            $("#tbodyDeptPolicy").find("tr span[icon]").click(function () {
                var tr = $(this).parents("tr:first");
                var level = parseInt($(tr).attr("level"));
                if ($(this).hasClass("tree-expanded")) {
                    $(this).removeClass("tree-expanded").addClass("tree-expand2");
                    while (
                    parseInt($(tr).next("tr").attr("level")) > level
                    ) {
                        tr = $(tr).next("tr");
                        //$(tr).hide();
                        $(tr).fadeOut(500, function () {
                            $("#deptPolicyGrid").reRenderScroller(569, 343, true);
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
                    $("#deptPolicyGrid").reRenderScroller(569, 343, true);
                }
            });
            //init handler for checkbox's click event
            $("#tbodyDeptPolicy").find("input[type='checkbox']").each(function () {
                $(this).change(function () {
                    var cbx = this;
                    var parentFuncTr;
                    var tr;
                    var childFuncTr;
                    if (this.checked) {
                        tr = $(this).parents("tr:first");
                        if ($(this).attr("cbxHost") == "true") {
                            $(tr).find("input[cbxRead]").get(0).checked = this.checked;
                        }
                        //all parents will be checked
                        if ($(tr).attr("parentID") != "0") {
                            parentFuncTr = $("#tbodyDeptPolicy").find("tr[_id='" + $(tr).attr("parentID") + "']");
                            if ($(this).attr("cbxRead") == "true") {
                                $(parentFuncTr).find("input[cbxRead]").get(0).checked = true;
                            }
                            else {
                                $(parentFuncTr).find("input[cbxHost]").get(0).checked = true;
                                $(parentFuncTr).find("input[cbxRead]").get(0).checked = true;
                            }
                            parentFuncTr = $("#tbodyDeptPolicy").find("tr[_id='" + $(parentFuncTr).attr("parentID") + "']");
                            while (parentFuncTr.length > 0) {
                                if ($(this).attr("cbxRead") == "true") {
                                    $(parentFuncTr).find("input[cbxRead]").get(0).checked = true;
                                }
                                else {
                                    $(parentFuncTr).find("input[cbxHost]").get(0).checked = true;
                                    $(parentFuncTr).find("input[cbxRead]").get(0).checked = true;
                                }
                                parentFuncTr = $("#tbodyDeptPolicy").find("tr[_id='" + $(parentFuncTr).attr("parentID") + "']");
                            }
                        }
                        //all childs will be checked
                        if ($(tr).attr("TotalSub") != "0") {
                            childFuncTr = $("#tbodyDeptPolicy").find("tr[parentID='" + $(tr).attr("_id") + "']");
                            $(childFuncTr).each(function () {
                                if ($(cbx).attr("cbxRead") == "true") {
                                    $(this).find("input[cbxRead]").get(0).checked = true;
                                }
                                else {
                                    $(this).find("input[cbxHost]").get(0).checked = true;
                                    $(this).find("input[cbxRead]").get(0).checked = true;
                                }
                            });
                            $(childFuncTr).each(function () {
                                childFuncTr = $("#tbodyDeptPolicy").find("tr[parentID='" + $(this).attr("_id") + "']");
                                while (childFuncTr.length > 0) {
                                    $(childFuncTr).each(function () {
                                        if ($(cbx).attr("cbxRead") == "true") {
                                            $(this).find("input[cbxRead]").get(0).checked = true;
                                        }
                                        else {
                                            $(this).find("input[cbxHost]").get(0).checked = true;
                                            $(this).find("input[cbxRead]").get(0).checked = true;
                                        }
                                    });
                                    childFuncTr = $("#tbodyDeptPolicy").find("tr[parentID='" + $(childFuncTr).attr("_id") + "']");
                                }
                            });
                        }
                    }
                    else {
                        tr = $(this).parents("tr:first"); //all parents will be un-check

                        //------------------------------------------------
                        //section 1
                        //find all tr which have same level and same parent

                        if (!accManager.dept.haveOneCheckedInSameLevelAndParent(cbx, tr)) {
                            if ($(tr).attr("parentID") != "0") {
                                parentFuncTr = $("#tbodyDeptPolicy").find("tr[_id='" + $(tr).attr("parentID") + "']");
                                if ($(this).attr("cbxRead") == "true") {
                                    $(parentFuncTr).find("input[cbxRead]").get(0).checked = false;
                                }
                                else {
                                    $(parentFuncTr).find("input[cbxHost]").get(0).checked = false;
                                }
                                if (!accManager.dept.haveOneCheckedInSameLevelAndParent(cbx, parentFuncTr)) {
                                    parentFuncTr = $("#tbodyDeptPolicy").find("tr[_id='" + $(parentFuncTr).attr("parentID") + "']");
                                    while (parentFuncTr.length > 0) {
                                        if ($(this).attr("cbxRead") == "true") {
                                            $(parentFuncTr).find("input[cbxRead]").get(0).checked = false;
                                        }
                                        else {
                                            $(parentFuncTr).find("input[cbxHost]").get(0).checked = false;
                                        }
                                        parentFuncTr = $("#tbodyDeptPolicy").find("tr[_id='" + $(parentFuncTr).attr("parentID") + "']");
                                        if (parentFuncTr.length < 0 || accManager.dept.haveOneCheckedInSameLevelAndParent(cbx, parentFuncTr)) {
                                            break;
                                        }
                                    }
                                }
                            }
                        }
                        //--end
                        //------------------------------------------------

                        //------------------------------------------------
                        //section 2
                        //if not read, then must not host
                        accManager.dept.uncheckedHost($(tr).find("input[cbxHost]"));
                        //------------------------------------------------

                        //all childs will be checked
                        if ($(tr).attr("TotalSub") != "0") {
                            childFuncTr = $("#tbodyDeptPolicy").find("tr[parentID='" + $(tr).attr("_id") + "']");
                            $(childFuncTr).each(function () {
                                if ($(cbx).attr("cbxRead") == "true") {
                                    $(this).find("input[cbxRead]").get(0).checked = false;
                                }
                                else {
                                    $(this).find("input[cbxHost]").get(0).checked = false;
                                }
                            });
                            $(childFuncTr).each(function () {
                                childFuncTr = $("#tbodyDeptPolicy").find("tr[parentID='" + $(this).attr("_id") + "']");
                                while (childFuncTr.length > 0) {
                                    $(childFuncTr).each(function () {
                                        if ($(cbx).attr("cbxRead") == "true") {
                                            $(this).find("input[cbxRead]").get(0).checked = false;
                                        }
                                        else {
                                            $(this).find("input[cbxHost]").get(0).checked = false;
                                        }
                                    });
                                    childFuncTr = $("#tbodyDeptPolicy").find("tr[parentID='" + $(childFuncTr).attr("_id") + "']");
                                }
                            });
                        }
                    }
                });
            });

            accManager.utils.unBlock();
        });
    },
    updateGroupPolicy: function (groupId, popuid) {
        var data = this.getChangedData();
        if (data.policiesDataEdited == "") {
            accManager.utils.showErr("There is no change!");
            return;
        }
        var param = {
            groupPolicies: data.policiesDataEdited,
            oldGroupPolicies: data.policiesDataBeforeEdit,
            groupId: groupId
        };
        accManager.utils.blockUI();
        $.post("UpdateGroupPolicy", param, function (data) {
            if (parseInt(data.ErrorCode) != 0) {
                accManager.utils.showErr(data.ErrorMessage);
            }
            else {
                accManager.utils.showSuccess("Update successfully!");
                accManager.dept.policiesDataBeforeEdit = accManager.dept.getCurrentPoliciesData();
            }
            closePopup(popuid);
            accManager.utils.unBlock();
        });
    },
    getCurrentPoliciesData: function () {
        this.FuncIds = [];
        $("#tbodyDeptPolicy").find("tr").each(function () {
            var cbxRead = $(this).find("input[cbxRead]");
            var cbxHost = $(this).find("input[cbxHost]");
            var funcID = $(this).find("input[funcid]").val();
            if (!cbxRead.get(0).checked && !cbxHost.get(0).checked) return;
            if (cbxRead.get(0).checked && cbxHost.get(0).checked) {
                accManager.dept.FuncIds.push(funcID + "," + 3);
            }
            else if (cbxRead.get(0).checked) {
                accManager.dept.FuncIds.push(funcID + "," + 1);
            }
            else if (cbxHost.get(0).checked) {
                accManager.dept.FuncIds.push(funcID + "," + 2);
            }
        });
        for (var i = 0; i < accManager.dept.AllFuncIds.length; ++i) {
            if (!this.isExisted(accManager.dept.AllFuncIds[i])) {
                accManager.dept.FuncIds.push(accManager.dept.AllFuncIds[i] + "," + 0);
            }
        }
        return accManager.dept.FuncIds.join(";");
    },
    getChangedData: function () {
        var arrBeforeEdit = this.policiesDataBeforeEdit.split(";");
        var arrEdited = this.getCurrentPoliciesData().split(";");

        var arrOldPoliciesData = [];
        var arrPoliciesEdited = [];

        var _funcIds = [];

        var isExisted = function (funcId) {
            for (var i = 0; i < _funcIds.length; ++i) {
                if (_funcIds[i] == funcId) return true;
            }
            return false;
        };

        for (var i = 0; i < this.AllFuncIds.length; ++i) {
            if (isExisted(this.AllFuncIds[i])) continue;
            var valueBeforeEdit = this.getValueOfFuncId(arrBeforeEdit, this.AllFuncIds[i]);
            var valueEdited = this.getValueOfFuncId(arrEdited, this.AllFuncIds[i]);
            if (valueBeforeEdit != valueEdited) {
                _funcIds.push(this.AllFuncIds[i]);
                arrOldPoliciesData.push(this.AllFuncIds[i] + "," + valueBeforeEdit);
                arrPoliciesEdited.push(this.AllFuncIds[i] + "," + valueEdited);
            }
        }

        var result = {
            policiesDataBeforeEdit: arrOldPoliciesData.join(";"),
            policiesDataEdited: arrPoliciesEdited.join(";")
        };

        var arrOld = result.policiesDataBeforeEdit.split(";");
        var arrNew = result.policiesDataEdited.split(";");

        for (var m = 0; m < arrOldPoliciesData.length; ++m) {
            var keyValues1 = arrOldPoliciesData[m].split(",");
            this.appendParentAtOld(arrOld, keyValues1[0]);
        }

        for (var n = 0; n < arrPoliciesEdited.length; ++n) {
            var keyValues2 = arrPoliciesEdited[n].split(",");
            this.appendParentAtNew(arrNew, keyValues2[0]);
        }

        result = {
            policiesDataBeforeEdit: arrOld.join(";"),
            policiesDataEdited: arrNew.join(";")
        };

        return result;
    },
    appendParentAtOld: function (arr, funcId) {
        var node = this.findNodeInFunctionTrees(this.functionTrees, funcId);
        if (node == null || node.ParentId == 0) return;
        var parentNode = this.findNodeInFunctionTrees(this.functionTrees, node.ParentId.toString());
        var isExisted = false;
        for (var i = 0; i < arr.length; ++i) {
            var keyValues = arr[i].split(",");
            if (keyValues[0] == parentNode.ID.toString()) isExisted = true;
        }
        if (!isExisted) {
            var arrBeforeEdit = this.policiesDataBeforeEdit.split(";");
            for (var j = 0; j < arrBeforeEdit.length; ++j) {
                var keyValues2 = arrBeforeEdit[j].split(",");
                if (keyValues2[0] == parentNode.ID.toString()) {
                    arr.push(arrBeforeEdit[j]);
                    break;
                }
            }
        }
        this.appendParentAtOld(arr, parentNode.ID.toString());
    },
    appendParentAtNew: function (arr, funcId) {
        var node = this.findNodeInFunctionTrees(this.functionTrees, funcId);
        if (node == null || node.ParentId == 0) return;
        var parentNode = this.findNodeInFunctionTrees(this.functionTrees, node.ParentId.toString());
        var isExisted = false;
        for (var i = 0; i < arr.length; ++i) {
            var keyValues = arr[i].split(",");
            if (keyValues[0] == parentNode.ID.toString()) isExisted = true;
        }
        if (!isExisted) {
            var arrEdited = this.getCurrentPoliciesData().split(";");
            for (var j = 0; j < arrEdited.length; ++j) {
                var keyValues2 = arrEdited[j].split(",");
                if (keyValues2[0] == parentNode.ID.toString()) {
                    arr.push(arrEdited[j]);
                    break;
                }
            }
        }
        this.appendParentAtNew(arr, parentNode.ID.toString());
    },
    findNodeInFunctionTrees: function (trees, funcId) {
        for (var j = 0; j < trees.length; ++j) {
            if (funcId == trees[j].ID.toString()) {
                return trees[j];
            }
            else {
                if (trees[j].TotalSub == 0) continue;
                var node = this.findNodeInFunctionTrees(trees[j].SubFuncs, funcId);
                if (node != null) return node;
            }
        }
        return null;
    },
    getValueOfFuncId: function (arrPolicies, funcId) {
        for (var i = 0; i < arrPolicies.length; ++i) {
            var dic = arrPolicies[i].split(",");
            if (dic[0] == funcId.toString()) {
                return dic[1];
            }
        }
        return "";
    },
    isExisted: function (id) {
        for (var i = 0; i < this.FuncIds.length; ++i) {
            if (parseInt(this.FuncIds[i].split(',')[0]) == id) return true;
        }
        return false;
    },
    addDept: function () {
        var name = $("#tbxDeptNameNew").val();
        var desc = $("#tbxDeptDescNew").val();

        if (accManager.utils.isEmpty(name)) {
            accManager.utils.showErr("Please input Department Name!");
            return;
        }

        if (!accManager.utils.validateStringInput(name)) {
            accManager.utils.showErr("Department name is invalid!");
            return;
        }

        if (!accManager.utils.validateStringInput2(desc)) {
            accManager.utils.showErr("Department description is invalid!");
            return;
        }
        var param = {
            name: name,
            description: accManager.utils.encodeSpecifiedCharacter(desc)
        };
        accManager.utils.blockUI();
        $.post("AddDept", param,
            function (data) {
                if (parseInt(data.ErrorCode) != 0) {
                    accManager.utils.showErr(data.ErrorMessage);
                    accManager.utils.unBlock();
                }
                else {
                    accManager.utils.showSuccess("Add new department (" + name + ") successfully!");
                    $("#tbxDeptNameNew").val("");
                    $("#tbxDeptDescNew").val("");
                    setTimeout(function () {
                        $.post("GetDepts", function (json) {
                            viewModel.Departments = json;
                            accManager.utils.unBlock();
                        });
                    }, 800);
                }
            });
    },
    init: function () {
        accManager.dept.renderGrid(window.viewModel.Departments);

        $("#tbxSearcDept").blur(function () {
            if ($(this).val() == "") $(this).val("All");
        }).focus(function () {
            if ($(this).val() == "All") $(this).val("");
        }).keyup(function (e) {
            if (e.keyCode == 13) {
                accManager.dept.search();
            }
        });
        $("#btnSearchDept").click(function () {
            accManager.dept.search();
        });
    },
    search: function () {
        $("#divHeaderDeptName").removeClass("asc").removeClass("desc");
        $("#divHeaderNoOfUser").removeClass("asc").removeClass("desc");
        var text = $("#tbxSearcDept").val().replace(/ /g, "") != "All" ? accManager.utils.trimStartEnd($("#tbxSearcDept").val()) : "";
        var arr = jlinq.from(window.viewModel.Departments).starts("Name", text).select();
        accManager.dept.renderGrid(arr);
    },
    renderGrid: function (arr) {
        if (arr.length > 0) {
            $("#scriptTmpDepts").tmpl(arr).appendTo($("#tbodyViewAllDept").empty());
            $("#divViewAllDept").removeClass("hidden");
            var timerId = setTimeout(function () {
                $("#tbodyViewAllDept").find("tr").mouseover(function () {
                    $(this).find("td").css("background-color", "#FFDDFE");
                }).mouseout(function () {
                    $(this).find("td").css("background-color", "");
                });
                clearTimeout(timerId);
            }, 200);
        }
        else {
            $("#scriptTmpDepts_NoInfo").tmpl([{ noinfo: true}]).appendTo($("#tbodyViewAllDept").empty());
            $("#divViewAllDept").removeClass("hidden");
        }
    },
    editClick: function (e) {
        var target = e.target || e.srcElement;

        if ($(target).parents("tr:first").find("input[editor]").length > 0) return;

        var colName = $(target).parents("tr:first").find("div[deptname]").hide();
        var colDesc = $(target).parents("tr:first").find("div[deptdesc]").hide();

        colName.parent().append("<input type='text' style='width:116px' editor='true' deptname='true'/>").find("input[editor]").val(accManager.utils.decodeSpecifiedCharacter(accManager.utils.trimStartEnd(colName.html())));
        colDesc.parent().append("<input type='text' style='width:296px' editor='true' deptdesc='true' />").find("input[editor]").val(accManager.utils.decodeSpecifiedCharacter(accManager.utils.trimStartEnd(colDesc.html())));

        $(target).parent().find("img[alt='Edit']").hide();
        $(target).parent().find("img[alt='Save']").show();
        $(target).parent().find("img[alt='Cancel']").show();
    },
    saveClick: function (e, id) {
        var target = e.target || e.srcElement;
        this.saveEdit($(target).parents("tr:first"), id);
    },
    cancelClick: function (e) {
        var target = e.target || e.srcElement;
        $(target).parents("tr:first").find("input[editor]").remove();
        $(target).parents("tr:first").find("div[deptname]").show();
        $(target).parents("tr:first").find("div[deptdesc]").show();

        $(target).parent().find("img[alt='Edit']").show();
        $(target).parent().find("img[alt='Save']").hide();
        $(target).parent().find("img[alt='Cancel']").hide();
    },
    saveEdit: function (tr, id) {
        var name = tr.find("input[editor][deptname]").val();
        var desc = tr.find("input[editor][deptdesc]").val();

        if (!accManager.utils.validateStringInput(name)) {
            accManager.utils.showErr("Department name is invalid!");
            return;
        }

        if (!accManager.utils.validateStringInput2(desc)) {
            accManager.utils.showErr("Department description is invalid!");
            return;
        }

        var param = {
            id: id,
            name: name,
            description: accManager.utils.encodeSpecifiedCharacter(desc)
        };
        accManager.utils.blockUI();
        $.post("UpdateDept", param,
            function (data) {
                if (parseInt(data.ErrorCode) != 0) {
                    accManager.utils.showErr(data.ErrorMessage);
                    accManager.utils.unBlock();
                }
                else {
                    var colName = tr.find("div[deptname]");
                    var colDesc = tr.find("div[deptdesc]");

                    colName.html(name).show();
                    colDesc.html(desc).show();

                    tr.find("input[editor]").remove();
                    tr.find("img[alt='Edit']").show();
                    tr.find("img[alt='Save']").hide();
                    tr.find("img[alt='Cancel']").hide();
                    accManager.utils.showSuccess("Update successfully!");
                    accManager.utils.unBlock();
                    $.post("GetDepts", function (json) {
                        window.viewModel.Departments = json;
                    });
                    $("#divHeaderDeptName").removeClass("asc").removeClass("desc");
                    $("#divHeaderNoOfUser").removeClass("asc").removeClass("desc");
                }
            });
    },
    deleteClick: function (id, name) {
        closePopup();
        var strC = confirm("Are you sure you want to delete this Department?");
        if (strC) {
            accManager.utils.blockUI();
            var param = {
                id: id
            };
            $.post("DeleteDept", param, function (data) {
                if (parseInt(data.ErrorCode) != 0) {
                    accManager.utils.showErr(data.ErrorMessage);
                    accManager.utils.unBlock();
                } else {
                    setTimeout(function () {
                        $.post("GetDepts", function (json) {
                            window.viewModel.Departments = json;
                            accManager.dept.renderGrid(window.viewModel.Departments);
                            accManager.utils.unBlock();
                            accManager.utils.showSuccess("Delete group (" + name + ") successfully!");
                            $("#divHeaderDeptName").removeClass("asc").removeClass("desc");
                            $("#divHeaderNoOfUser").removeClass("asc").removeClass("desc");
                        });
                    }, 500);
                }
            });
        }
    },
    sortByName: function (e) {
        var target = e.target || e.srcElement;
        var tag;
        if (!target.sortOrder) {
            tag = "Name";
            target.sortOrder = true;
            $(target).removeClass("asc").removeClass("desc").addClass("asc");
        }
        else {
            tag = "-Name";
            target.sortOrder = false;
            $(target).removeClass("desc").removeClass("asc").addClass("desc");
        }
        $("#divHeaderNoOfUser").removeClass("asc").removeClass("desc");
        jlinq.from(window.viewModel.Departments).sort(tag);
        accManager.dept.renderGrid(window.viewModel.Departments);
    },
    sortByNumberOfUsers: function (e) {
        var target = e.target || e.srcElement;
        var tag;
        if (!target.sortOrder) {
            tag = "NoOfUser";
            target.sortOrder = true;
            $(target).removeClass("asc").removeClass("desc").addClass("asc");
        }
        else {
            tag = "-NoOfUser";
            target.sortOrder = false;
            $(target).removeClass("desc").removeClass("asc").addClass("desc");
        }
        $("#divHeaderDeptName").removeClass("asc").removeClass("desc");
        jlinq.from(window.viewModel.Departments).sort(tag);
        accManager.dept.renderGrid(window.viewModel.Departments);
    }
};

accManager.utils = {
    getMessageType: function () {
        return $("#err").find("div[class]").attr("class");
    },
    hideMsg: function () {
        $("#err").hide();
    },
    showErr: function (msg) {
        showAlertMessage("err", msg, 1);
        $("#err").show();
    },
    showSuccess: function (msg) {
        showAlertMessage("err", msg, 0);
        $("#err").show();
        var timerId = setTimeout($.proxy(function () {
            this.hideMsg();
            clearTimeout(timerId);
        }, this), 2000);
    },
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
    },
    validateStringInput: function (str) {
        if (str.indexOf("<") >= 0) return false;
        if (str.indexOf(">") >= 0) return false;
        if (str.indexOf(";") >= 0) return false;
        if (str.indexOf("'") >= 0) return false;
        if (str.indexOf('"') >= 0) return false;
        return true;
    },
    validateStringInput2: function (str) {
        if (str.indexOf("<") >= 0) return false;
        if (str.indexOf(">") >= 0) return false;
        return true;
    },
    removeSpecifiedCharacter: function (str) {
        str = str.replace(/>/g, "");
        str = str.replace(/</g, "");
        str = str.replace(/'/g, "");
        str = str.replace(/"/g, "");
        return str;
    },
    isEmpty: function (value) {
        return value.replace(/ /g, "") == "";
    },
    encodeSpecifiedCharacter: function (value) {
        if (value == null || typeof (value) == "undefined") return "";
        value = value.replace(/'/g, "&apos;");
        value = value.replace(/"/g, "&quot;");
        return value;
    },
    decodeSpecifiedCharacter: function (value) {
        if (value == null || typeof (value) == "undefined") return "";
        value = value.replace(/&apos;/g, "'");
        value = value.replace(/&quot;/g, '"');
        value = value.replace(/&amp;/g, '&');
        return value;
    },
    trimStart: function (value) {
        while (value[0] != "undefined" && value[0] == " ") {
            if (value.length == 1) return "";
            value = value.substr(1, value.length - 1);
        }
        return value;
    },
    trimEnd: function (value) {
        while (value.length > 0 && value[value.length - 1] != "undefined" && value[value.length - 1] == " ") {
            if (value.length == 1) return "";
            value = value.substr(0, value.length - 1);
        }
        return value;
    },
    trimStartEnd: function (value) {
        return this.trimEnd(this.trimStart(value));
    }
};

accManager.others = {
    init: function () {
        if ($.browser.mozilla) {
            $("#tbxPassNewAcc").css("margin-left", "11px");
        }
        else {
            $("#tbxPassNewAcc").css("margin-left", "13px");
        }
    }
};

$(document).ready(function () {
    accManager.init();
});
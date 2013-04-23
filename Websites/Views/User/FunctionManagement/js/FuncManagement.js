var funcManager = {};

funcManager.operationAddFunction = {
    init: function () {
        $("#btnAddFunction").click(function () {
            $("#txtName").val("");
            $("#txtDesc").val("");
            $("#txtFileNames").val("");
            $("#slsystem").removeAttr("disabled");
            showPopup
            ({
                content: $("#divAddFunction"),
                title: "Add Function",
                width: 600,
                height: 300,
                buttons: [{
                    "text": "Submit",
                    callback: function (popuid) {
                        funcManager.operationAddFunction.addFunc(popuid);
                    }
                }],
                multiple: false,
                dialog: true,
                onload: function () {
                    $("#slsystem").change(function () {
                        funcManager.operationAddFunction.fillParentFunction($(this).val());
                    });
                }
            });
        });
        this.fillParentFunction();
    },
    fillParentFunction: function (systemName) {
        if (typeof systemName == "undefined") {
            var arr = new Array();
            arr.push("<option value='0' selected='selected'>None</option>");
            for (var i = 0; i < window.viewModel.ParentFunctionList.length; i++) {
                var item = window.viewModel.ParentFunctionList[i];
                arr.push("<option value='" + item.ID + "'>" + item.Name + "</option>");
            }
            $("#slParent").empty().append(arr.join(""));
        }
        else {
            $.get("GetParentFunction", { systemName: systemName }, $.proxy(function (json) {
                window.viewModel.ParentFunctionList = json;
                this.fillParentFunction();
            }, this));
        }
    },
    addFunc: function (popuid) {
        if (funcManager.utils.isEmpty($("#txtName").val())) {
            funcManager.utils.showErr("Please input function name!");
            return;
        }
        if (funcManager.utils.isEmpty($("#txtFileNames").val())) {
            funcManager.utils.showErr("Please input file names!");
            return;
        }
        if (!funcManager.utils.validateStringInput($("#txtName").val())) {
            funcManager.utils.showErr("Function name is invalid!");
            return;
        } if (!funcManager.utils.validateStringInput($("#txtFileNames").val())) {
            funcManager.utils.showErr("File names is invalid!");
            return;
        }
        var param = {
            name: $("#txtName").val(),
            desc: $("#txtDesc").val(),
            fileNames: $("#txtFileNames").val(),
            parent: $("#slParent").val(),
            systemName: $("#slsystem").val()
        };
        funcManager.utils.blockUI();
        $.post("AddFunction", param, function(data) {
            if (parseInt(data.ErrorCode) != 0) {
                funcManager.utils.showErr(data.ErrorMessage);
                funcManager.utils.unBlock();
            }
            else {
                closePopup(popuid);
                $.get("ReloadViewModel", null, function (json) {
                    window.viewModel = json;
                    funcManager.grid.render();
                    funcManager.utils.showSuccess("Add function successfully!");
                    funcManager.utils.unBlock();
                });
            }
        });
    }
};

funcManager.grid = {
    width: 1141,
    height: 750,
    init: function () {
        this.render();
    },
    editFunc: function (funcId, funcName) {
        var arr = jlinq.from(this.allFuncs).equals("ID", funcId).select();
        if (arr.length <= 0) return;
        $("#txtName").val(arr[0].Name);
        $("#txtDesc").val(arr[0].Desc);
        $("#txtFileNames").val(arr[0].Files);
        $("#slsystem").val(arr[0].System);
        $("#slParent").val(arr[0].ParentId);
        $("#slsystem").attr("disabled", "disabled");
        showPopup
        ({
            content: $("#divAddFunction"),
            title: "Edit Function: " + funcName,
            width: 600,
            height: 300,
            buttons: [{
                "text": "Submit",
                callback: function (popuid) {
                    funcManager.grid.submitEdit(funcId, popuid);
                }
            }],
            multiple: false,
            dialog: true,
            onload: function () {
                $("#slsystem").change(function () {
                    funcManager.operationAddFunction.fillParentFunction($(this).val());
                });
            }
        });
    },
    submitEdit: function (funcId, popuid) {
        if (funcManager.utils.isEmpty($("#txtName").val())) {
            funcManager.utils.showErr("Please input function name!");
            return;
        }
        if (funcManager.utils.isEmpty($("#txtFileNames").val())) {
            funcManager.utils.showErr("Please input file names!");
            return;
        }
        if (!funcManager.utils.validateStringInput($("#txtName").val())) {
            funcManager.utils.showErr("Function name is invalid!");
            return;
        } if (!funcManager.utils.validateStringInput($("#txtFileNames").val())) {
            funcManager.utils.showErr("File names is invalid!");
            return;
        }
        var param = {
            funcId: funcId,
            name: $("#txtName").val(),
            desc: $("#txtDesc").val(),
            fileNames: $("#txtFileNames").val(),
            parent: $("#slParent").val(),
            systemName: $("#slsystem").val()
        };
        funcManager.utils.blockUI();
        $.post("EditFunction", param, function (data) {
            if (parseInt(data.ErrorCode) != 0) {
                funcManager.utils.showErr(data.ErrorMessage);
                funcManager.utils.unBlock();
            }
            else {
                closePopup(popuid);
                $.get("ReloadViewModel", null, function (json) {
                    window.viewModel = json;
                    funcManager.grid.render();
                    funcManager.utils.showSuccess("Update successfully!");
                    funcManager.utils.unBlock();
                });
            }
        });
    },
    render: function () {
        var arrEle = [];
        var level = 0;
        this.allFuncs = [];
        for (var i = 0; i < window.viewModel.FunctionList.length; i++) {
            this.fillArray4Render(arrEle, window.viewModel.FunctionList[i], level);
        }

        var arrRows = [];
        for (var j = 0; j < arrEle.length; j++) {
            this.concatTo1Array(arrRows, arrEle[j]);
        }
        $("#tbodyGridFunction").empty().append(arrRows);
        $("#divFuncGrid").show().reRenderScroller(this.width, this.height, true);
        var timerId = setTimeout(function () {
            $("#tbodyGridFunction").find("tr").mouseover(function () {
                $(this).find("td").css("background-color", "#FFDDFE");
            }).mouseout(function () {
                $(this).find("td").css("background-color", "");
            });
            clearTimeout(timerId);
        }, 200);
    },
    fillArray4Render: function (arrEle, rootFunc, level) {
        this.allFuncs.push(rootFunc);
        rootFunc.level = level;
        var row = $("#scriptTemplateGridFunction").tmpl([rootFunc]).get(0);
        var item = {
            row: row,
            subRows: []
        };
        if (rootFunc.TotalSub > 0) {
            for (var j = 0; j < rootFunc.SubFuncs.length; j++) {
                this.fillArray4Render(item.subRows, rootFunc.SubFuncs[j], level + 1);
            }
        }
        arrEle.push(item);
    },
    concatTo1Array: function (arr, item) {
        arr.push(item.row);
        if (item.subRows.length > 0) {
            for (var i = 0; i < item.subRows.length; i++) {
                this.concatTo1Array(arr, item.subRows[i]);
            }
        }
    },
    deleteFunc: function (funcId) {
        closePopup();
        var strC = confirm("Are you sure you want to delete this Function?");
        if (!strC) return;
        funcManager.utils.blockUI();
        $.post("DeleteFunction", { funcId: funcId }, function (data) {
            if (parseInt(data.ErrorCode) != 0) {
                funcManager.utils.showErr(data.ErrorMessage);
                funcManager.utils.unBlock();
            }
            else {
                $.get("ReloadViewModel", null, function (json) {
                    window.viewModel = json;
                    funcManager.grid.render();
                    funcManager.utils.showSuccess("Delete successfully!");
                    funcManager.utils.unBlock();
                });
            }

        });
    }
};

funcManager.utils = {
    hideMsg: function () {
        $("#err").hide();
    },
    showErr: function (msg) {
        showAlertMessage("err", msg, 1);
    },
    showSuccess: function (msg) {
        showAlertMessage("err", msg, 0);
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
        return true;
    },
    isEmpty: function (value) {
        return value.replace(/ /g, "") == "";
    }
};

funcManager.init = function () {
    this.operationAddFunction.init();
    this.grid.init();
};

$(document).ready(function () {
    funcManager.init();
});
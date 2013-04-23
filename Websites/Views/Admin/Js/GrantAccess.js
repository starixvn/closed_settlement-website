var grantAccess = {
    width: 1193,
    height: 700,
    //pageSize will be assigned in razor template
    pageIndex: 0
};

grantAccess.init = function () {
    $("#divMainGrid").removeClass("hidden").reRenderScroller(grantAccess.width, grantAccess.height, true);
    grantAccess.initPager(grantAccess.getCurrentVMLength(), grantAccess.pageSize, grantAccess.pageIndex);

    $("#btnAdd").click(function () {
        $("#msgError").hide();
        grantAccess.addUser();
    });

    $("#divAccountSearch").click(function (e) {
        var target = e.target;
        $(this).hide();
        $(this).parent().append("<div class='w160' editor='true'><input type='text' class='inputtext' style='width:158px' /></div>");
        var textbox = $(this).parent().find("input");
        textbox.focus();
        textbox.focusout(function (e) {
            $(this).parent().remove();
            $(target).show();
        });
        textbox.keyup(function (e) {
            if (e.keyCode == 27) {
                $(this).parent().remove();
                $(target).show();
            }
            if (e.keyCode == 13) {
                if ($(this).val().replace(/ /g, '') == "") {
                    grantAccess.vmFiltering = null;
                }
                else {
                    grantAccess.vmFiltering = jlinq.from(grantAccess.modelForSearch).contains("Account", $(this).val()).select();
                    for (var i = 0; i < grantAccess.vmFiltering.length; ++i) {
                        grantAccess.vmFiltering[i].Index = i + 1;
                    }
                }
                grantAccess.pageIndex = 0;
                grantAccess.buildGrid(grantAccess.concatVM4Paging());
                grantAccess.initPager(grantAccess.getCurrentVMLength(), grantAccess.pageSize, grantAccess.pageIndex);
            }
        });
    });
};

grantAccess.getCurrentVMLength = function () {
    if (typeof grantAccess.vmFiltering == "undefined" || grantAccess.vmFiltering == null)
        return this.model.length;
    else return this.vmFiltering.length;
};

grantAccess.showErr = function (text) {
    showAlertMessage("msgError", text, 1);
    $("#msgError").show().find("div:first").width(1210);
};

grantAccess.showSuccess = function (text) {
    showAlertMessage("msgError", text, 0);
    $("#msgError").show().find("div:first").width(1210);
};

grantAccess.validateValue = function (str) {
    if (str.indexOf("<") >= 0 ) return false;
    return true;
};

grantAccess.addUser = function () {
    if ($("#tbxAccount").val() == "") {
        grantAccess.showErr("Please input username!");
        return;
    }
    if (!this.validateValue($("#tbxAccount").val())) {
        grantAccess.showErr("Username is invalid!");
        return;
    }
    if (!this.validateValue($("#tbxRemark").val())) {
        grantAccess.showErr("Remark is invalid!");
        return;
    }
    var param = {
        "userName": $("#tbxAccount").val(),
        "remark": $("#tbxRemark").val()
    };

    grantAccess.blockUI();

    $.post("AddUser", param,
        function (data) {
            if (parseInt(data.ErrorCode) != 0) {
                grantAccess.showErr(data.ErrorMessage);
                grantAccess.unBlock();
            }
            else {
                $("#tbxAccount").val("");
                $("#tbxRemark").val("");
                grantAccess.reloadData("Add new account successfully!");
            }
        });

};

grantAccess.reloadData = function (text, standPaging) {
    $.get("ReloadData", function (data) {
        grantAccess.model = data.vm1;
        grantAccess.modelForSearch = data.vm2;

        grantAccess.vmFiltering = null;
        if (!standPaging) {
            grantAccess.pageIndex = 0;
        }
        grantAccess.buildGrid(grantAccess.concatVM4Paging());
        grantAccess.initPager(grantAccess.getCurrentVMLength(), grantAccess.pageSize, grantAccess.pageIndex);

        grantAccess.showSuccess(text);
        grantAccess.unBlock();
    });
};

grantAccess.buildGrid = function (model) {
    var tpl = $("#scriptMainTemplate");
    var grid = $("#tobdyMainGrid");
    if (model.length > 400) {
        grid.empty()
        var arr1 = [];
        for (var i = 0; i < 250; ++i) {
            arr1.push(model[i]);
        }
        tpl.tmpl(arr1).appendTo(grid);
        arr1 = [];
        for (var i = 250; i < model.length; ++i) {
            arr1.push(model[i]);
        }
        tpl.tmpl(arr1).appendTo(grid);
    }
    else {
        tpl.tmpl(model).appendTo(grid.empty());
    }
    if (model.length == 0)
        $('#pagination').hide();
    $("#divMainGrid").removeClass("hidden").reRenderScroller(grantAccess.width, grantAccess.height, true);
};

grantAccess.concatVM4Paging = function () {
    var vm = [];
    if (typeof grantAccess.vmFiltering == "undefined" || grantAccess.vmFiltering == null)
        vm = this.model;
    else vm = grantAccess.vmFiltering;

    if (vm.length <= this.pageSize) {
        return vm;
    }
    var _vm = [];

    var start = this.pageIndex * this.pageSize;
    var end = 0;

    if (vm.length < (start + this.pageSize)) {
        end = vm.length;
    }
    else {
        end = start + this.pageSize;
    }

    for (var i = start; i < end; ++i) {
        _vm.push(vm[i]);
    }
    return _vm;
};

grantAccess.pageSelectCallback = function (page_index, _items_per_page, jq) {
    var max_page = Math.ceil(grantAccess.model.length / _items_per_page);
    page_index = page_index < max_page ? page_index : max_page - 1;

    this.pageSize = parseInt(_items_per_page);
    this.pageIndex = page_index;
    this.buildGrid(grantAccess.concatVM4Paging());
    this.initPager(grantAccess.getCurrentVMLength(), grantAccess.pageSize, grantAccess.pageIndex);
    return false;
};

grantAccess.initPager = function (total_item, items_per_page, current_page) {
    var opt = {
        callback: function () {
            grantAccess.pageSelectCallback(arguments[0], arguments[1], arguments[2]);
        },
        item_per_page_arr: [100, 200, 300, 400, 500],
        items_per_page: items_per_page,
        current_page: current_page
    };
    if (total_item > 0)
        $("#pagination").pagination(total_item, opt);
    else
        $("#pagination").hide();
};

grantAccess.blockUI = function () {
    if (parent.parent.BlockUI) {
        try { parent.parent.BlockUI(); } catch (e) { }
    }
    else {
        try { parent.BlockUI(); } catch (e) { }
    }
};

grantAccess.unBlock = function () {
    if (parent.parent.$ && parent.parent.$.unblockUI) {
        try { parent.parent.$.unblockUI(); } catch (e) { }
    }
    else {
        try { parent.$.unblockUI(); } catch (e) { }
    }
};

grantAccess.beginEdit = function (e) {
    $("#msgError").hide();
    var target = e.target || e.srcElement;
    var tr = $(target).parents("tr:first");
    var iconSave = $(target).parent().find("img[title='Save']");
    var iconUndo = $(target).parent().find("img[title='Cancel']");

    $(target).hide();
    iconSave.show();
    iconUndo.show();

    if (tr.hasClass("bgeven")) {
        tr.get(0).bgeven = true;
    }
    tr.attr("class", "bgedit");

    var cbxGrantAccess = tr.find("input[grantaccess]");
    var labelRemark = tr.find("span[remark]");

    cbxGrantAccess.hide();
    cbxGrantAccess.parent().append("<input type='checkbox' editor='true'/ >");
    var cbxGrantAccessEditor = cbxGrantAccess.parent().find("input[editor]");
    if (cbxGrantAccess.is(":checked")) {
        cbxGrantAccessEditor.get(0).checked = true;
    }

    labelRemark.hide();
    labelRemark.parent().append("<input  id='tbxremark1' style='width:157px' type='text' class='txt' editor='true'/>");
    $('#tbxremark1').val(labelRemark.html());
};

grantAccess.endEdit = function (e, type, custid) {
    var target = e.target || e.srcElement;
    var tr = $(target).parents("tr:first");
    var iconEdit = $(target).parent().find("img[title='Edit']");
    var iconSave = $(target).parent().find("img[title='Save']");
    var iconUndo = $(target).parent().find("img[title='Cancel']");

    var endEdit = function () {
        iconEdit.show();
        iconSave.hide();
        iconUndo.hide();

        tr.removeAttr("class");
        if (tr.get(0).bgeven) {
            tr.attr("class", "bgeven");
        }

        tr.find("input[editor]").each(function () {
            var parent = $(this).parent();
            $(this).remove();
            parent.children().show();
        });
    };

    if (type == "Save") {
        var cbxGrantAccess = tr.find("input[grantaccess]");
        var cbxGrantAccessEditor = cbxGrantAccess.parent().find("input[editor]");

        var labelRemark = tr.find("span[remark]");
        var labelRemarkEditor = labelRemark.parent().find("input[editor]");
        if (!this.validateValue(labelRemarkEditor.val())) {
            grantAccess.showErr("Remark is invalid!");
            return;
        }
        var param = {
            "custId": custid,
            "isAllowed": cbxGrantAccessEditor.get(0).checked,
            "remark": labelRemarkEditor.val()
        };

        grantAccess.blockUI();
        $.post("Update", param,
        function (data) {
            if (parseInt(data.ErrorCode) != 0) {
                grantAccess.showErr(data.ErrorMessage);
            }
            else {
                grantAccess.reloadData("Update successfully!", true);
            }
            grantAccess.unBlock();
        });
    }
    else {//Cancel
        endEdit();
    }
};

grantAccess.remove = function (custId, CustName) {
    if (!confirm('Are you sure you want to remove "' + CustName + '"?')) {
        return;
    }

    $("#msgError").hide();
    var param = {
        "custId": custId
    };
    grantAccess.blockUI();
    $.post("Delete", param,
        function (data) {
            if (parseInt(data.ErrorCode) != 0) {
              
                grantAccess.showErr(data.ErrorMessage);
                grantAccess.unBlock();
            }
            else {
               
                grantAccess.reloadData("Remove successfully!");
            }
        });
};

$(document).ready(function () {
    grantAccess.init();
});
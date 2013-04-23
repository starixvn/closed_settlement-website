var accountManagement =
{
    viewAllDepartments: {}
    , viewAllAccount: {}
    , rolePolicy: {}
    , accountPolicy: {}
    , utils: {}
};

accountManagement.viewAllDepartments.LoadDepartmentInfomation = function () {
    if (parent.BlockUI) parent.BlockUI();
    $.ajax({
        type: "POST",
        url: "GetDepartments2.ashx?t=all",
        async: true,
        data: null,
        success: (function (rs) {
            parent.jQuery.unblockUI();
            this.renderTemplate(JSON.parse(rs));
        }).bind(this)
        , error: function () {
            parent.jQuery.unblockUI();
        }
    });
};

accountManagement.viewAllDepartments.OnEnter = function (e) {
    var code = (e && e.which) ? e.which : e.keyCode;
    if (code == 13) {
        obj("btnAddDept").click();
        return false;
    }
    return true;
};

accountManagement.viewAllDepartments.DelDept = function (id) {
    var strC = confirm("Are you sure you want to delete this Department?");
    if (strC) {
        var f = self.frames["frDelete"];
        if (f) {
            parent.BlockUI();
            f.location.href = "UpdateDept.ashx?type=del&groupId=" + id;
        }
    }
};

accountManagement.viewAllDepartments.OnUpdate = function (type) {
    if (type == "add") {
        if (trim($("#txtDept").val()) == "") {
            accountManagement.utils.showAlertMessage("errfunc", "Please input Department name.", 1);
        }
        else {
            parent.BlockUI();
            document.fAddDept.submit();
        }
    }
    else if (type == "acc") {
        if ($("#txtUName").val() == "") {
            accountManagement.utils.showAlertMessage("errfunc", "Please input User Name.", 1);
            return;
        }
        if ($("#txtPwd").val() == "") {
            accountManagement.utils.showAlertMessage("errfunc", "Please input Password.", 1);
            return;
        }
        if ($("#selGroup").val() == "0") {
            accountManagement.utils.showAlertMessage("errfunc", "Please choose Department.", 1);
            return;
        }
        parent.BlockUI();
        document.fAddAcc.submit();
    }
    else if (type == "exist") {
        if ($("#txtEUName").val() == "") {
            accountManagement.utils.showAlertMessage("errfunc", "Please input User Name.", 1);
            $("#txtEUName").focus();
            return;
        }
        if ($("#selEGroup").val() == "0") {
            accountManagement.utils.showAlertMessage("errfunc", "Please choose Department.", 1);
            $("#selEGroup").focus();
            return;
        }
        parent.BlockUI();
        document.fAddExistAcc.submit();
    }
    else {
        document.fEditDept.submit();
    }
};

accountManagement.viewAllDepartments.deleteDepartment = function (groupid) {
    var index = -1;
    for (var i = 0; i < this.viewModel.dataTable.length; ++i) {
        if (this.viewModel.dataTable[i].groupid == groupid) {
            index = i;
            break;
        }
    }
    if (index != -1) {
        this.viewModel.dataTable.splice(index, 1);
    }
    this.renderTemplate();
    index = -1;
    for (var i = 0; i < this.viewModelBackup.dataTable.length; ++i) {
        if (this.viewModelBackup.dataTable[i].groupid == groupid) {
            index = i;
            break;
        }
    }
    if (index != -1) {
        this.viewModelBackup.dataTable.splice(index, 1);
    }
};

//Old popup functions: OpenEditDept2(groupId);
accountManagement.viewAllDepartments.beginEditMode = function (e, groupId) {
    var target = accountManagement.utils.getTarget(e);
    if (target == null) return;
    if (this.isEditting(target)) return;

    //--------------------------------------------------------------------------------------------------------------
    var record = jlinq.from(this.viewModel.dataTable).equals("groupid", groupId).select();
    record = record.length == 1 ? record[0] : null;
    if (record == null) return;

    //--------------------------------------------------------------------------------------------------------------
    $(document).keyup((function (e) {
        //ESC press
        if (e.keyCode == 27) {
            this.endEditMode(target);
            $(document).unbind("keyup");
        }
    }).bind(this));
    //--------------------------------------------------------------------------------------------------------------

    var trElement = $(target).parents("tr")[0];
    accountManagement.utils.showTooltip($(trElement), "Press ESC to cancel");

    var arrElementCanChange = $(trElement).find("[canchange]");
    if (arrElementCanChange.length == 2) {
        //Department
        var departmentElement = $(arrElementCanChange.get(0));

        //Descripton
        var descriptionElement = $(arrElementCanChange.get(1));

        //for 1 row
        var decreaseWidth = 2;
        if (accountManagement.utils.isIE()) {
            decreaseWidth = 4;
        }

        departmentElement.hide();
        departmentElement.parent().append("<input editor='true' type='text' title='Press ESC to cancel' style='width:" + (departmentElement.width() - decreaseWidth) + "px' value='" + record.department + "'/>");

        var saveChange = (function () {
            this.saveChange(groupId,
                        $(departmentElement.parent().find("input[editor]").get(0)).val()
                        , $(descriptionElement.parent().find("input[editor]").get(0)).val()
                        , (function () {//succes function
                            departmentElement.html(
                                $(departmentElement.parent().find("input[editor]").get(0)).val()
                                );
                            descriptionElement.html(
                                $(descriptionElement.parent().find("input[editor]").get(0)).val()
                                );
                            record.department = $(departmentElement.parent().find("input[editor]").get(0)).val();
                            record.desc = $(descriptionElement.parent().find("input[editor]").get(0)).val();
                            this.synchronousData(record);
                            this.endEditMode(target);
                        }).bind(this)
                        , (function (e) {//error function
                            this.endEditMode(target);
                            accountManagement.utils.showAlertMessage("errfunc", e, 1);
                        }).bind(this)
                    );
        }).bind(this);

        departmentElement.parent().find("input[editor]").keyup((function (event) {
            //ESC press
            if (event.keyCode == 27) {
                this.endEditMode(target);
            }
            else if (event.keyCode == 13) {
                saveChange();
            }
        }).bind(this));
        departmentElement.parent().find("input[editor]").focus();

        //Description
        descriptionElement.hide();
        descriptionElement.parent().append("<input editor='true' type='text' title='Press ESC to cancel' style='width:" + (descriptionElement.width() - decreaseWidth) + "px' value='" + record.desc + "'/>");
        descriptionElement.parent().find("input[editor]").keyup((function (event) {
            //ESC press
            if (event.keyCode == 27) {
                this.endEditMode(target);
            }
            else if (event.keyCode == 13) {
                saveChange();
            }
        }).bind(this));

        //set save icon
        $(target).hide();
        $(target).parent().append("<img alt='Save' src='images/Save.png' editor='true' style='cursor:pointer;' />");
        $(target).parent().find("img[editor]").click(saveChange);

        accountManagement.utils.fillScrollBar("divViewAllDepartmentsTemplate");
    }
};

accountManagement.viewAllDepartments.isEditting = function (target) {
    var count = $($(target).parents("table").get(0)).find("input[editor]").length;
    if (count > 0) return true;
    return false;
};

accountManagement.viewAllDepartments.saveChange = function (groupId, deparment, description, successCallBack, errCallBack) {
    parent.BlockUI();
    $.ajax({
        type: "POST",
        url: "UpdateDept.ashx?type=edit",
        async: true,
        data: '{"groupId":"' + groupId + '", "txtDept":"' + deparment + '", "txtDesc": "' + description + '"}',
        success: function () {
            parent.jQuery.unblockUI();
            if (typeof successCallBack != "undefined" && successCallBack != null)
                successCallBack();
        }
        , error: function (e) {
            parent.jQuery.unblockUI();
            if (typeof errCallBack != "undefined" && errCallBack != null)
                errCallBack(e);
        }
    });
};

accountManagement.viewAllDepartments.endEditMode = function (target) {
    var trElement = $(target).parents("tr")[0];
    $(trElement).RemoveBubblePopup();
   
    $(trElement).find("td [editor]").remove();
    $(trElement).find("td [canchange]").show();
    $(target).show();
    accountManagement.utils.fillScrollBar("divViewAllDepartmentsTemplate");
};

accountManagement.accountPolicy.renderTemplate = function (viewModel) {
    if (!viewModel) {
        viewModel = this.viewModel;
    }
    else {
        this.viewModel = viewModel;
    }
    this.container = document.getElementById("divAccountPolicy");
    ko.applyBindings(this.viewModel, this.container);
};

accountManagement.rolePolicy.renderTemplate = function (viewModel) {
    if (viewModel) {
        this.viewModel = viewModel;
    }
    this.container = document.getElementById("divRolePolicy"); ;
    ko.applyBindings(this.viewModel, this.container);
};

accountManagement.viewAllDepartments.initSorting = function (container) {
    var arrElementHeaderSortable = $(container).find("table thead tr[class='title'] div[sortable='true']");

    //Sort by department name
    var sortOrderDepartment = "ASC";
    var departmentHeader = $(arrElementHeaderSortable.get(0));
    //accountManagement.utils.showTooltip(departmentHeader, "Sort by department name");
    //departmentHeader.HideBubblePopup();
    departmentHeader.click((function () {
        parent.BlockUI();
        if (sortOrderDepartment == "ASC") {
            jlinq.from(this.viewModel.dataTable).sort("-department").select();
            sortOrderDepartment = "DESC";
        }
        else {
            jlinq.from(this.viewModel.dataTable).sort("department").select();
            sortOrderDepartment = "ASC";
        }
        this.renderTemplate();
        accountManagement.utils.fillScrollBar("divViewAllDepartmentsTemplate");
        parent.jQuery.unblockUI();
    }).bind(this));

    //Sort by no of user
    var sortOrderNoOfUser = "ASC";
    var noOfUserHeader = $(arrElementHeaderSortable.get(1));
    //accountManagement.utils.showTooltip(noOfUserHeader, "Sort by no of user");
    //noOfUserHeader.HideBubblePopup();
    noOfUserHeader.click((function () {
        parent.BlockUI();
        if (sortOrderNoOfUser == "ASC") {
            jlinq.from(this.viewModel.dataTable).sort("-nouser").select();
            sortOrderNoOfUser = "DESC";
        }
        else {
            jlinq.from(this.viewModel.dataTable).sort("nouser").select();
            sortOrderNoOfUser = "ASC";
        }
        this.renderTemplate();
        accountManagement.utils.fillScrollBar("divViewAllDepartmentsTemplate");
        parent.jQuery.unblockUI();
    }).bind(this));
};

accountManagement.rolePolicy.SelGroupPolicy = function (id) {
    parent.BlockUI();
    $.ajax({
        type: "POST",
        url: "UserControlService.svc/GetGroupPolicyList",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        data: '{"groupId":' + id + '}',
        success: (function (rs) {
            parent.jQuery.unblockUI();
            this.BindGroupPolicy(id, rs);
        }).bind(this)
        , error: function (e) {
            parent.jQuery.unblockUI();
            accountManagement.utils.showAlertMessage("errfunc", e, 1);
        }
    });
};

accountManagement.rolePolicy.BindGroupPolicy = function (groupId, rs) {
    //-------------------------
    //  Using tab
    //-------------------------
    /*accountManagement.utils.hideTab7();
    rs.groupId = groupId;
    rs.groupName = rs[0].GroupName;
    this.renderTemplate(rs);
    this.title = "";
    this.strFunc = "";
    this.WriteData(rs, 0);
    $("#tbodyRolePolicyData").empty().append(this.title);
    $("#arrFuncId").val(strFunc.substring(1, strFunc.length));

    //----------------------------------------------------------------
    //set no border bottom at last row
    $("#tbodyRolePolicyData").find("tr").last().find("td").each(function (index, element) {
    $(element).get(0).style.borderBottom = "0px none";
    });
    //----------------------------------------------------------------

    accountManagement.utils.showTab6();*/


    //-------------------------
    //  Using popup dom
    //-------------------------
    $("#divAccountPolicy").empty();
    rs.groupId = groupId;
    rs.groupName = rs[0].GroupName;
    this.renderTemplate(rs);
    this.title = "";
    this.strFunc = "";
    this.WriteData(rs, 0);
    $("#tbodyRolePolicyData").empty().append(this.title);
    $("#arrFuncId").val(strFunc.substring(1, strFunc.length));

    //----------------------------------------------------------------
    //set no border bottom at last row
    $("#tbodyRolePolicyData").find("tr").last().find("td").each(function (index, element) {
        $(element).get(0).style.borderBottom = "0px none";
    });
    //----------------------------------------------------------------
    accountManagement.utils.showPopupDomElement("divRolePolicy");
};

accountManagement.rolePolicy.cancelUpdate = function () {
    accountManagement.utils.hidePopupDomElement("divRolePolicy");
};

accountManagement.rolePolicy.onSubmit = function () {
    parent.BlockUI();
    document.fUpdatePolicy.submit();
};

accountManagement.rolePolicy.WriteData = function (listFunction, level) {
    for (var i = 0; i < listFunction.length; i++) {
        strFunc += "," + listFunction[i].ID;

        this.title += "<tr>";
        this.title += "<td style='text-align:left;padding:1px' class='indent" + level + "'>";
        if (level == 0) {
            this.title += "<div style='width:301px;overflow:hidden'>";
            this.title += listFunction[i].FuncDesc;
            this.title += "</div>";
        }
        else {
            this.title += "<span style='width:300px;overflow:hidden'>";
            this.title += listFunction[i].FuncDesc;
            this.title += "</span>";
        }
        this.title += "</td>";
        var isRead = "";
        var isHost = "";
        if (listFunction[i].IsGroupHost == true) { isHost = "checked"; }
        if (listFunction[i].IsGroupRead == true) { isRead = "checked"; }
        this.title += "<td class='c'><div style='width:100px;overflow:hidden'><input type='checkbox' class='chkR' parentId='" + listFunction[i].ParentId + "' id='chkR" + listFunction[i].ID + "' name='chkR" + listFunction[i].ID + "' " + isRead + " onclick='javascript:chkCheckRead(this);'></div></td>";
        this.title += "<td class='c'><div style='width:92px;overflow:hidden'><input type='checkbox' class='chkH' parentId='" + listFunction[i].ParentId + "' id='chkH" + listFunction[i].ID + "' name='chkH" + listFunction[i].ID + "' " + isHost + " onclick='javascript:chkCheckHost(this);'></div></td>";

        this.title += "</tr>";
        this.WriteData(listFunction[i].SubFuncs, level + 1);
    }
};

accountManagement.viewAllDepartments.initSearching = function (container) {
    if (!container) {
        this.container = document.getElementById("htmlGroupList");
        container = this.container;
    }
    var keyWordElement = $(container).find("div[searcharea='true'] input[keyword='true']");
    keyWordElement.blur(function () {
        if ($(this).val() == "") $(this).val("All");
    });
    keyWordElement.focus(function () {
        if ($(this).val() == "All") $(this).val("");
    });
    var searchFunction = (function (e) {
        var target = keyWordElement;

        if (e.keyCode == 13 || $(accountManagement.utils.getTarget(e)).attr("type") == "button") {
            if ($(target).val() == "All" || $(target).val() == "") {
                this.restoreData();
                this.renderTemplate(this.viewModel);
            }
            else {
                this.viewModel.dataTable = jlinq.from(this.viewModel.dataTable).starts("department", $(target).val()).select();
                if (this.viewModel.dataTable.length <= 0) {
                    this.restoreData();
                    this.viewModel.dataTable = jlinq.from(this.viewModel.dataTable).starts("department", $(target).val()).select();
                    this.renderTemplate(this.viewModel);
                }
                else {
                    this.renderTemplate();
                    accountManagement.utils.fillScrollBar("divViewAllDepartmentsTemplate");
                }
            }
        }
        else if (accountManagement.utils.isAlphanumeric(e) || e.keyCode == 8) {
            if ($(target).get(0).timeoutID)
                clearTimeout($(target).get(0).timeoutID);

            $(target).get(0).timeoutID = setTimeout((function () {
                if ($(target).val() == "" || $(target).val().toLowerCase() == "all") {
                    return;
                }
                var departmentMatched = jlinq.from(this.viewModel.dataTable).starts("department", $(target).val()).select();
                if (departmentMatched.length <= 0) {
                    this.restoreData();
                    departmentMatched = jlinq.from(this.viewModel.dataTable).starts("department", $(target).val()).select();
                }
                accountManagement.utils.autoComplete(target, departmentMatched, "department");
                clearTimeout($(target).get(0).timeoutID);
            }).bind(this), 500);
        }
    }).bind(this);
    keyWordElement.keyup(searchFunction);
    $(container).find("input[type='button']").click(searchFunction);
};

accountManagement.viewAllDepartments.restoreData = function () {
    this.viewModel = accountManagement.utils.Clone(this.viewModelBackup);
};

accountManagement.viewAllDepartments.synchronousData = function (record) {
    var records = jlinq.from(this.viewModel.dataTable).equals("groupid", record.groupid).select();
    if (records.length > 0) {
        records[0].department = record.department;
        records[0].desc = record.desc;
    }
};

accountManagement.init = function () {
    this.viewAllAccount.initSearchingArea();
    this.viewAllDepartments.initSearching();
    if (accountManagement.utils.readCookie("ACCOUNT_MANAGEMENT.CURRENT_TAB") != null){
        if (accountManagement.utils.readCookie("ACCOUNT_MANAGEMENT.CURRENT_TAB") == "VIEW ALL ACCOUNTS"){
            parent.BlockUI();
            this.viewAllAccount.preGetAllAccount();
            $('#tabs').tabs();
            $('#tabs').show();
            $('#tabs').tabs("select", 4);
        }
    }
    else {
        this.viewAllAccount.preGetAllAccount();
        this.viewAllDepartments.LoadDepartmentInfomation(document.getElementById("htmlGroupList"));
        $('#tabs').tabs();
        $('#tabs').show();
    }
};

accountManagement.viewAllDepartments.renderTemplate = function (viewModel) {
    this.container = document.getElementById("htmlGroupList");
    if (viewModel) {
        this.viewModel = viewModel;
    }
    this.initSorting(this.container);
    if (!this.viewModelBackup) {
        this.viewModelBackup = accountManagement.utils.Clone(viewModel);
    }
    ko.applyBindings(this.viewModel, this.container);
    $(this.container).show();
    accountManagement.utils.fillScrollBar("divViewAllDepartmentsTemplate");
};

accountManagement.viewAllAccount.isEditting = function (target) {
    var count = $($(target).parents("table").get(0)).find("input[editor]").length;
    if (count > 0) return true;
    return false;
};

//Old function: OpenEditAcc(${custid},'${user}','${dept}')
accountManagement.viewAllAccount.beginEditMode = function (e, custid) {
    var target = accountManagement.utils.getTarget(e);
    if (target == null) return;
    if (this.isEditting(target)) return;

    //--------------------------------------------------------------------------------------------------------------
    var record = jlinq.from(this.viewModel.dataTable).equals("custid", custid).select();
    record = record.length == 1 ? record[0] : null;
    if (record == null) return;

    var trElement = $(target).parents("tr")[0];
    accountManagement.utils.showTooltip($(trElement), "Press ESC to cancel");

    //--------------------------------------------------------------------------------------------------------------
    $(document).keyup((function (e) {
        //ESC press
        if (e.keyCode == 27) {
            this.endEditMode(target);
            $(document).unbind("keyup");
        }
    }).bind(this));
    //--------------------------------------------------------------------------------------------------------------

    var arrElementCanChange = $(trElement).find("[canchange]");
    var fnameElement = arrElementCanChange.get(0);
    var lnameElement = arrElementCanChange.get(1);
    var deptElement = arrElementCanChange.get(2);
    //--------------------------------------------------------------------------------------------------------------
    var saveChange = (function () {
        this.saveChange(
            record.custid
            , $(trElement).find("select[editor]").val()
            , $(fnameElement).parent().find("input[editor]").val()
            , $(lnameElement).parent().find("input[editor]").val()
            , (function () {
                $(fnameElement).html($(fnameElement).parent().find("input[editor]").val());
                $(lnameElement).html($(lnameElement).parent().find("input[editor]").val());
                $(deptElement).html($(trElement).find("select[editor] option[value='" + $(trElement).find("select[editor]").val() + "']").text());

                record.fname = $(fnameElement).html();
                record.lname = $(lnameElement).html();
                record.groupid = $(trElement).find("select[editor]").val();
                record.dept = $(deptElement).html();
                this.synchronousData(record);
                this.endEditMode(target);
            }).bind(this)
            , (function (e) {
                this.endEditMode(target);
            }).bind(this)
        );
    }).bind(this);
    //--------------------------------------------------------------------------------------------------------------
    var decreaseWidth = 2;
    if (accountManagement.utils.isIE()) {
        decreaseWidth = 4;
    }
    //--------------------------------------------------------------------------------------------------------------
    $(fnameElement).hide();
    $(fnameElement).parent().append("<input type='text' editor='true' value='" + record.fname + "' style='width:" + ($(fnameElement).width() - decreaseWidth) + "px; height:14px' title='Press ESC to cancel' />");
    $(fnameElement).parent().find("input[editor]").focus();
    $(fnameElement).parent().find("input[editor]").keyup((function (e) {
        //ESC key press
        if (e.keyCode == 27) {
            this.endEditMode(target);
        }
        else if (e.keyCode == 13) {
            saveChange();
        }
    }).bind(this));

    $(lnameElement).hide();
    $(lnameElement).parent().append("<input type='text' editor='true' value='" + record.lname + "' style='width:" + ($(lnameElement).width() - decreaseWidth) + "px; height:14px' title='Press ESC to cancel' />");
    $(lnameElement).parent().find("input[editor]").keyup((function (e) {
        //ESC key press
        if (e.keyCode == 27) {
            this.endEditMode(target);
        }
        else if (e.keyCode == 13) {
            saveChange();
        }
    }).bind(this));

    $(deptElement).hide();
    var selectDepartmentListTemp = $("#selectDepartmentListTemp").clone();
    selectDepartmentListTemp.remove();
    selectDepartmentListTemp.removeAttr("id").attr("editor", "true");
    selectDepartmentListTemp.width($(deptElement).width());
    selectDepartmentListTemp.val(record.groupid);
    $(deptElement).parent().append(selectDepartmentListTemp);
    selectDepartmentListTemp.keyup((function (e) {
        //ESC key press
        if (e.keyCode == 27) {
            this.endEditMode(target);
        }
        else if (e.keyCode == 13) {
            saveChange();
        }
    }).bind(this));

    //set save icon
    var imgedit = $(trElement).find("[imgedit]");
    imgedit.hide();
    imgedit.parent().append("<img alt='Save' src='images/Save.png' editor='true' style='cursor:pointer' />");
    imgedit.parent().find("img[editor]").click(saveChange);
};

accountManagement.viewAllAccount.SelAccountByGroupName = function (id, name) {
    if (parent.BlockUI) parent.BlockUI();
    this.viewModel.dataTable = jlinq.from(this.viewModelBackup.dataTable).equals("groupid", id).select();
    this.renderTemplate();
    $('#tabs').tabs("select", 4);
    parent.jQuery.unblockUI();
};

accountManagement.viewAllAccount.preGetAllAccount = function () {
    var id = '', name = 'All';
    var args = 't=mem&groupId=' + id + '&dname=' + name;
    if (id == '' && name == '') {
        var search = "All";
        args += "&txtSearch=" + search;
        var alpha = "All";
        args += "&listAlpha=" + alpha;
    }
    $.ajax({
        type: "POST",
        url: "GetDepartments2.ashx",
        async: true,
        data: args,
        success: (function (rs) {
            rs = JSON.parse(rs);
            this.viewModel = rs;
            setTimeout((function () {
                this.viewModelBackup = accountManagement.utils.Clone(this.viewModel);
            }).bind(this), 2000);
            if (accountManagement.utils.readCookie("ACCOUNT_MANAGEMENT.CURRENT_TAB") != null){
                if (accountManagement.utils.readCookie("ACCOUNT_MANAGEMENT.CURRENT_TAB") == "VIEW ALL ACCOUNTS"){
                    accountManagement.utils.eraseCookie("ACCOUNT_MANAGEMENT.CURRENT_TAB");
                    accountManagement.viewAllAccount.SelectAccounts();
                }
            }
        }).bind(this)
    });
};

accountManagement.viewAllAccount.SelectAccounts = function () {
    if (parent.BlockUI) parent.BlockUI();
    if (!this.viewModel && !this.viewModelBackup) {
        var timeoutID = setTimeout((function () {
            clearTimeout(timeoutID);
            this.SelectAccounts();
            parent.jQuery.unblockUI();
        }).bind(this), 2000);
        this.preGetAllAccount();
        return;
    }
    else {
        this.renderTemplate();
        parent.jQuery.unblockUI();
    }
};

accountManagement.accountPolicy.SelAccountPolicy = function (id, dept, gId, user) {
    this.groupName = dept;
    this.groupId = gId;
    this.userName = user;
    this.custId = id;
    parent.BlockUI();
    $.ajax({
        type: "POST",
        url: "UserControlService.svc/GetUserPolicyList",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        data: '{"userId":' + id + '}',
        success: (function (rs) {
            if (parent.jQuery && parent.jQuery.unblockUI) parent.jQuery.unblockUI();
            this.BindAccountPolicy(id, dept, gId, user, rs);
        }).bind(this)
        , error: function (e) {
            if (parent.jQuery && parent.jQuery.unblockUI) parent.jQuery.unblockUI();
            accountManagement.utils.showAlertMessage("errfunc", e, 1);
        }
    });
};

accountManagement.accountPolicy.BindAccountPolicy = function (id, dept, gId, user, rs) {
    //-------------------------
    //  Using tab
    //-------------------------
    /*rs.groupId = gId;
    rs.groupName = dept;
    rs.userName = user;
    rs.custId = id;
    accountManagement.utils.hideTab6();
    this.renderTemplate(rs);
    this.html = "";
    this.strFunc = "";
    this.WriteDataAccountPolicy(rs, 0);
    $('#tbodyAccountPolicyData').append(this.html);
    //----------------------------------------------------------------
    //set no border bottom at last row
    $("#tbodyAccountPolicyData").find("tr").last().find("td").each(function (index, element) {
    $(element).get(0).style.borderBottom = "0px none";
    });
    //----------------------------------------------------------------
    $("#arrFuncId").val(this.strFunc.substring(1, this.strFunc.length));
    accountManagement.utils.showTab7();*/


    //-------------------------
    //  Using popup dom
    //-------------------------
    rs.groupId = gId;
    rs.groupName = dept;
    rs.userName = user;
    rs.custId = id;

    $("#divRolePolicy").empty();

    this.renderTemplate(rs);
    this.html = "";
    this.strFunc = "";
    this.WriteDataAccountPolicy(rs, 0);
    $('#tbodyAccountPolicyData').append(this.html);
    //----------------------------------------------------------------
    //set no border bottom at last row
    $("#tbodyAccountPolicyData").find("tr").last().find("td").each(function (index, element) {
        $(element).get(0).style.borderBottom = "0px none";
    });
    //----------------------------------------------------------------
    $("#arrFuncId").val(this.strFunc.substring(1, this.strFunc.length));

    accountManagement.utils.showPopupDomElement("#divAccountPolicy");
};

accountManagement.accountPolicy.onSubmit = function () {
    parent.BlockUI();
    document.fUpdatePolicy.submit();
};

accountManagement.accountPolicy.cancelUpdate = function () {
    accountManagement.utils.hidePopupDomElement("divAccountPolicy");
};

accountManagement.accountPolicy.WriteDataAccountPolicy = function (listFunction, level) {
    for (var i = 0; i < listFunction.length; i++) {
        this.strFunc += "," + listFunction[i].ID;

        this.html += "<tr>";

        this.html += "<td class='l indent" + level + "' style='text-align:left'>";
        if (level == 0) {
            this.html += "<div style='width:301px'>";
            this.html += listFunction[i].FuncDesc;
            this.html += "</div>";
        }
        else {
            this.html += "<span>";
            this.html += listFunction[i].FuncDesc;
            this.html += "</span>";
        }
        this.html += "</td>";
        var isRead = "";
        var isHost = "";
        var isGroupRead = "";
        var isGroupHost = "";
        if (listFunction[i].IsHost == true) { isHost = "checked"; }
        if (listFunction[i].IsRead == true) { isRead = "checked"; }
        if (listFunction[i].IsGroupHost == true) { isGroupHost = "checked"; }
        if (listFunction[i].IsGroupRead == true) { isGroupRead = "checked"; }
        this.html += "<td class='c'><div style='width:100px'><input type='checkbox' class='chkR' parentId='" + listFunction[i].ParentId + "' id='chkR" + listFunction[i].ID + "' name='chkR" + listFunction[i].ID + "' " + isRead + " onclick='chkCheckRead(this);'></div></td>";
        this.html += "<td class='c'><div style='width:100px'><input type='checkbox' class='chkH' parentId='" + listFunction[i].ParentId + "' id='chkH" + listFunction[i].ID + "' name='chkH" + listFunction[i].ID + "' " + isHost + " onclick='chkCheckHost(this);'></div></td>";
        this.html += "<td class='c'><div style='width:100px'><input type='checkbox' " + isGroupRead + " disabled></div></td>";
        this.html += "<td class='c'><div style='width:82px'><input type='checkbox' " + isGroupHost + " disabled></div></td>";
        this.html += "</tr>";

        this.WriteDataAccountPolicy(listFunction[i].SubFuncs, level + 1);
    }
};

accountManagement.viewAllAccount.saveChange = function (custId, groupid, firstname, lastname, successCallBack, errCallBack) {
    parent.BlockUI();
    $.ajax({
        type: "POST",
        url: "UpdateAccount.ashx?type=edit",
        async: true,
        data: '{\"custId\":\"' + custId + '\", \"groupid\":\"' + groupid + '\", \"firstname\":\"' + firstname + '\", \"lastname\":\"' + lastname + '\"}',
        success: function () {
            parent.jQuery.unblockUI();
            if (typeof successCallBack != "undefined" && successCallBack != null)
                successCallBack();
        }
        , error: function (e) {
            parent.jQuery.unblockUI();
            if (typeof errCallBack != "undefined" && errCallBack != null)
                errCallBack(e);
        }
    });
};

accountManagement.viewAllAccount.endEditMode = function (target) {
    var trElement = $(target).parents("tr")[0];
    $(trElement).RemoveBubblePopup();
    $(trElement).find("td [editor]").remove();
    $(trElement).find("td [canchange]").show();
    $(trElement).find("img[imgedit]").show();
};

accountManagement.viewAllAccount.initSearchingArea = function () {
    var arr = ["All", "0-9", "A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M",
              "N", "O", "P", "Q", "R", "S", "T", "U", "V", "W", "X", "Y", "Z"];
    for (var i = 0; i < arr.length; ++i) {
        $("#listAlpha").append("<option value='" + arr[i] + "'>" + arr[i] + "</option>");
    }

    var searchFunction = (function (e) {
        var target = $("#txtSearch").get(0);

        if (e.keyCode == 13 || $(accountManagement.utils.getTarget(e)).attr("type") == "button") {
            if ($(target).val() == "All" || $(target).val() == "") {
                this.restoreData();
                if ($("#listAlpha").val() != "All") {
                    if ($("#listAlpha").val() == "0-9") {
                        for (var i = 0; i < 10; ++i) {
                            this.viewModel.dataTable = jlinq.from(this.viewModel.dataTable).starts("user", "'" + i + "'").select();
                        }
                    }
                    else {
                        this.viewModel.dataTable = jlinq.from(this.viewModel.dataTable).starts("user", $("#listAlpha").val()).select();
                    }
                }
                this.renderTemplate();
            }
            else {
                var searchAccount = (function () {
                    this.viewModel.dataTable = jlinq.from(this.viewModel.dataTable).contains("user", $(target).val()).select();
                    if ($("#listAlpha").val() != "All") {
                        if ($("#listAlpha").val() == "0-9") {
                            for (var i = 0; i < 10; ++i) {
                                this.viewModel.dataTable = jlinq.from(this.viewModel.dataTable).starts("user", "'" + i + "'").select();
                            }
                        }
                        else {
                            this.viewModel.dataTable = jlinq.from(this.viewModel.dataTable).starts("user", $("#listAlpha").val()).select();
                        }
                    }
                }).bind(this);
                searchAccount();
                if (this.viewModel.dataTable.length <= 0) {
                    this.restoreData();
                    searchAccount();
                    if (this.viewModel.dataTable.length <= 0) {
                        this.renderTemplate();
                    }
                }
                else {
                    this.renderTemplate();
                    accountManagement.utils.fillScrollBar("divViewAllAccountsTemplate");
                }
            }
        }
        else if (accountManagement.utils.isAlphanumeric(e) || e.keyCode == 8) {
            if ($(target).get(0).timeoutID) {
                clearTimeout($(target).get(0).timeoutID);
            }
            $(target).get(0).timeoutID = setTimeout((function () {
                if ($(target).val() == "" || $(target).val().toLowerCase() == "all") {
                    return;
                }
                var accountstMatched = jlinq.from(this.viewModel.dataTable).starts("user", $(target).val()).select();
                if (accountstMatched.length <= 0) {
                    this.restoreData();
                    accountstMatched = jlinq.from(this.viewModel.dataTable).starts("user", $(target).val()).select();
                }
                accountManagement.utils.autoComplete(target, accountstMatched, "user");
                clearTimeout($(target).get(0).timeoutID);
            }).bind(this), 500);
        }
    }).bind(this);

    //btnSearch
    $("#btnGo").click(searchFunction);

    //Keyword
    $("#txtSearch").blur(function () {
        if ($(this).val() == "")
            $(this).val("All");
    })
    .focus(function () {
        if ($(this).val() == "All")
            $(this).val("");
    })
    .keyup(searchFunction);
};

accountManagement.viewAllAccount.updateStatus = function (custId, groupId, dept) {
    try {
        if (accountManagement.utils.readCookie("ACCOUNT_MANAGEMENT.CURRENT_TAB") == null){
            var now = new Date();
            //now.setDate(now.getDate() + 1);
            accountManagement.utils.createCookie("ACCOUNT_MANAGEMENT.CURRENT_TAB", "VIEW ALL ACCOUNTS", now);
        }
        var obj = document.getElementById(String(custId) + '_closed');
        obj.checked = !obj.checked;
        var strClosed = obj.checked;

        var _URL = 'UpdateAccount.ashx?type=status&groupId=' + groupId + '&custid=' + custId + '&active=' + strClosed + '&dept=' + dept;
        var f = self.frames['frUpd'];
        parent.BlockUI();
        if (typeof f != 'undefined' && f) f.location.replace(_URL);
    }
    catch (ex) {
        accountManagement.utils.showAlertMessage("errfunc", ex, 1);
    }
};

accountManagement.viewAllAccount.initSortting = function (container) {
    var arrSortableElement = $(container).find("[sortable]");
    var noHeader = $(arrSortableElement.get(0));
    var userNameHeader = $(arrSortableElement.get(1));
    var creatorHeader = $(arrSortableElement.get(2));
    var createdDateHeader = $(arrSortableElement.get(3));
    var departmentHeader = $(arrSortableElement.get(4));

    //-------------------------------------------------------------------------------
    accountManagement.utils.showTooltip(noHeader, "Sort by no");
    noHeader.HideBubblePopup();
    //accountManagement.utils.showTooltip(userNameHeader, "Sort by User Name");
    //userNameHeader.HideBubblePopup();
    accountManagement.utils.showTooltip(creatorHeader, "Sort by creator");
    creatorHeader.HideBubblePopup();
    accountManagement.utils.showTooltip(createdDateHeader, "Sort by created date");
    createdDateHeader.HideBubblePopup();
    accountManagement.utils.showTooltip(departmentHeader, "Sort by department");
    departmentHeader.HideBubblePopup();
    //-------------------------------------------------------------------------------
    //Sort by no
    var sortOrderNo = "ASC";
    noHeader.click((function () {
        parent.BlockUI();
        if (sortOrderNo == "ASC") {
            jlinq.from(this.viewModel.dataTable).sort("-no").select();
            sortOrderNo = "DESC";
        }
        else {
            jlinq.from(this.viewModel.dataTable).sort("no").select();
            sortOrderNo = "ASC";
        }
        this.renderTemplate();
        accountManagement.utils.fillScrollBar("divViewAllAccountsTemplate");
        parent.jQuery.unblockUI();
    }).bind(this));

    //Sort by user name
    var sortOrderUserName = "ASC";
    userNameHeader.click((function () {
        parent.BlockUI();
        if (sortOrderUserName == "ASC") {
            jlinq.from(this.viewModel.dataTable).sort("-user").select();
            sortOrderUserName = "DESC";
        }
        else {
            jlinq.from(this.viewModel.dataTable).sort("user").select();
            sortOrderUserName = "ASC";
        }
        this.renderTemplate();
        accountManagement.utils.fillScrollBar("divViewAllAccountsTemplate");
        parent.jQuery.unblockUI();
    }).bind(this));

    //Sort by creator
    var sortOrderCreator = "ASC";
    creatorHeader.click((function () {
        parent.BlockUI();
        if (sortOrderCreator == "ASC") {
            jlinq.from(this.viewModel.dataTable).sort("-creator").select();
            sortOrderCreator = "DESC";
        }
        else {
            jlinq.from(this.viewModel.dataTable).sort("creator").select();
            sortOrderCreator = "ASC";
        }
        this.renderTemplate();
        accountManagement.utils.fillScrollBar("divViewAllAccountsTemplate");
        parent.jQuery.unblockUI();
    }).bind(this));

    //Sort by created date
    var sortOrderCreatedDate = "ASC";
    createdDateHeader.click((function () {
        parent.BlockUI();
        if (this.viewModel.dataTable.length > 0 && !this.viewModel.dataTable[0].createdDateObj) {
            $(this.viewModel.dataTable).each(function (index, element) {
                element.createdDateObj = new Date(element.createddate);
            });
        }
        if (sortOrderCreatedDate == "ASC") {
            jlinq.from(this.viewModel.dataTable).sort("-createdDateObj").select();
            sortOrderCreatedDate = "DESC";
        }
        else {
            jlinq.from(this.viewModel.dataTable).sort("createdDateObj").select();
            sortOrderCreatedDate = "ASC";
        }
        this.renderTemplate();
        accountManagement.utils.fillScrollBar("divViewAllAccountsTemplate");
        parent.jQuery.unblockUI();
    }).bind(this));

    //Sort by department
    var sortOrderDepartment = "ASC";
    departmentHeader.click((function () {
        parent.BlockUI();
        if (sortOrderDepartment == "ASC") {
            jlinq.from(this.viewModel.dataTable).sort("-dept").select();
            sortOrderDepartment = "DESC";
        }
        else {
            jlinq.from(this.viewModel.dataTable).sort("dept").select();
            sortOrderDepartment = "ASC";
        }
        this.renderTemplate();
        accountManagement.utils.fillScrollBar("divViewAllAccountsTemplate");
        parent.jQuery.unblockUI();
    }).bind(this));
};

accountManagement.viewAllAccount.restoreData = function () {
    this.viewModel = accountManagement.utils.Clone(this.viewModelBackup);
};

accountManagement.viewAllAccount.synchronousData = function (record) {
    var records = jlinq.from(this.viewModel.dataTable).equals("custid", record.custid).select();
    if (records.length > 0) {
        records[0].fname = record.fname;
        records[0].lname = record.lname;
        records[0].groupid = record.groupid;
        records[0].dept = record.dept;
    }
};

accountManagement.viewAllAccount.renderTemplate = function (viewModel) {
    this.container = document.getElementById("divViewAllAccount");
    if (viewModel) {
        this.viewModel = viewModel;
    }
    if (!this.viewModelBackup) {
        this.viewModelBackup = accountManagement.utils.Clone(this.viewModel);
    }
    this.initSortting(this.container);
    ko.applyBindings(this.viewModel, this.container);
    accountManagement.utils.Custom.init(this.container);
    $(this.container).show();
    var timeoutID = setTimeout(function () {
        accountManagement.utils.fillScrollBar("divViewAllAccountsTemplate");
        clearTimeout(timeoutID);
    }, 200);

};

accountManagement.utils.CheckScrollBar = function (ObjectId, Aces) {
    if (!Aces) { Aces = "either x or y"; }
    return this.IsScrollBarExist(ObjectId, Aces);
};

accountManagement.utils.fillScrollBar = function (templateID) {
    var endColumn = $("#" + templateID).find("div[flagEndCol='endCol']");
    var currentLastColumnWidth = endColumn.width();
    var oldWidth = parseInt(endColumn.attr("oldwidth"));
    if (accountManagement.utils.IsScrollBarExist(templateID, "y")) {
        $("#" + templateID).find("div[flagEndCol='endCol']").width(oldWidth - accountManagement.utils.getScrollbarWidth());
    }
    else {
        if (currentLastColumnWidth < oldWidth) {
            $("#" + templateID).find("div[flagEndCol='endCol']").width(currentLastColumnWidth + accountManagement.utils.getScrollbarWidth());
        }
    }
};

accountManagement.utils.changeSrcImage = function (target, newSrc) {
    $(target).attr("src", "");
    setTimeout(function () {
        $(target).attr("src", newSrc);
    }, 100);
};

accountManagement.utils.IsScrollBarExist = function (ObjectId, Aces) {
    if (!ObjectId) return false;
    if (typeof ObjectId != "string") return false;
    if (accountManagement.utils.isIE()) {
        var hasScrollbar = function (elementId) {
            var div = document.getElementById(elementId); // need real DOM Node, not jQuery wrapper
            if (!div) return false;
            var hasVerticalScrollbar = div.scrollHeight > div.clientHeight + 1;
            var hasHorizontalScrollbar = div.scrollWidth > div.clientWidth + 1;
            return { "hasVerticalScrollbar": hasVerticalScrollbar, "hasHorizontalScrollbar": hasHorizontalScrollbar };
        };
        var srcollbarDetechResult = hasScrollbar(ObjectId);
        switch (Aces) {
            case 'x':
                return srcollbarDetechResult.hasHorizontalScrollbar;
                break;
            case 'y':
                return srcollbarDetechResult.hasVerticalScrollbar;
                break;
            default:
                return false;
                break;
        }
    }
    var Object = document.getElementById(ObjectId);
    if (!Object) return false;
    switch (Aces) {
        case 'x':
            Object.scrollLeft = 1;
            if (Object.scrollLeft > 0) { Object.scrollLeft = 0; return true; }
            else { return false; }
            break;
        case 'y':
            Object.scrollTop = 1;
            if (Object.scrollTop > 0) { Object.scrollTop = 0; return true; }
            else { return false; }
            break;
        case 'xy':
            Object.scrollTop = 1;
            Object.scrollLeft = 1;
            if (Object.scrollTop > 0 && Object.scrollLeft > 0) { Object.scrollTop = 0; Object.scrollLeft = 0; return true; }
            else { return false; }
            break;
        default:
            Object.scrollTop = 1;
            Object.scrollLeft = 1;
            if (Object.scrollTop > 0 || Object.scrollLeft > 0) { Object.scrollTop = 0; Object.scrollLeft = 0; return true; }
            else { return false; }
            break;
    }
};

accountManagement.utils.getTarget = function (e) {
    if (!e) {
        return null;
    }
    e = e || window.event;
    return e.target || e.srcElement;
};

accountManagement.utils.getScrollbarWidth = function () {
    var scrollbarWidth;
    var div = $('<div style="width:50px;height:50px;overflow:hidden;position:absolute;top:-200px;left:-200px;"><div style="height:100px;"></div></div>');
    $('body').append(div);
    var w1 = $('div', div).innerWidth();
    div.css('overflow-y', 'auto');
    var w2 = $('div', div).innerWidth();
    $(div).remove();
    scrollbarWidth = (w1 - w2);
    return scrollbarWidth;
};

accountManagement.utils.showTab6 = function () {
    accountManagement.utils.hideTab7();
    $('#tabs_menu').find("a[href='#tabs-6']").parent().show();
    $('#tabs').tabs("select", 5);
};

accountManagement.utils.showTab7 = function () {
    accountManagement.utils.hideTab6();
    $('#tabs_menu').find("a[href='#tabs-7']").parent().show();
    $('#tabs').tabs("select", 6);
};

accountManagement.utils.hideTab7 = function () {
    $("#divAccountPolicy").empty();
    $('#tabs_menu').find("a[href='#tabs-7']").parent().hide();
    $('#tabs').tabs("select", 0);
};

accountManagement.utils.hideTab6 = function () {
    $("#divRolePolicy").empty();
    $('#tabs_menu').find("a[href='#tabs-6']").parent().hide();
    $('#tabs').tabs("select", 0);
};

accountManagement.utils.showAlertMessage = function (containerid, msg, type) {
    var _class = "";
    switch (type) {
        case 0:
            {
                _class = "alertboxscc";
                break;
            }
        case 1:
            {
                _class = "alertboxerr";
                break;
            }
        case 2:
            {
                _class = "alertboxwrn";
                break;
            }
    }
    //$("#" + containerid).html("<div class='" + _class + "'><div>" + msg + "</div></div>").show();
    $.msg({
        content: "<div type='message.custom' class='" + _class + "'><div>" + msg + "</div></div>"
        , fadeOut: 200
        , timeOut: 2000
        , klass: 'white-on-white'
    });
    //$("#jquery-msg-bg").remove();
    /*var timeoutID = setTimeout(function () {
    clearTimeout(timeoutID);
    $("#" + containerid).hide();
    }, 10000);*/
};

accountManagement.utils.showTooltip = function (obj, content) {
    if (!obj) return;
    obj = $(obj);
    obj.CreateBubblePopup();
    obj.ShowBubblePopup({
        innerHtml: content,
        alwaysVisible: false,
        closingDelay: 200,
        themeName: 'azure',
        themePath: '../js/jQuery/Bubblepopup/images/jquerybubblepopup-theme'
    });
};

accountManagement.utils.isIE = function () {
    if (navigator.appName == "Microsoft Internet Explorer") {
        return true;
    }
    return false;
};

accountManagement.utils.autoComplete = function (target, arrayValues, key) {
    if (!target) return;
    if (typeof target == "string") {
        if (target.indexOf("#") < 0)
            target = $("#" + target);
        else target = $(target);
    }
    else {
        target = $(target);
    }

    if (arrayValues.length <= 0) return;

    $(target).attr("autocomplete", "off");
    $(target).parent().find("div[popupcontianer]").remove();
    $(target).parent().append("<div popupcontianer='true'></div>");
    var container = $(target).parent().find("div[popupcontianer]");

    var htmlList = "";
    var itemStyle = " style='padding-left:4px; padding-top:2px; padding-botton:2px;' ";

    for (var i = 0; i < arrayValues.length; ++i) {
        htmlList += "<div" + itemStyle + " index='" + (i + 1) + "' value='" + arrayValues[i][key] + "'>" + arrayValues[i][key] + "</div>";
    }

    container.append("<div style='border:1px solid #1C94CD; background-color:white;position:absolute;' listpopup='true'></div>");
    var divListPopup = container.find("div[listpopup]");

    divListPopup.width(target.width());
    divListPopup.empty().append(htmlList);
    var height = 0;
    divListPopup.find("div").each(function (index, element) {
        height += $(element).height() + 2;
    });
    divListPopup.height(height);
    divListPopup.get(0).selectedIndex = 0;
    //---------------------------------------------------------------
    var oldHover = null;
    divListPopup.find("div").hover(function (e) {
        if (oldHover != null) {
            $(oldHover).get(0).style.backgroundColor = "white";
        }
        var _target = accountManagement.utils.getTarget(e);
        $(_target).get(0).style.backgroundColor = "#DFDFDF";
        oldHover = _target;
        divListPopup.get(0).selectedIndex = parseInt($(_target).attr("index"));
    })
    .click(function (e) {
        var _target = accountManagement.utils.getTarget(e);
        $(target).val($(_target).attr('value'));
        container.remove();
    })
    .highlight($(target).val());
    //---------------------------------------------------------------
    target.keyup(function (e) {
        if (divListPopup.parent().parent().length <= 0) {
            return;
        }
        if (divListPopup.find("div").length <= 0 || divListPopup.get(0).style.display == "none") {
            return;
        }
        //down key
        if (e.keyCode == 40) {
            if (divListPopup.get(0).selectedIndex == arrayValues.length) {
                return;
            }
            divListPopup.get(0).selectedIndex += 1;
            if (oldHover) {
                $(oldHover).get(0).style.backgroundColor = "white";
            }
            if (divListPopup.find("div").length >= divListPopup.get(0).selectedIndex) {
                divListPopup.find("div:nth-child(" + divListPopup.get(0).selectedIndex + ")").get(0).style.backgroundColor = "#DFDFDF";
                oldHover = divListPopup.find("div:nth-child(" + divListPopup.get(0).selectedIndex + ")").get(0);
                $(target).val($(oldHover).attr('value'));
                target.focus();
                target.select();
            }
        }
        else if (e.keyCode == 38) {//up key
            if (divListPopup.get(0).selectedIndex == 1 || divListPopup.get(0).selectedIndex == 0) {
                return;
            }
            divListPopup.get(0).selectedIndex -= 1;
            if (oldHover) {
                $(oldHover).get(0).style.backgroundColor = "white";
            }
            if (divListPopup.find("div").length >= divListPopup.get(0).selectedIndex) {
                divListPopup.find("div:nth-child(" + divListPopup.get(0).selectedIndex + ")").get(0).style.backgroundColor = "#DFDFDF";
                oldHover = divListPopup.find("div:nth-child(" + divListPopup.get(0).selectedIndex + ")").get(0);
                $(target).val($(oldHover).attr('value'));
                target.focus();
                target.select();
            }
        }
        else if (e.keyCode == 13) {//Enter press
            if (oldHover) {
                $(target).val($(oldHover).attr('value'));
            }
            oldHover = null;
            container.remove();
        }
        else if (e.keyCode == 27) {//ESC press
            oldHover = null;
            container.remove();
        }
    });
    $(document).click(function () {
        oldHover = null;
        container.remove();
    });
    divListPopup.show();
};

accountManagement.utils.isAlphanumeric = function (e) {
    var key;
    if (e.keyCode) {
        key = e.keyCode;
    }
    else if (e.which) {
        key = e.which;
    }
    return /[A-Za-z0-9 ]/.test(String.fromCharCode(key));
};

accountManagement.utils.Clone = function (viewModel) {
    if (!viewModel) {
        return null;
    }
    var Clone = function () { };
    Clone.prototype = viewModel;
    var viewModelBackup = new Clone();

    var cloneObjects = function () {
        var arrayClonedObjects = new Array();
        for (var i = 0; i < viewModel.dataTable.length; ++i) {
            var _Clone = function () { };
            _Clone.prototype = viewModel.dataTable[i];
            arrayClonedObjects.push(new _Clone());
        }
        return arrayClonedObjects;
    };
    viewModelBackup.dataTable = cloneObjects();
    return viewModelBackup;
}

accountManagement.utils.Custom = {
    checkboxHeight : "25"
    , radioHeight : "25"
    , selectWidth : "190"
    , init: function (container) {
        var inputs = $(container).find("input"), span = Array(), textnode, option, active;
        for (var a = 0; a < inputs.length; a++) {
            if ((inputs[a].type == "checkbox" || inputs[a].type == "radio") && inputs[a].className == "styled") {
                span[a] = document.createElement("span");
                span[a].className = inputs[a].type == "checkbox" ? "checkboxstyled" : "radiostyled";
                if (inputs[a].checked == true) {
                    if (inputs[a].type == "checkbox") {
                        position = "0 -" + (this.checkboxHeight * 2) + "px";
                        span[a].style.backgroundPosition = position;
                    } else {
                        position = "0 -" + (this.radioHeight * 2) + "px";
                        span[a].style.backgroundPosition = position;
                    }
                }
                $(inputs[a].parentNode).find("span").remove();
                inputs[a].parentNode.insertBefore(span[a], inputs[a]);
                inputs[a].onchange = Custom.clear;
                if (!inputs[a].getAttribute("disabled")) {
                    span[a].onmousedown = Custom.pushed;
                    span[a].onmouseup = Custom.check;
                } else {
                    span[a].className = span[a].className += " disabled";
                }
            }
        }

        document.onmouseup = Custom.clear;
    },
    pushed: function (evt) {
        var evt1;
        if (null != window.event && 'object' == typeof (window.event)) {
            evt1 = event.button;
        }
        else {
            evt1 = evt.button;
        }
        if (evt1 < 2) {
            element = this.nextSibling;
            if (element.checked == true && element.type == "checkbox") {
                this.style.backgroundPosition = "0 -" + this.checkboxHeight * 3 + "px";
            } else if (element.checked == true && element.type == "radio") {
                this.style.backgroundPosition = "0 -" + this.radioHeight * 3 + "px";
            } else if (element.checked != true && element.type == "checkbox") {
                this.style.backgroundPosition = "0 -" + this.checkboxHeight + "px";
            } else {
                this.style.backgroundPosition = "0 -" + this.radioHeight + "px";
            }
        }
    },
    check: function (evt) {
        var evt1;
        if (null != window.event && 'object' == typeof (window.event)) {
            evt1 = event.button;
        }
        else {
            evt1 = evt.button;
        }
        if (evt1 < 2) {
            element = this.nextSibling;
            if (element.checked == true && element.type == "checkbox") {
                this.style.backgroundPosition = "0 0";
                element.checked = false;

            } else {
                if (element.type == "checkbox") {
                    this.style.backgroundPosition = "0 -" + this.checkboxHeight * 2 + "px";
                } else {
                    this.style.backgroundPosition = "0 -" + this.radioHeight * 2 + "px";
                    group = this.nextSibling.name;
                    inputs = document.getElementsByTagName("input");
                    for (a = 0; a < inputs.length; a++) {
                        if (inputs[a].name == group && inputs[a] != this.nextSibling) {
                            inputs[a].previousSibling.style.backgroundPosition = "0 0";
                        }
                    }
                }
                element.checked = true;
            }
            if (this.hasEvent(element, "onclick")) {
                element.click();
            }
        }

    },
    clear: function () {
        inputs = document.getElementsByTagName("input");
        for (var b = 0; b < inputs.length; b++) {
            if (inputs[b].type == "checkbox" && inputs[b].checked == true && inputs[b].className == "styled") {
                if (inputs[b].previousSibling.style)
                    inputs[b].previousSibling.style.backgroundPosition = "0 -" + this.checkboxHeight * 2 + "px";

            } else if (inputs[b].type == "checkbox" && inputs[b].className == "styled") {
                if (inputs[b].previousSibling.style)
                    inputs[b].previousSibling.style.backgroundPosition = "0 0";
            } else if (inputs[b].type == "radio" && inputs[b].checked == true && inputs[b].className == "styled") {
                if (inputs[b].previousSibling.style)
                    inputs[b].previousSibling.style.backgroundPosition = "0 -" + this.radioHeight * 2 + "px";
            } else if (inputs[b].type == "radio" && inputs[b].className == "styled") {
                if (inputs[b].previousSibling.style)
                    inputs[b].previousSibling.style.backgroundPosition = "0 0";
            }
        }
    },
    choose: function () {
        option = this.getElementsByTagName("option");
        for (d = 0; d < option.length; d++) {
            if (option[d].selected == true) {
                document.getElementById("select" + this.name).childNodes[0].nodeValue = option[d].childNodes[0].nodeValue;
            }
        }
    }
    , hasEvent: function (elem, evt) {
        if (elem != null) {
            if (eval('elem.' + evt)) {
                return true;
            }
        }
        return false;
    }
};

accountManagement.utils.hidePopupDomElement = function (target) {
    if (!target) {
        return;
    }
    else {
        if (typeof target == "string") {
            if (target.indexOf("#") >= 0) {
                target = $(target);
            }
            else {
                target = $("#" + target);
            }
        }
    }
    //---------------------------------------------------------------------------------
    $(target).hide();
    $(target).remove();
    if ($(target).get(0).actualParent) {
        $($(target).get(0).actualParent).append(target);
    }
    $("body").find("div[type='popup.dom']").remove();
};

accountManagement.utils.eraseCookie = function (name) {
	this.createCookie(name, "", -1);
};

accountManagement.utils.createCookie = function(name, value, days) {
	if (days) {
		var date = new Date();
		date.setTime(date.getTime()+(days*24*60*60*1000));
		var expires = "; expires="+date.toGMTString();
	}
	else var expires = "";
	document.cookie = name + "=" + value + expires + "; path=/";
};

accountManagement.utils.readCookie = function (name) {
	var nameEQ = name + "=";
	var ca = document.cookie.split(';');
	for(var i=0;i < ca.length;i++) {
		var c = ca[i];
		while (c.charAt(0) == ' ') c = c.substring(1, c.length);
		if (c.indexOf(nameEQ) == 0) return c.substring(nameEQ.length, c.length);
	}
	return null;
};

accountManagement.utils.showPopupDomElement = function (target) {
    if (!target) {
        return;
    }
    else {
        if (typeof target == "string") {
            if (target.indexOf("#") >= 0) {
                target = $(target);
            }
            else {
                target = $("#" + target);
            }
        }
    }
    $(target).get(0).actualParent = $(target).parent();
    //---------------------------------------------------------------------------------
    var createCover = function () {
        accountManagement.utils.hidePopupDomElement(target);
        $("body").append("<div type='popup.dom' level='cover'></div>");
        var divCover = $("body").find("div[type='popup.dom'][level='cover']").get(0);

        $(divCover).width($(window).width());
        $(divCover).height($(window).height());

        $(divCover).get(0).style.backgroundColor = "white";
        $(divCover).get(0).style.position = "absolute";
        $(divCover).get(0).style.zIndex = 9998;
        $(divCover).get(0).style.top = "0px";
        $(divCover).get(0).style.left = "0px";
        $(divCover).get(0).style.opacity = 0;
        $(divCover).get(0).style.filter = "alpha(opacity = 0)";

        $(window).resize(function () {
            $(divCover).width($(window).width());
            $(divCover).height($(window).height());
        });
    };

    var createContainer = function () {
        $("body").append("<div type='popup.dom' level='container'></div>");
        var divContainer = $("body").find("div[type='popup.dom'][level='container']").get(0);
        $(divContainer).get(0).style.position = "absolute";
        $(divContainer).get(0).style.top = "0px";
        $(divContainer).get(0).style.left = "0px";
        $(divContainer).get(0).style.zIndex = 9999;
        $(divContainer).width($(window).width());
        $(divContainer).height($(window).height());
        $(divContainer).attr("align", "center");

        $(window).resize(function () {
            $(divContainer).width($(window).width());
            $(divContainer).height($(window).height());
        });

        $(divContainer).append("<div level='content'></div>");
        var divContent = $(divContainer).find("div[level='content']");
        $(divContent).get(0).style.backgroundColor = "#334C5F";
        var margin = 80;
        $(divContent).get(0).style.marginTop = margin + "px";

        //---------------------------------------------------------------------------------
        //Create image closing icon
        $(divContent).append("<div type='close.icon'></div>");
        var closeIcon = $(divContent).find("div[type='close.icon']");
        $(closeIcon).get(0).style.display = 'block';
        $(closeIcon).get(0).style.background = 'url("images/fancybox.png") repeat scroll -40px 0 transparent';
        $(closeIcon).get(0).style.cursor = 'pointer';
        $(closeIcon).get(0).style.position = "absolute";
        var size = 30;
        $(closeIcon).get(0).style.height = size + 'px';
        $(closeIcon).get(0).style.width = size + 'px';

        $(closeIcon).click(function () {
            accountManagement.utils.hidePopupDomElement(target);
        });
        //---------------------------------------------------------------------------------
        //Append target
        $(target).remove();
        $(divContent).append(target);
        $(target).show();
        $(divContent).width($(target).width());
        $(divContent).height($(target).height());
        $(divContent).get(0).style.padding = "8px";
        $(divContent).corner();
        //$(divContent).draggable();
        //---------------------------------------------------------------------------------
        //when user drap inner divContent-> stop drag
        //Just enable drag when user drag on border of divContent
        $(target).mousedown(function () {
            //this.mouseDowning = true;
        });
        $(target).mouseup(function () {
            //this.mouseDowning = false;
        });
        $(target).mousemove(function (e) {
            if (typeof this.mouseDowning != "undefined" && this.mouseDowning == true) {
                var event = e || window.event;
                if (event.stopPropagation) {
                    //event.stopPropagation();
                } else {
                    //event.cancelBubble = true;
                }
            }
        });
        //---------------------------------------------------------------------------------
        if (navigator.appName == "Netscape") {
            $(closeIcon).get(0).style.left = $(target).width() + $(divContent).get(0).offsetLeft + 'px';
            $(closeIcon).get(0).style.top = margin - (size / 2) + "px";
        }
        else {
            $(closeIcon).get(0).style.left = $(target).width() + 'px';
            $(closeIcon).get(0).style.top = (-1 * (size/2)) + "px";
        }
    };
    createCover();
    createContainer();
    $(document).keyup(function(e){
        e = e || window.event;
        if (e.keyCode == 27){//ESC press
            if ($("body").find("div[type='popup.dom']").length > 0){
                accountManagement.utils.hidePopupDomElement(target);
            }
        }
    });
};

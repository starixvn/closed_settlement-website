var menuList;




function GetMenuList() {
    menuList = null;
    try { parent.BlockUI(); } catch (e) { }
    $.ajax({
        type: "POST",
        url: "../Services/WCF/ServiceMenu.svc/ManageMenuGetMenuList",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        data: '{"system":"' + $('#ddlSystem').val() + '"}',
        success: function (rs) {
            $('#tblcontent').html('');

            menuList = rs;

            WriteData(rs, 0);
            //add event for rows data only, ignore title row
            $(".tbllayout tr:not(:.title)").mouseover(function () {
                rowover(this)
            });
            $(".tbllayout tr:not(:.title)").mouseout(function ()
            { rowout() });
                        
        }
    });
}

function WriteData(listMenu, level) {
    for (var i = 0; i < listMenu.length; i++) {
        var strRow="";
        strRow="<tr>";
        
        strRow+="<td class='l indent" + level + "'><span>";
        strRow+=listMenu[i].Name;
        strRow += "</span></td>";

        strRow += "<td>";
        strRow += "<img class='hand' src='../App_Themes/Images/icn_add.png' onclick='ShowPopupAddMenu(" + listMenu[i].Id + ',"' + listMenu[i].FunctionName + '"' + ")' />";
        strRow += "</td>";

        strRow += "<td>";
        strRow += "<img class='hand' src='../App_Themes/Images/icn_edit.png' onclick='ShowPopUpEditMenu(" + listMenu[i].Id + ")' />";
        strRow += "</td>";

        strRow += "<td>";
        strRow += "<img class='hand' src='../App_Themes/Images/icn_del.png' onclick='deleteMenu(" + listMenu[i].Id + ")' />";
        strRow += "</td>";
        
        strRow+="<td>";
        strRow+=listMenu[i].FunctionName;
        strRow+="</td>";
        
        strRow+="<td class='l'>";
        strRow+=listMenu[i].Url;
        strRow += "</td>";

        strRow += "<td class='l'>";
        strRow += listMenu[i].System;
        strRow += "</td>";
        
        strRow+="<td>";
        strRow+=listMenu[i].Target;
        strRow+="</td>";
        
        strRow+="<td>";
        var ischeck;
        listMenu[i].IsHide==true ? ischeck="checked='checked'": ischeck="";
        strRow+="<input type='checkbox' disabled='disabled' " + ischeck  + " />";
        strRow+="</td>";
        
        strRow+="<td>";
        strRow+=listMenu[i].Order;
        strRow += "</td>";

        strRow += "<td>";
        strRow += listMenu[i].Level;
        strRow += "</td>";        
        
        strRow+="</tr>";
        
        $("#tblcontent").append(strRow);
        WriteData(listMenu[i].SubMenu, level+1);
    }
     try { parent.jQuery.unblockUI(); } catch (e) { }
 }


 function CheckDecNumber(e) {

     var code = "";
     if (e.which)
         code = e.which;
     else if (e.keyCode)
         code = e.keyCode;
     
     if ( code == 37 || code == 39 || code == 36 || code == 35) //e or E or "-"
         return true;
     if ((code < 48 || code > 57) && code != 8 && code != 13 && code != 0 && code != 46 && code != 9)
         return false;
     return true;
 }

function ShowPopupAddMenu(parentId, funcName) {
    if (!parentId)
        parentId = '0';
    if (!funcName)
        funcName = '';
    var url = "UserControl/AddMenu.aspx?edit=0&parentId=" + parentId + "&funcName=" + funcName + '&system=' + $('#ddlSystem').val();
    currSystem = $('#ddlSystem').val();
    showPopup("Manage Menu", url, "", "", 500, 300, false, 3000, function () { GetMenuList(); }); 
}
function ShowPopUpEditMenu(MenuId)
{
    
     var currMenu=new MenuItem();
     getMenuByID(menuList, MenuId, currMenu);

     var param = '';
     param += '&MenuId=' + MenuId;
     param += '&Url=' + encodeURIComponent(currMenu.Url);
     param += '&System=' + currMenu.System;
     param += '&Target=' + currMenu.Target;
     param += '&IsHide=' + currMenu.IsHide;
     param += '&Order=' + currMenu.Order;     
     param += '&Name=' + currMenu.Name;
     param += '&funcName=' + currMenu.FunctionName;

     var url = "UserControl/AddMenu.aspx?edit=1" + param;
     showPopup("Manage Menu", url, "", "", 500, 300, false, 3000, function () { GetMenuList(); }); 

     
 }


function deleteMenu(Id) {
   
    if (!confirm('Are you sure you want to delete this item menu?'))
        return;

    try { parent.BlockUI(); } catch (e) { }
    $.ajax({
        type: "POST",
        url: "../Services/WCF/ServiceMenu.svc/DelMenuItem",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        data: '{"menuId":' + Id + '}',
        success: function(rs) {
            var errcode = rs.ErrorCode;
            var errmsg = rs.ErrorMessage;
            if (errcode == 0) {
                showAlertMessage("errMsg", "Deleted Successfully", 0);
                setTimeout('GetMenuList();', 3000);                
            }
            else {
                showAlertMessage("errMsg", errmsg, 1);
            }
        }

    });

    parent.jQuery.unblockUI();
       
}

function getMenuByID(listMenu, id, refObjMenu)
{
   
    if (listMenu==null ||  listMenu.length==0)
    {
        return ;
    }
    for (var i = 0; i < listMenu.length; i++)
    {
        if (listMenu[i].Id==id)
        {
            refObjMenu.Id=listMenu[i].Id;
            refObjMenu.Name=listMenu[i].Name;
            refObjMenu.FunctionName=listMenu[i].FunctionName;
            refObjMenu.Url = listMenu[i].Url;
            refObjMenu.System = listMenu[i].System;
            refObjMenu.Target=listMenu[i].Target;
            refObjMenu.IsHide = listMenu[i].IsHide;            
            refObjMenu.Order = listMenu[i].Order;
            
            return ;
        }
       getMenuByID(listMenu[i].SubMenu, id, refObjMenu);
    }
}

function rowover(obj) {
    $(".tbllayout .overrow").removeClass("overrow");
    $(obj).addClass("overrow");
}
function rowout() {
    $(".tbllayout .overrow").removeClass("overrow");
}

function MenuItem() {
    
    this.Id=0;
    this.Name = "Name";
    this.ParentId = 0;
    this.FunctionName="";
    this.Url = "";
    this.System = "";
    this.Target="";
    this.IsHide=false;
    this.Order = 0;    
}


function IsValidParam() {

    $('#errMsg').html('');

    var arr = ['txtMenu', 'txtUrl', 'txtOrder', 'txtLevel'];
    for (i = 0; i < arr.length; i++) {
        if ($('#' + arr[i]).val() == '') {
            showAlertMessage('errMsg', arr[i].replace('txt', '') + ' can not be empty.', 1);
            $('#' + arr[i]).focus();
            return false;
        }
    }

    return true;
}

function InsertData() {
    var f = IsValidParam();
    var funcExcute = '';
    if (f) {       

        var currMenu = new MenuItem();
        currMenu.ParentId = $('#parentId').val();
        currMenu.Name = $('#txtMenu').val();
        currMenu.FunctionName = $('#ddlFunction').val();
        currMenu.Url = $('#txtUrl').val();
        currMenu.System = $('#system').val();
        currMenu.Target = $('#txtTarget').val();
        currMenu.IsHide = $('#chkIsHide').is(':checked');
        currMenu.Order = $('#txtOrder').val();        

        var rstext = JSON.stringify(currMenu);
        var data = '{"menu":' + rstext + '}';
    }
    else
        return;

    try { parent.BlockUI(); } catch (e) { }
    $.ajax({
        type: "POST",
        url: "../Services/WCF/ServiceMenu.svc/InsMenuItem" ,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        data: data,
        success: function(rs) {
            var errcode = rs.ErrorCode;
            var errmsg = rs.ErrorMessage;
            if (errcode == 0) {
                showAlertMessage("errMsg", "Inseted Successfully", 0);
                afterUpdate(true, 3000);
            }
            else {
                showAlertMessage("errMsg", errmsg, 1);
            }
        }

    });

    parent.jQuery.unblockUI();
}


function UdpData() {
    var f = IsValidParam();
    var funcExcute = '';
    if (f) {

        var currMenu = new MenuItem();
        currMenu.Id = $('#MenuId').val();        
        currMenu.Name = $('#txtMenu').val();
        currMenu.FunctionName = $('#ddlFunction').val();
        currMenu.Url = $('#txtUrl').val();        
        currMenu.Target = $('#txtTarget').val();
        currMenu.IsHide = $('#chkIsHide').is(':checked');
        currMenu.Order = $('#txtOrder').val();
        currMenu.System = $('#system').val();
        var rstext = JSON.stringify(currMenu);
        var data = '{"menu":' + rstext + '}';
    }
    else
        return;

    try { parent.BlockUI(); } catch (e) { }
    $.ajax({
        type: "POST",
        url: "../Services/WCF/ServiceMenu.svc/UdpMenuItem",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        data: data,
        success: function(rs) {
            var errcode = rs.ErrorCode;
            var errmsg = rs.ErrorMessage;
            if (errcode == 0) {
                showAlertMessage("errMsg", "Updated Successfully", 0);
                afterUpdate(true, 3000);
            }
            else {
                showAlertMessage("errMsg", errmsg, 1);
            }
        }

    });

    parent.jQuery.unblockUI();
}

function SubmitData() {
    var IsAddNew = $('#edit').val() == '0';
    if (IsAddNew) {
        InsertData();
    }
    else {
        UdpData();
    }
}

function LoadFuncGroup(funcname) {
    $.ajax({
        type: "POST",
        url: "../Services/WCF/ServiceMenu.svc/GetGroupFunc",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        data: '',
        success: function(listMenu) {
        var strGroupFunc = "<select id='ddlFunction' name='ddlFunction'>";

        for (i = 0; i < listMenu.length; i++) {
                if (funcname == listMenu[i].FunctionName)
                    strGroupFunc += "<option selected='selected' value='" + listMenu[i].FunctionName + "'>" + listMenu[i].FunctionName + "</option>";
                else
                    strGroupFunc += "<option value='" + listMenu[i].FunctionName + "'>" + listMenu[i].FunctionName + "</option>";
            }
            strGroupFunc += "</select>";
            $('#spFuncGroup').html(strGroupFunc);
        }

    });
}

function GetSystemList() {

    $.ajax({
        type: "POST",
        url: "../Services/WCF/HelperService.svc/GetSystemList",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        data: '',
        success: function (rs) {
            $("#systemTmpl").tmpl(rs).appendTo("#ddlSystem");            
        }
    });
}

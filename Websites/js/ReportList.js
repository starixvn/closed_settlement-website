var currentItem = 0;
var oldClassName = 'Bleft_Sub';
var newClassName = 'Bleft_SubA';

function obj(id) {
    return document.getElementById(id);
}

function menu_toggle(menu_id) {
    obj(menu_id).style.display = (obj(menu_id).style.display == 'none') ? '' : 'none';
    if (menu_id!='SubChildHR')
    {
        var aMenu = obj(menu_id + 'M');
        aMenu.className = (aMenu.className == 'Bleft_Parent') ? 'Bleft_ParentAc' : 'Bleft_Parent';
    }
}
function menu_toggleSub(menu_id) {
    obj(menu_id).style.display = (obj(menu_id).style.display == 'none') ? '' : 'none';
}

function clearCurrentLeftMenu() {
    var item = obj(currentItem);
    if (item) obj(currentItem).className = oldClassName;
}
function ChangeClass(menu_id) {
    clearCurrentLeftMenu();
    obj(menu_id).className = newClassName;
    if (currentItem != menu_id) {
        var item = obj(currentItem);
        if (item) obj(currentItem).className = oldClassName;
    }
    currentItem = menu_id;
}

function openRpt(url, ob) {
    ChangeClass(ob.id);
    var fdate = obj('fdate').value;
    var tdate = obj('tdate').value;
    if (url.indexOf('?') > -1)
        url = url + "&fdate=" + fdate + "&tdate=" + tdate;
    else
        url = url + "?fdate=" + fdate + "&tdate=" + tdate;
            
    //parent.BlockUI();
    parent.gotoURL('centerFr', url);
}

function openRptNew(url) {
    var fdate = obj('fdate').value;
    var tdate = obj('tdate').value;
    if (url.indexOf('?') > -1)
        url = url + "&fdate=" + fdate + "&tdate=" + tdate;
    else
        url = url + "?fdate=" + fdate + "&tdate=" + tdate;
    window.open(url, '_blank');
}
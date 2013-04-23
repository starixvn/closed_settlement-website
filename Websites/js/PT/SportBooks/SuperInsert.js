var chkcopy = new Array();
var chkCopyId = ["chkCopyHdpD1","chkCopy1x21", "chkCopyHdpL1"]
var PTType = ["listS", "listM", "listA"];
chkCopyTo = ["1x21", "HdpL1"];
chkcopy[chkCopyId[0]] = ["OUD1", "1stHdpD1", "1stOUD1", "OE1"];
chkcopy[chkCopyId[1]] = ["CS1", "TG1", "MP1", "Outright1"];
chkcopy[chkCopyId[2]] = ["OUL1", "1stHdpL1", "1stOUL1"];



$(document).ready(function() {
    var data = null;
    //parent.BlockUI();
    //Assign Super Info
    var SuperInfo = $("#SuperInfo");
    SuperInfo.setTemplateURL("../../App_Templates/Customer/PT/SportBooks/Super/Insert/Info.htm");
    SuperInfo.processTemplate(data);
    AssignCommission();
    AssignSite();
    AssignCurrency();

    //Assing Soccer PT
    var SuperSoccer = $("#SuperSoccer");
    SuperSoccer.setTemplateURL("../../App_Templates/Customer/PT/SportBooks/Super/Insert/SoccerPT.htm");
    SuperSoccer.processTemplate(data);
    AssignPT();
    //parent.jQuery.unblockUI();
});

function AssignCommission() {
    $(".commlist").one("mousedown", function() {
        var curr = $(this).val();
        var max = roundNumber($(this).attr("max"),4);
        CreateOption($(this), curr, max, 0, 0.0005, 4);
    });
}

function AssignPT()
{
    $(".select-pt").one("mousedown", function() {
        if ($("#" + $(this).attr("id") + " option").size() <= 1) {
            var curr = $(this).val();
            var max = $(this).attr("max");
            var min = $(this).attr("min");
            CreateOption($(this), curr, max, min, 0.01, 2);
        }
    });
}


function On_ChangeSuper(obj) {
    var Slist = $(obj);
    var Sid = Slist.attr("id");
    var Mlist = $("#" + Sid.replace("listS", "listM"));
    var Alist = $("#" + Sid.replace("listS", "listA"));
    var Svalue = Slist.val();
    Mlist.attr("max", Svalue);
    Alist.attr("max", Svalue);
    Mlist.val(Svalue);
    Alist.val(Svalue);
    CreateOption(Mlist, Svalue, Svalue, Mlist.attr("min"), 0.01, 2);
    CreateOption(Alist, Svalue, Svalue, Alist.attr("min"), 0.01, 2);
}
function On_ChangeMaster(obj) {
    var Mlist = $(obj);
    var Alist = $("#" + Mlist.attr("id").replace("listM", "listA"));
    var Mvalue=Mlist.val();
    Alist.attr("max", Mvalue);
    Alist.val(Mvalue);
    CreateOption(Alist, Mvalue, Mvalue, Alist.attr("min"), 0.01, 2);
}

function CopyPT(id, isCopy) {
    var arrType = chkcopy[id];
    for (var i=0; i < arrType.length; i++) {
        for (var j=0; j < PTType.length; j++) {
            var tmp = PTType[j] + arrType[i];
            if (isCopy) {
                $("#" + tmp).attr("disabled", "disabled");
            }
            else {
                $("#" + tmp).removeAttr("disabled");
            }
        }
    }
}
function CopyPositionTaking(obj) {
    CopyPT(obj.id, obj.checked);
}
function CopyAllPT(obj) {
    if (obj.checked) {
        for (var i = 0; i < chkCopyId.length; i++) {
            var tmp = $("#" + chkCopyId[i]);
            if (!tmp.attr("checked")) {
                CopyPT(chkCopyId[i], true);
            }
            tmp.attr("disabled", "disabled");
        }
        for (var x = 0; x < PTType.length; x++) {
            for (var y = 0; y < chkCopyTo.length; y++) {
                var tmp1 = $("#" + PTType[x] + chkCopyTo[y]);
                tmp1.attr("disabled", "disabled");
            }
        }
    }
    else {
        for (var i = 0; i < chkCopyId.length; i++) {
            var tmp = $("#" + chkCopyId[i]);
            if (tmp.attr("checked")) {
            }
            else {
                CopyPT(chkCopyId[i], false);
            }
            tmp.removeAttr("disabled");
        }
        for (var x = 0; x < PTType.length; x++) {
            for (var y = 0; y < chkCopyTo.length; y++) {
                var tmp1 = $("#" + PTType[x] + chkCopyTo[y]);
                tmp1.removeAttr("disabled");
            }
        }
    }

}
function ReducePT(id, step) {
    var obj = $("#" + id);
    var curr = obj.val();
    var min = obj.attr("min");
  
    if (obj.val() > min) {
        var newvalue = roundNumber((curr * 100 - step) / 100, 2);
        var max = obj.attr("max");
        if (newvalue < min) newvalue = curr;

        if ($("#" + id + " option").size() <= 1) {
            CreateOption(obj, newvalue, max, min, 0.01, 2);
        }
        else {
            $("#" + id + " option[value='" + newvalue + "']").attr("selected", "selected");
        }
        var f = id.substring(5, 4);
        if (f == 'S') On_ChangeSuper(document.getElementById(id));
        else if (f == 'M') On_ChangeMaster(document.getElementById(id));
    }
   
}

function IncreasePT(id, step) {
    var obj = $("#" + id);
    var curr = obj.val();
    var min = obj.attr("min");
    var max = obj.attr("max");
    if (obj.val() < max) {
        var newvalue = roundNumber((curr * 100 + step) / 100, 2);
        if (newvalue > max) newvalue = curr;
        if ($("#" + id + " option").size() <= 1) {
            CreateOption(obj, newvalue, max, min, 0.01, 2);
        }
        else {
            $("#" + id + " option[value='" + newvalue + "']").attr("selected", "selected");
        }
        var f = id.substring(5, 4);
        if (f == 'S') On_ChangeSuper(document.getElementById(id));
        else if (f == 'M') On_ChangeMaster(document.getElementById(id));
    }
  
}

function openGotoBox() {
    var offset = $('#GotoPage').offset();
    $("#page_menu").css("left", offset.left + "px");
    $("#page_menu").css("top", (offset.top + $('#GotoPage').height()+ 2) + "px");
    if ($("#page_menu").css("display") == "none") {
        $("#page_menu").css("display", "block");
    }
    else {
        $("#page_menu").css("display", "none");
    }
    
}

//check Numeric input field by id
function isNumberNotNull(id) {
    var result = false;
    try {
       
        if (!jQuery.trim($("#" + id).val())== "") {
            result = !isNaN($("#" + id).val());
        }
    } catch (error) {
}
    return result;
}

function PagerKeyPress(e) {

    if (!e) e = window.event;
    var code = (e.keyCode) ? e.keyCode : e.which;

    
    if (code == 69 || code == 101 || code == 45 || code == 37 || code == 39 || code == 36 || code == 35) //e or E or "-"
        return true;
    if ((code < 48 || code > 57) && code != 8 && code != 13 && code != 0 && code != 46 && code != 9) {
        return false;
    }
    
    if (code == 13) {
        try {
            $("#pagenav_ibtn").click();
            return true;
        } catch (error)
        {return false;}
    }
}
function CheckValidPage() {
    if (parseInt($('#txtpage').value) > parseInt($('#hdtotalpages').value)) {
        return false;
    }

    var currentPage = $('#currentpage').firstChild.nodeValue;
    currentPage = parseInt(currentPage.substring(0, currentPage.indexOf(':')));
    if (currentPage == parseInt($('#txtpage').value)) {
        return false;
    }

    return true;
}
function GotoPageNext(url) {
    if (CheckValidPage() == false) {
        return false;
    }

    var param = "";
    if ($('#txtpage').value != 0) {
        param = 'page=' + $('#txtpage').value;
        location.href = url + param;
    }
    else {
        $('#txtpage').focus();
    }
}

function clearMessage(containerid) {

    $("#" + containerid).html('');
}

function showAlertMessage(containerid, msg, type) {
    // Success=0, Error=1, Warning=2
    if (document.getElementById(containerid) != null) {
        switch (type) {
            case 0:
                $("#" + containerid).html("<div class='alertboxscc'><div>" + msg + "</div></div>");
                var t = setTimeout("clearMessage('" + containerid + "')", 6000);
                break;
            case 1:
                $("#" + containerid).html("<div class='alertboxerr'><div>" + msg + "</div></div>");
                break;
            case 2:
                $("#" + containerid).html("<div class='alertboxwrn'><div>" + msg + "</div></div>");
                break;
            default:
                break;
        }
        window.scroll(0, 0);
        parent.scroll(0, 0);

    }
}

function formatAlertMessage(msg, type) {
    // Success=0, Error=1, Warning=2
    switch (type) {
        case 0:
            return "<div class='msgcorrect'>" + msg + "</div>";
            break;
        case 1:
            return "<div class='msgerror'>" + msg + "</div>";
            break;
        case 2:
            return "<div class='msgwarning'>" + msg + "</div>";
            break;
        default:
            return "";
            break;
    }
}

function showEnrichMessage(containerid, ErrorHandler) {
    var isError500 = true;
    var isSuccess = true;
    var result = '';
    if (ErrorHandler != null && ErrorHandler.length > 0) {
        $.each(ErrorHandler, function (index, value) {
            if (value.Key > 0) {
                if (isError500) {
                    result += formatAlertMessage(value.Value, 1);
                }
                isSuccess = false;
            }
            else {
                result += formatAlertMessage(value.Value, 0);
            }
            if (value.Key == 500) isError500 = false;
        });
    }

    if (isSuccess) {
        result = '<div class="boxscc">' + result + '</div>';
        t1 = setTimeout("clearMessage('" + containerid + "')", 8000);
    }
    else {
        result = '<div class="boxerr">' + result + '</div>';
        t2 = setTimeout("clearSuccessMessages()", 8000);
    }

    $("#".concat(containerid)).html(result);

    window.scroll(0, 0);
    parent.scroll(0, 0);
    return isSuccess;
}

function clearSuccessMessages() {

    $("#err .msgcorrect").remove();
    clearTimeout(t2);
}
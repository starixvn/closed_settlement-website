function gotoURL(frmName, url) {
    $('#' + frmName).attr({ src: url });
}

function BlockUI(msg) {
    if (msg == null) {
        jQuery.blockUI({
            message: $('img#displayBox'),
            fadeIn: 0,
            fadeOut: 0,
            css: {
                top: ($(window).height() - 100) / 2 + 'px',
                left: ($(window).width() - 100) / 2 + 'px',
                width: '100px',
                border: 'none'
            }
        });
    } else {
    jQuery.blockUI({
        message: '<table style="border:solid 2px #000;background-color:#fff;"><tr><td align="center"><img id="displayBox" src="../App_Themes/Images/loading51.gif" alt=""/></td></tr><tr><td><b>' + msg + '</b></td></tr></table>',
        fadeIn: 0,
        fadeOut: 0,
        css: {
            top: ($(window).height() - 136) / 2 + 'px',
            left: ($(window).width() - 136) / 2 + 'px',
            border:'none'
        }
    });
    }
}

function resizeIframe() {
    fheight = $('.wrapper').height() - topheight;
    $('#centerFr').height(fheight + "px");
    $('#leftPanel').height(fheight + "px");
}

function changeiconleftright(showlefticon) {
    var btnleftright = $("#btnleftright");
    if (showlefticon) {
        btnleftright.removeClass("leftright");
        btnleftright.addClass("rightleft");
    }
    else {
        btnleftright.removeClass("rightleft");
        btnleftright.addClass("leftright");
    }
}

function leftright() {
    var leftpanel = $('.leftcontent');
    var rightpanel = $('.rightcontent');
   
    
    if (showleft) {
        leftcontent = 0;
        contentwidth = fullwidth - leftcontent-5;
        leftpanel.css("width", leftcontent + "px");
        rightpanel.css("width", contentwidth + "px");
        leftpanel.hide();
    }
    else {
        leftcontent = 239;
        contentwidth = fullwidth - leftcontent-5;
        leftpanel.css("width", leftcontent + "px");
        rightpanel.css("width", contentwidth + "px");
        leftpanel.show();
    }
    showleft = !showleft;
    changeiconleftright(showleft);
}

function update_online_status() {
    UpdateOnlineStatus.location.href = "../Handlers/OnlineUpd.ashx";
}
function QueryData(queryString, preserveDuplicates) {

    // if a query string wasn't specified, use the query string from the URL
    if (queryString == undefined) {
        queryString = location.search ? location.search : '';
    }

    // remove the leading question mark from the query string if it is present
    if (queryString.charAt(0) == '?') queryString = queryString.substring(1);

    // check whether the query string is empty
    if (queryString.length > 0) {

        // replace plus signs in the query string with spaces
        queryString = queryString.replace(/\+/g, ' ');

        // split the query string around ampersands and semicolons
        var queryComponents = queryString.split(/[&;]/g);

        // loop over the query string components
        for (var index = 0; index < queryComponents.length; index++) {

            // extract this component's key-value pair
            var keyValuePair = queryComponents[index].split('=');
            var key = decodeURIComponent(keyValuePair[0]);
            var value = keyValuePair.length > 1
                       ? decodeURIComponent(keyValuePair[1])
                       : '';

            // check whether duplicates should be preserved
            if (preserveDuplicates) {

                // create the value array if necessary and store the value
                if (!(key in this)) this[key] = [];
                this[key].push(value);

            } else {

                // store the value
                this[key] = value;

            }

        }

    }

}
$(function () {
    (function ShowUrl() {
        var getData = new QueryData(top.location.search, true);
        if (typeof getData['menuUrl'] != 'undefined') {
            var fullDate = new Date()
            //convert month to 2 digits
            //var twoDigitMonth = ((fullDate.getMonth().length + 1) === 1) ? (fullDate.getMonth() + 1) : '0' + (fullDate.getMonth() + 1);

            var currentDate = (fullDate.getMonth() + 1)  + "/" + fullDate.getDate() + "/" + fullDate.getFullYear();

            $('#centerFr').attr("src", getData['menuUrl'] + "?fdate=" + currentDate + "&tdate=" + currentDate);
        }
    })();
});

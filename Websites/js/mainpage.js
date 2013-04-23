function trimSpace(str) {
    return str.replace(/\s/g, "");
};

var timerId = "";
var isonauto = false;
var showleft = false;
var fullheight, contentheight, fullwidth, contentwidth;
var bannerbox = 43;
var leftcontent = 0;
var autosearch;
var mainMenu = '';

function submitSearch(username) {
    txtusername = $('#txtuserName');
    if (username == null || username == "") {
        username = encodeURIComponent(escape(jQuery.trim(txtusername.val())));        

        if (username != "") {
            $('#leftPanel').attr({ src: 'CustInfo.aspx?username=' + username });
            Set_Cookie(accid + "_custname", unescape(username));
        }
        else {
            username = Get_Cookie(accid + "_custname");
            if (username != "") {
                $('#leftPanel').attr({ src: 'CustInfo.aspx?username=' + username });
            }
        }
    }
    else {
        
        username = username.replace("&amp;", "&");
        Set_Cookie(accid + "_custname", username);
        $('#leftPanel').attr({ src: 'CustInfo.aspx?username=' + jQuery.trim(escape(username)) });
    }

    if (!showleft) {
        leftright(); 
    }
    txtusername.val("");
}

function gotoURL(frmName, url) {
    $('#' + frmName).attr({ src: url });
}
 
function BlockUI(msg) {    
    if (msg == null || msg == "") {
        jQuery.blockUI({
            message: '<table class="blockUI"><tr><td class="tdloading"></td></tr></table>',
            fadeIn: 0,
            fadeOut: 0,
            css: {
                top: ($(window).height() - 68) / 2 + 'px',
                left: ($(window).width() - 68) / 2 + 'px',
                border: 'none'
            }
        });
    } else {
        jQuery.blockUI({
            message: '<table class="blockUI"><tr><td class="tdloading"></td></tr><tr><td><b>' + msg + '</b></td></tr></table>',
            fadeIn: 0,
            fadeOut: 0,
            css: {
                top: ($(window).height() - 68) / 2 + 'px',
                left: ($(window).width() - 68) / 2 + 'px',
                border: 'none'
            }
        });
    }
}

function MainMenu_Click(item, url) {
    url = trimSpace(url);

    $("#mainmenu .mcurrent").removeClass("mcurrent");
    $("#" + item.id).addClass("mcurrent");
    $("#" + item.id + " .mcurrent").css("outline", "0");
    if (url.indexOf(".aspx") > 0) {
        gotoURL('centerFr', url);
    }
}

function Submenu_Click(url) {
//if (url.indexOf(".aspx") > 0) {
    gotoURL('centerFr', url);
//}
}

function KeyPress(e) {
    e = (e) ? e : window.event;
    var key = (e.keyCode) ? e.keyCode : e.which;
    if (key == 13) {
        submitSearch();
    }
    return;
}

function overlay(id) {
    el = $("#" + id);
    el.css("display", el.css("display") == "inline" ? "none" : "inline");
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
        contentwidth = fullwidth - leftcontent;
        leftpanel.css("width", leftcontent + "px");
        rightpanel.css("width", contentwidth + "px");
        leftpanel.hide();
    }
    else {
        leftcontent = 273;
        contentwidth = fullwidth - leftcontent;
        leftpanel.css("width", leftcontent + "px");
        rightpanel.css("width", (contentwidth-3) + "px");
        leftpanel.show();
    }
    showleft = !showleft;
    changeiconleftright(showleft);
}

function update_online_status() {
    UpdateOnlineStatus.location.href = "Handlers/OnlineUpd.ashx";
}

jQuery(document).ready(function () {

    pre_resize();
    $(".subtopmenu").width((fullwidth - 280) + "px");

    //Add events to page
    $(window).resize(function () {
        pre_resize();
    });

    //Gen menu

    $.ajax({
        type: "POST",
        url: "Services/WCF/ServiceMenu.svc/GetMenuList",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        data: '',
        success: function (rs) {
            if (rs != null) {
                renderMenu(rs, rs, 0);
                $('#mainmenu').html(mainMenu);
            }
            else {
                LogOut();
                setTimeout('RequestLogin()', 2000);
            }
        }
    });


});

var currentParentId = 0;
function renderMenu(parent, all, level) {

    if (parent && parent.length > 0) {
        var url = '';
        var target = '';
        var parentId = '';
        var parentIndex = 0;
        var menuClass = '';
        var downIcon = '';


        if (level == 0)
            mainMenu += '<ul id="nav">';
        else
            mainMenu += '<ul>';

        var total = parent.length;
        for (var i = 0; i < total; i++) {
            var menu = parent[i];
            var totalChild = menu.SubMenu.length;
            menuClass = '';
            parentId = 'id="pId' + parentIndex + '"';

            url = trimSpace(menu.Url);
            url = url.toLowerCase().indexOf('n/a') > -1 ? 'javascript:void(0)' : url;

            target = menu.Target == '' ? 'centerFr' : menu.Target;

            if (level >= 1) {
                if (totalChild > 1) {
                    menuClass = ' class="detail"';
                }
                else if (totalChild == 1) {
                    menuClass = ' class="single detail"';
                }
            }


            mainMenu += ('<li ' + (level == 0 ? parentId : '') + menuClass + '>\
                        <a href="' + url + '" target="' + target + '" onclick="addParentActiveClass(\'pId' + currentParentId + '\')">' + menu.Name + '</a>');

            renderMenu(menu.SubMenu, all, level + 1);
            mainMenu += '</li>';

            if (level != 0) continue;
            parentIndex++;
            currentParentId = parentIndex;

        }
        mainMenu += '</ul>';
    }
}
function addParentActiveClass(parentId) {
    BlockUI();
    $('#nav li').removeClass('mcurrent');
    $('#nav #' + parentId).addClass('mcurrent');
    jQuery.unblockUI();
}


function RequestLogin() {
    window.location.href = "SignIn.aspx";
}

function LogOut() {
    $.ajax({
        type: "POST",
        url: "PreLogin/LogOut.ashx",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        data: '',
        success: function (rs) {

        }
    });
}

function pre_resize() {
    fullheight = $(".fullsize").height();
    fullwidth = $(".fullsize").width();
    contentheight = fullheight - bannerbox;
    contentwidth = fullwidth - leftcontent;


    $(".contentbox").height(contentheight + "px");
    $(".rightcontent").width((contentwidth - 3) + "px");

    $('#centerFr').height(contentheight + "px");
    $('#leftPanel').height(contentheight + "px");
}


(function ($) {
    var cache = [];
    // Arguments are image paths relative to the current page.
    $.preLoadImages = function () {
        var args_len = arguments.length;
        for (var i = args_len; i--; ) {
            var cacheImage = document.createElement('img');
            cacheImage.src = arguments[i];
            cache.push(cacheImage);
        }
    }
})(jQuery)
jQuery.preLoadImages("App_Themes/Images/Banner/out.gif", "App_Themes/Images/Banner/in.gif");

function CheckIsMenuIgnore(nameName) {
    for (var k = 0; k < arrayMenuNameIgnore.length; k++) {
        if (arrayMenuNameIgnore[k] == nameName)
            return true;
    }
    return false
};

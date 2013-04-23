/*---------Notes---------*/
//must use jquery.blockUI.1.33.js

function showPopup(options) {
    var p = new popup();
    p.showPopup(options);
    return p;
}

function closePopup(id) {
    if (typeof id == "undefined") {
        jQuery(document).find("div[ugpopup]").each(function () {
            closePopup(jQuery(this).attr("id"));
        });
    }
    else {
        if (jQuery("#" + id).length <= 0) return;
        jQuery("#" + id).get(0).popup.closePopup();
    }
}

function popup() {
    this.imgPath = "";
    this.content = "";
    this.width = 0;
    this.height = 0;
    this.title = "";
    this.titleColor = "black";
    this.backgroundColor = "#E8F4FE";
};

popup.id = 0;

popup.prototype.showPopup = function (options) {
    this.closePopup();

    if (typeof options == "undefined") options = {};
    if (typeof options.content == "undefined" || options.content == null) options.content = "";
    if (typeof options.title == "undefined" || options.title == null) options.title = "";
    if (typeof options.width == "undefined" || options.width == null) options.width = 600;
    if (typeof options.height == "undefined" || options.height == null) options.height = 800;
    if (typeof options.buttons == "undefined" || options.buttons == null) options.buttons = [];
    if (typeof options.multiple == "undefined" || options.multiple == null) options.multiple = false;
    if (typeof options.dialog == "undefined" || options.dialog == null) options.dialog = false;
    if (typeof options.onload == "undefined" || options.onload == null) options.onload = function () { };
    if (typeof options.onclose == "undefined" || options.onclose == null) options.onclose = function () { };

    this.content = options.content;
    this.title = options.title;
    this.width = options.width;
    this.height = options.height;
    this.buttons = options.buttons;
    this.multiple = options.multiple;
    this.dialog = options.dialog;
    this.onload = options.onload;
    this.onclose = options.onclose;

    this.imgPath = this.getAppPath() + "js/Popup/Images/";

    popup.id += 1;

    if (!this.multiple) {
        jQuery(document).find("body").find("div[ugpopup]").remove();
    }

    jQuery("div[class='blockUI blockOverlay']").height(jQuery(document).height());

    jQuery(document).find("body").append("<div id='ugpopup" + popup.id + "' ugpopup='true' style='z-index:10000'></div>");

    this.divPopup = jQuery("#ugpopup" + popup.id);

    jQuery(this.divPopup).get(0).popup = this;

    jQuery(this.divPopup).width(this.width).height(this.height);

    //this.overlayer(jQuery(this.divPopup));
    this.head(jQuery(this.divPopup));
    this.body(jQuery(this.divPopup));
    this.bottom(jQuery(this.divPopup));
    this.addButtons(jQuery(this.divPopup));

    jQuery(this.divPopup).show();
    jQuery(this.divPopup).CenterIt();
    jQuery(this.divPopup).css("position", "absolute");
    jQuery(this.content).show();

    this.onload();

    return jQuery(this.divPopup).attr("id");
};

popup.prototype.reCenter = function () {
    jQuery(this.divPopup).css("position", "");
    jQuery(this.divPopup).get(0).CenterItem();
    jQuery(this.divPopup).css("position", "absolute");
};

popup.prototype.closePopup = function () {
    if (typeof this.divPopup == "undefined") return;

    if (typeof this.content != "string") {
        jQuery(this.backupParent).append(jQuery(this.content).hide().remove());
    }
    jQuery(this.divPopup).remove();
    if (typeof this.onclose != "undefined") {
        this.onclose();
    }
    var ug_head_center = jQuery(this.divPopup).find("div[ug_head_center]");
    jQuery(document).unbind("mousemove", ug_head_center.get(0)._onmousemove);
    this.removeOverlayer();
};

popup.prototype.isExisted = function () {
    var popup = jQuery(document).find("body").find("div[ugpopup]");
    if (popup.length <= 0) return false;
    return true;
};

popup.prototype.reHeight = function (height) {
    this.divPopup.find("div[ug_mid_left]").height(height);
    this.divPopup.find("div[ug_mid_center]").height(height);
    this.divPopup.find("div[ug_mid_right]").height(height);
    this.reCenter();
};

popup.prototype.removeOverlayer = function () {
    jQuery(window.document).find("body").find("div[ug_overlayer]").remove();
};

popup.prototype.overlayer = function (p) {
    if (!this.dialog) return;

    var _top = window;

    var transparent = ""; //';filter:alpha(opacity=20); -moz-opacity: 0.2;opacity: 0.2;"';

    jQuery(_top.document).find("body").append("<div ug_overlayer='true' style='top:0px; left:0px;position:absolute;z-index:8000;" + transparent + "'></div>");
    var ug_overlayer = jQuery(_top.document).find("body").find("div[ug_overlayer]");
    //Get the screen height and width
    jQuery(ug_overlayer).attr("class", "transparent_class");

    var maskHeight = jQuery(_top.document).height();
    var maskWidth = jQuery(_top).width();
    //Set height and width to mask to fill up the whole screen
    jQuery(ug_overlayer).width(maskWidth).height(maskHeight);
};

popup.prototype.head = function (p) {
    this.headPartial1(p);

    this.createCloseButton(p);
};

popup.prototype.headPartial1 = function (p) {
    var height_head = 40;
    var width_head_left = 22;
    var width_head_right = 23;
    var width_head_center = this.width - (width_head_left + width_head_right);
    jQuery(p).append
    ("\
        <div ug_head_left='true' style='float:left; background-image:url(\"" + this.imgPath + "L_top.png" + "\"); width:" + width_head_left + "px; height:" + height_head + "px;'>\
        </div>\
        <div ug_head_center='true' style='cursor:move;position:relative;float:left; background-image:url(\"" + this.imgPath + "C_top.png" + "\"); width:" + width_head_center + "px; height:" + height_head + "px'>\
        </div>\
        <div ug_head_right='true' style='float:left; background-image:url(\"" + this.imgPath + "R_top.png" + "\"); width:" + width_head_right + "px; height:" + height_head + "px;'>\
        </div>\
    ");
    var ug_head_center = jQuery(p).find("div[ug_head_center]");
    //title
    jQuery(ug_head_center).html("<span style='position:absolute; left:0px; top: 20px'>" + this.title + "</span>").css("color", this.titleColor).css("font-weight", "bold");
    jQuery(ug_head_center).disableTextSelect();

    var _moveTarget = jQuery(ug_head_center).get(0);
    _moveTarget._onmousedown = jQuery.proxy(function (e) {
        jQuery(document).find("div[ugpopup]").css("z-index", "");
        jQuery(this.divPopup).css("z-index", 90000);
        _moveTarget.mouseLeft = e.pageX;
        _moveTarget.mouseTop = e.pageY;
        _moveTarget.isMove = true;
    }, this);

    _moveTarget._onmousemove = jQuery.proxy(function (e) {
        if (!_moveTarget.isMove) return;
        if (e.pageX >= _moveTarget.mouseLeft) {
            if (e.pageY >= _moveTarget.mouseTop) {
                jQuery(p).offset(
                {
                    left: jQuery(p).offset().left + (e.pageX - _moveTarget.mouseLeft),
                    top: jQuery(p).offset().top + (e.pageY - _moveTarget.mouseTop)
                });
            }
            else {
                jQuery(p).offset(
                {
                    left: jQuery(p).offset().left + (e.pageX - _moveTarget.mouseLeft),
                    top: jQuery(p).offset().top - (_moveTarget.mouseTop - e.pageY)
                });
            }
        }
        else {
            if (e.pageY >= _moveTarget.mouseTop) {
                jQuery(p).offset(
                {
                    left: jQuery(p).offset().left - (_moveTarget.mouseLeft - e.pageX),
                    top: jQuery(p).offset().top + (e.pageY - _moveTarget.mouseTop)
                });
            }
            else {
                jQuery(p).offset(
                {
                    left: jQuery(p).offset().left - (_moveTarget.mouseLeft - e.pageX),
                    top: jQuery(p).offset().top - (_moveTarget.mouseTop - e.pageY)
                });
            }
        }
        _moveTarget.mouseLeft = e.pageX;
        _moveTarget.mouseTop = e.pageY;
    }, this);

    _moveTarget._onmouseup = jQuery.proxy(function (e) {
        _moveTarget.isMove = false;
    }, this);

    jQuery(ug_head_center).mousedown(_moveTarget._onmousedown)
    .mouseup(_moveTarget._onmouseup)
    .mousemove(_moveTarget._onmousemove);

    jQuery(document).mousemove(_moveTarget._onmousemove).mouseup(_moveTarget._onmouseup);
};

popup.prototype.createCloseButton = function (p) {
    var ug_head_center = jQuery(p).find("div[ug_head_center]");
    //close image
    jQuery(ug_head_center).append("<div ug_close_img='true' style='background-image:url(" + this.imgPath + "icon_close1.png" + "); background-position:0px 0px;cursor:pointer;position:absolute; right:0px; top:" + 18 + "px; width:19px; height:18px'></div>");
    var ug_close_img = jQuery(ug_head_center).find("div[ug_close_img]");
    jQuery(ug_close_img).mouseover(jQuery.proxy(function () {
        jQuery(this.divPopup).find("div[ug_close_img]").css("background-position", "0px 18px");
    }, this))
    .mouseout(jQuery.proxy(function () {
        jQuery(this.divPopup).find("div[ug_close_img]").css("background-position", "0px 0px");
    }, this))
    .click(jQuery.proxy(function () {
        this.closePopup();
    }, this));
};

popup.prototype.body = function (p) {
    var height_head = 40;
    var height_bottom = 47;
    var height_mid = this.height - (height_head + height_bottom);
    var width_mid_left = 22;
    var width_mid_right = 23;
    var width_mid_center = this.width - (width_mid_left + width_mid_right);
    jQuery(p).append
    ("\
        <div ug_mid_left='true' style='float:left; background-image:url(\"" + this.imgPath + "mid_L.png" + "\"); width:" + width_mid_left + "px; height:" + height_mid + "px;'>\
        </div>\
        <div ug_mid_center='true' style='float:left; background-color:" + this.backgroundColor + "; width:" + width_mid_center + "px; height:" + height_mid + "px'>\
        </div>\
        <div ug_mid_right='true' style='float:left; background-image:url(\"" + this.imgPath + "mid_R.png" + "\"); width:" + width_mid_right + "px; height:" + height_mid + "px;'>\
        </div>\
    ");
    var ug_mid_center = jQuery(p).find("div[ug_mid_center]");
    if (typeof this.content == "string") {//iframe
        var ug_frame = jQuery(ug_mid_center).append("<iframe frameborder='0' noresize='noresize' scrolling='auto' ALLOWTRANSPARENCY='true'></iframe>").find("iframe");
        jQuery(ug_frame).height(height_mid).width(width_mid_center);
        jQuery(ug_frame).attr("src", this.content);
    }
    else {//show div(object)
        this.backupParent = jQuery(this.content).parent();
        jQuery(ug_mid_center).append(jQuery(this.content).remove());
    }
};

popup.prototype.bottom = function (p) {
    var height_bottom = 47;
    var width_bottom_left = 22;
    var width_bottom_right = 22;
    var width_bottom_center = this.width - (width_bottom_left + width_bottom_right);
    jQuery(p).append
    ("\
        <div ug_bottom_left='true' style='float:left; background-image:url(\"" + this.imgPath + "L_bottom.png" + "\"); width:" + width_bottom_left + "px; height:" + height_bottom + "px;'>\
        </div>\
        <div ug_bottom_center='true' style='float:left; background-image:url(\"" + this.imgPath + "C_bottom.png" + "\"); width:" + width_bottom_center + "px; height:" + height_bottom + "px'>\
        </div>\
        <div ug_bottom_right='true' style='float:left; background-image:url(\"" + this.imgPath + "R_bottom.png" + "\"); width:" + width_bottom_right + "px; height:" + height_bottom + "px;'>\
        </div>\
    ");
};

popup.prototype.addButtons = function (p) {
    if (this.buttons.length <= 0) return;
    var ug_bottom_center = jQuery(this.divPopup).find("div[ug_bottom_center]");
    for (var i = 0; i < this.buttons.length; ++i) {
        jQuery(ug_bottom_center).append("<input type='button' class='btn' value='" + this.buttons[i].text + "' style='float:right; margin-right:2px; margin-top:5px' />");
        var btns = jQuery(ug_bottom_center).find("input[type='button']");
        jQuery(jQuery(btns).get(jQuery(btns).length - 1)).get(0).callback = this.buttons[i].callback;
        jQuery(jQuery(btns).get(jQuery(btns).length - 1)).click(jQuery.proxy(function (e) {
            e.target.callback(jQuery(this.divPopup).attr("id"), e.target);
        }, this));
    }
};

popup.prototype.getAppPath = function () {
    var baseURL = location.href;
    var rootURL = baseURL.substring(0, baseURL.indexOf('/', 7));

    return rootURL + baseURL.substring(baseURL.indexOf('/', 8), baseURL.indexOf('/', baseURL.indexOf('/', 8) + 1)) + "/";

    // if the root url is localhost, don't add the directory as cassani doesn't use it
    if (baseURL.indexOf('localhost') == -1) {
        return rootURL + "/";
    } else {
        return rootURL + baseURL.substring(baseURL.indexOf('/', 8), baseURL.indexOf('/', baseURL.indexOf('/', 8) + 1)) + "/";
    }
}

//------------------------------------------------------------------------------------------------------------------------------------

popup.prototype.initPopup = function (options, div) {
    this.closePopup();

    if (typeof options == "undefined") options = {};
    if (typeof options.title == "undefined" || options.title == null) options.title = "";
    if (typeof options.width == "undefined" || options.width == null) options.width = 600;
    if (typeof options.height == "undefined" || options.height == null) options.height = 800;
    if (typeof options.buttons == "undefined" || options.buttons == null) options.buttons = [];
    if (typeof options.multiple == "undefined" || options.multiple == null) options.multiple = false;
    if (typeof options.dialog == "undefined" || options.dialog == null) options.dialog = false;
    if (typeof options.onload == "undefined" || options.onload == null) options.onload = function () { };
    if (typeof options.onclose == "undefined" || options.onclose == null) options.onclose = function () { };

    this.content = div;
    this.title = options.title;
    this.width = options.width;
    this.height = options.height;
    this.buttons = options.buttons;
    this.multiple = options.multiple;
    this.dialog = options.dialog;
    this.onload = options.onload;
    this.onclose = options.onclose;

    this.imgPath = this.getAppPath() + "Scripts/Lib/Popup/Images/";

    popup.id += 1;

    if (!this.multiple) {
        jQuery(document).find("body").find("div[ugpopup]").remove();
    }

    jQuery(document).find("body").append("<div id='ugpopup" + popup.id + "' ugpopup='true' style='z-index:9000'></div>");

    this.divPopup = jQuery("#ugpopup" + popup.id);

    jQuery(this.divPopup).get(0).popup = this;

    jQuery(this.divPopup).width(this.width).height(this.height);

    this.overlayer(jQuery(this.divPopup));
    this.head2(jQuery(this.divPopup));
    this.body(jQuery(this.divPopup));
    this.bottom(jQuery(this.divPopup));
    this.addButtons(jQuery(this.divPopup));

    jQuery(this.divPopup).hide();

    return jQuery(this.divPopup).attr("id");
};


popup.prototype.head2 = function (p) {
    this.headPartial1(p);

    this.createCloseButton2(p);
};

popup.prototype.createCloseButton2 = function (p) {
    var ug_head_center = jQuery(p).find("div[ug_head_center]");
    //close image
    jQuery(ug_head_center).append("<div ug_close_img='true' style='background-image:url(" + this.imgPath + "icon_close1.png" + "); background-position:0px 0px;cursor:pointer;position:absolute; right:0px; top:" + 18 + "px; width:19px; height:18px'></div>");
    var ug_close_img = jQuery(ug_head_center).find("div[ug_close_img]");
    jQuery(ug_close_img).mouseover(jQuery.proxy(function () {
        jQuery(this.divPopup).find("div[ug_close_img]").css("background-position", "0px 18px");
    }, this))
    .mouseout(jQuery.proxy(function () {
        jQuery(this.divPopup).find("div[ug_close_img]").css("background-position", "0px 0px");
    }, this))
    .click(jQuery.proxy(function () {
        jQuery(this.divPopup).hide();
        this.onclose();
    }, this));
};

jQuery.fn.initPopup = function (options) {
    preloadImage();
    return this.each(function () {
        if (typeof jQuery(this).get(0).p == "undefined" || jQuery(this).get(0).p == null) {
            var p = new popup();
            jQuery(this).get(0).p = p;
            jQuery(this).get(0).popupOptions = options;
        }
        jQuery(this).get(0).p.initPopup(options, jQuery(this));
    });
};

jQuery.fn.showPopup = function () {
    return this.each(function () {
        if (jQuery(this).get(0).p == "undefined" || jQuery(this).get(0).p == null) return;

        jQuery(jQuery(this).get(0).p.divPopup).show();
        jQuery(jQuery(this).get(0).p.divPopup).CenterIt();
        jQuery(jQuery(this).get(0).p.divPopup).css("position", "absolute");
        jQuery(jQuery(this).get(0).p.content).show();
        jQuery(this).get(0).p.onload();
    });
};

jQuery.fn.closePopup = function () {
    return this.each(function () {
        if (jQuery(this).get(0).p == "undefined" || jQuery(this).get(0).p == null) return;
        (jQuery(this).get(0).p.divPopup).hide();
        jQuery(this).get(0).p.onclose();
    });
};

jQuery.fn.disableTextSelect = function () {
    return this.each(function () {
        jQuery(this).css({
            'MozUserSelect': 'none'
        }).bind('selectstart', function () {
            return false;
        }).mousedown(function () {
            return false;
        });
    });
};

///dependency :Scripts/Lib/jquery.imgpreload.js
//preload images
var p = "js/Popup/Images/";
var the_images = [
    (new popup()).getAppPath() + p + "C_bottom.png",
    (new popup()).getAppPath() + p + "C_top.png",
    (new popup()).getAppPath() + p + "icon_close1.png",
    (new popup()).getAppPath() + p + "L_bottom.png",
    (new popup()).getAppPath() + p + "L_top.png",
    (new popup()).getAppPath() + p + "mid_L.png",
    (new popup()).getAppPath() + p + "mid_R.png",
    (new popup()).getAppPath() + p + "R_bottom.png",
    (new popup()).getAppPath() + p + "R_top.png"
];
function preloadImage() {
    jQuery.imgpreload(the_images);
}

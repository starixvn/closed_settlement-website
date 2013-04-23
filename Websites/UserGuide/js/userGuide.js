var userGuide = {
    imgPath: "",
    path: "",
    width:0,
    height:0,
    fName:"",
    title:"",
    titleColor: "black",
    backgroundColor: "#fbe3d2"
};
userGuide.show = function (fName, title, width, height) {
    this.close();

    if (typeof width == "undefined") width = 600;
    if (typeof height == "undefined") height = 800;

    this.width = width;
    this.height = height;
    this.fName = fName;
    this.title = title;

    this.path = this.getAppPath() + "UserGuide/";
    this.imgPath = this.path + "image/";

    var topBody = this.getTopBody();

    $(topBody).find("body").append("<div ugpopup='true' class='ug_popup'></div>");
    var divPopup = $(topBody).find("div[ugpopup]").width(width).height(height);

    this.head(divPopup);
    this.body(divPopup);
    this.bottom(divPopup);

    divPopup.show();
    divPopup.CenterIt();
    divPopup.css("position", "absolute");
};

userGuide.getTopBody = function () {
    return $(window.top.document).find("#content").get(0).contentDocument;
};

userGuide.close = function(){
    $(this.getTopBody()).find("div[ugpopup]").remove();
};

userGuide.isExisted = function() {
    var popup = $(document).find("body").find("div[ugpopup]");
    if (popup.length <= 0) return false;
    return true;
};

userGuide.head = function(popup) {
    var height_head = 40;
    var width_head_left = 22;
    var width_head_right = 23;
    var width_head_center = this.width - (width_head_left + width_head_right);
    $(popup).append
    ("\
        <div ug_head_left='true' style='float:left; background-image:url(\"" + this.imgPath + "L_top.png" + "\"); width:" + width_head_left + "px; height:" + height_head + "px;'>\
        </div>\
        <div ug_head_center='true' style='position:relative;float:left; background-image:url(\"" + this.imgPath + "C_top.png" + "\"); width:" + width_head_center + "px; height:" + height_head + "px'>\
        </div>\
        <div ug_head_right='true' style='float:left; background-image:url(\"" + this.imgPath + "R_top.png" + "\"); width:" + width_head_right + "px; height:" + height_head + "px;'>\
        </div>\
    ");
    var ug_head_center = $(popup).find("div[ug_head_center]");
    //title
    $(ug_head_center).html("<span style='position:absolute; left:0px; top: 15px'>" + this.title + "</span>").css("color", this.titleColor).css("font-weight", "bold");
    $(ug_head_center).disableTextSelect();
    $(ug_head_center).mousedown(function(e){
        $(this).css("cursor", "move");
        this.mouseLeft = e.pageX;
        this.mouseTop = e.pageY;
        this.isMove = true;
    })
    .mouseup(function(){
        $(this).css("cursor", "default");
        this.isMove = false;
    })
    .mouseout(function(){
        $(this).css("cursor", "default");
    })
    .mousemove(function(e){
        if (!this.isMove) return;
        if (e.pageX >= this.mouseLeft) {
            if (e.pageY >= this.mouseTop) {
                $(popup).offset(
                {
                    left: $(popup).offset().left + (e.pageX - this.mouseLeft),
                    top: $(popup).offset().top + (e.pageY - this.mouseTop)
                });
            }
            else {
                $(popup).offset(
                {
                    left: $(popup).offset().left + (e.pageX - this.mouseLeft),
                    top: $(popup).offset().top - (this.mouseTop - e.pageY)
                });
            }
        }
        else {
            if (e.pageY >= this.mouseTop) {
                $(popup).offset(
                {
                    left: $(popup).offset().left - (this.mouseLeft - e.pageX),
                    top: $(popup).offset().top + (e.pageY - this.mouseTop)
                });
            }
            else {
                $(popup).offset(
                {
                    left: $(popup).offset().left - (this.mouseLeft - e.pageX),
                    top: $(popup).offset().top - (this.mouseTop - e.pageY)
                });
            }
        }
        this.mouseLeft = e.pageX;
        this.mouseTop = e.pageY;
    });
    
    //close image
    $(ug_head_center).append("<img ug_close_img='true' src='" + this.imgPath + "icon_close.png' style='cursor:pointer;position:absolute; right:0px; top:" + 18 + "px' />");
    var ug_close_img = $(ug_head_center).find("img[ug_close_img]");
    $(ug_close_img).mouseover(function(){
        $(this).attr("src", userGuide.imgPath + "icon_close_hover.png");
    })
    .mouseout(function(){
        $(this).attr("src", userGuide.imgPath + "icon_close.png");
    })
    .click(function(){
        userGuide.close();
    });
};

userGuide.body = function(popup) {
    var height_head = 40;
    var height_bottom = 42;
    var height_mid = this.height - (height_head + height_bottom);
    var width_mid_left = 22;
    var width_mid_right = 23;
    var width_mid_center = this.width - (width_mid_left + width_mid_right);
    $(popup).append
    ("\
        <div ug_mid_left='true' style='float:left; background-image:url(\"" + this.imgPath + "mid_L.png" + "\"); width:" + width_mid_left + "px; height:" + height_mid + "px;'>\
        </div>\
        <div ug_mid_center='true' style='float:left; background-color:" + this.backgroundColor + "; width:" + width_mid_center + "px; height:" + height_mid + "px'>\
            <iframe frameborder='0' noresize='noresize' scrolling='auto' ALLOWTRANSPARENCY='true'></iframe>\
        </div>\
        <div ug_mid_right='true' style='float:left; background-image:url(\"" + this.imgPath + "mid_R.png" + "\"); width:" + width_mid_right + "px; height:" + height_mid + "px;'>\
        </div>\
    ");
    var ug_frame = $(popup).find("iframe");
    $(ug_frame).height(height_mid).width(width_mid_center);
    $(ug_frame).attr("src", this.path + "View.aspx?functionName=" + this.fName);
};

userGuide.bottom = function(popup) {
    var height_bottom = 42;
    var width_bottom_left = 22;
    var width_bottom_right = 22;
    var width_bottom_center = this.width - (width_bottom_left + width_bottom_right);
    $(popup).append
    ("\
        <div ug_bottom_left='true' style='float:left; background-image:url(\"" + this.imgPath + "L_bottom.png" + "\"); width:" + width_bottom_left + "px; height:" + height_bottom + "px;'>\
        </div>\
        <div ug_bottom_center='true' style='float:left; background-image:url(\"" + this.imgPath + "C_bottom.png" + "\"); width:" + width_bottom_center + "px; height:" + height_bottom + "px'>\
        </div>\
        <div ug_bottom_right='true' style='float:left; background-image:url(\"" + this.imgPath + "R_bottom.png" + "\"); width:" + width_bottom_right + "px; height:" + height_bottom + "px;'>\
        </div>\
    ");
};

userGuide.getAppPath = function() {
    var baseURL = location.href;
    var rootURL = baseURL.substring(0, baseURL.indexOf('/', 7));
    
    return rootURL + baseURL.substring(baseURL.indexOf('/', 8), baseURL.indexOf('/', baseURL.indexOf('/', 8)+1)) + "/";        
    
    // if the root url is localhost, don't add the directory as cassani doesn't use it
    if (baseURL.indexOf('localhost') == -1) {
        return rootURL + "/";
    } else {
        return rootURL + baseURL.substring(baseURL.indexOf('/', 8), baseURL.indexOf('/', baseURL.indexOf('/', 8)+1)) + "/";        
    }
}

jQuery.fn.disableTextSelect = function() {
  return this.each(function() {
    $(this).css({
      'MozUserSelect' : 'none'
    }).bind('selectstart', function() {
      return false;
    }).mousedown(function() {
      return false;
    });
  });
};
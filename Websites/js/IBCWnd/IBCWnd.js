// IBCWnd v1.0
// Developed by Qui
// Reference: http://students.infoiasi.ro/~mishoo/site/calendar.epl

// Dragable window for IBCAgent2 


// class prototype
IBCWnd = function()
{
    this.is_ie = window.attachEvent?true:false;
    this.dragging = false;
    this.createdElement = false;
    
    var scripts = document.getElementsByTagName("script");
    this.baseURL = "../Js/IBCWnd/";
    
    this.js = "IBCWnd.js";
    this.css = "IBCWnd.css";
    this.blank = "blank.html";
    
    if(scripts.length > 0)
    {
        for(var i=0; i<scripts.length; i++)
        {
            var idx = scripts[i].src.indexOf(this.js);
            if(idx != -1)
            {
                this.baseURL = scripts[i].src.substr(0, idx);
                break;
            }
        }
    }
    
    this.cssURL = this.baseURL + this.css;
    this.jsURL = this.baseURL + this.js;
    this.blankURL = this.baseURL + this.blank;

    if(!this.is_ie)
    {
        var cssEl = "<link href=\"" + this.cssURL + "\" type=\"text/css\" rel=\"stylesheet\">&nbsp;";
        var divCss = IBCWnd_createElement("div", document.body);
        divCss.innerHTML = cssEl;
    }
    else
    {
        document.createStyleSheet(this.cssURL);
    }
}

IBCWnd.prototype.open = function(url, titleText, left, top, width, height, windowCss, titleCss, closeBtnCss) {
    if (typeof left == 'undefined' ||
        typeof top == 'undefined' ||
        typeof width == 'undefined' ||
        typeof height == 'undefined' ||
        typeof url == 'undefined') {
        alert('ibcWnd.open(url, titleText, left, top, width, height, windowCss, titleCss, closeBtnCss)\r\n\r\n\   - Invalid parameters  ');
        return false;
    }

    //top = yMousePos;

    IBCWnd_preWidth = 0;
    IBCWnd_preHeight = 0;

    if (IBCWnd_autoKeepPosition || this.autoKeepPosition) {
        if (typeof IBCWnd_preLeft != 'undefined') left = IBCWnd_preLeft;
        if (typeof IBCWnd_preTop != 'undefined') top = IBCWnd_preTop;
    }

    if (!this.createdElement) {
        this.mask = IBCWnd_createElement("div");
        this.mask.className = "IBCWndMask";
        this.mask.style.position = "absolute";
        this.mask.style.display = "none";
        this.mask.innerHTML = "<input class=\"IBCWndTitleMask\" readonly=\"true\" type=\"text\" value=\"\" />";

        this.element = IBCWnd_createElement("div");
        this.element.className = (windowCss ? (windowCss) : "IBCWnd");
        this.element.style.position = "absolute";
        this.element.style.display = "none";
        this.element.style.zIndex = 1000;

        var table = IBCWnd_createElement("table", this.element);
        table.cellPadding = 0; table.cellSpacing = 0; table.className = "IBCWndTable"; table.border = 0;
        var thead = IBCWnd_createElement("thead", table);
        var tr = IBCWnd_createElement("tr", thead);
        tr.oncontextmenu = function() { return false; };
        tr.className = (titleCss ? (titleCss) : "IBCWndTitle");
        tr.style.height = "20px"; tr.style.overflow = "hidden"; tr.style.zIndex = 1000;

        var td1 = IBCWnd_createElement("td", tr);
        td1.className = "IBCWndTitleText"; td1.style.cursor = "move";
        IBCWnd_addEvent(td1, "mousedown", IBCWnd_dragStart);
        td1.className = (titleCss ? (titleCss) : "IBCWndTitle");
        this.maskTitle = td1;
        var div1 = IBCWnd_createElement("div", td1); div1.style.height = "20px"; div1.style.display = "block"; div1.style.zIndex = 1000;
        div1.style.paddingLeft = "10px"; div1.style.lineHeight = "18px"; div1.style.color = "#ffffff"; div1.style.textAlign = "center";
        this.title = div1;

        var td2 = IBCWnd_createElement("td", tr); td2.className = "IBCWndTitleButton";
        td2.onclick = function() { ibcWnd.close(); }; td2.oncontextmenu = function() { return false; };
        td2.innerHTML = "<div oncontextmenu=\"return false\" title=\"Close\" class=\"" + (closeBtnCss ? closeBtnCss : "IBCWndCloseButton") + "\" style=\"float:right\"></div>";
        td2.vAlign = "center"; td2.align = "right";
        td2.className = (titleCss ? (titleCss) : "IBCWndTitle");
        var tbody = IBCWnd_createElement("tbody", table);

        tr = IBCWnd_createElement("tr", tbody);
        td = IBCWnd_createElement("td", tr); td.colSpan = 2;
        td.align = "left"; td.vAlign = "top"; td.className = "IBCWndNoPadding";
        this.content = td;

        this.iframe = IBCWnd_createIframe();
        this.content.appendChild(this.iframe);

        document.body.appendChild(this.mask);
        document.body.appendChild(this.element);

        this.createdElement = true;
    }

    
    this.setRect(left, top, width, height);

    this.url = url;
    this.iframe.src = this.blankURL + '?v=109';
    this.loaded = false;

    this.title.innerHTML = titleText;
    this.element.style.display = 'block';

    // listen event

    IBCWnd_addEvent(this.iframe, "load", function() {
        IBCWnd_addEvent(IBCWnd._W.is_ie ? IBCWnd._W.iframe.document : IBCWnd._W.iframe.contentDocument, "keydown", IBCWnd_keydown);
    });

    IBCWnd_addEvent(window, "resize", IBCWnd_calculateViewport);

    // add public reference for active window
    IBCWnd._W = this;
}

// Event on closing
IBCWnd.prototype.onClosing = function(){return true;};

IBCWnd.prototype.close = function (force) {
    if (!force) {
        var bContinue = this.onClosing();
        if (!bContinue) return false;
    }

    // un-listen event
    if (typeof (this.iframe) == 'undefined')
        return;
    IBCWnd_removeEvent(this.is_ie ? this.iframe.document : this.iframe.contentDocument, "keydown", IBCWnd_keydown);
    IBCWnd_removeEvent(window, "resize", IBCWnd_calculateViewport);

    this.url = null;
    this.loaded = false;
    this.element.style.display = 'none';
    this.viewport = null;

    if (this.iframe.src != this.blankURL + '?v=109') this.iframe.src = this.blankURL + '?v=109';

    // Reset event
    this.onClosing = function () { return true; };

}

IBCWnd.prototype.setRect = function(left, top, width, height) {
    if (!this.createdElement) return false;

    var scrOfX = 0;
    var scrOfY = 0;
    if (this.is_ie && typeof (window.pageXOffset) == 'number') {
        scrOfX = window.pageXOffset;
    } else if (document.body && (document.body.scrollLeft || document.body.scrollTop)) {
        scrOfX = document.body.scrollLeft;
    } else if (document.documentElement && (document.documentElement.scrollLeft || document.documentElement.scrollTop)) {
        scrOfX = document.documentElement.scrollLeft;
    }
    if (this.is_ie && typeof (window.pageYOffset) == 'number') {
        scrOfY = window.pageYOffset;
    } else if (document.body && (document.body.scrollLeft || document.body.scrollTop)) {
        scrOfY = document.body.scrollTop;
    } else if (document.documentElement && (document.documentElement.scrollLeft || document.documentElement.scrollTop)) {
        scrOfY = document.documentElement.scrollTop;
    }

    //nqminh
    scrOfY += 15;

    this.scrOfX = scrOfX;
    this.scrOfY = scrOfY;

    if (!this.loaded) {
        left += scrOfX;
        //top += scrOfY;
    }

    this.left = left;
    this.top = top;

    this.width = (width < 30) ? 30 : width;
    this.height = (height < 30) ? 30 : height;

    this.calculateViewport();


    this.mask.style.left = this.left + "px";
    this.mask.style.top = this.top + "px";

    this.mask.style.width = this.mask.firstChild.style.width = this.width + "px";
    this.mask.style.height = this.mask.firstChild.style.height = this.height + "px";

    this.title.style.width = this.maskTitle.style.width = (this.width - 20) + 'px';
    this.content.style.width = (this.width) + "px";
    this.content.style.height = (this.height - 20) + "px";

    this.element.style.left = this.left + "px";
    this.element.style.top = this.top + "px";
    this.element.style.width = this.width + "px";
    this.element.style.height = this.height + "px";

    if (width <= 5 || height <= 5)
        this.element.style.visibility = "hidden";
    else
        this.element.style.visibility = "visible";

    return true;
}

IBCWnd.prototype.calculateViewport = function()
{
    this.viewport = [];
    if(!this.is_ie)
    {
        // in standards compliant mode (i.e. with a valid doctype as the first line in the document)
        if (typeof document.documentElement != 'undefined'
         && typeof document.documentElement.scrollWidth !=
         'undefined' && document.documentElement.scrollWidth != 0)
        {
           this.viewport.width = document.documentElement.scrollWidth;
           this.viewport.height = document.documentElement.scrollHeight;
        }        

        this.viewport.width = Math.max(this.viewport.width, (Math.max(document.body.scrollWidth, document.body.clientWidth)));
        this.viewport.height = Math.max(this.viewport.height, (Math.max(document.body.scrollHeight, document.body.clientHeight)));
    }
    else
    {
        // in standards compliant mode (i.e. with a valid doctype as the first line in the document)
        if (typeof document.documentElement != 'undefined'
         && typeof document.documentElement.clientWidth !=
         'undefined' && document.documentElement.clientWidth != 0)
        {
           this.viewport.width = document.documentElement.clientWidth;
           this.viewport.height = document.documentElement.clientHeight;
        }
        // older versions of IE
        else
        {
           this.viewport.width = document.getElementsByTagName('body')[0].clientWidth;
           this.viewport.height = document.getElementsByTagName('body')[0].clientHeight;
        }

        this.viewport.width = Math.max(document.body.scrollWidth, this.viewport.width);
        this.viewport.height = Math.max(document.body.scrollHeight, this.viewport.height);
    }
    
    this.maxLeft = this.viewport.width - this.width - 6;
    this.maxTop = this.viewport.height - this.height - 6;
}

// utility functions
function IBCWnd_addEvent(el, evname, func) {
	if (el.attachEvent) { // IE
		el.attachEvent("on" + evname, func);
	} else if (el.addEventListener) { // Gecko / W3C
		el.addEventListener(evname, func, true);
	} else { // Opera (or old browsers)
		el["on" + evname] = func;
	}
}

function IBCWnd_removeEvent(el, evname, func) {
	if (el.detachEvent) { // IE
		el.detachEvent("on" + evname, func);
	} else if (el.removeEventListener) { // Gecko / W3C
		el.removeEventListener(evname, func, true);
	} else { // Opera (or old browsers)
		el["on" + evname] = null;
	}
}

function IBCWnd_createElement(type, parent) {
	var el = null;
	if (document.createElementNS) {
		// use the XHTML namespace; IE won't normally get here unless
		// _they_ "fix" the DOM2 implementation.
		el = document.createElementNS("http://www.w3.org/1999/xhtml", type);
	} else {
		el = document.createElement(type);
	}
	if (typeof parent != "undefined") {
		parent.appendChild(el);
	}
	return el;
}

// static property
IBCWnd_autoKeepPosition = false;

IBCWnd_preLeft = 0;
IBCWnd_preTop = 0;
IBCWnd_preWidth = 0;
IBCWnd_preHeight = 0;

// static functions, were called in event
IBCWnd_calculateViewport = function()
{
    var wnd = IBCWnd._W;
    wnd.calculateViewport();
}

IBCWnd_createIframe = function()
{
    var iframe = document.createElement("iframe"); iframe.setAttribute("border", 0); iframe.className = "IBCWndNoPadding";
    iframe.setAttribute("frameborder", 0); iframe.style.backgroundColor = "#ffffff";
    iframe.width = "100%"; iframe.height = "100%";
    return iframe;
}

function IBCWnd_keydown(ev)
{
    var wnd = IBCWnd._W;
    if(!wnd.loaded) return true;
    
	if (wnd.dragging) {
	    wnd.mask.style.display = "none";
	    wnd.dragging = false;
		return false;
	}
    
    if(wnd.is_ie) {
        if(window.event.keyCode == 27) wnd.close();
    }
    else {
        if(ev.which == 27) wnd.close(); 
    }
    
    return true;
}

function IBCWnd_dragStart(ev) 
{    
    var wnd = IBCWnd._W;
    var leftMouse = 1;
    if(wnd.is_ie) {
        leftMouse = window.event.button;
    }
    else {
        leftMouse = ev.which;
    }
    
    if(leftMouse != 1) return false;
    
    wnd.mask.style.display = "block";
    wnd.mask.style.left = wnd.element.style.left;
    wnd.mask.style.top = wnd.element.style.top;
    
	if (wnd.dragging) {
		return false;
	}
	wnd.dragging = true;
	
	var posX;
	var posY;
	if (wnd.is_ie) {
		posY = window.event.clientY + document.body.scrollTop;
		posX = window.event.clientX + document.body.scrollLeft;
	} else {
		posY = ev.clientY + window.scrollY;
		posX = ev.clientX + window.scrollX;
	}
	
	var st = wnd.element.style;
	wnd.xOffs = posX - parseInt(st.left);
	wnd.yOffs = posY - parseInt(st.top);

	IBCWnd_addEvent(document, "mousemove", IBCWnd_calDragIt);
	IBCWnd_addEvent(document, "mouseover", IBCWnd_stopEvent);
	IBCWnd_addEvent(document, "mouseup", IBCWnd_calDragEnd);
	
	IBCWnd_stopEvent(ev);
};

function IBCWnd_calDragIt(ev) {
    try
    {
        var wnd = IBCWnd._W;
	    if (!wnd.dragging) {
		    return false;
	    }
	    var posX;
	    var posY;
	    if (wnd.is_ie) {
		    posY = window.event.clientY + document.body.scrollTop;
		    posX = window.event.clientX + document.body.scrollLeft;
	    } else {
		    posX = ev.pageX;
		    posY = ev.pageY;
	    }

        var left = (posX - wnd.xOffs);
        var top = (posY - wnd.yOffs);
        
        // prevent horizontal move verflow width
        if(wnd.width < wnd.viewport.width - 6)
        {
            if(left <= 1) left = 1; if(left >= wnd.maxLeft) left = wnd.maxLeft;
            wnd.mask.style.left = left + "px";
        }
        
        // prevent vertical move overflow height
        if(wnd.height < wnd.viewport.height)
        {
            if(top <= 1) top = 1; if(top >= wnd.maxTop) top = wnd.maxTop;
            wnd.mask.style.top = top + "px";
        }
    }
    catch(ex)
    {}

};

IBCWnd_calDragEnd = function(ev) 
{
    var wnd = IBCWnd._W;
    wnd.mask.style.display = "none";
    
	if (!wnd.dragging) {
		return false;
	}
	
	wnd.dragging = false;
	
	wnd.left = IBCWnd_preLeft = parseInt(wnd.mask.style.left);
	wnd.top = IBCWnd_preTop = parseInt(wnd.mask.style.top);
	
	wnd.element.style.left = wnd.mask.style.left;
	wnd.element.style.top = wnd.mask.style.top;
	
    IBCWnd_removeEvent(document, "mousemove", IBCWnd_calDragIt);
    IBCWnd_removeEvent(document, "mouseover", IBCWnd_stopEvent);
    IBCWnd_removeEvent(document, "mouseup", IBCWnd_calDragEnd);
};

IBCWnd_stopEvent = function(ev) 
{
    var wnd = IBCWnd._W;
	if (wnd.is_ie) {
		window.event.cancelBubble = true;
		window.event.returnValue = false;
	} else {
		ev.preventDefault();
		ev.stopPropagation();
	}
	return false;
};

// init object
var ibcWnd;
IBCWnd_addEvent(window, 'load', function() { ibcWnd = new IBCWnd(); });
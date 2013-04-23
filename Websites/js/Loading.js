//--------- Window Popup ------------
// JScript File
var IE = document.all?true:false;
// utility functions --------------------------
IBC = [];
IBC.addEvent = function(el, evname, func) {
	if (el.attachEvent) { // IE
		el.attachEvent("on" + evname, func);
	} else if (el.addEventListener) { // Gecko / W3C
		el.addEventListener(evname, func, true);
	} else { // Opera (or old browsers)
		el["on" + evname] = func;
	}
}

IBC.removeEvent = function(el, evname, func) {
	if (el.detachEvent) { // IE
		el.detachEvent("on" + evname, func);
	} else if (el.removeEventListener) { // Gecko / W3C
		el.removeEventListener(evname, func, true);
	} else { // Opera (or old browsers)
		el["on" + evname] = null;
	}
}

IBC.calculateViewport = function()
{
    window.viewport = [];
    if(!this.is_ie)
    {
        // in standards compliant mode (i.e. with a valid doctype as the first line in the document)
        if (typeof document.documentElement != 'undefined'
         && typeof document.documentElement.scrollWidth !=
         'undefined' && document.documentElement.scrollWidth != 0)
        {
           window.viewport.width = document.documentElement.scrollWidth;
           window.viewport.height = document.documentElement.scrollHeight;
        }        

        window.viewport.width = Math.max(window.viewport.width, (Math.max(document.body.scrollWidth, document.body.clientWidth)));
        window.viewport.height = Math.max(window.viewport.height, (Math.max(document.body.scrollHeight, document.body.clientHeight)));
    }
    else
    {
        // in standards compliant mode (i.e. with a valid doctype as the first line in the document)
        if (typeof document.documentElement != 'undefined'
         && typeof document.documentElement.clientWidth !=
         'undefined' && document.documentElement.clientWidth != 0)
        {
           window.viewport.width = document.documentElement.clientWidth;
           window.viewport.height = document.documentElement.clientHeight;
        }
        // older versions of IE
        else
        {
           window.viewport.width = document.getElementsByTagName('body')[0].clientWidth;
           window.viewport.height = document.getElementsByTagName('body')[0].clientHeight;
        }

        window.viewport.width = Math.max(document.body.scrollWidth, window.viewport.width);
        window.viewport.height = Math.max(document.body.scrollHeight, window.viewport.height);
    }
}

IBC.createElement = function(type, parent) {
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

//--------------------------------------------


function CreateMaskLoadingDiv(rootPath)
{
    rootPath = (rootPath == null) ? '../../' : rootPath;

    window.divMaskLoading = IBC.createElement("div");
    var div1 = IBC.createElement("div");
    div1.style.width = '100px'; 
    div1.style.height = '100px';
   
    div1.style.position = 'relative';
    div1.style.background = 'url(' + rootPath + 'App_Themes/SteelBlue/Images/loader3.gif)';
    window.divMaskLoading.style.display = 'none';
    window.divMaskLoading.style.position = 'absolute';
    window.divMaskLoading.style.top = '0px';window.divMaskLoading.style.left = '0px';
    window.divMaskLoading.style.filter = 'alpha(opacity=50)';
    window.divMaskLoading.style.opacity = '0.5';
    window.divMaskLoading.style.backgroundColor = 'white';
    window.divMaskLoading.appendChild(div1);
    document.body.appendChild(window.divMaskLoading);
    
}

function CreateMaskLoadingPopup()
{

    window.divMaskLoadingPopup = IBC.createElement("div");
    var div1 = IBC.createElement("div");
    div1.style.width = '100px'; 
    div1.style.height = '100px';
   
    div1.style.position = 'relative';
    div1.style.background = 'url(../App_Themes/SteelBlue/Images/loader3.gif)';
    window.divMaskLoadingPopup.style.display = 'none';
    window.divMaskLoadingPopup.style.position = 'absolute';
    window.divMaskLoadingPopup.style.top = '0px';window.divMaskLoadingPopup.style.left = '0px';
    window.divMaskLoadingPopup.style.filter = 'alpha(opacity=50)';
    window.divMaskLoadingPopup.style.opacity = '0.5';
    window.divMaskLoadingPopup.style.backgroundColor = 'white';
    window.divMaskLoadingPopup.appendChild(div1);
    document.body.appendChild(window.divMaskLoadingPopup);
    
}

// start up script ---------------------------
IBC.addEvent(window, "load", _startup);
function _startup()
{
   
    CreateMaskLoadingDiv();
    CreateMaskLoadingPopup();
    IBC.calculateViewport();
}

// JS function --------------------------
function LoadImagePending()
{   
    if(!window.divMaskLoadingPopup) return;
    if(typeof ibcWnd != 'undefined') if(ibcWnd.loaded) ibcWnd.close();    
    window.divMaskLoadingPopup.style.width = (window.viewport.width - 20) + 'px';
    window.divMaskLoadingPopup.style.height = window.viewport.height + 'px';    
    window.divMaskLoadingPopup.firstChild.style.left = Math.floor(window.viewport.width/2) + 'px';
    window.divMaskLoadingPopup.firstChild.style.top = document.body.scrollTop + 'px';
    window.divMaskLoadingPopup.style.display = 'block';
}
// JScript File

/**************************************************
 * dom-drag.js
 * 09.25.2001
 * www.youngpup.net
 **************************************************
 * 10.28.2001 - fixed minor bug where events
 * sometimes fired off the handle, not the root.
 **************************************************/
/* Tu Hoang: added a few changes to fit my use */
var Drag = {

	obj : null,

	init : function(o, oRoot, minX, maxX, minY, maxY, bSwapHorzRef, bSwapVertRef, fXMapper, fYMapper)
	{
		
		o.onmousedown	= Drag.start;

		o.hmode			= bSwapHorzRef ? false : true ;
		o.vmode			= bSwapVertRef ? false : true ;

		o.root = oRoot && oRoot != null ? oRoot : o ;
	
		/* Tu's change */
		o.root.style.position = "absolute";		
		/* ----------- */		
		//if (o.hmode  && isNaN(parseInt(o.root.style.left  ))) o.root.style.left   = "0px";
		//if (o.vmode  && isNaN(parseInt(o.root.style.top   ))) o.root.style.top    = "0px";
		//if (!o.hmode && isNaN(parseInt(o.root.style.right ))) o.root.style.right  = "0px";
		//if (!o.vmode && isNaN(parseInt(o.root.style.bottom))) o.root.style.bottom = "0px";

		o.minX	= typeof minX != 'undefined' ? minX : null;
		o.minY	= typeof minY != 'undefined' ? minY : null;
		o.maxX	= typeof maxX != 'undefined' ? maxX : null;
		o.maxY	= typeof maxY != 'undefined' ? maxY : null;

		o.xMapper = fXMapper ? fXMapper : null;
		o.yMapper = fYMapper ? fYMapper : null;

		o.root.onDragStart	= new Function();
		o.root.onDragEnd	= new Function();
		o.root.onDrag		= new Function();
	},

	start : function(e)
	{		
		var o = Drag.obj = this;
		e = Drag.fixE(e);
		var y = parseInt(o.vmode ? o.root.style.top  : o.root.style.bottom);
		var x = parseInt(o.hmode ? o.root.style.left : o.root.style.right );
		
		/* Tu's: */
		if (!x && !y ){
			var w = Drag.obj.root.style["width"] ? parseInt(Drag.obj.root.style["width"]) / 2 : 0;			
			Drag.obj.root.style[o.hmode ? "left" : "right"] = e.clientX - w + "px";
			Drag.obj.root.style[o.vmode ? "top" : "bottom"] = e.clientY - 5 + "px";			
		}
		/* ---- */
		
		o.root.onDragStart(x, y);
		
		o.lastMouseX	= e.clientX;
		o.lastMouseY	= e.clientY;

		if (o.hmode) {
			if (o.minX != null)	o.minMouseX	= e.clientX - x + o.minX;
			if (o.maxX != null)	o.maxMouseX	= o.minMouseX + o.maxX - o.minX;
		} else {
			if (o.minX != null) o.maxMouseX = -o.minX + e.clientX + x;
			if (o.maxX != null) o.minMouseX = -o.maxX + e.clientX + x;
		}

		if (o.vmode) {
			if (o.minY != null)	o.minMouseY	= e.clientY - y + o.minY;
			if (o.maxY != null)	o.maxMouseY	= o.minMouseY + o.maxY - o.minY;
		} else {
			if (o.minY != null) o.maxMouseY = -o.minY + e.clientY + y;
			if (o.maxY != null) o.minMouseY = -o.maxY + e.clientY + y;
		}

		document.onmousemove	= Drag.drag;
		document.onmouseup		= Drag.end;
		
		return false;
	},

	drag : function(e)
	{		
		e = Drag.fixE(e);
		var o = Drag.obj;

		var ey	= e.clientY;
		var ex	= e.clientX;
		var y = parseInt(o.vmode ? o.root.style.top  : o.root.style.bottom);
		var x = parseInt(o.hmode ? o.root.style.left : o.root.style.right );
		var nx, ny;

		if (o.minX != null) ex = o.hmode ? Math.max(ex, o.minMouseX) : Math.min(ex, o.maxMouseX);
		if (o.maxX != null) ex = o.hmode ? Math.min(ex, o.maxMouseX) : Math.max(ex, o.minMouseX);
		if (o.minY != null) ey = o.vmode ? Math.max(ey, o.minMouseY) : Math.min(ey, o.maxMouseY);
		if (o.maxY != null) ey = o.vmode ? Math.min(ey, o.maxMouseY) : Math.max(ey, o.minMouseY);

		// limit movement
		var myWidth = 0;
		if( typeof( window.innerWidth ) == 'number' ) {
			myWidth = window.innerWidth;
		} else if( document.documentElement && ( document.documentElement.clientWidth || document.documentElement.clientHeight ) ) {
			myWidth = document.documentElement.clientWidth;
		} else if( document.body && ( document.body.clientWidth || document.body.clientHeight ) ) {
			myWidth = document.body.clientWidth;
		}
		
		nx = x + ((ex - o.lastMouseX) * (o.hmode ? 1 : -1));
		ny = y + ((ey - o.lastMouseY) * (o.vmode ? 1 : -1));
		if (o.xMapper)		nx = o.xMapper(y)
		else if (o.yMapper)	ny = o.yMapper(x)
		var rwidth = parseInt(o.root.style.width);
		var rwidth = parseInt(o.root.style.width);
		var tx = nx + rwidth;		
		if (nx > 0 && tx < myWidth && ny > 0){
			Drag.obj.root.style[o.hmode ? "left" : "right"] = nx + "px";
			Drag.obj.root.style[o.vmode ? "top" : "bottom"] = ny + "px";
			Drag.obj.lastMouseX	= ex;
			Drag.obj.lastMouseY	= ey;
			Drag.obj.root.onDrag(nx, ny);
		}
		return false;
	},

	end : function()
	{
		document.onmousemove = null;
		document.onmouseup   = null;
		Drag.obj.root.onDragEnd(	parseInt(Drag.obj.root.style[Drag.obj.hmode ? "left" : "right"]), 
									parseInt(Drag.obj.root.style[Drag.obj.vmode ? "top" : "bottom"]));
		Drag.obj = null;
	},

	fixE : function(e)
	{
		if (typeof e == 'undefined') e = window.event;
		if (typeof e.layerX == 'undefined') e.layerX = e.offsetX;
		if (typeof e.layerY == 'undefined') e.layerY = e.offsetY;
		return e;
	}
};

/* 
	Author: Tu Hoang
	Many thanks to Dom-Drag www.youngpup.net 	
*/
/* tooltip implementation */
var MyToolTip = {
	o: null, 
	content: null,
	tooltip: null, 
	id: null, 
	style: null,
	shadow_width: null, 
	shadow_color: null, 
	opacity: null,
	
	init: function(obj, str, cssstyle, opa, shadowwidth, shadowcolor){
		o = obj;		
		id = Math.floor(Math.random()*1000001);			
		content = str;		
		tooltip = null;
		opacity = opa;
		style = cssstyle;
		shadow_width = shadowwidth ? shadowwidth : null;
		shadow_color = shadowcolor ? shadowcolor: null;		
							
		var tt = document.getElementById("tt_" + id);
		if (!tt){
			tt = document.createElement("div");
			tt.setAttribute("id", "tt_" + id);
			if (style != null){
				if (document.all){					
					// IE 5.5 or above
					var ieFilter = shadow_width ? " progid:DXImageTransform.Microsoft.DropShadow(OffX=" + shadow_width + ", OffY=" + shadow_width + ", Positive=true, Color=" + (shadow_color ? shadow_color : "black") + ") " : "";
					ieFilter += opacity ? "progid:DXImageTransform.Microsoft.Alpha(opacity=" + opacity + ")" : "";
					var checkstyle = style.split(":");
					checkstyle.length > 1 ? tt.style.setAttribute("cssText", style): tt.className = style;	
					tt.style.filter = ieFilter;									
				}
				else{
					// shadow for firefox implement later					
					var checkstyle = style.split(":");					
					checkstyle.length > 1 ? tt.setAttribute("style", style) : tt.className = style;
					opacity ? tt.style.MozOpacity = opacity/100.0 : false;
				}				
			}			
			tt.innerHTML = content;
			tt.style.position = "absolute";						
			o.appendChild(tt);
		}		
		tooltip = tt;
				
		o.onmousemove = MyToolTip.move;
		o.onmouseout = MyToolTip.destroy;
	},
	
	move: function(e){
		e = MyToolTip.fixE(e);
		
		var top = 0, left = 0;
		if (document.documentElement && document.documentElement.scrollTop){
			top = document.documentElement.scrollTop;
			left = document.documentElement.scrollLeft;				
		}
		else if (document.all){
			top = document.body.scrollTop;
			left = document.body.scrollLeft;		
		}
		else {
			top = window.pageYOffset;
			left = window.pageXOffset;			
		}			
		
		var x = (e.pageX ? e.clientX : e.x + document.body.scrollLeft) + 5;
		//var y = (e.pageY ? e.clientY : e.y + document.body.scrollTop) + 10;
		
		/*******/
		var y=e.clientY + 10;		
		/****************/
		//alert("e.pageY="+e.pageY+" e.clientY="+e.clientY+" e.y="+e.Y+" document.body.scrollTop="+document.body.scrollTop+" y="+y);		
		
		var w = document.all ? document.body.offsetWidth : window.innerWidth;
		var h = document.all ? document.body.offsetHeight : window.innerHeight;
		var tt = tooltip ? tooltip : false;
		if (tt){
			var tt_w = tt.style.width ? parseInt(tt.style.width) : 0;
			var tt_h = tt.style.height ? parseInt(tt.style.height) : 0;			
			x = ((x + tt_w) > w) ? (x - tt_w - 5) : x;
			y = ((y + tt_h) > h) ? (y - tt_h - 20) : y;
			tt.style.left = x + left;
			tt.style.top = y + top;			
		}
	}, 
	
	destroy: function(e){		
		var tt = tooltip ? tooltip : false;
		if (tt){
			try{					
				o.removeChild(tt);
			}
			catch(e){				
				tt.visibility = "hidden";
			}
		}		
	},
	 
	fixE: function(e){
		if (!e)	e = window.event;
		if (typeof e.layerX == 'undefined') e.layerX = e.offsetX;
		if (typeof e.layerY == 'undefined') e.layerY = e.offsetY;
		return e;
	}	
}

var SingleFrame = {	 
	root: null,
	handle: null, 
	
	init: function(link, width, height, posX, posY, style, handle_style){		
		var hd_style = handle_style ? handle_style : "width: " + (width?width:300) + "px; height: 20px; cursor: move; background-color: gray; border: outset 1px #dddddd;";
					
		var top = 0, left = 0;
		if (document.documentElement && document.documentElement.scrollTop){
			top = document.documentElement.scrollTop;
			left = document.documentElement.scrollLeft;	
		}
		else if (document.all){
			top = document.body.scrollTop;
			left = document.body.scrollLeft;
		}
		else {
			top = window.pageYOffset;
			left = window.pageXOffset;
		}
			
		this.root = document.getElementById("frdiv_single");		
		if (!this.root){
			var div = document.createElement("div");
			div.setAttribute("id", "frdiv_single");
						
			var div2 = document.createElement("div");
			div2.setAttribute("id", "hddiv_single");
						
			var frame = document.createElement("iframe");
			frame.setAttribute("id", "fr_single");
			frame.setAttribute("frameborder", 0);
			frame.setAttribute("width", width);
			frame.setAttribute("height", height);
			frame.style.backgroundColor = "white";
			frame.src = link;
			
			/* set style */	
			var type_style = style.split(":");
			var hs_check = hd_style.split(";");
				
			if (document.all){
				type_style.length > 1 ? div.style.setAttribute("cssText", style) : div.className = style;
				hs_check.length > 1 ? div2.style.setAttribute("cssText", hd_style) : div2.className = hd_style;
				
			}
			else{
				type_style.length > 1 ? div.setAttribute("style", style) : div.className = style;
				hs_check.length > 1 ? div2.setAttribute("style", hd_style) : div2.className = hd_style;
				
			}
			div.style.width = width ? (width + 2) + "px" : "300px";			
			(!div2.style.width) ? div2.style.width = width : true;
			(!div2.style.height) ? div2.style.height = 20: true;
			(!div2.style.cursor) ? div2.style.cursor = "move": true;
			
			var closebutton = "<table height=\"100%\" width=\"100%\" border=\"0\" cellspacing=\"0\" cellpadding=\"0\">";
			closebutton += "<tr><td id=\"td_closebutton\" style=\"width:" + (width - 20) + ";\">&nbsp;</td><td>";			
			closebutton += "<img style=\"cursor: pointer;\" border=0 width=18 height=18 src=\"../Image/mm.gif\" onclick=\"SingleFrame.destroy();\">";
			closebutton += "</td></tr></table>";
			div2.innerHTML = closebutton;			
			(posX || posY) ? div.style.position = "absolute" : true;			
						
			posX ? div.style.left = posX + left : true;
			posY ? div.style.top = posY + top : true;
	
			div.appendChild(div2);
			div.appendChild(frame);
			
			this.handle = div2;						
			this.root = div;
			document.body.appendChild(this.root);			
		}
		else {			
			var div = document.getElementById("frdiv_single");									
			var div2 = document.getElementById("hddiv_single");									
			var frame = document.getElementById("fr_single");
			
			if (div && div2 && frame){
				frame.setAttribute("width", width);
				frame.setAttribute("height", height);
				frame.src = link;
				var type_style = style.split(":");
				var hs_check = hd_style.split(";");
			
				if (document.all){
					type_style.length > 1 ? div.style.setAttribute("cssText", style) : div.className = style;
					hs_check.length > 1 ? div2.style.setAttribute("cssText", hd_style) : div2.className = hd_style;					
				}
				else{
					type_style.length > 1 ? div.setAttribute("style", style) : div.className = style;
					hs_check.length > 1 ? div2.setAttribute("style", hd_style) : div2.className = hd_style;					
				}
			
				div.style.width = width ? (width + 2) + "px" : "300px";				
				div2.style.width = width;
				div2.style.height = 20;
				div2.style.cursor = "move";
								
				posX ? div.style.left = posX + left : true;
				posY ? div.style.top = posY + top : true;				
			}			
		}
	},	
	enableDrag: function(value) {
		if (value){
			this.handle && this.root ? Drag.init(this.handle, this.root) : alert("Hasn't initialized SingleFrame object !!");		
		}
		else {
			document.onmousemove = null;
			document.onmouseup   = null;
			this.handle ? this.handle.onmousedown = null: false;
		}
	}, 
	
	move: function(offsetX, offsetY){
		var r = this.root;
		if (r){
			r.style.top = parseInt(r.style.top?r.style.top:0) + offsetY;
			r.style.left = parseInt(r.style.left?r.style.left:0) + offsetX;		
		}
	}, 
	
	destroy: function(){
		if (this.root){
			var obj = document.getElementById("frdiv_single");
			obj ? document.body.removeChild(obj) : false;			
		}
	}	
}

/* use for adjust the frame movement with scroll - comment if no need to use this */
/*
document.all ? window.onscroll = scrollFrame : window.addEventListener("DOMMouseScroll", scrollFrame, false);
function scrollFrame(){	
	var top = 0, left = 0;
	if (document.documentElement && document.documentElement.scrollTop){
		top = document.documentElement.scrollTop;
		left = document.documentElement.scrollLeft;	
	}
	else if (document.all){
		top = document.body.scrollTop;
		left = document.body.scrollLeft;
	}
	else {
		top = window.pageYOffset;
		left = window.pageXOffset;
	}	
	SingleFrame.move(left, top);
}
*/
/****************************** Testing Function **********************/

function action(){
	Drag.init(document.getElementById("handle"), document.getElementById("htmldiv"));
}

function remove(){
	SingleFrame.destroy();
}

function test(){
	/* pop up center screen */
	SingleFrame.init("webform1.aspx", 300, 300, 100, 200, "border: ridge 3px gray;", "class1");
	SingleFrame.enableDrag(true);
}
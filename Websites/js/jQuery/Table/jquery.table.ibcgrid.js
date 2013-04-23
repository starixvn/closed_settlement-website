/**
*
*  IBCGrid plugin for jQuery
*  
*
**/

jQuery.fn.IniTable = function(options) {
    var defaults = {  
        isResizable: true,
        tBody:null
    };  
    var options = $.extend(defaults, options);  
    this.each(function(){
        obj=$(this);
		if (options.isResizable)
		{
		     obj.kiketable_colsizable({
				dragMove : false, 
				dragProxy : "line",
				tbody: options.tBody
				})
			    .end()
		}
	});
};

/*! 
jquery.event.drag.js ~ v1.4 ~ Copyright (c) 2008, Three Dub Media (http://threedubmedia.com)  
Liscensed under the MIT License ~ http://threedubmedia.googlecode.com/files/MIT-LICENSE.txt
*/
;(function($){ // secure $ jQuery alias
/*******************************************************************************************/
// Created: 2008-06-04 | Updated: 2009-01-26
/*******************************************************************************************/
// Events: drag, dragstart, dragend
/*******************************************************************************************/

// jquery method
$.fn.drag = function( fn1, fn2, fn3 ){
	if ( fn2 ) this.bind('dragstart', fn1 ); // 2+ args
	if ( fn3 ) this.bind('dragend', fn3 ); // 3 args
	return !fn1 ? this.trigger('drag') // 0 args
		: this.bind('drag', fn2 ? fn2 : fn1 ); // 1+ args
	};

// local refs
var $event = $.event, $special = $event.special,

// special event configuration
drag = $special.drag = {
	not: ':input', // don't begin to drag on event.targets that match this selector
	distance: 0, // distance dragged before dragstart
	which: 1, // mouse button pressed to start drag sequence
	setup: function( data ){
		data = $.extend({ 
			distance: drag.distance, 
			which: drag.which, 
			not: drag.not
			}, data || {});
		data.distance = squared( data.distance ); //  x² + y² = distance²
		$event.add( this, "mousedown", handler, data );
		},
	teardown: function(){
		$event.remove( this, "mousedown", handler );
		if ( this === drag.dragging ) drag.dragging = drag.proxy = null; // deactivate element
		selectable( this, true ); // enable text selection
		}
	};

// handle drag-releatd DOM events
function handler ( event ){ 
	var elem = this, returned, data = event.data || {};
	// mousemove or mouseup
	if ( data.elem ){ 
		// update event properties...
		elem = event.dragTarget = data.elem; // drag source element
		event.dragProxy = drag.proxy || elem; // proxy element or source
		event.cursorOffsetX = data.pageX - data.left; // mousedown offset
		event.cursorOffsetY = data.pageY - data.top; // mousedown offset
		event.offsetX = event.pageX - event.cursorOffsetX; // element offset
		event.offsetY = event.pageY - event.cursorOffsetY; // element offset
		}
	// mousedown, check some initial props to avoid the switch statement
	else if ( drag.dragging || ( data.which>0 && event.which!=data.which ) || 
		$( event.target ).is( data.not ) ) return;
	// handle various events
	switch ( event.type ){
		// mousedown, left click, event.target is not restricted, init dragging
		case 'mousedown':
			$.extend( data, $( elem ).offset(), { 
				elem: elem, target: event.target,
				pageX: event.pageX, pageY: event.pageY
				}); // store some initial attributes
			$event.add( document, "mousemove mouseup", handler, data );
			selectable( elem, false ); // disable text selection
			return false; // prevents text selection in safari 
		// mousemove, check distance, start dragging
		case !drag.dragging && 'mousemove': 
			if ( squared( event.pageX-data.pageX ) 
				+ squared( event.pageY-data.pageY ) //  x² + y² = distance²
				< data.distance ) break; // distance tolerance not reached
			event.target = data.target; // force target from "mousedown" event (fix distance issue)
			returned = hijack( event, "dragstart", elem ); // trigger "dragstart", return proxy element
			if ( returned !== false ){ // "dragstart" not rejected
				drag.dragging = elem; // activate element
				drag.proxy = event.dragProxy = $( returned || elem )[0]; // set proxy
				}
		// mousemove, dragging
		case 'mousemove': 
			if ( drag.dragging ){
				returned = hijack( event, "drag", elem ); // trigger "drag"		
				if ( $special.drop ){ // manage drop events
					$special.drop.allowed = ( returned !== false ); // prevent drop
					$special.drop.handler( event ); // "dropstart", "dropend"
					}
				if ( returned !== false ) break; // "drag" not rejected, stop		
				event.type = "mouseup"; // helps "drop" handler behave
				}
		// mouseup, stop dragging
		case 'mouseup': 
			$event.remove( document, "mousemove mouseup", handler ); // remove page events
			if ( drag.dragging ){
				if ( $special.drop ) $special.drop.handler( event ); // "drop"
				hijack( event, "dragend", elem ); // trigger "dragend"	
				}
			selectable( elem, true ); // enable text selection
			drag.dragging = drag.proxy = data.elem = null; // deactivate element
			break;
		} 
	};

// set event type to custom value, and handle it
function hijack ( event, type, elem ){
	event.type = type; // force the event type
	var result = $event.handle.call( elem, event );
	return result===false ? false : result || event.result;
	};
	
// return the value squared	
function squared ( value ){ return Math.pow( value, 2 ); };
	
// toggles text selection attributes	
function selectable ( elem, bool ){ 
	if ( !elem ) return; // maybe element was removed ? 
	elem.unselectable = bool ? "off" : "on"; // IE
	elem.onselectstart = function(){ return bool; }; // IE
	if ( document.selection && document.selection.empty ) document.selection.empty(); // IE
	if ( elem.style ) elem.style.MozUserSelect = bool ? "" : "none"; // FF
	};	
	
/*******************************************************************************************/
})( jQuery ); // confine scope



/*
 * jQuery kiketable.colsizable plugin
 * Version 1.1 (20-MAR-2009)
 * @requires jQuery v1.3.2 or later (http://jquery.com)
 * @requires jquery.event.drag-1.4.js (http://blog.threedubmedia.com/2008/08/eventspecialdrag.html)
 *
 * Examples at: http://www.ita.es/jquery/jquery.kiketable.colsizable.htm
 * Copyright (c) 2007-2009 Enrique Meléndez Estrada
 * Dual licensed under the MIT and GPL licenses:
 * http://www.opensource.org/licenses/mit-license.php
 * http://www.gnu.org/licenses/gpl.html
 *
 */
(function($){

	$.fn.kiketable_colsizable = function(o) {
		// default parameters, properties or settings
		o = $.extend({}, $.fn.kiketable_colsizable.defaults, o);
		o.dragProxy = (o.dragProxy === "line" ) ? false : true;
				
		this
			.filter("table:not(."+o.namespace+")") // only for "virgin" html table(s)
			.addClass(o.namespace)
			.each(function(index){
				o.renderTime = new Date().getTime();
				//
				// global variables
				//
				var isMultiTable=false;
				var oTable =	this,
					$Table =	$(this),
					bTable =    o.tbody,//Body table
					_Cols =		oTable.getElementsByTagName("col");
					if (bTable!=null)
					{
					    var $bTable=    $(bTable),
					    _bCols =    bTable.getElementsByTagName("col");
					    isMultiTable=true;
					}
					;
				_Cols.length && $(o.dragCells,this)
					.each(function(index){
						if (!$(this).hasClass('kiketable-th'))
							$(this).addClass('kiketable-th').wrapInner('<div class="kiketable-th-text"></div>');
						$('<div class="'+o.classHandler+'" title="'+ o.title+'"></div>')
							.prependTo(this)
							.each(function(){
								//
								// global properties
								//
								this.td =	this.parentNode; // alias for TD / CELL of this, if jerarchy changes in future, only depends on this var
								this.$td =	$(this.td);
								this.col = _Cols[this.td.cellIndex];
								this.ncol=_Cols[this.td.cellIndex+1];
                                if (isMultiTable)
								    this.bcol = _bCols[this.td.cellIndex];
								    this.nbcol = _bCols[this.td.cellIndex+1];
							})
							.dblclick( function() { 
								// if loading fast, only once...
								if (this.wtd == null){		
										this.wtd =		this.col.offsetWidth; 
										this.wtd0=		this.wtd;
										};
								
								// change column width
								var minimized = this.wtd == o.minWidth;
								this.wtd = (minimized) ? this.wtd0 : o.minWidth;
								this.col.style.width = this.wtd + "px";
								if (isMultiTable)
								    this.bcol.style.width = this.wtd + "px";
								// change table width (if not fixed)
								if(!o.fixWidth){
									var d = this.wtd0-o.minWidth;
									oTable.style.width = $Table.width()+((minimized)?d:-d)+"px";
									if (isMultiTable)
									    bTable.style.width = $bTable.width()+((minimized)?d:-d)+"px";
									};
								$(this).trigger('minimized');
							})
							//
							// bind a dragstart event, return the proxy element 
							//
							.bind( 'dragstart', function(e){ 
								this.cell_width =	this.$td.width();
								this.table_width =	$Table.width();
								this.left0 =		e.offsetX;
								this.d1 = this.cell_width - this.left0; // precalc for drag event
								this.d2 = o.minWidth - this.d1; // precalc for drag event

								return $(this)
										.clone()
										.appendTo(this.td)
										.css("opacity",o.dragOpacity)
										.css((o.dragProxy)?{
											top:	this.$td.offset().top,
											left:	this.$td.offset().left,
											width:	this.cell_width
											}:{
											top:	this.$td.offset().top,
											left:	e.offsetX
											})
										.removeClass(o.classHandler)
										.addClass(	(o.dragProxy)? o.classDragArea :	o.classDragLine)
										.height($Table.height())
							})
							//
							// bind a drag event, update proxy position 
							//
							.bind( 'drag', (o.dragMove || o.dragProxy)? function(e){ 
									var w = e.offsetX + this.d1;
									if(w - this.d2 - this.d1 >= 0){
										e.dragProxy.style.width = w + "px"; //$(e.dragProxy).css({width: w}) ;
										if (o.dragMove){
											this.col.style.width = w +"px"; // cell width
											if(!o.fixWidth){
												oTable.style.width = (this.table_width - this.cell_width+ w) + "px";
												if (isMultiTable)
												    bTable.style.width =   (this.table_width - this.cell_width+ w) + "px";
												};
										};
									}
								}: function(e){
									var x = e.offsetX;
									if (x - this.d2 >= 0)
										e.dragProxy.style.left = x+"px"; //$(e.dragProxy).css({left: e.offsetX}); 
							}) 
							//
							// bind a dragend event, remove proxy
							//
							.bind( 'dragend', function(e){ 
								if (!o.dragMove){
									var delta = parseInt(e.dragProxy.style.left) - this.left0;
									alert($(this.col).width());
									alert($(this.ncol).width());
									this.col.style.width = (o.dragProxy) ? e.dragProxy.style.width : (this.cell_width + delta)+"px"; // cell width
									
									if (isMultiTable)
									    {
									        this.bcol.style.width= (o.dragProxy) ? e.dragProxy.style.width : (this.cell_width + delta)+"px"; // cell width
									       
									    }
									
									// change table width (if not fixed) 
									if(!o.fixWidth)
										{
										    oTable.style.width = ((o.dragProxy) ? this.table_width - this.cell_width + parseInt(e.dragProxy.style.width) : this.table_width + delta)+"px";
										    if (isMultiTable)
										        bTable.style.width = ((o.dragProxy) ? this.table_width - this.cell_width + parseInt(e.dragProxy.style.width) : this.table_width + delta)+"px";
										}
								}
								$(e.dragProxy)[o.fxHide](o.fxSpeed, function(){$(this).remove()}); 
								$(this).trigger('minimized');
							})
							.bind('minimized', function(e){
								$(this.col)[(parseInt(this.col.style.width) <= o.minWidth) ? "addClass":"removeClass"](o.classMinimized)
								if (isMultiTable)
								    $(this.bcol)[(parseInt(this.bcol.style.width) <= o.minWidth) ? "addClass":"removeClass"](o.classMinimized)
							});
						});
				o.renderTime = new Date().getTime() - o.renderTime;
				o.onLoad();
				});
		return this;
	};
	$.fn.kiketable_colsizable.defaults = {
		dragCells :		"tr:first > *",// cells for allocating column sizing handlers (by default: all cells of first row)
		dragMove :		true,		// see column moving its width? (true/false)
		dragProxy :		"line",		// Shape of dragging ghost ("line"/"area")
		dragOpacity :	.3,			// Opacity for dragging ghost (0 - 1)
		minWidth :		8,			// width for minimized column (px)
		fixWidth :		true,		// table with fixed width? (true/false)
		fxHide :		"fadeOut",	// effect for hiding (fadeOut/hide/slideUp)
		fxSpeed:		200,		// speed for hiding (miliseconds)
		namespace :		"kiketable-colsizable",
		classHandler :	"kiketable-colsizable-handler",
		classDragLine :	"kiketable-colsizable-dragLine",
		classDragArea :	"kiketable-colsizable-dragArea",
		classMinimized: "kiketable-colsizable-minimized",
		title :			'Expand/Collapse this column',
		renderTime :	0,
		tbody:  null,
		onLoad : function(){}
	};
}) (jQuery);

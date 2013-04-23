var jOldVerion = parseFloat($.fn.jquery.substring(0, 3)) <= 1.5 ? true : false;

(function ($) {
    $.fn.gridScroller = function (opts) {
        return this.each(function () {
            var o=$(this);
            var xscrollbar = o.find(".x-scrollbar");
            var yscrollbar = o.find(".y-scrollbar");
            var container = o.find(".container");
            var options = $.extend({}, $.fn.renderScroller.defaults, opts);
           
            $(window).resize(function () {
                o.resetGrid(options.tblContentId);
                var opt=$.extend({},{}, options);
                opt.manualCopyHeader=true;
                o.renderScroller(opt);
            });
            o.renderScroller(options);
            return new scroller(o, container, xscrollbar, yscrollbar);
        });
    };

    $.fn.resetGrid=function(){
        return this.each(function (id) {
            var o=$(this);
            var xscrollbar = o.find(".x-scrollbar");
            var container = o.find(".container");
            var tblContent=$("#" + id);
            var colHeaderContainer = o.find(".col_header");
            var colFooterContainer = o.find(".col_footer");
          
            o.css("width", "");
            container.css("width", "");
            o.find(".content-container").css("width","")
            xscrollbar.css("width", "");
            colHeaderContainer.css("width", "");
            colFooterContainer.css("width", "");
            colHeaderContainer.find(".header-container").css("width",  "");
            o.find(".footer-container").css("width", "");
            o.find(".scrollbar-x-content").css("width", "");
            o.find(".free-cell-bottom-left").css("width", "");
        });
    };

    $.fn.renderScroller = function(options)
    {
        return this.each(function () {
            var bar = 17; 
            var o=$(this);
            var titleHeight= o.find(".grid-title") ? 23:0;
            var pager= o.find(".grid-pager");
            var pagerHeight = pager.length>0 ? 23:0;
            var xscrollbar = o.find(".x-scrollbar");
            var yscrollbar = o.find(".y-scrollbar");
            var container = o.find(".container");
            var tblContent=$("#" + options.tblContentId);
            var tblWidth=Math.max(tblContent.width(),options.gridWidth);
            var tblHeight=tblContent.height();
            var freeCellTop= o.find(".free-top-right");
            var freeCellBottom= o.find(".free-bottom-right"); 

            var colHeaderContainer = o.find(".col_header");
            var rowHeaderContainer = o.find(".row_header");
            var colFooterContainer = o.find(".col_footer");
            var rowFooterContainer = o.find(".row_footer");
        
            var freezeWidth = options.freezeWidth;
            var numOfFreezeCol = options.numberFreezeCol;
            var freezeHeight = tblContent.find("thead").height();
            var freezeFooterHeight = colFooterContainer.find("thead").height();

            var gridWidth = options.fullWidth ? (($(window).width() < options.minWidth ? options.minWidth : $(window).width()) - options.marginRight) : options.gridWidth;
            var isScrollX=gridWidth < tblWidth;
            gridWidth=isScrollX ? gridWidth:(tblWidth + bar);

            var gridHeight = options.fullHeight ? ($(window).height() - options.gridHeight  ) : options.gridHeight;
            var isScrollY= gridHeight - titleHeight - pagerHeight - bar - freezeFooterHeight < tblHeight ;
       
            gridHeight=isScrollY ? gridHeight - titleHeight - pagerHeight - freezeFooterHeight  : (tblHeight + titleHeight);
                      
            if (!options.manualCopyHeader) {
                var thead = $("<thead></thead>");
                var tbody = $("<tbody></tbody>");
              
                var theadFreezeCell = $("<thead></thead>");
                var i, j;
                var tr, cols;
                var rows = tblContent.find("thead tr");
                for (i = 0; i < rows.length; i++) {
                    var className=$(rows[i]).attr("class");
                    cols = $(rows[i]).find("th").clone();
                    tr = $(className!==undefined? "<tr class='" + className + "'></tr>": "<tr></tr>");
                    for (j = 0; j < numOfFreezeCol; j++) {
                        tr.append(cols[j]);
                    }
                    thead.append(tr.clone());
                    theadFreezeCell.append(tr);
                }
            
                o.find(".row_header_content").empty().append(thead);
                o.find(".free-cell-top-left").empty().append(theadFreezeCell);

                rows =tblContent.find("tbody tr");
                for (i = 0; i < rows.length; i++) {
                    var className=$(rows[i]).attr("class");
                    cols = $(rows[i]).find("td").clone();
                    tr = $(className!==undefined? "<tr class='" + className + "'></tr>": "<tr></tr>");
                    for (j = 0; j < numOfFreezeCol; j++) {
                        tr.append(cols[j]);
                    }
                    tbody.append(tr);
                }
                o.find(".row_header_content").append(tbody);

                rows =tblContent.find("thead tr");
                thead = $("<thead></thead>");
                for (i = 0; i < rows.length; i++) {
                    var className=$(rows[i]).attr("class");
                    tr = $(className!==undefined? "<tr class='" + className + "'></tr>": "<tr></tr>");
                    cols = $(rows[i]).find("th").clone();
                    for (j = 0; j < cols.length; j++) {
                        tr.append(cols[j]);
                    }
                    thead.append(tr);
                }
                o.find(".col_header_content").empty().append(thead);
            }
          
            o.css("width", gridWidth + "px");
            
            container.css("width", gridWidth + "px");
            pager.css("width", gridWidth + "px");
            o.find(".content-container").css("width",tblWidth + "px")
            xscrollbar.css("width", (gridWidth - freezeWidth- bar - options.padding*2) + "px");
            colHeaderContainer.css("width", gridWidth + "px");
            colHeaderContainer.find(".header-container").css("width", tblWidth + "px");
            o.find(".footer-container").css("width", tblWidth + "px");
            o.find(".scrollbar-x-content").css("width", (tblWidth-freezeWidth) + "px");
            o.find(".free-cell-bottom-left").css("width", freezeWidth  + "px");
            
            o.css("height", (gridHeight + freezeFooterHeight + bar + pagerHeight) + "px");
            rowHeaderContainer.css("height", (gridHeight - titleHeight)  + "px");
            container.css("height", (gridHeight - titleHeight)  + "px");
            yscrollbar.css("height", (gridHeight - freezeHeight - titleHeight) + "px");
            o.find(".free-cell-top-right").css("height", freezeHeight  - ($.browser.msie ? (options.padding*2 +1):0) + "px");
            o.find(".free-cell-bottom-right").css("height", (freezeFooterHeight + bar) + "px");
            o.find(".scrollbar-y-content").css("height", (tblHeight - freezeHeight)  + "px");

            yscrollbar.css("top", (freezeHeight + titleHeight) + "px");
            xscrollbar.css("top", gridHeight + "px");
            o.find(".free-bottom-left").css("top", gridHeight + "px");
            freeCellBottom.css("top", gridHeight + "px");
            rowFooterContainer.css("top",(gridHeight + bar + 1 ) + "px");
            colFooterContainer.css("top",(gridHeight + bar + 1 ) + "px").css("width", gridWidth + "px");
            pager.css("top",(gridHeight + bar + freezeFooterHeight) + "px")

            xscrollbar.css("left", (freezeWidth + options.padding*2)  + "px");
            yscrollbar.css("left", (gridWidth - bar - 1) + "px");
            freeCellTop.css("left", (gridWidth - bar - 1) + "px");
            freeCellBottom.css("left", (gridWidth - bar -1) + "px");

            if (isScrollY)
            {
                yscrollbar.css("overflow-y","scroll");
            }
            else
            {
                yscrollbar.css("overflow-y","hidden");
            }
            
            if (isScrollX)
            {
                xscrollbar.css("overflow-x","scroll");
            }
            else
            {
                xscrollbar.css("overflow-x","hidden");
            }
        });
    };

     $.fn.renderScroller.defaults = {
        tblContentId:'grid1',
        manualCopyHeader:false,
        fullWidth:true,
        fullHeight:true,
		gridWidth: 0,
        minWidth:600,
        gridHeight:300,
        freezeWidth:241,
        numberFreezeCol:2,
        padding:2,
        marginRight:6
	    };

    var scroller = function(obj, container,xscrollbar,yscrollbar) {
        this.preLeft = xscrollbar.scrollLeft();
        this.preTop = yscrollbar.scrollTop();
        this.colHeaderContainer = $(obj).find(".col_header");
        this.rowHeaderContainer = $(obj).find(".row_header");
        this.colFooterContainer = $(obj).find(".col_footer");
        this.initialize(container,xscrollbar,yscrollbar);
    };

    scroller.prototype = {
        initialize:function (container,xscrollbar,yscrollbar) {
            var me = this;

            yscrollbar.scroll(function () {
                var top = yscrollbar.scrollTop();
                if (me.preTop != top) {
                    if (jOldVerion) {
                        container.attr("scrollTop", top);
                        me.rowHeaderContainer.attr("scrollTop", top);
                    }
                    else {
                        container.prop("scrollTop", top);
                        me.rowHeaderContainer.prop("scrollTop", top);
                    }
                   me.preTop = top;
                }
            });


            container.scroll(function () {
                var top = container.scrollTop();
                if (me.preTop != top) {
                    if (jOldVerion) {
                        yscrollbar.attr("scrollTop", top);
                        me.rowHeaderContainer.attr("scrollTop", top);
                    }
                    else {
                        yscrollbar.prop("scrollTop", top);
                        me.rowHeaderContainer.prop("scrollTop", top);
                    }
                    me.preTop = top;
                }
            });

            xscrollbar.scroll(function () {
                var left = xscrollbar.scrollLeft();
                if (me.preLeft != left) {
                    if (jOldVerion) {
                        container.attr("scrollLeft", left);
                        me.colHeaderContainer.attr("scrollLeft", left);
                        me.colFooterContainer.attr("scrollLeft", left);
                    }
                    else {
                        container.prop("scrollLeft", left);
                        me.colHeaderContainer.prop("scrollLeft", left);
                        me.colFooterContainer.prop("scrollLeft", left);
                    }
                    me.preLeft = left;
                }

            });
        }
    };
})(jQuery);
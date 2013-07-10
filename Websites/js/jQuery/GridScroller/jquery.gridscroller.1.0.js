var jOldVerion = parseFloat($.fn.jquery.substring(0, 3)) <= 1.5 ? true : false;
var ub = $.browser;
ub.chrome = /chrome/.test(navigator.userAgent.toLowerCase());
var ff16 = ub.mozilla && parseInt(ub.version, 10) >= 16;

(function ($) {

    $.fn.gridScroller = function (opts) {

        return this.each(function () {
            var o = $(this);
            var xscrollbar = o.find(".x-scrollbar");
            var yscrollbar = o.find(".y-scrollbar");
            var container = o.find(".container");
            var rowheader = o.find(".row_header");
            var options = $.extend({}, $.fn.renderScroller.defaults, opts);

            $(window).resize(function () {
                if (!o.hasClass("hidden")) {
                    o.resetGrid(options.tblContentId);
                    var opt = $.extend({}, {}, options);
                    opt.manualCopyHeader = true;
                    o.renderScroller(opt);
                    o.find('.grid-fixed-table').append(' ');
                }
            });
            o.renderScroller(options);
            o.find('.grid-fixed-table').append(' ');
            return new scroller(o, container, rowheader, xscrollbar, yscrollbar);
        });

    };

    $.fn.initGrid = function (options) {
        return this.each(function () {
            var o = $(this);
            //Render Free Cell
            var freeCellTpl = '<div class="free-cell free-bottom-left">\
                    <table class="grid-fixed-table">\
                        <thead>\
                            <tr>\
                                <th class="free-cell-bottom-left">\
                                    <div>\
                                    </div>\
                                </th>\
                            </tr>\
                        </thead>\
                    </table>\
                </div>\
                <div class="free-cell free-top-right">\
                    <table class="grid-fixed-table">\
                        <thead>\
                            <tr>\
                                <th class="free-cell-top-right">\
                                    <div>\
                                    </div>\
                                </th>\
                            </tr>\
                        </thead>\
                    </table>\
                </div>\
                <div class="free-cell free-bottom-right">\
                    <table class="grid-fixed-table">\
                        <thead>\
                            <tr>\
                                <th class="free-cell-bottom-right">\
                                    <div>\
                                    </div>\
                                </th>\
                            </tr>\
                        </thead>\
                    </table>\
                </div>';
            o.append(freeCellTpl);
            //Render Scrollbar
            var scrollbarTpl = '<div class="y-scrollbar">\
                    <div class="scrollbar-y-content">\
                        &nbsp;</div>\
                </div>\
                <div class="x-scrollbar">\
                    <div class="scrollbar-x-content">\
                        &nbsp;</div>\
                </div>';
            o.append(scrollbarTpl);
            if (options.isShowPager) {
                var pagerTpl = '<div class="grid-pager">\
                                <div class="grid-pager-content">\
                                    <div id="pagination">&nbsp;</div>\
                                </div>\
                            </div>';
                o.append(pagerTpl);
            }
        });
    };

    $.fn.resetGrid = function () {
        return this.each(function (id) {
            var o = $(this);
            var xscrollbar = o.find(".x-scrollbar");
            var container = o.find(".container");
            var tblContent = $("#" + id);
            var colHeaderContainer = o.find(".col_header");
            var colFooterContainer = o.find(".col_footer");

            o.css("width", "");
            container.css("width", "");
            o.find(".content-container").css("width", "")
            xscrollbar.css("width", "");
            colHeaderContainer.css("width", "");
            colFooterContainer.css("width", "");
            colHeaderContainer.find(".header-container").css("width", "");
            o.find(".footer-container").css("width", "");
            o.find(".scrollbar-x-content").css("width", "");
            o.find(".free-cell-bottom-left").css("width", "");

        });
    };

    $.fn.renderScroller = function (options) {
        return this.each(function () {

            var bar = options.bar;
            var o = $(this);
            var title = o.find(".grid-title");
            var titleHeight = (!title.hasClass('hidden') && title.length > 0) ? title.height() : 0;
            var pager = o.find(".grid-pager");
            var pagerHeight = pager.length > 0 ? pager.height() : 0;
            var xscrollbar = o.find(".x-scrollbar");
            var yscrollbar = o.find(".y-scrollbar");
            var container = o.find(".container");
            var tblContent = o.find("#" + options.tblContentId);

            var tblWidth = Math.max(tblContent.width(), options.gridWidth);


            var freeCellTop = o.find(".free-top-right");
            var freeCellBottom = o.find(".free-bottom-right");

            var colHeaderContainer = o.find(".col_header");
            var rowHeaderContainer = o.find(".row_header");
            var rowHeaderWarpper = o.find(".row_header_wrapper");
            var colFooterContainer = o.find(".col_footer");
            var rowFooterContainer = o.find(".row_footer");
            var isShowFooter = !colFooterContainer.hasClass('hidden') && colFooterContainer.length > 0;

            var freezeWidth = options.freezeWidth;
            var numOfFreezeCol = options.numberFreezeCol;


            var gridWidth = options.fullWidth ? (($(window).width() < options.minWidth ? options.minWidth : $(window).width()) - options.marginRight) : options.gridWidth;
            var isScrollX = gridWidth < tblWidth + 17;
            gridWidth = isScrollX ? gridWidth : (tblWidth + bar);



            if (!options.manualCopyHeader) {
                var thead = $("<thead></thead>");
                var tbody = $("<tbody></tbody>");

                var theadFreezeCell = $("<thead></thead>");
                var i, j;
                var tr, cols;
                var rows = tblContent.find("thead tr");
                for (i = 0; i < rows.length; i++) {
                    var className = $(rows[i]).attr("class");
                    cols = $(rows[i]).find("th").clone();
                    tr = $(className !== undefined ? "<tr class='" + className + "'></tr>" : "<tr></tr>");
                    for (j = 0; j < numOfFreezeCol; j++) {
                        tr.append(cols[j]);
                    }
                    thead.append(tr.clone());
                    theadFreezeCell.append(tr);
                }

                o.find(".row_header_content").empty().append(thead);
                o.find(".free-cell-top-left").empty().append(theadFreezeCell);

                rows = tblContent.find("tbody tr");
                for (i = 0; i < rows.length; i++) {
                    var className = $(rows[i]).attr("class");
                    cols = $(rows[i]).find("td").clone();
                    tr = $(className !== undefined ? "<tr class='" + className + "'></tr>" : "<tr></tr>");
                    for (j = 0; j < numOfFreezeCol; j++) {
                        tr.append(cols[j]);
                    }
                    tbody.append(tr);
                }
                o.find(".row_header_content").append(tbody);

                rows = tblContent.find("thead tr");
                thead = $("<thead></thead>");
                for (i = 0; i < rows.length; i++) {
                    var className = $(rows[i]).attr("class");
                    tr = $(className !== undefined ? "<tr class='" + className + "'></tr>" : "<tr></tr>");
                    cols = $(rows[i]).find("th").clone();
                    for (j = 0; j < cols.length; j++) {
                        tr.append(cols[j]);
                    }
                    thead.append(tr);
                }
                o.find(".col_header_content").empty().append(thead);
            }

            o.css("width", gridWidth + "px");
            o.find(".content-container").css("width", tblWidth + "px")
            container.css("width", gridWidth + "px");

            var isFreeBottomLeft_Visible = o.find(".free-bottom-left").css("display") != "none";
            xscrollbar.css("width", (gridWidth - freezeWidth - bar - 
                                     (isFreeBottomLeft_Visible ? options.padding * 2 : 0)) + "px");
            colHeaderContainer.css("width", gridWidth + "px");
            colHeaderContainer.find(".header-container").css("width", tblWidth + "px");
            o.find(".footer-container").css("width", tblWidth + "px");
            o.find(".scrollbar-x-content").css("width", (tblWidth - freezeWidth) + "px");
            o.find(".free-cell-bottom-left").css("width", freezeWidth + "px");
            pager.css("width", gridWidth + "px");
            rowHeaderContainer.css({ "width": (freezeWidth + 5 + 17) + "px" });
            rowHeaderWarpper.css({ "width": (freezeWidth + 5) + "px" });

            var tblHeight = tblContent.height();
            var freezeHeight = tblContent.find("thead").height();
            var freezeFooterHeight = colFooterContainer.length > 0 ? colFooterContainer.find("thead").height() : 0;
            var gridHeight = options.fullHeight ? ($(window).height() - options.gridHeight) : options.gridHeight;
            var isScrollY = gridHeight - titleHeight - pagerHeight - bar - freezeFooterHeight < tblHeight;
            gridHeight = isScrollY ? gridHeight : tblHeight + titleHeight + pagerHeight + bar + freezeFooterHeight;

            var containerHeight = gridHeight - titleHeight - freezeFooterHeight - pagerHeight - bar;
            o.css("height", (gridHeight - 1 - (isScrollX ? -1 : bar)) + "px");
            rowHeaderContainer.css("height", containerHeight + "px");
            container.css("height", containerHeight + "px");
            rowHeaderWarpper.css("height", containerHeight + "px");
            yscrollbar.css("height", (containerHeight - freezeHeight) + "px");
            o.find(".free-cell-top-right").css("height", freezeHeight - ((ub.msie || ff16 || ub.chrome) ? (options.padding * 2 + (ub.chrome ? 0:1)) : 0) + "px");
            if (isShowFooter) {
                o.find(".free-cell-bottom-right").css("height", (freezeFooterHeight + (isScrollX ? bar : 0) + 1) + "px");
            }
            freeCellBottom.css("top", (gridHeight - pagerHeight - freezeFooterHeight - bar) + "px");
            o.find(".scrollbar-y-content").css("height", (tblHeight - freezeHeight) + "px");

            yscrollbar.css("top", (freezeHeight + titleHeight) + "px");
            xscrollbar.css("top", (gridHeight - pagerHeight - bar - 1) + "px");
            o.find(".free-bottom-left").css("top", (gridHeight - pagerHeight - bar - 1) + "px");

            rowFooterContainer.css("top", (gridHeight - pagerHeight - freezeFooterHeight - bar) + "px");
            colFooterContainer.css("top", (gridHeight - pagerHeight - freezeFooterHeight - bar) + "px").css("width", gridWidth + "px");
            pager.css("top", (gridHeight - pagerHeight - (isScrollX ? 0 : bar) - (isShowFooter ? 1 : 0)) + "px")

            xscrollbar.css("left", (freezeWidth + options.padding * 2) + "px");
            yscrollbar.css("left", (gridWidth - bar - 1) + "px");
            freeCellTop.css("left", (gridWidth - bar - 1) + "px");
            freeCellBottom.css("left", (gridWidth - bar - 1) + "px");

            if (isScrollY) {
                yscrollbar.css("overflow-y", "scroll");
            }
            else {
                yscrollbar.css("overflow-y", "hidden");
            }

            if (isScrollX) {
                xscrollbar.css("overflow-x", "scroll");
            }
            else {
                xscrollbar.css("overflow-x", "hidden");
            }
        });
    };

    $.fn.renderScroller.defaults = {
        tblContentId: 'grid1',
        isUseLeftTable: false,
        isShowPager: false,
        fullWidth: true,
        fullHeight: true,
        gridWidth: 0, //=totalColumnWidth + totalColumn*4 +totalColumn
        minWidth: 600,
        gridHeight: 300,
        freezeWidth: 241, //=totalColumnWidth + (totalColumn-1)*4 + (totalColumn-1)
        numberFreezeCol: 2,
        padding: 2,
        bar:17,
        marginRight: 8
    };

    var scroller = function (obj, container, rowheader, xscrollbar, yscrollbar) {
        this.preLeft = xscrollbar.scrollLeft();
        this.preTop = yscrollbar.scrollTop();
        this.colHeaderContainer = $(obj).find(".col_header");
        this.rowHeaderContainer = $(obj).find(".row_header");
        this.colFooterContainer = $(obj).find(".col_footer");
        this.initialize(container, rowheader, xscrollbar, yscrollbar);
    };

    scroller.prototype = {
        initialize: function (container, rowheader, xscrollbar, yscrollbar) {
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
                        me.rowHeaderContainer.attr("scrollTop", top);
                        yscrollbar.attr("scrollTop", top);
                    }
                    else {
                        me.rowHeaderContainer.prop("scrollTop", top);
                        yscrollbar.prop("scrollTop", top);
                    }
                    me.preTop = top;
                }
            });

            rowheader.scroll(function () {
                var top = rowheader.scrollTop();
                if (me.preTop != top) {
                    if (jOldVerion) {
                        container.attr("scrollTop", top);
                        yscrollbar.attr("scrollTop", top);
                    }
                    else {
                        container.prop("scrollTop", top);
                        yscrollbar.prop("scrollTop", top);
                    }
                    me.preTop = top;
                }
            });

            xscrollbar.scroll(function () {
                var left = xscrollbar.scrollLeft();
                if (me.preLeft != left) {
                    if (jOldVerion) {
                        me.colHeaderContainer.attr("scrollLeft", left);
                        me.colFooterContainer.attr("scrollLeft", left);
                        container.attr("scrollLeft", left);
                    }
                    else {
                        me.colHeaderContainer.prop("scrollLeft", left);
                        me.colFooterContainer.prop("scrollLeft", left);
                        container.prop("scrollLeft", left);
                    }
                    me.preLeft = left;
                }
            });
        }
    };
})(jQuery);

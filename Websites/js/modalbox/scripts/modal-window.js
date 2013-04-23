/************************************************************************************************************
* SIMPLE MODAL v 2.0
* © 2009 FISHBOWL MEDIA LLC
* http://fishbowlmedia.com
***********************************************************************************************************/

(function($) {
    var _settings = {
        width: 800, // Use this value if not set in CSS or HTML
        height: 600, // Use this value if not set in CSS or HTML
        id: 'modal',
        src:''
    }

    /**********************************
    * DO NOT CUSTOMIZE BELOW THIS LINE
    **********************************/
    $.modal = function(options) {
        return _modal(this, options);
    }
    $.modal.open = function() {
        _modal.open();
    }
    $.modal.close = function() {
        _modal.close();
    }
    $.fn.modal = function(options) {
        return _modal(this, options);
    }
    _modal = function(sender, params) {
        this.options = {
            parent: null,
            overlayOpacity: null,
            id: null,
            content: null,
            width: null,
            height: null,
            modalClassName: null,
            imageClassName: null,
            titleClassName: null,
            closeClassName: null,
            overlayClassName: null,
            titleText: null,
            src: null
        }
        this.options = $.extend({}, options, _defaults);
        this.options = $.extend({}, options, _settings);
        this.options = $.extend({}, options, params);
        this.close = function() {
            jQuery('.' + options.modalClassName + ', .' + options.overlayClassName).fadeOut(_settings.fadeOutSpeed, function() { jQuery(this).unbind().remove(); });
        }
        this.open = function() {
            if (typeof options.src == 'function') {
                options.src = options.src(sender)
            } else {
                options.src = options.src || _defaults.src(sender);
            }

            var fileExt = /^.+\.((jpg)|(gif)|(jpeg)|(png)|(jpg))$/i;
            var contentHTML = '';
            if (fileExt.test(options.src)) {
                contentHTML = '<div class="' + options.imageClassName + '"><img src="' + options.src + '"/></div>';
            }
            else {

                contentHTML = '<iframe width="' + options.width + '" height="' + options.height + '" frameborder="0" scrolling="auto" allowtransparency="true" src="' + options.src + '">&lt/iframe>';
            }
            options.content = options.content || contentHTML;

            if (jQuery('.' + options.modalClassName).length && jQuery('.' + options.overlayClassName).length) {
                jQuery('.' + options.modalClassName).html(options.content);
            } else {
                $overlay = jQuery((_isIE6()) ? '<iframe src="BLOCKED SCRIPT\'&lt;html&gt;&lt;/html&gt;\';" scrolling="no" frameborder="0" class="' + options.overlayClassName + '"></iframe><div class="' + options.overlayClassName + '"></div>' : '<div class="' + options.overlayClassName + '"></div>');
                $overlay.appendTo(options.parent);
                var modalcontent;
                modalcontent += '<div id="' + options.id + '" class="' + options.modalClassName + '" style="width:' + (options.width) + 'px; height:' + (options.height + 21) + 'px; top:' + ($(window).height() - (options.height + 21)) / 2 + 'px; left:' + ($(window).width() - options.width) / 2 + 'px;">';
                modalcontent += '<div class="' + options.titleClassName + '" style="width:' + options.width + 'px; height:18px;"><span>' + options.titleText + '<span></div>';
                modalcontent += '<div style="position:absolute;top:21px;width:' + options.width + 'px; height:' + options.height + 'px;">' + options.content + '</div>';
                modalcontent += '</div>';
                $modal = jQuery(modalcontent);
                $modal.appendTo(options.parent);

                $close = jQuery('<a class="' + options.closeClassName + '"></a>');
                $close.appendTo($modal);

                //var overlayOpacity = _getOpacity($overlay.not('iframe')) || options.overlayOpacity;
                //$overlay.fadeTo(0, 0).show().not('iframe').fadeTo(_settings.fadeInSpeed, overlayOpacity);
                //$modal.fadeIn(_settings.fadeInSpeed);
                //$modal.draggable({ iframeFix: true, scroll:false });
                $modal.jqDrag('.' + options.titleClassName);
                $close.click(function() { jQuery.modal().close(); });
                $overlay.click(function() { jQuery.modal().close(); });
            }
        }
        return this;
    }
    _isIE6 = function() {
        if (document.all && document.getElementById) {
            if (document.compatMode && !window.XMLHttpRequest) {
                return true;
            }
        }
        return false;
    }
    //    _getOpacity = function(sender) {
    //        $sender = jQuery(sender);
    //        opacity = $sender.css('opacity');
    //        filter = $sender.css('filter');

    //        if (filter.indexOf("opacity=") >= 0) {
    //            return parseFloat(filter.match(/opacity=([^)]*)/)[1]) / 100;
    //        }
    //        else if (opacity != '') {
    //            return opacity;
    //        }
    //        return '';
    //    }
    _defaults = {
        parent: 'body',
        /*overlayOpacity: 85,*/
        id: 'modal',
        content: null,
        width: 800,
        height: 600,
        modalClassName: 'modal-window',
        titleClassName: 'modal-title',
        imageClassName: 'modal-image',
        closeClassName: 'close-window',
        overlayClassName: 'modal-overlay',
        titleText: '',
        src: function(sender) {
            return jQuery(sender).attr('href');
        }
    }
})(jQuery);
var hack = {
    ieBookmark: null,
    ieSaveSelect: function (evt) {
        //store bookmark
        var editor = tinyMCE.get('news_content');
        hack.ieBookmark = editor.selection.getBookmark();
    }
};
var newsAjax = "../../Services/WCF/ReleaseLog/ServiceHandler.svc/";
/// whatsnews const
var whatNews = {
    loaded: null,
    page: {list: "News.aspx", detail: "NewsArchive.aspx"},
    action: { SaveNews: newsAjax.concat("InsNews"),
        DelNews: newsAjax.concat("DelNews"),
        ListNews: newsAjax.concat("GetNewsList"),
        DetailNews: newsAjax.concat("GetNewsDetail"),
        HotNews: newsAjax.concat("HotNews"),
        Upload: "".concat("NewsUpload.aspx")
    },
    message: { news_deleted: "This release log has been deleted.",
        news_delfailed: 'This release log has been deleted fail.',
        news_confirm: "Are you sure you want to delete this release log?",
        cont_empty: "The release log content can not be blank.",
        title_empty: "The release log title can not be blank.",
        data_notfound: '<p align="center">Data cannot found.</p>',
        file_error: 'Sorry, You have chosen an invalid file extension. Please choose an image file.',
        func_empty: 'The release log function can not empty.',
        upload_fail: 'You don\'t have permission to upload file.'
    },
    appId: 0,
    curNewsId: 0,
    pageSize: 15,
    curPage: 1,
    thumbnail: "",
    setting: { url: "", appId: 0 },
    updating: false,
    imageExt: "jpg,jpeg,gif,png,bmp"
};
/// whatnews management
$.extend(whatNews, {
    setCurrentPage: function (page) {
        if (typeof page == 'undefined') {
            var page = 1;
        }
        whatNews.curPage = parseInt(page);
    },
    setThumbnail: function (thumb) {
        if (typeof thumb == 'undefined') {
            var thumb = "";
        }
        whatNews.thumbnail = thumb;
        $("#news_thumbnail").val(thumb);
        whatNews.hideThumbnail(thumb);
    },
    hideThumbnail: function (thumb) {
        if (typeof thumb != 'undefined' && thumb != "") {
            $("#aimg-clear").show();
            $("#aimg-upload").hide();
        } else {
            $("#aimg-clear").hide();
            $("#aimg-upload").show();
        }
        return false;
    },
    pageLoad: function (hash) {
        var hashInfo = hash;
        if (hashInfo) {
            // restore ajax loaded state
            if ($.browser.msie) {
                hashInfo = encodeURIComponent(hashInfo);
            }
        } else {
            hashInfo = "1";
        }
        whatNews.loaded = true;
        var hashParser = hashInfo.split(".");
        whatNews.setCurrentPage(hashParser[0]);
        $("#goBack").attr("href", "News.aspx#".concat(hashParser[0]));
        switch (hashParser.length) {
            case 1:
                whatNews.goPage(hashParser[0]);
                break;
            case 2:
                whatNews.goDetail(hashParser[1]);
                break;
            case 3:
                switch (hashParser[2]) {
                    case 'a':
                    case 'e':
                        whatNews.goUpdate(hashParser[1]);
                        break;
                    case 'd':
                        whatNews.goDelete(hashParser[1]);
                        break;
                    default:
                        break;
                }
                break;
            default:
                whatNews.goPage(1);
                break;
        }
    },
    displayFeature: function (release) {
        if (typeof release != 'undefined' && release != null) {
            $.tmpl(NewsTemplate.feature, release).appendTo("#news-feature");
        }
    },
    displayShortContent: function (content, newsId) {
        content = unescape(content);
        content = whatNews.stripTags(content);
        if (content.length > 1000) {
            var indOf = content.indexOf(" ", 1000);
            indOf = (indOf != -1) ? indOf : 1000;
            content = content.substring(0, indOf).concat("...");
        }
        content = content.concat('&nbsp;<a id="viewDetail" href="NewsArchive.aspx#1.' + newsId + '">View Detail</a>');
        return content;
    },
    goPage: function (page) {
        ///<summary>List News item</summary>
        ///<param name="page" type="Int">Page number of the list which we will display</param>
        if (typeof page == 'undefined' || page == null) {
            var page = 1
        }
        if (location.href.indexOf("News.aspx") == -1) {
            location.href = "News.aspx#".concat(page);
            return;
        }
        whatNews.setCurrentPage(page);
        var resp = jQuery.jCache.getItem("lnews.".concat(page));
        if (typeof resp == 'undefined' || resp == null) {
            try { parent.BlockUI(); } catch (e) { }
            whatNews.overlay("0.5");
            $.ajax({
                type: "GET",
                url: whatNews.action.ListNews,
                data: { page: page, appId: whatNews.appId },
                cache: false,
                success: function (resp) {
                    try { parent.jQuery.unblockUI(); } catch (e) { }
                    whatNews.overlay("1");
                    if (typeof resp.Rows != 'undefined') {
                        whatNews.displayPage(resp);
                    }
                }
            });
        } else { whatNews.displayPage(resp); }
        return false;
    },
    displayPage: function (data) {
        ///<summary></summary>
        ///<param></param>
        /// permission host
        data.HostRole ? $("div.role").show() : $("div.role").remove();
        if (data.Rows != null && data.Rows.length > 0) {
            $.jCache.setItem("lnews.".concat(whatNews.curPage), data);
            data = $.extend(data, { CurPage: whatNews.curPage, Length: data.Rows.length });
            /// set template
            whatNews.eWhatNews.empty();
            $.tmpl(NewsTemplate.list, data).appendTo("#whatnews_content");
            /// set paging
            $("div#pagination").pagination(data.Total, { current_page: whatNews.curPage, items_per_page: whatNews.pageSize, callback: whatNews.goPage, show_pagesize: false, pageBegin: 1, goPage: true });
            /// bind event news item
            window.setTimeout(function () {
                $("div#news div.r-item").bind("mouseover", function (evt) {
                    $("div#news div.r-item").removeClass("active");
                    $(this).addClass("active");
                });
                $("div#news div.r-item").bind("mouseout", function (evt) {
                    $("div#news div.r-item").removeClass("active");
                });
                $("div#news div.r-item").bind("click", function (evt) {
                    var hash = "".concat(whatNews.curPage).concat(".").concat($(this).attr("rel"));
                    self.location.href = "NewsArchive.aspx#".concat(hash);
                    return false;
                });
            }, 500);
        } else {
            if (this.curPage > 1) {
                this.goPage(this.curPage - 1);
            } else {
                whatNews.eWhatNews.html(this.message.data_notfound);
            }
        }
    },
    goDetail: function (newsID) {
        ///<summary></summary>
        ///<param></param>
        if (typeof newsID != 'undefined' && parseInt(newsID) > 0) {
            this.curNewsId = newsID;
            var resp = $.jCache.getItem("inews.".concat(newsID));
            if (typeof resp == 'undefined' || resp == null) {
                try { parent.BlockUI(); } catch (e) { }
                this.overlay("0.5");
                $.ajax({
                    type: "GET",
                    url: whatNews.action.DetailNews,
                    data: { newsId: newsID, appId: whatNews.appId },
                    cache: false,
                    success: function (resp) {
                        try { parent.jQuery.unblockUI(); } catch (e) { }
                        whatNews.overlay("1");
                        whatNews.showDetail(resp);
                    }
                });
            } else { whatNews.showDetail(resp); }
        }
        return false;
    },
    showDetail: function (data) {
        if (typeof data != 'undefined' && data.NewsID > 0) {
            $.jCache.setItem("inews.".concat(data.NewsID), data);
            data = $.extend(data, { CurPage: this.curPage });
            whatNews.eWhatNews.empty();
            $.tmpl(NewsTemplate.detail, data).appendTo("#whatnews_content");
            window.setTimeout(function () {
                // permission host
                data.HostRole ? $(".role").show() : $(".role").remove();
                $(".return").click(function () {
                    var hash = this.href;
                    hash = hash.replace(/^.*#/, '');
                    $.historyLoad(hash);
                    return false;
                });
                /// rewrite ahref
                var lahref = $(".newscont a");
                if (typeof lahref != 'undefined' && lahref.length > 0) {
                    $.each(lahref, function (i, elm) {
                        $(elm).attr("target", "_blank");
                    });
                }
                /// bind direction
                whatNews.getDirection(data.NewsID);
            }, 500);
        } else {
            whatNews.eWhatNews.html(this.message.data_notfound);
        }
    },
    goUpdate: function (newsId) {
        ///<summary>Update News item</summary>
        ///<param name="newsID" type="Int">Id of news item which we will delete</param>
        if (whatNews.updating) { return; }
        if (typeof newsId == 'undefined') {
            var newsId = 0;
        }
        var data = null;
        $("#news_direction").hide();
        $("#news_addbtn").hide();
        var recipient = $("#recipient").val();
        if (newsId > 0) {

            this.curNewsId = newsId;
            var resp = $.jCache.getItem("inews.".concat(newsId));
            if (typeof resp == 'undefined' || resp == null) {
                try { parent.BlockUI(); } catch (e) { }
                this.overlay("0.5");
                $.ajax({
                    type: "GET",
                    url: whatNews.action.DetailNews,
                    data: { newsId: newsId, appId: whatNews.appId },
                    cache: false,
                    success: function (resp) {
                        try { parent.jQuery.unblockUI(); } catch (e) { }
                        whatNews.overlay("1");
                        whatNews.templateUpdate($.extend(resp, { Recipient: recipient }));
                    }
                });
            } else {
                whatNews.templateUpdate($.extend(resp, { Recipient: recipient }));
            }
        } else {
            this.curNewsId = 0, reception = $("#reception").val();
            var currentTime = new Date();
            var curDate = (currentTime.getMonth() + 1) + "/" + currentTime.getDate() + "/" + currentTime.getFullYear();
            data = { NewsID: "0", Title: "", Date: curDate, NewsContent: "", Func: "", Feature: 0, Thumbnail: "", Recipient: recipient };
            this.templateUpdate(data);
        }
        return false;
    },
    templateUpdate: function (data) {
        if (typeof data != 'undefined' && data != null) {
            whatNews.updating = true;
            data = $.extend(data, { CurPage: this.curPage });
            whatNews.eWhatNews.empty();
            $.tmpl(NewsTemplate.update, data).appendTo(whatNews.eWhatNews);
            window.setTimeout(function () {

                $("a.cancel").click(function () {
                    whatNews.cancelNews();
                    return false;
                });
                $("a.reset").click(function () {
                    whatNews.resetNews();
                    return false;
                });
                $("a.save").click(function () {
                    whatNews.saveNews();
                    return false;
                });
                whatNews.initEditor();
                whatNews.setupCalendar();
                /// set auto focus
                $("#NewsTitleTextBox").focus();

                /// set is feature
                var isFeature = (data.Feature == 1) ? true : false;
                $("#news_feature").attr("checked", isFeature);
                $("#news_feature").bind("click", function (e) {
                    if ($(this).attr("checked")) {
                        $(this).val('1');
                    } else {
                        $(this).val('0');
                    }
                });
                /// image thumbnail
                whatNews.setThumbnail(data.Thumbnail);
                $("#aimg-upload").bind("click", function (e) {
                    whatNews.popupImage(true);
                });
                $("#aimg-clear").bind("click", function (e) {
                    whatNews.setThumbnail('');
                });
                $("#news_notify").bind("click", function (e) {
                    var chk = $(this).attr("checked");
                    chk ? $("#tr_notify").show() : $("#tr_notify").hide();
                    chk ? $("#span_tip").show() : $("#span_tip").hide();
                });
            }, 500);
        } else {
            whatNews.eWhatNews.html(this.message.data_notfound);
        }
    },
    saveNews: function () {
        var newsDate = $.trim($("#news_date").val());
        var newsTitle = $.trim($("#news_title").val());
        if (newsTitle == '') {
            this.showMessage(this.message.title_empty, 5000);
            $("#news_title").focus();
            return;
        }
        var newsContent = tinyMCE.activeEditor.getContent();
        var isBlank = whatNews.checkBlank(newsContent);
        if (isBlank) {
            tinyMCE.get("news_content").focus();
            this.showMessage(this.message.cont_empty, 5000);
            return;
        }
        /// check thumbnail
        if ($.trim($("#news_thumbnail").val()) != "") {
            if (!whatNews.checkFileType($("#news_thumbnail").val())) {
                whatNews.showMessage(whatNews.message.file_error, 5000);
                whatNews.setThumbnail(whatNews.thumbnail);
                $("#news_thumbnail").focus();
                return;
            }
        }
        whatNews.setThumbnail($.trim($("#news_thumbnail").val()));
        ///off func list
        var func = "";
        var feature = parseInt($("#news_feature").val());
        try { parent.BlockUI(); } catch (e) { }
        this.overlay("0.5");
        
        var recipient = $("#news_notify").attr("checked") ? this.stripTags($("#news_recipient").val()) : "";
        var newsData = { NewsID: this.curNewsId, Date: newsDate, Title: encodeURIComponent(newsTitle), NewsContent: encodeURIComponent(newsContent), Func: func, Feature: feature, AppID: whatNews.appId, Thumbnail: whatNews.thumbnail, Recipient: encodeURIComponent(recipient) };
        $.ajax({
            type: "POST",
            url: whatNews.action.SaveNews,
            data: $.toJSON({ newsItem: newsData }),
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (data) {
                try { parent.jQuery.unblockUI(); } catch (e) { }
                whatNews.overlay("1");
                whatNews.updating = false;
                var dataMessage = data.split('|');
                var isError = dataMessage[0] == "True" ? true : false;
                var errorMessage = dataMessage[1];
                var newsID = dataMessage[2];

                if (isError) {
                    whatNews.showMessage(errorMessage, 3000);
                }
                else {
                    var hash = location.href;
                    hash = hash.replace(/^.*#/, '');
                    hash = hash.split(".");
                    if (whatNews.curNewsId == 0) {
                        /* go page first */
                        whatNews.setCurrentPage(1);
                        hash = "1." + newsID;
                    } else {
                        /* go page current */
                        hash = hash[0] + "." + newsID;
                        $.jCache.removeItem("inews.".concat(newsID));
                    }
                    $.historyLoad(hash);
                }
            }
        });
    },
    checkBlank: function (text) {
        if (typeof text != 'undefined') {
            text = whatNews.stripTags(text);
            text = text.split('&nbsp;').join('');
            text = text.replace(/ /gi, '');
            text = text.replace(/\n/gi, '');
            var reg = new RegExp('[a-zA-Z0-9]');
            if (text != '') {
                return reg.exec(text) == null ? true : false;
            }
            return true;
        }
        return true;
    },
    resetNews: function () {
        whatNews.updating = false;
        if (this.curNewsId > 0) {
            this.goUpdate(this.curNewsId);
        } else {
            this.goUpdate(0);
        }
    },
    cancelNews: function () {
        whatNews.updating = false;
        var hash = location.href;
        hash = hash.replace(/^.*#/, '');
        hash = hash.split(".");
        if (this.curNewsId > 0) {
            hash = hash[0] + "." + hash[1];
        } else {
            hash = hash[0];
        }
        $.historyLoad(hash);
    },
    goDelete: function (newsID) {
        ///<summary>Delete News item</summary>
        ///<param name="newsID" type="Int">Id of news item which we will delete</param>
        if (parseInt(newsID) > 0) {
            var strC = whatNews.confirm(whatNews.message.news_confirm);
            if (strC) {
                try { parent.BlockUI(); } catch (e) { }
                whatNews.overlay("0.5");
                $.ajax({
                    type: "POST",
                    url: whatNews.action.DelNews,
                    data: $.toJSON({ newsId: newsID, appId: whatNews.appId }),
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success: function (resp) {
                        try { parent.jQuery.unblockUI(); } catch (e) { }
                        whatNews.overlay("1");
                        if (resp.indexOf('Success') != -1) {
                            $.jCache.removeItem("inews.".concat(newsID));
                            whatNews.removeID(newsID);
                            $("#whatnews_content").html('<p align="center" style="color:red;text-align:center;">' + whatNews.message.news_deleted + '</p>');
                            window.setTimeout(function () {
                                var hash = location.href;
                                hash = hash.replace(/^.*#/, '');
                                hash = hash.split(".");
                                hash = hash[0];
                                $.historyLoad(hash);
                            }, 1000);
                        } else {
                            whatNews.alert(whatNews.message.news_delfailed.concat("-").concat(resp));
                        }
                    }
                });
            }
        }
    },
    setupCalendar: function () {
        ///  Set up calendar
        var newsDate = $('#news_date').val();
        var dates = newsDate.split('/');
        var year = dates[2];
        var month = dates[0];
        var day = dates[1];
        month = month < 10 ? '0' + month : month;
        day = day < 10 ? '0' + day : day;
        var selectionDate = year + month + dates;

        var cal = Calendar.setup({
            animation: false,
            showTime: false,
            bottomBar: true,
            trigger: "news_date_trigger",
            inputField: "news_date",
            dateFormat: "%m/%d/%Y",
            selection: [selectionDate],
            onSelect: function () {
                this.hide();
            }
        });
        cal.selection.set(selectionDate);
    },
    stripTags: function (str, allowed_tags) {
        var key = "", allowed = false; var matches = []; var allowed_array = []; var allowed_tag = ""; var i = 0; var k = ""; var html = ""; var replacer =
        function (search, replace, str) { return str.split(search).join(replace) }; if (allowed_tags) allowed_array = allowed_tags.match(/([a-zA-Z0-9]+)/gi); str += ""; matches = str.match(/(<\/?[\S][^>]*>)/gi); for (key in matches) {
            if (isNaN(key)) continue; html = matches[key].toString(); allowed = false; for (k in allowed_array) {
                allowed_tag = allowed_array[k]; i = -1; if (i != 0) i = html.toLowerCase().indexOf("<" + allowed_tag + ">"); if (i != 0) i = html.toLowerCase().indexOf("<" + allowed_tag + " "); if (i != 0) i = html.toLowerCase().indexOf("</" + allowed_tag);
                if (i == 0) { allowed = true; break }
            } if (!allowed) str = replacer(html, "", str)
        }
        return str
    },
    showMessage: function (message, timewait) {
        $("#divEditNewsErrorMsg").html('<small>' + message + '</small>');
        $("#divEditNewsErrorContainer").show();
        if (typeof timewait != 'undefined' && timewait > 0) {
            window.setTimeout(function () { $("#divEditNewsErrorContainer").slideUp("slow"); }, timewait);
        }
    },
    encode: function (text) {
        text = ("" + text).split("<").join("&lt;").split(">").join("&gt;").split('"').join("&#34;").split("'").join("&#39;");
        return text;
    },
    decode: function (text) {
        text = ("" + text).split("&lt;").join("<").split("&gt;").join(">").split('&#34;').join('"').split("&#39;").join("'");
        return text;
    },
    overlay: function (status) {
        ///
        ///
        status = parseFloat(status) > 1 ? 1 : status;
        whatNews.eWhatNews.css({ opacity: status, float: "left" });
        if (top.location == self.location) {
            if (status == 1) {
                whatNews.eWhatNews.css({ "background-image": "" });
            }
            else {
                whatNews.eWhatNews.css({ "background-image": "url(../../App_Themes/Images/ajax-loader.gif)", "background-position": "center center", "background-repeat": "no-repeat" });
            }
        }
    },
    setFeature: function () {
        var isFeature = $("img#feature").attr("rel");
        isFeature = (isFeature == 0) ? 1 : 0;
        if (typeof isFeature != 'undefined') {
            try { parent.BlockUI(); } catch (e) { }
            $.ajax({
                type: "POST",
                url: whatNews.action.HotNews,
                data: $.toJSON({ newsId: this.curNewsId, feature: isFeature, appId: this.appId }),
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (resp) {
                    try { parent.jQuery.unblockUI(); } catch (e) { }
                    whatNews.overlay("1");
                    if (typeof resp != 'undefined' && resp.indexOf("Success") != -1) {
                        var src = $("img#feature").attr("src");
                        src = isFeature == 1 ? src.replace("uncheck", "check") : src.replace("check", "uncheck");
                        var title = isFeature == 1 ? "UnFeature" : "Feature";
                        $("img#feature").attr("src", src)
                            .attr("title", title)
                            .attr("rel", isFeature);
                        /// remove client cache
                        $.jCache.removeItem("inews.".concat(whatNews.curNewsId));
                    } else {
                        whatNews.alert(resp);
                    }
                }
            });
        }
    }
});
/// whatnews editor
$.extend(whatNews, {
    imageTab: 'url',
    initEditor: function () {
        try {
            $("textarea#news_content").tinymce({
                // Location of TinyMCE script
                script_url: '../../js/tiny_mce/tiny_mce.js',

                // General options
                theme: "advanced",
                plugins: "table,save,emotions,preview,media,paste",

                // Theme options
                theme_advanced_buttons1: "newdocument,|,bold,italic,underline,strikethrough,|,justifyleft,justifycenter,justifyright,justifyfull,styleselect,formatselect,fontselect,fontsizeselect,|,forecolor,backcolor,|,code,preview,help",
                theme_advanced_buttons2: "pastetext,pasteword,|,bullist,numlist,|,outdent,indent,blockquote,|,undo,redo,|,link,unlink,anchor,|,image,upload,emotions,media,|,tablecontrols,pagebreak",
                theme_advanced_buttons3: "",
                theme_advanced_toolbar_location: "top",
                theme_advanced_toolbar_align: "left",
                //theme_advanced_statusbar_location: "bottom",
                theme_advanced_resizing: false,

                // Example content CSS
                content_css: "css/News.css",
                paste_auto_cleanup_on_paste: true,
                paste_remove_spans: true,
                paste_remove_styles: true,
                tab_focus: ':prev,:next',
                entity_encoding: "raw",
                invalid_elements: "script,noscript,link,title,head,body,html,frame,iframe,input,textarea,select",
                setup: function (ed) {
                    // add upload button
                    ed.addButton('upload', {
                        title: 'Upload image',
                        image: 'images/image-up1.png',
                        onclick: function () {
                            whatNews.popupImage();
                            return;
                        }
                    });
                }
            });
        } catch (e) { }
    },
    popupImage: function (is_thumbnail) {
        try {
            var wleft = ($('body').width() / 2) - 170;
            $("#divUploadBar").css({ left: "".concat(wleft).concat("px") }).fadeIn("slow");
            $("#divUploadBar").draggable({
                handle: "#bardrag"
            });
            $("#divUploadBar").ajaxUpload({ ajaxFile: "NewsUpload.aspx", file_types: whatNews.imageExt });
            $("#div-img-headset").bind("click", function (e) {
                if ($("#div-img-contset").is(":hidden")) {
                    $("#div-img-contset").slideDown();
                    $(this).html("" + $(this).html().split("+").join("-"));
                } else {
                    $("#div-img-contset").slideUp();
                    $(this).html("" + $(this).html().split("-").join("+"));
                }
            });
            if (typeof is_thumbnail != 'undefined' && is_thumbnail) {
                $("#is_thumbnail").attr("checked", true);
            }
            $("a.tab-item").bind("click", function (e) {
                $("a.tab-item").removeClass("current");
                $(this).addClass("current");
                //tab content
                $("div.chooseimg").hide();
                $("div#tab-" + $(this).attr("rel")).show();
                $("input#txt_url").val('');
                whatNews.imageTab = $(this).attr("rel");
            });
            // check ie7
            if ($.browser.msie && $.browser.version <= 7) {
                $("div#bardrag").css({ "margin-top": "0px" });
                $("div#bardrag .boxclose").css({ "margin-top": "-15px" });
            }
        } catch (e) { }
    },
    uploadFile: function (data) {
        try {
            var dataMessage = data.split('|');
            if (dataMessage.length > 1) {
                var isError = dataMessage[0] == "True" ? true : false;
                var errorMessage = dataMessage[1];
                var imageUrl = dataMessage[2];
                if (isError) {
                    whatNews.uploadMessage(errorMessage, 5000);
                } else {
                    var html = '<img src="' + imageUrl + '" width="300" height="300" border="0" />';
                    if ($.browser.msie) {
                        try {
                            tinyMCE.execCommand('mceInsertContent', false, html);
                        } catch (e) { }
                    } else {
                        tinyMCE.execCommand("mceInsertContent", false, html);
                    }
                    if ($("#is_thumbnail").attr("checked")) {
                        whatNews.setThumbnail(imageUrl);
                    }
                    whatNews.cancelUploadImage();
                }
            } else {
                whatNews.uploadMessage(whatNews.message.upload_fail, 5000);
            }
        } catch (e) { }
        return false;
    },
    cancelUploadImage: function () {
        whatNews.overlay("1");
        $("#divUploadBar").fadeOut("slow");
        $("#txtfile").val('');
        $("#txt_url").val('');
        $("#is_thumbnail").attr("checked", false);
        $("input.clone").remove();
        $("#myFrame").attr("src", "about:blank");
    },
    checkFileType: function (file_) {
        var ext_ = file_.toLowerCase().substr(file_.toLowerCase().lastIndexOf('.') + 1);
        if (!whatNews.imageExt.match(ext_)) {
            return false;
        }
        else return true;
    },
    uploadMessage: function (message, timewait) {
        $("#upload_message span").html('<small>' + message + '</small>');
        $("#upload_message").show();
        $("#txtfile").val('');
        $("input.clone").remove();
        $("#myFrame").attr("src", "about:blank");
        if (typeof timewait != 'undefined' && timewait > 0) {
            window.setTimeout(function () { $("#upload_message").slideUp("slow"); }, timewait);
        }
    }
});
/// whatnews direction
$.extend(whatNews, {
    listNewsID: [],
    direction: { pre: 0, cur: 0, next: 0 },
    totalRows: 0,
    getDirection: function (newsID) {
        if (this.listNewsID.length == 0) {
            $.ajax({
                type: "GET",
                url: whatNews.action.ListNews,
                data: { page: whatNews.curPage, appId: whatNews.appId },
                cache: false,
                success: function (resp) {
                    if (typeof resp.Rows != 'undefined' && resp.Rows != null) {
                        whatNews.totalRows = resp.Total;
                        $.each(resp.Rows, function (i, val) {
                            whatNews.listNewsID.push(val.NewsID + "." + whatNews.curPage);
                        });
                        whatNews.listNewsID.sort(function (x, y) { return x - y; });
                        //alert(whatNews.listNewsID.join(","));
                        whatNews.calDirection(newsID);
                    }
                }
            });
        } else {
            this.calDirection(newsID);
        }
    },
    calDirection: function (newsID) {
        try {
            if (this.listNewsID.length > 0) {
                var inOf = null, len = this.listNewsID.length, strID = "".concat(newsID).concat("." + whatNews.curPage);
                for (var i = 0; i < len; i++) {
                    if (strID == this.listNewsID[i]) { inOf = i; break; }
                }
                //alert(strID + " - " + whatNews.listNewsID.join(",").indexOf(strID));
                if (inOf != null) {
                    this.direction.cur = strID;
                    this.direction.pre = (inOf > 0) ? this.listNewsID[inOf - 1] : 0;
                    this.direction.next = (inOf < (len - 1)) ? this.listNewsID[inOf + 1] : 0;

                    if ((inOf == 0) && (whatNews.curPage < Math.ceil(whatNews.totalRows / whatNews.pageSize))) {
                        $.ajax({
                            type: "GET",
                            url: whatNews.action.ListNews,
                            data: { page: whatNews.curPage + 1, appId: whatNews.appId },
                            cache: false,
                            success: function (resp) {
                                if (typeof resp.Rows != 'undefined' && resp.Rows != null) {
                                    whatNews.totalRows = resp.Total;
                                    $.each(resp.Rows, function (i, val) {
                                        whatNews.listNewsID.push(val.NewsID + "." + (parseInt(whatNews.curPage) + 1));
                                    });
                                    whatNews.listNewsID.sort(function (x, y) { return x - y; });
                                    //alert(whatNews.listNewsID.join(","));
                                    window.setTimeout(function () { whatNews.calDirection(newsID); }, 500);
                                }
                            }
                        });
                    } else if ((inOf == (len - 1)) && (whatNews.curPage > 1)) {
                        $.ajax({
                            type: "GET",
                            url: whatNews.action.ListNews,
                            data: { page: whatNews.curPage - 1, appId: whatNews.appId },
                            cache: false,
                            success: function (resp) {
                                if (typeof resp.Rows != 'undefined' && resp.Rows != null) {
                                    whatNews.totalRows = resp.Total;
                                    $.each(resp.Rows, function (i, val) {
                                        whatNews.listNewsID.push(val.NewsID + "." + (parseInt(whatNews.curPage) - 1));
                                    });
                                    whatNews.listNewsID.sort(function (x, y) { return x - y; });
                                    //alert(whatNews.listNewsID.join(","));
                                    window.setTimeout(function () { whatNews.calDirection(newsID); }, 500);
                                }
                            }
                        });
                    }

                    this.goDirection();
                }
            }
        } catch (e) { }
    },
    goDirection: function () {
        try {
            //alert($.param(whatNews.direction));
            if (parseFloat(whatNews.direction.pre) > 0) {
                var splitPre = "".concat(whatNews.direction.pre).split(".");
                $("div#news_direction a.pre").attr("href", "#".concat(splitPre[1] + "." + splitPre[0]))
                    .removeClass("nolink");
            } else {
                $("div#news_direction a.pre").attr("href", "javascript:;")
                    .addClass("nolink");
            }
            if (parseFloat(whatNews.direction.next) > 0) {
                var splitNext = "".concat(whatNews.direction.next).split(".");
                $("div#news_direction a.next").attr("href", "#".concat(splitNext[1] + "." + splitNext[0]))
                    .removeClass("nolink");
            } else {
                $("div#news_direction a.next").attr("href", "javascript:;")
                    .addClass("nolink");
            }
            $("div#news_direction").show();
        } catch (e) { }
    },
    removeID: function (newsID) {
        try {
            var len = whatNews.listNewsID.length, strID = "".concat(newsID).concat("." + whatNews.curPage);
            for (var i = 0; i < len; i++) {
                if (strID == whatNews.listNewsID[i]) { whatNews.listNewsID.splice(i, 1); break; }
            }
        } catch (e) {}
    }
});
/// whatnews function list
$.extend(whatNews, {
    fullList: null,
    getFuncList: function (func_name) {
        if (whatNews.fullList == null) {
            $.ajax({
                type: "POST",
                url: "../UserControl/UserControlService.svc/GetFunctionList",
                data: {},
                success: function (resp) {
                    if (typeof resp != 'undefined') {
                        whatNews.fullList = resp;
                        whatNews.renderFuncList(resp, func_name);
                    }
                }
            });
        } else {
            whatNews.renderFuncList(whatNews.fullList, func_name);
        }
    },
    renderFuncList: function (full_list, func_name) {
        if (full_list.length > 0) {
            if ($("#news_func").length > 0) { $("#news_func").empty(); }
            var option = { Value: "", Text: "Choosing a function" };
            whatNews.addOption(option);
            $.each(full_list, function (ind, item1) {
                option.Value = item1.Name;
                option.Text = item1.Desc;
                whatNews.addOption(option);
                if (item1.SubFuncs.length > 0) {
                    $.each(item1.SubFuncs, function (ind, item2) {
                        option.Value = item2.Name;
                        option.Text = item2.Desc;
                        whatNews.addOption(option);
                    });
                }
            });
            if (typeof func_name != 'undefined' && func_name != '') {
                $("#news_func").val(func_name);
            }
        }
        return false;
    },
    addOption: function (option) {
        if (typeof option != 'undefined') {
            var options = $("#news_func").attr('options');
            options[options.length] = new Option(option.Text, option.Value);
        }
        return false;
    }
});
/// popup message
$.extend(whatNews, {
    alert: function (message) {
        window.alert(message);
    },
    confirm: function (message) {
        return window.confirm(message);
    }
});
/// init whatnews management
$.extend(whatNews, {
    eWhatNews: null,
    initApp: function (enews, opts) {
        if (typeof opts != 'undefined' && $(enews).length > 0) {
            whatNews.eWhatNews = enews;
            whatNews.appId = opts.AppID;
            $.historyInit(whatNews.pageLoad);
            if (whatNews.loaded == null) {
                whatNews.pageLoad(location.hash);
            }
        }
    }
});
/// whatnews docking toolbar
$.extend(whatNews, {
    dockUrl: "Handlers/Others/NewsList.ashx",
    eDocking: null,
    timeRun: null,
    scrollData: [],
    curScroll: 0,
    isDockHide: false,
    setting: { AppID: 0, ActionClick: null },
    initDockBar: function (opts) {
        if (typeof opts != 'undefined') {
            this.setting = $.extend(this.setting, opts);
        } else { return; }
        this.eDocking = $('<div id="whatnews-dockbar" style="display: none;" class="docking"></div>');
        $('body').append(this.eDocking);
        $.tmpl(whatNews.dockTemplate.dockCont, { }).appendTo(this.eDocking);
        var params = { page: 1, appId: this.setting.AppID };
        $.ajax({
            type: "GET",
            url: whatNews.dockUrl,
            data: params,
            cache: false,
            success: function (resp) {
                if (typeof resp.Rows != 'undefined' && resp.Rows != null) {
                    $.each(resp.Rows, function (ind, row) {
                        whatNews.scrollData.push(row);
                    });
                    window.setTimeout(function () {
                        var chk_cookie = ($.cookie("hide_bar")!=null)? true : false;
                        $("#whatnews-dockbar-close").bind("click", function (e) {
                            whatNews.closeDock();
                        });
                        if(chk_cookie) {
                            whatNews.timeRun = null;
                            $("#whatnews-dockbar").css({width: "12px"}).show();
                            $("#whatnews-dockbar-cont").hide();
                            $("div.dockclose").removeClass("hide");
                            $("div.dockclose").addClass("show").show();
                            whatNews.isDockHide = true;
                        } else {
                            whatNews.eDocking.toggle("slow");
                            whatNews.resetDock(); whatNews.runIt();
                            whatNews.timeRun = window.setInterval("whatNews.resetDock();whatNews.runIt();", 5000);
                        }
                    }, 500);
                }
            }
        });
    },
    runIt: function () {
        window.setTimeout(function () {
            var aLink = $('<div id="news-item"/>').attr("class", "divclick ".concat(whatNews.scrollData[whatNews.curScroll].NewsID))
                .css({ cursor: "pointer", width: "227px" })
                .attr("NewsID", whatNews.scrollData[whatNews.curScroll].NewsID)
                .bind("click", function (e) {
                    whatNews.actionLink($(this).attr("NewsID"));
                })
                .bind("mouseover", function (e) {
                    $(this).addClass("active");
                    //window.clearInterval(whatNews.timeRun);
                    //whatNews.timeRun = null;
                })
                .bind("mouseout", function (e) {
                    $(this).removeClass("active");
                    if (whatNews.timeRun == null) {
                        //whatNews.timeRun = window.setInterval("whatNews.resetDock();whatNews.runIt();", 5000);
                    }
                });
            var newsData = $.extend(whatNews.scrollData[whatNews.curScroll], { });
            newsData.Title = whatNews.stripTags(newsData.Title);
            $.tmpl(whatNews.dockTemplate.newsItem, newsData).appendTo(aLink);
            $("#whatnews-dockbar-cont").hide()
                .html(aLink)
                .animate({ width: '230px' }, 1500)
                .show();
            //$("#whatnews-dockbar-cont").hide().animate({ height: '26px' }, 1500).show();
            //$("#whatnews-dockbar-cont").fadeIn("slow");
            whatNews.curScroll = (whatNews.curScroll >= whatNews.scrollData.length - 1) ? 0 : (whatNews.curScroll + 1);
        }, 1000);
    },
    resetDock: function () {
        $("#whatnews-dockbar-cont").animate({ width: '0px' }, 1000);
        //$("#whatnews-dockbar-cont").animate({ height: '0px' }, 1500);
        //$("#whatnews-dockbar-cont").fadeOut("slow");
    },
    actionLink: function (newsId) {
        try { whatNews.setting.ActionClick(newsId); } catch (e) { }
        return false;
    },
    closeDock: function () {
        window.clearInterval(whatNews.timeRun);
        whatNews.timeRun = null;
        if (whatNews.isDockHide) {
            $.cookie("hide_bar", null);
            $("div.dockclose").hide();
            $("#whatnews-dockbar-cont").show();
            whatNews.eDocking.animate({ width: '247px' }, 1000, function () {
                $("div.dockclose").removeClass("show");
                $("div.dockclose").addClass("hide").show();
                whatNews.resetDock(); whatNews.runIt();
                whatNews.timeRun = window.setInterval("whatNews.resetDock();whatNews.runIt();", 5000);
                whatNews.isDockHide = false;
            });
        } else {
            $.cookie("hide_bar", "yes", { expires: 7 });
            $("div.dockclose").hide();
            whatNews.eDocking.animate({ width: '12px' }, 1000, function () {
                $("#whatnews-dockbar-cont").hide();
                $("div.dockclose").removeClass("hide");
                $("div.dockclose").addClass("show").show();
                whatNews.isDockHide = true;
            });
        }
    },
    dockTemplate: {
        dockCont: '<div id="whatnews-dockbar-cont" style="display: none;" class="dockcont"></div><div id="whatnews-dockbar-close" class="dockclose hide">&nbsp;</div>',
        newsItem: '<div class="newsgo">&nbsp;</div><div class="newstitle">${Title}</div>'
    }
});
/// whatnews ajax request / cross domain
$.extend(whatNews, {
    toParams: function (params) {
        if (typeof (params) == 'udefined' || typeof (params) != 'object') {
            var params = {};
        }
        if (this.dockUrl.indexOf(document.domain) == -1) {
            params = $.extend(params, { jsoncallback: "?" });
        }
        return params;
    }
});
/// jQuery whatnews management plugins
(function ($) {
    $.fn.whatNews = function (options) {
        var opts = $.extend({}, $.fn.whatNews.defaults, options);
        whatNews.initApp($(this), opts);
    };

    $.fn.whatNews.defaults = {
        AppID: 0,
        PageSize: 10
    };
})(jQuery);
/// jQuery whatnews docking plugins
(function ($) {
    $.fn.newsdocking = function(options) {
        var opts = $.extend({}, $.fn.newsdocking.defaults, options);
        whatNews.initDockBar(opts);
    };

    $.fn.newsdocking.defaults = {
        AppID: 0,
        ActionClick: null
    };
})(jQuery);
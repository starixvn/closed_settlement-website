$(function () {
    ql.init();
    ql.initDataFromParent();
});
function testframe_loaded() {
    try { $('div#divshow').unblock() } catch (e) { }
}
var ql = {
    pagerSetting: {
        pageDisplay: 12,
        pageStart: 1,
        pageSize: 5
    },
    blockUITimeoutId: 0,
    arrayMenu: [],
    init: function () {
        ql.initSearch();

    },
    getAllMenu: function (keyWord) {
        $.ajax({
            type: "POST",
            url: "GetSearchMenu",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: '{"keyWord": ' + JSON.stringify(keyWord) + ',"isSuggestion":"false"}',
            success: function (listMenu) {
                if (typeof listMenu != "undefined") {
                    if (listMenu.length > 0) {
                        ql.arrayMenu = listMenu;
                        $("#resultPagination").show();
                        ql.loadResultPager(1);
                        ql.initPager();
                    }
                    else {
                        //$("#resultTemplateNoInfo").tmpl([{ noinfo: true}]).appendTo($("#resultView").empty());
                        $("#resultView").empty();
                        $("#resultPagination").hide();
                    }
                    $("#numberOfResult").html((listMenu.length > 1 ? "There are <b>" : "There is <b>") + listMenu.length + "</b>" + (listMenu.length > 1 ? " results" : " result") + " match.");
                }
            }
        , error: function (e) {
            ql.showError("Search Error!");
        }
        });
    },
    addEventLinkPage: function () {
        $("#resultView a.linkDirectToPage").each(function () {
            var $this = $(this);
            $this.click(function (e) {
                //handle report menu
                if ($this.attr('linkTarget') != '_blank') {
                    if ($this.attr('IsReportMenu') == 'true') {
                        var menuUrl = $this.attr("href");
                        //alert($this.attr("href"));
                        window.open("../Report/Index.aspx?menuUrl=" + menuUrl, "_blank");
                    }
                    else {
                        parent.$('#item_' + $this.next().val()).trigger('click');
                        if ($.browser.msie && $.browser.version == 7) {
                            //if IE 7 do it
                            parent.$("#centerFr").attr("src", $this.attr("href").replace('/QuickLink', ''));
                        }
                        else {
                            parent.$("#centerFr").attr("src", $this.attr("href"));
                        }


                    }
                }
                else {
                    if ($.browser.msie && $.browser.version == 7) {
                        //if IE 7 do it
                        var newUrl = $this.attr("href").replace(document.location, '').replace('/QuickLink', '');
                        window.open(newUrl, null);
                    }
                    else {
                        window.open('../' + $this.attr("href"), "_blank");
                    }
                }
                e.preventDefault();
            });
        });
    },
    initDataFromParent: function () {
        $("#tbxKeyWord").val(parent.$("#tbxSiteSearchMenu").val());
        ql.search($("#tbxKeyWord").val());
    },
    showError: function (msg) {
        var msgObj = $("#msg");
        msgObj.html(msg).css("color", "Red").show();
        setTimeout(function () {
            msgObj.hide();
        }, 3000);
    },
    showSuccess: function (msg, targetID) {

    },
    initPager: function () {
        var numOfPage = Math.round((ql.arrayMenu.length / this.pagerSetting.pageDisplay) + 0.49);
        $("#resultPagination").paginate({
            count: numOfPage,
            start: this.pagerSetting.pageStart,
            display: this.pagerSetting.pageSize,
            border: true,
            border_color: '#7C98A4',
            text_color: '#000000',
            background_color: '#E3F2E1',
            border_hover_color: '#A2320F',
            text_hover_color: '#A2320F',
            background_hover_color: '#CAE6C6',
            rotate: false,
            images: false,
            mouse: 'press',
            onChange: function (page) {
                ql.loadResultPager(page);
            }
        });
    },
    loadResultPager: function (pageIndex) {
        var tempArrayMenu = jlinq.from(ql.arrayMenu).skip((pageIndex - 1) * this.pagerSetting.pageDisplay).take(this.pagerSetting.pageDisplay);
        $("#resultTemplate").tmpl(tempArrayMenu).appendTo($("#resultView").empty());
        ql.addEventLinkPage();
        ql.addEventShowPreviewPage();
    },
    initSearch: function () {
        //tbxUserName
        $("#tbxKeyWord").keyup(function (e) {
            if (e.keyCode == 37 || e.keyCode == 38 || e.keyCode == 39 || e.keyCode == 40) return true;
            else if (e.keyCode == 13) {
                ql.search($(this).val());
            }
        })
    .val("Site Name")//.css("font-style", "italic")
    .focus(function () {
        if ($(this).val().indexOf("Site Name") >= 0) {
            $(this).val("");
        }
    })
    .focusout(function () {
        if ($(this).val() == "") {
            $(this).val("Site Name"); //.css("font-style", "italic");
        }
    });
    },
    search: function (val) {
        try { parent.BlockUI(); } catch (e) { }
        if (val == "Site Name") {
            val = "";
        }
        ql.getAllMenu(val);
        try { parent.jQuery.unblockUI(); } catch (e) { }
    },
    addEventShowPreviewPage: function () {
        $('.divitemsuggesstion ').each(function () {
            $(this).hover(
            function () {
                $(this).find('td.tdimagesearch').show();
                $(this).addClass('divitemsuggesstionhover');
            },
            function () {
                $(this).find('td.tdimagesearch').hide();
                $(this).removeClass('divitemsuggesstionhover');
            }
            );
        });
        $('.tdimagesearch').click(function () {
            $alink = $(this).prev().find('a');
            var fullDate = new Date();
            var currentDate = (fullDate.getMonth() + 1) + "/" + fullDate.getDate() + "/" + fullDate.getFullYear();
            var paramsDefault = "?fdate=" + currentDate + "&tdate=" + currentDate;
            if ($.browser.msie && $.browser.version == 7) {
                //if IE 7 do it
                if ($alink.attr('IsReportMenu') == 'true') {

                    $('#previewpage').attr("src", $alink.attr('href').replace('/QuickLink', '') + paramsDefault);
                }
                else {
                    $('#previewpage').attr('src', $alink.attr('href').replace('/QuickLink', ''));
                }
            }
            else {
                if ($alink.attr('IsReportMenu') == 'true') {
                    $('#previewpage').attr("src", "../" + $alink.attr('href').replace('../', '') + paramsDefault);
                }
                else {
                    $('#previewpage').attr('src', "../" + $alink.attr('href').replace('../', ''));
                }
            }
        });
        $('.tdimagesearch').each(function () {
            $(this).hover(
            function () {
                $alink = $(this).prev().find('a');
                var fullDate = new Date();
                var currentDate = (fullDate.getMonth() + 1) + "/" + fullDate.getDate() + "/" + fullDate.getFullYear();
                var paramsDefault = "?fdate=" + currentDate + "&tdate=" + currentDate;
                if ($.browser.msie && $.browser.version == 7) {
                    //if IE 7 do it
                    if ($alink.attr('IsReportMenu') == 'true') {

                        $('#previewpage').attr("src", $alink.attr('href').replace('/QuickLink', '') + paramsDefault);
                    }
                    else {
                        $('#previewpage').attr('src', $alink.attr('href').replace('/QuickLink', ''));
                    }
                }
                else {
                    if ($alink.attr('IsReportMenu') == 'true') {
                        $('#previewpage').attr("src", "../" + $alink.attr('href').replace('../', '') + paramsDefault);
                    }
                    else {
                        $('#previewpage').attr('src', "../" + $alink.attr('href').replace('../', ''));
                    }
                }
                $('#overlayDiv').show();
                try {
                    $('div#divshow').block({
                        message: '<table class="blockUI"><tr><td class="tdloading"></td></tr></table>',
                        css: { border: '0px solid #FFFFFF' }
                    });
                } catch (e) { }
            },
            function () {
                //do if mouse leave
            }
            );
        });
        $('.firsttd').each(function () {
            $(this).hover(
                    function () {
                        $('#previewpage').attr('src', "");
                        $('#overlayDiv').hide();
                        clearTimeout(ql.blockUITimeoutId);
                        try { $('div#divshow').unblock() } catch (e) { }
                    },
                    function () {
                        //do if mouse leave
                    }
                );
        });
        if ($("#tbxKeyWord").val() != "") {
            $('.firsttd > a, .firsttd > span').highlight($("#tbxKeyWord").val());
        }
    }

};
var currentType = "master";
function ExportToExcel()
{
    url = "DirectBalance.aspx?display=" + $("#directType").val()+ "&isexport=1";
    document.location.href = url;
}
function View_DL(roleid, user, custid, puser, pcustid) {
    BeginLoad();
    var param = 'roleid=' + roleid + '&pcustid=' + pcustid + "&puser=" + puser + "&custid=" + custid  + '&user=' + user;
    $.ajax({
                type: "POST",
                url: "DirectBalanceDownLine.ashx",
                data: param,
                success: function(rs) {
                    rs = $.evalJSON(rs);
                    rs.data = rs.data.replace("<!--Total-->", "");
                    rs.data = rs.data.replace("<!--/Total-->", "");
                    $('#sData').html(rs.data);
                    $('#dvHeader').html(rs.header);
                    FinishLoad();
                     
                    //--------------------------------
                    fixUIDownline();
                }
           });
}

var fixUIDownline = function() {
    $(document).find("div[class='maincontent_box']").width(480);
    var scrollBarWidth = getScrollbarWidth();
    $("#thDLHeaderScrollbar").width(scrollBarWidth);
    $("#divDLBodyScrollbar").width(scrollBarWidth);
    $("#divDLFooterScrollbar").width(scrollBarWidth);
    
    $("#tcontentdl").find("tbody tr:last td").css("border-bottom", "0px none");
    $("#tdNoInfomation").width(433).css("border-bottom", "1px solid #7C98A4");
    if ($("#tcontentdl").find("tr").length == 1) {
        $("#tcontentdl").parent().height(19);
    }
    
    switch (currentType) {
        case "master":
            if ($.browser.msie) {
                var version = getInternetExplorerVersion();
                if (version == 9) {
                    if ($("#tcontentdl").find("tr").length == 1) {
                        $("#divFooter1").parent().css("margin-top", "-1px");
                    }
                }
                else if (version == 8) {
                    if ($("#tcontentdl").find("tr").length == 1) {
                        $("#divFooter1").parent().css("margin-top", "-1px");
                    }
                }
                else if (version == 7) {
                    if ($("#tcontentdl").find("tr").length == 1) {
                        $("#divFooter1").parent().css("margin-top", "-1px");
                    }
                }
            }
            break;
        case "agent":
            if ($.browser.msie) {
                var version = getInternetExplorerVersion();
                if (version == 9) {
                    if ($("#tcontentdl").find("tr").length == 1) {
                        $("#divFooter1").parent().css("margin-top", "-1px");
                    }
                }
                else if (version == 8) {
                    if ($("#tcontentdl").find("tr").length == 1) {
                        $("#divFooter1").parent().css("margin-top", "-1px");
                    }
                }
                else if (version == 7) {
                    if ($("#tcontentdl").find("tr").length == 1) {
                        $("#divFooter1").parent().css("margin-top", "-1px");
                    }
                }
            }
            break;
    }
    
    //no information
    var isNoInfo = false;
    if ($("#tcontentdl").html().indexOf("There is no information available") >= 0) {
        $("#divFooterTotalValue").parent().remove();
        $("#tdNoInfomation").css("border-bottom", "1px solid #7C98A4");
        isNoInfo = true;
    }
    switch (currentType) {
        case "master":
            if ($.browser.msie) {
                var version = getInternetExplorerVersion();
                if (version == 9) {
                    $("#tdNoInfomation").width(484);
                    if (!isNoInfo) {
                        $("#tdNoInfomation").css("border-bottom", "0px none");
                    }
                }
                else if (version == 8) {
                    $("#tdNoInfomation").width(484);
                    if (!isNoInfo) {
                        $("#tdNoInfomation").css("border-bottom", "0px none");
                    }
                }
                else if (version == 7) {
                    $("#tdNoInfomation").width(484);
                    if (!isNoInfo) {
                        $("#tdNoInfomation").css("border-bottom", "0px none");
                    }
                }
            }
            else {
                $("#tdNoInfomation").width(484);
                if (!isNoInfo) {
                    $("#tdNoInfomation").css("border-bottom", "0px none");
                }
            }
            break;
        case "agent":
            if ($.browser.msie) {
                var version = getInternetExplorerVersion();
                if (version == 9) {
                    $("#tdNoInfomation").width(484);
                    if (!isNoInfo) {
                        $("#tdNoInfomation").css("border-bottom", "0px none");
                    }
                }
                else if (version == 8) {
                    $("#tdNoInfomation").width(484);
                    if (!isNoInfo) {
                        $("#tdNoInfomation").css("border-bottom", "0px none");
                    }
                }
                else if (version == 7) {
                    $("#tdNoInfomation").width(484);
                    if (!isNoInfo) {
                        $("#tdNoInfomation").css("border-bottom", "0px none");
                    }
                }
            }
            else {
                $("#tdNoInfomation").width(484);
                if (!isNoInfo) {
                    $("#tdNoInfomation").css("border-bottom", "0px none");
                }
            }
            break;
    }
    
    switch (currentType) {
        case "master":
            if ($.browser.msie) {
                var version = getInternetExplorerVersion();
                if (version == 9) {
                }
                else if (version == 8) {
                    $("#divFooter1").width(334);
                }
                else if (version == 7) {
                    $("#divFooter1").width(334);
                }
            }
            break;
        case "agent":
            if ($.browser.msie) {
                var version = getInternetExplorerVersion();
                if (version == 9) {
                }
                else if (version == 8) {
                    $("#divFooter1").width(334);
                }
                else if (version == 7) {
                    $("#divFooter1").width(334);
                }
            }
            break;
    }
};

var getScrollbarWidth = function () {
    var scrollbarWidth;
    var div = $('<div style="width:50px;height:50px;overflow:hidden;position:absolute;top:-200px;left:-200px;"><div style="height:100px;"></div></div>');
    $('body').append(div);
    var w1 = $('div', div).innerWidth();
    div.css('overflow-y', 'auto');
    var w2 = $('div', div).innerWidth();
    $(div).remove();
    scrollbarWidth = (w1 - w2);
    return scrollbarWidth;
};

function Refresh()
{
    if ($('#directType').val()!="")
        ViewBalance($('#directType').val());
    else
        ViewBalance('master'); 
}
function ViewBalance(type)
{
    currentType = type;
    BeginLoad();
    var param = 'display=' + type + '&mode=16';
            $.ajax({
                     type: "POST",
                     url: "DirectBalance.ashx",
                     data: param,
                     success: function(rs) {
                        rs = rs.replace("<!--Content-->", "");
                        rs = rs.replace("><!--Rows-->", "");
                        $('#sData').html(rs);
                        
                        FinishLoad();
                        ShowHide(type);
                        
                        fixUI(type);
                     }
                 });                    
}

function fixUI(type) {
    var scrollBarWidth = getScrollbarWidth();
                         
    $("#thHeaderScrollBar").width(scrollBarWidth);
    $("#tdFooterScrollbar").width(scrollBarWidth);
    
    $("#table2").find("div[divBodyScrollBar='true']").width(scrollBarWidth);
    
    var _width = 0;
    $("#table2").find("tbody tr:first td").each(function(index, element){
        _width += $(this).find('div').width();
    });
    $("#table2").width(_width).parent().width(_width + 6);
    $("#table").width(_width + 7);
    
    $("#table2").find("tr:last td").css("border-bottom", "0px none");
    
    $(document).find("div[class='maincontent_box']").width(429);
    
    switch(type)
    {
        case "master":
            if ($.browser.msie) {
                var version = getInternetExplorerVersion();
                if (version == 9) {
                    $("#tdFooterWidth").width(218);//ok
                }
                else if (version == 8) {
                    $("#tdFooterWidth").width(223);//ok
                }
                else if (version == 7) {
                    $("#tdFooterWidth").width(224);//ok
                }
            }
            else {
                $("#tdFooterWidth").width(222);
            }
            break;
        case "agent":
            if ($.browser.msie) {
                var version = getInternetExplorerVersion();
                if (version == 9) {
                    $("#tdFooterWidth").width(265);//ok
                }
                else if (version == 8) {
                    $("#tdFooterWidth").width(269);//ok
                }
                else if (version == 7) {
                    $("#tdFooterWidth").width(269);//ok
                }
            }
            else {
                $("#tdFooterWidth").width(245);
            }
            break;
        case "member":
            if ($.browser.msie) {
                var version = getInternetExplorerVersion();
                if (version == 9) {
                    $("#tdFooterWidth").width(265);//ok
                }
                else if (version == 8) {
                    $("#tdFooterWidth").width(304);
                }
                else if (version == 7) {
                    $("#tdFooterWidth").width(303);
                }
                
                //------------------------
                if (version == 7 || version == 8) {
                    $("#_th2").width(105);
                    $("#table2").parent().width(468);
                    $("#table").width(469);
                    $(document).find("div[class='maincontent_box']").width(464);
                }
            }
            else {
                $("#tdFooterWidth").width(245);
            }
            break;
    }
}

function getInternetExplorerVersion()
// Returns the version of Internet Explorer or a -1
// (indicating the use of another browser).
{
    if (XDomainRequest && window.msPerformance) {
        return 9;
    }
    else {
        var rv = -1; // Return value assumes failure.
        if (navigator.appName == 'Microsoft Internet Explorer')
        {
            var ua = navigator.userAgent;
            var re  = new RegExp("MSIE ([0-9]{1,}[\.0-9]{0,})");
            if (re.exec(ua) != null)
                rv = parseFloat( RegExp.$1 );
        }
        return rv;
    }
}

function BeginLoad()
{
    try{parent.BlockUI();}catch(e){}    
}

function FinishLoad()
{
     parent.jQuery.unblockUI();
}

function ShowHide(type)
{
    if (type=='master')
    {
        //hide link master
        $('#titleDirectMaster').show();
        $('#lkDirectMaster').hide();
        //show other links
        $('#titleDirectAgent').hide();
        $('#lkDirectAgent').show();
        $('#titleDirectMember').hide();
        $('#lkDirectMember').show();
    }
    else if (type == 'agent' )
    {
        //hide link agent
        $('#titleDirectAgent').show();
        $('#lkDirectAgent').hide();
        //show other links
        $('#titleDirectMaster').hide();
        $('#lkDirectMaster').show();
        $('#titleDirectMember').hide();
        $('#lkDirectMember').show();
    }
    else
    {
        type = 'member'
        //hide link member
        $('#titleDirectMember').show();
        $('#lkDirectMember').hide();
        //show other links
        $('#titleDirectMaster').hide();
        $('#lkDirectMaster').show();
        $('#titleDirectAgent').hide();
        $('#lkDirectAgent').show();
    }
    
    $('#directType').val(type);
    $('#btnRefresh').show();
    $('#btnExport').show();
    
}
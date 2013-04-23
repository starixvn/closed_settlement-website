function GenSubMenu(submenu, url) {
    url = trimSpace(url);

    if (accMenu[submenu] != null) {
         var result = '<ul class="smlist">';
        var subid = "";

        var len = accMenu[submenu].length;
        
        for (var i = 0; i < len; i++) {
            var menu = accMenu[submenu][i].split("|");
            subid = submenu + "_" + i;
            var is3thmenu = false;
            is3thmenu = menu[1].indexOf(".aspx") < 0;
            if (!is3thmenu) {
                if (url.indexOf(".aspx") > 0 && i == 0) {
                    result += '<li ><a class="subtopmenucurrent" onmouseover="smenuover(\'' + subid +
                    '\');" onmouseout="smenuout(\'' + subid + '\');" id="' + subid +
                    '"  href="javascript:void(0)" onclick="activeParentMenu(this);Submenu_Click(\'' + menu[1] + '\');">' + 
                        menu[0] + '</a>';
                }
                else {
                     result += '<li ><a onmouseover="smenuover(\'' + subid + '\');" onmouseout="smenuout(\'' + subid + '\');" id="' +
                        subid + '"  href="javascript:void(0)" onclick="activeParentMenu(this);Submenu_Click(\'' + menu[1] + '\');">' + 
                            menu[0] + '</a>';
                }
            }
             else {
                
                if (menu[1].indexOf('sMenu') !=-1)
                    result += '<li ><a onmouseover="smenuover(\'' + subid + '\');" onmouseout="smenuout(\'' + subid + '\');" id="' + 
                        subid + '"  href="javascript:void(0)" >' + menu[0] + '</a>';
                else {
                    result += '<li ><a onmouseover="smenuover(\'' + subid + '\');" onmouseout="smenuout(\'' + subid + '\');" id="' +
                    subid + '"  href="javascript:void(0)"  onclick="activeParentMenu(this);Submenu_Click(\'' + menu[1] + '\');">' + menu[0] + '</a>';
                }
                
            }
            //Contain sub menu
            if (is3thmenu && subaccMenu[menu[1]]!=null)
            {
            var smenu = subaccMenu[menu[1]];
            result += '<div class="ssmenu" id="s' + subid + '" onmouseover="ssmenuover();" onmouseout="ssmenuout(\'' + subid +  '\');" >';
           
            for (j = 0; j < smenu.length; j++) {
                var smenuitem = smenu[j].split("|");
                result += '<a href="javascript:void(0)" onclick="activeParentMenu(document.getElementById(\'' + subid + '\'));Submenu_Click(\'' + smenuitem[1] + '\');">' + smenuitem[0] + '</a><br>';
            }
            result += '</div>';
            }
           
            result += '</li>';
        }
        result += '</ul>';
        return result;
    }
    else
    {
        return "";
    }
    
}

function menuclick(menu, obj, url) {
    if (menu != "") {
        document.getElementById("submenu").innerHTML = GenSubMenu(menu, url);
    }
    else {
        document.getElementById("submenu").innerHTML = "";
    }

    MainMenu_Click(obj, url);

    //reset active link in leftpanel
    if (window.frames.leftPanel.resetCustMenu) {
        window.frames.leftPanel.resetCustMenu();
    }
}



function activeParentMenu(obj) {
    $("#submenu .subtopmenucurrent").removeClass("subtopmenucurrent");
    $("#" + obj.id).addClass("subtopmenucurrent");
    $("#submenu .subtopmenucurrent").css("outline", "0");
    //reset active link in leftpanel
    if (window.frames.leftPanel.resetCustMenu) {
        window.frames.leftPanel.resetCustMenu();
    }
}
var issubmenuover=false;
function resetMenu() {
    document.getElementById("submenu").innerHTML = "";
    $("#mainmenu .current").removeClass("current");
}

//sub menu
function smenuover(oid) {
    
    $("#s" + oid).addClass("ssmenuon");
}
function smenuout(oid) {
    
    setTimeout("hidesmenu('" + oid +"');",500);
    //$("#s" + oid).removeClass("ssmenuon");
}
function hidesmenu(id)
{
     if (!issubmenuover)
     {
        $("#s" + id).removeClass("ssmenuon");
     }
}
function ssmenuover()
{
    issubmenuover=true;
}
function ssmenuout(id)
{
    issubmenuover=false;
    smenuout(id);
 }
 //site search box
 var timeOutId = 0;
 var abortId;
 $(function () {
     //gen search site 
     $("#tbxSiteSearchMenu").keyup(function (e) {
         if ($('#suggestionDiv').is(":visible")) {
             if (e.keyCode == 37 || e.keyCode == 39) return true;
             else if (e.keyCode == 38) {
                 var divSuggestion = $('div.childInforDiv[isHoving="true"]');
                 if (divSuggestion.length == 0) {
                     $('div.childInforDiv:last-child').addClass('smallinfodivhover');
                     $('div.childInforDiv:last-child').attr('isHoving', true);
                 }
                 else {
                     $(divSuggestion[0]).prev().addClass('smallinfodivhover');
                     $(divSuggestion[0]).prev().attr('isHoving', true);
                     $(divSuggestion[0]).removeClass('smallinfodivhover');
                     $(divSuggestion[0]).attr('isHoving', false);
                 }
                 PutTextInSearchBox();
             }
             else if (e.keyCode == 40) {
                 //$('#suggestionDiv')
                 //$('div.childInforDiv')
                 var divSuggestion = $('div.childInforDiv[isHoving="true"]');
                 if (divSuggestion.length == 0) {
                     $('div.childInforDiv:first-child').addClass('smallinfodivhover');
                     $('div.childInforDiv:first-child').attr('isHoving', true);

                 }
                 else {
                     $(divSuggestion[0]).next().addClass('smallinfodivhover');
                     $(divSuggestion[0]).next().attr('isHoving', true);
                     $(divSuggestion[0]).removeClass('smallinfodivhover');
                     $(divSuggestion[0]).attr('isHoving', false);
                 }
                 PutTextInSearchBox();
             }
             else if (e.keyCode == 13) {
                 $('#suggestionDiv').hide();
                 $("#centerFr").attr("src", "QuickLink/Index");
             }
             else {
                 if (!(typeof abortId == 'undefined' || abortId == null)) {
                     abortId.abort();
                 }
                 ShowSuggestionBox();
             }
         }
         else {
             if (e.keyCode == 37 || e.keyCode == 38 || e.keyCode == 39) return true;
             else if (e.keyCode == 13) {
                 $('#suggestionDiv').hide();
                 $("#centerFr").attr("src", "QuickLink/Index");
             }
             else {
                 if (!(typeof abortId == 'undefined' || abortId == null)) {
                     abortId.abort();
                 }
                 ShowSuggestionBox();
             }
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
            $(this).val("Site Name");
        }
        timeOutId = setTimeout(function () {
            $('#suggestionDiv').hide()
        }, 200);
    });

//     $('#sitesearchIcon').hover(
//    function () {
//        $(this).addClass('ui-state-hover');
//    },
//    function () { $(this).removeClass('ui-state-hover'); }
//    );

     $('#sitesearchIcon').click(function () {
         $("#centerFr").attr("src", "QuickLink/Index");
     });
 });
 function ShowSuggestionBox() {
     abortId = $.ajax({
         type: "POST",
         url: "QuickLink/GetSearchMenu",
         contentType: "application/json; charset=utf-8",
         dataType: "json",
         data: '{"keyWord": ' + JSON.stringify($('#tbxSiteSearchMenu').val()) + ',"isSuggestion":"true"}',
         success: function (listMenu) {
             abortId = null;
             if (typeof listMenu != "undefined") {
                 if ($.browser.msie && $.browser.version == 7) {
                     for (var i = 0; i < listMenu.length; i++) {
                         if (listMenu[i].Target != '_blank' && listMenu[i].IsReportMenu == true) {
                             listMenu[i].Url = listMenu[i].Url.substring(3);
                         }
                     }
                 }
                 if (listMenu.length > 0) {
                     //$("#resultTemplate").tmpl(tempArrayMenu).appendTo($("#resultView").empty());
                     $("#suggestionDiv").addClass('sitesearchboxsuggestion');
                     $("#suggestionSiteSearchTemplate").tmpl(listMenu).appendTo($("#suggestionDiv").empty());
                     AddClickEventForResultSearchLink();
                 }
                 else {
                     $("#suggestionDiv").empty();
                     $("#suggestionDiv").removeClass('sitesearchboxsuggestion');
                 }

             }
             $('#suggestionDiv').show();
         }
        , error: function (e) {

        }
     });
 }
 function AddClickEventForResultSearchLink() {
     $("#suggestionDiv a.linkDirectToPage").each(function () {
         var $this = $(this);
         $this.click(function (e) {
             clearTimeout(timeOutId);
             $('#suggestionDiv').hide();
             if ($this.attr('linkTarget') != '_blank') {
                 if ($this.attr('IsReportMenu') == 'true') {
                     var menuUrl = $this.attr("href");
                     window.open("Report/Index.aspx?menuUrl=" + menuUrl, "_blank");
                 }

                 else {
                     $('#item_' + $this.next().val()).trigger('click');
                     $('#centerFr').attr("src", $this.attr("href"));
                 }
             }
             else {
                 if ($.browser.msie && $.browser.version == 7) {
                     //if IE 7 do it
                     var newUrl = $this.attr("href");
                     window.open(newUrl, "_blank");
                 }
                 else {
                     window.open($this.attr("href"), "_blank");
                 }
             }
             e.preventDefault();

         });
     });
     $('div.childInforDiv').hover(
    function () {
        $('div.childInforDiv[isHoving="true"]').removeClass('smallinfodivhover');
        $('div.childInforDiv[isHoving="true"]').attr('isHoving', false);
        $(this).addClass('smallinfodivhover');

    },
    function () {
        $(this).removeClass('smallinfodivhover');
    }
    );
     if ($('#tbxSiteSearchMenu').val() != "") {
         $('.childInforDiv > a, .childInforDiv > span').highlight($('#tbxSiteSearchMenu').val());
     }
     //    .each(function () {
     //        $(this).highlight($('#tbxSiteSearchMenu').val());
     //    });
 }
 function ShowHideSearchBox() {
     $('div.siteseachbox').slideToggle('fast');
 }

 function PutTextInSearchBox() {
     var textInput = $('div.childInforDiv[isHoving="true"] > a').text();
     if ($.browser.msie) {
         textInput = textInput.substr(0, textInput.length - 1);
     }
     else {
         textInput = textInput.substr(1, textInput.length - 2);
     }
     if (textInput.length > 0) {
         $("#tbxSiteSearchMenu").val(textInput);
     }
 }

  

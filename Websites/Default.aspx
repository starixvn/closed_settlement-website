<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Default.aspx.cs" Inherits="_Default" %>
<%@ Import Namespace="Sunplus.Accounting.Core.Configurations" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <title></title>    
    
    <link href="App_Themes/Public/input.css?v=2" rel="stylesheet" type="text/css" />
    <link href="js/jQuery/UI/css/custom-theme/jquery-ui-1.8.10.custom.css" rel="stylesheet" type="text/css" />
    <link href="App_Themes/SettleTheme2012/main.css" rel="stylesheet" type="text/css" />  
    <link href="App_Themes/SettleTheme2012/Menu/menu.css" rel="stylesheet" type="text/css" />
    <style type="text/css">
        .ui-widget-overlay
        {
            background: none #fff;
            opacity: 0.1;
        }
    </style>

    <script src="js/jQuery/jquery.js" type="text/javascript"></script>
    <script src="js/jQuery/UI/jquery-ui-1.8.10.custom.min.js" type="text/javascript"></script>
    <script src="js/jQuery/jquery-jtemplates.js" type="text/javascript"></script>
    <script src="js/jQuery/autocomplete/jquery.autocomplete.js" type="text/javascript"></script>
    <script src="js/jQuery/jquery.blockUI.1.33.js" type="text/javascript"></script>
    <script src="js/dialogbox.js" type="text/javascript"></script>
    <script src="js/BlockUI.js" type="text/javascript"></script>
    <script src="js/mainpage.js?vs=<%=WebSettings.Version%>" type="text/javascript"></script>
    <script src="js/mainmenu.js?vs=<%=WebSettings.Version%>" type="text/javascript"></script>
    <script src="js/jscookies.js?vs=<%=WebSettings.Version%>" type="text/javascript"></script>
    <link href="js/IBCWnd/IBCWnd.css" rel="stylesheet" type="text/css" />
    <script src="js/IBCWnd/IBCWnd.js?vs=<%=WebSettings.Version%>" type="text/javascript"></script>
    <script src="js/showhidepopup.js" type="text/javascript"></script>
    <script src="js/json2.js" type="text/javascript"></script>
    <script src="js/jQuery/jquery.tmpl.js" type="text/javascript"></script>
    <script src="js/jQuery/highlight/jquery.highlight-3.js" type="text/javascript"></script>
</head>
<body>
    <div id="fullsize" class="fullsize">
    </div>
    <div>
        <div class="bannerbox">
            <!--Banner-->
            <div class="top_banner2">
                <div id="button_main">
                    <div class="small-logo">&nbsp;</div>
                    <!--Main Menu-->
                    <div id="mainmenu"></div>

                    <div id="login">
                        <div class="btngroup-group pull-left" id="divUserBox">
                            <a href="#" data-toggle="dropdown" class="btngroup dropdown-toggle"><i class="icon-user">
                            </i><%=accUsername%> <span class="caret"></span></a>
                            <ul class="dropdown-menu">
                                <li><a onclick="closeUserRigthBox(); gotoURL('centerFr','VerifyPassword.aspx')"  href="javascript:void(0);">User Info</a></li>
                                <li class="divider"></li>
                                <li><a href="PreLogin/LogOut.ashx" onclick="closeUserRigthBox();">Log out</a></li>
                            </ul>
                        </div>
                        
                    </div>
                </div>
            </div>
            <!--/Baner-->
        </div>
        
        <div class="contentbox">
            <div class="rightcontent">
                <iframe id="centerFr" name="centerFr" src="_blankpage.htm" scrolling="auto" marginwidth="0"
                    marginheight="0" frameborder="0" vspace="0" hspace="0" style="width: 100%; height: 0px;"  allowTransparency="true">
                </iframe>
            </div>
        </div>
    </div>
    <iframe id="UpdateOnlineStatus" name="UpdateOnlineStatus" width="0" height="0" style="display: none">
    </iframe>

    <script type="text/javascript">
        //menu setting
        setInterval('update_online_status()', 45000);
        var accid = "<%=accUsername%>"; //use for write and read cookie
        $('#divUserBox').click(
            function () {
                $('div.siteseachbox').hide();
                if ($(this).hasClass('open'))
                    $(this).removeClass('open');
                else
                    $(this).addClass('open');
            }
        );

        function closeUserRigthBox() {
            //$('#subUserBox').hide();
            $('#divUserBox div.siteseachbox').removeClass('open');
        }

        $(document).ready(function () {
            var getURLParam = function (strParamName) {
                var strReturn = null;
                var strHref = window.location.href;
                if (strHref.indexOf("?") > -1) {
                    var strQueryString = strHref.substr(strHref.indexOf("?")).toLowerCase();
                    var aQueryString = strQueryString.split("&");
                    for (var iParam = 0; iParam < aQueryString.length; iParam++) {
                        if (aQueryString[iParam].indexOf(strParamName.toLowerCase() + "=") > -1) {
                            var aParam = aQueryString[iParam].split("=");
                            strReturn = aParam[1];
                            break;
                        }
                    }
                }
                if (strReturn == null)
                    return null;
                return unescape(strReturn);
            };
            var uname = getURLParam('uname');
            if (uname != null && uname != '') {
                submitSearch(uname);
            }
        });
    </script>
</body>
</html>

<%@ Page Language="C#" AutoEventWireup="true" CodeFile="NewsArchive.aspx.cs" Inherits="Others_NewsArchive" %>
<%@ Import Namespace="Sunplus.Accounting.Core.Common" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>Accounting - Release log</title>
    <link rel="stylesheet" type="text/css" href="../../App_Themes/Public/common.css" />
    <link rel="stylesheet" type="text/css" href="../../App_Themes/Public/table.css" />    
    <link rel="stylesheet" type="text/css" href="../../App_Themes/Public/image.css" />
    <link rel="stylesheet" type="text/css" href="../../App_Themes/Public/input.css" />
    <link href="../../App_Themes/Public/Message.css" rel="stylesheet" type="text/css" />
    
    <script src="../../js/common.js" type="text/javascript"></script>
    <script src="../../js/jQuery/jquery.js" type="text/javascript" ></script>
    <script src="../../js/jQuery/jquery.history.js" type="text/javascript"></script>
    <script src="../../js/jQuery/jquery.blockUI.1.33.js" type="text/javascript"></script>   
    <script src="../../js/IBCWnd/IBCWnd.js" type="text/javascript" ></script>
    <script src="../../js/jQuery/jquery.json-2.2.min.js" type="text/javascript"></script>        
    
    <!-- Tinymce jQuery -->    
    <script src="../../js/jQuery/jquery.ui.core.min.js" type="text/javascript"></script>
    <script src="../../js/jQuery/jquery.ui.widget.min.js" type="text/javascript"></script>
    <script src="../../js/jQuery/jquery.ui.mouse.min.js" type="text/javascript"></script>
    <script src="../../js/jQuery/jquery.ui.draggable.min.js" type="text/javascript"></script>
    <script src="../../js/tiny_mce/tiny_mce.js" type="text/javascript"></script>
    <script src="../../js/tiny_mce/jquery.tinymce.js" type="text/javascript"></script>
    <script src="../../js/tiny_mce/ajax.upload.js" type="text/javascript"></script>

    <!-- Calendar -->
    <script src="../../js/Calendar2/js/jscal2.js" type="text/javascript"></script>
    <script src="../../js/Calendar2/js/lang/en.js" type="text/javascript"></script>
    <link href="../../js/Calendar2/css/jscal2.css" type="text/css" rel="stylesheet" />
    <link href="../../js/Calendar2/css/border-radius.css" type="text/css" rel="stylesheet" />
    <link href="../../js/Calendar2/css/blue/blue.css" type="text/css" rel="stylesheet" />    
    <!-- End of Calendar -->    
    
    <script src="../../js/jQuery/Template/jquery.tmpl.js" type="text/javascript"></script>
    <script src="../../js/jQuery/jquery.jcache.js" type="text/javascript"></script>
    <script src="js/Template.js?v=<%=AccCommon.Version() %>" type="text/javascript"></script>
    <script src="js/News_V2.js?v=<%=AccCommon.Version() %>" type="text/javascript"></script>
    <link href="css/News.css" rel="Stylesheet" type="text/css" />
</head>
<body>
    <script type="text/javascript">
        // --- Call pageload to load data by ajax --- //
        // --- Initialize history --- //
        // --- Store url in history --- //
        jQuery(document).ready(function () {
            $("#whatnews_content").whatNews({ AppID: "<%=AppID %>" });
        });
    </script>
    <div class="main">
        <div class="maincontent">
            <table cellspacing="0px" cellpadding="0px" border="0" class="maincontent_header">
                <tbody>
                    <tr>
                        <td class="maincontent_headerL">
                        </td>
                        <td class="maincontent_headerC">
                            Release Log
                        </td>
                        <td class="maincontent_headerR">
                        </td>
                        <td>&nbsp;</td>
                        <td>
                            <a id="goBack" href="#" class="" title="Release List"><img alt="Release List" src="images/news-back.png" border="0" height="20" /></a>
                        </td>
                    </tr>
                </tbody>
            </table>
            <div class="maincontent_news">
                <div id="news">
                    <div style="float:left;width:935px;">
                        
                        <div id="news_addbtn" class="news_addbtn role">
                            <a style="display:none;" href="NewsArchive.aspx#1.0.a" class="" title="Add Release"><img alt="Add Release Log" src="images/news-add.png" style="width:20px" /></a>
                        </div>
                    </div>
                    <div id="whatnews_content" AppID="<%=AppID %>">
                    </div> 
                </div>           
            </div>
        </div>
    </div>      
    <div id="divUploadBar" class="uploadbox" style="">
            <div class="uploadbox-header" style="" id="bardrag">
                <h2 class="boxh2" style="">Upload Image<span class="boxclose" style=""><a onclick="whatNews.cancelUploadImage();" style="display:inline" href="javascript:;"><img alt="Close" src="images/icon-close1.png" border="0" width="20" style="display:inline;" /></a></span></h2>
            </div>    
            <ul style="" class="" id="uploadbox-tab">
                <li><a href="javascript:;" class="tab-item current" rel="url">From URL</a></li>
                <li><a href="javascript:;" class="tab-item" rel="choose">Upload Image</a></li>
            </ul>
            <div class="chooseimg" style="display:;" id="tab-url">	        
                <span style="font-weight:bold;">Image location</span><br />
                <input type="input" style="width: 300px; padding: 2px; height: 15px;" id="txt_url"  value=""/>
            </div>
            <div class="chooseimg" style="display:none;" id="tab-choose">	        
                <span style="font-weight:bold;">Choosing image</span><br />
                <input style="border: 1px solid #818181;" type="file" id="txtfile" name="txtfile" size="40" class="inputfile"/><br />
                <small>(Max size: 2M. File type: jpg, jpeg, gif, png, bmp)</small>         
            </div>
            <div style="float:left; width:320px; padding: 10px 0 0 10px;" id="div-img-thumb">
                <span style="font-weight:bold;">Is thumbnail&nbsp;<input type="checkbox" id="is_thumbnail" value=""/></span>
            </div>
            <div style="float:left; width:320px; padding: 10px 0 0 10px; display:none;" id="upload_message">
                <span style="color:red;"></span>
            </div>
            <div class="uploadbuttonbar" style="">             
                <input style="height: 15px; width: 50px;" id="btnUpload" class="btn" value="Upload" />
                <input style="height: 15px; width: 50px;" onclick="whatNews.cancelUploadImage();" class="btn" value="Cancel" />         
            </div> 
           <br style="clear:both" />
    </div>
    <input id="recipient" type="hidden" value="<%=Recipient %>" />
</body>
</html>
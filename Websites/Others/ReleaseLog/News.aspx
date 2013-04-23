<%@ Page Language="C#" AutoEventWireup="true" CodeFile="News.aspx.cs" Inherits="Others_News" %>
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
    <link rel="stylesheet" type="text/css" href="../../App_Themes/Public/Message.css"  />
    
    <script src="../../js/common.js" type="text/javascript"></script>
    <script src="../../js/jQuery/jquery.js" type="text/javascript" ></script>
    <script src="../../js/jQuery/jquery.history.js" type="text/javascript"></script>
    <script src="../../js/jQuery/jquery.blockUI.1.33.js" type="text/javascript"></script>       

    <!-- jQuery Pager -->    
    <script src="../../js/jQuery/Pager/jpagergopage.js" type="text/javascript"></script>
    <link rel="stylesheet" type="text/css" href="../../js/jQuery/Pager/pagination.css" />

    <script src="../../js/jQuery/Template/jquery.tmpl.js" type="text/javascript"></script>
    <script src="../../js/jQuery/jquery.jcache.js" type="text/javascript"></script>
    <script src="js/News_V2.js?v=<%=AccCommon.Version() %>" type="text/javascript"></script>
    <script src="js/Template.js?v=<%=AccCommon.Version() %>" type="text/javascript"></script>
    <link rel="stylesheet" type="text/css" href="css/News.css"/>
</head>
<body>
    <script type="text/javascript">
        // --- Call pageload to load data by ajax --- //
        // --- Initialize history --- //
        // --- Store url in history --- //
        var releaseFeature = <%=releaseFeature %>;
        jQuery(document).ready(function () {
            $("#whatnews_content").whatNews({ AppID: "<%=AppID %>" });
            whatNews.displayFeature(releaseFeature);
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
                    </tr>
                </tbody>
            </table>
            <div class="maincontent_news">
                <% if (viewWhatNews) {  %>
                <% if (releaseFeature != null)
                   {  %>
                <div id="news-feature" style="float: left">       
                </div>
                <% } %>
                <div id="whatnews_header" class="whatnews_header">
                    <div class="cap" style="float:left;">Latest Releases</div>
                    <div class="role" style="float: right;"><a href="NewsArchive.aspx#1.0.a" class="" title="Add Release"><strong>Add Release</strong>&nbsp;<img alt="Add Release Log" src="images/news-add.png"/></a></div>
                </div>            
                <div id="news">
                    <div id="whatnews_content" class="whatnews_content" AppID="<%=AppID %>" style="border:1px solid #BBCCED;">
                    </div>            
                    <br clear="all" />
                    <div class="pagination" style="min-height: 20px;">
                    <div id="pagination"></div>     
                    </div>
                </div>
                <% } %>
            </div>
        </div>
    </div>      
</body>
</html>

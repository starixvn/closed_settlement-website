<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Result.aspx.cs" Inherits="Settle_Result" Title="Result"%>
<%@ Import Namespace="Sunplus.Accounting.Core.Common" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Result</title>
    <link href="../App_Themes/Public/common.css" rel="stylesheet" type="text/css" />
    <link href="../App_Themes/Public/table.css" rel="stylesheet" type="text/css" />
    <link href="../App_Themes/Public/input.css" rel="stylesheet" type="text/css" />
    <link href="../App_Themes/Settle/Settle.css?ver=1" rel="stylesheet" type="text/css" />
    <link href="../App_Themes/Public/Message.css" rel="stylesheet" type="text/css" />
    <link href="../js/jQuery/UI/css/custom-theme/jquery-ui-1.8.10.custom.css" rel="stylesheet"
        type="text/css" />
    <script src="../js/jQuery/jquery.js" type="text/javascript"></script>
    <script src="../js/jQuery/UI/jquery-ui-1.8.10.custom.min.js" type="text/javascript"></script>
    <script type="text/javascript" src="../js/prototype.js"></script>    
    <script src="../js/jQuery/jquery.blockUI.1.33.js" type="text/javascript"></script>
    <script src="../js/BlockUI.js" type="text/javascript"></script>  
    <script type="text/javascript" src="../js/common.js?vers=<%=AccCommon.Version() %>"></script>
    <script type="text/javascript" src="../js/Result.js?vers=1"></script>
    <link href="../js/Window/dhtmlwindow/dhtmlwindow.css" rel="stylesheet" type="text/css" />
    <script src="../js/dialogbox.js" type="text/javascript"></script>
    <script type="text/javascript">
        var rootpath = '../';
        function ShowGoalRecord(url, title, width, height) {
            url = url.replace('../', '');
            parent.OpenPopup(url, title, 350, 300, true, false, window, 1000, null, null);
        }

    </script>
</head>
<body>
    <div class="main">
        <div class="maincontent">

            <div id="tabmain_conts">
                <div id="content_tab">
                    <ul>
                        <li id="current"><a href="#"><span>
                            <asp:Literal ID="sTitle" runat="server"></asp:Literal></span></a></li>
                    </ul>
                </div>
            </div>

            <div class="maincontent_box">
                <div id="searchbox">
                    <asp:Literal ID="sSearch" runat="server"></asp:Literal>
                </div>
            </div>
            <div class="bg_table">
                  <div id="_sContent"><asp:Literal ID="sContent" runat="server"></asp:Literal></div>          
            </div>
        </div>
    </div>
</body>
</html>
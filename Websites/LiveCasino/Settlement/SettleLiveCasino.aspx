<%@ Page Language="C#" AutoEventWireup="true" CodeFile="SettleLiveCasino.aspx.cs"
    Inherits="LiveCasino_Settlement_SettleLiveCasino" %>

<%@ Import Namespace="Sunplus.Accounting.Core.Common" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Settlement Live Casino</title>
    <link href="../../App_Themes/Public/common.css" rel="stylesheet" type="text/css" />
    <link href="../../App_Themes/Report/reportgrid.css" rel="stylesheet" type="text/css" />
    <link href="../../App_Themes/Public/input.css" rel="stylesheet" type="text/css" />
    <link href="../../js/jQuery/UI/css/custom-theme/jquery-ui-1.8.10.custom.css" rel="stylesheet"
        type="text/css" />
    <link href="../../js/Window/dhtmlwindow/dhtmlwindow.css" rel="stylesheet" type="text/css" />
    <script src="../../js/jQuery/jquery-latest.js" type="text/javascript"></script>
    <script src="../../js/jQuery/gridscroller.js" type="text/javascript"></script>
    <script src="../../js/jQuery/UI/jquery-ui-1.8.10.custom.min.js" type="text/javascript"></script>
    <script src="../../js/jQuery/Template/jquery.tmpl.js" type="text/javascript"></script>
    <script src="../../js/jQuery/jquery.blockUI.1.33.js" type="text/javascript"></script>
    <script src="../../js/BlockUI.js" type="text/javascript"></script>  
    <script src="../../js/dialogbox.js" type="text/javascript"></script>
    <script src="script/settle-live-casino.js?vers=2" type="text/javascript"></script>
    <script src="../../js/jQuery/jquery.queue.js" type="text/javascript"></script>
    <script src="../../js/Window/dhtmlwindow/dhtmlwindow.js" type="text/javascript"></script>
    <style type="text/css">
        .title1
        {
            text-align: center;
            color: White;
            font-weight: bold;
            background-color: #587281 !important;
        }
        .title2
        {
            text-align: center;
            color: Black;
            font-weight: bold;
            background-color: #B5C4D0 !important;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
    <input id="hiddenServerDate" type="hidden" value="<%= DateTime.Now.ToShortDateString() %>" />
    <input id="hiddenServerDatePrevious" type="hidden" value="<%= DateTime.Now.ToShortDateString() %>" />
    </form>
    <div class="main">
        <div class="maincontent">
            <div id="tabmain_conts">
                <div id="content_tab">
                    <ul>
                        <li id="current"><a href="#"><span>
                            Live Casino</span></a></li>
                    </ul>
                </div>
            </div>

            <div class="maincontent_box">
                <table border="0">
                    <tbody>
                        <tr>
                            <td>
                                <div class="b">
                                    From:</div>
                            </td>
                            <td>
                                <input id="fdate" type="text" class="txtdate" readonly="readonly" />
                            </td>
                            <td>
                                <div class="b">
                                    To:</div>
                            </td>
                            <td>
                                <input id="tdate" type="text" class="txtdate" readonly="readonly" />
                            </td>
                            <td>
                                <div class="b">
                                    Leagues:</div>
                            </td>
                            <td>
                                <div>
                                    <select id="selLeagues">
                                    </select>
                                </div>
                            </td>
                            <td>
                                <div class="b">
                                    Status:</div>
                            </td>
                            <td>
                                <div>
                                    <select id="selStatus">
                                        <option value="0">All</option>
                                        <option value="1">Running</option>
                                        <option value="2">Completed</option>
                                        <option value="3">Refund</option>
                                        <option value="5">Closed</option>
                                    </select>
                                </div>
                            </td>
                            <td>
                                <input id="btnSubmit" type="button" value="Submit" class="btn" />
                            </td>
                        </tr>
                    </tbody>
                </table>
                
            </div>
            <iframe id="iframeData" frameborder="0" noresize="noresize" scrolling="auto" width="100%" height="580px"></iframe>
        </div>
    </div>
    <script type="text/javascript">
    <%= Leagues %>
    </script>
</body>
</html>

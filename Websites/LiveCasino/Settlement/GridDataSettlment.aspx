<%@ Page Language="C#" AutoEventWireup="true" CodeFile="GridDataSettlment.aspx.cs"
    Inherits="LiveCasino_Settlement_GridDataSettlment" %>
    <%@ Import Namespace="Sunplus.Accounting.Core.Common" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <link href="../../App_Themes/Public/common.css" rel="stylesheet" type="text/css" />
    <link href="../../App_Themes/Report/reportgrid.css" rel="stylesheet" type="text/css" />
    <link href="../../App_Themes/Public/input.css" rel="stylesheet" type="text/css" />
    <link href="../../js/jQuery/UI/css/custom-theme/jquery-ui-1.8.10.custom.css" rel="stylesheet"
        type="text/css" />
    <link href="../../App_Themes/Settle/Settle.css" rel="stylesheet" type="text/css" />
    <link href="../../js/Window/dhtmlwindow/dhtmlwindow.css" rel="stylesheet" type="text/css" />
    <script src="../../js/jQuery/jquery-latest.js" type="text/javascript"></script>
    <script src="../../js/jQuery/gridscroller.js" type="text/javascript"></script>
    <script src="../../js/jQuery/UI/jquery-ui-1.8.10.custom.min.js" type="text/javascript"></script>
    <script src="../../js/Window/dhtmlwindow/dhtmlwindow.js" type="text/javascript"></script>
    
    <script src="../../js/Settlement.js?vers=1" type="text/javascript"></script>
    <script src="script/gridata-settlement.js?vers=1" type="text/javascript"></script>
</head>

<body>
    <div style="height: 570px; overflow: hidden;">
        <div class="maincontent">
            <div id="divContainer" style="margin-top: 4px">
                <div id="divMainGrid" class="content-warper">
                    <div class="content-header clearfix">
                        <div class="header-scroll">
                            <table class="tbl-header-scroll">
                                <tr>
                                    <td id="tdtitle1" colspan="8" class="title1" style="text-align:center">
                                        <%= Title %>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="border-bottom: none 0px;" class="title2">
                                        <div class="col140 c">
                                            CODE
                                        </div>
                                    </td>
                                    <td style="border-bottom: none 0px;" class="title2">
                                        <div class="col140 c">
                                            KICK-OFF TIME
                                        </div>
                                    </td>
                                    <td style="border-bottom: none 0px;" class="title2">
                                        <div class="col200 c">
                                            EVENT
                                        </div>
                                    </td>
                                    <td style="border-bottom: none 0px;" class="title2">
                                        <div class="col80 c">
                                            FINAL SCORE
                                        </div>
                                    </td>
                                    <td style="border-bottom: none 0px;" class="title2">
                                        <div class="col120 c">
                                            RESULT
                                        </div>
                                    </td>
                                    <td style="border-bottom: none 0px;" class="title2">
                                        <div class="col100 c">
                                            EVENT STATUS
                                        </div>
                                    </td>
                                    <td style="border-bottom: none 0px;" class="title2">
                                        <div class="col80 c">
                                            CALCULATION
                                        </div>
                                    </td>
                                    <td style="border-bottom: none 0px;" class="title2">
                                        <div class="col80 c">
                                            REFUND
                                        </div>
                                    </td>
                                </tr>
                            </table>
                        </div>
                    </div>
                    <div class="content-body clearfix">
                        <div class="body-scroll">
                            <table class="tbl-body-scroll">
                                <tbody id="tobdyMainGrid">
                                    <%= HTML %>
                                </tbody>
                            </table>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
</body>
</html>

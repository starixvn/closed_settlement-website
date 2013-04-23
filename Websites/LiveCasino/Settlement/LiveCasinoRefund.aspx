<%@ Page Language="C#" AutoEventWireup="true" CodeFile="LiveCasinoRefund.aspx.cs"
    Inherits="LiveCasino_Settlement_LiveCasinoRefund" %>

<%@ Import Namespace="Sunplus.Accounting.Core.Common" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Refund Live Casino</title>
    <link href="../../App_Themes/Public/common.css" rel="stylesheet" type="text/css" />
    <link href="../../App_Themes/Report/reportgrid.css" rel="stylesheet" type="text/css" />
    <link href="../../App_Themes/Public/input.css" rel="stylesheet" type="text/css" />
    <link href="../../js/jQuery/UI/css/custom-theme/jquery-ui-1.8.10.custom.css" rel="stylesheet"
        type="text/css" />
    <script src="../../js/jQuery/jquery-latest.js" type="text/javascript"></script>
    <script src="../../js/jQuery/gridscroller.js" type="text/javascript"></script>
    <script src="../../js/jQuery/UI/jquery-ui-1.8.10.custom.min.js" type="text/javascript"></script>
    <script src="script/refund.js?vers=<%=AccCommon.Version() %>" type="text/javascript"></script>
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
    <form runat="server">
        <input type="hidden" id="hiddenMatchID" value="<%= MatchID %>" />
        <input type="hidden" value="<%= AdminName %>" id="hiddenAdminName" />
        <input type="hidden" value="<%= AdminID %>" id="hiddenAdminID" />
        <input type="hidden" value="<%= ShowDate %>" id="hiddenShowDate" />
    </form>
    <div id="msg" style="width:628px;color:red;border:1px solid red; padding:2px 0 2px 2px; display:none; margin-bottom:2px;"></div>
    <div class="content-header clearfix">
        <div class="header-scroll">
            <table class="tbl-header-scroll">
                <tr>
                    <td id="tdtitle1" colspan="11" class="title1" style="padding-top:4px; padding-bottom:4px;border-left:1px solid #7C98A4">
                        <%= LeagueName %>
                    </td>
                </tr>
                <tr>
                    <td class="title2" style="border-left:1px solid #7C98A4">
                        <div class="col140 c" style="padding-top:2px; padding-bottom:2px">
                            CODE
                        </div>
                    </td>
                    <td class="title2">
                        <div class="col140 c">
                            KICK-OFF TIME
                        </div>
                    </td>
                    <td class="title2">
                        <div class="col240 c">
                            EVENT
                        </div>
                    </td>
                    <td class="title2">
                        <div class="col100 c">
                            Result
                        </div>
                    </td>
                </tr>
                <tr>
                    <td style="border-left:1px solid #7C98A4">
                        <div class="col140 c" style="font-weight: normal;padding-top:2px; padding-bottom:2px">
                            <%= Code %>
                        </div>
                    </td>
                    <td>
                        <div class="col140 c" style="font-weight: normal">
                            <%= KickOffTime %>
                        </div>
                    </td>
                    <td>
                        <div class="col240 l" style="font-weight: bold">
                            <%= Event %>
                        </div>
                    </td>
                    <td>
                        <div class="col100 c" style="font-weight: normal">
                            <%= Result %>
                        </div>
                    </td>
                </tr>
                </tbody>
            </table>
            <table>
                <tr>
                    <td>
                        <div class="c" style="font-weight: normal; width:600px;margin-top:10px">
                            <input type="button" class="btn" value="Refund All" onclick="javascript:refund.submit();" />
                            <input type="button" class="btn" value="Cancel" onclick="javascript:refund.closePopup();" />
                        </div>
                    </td>
                </tr>
            </table>
        </div>
    </div>
</body>
</html>

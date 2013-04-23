<%@ Page Language="C#" AutoEventWireup="true" CodeFile="LiveCasinoReCalc.aspx.cs"
    Inherits="LiveCasino_Settlement_LiveCasinoReCalc" %>

<%@ Import Namespace="Sunplus.Accounting.Core.Common" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Re-Calc Live Casino</title>
    <link href="../../App_Themes/Public/common.css" rel="stylesheet" type="text/css" />
    <link href="../../App_Themes/Report/reportgrid.css" rel="stylesheet" type="text/css" />
    <link href="../../App_Themes/Public/input.css" rel="stylesheet" type="text/css" />
    <link href="../../js/jQuery/UI/css/custom-theme/jquery-ui-1.8.10.custom.css" rel="stylesheet"
        type="text/css" />
    <script src="../../js/jQuery/jquery-latest.js" type="text/javascript"></script>
    <script src="../../js/jQuery/gridscroller.js" type="text/javascript"></script>
    <script src="../../js/jQuery/UI/jquery-ui-1.8.10.custom.min.js" type="text/javascript"></script>
    <script src="../../js/jQuery/Number/jquery.caret.1.02.js" type="text/javascript"></script>
    <script src="script/re-calc.js?vers=<%=AccCommon.Version() %>" type="text/javascript"></script>
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
        .rowWhite{}
        .rowWhite td
        {
            background-color:White;
        }
    </style>
</head>
<body>
    <form runat="server">
        <input type="hidden" value="<%= MatchID %>" id="hiddenMatchID" />
        <input type="hidden" value="<%= BetType %>" id="hiddenBetType" />
        <input type="hidden" value="<%= AdminID %>" id="hiddenAdminID" />
        <input type="hidden" value="<%= AdminName %>" id="hiddenAdminName" />
        <input type="hidden" value="<%= LeagueName %>" id="hiddenLeagueName" />
        <input type="hidden" value="<%= ShowDate %>" id="hiddenShowDate" />
    </form>
    <div id="msg" style="width:97%;color:red;border:1px solid red; padding:2px 0 2px 2px; display:none; margin-bottom:2px; margin-left:10px"></div>
    <div class="content-header clearfix">
        <div class="header-scroll" style="margin-left:10px">
            <table class="tbl-header-scroll  hidden" id="tbBaccarat">
                <tbody>
                    <tr>
                        <td id="tdtitle1" colspan="11" class="title1" style="padding-top: 4px; padding-bottom: 4px;
                            border-left: 1px solid #7C98A4">
                            <%= LeagueName %>
                        </td>
                    </tr>
                    <tr>
                        <td class="title2" style="border-left: 1px solid #7C98A4">
                            <div class="col140 c" style="padding-top: 2px; padding-bottom: 2px">
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
                            <div class="col40 c">
                                1
                            </div>
                        </td>
                        <td class="title2">
                            <div class="col40 c">
                                2
                            </div>
                        </td>
                        <td class="title2">
                            <div class="col40 c">
                                3
                            </div>
                        </td>
                        <td class="title2">
                            <div class="col40 c">
                                4
                            </div>
                        </td>
                        <td class="title2">
                            <div class="col40 c">
                                5
                            </div>
                        </td>
                        <td class="title2">
                            <div class="col40 c">
                                6
                            </div>
                        </td>
                        <td class="title2">
                            <div class="col100 c">
                                Final Score
                            </div>
                        </td>
                        <td class="title2">
                            <div class="col100 c">
                                Result
                            </div>
                        </td>
                    </tr>
                    <tr class="rowWhite">
                        <td style="border-left: 1px solid #7C98A4">
                            <div class="col140 c" style="font-weight: normal; padding-top: 2px; padding-bottom: 2px">
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
                            <div class="col40 c">
                                <input id="tbx1" type="text" style="width: 38px; background-color: #FEF8B6; text-transform: uppercase"
                                    maxlength="2" value="<%= P1 %>" />
                            </div>
                        </td>
                        <td>
                            <div class="col40 c">
                                <input id="tbx2" type="text" style="width: 38px; background-color: #CBE1F1; text-transform: uppercase"
                                    maxlength="2" value="<%= B2 %>" />
                            </div>
                        </td>
                        <td>
                            <div class="col40 c">
                                <input id="tbx3" type="text" style="width: 38px; background-color: #FEF8B6; text-transform: uppercase"
                                    maxlength="2" value="<%= P3 %>" />
                            </div>
                        </td>
                        <td>
                            <div class="col40 c">
                                <input id="tbx4" type="text" style="width: 38px; background-color: #CBE1F1; text-transform: uppercase"
                                    maxlength="2" value="<%= B4 %>" />
                            </div>
                        </td>
                        <td>
                            <div class="col40 c">
                                <input id="tbx5" type="text" style="width: 38px; background-color: #FEF8B6; text-transform: uppercase"
                                    maxlength="2" value="<%= P5 %>" />
                            </div>
                        </td>
                        <td>
                            <div class="col40 c">
                                <input id="tbx6" type="text" style="width: 38px; background-color: #CBE1F1; text-transform: uppercase"
                                    maxlength="2" value="<%= B6 %>" />
                            </div>
                        </td>
                        <td>
                            <div class="col100 c" style="font-weight: normal">
                                <%= FinalScore %>
                            </div>
                        </td>
                        <td>
                            <div class="col100 c" style="font-weight: normal">
                                <%= Result %>
                            </div>
                        </td>
                    </tr>
                    <tr class="rowWhite">
                        <td style="border-left: 1px solid #7C98A4">
                            <div class="c" style="font-weight: normal">
                                Full Time
                            </div>
                        </td>
                        <td>
                            <div class="c" style="font-weight: normal">
                                SETTLED
                            </div>
                        </td>
                        <td colspan="9">
                            <input id="btnSubmit" type="button" class="btn" value="Calculate All" />
                        </td>
                    </tr>
                </tbody>
            </table>

            <table class="tbl-header-scroll hidden" id="tbRoutle">
                <tbody>
                    <tr>
                        <td id="tdRoutleTitle" colspan="5" class="title1" style="padding-top: 4px; padding-bottom: 4px;
                            border-left: 1px solid #7C98A4">
                            <%= LeagueName %>
                        </td>
                    </tr>
                    <tr>
                        <td class="title2" style="border-left: 1px solid #7C98A4">
                            <div class="col140 c" style="padding-top: 2px; padding-bottom: 2px">
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
                                Final Score
                            </div>
                        </td>
                        <td class="title2">
                            <div class="col100 c">
                                Result
                            </div>
                        </td>                        
                    </tr>
                    <tr class="rowWhite">
                        <td style="border-left: 1px solid #7C98A4">
                            <div class="col140 c" style="font-weight: normal; padding-top: 2px; padding-bottom: 2px">
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
                                <input id="txtRoutleFinalScore" type="text" style="width: 38px; background-color: #FEF8B6; text-transform: uppercase; text-align:center;"
                                    maxlength="2" value="<%= FinalScore %>" />
                            </div>
                        </td>
                        <td>
                            <div class="col100 c" style="font-weight: normal">
                                <%= Result %>
                            </div>
                        </td>
                    </tr>
                    <tr class="rowWhite">
                        <td style="border-left: 1px solid #7C98A4">
                            <div class="c" style="font-weight: normal">
                                Full Time
                            </div>
                        </td>
                        <td>
                            <div class="c" style="font-weight: normal">
                                SETTLED
                            </div>
                        </td>
                        <td colspan="9">
                            <input id="btnRoutleCalc" type="button" class="btn" value="Calculate All" />
                        </td>
                    </tr>
                </tbody>
            </table>
        </div>
    </div>
</body>
</html>

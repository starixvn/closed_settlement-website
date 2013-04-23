<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ReCalcNumberGame.aspx.cs" Inherits="Settle_ReCalcNumberGame" %>
<%@ Import Namespace="Sunplus.Accounting.Core.Common" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <link href="../App_Themes/Public/common.css" rel="stylesheet" type="text/css" />
    <link href="../App_Themes/Public/table.css" rel="stylesheet" type="text/css" />
    <link href="../App_Themes/Public/input.css" rel="stylesheet" type="text/css" />
    <link href="../App_Themes/Settle/Settle.css" rel="stylesheet" type="text/css" />
    <link href="../js/Controls/StyledCheckbox/form[1].css" rel="stylesheet" type="text/css" />
    <link rel="stylesheet" type="text/css" href="../js/IBCWnd/IBCWnd.css" />
    <link href="../App_Themes/Public/Message.css" rel="stylesheet" type="text/css" />
    <link href="../js/jQuery/Bubblepopup/css/jquery.bubblepopup.v2.3.1.css" rel="stylesheet" type="text/css" />
    <link href="../js/jQuery/UI/css/custom-theme/jquery-ui-1.8.10.custom.css" rel="stylesheet"
        type="text/css" />
    <link href="../js/Window/dhtmlwindow/dhtmlwindow.css" rel="stylesheet" type="text/css" />

    <script src="../js/jQuery/jquery-latest.js" type="text/javascript"></script>

    <script src="../js/jQuery/Number/jquery.caret.1.02.js" type="text/javascript"></script>
    <script src="../js/jQuery/Number/jquery.formattyping.js" type="text/javascript"></script>

    <script src="Js/re-calc-numbergame.js?vers=5" type="text/javascript"></script>

    <style type="text/css">
        .recal_td_title
        {
            height:20px;
            text-align:center;
            vertical-align:middle;
            background-color:#587281;
            border:1px solid #899490;
            color:White;
            font-weight:bold;
        }
        .recal_td_header
        {
            height:24px;
            text-align:center;
            vertical-align:middle;
            width: 120px;
            font-weight:bold;
            border:1px solid #899490;
            background-color:#B6C4D1;
        }
        .recal_td
        {
            height:28px;
            text-align:center;
            vertical-align:middle;
            font-weight:bold;
            border:1px solid #899490;
        }
        
        .recal_textbox
        {
            width:100px;
            height:18px;
            text-align:center;
            vertical-align:middle;
            margin-top:2px;
            margin-bottom:2px;
        }
    </style>
</head>
<body>
    <div id="popup_recalc_numbergame">
        <div style="padding-left: 4px; border:1px solid red ; color:red; display:none; padding-top:4px;; padding-bottom:4px; margin-top:2px" msg="true"></div>
        <div style="text-align:center; font-weight:bold; vertical-align:middle; height:20px; padding-top:8px">Number Game</div>
        <div>
            <table cellpadding="0" cellspacing="0" style="border-collapse:collapse">
                <tr>
                    <td class="recal_td_title" title="true" colspan="3" style="">
                    </td>
                </tr>
                <tr>
                    <td class="recal_td_header">
                        Sequence
                    </td>
                    <td class="recal_td_header">
                        Result
                    </td>
                    <td class="recal_td_header">
                        Is N/A
                    </td>
                </tr>
                <tr>
                    <td class="recal_td">
                        1
                    </td>
                    <td class="recal_td">
                        <input id="tbxResultReCal1" type="text" class="recal_textbox" />
                        <span style="display:none">N/A</span>
                    </td>
                    <td class="recal_td">
                        <input id="cbxSetNaSeq1" type="checkbox" onclick="javascript:setNASequence1();" />
                    </td>
                </tr>
                <tr>
                    <td class="recal_td">
                        2
                    </td>
                    <td class="recal_td">
                        <input id="tbxResultReCal2" type="text" class="recal_textbox" />
                        <span style="display:none">N/A</span>
                    </td>
                    <td class="recal_td">
                        <input id="cbxSetNaSeq2" type="checkbox" onclick="javascript:setNASequence2();" />
                    </td>
                </tr>
                <tr>
                    <td class="recal_td">
                        3
                    </td>
                    <td class="recal_td">
                        <input id="tbxResultReCal3" type="text" class="recal_textbox" />
                        <span style="display:none">N/A</span>
                    </td>
                    <td class="recal_td">
                        <input id="cbxSetNaSeq3" type="checkbox" onclick="javascript:setNASequence3();" />
                    </td>
                </tr>
                <tr>
                    <td colspan="3" align="center">
                        <input id="btnSubmit" type="button" value="Submit" class="btn" style="margin-top:10px" />
                    </td>
                </tr>
            </table>
        </div>
    </div>
    <script type="text/javascript">
        <%= Balls %>
        var matchid = <%= matchid %>;
        var matchcode = '<%= matchcode %>';
        var kickoffdate = '<%= kickoffdate %>';
    </script>
</body>
    
</html>

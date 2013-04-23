<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Calculation.aspx.cs" Inherits="Settle_Calculation" Theme="" %>
<%@ Import Namespace="Sunplus.Accounting.Core.Common" %>
 
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Calculation</title>      
    <link href="../App_Themes/Public/common.css" rel="stylesheet" type="text/css" />
    <link href="../App_Themes/Public/table.css" rel="stylesheet" type="text/css" />
    <link href="../App_Themes/Public/input.css" rel="stylesheet" type="text/css" />
    <link href="../App_Themes/Settle/Settle.css" rel="stylesheet" type="text/css" />
    <link href="../js/IBCWnd/IBCWnd.css" rel="stylesheet" type="text/css" />
    <link href="../App_Themes/Public/Message.css" rel="stylesheet" type="text/css" />
    <link href="../js/jQuery/UI/css/custom-theme/jquery-ui-1.8.10.custom.css?ver=<%=Sunplus.Accounting.Core.Configurations.WebSettings.Version%>" rel="stylesheet"
        type="text/css" />

    <script type="text/javascript" src="../js/prototype.js"></script> 
    <script type="text/javascript" src="../js/common.js?vers=<%=Sunplus.Accounting.Core.Configurations.WebSettings.Version%>"></script>
    <script src="../js/jQuery/jquery-1.7.js" type="text/javascript"></script>
    <script src="../js/IBCWnd/IBCWnd.js" type="text/javascript"></script>
    <script type="text/javascript" src="../js/Calculation.js?vers=<%=Sunplus.Accounting.Core.Configurations.WebSettings.Version%>"></script>
    <script src="../js/Settlement.js" type="text/javascript"></script>
    <script src="../js/Common/ControlUI.js" type="text/javascript"></script>
    <script src="../js/jQuery/UI/jquery-ui-1.8.10.custom.min.js?ver=<%=Sunplus.Accounting.Core.Configurations.WebSettings.Version%>" type="text/javascript"></script>
    <script src="../js/dialogbox.js" type="text/javascript"></script>
    <script src="../js/jQuery/jquery.blockUI.1.33.js" type="text/javascript"></script>
    <script src="../js/BlockUI.js" type="text/javascript"></script>
    <script src="../js/jQuery/Number/jquery.caret.1.02.js" type="text/javascript"></script>
    <script src="../js/jQuery/Number/jquery.formattyping.js" type="text/javascript"></script>
    <script src="../js/Settle/ScoreConfirm.js?vers=<%=Sunplus.Accounting.Core.Configurations.WebSettings.Version%>" type="text/javascript"></script>
    <script src="Js/ApplyHedging.js" type="text/javascript"></script>
    <script src="Js/GoalRecords.js?ver=<%=Sunplus.Accounting.Core.Configurations.WebSettings.Version %>" type="text/javascript"></script>
    
    <script type="text/javascript">
        jQuery.noConflict();
        jQuery(document).ready(function () {
            if (jQuery('input[name="chkApplyHedging"]:checked').attr('id') != undefined) {
                apply.updatedMatchHedgingResult();
            }
        });
        
    </script>
    
</head>
<body >
   <div class="overlay" id="overlaydiv" style="display:none;">
   
   </div>

    <table width="100%" height="25" border="0">           
        <tr>
            <td class="c b">SPORT:&nbsp;<%Response.Write(sportName); %></td>
        </tr>
        <tr>
            <td class="c b"><div id="errmsg" style="display:none;">We prevent the frequent calculations during peak hour at weekend, please hold on and try again in <span id="lblduration"></span> seconds. Thanks!</div></td>
        </tr>
    </table> 
    <div id="_sContent" >
        <asp:Literal id="sContent" runat="server" />  
    </div>
      
    <input type="hidden" value="<%=IsAlreadyRefundHT %>" id="IsAlreadyRefundHT" name="IsAlreadyRefundHT" />

    <script src="Js/HandleScore.js?ver=<%=Sunplus.Accounting.Core.Configurations.WebSettings.Version %>" type="text/javascript"></script>
    <script type="text/javascript">
        jQuery(document).ready(function () {
            ScoreHandler.init();
        });
    </script>
</body>
</html>

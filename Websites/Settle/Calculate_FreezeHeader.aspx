<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Calculate_FreezeHeader.aspx.vb"
    Inherits="Settle_Index" %>

<%@ Import Namespace="Sunplus.Accounting.Core.Common" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>
        <%=title%></title>
    <link rel="stylesheet" type="text/css" href="../App_Themes/Public/common.css" />
    <link rel="stylesheet" type="text/css" href="../App_Themes/Public/table.css" />
    <link rel="stylesheet" type="text/css" href="../App_Themes/Public/input.css" />
    <link rel="stylesheet" type="text/css" href="../App_Themes/Settle/Settle.css" />
    <link rel="stylesheet" type="text/css" href="../js/Controls/StyledCheckbox/form[1].css" />
    <link rel="stylesheet" type="text/css" href="../js/IBCWnd/IBCWnd.css" />
    <link rel="stylesheet" type="text/css" href="../App_Themes/Public/Message.css" />
    <link rel="stylesheet" type="text/css" href="../js/jQuery/Bubblepopup/css/jquery.bubblepopup.v2.3.1.css" />
    <link rel="stylesheet" type="text/css" href="../js/jQuery/UI/css/custom-theme/jquery-ui-1.8.10.custom.css" />
    <link rel="stylesheet" type="text/css" href="../js/Window/dhtmlwindow/dhtmlwindow.css" />
    <link rel="stylesheet" type="text/css" href="Styles/SettlementManager.css" />
    <link rel="stylesheet" type="text/css" href="../js/jQuery/GridScroller/gridscroller.css" />
    <script type="text/javascript" src="../js/Controls/StyledCheckbox/custom-form-elements.js"></script>
    <script type="text/javascript" src="../js/prototype.js"></script>
    <%--<script type="text/javascript" src="../js/jQuery/jquery.js"></script>--%>
    <script type="text/javascript" src="../js/jQuery/jquery-1.7.2.min.js"></script>
    <script type="text/javascript" src="../js/jQuery/UI/jquery-ui-1.8.10.custom.min.js"></script>
    <script type="text/javascript" src="../js/jQuery/jquery.shiftclick.js"></script>
    <script type="text/javascript" src="../js/jQuery/jquery.blockUI.1.33.js"></script>
    <script type="text/javascript" src="../js/BlockUI.js"></script>
    <script type="text/javascript" src="../js/common.js?vers=1"></script>
    <script type="text/javascript" src="../js/Settlement.js?vers=2"></script>
    <script type="text/javascript" src="../js/Calculation.js?vers=3"></script>
    <script type="text/javascript" src="../js/Common/ControlUI.js"></script>
    <script type="text/javascript" src="Js/multipleCalculate.js"></script>
    <script type="text/javascript" src="Js/UndecidedSettleMatch.js?vers=1"></script>
    <script type="text/javascript" src="../js/dialogbox.js?ver=6"></script>
    <script type="text/javascript" src="../js/jQuery/GridScroller/jquery.gridscroller.1.0.js"></script>
    <script type="text/javascript">
        var rootpath = '../';
        jQuery.noConflict();
        function updateIsPending(obj, matchid) {
            if (confirm("Are you sure you want to update PENDING status for this match?")) {

                try { PTBlockUI(); } catch (e) { }
                var param = '{"matchid":' + matchid + ',"ispending":' + jQuery(obj).attr("checked") + '}';
                jQuery.ajax({
                    type: "POST",
                    url: "Service/SettlementService.svc/UdpMatch_Pending",
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    data: param,
                    success: function (rs) {
                        if (rs.errCode > 0) {
                            //showAlertMessage("err",rs.errMessage,1);
                            alert(rs.ErrorMessage);
                            jQuery(obj).attr("checked", !jQuery(obj).attr("checked"));
                        }
                        else {
                            alert("Update PENDING status successful.");
                            GetSettleMatch();
                        }
                        try { PTUnBlockUI(); } catch (e) { }
                    }
                });
            }
            else {
                jQuery(obj).attr("checked", !jQuery(obj).attr("checked"));
            }
        }
    </script>
    <%--<style type="text/css">
        .ui-widget-overlay {background-color:transparent;  opacity: .10;filter:Alpha(Opacity=10); }
    </style>--%>
</head>
<body>
    <div style="display: none;" id="dialog-message" title="Calculate MP">
        <p>
            <span class="ui-icon ui-icon-circle-check" style="float: left; margin: 0 12px 12px 0;">
            </span><span id="msgContent"></span>
        </p>
    </div>
    <div style="display: none;" id="dialog-result" title="Calculate MP">
        <p>
            <span class="ui-icon ui-icon-circle-check" style="float: left; margin: 0 12px 12px 0;">
            </span><span id="msgResult"></span>
        </p>
    </div>
    <div style="display: none;" id="dialog-numbergame" title="">
    </div>
    <div class="main">
        <div class="maincontent">
            <div id="tabmain_conts">
                <div id="content_tab">
                    <ul>
                        <li id="current"><a href="#"><span>
                            <%=title%></span></a></li>
                    </ul>
                </div>
            </div>
            <div class="maincontent_box">
                <div id="searchbox">
                    <asp:Literal ID="searchContent" runat="server"></asp:Literal>
                </div>
            </div>
            <div id="divcontainer" class="bg_table allhidden">
                <div id='sContent'>
                </div>
                <div style="display: none;" id="divMatch" class="divMatch65">
                    <iframe frameborder="0" src="" id="frMatch" name="frMatch" class="frMatch65" scrolling="no"
                        allowtransparency="true"></iframe>
                </div>
                <form>
                <input type="hidden" id="hidcontent" />
                </form>
                <div id="numbergame_templaterf" style="display: none;">
                    <div id="numbergameboxrf{matchid}" class="numbergameboxrf">
                        <div class="rowrf" style="">
                            <input tabindex="9" class="" type="checkbox" id="chkissend{matchid}" rel="{matchid}"
                                onclick="numgamerefund.isCheckSend(this);" />&nbsp;<strong style="cursor: pointer;"
                                    onclick="numgamerefund.isCheckSend();">Send Message</strong>
                        </div>
                        <div class="rowrf message {matchid}">
                            <div class="colright label">
                                <span class="require">*</span>Message EN</div>
                            <div class="colleft">
                                <textarea tabindex="10" cols="" rows="" id="msgen{matchid}" class="ereacontent {matchid}"></textarea>
                            </div>
                        </div>
                        <div class="rowrf message {matchid}">
                            <div class="colright label">
                                <span class="require">*</span>Message CH</div>
                            <div class="colleft">
                                <textarea tabindex="11" cols="" rows="" id="msgch{matchid}" class="ereacontent {matchid}"></textarea>
                            </div>
                        </div>
                        <div class="rowrf message {matchid}">
                            <div class="colright label">
                                <span class="require">*</span>Message CS</div>
                            <div class="colleft">
                                <textarea tabindex="12" cols="" rows="" id="msgcs{matchid}" class="ereacontent {matchid}"></textarea>
                            </div>
                        </div>
                        <div class="rowrf" style="margin-top: 10px;">
                            <input tabindex="13" class="btn" type="button" onclick="numgamerefund.refundSubmit('{matchid}', '{type}');"
                                value="Submit" />&nbsp;&nbsp;<input class="btn" type="button" onclick="numgamerefund.closeNumbergamePopup();"
                                    value="Cancel" tabindex="14" />
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
    <script type="text/javascript">
        Custom.init();

        var getNumberGameBalls = function (matchid, matchcode, kickoffdate) {
            clearTimeouts_ReCalcNumberGame();
            var width = 360;
            var url = "ReCalcNumberGame.aspx?matchid=" + matchid + "&matchcode=" + matchcode + "&kickoffdate=" + kickoffdate;
            var title = "Settlement";
            showSettlementPopup(url, title, width, 260);
        };

        var recalNumberGameTimerIds = [];

        function clearTimeouts_ReCalcNumberGame() {
            for (var i = 0; i < recalNumberGameTimerIds.length; ++i) {
                clearTimeout(recalNumberGameTimerIds[i]);
            }
            this.recalNumberGameTimerIds = [];
        }

        function addTimerId_ReCalNumberGame() {

            var timerId = setTimeout(function () {
                clearTimeouts_ReCalcNumberGame();
                GetSettleMatch();
            }, 3000);
            recalNumberGameTimerIds.push(timerId);
        }

        jQuery(document).ready(function () {
            jQuery("#btnSubmit").click(function () {
                clearTimeouts_ReCalcNumberGame();
            });
        });

        undecided.userId = '<%=AppSession.AccId %>';
        setInterval(settlementManagement.getCalcMPStatus, 10000);
    </script>
</body>
</html>

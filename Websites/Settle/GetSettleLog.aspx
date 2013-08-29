<%@ Page Language="C#" AutoEventWireup="true" CodeFile="GetSettleLog.aspx.cs" Inherits="Settle_GetSettleLog" Theme="" %>
<%@ Import Namespace="Sunplus.Accounting.Core.Configurations" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Settlement Log</title>
    <meta http-equiv="X-UA-Compatible" content="IE=EmulateIE7" />
    <link href="../App_Themes/Public/common.css" rel="stylesheet" type="text/css" />
    <link href="../App_Themes/Public/table.css" rel="stylesheet" type="text/css" />
    <link href="../App_Themes/Public/input.css" rel="stylesheet" type="text/css" />
    <link href="../App_Themes/Settle/Settle.css" rel="stylesheet" type="text/css" />
    <link href="../App_Themes/Public/Message.css" rel="stylesheet" type="text/css" />
    <link href="../App_Themes/Report/reportgrid.css" rel="stylesheet" type="text/css" />
    <script src="../js/jQuery/jquery.js" type="text/javascript"></script>
    <script src="../js/jQuery/jquery.json-2.2.min.js" type="text/javascript"></script>
    <script src="../js/jQuery/json2.js" type="text/javascript"></script>
    
    <script src="../js/jQuery/gridscroller.js" type="text/javascript"></script>
    <script src="../js/common.js" type="text/javascript"></script>
    <script type="text/javascript" src="../js/jQuery/Template/jquery.tmpl.js"></script>
    <script src="../js/jQuery/scrolltable/webtoolkit.jscrollable.js" type="text/javascript"></script>
    <script src="../js/jQuery/scrolltable/webtoolkit.scrollabletable.js" type="text/javascript"></script>
    <script src="../js/Settlement.js" type="text/javascript"></script>
    <script src="Js/ViewSettleLog.js?ver=11dd22" type="text/javascript"></script>
</head>
<body>
<div class="main">
        <div class="maincontent">
            <div id="tabmain_conts">
                <div id="content_tab">
                    <ul>
                        <li id="current"><a href="#"><span>
                            Settlement Log</span></a></li>
                    </ul>
                </div>
            </div>

            <%=html.ToString() %>
            
            <div class="maincontent_box">
                <div id="searchbox">
                    <table>
                        <tbody>
                            <tr id="trmsg">
                                <td colspan="7">
                                    <span id="spMsg"></span>
                                </td>
                            </tr>
                            <tr>
                                <td class="pad73 b">Match Code:</td>
                                <td>
                                    <input type="text" value="<%=Matchcode %>" size="16" maxlength="40" name="txtMatchCode" id="txtMatchCode">
                                </td>
                                <td id="tdFromDate" class="pad73 b">From Date:</td>
                                <td>  <%=FromDate %> </td>
                                <td id="tdToDate" class="pad73 b">To Date</td>                                    
                                <td><%=ToDate %></td>
                                <td align="right" colspan="2" class="pad73">
                                    <input type="button" class="btn" onclick="settleLog.selSettleLog()" value="Search"
                                        name="btnSubmit" id="btnSubmit">
                                </td>
                            </tr>
                        </tbody>
                    </table>
                </div>
            </div>
            
            <div id="mainreport" class="bg_table">
                <div id="settlementLog"></div>
                <script type="text/html" id="scriptSettlementLog">
                
                    {{each(i, current) Rows}}
                        <table height="20" cellspacing="0" cellpadding="0" border="0" style="margin-bottom: 3px;"
                            class="menu_box">
                            <tbody>
                                <tr onclick="DisplayMatchInfoSettleLog('${MatchId}');" id="btntop" style="cursor: pointer;">
                                    <td class="btnleft">
                                    </td>
                                    <td align="left" class="btnmiddle " style="width: auto">
                                        <span class="mp">${LeagueName}</span>
                                    </td>
                                    <td width="15px" class="md2 right">
                                    </td>
                                    <td class="btnright">
                                    </td>
                                </tr>
                            </tbody>
                        </table>
                        <div id="${MatchId}" style="padding-bottom:5px;">
                            <div class="content-warper hidden">
                                <div class="content-toolbox bgtitle">
                                    <div id="gridtitle" class="gridtitle">&nbsp;
                                        {{if (SportType == 162)}}
                                            ${ShowTime}
                                        {{else}}
                                            <span class='ht'>${HomeName}</span> -vs- <span class='at'>${AwayName}</span>
                                        {{/if}}
                                        &nbsp;&nbsp;&nbsp;&nbsp;Event date: ${EventDate}</div>
                                    <div class="btngroup">
                                    </div>
                                 </div>
                                <div class="content-header clearfix">
                                    <div class="header-scroll">
                                        <table class="tbl-header-scroll">
                                            <tr>
                                                <td style="border-bottom: none 0px;">
                                                    <div class="col20 c">
                                                        #</div>
                                                </td>
                                                <td style="border-bottom: none 0px;">
                                                    <div class="col120 c">
                                                        Date</div>
                                                </td>
                                                <td style="border-bottom: none 0px;">
                                                    <div class="col80 c">
                                                        UserName</div>
                                                </td>
                                                <td style="border-bottom: none 0px;">
                                                    <div class="col180 c">
                                                        Action</div>
                                                </td>
                                                <td style="border-bottom: none 0px;">
                                                    <div class="col40 c">
                                                        HT Score</div>
                                                </td>
                                                <td style="border-bottom: none 0px;">
                                                    <div class="col40 c">
                                                        FT Score</div>
                                                </td>
                                                <td style="border-bottom: none 0px;">
                                                    <div class="col60 c">
                                                        Goal Record</div>
                                                </td>
                                                <td style="border-bottom: none 0px;">
                                                    <div class="col60 c">
                                                        Result String</div>
                                                </td>
                                                <td style="border-bottom: none 0px;">
                                                    <div class="col160 c">
                                                        Remark</div>
                                                </td>
                                                <td style="border-bottom: none 0px;">
                                                    <div class="col80 c">
                                                        IP</div>
                                                </td>
                                            </tr>
                                        </table>
                                    </div>
                                </div>
                                <div class="content-body clearfix">
                                    <div class="body-scroll">
                                        <table class="tbl-body-scroll">
                                            {{if (SettleLogDetails != null && SettleLogDetails.length > 0)}}
                                                {{each(j, curentChild) SettleLogDetails}}
                                                    <tr>
                                                        <td>
                                                            <div class="col20 c">
                                                                ${No}</div>
                                                        </td>
                                                        <td>
                                                            <div class="col120 c">
                                                                ${Date}</div>
                                                        </td>
                                                        <td>
                                                            <div class="col80 c">
                                                                ${UserName}</div>
                                                        </td>
                                                        <td>
                                                            <div class="col180 c">
                                                                ${Action}</div>
                                                        </td>
                                                        <td>
                                                            <div class="col40 c">
                                                                ${HTScore}</div>
                                                        </td>
                                                        <td>
                                                            <div class="col40 c">
                                                                ${FTScore}</div>
                                                        </td>
                                                        <td>
                                                            <div class="col60 c">
                                                                ${GoalRecord}</div>
                                                        </td>
                                                        <td>
                                                            <div class="col60 c">
                                                                ${ResultString}</div>
                                                        </td>
                                                        <td>
                                                            <div class="col160 c">
                                                                ${Remark}</div>
                                                        </td>
                                                        <td>
                                                            <div class="col80 c">
                                                                ${Ip}</div>
                                                        </td>
                                                    </tr>
                                                    {{/each}}
                                                {{else}}
                                                    <tr><td colspan="10" style="width:887px; border-right:none;">There is no information available.</td></tr>
                                                {{/if}}
                                            </table>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        {{/each}}
                    
                    </script>
                    
                
                <input type="hidden" id="t" name="t" value="<%=Request["t"] %>" />
            </div>
    </div>
    </div>
</body>
</html>

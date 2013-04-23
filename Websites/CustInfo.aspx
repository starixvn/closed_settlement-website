<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CustInfo.aspx.cs" Inherits="CustInfo"  Async="true"%>
<%@ Import Namespace="Sunplus.Accounting.Core.Common" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Customer Info</title>
    <link href="App_Themes/LeftPanel/left_panel.css" rel="stylesheet" type="text/css" />
    <link href="App_Themes/Public/common.css" rel="stylesheet" type="text/css" />
    <link href="App_Themes/Public/input.css" rel="stylesheet" type="text/css" />
    <link href="js/jQuery/scrollbar/jScrollPane.css" rel="stylesheet" type="text/css" />
    <link href="App_Themes/Public/Pagination.css" rel="stylesheet" type="text/css" />
    <link href="App_Themes/Public/Message.css" rel="stylesheet" type="text/css" />

    <link href="js/jQuery/message/jquery.msg.css" rel="stylesheet" type="text/css" />

    <script src="js/jQuery/jquery.js" type="text/javascript"></script>
    <script src="js/jQuery/jquery.json-1.3.js" type="text/javascript"></script>
    <script type="text/javascript" src="js/jQuery/scrollbar/jquery.mousewheel.js"></script>
    <script type="text/javascript" src="js/jQuery/scrollbar/jScrollPane.js"></script>
    <script type="text/javascript" src="js/common.js?v=<%=AccCommon.Version() %>"></script>
    <script type="text/javascript" src="js/CustomerInfo.js?v=<%=AccCommon.Version() %>"></script>
    <script type="text/javascript" src="js/CustSetting.js?v=<%=AccCommon.Version() %>"></script>

    <script src="js/jscookies.js?v=<%=AccCommon.Version() %>" type="text/javascript"></script>
    <script src="js/showhidepopup.js" type="text/javascript"></script>
    <script type="text/javascript" src="js/Menu/Dropdown/menu.js?v=<%=AccCommon.Version() %>"></script>
    <script src="js/pager_v2.js" type="text/javascript"></script>

    <script src="js/jQuery/message/jquery.center.min.js" type="text/javascript"></script>
    <script src="js/jQuery/message/jquery.msg.js" type="text/javascript"></script>

    <style type="text/css">
        #popLimitPerIP
        {
            display: none;
            position: absolute;
            background: #fff;
            left: 105px;
            width: 140px;
            padding: 2px;
            border: solid 1px;
        }
        .memberClosed
        {
            background-color: #CCC7BA;
        }
        .memberSuspended
        {
            background-color: #F8DED7;
        }
        .memberDisabled
        {
            background-color: #A0A97A;
        }
    </style>
    <script type="text/javascript">
        function resetCustMenu() {
            $('.button_left02_active').removeClass("button_left02_active");
        }
              
        function ViewVRHistory(CustID,UserName) 
        {
            var url = 'Report/Others/VRHistory.aspx?CustID=' + CustID + '&UserName=' + UserName;
            showPopup('VIRTUAL RATE UPDATE HISTORY (' + UserName + ')', url, 0, 0, 420, 270, false, 0, null);
        }
    
        var result = "";
        var custid; var roleid;
        var totalPage = 1;

        $(document).ready(
          function init() {

              $('#pane2').jScrollPane({ showArrows: true, scrollbarWidth: 15, arrowSize: 16 });
              if (custid != null && roleid != null) {
                  ajaxGetDownlinelist(custid, roleid, 1);
                  window.setTimeout(function () {
                      ajaxOnlinelist(custid);
                  }, 5000);
                  
              }
              try { parent.jQuery.unblockUI(); } catch (e) { }
              $('.button_left02').click(function () {
                  $('.button_left02_active').removeClass("button_left02_active");
                  $(this).addClass("button_left02_active");
                  //Reset main menu
                  if (parent.resetMenu) {
                      parent.resetMenu();
                  }
              });

              //Johnny Add: User info hide or show is store in cookie.
              //alert(Get_Cookie("coUserInfo"));
              var className = Get_Cookie("coUserInfo");
              var curr = $("#btnupdownCustInfo");
              var tmp = $("#custInfoDetal");
              if (className == "show") {
                  tmp.removeClass("hidden");
                  curr.attr("title", "Hide Detail");
                  curr.html('<img src="App_Themes/Images/icon_down.gif" class="imgDownUp" />');
              }
          }
          );
        
        var currlevelbox = "";
        function OpenCloseMenuDangerous(obj, divid, isBA, currentLevel) {
//            if (divid == "divLevelOriginal") {
//                if (isBA == "True" || currentLevel == 100) {
//                    return false;
//                }
//            }
            if (currlevelbox==divid) {
                $(".divLevel").hide();
                currlevelbox = "";
            }
            else {
                $(".divLevel").hide();
                $("#" + divid).show();
                var top = obj.offsetTop;
                $("#" + divid).css('top', top + "px");
                currlevelbox = divid;
            }
         
        }
     
        function OpenCloseMenuDangerous1() {
            //$('#dangerousbox').css('display', ($('#dangerousbox').css('display') == 'inline') ? 'none' : 'inline');
            $('#dangerousbox').toggle();
        }

        function ajaxGetDownlinelist(custid, roleid, pageindex, iscallbypager) {
            var params = '{"custId":' + custid + ', "roleId":' + roleid + ', "pageIndex":' + pageindex + '}';
            $.ajax({
                type: "POST",
                url: "Services/WCF/Customer/ServiceCustomerInfo.svc/GenDownlineList",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                data: params,
                success: function (rs) {
                    var rsvalue = $.evalJSON(rs);

                    //var rsvalue = jQuery.parseJSON(rs);
                    totalPage = parseInt(rsvalue.Totalpages);
                    $("#downlinecontent").html(rsvalue.Content);
                    //Add Pager
                    if (iscallbypager != null) {
                        left_Tab1_Click(document.getElementById("left_tab1_2"));
                    }
                    if (totalPage > 1) {
                        $("#dllpager").html(rsvalue.Pager);
                        $(".pagenav_ibtn").click(function () {
                            if (isNumberNotNull("txtpage")) {
                                var cpage = $("#txtpage").val();
                                if (parseInt(cpage) <= totalPage) {
                                    ajaxGetDownlinelist(custid, roleid, cpage, "");
                                }
                            }
                            return false;
                        });
                    }
                },
                error: function (xhr, status) {
                    $("#downlinecontent").html(status);
                }
            });
        }
        function pageritemclick(pageindex) {
            ajaxGetDownlinelist(custid, roleid, pageindex,"")
        }
      
                 
        function ajaxOnlinelist(custid) {
                var params = '{"custId":' + custid + '}';
                $.ajax({
                    type: "POST",
                    url: "Services/WCF/Customer/ServiceCustomerInfo.svc/GenOnlineList",
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    data: params,
                    success: function(rs) {
                        //document.getElementById("onlinepanel").innerHTML = rs;
                        $('#onlinepanel').html(rs);
                    }
                });
            }

            function changeDisplayStatus() {
                setTimeout("delayreload()", 2000);
            }
            function delayreload() {
                location.href = location.href;
                try{parent.jQuery.unblockUI();}catch(e){}
            }
            
            function showhideLimitPerIP()
            {
                $("#popLimitPerIP").toggle();
            }
            
            function ChangeLimitPerIP(obj)
            {
                var limitPerIP=$(obj).val();
                if (roleid>1)
                {
                    if (limitPerIP>0)
                    {
                        $("#chkIsUpdateDL").attr("disabled","disabled");
                        $("#chkIsUpdateDL").attr("checked","true");
                    }
                    else
                    {
                        $("#chkIsUpdateDL").removeAttr("disabled");
                    }
                }
            }
            function UpdateLimitPerIP()
            {
                var isUpdateDownline=$('#chkIsUpdateDL').attr('checked');
                var limitPerIP=$('#lstLimitPerIP').val();
                //alert(isUpdateDownline + "|" + limitPerIP);
                updateLimitPerIP(limitPerIP, isUpdateDownline);
            }
         
    </script>
    <script language="javascript" type="text/javascript">
        function showhideRacebooksStatus(id, roleids) {
            var title = 'Update Racing Status';
            var url = 'Customer/PT/Racing/RacingStatusUpdate.aspx?custId=' + id;
            var width = 350;
            var height = 360;
            parent.OpenPopup(url, title, width, height, true, true, window, 2000, null, null);
        }
         function showhideP2PStatus(id, roleids) {
            var title = 'Update P2P Status';
            var url = 'Customer/PT/Poker/P2PStatusUpdate.aspx?custId=' + id + '&roleId=' + roleid;
            var width = 400;
            var height = 300;
            parent.OpenPopup(url, title, width, height, true, true, window, 2000, null, null);
        }
        
    </script>
</head>
<body>
    <script type="text/javascript">
        function ViewLogs(TabName, CustName) {
            document.getElementById('TabLogs').value = TabName;
            var params = "?tabid=CustomerSetting&custname=" + CustName;
            if (TabName == "Login")
                parent.frames["centerFr"].location = "LogDisplay/Setting/Display.aspx?tabid=Login&custname=" + CustName + '&fromdate=' + '<%=DateTime.Now.AddDays(-1).ToString("MM/dd/yyyy")%>' + '&todate=' + '<%=DateTime.Now.ToString("MM/dd/yyyy")%>';
            else if (TabName == "Setting")
                parent.frames["centerFr"].location = "LogDisplay/Setting/Display.aspx?tabid=CustomerSetting&custname=" + CustName + '&fromdate=' + '<%=DateTime.Now.AddDays(-1).ToString("MM/dd/yyyy")%>' + '&todate=' + '<%=DateTime.Now.ToString("MM/dd/yyyy")%>';
            else if (TabName == "Status")
                parent.frames["centerFr"].location = "LogDisplay/Setting/Display.aspx?tabid=Status&custname=" + CustName + '&fromdate=' + '<%=DateTime.Now.AddDays(-1).ToString("MM/dd/yyyy")%>' + '&todate=' + '<%=DateTime.Now.ToString("MM/dd/yyyy")%>';
            else if (TabName == "Credit")
                parent.frames["centerFr"].location = "LogDisplay/Setting/Display.aspx?tabid=Credit&custname=" + CustName + '&fromdate=' + '<%=DateTime.Now.AddDays(-1).ToString("MM/dd/yyyy")%>' + '&todate=' + '<%=DateTime.Now.ToString("MM/dd/yyyy")%>';
        }  
        
     
    </script>
    <script type="text/javascript" src="js/ToolTip/wz_tooltip/wz_tooltip.js"></script>
    <script type="text/javascript" src="js/ToolTip/wz_tooltip/tip_balloon.js"></script>
    
    <div style="width: 100%; text-align: center;">
        <asp:Literal EnableViewState="false" ID="HtmlHolder" runat="server"></asp:Literal>
    </div>
    <iframe id="frmUpdate" name="frmUpdate" frameborder="0" height="0" width="0" />
</body>
</html>

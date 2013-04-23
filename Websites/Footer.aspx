<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Footer.aspx.cs" Inherits="_Footer" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Admin Site</title>
    <style type="text/css">
        div.copyright
        {
            float: right;
            color: #434343;
            font-family: Tahoma;
            font-size: 11px;
            text-align: right;
            padding-right: 6px;
            vertical-align: bottom;
        }
        a
        {
            color: #0066CC;
            text-decoration: none;
        }
        a.hover
        {
            text-decoration: underline;
        }
       
        #lastnewsrotate
        {
        	font-family: Tahoma;
        	padding-left: 6px;
        	font-size: 11px;
        	height:14px;
        	float:left;
            width: 500px;
        }
        #divmore
        {
        	font-family: Tahoma;
        	font-size: 11px;
        	height:14px;
            float: left;
            padding-left: 6px;
            font-weight: bold;
        }
    </style>

    <script src="js/jQuery/jquery.min.js" type="text/javascript"></script>
    <script src="js/jQuery/jquery.cycle.js" type="text/javascript"></script>
    <script src="js/jQuery/jquery.cookie.js" type="text/javascript"></script>
    <script type="text/javascript">
        function SwapVersion() {
            $.ajax({
                type: "POST",
                url: "Handlers/SwapVersion.ashx",
                contentType: "application/json; charset=utf-8",
                success: function (data) {
                    top.location.href = data;

                }
            })
        }
        function gotoURL(frmName, url) {
            var obj = parent.content.document.getElementById(frmName);
            $(obj).attr("src", url);
        }
</script>
</head>
<body style="margin: 0px">
    <div style="padding-left:6px;"></div>
    <div class="copyright" >
       &nbsp; Copyright &copy;2012
    </div>
</body>
</html>

<%@ Page Language="C#" AutoEventWireup="true" CodeFile="SelectMatchMoveBet.aspx.cs" Inherits="Settle_SelectMatchMoveBet" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Select match</title>
    
    <link href="../App_Themes/Public/input.css" rel="stylesheet"
        type="text/css" />
    <link href="../App_Themes/Settle/MoveBet.css" rel="stylesheet" type="text/css" />

    <script src="../js/jQuery/jquery.min.js" type="text/javascript"></script>
    <script src="../js/jQuery/jquery.json-1.3.min.js" type="text/javascript"></script>
    <script src="../js/jQuery/jquery-jtemplates.js" type="text/javascript"></script>    
    <script src="../js/Settle/BetdaqMoveBet.js" type="text/javascript"></script>
    <script src="../js/common.js" type="text/javascript"></script>
    <script type="text/javascript">
        var matchid = gup('matchid');
        var matchcode = gup('matchcode');
        $(document).ready(function () {
            RenderMatches(matchid, matchcode, '<%=sysPath%>');
        });
    </script>
</head>
<body>
    <form id="frmMatch">
    <div id="divMatches">
    
    </div>
    </form>
</body>
</html>

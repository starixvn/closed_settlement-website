<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Menu.aspx.cs" Inherits="Others_Menu" theme=""%>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">
    <title></title>
    <link rel="stylesheet" type="text/css" href="../App_Themes/SteelBlue/menu.css"/>     
    <script type="text/javascript" src="../js/prototype.js"></script>    
    
</head>
<body class="no-margin">
    <asp:Literal ID="sMenu" runat="server"></asp:Literal>
    <script type="text/javascript">
        var fr =parent.frames['fContent'];
        //if (typeof fr !='undefined' && fr) fr.location.replace('../UserControl/Index.aspx');
        if (typeof fr != 'undefined' && fr) fr.location.replace('../BetList/BetListByIP.aspx?bltype=3&nodate=yes');
    </script>
</body>
</html>

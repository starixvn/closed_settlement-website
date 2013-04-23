<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Function.aspx.cs" Inherits="UserControl_Function" Title="Untitled Page" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">
    <title></title>
     <link href="../App_Themes/Public/table.css" rel="stylesheet" type="text/css" />
    <link href="../App_Themes/Public/input.css" rel="stylesheet" type="text/css" />
    <link href="../App_Themes/Public/common.css" rel="stylesheet" type="text/css" />
    <link href="../App_Themes/Public/Message.css" rel="stylesheet" type="text/css" />
    <link href="../App_Themes/users.css" rel="stylesheet" type="text/css" />

    <script src="JS/ManageFunction.js" type="text/javascript"></script>

    <script src="../js/jQuery/jquery.js" type="text/javascript"></script>
    <script type="text/javascript" src="../js/common.js"></script>
    <script type="text/javascript">

        var isDelFunc = false;
        function OpenEditDept(FuncId, FuncName, FuncDesc, Files, ParentId, system) {
            FuctionParentEdit(system, ParentId);
            $("#spTitle").html("Edit Function");
            $("#divFuncUpd").show();
            $("#hdAdd").val("0");
            $("#txtName").val(FuncName);
            $("#txtDesc").val(FuncDesc);
            $("#txtFileNames").val(Files);

            $("#slsystem").value = system;
            $("#hdsystem").value = system;
            $("#slsystem").attr("disabled","disabled");

            $("#hdFuncId").val(FuncId);
            alert_error("", "err");
            $("#txtName").focus();
        }
        function DelFunction(FuncId) {
            if (confirm("Are you sure you want to delete this function?")) {
                isDelFunc = true;
                $("#hdAdd").val("2");
                $("#hdFuncId").val(FuncId);
                $("#frUpd").submit();
            }
        }
        function AddFunc() {
            isDelFunc = false;
            $("#spTitle").html("Add Function");
            if ($("#divFuncUpd").css("display") == "" && $("#hdAdd").val() == "1")
                $("#divFuncUpd").hide();
            else
                $("#divFuncUpd").show();
            $("#hdAdd").val("1");
            $("#txtName").val("");
            $("#txtDesc").val("");
            $("#txtFileNames").val("");
            $("#slParent").value = "";
            $("#slParent").removeAttr("disabled");
            $("#slsystem").removeAttr("disabled");
            alert_error("", "err");
        }
        function CancelUpd() {
            $("#divFuncUpd").hide();
            alert_error("", "err");
        }
        function CheckForm() {
            if (isDelFunc)
                return true;
            if (isEmpty("txtName")) {
                alert_error("Please input function name.", 'err');
                return false;
            }
            if (isEmpty("txtDesc")) {
                alert_error("Please input function description.", 'err');
                return false;
            }
            if (isEmpty("txtFileNames")) {
                alert_error("Please input file name.", 'err');
                return false;
            }
            return true;
        }
        function UpdateErr(errMsg) {
            alert_error(errMsg, 'err');
        }
        function UpdateSucc() {
            location.href = location.href;

        }

        function FuctionParent(system) {
            if (system) {
                sys = system.value;
            }
            $.ajax({
                type: "POST",
                url: "FunctionParent.ashx",
                async: true,
                data: 'system=' + sys + '&parentid=0',
                success: function(strdata) {
                    $('#parent').html(strdata);
                }
            });
        }
        function FuctionParentEdit(system, parentid) {
             $.ajax({
                type: "POST",
                url: "FunctionParent.ashx",
                async: true,
                data: 'system=' + system + '&parentid=' + parentid,
                success: function(strdata) {
                    $('#parent').html(strdata);
                    }
            });
            
        }
        GetFunctionList();
    </script>
   
</head>
<body>
    <div class="main">
        <div class="maincontent">
             <table border="0" cellpadding="0px" cellspacing="0px" class="maincontent_header">
                <tr>
                    <td class="maincontent_headerL">
                    </td>
                    <td class="maincontent_headerC">Function
                    </td>
                    <td class="maincontent_headerR">
                    </td>
                </tr>
            </table>
            <div class="maincontent_box">
                <div class="errMsg" id="err"></div>
                <div style="padding:2px;">
                    <span class="btn" onclick="AddFunc();" style="cursor:pointer;">Add Function</span>
                </div>
            </div>
            <div class="bg_table">
                <div id="sContent">
                  <asp:Literal ID="htmlAdd" runat="server"></asp:Literal>     
                </div>
            </div>
        </div>
    </div>
</body>
</html>

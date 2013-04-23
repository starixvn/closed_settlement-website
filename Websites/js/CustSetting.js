

 function ClosePopup(div)
 {
        var chk=$("#" + div.replace(/Pop/,"chk"));
        var chkhdf=$("#" + div.replace(/Pop/,"chkhdf"));
        var chkDL=$("#" + div.replace(/Pop/,"chkDL"));
        var chkhdfDL = $("#" + div.replace(/Pop/, "chkhdfDL"));
          if (chkhdf.val()=="1")
          {
             chk.attr('checked', true);
          }
          else
          {
             chk.attr('checked',false);
         }
          if (chkDL!=null)
          {
              if (chkhdfDL.val()=="1")
              {
                 chkDL.attr('checked',true);
              }
              else
              {
                 chkDL.attr('checked',false);
              }
              chkDL.attr('disabled','disabled');
          }
          $("#" + div).css('display','none');
          overlay(div.replace(/Pop/,"overlay"));
      }

   function ClosePopup1(div) {
          $("#" + div).toggle();
          overlay(div.replace(/popup/, "overlay"));
      }
  function ClosePopUpDanger(div) {
      $("#" + div).toggle();
      overlay(div.replace(/popup/, "overlay"));
      $('#dangerousAgentoverlay').css('display', 'none');
      $("#" + currlevelbox).css('display', 'none');
      }
 function overlay(id) {
     var el = $("#" + id);
     el.toggle();
    }

    function UpdateSettingStatus(chk1, chk2, type) {
        

    try {                
        var yes = false;
        var isUpdateDownline = false;
        yes = !$("#" + chk1).attr("checked");
        if ($("#" + chk2) != null)
        {
            isUpdateDownline = !$("#" + chk2).attr("checked");
        }

        if ($("#" + chk1).attr('checked') == false && type == "disabledCS" && roleid != 1) {
          
            alert('Casino downlines will be disabled as well');
        }
        
         if ($("#" + chk1).attr('checked') == false && type == "disabledBingoGame" && roleid != 1) {
          
            alert('Bingo downlines will be disabled as well');
        }

        if (type == 'SelectedBetting') {
            yes = $("#" + chk1).attr('checked');
            if (yes)
                isUpdateDownline = true;
            else {
                isUpdateDownline = !$("#" + chk2).attr("checked");
            }
            
        }
        var _URL ='Handlers/UpdateCustomerStatus.ashx?isupddownline=' + isUpdateDownline + '&type=' + type +'&status=';
        _URL += String(yes);
        _URL += '&custid=' + GetCustId();  
        var fr =self.frames['frmUpdate'];
        if (typeof fr !='undefined' && fr) fr.location.href =_URL;
        if ($("#" + chk1).attr('checked'))
        {
            $("#" + chk1 + "hdf").val("0");
        }
        else
        {
            $("#" + chk1 + "hdf").val("1");
        }
        if ($("#" + chk2) != null)
        {
            if ($("#" + chk2).attr('checked'))
            {
                $("#" + chk1 + "hdfDL").val('0');
            }
            else
            {
                $("#" + chk1 + "hdfDL").val('1');
            }
            $("#" + chk1 + "DL").attr('disabled', 'disabled');
        }

        $("#" + chk1.replace(/chk/, "Pop")).css('display',($("#" + chk1.replace(/chk/, "Pop")).css('display') == 'none') ? 'inline' : 'none');
         overlay(chk1.replace(/chk/,"overlay"));
    }
    catch(ex) {
        alert(ex);
    }            
 }
 function changeDisable(obj, type) {
     if (type == 'SelectedBetting') {
         if ($("#" + obj.id + "DL") != null) {
             if ($("#" + obj.id).attr('checked'))//Selected
             {
                 $("#" + obj.id + "DL").attr('disabled', 'disabled');
                 $("#" + obj.id + "DL").attr("checked", true);
              
             }
             else//UnSelect
             {
                 var oldStatus = $("#" + obj.id + "hdf").val() == "1" ? "1" : "0"; ;
                 var newStatus = $("#" + obj.id).attr('checked') ? "1" : "0";

                 if (oldStatus != newStatus) {
                     $("#" + obj.id + "DL").attr('disabled', '');

                 }
                 else {

                     if ($("#" + obj.id + "hdfDL").val() == "1") {
                         $("#" + obj.id + "DL").attr("checked", true);
                     }
                     else {
                         $("#" + obj.id + "DL").attr("checked", false);
                     }
                     $("#" + obj.id + "DL").attr('disabled', 'disabled');
                 }
             }
         }
     }
     else {
         if ($("#" + obj.id + "DL") != null) {
             if (!$("#" + obj.id).attr('checked'))//Disabled
             {
                 $("#" + obj.id + "DL").attr('disabled', 'disabled');
                 $("#" + obj.id + "DL").attr("checked", false);
             }
             else//Enabled
             {
                 var oldHRStatus = $("#" + obj.id + "hdf").val() == "1" ? "1" : "0"; ;
                 var newHRStatus = $("#" + obj.id).attr('checked') ? "1" : "0";

                 if (oldHRStatus != newHRStatus) {
                     $("#" + obj.id + "DL").attr('disabled', ''); ;
                 }
                 else {
                     if ($("#" + obj.id + "hdfDL").val() == "1") {
                         $("#" + obj.id + "DL").attr("checked", true);
                     }
                     else {
                         $("#" + obj.id + "DL").attr("checked", false);
                     }
                     $("#" + obj.id + "DL").attr('disabled', 'disabled');
                 }
             }
         }
     }

     var chk1 = obj.id;
     //    alert((chk1));
     //    alert($("#" + chk1).attr('checked'));
     //    alert($('#chkOldValueNumberGame').val());

     if (type == "HR")
         if (($("#" + chk1).attr('checked') == true && $('#chkOldValueSATote').val() == '1') ||
            ($("#" + chk1).attr('checked') == false && $('#chkOldValueSATote').val() == '0')) {
             $("#disabledHRPop .btn:first").removeAttr("disabled");
         } else {
             $("#disabledHRPop .btn:first").attr("disabled", "disabled");
         }
     if (type == "CS")
         if (($("#" + chk1).attr('checked') == true && $('#chkOldValueCasino').val() == '1') ||
            ($("#" + chk1).attr('checked') == false && $('#chkOldValueCasino').val() == '0')) {
             $("#disabledCSPop .btn:first").removeAttr("disabled");
         } else {

             $("#disabledCSPop .btn:first").attr("disabled", "disabled");
         }
     if (type == "OpenTote")
         if (($("#" + chk1).attr('checked') == true && $('#chkOldValueUKTote').val() == '1') ||
            ($("#" + chk1).attr('checked') == false && $('#chkOldValueUKTote').val() == '0')) {
             $("#disabledHROpenTotePop .btn:first").removeAttr("disabled");
         } else {
             $("#disabledHROpenTotePop .btn:first").attr("disabled", "disabled");
         }
     if (type == "FI")
         if (($("#" + chk1).attr('checked') == true && $('#chkOldValueFinance').val() == '1') ||
            ($("#" + chk1).attr('checked') == false && $('#chkOldValueFinance').val() == '0')) {
             $("#disabledFinancialPop .btn:first").removeAttr("disabled");
         } else {
             $("#disabledFinancialPop .btn:first").attr("disabled", "disabled");
         }
     if (type == "BI")
         if (($("#" + chk1).attr('checked') == true && $('#chkOldValueBingo').val() == '1') ||
            ($("#" + chk1).attr('checked') == false && $('#chkOldValueBingo').val() == '0')) {
             $("#disabledBingoGamePop .btn:first").removeAttr("disabled");
         } else {
             $("#disabledBingoGamePop .btn:first").attr("disabled", "disabled");
         }
    if (type == "NumberGame")
            if (($("#" + chk1).attr('checked') == true && $('#chkOldValueNumberGame').val() == "1") ||
        ($("#" + chk1).attr('checked') == false && $('#chkOldValueNumberGame').val() == "0")) {
                $("#disabledNumberGamePop .btn:first").removeAttr("disabled");
            } else {
                $("#disabledNumberGamePop .btn:first").attr("disabled", "disabled");
            }

    if (type == "LCS")
        if (($("#" + chk1).attr('checked') == true && $('#chkOldValueLiveCasino').val() == '1') ||
            ($("#" + chk1).attr('checked') == false && $('#chkOldValueLiveCasino').val() == '0')) {
            $("#disabledLCSPop .btn:first").removeAttr("disabled");
        } else {

            $("#disabledLCSPop .btn:first").attr("disabled", "disabled");
        }

 } 
 function ShowDisablePop(obj) 
    {
        DisabeAllCheckBox();

        var _obj=obj;
        overlay(_obj.id + "overlay");
     
        var curleft = curtop = 0;
        if (obj.offsetParent) {
            do
             {
	           
	            curtop += obj.offsetTop;
              }
              
            while (obj = obj.offsetParent);
            }
    	  //return [curleft,curtop];
    
    	curleft =73;
    	  curtop= curtop+ _obj.offsetHeight + 6;// - top.leftPanel.document.documentElement.scrollTop;
    	  $("#" + _obj.id + 'Pop').css("top",curtop + "px");
    	  $("#" + _obj.id + 'Pop').css("left", curleft + "px");
    	  $("#" + _obj.id + 'Pop').css("display", ($("#" + _obj.id + 'Pop').css("display") == 'inline') ? 'none' : 'inline');

   }

   function ShowDisablePopCS(obj) {
       
       DisabeAllCheckBox();

       var _obj = obj;
       overlay(_obj.id + "overlay");

       var curleft = curtop = 0;
       if (obj.offsetParent) {
           do {

               curtop += obj.offsetTop;
           }
           while (obj = obj.offsetParent);
       }
       //return [curleft,curtop];

       curleft = 73;
       curtop = curtop + _obj.offsetHeight + 6; // - top.leftPanel.document.documentElement.scrollTop;
       $("#" + _obj.id + 'Pop').css("top", curtop + "px");
       $("#" + _obj.id + 'Pop').css("left", curleft + "px");
       $("#" + _obj.id + 'Pop').css("display", ($("#" + _obj.id + 'Pop').css("display") == 'inline') ? 'none' : 'inline');

       if (!$("#" + _obj.id).hasClass("chkchecked")) {
           $("#" + _obj.id + 'boxDL').css("display", 'inline');
       }
       else {
           $("#" + _obj.id + 'boxDL').css("display", 'none');
       }

   }

   function ShowClosedPop(obj) {
      
       var _obj = obj;
       overlay(_obj.id + "overlay");
       var curleft = curtop = 0;
       if (obj.offsetParent) {
           do {
               curleft += obj.offsetLeft;
               curtop += obj.offsetTop;
           }

           while (obj = obj.offsetParent);
       }
       curleft = 73;
       curtop = curtop + _obj.offsetHeight + 6;
       $("#" + _obj.id + 'Pop').css("top", curtop + "px");
       $("#" + _obj.id + 'Pop').css("left", curleft + "px");
       $("#" + _obj.id + 'Pop').css("display", ($("#" + _obj.id + 'Pop').css("display") == 'inline') ? 'none' : 'inline');
     
   }
   function HideClosedPop(div) {

       $("#" + div).toggle();
       overlay(div.replace(/Pop/, "overlay"));
  }

   function DisabeAllCheckBox() {
    $("#disabledHRPop .btn:first").attr("disabled", "disabled");
    $("#disabledHROpenTotePop .btn:first").attr("disabled", "disabled");
    $("#disabledFinancialPop .btn:first").attr("disabled", "disabled");
    $("#disabledBingoGamePop .btn:first").attr("disabled", "disabled");
    $("#disabledNumberGamePop .btn:first").attr("disabled", "disabled");
    $("#disabledCSPop .btn:first").attr("disabled", "disabled");
    $("#disabledLCSPop .btn:first").attr("disabled", "disabled");
};

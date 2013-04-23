$(document).ready(function()
{
    $("#chkCopyAll").click(function(){
        var checkedStatus = this.checked;
        CopyMixedSportPT(checkedStatus); 
    });
    $("#chkCopyBasket").click(function(){
        var checkedStatus = this.checked;
        CopyMixedSportPT(checkedStatus);        
    });
    
    function CopyMixedSportPT(checkedStatus)
    {
        $("#tbl99_9 .copyall").attr('disabled', checkedStatus);        
        $("#tbl99_9 #txtMinbet99_9").attr('disabled', checkedStatus);
        $("#tbl99_9 #txtMaxbet99_9").attr('disabled', checkedStatus);
        $("#tbl99_9 #txtMaxPerMatch99_9").attr('disabled', checkedStatus);
        
        if(!checkedStatus)
        {           
            var IsAuto = $("#chkMixParlay").checked;
            if (IsAuto == undefined) 
            {
                IsAuto = $('#chkMixParlay').is(':checked');
                
                if(IsAuto == false)
                    IsAuto = $('#chkAutoMixParlay').is(':checked');                    
            }
            
            $('#listSMixParlay').attr('disabled', IsAuto);
            $('#autoSMixParlay').attr('disabled', !IsAuto);
            
            $('#listMMixParlayOnMember').attr('disabled', IsAuto);
            $('#autoMMixParlayOnMember').attr('disabled', !IsAuto);
            
        }
     }   

});
//listMMixParlay-listMMixParlayOnMember-chkAutoMixParlay
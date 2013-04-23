
$(window).load(
    function() {
        settleLog.selSettleLog();
    }
);


var settleLog = {
    matchId: '',
    fDate: '',
    tDate: '',
    selSettleLog: function() {
        settleLog.matchId = $('#txtMatchCode').val();
        settleLog.fdate = $('#fromdate').val();
        settleLog.tDate = $('#todate').val();
        
    	if(settleLog.matchId == '')
        	return;
    	
    	var data = { matchCode: settleLog.matchId, fDate: settleLog.fdate, tDate: settleLog.tDate };
        if (typeof (parent.BlockUI) == undefined)
            try { parent.parent.BlockUI(); } catch (e) { }
        else
            try { parent.BlockUI(); } catch (e) { }

        $('#settlementLog').html('');
        $.ajax({
            type: "POST",
            url: "Service/SettlementService.svc/SelSettleLogByMachCode",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: JSON.stringify(data),
            success: function(rs) {
                if (rs != null && rs.Rows != null && rs.Rows.length > 0) {
                    $('#scriptSettlementLog').tmpl(rs).appendTo($('#settlementLog').empty());
                }
                else {
                    $('#settlementLog').html('There is no information available.');
                }
            },
            error: function() {
                $('#settlementLog').html('There is no information available.');
            },
            complete: function() {
                $(".content-warper").removeClass("hidden").reRenderScroller(870, 350, true);

                if (typeof (parent.jQuery.unblockUI) == undefined)
                    try { parent.parent.jQuery.unblockUI(); } catch (e) { }
                else
                    try { parent.jQuery.unblockUI(); } catch (e) { }
            }
        });
    }
}
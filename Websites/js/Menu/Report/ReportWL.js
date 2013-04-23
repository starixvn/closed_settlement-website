function GetServerData() {
    
}

function WL2a() {
    var date = '2009-11-11';
    var db = 1;
    var params = '{"date":"' + date + '","dbtype":' + db + '}';
    
    $.ajax({
        type: "POST",
        url: "Services/WCF/Report/ReportWL.svc/WL2a",
        contentType: "application/json; charset=utf-8; text/xml",
        dataType: "json",
        data: params,
        success: function(rs) {
        //$('#ctrDownlines').html(rs);
            alert(rs);
        }
    });
}
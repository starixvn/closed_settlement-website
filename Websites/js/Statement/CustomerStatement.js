function reset_date(){
  var dayNoReset=7;
  var fdateId='fdate';
  var tdateId='tdate';
  var td = new Date();
  var fd=new Date(td.getTime()-dayNoReset*24*60*60*1000);
  document.getElementById(fdateId).value=(fd.getMonth()+1)+"/"+fd.getDate()+"/"+fd.getFullYear();
  document.getElementById(tdateId).value=(td.getMonth()+1)+"/"+td.getDate()+"/"+td.getFullYear();
  }

function obj(id){
	return document.getElementById(id);
}

function GetDataReport() {
    var fdate,tdate
    if (obj('fdate'))
    {
       fdate = obj('fdate').value ;
    }
    else
    {
       fdate = '';
    }
    if (obj('tdate'))
    {
       tdate = obj('tdate').value ;
    }
    else
    {
       tdate = '';
    }
    
    try{parent.BlockUI();}catch(e){}
    $.ajax({
    url: 'GetData.ashx?fdate=' + fdate + '&tdate=' + tdate +  '&repno=' + obj('repno').value,
    dataType:'html',
    success: function(data)
    {
        obj('sData').innerHTML = data;
        parent.jQuery.unblockUI();
    }
    });   
}
function SBD(transtype,fdate)
{
    url = 'StatementByDay.aspx?transtype=' + transtype + '&fdate=' + fdate;
    document.location.href = url;
}
function VBL(fdate)
{
    url = '../BetList/BetList.aspx?bltype=2&fdate=' + fdate;
    document.location.href = url;
}
function ViewStatement(repno)
{
    url = "CustomerStatement.aspx?repno=" + repno;
    document.location.href = url;
}

function ExportToExcel()
{
    var fdate = '';
    var tdate = '';
    if (obj('fdate'))
    {
        fdate = obj('fdate').value;
    }
    if (obj('tdate'))
    {
        tdate = obj('tdate').value;
    }
    
    location.href = 'CustomerStatement.aspx?fdate='+ fdate + '&tdate=' + tdate + '&repno=' + obj('repno').value + '&IsExport=1';
}

function DelTrans(refno)  
{
    if(confirm("Do you want to delete the transaction with Ref.No: "+refno +"?"))
    {
        if (confirm("To double check, do you want to delete the transaction with Ref.No: "+refno +"?"))
        {
            try{parent.BlockUI();}catch(e){}
            $.ajax({
                url: 'DelTrans.ashx?refno=' + refno,
                dataType: 'html',
                success: function(data) {
                    var arr = data.split('|');
                    if (arr[0] == "1") {
                        obj('sData').innerHTML = arr[1];
                        parent.jQuery.unblockUI();
                    }
                    else {
                        setTimeout("GetDataReport()", 3000);
                    }

                }
            });     
        }
    }

}
  
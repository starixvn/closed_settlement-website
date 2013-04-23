function ChangeType(rdo) {
    obj('isYesterday').value =(rdo.id =='alltrans') ? '0' :'1';
    document.forms[0].submit();
}

function LoadSuperBalance() {    
    setTimeout('ibcWnd.close()', 4000);
}


function GetData(IsExport) {

    IsExport = (IsExport) ? '1' : '0';
        var param = '&IsYesterDay=' +  ((obj('yestrans').checked) ? '1' : '0');                
        
        if(custid>0) 
        {
            if(isYesterday == '1')
            {
                obj('yestrans').checked = 'checked'; 
                obj('alltrans').checked = ''; 
            }
            else
            {
                obj('yestrans').checked = '';
                obj('alltrans').checked = 'checked';
            }                    
            obj('yestrans').disabled = 'disabled';
            obj('alltrans').disabled = 'disabled';
        }
        
        if(obj('dbRep'))
            param += '&IsRep=' + ((obj('dbRep').checked) ? '1' : '0') ;
        else
            param += '&IsRep=0' ;
        param += '&username=' + ((obj('txtSearch')) ?  obj('txtSearch').value : '');
        param += '&letter=' +  ((obj('listAlpha')) ?  obj('listAlpha').value : '');
        param += '&currency=' +  ((obj('listCurrency')) ? obj('listCurrency').value : '');
        param += '&custid=' + custid;
        param += '&roleid=' + roleid;
        param += '&IsExport=' + IsExport;

        parent.BlockUI();
        if (IsExport == '0') {
            $.ajax(
             {
                 type: "POST",
                 url: "SuperBalance.ashx",
                 data: param,
                 success: function(rs) {
                 obj('sData').innerHTML = rs;
                 //$("DIV#sData").html() = rs;
                 
                 InitFixedHeader(580, 'table', 'trFooter', 'trHeader');
                     try{parent.jQuery.unblockUI();}catch(e){}
                 }
             });                
        }
        else { //export to excel
            //alert('export');return;
            try{parent.jQuery.unblockUI();}catch(e){}
            window.location.href = 'SuperBalance.aspx?t=export' + param;
        }
}

function on_Blur()
{
    if(obj('txtSearch').value.length == 0)
        obj('txtSearch').value = 'All';
}

function On_Focus()
{
    if(obj('txtSearch').value == 'All')
        obj('txtSearch').value = '';
}
function On_KeyPress(e)
{
    if (!e) e = window.event;
    var code = (e.keyCode) ? e.keyCode : e.which; 				
    if (code==13) GetData();
}

function ExportToExcel() {
    GetData(1);
}

//Transfer.aspx?amount=78,500.14&isYesterday=1
function OpenPopupTransferBalance(fromCusId, fromUser, amount)
{
    var width = 550;
    var height = 200;
    var param = '&IsYesterDay=' + ((obj('yestrans').checked) ? '1' : '0') + '&isTransferFromSuperBalance=true';
    if (obj('dbRep'))
        param += '&IsRep=' + ((obj('dbRep').checked) ? '1' : '0');
    else
        param += '&IsRep=0';              
    
    if (parent.ibcWnd)
        parent.ibcWnd.open('../../Transfer.aspx?fcusid=' + fromCusId + '&fuser='+ fromUser + '&amount=' + amount  + param, '', 50, 100, width, height, 'class1', 'class2');
    else
        ibcWnd.open('../../Transfer.aspx?fcusid=' + fromCusId + '&fuser='+ fromUser + '&amount=' + amount  + param, '', 50, 100, width, height, 'class1', 'class2');
}
var datarows;
var total;
var isDisableAll = false;
var credit=0;
var balanceColor=1;
var balance=2;
var YesterdaybalanceColor=3;
var yesterdaybalance=4;
var outstanding=5;
var id=6;
var yesterdaybalance_value=7
var username=8;
var lastname=10;
var firstname = 9;
var chkdisable = 11;
var isclosed = 12;
var SyncCasino = 13;


function RenderCustBalance(data, totalrow, _isDisableAll) {
   
    var i = 0;
    var content = [];
    var strcontent = "";
    content.push('<form id="frmCustBal" name="frmCustBal" method="post">');
    content.push('<table class="tbllayout" cellspacing="1px">');
	content.push('<tr class="title">');
	content.push('<td colspan="5"></td>');
	content.push('<td align="center">');
	if (_isDisableAll)
	{
	    content.push('	<input class="btn" type="submit" value="Submit" disabled />');
	}
	else
	{
	    content.push('	<input class="btn" type="submit" id="submitQuery2" name="submitQuery2" value="Submit" onclick="return updateCheckList(\'frmCustBal\');" />');
	}
	content.push('</td>');
	content.push('<td colspan="3"></td>');
    content.push('</tr>');
    content.push('<tr class="title c">');
	content.push('<th width="30">No.</th>');
	content.push('<th width="110">Credit</th>');
	content.push('<th width="110">Balance</th>');
	content.push('<th width="110">Yesterday Balance</th>');
	content.push('<th width="110">Outstanding</th>');
	content.push('<th width="60">');
	if (_isDisableAll) {
	    content.push('  <input type="checkbox" id="_checkAll" name="_checkAll" disabled />');
	}
	else {
	    content.push('  <input type="checkbox" id="_checkAll" name="_checkAll" onclick="Bala_CheckAll(\'frmCustBal\',\'_checkAll\');"/>');
	}
	content.push('</th>');
	content.push('<th width="90">User Name</th>');
	content.push('<th width="120">First Name</th>');
	content.push('<th >Last Name</th>');
    content.push('</tr>');
    //Render Content
    for (i=0;i<totalrow;i++) {

        content.push('<tr class="' + data[i][isclosed] + '">');
        content.push('<td class="c">' + (i+1) + '</td>');
        content.push('<td class="r"><b>' + data[i][credit] + '</b></td>');
        content.push('<td class="r" style="color:' + data[i][balanceColor]+ ';"><b>' + data[i][balance] + '</b></td>');
        content.push('<td class="r" style="color:' + data[i][YesterdaybalanceColor]+ ';"><b>' + data[i][yesterdaybalance] + '</b></td>');
        content.push('<td class="r"><b>' + data[i][outstanding] + '</b></td>');
        content.push('<td class="c">');
        if (data[i][chkdisable] == "yes") {
            content.push('    <input SyncCasino="'+ data[i][SyncCasino] +'" type="checkbox" id="' + data[i][id] + '" name="' + data[i][id] + '" value="' + data[i][yesterdaybalance_value] + '" disabled />');
        }
        else {
            content.push('    <input SyncCasino="' + data[i][SyncCasino] + '" type="checkbox" id="' + data[i][id] + '" name="' + data[i][id] + '" value="' + data[i][yesterdaybalance_value] + '" onclick="_CheckItem(\'frmCustBal\',\'' + data[i][id] + '\');" />');
        }
        content.push('</td>');
        content.push('<td class="c">');
        content.push('  <a href="../CustInfo.aspx?username=' + data[i][username] + '" target="leftPanel">');
        content.push(data[i][username]);
        content.push('  </a>');
        content.push('</td>');
        content.push('<td class="c">' + data[i][firstname] + '</td>');
        content.push('<td class="c">' + data[i][lastname] + '</td>');
        content.push('</tr>');
    }
    content.push('<tr class="title">');
	content.push('<td colspan="5"></td>');
	content.push('<td align="center">');
	if (_isDisableAll)
	{
	    content.push('  <input class="btn" type="submit" value="Submit" disabled />');
	}
	else
	{
	    content.push('  <input class="btn" type="submit" id="submitQuery" name="submitQuery" value="Submit" onclick="return updateCheckList(\'frmCustBal\');" />');
	}
	content.push('	<input type="hidden" id="memberIdList" name="memberIdList" value="" />');
	content.push('</td>');
	content.push('<td colspan="3"></td>  ');
    content.push('</tr>');
    content.push('</table>');
    content.push('</form>');
    strcontent = content.join('');
    document.write(strcontent);
    try{parent.jQuery.unblockUI();}catch(e){}
}

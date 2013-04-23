function ExportToExcel()
{
    var ReportBy = GetReportByValue();
    var currency = getCurrency();
    if (obj('reportDetail').value==0) //main
    {
        location.href = 'ReportWinlossNumberGame.aspx?fdate=' + obj('fdate').value + '&tdate=' + obj('tdate').value + '&reportby=' + ReportBy + '&date=' + obj('dlDate').value + '&sport=' + obj('dlSport').value + '&currency=' + currency + '&IsExport=1' + '&reportdetail=0';
    }
    else //detail
    {
        location.href = 'ReportWinlossNumberGame.aspx?fdate=' + obj('fdate').value + '&tdate=' + obj('tdate').value + '&sport=' + obj('dlSport').value + '&currency=' + currency + '&IsExport=1' + '&reportdetail=1' + '&currencyname=' + obj('currencyName').value;
    }
}
function getCurrency()
{
   var dlCurrency = obj('dlCurrency');
   if (dlCurrency.value == 0 || dlCurrency.value == -1 || dlCurrency.value == -2) //-1: Base Currency USD, -2: Base Currency RM
   {
     return dlCurrency.value;
   }
   else //return CustID
   {
      var arr = dlCurrency.value.split('_')  ;
      return arr[1];
   }
}
function getCurrencyById(id)
{
    var currencyOption = obj('currencyOption').value;
    if (currencyOption=='-1' || currencyOption == '-2')//-1: Base Currency USD, -2: Base Currency RM
    {
        return currencyOption;
    }
    else
    {
        var count = dlCurrency.options.length;
        var arr;
        for (var i=0;i<count;i++)
        {
            arr = dlCurrency.options[i].value.split('_');
            if (id==arr[0])
                return arr[1];
        }
    }
    return id;
}
function DoReset()
{
    obj('dlDate').selectedIndex = 0;
    obj('dlSport').selectedIndex = 0;
    obj('dlCurrency').selectedIndex = 0;
    GetDateTime(0);
}
function ResetAll()
{
    //obj('inProgressBar').style.display='none';
    obj('dateOption').value = '';
    obj('sportOption').value = '';
    obj('currencyOption').value = '';
    obj('reportByOption').value = '';
    obj('reportDetail').value = 0;
    obj('fromdate').value = '';
    obj('todate').value = '';
    obj('GoBackBtn').style.display = 'none';
    obj('currencyIndex').value = '';
    
    obj('currencyName').value = '';
    obj('fromdatedetail').value = '';
    obj('todatedetail').value = '';
    obj('sportOptionDetail').value = '';
    obj('currencyOptionDetail').value = '';
    
    obj('dlDate').selectedIndex = 0;
    obj('dlSport').selectedIndex = 0;
    obj('dlCurrency').selectedIndex = 0;
    GetDateTime(0);
}
function daysInMonth(month,year) {
    var m = [31,28,31,30,31,30,31,31,30,31,30,31];
    if (month != 2) return m[month - 1];
    if (year%4 != 0) return m[1];
    if (year%100 == 0 && year%400 != 0) return m[1];
    return m[1] + 1;
} 

//this happen when click a currency Name in the grid to view detail
function viewDetail(currName,currid,reportType)
{
    try{parent.BlockUI();}catch(e){}
    var dateOption = obj('dateOption').value;
    var sportOption = -1;
    var currencyOption = obj('currencyOption').value;
    var reportByOption = obj('reportByOption').value;
    var dlCurrency = obj('dlCurrency');
    var dlSport = obj('dlSport');
    var dlDate = obj('dlDate');
    var fdate = obj('fdate');
    var tdate = obj('tdate');
    var currency = '0';
    var fromdate = obj('fromdate').value;
    var todate = obj('todate').value;

    dlDate.selectedIndex = 8 //Specific date
    if (reportByOption == 1)//daily
    {
        //input fdate, tdate textbox
        fdate.value = reportType;
        tdate.value = reportType;    
        
        fromdate = fdate.value;
        todate = tdate.value;
        sportOption = obj('sportOption').value;
    }
    else if (reportByOption == 2)//montly
    {
        var myarr = reportType.split('/');
        var month = myarr[0];
        var year = myarr[1];
        var day = daysInMonth(month,year);
        fdate.value = month + '/01/' + year;
        tdate.value = month + '/' + day + '/' + year;
        
        fromdate = fdate.value;
        todate = tdate.value;
        sportOption = obj('sportOption').value;
    }
    else if (reportByOption==3) // Sport
    {
        fromdate = obj('fromdate').value;
        todate = obj('todate').value;
        for (var i=0;i<dlSport.options.length;i++)
        {
            if (reportType==dlSport.options[i].value)
            {
                dlSport.options[i].selected = true;
            }
        }
        sportOption = reportType;
    }
    
    //input currency option
    if (currencyOption== -1)
    {
        dlCurrency.selectedIndex = 1;
        currency = '-1';
    }
    else if (currencyOption==-2)
    {
        dlCurrency.selectedIndex = 2;
        currency = '-2';
    }
    else
    {
        var count = dlCurrency.options.length;
        var arr;
        for (var i=0;i<count;i++)
        {
            arr = dlCurrency.options[i].value.split('_');
            if (currid == arr[0])
            {
                dlCurrency.options[i].selected = true;
                currency = arr[1];
                break;
            }
        }
        
    }

    $.ajax({
    url: 'GetData.ashx?mode=30'  + '&sport=' + sportOption + '&fdate=' + fromdate + '&tdate=' + todate + '&currency=' + currency  + '&reportdetail=1' + '&currencyname=' + currName,
    dataType:'html',
    success: function(data)
    {
        obj('sData').innerHTML = data;
        
        //input hidden field
        obj('reportDetail').value = 1;
        obj('currencyName').value = currName;
        obj('GoBackBtn').style.display = 'inline';
        obj('fromdatedetail').value = fromdate;
        obj('todatedetail').value = todate;
        obj('sportOptionDetail').value = sportOption;
        obj('currencyOptionDetail').value = currency;
        try{parent.jQuery.unblockUI();}catch(e){}
        Init(990);
    }
    });   
    
}

function GoBack()
{
  //dlDate
  obj('dlDate').selectedIndex =   obj('dateOption').value;
  
  //fromdate, todate
  obj('fdate').value = obj('fromdate').value;
  obj('tdate').value = obj('todate').value;
  
  //report By
  var reportByOption =  obj('reportByOption').value
    
  if (reportByOption==1)
    obj('rdDaily').checked=true
  else if (reportByOption==2)
    obj('rdMonthly').checked=true
  else
    obj('rdSport').checked=true
    
  
  //dl Sport
  var sportOption = obj('sportOption').value
  if (sportOption==-1)
    obj('dlSport').selectedIndex = 0;
  else if (sportOption==0)
    obj('dlSport').selectedIndex = 1;
  else
    obj('dlSport').selectedIndex = 2;
    
  //dl Currency  
  obj('dlCurrency').selectedIndex = obj('currencyIndex').value;
  
  
  obj('reportDetail').value = 0;
  GetDataReport(1);
}
function getBL(custid,username)
{
    var showSB = 1;
    var showHR = 1;
    var sportOption = obj('sportOptionDetail').value;
    var currencyOption = obj('currencyOptionDetail').value;
    if (sportOption == '-1') //Show all
    {
        showSB = 1;
        showHR = 1;
    }
    else if (sportOption == '0') //SB
    {
        showSB = 1;
        showHR = 0;
    }
    else if (sportOption == '151') //Racing
    {
        showSB = 0;
        showHR = 1;
    }
    var baseCurrency = '';
    if (currencyOption=='-1' ) //base usd
        baseCurrency = 'usd';
    else if (currencyOption ==  '-2') //base rm
        baseCurrency = 'rm';
    else
        baseCurrency = 'none';
    
    var url = '../BetList/BetListFun88.aspx?custid=' + custid + '&username=' + username + '&fdate=' + obj('fromdatedetail').value + '&tdate=' + obj('todatedetail').value + '&showSB=' + showSB + '&showHR=' + showHR + '&baseCurrency=' + baseCurrency;
    
    var NewWindow = window.open(url,'BetList',' scrollbars=yes, toolbar=no, menubar=no,location=no,status=no,resizable=yes');
    if (NewWindow) NewWindow.focus();
}

function GetDateTime(vl)
{
    if (vl!=8)
    {
       $.ajax({
        url: 'GetData.ashx?mode=23&date=' + vl ,
        dataType:'html',
        async: false,
        success: function(data)
        {
            var arr = data.split("|");        
            obj('fdate').value = arr[0];
            var dt1 = arr[0].split("/");
            cal1.selection.set(dt1[2] + dt1[0] + dt1[1]   );
            obj('tdate').value = arr[1];
            var dt2 = arr[1].split("/");
            cal2.selection.set(dt2[2] + dt2[0] + dt2[1])
        }
        });  
    }
    
}

function GetDataReport(isBack)
{
    if (obj('reportDetail').value == 1)
    {
        GetReportDetail();
    }
    else
    {
        var dlDate = obj('dlDate');
        if (dlDate.value==0)
        {
            GetDateTime(0);
            dlDate.selectedIndex = 1;
        }
        else if (dlDate.value!=8)
        {
            GetDateTime(dlDate.value);
        }

        var ReportBy = GetReportByValue();
        var currency = getCurrency();
        try{parent.BlockUI();}catch(e){}
        $.ajax({
        url: 'GetData.ashx?mode=30&date=' + obj('dlDate').value + '&sport=' + obj('dlSport').value + '&fdate=' + obj('fdate').value + '&tdate=' + obj('tdate').value + '&currency=' + currency + '&reportby=' + ReportBy + '&reportdetail=0' + '&isBack=' + isBack,
        dataType:'html',
        success: function(data)
        {
            obj('sData').innerHTML = data;
            
            //input hidden field
            obj('dateOption').value = obj('dlDate').value;
            obj('sportOption').value = obj('dlSport').value;
            obj('currencyOption').value = obj('dlCurrency').value;
            obj('reportByOption').value = ReportBy;
            obj('reportDetail').value = 0;
            obj('fromdate').value = obj('fdate').value;
            obj('todate').value = obj('tdate').value;
            obj('GoBackBtn').style.display = 'none';
            obj('currencyIndex').value = obj('dlCurrency').selectedIndex;
            try{parent.jQuery.unblockUI();}catch(e){}
            Init(990);
        }
        });   
    }

    
}
function GetReportDetail()
{
    var dlCurrency = obj('dlCurrency');
    //check if not choose currency, force to choose
    if (dlCurrency.value == '0')
    {
        alert('Please choose a currency!');
        return;
    }
    
    //set fdate, tdate
    var dlDate = obj('dlDate');
    if (dlDate.value==0)
    {
        GetDateTime(0);
        dlDate.selectedIndex = 1;
    }
    else if (dlDate.value!=8)
    {
        GetDateTime(dlDate.value);
    }
    
    //get report by
    var ReportBy = GetReportByValue();
    
    //get currency
    var currency, currName
   if (dlCurrency.value == -1) //-1: Base Currency USD, -2: Base Currency RM
   {
     currency = '-1';
     currName = 'US$';
   }
   else if (dlCurrency.value == -2)
   {
     currency = '-2';
     currName = 'RM';
   }
   else //return CustID
   {
      var arr = dlCurrency.options[dlCurrency.selectedIndex].value.split('_');
      currency = arr[1];
      currName = dlCurrency.options[dlCurrency.selectedIndex].text;
   }
    
   try{parent.BlockUI();}catch(e){}
   $.ajax({
    url: 'GetData.ashx?mode=30'  + '&sport=' + obj('dlSport').value + '&fdate=' + obj('fdate').value + '&tdate=' + obj('tdate').value + '&currency=' + currency  + '&reportdetail=1' + '&currencyname=' + currName,
    dataType:'html',
    success: function(data)
    {
        obj('sData').innerHTML = data;
        
        //input hidden field
        obj('reportDetail').value = 1;
        obj('currencyName').value = currName;
        obj('GoBackBtn').style.display = 'inline';
        obj('fromdatedetail').value = obj('fdate').value;
        obj('todatedetail').value = obj('tdate').value;
        obj('sportOptionDetail').value = obj('dlSport').value;
        obj('currencyOptionDetail').value = currency;
        try{parent.jQuery.unblockUI();}catch(e){}
        Init(990);
    }
    });    
    
}
function GetReportByValue()
{
    if (obj('rdDaily').checked==true)
        return 1;
    else if (obj('rdMonthly').checked==true)
        return 2;
    else if (obj('rdSport').checked==true)
        return 3;
}

window.onresize = Resize;

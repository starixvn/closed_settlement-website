//format number
//@PhuongDN
//example
//var nr = 1345652345.67890;
//formatNumber(nr ,2,',','.','','','',' CR')-->1,345,652,345.68
function formatNumber(num, dec, thou, pnt, curr1, curr2, n1, n2) {
    var sign = '';
    if (num < 0) sign = '-';
    var x = Math.round(num * Math.pow(10, dec));
    if (x >= 0) n1 = n2 = '';
    var y = ('' + Math.abs(x)).split('');
    var z = y.length - dec;
    y.splice(z, 0, pnt);
    while (z > 3) {
        z -= 3;
        y.splice(z, 0, thou);
    }
    var r = curr1 + n1 + y.join('') + n2 + curr2;
    if (num > -1 && num < 1 && r.indexOf('.') == 0) r = "0" + r;
    return sign + r;
    return num;

}

//format number to form: #,###,###.00
function formatNumber2(nr) {
    return formatNumber(nr, 2, ',', '.', '', '', '', '');
}

function RenderMatchWL2HTML() {
    var trs = new Array();

    var str = '';
    var matchcode = 0;
    var eventdate = 1;
    var kickofftime = 2;
    var liveindicator = 3;
    var bettype = 4;
    var homeid = 5;
    var awayid = 6;
    var winlost = 7;
    var pos = 8;
    var comm = 9;
    var VirtualComm = 10;
    var LineTotal = 11;
    var VirtualLineTotal = 12;
    var leaguenametc = 13;
    var IsNeutral = 14;
    
    var grandtotal = 0, com = 0, grandvirtual = 0, grandvariance = 0, comvirtual = 0;
    var grandtotalbet = 0, combet = 0, grandvirtualbet = 0, grandvariancebet = 0, comvirtualbet = 0;
    var grandtotalbetb = 0, combetb = 0, grandvirtualbetb = 0, grandvariancebetb = 0, comvirtualbetb = 0;
    
    var bettypename = '';
    var llive = 0;
    var oldleague = '';
    var index = 0;
    var color1 = '', color2='';
    var leaguename = '';
    var db = ($('#rdMain').length > 0) ? (($('#rdMain').is(':checked')) ? 'main' : 'rep') : 'rep';


    trs.push("<table id='table' class='tbllayout' cellspacing='1' width='1120px' >");
    trs.push("<thead>");
    trs.push("<tr id='trHeader' ><td colspan='12' style='background-color:#ffabab;'><b>Match Win Loss in " + $('#fdate').val() + "</b></td></tr>");
    trs.push("<tr class='title' style='height:25px'>");   
    trs.push("<th>Bet Type</th>");
    trs.push("<th>Date</th>");
    trs.push("<th colspan='3'>Match</th>");
    trs.push("<th style='width:80px'>Winloss</th>");
    trs.push("<th style='width:80px'>Position</th>");
    trs.push("<th style='width:80px'>Comm</th>");
    trs.push("<th style='width:80px'>Virtual <br/> Comm</th>");
    trs.push("<th style='width:80px'>Line Total</th>");
    trs.push("<th style='width:80px'>Virtual <br/> Line Total</th>");
    trs.push("<th style='width:80px'>Variance</th>");
    trs.push("</tr>");
    trs.push("</thead>");
    
    trs.push("<tbody>");

    for (i = 0; i < cnt; i++) {
    
        //begin total footer
        comvirtual += parseFloat(MatchWL[i][VirtualComm]);
        com += parseFloat(MatchWL[i][comm]);
        grandtotal += parseFloat(MatchWL[i][LineTotal]);
        grandvirtual += parseFloat(MatchWL[i][VirtualLineTotal]); 
        grandvariance += parseFloat(MatchWL[i][LineTotal]) - parseFloat(MatchWL[i][VirtualLineTotal]); 
        
        //end total footer  

        //begin bettype or live
        if (!(MatchWL[i][bettype] == bettypename) 
            || (MatchWL[i][liveindicator] != llive)
            || (MatchWL[i][leaguenametc] != oldleague)
            )
        {
            bettypename = MatchWL[i][bettype];
            llive = MatchWL[i][liveindicator];
            if (index != 0)
            {                
                color1 = "#efefef"; color2 = "#dedede";                
                trs.push(ToTalRow(combet, comvirtualbet, grandtotalbet, grandvirtualbet, grandvariancebet, "", color1, color2));                
                combet = 0; grandtotalbet = 0; comvirtualbet = 0; grandvirtualbet = 0; grandvariancebet = 0;
            }
            index++;
        }
        //begin bettype or live
        
        //begin leaguename
        leaguename = MatchWL[i][leaguenametc];
        if (leaguename != oldleague)
        {
            if (i > 1)
            {
                color1 = "#deffde"; color2 = "#cdffcd";
                trs.push(ToTalRow(combetb, comvirtualbetb, grandtotalbetb, grandvirtualbetb, grandvariancebetb, oldleague.toUpperCase(), color1, color2));                
                combetb = 0; grandtotalbetb = 0; grandvirtualbetb = 0; grandvariancebetb = 0; comvirtualbetb = 0;
            }
            oldleague = leaguename;
        }
        // end leaguename
        
        
        //begin total of bettype or live
        combet += parseFloat(MatchWL[i][comm]); 
        comvirtualbet += parseFloat(MatchWL[i][VirtualComm]); 
        grandtotalbet += parseFloat(MatchWL[i][LineTotal]); 
        grandvirtualbet += parseFloat(MatchWL[i][VirtualLineTotal]); 
        grandvariancebet += parseFloat(MatchWL[i][LineTotal]) - parseFloat(MatchWL[i][VirtualLineTotal]);
        //end total of bettype or live

        //begin total of league
        combetb += parseFloat(MatchWL[i][comm]);
        comvirtualbetb += parseFloat(MatchWL[i][VirtualComm]);
        grandtotalbetb += parseFloat(MatchWL[i][LineTotal]); 
        grandvirtualbetb += parseFloat(MatchWL[i][VirtualLineTotal]); 
        grandvariancebetb += parseFloat((MatchWL[i][LineTotal]) - parseFloat(MatchWL[i][VirtualLineTotal])); 
        //end total of league
        
        
        //begin earch row
        var strRow = '';
        trs.push("<tr style='height:30px;' bgcolor='white'>");
        var strbettype = "";
        if(MatchWL[i][liveindicator] == '1')
            strbettype += "<b>!Live- </b>";
        strbettype += MatchWL[i][bettype] + "<br/>" + MatchWL[i][matchcode] ;
        
        trs.push("<td>"+ strbettype +"</td>");
        trs.push("<td>"+ MatchWL[i][eventdate] + "<br/>" + MatchWL[i][kickofftime] + "</td>");
        
        if(MatchWL[i][bettype] == 10)
            trs.push("<td colspan='3' align='center'>"+ MatchWL[i][homeid] +"</td>");
        else
        {
            trs.push("<td class='r'>" + MatchWL[i][homeid] + ((MatchWL[i][IsNeutral] == '1') ? 'N' : '') + "</td>");
            trs.push("<td>-vs-</td>"); 
            trs.push("<td align='left'>"+ MatchWL[i][awayid] +"</td>");
        }
        
        trs.push("<td class='r'>"+ formatNumber2(MatchWL[i][winlost]) +"</td>");
        trs.push("<td class='r'>"+ formatNumber2(MatchWL[i][pos]) +"</td>");
        trs.push("<td class='r'>"+ formatNumber2(MatchWL[i][comm]) +"</td>");
        trs.push("<td class='r'>"+ formatNumber2(MatchWL[i][VirtualComm]) +"</td>");
        trs.push("<td class='r'><b>"+ formatNumber2(MatchWL[i][LineTotal]) +"<b/></td>");
        trs.push("<td class='r'><b>"+ formatNumber2(MatchWL[i][VirtualLineTotal]) +"</b></td>");
        trs.push("<td class='r rspace'>" + formatNumber2(parseFloat(MatchWL[i][LineTotal]) - parseFloat(MatchWL[i][VirtualLineTotal])) + "</td>");
        trs.push("</tr>");

        
    }

    //begin bettype or live
    color1 = ""; color2 = "";
    trs.push(ToTalRow(combet, comvirtualbet, grandtotalbet, grandvirtualbet, grandvariancebet, "", color1, color2));
    combet = 0; grandtotalbet = 0; comvirtualbet = 0; grandvirtualbet = 0; grandvariancebet = 0;
    //end bettype or live

    //begin leaguename
    color1 = "#deffde"; color2 = "";
    trs.push(ToTalRow(combetb, comvirtualbetb, grandtotalbetb, grandvirtualbetb, grandvariancebetb, oldleague.toUpperCase(), color1, color2));
    //end leaguename

    //begin grand total 1
    color1 = "#88aaff"; color2 = "#cdffcd";
    trs.push(ToTalRow(com, comvirtual, grandtotal, grandvirtual, grandvariance, 'GRAND', color1, color2));
    //end grand total 1

    str += trs.join('');

    //begin winlost_8rm
    grandtotal += winlost_8rm;
    grandvirtual += winlost_8rm;
    trs.push("<tr>");
    trs.push("<td colspan='9' class='r'><b>8RM Total:<b/></td>");
    trs.push("<td class='r'><b>" + formatNumber2(winlost_8rm) + "<b/></td>");
    trs.push("<td class='r'><b>" + formatNumber2(winlost_8rm) + "<b/></td>");
    trs.push("<td class='r rspace'>0.0</td>");
    trs.push("</tr>");
    //end winlost_8rm

    //Begin Mixparlay - OutRigth - FI - HR - 12BET - 3rdparty - Casino;
    eventdate = 0;
    winlost = 1;
    pos = 2;
    comm = 3;
    VirtualComm = 4;
    LineTotal = 5;
    VirtualLineTotal = 6;
    bettype = 7;

    var parlayGrandTotal = 0, parlayTotal = 0, parlayCommission = 0, parlayVirtualCommission = 0, parlayVirtualTotal = 0, parlayVarianceTotal = 0;
    var palayvariance = 0;
    
    for (i = 0; i < cnt1; i++) {
        switch (MatchWL1[i][bettype].toLowerCase()) {
            case 'fodds hr':
                bettypename = "<a href='FOddsHRWLDetails.aspx?wldate=" + MatchWL1[i][eventdate] + "&bettype=" + MatchWL1[i][bettype] + "&db=" + db + "' target='_blank'>" + MatchWL1[i][bettype] + "</a>";
                break;
            case 'tote hr':
                bettypename = "<a href='ToteHRWLDetails.aspx?wldate=" + MatchWL1[i][eventdate] + "&bettype=" + MatchWL1[i][bettype] + "&db=" + db + "' target='_blank'>" + MatchWL1[i][bettype] + "</a>";
                break;
            case 'finance':
                bettypename = "<a href='FIWLDetails_1.aspx?wldate=" + MatchWL1[i][eventdate] + "&bettype=" + MatchWL1[i][bettype] + "&db=" + db + "' target='_blank'>" + MatchWL1[i][bettype] + "</a>";
                break;
            case 'casino':
            case 'p2p':
                bettypename = "<a href='CasinoDetail_1.aspx?wldate=" + MatchWL1[i][eventdate] + "&bettype=" + MatchWL1[i][bettype] + "&db=" + db + "' target='_blank'>" + MatchWL1[i][bettype] + "</a>";
                break;
            case 'casino mini game':
                bettypename = "<a href='MiniGamesDetail_1.aspx?wldate=" + MatchWL1[i][eventdate] + "&db=" + db + "' target='_blank'>" + MatchWL1[i][bettype] + "</a>";
                break;
                
            default:
                bettypename = MatchWL1[i][bettype];
                break;
        }

        if (MatchWL1[i][bettype].toLowerCase() == '12bet agents' || MatchWL1[i][bettype].toLowerCase() == 'fi 3rd-party' || MatchWL1[i][bettype].toLowerCase() == 'finance') {
            if (parseFloat(MatchWL1[i][LineTotal]) == 0 && parseFloat(MatchWL1[i][VirtualLineTotal]) == 0) {
                continue;
            }
            if (MatchWL1[i][bettype].toLowerCase() == '12bet agents') continue;
            if (MatchWL1[i][bettype].toLowerCase() == 'fi 3rd-party') {
                bettypename = "<div title='Finance 3rd-Party Share'>" + MatchWL1[i][bettype] + "</div>";
            }
        }

        trs.push("<tr bgcolor='white'>");
        trs.push("<td class='c'>" + bettypename + "</td>");
        trs.push("<td class='c'>" + MatchWL1[i][eventdate] + "</td>");
        trs.push("<td class='c' colspan='3'></td>");
        trs.push("<td class='r'>" + formatNumber2(MatchWL1[i][winlost]) + "</td>");
        trs.push("<td class='r'>" + formatNumber2(MatchWL1[i][pos]) + "</td>");
        trs.push("<td class='r'>" + formatNumber2(MatchWL1[i][comm]) + "</td>");

        var strTD_VirtualComm = '', strTD_VirtualLineTotal = '', strTD_Variance = '';
        switch (MatchWL1[i][bettype].toLowerCase()) {
            case 'fodds hr':
            case 'tote hr':
            case 'finance':
            case 'casino':
            case 'p2p':
            case 'fi 3rd-party':
                strTD_VirtualComm = strTD_VirtualLineTotal = "<td class='r'>N/A</td>";
                strTD_Variance = "<td class='r rspace'>N/A</td>";
                break;
            default:
                parlayVirtualTotal += parseFloat(MatchWL1[i][VirtualLineTotal]);
                parlayVirtualCommission += parseFloat(MatchWL1[i][VirtualComm]);
                palayvariance = parseFloat(MatchWL1[i][LineTotal]) - parseFloat(MatchWL1[i][VirtualLineTotal]);
                parlayVarianceTotal += palayvariance;                
            
                strTD_VirtualComm = "<td class='r'>" + formatNumber2(MatchWL1[i][VirtualComm]) + "</td>";
                strTD_VirtualLineTotal = "<td class='r'>" + formatNumber2(MatchWL1[i][VirtualLineTotal]) + "</td>";
                strTD_Variance = "<td class='r rspace'>" + formatNumber2(palayvariance) + "</td>";
                break;
        }

        parlayCommission += parseFloat(MatchWL1[i][comm]); 
        parlayGrandTotal += parseFloat(MatchWL1[i][LineTotal]); 

        trs.push(strTD_VirtualComm);
        trs.push("<td class='r'><b>" + formatNumber2(MatchWL1[i][LineTotal]) + "</b></td>");
        trs.push(strTD_VirtualLineTotal);
        trs.push(strTD_Variance);
        trs.push("</tr>");

    }    
    //End Mixparlay - OutRigth - FI - HR - 12BET - 3rdparty - Casino;

    trs.push("</tbody>");

    //begin Grand total footer
    var TotalComMP = com + parlayCommission;
    var TotalComVirtualMP = comvirtual + parlayVirtualCommission;
    var TotalLineMP = grandtotal + parlayGrandTotal;
    var TotalVirtalLineMP = grandvirtual + parlayVirtualTotal;
    var TotalVarianceMP = grandvariance + parlayVarianceTotal;
    //end Grand total footer

    trs.push("<tfoot>");
    trs.push(GrandTotal(TotalComMP, TotalComVirtualMP, TotalLineMP, TotalVirtalLineMP, TotalVarianceMP, 'GRAND ', '#88aaff', '#88aaff', 'trFooter'));
    trs.push("</tfoot>");
    trs.push("</table>");

    str = trs.join('');

    $('#spMatchWL').html(str);

    FixedHeader(1120);
}



function ToTalRow(Comm, VirtualComm, LineTotal, VirtualLineTotal, Variance, strTitleTotal, color1, color2) {
    
    var str = '';
    str += "<tr align='center'>";
    str += "<td colspan ='7' class='r' style='background-color:" + color1 + "'><b>"+ strTitleTotal +" TOTAL:</b></td>";
    str += "<td class='r' style='background-color:" + color1 + "'>"+ formatNumber2(Comm) +"</td>";
    str += "<td class='r' style='background-color:" + color1 + "''>"+ formatNumber2(VirtualComm) +"</td>";
    str += "<td class='r' style='background-color:" + color1 + "'><b>"+ formatNumber2(LineTotal) +"</b></td>";
    str += "<td class='r' style='background-color:" + color1 + "'><b>"+ formatNumber2(VirtualLineTotal) +"</b></td>";
    str += "<td class='r rspace'  style='background-color:" + color1 + "'>" + formatNumber2(Variance) + "</td>";
    str += "</tr>";

    
    return str;
}

function GrandTotal(Comm, VirtualComm, LineTotal, VirtualLineTotal, Variance, strTitleTotal, color1, color2, id) {

    var str = '';
    str += "<tr id='" + id + "' >";
    str += "<td colspan ='7' class='r' style='background-color:#88AAFF'><b>" + strTitleTotal + " TOTAL:</b></td>";
    str += "<td class='r' style='background-color:#88AAFF'>" + formatNumber2(Comm) + "</td>";
    str += "<td class='r' style='background-color:#88AAFF'>" + formatNumber2(VirtualComm) + "</td>";
    str += "<td class='r' style='background-color:#88AAFF'><b>" + formatNumber2(LineTotal) + "</b></td>";
    str += "<td class='r' style='background-color:#88AAFF'><b>" + formatNumber2(VirtualLineTotal) + "</b></td>";
    str += "<td class='r rspace' style='background-color:#88AAFF'>" + formatNumber2(Variance) + "</td>";
    str += "</tr>";
    return str;
}


//for report 2a match wl
function FixedHeader() {
    var widthmainpage = 1120;
    if ($('#trFooter').length == 0) return;

    $('#trFooter').show();
    var topHeader = $('#trHeader').position().top;
    var topFooter = $('#trFooter').position().top;
    var RealHeight = $(window).height() - 100;

    if ((topFooter - topHeader) > $(window).height()) {
        jQuery('#table').Scrollable(RealHeight, widthmainpage);
    }
    else {
        $('#table .rspace').removeClass('rspace');
    }

}


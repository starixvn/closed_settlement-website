// JScript File

function reSelectGroupId(check)
{
    var groupid = document.getElementById("cbogroup").value;
    var frm = document.getElementById('frmterm');
    frm.src = "TermController.aspx?groupid=" + groupid + "&check="  + check ;
}

function setvalue(check)
{
    var f = frames["frmterm"] ;        
    var grade = obj('cbograde');
    if(f.groupid == 0)
    {
        /*obj('txtPreAmt').disabled = true;
        obj('txtInsAmt').disabled = true;
        obj('s_currency').disabled = true;        
        obj('day').disabled = true;        
        obj('remark').disabled = true;        
        obj('cbograde').disabled = true; */       
    } 
    else
    {
        /*obj('txtPreAmt').disabled = false;
        obj('txtInsAmt').disabled = false;
        obj('s_currency').disabled = false;        
        obj('day').disabled = false;        
        obj('remark').disabled = false;        
        obj('cbograde').disabled = false;*/        
        
        if(check == '0') // if group head line is checked then just load checker
        {
            for(i = 0;i <grade.length; i++)
            {
                if(grade.options[i].value == f.creditgrade)
                {
                    grade.options[i].selected = true;
                    break;
                }
            }
            obj('txtPreAmt').value = f.pre;
            obj('txtInsAmt').value = f.installamt;
            obj('s_currency').innerHTML = f.curr;
            obj('remark').value = f.remark;
            
            var day = obj('day');
            for(i = 0; i<day.length; i++)
            {
                if(day.options[i].value == f.installdate)
                {
                    day.options[i].selected = true;
                    break;
                }    
            }
        }
        
        var CheckerList = obj('ddlCheckerList');               
        for(i = 0; i<CheckerList.length; i++)
        {
            if(CheckerList.options[i].value == f.checkerid)
            {
                CheckerList.options[i].selected = true;
            }
        }       
        
        if(!obj('groupheadline').checked )//uncheck 
        {
            obj('ddlCheckerList').options[0].selected = true;
        }
    }
}

function SetChecker(elemt, check) {
    //elemt.checked=!elemt.checked;

    var isHeadLine = elemt.checked;
    //alert(isHeadLine);
    reSelectGroupId(check);
    
    var CheckerList = obj('ddlCheckerList');               
    var f = frames["frmterm"] ;        
    
    //var id = (checkerid) ? checkerid : f.checkerid;
    //alert('checkerid1 = ' + checkerid + '\ncheckerid = ' + f.checkerid);
    
    var id = f.checkerid;

    CheckerList.disabled = !isHeadLine;  //!(elemt.checked);

    if (!isHeadLine)//uncheck 
    {
        obj('ddlCheckerList').options[0].selected = true;
    }
    else//check must be restore status of checker for group
    {   
        for(i = 0; i<CheckerList.length; i++)
        {
            if(CheckerList.options[i].value == id)
            {
                CheckerList.options[i].selected = true;
            }
        }
    }
    
}
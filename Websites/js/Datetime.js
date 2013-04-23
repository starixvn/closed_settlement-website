//Render Montly Drop down box
function RenderMonthly(selectControlId, startYear, currentYear, currentMonth) {
    var objSelectControl = $("#" + selectControlId);
    objSelectControl.find("option").remove();
    var date = new Date();

  //  alert('startYear: ' + startYear + '\ncurrentYear: ' + currentYear + '\ncurrentMonth: ' + currentMonth);

    if ((typeof currentMonth == 'undefined') || currentMonth == null) {

        currentMonth = date.getMonth() + 1;
    }

    if ((typeof currentYear == 'undefined') || currentYear == null) {

        currentYear = date.getYear();
        if (currentYear < 1000)
            currentYear += 1900;
    }

    if (startYear > currentYear)
        return;

    for (var year = currentYear; year >= startYear; year--) {
        if (startYear == currentYear) {
            for (var month = currentMonth; month >= 1; month--) {
                if (month < 10) {
                    if (month == currentMonth) {
                        objSelectControl.append("<option value='0" + month + "_" + year + "' selected='selected'>0" + month + "/" + year + "</option>")
                    }
                    else {
                        objSelectControl.append("<option value='0" + month + "_" + year + "'>0" + month + "/" + year + "</option>")
                    }
                }
                else {
                    if (month == currentMonth) {
                        objSelectControl.append("<option value='" + month + "_" + year + "' selected='seleted'>" + month + "/" + year + "</option>")
                    }
                    else {
                        objSelectControl.append("<option value='" + month + "_" + year + "'>" + month + "/" + year + "</option>")
                    }
                }
            }
        }
        else {
            var optgroup = "<optgroup label=" + year + ">";
            if (year == currentYear) {
                for (var month = currentMonth; month >= 1; month--) {
                    if (month < 10) {
                        if (month == currentMonth) {
                            optgroup += "<option value='0" + month + "_" + year + "'  selected='selected'>0" + month + "/" + year + "</option>";
                        }
                        else {
                            optgroup += "<option value='0" + month + "_" + year + "'>0" + month + "/" + year + "</option>";
                        }
                    }
                    else {
                        if (month == currentMonth) {
                            optgroup += "<option value='" + month + "_" + year + "' selected='selected'>" + month + "/" + year + "</option>";
                        }
                        else {
                            optgroup += "<option value='" + month + "_" + year + "'>" + month + "/" + year + "</option>";
                        }
                    }


                }
            }
            else {
                for (var month = 12; month >= 1; month--) {
                    if (month < 10) {

                        optgroup += "<option value='0" + month + "_" + year + "'>0" + month + "/" + year + "</option>";
                    }
                    else {
                        optgroup += "<option value='" + month + "_" + year + "'>" + month + "/" + year + "</option>";
                    }


                }
            }
            optgroup += "</optgroup>";
            objSelectControl.append(optgroup);
        }

    }

    var selectedMonth = currentMonth;
    var selectedYear = currentYear;



}


function daysInMonth( /* int */monthNum, /* int* */yearNum)
/*
Author: Matthias Dailey		March 30, 2009
	
this code is freely distributable and modifiable as 
long as the author's name is kept unmodified in this header.
	
Preconditions: 
monthNum is a whole number representing a month
yearNum is a whole number representing a yearNum
for example, monthNum = 1 for January, 12 for December, etc.
	
Postconditions: 
returns a number- the last day in the specified month
	
Notes: 
if monthNum is unspecified, monthNum is made to be the current month.
if monthNum or yearNum are invalid values, the value false is returned
some ideas taken from http://javascript.about.com/library/bllday.htm
*/
{

    //alert('yearNum1: ' + yearNum + '\nmonthNum1: ' + monthNum);
    // if monthNum or yearNum are not specified, set them to the current month/yearNum
    if (monthNum == undefined && yearNum == undefined) {
        now = new Date();
        monthNum = now.getMonth() + 1;
        yearNum = now.getFullYear();
    }
    else if (monthNum || yearNum || monthNum == "" || yearNum == "") {
        now = new Date();
        if (monthNum == undefined || monthNum == "")
            monthNum = now.getMonth() + 1;
        if (yearNum == undefined || yearNum == "")
            yearNum = now.getFullYear();
    }
    // turn 01 into 1
    monthNum = Number(monthNum);
    yearNum = Number(yearNum);

    // check if monthNum and yearNum are numbers and whole numbers and monthNum is between 1 and 12
    if (isNaN(monthNum) || isNaN(yearNum) || monthNum % 1 != 0 || yearNum % 1 != 0 || monthNum < 1 || monthNum > 12) {
        return false;
    }
    // create date

   // alert('yearNum2: ' + yearNum + '\nmonthNum2: ' + monthNum);
    
    var d = new Date(yearNum, monthNum, 0);
        return d.getDate();

}

function getMonthName(month) {
    //var d = new Date();
    var months = new Array(12);
    month -= 1;
    months[0] = "January";
    months[1] = "February";
    months[2] = "March";
    months[3] = "April";
    months[4] = "May";
    months[5] = "June";
    months[6] = "July";
    months[7] = "August";
    months[8] = "September";
    months[9] = "October";
    months[10] = "November";
    months[11] = "December";
    return months[month];
}


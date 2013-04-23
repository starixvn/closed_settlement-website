
var ScoreHandler = {

    OriginalHomeScores: new Array(),
    OriginalAwayScores: new Array(),
    HomeElements: new Array(),
    AwayElements: new Array(),


    init: function () {

        var homeElements = jQuery('input:text[name*="ome"]');
        var awayElements = jQuery('input:text[name*="way"]');

        var arrHomeScore = new Array();
        var arrAwayScore = new Array();
        if (homeElements != null) {
            homeElements.each(function () {

                arrHomeScore.push(jQuery(this).val());
            });
        }
        if (awayElements != null) {
            awayElements.each(function () {

                arrAwayScore.push(jQuery(this).val());
            });
        }


        this.HomeElements = homeElements;
        this.AwayElements = awayElements;

        this.OriginalHomeScores = arrHomeScore;
        this.OriginalAwayScores = arrAwayScore;


    },

    ChangeInputWay: function (rdo) {
        var sportID = $('sParams').value.split('|')[1];
        ScoreHandler.ScoreDirect(parseInt(sportID, 10), $('scoreDirect').checked);
    },

   /* ScoreDirect: function (sportID, val) {
        var effectedElements = 0;
        switch (sportID) {
            case 2: // basketball
            case 3: // football
            case 26: // rugby
            case 32: // netball
                effectedElements = 2;
                var IsHTOnly = $('IsHTOnly').value;
                if ((IsHTOnly == '1') && val) {
                    $('txt1qHome').disabled = true;
                    $('txt2qHome').disabled = true;
                    $('txt3qHome').disabled = true;
                    $('txt4qHome').disabled = true;
                    $('txtOTHome').disabled = true;
                    $('txt1qAway').disabled = true;
                    $('txt2qAway').disabled = true;
                    $('txt3qAway').disabled = true;
                    $('txt4qAway').disabled = true;
                    $('txtOTAway').disabled = true;

                    $('homeH').disabled = false;
                    $('homeF').disabled = true;
                    $('awayH').disabled = false;
                    $('awayF').disabled = true;
                }
                else if ((IsHTOnly == '1') && !val) {
                    $('txt1qHome').disabled = false;
                    $('txt2qHome').disabled = false;
                    $('txt3qHome').disabled = true;
                    $('txt4qHome').disabled = true;
                    $('txtOTHome').disabled = true;
                    $('txt1qAway').disabled = false;
                    $('txt2qAway').disabled = false;
                    $('txt3qAway').disabled = true;
                    $('txt4qAway').disabled = true;
                    $('txtOTAway').disabled = true;

                    $('homeH').disabled = true;
                    $('homeF').disabled = true;
                    $('awayH').disabled = true;
                    $('awayF').disabled = true;
                }
                else if ((IsHTOnly == '0') && !val) {
                    $('txt1qHome').disabled = false;
                    $('txt2qHome').disabled = false;
                    $('txt3qHome').disabled = false;
                    $('txt4qHome').disabled = false;
                    $('txtOTHome').disabled = false;
                    $('txt1qAway').disabled = false;
                    $('txt2qAway').disabled = false;
                    $('txt3qAway').disabled = false;
                    $('txt4qAway').disabled = false;
                    $('txtOTAway').disabled = false;

                    $('homeH').disabled = true;
                    $('homeF').disabled = true;
                    $('awayH').disabled = true;
                    $('awayF').disabled = true;
                }
                else if ((IsHTOnly == '0') && val) {
                    $('txt1qHome').disabled = true;
                    $('txt2qHome').disabled = true;
                    $('txt3qHome').disabled = true;
                    $('txt4qHome').disabled = true;
                    $('txtOTHome').disabled = true;
                    $('txt1qAway').disabled = true;
                    $('txt2qAway').disabled = true;
                    $('txt3qAway').disabled = true;
                    $('txt4qAway').disabled = true;
                    $('txtOTAway').disabled = true;

                    $('homeH').disabled = false;
                    $('homeF').disabled = false;
                    $('awayH').disabled = false;
                    $('awayF').disabled = false;
                }
                break;
            case 4: // ice hockey
            case 28: // field hockey 
                effectedElements = 1;
                $('txt1pHome').disabled = val;
                $('txt2pHome').disabled = val;
                $('txt3pHome').disabled = val;
                $('txtOTHome').disabled = val;
                $('txt1pAway').disabled = val;
                $('txt2pAway').disabled = val;
                $('txt3pAway').disabled = val;
                $('txtOTAway').disabled = val;
                $('homeF').disabled = !val;
                $('awayF').disabled = !val;
                break;
            case 5: // tennis 
                effectedElements = 1;
                $('txt1sHome').disabled = val;
                $('txt2sHome').disabled = val;
                $('txt3sHome').disabled = val;
                $('txt4sHome').disabled = val;
                $('txt5sHome').disabled = val;
                $('txt1sAway').disabled = val;
                $('txt2sAway').disabled = val;
                $('txt3sAway').disabled = val;
                $('txt4sAway').disabled = val;
                $('txt5sAway').disabled = val;

                $('txtFSHome').disabled = !val;
                $('txtFSAway').disabled = !val;
                break;
            case 8: // baseball
                effectedElements = 2;
                $('txt1Home').disabled = val;
                $('txt2Home').disabled = val;
                $('txt3Home').disabled = val;
                $('txt4Home').disabled = val;
                $('txt5Home').disabled = val;
                $('txt6Home').disabled = val;
                $('txt7Home').disabled = val;
                $('txt8Home').disabled = val;
                $('txt9Home').disabled = val;
                $('txtEIHome').disabled = val;
                $('txt1Away').disabled = val;
                $('txt2Away').disabled = val;
                $('txt3Away').disabled = val;
                $('txt4Away').disabled = val;
                $('txt5Away').disabled = val;
                $('txt6Away').disabled = val;
                $('txt7Away').disabled = val;
                $('txt8Away').disabled = val;
                $('txt9Away').disabled = val;
                $('txtEIAway').disabled = val;
                $('homeH').disabled = !val;
                $('awayH').disabled = !val;
                $('homeF').disabled = !val;
                $('awayF').disabled = !val;
                break;
            case 9: // badminton
            case 6: // volleyball
            case 30: //Squash
                effectedElements = 1;
                $('txt1sHome').disabled = val;
                $('txt2sHome').disabled = val;
                $('txt3sHome').disabled = val;
                $('txt4sHome').disabled = val;
                $('txt5sHome').disabled = val;
                $('txt1sAway').disabled = val;
                $('txt2sAway').disabled = val;
                $('txt3sAway').disabled = val;
                $('txt4sAway').disabled = val;
                $('txt5sAway').disabled = val;

                $('txtTSHome').disabled = !val;
                $('txtTSAway').disabled = !val;
                break;
            default: break;
        }
        var lengthHome = ScoreHandler.OriginalHomeScores.length;
        for (var i = 0; i < lengthHome - effectedElements; i++) {
            $(ScoreHandler.HomeElements[i]).value = (val ? "" : ScoreHandler.OriginalHomeScores[i]);
            $(ScoreHandler.AwayElements[i]).value = (val ? "" : ScoreHandler.OriginalAwayScores[i]);
        }
        for (var j = 1; j <= effectedElements; j++) {
            $(ScoreHandler.HomeElements[lengthHome - j]).value = ScoreHandler.OriginalHomeScores[lengthHome - j];
            $(ScoreHandler.AwayElements[lengthHome - j]).value = ScoreHandler.OriginalAwayScores[lengthHome - j];
        }
        isInputScoreDirectly = val;
        $('isSD').value = (isInputScoreDirectly) ? "1" : "0";
    },*/

    ScoreDirect: function (sportID, inputDirect) {

        var effectedElements = 0;
        var IsHTOnly = "";
        switch (sportID) {
            case 4:
            case 28:
            case 5:
            case 9:
            case 6:
            case 30:
                effectedElements = 1;
                break;
            case 8:
                effectedElements = 2;
                break;
            case 2:
            case 3:
            case 26:
            case 32:
                effectedElements = 2;
                IsHTOnly = $('IsHTOnly').value;
                break;
            default: break;

        }
        
        var lengthHome = ScoreHandler.OriginalHomeScores.length;
        if (IsHTOnly == "" || IsHTOnly == "0") {
            for (var i = 0; i < lengthHome; i++) {

                var homeElement = $(ScoreHandler.HomeElements[i]);
                var awayElement = $(ScoreHandler.AwayElements[i]);

                if (i < lengthHome - effectedElements) {
                    homeElement.value = (inputDirect ? "" : ScoreHandler.OriginalHomeScores[i]);
                    homeElement.disabled = inputDirect;
                    awayElement.value = (inputDirect ? "" : ScoreHandler.OriginalAwayScores[i]);
                    awayElement.disabled = inputDirect;
                }
                else {
                    homeElement.disabled = !inputDirect;
                    awayElement.disabled = !inputDirect;
                    homeElement.value = ScoreHandler.OriginalHomeScores[i];
                    awayElement.value = ScoreHandler.OriginalAwayScores[i];
                }
            }
        }
        else {
            for (var i = 0; i < lengthHome; i++) {

                var homeElement = $(ScoreHandler.HomeElements[i]);
                var awayElement = $(ScoreHandler.AwayElements[i]);
                
                if (i < lengthHome - effectedElements) {
                    homeElement.value = (inputDirect ? "" : ScoreHandler.OriginalHomeScores[i]);
                    awayElement.value = (inputDirect ? "" : ScoreHandler.OriginalAwayScores[i]);
                    if (!inputDirect && (i = 0 || i == 1)) {
                        homeElement.disabled = false;
                        awayElement.disabled = false;
                    }
                    else {
                        homeElement.disabled = true;
                        awayElement.disabled = true;
                    }
                }
                else {
                    if (inputDirect && i == lengthHome - 2) {
                        homeElement.disabled = false;
                        awayElement.disabled = false;
                    }
                    else {
                        homeElement.disabled = true;
                        awayElement.disabled = true;
                    }
                    homeElement.value = ScoreHandler.OriginalHomeScores[i];
                    awayElement.value = ScoreHandler.OriginalAwayScores[i];
                }

            }
        }
        isInputScoreDirectly = inputDirect;
        $('isSD').value = (inputDirect) ? "1" : "0";
    }

};




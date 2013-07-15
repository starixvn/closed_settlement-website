var apollo = {
    updateConfirmedAction_Single: function (matchId, type, homeHT, awayHT, homeFT, awayFT, betStatus, goalrecord) {

        var actionType = apollo.parseType(type);

        var param = {
            matchId: matchId,
            action: actionType,
            homeHT: homeHT,
            awayHT: awayHT,
            homeFT: homeFT,
            awayFT: awayFT,
            betStatus: betStatus,
            goalrecord: goalrecord
        };

        var action = 'Settlement/SetConfirmedAction_Single';

        parent.$.ajax({
            type: "POST",
            url: action,
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: JSON.stringify(param),
            success: function (rs) {

            },
            complete: function () {

            }
        });
    },

    updateConfirmedAction_Parlay: function (matchId, type) {

        var actionType = apollo.parseType(type);

        var param = {
            matchIds: matchId,
            action: actionType
        };

        var action = 'Settlement/Set_ConfirmedAction_Parlay';

        parent.$.ajax({
            type: "POST",
            url: action,
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: JSON.stringify(param),
            success: function (rs) {

            },
            complete: function () {

            }
        });
    },

    parseType: function (type) {
        var rs = 0;
        switch (type) {
            case 'unsettle': // 0: Unsettle
                rs = 0;
                break;
            case 'calculate': // 1: Calculate FG/NG
                rs = 1;
                break;
            case 'billht': // 2: Calculate HT
                rs = 2;
                break;
            case 'billft': // 3: Calculate FT
                rs = 3;
                break;
            case 'bill': // 4: Calculate All
                rs = 4;
                break;
            case 'm': // 5: Calculate Parlay
                rs = 5;
                break;
            case 'refundhalftime': // 11: Refund HT
                rs = 11;
                break;
            case 'refundfulltime': // 12: Refund FT
                rs = 12;
                break;
            case 'refund': // 13: Refund All
                rs = 13;
                break;
            case 'rht': // 14: Refund Parlay HT
                rs = 14;
                break;
            case 'rft': // 15: Refund Parlay FT
                rs = 15;
                break;
            case 'r': // 16: Refund Parlay All
                rs = 16;
                break;
            case 'refundfirstnogoal': // 17: Refund All FGLG	
                rs = 17;
                break;
            case 'refundalllastgoal': // 18: Refund All LG
                rs = 18;
                break;
        }
        return rs;
    }
};
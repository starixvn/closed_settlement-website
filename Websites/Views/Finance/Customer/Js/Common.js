function editSetting(custId, roleId) {
    showPopup("@Resources.Language.PokerPageTitle"
                , "Customer/PT/Poker/SuperUpdate.aspx?custId=" + custId + "&roleId=" + roleId
                , 0, 0, 640, 480, false, 2000, null);
}

function liveCasinoSetting(custId, exchangeId, uplineId) {
    showPopup("@Resources.Language.liveCasinoTitle"
                , "LiveCasino/PTSetting/EditSuper.aspx?custId=" + custId + "&exchangeId=" + exchangeId
                , 0, 0, 640, 480, false, 2000, null);
}

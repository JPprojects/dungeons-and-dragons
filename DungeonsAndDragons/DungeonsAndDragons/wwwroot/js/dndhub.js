"use strict";

// ********** Connection String ********** //

var connection = new signalR.HubConnectionBuilder().withUrl("/dndhub").build();

// ********** Functions ********** //

function joinGame(){
    var gameid = $("#gameid").text();
    connection.invoke("JoinGame", gameid).catch(function (err) {
    return console.error(err.toString());
    }); 
}

// ********** Broadcast Events ********** //



// ********** Listener Events ********** //

connection.on("UpdatePlayerInvites", function (acceptedplayers, pendingplayers) {
    var accepted = jQuery.parseJSON(acceptedplayers);
    var pending = jQuery.parseJSON(pendingplayers);
    $("#acceptedPlayers").empty();
    $("#pendingPlayers").empty();
    accepted.forEach(element => {
        $("#acceptedPlayers").append('<p>' + element.userusername + ' playing as <a href="../../PlayableCharacter/View/' + element.playablecharacterid + '">' + element.playablecharactername + '</a></p>');
    });
    pending.forEach(element => {
        $("#pendingPlayers").append("<p>" + element.userusername + "</p>");
    });
});

connection.on("StartMerchantRedirect", function (gameid){
    window.location.replace("../../Merchant/View/" + gameid);
});

connection.on("EndMerchantRedirect", function (gameid){
    window.location.replace("../../Game/View/" + gameid);
});

connection.on("StartBattleRedirect", function (gameid, npcId){
    window.location.replace("../../Battle/View/" + gameid + "?npcId=" + npcId);
});

// ********** Establish Connection ********** //

connection.start().then(function(result){
    joinGame();
    }).catch(function (err) {
    return console.error(err.toString());

});
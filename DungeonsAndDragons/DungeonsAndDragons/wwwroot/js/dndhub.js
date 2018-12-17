"use strict";

var connection = new signalR.HubConnectionBuilder().withUrl("/dndhub").build();
var jsonBattle = $("#jsonBattle").text();
if (jsonBattle != "")
{
    var battleJson = jQuery.parseJSON(jsonBattle);
};

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

connection.on("StartBattleRedirect", function (gameid, npcId){
    window.location.replace("../../Battle/View/" + gameid + "?npcId=" + npcId);
});

connection.on("EndBattleRedirect", function (gameid){
    window.location.replace("../../Game/View/" + gameid);
});

$("#playerAttackButton").click(function(){
    battleJson.NPC.currentHp -= 10;
    UpdateJson();
});

function UpdateJson() {
    console.log("ajax running");
    $.ajax({
        url: '../UpdateJSON',
        type: 'POST',
        data: {"json" : JSON.stringify(battleJson)}
    })
};

connection.start().then(function(result){
    joinGame();
    }).catch(function (err) {
    return console.error(err.toString());

});

function joinGame(){
    var gameid = $("#gameid").text();
    connection.invoke("JoinGame", gameid).catch(function (err) {
    return console.error(err.toString());
    }); 
}



connection.on("UpdateBattleStats", function (updatedStatsJson) {
    location.reload();
});
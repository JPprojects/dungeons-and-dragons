"use strict";

$(document).ready(function() {
    LoadBattleValues();
});

// ********** Connection String ********** //

var connection = new signalR.HubConnectionBuilder().withUrl("/dndhub").build();
var jsonBattle = $("#jsonBattle").text();
if (jsonBattle != "")
{
    var battleJson = jQuery.parseJSON(jsonBattle);
};

// ********** Functions ********** //

function joinGame(){
    var gameid = $("#gameid").text();
    connection.invoke("JoinGame", gameid).catch(function (err) {
    return console.error(err.toString());
    }); 
}

function UpdateJson() {
    $.ajax({
        url: '../UpdateJSON',
        type: 'POST',
        data: {"json" : JSON.stringify(battleJson)}
    })
};

function LoadBattleValues() {
    $(".enemyName").text(battleJson.NPC.name);
    $("#enemyImage").attr("src", battleJson.NPC.imagePath);
    $("#enemyCurrentHp").text(battleJson.NPC.currentHp);
    $("#enemyMaxHp").text(battleJson.NPC.maxHp);
    $("#enemyAttack").text(battleJson.NPC.attack);
    battleJson.players.forEach(function(element) {
        $("." + element.id + "-name").text(element.name);
        $("#" + element.id + "-image").attr("src", element.imagePath);
        $("#" + element.id + "-currentHp").text(element.currentHp);
        $("#" + element.id + "-maxHp").text(element.currentHp);
        $("#" + element.id + "-attack").text(element.attack);
    })
};

function UpdateJsonDiv(updatedStatsJson) {
    $("#jsonBattle").text(updatedStatsJson);
    battleJson = jQuery.parseJSON(updatedStatsJson);
    LoadBattleValues();
};

// ********** Broadcast Events ********** //

$("#playerAttackButton").click(function(){
    battleJson.NPC.currentHp -= 10;
    UpdateJson();
});

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

connection.on("StartBattleRedirect", function (gameid, npcId){
    window.location.replace("../../Battle/View/" + gameid + "?npcId=" + npcId);
});

connection.on("EndBattleRedirect", function (gameid){
    window.location.replace("../../Game/View/" + gameid);
});

connection.on("UpdateBattleStats", function (gameId, updatedStatsJson) {
    UpdateJsonDiv(updatedStatsJson);
});

// ********** Establish Connection ********** //

connection.start().then(function(result){
    joinGame();
    }).catch(function (err) {
    return console.error(err.toString());

});
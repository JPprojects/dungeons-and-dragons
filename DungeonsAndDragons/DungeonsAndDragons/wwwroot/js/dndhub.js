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

function UpdateJson(characterId = 0, characterCurrentHp = 0) {
    $.ajax({
        url: '../UpdateJSON',
        type: 'POST',
        data: {
            "json" : JSON.stringify(battleJson),
            "characterId" : characterId,
            "characterCurrentHp" : characterCurrentHp
        }
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
        $("#" + element.id + "-maxHp").text(element.maxHp);
        $("#" + element.id + "-attack").text(element.attack);
    })
    ShowAndHidePlayerAttackButton();
};

function ShowAndHidePlayerAttackButton() {
    if ($("#loggedInUserId").text() == battleJson.currentPlayerId) {
        $("#playerAttack").attr("style", "display:block");
    }
    else {
        $("#playerAttack").attr("style", "display:none");
    }
};

function UpdateJsonDiv(updatedStatsJson) {
    battleJson = jQuery.parseJSON(updatedStatsJson);
    $("#jsonBattle").text(updatedStatsJson);
    LoadBattleValues();
};

function SetCurrentPlayerId() {
    var currentPlayerIndex = GetCurrentPlayerIndex();
    if (currentPlayerIndex == battleJson.players.length - 1) {
        var nextPlayerIndex = 0;
    }
    else {
        var nextPlayerIndex = currentPlayerIndex + 1
    }
    battleJson.currentPlayerId = battleJson.players[nextPlayerIndex].userid
};

function PlayerAttack() {
    var currentPlayerIndex = GetCurrentPlayerIndex();
    battleJson.NPC.currentHp -= battleJson.players[currentPlayerIndex].attack;
    if (battleJson.NPC.currentHp < 0) { battleJson.NPC.currentHp = 0 };
};

function GetCurrentPlayerIndex() {
    var currentPlayer = battleJson.players.find(obj => { return obj.userid == battleJson.currentPlayerId});
    return battleJson.players.indexOf(currentPlayer);
};

// ********** Broadcast Events ********** //

$("#playerAttackButton").click(function(){
    if (battleJson.NPC.currentHp == 0) { alert("STAAAHP HE ALREADY DED!") };
    PlayerAttack();
    SetCurrentPlayerId();
    UpdateJson();
});

$("#npcAttackButton").click(function(){
    var characterId = $("#npcAttack").val();
    battleJson.players.find(obj => { return obj.id == characterId}).currentHp -= battleJson.NPC.attack;
    if (battleJson.players.find(obj => { return obj.id == characterId}).currentHp < 0) { battleJson.players.find(obj => { return obj.id == characterId}).currentHp = 0 };
    var newHp = battleJson.players.find(obj => { return obj.id == characterId}).currentHp;
    UpdateJson(characterId, newHp);
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
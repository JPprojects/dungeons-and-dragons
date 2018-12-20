"use strict";

$(document).ready(function() {
    LoadBattleValues();
    SetLiveCharacters();
});

// ********** Connection String ********** //

var connection = new signalR.HubConnectionBuilder().withUrl("/dndhub").build();
var jsonBattle = $("#jsonBattle").text();
if (jsonBattle != "")
{
    var battleJson = jQuery.parseJSON(jsonBattle);
};
var liveCharacters = [];

// ********** Functions ********** //

function joinGame(){
    var gameid = $("#gameid").text();
    connection.invoke("JoinGame", gameid).catch(function (err) {
    return console.error(err.toString());
    }); 
}

function UpdateJson(characterId = 0, characterCurrentHp = 0, attackingPlayerId = 0) {
    $.ajax({
        url: '../UpdateJSON',
        type: 'POST',
        data: {
            "json" : JSON.stringify(battleJson),
            "characterId" : characterId,
            "characterCurrentHp" : characterCurrentHp,
            "attackingPlayerId": attackingPlayerId
        }
    });
};


function CreateBorder() {
    var currentPlayerId = battleJson.currentPlayerId;
    var attackingPlayerId = battleJson.players.find(obj => { return obj.userid == currentPlayerId}).id;
    $(".card-body").css("border", "none");
    $("#" + attackingPlayerId + "-card").css("border", "1px solid green");

}

function LoadBattleValues() {
    attackAnimation()
    $(".enemyName").text(battleJson.NPC.name);
    $("#enemyImage").attr("src", battleJson.NPC.imagePath);
    $("#enemyCurrentHp").text(battleJson.NPC.currentHp);
    $("#enemyMaxHp").text(battleJson.NPC.maxHp);
    $("#enemyHealthBar").attr("aria-valuemax", battleJson.NPC.maxHp);
    $("#enemyHealthBar").attr("aria-valuenow", battleJson.NPC.currentHp);
    $("#enemyHealthBar").css("width", (battleJson.NPC.currentHp / battleJson.NPC.maxHp * 100) + "%");
    $("#enemyAttack").text(battleJson.NPC.attack);
    battleJson.players.forEach(function(element) {
        $("." + element.id + "-name").text(element.name);
        $("#" + element.id + "-image").attr("src", element.imagePath);
        $("#" + element.id + "-currentHp").text(element.currentHp);
        $("#" + element.id + "-maxHp").text(element.maxHp);
        $("#" + element.id + "-healthBar").attr("aria-valuemax", element.maxHp);
        $("#" + element.id + "-healthBar").attr("aria-valuenow", element.currentHp);
        $("#" + element.id + "-healthBar").css("width", (element.currentHp / element.maxHp * 100) + "%");
        $("#" + element.id + "-attack").text(element.attack);
    })
    SetLiveCharacters();
    ShowAndHidePlayerAttackButton();
    CreateBorder();
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
    if (currentPlayerIndex == liveCharacters.length - 1) {
        var nextPlayerIndex = 0;
    }
    else {
        var nextPlayerIndex = currentPlayerIndex + 1
    }
    battleJson.currentPlayerId = liveCharacters[nextPlayerIndex];
};

function PlayerAttack() {
    var currentPlayer = battleJson.players.find(obj => {
        return obj.userid == battleJson.currentPlayerId
    });
    battleJson.NPC.currentHp -= currentPlayer.attack;
    if (battleJson.NPC.currentHp < 0) { battleJson.NPC.currentHp = 0 };
};

function GetCurrentPlayerIndex() {
    var currentPlayer = liveCharacters.indexOf(battleJson.currentPlayerId);

    return currentPlayer;
};

function GetPlayerIndex(userId) {
    var currentPlayer = liveCharacters.indexOf(userId);

    return currentPlayer;
};

function SetLiveCharacters() {

    liveCharacters = [];

    battleJson.players.forEach(function(element){
        if (element.currentHp > 0) {
            liveCharacters.push(element.userid)
        };
    });

};

// ********** Broadcast Events ********** //

$("#playerAttackButton").click(function(){
    if (battleJson.NPC.currentHp == 0) { alert("STAAAHP HE ALREADY DED!") };
    PlayerAttack();
    var currentPlayerId = battleJson.currentPlayerId;

    var attackingPlayerId = battleJson.players.find(obj => { return obj.userid == currentPlayerId}).id;
    console.log(attackingPlayerId);
    SetCurrentPlayerId();
    UpdateJson(0, 0, attackingPlayerId);
});

$("#npcAttackButton").click(function(){
    var characterId = $("#npcAttack").val();
    var attackedPlayerId = battleJson.players.find(obj => { return obj.id == characterId}).userid;
    console.log(attackedPlayerId);
    battleJson.players.find(obj => { return obj.id == characterId}).currentHp -= battleJson.NPC.attack;
    if (battleJson.players.find(obj => { return obj.id == characterId}).currentHp < 0) 
    {
        battleJson.players.find(obj => { return obj.id == characterId}).currentHp = 0;
        if (attackedPlayerId == battleJson.currentPlayerId){
            SetCurrentPlayerId();
        };
    };
    var newHp = battleJson.players.find(obj => { return obj.id == characterId}).currentHp;
    UpdateJson(characterId, newHp);
});

// ********** Listener Events ********** //

connection.on("EndBattleRedirect", function (gameid){
    window.location.replace("../../Game/View/" + gameid);
});

connection.on("UpdateBattleStats", function (gameId, updatedStatsJson, attackingPlayerId) {
    attackAnimation(attackingPlayerId);
    UpdateJsonDiv(updatedStatsJson);
});

// ********** Establish Connection ********** //

connection.start().then(function(result){
    joinGame();
    }).catch(function (err) {
    return console.error(err.toString());

    });


// ************ Animations ************ //

function moveUp(id){ 
    $("#" + id + "-image").css("top", "-100px");
}

function moveDown(id){
    $("#" + id + "-image").css("top", "0px");


}

function attackAnimation(id){
    moveUp(id);
    setTimeout(moveDown, 300, id);
}







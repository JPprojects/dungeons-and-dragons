"use strict";

$(document).ready(function() {
    CreateJsonObject();
});

var inventoryJson;

// ********** Connection String ********** //

var connection = new signalR.HubConnectionBuilder().withUrl("/dndhub").build();

// ********** Functions ********** //

function CreateJsonObject() {
    inventoryJson = jQuery.parseJSON($("#inventoryJson").text());
};

function SendJsonDataToController(itemId, playableCharacterId) {
    console.log(itemId);
    $.ajax({
        url: '../Use',
        type: 'POST',
        data: {
            "json" : JSON.stringify(inventoryJson),
            "itemId" : itemId,
            "playableCharacterId" : 1
        }
    });
};

function UpdateInventoryDivValues() {
    
    SendJsonDataToController(item);
};

// ********** Broadcast Events ********** //

$(".useItem").click(function() {
    var itemId = this.id;
    SendJsonDataToController(itemId, $("#playableCharacterId").text());
});

// ********** Listener Events ********** //

connection.on("UpdateUserInvites", function(){
    updateUserInvites();
});

// ********** Establish Connection ********** //

connection.start().catch(function (err) {
    return console.error(err.toString());
});

"use strict";

var connection = new signalR.HubConnectionBuilder().withUrl("/dndhub").build();
var accepted;

connection.on("UpdatePlayerInvites", function (acceptedplayers, pendingplayers) {
    accepted = jQuery.parseJSON(acceptedplayers);
    var pending = jQuery.parseJSON(pendingplayers);
    $("#acceptedPlayers").empty();
    document.getElementById("pendingPlayers").remove();
    accepted.forEach(element => {
        $("#acceptedPlayers").append(element.userusername);
    });

});

connection.start().catch(function (err) {
    return console.error(err.toString());
});
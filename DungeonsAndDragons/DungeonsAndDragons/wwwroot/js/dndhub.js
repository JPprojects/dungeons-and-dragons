"use strict";

var connection = new signalR.HubConnectionBuilder().withUrl("/dndhub").build();
var accepted;

connection.on("UpdatePlayerInvites", function (acceptedplayers, pendingplayers) {
    accepted = jQuery.parseJSON(acceptedplayers);
    var pending = jQuery.parseJSON(pendingplayers);
    $("#acceptedPlayers").empty();
    $("#pendingPlayers").empty();
    accepted.forEach(element => {
        $("#acceptedPlayers").append("<p>" + element.userusername + "</p>");
    });
    pending.forEach(element => {
        $("#pendingPlayers").append("<p>" + element.userusername + "</p>");
    });

});

connection.start().catch(function (err) {
    return console.error(err.toString());
});
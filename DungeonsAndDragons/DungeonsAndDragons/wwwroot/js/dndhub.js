"use strict";

var connection = new signalR.HubConnectionBuilder().withUrl("/dndhub").build();

connection.on("ReceiveMessage", function (user, message) {
    var msg = message.replace(/&/g, "&amp;").replace(/</g, "&lt;").replace(/>/g, "&gt;");
    var encodedMsg = user + " says " + msg;
    var li = document.createElement("li");
    li.textContent = encodedMsg;
    document.getElementById("messagesList").appendChild(li);
});

connection.start().catch(function (err) {
    return console.error(err.toString());
});

$(".acceptInvite").click(function(){
    event.preventDefault();
    var gameUserId = $(this).attr("value");
    connection.invoke("AcceptInvite", gameUserId).catch(function (err) {
        return console.error(err.toString());
    });
});
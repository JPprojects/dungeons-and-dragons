"use strict";

// ********** Connection String ********** //

var connection = new signalR.HubConnectionBuilder().withUrl("/dndhub").build();

// ********** Functions ********** //

function updateUserInvites(){
    location.reload();
};

// ********** Broadcast Events ********** //

function joinUserGroup(){
    var userId = $("#userId").text();
    connection.invoke("JoinUserGroup", userId).catch(function (err) {
    return console.error(err.toString());
    }); 
}

// ********** Listener Events ********** //

connection.on("UpdateUserInvites", function(){
    updateUserInvites();
});

// ********** Establish Connection ********** //

connection.start().then(function(result){
    joinUserGroup();
    }).catch(function (err) {
    return console.error(err.toString());

});

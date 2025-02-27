"use strict";

var connection = new signalR.HubConnectionBuilder()
    .withUrl('/workflowhub')
    .withAutomaticReconnect()
    .build();

function get_workflow_id() {
    return document.querySelector('#workflow_id').value;
}

connection.start().then(function () {
    connection.invoke('ClientJoin', get_workflow_id()).catch(function (err) {
        return console.error(err.toString());
    });
}).catch(function (err) {
    return console.error(err.toString());
})

connection.on("ReceiveMessage", function (user, message) {
    console.log("ReceiveMessage", user, message);
});


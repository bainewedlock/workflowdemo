"use strict";

var connection = new signalR.HubConnectionBuilder()
    .withUrl('/workflowhub')
    .withAutomaticReconnect()
    .build();

function get_workflow_id() {
    return document.querySelector('#workflow_id').value;
}

connection.start().then(function () {
    console.log('connected');
    connection.invoke('client_join', get_workflow_id()).catch(function (err) {
        return console.error(err.toString());
    });
});

connection.on("action", function (a) {
    console.log("action: " + action);
})

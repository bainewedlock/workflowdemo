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
});

function resume_workflow() {
    console.log("könnte gehen");
    connection.invoke("Resume", get_workflow_id()).catch(function (err) {
        return console.error(err.toString());
    });
//    document.getElementById("resume_btn").onclick = () => {
//        connection.invoke("Resume", get_workflow_id()).catch(function (err) {
//        return console.error(err.toString());
//    });
}

connection.on("Log", function (data) {
    if(document.getElementById("empty_log"))
        document.getElementById("empty_log").remove();
    var t = document.getElementById("log");
    var row = t.insertRow(1);
    row.insertCell(0).innerHTML = data.timestamp;
    row.insertCell(1).innerHTML = data.category;
    row.insertCell(2).innerHTML = data.step;
    row.insertCell(3).innerHTML = data.message;
});

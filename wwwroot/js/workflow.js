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

connection.on("Log", function (data) {
    if(document.getElementById("empty_log"))
        document.getElementById("empty_log").remove();
    var t = document.getElementById("log");
    var row = t.insertRow(1);
    row.insertCell(0).innerHTML = data.timestamp;
    row.insertCell(1).innerHTML = data.message;
//    var li = document.createElement("li");
//    li.textContent = `[${data.timestamp}] ${data.message}`;
//    document.getElementById("log").appendChild(li);
});

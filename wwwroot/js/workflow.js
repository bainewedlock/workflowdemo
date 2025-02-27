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
//    console.log("tja");
//    var li = document.createElement("li");
//    //li.textContent = `[${data.timestamp}] ${data.message}`;
//    li.textContent = "hä";
//    console.log("tjo", document.getElementById("log"));
//    document.getElementById("log").apendChild(li);
    console.log("tjo");
}).catch(function (err) {
    return console.error(err.toString());
});


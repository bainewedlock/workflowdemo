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
}).catch(function (err) {
    return console.error(err.toString());
})

connection.on("action", function (a) {
    console.log("actionr: " + action);
});


connection.on("updates_message", function (upd) {
    console.log("upd: " + action);
});

connection.on("ReceiveMessage", function (user, message) {
    console.log("action2: " + user);
});

connection.on("ReceiveMessage2", function (user, message) {
    var li = document.createElement("li");
    document.getElementById("messagesList").appendChild(li);
    // We can assign user-supplied strings to an element's textContent because it
    // is not interpreted as markup. If you're assigning in any other way, you 
    // should be aware of possible script injection concerns.
    li.textContent = `${user} says ${message}`;
    console.log("ja gut äh");
});

document.getElementById("sendButton").addEventListener("click", function (event) {
    var user = document.getElementById("userInput").value;
    var message = document.getElementById("messageInput").value;
    connection.invoke("SendMessage", user, message).catch(function (err) {
        return console.error(err.toString());
    });
    event.preventDefault();
});

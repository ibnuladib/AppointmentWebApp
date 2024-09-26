$(document).ready(function () {
    const connection = new signalR.HubConnectionBuilder()
        .withUrl("/logHub")
        .build();

    connection.on("ReceiveLogUpdate", function (message) {
        const logElement = document.createElement("li");
        const timestamp = new Date().toLocaleString();

        let combinedString = timestamp.concat(": ", message);
        logElement.innerHTML = `<span class="timestamp">${timestamp}</span>: ${message}`;
        document.getElementById("logList").appendChild(logElement);
    });

    connection.start().catch(function (err) {
        console.error(err.toString());
    });
});

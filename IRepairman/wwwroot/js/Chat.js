const connection = new signalR.HubConnectionBuilder()
    .withUrl("/chatHub")
    .build();

connection.on("ReceiveMessage", (senderId, message) => {
    // Обработка полученных сообщений, например, добавление их в чат окно.
    console.log(`Received message from ${senderId}: ${message}`);
});

async function openChat(receiverId) {
    // Отправка сообщения через SignalR при нажатии кнопки Contact.
    const message = prompt("Enter your message:");
    if (message) {
        await connection.invoke("SendMessage", receiverId, message);
    }
}

connection.start().catch(err => console.error(err));

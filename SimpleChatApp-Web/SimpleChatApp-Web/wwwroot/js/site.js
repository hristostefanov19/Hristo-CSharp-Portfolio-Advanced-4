const connection = new signalR.HubConnectionBuilder()
    .withUrl("/chatHub")
    .build();

let currentUser = "";

function getUserColor(user) {
    let hash = 0;
    for (let i = 0; i < user.length; i++) {
        hash = user.charCodeAt(i) + ((hash << 5) - hash);
    }
    return `hsl(${hash % 360}, 70%, 80%)`;
}

connection.on("ReceiveMessage", (user, message, time) => {
    const li = document.createElement("li");
    li.textContent = `${new Date(time).toLocaleTimeString()} - ${user}: ${message}`;
    li.classList.add("list-group-item");
    li.style.backgroundColor = getUserColor(user);
    document.getElementById("messagesList").appendChild(li);
    li.scrollIntoView(); 
});

connection.on("UpdateUsers", (users) => {
    const online = document.getElementById("onlineUsers");
    online.innerHTML = "";
    users.forEach(u => {
        const li = document.createElement("li");
        li.textContent = u;
        li.classList.add("list-group-item");
        li.style.backgroundColor = getUserColor(u);
        online.appendChild(li);
    });
    document.getElementById("onlineCount").textContent = `Online: ${users.length}`;
});

connection.start().then(() => {
    const nameInput = document.getElementById("userInput");
    nameInput.addEventListener("blur", function () {
        const name = this.value.trim();
        if (name) {
            currentUser = name;
            connection.invoke("SetUserName", currentUser);
        }
    });
}).catch(err => console.error(err.toString()));

function sendMessage() {
    const message = document.getElementById("messageInput").value.trim();
    if (currentUser && message) {
        connection.invoke("SendMessage", currentUser, message);
        document.getElementById("messageInput").value = "";
    } else if (!currentUser) {
        alert("Please enter your name first!");
    }
}

document.getElementById("messageInput").addEventListener("keypress", function (e) {
    if (e.key === "Enter") sendMessage();
});

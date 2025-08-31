using Microsoft.AspNetCore.SignalR;
using System.Collections.Concurrent;

namespace SimpleChatApp_Web_Portfolio.Hubs
{
    public class ChatHub : Hub
    {
        private static ConcurrentDictionary<string, string> OnlineUsers = new();
        private static List<(string User, string Message, DateTime Time)> ChatHistory = new();

        public override async Task OnConnectedAsync()
        {
            OnlineUsers.TryAdd(Context.ConnectionId, "Unknown");

            foreach (var msg in ChatHistory)
            {
                await Clients.Caller.SendAsync("ReceiveMessage", msg.User, msg.Message, msg.Time);
            }

            await Clients.All.SendAsync("UpdateUsers", OnlineUsers.Values);

            await base.OnConnectedAsync();
        }

        public override async Task OnDisconnectedAsync(System.Exception exception)
        {
            OnlineUsers.TryRemove(Context.ConnectionId, out _);
            await Clients.All.SendAsync("UpdateUsers", OnlineUsers.Values);
            await base.OnDisconnectedAsync(exception);
        }

        public async Task SetUserName(string name)
        {
            OnlineUsers[Context.ConnectionId] = name;
            await Clients.All.SendAsync("UpdateUsers", OnlineUsers.Values);
        }

        public async Task SendMessage(string user, string message)
        {
            var time = DateTime.Now;
            ChatHistory.Add((user, message, time));
            if (ChatHistory.Count > 50) ChatHistory.RemoveAt(0);

            await Clients.All.SendAsync("ReceiveMessage", user, message, time);
        }
    }
}


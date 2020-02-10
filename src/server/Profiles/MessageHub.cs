using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Krokodil.Profiles
{
    public class MessageHub: Hub
    {
        public async Task SendRoomMessage(string roomName, string message)
        {
            await Clients.Group(roomName).SendAsync("SendMessage", message);
        }

        public async Task JoinRoom(string roomName)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, roomName);
            await Clients.Group(roomName).SendAsync("Send", $"{Context.ConnectionId} has joined the group {roomName}.");
        }

        public async Task LeaveRoom(string roomName)
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, roomName);
            await Clients.Group(roomName).SendAsync("Send", $"{Context.ConnectionId} has left the group {roomName}.");
        }
    }
}

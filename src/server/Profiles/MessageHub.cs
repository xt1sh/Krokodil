using Krokodil.Services;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Krokodil.Profiles
{
    public class MessageHub: Hub
    {
        private readonly IGameManager _gameManager;

        public MessageHub(IGameManager gameManager)
        {
            _gameManager = gameManager;
        }

        public async Task SendRoomMessage(string roomName, string userId, string userName, string message)
        {
            await Clients.Group(roomName).SendAsync("ReceiveMessage", userId, userName, message);
        }

        public async Task JoinRoom(string roomName, string userId, string userName)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, roomName);
            await Clients.Group(roomName).SendAsync("Send", $"{Context.ConnectionId} has joined the group {roomName}.");
            await _gameManager.ConnectUserAsync(
                new Models.User { Id = userId, Name = userName, SingalrId = Context.ConnectionId },
                roomName);
        }

        public async Task LeaveRoom(string roomName)
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, roomName);
            await Clients.Group(roomName).SendAsync("Send", $"{Context.ConnectionId} has left the group {roomName}.");
        }
    }
}

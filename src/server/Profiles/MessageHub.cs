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
            await Clients.Group(roomName).SendAsync("ReceiveRoomSystemMessage", $"{userName} has joined");
            await _gameManager.ConnectUserAsync(
                new Models.User { Id = userId, Name = userName, SingalrId = Context.ConnectionId },
                roomName);
        }

        public async Task LeaveRoom(string roomName)
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, roomName);
            await Clients.Group(roomName).SendAsync("ReceiveRoomSystemMessage", $"{Context.ConnectionId} has left the group");
        }

        public async Task Draw(object data, string roomName)
        {
            await Clients.Groups(roomName).SendAsync("ReceiveDraw", data);
        }

        public async Task Clear(string roomName)
        {
            await Clients.Groups(roomName).SendAsync("ClearData");
        }

        public override async Task OnDisconnectedAsync(Exception exception)
        {
            var user = _gameManager.GetUserBySignalR(Context.ConnectionId);
            await Clients.Group(user.RoomId).SendAsync("ReceiveRoomSystemMessage", $"{user.Name} has left the group");
            await _gameManager.DisconnectUserBySignalrAsync(Context.ConnectionId);
            await base.OnDisconnectedAsync(exception);
        }
    }
}

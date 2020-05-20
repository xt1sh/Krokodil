using Krokodil.Models;
using System;
using System.Threading.Tasks;

namespace Krokodil.Services
{
    public interface IGameManager
    {
        public event EventHandler<StartGameEventArgs> StartGame;

        Task DeleteRoomAsync(Room room);
        Task DeleteRoomAsync(string id);
        Room GetRandomRoom();
        User GetUserBySignalR(string id);
        Room GetRoom(string id);
        Task<Room> InitializeRoomAsync(bool isPrivate);
        Task DisconnectUserByIdAsync(string userId);
        Task DisconnectUserBySignalrAsync(string signalrId);
        Task<User> ConnectUserAsync(User user, string roomId);
    }
}
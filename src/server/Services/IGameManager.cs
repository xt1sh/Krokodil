using Krokodil.Models;
using System.Threading.Tasks;

namespace Krokodil.Services
{
    public interface IGameManager
    {
        Task DeleteRoomAsync(Room room);
        Task DeleteRoomAsync(string id);
        Room GetRandomRoom();
        Room GetRoom(string id);
        Task<Room> InitializeRoomAsync(bool isPrivate);
        Task DisconnectUserAsync(string userId);
        Task<User> ConnectUserAsync(User user, string roomId);
    }
}
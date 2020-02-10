using Krokodil.Models;
using Krokodil.Profiles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Krokodil.Services
{
    public class GameManager : IGameManager
    {
        private readonly ApplicationDbContext _context;

        public GameManager(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Room> InitializeRoomAsync(bool isPrivate)
        {
            var room = new Room
            {
                IsPrivate = isPrivate
            };

            room.Id = GenerateId();

            while (_context.Rooms.Any(x => x.Id == room.Id))
            {
                room.Id = GenerateId();
            }

            room.TimeStarted = DateTime.Now;
            room.Round = 0;

            _context.Add(room);

            if (await SaveChagesAsync())
                return room;

            return null;
        }

        public Room GetRoom(string id) =>
            _context.Rooms.FirstOrDefault(r => r.Id == id);

        public Room GetRandomRoom()
        {
            var room = _context.Rooms.Where(r =>
                !r.IsPrivate &&
                r.Users.Count() < 7 &&
                (DateTime.Now - r.TimeStarted).TotalMinutes < 5).FirstOrDefault();

            return room;
        }

        public async Task DeleteRoomAsync(string id)
        {
            var room = _context.Rooms.FirstOrDefault(r => r.Id == id);
            await DeleteRoomAsync(room);
        }

        public async Task DeleteRoomAsync(Room room)
        {
            _context.Remove(room);
            _ = await SaveChagesAsync();
        }

        private string GenerateId()
        {
            var random = new Random(DateTime.Now.Millisecond);
            const string chars = "abcdefghijklmnopqrstuvwxyz0123456789";
            return new string(Enumerable.Repeat(chars, 7)
              .Select(s => s[random.Next(s.Length)]).ToArray());
        }

        private async Task<bool> SaveChagesAsync() =>
            await _context.SaveChangesAsync() > 0;
    }
}

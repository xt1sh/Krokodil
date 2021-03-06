﻿using Krokodil.Models;
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
        public event EventHandler<StartGameEventArgs> StartGame;

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

        public User GetUserBySignalR(string id) =>
            _context.Users.FirstOrDefault(u => u.SingalrId == id);

        public Room GetRoom(string id) =>
            _context.Rooms.FirstOrDefault(r => r.Id == id);

        public Room GetRandomRoom()
        {
            var rooms = _context.Rooms
               .Where(r => !r.IsPrivate).ToList();

            if (rooms.Count == 0)
                return null;

            

            var rand = new Random(DateTime.Now.Millisecond);

            var index = rand.Next(0, rooms.Count() - 1);

            var room = rooms[index];

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

        public async Task<User> ConnectUserAsync(User user, string roomId)
        {
            var room = _context.Rooms.FirstOrDefault(r => r.Id == roomId);
            user.Score = 0;

            var users = _context.Users.Where(u => u.RoomId == room.Id).ToList();

            users ??= new List<User>();
            if (users.Count == 1)
            {
                users.Add(user);
                room.Users = users;
                _context.Update(room);
                await SaveChagesAsync();

                StartGameEventArgs args = new StartGameEventArgs();
                args.roomId = roomId;
                OnStartGame(args);

                return user;
            }
            users.Add(user);
            room.Users = users;

            _context.Update(room);
            await SaveChagesAsync();

            return user;
        }

        public async Task DisconnectUserByIdAsync(string userId)
        {
            var user = _context.Users.FirstOrDefault(u => u.Id == userId);
            _context.Users.Remove(user);

            _ = await SaveChagesAsync();
        }

        public async Task DisconnectUserBySignalrAsync(string signalrId)
        {
            var user = _context.Users.FirstOrDefault(u => u.SingalrId == signalrId);
            _context.Users.Remove(user);

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

        protected virtual void OnStartGame(StartGameEventArgs e)
        {
            StartGame?.Invoke(this, e);
        }

    }

    
}

using Krokodil.Services;
using Krokodil.Services.PlayerPicker;
using Krokodil.Services.WordPicker;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Timers;

namespace Krokodil.Profiles
{
    public class MessageHub: Hub
    {
        private readonly IGameManager _gameManager;
        private readonly IPlayerPicker _playerPicker;
        private readonly IWordPicker _wordPicker;
        private readonly ITimerManager _timerManager;
        private readonly IHubContext<MessageHub> _context;

        public MessageHub(IGameManager gameManager, IWordPicker wordPicker, IPlayerPicker playerPicker, ITimerManager timerManager, IHubContext<MessageHub> context)
        {
            _gameManager = gameManager;
            _gameManager.StartGame += this.StartGame;
            _playerPicker = playerPicker;
            _wordPicker = wordPicker;
            _timerManager = timerManager;
            _context = context;
        }

        public async Task SendRoomMessage(string roomName, string userId, string userName, string message)
        {
            if(message.ToUpper() == _wordPicker.GetCachedWord(roomName).ToUpper())
            {
                await Clients.Group(roomName).SendAsync("ReceiveMessage", userId, userName, message);
                await StartRound(roomName);
            }
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
            await _context.Groups.RemoveFromGroupAsync(Context.ConnectionId, roomName);
            await _context.Clients.Group(roomName).SendAsync("ReceiveRoomSystemMessage", $"{Context.ConnectionId} has left the group");
        }

        public async Task Draw(object data, string roomName)
        {
            await _context.Clients.Groups(roomName).SendAsync("ReceiveDraw", data);
        }

        public async Task Clear(string roomName)
        {
            await _context.Clients.Groups(roomName).SendAsync("ClearData");
        }

        public override async Task OnDisconnectedAsync(Exception exception)
        {
            var user = _gameManager.GetUserBySignalR(Context.ConnectionId);
            await _context.Clients.Group(user.RoomId).SendAsync("ReceiveRoomSystemMessage", $"{user.Name} has left the group");
            await _gameManager.DisconnectUserBySignalrAsync(Context.ConnectionId);
            await base.OnDisconnectedAsync(exception);
        }

        public async Task StartRound(string roomId)
        {
            
            var userId = _playerPicker.SetPlayer(roomId);
            var words = _wordPicker.GetRandomWords(3);
            await Clear(roomId);
            await _context.Clients.Groups(roomId).SendAsync("StartRound", userId, words);
        }

        public async void StartGame(object sender, StartGameEventArgs args)
        {
            await StartRound(args.roomId);
        }

        public async void TimerElapsed(object sender, ElapsedEventArgs args)
        {
            var timer = sender as Timer;
            timer.Stop();
            var roomId = _timerManager.GetRoomId(timer);
            await StartRound(roomId);
        }

        public async Task Play(string word, string roomName)
        {
            _wordPicker.RemoveCachedWord(roomName);
            _wordPicker.CacheWord(roomName, word, 3);
            if(!_timerManager.StartTimer(roomName))
            {
                var timer = _timerManager.CreateTimer(roomName);
                timer.Elapsed += TimerElapsed;
                _timerManager.StartTimer(roomName);
            }

            
        }
    }
}

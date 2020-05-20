using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Krokodil.Models;
using Krokodil.Profiles;
using Krokodil.Services;
using Krokodil.Services.WordPicker;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;

namespace Krokodil.Controllers
{
    [Route("[action]")]
    [ApiController]
    public class GameController : ControllerBase
    {
        private readonly IGameManager _gameManager;
        private readonly ApplicationDbContext _context;
        private readonly IWordPicker _wp;

        public GameController(IGameManager gameManager,
            ApplicationDbContext context,
            IWordPicker wp)
        {
            _gameManager = gameManager;
            _context = context;
            _wp = wp;
        }

        [HttpGet]
        public IActionResult GetRandomWords()
        {
            return Ok(_wp.GetRandomWords(3));

        }

        [HttpGet]
        public async Task<IActionResult> GetRandomRoom()
        {
            var room = _gameManager.GetRandomRoom();

            if (room == null)
            {
                room = await _gameManager.InitializeRoomAsync(false);
            }

            return Ok(room.Id);
        }

        [HttpGet]
        public async Task<IActionResult> GetPrivateRoom() 
        {
            var room = await _gameManager.InitializeRoomAsync(true);
            if (room == null)
                return BadRequest();
            return Ok(room.Id);
        }

        [HttpPost("{roomId}")]
        public async Task<IActionResult> ConnectToRoom(User user, string roomId)
        {
            if(user.Name == default || user.Id == default)
            {
                return BadRequest();
            }

            var newUser = await _gameManager.ConnectUserAsync(user, roomId);

            if (newUser != null)
                return Ok();

            return BadRequest();
        }

        [HttpPost("{userId}")]
        public async Task<IActionResult> Disconnect(string userId)
        {
            await _gameManager.DisconnectUserByIdAsync(userId);

            return Ok();
        }

        public IActionResult GetAll()
        {
            var rooms = _context.Rooms.ToList();
            var users = _context.Users.ToList();
            foreach(var user in users)
            {
                user.Room = null;
            }

            return Ok(new { rooms, users });
        }
    }
}
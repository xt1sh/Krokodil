using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Krokodil.Profiles;
using Krokodil.Services;
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

        public GameController(IGameManager gameManager)
        {
            _gameManager = gameManager;
        }

        [HttpGet]
        public async Task<IActionResult> GetRandomRoom([FromQuery] string userId)
        {
            var room = _gameManager.GetRandomRoom();

            if(room == null)
            {
                room = await _gameManager.InitializeRoomAsync(false);
            }

            return Ok(room.Id);
        }
    }
}
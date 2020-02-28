using Krokodil.Profiles;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Krokodil.Services.PlayerPicker
{
	public class PlayerPicker : IPlayerPicker
	{
		private readonly ApplicationDbContext _context;
		private readonly IMemoryCache _cache;

		public PlayerPicker(ApplicationDbContext context,
			IMemoryCache memoryCache)
		{
			_context = context;
			_cache = memoryCache;
		}


		public string SetPlayer(string roomId)
		{
			using (var context = new ApplicationDbContext(new DbContextOptionsBuilder<ApplicationDbContext>().UseSqlite("Data Source=db.db;").Options))
			{
				var users = context.Users.Where(u => u.RoomId == roomId).ToList();

				var rnd = new Random();
				var nextPlayer = users.Skip(rnd.Next(users.Count())).First();
				string userId;
				if (_cache.TryGetValue(string.Concat(roomId, "U"), out userId))
				{
					while (nextPlayer.Id == userId)
					{
						nextPlayer = users.Skip(rnd.Next(users.Count())).First();
					}
					this.removeCachedPlayer(roomId);
					this.cachePlayerId(roomId, nextPlayer.Id, 3);
					return nextPlayer.Id;
				}
				this.cachePlayerId(roomId, nextPlayer.Id, 3);
				return nextPlayer.Id;
			}
		}

		private void cachePlayerId(string key, string userId, double minutes)
		{
			_cache.Set(string.Concat(key, "U"), userId, new MemoryCacheEntryOptions
			{
				AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(minutes)
			});
		}

		private void removeCachedPlayer(string key)
		{
			_cache.Remove(string.Concat(key, "U"));
		}
	}
}

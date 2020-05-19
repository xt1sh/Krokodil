using System;
using System.Timers;
using System.Collections.Generic;

namespace Krokodil.Services.TimerControls
{
	public class TimerControls : ITimerControls
	{
		private Dictionary<string, Timer> _timers;

		public TimerControls()
		{}


		public Timer GetTimerByRoomId(string id)
		{
			if (_timers.ContainsKey(id))
			{
				return _timers[id];
			}

			return null;
		}


		public void CreateTimer(string roomId)
		{
			if (!_timers.ContainsKey(roomId))
			{
				_timers.Add(roomId, new Timer());
			}
		}


		public void RemoveTimer(string roomId)
		{
			if (_timers.ContainsKey(roomId))
			{
				_timers.Remove(roomId);
			}
		}
	}
}


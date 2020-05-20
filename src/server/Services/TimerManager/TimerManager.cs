using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Timers;

namespace Krokodil.Services.TimerManager
{
	public class TimerManager : ITimerManager
	{
		private Dictionary<Timer, string> _timers;

		public TimerManager()
		{
			_timers = new Dictionary<Timer, string>();
		}
		public Timer CreateTimer(string roomId)
		{
			var timer = new Timer(30000);
			_timers.Add(timer, roomId);
			return timer;

		}

		public string GetRoomId(Timer timer)
		{
			if (_timers.ContainsKey(timer))
			{
				return _timers[timer];
			}

			return null;
		}

		public bool StartTimer(string roomId)
		{
			if (_timers.Count == 0)
			{
				return false;
			}
			var timer = _timers.Where(i => i.Value == roomId).Select(i => i.Key).FirstOrDefault();
			if (timer is null)
			{
				return false;
			}
			timer.Start();
			return true;
		}

		public bool StopTimer(string roomId)
		{
			var timer = _timers.Where(i => i.Value == roomId).Select(i => i.Key).FirstOrDefault();
			if (timer is null)
			{
				return false;
			}
			timer.Stop();
			return true;
		}

		
	}
}

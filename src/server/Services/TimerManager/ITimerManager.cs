using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Timers;

namespace Krokodil.Services
{
	public interface ITimerManager
	{
		Timer CreateTimer(string roomId);
		string GetRoomId(Timer timer);
		bool StartTimer(string roomId);
		bool StopTimer(string roomId);
	}
}

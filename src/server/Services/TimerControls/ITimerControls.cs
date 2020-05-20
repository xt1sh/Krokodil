using System;
using System.Timers;

namespace Krokodil.Services.TimerControls
{
	public interface ITimerControls
	{
		Timer GetTimerByRoomId(string id);
		void CreateTimer(string roomId);
		void RemoveTimer(string roomId);
	}
}


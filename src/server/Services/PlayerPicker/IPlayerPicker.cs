using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Krokodil.Services.PlayerPicker
{
	public interface IPlayerPicker
	{
		string SetPlayer(string roomId);
		string GetDrawer(string roomId);
	}
}

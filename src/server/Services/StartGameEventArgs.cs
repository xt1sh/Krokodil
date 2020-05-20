using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Krokodil.Services
{
	public class StartGameEventArgs: EventArgs
	{
		public string roomId { get; set; }
	}
}

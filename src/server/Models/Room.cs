using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Krokodil.Models
{
    public class Room
    {
        public string Id { get; set; }
        public IEnumerable<User> Users { get; set; }
        public bool IsPrivate { get; set; }
        public int Round { get; set; }
        public DateTime TimeStarted { get; set; }
    }
}

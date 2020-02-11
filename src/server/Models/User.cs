﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Krokodil.Models
{
    public class User
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public int Score { get; set; }
        public string RoomId { get; set; }
        public Room Room { get; set; }
    }
}

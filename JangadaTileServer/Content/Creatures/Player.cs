﻿using JangadaTileServer.Network;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JangadaTileServer.Content.Creatures
{
    class Player : Creature
    {
        public JWebClient Client { get; set; }

    }
}

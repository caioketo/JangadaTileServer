using JangadaTileServer.Content.Creatures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JangadaTileServer.Content.World
{
    class Area
    {
        public int Id { get; set; }
        public Guid Guid { get; set; }
        public Terrain Terrain { get; set; }
        public List<Creature> Creatures { get; set; }
        public List<Player> Players { get; set; }

        public Area(int id)
        {
            this.Id = id;
            this.Guid = Util.GenGuid();
            this.Creatures = new List<Creature>();
            this.Players = new List<Player>();
        }
    }
}

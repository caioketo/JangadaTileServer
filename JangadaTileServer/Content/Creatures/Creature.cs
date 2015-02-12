using JangadaTileServer.Content.Utils;
using JangadaTileServer.Content.World;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JangadaTileServer.Content.Creatures
{
    class Creature
    {
        public int CreatureId { get; set; }
        public Guid CreatureGuid { get; set; }
        public Utils.Position Position { get; set; }
        public int Direction { get; set; }
        public Area Area { get; set; }
        public Stats Stats { get; set; }
        public int Health { get; set; }
        public int Mana { get; set; }
        public List<Skills> Skills { get; set; }

        public Creature(int creatureId)
        {
            this.CreatureId = creatureId;
            this.Skills = new List<Skills>();
        }

        public bool IsVisible(Utils.Position position)
        {
            return (position.X >= this.Position.X - 19 && position.X <= this.Position.X + 19 &&
                position.Y >= this.Position.Y - 13 && position.Y <= this.Position.Y + 13 &&
                position.Z == this.Position.Z);
        }
    }
}

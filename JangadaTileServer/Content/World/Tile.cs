using JangadaTileServer.Content.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JangadaTileServer.Content.World
{
    class Tile
    {
        public Item Ground { get; set; }
        public List<Item> Items { get; set; }
        public Position Position { get; set; }

        public Tile(Position position)
        {
            this.Items = new List<Item>();
            this.Position = position;
        }
    }
}

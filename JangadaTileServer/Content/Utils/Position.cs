using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JangadaTileServer.Content.Utils
{
    public class Position
    {
        public Position(Position pos)
        {
            this.X = pos.X;
            this.Y = pos.Y;
            this.Z = pos.Z;
        }
        public Position(int x, int y, int z)
        {
            this.X = x;
            this.Y = y;
            this.Z = z;
        }
        public int X { get; set; }
        public int Y { get; set; }
        public int Z { get; set; }
    }
}

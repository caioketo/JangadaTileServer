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

        public static double Distance(Position p1, Position p2)
        {
            return Math.Sqrt((p2.X - p1.X) * (p2.X - p1.X) + (p2.Y - p1.Y) * (p2.Y - p1.Y));
        }

        public static JangadaTileServer.Content.Utils.Enums.Direction GetDirectionPosition(Position p1, Position p2)
        {
            if (p1.X > p2.X)
            {
                if (p1.Y > p2.Y)
                {
                    return Enums.Direction.NORTH_WEST;
                }
                else if (p1.Y < p2.Y)
                {
                    return Enums.Direction.SOUTH_WEST;
                }
                else
                {
                    return Enums.Direction.WEST;
                }
            }
            else if (p1.X < p2.X)
            {
                if (p1.Y > p2.Y)
                {
                    return Enums.Direction.NORTH_EAST;
                }
                else if (p1.Y < p2.Y)
                {
                    return Enums.Direction.SOUTH_EAST;
                }
                else
                {
                    return Enums.Direction.EAST;
                }
            }
            else
            {
                if (p1.Y > p2.Y)
                {
                    return Enums.Direction.NORTH;
                }
                else if (p1.Y < p2.Y)
                {
                    return Enums.Direction.SOUTH;
                }
            }
            return Enums.Direction.NONE;
        }
    }
}

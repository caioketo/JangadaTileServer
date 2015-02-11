using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JangadaTileServer.Content.World
{
    class Terrain
    {
        public Tile[,] Tiles { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }

        public Terrain(int width, int height)
        {
            this.Width = width;
            this.Height = height;
            this.Tiles = new Tile[this.Width, this.Height];
        }
    }
}

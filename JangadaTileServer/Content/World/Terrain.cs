using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JangadaTileServer.Content.World
{
    class Terrain
    {
        public int Id { get; set; }
        public Tile[,] Tiles { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }

        public Terrain(int width, int height)
        {
            this.Width = width;
            this.Height = height;
            this.Tiles = new Tile[this.Width, this.Height];
        }

        public Terrain(int id)
        {
            this.Id = id;
            Util.LoadTerrain(this);
        }


        public Tile GetTile(int x, int y)
        {
            return Tiles[x, y];
        }
    }
}

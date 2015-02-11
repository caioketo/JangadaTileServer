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
            LoadTerrain();
        }


        private void LoadTerrain()
        {
            //LOAD FROM FILE
            using (FileStream sr = File.OpenRead(@"c:\Users\Caio\Desktop\teste.json"))
            {
                using (JsonTextReader reader = new JsonTextReader(new StreamReader(sr)))
                {
                    dynamic terrain = JToken.ReadFrom(reader);
                    this.Width = terrain.width;
                    this.Height = terrain.height;
                    this.Tiles = new Tile[this.Width, this.Height];
                    dynamic layer1 = terrain.layers[0];
                    int x = 0;
                    int y = 0;
                    for (int i = 0; i < this.Width * this.Height; i++)
                    {
                        if (x == this.Width)
                        {
                            x = 0;
                            y++;
                        }
                        this.Tiles[x, y] = new Tile(new Utils.Position(x, y, 1));
                        this.Tiles[x, y].Ground = new Item((int)layer1.data[i]);
                        x++;
                    }
                }
            }
        }

        public Tile GetTile(int x, int y)
        {
            return Tiles[x, y];
        }
    }
}

using JangadaTileServer.Content.World;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JangadaTileServer
{
    class Util
    {
        public static Guid GenGuid()
        {
            return Guid.NewGuid();
        }

        public static string Path()
        {
            return System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
        }

        public enum RES_TYPE
        {
            SCRIPT,
            TERRAIN,
            RESPAWN
        }

        public static string PathRes(RES_TYPE type)
        {
            switch (type)
            {
                case RES_TYPE.SCRIPT:
                    return Path() + @"\Res\Scripts\";
                case RES_TYPE.TERRAIN:
                    return Path() + @"\Res\Terrains\";
                case RES_TYPE.RESPAWN:
                    return Path() + @"\Res\";
                default:
                    return "";
            }
        }

        public static void LoadTerrain(Terrain terrain)
        {
            //LOAD FROM FILE
            using (FileStream sr = File.OpenRead(PathRes(RES_TYPE.TERRAIN) + terrain.Id.ToString() + ".json"))
            {
                using (JsonTextReader reader = new JsonTextReader(new StreamReader(sr)))
                {
                    dynamic terrainJSON = JToken.ReadFrom(reader);
                    terrain.Width = terrainJSON.width;
                    terrain.Height = terrainJSON.height;
                    terrain.Tiles = new Tile[terrain.Width, terrain.Height];
                    dynamic layer1 = terrainJSON.layers[0];
                    int x = 0;
                    int y = 0;
                    for (int i = 0; i < terrain.Width * terrain.Height; i++)
                    {
                        if (x == terrain.Width)
                        {
                            x = 0;
                            y++;
                        }
                        terrain.Tiles[x, y] = new Tile(new JangadaTileServer.Content.Utils.Position(x, y, 1));
                        if (x == 60 && y == 40)
                        {
                            terrain.Tiles[x, y].Ground = new JangadaTileServer.Content.Item(170);
                        }
                        else
                        {
                            terrain.Tiles[x, y].Ground = new JangadaTileServer.Content.Item((int)layer1.data[i]);
                        }
                        x++;
                    }
                }
            }
        }

        public static List<Respawn> LoadRespawns()
        {
            //LOAD FROM FILE
            List<Respawn> Respawns = new List<Respawn>();
            using (FileStream sr = File.OpenRead(PathRes(RES_TYPE.RESPAWN) + @"\respawns.json"))
            {
                using (JsonTextReader reader = new JsonTextReader(new StreamReader(sr)))
                {
                    dynamic respawns = JToken.ReadFrom(reader);
                    int respCount = respawns.count;
                    for (int r = 0; r < respCount; r++)
                    {
                        dynamic respawnJSON = respawns.respawns[r];

                        Respawn resp = new Respawn();
                        resp.AreaId = respawnJSON.areaId;
                        int creatures = respawnJSON.creaturesCount;
                        for (int i = 0; i < creatures; i++)
                        {
                            int creatureId = respawnJSON.creatures[i].id;
                            int creatureQty = respawnJSON.creatures[i].qty;
                            resp.CreaturesIdToRespawn.Add(creatureId);
                            resp.CreaturesQtyToRespawn.Add(creatureQty);
                        }
                        int q1x = respawnJSON.q1.x;
                        int q1y = respawnJSON.q1.y;
                        int q1z = respawnJSON.q1.z;
                        int q2x = respawnJSON.q2.x;
                        int q2y = respawnJSON.q2.y;
                        int q2z = respawnJSON.q2.z;
                        resp.Q1 = new JangadaTileServer.Content.Utils.Position(q1x, q1y, q1z);
                        resp.Q2 = new JangadaTileServer.Content.Utils.Position(q2x, q2y, q2z);
                        resp.RespawnTime = respawnJSON.respawnTime;

                        Respawns.Add(resp);
                    }
                }
            }
            return Respawns;
        }
    }
}

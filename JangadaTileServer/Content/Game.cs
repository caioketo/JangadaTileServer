using JangadaTileServer.Content.Creatures;
using JangadaTileServer.Content.World;
using JangadaTileServer.Network;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JangadaTileServer.Content
{
    class Game
    {
        public List<Area> Areas { get; set; }
        List<Respawn> Respawns = new List<Respawn>();
        private static Game instance;
        public static Game GetInstance()
        {
            if (instance == null)
            {
                instance = new Game();
            }
            return instance;
        }

        public Game()
        {
            this.Areas = new List<Area>();
            instance = this;
            this.Areas.Add(new Area(1));
            LoadRespawns();
        }

        private void LoadRespawns()
        {
            //LOAD FROM FILE
            /*Respawn resp = new Respawn();
            resp.AreaId = 1;
            resp.CreaturesIdToRespawn.Add(1);
            resp.CreaturesQtyToRespawn.Add(5);
            resp.Q1 = new Utils.Position(1, 2, 1);
            resp.Q2 = new Utils.Position(10, 6, 1);
            resp.RespawnTime = 10000;
            Respawns.Add(resp);*/
            Respawns = Util.LoadRespawns();
            foreach (Respawn respawn in Respawns)
            {
                respawn.Run();
            }
            new Utils.Skills(1);
            new Utils.Skills(2);
        }

        internal void OnPlayerLogin(Network.JWebClient client)
        {
            client.Player = new Creatures.Player(1, new Utils.Position(40, 40, 1), client);
            this.Areas[0].AddPlayer(client.Player);
            MessageHelper.SendInitialPacket(client);

            foreach (Player player in client.Player.Area.PlayersInViewArea(client.Player.Position))
            {
                if (player.CreatureGuid != client.Player.CreatureGuid)
                {
                    MessageHelper.SendPlayerLogin(player.Client, client.Player);
                }
            }
        }


        internal void OnPlayerMove(JWebClient client, RequestMovementPacket.Types.MovementType movementType)
        {
            client.Player.Walk(movementType);
        }

        internal void CreatureMoved(Creature creature)
        {
            if (creature is Player)
            {
                MessageHelper.SendPlayerMove(((Player)creature).Client);

                foreach (Player player in creature.Area.PlayersInViewArea(creature.Position))
                {
                    if (player.CreatureGuid != creature.CreatureGuid)
                    {
                        MessageHelper.SendCharacterMove(player.Client, (Player)creature);
                    }
                }
            }
            else
            {
                foreach (Player player in creature.Area.PlayersInViewArea(creature.Position))
                {
                    if (player.CreatureGuid != creature.CreatureGuid)
                    {
                        MessageHelper.SendCreatureMove(player.Client, creature);
                    }
                }
            }
        }

        internal Area GetArea(int AreaId)
        {
            foreach (Area area in Areas)
            {
                if (area.Id == AreaId)
                {
                    return area;
                }
            }
            return null;
        }

        internal void OnAddCreature(Area area, Creature creature)
        {
            foreach (Player player in area.GetPlayers())
            {
                MessageHelper.SendCreatureRespawn(player.Client, creature);
            }
        }
    }
}

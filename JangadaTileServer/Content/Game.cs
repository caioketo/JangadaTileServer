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
                        MessageHelper.SendCreatureMove(player.Client, (Player)creature);
                    }
                }
            }
        }
    }
}

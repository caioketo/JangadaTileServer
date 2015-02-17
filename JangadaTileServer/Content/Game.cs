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
            Utils.Position newPos = client.Player.Position;
            switch (movementType)
            {
                case RequestMovementPacket.Types.MovementType.UP:
                    newPos.Y--;
                    break;
                case RequestMovementPacket.Types.MovementType.DOWN:
                    newPos.Y++;
                    break;
                case RequestMovementPacket.Types.MovementType.LEFT:
                    newPos.X--;
                    break;
                case RequestMovementPacket.Types.MovementType.RIGHT:
                    newPos.X++;
                    break;
                default:
                    break;
            }
            if (newPos.X < 0)
            {
                newPos.X = 0;
            }
            if (newPos.Y < 0)
            {
                newPos.Y = 0;
            }
            client.Player.Position = newPos;
            MessageHelper.SendPlayerMove(client, movementType);
            foreach (Player player in client.Player.Area.PlayersInViewArea(newPos))
            {
                if (player.CreatureGuid != client.Player.CreatureGuid)
                {
                    MessageHelper.SendCreatureMove(player.Client, client.Player);
                }
            }
        }
    }
}

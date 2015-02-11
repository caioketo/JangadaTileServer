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
            client.Player = new Creatures.Player(1, new Utils.Position(40, 40, 1));
            client.Player.Area = this.Areas[0];
            MessageHelper.SendAreaDescription(client, this.Areas[0]);
        }
    }
}

using JangadaTileServer.Content.Creatures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JangadaTileServer.Content.World
{
    class Area
    {
        public int Id { get; set; }
        public Guid Guid { get; set; }
        public Terrain Terrain { get; set; }
        public List<Creature> Creatures { get; set; }
        public List<Player> Players { get; set; }

        public List<Player> GetPlayers()
        {
            return Players;
        }

        public Area(int id)
        {
            this.Id = id;
            this.Guid = Util.GenGuid();
            this.Creatures = new List<Creature>();
            this.Players = new List<Player>();
            this.Terrain = new Terrain(id);
        }

        public List<Player> PlayersInViewArea(Utils.Position position)
        {
            List<Player> players = new List<Player>();

            foreach (Player player in Players)
            {
                if (player.IsVisible(position))
                {
                    players.Add(player);
                }
            }


            return players;
        }

        internal void AddPlayer(Player player)
        {
            player.Area = this;
            Players.Add(player);
        }


        public int GetCreaturesCountInArea(int creatureId, Utils.Position Q1, Utils.Position Q2)
        {
            int count = 0;
            foreach (Creature creature in Creatures)
            {
                if (creature.CreatureId == creatureId &&
                    ((creature.Position.X >= Q1.X && creature.Position.X <= Q2.X) &&
                    (creature.Position.Y >= Q1.Y && creature.Position.Y <= Q2.Y)))
                {
                    count++;
                }
            }

            return count;
        }

        public void AddCreature(Creature creature)
        {
            creature.Area = this;
            Creatures.Add(creature);
            Game.GetInstance().OnAddCreature(this, creature);
        }

        public bool CreatureInPos(Utils.Position pos)
        {
            foreach (Creature creature in Creatures)
            {
                if (creature.Position.X == pos.X && creature.Position.Y <= pos.Y)
                {
                    return true;
                }
            }

            foreach (Player player in Players)
            {
                if (player.Position.X == pos.X && player.Position.Y == pos.Y)
                {
                    return true;
                }
            }

            return false;
        }
    }
}

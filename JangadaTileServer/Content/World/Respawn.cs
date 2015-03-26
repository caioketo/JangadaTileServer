using JangadaTileServer.Content.Creatures;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace JangadaTileServer.Content.World
{
    public class Respawn
    {
        public int RespawnTime;
        public List<int> CreaturesIdToRespawn = new List<int>();
        public List<int> CreaturesQtyToRespawn = new List<int>();
        public int AreaId { get; set; }
        public Utils.Position Q1 { get; set; }
        public Utils.Position Q2 { get; set; }
        private Random random;
        private BackgroundWorker worker;

        public Respawn()
        {
            random = new Random();
            worker = new BackgroundWorker();
            worker.DoWork += worker_DoWork;
        }

        void timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            int totalCount = 0;
            //Check if still alive the monsters
            foreach (int creatureId in CreaturesIdToRespawn)
            {
                totalCount += Game.GetInstance().GetArea(AreaId).GetCreaturesCountInArea(creatureId, Q1, Q2);
            }


            if (!worker.IsBusy && totalCount <= 0)
                worker.RunWorkerAsync();
        }

        void worker_DoWork(object sender, DoWorkEventArgs e)
        {
            //RESPAWN CREATURES
            for (int i = 0; i < CreaturesIdToRespawn.Count; i++)
            {
                int creatureId = CreaturesIdToRespawn[i];
                int qty = CreaturesQtyToRespawn[i];
                for (int q = 0; q < qty; q++)
                {
                    Creature creatureToRespawn = new Creature(creatureId, RandomPosition());
                    Game.GetInstance().GetArea(AreaId).AddCreature(creatureToRespawn);
                }
                Console.WriteLine("Resp: " + creatureId + " qty: " + qty);
            }
        }

        int NextInt(int min, int max)
        {
            return random.Next(min, max);
        }

        internal Utils.Position RandomPosition()
        {
            int x = NextInt(Q1.X, Q2.X);
            int y = NextInt(Q1.Y, Q2.Y);
            return new Utils.Position(x, y, Q1.Z);
        }

        public void Run()
        {
            if (RespawnTime > 0)
            {
                Timer timer = new Timer(RespawnTime);
                timer.Elapsed += timer_Elapsed;
                timer.Start();
            } 
        }
    }
}

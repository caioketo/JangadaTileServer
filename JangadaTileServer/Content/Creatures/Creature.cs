using JangadaTileServer.Content.Creatures.AI;
using JangadaTileServer.Content.Scripting;
using JangadaTileServer.Content.Utils;
using JangadaTileServer.Content.World;
using JangadaTileServer.Network;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace JangadaTileServer.Content.Creatures
{
    class Creature
    {
        public int CreatureId { get; set; }
        public Guid CreatureGuid { get; set; }
        public string Name { get; set; }
        public Utils.Position Position { get; set; }
        public int Direction { get; set; }
        public Area Area { get; set; }
        public Stats Stats { get; set; }
        public int Health { get; set; }
        public int Mana { get; set; }
        public List<Skills> Skills { get; set; }
        public RequestMovementPacket.Types.MovementType LastMoveType { get; set; }
        public Creature Target { get; set; }
        public Task AttackTask { get; set; }
        public CancellationTokenSource AttackCancellationToken { get; set; }
        public SimpleAI AI { get; set; }
        public Task AITask { get; set; }


        public int Speed
        {
            get
            {
                return (this.Stats.Get(Stats.STAT.DEX));
            }
        }

        public Creature(int creatureId)
        {
            this.CreatureId = creatureId;
            this.CreatureGuid = Util.GenGuid();
            this.Skills = new List<Skills>();
            this.Stats = new Stats();
            this.Name = "Keto";            
        }

        public Creature(int creatureId, Utils.Position position)
        {
            this.CreatureId = creatureId;
            this.CreatureGuid = Util.GenGuid();
            this.Skills = new List<Skills>();
            this.Stats = new Stats();
            this.Name = "Keto";
            this.Position = position;
            if (!(this is Player))
            {
                this.AI = new SimpleAI(this);
                this.AITask = Task.Factory.StartNew(() =>
                {
                    while (true)
                    { 
                    Thread.Sleep(2000);
                    this.AI.Run();
                        }
                });
            }
        }

        public bool IsVisible(Utils.Position position)
        {
            return (position.X >= this.Position.X - 19 && position.X <= this.Position.X + 19 &&
                position.Y >= this.Position.Y - 13 && position.Y <= this.Position.Y + 13 &&
                position.Z == this.Position.Z);
        }

        public void Attack(Creature target)
        {
            Console.WriteLine("Attack command!");
            if (this.AttackTask != null && this.AttackTask.Status == TaskStatus.Running)
            {
                Console.WriteLine("Task != null && Status == Running");
                this.AttackCancellationToken.Cancel();
            }
            this.AttackCancellationToken = new CancellationTokenSource();
            CancellationToken ct = this.AttackCancellationToken.Token;
            this.Target = target;
            int atkSpeed = 2000;
            this.AttackTask = Task.Factory.StartNew(() =>
            {
                AttackMethod(atkSpeed);
            }, ct);
        }

        public void AttackMethod(int atkSpeed)
        {
            Console.WriteLine("Started Attack Task!");
            Thread.Sleep(atkSpeed);
            if (this.Target == null)
            {
                this.AttackCancellationToken.Cancel();
                Console.WriteLine("Cancelled Attack Task");
            }
        }

        public void Walk(RequestMovementPacket.Types.MovementType movementType)
        {
            this.LastMoveType = movementType;
            Utils.Position newPos = new Utils.Position(this.Position);
            

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

            if (this.Area.CreatureInPos(newPos))
            {
                if (this is Player)
                {
                    MessageHelper.SendNotPossible((Player)this);
                }
                else
                {
                    this.Walk(RequestMovementPacket.Types.MovementType.DOWN);
                }
                return;
            }
            this.Position = newPos;
            Game.GetInstance().CreatureMoved(this);

            //int interval = 5000;
            //Task.Factory.StartNew(() =>
            //{
                //Thread.Sleep(interval);
                //this.Position = newPos;
                //Game.GetInstance().CreatureMoved(this);
            //});
        }

        public void CastSkill(int SkillId)
        {
            //TODO CAST SKILL TESTAR LUA FUNCTIONS
            Skills skillToCast = null;
            foreach (Skills skill in this.Skills)
            {
                if (skill.Id == SkillId)
                {
                    skillToCast = skill;
                }
            }
            if (skillToCast == null)
            {
                return;
            }
            ScriptManager.GetInstance().ExecuteScript(skillToCast.CastFunction, this);
        }

    }
}

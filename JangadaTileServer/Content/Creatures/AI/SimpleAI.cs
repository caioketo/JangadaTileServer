using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JangadaTileServer.Content.Creatures.AI
{
    class SimpleAI
    {
        public enum AIState
        {
            STOP,
            WALK,
            ATACK
        }

        public AIState State { get; set; }
        public Creature Creature { get; set; }

        public SimpleAI(Creature creature)
        {
            this.State = AIState.WALK;
            this.Creature = creature;
        }


        public void Targetting()
        {
            Player closest = null;
            double closestDist = 999;
            List<Player> players = this.Creature.Area.PlayersInViewArea(this.Creature.Position);
            foreach (Player player in players)
            {
                if (closest == null)
                {
                    closest = player;
                    closestDist = Utils.Position.Distance(this.Creature.Position, player.Position);
                }
                else
                {
                    if (Utils.Position.Distance(this.Creature.Position, player.Position) < closestDist)
                    {
                        closest = player;
                        closestDist = Utils.Position.Distance(this.Creature.Position, player.Position);
                    }
                }
            }
            if (closest != null)
            {
                this.Creature.Target = closest;
                if (Utils.Position.Distance(this.Creature.Position, this.Creature.Target.Position) <= 1.5f)
                {
                    this.State = AIState.ATACK;
                }
                else
                {
                    this.State = AIState.WALK;
                }
            }
            else
            {
                this.State = AIState.STOP;
            }
        }

        public void Run()
        {
            if (this.Creature == null)
            {
                return;
            }

            if (this.Creature.Target == null)
            {
                Targetting();
            }

            switch (State)
            {
                case AIState.WALK:
                    if (Utils.Position.Distance(this.Creature.Position, this.Creature.Target.Position) <= 1.5f)
                    {
                        this.State = AIState.ATACK;
                    }
                    else
                    {
                        JangadaTileServer.Content.Utils.Enums.Direction dirTarget = Utils.Position.GetDirectionPosition(
                            this.Creature.Position, this.Creature.Target.Position);
                        RequestMovementPacket.Types.MovementType? move = null;
                        switch (dirTarget)
                        {
                            case JangadaTileServer.Content.Utils.Enums.Direction.NORTH:
                                move = RequestMovementPacket.Types.MovementType.UP;
                                break;
                            case JangadaTileServer.Content.Utils.Enums.Direction.EAST:
                            case JangadaTileServer.Content.Utils.Enums.Direction.NORTH_EAST:
                            case JangadaTileServer.Content.Utils.Enums.Direction.SOUTH_EAST:
                                move = RequestMovementPacket.Types.MovementType.RIGHT;
                                break;
                            case JangadaTileServer.Content.Utils.Enums.Direction.WEST:
                            case JangadaTileServer.Content.Utils.Enums.Direction.NORTH_WEST:
                            case JangadaTileServer.Content.Utils.Enums.Direction.SOUTH_WEST:
                                move = RequestMovementPacket.Types.MovementType.LEFT;
                                break;
                            case JangadaTileServer.Content.Utils.Enums.Direction.SOUTH:
                                move = RequestMovementPacket.Types.MovementType.DOWN;
                                break;
                            default:
                                break;
                        }
                        if (move.HasValue)
                        {
                            this.Creature.Walk(move.Value);
                        }
                    }
                    break;
                case AIState.ATACK:
                    if (Utils.Position.Distance(this.Creature.Position, this.Creature.Target.Position) > 1.5f)
                    {
                        this.State = AIState.WALK;
                    }
                    break;
                default:
                    break;
            }
        }
    }
}

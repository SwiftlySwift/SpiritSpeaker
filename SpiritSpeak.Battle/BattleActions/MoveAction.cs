using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SpiritSpeak.Combat.BattleActions
{
    public class MoveAction : BattleAction
    {
        public Spirit Target { get; set; }

        public List<Point> Movements { get; set; }

        public MoveAction() : base()
        {
            Movements = new List<Point>();
        }

        public override BattleActionResult DoAction(Battle battle)
        {
            var result = new BattleActionResult();
            var movementResult = new MoveActionResult();

            if (Target != null && Movements.Any())
            {
                movementResult.DesiredMoves = Movements;
                movementResult.Origin = Target.GridLocation;
                movementResult.Target = Target;
                var idx = 0;
                foreach (var move in Movements)
                {
                    var succeeded = battle.MoveSpirit(Target, move);
                    if (!succeeded)
                    {
                        break; //If something blocked our move, stop moving. 
                    }
                    idx++;
                }
                movementResult.ActualMoves = Movements.Take(idx).ToList(); //Trim to just the successful moves
                movementResult.Destination = Target.GridLocation;
            }

            result.MoveActionResults.Add(movementResult);

            return result;
        }
    }
}

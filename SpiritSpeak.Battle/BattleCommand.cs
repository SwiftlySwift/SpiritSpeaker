using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SpiritSpeak.Combat
{
    public class BattleCommand
    {
        public string DebugMessage { get; set; }
        public Spirit Source { get; set; }
        public List<Point> Movements { get; set; }
        public BattleAction Action { get; set; }

        public BattleCommand()
        {
            Movements = new List<Point>();
            Action = new BattleAction();
        }

        public BattleActionResult DoAction(Battle battle)
        {
            var result = new BattleActionResult();
            result.DebugMessage = DebugMessage;

            if (Source == null)
            {
                return result;
            }

            //Handle Movements
            if (Movements != null)
            {
                result.DesiredMoves = Movements;
                result.Origin = Source.GridLocation;
                result.Source = Source;
                var idx = 0;
                foreach(var move in Movements)
                {
                    var succeeded = battle.MoveSpirit(Source, move);
                    if (!succeeded)
                    {
                        break; //If something blocked our move, stop moving. 
                    }
                    idx++;
                }
                result.ActualMoves = Movements.Take(idx).ToList(); //Trim to just the successful moves
                result.Destination = Source.GridLocation;
            }

            if (Action.Target != null && Source.InRangeOf(Action.Target))
            {
                var damage = Action.Target.TakeDamage(Action.Damage);

                result.DamageResults.Add(new DamageResult()
                {
                    Amount = damage,
                    Source = Source,
                    Target = Action.Target
                });
            }

            return result;
        }
    }
}
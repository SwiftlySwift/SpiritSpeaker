using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SpiritSpeak.Combat
{
    public class BattleAction
    {
        public string DebugMessage { get; set; }

        public Spirit Target { get; set; }

        public Spirit Source { get; set; }

        public int Damage { get; set; }

        public List<Point> Movements { get; set; }

        public BattleAction()
        {
            Movements = new List<Point>();
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

            if (Target != null && Source.InRangeOf(Target))
            {
                var damage = Target.TakeDamage(Damage);

                result.DamageResults.Add(new DamageResult()
                {
                    Amount = damage,
                    Source = Source,
                    Target = Target
                });
            }

            return result;
        }
    }
}
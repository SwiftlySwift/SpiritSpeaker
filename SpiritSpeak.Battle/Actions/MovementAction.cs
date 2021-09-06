using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SpiritSpeak.Combat.Actions
{
    public class MovementAction : BaseAction
    {
        public List<Point> Movements { get; set; }
        public bool IgnoreTerrain { get; set; }
        public bool Shove { get; set; } // Shoves are movements as a result of a skill, not movement as a result of a character moving.

        public MovementResult GetResult(Battle battle)
        {
            var result = new MovementResult();

            //Handle Movements
            if (Movements != null)
            {
                result.DesiredMoves = Movements;
                result.Origin = Source.GridLocation;
                result.Source = Source;
                var idx = 0;
                foreach (var move in Movements)
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
                result.Shove = Shove;
            }

            return result;
        }
    }
}

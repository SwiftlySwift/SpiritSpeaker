using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace SpiritSpeak.Combat
{
    public class BattleActionResult
    {
        public string DebugMessage { get; set; }

        public List<DamageResult> DamageResults { get; set; }

        public Spirit Source { get; set; }

        public List<Point> DesiredMoves { get; set; }
        public List<Point> ActualMoves { get; set; }

        public Point Origin { get; set; }
        public Point Destination { get; set; }

        public BattleActionResult()
        {
            DamageResults = new List<DamageResult>();
        }
    }
}
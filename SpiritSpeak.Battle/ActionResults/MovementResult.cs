using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace SpiritSpeak.Combat.Actions
{
    public class MovementResult
    {
        public Spirit Source { get; set; }
        public bool Shove { get; set; }

        public List<Point> DesiredMoves { get; set; }
        public List<Point> ActualMoves { get; set; }

        public Point Origin { get; set; }
        public Point Destination { get; set; }
    }
}
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace SpiritSpeak.Combat.BattleActions
{
    public class MoveActionResult
    {
        public Spirit Target { get; set; }

        public List<Point> DesiredMoves { get; set; }
        public List<Point> ActualMoves { get; set; }

        public Point Origin { get; set; }
        public Point Destination { get; set; }
    }
}

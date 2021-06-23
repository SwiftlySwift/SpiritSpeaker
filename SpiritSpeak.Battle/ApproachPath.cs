using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace SpiritSpeak.Combat
{
    public class ApproachPath
    {
        public List<Point> Movements { get; set; }
        public Point Target { get; set; }
        public bool AtTarget { get; set; }

        public ApproachPath()
        {
            Movements = new List<Point>();
        }
        
    }
}

using System;
using System.Collections.Generic;
using System.Text;

namespace SpiritSpeak.Combat
{
    public class BattleAction
    {
        public Spirit Target { get; set; }
        public int Damage { get; set; }
        public string AnimationName { get; set; }
    }
}

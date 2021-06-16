using System;
using System.Collections.Generic;
using System.Text;

namespace SpiritSpeak.Combat.Test
{
    public class DumbCommander : Commander
    {
        public DumbCommander() : base(2)
        {

        }

        public override BattleAction GetAction(Battle battle)
        {
            return new BattleAction()
            {
                DebugMessage = "uh....",
            };
        }
    }
}

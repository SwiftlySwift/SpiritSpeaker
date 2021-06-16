using System;
using System.Collections.Generic;
using System.Text;

namespace SpiritSpeak.Combat.Test
{
    public class SlowCommander : Commander
    {
        private int delay = 0;

        public SlowCommander():base(13)
        {

        }

        public override BattleAction GetAction(Battle battle)
        {
            delay++;
            if (delay < 3)
                return null;

            return new BattleAction()
            {
                DebugMessage = "oh!",
            };
        }
    }
}

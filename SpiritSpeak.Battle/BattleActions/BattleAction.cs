using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SpiritSpeak.Combat.BattleActions
{
    public class BattleAction
    {
        public List<BattleAction> ChildActions { get; set; }

        public BattleAction()
        {
            ChildActions = new List<BattleAction>();
        }

        public virtual BattleActionResult DoAction(Battle battle)
        {
            var result = new BattleActionResult();

            return result;
        }
    }
}
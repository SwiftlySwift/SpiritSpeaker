using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SpiritSpeak.Combat
{
    public class HumanCommander : Commander
    {
        private static Random _random = new Random();

        public bool ActionConfirmed { get; set; }

        public BattleAction BattleAction { get; set; }

        public HumanCommander(int teamId) : base(teamId)
        {

        }

        public override BattleAction GetAction(Battle battle)
        {
            if (BattleAction != null && ActionConfirmed)
            {
                var action = BattleAction;
                BattleAction = null;
                ActionConfirmed = false;
                return action;
            }
            else
            {
                return null;
            }
        }
    }
}
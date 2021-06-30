using Microsoft.Xna.Framework;
using SpiritSpeak.Combat.BattleActions;
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
            BattleAction = new BattleAction();
        }

        public override BattleAction GetAction(Battle battle)
        {
            if (ActionConfirmed)
            {
                var action = BattleAction;
                BattleAction = new BattleAction();
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
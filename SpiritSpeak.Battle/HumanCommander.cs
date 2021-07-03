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

        public BattleCommand BattleAction { get; set; }

        public HumanCommander(int teamId) : base(teamId)
        {
            BattleAction = new BattleCommand();
        }

        public override BattleCommand GetAction(Battle battle)
        {
            if (ActionConfirmed)
            {
                var action = BattleAction;
                BattleAction = new BattleCommand();
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
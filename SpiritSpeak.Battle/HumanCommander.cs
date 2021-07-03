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

        public BattleCommand Command { get; set; }

        public HumanCommander(int teamId) : base(teamId)
        {
            Command = new BattleCommand();
        }

        public override BattleCommand GetAction(Battle battle)
        {
            if (ActionConfirmed)
            {
                var action = Command;
                Command = new BattleCommand();
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
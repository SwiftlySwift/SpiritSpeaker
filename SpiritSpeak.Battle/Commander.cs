using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SpiritSpeak.Combat
{
    public class Commander
    {
        private static Random _random = new Random();

        public List<Spirit> Spirits { get; set; }
        public List<Speaker> Speakers { get; set; }
        public int Initiative { get; set; }
        public int TeamId { get; private set; }

        public Commander(int teamId)
        {
            Spirits = new List<Spirit>();
            Speakers = new List<Speaker>();

            TeamId = teamId;
        }

        public virtual BattleCommand GetAction(Battle battle)
        {
            var enemies = battle.GetEnemyTargets(TeamId);

            var mySpirit = Spirits[_random.Next(Spirits.Count)];

            if (mySpirit != null && enemies.Count > 0)
            {
                var randomEnemy = enemies[_random.Next(enemies.Count)];

                var approach = mySpirit.GetApproachPath(randomEnemy);

                var command = new BattleCommand();

                //if (approach != null)
                //{
                //    command.Movements = approach.Movements;
                //    if (approach.AtTarget)
                //    {
                //        action.Damage = mySpirit.Strength;
                //        action.Target = randomEnemy;
                //        action.AnimationName = "bop";
                //    }
                //}

                return command;
            }

            return new BattleCommand() {};
        }
    }
}
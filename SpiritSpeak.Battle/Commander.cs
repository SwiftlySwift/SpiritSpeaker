using Microsoft.Xna.Framework;
using SpiritSpeak.Combat.BattleActions;
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

        public virtual BattleAction GetAction(Battle battle)
        {
            var action = new BattleAction();

            var enemies = battle.GetEnemyTargets(TeamId);
            var mySpirit = Spirits[_random.Next(Spirits.Count)];

            if (mySpirit != null && enemies.Count > 0)
            {
                var randomEnemy = enemies[_random.Next(enemies.Count)];
                var approach = mySpirit.GetApproachPath(randomEnemy);

                var moveAction = new MoveAction()
                {
                    Target = mySpirit,
                    Movements = approach.Movements
                };
                var attackAction = new DamageAction()
                {
                    Target = randomEnemy,
                    Source = mySpirit,
                    Amount = mySpirit.Strength
                };
                moveAction.ChildActions.Add(attackAction); //Move, then attack if possible.
            }

            return action;
        }
    }
}
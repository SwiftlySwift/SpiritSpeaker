using Microsoft.Xna.Framework;
using SpiritSpeak.Combat.Actions;
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

                if (approach.Movements.Count > 0)
                {
                    var moveAction = new MovementAction()
                    {
                        IgnoreTerrain = false,
                        Movements = approach.Movements,
                        Shove = false,
                        Source = mySpirit,
                        Targetting = null
                    };
                    command.MovementActions.Add(moveAction);
                }
                if (approach.AtTarget)
                {
                    var attackAction = new DamageAction()
                    {
                        Source = mySpirit,
                        Targetting = new Targetting()
                        {
                            DirectTargets = new List<Spirit>() { randomEnemy }
                        },
                        Amount = mySpirit.Strength
                    };
                    var animationAction = new AnimationAction()
                    {
                        Source = mySpirit,
                        Targetting = new Targetting()
                        {
                            DirectTargets = new List<Spirit>() { randomEnemy }
                        },
                        Animation = Animation.Bonk
                    };

                    command.DamageActions.Add(attackAction);
                    command.AnimationActions.Add(animationAction);
                }

                return command;
            }

            return new BattleCommand() {};
        }
    }
}
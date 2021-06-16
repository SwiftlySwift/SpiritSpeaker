using System;
using System.Collections.Generic;
using System.Linq;

namespace SpiritSpeak.Combat
{
    public class Commander : ITeamMember
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
            var allies = battle.GetAllyTargets(TeamId);
            var enemies = battle.GetEnemyTargets(TeamId);

            var strongestSpirit = Spirits.OrderByDescending(x => x.Strength).FirstOrDefault();
            if (strongestSpirit != null && enemies.Count > 0)
            {
                var randomEnemy = enemies[_random.Next(enemies.Count)];
                return new BattleAction() { DebugMessage = "RAWR!", Damage = strongestSpirit.Strength, Source = strongestSpirit, Target = randomEnemy };
            }

            return new BattleAction() { DebugMessage = "RAWR!" };
            //Return an object describing what you want to do
        }
    }
}
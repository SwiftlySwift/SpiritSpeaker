using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SpiritSpeak.Combat
{
    public class Battle
    {
        public List<Speaker> Speakers => Commanders.SelectMany(x => x.Speakers).ToList();
        public List<Spirit> Spirits => Commanders.SelectMany(x => x.Spirits).ToList();
        public List<Commander> Commanders { get; set; }

        public int CurrentInitiative { get; set; }
        public int MaxInitiative { get; set; }

        public Battle()
        {
            Commanders = new List<Commander>();
        }

        public void StartCombat()
        {
            TieBreakInitiatives();
            CurrentInitiative = Commanders.Min(x => x.Initiative);
            MaxInitiative = Commanders.Max(x => x.Initiative);
        }

        public BattleActionResult TakeTurn()
        {
            Commander currentCommander = Commanders.FirstOrDefault(x => x.Initiative == CurrentInitiative);
            
            var action = currentCommander.GetAction(this);

            if (action != null)
            {
                AdvanceInitiative();
                return action.DoAction(this);
            }

            return null;
        }

        public List<ITarget> GetEnemyTargets(int teamId)
        {
            return
                Speakers.Where(x => x.TeamId != teamId || x.TeamId == -1).ToList<ITarget>().
                Union(Spirits.Where(x => x.TeamId != teamId || x.TeamId == -1)).ToList();
        }
        public List<ITarget> GetAllyTargets(int teamId)
        {
            return
                Speakers.Where(x => x.TeamId == teamId && x.TeamId != -1).ToList<ITarget>().
                Union(Spirits.Where(x => x.TeamId == teamId && x.TeamId != -1)).ToList();
        }

        private void AdvanceInitiative()
        {
            do
            {
                CurrentInitiative++;
                if (CurrentInitiative > MaxInitiative)
                {
                    CurrentInitiative = 0;
                }
            }
            while (Commanders.FirstOrDefault(x => x.Initiative == CurrentInitiative) == null);
        }

        private void TieBreakInitiatives()
        {
            //Sort out any duplicate initiatives
        }
    }
}

using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SpiritSpeak.Combat
{
    public class Battle
    {
        public const int GRID_MAX_X = 5;
        public const int GRID_MAX_Y = 5; 

        public List<Speaker> Speakers => Commanders.SelectMany(x => x.Speakers).ToList();
        public List<Spirit> Spirits => Commanders.SelectMany(x => x.Spirits).ToList();
        public List<Commander> Commanders { get; set; }
        public int CurrentInitiative { get; set; }
        public int MaxInitiative { get; set; }

        public GridTile[,] Grid { get; set; }


        public Battle()
        {
            Commanders = new List<Commander>();
            Grid = new GridTile[5, 5];
            for(var i = 0; i < Grid.GetLength(0); i++)
            {
                for (var j = 0; j < Grid.GetLength(1); j++)
                {
                    Grid.SetValue(new GridTile(), i, j);
                }
            }
        }

        internal bool MoveSpirit(Spirit source, Point move)
        {
            var currentLocation = Grid[source.GridLocation.X, source.GridLocation.Y];
            if (currentLocation.Spirit != source)
            {
                //wtf?
                throw new Exception("Battle grid out of sync, spirit not in correct location");
            }

            var newLocation = Grid[source.GridLocation.X + move.X, source.GridLocation.Y+move.Y];
            if (newLocation.Spirit != null)
            {
                //Spirit is in the way. This generally calls for cancelling the move, and reveals a hidden unit or trap.
                return false;
            }
            currentLocation.Spirit = null;
            source.GridLocation += move;
            Grid[source.GridLocation.X, source.GridLocation.Y].Spirit = source;
            return true;
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

        public List<Spirit> GetEnemyTargets(int teamId)
        {
            return (Spirits.Where(x => x.TeamId != teamId || x.TeamId == -1)).ToList();
        }
        public List<Spirit> GetAllyTargets(int teamId)
        {
            return (Spirits.Where(x => x.TeamId == teamId && x.TeamId != -1)).ToList();
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

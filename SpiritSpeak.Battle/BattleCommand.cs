using Microsoft.Xna.Framework;
using SpiritSpeak.Combat.Actions;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SpiritSpeak.Combat
{
    public class BattleCommand
    {
        public List<MovementAction> MovementActions { get; set; } 
        public List<DamageAction> DamageActions { get; set; } 
        public List<TerrainAction> TerrainActions { get; set; } 
        public List<AnimationAction> AnimationActions { get; set; } 

        public BattleCommand()
        {
            MovementActions = new List<MovementAction>();
            DamageActions = new List<DamageAction>();
            TerrainActions = new List<TerrainAction>();
            AnimationActions = new List<AnimationAction>();
        }

        public BattleActionResult DoAction(Battle battle)
        {
            var result = new BattleActionResult();

            //First process movements that aren't Shoves
            foreach(var a in MovementActions.Where(x => !x.Shove))
            {
                result.MovementResults.Add(a.GetResult(battle));
            }

            //Deal damage
            foreach (var a in DamageActions)
            {
                result.DamageResults.AddRange(a.GetResult(battle));
            }

            //Calculate if/how terrain changes
            foreach (var a in TerrainActions)
            {
                result.TerrainResults.Add(a.GetResult(battle));
            }

            //Calculate animations
            foreach (var a in AnimationActions)
            {
                result.AnimationResults.Add(a.GetResult(battle));
            }

            //Lastly process movements that are Shoves
            foreach (var m in MovementActions.Where(x => x.Shove))
            {
                result.MovementResults.Add(m.GetResult(battle));
            }

            return result;
        }
    }
}
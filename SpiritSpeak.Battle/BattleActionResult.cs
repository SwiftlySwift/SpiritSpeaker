using Microsoft.Xna.Framework;
using SpiritSpeak.Combat.Actions;
using System.Collections.Generic;

namespace SpiritSpeak.Combat
{
    public class BattleActionResult
    {
        public List<DamageResult> DamageResults { get; set; }
        public List<MovementResult> MovementResults { get; set; }
        public List<AnimationResult> AnimationResults { get; set; }
        public List<TerrainResult>  TerrainResults { get; set; }

        public BattleActionResult()
        {
            DamageResults = new List<DamageResult>();
            MovementResults = new List<MovementResult>();
            AnimationResults = new List<AnimationResult>();
            TerrainResults = new List<TerrainResult>();
        }
    }
}
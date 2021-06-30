using Microsoft.Xna.Framework;
using SpiritSpeak.Combat.BattleActions;
using System.Collections.Generic;

namespace SpiritSpeak.Combat
{
    public class BattleActionResult
    {
        public List<DamageActionResult> DamageActionResults { get; set; }

        public List<MoveActionResult> MoveActionResults { get; set; }

        public BattleActionResult()
        {
            DamageActionResults = new List<DamageActionResult>();
            MoveActionResults = new List<MoveActionResult>();
        }

    }
}
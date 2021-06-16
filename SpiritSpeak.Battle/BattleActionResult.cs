using System.Collections.Generic;

namespace SpiritSpeak.Combat
{
    public class BattleActionResult
    {
        public string DebugMessage { get; set; }

        public List<DamageResult> DamageResults { get; set; }

    }
}
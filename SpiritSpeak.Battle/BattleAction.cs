using System;
using System.Collections.Generic;

namespace SpiritSpeak.Combat
{
    public class BattleAction
    {
        public string DebugMessage { get; set; }

        public ITarget Target { get; set; }

        public ITarget Source { get; set; }

        public int Damage { get; set; }

        public BattleActionResult DoAction(Battle battle)
        {
            //Tell me what happened when you did what you did

            var damage = Target.TakeDamage(Damage);
            var damageResult = new DamageResult()
            {
                Amount = damage,
                Source = Source,
                Target = Target
            };

            return new BattleActionResult()
            {
                DebugMessage = DebugMessage,
                DamageResults = new List<DamageResult>()
                {
                    damageResult
                }
            };
        }
    }
}
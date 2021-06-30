using System;
using System.Collections.Generic;
using System.Text;

namespace SpiritSpeak.Combat.BattleActions
{
    public class DamageAction : BattleAction
    {
        public int Amount { get; set; }
        public Spirit Source { get; set; }
        public Spirit Target { get; set; }

        public DamageAction() : base()
        {

        }

        public override BattleActionResult DoAction(Battle battle)
        {
            var damageActionResult = new DamageActionResult();
            var battleActionResult = new BattleActionResult();

            if (Target != null && Source.InRangeOf(Target))
            {
                var damage = Target.TakeDamage(Amount);

                damageActionResult.Source = Source;
                damageActionResult.Target = Target;
                damageActionResult.Amount = damage;
            }
            battleActionResult.DamageActionResults.Add(damageActionResult);

            return battleActionResult;
        }


    }
}

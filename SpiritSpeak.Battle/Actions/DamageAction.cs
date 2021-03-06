using System;
using System.Collections.Generic;
using System.Text;

namespace SpiritSpeak.Combat.Actions
{
    public class DamageAction : BaseAction
    {
        public int Amount { get; set; }
        public DamageAction()
        {

        }
        public List<DamageResult> GetResult(Battle battle)
        {
            List<DamageResult> results = new List<DamageResult>();

            List<Spirit> validTargets = Targetting.GetValidTargets(Source);

            foreach(var target in validTargets)
            {
                results.Add(new DamageResult()
                {
                    Amount = Amount,
                    Target = target
                });
                target.Vitality -= Amount;
            }

            return results;
        }
    }
}

using System;
using System.Collections.Generic;

namespace SpiritSpeak.Combat
{
    public class Targetting
    {
        public List<Spirit> DirectTargets { get; set; }

        internal List<Spirit> GetValidTargets(Spirit source)
        {
            return DirectTargets;
        }
    }
}
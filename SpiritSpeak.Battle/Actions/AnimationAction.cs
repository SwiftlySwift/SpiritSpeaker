using System;
using System.Collections.Generic;

namespace SpiritSpeak.Combat
{
    public enum Animation { Shove, Bonk, DoubleBonk }

    public class AnimationAction : BaseAction
    {
        public Animation Animation { get; set; }

        public AnimationResult GetResult(Battle b)
        {
            return new AnimationResult()
            {
                Animation = Animation,
                Source = Source,
                Targetting = Targetting
            };
        }
    }
}
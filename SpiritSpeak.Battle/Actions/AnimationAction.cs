using System;
using System.Collections.Generic;

namespace SpiritSpeak.Combat
{
    public enum AnimationType { Shove, Throw, DoubleBonk }

    public class AnimationAction : BaseAction
    {
        public AnimationType Animation { get; set; }
        public int SpriteId { get; set; }
        public float DelayInSeconds { get; set; }

        public AnimationResult GetResult(Battle b)
        {
            return new AnimationResult()
            {
                Animation = Animation,
                Source = Source,
                Targetting = Targetting,
                DelayInSeconds = DelayInSeconds,
                SpriteId = SpriteId
            };
        }
    }
}
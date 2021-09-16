namespace SpiritSpeak.Combat
{
    public class AnimationResult
    {
        public AnimationType Animation { get; set; }
        public Spirit Source { get; set; }
        public Targetting Targetting { get; set; }
        public float DelayInSeconds { get; set; }

        public int SpriteId { get; set; }
    }
}
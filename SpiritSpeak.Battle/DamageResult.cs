namespace SpiritSpeak.Combat
{
    public class DamageResult
    {
        public ITarget Target { get; set; }
        public ITarget Source { get; set; }
        public int Amount { get; set; }
    }
}
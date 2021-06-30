namespace SpiritSpeak.Combat.BattleActions
{
    public class DamageActionResult
    {
        public Spirit Target { get; set; }
        public Spirit Source { get; set; }
        public int Amount { get; set; }

        public DamageActionResult()
        {

        }
    }
}
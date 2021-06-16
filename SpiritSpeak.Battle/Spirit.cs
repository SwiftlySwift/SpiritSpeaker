using System;
using System.Collections.Generic;
using System.Text;

namespace SpiritSpeak.Combat
{
    public class Spirit : ITarget
    {
        private int _vitality;
        public int Vitality { get => _vitality; set => _vitality = Math.Max(Math.Min(MaxVitality, value),0); }
        public int MaxVitality { get; set; }

        public int Strength { get; set; }

        public Guid Id { get; private set; }
        public int TeamId { get; private set; }
        
        private Battle Battle { get; set; }

        public Spirit(Battle battle, int teamId)
        {
            Id = Guid.NewGuid();
            Battle = battle;
            TeamId = teamId;
        }

        public int TakeDamage(int damage)
        {
            Vitality -= damage;
            return damage;
        }
    }
}

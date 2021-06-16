using System;
using System.Collections.Generic;
using System.Text;

namespace SpiritSpeak.Combat
{
    public class Speaker : ITarget
    {
        public int Vitality { get; set; }
        
        public List<Spirit> ArchetypeSpirits { get; set; }

        public Guid Id { get; private set; }

        public int TeamId { get; private set; }

        private Battle Battle { get; set; }

        public Speaker(Battle battle, int teamId)
        {
            Id = Guid.NewGuid();
            TeamId = teamId;
            Battle = battle;
        }

        public int TakeDamage(int damage)
        {
            Vitality -= damage;
            return damage;
        }
    }
}

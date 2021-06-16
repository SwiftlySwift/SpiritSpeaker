using System;

namespace SpiritSpeak.Combat
{
    public interface ITarget : ITeamMember
    {
        int TakeDamage(int damage);

        Guid Id { get; }


    }
}
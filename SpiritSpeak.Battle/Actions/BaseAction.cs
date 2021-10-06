using System;
using System.Collections.Generic;
using System.Text;

namespace SpiritSpeak.Combat
{
    public enum ActionPhase { Prefix, During, Postfix }

    public abstract class BaseAction
    {
        public Spirit Source { get; set; } //Who's doing it?
        public Targetting Targetting { get; set; } //Who's it being done to?
        public BaseAction()
        {

        }
    }
}

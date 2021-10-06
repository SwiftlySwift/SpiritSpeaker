using System;
using System.Collections.Generic;
using System.Text;

namespace SpiritSpeak.Combat.Actions
{
    public class TerrainAction : BaseAction
    {
        public Terrain NewTerrain { get; set; }
        public int Duration { get; set; }
        public bool Overwrite { get; set; }

        public TerrainResult GetResult(Battle b)
        {
            throw new NotImplementedException();
        }
    }
}

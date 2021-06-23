using System;
using System.Collections.Generic;
using System.Text;

namespace SpiritSpeak.Combat
{
    public class GridTile
    {
        public Spirit Spirit { get; set; }
        public Terrain Terrain { get; set; }

        public GridTile()
        {
            Terrain = null;
            Spirit = null;
        }

    }
}

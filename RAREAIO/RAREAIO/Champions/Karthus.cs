using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LeagueSharp;
using RAREAIO.Spells;

namespace RAREAIO.Champions
{
    class Karthus
    {
        public SpellQ Q = new SpellQ(875, new[] { 80, 120, 200, 240 }, 0.3f, DamageType.Magical);
        public Karthus()
        {
            
        }

    }
}

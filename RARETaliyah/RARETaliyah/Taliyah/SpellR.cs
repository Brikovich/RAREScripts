using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LeagueSharp;
using LeagueSharp.SDK;

namespace RARETaliyah.Taliyah
{
    internal class SpellR
    {

        public Spell SpellDetails { get; private set; }

        public SpellR(Spell r)
        {
            this.SpellDetails = r;
        }

        public double CustomEDamage(Obj_AI_Base t)
        {


            return double.MinValue;
        }

    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LeagueSharp;
using LeagueSharp.Common;
using LeagueSharp.Data.DataTypes;

namespace RAREGangplank.Gangplank
{
    class Citrus : Spell
    {

        // class variables
        private static float RangeW
            =>
                LeagueSharp.Data.Data.Get<LeagueSharp.Data.DataTypes.SpellDatabase>()
                    .Spells.Single(spell => spell.ChampionName == "Gangplank" && spell.Slot == SpellSlot.W)
                    .Range;

        // class variables
        private static SpellDatabaseEntry SpellEEntry
            =>
                LeagueSharp.Data.Data.Get<LeagueSharp.Data.DataTypes.SpellDatabase>()
                    .Spells.Single(spell => spell.ChampionName == "Gangplank" && spell.Slot == SpellSlot.E);



        public Citrus() : base(SpellSlot.W, RangeW, TargetSelector.DamageType.Magical)
        {
            
        }

        public bool PlayerNeedsCitrus()
        {
            //TODO: Float Menu !!!
            return ( Gangplank.Player.IsImmovable || Gangplank.Player.IsStunned ||
                     Gangplank.Player.IsRooted || Gangplank.Player.IsCharmed )
                     && Gangplank.Player.Health < Gangplank.Player.MaxHealth; 
        }

        public double CirtusHeal()
        {
            double missingHealth = Gangplank.Player.MaxHealth - Gangplank.Player.Health;
            int[] heal = {50, 75, 100, 125, 150};

            if (this.Level >= 1)
            {
                return heal[this.Level - 1] + missingHealth * 0.15;
            }

            return 0d;
        }

    }
}

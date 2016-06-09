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

        /// <summary>
        /// The SpellDatabaseEntry for our W Spell
        /// </summary>
        private static SpellDatabaseEntry SpellWEntry
            =>
                LeagueSharp.Data.Data.Get<LeagueSharp.Data.DataTypes.SpellDatabase>()
                    .Spells.Single(spell => spell.ChampionName == "Gangplank" && spell.Slot == SpellSlot.W);



        public Citrus() : base(SpellSlot.W, SpellWEntry.Range, TargetSelector.DamageType.Magical)
        {
            
        }

        public bool PlayerNeedsCitrus()
        {
            var healthPercent = GMenu.MainMenu.Item("lifeCitrus").GetValue<Slider>().Value;
            return ( Gangplank.Player.IsImmovable || Gangplank.Player.IsStunned ||
                     Gangplank.Player.IsRooted || Gangplank.Player.IsCharmed )
                     && Gangplank.Player.Health < (Gangplank.Player.MaxHealth * healthPercent); 
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

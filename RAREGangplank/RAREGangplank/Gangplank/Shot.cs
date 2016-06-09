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
    class Shot : Spell
    {
        /// <summary>
        /// The SpellDatabaseEntry for our W Spell
        /// </summary>
        private static SpellDatabaseEntry SpellQEntry
            =>
                LeagueSharp.Data.Data.Get<LeagueSharp.Data.DataTypes.SpellDatabase>()
                    .Spells.Single(spell => spell.ChampionName == "Gangplank" && spell.Slot == SpellSlot.Q);



        public Shot() : base(SpellSlot.W, SpellQEntry.Range, TargetSelector.DamageType.Magical)
        {
            this.SetTargetted(SpellQEntry.Delay, SpellQEntry.MissileSpeed);
        }
    }
}

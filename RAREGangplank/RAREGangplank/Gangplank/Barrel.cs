﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LeagueSharp;
using LeagueSharp.Common;
using LeagueSharp.Data.DataTypes;
using LeagueSharp.Data.Enumerations;

namespace RAREGangplank.Gangplank
{
    class Barrel : Spell
    {
        // class variables
        private static float RangeE
            =>
                LeagueSharp.Data.Data.Get<LeagueSharp.Data.DataTypes.SpellDatabase>()
                    .Spells.Single(spell => spell.ChampionName == "Gangplank" && spell.Slot == SpellSlot.E)
                    .Range;

        // class variables
        private static SpellDatabaseEntry SpellEEntry
            =>
                LeagueSharp.Data.Data.Get<LeagueSharp.Data.DataTypes.SpellDatabase>()
                    .Spells.Single(spell => spell.ChampionName == "Gangplank" && spell.Slot == SpellSlot.E);

        
    
        // constuctor
        public Barrel() : base(SpellSlot.E, RangeE, TargetSelector.DamageType.Physical)
        {
            this.SetSkillshot(SpellEEntry.Delay, SpellEEntry.Width, SpellEEntry.MissileSpeed, SpellEEntry.CollisionObjects.Any(), Utilities.ConvertToSkillshotType(SpellEEntry.SpellType));
        }

        public static void HandleBarrel(Orbwalking.OrbwalkingMode orbMode)
        {
            switch (orbMode)
            {
                case Orbwalking.OrbwalkingMode.Combo:
                    break;
                case Orbwalking.OrbwalkingMode.LastHit:
                    break;
                case Orbwalking.OrbwalkingMode.Freeze:
                    break;
                case Orbwalking.OrbwalkingMode.Mixed:
                    break;
                case Orbwalking.OrbwalkingMode.LaneClear:
                    break;
            }
        }
        

    }
}
﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LeagueSharp;
using LeagueSharp.Common;
using LeagueSharp.Data.DataTypes;
using RAREGangplank.Gangplank.BarrelClasses;

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



        public Shot() : base(SpellSlot.Q, SpellQEntry.Range, TargetSelector.DamageType.Physical)
        {
            this.SetTargetted(SpellQEntry.Delay, SpellQEntry.MissileSpeed);
        }

        public void HandleSpell(Orbwalking.OrbwalkingMode orbMode)
        {

            if (orbMode == Orbwalking.OrbwalkingMode.Combo && GMenu.MainMenu.Item("shotCM").GetValue<bool>())
            {
                if (this.IsReady() && Barrel.Barrels != null)
                {
                    var barrel = Barrel.Barrels.FirstOrDefault(x => x.Health <= 1);

                    if (barrel != null && barrel.CountEnemiesInRange(Barrel.BarrelExplosionRadius) >= 1
                            && Cooldown < Gangplank.BarrelSpell.GetMaxCooldown())
                    {
                        CastOnUnit(barrel);
                    }
                }
            }
            else if (orbMode == Orbwalking.OrbwalkingMode.LaneClear && GMenu.MainMenu.Item("shotLC").GetValue<bool>())
            {
                var minion = ObjectManager.Get<Obj_AI_Minion>()
                    .Where(x => x.IsValid && this.IsInRange(x) && x.IsTargetable && x.Health <= this.GetDamage(x))
                        .OrderByDescending(y => y.Health)
                            .FirstOrDefault();

                // check if any barrel is stored
                if (Barrel.Barrels != null && Barrel.Barrels.Any(x => x.Health <= 1))
                {
                    var barrel = Barrel.Barrels.FirstOrDefault(x => x.Health <= 1);
                    if (barrel != null)
                        CastOnUnit(barrel);
                }
                else if (minion != null)
                {
                    this.CastOnUnit(minion);
                }
               
            }
        }

        
            

    }
}

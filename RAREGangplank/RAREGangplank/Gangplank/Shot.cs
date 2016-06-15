#region copyrights

//  Copyright 2016 Marvin Piekarek
//  Shot.cs is part of RAREGangplank.
//  RAREGangplank is free software: you can redistribute it and/or modify
//  it under the terms of the GNU General Public License as published by
//  the Free Software Foundation, either version 3 of the License, or
//  (at your option) any later version.
//  RAREGangplank is distributed in the hope that it will be useful,
//  but WITHOUT ANY WARRANTY; without even the implied warranty of
//  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the
//  GNU General Public License for more details.
//  You should have received a copy of the GNU General Public License
//  along with RAREGangplank. If not, see <http://www.gnu.org/licenses/>.

#endregion

#region usages

using System.Linq;
using LeagueSharp;
using LeagueSharp.Common;
using LeagueSharp.Data;
using LeagueSharp.Data.DataTypes;
using RAREGangplank.Gangplank.BarrelClasses;

#endregion

namespace RAREGangplank.Gangplank
{

    internal class Shot : Spell
    {
        #region Fields and Constants

        /// <summary>
        ///   The SpellDatabaseEntry for our W Spell
        /// </summary>
        private static SpellDatabaseEntry SpellQEntry
            =>
                Data.Get<SpellDatabase>()
                    .Spells.Single(spell => spell.ChampionName == "Gangplank" && spell.Slot == SpellSlot.Q);

        #endregion

        #region Constructors

        public Shot() : base(SpellSlot.Q, SpellQEntry.Range, TargetSelector.DamageType.Physical)
        {
            SetTargetted(SpellQEntry.Delay, SpellQEntry.MissileSpeed);
        }

        #endregion

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
                    .Where(x => x.IsValid && IsInRange(x) && x.IsTargetable && x.Health <= GetDamage(x))
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
                    CastOnUnit(minion);
                }
            }
        }

    }

}
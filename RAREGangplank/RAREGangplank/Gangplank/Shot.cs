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

using System;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using LeagueSharp;
using LeagueSharp.Common;
using LeagueSharp.Data;
using LeagueSharp.Data.DataTypes;
using RAREGangplank.Gangplank.BarrelClasses;
using RAREGangplank.Interfaces;
using SharpDX;

#endregion

namespace RAREGangplank.Gangplank
{

    internal class Shot : Spell, ISpell
    {
        #region Fields and Constants

        /// <summary>
        ///     The Barrel spell data
        /// </summary>
        private readonly BarrelSpell _barrelS;

        /// <summary>
        ///   The SpellDatabaseEntry for our W Spell
        /// </summary>
        private static SpellDatabaseEntry SpellQEntry
            =>
                Data.Get<SpellDatabase>()
                    .Spells.Single(spell => spell.ChampionName == "Gangplank" && spell.Slot == SpellSlot.Q);

        #endregion

        #region Constructors

        /// <summary>
        ///     a shotty constructor
        /// </summary>
        public Shot() : base(SpellSlot.Q, SpellQEntry.Range, TargetSelector.DamageType.Physical)
        {
            SetTargetted(SpellQEntry.Delay/1000f, SpellQEntry.MissileSpeed);
            _barrelS = new BarrelSpell();

            Obj_AI_Base.OnProcessSpellCast += Obj_AI_Base_OnProcessSpellCast;
        }

        #endregion

        private void Obj_AI_Base_OnProcessSpellCast(Obj_AI_Base sender, GameObjectProcessSpellCastEventArgs args)
        {
            if (sender.IsMe && args.Slot == SpellSlot.E)
            {
                Console.WriteLine("SpellE is called");
                if (_barrelS.GetStacks == 1)
                {
                    var barrel = _barrelS.activeBarrels.Where(x => x.Data.Health <= 1)
                        .OrderByDescending(y => y.Data.Distance(Gangplank.Player)).ToList();
                    var bases = ObjectManager.Get<Obj_AI_Base>().Where(x => x.IsValid && x.IsValidTarget(2000f) && !x.isBarrel()).ToList();

                    Console.WriteLine("BarrelFound!");
                    Console.WriteLine("Cast on barrel");
                    Cast(barrel.First().Data);
                    
                }
            }
        }

        #region Interface

        /// <summary>
        ///     Gets the current spell damage
        /// </summary>
        /// <param name="target">target</param>
        /// <returns></returns>
        public double GetSpellDamage(Obj_AI_Base target)
        {
            return GetDamage(target);
        }

        /// <summary>
        ///     casts your spell with a costum pos
        /// </summary>
        /// <param name="pos">position as Vector2</param>
        /// <returns>a boolean that says if it has casted the skill or not</returns>
        public bool CastSpell(Vector2 pos)
        {
            return Cast(pos);
        }

        /// <summary>
        ///     casts your spell with a costum target
        /// </summary>
        /// <param name="target">target as Obj_AI_Base</param>
        /// <returns>a boolean that says if it has casted the skill or not</returns>
        public bool CastSpell(Obj_AI_Base target)
        {
            return Cast(target) == CastStates.SuccessfullyCasted;
        }

        #endregion

        /// <summary>
        ///     
        /// </summary>
        /// <param name="orbMode"></param>
        public void HandleSpell(Orbwalking.OrbwalkingMode orbMode)
        {
            /*if (orbMode == Orbwalking.OrbwalkingMode.Combo && GMenu.MainMenu.Item("shotCM").GetValue<bool>())
            {
                if (this.IsReady() && _barrelS.activeBarrels != null)
                {
                    var barrel = _barrelS.activeBarrels.FirstOrDefault(x => x.Data.Health <= 1);

                    if (barrel != null && barrel.Data.CountEnemiesInRange(BarrelSpell.ExplosionRadius) >= 1
                        && Cooldown < _barrelS.GetMaxCooldown())
                    {
                        CastOnUnit(barrel.Data);
                    }
                }
            }
            else */

            if (orbMode == Orbwalking.OrbwalkingMode.LaneClear && GMenu.MainMenu.Item("shotLC").GetValue<bool>())
            {
                var minion = ObjectManager.Get<Obj_AI_Minion>()
                    .Where(x => x.IsValid && IsInRange(x) && x.IsTargetable && x.Health <= GetDamage(x) && !x.isBarrel())
                    .OrderByDescending(y => y.Health)
                    .FirstOrDefault();

                /*// check if any barrel is stored
                if (_barrelS.activeBarrels != null && _barrelS.activeBarrels.Any(x => x.Data.Health <= 1))
                {
                    var barrel = _barrelS.activeBarrels.FirstOrDefault(x => x.Data.Health <= 1);
                    if (barrel != null)
                        CastOnUnit(barrel.Data);
                }
                else*/
                if (minion != null)
                {
                    CastOnUnit(minion);
                }
            }
        }

    }

}
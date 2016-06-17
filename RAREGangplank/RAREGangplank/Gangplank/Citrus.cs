#region copyrights

//  Copyright 2016 Marvin Piekarek
//  Citrus.cs is part of RAREGangplank.
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
using RAREGangplank.Interfaces;
using SharpDX;

#endregion

namespace RAREGangplank.Gangplank
{

    internal class Citrus : Spell, ISpell
    {
        #region Fields and Constants

        /// <summary>
        ///   The SpellDatabaseEntry for our W Spell
        /// </summary>
        private static SpellDatabaseEntry SpellWEntry
            =>
                Data.Get<SpellDatabase>()
                    .Spells.Single(spell => spell.ChampionName == "Gangplank" && spell.Slot == SpellSlot.W);

        private readonly int[] heal = { 50, 75, 100, 125, 150 };

        #endregion

        #region Constructors

        /// <summary>
        ///     Constructor for our Citrus spell
        /// </summary>
        public Citrus() : base(SpellSlot.W, SpellWEntry.Range, TargetSelector.DamageType.Magical)
        {
            // not needed at all, but for initializing the inherited spell.
        }

        #endregion

        /// <summary>
        ///     Checks if the hero needs the citrus
        /// </summary>
        /// <returns>boolean that says if it's needed</returns>
        public bool PlayerNeedsCitrus()
        {
            var healthPercent = GMenu.MainMenu.Item("lifeCitrus").GetValue<Slider>().Value;
            return (Gangplank.Player.IsImmovable || Gangplank.Player.IsStunned ||
                    Gangplank.Player.IsRooted || Gangplank.Player.IsCharmed)
                   && Gangplank.Player.Health < Gangplank.Player.MaxHealth*healthPercent;
        }

        /// <summary>
        ///     get you the current healratio of your W Spell
        /// </summary>
        /// <returns>returns a float with the heal value </returns>
        public double GetSpellDamage(Obj_AI_Base target)
        {
            double missingHealth = Gangplank.Player.MaxHealth - Gangplank.Player.Health;
            
            if (Level >= 1)
            {
                return heal[Level - 1] + missingHealth * 0.15;
            }

            return 0d;
        }


        public bool CastSpell(Vector2 pos)
        {
            return Cast();
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

    }

}
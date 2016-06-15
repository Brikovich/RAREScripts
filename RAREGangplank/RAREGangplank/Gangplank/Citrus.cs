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

#endregion

namespace RAREGangplank.Gangplank
{

    internal class Citrus : Spell
    {
        #region Fields and Constants

        /// <summary>
        ///   The SpellDatabaseEntry for our W Spell
        /// </summary>
        private static SpellDatabaseEntry SpellWEntry
            =>
                Data.Get<SpellDatabase>()
                    .Spells.Single(spell => spell.ChampionName == "Gangplank" && spell.Slot == SpellSlot.W);

        #endregion

        #region Constructors

        public Citrus() : base(SpellSlot.W, SpellWEntry.Range, TargetSelector.DamageType.Magical)
        {
        }

        #endregion

        public bool PlayerNeedsCitrus()
        {
            var healthPercent = GMenu.MainMenu.Item("lifeCitrus").GetValue<Slider>().Value;
            return (Gangplank.Player.IsImmovable || Gangplank.Player.IsStunned ||
                    Gangplank.Player.IsRooted || Gangplank.Player.IsCharmed)
                   && Gangplank.Player.Health < Gangplank.Player.MaxHealth*healthPercent;
        }

        public double CirtusHeal()
        {
            double missingHealth = Gangplank.Player.MaxHealth - Gangplank.Player.Health;
            int[] heal = {50, 75, 100, 125, 150};

            if (Level >= 1)
            {
                return heal[Level - 1] + missingHealth*0.15;
            }

            return 0d;
        }

    }

}
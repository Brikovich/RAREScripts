#region copyrights

//  Copyright 2016 Marvin Piekarek
//  SpellQ.cs is part of RARETaliyah.
//  RARETaliyah is free software: you can redistribute it and/or modify
//  it under the terms of the GNU General Public License as published by
//  the Free Software Foundation, either version 3 of the License, or
//  (at your option) any later version.
//  RARETaliyah is distributed in the hope that it will be useful,
//  but WITHOUT ANY WARRANTY; without even the implied warranty of
//  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the
//  GNU General Public License for more details.
//  You should have received a copy of the GNU General Public License
//  along with SFXUtility. If not, see <http://www.gnu.org/licenses/>.

#endregion

#region usages

using LeagueSharp;
using LeagueSharp.SDK;

#endregion

namespace RARETaliyah.Taliyah
{
    internal class SpellQ
    {
        public SpellQ(Spell q)
        {
            SpellDetails = q;
        }

        public Spell SpellDetails { get; }

        public double FiveQDamage(Obj_AI_Base t, float minManaPercent = 0)
        {
            var eDamage = new[] {180, 240, 300, 360, 420};

            if (SpellDetails.Level >= 1 && minManaPercent <= GameObjects.Player.ManaPercent)
            {
                return eDamage[SpellDetails.Level] + GameObjects.Player.FlatMagicDamageMod*1.2;
            }

            return double.MinValue;
        }

        public double SingleEDamage(Obj_AI_Base t, float minManaPercent = 0)
        {
            var eDamage = new[] {60, 80, 100, 120, 140};

            if (SpellDetails.Level >= 1 && minManaPercent <= GameObjects.Player.ManaPercent)
            {
                return eDamage[SpellDetails.Level] + GameObjects.Player.FlatMagicDamageMod*0.4;
            }

            return double.MinValue;
        }
    }
}
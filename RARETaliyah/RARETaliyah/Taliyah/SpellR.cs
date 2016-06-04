#region copyrights

//  Copyright 2016 Marvin Piekarek
//  SpellR.cs is part of RARETaliyah.
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
    internal class SpellR
    {
        public SpellR(Spell r)
        {
            SpellDetails = r;
        }

        public Spell SpellDetails { get; private set; }

        public double CustomEDamage(Obj_AI_Base t)
        {
            return double.MinValue;
        }
    }
}
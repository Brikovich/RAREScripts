#region copyrights

//  Copyright 2016 Marvin Piekarek
//  Utilities.cs is part of RAREGangplank.
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

using System.Collections.Generic;
using LeagueSharp;
using LeagueSharp.Common;
using LeagueSharp.Data.Enumerations;

#endregion

namespace RAREGangplank
{

    internal static class Utilities
    {

        /// <summary>
        ///   Converts SpellTypes out of the Database into SkillshotTypes
        /// </summary>
        /// <param name="spellType">The SpellType of the DatabaseEntry</param>
        /// <returns>SkillshotType for you Spell</returns>
        public static SkillshotType ConvertToSkillshotType(SpellType spellType)
        {
            return new List<SpellType> {SpellType.SkillshotCircle, SpellType.SkillshotMissileCircle}.Contains(spellType)
                ? SkillshotType.SkillshotCircle
                : SkillshotType.SkillshotLine;
        }

        /// <summary>
        /// gets a boolean out of the menu
        /// </summary>
        /// <param name="name">name of the item you want to receive</param>
        /// <returns>bool that returns the items current value</returns>
        public static bool GetMenuBool(string name) => GMenu.MainMenu.Item(name).GetValue<bool>();

        /// <summary>
        /// gets a int out of the menu
        /// </summary>
        /// <param name="name">name of the item you want to receive</param>
        /// <returns>int that returns the items current value</returns>
        public static int GetMenuValue(string name) => GMenu.MainMenu.Item(name).GetValue<Slider>().Value;

        internal static bool isBarrel(this Obj_AI_Base baseObj)
        {
            if (baseObj.Name.Contains("barrel"))
            {
                return true;
            }
            return false;
        }
    }

}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LeagueSharp.Common;
using LeagueSharp.Data.Enumerations;

namespace RAREGangplank
{
    internal class Utilities
    {

        /// <summary>
        /// Converts SpellTypes out of the Database into SkillshotTypes
        /// </summary>
        /// <param name="spellType">The SpellType of the DatabaseEntry</param>
        /// <returns>SkillshotType for you Spell</returns>
        public static SkillshotType ConvertToSkillshotType(SpellType spellType)
        {
            return new List<SpellType> { SpellType.SkillshotCircle, SpellType.SkillshotMissileCircle }.Contains(spellType)
                ? SkillshotType.SkillshotCircle
                : SkillshotType.SkillshotLine;
        }

    }
}

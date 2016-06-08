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

        // class methods
        public static SkillshotType ConvertToSkillshotType(SpellType spellType)
        {
            return new List<SpellType> { SpellType.SkillshotCircle, SpellType.SkillshotMissileCircle }.Contains(spellType)
                ? SkillshotType.SkillshotCircle
                : SkillshotType.SkillshotLine;
        }

    }
}

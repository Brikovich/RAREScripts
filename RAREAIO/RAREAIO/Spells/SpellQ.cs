using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LeagueSharp;
using LeagueSharp.SDK;

namespace RAREAIO.Spells
{
    class SpellQ : Spell
    {
        private int[] damagePerLevel;
        public float damageRatio;

        public SpellQ(float range, int[] damage, float ratio, DamageType damageType) : base(SpellSlot.Q)
        {
            Range = range;
            DamageType = damageType;
            damagePerLevel = damage;
            damageRatio = ratio;

        }

        /// <summary>
        /// Calculates the damage of the Q Spell
        /// </summary>
        /// <returns>the damage as double</returns>
        public double GetDamageQ()
        {
            if (Level >= 1 && DamageType == DamageType.Magical)
            {
                return damagePerLevel[Level - 1] + GameObjects.Player.FlatMagicDamageMod * damageRatio;
            }

            if (Level >= 1 && DamageType == DamageType.Physical)
            {
                return damagePerLevel[Level - 1] + GameObjects.Player.FlatPhysicalDamageMod*damageRatio;
            }

            if (Level >= 1 && (DamageType == DamageType.Mixed || DamageType == DamageType.True)) //Mixed or Truedamage
            {
                return damagePerLevel[Level - 1];
            }

            return double.MinValue;
        }
        
    }
}

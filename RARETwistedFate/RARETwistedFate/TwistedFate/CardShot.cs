#region copyrights

//  Copyright 2016 Marvin Piekarek
//  CardShot.cs is part of RARETwistedFate.
//  RARETwistedFate is free software: you can redistribute it and/or modify
//  it under the terms of the GNU General Public License as published by
//  the Free Software Foundation, either version 3 of the License, or
//  (at your option) any later version.
//  RARETwistedFate is distributed in the hope that it will be useful,
//  but WITHOUT ANY WARRANTY; without even the implied warranty of
//  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the
//  GNU General Public License for more details.
//  You should have received a copy of the GNU General Public License
//  along with RARETwistedFate. If not, see <http://www.gnu.org/licenses/>.

#endregion

#region usages

using System.Linq;
using LeagueSharp;
using LeagueSharp.SDK;
using LeagueSharp.SDK.Enumerations;

#endregion

namespace RARETwistedFate.TwistedFate
{
    internal class CardShot
    {
        public Spell SpellQ;

        public CardShot()
        {
            SpellQ = new Spell(SpellSlot.Q, 1450);
            SpellQ.SetSkillshot(250, 50, 1000, true, SkillshotType.SkillshotLine);
            SpellQ.MinHitChance = HitChance.Medium;
        }

        public void HandleQ(OrbwalkingMode orbMode)
        {
            if (orbMode == OrbwalkingMode.Combo && IsOn(orbMode))
            {
                var heroes = Variables.TargetSelector.GetTargets(SpellQ.Range, DamageType.Magical, false);
                SpellQ.CastIfHitchanceMinimum(heroes.FirstOrDefault(), SpellQ.MinHitChance);
            }
            else if ((orbMode == OrbwalkingMode.Hybrid || orbMode == OrbwalkingMode.LaneClear) && IsOn(orbMode))
            {
                var minions = GameObjects.EnemyMinions.Where(m => SpellQ.IsInRange(m)).ToList();
                var farmloc = SpellQ.GetLineFarmLocation(minions);

                if (farmloc.MinionsHit >= 3)
                {
                    SpellQ.Cast(farmloc.Position);
                }
            }
            else if (orbMode == OrbwalkingMode.LastHit && IsOn(orbMode))
            {
                var minions = GameObjects.EnemyMinions.Where(m => SpellQ.IsInRange(m)).ToList();
                var farmloc = SpellQ.GetLineFarmLocation(minions);

                if (farmloc.MinionsHit >= 3)
                {
                    SpellQ.Cast(farmloc.Position);
                }
            }
        }

        public bool IsOn(OrbwalkingMode orbMode)
        {
            switch (orbMode)
            {
                case OrbwalkingMode.Combo:
                    return MenuTwisted.MainMenu["Q"]["ComboQ"];
                case OrbwalkingMode.LastHit:
                    return MenuTwisted.MainMenu["Q"]["FarmQ"];
                case OrbwalkingMode.Hybrid:
                    return MenuTwisted.MainMenu["Q"]["HybridQ"];
                case OrbwalkingMode.LaneClear:
                    return MenuTwisted.MainMenu["Q"]["HybridQ"];
            }

            return false;
        }
    }
}
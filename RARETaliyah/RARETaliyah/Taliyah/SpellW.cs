#region copyrights

//  Copyright 2016 Marvin Piekarek
//  SpellW.cs is part of RARETaliyah.
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

#region test

using LeagueSharp;
using LeagueSharp.SDK;
using SharpDX;

#endregion

namespace RARETaliyah.Taliyah
{
    internal class SpellW
    {
        public SpellW(Spell w)
        {
            SpellDetails = w;
        }

        public Spell SpellDetails { get; }

        public void CastAway(Obj_AI_Base target)
        {
            var pred = SpellDetails.GetPrediction(target, true);
            if (pred != null && target.IsValid && SpellDetails.IsReady())
            {
                SpellDetails.Cast(pred.CastPosition);
                SpellDetails.Cast(new Vector3(pred.CastPosition.X - 10, pred.CastPosition.Y - 10, pred.CastPosition.Z));
            }
        }

        public void CastToMe(Obj_AI_Base target)
        {
            var pred = SpellDetails.GetPrediction(target, true);
            if (pred != null && target.IsValid && SpellDetails.IsReady())
            {
                SpellDetails.Cast(pred.CastPosition);
                SpellDetails.Cast(new Vector3(pred.CastPosition.X + 10, pred.CastPosition.Y + 10, pred.CastPosition.Z));
            }
        }

        public double CustomWDamage(Obj_AI_Base t, float minManaPercent = 0)
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
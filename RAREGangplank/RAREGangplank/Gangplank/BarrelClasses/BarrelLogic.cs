#region copyrights

//  Copyright 2016 Marvin Piekarek
//  BarrelLogic.cs is part of RAREGangplank.
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

using System;
using System.Collections.Generic;
using System.Linq;
using LeagueSharp;
using LeagueSharp.Common;
using SharpDX;
using Color = System.Drawing.Color;

#endregion

namespace RAREGangplank.Gangplank.BarrelClasses
{

    internal class BarrelLogic
    {
        #region Fields and Constants

        internal static float Rotation = 28*(float) Math.PI/180;

        /// <summary>
        ///   all placed barrels
        /// </summary>
        internal BarrelSpell barrel;

        private int GetStacks => barrel.Instance.Ammo;

        #endregion

        #region Constructors

        public BarrelLogic()
        {
            barrel = new BarrelSpell();
        }

        #endregion

        #region lambda

        private Barrel GetBarrelsInRange(Vector2 pos) =>
            BarrelSpell.ActiveBarrels.FirstOrDefault(x => x.data.Distance(pos) <= barrel.ExplosionRadius);

        private int CountBarrelsInRange(Vector2 pos) =>
            BarrelSpell.ActiveBarrels.Count(x => x.data.Distance(pos) <= barrel.ExplosionRadius);

        private static Vector2 PredPosRotate(float angle, Vector2 pos, Vector2 rotatePos) =>
            pos.RotateAroundPoint(rotatePos, angle);

        private List<GrassObject> BushInRange() =>
            ObjectManager.Get<GrassObject>()
                .Where(g => g.Position.Distance(Gangplank.Player.Position) < barrel.Range)
                .ToList();

        #endregion

        #region methods

        public void HandleBarrel(Orbwalking.OrbwalkingMode orbMode)
        {
            if (orbMode == Orbwalking.OrbwalkingMode.LaneClear && GMenu.MainMenu.Item("barrelLC").GetValue<bool>())
            {
                // get the max count for placing the barrels
                var count = GMenu.MainMenu.Item("countLC").GetValue<Slider>().Value;
                // place the barrels with the function
                PlaceBarrels(orbMode, count);
            }

            if (orbMode == Orbwalking.OrbwalkingMode.Combo && GMenu.MainMenu.Item("barrelCM").GetValue<bool>())
            {
                // place the barrels with the function
                PlaceBarrels(orbMode, int.MaxValue);
            }
        }

        private void PlaceBarrels(Orbwalking.OrbwalkingMode orbMode, int maxBarrels)
        {
            if (orbMode == Orbwalking.OrbwalkingMode.Combo)
            {
                /*
                 * Just for testing proposes
                 * VectorIdea.Clear();

                var target = TargetSelector.GetTarget(barrel.Range, TargetSelector.DamageType.Physical, false);
                if (target == null) return;
                var pred = barrel.GetPrediction(target, true);

                VectorIdea.Add(
                    Gangplank.Player.Position.Extend(target.Position,
                        target.Distance(Gangplank.Player.Position) - target.AttackRange)
                        .To2D()
                        .RotateAroundPoint(Gangplank.Player.Position.To2D(), Rotation)
                        .To3D());
                VectorIdea.Add(Gangplank.Player.Position.Extend(pred.CastPosition,
                    pred.CastPosition.Distance(Gangplank.Player.Position) + barrel.ExplosionRadius/4f));
                VectorIdea.Add(pred.CastPosition);
                VectorIdea.Add(Gangplank.Player.Position.Extend(pred.CastPosition,
                    pred.CastPosition.Distance(Gangplank.Player.Position) - barrel.ExplosionRadius /2f));*/
            }
            else if (orbMode == Orbwalking.OrbwalkingMode.LaneClear)
            {
                // get all minions that are enemies and being in range of the E + 100 units
                // order them by their current health
                var minions = ObjectManager.Get<Obj_AI_Base>()
                    .Where(m => m.IsMinion && m.IsEnemy && m.Distance(Gangplank.Player.Position) <= barrel.Range + 100)
                    .OrderByDescending(m => m.Health).ToList();

                // spotting the best farm loc
                var bestFarm = GetBestLoc(minions);

                if (bestFarm.MinionsHit >= GMenu.MainMenu.Item("hitLC").GetValue<Slider>().Value)
                {
                    var countBarrelsOnFarm = CountBarrelsInRange(bestFarm.Position);

                    // this cast will place a barrel and it will be automatically saved into our 
                    // barrel list, so we will need to add a check if there is already a barrel ;)
                    if (countBarrelsOnFarm == 0)
                    {
                        barrel.Cast(bestFarm.Position);
                    }
                    // otherwise we will go on looking for another good position
                    // since the since the user decided to get more than one barrel
                    else if (maxBarrels > 1 && countBarrelsOnFarm > 0)
                    {
                        // get a new position (if possible) from the already placed barrel
                        var barrelsInRange = GetBarrelsInRange(bestFarm.Position);

                        if (barrelsInRange == null)
                            return;

                        // get the minions near to our barrel (230 < x < 660)
                        // so between the explosion range and the max connection range
                        var secminions = ObjectManager.Get<Obj_AI_Base>()
                            .Where(
                                m =>
                                    m.IsMinion && m.IsEnemy && barrelsInRange.data.Distance(m) > barrel.ExplosionRadius &&
                                    barrelsInRange.data.Distance(m) < barrel.ConnectionRadius).ToList();

                        // spotting the best farm loc
                        var secFarm = GetBestLoc(secminions);

                        // checking if the farmLoc is valid for us
                        if (secFarm.Position != Vector2.Zero &&
                            Gangplank.Player.Distance(secFarm.Position) <= barrel.Range)
                        {
                            barrel.Cast(secFarm.Position);
                        }
                    }
                }
            }
        }

        private void BarrelsDrawing(EventArgs args)
        {
            foreach (var barrelSlot in BarrelSpell.ActiveBarrels)
            {
                if (barrelSlot.data.Health >= 2)
                    Render.Circle.DrawCircle(barrelSlot.data.Position, 200f, Color.Brown);

                if (barrelSlot.data.Health <= 1)
                    Render.Circle.DrawCircle(barrelSlot.data.Position, 200f, Color.Green);
            }

            foreach (var bush in BushInRange())
            {
                Render.Circle.DrawCircle(bush.Position, 200f, Color.AntiqueWhite);
            }
        }

        internal float GetMaxCooldown()
        {
            if (barrel.Level > 0)
                return float.MaxValue;

            var cooldowns = new[] {18f, 17f, 16f, 15f, 14f};
            var barrelsCount = 2f + Gangplank.RSpell.Level;

            return cooldowns[barrel.Level - 1]/barrelsCount;
        }

        private MinionManager.FarmLocation GetBestLoc(List<Obj_AI_Base> baseList)
        {
            if (baseList == null)
                return new MinionManager.FarmLocation(Vector2.Zero, -1);

            var positions = MinionManager.GetMinionsPredictedPositions(baseList, barrel.Delay, barrel.Width,
                barrel.Speed,
                Gangplank.Player.Position, barrel.Range, false, SkillshotType.SkillshotCircle);

            // uses the predicted position to get a good circular farm spot 
            return MinionManager.GetBestCircularFarmLocation(positions, barrel.Width, barrel.Range);
        }

        private void CastBarrel(Vector2 pos)
        {
            if (pos.IsValid())
                barrel.Cast(pos);
        }

        #endregion
    }

}
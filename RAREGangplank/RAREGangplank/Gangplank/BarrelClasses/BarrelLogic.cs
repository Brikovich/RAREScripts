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

#endregion

namespace RAREGangplank.Gangplank.BarrelClasses
{

    internal class BarrelLogic
    {
        #region Fields and Constants

        /// <summary>
        ///   all placed barrels
        /// </summary>
        internal BarrelSpell barrel;

        private float GetMaxCooldown()
        {
            if (barrel.Level > 0)
                return float.MaxValue;

            var cooldowns = new[] { 18f, 17f, 16f, 15f, 14f };
            var barrelsCount = 2f + Gangplank.RSpell.Level;

            return cooldowns[barrel.Level - 1] / barrelsCount;
        }

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
            BarrelSpell.ActiveBarrels.FirstOrDefault(x => x.Data.Distance(pos) <= barrel.ExplosionRadius);

        private int CountBarrelsInRange(Vector2 pos) =>
            BarrelSpell.ActiveBarrels.Count(x => x.Data.Distance(pos) <= barrel.ExplosionRadius);

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

            if (Utilities.GetMenuBool("autoBarrel"))
            {
                AutomaticBarrel();
            }

            if (orbMode == Orbwalking.OrbwalkingMode.LaneClear && GMenu.MainMenu.Item("barrelLC").GetValue<bool>())
            {
                // place the barrels with the function
                PlaceFarmBarrel(orbMode);
            }

            if (orbMode == Orbwalking.OrbwalkingMode.Combo && GMenu.MainMenu.Item("barrelCM").GetValue<bool>())
            {
                // place the barrels with the function
                
            }
        }

        private void AutomaticBarrel()
        {
            var bushes = GetBushes(Gangplank.Player.Position);

            var minions =
                ObjectManager.Get<Obj_AI_Base>()
                    .Where(x => x.IsValidTarget() && x.IsMinion && x.Distance(Gangplank.Player.Position) < barrel.Range).ToList();
            var farmPos = barrel.GetBestLoc(minions);

            if (farmPos.MinionsHit == 0 && bushes == null)
            {

            }
            else if (farmPos.MinionsHit >= Utilities.GetMenuValue("hitLC"))
            {
                
            }
            else if (bushes != null)
            {
                
            }
        }

        private List<GrassObject> GetBushes(Vector3 pos) =>
            ObjectManager.Get<GrassObject>().Where(b => b.Position.Distance(pos) < barrel.Range).ToList();

        private void PlaceFarmBarrel(Orbwalking.OrbwalkingMode orbMode)
        {

            //TODO: needs to be change to the optional vaule of available barrels

            if (orbMode == Orbwalking.OrbwalkingMode.LaneClear)
            {
                var maxBarrels = GMenu.MainMenu.Item("countLC").GetValue<Slider>().Value;

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
                                    m.IsMinion && m.IsEnemy && barrelsInRange.Data.Distance(m) > barrel.ExplosionRadius &&
                                    barrelsInRange.Data.Distance(m) < barrel.ConnectionRadius).ToList();

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

        #endregion
    }

}
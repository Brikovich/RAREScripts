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

        internal static float AngleE = 28*(float) Math.PI/180;

        #endregion

        #region Constructors

        /// <summary>
        ///   Constructor for our Barrellogic
        /// </summary>
        public BarrelLogic()
        {
            barrel = new BarrelSpell();
        }

        #endregion

        
        #region lambda

        /// <summary>
        ///   Gets all barrels in range
        /// </summary>
        /// <param name="pos">from where</param>
        /// <param name="range"></param>
        /// <returns>returns a Barrel in range</returns>
        private static Barrel GetBarrelInRange(Vector2 pos, float range) =>
            BarrelSpell.ActiveBarrels.FirstOrDefault(x => x.Data.Distance(pos) <= range);

        /// <summary>
        ///   Counts the barrels in range
        /// </summary>
        /// <param name="pos">from where</param>
        /// <param name="range"></param>
        /// <returns>count as int</returns>
        private int CountBarrelsInRange(Vector2 pos, float range) =>
            BarrelSpell.ActiveBarrels.Count(x => x.Data.Distance(pos) <= range);

        /// <summary>
        ///   Returns all bushes in BarrelRange
        /// </summary>
        /// <param name="pos">from where</param>
        /// <returns>a list of GrassObjects</returns>
        private List<GrassObject> GetBushes(Vector3 pos) =>
            ObjectManager.Get<GrassObject>().Where(b => b.Position.Distance(pos) < barrel.Range).ToList();

        #endregion

        #region methods

        /// <summary>
        ///   automatically barrel-placing-method
        /// </summary>
        private void AutomaticBarrel()
        {
            var bushes = GetBushes(Gangplank.Player.Position);
            var minions = ObjectManager.Get<Obj_AI_Base>()
                    .Where(x => x.IsValidTarget() && x.IsMinion && x.Distance(Gangplank.Player.Position) < barrel.Range)
                    .ToList();
            var farmPos = barrel.GetBestLoc(minions);
            var targets = ObjectManager.Get<Obj_AI_Hero>().Where(x => x.IsValid && x.IsValidTarget(barrel.Range)).OrderByDescending(x => x.Distance(Gangplank.Player)).ToList();
            var myPos = Gangplank.Player.Position.To2D();

            if (barrel.GetStacks - 1 > 0 && CountBarrelsInRange(myPos, barrel.Range) == 0)
            {
                if (farmPos.MinionsHit == 0 && bushes == null)
                {
                    barrel.CastSpell(myPos.Extend(targets.Last().Position.To2D(), BarrelSpell.ExplosionRadius).RotateAroundPoint(myPos, AngleE));
                }
                else if (farmPos.MinionsHit >= Utilities.GetMenuValue("hitLC"))
                {
                    barrel.CastSpell(farmPos.Position);
                }
                else if (bushes != null)
                {
                    barrel.CastSpell(bushes.Single(x => x.Position.Distance(Gangplank.Player.Position) > BarrelSpell.ExplosionRadius && 
                        x.Position.Distance(Gangplank.Player.Position) < BarrelSpell.ExplosionRadius*2).Position.To2D());
                }
            } 
            
        }

        /// <summary>
        ///   Logic for placing the barrel while farming
        /// </summary>
        /// <param name="orbMode"></param>
        private void PlaceFarmBarrel()
        {
            var maxBarrels = GMenu.MainMenu.Item("countLC").GetValue<Slider>().Value;

            // get all minions that are enemies and being in range of the E + 100 units
            // order them by their current health
            var minions = ObjectManager.Get<Obj_AI_Base>()
                .Where(m => m.IsMinion && m.IsEnemy && m.Distance(Gangplank.Player.Position) <= barrel.Range + 100)
                .OrderByDescending(m => m.Health).ToList();

            // spotting the best farm loc
            var bestFarm = barrel.GetBestLoc(minions);

            if (bestFarm.MinionsHit >= GMenu.MainMenu.Item("hitLC").GetValue<Slider>().Value)
            {
                var countBarrelsOnFarm = CountBarrelsInRange(bestFarm.Position, BarrelSpell.ExplosionRadius);

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
                    var barrelInRange = GetBarrelInRange(bestFarm.Position, BarrelSpell.ExplosionRadius);

                    if (barrelInRange == null)
                        return;

                    // get the minions near to our barrel (230 < x < 660)
                    // so between the explosion range and the max connection range
                    var secminions = ObjectManager.Get<Obj_AI_Base>()
                        .Where(
                            m =>
                                m.IsMinion && m.IsEnemy && barrelInRange.Data.Distance(m) > BarrelSpell.ExplosionRadius &&
                                barrelInRange.Data.Distance(m) < BarrelSpell.ConnectionRadius).ToList();

                    // spotting the best farm loc
                    var secFarm = barrel.GetBestLoc(secminions);

                    // checking if the farmLoc is valid for us
                    if (secFarm.Position != Vector2.Zero &&
                        Gangplank.Player.Distance(secFarm.Position) <= barrel.Range)
                    {
                        barrel.Cast(secFarm.Position);
                    }
                }
            }
        }

        private void PlaceComboBarrel()
        {
            var target = TargetSelector.GetTarget(barrel.Range, TargetSelector.DamageType.Physical, false);
            var myPos = Gangplank.Player.Position.To2D();
            var barrelInRange = GetBarrelInRange(myPos, barrel.Range);

            // check if we got any Barrel in Range ?!
            if (CountBarrelsInRange(myPos, barrel.Range) <= 0)
            {
                // no ? then get one :P
                AutomaticBarrel();
                return;
            }
            else
            {

                if (barrel.GetStacks - 1 == 1)
                {
                    SingleBarrelCombo(barrelInRange, target);
                }
                else if (barrel.GetStacks - 1 == 2)
                {
                    
                }
                else if (barrel.GetStacks - 1 == 3)
                {

                }
            }
            

        }

        private void SingleBarrelCombo(Barrel barrelInRange, Obj_AI_Base target)
        {
            var pred = barrel.GetPrediction(target, true);
            var distance = barrelInRange.Data.Position.Distance(pred.CastPosition);

            if (distance <= BarrelSpell.ConnectionRadius)
            {
                var cast = barrelInRange.Data.Position.Extend(pred.CastPosition, distance);
                barrel.CastSpell(cast.To2D());
            }
        }
        #endregion
    }

}
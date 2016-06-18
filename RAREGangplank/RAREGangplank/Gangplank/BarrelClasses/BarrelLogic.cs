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
using System.Diagnostics;
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

        internal static float QRange = 625;

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
        private Barrel GetBarrelInRange(Vector2 pos, float range)
        {
            try
            {
                return barrel.ActiveBarrels.FirstOrDefault(x => x.Data.Distance(pos) <= range);
            }
            catch (Exception)
            {
                return null;
            }
        }
            

        /// <summary>
        ///   Counts the barrels in range
        /// </summary>
        /// <param name="pos">from where</param>
        /// <param name="range"></param>
        /// <returns>count as int</returns>
        private int CountBarrelsInRange(Vector2 pos, float range)
        {
            try
            {
                return barrel.ActiveBarrels.Count(x => x.Data.Distance(pos) <= range);
            }
            catch (Exception)
            {
                return 0;
            }
            
        }
            

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
        internal void AutomaticBarrel()
        {
            var bushes = GetBushes(Gangplank.Player.Position);
            var minions = ObjectManager.Get<Obj_AI_Base>()
                    .Where(x => x.IsValidTarget() && x.IsMinion && x.Distance(Gangplank.Player.Position) < barrel.Range)
                    .ToList();
            var farmPos = barrel.GetBestLoc(minions);
            var targets = ObjectManager.Get<Obj_AI_Hero>().Where(x => x.IsValid && x.IsValidTarget(barrel.Range)).OrderByDescending(x => x.Distance(Gangplank.Player)).ToList();
            var target = TargetSelector.GetTarget(2000f, TargetSelector.DamageType.Physical);
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
                else if (bushes != null && target != null)
                {
                    try
                    {
                        var bush =
                        bushes.Single(
                            x => x.Position.Distance(Gangplank.Player.Position) > BarrelSpell.ExplosionRadius &&
                                 x.Position.Distance(Gangplank.Player.Position) < BarrelSpell.ExplosionRadius * 2);
                        if (bush != null)
                        {
                            barrel.Cast(bush.Position);
                        }
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine($"{DateTime.Now} => {e.Message}");
                    }
                    
                    
                }
            } 
            
        }

        /// <summary>
        ///   Logic for placing the barrel while farming
        /// </summary>
        /// <param name="orbMode"></param>
        internal void PlaceFarmBarrel()
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

        internal void PlaceComboBarrel()
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
                    DoubleBarrelCombo();
                }
                else if (barrel.GetStacks - 1 >= 3)
                {

                }
            }
            

        }

        private void DoubleBarrelCombo()
        {
            // double barrel combo :) 
            // get barrel in range ?!
            var barrelInRange =
                barrel.ActiveBarrels.Single(x => x.Data.Distance(Gangplank.Player.Position) <= barrel.Range);
            if (barrelInRange == null)
            {
                AutomaticBarrel();
                return;
            }

            if (barrelInRange.Data.Health <= 1 && !barrelInRange.Data.IsDead)
            {
                var target = TargetSelector.GetTarget(barrel.Range, TargetSelector.DamageType.Physical, false);
                var distance = barrelInRange.Data.Distance(target);

                if (target != null && barrelInRange.Data.Distance(Gangplank.Player) > QRange &&
                    distance < (BarrelSpell.ConnectionRadius*2 - BarrelSpell.ExplosionRadius))
                {
                    // between ~ 1000 to 660 range => ONE must be max ranged to the passive one to get to the 
                    // target properly
                    if (distance < BarrelSpell.ConnectionRadius*2 - BarrelSpell.ExplosionRadius &&
                        distance > BarrelSpell.ConnectionRadius)
                    {
                        var castpos = barrelInRange.Data.Position.Extend(target.Position, BarrelSpell.ConnectionRadius);
                        barrel.Cast(castpos);
                        Utility.DelayAction.Add((int)Math.Round(barrel.Cooldown*1000,0), () =>
                        {
                            var nearestBarrel = barrel.ActiveBarrels.OrderByDescending(x => x.Data.Distance(target)).FirstOrDefault();
                            if (nearestBarrel != null)
                            {
                                barrel.Cast(castpos.Extend(target.Position,
                                    castpos.Distance(nearestBarrel.Data.Position)));
                            }
                                
                        });
                    }
                    
                }
                else
                {
                    // need other barrel for casting our combo :S
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